using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PanPacificClass;

public partial class PYR008: System.Web.UI.Page
{

    UserInfo _UserInfo = new UserInfo();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList ( );
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand ( );

    string _ProgramId = "PYR008";
    string Ssql = "";
    

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
        
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");

        
        if (!Page.IsPostBack)
        {
            //權限驗證
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else if ( _UserInfo.CheckPermission ( _ProgramId ) == false )
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            if ( Request [ "Company" ] != null )
            {
                PayrollLt.Company = Request [ "Company" ].ToString ( ).Trim ( );
                SearchList1.Company.SelectedValue = PayrollLt.Company;
                SearchList1.Company.Enabled = false;
            }

            if ( Request [ "EmployeeId" ] != null )
            {
                PayrollLt.EmployeeId = Request [ "EmployeeId" ].ToString ( ).Trim ( );
            }                                 
            
            YearList ( );
            ReportViewer1.Visible = false;
        }
        else
        {
            LabMsg.Text = "";

        }

        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

    //年度下拉單設定
    protected void YearChange ( object sender , EventArgs e )
    {
        YearList ( );
    }
    private void YearList ( )
    {
        if ( SalaryY.Items.Count > 0 )
        {
            int Year = int.Parse ( SalaryY.SelectedValue );
            SalaryY.Items.Clear ( );
            for ( int i = Year - 5 ; i < Year + 5 ; i++ )
            {
                SalaryY.Items.Add ( i.ToString ( ) );
            }
            SalaryY.SelectedValue = Year.ToString ( );
        }
        else
        {
            int ToDayYear = _UserInfo.SysSet.isTWCalendar ? DateTime.Today.Year - 1911 : DateTime.Today.Year;
            for ( int i = ToDayYear - 5 ; i < ToDayYear + 5 ; i++ )
            {
                SalaryY.Items.Add ( i.ToString ( ) );
            }
            SalaryY.SelectedValue = ToDayYear.ToString ( );
        }
    }

    //查詢按鈕
    protected void btnQuery_Click ( object sender , EventArgs e )
    {
        // 清除之前的資料
        ReportViewer1.LocalReport.DataSources.Clear ( );
        ReportViewer1.Visible = false;
        LabMsg.Text = "";

        if ( string.IsNullOrEmpty ( SearchList1.CompanyValue ) )
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            ReportViewer1.Visible = false;
            Payroll py = new Payroll ( );
            PayrollLt.DeCodeKey = "dbo.PYR001" + DateTime.Now.ToString ( "yyyyMMddmmss" );
            py.BeforeQuery ( PayrollLt.DeCodeKey );
            try
            {
                LabMsg.Text = "";
                bindData2 ( );
            }
            catch { }
            py.AfterQuery ( PayrollLt.DeCodeKey );
        }
    }

    protected void bindData2 ( )
    {
        string Ssql = "";
        SqlCommand sqlcmd = new SqlCommand ( );
        DataTable DT;
        DataTable DTSub;
        string strCompanyNo = ( ( string.IsNullOrEmpty ( PayrollLt.Company ) ) ? SearchList1.Company.SelectedValue : PayrollLt.Company ).Trim ( );
        string strCompanyName = DBSetting.CompanyName ( strCompanyNo );
        string strDepId = SearchList1.Department.SelectedValue;
        string strEmployeeId = SearchList1.Employee.SelectedValue;
        string strSalaryYM = int.Parse(SalaryY.SelectedValue) + 1911 + "";
        
        Ssql = @"
select cpm.Company,cpm.CompanyName,cpm.Boss,cpm.EmployeeId,cpm.EmployeeName,cpm.IDNo,cpm.DeptId,
convert(varchar,(convert(int,substring(convert(varchar,phh.SalaryYM),1,4))-1911)) sy,
sum(" + PayrollLt.DeCodeKey + "(phh.LI_Fee)) LI_FeeAll," +
"sum(" + PayrollLt.DeCodeKey + "(phh.HI_Fee)) HI_FeeAll" +
@" from (select c.Company,c.CompanyName,c.Boss,pm.EmployeeId,pm.EmployeeName,pm.IDNo,pm.DeptId from Company c 
left join Personnel_Master pm on c.Company=pm.Company) cpm 
left join Payroll_History_Heading phh on cpm.Company=phh.Company and cpm.EmployeeId=phh.EmployeeId";
        Ssql += " Where cpm.Company='" + strCompanyNo + "'";
        //計薪年月
        Ssql += string.Format ( " And substring(convert(varchar,phh.SalaryYM),1,4) = {0}" , strSalaryYM.Substring ( 0 , 4 ) );
        ////計薪期別
        if ( !string.IsNullOrEmpty ( strDepId ) )
            if ( strDepId == "%%" )
                Ssql += string.Format ( " And cpm.DeptId like '{0}'" , strDepId );
            else
                Ssql += string.Format ( " And cpm.DeptId = '{0}'" , strDepId );
        //員工帳號
        if ( !string.IsNullOrEmpty ( strEmployeeId ) )
            if ( strEmployeeId == "%%" )
                Ssql += string.Format ( " And cpm.EmployeeId like '{0}'" , strEmployeeId );
            else
                Ssql += string.Format ( " And cpm.EmployeeId = '{0}'" , strEmployeeId );
        
        Ssql += " group by cpm.Company,cpm.CompanyName,cpm.Boss,cpm.EmployeeId,cpm.EmployeeName,cpm.IDNo,cpm.DeptId,convert(varchar,(convert(int,substring(convert(varchar,phh.SalaryYM),1,4))-1911)) ";
        
        DT = _MyDBM.ExecuteDataTable ( Ssql );
        
        if ( DT == null )
        {
            LabMsg.Text = "尋找不到資料";
        }
        else
        {
            if ( DT.Rows.Count <= 0 )
            {
                LabMsg.Text = "此年度無資料";
            }
        }

        Ssql = @"select Company,EmployeeId,EmployeeName,IDNo,DeptId,substring(convert(varchar,SalaryYM),1,4) sy,DependentsName,Dependent_IDNo,Dependent_title,SUM(Dependent_HI_Fee) Dependent_HI_Fee 
                from
                (select a.Company,a.EmployeeId,a.EmployeeName,a.IDNo,a.DeptId,a.SalaryYM,hid.DependentsName,a.Dependent_IDNo,
                (select CodeName from CodeDesc Where CodeID='PY#Depende' And CodeCode=hid.Dependent_title) Dependent_title,a.Dependent_HI_Fee 
                 from 
                 (Select pm.Company,pm.EmployeeId,pm.EmployeeName,pm.IDNo,pm.DeptId,phh2.SalaryYM,phh2.Dependent_IDNo,phh2.Dependent_HI_Fee 
                  from Personnel_Master pm 
                  left join 
                  (Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent1_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent1_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh " +
                   @"union 
                   Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent2_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent2_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh " +
                   @"union
                   Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent3_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent3_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh " +
                  @") phh2 on phh2.Company=pm.Company and phh2.EmployeeId=pm.EmployeeId 
                    group by pm.Company,pm.EmployeeId,pm.EmployeeName,pm.IDNo,pm.DeptId,phh2.SalaryYM,phh2.Dependent_IDNo,phh2.Dependent_HI_Fee) a 
                  left join
                  (Select Personnel_Master.Company,Personnel_Master.EmployeeId,Personnel_Master.IDNo,Personnel_Master.DeptId,HealthInsurance_Heading.EffectiveDate,HealthInsurance_Detail.DependentsName,
                   HealthInsurance_Detail.IDNo as DependentsIDNo,HealthInsurance_Detail.BirthDate,HealthInsurance_Detail.Dependent_title,HealthInsurance_Detail.EffectiveDate as DependentsEffectiveDate 
                    from Personnel_Master,HealthInsurance_Heading
                    left join HealthInsurance_Detail on HealthInsurance_Heading.Company = HealthInsurance_Detail.Company AND HealthInsurance_Heading.EmployeeId = HealthInsurance_Detail.EmployeeId 
                    Where Personnel_Master.Company = HealthInsurance_Heading.Company And Personnel_Master.EmployeeId = HealthInsurance_Heading.EmployeeId) hid
                    on a.Company=hid.Company and hid.EmployeeId=a.EmployeeId and hid.DependentsIDNo=a.Dependent_IDNo 
                    group by a.Company,a.EmployeeId,a.EmployeeName,a.IDNo,a.DeptId,a.SalaryYM,hid.DependentsName,a.Dependent_IDNo,Dependent_title,a.Dependent_HI_Fee)t ";
        Ssql += " Where Company='" + strCompanyNo + "'";
        //計薪年月
        Ssql += string.Format ( " And substring(convert(varchar,SalaryYM),1,4) = {0}" , strSalaryYM.Substring ( 0 , 4 ) );
        ////計薪期別
        //Ssql += string.Format(" And PWH.PeriodCode = '{0}'", strPeriodCode);
        if ( !string.IsNullOrEmpty ( strDepId ) )
            if ( strDepId == "%%" )
                Ssql += string.Format ( " And DeptId like '{0}'" , strDepId );
            else
                Ssql += string.Format ( " And DeptId = '{0}'" , strDepId );
        //員工帳號
        if ( !string.IsNullOrEmpty ( strEmployeeId ) )
            if ( strEmployeeId == "%%" )
                Ssql += string.Format ( " And EmployeeId like '{0}'" , strEmployeeId );
            else
                Ssql += string.Format ( " And EmployeeId = '{0}'" , strEmployeeId );
        Ssql += " group by Company,EmployeeId,EmployeeName,IDNo,DeptId,substring(convert(varchar,SalaryYM),1,4),DependentsName,Dependent_IDNo,Dependent_title order by Dependent_HI_Fee desc";
        DTSub = _MyDBM.ExecuteDataTable ( Ssql );
        if ( DTSub == null )
        {
            LabMsg.Text = "尋找不到資料";
        }

        if ( LabMsg.Text == "" )
        {
            ReportViewer1.LocalReport.DataSources.Add ( new Microsoft.Reporting.WebForms.ReportDataSource ( "DataSet1" , DT ) );
            SubDataSources = DTSub;
            ReportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler ( LocalReport_SubreportProcessing );
            ReportViewer1.LocalReport.SetParameters ( new Microsoft.Reporting.WebForms.ReportParameter ( "CompanyName" , DBSetting.CompanyName ( SearchList1.CompanyValue ) ) );
            ReportViewer1.LocalReport.SetParameters ( new Microsoft.Reporting.WebForms.ReportParameter ( "SalaryY" , SalaryY.SelectedValue ) );
            ReportViewer1.Visible = true;
        }
        JsUtility.CloseWaitScreenAjax ( UpdatePanel1 , "" );    //關閉執行等待畫面        
    }


    DataTable SubDataSources = new DataTable ( );

    void LocalReport_SubreportProcessing ( object sender , Microsoft.Reporting.WebForms.SubreportProcessingEventArgs e )
    {
        e.DataSources.Add ( new Microsoft.Reporting.WebForms.ReportDataSource ( "Dependents", SubDataSources )  );
    }



}
