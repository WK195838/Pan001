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

public partial class Basic_IncomeTaxQuery : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM041";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    decimal NWAmount = 0, TaxAmount = 0;
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

        if (!Page.IsPostBack || Request.Form[0].Contains(SearchList1.Company.UniqueID))
        {//只有在頁面初始化及公司改變時,才需要重設年月
            SalaryYM1.SetSalaryYM(SearchList1.CompanyValue.Trim());//月份
            SalaryYM2.SetSalaryYM(SearchList1.CompanyValue.Trim());//月份
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
            Navigator1.Visible = false;
            SetResignCode();
            btnToExcel.Attributes.Add("onclick", "alert('請先選擇查詢條件!');return false;");
        }
        else
        {
            if (string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                lbl_Msg.Text = "請選擇先公司!";
            }
            else
            {
                BindData();
            }
        }
        
        //下拉式選單連動設定
        //SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'Payroll' And RTrim(ProgramPath)='Main/IncomeTaxQuery.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                        e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;
                    
                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                try
                {
                    if (DataBinder.Eval(e.Row.DataItem, "OtherNWSalary") != null)
                        NWAmount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OtherNWSalary"));
                    if (DataBinder.Eval(e.Row.DataItem, "SalaryAmount") != null)
                        TaxAmount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SalaryAmount"));
                }
                catch { }
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "當前總計";
                e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Underline = true;
                e.Row.Cells[3].Text = NWAmount.ToString("n0");
                e.Row.Cells[3].Font.Bold = true;
                e.Row.Cells[3].Font.Underline = true;
                e.Row.Cells[4].Text = TaxAmount.ToString("n0");
                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[4].Font.Underline = true;
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
        NWAmount = 0;
        TaxAmount = 0;
       // Ssql = "SELECT * FROM Personnel_Master Where Company='" + CompanyList1.SelectValue.Trim() + "'";

        if (string.IsNullOrEmpty(SearchList1.CompanyValue)) return;

        Payroll py = new Payroll();
        Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
        PayrollLt.DeCodeKey = "dbo.IncomeTaxQuery" + DateTime.Now.ToString("yyyyMMddmmss");
        py.BeforeQuery(PayrollLt.DeCodeKey);
        try
        {
            //2012/11/12 依財務Ivy要求除【應稅所得】外，需加計【其它應稅調整】及【應稅加班費】
            Ssql = "SELECT Company,EmployeeId,SalaryYM " +
                " ,SUM(Case When SalaryItem='03' Then " + PayrollLt.DeCodeKey + "(PWD.SalaryAmount) else 0 end) " +
                " As PWDTax " +
                " ,SUM(Case When SP.NWTax='Y' Then Case PMType  " +
                "  When 'A' " +
                "  then IsNull(" + PayrollLt.DeCodeKey + "  (PWD.SalaryAmount),0) " +
                "  else IsNull(" + PayrollLt.DeCodeKey + "  (PWD.SalaryAmount),0)* -1 end " +
                "  else 0 end) As PWDNWAmount " +
                " FROM Payroll_History_Detail  PWD " +
                " Left Join SalaryStructure_Parameter SP " +
                " On PWD.SalaryItem = SP.SalaryId " +
                " Where Company='{Company}' And EmployeeId Like '{Employee}' And PWD.SalaryYM Between '" + SalaryYM1.SelectSalaryYM + "' and '" + SalaryYM2.SelectSalaryYM + "'" +
                //And NWTax='" + NTorWT + "' And (SalaryId > '" + strSQL + "' Or SalaryId in ('08','09'))
                //" Where Company='{Company}' And PWD.SalaryYM Like '{Year}'" +
                " Group by Company,EmployeeId,SalaryYM";

            //2011/10/12依財務Ivy要求加入獎金發放之金額及其代扣稅額
            Ssql = " Select PWD.Company" +
                " ,RTRIM( convert(char, convert(decimal,left(PWD.SalaryYM,4))- 1911) )+ '/' + right(PWD.SalaryYM,2) AS SalaryYM" +
                " ,DeptId" +
                " ,IsNull((SELECT Top 1 DepName from Department Where Company=PWD.Company and DepCode=PM.DeptId),'') DepName" +
                " ,PWD.EmployeeId" +
                " ,EmployeeName" +
                " ,(Case When IsNull(ResignCode,'N')='N'then '' else ResignCode end) as ResignCode" +
                " ,(PWDNWAmount+IsNull(CostAmt,0)) As OtherNWSalary " +
                " ,(PWDTax+IsNull(Tax,0)) As SalaryAmount " +
                " From(" + Ssql + " ) PWD " +
                " Left Join Personnel_Master PM on PWD.Company=PM.Company And PWD.EmployeeId=PM.EmployeeId" +
                " Left Join (Select Company,EmployeeId,Substring(Convert(varchar,AmtDate,112),1,6) As SalaryYM " +
                " ,Sum(IsNull(" + PayrollLt.DeCodeKey + "(Pay_AMT),0)) As Tax " +
                " ,Sum(IsNull(" + PayrollLt.DeCodeKey + "(CostAmt),0)) As CostAmt " +
                " from BonusMaster where CostYear Between '" + SalaryYM1.SelectSalaryYM.Remove(4) + "' and '" + SalaryYM2.SelectSalaryYM.Remove(4) + "' and Convert(varchar,AmtDate,112) Between '" + SalaryYM1.SelectSalaryYM + "01' and '" + SalaryYM2.SelectSalaryYM + "31' " +
                " and Del_Mark is null Group By Company,EmployeeId,CostYear,Substring(Convert(varchar,AmtDate,112),1,6) ) BM " +
                " On BM.Company = PWD.Company And BM.EmployeeId = PWD.EmployeeId " +
                " And PWD.SalaryYM=BM.SalaryYM ";

            string ShowResignC = "";
            if (cbResignC.Items.Count > 0)
            {//2011/10/12 依人事要求加入離職篩選條件
                string strTL = "";
                foreach (ListItem lis in cbResignC.Items)
                {
                    if (lis.Selected)
                    {
                        strTL += "'" + lis.Value + "',";
                        ShowResignC += "（Ｖ）" + lis.Text + "　";
                    }
                }
                if (strTL.Length > 0)
                {
                    Ssql += " Where ResignCode in (" + strTL + "'-')";
                }
            }

            //公司
            if (string.IsNullOrEmpty(SearchList1.CompanyValue) == false) Ssql = Ssql.Replace("{Company}", SearchList1.CompanyValue);

            //員工
            if (SearchList1.EmployeeValue.ToUpper() == "ALL")
                Ssql = Ssql.Replace("{Employee}", "%");
            else if (string.IsNullOrEmpty(SearchList1.EmployeeValue) == false)
                Ssql = Ssql.Replace("{Employee}", SearchList1.EmployeeValue);

            //部門
            if (SearchList1.DepartmentValue.Replace("%","") != "")
                Ssql += " And DeptId = '" + SearchList1.DepartmentValue.Trim() + "'";

            //年份
            Ssql += " And PWD.SalaryYM Between '" + SalaryYM1.SelectSalaryYM + "' and '" + SalaryYM2.SelectSalaryYM + "'  ";
            

            //SDS_GridView.SelectCommand = Ssql;
            Ssql += " Order by PWD.EmployeeId, PWD.SalaryYM ";
          
            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);
            #region 更新匯出按鈕用資料            
            //"資料報表標題", "條件名稱1,條件內容1,條件名稱2,條件內容2...", "資料欄位名稱,若未傳入則使用DB中的欄位描述,(必須是單一資料表才找得出);各欄位指定型態,若無分號則以文字格式顯示", "SELECT * FROM HOLIDAY"
            string strHeaderLine, strQueryLine, strDataTitel, strDataBody;
            strHeaderLine = "'所得稅扣繳資料彙整'";
            strQueryLine = "'公司：," + SearchList1.CompanyText + "";
            if (SearchList1.Department.SelectedIndex > -1)
                strQueryLine += ",部門：," + SearchList1.DepartmentText + "";
            else
                strQueryLine += ",部門：,全部";
            if (SearchList1.Employee.SelectedIndex > -1)
                strQueryLine += ",員工：," + SearchList1.EmployeeText + "";
            else
                strQueryLine += ",員工：,全部";
            strQueryLine += ",年度：," + SalaryYM1.SelectSalaryYMName + " ～ " + SalaryYM2.SelectSalaryYMName + "";
            strQueryLine += ",是否在職：," + ShowResignC + "'";
            strDataTitel = "''";
            if (theDT != null)
            {
                Ssql = "IncomeTaxQuery" + SalaryYM1.SelectSalaryYM + "_" + SalaryYM2.SelectSalaryYM;
                Session[Ssql] = theDT;
                strDataTitel = "'月份,部門代號,部門名稱,員工代號,員工姓名,應稅所得,扣繳金額;,S,S,S,S,S,,f0,f0'";
            }
            else
            {
                Ssql = "";
            }

            if (Ssql != "")
            {
                strDataBody = "'" + _UserInfo.SysSet.rtnHash(Ssql).Replace('\\', '＼') + "'";
                btnToExcel.Attributes.Add("onclick", "return ExportExcel(" + strHeaderLine + "," + strQueryLine + "," + strDataTitel + "," + strDataBody + ");return false;");
            }
            else
            {
                btnToExcel.Attributes.Add("onclick", "alert('請先選擇查詢條件!');return false;");
            }
            #endregion

            GridView1.Visible = false;
            Navigator1.Visible = false;
            lbl_Msg.Text = "";
            if (theDT.Rows.Count > 0)
            {
                GridView1.DataSource = theDT;
                GridView1.DataBind();
                GridView1.Visible = true;

                //if (GridView1.PageCount > 1)
                {
                    Navigator1.BindGridView = GridView1;
                    Navigator1.DataBind();
                    Navigator1.Visible = true;
                }
            }
            else
            {
                lbl_Msg.Text = "查無資料!!";
            }
        }
        catch { }
        py.AfterQuery(PayrollLt.DeCodeKey);
         
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
                if (i < 3)
                    e.Row.Cells[i].Style.Add("text-align", "left");
                else
                    e.Row.Cells[i].Style.Add("text-align", "right");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[i].CssClass = "Grid_GridLine";
                if (i < 3)
                    e.Row.Cells[i].Style.Add("text-align", "left");
                else
                    e.Row.Cells[i].Style.Add("text-align", "right");
            }
            GridView1.ShowFooter = true;
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    //void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(SearchList1.CompanyValue))
    //    {
    //        Navigator1.Visible = true;
    //        GridView1.Visible = true;
    //        BindData();
    //    }
    //}

    //private void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(CompanyList1.SelectValue))
    //    {
    //        Navigator1.Visible = true;
    //        GridView1.Visible = true;
    //        BindData();
    //    }

    //}

    //protected void EmployeeIdList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindData();
    //}

    /// <summary>
    /// 設定是含離職核取方塊:2011/09/16 依人事要求加入離職篩選條件
    /// </summary>
    private void SetResignCode()
    {
        cbResignC.Items.Clear();
        Ssql = "select [CodeCode],[CodeName] from CodeDesc Where CodeID = 'PY#ResignC'";
        DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        ListItem theItem = new ListItem();
        for (int i = 0; i < DT.Rows.Count; i++)
        {
            theItem = new ListItem();
            theItem.Value = DT.Rows[i]["CodeCode"].ToString();
            theItem.Text = DT.Rows[i]["CodeName"].ToString();
            if (!DT.Rows[i]["CodeCode"].ToString().Equals("Y"))
                theItem.Selected = true;
            cbResignC.Items.Add(theItem);
        }
    }
}
