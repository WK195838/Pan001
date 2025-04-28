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

public partial class PYR024 : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR024";
    DBManger _MyDBM;
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    string sKind = "W";
    string PayrollTable = "Payroll_History";
    //標題補充文字
    string ShowTitle = "";
    string ShowMode = "(無測試模式)";

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";

        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "Payroll_History"; ;//無測試用資料表
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }
    //**-------------從這裡開始試算和確認的報表程式都相同---------------**//
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

            if (string.IsNullOrEmpty(PayrollLt.Company))
                clDeptId_S.SetDTList("Department", "DepCode", "DepName", "", 5, "");
            else
                clDeptId_S.SetDTList("Department", "DepCode", "DepName", "Company='" + PayrollLt.Company + "'", 5, "");

            if (string.IsNullOrEmpty(PayrollLt.Company))
                clDeptId_E.SetDTList("Department", "DepCode", "DepName", "", 5, "");
            else
                clDeptId_E.SetDTList("Department", "DepCode", "DepName", "Company='" + PayrollLt.Company + "'", 5, "");
        }
        else
        {
            //if (string.IsNullOrEmpty(PayrollLt.Company))
            //    clDeptId_S.SetDTList("Department", "DepCode", "DepName", "", 2);
            //else
            //    clDeptId_S.SetDTList("Department", "DepCode", "DepName", "Company='" + PayrollLt.Company + "'", 2);

            //if (string.IsNullOrEmpty(PayrollLt.Company))
            //    clDeptId_E.SetDTList("Department", "DepCode", "DepName", "", 2);
            //else
            //    clDeptId_E.SetDTList("Department", "DepCode", "DepName", "Company='" + PayrollLt.Company + "'", 2);


            if (ViewState["Sourcedata"] != null)
            {
                try
                {
                    //cryReportSource.Report.Parameters.Add((CrystalDecisions.Web.Parameter)ViewState["SourcedataParameters"]);
                    cryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                    CrystalReportViewer1.DataBind();
                }
                catch (Exception ex)
                {

                    CrystalReportViewer1.Visible = false;
                    LabMsg.Text = "報表逾時，請重新產生！" + ex.Message;
                }
            }
        }

        //SalaryYM1.Enabled = false;
        PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());

        PeriodList1.Enabled = false;
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo." + _ProgramId + DateTime.Now.ToString("yyyyMMddmmss");
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
        string Ssql = "", sqlTable1 = "", sqlTable2 = "";
        SqlCommand sqlcmd = new SqlCommand();
        DataTable DT, DTSub;

        string strCompanyNo = ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim();
        string strCompanyName = DBSetting.CompanyName(strCompanyNo);
        string strPeriodCode = ((string.IsNullOrEmpty(PayrollLt.PeriodCode)) ? PeriodList1.selectIndex.ToString() : PayrollLt.PeriodCode).Trim();
        string strSalaryYM = SalaryYM1.SelectSalaryYM.Trim();
        //string strEmployeeId = ((string.IsNullOrEmpty(PayrollLt.EmployeeId)) ? "" : PayrollLt.EmployeeId).Trim();

        //參數
        sqlcmd.Parameters.Add("@ls_PayrollTable", System.Data.SqlDbType.VarChar).Value = PayrollTable;
        sqlcmd.Parameters.Add("@ls_CompanyId", System.Data.SqlDbType.VarChar).Value = strCompanyNo;
        sqlcmd.Parameters.Add("@ls_SalaryYM", System.Data.SqlDbType.VarChar).Value = strSalaryYM;
        sqlcmd.Parameters.Add("@ls_PeriodCode", System.Data.SqlDbType.VarChar).Value = strPeriodCode;
        sqlcmd.Parameters.Add("@ls_DeptId_S", System.Data.SqlDbType.VarChar).Value = clDeptId_S.SelectedCode.Trim();
        sqlcmd.Parameters.Add("@ls_DeptId_E", System.Data.SqlDbType.VarChar).Value = clDeptId_E.SelectedCode.Trim();
        sqlcmd.Parameters.Add("@ls_EmployeeId_S", System.Data.SqlDbType.VarChar).Value = txtEmpSatrt.Text.Trim();
        sqlcmd.Parameters.Add("@ls_EmployeeId_E", System.Data.SqlDbType.VarChar).Value = txtEmpEnd.Text.Trim();
        sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = PayrollLt.DeCodeKey;

        //主檔
        Ssql = "dbo.sp_PY_CostCenterM";
        //DT = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters);
        DataSet Ds = new DataSet();
        int iRet = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters, out Ds);
        if (Ds.Tables.Count > 1)
        {
            Ds.Tables[0].TableName = "NotTable1";
            Ds.Tables[1].TableName = "NotTable2";
            DT = Ds.Tables[2];
            DT.TableName = "Table1";
        }
        else
            DT = Ds.Tables[0];               

        //明細
        Ssql = "dbo.sp_PY_CostCenterDetail";
        DTSub = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters);

        int iTotalPerson = 0;
        if (DTSub != null)
            if (DTSub.Rows.Count > 0)
                iTotalPerson = (int)DTSub.Rows[0][0];
        #region 設值給報表參數
        //公司
        cryReportSource.Report.Parameters.Add(NewCP("CompanyName", strCompanyName));

        //報表標題
        cryReportSource.Report.Parameters.Add(NewCP("RPTTitle", "成本中心彙總表" + ShowTitle));

        //計薪年月
        cryReportSource.Report.Parameters.Add(NewCP("SalaryYM", strSalaryYM));

        //計薪期別        
        cryReportSource.Report.Parameters.Add(NewCP("PeriodCode", PeriodList1.selectPeriod + "日"));

        //部門起迄
        cryReportSource.Report.Parameters.Add(NewCP("DepS", clDeptId_S.SelectedCodeName.Trim().Replace(" ", "")));
        cryReportSource.Report.Parameters.Add(NewCP("DepE", clDeptId_E.SelectedCodeName.Trim().Replace(" ", "")));

        //員工起迄
        cryReportSource.Report.Parameters.Add(NewCP("EmpS", txtEmpSatrt.Text.Trim()));
        cryReportSource.Report.Parameters.Add(NewCP("EmpE", txtEmpEnd.Text.Trim()));

        //列印日期
        cryReportSource.Report.Parameters.Add(NewCP("prtDate", _UserInfo.SysSet.FormatDate(DateTime.Today.ToString())));

        //人數
        cryReportSource.Report.Parameters.Add(NewCP("TheCounts", iTotalPerson.ToString()));


        DataTable dtTN = null;
        for (int i = 1; i <= 20; i++)
        {//取得各薪資項目之欄位名稱
            CrystalDecisions.Web.Parameter TitleName = new CrystalDecisions.Web.Parameter();
            TitleName.Name = "TitleName" + i.ToString().PadLeft(2, '0');
            dtTN = _MyDBM.ExecuteDataTable("Select SalaryName,SalaryId from SalaryStructure_Parameter Where SalaryMasterList = '" + i.ToString() + "' Order By SalaryId");
            TitleName.DefaultValue = "";
            if (dtTN != null)
                if (dtTN.Rows.Count > 0)
                {
                    TitleName.DefaultValue = dtTN.Rows[0][0].ToString();
                    if (dtTN.Rows[0]["SalaryId"].ToString().Trim().Equals("01"))
                        TitleName.DefaultValue += "(實發)";
                }
            cryReportSource.Report.Parameters.Add(TitleName);
        }
        #endregion

        if (DT == null)
        {
            LabMsg.Text = "查無資料!";
        }
        else
        {
            LabMsg.Text = "";
            cryReportSource.ReportDocument.SetDataSource(DT);
            //cryReportSource.ReportDocument.Subreports[0].SetDataSource(DTSub);

            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.Visible = true;
            ViewState["Sourcedata"] = DT;
            //ViewState["SourcedataSub"] = DTSub;
            ////直接存下來 @"c:\"            
            //cryReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, _ProgramId+"_" + _UserInfo.UData.Company + _UserInfo.UData.EmployeeId + "_" + DateTime.Today.ToString("yyyyMMdd") + ".pdf");
        }
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面        
    }

    protected CrystalDecisions.Web.Parameter NewCP(string strName, string strValue)
    {
        CrystalDecisions.Web.Parameter Crystalparam = new CrystalDecisions.Web.Parameter();
        Crystalparam.Name = strName;
        Crystalparam.DefaultValue = strValue;
        return Crystalparam;
    }
}