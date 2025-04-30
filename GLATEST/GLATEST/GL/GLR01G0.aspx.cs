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



public partial class GLR01G0    : System.Web.UI.Page
{

    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR01G0";
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
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtDateS.CssClass = "JQCalendar";
        txtDateE.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            DrpCompanyList.SelectValue = Session["Company"].ToString();
        }
        else
        {


            if (ViewState["Sourcedata"] != null)
            {
                //設定縮放功能
                //找出控制項的值

               // int intPageZoom = (((DropDownList)this.CrystalReportViewer1.Controls[2].Controls[15]).SelectedIndex + 1) * 25;
               // CrystalReportViewer1.Zoom(intPageZoom);
                cryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
        
        }

        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

  

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        bindData();       
    }

    protected void bindData()
    {        

        _MyDBM = new DBManger();
        _MyDBM.New();

        string strSQL = "";
        SqlCommand sqlcmd = new SqlCommand();       
        DataTable DT;

        string tCompanyNo = DrpCompanyList.SelectValue.Trim();
        string tCompanyName ="";


        strSQL = "Select CompanyName From Company Where Company ='" + tCompanyNo + "'";
        DT = _MyDBM.ExecuteDataTable(strSQL);
        if (DT.Rows.Count != 0) tCompanyName = DT.Rows[0]["CompanyName"].ToString();


        //檢查起始日期

        if(txtDateS.Text.Trim()=="")
        {
         JsUtility.ClientMsgBoxAjax("請輸入日期！", UpdatePanel1, "");
            txtDateS.Focus();
            return;
        }

        //檢查結束日期

        if(txtDateE.Text.Trim()=="")
        {
         JsUtility.ClientMsgBoxAjax("請輸入日期！", UpdatePanel1, "");
            txtDateE.Focus();
            return;
        }

        
        string tVoucherSDate = _UserInfo.SysSet.ToADDate(txtDateS.Text.Trim());
        string tVoucherEDate = _UserInfo.SysSet.ToADDate(txtDateE.Text.Trim());

         //因轉錯誤轉不出來為1912/01/01
        if (tVoucherSDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            txtDateS.Focus();
            return;
        }

         //因轉錯誤轉不出來為1912/01/01
        if (tVoucherEDate == "1912/01/01")
        {
           JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
           txtDateE.Focus();
            return;
        }


        tVoucherSDate=GetDate(tVoucherSDate);
        tVoucherEDate=GetDate(tVoucherEDate);

        strSQL="dbo.sp_GLR1G0";

        sqlcmd.Parameters.Add("@Company",SqlDbType.Char).Value=tCompanyNo;
        sqlcmd.Parameters.Add("@startDate",SqlDbType.Char).Value=tVoucherSDate;
        sqlcmd.Parameters.Add("@endDate",SqlDbType.Char).Value=tVoucherEDate;
        sqlcmd.Parameters.Add("@action",SqlDbType.Int).Value=Convert.ToInt16(RadioButtonList1.SelectedValue);

        DT = _MyDBM.ExecStoredProcedure(strSQL, sqlcmd.Parameters);
      
      
        // 設值給報表參數

        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = tCompanyName;
        cryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();
        SDateparam.Name = "StartDate";
        SDateparam.DefaultValue = tVoucherSDate;
        cryReportSource.Report.Parameters.Add(SDateparam);

        CrystalDecisions.Web.Parameter Edateparam = new CrystalDecisions.Web.Parameter();
        Edateparam.Name = "EndDate";
        Edateparam.DefaultValue = tVoucherEDate;
        cryReportSource.Report.Parameters.Add(Edateparam);


        cryReportSource.ReportDocument.SetDataSource(DT);

        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
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

    protected string GetDate(string selectVal)
    {
        string tVal = "";

        if (selectVal.Trim() == "")
            tVal = DateTime.Now.ToString("yyyyMMdd");
        else
            tVal = DateTime.Parse(selectVal).ToString("yyyyMMdd");
        return tVal;
    }

  

    
}
