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

public partial class Basic_PersonnelAdjustment_U : System.Web.UI.Page
{
    string Ssql = "";
    SysSetting SysSet = new SysSetting();
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM006";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PersonnelAdjustment.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
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
  

        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        //下拉式選單位置
        int[] Range = { 5 , 7 , 9 , 11 , 13 };
        
        //下拉式選單設定
        for (int i = 0; i < Range.Length; i++)
        {
            DropDownList DDL = new DropDownList();
            DDL.Style.Add("width" , "155px");
            ((TextBox)DetailsView1.Rows[Range[i]].Cells[1].Controls[0]).Visible = false;
            DetailsView1.Rows[Range[i]].Cells[1].Controls.Add(DDL);
        }

    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {

        //資料庫查詢陣列
        String[] SqlCmd = { 
                    //0部門
                    "",
                    //1職稱
                    "Select CodeCode,CodeName from CodeDesc Where CodeID ='PY#TitleCo'",
                    //2職等
                    "SELECT SalaryLevel_CheckStandard.Level FROM SalaryLevel_CheckStandard",
                    //3薪制
                    "Select CodeCode,CodeName from CodeDesc Where CodeID ='PY#PayCode'",
                    //4班別
                    "SELECT CodeCode,CodeName FROM CodeDesc  Where CodeID ='PY#Shift'"
                };
        string tmpCompany = "";
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {
            string text0 = DetailsView1.Rows[i].Cells[0].Text;
            string text1 = DetailsView1.Rows[i].Cells[1].Text;
            DataTable DT = _MyDBM.ExecuteDataTable("SELECT dbo.GetColumnTitle('PersonnelAdjustment','" + text0 + "')");
            DataTable DT2 = new DataTable();
            


            if (DT.Rows.Count > 0)
            {
                switch (text0)
                {
                    //  公司
                    case "Company":
                        tmpCompany = text1;
                        DT2 = _MyDBM.ExecuteDataTable("SELECT Company,CompanyName FROM Company Where Company = '" + text1 + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["Company"].ToString() + " - " + DT2.Rows[0]["CompanyName"].ToString();
                        break;

                    //  員工
                    case "EmployeeId":
                        DT2 = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company = '" + tmpCompany + "' and EmployeeId = '" + text1 + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["EmployeeId"].ToString() + " - " + DT2.Rows[0]["EmployeeName"].ToString();
                        break;

                    //  生效日期
                    case "EffectiveDate":
                        DetailsView1.Rows[i].Cells[1].Text = SysSet.FormatDate(text1);
                        break;

                    //  部門（自）
                    case "DepCode_F":
                        DT2 = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where DepCode = '" + text1 + "' and Company = '" + tmpCompany + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["DepName"].ToString();
                        break;

                    //  調動類別
                    case "AdjustmentCategory":
                    //  職稱（自）
                    case "Title_F":
                    //  薪制（自）
                    case "SalarySystem_F":
                    //  班別（自）
                    case "Class_F":
                        string TmpCodeID =
                            text0.Contains("AdjustmentCategory") ? "PY#Adjustm" :
                            text0.Contains("Class_") ? "PY#Shift" :
                            text0.Contains("SalarySystem_") ? "PY#PayCode" :
                            text0.Contains("Title_") ? "PY#TitleCo" :
                            "";
                           

                        DT2 = _MyDBM.ExecuteDataTable("Select CodeCode,CodeName from CodeDesc Where CodeID ='" + TmpCodeID + "' And CodeCode = '" + text1 + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["CodeName"].ToString();
                        break;

                    // 部門（至）-     -  薪制（至）- 班別（至）

                    case "DepCode_T":
                        SetDDL("SELECT DepCode,DepName FROM Department Where Company ='" + tmpCompany + "'" , (DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1] , "DepCode" , "DepName");
                        ((DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1]).SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        break;
                    
                    //職等
                    case "Level_T":
                        SetDDL(SqlCmd[2] , (DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1] , "Level" , "");
                        ((DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1]).SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        break;

                    //職稱（至）-
                    case "Title_T":
                        SetDDL(SqlCmd[1] , (DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1] , "CodeCode" , "CodeName");
                        ((DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1]).SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.Trim();
                        break;
                    case "SalarySystem_T":
                        SetDDL(SqlCmd[3] , (DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1] , "CodeCode" , "CodeName");
                        ((DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1]).SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        break;
                    case "Class_T":
                        SetDDL(SqlCmd[4] , (DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1] , "CodeCode" , "CodeName");
                        ((DropDownList)DetailsView1.Rows[i].Cells[1].Controls[1]).SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        break;

                }

                //修改欄位顯示名稱
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
            }
        }
    }

    //下拉式選單快速設定
    private void SetDDL(string SqlCmd , DropDownList DDL , string Id , string Name)
    {
        DataTable DT = _MyDBM.ExecuteDataTable(SqlCmd);
        DDL.Items.Clear();
        if (DT.Rows.Count < 1)
            DDL.Items.Add(GetListItem("無資料" , ""));
        else
        {
            for (int i = 0 ; i < DT.Rows.Count ; i++)
            {
                DDL.Items.Add(GetListItem(Name == "" ? DT.Rows[i][Id].ToString() : DT.Rows[i][Id].ToString() + " - " + DT.Rows[i][Name].ToString() , DT.Rows[i][Id].ToString()));
                DDL.Enabled = true;
            }
        }
    }
    private ListItem GetListItem(string text , string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value;
        return li;
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

        //下拉式選單位置
        int[] Range = { 5 , 7 , 9 , 11 , 13 };
        //整理所有要存入的數值
        for (int i = 0 ; i < Range.Length ; i++)
            e.NewValues[i] = ((DropDownList)dv.Rows[Range[i]].Cells[1].Controls[1]).SelectedValue.Trim();



        for (int i = 4; i < dv.Rows.Count - 2; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();

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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonnelAdjustment";
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

        if (e.Exception != null)
        {
            return;
        }

        str = new StringBuilder();
        str.Append("<script language=javascript>");
        str.Append("window.close();");
        str.Append("window.opener.location='PersonnelAdjustment.aspx';");
        str.Append("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }

    private string getPostBackControlName()
    {
        Control control = null;
        string ctrlname = Page.Request.Params["__EVENTTARGET"];
        if (ctrlname != null && ctrlname != String.Empty)
        {
            control = Page.FindControl(ctrlname);
        }
        else
        {
            Control c;
            foreach (string ctl in Page.Request.Form)
            {
                if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                {
                    c = Page.FindControl(ctl.Substring(0, ctl.Length - 2));
                }
                else
                {
                    c = Page.FindControl(ctl);
                }
                if (c is System.Web.UI.WebControls.Button ||
                         c is System.Web.UI.WebControls.ImageButton)
                {
                    control = c;
                    break;
                }
            }
        }
        if (control != null)
            return control.ID;
        else
            return string.Empty;
    }
}
