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

public partial class GLR0330    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0330";
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
            Drpcompany.SelectValue = "20";
            BindReportdata(Drpcompany.SelectValue);
            BindAcctYear(Drpcompany.SelectValue);
            BindDepatrment(Drpcompany.SelectValue);
			
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
        Drpcompany.SelectedChanged += new UserControl_CompanyList.SelectedIndexChanged(Selected_IndexChanged);
      

        ScriptManager1.RegisterPostBackControl(btnQuery);
	}

	

	protected void btnQuery_Click(object sender, ImageClickEventArgs e)
	{
        BindReportData();
	}


    protected void BindReportData()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
        string strSQL = "";
        DataTable DT;
        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();
        


        // Check 會計年度
        string myAcctYear = DrpAcctYear.SelectedValue;
        if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
        {           
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
             
       

        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company = '" +Drpcompany.SelectValue+"'";

        DT = _MyDBM.ExecuteDataTable(strSQL);

        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
       
        // 報表名稱
        string myReportName = SplitViaDash(DrpReportCode.SelectedItem.Text, 1);
        // 成本中心
        string myDept =SplitViaDash(DrpDepart.SelectedItem.Text, 1);
       
        // 設值給報表參數

        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter Acctyearparam = new CrystalDecisions.Web.Parameter();
        Acctyearparam.Name = "AcctYear";
        Acctyearparam.DefaultValue = myAcctYear;
        CryReportSource.Report.Parameters.Add(Acctyearparam);

        CrystalDecisions.Web.Parameter Reportparam = new CrystalDecisions.Web.Parameter();
        Reportparam.Name = "ReportName";
        Reportparam.DefaultValue = myReportName;
        CryReportSource.Report.Parameters.Add(Reportparam);              

        CrystalDecisions.Web.Parameter departparam = new CrystalDecisions.Web.Parameter();
        departparam.Name = "Department";
        departparam.DefaultValue = myDept;
        CryReportSource.Report.Parameters.Add(departparam);

        CrystalDecisions.Web.Parameter YearIdparam = new CrystalDecisions.Web.Parameter();
        YearIdparam.Name = "YearId";
        YearIdparam.DefaultValue = DrpYearId.Text;
        CryReportSource.Report.Parameters.Add(YearIdparam);
            

        // 執行計算損益表資料之Stored Procedure


        


        
         sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=Drpcompany.SelectValue;
         sqlcmd.Parameters.Add("@Department",SqlDbType.Char).Value=DrpDepart.SelectedValue;
         sqlcmd.Parameters.Add("@ReportID",SqlDbType.Char).Value=DrpReportCode.SelectedValue.Substring(0,3);
         sqlcmd.Parameters.Add("@AcctYear",SqlDbType.Decimal).Value=myAcctYear;               
         sqlcmd.Parameters.Add("@YearId",SqlDbType.TinyInt).Value=DrpYearId.SelectedValue;
        

        DT = _MyDBM.ExecStoredProcedure("sp_GLR0330",sqlcmd.Parameters);

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
        strSQL = @"SELECT distinct Convert(char(4), AcctYear) AS AcctYear 
                     FROM GLAcctPeriod WHERE company='" + company + "'";
        dt.Clear();
        dt = _MyDBM.ExecuteDataTable(strSQL);

        if (dt.Columns.Count != 0)
        {
            FillDropDownList(DrpAcctYear, "AcctYear", "AcctYear", dt);
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
        ListItem li = new ListItem();
        li.Text = "-全部-";
        li.Value = "";

        strSQL = @"SELECT Rtrim(Depcode) AS Code,Rtrim(Depcode)+'-'+DepName AS DepName FROM
                    DEPARTMENT where company='" + company + "'";
        dt.Clear();
        dt = _MyDBM.ExecuteDataTable(strSQL);

        if (dt.Columns.Count != 0)
        {
            FillDropDownList(DrpDepart, "Code", "DepName", dt);
            DrpDepart.Items.Insert(0, li);
           
        }

    }


    protected void Selected_IndexChanged(object sender ,EventArgs e)
    {

        BindReportdata(Drpcompany.SelectValue);
        BindAcctYear(Drpcompany.SelectValue);
        BindDepatrment(Drpcompany.SelectValue);
    
    }


    // 傳回 [代號 - 名稱] 字串組合之第 nIndex 個值
    private string SplitViaDash(string strInput, int nIndex)
    {
        char[] delimiterChars = { '-' };
        string[] words = strInput.Split(delimiterChars);
        if (nIndex > words.GetUpperBound(0)) return "";
        else return words[nIndex].Trim();
    }
}
