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

public partial class LaborDataDetail : System.Web.UI.Page
{

    #region 共用參數

    string Ssql = "";
    UserInfo _UserInfo = new UserInfo( );
    string _ProgramId = "PYQ002";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand( );

    #endregion

    #region 資料設定


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

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
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

        }
    }

    //  載入網頁時執行此區
    protected void Page_Load( object sender , EventArgs e )
    {
        ScriptManager.RegisterStartupScript ( UpdatePanel1 , this.GetType ( ) , "" , @"JQ();" , true );
        lbl_Msg.Text = "";//清空訊息
        Navigator1.BindGridView = GridView1;

        CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged( CompanyList1_SelectedIndex );
        DepList1.SelectedIndexChanged += new EventHandler(DepList1_SelectedIndexChanged);
        IDNo1.TextChanged += new EventHandler(IDNo1_TextChanged);
        TrxType.SelectedIndexChanged += new EventHandler(TrxType_SelectedIndexChanged);


        if (!Page.IsPostBack)
        {
            TrxType.RepeatDirection = RepeatDirection.Horizontal;
            for (int i = 0 ; i < 4 ; i++)
            {
                TrxType.SelectedIndex = 0;
                TrxType.Items.Add(Li(i == 1 ? " 投保 " : i == 2 ? " 調整 " : i == 3 ? " 退保 " : " 全部 " , i == 1 ? "A1" : i == 2 ? "A2" : i == 3 ? "A3" : ""));
            }

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
            AuthRight();
        }
        else if (Request.Form["__EVENTTARGET"].Contains("EmployeeIdList"))
        {
            BindData();
            showPanel();
        }
    }

    void TrxType_SelectedIndexChanged(object sender , EventArgs e)
    {
        BindData();
        showPanel();
    }

    void IDNo1_TextChanged(object sender , EventArgs e)
    {
        BindData();
        showPanel();
    }

    //  公司下拉式選單
    void CompanyList1_SelectedIndex( object sender , UserControl_CompanyList.SelectEventArgs e )
    {
        #region 部門選單
        DepList1.Items.Clear();
        DepList1.Items.Insert(0 , Li("全部" , ""));
        for (int i = 0 ; i < e.Department_Basic.Count ; i++)
        {
            DepList1.Items.Add(e.Department_Basic[i]);
        }
        DepList1.DataBind();
        #endregion

        #region 員工選單
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert( 0 , Li( "全部" , "" ) );
        for ( int i = 0 ; i < e.Personnel_Master.Count ; i++ )
        {
            EmployeeIdList1.Items.Add(e.Personnel_Master[i]);
        }
        EmployeeIdList1.DataBind( );
        #endregion

        BindData();
        showPanel();
    }
    // 部門下拉式選單
    void DepList1_SelectedIndexChanged(object sender , EventArgs e)
    {
        #region 員工選單
        DataTable DT = _MyDBM.ExecuteDataTable("select DeptId,EmployeeId,EmployeeName from Personnel_Master where Company = '" + CompanyList1.SelectValue + "' ");
        EmployeeIdList1.Items.Clear();
        EmployeeIdList1.Items.Insert(0 , Li("全部" , ""));
        for (int i = 0 ; i < DT.Rows.Count;i++)
        {
                                                                                                                                                                                                                             string[] Employee = { DT.Rows[i]["DeptId"].ToString().Trim() , DT.Rows[i]["EmployeeId"].ToString().Trim() , DT.Rows[i]["EmployeeName"].ToString() };
            if (((DropDownList)sender).SelectedItem.Text != "全部")
            {

                if (DT.Rows.Count > 0 && ((DropDownList)sender).SelectedItem.Value.Trim() == Employee[0].Trim())
                {
                    EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2] , Employee[1]));
                }
            }
            else
                EmployeeIdList1.Items.Add(Li(Employee[1] + Employee[2] , Employee[1]));
        }
        EmployeeIdList1.DataBind();
        BindData();
        showPanel();
        #endregion
    }



    private ListItem Li( string text , string value )
    {
        ListItem li = new ListItem( );
        li.Text = text;
        li.Value = value.Trim();
        return li;
    }

    protected void btnQuery_Click( object sender , EventArgs e )
    {
        lbl_Msg.Text = "";
        BindData( );
        showPanel( );
    }

    //設定搜尋條件
    private void SetSsql( )
    {
        Ssql = "SELECT A.Company,A.EmployeeId,A.EmployeeName,A.DeptId,A.IDNo,B.TrxType,B.EffectiveDate,B.LI_amount FROM Personnel_Master AS A,LaborInsurance AS B Where 0=0 ";

        if ( CompanyList1.SelectValue.Length > 0 )
        {
            Ssql += string.Format(" And A.Company like '%{0}%'" , CompanyList1.SelectValue.Trim());
        }
        else
        {
            Ssql += string.Format(" And A.Company = '%{0}%'" , CompanyList1.SelectValue.Trim());
        }

        if (DepList1.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And A.DeptId like '%{0}%'" , DepList1.SelectedValue.Trim());
        }

        if ( EmployeeIdList1.SelectedValue.Length > 0 )
        {
            Ssql += string.Format(" And A.EmployeeId like '%{0}%'" , EmployeeIdList1.SelectedValue.Trim());
        }

        if (IDNo1.Text.Length > 0)
        {
            Ssql += string.Format(" And A.IDNo like '%{0}%'", _UserInfo.SysSet.CleanSQL(IDNo1.Text.Trim()).ToString());
        }

        if (TrxType.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And B.TrxType like '%{0}%'" , TrxType.SelectedValue.Trim());
        }

        if (EffectiveDateSt.Text.Length > 0)
        {
            Ssql += string.Format(" And Convert(int,Convert(char,EffectiveDate,112)) > '{0}'" , _UserInfo.SysSet.FormatADDate(EffectiveDateSt.Text.Trim()).Replace("/" , ""));
        }
        if (EffectiveDateEd.Text.Length > 0)
        {
            Ssql += string.Format(" And Convert(int,Convert(char,EffectiveDate,112)) < '{0}'" , _UserInfo.SysSet.FormatADDate(EffectiveDateEd.Text.Trim()).Replace("/" , ""));
        }

        


        Ssql += " AND B.Company = A.Company AND B.EmployeeId = A.EmployeeId ";

    }

    //  判斷搜尋時是否有此資料
    private bool hasData( )
    {
        SetSsql( );
        MyCmd.Parameters.Clear( );
        DataTable tb = _MyDBM.ExecuteDataTable( Ssql , MyCmd.Parameters , CommandType.Text );
        return tb.Rows.Count > 0 ? true : false;
    }

    //  綁定資料至Gridview
    private void BindData( )
    {
        SetSsql( );
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind( );
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind( );
    }

    protected void GridView1_RowDataBound( object sender , GridViewRowEventArgs e )
    {
        if ( e.Row.RowType == DataControlRowType.DataRow )
        {
            //員工編號
            if (EmployeeIdList1.Items.Count > 1) e.Row.Cells[0].Text = EmployeeIdList1.Items.FindByValue(e.Row.Cells[0].Text.Trim()).Text;
            //部門
            try
            {
                if (DepList1.Items.Count > 1) e.Row.Cells[1].Text = DepList1.Items.FindByValue(e.Row.Cells[1].Text.Trim()).Text;
            }
            catch { }
            
            //異動類別
            e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace("A1" , "投保").Replace("A2" , "調整").Replace("A3" , "退保");
            //生效日期
            e.Row.Cells[4].Text = _UserInfo.SysSet.FormatDate(e.Row.Cells[4].Text);

            try
            {
                //金額
                e.Row.Cells[5].Text = Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(e.Row.Cells[5].Text)).ToString("n0");
            }
            catch {
                e.Row.Cells[5].Text = "--";
            }
        }
    }

    private DropDownList SetDropDownList( int n , DropDownList DDL )
    {
        switch ( n )
        {
            case 1:
                DDL.DataSource = EmployeeIdList1.Items;
                DDL.DataBind( );
                DDL.SelectedIndex = EmployeeIdList1.SelectedIndex;
                DDL.Items.RemoveAt( 0 );
                DDL.Width = 120;
                break;
        }
        return DDL;
    }

    protected void GridView1_RowCreated(object sender , GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            e.Row.Visible = false;
        }
    }

    
    protected void GridView1_RowCommand(object sender , GridViewCommandEventArgs e)
    {
        if (!String.IsNullOrEmpty(e.CommandName))
        {
            try
            {lbl_Msg.Text = "";}
            catch (Exception ex)
            {lbl_Msg.Text = ex.Message;}
        }
    }

    //控制頁面 Panel，查無資料時顯示用
    private void showPanel()    {Panel_Empty.Visible = hasData() ? false : true;}

}//  程式尾部
