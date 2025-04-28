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

public partial class AnnualLeaveSettlement : System.Web.UI.Page
{

    #region 共用參數
        
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM019";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    SysSetting SysSet = new SysSetting();

    #endregion

    #region 資料設定

    string[] DataKey = { 
        "Company", 
        "EmployeeId", 
        "ALYear"
    };

    string[] DataName = {
        "Company",
        "EmployeeId",
        "ALYear",
        "ALDays",
        "LeaveDays",
        "LYTransDays",
        "ConvertibleDays",        
        "TransOrNot"
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/AnnualLeaveSettlement.aspx'");
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
        //為日期欄位增加小日曆元件
        txtALYear.CssClass = "JQCalendar";
        //btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + txtALYear.ClientID + ");");
     
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
            qyYearList.SetYearList(_UserInfo.SysSet.YearB, _UserInfo.SysSet.YearE, "");
            CL_ALTrans.SetCodeList("PY#ALTrans", 5);
            SetResignCode();
            SetInit();
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            SetInit();
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

    protected void SetInit()
    {
        #region 特休參數
        Ssql = "SELECT isNull([AL_Para1],0) As [AL_Para1],isNull([AL_Para2],0) As [AL_Para2],isNull([AL_Para3],0) As [AL_Para3] " +
            " ,isNull([AL_Para4],0) As [AL_Para4],isNull([AL_Para5],0) As [AL_Para5],isNull([AL_Para6],0) As [AL_Para6] " +
            " FROM [PersonnelSalary_Parameter] Where [Category]='01' And [Company]='" + _UserInfo.UData.Company + "'";
        DataTable Dt = _MyDBM.ExecuteDataTable(Ssql);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                CL_ALTrans.SelectedCode = Dt.Rows[0]["AL_Para6"].ToString();
            }
            else
            {
                #region 設定特休參數
                Ssql = " INSERT INTO [PersonnelSalary_Parameter] " +
                    " ([Category],[Company],[OT_Para1],[OT_Para2],[OT_Para3],[OT_Para4] " +
                    " ,[OT_Para5_1_4],[OT_Para5_4_8],[OT_Para5_8_12],[OT_Para31_1_4],[OT_Para31_4_8],[OT_Para31_8_12],[OT_Para30_1_4],[OT_Para30_4_8],[OT_Para30_8_10],[OT_Para30_10_12],[OT_Para5_1_4],[OT_Para6],[OT_Para7] " +
                    " ,[PY_Para1],[PY_Para2],[PY_Para3],[PY_Para4],[PY_Para5],[PY_Para6],[PY_Para7] " +
                    " ,[AL_Para1],[AL_Para2],[AL_Para3],[AL_Para4],[AL_Para5],[AL_Para6] " +
                    " ) " +
                    " Select '01','" + _UserInfo.UData.Company + "',46,24,1.50,1.50,1.50,1.67,1.67,1.50,1.67,1.67,1.50,1.67,1.67,1.67,2.00,2.00,0.000,0,0.005,0.00,0.00,0.06,2000 " +
                    " ,10,15,1,1,30,'03' ";
                _MyDBM.ExecuteCommand(Ssql);
                CL_ALTrans.SelectedCode = "03";
                #endregion
            }

            if (CL_ALTrans.SelectedCode.Length > 0)
                CL_ALTrans.Enabled = false;
        }
        #endregion
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
        Ssql = "SELECT ALS.*,IsNull(PM.deptid,'') AS Depid FROM AnnualLeaveSettlement ALS join Personnel_Master PM On ALS.EmployeeId = PM.EmployeeId And ALS.Company = PM.Company Where 0=0 ";

        if (cbResignC.Items.Count > 0)
        {//2011/10/12 依人事要求加入離職篩選條件
            string strTL = "";
            foreach (ListItem lis in cbResignC.Items)
            {
                if (lis.Selected)
                {
                    strTL += "'" + lis.Value + "',";
                }
            }
            if (strTL.Length > 0)
            {
                Ssql += " And PM.ResignCode in (" + strTL + "'-')";
            }
        }

        //公司
        if (SearchList1.CompanyValue.Length == 0)
        {
            lbl_Msg.Text = "請先選擇公司";
            DDLSr("0", "1");
        }
        else
            DDLSr("ALS.Company", SearchList1.CompanyValue);

        //部門
        DDLSr ( "PM.deptid " , SearchList1.DepartmentValue );

        //員工
        DDLSr ( "ALS.EmployeeId" , SearchList1.EmployeeValue );

        //特休年度
        DDLSr("ALS.ALYear", qyYearList.SelectADYear);

        Ssql += " Order By Depid,ALS.EmployeeId";
    }

    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr ( string Name , string Value )
    {
        if ( Value.Length > 0 )
            Ssql += string.Format ( " And " + Name + " like '%{0}%'" , Value );
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
            string tmpEmployeeId = Request.Form[temId + "1$ddlCodeList"];
            //ALYear
            string tmpALYear = Request.Form[temId + "2$ddlYear"].ToString();
            //TransOrNot
            string tmpTransOrNot = "N";
            if (Request.Form[temId + "7"] != null)
            {
                tmpTransOrNot = (Request.Form[temId + "7"].ToString().Equals("on")) ? "Y" : "N";
            }            


            if (
                ValidateData("Company", tmpCompany,
                             "EmployeeId", tmpEmployeeId,
                             "ALYear", tmpALYear
                            )
                )
            {

                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Company", tmpCompany);
                SDS_GridView.InsertParameters.Add("EmployeeId", tmpEmployeeId);
                SDS_GridView.InsertParameters.Add("ALYear", tmpALYear);
                SDS_GridView.InsertParameters.Add("ALDays", Request.Form[temId + "3"].ToString());
                SDS_GridView.InsertParameters.Add("LeaveDays", Request.Form[temId + "4"].ToString());
                SDS_GridView.InsertParameters.Add("LYTransDays", Request.Form[temId + "5"].ToString());
                SDS_GridView.InsertParameters.Add("ConvertibleDays", Request.Form[temId + "6"].ToString());
                SDS_GridView.InsertParameters.Add("TransOrNot", tmpTransOrNot);

                WriteLog(true, "A", "", 0);

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
                WriteLog(false, "A", "", i);
            }

            BindData();
            hid_IsInsertExit.Value = "";
        }
    }

    //  判斷資料是否重覆
    private bool ValidateData(string Key1, string strID1, string Key2, string strID2, string Key3, string strID3)
    {        
        Ssql = "Select AnnualLeaveSettlement.* FROM AnnualLeaveSettlement  Where " + Key1 + " = '" + strID1 +
     "' and " + Key2 + " ='" + strID2 +
     "' and " + Key3 + " =" + strID3 + "";

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
        string L3PK = btnDelete.Attributes["L3PK"].ToString();

        string sql = "Delete FROM AnnualLeaveSettlement  Where Company='" + L1PK +
            "' and EmployeeId='" + L2PK +
            "' and ALYear = " + L3PK;

        WriteLog(true, "D", "", 0);

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        WriteLog(false, "D", "", result > 0 ? 1 : 0);

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
        string DDId = hid_IsInsertExit.Value.Replace("_", "$") + "$CB0";
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$ctl0";

        if (Request.Form[DDId + "7"] != null)
            e.Keys.Add("TransOrNot", Request.Form[DDId + "7"].ToString().Equals("on") ? "Y" : "N");
        else
            e.Keys.Add("TransOrNot", "N");
                
        #region 開始異動前,先寫入LOG
        WriteLog(true, "U", "", 0);
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
        WriteLog(false, "U", "", (e.Exception == null) ? 1 : 0);
        #endregion

    }
    #endregion


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                if (i == 2)
                {//下拉式選單                    
                    ddlAddNew.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", "", 5);
                    e.Row.Cells[i].Style.Add("text-align", "left");
                }
                else if (i == 3)
                {
                    ASP.usercontrol_yearlist_ascx ddlTemp = new ASP.usercontrol_yearlist_ascx();
                    ddlTemp = (ASP.usercontrol_yearlist_ascx)LoadControl("~/UserControl/YearList.ascx");
                    ddlTemp.SetYearList(_UserInfo.SysSet.YearB, _UserInfo.SysSet.YearE, "");
                    ddlTemp.SelectADYear = e.Row.Cells[i].Text.Trim();
                    e.Row.Cells[i].Text = ddlTemp.SelectYear.Trim();
                }
                else if (i == 8)
                {
                    e.Row.Cells[i].Text = (e.Row.Cells[i].Text.Trim().Equals("Y") ? "是" : "否");
                }

                if (i == 2)
                {
                    ddlAddNew.SelectedCode = e.Row.Cells[i].Text.Trim();
                    e.Row.Cells[i].Text = ddlAddNew.SelectedCodeName.Trim();
                }
            }

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID  + ",'" + e.Row.ClientID +"') && SaveValue("+HiddenField1.ClientID+",'UpData')");
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
                for (int i = 2; i < 9; i++)
                {
                    if (i < 4)
                    {
                     //   e.Row.Cells[i].Style.Add("text-align", "right");
                    }
                    else if (i < 8)
                    {
                        ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("text-align", "right");
                        ((TextBox)e.Row.Cells[i].Controls[0]).Width = 30;
                    }
                    else
                    {
                        CheckBox CB = new CheckBox();
                        CB.ID = "CB" + (i - 1).ToString().PadLeft(2, '0');
                        CB.Checked = (DataBinder.Eval(e.Row.DataItem, "TransOrNot").ToString().Trim().Equals("Y"));
                        e.Row.Cells[i].Controls.Add(CB);
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
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                TextBox tbAddNew = new TextBox();                
                switch(i)
                {
                    case 2:
                        ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                        ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                        ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                        if (SearchList1.CompanyValue.Length > 0)
                        {
                            Ssql = "Company='" + SearchList1.CompanyValue.Trim() + "'";
                            if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
                                Ssql += " And DeptId='" + SearchList1.DepartmentValue.Trim() + "'";
                        }
                        else { Ssql = ""; }
                        ddlAddNew.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);

                        e.Row.Cells[i].Controls.Add(ddlAddNew);
                        strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                        break;
                    case 3:
                        ASP.usercontrol_yearlist_ascx ylAddNew = new ASP.usercontrol_yearlist_ascx();
                        ylAddNew = (ASP.usercontrol_yearlist_ascx)LoadControl("~/UserControl/YearList.ascx");
                        ylAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                        
                        e.Row.Cells[i].Controls.Add(ylAddNew);

                        ylAddNew.SetYearList(_UserInfo.SysSet.YearB, _UserInfo.SysSet.YearE, "");
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                        tbAddNew.Width = 30;
                        tbAddNew.Style.Add("text-align", "right");
                        e.Row.Cells[i].Controls.Add(tbAddNew);
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                        break;
                    case 8:
                        CheckBox cbAddNew = new CheckBox();
                        cbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                        e.Row.Cells[i].Controls.Add(cbAddNew);                        
                        break;

                    default:
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
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";
            for (int i = 1; i < 8; i++)
            {
                TextBox tbAddNew = new TextBox();
                switch (i)
                {
                    case 1:
                        ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                        if (SearchList1.CompanyValue.Length > 0)
                        {
                            Ssql = "Company='" + SearchList1.CompanyValue.Trim() + "'";
                            if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
                                Ssql += " And DeptId='" + SearchList1.DepartmentValue.Trim() + "'";
                        }
                        else { Ssql = ""; }
                        ddlAddNew.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);

                        strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                        break;
                    case 2:
                        ASP.usercontrol_yearlist_ascx ylAddNew = (ASP.usercontrol_yearlist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                        ylAddNew.SetYearList(_UserInfo.SysSet.YearB, _UserInfo.SysSet.YearE, "");
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        tbAddNew = (TextBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                        tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Width = 30;
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                        break;
                    case 7:
                        CheckBox cbAddNew = (CheckBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));                        
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

            //i = e.Row.Cells.Count - 1;
            //if (i > 0)
            //{
            //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
            //    e.Row.Cells[i - 1].Style.Add("width", "70px");
            //}
            //e.Row.Cells[i].Style.Add("text-align", "right");

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

    private void WriteLog(bool n, string c, string strSQL, int i) 
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeaveSettlement";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = c;
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = strSQL;
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

    protected void btnSetALDays_Click(object sender, EventArgs e)
    {
        string Err = "";
        lbl_Msg.Text = "";
        string strLogMsg = "";
        txtALYear.Text = txtALYear.Text.Trim();
        if (SearchList1.CompanyValue.Length == 0)
        {
            Err = "請選擇公司!";
        }
        else if ((txtALYear.Text.Length == 0) || (SysSet.FormatADDate(txtALYear.Text).Contains("Date")))
        {
            ////日期
            //if ( txtALYear.Text.Length > 0 )
            //    Ssql += string.Format ( " And Convert(varchar,ALS.ALYear,111) like '%{0}%'" , SysSet.FormatADDate ( txtALYear.Text ) );

            Err = "[到職基準日]輸入有誤! 請輸入日期!";
            txtALYear.Text = "";
        }
        else if (CL_ALTrans.SelectedCode.Length == 0)
        {
            Err = "請選擇[未休特休轉換方式]!";
        }
        
        if (Err.Length > 0)
        {
            lbl_Msg.Text = Err;
            return;
        }
        else
        {
            int iInsertCount = 0;
            string strCompany = "", strEmployeeId = "", strYear = "";
            string strALDate = SysSet.FormatADDate(txtALYear.Text);
            string strALDays = "", strLastALDays = "", strALUseDays = "";
            strLogMsg = "Company=" + SearchList1.CompanyValue + "|Department=" + SearchList1.DepartmentValue + "|Employee=" + SearchList1.EmployeeValue;

            strYear = qyYearList.SelectADYear;

            #region 開始異動前,先寫入LOG
            WriteLog(true, "A", strLogMsg, 0);
            #endregion

            #region 設定特休天數
            Ssql = "select * "+
                " ,DATEDIFF(month,IsNull(HireDate,CONVERT(datetime,'" + strALDate + "')),CONVERT(datetime,'" + strALDate + "')) As HDMonths " +
                " ,Datepart(day, IsNull(HireDate,convert(datetime,'" + strALDate + "'))) As HDDate " +
                " ,Datepart(day, convert(datetime,'" + strALDate + "')) As ALDate " +
                " ,(Datepart(day, convert(datetime,'" + strALDate + "')) - Datepart(day, IsNull(HireDate,convert(datetime,'" + strALDate + "')))) As DifDays " +
                " From [Personnel_Master] Where 0=0 ";
            //公司
            DDLSr("Company", SearchList1.CompanyValue);

            //部門
            DDLSr("deptid ", SearchList1.DepartmentValue);

            //員工
            DDLSr("EmployeeId", SearchList1.EmployeeValue);

            Ssql += " Order by Company,deptid,EmployeeId";
            DataTable Dt = _MyDBM.ExecuteDataTable(Ssql);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    iInsertCount = 0;
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        strCompany = Dt.Rows[i]["Company"].ToString().Trim();
                        strEmployeeId = Dt.Rows[i]["EmployeeId"].ToString().Trim();

                        #region 取得特休年資天數
                        strALDays = Dt.Rows[i]["HDMonths"].ToString();
                        if (!Dt.Rows[i]["HDDate"].ToString().Equals(Dt.Rows[i]["ALDate"].ToString()))
                        {
                            if (((int)Dt.Rows[i]["DifDays"]) < 30)
                            {
                                strALDays = ((int)Dt.Rows[i]["HDMonths"] - 1).ToString();
                            }
                        }
                        
                        Ssql = " SELECT Top 1 [ALDays] FROM [AnnualLeave_SeriesParameter] where [MonthLowerLimit]<= " + strALDays + " Order by [MonthLowerLimit] Desc";
                        DataTable DtHDMonths = _MyDBM.ExecuteDataTable(Ssql);
                        strALDays = "0";
                        if (DtHDMonths != null)
                        {
                            if (DtHDMonths.Rows.Count > 0)
                            {
                                strALDays = DtHDMonths.Rows[0][0].ToString();                                  
                            }
                        }
                        DtHDMonths.Dispose();
                        #endregion

                        strLastALDays = "0";
                        if (CL_ALTrans.SelectedCode.Equals("01"))
                        {//轉特休
                            #region 取得並加入去年可轉休天數
                            Ssql = " SELECT (IsNull(ConvertibleDays,0)+" + strALDays + ") As ALDays,IsNull(ConvertibleDays,0) As LastALDays From AnnualLeaveSettlement " +
                                " WHERE Company='" + strCompany + "' And EmployeeId='" + strEmployeeId + "' And ALYear=(" + strYear + "-1) ";
                            DataTable DtLastYear = _MyDBM.ExecuteDataTable(Ssql);
                            if (DtLastYear != null)
                            {
                                if (DtLastYear.Rows.Count > 0)
                                {
                                    strALDays = DtLastYear.Rows[0]["ALDays"].ToString();
                                    strLastALDays = DtLastYear.Rows[0]["LastALDays"].ToString();
                                    //將已轉入特休之資料,標記為已轉
                                    Ssql = " Update AnnualLeaveSettlement Set TransOrNot='Y' " +
                                " WHERE Company='" + strCompany + "' And EmployeeId='" + strEmployeeId + "' And ALYear=(" + strYear + "-1) ";
                                    _MyDBM.ExecuteDataTable(Ssql);
                                }
                            }
                            DtLastYear.Dispose();
                            #endregion
                        }
                        else if (CL_ALTrans.SelectedCode.Equals("02"))
                        {//轉薪資
                            //由薪資計算時結算,不在此處轉入
                        }

                        #region 取得指定年度已休特休天數
                        strALUseDays = "0";
                        Ssql = " Select (Sum([hours])/8+Sum([days])) As ALUseDays From Leave_Trx " +
                            " Where LeaveType_Id='A' And Company='" + strCompany + "' And EmployeeId='" + strEmployeeId + "'" +
                            " And round(Payroll_Processingmonth/100,0) = " + strYear +
                            " and Not ([ApproveDate] is Null) Group By [Company],[EmployeeId]";
                        DataTable DtALUseDays = _MyDBM.ExecuteDataTable(Ssql);

                        if (DtALUseDays != null)
                        {
                            if (DtALUseDays.Rows.Count > 0)
                            {
                                strALUseDays = DtALUseDays.Rows[0]["ALUseDays"].ToString().Trim();
                            }
                        }
                        DtALUseDays.Dispose();
                        #endregion

                        #region 結算指定年度特休天數
                        if (ValidateData("Company", strCompany,
                         "EmployeeId", strEmployeeId,
                         "ALYear", strYear))
                        {
                            Ssql = "INSERT INTO AnnualLeaveSettlement([Company],[EmployeeId],[ALYear],[ALDays],[LeaveDays],[LYTransDays],[ConvertibleDays],[TransOrNot]) " +
                                " VALUES ('" + strCompany + "','" + strEmployeeId + "'," + strYear + "," + strALDays + "," + strALUseDays +
                                "," + strLastALDays + ",(" + strALDays + "-" + strLastALDays + "-" + strALUseDays + "),'N')";
                        }
                        else
                        {
                            Ssql = "UPDATE AnnualLeaveSettlement SET ALDays=" + strALDays + ", LeaveDays=" + strALUseDays +
                                ", LYTransDays=" + strLastALDays +
                                ", ConvertibleDays=(" + strALDays + "-" + strLastALDays + "-" + strALUseDays + "), TransOrNot='N'" +
                                " WHERE Company='" + strCompany + "' And EmployeeId='" + strEmployeeId + "' And ALYear=" + strYear;
                        }
                        iInsertCount += _MyDBM.ExecuteCommand(Ssql);
                        #endregion
                    }
                }
            }

            #endregion

            if (iInsertCount > 0)
            {
                lbl_Msg.Text = "設定成功! 共增修 " + iInsertCount.ToString() + " 筆";
            }
            else if (iInsertCount == 0)
            {
                lbl_Msg.Text = "設定失敗! 請確認各項數值是否輸入有誤!!";
            }
            else
            {
                lbl_Msg.Text = "設定失敗! 請確認各項數值是否輸入有誤!!";
            }

            #region 完成異動後,更新LOG資訊
            WriteLog(false, "A", strLogMsg, 0);
            #endregion
        }
        BindData();
        showPanel();
    }

    private void SetResignCode()
    {
        cbResignC.Items.Clear();
        Ssql = "select [CodeCode],[CodeName] from CodeDesc Where CodeID = 'PY#ResignC'";
        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        ListItem theItem = new ListItem();
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            theItem = new ListItem();
            theItem.Value = DT.Rows[i]["CodeCode"].ToString();
            theItem.Text = DT.Rows[i]["CodeName"].ToString();
            if (!DT.Rows[i]["CodeCode"].ToString().Equals("Y"))
                theItem.Selected = true;
            cbResignC.Items.Add(theItem);
        }
    }
}//  程式尾部
