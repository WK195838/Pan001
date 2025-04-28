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

public partial class Basic_LaborInsuranceRateParameter : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB015";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/LaborInsuranceRateParameter.aspx'");
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
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
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
            Panel_Empty.Visible = true;
        }
    }

    private bool hasData()
    {
        Ssql = "SELECT * FROM LaborInsurance_RateParameter Where 0=0";
        //if (txtDate.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And Convert(varchar,LaborInsurance_RateParameterDate,111) like '%{0}%'", _UserInfo.SysSet.GetADDate(txtDate.Text));
        //}
        //Ssql += string.Format(" And Convert(varchar,LaborInsurance_RateParameterDate,111) like '%{0}%'", YearList1.SelectADYear);

        if (txtVersionNo.Text.Length > 0)
        {
            Ssql += string.Format(" And VersionNo like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtVersionNo.Text).ToString());
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
        Ssql = "SELECT * FROM LaborInsurance_RateParameter Where 0=0";
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
        //    Ssql += string.Format(" And Convert(varchar,LaborInsurance_RateParameterDate,111) like '%{0}%'", txtDate.Text);                        
        //}
        //else
        //Ssql += string.Format(" And Convert(varchar,LaborInsurance_RateParameterDate,111) like '%{0}%'", YearList1.SelectADYear);

        if (txtVersionNo.Text.Length > 0)
        {
            Ssql += string.Format(" And VersionNo like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtVersionNo.Text).ToString());
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
        //VersionNo	版本號
        //LI_Premium_Rate	勞保費率
        //EmpBurdenRate	員工負擔比率
        //CompensationRate	墊償比率
        //CompanyBurdenRate	公司負擔比率
        //FatalityRate	職災比率
        //EmploymentInsRate	就業保險費率
        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "3"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "4"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "5"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "6"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "7"].ToString()))
            {
                return;
            }
            //位數判斷
            if (datanum(Request.Form[temId + "1"].ToString(), "1") == false || datanum(Request.Form[temId + "2"].ToString(), "2") == false || datanum(Request.Form[temId + "3"].ToString(), "3") == false || datanum(Request.Form[temId + "4"].ToString(), "4") == false || datanum(Request.Form[temId + "5"].ToString(), "5") == false || datanum(Request.Form[temId + "6"].ToString(), "6") == false || datanum(Request.Form[temId + "7"].ToString(), "7") == false)
            {
                return;
            }
            //小數判斷
            if (IsDecimal(Request.Form[temId + "4"].ToString()) == false)
            {
                lbl_Msg.Text = "墊償比率 整數位數必須為0";
                BindData();
                return;
            }
            if (IsDecimal(Request.Form[temId + "6"].ToString()) == false)
            {
                lbl_Msg.Text = "職災比率 整數位數必須為0";
                BindData();
                return;
            }
            //新增
            if (ValidateData(Request.Form[temId + "1"].ToString()))
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("VersionNo", Request.Form[temId + "1"].ToString());
                SDS_GridView.InsertParameters.Add("LI_Premium_Rate", Request.Form[temId + "2"].ToString());
                SDS_GridView.InsertParameters.Add("EmpBurdenRate", Request.Form[temId + "3"].ToString());
                SDS_GridView.InsertParameters.Add("CompensationRate", Request.Form[temId + "4"].ToString());
                SDS_GridView.InsertParameters.Add("CompanyBurdenRate", Request.Form[temId + "5"].ToString());
                SDS_GridView.InsertParameters.Add("FatalityRate", Request.Form[temId + "6"].ToString());
                SDS_GridView.InsertParameters.Add("EmploymentInsRate", Request.Form[temId + "7"].ToString());

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
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance_RateParameter";
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
    }

    private bool datanum(string p, string tbnum)
    {
        switch (tbnum)
        {
            case "1":
                #region
                if (p.Length > 2)
                {
                    lbl_Msg.Text = "版本號 位數必須小於2";
                    return false;
                }
                else
                {
                    return true;
                }
                #endregion

            case "2":
                #region
                if (p.Length > 6)
                {
                    lbl_Msg.Text = "勞保費率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 4)
                    {
                        lbl_Msg.Text = "勞保費率 小數位數必須小於4";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 3)
                        {
                            lbl_Msg.Text = "勞保費率 整數位數必須小於3";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            case "3":
                #region
                if (p.Length > 6)
                {
                    lbl_Msg.Text = "員工負擔比率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 4)
                    {
                        lbl_Msg.Text = "員工負擔比率 小數位數必須小於4";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 3)
                        {
                            lbl_Msg.Text = "員工負擔比率 整數位數必須小於3";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            case "4":
                #region
                if (p.Length > 7)
                {
                    lbl_Msg.Text = "墊償比率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 6)
                    {
                        lbl_Msg.Text = "墊償比率 小數位數必須小於6";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 2)
                        {
                            lbl_Msg.Text = "墊償比率 整數位數必須小於2";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            case "5":
                #region
                if (p.Length > 6)
                {
                    lbl_Msg.Text = "公司負擔比率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 4)
                    {
                        lbl_Msg.Text = "公司負擔比率 小數位數必須小於4";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 3)
                        {
                            lbl_Msg.Text = "公司負擔比率 整數位數必須小於3";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            case "6":
                #region
                if (p.Length > 7)
                {
                    lbl_Msg.Text = "職災比率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 6)
                    {
                        lbl_Msg.Text = "職災比率 小數位數必須小於6";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 2)
                        {
                            lbl_Msg.Text = "職災比率 整數位數必須小於2";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            case "7":
                #region
                if (p.Length > 6)
                {
                    lbl_Msg.Text = "就業保險費率 位數必須小於6";
                    return false;
                }
                else
                {
                    if (p.LastIndexOf(".") > 4)
                    {
                        lbl_Msg.Text = "就業保險費率 小數位數必須小於4";
                        return false;
                    }
                    else
                    {
                        if (p.IndexOf(".") > 3)
                        {
                            lbl_Msg.Text = "就業保險費率 整數位數必須小於3";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                #endregion
            default:
                return false;
        }
    }

    //判斷整數是否為0 true:符合 false:不符合
    private bool IsDecimal(string p)
    {
        if (p.Substring(0, 1) == "0")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private bool ValidateData(string strVersionNo)
    {
        //判斷資料是否重覆
        Ssql = "Select LI_Premium_Rate, EmpBurdenRate, CompensationRate, CompanyBurdenRate, FatalityRate, EmploymentInsRate From LaborInsurance_RateParameter WHERE VersionNo = '" + strVersionNo + "'";

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
        //string L2PK = btnDelete.Attributes["L2PK"].ToString();

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance_RateParameter";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "Delete From LaborInsurance_RateParameter Where VersionNo='" + L1PK + "'";

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
                //if (i == 0)
                //{//將日期欄位格式為化為西元日期
                //    e.NewValues[i] = _UserInfo.SysSet.FormatADDate(e.NewValues[i].ToString());
                //    tempOldValue = _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(tempOldValue));
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance_RateParameter";
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
                for (int j = 3; j < 9; j++)
                {

                    if (j == 5 || j == 7)
                    {
                        ((TextBox)e.Row.Cells[j].Controls[0]).Style.Add("text-align", "right");
                        ((TextBox)e.Row.Cells[j].Controls[0]).Style.Add("width", "70px");
                        ((TextBox)e.Row.Cells[j].Controls[0]).MaxLength = 7;
                    }
                    else
                    {
                        ((TextBox)e.Row.Cells[j].Controls[0]).Style.Add("text-align", "right");
                        ((TextBox)e.Row.Cells[j].Controls[0]).Style.Add("width", "70px");
                        ((TextBox)e.Row.Cells[j].Controls[0]).MaxLength = 6;
                    }

                }
            }
            else
            {
                #region 查詢用
                e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                if (e.Row.Cells[1].Controls[0] != null)
                {
                    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }
                #endregion
                for (int j = 3; j < 9; j++)
                {
                    e.Row.Cells[j].Style.Add("text-align", "right");
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                TextBox tbAddNew = new TextBox();
                tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                if (i == 3 || i == 4 || i == 6 || i == 8)
                {
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.MaxLength = 6;
                    tbAddNew.Style.Add("width", "70px");
                }
                else if (i == 5 || i == 7)
                {
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.MaxLength = 7;
                    tbAddNew.Style.Add("width", "70px");
                }
                else
                {
                    tbAddNew.MaxLength = 1;
                    tbAddNew.Style.Add("width", "70px");
                }
                e.Row.Cells[i].Controls.Add(tbAddNew);


                strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

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

            for (int i = 1; i < 8; i++)
            {
                TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                if (i == 2 || i == 3 || i == 5 || i == 7)
                {
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.MaxLength = 6;
                    tbAddNew.Style.Add("width", "70px");
                }
                else if (i == 4 || i == 6)
                {
                    tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.MaxLength = 7;
                    tbAddNew.Style.Add("width", "70px");
                }
                else
                {
                    tbAddNew.MaxLength = 1;
                    tbAddNew.Style.Add("width", "70px");
                }
                strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

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

    /// <summary>
    /// 設定全年度週末例假日(週六日)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnSetYearHoilday_Click(object sender, EventArgs e)
    //{
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
    //    MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance_RateParameter";
    //    MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
    //    MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
    //    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "設定 " + YearList1.SelectADYear + "年，全年度週末例假日";
    //    MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
    //    //此時不設定異動結束時間
    //    //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
    //    MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
    //    _MyDBM.DataChgLog(MyCmd.Parameters);
    //    #endregion
    //    bool blSuccess = true;
    //    LockBtn(false);
    //    //先刪除已存在之例假日
    //    string Ssql = "Select LaborInsurance_RateParameterName From LaborInsurance_RateParameter WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,LaborInsurance_RateParameterDate,111) like '" + YearList1.SelectADYear + "/%')";
    //    DataTable tbResult = _MyDBM.ExecuteDataTable(Ssql);
    //    if (tbResult != null)
    //    {
    //        if (tbResult.Rows.Count > 0)
    //        {
    //            Ssql = "Delete From LaborInsurance_RateParameter WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,LaborInsurance_RateParameterDate,111) like '" + YearList1.SelectADYear + "/%')";
    //            _MyDBM.ExecuteCommand(Ssql);
    //        }
    //    }

    //    string firstdate = YearList1.SelectADYear + "/01/01";
    //    string lastdate = YearList1.SelectADYear + "/12/31";
    //    DateTime startDate = DateTime.Parse(firstdate), thisDate;
    //    for (int i = 0; i < (DateTime.Parse(lastdate).DayOfYear - startDate.DayOfYear); i++)
    //    {//從年頭到年尾
    //        thisDate = startDate.AddDays(i);
    //        //新增星期六日
    //        if ((thisDate.DayOfWeek == DayOfWeek.Saturday) || (thisDate.DayOfWeek == DayOfWeek.Sunday))
    //        {
    //            try
    //            {
    //                Ssql = "Insert Into LaborInsurance_RateParameter (Company, LaborInsurance_RateParameterDate, LaborInsurance_RateParameterName) Values ('" + _UserInfo.UData.Company + "',Convert(smalldatetime,'" + thisDate.ToString("yyyy/MM/dd") + "'),'週末例假日')";
    //                _MyDBM.ExecuteCommand(Ssql);
    //            }
    //            catch (Exception ex)
    //            {
    //                //有些年份會因為沒有2/29而產生Exception,因此要避開
    //                if (!(ex.Message.Contains("產生超出範圍的") && ex.Message.Contains("smalldatetime")))
    //                {
    //                    lbl_Msg.Text = ex.Message;
    //                    blSuccess = false;
    //                }
    //            }
    //        }
    //    }

    //    #region 完成異動後,更新LOG資訊
    //    MyCmd.Parameters["@SQLcommand"].Value = (blSuccess) ? "Success" : lbl_Msg.Text;
    //    MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
    //    _MyDBM.DataChgLog(MyCmd.Parameters);
    //    #endregion

    //    LockBtn(true);
    //    BindData();
    //}

    //protected void LockBtn(bool lorUnlock)
    //{
    //    btnSetWeekend.Enabled = lorUnlock;
    //    btnQuery.Enabled = lorUnlock;
    //    btnCalendar1.Enabled = lorUnlock;
    //}
}
