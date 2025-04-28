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

public partial class Payroll_Payroll015 : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM015";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'ePayroll' And RTrim(ProgramPath)='Payroll/Payroll015.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Detail", "Add" };

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
                {//新增(是否有試算權限)
                    btnConfirmPayroll.Visible = Find;
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
        Navigator1.BindGridView = GridView1;
        // 需要執行等待畫面的按鈕
        btnConfirmPayroll.Attributes.Add("onClick", "drawWait('')");

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

            AuthRight();

            PeriodList1.defaulPeriod();
            Navigator1.Visible = false;
            SalaryYM1.defaultlist();
        }
        else
        {
            LabMsg.Text = "";
            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);

            if (Navigator1.Visible)
                BindData();
        }

        ScriptManager1.RegisterPostBackControl(btnConfirmPayroll);
    }

    /// <summary>
    /// 薪資試算確認
    /// </summary>
    protected void btnConfirmPayroll_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        Navigator1.Visible = false;
        GridView1.Visible = false;

        if (Session["SoftwareAuth"] == null || (bool)Session["SoftwareAuth"] == false)
        {
            LabMsg.Text = "您的軟體授權已到期!無法使用此功能";
            return;
        }

        int retResult = 0;
        Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
        sYM = Payroll.CheckSalaryYM(CompanyList1.SelectValue, SalaryYM1.SelectSalaryYM, PeriodList1.selectIndex.ToString());

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else if (sYM.beReDraft)
        {
            //指定之計薪年月,須重新試算
            retResult = -7;
            LabMsg.Text = SalaryYM1.SelectSalaryYMName + "薪資確認失敗! " + _UserInfo.SysSet.PYReturnMsg(retResult);
        }
        else
        {
            btnConfirmPayroll.Enabled = false;
            LabMsg.Text = "開始確認,請稍候...";
            Payroll Y = new Payroll();
            DataTable Dt;
            int iRet = Y.ConfirmDraftPayroll(CompanyList1.SelectValue, SalaryYM1.SelectSalaryYM, PeriodList1.selectIndex.ToString(), out Dt);
            gvMsg.DataSource = Dt;
            gvMsg.DataBind();
            //if (iRet == 0) iRet = 13;
            LabMsg.Text = SalaryYM1.SelectSalaryYMName + "薪資確認:" + _UserInfo.SysSet.PYReturnMsg(iRet);
            if (iRet < 0)
            {
                try
                {
                    string LYM, NYM;
                    if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
                    {
                        LYM = (sYM.LastConfirmYM / 100 - 1911).ToString() + "年" + (sYM.LastConfirmYM % (sYM.LastConfirmYM / 100)).ToString() + "月";
                        NYM = (sYM.NextConfirmYM / 100 - 1911).ToString() + "年" + (sYM.NextConfirmYM % (sYM.NextConfirmYM / 100)).ToString() + "月";
                    }
                    else
                    {
                        LYM = (sYM.LastConfirmYM / 100).ToString() + "年" + (sYM.LastConfirmYM % (sYM.LastConfirmYM / 100)).ToString() + "月";
                        NYM = (sYM.NextConfirmYM / 100).ToString() + "年" + (sYM.NextConfirmYM % (sYM.NextConfirmYM / 100)).ToString() + "月";
                    }
                    LabMsg.Text += "</br>上次確認薪資年月為 " + LYM + "</br>待確認薪資年月應為 " + NYM;
                }
                catch
                { }
            }
            btnConfirmPayroll.Enabled = true;
        }
    }

    /// <summary>
    /// 薪資查詢
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            int retResult = 0;
            Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
            sYM = Payroll.CheckSalaryYM(CompanyList1.SelectValue, SalaryYM1.SelectSalaryYM, PeriodList1.selectIndex.ToString());
            if (sYM.isControl == false)
            {//未經控管
                retResult = -2;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算與確認作業! 無結果可查詢";
            }
            else if (string.IsNullOrEmpty(sYM.DraftDate))
            {
                //指定公司於指定計薪年月未經試算
                retResult = -3;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無結果可查詢";
            }
            else if (string.IsNullOrEmpty(sYM.ConfirmDate))
            {
                //指定公司於指定計薪年月已完成確認試算,不可再進行確認
                retResult = -4;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行確認作業! 無結果可查詢";
            }
            else
            {
                Navigator1.Visible = true;
                GridView1.Visible = true;
                BindData();
            }

            if (retResult < 0)
            {
                if (retResult == 0)
                    LabMsg.Text = "查無資料!!";
                Navigator1.Visible = false;
                GridView1.Visible = false;
            }
        }
    }

    private void BindData()
    {
        BindData("");
        BindDataConfirmResult();
    }

    /// <summary>
    /// 取得確認後薪資明細
    /// </summary>
    /// <param name="SortExpression"></param>
    private void BindData(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.DCK" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(DeCodeKey);

        string strSort = " Order By PRH.Company,IsNull(DeptId,'ZZZ'),PRH.EmployeeId";

        Ssql = "SELECT [DeptId],PRH.[Company],PRH.[EmployeeId],[SalaryYM],[PeriodCode],[Paydays],[LeaveHours_deduction],[TaxRate],[ResignCode]" +
            ",[HI_Person],[WT_Overtime],[NT_Overtime],[OnWatch],[Dependent1_IDNo],[Dependent2_IDNo],[Dependent3_IDNo]" +
            "," + DeCodeKey + "([BaseSalary]) As BaseSalary" +
            "," + DeCodeKey + "([LI_Fee]) As LI_Fee " +
            "," + DeCodeKey + "([HI_Fee]) As HI_Fee " +
            //"," + DeCodeKey + "([NT_P]) " +
            //"," + DeCodeKey + "([WT_P_Salary])" +
            //"," + DeCodeKey + "([NT_M])" +
            //"," + DeCodeKey + "([WT_P_Bonus])" +
            //"," + DeCodeKey + "([WT_M_Salary])" +
            //"," + DeCodeKey + "([WT_M_Bonus])" +
            //"," + DeCodeKey + "([P1_borrowing])" +
            //"," + DeCodeKey + "([WT_Overtime_Fee])" +
            //"," + DeCodeKey + "([NT_Overtime_Fee])" +
            //"," + DeCodeKey + "([OnWatch_Fee])" +
            //"," + DeCodeKey + "([Dependent1_HI_Fee])" +
            //"," + DeCodeKey + "([Dependent2_HI_Fee])" +
            //"," + DeCodeKey + "([Dependent3_HI_Fee])" +
            "  FROM Payroll_History_Heading PRH " +
            " left join (SELECT [Company],[EmployeeId] as PMEmployeeId,[DeptId] FROM [Personnel_Master]) PM On PRH.[Company]=PM.[Company] And PRH.[EmployeeId]=PM.[PMEmployeeId]" +
            " Where PRH.Company='" + CompanyList1.SelectValue.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And SalaryYM = {0}", SalaryYM1.SelectSalaryYM.Trim());
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", PeriodList1.selectIndex.ToString());

        if (!string.IsNullOrEmpty(SortExpression))
        {
            strSort = "Order By " + SortExpression + ((bool)ViewState["SortDirectionDesc"] ? " Desc" : "");
        }

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + strSort);

        py.AfterQuery(DeCodeKey);

        GridView1.DataSource = theDT;
        GridView1.DataBind();

        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    /// <summary>
    /// 取得確認薪資LOG訊息
    /// </summary>
    private void BindDataConfirmResult()
    {
        string strSort = " Order by ChgStartDateTime DESC,ChgStopDateTime ";

        Ssql = "select * from [DataChangeLog] Where TableName='sp_PY_ConfirmDraftPayroll' and ChangItem like '" +
            CompanyList1.SelectValue + "_" + SalaryYM1.SelectSalaryYM + "_" + PeriodList1.selectIndex.ToString() + "%' ";

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + strSort);

        gvMsg.DataSource = theDT;
        gvMsg.DataBind();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;
        Label lbl;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時                    
                }
                else
                {
                    //查詢時                   
                    string strPan = "&nbsp;&nbsp;&nbsp;&nbsp;";
                    e.Row.Cells[1].Text = strPan + e.Row.Cells[1].Text + "-" + DBSetting.DepartmentName(CompanyList1.SelectValue, e.Row.Cells[1].Text.Trim());
                    e.Row.Cells[2].Text = strPan + e.Row.Cells[2].Text + "-" + DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim());

                    for (int i = 3; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Style.Add("text-align", "right");
                        try
                        {
                            if (i == 4 || i > 6)
                            {
                                e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text.Trim()).ToString("N2") + strPan;
                            }
                            else
                            {
                                e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text.Trim()).ToString("N0") + strPan;
                            }
                        }
                        catch { }
                    }
                }

                link = (LinkButton)e.Row.FindControl("btnSelect");
                string tmpUrl = "Payroll013_D.aspx?Kind=H&";
                tmpUrl += "Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim();
                tmpUrl += "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim();
                tmpUrl += "&SYM=" + SalaryYM1.SelectSalaryYM.Trim();
                tmpUrl += "&SPeriod=" + PeriodList1.selectIndex.ToString();
                link.Attributes.Add("onclick", "javascript:var win =window.open('" + tmpUrl + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");

                break;

            case DataControlRowType.Header:


                break;
        }
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["SortDirectionDesc"] == null)
            ViewState["SortDirectionDesc"] = true;
        BindData(e.SortExpression);
        ViewState["SortDirectionDesc"] = (!((bool)ViewState["SortDirectionDesc"]));
    }
}
