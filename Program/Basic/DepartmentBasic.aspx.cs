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

public partial class DepartmentBasic : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB003";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/DepartmentBasic.aspx'");
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
            //08.25 修改
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            //YearList1.initList();
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            if (Request.Form[19] != null)
            {
                //新增
                btnEmptyNew_Click(sender, e);
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
        Ssql = "SELECT * FROM Department_Basic Where 0=0";//Holiday
        if (txtDepCode.Text.Length > 0)
        {
            Ssql += string.Format(" And DepCode like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtDepCode.Text).ToString());
        }

        if (txtDepName.Text.Length > 0)
        {
            Ssql += string.Format(" And DepName like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtDepName.Text).ToString());
        }

        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
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
        Ssql = "SELECT * FROM Department_Basic Where 0=0";//Holiday
        if (txtDepCode.Text.Length > 0)
        {
            Ssql += string.Format(" And DepCode like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtDepCode.Text).ToString());
            //Ssql += string.Format(" And Convert(varchar,HolidayDate,111) like '%{0}%'", _UserInfo.SysSet.GetADDate(txtDate.Text));

        }

        if (txtDepName.Text.Length > 0)
        {
            Ssql += string.Format(" And DepName like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtDepName.Text).ToString());
            //Ssql += string.Format(" And HolidayName like '%{0}%'", txtDateDesc.Text);
        }

        SDS_GridView.SelectCommand = Ssql;

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
        
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindData();
        showPanel();
        hid_DepCode.Value = "";
        hid_DepName.Value = "";
        lbl_Msg.Text = "";
        //hid_DepName.Value = "";
        //hid_Date.Value = "";
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        hid_DepCode.Value = GridView1.SelectedDataKey.Values["DepCode"].ToString();
        hid_DepName.Value = GridView1.SelectedDataKey.Values["DepName"].ToString();
    }

    protected void btnEmptyNew_Click(object sender, EventArgs e)
    {


        //判斷是否有空值
        if (String.IsNullOrEmpty(Request.Form[16].ToString()) ||
            String.IsNullOrEmpty(Request.Form[17].ToString()) || 
            String.IsNullOrEmpty(Request.Form[18].ToString()) || 
            String.IsNullOrEmpty(Request.Form[19].ToString()) || 
            String.IsNullOrEmpty(Request.Form[20].ToString()) || 
            String.IsNullOrEmpty(Request.Form[21].ToString()) )
        {
            return;
        }
  
       
        //新增
        if (ValidateData(Request.Form[16].ToString(), Request.Form[17].ToString()))
        {
            SDS_GridView.InsertParameters.Clear();
            SDS_GridView.InsertParameters.Add("Company", Request.Form[16].ToString());
            SDS_GridView.InsertParameters.Add("DepCode", Request.Form[17].ToString());
            SDS_GridView.InsertParameters.Add("DepName", Request.Form[18].ToString());
            SDS_GridView.InsertParameters.Add("ChiefTitle", Request.Form[19].ToString());
            SDS_GridView.InsertParameters.Add("ChiefID", Request.Form[20].ToString());
            SDS_GridView.InsertParameters.Add("ParentDepCode", Request.Form[21].ToString());
            //訊息寫入LOG檔
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
            
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Department_Basic";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = 1911 / 1 / 1;
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.NVarChar, 32).Value = _UserInfo.UData.UserId;            
            //寫入LOG
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
            int i = 0;
            //08.25 修改
            //try
            //{
                i = SDS_GridView.Insert();
            //}
            //catch (Exception ex)
            //{
            //    lbl_Msg.Text = ex.Message;
            //}
            if (i == 1)
            {
                lbl_Msg.Text = i.ToString() + " 個資料列 " + "新增成功!!";
            }
            else
            {
                lbl_Msg.Text = "新增失敗!!";
            }

            //TableName	異動資料表	varchar	60
            //TrxType	異動類別(A/U/D)	char	1
            //ChangItem	異動項目(欄位)	varchar	255
            //SQLcommand	異動項目(異動值/指令)	varchar	2000
            //ChgStartDateTime	異動開始時間	smalldatetime	
            //ChgStopDateTime	異動結束時間	smalldatetime	
            //ChgUser	使用者代號	nvarchar	32
            //MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "";
            //MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "";
            //MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "";
            //MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
            //MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = 0;
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = 1911 / 1 / 1;
            //MyCmd.Parameters.Add("@ChgUser", SqlDbType.NVarChar, 32).Value = "";
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (i == 1) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            //寫入LOG
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            BindData();
            showPanel();
            hid_DepCode.Value = "";
            hid_DepName.Value = "";
            //hid_DepName.Value = "";
            //hid_Date.Value = "";
        }
        else
        {
            lbl_Msg.Text = " 新增失敗!!" + " 原因: 資料重覆 ";
            BindData();
  
   
        }
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";
            BindData();
            hid_DepCode.Value = "";
            hid_DepName.Value = "";
            //hid_DepName.Value = "";
            //hid_Date.Value = "";
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }

        showPanel();
    }


    private bool ValidateData(string strCompany,string strDepCode)
    {
        //判斷資料是否重覆
        Ssql = "Select DepName,ChiefTitle,ChiefID,ParentDepCode From Department_Basic WHERE DepCode = '" + strDepCode + "' And Company = '" + strCompany + "'";
        //Department_Basic
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

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();

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

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Department_Basic";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = 1911 / 1 / 1;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.NVarChar, 32).Value = _UserInfo.UData.UserId;
        //寫入LOG
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        
        string sql = "Delete From Department_Basic Where Company='" + L1PK + "' And DepCode='" + L2PK + "'";

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
        //寫入LOG
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
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
            }
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
        // 

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

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Department_Basic";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = 1911 / 1 / 1;
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.NVarChar, 32).Value = _UserInfo.UData.UserId;
        //寫入LOG
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion


    }


    protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {

        //if (e.NewValues["DepName"] == null)
        //{
        //    lbl_Msg.Text = "部門名稱不能空白";
        //    return;
        //}
        //if (e.NewValues["ChiefTitle"] == null)
        //{
        //    //if (string.IsNullOrEmpty(e.NewValues["ChiefTitle"].Trim()))
        //    //{
        //    lbl_Msg.Text = "主管職稱不能空白";
        //    return;
        //    //}
        //}
        //if (e.NewValues["ParentDepCode"] == null)
        //{
        //    lbl_Msg.Text = "父階部門代號不能空白";
        //    return;
        //}

        if (e.Exception == null)
        {
            GridView1.EditIndex = -1;
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "更新成功!!";
            BindData();
            hid_DepCode.Value = "";
            hid_DepName.Value = "";
            //hid_DepName.Value = "";
            //hid_Date.Value = "";
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
        //寫入LOG
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
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
                #region 修改用
                //if (e.Row.Cells[4].Controls.Contains("DepName") == null)
                //{
                //    lbl_Msg.Text = "部門名稱不能空白";
                //    return;
                //}
                //if (e.NewValues["ChiefTitle"] == null)
                //{
                //    //if (string.IsNullOrEmpty(e.NewValues["ChiefTitle"].Trim()))
                //    //{
                //    lbl_Msg.Text = "主管職稱不能空白";
                //    return;
                //    //}
                //}
                //if (e.NewValues["ParentDepCode"] == null)
                //{
                //    lbl_Msg.Text = "父階部門代號不能空白";
                //    return;
                //}
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
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            #region 新增用欄位
            strValue = "";
            //08.25 修改 
            //Label lblAddNew = new Label();

            //lblAddNew.ID = "lblAddNew01";
            //e.Row.Cells[2].Controls.Add(lblAddNew);
            //e.Row.Cells[2].Text = "02";
            //e.Row.Cells[2].Style.Add("width", "100px");




            //UserControl_CompanyList CompanyList12 = new UserControl_CompanyList();

            //CompanyList12.ID = "CompanyList12";
            //CompanyList12.TextMode = UserControl_CompanyList.TextModeEnum.NumShortName;


            UserControl_CompanyList CompanyList1 = (UserControl_CompanyList)LoadControl("../UserControl/CompanyList.ascx");
            CompanyList1.ID = "CompanyList1";
            CompanyList1.EnableViewState = false;
            CompanyList1.TextMode = UserControl_CompanyList.TextModeEnum.NumShortName;
            CompanyList1.Visible = true;
         

            e.Row.Cells[2].Controls.Add(CompanyList1);

      
            for (int i = 3; i < e.Row.Cells.Count; i++)
            {
                TextBox tbAddNew = new TextBox();
                tbAddNew.ID = "tbAddNew" + (i - 3).ToString().PadLeft(2, '0');
                tbAddNew.Style.Add("width", "100px");
                e.Row.Cells[i].Controls.Add(tbAddNew);
                //e.Row.Cells[2].Style.Add("width", "100px");
                //if (i == 2)
                //{
                //    tbAddNew.Text = "01";
                //    tbAddNew.ReadOnly = true;
                //}
                if (i != 6)
                {
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }

                // //數字欄位靠右
                //if (GridView1.Columns[i].ToString() == "部門代號"||GridView1.Columns[i].ToString()=="父階部門代號")
                //{//數字欄位需靠右對齊
                //    TextBox tc = (TextBox)e.Row.Cells[i].Controls[0];
                //    tc.Style.Add("text-align", "right");
                //}
                
            }

            ImageButton btAddNew = new ImageButton();
            btAddNew.ID = "btAddNew";
            btAddNew.SkinID = "NewAdd";
            btAddNew.CommandName = "Insert";
            btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?'));";
            e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            #region 新增用欄位

            strValue = "";
            //lbl_Msg.Text = e.Row.Cells[0].Text;
            //e.Row.Cells[0].Controls.Add("01");
            Label lblAddNew = (Label)e.Row.FindControl("lblAddNew01");

            lblAddNew.Text = "01";
            lblAddNew.Style.Add("width", "100px");

            for (int i = 1; i < 6; i++)
            {

                TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
                tbAddNew.Style.Add("width", "100px");

                //if (i == 6)
                //{
                //    tbAddNew.ReadOnly = true;
                //    tbAddNew.Text = "01";
                ////////    Label lblAddNew = (Label)e.Row.FindControl("lblAddNew01");

                ////////    lblAddNew.Text = "01";
                ////////    lblAddNew.Style.Add("width", "150px");
                //}

                if (i != 4)
                {
                    strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
                }

                //if (GridView1.Columns[i].ToString() == "部門代號")
                //{//數字欄位需靠右對齊

                //    TextBox tc = (TextBox)e.Row.Cells[i].Controls[0];
                //    tc.Style.Add("text-align", "right");

                //}
            }

            ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            if (btnNew != null)
                btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?'));");
            #endregion
        }
    }
    //08.23 修改---Department_Basic不需要
    /// <summary>
    /// 設定全年度週末例假日(週六日)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnSetYearHoilday_Click(object sender, EventArgs e)
    //{
    //    LockBtn(false);
    //    //先刪除已存在之例假日
    //    string Ssql = "Select HolidayName From Holiday WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,HolidayDate,111) like '" + YearList1.SelectADYear + "/%')";
    //    DataTable tbResult = _MyDBM.ExecuteDataTable(Ssql);
    //    if (tbResult != null)
    //    {
    //        if (tbResult.Rows.Count > 0)
    //        {
    //            Ssql = "Delete From Holiday WHERE Company = '" + _UserInfo.UData.Company + "' And (Convert(varchar,HolidayDate,111) like '" + YearList1.SelectADYear + "/%')";
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
    //                Ssql = "Insert Into Holiday (Company, HolidayDate, HolidayName) Values ('" + _UserInfo.UData.Company + "',Convert(smalldatetime,'" + thisDate.ToString("yyyy/MM/dd") + "'),'週末例假日')";
    //                _MyDBM.ExecuteCommand(Ssql);
    //            }
    //            catch (Exception ex)
    //            {
    //                //
    //            }
    //        }
    //    }

    //    LockBtn(true);
    //    BindData();
    //}

    //protected void LockBtn(bool lorUnlock)
    //{
    //    //btnSetWeekend.Enabled = lorUnlock;
    //    btnQuery.Enabled = lorUnlock;
    //    //btnCalendar1.Enabled = lorUnlock;
    //}
    //protected void GridView1_DataBound(object sender, EventArgs e)
    //{
    //    for (int i = 0; i <  .Rows.Count; i++)
    //    {
    //        if (GridView1.cells[i].Text.Contains("DepCode"))
    //        {//數字欄位需靠右對齊
    //            TableCell tc = (TableCell)GridView1.Rows.Cells[i];
    //            tc.Style.Add("text-align", "right");
    //        }
    //    }
    //}
}
