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

public partial class PYR001PIC: System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR001";
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

            SalaryYM1.defaultlist();
            ReportViewer1.Visible = false;
        }
        else
        {
            LabMsg.Text = "";
            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);

            if (Navigator1.Visible)
                BindData();
        }

        ScriptManager1.RegisterPostBackControl(btnQuery);
        ScriptManager1.RegisterPostBackControl(btReport);  
    }

    protected void btReport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            ReportViewer1.Visible = false;
            int retResult = 0;
            Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
            sYM = Payroll.CheckSalaryYM(SearchList1.CompanyValue, SalaryYM1.SelectSalaryYM, "0");
            if (sYM.isControl == false)
            {//未經控管
                retResult = -2;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無法產生報表";
            }
            else if (string.IsNullOrEmpty(sYM.DraftDate))
            {
                //指定公司於指定計薪年月未經試算
                retResult = -3;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無法產生報表";
            }
            else if (string.IsNullOrEmpty(sYM.ConfirmDate) && RpKind.SelectedValue.Equals("Payroll_History"))
            {
                //指定公司於指定計薪年月已完成確認試算,不可再進行確認
                retResult = -4;
                LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行確認作業! 無法產生報表";
            }
            else
            {
                // 清除之前的資料
                ReportViewer1.LocalReport.DataSources.Clear();
                Navigator1.Visible = false;
                GridView1.Visible = false;
                bool rebind = false;
                DataTable Table1 = new DataTable();

                Table1 = (DataTable)GridView1.DataSource;
                if (Table1 == null)
                    rebind = true;
                else if (Table1.Rows.Count == 0)
                    rebind = true;
                if (rebind.Equals(true))
                {
                    BindData();
                    Table1 = (DataTable)GridView1.DataSource;
                    if (Table1 != null)
                        for (int i = 0; i < Table1.Rows.Count; i++)
                        {
                            //找出部門名稱做為群組依據
                            Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());

                        }
                    else
                        retResult = -1;
                }

                if (retResult >= 0)
                {
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("PayRoll01", Table1));
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(SearchList1.CompanyValue)));
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SalaryYM", SalaryYM1.SelectSalaryYMName));
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReportKind", "(" + RpKind.SelectedItem + ")"));
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
    }

    /// <summary>
    /// 薪資查詢
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        
        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            ReportViewer1.Visible = false;
            int retResult = 0;
            Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
            sYM = Payroll.CheckSalaryYM(SearchList1.CompanyValue, SalaryYM1.SelectSalaryYM, "0");
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
            else if (string.IsNullOrEmpty(sYM.ConfirmDate) && RpKind.SelectedValue.Equals("Payroll_History"))
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
    }
    private void BindData(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PYR001PIC" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(DeCodeKey);
        string PWDorPHD = RpKind.SelectedValue.Equals("W") ? "Working" : "_History";
        string strSort = " Order By PRH.Company,IsNull(DeptId,'ZZZ'),PRH.EmployeeId";
        string strROWID = " 0 ";
        //檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        if (_MyDBM.ServerVersion().StartsWith("08."))
            strROWID = " 0 ";
        else
            strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";        

        Ssql = "SELECT [DeptId],PRH.[Company],PRH.[EmployeeId],[SalaryYM],[PeriodCode],[Paydays],[LeaveHours_deduction],[TaxRate],[ResignCode] " +
            ",[HI_Person],[WT_Overtime],[NT_Overtime],[OnWatch],[Dependent1_IDNo],[Dependent2_IDNo],[Dependent3_IDNo] " +
            ",(Select EmployeeName From Personnel_Master Where [Company]=PRH.[Company] And [EmployeeId]=PRH.[EmployeeId]) As [EmployeeName] " +
            ",(" + DeCodeKey + "([BaseSalary])+" + GetDetailSQL(DeCodeKey, "_Master_Special", "01") + ") As BaseSalary" +            
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "04") + " As HI_Fee " +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "05") + " As LI_Fee " +
            //"," + DeCodeKey + "([NT_P]) " +
            //"," + DeCodeKey + "([WT_P_Salary])" +
            //"," + DeCodeKey + "([NT_M])" +
            //"," + DeCodeKey + "([WT_P_Bonus])" +
            //"," + DeCodeKey + "([WT_M_Salary])" +
            //"," + DeCodeKey + "([WT_M_Bonus])" +
            //"," + DeCodeKey + "([P1_borrowing])" +            
            //"," + DeCodeKey + "([WT_Overtime_fee])" +
            //"," + DeCodeKey + "([NT_Overtime_fee])" +
            //"," + DeCodeKey + "([OnWatch_Fee])" +
            //"," + DeCodeKey + "([Dependent1_HI_Fee])" +
            //"," + DeCodeKey + "([Dependent2_HI_Fee])" +
            //"," + DeCodeKey + "([Dependent3_HI_Fee])" +           
            ",0 As Other01,0 As OtherWT,0 As OtherNT,0 As Parking_Fee,0 As WR_Fee,0 As OtherNTP " +
            ",0 As MAmount" +
            ",0 As PayAmount" +
            ",0 As WT_Amount" +
            ",(" + DeCodeKey + "([BaseSalary])+" + GetDetailSQL(DeCodeKey, "_Master_Special", "01") + "-" + GetDetailSQL(DeCodeKey, PWDorPHD, "01") + ") As LeaveDeduction" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "03") + " As TAX" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "07") + " As FB_Fee" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "08") + " As [WT_Overtime_fee]" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "09") + " As [NT_Overtime_fee]" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "16") + " As Pension" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "21") + " As Attendance" +
            ",(" + DeCodeKey + "([BaseSalary])+" + GetDetailSQL(DeCodeKey, PWDorPHD, "08") + "+" + GetDetailSQL(DeCodeKey, PWDorPHD, "09") + ") As [PAmount]" +
            "," + strROWID + " as ROWID " +
            ",'' As [DeptGroup]" +
            " FROM [Payroll" + PWDorPHD + "_Heading] PRH " +
            " left join (SELECT [Company],[EmployeeId] as PMEmployeeId,[DeptId] FROM [Personnel_Master]) PM On PRH.[Company]=PM.[Company] And PRH.[EmployeeId]=PM.[PMEmployeeId]" +
            " Where PRH.Company='" + SearchList1.CompanyValue.Trim() + "'";
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
        //計薪年月
        Ssql += string.Format(" And SalaryYM = {0}", SalaryYM1.SelectSalaryYM.Trim());
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", '0');

        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "EmployeeId";
            if (ViewState["SortDirectionDesc"] == null)
                ViewState["SortDirectionDesc"] = false;
        }
        
        strSort = "Order By " + SortExpression + ((bool)ViewState["SortDirectionDesc"] ? " Desc" : "");            
        
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
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Visible = true;            
        }
    }

    public Decimal[] decAmount = new Decimal[30];
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Decimal tempDec = 0, Pamount = 0, Mamount = 0, NTAmount = 0;
        int iSeq = 0;
        string strPan = "&nbsp;&nbsp;";

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
                    //處理項次
                    iSeq = Convert.ToInt32(e.Row.Cells[0].Text);
                    if (iSeq == 0)
                    {
                        GridView theGV = ((GridView)sender);
                        iSeq = theGV.PageIndex * theGV.PageSize + e.Row.RowIndex + 1;
                    }
                    e.Row.Cells[0].Text = strPan + iSeq.ToString() + strPan;

                    e.Row.Cells[1].Text = strPan + e.Row.Cells[1].Text + strPan;
                    e.Row.Cells[2].Text = strPan + e.Row.Cells[2].Text + strPan;

                    for (int i = 3; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Style.Add("text-align", "right");
                        try
                        {
                            tempDec = Convert.ToDecimal(e.Row.Cells[i].Text.Trim());
                            if (i > 2 && i < 10)
                            {
                                Pamount += tempDec;
                                if (i == 7 || i == 9)//加項免稅合計
                                    NTAmount += tempDec;
                            }
                            else if (i > 10 && i < 19)
                            {
                                Mamount += tempDec;
                                if (i == 11)//減項應稅
                                    NTAmount -= tempDec;
                            }
                            else if (i == 19)//扣項合計
                                tempDec = Mamount;
                            else if (i == 20)//實領金額
                            {
                                tempDec = Pamount - Mamount;
                                if (tempDec < 0)
                                    tempDec = 0;
                            }
                            else if (i == 21)//應稅金額
                                tempDec = Pamount - NTAmount;

                            decAmount[i] += tempDec;
                            if (i == 11)
                            {//可能為非整數
                                e.Row.Cells[i].Text = tempDec.ToString("N2").Replace(".00", "") + strPan;
                            }
                            else
                            {
                                e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
                            }

                        }
                        catch { }
                    }    
                }
                break;
            case DataControlRowType.Header:
                for (int i = 3; i < e.Row.Cells.Count; i++)
                {
                    decAmount[i] = 0;
                }                
                GridView1.ShowFooter = true;
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[2].Text = strPan + "小計" + strPan;
                for (int i = 3; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                    try
                    {
                        tempDec = decAmount[i];                        

                        if (i == 11)
                        {//可能為非整數
                            e.Row.Cells[i].Text = tempDec.ToString("N2").Replace(".00", "") + strPan;
                        }
                        else
                        {
                            e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
                        }
                    }
                    catch { }
                }                
                break;
        }
    }

    //取得薪資項目金額
    private string GetDetailSQL(string DeCodeKey,string PWDorPHD, string SalaryItem)
    {
        bool blIsSpecial = PWDorPHD.Equals("_Master_Special");
        string tempSQL = " IsNull((Select Sum(" + DeCodeKey + "([SalaryAmount])) from [Payroll" + PWDorPHD + (blIsSpecial ? "" : "_Detail") + "] " +
            " Where PRH.[Company]=[Company] And PRH.[EmployeeId]=[EmployeeId] And PRH.[SalaryYM]=[SalaryYM] " +
            " And [PeriodCode] in ('A') And [SalaryItem]='" + SalaryItem + "'),0) ";

        if (RpSalaryKind.SelectedValue.Length == 0 && blIsSpecial)
            tempSQL = " 0 ";
        else if (RpSalaryKind.SelectedValue.Length > 0 && !blIsSpecial)
        {
            tempSQL = "(" + tempSQL + "+" + GetDetailSQL(DeCodeKey, "_Master_Special", SalaryItem) + ")";
        }
        return tempSQL;
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["SortDirectionDesc"] == null)
            ViewState["SortDirectionDesc"] = false;
        BindData(e.SortExpression);
        ViewState["SortDirectionDesc"] = (!((bool)ViewState["SortDirectionDesc"]));
    }
}
