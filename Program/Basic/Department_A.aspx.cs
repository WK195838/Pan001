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

public partial class Basic_Department_A : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB003";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany;

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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Add");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
        }
        btnSaveGo.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='GO';");
        btnSaveExit.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }

    protected void DetailsView1_ItemCreated(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Department','" + DetailsView1.Rows[i].Cells[0].Text + "')";
            //Company	公司編號
            //DepCode	部門代號
            //DepName	部門名稱
            //DepNameE	部門英文名稱
            //CostType	成本類型
            //DepType	部門類型
            //ChiefTitle	主管職稱
            //ChiefID	主管
            //ParentDepCode	父階部門代號
            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            if (DT.Rows.Count > 0)
            {
                TextBox tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                tb.Style.Add("width", "145px");
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("Company"))
                {//下拉式選單

                    DropDownList DDL = new DropDownList();
                    DataTable dt = _MyDBM.ExecuteDataTable("SELECT Company,CompanyShortName FROM Company Where Company='" + Request["Company"].Trim() + "'");
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = dt.Rows[j]["Company"].ToString() + " - " + dt.Rows[j]["CompanyShortName"];
                        li.Value = dt.Rows[j]["Company"].ToString();
                        DDL.Items.Add(li);
                    }
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);

                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParentDepCode"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where Company='" + Request["Company"].Trim() + "'");
                    DDL.Items.Add("無");
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        ListItem li = new ListItem();
                        li.Text = dt2.Rows[j]["DepCode"].ToString() + " - " + dt2.Rows[j]["DepName"];
                        li.Value = dt2.Rows[j]["DepCode"].ToString();
                        DDL.Items.Add(li);
                    }
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepCode"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 10;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepNameE"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 100;
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepName"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 50;
                }

                if (DetailsView1.Rows[i].Cells[0].Text.Contains("CostType"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 1;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("DepType"))
                {//數字欄位需靠右對齊
                    DropDownList ddl = new DropDownList();
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT CodeCode,CodeName FROM CodeDesc Where CodeID='DeptType'");
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ddl.Items.Add(new ListItem(dt2.Rows[j]["CodeCode"].ToString() + " - " + dt2.Rows[j]["CodeName"].ToString(), dt2.Rows[j]["CodeCode"].ToString()));
                            }
                        }
                    }
                    ddl.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(ddl);
                }
                else if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefTitle"))
                {//數字欄位需靠右對齊
                    tb = (TextBox)DetailsView1.Rows[i].Cells[1].Controls[0];
                    tb.MaxLength = 10;
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefID"))
                {//數字欄位需靠右對齊
                    DropDownList DDL = new DropDownList();
                    DataTable dt2 = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company='" + Request["Company"].Trim() + "' order by EmployeeId");
                    DDL.Items.Add(Li("無",""));
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                DDL.Items.Add(Li(dt2.Rows[j]["EmployeeId"].ToString() + " - " + dt2.Rows[j]["EmployeeName"].ToString(), dt2.Rows[j]["EmployeeId"].ToString()));
                            }
                        }
                    }
                    DDL.Style.Add("width", "150px");
                    tb.Visible = false;
                    tb.Style.Add("width", "1px");
                    DetailsView1.Rows[i].Cells[1].Controls.Add(DDL);
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count <= 0)
                        {
                            HyperLink lbtn = new HyperLink();
                            lbtn.Text = " 請先建立人事資料";
                            lbtn.Style.Add("color", "red");
                            //lbtn.Attributes.Add("onclick", "javascript:var win =window.open('../Main/PersonnelMaster_M.aspx','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                            DetailsView1.Rows[i].Cells[1].Controls.Add(lbtn);

                        }
                    }
                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

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

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
        //e.Values.

        if (e.Values["DepCode"] == null)
        {
            Err += "部門代號不可空白<br>";
        }



        DetailsView dv = (DetailsView)sender;
        for (int i = 0; i < dv.Rows.Count; i++)
        {
            #region ---下拉式選單---
            //switch (i)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        if (e.Values[i].ToString() == "" ||e.Values[i]==null )
            //        {
            //            Err += dv.Rows[i].Cells[0].Text + "不可空白<br>";
            //        }
            //        break;
            //    case 2:
            //        if (e.Values[i].ToString() == "")
            //        {
            //            Err += dv.Rows[i].Cells[0].Text + "不可空白<br>";
            //        }
            //        break;
            //    case 3:
            //        if (e.Values[i].ToString() == "")
            //        {
            //            Err += dv.Rows[i].Cells[0].Text + "不可空白<br>";
            //        }
            //        break;
            //    case 4:
            //        if (e.Values[i].ToString() == "")
            //        {
            //            Err += dv.Rows[i].Cells[0].Text + "不可空白<br>";
            //        }
            //        break;
            //    case 5:
            //        break;
            //    case 6:
            //        if (e.Values[i].ToString() == "")
            //        {
            //            Err += dv.Rows[i].Cells[0].Text + "不可空白<br>";
            //        }
            //        break;
            //    case 7:
            //    case 8:
            //        break;
            //}
            if (i == 0)
            {//下拉式選單
                DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values[i] = ddl.SelectedValue.Trim();
            }
            if (i == 5)
            {
                DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values[i] = ddl.SelectedValue.Trim();
            }
            if (i == 7)
            {
                DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values[i] = ddl.SelectedValue.Trim();
            }
            if (i == 8)
            {
                DropDownList ddl = (DropDownList)dv.Rows[i].Cells[1].Controls[1];
                e.Values[i] = ddl.SelectedValue.Trim();
            }
            if (!(i == 0 || i == 8))
            {
                if (e.Values[i] == null)
                {//將空欄位放入空白
                    e.Values[i] = " ";
                }
            }
            //}
            #endregion

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
            if (!ValidateData(e.Values["Company"].ToString(), e.Values["DepCode"].ToString()))//, e.Values["EmployeeId"].ToString()
            {
                Err += "代號重覆!!此公司已有此部門代號!<br>";
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Department";
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
            DropDownList ddl = (DropDownList)DetailsView1.Rows[0].Cells[1].Controls[1];
            string Com = ddl.SelectedValue.Trim();
            str = new StringBuilder();
            str.Append("<script language=javascript>");
            str.Append("window.opener.location='Department.aspx?Company=" + Com + "';");
            str.Append("window.close();");
            str.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", str.ToString());
        }
    }

    private bool ValidateData(string strCompany,string strDepCode)
    {
        Ssql = "Select * From Department Where Company='" + strCompany + "' And DepCode='" + strDepCode + "'";//  And EmployeeId='" + EmployeeId + "'
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
