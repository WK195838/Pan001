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

public partial class BonusMaster : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM018";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    //ASP.usercontrol_yearlist_ascx.SelectedIndexChanged Dyear = new UserControl_YearList.SelectedIndexChanged();
    decimal[] countAmt = new decimal[10];
    bool blNew = false;
    public static string AmtDate = string.Empty;
    int DLShowKind = 2;

    protected void Page_PreInit(object sender, EventArgs e)
    {
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + SysSetting.SystemName.Payroll + "' And RTrim(ProgramPath)='Basic/BonusMaster.aspx'");
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
                    btnEmptyNew.Visible = Find;
                    blNew = Find;
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
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
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
            //年月
            if (RB_Sel2.Checked == true)
            {
                YearList1.Enabled = true;
                MonthList1.Enabled = true;
            }
            else
            {
                YearList1.Enabled = false;
                MonthList1.Enabled = false;
            }
            //年月處理
            YearList1.initList();
            MonthList1.initList();

            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;

            try
            {//設定下拉單是否顯示代碼或預設未選項
                DLShowKind = int.Parse(_UserInfo.SysSet.GetConfigString("DLShowKind"));
            }
            catch { }
            //獎金名目
            CostId.SetCodeList("PY#Bonus", DLShowKind, "全部");
            RB_Sel1.Checked = true;
            BindData();
            //showPanel();
            AuthRight();

        }
        else
        {
            blNew = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "ADD");
            //再查詢先清除,查無資料訊息
            //Panel_Empty.Visible = false;


            //獎金名目
            //CostId_SelectedIndexChanged(0, EventArgs.Empty);

            //讀取資料
            BindData();
        }



        if (SearchList1.CompanyValue == "")
        {
            btnNew.Visible = false;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        }
        else if (blNew == true)
        {
            btnNew.Visible = true;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Company=" + SearchList1.CompanyValue.ToString() + "&DepId=" + SearchList1.DepartmentValue.ToString() + "&EmployeeId=" + SearchList1.EmployeeValue.ToString() + "&Style=A','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Company=" + SearchList1.CompanyValue.ToString() + "&DepId=" + SearchList1.DepartmentValue.ToString() + "&EmployeeId=" + SearchList1.EmployeeValue.ToString() + "&Style=A','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
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
        //年月處理
        AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString() + "/01";
        AmtDate = _UserInfo.SysSet.FormatADDate(AmtDate).ToString();
        AmtDate = AmtDate.Substring(0, AmtDate.IndexOf("/") + 3);

        Ssql = "Select A.Row_Count, A.Company,(A.DepId+'-'+A.DepName) DepName,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId" +
        ",(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount" +
        ",Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown" +
        ",ISNULL(A.Pay_AMT,0) as Pay_AMT,ISNULL(A.HI2,0) as HI2 ";

        Ssql += "  From BonusMaster A  Where 1=1 ";

        //公司
        DDLSr("A.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("A.DepId", SearchList1.DepartmentValue);
        //員工
        DDLSr("A.EmployeeId", SearchList1.EmployeeValue);

        //年月
        if (RB_Sel1.Checked != true)
        {
            DDLSr(" Convert(varchar(7),A.AmtDate,111)", AmtDate);
        }
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
                    e.Row.Cells[5].Style.Add("text-align", "left");
                    e.Row.Cells[9].Style.Add("text-align", "right");
                    e.Row.Cells[10].Style.Add("text-align", "left");
                    e.Row.Cells[13].Style.Add("text-align", "left");
                    //發放年月_UserInfo.SysSet
                    int iColumns = 10;
                    e.Row.Cells[iColumns].Text = _UserInfo.SysSet.ToTWDate(e.Row.Cells[iColumns].Text);
                    e.Row.Cells[iColumns].Text = e.Row.Cells[iColumns].Text.Substring(0, e.Row.Cells[iColumns].Text.IndexOf("/")) + "年" + e.Row.Cells[iColumns].Text.Substring(e.Row.Cells[iColumns].Text.IndexOf("/") + 1, 2) + "月";

                    #region 6:獎金金額;7:代扣繳金額;8:補充保費
                    decimal theSum = 0;
                    int iMax = 9;
                    //if (e.Row.Cells[13].Text.StartsWith("N"))
                    //{
                    //    iMax -= 1;
                    //    e.Row.Cells[iMax].Text = "";
                    //    e.Row.Cells[iMax].Style.Add("text-align", "center");
                    //}
                    for (iColumns = 6; iColumns < iMax; iColumns++)
                    {//6:獎金金額;7:代扣繳金額;8:補充保費
                        e.Row.Cells[iColumns].Style.Add("text-align", "right");
                        try
                        {
                            if(iColumns != 8)
                            {
                                decTemp = Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(e.Row.Cells[iColumns].Text));

                            }
                            else
                            {
                                decTemp = Convert.ToDecimal(e.Row.Cells[iColumns].Text);
                            }
                            if (iColumns == 6) theSum = decTemp;
                            else theSum -= decTemp;

                            countAmt[iColumns] += decTemp;
                            e.Row.Cells[iColumns].Text = decTemp.ToString("N2").Replace(".00", "");//金額解密                                                                
                        }
                        catch
                        {

                        }
                        if (e.Row.Cells[iColumns].Text.Replace("&nbsp;", "") == "")
                            e.Row.Cells[iColumns].Text = "0";
                    }


                    //實計發放金額
                    countAmt[9] += theSum;
                    e.Row.Cells[9].Text = theSum.ToString("N2").Replace(".00", "");
                    #endregion
                }
                link = (LinkButton)e.Row.FindControl("btnSelect");

                link.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Row_Count=" + GridView1.DataKeys[e.Row.RowIndex].Values["Row_Count"].ToString().Trim() + "&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&DepId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().IndexOf("-")) + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")) + "&CTName=" + GridView1.DataKeys[e.Row.RowIndex].Values["CostName"].ToString() + "&AmtDate=" + GridView1.DataKeys[e.Row.RowIndex].Values["AmtDate"].ToString() + " &Style=Q','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");

                link = (LinkButton)e.Row.FindControl("btnEdit");

                link.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Row_Count=" + GridView1.DataKeys[e.Row.RowIndex].Values["Row_Count"].ToString().Trim() + "&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&DepId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DepName"].ToString().IndexOf("-")) + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")) + "&CTName=" + GridView1.DataKeys[e.Row.RowIndex].Values["CostName"].ToString() + "&AmtDate=" + GridView1.DataKeys[e.Row.RowIndex].Values["AmtDate"].ToString() + " &Style=U','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");

                if (DataBinder.Eval(e.Row.DataItem, "ControlDown").ToString().Contains("Y"))
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "";
                }

                break;

            case DataControlRowType.Header:

                for (int i = 6; i < 10; i++)
                {
                    countAmt[i] = 0;
                }

                link = (LinkButton)e.Row.FindControl("btnNew");
                if (link != null)
                {
                    //指定位置用top=100px,left=100px,
                    link.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");

                }
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "合計";
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Underline = true;
                e.Row.Cells[0].Font.Size = 12;
                e.Row.Cells[1].Visible = false;
                for (int i = 6; i < 10; i++)
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

        //年月處理
        AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString() + "/01";
        AmtDate = _UserInfo.SysSet.FormatADDate(AmtDate).ToString();
        AmtDate = AmtDate.Substring(0, AmtDate.IndexOf("/") + 3);

        string lastYM = DateTime.Now.AddMonths(-1).ToString("yyyyMM");

        //lbl_Msg.Text = lastYM;
        Ssql = "Select A.Row_Count,A.Company,(A.DepId+'-'+A.DepName) DepName,(Rtrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId" +
        ",(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount" +
        ",Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown" +
        ",ISNULL(A.Pay_AMT,0)as Pay_AMT,Case When A.HI2 Is null then dbo.getHI2(dbo.DecodeAmount(CostAmt),dbo.DecodeAmount(Insured_amount)) else dbo.DecodeAmount(A.HI2) End as HI2 ";
        Ssql += "  From BonusMaster A Left Join [HealthInsurance_Heading] B On A.EmployeeId=B.EmployeeId Where 1=1 ";

        //公司
        DDLSr(" A.Company ", SearchList1.CompanyValue);

        //部門
        DDLSr(" A.DepId", SearchList1.DepartmentValue);
        //員工
        DDLSr(" A.EmployeeId", SearchList1.EmployeeValue);

        //年月
        if (RB_Sel1.Checked != true)
        {
            DDLSr(" Convert(varchar(7),A.AmtDate,111)", AmtDate);
        }

        //獎金名目
        if (!string.IsNullOrEmpty(CostId.SelectedCode.Trim()))
            DDLSr("CostId", CostId.SelectedCode.Trim());

        //判斷刪除駐記
        DDLSr("A.Del_Mark", "");

        Ssql += "ORDER By DepId,A.EmployeeId";

        SDS_GridView.SelectCommand = Ssql;
        GridView1.DataBind();
        if (GridView1.Rows.Count > 0)
        {
            Panel_Empty.Visible = false;
        }
        else
        {
            Panel_Empty.Visible = true;
        }
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();
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
    {//資料刪除 _UserInfo.SysSet
        LinkButton btnDelete = (LinkButton)sender;
        string L1PK = btnDelete.Attributes["L1PK"].ToString();
        string L2PK = btnDelete.Attributes["L2PK"].ToString();
        string L3PK = btnDelete.Attributes["L3PK"].ToString();
        string L4PK = btnDelete.Attributes["L4PK"].ToString();
        string L5PK = btnDelete.Attributes["L5PK"].ToString();
        string L6PK = btnDelete.Attributes["L6PK"].ToString();


        string sql = "";//= "Delete From BonusMaster Where Company='" + L1PK + "' And EmployeeId='" + L2PK.Substring(0, L2PK.IndexOf("-")) + "' and CostId='" + L3PK.Substring(0, L3PK.IndexOf("-")) + "' and AmtDate='" + L4PK + "' and Row_Count='" + L6PK + "'";

        sql = "Update BonusMaster ";
        sql += "  SET Del_Mark='Y' ";
        sql += " Where Company='" + L1PK + "' And EmployeeId='" + L2PK.Substring(0, L2PK.IndexOf("-")) + "' and CostId='" + L3PK.Substring(0, L3PK.IndexOf("-")) + "' and Cast(AmtDate as SmallDateTime)='" + _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(L4PK)) + "' and Row_Count='" + L6PK + "'";


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

    protected void RB_Sel1_CheckedChanged(object sender, EventArgs e)
    {
        if (RB_Sel1.Checked == true)
        {
            //全部選項還原
            RB_Sel1.Checked = true;
            RB_Sel1.BackColor = System.Drawing.Color.White;

            //民國選項失效,反灰色
            RB_Sel2.Checked = false;
            YearList1.Enabled = false;
            MonthList1.Enabled = false;
            RB_Sel2.BackColor = System.Drawing.Color.Gray;
        }
        BindData();
    }
    protected void RB_Sel2_CheckedChanged(object sender, EventArgs e)
    {
        if (RB_Sel2.Checked == true)
        {
            //民國選項還原
            RB_Sel2.Checked = true;
            YearList1.Enabled = true;
            MonthList1.Enabled = true;
            RB_Sel2.BackColor = System.Drawing.Color.White;

            //全部選項失效,反灰色
            RB_Sel1.Checked = false;
            RB_Sel1.BackColor = System.Drawing.Color.Gray;
        }
        BindData();
    }
}
