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
using System.Data.SqlClient;

public partial class GLA0460 : System.Web.UI.Page

{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLA0460";
    DBManger _MyDBM;
    SqlCommand MyCmd = new SqlCommand();
    String sCompany;

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
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
        int i = 0;

        string[] Auth = { "Delete", "Modify", "Detail", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(_ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    GridView1.Columns[i].Visible = Find;
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
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息

        //下拉式選單連動設定
    
        Navigator1.BindGridView = GridView1;     
    
      
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
           
        
        if (!Page.IsPostBack)
        {
            //if (_UserInfo.AuthLogin == false)
            //{
            //    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            //}
            //else
            //{
            //    if (_UserInfo.CheckPermission(_ProgramId) == false)
            //        Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            //}
            //設定公司
             CompanyList1.SelectValue = _UserInfo.UData.Company;
             if (sCompany != null)
             {
                 CompanyList1.SelectValue = sCompany;
             }
             else
             {
                 CompanyList1.SelectValue = "20";
             }
            BindData();
            //showPanel();
            //AuthRight();
        }
        else
        {
            BindData();
        }
       
            btnNew.Visible = true;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('GLA0460_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
           
    }
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();
        showPanel();
    }
    private void showPanel()
    {     

            if (CompanyList1.SelectValue == "")
            {
                Panel_Empty.Visible = false;

            }
            else
            {
                Panel_Empty.Visible = true;
            }
        
    }

   

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;
        //Label lbl;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                //lbl = (Label)e.Row.FindControl("lbl_ProgramType");

                //switch (lbl.Text)
                //{
                //    case "P":
                //        lbl.Text = "程式";
                //        break;

                //    case "M":
                //        lbl.Text = "選單";
                //        break;
                //}

                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時
                }
                else
                {
                    //查詢時
                    //for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                    //    e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;
                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                    if (DBSetting.PersonalName(e.Row.Cells[3].Text, e.Row.Cells[4].Text.Trim()) != null)
                    {
                        e.Row.Cells[4].Text = e.Row.Cells[4].Text.Trim() + " - " + DBSetting.PersonalName(e.Row.Cells[3].Text, e.Row.Cells[4].Text.Trim()).ToString();
                    }
                    e.Row.Cells[3].Text = e.Row.Cells[3].Text + " - " + DBSetting.CompanyName(e.Row.Cells[3].Text).ToString();
                }
                //lbl_Msg.Text = int.Parse(DateTime.Parse(GridView1.DataKeys[e.Row.RowIndex].Values["EffectiveDate"].ToString().Trim()).ToString("yyyy").Trim()).ToString()+ "/" ; 
                //lbl_Msg.Text = DateTime.Parse(GridView1.DataKeys[e.Row.RowIndex].Values["EffectiveDate"].ToString().Trim()).ToString("MM/dd").Trim();
                //DateTime.Parse(GridView1.DataKeys[e.Row.RowIndex].Values["EffectiveDate"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss").Trim();
                // "&EffectiveDate=" + DateTime.Parse(GridView1.DataKeys[e.Row.RowIndex].Values["EffectiveDate"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm:ss").Trim() +
                link = (LinkButton)e.Row.FindControl("btnSelect");
                link.Attributes.Add("onclick", "javascript:var win =window.open('GLA0460_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim()+ " &Kind=Query','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");// 

                link = (LinkButton)e.Row.FindControl("btnEdit");
                link.Attributes.Add("onclick", "javascript:var win =window.open('GLA0460_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");// 
                break;

            case DataControlRowType.Header:

                //if (CompanyList1.SelectValue != "")
                //{
                //((LinkButton)e.Row.FindControl("btnNew")).Visible = false;
                link = (LinkButton)e.Row.FindControl("btnNew");

                if (link != null)
                {
                    //指定位置用top=100px,left=100px,
                    link.Attributes.Add("onclick", "javascript:var win =window.open('GLA0460_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
                }
                //}
                    break;
                
        }
    }

    private void BindData()
    {
        SqlCommand sqlcmd = new SqlCommand();



        Ssql = "SELECT * FROM  GLParm WHERE 1=1 ";
        sqlDatasource.SelectParameters.Clear();

        //公司
        if (CompanyList1.SelectValue != "")
        {
            Ssql += " AND Company=@company  ";
            //sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = CompanyList1.SelectValue;
            sqlDatasource.SelectParameters.Add("company", DbType.StringFixedLength, CompanyList1.SelectValue);
           
        }

        //累積盈虧科目
        if (txtAccuPLAcctNo.Text.Trim() != "")
        {
            Ssql += " AND AccuPLAcctNo Like rtrim(ltrim(@AccuPLAcctNo))+'%' ";
            //sqlcmd.Parameters.Add("@acctNo", SqlDbType.Char).Value = txtAcctNo.Text.Trim();          
            sqlDatasource.SelectParameters.Add("AccuPLAcctNo", DbType.StringFixedLength, txtAccuPLAcctNo.Text.Trim());
        
        }

        //本期損益科目
        if (txtPeriodPLAcctNo.Text.Trim() != "")
        {
            Ssql += " AND PeriodPLAcctNo Like rtrim(ltrim(@PeriodPLAcctNo))+'%' ";
            //sqlcmd.Parameters.Add("@acctNo", SqlDbType.Char).Value = txtAcctNo.Text.Trim();          
            sqlDatasource.SelectParameters.Add("PeriodPLAcctNo", DbType.StringFixedLength, txtPeriodPLAcctNo.Text.Trim());

        }

       

       

      

        sqlDatasource.SelectCommand = Ssql;
        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }
  
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindData();
        //showPanel();
        if (CompanyList1.SelectValue == "")
        {
            lbl_Msg.Text = "請選擇公司編號";
        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "GLParm";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "D";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "";
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
    }

    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
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
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        if (e.Exception != null)
            return;

        //showPanel();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        //string L2PK = btnDelete.Attributes["L2PK"].ToString();

        string sql = "Delete From GLParm Where Company='" + L1PK + "'";// And DepositBank='" + L3PK + "' And DepositBankAccount='" + L4PK + "'

        int result = _MyDBM.ExecuteCommand(sql.ToString());

        if (result > 0)
        {
            lbl_Msg.Text = "資料刪除成功 !!";
            BindData();
            Navigator1.DataBind();
        }
        else
        {
            lbl_Msg.Text = "資料刪除失敗 !!";
        }
        BindData();
        //showPanel();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
}
