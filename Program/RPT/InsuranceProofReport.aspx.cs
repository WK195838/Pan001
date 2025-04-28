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

public partial class RPT_InsuranceProofReport : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR008";
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

        CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged(CompanyList1_SelectedIndex);
        DepList1.SelectedIndexChanged += new EventHandler(DepList1_SelectedIndexChanged);
        
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
            //PeriodList1.defaulPeriod();

            //if (Request["Kind"] != null)
            //{
            //    sKind = Request["Kind"].ToString().Trim();
            //    if (!sKind.Equals("W"))
            //    {
            //        //StyleTitle1.Title = "已確認薪資查詢";
            //    }
            //}

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
            SetSalaryY();
            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);
            //if (Request["SYM"] != null)
            //{
            //    SalaryYM1.SelectSalaryYM = Request["SYM"].ToString().Trim();
            //}

        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                cryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
        }

        //SalaryYM1.Enabled = false;
        //PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());

        //PeriodList1.Enabled = false;
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }
    ////  設定年月選擇鈕
    //private void SetCKYM()
    //{
    //    CKYM.Items.Add(Li("是", "1"));
    //    CKYM.Items.Add(Li("否", "0"));
    //    CKYM.SelectedIndex = 0;
    //}
    private void SetSalaryY()
    {
        int nowyear = int.Parse(DateTime.Now.Year.ToString());
        SalaryY.Items.Clear();
        for (int j = nowyear - 1; j < nowyear + 10; j++)
        {
            int adyear = j - 1911;
            SalaryY.Items.Add(Li(adyear.ToString(), j.ToString()));
        }
        SalaryY.SelectedValue = nowyear.ToString();
    }
    //  公司下拉式選單
    void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        #region 部門選單
        DepList1.Items.Clear();
        DepList1.Items.Insert(0, Li("全部", "%%"));
        for (int i = 0; i < e.Department_Basic.Count; i++)
        {
            DepList1.Items.Add(e.Department_Basic[i]);
        }
        DepList1.DataBind();
        #endregion

        #region 員工選單
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0, Li("全部", "%%"));
        for (int i = 0; i < e.Personnel_Master.Count; i++)
        {
            EmployeeIdList1.Items.Add(e.Personnel_Master[i]);
        }
        EmployeeIdList1.DataBind();
        #endregion
    }
    // 部門下拉式選單
    void DepList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region 員工選單
        DataTable DT = _MyDBM.ExecuteDataTable("select DeptId,EmployeeId,EmployeeName from Personnel_Master where Company = '" + CompanyList1.SelectValue + "' ");
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0, Li("全部", "%%"));
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            string[] Employee = { DT.Rows[i]["DeptId"].ToString().Trim(), DT.Rows[i]["EmployeeId"].ToString().Trim(), DT.Rows[i]["EmployeeName"].ToString() };
            if (((DropDownList)sender).SelectedItem.Text != "全部")
            {

                if (DT.Rows.Count > 0 && ((DropDownList)sender).SelectedItem.Value.Trim() == Employee[0].Trim())
                {
                    EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2], Employee[1]));
                }
            }
            else
                EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2], Employee[1]));
        }
        EmployeeIdList1.DataBind();
        #endregion
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.PYR001" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(PayrollLt.DeCodeKey);
        try
        {
            LabMsg.Text = "";
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
        DataTable DTSub;
        string strCompanyNo = ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim();
        string strCompanyName = DBSetting.CompanyName(strCompanyNo);
        string strDepId = DepList1.SelectedValue;
        string strEmployeeId = EmployeeIdList1.SelectedValue;
        string strSalaryYM = SalaryY.SelectedValue;//SalaryYM1.SelectSalaryYM.Trim();
        Ssql = @"select cpm.Company,
cpm.CompanyName,
cpm.Boss,
cpm.EmployeeId,
cpm.EmployeeName,
cpm.IDNo,
cpm.DeptId,
convert(varchar,(convert(int,substring(convert(varchar,phh.SalaryYM),1,4))-1911)) sy,
sum(" + PayrollLt.DeCodeKey + "(phh.LI_Fee)) LI_FeeAll"+
",sum(" + PayrollLt.DeCodeKey + "(phh.HI_Fee)) HI_FeeAll"+
",(Select sum(" + PayrollLt.DeCodeKey + "(HI2)) from BonusMaster BM Where BM.Company=cpm.Company and BM.EmployeeId=cpm.EmployeeId and CostYear=" + strSalaryYM.Substring(0, 4) + " ) HI2" +
@" from (select c.Company,c.CompanyName,c.Boss,pm.EmployeeId,pm.EmployeeName,pm.IDNo,pm.DeptId from Company c 
left join Personnel_Master pm on c.Company=pm.Company) cpm 
left join Payroll_History_Heading phh on cpm.Company=phh.Company and cpm.EmployeeId=phh.EmployeeId";
        Ssql += " Where cpm.Company='" + strCompanyNo + "'";
        //計薪年月
        Ssql += string.Format(" And substring(convert(varchar,phh.SalaryYM),1,4) = {0}", strSalaryYM.Substring(0, 4));
        ////計薪期別
        if (!string.IsNullOrEmpty(strDepId))
            if (strDepId == "%%")
                Ssql += string.Format(" And cpm.DeptId like '{0}'", strDepId);
            else
                Ssql += string.Format(" And cpm.DeptId = '{0}'", strDepId);
        //員工帳號
        if (!string.IsNullOrEmpty(strEmployeeId))
            if (strEmployeeId == "%%")
                Ssql += string.Format(" And cpm.EmployeeId like '{0}'", strEmployeeId);
            else
                Ssql += string.Format(" And cpm.EmployeeId = '{0}'", strEmployeeId);
        Ssql += " group by cpm.Company,cpm.CompanyName,cpm.Boss,cpm.EmployeeId,cpm.EmployeeName,cpm.IDNo,cpm.DeptId,convert(varchar,(convert(int,substring(convert(varchar,phh.SalaryYM),1,4))-1911)) ";
        DT = _MyDBM.ExecuteDataTable(Ssql);
        if (DT == null)
        {
            LabMsg.Text = "尋找不到資料";
        }
        else
        {
            if (DT.Rows.Count <= 0)
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
                  (Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent1_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent1_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh "+
                   @"union 
                   Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent2_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent2_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh "+
                   @"union
                   Select phh.Company,phh.EmployeeId,phh.SalaryYM SalaryYM,phh.Dependent3_IDNo Dependent_IDNo," + PayrollLt.DeCodeKey + "(phh.Dependent3_HI_Fee) Dependent_HI_Fee from Payroll_History_Heading phh "+
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
        Ssql += string.Format(" And substring(convert(varchar,SalaryYM),1,4) = {0}", strSalaryYM.Substring(0, 4));
        ////計薪期別
        //Ssql += string.Format(" And PWH.PeriodCode = '{0}'", strPeriodCode);
        if (!string.IsNullOrEmpty(strDepId))
            if (strDepId == "%%")
                Ssql += string.Format(" And DeptId like '{0}'", strDepId);
            else
            Ssql += string.Format(" And DeptId = '{0}'", strDepId);
        //員工帳號
        if (!string.IsNullOrEmpty(strEmployeeId))
            if(strEmployeeId=="%%")
                Ssql += string.Format(" And EmployeeId like '{0}'", strEmployeeId);
            else
            Ssql += string.Format(" And EmployeeId = '{0}'", strEmployeeId);
        Ssql += " group by Company,EmployeeId,EmployeeName,IDNo,DeptId,substring(convert(varchar,SalaryYM),1,4),DependentsName,Dependent_IDNo,Dependent_title order by Dependent_HI_Fee desc";
        DTSub = _MyDBM.ExecuteDataTable(Ssql);
        if (DTSub == null)
        {
            LabMsg.Text = "尋找不到資料";
        }
        //else
        //{
        //    if (DTSub.Rows.Count <= 0)
        //    {
        //        LabMsg.Text = "此年度無此員工資料";
        //    }
        //}
        // 設值給報表參數
        //cryReportSource.Report.Parameters.Add(NewParameter("CompanyName", strCompanyName));//公司名稱;
        //cryReportSource.Report.Parameters.Add(NewParameter("Boss", strCompanyName));//公司名稱;
        //列印日期
        //cryReportSource.Report.Parameters.Add(NewCP("prtDate", _UserInfo.SysSet.FormatDate(DateTime.Today.ToString())));
        if (LabMsg.Text == "")
        {
            cryReportSource.ReportDocument.SetDataSource(DT);
            cryReportSource.ReportDocument.Subreports[0].SetDataSource(DTSub);
            cryReportSource.ReportDocument.Subreports[1].SetDataSource(DTSub);

            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.Visible = true;
            ViewState["Sourcedata"] = DT;
        }
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面        
    }
    private ListItem Li(string text, string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value.Trim();
        return li;
    }

    private CrystalDecisions.Web.Parameter NewParameter(string Name, string Value)
    {
        CrystalDecisions.Web.Parameter NewParameter = new CrystalDecisions.Web.Parameter();
        NewParameter.Name = Name;
        NewParameter.DefaultValue = Value;
        return NewParameter;
    }
}
