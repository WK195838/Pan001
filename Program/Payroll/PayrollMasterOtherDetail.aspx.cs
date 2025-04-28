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

public partial class PayrollMasterOtherDetail : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM012";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll py = new Payroll();

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
    }

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

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息

        Navigator1.BindGridView = GridView1;
        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);

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

            this.CL_Period.SetCodeList("PY#Period", 5);
            SalaryYM1.defaultlist();
            SalaryYM2.defaultlist();
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            SalaryYM1.SelectSalaryYM = DateTime.Now.ToString("yyyyMM");
            SalaryYM2.SelectSalaryYM = DateTime.Now.ToString("yyyyMM");
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            #region ---根據postback回來的不同 給予不同的功能---
            if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
            {
                BindData();
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[ddlId + "$tbAddNew05"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
            }
            else
            {
                if (!Request.Form["__EVENTTARGET"].ToString().Contains("GridView1$ctl01$ddl01"))
                {
                    if (Request.Form["__EVENTTARGET"].ToString() == "" && Request.Form["__EVENTARGUMENT"].ToString() == "")
                    {

                    }
                    else
                    {
                        BindData();
                    }
                }
            }
            #endregion
        }

        //btnCalendar1.Attributes.Add("onclick", "return GetPromptDate(" + txtDate.ClientID + ");");
        //btnSetWeekend.Attributes.Add("onclick", "return confirm('原年度若有設定之假日將被清空!是否確定新增 '+document.getElementById('" + YearList1.YListClientID() + "').value+' 年度週末假日?');");

    }

    private void showPanel()
    {
        if (hasData())
        {
            Panel_Empty.Visible = false;            
        }
        else
        {
            if (SearchList1.CompanyValue == "")
            {
                Panel_Empty.Visible = false;          
            }
            else
            {                
                Panel_Empty.Visible = true;             
            }
        }
    }

    // 搜尋模組連動
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();
        showPanel();
    }

    private bool hasData()
    {
        GridView1.Visible = true;
        if (GridView1.Rows.Count > 0)
        {
            Navigator1.Visible = (GridView1.PageCount > 1) ? true : false;            
            return true;
        }
        else
        {            
            Navigator1.Visible = false;
            return false;
        }
    }

    private void BindData()
    {        
        Ssql = "SELECT * FROM [Payroll_Master_OtherDetail] Where 0=0";

        if (SearchList1.CompanyValue.Length > 0)
        {//公司
            Ssql += string.Format(" And Company='{0}'", SearchList1.CompanyValue.Trim());
        }
        else
        {
            Ssql += string.Format(" And Company='{0}'", SearchList1.CompanyValue.Trim());
            lbl_Msg.Text = "請先選擇公司";
        }

        if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
        {//部門
            Ssql += string.Format(" And EmployeeId in (select [EmployeeId] From [Personnel_Master] Where [DeptId]='{0}')", SearchList1.DepartmentValue.Trim());
        }

        if (SearchList1.EmployeeValue.Replace("%", "").Length > 0)
        {//員工
            Ssql += string.Format(" And EmployeeId ='{0}'", SearchList1.EmployeeValue.Trim());
        }

        if (cbYM.Checked == true)
        {
            if (SalaryYM1.SelectSalaryYM.Length > 0)
            {//年月
                Ssql += string.Format(" And [SalaryYM] >={0}", SalaryYM1.SelectSalaryYM.Trim());
            }

            if (SalaryYM2.SelectSalaryYM.Length > 0)
            {//年月
                Ssql += string.Format(" And [SalaryYM] <={0}", SalaryYM2.SelectSalaryYM.Trim());
            }
        }
        if (CL_Period.SelectedCode.Length>0)
        {//期間
            Ssql += string.Format(" And [PeriodCode] ='{0}'", CL_Period.SelectedCode.Trim());
        }

        SDS_GridView.SelectCommand = Ssql;

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        if (SearchList1.CompanyValue.Length > 0)
        {
            GridView1.Visible = true;
            BindData();
            showPanel();
        }
        else
        {
            lbl_Msg.Text = "請先選擇公司";
            GridView1.Visible = false;
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hid_Company.Value = GridView1.SelectedDataKey.Values["Company"].ToString();
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["LaborInsurance_RateParameterDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
        
        string Err = "";
        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "1$ddlCodeList"].ToString()))
            {
                Err += "請先選擇員工";
            }
            else if (String.IsNullOrEmpty(Request.Form[temId + "3$ddlCodeList"].ToString()))
            {
                Err += "請先選擇期間";
            }
            else if (String.IsNullOrEmpty(Request.Form[temId + "4$ddlCodeList"].ToString()))
            {
                Err += "請先選擇薪資項目";
            }
            else if (String.IsNullOrEmpty(Request.Form[temId + "5"].ToString()))
            {
                Err += "請先輸入金額";
            }
            else 
            {
                //                
            }
            string sYM = Request.Form[temId + "2$YearList1$ddlYear"].ToString() + Request.Form[temId + "2$MonthList1$MonthList"].ToString();
            if (Err == "")
            {
                //新增
                if (ValidateData(SearchList1.Company.SelectedValue.Trim(), Request.Form[temId + "1$ddlCodeList"].ToString(), sYM
                    , Request.Form[temId + "3$ddlCodeList"].ToString(), Request.Form[temId + "4$ddlCodeList"].ToString()))
                {
                    SDS_GridView.InsertParameters.Clear();
                    SDS_GridView.InsertParameters.Add("Company", SearchList1.Company.SelectedValue.Trim());
                    SDS_GridView.InsertParameters.Add("EmployeeId", Request.Form[temId + "1$ddlCodeList"].ToString().Trim());
                    SDS_GridView.InsertParameters.Add("SalaryYM", sYM);
                    SDS_GridView.InsertParameters.Add("PeriodCode", Request.Form[temId + "3$ddlCodeList"].ToString());
                    SDS_GridView.InsertParameters.Add("SalaryItem", Request.Form[temId + "4$ddlCodeList"].ToString());
                    SDS_GridView.InsertParameters.Add("SalaryAmount", py.EnCodeAmount(Request.Form[temId + "5"].ToString()));

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
                    MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonalShift";
                    MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                    MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
                    MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                    //此時不設定異動結束時間
                    //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                    MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                    _MyDBM.DataChgLog(MyCmd.Parameters);
                    #endregion

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

                    #region 完成異動後,更新LOG資訊
                    MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
                    MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                    _MyDBM.DataChgLog(MyCmd.Parameters);
                    #endregion

                    BindData();
                    showPanel();
                }
                else
                {
                    lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
                    BindData();
                }
                hid_IsInsertExit.Value = "";
            }
            else
            {
                lbl_Msg.Text = Err;
                BindData();
                hid_IsInsertExit.Value = "";
            }
        }
  
    }

    private bool ValidateData(string strCompany, string strEmployeeId, string strSalaryYM, string strPeriodCode, string strSalaryItem)
    {
        //判斷資料是否重覆
        Ssql = " Where Company='" + strCompany + "' and EmployeeId='" + strEmployeeId + "' and SalaryYM=" + strSalaryYM +
            " and PeriodCode='" + strPeriodCode + "' and SalaryItem='" + strSalaryItem + "' ";
        Ssql = "Select *  From Payroll_Master_OtherDetail " + Ssql;

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }    

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
//
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
//
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string L3PK = btnDelete.Attributes["L3PK"].ToString();
        string L4PK = btnDelete.Attributes["L4PK"].ToString();
        string L5PK = btnDelete.Attributes["L5PK"].ToString();

        string sql = " Where Company='" + L1PK + "' and EmployeeId='" + L2PK + "' and SalaryYM=" + L3PK + " " +
            " and PeriodCode='" + L4PK + "' and SalaryItem='" + L5PK + "' ";

        #region 開始異動前,先寫入LOG
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Payroll_Master_OtherDetail";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = sql;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        sql = "Delete From Payroll_Master_OtherDetail " + sql;
        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();
        showPanel();
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
            //    e.Row.Cells[i - 1].Style.Add("width", "100px");
            //}
            //e.Row.Cells[i].Style.Add("text-align", "left");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
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
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        for (int i = 0; i < 2; i++)
        {
            try
            {
                tempOldValue = "";
                if (e.OldValues[i] != null)
                    tempOldValue = e.OldValues[i].ToString().Trim();

                if (e.NewValues[i] != null)
                    e.NewValues[i] = py.EnCodeAmount(e.NewValues[i].ToString().Trim());
                else
                    e.NewValues[i] = "";
                
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }

                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += GridView1.HeaderRow.Cells[i + 3].Text.Trim() + "|";
                        UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonalShift";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = UpdateItem;
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
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
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
            return;

        showPanel();
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
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "", tempValue = "";

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
                    ASP.usercontrol_salaryym_ascx ddlTemp= new ASP.usercontrol_salaryym_ascx();
                    ddlTemp = (ASP.usercontrol_salaryym_ascx)LoadControl("~/UserControl/SalaryYM.ascx");
                    ddlTemp.SetOtherYMList(DateTime.Now.Year - 1, DateTime.Now.Year + 1, DateTime.Now.Year.ToString());
                    ddlTemp.SelectSalaryYM = e.Row.Cells[i].Text.Trim();
                    e.Row.Cells[i].Text = ddlTemp.SelectSalaryYMName.Trim();
                }
                else if (i == 4)
                {
                    ddlAddNew.SetCodeList("PY#Period", 5);
                }
                else if (i == 5)
                {
                    ddlAddNew.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5);
                }

                if ((i == 2) || (i == 4) || (i == 5))
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
                    IB.Attributes.Add("onclick", "return (confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "') );");
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
                #endregion

                //金額
                TextBox TB = ((TextBox)e.Row.Cells[6].Controls[0]);
                if (TB != null)
                {
                    TB.Style.Add("text-align", "right");
                    TB.MaxLength = 7;
                    TB.Text = py.DeCodeAmount(TB.Text).ToString();
                }
            }
            else
            {
                #region 查詢用
                //e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;
                if (e.Row.Cells[0].Controls[0] != null)
                {
                    LinkButton LB = (LinkButton)e.Row.Cells[0].Controls[1];
                    //if (((DataRowView)DataBinder.GetDataItem(e.Row)).Row["ItemType"] != null)
                    //    if (((DataRowView)DataBinder.GetDataItem(e.Row)).Row["ItemType"].ToString().Equals("0"))
                    //        LB.Text = "";
                }

                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                #endregion
                //金額
                e.Row.Cells[6].Style.Add("text-align", "right");
                //e.Row.Cells[6].Text = string.Format("{0:###,##0.#}", int.Parse(py.DeCodeAmount(e.Row.Cells[3].Text).ToString()));
                e.Row.Cells[6].Text = int.Parse(py.DeCodeAmount(e.Row.Cells[6].Text).ToString()).ToString("N0");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (i == 2)
                {//下拉式選單
                    ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                    ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    Ssql = "Upper(IsNull(ResignCode,'')) != 'Y'";
                    if (SearchList1.CompanyValue.Length > 0)
                    {
                        Ssql += " And Company='" + SearchList1.CompanyValue.Trim() + "'";
                        if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
                            Ssql += " And DeptId='" + SearchList1.DepartmentValue.Trim() + "'";
                    }
                    else { Ssql = "1=0"; }
                    ddlAddNew.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);
                    
                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 3)
                {
                    ASP.usercontrol_salaryym_ascx ddlAddNew = new ASP.usercontrol_salaryym_ascx();
                    ddlAddNew = (ASP.usercontrol_salaryym_ascx)LoadControl("~/UserControl/SalaryYM.ascx");
                    ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    ddlAddNew.SetOtherYMList(DateTime.Now.Year - 1, DateTime.Now.Year + 1, DateTime.Now.Year.ToString());
                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                }
                else if (i == 4)
                {
                    ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                    ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    ddlAddNew.SetCodeList("PY#Period", 5);
                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 5)
                {
                    ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                    ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    ddlAddNew.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5);                    

                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 6)
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "100px");
                    tbAddNew.MaxLength = 7;
                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
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

            for (int i = 1; i < 6; i++)
            {
                if (i == 1)
                {
                    ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    Ssql = "Upper(IsNull(ResignCode,'')) != 'Y'";
                    if (SearchList1.CompanyValue.Length > 0)
                    {
                        Ssql += " And Company='" + SearchList1.CompanyValue.Trim() + "'";
                        if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
                            Ssql += " And DeptId='" + SearchList1.DepartmentValue.Trim() + "'";
                    }
                    else { Ssql = "1=0"; }
                    ddlAddNew.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);

                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 2)
                {
                    ASP.usercontrol_salaryym_ascx tbAddNew = (ASP.usercontrol_salaryym_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    tbAddNew.SetOtherYMList(DateTime.Now.Year - 1, DateTime.Now.Year + 1, DateTime.Now.Year.ToString());
                }
                else if (i == 3)
                {                    
                    ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    ddlAddNew.SetCodeList("PY#Period", 5);

                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 4)
                {
                    ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    ddlAddNew.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5);

                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else if (i == 5)
                {
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "100px");
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }
}
