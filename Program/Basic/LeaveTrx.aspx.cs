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

public partial class LeaveTrx : System.Web.UI.Page
{

    #region 共用參數
        
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM003";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    SysSetting SysSet = new SysSetting();
    int DLShowKind = 1;
    string DLDefItem = "";

    #endregion

    #region 資料設定

    string DataSouceName = "Leave_Trx";

    string[] DataKey = { 
        "Company", 
        "EmployeeId", 
        "LeaveType_Id", 
        "BeginDateTime" 
    };

    string[] DataName = {
        "Company",
        "EmployeeId",
        "LeaveType_Id",
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
        if (_UserInfo.SysSet.isTWCalendar)
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
    }
    
    
    //  身分驗證副程式
    private void AuthRight()
    {
        //驗證權限
        if ( _UserInfo.AuthLogin == false )
        {
            Response.Redirect ( "\\" + Application [ "WebSite" ].ToString ( ) + "\\" + Application [ "ErrorPage" ].ToString ( ) + "?ErrMsg=UnLogin" );
        }
        else
        {
            if ( _UserInfo.CheckPermission ( _ProgramId ) == false )
                Response.Redirect ( "\\" + Application [ "WebSite" ].ToString ( ) + "\\" + Application [ "ErrorPage" ].ToString ( ) + "?ErrMsg=NoRight" );
        }

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
        ScriptManager.RegisterStartupScript ( UpdatePanel1 , this.GetType ( ) , "" , @"JQ();" , true );
        lbl_Msg.Text = "";//清空訊息

        Navigator1.BindGridView = GridView1;

        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged ( SearchList1_SelectedChanged );
        
        if (!Page.IsPostBack)
        {
            #region 依人事2011/05/11建議調整,使可設定是否顯示代碼
            try
            {//設定下拉單是否顯示代碼或預設未選項
                DLShowKind = int.Parse(_UserInfo.SysSet.GetConfigString("DLShowKind"));
            }
            catch { }
            try
            {//設定未選項文字
                DLDefItem = _UserInfo.SysSet.GetConfigString("DLDefItem");
            }
            catch { }
            #endregion
            
            AuthRight ( );
            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            CL_LeaveType.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind + 3, "全部");
            CL_LeaveType.StyleAdd("width", "165px");
            BindData();
            showPanel();
        }
        else
        {
            if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit") || HiddenField1.Value == "UpData")
            {
                if (Request.Form["__EVENTTARGET"].ToString() == "" && Request.Form["__EVENTARGUMENT"].ToString() == "")
                {

                }
                else
                {
                    DataBind();
                }
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string temId = hid_IsInsertExit.Value.Replace("_", "$");
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

    void CL_LeaveType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0 && CL_LeaveType.SelectedCode != "")
            ((ASP.usercontrol_codelist_ascx)GridView1.FooterRow.Cells[3].FindControl("TB02")).SelectedCode = CL_LeaveType.SelectedCode;
    }
    // 搜尋模組連動
    void SearchList1_SelectedChanged ( object sender , SearchList.SelectEventArgs e )
    {
        if ( e.type == "Company" )
        {
            CL_LeaveType.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind + 3, "全部");
            CL_LeaveType.StyleAdd("width", "165px");
        }

        BindData ( );
        showPanel ( );
    }


    //  判斷搜尋時是否有此資料
    private bool hasData()
    {
        SetSsql();
        MyCmd.Parameters.Clear();
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql, MyCmd.Parameters, CommandType.Text);
        return tb.Rows.Count > 0 ? true : false;
    }
    
    //  綁定資料至Gridview
    private void BindData()
    {
        SetSsql();
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind();        
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();        
    }

    private void SetSsql ( )
    {
        Ssql = "SELECT LT.*,IsNull(LT.Deptid,PM.deptid) AS Depid FROM Leave_Trx LT left join Personnel_Master PM On LT.EmployeeId = PM.EmployeeId And LT.Company = PM.Company Where 0=0 ";
        //公司
        DDLSr ( "LT.Company" , SearchList1.CompanyValue );

        //部門
        DDLSr ( "PM.deptid " , SearchList1.DepartmentValue );

        //員工
        DDLSr ( "LT.EmployeeId" , SearchList1.EmployeeValue );

        if (CL_LeaveType.SelectedCode.Length > 0)
        {
            Ssql += string.Format(" And LeaveType_Id = '{0}'", CL_LeaveType.SelectedCode);
        }


        Ssql += " Order By Depid";
    }

    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr ( string Name , string Value )
    {
        if ( Value.Length > 0 )
            Ssql += string.Format ( " And " + Name + " like '%{0}%'" , Value );
        else
            Ssql += string.Format ( " And " + Name + " = '{0}'" , Value );
    }

    //  當按下搜尋按鈕時
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = string.Empty;
        BindData();
        hid_IsInsertExit.Value = "";

    }

    //新增
    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        if (hid_IsInsertExit.Value.Length > 1)
        {

            string temId = hid_IsInsertExit.Value.Replace("_" , "$") + "$TB0";

            if (Request.Form[temId + "1"] == "請選擇" || Request.Form[temId + "2$ddlCodeList"] == "" || Request.Form[temId + "1"] == null)
            {
                lbl_Msg.Text = "請先選擇項目";
                BindData();
                return;
            }
            else
            {
                string[] tmp ={
            SearchList1.CompanyValue, //Company 0
            Request.Form[temId + "1"].Remove(Request.Form[temId + "1"].IndexOf(" ")),//EmployeeId   1
            Request.Form[temId + "2$ddlCodeList"],//LeaveType_Id 2
            SysSet.FormatADDate(Request.Form[temId + "3"])+" "+Request.Form[temId + "4"],//BeginDateTime    3
            SysSet.FormatADDate(Request.Form[temId + "5"])+" "+Request.Form[temId + "6"],//EndDateTime  4
            Request.Form[temId + "7"],//hours  5
            Request.Form[temId + "8"],//days   6
            (int.Parse(Request.Form[temId + "9"])+1911) + Request.Form[temId + "10"].ToString()//Payroll_Processingmonth 7
            };

                if (
                    ValidateData
                    (
                    "Company" , tmp[0] ,
                    "EmployeeId" , tmp[1] ,
                    "LeaveType_Id" , tmp[2] ,
                    "BeginDateTime" , tmp[3]
                    )//#ValidateData
                    )//#if
                {

                    SDS_GridView.InsertParameters.Clear();
                    SDS_GridView.InsertParameters.Add("Company" , tmp[0]);
                    SDS_GridView.InsertParameters.Add("EmployeeId" , tmp[1]);
                    SDS_GridView.InsertParameters.Add("LeaveType_Id" , tmp[2]);
                    SDS_GridView.InsertParameters.Add("BeginDateTime" , tmp[3]);
                    SDS_GridView.InsertParameters.Add("EndDateTime" , tmp[4]);
                    SDS_GridView.InsertParameters.Add("hours" , tmp[5]);
                    SDS_GridView.InsertParameters.Add("days" , tmp[6]);
                    SDS_GridView.InsertParameters.Add("Payroll_Processingmonth" , tmp[7]);

                    WriteLog(true , "A" , 0);

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
                    WriteLog(false , "A" , i);

                }
                    BindData();
            }

            hid_IsInsertExit.Value = "";
        }
    }

    //判斷資料是否重覆
    private bool ValidateData(string Key1, string strID1, string Key2, string strID2, string Key3, string strID3, string Key4, string strID4)
    {
        Ssql = "Select Leave_Trx.* FROM Leave_Trx  Where " + Key1 + " = '" + strID1 +
     "' and " + Key2 + " ='" + strID2 +
     "' and " + Key3 + " ='" + strID3 +
     "' and " + Key4 + " = '" + strID4 + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆"; 
            return false;
        }
        return true;
    }

    //判斷日期是否有錯誤
    private bool CkDateRage(string StartTime,string EndTime,string AppTime)
    {
        if (int.Parse(StartTime) >  int.Parse(EndTime))
        {
            lbl_Msg.Text = "起始時間 不可晚於 結束結束";
            return false;
        }
        else
        {
             return true;
        }
    }

    //刪除按鈕
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string L3PK = btnDelete.Attributes["L3PK"].ToString();
        DateTime L4PK = new DateTime();
        L4PK = Convert.ToDateTime(btnDelete.Attributes["L4PK"].ToString()).AddYears(1911);

        string sql = "Delete FROM Leave_Trx  Where Company='" + L1PK +
            "' and EmployeeId='"+ L2PK +
            "' and LeaveType_Id='" + L3PK +
            "' and (convert(varchar,BeginDateTime,120) = '" + Convert.ToDateTime(L4PK).ToString("yyyy-MM-dd HH:mm:ss") + "')";

        WriteLog(true, "D", 0);

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        WriteLog(false, "D", result>0?1:0);

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

    //建表格
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
                e.Row.Cells[i - 1].Style.Add("width", "30px");
            }
            e.Row.Cells[i].Style.Add("text-align","right");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            e.Row.Visible = false;
        }
    }
    
    //更新區
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$TB0";
        //e.NewValues["LeaveType_Id"] = Request.Form[temId + "2$ddlCodeList"];
        e.NewValues["EndDateTime"] = SysSet.FormatADDate(e.NewValues["EndDateTime"].ToString()) + " " + Request.Form[temId + "4"];
        e.NewValues["Payroll_Processingmonth"] = (int.Parse(Request.Form[temId + "9"]) + 1911) + Request.Form[temId + "10"];


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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Leave_Trx";
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

    //#新增顯示區
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
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?'),SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'),SaveValue(" + HiddenField1.ClientID + ",'UpData')");
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
                #region @   設定欄位格式    @
                for (int i = 2; i < 9; i++)
                {
                   
                    
                    //文字對齊與儲存格大小
                    if (i < 5)
                    {
                        e.Row.Cells[i].Style.Add("width", "auto");
                        switch (i)
                        {
                            case 2:
                                //員工編號
                                try{
                                e.Row.Cells[i].Text = SearchList1.Employee.Items.FindByValue(e.Row.Cells[i].Text.Trim()).Text;
                                }
                                catch{}
                                break;
                            case 3:
                                //假別
                                //e.Row.Cells[i].Text = LeaveType_Id1.Items.FindByValue(e.Row.Cells[i].Text.Trim()).Text;
                                ASP.usercontrol_codelist_ascx CL = new ASP.usercontrol_codelist_ascx();
                                CL = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                                CL.ID = "TB02";
                                CL.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind);
                                CL.SelectedCode = e.Row.Cells[3].Text;
                                //e.Row.Cells[3].Controls.Add(CL);
                                e.Row.Cells[3].Text = CL.SelectedCodeName;
                                break;
                            case 4:
                                //起始日期
                                e.Row.Cells[i].Text = SysSet.ToTWDate(e.Row.Cells[i].Text) + " " + Convert.ToDateTime(e.Row.Cells[4].Text).ToString("HH:mm");
                                break;
                        }
                    }
                    else
                    {
                        ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("text-align", "right");
                        switch (i)
                        {
                            case 5:
                                
                                //終止日期
                                string tmptime = Convert.ToDateTime(((TextBox)e.Row.Cells[i].Controls[0]).Text).ToString("HH:mm");
                                ((TextBox)e.Row.Cells[i].Controls[0]).Text = SysSet.ToTWDate(((TextBox)e.Row.Cells[i].Controls[0]).Text);
                                ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("width", "50px");
                                //日曆元件
                                ImageButton btOpenCal = new ImageButton();
                                btOpenCal.ID = "btnCalendar";
                                btOpenCal.SkinID = "Calendar1";
                                btOpenCal.OnClientClick = "return GetPromptDate(" + ((TextBox)e.Row.Cells[i].Controls[0]).ClientID + ");";
                                e.Row.Cells[i].Controls.Add(btOpenCal);
                                //時間選單
                                DropDownList DDL = new DropDownList();
                                DDL.ID = "TB04";
                                for (int n = 0; n < 24; n++)
                                {
                                    string tmpHM = n.ToString("D2") + ":" + "00";
                                    DDL.Items.Add(tmpHM);
                                    tmpHM = n.ToString("D2") + ":" + "30";
                                    DDL.Items.Add(tmpHM);
                                }
                                DDL.SelectedValue = tmptime;
                                e.Row.Cells[i].Controls.Add(DDL);
                                strValue += "checkColumns(" + DDL.ClientID + ") && ";
                                break;
                            case 6:
                            case 7:

                                ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("width", "20px");
                                break;
                            case 8:
                               
                                ((TextBox)e.Row.Cells[i].Controls[0]).Visible = false;
                                //計薪月份
                                //年下拉式選單
                                DropDownList DDL1 = new DropDownList();
                                DDL1.ID = "TB09";
                                for (int n = DateTime.Today.Year - 1911 - 5; n < DateTime.Today.Year - 1911 + 5; n++)
                                {
                                    string tmpHM = n.ToString("D2");
                                    DDL1.Items.Add(tmpHM);
                                }
                                int tmp = (int.Parse(((TextBox)e.Row.Cells[i].Controls[0]).Text.Substring(0, 4)) - 1911);
                                DDL1.SelectedValue = tmp.ToString();
                                DDL1.Width = 50;
                                e.Row.Cells[i].Controls.Add(DDL1);
                                strValue += "checkColumns(" + DDL1.ClientID + ") && ";
                                //月下拉式選單
                                DropDownList DDL2 = new DropDownList();
                                DDL2.ID = "TB010";
                                for (int n = 1; n < 13; n++)
                                {
                                    string tmpHM = n.ToString("D2");
                                    DDL2.Items.Add(tmpHM);
                                }

                                DDL2.SelectedValue = ((TextBox)e.Row.Cells[i].Controls[0]).Text.Substring(4,2);
                                DDL2.Width = 40;
                                e.Row.Cells[i].Controls.Add(DDL2);
                                strValue += "checkColumns(" + DDL2.ClientID + ") && ";
                                break;
                        }
                    }
                }
                
                #endregion

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
                //員工編號
                try
                {
                    e.Row.Cells[ 2 ].Text = SearchList1.Employee.Items.FindByValue ( e.Row.Cells[ 2 ].Text.Trim ( ) ).Text;
                }
                catch {                }
                //假別
                //e.Row.Cells[3].Text = LeaveType_Id1.Items.FindByValue(e.Row.Cells[3].Text.Trim()).Text;
                ASP.usercontrol_codelist_ascx CL = new ASP.usercontrol_codelist_ascx();
                CL = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                CL.ID = "TB02";
                CL.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind);
                CL.SelectedCode = e.Row.Cells[3].Text;
                e.Row.Cells[3].Text = CL.SelectedCodeName;

                //起始日期
                e.Row.Cells[4].Text = SysSet.ToTWDate(e.Row.Cells[4].Text) + " " + Convert.ToDateTime(e.Row.Cells[4].Text).ToString("HH:mm");
                //終止日期
                e.Row.Cells[5].Text = SysSet.ToTWDate(e.Row.Cells[5].Text) + " " + Convert.ToDateTime(e.Row.Cells[5].Text).ToString("HH:mm");
                //時數
                //e.Row.Cells[6].Text
                //天數
                e.Row.Cells[7].Text += " 天";
                //計薪年月
                e.Row.Cells[8].Text = (int.Parse(e.Row.Cells[8].Text.Substring(0, 4))-1911)+"/"+ e.Row.Cells[8].Text.Substring(4,2);

                #endregion
            }

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                TextBox TB = new TextBox();
                TB.Style.Add("text-align", "right");
                DropDownList DDL = new DropDownList();
                ImageButton btOpenCal = new ImageButton();
                switch (i)
                {
                    case 2://員工編號
                        DDL.ID = "TB01";
                        DDL.DataSource = SearchList1.Employee.Items;
                        DDL.DataBind();
                        DDL.SelectedIndex = SearchList1.Employee.SelectedIndex;
                        if (DDL.Items.Count > 0) DDL.Items.RemoveAt(0);
                        DDL.Width = 112;
                        e.Row.Cells[i].Controls.Add(DDL);
                        strValue += "checkColumns(" + DDL.ClientID + ") && ";
                        break;
                    case 3://假別                        
                        ASP.usercontrol_codelist_ascx CL = new ASP.usercontrol_codelist_ascx();
                        CL = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                        CL.ID = "TB02";
                        CL.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind);
                        CL.SelectedCode = CL_LeaveType.SelectedCode;
                        e.Row.Cells[i].Controls.Add(CL);
                        strValue += "checkColumns(" + CL.CLClientID() + ") && ";
                        break;
                    case 4://起始時間
                        TB.ID = "TB03";
                        TB.Width = 60;
                        TB.CssClass = "JQCalendar";
                        e.Row.Cells[i].Controls.Add(TB);
                        //時間選單
                        DDL.ID = "TB04";
                        for (int n = 0; n < 24; n++)
                        {
                            string tmpHM = n.ToString("D2") + ":" + "00";
                            DDL.Items.Add(tmpHM);
                            tmpHM = n.ToString("D2") + ":" + "30";
                            DDL.Items.Add(tmpHM);
                        }
                        DDL.SelectedValue = "12:00";
                        e.Row.Cells[i].Controls.Add(DDL);
                        strValue += "checkColumns(" + DDL.ClientID + ") && ";
                        break;
                    case 5://終止時間
                        TB.ID = "TB05";
                        TB.Width = 60;;
                        TB.CssClass = "JQCalendar";
                        e.Row.Cells[i].Controls.Add(TB);
                      
                        //時間選單
                        DDL.ID = "TB06";
                        for (int n = 0; n < 24; n++)
                        {
                            string tmpHM = n.ToString("D2") + ":" + "00";
                            DDL.Items.Add(tmpHM);
                            tmpHM = n.ToString("D2") + ":" + "30";
                            DDL.Items.Add(tmpHM);
                        }
                        DDL.SelectedValue = "12:00";
                        e.Row.Cells[i].Controls.Add(DDL);
                        strValue += "checkColumns(" + DDL.ClientID + ") && ";
                        break;
                    case 6://時數
                        TB.ID = "TB07";
                        TB.Width = 20;

                        e.Row.Cells[i].Controls.Add(TB);
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        break;
                    case 7://天數
                        TB.ID = "TB08";
                        TB.Width = 20;
                        e.Row.Cells[i].Controls.Add(TB);
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        break;
                    case 8://計薪月份
                        //年下拉式選單
                        DropDownList DDL1 = new DropDownList();
                        DDL1.ID = "TB09";
                        for (int n = DateTime.Today.Year - 1911 - 5; n < DateTime.Today.Year - 1911 + 5; n++)
                        {
                            string tmpHM = n.ToString("D2");
                            DDL1.Items.Add(tmpHM);
                        }
                        int tmp = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                        DDL1.SelectedValue = tmp.ToString();
                        DDL1.Width = 50;
                        e.Row.Cells[i].Controls.Add(DDL1);
                        strValue += "checkColumns(" + DDL1.ClientID + ") && ";
                        //月下拉式選單
                        DropDownList DDL2 = new DropDownList();
                        DDL2.ID = "TB010";
                        for (int n = 1; n < 13; n++)
                        {
                            string tmpHM = n.ToString("D2");
                            DDL2.Items.Add(tmpHM);
                        }

                        DDL2.SelectedValue = DateTime.Now.Month.ToString("D2");
                        DDL2.Width = 40;
                        e.Row.Cells[i].Controls.Add(DDL2);
                        strValue += "checkColumns(" + DDL2.ClientID + ") && ";
                        break;
                    default://其他
                        break;
                }

            }
            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[1].Controls.Add(btAddNew); 
             
            #endregion

        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位
            strValue = "";
            for (int i = 1; i < 8; i++)
            {
                TextBox TB = new TextBox();
                TB.Style.Add("text-align", "right");
                DropDownList DDL = new DropDownList();
                DropDownList DDL2 = new DropDownList();
                ImageButton btOpenCal = new ImageButton();

                switch (i)
                {
                    case 1://員工編號
                        DDL = (DropDownList)e.Row.FindControl("TB01");
                        DDL.DataSource = SearchList1.Employee.Items;
                        DDL.DataBind();
                        DDL.SelectedIndex = SearchList1.Employee.SelectedIndex;
                        if (DDL.Items.Count > 0) DDL.Items.RemoveAt(0);
                        DDL.Width = 120;
                        strValue += "checkColumns(" + DDL.ClientID + ") && ";
                        break;
                    case 2://假別
                        ASP.usercontrol_codelist_ascx CL = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("TB02");
                        CL.SetDTList("LeaveType_Basic", "Leave_ID", "Leave_Desc", "Company='" + SearchList1.CompanyValue.Replace("%", "") + "'", DLShowKind);
                        CL.SelectedCode = CL_LeaveType.SelectedCode;
                        strValue += "checkColumns(" + CL.CLClientID() + ") && ";
                        break;
                    case 3://起始日期
                        TB = (TextBox)e.Row.FindControl("TB0" + i.ToString());
                        TB.Width = 60;
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        //下拉式選單
                        DDL2 = (DropDownList)e.Row.FindControl("TB04");
                        for (int n = 0; n < 24; n++)
                        {
                            string tmpHM = n.ToString("D2") + ":" + "00";
                            DDL2.Items.Add(tmpHM);
                            tmpHM = n.ToString("D2") + ":" + "30";
                            DDL2.Items.Add(tmpHM);
                        }
                        DDL2.SelectedValue = "12:00";
                        DDL2.Width = 60;
                        strValue += "checkColumns(" + DDL2.ClientID + ") && ";
                        break;
                    case 4://終止日期
                        TB = (TextBox)e.Row.FindControl("TB05");
                        TB.Width = 60;
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        //下拉式選單
                        DDL2 = (DropDownList)e.Row.FindControl("TB06");
                        for (int n = 0; n < 24; n++)
                        {
                            string tmpHM = n.ToString("D2") + ":" + "00";
                            DDL2.Items.Add(tmpHM);
                            tmpHM = n.ToString("D2") + ":" + "30";
                            DDL2.Items.Add(tmpHM);
                        }
                        DDL2.SelectedValue = "12:00";
                        DDL2.Width = 64;
                        strValue += "checkColumns(" + DDL2.ClientID + ") && ";
                        break;
                    case 5://時數
                        TB = (TextBox)e.Row.FindControl("TB07");
                        TB.Width = 20;
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        break;
                    case 6://天數
                        TB = (TextBox)e.Row.FindControl("TB08");
                        TB.Width = 20;
                        strValue += "checkColumns(" + TB.ClientID + ") && ";
                        break;
                    case 7://計薪年月
                        //年下拉式選單
                        DropDownList DD101 = new DropDownList();
                        DD101 = (DropDownList)e.Row.FindControl("TB09");
                        for (int n = DateTime.Today.Year - 1911 - 5; n < DateTime.Today.Year - 1911 + 5; n++)
                        {
                            string tmpHM = n.ToString("D2");
                            DD101.Items.Add(tmpHM);
                        }
                        int tmp = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                        DD101.SelectedValue = tmp.ToString();
                        strValue += "checkColumns(" + DD101.ClientID + ") && ";
                        //月下拉式選單
                        DropDownList DD102 = new DropDownList();
                        DD102 = (DropDownList)e.Row.FindControl("TB010");
                        for (int n = 1; n < 13; n++)
                        {
                            string tmpHM = n.ToString("D2");
                            DD102.Items.Add(tmpHM);
                        }

                        DD102.SelectedValue = DateTime.Now.Month.ToString("D2");
                        strValue += "checkColumns(" + DD102.ClientID + ") && ";
                        break;
                    default:

                        break;
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        } 
    }

    //  控制頁面 Panel，查無資料時顯示用
    private void showPanel()
    {

        if (hasData())
        {
            Panel_Empty.Visible = false;
        }
        else
        {
            Panel_Empty.Visible = true;
        }
    }
    
    //  當按下搜尋時呼叫此區
    protected void Navigator1_Load(object sender, EventArgs e)
    {

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
                        break;
                    case "Update":
                        break;
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
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$TB0";
        e.Keys["LeaveType_Id"] = Request.Form[temId + "2$ddlCodeList"];
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void SDS_GridView_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

    private void WriteLog(bool n, string c, int i)
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Leave_Trx";
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
}//  程式尾部
