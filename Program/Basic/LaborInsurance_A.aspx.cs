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

public partial class Basic_LaborInsurance_A : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM051";
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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Add");
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
        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('LaborInsurance','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("Company"))
                {
                    //根據登入者的公司 帶入公司欄位
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 2;
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DataTable dt = _MyDBM.ExecuteDataTable("SELECT Company,CompanyShortName FROM Company Where Company='" + Request["Company"].Trim() + "' ");//Where CompanyName='" + _UserInfo.UData.Company + "'
                    DropDownList ddl = new DropDownList();
                    ddl.ID = "DDL" + (i + 1).ToString("D2");
                    ddl.Style.Add("width", "150px");
                    ddl.Items.Add("請選擇");
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = dt.Rows[j]["Company"].ToString() + " - " + dt.Rows[j]["CompanyShortName"].ToString();
                        li.Value = dt.Rows[j]["Company"].ToString();
                        ddl.Items.Add(li);
                    }
                    ddl.AutoPostBack = true;
                    ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
                    DetailsView1.Rows[i].Cells[1].Controls.Add(ddl);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("EmployeeId"))
                {//欄位輸入欄位長度限制
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 5;
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    //根據登入者的公司 選擇可以新增的員工編號
                    //DataTable dt = _MyDBM.ExecuteDataTable("SELECT Personnel_Master.EmployeeId,Personnel_Master.EmployeeName FROM Personnel_Master,Company_Master Where Personnel_Master.Company=Company_Master.Company And Company_Master.CompanyShortName='" + _UserInfo.UData.Company + "'");
                    DropDownList ddl2 = new DropDownList();
                    ddl2.ID = "DDL" + (i + 1).ToString("D2");
                    ddl2.Style.Add("width", "150px");
                    ddl2.Items.Add("請選擇");
                    ddl2.AutoPostBack = true;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(ddl2);
                }

                if (DetailsView1.Rows[i].Cells[0].Text.Contains("TrxType"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    //DDL.Style.Add("text-align", "right");
                    //預設為0
                    string[] TType ={ "A1", "A2", "A3" };
                    string[] TName ={ "勞保投保", "勞保調整", "勞保退保" };
                    for (int j = 0; j < 3; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = TType[j].ToString() + "- " + TName[j].ToString();
                        li.Value = TType[j].ToString();
                        DDL.Items.Add(li);
                    }
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("LI_amount"))
                {//欄位輸入欄位長度限制

                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.MaxLength = 7;
                }
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//日期欄位靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("text-align", "right");
                    tb.CssClass = "JQCalendar";
                   
                }
            }
        }
    }
    void ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        string temp = ((DropDownList)sender).SelectedValue.ToString();
        string sqlstr = "SELECT EmployeeId,EmployeeName FROM Personnel_Master Where IsNull(ResignCode,'') != 'Y' And Company='" + temp + "' order by EmployeeId";
        DropDownList ddl = (DropDownList)DetailsView1.Rows[1].Cells[1].Controls[1];


        DataTable dt = _MyDBM.ExecuteDataTable(sqlstr);

        ddl.Items.Clear();
        ddl.Items.Add("請選擇");
        for (int j = 0; j < dt.Rows.Count; j++)
        {

            ListItem li = new ListItem();
            li.Text = dt.Rows[j]["EmployeeId"].ToString() + " - " + dt.Rows[j]["EmployeeName"].ToString();
            li.Value = dt.Rows[j]["EmployeeId"].ToString();
            ddl.Items.Add(li);
        }
        DetailsView1.Rows[1].Cells[1].Controls.Add(ddl);


    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";


        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {
            #region ---下拉式選單---
            if (e.Values[i] == null)
            {
                e.Values[i] = " ";
            }

            if (i == 0)
            {
                DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values["Company"] = ddl.SelectedValue.Trim();
            }
            else if (i == 1)
            {
                DropDownList ddl1 = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values["EmployeeId"] = ddl1.SelectedValue.Trim();
            }
            else if (i == 2)
            {
                DropDownList ddl1 = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values["TrxType"] = ddl1.SelectedValue.Trim();
            }
            else if (i == 4)
            {
                if (string.IsNullOrEmpty(e.Values[i].ToString().Trim()))
                {//將數字空欄位放入0
                    e.Values[i] = "0";
                }
                //將投保金額去除千分號後加密
                e.Values[i] = py.EnCodeAmount(e.Values[i].ToString().Replace(",", "")).ToString();
            }

            //日期欄位
            if (dv.Rows[i].Cells[0].Text.Contains("日"))
            {
                e.Values[i] = _UserInfo.SysSet.FormatADDate(e.Values[i].ToString());
            }

            //}
            #endregion
            //薪資比率

            

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
            if (!ValidateData(e.Values["Company"].ToString(), e.Values["EmployeeId"].ToString()))//, e.Values["EmployeeId"].ToString()
            {
                Err += "資料重覆!!此公司下已有此筆資料!<br>";
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
            ScriptManager.RegisterStartupScript ( this.Page , this.Page.GetType ( ) , "msg" , "window.close();window.opener.location='LaborInsurance.aspx';" , true );
        }
    }

    private bool ValidateData(string Company, string EmployeeId)
    {
        Ssql = "Select * From LaborInsurance Where Company='" + Company + "' And EmployeeId='" + EmployeeId + "'";//  And EmployeeId='" + EmployeeId + "'
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
