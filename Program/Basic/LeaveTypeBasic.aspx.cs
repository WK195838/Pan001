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

public partial class LeaveTypeBasic : System.Web.UI.Page
{

    #region 共用參數

    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB004";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

    #endregion

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

        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/LeaveTypeBasic.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }


    //  身分驗證副程式
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

    //  載入網頁時執行此區
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
            CompanyList1.SelectValue = _UserInfo.UData.Company;
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
                //BindData();

            }
        }




    }

    //  判斷搜尋時是否有此資料
    private bool hasData()
    {
        Ssql = "SELECT * FROM LeaveType_Basic Where 0=0";

        if (txtLeave_Id.Text.Length > 0)
        {
            Ssql += string.Format(" And Leave_Id like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtLeave_Id.Text).ToString());
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

    //  綁定資料至Gridview
    private void BindData()
    {
        Ssql = "SELECT * FROM LeaveType_Basic Where Company='" + CompanyList1.SelectValue + "'";
        if (txtLeave_Id.Text.Length > 0)
        {
            Ssql += string.Format(" And Leave_Id like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtLeave_Id.Text).ToString());
        }

        Ssql += " Order By Leave_Id ";
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    //  當按下搜尋按鈕時
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = string.Empty;
        BindData();
        hid_IsInsertExit.Value = "";
    }

    //  新增按鈕控制項
    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
        
        try
        {
            //  判斷是否有欄位沒輸入
            if (
                String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "5"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "6"].ToString()) ||
                String.IsNullOrEmpty(Request.Form[temId + "7"].ToString())
                )
            {

                return;
            }
        }
        catch {
            return;
        }

        //新增

        if (ValidateData(Request.Form[temId + "1"].ToString()))
        {

            SDS_GridView.InsertParameters.Clear();
            SDS_GridView.InsertParameters.Add("Company", CompanyList1.SelectValue);
            SDS_GridView.InsertParameters.Add("Leave_Id"        , Request.Form[temId + "1"].ToString());
            SDS_GridView.InsertParameters.Add("Leave_Desc"      , Request.Form[temId + "2"].ToString());
            SDS_GridView.InsertParameters.Add("SalaryType"      , Request.Form[temId + "3"].ToString());
            SDS_GridView.InsertParameters.Add("Pay_days"        , Request.Form[temId + "4"].ToString());
            SDS_GridView.InsertParameters.Add("Pay_rate"        , Request.Form[temId + "5"].ToString());
            SDS_GridView.InsertParameters.Add("Annual_LeaveDays", Request.Form[temId + "6"].ToString());
            SDS_GridView.InsertParameters.Add("Attendance"      , Request.Form[temId + "7"].ToString());
            WriteLog(true, "A", 0);

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
            WriteLog(false, "A", i);
            BindData();
        }
        else
        {
            BindData();
        }
        hid_IsInsertExit.Value = "";
    }

    //  判斷資料是否重覆
    private bool ValidateData(string strID)
    {

        Ssql = "Select Leave_Id FROM LeaveType_Basic WHERE Leave_Id = '" + strID + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            return false;
        }
        return true;
    }


    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";
            BindData();
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }
        showPanel();
    }

    
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();

        string sql = "Delete FROM LeaveType_Basic  Where  Company='" + L1PK + "' and Leave_Id='" + L2PK + "' ";

        WriteLog(true, "D", 0);

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        WriteLog(false, "D", result > 0 ? 1 : 0);

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";

        }
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

            i = e.Row.Cells.Count - 1;
            if (i > 0)
            {
                e.Row.Cells[i - 1].Style.Add("text-align", "right");
                e.Row.Cells[i - 1].Style.Add("width", "80px");
            }
            e.Row.Cells[i].Style.Add("text-align", "left");

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
        string temId = UpdataID.Value.Replace ( "_" , "$" ) + "$";        
        e.NewValues["SalaryType"] = Request.Form[temId + "tbAddNew03"];
        e.NewValues["Attendance"] = Request.Form[temId + "tbAddNew07"];
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LeaveType_Basic";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
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
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

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
            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用

                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + UpdataID.ClientID + ",'" + e.Row.ClientID + "');");
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

                ((TextBox)e.Row.Cells[4].Controls[0]).Visible = false;
                DropDownList DDL = new DropDownList();
                DDL.ID = "tbAddNew03";
                for (int i2 = 0; i2 < 2; i2++)
                {
                    ListItem li = new ListItem();
                    li.Text = i2 == 0 ? "計　薪" : "不計薪";
                    li.Value = i2 == 0 ? "Y" : "N";
                    DDL.Items.Add(li);
                }
                DDL.SelectedValue = ((TextBox)e.Row.Cells[4].Controls[0]).Text;
                e.Row.Cells[4].Controls.Add(DDL);

                ( ( TextBox ) e.Row.Cells[ 8 ].Controls[ 0 ] ).Visible = false;
                DropDownList DDL2 = new DropDownList ( );
                DDL2.ID = "tbAddNew07";
                for ( int i2 = 0 ; i2 < 2 ; i2++ )
                {
                    ListItem li = new ListItem ( );
                    li.Text = i2 == 1 ? "扣" : "不　扣";
                    li.Value = i2 == 1 ? "Y" : "N";
                    DDL2.Items.Add ( li );
                }
                DDL2.SelectedValue = ( ( TextBox ) e.Row.Cells[ 8 ].Controls[ 0 ] ).Text;
                e.Row.Cells[ 8 ].Controls.Add ( DDL2 );

                for ( int i = 3 ; i < e.Row.Cells.Count-1 ; i++ )
                {
                    ( ( TextBox ) e.Row.Cells[ i ].Controls[ 0 ] ).Width = 90;
                    ( ( TextBox ) e.Row.Cells[ i ].Controls[ 0 ] ).Style.Add ( "text-align" , "right" );
                }

                             


                #endregion
            }
            else
            {
                #region 查詢用
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }

                for (int i = 5; i < 8; i++)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                }
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.Replace("Y", "計　薪").Replace("N","不計薪");
                e.Row.Cells[ 6 ].Text += e.Row.Cells[ 6 ].Text.Replace("&nbsp;","").Trim().Length > 0 ? "%" : "";
                e.Row.Cells[ 8 ].Text = e.Row.Cells[ 8 ].Text.Trim ( ).Length < 1 ? "不　扣" : e.Row.Cells[ 8 ].Text.Replace ( "Y" , "扣" ).Replace ( "N" , "不　扣" ).Replace ( "&nbsp;" , "不　扣" );
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";


            for (int i = 2; i < e.Row.Cells.Count-1; i++)
            {
                if (i != 4 && i != 8)
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    tbAddNew.Width = 80;
                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
                else if (i == 4 )
                {
                    DropDownList DDL = new DropDownList();
                    DDL.ID = "tbAddNew03";
                    for (int i2 = 0; i2 < 2; i2++)
                    {
                        ListItem li = new ListItem();
                        li.Text = i2 == 0 ? "計　薪" : "不計薪";
                        li.Value = i2 == 0 ? "Y" : "N";
                        DDL.Items.Add(li);
                    }
                    e.Row.Cells[i].Controls.Add(DDL); 
                }
                else if ( i == 8 )
                {
                    DropDownList DDL = new DropDownList ( );
                    DDL.ID = "tbAddNew07";
                    for ( int i2 = 0 ; i2 < 2 ; i2++ )
                    {
                        ListItem li = new ListItem ( );
                        li.Text = i2 == 1 ? "扣" : "不　扣";
                        li.Value = i2 == 1 ? "Y" : "N";
                        DDL.Items.Add ( li );
                    }
                    e.Row.Cells[ i ].Controls.Add ( DDL );
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
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位
            strValue = "";
            for (int i = 1; i < 8; i++)
            {
                switch (i)
                {
                    case 3:
                        DropDownList DDL = new DropDownList();
                        DDL = (DropDownList)e.Row.FindControl("tbAddNew03");
                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            ListItem li = new ListItem();
                            li.Text = i2 == 0 ? "計　薪" : "不計薪";
                            li.Value = i2 == 0 ? "Y" : "N";
                            DDL.Items.Add(li);
                        }
                        break;
                    case 7:
                        DropDownList DDL2 = new DropDownList ( );
                        DDL2 = ( DropDownList ) e.Row.FindControl ( "tbAddNew07" );
                        for ( int i2 = 0 ; i2 < 2 ; i2++ )
                        {
                            ListItem li = new ListItem ( );
                            li.Text = i2 == 1 ? "扣" : "不　扣";
                            li.Value = i2 == 1 ? "Y" : "N";
                            DDL2.Items.Add ( li );
                        }
                        break;
                    default:
                        TextBox tbAddNew = new TextBox();
                        tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                        tbAddNew.Width = 80;
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                        break;
                }

            }


            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion

        }
    }

    protected void SDS_GridView_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

    private void WriteLog(bool n, string c, int i)
    {
        if (n)
        {
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LeaveType_Basic";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = c;
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
        else
        {
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
    }


    //  控制頁面 Panel，查無資料時顯示用
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





}//  程式尾部
