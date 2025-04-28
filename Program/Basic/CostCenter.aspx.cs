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

public partial class CostCenter : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM020";
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        lbl_Msg.Text = "";//清空訊息

        //下拉式選單連動設定    
        Navigator1.BindGridView = GridView1;
        SearchList1.SelectedChanged += new SearchList.SelectedIndexChanged(SearchList1_SelectedChanged);
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
            if (sCompany != null)
            {
                SearchList1.CompanyValue = sCompany;
            }
            //年月處理
            //YearList1.SetYearList(99, 110, "99");
            //AmtDate = YearList1.SelectYear.ToString() + "/" + MonthList1.SelectMonth.ToString();
            SetResignCode();
            BindData();
            showPanel();
            AuthRight();
        }
        else
        {
            BindData();
        }


        if (SearchList1.CompanyValue == "")
        {
            btnNew.Visible = false;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        }
        else
        {
            btnNew.Visible = true;
            btnNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Company=" + SearchList1.CompanyValue + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
            btnEmptyNew.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Company=" + SearchList1.CompanyValue + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
        }
        //清除選項之提示訊息
        if (SearchList1.CompanyValue != "" && RB_Sel1.Checked ==true ) 
        {
            Clear.Attributes.Add("onclick", "return ( confirm('確定要清除【 " + SearchList1.CompanyText.Substring(SearchList1.CompanyText.IndexOf("-") + 1).Trim() + "】的分攤資料嗎?') );");
        }

        if (SearchList1.DepartmentText == "全部" && RB_Sel2.Checked == true)
        {
            Clear.Attributes.Add("onclick", "return ( confirm('確定要清除【 " + SearchList1.CompanyText.Substring(SearchList1.CompanyText.IndexOf("-") + 1).Trim() + " 】全部門分攤資料嗎?') );");
        }
        else if (SearchList1.DepartmentText != "全部" && RB_Sel2.Checked == true)
        {
            Clear.Attributes.Add("onclick", "return ( confirm('確定要清除【" + SearchList1.CompanyText.Substring(SearchList1.CompanyText.IndexOf("-") + 1).Trim() + "】─【" + SearchList1.DepartmentText.Substring(SearchList1.DepartmentText.IndexOf("-") + 1).Trim() + "】的分攤資料嗎?') );");
        }

    }
    void SearchList1_SelectedChanged(object sender, SearchList.SelectEventArgs e)
    {
        BindData();
        showPanel();
    }
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

                link = (LinkButton)e.Row.FindControl("btnEdit");
                link.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx?Kind=M&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().IndexOf("-")).TrimEnd() + "&DeptId=" + GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().Substring(0, GridView1.DataKeys[e.Row.RowIndex].Values["DeptId"].ToString().IndexOf("-")).TrimEnd() + "','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");// 
                break;

            case DataControlRowType.Header:

                //if (CompanyList1.SelectValue != "")
                //{
                //((LinkButton)e.Row.FindControl("btnNew")).Visible = false;
                link = (LinkButton)e.Row.FindControl("btnNew");

                if (link != null)
                {
                    //指定位置用top=100px,left=100px,
                    link.Attributes.Add("onclick", "javascript:var win =window.open('CostCenter_M.aspx','','width=1028px,height=764px,scrollbars=yes,resizable=yes');");
                }
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
        if (txtDateS.Text.Trim().Length > 0 || txtDateE.Text.Trim().Length > 0)
        { Ssql += ""; }
        else
        { Ssql += " Left outer"; }
        Ssql += " join (Select B.Company,B.DeptId,B.Deptname,B.EmployeeId,B.EmployeeName,Sum(Cast(B.Balance as decimal(9, 2))) as Balance ";
        Ssql += " From CostCenter B ";
        if (txtDateS.Text.Trim().Length > 0 && txtDateE.Text.Trim().Length > 0)
        {
            Ssql += " Where [B_effective] Between Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()) + "') And Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateE.Text.Trim()) + "') ";
        }
        else if (txtDateS.Text.Trim().Length > 0)
        {
            Ssql += " Where [B_effective] > Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateS.Text.Trim()) + "') ";
        }
        else if (txtDateE.Text.Trim().Length > 0)
        {
            Ssql += " Where [B_effective] < Convert(Datetime,'" + _UserInfo.SysSet.FormatADDate(txtDateE.Text.Trim()) + "') ";
        }
        Ssql += " Group By B.DeptId,B.Deptname,B.EmployeeId,B.Company,B.EmployeeName) K ON A.DeptId=K.DeptId And a.EmployeeId=K.EmployeeId ";
        Ssql +=" Where 1=1";

        //公司
        DDLSr("A.Company", SearchList1.CompanyValue);

        //部門
        DDLSr("A.DeptId", SearchList1.DepartmentValue);

        //員工
        DDLSr("A.EmployeeId", SearchList1.EmployeeValue);

        if (cbResignC.Items.Count > 0)
        {//2012/04/16 依人事要求加入離職篩選條件
            string strTL = "";
            foreach (ListItem lis in cbResignC.Items)
            {
                if (lis.Selected)
                {
                    strTL += "'" + lis.Value + "',";
                }
            }
            if (strTL.Length > 0)
            {
                Ssql += " And A.ResignCode in (" + strTL + "'-')";
            }
        }        

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

    protected void RB_Sel1_CheckedChanged(object sender, EventArgs e)
    {
        if (RB_Sel1.Checked == true)
        {
            RB_Sel2.Checked = false;
        }       
    }
    protected void RB_Sel2_CheckedChanged(object sender, EventArgs e)
    {
        if (RB_Sel2.Checked == true)
        {
            RB_Sel1.Checked = false;
        }      
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        if(RB_Sel1.Checked ==false && RB_Sel2.Checked ==false)
        {
            lbl_Msg.Text = "請選擇清除選項!!";
        }

        //依全公司進行清除
        if (SearchList1.CompanyValue != "" && RB_Sel1.Checked == true) 
        {
           Del_Company();
           BindData();
           lbl_Msg.Text = "已依公司選項清除【" + SearchList1.CompanyText + " 】的分攤資料";
        }

        //依全部門或部份挑選 進行清除
        if (RB_Sel2.Checked == true && (SearchList1.DepartmentText == "全部" || SearchList1.DepartmentText != "全部"))
        {
           Del_Department();
           BindData();
           if (SearchList1.DepartmentText != "全部")
           {
               lbl_Msg.Text = "已依部門選項清除【" + SearchList1.CompanyText + "】─【" + SearchList1.DepartmentText + "】的分攤資料";
           }
           else 
           {
               lbl_Msg.Text = "已依部門選項清除【" + SearchList1.CompanyText + "】全部門分攤資料";
           }
        } 

    }

    private void Del_Company()
    {
        string SQLC = "";
        DataTable DPK;
        SQLC = "Delete CostCenter Where 1=1";

        SQLC += "  and Company='" + SearchList1.CompanyValue + "'";

        DPK = _MyDBM.ExecuteDataTable(SQLC);   

        return ;
    }
    private void Del_Department()
    {
        string SQLC = "";
        DataTable DPK;
        SQLC = "Delete CostCenter Where 1=1";

        SQLC += "  and Company='" + SearchList1.CompanyValue + "'";

        if (SearchList1.DepartmentText != "全部")
        {
            SQLC += "  and DeptId='" + SearchList1.DepartmentValue  + "'";
            
        }
        else 
        {
            lbl_Msg.Text = "已依部門選項清除全部門分攤資料";
        }

        DPK = _MyDBM.ExecuteDataTable(SQLC);

        
        return;
    }

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
