﻿using System;
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

public partial class Basic_PayrollControl : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB005";
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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PayrollControl.aspx'");
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        lbl_Msg.Text = "";//清空訊息

        Navigator1.BindGridView = GridView1;

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
            if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
                string temId = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[temId + "$tbAddNew01"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
            }
            else
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
            Panel_Empty.Visible = true;
        }
    }

    private bool hasData()
    {
        Ssql = "SELECT * FROM PayrollControl Where 0=0";
        //if (txtDate.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", _UserInfo.SysSet.GetADDate(txtDate.Text));
        //}
        //Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", YearList1.SelectADYear);

        if (txtCompany.Text.Length > 0)
        {
            Ssql += string.Format(" And Company like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCompany.Text).ToString());
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
        Ssql = "SELECT * FROM PayrollControl Where 0=0";
        //if (txtDate.Text.Length > 0)
        //{
        //    try
        //    {
        //        string theDate = _UserInfo.SysSet.ToADDate(txtDate.Text);
        //        if (!theDate.Contains("DateTime"))
        //            txtDate.Text = theDate;
        //    }
        //    catch 
        //    { 
        //    }
        //    Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", txtDate.Text);                        
        //}
        //else
        //Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", YearList1.SelectADYear);

        if (txtCompany.Text.Length > 0)
        {
            Ssql += string.Format(" And Company like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCompany.Text).ToString());
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
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["HolidayDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
        string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$YearMonth0";

        //新增資料
        //if (String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()))
        //{
        //    return;
        //}
        //string test = Convert.ToDateTime(Request.Form[ddlId + "2"]).ToString();
        //Request.Form[ddlId + "2"].GetType();
       
        //新增
        if (ValidateData(Request.Form[temId + "1"].ToString()))
        {
            SDS_GridView.InsertParameters.Clear();
            SDS_GridView.InsertParameters.Add("Company", Request.Form[temId + "1"].ToString());
            SDS_GridView.InsertParameters.Add("SalaryYM", Request.Form[ddlId + "2"].ToString() + Request.Form[ddlId + "3"].ToString());
            SDS_GridView.InsertParameters.Add("DraftDate", _UserInfo.SysSet.ToADDate(Request.Form[temId + "3"].ToString()));
            SDS_GridView.InsertParameters.Add("ConfirmDate", _UserInfo.SysSet.ToADDate(Request.Form[temId + "4"].ToString()));
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PayrollControl";
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
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            BindData();
        }
        hid_IsInsertExit.Value = "";
    }

    private bool ValidateData(string strCompany)
    {
        //判斷資料是否重覆
        Ssql = "Select SalaryYM, DraftDate, ConfirmDate From PayrollControl WHERE Company = '" + strCompany + "'";

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
        //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Holiday";
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PayrollControl";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "Delete From PayrollControl Where Company='" + L1PK + "'";

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
            //e.Row.Cells[i].Style.Add("text-align", "right");
            //e.Row.Cells[i].Style.Add("width", "100px");

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
        string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$YearMonth0";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        for (int i = 0; i < 4; i++)
        {
            try
            {
                tempOldValue = e.OldValues[i].ToString().Trim();
                e.NewValues[i] = e.NewValues[i].ToString().Trim();

                //日期欄位為KEY值不可修改
                if (i == 1 || i == 2)
                {//將日期欄位格式為化為西元日期
                    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                }
                if (i == 0)
                {
                    //e.NewValues[i]=e.
                    e.NewValues[i] = Request.Form[ddlId + "2"].ToString() + Request.Form[ddlId + "3"].ToString();
                }


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
        string p = e.NewValues[0].ToString();
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PayrollControl";
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
                ////確認
                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');");

                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                ////取消
                //if (e.Row.Cells[1].Controls[2] != null)
                //{
                //    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                #endregion

                for (int i = 3; i < e.Row.Cells.Count; i++)
                {
                    try
                    {
                        TextBox tb = ((TextBox)e.Row.Cells[i].Controls[0]);
                        if (i == 4 || i == 5)
                        {
                            tb.Style.Add("text-align", "right");
                            tb.Style.Add("width", "70px");
                            if (tb.Text.Contains("/"))
                            {
                                tb.Text = _UserInfo.SysSet.FormatDate(Convert.ToDateTime(tb.Text).ToString("yyyy/MM/dd"));
                            }
                            if (i == 4 || i == 5)
                            {//為日期欄位增加小日曆元件
                                tb.CssClass = "JQCalendar";
                                //ImageButton btOpenCal = new ImageButton();
                                //btOpenCal.ID = "btOpenCal" + i.ToString();
                                //btOpenCal.SkinID = "Calendar1";
                                //btOpenCal.OnClientClick = "return GetPromptDate(" + e.Row.Cells[i].Controls[0].ClientID + ");";
                                //e.Row.Cells[i].Controls.Add(btOpenCal);
                            }

                        }
                        if (i == 3)
                        {
                            tb.Visible = false;
                            DropDownList ddlAddNew = new DropDownList();
                            ddlAddNew.ID = "YearMonth" + (i - 1).ToString().PadLeft(2, '0');
                            int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                            for (int n = (nowyear - 1); n < (nowyear + 11); n++)
                            {
                                string tmp = n.ToString("D2");
                                ddlAddNew.Items.Add(tmp);
                            }
                            ddlAddNew.SelectedValue = (int.Parse(tb.Text.Substring(0, 4)) - 1911).ToString();
                            e.Row.Cells[i].Controls.Add(ddlAddNew);
                            DropDownList ddlAddNew2 = new DropDownList();
                            ddlAddNew2.ID = "YearMonth" + i.ToString().PadLeft(2, '0');
                            for (int n = 1; n < 13; n++)
                            {
                                string tmp = n.ToString("D2");
                                ddlAddNew2.Items.Add(tmp);
                            }
                            ddlAddNew2.SelectedValue = tb.Text.Substring(4, 2).ToString();
                            e.Row.Cells[i].Controls.Add(ddlAddNew2);
                        }
                    }
                    catch { }
                }
                //string c = ;
            }
            else
            {
                #region 查詢用
                ////e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
             
                #endregion
                if (DBSetting.CompanyName(e.Row.Cells[0].Text).ToString() != "")
                {
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text + " - " + DBSetting.CompanyName(e.Row.Cells[0].Text).ToString();
                }
                e.Row.Cells[1].Text=(int.Parse(e.Row.Cells[1].Text.Substring(0,4).ToString())-1911).ToString()+e.Row.Cells[1].Text.Substring(4,2);
                if (e.Row.Cells[2].Text.Contains("/"))
                {
                    e.Row.Cells[2].Text = _UserInfo.SysSet.FormatDate(Convert.ToDateTime(e.Row.Cells[2].Text).ToString("yyyy/MM/dd"));
                }
                if (e.Row.Cells[3].Text.Contains("/"))
                {
                    e.Row.Cells[3].Text = _UserInfo.SysSet.FormatDate(Convert.ToDateTime(e.Row.Cells[3].Text).ToString("yyyy/MM/dd"));
                }   
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            
            strValue = "";
            e.Row.Visible = false;
            #region 新增用欄位
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (i == 2 || i == 4 || i == 5)
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    if (i == 4 || i == 5)
                    {
                        tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Style.Add("width", "70px");
                    }
                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                    if (i == 4 || i == 5)
                    {//為日期欄位增加小日曆元件
                        ImageButton btOpenCal = new ImageButton();
                        btOpenCal.ID = "btOpenCal" + i.ToString();
                        btOpenCal.SkinID = "Calendar1";
                        btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
                        e.Row.Cells[i].Controls.Add(btOpenCal);
                    }
                    if (i == 2)
                    {
                        ImageButton btOpenList = new ImageButton();
                        btOpenList.ID = "btOpen" + i.ToString();
                        btOpenList.SkinID = "OpenWin1";
                        //Company,CompanyShortName,CompanyName,ChopNo
                        btOpenList.OnClientClick = "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');";
                        e.Row.Cells[i].Controls.Add(btOpenList);
                    }
                }

             
                if (i == 3)
                {
                    DropDownList ddlAddNew = new DropDownList();
                    ddlAddNew.ID = "YearMonth" + (i - 1).ToString().PadLeft(2, '0');
                    int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                    for (int n = (nowyear - 1); n < (nowyear + 11); n++)
                    {
                        string tmp = n.ToString("D2");
                        ddlAddNew.Items.Add(tmp);
                    }
                    ddlAddNew.SelectedValue = nowyear.ToString();
                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
                    DropDownList ddlAddNew2 = new DropDownList();
                    ddlAddNew2.ID = "YearMonth" + i.ToString().PadLeft(2, '0');
                    for (int n = 1; n < 13; n++)
                    {
                        string tmp = n.ToString("D2");
                        ddlAddNew2.Items.Add(tmp);
                    }
                    ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));
                    e.Row.Cells[i].Controls.Add(ddlAddNew2);
                    strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
                }

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
            //e.Row.Visible = GridView1.ShowFooter;
            e.Row.Visible = false;
            #region 新增用欄位

            strValue = "";

            for (int i = 1; i < 5; i++)
            {
                if (i == 1 || i == 3 || i == 4)
                {
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                    if (i == 3 || i == 4)
                    {
                        tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Style.Add("width", "70px");
                    }
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

                    if (i == 3 || i == 4)
                    {//為日期欄位增加小日曆元件
                        ImageButton btnCalendar = (ImageButton)e.Row.FindControl("btnCalendar" + (i - 2).ToString());
                        if (btnCalendar != null)
                            btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
                    }
                    if (i == 1)
                    {
                        ImageButton btnOpen = (ImageButton)e.Row.FindControl("btnOpen" + i.ToString());
                        //btOpenList.SkinID = "OpenWin1";
                        //Company,CompanyShortName,CompanyName,ChopNo
                        if (btnOpen != null)
                         {
                             //btnOpen.SkinID = "OpenWin1";
                             btnOpen.Attributes.Add("onclick", "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');");
                         }
                        //e.Row.Cells[i].Controls.Add(btOpenList);
                    }
                }
                if (i == 2)
                {
                    DropDownList ddlAddNew = new DropDownList();
                    ddlAddNew = (DropDownList)e.Row.FindControl("YearMonth" + i.ToString().PadLeft(2, '0'));
                    int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
                    for (int n = (nowyear - 1); n < (nowyear + 11); n++)
                    {
                        string tmp = n.ToString("D2");
                        ddlAddNew.Items.Add(tmp);
                    }
                    ddlAddNew.SelectedValue = nowyear.ToString();
                    //e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
                    DropDownList ddlAddNew2 = new DropDownList();
                    ddlAddNew2 = (DropDownList)e.Row.FindControl("YearMonth" + (i + 1).ToString().PadLeft(2, '0'));
                    for (int n = 1; n < 13; n++)
                    {
                        string tmp = n.ToString("D2");
                        ddlAddNew2.Items.Add(tmp);
                    }
                    ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));

                    //e.Row.Cells[i].Controls.Add(ddlAddNew2);
                    strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

}
