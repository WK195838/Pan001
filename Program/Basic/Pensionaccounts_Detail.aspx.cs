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

public partial class Pensionaccounts_Detail : System.Web.UI.Page
{

    #region 共用參數

    string Ssql = "";
    UserInfo _UserInfo = new UserInfo( );
    Payroll P = new Payroll ( );
    string _ProgramId = "PYQ001";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand( );
    SysSetting SysSet = new SysSetting( );

    #endregion

    #region 資料設定
    string DataSouceName = "Pensionaccounts_Detail";
    string PayrollTable = "PayrollWorking";

    string[ ] DataKey = { 
        "Company", 
        "EmployeeId", 
        "salary_month", 
        "BeginDateTime" 
    };

    string[ ] DataName = {
        "Company",
        "EmployeeId",
        "salary_month",
        "BeginDateTime",
        "EndDateTime",
        "hours",
        "days",
        "ApproveDate",
        "Payroll_Processingmonth"
    };

    #endregion

    decimal ActualAmount_S = 0, ActualAmount_C = 0;

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";

        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "PayrolltestWorking";
    }

    protected override void OnInit( EventArgs e )
    {
        base.OnInit( e );
        _MyDBM = new DBManger( );
        _MyDBM.New( );
        if ( _UserInfo.SysSet.isTWCalendar )
        {
            System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo( "zh-TW" );
            cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar( );
            System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        }
        DataTable DT = _MyDBM.ExecuteDataTable( "Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/OverTimeTrx.aspx'" );
        if ( DT.Rows.Count > 0 )
            _ProgramId = DT.Rows[ 0 ][ 0 ].ToString( );
    }

    //  身分驗證副程式
    private void AuthRight( )
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[ ] Auth = { "Delete" , "Modify" , "Add" };

        if ( ConfigurationManager.AppSettings[ "EnableProgramAuth" ] != null && ConfigurationManager.AppSettings[ "EnableProgramAuth" ].ToString( ) == "true" )
        {
            for ( i = 0 ; i < Auth.Length ; i++ )
            {
                Find = _UserInfo.CheckPermission( _ProgramId , Auth[ i ] );
                if ( i < ( Auth.Length - 1 ) )
                {//刪/修/詳
                    GridView1.Columns[ i ].Visible = Find;
                    //設定標題樣式
                    if ( Find && ( SetCss == false ) )
                    {
                        SetCss = true;
                        GridView1.Columns[ i ].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    GridView1.ShowFooter = Find;
                }
            }

            //查詢(執行)
            if ( ( _UserInfo.CheckPermission( _ProgramId ) ) || Find )
            {
                Find = true;
            }
            else
            {
                Response.Redirect( "\\" + Application[ "WebSite" ].ToString( ) + "\\" + Application[ "ErrorPage" ].ToString( ) + "?ErrMsg=NoRight" );
            }

            //版面樣式調整
            if ( SetCss == false )
            {
                GridView1.Columns[ ( Auth.Length - 1 ) ].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    //  載入網頁時執行此區
    protected void Page_Load( object sender , EventArgs e )
    {
        lbl_Msg.Text = "";//清空訊息
        Navigator1.BindGridView = GridView1;

        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged ( SearchList1_SelectedChanged );

        if ( !Page.IsPostBack )
        {
            SetDate( );
            if ( _UserInfo.AuthLogin == false )
            {
                Response.Redirect( "\\" + Application[ "WebSite" ].ToString( ) + "\\" + Application[ "ErrorPage" ].ToString( ) + "?ErrMsg=UnLogin" );
            }
            else
            {
                if ( _UserInfo.CheckPermission( _ProgramId ) == false )
                    Response.Redirect( "\\" + Application[ "WebSite" ].ToString( ) + "\\" + Application[ "ErrorPage" ].ToString( ) + "?ErrMsg=NoRight" );
            }
            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            SetResignCode();
            BindData( );
            showPanel( );
            AuthRight( );
        }
        else
        {
            if (Session["QueryTable"] != null)
                BindData(Session["QueryTable"].ToString());
            else
                BindData();
            showPanel( );
        }

    }

    private void SetDate( )
    {
        YearList.Items.Add( Li( "全部" , "" ) );
        for ( int i = 2005 ; i < DateTime.Today.Year + 5 ; i++ )
        {
            YearList.Items.Add( Li( Convert.ToString( i - 1911 ) , i.ToString( ) ) );
        }
        MonthsList.Items.Add( Li( "全部" , "" ) );
        for ( int i = 1 ; i < 13 ; i++ )
        {
            MonthsList.Items.Add( i.ToString( ).PadLeft( 2 , '0' ) );
        }

    }

    // 搜尋模組連動
    void SearchList1_SelectedChanged ( object sender , SearchList.SelectEventArgs e )
    {
        BindData ( );
        showPanel ( );
    }

    private ListItem Li( string text , string value )
    {
        ListItem li = new ListItem( );
        li.Text = text;
        li.Value = value;
        return li;
    }

    protected void btnQuery_Click( object sender , EventArgs e )
    {
        lbl_Msg.Text = "";
        BindData( );
        showPanel( );
    }

    private void SetSsql(string QueryTable)
    {
        SetSsql(QueryTable, "");
    }

    //設定搜尋條件
    private void SetSsql(string QueryTable, string strDeCodeKey)
    {
        Session["QueryTable"] = QueryTable;
        if (QueryTable.Equals(DataSouceName))
        {
            if (strDeCodeKey == "")
                Ssql = " PD.* ,HireDate, ResignDate ";
            else
                Ssql = " Sum(" + strDeCodeKey + "(ActualAmount_C))  ";

            Ssql = "SELECT " + Ssql +
                " FROM " + QueryTable + " PD left join Personnel_Master PM On PD.EmployeeId = PM.EmployeeId And PD.Company = PM.Company Where 0=0 ";
        }
        else
        {
            if (strDeCodeKey == "")
                Ssql = " PD.*,null as [ActualDate] " +
                " ,(select Top 1 [SalaryAmount] FROM " + QueryTable + " PDC where PD.Company = PDC.Company And PD.EmployeeId = PDC.EmployeeId And SalaryYM=salary_month And SalaryItem='16') as [ActualAmount_C] " +
                " ,HireDate, ResignDate ";
            else
                Ssql = " Sum(" + strDeCodeKey + "(ActualAmount_C))  ";

            Ssql = "SELECT  " + Ssql +
                " FROM (select [Company],[EmployeeId],[SalaryYM] as [salary_month],[SalaryAmount] as [ActualAmount_S],PeriodCode FROM " + QueryTable + " where SalaryItem='06') " +
                " PD left join Personnel_Master PM On PD.EmployeeId = PM.EmployeeId And PD.Company = PM.Company Where 0=0 ";
        }
        //公司
        DDLSr ( "PD.Company" , SearchList1.CompanyValue );

        //部門
        DDLSr ( "PM.deptid " , SearchList1.DepartmentValue );

        //員工
        DDLSr ( "PD.EmployeeId" , SearchList1.EmployeeValue );


        if ( YearList.SelectedValue.Length > 0 || MonthsList.SelectedValue.Length > 0 )
        {
            Ssql += string.Format ( " And PD.salary_month like '{0}%%{1}'" , YearList.SelectedValue , MonthsList.SelectedValue );
        }

        if (cbResignC.Items.Count > 0)
        {//2012/05/18 依人事要求加入離職篩選條件
            string strTL = "";
            foreach (ListItem lis in cbResignC.Items)
            {
                if (lis.Selected)
                {
                    strTL += "'" + lis.Value + "',";
                }
            }
            if (strTL.Length > 0)
            {
                Ssql += " and ResignCode in (" + strTL + "'-')";
            }
        }

        if (strDeCodeKey == "")
            Ssql += " Order By PD.Company,PD.EmployeeId,Convert(int,PD.salary_month) ";
        else
            Ssql += " Group By PD.Company ";
    }
    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr ( string Name , string Value )
    {
        if ( Value.Length > 0 )
            Ssql += string.Format ( " And " + Name + " like '%{0}%'" , Value );
        else
            Ssql += string.Format ( " And " + Name + " = '{0}'" , Value );
    }


    //  判斷搜尋時是否有此資料
    private bool hasData( )
    {
        return GridView1.Rows.Count > 0 ? true : false;
    }

    private void BindData()
    {
        BindData(DataSouceName);
    }

    //  綁定資料至Gridview
    private void BindData(string QueryTable)
    {
        ActualAmount_S = 0;
        ActualAmount_C = 0;
        Payroll py = new Payroll();
        Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
        PayrollLt.DeCodeKey = "dbo.PensionaccountsDetail" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(PayrollLt.DeCodeKey);        
        SetSsql(QueryTable, PayrollLt.DeCodeKey);
        try
        {
            DataTable theDTT = _MyDBM.ExecuteDataTable(Ssql);
            labTotalActualAmount_C.Text = "本次查詢之企業提繳總額：" + ((decimal)theDTT.Rows[0][0]).ToString("N0");
        }
        catch { labTotalActualAmount_C.Text = ""; }
        py.AfterQuery(PayrollLt.DeCodeKey);

        SetSsql(QueryTable);
        //SDS_GridView.SelectCommand =Ssql;
        //GridView1.DataBind( );
        //Navigator1.BindGridView = GridView1;
        //Navigator1.DataBind( );        
        DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

        GridView1.Visible = false;
        Navigator1.Visible = false;
        lbl_Msg.Text = "";
        if (theDT.Rows.Count > 0)
        {
            GridView1.DataSource = theDT;
            GridView1.DataBind();
            GridView1.Visible = true;

            //if (GridView1.PageCount > 1)
            {
                Navigator1.BindGridView = GridView1;
                Navigator1.DataBind();
                Navigator1.Visible = true;
            }
        }
        else
        {
            lbl_Msg.Text = "查無資料!!";
        }

    }

    #region #新增設定區#
    //  新增按鈕
    protected void btnEmptyNew_Click( object sender , EventArgs e )
    {
        if ( hid_IsInsertExit.Value.Length > 0 )
        {
            //參數設定---------------------------
            string temId = hid_IsInsertExit.Value.Replace( "_" , "$" ) + "$tbAddNew0";
            //Company
            string Company = SearchList1.CompanyValue.Trim ( );
            //EmployeeId
            string EmployeeId = Request.Form[ temId + "0" ].Remove( Request.Form[ temId + "0" ].IndexOf( " " ) );
            //salary_month
            string salary_month = Request.Form[ temId + "1" ] + Request.Form[ temId + "2" ];
            //ActualDate
            string ActualDate = SysSet.FormatADDate( Request.Form[ temId + "3" ] );
            //ActualAmount_S
            string ActualAmount_S = Request.Form[ temId + "4" ];
            //ActualAmount_C
            string ActualAmount_C = Request.Form[ temId + "5" ];

            //-----------------------------------

            if ( ValidateData( "Company" , Company , "EmployeeId" , EmployeeId , "salary_month" , salary_month ) )
            {

                SDS_GridView.InsertParameters.Clear( );
                SDS_GridView.InsertParameters.Add( "Company" , Company );
                SDS_GridView.InsertParameters.Add( "EmployeeId" , EmployeeId );
                SDS_GridView.InsertParameters.Add( "salary_month" , salary_month );
                SDS_GridView.InsertParameters.Add( "ActualDate" , ActualDate );
                SDS_GridView.InsertParameters.Add( "ActualAmount_S" , ActualAmount_S );
                SDS_GridView.InsertParameters.Add( "ActualAmount_C" , ActualAmount_C );

                WriteLog( true , "A" , 0 );

                int i = 0;
                try
                {
                    i = SDS_GridView.Insert( );
                }
                catch ( Exception ex )
                {
                    lbl_Msg.Text = ex.Message;
                }
                if ( i == 1 )
                {
                    lbl_Msg.Text = i.ToString( ) + " 個資料列 " + "新增成功!!";
                }
                else
                {
                    lbl_Msg.Text = "新增失敗!!" + lbl_Msg.Text;
                }
                WriteLog( false , "A" , i );
            }

            BindData( );
            hid_IsInsertExit.Value = "";
        }
    }

    //  判斷資料是否重覆
    private bool ValidateData( string Key1 , string strID1 , string Key2 , string strID2 , string Key3 , string strID3 )
    {
        Ssql = "Select Pensionaccounts_Detail.* FROM Pensionaccounts_Detail  Where " + Key1 + " = '" + strID1 +
     "' and " + Key2 + " ='" + strID2 +
     "' and " + Key3 + " ='" + strID3 + "'";

        DataTable tb = _MyDBM.ExecuteDataTable( Ssql );

        if ( tb.Rows.Count > 0 )
        {
            lbl_Msg.Text = "新增失敗!!  原因: 資料重覆";
            return false;
        }
        return true;
    }
    #endregion

    #region #刪除設定區#

    protected void btnDelete_Click( object sender , EventArgs e )
    {
        LinkButton btnDelete = ( LinkButton ) sender;
        string L1PK = btnDelete.Attributes[ "L1PK" ].ToString( );
        string L2PK = btnDelete.Attributes[ "L2PK" ].ToString( );
        string L3PK = btnDelete.Attributes[ "L3PK" ].ToString( );

        string sql = "Delete FROM Pensionaccounts_Detail  Where Company='" + L1PK + "' and EmployeeId='" + L2PK + "' and salary_month = '" + L3PK + "'";

        WriteLog( true , "D" , 0 );

        int result = _MyDBM.ExecuteCommand( sql.ToString( ) );

        WriteLog( false , "D" , result > 0 ? 1 : 0 );

        if ( result > 0 )
        {
            lbl_Msg.Text = "資料刪除成功 !!";
            Navigator1.DataBind( );
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";
        }
        BindData( );
    }

    protected void GridView1_RowDeleted( object sender , GridViewDeletedEventArgs e )
    {
        if ( e.Exception == null )
        {
            lbl_Msg.Text = e.AffectedRows.ToString( ) + " 個資料列 " + "刪除成功!!";
            BindData( );
            hid_IsInsertExit.Value = "";
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
            return;
        }
        showPanel( );
    }

    protected void GridView1_RowDeleting( object sender , GridViewDeleteEventArgs e )
    {
        //
    }
    #endregion

    #region #更新設定區#
    protected void GridView1_RowUpdating( object sender , GridViewUpdateEventArgs e )
    {
        string DDId = hid_IsInsertExit.Value.Replace( "_" , "$" ) + "$UDD";
        string temId = hid_IsInsertExit.Value.Replace( "_" , "$" ) + "$ctl0";
        
        if ( hid_IsInsertExit.Value.Length > 1 )
        {
            e.NewValues[ "ActualDate" ] = SysSet.FormatADDate( e.NewValues[ "ActualDate" ].ToString( ) );
            #region 開始異動前,先寫入LOG
            //TableName	異動資料表	varchar	60
            //TrxType	異動類別(A/U/D)	char	1
            //ChangItem	異動項目(欄位)	varchar	255
            //SQLcommand	異動項目(異動值/指令)	varchar	2000
            //ChgStartDateTime	異動開始時間	smalldatetime	
            //ChgStopDateTime	異動結束時間	smalldatetime	
            //ChgUser	使用者代號	nvarchar	32
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear( );
            MyCmd.Parameters.Add( "@TableName" , SqlDbType.VarChar , 60 ).Value = "OverTimeTrx";
            MyCmd.Parameters.Add( "@TrxType" , SqlDbType.Char , 1 ).Value = "U";
            MyCmd.Parameters.Add( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = "ALL";
            MyCmd.Parameters.Add( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = "";
            MyCmd.Parameters.Add( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog( MyCmd.Parameters );
            #endregion
        }
    }

    protected void GridView1_RowUpdated( object sender , GridViewUpdatedEventArgs e )
    {
        if ( e.Exception == null )
        {
            GridView1.EditIndex = -1;
            lbl_Msg.Text = e.AffectedRows.ToString( ) + " 個資料列 " + "更新成功!!";
            BindData( );
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
        MyCmd.Parameters.Add( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
        _MyDBM.DataChgLog( MyCmd.Parameters );
        #endregion

    }
    #endregion


    protected void GridView1_RowDataBound( object sender , GridViewRowEventArgs e )
    {
        string strValue = "";

        if ( e.Row.RowType == DataControlRowType.DataRow )
        {
            if ( ( e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit ) || ( e.Row.RowState == ( System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit ) ) )
            {
                #region 修改用

                //確認
                if ( e.Row.Cells[ 1 ].Controls[ 0 ] != null )
                {
                    ImageButton IB = ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 0 ];
                    IB.Attributes.Add( "onclick" , "if(confirm('確定要修改資料嗎?')){SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');SaveValue(" + HiddenField1.ClientID + ",'UpData');}" );
                    IB.Attributes.Add( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add( "filter" , "alpha(opacity=50)" );

                }
                //取消
                if ( e.Row.Cells[ 1 ].Controls[ 2 ] != null )
                {
                    ImageButton IB = ( ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 2 ] );
                    IB.Attributes.Add( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add( "filter" , "alpha(opacity=50)" );
                }
                //員工編號
                e.Row.Cells[2].Text = SearchList1.Employee.Items.FindByValue(e.Row.Cells[2].Text.Trim()).Text;
                //薪資提撥月份
                e.Row.Cells[ 3 ].Text = YearList.Items.FindByValue( e.Row.Cells[ 3 ].Text.Substring( 0 , 4 ) ).Text + "/" + e.Row.Cells[ 3 ].Text.Substring( 3 , 2 );

                //員工提撥日期
                //員工提撥金額
                //企業提繳金
                for ( int n = 4 ; n < 7 ; n++ )
                {
                    TextBox tb = ( TextBox ) e.Row.Cells[ n ].Controls[ 0 ];
                    tb.Text = n == 4 ? SysSet.FormatDate( tb.Text ) :tb.Text;
                    tb.Width = n == 4 ? 60 : 120;
                    tb.Style.Add( "text-align" , "right" );
                    if ( n == 4 )
                    {
                        //日曆元件
                        ImageButton btOpenCal = new ImageButton( );
                        btOpenCal.ID = "btnCalendar";
                        btOpenCal.SkinID = "Calendar1";
                        btOpenCal.OnClientClick = "return GetPromptDate(" + tb.ClientID + ");";
                        e.Row.Cells[ n ].Controls.Add( btOpenCal );
                    }
                }
                
                #endregion
            }
            else
            {
                #region 查詢用
                if ( e.Row.Cells[ 1 ].Controls[ 0 ] != null )
                {
                    ImageButton IB = ( ImageButton ) e.Row.Cells[ 1 ].Controls[ 0 ];
                    IB.Attributes.Add( "onmouseout" , "this.filters['alpha'].opacity=50" );
                    IB.Attributes.Add( "onmouseover" , "this.filters['alpha'].opacity=100" );
                    IB.Style.Add( "filter" , "alpha(opacity=50)" );
                }
                //員工編號
                try
                {
                    e.Row.Cells[ 2 ].Text = SearchList1.Employee.Items.FindByValue ( e.Row.Cells[ 2 ].Text.Trim ( ) ).Text;
                }
                catch
                { }
                //薪資提撥月份
                e.Row.Cells[ 3 ].Text = YearList.Items.FindByValue( e.Row.Cells[ 3 ].Text.Substring( 0 , 4 ) ).Text + "/" + e.Row.Cells[ 3 ].Text.Substring( 4 , 2 );
                // 員工提撥日期
                strValue = SysSet.FormatDate(e.Row.Cells[4].Text);
                if (!strValue.Contains("/"))
                    strValue = "--";
                e.Row.Cells[4].Text = strValue;

                //員工提撥金額
                if ( P.DeCodeAmount ( e.Row.Cells[ 5 ].Text ).ToString ( "#,0" ).Length > 0 )
                {
                    e.Row.Cells[ 5 ].Text = P.DeCodeAmount ( e.Row.Cells[ 5 ].Text ).ToString ( "#,0" );
                }
                else
                {
                    int n;
                    if(!int.TryParse(e.Row.Cells[ 5 ].Text,out n))
                    {
                        e.Row.Cells[ 5 ].Text = "";
                    }
                    
                }

                //企業提繳金額
                if ( P.DeCodeAmount ( e.Row.Cells[ 6 ].Text ).ToString ( "#,0" ).Length > 0 )
                {
                    e.Row.Cells[ 6 ].Text = P.DeCodeAmount ( e.Row.Cells[ 6 ].Text ).ToString ( "#,0" );
                }
                else
                {
                    int n;
                    if ( !int.TryParse ( e.Row.Cells[ 6 ].Text , out n ) )
                    {
                        e.Row.Cells[ 6 ].Text = "";
                    }

                }

                // 到職日期
                strValue = SysSet.FormatDate(e.Row.Cells[7].Text);
                if (!strValue.Contains("/") || !strValue.StartsWith(e.Row.Cells[3].Text))
                    strValue = "--";
                e.Row.Cells[7].Text = strValue;

                // 離職日期
                strValue = SysSet.FormatDate(e.Row.Cells[8].Text);
                if (!strValue.Contains("/") || !strValue.StartsWith(e.Row.Cells[3].Text))
                    strValue = "--";
                e.Row.Cells[8].Text = strValue;
                #endregion
            }

            try
            {
                if (DataBinder.Eval(e.Row.DataItem, "ActualAmount_S") != null)
                    ActualAmount_S += Convert.ToDecimal(P.DeCodeAmount(DataBinder.Eval(e.Row.DataItem, "ActualAmount_S").ToString()));
                if (DataBinder.Eval(e.Row.DataItem, "ActualAmount_C") != null)
                    ActualAmount_C += Convert.ToDecimal(P.DeCodeAmount(DataBinder.Eval(e.Row.DataItem, "ActualAmount_C").ToString()));
            }
            catch { }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = "當前總計";
            e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
            e.Row.Cells[2].Font.Bold = true;
            e.Row.Cells[2].Font.Underline = true;
            e.Row.Cells[5].Text = ActualAmount_S.ToString("n0");
            e.Row.Cells[5].Font.Bold = true;
            e.Row.Cells[5].Font.Underline = true;
            e.Row.Cells[6].Text = ActualAmount_C.ToString("n0");
            e.Row.Cells[6].Font.Bold = true;
            e.Row.Cells[6].Font.Underline = true;
        }
        else if ( e.Row.RowType == DataControlRowType.EmptyDataRow )
        {
            

        }
    }

    private DropDownList SetDropDownList( int n , DropDownList DDL )
    {
        switch ( n )
        {

            case 2://年下拉式選單
                for ( int i = DateTime.Today.Year - 5 ; i < DateTime.Today.Year + 5 ; i++ )
                {
                    DDL.Items.Add( Li( Convert.ToString( i - 1911 ) , i.ToString( ) ) );
                }
                DDL.SelectedValue = DateTime.Now.Year.ToString( );
                break;

            case 3://月下拉式選單
                for ( int i = 1 ; i < 13 ; i++ )
                {
                    DDL.Items.Add( i.ToString( ).PadLeft( 2 , '0' ) );
                }
                DDL.SelectedValue = DateTime.Now.Month.ToString( );
                break;
        }
        return DDL;
    }

    protected void GridView1_RowCreated( object sender , GridViewRowEventArgs e )
    {

        if (e.Row.RowType == DataControlRowType.DataRow || (e.Row.RowType == DataControlRowType.Footer))
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }

        if ( e.Row.RowType == DataControlRowType.Header )
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[ 0 ].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[ e.Row.Cells.Count - 1 ].CssClass = "paginationRowEdgeRl";

        }
        else if ( ( e.Row.RowType == DataControlRowType.DataRow ) || ( e.Row.RowType == DataControlRowType.EmptyDataRow ) )
        {

            e.Row.Attributes.Add( "onmouseover" , "setnewcolor(this);" );
            e.Row.Attributes.Add( "onmouseout" , "setoldcolor(this);" );
            int i = 0;
            for ( i = 0 ; i < e.Row.Cells.Count ; i++ )
            {
                e.Row.Cells[ i ].CssClass = "Grid_GridLine";
            }

            i = 6;
            if (i <= e.Row.Cells.Count)
            {
                e.Row.Cells[ i - 1 ].Style.Add( "text-align" , "right" );
                e.Row.Cells[ i - 1 ].Style.Add( "width" , "70px" );
            }
            if (i <= e.Row.Cells.Count - 1)
                e.Row.Cells[i].Style.Add("text-align", "right");
        }
        else if ( e.Row.RowType == DataControlRowType.Pager )
        {
            e.Row.Visible = false;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[i].CssClass = "Grid_GridLine";
                if (i < 3)
                    e.Row.Cells[i].Style.Add("text-align", "left");
                else
                    e.Row.Cells[i].Style.Add("text-align", "right");
            }
            GridView1.ShowFooter = true;
        }
    }

    protected void GridView1_RowEditing( object sender , GridViewEditEventArgs e )
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindData( );
    }

    protected void GridView1_RowCancelingEdit( object sender , GridViewCancelEditEventArgs e )
    {
        GridView1.EditIndex = -1;
        BindData( );
    }

    protected void GridView1_RowCommand( object sender , GridViewCommandEventArgs e )
    {
        if ( !String.IsNullOrEmpty( e.CommandName ) )
        {
            try
            {
                //GridViewRow gvRow = null;
                switch ( e.CommandName )
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
            catch ( Exception ex )
            {
                lbl_Msg.Text = ex.Message;
            }
        }
    }

    protected void SDS_GridView_Selecting( object sender , SqlDataSourceSelectingEventArgs e )
    {

    }

    private void WriteLog( bool n , string c , int i )
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
            MyCmd.Parameters.Clear( );
            MyCmd.Parameters.Add( "@TableName" , SqlDbType.VarChar , 60 ).Value = "Pensionaccounts_Detail";
            MyCmd.Parameters.Add( "@TrxType" , SqlDbType.Char , 1 ).Value = c;
            MyCmd.Parameters.Add( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = "ALL";
            MyCmd.Parameters.Add( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = "";
            MyCmd.Parameters.Add( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog( MyCmd.Parameters );
            #endregion
        }
        else
        {
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters[ "@SQLcommand" ].Value = ( i == 1 ) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
            _MyDBM.DataChgLog( MyCmd.Parameters );
            #endregion
        }
    }

    //  控制頁面 Panel，查無資料時顯示用
    private void showPanel( )
    {
        Panel_Empty.Visible = hasData( ) ? false : true;
    }

    //  當按下搜尋時呼叫此區
    protected void Navigator1_Load( object sender , EventArgs e )
    {

    }

    protected void GridView1_SelectedIndexChanged( object sender , EventArgs e )
    {

    }

    protected void QueryWorking_Click(object sender, EventArgs e)
    {
        BindData(PayrollTable + "_Detail");
    }

    /// <summary>
    /// 設定是含離職核取方塊:2011/09/16 依人事要求加入離職篩選條件
    /// </summary>
    private void SetResignCode()
    {
        cbResignC.Items.Clear();
        Ssql = "select [CodeCode],[CodeName] from CodeDesc Where CodeID = 'PY#ResignC'";
        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        ListItem theItem = new ListItem();
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            theItem = new ListItem();
            theItem.Value = DT.Rows[i]["CodeCode"].ToString();
            theItem.Text = DT.Rows[i]["CodeName"].ToString();
            if (!DT.Rows[i]["CodeCode"].ToString().Equals("Y"))
                theItem.Selected = true;
            cbResignC.Items.Add(theItem);
        }
    }
}//  程式尾部
