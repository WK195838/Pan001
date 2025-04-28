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

public partial class Basic_UsuallyPayRoll : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM061";
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
            if (Request.Form["__EVENTARGUMENT"].ToString().Contains("Edit"))
            {
                if (Request.Form["__EVENTTARGET"].ToString() == "" && Request.Form["__EVENTARGUMENT"].ToString() == "")
                {

                }
                else
                {
                    BindData();
                }
            }
            else if (!string.IsNullOrEmpty(hid_IsInsertExit.Value))
            {
               
                string temId = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[temId + "$ddl01"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
            }
        }
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
        Ssql = "SELECT SalaryId,RegularPay FROM SalaryStructure_Parameter Where 0=0";

        if (txtSalaryId.Text.Length > 0)
        {
            Ssql += string.Format(" And SalaryId like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtSalaryId.Text).ToString());
        }

        MyCmd.Parameters.Clear();
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
        Ssql = "SELECT SalaryId,RegularPay FROM SalaryStructure_Parameter Where 0=0";

        if (txtSalaryId.Text.Length > 0)
        {
            Ssql += string.Format(" And SalaryId like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtSalaryId.Text).ToString());
        }

        SDS_GridView.SelectCommand = Ssql+ " And RegularPay='Y' order by SalaryId" ;

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
        //string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$lbl0";
        string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$ddl0";

        //新增資料
        //if (String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()))
        //{
        //    return;
        //}

        ////新增
        if (hid_IsInsertExit.Value != "")
        {
            string pay = "Y";
            SDS_GridView.UpdateParameters.Clear();
            SDS_GridView.UpdateParameters.Add("SalaryId", Request.Form[ddlId + "1"].ToString());
            SDS_GridView.UpdateParameters.Add("RegularPay", pay);
            


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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = Request.Form[ddlId + "1"].ToString();
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = pay;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            int i = 0;
            try
            {
                i = SDS_GridView.Update();
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = ex.Message;
            }
            if (i == 1)
            {
                lbl_Msg.Text = i.ToString() + " 個資料列 " + "更新成功!!";
            }
            else
            {
                lbl_Msg.Text = "更新失敗!!" + lbl_Msg.Text;
            }

            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            BindData();
            //showPanel();
        }
        else
        {
            BindData();
        }
        //hid_IsInsertExit.Value = "";
    }

    //private bool ValidateData(string strCompany)
    //{
    //    //判斷資料是否重覆
    //    Ssql = "Select SalaryYM, DraftDate, ConfirmDate From PayrollControl WHERE Company = '" + strCompany + "'";

    //    DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

    //    if (tb.Rows.Count > 0)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "UPDATE SalaryStructure_Parameter SET RegularPay = 'N' WHERE SalaryId='" + L1PK + "'";

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            lbl_Msg.Text = "資料更新成功 !!";

            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料更新失敗 !!";
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
    //protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView1.EditIndex = e.NewEditIndex;
    //    BindData();
    //}

    //protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView1.EditIndex = -1;
    //    BindData();
    //}
    //protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    string Err = "", tempOldValue = "";
    //    string UpdateItem = "", UpdateValue = "";
    //      string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$ddl0";
    //    UpdateValue = "(Key=";
    //    //將此筆資料的KEY值找出
    //    for (int i = 0; i < e.Keys.Count; i++)
    //    {
    //        e.Keys[i] = e.Keys[i].ToString().Substring(0, 2);
    //        UpdateValue += e.Keys[i].ToString().Trim() + "|";
    //    }
    //    UpdateValue += ")";

    //    for (int i = 0; i < 1; i++)
    //    {
    //        try
    //        {
    //            tempOldValue = e.OldValues[i].ToString().Trim();
    //            e.NewValues[i] = e.NewValues[i].ToString().Trim();

    //            //日期欄位為KEY值不可修改
    //            //if (i == 1 || i == 2)
    //            //{//將日期欄位格式為化為西元日期
    //            //    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
    //            //    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
    //            //}
    //            if (i == 0)
    //            {

    //                e.NewValues[i] = Request.Form[ddlId + "2"].ToString();
    //            }

    //            if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
    //            {//將空欄位放入半形空格
    //                e.NewValues[i] = " ";
    //            }

    //            if (e.NewValues[i].ToString().Trim() != tempOldValue)
    //            {
    //                try
    //                {
    //                    UpdateItem += GridView1.HeaderRow.Cells[i + 1].Text.Trim() + "|";
    //                    UpdateValue += e.OldValues[i].ToString().Trim() + "->" + e.NewValues[i].ToString().Trim() + "|";
    //                }
    //                catch
    //                { }
    //            }
    //        }
    //        catch
    //        { }
    //    }
    //    //string p = e.NewValues[0].ToString();
    //    #region 開始異動前,先寫入LOG
    //    //TableName	異動資料表	varchar	60
    //    //TrxType	異動類別(A/U/D)	char	1
    //    //ChangItem	異動項目(欄位)	varchar	255
    //    //SQLcommand	異動項目(異動值/指令)	varchar	2000
    //    //ChgStartDateTime	異動開始時間	smalldatetime	
    //    //ChgStopDateTime	異動結束時間	smalldatetime	
    //    //ChgUser	使用者代號	nvarchar	32
    //    DateTime StartDateTime = DateTime.Now;
    //    MyCmd.Parameters.Clear();
    //    MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryStructure_Parameter";
    //    MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
    //    MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = UpdateItem;
    //    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
    //    MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
    //    //此時不設定異動結束時間
    //    //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
    //    MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
    //    _MyDBM.DataChgLog(MyCmd.Parameters);
    //    #endregion
    //}


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
                if (e.Row.Cells[0].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[0].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');");

                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                //取消
                if (e.Row.Cells[0].Controls[2] != null)
                {
                    ImageButton IB = ((ImageButton)e.Row.Cells[0].Controls[2]);
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                #endregion

                //((TextBox)e.Row.Cells[2].Controls[0]).Visible = false;
                //DropDownList ddlAddNew = new DropDownList();
                //ddlAddNew.ID = "ddl02";
                //string[] selstr ={ "是", "否" };
                //string[] valstr ={ "Y", "N" };
                //for (int j = 0; j < 2; j++)
                //{
                //    ListItem li = new ListItem();
                //    li.Text = valstr[j].ToString() + "- " + selstr[j].ToString();
                //    li.Value = valstr[j].ToString();
                //    ddlAddNew.Items.Add(li);
                //}
                //ddlAddNew.SelectedValue = ((TextBox)e.Row.Cells[2].Controls[0]).Text.ToString();

                //e.Row.Cells[2].Controls.Add(ddlAddNew);

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
                //e.Row.Cells[4].Text = Convert.ToDateTime(e.Row.Cells[4].Text).ToString("yy/MM/dd");
                //e.Row.Cells[5].Text = Convert.ToDateTime(e.Row.Cells[5].Text).ToString("yy/MM/dd");
                #endregion
                Ssql = " SELECT SalaryId,SalaryName FROM SalaryStructure_Parameter where SalaryId='" + e.Row.Cells[1].Text + "'";
                DataTable dt = _MyDBM.ExecuteDataTable(Ssql);
                e.Row.Cells[1].Text = e.Row.Cells[1].Text + " - " + dt.Rows[0]["SalaryName"].ToString().Trim();
                e.Row.Cells[2].Text = e.Row.Cells[2].Text + " - 是";
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 1; i < 3; i++)
            {
                string Ssql2 = "SELECT SalaryId,SalaryName,RegularPay FROM SalaryStructure_Parameter Where RegularPay='N'";
                DataTable DT = _MyDBM.ExecuteDataTable(Ssql2);
                if (i == 2)
                {
                    Label lbl2 = new Label();
                    lbl2.ID = "lbl" + i.ToString().PadLeft(2, '0');
                    //string[] selstr ={ "否" };
                    if (DT.Rows.Count > 0)
                    {
                        //for (int j = 0; j < DT.Rows.Count; j++)
                        //{
                        //    ListItem li = new ListItem();
                        //    li.Text = "Y- 是";
                        //    li.Value = "Y";
                        //    ddlAddNew2.Items.Add(li);
                        //}
                        //tbAddNew2.ReadOnly = true;
                        lbl2.Text = "Y - 是";
                    }
                    else
                    {
                        //tbAddNew2.ReadOnly = true;
                        lbl2.Text = "無資料";
                    }

                    e.Row.Cells[i].Controls.Add(lbl2);
                    strValue += "checkColumns(" + lbl2.ClientID + ") && ";
                }
                if (i == 1)
                {
                    DropDownList ddlAddNew = new DropDownList();
                    ddlAddNew.ID = "ddl" + i.ToString().PadLeft(2, '0');
                    if (DT.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT.Rows.Count; j++)
                        {
                            ListItem li = new ListItem();
                            li.Text = DT.Rows[j]["SalaryId"].ToString() + "- " + DT.Rows[j]["SalaryName"].ToString();
                            li.Value = DT.Rows[j]["SalaryId"].ToString();
                            ddlAddNew.Items.Add(li);
                        }
                    }
                    else
                    {
                        ddlAddNew.Items.Add("無資料");
                    }

                    e.Row.Cells[i].Controls.Add(ddlAddNew);
                    strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
                }

            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            e.Row.Cells[0].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";

            for (int i = 1; i < 3; i++)
            {
                string Ssql2 = "SELECT SalaryId,SalaryName,RegularPay FROM SalaryStructure_Parameter Where RegularPay='N'";
                DataTable DT = _MyDBM.ExecuteDataTable(Ssql2);
                if (i == 1)
                {
                    DropDownList ddlAddNew = new DropDownList();
                    ddlAddNew = (DropDownList)e.Row.FindControl("ddl" + i.ToString().PadLeft(2, '0'));
                    if (DT.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT.Rows.Count; j++)
                        {
                            ListItem li = new ListItem();
                            li.Text = DT.Rows[j]["SalaryId"].ToString() + "- " + DT.Rows[j]["SalaryName"].ToString();
                            li.Value = DT.Rows[j]["SalaryId"].ToString();
                            ddlAddNew.Items.Add(li);
                        }
                    }
                    else
                    {
                        ddlAddNew.Items.Add("無資料");
                    }
                    strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
                }
                if (i == 2)
                {
                    Label lbl2 = (Label)e.Row.FindControl("lbl0" + i.ToString());
                    if (DT.Rows.Count > 0)
                    {
                        //tbAddNew2.ReadOnly = true;
                        lbl2.Text = "Y - 是";
                    }
                    else
                    {
                        //tbAddNew2.ReadOnly = true;
                        lbl2.Text = "無資料";
                    }
                    strValue += "checkColumns(" + lbl2.ClientID + ") && ";
                }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

}
