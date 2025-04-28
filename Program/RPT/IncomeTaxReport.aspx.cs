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
using System.Data.SqlClient ;
using PanPacificClass;


public partial class Basic_IncomeTaxReport : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PayRoll01";
    DBManger _MyDBM;
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    string sKind = "W";
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

        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        if (!Page.IsPostBack)
        {
            YearList1.initList();

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

            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);
            //if (Request["SYM"] != null)
            //{
            //    SalaryYM1.SelectSalaryYM = Request["SYM"].ToString().Trim();
            //}

            //if (Request["SPeriod"] != null)
            //{
            //    PayrollLt.PeriodCode = Request["SPeriod"].ToString().Trim();
            //    PeriodList1.selectIndex = int.Parse(PayrollLt.PeriodCode);
            //}
            SetResignCode();
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

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.PYR001" + DateTime.Now.ToString("yyyyMMddmmss");
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

        //2011/10/12依財務Ivy確認修改稅額及應稅金額
        Ssql = "SELECT Company,EmployeeId,SalaryYM " +
            " ,SUM(Case When SalaryItem='03' Then " + PayrollLt.DeCodeKey + "(PWD.SalaryAmount) else 0 end) " +
            " As PWDTax " +
            " ,SUM(Case When SP.NWTax='Y' Then Case PMType When 'A' then IsNull(" + PayrollLt.DeCodeKey + "  (PWD.SalaryAmount),0) else IsNull(" + PayrollLt.DeCodeKey + "  (PWD.SalaryAmount),0)* -1 end " +
            " else 0 end) As PWDNWAmount " +
            " FROM PayrollWorking_Detail  PWD " +
            " Left Join SalaryStructure_Parameter SP " +
            " On PWD.SalaryItem = SP.SalaryId " +
            //" Where Company='{Company}' And EmployeeId Like '{Employee}' And PWD.SalaryYM Like '{Year}'" +
            " Where Company='{Company}' And PWD.SalaryYM Like '{Year}'" +
            " Group by Company,EmployeeId,SalaryYM";

        //2011/10/12依財務Ivy要求加入獎金發放之金額及其代扣稅額
        Ssql = " Select PWD.Company,PWD.EmployeeId,DeptId" +
            " ,IsNull((SELECT Top 1 DepName from Department Where Company=PWD.Company and DepCode=PM.DeptId),'') DepName" +
            " ,(Case When IsNull(ResignCode,'N')='N'then '' else ResignCode end) as ResignCode,ResignCode As QueryResignCode,EmployeeName" +
            //" ,RTRIM( convert(char, convert(decimal,left(PWD.SalaryYM,4))- 1911) )+ '\' + right(PWD.SalaryYM,2) AS SalaryYM" +
            " ,(PWDTax+IsNull(Tax,0)) As SalaryAmount,(PWDNWAmount+IsNull(CostAmt,0)) As OtherNWSalary " +
            " From(" + Ssql + " ) PWD " +
            " Left Join Personnel_Master PM on PWD.Company=PM.Company And PWD.EmployeeId=PM.EmployeeId" +
            " Left Join (Select Company,EmployeeId,Substring(Convert(varchar,AmtDate,112),1,6) As SalaryYM " +
            " ,Sum(IsNull(" + PayrollLt.DeCodeKey + "(Pay_AMT),0)) As Tax " +
            " ,Sum(IsNull(" + PayrollLt.DeCodeKey + "(CostAmt),0)) As CostAmt " +
            " from BonusMaster Where CostYear Like '{Year}' Group By Company,EmployeeId,CostYear,Substring(Convert(varchar,AmtDate,112),1,6) ) BM " +
            " On BM.Company = PWD.Company And BM.EmployeeId = PWD.EmployeeId " +
            " And PWD.SalaryYM=BM.SalaryYM ";

        Ssql = "Select Company,EmployeeId,DeptId,ResignCode,EmployeeName" +
            " , DepName" +
            ",Sum(SalaryAmount) as SalaryAmount,Sum(OtherNWSalary) as OtherNWSalary " +
            " From (" + Ssql + ") PWD ";            

        if (cbResignC.Items.Count > 0)
        {//2011/10/12 依人事要求加入離職篩選條件
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
                Ssql += " Where QueryResignCode in (" + strTL + "'-')";
            }
        }
        Ssql += " Group By Company,EmployeeId,DeptId,ResignCode,EmployeeName,DepName";
        Ssql += " ORDER BY EmployeeId";
        //公司
        if (string.IsNullOrEmpty(CompanyList1.SelectValue) == false) Ssql = Ssql.Replace("{Company}", CompanyList1.SelectValue);

        //年份
        if (string.IsNullOrEmpty(YearList1.SelectADYear) == false) Ssql = Ssql.Replace("{Year}", YearList1.SelectADYear + "%");

        DT = _MyDBM.ExecuteDataTable(Ssql);

        // 設值給報表參數
       //CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();

        
        //cparam.Name = "IM";
        //cparam.DefaultValue = "DDDD";
        //cryReportSource.Report.Parameters.Add(cparam);
         
        //CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();

       
        
        //SDateparam.Name = "SalaryYM";
        //SDateparam.DefaultValue = strSalaryYM;
        //cryReportSource.Report.Parameters.Add(SDateparam);

        cryReportSource.ReportDocument.SetDataSource(DT);

        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
        ViewState["Sourcedata"] = DT;

        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面        
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
