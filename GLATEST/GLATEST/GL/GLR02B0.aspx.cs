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

public partial class GLR02B0 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02B0";
    DBManger _MyDBM;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        CrystalReportViewer1.DisplayGroupTree = false;
        CrystalReportViewer1.HasToggleGroupTreeButton = false;
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtBaseDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            //載入公司資料
            string strSQL = "SELECT Company, Company + '-' + CompanyShortName AS CompanyName FROM Company";

            DataTable dtComp = new DataTable();

            dtComp = _MyDBM.ExecuteDataTable(strSQL);
            dtComp.Columns[0].ColumnName = "CompanyNo";
            dtComp.Columns[1].ColumnName = "CompanyName";

            if (dtComp.Columns.Count != 0)
            {
                FillDropDownList(DrpCompany, "CompanyNo", "CompanyName", dtComp);
            }
            DrpCompany.SelectedValue = "20";

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

        btnQuery.Attributes.Add("onClick", "drawWait('')");
        ScriptManager1.RegisterPostBackControl(btnQuery);

    }

    

    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindData();

    }

    /// <summary>
    /// 處理報表資料
    /// </summary>
    protected void BindData()
    {
        //-------------------------------------------------------------------------------
        // 畫面查詢條件檢查
        //-------------------------------------------------------------------------------

        string strSQL;
        DataTable DT;

        int intsubidx = 6;

        // Check 日期

        if (txtBaseDate.Text == "")
        {
            JsUtility.ClientMsgBoxAjax("日期不可空白！", UpdatePanel1, "");
            txtBaseDate.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, ""); 
            return;
        }
        
        string myBaseDate = "";
        string myPreyearDate = "";
        myBaseDate = _UserInfo.SysSet.ToADDate(txtBaseDate.Text.Trim());
        DateTime basedate = _UserInfo.SysSet.GetADDate(txtBaseDate.Text.Trim());

        

        if (myBaseDate == "1912/01/01")
        {
            JsUtility.ClientMsgBoxAjax("請輸入正確數字！", UpdatePanel1, "");
            txtBaseDate.Focus();
            JsUtility.CloseWaitScreenAjax(UpdatePanel1, ""); 
            return;
        
        }
        myBaseDate = myBaseDate.Replace("/", "");

        myPreyearDate = basedate.AddYears(-1).Year.ToString() + "1231";
                 

        //-------------------------------------------------------------------------------
        // 取出報表參數相關資訊並設值給報表參數
        //-------------------------------------------------------------------------------
        // 取出公司全名
        string myCompanyName = "";
        strSQL = "Select CompanyName From Company Where Company = '"+DrpCompany.SelectedValue+"'" ;
        DT = _MyDBM.ExecuteDataTable(strSQL);      
        if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
              
        //執行計算試算表資料之Stored Procedure 
        DT.Clear();

        strSQL = "PrGLA02B0_F";

        SqlCommand sqlcmd = new SqlCommand();

        sqlcmd.Parameters.Add("@company",SqlDbType.Char).Value=DrpCompany.SelectedValue;
        sqlcmd.Parameters.Add("@subidx", SqlDbType.Int).Value = intsubidx;
        sqlcmd.Parameters.Add("@BaseDate", SqlDbType.Char).Value = myBaseDate;
        sqlcmd.Parameters.Add("@PreYearDate", SqlDbType.Char).Value = myPreyearDate;


        DT = _MyDBM.ExecStoredProcedure(strSQL, sqlcmd.Parameters);

        // 設值給報表參數
        CryReportSource.Report.Parameters.Add(NewParameter("CompanyName", myCompanyName));

        CryReportSource.Report.Parameters.Add(NewParameter("BaseDate", myBaseDate));

        CryReportSource.ReportDocument.SetDataSource(DT);
        CrystalReportViewer1.DataBind();
        CrystalReportViewer1.Visible = true;
        ViewState["Sourcedata"] = DT;
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, ""); 
    }

    private CrystalDecisions.Web.Parameter NewParameter(string Name, string Value)
    {
        CrystalDecisions.Web.Parameter NewParameter = new CrystalDecisions.Web.Parameter();
        NewParameter.Name = Name;
        NewParameter.DefaultValue = Value;
        return NewParameter;
    }

    protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
    {
        DDL.DataSource = dt;
        DDL.DataTextField = SetText;
        DDL.DataValueField = SetValue;
        DDL.DataBind();
    }

   
   
}
