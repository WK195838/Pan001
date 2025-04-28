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

public partial class CodeMaster: System.Web.UI.Page
{

    #region 共用參數

    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB001";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

    #endregion

    #region 資料設定
    string DataSouceName = "CodeMaster";

    string[] DataKey = { 
        "CodeID"
    };

    string[] DataName = {
        "CodeDecs",
        "CodeLen",
        "CodeDescLen",
        "Maint"
    };

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
        if (_UserInfo.SysSet.isTWCalendar)
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
    }

    //  身分驗證副程式
    private void AuthRight()
    {
        if ( _UserInfo.AuthLogin == false )
        {
            Response.Redirect ( "\\" + Application [ "WebSite" ].ToString ( ) + "\\" + Application [ "ErrorPage" ].ToString ( ) + "?ErrMsg=UnLogin" );
        }
        else
        {
            if (_UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "") == false)
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
        }


        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Add", "Maint" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, Auth[i]);

                switch (Auth[i])
                {
                    case "Delete":
                    case "Modify":
                        GridView1.Columns[i].Visible = Find;
                        if (Find && (SetCss == false))
                        {
                            SetCss = true;
                            GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";

                        }
                        break;

                    case "Add":
                        GridView1.ShowFooter = Find;
                        break;
                    case "Maint":
                        GridView1.Columns[GridView1.Columns.Count - 1].Visible = Find;
                        //設定標題樣式
                        if (Find && (SetCss == false))
                        {
                            SetCss = true;
                            GridView1.Columns[GridView1.Columns.Count - 1].HeaderStyle.CssClass = "paginationRowEdgeLl";
                        }
                        break;
                    default:
                        break;
                }
            }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "")) || Find)
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
                GridView1.Columns[(Auth.Length - 2)].HeaderStyle.CssClass = "paginationRowEdgeLl";
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
            AuthRight ( );
            BindData();
            showPanel();
           
        }
        else
        {
            if ( Request.Form.Count > 0 && Request.Form [ 0 ].Contains ( "UpDate" ) )
            {
                DataBind ( );
            }
            else
            {
                BindData();
            }
        }
    }

    //  判斷搜尋時是否有此資料
    private bool hasData()
    {
        Ssql = "SELECT * FROM " + DataSouceName + " Where 0=0";

        if (txtCodeID.Text.Length > 0)
        {
            Ssql += string.Format(" And CodeID like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCodeID.Text).ToString());
        }

        if (txtCodeDecs.Text.Length > 0)
        {
            Ssql += string.Format(" And CodeDecs like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCodeDecs.Text).ToString());
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
        Ssql = "SELECT * FROM " + DataSouceName + " Where 0=0";

        if (txtCodeID.Text.Length > 0)
        {
            Ssql += string.Format(" And CodeID like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCodeID.Text).ToString());
        }

        if (txtCodeDecs.Text.Length > 0)
        {
            Ssql += string.Format(" And CodeDecs like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCodeDecs.Text).ToString());
        }


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
    }

    // 明細按鈕
    protected void btnViewDesc_Click( object sender , EventArgs e )
    {
        Button Btn = ( Button ) sender;
        string Code = Btn.Attributes [ "Code" ].ToString ( );
       
    }


    #region #新增設定區#
    //  新增按鈕
    protected void CodeMasterbtnNew_Click ( object sender , EventArgs e )
    {
        if ( ValidateData ( CodeMasterNew01.Text ) )
        {
            SDS_GridView.InsertParameters.Clear ( );
            SDS_GridView.InsertParameters.Add ( "CodeID" , CodeMasterNew01.Text );
            SDS_GridView.InsertParameters.Add ( "CodeDecs" , CodeMasterNew02.Text );
            SDS_GridView.InsertParameters.Add ( "CodeLen" , CodeMasterNew03.Text );
            SDS_GridView.InsertParameters.Add ( "CodeDescLen" , CodeMasterNew04.Text );
            SDS_GridView.InsertParameters.Add ( "Maint" , CodeMasterNew05.SelectedValue );

            WriteLog ( true , "A" , 0 );
            int i = 0;
            try
            {
                i = SDS_GridView.Insert ( );
            }
            catch ( Exception ex )
            {
                lbl_Msg.Text = ex.Message;
            }

            CodeMasterNew01.Text = "";
            CodeMasterNew02.Text = "";
            CodeMasterNew03.Text = "";
            CodeMasterNew04.Text = "";
            CodeMasterNew05.SelectedIndex = 0;

            lbl_Msg.Text = i == 1 ? i.ToString ( ) + " 個資料列 " + "新增成功!!" : lbl_Msg.Text = "新增失敗!!" + lbl_Msg.Text;

            WriteLog ( false , "A" , i );
        }
    }
    
    //  判斷資料是否重覆
    private bool ValidateData(string strCodeID)
    {
        //判斷資料是否重覆
        Ssql = "Select CodeID From CodeMaster WHERE CodeID = '" + strCodeID + "'";

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if ( CodeMasterNew01.Text.Length == 0 || CodeMasterNew02.Text.Length == 0 || CodeMasterNew03.Text.Length == 0 || CodeMasterNew04.Text.Length == 0 )
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料不可空白";
            return false;
        }


        if ( tb.Rows.Count > 0)
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();


        string sql = "Delete From CodeMaster  Where CodeID='" + L1PK + "' ";

        WriteLog(true, "D", 0);

        int result = _MyDBM.ExecuteCommand(sql);

        WriteLog(false, "D", result > 0 ? 1 : 0);

        lbl_Msg.Text = result > 0 ? "資料刪除成功 !!" : "資料刪除失敗 !!";

        BindData ( );
    }

    #region #更新設定區#
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        hid_IsInsertExit.Value = hid_IsInsertExit.Value.Replace ( "_" , "$" );

        e.NewValues [ "CodeDecs" ] = Request.Form [ hid_IsInsertExit.Value + "$TB4" ].ToString ( );
        e.NewValues [ "CodeLen" ] = Request.Form [ hid_IsInsertExit.Value + "$TB5" ].ToString ( );
        e.NewValues [ "CodeDescLen" ] = Request.Form [ hid_IsInsertExit.Value + "$TB6" ].ToString();
        e.NewValues [ "Maint" ] = Request.Form [ hid_IsInsertExit.Value + "$DD2" ].Replace ( "啟用" , "Y" ).Replace ( "關閉" , "N" ).ToString ( );

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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "OverTime_Master";
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
    #endregion

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
                    IB.ID = "UpDate";
                    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');");
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");

                }
                //取消
                if (e.Row.Cells[1].Controls[2] != null)
                {
                    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                    IB.ID = "Cancel";
                    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                    IB.Style.Add("filter", "alpha(opacity=50)");
                }

                #region @   TextBox 對齊,大小 設定    @
                for (int i = 4; i < 7; i++)
                {
                    
                    ( ( TextBox ) e.Row.Cells [ i ].Controls [ 0 ] ).ID = "TB"+i;
                    ((TextBox)e.Row.Cells[i].Controls[0]).Style.Add("text-align", "right");
                    ((TextBox)e.Row.Cells[i].Controls[0]).Width = i == 3 ? 160 : 50;
                    ((TextBox)e.Row.Cells[i].Controls[0]).MaxLength = i == 3 ? 22 : 3;
                }
                #endregion

                #region @   維 護 碼 下拉式選單 @
                //先隱藏TextBox
                ((TextBox)e.Row.Cells[7].Controls[0]).Visible = false;
                DropDownList DD1 = new DropDownList();
                DD1.ID = "DD2";
                DD1.Items.Add("啟用");
                DD1.Items.Add("關閉");
                DD1.Width = 64;
                DD1.SelectedValue = ((TextBox)e.Row.Cells[7].Controls[0]).Text.Replace("Y","啟用").Replace("N","關閉");
                e.Row.Cells[7].Controls.Add(DD1);
                #endregion


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

                string CodeID = "";
                
                 for ( int i = 0 ; i < e.Row.Cells.Count ; i++ )
                    {
                        if ( ( ( ( DataControlFieldCell ) ( e.Row.Cells [ i ] ) ).ContainingField ).GetType ( ).Name == "BoundField" )
                        {
                            string Name = ( ( BoundField ) ( ( ( DataControlFieldCell ) ( e.Row.Cells [ i ] ) ).ContainingField ) ).DataField.ToString ( );
                            switch ( Name )
                            {
                                case "CodeID":
                                    CodeID = e.Row.Cells [ i ].Text;
                                    //檔案名稱
                                    string tUrl = "CodeDesc.aspx";
                                    //要傳的資料
                                    tUrl += "?Code=" + CodeID.Replace("#",".");   
                                    SetWindowOpen ( e.Row.FindControl ( "ViewDesc" ) , tUrl , "800" , "360" );
                                    e.Row.Cells [ i ].Style.Add ( "text-align" , "left" );
                                    break;
                                    
                                case "CodeDecs":
                                    e.Row.Cells [ i ].Style.Add ( "text-align" , "left" );
                                    break;
                                case "CodeLen":
                                case "CodeDescLen":
                                    e.Row.Cells [ i ].Style.Add ( "text-align" , "right" );
                                    break;

                                case "Maint":
                                    #region @   維護碼顯示修改  @
                                    e.Row.Cells [ i ].Text = e.Row.Cells [ i ].Text.Replace ( "Y" , "啟用" ).Replace ( "N" , "關閉" );
                                    e.Row.Cells [ i ].Style.Add ( "text-align" , "left" );
                                    #endregion
                                    break;
                            }
                        }
                    }
                    #endregion
                
                #region @   過濾使用者是否可以看到維護碼為關閉的資料
                bool Find = false;
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Maint");
                if (!Find)
                {
                    DataTable DT = _MyDBM.ExecuteDataTable("SELECT Maint,CodeID FROM CodeMaster Where CodeID = '" + CodeID + "'");
                    if (DT.Rows[0]["Maint"].ToString() == "N")
                    {            
                        e.Row.Visible = Find;
                    }
                }
                #endregion

                #region @   設定除ADM以外都不可看見 Function 與 Sqlcomm 開頭的代碼欄位  @
                if ( _UserInfo.UData.Role.ToString ( ).ToLower ( ) != "administrator" && ( CodeID.ToLower ( ).Contains ( "function" ) || CodeID.ToLower ( ).Contains ( "sqlcomm" ) ) )
                {
                    e.Row.Visible = Find;
                }
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            bool s = true;
            if ( Request.Form.Count > 0 )
            {
                if(Request.Form [ 0 ].Contains ( "btnDelete" ) && GridView1.Rows.Count <= 2)
                {
                    s = false;
                }
            }


            if ( s )
            {
                NewTable.Visible = false;
                btnNew.OnClientClick = "return (confirm('確定要新增嗎?'));";
                e.Row.Cells [ e.Row.Cells.Count - 6 ].Controls.Add ( btnNew );
                e.Row.Cells [ e.Row.Cells.Count - 5 ].Controls.Add ( CodeMasterNew01 );
                e.Row.Cells [ e.Row.Cells.Count - 4 ].Controls.Add ( CodeMasterNew02 );
                e.Row.Cells [ e.Row.Cells.Count - 3 ].Controls.Add ( CodeMasterNew03 );
                e.Row.Cells [ e.Row.Cells.Count - 2 ].Controls.Add ( CodeMasterNew04 );
                e.Row.Cells [ e.Row.Cells.Count - 1 ].Controls.Add ( CodeMasterNew05 );
            }

        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            e.Row.Visible = false;
            NewTable.Visible = GridView1.ShowFooter;           
        }
    }
    
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Style.Add ( HtmlTextWriterStyle.TextAlign , "center" );
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
                e.Row.Cells[i - 1].Style.Add("width", "70px");
            }
            e.Row.Cells[i].Style.Add("text-align", "right");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
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
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "OverTime_Master";
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
        Panel_Empty.Visible = hasData() ? false : true;
    }

    //
    private void SetWindowOpen ( object sender , string Url , string width , string height )
    {

        int top = ( System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - int.Parse ( height ) ) / 2;
        int left = ( System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - int.Parse ( width ) ) / 2;
        top = top > 0 ? top : 10;
        left = left > 0 ? left : 10;

        string Str = "javascript:var win =window.open('" + Url + "','','width=" + width + ",height=" + height + ",top=" + top + ",left=" + left + ",scrollbars=no,resizable=yes');";

        switch ( sender.GetType ( ).Name )
        {
            case "Button":
                Button BT = ( Button ) sender;
                BT.Attributes.Add ( "onclick" , Str );
                break;
            case "LinkButton":
                LinkButton LB = ( LinkButton ) sender;
                LB.Attributes.Add ( "onclick" , Str );
                break;
            case "ImageButton":
                ImageButton IB = ( ImageButton ) sender;
                IB.Attributes.Add ( "onclick" , Str );
                break;
        }
    }

}//  程式尾部
