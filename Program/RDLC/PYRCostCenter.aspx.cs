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

public partial class PYRCostCenter : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYR012";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    String sCompany,sEmployeeId;
    public static string AmtDate = string.Empty;


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
    //                //btnEmptyNew.Visible = Find;
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
       
        //BounsDate.CssClass = "JQCalendar";     
        Sel_DepId.Attributes.Add("onClick", "drawWait('')");
        lbl_Msg.Text = "";//清空訊息

        //下拉式選單連動設定    
        Navigator1.BindGridView = GridView1;
        //SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
        //Ssql = "SELECT Company,CompanyShortName FROM Company_Master Where CompanyShortName='" + _UserInfo.UData.Company + "'";

        //DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        //tbCompany.Text = DT.Rows[0][0].ToString() + DT.Rows[0][1].ToString();
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

            //if (sCompany != null)
            //{
            //    SearchList1.CompanyValue = sCompany;
            //}

            //年月處理
            //YearList1.SetYearList(99, 110, "99");
            //AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString();
            //SearchList1.Visible = false;
            //btnQuery.Visible = false;
            //BindData();
            //showPanel();
            //AuthRight();
            Sel_DepId.Text = "報表列印";
            Rab_Sel1.Visible = false;
            Sel_Employ.Visible = false;
            Button1.Visible = false;
            CompanyList1.Visible = false;
            Epploy.Visible = false;
            ReportViewer1.Visible = false;
            Select_Data();
        }
        else
        {
            //公司找出相關資料
            if (!string.IsNullOrEmpty(CompanyList1.SelectValue) || !string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                Select_Data();
            }
            else
            {
                DepName.Items.Clear();
                Epploy.Items.Clear();
            }
            
            //BindData();
        }
        //if (SearchList1.CompanyValue == "")
        //{
        //    //btnNew.Visible = false;
        //    //btnNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        //    //btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        //}
        //else
        //{
        //    //btnNew.Visible = true;
        //    //btnNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Company=" + SearchList1.CompanyValue + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        //    btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Company=" + SearchList1.CompanyValue + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        //}
        ScriptManager1.RegisterPostBackControl(Sel_DepId);
    }

    //void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    //{
    //    BindData();
    //    showPanel();
    //}

    private void showPanel()
    {
        lbl_Msg.Text = "";

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
        Ssql = "SELECT Distinct A.Company,(A.DeptId+'-'+D.DepName)DeptId,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId, ";
        Ssql += " Case When Cast(ISnull(K.Balance,'0') as decimal(9, 2))<=0 Then '0.00%' Else Cast(Sum(Cast(K.Balance as decimal(9, 2)))as varchar)+'%' end Balance, ";//--已分配
        Ssql += " Case When ISnull(100-Cast(K.Balance as decimal(9, 2)),0) =0 then '100%' Else Cast(100-Cast(K.Balance as decimal(9, 2)) as varchar)+'%' end Remainder ";//---剩餘分配 
        Ssql += " FROM Personnel_Master A  ";
        Ssql += " inner join (Select Company,DepCode,DepName From Department) D On D.Company=a.Company and D.DepCode=A.DeptId ";
        Ssql += " Left outer join (Select B.Company,B.DeptId,B.Deptname,B.EmployeeId,B.EmployeeName,Sum(Cast(B.Balance as decimal(9, 2)))as Balance ";
        Ssql += " From CostCenter B Group By B.DeptId,B.Deptname,B.EmployeeId,B.Company,B.EmployeeName) K ON A.DeptId=K.DeptId And a.EmployeeId=K.EmployeeId ";
        Ssql += " Where 1=1";
       
        //公司
        DDLSr("A.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("A.DeptId", SearchList1.DepartmentValue);

        //員工
        DDLSr("A.EmployeeId", SearchList1.EmployeeValue);

        Ssql += " Group By A.Company,A.DeptId,D.DepName,A.EmployeeId,A.EmployeeName,K.Balance ";


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
                link.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Kind=Query&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")).TrimEnd() + "&DeptId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().IndexOf("-")).TrimEnd() + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");// 

                //link = (LinkButton)e.Row.FindControl("btnEdit");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Kind=M&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")).TrimEnd() + "&DeptId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().IndexOf("-")).TrimEnd() + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");// 
                break;

            case DataControlRowType.Header:

                //if (CompanyList1.SelectValue != "")
                //{
                //((LinkButton)e.Row.FindControl("btnNew")).Visible = false;
                //link = (LinkButton)e.Row.FindControl("btnNew");

                //if (link != null)
                //{
                //    //指定位置用top=100px,left=100px,
                //    link.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
                //}
                //}
                    break;
                
        }
    }

    private void BindData()
    {
        Ssql = "SELECT Distinct A.Company,(A.DeptId+'-'+D.DepName)DeptId,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId, ";
        Ssql += " Case When Cast(ISnull(K.Balance,'0') as decimal(9, 2))<=0 Then '0.00%' Else Cast(Sum(Cast(K.Balance as decimal(9, 2)))as varchar)+'%' end Balance, ";//--已分配
        Ssql += " Case When ISnull(Cast(K.Balance as decimal(9, 2)),0) =0 then '100%' Else Cast(100-Cast(K.Balance as decimal(9, 2)) as varchar)+'%' end Remainder ";//---剩餘分配 
        Ssql += " FROM Personnel_Master A  ";
        Ssql += " inner join (Select Company,DepCode,DepName From Department) D On D.Company=a.Company and D.DepCode=A.DeptId ";
        Ssql += " Left outer join (Select B.Company,B.DeptId,B.Deptname,B.EmployeeId,B.EmployeeName,Sum(Cast(B.Balance as decimal(9, 2))) as Balance ";
        Ssql += " From CostCenter B Group By B.DeptId,B.Deptname,B.EmployeeId,B.Company,B.EmployeeName) K ON A.DeptId=K.DeptId And a.EmployeeId=K.EmployeeId ";
        Ssql +=" Where 1=1";

        //公司
        DDLSr("A.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("A.DeptId", SearchList1.DepartmentValue);

        //員工
        DDLSr("A.EmployeeId", SearchList1.EmployeeValue);

        Ssql+=" Group By A.Company,A.DeptId,D.DepName,A.EmployeeId,A.EmployeeName,K.Balance ";

        SDS_GridView.SelectCommand = Ssql; 

        GridView1.DataBind();
        Navigator1.BindGridView = GridView1;
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
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "CostCenter";
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
      
        string sql = "Delete From CostCenter Where Company='" + L1PK + "' And EmployeeId='" + L2PK.Substring(0, L2PK.IndexOf("-")).Trim() + "'";// And DepositBank='" + L3PK + "' And DepositBankAccount='" + L4PK + "'

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
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }

    private void Select_Data() 
    {//依部門代出相關資料
        //分攤至部門
        string ApptrionName="";
        String StrSql;
        string EpployName="";
        DataTable DRK_2;
        //SearchList1.CompanyValue 
        if (DepName.Text == "" || DepName.Text == "請選擇") 
        {
            StrSql = " Select A.Company, Rtrim(A.DepCode)+'-'+A.DepName DepName From Department A Where 1=1 ";

            if (!string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                StrSql += "And A.Company='" + SearchList1.CompanyValue + "'";
            }
            else if (!string.IsNullOrEmpty(CompanyList1.SelectValue))
            {
                StrSql += "And A.Company='" + CompanyList1.SelectValue + "'";
            }

            StrSql += "  and A.DepType='3' Order By A.DepCode ";

            DataTable DRK = _MyDBM.ExecuteDataTable(StrSql);
            DepName.Items.Clear();
            DepName.Items.Add("請選擇");
            for (int i = 0; i < DRK.Rows.Count; i++)
            {
                ApptrionName = DRK.Rows[i]["DepName"].ToString();
                DepName.Items.Add(ApptrionName);
            }
        }
       
        //員工
        if (Epploy.Text == "" || Epploy.Text=="請選擇")
        {
            StrSql = " Select (RTrim(B.EmployeeId)+'-'+B.EmployeeName) EmployeeId From Personnel_Master B Where 1=1 ";

            if (!string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                StrSql += "And B.Company='" + SearchList1.CompanyValue + "'";
            }
            else if (!string.IsNullOrEmpty(CompanyList1.SelectValue))
            {
                StrSql += "And B.Company='" + CompanyList1.SelectValue + "'";
            }

            StrSql += " Order by DeptId,EmployeeId";

            DRK_2 = _MyDBM.ExecuteDataTable(StrSql);
            Epploy.Items.Clear();
            Epploy.Items.Add("請選擇");
            for (int i = 0; i < DRK_2.Rows.Count; i++)
            {
                EpployName = DRK_2.Rows[i]["EmployeeId"].ToString();
                Epploy.Items.Add(EpployName);
            }    
        }
        
    }
    protected void Sel_DepId_Click(object sender, EventArgs e)
    {
        //依部門列印
        string Epploy_No = "";
        string Dep_No = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue) && string.IsNullOrEmpty(SearchList1.CompanyValue))
        {
            lbl_Msg.Text = "請輸入公司別!!";
        }
        else if (Rab_Sel1.Checked == false && Rab_Sel2.Checked == false && Rab_Sel3.Checked == false)
        {
            lbl_Msg.Text = "請選擇列印選項!!";
        }
        else
        {
            String Sqlstr = "";

            Sqlstr = "SELECT Distinct A.Company,(A.DeptId+'-'+D.DepName)DeptId,(RTrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId, IsNull(K.ApportionId+'-'+K.ApportionName,'') ApportionId,";
            Sqlstr += " Case When Cast(ISnull(K.Balance,'0') as decimal(9, 2))<=0 Then '0.00' Else Cast(Sum(Cast(K.Balance as decimal(9, 2)))as varchar) end Balance ";
            Sqlstr += " FROM Personnel_Master A ";
            Sqlstr += " inner join (Select Company,DepCode,DepName From Department) D On D.Company=a.Company and D.DepCode=A.DeptId  ";
            Sqlstr += " Left outer join (Select B.Company,B.DeptId,B.Deptname,B.EmployeeId,B.EmployeeName,B.ApportionId,B.ApportionName,Cast(B.Balance as decimal(9, 2))as Balance  From CostCenter B ) K ON A.DeptId=K.DeptId And a.EmployeeId=K.EmployeeId  ";
            Sqlstr += "Where 1=1 ";

            if (!string.IsNullOrEmpty(CompanyList1.SelectValue))
            {
                Sqlstr += " and A.Company='" + CompanyList1.SelectValue + "'";
            }

            if (!string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                Sqlstr += " and A.Company='" + SearchList1.CompanyValue + "'";
            }
            //分攤至部門
            if (DepName.SelectedValue != "請選擇")
            {                
               Dep_No = DepName.SelectedValue.ToString().Substring(0, DepName.SelectedValue.ToString().IndexOf("-"));
               Sqlstr += " and K.ApportionId='" + Dep_No + "' ";
            }
            //部門
            if (SearchList1.DepartmentText != "全部")
            {               
               Dep_No = SearchList1.DepartmentValue;
               Sqlstr += " and  A.DeptId='" + Dep_No + "' ";
            }
            //員工
            if (Epploy.SelectedValue != "請選擇")
            {              
                Epploy_No = Epploy.SelectedValue.ToString().Substring(0, Epploy.SelectedValue.ToString().IndexOf("-"));
                Sqlstr += " and (A.EmployeeId='" + Epploy_No + "')";
            }

            if (SearchList1.EmployeeText != "全部")
            {               
                Epploy_No = SearchList1.EmployeeValue;                
                Sqlstr += " and (A.EmployeeId='" + Epploy_No + "')";
            }

            Sqlstr += " Group By A.Company,A.DeptId,D.DepName,A.EmployeeId,A.EmployeeName,K.Balance,K.ApportionId,K.ApportionName";

            DataTable Table_Tpk = _MyDBM.ExecuteDataTable(Sqlstr);

            ReportViewer1.LocalReport.DataSources.Clear();
            
            if (Rab_Sel1.Checked == true)
            {
                //依部門
                ReportViewer1.LocalReport.ReportPath = @"RDLC\CostCenter_Dep2.rdlc";
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DepName", "依部門"));
            }
           
            if (Rab_Sel2.Checked == true)
            {
                //依員工
                ReportViewer1.LocalReport.ReportPath = @"RDLC\CostCenter_Dep.rdlc";
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DepName", "依部門"));
            }
            

            if (Rab_Sel3.Checked == true)
            {
                //依分攤至部門
                ReportViewer1.LocalReport.ReportPath = @"RDLC\CostCenter_Dep2.rdlc";
                ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DepName", "依個人"));
            }


            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CostCenter", Table_Tpk));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(SearchList1.CompanyValue)));
            ReportViewer1.Visible = true;
        }
    }
    protected void Sel_Employ_Click(object sender, EventArgs e)
    {
        //依員工列印
        string Epploy_No = "";
        string Dep_No="";
         if (string.IsNullOrEmpty(CompanyList1.SelectValue) && string.IsNullOrEmpty(SearchList1.CompanyValue))        
        {
            lbl_Msg.Text = "請輸入公司別!!";
        }
        else
        {
            String Sqlstr = "";
            Sqlstr = "SELECT Distinct A.Company,(A.DeptId+'-'+D.DepName)DeptId,(RTrim(A.EmployeeId)+'-'+A.EmployeeName) EmployeeId, IsNull(K.ApportionId+'-'+K.ApportionName,'') ApportionId,";
            Sqlstr += " Case When Cast(ISnull(K.Balance,'0') as decimal(9, 2))<=0 Then '0.00' Else Cast(Sum(Cast(K.Balance as decimal(9, 2)))as varchar) end Balance ";
            Sqlstr += " FROM Personnel_Master A ";
            Sqlstr += " inner join (Select Company,DepCode,DepName From Department) D On D.Company=a.Company and D.DepCode=A.DeptId  ";
            Sqlstr += " Left outer join (Select B.Company,B.DeptId,B.Deptname,B.EmployeeId,B.EmployeeName,B.ApportionId,B.ApportionName,Cast(B.Balance as decimal(9, 2))as Balance  From CostCenter B ) K ON A.DeptId=K.DeptId And a.EmployeeId=K.EmployeeId  ";
            Sqlstr += "Where 1=1 ";

            if (!string.IsNullOrEmpty(CompanyList1.SelectValue))
            {
                Sqlstr += " and A.Company='" + CompanyList1.SelectValue + "'";
            }

            if (!string.IsNullOrEmpty(SearchList1.CompanyValue))
            {
                Sqlstr += " and A.Company='" + SearchList1.CompanyValue + "'";
            }

            //分攤至部門
            if (DepName.SelectedValue != "請選擇")
            {
                Dep_No = DepName.SelectedValue.ToString().Substring(0, DepName.SelectedValue.ToString().IndexOf("-"));
                Sqlstr += " and K.ApportionId='" + Dep_No + "' ";
            }
            //部門
            if (SearchList1.DepartmentText != "全部")
            {
                Dep_No = SearchList1.DepartmentValue;
                Sqlstr += " and  A.DeptId='" + Dep_No + "' ";
            }
            //員工

            if (Epploy.SelectedValue != "請選擇")
            {
                Epploy_No = Epploy.SelectedValue.ToString().Substring(0, Epploy.SelectedValue.ToString().IndexOf("-"));
                Sqlstr += " and (A.EmployeeId='" + Epploy_No + "')";
            }
            if (SearchList1.EmployeeText != "全部")
            {
                Epploy_No = SearchList1.EmployeeValue;
                Sqlstr += " and (A.EmployeeId='" + Epploy_No + "')";
            }

            Sqlstr += " Group By A.Company,A.DeptId,D.DepName,A.EmployeeId,A.EmployeeName,K.Balance,K.ApportionId,K.ApportionName";

            DataTable Table_Tpk = _MyDBM.ExecuteDataTable(Sqlstr);

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.ReportPath = @"RDLC\CostCenter_Dep2.rdlc";
            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("CostCenter", Table_Tpk));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("CompanyName", DBSetting.CompanyName(CompanyList1.SelectValue)));
            ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DepName", "依個人"));
            ReportViewer1.Visible = true;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Sel_Employ_Click(0, EventArgs.Empty);
    }
    protected void Rab_Sel1_CheckedChanged(object sender, EventArgs e)
    {
        //判斷選項
        if (Rab_Sel1.Checked == true)
        {
            Rab_Sel2.Checked = false;
            Rab_Sel3.Checked = false;
        }   
    }
    protected void Rab_Sel2_CheckedChanged(object sender, EventArgs e)
    {
        if (Rab_Sel2.Checked == true)
        {
            Rab_Sel1.Checked = false;
            Rab_Sel3.Checked = false;
        }
    }
    protected void Rab_Sel3_CheckedChanged(object sender, EventArgs e)
    {
        if (Rab_Sel3.Checked == true)
        {
            Rab_Sel1.Checked = false;
            Rab_Sel2.Checked = false;
        }
    }
}
