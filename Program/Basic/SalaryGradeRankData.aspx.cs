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

public partial class SalaryGradeRankData : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM017";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryGradeRankData.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
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

            CL_Level.SetDTList("SalaryLevel_CheckStandard", "Level", "LevelName", "", 5, "全部");
            CL_Rank.SetCodeList("PY#Rank", 5, "全部");
            CL_LevelRank.SetDTList("SalaryLevel_CheckStandard", "Level", "LevelName", "", 5);
            GetInit();
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
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
                if (Request.Form[ddlId + "3"] != null)
                {
                    //新增
                    btnEmptyNew_Click(sender, e);
                    hid_IsInsertExit.Value = "";
                }
            }
            else
            {
                if (!Request.Form["__EVENTTARGET"].ToString().Contains("GridView1$ctl01$tbAddNew0"))
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
        btnSetBaseSalary.Attributes.Add("onclick", "return confirm('指定［職等['+document.getElementById('" + CL_LevelRank.CLClientID() + "').options[document.getElementById('" + CL_LevelRank.CLClientID() + "').selectedIndex].text+']］下所有［級數］之［本薪］將被重設!\\n是否確定批次設定職等['+document.getElementById('" + CL_LevelRank.CLClientID() + "').options[document.getElementById('" + CL_LevelRank.CLClientID() + "').selectedIndex].text+'] 之本薪!?');");        
    }

    protected void GetInit()
    {
        #region 薪資參數
        //Ssql = "SELECT isNull([AL_Para1],0) As [AL_Para1],isNull([AL_Para2],0) As [AL_Para2],isNull([AL_Para3],0) As [AL_Para3] " +
        //    " ,isNull([AL_Para4],0) As [AL_Para4],isNull([AL_Para5],0) As [AL_Para5],isNull([AL_Para6],0) As [AL_Para6],isNull([SalaryPoint],0) As [SalaryPoint] " +
        //    " FROM [PersonnelSalary_Parameter] Where [Category]='01' And [Company]='" + _UserInfo.UData.Company + "'" +
        //    " Order By [Company]";
        //DataTable Dt = _MyDBM.ExecuteDataTable(Ssql);
        //if (Dt != null)
        //{
        //    if (Dt.Rows.Count > 0)
        //    {
        //        lbl_SalsryPoint.Text = Dt.Rows[0]["SalaryPoint"].ToString();
        //    }
        //}
        int iSPValue = 1;
        if (int.TryParse(DBSetting.GetPSParaValue(_UserInfo.UData.Company, "SalaryPoint"), out iSPValue) == true)
            lbl_SalsryPoint.Text = "1薪點：" + iSPValue.ToString() + "元";        
        #endregion
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
        Ssql = "SELECT * FROM SalaryGradeRankData Where 0=0";

        if (CL_Level.SelectedCode.Trim().Length > 0)
        {
            Ssql += string.Format(" And Level = '{0}'", CL_Level.SelectedCode.Trim());
        }

        if (CL_Rank.SelectedCode.Trim().Length > 0)
        {
            Ssql += string.Format(" And Rank = '{0}'", CL_Rank.SelectedCode.Trim());
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
        Ssql = "SELECT * FROM SalaryGradeRankData Where 0=0";

        if (CL_Level.SelectedCode.Trim().Length > 0)
        {
            Ssql += string.Format(" And Level = '{0}'", CL_Level.SelectedCode.Trim());
        }

        if (CL_Rank.SelectedCode.Trim().Length > 0)
        {
            Ssql += string.Format(" And Rank = '{0}'", CL_Rank.SelectedCode.Trim());
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
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["SalaryLevel_CheckStandardDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";

        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "1$ddlCodeList"].ToString())
                || String.IsNullOrEmpty(Request.Form[temId + "2$ddlCodeList"].ToString())
                || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString())
                || String.IsNullOrEmpty(Request.Form[temId + "4"].ToString())
                )
            {
                return;
            }

            //新增
            if (ValidateData(Request.Form[temId + "1$ddlCodeList"].ToString(), Request.Form[temId + "2$ddlCodeList"].ToString()))
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("Level", Request.Form[temId + "1$ddlCodeList"].ToString());
                SDS_GridView.InsertParameters.Add("Rank", Request.Form[temId + "2$ddlCodeList"].ToString());
                SDS_GridView.InsertParameters.Add("SalaryPoint", _UserInfo.SysSet.rtnHash(Request.Form[temId + "3"].ToString()));
                SDS_GridView.InsertParameters.Add("BaseSalary", _UserInfo.SysSet.rtnHash(Request.Form[temId + "4"].ToString()));

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
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
                MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "Level=" + Request.Form[temId + "1$ddlCodeList"].ToString() + "|Rank=" + Request.Form[temId + "2$ddlCodeList"].ToString();
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
    }

    private bool ValidateData(string strLevel, string strRank)
    {
        //判斷資料是否重覆
        Ssql = "Select Rank, SalaryPoint, BaseSalary From SalaryGradeRankData WHERE Level = '" + strLevel.Trim() + "' And Rank = '" + strRank.Trim() + "'";

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
        //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryLevel_CheckStandard";
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

        //L2PK = _UserInfo.SysSet.FormatADDate(L2PK);

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryLevel_CheckStandard";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "DELETE FROM SalaryGradeRankData WHERE Level='" + L1PK + "' And Rank='" + L2PK + "'";

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
            //DataRow 的 哪一個cell 加上style 
           //i = e.Row.Cells.Count - 1;
           //if (i > 0)
           //{
           //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
           //    //e.Row.Cells[i - 1].Style.Add("width", "100px");
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
        string tempOldValue = "";//Err = "",
        string UpdateItem = "", UpdateValue = "";

        UpdateValue = "(Key=";
        //將此筆資料的KEY值找出
        for (int i = 0; i < e.Keys.Count; i++)
        {
            UpdateValue += e.Keys[i].ToString().Trim() + "|";
        }
        UpdateValue += ")";

        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (e.OldValues[i] != null)
                    tempOldValue = e.OldValues[i].ToString().Trim();
                else
                    tempOldValue = "";
                e.NewValues[i] = e.NewValues[i].ToString().Trim();

                //日期欄位為KEY值不可修改
                //if (i == 0)
                //{//將日期欄位格式為化為西元日期
                //    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                //    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
                //}

                if (string.IsNullOrEmpty(e.NewValues[i].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[i] = " ";
                }
                else
                {
                    e.NewValues[i] = _UserInfo.SysSet.rtnHash(e.NewValues[i].ToString().Replace(",", "").Trim());
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryLevel_CheckStandard";
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
            //查詢與修改時固定的顯示
            for (int i = 2; i < 4; i++)
            {
                ASP.usercontrol_codelist_ascx tbAddNew = new ASP.usercontrol_codelist_ascx();
                tbAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                if (i == 2)
                {
                    tbAddNew.SetDTList("SalaryLevel_CheckStandard", "Level", "LevelName", "", 5, "未設定");
                }
                else
                {
                    tbAddNew.SetCodeList("PY#Rank", 5, "未設定");
                }
                tbAddNew.SelectedCode = e.Row.Cells[i].Text.Trim();
                e.Row.Cells[i].Text = ((string.IsNullOrEmpty(tbAddNew.SelectedCode)) ? e.Row.Cells[i].Text.Trim() + "-" : "") + tbAddNew.SelectedCodeName;
            }

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                //確認
                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?');");
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
                
                TextBox tempTB;
                tempTB = (TextBox)e.Row.Cells[4].Controls[0];
                tempTB.Style.Add("text-align", "right");
                tempTB.Style.Add("width", "100px");
                if (!string.IsNullOrEmpty(tempTB.Text.Replace("&nbsp;", "")))
                    try
                    {
                        tempTB.Text = string.Format("{0:0,0}", int.Parse(_UserInfo.SysSet.rtnTrans(tempTB.Text)));
                    }
                    catch { }
                tempTB = (TextBox)e.Row.Cells[5].Controls[0];
                tempTB.Style.Add("text-align", "right");
                tempTB.Style.Add("width", "100px");
                if (!string.IsNullOrEmpty(tempTB.Text.Replace("&nbsp;", "")))
                    try
                    {
                        tempTB.Text = string.Format("{0:0,0}", int.Parse(_UserInfo.SysSet.rtnTrans(tempTB.Text)));
                    }
                    catch { }
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

                e.Row.Cells[4].Style.Add("text-align", "right");
                if (!string.IsNullOrEmpty(e.Row.Cells[4].Text.Replace("&nbsp;", "")))
                {
                    try
                    {
                        e.Row.Cells[4].Text = string.Format("{0:0,0}", int.Parse(_UserInfo.SysSet.rtnTrans(e.Row.Cells[4].Text)));
                    }
                    catch { }
                }
                e.Row.Cells[5].Style.Add("text-align", "right");
                if (!string.IsNullOrEmpty(e.Row.Cells[5].Text.Replace("&nbsp;", "")))
                {
                    try
                    {
                        e.Row.Cells[5].Text = string.Format("{0:0,0}", int.Parse(_UserInfo.SysSet.rtnTrans(e.Row.Cells[5].Text)));
                    }
                    catch { }
                }
            }

            if (e.Row.Cells[0].Controls[1] != null)
            {//刪除鈕:加確認訊息
                LinkButton IB = (LinkButton)e.Row.Cells[0].Controls[1];
                IB.OnClientClick = "return confirm('" + string.Format("確定刪除\\n\\n職等[{0}]\\n級數[{1}]\\n\\n之薪職等級與本薪?", e.Row.Cells[2].Text, e.Row.Cells[3].Text) + "');";
            }            
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (i == 2 || i == 3)
                {
                    ASP.usercontrol_codelist_ascx tbAddNew = new ASP.usercontrol_codelist_ascx();
                    tbAddNew = (ASP.usercontrol_codelist_ascx)LoadControl("~/UserControl/CodeList.ascx");
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    if (i == 2)
                    {
                        tbAddNew.SetDTList("SalaryLevel_CheckStandard", "Level", "LevelName", "", 5);
                    }
                    else
                    {
                        tbAddNew.SetCodeList("PY#Rank", 5);
                    }

                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    //strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }
                else
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');

                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "100px");
                    if (i == 3)
                    {
                        tbAddNew.MaxLength = 3;
                    }
                    else
                    {
                        tbAddNew.MaxLength = 7;
                    }

                    e.Row.Cells[i].Controls.Add(tbAddNew);
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
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

            for (int i = 1; i < 5; i++)
            {
                if (i == 1 || i == 2)
                {
                    ASP.usercontrol_codelist_ascx tbAddNew = (ASP.usercontrol_codelist_ascx)e.Row.FindControl("tbAddNew0" + i.ToString());
                    if (i == 1)
                    {
                        tbAddNew.SetDTList("SalaryLevel_CheckStandard", "Level", "LevelName", "", 5);
                    }
                    else
                    {
                        tbAddNew.SetCodeList("PY#Rank", 5);
                    }              
                }
                else
                {
                    TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "100px");
                    if (i == 3)
                    {
                        tbAddNew.MaxLength = 3;
                    }
                    else
                    {
                        tbAddNew.MaxLength = 7;
                    }
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }                
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

    /// <summary>
    /// 設定單一職等本薪
    /// </summary>
    protected void btnSetBaseSalary_Click(object sender, EventArgs e)
    {        
        bool blSuccess = true;

        if (string.IsNullOrEmpty(CL_LevelRank.SelectedCode))
        {
            lbl_Msg.Text = "請選擇［本薪職等］";
            blSuccess = false;
        }
        else if (string.IsNullOrEmpty(txt_BaseSalary.Text))
        {
            lbl_Msg.Text = "請輸入［１級本薪］";
            blSuccess = false;
        }
        else if (string.IsNullOrEmpty(txt_Bracket.Text))
        {
            lbl_Msg.Text = "請輸入［級差］";
            blSuccess = false;
        }

        int tempBaseSalary = 0;
        if (blSuccess == true)
            try
            {
                tempBaseSalary = Convert.ToInt32(txt_BaseSalary.Text);
                if (tempBaseSalary <= 0)
                {
                    lbl_Msg.Text = "［１級本薪］設定有誤!\n必需為正整數";
                    blSuccess = false;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = "［１級本薪］設定有誤!\n" + ex.Message;
                blSuccess = false;
            }

        int tempBracket = 0;
        if (blSuccess == true)
            try
            {
                tempBracket = Convert.ToInt32(txt_Bracket.Text);
                if (tempBracket <= 0)
                {
                    lbl_Msg.Text = "［級差］設定有誤!\n必需為正整數";
                    blSuccess = false;
                }
            }
            catch (Exception ex)
            {
                lbl_Msg.Text = "［級差］設定有誤!\n" + ex.Message;
                blSuccess = false;
            }

        if (blSuccess == true)
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "SalaryGradeRankData";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "BaseSalary";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "設定 " + CL_LevelRank.SelectedCodeName.Trim() + "，所有級數之本薪";
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            string Ssql = "Select CodeCode From CodeDesc Where CodeID = 'PY#Rank' Order By CodeCode";
            DataTable tbResult = _MyDBM.ExecuteDataTable(Ssql);
            if (tbResult != null)
            {
                if (tbResult.Rows.Count > 0)
                {
                    string tempRank = tbResult.Rows[0][0].ToString().Trim();

                    for (int i = 0; i < tbResult.Rows.Count; i++)
                    {
                        tempRank = tbResult.Rows[i][0].ToString().Trim();

                        Ssql = "Select * From SalaryGradeRankData WHERE Level = '" + CL_LevelRank.SelectedCode.Trim() + "' And Rank = '" + tempRank + "'";
                        DataTable tbTempResult = _MyDBM.ExecuteDataTable(Ssql);

                        if (i > 0)
                        {                            
                            tempBaseSalary += tempBracket;
                        }

                        Ssql = "Insert Into SalaryGradeRankData ([Level],[Rank],[BaseSalary]) Values ('" + CL_LevelRank.SelectedCode.Trim() + "','" + tempRank + "','" + _UserInfo.SysSet.rtnHash(tempBaseSalary.ToString()) + "')";
                        if (tbTempResult != null)
                        {
                            if (tbTempResult.Rows.Count > 0)
                            {
                                Ssql = "Update SalaryGradeRankData Set [BaseSalary]='" + _UserInfo.SysSet.rtnHash(tempBaseSalary.ToString()) + "'" +
                                    " WHERE Level = '" + CL_LevelRank.SelectedCode.Trim() + "' And Rank = '" + tempRank + "'";
                            }
                        }

                        _MyDBM.ExecuteCommand(Ssql);
                    }
                }
            }

            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (blSuccess) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
        BindData();
    }
}
