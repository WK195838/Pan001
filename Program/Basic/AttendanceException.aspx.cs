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

public partial class Basic_AttendanceException : System.Web.UI.Page
{
    string Ssql = "";
    string strSQLCommand = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM072";
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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PayrollControl.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        ////驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        //string[] Auth = { "Delete", "Modify", "Add" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {
    //        for (i = 0; i < Auth.Length; i++)
    //        {
    //            Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
    //            if (i < (Auth.Length - 1))
    //            {//刪/修/詳
    //                //GridView1.Columns[i].Visible = Find;
    //                //設定標題樣式
    //                if (Find && (SetCss == false))
    //                {
    //                    SetCss = true;
    //                    GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
    //                }
    //            }
    //            else
    //            {//新增
    //                GridView1.ShowFooter = Find;
    //            }
    //        }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            {
                Find = true;
            }
            else
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            ////版面樣式調整
            //if (SetCss == false)
            //{
            //    GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            //}
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        Navigator1.BindGridView = GridView1;
        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
        //CompanyList1.SelectedIndex += new UserControl_CompanyList.SelectedIndexChanged(CompanyList1_SelectedIndex);
        //DepList1.SelectedIndexChanged += new EventHandler(DepList1_SelectedIndexChanged);
        Memo.SelectedIndexChanged += new EventHandler(Memo_SelectedIndexChanged);
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
            AuthRight();

            SearchList1.CompanyValue = _UserInfo.UData.Company;

            SalaryYMS.SetOtherYMList();
            SalaryYMS.SelectSalaryYM = DateTime.Now.ToString("yyyyMM");

            SalaryYME.SetOtherYMList();
            SalaryYME.SelectSalaryYM = DateTime.Now.ToString("yyyyMM");

            insertdata();
            updatedata();
            checkleave();
            BindData();            
        }
        else
        {
            sCompany = SearchList1.CompanyValue.Trim().Replace("%", "");
            BindData();            
        }
        tbAttendanceDate.CssClass = "JQCalendar";
        //btnCalendar1.Attributes.Add("onclick", "return GetPromptDate(" + tbAttendanceDate.ClientID + ");");
    }
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();       
    }

    void Memo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();        
    }
    private ListItem Li(string text, string value)
    {
        ListItem li = new ListItem();
        li.Text = text;
        li.Value = value;
        return li;
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        BindData();
                
        if (SearchList1.CompanyValue == "")
        {
            lbl_Msg.Text = "請選擇公司編號";
        }
    }
    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr(string Name, string Value)
    {
        if (Value.Length > 0)
            Ssql += string.Format(" And " + Name + " like '%{0}%'", Value);
        else
            Ssql += string.Format(" And " + Name + " = '{0}'",Value);
    }
    private void BindData()
    {
        Ssql = "SELECT *,AECode+'-'+(Select CodeName From CodeDesc where CodeID='PY#AECode' And CodeCode=AECode) As [AEDesc]" +
            " ,(Case When OverTime > 0 Then ' 遲到'+Convert(varchar,OverTime)+'分 ' Else Memo End) ShowMeno " +
            " FROM AttendanceException Where 0=0";
        //公司
        DDLSr("Company", SearchList1.CompanyValue);

        //部門
        DDLSr("DeptId", SearchList1.DepartmentValue);
        //員工
        DDLSr("EmployeeId", SearchList1.EmployeeValue);


        if (tbAttendanceDate.Text.Length > 0)
        {
            string cformatdate = tbAttendanceDate.Text.Trim();
            if (cformatdate.Length > 7)
                cformatdate = _UserInfo.SysSet.FormatADDate(cformatdate);
            Ssql += string.Format(" And Convert(varchar,AttendanceDate,111) like '%{0}%'", cformatdate.Trim());
        }
        if (Memo.SelectedValue.Length > 0)
        {
            Ssql += string.Format(" And IsNull(AECODE,'0') = '{0}'", Memo.SelectedValue);
        }
        if (SalaryYMS.SelectSalaryYM != "")
        {
            Ssql += string.Format(" And Payroll_Processingmonth >= {0}", SalaryYMS.SelectSalaryYM);
        }
        if (SalaryYME.SelectSalaryYM != "")
        {
            Ssql += string.Format(" And Payroll_Processingmonth <= {0}", SalaryYME.SelectSalaryYM);
        }
        //0	正常		1	遲到		2	早退		3	公出		4	休假		5	忘刷		9	曠職		B	駐點		Z	免刷卡	
        Ssql += " And ((Not Memo in ('正常上下班','休假','請假') And IsNull(AECODE,'0') in ('1','2','5','9')) Or (OverTime > 0)) ";
        SDS_GridView.SelectCommand = Ssql ;// or AECode<>'0' and AECode is not null
        GridView1.DataBind();

        Navigator1.Visible = false;
        Panel_Empty.Visible = false;
        if (GridView1.PageCount > 0)
            Navigator1.Visible = true;
        else if (GridView1.Rows.Count == 0)
            Panel_Empty.Visible = true;

        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[3].Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
            {
                #region 修改用
                ////確認
                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onclick", "return confirm('確定要修改資料嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "');");

                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                ////取消
                //if (e.Row.Cells[1].Controls[2] != null)
                //{
                //    ImageButton IB = ((ImageButton)e.Row.Cells[1].Controls[2]);
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}
                #endregion
                //string c = ;
            }
            else
            {
                #region 查詢用
                ////e.Row.Cells[4].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[4].Text;

                //if (e.Row.Cells[1].Controls[0] != null)
                //{
                //    ImageButton IB = (ImageButton)e.Row.Cells[1].Controls[0];
                //    IB.Attributes.Add("onmouseout", "this.filters['alpha'].opacity=50");
                //    IB.Attributes.Add("onmouseover", "this.filters['alpha'].opacity=100");
                //    IB.Style.Add("filter", "alpha(opacity=50)");
                //}                
                string sCom = SearchList1.CompanyValue.ToString();
                if (!string.IsNullOrEmpty(DBSetting.PersonalName(sCom, e.Row.Cells[0].Text.Trim())))
                {
                    if (DBSetting.PersonalName(sCom, e.Row.Cells[0].Text.Trim()).ToString() != "")
                    {

                        e.Row.Cells[0].Text = e.Row.Cells[0].Text + " - " + DBSetting.PersonalName(sCom, e.Row.Cells[0].Text.Trim()).ToString();
                    }
                }
                if (!string.IsNullOrEmpty(DBSetting.DepartmentName(sCom, e.Row.Cells[2].Text.Trim())))
                {
                    if (DBSetting.DepartmentName(sCom, e.Row.Cells[2].Text.Trim()).ToString() != "")
                    {

                        e.Row.Cells[2].Text = e.Row.Cells[2].Text + " - " + DBSetting.DepartmentName(sCom, e.Row.Cells[2].Text.Trim()).ToString();
                    }
                }
                //e.Row.Cells[1].Text = (int.Parse(e.Row.Cells[1].Text.Substring(0, 4).ToString()) - 1911).ToString() + e.Row.Cells[1].Text.Substring(4, 2);
                if (e.Row.Cells[1].Text.Contains("/"))
                {
                    e.Row.Cells[1].Text = _UserInfo.SysSet.FormatDate(e.Row.Cells[1].Text).ToString();
                }

                if (!(string.IsNullOrEmpty(e.Row.Cells[4].Text.Trim())))
                {
                    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Replace("&nbsp;", "");
                    if (e.Row.Cells[4].Text.Trim().Length > 0)
                    {
                        e.Row.Cells[4].Text = e.Row.Cells[4].Text.Trim().PadLeft(4, '0').Insert(2, " : ");
                    }
                }

                if (!(string.IsNullOrEmpty(e.Row.Cells[5].Text.Trim())))
                {
                    e.Row.Cells[5].Text = e.Row.Cells[5].Text.Replace("&nbsp;", "");
                    if (e.Row.Cells[5].Text.Trim().Length > 0)
                    {
                        e.Row.Cells[5].Text = e.Row.Cells[5].Text.Trim().PadLeft(4, '0').Insert(2, " : ");
                    }
                }
                #endregion
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {

            strValue = "";
            e.Row.Visible = false;
            #region 新增用欄位
            //for (int i = 2; i < e.Row.Cells.Count; i++)
            //{
            //    if (i == 2 || i == 4 || i == 5)
            //    {
            //        TextBox tbAddNew = new TextBox();
            //        tbAddNew.ID = "tbAddNew" + (i - 1).ToString().PadLeft(2, '0');
            //        if (i == 4 || i == 5)
            //        {
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Style.Add("width", "70px");
            //        }
            //        e.Row.Cells[i].Controls.Add(tbAddNew);
            //        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";
            //        if (i == 4 || i == 5)
            //        {//為日期欄位增加小日曆元件
            //            ImageButton btOpenCal = new ImageButton();
            //            btOpenCal.ID = "btOpenCal" + i.ToString();
            //            btOpenCal.SkinID = "Calendar1";
            //            btOpenCal.OnClientClick = "return GetPromptDate(" + tbAddNew.ClientID + ");";
            //            e.Row.Cells[i].Controls.Add(btOpenCal);
            //        }
            //        if (i == 2)
            //        {
            //            ImageButton btOpenList = new ImageButton();
            //            btOpenList.ID = "btOpen" + i.ToString();
            //            btOpenList.SkinID = "OpenWin1";
            //            //Company,CompanyShortName,CompanyName,ChopNo
            //            btOpenList.OnClientClick = "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');";
            //            e.Row.Cells[i].Controls.Add(btOpenList);
            //        }
            //    }


            //    if (i == 3)
            //    {
            //        DropDownList ddlAddNew = new DropDownList();
            //        ddlAddNew.ID = "YearMonth" + (i - 1).ToString().PadLeft(2, '0');
            //        int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //        for (int n = (nowyear - 1); n < (nowyear + 11); n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew.Items.Add(tmp);
            //        }
            //        ddlAddNew.SelectedValue = nowyear.ToString();
            //        e.Row.Cells[i].Controls.Add(ddlAddNew);
            //        strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
            //        DropDownList ddlAddNew2 = new DropDownList();
            //        ddlAddNew2.ID = "YearMonth" + i.ToString().PadLeft(2, '0');
            //        for (int n = 1; n < 13; n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew2.Items.Add(tmp);
            //        }
            //        ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));
            //        e.Row.Cells[i].Controls.Add(ddlAddNew2);
            //        strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
            //    }

            //}

            //ImageButton btAddNew = new ImageButton();
            //btAddNew.ID = "btAddNew";
            //btAddNew.SkinID = "NewAdd";
            //btAddNew.CommandName = "Insert";
            //btAddNew.OnClientClick = "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));";
            //e.Row.Cells[1].Controls.Add(btAddNew);
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
        {
            //權限
            //e.Row.Visible = GridView1.ShowFooter;
            e.Row.Visible = false;
            #region 新增用欄位

            //strValue = "";

            //for (int i = 1; i < 5; i++)
            //{
            //    if (i == 1 || i == 3 || i == 4)
            //    {
            //        TextBox tbAddNew = (TextBox)e.Row.FindControl("tbAddNew0" + i.ToString());
            //        if (i == 3 || i == 4)
            //        {
            //            tbAddNew.Style.Add("text-align", "right");
            //            tbAddNew.Style.Add("width", "70px");
            //        }
            //        strValue += "checkColumns(" + tbAddNew.ClientID + ") && ";

            //        if (i == 3 || i == 4)
            //        {//為日期欄位增加小日曆元件
            //            ImageButton btnCalendar = (ImageButton)e.Row.FindControl("btnCalendar" + (i - 2).ToString());
            //            if (btnCalendar != null)
            //                btnCalendar.Attributes.Add("onclick", "return GetPromptDate(" + tbAddNew.ClientID + ");");
            //        }
            //        if (i == 1)
            //        {
            //            ImageButton btnOpen = (ImageButton)e.Row.FindControl("btnOpen" + i.ToString());
            //            //btOpenList.SkinID = "OpenWin1";
            //            //Company,CompanyShortName,CompanyName,ChopNo
            //            if (btnOpen != null)
            //            {
            //                //btnOpen.SkinID = "OpenWin1";
            //                btnOpen.Attributes.Add("onclick", "return GetPromptWin1(" + tbAddNew.ClientID + ",'400','450','Company_Master','Company,CompanyShortName','CompanyShortName As 公司簡稱,CompanyName,ChopNo','Company');");
            //            }
            //            //e.Row.Cells[i].Controls.Add(btOpenList);
            //        }
            //    }
            //    if (i == 2)
            //    {
            //        DropDownList ddlAddNew = new DropDownList();
            //        ddlAddNew = (DropDownList)e.Row.FindControl("YearMonth" + i.ToString().PadLeft(2, '0'));
            //        int nowyear = int.Parse(DateTime.Now.Year.ToString()) - 1911;
            //        for (int n = (nowyear - 1); n < (nowyear + 11); n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew.Items.Add(tmp);
            //        }
            //        ddlAddNew.SelectedValue = nowyear.ToString();
            //        //e.Row.Cells[i].Controls.Add(ddlAddNew);
            //        strValue += "checkColumns(" + ddlAddNew.ClientID + ") && ";
            //        DropDownList ddlAddNew2 = new DropDownList();
            //        ddlAddNew2 = (DropDownList)e.Row.FindControl("YearMonth" + (i + 1).ToString().PadLeft(2, '0'));
            //        for (int n = 1; n < 13; n++)
            //        {
            //            string tmp = n.ToString("D2");
            //            ddlAddNew2.Items.Add(tmp);
            //        }
            //        ddlAddNew2.SelectedValue = (DateTime.Now.Month.ToString("D2"));

            //        //e.Row.Cells[i].Controls.Add(ddlAddNew2);
            //        strValue += "checkColumns(" + ddlAddNew2.ClientID + ") && ";
            //    }
            //}

            //ImageButton btnNew = (ImageButton)e.Row.FindControl("btnNew");
            //if (btnNew != null)
            //    btnNew.Attributes.Add("onclick", "return (" + strValue + " confirm('確定要新增嗎?') && SaveValue(" + hid_IsInsertExit.ClientID + ",'" + e.Row.ClientID + "'));");
            #endregion
        }
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
            int i = 0;
            for (i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
            }

            //i = e.Row.Cells.Count - 1;
            //if (i > 0)
            //{
            //    e.Row.Cells[i - 1].Style.Add("text-align", "right");
            //    e.Row.Cells[i - 1].Style.Add("width", "100px");
            //}
            //e.Row.Cells[i].Style.Add("text-align", "right");
            //e.Row.Cells[i].Style.Add("width", "100px");

        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
    //異常表新增資料
    private void insertdata()
    {
        //strSQLCommand = "select AttendanceSheet.* from AttendanceSheet " +
        //    " where not exists(select AE.Company,AE.EmployeeId,AE.AttendanceDate,AE.DeptId,AE.CardNo,AE.InTime,AE.OutTime,AE.Memo" +
        //    " from AttendanceException AE Where AttendanceSheet.Company=AE.Company And AttendanceSheet.EmployeeId=AE.EmployeeId " +
        //    " And Convert(varchar,AttendanceSheet.AttendanceDate,111)=Convert(varchar,AE.AttendanceDate,111)) ";

        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        strSQLCommand = "select Count(*) from AttendanceSheet ADS " +
            " Where " + Ssql + " And Not [Payroll_Processingmonth] in (Select SalaryYM from PayrollControl Where " + Ssql + " And Not([ConfirmDate] is Null))";

        DataTable dt = _MyDBM.ExecuteDataTable(strSQLCommand);
        if (dt != null && ((int)dt.Rows[0][0]) > 0)
        {
            #region 開始匯入前,先寫入LOG
            int result = 0;
            DateTime StartDateTime = DateTime.Now;
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AttendanceException";
            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = Ssql;
            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
            //此時不設定異動結束時間
            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion

            //刪除已計薪確認之卡鐘重匯資料
            result = _MyDBM.ExecuteCommand("DELETE FROM AttendanceSheet WHERE " + Ssql + " And Payroll_Processingmonth in (Select Distinct SalaryYM FROM PayrollControl Where " + Ssql + " And Not([ConfirmDate] is Null))");

            //刪除未計薪確認之差勤資料
            result = _MyDBM.ExecuteCommand("DELETE FROM AttendanceException  WHERE " + Ssql + " And Payroll_Processingmonth in (Select Distinct Payroll_Processingmonth FROM AttendanceSheet WHERE " + Ssql + ")");

            strSQLCommand = "Insert Into AttendanceException(Company,EmployeeId,AttendanceDate,DeptId,CardNo,InTime,OutTime,Memo,Payroll_Processingmonth) " +
                " select Company,EmployeeId,AttendanceDate" +
                " ,(Case When IsNull(DeptId,'') ='' Then (Select DeptId FROM  Personnel_Master WHERE ADS.Company=Company And ADS.EmployeeId=EmployeeId) " +
                " Else DeptId End) As DeptId,CardNo,InTime,OutTime,Memo,Payroll_Processingmonth from AttendanceSheet ADS " +
                " WHERE " + Ssql + " And [Payroll_Processingmonth] in (Select Distinct SalaryYM FROM PayrollControl Where " + Ssql + " And Not([ConfirmDate] is Null))";

            SDS_GridView.InsertCommand = strSQLCommand;
            int j = 0;
            try
            {
                j = SDS_GridView.Insert();

                if (j >= 0)
                {
                    //刪除卡鐘已匯入之資料
                    result = _MyDBM.ExecuteCommand("DELETE FROM AttendanceSheet WHERE " + Ssql);
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
        }
        
    }

    private void updatedata()
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        string Status = "", AEcode = "", ATdate = "", AEInTime = "", AEOutTime = "", dbShiftCode = "", dbDayShift = "", dbInTime = "", dbOutTime = "";
        int n ;
        //只取未計薪確認,且無差勤代碼之資料做更新
        strSQLCommand = "select * from AttendanceException Where " + Ssql + " And (IsNull(AECode,'')='' Or (OverTime>0 And AECode != '1')) " +
        " And Not Payroll_Processingmonth in (Select Distinct SalaryYM FROM PayrollControl Where " + Ssql + " And Not([ConfirmDate] is Null))";
        DataTable tb1 = _MyDBM.ExecuteDataTable(strSQLCommand);
        if (tb1 != null && tb1.Rows.Count > 0)
        {
            #region
            for (int i = 0; i < tb1.Rows.Count; i++)
            {
                AEInTime = tb1.Rows[i]["InTime"].ToString().Trim();
                AEOutTime = tb1.Rows[i]["OutTime"].ToString().Trim();

                if (ValidateTimeFormat(AEInTime, AEOutTime))
                {//差勤時間格式正確
                    Status = "";
                    AEcode = "";
                    string attdate = _UserInfo.SysSet.FormatADDate(tb1.Rows[i]["AttendanceDate"].ToString().Trim());
                    //找出員工個人班表
                    strSQLCommand = "select (Case When IsNull(Shift,'')='' Then '0' else Shift End) As Shift " +
                        " ,(Case When OutTime > InTime Then 'D' else 'N' end ) As DayShift,S.* from Personnel_Master PM Left Join ( " +
                        " select CodeCode, Substring(CodeName,CHARINDEX('(', CodeName)+1,CHARINDEX('-', CodeName)-CHARINDEX('(', CodeName)-1) as InTime " +
                        " ,Substring(CodeName,CHARINDEX('-', CodeName)+1,CHARINDEX(')', CodeName)-CHARINDEX('-', CodeName)-1) as OutTime " +
                        " From CodeDesc Where CodeID = 'PY#Shift' And CHARINDEX('(', CodeName)>0 And CHARINDEX(')', CodeName)>0 and CHARINDEX('-', CodeName)>0 " +
                        " ) S On (Case When IsNull(Shift,'')='' Then '0' else Shift End) = S.CodeCode" +
                        " Where Company='" + tb1.Rows[i]["Company"].ToString().Trim() + "' and EmployeeId='" + tb1.Rows[i]["EmployeeId"].ToString().Trim() + "'";
                    DataTable tb2 = _MyDBM.ExecuteDataTable(strSQLCommand);
                    if (tb2 != null && tb2.Rows.Count > 0)
                    {//找出員工班表設定
                        #region
                        if (int.TryParse(tb2.Rows[0]["Shift"].ToString(), out n) && tb2.Rows[0]["CodeCode"] != null)
                        {//數字為一般班表
                            dbShiftCode = tb2.Rows[0]["CodeCode"].ToString();
                            dbDayShift = tb2.Rows[0]["DayShift"].ToString();
                            dbInTime = tb2.Rows[0]["InTime"].ToString();
                            dbOutTime = tb2.Rows[0]["OutTime"].ToString();
                            #region ---班表判斷---
                            // 公出 休假 曠職 駐點 免刷卡 忘刷卡
                            ATdate = _UserInfo.SysSet.FormatADDate(tb1.Rows[i]["AttendanceDate"].ToString());
                            if (AEInTime.Trim() == "" && AEOutTime.Trim() == "")
                            {//無刷卡資料
                                #region ---請假曠職判斷---
                                strSQLCommand = "select * from Holiday where convert(varchar,HolidayDate,111)='" + ATdate + "' and Company='" + tb1.Rows[i]["Company"].ToString() + "'";
                                DataTable tb3 = _MyDBM.ExecuteDataTable(strSQLCommand);
                                if (tb3 != null)
                                {
                                    if (tb3.Rows.Count > 0)
                                    {
                                        Status = "休假日";
                                        AEcode = "0";
                                    }
                                    else
                                    {
                                        Status = setleave(tb1.Rows[i]["Company"].ToString(), tb1.Rows[i]["EmployeeId"].ToString(), ATdate, null, null);
                                        if (Status == "")
                                        {
                                            Status = "曠職";
                                            AEcode = "9";
                                        }
                                        else
                                        {
                                            Status = "請假";
                                            AEcode = "4";
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if ((AEInTime.Trim() != "" && AEOutTime.Trim() == "")
                                || (AEInTime.Trim() == AEOutTime.Trim()))
                            {
                                //因為卡鐘不可能只有下班時間而無上班時間,故只要考慮[皆無],[皆有]及[只有上班時間]
                                //上下班時間相同時=當日有上班時間,無下班時間
                                if (dbDayShift == "D")
                                {//日班
                                    #region ---非夜班---
                                    TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                    if (intime.TotalMinutes > 0)
                                    {
                                        Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘 下班未刷卡";
                                        AEcode = "1";
                                    }
                                    else
                                    {
                                        Status = "上班正常 下班未刷卡";
                                        AEcode = "5";
                                    }
                                    #endregion
                                }
                                else
                                {//夜班(要有實際案例才好調整,現在是預想的)
                                    #region ---上班刷卡下班未刷卡---
                                    //夜班當天刷卡
                                    string starttime = "";
                                    TimeSpan tdintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbInTime);
                                    TimeSpan tmintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime);
                                    if (tdintime.TotalMinutes < 0 && tmintime.TotalMinutes < 0)
                                    {
                                        starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1).ToString();
                                    }
                                    else
                                    {
                                        starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).ToString();
                                    }
                                    TimeSpan dayintime = DateTime.Parse(starttime) - DateTime.Parse(attdate + " 00:00").AddDays(1);
                                    if (dayintime.TotalMinutes < 0)
                                    {
                                        TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                        if (intime.TotalMinutes > 0)
                                        {

                                            Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘 下班未刷卡";
                                            AEcode = "1";
                                        }
                                    }
                                    //夜班上班隔天刷卡
                                    else
                                    {
                                        TimeSpan intime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1) - DateTime.Parse(attdate + " " + dbInTime);

                                        if (intime.TotalMinutes > 0)
                                        {
                                            Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘 下班未刷卡";
                                            AEcode = "1";
                                        }
                                    }
                                    #endregion
                                }
                            }
                            else
                            {//有上下班時時,且上下班時間不相同
                                if (dbDayShift == "D")
                                {//日班
                                    #region ---非夜班上下班---
                                    TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                    if (intime.TotalMinutes > 0)
                                    {
                                        Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                        AEcode = "1";
                                    }
                                    else
                                    {
                                        Status = "上班正常";
                                    }
                                    TimeSpan outtime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                    if (outtime.TotalMinutes < 0)
                                    {
                                        Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                        if (AEcode == "") AEcode = "2";
                                    }
                                    else
                                    {
                                        Status += " 下班正常";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region ---夜班上班---
                                    string starttime = "";
                                    TimeSpan tdintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbInTime);
                                    TimeSpan tmintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime);
                                    if (tdintime.TotalMinutes < 0 && tmintime.TotalMinutes < 0)
                                    {
                                        starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1).ToString();
                                    }
                                    else
                                    {
                                        starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).ToString();
                                    }
                                    TimeSpan dayintime = DateTime.Parse(starttime) - DateTime.Parse(attdate + " 00:00").AddDays(1);
                                    //TimeSpan tdintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbInTime);
                                    //TimeSpan tmintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime);
                                    //夜班上班當天刷卡
                                    if (dayintime.TotalMinutes < 0)
                                    {
                                        TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                        if (intime.TotalMinutes > 0)
                                        {

                                            Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                            AEcode = "1";
                                        }
                                        else
                                        {
                                            Status = "上班正常";
                                        }
                                    }
                                    //夜班上班隔天刷卡
                                    else
                                    {
                                        TimeSpan intime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1) - DateTime.Parse(attdate + " " + dbInTime);

                                        if (intime.TotalMinutes > 0)
                                        {
                                            Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                            AEcode = "1";
                                        }
                                    }
                                    #endregion

                                    #region ---夜班下班---

                                    //夜班下班昨天刷卡
                                    string endtime = "";
                                    TimeSpan tdouttime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                    TimeSpan tmouttime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                    if (tdouttime.TotalMinutes > 0 && tmouttime.TotalMinutes > 0)
                                    {
                                        endtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")).ToString();
                                    }
                                    else
                                    {
                                        endtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1).ToString();
                                    }
                                    TimeSpan dayouttime = DateTime.Parse(endtime) - DateTime.Parse(attdate + " 00:00").AddDays(1);
                                    if (dayouttime.TotalMinutes < 0)
                                    {

                                        TimeSpan outtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime).AddDays(1);
                                        if (outtime.TotalMinutes < 0)
                                        {
                                            Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                            if (AEcode == "") AEcode = "2";
                                        }
                                    }
                                    else
                                    //夜班下班當天刷卡
                                    {
                                        TimeSpan outtime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                        if (outtime.TotalMinutes < 0)
                                        {
                                            Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                            if (AEcode == "") AEcode = "2";
                                        }
                                        else
                                        {
                                            Status += " 下班正常";
                                        }

                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else
                        {//非數字設定或未設於代碼表中為特殊班表
                            if (tb2.Rows[0]["Shift"].ToString().ToUpper() == "B")
                            {
                                Status = "駐點";
                                AEcode = "B";
                            }
                            else if (tb2.Rows[0]["Shift"].ToString().ToUpper() == "Z")
                            {
                                Status = "免刷卡";
                                AEcode = "Z";
                            }
                            else if (tb2.Rows[0]["Shift"].ToString().ToUpper() == "A")
                            {
                                #region 依排班表
                                strSQLCommand = "select ps.*,(Case When ss.OutTime > ss.InTime Then 'D' else 'N' end ) As DayShift,ss.InTime,ss.OutTime " +
                                    " from PersonalShift ps left join SpecialShift ss on ps.Company=ss.Company and ps.ShiftCode=ss.ShiftCode " +
                                    " Where ps.Company='" + tb1.Rows[i]["Company"].ToString().Trim() + "' and ps.EmployeeId='" + tb1.Rows[i]["EmployeeId"].ToString().Trim() + "'";
                                DataTable tb4 = _MyDBM.ExecuteDataTable(strSQLCommand);
                                if (tb4 != null && tb4.Rows.Count > 0)
                                {
                                    dbDayShift = tb4.Rows[0]["DayShift"].ToString();
                                    dbInTime = tb4.Rows[0]["InTime"].ToString();
                                    dbOutTime = tb4.Rows[0]["OutTime"].ToString();

                                    if (dbDayShift == "D")
                                    {
                                        #region ---班表無跨天---
                                        TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                        if (intime.TotalMinutes > 0)
                                        {
                                            Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                            AEcode = "1";
                                        }
                                        else
                                        {
                                            Status = "上班正常";
                                        }
                                        TimeSpan outtime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                        if (outtime.TotalMinutes < 0)
                                        {
                                            Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                            if (AEcode == "") AEcode = "2";
                                        }
                                        else
                                        {
                                            Status += " 下班正常";
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region ---班表有跨天---
                                        #region ---夜班上班---
                                        string starttime = "";
                                        TimeSpan tdintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbInTime);
                                        TimeSpan tmintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime);
                                        if (tdintime.TotalMinutes < 0 && tmintime.TotalMinutes < 0)
                                        {
                                            starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1).ToString();
                                        }
                                        else
                                        {
                                            starttime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).ToString();
                                        }
                                        TimeSpan dayintime = DateTime.Parse(starttime) - DateTime.Parse(attdate + " 00:00").AddDays(1);
                                        //TimeSpan tdintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbInTime);
                                        //TimeSpan tmintime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime);
                                        //夜班上班當天刷卡
                                        if (dayintime.TotalMinutes < 0)
                                        {
                                            TimeSpan intime = DateTime.Parse(AEInTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                            if (intime.TotalMinutes >= 0)
                                            {

                                                Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                                AEcode = "1";
                                            }
                                            else
                                            {
                                                Status = "上班正常";
                                            }
                                        }
                                        //夜班上班隔天刷卡
                                        else
                                        {
                                            TimeSpan intime = DateTime.Parse(attdate + " " + AEInTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1) - DateTime.Parse(attdate + " " + dbInTime);

                                            if (intime.TotalMinutes > 0)
                                            {
                                                Status = "遲到" + intime.TotalMinutes.ToString() + "分鐘";
                                                AEcode = "1";
                                            }
                                        }
                                        #endregion

                                        #region ---夜班下班---

                                        //夜班下班昨天刷卡
                                        string endtime = "";
                                        TimeSpan tdouttime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbInTime).TimeOfDay;
                                        TimeSpan tmouttime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                        if (tdouttime.TotalMinutes > 0 && tmouttime.TotalMinutes > 0)
                                        {
                                            endtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")).ToString();
                                        }
                                        else
                                        {
                                            endtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")).AddDays(1).ToString();
                                        } TimeSpan dayouttime = DateTime.Parse(endtime) - DateTime.Parse(attdate + " 00:00").AddDays(1);
                                        if (dayouttime.TotalMinutes < 0)
                                        {

                                            TimeSpan outtime = DateTime.Parse(attdate + " " + AEOutTime.PadLeft(4, '0').Insert(2, ":")) - DateTime.Parse(attdate + " " + dbOutTime).AddDays(1);
                                            if (outtime.TotalMinutes < 0)
                                            {
                                                Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                                if (AEcode == "") AEcode = "2";
                                            }
                                        }
                                        else
                                        //夜班下班當天刷卡
                                        {
                                            TimeSpan outtime = DateTime.Parse(AEOutTime.PadLeft(4, '0').Insert(2, ":")).TimeOfDay - DateTime.Parse(dbOutTime).TimeOfDay;
                                            if (outtime.TotalMinutes < 0)
                                            {
                                                Status += " 早退" + outtime.TotalMinutes.ToString().Substring(1, outtime.TotalMinutes.ToString().Length - 1) + "分鐘";
                                                AEcode = "2";
                                            }
                                            else
                                            {
                                                Status += " 下班正常";
                                            }

                                        }
                                        #endregion
                                        #endregion
                                    }                                    
                                }
                                #endregion
                            }
                        }

                        if (AEcode == "")
                        {
                            Status = "正常上下班";
                            AEcode = "0";
                        }

                        try
                        {
                            strSQLCommand = "UPDATE AttendanceException SET AECode=(Case When OverTime>0 Then '1' else '" + AEcode + "' End),Memo='" + Status +
                                "' where Company='" + tb1.Rows[i]["Company"].ToString().Trim() + "' and EmployeeId='" + tb1.Rows[i]["EmployeeId"].ToString().Trim() + "' and convert(varchar,AttendanceDate,111)='" + attdate + "'";
                            _MyDBM.ExecuteCommand(strSQLCommand);                            
                        }
                        catch (Exception ex)
                        {
                            lbl_Msg.Text = ex.Message.ToString();
                        }
                        #endregion                            
                    }
                }
                else
                { }
            }
            #endregion
        }
        //UPDATE AttendanceException
        //   SET AE.DeptId=Personnel_Master.DeptId
        //   FROM  Personnel_Master
        //       WHERE AE.Company=Personnel_Master.Company And AE.EmployeeId=Personnel_Master.EmployeeId  
    }

    /// <summary>
    /// 驗証起迄時間是否符合規定
    /// </summary>
    /// <param name="strIntime"></param>
    /// <param name="strOutTime"></param>
    /// <returns></returns>
    private bool ValidateTimeFormat(string strIntime, string strOutTime)
    {
        strIntime = strIntime.Trim();

        if (strIntime != "")
        {
            if (strIntime.Length < 3)
            {
                if (int.Parse(strIntime.PadLeft(2, '0')) > 59)
                {
                    return false;
                }
            }
            else if (int.Parse(strIntime.PadLeft(4, '0').Substring(0, 2)) > 23)
            {
                return false;
            }
        }

        strOutTime = strOutTime.Trim();
        if (strOutTime != "")
        {
            if (strOutTime.Length < 3)
            {
                if (int.Parse(strOutTime.PadLeft(2, '0')) > 59)
                {
                    return false;
                }
            }
            else if (int.Parse(strOutTime.PadLeft(4, '0').Substring(0, 2)) > 23)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 取得是否請假之資訊(回傳空值,表示無請假資料)
    /// </summary>
    /// <param name="strCompany"></param>
    /// <param name="strEmployeeId"></param>
    /// <param name="strAttendanceDate">yyyy/MM/dd</param>
    /// <param name="strInTime">暫無作用</param>
    /// <param name="strOutTime">暫無作用</param>
    /// <returns></returns>
    private string setleave(string strCompany, string strEmployeeId, string strAttendanceDate, string strInTime, string strOutTime)
    {
        string status = "";
        string attdate = strAttendanceDate.Trim();

        Ssql = "select LT.*,IsNull(LTB.Leave_Desc,LT.LeaveType_Id) As Leave_Desc " +
            " from Leave_Trx LT left join LeaveType_Basic LTB on LT.Company=LTB.Company and LT.LeaveType_Id=LTB.Leave_Id " +
            " Where LT.Company='" + strCompany + "' and LT.EmployeeId='" + strEmployeeId.Trim() + "'" +
             " And '" + attdate + "' Between Convert(char,BeginDateTime,111) And Convert(char,EndDateTime,111) ";

        DataTable tb2 = _MyDBM.ExecuteDataTable(Ssql);
        if (tb2 != null && tb2.Rows.Count > 0)
        {
            for (int i = 0; i < tb2.Rows.Count; i++)
            {
                if (status.Length > 0)
                    status += "|";

                if (!tb2.Rows[i]["Leave_Desc"].ToString().Contains("公出"))
                if (tb2.Rows[i]["ApproveDate"].ToString() != "")
                {
                    status += "請假已核定：" + tb2.Rows[i]["Leave_Desc"].ToString();
                }
                else
                {
                    status += "請假待核定：" + tb2.Rows[i]["Leave_Desc"].ToString();
                }
            }
        }
        return status;
    }

    /// <summary>
    /// 檢核是否有請假紀錄,並將原因寫入差勤表中
    /// </summary>
    private void checkleave()
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        string status = "";
        //將未計薪確認之月份裡,差勤異常資料找出
        strSQLCommand = "select * from AttendanceException Where AECode in ('1','2','5','9') " +
            " And Not Payroll_Processingmonth in (Select Distinct SalaryYM FROM PayrollControl Where " + Ssql + " And Not([ConfirmDate] is Null))";
        DataTable tb1 = _MyDBM.ExecuteDataTable(strSQLCommand);
        if (tb1 != null && tb1.Rows.Count > 0)
        {
           #region
            for (int i = 0; i < tb1.Rows.Count; i++)
            {
                status = "";
                string attdate = _UserInfo.SysSet.FormatADDate(tb1.Rows[i]["AttendanceDate"].ToString().Trim());
                //檢查差勤異常資料是否有請假單
                strSQLCommand = " select IsNull(LTB.Leave_Desc,LT.LeaveType_Id) As Leave_Desc ,LT.* " +
                    " from Leave_Trx LT Left Join LeaveType_Basic LTB On LT.Company=LTB.Company and LT.LeaveType_Id=LTB.Leave_Id" +
                    " Where LT.Company='" + tb1.Rows[i]["Company"].ToString().Trim() + "' and LT.EmployeeId='" + tb1.Rows[i]["EmployeeId"].ToString().Trim() + "'"+
                    " And '" + attdate + "' Between Convert(char,BeginDateTime,111) And Convert(char,EndDateTime,111) ";

                DataTable tb2 = _MyDBM.ExecuteDataTable(strSQLCommand);
                if (tb2 != null && tb2.Rows.Count > 0)
                {
                    for (int j = 0; j < tb2.Rows.Count; j++)
                    {
                        if (status.Length > 0)
                            status += "|";
                        if (!tb2.Rows[j]["Leave_Desc"].ToString().Contains("公出"))
                        if (tb2.Rows[j]["ApproveDate"].ToString() != "")
                        {
                            status += "請假已核定：" + tb2.Rows[j]["Leave_Desc"].ToString();
                        }
                        else
                        {
                            status += "請假待核定：" + tb2.Rows[j]["Leave_Desc"].ToString();
                        }
                    }
                        
                    if (status.Contains("已核定"))
                        strSQLCommand = " AECode=(Case When OverTime>0 Then '1' else '4' End), ";
                    else
                        strSQLCommand = "";

                    strSQLCommand = "UPDATE AttendanceException SET " + strSQLCommand + " Memo='" + status + 
                        "' where Company='" + tb1.Rows[i]["Company"].ToString() + "' and EmployeeId='" + tb1.Rows[i]["EmployeeId"].ToString().Trim() +
                        "' and convert(char,AttendanceDate,111)='" + attdate + "'";
                    _MyDBM.ExecuteCommand(strSQLCommand);
                }

            }
           #endregion
        }

    }

}
