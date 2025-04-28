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

public partial class Payroll013_D : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM013";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    Decimal TotalSalary = 0, dOtherNT = 0, dOtherWT = 0;
    string sKind = "W";
    string strPan = "&nbsp;&nbsp;&nbsp;&nbsp;";
    string PayrollTable = "PayrollWorking";

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";

        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "PayrolltestWorking";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'ePayroll' And RTrim(ProgramPath)='Payroll/Payroll013.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Navigator1.BindGridView = GridView1;

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

            //Company=01&EmployeeId=001&SYM=201009&SPeriod=0
            PeriodList1.defaulPeriod();

            if (Request["Kind"] != null)
            {
                sKind = Request["Kind"].ToString().Trim();
                if (!sKind.Equals("W"))
                {
                    StyleTitle1.Title = "已確認薪資查詢";
                }
            }
            if (PayrollTable.Contains("test"))
                StyleTitle1.Title += "(測試模式)";

            if (Request["Company"] != null)
            {
                PayrollLt.Company = Request["Company"].ToString().Trim();
                CompanyList1.SelectValue = PayrollLt.Company;
                CompanyList1.Enabled = false;
            }

            if (Request["EmployeeId"] != null)
            {
                PayrollLt.EmployeeId = Request["EmployeeId"].ToString().Trim();
            }

            SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);
            if (Request["SYM"] != null)
            {
                SalaryYM1.SelectSalaryYM = Request["SYM"].ToString().Trim();
            }

            if (Request["SPeriod"] != null)
            {
                PayrollLt.PeriodCode = Request["SPeriod"].ToString().Trim();
                PeriodList1.selectIndex = int.Parse(PayrollLt.PeriodCode);
            }

            if (PayrollLt.Company.Length * PayrollLt.EmployeeId.Length > 0)
            {//取得中文姓名
                lblEmpID.Text = PayrollLt.EmployeeId + "-" + DBSetting.PersonalName(PayrollLt.Company, PayrollLt.EmployeeId);
            }            
        }
        else
        {
            LabMsg.Text = "";            
        }
        
        SalaryYM1.Enabled = false;
        PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());

        PeriodList1.Enabled = false;

        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.DCK" + DateTime.Now.ToString("yyyyMMddmmss");        
        py.BeforeQuery(PayrollLt.DeCodeKey);
        BindData();
        BindSalaryData();
        GetOtherData();
        py.AfterQuery(PayrollLt.DeCodeKey);

        if (GridView1.PageCount > 1)
            Navigator1.Visible = true;
        else
            Navigator1.Visible = false;        
    }

    private void BindSalaryData()
    {
        Label tempTitleLab, tempLab;
        string strColumnName, strTemp;
        //" + PayrollLt.DeCodeKey + "(IsNull(SalaryAmount,'0')) As DeCodeSalaryAmount
        Ssql = "SELECT [Company],[EmployeeId],[SalaryYM],[PeriodCode],[Paydays],[LeaveHours_deduction],[TaxRate],[ResignCode]" +
            ",[HI_Person],[WT_Overtime],[NT_Overtime],[OnWatch],[Dependent1_IDNo],[Dependent2_IDNo],[Dependent3_IDNo]" +
            "," + PayrollLt.DeCodeKey + "([BaseSalary]) As BaseSalary" +
            "," + PayrollLt.DeCodeKey + "([LI_Fee]) As LI_Fee " +
            "," + PayrollLt.DeCodeKey + "([HI_Fee]) As HI_Fee " +
            "," + PayrollLt.DeCodeKey + "([NT_P]) As NT_P" +
            "," + PayrollLt.DeCodeKey + "([WT_P_Salary])As WT_P_Salary" +
            "," + PayrollLt.DeCodeKey + "([NT_M]) As NT_M " +
            "," + PayrollLt.DeCodeKey + "([WT_P_Bonus]) As WT_P_Bonus" +
            "," + PayrollLt.DeCodeKey + "([WT_M_Salary]) As WT_M_Salary" +
            "," + PayrollLt.DeCodeKey + "([WT_M_Bonus]) As WT_M_Bonus" +
            "," + PayrollLt.DeCodeKey + "([P1_borrowing]) As P1_borrowing" +
            "," + PayrollLt.DeCodeKey + "([WT_Overtime_Fee]) As WT_Overtime_Fee" +
            "," + PayrollLt.DeCodeKey + "([NT_Overtime_Fee]) As NT_Overtime_Fee" +
            "," + PayrollLt.DeCodeKey + "([OnWatch_Fee]) As OnWatch_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent1_HI_Fee]) As Dependent1_HI_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent2_HI_Fee]) As Dependent2_HI_Fee" +
            "," + PayrollLt.DeCodeKey + "([Dependent3_HI_Fee]) As Dependent3_HI_Fee" +
            " FROM " + (sKind.Equals("W") ? ""+PayrollTable+"" : "Payroll_History") + "_Heading Where Company='" + PayrollLt.Company.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And SalaryYM = {0}", PayrollLt.SalaryYM.ToString());
        //計薪期別
        Ssql += string.Format(" And PeriodCode = '{0}'", PayrollLt.PeriodCode.Trim());
        //員工帳號
        Ssql += string.Format(" And EmployeeId = '{0}'", PayrollLt.EmployeeId.Trim());

        //if (tbEmployeeName.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And (EmployeeName like '%{0}%' Or EnglishName like '%{0}%' Or TitleCode like '%{0}%')", tbEmployeeName.Text);
        //}

        //SDS_GridView.SelectCommand = Ssql + " Order By Company,EmployeeId";
        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + " Order By Company,EmployeeId");

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                for (int i = 0; i < theDT.Columns.Count; i++)
                {
                    strTemp = "N0";
                    strColumnName = theDT.Columns[i].ColumnName.Trim();
                    tempTitleLab = (Label)this.Form.FindControl("lblTitle_" + strColumnName);
                    if (tempTitleLab != null)
                    {
                        Ssql = "Select dbo.GetColumnTitle('" + (sKind.Equals("W") ? ""+PayrollTable+"" : "Payroll_History") + "_Heading','" + tempTitleLab.Text + "')";
                        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                        if (DT.Rows.Count > 0)
                        {//欄位標題名稱
                            tempTitleLab.Text = DT.Rows[0][0].ToString().Trim();
                        }

                        if (tempTitleLab.Text.Contains("時數"))
                            strTemp = "N1";
                    }
                    tempLab = (Label)this.Form.FindControl("lbl_" + strColumnName);
                    if (tempLab != null)
                    {                        
                        tempLab.Text = ((Decimal)theDT.Rows[0][strColumnName]).ToString(strTemp) + strPan;
                    }
                }
            }
        }
    }

    private void BindData()
    {
        Ssql = "SELECT PWD.*," + PayrollLt.DeCodeKey + "(PWD.SalaryAmount) As DeCodeSalaryAmount ," + PayrollLt.DeCodeKey + "(PMO.SalaryAmount) As OtherAmount," + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)+" + PayrollLt.DeCodeKey + "(PMO.SalaryAmount) As TotalAmount, SalaryName " +
            " ,(Case PMType When 'A' then '加項' else '減項' end) As PMType" +
            " ,NWTax,RegularPay" +
            " ,(Case PMType When 'A' then (" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)+" + PayrollLt.DeCodeKey + "(PMO.SalaryAmount)) else (0-(" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)+" + PayrollLt.DeCodeKey + "(PMO.SalaryAmount))) end) As OtherNWSalary" +
            " FROM " + (sKind.Equals("W") ? ""+PayrollTable+"" : "Payroll_History") + "_Detail PWD " +
            " Left join Payroll_Master_OtherDetail PMO On  " +
            " PWD.Company = PMO.Company And PWD.EmployeeId = PMO.EmployeeId And PWD.SalaryYM = PMO.SalaryYM And PWD.PeriodCode = PMO.PeriodCode And PWD.SalaryItem = PMO.SalaryItem  " +
            " Left join SalaryStructure_Parameter SP On PWD.SalaryItem = SP.SalaryId ";

        Ssql = "SELECT PWD.*," + PayrollLt.DeCodeKey + "(PWD.SalaryAmount) As DeCodeSalaryAmount ,0 As OtherAmount," + PayrollLt.DeCodeKey + "(PWD.SalaryAmount) As TotalAmount, SalaryName " +
        " ,(Case PMType When 'A' then '加項' else '減項' end) As PMType" +
        " ,NWTax,RegularPay" +
        " ,(Case PMType When 'A' then (" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)) else (0-(" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount))) end) As OtherNWSalary" +
        " FROM " + (sKind.Equals("W") ? "" + PayrollTable + "" : "Payroll_History") + "_Detail PWD " +
        " Left join SalaryStructure_Parameter SP On PWD.SalaryItem = SP.SalaryId ";

        //公司
        Ssql += " Where PWD.Company='" + CompanyList1.SelectValue.Trim() + "'";
        //計薪年月
        Ssql += string.Format(" And PWD.SalaryYM = {0}", SalaryYM1.SelectSalaryYM.Trim());
        //計薪期別
        Ssql += string.Format(" And PWD.PeriodCode = '{0}'", PeriodList1.selectIndex.ToString());

        if (PayrollLt.EmployeeId.Length > 0)
        {
            Ssql += string.Format(" And PWD.EmployeeId = '{0}'", PayrollLt.EmployeeId);
        }
        
        //SDS_GridView.SelectCommand = Ssql + " Order By Company,EmployeeId";
        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql + " Order By PWD.Company,PWD.EmployeeId,PWD.SalaryItem");

        GridView1.DataSource = theDT;
        GridView1.DataBind();
                
        Navigator1.BindGridView = GridView1; 
        Navigator1.DataBind();
    }

    private void GetOtherData()
    {
        Payroll thePayroll = new Payroll();
        decimal dFixedSalary = 0, dNonFixedAmount = 0, dMAmount = 0;

        //取得經常性薪資項目
        dFixedSalary = thePayroll.GetPersonalYMRegularPay(PayrollLt);
        
        //需減去請假扣款
        //dFixedSalary -= (decimal)(thePayroll.GetPersonalHoursSalary(PayrollLt) * thePayroll.GetLeaveHours(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString()));

        #region 所得稅計算
        //int TaxSum = thePayroll.GetTaxSum(PayrollLt, 1);
        //IncomeTax.Tax mTax = IncomeTax.GetIncomeTax(TaxSum, PayrollLt);
        #endregion

        //固定金額
        lbl_RegularPay.Text = dFixedSalary.ToString("N0") + strPan;

        //非固定金額:目前應為其它應稅項目,不含其他扣款   
        dOtherWT = Convert.ToDecimal(thePayroll.GetPayrollNWTSalary(PayrollLt, "Y"));
        dNonFixedAmount = dOtherWT;
        lbl_NonFixedSalsry.Text = dNonFixedAmount.ToString("N0") + strPan;
        //免稅所得=其它免稅:含免稅調整,免稅加班
        dOtherNT = Convert.ToDecimal(thePayroll.GetPayrollNWTSalary(PayrollLt, "N"));
        lbl_NT_P.Text = dOtherNT.ToString("N0") + strPan;

        //取得應發與應扣金額        
        dMAmount = GetMAmount();
        //應發金額
        lblSAmount.Text = (dFixedSalary + dOtherWT + dOtherNT - dMAmount).ToString("N0") + strPan;
        //應扣金額
        lblMAmount.Text = dMAmount.ToString("N0") + strPan;     
        
        //Label theLabel;
        //for (int i = 1; i < 4; i++)
        //{
        //    theLabel = (Label)this.Form.FindControl("lbl_" + i.ToString().PadLeft(2, '0'));
        //    if (theLabel != null)
        //    {
        //        theLabel.Text = thePayroll.GetPersonalSalaryItem(PayrollLt, i.ToString().PadLeft(2, '0'));
        //        try
        //        {
        //            theLabel.Text = Convert.ToDecimal(theLabel.Text).ToString("N0");
        //        }
        //        catch { }
        //        theLabel.Text += strPan;
        //    }
        //}
    }
    
 
    /// <summary>
    /// 應扣金額
    /// </summary>
    /// <returns></returns>
    private decimal GetMAmount()
    {
        decimal dMAmount = 0;

        try
        {//所得稅
            dMAmount += Convert.ToDecimal(lbl_03.Text.Replace(",", "").Replace(strPan, ""));
        }
        catch { }
        try
        {//健保費
            dMAmount += Convert.ToDecimal(lbl_04.Text.Replace(",", "").Replace(strPan, ""));
        }
        catch { }
        try
        {//勞保費
            dMAmount += Convert.ToDecimal(lbl_05.Text.Replace(",", "").Replace(strPan, ""));
        }
        catch { }
        try
        {//福利金
            dMAmount += Convert.ToDecimal(lbl_07.Text.Replace(",", "").Replace(strPan, ""));
        }
        catch { }
        try
        {//其他扣款
            dMAmount += Convert.ToDecimal(lbl_21.Text.Replace(",", "").Replace(strPan, ""));
        }
        catch { }
        return dMAmount; 
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                if (i < 2)
                    e.Row.Cells[i].Style.Add("text-align", "left");
                else
                    e.Row.Cells[i].Style.Add("text-align", "right");
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
        Decimal tempDec = 0;

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
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    for (int i = 2; i < e.Row.Cells.Count; i++)
                    {
                        try
                        {
                            tempDec = Convert.ToDecimal(e.Row.Cells[i].Text);
                            if (i == (e.Row.Cells.Count - 1))
                            {
                                TotalSalary += tempDec;
                            }
                            e.Row.Cells[i].Text = tempDec.ToString("N0");
                        }
                        catch { }
                        
                    }
                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    try
                    {
                        tempDec = Convert.ToDecimal(e.Row.Cells[3].Text);                        
                    }
                    catch { }

                    switch (e.Row.Cells[0].Text.Trim())
                    {
                        case "01"://底薪
                            lbl_01.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "02"://餐費津貼
                            lbl_02.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "20"://職務津貼
                            lbl_20.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "21"://其他扣款
                            if (tempDec > 0)
                                lbl_21.ForeColor = System.Drawing.Color.Red;
                            else
                                lbl_21.ForeColor = System.Drawing.Color.Blue;
                            lbl_21.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "03"://所得稅
                            lbl_03.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "04"://健保
                            lbl_04.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "05"://勞保
                            lbl_05.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "07"://福利金
                            lbl_07.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "08"://應稅加班費
                            lbl_08.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "09"://免稅加班費
                            lbl_09.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "22"://其它應稅
                            lbl_22.Text = tempDec.ToString("N0") + strPan;
                            break;
                        case "23"://其它免稅
                            lbl_23.Text = tempDec.ToString("N0") + strPan;
                            break;
                        
                    }
                }

                break;

            case DataControlRowType.Header:


                break;
        }
    }
}
