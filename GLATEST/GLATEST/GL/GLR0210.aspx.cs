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


public partial class Template_WebReport : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0210";
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
        string strSQL = "";
        int icalendersYear = DateTime.Now.AddYears(-5).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        //GeneralModalPopup.Ajax.ModalPopup.RegisterLoadingWebButton(LoadingPanel,500,300);
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        TxtAccendDate.CssClass = "JQCalendar";
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

            DrpCompany.SelectedValue="20";

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
               
                cryrptsrc.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();               
            }
        
        }
       

        //製單開始
        string strScript = "return GetPromptTheDate(" + TxtAccendDate.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_endDate.Attributes.Add("onclick", strScript);

        ScriptManager1.RegisterPostBackControl(btnQuery);

    }

   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        
        _MyDBM = new DBManger();
        _MyDBM.New();

        // Check 會計期間之起始期數
        string StartPeriod = tbxStartPeriod.Text.Trim();
        if (StartPeriod == "" || String.Compare(StartPeriod, "01") < 0 || String.Compare(StartPeriod, "13") > 0)
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("會計期間之起始期數不可空白，而且必須介於 01 ~ 13 之間！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
            tbxStartPeriod.Focus();
            return;
        }

        //    // Check 會計期間之迄止日期
        string BaseDate = TxtAccendDate.Text.Trim();
        if (BaseDate == "")
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("會計期間之迄止日期不可空白！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
            TxtAccendDate.Focus();
            return;
        }

        // 取出指定日期之會計年度所在之指定期數的當期開始日(8碼字串)
        string myStartDate = "";
        string fromPeriodClose = "";
        StartPeriod = StartPeriod.PadRight(2, '0');
        BaseDate = _UserInfo.SysSet.ToADDate(BaseDate);


        //因轉錯誤轉不出來為1912/01/01
        if (BaseDate == "1912/01/01")
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
            TxtAccendDate.Focus();
            return;
        }

        //
        BaseDate=BaseDate.Replace("/","");


        String sql = "Select PeriodBegin, PeriodClose From dbo.fnGetAccPeriodInfo('" + DrpCompany.SelectedValue + "', " +
            BaseDate.Substring(0, 4) + ", '" + StartPeriod + "')";


        DataTable DT = _MyDBM.ExecuteDataTable(sql);

        //  DataSet ds = CliUtils.ExecuteSql("SGLWF03_View", "View_GLWF03", sql, true, null);// 用ServerPackage的名字及任一 InfoCommand的名字來執行取得DataSet
        if (DT.Rows.Count != 0)
        {
            myStartDate = DT.Rows[0]["PeriodBegin"].ToString();
            fromPeriodClose = DT.Rows[0]["PeriodClose"].ToString();
        }
        if (String.Compare(myStartDate, BaseDate) > 0)
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("會計期間起始期數所在之開始日期不可大於之迄止日期！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
            tbxStartPeriod.Focus();
            return;
        }

        //    // 執行計算損益表資料之Stored Procedure 
        sql = "dbo.sp_ComputeRep10";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectedValue;
        sqlcmd.Parameters.Add("@ReportID", SqlDbType.Char).Value = DrpReportCode.SelectedValue;
        sqlcmd.Parameters.Add("@BaseDate", SqlDbType.Char).Value = BaseDate;
        sqlcmd.Parameters.Add("@AmtType", SqlDbType.Char).Value = "4";
        sqlcmd.Parameters.Add("@StartPeriod", SqlDbType.Char).Value = StartPeriod;


        _MyDBM.ExecStoredProcedure(sql, sqlcmd.Parameters);
        //CliUtils.CallMethod("SGLWF03_View", "ComputeRep10",
        //    new object[] { wddlCompany.Text, wddlReportID.Text.Substring(0, 3), BasteDate, "4", StartPeriod });

        //-------------------------------------------------------------------------------
        // 設值給報表參數
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        DT.Clear();
        sql = "Select CompanyName From Company Where Company = '" + DrpCompany.SelectedValue + "'";
        // ds = CliUtils.ExecuteSql("SGLWF03_View", "View_GLWF03", sql, true, null);// 用ServerPackage的名字及任一 InfoCommand的名字來執行取得DataSet
        DT = _MyDBM.ExecuteDataTable(sql);

        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        //    // 取出迄止日所在期數之關帳碼
        string toPeriodClose = "";
        sql = "Select dbo.fnAccPeriodDates('" + DrpCompany.SelectedValue + "', '" + BaseDate + "')";

        //ds = CliUtils.ExecuteSql("SGLWF03_View", "View_GLWF03", sql, true, null);// 用ServerPackage的名字及任一 InfoCommand的名字來執行取得DataSet
        DT.Clear();
        string myCloseRemark = "";
        if (DT.Rows.Count != 0) toPeriodClose = DT.Rows[0][0].ToString().Substring(30, 1);
        {
            if (fromPeriodClose != "Y" || toPeriodClose != "Y")
            { myCloseRemark = "（試算）"; }
        }


        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        cryrptsrc.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter remarkparam = new CrystalDecisions.Web.Parameter();
        remarkparam.Name = "CloseRemark";
        remarkparam.DefaultValue = myCloseRemark;
        cryrptsrc.Report.Parameters.Add(remarkparam);

        CrystalDecisions.Web.Parameter startparam = new CrystalDecisions.Web.Parameter();
        startparam.Name = "StartDate";
        startparam.DefaultValue =myStartDate ;
        cryrptsrc.Report.Parameters.Add(startparam);

        CrystalDecisions.Web.Parameter endparam = new CrystalDecisions.Web.Parameter();
        endparam.Name = "EndDate";
        endparam.DefaultValue = BaseDate;
        cryrptsrc.Report.Parameters.Add(endparam);

        DT.Clear();
        sql = "SELECT * FROM GLWF03";
        DT = _MyDBM.ExecuteDataTable(sql);
        cryrptsrc.ReportDocument.SetDataSource(DT);
        CrystalReportViewer1.DataBind();        
        ViewState["Sourcedata"] = DT;
        DT.Dispose();
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");     
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
                     FROM GLReportDefHead WHERE company='" + DrpCompany.SelectedValue + "' AND reporttype='10'";
        dtComp.Clear();
        dtComp = _MyDBM.ExecuteDataTable(strSQL);

        if (dtComp.Columns.Count != 0)
        {
            FillDropDownList(DrpReportCode, "ReportID", "ReportName", dtComp);
        }    
    
    }





   
    protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 由選擇之公司別動態決定報表代號之待選內容
        BindTypeList();
    }
}
