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
using PanPacificClass;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;


public partial class GLR0220    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0230";
    DBManger _MyDBM;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }





    protected void Page_Load(object sender, EventArgs e)
    {
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;
        if (!IsPostBack)
        {
            //載入公司資料
            string strSQL = "SELECT Company, Company + '-' + CompanyShortName AS CompanyName FROM Company";
            _MyDBM = new DBManger();
            _MyDBM.New();

            DataTable dtComp = new DataTable();

            dtComp = _MyDBM.ExecuteDataTable(strSQL);
            dtComp.Columns[0].ColumnName = "CompanyNo";
            dtComp.Columns[1].ColumnName = "CompanyName";

            if (dtComp.Columns.Count != 0)
            {
                FillDropDownList(Drpcompany, "CompanyNo", "CompanyName", dtComp);
            }
            // 預先放入第一個公司別之損益表報表代號及會計年度
            Drpcompany.SelectedValue = "20";
            BindReportdata(Drpcompany.SelectedValue);
            BindAcctYear(Drpcompany.SelectedValue);

        }

        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);

                CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
        
        }
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

    // 損益表報表代號
    /// <summary>
    /// 取得損益表代號
    /// </summary>
    /// <param name="company">公司代碼</param>
    protected void BindReportdata(string company )
    {
        string strSQL;

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dtrpt = new DataTable();
        strSQL = @"SELECT ReportID, ReportID+'-'+ReportName AS ReportNAme 
                     FROM GLReportDefHead WHERE company='" + company + "' AND reporttype='10'";
        dtrpt.Clear();
        dtrpt = _MyDBM.ExecuteDataTable(strSQL);

        if (dtrpt.Columns.Count != 0)
        {
            FillDropDownList(DrpReportCode, "ReportID", "ReportName", dtrpt);
        }    

    
    }

    // 會計年度下拉選單
    /// <summary>
    /// 會計年度
    /// </summary>
    /// <param name="company">會計年度</param>
    protected void BindAcctYear(string company)
    {
        string strSQL;

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dt = new DataTable();
        strSQL = @"SELECT AcctYear 
                     FROM GLAcctPeriod WHERE company='" + company + "'";
        dt.Clear();
        dt = _MyDBM.ExecuteDataTable(strSQL);

        if (dt.Columns.Count != 0)
        {
            FillDropDownList(DrpAcctYear, "AcctYear", "AcctYear", dt);
        }    
    
    }
    


    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }


    protected void BindData()
    {

        _MyDBM = new DBManger();
        _MyDBM.New();
        string strsql = "";
        DataTable DT;
        SqlCommand sqlcmd = new SqlCommand();
       
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
     

        // Check 會計年度
        string myAcctYear = DrpAcctYear.SelectedValue;


        if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
        {           
            JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
            DrpAcctYear.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
            return;
        }
        if (DrpReportCode.SelectedValue=="")
        {
            JsUtility.ClientMsgBoxAjax("報表不可空白！", UpdatePanel1, "");
            DrpReportCode.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
            return;        
        }

        if (string.Compare(DrpAcctPeroidFrom.SelectedValue, DrpAcctPeroidTo.SelectedValue) > 0)
        {
            JsUtility.ClientMsgBoxAjax("結束期間不得小於開始期間！", UpdatePanel1, "");
            DrpAcctYear.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
            return; 
        }
     


        // 取出指定會計年度及指定期數的之相關資訊當期開始日(8碼字串)、當期結束日
        string myStartDate = "";
        string myEndDate = "";
        string PeriodClose = "";

        //用起始期數取開始日
        strsql =string.Format(@"Select PeriodBegin, PeriodEnd, PeriodClose From 
dbo.fnGetAccPeriodInfo('{0}',{1}, '{2}')", Drpcompany.SelectedValue, myAcctYear,DrpAcctPeroidFrom.SelectedValue);

         DT = _MyDBM.ExecuteDataTable(strsql);
              
        if (DT.Rows.Count != 0)
        {
            myStartDate = DT.Rows[0]["PeriodBegin"].ToString();         
        }
        DT.Clear();

        // 用迄止期數取截止日


        strsql = string.Format(@"Select  PeriodEnd, PeriodClose From 
dbo.fnGetAccPeriodInfo('{0}',{1}, '{2}')", Drpcompany.SelectedValue, myAcctYear, DrpAcctPeroidTo.SelectedValue);

        DT = _MyDBM.ExecuteDataTable(strsql);

        if (DT.Rows.Count != 0)
        {
            myEndDate = DT.Rows[0]["PeriodEnd"].ToString();
            PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
        }
        DT.Clear();





        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        strsql = "Select CompanyName From Company Where Company = '"+Drpcompany.SelectedValue.Trim() +"'" ;
        DT = _MyDBM.ExecuteDataTable(strsql);        
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        // 關帳註記
        string myCloseRemark = "";
        if (PeriodClose != "Y") myCloseRemark = "（試算）";
        DT.Clear();

        // 執行計算損益表資料之Stored Procedure
 
        sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=Drpcompany.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@ReportID",SqlDbType.Char).Value=DrpReportCode.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@AcctYear",SqlDbType.Decimal).Value=decimal.Parse(myAcctYear);
        sqlcmd.Parameters.Add("@FromPeriod", SqlDbType.Char).Value = DrpAcctPeroidFrom.SelectedValue;
        sqlcmd.Parameters.Add("@ToPeriod", SqlDbType.Char).Value = DrpAcctPeroidTo.SelectedValue;

        DT = _MyDBM.ExecStoredProcedure("dbo.sp_ComputeRep10_Yearly",sqlcmd.Parameters);
        

        // 設值給報表參數
        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);        

        CrystalDecisions.Web.Parameter remarkparam = new CrystalDecisions.Web.Parameter();
        remarkparam.Name = "CloseRemark";
        remarkparam.DefaultValue = myCloseRemark;
        CryReportSource.Report.Parameters.Add(remarkparam);

        CrystalDecisions.Web.Parameter AcctYearparam = new CrystalDecisions.Web.Parameter();
        AcctYearparam.Name = "AcctYear";
        AcctYearparam.DefaultValue =myAcctYear;
        CryReportSource.Report.Parameters.Add(AcctYearparam);

        CrystalDecisions.Web.Parameter fromperiodparam = new CrystalDecisions.Web.Parameter();
        fromperiodparam.Name = "FromPeriod";
        fromperiodparam.DefaultValue = DrpAcctPeroidFrom.SelectedValue;
        CryReportSource.Report.Parameters.Add(fromperiodparam);

        CrystalDecisions.Web.Parameter toperiodparam = new CrystalDecisions.Web.Parameter();
        toperiodparam.Name = "ToPeriod";
        toperiodparam.DefaultValue = DrpAcctPeroidTo.SelectedValue;
        CryReportSource.Report.Parameters.Add(AcctYearparam);      


        CrystalDecisions.Web.Parameter startparam = new CrystalDecisions.Web.Parameter();
        startparam.Name = "StartDate";
        startparam.DefaultValue = myStartDate;
        CryReportSource.Report.Parameters.Add(startparam);

        CrystalDecisions.Web.Parameter endparam = new CrystalDecisions.Web.Parameter();
        endparam.Name = "EndDate";
        endparam.DefaultValue = myEndDate;
        CryReportSource.Report.Parameters.Add(endparam);

        if (DT.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(DT);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = DT;
            DT.Dispose();
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");
        
        }


        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    

       


    
    }



   

  
    protected void Drpcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindReportdata(Drpcompany.SelectedValue);
        BindAcctYear(Drpcompany.SelectedValue);  

    }
}
