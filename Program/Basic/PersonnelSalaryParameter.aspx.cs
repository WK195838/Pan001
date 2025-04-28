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

public partial class PersonnelSalaryParameter : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB018";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCategory = "01", sCompany = "";

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'ePayroll' And RTrim(ProgramPath)='Basic/PersonnelSalaryParameter.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }
        
    protected void Page_Load(object sender, EventArgs e)
    {
        bool blInsertMod = false;
        
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");              

        #region 權限控管
        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
        }
                
        if ((ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;

            blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, (blInsertMod ? "Add" : "Modify"));
            if (blCheckProgramAuth)
            {
                DetailsView1.DefaultMode = (blInsertMod ? DetailsViewMode.Insert : DetailsViewMode.Edit);
            }
            else
            {
                DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            }            
        }
        #endregion

        sCompany = CompanyList1.SelectValue.Trim();
        if (String.IsNullOrEmpty(sCompany))
        {
            lbl_Msg.Text = "";
            btnSaveGo.Visible = false;
            btnEdit.Visible = false;
            btnCancel.Visible = false;
            DetailsView1.Visible = false;
        }
        else
        {
            btnCancel.Visible = true;
            DetailsView1.Visible = true;
            DetailsView1.DefaultMode = DetailsViewMode.Insert;
            BindData();
        }

        //標題列不顯示回上一頁
        StyleTitle0.ShowBackToPre = false;                
    }

    private void BindData()
    {
        Ssql = "SELECT * FROM PersonnelSalary_Parameter WHERE Category='" + sCategory + "' ";
        Ssql += string.Format(" And Company = '{0}'", CompanyList1.SelectValue);
        
        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        if (theDT.Rows.Count == 0)
        {
            DetailsView1.DefaultMode = DetailsViewMode.Insert;
            if (DetailsView1.CurrentMode != DetailsViewMode.Insert)
                DetailsView1.ChangeMode(DetailsViewMode.Insert);
            btnSaveGo.Visible = true;
            btnEdit.Visible = false;
            lbl_Msg.Text = "";
        }
        else
        {
            DetailsView1.DefaultMode = DetailsViewMode.Edit;
            if (DetailsView1.CurrentMode != DetailsViewMode.Edit)
                DetailsView1.ChangeMode(DetailsViewMode.Edit);
            btnSaveGo.Visible = false;
            btnEdit.Visible = true;
        }

        DetailsView1.DataSourceID = "";
        DetailsView1.DataSource = theDT;
        DetailsView1.DataBind();
    }


    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        DetailsView thisDv = (DetailsView)sender;
        if (thisDv.Rows.Count == 0)
        {
            return;
        }
        DetailsViewRow thisDvRow = thisDv.Rows[0];
        Ssql = "Select Top 1 * From PersonnelSalary_Parameter ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempValue = "";

        if (dtTitle.Columns.Count > 0)
        {
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                string tempTitle = dtTitle.Columns[i].ColumnName.Trim();
                Ssql = "Select dbo.GetColumnTitle('PersonnelSalary_Parameter','" + tempTitle + "')";

                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

                if (DT.Rows.Count > 0)
                {
                    if (tempTitle.Equals("Category") || (tempTitle.Equals("Company")) || (tempTitle.Contains("AL_Para")))
                    {
                        //
                    }
                    else
                    {
                        //欄位標題名稱
                        Label lb = (Label)thisDvRow.FindControl("lblTitle_" + tempTitle);
                        if (lb != null)
                        {
                            lb.Text = DT.Rows[0][0].ToString().Trim();
                            TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);

                            if ((thisDv.CurrentMode == DetailsViewMode.Edit) && (tempTitle.Equals("Company") || tempTitle.Equals("EmployeeId")))
                            {//KEY值設為唯讀
                                tb.ReadOnly = true;
                            }

                            switch (thisDv.CurrentMode)
                            {
                                case DetailsViewMode.Edit://修改模式
                                case DetailsViewMode.Insert://新增模式
                                    if (tb != null)
                                    {
                                        //指定欄位寬度
                                        tb.Style.Add("width", "100px");

                                        if (tempTitle.Equals("DepositBank") || tempTitle.Equals("PYBankBranch")
                                            || tempTitle.Equals("PYBankAccount") || tempTitle.Equals("BankCustId"))
                                        {//文字欄位若空白需預設為空格
                                            if (tempValue.Length == 0)
                                                tempValue = " ";
                                        }
                                        else
                                        {//數字欄位需靠右對齊
                                            tb.Style.Add("text-align", "right");
                                            //預設為0
                                            if (tempValue.Length == 0)
                                                tempValue = "0";
                                        }

                                        if (tempTitle.Equals("PY_Para7") || tempTitle.Equals("PY_Para8") || tempTitle.Equals("PY_Para9"))
                                        {
                                            try
                                            {
                                                tb.Text = Convert.ToDecimal(tb.Text).ToString("n0");
                                            }
                                            catch { }
                                        }
                                    }
                                    break;
                                case DetailsViewMode.ReadOnly://唯讀模式
                                    //指定欄位寬度
                                    Label tempLabel = ((Label)thisDvRow.FindControl("lbl_" + tempTitle));
                                    tempLabel.Style.Add("width", "100px");
                                    if (tempTitle.Equals("PY_Para7") || tempTitle.Equals("PY_Para8") || tempTitle.Equals("PY_Para9"))
                                    {
                                        try
                                        {
                                            tempLabel.Text = Convert.ToDecimal(tempLabel.Text).ToString("n0");
                                        }
                                        catch { }
                                    }
                                    
                                    if (!(tempTitle.Equals("DepositBank") || tempTitle.Equals("PYBankBranch") || tempTitle.Equals("PYBankAccount")))
                                    {//數字欄位需靠右對齊
                                        //Label會轉成SPAN無法對齊,只能塞空白=>"&nbsp;&nbsp;&nbsp;&nbsp;",所以改為在介面設定
                                        tempLabel.Style.Add("text-align", "right");
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        DetailsView1.DataSource = null;
        DetailsView1.DataSourceID = "SqlDataSource1";
        switch (DetailsView1.CurrentMode)
        {
            case DetailsViewMode.Insert:
                DetailsView1.InsertItem(true);
                BindData();
                break;
            case DetailsViewMode.Edit:
                DetailsView1.UpdateItem(true);
                BindData();
                break;
        }
    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        string Err = "";
        string InsertItem = "", InsertValue = "";
        string sqlItem = "", sqlValue = "";

        //Request.Form["ctl00$cphEEOC$DetailsView1$txt_OT_Para1"]
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        Ssql = "Select Top 1 * From PersonnelSalary_Parameter ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "", tempValue = "";
        InsertItem = "ALL";
                
        if (dtTitle.Columns.Count > 0)
        {
            SqlDataSource1.InsertParameters.Clear();
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();

                if (!(tempTitle.Contains("AL_Para")))
                {//特休參數不在此維護
                    if (tempTitle.Equals("Category"))
                    {
                        tempValue = sCategory;
                    }
                    else if (tempTitle.Equals("Company"))
                    {
                        tempValue = CompanyList1.SelectValue;
                    }
                    else
                    {
                        if (Request.Form["ctl00$cphEEOC$DetailsView1$txt_" + tempTitle] != null)
                        {
                            tempValue = Request.Form["ctl00$cphEEOC$DetailsView1$txt_" + tempTitle].ToString().Trim();
                        }

                        //TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);

                        //if (tb.Text.Trim().Length == 0)
                        //{
                        //    Err = ((Label)thisDvRow.FindControl("lblTitle_" + tempTitle)).Text + "不可空白<br>";
                        //}
                        //else
                        {
                            //tempValue = tb.Text.Trim();

                            if (tempTitle.Equals("DepositBank") || tempTitle.Equals("PYBankBranch") || tempTitle.Equals("PYBankAccount"))
                            {//文字欄位若空白需預設為空格
                                if (tempValue.Length == 0)
                                    tempValue = " ";
                            }
                            else
                            {//數字欄位若空白需預設為0
                                if (tempValue.Length == 0)
                                    tempValue = "0";
                                else
                                    tempValue = tempValue.Replace(",", "");
                            }
                        }
                    }
                    InsertValue += tempValue.Trim() + "|";
                    SqlDataSource1.InsertParameters.Add(tempTitle, tempValue);
                    sqlItem += (sqlItem.Length > 0 ? "," : "") + tempTitle;
                    sqlValue += (sqlValue.Length > 0 ? ",@" : "@") + tempTitle;
                }
            }
        }

        if (Err.Equals(""))
        {
            if (sqlItem.Length * sqlValue.Length > 0)
                SqlDataSource1.InsertCommand = "INSERT INTO [PersonnelSalary_Parameter] (" + sqlItem + ") Select " + sqlValue;
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonnelSalary_Parameter";
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
        string tempSQL = "";

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

//        BindData();
    }

    private bool ValidateData(string Category, string Company)
    {
        Ssql = "Select * From PersonnelSalary_Parameter Where Category='" + Category + "' And Company='" + Company + "'";
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
        string Err = "";
        string UpdateItem = "", UpdateValue = "", tempValue = "";
        string sqlItem = "", sqlWhere = " (Category=@Category And Company = @Company) ";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";
        
        DetailsViewRow thisDvRow = ((DetailsView)sender).Rows[0];
        Ssql = "Select Top 1 * From PersonnelSalary_Parameter ";
        DataTable dtTitle = _MyDBM.ExecuteDataTable(Ssql);
        string tempTitle = "";
        UpdateItem = "ALL";

        if (dtTitle.Rows.Count > 0)
        {
            SqlDataSource1.UpdateParameters.Clear();
            for (int i = 0; i < dtTitle.Columns.Count; i++)
            {//修改欄位顯示
                tempTitle = dtTitle.Columns[i].ColumnName.Trim();

                if (!(tempTitle.Contains("AL_Para")))
                {//特休參數不在此維護
                    if (tempTitle.Equals("Category"))
                    {
                        tempValue = sCategory;
                    }
                    else if (tempTitle.Equals("Company"))
                    {
                        tempValue = CompanyList1.SelectValue;
                    }
                    else
                    {
                        if (Request.Form["ctl00$cphEEOC$DetailsView1$txt_" + tempTitle] != null)
                        {
                            tempValue = Request.Form["ctl00$cphEEOC$DetailsView1$txt_" + tempTitle].ToString().Trim();
                        }

                        //TextBox tb = (TextBox)thisDvRow.FindControl("txt_" + tempTitle);

                        //tempValue = tb.Text.Trim();

                        if (tempTitle.Equals("DepositBank") || tempTitle.Equals("PYBankBranch") || tempTitle.Equals("PYBankAccount"))
                        {//文字欄位若空白需預設為空格
                            if (tempValue.Length == 0)
                                tempValue = " ";
                        }
                        else
                        {//數字欄位若空白需預設為0
                            if (tempValue.Length == 0)
                                tempValue = "0";
                            else
                                tempValue = tempValue.Replace(",", "");
                        }
                    }
                    UpdateValue += tempValue + "|";
                    SqlDataSource1.UpdateParameters.Add(tempTitle, tempValue);
                    sqlItem += (sqlItem.Length > 0 ? "," : "") + tempTitle + "=@" + tempTitle;                    
                }
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
            if (sqlItem.Length * sqlWhere.Length > 0)
                SqlDataSource1.UpdateCommand = "Update [PersonnelSalary_Parameter] Set " + sqlItem + " Where " + sqlWhere;

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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonnelSalary_Parameter";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (UpdateItem.Length * 2 > 255) ? "長度:" + UpdateItem.Length.ToString() : UpdateItem;
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            //SqlDataSource1.Update();
        }
    }
    
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        StringBuilder str;

        if (e.Exception == null)
        {
            lbl_Msg.Text = "更新成功!!";
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
    }

    protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
    {
        switch (e.CommandName)
        { 
            case "Cancel":
                lbl_Msg.Text = "";
                break;
        }
    }
}
