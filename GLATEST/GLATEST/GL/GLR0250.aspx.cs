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




public partial class GLR0250 : System.Web.UI.Page

{

    
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0250";
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
        string strSQL;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;

        int icalendersYear = DateTime.Now.AddYears(-10).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        Txtdate.CssClass = "JQCalendar";
       
        if (!IsPostBack)
        {
            //載入公司資料
            strSQL = "SELECT Company, Company + '-' + CompanyShortName AS CompanyName FROM Company";
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

            DrpCompany.SelectedValue = "20";

            // 預先放入第一個公司別之損益表報表代號

            BindTypeList();

        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);

                Cryrptsource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
        
        
        }


        //製單開始
        string strScript = "return GetPromptTheDate(" + Txtdate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_Date.Attributes.Add("onclick", strScript);

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
        string strsql;
        DataTable DT;
        string strReportPath;

        _MyDBM = new DBManger();
        _MyDBM.New();
        // Check 日期
        //轉換為西元年
        string myBaseDate = "";

        if (Txtdate.Text.Trim() == "")        {
            
            JsUtility.ClientMsgBoxAjax("日期不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        myBaseDate = _UserInfo.SysSet.ToADDate(Txtdate.Text.Trim());
        if (myBaseDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("日期格式錯誤", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;        
        }

        myBaseDate = myBaseDate.Replace("/", "");
        

        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";

        strsql = "Select CompanyName From Company Where Company = '"+DrpCompany.SelectedValue+"'";

        DT = _MyDBM.ExecuteDataTable(strsql);
        if (DT.Rows.Count != 0)
        {
            myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        }

        // 取出指定日期所在關帳碼，並設定試算註記
        string PeriodClose = "";
        DT.Clear();
        strsql = "Select dbo.fnAccPeriodDates('"+DrpCompany.SelectedValue+"','"+myBaseDate+"')";
        DT = _MyDBM.ExecuteDataTable(strsql);

        if (DT.Rows.Count != 0)
        {
            PeriodClose = DT.Rows[0][0].ToString().Substring(30, 1);
        }
        string myCloseRemark = "";
        if (PeriodClose != "Y") myCloseRemark = "（試算）";



        //執行計算損益表資料之Stored Procedure 

        strsql = "dbo.sp_ComputeRep2021_Date";
        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectedValue;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = DrpReportType.SelectedValue;
        sqlcmd.Parameters.Add("@BaseDate", SqlDbType.Char).Value = myBaseDate;
        sqlcmd.Parameters.Add("@ReportStyle", SqlDbType.Char).Value = ddlReportStyle.SelectedValue;      

        DT=_MyDBM.ExecStoredProcedure(strsql, sqlcmd.Parameters);


        // 設值給報表參數

        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        Cryrptsource.Report.Parameters.Add(cparam);        

        CrystalDecisions.Web.Parameter remarkparam = new CrystalDecisions.Web.Parameter();
        remarkparam.Name = "CloseRemark";
        remarkparam.DefaultValue = myCloseRemark;
        Cryrptsource.Report.Parameters.Add(remarkparam);      

        CrystalDecisions.Web.Parameter Baseparam = new CrystalDecisions.Web.Parameter();
        Baseparam.Name = "BaseDate";
        Baseparam.DefaultValue = myBaseDate;
        Cryrptsource.Report.Parameters.Add(Baseparam);
    
        // 決定報表樣式
        if (ddlReportStyle.Text == "1")
        {
            strReportPath = Server.MapPath("GLR0250A.rpt");
        }
        else
        {
            strReportPath = Server.MapPath("GLR0250B.rpt");
        }

        Cryrptsource.Report.FileName = strReportPath;
        Cryrptsource.ReportDocument.SetDataSource(DT);
        CrystalReportViewer1.DataBind();
        ViewState["Sourcedata"] = DT;

        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    
    }


    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }



    /// <summary>
    /// 依照公司不同更換項目
    /// </summary>
    protected void BindTypeList()
    {
        string strSQL;

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dtComp = new DataTable();
        strSQL = @"SELECT ReportID, ReportID+'-'+ReportName AS ReportNAme 
                     FROM GLReportDefHead WHERE company='" + DrpCompany.SelectedValue + "' AND reporttype='20'";
        dtComp.Clear();
        dtComp = _MyDBM.ExecuteDataTable(strSQL);

        if (dtComp.Columns.Count != 0)
        {
            FillDropDownList(DrpReportType, "ReportID", "ReportName", dtComp);
        }

    }





    protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 由選擇之公司別動態決定報表代號之待選內容
        BindTypeList();
    }
}
