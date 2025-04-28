using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ExportCompanyBurden : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYE001";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
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
            if (PayrollTable.Contains("test"))
                StyleTitle1.Title += "(測試模式)";
            SalaryYM1.defaultlist();
            BindData();
            ReportViewer1.Visible = false;
        }
        //else
        //{
        //    BindData();
        //}
        //if (Session["ExportDT"] != null)
        //    btnToExcel.Attributes.Add("onclick", "return GVExportToExcel('" + Session["ExportDT"].GetType().Name + "');");
        
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
            
            // 清除之前的資料
            ReportViewer1.LocalReport.DataSources.Clear();
            GridView1.Visible = false;
            btnToExcel.Visible = false;
                
            DataTable Table1 = new DataTable();
            BindData();                
            Table1 = (DataTable)GridView1.DataSource;
            Table1.TableName = "CompanyBurden";
            
            if (retResult >= 0)
            {
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(Table1.TableName, Table1));
                Table1 = (DataTable)GridView2.DataSource;
                Table1.TableName = "CompanyBurdenHI2";
                ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(Table1.TableName, Table1));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(SearchList1.CompanyValue)));
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SalaryYM", SalaryYM1.SelectSalaryYMName + "勞健保及勞退公司負擔及自付額報表"));                    
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

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        btnToExcel.Visible = false;

        if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            ReportViewer1.Visible = false;
            int retResult = 0;
                        
            GridView1.Visible = true;
            btnToExcel.Visible = true;
            BindData();
        }
    }

    private void BindData()
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.ExportCompanyBurden" + DateTime.Now.ToString("yyyyMMddmmss");        
        py.BeforeQuery(PayrollLt.DeCodeKey);
        string Ssql = "";
        try
        {
            Ssql = "SELECT PM.[Company]+'-'+(select [CompanyName] from [Company] where [Company]= PM.[Company]) As [Company] " +
                " ,PM.[DeptId]+'-'+(select [DepName] from [Department] where [Company]= PM.[Company] And [DepCode]= PM.[DeptId]) As [DeptId]" +
                " ,'" + SalaryYM1.SelectSalaryYMName.Trim() + "' As [SalaryYM] " +
                " ,PM.[EmployeeId] ,[EmployeeName] " +
                " ," + PayrollLt.DeCodeKey + "([BaseSalary]) As [BaseSalary] " +
                " ,Sum(Case When [SalaryItem] = '02' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem02] " +
                //" --[LiAmt]Convert(int,[RangeNo]) " +
                " ,Sum(Case When [SalaryItem] = '19' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [LIS] " +
                " ,Sum(Case When [SalaryItem] = '15' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem15] " +
                " ,Sum(Case When [SalaryItem] = '05' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem05] " +
                //" --[LiAmt] " +
                " ,Sum(Case When [SalaryItem] = '18' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [HIS] " +
                " ,Sum(Case When [SalaryItem] = '14' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem14] " +
                " ,Sum(Case When [SalaryItem] = '04' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem04] " +
                //" --[M_Contribution_Wages] " +
                " ,Sum(Case When [SalaryItem] = '17' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [RPS] " +
                " ,Sum(Case When [SalaryItem] = '16' then " + PayrollLt.DeCodeKey + "([SalaryAmount]) else 0 end) As [SalaryItem16] " +
                " ,IsNull((Select Sum(" + PayrollLt.DeCodeKey + "([HI2])) as HI2 from [vPRA_IncomHI2] " +
                " Where [IncomeDate]/100=(" + SalaryYM1.SelectSalaryYM.Trim() + "-191100) " +
                " and Company = PM.[Company] and EmployeeId=PM.[EmployeeId] " +
                " Group By Company,EmployeeId),0) as HI2" +
                " FROM [Personnel_Master] PM " +
                " left Join  " +
                " ( " +
                " select [BaseSalary],PD.* from [" + PayrollTable + "_Heading] PH Left Join [" + PayrollTable + "_Detail] PD On  " +
                " PH.[Company]= PD.[Company] and PH.[EmployeeId] = PD.[EmployeeId] and PH.[SalaryYM] = PD.[SalaryYM]  " +
                " ) PHD " +
                " On PM.[Company]= PHD.[Company] and PM.[EmployeeId] = PHD.[EmployeeId]  " +
                " where PM.[Company] = '" + SearchList1.CompanyValue + "' and [SalaryYM]=" + SalaryYM1.SelectSalaryYM.Trim();
                //" --PHD.[PeriodCode] = 'C' " +

            if (SearchList1.DepartmentValue.Trim().Replace("%%", "").Length != 0)
            {
                Ssql += " And PM.[DeptId] = '" + SearchList1.DepartmentValue.Trim() + "'";
            }

            if (SearchList1.EmployeeValue.Trim().Replace("%%", "").Length != 0)
            {
                Ssql += " And PM.[EmployeeId] = '" + SearchList1.EmployeeValue.Trim() + "'";
            }

            Ssql += " Group By PM.[Company],PM.[DeptId],[SalaryYM],PM.[EmployeeId],[EmployeeName],[BaseSalary] ";
            
            if (Session["ExportCompanyBurdenSort"] != null)
            {
                Ssql += " Order by " + Session["ExportCompanyBurdenSort"].ToString();
            }
            else
            {
                Ssql += " Order By [SalaryYM],PM.[EmployeeId] ";
            }
            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);
            
            if (theDT.Rows.Count > 0)
            {                
                GridView1.DataSource = theDT;
                GridView1.DataBind();

                string strTitile = "";
                //使用GridView,則欄位格式可依設定
                Session["ExportDT"] = GridView1;
                Session["ExportDTTitile"] = null;
                //使用DataTable,則欄位名將直接使用SQL命令中的命名
                //Session["ExportDT"] = theDT;
                //Session["ExportDTTitile"] = "公司別,部門,年月,員工編號,姓名,薪資,伙食費,勞保投保級距,勞保公司負擔金額,勞保員工自付額,健保投保級距,健保公司負擔金額,健保員工自付額,勞退投保級距,勞退公司負擔金額";
                btnToExcel.Attributes.Add("onclick", "return DSExportToExcel();");
            }
            else
            {
                GridView1.Visible = false;
                Session["ExportDT"] = null;
                Session["ExportDTTitile"] = null;
            }


            if (GridView1.Rows.Count == 0)
                lbl_Msg.Text = "查無資料!!";
            else
                lbl_Msg.Text = "";

            Session["ExportDT2"] = GetTable(PayrollLt.DeCodeKey);
            GridView2.DataSource = (DataTable)Session["ExportDT2"];
            GridView2.DataBind();
        }
        catch { }
        py.AfterQuery(PayrollLt.DeCodeKey);
    }

    private DataTable GetTable(string DeCodeKey)
    {
        string Ssql = "";
        SqlCommand sqlcmd = new SqlCommand();
        //參數        
        sqlcmd.Parameters.Add("@ls_Company", System.Data.SqlDbType.VarChar).Value = SearchList1.CompanyValue;
        sqlcmd.Parameters.Add("@ls_CostYear", System.Data.SqlDbType.VarChar).Value = SalaryYM1.SelectSalaryYM.Trim();
        sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = DeCodeKey;

        //主檔
        Ssql = "dbo.SP_PRA_HI2Report02";
        DataTable theDT = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters);
        return theDT;
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

                //if ((i + 1) == e.Row.Cells.Count)
                if (i > 4)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                }
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

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                //lbl = (Label)e.Row.FindControl("lbl_ProgramType");

                //switch (lbl.Text)
                //{
                //    case "P":
                //        lbl.Text = "程式";
                //        break;

                //    case "M":
                //        lbl.Text = "選單";
                //        break;
                //}

                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時                    
                }
                else
                {
                    //查詢時                   
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                    {
                        e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;
                    }


                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                //link = (LinkButton)e.Row.FindControl("btnSelect");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Kind=Query&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");

                //link = (LinkButton)e.Row.FindControl("btnEdit");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                break;

            case DataControlRowType.Header:

                //link = (LinkButton)e.Row.FindControl("btnNew");
                //if (link != null)
                //{
                //    //指定位置用top=100px,left=100px,
                //    link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_A.aspx','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                //}
                break;
        }
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string strSort = "";
        if (Session["ExportCompanyBurdenSort"] != null)
            strSort = Session["ExportCompanyBurdenSort"].ToString();
        if (strSort.EndsWith("desc"))
            Session["ExportCompanyBurdenSort"] = e.SortExpression;
        else
            Session["ExportCompanyBurdenSort"] = e.SortExpression + " desc";
        BindData();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindData();
    }

}
