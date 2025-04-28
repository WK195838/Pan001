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

/// <summary>
/// 2013/12/11 增加加計年資欄位與介面位置調整，並配合修改年資計算
/// 2013/12/12 依需求控管加計年資之填寫
/// </summary>
public partial class PersonnelMaster_M : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM001";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany, sDepId, sEmployeeId;
    String savePath, theFileName, theFileStyle;
    bool blReadOnly = false;
    int iTabCount = 8;
    bool blTabSecurity = false;
    int DLShowKind = 2;
    string DLDefItem = "";
    //查詢健保金額權限
    bool blHIIAShow = false;

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Main/PersonnelMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
        savePath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("picture") + "\\";
        blTabSecurity = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "TabSecurity");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //查詢健保金額權限
        blHIIAShow = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "HIIA");

        ScriptManager.RegisterStartupScript(UpdatePanel0, this.GetType(), "", @"JQ();", true);
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);

        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString());        

        bool blInsertMod = false;

        #region 依人事2011/05/11建議調整,使可設定是否顯示代碼
        try
        {//設定下拉單是否顯示代碼或預設未選項
            DLShowKind = int.Parse(_UserInfo.SysSet.GetConfigString("DLShowKind"));
        }
        catch { }
        try
        {//設定未選項文字
            DLDefItem = _UserInfo.SysSet.GetConfigString("DLDefItem");
        }
        catch { }        
        #endregion

        #region 照片設定
        ib_Picture.AlternateText = "上傳照片";
        ib_Picture.ImageUrl = _UserInfo.SysSet.GetConfigString("picture") + "/picture.jpg";

        if ((Request["Company"] == null) && (Request["EmployeeId"] == null))
        {//新增模式
            if (Request["addCompany"] != null) sCompany = Request["addCompany"];
            if (Request["addDep"] != null) sDepId = Request["addDep"];            
            if (Request["addEmployeeID"] != null) sEmployeeId = Request["sEmployeeId"];
            //if (Request["addEmployeeName"] != null) sEmployeeId = Request["addEmployeeName"];

            //2011/05/09 kaya 修正新增時部門無法正確寫入之問題
            ViewState["sCompany"] = sCompany;
            ViewState["sDepId"] = sDepId;            
   
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InsertMode.ClientID + ".value='GOEDIT';");
            btnSaveGoNext.Attributes.Add("onclick", "javascript:" + hid_InsertMode.ClientID + ".value='GO';");
            btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InsertMode.ClientID + ".value='EXIT';");
            btnCancel.Attributes.Add("onclick", "javascript:window.close();");
            blInsertMod = true;

            //新增時不顯示頁籤
            UpdatePanel1.Visible = false;

   

            //theFileName = "Pic_" + _UserInfo.UData.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");


            if (Session["Image"] == null)
            {
                //產生暫存名稱
                theFileName = "Pic_" + _UserInfo.UData.Company + "_" + _UserInfo.UData.UserId + DateTime.Now.ToString("yyyyMMddHHmmss");
                Session["Image"] = theFileName;
            }

            theFileStyle = _UserInfo.SysSet.chekPic(savePath + Session["Image"].ToString());
         
            if (theFileStyle.Length > 0)
            {
                ib_Picture.AlternateText = "變更照片";
                ib_Picture.ImageUrl = _UserInfo.SysSet.GetConfigString("picture") + "/" + Session["Image"].ToString() + theFileStyle;
            }

            //ib_Picture.Attributes.Add("onclick", "return Upload(" + hid_UplodFileStyle.ClientID + ",'picture','" + _UserInfo.SysSet.rtnHash(savePath).Replace('\\', '＼') + "','" + theFileName + "');");
            ib_Picture.OnClientClick = "return UploadTo(" + hid_UplodFileStyle.ClientID + ",'Y','picture','" + _UserInfo.SysSet.rtnHash(savePath).Replace('\\', '＼') + "','" + Session["Image"] + "');";

            if (Request.Form["DetailsView1$txt_CompL_Company$companyList"] != null)
            {//取得公司選項,決定是否重新載入部門下拉單
                sCompany = Request.Form["DetailsView1$txt_CompL_Company$companyList"].ToString();
                if (ViewState["sCompany"] == null) ViewState["sCompany"] = "";

                bool reloadDeptId = false;
                if (!ViewState["sCompany"].Equals(sCompany))
                {
                    reloadDeptId = true;
                }
                else if (Request.Form["DetailsView1$txt_CL_DeptId$ddlCodeList"] == null)
                {
                    reloadDeptId = true;
                }
                else if (string.IsNullOrEmpty(Request.Form["DetailsView1$txt_CL_DeptId$ddlCodeList"].ToString()))
                {
                    reloadDeptId = true;
                }

                if (reloadDeptId == true)
                {
                    ASP.usercontrol_codelist_ascx theCL = (ASP.usercontrol_codelist_ascx)DetailsView1.FindControl("txt_CL_DeptId");
                    if (theCL != null)
                    {//2011/05/09 kaya 修正新增時部門無法正確寫入之問題
                        if (!ViewState["sDepId"].Equals(theCL.SelectedCode))
                        {
                            ViewState["sDepId"] = theCL.SelectedCode;
                        }
                        theCL.SetDTList("Department", "DepCode", "DepName", "Company='" + sCompany + "'", 2);
                        theCL.SelectedCode = ViewState["sDepId"].ToString();
                    }
                    ViewState["sCompany"] = sCompany;
                }
            }
        }
        else
        {
            UpdatePanel1.Visible = true;

            if (Request["Kind"] != null)
            {
                blReadOnly = Request["Kind"].Equals("Query");
            }
            
            //修改模式
            trInsert.Style.Add("display", "none");
            try
            {
                theFileName = "Pic_" + Request["Company"].ToString().Trim() + "_" + Request["EmployeeId"].ToString().Trim();
            }
            catch {
                theFileName = "picture";
            }
            theFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName);
            
            string theShowPic = theFileName;
            if (theFileStyle.Length > 0)
            {
                ib_Picture.AlternateText = "點擊上傳，可變更照片";
                if (System.IO.File.Exists(savePath + theFileName + "_new" + theFileStyle))
                {
                    theShowPic = theFileName + "_new";
                }
                else
                {
                    theShowPic = theFileName;
                }
                ib_Picture.ImageUrl = _UserInfo.SysSet.GetConfigString("picture") + "/" + theShowPic + theFileStyle;
            }
            else
            {
                theShowPic = theFileName + "_new";
                theFileStyle = _UserInfo.SysSet.chekPic(savePath + theShowPic);
                if (System.IO.File.Exists(savePath + theShowPic + theFileStyle))
                {
                    ib_Picture.ImageUrl = _UserInfo.SysSet.GetConfigString("picture") + "/" + theShowPic + theFileStyle;
                }
                else
                {
                    theShowPic = theFileName;
                }
            }
            //ib_Picture.Attributes.Add("onclick", "return Upload(" + hid_UplodFileStyle.ClientID + ",'picture','" + _UserInfo.SysSet.rtnHash(savePath).Replace('\\', '＼') + "','" + theFileName + "_new" + "');");
            ib_Picture.OnClientClick = "return UploadTo(" + hid_UplodFileStyle.ClientID + ",'Y','picture','" + _UserInfo.SysSet.rtnHash(savePath).Replace('\\', '＼') + "','" + theFileName + "_new" + "');";
        }
        #endregion

        #region 權限控管
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Detail");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");

                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, (blInsertMod ? "Add" : "Modify"));
                if (blCheckProgramAuth)
                {
                    DetailsView1.DefaultMode = (blInsertMod ? DetailsViewMode.Insert : DetailsViewMode.Edit);
                    DetailsView01.DefaultMode = DetailsView1.DefaultMode;
                }
            }
        }
        #endregion

        //標題列不顯示回上一頁
        StyleTitle0.ShowBackToPre = false;

        GridView theGv;
        #region 設定頁籤使用的KEY和各項設定
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
            sEmployeeId = DetailsView1.DataKey[1].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
            if (Request["EmployeeId"] != null)
                sEmployeeId = Request["EmployeeId"].Trim();
        }

        HiddenField theHiddenField;
        HtmlTableCell theTdTab;
        GridView tempGv;
        ASP.usercontrol_navigator_gv_ascx tempNg;
        ASP.usercontrol_styletitle_ascx tempStyleTitle;

        int tabId = 0;

        for (int i = 1; i <= iTabCount; i++)
        {
            theHiddenField = (HiddenField)this.Form.FindControl("hid_IsInsertExit0" + i.ToString());
            //頁籤之標題列不顯示回上一頁
            tempStyleTitle = (ASP.usercontrol_styletitle_ascx)this.Form.FindControl("StyleTitle" + i.ToString());
            tempStyleTitle.ShowBackToPre = false;

            if (!Page.IsPostBack)
            {
                theGv = (GridView)this.Form.FindControl("GridView0" + i.ToString());
                //設定每個頁籤中的功能權限
                if (theGv != null)
                    AuthRight(theGv);

                if (i == 1)
                {
                    //先隱藏所有頁籤
                    setTabBg(0);
                    //暫時改為不預設顯示頁籤
                    //lbTab1_Click(sender, e);
                }
            }
            else
            {
                //找出是否有頁籤要做新增
                if (i > 1)
                {
                    string theKey = "$tbAddNew01" + ((!((i == 4) || (i == 5))) ? "" : "$ddlCodeList");
                    if (Request.Form[theHiddenField.Value.Replace("_", "$") + theKey] != null)
                    {
                        tabId = i;
                    }
                }

                if (tabId != 0)
                {//新增
                    btnEmptyNew_Click(tabId, sender, e);
                    tabId = 0;
                }
            }

            if ((i > 1) && (i != 8))
            {//i==1是主檔,故不用做下列設定
                theHiddenField.Value = "";
                tempGv = ((GridView)this.Form.FindControl("GridView0" + i.ToString()));
                tempNg = ((ASP.usercontrol_navigator_gv_ascx)this.Form.FindControl("Navigator0" + i.ToString()));
                tempNg.BindGridView = tempGv;
            }

            //TAB            
            theTdTab = (HtmlTableCell)this.Form.FindControl("tdTab" + i.ToString());
            theTdTab.Attributes.Add("onmouseover", "setnewbg(this);");
            theTdTab.Attributes.Add("onmouseout", "setoldbg(this);");
        }
        #endregion

        //預設健保投保資料
        UpdateHIData();

        #region 查詢顯示控管
        if (blReadOnly)
        {
            ib_Picture.Enabled = false;
            ib_Picture.AlternateText = "";

            DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            //DetailsView1.Enabled = false;
            DetailsView01.DefaultMode = DetailsViewMode.ReadOnly;
            //DetailsView01.Enabled = false;
            DetailsView07.DefaultMode = DetailsViewMode.ReadOnly;
            DetailsView08.DefaultMode = DetailsViewMode.ReadOnly;

            for (int i = 2; i <= iTabCount; i++)
            {
                theGv = (GridView)this.Form.FindControl("GridView0" + i.ToString());
                if (theGv != null)
                {
                    theGv.ShowFooter = false;
                    theGv.Columns[0].Visible = false;
                    theGv.Columns[1].Visible = false;
                }
            }
        }
        #endregion
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



    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
                
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        Ssql = "Select Top 1 * From Personnel_Master ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "", tempValue = "";
        InsertItem = "ALL";
        string tempCompList = "lbl";

        if (dtTitle.Columns.Count > 0)
        {
            SqlDataSource1.InsertParameters.Clear();
            string tbCols = "", tbValues = "";
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();
                tempValue = (((DetailsView)sender).CurrentMode == DetailsViewMode.ReadOnly ? "lbl" : "txt");
                TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);
                ASP.usercontrol_codelist_ascx theCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl(tempValue + "_CL_" + tempTitle);
                
                if (((DetailsView)sender).CurrentMode.Equals(DetailsViewMode.Edit))
                    tempCompList = "lbl";
                else
                    tempCompList = tempValue;
                ASP.usercontrol_companylist_ascx theCompL = (ASP.usercontrol_companylist_ascx)thisDvRow.FindControl(tempCompList + "_CompL_" + tempTitle);

                if (tempTitle.Equals("Company") && theCompL.SelectValue=="")
                {
                    Err += "請選擇" + ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "<br>";
                }
                else if (tempTitle.Equals("EmployeeId") && tb.Text.Trim().Length == 0)
                {
                    Err += ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "不可空白<br>";
                }

                if (theCompL != null)
                {
                    tempValue = theCompL.SelectValue.Trim();                    
                }
                else if (theCL != null)
                {
                    tempValue = theCL.SelectedCode.Trim();
                    if (tempValue.Equals(""))
                    {                        //必選下拉單
                        string theNeedCLList = ",DeptId,Sex,EducationCode,ResignCode,TitleCode,";
                        if (theNeedCLList.Contains("," + tempTitle + ","))
                        {//新增時不用檢核下半部的資料|| tempTitle.Equals("PayCode")
                            Err += "請選擇[" + ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "]<br>";
                        }
                    }
                }
                else
                {
                    if (tb != null)
                        tempValue = tb.Text.Trim();
                    else
                        tempValue = "";
                }

                #region 2013/12/12 依需求控管加計年資之填寫
                int tempInt = 0;
                if (tempTitle.Equals("PlusYear") && tempValue != "")
                {
                    if (int.TryParse(tempValue, out tempInt) == false)
                        Err += ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "之[年]必須是數字<br>";
                }
                else if (tempTitle.Equals("PlusMonth") && tempValue != "")
                {
                    if (int.TryParse(tempValue, out tempInt) == false)
                        Err += ((Label)thisDvRow.FindControl("lblTitle_PlusYear")).Text + "之[月]必須是數字<br>";
                    else if (tempInt > 11)
                        Err += ((Label)thisDvRow.FindControl("lblTitle_PlusYear")).Text + "之[月]不可大於11!滿12個月請於[年]中加計<br>";
                }
                #endregion

                if (tempTitle.ToUpper().Contains("DATE") || tempTitle.Equals("LeaveWithoutPay"))
                {//將日期欄位格式化                        
                    try
                    {
                        tempValue = _UserInfo.SysSet.FormatADDate(tempValue);
                        if (tempValue.Contains("DateTime"))
                            tempValue = "1912/01/01";
                    }
                    catch
                    {
                        tempValue = "1912/01/01";
                    }
                }
                else if (tempTitle.Equals("DependentsNum") || tempTitle.Equals("SpecialSeniority"))
                {//數字欄位若空白需預設為0
                    if (tempValue.Length == 0)
                        tempValue = "0";
                }                
                else
                {//文字欄位若空白需預設為空格
                    if (tempValue.Length == 0)
                        tempValue = " ";
                }

                InsertValue += tempValue.Trim() + "|";
                SqlDataSource1.InsertParameters.Add(tempTitle, tempValue);
                                
                tbCols += "[" + tempTitle + "],";
                tbValues += "@" + tempTitle + ",";
            }
            Ssql = "Insert Into Personnel_Master (" + tbCols + ") VALUES (" + tbValues + ")";
            SqlDataSource1.InsertCommand = Ssql.Replace(",)", ")");
        }

        if (Err.Equals(""))
        {
            ASP.usercontrol_companylist_ascx theCompL = (ASP.usercontrol_companylist_ascx)thisDvRow.FindControl(tempCompList + "_CompL_Company");
            
            sCompany = theCompL.SelectValue.Trim();
            sEmployeeId = ((TextBox)thisDvRow.FindControl("txt_EmployeeId")).Text.Trim();
            if (!ValidateData(sCompany, sEmployeeId))
            {
                Err += "代號重覆!!此公司下已有此員工代號!";
                sCompany = "";
                sEmployeeId = "";
            }
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
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Personnel_Master";
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
    }

    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        StringBuilder str;
        //DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        if (e.Exception == null)
        {
            lbl_Msg.Text = "新增成功!!";
            if (Session["Image"] != null)
            {
                theFileStyle = _UserInfo.SysSet.chekPic(savePath + Session["Image"].ToString());
                if (theFileStyle.Length > 0)
                {
                    string theTempPic = savePath + Session["Image"].ToString() + theFileStyle;
                    string theCopyFile = savePath + "Pic_" + sCompany + "_" + sEmployeeId + theFileStyle;

                    System.IO.File.Move(theTempPic, theCopyFile);

                    Session["Image"] = null;

                    ib_Picture.AlternateText = "上傳照片";
                    ib_Picture.ImageUrl = _UserInfo.SysSet.GetConfigString("picture") + "/picture.jpg";
                }
            }
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

        if (e.Exception != null)
        {
            return;
        }
                
        if (hid_InsertMode.Value.Trim().Equals("EXIT"))
        {
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='PersonnelMaster.aspx';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
        else if (hid_InsertMode.Value.Trim().Equals("GOEDIT"))
        {
            Response.Redirect(Request.RawUrl.Remove(Request.RawUrl.IndexOf(".aspx") + 5) + "?Company=" + sCompany.Trim() + "&EmployeeId=" + sEmployeeId.Trim() + "");
        }
    }

    private bool ValidateData(string Company, string EmployeeId)
    {
        Ssql = "Select * From Personnel_Master Where Company='" + Company + "' And EmployeeId='" + EmployeeId + "'";
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


    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {        
        DetailsView thisDv = (DetailsView)sender;
        DetailsViewRow thisDvRow = thisDv.Rows[0];
        Ssql = "Select Top 1 * From Personnel_Master ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string strJavascript = "", tempValue = "";
        int tempYear = 0, tempMonth = 0;
        //年齡
        Label theYears = ((Label)thisDvRow.FindControl("lbl_Year01"));
        if (theYears != null)
        {
            try
            {
                tempYear = ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["BirthDate"]).Year;
            }
            catch 
            {
                tempYear = 0;
            }

            if ((tempYear > 1912) || (tempYear > 1 && tempYear < 1900))
                theYears.Text = (DateTime.Today.Year - tempYear).ToString().Trim();
            else
                theYears.Text = "";
        }
        //年資        
        theYears = ((Label)thisDvRow.FindControl("lbl_Year02"));
        if (theYears != null)
        {            
            #region 年資計算
            //2013/12/11 增加加計年資欄位與介面位置調整，並配合修改年資計算---Start
            try
            {                
                //ReHireDate
                tempYear = ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["HireDate"]).Year;
                tempMonth = ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["HireDate"]).Month;

                if (((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ReHireDate"]) > ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["HireDate"]))
                {//復職日期大於到職日時,使用職日期
                    tempYear = ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ReHireDate"]).Year;
                    tempMonth = ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ReHireDate"]).Month;
                }
            }
            catch
            {
                tempYear = 0;
                tempMonth = 0;
            }             
            //計算年資
            int InCompanyYear = 0;
            int InCompanyMonth = 0;
            //計算在公司內的在職年資
            if ((tempYear > 1912) || (tempYear > 1 && tempYear < 1900))
            {
                if (((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ResignDate"]) > ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["HireDate"])
                    && ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ResignDate"]) > ((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ReHireDate"])
                    )
                {//離職日期大於到職日&復職日期時
                    InCompanyYear = (((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ResignDate"]).Year - tempYear);
                    InCompanyMonth = (((DateTime)((DataRowView)DataBinder.GetDataItem(sender)).Row["ResignDate"]).Month - tempMonth);
                    tempYear = -1;
                    tempMonth = -1;
                }
                else
                {
                    InCompanyYear = (DateTime.Today.Year - tempYear);
                    InCompanyMonth = (DateTime.Today.Month - tempMonth);
                }
                if (InCompanyMonth < 0)
                {
                    InCompanyMonth += 12;
                    InCompanyYear--;
                }
            }

            if (tempYear != -1 && tempMonth != -1)
            {
                try
                {//加入加計年資
                    InCompanyYear += ((int)((DataRowView)DataBinder.GetDataItem(sender)).Row["PlusYear"]);
                    InCompanyMonth += ((int)((DataRowView)DataBinder.GetDataItem(sender)).Row["PlusMonth"]);
                }
                catch
                {
                }
            }

            if (InCompanyMonth > 11)
            {
                InCompanyYear += InCompanyMonth / 12;
                InCompanyMonth = InCompanyMonth % 12;
            }
            #endregion
            if (InCompanyYear > 0 || InCompanyMonth > 0)
                theYears.Text = " " + InCompanyYear.ToString() + " 年 " + InCompanyMonth.ToString() + " 月";
            else
                theYears.Text = "";
            //2013/12/11 增加加計年資欄位與介面位置調整，並配合修改年資計算---end
        }

        if (dtTitle.Columns.Count > 0)
        {
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                string tempTitle = dtTitle.Columns[i].ColumnName.Trim();
                Ssql = "Select dbo.GetColumnTitle('Personnel_Master','" + tempTitle + "'),IsNull(COLUMNPROPERTY( OBJECT_ID('Personnel_Master'),'" + tempTitle + "','PRECISION'),0)";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

                if (DT.Rows.Count > 0)
                {
                    //欄位標題名稱
                    Label theTitle = ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle));
                    //DB新增欄位,但程式尚未修改時先跳過此欄不做
                    //if (tempTitle.ToLower() == "rank") continue ;
                    if (theTitle == null) continue;

                    if (theTitle.Text.Trim().Equals(tempTitle))
                        theTitle.Text = DT.Rows[0][0].ToString().Trim();
                    //DB欄位長度
                    int colLen = (int)Convert.ToDecimal(DT.Rows[0][1].ToString());

                    tempValue = (thisDv.CurrentMode == DetailsViewMode.ReadOnly ? "lbl" : "txt");
                    TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);                    
                    ASP.usercontrol_codelist_ascx theCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl(tempValue + "_CL_" + tempTitle);
                    ASP.usercontrol_companylist_ascx theCompL = (ASP.usercontrol_companylist_ascx)thisDvRow.FindControl(tempValue + "_CompL_" + tempTitle);

                    if (thisDv.CurrentMode != DetailsViewMode.Insert)
                    {//KEY值設為唯讀:除了新增外,都不可修改的欄位
                        if (tempTitle.Equals("Company"))
                        {
                            if (theCompL != null)
                            {   
                                theCompL.Enabled = false;
                            }
                        }
                        else if (tempTitle.Equals("EmployeeId"))
                        {
                            if (tb != null)
                                tb.ReadOnly = true;
                        }
                    }
                    else
                    {
                        ImageButton ibEdit = (ImageButton)thisDvRow.FindControl("btnEdit");
                        if (ibEdit != null)
                            ibEdit.Visible = false;

                        ibEdit = (ImageButton)thisDvRow.FindControl("btnCancelEdit");
                        if (ibEdit != null)
                            ibEdit.Visible = false;                        
                    }

                    if ((tb == null) && (theCompL != null))
                    {
                        //公司下拉單
                        if (!string.IsNullOrEmpty(sCompany))
                        {
                            theCompL.SelectValue = sCompany;
                        }                        
                    }
                    else if ((tb == null) && (theCL != null))
                    {
                        #region 代碼下拉單
                        tempValue = "PY#" + tempTitle;
                        if (tempValue.Length > 10)
                            tempValue = tempValue.Remove(10);
                        //不使用代碼表設定或欄位名稱與代碼表ID不同之特別下拉單
                        string strSpeicalCLList = ",DeptId,Grade,ResignCode,isSales,";
                        if (!strSpeicalCLList.Contains("," + tempTitle + ","))
                        {
                            theCL.SetCodeList(tempValue, DLShowKind, DLDefItem);
                            //設定預設值:
                            strSpeicalCLList = ",Identify,Shift,LISubsidy,Nationality,PayCode,MaritalStatus,IDType,isBoss,";
                            if (strSpeicalCLList.Contains("," + tempTitle + ","))
                            {
                                try
                                {
                                    tempValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[tempTitle].ToString().Trim();
                                }
                                catch
                                {
                                    tempValue = "";
                                }

                                if (string.IsNullOrEmpty(tempValue))
                                {
                                    strSpeicalCLList = ",PayCode,isBoss,";
                                    if (strSpeicalCLList.Contains("," + tempTitle + ","))
                                    {
                                        //發薪時段預設為月薪
                                        if (tempTitle.Equals("PayCode"))
                                            theCL.SelectedCode = "1";
                                        else//是否雇主預設為一般勞工
                                            theCL.SelectedCode = "N";
                                    }
                                    else                                 
                                    {
                                        theCL.SelectedCode = "0";
                                    }
                                }                                 
                            }
                        }
                        else if (tempTitle.Equals("Grade"))
                        {
                            theCL.SetDTList("SalaryLevel_CheckStandard", "Level", "('核薪範圍:'+Replace(Convert(varchar,CONVERT(money,SalaryLowerLimit),1),'.00','')+'~'+Replace(Convert(varchar,CONVERT(money,SalaryUpperLimit),1),'.00',''))", "", DLShowKind);
                        }
                        else if (tempTitle.Equals("DeptId"))
                        {
                            if (string.IsNullOrEmpty(sCompany))
                                theCL.SetDTList("Department", "DepCode", "DepName", "", 2);
                            else
                                theCL.SetDTList("Department", "DepCode", "DepName", "Company='" + sCompany + "'", 2);
                            //2011/05/09 kaya 修正新增時部門無法正確寫入之問題
                            if (sDepId != null)
                                theCL.SelectedCode = sDepId;
                        }
                        else if (tempTitle.Equals("ResignCode"))
                        {//2011/05/12 kaya 依人事建議,離職碼不含已離職(為FOR顯示已離職,還是要有)
                            try
                            {
                                tempValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[tempTitle].ToString();
                            }
                            catch
                            {
                                tempValue = "";
                            }
                            if (tempValue != "Y")
                                theCL.SetDTList("CodeDesc", "CodeCode", "CodeName", "CodeID='PY#ResignC' and CodeCode != 'Y'", DLShowKind);
                            else
                                theCL.SetCodeList("PY#ResignC", DLShowKind);
                            if (string.IsNullOrEmpty(tempValue))
                                theCL.SelectedCode = "N";
                        }
                        else if (tempTitle.Equals("isSales"))
                        {//2011/11/07 新增業務員身份設定:使用是/否設定
                            try
                            {
                                tempValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[tempTitle].ToString();
                            }
                            catch
                            {
                                tempValue = "";
                            }
                            theCL.SetCodeList("PY#Regular", DLShowKind);
                            if (tempValue != "Y")
                                theCL.SelectedCode = "N";
                        }

                        try
                        {
                            tempValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[tempTitle].ToString();
                        }
                        catch
                        {
                            tempValue = "";
                        }

                        //2011/05/09 kaya 修正新增時部門無法正確寫入之問題時,應做此判斷供查詢與修改時正確顯示
                        if (tempValue.Trim() != "")
                        {
                            theCL.SelectedCode = tempValue.Trim();
                        }
                        else
                        {
                            if (thisDv.CurrentMode != DetailsViewMode.Insert)
                            {
                                //依人事2011/05/06建議調整性別為女性時,兵役設定預設值
                                if (tempTitle.Equals("Military") && ((DataRowView)DataBinder.GetDataItem(sender)).Row["Sex"].ToString().Equals("0"))
                                    theCL.SelectedCode = "2";
                            }
                        }

                        if (thisDv.CurrentMode == DetailsViewMode.ReadOnly)
                        {
                            theCL.Enabled = false;
                            if (theCL.SelectedCode == "")
                            {
                                theCL.SelectedCodeName = tempValue;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        switch (thisDv.CurrentMode)
                        {
                            case DetailsViewMode.Edit://修改模式
                            case DetailsViewMode.Insert://新增模式
                                if (tb != null)
                                {
                                    if (colLen > 0)
                                    {
                                        tb.MaxLength = ((tb.MaxLength == 0) ? colLen : ((tb.MaxLength > colLen) ? colLen : tb.MaxLength));
                                    }
                                    //指定欄位寬度
                                    tb.Style.Add("width", "100px");

                                    if (tempTitle.Equals("DependentsNum") || tempTitle.Equals("SpecialSeniority")
                                        || tempTitle.Equals("PlusYear") || tempTitle.Equals("PlusMonth"))
                                    {//數字欄位需靠右對齊                                    
                                        tb.Style.Add("text-align", "right");
                                        //預設為0
                                        if (tb.Text.Trim().Length == 0)
                                            tb.Text = "0";
                                    }
                                    else if (tempTitle.Equals("DeptId") || tempTitle.Equals("Grade"))
                                    {//為部門欄位增加提示視窗
                                        //ImageButton ib = (ImageButton)thisDvRow.FindControl("ibOW_" + tempTitle);
                                        //strJavascript = "return GetPromptWin1(" + tb.ClientID + ",'400','550','Department_Basic','DepCode,Company','DepCode As 部門代碼,DepName,ParentDepCode','Company,DepCode');";
                                        //ib.Attributes.Add("onclick", strJavascript);
                                    }
                                    else if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                                    {
                                        #region 將日期欄位格式化
                                        tb.MaxLength = 10;
                                        tb.Style.Add("text-align", "right");
                                        try
                                        {
                                            tempValue = _UserInfo.SysSet.FormatDate(tb.Text);
                                            if (tempValue.Contains("Date"))
                                                tempValue = "";
                                        }
                                        catch
                                        {
                                            tempValue = tb.Text;
                                        }
                                        
                                        if (tempValue.Contains(" Date"))
                                            tempValue = tb.Text;
                                        else if (tempValue.Trim().Equals("01/01/01") || tempValue.Trim().Equals("1912/01/01"))
                                        {
                                            tempValue = "";
                                        }

                                        tb.Text = tempValue;

                                        //為日期加小元件
                                        tb.CssClass = "JQCalendar";
                                        //ImageButton ib = (ImageButton)thisDvRow.FindControl("ibOW_" + tempTitle);
                                        //if ((DT.Rows[0][0].ToString().Trim().Contains("出生")) || (DT.Rows[0][0].ToString().Trim().Contains("到職")))
                                        //    strJavascript = "return GetPromptTheDate(" + tb.ClientID + ",'1','" + (DateTime.Today.Year - 1911).ToString() + "');";
                                        //else
                                        //    strJavascript = "return GetPromptDate(" + tb.ClientID + ");";
                                        //ib.Attributes.Add("onclick", strJavascript);
                                        #endregion
                                    }
                                    else
                                    {
                                        //其它文字欄位預設為空預設為0
                                        if (tb.Text.Trim().Length == 0)
                                            tb.Text = " ";
                                    }
                                }
                                break;
                            case DetailsViewMode.ReadOnly://唯讀模式
                                //指定欄位寬度
                                Label theLabel = (Label)thisDvRow.FindControl("lbl_" + tempTitle);
                                theLabel.Style.Add("width", "100px");

                                if (tempTitle.Equals("DependentsNum") || tempTitle.Equals("SpecialSeniority")
                                    || tempTitle.Equals("PlusYear") || tempTitle.Equals("PlusMonth"))
                                {//數字欄位需靠右對齊
                                    //Label會轉成SPAN無法對齊,只能塞空白=>"&nbsp;&nbsp;&nbsp;&nbsp;",所以改為在介面設定
                                    theLabel.Style.Add("text-align", "right");
                                }

                                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                                {//將日期欄位格式化                                
                                    try
                                    {
                                        tempValue = _UserInfo.SysSet.FormatDate(theLabel.Text);
                                    }
                                    catch
                                    {
                                        tempValue = theLabel.Text;
                                    }
                                                                        
                                    if (tempValue.Contains(" Date"))
                                        tempValue = theLabel.Text;
                                    else if (tempValue.Trim().Equals("01/01/01") || tempValue.Trim().Equals("1912/01/01"))
                                    {
                                        tempValue = "";
                                    }
                                    theLabel.Text = tempValue;
                                }

                                //if (string.IsNullOrEmpty(theLabel.Text))
                                //    theLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
                                break;
                        }
                    }
                }
            }
        }
    }       

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "";
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";
                
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        Ssql = "Select Top 1 * From Personnel_Master ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "";
        string tempValue = "";
        UpdateItem = "ALL";

        if (dtTitle.Rows.Count > 0)
        {
            SqlDataSource1.UpdateParameters.Clear();
            string tbCols = "", tbValues = "";
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();                
                TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);

                //if (tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId"))
                //{
                //    SqlDataSource1.UpdateParameters.Add(tempTitle, ((Label)thisDvRow.FindControl("lbl_" + tempTitle)).Text);
                //}
                //else

                ASP.usercontrol_codelist_ascx theCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl("txt_CL_" + tempTitle);
                tbValues = "";
                if ((tb == null) && (theCL != null))
                {//代碼下拉單
                    tbValues = " [" + tempTitle + "]=@" + tempTitle + ",";
                    if (theCL.SelectedCode.Trim().Equals(""))
                    {
                        //必選下拉單
                        string theNeedCLList = ",DeptId,Sex,EducationCode,ResignCode,TitleCode,PayCode,";
                        if (theNeedCLList.Contains("," + tempTitle + ","))
                        {
                            Err += "請選擇" + ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "<br>";
                        }
                    }

                    UpdateValue += theCL.SelectedCode + "|";
                    SqlDataSource1.UpdateParameters.Add(tempTitle, theCL.SelectedCode);                    
                }
                else if (tb != null)
                {
                    tbValues = " [" + tempTitle + "]=@" + tempTitle + ",";
                    tempValue = tb.Text;

                    #region 2013/12/12 依需求控管加計年資之填寫
                    int tempInt = 0;
                    if (tempTitle.Equals("PlusYear") && tempValue != "")
                    {
                        if (int.TryParse(tempValue, out tempInt) == false)
                            Err += ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "之[年]必須是數字<br>";
                    }
                    else if (tempTitle.Equals("PlusMonth") && tempValue != "")
                    {
                        if (int.TryParse(tempValue, out tempInt) == false)
                            Err += ((Label)thisDvRow.FindControl("lblTitle_PlusYear")).Text + "之[月]必須是數字<br>";
                        else if (tempInt > 11)
                            Err += ((Label)thisDvRow.FindControl("lblTitle_PlusYear")).Text + "之[月]不可大於11!滿12個月請於[年]中加計<br>";
                    }
                    #endregion

                    if (tempTitle.ToUpper().Contains("DATE") || tempTitle.Equals("LeaveWithoutPay"))
                    {//將日期欄位格式化            
                        tbValues = " [" + tempTitle + "]=Convert(smalldatetime,@" + tempTitle + "),";
                        try
                        {
                            tempValue = _UserInfo.SysSet.FormatADDate(tb.Text);
                            if (tempValue.Contains("DateTime"))
                                tempValue = "";
                        }
                        catch {
                            tempValue = "";
                        }
                        if (tempValue.Length == 0)
                            tb.Text = "";
                    }
                    else if (tempTitle.Equals("DependentsNum") || tempTitle.Equals("SpecialSeniority"))
                    {//數字欄位若空白需預設為0
                        if (tb.Text.Trim().Length == 0)
                            tempValue = "0";                        
                    }
                    else
                    {//文字欄位若空白需預設為空格
                        if (tb.Text.Trim().Length == 0)
                            tempValue = " ";                        
                    }

                    UpdateValue += tempValue + "|";
                    if ((tempTitle.ToUpper().Contains("DATE") || tempTitle.Equals("LeaveWithoutPay")) && (tempValue.Trim().Length == 0))
                    {
                        SqlDataSource1.UpdateParameters.Add(tempTitle, DbType.DateTime, null);
                    }
                    else
                    {
                        SqlDataSource1.UpdateParameters.Add(tempTitle, tempValue);
                    }
                }
                
                tbCols += tbValues;
            }

            Ssql = " Update Personnel_Master Set " + tbCols + "where (Company = @Company And EmployeeId = @EmployeeId) ";
            SqlDataSource1.UpdateCommand = Ssql.Replace(",where", " where");
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Personnel_Master";
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
        string theTempPicFileStyle = _UserInfo.SysSet.chekPic(savePath + theFileName + "_new");
        string theTempPic = savePath + theFileName + "_new" + theTempPicFileStyle;

        if (e.Exception == null)
        {
            lbl_Msg.Text = "更新成功!!";
            
            if (theTempPicFileStyle.Length > 0)
            {
                string theCopyFile = savePath + theFileName + _UserInfo.SysSet.chekPic(savePath + theFileName);
                if (!System.IO.File.Exists(theCopyFile))
                {
                    theCopyFile += theTempPicFileStyle;
                }

                if (System.IO.File.Exists(theTempPic))
                {
                    if (System.IO.File.Exists(theCopyFile))
                        System.IO.File.Delete(theCopyFile);
                    theCopyFile = savePath + theFileName + theTempPicFileStyle;
                    System.IO.File.Copy(theTempPic, theCopyFile, true);
                    System.IO.File.Delete(theTempPic);
                }
            }
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            if (theTempPicFileStyle.Length > 0)
            {
                System.IO.File.Delete(theTempPic);
            }
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
        {
            return;
        }

        //str = new StringBuilder();
        //str.Append("<script language=javascript>");
        //str.Append("window.close();");
        //str.Append("window.opener.location='PersonnelMaster.aspx';");
        //str.Append("</script>");
        //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        switch (e.CommandName)
        { 
            case "Cancel":
                lbl_Msg.Text = "";
                //先隱藏所有頁籤
                setTabBg(0);
                //改為不預設顯示頁籤
                //DoTab_Click(2, sender, e);
                break;
        }
    }    

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// For Tab INCLUDE FILE
    /// </summary>
    ////////////////////////////////////////////////////////////////////
    protected void lbTab1_Click(object sender, EventArgs e)
    {
        DoTab_Click(1, sender, e);
    }
    protected void lbTab2_Click(object sender, EventArgs e)
    {
        DoTab_Click(2, sender, e);
    }
    protected void lbTab3_Click(object sender, EventArgs e)
    {
        DoTab_Click(3, sender, e);
    }
    protected void lbTab4_Click(object sender, EventArgs e)
    {
        DoTab_Click(4, sender, e);
    }
    protected void lbTab5_Click(object sender, EventArgs e)
    {
        DoTab_Click(5, sender, e);
    }
    protected void lbTab6_Click(object sender, EventArgs e)
    {
        DoTab_Click(6, sender, e);
    }
    protected void lbTab7_Click(object sender, EventArgs e)
    {
        DoTab_Click(7, sender, e);
    }
    protected void lbTab8_Click(object sender, EventArgs e)
    {
        DoTab_Click(8, sender, e);
    }

    protected void DoTab_Click(int tabNo, object sender, EventArgs e)
    {
        lbl_TabMsg.Text = "";
        lbl_Msg.Text = "";

        Ssql = "";
        SqlDataSource tempSDS;
        GridView tempGv;
        ASP.usercontrol_navigator_gv_ascx tempNg;
        Label tempNoDataMsg;
        DetailsView tempDV;

        tempSDS = null;
        tempGv = null;
        tempNg = null;
        tempNoDataMsg = null;
        tempDV = null;

        if ((tabNo > 1) && (tabNo != 8))
        {
            tempSDS = ((SqlDataSource)this.Form.FindControl("SDS_GridView0" + tabNo.ToString()));
            tempGv = ((GridView)this.Form.FindControl("GridView0" + tabNo.ToString()));
            tempNg = ((ASP.usercontrol_navigator_gv_ascx)this.Form.FindControl("Navigator0" + tabNo.ToString()));
            tempNoDataMsg = ((Label)this.Form.FindControl("labNoData0" + tabNo.ToString()));
        }
        else
        {
            tempSDS = ((SqlDataSource)this.Form.FindControl("SqlDataSource" + tabNo.ToString()));
            tempDV = (DetailsView)this.Form.FindControl("DetailsView" + tabNo.ToString().PadLeft(2, '0'));
        }

        tempSDS.SelectParameters["Company"].DefaultValue = sCompany;
        tempSDS.SelectParameters["EmployeeId"].DefaultValue = sEmployeeId;

        tempSDS.DataBind();
        if (tabNo > 1)
        {
            if (tabNo != 8)
            {
                tempGv.DataBind();
                //是否顯示分頁導覽
                if (tempGv.PageCount > 1)
                {
                    tempNg.Visible = true;
                }
                else
                {
                    tempNg.Visible = false;
                }
                //是否顯示無資料訊息
                if (tempNoDataMsg != null)
                    tempNoDataMsg.Visible = ((tempGv.PageCount > 0) ? false : true);
            }

            if ((tabNo == 7) || (tabNo == 8))
            {
                DetailsView theDV = ((DetailsView)this.Form.FindControl("DetailsView" + tabNo.ToString().PadLeft(2, '0')));
                if (theDV != null)
                {
                    theDV.DefaultMode = DetailsView1.CurrentMode;
                    theDV.ChangeMode(DetailsView1.CurrentMode);
                    theDV.DataBind();

                    if ((theDV.Rows.Count == 0) && theDV.DefaultMode != DetailsViewMode.ReadOnly)
                    {
                        theDV.ChangeMode(DetailsViewMode.Insert);
                    }
                }
            }
        }
        else
        {
            tempDV.DataBind(); //不重新綁定,畫面會亂掉,但新修的資料會不見
        }

        setTabBg(tabNo);
    }    

    //各頁籤切換時,應改變的顯示
    protected void setTabBg(int showTab)
    {
        HtmlGenericControl theTab;
        HtmlTableCell theTdTab;

        for (int i = 1; i <= iTabCount; i++)
        {
            theTab = (HtmlGenericControl)this.Form.FindControl("Tab" + i.ToString());
            theTdTab = (HtmlTableCell)this.Form.FindControl("tdTab" + i.ToString());

            if (i == showTab)
            {
                theTab.Visible = true;
                theTdTab.Style.Add("background-image", "url(../App_Themes/ePayroll/images/Tab4.GIF)");
            }
            else
            {
                theTab.Visible = false;
                theTdTab.Style.Add("background-image", "url(../App_Themes/ePayroll/images/Tab1.GIF)");
            }

            if (i == 8)
            {//[安全控管]頁籤特別限定
                theTab.Visible = blTabSecurity && (i == showTab);
                theTdTab.Visible = blTabSecurity;
            }
        }
    }

    protected void GridView02_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(2, sender, e);
    }
    protected void GridView03_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(3, sender, e);
    }
    protected void GridView04_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(4, sender, e);
    }
    protected void GridView05_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(5, sender, e);
    }
    protected void GridView06_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(6, sender, e);
    }
    protected void GridView07_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView_RowCreated(7, sender, e);
    }
    //TAB8沒有GridView

    //各頁籤公用之GridView_RowCreated
    protected void GridView_RowCreated(int tabNo, object sender, GridViewRowEventArgs e)
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
                e.Row.Cells[i].Style.Add("text-align", "left");
            }

            i = e.Row.Cells.Count - 1;
            //if (i > 0)
            //{
            //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
            //    e.Row.Cells[i - 1].Style.Add("width", "100px");
            //}
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    protected void GridView02_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(2, sender, e);
    }
    protected void GridView03_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(3, sender, e);
    }
    protected void GridView04_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(4, sender, e);
    }
    protected void GridView05_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(5, sender, e);
    }
    protected void GridView06_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(6, sender, e);
    }
    protected void GridView07_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView_RowDataBound(7, sender, e);
    }

    //各頁籤公用之GridView_RowDataBound
    protected void GridView_RowDataBound(int tabNo, object sender, GridViewRowEventArgs e)
    {        
        string strValue = "", tempInsertID = "";
        GridView tempGv;
        tempGv = null;
        DropDownList tempDDL;
        ASP.usercontrol_codelist_ascx tempCL;
        string tempValue = "";
        string theFileName, savePath2;

        tempInsertID = ((HiddenField)this.Form.FindControl("hid_IsInsertExit0" + tabNo.ToString())).ClientID;
        tempGv = ((GridView)this.Form.FindControl("GridView0" + tabNo.ToString()));
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                lbl_TabMsg.Text = "";
                #region 修改用
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?');");
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

                //需要變更格式
                for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i + 1].Style.Add("text-align", "center");

                    if (
                        ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                        || ((tabNo == 2) && (i == 6))
                        || ((tabNo == 4) && (i == 1))
                        || (tabNo == 5)
                        || ((tabNo == 7) && ((i == 2) || (i == 3) || (i == 7)))
                        )
                    {
                        #region 下拉單欄位
                        if ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                        {//年月下拉單
                            strValue = (i == 4) ? "BeginYM" : "EndYM";
                            ASP.usercontrol_salaryym_ascx tempYM = (ASP.usercontrol_salaryym_ascx)e.Row.Cells[i + 1].FindControl("ddlYM" + strValue);

                            if (tempYM != null)
                            {
                                tempYM.SetOtherYMList(DateTime.Today.Year - 50, DateTime.Today.Year, DateTime.Today.Year.ToString());
                                tempYM.SelectSalaryYM = DataBinder.Eval(e.Row.DataItem, strValue).ToString();
                            }                            
                        }
                        else if ((tabNo == 7) && (i == 2))
                        {
                            strValue = "Suspends";
                            tempDDL = (DropDownList)e.Row.Cells[i + 1].FindControl("ddl" + strValue);
                            tempDDL.SelectedValue = DataBinder.Eval(e.Row.DataItem, strValue).ToString();
                        }
                        else
                        {
                            if (tabNo == 5)
                            {
                                switch (i)
                                {
                                    case 1:
                                        strValue = "Language";
                                        break;
                                    case 2:
                                        strValue = "Listen";
                                        break;
                                    case 3:
                                        strValue = "Speak";
                                        break;
                                    case 4:
                                        strValue = "Read";
                                        break;
                                    case 5:
                                        strValue = "Write";
                                        break;
                                }
                            }
                            else if ((tabNo == 2) && (i == 6))
                            {
                                strValue = "Comp";
                            }
                            else if ((tabNo == 4) && (i == 1))
                            {
                                strValue = "Category";
                            }
                            else if (i == 3)
                            {
                                strValue = "Subsidy_code";
                            }
                            else
                            {
                                strValue = "Dependent_title";
                            }
                            tempCL = (ASP.usercontrol_codelist_ascx)e.Row.Cells[i + 1].FindControl("CodeList" + strValue);
                            if ((tabNo == 5) && (i > 1))
                                tempCL.SetCodeList("PY#Level");
                            else
                                tempCL.SetCodeList("PY#" + ((strValue.Length > 7) ? strValue.Remove(7) : strValue));
                            tempCL.SelectedCode = DataBinder.Eval(e.Row.DataItem, strValue).ToString().Trim();
                        }
                        #endregion
                    }
                    else if ((tabNo == 4) && (i == 4))
                    {//需要加上傳開窗的按鈕
                        try
                        {
                            TextBox tb = (TextBox)e.Row.Cells[i + 1].Controls[0];
                            tb.Style.Add("width", "80px");

                            if (tb != null)
                            {
                                ImageButton ibOpen = new ImageButton();
                                ibOpen.ID = "ibOpenFile04" + i.ToString();
                                ibOpen.SkinID = "OpenFile";

                                theFileName = "skill_" + sCompany + "_" + sEmployeeId + "_";
                                savePath2 = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("skill") + "\\";

                                ibOpen.OnClientClick = "return UploadTo(" + tb.ClientID + ",'N','skill','" + _UserInfo.SysSet.rtnHash(savePath2).Replace('\\', '＼') + "','" + theFileName + "');";

                                e.Row.Cells[i + 1].Controls.Add(ibOpen);
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (((tabNo == 6) && ((i == 1) || (i == 2)))
                        || ((tabNo == 7) && (i == 6 || i == 8)))
                        {//日期欄位
                            if ((tabNo == 6) && (i == 1))
                            {//不可修改
                                tempValue = _UserInfo.SysSet.FormatDate(e.Row.Cells[i + 1].Text);
                                if (tempValue.Contains(" Date"))
                                    tempValue = e.Row.Cells[i + 1].Text;
                                e.Row.Cells[i + 1].Text = tempValue;
                            }
                            else
                            {//可修改
                                TextBox tempTB = (TextBox)e.Row.Cells[i + 1].Controls[0];
                                if (tempTB != null)
                                {
                                    tempTB.Style.Add("width", "65px");
                                    tempTB.Style.Add("text-align", "right");

                                    tempValue = _UserInfo.SysSet.FormatDate(tempTB.Text);
                                    if (tempValue.Contains(" Date"))
                                        tempValue = tempTB.Text;
                                    tempTB.Text = tempValue;

                                    //為日期欄位增加小日曆元件                                    
                                    tempTB.CssClass = "JQCalendar";
                                    //ImageButton btOpenCal = new ImageButton();
                                    //btOpenCal.ID = "btOpenCal0" + (i + 1).ToString();
                                    //btOpenCal.SkinID = "Calendar1";
                                    //if ((tabNo == 7) && (i == 6))
                                    //{//生日,年份選單較長
                                    //    btOpenCal.OnClientClick = "return GetPromptTheDate(" + tempTB.ClientID + ",'1','" + (DateTime.Today.Year - 1911).ToString() + "');";
                                    //}
                                    //else
                                    //{//一般日期
                                    //    btOpenCal.OnClientClick = "return GetPromptDate(" + tempTB.ClientID + ");";
                                    //}
                                    //e.Row.Cells[i + 1].Controls.Add(btOpenCal);

                                    //e.Row.Cells[i + 1].Style.Add("text-align", "right");                                    
                                }
                            }
                        }
                        else if (
                            ((tabNo == 2 || tabNo == 3) && (i < 4))                            
                            || (tabNo == 4) && (i == 2 || i == 3)
                            || ((tabNo == 6) && (i >= 3))
                            || ((tabNo == 7) && (i == 4 || i == 5))
                            )
                        {
                            try
                            {
                                TextBox tempTB = (TextBox)e.Row.Cells[i + 1].Controls[0];
                                if (tempTB != null)
                                {
                                    if ((tabNo == 6) && (i == 3))
                                    {
                                        tempTB.MaxLength = 40;
                                        tempTB.Style.Add("width", "300px");
                                    }
                                    else if ((tabNo == 4) && (i == 2))
                                    {//專長項目(可能為証照檢定名稱+號碼)
                                        tempTB.MaxLength = 50;
                                        tempTB.Style.Add("width", "300px");
                                    }
                                    else if ((tabNo == 2 || tabNo == 3) && (i < 4))
                                    {//學經歷
                                        if (i == 1)
                                        {
                                            tempTB.MaxLength = 3;
                                            tempTB.Style.Add("text-align", "right");
                                            tempTB.Style.Add("width", "20px");
                                        }
                                        else
                                        {
                                            tempTB.MaxLength = 20;
                                            tempTB.Style.Add("width", "100px");
                                        }
                                    }
                                    else
                                    {
                                        tempTB.Style.Add("width", "80px");
                                        if (tabNo == 6)
                                        {
                                            tempTB.Style.Add("text-align", "right");
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 查詢用
                //e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");                   
                }
                                
                for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                {
                    strValue = e.Row.Cells[i + 1].Text.Trim().ToUpper();

                    if ((((tabNo == 2) || (tabNo == 3)) && ((i == 4) || (i == 5)))
                        || ((tabNo == 6) && ((i == 4) || (i == 5)))
                        || ((tabNo == 7) && (i == 1)))
                    {//應向右對齊的欄位:數字或年月,不含日期(日期在格式化那區做了)
                        e.Row.Cells[i + 1].Style.Add("text-align", "right");
                    }

                    if (
                        ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                        || ((tabNo == 2) && (i == 6))
                        || ((tabNo == 4) && (i == 1))
                        || (tabNo == 5)
                        || ((tabNo == 7) && ((i == 2) || (i == 3) || (i == 7)))
                        )
                    {       
                        #region 下拉單欄位
                        //strValue = ((i == 2) ? "Suspends" : "Subsidy_code");
                        if ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                        {//年月下拉單
                            strValue = (i == 4) ? "BeginYM" : "EndYM";
                            ASP.usercontrol_salaryym_ascx tempYM = (ASP.usercontrol_salaryym_ascx)e.Row.Cells[i + 1].FindControl("ddlYM" + strValue);

                            if (tempYM != null)
                            {
                                tempYM.SetOtherYMList(DateTime.Today.Year - 50, DateTime.Today.Year, DateTime.Today.Year.ToString());
                                tempYM.SelectSalaryYM = DataBinder.Eval(e.Row.DataItem, strValue).ToString();
                            }
                            tempYM.Enabled = false;
                        }
                        else if ((tabNo == 7) && (i == 2))
                        {
                            strValue = "Suspends";
                            tempDDL = (DropDownList)e.Row.Cells[i + 1].FindControl("ddl" + strValue);
                            tempDDL.SelectedValue = DataBinder.Eval(e.Row.DataItem, strValue).ToString();
                            //查詢時不可修改
                            e.Row.Cells[i + 1].Text = tempDDL.SelectedItem.Text;
                        }
                        else
                        {
                            if (tabNo == 5)
                            {
                                switch(i)
                                {
                                    case 1:
                                        strValue = "Language";
                                        break;
                                    case 2:
                                        strValue = "Listen";
                                        break;
                                    case 3:
                                        strValue = "Speak";
                                        break;
                                    case 4:
                                        strValue = "Read";
                                        break;
                                    case 5:
                                        strValue = "Write";
                                        break;
                                } 
                            }
                            else if ((tabNo == 2) && (i == 6))
                            {
                                strValue = "Comp";
                            }
                            else if ((tabNo == 4) && (i == 1))
                            {
                                strValue = "Category";
                            }
                            else if (i == 3)
                            {
                                strValue = "Subsidy_code";
                            }
                            else
                            {
                                strValue = "Dependent_title";
                            }
                            tempCL = (ASP.usercontrol_codelist_ascx)e.Row.Cells[i + 1].FindControl("CodeList" + strValue);
                            if ((tabNo == 5) && (i > 1))
                                tempCL.SetCodeList("PY#Level");
                            else
                                tempCL.SetCodeList("PY#" + ((strValue.Length > 7) ? strValue.Remove(7) : strValue));

                            try
                            {
                                tempCL.SelectedCode = DataBinder.Eval(e.Row.DataItem, strValue).ToString().Trim();
                                //查詢時不可修改
                                e.Row.Cells[i + 1].Text = tempCL.SelectedCodeName;
                            }
                            catch {
                                if ((tabNo == 5) && (i > 1))
                                    lbl_TabMsg.Text = "尚未設定語言能力代碼!請與系統管理員聯繫!!";
                                else if ((tabNo == 5) && (i == 1))
                                    lbl_TabMsg.Text = "尚未設定語言代碼!請與系統管理員聯繫!!";
                                else
                                    lbl_TabMsg.Text = "尚未設定" + ((i == 3) ? "補助碼" : "眷屬稱謂代碼") + "!請與系統管理員聯繫!!";
                            }
                        }

                        e.Row.Cells[i + 1].Style.Add("text-align", "center");
                        #endregion
                    }
                    else if ((tabNo == 4) && (i == 4))
                    {//增加附檔連結
                        e.Row.Cells[i + 1].Style.Add("text-align", "center");
                        if (DataBinder.Eval(e.Row.DataItem, "filepath").ToString().Trim().Length > 0)
                        {
                            try
                            {
                                HyperLink hlOpen = new HyperLink();
                                tempValue = _UserInfo.SysSet.GetConfigString("skill") + "/" + "skill_" + sCompany + "_" + sEmployeeId + "_";
                                hlOpen.ID = "hlOpen";
                                hlOpen.SkinID = "OpenFile";
                                hlOpen.NavigateUrl = tempValue + DataBinder.Eval(e.Row.DataItem, "filepath").ToString().Trim();
                                hlOpen.Target = "_blank";

                                e.Row.Cells[i + 1].Controls.Add(hlOpen);
                            }
                            catch { }
                        }
                        else
                        {
                            e.Row.Cells[i + 1].Text = "無附件";
                        }
                    }
                    else
                    {//日期欄位
                        if (((tabNo == 6) && (i == 1 || i == 2))
                            || ((tabNo == 7) && (i == 6 || i == 8)))
                        {//日期欄位需要變更格式 
                            tempValue = _UserInfo.SysSet.FormatDate(e.Row.Cells[i + 1].Text);
                            if (tempValue.Contains(" Date"))
                                tempValue = e.Row.Cells[i + 1].Text;
                            e.Row.Cells[i + 1].Text = tempValue;
                            e.Row.Cells[i + 1].Style.Add("text-align", "right");
                        }
                    }
                }
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {            
            #region 新增用欄位
            strValue = "";

            for (int i = 1; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i + 1].Style.Add("text-align", "center");

                if (
                    ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                    || ((tabNo == 2) && (i == 6))
                    || ((tabNo == 4) && (i == 1))
                    || ((tabNo == 5) && ((i > 0) && (i < 6))) 
                    || ((tabNo == 7) && ((i == 2) || (i == 3) || (i == 7)))
                    )
                {//非文字填寫欄位
                    if ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                    {//年月下拉單
                        ASP.usercontrol_salaryym_ascx tempYM = new ASP.usercontrol_salaryym_ascx();
                        tempYM = (ASP.usercontrol_salaryym_ascx)LoadControl("~/UserControl/SalaryYM.ascx");
                        tempYM.ID = "tbAddNew" + i.ToString().PadLeft(2, '0');

                        tempYM.SetOtherYMList(DateTime.Today.Year - 50, DateTime.Today.Year, DateTime.Today.Year.ToString());

                        e.Row.Cells[i + 1].Controls.Add(tempYM);
                    }
                    else if ((tabNo == 7) && (i == 2))
                    {
                        DropDownList ddlAddNew = new DropDownList();
                        ddlAddNew.ID = "tbAddNew" + i.ToString().PadLeft(2, '0');

                        ListItem addLI = new ListItem();

                        addLI.Text = "否";
                        addLI.Value = "N";
                        ddlAddNew.Items.Add(addLI);
                        addLI = new ListItem();
                        addLI.Text = "是";
                        addLI.Value = "Y";
                        ddlAddNew.Items.Add(addLI);

                        e.Row.Cells[i + 1].Controls.Add(ddlAddNew);
                    }
                    else
                    {//代碼下拉單
                        ASP.usercontrol_codelist_ascx ddlAddNew = new ASP.usercontrol_codelist_ascx();

                        ddlAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                        ddlAddNew.ID = "tbAddNew" + i.ToString().PadLeft(2, '0');

                        if ((tabNo == 5) && ((i > 0) && (i < 6)))
                        {
                            if (i == 1)
                            {
                                ddlAddNew.SetCodeList("PY#Languag");
                                //ddlAddNew.SelectedCode = "1";
                            }
                            else
                            {
                                ddlAddNew.SetCodeList("PY#Level");
                                ddlAddNew.SelectedCode = "1";
                            }
                        }
                        else if ((tabNo == 2) && (i == 6))
                        {
                            ddlAddNew.SetCodeList("PY#Comp");
                        }
                        else if ((tabNo == 4) && (i == 1))
                        {
                            ddlAddNew.SetCodeList("PY#Categor");
                        }
                        else if (i == 3)
                        {
                            ddlAddNew.SetCodeList("PY#Subsidy");
                            ddlAddNew.SelectedCode = "4";
                        }
                        else
                        {
                            ddlAddNew.SetCodeList("PY#Depende");
                            //ddlAddNew.SelectedCode = "1";
                        }
                        e.Row.Cells[i + 1].Controls.Add(ddlAddNew);
                    }                    
                }
                else
                {//文字填寫欄位
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + i.ToString().PadLeft(2, '0');

                    if ((tabNo == 7) && (i == 1))
                    {
                        tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Style.Add("width", "20px");
                    }
                    else if (((tabNo == 6) && (i == 1 || i == 2)) || ((tabNo == 7) && (i == 6 || i == 8)))
                    {//日期
                        tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Style.Add("width", "65px");
                    }
                    else if ((tabNo == 6) && (i == 3))
                    {
                        tbAddNew.MaxLength = 40;
                        tbAddNew.Style.Add("width", "300px");
                    }
                    else if ((tabNo == 4) && (i == 2))
                    {//專長項目(可能為証照檢定名稱+號碼)
                        tbAddNew.MaxLength = 50;
                        tbAddNew.Style.Add("width", "300px");
                    }
                    else if ((tabNo == 2 || tabNo == 3) && (i < 4))
                    {//學經歷
                        if (i == 1)
                        {
                            tbAddNew.MaxLength = 3;
                            tbAddNew.Style.Add("text-align", "right");
                            tbAddNew.Style.Add("width", "20px");
                        }
                        else
                        {
                            tbAddNew.MaxLength = 20;
                            tbAddNew.Style.Add("width", "100px");
                        }
                    }
                    else 
                    {//一般
                        tbAddNew.Style.Add("width", "80px");
                    }

                    e.Row.Cells[i + 1].Controls.Add(tbAddNew);

                    //檢核欄位不得為空(下列為可為空之欄位)
                    if (!(
                        ((tabNo == 2) && (i == 3))
                        || ((tabNo == 3) && (i == 6))
                        || ((tabNo == 4) && (i == 3 || i == 4))
                        || ((tabNo == 6) && (i == 4 || i == 5))
                        ))
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

                    if ((tabNo == 4) && (i == 4))
                    {//需要加上傳開窗的欄位
                        ImageButton ibOpen = new ImageButton();
                        ibOpen.ID = "ibOpenFile04" + i.ToString();
                        ibOpen.SkinID = "OpenFile";

                        theFileName = "skill_" + sCompany + "_" + sEmployeeId + "_";
                        savePath2 = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("skill") + "\\";

                        ibOpen.OnClientClick = "return UploadTo(" + tbAddNew.ClientID + ",'N','skill','" + _UserInfo.SysSet.rtnHash(savePath2).Replace('\\', '＼') + "','" + theFileName + "');";
                                     
                        e.Row.Cells[i + 1].Controls.Add(ibOpen);
                    }
                    else if (((tabNo == 6) && (i == 1 || i == 2)) || ((tabNo == 7) && (i == 6 || i == 8)))
                    {//需要加日期開窗的欄位
                        strValue += "checkDate(" + tbAddNew.ClientID + ") && ";

                        //為日期欄位增加小日曆元件
                        tbAddNew.CssClass = "JQCalendar";
                        //ImageButton btOpenCal = new ImageButton();
                        //btOpenCal.ID = "btOpenCal0" + i.ToString();
                        //btOpenCal.SkinID = "Calendar1";

                        //if ((tabNo == 7) && (i == 6))
                        //{//生日,年份選單較長
                        //    btOpenCal.OnClientClick = "return GetPromptTheDate(" + tbAddNew.ClientID + ",'1','" + (DateTime.Today.Year - 1911).ToString() + "');";
                        //}
                        //else
                        //{//一般日期
                        //    btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
                        //}
                        //e.Row.Cells[i + 1].Controls.Add(btOpenCal);
                    }
                }
            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + tempInsertID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[1].Controls.Add(btAddNew); 
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {            
            //權限
            e.Row.Visible = tempGv.ShowFooter;
            #region 新增用欄位

            strValue = "";
            int TabColCount = Convert.ToInt32(getTabKey("TabColCount", tabNo));
            
            for (int i = 1; i <= TabColCount; i++)
            {
                if (
                    ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                    || ((tabNo == 2) && (i == 6))
                    || ((tabNo == 4) && (i == 1))
                    || ((tabNo == 5) && ((i > 0) && (i < 6)))
                    || ((tabNo == 7) && ((i == 2) || (i == 3) || (i == 7)))
                    )
                {
                    //非文字填寫欄位
                    if ((tabNo == 2 || tabNo == 3) && (i == 4 || i == 5))
                    {//年月下拉單
                        ASP.usercontrol_salaryym_ascx tempYM = (ASP.usercontrol_salaryym_ascx)e.Row.FindControl("tbAddNew0" + i.ToString());
                        tempYM.SetOtherYMList(DateTime.Today.Year - 50, DateTime.Today.Year, DateTime.Today.Year.ToString());
                    }
                    else if (!((tabNo == 7) && (i == 2)))
                    {//代碼下拉單
                        //初始化下拉單
                        ASP.usercontrol_codelist_ascx tempAddNewCL = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew0" + i.ToString());

                        if (tabNo == 5)
                        {
                            if (i == 1)
                            {
                                tempAddNewCL.SetCodeList("PY#Languag");
                                //tempAddNewCL.SelectedCode = "1";
                            }
                            else
                            {
                                tempAddNewCL.SetCodeList("PY#Level");
                                tempAddNewCL.SelectedCode = "1";
                            }
                        }
                        else if ((tabNo == 2) && (i == 6))
                        {
                            tempAddNewCL.SetCodeList("PY#Comp");
                        }
                        else if ((tabNo == 4) && (i == 1))
                        {
                            tempAddNewCL.SetCodeList("PY#Categor");
                        }
                        else if (i == 3)
                        {
                            tempAddNewCL.SetCodeList("PY#Subsidy");
                            tempAddNewCL.SelectedCode = "4";
                        }
                        else
                        {
                            tempAddNewCL.SetCodeList("PY#Depende");
                            //tempAddNewCL.SelectedCode = "1";
                        }
                    }
                }
                else
                {//文字填寫欄位
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                    //if (i == 2)
                    //{
                    //    tbAddNew.Style.Add("text-align", "right");
                    //    tbAddNew.Style.Add("width", "70px");
                    //}

                    //檢核欄位不得為空(下列為可為空之欄位)
                    if (!(
                        ((tabNo == 2) && (i == 3))
                        || ((tabNo == 3) && (i == 6))
                        || ((tabNo == 4) && (i == 3 || i == 4))
                        || ((tabNo == 6) && (i == 4 || i == 5))
                        ))
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                    
                    if ((tabNo == 4) && (i == 4))
                    {//需要加上傳開窗的欄位
                        ImageButton ibOpen = (ImageButton)e.Row.FindControl("ibOpenFile04" + i.ToString());
                        theFileName = "skill_" + sCompany + "_" + sEmployeeId + "_";
                        savePath2 = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("skill") + "\\";

                        //ibOpen.Attributes.Add("onclick", "return Upload('" + tbAddNew.ClientID + "','skill','" + _UserInfo.SysSet.rtnHash(savePath2).Replace('\\', '＼') + "','" + theFileName + "');");
                        ibOpen.OnClientClick = "return UploadTo(" + tbAddNew.ClientID + ",'N','skill','" + _UserInfo.SysSet.rtnHash(savePath2).Replace('\\', '＼') + "','" + theFileName + "');";                        
                    }
                    else if (((tabNo == 6) && (i == 1 || i == 2)) || ((tabNo == 7) && (i == 6 || i == 8)))
                    {//需要加日期開窗的欄位
                        tbAddNew.Style.Add("width", "65px");
                        tbAddNew.Style.Add("text-align", "right");

                        strValue += "checkDate(" + tbAddNew.ClientID + ") && ";

                        tbAddNew.CssClass = "JQCalendar";
                        //ImageButton btnCal = (ImageButton)e.Row.FindControl("btnCalendar0" + i.ToString());
                        //if ((tabNo == 7) && (i == 6))
                        //{//生日,年份選單較長
                        //    btnCal.Attributes.Add("onclick", "return GetPromptTheDate(" + tbAddNew.ClientID + ",'1','" + (DateTime.Today.Year - 1911).ToString() + "');");
                        //}
                        //else
                        //{//一般日期
                        //    btnCal.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
                        //}
                    }
                    else if ((tabNo == 6) && (i == 3))
                    {//訓練課程
                        tbAddNew.MaxLength = 40;
                        tbAddNew.Style.Add("width", "300px");
                    }
                    else if ((tabNo == 4) && (i == 2))
                    {//專長項目(可能為証照檢定名稱+號碼)
                        tbAddNew.MaxLength = 50;
                        tbAddNew.Style.Add("width", "300px");
                    }
                    else if ((tabNo == 2 || tabNo == 3) && (i < 4))
                    {//學經歷
                        if (i == 1)
                        {
                            tbAddNew.MaxLength = 3;
                            tbAddNew.Style.Add("text-align", "right");
                            tbAddNew.Style.Add("width", "20px");
                        }
                        else
                        {
                            tbAddNew.MaxLength = 20;
                            tbAddNew.Style.Add("width", "100px");
                        }
                    }
                    else
                    {
                        tbAddNew.Style.Add("width", "80px");
                    }                    
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + tempInsertID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

    //各頁籤公用之刪除函式
    protected void doDelete(int tabNo, object sender)
    {
        ASP.usercontrol_navigator_gv_ascx tempNg;
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString().Trim();
        string sql = "", sqlQuery = "";
        tempNg = null;

        Ssql = getTabKey("TableName", tabNo);
        sql = getTabKey("KeyColumn", tabNo);
        tempNg = ((ASP.usercontrol_navigator_gv_ascx)this.Form.FindControl("Navigator0" + tabNo.ToString()));

        //須做處理的KEY值
        switch (tabNo)
        {
            case 6:
                L1PK = Convert.ToDateTime(L1PK).ToString("yyyy/MM/dd");
                break;
        }

        #region 開始異動前,先寫入LOG
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = Ssql.Trim();
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = sql + "='" + L1PK + "'";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion               

        sqlQuery = " From " + Ssql + " Where Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "' And " + sql + "='" + L1PK + "'";
        sql = "Delete " + sqlQuery;
        sqlQuery = "select IsNull(filepath,'') filepath " + sqlQuery;

        int result = 0;
        try
        {
            if (tabNo == 4)
            {
                DataTable tempDT = _MyDBM.ExecuteDataTable(sqlQuery);
                sqlQuery = "";
                if (tempDT != null)
                {
                    if (tempDT.Rows.Count > 0)
                    {
                        sqlQuery = tempDT.Rows[0][0].ToString();
                    }
                }
            }
            
            result = _MyDBM.ExecuteCommand(sql.ToString());

            if (!string.IsNullOrEmpty(sqlQuery))
            {//連同紀錄中的附檔一併刪除
                sqlQuery = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + "Main\\" + _UserInfo.SysSet.GetConfigString("skill") + "\\skill_" + sCompany + "_" + sEmployeeId + "_" + sqlQuery;
                if (System.IO.File.Exists(sqlQuery))
                {
                    System.IO.File.Delete(sqlQuery);
                }
            }
        }
        catch (Exception ex)
        {
            lbl_TabMsg.Text = ex.Message;
        }

        if (result > 0)
        {
            lbl_TabMsg.Text = "資料刪除成功 !!";
            tempNg.DataBind();
        }
        else
        {
            lbl_TabMsg.Text = "資料刪除失敗 !!" + lbl_TabMsg.Text;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_TabMsg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData(tabNo);        
    }
    //各頁籤綁定資料
    private void BindData(int tabNo)
    {
        Ssql = "";
        SqlDataSource tempSDS;
        GridView tempGv;
        ASP.usercontrol_navigator_gv_ascx tempNg;
                
        tempSDS = null;
        tempGv = null;
        tempNg = null;

        Ssql = getTabKey("TableName", tabNo);

        if (tabNo > 1)
        {            
            tempSDS = ((SqlDataSource)this.Form.FindControl("SDS_GridView0" + tabNo.ToString()));
            tempGv = ((GridView)this.Form.FindControl("GridView0" + tabNo.ToString()));
            tempNg = ((ASP.usercontrol_navigator_gv_ascx)this.Form.FindControl("Navigator0" + tabNo.ToString()));
        }
        else
        {
            tempSDS = SqlDataSource1;
        }

        if (tabNo == 80)
        {
            string strWhere = string.Format(" And (Company = '{0}' And EmployeeId = '{1}') ", sCompany, sEmployeeId);

            Ssql = "SELECT Company,EmployeeId,CompanyCode,PayRollPW,ErrFrequency" +
                " ,(select Top 1 ERPID from " + Ssql + " a where a.CompanyCode != 'PAN-PACIFIC' " + strWhere + " ) as ERPID " +
                " ,(select Top 1 ERPID from " + Ssql + " a where a.CompanyCode = 'PAN-PACIFIC' " + strWhere + " ) as ADAcc " +
                " FROM " + Ssql + " Where 0=0 " + strWhere;
        }
        else
        {
            Ssql = "SELECT * FROM " + Ssql + " Where 0=0";
            Ssql += string.Format(" And (Company = '{0}' And EmployeeId = '{1}' ) ", sCompany, sEmployeeId);
        }
        tempSDS.SelectCommand = Ssql;

        if (tabNo > 1)
        {
            if (tabNo != 8)
            {
                //tempSDS.SelectCommand = Ssql;
                tempGv.DataBind();
                if (tempGv.PageCount > 1)
                    tempNg.Visible = true;
                else
                    tempNg.Visible = false;
                tempNg.BindGridView = tempGv;
                tempNg.DataBind();
            }

            if ((tabNo == 7) || (tabNo == 8))
            {
                DetailsView theDV = ((DetailsView)this.Form.FindControl("DetailsView" + tabNo.ToString().PadLeft(2, '0')));
                if (theDV != null)
                {
                    theDV.DefaultMode = DetailsView1.CurrentMode;
                    theDV.DataBind();

                    if ((theDV.Rows.Count == 0) && theDV.DefaultMode != DetailsViewMode.ReadOnly)
                    {
                        theDV.ChangeMode(DetailsViewMode.Insert);
                    }
                }
            }
        }
        else
        {
            //tempSDS.SelectCommand = Ssql;
            DetailsView01.DataBind();
            //DetailsView01
        }
    }

    //各頁籤公用之新增函式
    protected void btnEmptyNew_Click(int tabNo, object sender, EventArgs e)
    {
        string temId = "";
        bool blError = true;
        SqlDataSource tempSDS;
        GridView tempGv;
        ASP.usercontrol_navigator_gv_ascx tempNg;

        lbl_TabMsg.Text = "";

        Ssql = getTabKey("TableName", tabNo);
        tempSDS = ((SqlDataSource)this.Form.FindControl("SDS_GridView0" + tabNo.ToString()));
        tempGv = ((GridView)this.Form.FindControl("GridView0" + tabNo.ToString()));
        tempNg = ((ASP.usercontrol_navigator_gv_ascx)this.Form.FindControl("Navigator0" + tabNo.ToString()));
        //if ((tabNo == 5) && (Request.Form["hid_IsInsertExit0" + tabNo.ToString()] != null))
        //    temId = Request.Form["hid_IsInsertExit0" + tabNo.ToString()].ToString();
        //else
            temId = ((HiddenField)this.Form.FindControl("hid_IsInsertExit0" + tabNo.ToString())).Value;
        temId = temId.Replace("_", "$") + "$tbAddNew0";

        if (temId.Equals("$tbAddNew0"))
            return;
        else if (!((tabNo == 4) || (tabNo == 5)))
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()))
            {
                //|| String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "4"].ToString())
                //|| String.IsNullOrEmpty(Request.Form[temId + "5"].ToString())
                return;
            }
        }

        if (tabNo == 7)
        {//健保眷屬,除序號外應檢核眷屬之身份証字號不可重覆
            blError = ValidateData(0 - tabNo, Ssql.Trim(), Request.Form[temId + "5"].ToString());
        }

        //新增
        string theKey = ((!((tabNo == 4) || (tabNo == 5))) ? Request.Form[temId + "1"].ToString() : Request.Form[temId + "1$ddlCodeList"].ToString());

        if ((blError == true) && ValidateData(tabNo, Ssql.Trim(), theKey))
        {
            tempSDS.InsertParameters.Clear();
            if (tabNo < 6 && !(tabNo == 2 || tabNo == 3))
                tempSDS.InsertParameters.Add("SeqNo", (tempGv.Rows.Count + 1).ToString());
            tempSDS.InsertParameters.Add("Company", sCompany);
            tempSDS.InsertParameters.Add("EmployeeId", sEmployeeId);
            //For代碼下拉單
            string strTemp = "";

            switch (tabNo)
            {
                case 2:
                    tempSDS.InsertParameters.Add("SeqNo", Request.Form[temId + "1"].ToString());
                    tempSDS.InsertParameters.Add("School1", Request.Form[temId + "2"].ToString());
                    tempSDS.InsertParameters.Add("Department", Request.Form[temId + "3"].ToString());
                    tempSDS.InsertParameters.Add("BeginYM", Request.Form[temId + "4$YearList1$ddlYear"].ToString() + Request.Form[temId + "4$MonthList1$MonthList"].ToString());
                    tempSDS.InsertParameters.Add("EndYM", Request.Form[temId + "5$YearList1$ddlYear"].ToString() + Request.Form[temId + "5$MonthList1$MonthList"].ToString());
                    if (Request.Form[temId + "6$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "6$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "6"].ToString();
                    tempSDS.InsertParameters.Add("Comp", strTemp);
                    break;
                case 3:
                    tempSDS.InsertParameters.Add("SeqNo", Request.Form[temId + "1"].ToString());
                    tempSDS.InsertParameters.Add("Organization", Request.Form[temId + "2"].ToString());
                    tempSDS.InsertParameters.Add("Position", Request.Form[temId + "3"].ToString());
                    tempSDS.InsertParameters.Add("BeginYM", Request.Form[temId + "4$YearList1$ddlYear"].ToString() + Request.Form[temId + "4$MonthList1$MonthList"].ToString());
                    tempSDS.InsertParameters.Add("EndYM", Request.Form[temId + "5$YearList1$ddlYear"].ToString() + Request.Form[temId + "5$MonthList1$MonthList"].ToString());
                    tempSDS.InsertParameters.Add("Memo", Request.Form[temId + "6"].ToString());
                    break;
                case 4:                    
                    if (Request.Form[temId + "1$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "1$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "1"].ToString();
                    tempSDS.InsertParameters.Add("Category", strTemp);

                    tempSDS.InsertParameters.Add("Item", Request.Form[temId + "2"].ToString());
                    tempSDS.InsertParameters.Add("Memo", Request.Form[temId + "3"].ToString());
                    tempSDS.InsertParameters.Add("filepath", Request.Form[temId + "4"].ToString());
                    break;
                case 5:
                    if (Request.Form[temId + "1$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "1$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "1"].ToString();
                    blError = (!string.IsNullOrEmpty(strTemp));
                    tempSDS.InsertParameters.Add("Language", strTemp);

                    if (Request.Form[temId + "2$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "2$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "2"].ToString();
                    blError = (blError && (!string.IsNullOrEmpty(strTemp)));
                    tempSDS.InsertParameters.Add("Listen", strTemp);

                    if (Request.Form[temId + "3$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "3$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "3"].ToString();
                    blError = (blError && (!string.IsNullOrEmpty(strTemp)));
                    tempSDS.InsertParameters.Add("Speak", strTemp);

                    if (Request.Form[temId + "4$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "4$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "4"].ToString();
                    blError = (blError && (!string.IsNullOrEmpty(strTemp)));
                    tempSDS.InsertParameters.Add("Read", strTemp);

                    if (Request.Form[temId + "5$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "5$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "5"].ToString();
                    blError = (blError && (!string.IsNullOrEmpty(strTemp)));
                    tempSDS.InsertParameters.Add("Write", strTemp);                    
                    break;
                case 6:
                    tempSDS.InsertParameters.Add("Training_datefrom", _UserInfo.SysSet.FormatADDate(Request.Form[temId + "1"].ToString()));
                    tempSDS.InsertParameters.Add("Training_dateto", _UserInfo.SysSet.FormatADDate(Request.Form[temId + "2"].ToString()));
                    strTemp = Request.Form[temId + "3"].ToString().Trim();
                    if (strTemp.Length > 40)
                        strTemp = strTemp.Remove(40);
                    tempSDS.InsertParameters.Add("Training_courses", Request.Form[temId + "3"].ToString());
                    tempSDS.InsertParameters.Add("Hours", Request.Form[temId + "4"].ToString());
                    tempSDS.InsertParameters.Add("Amount", Request.Form[temId + "5"].ToString());
                    break;
                case 7:
                    tempSDS.InsertParameters.Add("SeqNo", Request.Form[temId + "1"].ToString());
                    tempSDS.InsertParameters.Add("Suspends", Request.Form[temId + "2"].ToString());
                    //補助碼
                    if (Request.Form[temId + "3$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "3$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "3"].ToString();
                    tempSDS.InsertParameters.Add("Subsidy_code", strTemp.ToString());
                    tempSDS.InsertParameters.Add("DependentsName", Request.Form[temId + "4"].ToString());
                    tempSDS.InsertParameters.Add("IDNo", Request.Form[temId + "5"].ToString());
                    tempSDS.InsertParameters.Add("BirthDate", _UserInfo.SysSet.FormatADDate(Request.Form[temId + "6"].ToString()));
                    //稱謂
                    if (Request.Form[temId + "7$ddlCodeList"] != null)
                        strTemp = Request.Form[temId + "7$ddlCodeList"].ToString();
                    else
                        strTemp = Request.Form[temId + "7"].ToString();
                    tempSDS.InsertParameters.Add("Dependent_title", strTemp.ToString());
                    tempSDS.InsertParameters.Add("EffectiveDate", _UserInfo.SysSet.FormatADDate(Request.Form[temId + "8"].ToString()));
                    break;
            }

            if (blError == true)
            {
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
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = Ssql.Trim() + "_" + tabNo;
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
                MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                //此時不設定異動結束時間
                //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion

                int i = 0;
                try
                {
                    i = tempSDS.Insert();
                }
                catch (Exception ex)
                {
                    lbl_TabMsg.Text = ex.Message;
                }

                if (i == 1)
                {
                    lbl_TabMsg.Text = i.ToString() + " 個資料列 " + "新增成功!!";
                }
                else
                {
                    lbl_TabMsg.Text = "新增失敗!!" + lbl_TabMsg.Text;
                }

                #region 完成異動後,更新LOG資訊
                MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_TabMsg.Text;
                MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                _MyDBM.DataChgLog(MyCmd.Parameters);
                #endregion
            }
            else
            {
                lbl_TabMsg.Text = "新增失敗!!請選擇資料!";
            }
        }
        else
        {
            lbl_TabMsg.Text = "新增失敗!!  原因: 資料重覆";            
        }

        for (int i = 2; i <= iTabCount; i++)
        {
            if (((HiddenField)this.Form.FindControl("hid_IsInsertExit0" + i.ToString())) != null)
                ((HiddenField)this.Form.FindControl("hid_IsInsertExit0" + i.ToString())).Value = "";
        }

        BindData(tabNo);
    }

    //各頁籤公用之檢核資料是否重覆之函式
    private bool ValidateData(int tabId, string tableName, string Key1)
    {
        //專長類不檢查
        if (tabId == 4)
            return true;

        //判斷資料是否重覆
        string keyColumn = getTabKey("KeyColumn", tabId);             

        DataTable tb = _MyDBM.ExecuteDataTable("Select * From " + tableName + " WHERE Company = '" + sCompany + "' And EmployeeId = '" + sEmployeeId + "' And " + keyColumn + " = '" + Key1 + "'");

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }

    //各頁籤共用之實作更新資料
    protected void GridView_RowUpdating(int tabNo, object sender, GridViewUpdateEventArgs e)
    {
        Ssql = "";
        GridView tempGv;
        SqlDataSource tempSDS;
        tempGv = null;

        Ssql = getTabKey("TableName", tabNo);
        tempGv = (GridView)this.Form.FindControl("GridView0" + tabNo.ToString());
        tempSDS = ((SqlDataSource)this.Form.FindControl("SDS_GridView0" + tabNo.ToString()));

        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        //下拉單要另外設定參數
        if (tabNo == 2)
        {
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["BeginYM"]);
            tempSDS.UpdateParameters.Add("BeginYM", ((ASP.usercontrol_salaryym_ascx)tempGv.Rows[e.RowIndex].FindControl("ddlYMBeginYM")).SelectSalaryYM);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["EndYM"]);
            tempSDS.UpdateParameters.Add("EndYM", ((ASP.usercontrol_salaryym_ascx)tempGv.Rows[e.RowIndex].FindControl("ddlYMEndYM")).SelectSalaryYM);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Comp"]);
            tempSDS.UpdateParameters.Add("Comp", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListComp")).SelectedCode);
        }
        else if (tabNo == 3)
        {
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["BeginYM"]);
            tempSDS.UpdateParameters.Add("BeginYM", ((ASP.usercontrol_salaryym_ascx)tempGv.Rows[e.RowIndex].FindControl("ddlYMBeginYM")).SelectSalaryYM);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["EndYM"]);
            tempSDS.UpdateParameters.Add("EndYM", ((ASP.usercontrol_salaryym_ascx)tempGv.Rows[e.RowIndex].FindControl("ddlYMEndYM")).SelectSalaryYM);
        }
        else if (tabNo == 4)
        {
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Category"]);
            tempSDS.UpdateParameters.Add("Category", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListCategory")).SelectedCode);
        }
        else if (tabNo == 5)
        {
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Language"]);
            tempSDS.UpdateParameters.Add("Language", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListLanguage")).SelectedCode);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Listen"]);
            tempSDS.UpdateParameters.Add("Listen", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListListen")).SelectedCode);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Speak"]);
            tempSDS.UpdateParameters.Add("Speak", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListSpeak")).SelectedCode);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Read"]);
            tempSDS.UpdateParameters.Add("Read", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListRead")).SelectedCode);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Write"]);
            tempSDS.UpdateParameters.Add("Write", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListWrite")).SelectedCode);
        }
        else if (tabNo == 7)
        {
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Suspends"]);
            tempSDS.UpdateParameters.Add("Suspends", ((DropDownList)tempGv.Rows[e.RowIndex].FindControl("ddlSuspends")).SelectedValue);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Subsidy_code"]);
            tempSDS.UpdateParameters.Add("Subsidy_code", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListSubsidy_code")).SelectedCode);
            tempSDS.UpdateParameters.Remove(tempSDS.UpdateParameters["Dependent_title"]);
            tempSDS.UpdateParameters.Add("Dependent_title", ((ASP.usercontrol_codelist_ascx)tempGv.Rows[e.RowIndex].FindControl("CodeListDependent_title")).SelectedCode);
        }

        for (int i = 0; i < tempGv.Columns.Count - 3; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();

                if ((tabNo == 6) && (i == 0)
                    || (tabNo == 7) && ((i == 2) || (i == 3)))
                {//只算TextBox,若中間有下拉單要扣掉
                    //將日期欄位格式為化為西元日期
                    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
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
                        UpdateItem += ((LinkButton)tempGv.HeaderRow.Cells[i + 3].Controls[0]).Text.Trim() + "|";
                        UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = Ssql.Trim() + "_" + tabNo;
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = UpdateItem;
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
    }

    //各頁籤共用之實作更新資料-完成
    protected void GridView_RowUpdated(int tabNo, object sender, GridViewUpdatedEventArgs e)
    {
        GridView tempGv;
        tempGv = null;

        Ssql = getTabKey("TableName", tabNo);
        tempGv = (GridView)this.Form.FindControl("GridView0" + tabNo.ToString());

        if (e.Exception == null)
        {
            tempGv.EditIndex = -1;
            lbl_TabMsg.Text = e.AffectedRows.ToString() + " 個資料列 " + "更新成功!!";
            BindData(tabNo);
        }
        else
        {
            lbl_TabMsg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_TabMsg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
            return;                
    }

    /// <summary>
    /// 傳回各TAB的資料表名稱或KEY欄名稱
    /// </summary>
    /// <param name="kind">TableName,KeyColumn,TableCount</param>
    /// <param name="tabNo">標籤序號</param>
    /// <returns>DB資料名稱</returns>
    private string getTabKey(string kind, int tabNo)
    {
        string strTableName = "", strColumn = "", strTableCount = "";

        switch (tabNo)
        {
            case 1:
                strTableName = " Personnel_Master ";
                strColumn = " EmployeeId ";
                strTableCount = "0";
                break;
            case 2:
                strTableName = " Education ";
                strColumn = "SeqNo";
                strTableCount = "6";
                break;
            case 3:
                strTableName = " CurriculumVitae ";
                strColumn = "SeqNo";
                strTableCount = "6";
                break;
            case 4:
                strTableName = " Skill ";
                strColumn = "SeqNo";
                strTableCount = "4";
                break;
            case 5:
                strTableName = " Language ";
                strColumn = "Language";
                strTableCount = "5";
                break;
            case 6:
                strTableName = " TrainingData ";
                strColumn = "Convert(varchar,Training_datefrom,111)";
                strTableCount = "5";
                break;
            case 7:
                strTableName = " HealthInsurance_Detail ";
                strColumn = "SeqNo";
                strTableCount = "8";
                break;
            case -7:
                strTableName = " HealthInsurance_Detail ";
                strColumn = "IDNo";
                strTableCount = "8";
                break;
           case 70:
               strTableName = " HealthInsurance_Heading ";
                strColumn = "";
                strTableCount = "";
                break;                
            case 8://Tab8不可刪
                strTableName = "";
                strColumn = "";
                strTableCount = "";
                break;
            case 80:
                strTableName = " PersonnelSecurity ";
                strColumn = "";
                strTableCount = "";
                break; 
        }

        switch (kind)
        {
            case "KeyColumn":
                return strColumn;
            case "TabColCount":
                return strTableCount;
        }

        return strTableName;
    }

    protected void btnDelete02_Click(object sender, EventArgs e)
    {
        doDelete(2, sender);
    }
    protected void btnEmptyNew02_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(2, sender, e);
    }
    protected void GridView02_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(2, sender, e);
    }
    protected void GridView02_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(2, sender, e);
    }
    protected void GridView02_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView02.EditIndex = e.NewEditIndex;
        BindData(2);
    }
    protected void GridView02_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView02.EditIndex = -1;
        BindData(2);
    }

    protected void btnDelete03_Click(object sender, EventArgs e)
    {
        doDelete(3, sender);
    }
    protected void btnEmptyNew03_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(3, sender, e);
    }
    protected void GridView03_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(3, sender, e);
    }
    protected void GridView03_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(3, sender, e);
    }
    protected void GridView03_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView03.EditIndex = e.NewEditIndex;
        BindData(3);
    }
    protected void GridView03_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView03.EditIndex = -1;
        BindData(3);
    }

    protected void btnDelete04_Click(object sender, EventArgs e)
    {
        doDelete(4, sender);
    }
    protected void btnEmptyNew04_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(4, sender, e);
    }
    protected void GridView04_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(4, sender, e);
    }
    protected void GridView04_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(4, sender, e);
    }
    protected void GridView04_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView04.EditIndex = e.NewEditIndex;
        BindData(4);
    }
    protected void GridView04_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView04.EditIndex = -1;
        BindData(4);
    }

    protected void btnDelete05_Click(object sender, EventArgs e)
    {
        doDelete(5, sender);
    }
    protected void btnEmptyNew05_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(5, sender, e);
    }
    protected void GridView05_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(5, sender, e);
    }
    protected void GridView05_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(5, sender, e);
    }
    protected void GridView05_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView05.EditIndex = e.NewEditIndex;
        BindData(5);
    }
    protected void GridView05_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView05.EditIndex = -1;
        BindData(5);
    }

    protected void btnDelete06_Click(object sender, EventArgs e)
    {
        doDelete(6, sender);
    }
    protected void btnEmptyNew06_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(6, sender, e);
    }
    protected void GridView06_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(6, sender, e);
    }
    protected void GridView06_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(6, sender, e);
    }
    protected void GridView06_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView06.EditIndex = e.NewEditIndex;
        BindData(6);
    }
    protected void GridView06_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView06.EditIndex = -1;
        BindData(6);
    }

    protected void btnDelete07_Click(object sender, EventArgs e)
    {
        doDelete(7, sender);
    }
    protected void btnEmptyNew07_Click(object sender, EventArgs e)
    {
        btnEmptyNew_Click(7, sender, e);
    }
    protected void GridView07_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView_RowUpdating(7, sender, e);
    }
    protected void GridView07_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridView_RowUpdated(7, sender, e);
    }
    protected void GridView07_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView07.EditIndex = e.NewEditIndex;
        BindData(7);
    }
    protected void GridView07_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView07.EditIndex = -1;
        BindData(7);
    }

    //==DV的部份===========================================================//    
    protected void btnSaveDV_Click(int tabNo, object sender, ImageClickEventArgs e)
    {
        DetailsView theDV = (DetailsView)this.Form.FindControl("DetailsView" + tabNo.ToString().PadLeft(2, '0'));
        switch (theDV.CurrentMode)
        {
            case DetailsViewMode.Insert:
                theDV.InsertItem(true);
                break;
            case DetailsViewMode.Edit:
                theDV.UpdateItem(true);
                break;
        }
    }
    
    protected void DetailsView_DataBound(int tabNo, object sender, EventArgs e)
    {
        DetailsView thisDv = (DetailsView)sender;
        DetailsViewRow thisDvRow = null;

        HtmlTableRow theTr = (HtmlTableRow)this.Form.FindControl("trInsert" + tabNo.ToString().PadLeft(2, '0'));
        ImageButton btnEdit = (ImageButton)this.Form.FindControl("btnEdit" + tabNo.ToString().PadLeft(2, '0'));
        ImageButton btnSave = (ImageButton)this.Form.FindControl("btnSave" + tabNo.ToString().PadLeft(2, '0'));

        theTr.Visible = true;

        if (thisDv.CurrentMode == DetailsViewMode.ReadOnly)
        {
            theTr.Visible = false;
        }
        else if (thisDv.CurrentMode == DetailsViewMode.Edit)
        {            
            btnEdit.Visible = true;
            btnSave.Visible = false;
        }
        else
        {
            btnEdit.Visible = false;
            btnSave.Visible = true;
        }        

        if (thisDv.Rows.Count > 0)
        { thisDvRow = thisDv.Rows[0]; }
        else
        {
            return;
        }

        string strTableName = getTabKey("TableName", tabNo * 10).Trim();

        Ssql = "Select Top 1 * From " + strTableName;
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string strJavascript = "", tempValue = "";

        ////查詢健保金額權限
        //bool blHIIAShow = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "HIIA");

        for (int i = 0; i < dtTitle.Columns.Count; i++)
        {//修改欄位顯示
            string tempTitle = dtTitle.Columns[i].ColumnName.Trim();
            Ssql = "Select dbo.GetColumnTitle('" + strTableName + "','" + tempTitle + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                TextBox tb = null;

                if (tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId") 
                    || tempTitle.Equals("TrxType") || tempTitle.Equals("CompanyCode")
                    )
                {
                    //不顯示的欄位
                }
                else
                {
                    //欄位標題名稱                    
                    if (thisDvRow.FindControl("lblTitle_" + tempTitle) != null)
                        ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text = DT.Rows[0][0].ToString().Trim();
                    //if ((blHIIAShow == false) && tempTitle.Equals("Insured_amount"))
                    //{
                    //    ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Visible = false;
                    //}

                    //可更新的欄位
                    tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);
                }

                try
                {
                    tempValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[tempTitle].ToString();
                }
                catch { }

                switch (thisDv.CurrentMode)
                {
                    case DetailsViewMode.Edit://修改模式                            
                    case DetailsViewMode.Insert://新增模式
                        if (tempTitle.Equals("Suspends"))
                        {//下拉單欄位:健保補助碼
                            DropDownList tempDDL = (DropDownList)thisDvRow.FindControl("ddl_" + tempTitle);
                             tempDDL.SelectedValue = (string.IsNullOrEmpty(tempValue.Trim()) ? "N" : tempValue.Trim());
                        }
                        else if (tempTitle.Equals("HI2"))
                        {//下拉單欄位:補充保費
                            DropDownList tempDDL = (DropDownList)thisDvRow.FindControl("ddl_" + tempTitle);
                            tempDDL.SelectedValue = (string.IsNullOrEmpty(tempValue.Trim()) ? "Y" : tempValue.Trim());
                        }
                        else if (tempTitle.Equals("Subsidy_code") || tempTitle.Equals("Dependent_title"))
                        {//代碼下拉單
                            ASP.usercontrol_codelist_ascx tempCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl("CodeList_" + tempTitle);
                            tempCL.SetCodeList("PY#" + ((tempTitle.Length > 7) ? tempTitle.Remove(7) : tempTitle));
                            if (tempTitle.Equals("Subsidy_code"))
                                tempCL.SelectedCode = (string.IsNullOrEmpty(tempValue.Trim()) ? "4" : tempValue.Trim());
                        }
                        else if (tb != null)
                        {
                            tb.Text = tempValue.Trim();

                            if ((blHIIAShow == false) && tempTitle.Equals("Insured_amount"))
                            {
                                tb.TextMode = TextBoxMode.Password;
                            }

                            ////指定欄位寬度
                            //tb.Style.Add("width", "100px");
                            if (tempTitle.Equals("Insured_amount"))
                            {//數字欄位需靠右對齊                                    
                                tb.Style.Add("text-align", "right");
                                //2012/05/08 改為有權限者可用
                                if (blHIIAShow == false)
                                {//無權限不顯示
                                    tb.TextMode = TextBoxMode.Password;
                                    tb.Enabled = false;
                                }
                                //解密
                                tempValue = _UserInfo.SysSet.rtnTrans(tempValue);
                              
                                //無值時預設為0,有值時加入千分號
                                if (tb.Text.Trim().Length == 0)
                                    tb.Text = "";
                                else
                                    try
                                    {
                                        tb.Text = (Convert.ToDecimal(tempValue)).ToString("N0");
                                    }
                                    catch { }
                            }
                            else if (tempTitle.Equals("Insured_person"))
                            {//數字欄位需靠右對齊                                    
                                tb.Style.Add("text-align", "right");
                                tb.Enabled = false;

                                //預設為1,本人
                                if (tb.Text.Trim().Length == 0)
                                    tb.Text = "1";
                            }
                            else if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                            {//將日期欄位格式化
                                tb.Style.Add("text-align", "right");
                                try
                                {
                                    tempValue = _UserInfo.SysSet.FormatDate(tb.Text);
                                    if (tempValue.Contains("Date"))
                                        tempValue = "";
                                }
                                catch
                                {
                                    tempValue = tb.Text;
                                }
                                tb.Text = tempValue;

                                //為日期欄位增加小日曆元件
                                tb.CssClass = "JQCalendar";
                                //ImageButton ib = (ImageButton)thisDvRow.FindControl("ibOW_" + tempTitle);
                                //if (ib != null)
                                //{
                                //    strJavascript = "return GetPromptDate(" + tb.ClientID + ");";
                                //    ib.Attributes.Add("onclick", strJavascript);
                                //}
                            }
                            else if (tempTitle.Equals("PayRollPW"))
                            {//需解密欄位                           
                                //解密
                                tempValue = _UserInfo.SysSet.rtnTrans(tempValue);
                                tb.TextMode = TextBoxMode.Password;                                
                                tb.Attributes.Add("value", tempValue);
                            }
                            else
                            {
                                //其它文字欄位預設為空預設為0
                                if (tb.Text.Trim().Length == 0)
                                    tb.Text = " ";
                            }
                        }
                        break;
                    case DetailsViewMode.ReadOnly://唯讀模式
                        //指定欄位寬度
                        Label theLabel = (Label)thisDvRow.FindControl("lbl_" + tempTitle);
                        //if (theLabel != null)
                        //    theLabel.Style.Add("width", "100px");
                        if (tempTitle.Equals("Insured_amount") || tempTitle.Equals("Insured_person"))
                        {//數字欄位需靠右對齊
                            //Label會轉成SPAN無法對齊,只能塞空白=>"&nbsp;&nbsp;&nbsp;&nbsp;",所以改為在介面設定
                            theLabel.Style.Add("text-align", "right");                                                       

                            if ((blHIIAShow == false) && tempTitle.Equals("Insured_amount"))
                            {//無權限時預設為***,有值時加入千分號
                                tempValue = "***";
                            }
                            else
                            {
                                if (tempValue.Length > 0)
                                {
                                    int iTemp = 0;
                                    //解密
                                    if (tempTitle.Equals("Insured_amount") && !(int.TryParse(tempValue, out iTemp)))
                                        tempValue = _UserInfo.SysSet.rtnTrans(tempValue);
                                    try
                                    {
                                        tempValue = (Convert.ToDecimal(tempValue)).ToString("N0");
                                    }
                                    catch { }
                                }
                            }

                        }
                        else if (tempTitle.Equals("Suspends"))
                        {//下拉單欄位
                            switch (tempValue)
                            {
                                case "N":
                                    tempValue = "投保";
                                    break;
                                case "Y":
                                    tempValue = "退保";
                                    break;
                                case "U":
                                    tempValue = "調整";
                                    break;
                            }
                        }
                        else if (tempTitle.Equals("HI2"))
                        {//下拉單欄位
                            switch (tempValue)
                            {
                                case "N":
                                    tempValue = "否";
                                    break;
                                case "Y":
                                    tempValue = "是";
                                    break;
                            }
                        }
                        else if (tempTitle.Equals("Subsidy_code"))
                        {//下拉單欄位
                            ASP.usercontrol_codelist_ascx tempCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl("lbl_CL_" + tempTitle);
                            tempCL.SetCodeList("PY#Subsidy");
                            try
                            {
                                tempCL.SelectedCode = tempValue;
                                tempValue = tempCL.SelectedCodeName;
                            }                            
                            catch {
                                lbl_TabMsg.Text = "尚未設定補助碼!請與系統管理員聯繫!!";
                            }
                            tempCL.Visible = false;
                        }
                        else if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                        {//將日期欄位格式化                                
                            try
                            {
                                tempValue = _UserInfo.SysSet.FormatDate(theLabel.Text);
                            }
                            catch
                            {
                                tempValue = theLabel.Text;
                            }

                            if (tempValue.Contains(" Date"))
                                tempValue = "";
                        }
                        else if (tempTitle.Equals("PayRollPW"))
                        {//需解密欄位
                            if (blTabSecurity)
                            {
                                //解密  
                                tempValue = _UserInfo.SysSet.rtnTrans(tempValue);
                            }
                            else
                            {
                                tempValue = "***";
                            }
                        }

                        if (theLabel != null)
                        {
                            theLabel.Text = tempValue;
                            //if (string.IsNullOrEmpty(theLabel.Text))
                            //    theLabel.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        break;
                }
            }
        }        
    }

    protected void DetailsView_ItemInserting(int tabNo, object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
                
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        string strTableName = getTabKey("TableName", tabNo * 10);
        SqlDataSource theSDS = (SqlDataSource)this.Form.FindControl("SqlDataSource" + tabNo);
        System.Data.SqlClient.SqlCommand theInsertCmd = new System.Data.SqlClient.SqlCommand();
        string theInsertColumnSQL = "", theInsertValueSQL = "";
        bool blToHash = false;
        int theCount = -1;

        Ssql = "Select Top 1 * From " + strTableName;
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "", tempValue = "";
        InsertItem = "ALL";

        if (dtTitle.Columns.Count > 0)
        {
            theSDS.InsertParameters.Clear();
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();
                tempValue = "";
                blToHash = false;
                TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);

                if (tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId") || tempTitle.Equals("TrxType"))
                {
                    switch (tempTitle.Trim())
                    {
                        case "Company":
                            tempValue = sCompany;
                            break;
                        case "EmployeeId":
                            tempValue = sEmployeeId;
                            break;
                        case "TrxType":
                            tempValue = "A";
                            break;
                    }
                }
                else if (tempTitle.Equals("CompanyCode"))
                {
                    tempValue = _UserInfo.UData.CompanyCode;
                }
                else
                {
                    if (tb != null)
                        tempValue = tb.Text.Trim();
                    if (tempTitle.ToUpper().Contains("DATE"))
                    {//將日期欄位格式化                        
                        try
                        {
                            tempValue = _UserInfo.SysSet.FormatADDate(tempValue);
                            if (tempValue.Contains("DateTime"))
                                tempValue = "1912/01/01";
                        }
                        catch
                        {
                            tempValue = "1912/01/01";
                        }
                    }
                    else if (tempTitle.Equals("Insured_person"))
                    {//數字欄位若空白需預設為0
                        if (tempValue.Length == 0)
                            tempValue = "0";
                        else
                            tempValue = tempValue.Replace(",", "");
                    }
                    else if (tempTitle.Equals("Insured_amount") && (blHIIAShow == true))
                    {//數字加密欄位若空白需預設為0,再加密
                        if (tempValue.Trim().Length == 0)
                            tempValue = "0";
                        else
                            tempValue = tempValue.Replace(",", "");
                        tempValue = _UserInfo.SysSet.rtnHash(tempValue);
                    }
                    else if (tempTitle.Equals("Subsidy_code"))
                    {//代碼下拉單
                        ASP.usercontrol_codelist_ascx tempCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl("CodeList_" + tempTitle);
                        tempValue = tempCL.SelectedCode;
                    }
                    else if (tempTitle.Equals("PayRollPW"))
                    {//需加密欄位
                        blToHash = true;
                    }
                    else
                    {//文字欄位若空白需預設為空格
                        if (tempValue.Length == 0)
                            tempValue = " ";
                    }
                }

                bool blUnUpdate = true;
                if (tempTitle.Equals("DownLoadPersonnel")
                    || tempTitle.Equals("CompanyCode")
                    || (tempTitle.Equals("Insured_amount") && (blHIIAShow == false))
                    )
                {//排除不維護的欄位
                    blUnUpdate = false;
                }
                if (blUnUpdate == true)
                {
                    InsertValue += tempValue.Trim() + "|";

                    theInsertColumnSQL += tempTitle + ",";
                    theInsertValueSQL += "@" + tempTitle + ",";

                    if (blToHash == true)
                    {
                        if (tempValue.Trim().Length > 0)
                            theInsertCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarBinary, 256).Value = _UserInfo.SysSet.rtnTransB(tempValue.Trim());
                        else
                            theInsertCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarBinary, 256).Value = null;
                    }
                    else
                    {
                        if (tempTitle.Equals("DownLoadTime"))
                        {
                            theInsertCmd.Parameters.Add("@" + tempTitle, SqlDbType.DateTime).Value = Convert.ToDateTime("1911/01/01");
                        }
                        else
                        {
                            theInsertCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarChar).Value = tempValue;
                        }
                        theSDS.InsertParameters.Add(tempTitle, tempValue);
                    }
                }
            }
        }

        if (theInsertColumnSQL.Length > 1)
        {
            Ssql = "INSERT INTO PersonnelSecurity (" + theInsertColumnSQL.Remove(theInsertColumnSQL.Length - 1) + ") Select " + theInsertValueSQL.Remove(theInsertValueSQL.Length - 1);
            theCount = _MyDBM.ExecuteCommand(Ssql, theInsertCmd.Parameters, CommandType.Text);
        }

        if (Err.Equals(""))
        {
            //子項,不用檢核
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
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = strTableName.Trim() + "_" + tabNo;
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (InsertItem.Length * 2 > 255) ? "ALL" : InsertItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = InsertValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
            
            if (theCount == 1)
            {
                e.Cancel = true;
                DoTab_Click(tabNo, sender, e);
                lbl_TabMsg.Text = "更新成功!!";
            }
        }
    }

    protected void DetailsView_ItemInserted(int tabNo, object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception == null)
        {
            lbl_Msg.Text = "新增成功!!";
            DoTab_Click(tabNo, sender, e);
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

        if (e.Exception != null)
        {
            return;
        }
    }

    protected void DetailsView_ItemUpdating(int tabNo, object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "";
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";
        
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        string strTableName = getTabKey("TableName", tabNo * 10);
        SqlDataSource theSDS = (SqlDataSource)this.Form.FindControl("SqlDataSource" + tabNo);
        System.Data.SqlClient.SqlCommand theUpdateCmd = new System.Data.SqlClient.SqlCommand();
        string theUpdateSQL = "";
        bool blToHash = false;
        int theCount = -1;

        Ssql = "Select Top 1 * From " + strTableName;
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "", tempValue="";
        UpdateItem = "ALL";

        if (dtTitle.Rows.Count > 0)
        {
            theSDS.UpdateParameters.Clear();
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();
                TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);
                if (tb != null)
                    tempValue = tb.Text;
                else
                    tempValue = "";
                //資料預設不加密
                blToHash = false;

                if (tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId") || tempTitle.Equals("TrxType"))
                {
                    switch (tempTitle.Trim())
                    {
                        case "Company":
                            tempValue = sCompany;
                            break;
                        case "EmployeeId":
                            tempValue = sEmployeeId;
                            break;
                        case "TrxType":
                            tempValue = "U";
                            break;
                    }
                }
                else if (tempTitle.Equals("CompanyCode"))
                {
                    tempValue = _UserInfo.UData.CompanyCode;
                }
                else if (tempTitle.Equals("Suspends") || tempTitle.Equals("HI2"))
                {//下拉單                    
                    DropDownList tempDDL = (DropDownList)thisDvRow.FindControl("ddl_" + tempTitle);
                    tempValue = tempDDL.SelectedValue;
                }
                else if (tempTitle.Equals("Subsidy_code"))
                {//代碼下拉單                    
                    ASP.usercontrol_codelist_ascx tempCL = (ASP.usercontrol_codelist_ascx)thisDvRow.FindControl("CodeList_" + tempTitle);
                    tempValue = tempCL.SelectedCode;
                }
                else if (tempTitle.Equals("PayRollPW"))
                {//需加密欄位                    
                    if (tempValue.Trim().Length > 0)
                        blToHash = true;
                }
                else
                {
                    //2011/08/17修正未將轉換正確格式之資料傳入參數之問題
                    if (tempTitle.ToUpper().Contains("DATE"))
                    {//將日期欄位格式化                        
                        try
                        {
                            tempValue = _UserInfo.SysSet.FormatADDate(tempValue);
                            if (tempValue.Contains("DateTime"))
                                tempValue = "1912/01/01";
                        }
                        catch
                        {
                            tempValue = "1912/01/01";
                        }                        
                    }
                    else if (tempTitle.Equals("Insured_person"))
                    {//數字欄位若空白需預設為0
                        if (tempValue.Trim().Length == 0)
                            tempValue = "0";
                        else
                            tempValue = tempValue.Replace(",", "");
                    }
                    else if (tempTitle.Equals("Insured_amount") && (blHIIAShow == true))
                    {//數字加密欄位若空白需預設為0,再加密
                        if (tempValue.Trim().Length == 0)
                            tempValue = "0";
                        else
                            tempValue = tempValue.Replace(",", "");
                        tempValue = _UserInfo.SysSet.rtnHash(tempValue);
                    }
                    else
                    {//文字欄位若空白需預設為空格
                        if (tempValue.Trim().Length == 0)
                            tempValue = " ";
                    }
                }

                bool blUnUpdate = true;
                if (tempTitle.Equals("DownLoadTime") || tempTitle.Equals("DownLoadPersonnel")
                    || tempTitle.Equals("CompanyCode")
                    || (tempTitle.Equals("Insured_amount") && (blHIIAShow == false))
                    )
                {//排除不維護的欄位
                    blUnUpdate = false;
                }
                if (blUnUpdate == true)
                {
                    UpdateValue += tempValue + "|";
                    //處理特殊欄位
                    if (tabNo == 8 && tempTitle == "ERPID")
                        theUpdateSQL += tempTitle + "=(Case when CompanyCode='PAN-PACIFIC' then IsNull(@ADAcc,'') else IsNull(@ERPID,'') end),";
                    //排除不更新(KEY)的欄位
                    else if (!(tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId")))
                        theUpdateSQL += tempTitle + "=@" + tempTitle + ",";                   


                    if (blToHash == true)
                    {
                        if (tempValue.Length > 0)
                            theUpdateCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarBinary, 256).Value = _UserInfo.SysSet.rtnTransB(tempValue);
                        else
                            theUpdateCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarBinary, 256).Value = null;
                    }
                    else
                    {
                        theUpdateCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarChar).Value = tempValue;
                        theSDS.UpdateParameters.Add(tempTitle, tempValue);
                        if (tabNo == 8 && tempTitle == "ERPID")
                        {
                            tempTitle = "ADAcc";
                            tempValue = ((TextBox)thisDvRow.FindControl("txt_" + tempTitle)).Text;
                            theUpdateCmd.Parameters.Add("@" + tempTitle, SqlDbType.VarChar).Value = tempValue;
                            theSDS.UpdateParameters.Add(tempTitle, tempValue);
                            UpdateValue += tempTitle + "=" + tempValue;
                            //theUpdateSQL = " ERPID='" + ((e.NewValues["ADAcc"] == null || e.NewValues["ADAcc"].ToString() == "") ? e.OldValues["ADAcc"] : e.NewValues["ADAcc"]) + "'";
                        }
                    }
                }
            }
        }

        if (theUpdateSQL.Length > 1)
        {
            if (tabNo == 7)
                theUpdateSQL = "UPDATE HealthInsurance_Heading SET " + theUpdateSQL.Remove(theUpdateSQL.Length - 1) + " WHERE (Company = @Company And EmployeeId = @EmployeeId)";
            else if (tabNo == 8)
                theUpdateSQL = "UPDATE PersonnelSecurity SET " + theUpdateSQL.Remove(theUpdateSQL.Length - 1) + " WHERE (Company = @Company And EmployeeId = @EmployeeId)";            
            theCount = _MyDBM.ExecuteCommand(theUpdateSQL, theUpdateCmd.Parameters, CommandType.Text);
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = strTableName.Trim();
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (UpdateItem.Length * 2 > 255) ? "長度:" + UpdateItem.Length.ToString() : UpdateItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            if (theCount >= 1)
            {
                e.Cancel = true;
                DoTab_Click(tabNo, sender, e);
                lbl_TabMsg.Text = "更新成功!!";
            }
        }
    }
    
    protected void DetailsView_ItemUpdated(int tabNo, object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception == null)
        {
            DoTab_Click(tabNo, sender, e);
            lbl_TabMsg.Text = "更新成功!!";
        }
        else
        {
            lbl_TabMsg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
        {
            return;
        }
    }

    protected void btnSave07_Click(object sender, ImageClickEventArgs e)
    {
        btnSaveDV_Click(7, sender, e);
    }
    protected void DetailsView07_DataBound(object sender, EventArgs e)
    {
        DetailsView_DataBound(7, sender, e);
    }
    protected void DetailsView07_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        DetailsView_ItemInserting(7, sender, e);
    }
    protected void DetailsView07_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        DetailsView_ItemInserted(7, sender, e);
    }
    protected void DetailsView07_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        DetailsView_ItemUpdating(7, sender, e);
    }
    protected void DetailsView07_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        DetailsView_ItemUpdated(7, sender, e);
    }

    protected void btnSave08_Click(object sender, ImageClickEventArgs e)
    {
        btnSaveDV_Click(8, sender, e);
    }
    protected void DetailsView08_DataBound(object sender, EventArgs e)
    {
        DetailsView_DataBound(8, sender, e);
    }
    protected void DetailsView08_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        DetailsView_ItemInserting(8, sender, e);
    }
    protected void DetailsView08_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        DetailsView_ItemInserted(8, sender, e);
    }
    protected void DetailsView08_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        DetailsView_ItemUpdating(8, sender, e);
    }
    protected void DetailsView08_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        DetailsView_ItemUpdated(8, sender, e);
    }       

    protected void UpdateHIData()
    {
        if ((!string.IsNullOrEmpty(sCompany)) && (!string.IsNullOrEmpty(sEmployeeId)))        
        {
            string tempSQL = "";
            #region 全民健保
            //投保人數
            int iInsuredPerson = 0, iInsuredPersons = 0;

            DataTable DT = _MyDBM.ExecuteDataTable("Select count(*) from HealthInsurance_Heading Where Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "'");
            if (DT != null)
            {
                iInsuredPerson = (int)DT.Rows[0][0];
            }

            //全民健保-員工眷屬加保人數
            DT = _MyDBM.ExecuteDataTable("Select count(*) from HealthInsurance_Detail Where Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "' And IsNull(Suspends,'N')='N' ");
            if (DT != null)
            {
                iInsuredPersons = iInsuredPerson + (int)DT.Rows[0][0];                
            }

            TextBox tb = (TextBox)DetailsView07.FindControl("txt_Insured_person");
            if (tb != null)
            {
                if (iInsuredPersons > 0)
                    tb.Text = iInsuredPersons.ToString();
            }


            //找出投保金額
            Payroll mPayroll = new Payroll();
            Payroll.PayrolList PayrolList = new Payroll.PayrolList();
            PayrolList.Company = sCompany;
            PayrolList.EmployeeId = sEmployeeId;
            PayrolList.DeCodeKey = "dbo.PMAddHI" + DateTime.Now.ToString("yyyyMMddmmss");
            PayrolList.SalaryYM = int.Parse(DateTime.Now.ToString("yyyyMM"));
            mPayroll.BeforeQuery(PayrolList.DeCodeKey);
            HealthInsurance.DependentPremium HI = HealthInsurance.GetPremium(PayrolList);
            int mInsured_Amount = HI.Insured_Amount;
            mPayroll.AfterQuery(PayrolList.DeCodeKey);
            if (mInsured_Amount > 0)
            {             
                //全民健保-員工本人
                if (iInsuredPerson > 0)
                {//更新                
                    //2012/05/08 依需求變動為:當調薪日期之生效日晚於健保投保金額生效日時,方才自動更新
                    tempSQL = "Update HealthInsurance_Heading " +
                        " Set Insured_person = " + iInsuredPersons.ToString() +
                        " , Insured_amount = '" + _UserInfo.SysSet.rtnHash(mInsured_Amount.ToString()) + "'" +
                        " Where Company='" + sCompany + "' And EmployeeId='" + sEmployeeId + "' " +
                        " and Exists (Select * from AdjustSalary_Master a where a.Company=HealthInsurance_Heading.Company and a.EmployeeId=HealthInsurance_Heading.EmployeeId " +
                        "             and convert(varchar,a.EffectiveDate,112)>=convert(varchar,HealthInsurance_Heading.EffectiveDate,112))";
                }
                else
                {//如果新資時偷懶沒填健保資料,就預設塞一筆進去           
                    tempSQL = "INSERT INTO HealthInsurance_Heading ([Company],[EmployeeId],[EffectiveDate],[TrxType],[Insured_amount],[Insured_person],[Suspends],[Subsidy_code])" +
                        " SELECT '" + sCompany + "','" + sEmployeeId +
                        "',Convert(smalldatetime,'" + DateTime.Today.ToString("yyyy/MM/dd HH:mm:ss") + "'),'A','" + _UserInfo.SysSet.rtnHash(mInsured_Amount.ToString()) + "'," + iInsuredPersons.ToString() + ",'N','4'";
                }

                _MyDBM.ExecuteCommand(tempSQL);
            }
            #endregion
        }
    }
}
