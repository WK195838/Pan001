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
//08.30 修改
using System.Text;

public partial class Basic_BankMaster_A : System.Web.UI.Page
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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/BankMaster.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Add");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        //08.31修改
        //ImageButton IB;
        //switch (DetailsView1.CurrentMode)
        //{
        //    case DetailsViewMode.Insert:
        //        IB = (ImageButton)DetailsView1.FindControl("btnSaveGo");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        //        IB = (ImageButton)DetailsView1.FindControl("btnSaveExit");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        //        hid_InserMode.Value = "";
        //        IB = (ImageButton)DetailsView1.FindControl("btnCancel");
        //        if (IB != null)
        //            IB.Attributes.Add("onclick", "javascript:window.close();");
        //        break;
        //}

        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//08.30 修改
            //修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Bank_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("TEL") || DetailsView1.Rows[i].Cells[0].Text.Contains("Fax") || DetailsView1.Rows[i].Cells[0].Text.Contains("TelexNum"))
                //{//數字欄位需靠右對齊
                //    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                //    //tb.Style.Add("text-align", "right");
                //    ////預設為0
                //    //tb.Text = "0";
                //}  
                TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                switch (i)
                {
                    case 0:
                        tb.MaxLength = 3;
                        break;
                    case 1:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 4;
                        break;
                    case 2:
                    case 3:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 32;
                        break;
                    case 4:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 14;
                        break;
                    case 5:
                    case 6:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 62;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 15;
                        break;
                    case 10:
                        //TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tb.MaxLength = 1;
                        break;
                }
                //透過英文欄位取出其描述
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

                #region ---不使用的程式碼---
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("Company"))
                //{
                //    //為公司欄位增加提示視窗
                //    ImageButton btOpenList = new ImageButton();
                //    btOpenList.ID = "btOpen" + i.ToString();
                //    btOpenList.SkinID = "OpenWin1";
                //    //Company,CompanyShortName,CompanyName,ChopNo
                //    btOpenList.OnClientClick = "return GetPromptWin(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ",'Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');";
                //    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenList);
                //}

                ////日期欄位
                //if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                //{//日期欄位靠右對齊
                //    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                //    tb.Style.Add("text-align", "right");

                //    //為日期欄位增加小日曆元件
                //    ImageButton btOpenCal = new ImageButton();
                //    btOpenCal.ID = "btOpenCal" + i.ToString();
                //    btOpenCal.SkinID = "Calendar1";
                //    btOpenCal.OnClientClick = "return GetPromptDate(" + DetailsView1.Rows[i].Cells[1].Controls[0].ClientID + ");";
                //    DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenCal);
                //}
                #endregion
            }
        }
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
//BankHeadOffice	銀行總行代號
//BankBranch	銀行分行代號
//BankName_C	銀行名稱－中文
//BankName_E	銀行名稱－英文
//BankAbbreviations	銀行簡稱
//BankAddress_C	銀行地址－中文
//BankAddress_E	銀行地址－英文
//TEL	電話
//Fax	傳真機
//TelexNum	電報號碼
//LocalBanks	本地銀行

        if (e.Values["BankHeadOffice"] == null)
        {
            Err = "銀行總行代號不可空白<br>";
        }

        if (e.Values["BankBranch"] == null)
        {
            Err += "銀行分行代號不可空白<br>";
        }

        if (Err.Equals(""))
        {
            if (!ValidateData(e.Values["BankHeadOffice"].ToString(), e.Values["BankBranch"].ToString()))
            {
                Err += "代號重覆!!此銀行總行下已有此銀行分行代號!";
            }
        }

        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {

            if (e.Values[i] == null)
            {
                e.Values[i] = " ";
            }
            ////日期欄位
            //if (dv.Rows[i].Cells[0].Text.Contains("日"))
            //{
            //    e.Values[i] = _UserInfo.SysSet.FormatADDate(e.Values[i].ToString());
            //}

            try
            {//準備寫入LOG用之參數                                
                InsertItem += DetailsView1.Rows[i].Cells[0].Text.Trim() + "|";
                InsertValue += e.Values[i].ToString() + "|";
            }
            catch
            { }
        }
        #region ---不使用的程式碼---
        ////特別年資
        //if (string.IsNullOrEmpty(e.Values["SpecialSeniority"].ToString().Trim()))
        //{
        //    e.Values["SpecialSeniority"] = "0";
        //}
        ////撫養人數
        //if (string.IsNullOrEmpty(e.Values["DependentsNum"].ToString().Trim()))
        //{
        //    e.Values["DependentsNum"] = "0";
        //}
        #endregion
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
            str.Append("window.opener.location='BankMaster.aspx';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    private bool ValidateData(string BankHeadOffice, string BankBranch)
    {
        Ssql = "Select * From Bank_Master Where BankHeadOffice='" + BankHeadOffice + "' And BankBranch='" + BankBranch + "'";
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
        DetailsView1.InsertItem(true);
    }
}
