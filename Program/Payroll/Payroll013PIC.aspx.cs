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

public partial class Payroll013PIC : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM013";
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
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Detail", "Detail", "Add" };

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
                    btnComputerPayroll.Visible = Find;                    
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
        btnComputerPayroll.Attributes.Add("onClick", "drawWait('')");

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

        ScriptManager1.RegisterPostBackControl(btnComputerPayroll);
    }

    /// <summary>
    /// 薪資試算
    /// </summary>
    protected void btnComputerPayroll_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        Navigator1.Visible = false;
        GridView1.Visible = false;

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            btnComputerPayroll.Enabled = false;
            LabMsg.Text = "開始試算,請稍候...";            
            Payroll Y = new Payroll();
            int iRet = Y.DraftPayroll(CompanyList1.SelectValue, SalaryYM1.SelectSalaryYM, "0");
            LabMsg.Text = SalaryYM1.SelectSalaryYM + " 試算" + _UserInfo.SysSet.PYReturnMsg(iRet);
            btnComputerPayroll.Enabled = true;
        }
    }

    /// <summary>
    /// 薪資試算查詢
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
            sYM = Payroll.CheckSalaryYM(CompanyList1.SelectValue, SalaryYM1.SelectSalaryYM, "0");
            if (sYM.isControl == false)
            {//未經控管
                retResult = -2;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無結果可查詢";
            }
            else if (string.IsNullOrEmpty(sYM.DraftDate))
            {
                //指定公司於指定計薪年月未經試算
                retResult = -3;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無結果可查詢";
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
    }
    private void BindData(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PR13PIC" + DateTime.Now.ToString("yyyyMMddmmss");
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
            " FROM PayrollWorking_Heading PRH " +
            " left join (SELECT [Company],[EmployeeId] as PMEmployeeId,[DeptId] FROM [Personnel_Master]) PM On PRH.[Company]=PM.[Company] And PRH.[EmployeeId]=PM.[PMEmployeeId]" +
            " Where PRH.Company='" + CompanyList1.SelectValue.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And SalaryYM = {0}", SalaryYM1.SelectSalaryYM.Trim());
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", '0');

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
                    e.Row.Cells[2].Text = strPan + e.Row.Cells[2].Text + "-" + DBSetting.DepartmentName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim());
                    e.Row.Cells[3].Text = strPan + e.Row.Cells[3].Text + "-" + DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[3].Text.Trim());

                    for (int i = 4; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Style.Add("text-align", "right");
                        try
                        {
                            if (i == 5)
                            {
                                e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text.Trim()).ToString("N2").Replace(".00", "") + strPan;
                            }
                            else
                            {
                                e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text.Trim()).ToString("N0") + strPan;
                            }
                        }
                        catch { }
                    }    
                }
                string tmpUrl = "";
                link = (LinkButton)e.Row.FindControl("btnSelect1");
                if (link != null)
                {
                    tmpUrl = "Payroll013PIC_D.aspx?";
                    tmpUrl += "Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim();
                    tmpUrl += "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim();
                    tmpUrl += "&SYM=" + SalaryYM1.SelectSalaryYM.Trim();
                    tmpUrl += "&SPeriod=A";
                    link.Attributes.Add("onclick", "javascript:var win =window.open('" + tmpUrl + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                }
                link = (LinkButton)e.Row.FindControl("btnSelect2");
                if (link != null)
                {
                    tmpUrl = "Payroll013PIC_D.aspx?";
                    tmpUrl += "Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim();
                    tmpUrl += "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim();
                    tmpUrl += "&SYM=" + SalaryYM1.SelectSalaryYM.Trim();
                    tmpUrl += "&SPeriod=B";
                    link.Attributes.Add("onclick", "javascript:var win =window.open('" + tmpUrl + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                }
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
