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

public partial class Pensionaccounts : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM009";
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/Pensionaccounts.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Delete" , "Modify" , "Detail" , "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0 ; i < Auth.Length ; i++)
            {
                Find = _UserInfo.CheckPermission(_ProgramId , Auth[i]);
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

    protected void Page_Load(object sender , EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息

        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged ( SearchList1_SelectedChanged );

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
            //設定公司
            if ( _UserInfo.UData.Company.Length > 0 )
                SearchList1.CompanyValue = _UserInfo.UData.Company;
            else
                SearchList1.CompanyValue = SearchList1.Company.Items.Count > 1 ? SearchList1.Company.Items[ 1 ].Value : SearchList1.Company.Items[ 0 ].Value;
            BindData();
            showPanel();
            AuthRight();
        }

        BindData();
        showPanel();
        btnNew.Attributes.Add(
            "onclick" , "javascript:var win =window.open('" +
            "Pensionaccounts_A.aspx?" +
            "Company=" + SearchList1.CompanyValue +
            "','','width=600px,height=450px,top=150px,left=150px,scrollbars=yes,resizable=yes');");
    }

    void SearchList1_SelectedChanged ( object sender , SearchList.SelectEventArgs e )
    {
        BindData ( );
        showPanel ( );
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


    #region 資料搜尋


    //  搜尋是否有符合的資料
    private void SetSsql ( )
    {
        Ssql = "SELECT OTT.*,PM.deptid AS Depid FROM Pensionaccounts_Master OTT left join Personnel_Master PM On OTT.EmployeeId = PM.EmployeeId And OTT.Company = PM.Company Where 0=0 ";
        //公司
        DDLSr ( "OTT.Company" , SearchList1.CompanyValue );

        //部門
        DDLSr ( "PM.deptid " , SearchList1.DepartmentValue );

        //員工
        DDLSr ( "OTT.EmployeeId" , SearchList1.EmployeeValue );

  
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
    private bool hasData()
    {
        SetSsql ( );
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        return tb.Rows.Count > 0 ? true : false;
    }
    //  綁定資料至Gridview
    private void BindData()
    {
        SetSsql ( );
        SDS_GridView.SelectCommand = Ssql + " Order By Company";
        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    #endregion



    protected void GridView1_RowDataBound(object sender , GridViewRowEventArgs e)
    {
        LinkButton link;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時
                }
                else
                {
                    //查詢時

                    try
                    {
                        e.Row.Cells[ 2 ].Text = this.SearchList1.Company.Items.FindByValue ( e.Row.Cells[ 2 ].Text.Trim ( ) ).Text;
                        e.Row.Cells[ 2 ].Width = 300;
                        e.Row.Cells[ 3 ].Text = this.SearchList1.Employee.Items.FindByValue ( e.Row.Cells[ 3 ].Text.Trim ( ) ).Text;
                    }
                    catch
                    { }
                }

                link = (LinkButton)e.Row.FindControl("btnSelect");
                link.Attributes.Add("onclick" , "javascript:var win =window.open('Pensionaccounts_I.aspx?Company=" +
                    GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() +
                    "&EmployeeId=" +
                     GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','height=400px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');");
                link = (LinkButton)e.Row.FindControl("btnEdit");
                link.Attributes.Add("onclick" , "javascript:var win =window.open('Pensionaccounts_U.aspx?Company=" +
                    GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() +
                    "&EmployeeId=" +
                     GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','height=480px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');");
                break;

            case DataControlRowType.Header:

                link = (LinkButton)e.Row.FindControl("btnNew");
                if (link != null)
                {
                    link.Attributes.Add("onclick" , "javascript:var win =window.open('Pensionaccounts_A.aspx?Company=" +
                    GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "','','width=800px,height=400px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
                }
                break;
        }
    }

    protected void GridView1_RowDeleting(object sender , GridViewDeleteEventArgs e)
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
        MyCmd.Parameters.Add("@TableName" , SqlDbType.VarChar , 60).Value = "SalaryStructure_Parameter";
        MyCmd.Parameters.Add("@TrxType" , SqlDbType.Char , 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem" , SqlDbType.VarChar , 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand" , SqlDbType.VarChar , 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime" , SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser" , SqlDbType.VarChar , 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
    }

    protected void GridView1_RowDeleted(object sender , GridViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            lbl_Msg.Text = e.AffectedRows.ToString() + " 個資料列 " + "刪除成功!!";
            BindData();
        }
        else
        {
            lbl_Msg.Text = "刪除失敗!!  原因: " + e.Exception.Message;
            e.ExceptionHandled = true;
        }

        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (e.Exception == null) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime" , SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
            return;

        showPanel();
    }

    protected void GridView1_RowCreated(object sender , GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover" , "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout" , "setoldcolor(this);");
            for (int i = 0 ; i < e.Row.Cells.Count ; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align" , "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            e.Row.Visible = false;
        }
    }
}
