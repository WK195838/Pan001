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
using System.Text;

public partial class AdjustSalaryMaster_M : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "", Ssql2 = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM004";
    DBManger _MyDBM;
    int saveon = 0;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCompany, sEmployeeId, sEffectiveDate;
    int iSPValue = 1;
    Payroll py = new Payroll();

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }
    private void AuthRight(GridView thisGridView)
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    thisGridView.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        thisGridView.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    thisGridView.ShowFooter = Find;
                }
            }

            //因為是附加在頁籤的功能,所以一定可以查詢
            ////查詢(執行)
            //if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            //{
            //    Find = true;
            //}
            //else
            //{
            //    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            //}

            //版面樣式調整
            if (SetCss == false)
            {
                thisGridView.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);

        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString()); 

        lbl_Msg.Text = "";
        lbl_Msg2.Text = "";
        StyleTitle1.ShowBackToPre = false;
        StyleTitle2.ShowBackToPre = false;

        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Add");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        //判斷新增或修改模式,預設為[false]=[修改]
        bool blInsertMod = false;
        //判斷是否唯讀模式
        bool blReadOnly = false;
        
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
            sEmployeeId = DetailsView1.DataKey[1].ToString().Trim();
            sEffectiveDate = DetailsView1.DataKey[2].ToString().Trim();
            sEffectiveDate = Convert.ToDateTime(sEffectiveDate).ToString("yyyy/MM/dd");
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");            
        }
        catch
        {
            if (Request.Form["Detailsview1$DDL01$ddlCodeList"] != null)
                sCompany = Request["Detailsview1$DDL01$ddlCodeList"].Trim();
            else if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
            else
                sCompany = "";

            if (Request.Form["Detailsview1$DDL02$ddlCodeList"] != null)
                sEmployeeId = Request["Detailsview1$DDL02$ddlCodeList"].Trim();
            else if (Request["EmployeeId"] != null)
                sEmployeeId = Request["EmployeeId"].Trim();
            else
                sEmployeeId = "";

            if (Request.Form["DetailsView1$ctl03"] != null)
                sEffectiveDate = Request["DetailsView1$ctl03"].Trim();
            else if (Request["EffectiveDate"] != null)
                sEffectiveDate = Request["EffectiveDate"].Trim();
            else
                sEffectiveDate = "";

            if (sCompany.Length * sEmployeeId.Length == 0)
            {
                blInsertMod = true;
            }
            else
            {
                if (Request["Kind"] == null || sEffectiveDate == "")
                {
                    blInsertMod = true;
                }

                if (sEffectiveDate != "")
                {
                    if ((sEffectiveDate.IndexOf('/') > 0 && Convert.ToInt32(sEffectiveDate.Remove(sEffectiveDate.IndexOf('/'))) < 1900)
                        || (sEffectiveDate.IndexOf('-') > 0 && Convert.ToInt32(sEffectiveDate.Remove(sEffectiveDate.IndexOf('-'))) < 1900))
                        sEffectiveDate = _UserInfo.SysSet.FormatADDate(sEffectiveDate);
                    
                    Ssql = string.Format(" Where Company = '{0}'", _UserInfo.SysSet.CleanSQL(sCompany.Trim()));
                    Ssql += string.Format(" And EmployeeId = '{0}'", _UserInfo.SysSet.CleanSQL(sEmployeeId.Trim()));

                    Ssql = "Select (Select count(*) from [Personnel_Master] " + Ssql +
                        ") PMCount, (Select count(*) from [AdjustSalary_Master] " + Ssql +
                        string.Format(" And EffectiveDate = '{0}'", _UserInfo.SysSet.CleanSQL(sEffectiveDate.Trim())) + ") PMDCount";

                    DataTable TB = _MyDBM.ExecuteDataTable(Ssql);

                    if (TB.Rows[0][0].ToString().Equals("0"))
                    {
                        blInsertMod = true;
                        sEmployeeId = "";
                    }
                    else if (TB.Rows[0][1].ToString().Equals("0"))
                    {
                        blInsertMod = true;
                    }
                    else
                    {
                        blInsertMod = false;
                    }
                }
            }

            if ((blInsertMod == false) && (Request.Form["Detailsview1$DDL02$ddlCodeList"] != null))
            {
                int iRemove = Request.RawUrl.IndexOf(".aspx?");
                if (iRemove > 0)
                    Response.Redirect(Request.RawUrl.Remove(Request.RawUrl.IndexOf(".aspx?") + 6) + "&Kind=M&Company=" + sCompany + "&EmployeeId=" + sEmployeeId + "&EffectiveDate=" + sEffectiveDate);
                else
                    Response.Redirect(Request.RawUrl + "?Kind=M&Company=" + sCompany + "&EmployeeId=" + sEmployeeId + "&EffectiveDate=" + sEffectiveDate);
            }
        }

        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
        if (blInsertMod == false)
        {
            btnSaveGo.CommandName = "Update";
            btnSaveExit.Visible = false;
            //2011/07/08 調薪功能暫不可修改上半
            btnSaveGo.Visible = false;
        }

        if (Request["Kind"] != null)
        {
            blReadOnly = Request["Kind"].Equals("Query");
        }

        if (DetailsView1.DefaultMode != (blInsertMod ? DetailsViewMode.Insert : DetailsViewMode.Edit))
            DetailsView1.DefaultMode = (blInsertMod ? DetailsViewMode.Insert : DetailsViewMode.Edit);
                
        if (!Page.IsPostBack)
        {
            hid_IsInsertExit.Value = "";
            saveon = 0;
            BindData();
        }
        else
        {
            if (!string.IsNullOrEmpty(hid_updateid.Value))
            {
                //轉換欄位標記 - > $
                hid_IsInsertExit.Value = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[hid_IsInsertExit.Value + "$ctl03"] != null)
                {
                    GridView1RowUpdate();
                }
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
                if (Request.Form[ddlId + "1$ddlCodeList"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
                else
                {
                    BindData();
                }
            }
            else if (Request.Form[Request.Form[0].Substring(Request.Form[0].IndexOf('|') + 1)] != null &&
                Request.Form[0].Substring(Request.Form[0].IndexOf('|') + 1).Contains("tbAddNew01"))
            {//變更薪資項目
                if (!Request.Form[0].Substring(Request.Form[0].IndexOf('|') + 1).Contains("ctl01$tbAddNew01"))
                    BindData();
                object theSender = this.FindControl(Request.Form[0].Substring(Request.Form[0].IndexOf('|') + 1));
                SelectedIndexChanged1(theSender, e);                
            }
            else if (!string.IsNullOrEmpty(Request.Form["__EVENTARGUMENT"]) || 
                Request.Form[0].Contains("UpdatePanel1|Navigator1$ddl_PageSize")) //if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
            {
                BindData();
            }
        }
        Navigator1.BindGridView = GridView1;
        
        //hid_IsInsertExit.Value = "";
        #region 查詢顯示控管
        if (blReadOnly)
        {
            DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            btnSaveExit.Visible = false;
            btnSaveGo.Visible = false;

            GridView theGv = (GridView)this.Form.FindControl("GridView1");
            if (theGv != null)
            {
                theGv.ShowFooter = false;
                theGv.Columns[0].Visible = false;
                theGv.Columns[1].Visible = false;
            }
        }
        #endregion
    }

    protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //For FW4.0改用DropDownList
        //sCompany = ((ASP.usercontrol_codelist_ascx)sender).SelectedCode.Trim();
        sCompany = ((DropDownList)sender).SelectedValue.Trim();
        ASP.usercontrol_codelist_ascx ddl = (ASP.usercontrol_codelist_ascx)DetailsView1.Rows[1].Cells[1].Controls[1];
        //排除已離職員工        
        Ssql = " And Upper(IsNull(ResignCode,'')) != 'Y' ";
        ddl.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", "Company='" + sCompany + "'" + Ssql, 5);
        ddl.SelectedCode = "";
        sEmployeeId = "";
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        if ((DetailsView1.DefaultMode == DetailsViewMode.Insert) || (Page.IsPostBack && saveon == 0))
        {
            for (int i = 0; i < DetailsView1.Rows.Count; i++)
            {//修改欄位顯示名稱
                Ssql = "Select dbo.GetColumnTitle('AdjustSalary_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                if (DT.Columns.Count > 0)
                {
                    if (DetailsView1.DefaultMode == DetailsViewMode.Insert)
                    {
                        #region
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("Company"))
                        {
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            tb.MaxLength = 2;
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");
                            //根據選擇的公司 帶入公司欄位
                            ASP.usercontrol_codelist_ascx ddl = new ASP.usercontrol_codelist_ascx();
                            ddl = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                            ddl.ID = "DDL" + (i + 1).ToString("D2");
                            ddl.SetDTList("Company", "Company", "CompanyName", "", 5);//Company='" + sCompany + "'
                            ddl.SelectedCode = sCompany;

                            ddl.StyleAdd("width", "150px");
                            ddl.AutoPostBack = true;
                            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

                            DetailsView1.Rows[i].Cells[1].Controls.Add(ddl);
                        }
                        else if (DetailsView1.Rows[i].Cells[0].Text.Contains("EmployeeId"))
                        {//欄位輸入欄位長度限制
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            tb.MaxLength = 5;
                            tb.Visible = false;
                            tb.Style.Add("width", "1px");

                            if (Request.Form["Detailsview1$DDL01$ddlCodeList"] != null)
                                sCompany = Request.Form["Detailsview1$DDL01$ddlCodeList"].Trim();
                            Ssql = (string.IsNullOrEmpty(sCompany) ? "" : "Company='" + sCompany + "'");
                            //排除已離職員工
                            Ssql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";

                            //根據選擇的公司 選擇可以新增的員工編號                            
                            ASP.usercontrol_codelist_ascx ddl2 = new ASP.usercontrol_codelist_ascx();
                            ddl2 = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                            ddl2.ID = "DDL" + (i + 1).ToString("D2");
                            ddl2.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);
                            ddl2.StyleAdd("width", "150px");
                            ddl2.AutoPostBack = true;
                            ddl2.SelectedCode = sEmployeeId;
                            if (Request["Kind"] != null)
                            {
                                if (Request["Kind"].ToString().Equals("M") && ddl2.SelectedCode.Equals(sEmployeeId))
                                {
                                    ASP.usercontrol_codelist_ascx ddl1 = ((ASP.usercontrol_codelist_ascx)DetailsView1.Rows[i - 1].Cells[1].FindControl("DDL" + (i).ToString("D2")));
                                    ddl1.Enabled = false;
                                    ddl2.Enabled = false;
                                }
                            }

                            DetailsView1.Rows[i].Cells[1].Controls.Add(ddl2);
                        }
                        else if (DetailsView1.Rows[i].Cells[0].Text.Contains("EffectiveDate"))
                        {
                            TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                            tb.Text = _UserInfo.SysSet.FormatDate(DateTime.Now.ToString("yyyy/MM/") + "01");
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        //if (DetailsView1.Rows[i].Cells[0].Text.Contains("Company"))
                        //{
                        //    //根據選擇的公司 帶入公司欄位
                        //    ASP.usercontrol_codelist_ascx ddl = new ASP.usercontrol_codelist_ascx();
                        //    ddl = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                        //    ddl.ID = "DDL" + (i + 1).ToString("D2");
                        //    ddl.SetDTList("Company", "Company", "CompanyName", "", 5);//Company='" + sCompany + "'
                        //    ddl.SelectedCode = sCompany;

                        //    ddl.StyleAdd("width", "150px");
                        //    ddl.Enabled = false;

                        //    DetailsView1.Rows[i].Cells[1].Controls.Add(ddl);
                        //}
                        //else if (DetailsView1.Rows[i].Cells[0].Text.Contains("EmployeeId"))
                        //{//欄位輸入欄位長度限制
                        //    if (Request.Form["Detailsview1$DDL01$ddlCodeList"] != null)
                        //        sCompany = Request.Form["Detailsview1$DDL01$ddlCodeList"].Trim();
                        //    Ssql = (string.IsNullOrEmpty(sCompany) ? "" : "Company='" + sCompany + "'");

                        //    //根據選擇的公司 選擇可以新增的員工編號                            
                        //    ASP.usercontrol_codelist_ascx ddl2 = new ASP.usercontrol_codelist_ascx();
                        //    ddl2 = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                        //    ddl2.ID = "DDL" + (i + 1).ToString("D2");
                        //    ddl2.SetDTList("Personnel_Master", "EmployeeId", "EmployeeName", Ssql, 5);
                        //    ddl2.SelectedCode = sEmployeeId;

                        //    ddl2.StyleAdd("width", "150px");
                        //    ddl2.Enabled = false;

                        //    DetailsView1.Rows[i].Cells[1].Controls.Add(ddl2);
                        //}
                        #endregion
                    }

                    if (DetailsView1.DefaultMode != DetailsViewMode.ReadOnly)
                    {
                        if (DetailsView1.Rows[i].Cells[0].Text.Contains("AdjustSalaryReason"))
                        {
                            //欄位輸入欄位長度限制
                            ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).MaxLength = 14;
                        }
                        else if (DetailsView1.Rows[i].Cells[0].Text.ToLower().Contains("date"))
                        {//日期欄位靠右對齊
                            if (DetailsView1.Rows[i].Cells[1].Controls.Count > 0)
                            {
                                TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                                tb.Style.Add("text-align", "right");
                                tb.MaxLength = 8;
                                //為日期欄位增加小日曆元件                            
                                tb.CssClass = "JQCalendar";
                            }
                        }
                        //else if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepositBank"))
                        //{//欄位輸入欄位長度限制
                        //    TextBox tb = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]);
                        //    tb.MaxLength = 3;
                        //    ImageButton btOpenList = new ImageButton();
                        //    btOpenList.ID = "btOpen" + i.ToString();
                        //    btOpenList.SkinID = "OpenWin1";
                        //    btOpenList.OnClientClick = "return GetPromptWin1(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','Bank_Master','BankHeadOffice','BankHeadOffice As 銀行總行代號,BankAbbreviations','BankHeadOffice');";
                        //    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenList);
                        //}
                    }
                    //設定欄位名稱
                    DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                }
            }
        }

        if ((DetailsView1.DefaultMode == DetailsViewMode.Insert) && hid_InserMode.Value.Trim().Equals("GO"))
        {
            SDS_GridView.SelectCommand = "select * from AdjustSalary_Master where 1=0";
            GridView1.DataBind();
        }
    }
    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        string strDate = "";
        if (DetailsView1.DefaultMode != DetailsViewMode.Insert)
        {
            for (int i = 0; i < DetailsView1.Rows.Count; i++)
            {//修改欄位顯示名稱
                Ssql = "Select dbo.GetColumnTitle('AdjustSalary_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                if (DT.Columns.Count > 0)
                {
                    if (DetailsView1.Rows[i].Cells[0].Text.ToLower().Contains("date"))
                    {//欄位輸入欄位長度限制

                        if (DetailsView1.Rows[i].Cells[1].Controls.Count > 0)
                        {
                            TextBox tb = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]);
                            if (tb != null)
                            {
                                tb.MaxLength = 10;

                                //日期欄位靠右對齊及格式限制
                                tb.Style.Add("text-align", "right");
                                strDate = tb.Text;
                                try
                                {
                                    if (strDate.Contains("1900-01-01") || strDate.Contains("1912-01-01"))
                                        strDate = "";
                                    else
                                        strDate = _UserInfo.SysSet.FormatDate(strDate);
                                }
                                catch { strDate = ""; }
                                tb.Text = strDate;

                                if (DetailsView1.Rows[i].Cells[1].Controls.Count < 2)
                                {//表示沒有小日期按鈕才加
                                    //為日期欄位增加小日曆元件                                    
                                    tb.CssClass = "JQCalendar";
                                }
                            }
                        }
                        else
                        {
                            strDate = DetailsView1.Rows[i].Cells[1].Text;
                            try
                            {
                                if (strDate.Contains("1900-01-01") || strDate.Contains("1912-01-01"))
                                    strDate = "";
                                else
                                    strDate = _UserInfo.SysSet.FormatDate(strDate);
                            }
                            catch { strDate = ""; }
                            DetailsView1.Rows[i].Cells[1].Text = strDate;
                        }
                    }

                    DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

                    if (DT.Rows[0][0].ToString().Trim().Contains("員工"))
                    {
                        if (DBSetting.PersonalName(sCompany, sEmployeeId) != null)
                        {
                            DetailsView1.Rows[1].Cells[1].Text = DetailsView1.Rows[1].Cells[1].Text.Trim() + " - " + DBSetting.PersonalName(sCompany, sEmployeeId).ToString();
                        }
                    }
                    else if (DT.Rows[0][0].ToString().Trim().Contains("公司"))
                    {
                        DetailsView1.Rows[0].Cells[1].Text = DetailsView1.Rows[0].Cells[1].Text.Trim() + " - " + DBSetting.CompanyName(sCompany).ToString();
                    }
                }
            }
        }
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
        ShowMsgBox1.Message = "";

        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {

            if (e.Values[i] == null)
            {
                e.Values[i] = " ";
            }

            if (i < 2)
            {//代碼下拉單
                ASP.usercontrol_codelist_ascx ddl = (ASP.usercontrol_codelist_ascx)dv.Rows[i].Cells[1].Controls[1];
                if (i == 0)
                {
                    e.Values["Company"] = ddl.SelectedCode.Trim();
                }
                else if (i == 1)
                {
                    e.Values["EmployeeId"] = ddl.SelectedCode.Trim();
                }
            }
            //日期欄位
            if (dv.Rows[i].Cells[0].Text.Contains("日"))
            {
                e.Values[i] = _UserInfo.SysSet.FormatADDate(e.Values[i].ToString());
            }

            try
            {//準備寫入LOG用之參數                                
                InsertItem += DetailsView1.Rows[i].Cells[0].Text.Trim() + "|";
                InsertValue += e.Values[i].ToString() + "|";
            }
            catch
            { }
        }
        if (Err.Equals(""))
        {
            if (!ValidateData2(e.Values["Company"].ToString(), e.Values["EmployeeId"].ToString(), e.Values["EffectiveDate"].ToString()))
            {
                Err += "資料重覆!!此公司下已有此筆資料!";
            }
        }
        //調過後的薪水如果沒輸入 就是原本薪水
        //if (string.IsNullOrEmpty(e.Values["NewSalary"].ToString().Trim()))
        //{
        //    e.Values["NewSalary"] = e.Values["OldlSalary"];
        //}
        ////撫養人數
        //if (string.IsNullOrEmpty(e.Values["DependentsNum"].ToString().Trim()))
        //{
        //    e.Values["DependentsNum"] = "0";
        //}
        if (Err != "")
        {//檢核失敗時,要顯示訊息
            ShowMsgBox1.Message = Err;
            lbl_Msg.Text = Err;
            e.Cancel = true;
        }
        else
        {//檢核成功後要做的處理
            #region 開始異動前,先寫入LOG
            //TableName	異動資料表	varchar	60
            //TrxType	異動類別(A/U/D)	char	1
            //ChangItem	異動項目(欄位)	varchar	255
            //SQLcommand	異動項目(異動值/指令)	varchar	2000
            //ChgStartDateTime	異動開始時間	smalldatetime	
            //ChgStopDateTime	異動結束時間	smalldatetime	
            //ChgUser	使用者代號	nvarchar	32
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Payroll_Master_Heading";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (InsertItem.Length * 2 > 255) ? "ALL" : InsertItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = InsertValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }

        //hid_Company.Value = txt_Company.Text;   
    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        StringBuilder str;

        if (e.Exception == null)
        {
            lbl_Msg.Text = "新增成功!!";
        }
        else
        {
            lbl_Msg.Text = "新增失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();

        if (e.Exception != null)
        {
            return;
        }

        if (hid_InserMode.Value.Trim().Equals("EXIT"))
        {
            ASP.usercontrol_codelist_ascx ddl = (ASP.usercontrol_codelist_ascx)DetailsView1.Rows[0].Cells[1].Controls[1];
            string Com = ddl.SelectedCode.Trim();
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='AdjustSalaryMaster.aspx?Company=" + Com + "';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }

    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        DetailsView dv = ((DetailsView)sender);
        //因為前2個是ReadOnly所以要扣掉
        //最後一個是按鈕列也要扣掉:no use
        for (int i = 0; i < dv.Rows.Count - 2; i++)
        {
            try
            {
                tempOldValue = "";
                if (e.OldValues.Count > i && e.OldValues[i] != null)
                    tempOldValue = e.OldValues[i].ToString().Trim();

                if (e.NewValues[i] != null)
                    e.NewValues[i] = e.NewValues[i].ToString().Trim();
                else
                    e.NewValues[i] = "";

                if (dv.Rows[i + 2].Cells[0].Text.Trim().Contains("日"))
                {//將日期欄位格式為化為西元日期    
                    if (e.NewValues[i] != "")
                        e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                    if (tempOldValue != "")
                        tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                }

                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }

                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += dv.Rows[i + 2].Cells[0].Text.Trim() + "|";
                        UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
                    }
                    catch
                    { }
                }
            }
            catch { }
        }

        if (Err != "")
        {//檢核失敗時,要顯示訊息
            ShowMsgBox1.Message = Err;
            lbl_Msg.Text = Err;
            e.Cancel = true;
        }
        else
        {//檢核成功後要做的處理
            #region 開始異動前,先寫入LOG
            //TableName	異動資料表	varchar	60
            //TrxType	異動類別(A/U/D)	char	1
            //ChangItem	異動項目(欄位)	varchar	255
            //SQLcommand	異動項目(異動值/指令)	varchar	2000
            //ChgStartDateTime	異動開始時間	smalldatetime	
            //ChgStopDateTime	異動結束時間	smalldatetime	
            //ChgUser	使用者代號	nvarchar	32
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Payroll_Master_Heading";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (UpdateItem.Length * 2 > 255) ? "長度:" + UpdateItem.Length.ToString() : UpdateItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        StringBuilder str;

        if (e.Exception == null)
        {
            //
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();

        if (e.Exception != null)
        {
            return;
        }
        if (hid_InserMode.Value.Trim().Equals("EXIT"))
        {
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.close();");
            str.Append("window.opener.location='AdjustSalaryMaster.aspx?Company=" + sCompany + "';");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        switch (DetailsView1.CurrentMode)
        {
            case DetailsViewMode.Insert:
                DetailsView1.InsertItem(true);
                break;
            case DetailsViewMode.Edit:
                DetailsView1.UpdateItem(true);
                break;
        }
    }
    private void BindData()
    {
        Ssql1 = "SELECT [Company],[EmployeeId],[EffectiveDate] "+
            " ,[AdjustSalaryItem]+' - '+(select SalaryName from SalaryStructure_Parameter where SalaryId=[AdjustSalaryItem] ) as [AdjustSalaryItem] "+
            " ,[AdjustSalaryReasonCode]+' - '+(select CodeName from CodeDesc where CodeID='PY#AdjCode' And CodeCode=[AdjustSalaryReasonCode] ) as [AdjustSalaryReasonCode]  "+
            " ,[AdjustSalaryReason],[ApproveDate],[OldlSalary],[NewSalary]  " +
            " FROM [AdjustSalary_Master] Where IsNull([AdjustSalaryItem],'00') != '00' ";
        
        string Csstr = "請選擇";
        string Esstr = "請選擇";
        string EDate = "";
        string strWhere = "";

        if (sCompany.Length > 0)
        {
            Csstr = sCompany.Trim();
        }

        if (sEmployeeId.Length > 0)
        {
            Esstr = sEmployeeId.Trim();
        }

        if (sEffectiveDate.Length > 0)
        {
            EDate = sEffectiveDate.Trim();
        } 

        if (DetailsView1.DefaultMode == DetailsViewMode.Insert)
        {
            string detailId = "DetailsView1$";

            if (Request.Form[detailId + "DDL01$ddlCodeList"] != null)
            {
                Csstr = Request.Form[detailId + "DDL01$ddlCodeList"].ToString();
            }

            if (Request.Form[detailId + "DDL02"] != null)
            {
                Esstr = Request.Form[detailId + "DDL02"].ToString();
            }
        }

        if (Csstr.Length > 0)
        {
            strWhere += string.Format(" And Company like '%{0}%'", Csstr.Trim());
        }

        if (Esstr.Length > 0)
        {
            strWhere += string.Format(" And EmployeeId like '%{0}%'", Esstr.Trim());
        }

        if (EDate.Length > 0)
        {
            strWhere += string.Format(" And Convert(char,EffectiveDate,111)='{0}' ", EDate.Trim());
        }

        SDS_GridView.SelectCommand = Ssql1 + strWhere + " ORDER BY AdjustSalaryItem";
        GridView1.DataBind();
        //Navigator1.BindGridView = GridView1;
        //Navigator1.DataBind();
    }
    /// <summary>
    /// 新增個人薪資項目
    /// </summary>
    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";        
        //string detailId = "DetailsView1$";
        //string Csstr = "";
        //string Esstr = "";        
        //if (String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "5"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()))
        //{
        //    return;
        //}
        
        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (Request.Form[temId + "3"].ToString().Length > 7 || Request.Form[temId + "4"].ToString().Length > 7)
            {
                lbl_Msg2.Text = "新增失敗!!  原因: 金額超過7個字";

            }
            //if (Request.Form[detailId + "DDL01"].ToString() != "請選擇" && Request.Form[detailId + "DDL02"].ToString() != "請選擇")
            //{
            //    Csstr = Request.Form[detailId + "DDL01"].Substring(0, Request.Form[detailId + "DDL01"].IndexOf(" "));
            //    Esstr = Request.Form[detailId + "DDL02"].Substring(0, Request.Form[detailId + "DDL02"].IndexOf(" "));
            //}
            if (!ValidateData(sCompany, sEmployeeId, sEffectiveDate, Request.Form[temId + "1$ddlCodeList"].ToString().Trim()))
            {
                lbl_Msg2.Text = "新增失敗!!  原因: 資料重覆";

            }
            //新增
            if (lbl_Msg2.Text == "")
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Company", sCompany);
                SDS_GridView.InsertParameters.Add("EmployeeId", sEmployeeId);
                SDS_GridView.InsertParameters.Add("EffectiveDate", sEffectiveDate);                
                SDS_GridView.InsertParameters.Add("AdjustSalaryItem", Request.Form[temId + "1$ddlCodeList"].ToString());
                SDS_GridView.InsertParameters.Add("AdjustSalaryReasonCode", Request.Form[temId + "2$ddlCodeList"].ToString());
                SDS_GridView.InsertParameters.Add("OldlSalary", py.EnCodeAmount(decimal.Parse(Request.Form[temId + "3"].ToString())));
                SDS_GridView.InsertParameters.Add("NewSalary", py.EnCodeAmount(decimal.Parse(Request.Form[temId + "4"].ToString())));
                SDS_GridView.InsertParameters.Add("AdjustSalaryReason", Request.Form[temId + "5"].ToString());

                #region 開始異動前,先寫入LOG
                //TableName	異動資料表	varchar	60
                //TrxType	異動類別(A/U/D)	char	1
                //ChangItem	異動項目(欄位)	varchar	255
                //SQLcommand	異動項目(異動值/指令)	varchar	2000
                //ChgStartDateTime	異動開始時間	smalldatetime	
                //ChgStopDateTime	異動結束時間	smalldatetime	
                //ChgUser	使用者代號	nvarchar	32
                DateTime StartDateTime = DateTime.Now;
                MyCmd.Parameters.Clear();
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AdjustSalary_Master";
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "薪資項目:" + Request.Form[temId + "1$ddlCodeList"].ToString();
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "金額:" + Request.Form[temId + "4"].ToString();
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                //此時不設定異動結束時間
                //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                int i = 0;
                try
                {
                    i = SDS_GridView.Insert();
                }
                catch (Exception ex)
                {
                    lbl_Msg2.Text = ex.Message;
                }
                if (i == 1)
                {
                    lbl_Msg2.Text = i.ToString() + " 個資料列 " + "新增成功!!";
                    //DetailsView1.InsertItem(true);
                }
                else
                {
                    lbl_Msg2.Text = "新增失敗!!" + lbl_Msg2.Text;
                }

                #region 完成異動後,更新LOG資訊
                MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg2.Text;
                MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                BindData();
                //showPanel();
            }
            else
            {
                BindData();
            }
        }
        //hid_IsInsertExit.Value = "";
    }
    private bool ValidateData(string strCompany, string strEmployeeId, string strEffectiveDate, string strSalaryItem)
    {
        //判斷資料是否重覆
        Ssql = "Select * From AdjustSalary_Master WHERE Company = '" + strCompany + "' And EmployeeId='" + strEmployeeId +
            "' And EffectiveDate = Convert(smalldatetime,'" + strEffectiveDate + "') And [AdjustSalaryItem] = '" + strSalaryItem + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }
    private bool ValidateData2(string strCompany, string strEmployeeId, string strEffectiveDate)
    {
        Ssql = "Select * From AdjustSalary_Master Where Company='" + strCompany + "' And EmployeeId='" + strEmployeeId + "' And EffectiveDate = Convert(smalldatetime,'" + strEffectiveDate + "')";
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 刪除調薪項目
    /// </summary>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        //Company	公司編號
        //EmployeeId	員工編號
        //SalaryItem	薪資項目
        //Amount	金額
        //string L1PK = btnDelete.Attributes["L1PK"].ToString();
        //string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string L1PK = sCompany;
        string L2PK = sEmployeeId;
        string L3PK = btnDelete.Attributes["L3PK"].ToString().Remove(2).Trim();

        #region 開始異動前,先寫入LOG
        //TableName	異動資料表	varchar	60
        //TrxType	異動類別(A/U/D)	char	1
        //ChangItem	異動項目(欄位)	varchar	255
        //SQLcommand	異動項目(異動值/指令)	varchar	2000
        //ChgStartDateTime	異動開始時間	smalldatetime	
        //ChgStopDateTime	異動結束時間	smalldatetime	
        //ChgUser	使用者代號	nvarchar	32
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AdjustSalary_Master";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        //string sql = "Delete From Payroll_Master_Detail Where Company='" + L1PK + "' And EmployeeId='" + L2PK + "' And SalaryItem='" + L3PK + "'";//Company='" + L1PK + "' And EmployeeId='" + L2PK + "' And
        string sql = "Delete From AdjustSalary_Master Where IsNull(AdjustSalaryItem,'00') = '00' Or EffectiveDate Is Null Or " +
            " (AdjustSalaryItem='" + L3PK + "' And Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "' And Convert(varchar,EffectiveDate,111)='" + sEffectiveDate + "')";
        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            lbl_Msg2.Text = "資料刪除成功 !!";          
            //Navigator1.DataBind();
        }
        else
        {
            lbl_Msg2.Text = "資料刪除失敗 !!";
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg2.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();
        //showPanel();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandName))
        {
            try
            {
                //GridViewRow gvRow = null;
                switch (e.CommandName)
                {
                    case "Delete":
                    case "Update":
                    case "Insert":
                        break;
                    case "Edit":
                    case "Cancel":
                    default:
                        lbl_Msg.Text = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";
        }
        else if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer) || (e.Row.RowType == DataControlRowType.EmptyDataRow))
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            int i = 0;
            for (i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData();
    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindData();
    }
    /// <summary>
    /// 修改個人薪資項目
    /// </summary>
    protected void GridView1RowUpdate()
    {
        string ID = hid_updateid.Value;
        string strClientID = hid_IsInsertExit.Value;
        string strAdjustSalaryReasonCode = Request.Form[strClientID + "$ctl02$ddlCodeList"].Trim();
        string ValueOld = Request.Form[strClientID + "$ctl03"].Trim().Replace(",", "");
        string ValueNew = Request.Form[strClientID + "$ctl04"].Trim().Replace(",", "");
        string strAdjustSalaryReason = Request.Form[strClientID + "$ctl05"].Trim();

        int i = 0;
        lbl_Msg2.Text = "修改失敗!<br>調薪項目[" + ID + "]的金額必需為0或正整數!<br>請重新輸入!";
        if (int.TryParse(ValueOld, out i) == false || int.TryParse(ValueNew, out i) == false)
        {
            i = -1;
        }

        if (i >= 0)
        {
            #region 開始異動前,先寫入LOG
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AdjustSalary_Master";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = ID;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            try
            {
                Ssql = "UPDATE AdjustSalary_Master SET AdjustSalaryReasonCode='" + strAdjustSalaryReasonCode + "' " +
                    " ,OldlSalary='" + py.EnCodeAmount(ValueOld) + "' " +
                    " ,NewSalary='" + py.EnCodeAmount(ValueNew) + "' " +
                    " ,AdjustSalaryReason='" + strAdjustSalaryReason + "' " +
                    " WHERE Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "' " +
                    " And Convert(varchar,EffectiveDate,111)='" + sEffectiveDate + "' " +
                    " And AdjustSalaryItem='" + ID.Remove(2) + "'";

                i = _MyDBM.ExecuteCommand(Ssql);
                if (i > 0)
                    lbl_Msg2.Text = "修改成功!<br>已更新調薪項目[" + ID + "]的資料共" + i.ToString() + "筆";
                else
                    lbl_Msg2.Text = "修改失敗!<br>找不到調薪項目[" + ID + "]的資料";
            }
            catch (Exception ex)
            {
                lbl_Msg2.Text = ex.Message;
            }
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = ((i == 1) ? "Success" : lbl_Msg2.Text.Replace("<br>", "")) +"|"+ Ssql;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }

        hid_updateid.Value = "";
        hid_IsInsertExit.Value = "";
        GridView1.EditIndex = -1;
        BindData();


    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //
    }
    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        //
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            string tempValue = e.Row.Cells[2].Text.Trim();
            DataTable dt4 = _MyDBM.ExecuteDataTable("SELECT SalaryId,SalaryName FROM SalaryStructure_Parameter Where SalaryId='" + tempValue + "'");
            if (dt4 != null)
            {
                if (dt4.Rows.Count > 0)
                {
                    tempValue = dt4.Rows[0]["SalaryId"].ToString() + " - " + dt4.Rows[0]["SalaryName"].ToString();
                }
            }

            e.Row.Cells[2].Style.Add("text-align", "left");            

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return (confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "') && SaveValue(" + hid_updateid.ClientID + ",'" + tempValue.Trim() + "'));");
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");                    
                }
                //取消
                if (e.Row.Cells[1].Controls[2] != null)
                {
                    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }                
                #endregion
                //薪資項目                
                e.Row.Cells[2].Text = tempValue;

                //調薪原因                
                ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                ddlAddNew.ID = "ctl02";
                ddlAddNew.SetCodeList("PY#AdjCode");
                ddlAddNew.SelectedCode = ((TextBox)e.Row.Cells[3].Controls[0]).Text.Remove(2).Trim();
                e.Row.Cells[3].Controls.Clear();
                e.Row.Cells[3].Controls.Add(ddlAddNew);

                //金額
                for (int i = 4; i <= 5; i++)
                {
                    TextBox TB = ((TextBox)e.Row.Cells[i].Controls[0]);
                    if (TB != null)
                    {
                        TB.Style.Add("text-align", "right");
                        TB.MaxLength = 7;
                        TB.Text = py.DeCodeAmount(TB.Text).ToString();
                    }
                }
            }
            else
            {
                #region 查詢用
                //e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;
                //if (e.Row.Cells[0].Controls[0] != null)
                //{
                //    LinkButton LB = (LinkButton)e.Row.Cells[0].Controls[1];
                //    if (((DataRowView)DataBinder.GetDataItem(e.Row)).Row["ItemType"] != null)
                //        if (((DataRowView)DataBinder.GetDataItem(e.Row)).Row["ItemType"].ToString().Equals("0"))
                //            LB.Text = "";
                //}

                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");                    
                }
                #endregion
                //薪資項目                
                e.Row.Cells[2].Text = tempValue;
                //金額
                e.Row.Cells[4].Style.Add("text-align", "right");                
                e.Row.Cells[4].Text = int.Parse(py.DeCodeAmount(e.Row.Cells[4].Text).ToString()).ToString("N0");
                e.Row.Cells[5].Style.Add("text-align", "right");                
                e.Row.Cells[5].Text = int.Parse(py.DeCodeAmount(e.Row.Cells[5].Text).ToString()).ToString("N0");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (i < 4)
                {//下拉式選單
                    ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();
                    ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    ddlAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');                    
                    if (i == 2)
                    {
                        ddlAddNew.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5);
                        ddlAddNew.AutoPostBack = true;
                        ddlAddNew.SelectedIndexChanged += new EventHandler(SelectedIndexChanged1);
                    }
                    else
                        ddlAddNew.SetCodeList("PY#AdjCode");
                    
                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "150px");
                    tbAddNew.MaxLength = 7;
                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }                
            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";

            for (int i = 1; i < 5; i++)
            {
                if (i < 3)
                {
                    ASP.usercontrol_codelist_ascx ddlAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    if (i == 1)
                    {
                        ddlAddNew.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5);
                        ddlAddNew.AutoPostBack = true;
                        ddlAddNew.SelectedIndexChanged += new EventHandler(SelectedIndexChanged1);
                    }
                    else
                        ddlAddNew.SetCodeList("PY#AdjCode");
                    strValue += "checkColumns(" + ddlAddNew.CLClientID() + ") && ";
                }
                else
                {
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "150px");
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

    protected void SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            DropDownList temp = ((DropDownList)sender);
            TextBox tb3 = (TextBox)temp.NamingContainer.Parent.FindControl("tbAddNew03");
            //
            string sqlstr1 = temp.SelectedValue.ToString().Trim();
            if (sqlstr1 == "" && Request.Form[temp.ClientID.Replace("_", "$")] != null) sqlstr1 = Request.Form[temp.ClientID.Replace("_", "$")].ToString().Trim();
            sqlstr1 = "SELECT Amount FROM Payroll_Master_Detail where SalaryItem='" + sqlstr1 + "' And Company='" + sCompany.Trim() + "' And EmployeeId='" + sEmployeeId.Trim() + "'";
            DataTable dt2 = _MyDBM.ExecuteDataTable(sqlstr1);
            if (tb3 != null)
            {
                tb3.Text = "";
                tb3.ReadOnly = true;
                if (dt2.Rows.Count > 0)
                {
                    tb3.Text = py.DeCodeAmount(dt2.Rows[0]["Amount"].ToString()).ToString();
                }
                else
                {
                    tb3.Text = "0";
                }
            }
        }
        catch { }
    }
}