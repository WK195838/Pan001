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


public partial class GLR0150 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR0150";
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
        CrystalReportViewer1.HasToggleGroupTreeButton = false;             
        // 需要執行等待畫面的按鈕
		  btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        txtDateS.Attributes.Add("onclick", "JQ()");
        TxtvoucherSDate.Attributes.Add("onclick", "JQ()");
        TxtvoucherEDate.Attributes.Add("onclick", "JQ()");
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

		  ScriptManager1.RegisterPostBackControl(btnQuery);
        imgbtnAcctNoFrom.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + DrpcompanyList.SelectValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvFromAcno.ClientID + "','" + txtAcctNameF.ClientID + "');");
        imgbtnAcctNoFrom.Attributes.Add("style", "cursor:hand;");

        imgbtnAcctNoTo.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + DrpcompanyList.SelectValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + wrvToAcctno.ClientID + "','" + txtAcctNameT.ClientID + "');");
        imgbtnAcctNoTo.Attributes.Add("style", "cursor:hand;");

		 
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

			string myVoucherSdate = "";
			string myVoucherEdate = "";
      
        //check公司

         if (DrpcompanyList.SelectValue == "")
         {
             DrpcompanyList.Focus();
             JsUtility.ClientMsgBoxAjax("請選擇查詢公司！", UpdatePanel1, "a");
             JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
             return;         
         }



        // Check 起迄會計科目
        if (wrvFromAcno.Text == "")
        {
            wrvFromAcno.Focus();
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

        // Check 截止日期
        string myCutDate ="";       

        if (txtDateS.Text=="")
        {
				wrvFromAcno.Focus();
            JsUtility.ClientMsgBoxAjax("截止日期不可空白！", UpdatePanel1, "a");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面 (Key值不要與 ClientMsgBoxAjax一樣即可)
            return;
        }


        myCutDate = _UserInfo.SysSet.ToADDate(txtDateS.Text);

        if (myCutDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
            txtDateS.Focus();
            return;
        }
        myCutDate = myCutDate.Replace("/", "");

		  //轉換傳票起始日期(=發票日期)

		  if (TxtvoucherSDate.Text.Trim() != "")
		  {
			  myVoucherSdate = _UserInfo.SysSet.ToADDate(TxtvoucherSDate.Text.Trim());

			  if (myVoucherSdate == "1912/01/01")
			  {
				  JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
				  JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
				  TxtvoucherSDate.Focus();
				  return;
			  }
			  myVoucherSdate = myVoucherSdate.Replace("/", "");
		  }

		  //轉換傳票結束日期(=發票日期)

		  if (TxtvoucherEDate.Text.Trim() != "")
		  {
			  myVoucherEdate = _UserInfo.SysSet.ToADDate(TxtvoucherEDate.Text.Trim());

			  if (myVoucherEdate == "1912/01/01")
			  {
				  JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
				  JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");    //關閉執行等待畫面
				  TxtvoucherEDate.Focus();
				  return;
			  }
			  myVoucherEdate = myVoucherEdate.Replace("/", "");
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
       sqlcmd.Parameters.Add("@FromAcno",SqlDbType.Char).Value=wrvFromAcno.Text;
       sqlcmd.Parameters.Add("@ToAcno",SqlDbType.Char).Value=wrvToAcctno.Text;
       sqlcmd.Parameters.Add("@CutDate",SqlDbType.Char).Value=myCutDate;
       sqlcmd.Parameters.Add("@PrintChoice",SqlDbType.Char).Value=ddlPrintChoice.SelectedValue.Trim();

		 sqlcmd.Parameters.Add("@InvoiceNoStart", SqlDbType.Char).Value = InvoiceNoStart.Text.Trim();
		 sqlcmd.Parameters.Add("@InvoiceNoEnd", SqlDbType.Char).Value = InvoiceNoEnd.Text.Trim();
		 sqlcmd.Parameters.Add("@VoucherSDate", SqlDbType.Char).Value = myVoucherSdate;
		 sqlcmd.Parameters.Add("@VoucherEDate", SqlDbType.Char).Value = myVoucherEdate;
		 sqlcmd.Parameters.Add("@CustomerName", SqlDbType.Char).Value = TxtCustomerName.Text.Trim();

       DT = _MyDBM.ExecStoredProcedure("dbo.sp_GLR0150_2", sqlcmd.Parameters);
             
     
        // 設值給報表參數
        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = myCompanyName.Trim();
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();
        SDateparam.Name = "CutDate";
        SDateparam.DefaultValue = myCutDate;
        CryReportSource.Report.Parameters.Add(SDateparam);

        CrystalDecisions.Web.Parameter PrintChoiceparam = new CrystalDecisions.Web.Parameter();
        PrintChoiceparam.Name = "PrintChoice";
        PrintChoiceparam.DefaultValue = ddlPrintChoice.SelectedValue;
        CryReportSource.Report.Parameters.Add(PrintChoiceparam);
    



    

        // 決定報表樣式
        if (ddlPrintChoice.SelectedValue == "1"||ddlPrintChoice.SelectedValue=="2")
        {
            strReportname = "GLR0150A.rpt";
            
        }
		  else if (ddlPrintChoice.SelectedValue == "4")
		  {
			  strReportname = "GLR0150D.rpt";

		  }        
        else
        {
            strReportname = "GLR0150C.rpt";
        }

        if (DT.Rows.Count > 0)
        {

            CryReportSource.Report.FileName = strReportname;
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
