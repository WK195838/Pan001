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

public partial class PYRBonus : System.Web.UI.Page
{
    string Ssql = "";
    string SqlA = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR011";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    //ASP.usercontrol_yearlist_ascx.SelectedIndexChanged Dyear = new UserControl_YearList.SelectedIndexChanged();
    
    public static string AmtDate = string.Empty;  
  
    public static double[] TK_A = new double [20];
    public static double[] TK_B = new double[20];
    public static double[] TK_C = new double[20];
    public static double[] TK_D = new double[20];


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
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/BonusMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    //private void AuthRight()
    //{
    //    //驗證權限
    //    bool Find = false;
    //    bool SetCss = false;
    //    int i = 0;

    //    string[] Auth = { "Delete", "Modify", "Detail", "Add" };

    //    if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
    //    {
    //        for (i = 0; i < Auth.Length; i++)
    //        {
    //            Find = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, Auth[i]);
    //            if (i < (Auth.Length - 1))
    //            {//刪/修/詳
    //                GridView1.Columns[i].Visible = Find;
    //                //設定標題樣式
    //                if (Find && (SetCss == false))
    //                {
    //                    SetCss = true;
    //                    GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
    //                }
    //            }
    //            else
    //            {//新增
    //                //btnNew.Visible = Find;
    //                btnEmptyNew.Visible = Find;
    //            }
    //        }

    //        //查詢(執行)
    //        if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
    //        {
    //            Find = true;
    //        }
    //        else
    //        {
    //            Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
    //        }

    //        //版面樣式調整
    //        if (SetCss == false)
    //        {
    //            GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
    //        }
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息   

        PRT_Cor.Attributes.Add("onClick", "drawWait('')");

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
                YearList1.Enabled =false;
                MonthList1.Enabled = false;
            }
            //年月處理
            YearList1.initList();
            MonthList1.initList();

            //設定公司
            SearchList1.CompanyValue = _UserInfo.UData.Company;        

            //獎金名目
            CostId_SelectedIndexChanged(0, EventArgs.Empty);
            RB_Sel1.Checked = true;
            BindData();
            //showPanel();
            //AuthRight();
            ReportViewer1.Visible = false;
            
        }
        else
        {
            //再查詢先清除,查無資料訊息
            //Panel_Empty.Visible = false;

           
            //獎金名目
            //CostId_SelectedIndexChanged(0, EventArgs.Empty);
           
            //讀取資料
            BindData();  
        }

       

        if (SearchList1.CompanyValue == "")
        {
            //btnNew.Visible = false;
            //btnNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        }
        else
        {
            //btnNew.Visible = true;
            //btnNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Company=" + SearchList1.CompanyValue.ToString() + "&DepId=" + SearchList1.DepartmentValue.ToString() + "&EmployeeId=" + SearchList1.EmployeeValue.ToString() + "&Style=A','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('BonusMaster_U.aspx?Company=" + SearchList1.CompanyValue.ToString() + "&DepId=" + SearchList1.DepartmentValue.ToString() + "&EmployeeId=" + SearchList1.EmployeeValue.ToString() + "&Style=A','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        }

        ScriptManager1.RegisterPostBackControl(PRT_Cor);       
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
                if (ReportViewer1.Visible == true)
                {
                    Panel_Empty.Visible = false;
                    btnEmptyNew.Visible = false;
                }
                else 
                {
                    Panel_Empty.Visible = true;
                    btnEmptyNew.Visible = false;
                }
                
            }
        }
    }

    private bool hasData()
    {
        //年月處理
        AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString() + "/01";
        AmtDate = _UserInfo.SysSet.FormatADDate(AmtDate).ToString();
        AmtDate = AmtDate.Substring(0, AmtDate.IndexOf("/") + 3);

        Ssql = "Select A.Row_Count, A.Company,(A.DepId+'-'+A.DepName) DepName,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId,(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount,Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown,ISNULL(A.Pay_AMT,0)as Pay_AMT,CASE ISNULL(HI2,'0.0') WHEN '' THEN '0.0' ELSE ISNULL(HI2,'0.0') END AS HI2 ";

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
        string CTid;

        if (CostId.Text == "全部")
        {
             CTid = "";
        }
        else
        {
            if (CostId.Text != "")
            {
                 CTid = CostId.Text.Substring(0, CostId.Text.IndexOf("-")).ToString();
                 DDLSr("A.CostId", CTid);
            }
        }

        //判斷刪除駐記
        DDLSr("A.Del_Mark", "");

        Ssql += "ORDER By DepId,EmployeeId";


        DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

        if (tb.Rows.Count > 0)
        {      
            Panel_Empty.Visible =false;
            return true;
        }
        else
        {
            Panel_Empty.Visible = true;
            btnEmptyNew.Visible = false;
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    { 
        string strPan = "&nbsp;&nbsp;";
        decimal testDecimal = 0;
        
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                        //查詢時
                        //調整欄位 
                        e.Row.Cells[3].Style.Add("text-align", "right");
                        e.Row.Cells[4].Style.Add("text-align", "right");
                        e.Row.Cells[5].Style.Add("text-align", "right");
                        e.Row.Cells[6].Style.Add("text-align", "right");
                        //發放年月  
                        e.Row.Cells[7].Text = _UserInfo.SysSet.ToTWDate(e.Row.Cells[7].Text);
                        e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, e.Row.Cells[7].Text.IndexOf("/")) + "年" + e.Row.Cells[7].Text.Substring(e.Row.Cells[7].Text.IndexOf("/") + 1,2) + "月";
                       
                        if (DBSetting.PersonalName(e.Row.Cells[3].Text, e.Row.Cells[4].Text.Trim()) != "")                        
                        {
                            e.Row.Cells[3].Text = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(e.Row.Cells[3].Text)).ToString("N2").Replace(".00", "");//獎金金額解密
                            if (e.Row.Cells[3].Text == "")
                            {
                                e.Row.Cells[3].Text = "0.0";
                            }
                            e.Row.Cells[4].Text = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(e.Row.Cells[4].Text)).ToString("N2").Replace(".00", "");//代扣繳金額
                            if (e.Row.Cells[4].Text == "")
                            {
                                e.Row.Cells[4].Text = "0.0";
                            }
                            if(e.Row.Cells[5].Text != "0.0" && !decimal.TryParse(e.Row.Cells[5].Text, out testDecimal))
                            {
                                e.Row.Cells[5].Text = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(e.Row.Cells[5].Text)).ToString("N2").Replace(".00", "");//補充保費
                            }
                            if (e.Row.Cells[5].Text == "")
                            {
                                e.Row.Cells[5].Text = "0.0";
                            }
                            e.Row.Cells[6].Text = Convert.ToDecimal((double.Parse(e.Row.Cells[3].Text) - double.Parse(e.Row.Cells[4].Text) - double.Parse(e.Row.Cells[5].Text))).ToString("N2").Replace(".00", "");//實計發放金額                           
                        }
                        //加總金額
                        for (int i = 3; i < e.Row.Cells.Count; i++)
                        {
                            TK_A[i] += double.Parse(e.Row.Cells[3].Text);
                            TK_B[i] += double.Parse(e.Row.Cells[4].Text);
                            TK_C[i] += double.Parse(e.Row.Cells[5].Text);
                            TK_D[i] += double.Parse(e.Row.Cells[6].Text);
                        }
                    break;
                    
                case DataControlRowType.Header:
                    for (int i = 3; i < e.Row.Cells.Count; i++)
                      {
                        TK_A[i] = 0.0;
                        TK_B[i] = 0.0;
                        TK_C[i] = 0.0;
                        TK_D[i] = 0.0;                       
                      }                
                    GridView1.ShowFooter = true;
                    break;
                case DataControlRowType.Footer:
                    e.Row.Cells[2].Text = strPan + "小計" + strPan;
                    for (int i = 3; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Style.Add("text-align", "right");
                        try
                        {
                            e.Row.Cells[3].Text = Convert.ToDecimal(TK_A[i].ToString()).ToString("N2").Replace(".00", "");
                            e.Row.Cells[4].Text = Convert.ToDecimal(TK_B[i].ToString()).ToString("N2").Replace(".00", "");
                            e.Row.Cells[5].Text = Convert.ToDecimal(TK_C[i].ToString()).ToString("N2").Replace(".00", "");
                            e.Row.Cells[6].Text = Convert.ToDecimal(TK_D[i].ToString()).ToString("N2").Replace(".00", "");
                        }                        
                        catch { }                        
                    }
                   // e.Row.Cells[3].Text = TK.ToString();
                    break;
            }
            
    }

    private void BindData()
    {//讀取資料

        //年月處理
        AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString()+ "/01";
        AmtDate = _UserInfo.SysSet.FormatADDate(AmtDate).ToString();
        AmtDate = AmtDate.Substring(0, AmtDate.IndexOf("/") + 3);
        string lastYM = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
        Ssql = "Select A.Row_Count,A.Company,(A.DepId+'-'+A.DepName) DepName,(Rtrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId,(A.CostId+'-'+A.CostName) as CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount,Case When (A.ControlDown is null Or A.ConTrolDown='N' Or A.ConTrolDown='') then 'N-否' Else 'Y-是' End ConTrolDown,ISNULL(A.Pay_AMT,0)as Pay_AMT,Case When A.HI2 Is null then dbo.getHI2(dbo.DecodeAmount(CostAmt),dbo.DecodeAmount(Insured_amount)) else dbo.DecodeAmount(A.HI2) End as HI2 ";
        Ssql += "  From BonusMaster A Left Join [HealthInsurance_Heading] B On A.EmployeeId=B.EmployeeId  Where 1=1 ";   

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
        string CTid;

        if (CostId.Text == "全部")
        {
             CTid = "";
        }
        else
        {
            if (CostId.Text != "")
            {
                 CTid = CostId.Text.Substring(0, CostId.Text.IndexOf("-")).ToString();
                 DDLSr("A.CostId", CTid);
            }
        }      
 
        //判斷刪除駐記
        DDLSr("A.Del_Mark","");

        Ssql += " ORDER By DepId,EmployeeId";
        
        SDS_GridView.SelectCommand = Ssql;
        
        GridView1.DataBind();

        if (GridView1.Rows.Count > 0)
        {
            Panel_Empty.Visible =false;
        }
        else
        {
            if (ReportViewer1.Visible == true)
            {
                Panel_Empty.Visible = false;
                btnEmptyNew.Visible = false;
            }
            else 
            {
                Panel_Empty.Visible = true;
                btnEmptyNew.Visible = false;
            }           
        }
        Navigator1.BindGridView = GridView1;
        Navigator1.DataBind();        
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //報表關閉
        Navigator1.Visible = true;
        GridView1.Visible = true;
        ReportViewer1.Visible = false;

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
                if (i == 6)
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
    }

    protected void CostId_SelectedIndexChanged(object sender, EventArgs e)
    {
        //獎金名目
        string Ssql = "";
        DataTable DT2;
        if (CostId.Text == "")
        {
            Ssql = "Select CodeCode+'-'+CodeName as Code_Name From CodeDesc Where 1=1 and CodeID=(Select CodeId From CodeMaster Where CodeId='PY#Bonus')";
            DT2 = _MyDBM.ExecuteDataTable(Ssql);
            CostId.Items.Add("全部");
            for (int i = 0; i < DT2.Rows.Count; i++)
            {
                CostId.Items.Add(DT2.Rows[i]["Code_Name"].ToString());
            }
        }
        BindData();
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

    protected void PRT_Cor_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SearchList1.Company.SelectedValue))
        {
            lbl_Msg.Text = "請選擇公司名稱";
        }
        else 
        {

            // 清除之前的資料
            ReportViewer1.LocalReport.DataSources.Clear();
            Navigator1.Visible = false;
            GridView1.Visible = false;

            decimal testDecimal = 0;

            bool rebind = false;
            string AmtDate;
            string Company_D="";
            Company_D = DBSetting.CompanyName(SearchList1.CompanyValue);
            DataTable Table1 = new DataTable();
            if (Table1 == null)
                rebind = true;
            else if (Table1.Rows.Count == 0)
                rebind = true;
            if (rebind.Equals(true))
            {
                BindData();                
                Table1 = _MyDBM.ExecuteDataTable(Ssql);
                if (Table1.Rows.Count != 0)
                {
                    for (int i = 0; i < Table1.Rows.Count; i++)
                    {
                        Table1.Rows[i]["CostAmt"] = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(Table1.Rows[i]["CostAmt"].ToString())).ToString("N2").Replace(".00", "");
                        Table1.Rows[i]["Pay_AMT"] = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(Table1.Rows[i]["Pay_AMT"].ToString())).ToString("N2").Replace(".00", "");
                        if (Table1.Rows[i]["HI2"].ToString() != "0.0" && !decimal.TryParse(Table1.Rows[i]["HI2"].ToString(), out testDecimal))
                        {
                            Table1.Rows[i]["HI2"] = Convert.ToDecimal(_UserInfo.SysSet.rtnTrans(Table1.Rows[i]["HI2"].ToString())).ToString("N2").Replace(".00", "");//補充保費
                        }
                        if (Table1.Rows[i]["HI2"].ToString() == "")
                        {
                            Table1.Rows[i]["HI2"] = "0.0";
                        }
                    }
                    ReportViewer1.LocalReport.ReportPath = @"RDLC\BounsRpt.rdlc";
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("BounsMaster", Table1));
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", Company_D));
                    ReportViewer1.Visible = true;
                }
                else
                {
                    ReportViewer1.Visible = false;
                    Panel_Empty.Visible = true;
                    btnEmptyNew.Visible = false;
                }                 
            }               
        }
    }   
}