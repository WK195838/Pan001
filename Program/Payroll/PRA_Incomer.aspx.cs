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

public partial class PRA_Incomer : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PRAI01";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll py = new Payroll();
    string TableName = "PRA_Incomer";
    string[] KeyFile = { "Company", "IDNo" };
    string[] FileName = {"Company"
      ,"IDNo"
      ,"name"
      ,"Address" };
    int[] FileLenMax = { 16, 10, 25, 100 };
    string[] FileShowKind = {"VF"
      ,""
      ,""
      ,"" };
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
        SetSqlDataSource();
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

            SearchList1.SelectValue = _UserInfo.UData.Company;
            SetFileName();  
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
                string ddlId = hid_IsInsertExit.Value.Replace("_", "$");
                if (Request.Form[0].Contains(ddlId + "$btAddNew"))
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
            if (SearchList1.SelectValue == "")
            {
                Panel_Empty.Visible = false;          
            }
            else
            {                
                Panel_Empty.Visible = true;             
            }
        }
    }

    private bool hasData()
    {
        GridView1.Visible = true;
        if (GridView1.Rows.Count > 0)
        {
            Navigator1.Visible = (GridView1.PageCount > 1) ? true : false;            
            return true;
        }
        else
        {            
            Navigator1.Visible = false;
            return false;
        }
    }

    private void BindData()
    {
        Ssql = "SELECT * FROM [" + TableName + "] Where 0=0";

        if (SearchList1.SelectValue.Length > 0)
        {//公司
            Ssql += string.Format(" And Company='{0}'", SearchList1.SelectValue.Trim());
        }
        else
        {
            Ssql += " And 1=0";
            lbl_Msg.Text = "請先選擇公司";
        }

        if (txtIDNo.Text.Trim().Length > 0)
        {//期間
            Ssql += string.Format(" And [IDNo] like '%{0}%'", txtIDNo.Text.Trim());
        }

        SDS_GridView.SelectCommand = Ssql;
        SDS_GridView.SelectParameters.Clear();
      
        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    //GridView 資料設定
    protected void SetFileName()
    {

        for (int i = 0; i < FileName.Length; i++)
        {
            BoundField Data = new BoundField();
            Data.DataField = FileName[i];
            Data.HeaderText = FileName[i];
            Data.SortExpression = FileName[i];
            if (FileShowKind[i] == "VF")
                Data.Visible = false;
            else if (FileShowKind[i] == "R")
                Data.ReadOnly = true;
            GridView1.Columns.Add(Data);
        }
    }

    //SqlDataSource 資料設定
    protected void SetSqlDataSource()
    {
        SDS_GridView.SelectParameters.Clear();
        SDS_GridView.DeleteParameters.Clear();
        SDS_GridView.InsertParameters.Clear();
        SDS_GridView.UpdateParameters.Clear();
        
        for (int i = 0; i < KeyFile.Length; i++)
        {
            QueryStringParameter Key = new QueryStringParameter();
            Key.Name = "key" + KeyFile[i];
            Key.QueryStringField = KeyFile[i];
            //SelectParameters
            SDS_GridView.SelectParameters.Add(Key);
            //DeleteParameters
            SDS_GridView.DeleteParameters.Add(Key);
            //UpdateParameters
            SDS_GridView.UpdateParameters.Add(Key);
        }

        //InsertParameters
        for (int i = 0; i < FileName.Length; i++)
        {
            Parameter Name = new Parameter();
            Name.Name = FileName[i];
            //InsertParameters
            SDS_GridView.InsertParameters.Add(Name);
            //UpdateParameters
            SDS_GridView.UpdateParameters.Add(Name);
        }

        SDS_GridView.ConnectionString = _MyDBM.GetConnectionString();
        string tmp = "";
        string Querykey = "";
        for (int i = 0; i < KeyFile.Length; i++)
        {
            Querykey += FileName[i] + "=@key" + FileName[i] + " and ";
        }        
        try
        {
            Querykey = Querykey.Remove(Querykey.Length - 4);
        }
        catch { Querykey = "1=0"; }
        //SelectCommand
        tmp = "SELECT * FROM " + TableName + " WHERE " + Querykey;
        SDS_GridView.SelectCommand = tmp;

        //DeleteCommand
        tmp = "DELETE FROM " + TableName + " WHERE " + Querykey;
        SDS_GridView.DeleteCommand = tmp;

        //InsertCommand
        tmp = "INSERT INTO " + TableName + "(";
        for (int i = 0; i < FileName.Length; i++)
            tmp += FileName[i] + ",";

        tmp = tmp.TrimEnd(',') + ") VALUES (";

        for (int i = 0; i < FileName.Length; i++)
            tmp += "@" + FileName[i] + ",";

        tmp = tmp.TrimEnd(',') + ") ";
        SDS_GridView.InsertCommand = tmp;


        //UpdateCommand
        tmp = "UPDATE " + TableName + " SET ";
        //KEY 為 Company 所以 0 跳過
        for (int i = 1; i < FileName.Length; i++)
        {
            tmp += FileName[i] + " = @" + FileName[i] + ",";
        }
        tmp = tmp.TrimEnd(',') + " Where " + Querykey;

        SDS_GridView.UpdateCommand = tmp;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        if (SearchList1.SelectValue.Length > 0)
        {
            GridView1.Visible = true;
            BindData();
            showPanel();
        }
        else
        {
            lbl_Msg.Text = "請先選擇公司";
            GridView1.Visible = false;
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hid_Company.Value = GridView1.SelectedDataKey.Values["Company"].ToString();
        //hid_Date.Value = ((DateTime)GridView1.SelectedDataKey["LaborInsurance_RateParameterDate"]).ToString("yyyy/MM/dd");
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {
        string temId = hid_IsInsertExit.Value.Replace("_", "$") + "$tbAddNew0";
        
        string Err = "";
        //新增資料
        if (hid_IsInsertExit.Value != "")
        {
            if (Err == "")
            {
                int i = 0;
                //新增
                if (ValidateData(SearchList1.SelectValue.Trim(), Request.Form[temId + "2"].ToString()))
                {
                    //SDS_GridView.InsertParameters.Clear();
                    for (i = 0; i < FileName.Length; i++)
                    {
                        string theValue = "";
                        try
                        {
                            if (i == 0)
                                theValue = SearchList1.SelectValue.Trim();
                            else
                                theValue = Request.Form[temId + (i + 1).ToString()].ToString().Trim();
                        }
                        catch
                        {
                            theValue = "";
                        }
                        SDS_GridView.InsertParameters[FileName[i]].DefaultValue = theValue;
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
                    MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
                    MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
                    MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
                    MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                    //此時不設定異動結束時間
                    //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                    MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                    _MyDBM.DataChgLog(MyCmd.Parameters);
                    #endregion

                    i = 0;
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
            else
            {
                lbl_Msg.Text = Err;
                BindData();
                hid_IsInsertExit.Value = "";
            }
        }
  
    }

    private bool ValidateData(string strKey1, string strKey2)
    {
        string[] inKey = { strKey1, strKey2 };
        string Querykey = "";
        for (int i = 0; i < KeyFile.Length; i++)
        {
            Querykey += FileName[i] + "='" + inKey[i] + "' and ";
        }
        try
        {
            Querykey = Querykey.Remove(Querykey.Length - 4);
        }
        catch { Querykey = "1=0"; }
        //判斷資料是否重覆        
        Ssql = "Select *  From  " + TableName + " Where " + Querykey;

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return false;
        }
        return true;
    }    

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
//
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
//
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();

        SDS_GridView.DeleteParameters[0].DefaultValue = L1PK;
        SDS_GridView.DeleteParameters[1].DefaultValue = L2PK;

        #region 開始異動前,先寫入LOG
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = TableName;
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = SDS_GridView.DeleteCommand + "|" + L1PK + "|" + L2PK;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        int result = SDS_GridView.Delete();
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
            //修改欄位顯示名稱
            for (int i = 0; i < GridView1.Columns.Count; i++)
            {
                Ssql = "Select dbo.GetColumnTitle('" + TableName + "','" + GridView1.Columns[i].HeaderText + "')";
                DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                if (DT.Rows != null && DT.Rows.Count > 0)
                {
                    GridView1.Columns[i].HeaderText = DT.Rows[0][0].ToString().Trim();
                }
            }
        }
        else if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer) || (e.Row.RowType == DataControlRowType.EmptyDataRow))
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            int i = 0;
            switch (e.Row.RowType)
            {
                case DataControlRowType.EmptyDataRow:
                    Table TB = new Table();
                    TB.Width = GridView1.Width;
                    for (int TbRow = 0; TbRow < 2; TbRow++)
                    {
                        TableRow TR = new TableRow();
                        if (TbRow == 0)
                            TR.CssClass = "button_bar_cell";
                        for (i = 0; i <= FileName.Length; i++)
                        {
                            if (i > 0 && (FileShowKind[i - 1] == "VF" || FileShowKind[i - 1] == "R"))
                            {//不可見或唯讀欄位
                            }
                            else
                            {
                                TableCell TC = new TableCell();

                                if (TbRow == 0)
                                {
                                    if (i == 0)
                                    {
                                    }
                                    else
                                    {
                                        Label lbAddNew = new Label();
                                        try
                                        {
                                            lbAddNew.Text = GridView1.Columns[i + 1].HeaderText;
                                            Ssql = "Select dbo.GetColumnTitle('" + TableName + "','" + GridView1.Columns[i + 1].HeaderText + "')";
                                            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
                                            if (DT.Rows != null && DT.Rows.Count > 0)
                                            {
                                                lbAddNew.Text = DT.Rows[0][0].ToString().Trim();
                                            }
                                        }
                                        catch { }
                                        TC.Controls.Add(lbAddNew);
                                    }
                                }
                                else
                                {
                                    TC.CssClass = "Grid_GridLine";
                                    if (i == 0)
                                    {
                                        ImageButton btnNew = new ImageButton();
                                        btnNew.ID = "btAddNew";
                                        btnNew.CommandName = "btnEmptyNew_Click";
                                        btnNew.SkinID = "NewAdd";
                                        TC.Controls.Add(btnNew);
                                    }
                                    else
                                    {
                                        TextBox tbAddNew = new TextBox();
                                        tbAddNew.ID = "tbAddNew" + i.ToString().PadLeft(2, '0');
                                        //tbAddNew.Style.Add("text-align", "right");
                                        tbAddNew.Style.Add("width", "100px");
                                        //GridView1.Columns[i].HeaderText

                                        TC.Controls.Add(tbAddNew);
                                    }
                                }
                                TR.Cells.Add(TC);
                            }
                        }
                        TB.Rows.Add(TR);
                    }
                    TableCell theETC = new TableCell();
                    theETC.ColumnSpan = FileName.Length;                    
                    theETC.Controls.Add(TB);
                    e.Row.Cells.Add(theETC);
                    break;
            }
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
        for (int i = 0; i < KeyFile.Length; i++)
        {
            tempOldValue = "";
            try
            {
                tempOldValue = e.OldValues[KeyFile[i]].ToString().Trim();
            }
            catch {
                if (KeyFile[i].ToLower() == "company")
                    tempOldValue = SearchList1.SelectValue.Trim();                
            }
            UpdateValue += tempOldValue + "|";
            SDS_GridView.UpdateParameters["key" + KeyFile[i]].DefaultValue = tempOldValue;            
        }
        UpdateValue += ")";

        for (int i = 0; i < FileName.Length; i++)
        {
            try
            {
                tempOldValue = "";
                if (e.OldValues[FileName[i]] != null)
                    tempOldValue = e.OldValues[FileName[i]].ToString().Trim();

                //if (e.NewValues[FileName[i]] != null)
                //    e.NewValues[FileName[i]] = py.EnCodeAmount(e.NewValues[FileName[i]].ToString().Trim());
                //else
                //    e.NewValues[FileName[i]] = "";

                if (string.IsNullOrEmpty(e.NewValues[FileName[i]].ToString()))
                {//將空欄位放入半形空格
                    e.NewValues[FileName[i]] = " ";
                }

                if (e.NewValues[FileName[i]].ToString().Trim() != tempOldValue)
                {
                    try
                    {
                        UpdateItem += GridView1.HeaderRow.Cells[i + 3].Text.Trim() + "|";
                        UpdateValue += e.OldValues[FileName[i]].ToString().Trim() + "->" + e.NewValues[FileName[i]].ToString().Trim() + "|";
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = TableName;
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
            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                for (int i = 0; i < GridView1.Columns.Count; i++)
                {
                    try
                    {
                        //設定textbox的欄位限制與大小
                        TextBox tmp = (TextBox)e.Row.Cells[i].Controls[0];
                        tmp.Width = 120;                        
                        tmp.MaxLength = FileLenMax[i];
                    }
                    catch { }
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";

            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                if (i == 2)
                {//預設值:不可見
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    tbAddNew.Text = SearchList1.SelectValue.Trim();
                    //tbAddNew.MaxLength = 7;
                    e.Row.Cells[i].Controls.Add(tbAddNew);                    
                }
                else
                {
                    TextBox tbAddNew = new TextBox();
                    tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
                    //tbAddNew.Style.Add("text-align", "right");
                    tbAddNew.Style.Add("width", "100px");
                    //tbAddNew.MaxLength = 7;
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

            for (int i = 0; i < FileName.Length; i++)
            {
                try
                {
                    if (FileShowKind[i] == "VF")
                    {//不可見欄位
                    }
                    else if (FileShowKind[i] == "R")
                    {//唯讀欄位
                    }
                    else
                    {
                        TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew" + i.ToString().PadLeft(2, '0'));
                        //tbAddNew.Style.Add("text-align", "right");
                        tbAddNew.Style.Add("width", "100px");
                        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                    }
                }
                catch { }
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btAddNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
    }    
}
