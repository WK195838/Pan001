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


public partial class GLR0300    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0310";
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
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
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
                FillDropDownList(DrpCompany, "CompanyNo", "CompanyName", dtComp);
            }
            // 預先放入第一個公司別之損益表報表代號及會計年度
            DrpCompany.SelectedValue = "20";
            BindReportdata(DrpCompany.SelectedValue);
            BindAcctYear(DrpCompany.SelectedValue);
            BindDepatrment(DrpCompany.SelectedValue);
            ListItem li = new ListItem();
            li.Text = "-全部-";
            li.Value = " ";
            DrpDepartment.Items.Insert(0, li);
        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

               // int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
               // CrystalReportViewer1.Zoom(intPageZoom);

                CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }

        }
       
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();  
    }


    protected void BindData()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
         string strSQL = "";
         DataTable Dt;
         SqlCommand sqlcmd = new SqlCommand();
        
         _MyDBM = new DBManger();
         _MyDBM.New();


        // Check 會計年度
        string myAcctYear = DrpAcctyear.SelectedValue.Trim();
        if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
        {
            DrpAcctyear.Focus();
            JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        // 取出指定會計年度及指定期數的之相關資訊當期開始日(8碼字串)、當期結束日
        string myStartDate = "";
        string myEndDate = "";
        string PeriodClose = "";

        strSQL = string.Format(@"Select PeriodBegin, PeriodEnd, 
        PeriodClose From dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",DrpCompany.SelectedValue,myAcctYear,DrpAcctperiod.SelectedValue);

        Dt = _MyDBM.ExecuteDataTable(strSQL);
               
        if (Dt.Rows.Count != 0)
        {
            myStartDate = Dt.Rows[0]["PeriodBegin"].ToString();
            myEndDate = Dt.Rows[0]["PeriodEnd"].ToString();
            PeriodClose = Dt.Rows[0]["PeriodClose"].ToString();
        }

        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company = '"+DrpCompany.SelectedValue+"'";
        Dt = _MyDBM.ExecuteDataTable(strSQL);

        if (Dt.Rows.Count != 0) myCompanyName = Dt.Rows[0]["CompanyName"].ToString();
        // 關帳註記
        string myCloseRemark = "";
        if (PeriodClose != "Y") myCloseRemark = "（試算）";
        // 報表名稱


        char[] delimiterChars = { '-' };
        string[] words = DrpReportCode.SelectedItem.Text.Split(delimiterChars);          
        string myReportName = words[1].Trim();
        

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
        AcctYearparam.DefaultValue = myAcctYear;
        CryReportSource.Report.Parameters.Add(AcctYearparam);

        CrystalDecisions.Web.Parameter startparam = new CrystalDecisions.Web.Parameter();
        startparam.Name = "StartDate";
        startparam.DefaultValue = myStartDate;
        CryReportSource.Report.Parameters.Add(startparam);

        CrystalDecisions.Web.Parameter endparam = new CrystalDecisions.Web.Parameter();
        endparam.Name = "EndDate";
        endparam.DefaultValue = myEndDate;
        CryReportSource.Report.Parameters.Add(endparam);

        CrystalDecisions.Web.Parameter Reportparam = new CrystalDecisions.Web.Parameter();
        Reportparam.Name = "ReportName";
        Reportparam.DefaultValue = myReportName;
        CryReportSource.Report.Parameters.Add(Reportparam);

        CrystalDecisions.Web.Parameter Acctparam = new CrystalDecisions.Web.Parameter();
        Acctparam.Name = "AcctPeriod";
        Acctparam.DefaultValue = DrpAcctperiod.SelectedValue;
        CryReportSource.Report.Parameters.Add(Acctparam);

        CrystalDecisions.Web.Parameter DepNameparam = new CrystalDecisions.Web.Parameter();
        DepNameparam.Name = "DeptName";
        DepNameparam.DefaultValue = DrpDepartment.SelectedValue;
        CryReportSource.Report.Parameters.Add(DepNameparam);
        
        // 執行計算損益表資料之Stored Procedure
        sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=DrpCompany.SelectedValue;
        sqlcmd.Parameters.Add("@Department",SqlDbType.Char).Value=DrpDepartment.SelectedValue;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = DrpReportCode.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@AcctYear",SqlDbType.Decimal).Value=myAcctYear;
        sqlcmd.Parameters.Add("@AcctPeriod",SqlDbType.Char).Value=DrpAcctperiod.SelectedValue.Trim();

       
        Dt = _MyDBM.ExecStoredProcedure("dbo.sp_GLR0310",sqlcmd.Parameters);

        if (Dt.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(Dt);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = Dt;
            Dt.Dispose();
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");

        }
             
    
      
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    
    }


    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.Items.Clear();
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
    protected void BindReportdata(string company)
    {
        string strSQL;

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dtrpt = new DataTable();
        strSQL = @"SELECT ReportID, ReportID+'-'+ReportName AS ReportNAme 
                     FROM GLReportDefHead WHERE company='" + company + "' AND reporttype='30'";
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
            FillDropDownList(DrpAcctyear, "AcctYear", "AcctYear", dt);
        }

    }

    /// <summary>
    /// 取得公司部門
    /// </summary>
    /// <param name="company"></param>
    protected void BindDepatrment(string company)
    {
        string strSQL;

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dt = new DataTable();
        strSQL = @"SELECT Rtrim(Depcode) AS Code,Rtrim(Depcode)+'-'+DepName AS DepName FROM
                    DEPARTMENT where company='" + company + "'";
        dt.Clear();
        dt = _MyDBM.ExecuteDataTable(strSQL);

        if (dt.Columns.Count != 0)
        {
            FillDropDownList(DrpDepartment, "Code", "DepName", dt);
        }

    }




    protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindReportdata(DrpCompany.SelectedValue);
        BindAcctYear(DrpCompany.SelectedValue);
        BindDepatrment(DrpCompany.SelectedValue);
        ListItem li = new ListItem();
        li.Text = "-全部-";
        li.Value = " ";
        DrpDepartment.Items.Insert(0, li);
    }
}
