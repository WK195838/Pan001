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

public partial class Basic_LaborInsurance : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM051";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany, sEmployeeId;
    Payroll py = new Payroll();

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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
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
                    btnEmptyNew.Visible = Find;
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
        // 需要執行等待畫面的按鈕
        btReSetAll.Attributes.Add("onClick", "drawWait('')");
        ScriptManager1.RegisterPostBackControl(btReSetAll);

        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
        Navigator1.BindGridView = GridView1;
        try
        {
            sCompany = GridView1.DataKeys[0].ToString().Trim();
            //sCompany = DetailsView1.DataKey[0].ToString().Trim();
            sEmployeeId = GridView1.DataKeys[1].ToString().Trim();
            //sEmployeeId = DetailsView1.DataKey[1].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
            if (Request["EmployeeId"] != null)
                sEmployeeId = Request["EmployeeId"].Trim();
        }
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
            SearchList1.CompanyValue = _UserInfo.UData.Company;
            if (sCompany != null)
            {
                SearchList1.CompanyValue = sCompany;
            }
            BindData();
            showPanel();
            AuthRight();
        }
        if (SearchList1.CompanyValue == "")
        {
            btnNew.Visible = false;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_A.aspx','','width=600px,height=600px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_A.aspx','','width=600px,height=600px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
        }
        else
        {
            btnNew.Visible = true;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_A.aspx?Company=" + SearchList1.CompanyValue + "','','width=600px,height=600px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_A.aspx?Company=" + SearchList1.CompanyValue + "','','width=600px,height=600px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
        }
    }

    private void showPanel()
    {
        if (hasData())
        {
            Panel_Empty.Visible = false;
        }
        else
        {
            if (SearchList1.CompanyValue == "")
            {
                Panel_Empty.Visible = false;

            }
            else
            {
                Panel_Empty.Visible = true;
            }
        }
    }
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();
        showPanel();
    }
//Company	公司編號
//EmployeeId	員工編號
//TrxType	異動類別
//EffectiveDate	生效日期
//LI_amount	勞保投保金額
    private bool hasData()
    {
        Ssql = "SELECT LI.* FROM LaborInsurance LI left join Personnel_Master PM on LI.Company=PM.Company and LI.EmployeeId=PM.EmployeeId Where 0=0";
        //if (CompanyList1.SelectValue == "")
        //{
        //    Ssql += string.Format(" And Company='%{0}%'", CompanyList1.SelectValue);
        //}
        //else
        //{
        //    Ssql += string.Format(" And Company like '%{0}%'", CompanyList1.SelectValue);
        //}
        //if (tbEmployeeId.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And EmployeeId like '%{0}%'", tbEmployeeId.Text);
        //}
        //公司
        DDLSr("LI.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("PM.DeptId", SearchList1.DepartmentValue);

        //員工
        DDLSr("LI.EmployeeId", SearchList1.EmployeeValue);
        //08.30 修改
        //if (tbEmployeeName.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And EmployeeName like '%{0}%'", tbEmployeeName.Text);
        //}

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;
        Label lbl;

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
                    e.Row.Cells[5].Text = py.DeCodeAmount(e.Row.Cells[5].Text).ToString("N0");
                }

                link = (LinkButton)e.Row.FindControl("btnSelect");
                link.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_I.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','height=800px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');");
                //"&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() +
                link = (LinkButton)e.Row.FindControl("btnEdit");
                link.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_U.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','height=800px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');");
                break;

            case DataControlRowType.Header:

                link = (LinkButton)e.Row.FindControl("btnNew");
                if (link != null)
                {
                    link.Attributes.Add("onclick", "javascript:var win =window.open('LaborInsurance_A.aspx','','width=600px,height=400px,top=100px,left=100px,scrollbars=yes,resizable=yes');");
                }
                break;
        }
    }

    private void BindData()
    {
        Ssql = "SELECT LI.* FROM LaborInsurance LI left join Personnel_Master PM on LI.Company=PM.Company and LI.EmployeeId=PM.EmployeeId Where 0=0";
        //if (CompanyList1.SelectValue == "")
        //{
        //        Ssql += string.Format(" And Company='%{0}%'", CompanyList1.SelectValue);
        //}
        //else
        //{
        //    Ssql += string.Format(" And Company like '%{0}%'", CompanyList1.SelectValue);
        //}
        //if (tbEmployeeId.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And EmployeeId like '%{0}%'", tbEmployeeId.Text);
        //}
        //08.30 修改
        //if (tbEmployeeName.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And (EmployeeName like '%{0}%' Or EnglishName like '%{0}%' Or TitleCode like '%{0}%')", tbEmployeeName.Text);
        //}
        //公司
        DDLSr("LI.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("PM.DeptId", SearchList1.DepartmentValue);

        //員工
        DDLSr("LI.EmployeeId", SearchList1.EmployeeValue);
        SDS_GridView.SelectCommand = Ssql + " Order By LI.Company,Li.EmployeeId";
        GridView1.DataBind();

        Navigator1.BindGridView = GridView1;
        //Navigator1.SQL = SDS_GridView.SelectCommand;
        Navigator1.DataBind();
    }
    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr(string Name, string Value)
    {
        if (Value.Length > 0)
            Ssql += string.Format(" And " + Name + " like '%{0}%'", Value);
        else
            Ssql += string.Format(" And " + Name + " = '{0}'", Value);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindData();
        showPanel();
        if (SearchList1.CompanyValue == "")
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "LaborInsurance";
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

        showPanel();
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        L2PK = L2PK.Substring(0, 10).Trim();
        string sql = "Delete From LaborInsurance Where Company='" + L1PK + "' And EmployeeId='" + L2PK + "'";
        //And EmployeeId='" + L2PK + "'
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
        BindData();
        showPanel();
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
                if (i != 5)
                    e.Row.Cells[i].Style.Add("text-align", "left");
                else
                    e.Row.Cells[i].Style.Add("text-align", "right");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    protected void btReSetAll_Click(object sender, EventArgs e)
    {
        if (SearchList1.CompanyValue.Replace("%", "").Length == 0)
        {
            lbl_Msg.Text = "請先指定公司!";
        }
        else
        {
            Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
            PayrollLt.Company = SearchList1.CompanyValue.Trim();
            PayrollLt.DeCodeKey = "dbo.setLI" + DateTime.Now.ToString("yyyyMMddHHmm");
            py.BeforeQuery(PayrollLt.DeCodeKey);
            ReSetLIAmount(PayrollLt);
            py.AfterQuery(PayrollLt.DeCodeKey);
            BindData();
        }
        PanPacificClass.JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面      
    }

    private void ReSetLIAmount(Payroll.PayrolList PayrollLt)
    {
        Ssql = "Select * From [Personnel_Master] Where IsNull(ResignCode,'') != 'Y' And Not(IsNull(ResignDate,HireDate-1) Between HireDate And GetDate()) " +
            " And [Company] = '" + PayrollLt.Company + "'";
        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        for (int i = 0; i < tb.Rows.Count;i++ )
        {
            PayrollLt.EmployeeId = tb.Rows[i]["EmployeeId"].ToString().Trim();
            LaborInsurance.Insurance LI = LaborInsurance.GetPremium(PayrollLt);
            //LI.Insured_Amount
            Ssql = "Select Count(*) From [LaborInsurance] Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
            DataTable tbcheck = _MyDBM.ExecuteDataTable(Ssql);
            if (tbcheck.Rows[0][0].ToString().Equals("0"))
            {
                Ssql = "INSERT INTO [LaborInsurance]([Company],[EmployeeId],[TrxType],[EffectiveDate],[LI_amount]) " +
                    " VALUES('" + PayrollLt.Company + "','" + PayrollLt.EmployeeId + "','A1',GetDate()," +
                    PayrollLt.DeCodeKey + "To('" + LI.Insured_Amount.ToString() + "'))";
            }
            else
            {
                Ssql = "Update [LaborInsurance] Set [LI_amount] = " + PayrollLt.DeCodeKey + "To('" + LI.Insured_Amount.ToString() + "') " +
                    //" , [TrxType] = 'A2'" +
                    " Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
            }
            _MyDBM.ExecuteCommand(Ssql);
        }
    }
}
