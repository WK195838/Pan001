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
using CrystalDecisions.CrystalReports.Engine;



public partial class GLR01H0    : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR01H0";
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
        

        int icalendersYear = DateTime.Now.AddYears(-5).Year - 1911;
        int icalendereYear = DateTime.Now.AddYears(5).Year - 1911;
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;
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
                CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
                CrystalReportViewer1.DataBind();
            }
         
        
        
        }
        string strScript = "return GetPromptTheDate(" + txtDateS.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_SDate.Attributes.Add("onclick", strScript);
        strScript = "return GetPromptTheDate(" + txtDateE.ClientID + "," + icalendersYear.ToString() + "," + icalendereYear.ToString() + ");";
        //ibOW_EDate.Attributes.Add("onclick", strScript);
        ScriptManager1.RegisterPostBackControl(btnQuery);




    }

   

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        Binddata();
    }

    protected void Binddata()
    {
        string strSQL = "";
        DataTable DT;
        string tCompanyNo = DrpCompanyList.SelectValue.Trim();
        string tCompanyName = "";

        _MyDBM = new DBManger();
        _MyDBM.New();

        SqlCommand sqlcmd = new SqlCommand();

        if (txtDateS.Text.Trim() == "")
        {
            JsUtility.ClientMsgBoxAjax("請輸入日期！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "a");    //關閉執行等待畫面
            txtDateS.Focus();
            return;
        }

        //檢查結束日期

        if (txtDateE.Text.Trim() == "")
        {
            JsUtility.ClientMsgBoxAjax("請輸入日期！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "a");    //關閉執行等待畫面
            txtDateE.Focus();
            return;
        }

        string tVoucherSDate = _UserInfo.SysSet.ToADDate(txtDateS.Text.Trim());
        string tVoucherEDate = _UserInfo.SysSet.ToADDate(txtDateE.Text.Trim());

        //因轉錯誤轉不出來為1912/01/01
        if (tVoucherSDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "a");    //關閉執行等待畫面
            txtDateS.Focus();
            return;
        }

        //因轉錯誤轉不出來為1912/01/01
        if (tVoucherEDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("請輸入正確日期或用日曆挑選！", UpdatePanel1, "");
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, "a");    //關閉執行等待畫面
            txtDateE.Focus();
            return;
        }


        tVoucherSDate = GetDate(tVoucherSDate);
        tVoucherEDate = GetDate(tVoucherEDate);
               

        string tType1 = "";     //PostFlag
        string tType2 = "";     //ApvlFlag
        string tAllocate = "";  //AllocateCode


        switch (RadioButtonList1.SelectedValue.ToString())
        {
            case "1":
                tType1 = "Y";
                tType2 = "Y ";
                break;
            case "2":
                tType1 = "YN ";
                tType2 = "Y";
                break;
            case "3":
                tType1 = "YN ";
                tType2 = "Y ";
                break;
        }

        switch (RadioButtonList2.SelectedValue.ToString())
        {
            case "Y":
                tAllocate = "NV ";
                break;
            case "N":
                tAllocate = "N ";
                break;
        }


        strSQL = "Select CompanyName From Company Where Company = '" + tCompanyNo + "'";
        
        DT = _MyDBM.ExecuteDataTable(strSQL);

        if (DT.Rows.Count != 0) tCompanyName = DT.Rows[0]["CompanyName"].ToString();

        DT.Clear();



        strSQL = "dbo.sp_GLR01H0";                       

        sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = tCompanyNo;
        sqlcmd.Parameters.Add("@VoucherSDate", SqlDbType.Char).Value = tVoucherSDate;
        sqlcmd.Parameters.Add("@VoucherEDate", SqlDbType.Char).Value = tVoucherEDate;
        sqlcmd.Parameters.Add("@Type1", SqlDbType.Char).Value = tType1;
        sqlcmd.Parameters.Add("@Type2", SqlDbType.Char).Value = tType2;
        sqlcmd.Parameters.Add("@Allocate", SqlDbType.Char).Value = tAllocate;

        DT = _MyDBM.ExecStoredProcedure(strSQL, sqlcmd.Parameters);
               

     
       
        // 設值給報表參數


        CrystalDecisions.Web.Parameter cparam = new CrystalDecisions.Web.Parameter();
        cparam.Name = "CompanyName";
        cparam.DefaultValue = tCompanyName;
        CryReportSource.Report.Parameters.Add(cparam);

        CrystalDecisions.Web.Parameter SDateparam = new CrystalDecisions.Web.Parameter();
        SDateparam.Name = "StartDate";
        SDateparam.DefaultValue = tVoucherSDate;
        CryReportSource.Report.Parameters.Add(SDateparam);

        CrystalDecisions.Web.Parameter Edateparam = new CrystalDecisions.Web.Parameter();
        Edateparam.Name = "EndDate";
        Edateparam.DefaultValue = tVoucherEDate;
        CryReportSource.Report.Parameters.Add(Edateparam);
        
      

        CryReportSource.ReportDocument.SetDataSource(DT);

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
