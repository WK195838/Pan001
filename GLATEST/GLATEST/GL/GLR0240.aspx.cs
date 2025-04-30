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


public partial class GLR0240    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0240";
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

               // int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
               // CrystalReportViewer1.Zoom(intPageZoom);

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


      
        if (DrpReportCode.SelectedValue == "")
        {
            JsUtility.ClientMsgBoxAjax("報表代號不可空白！", UpdatePanel1, "");
            DrpReportCode.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
            return;        
        }
              

        // 取出指定會計年度及指定期數的之相關資訊
        string PeriodClose = "";
        // 報表各欄抬頭
        string myHead11 = "", myHead12 = "", myHead13 = "", myHead14 = "", myHead15 = "", myHead16 = "";
        string myHead21 = "", myHead22 = "", myHead23 = "", myHead24 = "", myHead25 = "", myHead26 = "";

        strsql=string.Format(@"Select PeriodClose, Head1, Head2, Head3, Head4, Head5,Head6
          From dbo.fnGLR0240ReportHead('{0}',{1},'{2}')",Drpcompany.SelectedValue,myAcctYear,DrpAcctPeriod.SelectedValue);
          

        DT = _MyDBM.ExecuteDataTable(strsql); 
            
          
        if (DT.Rows.Count != 0)
        {
            PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
            myHead11 = DT.Rows[0]["Head1"].ToString();
            myHead12 = DT.Rows[0]["Head2"].ToString();
            myHead13 = DT.Rows[0]["Head3"].ToString();
            myHead14 = DT.Rows[0]["Head4"].ToString();
            myHead15 = DT.Rows[0]["Head5"].ToString();
            myHead16 = DT.Rows[0]["Head6"].ToString();
            myHead21 = DT.Rows[1]["Head1"].ToString();
            myHead22 = DT.Rows[1]["Head2"].ToString();
            myHead23 = DT.Rows[1]["Head3"].ToString();
            myHead24 = DT.Rows[1]["Head4"].ToString();
            myHead25 = DT.Rows[1]["Head5"].ToString();
            myHead26 = DT.Rows[1]["Head6"].ToString();
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
        sqlcmd.Parameters.Add("@AcctPeriod",SqlDbType.Char).Value=DrpAcctPeriod.SelectedValue.Trim();

        DT = _MyDBM.ExecStoredProcedure("dbo.sp_GLR0240", sqlcmd.Parameters);
        

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

        CrystalDecisions.Web.Parameter Acctperiodparam = new CrystalDecisions.Web.Parameter();
        Acctperiodparam.Name = "AcctPeriod";
        Acctperiodparam.DefaultValue = DrpAcctPeriod.SelectedValue.Trim();
        CryReportSource.Report.Parameters.Add(Acctperiodparam);

        CrystalDecisions.Web.Parameter Headparam1 = new CrystalDecisions.Web.Parameter();
        Headparam1.Name = "Head11";
        Headparam1.DefaultValue = myHead11;
        CryReportSource.Report.Parameters.Add(Headparam1);

        CrystalDecisions.Web.Parameter Headparam12 = new CrystalDecisions.Web.Parameter();
        Headparam12.Name = "Head12";
        Headparam12.DefaultValue = myHead12;
        CryReportSource.Report.Parameters.Add(Headparam12);

        CrystalDecisions.Web.Parameter Headparam13 = new CrystalDecisions.Web.Parameter();
        Headparam13.Name = "Head13";
        Headparam13.DefaultValue = myHead13;
        CryReportSource.Report.Parameters.Add(Headparam13);

        CrystalDecisions.Web.Parameter Headparam14 = new CrystalDecisions.Web.Parameter();
        Headparam14.Name = "Head14";
        Headparam14.DefaultValue = myHead14;
        CryReportSource.Report.Parameters.Add(Headparam14);

        CrystalDecisions.Web.Parameter Headparam15 = new CrystalDecisions.Web.Parameter();
        Headparam15.Name = "Head15";
        Headparam15.DefaultValue = myHead15;
        CryReportSource.Report.Parameters.Add(Headparam15);

        CrystalDecisions.Web.Parameter Headparam16 = new CrystalDecisions.Web.Parameter();
        Headparam16.Name = "Head16";
        Headparam16.DefaultValue = myHead16;
        CryReportSource.Report.Parameters.Add(Headparam16);


        CrystalDecisions.Web.Parameter Headparam2 = new CrystalDecisions.Web.Parameter();
        Headparam2.Name = "Head21";
        Headparam2.DefaultValue = myHead21;
        CryReportSource.Report.Parameters.Add(Headparam2);

        CrystalDecisions.Web.Parameter Headparam22 = new CrystalDecisions.Web.Parameter();
        Headparam22.Name = "Head22";
        Headparam22.DefaultValue = myHead22;
        CryReportSource.Report.Parameters.Add(Headparam22);

        CrystalDecisions.Web.Parameter Headparam23 = new CrystalDecisions.Web.Parameter();
        Headparam23.Name = "Head23";
        Headparam23.DefaultValue = myHead23;
        CryReportSource.Report.Parameters.Add(Headparam23);

        CrystalDecisions.Web.Parameter Headparam24 = new CrystalDecisions.Web.Parameter();
        Headparam24.Name = "Head24";
        Headparam24.DefaultValue = myHead24;
        CryReportSource.Report.Parameters.Add(Headparam24);

        CrystalDecisions.Web.Parameter Headparam25 = new CrystalDecisions.Web.Parameter();
        Headparam25.Name = "Head25";
        Headparam25.DefaultValue = myHead25;
        CryReportSource.Report.Parameters.Add(Headparam25);

        CrystalDecisions.Web.Parameter Headparam26 = new CrystalDecisions.Web.Parameter();
        Headparam26.Name = "Head26";
        Headparam26.DefaultValue = myHead26;
        CryReportSource.Report.Parameters.Add(Headparam26);
     

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
