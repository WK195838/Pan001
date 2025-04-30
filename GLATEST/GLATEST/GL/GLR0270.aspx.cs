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


public partial class GLR0270 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0270";
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
      
        ScriptManager1.RegisterPostBackControl(btnQuery);


    }

   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        binddata();
    }


    protected void binddata()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
        string strSQL = "";
        DataTable DT;
        SqlCommand sqlcmd = new SqlCommand();
        string myAcctYear = "";
        _MyDBM = new DBManger();
        _MyDBM.New();
        

        //檢查公司名
        if (Drpcompany.SelectedValue=="")
        {
            JsUtility.ClientMsgBoxAjax("請選擇公司！", UpdatePanel1, "");
            Drpcompany.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
            return;
        }

        myAcctYear = DrpAcctYear.SelectedValue;
        //會計年度
        if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
        {
            JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
            DrpAcctYear.Focus(); ;
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
            return;
        }
                
        //報表代碼

        if (DrpReportCode.SelectedValue == "")
        {
            JsUtility.ClientMsgBoxAjax("請先選擇報表代號！", UpdatePanel1, "");
            DrpReportCode.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
            return;       
        }
               
       
        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";

        DT = _MyDBM.ExecuteDataTable("Select CompanyName From Company Where Company ='"+Drpcompany.SelectedValue+"'");
             
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        

        // 取出指定年度及期數所在之當期結束日、關帳碼，並設定試算註記
        string myBaseDate = "";
        string PeriodClose = "";
        DT.Clear();

        strSQL = string.Format(@"Select PeriodEnd, PeriodClose From dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
            Drpcompany.SelectedValue,DrpAcctYear.SelectedValue,DrpAcctperiod.SelectedValue);

        DT = _MyDBM.ExecuteDataTable(strSQL);
      
        if (DT.Rows.Count != 0)
        {
            myBaseDate = DT.Rows[0]["PeriodEnd"].ToString();
            PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
        }
        DT.Clear();
        string myCloseRemark = "";
        if (PeriodClose != "Y") myCloseRemark = "（試算）";

        //執行計算損益表資料之Stored Procedure     

        sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=Drpcompany.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@ReportID",SqlDbType.Char).Value=DrpReportCode.SelectedValue;
        sqlcmd.Parameters.Add("@AcctYear",SqlDbType.Decimal).Value=decimal.Parse(DrpAcctYear.SelectedValue.Trim());
        sqlcmd.Parameters.Add("@AcctPeriod",SqlDbType.Char).Value=DrpAcctperiod.SelectedValue.Trim();
        sqlcmd.Parameters.Add("@CompareId",SqlDbType.Char).Value="2";

        DT = _MyDBM.ExecStoredProcedure("sp_GLR0260",sqlcmd.Parameters);


        // 設值給報表參數
        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter remarkparam = new CrystalDecisions.Web.Parameter();
        remarkparam.Name = "CloseRemark";
        remarkparam.DefaultValue = myCloseRemark;
        CryReportSource.Report.Parameters.Add(remarkparam);

        CrystalDecisions.Web.Parameter Baseparam = new CrystalDecisions.Web.Parameter();
        Baseparam.Name = "BaseDate";
        Baseparam.DefaultValue = myBaseDate;
        CryReportSource.Report.Parameters.Add(Baseparam);


        CrystalDecisions.Web.Parameter compareparam = new CrystalDecisions.Web.Parameter();
        compareparam.Name = "CompareId";
        compareparam.DefaultValue = "2";
        CryReportSource.Report.Parameters.Add(compareparam);





        if (DT.Rows.Count > 0)
        {
            CryReportSource.ReportDocument.SetDataSource(DT);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = DT;
           // DT.Dispose();
        }
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");

        }

        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面 
    
    
    
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
        strSQL = @"SELECT distinct ReportID, ReportID+'-'+ReportName AS ReportNAme 
                     FROM GLReportDefHead WHERE company='" + company + "' AND (reporttype='21' OR reporttype='20') ";
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

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

    protected void Drpcompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindReportdata(Drpcompany.SelectedValue);
        BindAcctYear(Drpcompany.SelectedValue);
    }
}
