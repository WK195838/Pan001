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

public partial class AnnualLeaveSeriesParameter : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB019";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/AnnualLeaveSeriesParameter.aspx'");
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
            CL_ALTrans.SetCodeList("PY#ALTrans", 5);            
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
            else if (!string.IsNullOrEmpty(hid_IsUpdateExit.Value))
            {
                //修改
            }
            else
            {
                BindData();
            }
            #endregion
        }

        txtMonthLowerLimit.Style.Add("text-align", "right");
        txtYearLowerLimit.Style.Add("text-align", "right");
        btnSetALPara.Attributes.Add("onclick", "return ( confirm('確定要重設特休參數嗎?') && SaveValue(" + hid_IsUpdateExit.ClientID + ",'ReSetALPara') );");
        btnSetALDaysPara.Attributes.Add("onclick", "return ( confirm('確定要重設特休參數與特休天數嗎?') && SaveValue(" + hid_IsUpdateExit.ClientID + ",'ReSetALPara&Days') );");        
        btnSetALDays.Attributes.Add("onclick", "return ( confirm('確定要重設特休天數嗎?') && SaveValue(" + hid_IsUpdateExit.ClientID + ",'ReSetALDays') );");        
        btnClear.Attributes.Add("onclick", "return ( confirm('確定要刪除所有特休天數之設定嗎?') );");
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
        Ssql = "SELECT * FROM AnnualLeave_SeriesParameter Where 0=0";

        int iQueryY = 0, iQueryM = 0;

        if (txtMonthLowerLimit.Text.Trim().Length > 0)
        {
            int.TryParse(txtMonthLowerLimit.Text.Trim(), out iQueryM);
        }

        if (txtYearLowerLimit.Text.Trim().Length > 0)
        {
            int.TryParse(txtYearLowerLimit.Text.Trim(), out iQueryY);
        }

        if (iQueryY * 12 + iQueryM > 0)
        {
            Ssql += string.Format(" And MonthLowerLimit >= {0} ", (iQueryY * 12 + iQueryM).ToString());
        }

        MyCmd.Parameters.Clear();
        //MyCmd.Parameters.Add("@ls_RangeNo", SqlDbType.Int).Value = 0;
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
        #region 特休參數
        Ssql = "SELECT isNull([AL_Para1],0) As [AL_Para1],isNull([AL_Para2],0) As [AL_Para2],isNull([AL_Para3],0) As [AL_Para3] " +
            " ,isNull([AL_Para4],0) As [AL_Para4],isNull([AL_Para5],0) As [AL_Para5],isNull([AL_Para6],0) As [AL_Para6] " +
            " FROM [PersonnelSalary_Parameter] Where [Category]='01' And [Company]='" + _UserInfo.UData.Company + "'";
        DataTable Dt = _MyDBM.ExecuteDataTable(Ssql);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                txtLeastYear.Text = Dt.Rows[0]["AL_Para1"].ToString();
                txtLeastYearALdays.Text = Dt.Rows[0]["AL_Para2"].ToString();
                txtRangYear.Text = Dt.Rows[0]["AL_Para3"].ToString();
                txtAddALdays.Text = Dt.Rows[0]["AL_Para4"].ToString();
                txtMaxALdays.Text = Dt.Rows[0]["AL_Para5"].ToString();
                CL_ALTrans.SelectedCode = Dt.Rows[0]["AL_Para6"].ToString();
            }
            else
            {
                #region 設定特休參數
                Ssql = " INSERT INTO [PersonnelSalary_Parameter] " +
                    " ([Category],[Company],[OT_Para1],[OT_Para2],[OT_Para3],[OT_Para4] " +
                    " ,[OT_Para5_1_4],[OT_Para5_4_8],[OT_Para5_8_12],[OT_Para31_1_4],[OT_Para31_4_8],[OT_Para31_8_12],[OT_Para30_1_4],[OT_Para30_4_8],[OT_Para30_8_10],[OT_Para30_10_12],[OT_Para6],[OT_Para7] " +
                    " ,[PY_Para1],[PY_Para2],[PY_Para3],[PY_Para4],[PY_Para5],[PY_Para6],[PY_Para7] " +
                    " ,[AL_Para1],[AL_Para2],[AL_Para3],[AL_Para4],[AL_Para5],[AL_Para6] " +
                    " ) " +
                    " Select '01','" + _UserInfo.UData.Company + "',46,24,1.50,1.50,1.50,1.67,1.67,1.50,1.67,1.67,1.50,1.67,1.67,1.67,2.00,2.00,0.000,0,0.005,0.00,0.00,0.06,2000 " +
                    " ,10,15,1,1,30,'03' ";
                _MyDBM.ExecuteCommand(Ssql);

                txtLeastYear.Text = "10";
                txtLeastYearALdays.Text = "15";
                txtRangYear.Text = "1";
                txtAddALdays.Text = "1";
                txtMaxALdays.Text = "30";
                CL_ALTrans.SelectedCode = "03";
                #endregion 
            }
        }
        #endregion
        #region 特休設定GridView
        Ssql = "SELECT * FROM AnnualLeave_SeriesParameter Where 0=0";

        int iQueryY = 0, iQueryM = 0;

        if (txtMonthLowerLimit.Text.Trim().Length > 0)
        {
            int.TryParse(txtMonthLowerLimit.Text.Trim(), out iQueryM);
        }

        if (txtYearLowerLimit.Text.Trim().Length > 0)
        {
            int.TryParse(txtYearLowerLimit.Text.Trim(), out iQueryY);
        }

        if (iQueryY * 12 + iQueryM > 0)
        {
            Ssql += string.Format(" And MonthLowerLimit >= {0} ", (iQueryY * 12 + iQueryM).ToString());
        }

        Ssql += " Order By MonthLowerLimit";
        SDS_GridView.SelectCommand = Ssql;

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
        #endregion
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        int iQuery = 0;

        txtMonthLowerLimit.Text = txtMonthLowerLimit.Text.Trim();        
        if (txtMonthLowerLimit.Text.Length > 0)
        {
            if (int.TryParse(txtMonthLowerLimit.Text, out iQuery) == false)
            {
                lbl_Msg.Text = "月數輸入有誤!請輸入正整數";
                txtMonthLowerLimit.Text = "";                
            }
        }

        txtYearLowerLimit.Text = txtYearLowerLimit.Text.Trim();
        if (txtYearLowerLimit.Text.Length > 0)
        {
            if (int.TryParse(txtYearLowerLimit.Text, out iQuery) == false)
            {
                lbl_Msg.Text = "年數輸入有誤!請輸入正整數";
                txtYearLowerLimit.Text = "";                
            }
        }

        if (lbl_Msg.Text.Length == 0)
        {
            BindData();
            showPanel();
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hid_Company.Value = GridView1.SelectedDataKey.Values["Company"].ToString();
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["AnnualLeave_SeriesParameterDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";

        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (String.IsNullOrEmpty(Request.Form[temId + "1"].ToString()) || String.IsNullOrEmpty(Request.Form[temId + "2"].ToString()))
            {
                return;
            }

            string strSeq = "";
            //新增
            if (ValidateData(Request.Form[temId + "1"].ToString(), out strSeq))
            {
                SDS_GridView.InsertParameters.Clear();
                SDS_GridView.InsertParameters.Add("RangeNo", strSeq);
                SDS_GridView.InsertParameters.Add("MonthLowerLimit", Request.Form[temId + "1"].ToString());
                SDS_GridView.InsertParameters.Add("ALDays", Request.Form[temId + "2"].ToString());                

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
                MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
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

    private bool ValidateData(string strMonthLowerLimit, out string strSeq)
    {
        //判斷資料是否重覆
        Ssql = "Select RangeNo,MonthLowerLimit, ALDays From AnnualLeave_SeriesParameter WHERE MonthLowerLimit = '" + strMonthLowerLimit + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        strSeq = "";

        if (tb.Rows.Count > 0)
        {
            strSeq = tb.Rows[0]["RangeNo"].ToString();
            return false;
        }
        else
        {
            Ssql = "Select Count(*) From AnnualLeave_SeriesParameter ";
            tb = _MyDBM.ExecuteDataTable(Ssql);
            int iCount = (int)tb.Rows[0][0] + 1;
            int iSeq = 1;
            strSeq = iSeq.ToString().PadLeft(2, '0');

            Ssql = "Select Count(*) From AnnualLeave_SeriesParameter WHERE [RangeNo] = '" + strSeq + "'";
            tb = _MyDBM.ExecuteDataTable(Ssql);
            while (((int)tb.Rows[0][0] > 0 && iSeq <= iCount))
            {
                iSeq++;
                strSeq = iSeq.ToString().PadLeft(2, '0');
                Ssql = "Select Count(*) From AnnualLeave_SeriesParameter WHERE [RangeNo] = '" + strSeq + "'";
                tb = _MyDBM.ExecuteDataTable(Ssql);
            }
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
        //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        string sql = "Delete From AnnualLeave_SeriesParameter Where RangeNo='" + L1PK + "'";

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
        
        hid_IsUpdateExit.Value = "";

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
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
                    IB.Attributes.Add("onclick", "return (confirm('確定要修改資料嗎?')  && SaveValue(" + hid_IsUpdateExit.ClientID + ",'" + e.Row.ClientID + "'));");
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
                ((TextBox)e.Row.Cells[2].Controls[0]).Style.Add("text-align", "right");
                ((TextBox)e.Row.Cells[3].Controls[0]).Style.Add("text-align", "right");                
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
                //e.Row.Cells[2].Style.Add("text-align", "right");
                //e.Row.Cells[2].Text = string.Format("{0:0,0}", int.Parse(e.Row.Cells[2].Text));
                e.Row.Cells[2].Text = "到職 " + e.Row.Cells[2].Text + " 個月(滿 " + (int.Parse(e.Row.Cells[2].Text) / 12).ToString() + " 年" + ((int.Parse(e.Row.Cells[2].Text) % 12 == 0) ? "" : " " + (int.Parse(e.Row.Cells[2].Text) % 12) + " 個月") + ")";
                //e.Row.Cells[3].Style.Add("text-align", "right");
                //e.Row.Cells[3].Text = string.Format("{0:0,0}", int.Parse(e.Row.Cells[5].Text));
                e.Row.Cells[3].Text = e.Row.Cells[3].Text + " 天";
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

                tbAddNew.Style.Add("text-align", "right");
                tbAddNew.MaxLength = 3;
                //tbAddNew.Style.Add("width", "70px");

                e.Row.Cells[i].Controls.Add(tbAddNew);


                strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
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

            for (int i = 1; i < 2; i++)
            {
                TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());

                tbAddNew.Style.Add("text-align", "right");
                tbAddNew.MaxLength = 3;
                //tbAddNew.Style.Add("width", "70px");

                strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }

    protected void btnSetALPara_Click(object sender, EventArgs e)
    {
        string Err = "", strLogMsg = "";
        int iLY = 0, iLYALD = 0, iRY = 0, iAALD = 0, iMALD = 0;
        lbl_Msg.Text = "";
        string strSeq = "";
                
        #region 檢核輸入值
        if ((txtLeastYear.Text.Trim().Length == 0) || (int.TryParse(txtLeastYear.Text.Trim(), out iLY).Equals(false)))
        {
            Err = "[最低年限]輸入有誤! 請輸入 0 或 正整數!";
            txtLeastYear.Text = "0";
        }
        else if ((txtLeastYearALdays.Text.Trim().Length == 0) || (int.TryParse(txtLeastYearALdays.Text.Trim(), out iLYALD).Equals(false)))
        {
            Err = "[滿最低年限之特休天數]輸入有誤! 請輸入 0 或 正整數!";
            txtLeastYearALdays.Text = "0";
        }
        else if ((txtRangYear.Text.Trim().Length == 0) || (int.TryParse(txtRangYear.Text.Trim(), out iRY).Equals(false)))
        {
            Err = "[間隔年數]輸入有誤! 請輸入 0 或 正整數!";
            txtRangYear.Text = "0";
        }
        else if ((txtAddALdays.Text.Trim().Length == 0) || (int.TryParse(txtAddALdays.Text.Trim(), out iAALD).Equals(false)))
        {
            Err = "[應增加之天數]輸入有誤! 請輸入正整數!";
            txtAddALdays.Text = "";
        }
        else if ((txtMaxALdays.Text.Trim().Length == 0) || (int.TryParse(txtMaxALdays.Text.Trim(), out iMALD).Equals(false)))
        {
            Err = "[最高特休天數]輸入有誤! 請輸入 0 或 正整數!";
            txtMaxALdays.Text = "0";
        }
        else if (iLYALD >= iMALD)
        {
            Err = "[滿最低年限之特休天數]不可大於或等於[最高特休天數]";
        }
        else if (iAALD < 1)
        {
            Err = "[應增加之天數]應為大於 0 之正整數";
            txtAddALdays.Text = "";
        }
        else if (CL_ALTrans.SelectedCode.Length == 0)
        {
            Err = "請選擇[未休特休轉換方式]!";
        }
        #endregion

        if (Err.Length > 0)
        {
            lbl_Msg.Text = Err;
            return;
        }
        else
        {
            int iMaxRY = iLY + ((iMALD - iLYALD) / iAALD + 1) * iRY;
            int iAddDays = iLYALD;
            int iInsertCount = 0;
            strLogMsg = "到職滿" + iLY.ToString() + "年後，給予特休" + iLYALD.ToString() + "天，每" +
                iRY.ToString() + "年多" + iAALD.ToString() + "天，最多" + iMALD.ToString() + "天" + "|未休特休轉換方式:" + CL_ALTrans.SelectedCodeName;

            #region 開始異動前,先寫入LOG
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "PersonnelSalary_Parameter";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "設定特休參數";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = (strLogMsg.Length > 1000) ? strLogMsg.Remove(1000) : strLogMsg;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            #region 儲存特休參數設定
            Ssql = " UPDATE [PersonnelSalary_Parameter] " +
                " SET [AL_Para1] = " + iLY.ToString() +
                " ,[AL_Para2] = " + iLYALD.ToString() +
                " ,[AL_Para3] = " + iRY.ToString() +
                " ,[AL_Para4] = " + iAALD.ToString() +
                " ,[AL_Para5] = " + iMALD.ToString() +
                " ,[AL_Para6] = '" + CL_ALTrans.SelectedCode + "'" +
                " WHERE [Category]='01' And [Company]='" + _UserInfo.UData.Company + "'";
            iInsertCount = _MyDBM.ExecuteCommand(Ssql);
            #endregion

            if (iInsertCount > 0)
            {
                lbl_Msg.Text = "參數設定成功!";
            }
            else if (iInsertCount == 0)
            {
                lbl_Msg.Text = "參數設定失敗! 請確認各項數值是否輸入有誤!!";
            }
            else
            {
                lbl_Msg.Text = "參數設定失敗! 請洽系統管理員!!";
            }

            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
        BindData();
        showPanel();
    }

    protected void btnSetALDays_Click(object sender, EventArgs e)
    {
        SetALDaysPara(false);
    }

    protected void btnSetALDaysPara_Click(object sender, EventArgs e)
    {
        SetALDaysPara(true);
    }

    protected void SetALDaysPara(bool setPara)
    {
        string Err = "", strLogMsg = "";
        int iLY = 0,iLYALD = 0, iRY = 0, iAALD = 0, iMALD = 0;
        lbl_Msg.Text = "";
        string strSeq = "", strMonthCount = "";
        
        #region 檢核輸入值
        if ((txtLeastYear.Text.Trim().Length == 0) || (int.TryParse(txtLeastYear.Text.Trim(), out iLY).Equals(false)))
        {
            Err = "[最低年限]輸入有誤! 請輸入 0 或 正整數!";
            txtLeastYear.Text = "0";
        }
        else if ((txtLeastYearALdays.Text.Trim().Length == 0) || (int.TryParse(txtLeastYearALdays.Text.Trim(), out iLYALD).Equals(false)))
        {
            Err = "[滿最低年限之特休天數]輸入有誤! 請輸入 0 或 正整數!";
            txtLeastYearALdays.Text = "0";
        }
        else if ((txtRangYear.Text.Trim().Length == 0) || (int.TryParse(txtRangYear.Text.Trim(), out iRY).Equals(false)))
        {
            Err = "[間隔年數]輸入有誤! 請輸入 0 或 正整數!";
            txtRangYear.Text = "0";
        }
        else if ((txtAddALdays.Text.Trim().Length == 0) || (int.TryParse(txtAddALdays.Text.Trim(), out iAALD).Equals(false)))
        {
            Err = "[應增加之天數]輸入有誤! 請輸入正整數!";
            txtAddALdays.Text = "";
        }
        else if ((txtMaxALdays.Text.Trim().Length == 0) || (int.TryParse(txtMaxALdays.Text.Trim(), out iMALD).Equals(false)))
        {
            Err = "[最高特休天數]輸入有誤! 請輸入 0 或 正整數!";
            txtMaxALdays.Text = "0";
        }
        else if (iLYALD >= iMALD)
        {
            Err = "[滿最低年限之特休天數]不可大於或等於[最高特休天數]";
        }
        else if (iAALD < 1)
        {
            Err = "[應增加之天數]應為大於 0 之正整數";
            txtAddALdays.Text = "";
        }
        else if (CL_ALTrans.SelectedCode.Length == 0)
        {
            Err = "請選擇[未休特休轉換方式]!";
        }
        #endregion

        if (Err.Length > 0)
        {
            lbl_Msg.Text = Err;
            return;
        }
        else 
        {
            int iMaxRY = iLY + ((iMALD - iLYALD) / iAALD + 1) * iRY;
            int iAddDays = iLYALD;
            int iInsertCount = 0;

            strLogMsg = "到職滿" + iLY.ToString() + "年後，給予特休" + iLYALD.ToString() + "天，每" +
                iRY.ToString() + "年多" + iAALD.ToString() + "天，最多" + iMALD.ToString() + "天" + "|未休特休轉換方式:" + CL_ALTrans.SelectedCodeName;

            if (setPara == true)
            {
                #region 儲存特休參數設定
                Ssql = " UPDATE [PersonnelSalary_Parameter] " +
                    " SET [AL_Para1] = " + iLY.ToString() +
                    " ,[AL_Para2] = " + iLYALD.ToString() +
                    " ,[AL_Para3] = " + iRY.ToString() +
                    " ,[AL_Para4] = " + iAALD.ToString() +
                    " ,[AL_Para5] = " + iMALD.ToString() +
                    " ,[AL_Para6] = '" + CL_ALTrans.SelectedCode + "'" +
                    " WHERE [Category]='01' And [Company]='" + _UserInfo.UData.Company + "'";
                _MyDBM.ExecuteCommand(Ssql);
                #endregion
            }

            #region 開始異動前,先寫入LOG
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "依參數設定";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = (strLogMsg.Length > 1000) ? strLogMsg.Remove(1000) : strLogMsg;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            #region 設定特休天數
            for (int i = iLY; i <= iMaxRY; i++)
            {
                if (iAddDays <= iMALD)
                {
                    strMonthCount = (i * 12).ToString();
                    if (ValidateData(strMonthCount, out strSeq))
                    {
                        Ssql = "INSERT INTO AnnualLeave_SeriesParameter([RangeNo],[MonthLowerLimit],[ALDays]) VALUES ('" + strSeq + "'," + strMonthCount + ", " + iAddDays.ToString() + ")";
                    }
                    else
                    {
                        Ssql = "UPDATE AnnualLeave_SeriesParameter SET ALDays=" + iAddDays.ToString() + " WHERE RangeNo='" + strSeq + "' And MonthLowerLimit=" + strMonthCount + "";
                    }

                    iInsertCount += _MyDBM.ExecuteCommand(Ssql);
                }
                i += (iRY - 1);
                iAddDays += iAALD;
            }
            #endregion

            if (iInsertCount > 0)
            {
                lbl_Msg.Text = "特休天數設定成功! 共增修 " + iInsertCount.ToString() + " 筆";
            }
            else if (iInsertCount == 0)
            {
                lbl_Msg.Text = "特休天數設定失敗! 請確認各項數值是否輸入有誤!!";
            }
            else
            {
                lbl_Msg.Text = "特休天數設定失敗! 請洽系統管理員!!";
            }

            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
        BindData();
        showPanel();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        string Err = "";
        lbl_Msg.Text = "";
        string strLogMsg = "";

        int iInsertCount = 0;
        #region 開始異動前,先寫入LOG
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "刪除所有特休天數之設定";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = (strLogMsg.Length > 1000) ? strLogMsg.Remove(1000) : strLogMsg;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        Ssql = "Delete From AnnualLeave_SeriesParameter";
        iInsertCount += _MyDBM.ExecuteCommand(Ssql);

        if (iInsertCount > 0)
        {
            lbl_Msg.Text = "刪除成功! 共刪除 " + iInsertCount.ToString() + " 筆";
        }
        else if (iInsertCount == 0)
        {
            lbl_Msg.Text = "無資料可刪除!!";
        }
        else
        {
            lbl_Msg.Text = "刪除失敗! 請洽系統管理員!!";
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        BindData();
        showPanel();
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
    //    MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AnnualLeave_SeriesParameter";
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
    //    string Ssql = "Select AnnualLeave_SeriesParameterName From AnnualLeave_SeriesParameter WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,AnnualLeave_SeriesParameterDate,111) like '" + YearList1.SelectADYear + "/%')";
    //    DataTable tbResult = _MyDBM.ExecuteDataTable(Ssql);
    //    if (tbResult != null)
    //    {
    //        if (tbResult.Rows.Count > 0)
    //        {
    //            Ssql = "Delete From AnnualLeave_SeriesParameter WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,AnnualLeave_SeriesParameterDate,111) like '" + YearList1.SelectADYear + "/%')";
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
    //                Ssql = "Insert Into AnnualLeave_SeriesParameter (Company, AnnualLeave_SeriesParameterDate, AnnualLeave_SeriesParameterName) Values ('" + _UserInfo.UData.Company + "',Convert(smalldatetime,'" + thisDate.ToString("yyyy/MM/dd") + "'),'週末例假日')";
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
