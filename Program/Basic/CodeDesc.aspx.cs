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

public partial class CodeDesc : System.Web.UI.Page
{

    #region 共用參數

    string Ssql = "";
    UserInfo _UserInfo = new UserInfo ( );
    string _ProgramId = "PYB002";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand ( );

    #endregion

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected override void OnInit ( EventArgs e )
    {
        base.OnInit ( e );
        _MyDBM = new DBManger ( );
        _MyDBM.New ( );
    }

    //  身分驗證副程式
    private void AuthRight ( )
    {
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ( ( blCheckLogin == false ) || ( ConfigurationManager.AppSettings [ "EnableProgramAuth" ] != null && ConfigurationManager.AppSettings [ "EnableProgramAuth" ].ToString ( ) == "true" ) )
        {
            bool blCheckProgramAuth = false;
            if ( blCheckLogin == false )
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg ( "UnLogin" );
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission ( _ProgramId , "Detail" );
                if ( blCheckProgramAuth == false )
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg ( "NoRight" );
            }
        }
        
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[ ] Auth = { "Delete" , "Modify" , "Add" , "Maint" };

        if ( ConfigurationManager.AppSettings[ "EnableProgramAuth" ] != null && ConfigurationManager.AppSettings[ "EnableProgramAuth" ].ToString ( ) == "true" )
        {
            for ( i = 0 ; i < Auth.Length ; i++ )
            {
                Find = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, Auth[i]);
                //if (Find && (SetCss == false))
                //{
                    SetCss = true;
                    GridView1.Columns[GridView1.Columns.Count - 1].HeaderStyle.CssClass = "paginationRowEdgeLl";
                //}
                switch ( Auth[ i ] )
                {
                    case "Delete":
                    case "Modify":
                        GridView1.Columns[ i ].Visible = Find;
                        if ( Find && ( SetCss == false ) )
                        {
                            SetCss = true;
                            GridView1.Columns[ i ].HeaderStyle.CssClass = "paginationRowEdgeLl";

                        }
                        break;

                    case "Add":
                        GridView1.ShowFooter = Find;
                        break;
                    case "Maint":
                        //GridView1.Columns[ GridView1.Columns.Count - 1 ].Visible = Find;
                        //設定標題樣式
                        break;
                    default:
                        break;
                }
            }

            //查詢(執行)
            if ( ( _UserInfo.CheckPermission ( _ProgramId ) ) || Find )
            {
                Find = true;
            }
            else
            {
                Response.Redirect ( "\\" + Application[ "WebSite" ].ToString ( ) + "\\" + Application[ "ErrorPage" ].ToString ( ) + "?ErrMsg=NoRight" );
            }

            //版面樣式調整
            if ( SetCss == false )
            {
                GridView1.Columns[ ( Auth.Length - 2 ) ].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    //  載入網頁時執行此區
    protected void Page_Load ( object sender , EventArgs e )
    {
        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());
        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "AB", Page.ResolveUrl("../Pages/pagefunction.js").ToString(), true);
        btnCancel.Attributes.Add ( "onclick" , "javascript:window.close();" );
        lbl_Msg.Text = "";//清空訊息
        Navigator1.BindGridView = GridView1;
        if ( !Page.IsPostBack )
        {
            AuthRight ( );
            btnQuery.OnClientClick = "return (SaveValue(" + hid_status.ClientID + ",'btnQuery'));";
            SetCdName ( );
            BindData ( );
            showPanel ( );
        }
        else
        {
            if ( hid_status.Value == "Update" )
            {
                if (GridView1.EditIndex < 0)
                    BindData();
            }
            else if ( hid_status.Value == "News" && !string.IsNullOrEmpty ( hid_IsInsertExit.Value ) )
            {
                string temId = hid_IsInsertExit.Value.Replace ( "_" , "$" );
                if (Request.Form[temId + "$tbAddNew01"] != null && Request.Form[temId + "$tbAddNew01"].ToString().Trim() != "")
                {
                    //新增
                    btnEmptyNew_Click ( sender , e );
                    hid_IsInsertExit.Value = "";
                }
                BindData ( );
            }
            else
            {
                BindData ( );
            }

            

        }

        hid_status.Value = "";
    }

    //代碼名稱下拉式選單連動


    //代碼名稱下拉式選單
    private void SetCdName ( )
    {
        //CdName.AutoPostBack = true;
        //DataTable DT = _MyDBM.ExecuteDataTable ( "SELECT CodeID,CodeDecs FROM CodeMaster" );
        //CdName.Items.Add ( Li ( "全部" , "" ) );
        //for ( int i2 = 0 ; i2 < DT.Rows.Count ; i2++ )
        //{
        //    //if ( !DT.Rows[ i2 ][ "CodeID" ].ToString ( ).ToLower ( ).Contains ( "function" ) && !DT.Rows[ i2 ][ "CodeID" ].ToString ( ).ToLower ( ).Contains ( "sqlcomm" ) || _UserInfo.UData.Role.ToLower ( ) == "administrator" )
        //    //2010/12/31 michelle 修 Role 中會有多重角色,所以改用 Contains 才會正確;function開放給一般權限控管
        //    if (!DT.Rows[i2]["CodeID"].ToString().ToLower().Contains("sqlcomm") || _UserInfo.UData.Role.ToLower().Contains("administrator"))
        //    {
        //        CdName.Items.Add ( Li ( DT.Rows[ i2 ][ "CodeID" ].ToString ( ) +" - "+ DT.Rows[ i2 ][ "CodeDecs" ].ToString ( ) , DT.Rows[ i2 ][ "CodeID" ].ToString ( ) ) );
        //    }

        //}
    }

    //判斷搜尋時是否有此資料
    private bool hasData ( )
    {
        SetSsql ( );
        MyCmd.Parameters.Clear ( );
        DataTable tb = _MyDBM.ExecuteDataTable ( Ssql , MyCmd.Parameters , CommandType.Text );
        return tb.Rows.Count > 0 ? true : false;
    }

    //綁定資料至Gridview
    private void BindData ( )
    {
        SetSsql ( );
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind ( );
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind ( );
    }
    //設定SQL
    private void SetSsql ( )
    {
        Ssql = "SELECT * FROM CodeDesc CD where CD.CodeID ='"+ Request.QueryString["Code"].Replace(".","#") +"'";

        if ( txtCodeCode.Text.Length > 0 )
        {
            Ssql += string.Format(" And CD.CodeCode like '%{0}%'", _UserInfo.SysSet.CleanSQL(txtCodeCode.Text).ToString());
        }
    }

    //當按下搜尋按鈕時
    protected void btnQuery_Click ( object sender , EventArgs e )
    {

        lbl_Msg.Text = string.Empty;
        BindData ( );
        hid_IsInsertExit.Value = "";
    }


    #region #新增設定區#
    //  新增按鈕
    protected void btnEmptyNew_Click ( object sender , EventArgs e )
    {


        if ( hid_IsInsertExit.Value.Length > 1 )
        {
            string temId = hid_IsInsertExit.Value.Replace ( "_" , "$" ) + "$tbAddNew0";
            string DDId = hid_IsInsertExit.Value.Replace ( "_" , "$" ) + "$DropDownList";

            if ( ValidateData ( Request.Form [ DDId + "1" ].ToString ( ) , Request.Form [ temId + "1" ].ToString ( ) ) )
            {
                SDS_GridView.InsertParameters.Clear ( );
                SDS_GridView.InsertParameters.Add ( "CodeID" , Request.Form [ DDId + "1" ].ToString ( ) );
                SDS_GridView.InsertParameters.Add ( "CodeCode" , Request.Form [ temId + "1" ].ToString ( ) );
                SDS_GridView.InsertParameters.Add ( "CodeName" , Request.Form [ temId + "2" ].ToString ( ) );
                SDS_GridView.InsertParameters.Add ( "Maint" , Request.Form [ DDId + "2" ].Replace ( "啟用" , "Y" ).Replace ( "關閉" , "N" ).ToString ( ) );

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

                lbl_Msg.Text = i == 1 ? i.ToString ( ) + " 個資料列 " + "新增成功!!" : "新增失敗!!" + lbl_Msg.Text;
                WriteLog ( false , "A" , i );
            }

            BindData ( );
            hid_IsInsertExit.Value = "";
        }
        else
        {
            if ( ValidateData ( DropDownList1.SelectedValue , tbAddNew01.Text ) )
            {
                SDS_GridView.InsertParameters.Clear ( );
                SDS_GridView.InsertParameters.Add ( "CodeID" , DropDownList1.SelectedValue );
                SDS_GridView.InsertParameters.Add ( "CodeCode" , tbAddNew01.Text );
                SDS_GridView.InsertParameters.Add ( "CodeName" , tbAddNew02.Text );
                SDS_GridView.InsertParameters.Add ( "Maint" , DropDownList2.SelectedValue );

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

                lbl_Msg.Text = i == 1 ? i.ToString ( ) + " 個資料列 " + "新增成功!!" : "新增失敗!!" + lbl_Msg.Text;
                WriteLog ( false , "A" , i );
            }
            BindData ( );
            hid_IsInsertExit.Value = "";
            tbAddNew01.Text = "";
            tbAddNew02.Text = "";


        }
    }

    //  判斷資料是否重覆
    private bool ValidateData ( string strCodeID , string strCodeCode )
    {
        //判斷資料是否重覆
        Ssql = "Select CodeID From CodeDesc WHERE CodeID = '" + strCodeID + "' and CodeCode= '" + strCodeCode + "'";

        DataTable tb = _MyDBM.ExecuteDataTable ( Ssql );

        if ( tb.Rows.Count > 0 )
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            return false;
        }
        else
            return true;
    }
    #endregion


    protected void btnDelete_Click ( object sender , EventArgs e )
    {
        LinkButton btnDelete = ( LinkButton ) sender;
        string L1PK = btnDelete.Attributes[ "L1PK" ].ToString ( );
        string L2PK = btnDelete.Attributes[ "L2PK" ].ToString ( );

        string sql = "Delete From CodeDesc  Where CodeID='" + L1PK + "' AND CodeCode = '" + L2PK + "'";

        WriteLog ( true , "D" , 0 );

        int result = _MyDBM.ExecuteCommand ( sql );

        WriteLog ( false , "D" , result > 0 ? 1 : 0 );

        lbl_Msg.Text = result > 0  ?"資料刪除成功 !!":"資料刪除失敗 !!";

        BindData ( );
    }


    #region #更新設定區#
    protected void GridView1_RowUpdating ( object sender , GridViewUpdateEventArgs e )
    {
        string DDId = hid_IsInsertExit.Value.Replace ( "_" , "$" ) + "$DD2";
        DataTable DT2 = _MyDBM.ExecuteDataTable ( "SELECT CodeID FROM CodeMaster Where CodeID = '" + e.Keys[ "CodeID" ] + "'" );
        if ( DT2.Rows.Count > 0 )
            e.Keys[ "CodeID" ] = DT2.Rows[ 0 ][ "CodeID" ].ToString ( );
        e.Keys.Add ( "Maint" , Request.Form[ DDId ].Replace ( "啟用" , "Y" ).Replace ( "關閉" , "N" ).ToString ( ) );
        #region 開始異動前,先寫入LOG
        //TableName	異動資料表	varchar	60
        //TrxType	異動類別(A/U/D)	char	1
        //ChangItem	異動項目(欄位)	varchar	255
        //SQLcommand	異動項目(異動值/指令)	varchar	2000
        //ChgStartDateTime	異動開始時間	smalldatetime	
        //ChgStopDateTime	異動結束時間	smalldatetime	
        //ChgUser	使用者代號	nvarchar	32
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear ( );
        MyCmd.Parameters.Add ( "@TableName" , SqlDbType.VarChar , 60 ).Value = "OverTime_Master";
        MyCmd.Parameters.Add ( "@TrxType" , SqlDbType.Char , 1 ).Value = "U";
        MyCmd.Parameters.Add ( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = "ALL";
        MyCmd.Parameters.Add ( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = "";
        MyCmd.Parameters.Add ( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add ( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog ( MyCmd.Parameters );
        #endregion


    }

    protected void GridView1_RowUpdated ( object sender , GridViewUpdatedEventArgs e )
    {
        if ( e.Exception == null )
        {
            GridView1.EditIndex = -1;
            lbl_Msg.Text = e.AffectedRows.ToString ( ) + " 個資料列 " + "更新成功!!";
            BindData ( );
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "更新失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters[ "@SQLcommand" ].Value = ( e.Exception == null ) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add ( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
        _MyDBM.DataChgLog ( MyCmd.Parameters );
        #endregion

    }
    #endregion

    //GridView1_RowDataBound
    protected void GridView1_RowDataBound ( object sender , GridViewRowEventArgs e )
    {
        string strValue = "";

        if ( e.Row.RowType == DataControlRowType.DataRow )
        {
            if ( ( e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit )
                || ( e.Row.RowState == ( System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit ) ) )
            {
                #region 修改用

                //確認
                if ( e.Row.Cells[ 1 ].Controls[ 0 ] != null )
                {
                    ImageButton IB = ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 0 ];
                    IB.Attributes.Add("onclick", "return (confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "') && SaveValue(" + hid_status.ClientID + ",'Update'));");                    
                    IB.Attributes.Add ( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add ( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add ( "filter" , "alpha(opacity=50)" );

                }
                //取消
                if ( e.Row.Cells[ 1 ].Controls[ 2 ] != null )
                {
                    ImageButton IB = ( ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 2 ] );
                    IB.Attributes.Add ( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add ( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add ( "filter" , "alpha(opacity=50)" );
                }
                #region @   TextBox 對齊,大小 設定    @
                for ( int i = 4 ; i < 5 ; i++ )
                {
                    ( ( TextBox ) e.Row.Cells[ i ].Controls[ 0 ] ).Style.Add ( "text-align" , "right" );
                    ( ( TextBox ) e.Row.Cells[ i ].Controls[ 0 ] ).Width = i == 4 ? 240 : 50;
                    ( ( TextBox ) e.Row.Cells[ i ].Controls[ 0 ] ).MaxLength = i == 4 ? 32 : 4;
                }
                #endregion

                #region @   維 護 碼 下拉式選單 @
                //先隱藏TextBox
                ( ( TextBox ) e.Row.Cells[ 5 ].Controls[ 0 ] ).Visible = false;
                DropDownList DD1 = new DropDownList ( );
                DD1.ID = "DD2";
                DD1.Items.Add ( "啟用" );
                DD1.Items.Add ( "關閉" );
                DD1.Width = 64;
                DD1.SelectedValue = ( ( TextBox ) e.Row.Cells[ 5 ].Controls[ 0 ] ).Text.Replace ( "Y" , "啟用" ).Replace ( "N" , "關閉" );
                e.Row.Cells[ 5 ].Controls.Add ( DD1 );
                #endregion

                DataTable DT2 = _MyDBM.ExecuteDataTable ( "SELECT CodeDecs FROM CodeMaster Where CodeID = '" + e.Row.Cells[ 2 ].Text + "'" );
                if ( DT2.Rows.Count > 0 )
                    e.Row.Cells[ 2 ].Text = DT2.Rows[ 0 ][ "CodeDecs" ].ToString ( );

                #endregion
            }
            else
            {
                #region 查詢用
                if ( e.Row.Cells[ 1 ].Controls[ 0 ] != null )
                {
                    ImageButton IB = ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 0 ];
                    IB.Attributes.Add ( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add ( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add ( "filter" , "alpha(opacity=50)" );
                }
                bool Find = false;
                Find = _UserInfo.CheckPermission ( _ProgramId , "Maint" );
                //Request.QueryString [ "Code" ].Replace(".","#")
                string strSQL = "SELECT Maint,CodeID FROM CodeDesc ";
                strValue = e.Row.Cells[2].Text.Replace("&nbsp;", "").Trim();
                if (strValue.Length != 0)
                    strSQL += "Where CodeID = '" + strValue + "'";
                else if (Request.QueryString["Code"].Replace(".", "#").Length!=0)
                    strSQL += "Where CodeID = '" + Request.QueryString["Code"].Replace(".", "#") + "'";
                if ( !Find )
                {
                    strValue = e.Row.Cells[3].Text.Replace("&nbsp;", "").Trim();

                    if (strValue.Length != 0)
                        strSQL += " And CodeCode = '" + strValue + "'";
                    DataTable DT = _MyDBM.ExecuteDataTable(strSQL);
                    if ( ( e.Row.Cells[ 2 ].Text.Length > 0 && e.Row.Cells[ 3 ].Text.Length > 0 ) || DT.Rows[ 0 ][ "Maint" ].ToString ( ) == "N" )
                    {
                        if ((_UserInfo.UData.Role.Contains("Administrator") != true) && (DT.Rows[0]["CodeID"].ToString().ToLower().Contains("function") || DT.Rows[0]["CodeID"].ToString().ToLower().Contains("sqlcomm")))
                            e.Row.Visible = Find;
                    }
                }

                strSQL = "SELECT CodeDecs,CodeID FROM CodeMaster Where CodeID = '" + e.Row.Cells[2].Text + "'";
                DataTable DT2 = _MyDBM.ExecuteDataTable(strSQL);
                if ( DT2.Rows.Count > 0 )
                    e.Row.Cells[ 2 ].Text = DT2.Rows[ 0 ][ "CodeID" ].ToString ( ) + " - " + DT2.Rows[ 0 ][ "CodeDecs" ].ToString ( );


                #region @   維護碼顯示修改  @
                e.Row.Cells[ 5 ].Text = e.Row.Cells[ 5 ].Text.Replace ( "Y" , "啟用" ).Replace ( "N" , "關閉" );

                #endregion

                #region @   欄位對齊設定    @
                for ( int i = 2 ; i < e.Row.Cells.Count ; i++ )
                {
                    e.Row.Cells[ i ].Style.Add ( "text-align" , "left" );
                    e.Row.Cells[ i ].Width = i == 2 ? 150 : i == 3 ? 30 : 100;
                }
                #endregion

                #region @   欄位對齊設定    @

                e.Row.Cells[ 2 ].Width = 130;
                e.Row.Cells[ 3 ].Width = 200;
                e.Row.Cells[ 5 ].Width = 100;
                #endregion


                #endregion


            }
        }
        else if ( e.Row.RowType == DataControlRowType.Footer )
        {
            #region 新增用欄位
            strValue = "";
            //代碼欄位 下拉式選單
            DropDownList DD1 = new DropDownList ( );
            DD1.Items.Add ( Request.QueryString [ "Code" ].Replace(".","#") );
            DD1.ID = "DropDownList1";
            DD1.Width = 210;
            e.Row.Cells[ 2 ].Controls.Add ( DD1 );

            //前四欄的 TextBox 設定
            for ( int i = 3 ; i < 5 ; i++ )
            {
                TextBox tbAddNew = new TextBox ( );
                tbAddNew.ID = "tbAddNew" + ( i - 2 ).ToString ( ).PadLeft ( 2 , '0' );
                #region @   新增欄位時的設定    @
                int n = 0;
                int n1 = 0;
                string a = "left";
                switch ( i )
                {

                    case 3:

                        n = 10;
                        n1 = 100;
                        break;
                    case 4:

                        n = 32;
                        n1 = 240;
                        break;
                    default:
                        n = 0;
                        break;
                }
                tbAddNew.Style.Add ( "text-align" , a );
                tbAddNew.MaxLength = n;
                tbAddNew.Width = n1;
                #endregion
                e.Row.Cells[ i ].Controls.Add ( tbAddNew );
                strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
            }
            //維 護 碼 下拉式選單
            DropDownList DD2 = new DropDownList ( );
            DD2.ID = "DropDownList2";
            DD2.Items.Add ( "啟用" );
            DD2.Items.Add ( "關閉" );
            DD2.Width = 64;
            e.Row.Cells[ 5 ].Controls.Add ( DD2 );
            strValue += "checkColumns(" + DD2.ClientID + ") && ";

            NewAdd.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "') && SaveValue(" + hid_status.ClientID + ",'News'));";            
            
            e.Row.Cells [ 1 ].Controls.Add ( NewAdd );

            #endregion

        }
        else if ( e.Row.RowType == DataControlRowType.EmptyDataRow )
        {
            //權限
            e.Row.Visible = GridView1.ShowFooter;
            NewTable.Visible = GridView1.ShowFooter;

            e.Row.FindControl ( "BOX" ).Controls.Add ( NewTable );
            DropDownList1.Items.Clear();
            DropDownList2.Items.Clear ( );
            DropDownList1.Items.Add ( Request.QueryString [ "Code" ].Replace ( "." , "#" ) );
            if ( _UserInfo.UData.Role.ToString ( ).ToLower ( ).Contains("administrator") )
            {
                DropDownList2.Items.Add ( new ListItem ( "啟用" , "Y" ) );
                DropDownList2.Items.Add ( new ListItem ( "關閉" , "N" ) );
            }
            else
            {
                DropDownList2.Items.Add ( new ListItem ( "啟用" , "Y" ) );
                DropDownList2.Visible = false;
                e.Row.FindControl ( "EmptyMaint" ).Visible = false;
            }
        }
    }

    void DD1_SelectedIndexChanged ( object sender , EventArgs e )
    {
        tempNews.Value = ( ( DropDownList ) sender ).SelectedIndex.ToString ( );
        tempNewscid.Value = ( ( DropDownList ) sender ).ClientID;
    }

    protected void GridView1_RowCreated ( object sender , GridViewRowEventArgs e )
    {
        if ( e.Row.RowType == DataControlRowType.Header )
        {
            e.Row.Cells[ 0 ].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[ e.Row.Cells.Count - 1 ].CssClass = "paginationRowEdgeRl";

        }
        else if ( ( e.Row.RowType == DataControlRowType.DataRow ) || ( e.Row.RowType == DataControlRowType.Footer ) || ( e.Row.RowType == DataControlRowType.EmptyDataRow ) )
        {
            e.Row.Attributes.Add ( "onmouseover" , "setnewcolor(this);" );
            e.Row.Attributes.Add ( "onmouseout" , "setoldcolor(this);" );
            int i = 0;
            for ( i = 0 ; i < e.Row.Cells.Count ; i++ )
            {
                e.Row.Cells[ i ].CssClass = "Grid_GridLine";
            }

            i = e.Row.Cells.Count - 1;
            if ( i > 0 )
            {
                //e.Row.Cells[ i - 1 ].Style.Add ( "text-align" , "right" );
                e.Row.Cells[ i - 1 ].Style.Add ( "width" , "100px" );
            }
            e.Row.Cells[ i ].Style.Add ( "text-align" , "right" );

        }
        else if ( e.Row.RowType == DataControlRowType.Pager )
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    protected void GridView1_RowEditing ( object sender , GridViewEditEventArgs e )
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData ( );
    }

    protected void GridView1_RowCancelingEdit ( object sender , GridViewCancelEditEventArgs e )
    {
        GridView1.EditIndex = -1;
        BindData ( );
    }

    protected void GridView1_RowCommand ( object sender , GridViewCommandEventArgs e )
    {
        if ( !String.IsNullOrEmpty ( e.CommandName ) )
        {
            try
            {
                //GridViewRow gvRow = null;
                switch ( e.CommandName )
                {
                    case "Delete":
                    case "Update":
                    case "Insert":
                        break;
                    case "Edit":
                    case "Cancel":
                    default:
                        lbl_Msg.Text = "";
                        break;
                }
            }
            catch ( Exception ex )
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    private void WriteLog ( bool n , string c , int i )
    {
        if ( n )
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
            MyCmd.Parameters.Clear ( );
            MyCmd.Parameters.Add ( "@TableName" , SqlDbType.VarChar , 60 ).Value = "OverTime_Master";
            MyCmd.Parameters.Add ( "@TrxType" , SqlDbType.Char , 1 ).Value = c;
            MyCmd.Parameters.Add ( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = "ALL";
            MyCmd.Parameters.Add ( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = "";
            MyCmd.Parameters.Add ( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add ( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog ( MyCmd.Parameters );
            #endregion
        }
        else
        {
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters[ "@SQLcommand" ].Value = ( i == 1 ) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add ( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
            _MyDBM.DataChgLog ( MyCmd.Parameters );
            #endregion
        }
    }

    //控制頁面 Panel，查無資料時顯示用
    private void showPanel ( )
    {
        Panel_Empty.Visible = hasData ( ) ? false : true;
    }

    //ListItem小程式
    private ListItem Li ( string Name , string Value )
    {
        ListItem li = new ListItem ( );
        li.Text = Name;
        li.Value = Value;
        return li;
    }

}//  程式尾部
