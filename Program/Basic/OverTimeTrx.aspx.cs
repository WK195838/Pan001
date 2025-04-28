using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class OverTimeTrx : System.Web.UI.Page
{

    #region 共用參數
        
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM002";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    SysSetting SysSet = new SysSetting();

    #endregion

    #region 資料設定

    string[] DataKey = { 
        "Company", 
        "EmployeeId", 
        "OverTime_Date", 
        "BeginDateTime" 
    };

    string[] DataName = {
        "Company",
        "EmployeeId",
        "OverTime_Date",
        "BeginDateTime",
        "EndDateTime",
        "hours",
        "days",
        "ApproveDate",
        "Payroll_Processingmonth"
    };

    #endregion

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/OverTimeTrx.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }
  
    //  身分驗證副程式
    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    GridView1.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    GridView1.ShowFooter = Find;
                }
            }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            {
                Find = true;
            }
            else
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            //版面樣式調整
            if (SetCss == false)
            {
                GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    //  載入網頁時執行此區
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        lbl_Msg.Text = "";//清空訊息
        Navigator1.BindGridView = GridView1;

        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged ( SearchList1_SelectedChanged );
        txtOverTime_Date.CssClass = "JQCalendar";
        //btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + txtOverTime_Date.ClientID + ");");
     
        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit") || HiddenField1.Value == "UpData")
            {
                if (Request.Form["__EVENTTARGET"].ToString() == "" && Request.Form["__EVENTARGUMENT"].ToString() == "")
                {
                    HiddenField1.Value = "";
                }
                else
                {
                    DataBind();
                }
            }
            else if (hid_IsInsertExit.Value.Length > 1)
            {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
            }
            else
            {
                    BindData();
                    showPanel();
            }
        }
  
    }
    // 搜尋模組連動
    void SearchList1_SelectedChanged ( object sender , SearchList.SelectEventArgs e )
    {
        BindData ( );
        showPanel ( );
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        BindData();
        showPanel();
    }

    //  判斷搜尋時是否有此資料
    private bool hasData()
    {
        SetSsql ( );
        MyCmd.Parameters.Clear();
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql, MyCmd.Parameters, CommandType.Text);
        return tb.Rows.Count > 0 ? true : false;
    }
    
    //  綁定資料至Gridview
    private void BindData()
    {
        SetSsql ( );
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind();        
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();        
    }

    private void SetSsql ( )
    {
        Ssql = "SELECT OTT.*,IsNull(OTT.Deptid,PM.deptid) AS Depid FROM OverTime_Trx OTT left join Personnel_Master PM On OTT.EmployeeId = PM.EmployeeId And OTT.Company = PM.Company Where 0=0 ";
        if (!Page.IsPostBack)
        {
            //Ssql += string.Format(" And Payroll_ProcessingMonth = '{0}'", DateTime.Now.ToString("yyyyMM"));
        }
        //公司
        DDLSr ( "OTT.Company" , SearchList1.CompanyValue );

        //部門
        DDLSr ( "PM.deptid " , SearchList1.DepartmentValue );

        //員工
        DDLSr ( "OTT.EmployeeId" , SearchList1.EmployeeValue );
        
        //日期
        if ( txtOverTime_Date.Text.Length > 0 )
            Ssql += string.Format ( " And Convert(varchar,OTT.OverTime_Date,111) like '%{0}%'" , SysSet.FormatADDate ( txtOverTime_Date.Text ) );


        Ssql += " Order By Depid,ApproveDate desc";
    }

    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr ( string Name , string Value )
    {
        if ( Value.Length > 0 )
            Ssql += string.Format ( " And " + Name + " like '%{0}%'" , Value );
        else
            Ssql += string.Format ( " And " + Name + " = '{0}'" , Value );
    }

    //  轉換年小程式
    private string Year(string str)
    {
        return ((int)(int.Parse(str) + 1911)).ToString();
    }

    #region #新增設定區#
    //  新增按鈕
    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        if (hid_IsInsertExit.Value.Length > 0)
        {
            string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";

            //Company
            string tmpCompany = SearchList1.CompanyValue;
            //EmployeeId
            string tmpEmployeeId = Request.Form[temId + "0"].Remove(Request.Form[temId + "0"].IndexOf(" "));
            //Payroll_ProcessingMonth
            string tmpPayroll_ProcessingMonth = Year(Request.Form[temId + "6"]) + Request.Form[temId + "7"];

            #region 2011/05/16 kaya 加入檢核
            bool blVerify = false;
            try
            {
                decimal[] theHH = { decimal.Parse(Request.Form[temId + "2"].ToString())
                                      , decimal.Parse(Request.Form[temId + "3"].ToString())
                                      , decimal.Parse(Request.Form[temId + "4"].ToString())
                                      , decimal.Parse(Request.Form[temId + "5"].ToString()) };

                if ((theHH[0] < 0) || (theHH[1] < 0) || (theHH[2] < 0) || (theHH[3] < 0)
                    || ((theHH[0] + theHH[1] + theHH[2] + theHH[3] == 0))
                    )
                {
                    lbl_Msg.Text = "新增失敗!! 加班時數不可為負且總時數不可小於等於 0 !";
                }
                else
                {
                    blVerify = true;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = "新增失敗!! 加班時數輸入有誤! 時數不可為負且總時數不可小於等於 0 ! ";
            }
            #endregion

            if (
                blVerify != false &&
                ValidateData("Company", tmpCompany,
                             "EmployeeId", tmpEmployeeId,
                             "OverTime_Date", Request.Form[temId + "1"].ToString()
                            )                 
                )
            {

                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Company", tmpCompany);
                SDS_GridView.InsertParameters.Add("EmployeeId", tmpEmployeeId);

                string strDate = Request.Form[temId + "1"].ToString();
                try
                {
                    strDate = Convert.ToDateTime(strDate).AddYears(1911).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                    try
                    {
                        string[] strDateList = Request.Form[temId + "1"].ToString().Split('/');
                        strDate = (Convert.ToInt32(strDateList[0]) + 1911).ToString() + "/" + strDateList[1] + "/" + strDateList[2];
                        strDate = Convert.ToDateTime(strDate).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                    }
                }
                SDS_GridView.InsertParameters.Add("OverTime_Date", strDate);
                SDS_GridView.InsertParameters.Add("OverTime1", Request.Form[temId + "2"].ToString());
                SDS_GridView.InsertParameters.Add("OverTime2", Request.Form[temId + "3"].ToString());
                SDS_GridView.InsertParameters.Add("Holiday", Request.Form[temId + "4"].ToString());
                SDS_GridView.InsertParameters.Add("NationalHoliday", Request.Form[temId + "5"].ToString());
                SDS_GridView.InsertParameters.Add("Payroll_ProcessingMonth", tmpPayroll_ProcessingMonth);

                WriteLog(true, "A", 0);

                int i = 0;
                try
                {
                    i = SDS_GridView.Insert();
                }
                catch (Exception ex)
                {
                    lbl_Msg.Text = ex.Message;
                }
                if (i == 1)
                {
                    lbl_Msg.Text = i.ToString() + " 個資料列 " + "新增成功!!";
                }
                else
                {
                    lbl_Msg.Text = "新增失敗!!" + lbl_Msg.Text;
                }
                WriteLog(false, "A", i);
            }

            BindData();
            hid_IsInsertExit.Value = "";
        }
    }

    //  判斷資料是否重覆
    private bool ValidateData(string Key1, string strID1, string Key2, string strID2, string Key3, string strID3)
    {
        try
        {
            strID3 = Convert.ToDateTime(strID3).AddYears(1911).ToString("yyyy/MM/dd");
        }
        catch {
            try
            {
                string[] strDate = strID3.Split('/');
                strID3 = (Convert.ToInt32(strDate[0]) + 1911).ToString() + "/" + strDate[1] + "/" + strDate[2];
                strID3 = Convert.ToDateTime(strID3).ToString("yyyy/MM/dd");
            }
            catch
            {
                lbl_Msg.Text = "新增失敗!!  原因: 日期(" + strID3 + ")轉換有誤 ";
                return false;
            }
        }
        Ssql = "Select OverTime_Trx.* FROM OverTime_Trx  Where " + Key1 + " = '" + strID1 +
     "' and " + Key2 + " ='" + strID2 +
     "' and (Convert(varchar," + Key3 + ",111) ='" + strID3 + "')";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            return false;
        }
        return true;
    }
    #endregion

    #region #刪除設定區#

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        DateTime L3PK = new DateTime();
        L3PK = Convert.ToDateTime(btnDelete.Attributes["L3PK"].ToString());

        string sql = "Delete FROM OverTime_Trx  Where Company='" + L1PK +
            "' and EmployeeId='" + L2PK +
            "' and (convert(varchar,OverTime_Date,120) = '" + Convert.ToDateTime(L3PK).ToString("yyyy-MM-dd HH:mm:ss") + "')";

        WriteLog(true, "D", 0);

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        WriteLog(false, "D", result > 0 ? 1 : 0);

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";

        }
        BindData();
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";
            BindData();
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }
        showPanel();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //
    }
    #endregion

    #region #更新設定區#
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string DDId = hid_IsInsertExit.Value.Replace("_", "$") + "$UDD";
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$ctl0";
        #region 2011/05/16 kaya 加入檢核
        bool blVerify = false;
        try
        {
            for (int i = 0; i < 5; i++)
            {
                if (e.NewValues[i] == null)
                    e.NewValues[i] = "0";
            }

            decimal[] theHH = { decimal.Parse(e.NewValues[0].ToString()), decimal.Parse(e.NewValues[1].ToString()), decimal.Parse(e.NewValues[2].ToString()), decimal.Parse(e.NewValues[3].ToString()) };

            if ((theHH[0] < 0) || (theHH[1] < 0) || (theHH[2] < 0) || (theHH[3] < 0)
                || ((theHH[0] + theHH[1] + theHH[2] + theHH[3] == 0))
                )
            {
                lbl_Msg.Text = "更新失敗!! 加班時數不可為負且總時數不可小於等於 0 !";
            }
            else
            {
                blVerify = true;
            }
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = "更新失敗!! 加班時數輸入有誤! 時數不可為負且總時數不可小於等於 0 ! ";
        }
        if (blVerify != true)
        {
            e.Cancel = true;            
            GridView1.EditIndex = -1;
            BindData();
            hid_IsInsertExit.Value = "";
        }        
        #endregion
        e.Keys.Add("Payroll_ProcessingMonth", Year(Request.Form[DDId + "4"].ToString()) + Request.Form[DDId + "5"].ToString());
        #region 開始異動前,先寫入LOG
        //TableName	異動資料表	varchar	60
        //TrxType	異動類別(A/U/D)	char	1
        //ChangItem	異動項目(欄位)	varchar	255
        //SQLcommand	異動項目(異動值/指令)	varchar	2000
        //ChgStartDateTime	異動開始時間	smalldatetime	
        //ChgStopDateTime	異動結束時間	smalldatetime	
        //ChgUser	使用者代號	nvarchar	32
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "OverTimeTrx";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

    }

    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        if (e.Exception == null)
        {
            GridView1.EditIndex = -1;
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "更新成功!!";
            BindData();
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

    }
    #endregion


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID  + ",'" + e.Row.ClientID +"'),SaveValue("+HiddenField1.ClientID+",'UpData')");
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                    
                }
                //取消
                if (e.Row.Cells[1].Controls[2] != null)
                {
                    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                #region @   設定對齊位置    @
                for (int i = 2; i < 8; i++)
                {
                    if (i < 4)
                        e.Row.Cells[i].Style.Add("text-align", "center");
                    else
                    {
                        ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("text-align", "right");
                        ((TextBox)e.Row.Cells[i].Controls[0]).Width = 80;
                    }
                }
                #endregion
                //  員工編號
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Trim() + "&nbsp;-&nbsp;" + DBSetting.PersonalName(SearchList1.CompanyValue.Trim(), e.Row.Cells[2].Text.Trim());                
                //  加班日期
                e.Row.Cells[3].Text = SysSet.FormatDate(e.Row.Cells[3].Text);
                ((TextBox)e.Row.Cells[8].Controls[0]).Visible = false;
                DropDownList UDD4 = new DropDownList();
                UDD4.ID = "UDD4";
                for (int n = DateTime.Today.Year - 1911 - 5; n < DateTime.Today.Year - 1911 + 5; n++)
                {
                    string tmpHM = n.ToString("D2");
                    UDD4.Items.Add(tmpHM);
                }
                int tmp = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                strValue = ((TextBox)e.Row.Cells[8].Controls[0]).Text;
                if (strValue.Replace("&nbsp;", "").Length < 6)
                {
                    strValue = DateTime.Now.ToString("yyyyMM");
                }

                UDD4.SelectedValue = ((int)(int.Parse(strValue.Substring(0, 4)) - 1911)).ToString();
                UDD4.Width = 50;
                e.Row.Cells[8].Controls.Add(UDD4);
                //月下拉式選單
                DropDownList UDD5 = new DropDownList();
                UDD5.ID = "UDD5";
                for (int n = 1; n < 13; n++)
                {
                    string tmpHM = n.ToString("D2");
                    UDD5.Items.Add(tmpHM);
                }

                UDD5.SelectedValue = strValue.Substring(4);
                UDD5.Width = 40;
                e.Row.Cells[8].Controls.Add(UDD5);
                #endregion               
            }
            else
            {
                #region 查詢用
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                //  員工編號
                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Trim() + "&nbsp;-&nbsp;" + DBSetting.PersonalName(SearchList1.CompanyValue.Trim(), e.Row.Cells[2].Text.Trim());
                
                //  加班日期
                e.Row.Cells[3].Text = SysSet.FormatDate(e.Row.Cells[3].Text);
                //  計薪月份
                if (e.Row.Cells[8].Text.Replace("&nbsp;", "").Length < 6)
                {
                    e.Row.Cells[8].Text = "尚未指定";                    
                }
                else
                {                    
                    if (DataBinder.Eval(e.Row.DataItem, "Completed") != null)
                    {//判斷是否已計薪
                        strValue = DataBinder.Eval(e.Row.DataItem, "Completed").ToString();
                        if (strValue.ToUpper().Equals("Y"))
                        {
                            e.Row.Cells[0].Text = "";
                            e.Row.Cells[1].Text = "";
                        }
                    }
                    try
                    {
                        e.Row.Cells[8].Text = ((int)(int.Parse(e.Row.Cells[8].Text.Substring(0, 4)) - 1911)).ToString() + "年" + e.Row.Cells[8].Text.Substring(4) + "月";
                    }
                    catch { }
                }
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";
            //for (int i = 2; i < e.Row.Cells.Count; i++)
            //{
            //    TextBox tbAddNew = new TextBox();
            //    DropDownList DDL = new DropDownList();
            //    switch(i)
            //    {
            //        case 2:
            //            DDL.ID = "tbAddNew00";
            //            DDL.DataSource = SearchList1.Employee.Items;
            //            DDL.DataBind();
            //            DDL.SelectedIndex = SearchList1.Employee.SelectedIndex;
            //            if (DDL.Items.Count > 0) DDL.Items.RemoveAt(0);
            //            DDL.Width = 120;
            //            e.Row.Cells[i].Controls.Add(DDL);
            //            strValue += "checkColumns(" + DDL.ClientID + ") && ";
            //            break;
            //        case 3:
            //        case 4:
            //        case 5:
            //        case 6:
            //        case 7:
            //            tbAddNew.ID = "tbAddNew0" + ((int)(i - 2)).ToString();
            //            tbAddNew.Width = 60;
            //            tbAddNew.Style.Add("text-align", "right");
            //            e.Row.Cells[i].Controls.Add(tbAddNew);
            //            //2011/05/09 kaya 依人事建議預設時數
            //            strValue += "setDefvalue(" + tbAddNew.ClientID + ",'0') && ";                        
            //            break;
            //        case 8://薪資處理月份
            //            //年下拉式選單
            //            DropDownList DD101 = new DropDownList();
            //            DD101.ID = "tbAddNew06";
            //            for (int n = DateTime.Today.Year - 1911-5; n < DateTime.Today.Year - 1911 + 5; n++)
            //            {
            //                string tmpHM = n.ToString("D2");
            //                DD101.Items.Add(tmpHM);
            //            }
            //            int tmp = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //            DD101.SelectedValue = tmp.ToString();
            //            DD101.Width = 50;
            //            e.Row.Cells[i].Controls.Add(DD101);
            //            strValue += "checkColumns(" + DD101.ClientID + ") && ";
            //            //月下拉式選單
            //            DropDownList DD102 = new DropDownList();
            //            DD102.ID = "tbAddNew07";
            //            for (int n = 1; n < 13; n++)
            //            {
            //                string tmpHM = n.ToString("D2");
            //                DD102.Items.Add(tmpHM);
            //            }

            //            DD102.SelectedValue = DateTime.Now.Month.ToString("D2");
            //            DD102.Width = 40;
            //            e.Row.Cells[i].Controls.Add(DD102);
            //            strValue += "checkColumns(" + DD102.ClientID + ") && ";
            //            break;

            //        default:
            //            break;
            //    }
            //    if (i == 3)
            //    {                    
            //        //日曆元件
            //        tbAddNew.CssClass = "JQCalendar";
            //        //ImageButton btOpenCal = new ImageButton();
            //        //btOpenCal.ID = "btnCalendar";
            //        //btOpenCal.SkinID = "Calendar1";
            //        //btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
            //        //e.Row.Cells[i].Controls.Add(btOpenCal);
            //    }
            //}
            //因應2017年政府要求一例一休法令，故將原本之新增功能隱藏
            //ImageButton btAddNew = new ImageButton();
            //btAddNew.ID = "btAddNew";
            //btAddNew.SkinID = "NewAdd";
            //btAddNew.CommandName = "Insert";
            //btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            //e.Row.Cells[1].Controls.Add(btAddNew); 

            #endregion

        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";
            //for (int i = 1; i < 8; i++)
            //{
            //    TextBox tbAddNew = new TextBox();
            //    switch (i)
            //    {
            //        case 1:
            //            DropDownList DD3 = new DropDownList();
            //            DD3 = (DropDownList)e.Row.FindControl("tbAddNew00");
            //            DD3.DataSource = SearchList1.Employee.Items;
            //            DD3.DataBind();
            //            DD3.SelectedIndex = SearchList1.Employee.SelectedIndex;
            //            if (DD3.Items.Count > 0) DD3.Items.RemoveAt(0);
            //            DD3.Width = 120;
            //            break;
            //        case 2:
                        
            //            tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0"+((int)(i-1)).ToString());
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Width = 60;
            //            strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
            //            //日曆元件
            //            tbAddNew.CssClass = "JQCalendar";
            //            //ImageButton btOpenCal = (ImageButton)e.Row.FindControl("btnCalendar1");
            //            //if (btOpenCal != null)
            //            //    btOpenCal.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
            //            break;
            //        case 3:
            //        case 4:
            //        case 5:
            //        case 6:
            //            tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + ((int)(i - 1)).ToString());
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Width = 60;
            //            //2011/05/09 kaya 依人事建議預設時數
            //            strValue += "setDefvalue(" + tbAddNew.ClientID + ",'0') && ";
            //            break;
            //        case 7:
            //            //年下拉式選單
            //            DropDownList DD101 = new DropDownList();
            //            DD101 = (DropDownList)e.Row.FindControl("tbAddNew06");
            //            for (int n = DateTime.Today.Year - 1911 - 5; n < DateTime.Today.Year - 1911 + 5; n++)
            //            {
            //                string tmpHM = n.ToString("D2");
            //                DD101.Items.Add(tmpHM);
            //            }
            //            int tmp = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //            DD101.SelectedValue = tmp.ToString();
            //            strValue += "checkColumns(" + DD101.ClientID + ") && ";
            //            //月下拉式選單
            //            DropDownList DD102 = new DropDownList();
            //            DD102 = (DropDownList)e.Row.FindControl("tbAddNew07");
            //            for (int n = 1; n < 13; n++)
            //            {
            //                string tmpHM = n.ToString("D2");
            //                DD102.Items.Add(tmpHM);
            //            }

            //            DD102.SelectedValue = DateTime.Now.Month.ToString("D2");
            //            strValue += "checkColumns(" + DD102.ClientID + ") && ";
            //            break;
            //        default:

            //            break;
            //    }
            //}

            //因應2017年政府要求一例一休法令，故將原本之新增功能隱藏
            //ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            //if (btnNew != null)
            //    btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion

        } 
    }
  
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";

        }
        else if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer) || (e.Row.RowType == DataControlRowType.EmptyDataRow))
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            int i = 0;
            for (i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
            }

            i = e.Row.Cells.Count - 1;
            if (i > 0)
            {
                e.Row.Cells[i - 1].Style.Add("text-align", "right");
                e.Row.Cells[i - 1].Style.Add("width", "70px");
            }
            e.Row.Cells[i].Style.Add("text-align", "right");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            e.Row.Visible = false;
        }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindData();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandName))
        {
            try
            {
                //GridViewRow gvRow = null;
                switch (e.CommandName)
                {
                    case "Delete"://刪除
                    case "Update":
                    case "Insert":
                        break;
                    case "Edit":
                    case "Cancel":
                    default:
                        lbl_Msg.Text = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    private void WriteLog(bool n,string c,int i) 
    {
        if (n)
        {
            #region 開始異動前,先寫入LOG
            //TableName	異動資料表	varchar	60
            //TrxType	異動類別(A/U/D)	char	1
            //ChangItem	異動項目(欄位)	varchar	255
            //SQLcommand	異動項目(異動值/指令)	varchar	2000
            //ChgStartDateTime	異動開始時間	smalldatetime	
            //ChgStopDateTime	異動結束時間	smalldatetime	
            //ChgUser	使用者代號	nvarchar	32
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "OverTime_Trx";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = c;
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
        else
        {
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
    }

    //  控制頁面 Panel，查無資料時顯示用
    private void showPanel ( ) { Panel_Empty.Visible = hasData ( ) ? false : true; }
    
}//  程式尾部
