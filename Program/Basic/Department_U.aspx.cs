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

public partial class Basic_Department_U : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB003";
    int saveon = 0;
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany,sDepCode;

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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Modify");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
            sDepCode = DetailsView1.DataKey[1].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
            if (Request["DepCode"] != null)
                sDepCode = Request["DepCode"].Trim();
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
                Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        #region
                if (DT.Rows.Count > 0)
                {
                    if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParentDepCode"))
                    {//下拉式選單

                        DropDownList DDL = new DropDownList();
                        TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where Company='" + sCompany + "' And DepCode<>'" + sDepCode + "'");
                        DDL.Items.Add("無");
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            ListItem li = new ListItem();
                            li.Text = dt2.Rows[j]["DepCode"].ToString() + " - " + dt2.Rows[j]["DepName"];
                            li.Value = dt2.Rows[j]["DepCode"].ToString();
                            DDL.Items.Add(li);
                        }
                        DDL.ID = "ddl01";
                        DDL.Style.Add("width", "150px");
                        tb.Visible = false;
                        tb.Style.Add("width", "1px");
                        DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                    }
                    if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefID"))
                    {//數字欄位需靠右對齊
                        DropDownList DDL = new DropDownList();
                        TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company='" + Request["Company"].Trim() + "' order by EmployeeId");
                        DDL.Items.Add(new ListItem("無", ""));
                        if (dt2 != null)
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt2.Rows.Count; j++)
                                {
                                    DDL.Items.Add(new ListItem(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
                                }
                            }
                        }
                        DDL.ID = "ddl02";
                        DDL.Style.Add("width", "150px");
                        tb.Visible = false;
                        tb.Style.Add("width", "1px");
                        DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                        DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                    }
                    if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepType"))
                    {//數字欄位需靠右對齊
                        DropDownList DDL = new DropDownList();
                        TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                        DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT CodeCode,CodeName FROM CodeDesc Where CodeID='DeptType'");
                        if (dt2 != null)
                        {
                            if (dt2.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt2.Rows.Count; j++)
                                {
                                    DDL.Items.Add(new ListItem(dt2.Rows[j]["CodeCode"].ToString() + " - " + dt2.Rows[j]["CodeName"].ToString(), dt2.Rows[j]["CodeCode"].ToString()));
                                }
                            }
                        }
                        DDL.ID = "ddl03";
                        DDL.Style.Add("width", "150px");
                        tb.Visible = false;
                        tb.Style.Add("width", "1px");
                        DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
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
            Ssql = "Select dbo.GetColumnTitle('Department','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepNameE"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 100;
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepName"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 50;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("CostType"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 1;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepType"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT CodeCode,CodeName FROM CodeDesc Where CodeID='DeptType'");
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                DDL.Items.Add(new ListItem(dt2.Rows[j]["CodeCode"].ToString() + " - " + dt2.Rows[j]["CodeName"].ToString(), dt2.Rows[j]["CodeCode"].ToString()));
                            }
                        }
                    }
                    DDL.ID = "ddl03";
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefTitle"))
                {//數字欄位需靠右對齊
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.Style.Add("width", "145px");
                    tb.MaxLength = 10;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefID"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company='" + Request["Company"].Trim() + "' order by EmployeeId");
                    DDL.Items.Add(new ListItem("無", ""));
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                DDL.Items.Add(new ListItem(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
                            }
                        }
                    }
                    DDL.ID = "ddl02";
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParentDepCode"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where Company='" + sCompany + "' And DepCode<>'" + sDepCode + "'");
                    DDL.Items.Add(new ListItem("無",""));
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = dt2.Rows[j]["DepCode"].ToString().Trim() + " - " + dt2.Rows[j]["DepName"];
                        li.Value = dt2.Rows[j]["DepCode"].ToString().Trim();
                        DDL.Items.Add(li);
                    }
                    DDL.ID = "ddl01";
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DDL.SelectedValue = ((TextBox)DetailsView1.Rows[i].Cells[1].Controls[0]).Text;
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParentDepCode"))
                //{//數字欄位需靠右對齊
                //    TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                //    tb.Style.Add("width", "145px");
                //    tb.MaxLength = 5;
                //}


                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

                if (DT.Rows[0][0].ToString().Trim().Contains("公司"))
                {
                    if (DBSetting.CompanyName(sCompany).ToString() != "")
                    {
                        DetailsView1.Rows[0].Cells[1].Text = DetailsView1.Rows[0].Cells[1].Text.Trim() + " - " + DBSetting.CompanyName(sCompany).ToString();
                    }
                }

            }
        }
    }
    private ListItem Li(string text, string value)
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
                if (i == 3)
                {
                    e.NewValues[i] = Request.Form[ddlId + "3"].ToString();
                }
                if (i == 6)
                {
                    e.NewValues[i] = Request.Form[ddlId + "1"].ToString();
                }
                if (i == 5)
                {
                    e.NewValues[i] = Request.Form[ddlId + "2"].ToString();
                }
                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }


                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += dv.Rows[i + 3].Cells[0].Text.Trim() + "|";
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Department";
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

        str = new StringBuilder();
        str.Append("<script language=javascript>");
        str.Append("window.close();");
        str.Append("window.opener.location='Department.aspx?Company=" + sCompany + "';");
        str.Append("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.UpdateItem(true);
    }
}
