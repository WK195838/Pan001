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

public partial class Template_WSingle : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLI01B0";
    DBManger _MyDBM = new DBManger();
    string strHeaderLine, strQueryLine, strDataTitel, strDataBody;
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
        DrpCompany.AutoPostBack = false;
        _MyDBM = new DBManger();
        _MyDBM.New();
        if (_UserInfo.SysSet.isTWCalendar)
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/CodeMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //JQ日曆元件無年份起迄可控管
        //int icalenderBeginYear = _UserInfo.SysSet.YearB;
        //int icalenderEndYear = _UserInfo.SysSet.YearE;
        string strScript = "";
        ScriptManager.RegisterStartupScript(ScriptManager1, this.GetType(), "", @"JQ();", true);
        txtCreateDateS.CssClass = "JQCalendar";
        txtCreateDateE.CssClass = "JQCalendar";
        txtVourDateS.CssClass = "JQCalendar";
        txtVourDateE.CssClass = "JQCalendar";
        TxtrevSDate.CssClass = "JQCalendar";
        TxtrevEDate.CssClass = "JQCalendar";
        if (!IsPostBack)
        {
            DrpCompany.SelectValue = Session["Company"].ToString();
            bindVourSrc();
            //BindGridData();            
        }
        else
        {
            string eventTarget = (this.Request["__EVENTTARGET"] == null) ? string.Empty : this.Request["__EVENTTARGET"];
            string eventArgument = (this.Request["__EVENTARGUMENT"] == null) ? string.Empty : this.Request["__EVENTARGUMENT"];

            if (eventTarget == "SetSessionPostBack")
            {
                Session["VoucherNo"] = eventArgument;
                Session["Company"] = DrpCompany.SelectValue.Trim();
                Session["OPMode"] = "Query";
                Session["FromURL"] = "";   

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
                
            }
        }
        NavigatorPager.BindGridView = GridVouncher;       
    }  
 
  

    private string CheckInputData()
    {
        string  strvourdateS = "";
        string  strvourdateE = "";
        string  strcredateS = "";
        string  strcredateE = "";
        string  strRevDateS = "";
        string  strRevDateE = "";

        if (txtVourDateS.Text == "" && txtVourDateE.Text == ""
            && txtCreateDateS.Text == "" && txtCreateDateE.Text == ""
             && TxtrevSDate.Text == "" && TxtrevEDate.Text == ""
             && txtVourNoS.Text == "" && txtVourNoE.Text == ""
            && txtCreateUser.Text == "")
        {
            return "請至少填入一項查詢條件!";
        }

        //檢查傳票起始日期
        if (txtVourDateS.Text != "")
        {
            strvourdateS=_UserInfo.SysSet.ToADDate(txtVourDateS.Text.Trim());

            if (strvourdateS == "1912/01/01")
            {
                return "傳票起始日期格式錯誤";            
            
            }        
        }
        
        //檢查傳票結束日期
        if (txtVourDateE.Text!="")
        {
            strvourdateE=_UserInfo.SysSet.ToADDate(txtVourDateE.Text.Trim());
            if (strvourdateE=="1912/01/01")
            {
                return "傳票結束日期格式錯誤";

            }
        }       
        //檢查傳票日期大小
        if (strvourdateS!= ""&&strvourdateE!="")
        {
            if (String.Compare(strvourdateS, strvourdateE) > 0)
            {
                return "傳票起始日期不可大於結束日";
            }        
        }


        //檢查製票開始日期
        if (txtCreateDateS.Text != "")
        {
            strcredateS = _UserInfo.SysSet.ToADDate(txtCreateDateS.Text.Trim());
            if (strcredateS == "1912/01/01")
            {
                return "製票開始日期格式錯誤";              
            }
        
        }
        //檢查製票結束日期
        if (txtCreateDateE.Text != "")
        {
            strcredateE = _UserInfo.SysSet.ToADDate(txtCreateDateE.Text.Trim());
            if (strcredateE == "1912/01/01")
            {
                return "製票結束日期格式錯誤";
            
            }        
        }

        //檢查製票日期大小
        if (strcredateS != "" && strcredateE != "")
        {
            if (String.Compare(strcredateS, strcredateE) > 0)
            {
                return "製票起始日期不可大於起始日";
            }
        }




        //檢查迴轉日期
       if (TxtrevSDate.Text != "")
        {
            strRevDateS = _UserInfo.SysSet.ToADDate(TxtrevSDate.Text.Trim());
            if (strRevDateS == "1912/01/01")
            {
                return "迴轉開始日期格式錯誤";
            }

        }
        //檢查迴轉結束日期
        if (TxtrevEDate.Text != "")
        {
            strRevDateE = _UserInfo.SysSet.ToADDate(TxtrevEDate.Text.Trim());
            if (strRevDateE == "1912/01/01")
            {
                return "迴轉結束日期格式錯誤";

            }
        }

        //檢查迴轉日期大小
        if (strRevDateS != "" && strRevDateE != "")
        {
            if (String.Compare(strRevDateS, strRevDateE) > 0)
            {
                return "迴轉起始日期不可大於結束日";
            }
        }




        return "";
    
    }



    public void BindGridData()
    {
        string ErrMessage = CheckInputData();
        
        if (ErrMessage != "")
        {
            JsUtility.ClientMsgBox(ErrMessage, this.Page, "");
            return;
        }
        
        string strExcelSQL = "";
        strExcelSQL = "SELECT Company,VoucherNo,VoucherEntryDate,VoucherDate,VoucherOwner,VoucherSource,ApprovalCode,PostCode,RevDate,DletFlag FROM GLVoucherHead WHERE 1=1";

        string strCompany=DrpCompany.SelectValue.Trim();
        string strVoucherNoS=txtVourNoS.Text.Trim();
        string strVoucherNoE=txtVourNoE.Text.Trim();
        string strVoucherEntryDateS = "";
        string strVoucherEntryDateE = "";
        string strVoucherDateS = "";
        string strVoucherDateE = "";
        string strOwner = txtCreateUser.Text.Trim();
        string strVoucherSrc = txtVourSrc.SelectedValue.Trim();//txtVourSrc.Text.Trim();
        string strRevDateS = "";
        string strRevDateE = "";
        
        if(strCompany!="")
        { 
          strExcelSQL += " AND Company='" + strCompany + "'";
        }

        if (strOwner != "")
        {
            strExcelSQL += " AND VoucherOwner like '%" + strOwner + "%'";            
        }
        
        if (strVoucherSrc != "")
        {
            strExcelSQL += " AND VoucherSource like '%" + strVoucherSrc + "%'";            
        }

        if (strVoucherNoS != "" && strVoucherNoE != "")
        {
            strExcelSQL += " AND (VoucherNo Between " + strVoucherNoS + " AND " + strVoucherNoE + ")";
        }
        else if (strVoucherNoS != "")
        {
            strExcelSQL += " AND VoucherNo >= " + strVoucherNoS + "";
        }
        else if (strVoucherNoE != "")
        {
            strExcelSQL += " AND VoucherNo <= " + strVoucherNoE + "";
        }
        #region 日期條件
        //製票開始日期
       strVoucherEntryDateS= _UserInfo.SysSet.ToADDate(txtCreateDateS.Text.Trim()).Replace("/","");
       if (strVoucherEntryDateS == "19120101")
       {       
           strVoucherEntryDateS = string.Empty;       
       }

       //製票結束日
       strVoucherEntryDateE = _UserInfo.SysSet.ToADDate(txtCreateDateE.Text.Trim()).Replace("/", "");
       if (strVoucherEntryDateE == "19120101")
       {   
           strVoucherEntryDateE = string.Empty;
       }
       
        //傳票開始日期
       strVoucherDateS = _UserInfo.SysSet.ToADDate(txtVourDateS.Text.Trim()).Replace("/", "");
       if (strVoucherDateS == "19120101")
       {     
           strVoucherDateS = string.Empty;
       }

       //傳票結束日期
       strVoucherDateE = _UserInfo.SysSet.ToADDate(txtVourDateE.Text.Trim()).Replace("/", "");
       if (strVoucherDateE == "19120101")
       {    
           strVoucherDateE = string.Empty;
       }


        //傳票迴轉日期
       strRevDateS = _UserInfo.SysSet.ToADDate(TxtrevSDate.Text.Trim()).Replace("/", "");
       if (strRevDateS == "19120101")
       {      
           strRevDateS = string.Empty;
       }

       strRevDateE = _UserInfo.SysSet.ToADDate(TxtrevEDate.Text.Trim()).Replace("/", "");
       if (strRevDateE == "19120101")
       {        
           strRevDateE = string.Empty;
       }

       
       if (strVoucherEntryDateS != "" && strVoucherEntryDateE != "")
           strExcelSQL += " AND (VoucherEntryDate Between " + strVoucherEntryDateS + " AND " + strVoucherEntryDateE + ")";
       else if (strVoucherEntryDateS != "")
           strExcelSQL += " AND VoucherEntryDate >= " + strVoucherEntryDateS + "";
       else if (strVoucherEntryDateE != "")
           strExcelSQL += " AND VoucherEntryDate <= " + strVoucherEntryDateE + "";

       if (strVoucherDateS != "" && strVoucherDateE != "")
           strExcelSQL += " AND (VoucherDate Between " + strVoucherDateS + " AND " + strVoucherDateE + ")";
       else if (strVoucherDateS != "")
           strExcelSQL += " AND VoucherDate >= " + strVoucherDateS + "";
       else if (strVoucherDateE != "")
           strExcelSQL += " AND VoucherDate <= " + strVoucherDateE + "";
        
       if (strRevDateS != "" && strRevDateE != "")
           strExcelSQL += " AND (RevDate Between " + strRevDateS + " AND " + strRevDateE + ")";
       else if (strRevDateS != "")
           strExcelSQL += " AND RevDate >= " + strRevDateS + "";
       else if (strRevDateE != "")
           strExcelSQL += " AND RevDate <= " + strRevDateE + "";
        #endregion
       strExcelSQL += " order by VoucherNo ";

       ViewState["ExcelCmd"] = strExcelSQL.Replace('\n',' ').Replace('\r',' ');

       DataTable dt = _MyDBM.ExecuteDataTable(strExcelSQL);
       
       GridVouncher.DataSource = dt;
       GridVouncher.DataBind();

       NavigatorPager.BindGridView = GridVouncher;
       NavigatorPager.DataBind();
        
        //匯出excel
        if (ViewState["ExcelCmd"] != null)
        {
           
            strHeaderLine = "'傳票查詢作業'";
            strQueryLine = "'公司別：," + DrpCompany.SelectValue + ",傳票號碼：," + txtVourNoS.Text + "~" + txtVourNoE.Text + "'";
            strDataTitel = "'公司別,傳票號碼,製票日期,傳票日期,製票人,傳票來源,主管核對作業,過帳註記,迴轉日期,作廢註記'";
            strDataBody = "'" + _UserInfo.SysSet.rtnHash(ViewState["ExcelCmd"].ToString()).Replace('\\', '＼').Replace('+', '＋') + "'";
            ExportExcel.Attributes.Add("onclick", "javascript:ExportExcel(" + strHeaderLine + "," + strQueryLine + "," + strDataTitel + "," + strDataBody + ");return false;");
            ExportExcel.Visible = true;
        }
    }
    
    /// <summary>
    /// 取得傳票來源
    /// </summary>
    private void bindVourSrc()
    {
        string strSQL = "select CodeID,CodeCode,CodeName,Maint from CodeDesc where CodeID='AH17'";

        DataTable dt = _MyDBM.ExecuteDataTable(strSQL);

        txtVourSrc.DataSource = dt;
        txtVourSrc.DataValueField = "CodeCode";
        txtVourSrc.DataTextField = "CodeName";
        txtVourSrc.DataBind();
        ListItem li = new ListItem();
        li.Text = "全部";
        li.Value = "";
        txtVourSrc.Items.Insert(0, li);

    } 
   
    protected void btnQuery_Click(object sender, ImageClickEventArgs e)
    {
        BindGridData();
    }


    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        txtCreateDateE.Text = "";
        txtCreateDateS.Text = "";
        txtCreateUser.Text = "";
        TxtrevEDate.Text = "";
        TxtrevSDate.Text = "";
        txtVourDateE.Text = "";
        txtVourDateS.Text = "";
        txtVourNoE.Text = "";
        txtVourNoS.Text = "";
        txtVourSrc.Text = "";

    }

   protected void GridVouncher_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Style["cursor"] = "hand";
            e.Row.Attributes.Add("onmouseover", "oldColor=this.style.backgroundColor;this.style.backgroundColor='silver'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=oldColor");

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "center");
            }

            e.Row.ToolTip = "單擊查詢 傳票號碼：(" + e.Row.Cells[1].Text.Trim() + ") 的資料...";

            e.Row.Attributes.Add("onclick", "javascript:wop('" + e.Row.Cells[1].Text.Trim() + "');");
          
        
        }



    }
  
}
