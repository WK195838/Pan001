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
    string _ProgramId = "PYR020";
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
            SetLeaveType();
            SetResignCode();
        }
        else
        {
            LabMsg.Text = "";
            //當搜尋元件中的公司下拉單變更選擇時,要重設假別選項
            if (Request.Form["__EVENTTARGET"].Contains(SearchList1.GetID(SearchList.theDropDownList.CompanyList)))
                SetLeaveType();
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
        else if (txtDateS.Text.Length == 0 || txtDateE.Text.Length == 0)
        {
            LabMsg.Text = "請先輸入起迄日期!";
        }
        else if (!txtDateS.Text.StartsWith(txtDateE.Text.Substring(0, txtDateE.Text.IndexOf("/"))))
        {
            LabMsg.Text = "起迄日期需同一年度!";
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

                Decimal tempDec = 0, Pamount = 0;

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
                strValue = "部門名稱：" + SearchList1.DepartmentText.Trim();
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Query1", strValue));
                if (string.IsNullOrEmpty(txtDateS.Text) && string.IsNullOrEmpty(txtDateE.Text))
                    strValue = "日　　期：";
                else
                    strValue = "日　　期：" + txtDateS.Text + " ～ " + txtDateE.Text;
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
        else if (txtDateS.Text.Length == 0 || txtDateE.Text.Length == 0)
        {
            LabMsg.Text = "請先輸入起迄日期!";
        }
        else if (!txtDateS.Text.StartsWith(txtDateE.Text.Substring(0, txtDateE.Text.IndexOf("/"))))
        {
            LabMsg.Text = "起迄日期需同一年度!";
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
        string DeCodeKey = "dbo.PYR020" + DateTime.Now.ToString("yyyyMMddHHmm");
        py.BeforeQuery(DeCodeKey);
        string Sqlcommand1 = "", Sqlcommand2 = "", Sqlcommand3 = "";
        string strSort = " Order By OT.Company,IsNull(IsNull(OT.[DeptId],PM.[DeptId]),'ZZZ'),OT.EmployeeId";
        string strROWID = " 0 ";
        //檢查DB版本:"08."開頭的版本為SQL2000,需使用SQL2000之指令
        if (_MyDBM.ServerVersion().StartsWith("08."))
            strROWID = " 0 ";
        else
            strROWID = " Row_Number() OVER (ORDER BY [EmployeeId] ASC) ";

        Ssql = " Select [Company],[EmployeeId],[EmployeeName],[DeptId],[DeptName] " +
            " ,([OverTime1]+[OverTime2]+[Holiday]+[NationalHoliday]) As [OTTotal] " +
            " ,[OverTime1],[OverTime2],[Holiday],[NationalHoliday] " +
            " ,([LT01]+[LT02]+[LT03]+[LT04]+[LT05]+[LT06]+[LT07]+[LT08]+[LT09]+[LT11]) As [LTTotal] " +
            " ,(Case When LT01 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='V'),365)*8 then -1 else 1 end)*LT01 As [LT01] " +
            " ,(Case When LT02 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='1'),365)*8 then -1 else 1 end)*LT02 As [LT02] " +
            " ,(Case When LT03 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='2'),365)*8 then -1 else 1 end)*LT03 As [LT03] " +
            " ,(Case When LT04 > (Isnull((select [ALDays] from [AnnualLeaveSettlement] Where [Company]=LOT.[Company] and [EmployeeId]=LOT.[EmployeeId] and [ALYear]=DatePart(Year,Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()) + "'))) " +
            " ,IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='3'),365))*8) then -1 else 1 end)*LT04 As [LT04] " +
            " ,(Case When LT05 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='4'),365)*8 then -1 else 1 end)*LT05 As [LT05] " +
            " ,(Case When LT06 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='5'),365)*8 then -1 else 1 end)*LT06 As [LT06] " +
            " ,(Case When LT07 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='6'),365)*8 then -1 else 1 end)*LT07 As [LT07] " +
            " ,(Case When LT08 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='7'),365)*8 then -1 else 1 end)*LT08 As [LT08] " +
            " ,(Case When LT09 > IsNull((Select [Annual_LeaveDays] From [LeaveType_Basic] Where [Company]=LOT.[Company] and [Leave_Id]='8'),365)*8 then -1 else 1 end)*LT09 As [LT09] " +
            " ,[LT10] " +
            " From ( " +
            " Select LOT.[Company],LOT.[EmployeeId],[EmployeeName] " +
            " ,(Case When IsNull(LOT.[DeptId],'')='' then PM.[DeptId] else LOT.[DeptId] end) As [DeptId] " +
            " ,(select [DepName] from [Department] where [Company]=LOT.[Company] and [DepCode]=(Case When IsNull(LOT.[DeptId],'')='' then PM.[DeptId] else LOT.[DeptId] end)) As [DeptName] " +
            " ,Sum([OverTime1]) [OverTime1],Sum([OverTime2]) [OverTime2],Sum([Holiday]) [Holiday],Sum([NationalHoliday]) [NationalHoliday] " +
            " ,Sum([LT01]) [LT01],Sum([LT02]) [LT02],Sum([LT03]) [LT03],Sum([LT04]) [LT04],Sum([LT05]) [LT05] " +
            " ,Sum([LT06]) [LT06],Sum([LT07]) [LT07],Sum([LT08]) [LT08],Sum([LT09]) [LT09],Sum([LT10]) [LT10] " +
            " ,Sum([LT11]) [LT11]" +
            " from ( " +
            " SELECT [Company],[EmployeeId],[DeptId]  " +
            "  ,[OverTime_Date] As [TheDate] " +
            "  ,[OverTime1],[OverTime2],[Holiday],[NationalHoliday] " +
            " ,0 LT01,0 LT02,0 LT03,0 LT04,0 LT05,0 LT06,0 LT07,0 LT08,0 LT09,0 [LT10],0 [LT11] " +
            "  FROM [OverTime_Trx] OT " +
            "  Where Not([ApproveDate] is Null) And IsNull([Overtime_pay],'Y')='Y' " +
            " Union " +
            " SELECT [Company],[EmployeeId],[DeptId] " +
            " ,[BeginDateTime] As [TheDate] " +
            " ,0 [OverTime1],0 [OverTime2],0 [Holiday],0 [NationalHoliday] " +
            " ,(Case [LeaveType_Id] When 'V' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT01] " +
            " ,(Case [LeaveType_Id] When '1' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT02] " +
            " ,(Case [LeaveType_Id] When '2' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT03] " +
            " ,(Case [LeaveType_Id] When '3' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT04] " +
            " ,(Case [LeaveType_Id] When '4' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT05] " +
            " ,(Case [LeaveType_Id] When '5' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT06] " +
            " ,(Case [LeaveType_Id] When '6' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT07] " +
            " ,(Case [LeaveType_Id] When '7' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT08] " +
            " ,(Case [LeaveType_Id] When '8' then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT09] " +
            " ,0 [LT10] " +
            " ,(Case When ([LeaveType_Id] != '1' and [LeaveType_Id] != '2' and [LeaveType_Id] != '3' and [LeaveType_Id] != '4' " +
            "   and [LeaveType_Id] != '5' and [LeaveType_Id] != '6' and [LeaveType_Id] != '7' and [LeaveType_Id] != '8' " +
            "   and [LeaveType_Id] != 'V' ) then (IsNull([hours],0)+IsNull([days],0)*8) else 0 end ) As [LT11] " +
            "  FROM [Leave_Trx] LT " +
            "  Where Not([ApproveDate] is Null) ";
            if (cbLeaveType.Items.Count > 0)
            {//2011/09/16 依人事要求加入假別篩選條件
                string strTL = "";
                foreach (ListItem lis in cbLeaveType.Items)
                {
                    if (lis.Selected)
                    {
                        strTL += "'" + lis.Value + "',";
                    }
                }
                if (strTL.Length > 0)
                {
                    Ssql += " And LT.LeaveType_Id in (" + strTL + "'-')";
                }
            }
            Ssql += " Union Select [Company],[EmployeeId],[DeptId] " +
            " ,[AttendanceDate] As [TheDate] " +
            " ,0 [OverTime1],0 [OverTime2],0 [Holiday],0 [NationalHoliday] " +
            " ,0 LT01,0 LT02,0 LT03,0 LT04,0 LT05,0 LT06,0 LT07,0 LT08,0 LT09 " +
            " ,(Case When [OverTime] >0 then 1 else 0 end) [LT10],0 [LT11] From [AttendanceException] Where ([AECode] = '1' Or [OverTime]>0) " +
            " ) LOT " +
            " Left Join [Personnel_Master] PM On LOT.[Company]=PM.[Company] And LOT.[EmployeeId]=PM.[EmployeeId] " +
            " Where [TheDate] Between Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()) + "') And Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateE.Text.Trim()) + "') ";
            if (cbResignC.Items.Count > 0)
            {//2011/09/16 依人事要求加入離職篩選條件
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
            Ssql +=" Group By LOT.[Company],LOT.[EmployeeId],(Case When IsNull(LOT.[DeptId],'')='' then PM.[DeptId] else LOT.[DeptId] end),[EmployeeName] " +
            " ) LOT  ";

        Ssql += string.Format(" Where LOT.[Company] = '{0}'", SearchList1.CompanyValue.Replace("%%", "").Trim());

        //部門
        if (SearchList1.DepartmentValue.Replace("%%", "").Length > 0)
        {
            Ssql += string.Format(" And LOT.[DeptId] = '{0}'", SearchList1.DepartmentValue.Replace("%%", "").Trim());
        }
        //員工
        if (SearchList1.EmployeeValue.Replace("%%", "").Length > 0)
        {
            Ssql += string.Format(" And LOT.EmployeeId = '{0}'", SearchList1.EmployeeValue.Replace("%%", "").Trim());
        }

        //if (txtDateS.Text.Trim().Length > 0)
        //{
        //    Ssql += string.Format(" And Convert(varchar,[TheDate],112) >= '{0}'", _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()).Replace("/", ""));
        //}
        //if (txtDateE.Text.Trim().Length > 0)
        //{
        //    Ssql += string.Format(" And Convert(varchar,[TheDate],112) <= '{0}'", _UserInfo.SysSet.FormatADDate(txtDateE.Text.Trim()).Replace("/", ""));
        //}

        //Ssql+="Group By LOT.[Company],LOT.[EmployeeId],IsNull(LOT.[DeptId],PM.[DeptId]),[EmployeeName]";

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

    /// <summary>
    /// 設定假別核取方塊:2011/08/18 依人事要求加入假別篩選條件
    /// </summary>
    private void SetLeaveType()
    {
        cbLeaveType.Items.Clear();
        Ssql = "SELECT [Company],[Leave_Id],[Leave_Desc],(Case [SalaryType] when 'Y' then '' else '(扣薪假)' end) [LeaveType],[SalaryType] " +
            " FROM [LeaveType_Basic] where [Company]='" + SearchList1.CompanyValue.Replace("%", "") + "' Order by SalaryType";
        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        ListItem theItem = new ListItem();
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            theItem = new ListItem();
            theItem.Value = DT.Rows[i]["Leave_Id"].ToString();
            theItem.Text = DT.Rows[i]["Leave_Desc"].ToString();
            if (DT.Rows[i]["SalaryType"].ToString().Equals("N"))
                theItem.Selected = true;
            cbLeaveType.Items.Add(theItem);
        }
    }

    /// <summary>
    /// 設定是含離職核取方塊:2011/09/16 依人事要求加入離職篩選條件
    /// </summary>
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
}
