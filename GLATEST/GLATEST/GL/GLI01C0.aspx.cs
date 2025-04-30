using System;
using System.Data;
using System.Data.OleDb;
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

public partial class GL_GLI01C0 : System.Web.UI.Page
{
    string sortCol = "OrderID";
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLI01C0";
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
        string strScript = string.Empty;
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/ModPopFunction.js").ToString());   
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtVoucherSDate.CssClass = "JQCalendar";
        // Insure that the __doPostBack() JavaScript method is created
        this.ClientScript.GetPostBackEventReference(this, string.Empty);

        //傳票起始

        if (!IsPostBack)
        {
            CompanyList1.SelectValue = "20";            
            txtAcctNo.Text = "212101";           
            
        }
        else {
            string eventTarget = (this.Request["__EVENTTARGET"] == null) ? string.Empty : this.Request["__EVENTTARGET"];
            string eventArgument = (this.Request["__EVENTARGUMENT"] == null) ? string.Empty : this.Request["__EVENTARGUMENT"];

            if (eventTarget == "SetSessionPostBack")
            {
                Session["VoucherNo"] = eventArgument;
                Session["Company"] = CompanyList1.SelectValue.Trim();
                Session["OPMode"] = "Query";
                Session["FromURL"] = "";
                //MyUtlity.SysUtlity.ClientMsgBox(Session["VoucherNo"].ToString(), this.Page, "SetSessionPostBack");

                string JSString = "window.open(\"GLA0110.aspx\",\"_new\", \"toolbar=no, status=yes, resizable=yes, scrollbars=yes, width=990, height=680\");";
                JsUtility.DoJavascript(JSString, this.Page, "DoJavascript");

            }
            else
            {
                string openbtnID = "";
                foreach (string str in Request.Form)
                {
                    if (str != null)
                    {
                        Control c = Page.FindControl(str.Replace(".x", ""));
                        if (c is ImageButton)
                        {
                            openbtnID = c.ID;
                            break;
                        }
                    }
                }
                if (openbtnID != "imgbtnPosting")
                {
                    PopulateData();
                }
            }
        }
        

        NavigatorPager.BindGridView = GridMaster;
        

        //imgbtnAcctNo加入onclick事件GetAcctnoPopupDialog()
        imgbtnAcctNo.Attributes.Add("onclick", "return GetAcctnoPopupDialog(550, 400, '" + CompanyList1.SelectValue.Trim() + "', 'GLAcctDef', 'CodeCode,CodeName', '" + txtAcctNo.ClientID + "','" + txtAcctName.ClientID + "');");
        imgbtnAcctNo.Attributes.Add("style", "cursor:hand;");

        //imgbtnCompany加入onclick事件GetModelPopupDialog()
       // imgbtnCompany.Attributes.Add("onclick", "return GetModelPopupDialog(280, 300, 'Company', 'Company as Code, CompanyShortName as Name', 'Company', '" + txtCompany.ClientID + "','" + txtCompanyName.ClientID + "');");
       // imgbtnCompany.Attributes.Add("style", "cursor:hand;");
    }
    protected void PopulateData()
    {
        _MyDBM = new DBManger();
        _MyDBM.New();

        string sql = "dbo.sp_GLI01C0";
        DataTable dt = new DataTable();

        SqlCommand sqlcmd = new SqlCommand();   
        string strvoucherDate="";

        //檢查日期並進行轉換
        if (txtVoucherSDate.Text != "")
        {
            strvoucherDate=_UserInfo.SysSet.FormatADDate(txtVoucherSDate.Text.Trim());
            //檢查日期是否正確.若轉出為1912/01/01視為錯誤
            if (strvoucherDate == "1912/01/01")
            {
                JsUtility.ClientMsgBox("日期錯誤!請重新輸入",this.Page,"");
                return;             
            }       
        
        }

        
        try
        {
            //sqlcmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = CompanyList1.SelectValue.Trim();
            //sqlcmd.Parameters.Add("@AcctNo", SqlDbType.Char, 8).Value = txtAcctNo.Text.Trim();
            //sqlcmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = strvoucherDate.Trim().Replace("/", "");
            //sqlcmd.Parameters.Add("@VoucherFilter", SqlDbType.Char, 1).Value = ddlContent.SelectedValue;
            //sqlcmd.Parameters.Add("@Allocate", SqlDbType.Char, 1).Value = ddlAllocation.SelectedValue;
            //dt=_MyDBM.ExecStoredProcedure(sql, sqlcmd.Parameters);

            //GridMaster.DataSource = dt;
            //GridMaster.DataBind();
            //改為用datasource換頁才可正常

            GridDataSource.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            GridDataSource.SelectCommand = sql;
            GridDataSource.SelectParameters.Clear();
            GridDataSource.SelectParameters.Add("Company", CompanyList1.SelectValue.Trim());
            GridDataSource.SelectParameters.Add("AcctNo", txtAcctNo.Text.Trim());
            GridDataSource.SelectParameters.Add("StartDate", strvoucherDate.Trim().Replace("/", ""));
            GridDataSource.SelectParameters.Add("VoucherFilter", ddlContent.SelectedValue);
            GridDataSource.SelectParameters.Add("Allocate", ddlAllocation.SelectedValue);

            GridMaster.DataBind();
            NavigatorPager.BindGridView = GridMaster;
            NavigatorPager.DataBind();

                       
        }
        catch (InvalidCastException e)
        {
            throw (e);
        }
        finally
        {

        }
    }

   
    protected void Datagrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DataGridItem DG = Datagrid1.SelectedItem;
        //Session["VoucherNo"] = DG.Cells[1].Text;
        //Session["Company"] = txtCompany.Text.Trim();
        //Session["OPMode"] = "Query";
        //Session["FromURL"] = "";
        ////Wop("GLA0110.aspx", this.Page, "A1");
        //MyUtlity.SysUtlity.ClientMsgBox(Session["VoucherNo"].ToString(),this.Page,"B1");
    }
   
    protected void imgbtnPosting_Click(object sender, ImageClickEventArgs e)
    {
        if (CompanyList1.SelectValue.Trim() != "" && txtAcctNo.Text.Trim() != "")
        {
            PopulateData();
        }
        else
        {
            JsUtility.ClientMsgBox("請輸入公司代號與會計科目", this.Page, "");      
        
        }
    }
    
    /// <summary>
    /// 轉換日期格式 ConvertStringToDateFormat
    /// </summary>
    /// <param name="tString">yyyyMMdd(傳入的日期字串)</param>
    /// <returns>yyyy/MM/dd(格式化的的日期字串)</returns>
    /// <remarks>yyyy/MM/dd</remarks>
    protected string ConvertStringToDateFormat(string tString)
    {
        if (tString == "" || tString == "&nbsp;")
        {
            return "";
        }
        Int32 iString = Convert.ToInt32(tString);
        if (iString == 0)
        {
            return "";
        }
        else
        {
            return string.Format("{0:0000/00/00}", iString);
        }
    }
    /// <summary>
    /// 轉換日期格式 ConvertDateFormatToString
    /// </summary>
    /// <param name="tString">yyyy/MM/dd(傳入的日期字串)</param>
    /// <returns>yyyy/MM/dd(格式化的的日期字串)</returns>
    /// <remarks>yyyy/MM/dd</remarks>
    protected string ConvertDateFormatToString(string tString)
    {
        if (tString == ""||tString=="&nbsp;")
        {
            return "";
        }
        else
        {
            return tString.Replace("/", "");
        }
    }
    /// <summary>
    /// 轉換金額 ConvertStringToMoney
    /// </summary>
    /// <param name="tString">00000.00(傳入的數字字串)</param>
    /// <returns>###,##0.00(格式化的的數字字串)</returns>
    /// <remarks></remarks>
    protected string ConvertStringToMoney(string tString, bool tType)
    {
        if ((tString == "0.00" || tString == ""||tString=="&nbsp;") && tType)
        {
            return "";
        }
        else
        {
            return string.Format("{0:#,###,###,###,##0.00}", Convert.ToDecimal(tString));
        }
    }
    protected void GridMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {           
            e.Row.Attributes.Add("onmouseover", "oldColor=this.style.backgroundColor;this.style.backgroundColor='silver'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=oldColor");
         

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                //e.Row.Cells[i].Style.Add("text-align", "center");
            }

           e.Row.Cells[0].Text = ConvertStringToDateFormat(e.Row.Cells[0].Text.Trim());
            // 借方/貸方金額=0,則以空白表示
           e.Row.Cells[3].Text = ConvertStringToMoney(e.Row.Cells[3].Text.Trim(),true);                   
           e.Row.Cells[4].Text = ConvertStringToMoney(e.Row.Cells[4].Text.Trim(),true);                   
           e.Row.Cells[5].Text = ConvertStringToMoney(e.Row.Cells[5].Text.Trim(),false);               


            if (e.Row.Cells[1].Text == "" || e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[1].Text = "<<上期餘額>>";
            }
            else
            {
                e.Row.ToolTip = "單擊查詢 傳票號碼：(" + e.Row.Cells[1].Text.Trim() + ") 的資料...";
              
                e.Row.Attributes.Add("onclick", "javascript:wop('" + e.Row.Cells[1].Text.Trim() + "');");
                e.Row.Style["cursor"] = "hand";
            }
                     

        }


    }
}
