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

public partial class RPT_GroupInsuranceReport : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR009";
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
        string strCompanyNo = ((string.IsNullOrEmpty(PayrollLt.Company)) ? CompanyList1.SelectValue : PayrollLt.Company).Trim();
        string strCompanyName = DBSetting.CompanyName(strCompanyNo);
        string strDepId = DepList1.SelectedValue;
        string strEmployeeId = EmployeeIdList1.SelectedValue;
        //string strSalaryYM = SalaryYM1.SelectSalaryYM.Trim();
        Ssql = @"select c.Company
,c.CompanyName
,pm.EmployeeId
,pm.EmployeeName
,pm.DeptId
,d.DepName 
,im.InsuranceCode
,im.EffectiveDate
," + PayrollLt.DeCodeKey + "(im.InsuredSalary ) InsuredSalary "+ 
@",im.SurrenderDate 
from Company c left join
Personnel_Master pm on c.Company=pm.Company 
left join Department d on pm.Company=d.Company and pm.DeptId= d.DepCode 
left join Insurance_Master im on pm.Company=im.Company and pm.EmployeeId=im.EmployeeId 
";
        Ssql += " Where c.Company='" + strCompanyNo + "'";
        //計薪年月
       // Ssql += string.Format(" And substring(convert(varchar,phh.SalaryYM),1,4) = {0}", strSalaryYM.Substring(0, 4));
        ////計薪期別
        if (!string.IsNullOrEmpty(strDepId))
            if (strDepId == "%%")
                Ssql += string.Format(" And pm.DeptId like '{0}'", strDepId);
            else
                Ssql += string.Format(" And pm.DeptId = '{0}'", strDepId);
        //員工帳號
        if (!string.IsNullOrEmpty(strEmployeeId))
            if (strEmployeeId == "%%")
                Ssql += string.Format(" And pm.EmployeeId like '{0}'", strEmployeeId);
            else
                Ssql += string.Format(" And pm.EmployeeId = '{0}'", strEmployeeId);
        //Ssql += " group by cpm.Company,cpm.CompanyName,cpm.Boss,cpm.EmployeeId,cpm.EmployeeName,cpm.IDNo,cpm.DeptId,substring(convert(varchar,phh.SalaryYM),1,4)";
        DT = _MyDBM.ExecuteDataTable(Ssql);
        if (DT == null)
        {
            LabMsg.Text = "尋找不到資料";
        }

        // 設值給報表參數
        cryReportSource.Report.Parameters.Add(NewParameter("CompanyName", strCompanyName));//公司名稱;
        //列印日期
        //cryReportSource.Report.Parameters.Add(NewCP("prtDate", _UserInfo.SysSet.FormatDate(DateTime.Today.ToString())));
        if (LabMsg.Text == "")
        {
            cryReportSource.ReportDocument.SetDataSource(DT);
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
