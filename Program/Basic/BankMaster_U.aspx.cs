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
//08.30 新增
using System.Text;

public partial class Basic_BankMaster_U : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB010";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/BankMaster.aspx'");
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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Modify");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        //ImageButton IB;
        //switch (DetailsView1.CurrentMode)
        //{
        //    case DetailsViewMode.Edit:
        //        IB = (ImageButton)DetailsView1.FindControl("btnCancel");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:window.close();");
        //        break;

        //}
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Bank_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("TEL") || DetailsView1.Rows[i].Cells[0].Text.Contains("Fax") || DetailsView1.Rows[i].Cells[0].Text.Contains("TelexNum"))
                //{//數字欄位需靠右對齊
                //    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                //    tb.Style.Add("text-align", "right");
                //}
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("BankName_C") || DetailsView1.Rows[i].Cells[0].Text.Contains("BankName_E"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 32;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("BankAbbreviations"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 14;
                }
                //透過英文欄位取出其描述
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("BankAddress_C") || DetailsView1.Rows[i].Cells[0].Text.Contains("BankAddress_E"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 62;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("TEL") || DetailsView1.Rows[i].Cells[0].Text.Contains("Fax") || DetailsView1.Rows[i].Cells[0].Text.Contains("TelexNum"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 15;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("LocalBanks"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 1;
                }



                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                //if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                //{//將日期欄位格式化
                //    try
                //    {
                //        TextBox tbDate = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                //        tbDate.Text = _UserInfo.SysSet.FormatDate(tbDate.Text);
                //    }
                //    catch { }

                //    //為日期欄位增加小日曆元件
                //    ImageButton btOpenCal = new ImageButton();
                //    btOpenCal.ID = "btOpenCal" + i.ToString();
                //    btOpenCal.SkinID = "Calendar1";
                //    btOpenCal.OnClientClick = "return GetPromptDate(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ");";
                //    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenCal);
                //}
            }
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
        //因為前2個是ReadOnly所以要扣掉,最後一個是按鈕列也要扣掉
        for (int i = 0; i < dv.Rows.Count - 3; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();
                //if (dv.Rows[i + 2].Cells[0].Text.Trim().Contains("日"))
                //{//將日期欄位格式為化為西元日期
                //    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                //    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                //}

                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }
                //紀錄修改的欄位跟值
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Bank_Master";
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
        str.Append("window.opener.location='BankMaster.aspx';");
        str.Append("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }
}
