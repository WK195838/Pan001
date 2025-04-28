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
using FormsAuth;

public partial class PersonnelAdjustment : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    
    string _ProgramId = "PYM006";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

    string[] FileDataKey ={ "Company","EmployeeId","AdjustmentCategory","EffectiveDate" };

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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PersonnelAdjustment.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Detail", "Add" };

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
                    btnNew.Visible = Find;
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
        
        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged ( SearchList1_SelectedChanged );

        SetSqlCommand();

        Navigator1.BindGridView = GridView1;

        if (!Page.IsPostBack)
        {
    
            #region GridView @ 顯示資料設定 @
            string[] DataField = { "EmployeeId", "AdjustmentCategory", "EffectiveDate", "Company" };
            string[] HeaderText = { "員　　工", "調動類別", "生效日期", "公　　司" };

            for (int i = 0; i < DataField.Length; i++)
            {
                BoundField Data = new BoundField();

                Data.DataField = DataField[i];
                Data.HeaderText = HeaderText[i];
                Data.SortExpression = DataField[i];
                GridView1.Columns.Add(Data);
            }

            #endregion

            
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            BindData();
            showPanel();
            AuthRight();
            SetSqlParameters();
        }

        BindData();
        showPanel();
        btnNew.Attributes.Add ( "onclick" , "javascript:var win =window.open('PersonnelAdjustment_A.aspx?C=" + SearchList1.CompanyValue + "&E=" + SearchList1.EmployeeValue + "','','width=600px,height=600px,top=100px,left=100px,scrollbars=yes,resizable=yes');" );
  
        
    }

    //SetSQL
    protected void SetSqlCommand()
    {
        SDS_GridView.SelectCommand = "SELECT PersonnelAdjustment.* FROM PersonnelAdjustment";
        SDS_GridView.DeleteCommand = "DELETE FROM PersonnelAdjustment WHERE Company = @Company AND EmployeeId=@EmployeeId AND AdjustmentCategory=@AdjustmentCategory AND EffectiveDate=@EffectiveDate";
    }
    protected void SetSqlParameters()
    {
        for ( int i = 0; i < FileDataKey.Length; i++ )
        {
            Parameter Pa = new Parameter();
            Pa.Name = FileDataKey[i];
            SDS_GridView.DeleteParameters.Add(Pa);
        }

    }
    private string GetGVValue ( int n , string str )
    {
        return GridView1.DataKeys[ n ].Values[ str ].ToString ( ).Trim ( );
    }

    void SearchList1_SelectedChanged ( object sender , SearchList.SelectEventArgs e )
    {
        BindData ( );
        showPanel ( );
    }

    private void SetSsql ()
    {
        Ssql = "SELECT * FROM PersonnelAdjustment  Where 0=0 ";
        //公司
        DDLSr ( "Company" , SearchList1.CompanyValue );
        
        //部門
        if ( SearchList1.DepartmentValue.Length > 0 )
            Ssql += string.Format ( " And ( DepCode_F like '%{0}%' or  DepCode_T like '%{0}%' )" , SearchList1.DepartmentValue );
        else
            Ssql += string.Format ( " And DepCode_F = '{0}' or DepCode_T = '{0}' " , SearchList1.DepartmentValue );
        
        //員工
        DDLSr ( "EmployeeId" , SearchList1.EmployeeValue );


        Ssql += " Order By EmployeeId ";
    }

    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr ( string Name , string Value )
    {
        if ( Value.Length > 0 )
            Ssql += string.Format ( " And " + Name + " like '%{0}%'" , Value );
        else
            Ssql += string.Format ( " And " + Name + " = '{0}'" , Value );
    }


    private void BindData()
    {
        SetSsql ();
        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    protected void btnQuery_Click ( object sender , EventArgs e )
    {
        BindData ( );
        showPanel ( );
    }

    private void showPanel ( )
    {
        Panel_Empty.Visible = hasData ( ) ? false : true;
    }
    private bool hasData ( )
    {
        SetSsql ( );
        DataTable tb = _MyDBM.ExecuteDataTable ( Ssql );
        return tb.Rows.Count > 0 ? true : false;
    }

    //GridView1
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[6].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }
            e.Row.Cells[6].Visible = false;
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    //DataBound
    protected void GridView1_RowDataBound ( object sender , GridViewRowEventArgs e )
    {
        LinkButton link;


        switch ( e.Row.RowType )
        {
            case DataControlRowType.DataRow:


                if ( ( e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit )
                || ( e.Row.RowState == ( System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit ) ) )
                {
                    //修改時
                }
                else
                {
                    //查詢時

                    #region 修改顯示方式
                    try
                    {
                        e.Row.Cells[ 3 ].Text = SearchList1.Employee.Items.FindByValue ( e.Row.Cells[ 3 ].Text.Trim ( ) ).Text;
                    }
                    catch { }
                    DataTable DT = _MyDBM.ExecuteDataTable ( "SELECT CodeName FROM CodeDesc Where CodeCode = '" + e.Row.Cells[ 4 ].Text.Trim ( ) + "' and CodeID = 'PY#Adjustm'" );
                    if ( DT.Rows.Count > 0 ) e.Row.Cells[ 4 ].Text = DT.Rows[ 0 ][ "CodeName" ].ToString ( );
                    e.Row.Cells[5].Text = _UserInfo.SysSet.FormatDate(Convert.ToDateTime(e.Row.Cells[5].Text).ToString("yyyy/MM/dd"));
                    e.Row.Cells[ 5 ].Style.Add ( "text-align" , "right" );
                    #endregion

                }

                #region  @  偷懶參數設定區  @
                int n = e.Row.RowIndex;
                string tmpstr = "Company=" + GetGVValue ( n , "Company" ) +
                                "&EmployeeId=" + GetGVValue ( n , "EmployeeId" ) +
                                "&AdjustmentCategory=" + GetGVValue ( n , "AdjustmentCategory" ) +
                                "&EffectiveDate=" + DateTime.Parse ( GetGVValue ( n , "EffectiveDate" ) ).ToString ( "yyyy-MM-dd HH:mm:ss" ).Trim ( ) +
                                "','','height=800px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');";
                #endregion

                link = ( LinkButton ) e.Row.FindControl ( "btnSelect" );
                link.Attributes.Add ( "onclick" , "javascript:var win =window.open('PersonnelAdjustment_I.aspx?" + tmpstr );
                link = ( LinkButton ) e.Row.FindControl ( "btnEdit" );
                link.Attributes.Add ( "onclick" , "javascript:var win =window.open('PersonnelAdjustment_U.aspx?" + tmpstr );
                break;

            case DataControlRowType.Header:

                link = ( LinkButton ) e.Row.FindControl ( "btnNew" );
                if ( link != null )
                {
                    link.Attributes.Add ( "onclick" , "javascript:var win =window.open('PersonnelAdjustment_A.aspx','','width=600px,height=400px,top=100px,left=100px,scrollbars=yes,resizable=yes');" );
                }
                break;
        }
    }

    //DELETE
    protected void btnDelete_Click ( object sender , EventArgs e )
    {
        LinkButton btnDelete = ( LinkButton ) sender;
        string L1PK = btnDelete.Attributes[ "L1PK" ].ToString ( );
        string L2PK = btnDelete.Attributes[ "L2PK" ].ToString ( );
        string L3PK = btnDelete.Attributes[ "L3PK" ].ToString ( );
        string L4PK = btnDelete.Attributes[ "L4PK" ].ToString ( );

        string sql = "Delete From PersonnelAdjustment Where Company='" + L1PK +
            "' AND EmployeeId='" + L2PK +
            "' AND AdjustmentCategory='" + L3PK +
            "' AND (convert(varchar,EffectiveDate,120) = '" + Convert.ToDateTime ( L4PK ).ToString ( "yyyy-MM-dd HH:mm:ss" ) + "')";

        int result = _MyDBM.ExecuteCommand ( sql.ToString ( ) );

        if ( result > 0 )
        {
            lbl_Msg.Text = "資料刪除成功 !!";

            Navigator1.DataBind ( );
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";
        }
        BindData ( );
        showPanel ( );
    }
    protected void GridView1_RowDeleting ( object sender , GridViewDeleteEventArgs e )
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
        MyCmd.Parameters.Add ( "@TableName" , SqlDbType.VarChar , 60 ).Value = "PersonnelAdjustment";
        MyCmd.Parameters.Add ( "@TrxType" , SqlDbType.Char , 1 ).Value = "D";
        MyCmd.Parameters.Add ( "@ChangItem" , SqlDbType.VarChar , 255 ).Value = "ALL";
        MyCmd.Parameters.Add ( "@SQLcommand" , SqlDbType.VarChar , 2000 ).Value = "";
        MyCmd.Parameters.Add ( "@ChgStartDateTime" , SqlDbType.SmallDateTime ).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add ( "@ChgUser" , SqlDbType.VarChar , 32 ).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog ( MyCmd.Parameters );
        #endregion
    }
    protected void GridView1_RowDeleted ( object sender , GridViewDeletedEventArgs e )
    {
        if ( e.Exception == null )
        {
            lbl_Msg.Text = e.AffectedRows.ToString ( ) + " 個資料列 " + "刪除成功!!";
            BindData ( );
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters[ "@SQLcommand" ].Value = ( e.Exception == null ) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add ( "@ChgStopDateTime" , SqlDbType.SmallDateTime ).Value = DateTime.Now;
        _MyDBM.DataChgLog ( MyCmd.Parameters );
        #endregion

        if ( e.Exception != null )
            return;

        showPanel ( );
    }


}
