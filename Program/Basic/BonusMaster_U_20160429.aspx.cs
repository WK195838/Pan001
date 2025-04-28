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
using System.Globalization;
using System.Data.SqlClient;

public partial class BonusMaster_U_20160429 : System.Web.UI.Page
{
    #region 變數宣告
    //#---------------------------#//
    //string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM018";
    DBManger _MyDBM;
    SysSetting SysSet = new SysSetting();
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    string RowCount;
    public static int Date_Count = 0;

    public static string msg = "";

    public static string Style_page = "";

    public double Total = 0.0;//獎金總額

    public double PTotal = 0.0;//獎金總額

    public double Pamt = 0.0;//代扣繳稅額

    public double PTamt = 0.0;//定額時之代扣繳稅額

    string PayrollTable = "PayrollWorking";
    //標題補充文字
    string ShowTitle = "";
    string ShowMode = "(測試模式)";
    //#---------------------------#//
    #endregion

    protected void Page_PreInit(object sender, EventArgs e)
    {
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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + SysSetting.SystemName.Payroll + "' And RTrim(ProgramPath)='Basic/BonusMaster_U.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    #region 驗證權限
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
                    //GridView1.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        //GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                else
                {//新增
                    //btnNew.Visible = Find;
                    //btnEmptyNew.Visible = Find;
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
                //GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {//頁面啟動,所需執行之相關資料
        //下拉式選單連動設定
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);


        //日曆元件


        //20110124 日期選項新作法
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        //ScriptManager.RegisterStartupScript(UpdatePanel2, this.GetType(), "", @"JQ();", true);

        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString(), Page.ResolveUrl("~/Pages/pagefunction.js").ToString()); 



        //為日期欄位增加小日曆元件
        TxDate.CssClass = "JQCalendar";
 


        //銀行元件       
        ImageButton btOpenCal1 = new ImageButton();
        btOpenCal1.OnClientClick = "return GetPromptWin1(" + BankCode.ClientID + ",'400','450','Bank_Master','BankHeadOffice','BankHeadOffice As 銀行總行代號,BankAbbreviations','BankHeadOffice');";
        btOpenCal1.SkinID = "OpenWin1";
        DetailsView5.Controls.AddAt(2, btOpenCal1);      

        btnCancel.Attributes.Add("onclick", "javascript:window.close();"); 
        btnUpdate.Attributes.Add("onclick", "javascript:" + hid_InserMode.ClientID + ".value='EXIT';");  
       
        //判斷 AutoPosBack

        if (!Page.IsPostBack)
        {
            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "ADD") == false && _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Modify") == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            //獎金名目
            Money_name_SelectedIndexChanged(0, EventArgs.Empty);
            DP_Person_SelectedIndexChanged(0, EventArgs.Empty);

            //處理選項變換
            if (RB_standard.Checked == true)
            {
                RB_Person.Checked = false;
                RB_standard.Checked = true;
                return;
            }

            if (RB_Person.Checked == true)
            {
                RB_standard.Checked = false;
                RB_Person.Checked = true;
            }

           
            //判斷接收資料
            if (Request["Company"] != null)
            {
                SearchList1.CompanyValue = Request["Company"].Trim();

            }

            if (Request["DepId"] != null)
            {
                SearchList1.DepartmentValue = Request["DepId"].Trim();
            }

            if (Request["EmployeeId"] != null)
            {
                SearchList1.EmployeeValue = Request["EmployeeId"].Trim();
            }

            if (Request["CTName"] != null)
            {
                Money_name.Text = Request["CTName"].Trim();
            }

            //樣版標題定義
            if (Request["Style"] != null)
            {
                if (setSatus(Request["Style"]))
                {
                    Qurey();
                }
            }
        }

        GetHI2();
    }

    decimal SumC = 0, theB = 0;
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Style.Add("text-align", "center");
                try
                {
                    e.Row.Cells[0].Text = "項目";
                    e.Row.Cells[1].Text = "給付<br/>日期";
                    e.Row.Cells[2].Text = "當月<br/>投保金額<br/>(A)";
                    e.Row.Cells[3].Text = "4倍投保<br/>金額<br/>(B=AX4)";
                    e.Row.Cells[4].Text = "單次<br/>獎金金額<br/>(C)";
                    e.Row.Cells[5].Text = "累計<br/>獎金金額<br/>(D)";
                    e.Row.Cells[6].Text = "累計超過<br/>4倍投保<br/>金額之獎金<br/>(E=D-B)";
                    e.Row.Cells[7].Text = "補充保險費<br/>費基<br/>(F)<br/>min(E,C)";
                    e.Row.Cells[8].Text = "補充保險費<br/>金額<br/>(G=F*" + ((ViewState["HI62Rate"] != null) ? ViewState["HI62Rate"] : "2") + "%)";
                    e.Row.Cells[9].Text = "是否<br/>已發放";
                }
                catch { }
                SumC = 0;
                break;
            case DataControlRowType.DataRow:
                e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
                e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].CssClass = "Grid_GridLine";
                    if (i == e.Row.Cells.Count - 1)
                        e.Row.Cells[i].Style.Add("text-align", "center");//e.Row.Cells[i].Style.Add("font-weight", "bold");
                    else if (i > 1)
                        e.Row.Cells[i].Style.Add("text-align", "right");
                }
                break;
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            //case DataControlRowType.Header:
            //    if (ViewState["HI62Rate"] != null)
            //        e.Row.Cells[8].Text = "補充保險費<br/>金額<br/>(G=F*" + ViewState["HI62Rate"] + "%)";                
            //    break;
            case DataControlRowType.DataRow:
                string strYM = e.Row.Cells[1].Text.Remove(7).Replace("/", "");
                e.Row.Cells[1].Text = _UserInfo.SysSet.FormatDate(e.Row.Cells[1].Text);
                for (int i = 2; i < e.Row.Cells.Count - 1; i++)
                    try
                    {
                        decimal dTemp = 0;
                        decimal.TryParse(e.Row.Cells[i].Text, out dTemp);
                        switch (i)
                        {
                            case 2:
                                if (dTemp == 0)
                                {
                                    //設定計薪資料
                                    Payroll.PayrolList PayrollLtLast = new Payroll.PayrolList();
                                    PayrollLtLast.Company = SearchList1.CompanyValue;
                                    PayrollLtLast.EmployeeId = SearchList1.EmployeeValue;
                                    PayrollLtLast.PeriodCode = "0";
                                    PayrollLtLast.DeCodeKey = ViewState["DeCodeKey"].ToString();
                                    PayrollLtLast.SalaryYM = int.Parse(strYM);
                                    //取得指定年月之健保級距
                                    HealthInsurance.DependentPremium HI = HealthInsurance.GetPremium(PayrollLtLast);
                                    dTemp = Convert.ToDecimal(HI.Insured_Amount);
                                }
                                break;
                            case 3:
                                if (dTemp == 0)
                                {
                                    decimal.TryParse(e.Row.Cells[i - 1].Text, out dTemp);
                                    dTemp *= 4;
                                }
                                theB = dTemp;
                                break;
                            case 4:
                                SumC += dTemp;
                                break;
                            case 5:
                                dTemp = SumC;
                                break;
                            case 6:
                                if (dTemp == 0)
                                    dTemp = SumC - theB;
                                else
                                    dTemp += SumC;                               

                                if (dTemp < 0) dTemp = 0;
                                else if (ViewState["HI62UpperLimit"] != null)
                                {
                                    try
                                    {
                                        if (dTemp > decimal.Parse(ViewState["HI62UpperLimit"].ToString()))
                                            dTemp = decimal.Parse(ViewState["HI62UpperLimit"].ToString());
                                    }
                                    catch { }
 
                                }
                                break;
                            case 7:
                                decimal dTempC = 0;
                                decimal dTempE = 0;
                                decimal.TryParse(e.Row.Cells[4].Text, out dTempC);
                                decimal.TryParse(e.Row.Cells[6].Text, out dTempE);
                                dTemp = Math.Min(dTempC, dTempE);
                                break;
                            case 8:
                                decimal.TryParse(e.Row.Cells[i - 1].Text, out dTemp);
                                dTemp *= (decimal)0.02;
                                break;
                            default:
                                break;
                        }
                        e.Row.Cells[i].Text = dTemp.ToString("N0");
                    }
                    catch { }
                e.Row.Cells[9].Text = (e.Row.Cells[9].Text.Trim() == "N") ? "否" : "是";
                break;
        }
    }

    private void GetHI2()
    {
        string Ssql = "";
        Payroll py = new Payroll();
        string DeCodeKey = "dbo.BonusHI2Key";
        ViewState["DeCodeKey"] = DeCodeKey;
        py.BeforeQuery(DeCodeKey);

        Ssql = "SP_PRA_BonusHI2";
        SqlCommand sqlcmd = new SqlCommand();
        sqlcmd.Parameters.Add("@ls_PayrollTable", System.Data.SqlDbType.VarChar).Value = PayrollTable;
        sqlcmd.Parameters.Add("@ls_Company", System.Data.SqlDbType.VarChar).Value = SearchList1.CompanyValue.Trim();
        sqlcmd.Parameters.Add("@ls_EmployeeId", System.Data.SqlDbType.VarChar).Value = SearchList1.EmployeeValue.Trim().Replace("%", "");
        sqlcmd.Parameters.Add("@ls_CostYear", System.Data.SqlDbType.VarChar).Value = _UserInfo.SysSet.FormatADDate(TxDate.Text).Remove(4);
        sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = DeCodeKey;
        DataSet Ds = new DataSet();
        int iRet = _MyDBM.ExecStoredProcedure(Ssql, sqlcmd.Parameters, out Ds);

        if (Ds != null)
        {
            #region 處理結果
            if (Ds.Tables.Count > 1)
            {
                string LastDate = "";
                try
                {
                    LastDate = Ds.Tables[1].Rows[0][0].ToString();
                }
                catch
                {
                }
                if (LastDate == "") LastDate = DateTime.Now.ToString("yyyy/MM") + "/01";
                LastPayDate.Value = _UserInfo.SysSet.FormatDate(LastDate);
            }

            if (Ds.Tables.Count > 2)
            {
                ViewState["HI62Rate"] = "";
                ViewState["HI62UpperLimit"] = "";
                try
                {
                    ViewState["HI62Rate"] = ((Decimal)Ds.Tables[2].Rows[0][0] * 100).ToString("N0");
                    ViewState["HI62UpperLimit"] = Ds.Tables[2].Rows[0][1].ToString();
                }
                catch
                {
                    ViewState["HI62Rate"] = null;
                    ViewState["HI62UpperLimit"] = null;
                }
            }

            if (Ds.Tables.Count > 0)
            {
                GridView1.DataSource = Ds.Tables[0];
                GridView1.DataBind();
            }
            #endregion
        }
        py.AfterQuery(DeCodeKey);
    }

    protected bool setSatus(string Style)
    {
        bool blNew = false;
        switch (Style)
        {
            case "A":
                StyleTitle1.Title = "新增獎金發放";
                Style_page = Request["Style"];
                //預設值
                RB_standard.Checked = true;
                RB_standard_CheckedChanged(0, EventArgs.Empty);               
                btnAdd.ToolTip = "新增資料後繼續下一筆";
                btnUpdate.ToolTip = "新增後離開";
                break;
            case "U":
                StyleTitle1.Title = "獎金發放維護";
                Style_page = Request["Style"];
                SearchList1.Company.Enabled = false;
                SearchList1.Department.Enabled = false;
                SearchList1.Employee.Enabled = false;
                Money_name.Enabled = false;
                RowCount = Request["Row_Count"].ToString().Trim();
                //預設值
                RB_standard.Checked = true;
                RB_standard_CheckedChanged(0, EventArgs.Empty);                
                btnAdd.ToolTip = "更新資料";
                btnUpdate.ToolTip = "更新後離開";
                blNew = true;
                break;
            case "Q":
                StyleTitle1.Title = "獎金發放查詢";
                Style_page = Request["Style"];
                SearchList1.Company.Enabled = false;
                SearchList1.Department.Enabled = false;
                SearchList1.Employee.Enabled = false;
                Money_name.Enabled = false;
                RB_standard.Enabled = false;
                Tx_Amount.Enabled = false;
                RB_Person.Enabled = false;
                DP_Person.Enabled = false;
                Tx_multiple.Enabled = false;
                TxDate.Enabled = false;
                BankCode.Enabled = false;
                BankNumber.Enabled = false;
                ControlDown.Enabled = false;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                blNew = true;
                break;
        }
        return blNew;
    }

    // 搜尋模組連動
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        string Ssql = "";
        Ssql = "SELECT [Company] ,[EmployeeId] ,[DepositBank] ,[DepositBankAccount] ,[Period2DepositDate] ,[Period1DepositDate] " +
            " FROM [Payroll_Master_Heading] Where EmployeeId ='" + SearchList1.EmployeeValue.Trim() + "'";
        if (SearchList1.CompanyValue.Length > 0)
        {//公司
            Ssql += string.Format(" And Company='{0}'", SearchList1.CompanyValue.Trim());
        }
        if (SearchList1.DepartmentValue.Replace("%", "").Length > 0)
        {//部門
            Ssql += string.Format(" And EmployeeId in (select [EmployeeId] From [Personnel_Master] Where [DeptId]='{0}')", SearchList1.DepartmentValue.Trim());
        }
        DataTable dt = _MyDBM.ExecuteDataTable(Ssql);
        if (dt != null && dt.Rows.Count > 0)
        {
            try
            {
                BankCode.Text = dt.Rows[0]["DepositBank"].ToString();
            }
            catch { }
            try
            {
                BankNumber.Text = dt.Rows[0]["DepositBankAccount"].ToString();
            }
            catch { }
        }
        else
        {
            BankCode.Text = "";
            BankNumber.Text = "";
        }
    }

    #region 新增資料 btnAdd
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {//新增

        lbl_Msg.Text = "";
       
        //檢核判斷
        if (Chk_Data() != true)
        {
            return;
        }

        
            if (Request["Style"].ToString() == "A")
            {
                //檢查是否有新增過資料
                if (Check_Data(SearchList1.Company.Text) == true)
                {
                    //ShowMsgBox2.Message = msg;
                    lbl_Msg.Text = msg;
                    return;
                }

                if (RB_standard.Checked == true)
                {//定額選項
                    calculate_2();
                }
                else
                {
                    calculate();
                }

                DataNewAdd();
                lbl_Msg.Text = "新增完成";
            }
            else
            {
                if (RB_standard.Checked == true)
                {//定額選項
                    calculate_2();
                }
                else
                {
                    calculate();
                }
                DataUpdate();
                lbl_Msg.Text = "更新完成";
            }
            //重新取得補充保費計算表
            GetHI2(); 
    }
    #endregion

    #region 修改資料 btnUpdate
    protected void btnUpdate_Click(object sender, ImageClickEventArgs e)
    {//更新
        
      lbl_Msg.Text = "";
      StringBuilder str;
      //檢核判斷
      if (Chk_Data() != true)
      {
          return;
      }
      
        if (Request["Style"].ToString() != "U")
        {
            //檢查是否有新增過資料
            if (Check_Data(SearchList1.Company.Text) == true)
            {
                //ShowMsgBox2.Message = msg;
                lbl_Msg.Text = msg;
                return;
            }
            if (RB_standard.Checked == true)
            {//定額選項
                calculate_2();
            }
            else
            {
                calculate();
            }            
            DataNewAdd();
            lbl_Msg.Text = "新增完成";

            //新增後離開
          
            if (hid_InserMode.Value.Trim().Equals("EXIT"))
            {
                string strtemp = "window.opener.location='BonusMaster.aspx?Company=" + SearchList1.Company.Text + "';window.close();";
                ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "test", @strtemp, true);                 
            }

        }
        else
        {
            if (RB_standard.Checked == true)
            {//定額選項
                calculate_2();
            }
            else
            {
                calculate();
            }           
            DataUpdate();
            lbl_Msg.Text = "更新完成";
            //更新後離開            
            if (hid_InserMode.Value.Trim().Equals("EXIT"))
            {
                string strtemp = "window.opener.location='BonusMaster.aspx?Company=" + SearchList1.Company.Text + "';window.close();";
                ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "test", @strtemp, true);   
            }
        }
    }
    #endregion

    protected void btnCancel_Click(object sender, ImageClickEventArgs e)
    {//取消
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
    }
    protected void RB_standard_CheckedChanged(object sender, EventArgs e)
    {//定額選項
        if (RB_standard.Checked == true)
        {
            RB_Person.Checked = false;
            DP_Person.Enabled = false;
            Tx_multiple.Enabled = false;
            Tx_multiple.BackColor = System.Drawing.Color.Gray;
            DP_Person.BackColor = System.Drawing.Color.Gray;
            if (Request["Style"] != "Q")
            {
                Tx_Amount.Enabled = true;
                Tx_Amount.BackColor = System.Drawing.Color.White;
            }
            return;
        }
    }
    protected void RB_Person_CheckedChanged(object sender, EventArgs e)
    {//比例選項
        if (RB_Person.Checked==true)
        {
            RB_standard.Checked = false;
            Tx_Amount.Enabled = false;
            Tx_Amount.BackColor = System.Drawing.Color.Gray;
            if (Request["Style"] != "Q")
            {
              DP_Person.Enabled = true;
              DP_Person.BackColor = System.Drawing.Color.White;
              Tx_multiple.Enabled =true;
              Tx_multiple.BackColor = System.Drawing.Color.White;
              DP_Person_SelectedIndexChanged(0, EventArgs.Empty);
            }           
            //比例內容選項
           
        }
    }
    protected void Money_name_SelectedIndexChanged(object sender, EventArgs e)
    {//獎金名目
        string Ssql = "";
        DataTable DT2;
        if (Money_name.Text == "")
        { 
          Ssql = "Select CodeCode+'-'+CodeName as Code_Name From CodeDesc Where 1=1 and CodeID=(Select CodeId From CodeMaster Where CodeId='PY#Bonus')";
          DT2 = _MyDBM.ExecuteDataTable(Ssql);
          Money_name.Items.Add("請選擇");
          for (int i = 0; i < DT2.Rows.Count; i++)
           {
             Money_name.Items.Add(DT2.Rows[i]["Code_Name"].ToString());
           }
        }
    }
    protected void DP_Person_SelectedIndexChanged(object sender, EventArgs e)
    {//比例內容選項
        String SQL;
        DataTable DFT;
        SQL = "SELECT SalaryId,SalaryName,(SalaryId+'-'+SalaryName)as SalaryId_Name FROM SalaryStructure_Parameter Where 1=1 ";//SalaryId='01' AND PMTYPE='A' AND SalaryType='A'
        DFT = _MyDBM.ExecuteDataTable(SQL);
        DP_Person.Items.Clear();
        for (int i = 0; i < DFT.Rows.Count; i++)
        {
            string De_Name = DFT.Rows[i]["SalaryId_Name"].ToString();
            DP_Person.Items.Add(DFT.Rows[i]["SalaryId_Name"].ToString());
        }
        //DP_Person.Items.Add("本薪");
    }

    private Boolean Chk_Data()
    {//檢核判斷

       //檢查公司名稱      
        if (SearchList1.Company.Text == "")        
        {
            lbl_Msg.Text = "請選擇公司名稱";
            return false;
        }


        if (RB_Person.Checked==true)
        {
            if (Tx_multiple.Text == "")
            {
                lbl_Msg.Text = "請輸入倍數!";
                return false;
            }
        }

        if (RB_standard.Checked == true)
        {
            if (Tx_Amount.Text == "")
            {
                lbl_Msg.Text = "請輸入定額金額!";
                return false;
            }
        }

        if (RB_Person.Checked == true)
        {
            if (DP_Person.Text == "")
            {
                lbl_Msg.Text = "請選擇比例項目!";
                return false;
            }
        }

        if (RB_Person.Checked != true && RB_standard.Checked != true)
        {
            lbl_Msg.Text = "請輸入獎金金額選項!";
            return false;
        }


        if (Money_name.Text == "請選擇")
        {
            lbl_Msg.Text = "請選擇獎金名目!";
            return false;
        }       

        if (TxDate.Text == "")         
        {
            lbl_Msg.Text = "請選擇發放日期!";
            return false;
        }        
        else
        {
            int iTxDate = Convert.ToInt32(TxDate.Text.Replace("/", ""));
            string LastDate = LastPayDate.Value;
            if (LastPayDate.Value == "") LastDate = DateTime.Now.ToString("yyyy/MM") + "/01";
            LastPayDate.Value = _UserInfo.SysSet.FormatDate(LastDate);
            int iLastPayDate = Convert.ToInt32(LastPayDate.Value.Replace("/", ""));
            if (iTxDate <= iLastPayDate)
            {
                lbl_Msg.Text = "發放日期不可小於已確認獎金發放之日期(" + LastPayDate.Value + ")!";
                return false;
            }
            else
            {
                return true;
            }
        }


    }

    #region 比例選項 個人金額計算
    private void calculate ()
    {//個人金額計算
        String Sql = "";
        DataTable DTK;        
        Payroll thePayroll = new Payroll();
        double Personal = 0.0;
        Pamt = 0.0;
        Payroll.PayrolList PayrollLt = new Payroll.PayrolList();

        PayrollLt.Company = SearchList1.Company.Text;

        PayrollLt.EmployeeId = SearchList1.Employee.Text;
        PayrollLt.DeCodeKey = "dbo.Bonus" + DateTime.Now.ToString("yyMMddmmss");
        thePayroll.BeforeQuery(PayrollLt.DeCodeKey);
        if (SearchList1.Company.Text != "")
         {
            //找出個人薪資           
           try
             {
                 Personal += double.Parse(thePayroll.GetPersonalSalaryItem(PayrollLt, DP_Person.SelectedValue.ToString().Substring(0, DP_Person.SelectedValue.ToString().IndexOf("-")).TrimEnd()).ToString());
             }
            catch (Exception ex){

                string strERR = ex.Message;
            }
            
            //計算到職日至今

            Sql = "SELECT Case When Convert(varchar(4),Cast(HireDate as SmallDateTime),111)=Convert(varchar(4),getdate(),111) then Cast(Convert(decimal(9,2),Datediff([day],Cast(HireDate as SmallDateTime),getdate())/365.00)as varchar) ";
            Sql +="Else '1.0' end DayDiff ";
            Sql +=" FROM Personnel_Master WHERE COMPANY ='" + SearchList1.Company.Text + "' and EmployeeId='" + SearchList1.EmployeeValue.ToString() + "'";
 
            DTK = _MyDBM.ExecuteDataTable(Sql);

            if (DTK.Rows.Count != 0)
              {
                  //未含本薪的獎金金額
                  try
                  {
                      Total = (double)(Personal * (double.Parse(Tx_multiple.Text)) * (double.Parse(DTK.Rows[0]["DayDiff"].ToString())));//+ (double)Personal
                  }
                  catch { }

                  double PY6 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para6"));//稅率
                  double PY7 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para7"));//最低代扣稅額
                  double PY8 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para8"));//獎金應扣標準

                  //稅額計算與判斷
                  Pamt = PY6 * Total;
                  try
                  {
                      //當如下條件成立時,進行扣款:
                      //a.最低代扣稅額 > 0 且 稅額 >= 最低代扣稅額
                      //b.獎金應扣標準 > 0 且 獎金總金額 >= 獎金應扣標準
                      //c.最低代扣稅額 與 獎金應扣標準 同時=0
                      if ((PY7 > 0 && Pamt >= PY7) || (PY8 > 0 && PTotal >= PY8) || (PY7 == 0 && PY8 == 0))
                      {
                          //當員工為個人項目時,才顯示在畫面訊息!
                          if (SearchList1.EmployeeText != "全部")
                          {
                              lb_money.Text = "共合計：" + Math.Round(Total, MidpointRounding.AwayFromZero).ToString() + " 元,代扣繳稅額：" + Math.Round(PTamt, MidpointRounding.AwayFromZero).ToString() + "元";
                          }
                      }
                      else
                      {
                          Pamt = 0;
                          //當員工為個人項目時,才顯示在畫面訊息!
                          if (SearchList1.EmployeeText != "全部")
                          {
                              lb_money.Text = "共合計：" + Math.Round(Total, MidpointRounding.AwayFromZero).ToString() + "元,無代扣繳稅額!!";
                          }
                      }                   
                  }
                  catch { }      
              }
          }   
        thePayroll.AfterQuery(PayrollLt.DeCodeKey);
        return;

    }
    #endregion

    #region 針對全部人員計算代繳稅額
    private void calculate_3(string employy)
    {//個人金額計算
        String Sql = "";
        DataTable DTK;       
        Payroll thePayroll = new Payroll();
        double Personal = 0.0;
        Pamt = 0.0;
        Payroll.PayrolList PayrollLt = new Payroll.PayrolList();

        PayrollLt.Company = SearchList1.Company.Text;

        PayrollLt.EmployeeId = employy; //SearchList1.Employee.Text;
        PayrollLt.DeCodeKey = "dbo.Bonus" + DateTime.Now.ToString("yyMMddmmss");
        thePayroll.BeforeQuery(PayrollLt.DeCodeKey);
        if (SearchList1.Company.Text != "")
        {
            //找出個人薪資           
            try
            {
                Personal += double.Parse(thePayroll.GetPersonalSalaryItem(PayrollLt, DP_Person.SelectedValue.ToString().Substring(0, DP_Person.SelectedValue.ToString().IndexOf("-")).TrimEnd()).ToString());
            }
            catch (Exception ex)
            {

                string strERR = ex.Message;
            }

            //計算到職日至今

            Sql = "SELECT Case When Convert(varchar(4),Cast(HireDate as SmallDateTime),111)=Convert(varchar(4),getdate(),111) then Cast(Convert(decimal(9,2),Datediff([day],Cast(HireDate as SmallDateTime),getdate())/365.00)as varchar) ";
            Sql += "Else '1.0' end DayDiff ";
            Sql += " FROM Personnel_Master WHERE COMPANY ='" + SearchList1.Company.Text + "' and EmployeeId='" + employy + "'";

            DTK = _MyDBM.ExecuteDataTable(Sql);

            if (DTK.Rows.Count != 0)
            {
                //未含本薪的獎金金額
                try
                {
                    PTotal = (double)(Personal * (double.Parse(Tx_multiple.Text)) * (double.Parse(DTK.Rows[0]["DayDiff"].ToString())));
                }
                catch { }

                double PY6 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para6"));//稅率
                double PY7 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para7"));//最低代扣稅額
                double PY8 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para8"));//獎金應扣標準

                //稅額計算與判斷
                Pamt = PTotal * PY6; 

                
                try
                {

                    //當如下條件成立時,進行扣款:
                    //a.最低代扣稅額 > 0 且 稅額 >= 最低代扣稅額
                    //b.獎金應扣標準 > 0 且 獎金總金額 >= 獎金應扣標準
                    //c.最低代扣稅額 與 獎金應扣標準 同時=0
                    if ((PY7 > 0 && Pamt >= PY7) || (PY8 > 0 && PTotal >= PY8) || (PY7 == 0 && PY8 == 0))
                    {
                         //當員工為個人項目時,才顯示在畫面訊息!
                        if (SearchList1.EmployeeText != "全部")
                        {
                            lb_money.Text = "共合計：" + Math.Round(Total, MidpointRounding.AwayFromZero).ToString() + " 元,代扣繳稅額：" + Math.Round(PTamt, MidpointRounding.AwayFromZero).ToString() + "元";
                        }
                    }
                    else
                    {
                        Pamt = 0;
                         //當員工為個人項目時,才顯示在畫面訊息!
                        if (SearchList1.EmployeeText != "全部")
                        {
                            lb_money.Text = "共合計：" + Math.Round(Total, MidpointRounding.AwayFromZero).ToString() + "元,無代扣繳稅額!!";
                        }
                    }                    
                }
                catch { }
            }
        }
        thePayroll.AfterQuery(PayrollLt.DeCodeKey);
        return;

    }
    #endregion

    #region 定額選項 稅額計算
    private void calculate_2() 
    {
        //代扣繳稅額計算
        PTamt = 0.0;

        double PY6 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para6"));//稅率
        double PY7 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para7"));//最低代扣稅額
        double PY8 = double.Parse(DBSetting.GetPSParaValue(SearchList1.CompanyValue, "PY_Para8"));//獎金應扣標準

        long TxAmt = long.Parse(Tx_Amount.Text);

        //稅額
        PTamt = (double)(TxAmt * PY6);
        //當如下條件成立時,進行扣款:
        //a.最低代扣稅額 > 0 且 稅額 >= 最低代扣稅額
        //b.獎金應扣標準 > 0 且 獎金總金額 >= 獎金應扣標準
        //c.最低代扣稅額 與 獎金應扣標準 同時=0
        if ((PY7 > 0 && PTamt >= PY7)||(PY8 >0 && TxAmt >=PY8)||(PY7 ==0 && PY8==0))
        {
            if (SearchList1.EmployeeText != "全部")
            {
                lb_money.Text = "代扣繳稅額：" + Math.Round(PTamt, MidpointRounding.AwayFromZero).ToString() + "元";
            }
        }
        else 
        {
            PTamt = 0;
            if (SearchList1.EmployeeText != "全部")
            {
                lb_money.Text = "無代扣繳稅額!!";
            }
        }
        
    }
    #endregion

    private void Qurey() 
    {//查詢
        String SQL;
        DataTable DFT;
        SQL = "Select * From BonusMaster Where 1=1";
        SQL += " AND Company='" + Request["Company"].ToString() + "' ";
        SQL += " AND   DepId='" + Request["DepId"].ToString() + "' ";
        SQL += " AND EmployeeId='" + Request["EmployeeId"].ToString() + "' ";
        SQL += " AND CostId='" + Request["CTName"].Substring(0, Request["CTName"].IndexOf("-")) + "' ";
        SQL += " AND CostName='"+ Request["CTName"].Substring(Request["CTName"].IndexOf("-")+1) + "' ";
        SQL += " AND Cast(AmtDate as SmallDatetime)='" + _UserInfo.SysSet.FormatADDate(_UserInfo.SysSet.FormatDate(Request["AmtDate"].Substring(Request["AmtDate"].IndexOf("-") + 1))) + "'";

        DFT = _MyDBM.ExecuteDataTable(SQL);

        if (DFT.Rows.Count != 0)
        {
            //定額選項
            if (DFT.Rows[0]["Surely_Status"].ToString()=="1")
            {
                RB_standard.Checked = true;
                RB_standard_CheckedChanged(0, EventArgs.Empty);
                Tx_Amount.Text =_UserInfo.SysSet.rtnTrans(DFT.Rows[0]["CostAmt"].ToString());//T1
            }
            //比例選項
            if (DFT.Rows[0]["Balance_Status"].ToString()=="1")
            {
                RB_Person.Checked = true;
                RB_Person_CheckedChanged(0, EventArgs.Empty);
               
                lb_money.Text = "共合計：" + _UserInfo.SysSet.rtnTrans(DFT.Rows[0]["CostAmt"].ToString()) + " 元,代扣繳稅金額：" +_UserInfo.SysSet.rtnTrans(DFT.Rows[0]["Pay_AMT"].ToString())+" 元";

                string chek = _UserInfo.SysSet.rtnTrans(DFT.Rows[0]["Balance"].ToString()).ToString();

               
                 //if (Request.Form["Tx_multiple"] != chek && Request.Form["Tx_multiple"].ToString()!=null)
                 // {
                 //   Tx_multiple.Text = Request.Form["Tx_multiple"];                    
                 // }
            
                 //else 
                 // {
                    Tx_multiple.Text = double.Parse(_UserInfo.SysSet.rtnTrans(DFT.Rows[0]["Balance"].ToString()).ToString()).ToString();
                  //}
               
            
                DP_Person.Text = DFT.Rows[0]["BalanceItem"].ToString();
            }
            TxDate.Text =_UserInfo.SysSet.ToTWDate(DFT.Rows[0]["AmtDate"].ToString());
            BankCode.Text = DFT.Rows[0]["DepositBank"].ToString();
            BankNumber.Text = DFT.Rows[0]["DepositBankAccount"].ToString();

            if (DFT.Rows[0]["ControlDown"].ToString().Trim() == "Y")
            {
                ControlDown.SelectedIndex=0 ;
                setSatus("Q");
            }
            else 
            {
                ControlDown.SelectedIndex=1 ;                
            }
            
        }
      

        //寫入Log

        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "BonusMaster_U";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "Q";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = SearchList1.EmployeeText.ToString();//_UserInfo.UData.UserId
        _MyDBM.DataChgLog(MyCmd.Parameters);
        return;

    }

    #region 檢查是否有新增過資料
    private Boolean Check_Data(string Company) 
    {
        string SQL;      
        DataTable DFT;
        //檢查是否有新增過資料
        SQL = "Select Count(*) To_Count From BonusMaster A  Where A.Company='" + Company + "' and A.EmployeeId='" + SearchList1.Employee.SelectedValue + "' And A.CostYear='" + DateTime.Today.ToShortDateString().Substring(0, DateTime.Today.ToShortDateString().IndexOf("/")) + "' and A.CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "'";
        DFT = _MyDBM.ExecuteDataTable(SQL);
        if (Convert.ToInt32(DFT.Rows[0]["To_Count"].ToString()) != 0)
        {
            Date_Count = Convert.ToInt32(DFT.Rows[0]["To_Count"].ToString());
            msg = DateTime.Today.ToShortDateString().Substring(0, DateTime.Today.ToShortDateString().IndexOf("/")) + "年度" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + ",已有" + Convert.ToInt32(DFT.Rows[0]["To_Count"].ToString()) + "筆資料";
            return true;
        }
        else 
        {
            return false;
        } 
    }
    #endregion

    #region 整筆檢查是否有新增過資料
    private Boolean Check_Data_2(string Company, string EmployeeId) 
    {
        string SQL;
        DataTable DFT;
        //檢查是否有新增過資料
        string strAmtdate = _UserInfo.SysSet.FormatADDate(TxDate.Text);
        SQL = "Select Count(*) To_Count From BonusMaster A  Where A.Company='" + Company + "' and A.EmployeeId='" + EmployeeId + "' and A.AmtDate=Cast('" + strAmtdate + "'as SmallDateTime) and CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-")+1).Trim () + "'";
        DFT = _MyDBM.ExecuteDataTable(SQL);
        if (Convert.ToInt32(DFT.Rows[0]["To_Count"].ToString()) != 0)
        {
            return true;
        }
        else
        {
            return false;
        } 
    
    }
    #endregion

    #region 新增資料 DataNewAdd
    private void DataNewAdd()
    {
        String SQL;
        DataTable DFT;
        
        //新增資料

        if (!string.IsNullOrEmpty(SearchList1.Company.Text))
        {
            #region 當 (部門或員工任一值為"全部") 或 (部門或員工任一值皆不為"全部")
            
            if ((SearchList1.DepartmentText == "全部" || SearchList1.EmployeeText == "全部") || (SearchList1.DepartmentText != "全部" || SearchList1.EmployeeText != "全部"))
            {
                string ssql = "Select A.Company,DeptId,B.DepName,EmployeeId,EmployeeName  ";
                ssql += " From Personnel_Master A ";
                ssql += " inner join Department B ON A.Company=B.Company and A.DeptId=B.DepCode ";
                ssql += " Where A.Company='" + SearchList1.Company.Text + "' ";
                                
                if (SearchList1.DepartmentText != "全部") 
                {
                    ssql += " And A.DeptId='" + SearchList1.DepartmentText.Substring(0, SearchList1.DepartmentText.IndexOf("-")).Trim() + "'";
                }

                if (SearchList1.EmployeeText != "全部")
                {
                    ssql += " And A.EmployeeId='" + SearchList1.EmployeeText.Substring(0, SearchList1.EmployeeText.IndexOf("-")).Trim() + "'";
                }
                else
                {
                    //批次新增時,不應包含已離職人員
                    ssql += " And Upper(IsNull(ResignCode,'')) != 'Y' ";
                }

                DataTable DRP = _MyDBM.ExecuteDataTable(ssql);

                string strAmtdate = _UserInfo.SysSet.FormatADDate(TxDate.Text);
                //新增前判斷
                for (int j = 0; j < DRP.Rows.Count; j++)
                {
                    if (Check_Data_2(DRP.Rows[j]["Company"].ToString().Trim(), DRP.Rows[j]["EmployeeId"].ToString().Trim()) == true)
                    {
                        #region 更新資料

                        string SQL2 = "Select Row_Count From BonusMaster where 1=1 and Company='" + DRP.Rows[j]["Company"].ToString().Trim() + 
                            "'  and EmployeeId='" + DRP.Rows[j]["EmployeeId"].ToString().Trim() + "' and AmtDate='" + strAmtdate + 
                            "' and CostId='" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")).Trim() + "'";

                        DataTable DWR = _MyDBM.ExecuteDataTable(SQL2);

                        SQL = "Update BonusMaster";
                        SQL += "  SET Company='" + DRP.Rows[j]["Company"].ToString().Trim() + "', ";
                        SQL += "         DepId='" + DRP.Rows[j]["DeptId"].ToString().Trim() + "', ";
                        SQL += "   EmployeeId='" + DRP.Rows[j]["EmployeeId"].ToString().Trim() + "', ";
                        SQL += "  EmployeeName='" + DRP.Rows[j]["EmployeeName"].ToString().Trim() + "', ";
                        SQL += "       CostId='" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")) + "', ";
                        SQL += "     CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "', ";

                        //定額選項
                        if (RB_standard.Checked == true)
                        {
                            calculate_2();
                            string PMT = PTamt.ToString();
                            SQL += "      Surely_Status='1', ";
                            SQL += "     Balance_Status='0', ";
                            SQL += "           CostAmt='" + _UserInfo.SysSet.rtnHash(Tx_Amount.Text) + "', ";

                            SQL += "           Pay_AMT='" + _UserInfo.SysSet.rtnHash(PMT.ToString()) + "',";
                        }
                        else if (RB_Person.Checked == true)
                        {
                            //比例選項
                            SQL += "      Surely_Status='0', ";
                            SQL += "     Balance_Status='1', ";
                             //稅額與獎金計算
                            calculate_3(DRP.Rows[j]["EmployeeId"].ToString().Trim());
                            if (Total!=0)
                            {
                                SQL += "           CostAmt='" + _UserInfo.SysSet.rtnHash(Total.ToString()) + "', ";
                            }
                            else 
                            {
                                SQL += "           CostAmt='" + _UserInfo.SysSet.rtnHash(PTotal.ToString()) + "', ";
                            }
                            


                            SQL += "           Balance='" + _UserInfo.SysSet.rtnHash(Tx_multiple.Text) + "',";
                            SQL += "           Pay_AMT='" + _UserInfo.SysSet.rtnHash(Pamt.ToString()) + "',";
                            SQL += "       BalanceItem='" + DP_Person.Text + "',";
                        }

                        //獎金發放日期
                        SQL += "           AmtDate=Cast('" + strAmtdate + "' as SmallDateTime), ";

                        //發放年度
                        SQL += "           CostYear='" + strAmtdate.Remove(4) + "', ";

                        //銀行代碼
                        SQL += "           DepositBank='" + BankCode.Text + "', ";

                        //銀行帳號
                        SQL += "    DepositBankAccount='" + BankNumber.Text + "',";

                        //發放與否
                        SQL += "           ControlDown='" + ControlDown.Text + "' ";

                        SQL += " Where Company='" + SearchList1.Company.Text + "'";
                        SQL += "   And    Depid='" + DRP.Rows[j]["DeptId"].ToString().Trim() + "'";
                        SQL += "   And EmployeeId='" + DRP.Rows[j]["EmployeeId"].ToString().Trim() + "'";
                        SQL += "   And (CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "'OR CostId='" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")) + "')";
                        SQL += "    And Row_Count='" + DWR.Rows[0]["Row_Count"].ToString().Trim() + "'";

                        DFT = _MyDBM.ExecuteDataTable(SQL);
                        #endregion
                    }
                    else
                    {
                        #region 新增資料
                        SQL = "INSERT INTO BonusMaster(Row_Count,Company,DepId,DepName,EmployeeId,EmployeeName,CostId,CostName,CostYear,AmtDate,DepositBank,DepositBankAccount,ControlDown,CostAmt,Balance, BalanceItem,Balance_Status,Surely_Status,Pay_AMT) ";
                        SQL += " Select Distinct  ";
                        SQL += "  (Select case when IsNull(Max(Row_Count),0)+1 < 10  Then '0' + cast((IsNull(Max(Row_Count),0)+1)as varchar) ";
                        SQL += " else cast(IsNull(Max(Row_Count),0)+1 as varchar) end Rount ";
                        SQL += " From BonusMaster) Row_Count,";
                        SQL += " A.Company,";
                        SQL += "  '" + DRP.Rows[j]["DeptId"].ToString().Trim() + "' as DepId ,'" + DRP.Rows[j]["DepName"].ToString().Trim() + 
                            "' as DepName,'" + DRP.Rows[j]["EmployeeId"].ToString().Trim() + "' as EmployeeId,'" + DRP.Rows[j]["EmployeeName"].ToString().Trim() + 
                            "' as EmployeeName ,'" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")) + "' AS CostId ,'" +
                            Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "' AS CostName ,'" + strAmtdate.Remove(4) + "' as CostYear,CAST('" + strAmtdate + "' as SmallDateTime) as AmtDate,'" 
                            + BankCode.Text + "' as DepositBank,'" + BankNumber.Text + "' as DepositBankAccount,'" + ControlDown.Text + "' as ControlDown";

                        //定額選項
                        if (RB_standard.Checked == true)
                        {
                            calculate_2();
                            SQL += " ,'" + _UserInfo.SysSet.rtnHash(Tx_Amount.Text) + "' as CostAmt,'' as balance,'' as BalanceItem,'0' as Balance_Status,'1' as Surely_Status";

                            //代繳稅額處理(定額選項時)
                            string PMT = PTamt.ToString();
                            SQL += ",'" + _UserInfo.SysSet.rtnHash(PMT) + "' as Pay_AMT ";
                        }
                        else if (RB_Person.Checked == true)
                        {
                            //比例選項
                            //稅額與獎金計算
                            calculate_3(DRP.Rows[j]["EmployeeId"].ToString().Trim());

                            SQL += ",'" + _UserInfo.SysSet.rtnHash(PTotal.ToString()) + "' as CostAmt, '" + _UserInfo.SysSet.rtnHash(Tx_multiple.Text) + "' as Balance,'" + DP_Person.Text + "' as BalanceItem,'1' as Balance_Status, '0' as Surely_Status,'" + _UserInfo.SysSet.rtnHash(Pamt.ToString()) + "' as Pay_AMT ";

                        }

                        SQL += " From Personnel_Master A,Department B ";
                        SQL += " Where (A.Company='" + SearchList1.Company.Text + "' OR A.Company is Null OR A.Company='') ";

                        //部門選項
                        if ((SearchList1.DepartmentText != "全部"))
                        {
                            SQL += " And A.DeptId='" + SearchList1.DepartmentText.Substring(0, SearchList1.DepartmentText.IndexOf("-")) + "'";
                        }

                        //員工選項
                        if ((SearchList1.EmployeeText != "全部"))
                        {
                            SQL += " And A.EmployeeId='" + SearchList1.EmployeeText.Substring(0, SearchList1.EmployeeText.IndexOf("-")) + "'";
                        }
                        SQL += " And A.DeptId=B.DepCode ";

                        DFT = _MyDBM.ExecuteDataTable(SQL);
                        SQL = "";
                        #endregion
                    }
                    lbl_Msg.Text = "新增資料,正在進行...請稍候!!";
                }
            }
            #endregion
        }

        #region 寫入Log
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "BonusMaster_U";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "A";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = SearchList1.EmployeeText.ToString();//_UserInfo.UData.UserId
        _MyDBM.DataChgLog(MyCmd.Parameters);

        #endregion

        lbl_Msg.Text = "新增完成!!";
        return;    
    }
    #endregion

    #region 更新資料 DataUpdate
    private void DataUpdate() 
    {//更新
        String SQL;
        DataTable DFT;
        string strAmtdate = _UserInfo.SysSet.FormatADDate(TxDate.Text);
        #region 連續更新功能

        //if (!string.IsNullOrEmpty(SearchList1.Company.Text))
        //{
            #region 當 (部門或員工任一值為"全部") 或 (部門或員工任一值皆不為"全部")
            if ((SearchList1.DepartmentText == "全部" || SearchList1.EmployeeText == "全部") || (SearchList1.DepartmentText != "全部" || SearchList1.EmployeeText != "全部"))
            {
                string ssql = "Select DeptId,B.DepName,EmployeeId,EmployeeName  ";
                ssql += " From Personnel_Master A ";
                ssql += " inner join Department B ON A.Company=B.Company and A.DeptId=B.DepCode ";
                ssql += " Where A.Company='" + SearchList1.Company.Text + "' ";

                if (SearchList1.DepartmentText != "全部")
                {
                    ssql += " And A.DeptId='" + SearchList1.DepartmentText.Substring(0, SearchList1.DepartmentText.IndexOf("-")).Trim() + "'";
                }

                if (SearchList1.EmployeeText != "全部")
                {
                    ssql += " And A.EmployeeId='" + SearchList1.EmployeeText.Substring(0, SearchList1.EmployeeText.IndexOf("-")).Trim() + "'";
                }

                DataTable DRP = _MyDBM.ExecuteDataTable(ssql);

                for (int i = 0; i < DRP.Rows.Count; i++)
                {
                             
                }
            }
            #endregion
            //}
        #endregion

            SQL = "Update BonusMaster";
            SQL += "  SET Company='" + SearchList1.Company.Text + "', ";
            SQL += "         DepId='" + SearchList1.Department.Text + "', ";
            SQL += "   EmployeeId='" + SearchList1.EmployeeText.Substring(0, SearchList1.EmployeeText.IndexOf("-")) + "', ";
            SQL += "  EmployeeName='" + SearchList1.EmployeeText.Substring(SearchList1.EmployeeText.IndexOf("-") + 1) + "', ";
            SQL += "       CostId='" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")) + "', ";
            SQL += "     CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "', ";

            //定額選項
            if (RB_standard.Checked == true)
            {
                calculate_2();
                string PMT = PTamt.ToString();
                SQL += "      Surely_Status='1', ";
                SQL += "     Balance_Status='0', ";
                SQL += "           CostAmt='" + _UserInfo.SysSet.rtnHash(Tx_Amount.Text) + "', ";
                SQL += "           Pay_AMT='" + _UserInfo.SysSet.rtnHash(PMT.ToString()) + "',";
            }
            else if (RB_Person.Checked == true)
            {
                //比例選項
                //稅額與獎金計算
                calculate();//DRP.Rows[i]["EmployeeId"].ToString().Trim()
                SQL += "      Surely_Status='0', ";
                SQL += "     Balance_Status='1', ";
                SQL += "           CostAmt='" + _UserInfo.SysSet.rtnHash(Total.ToString()) + "', ";
                SQL += "           Balance='" + _UserInfo.SysSet.rtnHash(Tx_multiple.Text) + "',";
                SQL += "           Pay_AMT='" + _UserInfo.SysSet.rtnHash(Pamt.ToString()) + "',";
                SQL += "       BalanceItem='" + DP_Person.Text + "',";
            }

            //獎金發放日期
            SQL += "           AmtDate=Cast('" + strAmtdate + "' as SmallDateTime), ";

            //發放年度
            SQL += "           CostYear='" + strAmtdate.Remove(4) + "', ";

            //銀行代碼
            SQL += "           DepositBank='" + BankCode.Text + "', ";

            //銀行帳號
            SQL += "    DepositBankAccount='" + BankNumber.Text + "',";

            //發放與否
            SQL += "           ControlDown='" + ControlDown.Text + "' ";

            SQL += " Where Company='" + SearchList1.Company.Text + "'";
            SQL += "   And    Depid='" + SearchList1.Department.Text + "'";
            SQL += "   And EmployeeId='" + SearchList1.EmployeeText.Substring(0, SearchList1.EmployeeText.IndexOf("-")) + "'";
            SQL += "   And (CostName='" + Money_name.Text.Substring(Money_name.Text.IndexOf("-") + 1) + "'OR CostId='" + Money_name.Text.Substring(0, Money_name.Text.IndexOf("-")) + "')";
           
            if (Request["Style"] == "U")
            {
                RowCount = Request["Row_Count"].ToString().Trim();
                SQL += "    And Row_Count='" + RowCount + "'";
            }          

            DFT = _MyDBM.ExecuteDataTable(SQL);       

       #region 寫入Log
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "BonusMaster_U";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = SearchList1.EmployeeText.ToString();//_UserInfo.UData.UserId
        _MyDBM.DataChgLog(MyCmd.Parameters);
       #endregion

        return;

    }
    #endregion 

    protected void Tx_multiple_TextChanged(object sender, EventArgs e)
    {
        //AutoPostBack=trun 計算
        if (RB_standard.Checked == true)
        {//定額選項
            calculate_2();
        }
        else if (RB_Person.Checked == true)  
        {
            calculate();
        }    
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //即時按鈕試算
        if (RB_standard.Checked == true)
        {//定額選項
            calculate_2();
        }
        else if (RB_Person.Checked == true)        
        {
            calculate();
        }
    }
}
