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

public partial class GLR02R0 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02R0";
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
            BindAcctYear(DrpCompany.SelectedValue);
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
		//-------------------------------------------------------------------------------
		// 畫面查詢條件檢查
		//-------------------------------------------------------------------------------
        string strSQL = "";
        DataTable DT;
        SqlCommand sqlcmd = new SqlCommand();
        _MyDBM = new DBManger();
        _MyDBM.New();


		// Check 會計年度
		string myAcctYear ="" ;
        if (DrpAcctYear.SelectedValue == "" || String.Compare(DrpAcctYear.SelectedValue, "1900") < 0 || String.Compare(DrpAcctYear.SelectedValue, "2099") > 0)
		{
			
			JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
			JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
			return;
		}

        myAcctYear = DrpAcctYear.SelectedValue;

		// Check 會計期數

		if (String.Compare(DrpFromperiod.SelectedValue, DrpToperiod.SelectedValue) > 0)
		{
			
			JsUtility.ClientMsgBoxAjax("起始會計期間不可大於迄止會計期間！", UpdatePanel1, "");
			JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
			return;
		}






        //// Check 成本中心１
      

		//-------------------------------------------------------------------------------
		// 取出報表參數相關資訊並設值給報表參數
		//-------------------------------------------------------------------------------
		string myStartDate = "";
		string myEndDate = "";
		string PeriodClose = "";
		// 用起始期數取開始日
        strSQL = string.Format(@"Select PeriodBegin From 
         dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",DrpCompany.SelectedValue,myAcctYear,DrpFromperiod.SelectedValue);

        DT = _MyDBM.ExecuteDataTable(strSQL);		
		
		if (DT.Rows.Count != 0)
			myStartDate = DT.Rows[0]["PeriodBegin"].ToString();

		// 用迄止期數取截止日
        strSQL = string.Format(@"Select PeriodEnd,PeriodClose From 
         dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')", DrpCompany.SelectedValue, myAcctYear,DrpToperiod.SelectedValue);

        DT = _MyDBM.ExecuteDataTable(strSQL);	
		
		if (DT.Rows.Count != 0)
		{
			myEndDate = DT.Rows[0]["PeriodEnd"].ToString();
			PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
		}

		// 取出公司全名
		string myCompanyName = "";
	
        strSQL="SELECT CompanyName From Company Where Company='"+DrpCompany.SelectedValue +"'";

        DT = _MyDBM.ExecuteDataTable(strSQL);       

		if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        
		// 關帳註記
		string myCloseRemark = "";
		if (PeriodClose != "Y") myCloseRemark = "（試算）";
		// 報表名稱
		string myReportName = " 傳票刪除統計 ";

       
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

        CrystalDecisions.Web.Parameter Fromparam = new CrystalDecisions.Web.Parameter();
        Fromparam.Name = "FromPeriod";
        Fromparam.DefaultValue = DrpFromperiod.SelectedValue;
        CryReportSource.Report.Parameters.Add(Fromparam);

        CrystalDecisions.Web.Parameter Toparam = new CrystalDecisions.Web.Parameter();
        Toparam.Name = "ToPeriod";
        Toparam.DefaultValue = DrpToperiod.SelectedValue;
        CryReportSource.Report.Parameters.Add(Toparam);

        CrystalDecisions.Web.Parameter Startparam = new CrystalDecisions.Web.Parameter();
        Startparam.Name = "StartDate";
        Startparam.DefaultValue = myStartDate;
        CryReportSource.Report.Parameters.Add(Startparam);

        CrystalDecisions.Web.Parameter Endparam = new CrystalDecisions.Web.Parameter();
        Endparam.Name = "EndDate";
        Endparam.DefaultValue = myEndDate;
        CryReportSource.Report.Parameters.Add(Endparam);
        
        CrystalDecisions.Web.Parameter Reportparam = new CrystalDecisions.Web.Parameter();
        Reportparam.Name = "ReportName";
        Reportparam.DefaultValue = myReportName;
        CryReportSource.Report.Parameters.Add(Reportparam);

        strSQL = @"select VoucherDate,VoucherNo,LstChgDateTime from glvoucherhead
           where company=@company AND RTRIM(dletflag)='Y'
           and voucherdate between @Startdate AND @Enddate ";


        sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = DrpCompany.SelectedValue;
        sqlcmd.Parameters.Add("@Startdate", SqlDbType.Char).Value = myStartDate;
        sqlcmd.Parameters.Add("@Enddate", SqlDbType.Char).Value = myEndDate;


        DT = _MyDBM.ExecuteDataTable(strSQL, sqlcmd.Parameters, CommandType.Text);
        
        string myCountNum = DT.Rows.Count.ToString();

        CrystalDecisions.Web.Parameter Countparam = new CrystalDecisions.Web.Parameter();
        Countparam.Name = "CountNum";
        Countparam.DefaultValue = myCountNum;
        CryReportSource.Report.Parameters.Add(Countparam);

        if (DT.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(DT);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = DT;
            CrystalReportViewer1.Visible =true;
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





    protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAcctYear(DrpCompany.SelectedValue);
    }
}
