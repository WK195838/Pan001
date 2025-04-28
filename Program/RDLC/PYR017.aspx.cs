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

public partial class RDLCPYR : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR017";
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

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);

        Navigator1.BindGridView = GridView1;
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        btReport.Attributes.Add("onClick", "drawWait('')");

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

            Navigator1.Visible = false;
            ReportViewer1.Visible = false;
            int YB = 0, YE = 0, YD = 0;
            YB = _UserInfo.SysSet.YearB;
            YE = _UserInfo.SysSet.YearE;
            YD = DateTime.Now.Year;
            if (YB < 1000 && YD > 1000)
                YD = YD - 1911;
            else if (YB > 1000 && YD < 1000)
                YD = YD + 1911;
            YLSelect.SetYearList(YB, YE, YD.ToString(), "請選擇");
        }
        else
        {
            LabMsg.Text = "";

            if (Navigator1.Visible)
                BindData();
        }

        ScriptManager1.RegisterPostBackControl(btnQuery);
        ScriptManager1.RegisterPostBackControl(btReport);
    }

    protected void btReport_Click(object sender, EventArgs e)
    {
        // 清除之前的資料
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.Visible = false;
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請先選擇公司!";
        }
        else if (YLSelect.SelectADYear == "")
        {
            LabMsg.Text = "請指定統計年度!";
        }
        else
        {
            int retResult = 0;
            LabMsg.Text = "";

            string strTitle = StyleTitle1.Title;
            DataTable Table1 = new DataTable();

            if (retResult >= 0)
            {
                Navigator1.Visible = false;
                GridView1.Visible = false;

                Table1 = GetTable("");
                if (Table1 != null)
                    //for (int i = 0; i < Table1.Rows.Count; i++)
                    //{
                    //    //找出部門名稱做為群組依據
                    //    Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());                                                
                    //}
                    retResult = 0;
                else
                    retResult = -1;
            }

            if (retResult >= 0)
            {
                string strValue = DBSetting.CompanyName(SearchList1.CompanyValue);
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("RDLCPYR", Table1));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", strValue));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Title", strTitle));
                strValue = "　";
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Query1", strValue));
                strValue = "統計年度：" + YLSelect.SelectYear;
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Query2", strValue));                
                ReportViewer1.Visible = true;
            }
            else
            {
                if (LabMsg.Text.Length == 0)
                    LabMsg.Text = "查無資料";
                ReportViewer1.Visible = false;
            }
        }
    }

    /// <summary>
    /// 薪資查詢
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        ReportViewer1.Visible = false;

        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else if (YLSelect.SelectADYear == "")
        {
            LabMsg.Text = "請指定統計年度!";
        }
        else
        {
            Navigator1.Visible = true;
            GridView1.Visible = true;
            BindData();
        }
    }

    private DataTable GetTable(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PYR017" + DateTime.Now.ToString("yyyyMMddHHmm");
        py.BeforeQuery(DeCodeKey);
        string Sqlcommand1 = "", Sqlcommand2 = "", Sqlcommand3 = "";
        string strSort = " Order By Company,IsNull(DeptId,'ZZZ'),EmployeeId";
        string strROWID = " 0 ";
        //檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        if (_MyDBM.ServerVersion().StartsWith("08."))
            strROWID = " 0 ";
        else
            strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";

        Ssql = " SELECT *, (M01+M02+M03+M04+M05+M06+M07+M08+M09+M10+M11+M12) As Total " +
            " From ( " +
            " SELECT OT.[Company],OT.[EmployeeId],[EmployeeName] " +
            " ,IsNull(OT.[DeptId],PM.[DeptId]) As [DeptId] " +
            " ,(select [DepName] from [Department] where [Company]=OT.[Company] and [DepCode]=IsNull(OT.[DeptId],PM.[DeptId])) As [DeptName] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 1 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M01] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 2 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M02] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 3 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M03] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 4 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M04] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 5 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M05] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 6 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M06] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 7 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M07] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 8 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M08] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 9 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M09] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 10 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M10] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 11 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M11] " +
            " ,Sum(Case DatePart(Month,[OverTime_Date]) When 12 then (IsNull([OverTime1],0)+IsNull([OverTime2],0)+IsNull([Holiday],0)+IsNull([NationalHoliday],0)) else 0 end ) As [M12] " +
            "  FROM [OverTime_Trx] OT  " +
            "  Left Join [Personnel_Master] PM On OT.[Company]=PM.[Company] And OT.[EmployeeId]=PM.[EmployeeId]  " +
            "  Where Not([ApproveDate] is Null) And IsNull([Overtime_pay],'Y')='Y' And DatePart(Year,[OverTime_Date])=" + YLSelect.SelectADYear +
            " Group By OT.[Company],OT.[EmployeeId],[EmployeeName],IsNull(OT.[DeptId],PM.[DeptId]),DatePart(Year,[OverTime_Date]) " +
            " ) OT ";

        Ssql += string.Format(" Where [Company] = '{0}'", SearchList1.CompanyValue.Replace("%%", "").Trim());

        //部門
        if (SearchList1.DepartmentValue.Replace("%%", "").Length > 0)
        {
            Ssql += string.Format(" And DeptId = '{0}'", SearchList1.DepartmentValue.Replace("%%", "").Trim());
        }
        //員工
        if (SearchList1.EmployeeValue.Replace("%%", "").Length > 0)
        {
            Ssql += string.Format(" And EmployeeId = '{0}'", SearchList1.EmployeeValue.Replace("%%", "").Trim());
        }

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "EmployeeId";
            if (ViewState["SortDirectionDesc"] == null)
                ViewState["SortDirectionDesc"] = false;
        }

        strSort = "Order By " + SortExpression + ((bool)ViewState["SortDirectionDesc"] ? " Desc" : "");

        DataTable theDT = _MyDBM.ExecuteDataTable(Sqlcommand1 + Sqlcommand2 + Sqlcommand3 + Ssql + strSort);

        py.AfterQuery(DeCodeKey);

        return theDT;
    }

    private void BindData()
    {
        BindData("");
    }
    private void BindData(string SortExpression)
    {
        GridView1.DataSource = GetTable(SortExpression);
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
                if (i < 2)
                    e.Row.Cells[i].Style.Add("text-align", "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Visible = true;
        }
    }

    public Decimal[] decAmount = new Decimal[30];

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strPan = "&nbsp;&nbsp;", strValue = "";

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

                    if (DataBinder.Eval(e.Row.DataItem, "DeptName") != null)
                        strValue = DataBinder.Eval(e.Row.DataItem, "DeptName").ToString();
                    e.Row.Cells[0].Text = strPan + e.Row.Cells[0].Text + "-" + strValue + strPan;
                    if (DataBinder.Eval(e.Row.DataItem, "EmployeeName") != null)
                        strValue = DataBinder.Eval(e.Row.DataItem, "EmployeeName").ToString();
                    e.Row.Cells[1].Text = strPan + e.Row.Cells[1].Text + "-" + strValue + strPan;

                    //for (int i = 3; i < e.Row.Cells.Count; i++)
                    //{
                    //    e.Row.Cells[i].Style.Add("text-align", "right");                       
                    //}
                }
                break;
            case DataControlRowType.Header:

                break;
            case DataControlRowType.Footer:

                break;
        }
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["SortDirectionDesc"] == null)
            ViewState["SortDirectionDesc"] = false;
        BindData(e.SortExpression);
        ViewState["SortDirectionDesc"] = (!((bool)ViewState["SortDirectionDesc"]));
    }
}
