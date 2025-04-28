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

public partial class db_Leave_Trx : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYI014";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string sCompany = "";

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
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

            CompanyList1.SelectValue = _UserInfo.UData.Company;
            SalaryYM1.SetOtherYMList();
            SalaryYM1.SelectSalaryYM = DateTime.Now.ToString("yyyyMM");
            Navigator1.Visible = false;
            SetLeaveType();
        }
        else
        {
            sCompany = CompanyList1.SelectValue.Trim().Replace("%", "");            
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'Payroll' And RTrim(ProgramPath)='Import/IncomePayrollMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    /// <summary>
    /// 設定假別核取方塊:2011/08/17 依人事要求加入假別篩選條件
    /// </summary>
    private void SetLeaveType()
    {
        cbLeaveType.Items.Clear();
        Ssql = "SELECT [Company],[Leave_Id],[Leave_Desc],(Case [SalaryType] when 'Y' then '' else '(扣薪假)' end) [LeaveType],[SalaryType] " +
            " FROM [LeaveType_Basic] where [Company]='" + CompanyList1.SelectValue + "' Order by SalaryType";
        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        ListItem theItem = new ListItem();
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            theItem = new ListItem();
            theItem.Value = DT.Rows[i]["Leave_Id"].ToString();
            theItem.Text = DT.Rows[i]["Leave_Desc"].ToString();
            if (DT.Rows[i]["SalaryType"].ToString().Equals("N"))
                theItem.Selected = true;
            cbLeaveType.Items.Add(theItem);
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;

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
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                        e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;


                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                //link = (LinkButton)e.Row.FindControl("btnSelect");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Kind=Query&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");

                //link = (LinkButton)e.Row.FindControl("btnEdit");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                break;

            case DataControlRowType.Header:

                link = (LinkButton)e.Row.FindControl("btnNew");
                if (link != null)
                {
                    //指定位置用top=100px,left=100px,
                    link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_A.aspx','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
                }
                break;
        }
    }

    private void BindData()
    {
        DataTable theDT = null;
        GridView1.Visible = false;
        Navigator1.Visible = false;
        
        try
        {
            Ssql = "SELECT LT.*,PM.EmployeeName,(Case When IsNull(LTB.Leave_Desc,'')='' then LT.LeaveType_Id+'-未設定' else LTB.Leave_Desc End) As [Leave_Desc] " +
                " FROM [Leave_Trx_Temp] as LT " +
                " left join  Personnel_Master  as PM on LT.Company=PM.Company and LT.EmployeeId=PM.EmployeeId " +
                " left join  LeaveType_Basic as LTB on LT.Company=LTB.Company and LT.LeaveType_Id=LTB.Leave_Id " +
                " Where ([days]+[hours])>0 And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM;

            if (sCompany.Length > 0)
                Ssql += " And LT.company='" + sCompany + "'";

            if (cbLeaveType.Items.Count > 0)
            {//2011/08/17 依人事要求加入假別篩選條件
                string strTL = "";
                foreach (ListItem lis in cbLeaveType.Items)
                {
                    if (lis.Selected)
                    {
                        strTL += "'" + lis.Value + "',";
                    }
                }
                if (strTL.Length > 0)
                {
                    Ssql += " And LT.LeaveType_Id in (" + strTL + "'-')";
                }
            }

            //Ssql += " order by " + rbOrder.SelectedValue;
            Ssql += " order by " + rbOrder.SelectedValue + ",convert(char,BeginDateTime,121),PM.EmployeeId,(case when '" + rbOrder.SelectedValue + "'='LeaveType_Id' then IsNull(LTB.Leave_Desc,'') else LeaveType_Id end)";

            theDT = _MyDBM.ExecuteDataTable(Ssql);

            if (theDT.Rows.Count == 0)
                lbl_Msg.Text = "查無資料!!";
            else
                lbl_Msg.Text = "";
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = "資料有誤!!" + ex.Message;
        }

        GridView1.DataSource = theDT;
        GridView1.DataBind();

        if (theDT.Rows.Count > 0)
        {
            GridView1.Visible = true;
            if (GridView1.PageCount > 0)
                Navigator1.Visible = true;
        }

        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            lbl_Msg.Text = "請選擇先公司!";
        }
        else
        {
            BindData();
        }
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

    void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {
        SetLeaveType();
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";
                
        string strSQL = "";
        strSQL = "Select Count(*) from PayrollControl Where Not([ConfirmDate] is Null) And " + Ssql + " And SalaryYM=" + SalaryYM1.SelectSalaryYM;
        //查詢是否已確認薪資
        DataTable dt = _MyDBM.ExecuteDataTable(strSQL);
        if (dt != null && ((int)dt.Rows[0][0]) == 0)
        {
            SetTrx();
        }
        else
        {
            lbl_Msg.Text = SalaryYM1.SelectSalaryYMName + "已完成薪資確認作業! 不可再匯入該月資料";
        }
    }

    private void SetTrx()
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        #region 開始匯入前,先寫入LOG
        int result = 0;
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Leave_Trx";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = Ssql;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        try
        {
            string strSQL = "";           
            //查詢可轉換資料筆數            
            DataTable dt = _MyDBM.ExecuteDataTable("Select Count(*) FROM Leave_Trx_Temp WHERE " + Ssql + " And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM);
            if (dt != null && ((int)dt.Rows[0][0]) > 0)
            {
                //刪除已存在目地資料
                _MyDBM.ExecuteCommand("DELETE FROM Leave_Trx WHERE " + Ssql + " And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM);

                strSQL = "INSERT INTO [Leave_Trx] " +
                    " ([Company],[EmployeeId],[LeaveType_Id],[BeginDateTime],[EndDateTime],[DeptId],[hours],[days],[Payroll_Processingmonth],[ApproveDate])  " +
                    " Select [Company],[EmployeeId],[LeaveType_Id],[BeginDateTime],[EndDateTime],[DeptId],[hours],[days],[Payroll_Processingmonth],[ApproveDate] From [Leave_Trx_Temp]  " +
                    " Where ([days]+[hours])>0 And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM + " And " + Ssql;
                //新增目的資料
                _MyDBM.ExecuteCommand(strSQL);

                //刪除來源資料
                result = _MyDBM.ExecuteCommand("DELETE FROM Leave_Trx_Temp WHERE " + Ssql + " And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM);
            }
            else
            {
                result = 0;
                lbl_Msg.Text = "無轉換資料";
            }
        }
        catch (Exception ex)
        {
            lbl_Msg.Text = ex.Message;
        }
        #region 完成異動後,更新LOG資訊
        MyCmd.Parameters["@SQLcommand"].Value = (result > 0) ? "Success" : lbl_Msg.Text;
        MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion

        BindData();

        if (result > 0)
            lbl_Msg.Text = "轉換完成";
        else
            lbl_Msg.Text = "無轉換資料";

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        //刪除
        int del = _MyDBM.ExecuteCommand("DELETE FROM Leave_Trx_Temp WHERE " + Ssql + " And Payroll_Processingmonth=" + SalaryYM1.SelectSalaryYM);

        BindData();

        if (del != 0)
            lbl_Msg.Text = "刪除完成";
        else
            lbl_Msg.Text = "無刪除資料";
    }
}
