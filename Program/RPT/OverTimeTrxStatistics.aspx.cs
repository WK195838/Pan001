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

public partial class OverTimeTrxStatistics : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR005";
    DBManger _MyDBM;
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
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
        CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged(CompanyList1_SelectedIndex);
        DepList1.SelectedIndexChanged += new EventHandler(DepList1_SelectedIndexChanged);
        
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        if (!Page.IsPostBack)
        {
            SetCKYM();

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
            }

            CompanyList1.SelectValue = _UserInfo.UData.Company;
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
        PayrollLt.SalaryYM = Convert.ToInt32(SalaryYM1.SelectSalaryYM.Trim());

        
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }
    //  設定年月選擇鈕
    private void SetCKYM()
    {
        CKYM.Items.Add(Li("是" , "1"));
        CKYM.Items.Add(Li("否" , "0"));
        CKYM.SelectedIndex = 0;
    }


    //  公司下拉式選單
    void CompanyList1_SelectedIndex(object sender , UserControl_CompanyList.SelectEventArgs e)
    {
        #region 部門選單
        DepList1.Items.Clear();
        DepList1.Items.Insert(0 , Li("全部" , "%%"));
        for (int i = 0 ; i < e.Department_Basic.Count ; i++)
        {
            DepList1.Items.Add(e.Department_Basic[i]);
        }
        DepList1.DataBind();
        #endregion

        #region 員工選單
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0 , Li("全部" , "%%"));
        for (int i = 0 ; i < e.Personnel_Master.Count ; i++)
        {
            EmployeeIdList1.Items.Add(e.Personnel_Master[i]);
        }
        EmployeeIdList1.DataBind();
        #endregion
    }
    // 部門下拉式選單
    void DepList1_SelectedIndexChanged(object sender , EventArgs e)
    {
        #region 員工選單
        DataTable DT = _MyDBM.ExecuteDataTable("select DeptId,EmployeeId,EmployeeName from Personnel_Master where Company = '" + CompanyList1.SelectValue + "' ");
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0 , Li("全部" , "%%"));
        for (int i = 0 ; i < DT.Rows.Count ; i++)
        {
            string[] Employee = { DT.Rows[i]["DeptId"].ToString().Trim() , DT.Rows[i]["EmployeeId"].ToString().Trim() , DT.Rows[i]["EmployeeName"].ToString() };
            if (((DropDownList)sender).SelectedItem.Text != "全部")
            {

                if (DT.Rows.Count > 0 && ((DropDownList)sender).SelectedItem.Value.Trim() == Employee[0].Trim())
                {
                    EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2] , Employee[1]));
                }
            }
            else
                EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2] , Employee[1]));
        }
        EmployeeIdList1.DataBind();
        #endregion
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        bindData();
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
        string strSalaryYM = CKYM.SelectedValue == "0" ? SalaryYM1.SelectSalaryYM.Trim().Substring(0 , 4) + "%" : SalaryYM1.SelectSalaryYM.Trim();

        Ssql = @"
SELECT  P.*,A+B+C+D Tt from
(
SELECT         
C.Company,
C.CompanyName,
D.DepCode, 
D.DepName,
PM.EmployeeId,
PM.EmployeeName,
PM.ResignCode,
substring(convert(varchar,OTT.OverTime_Date,112),1,6) OTD,
Sum(OTT.OverTime1) A,
Sum(OTT.OverTime2) B,
Sum(OTT.Holiday) C,
Sum(OTT.NationalHoliday) D

FROM            
Company C INNER JOIN
Department D ON C.Company = D.Company INNER JOIN
Personnel_Master PM ON D.Company = PM.Company AND
D.DepCode = PM.DeptId AND
PM.DeptId LIKE '" + strDepId + @"'INNER JOIN
OverTime_Trx OTT ON PM.Company = OTT.Company AND OTT.Company LIKE '" + strCompanyNo + @"' AND
PM.EmployeeId = OTT.EmployeeId AND
OTT.EmployeeId LIKE '" + strEmployeeId + @"' AND
substring(convert(varchar,OTT.OverTime_Date,112),1,6) LIKE'" + strSalaryYM + @"'

GROUP BY
C.Company,
C.CompanyName,
D.DepCode, 
D.DepName,
PM.EmployeeId,
PM.EmployeeName,
substring(convert(varchar,OTT.OverTime_Date,112),1,6)
,PM.ResignCode
) P
";

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
                Ssql += " Where P.ResignCode in (" + strTL + "'-')";
            }
        }

        DT = _MyDBM.ExecuteDataTable(Ssql);
        // 設值給報表參數

        cryReportSource.Report.Parameters.Add(NewParameter("CompanyName" , strCompanyName));//公司名稱

        // ===============
        cryReportSource.ReportDocument.SetDataSource(DT);

        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
        ViewState["Sourcedata"] = DT;

        JsUtility.CloseWaitScreenAjax(UpdatePanel1 , "");    //關閉執行等待畫面        
    }


    private ListItem Li(string text , string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value.Trim();
        return li;
    }

    private CrystalDecisions.Web.Parameter NewParameter(string Name,string Value)
    {
        CrystalDecisions.Web.Parameter NewParameter = new CrystalDecisions.Web.Parameter();
        NewParameter.Name = Name;
        NewParameter.DefaultValue = Value;
        return NewParameter;
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