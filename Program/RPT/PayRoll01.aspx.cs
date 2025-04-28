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

public partial class PayRoll01 : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PayRoll01";
    DBManger _MyDBM;
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    string sKind = "W";

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

        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
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
                    //StyleTitle1.Title = "已確認薪資查詢";
                }
            }

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
        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設置磁碟選項與檔名
                string strCompanyNo = ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim();
                string strPeriodCode = ((string.IsNullOrEmpty(PayrollLt.PeriodCode)) ? PeriodList1.selectIndex.ToString() : PayrollLt.PeriodCode).Trim();
                string strSalaryYM = SalaryYM1.SelectSalaryYM.Trim();
                string strEmployeeId = ((string.IsNullOrEmpty(PayrollLt.EmployeeId)) ? "" : PayrollLt.EmployeeId).Trim();
                string FileName = Request.PhysicalApplicationPath + "\\Export\\" + _ProgramId + "_" + strCompanyNo + strSalaryYM + strPeriodCode + strEmployeeId + ".txt";
                ViewState["Sourcedata"] = WriteToTXT(FileName, ViewState["Sourcedata"]);
                cryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();

                #region 將報表資料匯出至伺服器
                //try
                //{                    
                //    //CrystalDecisions.Shared.DiskFileDestinationOptions
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument MyReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //    //指定報表容器
                //    MyReport.Load(Server.MapPath("PayRoll01.rpt"));
                //    //傳入資料集
                //    MyReport.Database.Tables[0].SetDataSource((DataTable)ViewState["Sourcedata"]);
                //    CrystalDecisions.Shared.DiskFileDestinationOptions diskOpts = new CrystalDecisions.Shared.DiskFileDestinationOptions();
                //    MyReport.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                //    MyReport.ExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.Excel;
                //    //傳入參數
                //    MyReport.SetParameterValue("CompanyName", ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim());
                //    MyReport.SetParameterValue("SalaryYM", SalaryYM1.SelectSalaryYM.Trim());
                //    //設置磁碟選項與檔名
                //    if (!System.IO.Directory.Exists(Request.PhysicalApplicationPath + "\\Export\\"))
                //    {
                //        System.IO.Directory.CreateDirectory(Request.PhysicalApplicationPath + "\\Export\\");
                //    }
                //    diskOpts.DiskFileName = Request.PhysicalApplicationPath + "\\Export\\" + _ProgramId + ".xls";
                //    MyReport.ExportOptions.DestinationOptions = diskOpts;
                //    MyReport.Export(MyReport.ExportOptions);                    
                //}
                //catch (Exception ex)
                //{
                //    LabMsg.Text = ex.Message;
                //}
                #endregion
            }
        }

        //SalaryYM1.Enabled = false;
        PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());

        PeriodList1.Enabled = false;
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

    public string WriteToTXT(String TxtFile, object Msg)
    {
        //TxtFile = TxtFile.Replace(":", "_");
        //String path = String.Format("{0}.txt", TxtFile);
        String path = TxtFile;
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
        }
        else
        {
            if (fileInfo.Exists)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(path, System.Text.Encoding.Default);
                path = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            else
            {
                try
                {
                    System.IO.StreamWriter sw = fileInfo.AppendText();
                    sw.Write(Msg);                    
                    sw.Flush();
                    sw.Close();
                    path = Convert.ToString(Msg);
                }
                catch (Exception ex)
                {
                    path = ex.Message;
                }
            }
        }        
        return path;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.PayRoll01" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(PayrollLt.DeCodeKey);
        try
        {
            bindData();
        }
        catch { }
        py.AfterQuery(PayrollLt.DeCodeKey);
    }

    protected void bindData()
    {
        string Ssql = "";
        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT;

        string strCompanyNo = ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim();
        string strCompanyName = DBSetting.CompanyName(strCompanyNo);
        string strPeriodCode = ((string.IsNullOrEmpty(PayrollLt.PeriodCode)) ? PeriodList1.selectIndex.ToString() : PayrollLt.PeriodCode).Trim();
        string strSalaryYM = SalaryYM1.SelectSalaryYM.Trim();
        string strEmployeeId = ((string.IsNullOrEmpty(PayrollLt.EmployeeId)) ? "" : PayrollLt.EmployeeId).Trim();

        Ssql = "SELECT PWH.[Company],ISNULL(D.DepCode, PM.DeptId) AS DeptId,IsNull([DepName],[DeptId]) As [DepName],C.[CompanyName] " +
            " ,PWH.[EmployeeId],PM.[EmployeeName],[SalaryYM],PWH.[PeriodCode]"+
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
            ",[Paydays],[LeaveHours_deduction],[TaxRate] " +
            " ,[NT_P],[WT_P_Salary],[HI_Person],[WT_Overtime] " +
            " ,[NT_Overtime],[OnWatch] " +
            " FROM [PayrollWorking_Heading] PWH" +
            " Left Join [Personnel_Master] PM On PWH.[Company] = PM.[Company] And PWH.[EmployeeId] = PM.[EmployeeId]" +
            " Left Join [Company] C On PWH.[Company] = C.[Company] " +
            " Left Join [Department] D On PM.[Company] = D.[Company] And  PM.[DeptId] = D.[DepCode] ";
        Ssql += " Where PWH.Company='" + strCompanyNo + "'";
        //計薪年月
        Ssql += string.Format(" And PWH.SalaryYM = {0}", strSalaryYM);
        //計薪期別
        Ssql += string.Format(" And PWH.PeriodCode = '{0}'", strPeriodCode);
        
        //員工帳號
        if (!string.IsNullOrEmpty(strEmployeeId))
            Ssql += string.Format(" And PWH.EmployeeId = '{0}'", strEmployeeId);

        DT = _MyDBM.ExecuteDataTable(Ssql);
        
        // 設值給報表參數
        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = strCompanyName;
        cryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();
        SDateparam.Name = "SalaryYM";
        SDateparam.DefaultValue = strSalaryYM;
        cryReportSource.Report.Parameters.Add(SDateparam);

        cryReportSource.ReportDocument.SetDataSource(DT);

        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
        ViewState["Sourcedata"] = DT;

        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面        
    }    
}