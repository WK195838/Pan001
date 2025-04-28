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

public partial class Basic_LaborInsurance_U : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM051";
    int saveon = 0;
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany, sEmployeeId;
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript ( UP1 , this.GetType ( ) , "" , @"fn();" , true ); 
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
        if (!Page.IsPostBack)
        {
            saveon = 0;
        }
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {

        if (Page.IsPostBack && saveon == 0)
        {

            for (int i = 0; i < DetailsView1.Rows.Count; i++)
            {//修改欄位顯示名稱
                Ssql = "Select dbo.GetColumnTitle('LaborInsurance','" + DetailsView1.Rows[i].Cells[0].Text + "')";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                #region
                if (DT.Rows.Count > 0)
                {
                    if (DetailsView1.Rows[i].Cells[0].Text.Contains("TrxType"))
                    {//下拉式選單

                        DropDownList DDL = new DropDownList();
                        TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        //DDL.Style.Add("text-align", "right");
                        //預設為0
                        //DDL.ID = "ddl01"; 
                        tb.Visible = false;
                        tb.Style.Add("width", "1px");
                        string[] TType ={ "A1", "A2", "A3" };
                        string[] TName ={ "勞保投保", "勞保調整", "勞保退保" };
                        DDL.Style.Add("width", "150px");
                        for (int j = 0; j < 3; j++)
                        {
                            ListItem li = new ListItem();
                            li.Text = TType[j].ToString() + "- " + TName[j].ToString();
                            li.Value = TType[j].ToString();
                            DDL.Items.Add(li);
                        }
                        string tmp = "";
                        if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[0].ToString())
                        {
                            tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- "+TName[0].ToString();
                        }
                        else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[1].ToString())
                        {
                            tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- "+TType[1].ToString();
                        }
                        else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[2].ToString())
                        {
                            tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() + "- " + TType[2].ToString();
                        }
                        DDL.SelectedValue = tmp;
                        DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                    }

                }
                #endregion
            }

        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('LaborInsurance','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {

                if (DetailsView1.Rows[i].Cells[0].Text.Contains("LI_amount"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 7;
                    //解密
                    tb.Text = py.DeCodeAmount(tb.Text).ToString("N0");
                }

                if (DetailsView1.Rows[i].Cells[0].Text.Contains("TrxType"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    //DDL.Style.Add("text-align", "right");
                    //預設為0
                    //DDL.ID = "ddl01"; 
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    string[] TType ={ "A1", "A2", "A3" };
                    string[] TName ={ "勞保投保", "勞保調整", "勞保退保" };
                    DDL.Style.Add("width", "150px");
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = TType[j].ToString() + "- " + TName[j].ToString();
                        li.Value = TType[j].ToString();
                        DDL.Items.Add(li);
                    }
                    string tmp = "";
                    if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[0].ToString())
                    {
                        tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString();// + "- " + TName[0].ToString() + "- " + TName[1].ToString()+ "- " + TName[2].ToString()
                    }
                    else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[1].ToString())
                    {
                        tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString();
                    }
                    else if (((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text == TType[2].ToString())
                    {
                        tmp = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text.ToString() ;
                    }
                    DDL.SelectedValue = tmp;
                    //DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }


                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//日期欄位靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 8;
                    tb.CssClass = "JQCalendar";
                    //為日期欄位增加小日曆元件
                    try
                    {
                        TextBox tbDate = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        tbDate.Text = _UserInfo.SysSet.FormatDate(tbDate.Text);
                    }
                    catch { }
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("員工"))
                {
                    if (DBSetting.PersonalName(sCompany, sEmployeeId) != null)
                    {
                        DetailsView1.Rows[1].Cells[1].Text = DetailsView1.Rows[1].Cells[1].Text.Trim() + " - " + DBSetting.PersonalName(sCompany, sEmployeeId).ToString();
                    }
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("公司"))
                {
                    DetailsView1.Rows[0].Cells[1].Text = DetailsView1.Rows[0].Cells[1].Text.Trim() + " - " + DBSetting.CompanyName(sCompany).ToString();
                }
            }
        }
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        string ddlId = "DetailsView1$ddl0";
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
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();
                
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }

                if (i == 0)
                {
                    DropDownList ddl1 = (DropDownList)dv.Rows[i+2].Cells[1].Controls[1];
                    e.NewValues["TrxType"] = ddl1.SelectedValue.Trim();
                }
                else if (i == 1)
                {
                    if (dv.Rows[i + 2].Cells[0].Text.Trim().Contains("日"))
                    {//將日期欄位格式為化為西元日期
                        e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                        tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                    }
                }
                else if (i == 2)
                {
                    if (string.IsNullOrEmpty(e.NewValues[i].ToString().Trim()))
                    {//將數字空欄位放入0
                        e.NewValues[i] = "0";
                    }
                    //將投保金額去除千分號後加密
                    e.NewValues[i] = py.EnCodeAmount(e.NewValues[i].ToString().Replace(",","")).ToString();
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance";
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
        saveon = 1;
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

        ScriptManager.RegisterStartupScript ( this.Page , this.Page.GetType ( ) , "msg" , "window.close();window.opener.location='LaborInsurance.aspx';" , true );
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }
}
