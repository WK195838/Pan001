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

public partial class Basic_PersonalShift : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM075";
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
        if (_UserInfo.SysSet.isTWCalendar)
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/LaborInsuranceRateParameter.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
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
                    GridView1.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    GridView1.ShowFooter = Find;
                }
            }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            {
                Find = true;
            }
            else
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            //版面樣式調整
            if (SetCss == false)
            {
                GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        Navigator1.BindGridView = GridView1;
        CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged(CompanyList1_SelectedIndex);
        DepList1.SelectedIndexChanged += new EventHandler(DepList1_SelectedIndexChanged);
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

            //YearList1.initList();
            
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            #region ---根據postback回來的不同 給予不同的功能---
            if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
            {
                BindData();
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$ddl0";
                if (Request.Form[ddlId + "1"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
            }
            else
            {
                if (!Request.Form["__EVENTTARGET"].ToString().Contains("GridView1$ctl01$ddl01"))
                {
                    if (Request.Form["__EVENTTARGET"].ToString() == "" && Request.Form["__EVENTARGUMENT"].ToString() == "")
                    {

                    }
                    else
                    {
                        BindData();
                    }
                }
            }
            #endregion
        }

        //btnCalendar1.Attributes.Add("onclick", "return GetPromptDate(" + txtDate.ClientID + ");");
        //btnSetWeekend.Attributes.Add("onclick", "return confirm('原年度若有設定之假日將被清空!是否確定新增 '+document.getElementById('" + YearList1.YListClientID() + "').value+' 年度週末假日?');");

    }

    private void showPanel()
    {
        if (hasData())
        {
            Panel_Empty.Visible = false;
           
        }
        else
        {
            if (CompanyList1.SelectValue == "")
            {
                Panel_Empty.Visible = false;
          
            }
            else
            {
                Panel_Empty.Visible = true;
             
            }
        }
    }
    void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        #region 部門選單
        DepList1.Items.Clear();
        DepList1.Items.Insert(0, Li("全部", ""));
        for (int i = 0; i < e.Department_Basic.Count; i++)
        {
            DepList1.Items.Add(e.Department_Basic[i]);
        }
        DepList1.DataBind();
        #endregion

        #region 員工選單

            EmployeeIdList1.Items.Clear();
            EmployeeIdList1.Items.Insert(0, Li("全部", ""));

            for (int i = 0; i < e.Personnel_Master.Count; i++)
            {
                EmployeeIdList1.Items.Add(e.Personnel_Master[i]);
            }
            EmployeeIdList1.DataBind();
        #endregion

        BindData();
        showPanel();

    }
    void DepList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region 員工選單
        DataTable DT = _MyDBM.ExecuteDataTable("select DeptId,EmployeeId,EmployeeName from Personnel_Master where Company = '" + CompanyList1.SelectValue + "' ");
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0, Li("全部", ""));
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            string[] Employee = { DT.Rows[i]["DeptId"].ToString(), DT.Rows[i]["EmployeeId"].ToString(), DT.Rows[i]["EmployeeName"].ToString() };
            if (((DropDownList)sender).SelectedItem.Text != "全部")
            {

                if (DT.Rows.Count > 0 && ((DropDownList)sender).SelectedItem.Value == Employee[0])
                {
                    EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2], Employee[1]));
                }
            }
            else
                EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2], Employee[1]));
        }
        EmployeeIdList1.DataBind();
        BindData();
        showPanel();
        #endregion
    }
    private bool hasData()
    {
        Ssql = "SELECT * FROM PersonalShift Where 0=0";
        if (CompanyList1.SelectValue == "")
        {
            Ssql += string.Format(" And Company='%{0}%'", CompanyList1.SelectValue);
        }
        else
        {
            Ssql += string.Format(" And Company like '%{0}%'", CompanyList1.SelectValue);
        }
        if (DepList1.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And DeptId like '%{0}%'", DepList1.SelectedValue);
        }
        if (EmployeeIdList1.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And EmployeeId like '%{0}%'", EmployeeIdList1.SelectedValue.Trim());
        }
        MyCmd.Parameters.Clear();
        //MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 0;
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql, MyCmd.Parameters, CommandType.Text);

        if (tb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void BindData()
    {
        Ssql = "SELECT * FROM PersonalShift Where 0=0";
        if (CompanyList1.SelectValue == "")
        {
            Ssql += string.Format(" And Company='%{0}%'", CompanyList1.SelectValue);
        }
        else
        {
            Ssql += string.Format(" And Company like '%{0}%'", CompanyList1.SelectValue);
        }
        if (DepList1.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And DeptId like '%{0}%'", DepList1.SelectedValue);
        }
        if (EmployeeIdList1.SelectedValue.Length>0)
        {
            Ssql += string.Format(" And EmployeeId like '%{0}%'", EmployeeIdList1.SelectedValue.Trim());
        }
        SDS_GridView.SelectCommand = Ssql;

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        BindData();
        showPanel();
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hid_Company.Value = GridView1.SelectedDataKey.Values["Company"].ToString();
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["LaborInsurance_RateParameterDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
        string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$ddl0";

        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()))
            {
                return;
            }
            if (CompanyList1.SelectValue == "")
            {
                lbl_Msg.Text = "請先選擇公司";
            }
            int DStart = int.Parse(_UserInfo.SysSet.ToADDate(Request.Form[temId + "2"].ToString()).Replace("/", ""));
            int DEnd = int.Parse(_UserInfo.SysSet.ToADDate(Request.Form[temId + "3"].ToString()).Replace("/", ""));
            if (DEnd < DStart)
            {
                lbl_Msg.Text = "迄止日期要大於開始日期";
            }
            if (lbl_Msg.Text == "")
            {
                //新增
                if (ValidateData(CompanyList1.SelectValue.ToString(), Request.Form[ddlId + "1"].ToString(), _UserInfo.SysSet.ToADDate(Request.Form[temId + "2"].ToString())))
                {
                    if (ValidateDate(CompanyList1.SelectValue.ToString(), Request.Form[ddlId + "1"].ToString(), _UserInfo.SysSet.ToADDate(Request.Form[temId + "2"].ToString()), _UserInfo.SysSet.ToADDate(Request.Form[temId + "3"].ToString())))
                    {
                        SDS_GridView.InsertParameters.Clear();
                        SDS_GridView.InsertParameters.Add("Company", CompanyList1.SelectValue.ToString());
                        if (DepList1.SelectedValue.Length > 0)
                        {
                            SDS_GridView.InsertParameters.Add("DeptId", DepList1.SelectedValue.ToString().Trim());
                        }
                        else
                        {
                            if (DBSetting.PersonalDateList(CompanyList1.SelectValue.ToString(), Request.Form[ddlId + "1"].ToString().Trim()).DeptId.ToString() != "")
                            {

                                SDS_GridView.InsertParameters.Add("DeptId", DBSetting.PersonalDateList(CompanyList1.SelectValue.ToString(), Request.Form[ddlId + "1"].ToString().Trim()).DeptId.ToString());
                            }
                        }
                        SDS_GridView.InsertParameters.Add("EmployeeId", Request.Form[ddlId + "1"].ToString().Trim());
                        SDS_GridView.InsertParameters.Add("DateStart", _UserInfo.SysSet.ToADDate(Request.Form[temId + "2"].ToString()));
                        SDS_GridView.InsertParameters.Add("DateEnd", _UserInfo.SysSet.ToADDate(Request.Form[temId + "3"].ToString()));
                        SDS_GridView.InsertParameters.Add("ShiftCode", Request.Form[ddlId + "4"].ToString());

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
                        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonalShift";
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
                            i = SDS_GridView.Insert();
                        }
                        catch (Exception ex)
                        {
                            lbl_Msg.Text = ex.Message;
                        }
                        if (i == 1)
                        {
                            lbl_Msg.Text = i.ToString() + " 個資料列 " + "新增成功!!";
                        }
                        else
                        {
                            lbl_Msg.Text = "新增失敗!!" + lbl_Msg.Text;
                        }

                        #region 完成異動後,更新LOG資訊
                        MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
                        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                        _MyDBM.DataChgLog(MyCmd.Parameters);
                        #endregion

                        BindData();
                        showPanel();
                    }
                    else
                    {
                        lbl_Msg.Text = "新增失敗!!  原因: 時間範圍重疊";
                        BindData();
                    }
                }
                else
                {
                    lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
                    BindData();
                }
                hid_IsInsertExit.Value = "";
            }
            else
            {
                BindData();
                hid_IsInsertExit.Value = "";
            }
        }
  
    }

    private bool ValidateData(string strCompany, string strEmployeeId,string strDateStart)
    {
        //判斷資料是否重覆
        Ssql = "Select DeptId,DateEnd,ShiftCode  From PersonalShift WHERE Company = '" + strCompany + "' and EmployeeId='" + strEmployeeId.Trim() + "' and convert(varchar,DateStart,111)='" + strDateStart + "' ";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }
    private bool ValidateDate(string strCompany, string strEmployeeId, string strDateStart, string strDateEnd)
    {
        int DateStart = int.Parse(strDateStart.Replace("/", ""));
        int DateEnd = int.Parse(strDateEnd.Replace("/", ""));
        Ssql = "Select DeptId,DateStart ,DateEnd  From PersonalShift WHERE Company = '" + strCompany + "' and EmployeeId='" + strEmployeeId.Trim() + "' and ((Convert(int,convert(varchar,DateStart,112))<='" + DateStart + "' and Convert(int,convert(varchar,DateEnd,112))>='" + DateStart + "') or (Convert(int,convert(varchar,DateStart,112))<='" + DateEnd + "' and Convert(int,convert(varchar,DateEnd,112))>='" + DateEnd + "')) ";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //#region 開始異動前,先寫入LOG
        ////TableName	異動資料表	varchar	60
        ////TrxType	異動類別(A/U/D)	char	1
        ////ChangItem	異動項目(欄位)	varchar	255
        ////SQLcommand	異動項目(異動值/指令)	varchar	2000
        ////ChgStartDateTime	異動開始時間	smalldatetime	
        ////ChgStopDateTime	異動結束時間	smalldatetime	
        ////ChgUser	使用者代號	nvarchar	32
        //DateTime StartDateTime = DateTime.Now;
        //MyCmd.Parameters.Clear();
        //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance_RateParameter";
        //MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        //MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        //MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        //MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        ////此時不設定異動結束時間
        ////MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        //MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        //_MyDBM.DataChgLog(MyCmd.Parameters);
        //#endregion
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        //if (e.Exception == null)
        //{
        //    lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";
        //    BindData();
        //    hid_Company.Value = "";
        //    hid_Date.Value = "";
        //}
        //else
        //{
        //    lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
        //    e.ExceptionHandled = true;            
        //}

        //#region 完成異動後,更新LOG資訊
        //MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        //_MyDBM.DataChgLog(MyCmd.Parameters);
        //#endregion

        //if (e.Exception != null)
        //    return;

        //showPanel();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string L3PK = btnDelete.Attributes["L3PK"].ToString();
        L3PK = _UserInfo.SysSet.FormatADDate(L3PK);
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonalShift";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "Delete From PersonalShift Where Company='" + L1PK + "' and EmployeeId='" + L2PK + "' and convert(varchar,DateStart,111)='" + L3PK + "' ";

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();
        showPanel();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
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
            }

            //i = e.Row.Cells.Count - 1;
            //if (i > 0)
            //{
            //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
            //    e.Row.Cells[i - 1].Style.Add("width", "100px");
            //}
            //e.Row.Cells[i].Style.Add("text-align", "left");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindData();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string Err = "", tempOldValue = "";
        string UpdateItem = "", UpdateValue = "";
        string ddlId = hid_Company.Value.Replace("_", "$") + "ddl0";
        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        for (int i = 0; i < 2; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();

                //日期欄位為KEY值不可修改
                if (i == 0)
                {//將日期欄位格式為化為西元日期
                    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                }

                //if (i == 0)
                //{
                //    //DropDownList ddl01=(DropDownList)
                //    //GridView1.SelectedRow.Cells[0].Text
                //    e.NewValues[i] = Request.Form[ddlId + "1"].ToString().Trim();
                //}

                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }

                if (e.NewValues[i].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += GridView1.HeaderRow.Cells[i + 3].Text.Trim() + "|";
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonalShift";
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


    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        if (e.Exception == null)
        {
            GridView1.EditIndex = -1;
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "更新成功!!";
            BindData();
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
            return;

        showPanel();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandName))
        {
            try
            {
                //GridViewRow gvRow = null;
                switch (e.CommandName)
                {
                    case "Delete"://刪除
                        break;
                    case "Update":
                        break;
                    case "Insert":
                        break;
                    case "Edit":
                    case "Cancel":
                    default:
                        lbl_Msg.Text = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_Company.ClientID + ",'" + e.Row.ClientID + "') ;");
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
                #endregion

                for (int i = 2; i < 6; i++)
                {
                    switch (i)
                    {
                        //case 2:
                        //    ((TextBox)e.Row.Cells[i].Controls[0]).Visible = false;
                        //    DataTable dt = _MyDBM.ExecuteDataTable("select DepCode,DepName from Department where Company='" + CompanyList1.SelectValue.ToString().Trim() + "' ");
                        //    DropDownList ddl01 = new DropDownList();
                        //    ddl01.ID = "ddl" + (i - 1).ToString().PadLeft(2, '0');
                        //    ddl01.Items.Clear();
                        //    if (dt != null)
                        //    {
                        //        if (dt.Rows.Count > 0)
                        //        {
                        //            for (int j = 0; j < dt.Rows.Count; j++)
                        //            {
                        //                ddl01.Items.Add(Li(dt.Rows[j]["DepCode"].ToString() + " - " + dt.Rows[j]["DepName"].ToString(), dt.Rows[j]["DepCode"].ToString()));
                        //            }
                        //        }
                        //    }
                        //    ddl01.SelectedValue = ((TextBox)e.Row.Cells[2].Controls[0]).Text;
                        //    e.Row.Cells[2].Controls.Add(ddl01);
                        //    break;
                        case 2:
                            if (DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim()) != "")
                            {
                                e.Row.Cells[2].Text = e.Row.Cells[2].Text.Trim() + " - " + DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim());
                            }
                            break;
                        case 3:
                            e.Row.Cells[3].Text = _UserInfo.SysSet.FormatDate(e.Row.Cells[3].Text);
                            break;
                        case 4:
                            //為日期欄位增加小日曆元件
                           ((TextBox)e.Row.Cells[i].Controls[0]).Text = _UserInfo.SysSet.FormatDate(((TextBox)e.Row.Cells[i].Controls[0]).Text);
                           ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("width", "70px");

                            ((TextBox)e.Row.Cells[i].Controls[0]).CssClass = "JQCalendar";
                            //ImageButton btOpenCal = new ImageButton();
                            //btOpenCal.ID = "btnCalendar" + (i - 1).ToString("D2");
                            //btOpenCal.SkinID = "Calendar1";
                            //btOpenCal.OnClientClick = "return GetPromptDate(" + ((TextBox)e.Row.Cells[i].Controls[0]).ClientID + ");";
                            //e.Row.Cells[i].Controls.Add(btOpenCal);
                            break;
                        case 5:
                            ((TextBox)e.Row.Cells[i].Controls[0]).Visible = false;
                            DropDownList ddl04 = new DropDownList();
                            ddl04.ID = "ddl" + (i - 1).ToString().PadLeft(2, '0');
                            ddl04.Items.Clear();
                            string sqlstr = "select ShiftCode,InTime,OutTime from SpecialShift where Company='" + CompanyList1.SelectValue.ToString().Trim() + "'";
                            DataTable dt4 = _MyDBM.ExecuteDataTable(sqlstr);
                            if (dt4 != null)
                            {
                                if (dt4.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt4.Rows.Count; j++)
                                    {
                                        ddl04.Items.Add(Li(dt4.Rows[j]["ShiftCode"].ToString().Trim() + " ( " + dt4.Rows[j]["InTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " ~ " + dt4.Rows[j]["OutTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " )", dt4.Rows[j]["ShiftCode"].ToString().Trim()));
                                    }
                                }
                            }
                            ddl04.SelectedValue = ((TextBox)e.Row.Cells[i].Controls[0]).Text;
                            e.Row.Cells[i].Controls.Add(ddl04);
                            break;
                    }
                }

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
                #endregion
                e.Row.Cells[3].Text = _UserInfo.SysSet.ToTWDate(e.Row.Cells[3].Text);
                e.Row.Cells[4].Text = _UserInfo.SysSet.ToTWDate(e.Row.Cells[4].Text);
                //if (DBSetting.DepartmentName(CompanyList1.SelectValue, e.Row.Cells[2].Text) != "")
                //{
                //    e.Row.Cells[2].Text = e.Row.Cells[2].Text + " - " + DBSetting.DepartmentName(CompanyList1.SelectValue, e.Row.Cells[2].Text);
                //}
                if (DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim()) != "")
                {
                    e.Row.Cells[2].Text = e.Row.Cells[2].Text.Trim() + " - " + DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[2].Text.Trim());
                }
                if (e.Row.Cells[5].Text != null)
                {
                    string sqlstr = "select InTime,OutTime from SpecialShift where Company='" + CompanyList1.SelectValue.ToString().Trim() + "' and ShiftCode='"+e.Row.Cells[5].Text.Trim()+"'";
                    DataTable dt4 = _MyDBM.ExecuteDataTable(sqlstr);
                    if (dt4 != null)
                    {
                        if (dt4.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                e.Row.Cells[5].Text = e.Row.Cells[5].Text.Trim() + " ( " + dt4.Rows[j]["InTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " ~ " + dt4.Rows[j]["OutTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " )";
                            }
                        }
                    }
                }
                //for (int j = 3; j < 9; j++)
                //{
                //    e.Row.Cells[j].Style.Add("text-align", "right");
                //}
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (!(i == 2 || i == 5))
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    switch (i)
                    {
                        case 3:
                        case 4:
                            //為日期欄位增加小日曆元件
                            tbAddNew.Style.Add("width", "70px");
                            tbAddNew.CssClass = "JQCalendar";
                            //e.Row.Cells[i].Controls.Add(tbAddNew);                            
                            //ImageButton btOpenCal = new ImageButton();
                            //btOpenCal.ID = "btnCalendar" + (i - 1).ToString("D2");
                            //btOpenCal.SkinID = "Calendar1";
                            //btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
                            //e.Row.Cells[i].Controls.Add(btOpenCal);
                            break;
                        case 5:
                            tbAddNew.MaxLength = 10;
                            tbAddNew.Style.Add("width", "70px");
                            e.Row.Cells[i].Controls.Add(tbAddNew);
                            break;



                    }
                    //e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
                //if(i==2)
                //{
                //    DataTable dt = _MyDBM.ExecuteDataTable("select DepCode,DepName from Department where Company='" + CompanyList1.SelectValue.ToString().Trim() + "' ");
                //    DropDownList ddl01 = new DropDownList();
                //    ddl01.ID = "ddl" + (i - 1).ToString().PadLeft(2, '0');
                //    ddl01.Items.Clear();
                //    ddl01.Items.Insert(0, Li("請選擇", ""));
                //    if (dt != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            for (int j = 0; j < dt.Rows.Count; j++)
                //            {
                //                ddl01.Items.Add(Li(dt.Rows[j]["DepCode"].ToString() + " - " + dt.Rows[j]["DepName"].ToString(), dt.Rows[j]["DepCode"].ToString()));
                //            }
                //        }
                //    }
                //    ddl01.AutoPostBack = true;
                //    ddl01.SelectedIndexChanged += new EventHandler(ddl01_SelectedIndexChanged);
                //    e.Row.Cells[i].Controls.Add(ddl01);
                //    strValue += "checkColumns(" + ddl01.ClientID + ") && ";
                //}
                if (i == 2)
                {
                    DropDownList ddl01 = new DropDownList();
                    ddl01.ID = "ddl" + (i - 1).ToString().PadLeft(2, '0');
                    ddl01.Items.Clear();
                    ddl01.Items.Insert(0, Li("請選擇", ""));
                    string sqlstr = "select EmployeeId,EmployeeName from Personnel_Master where Company='" + CompanyList1.SelectValue.ToString().Trim() + "'";
                    if (DepList1.SelectedValue.Length > 0)
                    {
                        sqlstr += " and DeptId='" + DepList1.SelectedValue.ToString().Trim() + "'";
                    }
                    DataTable dt2 = _MyDBM.ExecuteDataTable(sqlstr);
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ddl01.Items.Add(Li(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
                            }
                        }
                    }
                    e.Row.Cells[i].Controls.Add(ddl01);
                    strValue += "checkColumns(" + ddl01.ClientID + ") && ";
                }
                if (i == 5)
                {
                    DropDownList ddl04 = new DropDownList();
                    ddl04.ID = "ddl" + (i - 1).ToString().PadLeft(2, '0');
                    ddl04.Items.Clear();
                    ddl04.Items.Insert(0, Li("請選擇", ""));
                    string sqlstr = "select ShiftCode,InTime,OutTime from SpecialShift where Company='" + CompanyList1.SelectValue.ToString().Trim() + "'";
                    DataTable dt4 = _MyDBM.ExecuteDataTable(sqlstr);
                    if (dt4 != null)
                    {
                        if (dt4.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                ddl04.Items.Add(Li(dt4.Rows[j]["ShiftCode"].ToString().Trim() + " ( " + dt4.Rows[j]["InTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " ~ " + dt4.Rows[j]["OutTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " )", dt4.Rows[j]["ShiftCode"].ToString().Trim()));
                            }
                        }
                    }
                    e.Row.Cells[i].Controls.Add(ddl04);
                    strValue += "checkColumns(" + ddl04.ClientID + ") && ";
                }


                //if (i == 3)
                //{//為日期欄位增加小日曆元件
                //    ImageButton btOpenCal = new ImageButton();
                //    btOpenCal.ID = "btOpenCal";
                //    btOpenCal.SkinID = "Calendar1";
                //    btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
                //    e.Row.Cells[i].Controls.Add(btOpenCal);
                //}
            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";

            for (int i = 1; i < 5; i++)
            {
                if (!(i == 1 || i == 4))
                {
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                    switch (i)
                    {
                        case 2:
                        case 3:
                            //為日期欄位增加小日曆元件
                            tbAddNew.Style.Add("width", "70px");
                            tbAddNew.CssClass = "JQCalendar";
                            //ImageButton btnCalendar = (ImageButton)e.Row.FindControl("btnCalendar" + i.ToString("D2"));
                            //if (btnCalendar != null)
                            //    btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
                            break;
                        case 4:
                            tbAddNew.MaxLength = 10;
                            tbAddNew.Style.Add("width", "70px");
                            break;
                    }
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
                #region
                //if (i == 1)
                //{
                //    DropDownList ddl01 = new DropDownList();
                //    ddl01 = (DropDownList)e.Row.FindControl("ddl01");
                //    ddl01.Items.Clear();
                //    ddl01.Items.Insert(0, Li("請選擇", ""));
                //    DataTable dt = _MyDBM.ExecuteDataTable("select DepCode,DepName from Department where Company='"+CompanyList1.SelectValue.ToString().Trim()+"' ");
                //    if (dt != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            for (int j = 0; j < dt.Rows.Count; j++)
                //            {
                //                ddl01.Items.Add(Li(dt.Rows[j]["DepCode"].ToString() + " - " + dt.Rows[j]["DepName"].ToString(), dt.Rows[j]["DepCode"].ToString()));
                //            }
                //        }
                //    }
                //    ddl01.AutoPostBack = true;
                //    strValue += "checkColumns(" + ddl01.ClientID + ") && ";
                //}
                #endregion
                if (i == 1)
                {
                    DropDownList ddl01 = new DropDownList();
                    ddl01 = (DropDownList)e.Row.FindControl("ddl01");
                    ddl01.Items.Clear();
                    ddl01.Items.Insert(0, Li("請選擇", ""));
                    string sqlstr = "select EmployeeId,EmployeeName from Personnel_Master where Company='" + CompanyList1.SelectValue.ToString().Trim() + "'";
                    if (DepList1.SelectedValue.Length > 0)
                    {
                        sqlstr += " and DeptId='" + DepList1.SelectedValue.ToString().Trim() + "'";
                    }
                    DataTable dt2 = _MyDBM.ExecuteDataTable(sqlstr);
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ddl01.Items.Add(Li(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
                            }
                        }
                    }
                    strValue += "checkColumns(" + ddl01.ClientID + ") && ";
                }
                if (i == 4)
                {
                    DropDownList ddl04 = new DropDownList();
                    ddl04 = (DropDownList)e.Row.FindControl("ddl04");
                    ddl04.Items.Clear();
                    ddl04.Items.Insert(0, Li("請選擇", ""));
                    string sqlstr = "select ShiftCode,InTime,OutTime from SpecialShift where Company='" + CompanyList1.SelectValue.ToString().Trim() + "'";
                    DataTable dt4 = _MyDBM.ExecuteDataTable(sqlstr);
                    if (dt4 != null)
                    {
                        if (dt4.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                ddl04.Items.Add(Li(dt4.Rows[j]["ShiftCode"].ToString().Trim() + " ( " + dt4.Rows[j]["InTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " ~ " + dt4.Rows[j]["OutTime"].ToString().Trim().PadLeft(4, '0').Insert(2, " : ") + " )", dt4.Rows[j]["ShiftCode"].ToString().Trim()));
                            }
                        }
                    }
                    strValue += "checkColumns(" + ddl04.ClientID + ") && ";
                }
                //if (i == 2)
                //{//為日期欄位增加小日曆元件
                //    ImageButton btnCalendar = (ImageButton)e.Row.FindControl("btnCalendar");
                //    if (btnCalendar != null)
                //        btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
                //}
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

    //void ddl01_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string temp = ((DropDownList)sender).SelectedValue.ToString();
    //    string sqlstr="select EmployeeId,EmployeeName from Personnel_Master where Company='"+CompanyList1.SelectValue.ToString().Trim()+"' and DeptId='"+temp.Trim()+"'";
    //    DataTable dt2 = _MyDBM.ExecuteDataTable(sqlstr);
    //    DropDownList ddl02 = (DropDownList)GridView1.FooterRow.FindControl("ddl02");
    //    ddl02.Items.Clear();
    //    ddl02.Items.Insert(0,Li("請選擇", ""));
    //    if (dt2 != null)
    //    {
    //        if (dt2.Rows.Count > 0)
    //        {
    //            for (int j = 0; j < dt2.Rows.Count; j++)
    //            {
    //                ddl02.Items.Add(Li(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
    //            }
    //        }
    //    }
    //}
    //protected void ddl01_SelectedChanged(object sender, EventArgs e)
    //{
    //    DropDownList temp = ((DropDownList)sender);
    //    string sqlstr = "select EmployeeId,EmployeeName from Personnel_Master where Company='" + CompanyList1.SelectValue.ToString().Trim() + "' and DeptId='" + temp.SelectedValue.ToString().Trim() + "'";
    //    DataTable dt2 = _MyDBM.ExecuteDataTable(sqlstr);
    //    DropDownList ddl02 = (DropDownList)temp.NamingContainer.FindControl("ddl02");
    //    ddl02.Items.Clear();
    //    ddl02.Items.Insert(0,Li("請選擇", ""));
    //    if (dt2 != null)
    //    {
    //        if (dt2.Rows.Count > 0)
    //        {
    //            for(int j=0;j<dt2.Rows.Count;j++)
    //            {
    //                ddl02.Items.Add(Li(dt2.Rows[j]["EmployeeId"].ToString().Trim() + " - " + dt2.Rows[j]["EmployeeName"].ToString().Trim(), dt2.Rows[j]["EmployeeId"].ToString().Trim()));
    //            }
    //        }
    //    }

    //}
    private ListItem Li(string text, string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value;
        return li;
    }
}
