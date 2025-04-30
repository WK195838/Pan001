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
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

/// <summary>
/// 2013/11/25 Michelle 修正
/// 1.原上期餘額是從網頁傳,但多科目查詢時會有問題;每個科目的上期餘額需重計
/// 2.在某些情況下二張不同報表資料會錯亂,所以需加入判斷區分之
/// </summary>
public partial class GLR02A0: System.Web.UI.Page
{    
   // private System.ComponentModel.IContainer components;

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02A0";
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
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/ModPopFunction.js").ToString());   
        CrystalReportViewer1.DisplayGroupTree = false;
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        dtStartDate.CssClass = "JQCalendar";
        dtEndDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {

            //載入公司資料
         string   strSQL = "SELECT Company, Company + '-' + CompanyShortName AS CompanyName FROM Company";
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

            Drpcompany.SelectedValue = "20";

           
        }

        else
        {
            //在某些情況下二張不同報表資料會錯亂,所以需加入判斷區分之
            string strReportPath = "";
            if (ddlPrintChoice.SelectedValue == "1")
            {
                strReportPath = Server.MapPath("GLR02A0A.rpt");
            }
            else
            {
                strReportPath = Server.MapPath("GLR02A0B.rpt");
            }
            Crysource.Report.FileName = strReportPath;

            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值
                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);
                Crysource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
        }


        //製單開始

        //會計號碼
        //起
        imgbtnAcctNoStart.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + Drpcompany.SelectedValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvFromAcno.ClientID + "','" + hidacct1.ClientID + "');");
        imgbtnAcctNoStart.Attributes.Add("style", "cursor:hand;");
        //迄
        imgbtnAcctNoEnd.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + Drpcompany.SelectedValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvToAcno.ClientID+ "','" + hidacct2.ClientID + "');");
        imgbtnAcctNoEnd.Attributes.Add("style", "cursor:hand;");

        ScriptManager1.RegisterPostBackControl(btnQuery);

    }
   
    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

    protected void BindData()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        string strSQL = "";
        SqlCommand sqlcmd = new SqlCommand();
        string strReportPath = "";
       
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
        DataTable DT;

        // Check 起迄會計科目
        if (wrvFromAcno.Text == "")
        {
            wrvFromAcno.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        if (String.Compare(wrvFromAcno.Text, wrvToAcno.Text) > 0)
        {
            wrvFromAcno.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可大於迄止會計科目！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        // Check 截止日期
        string myStartDate = dtStartDate.Text;
        string myEndDate = dtEndDate.Text;
        //因輸入名國年會跑出報表輸入值
        if (myStartDate.Replace("/","").Length > 7)
        {
           PanPacificClass.JsUtility.ClientMsgBoxAjax("請輸入民國日期或用日曆挑選！", UpdatePanel1, "");
           dtStartDate.Focus();
           return;
        }
        //因輸入名國年會跑出報表輸入值
        if (myEndDate.Replace("/","").Length > 7)
        {
           PanPacificClass.JsUtility.ClientMsgBoxAjax("請輸入民國日期或用日曆挑選！", UpdatePanel1, "");
           dtEndDate.Focus();
           return;
        }

        myStartDate = _UserInfo.SysSet.ToADDate(myStartDate);

        //因轉錯誤轉不出來為1912/01/01
        if (myStartDate == "1912/01/01")
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            dtStartDate.Focus();
            return;
        }

        myEndDate = _UserInfo.SysSet.ToADDate(myEndDate);

        //因轉錯誤轉不出來為1912/01/01
        if (myEndDate == "1912/01/01")
        {
            PanPacificClass.JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            dtEndDate.Focus();
            return;
        }
       
        

        //string PeriodClose = "";
        if (myStartDate == "" || myEndDate == "")
        {
            dtStartDate.Focus();
            JsUtility.ClientMsgBoxAjax("日期不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }
        if (String.Compare(myStartDate, myEndDate) > 0)
        {
            dtStartDate.Focus();
            JsUtility.ClientMsgBoxAjax("起始日期不可大於迄止日期！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        myStartDate = myStartDate.Replace("/", "");
        myEndDate = myEndDate.Replace("/", "");

        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名

        string myCompanyName = "";

        strSQL="Select CompanyName From Company Where Company ='"+Drpcompany.SelectedValue+"'";
        DT = _MyDBM.ExecuteDataTable(strSQL);
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        //執行計算損益表資料之Stored Procedure 
        string spName = "";
        //---報表列印方式(以不同sp建立)
        if (ddlPrintChoice.SelectedValue == "1")
        {
            spName = "dbo.sp_GLR02A0";
        }
        else
        {
            spName = "dbo.sp_GLR02A0b";
        }

        DT.Clear();

        sqlcmd.Parameters.Clear();
        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = Drpcompany.SelectedValue;
        sqlcmd.Parameters.Add("@FromAcno", SqlDbType.Char).Value = wrvFromAcno.Text;
        sqlcmd.Parameters.Add("@ToAcno", SqlDbType.Char).Value = wrvToAcno.Text;
        sqlcmd.Parameters.Add("@StartDate", SqlDbType.Char).Value = myStartDate;
        sqlcmd.Parameters.Add("@EndDate", SqlDbType.Char).Value = myEndDate;

        DT = _MyDBM.ExecStoredProcedure(spName, sqlcmd.Parameters);

        //CrystalDecisions.CrystalReports.Engine.ReportDocument Crypt = new ReportDocument();
             
       


     
        //// 設值給報表參數

        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName;
        Crysource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();
        SDateparam.Name = "StartDate";
        SDateparam.DefaultValue = myStartDate;
        Crysource.Report.Parameters.Add(SDateparam);

        CrystalDecisions.Web.Parameter Edateparam = new CrystalDecisions.Web.Parameter();
        Edateparam.Name = "EndDate";
        Edateparam.DefaultValue = myEndDate;
        Crysource.Report.Parameters.Add(Edateparam);

        //2013/11/25 Michelle 修正,原上期餘額是從網頁傳,但多科目查詢時會有問題,所以改掉;現由SP內直接抓取,此處參數已無用,故暫傳空值
        CrystalDecisions.Web.Parameter Amtparam = new CrystalDecisions.Web.Parameter();
        Amtparam.Name = "StartAmt";
        Amtparam.DefaultValue = "";
        Crysource.Report.Parameters.Add(Amtparam);

        //// 決定報表樣式
        if (ddlPrintChoice.SelectedValue == "1")
        {
            strReportPath = Server.MapPath("GLR02A0A.rpt");

        }
        else
        {
            strReportPath =Server.MapPath("GLR02A0B.rpt");
        }
       
           
            Crysource.Report.FileName = strReportPath;

            //Crypt.Load(strReportPath);
            //Crypt.SetDataSource(DT);
            //Crypt.SetParameterValue("CompanyName", myCompanyName);
            //Crypt.SetParameterValue("StartDate", myStartDate);
            //Crypt.SetParameterValue("EndDate", myEndDate);
            //Crypt.SetParameterValue("StartAmt", myCompanyName);
          
       
       


            Crysource.ReportDocument.SetDataSource(DT);
            //CrystalReportViewer1.ReportSourceID = "Crycource";
            //CrystalReportViewer1.ReportSource = Crypt;
          
            CrystalReportViewer1.DataBind();
            CrystalReportViewer1.Visible = true;
            ViewState["Sourcedata"] = DT;
            // Crypt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,"C://TEST.pdf"); 

            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    }




   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();
    }
       

    

    // 傳回 公司別代號
    public string GetCompany()
    {
        return Drpcompany.Text;
    }
    // 傳回 起始科目欄位值
    public string GetFromAcno()
    {
        return wrvFromAcno.Text;
    }
    // 傳回 迄止科目欄位值
    public string GetToAcno()
    {
        return wrvToAcno.Text;
    }
    //金額3位1撇
    protected string TransData(Decimal tValue)
    {
        string tRtnValue = "";

        tRtnValue = tValue.ToString("#,0.00");

        return tRtnValue;
    }

}
