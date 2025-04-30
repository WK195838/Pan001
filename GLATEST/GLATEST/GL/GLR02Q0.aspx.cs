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
using CrystalDecisions.CrystalReports.Engine;



public partial class GLR02Q0    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02Q0";
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
                FillDropDownList(Drpcompany, "CompanyNo", "CompanyName", dtComp);
            }
            // 預先放入第一個公司別之損益表報表代號及會計年度
            Drpcompany.SelectedValue = "20";           
            BindAcctYear(Drpcompany.SelectedValue);
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

    //檢查與建資料
    protected void BindData()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
        string strSQL = "";
        DataTable DT= new DataTable();


        SqlCommand sqlcmd = new SqlCommand();
        _MyDBM = new DBManger();
        _MyDBM.New();

        // Check 會計年度
        string myAcctYear = DrpAcctYear.SelectedValue;
        if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
        {
            DrpAcctYear.Focus();
            JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        

        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數
        //-------------------------------------------------------------------------------
        string myStartDate = "";
        string myEndDate = "";
        string PeriodClose = "";
        // 用起始期數取開始日

        strSQL = string.Format(@"Select PeriodBegin,PeriodEnd,PeriodClose
             From dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",Drpcompany.SelectedValue,myAcctYear,DrpAcctperiod.SelectedValue);
        DT = _MyDBM.ExecuteDataTable(strSQL);       
        
        if (DT.Rows.Count != 0)
        {
            myStartDate = DT.Rows[0]["PeriodBegin"].ToString();
            myEndDate = DT.Rows[0]["PeriodEnd"].ToString();
            PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
        }
        //// 用迄止期數取截止日
      

        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company ='" + Drpcompany.SelectedValue + "'";
        
        DT = _MyDBM.ExecuteDataTable(strSQL);
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        // 關帳註記
        string myCloseRemark = "";
        if (PeriodClose != "Y") myCloseRemark = "（試算）";
        // 報表名稱
        string myReportName = " 傳票張數統計 ";
        //// 成本中心



        // 設值給報表參數

        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter remarkparam = new CrystalDecisions.Web.Parameter();
        remarkparam.Name = "CloseRemark";
        remarkparam.DefaultValue = myCloseRemark;
        CryReportSource.Report.Parameters.Add(remarkparam);

        CrystalDecisions.Web.Parameter Acctyearparam = new CrystalDecisions.Web.Parameter();
        Acctyearparam.Name = "AcctYear";
        Acctyearparam.DefaultValue = myAcctYear;
        CryReportSource.Report.Parameters.Add(Acctyearparam);


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


        CrystalDecisions.Web.Parameter periodparam = new CrystalDecisions.Web.Parameter();
        periodparam.Name = "FromPeriod";
        periodparam.DefaultValue = DrpAcctperiod.SelectedValue;
        CryReportSource.Report.Parameters.Add(periodparam);


        strSQL = @"select VoucherDate,count(*) as CountNum 
        from glvoucherhead where company=@Company and (dletflag is null or rtrim(dletflag)<>'Y')
        and voucherdate between @StartDate and @endDate group by VoucherDate";

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = Drpcompany.SelectedValue;
        sqlcmd.Parameters.Add("@StartDate", SqlDbType.Char).Value =myStartDate;
        sqlcmd.Parameters.Add("@endDate", SqlDbType.Char).Value = myEndDate;

        DT = _MyDBM.ExecuteDataTable(strSQL, sqlcmd.Parameters, CommandType.Text);



        if (DT.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(DT);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = DT;
            CrystalReportViewer1.Visible = true;
            // DT.Dispose();
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");
            CrystalReportViewer1.Visible = false;
        }
      
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    
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

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

   
}
