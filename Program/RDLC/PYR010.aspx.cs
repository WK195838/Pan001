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

public partial class PYR010: System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR010";
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
        BounsDate.CssClass = "JQCalendar";

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
            CodeList1.SetCodeList("PY#Bonus");
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
        // 清除之前的資料
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.Visible = false;
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請先選擇公司!";         
        }
        else if (rbList.SelectedValue.Equals("C") && CodeList1.SelectedCode.Trim().Length==0)
        {
            LabMsg.Text = "請先選擇獎金別!";
        }
        else
        {
            int retResult = 0;
            LabMsg.Text = "";
            if (!rbList.SelectedValue.Equals("C"))
            {
                Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
                sYM = Payroll.CheckSalaryYM(SearchList1.CompanyValue, SalaryYM1.SelectSalaryYM, "0");
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
                    retResult = 1;
                    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行確認作業! 此薪津簽收表僅供參考";
                }
            }

            string strTitle = (retResult.Equals(1) ? "待確認" : "") + rbList.SelectedItem.Text;
            DataTable Table1 = new DataTable();

            if (retResult >= 0)
            {
                Navigator1.Visible = false;
                GridView1.Visible = false;

                Decimal tempDec = 0, Pamount = 0;                

                if (rbList.SelectedValue.Equals("A"))
                {
                    Table1 = GetTable("");
                    if (Table1 != null)
                        for (int i = 0; i < Table1.Rows.Count; i++)
                        {
                            //找出部門名稱做為群組依據
                            Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());

                            Pamount = Convert.ToDecimal(Table1.Rows[i]["BaseSalary"].ToString());

                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Other01"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["OtherWT"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["OtherNT"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["WT_Overtime_fee"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["NT_Overtime_fee"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Attendance"].ToString());
                            
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["LeaveDeduction"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["LI_Fee"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["HI_Fee"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["FB_Fee"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["TAX"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["Parking_Fee"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["WR_Fee"].ToString());
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["OtherNTP"].ToString());

                            Pamount = (Pamount > 0) ? Pamount : 0;
                            changePay(Table1, i, Pamount);
                        }
                }
                else if (rbList.SelectedValue.Equals("B"))
                {
                    Table1 = GetTable("");
                    Payroll thePayroll = new Payroll();

                    if (Table1 != null)
                        for (int i = 0; i < Table1.Rows.Count; i++)
                        {
                            //找出部門名稱做為群組依據
                            Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());

                            Pamount = Convert.ToDecimal(Table1.Rows[i]["Non01"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non02"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non03"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non04"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non05"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non06"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non07"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non08"].ToString());
                            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non09"].ToString());

                            Pamount -= (decimal)(Pamount / 240 * thePayroll.GetLeaveHours(SearchList1.CompanyValue, Table1.Rows[i]["EmployeeId"].ToString(), SalaryYM1.SelectSalaryYM));
                            Pamount -= Convert.ToDecimal(Table1.Rows[i]["NonM10"].ToString());

                            Pamount = (Pamount > 0) ? Pamount : 0;
                            changePay(Table1, i, Pamount);
                        }
                }
                else if (rbList.SelectedValue.Equals("C"))
                {
                    Table1 = GetBouns("");
                    if (Table1 != null)
                        for (int i = 0; i < Table1.Rows.Count; i++)
                        {
                            //找出部門名稱做為群組依據
                            Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());

                            Pamount = Convert.ToDecimal(Table1.Rows[i]["PayAmount"].ToString());

                            Pamount = (Pamount > 0) ? Pamount : 0;
                            changePay(Table1, i, Pamount);
                        }
                    if (CodeList1.SelectedCode.Trim().Length > 0)
                        strTitle = CodeList1.SelectedCodeName.Trim();
                }

                if (Table1 == null)
                    retResult = -1;
            }

            if (retResult >= 0)
            {
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("PayRoll01", Table1));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(SearchList1.CompanyValue)));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SalaryYM", SalaryYM1.SelectSalaryYMName));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SalaryKind", strTitle));
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

    private void changePay(DataTable theTable, int rowindex, Decimal Pamount)
    {
        Decimal tempDec = 0;
        theTable.Rows[rowindex]["PayAmount"] = Pamount;

        theTable.Rows[rowindex]["Pay1000"] = (Pamount - Pamount % 1000) / 1000;

        tempDec = Pamount % 1000;
        theTable.Rows[rowindex]["Pay0500"] = (tempDec - tempDec % 500) / 500;

        tempDec = Pamount % 500;
        theTable.Rows[rowindex]["Pay0100"] = (tempDec - tempDec % 100) / 100;

        tempDec = Pamount % 100;
        theTable.Rows[rowindex]["Pay0050"] = (tempDec - tempDec % 50) / 50;

        tempDec = Pamount % 50;
        theTable.Rows[rowindex]["Pay0010"] = (tempDec - tempDec % 10) / 10;

        tempDec = Pamount % 10;
        theTable.Rows[rowindex]["Pay0005"] = (tempDec - tempDec % 5) / 5;

        theTable.Rows[rowindex]["Pay0001"] = Pamount % 5;                
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

    private DataTable GetBouns(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PYR010" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(DeCodeKey);        
        string strSort = " Order By PRH.Company,IsNull(DeptId,'ZZZ'),PRH.EmployeeId";
        string strROWID = " 0 ";
        //檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        if (_MyDBM.ServerVersion().StartsWith("08."))
            strROWID = " 0 ";
        else
            strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";

        Ssql = " SELECT [Company],[EmployeeId] ,[EmployeeName],[CostId],[CostName]" +
            ",(Select DeptId From Personnel_Master Where [Company]=[BonusMaster].[Company] And [EmployeeId]=[BonusMaster].[EmployeeId]) As [DeptId]" +
            ",'' As [DeptGroup]" +            
            ",Sum(" + DeCodeKey + "([CostAMT])-" + DeCodeKey + "([Pay_AMT])) As PayAmount" +
            //"," + strROWID + " as ROWID " +
            ",0 as Pay1000,0 as Pay0500,0 as Pay0100,0 as Pay0050,0 as Pay0010,0 as Pay0005,0 as Pay0001 " +
            " FROM [BonusMaster] " +
            " Where Company='" + SearchList1.CompanyValue.Trim() + "'";

        //員工
        if (SearchList1.EmployeeValue.Replace("%%", "").Length > 0)
        {
            Ssql += string.Format(" And EmployeeId = '{0}'", SearchList1.EmployeeValue.Replace("%%", "").Trim());
        }
        //計薪年月
        if (BounsDate.Text.Trim().Length == 0)
        {            
            Ssql += string.Format(" And Convert(varchar,AmtDate,112) like '{0}%' ", SalaryYM1.SelectSalaryYM.Trim());
        }

        if (BounsDate.Text.Trim().Length > 0)
        {
            //Ssql += string.Format(" And Convert(varchar,AmtDate,111) = '{0}'", _UserInfo.SysSet.ToADDate(BounsDate.Text));
            Ssql += string.Format(" And Convert(varchar,AmtDate,111) in ('{0}','{1}')", _UserInfo.SysSet.ToADDate(BounsDate.Text), BounsDate.Text);
        }

        if (CodeList1.SelectedCode.Trim().Length > 0)
        {            
            Ssql += string.Format(" And CostId = {0}", CodeList1.SelectedCode.Trim());
        }
        
        if (string.IsNullOrEmpty(SortExpression))
        {
            SortExpression = "EmployeeId";
            if (ViewState["SortDirectionDesc"] == null)
                ViewState["SortDirectionDesc"] = false;
        }
        Ssql += " Group by [Company],[EmployeeId],[EmployeeName],[CostId],[CostName] ";
        
        //部門
        if (SearchList1.DepartmentValue.Replace("%%", "").Length > 0)
        {
            Ssql = " Select * from (" + Ssql + ") BM ";
            Ssql += string.Format(" Where DeptId = '{0}'", SearchList1.DepartmentValue.Replace("%%", "").Trim());
        }
        strSort = "Order By " + SortExpression + ((bool)ViewState["SortDirectionDesc"] ? " Desc" : "");

        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + strSort);

        py.AfterQuery(DeCodeKey);

        return theDT;
    }

    private DataTable GetTable(string SortExpression)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PYR010" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(DeCodeKey);
        string PWDorPHD = "PayrollWorking";
        string Sqlcommand1 = "", Sqlcommand2 = "", Sqlcommand3 = "";
        string strSort = " Order By PRH.Company,IsNull(DeptId,'ZZZ'),PRH.EmployeeId";
        string strROWID = " 0 ";
        //檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        if (_MyDBM.ServerVersion().StartsWith("08."))
            strROWID = " 0 ";
        else
            strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";

        Sqlcommand1 = "SELECT [DeptId],PRH.[Company],PRH.[EmployeeId],[SalaryYM],[PeriodCode],[Paydays],[LeaveHours_deduction],[TaxRate],[ResignCode] " +
            ",[HI_Person],[WT_Overtime],[NT_Overtime],[OnWatch],[Dependent1_IDNo],[Dependent2_IDNo],[Dependent3_IDNo] " +
            ",(Select EmployeeName From Personnel_Master Where [Company]=PRH.[Company] And [EmployeeId]=PRH.[EmployeeId]) As [EmployeeName] " +
            "," + DeCodeKey + "([BaseSalary]) As BaseSalary" +
            "," + DeCodeKey + "([LI_Fee]) As LI_Fee " +
            "," + DeCodeKey + "([HI_Fee]) As HI_Fee " +
            "," + DeCodeKey + "([WT_Overtime_Fee]) As [WT_Overtime_fee]" +
            "," + DeCodeKey + "([NT_Overtime_Fee]) As [NT_Overtime_fee]" +
            "," + DeCodeKey + "([BaseSalary])+" + DeCodeKey + "([WT_Overtime_Fee])+" + DeCodeKey + "([NT_Overtime_Fee]) As [PAmount]" +
            ",0 As Other01,0 As OtherWT,0 As OtherNT,0 As Parking_Fee,0 As WR_Fee,0 As OtherNTP " +
            ",0 As MAmount" +
            ",0 As PayAmount" +
            ",0 As WT_Amount" +
            ",(" + DeCodeKey + "([BaseSalary])-" + GetDetailSQL(DeCodeKey, PWDorPHD, "01", rbList.SelectedValue) + ") As LeaveDeduction" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "03", rbList.SelectedValue) + " As TAX" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "07", rbList.SelectedValue) + " As FB_Fee" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "16", rbList.SelectedValue) + " As Pension" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "21", rbList.SelectedValue) + " As Attendance";

        Sqlcommand2 = "," + GetDetailSQL(DeCodeKey, PWDorPHD, "22", rbList.SelectedValue) + " As Non01" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "23", rbList.SelectedValue) + " As Non02" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "24", rbList.SelectedValue) + " As Non03" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "25", rbList.SelectedValue) + " As Non04" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "26", rbList.SelectedValue) + " As Non05" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "27", rbList.SelectedValue) + "+" + GetDetailSQL(DeCodeKey, PWDorPHD, "02", rbList.SelectedValue) + " As Non06" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "28", rbList.SelectedValue) + " As Non07" +
            ",0 As Non08" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "08", rbList.SelectedValue) + "+" + GetDetailSQL(DeCodeKey, PWDorPHD, "09", rbList.SelectedValue) + " As Non09" +
            "," + GetDetailSQL(DeCodeKey, PWDorPHD, "29", rbList.SelectedValue) + " As NonM10" +
            "," + strROWID + " as ROWID " +
            ",'' As [DeptGroup]" +
            ",0 as Pay1000,0 as Pay0500,0 as Pay0100,0 as Pay0050,0 as Pay0010,0 as Pay0005,0 as Pay0001 ";

        Sqlcommand3 = " FROM [" + PWDorPHD + "_Heading] PRH " +
            " left join (SELECT [Company],[EmployeeId] as PMEmployeeId,[DeptId] FROM [Personnel_Master]) PM On PRH.[Company]=PM.[Company] And PRH.[EmployeeId]=PM.[PMEmployeeId]" +
            " Where PRH.Company='" + SearchList1.CompanyValue.Trim() + "'";
        Ssql = "";
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
        Decimal tempDec = 0, Pamount = 0;
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
                    GridView theGV = ((GridView)sender);
                    if (iSeq == 0)
                    {                        
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
                            if (i == 3)
                            {                                
                                Pamount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BaseSalary").ToString());

                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Other01").ToString());
                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OtherWT").ToString());
                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OtherNT").ToString());
                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "WT_Overtime_fee").ToString());
                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NT_Overtime_fee").ToString());
                                Pamount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Attendance").ToString());

                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "LI_Fee").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "HI_Fee").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "FB_Fee").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TAX").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Parking_Fee").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "WR_Fee").ToString());
                                Pamount -= Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OtherNTP").ToString());

                                Pamount = (Pamount > 0) ? Pamount : 0;
                                tempDec = Pamount;
                            }
                            else if (i == 4)//1000
                            {
                                tempDec = (Pamount - Pamount % 1000) / 1000;
                            }
                            else if (i == 5)//500
                            {
                                tempDec = Pamount % 1000;
                                tempDec = (tempDec - tempDec % 500) / 500;
                            }
                            else if (i == 6)//100
                            {
                                tempDec = Pamount % 500;
                                tempDec = (tempDec - tempDec % 100) / 100;
                            }
                            else if (i == 7)//50
                            {
                                tempDec = Pamount % 100;
                                tempDec = (tempDec - tempDec % 50) / 50;
                            }
                            else if (i == 8)//10
                            {
                                tempDec = Pamount % 50;
                                tempDec = (tempDec - tempDec % 10) / 10;
                            }
                            else if (i == 9)//5
                            {
                                tempDec = Pamount % 10;
                                tempDec = (tempDec - tempDec % 5) / 5;
                            }
                            else if (i == 10)//1
                            {
                                tempDec = Pamount % 5;                                
                            }

                            decAmount[i] += tempDec;

                            e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
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
                        e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
                    }
                    catch { }
                }                
                break;
        }
    }

    private string GetDetailSQL(string DeCodeKey, string PWDorPHD, string SalaryItem, string PeriodCode)
    {
        string tempSQL = " IsNull((Select Sum(" + DeCodeKey + "([SalaryAmount])) from [" + PWDorPHD + "_Detail] " +
            " Where PRH.[Company]=[Company] And PRH.[EmployeeId]=[EmployeeId] And PRH.[SalaryYM]=[SalaryYM] " +
            " And [PeriodCode] in ('" + PeriodCode + "') And [SalaryItem]='" + SalaryItem + "'),0) ";
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
