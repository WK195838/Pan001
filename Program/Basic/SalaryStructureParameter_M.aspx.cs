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

public partial class SalaryStructureParameter_M : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB007";
    
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string[] tempShow = new string[20];
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        StyleTitle1.ShowBackToPre = false;
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Modify");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        bool blReadOnly = false;
        bool blInsertMod = false;
        if (!Page.IsPostBack)
        {

        }
        else
        {

        }

        if (Request["Kind"] != null)
        {
            blReadOnly = Request["Kind"].Equals("Query");
        }
        else
        {            
            blInsertMod = true;
        }

        DetailsView1.DefaultMode = (blInsertMod == true) ? DetailsViewMode.Insert : DetailsViewMode.Edit;

        if (blReadOnly == false)
        {
            btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
            btnSaveGo.CommandName = (blInsertMod == true) ? "Insert" : "Update";
            btnSaveGo.AlternateText = (blInsertMod == true) ? btnSaveGo.AlternateText : "儲存修改";
            btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
            btnSaveExit.CommandName = (blInsertMod == true) ? "Insert" : "Update";
        }
        
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
        
        #region 查詢顯示控管
        if (blReadOnly)
        {
            StyleTitle1.Title = "薪資結構參數查詢";
            DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            btnSaveGo.Visible = false;
            btnSaveExit.Visible = false;
        }
        #endregion
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";
            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Columns.Count > 0)
            {                
                string strTempValue = "";
                strTempValue = DetailsView1.Rows[i].Cells[0].Text;
                #region 一般文字輸入
                if (DetailsView1.DefaultMode != DetailsViewMode.ReadOnly)
                {
                    if (DetailsView1.Rows[i].Cells[1].Controls.Count > 0)
                    {
                        TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        if (strTempValue.Contains("SalaryName"))
                        {//數字欄位需靠右對齊                                            
                            tb.Style.Add("width", "145px");
                            tb.MaxLength = 14;
                        }
                        else if (strTempValue.Contains("AcctNo"))
                        {//數字欄位需靠右對齊                        
                            tb.Style.Add("text-align", "right");
                            tb.Style.Add("width", "145px");
                            tb.MaxLength = 8;
                        }
                        else if (strTempValue.Contains("FixedAmount"))
                        {//數字欄位需靠右對齊                        
                            tb.Style.Add("text-align", "right");
                            tb.Style.Add("width", "145px");
                            tb.MaxLength = 7;
                        }
                        else if (strTempValue.Contains("SalaryRate"))
                        {//數字欄位需靠右對齊                        
                            tb.Style.Add("text-align", "right");
                            tb.Style.Add("width", "145px");
                            tb.MaxLength = 4;
                        }
                        else if (strTempValue.Contains("P1SalaryMasterList") || strTempValue.Contains("SalaryMasterList")
                            || strTempValue.Contains("P1Payroll") || strTempValue.Contains("Payroll"))
                        {//數字欄位需靠右對齊                    
                            tb.Style.Add("width", "145px");
                            tb.Style.Add("text-align", "right");
                            tb.MaxLength = 2;
                        }
                        else if (strTempValue.Contains("ParameterList"))
                        {
                            tb.Style.Add("width", "250px");
                            tb.MaxLength = 500;
                            ImageButton btOpenList = new ImageButton();
                            btOpenList.ID = "btOpen" + i.ToString();
                            btOpenList.SkinID = "OpenWin1";
                            btOpenList.OnClientClick = "return GetPromptWin3(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'400','450','SalaryStructure_Parameter','SalaryId','SalaryId As 薪資代碼','SalaryId');";
                            DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenList);
                        }
                        //假如有給值,就要更新它;沒給就不用理
                        tempShow[i] = (tempShow[i] == null) ? tempShow[i] : tb.Text;
                    }
                }
                #endregion
                #region 代碼下拉單
                if (strTempValue.Contains("NWTax") || strTempValue.Contains("PMType")
                    || strTempValue.Contains("ItemType") || strTempValue.Contains("SalaryType")
                    || strTempValue.Contains("Properties") || strTempValue.Contains("P1CostSalaryItem")
                    || strTempValue.Contains("RegularPay") || strTempValue.Contains("FunctionId")
                    || strTempValue.Contains("BaseItem")
                    )
                {
                    if (DetailsView1.Rows[i].Cells[1].Controls.Count > 0)
                    {
                        TextBox theTB = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        if (theTB != null)
                        {
                            theTB.Visible = false;
                            theTB.Style.Add("width", "1px");
                        }
                    }
                    ASP.usercontrol_codelist_ascx DDL = new ASP.usercontrol_codelist_ascx();
                    DDL = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    DDL.ID = "CL_" + (i - 1).ToString().PadLeft(2, '0');
                    if (strTempValue.Contains("FunctionId"))
                        DDL.SetDTList("CodeDesc", "CodeCode", "CodeName", "CodeID like 'function' OR CodeID like 'sqlcomm'", 5, "不選擇公式");
                    else if (strTempValue.Contains("BaseItem"))
                        DDL.SetDTList("SalaryStructure_Parameter", "SalaryId", "SalaryName", "", 5, "無");
                    else
                        DDL.SetCodeList("PY#" + ((strTempValue.Length > 7) ? strTempValue.Remove(7) : strTempValue));
                    DDL.StyleAdd("width", "150px");

                    if (DetailsView1.DefaultMode == DetailsViewMode.ReadOnly)
                    {
                        try
                        {
                            DDL.SelectedCode = ((DataRowView)DataBinder.GetDataItem(sender)).Row[i].ToString();
                        }
                        catch (Exception ex) { }
                        tempShow[i] = DDL.SelectedCodeName.Trim();
                        DetailsView1.Rows[i].Cells[1].Text = DDL.SelectedCodeName.Trim();
                    }
                    else
                    {
                        DDL.SelectedCode = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        //if (strTempValue.Contains("ItemType"))
                        //{
                        //    if (DDL.SelectedCode.Equals("0"))
                        //        DDL.Enabled = false;
                        //    else
                        //    {
                        //        DDL.SetDTList("CodeDesc", "CodeCode", "CodeName", "CodeID = 'PY#ItemTyp' And [CodeCode] <> '0'", 5, "請選擇");
                        //        DDL.SelectedCode = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        //    }
                        //}
                        DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                    }
                }
                #endregion
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
            }
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {                
        if (DetailsView1.DefaultMode != DetailsViewMode.Insert)
        {
            if (DataBinder.GetDataItem(sender) != null)
            {
                string strValue = "";
                for (int i = 0; i < DetailsView1.Rows.Count; i++)
                {//為代碼下拉單設定已選擇項目
                    strValue = ((DataRowView)DataBinder.GetDataItem(sender)).Row[i].ToString();
                    try
                    {
                        if (DetailsView1.DefaultMode == DetailsViewMode.Edit)
                        {
                            if (DetailsView1.Rows[i].Cells[1].Controls.Count > 1)
                            {
                                ASP.usercontrol_codelist_ascx DDL = (ASP.usercontrol_codelist_ascx)DetailsView1.Rows[i].Cells[1].Controls[1];
                                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("項目別"))
                                if (i == 4)
                                {
                                    if (strValue.Equals("0"))
                                        DDL.Enabled = false;
                                    else
                                        DDL.SetDTList("CodeDesc", "CodeCode", "CodeName", "CodeID = 'PY#ItemTyp' And [CodeCode] <> '0'", 5, "請選擇");
                                }
                                DDL.SelectedCode = strValue;
                            }
                        }
                        else
                        {
                            DetailsView1.Rows[i].Cells[1].Text = (tempShow[i] == null) ? strValue : tempShow[i];
                        }
                    }
                    catch { }
                }
            }
        }
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
        //e.Values.
        if (e.Values["SalaryId"] == null)
        {
            Err = "薪資代碼不可空白<br>";
        }


        //if (e.Values["EmployeeId"] == null)
        //{
        //    Err += "員工代號不可空白<br>";
        //}
        //if(e.Values["SalaryId"])
        if (Err.Equals(""))
        {
            if (!ValidateData(e.Values["SalaryId"].ToString()))//, e.Values["EmployeeId"].ToString()
            {
                Err += "代號重覆!!已有此薪資代碼!<br>";
            }
            if (e.Values["SalaryId"].ToString().Substring(0, 1) == "0" || e.Values["SalaryId"].ToString().Substring(0, 1) == "1")
            {
                if (Convert.ToInt32(e.Values["SalaryId"]) < 20)
                {
                    Err += "此薪資代碼為保留碼!! 請輸入其它薪資代碼";
                }
            }
        }


        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {
            #region 代碼下拉單
            if ((i == 2) || (i == 3) || (i == 4) || (i == 5) || (i == 8) || (i == 9) || (i == 15) || (i == 16) || (i == 17))
            {
                ASP.usercontrol_codelist_ascx ddl = (ASP.usercontrol_codelist_ascx)dv.Rows[i].Cells[1].Controls[1];
                if ((i == 15) && (ddl.SelectedCode.Trim() == "C"))
                {
                    if (e.Values[i - 2].ToString() != "0" && e.Values[i - 1].ToString() != "0")
                    {
                        Err += "選擇非上期項目，上期總表與上期薪資單的值必須為零<br>";
                    }
                    else if (e.Values[i - 2].ToString() == "0" && e.Values[i - 1].ToString() != "0")
                    {
                        Err += "選擇非上期項目，上期薪資單的值必須為零<br>";
                    }
                    else if (e.Values[i - 2].ToString() != "0" && e.Values[i - 1].ToString() == "0")
                    {
                        Err += "選擇非上期項目，上期總表的值必須為零<br>";
                    }
                    else
                    {
                        e.Values[i] = "C";
                    }
                }
                else
                {
                    e.Values[i] = ddl.SelectedCode.Trim();
                }
            }
            #endregion

            #region
            if (i == 6 || i == 10 || i == 11 || i == 13 || i == 14)
            {
                if (e.Values[i] == null)
                {//將數字空欄位放入0
                    e.Values[i] = "0";
                }
            }
            if (i == 1 || i == 8 || i == 12)
            {
                if (e.Values[i] == null)
                {//將數字空欄位放入0
                    e.Values[i] = " ";
                }
            }

            //}
            #endregion
            //薪資比率
            if (dv.Rows[i].Cells[0].Text.Contains("SalaryRate") || dv.Rows[i].Cells[0].Text.Contains("薪資比率"))
            {
                if (e.Values[i] != null)
                {
                    if (e.Values[i].ToString().IndexOf(".") != 1)
                    {
                        Err += "薪資比率!!格式錯誤<br>";
                    }
                    else if (e.Values[i].ToString() == "0" || e.Values[i].ToString() == " ")
                    {
                        e.Values[i] = "0.00";
                    }
                }
                else
                    e.Values[i] = "0.00";
            }
            if (dv.Rows[i].Cells[0].Text.Contains("ParameterList") || dv.Rows[i].Cells[0].Text.Contains("參數"))
            {
                #region ---參數位數判斷---
                ASP.usercontrol_codelist_ascx ddl = (ASP.usercontrol_codelist_ascx)dv.Rows[i - 1].Cells[1].Controls[1];
                string Fomula = ddl.SelectedCodeName;
                if (Fomula.IndexOf(":") > 0)
                {//區分出公式(公式說明:公式)
                    Fomula = Fomula.Substring(Fomula.IndexOf(":") + 1);
                }
                string strChar = "";
                if (e.Values[i - 1].ToString() == "")
                {//不選擇公式
                    if (e.Values[i] != null)
                    {
                        Err += "要輸入或選擇參數，請先選擇公式<br>";
                    }
                }
                else
                {
                    for (int j = 0; j < Fomula.Length; j++)
                    {
                        switch (Fomula[j].ToString())
                        {
                            case "(":
                            case ")":
                            case "+":
                            case "-":
                            case "*":
                            case "/":
                                break;
                            default:
                                if (strChar.IndexOf(Fomula[j].ToString()) == -1)
                                {
                                    strChar += Fomula[j].ToString();
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                        }
                    }

                    //檢查輸入參數個數是否等於公式參數
                    if (e.Values[i] != null)
                    {
                        #region ---參數個數與公式所需參數判斷---
                        string t = e.Values[i].ToString().Substring(e.Values[i].ToString().Trim().Length - 1, 1); //e.Values[i].ToString().Substring((e.Values[i].ToString().Trim().Length - 6), (e.Values[i].ToString().Trim().Length - 1));
                        if (t == ",")
                        {
                            string temp = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            string[] para = temp.ToString().Split(new char[] { ',' });
                            if (para.Length > strChar.Length)
                            {
                                Err += "選擇或輸入的參數過多<br>";
                            }
                            else if (para.Length < strChar.Length)
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                                for (int k = 0; k < (strChar.Length - para.Length); k++)
                                {
                                    e.Values[i] += "0,";
                                }
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            }
                            else
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length - 1);
                            }
                        }
                        else
                        {
                            string temp = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            string[] para = temp.ToString().Split(new char[] { ',' });
                            if (para.Length > strChar.Length)
                            {
                                Err += "選擇或輸入的參數過多<br>";
                            }
                            else if (para.Length < strChar.Length)
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                                for (int k = 0; k < (strChar.Length - para.Length); k++)
                                {
                                    e.Values[i] += ",0";
                                }
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            }
                            else
                            {
                                e.Values[i] = e.Values[i].ToString().Substring(0, e.Values[i].ToString().Trim().Length);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Err += "設定公式後，請輸入參數";
                    }
                }
                #endregion
            }

            try
            {//準備寫入LOG用之參數                                
                InsertItem += DetailsView1.Rows[i].Cells[0].Text.Trim() + "|";
                InsertValue += e.Values[i].ToString() + "|";
            }
            catch
            { }

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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
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

        if (e.Exception != null)
        {
            return;
        }

        if (hid_InserMode.Value.Trim().Equals("EXIT"))
        {
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='SalaryStructureParameter_PIC.aspx';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    private bool ValidateData(string SalaryId)
    {
        Ssql = "Select * From SalaryStructure_Parameter Where SalaryId='" + SalaryId + "'";//  And EmployeeId='" + EmployeeId + "'
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

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        string ddlId = "DetailsView1$CL_";
        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        DetailsView dv = ((DetailsView)sender);
        //因為前1個是ReadOnly所以要扣掉,最後一個是按鈕列也要扣掉
        for (int i = 0; i < dv.Rows.Count - 2; i++)
        {
            try
            {
                if (e.OldValues[i] == null)
                {
                    tempOldValue = "";
                }
                else
                {
                    tempOldValue = e.OldValues[i].ToString().Trim();
                }
                if (e.NewValues[i] == null)
                {
                    e.NewValues[i] = "";
                }
                else
                {
                    e.NewValues[i] = e.NewValues[i].ToString().Trim();
                }
                //薪資比率位數判斷
                if (dv.Rows[i].Cells[0].Text.Contains("SalaryRate") || dv.Rows[i].Cells[0].Text.Contains("薪資比率"))
                {
                    if (e.NewValues[i-1].ToString().IndexOf(".") != 1)
                    {
                        Err += "薪資比率!!格式錯誤<br>";
                    }
                    else if (e.NewValues[i-1].ToString() == "0" || e.NewValues[i].ToString() == " ")
                    {
                        e.NewValues[i-1] = "0.00";
                    }
                }
                if (dv.Rows[i + 2].Cells[0].Text.Trim().Contains("ParameterList") || dv.Rows[i + 2].Cells[0].Text.Trim().Contains("參數"))
                {

                    if (Request.Form[ddlId + (i.ToString().PadLeft(2, '0')) + "$ddlCodeList"].ToString() == "")
                    {//不選擇公式
                        if (e.NewValues[i + 1] != null)
                        {
                            Err += "要輸入或選擇參數，請先選擇公式<br>";
                        }
                    }
                    else
                    {
                        string Fomula = Request.Form[ddlId + (i.ToString().PadLeft(2, '0')) + "$ddlCodeList"].ToString();
                        if (Fomula.IndexOf(":") > 0)
                        {//區分出公式(公式說明:公式)
                            Fomula = Fomula.Substring(Fomula.IndexOf(":") + 1);
                        }
                        string strChar = "";
                        for (int j = 0; j < Fomula.Length; j++)
                        {
                            switch (Fomula[j].ToString())
                            {
                                case "(":
                                case ")":
                                case "+":
                                case "-":
                                case "*":
                                case "/":
                                    break;
                                default:
                                    if (strChar.IndexOf(Fomula[j].ToString()) == -1)
                                    {
                                        strChar += Fomula[j].ToString();
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                            }
                        }
                        if (e.NewValues[i + 1] != null)
                        {
                            #region ---判斷參數位數---
                            string t = e.NewValues[i + 1].ToString().Substring(e.NewValues[i + 1].ToString().Trim().Length - 1, 1); //e.Values[i].ToString().Substring((e.Values[i].ToString().Trim().Length - 6), (e.Values[i].ToString().Trim().Length - 1));
                            if (t == ",")
                            {
                                string temp = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                string[] para = temp.ToString().Split(new char[] { ',' });
                                if (para.Length > strChar.Length)
                                {
                                    Err += "選擇或輸入的參數過多<br>";
                                }
                                else if (para.Length < strChar.Length)
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                    for (int k = 0; k < (strChar.Length - para.Length); k++)
                                    {
                                        e.NewValues[i + 1] += "0,";
                                    }
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                }
                                else
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length - 1);
                                }
                            }
                            else
                            {
                                string temp = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                string[] para = temp.ToString().Split(new char[] { ',' });
                                if (para.Length > strChar.Length)
                                {
                                    Err += "選擇或輸入的參數過多<br>";
                                }
                                else if (para.Length < strChar.Length)
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                    for (int k = 0; k < (strChar.Length - para.Length); k++)
                                    {
                                        e.NewValues[i + 1] += ",0";
                                    }
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                }
                                else
                                {
                                    e.NewValues[i + 1] = e.NewValues[i + 1].ToString().Substring(0, e.NewValues[i + 1].ToString().Trim().Length);
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            Err += "設定公式後，請輸入參數<br>";
                        }
                    }
                }

                if ((i == 1) || (i == 2) || (i == 3) || (i == 4) || (i == 7) || (i == 8) || (i == 14) || (i == 15) || (i == 16))
                {
                    string strTemp = ddlId + (i.ToString().PadLeft(2, '0')) + "$ddlCodeList";
                    if ((i == 14) && (Request.Form[strTemp].ToString() == "C"))
                    {
                        if (e.NewValues[i - 2].ToString() != "0" && e.NewValues[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期總表與上期薪資單的值必須為零<br>";
                        }
                        else if (e.NewValues[i - 2].ToString() == "0" && e.NewValues[i - 1].ToString() != "0")
                        {
                            Err += "選擇非上期項目，上期薪資單的值必須為零<br>";
                        }
                        else if (e.NewValues[i - 2].ToString() != "0" && e.NewValues[i - 1].ToString() == "0")
                        {
                            Err += "選擇非上期項目，上期總表的值必須為零<br>";
                        }
                        else
                        {
                            e.NewValues[i] = Request.Form[strTemp].ToString();
                        }
                    }
                    else
                    {
                        e.NewValues[i] = Request.Form[strTemp].ToString();
                    }
                }

                if (i == 5 || i == 9 || i == 10 || i == 12 || i == 13)
                {
                    if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                    {//將數字空欄位放入0
                        e.NewValues[i] = "0";
                    }
                }
                if (i == 6)
                {
                    if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                    {//將空欄位放入半形空格
                        e.NewValues[i] = "0.000";
                    }
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
            DetailsView1.DataBind();
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
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
            lbl_Msg.Text = "修改成功!!";
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

        if (e.Exception != null)
        {
            return;
        }

        if (hid_InserMode.Value.Trim().Equals("EXIT"))
        {
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='SalaryStructureParameter_PIC.aspx';");
            str.Append("window.close();");
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
}
