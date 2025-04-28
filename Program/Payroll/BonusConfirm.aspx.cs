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
using System.Data.SqlClient;

public partial class BonusConfirm : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM023";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    decimal[] countAmt = new decimal[10];
    int DLShowKind = 2;

    string PayrollTable = "PayrollWorking";
    //標題補充文字
    string ShowTitle = "";
    string ShowMode = "(測試模式)";

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";

        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "PayrolltestWorking";
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'ePayroll' And RTrim(ProgramPath)='Payroll/Payroll015.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        string[] Auth = { "Detail", "Add" };

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
                {//新增(是否有試算權限)
                    btnConfirmPayroll.Visible = Find;                    
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);

        Navigator1.BindGridView = GridView1;
        // 需要執行等待畫面的按鈕
        btnConfirmPayroll.Attributes.Add("onClick", "drawWait('')");

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

            try
            {//設定下拉單是否顯示代碼或預設未選項
                DLShowKind = int.Parse(_UserInfo.SysSet.GetConfigString("DLShowKind"));
            }
            catch { }

            CostId.SetCodeList("PY#Bonus", DLShowKind, "全部");

            Navigator1.Visible = false;
        }
        else
        {
            LabMsg.Text = "";
            //SalaryYM1.SetSalaryYM(CompanyList1.SelectValue);

            if (Navigator1.Visible)
                BindData();
        }

        ScriptManager1.RegisterPostBackControl(btnConfirmPayroll);
    }

    /// <summary>
    /// 薪資試算確認
    /// </summary>
    protected void btnConfirmPayroll_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";
        Navigator1.Visible = false;
        GridView1.Visible = false;

        if (Session["SoftwareAuth"] == null || (bool)Session["SoftwareAuth"] == false)
        {
            LabMsg.Text = "您的軟體授權已到期!無法使用此功能";
            return;
        }
        else if (string.IsNullOrEmpty(txtAmtDate.Text))
        {
            LabMsg.Text = "請輸入獎金發放日期";
            return;
        }
        else if (hasData() == false)
        {
            LabMsg.Text = "查無可發放之獎金資料";
            return;
        }

        int retResult = 0;
        try
        {
            btnConfirmPayroll.Enabled = false;
            Ssql = "";
            #region 原確認發放功能            
            //Ssql = "Update BonusMaster " +
            //    " Set ControlDown = 'Y'" +
            //    " Where IsNull(ControlDown,'') != 'Y' ";

            ////公司
            //DDLSr("Company", SearchList1.CompanyValue);

            ////部門
            //DDLSr("DepId", SearchList1.DepartmentValue);
            ////員工
            //DDLSr("EmployeeId", SearchList1.EmployeeValue);

            ////獎金發放日期
            //if (!string.IsNullOrEmpty(txtAmtDate.Text))
            //    DDLSr(" Convert(varchar,AmtDate,111)", _UserInfo.SysSet.FormatADDate(txtAmtDate.Text));
            ////獎金名目
            //if (!string.IsNullOrEmpty(CostId.SelectedCode.Trim()))
            //    DDLSr("CostId", CostId.SelectedCode.Trim());

            ////判斷刪除駐記
            //DDLSr("Del_Mark", "");

            //retResult = _MyDBM.ExecuteCommand(Ssql);

            //LabMsg.Text = "確認發放" + retResult.ToString() + "筆";
            #endregion
            CountHI2AndConfirm();
            btnConfirmPayroll.Enabled = true;
        }
        catch (Exception ex)
        {
            LabMsg.Text = "確認發放失敗" + ex.Message;
        }
    }

    private void CountHI2AndConfirm()
    {
        string Ssql = "";
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.BonusHI2CountKey";
        ViewState["DeCodeKey"] = DeCodeKey;
        py.BeforeQuery(DeCodeKey);

        Ssql = "SP_PRA_BonusHI2Count";
        SqlCommand sqlcmd = new SqlCommand();
        sqlcmd.Parameters.Add("@ls_PayrollTable", System.Data.SqlDbType.VarChar).Value = PayrollTable;
        sqlcmd.Parameters.Add("@ls_Company", System.Data.SqlDbType.VarChar).Value = SearchList1.CompanyValue.Trim();
        sqlcmd.Parameters.Add("@ls_DepId", System.Data.SqlDbType.VarChar).Value = SearchList1.DepartmentValue.Trim().Replace("%", "");
        sqlcmd.Parameters.Add("@ls_EmployeeId", System.Data.SqlDbType.VarChar).Value = SearchList1.EmployeeValue.Trim().Replace("%", "");
        sqlcmd.Parameters.Add("@ls_AmtDate", System.Data.SqlDbType.VarChar).Value = _UserInfo.SysSet.FormatADDate(txtAmtDate.Text);
        sqlcmd.Parameters.Add("@ls_CostId", System.Data.SqlDbType.VarChar).Value = CostId.SelectedCode.Trim();
        sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = DeCodeKey;
        sqlcmd.Parameters.Add("@ls_ChgUser", System.Data.SqlDbType.VarChar).Value = _UserInfo.UData.UserId;

        DataSet Ds = new DataSet();
        int iRet = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters, out Ds);
        string strMsg = "確認完成!!";
        if (Ds != null)
        {
            #region 處理結果
            if (Ds.Tables.Count > 0)
            {
                try
                {
                    iRet = (int)Ds.Tables[0].Rows[0][0];
                    switch (iRet)
                    {
                        case -1:
                            strMsg = "無法確認發放!";
                            break;
                        case -2:
                            strMsg = "無法確認發放!";
                            break;
                        default:
                            strMsg = "確認發放" + iRet.ToString() + "筆";
                            break;
                    }
                }
                catch { }
            }

            if (Ds.Tables.Count > 1)
            {
                try
                {
                    DataTable dt = Ds.Tables[1];
                    if (iRet < 0) strMsg += "<br>" + dt.Rows[0][0].ToString();
                }
                catch { }
            }

            DataTable dt2 = null;
            if (Ds.Tables.Count > 2)
            {
                try
                {
                    dt2 = Ds.Tables[2];
                }
                catch { }
            }
            GridView2.DataSource = dt2;
            GridView2.DataBind();
            #endregion
        }
        LabMsg.Text = strMsg;

        py.AfterQuery(DeCodeKey);
    }

    /// <summary>
    /// 薪資查詢
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(SearchList1.CompanyValue) || SearchList1.CompanyValue.Trim().Replace("%%", "") == "")
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {
            int retResult = 0;
            {
                Navigator1.Visible = true;
                GridView1.Visible = true;
                BindData();
            }

            if (retResult < 0)
            {
                if (retResult == 0)
                    LabMsg.Text = "查無資料!!";
                Navigator1.Visible = false;
                GridView1.Visible = false;
            }
        }
    }

    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();
        showPanel();
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

    private bool hasData()
    {
        Ssql = "Select A.Row_Count,A.Company,(A.DepId+'-'+A.DepName) DepName,(Rtrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId" +
       ",(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount" +
       ",Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown" +
       ",ISNULL(A.Pay_AMT,0)as Pay_AMT,ISNULL(A.HI2,0) as HI2 ";
        Ssql += "  From BonusMaster A  Where IsNull(ControlDown,'') != 'Y' ";

        //公司
        DDLSr("A.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("A.DepId", SearchList1.DepartmentValue);
        //員工
        DDLSr("A.EmployeeId", SearchList1.EmployeeValue);

        //獎金發放日期
        DDLSr(" Convert(varchar,A.AmtDate,111)", _UserInfo.SysSet.FormatADDate(txtAmtDate.Text));
        //獎金名目
        if (!string.IsNullOrEmpty(CostId.SelectedCode.Trim()))
            DDLSr("CostId", CostId.SelectedCode.Trim());

        //判斷刪除駐記
        DDLSr("A.Del_Mark", "");

        Ssql += "ORDER By DepId,EmployeeId";


        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        
        if (tb.Rows.Count > 0)
        {
            Panel_Empty.Visible = false;
            return true;
        }
        else
        {
            Panel_Empty.Visible = true;
            return false;
        }

    }

    // 取得下拉式選單Sql的設定，給予欄位名稱與值
    private void DDLSr(string Name, string Value)
    {//SQL條件值
        if (Value.Length > 0)
            Ssql += string.Format(" And (" + Name + " like '{0}%' OR " + Name + "='{0}')", Value);
        else
            Ssql += string.Format(" And (" + Name + " = '{0}' OR " + Name + " IS Null)", Value);
    }

    /// <summary>
    /// 獎金列表繫結
    /// </summary>
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;
        decimal decTemp = 0;
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
                    //調整欄位 
                    e.Row.Cells[3].Style.Add("text-align", "left");
                    e.Row.Cells[7].Style.Add("text-align", "right");
                    e.Row.Cells[8].Style.Add("text-align", "left");
                    e.Row.Cells[11].Style.Add("text-align", "left");
                    //發放年月_UserInfo.SysSet
                    int iColumns = 8;
                    e.Row.Cells[iColumns].Text = _UserInfo.SysSet.ToTWDate(e.Row.Cells[iColumns].Text);
                    e.Row.Cells[iColumns].Text = e.Row.Cells[iColumns].Text.Substring(0, e.Row.Cells[iColumns].Text.IndexOf("/")) + "年" + e.Row.Cells[iColumns].Text.Substring(e.Row.Cells[iColumns].Text.IndexOf("/") + 1, 2) + "月";

                    #region 4:獎金金額;5:代扣繳金額;6:補充保費
                    decimal theSum = 0;
                    int iMax = 7;
                    if (e.Row.Cells[13].Text.StartsWith("N"))
                    {
                        iMax -= 1;
                        e.Row.Cells[iMax].Text = "";
                        e.Row.Cells[iMax].Style.Add("text-align", "center");
                    }
                    for (iColumns = 4; iColumns < iMax; iColumns++)
                    {//6:獎金金額;7:代扣繳金額;8:補充保費
                        e.Row.Cells[iColumns].Style.Add("text-align", "right");
                        try
                        {
                            decTemp = Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(e.Row.Cells[iColumns].Text));
                            if (iColumns == 4) theSum = decTemp;
                            else theSum -= decTemp;
                            countAmt[iColumns] += decTemp;
                            e.Row.Cells[iColumns].Text = decTemp.ToString("N2").Replace(".00", "");//金額解密                                                                
                        }
                        catch { }
                        if (e.Row.Cells[iColumns].Text.Replace("&nbsp;", "") == "")
                            e.Row.Cells[iColumns].Text = "0";
                    }

                    //實計發放金額
                    countAmt[7] += theSum;
                    e.Row.Cells[7].Text = theSum.ToString("N2").Replace(".00", "");
                    #endregion
                }
                link = (LinkButton)e.Row.FindControl("btnSelect");

                link.Attributes.Add("onclick", "javascript:var win =window.open('../Basic/BonusMaster_U.aspx?Row_Count=" + GridView1.DataKeys[e.Row.RowIndex].Values["Row_Count"].ToString().Trim() + "&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&DepId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().IndexOf("-")) + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")) + "&CTName=" + GridView1.DataKeys[e.Row.RowIndex].Values["CostName"].ToString() + "&AmtDate=" + GridView1.DataKeys[e.Row.RowIndex].Values["AmtDate"].ToString() + " &Style=Q','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");

                break;

            case DataControlRowType.Header:

                for (int i = 4; i < 8; i++)
                {
                    countAmt[i] = 0;
                }
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "合計";
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Underline = true;
                e.Row.Cells[0].Font.Size = 12;
                e.Row.Cells[1].Visible = false;
                for (int i = 4; i < 8; i++)
                {
                    e.Row.Cells[i].Font.Bold = true;
                    e.Row.Cells[i].Font.Underline = true;
                    e.Row.Cells[i].Font.Size = 10;
                    e.Row.Cells[i].Text = countAmt[i].ToString("N2").Replace(".00", "");
                }
                break;
        }
    }

    private void BindData()
    {//讀取資料

        Ssql = "Select A.Row_Count,A.Company,(A.DepId+'-'+A.DepName) DepName,(Rtrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId" +
        ",(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount" +
        ",Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown" +
        ",ISNULL(A.Pay_AMT,0)as Pay_AMT,ISNULL(A.HI2,0) as HI2 ";
        Ssql += "  From BonusMaster A  Where IsNull(ControlDown,'') = 'Y' ";

        //公司
        DDLSr(" A.Company ", SearchList1.CompanyValue);

        //部門
        DDLSr(" A.DepId", SearchList1.DepartmentValue);
        //員工
        DDLSr(" A.EmployeeId", SearchList1.EmployeeValue);

        //發放日期
        if (!string.IsNullOrEmpty(txtAmtDate.Text))
        {
            DDLSr(" Convert(varchar,A.AmtDate,111)", _UserInfo.SysSet.FormatADDate(txtAmtDate.Text));
        }
        //獎金名目
        if (!string.IsNullOrEmpty(CostId.SelectedCode.Trim()))
            DDLSr("CostId", CostId.SelectedCode.Trim());

        //判斷刪除駐記
        DDLSr("A.Del_Mark", "");

        if (Session["SortColumnName"] != null)
        {
            string strTemp = ",AmtDate DESC,DepId,EmployeeId".ToUpper();
            strTemp = strTemp.Replace(Session["SortColumnName"].ToString().Replace(" desc", "").ToUpper(), "1");
            strTemp = strTemp.Replace(",1 DESC,", ",").Replace(",1,", ",");
            Ssql += " Order by " + Session["SortColumnName"].ToString() + strTemp;
        }
        else
        {
            Ssql += " Order By AmtDate DESC,DepId,EmployeeId ";
        }        

        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);
        GridView1.DataSource = tb;
        GridView1.DataBind();
        if (GridView1.Rows.Count > 0)
        {
            Panel_Empty.Visible = false;
            Navigator1.Visible = true;
        }
        else
        {
            Panel_Empty.Visible = true;
            Navigator1.Visible = false;
        }
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "BonusMaster";
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
                if (i == 5)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[i].CssClass = "Grid_GridLine";
                //e.Row.Cells[i].Style.Add("text-align", "left");
                if (i > 5 && i < 10)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                }
            }
        }
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string strSort = "";
        if (Session["SortColumnName"] != null)
            strSort = Session["SortColumnName"].ToString();
        if (strSort.EndsWith("desc"))
            Session["SortColumnName"] = e.SortExpression;
        else
            Session["SortColumnName"] = e.SortExpression + " desc";
        BindData();
    }
}
