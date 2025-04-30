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
             


public partial class GLR0150 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLB020123";
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
        int icalendersYear = DateTime.Now.AddYears(-10).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        CrystalReportViewer1.DisplayGroupTree = false;
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtDateS.CssClass = "JQCalendar";
        txtDateE.CssClass = "JQCalendar";
        txtMakeDateS.CssClass = "JQCalendar";
        txtMakeDateE.CssClass = "JQCalendar";
        // 需要執行等待畫面的按鈕
		btnQuery.Attributes.Add("onClick", "drawWait('')");
        if (!IsPostBack)
        {
            DrpcompanyList.SelectValue = Session["Company"].ToString();

        }
        else
        {
            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

                //int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
                //CrystalReportViewer1.Zoom(intPageZoom);
                CryReportSource.Report.FileName = ViewState["ReportName"].ToString();
                CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
               
            }
        
        }
        //傳票日期
        string strScript = "return GetPromptTheDate(" + txtDateS.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_StartDate.Attributes.Add("onclick", strScript);

        strScript = "return GetPromptTheDate(" + txtDateE.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_EndDate.Attributes.Add("onclick", strScript);

        strScript = "return GetPromptTheDate(" + txtMakeDateS.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_MakeStartDate.Attributes.Add("onclick", strScript);

        strScript = "return GetPromptTheDate(" + txtMakeDateE.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_MakeEndDate.Attributes.Add("onclick", strScript);

        imgbtnAcctNo.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + DrpcompanyList.SelectValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvFromAcno.ClientID + "','" + txtAcctName.ClientID + "');");
        imgbtnAcctNo.Attributes.Add("style", "cursor:hand;");

        imgbtnAcctNoEnd.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + DrpcompanyList.SelectValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvToAcctno.ClientID + "','" + txtAcctNameTo.ClientID + "');");
        imgbtnAcctNoEnd.Attributes.Add("style", "cursor:hand;");


        ScriptManager1.RegisterPostBackControl(btnQuery);

    }


    protected void bindata()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------
         DataTable DT;
         _MyDBM = new DBManger();
         _MyDBM.New();
         string strSQL = "";
         SqlCommand sqlcmd = new SqlCommand();
         string strReportname = "";
         string myVouncherSDate = "";
         string myVouncherEDate = "";
         string myEntrySDate = "";
         string myEntryEDate="";
        


      
        //check公司

         if (DrpcompanyList.SelectValue == "")
         {
             DrpcompanyList.Focus();
             JsUtility.ClientMsgBoxAjax("請選擇查詢公司！", UpdatePanel1, "a");
             JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
             return;         
         }



        // Check 起會計科目
        if (wrvFromAcno.Text == "")
        {
            wrvFromAcno.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        // Check 迄會計科目
        if (wrvToAcctno.Text == "")
        {
            wrvToAcctno.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }




        if (String.Compare(wrvFromAcno.Text, wrvToAcctno.Text) > 0)
        {
            wrvFromAcno.Focus();
            JsUtility.ClientMsgBoxAjax("起始會計科目不可大於迄止會計科目！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }

        

        //傳票起始日期



        if (txtDateS.Text != "")
        {
            myVouncherSDate = _UserInfo.SysSet.ToADDate(txtDateS.Text);

            if (myVouncherSDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
                txtDateS.Focus();
                return;
            }
            myVouncherSDate = myVouncherSDate.Replace("/", "");

        }

        //傳票結束日期
        if (txtDateE.Text != "")
        {
            myVouncherEDate = _UserInfo.SysSet.ToADDate(txtDateE.Text);

            if (myVouncherEDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
                txtDateS.Focus();
                return;
            }
            myVouncherEDate = myVouncherEDate.Replace("/", "");

        }

        if (txtDateS.Text != "" && txtDateE.Text != "")
        {
            if (String.Compare(myVouncherSDate, myVouncherEDate) > 0)
            {
                
                JsUtility.ClientMsgBoxAjax("起始日期不可大於迄止日期！", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
                return;
            }

        
        }




        //製票起始日期
        if (txtMakeDateS.Text != "")
        {
            myEntrySDate = _UserInfo.SysSet.ToADDate(txtMakeDateS.Text);

            if (myEntrySDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
                txtMakeDateS.Focus();
                return;
            }
            myEntrySDate = myEntrySDate.Replace("/", "");
        }


        //製票結束日期
        if (txtMakeDateE.Text != "")
        {
            myEntryEDate = _UserInfo.SysSet.ToADDate(txtMakeDateE.Text);

            if (myEntryEDate == "1912/01/01")
            {
                JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
                txtMakeDateE.Focus();
                return;
            }
            myEntryEDate = myEntryEDate.Replace("/", "");
        }


        if (txtMakeDateS.Text != "" && txtMakeDateE.Text != "")
        {
            if (String.Compare(myEntrySDate, myEntryEDate) > 0)
            {
               
                JsUtility.ClientMsgBoxAjax("起始日期不可大於迄止日期！", UpdatePanel1, "a");
                JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
                return;
            }


        }








        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數，並計算產生報表結果資料
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company ='" + DrpcompanyList.SelectValue + "'";
        DT = _MyDBM.ExecuteDataTable(strSQL);

        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
        //執行計算損益表資料之Stored Procedure        
              
       sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=DrpcompanyList.SelectValue;
       sqlcmd.Parameters.Add("@AcctNoStart",SqlDbType.Char).Value=wrvFromAcno.Text;
       sqlcmd.Parameters.Add("@AcctNoEnd",SqlDbType.Char).Value=wrvToAcctno.Text;
       sqlcmd.Parameters.Add("@CreateUser",SqlDbType.Char).Value=txtCreater.Text;
       sqlcmd.Parameters.Add("@VoucherSDate",SqlDbType.Char).Value=myVouncherSDate;
       sqlcmd.Parameters.Add("@VoucherEDate",SqlDbType.Char).Value=myVouncherEDate;
       sqlcmd.Parameters.Add("@VoucherEntrySDate",SqlDbType.Char).Value=myEntrySDate;
       sqlcmd.Parameters.Add("@VoucherEntryEDate",SqlDbType.Char).Value=myEntryEDate;

        


       DT = _MyDBM.ExecStoredProcedure("dbo.sp_GLB020123", sqlcmd.Parameters);

     
             


     
        // 設值給報表參數
       // 設值給報表參數
       CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
       cparam.Name = "CompanyName";
       cparam.DefaultValue = myCompanyName;
       CryReportSource.Report.Parameters.Add(cparam);             

       CrystalDecisions.Web.Parameter voucherSparam = new CrystalDecisions.Web.Parameter();
       voucherSparam.Name = "voucherStartDate";
       voucherSparam.DefaultValue = myVouncherSDate;
       CryReportSource.Report.Parameters.Add(voucherSparam);

       CrystalDecisions.Web.Parameter voucherEparam = new CrystalDecisions.Web.Parameter();
       voucherEparam.Name = "voucherEndDate";
       voucherEparam.DefaultValue = myVouncherEDate;
       CryReportSource.Report.Parameters.Add(voucherEparam);

       CrystalDecisions.Web.Parameter voucherCSparam = new CrystalDecisions.Web.Parameter();
       voucherCSparam.Name = "voucherCreatDateS";
       voucherCSparam.DefaultValue = myEntrySDate;
       CryReportSource.Report.Parameters.Add(voucherCSparam);

       CrystalDecisions.Web.Parameter voucherCEparam = new CrystalDecisions.Web.Parameter();
       voucherCEparam.Name = "voucherCreatDateE";
       voucherCEparam.DefaultValue = myEntryEDate;
       CryReportSource.Report.Parameters.Add(voucherCEparam);
    



    

        // 決定報表樣式
     

        if (DT.Rows.Count > 0)
        {          
            CryReportSource.ReportDocument.SetDataSource(DT);
            CrystalReportViewer1.DataBind();
            ViewState["Sourcedata"] = DT;
            ViewState["ReportName"] = strReportname;
          
        }
        
        else
        {
            JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");
        
        }



        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
    
    }


   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        bindata();
    }

  

    
    
	
}
