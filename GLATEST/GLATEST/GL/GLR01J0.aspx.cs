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

public partial class GLR01J0 : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _SystemId = "EBOSGL";
    string _ProgramId = "GLR01J0";
    DBManger _MyDBM;


    protected void Page_PreInit(object sender, EventArgs e)
    {
        _MyDBM = new DBManger();
        _MyDBM.New();
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
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtEntrySdate.CssClass = "JQCalendar";
        txtEntryEdate.CssClass = "JQCalendar";
        txtVouncherSDate.CssClass = "JQCalendar";
        txtVouncherEDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            DrpCompany.SelectValue = Session["Company"].ToString();
            VoucherSourceS.SetCodeList("AH17", 4, "全部");
            VoucherSourceS.StyleAdd("width", "125px");
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
        Binddata();
    }

    protected void Binddata()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------

        string strSQL = "";

        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT;
        SqlCommand sqlcmd =  new SqlCommand();
        

        // Check 傳票日期
        string myVchStartDate = "";
        string myVchEndDate = "";
        string myEntStartDate = "";
        string myEntEndDate = "";
        string myVchStartNo = txtVoucherSNo.Text.Trim();
        string myVchEndNo = txtVoucherENo.Text.Trim();
        string myVchOwner = "";
        string myVchSource = "";

        //string PeriodClose = "";
        if (txtVouncherSDate.Text.Trim() == "" && txtVouncherEDate.Text.Trim() == "" && txtEntrySdate.Text.Trim() == "" && txtEntryEdate.Text.Trim() == "" && txtVoucherSNo.Text.Trim() == "" && txtVoucherENo.Text.Trim() == "")
        {
          
            JsUtility.ClientMsgBoxAjax("請輸入欲查詢之日期範圍或是傳票號碼範圍。", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        //轉換日期

        if (txtEntrySdate.Text.Trim() != "")
        {

            myEntStartDate = _UserInfo.SysSet.ToADDate(txtEntrySdate.Text.Trim());

            if (myEntStartDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                txtEntrySdate.Focus();
                return;
            }
        }

        if (txtEntryEdate.Text.Trim()!="")
        {

            myEntEndDate = _UserInfo.SysSet.ToADDate(txtEntryEdate.Text.Trim());

            if (myEntEndDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                txtEntryEdate.Focus();
                return;
            }
        }


        if (txtVouncherSDate.Text.Trim() != "")
        {
            myVchStartDate = _UserInfo.SysSet.ToADDate(txtVouncherSDate.Text.Trim());

            if (myVchStartDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                txtVouncherSDate.Focus();
                return;
            }
        }

        if (txtVouncherEDate.Text.Trim() != "")
        {
            myVchEndDate = _UserInfo.SysSet.ToADDate(txtVouncherEDate.Text.Trim());

            if (myVchEndDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
                txtVouncherEDate.Focus();
                return;
            }
        }


        if (myVchStartDate != "" && myVchEndDate != "")
        {
            if (String.Compare(myVchStartDate, myVchEndDate) > 0)
            {
                JsUtility.ClientMsgBoxAjax("傳票起始日期不可大於傳票迄止日期！", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
                return;
            }
        }
        else if ((myVchStartDate != "" && myVchEndDate == "") || (myVchStartDate == "" && myVchEndDate != ""))
        {
            JsUtility.ClientMsgBoxAjax("傳票日期必須有起訖！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }


        if (myEntStartDate != "" && myEntEndDate != "")
        {
            if (String.Compare(myEntStartDate, myEntEndDate) > 0)
            {

                JsUtility.ClientMsgBoxAjax("製票起始日期不可大於迄止日期！", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
                return;
            }
        }
        else if ((myEntStartDate != "" && myEntEndDate == "") || (myEntStartDate == "" && myEntEndDate != ""))
        {
            JsUtility.ClientMsgBoxAjax("製票日期必須有起訖！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }


        if (myVchStartNo != "" && myVchEndNo != "")
        {
            if (String.Compare(myVchStartNo, myVchEndNo) > 0)
            {
                txtVoucherSNo.Focus();
                JsUtility.ClientMsgBoxAjax("傳票起始號碼不可大於傳票迄止號碼！", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
                return;
            }
        }
        else if ((myVchStartNo != "" && myVchEndNo == "") || (myVchStartNo == "" && myVchEndNo != ""))
        {
            JsUtility.ClientMsgBoxAjax("傳票起始號碼必須有起訖！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }



        myVchStartDate = myVchStartDate.Replace("/","");
        myVchEndDate = myVchEndDate.Replace("/","");
        myEntStartDate =myEntStartDate.Replace("/","") ;
        myEntEndDate = myEntEndDate.Replace("/","");
        
        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        string myCalendarType = "";
        string myDateFormat = "";

        strSQL = string.Format(@"Select C.CompanyName as CompanyName,G.CalendarType as
 CalendarType,G.[DateFormat] as DateFomat From
 Company C,GLParm G Where C.Company = G.Company and 
C.Company = '{0}'", DrpCompany.SelectValue);

        DT = _MyDBM.ExecuteDataTable(strSQL);
        
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        if (DT.Rows.Count != 0) myCalendarType = DT.Rows[0]["CalendarType"].ToString();
        if (DT.Rows.Count != 0) myDateFormat = DT.Rows[0]["DateFomat"].ToString();


        DT.Clear();

        strSQL = "dbo.sp_GLR01J0";
        
       

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectValue;
        sqlcmd.Parameters.Add("@VoucherEntryDateS", SqlDbType.Char).Value =myEntStartDate;
        sqlcmd.Parameters.Add("@VoucherEntryDateE", SqlDbType.Char).Value =myEntEndDate;
        sqlcmd.Parameters.Add("@VoucherDateS", SqlDbType.Char).Value = myVchStartDate;
        sqlcmd.Parameters.Add("@VoucherDateE", SqlDbType.Char).Value = myVchEndDate;
        sqlcmd.Parameters.Add("@VoucherNoS", SqlDbType.Char).Value = myVchStartNo;
        sqlcmd.Parameters.Add("@VoucherNoE", SqlDbType.Char).Value = myVchEndNo;
        sqlcmd.Parameters.Add("@VoucherOwner", SqlDbType.VarChar).Value = myVchOwner;
        sqlcmd.Parameters.Add("@VoucherSource", SqlDbType.VarChar).Value = myVchSource;
        sqlcmd.Parameters.Add("@calendarType", SqlDbType.Char).Value = myCalendarType;
        sqlcmd.Parameters.Add("@dateType", SqlDbType.Char).Value = myDateFormat;
                

        DT = _MyDBM.ExecStoredProcedure(strSQL, sqlcmd.Parameters);
        
        ////-------------------
        //JsUtility.ClientMsgBoxAjax("ok", UpdatePanel1, "a");
        ////------------------

        // 設值給報表參數


        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter Ctypeparam = new CrystalDecisions.Web.Parameter();
        Ctypeparam.Name = "CalendarType";
        Ctypeparam.DefaultValue = myCalendarType;
        CryReportSource.Report.Parameters.Add(Ctypeparam);

        CrystalDecisions.Web.Parameter Edateparam = new CrystalDecisions.Web.Parameter();
        Edateparam.Name = "DateFomat";
        Edateparam.DefaultValue = myDateFormat;
        CryReportSource.Report.Parameters.Add(Edateparam);

        CryReportSource.ReportDocument.SetDataSource(DT);
        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
        ViewState["Sourcedata"] = DT;

        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    }

   

   
 
    //金額3位1撇
    protected string TransData(Decimal tValue)
    {
        string tRtnValue = "";

        tRtnValue = tValue.ToString("#,0.00");

        return tRtnValue;
    }

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

}
