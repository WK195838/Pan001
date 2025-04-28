using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class RDLC_PYR013 : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR013";
    DBManger _MyDBM;
    string[] TableName = { "PersonnelAdjustment", "AdjustSalary_Master", "" };
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
        //btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
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
        }
        else
        {
            LabMsg.Text = "";
            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);

            if (Navigator1.Visible)
                BindData();
        }
        txtDateS.CssClass = "JQCalendar";
        txtDateE.CssClass = "JQCalendar";
        //.CssClass = "JQCalendar";
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            LabMsg.Text = "開始產生報表，請稍候...";
            int retResult = 0;
            //Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
            //sYM = Payroll.CheckSalaryYM(SearchList1.CompanyValue, SalaryYM1.SelectSalaryYM, "0");
            //if (sYM.isControl == false)
            //{//未經控管
            //    retResult = -2;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無法產生報表";
            //}
            //else if (string.IsNullOrEmpty(sYM.DraftDate))
            //{
            //    //指定公司於指定計薪年月未經試算
            //    retResult = -3;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無法產生報表";
            //}
            //else if (string.IsNullOrEmpty(sYM.ConfirmDate) && RpKind.SelectedValue.Equals("Payroll_History"))
            //{
            //    //指定公司於指定計薪年月已完成確認試算,不可再進行確認
            //    retResult = -4;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行確認作業! 無法產生報表";
            //}
            //else
            //{
            //    // 清除之前的資料
            //    ReportViewer1.LocalReport.DataSources.Clear();
            //    Navigator1.Visible = false;
            //    GridView1.Visible = false;

            //    Payroll thePayroll = new Payroll();
            //    Decimal Pamount = 0, Mamount = 0, tempDec = 0;
            DataTable Table1 = GetTable("");

            //    if (Table1 != null)
            //    {
            //        for (int i = 0; i < Table1.Rows.Count; i++)
            //        {
            //            //找出部門名稱做為群組依據
            //            Table1.Rows[i]["DeptGroup"] = Table1.Rows[i]["DeptId"].ToString() + "-" + DBSetting.DepartmentName(Table1.Rows[i]["Company"].ToString(), Table1.Rows[i]["DeptId"].ToString());

            //            Pamount = Convert.ToDecimal(Table1.Rows[i]["Non01"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non02"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non03"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non04"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non05"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non06"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non07"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non08"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non09"].ToString());
            //            Pamount += Convert.ToDecimal(Table1.Rows[i]["Non10"].ToString());
            //            //加項合計
            //            Table1.Rows[i]["PAmount"] = Pamount;
            //            //應稅金額                        
            //            if (!decimal.TryParse(Table1.Rows[i]["NT_Overtime_fee"].ToString(), out Mamount))
            //                Mamount = 0;
            //            //伙食費
            //            if (!decimal.TryParse(Table1.Rows[i]["Non00"].ToString(), out tempDec))
            //                tempDec = 0;
            //            Table1.Rows[i]["WT_Amount"] = ((Pamount - Mamount - tempDec) >= 0) ? (Pamount - Mamount - tempDec) : 0;

            //            Table1.Rows[i]["LeaveDeduction"] = Table1.Rows[i]["Non11"];
            //            Mamount = Convert.ToDecimal(Table1.Rows[i]["Non11"].ToString());
            //            Mamount += Convert.ToDecimal(Table1.Rows[i]["TAX"].ToString());
            //            //減項合計
            //            Table1.Rows[i]["MAmount"] = Mamount;

            //            Table1.Rows[i]["PayAmount"] = (Pamount >= Mamount) ? (Pamount - Mamount) : 0;
            //        }
                //}
                //else
                //    retResult = -1;

            if (retResult >= 0)
            {
                if (RDLCCondition.Items[0].Selected == true)//每個月的
                    ReportViewer1.LocalReport.ReportPath = @"RDLC\PYR013P.rdlc";
                else//3個月出一次的
                    switch(AdjCondition.SelectedValue.Trim())
                    {
                        case "All":
                            ReportViewer1.LocalReport.ReportPath = @"RDLC\PYR013C1.rdlc";
                            break;
                        case "PA":
                            ReportViewer1.LocalReport.ReportPath = @"RDLC\PYR013C2.rdlc";
                            break;
                        case "AM":
                            ReportViewer1.LocalReport.ReportPath = @"RDLC\PYR013C3.rdlc";
                            break;
                        case "PM":
                            ReportViewer1.LocalReport.ReportPath = @"RDLC\PYR013C4.rdlc";
                            break;
                    }
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("PYR013", Table1));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(SearchList1.CompanyValue)));
                //ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SalaryYM", SalaryYM1.SelectSalaryYMName));
                //ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReportKind", "(" + RpKind.SelectedItem + ")"));
                ReportViewer1.Visible = true;
            }
            else
            {
                //BindData();
                if (LabMsg.Text.Length == 0)
                    LabMsg.Text = "查無資料";
                ReportViewer1.Visible = false;
            }
                Button1.Enabled = true;
            //}
        }
    }
    protected void SelectChanged(object sender, EventArgs e)
    {
        if (RDLCCondition.Items[1].Selected)
        {
            AdjCondition.Enabled = true;
        }
        else
        {
            AdjCondition.Enabled = false;
        }
    }
    /// <summary>
    /// 頁面查詢
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
            //Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
            //sYM = Payroll.CheckSalaryYM(SearchList1.CompanyValue, SalaryYM1.SelectSalaryYM, "0");
            //if (sYM.isControl == false)
            //{//未經控管
            //    retResult = -2;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無結果可查詢";
            //}
            //else if (string.IsNullOrEmpty(sYM.DraftDate))
            //{
            //    //指定公司於指定計薪年月未經試算
            //    retResult = -3;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行試算作業! 無結果可查詢";
            //}
            //else if (string.IsNullOrEmpty(sYM.ConfirmDate) && RpKind.SelectedValue.Equals("Payroll_History"))
            //{
            //    //指定公司於指定計薪年月已完成確認試算,不可再進行確認
            //    retResult = -4;
            //    LabMsg.Text = SalaryYM1.SelectSalaryYMName + "尚未進行確認作業! 無結果可查詢";
            //}
            //else
            //{
                Navigator1.Visible = true;
                GridView1.Visible = true;
            //}

            if (retResult < 0)
            {
                if (retResult == 0)
                    LabMsg.Text = "查無資料!!";
                Navigator1.Visible = false;
                GridView1.Visible = false;
            }
        }
    }
    private DataTable GetTable(string TableName)
    {
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.PYR013" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(DeCodeKey);
        MyCmd.Parameters.Add("@Company", SqlDbType.Char).Value = SearchList1.CompanyValue.Trim();
        MyCmd.Parameters.Add("@DeptId", SqlDbType.Char).Value = SearchList1.DepartmentValue.Replace("%","").Trim();
        MyCmd.Parameters.Add("@EmployeeId", SqlDbType.Char).Value = SearchList1.EmployeeValue.Replace("%", "").Trim();
        //MyCmd.Parameters.Add("@TableName", SqlDbType.Char).Value = TableName;
        if (txtDateS.Text != "")
        {
            MyCmd.Parameters.Add("@DateS", SqlDbType.Char).Value = _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()).Replace("/", "");
        }
        else
        {
            MyCmd.Parameters.Add("@DateS", SqlDbType.Char).Value = "";
        }
        if (txtDateE.Text != "")
        {
            MyCmd.Parameters.Add("@DateE", SqlDbType.Char).Value = _UserInfo.SysSet.FormatADDate(txtDateE.Text.Trim()).Replace("/", "");
        }
        else
        {
            MyCmd.Parameters.Add("@DateE", SqlDbType.Char).Value = "";
        }
        MyCmd.Parameters.Add("@CodeKey", SqlDbType.Char).Value = DeCodeKey;
        //MyCmd.Parameters.Add("@Category", SqlDbType.Char).Value = RDLCCondition.SelectedValue.Trim();
        //MyCmd.Parameters.Add("@Condition", SqlDbType.Char).Value = AdjCondition.SelectedValue.Trim();

        DataTable Dt = _MyDBM.ExecStoredProcedure("sp_PYR013", MyCmd.Parameters);
        py.AfterQuery(DeCodeKey);
        //string strROWID = " 0 ";
        ////檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        //if (_MyDBM.ServerVersion().StartsWith("08."))
        //    strROWID = " 0 ";
        //else
        //    strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";

        return Dt;
    }
    private void BindData()
    {
        BindData("");
    }
    private void BindData(string SortExpression)
    {

        GridView1.DataSource = GetTable("");
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
                //    iSeq = Convert.ToInt32(e.Row.Cells[0].Text);
                //    if (iSeq == 0)
                //    {
                //        GridView theGV = ((GridView)sender);
                //        iSeq = theGV.PageIndex * theGV.PageSize + e.Row.RowIndex + 1;
                //    }
                //    e.Row.Cells[0].Text = strPan + iSeq.ToString() + strPan;

                //    e.Row.Cells[1].Text = strPan + e.Row.Cells[1].Text + strPan;
                //    e.Row.Cells[2].Text = strPan + e.Row.Cells[2].Text + strPan;

                //    for (int i = 3; i < e.Row.Cells.Count; i++)
                //    {
                //        e.Row.Cells[i].Style.Add("text-align", "right");
                //        try
                //        {
                //            tempDec = Convert.ToDecimal(e.Row.Cells[i].Text.Trim());
                //            if (i > 2 && i < 10)
                //            {
                //                Pamount += tempDec;
                //                if (i == 7 || i == 9)//加項免稅合計
                //                    NTAmount += tempDec;
                //            }
                //            else if (i > 10 && i < 19)
                //            {
                //                Mamount += tempDec;
                //                if (i == 11)//減項應稅
                //                    NTAmount -= tempDec;
                //            }
                //            else if (i == 19)//扣項合計
                //                tempDec = Mamount;
                //            else if (i == 20)//實領金額
                //            {
                //                tempDec = Pamount - Mamount;
                //                if (tempDec < 0)
                //                    tempDec = 0;
                //            }
                //            else if (i == 21)//應稅金額
                //                tempDec = Pamount - NTAmount;

                //            decAmount[i] += tempDec;
                //            if (i == 11)
                //            {//可能為非整數
                //                e.Row.Cells[i].Text = tempDec.ToString("N2").Replace(".00", "") + strPan;
                //            }
                //            else
                //            {
                //                e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
                //            }

                //        }
                //        catch { }
                //    }
                }
                break;
            //case DataControlRowType.Header:
            //    for (int i = 3; i < e.Row.Cells.Count; i++)
            //    {
            //        decAmount[i] = 0;
            //    }
            //    GridView1.ShowFooter = true;
            //    break;
            //case DataControlRowType.Footer:
            //    e.Row.Cells[2].Text = strPan + "小計" + strPan;
            //    for (int i = 3; i < e.Row.Cells.Count; i++)
            //    {
            //        e.Row.Cells[i].Style.Add("text-align", "right");
            //        try
            //        {
            //            tempDec = decAmount[i];

            //            if (i == 11)
            //            {//可能為非整數
            //                e.Row.Cells[i].Text = tempDec.ToString("N2").Replace(".00", "") + strPan;
            //            }
            //            else
            //            {
            //                e.Row.Cells[i].Text = tempDec.ToString("N0") + strPan;
            //            }
            //        }
            //        catch { }
            //    }
            //    break;
        }
    }

    //取得薪資項目金額
    private string GetDetailSQL(string DeCodeKey, string PWDorPHD, string SalaryItem)
    {
        //bool blIsSpecial = PWDorPHD.Equals("_Master_Special");
        //string tempSQL = " IsNull((Select Sum(" + DeCodeKey + "([SalaryAmount])) from [Payroll" + PWDorPHD + (blIsSpecial ? "" : "_Detail") + "] " +
        //    " Where PRH.[Company]=[Company] And PRH.[EmployeeId]=[EmployeeId] And PRH.[SalaryYM]=[SalaryYM] " +
        //    " And [PeriodCode] in ('A') And [SalaryItem]='" + SalaryItem + "'),0) ";

        //if (RpSalaryKind.SelectedValue.Length == 0 && blIsSpecial)
        //    tempSQL = " 0 ";
        //else if (RpSalaryKind.SelectedValue.Length > 0 && !blIsSpecial)
        //{
        //    tempSQL = "(" + tempSQL + "+" + GetDetailSQL(DeCodeKey, "_Master_Special", SalaryItem) + ")";
        //}
        return "";
    }
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["SortDirectionDesc"] == null)
            ViewState["SortDirectionDesc"] = false;
        BindData(e.SortExpression);
        ViewState["SortDirectionDesc"] = (!((bool)ViewState["SortDirectionDesc"]));
    }
}