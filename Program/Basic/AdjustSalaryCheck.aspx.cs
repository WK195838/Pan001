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

public partial class Basic_AdjustSalaryCheck : System.Web.UI.Page
{
    string Ssql = "";
    string Ssql1 = "";
    string Ssql2 = "";
    
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM022";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
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
        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/AdjustSalaryMaster.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息

        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        //Ssql = "SELECT Company,CompanyShortName FROM Company_Master Where CompanyShortName='" + _UserInfo.UData.Company + "'";

        //DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
        //tbCompany.Text = DT.Rows[0][0].ToString() + DT.Rows[0][1].ToString();
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
            BindData();


        }
        else
        {
            //BindData();
        }
        EffectiveDate.CssClass = "JQCalendar";
    }

    private void BindData()
    {
        
        //ImageButton btOpenCal = (ImageButton)EffectiveDate.FindControl("btnCalendar1");
        ////ImageButton btOpenCal = (ImageButton)e.Row.FindControl("btnCalendar1");
        //if (btOpenCal != null)
        //    btOpenCal.Attributes.Add("onclick", "return GetPromptDate(" + EffectiveDate.ClientID + ");");
        //DetailsView1.Rows[i].Cells[1].Controls.Add(btOpenCal);



        //DataTable DT2 = _MyDBM.ExecuteDataTable(Ssql + " Order By Company");

    }
 
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (CompanyList1.SelectValue != "")
        {
            if (!string.IsNullOrEmpty(EffectiveDate.Text))
            {
                lbl_Err.Text = "";
                string UpdateItem = "", UpdateValue = "", Olddate = "";
                string Nowedate = _UserInfo.SysSet.FormatADDate(EffectiveDate.Text.ToString());
                Ssql = "SELECT * from AdjustSalary_Master Where (ApproveDate is null Or ApproveDate = convert(datetime ,'1912/01/01')) And datediff(day, Convert(datetime,EffectiveDate),convert(datetime ,'" + Nowedate + "'))>=0 And Company='" + CompanyList1.SelectValue + "'";

                DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

                int j = 0, p = 0;
                int allnum = 0;
                if (tb.Rows.Count > 0)
                {
                    for (int i = 0; i < tb.Rows.Count; i++)
                    {//為了一筆一筆核定並紀錄LOG,故使用迴圈做單筆更新而非整批更新
                        try
                        {                            
                            string effdate = Convert.ToDateTime(tb.Rows[i]["EffectiveDate"]).ToString("yyyy/MM/dd");

                            Ssql1 = "UPDATE AdjustSalary_Master SET ApproveDate=GetDate() " +
                                " Where Convert(varchar,EffectiveDate,111)='" + effdate + "' " +
                                " And RTrim([Company])='" + tb.Rows[i]["Company"].ToString().Trim() + "' " +
                                " And RTrim([EmployeeId])='" + tb.Rows[i]["EmployeeId"].ToString().Trim() + "' " +
                                " And RTrim([AdjustSalaryItem])='" + tb.Rows[i]["AdjustSalaryItem"].ToString().Trim() + "' " +
                                " And (ApproveDate is null Or ApproveDate = convert(datetime ,'1912/01/01')) ";
                            j = _MyDBM.ExecuteCommand(Ssql1);

                            Ssql2 = "UPDATE Payroll_Master_Detail SET Amount='" + tb.Rows[i]["NewSalary"].ToString().Trim() + "' Where SalaryItem='" + tb.Rows[i]["AdjustSalaryItem"].ToString().Trim() + "' And Company='" + tb.Rows[i]["Company"].ToString().Trim() + "' And EmployeeId='" + tb.Rows[i]["EmployeeId"].ToString().Trim() + "'";
                                    p = _MyDBM.ExecuteCommand(Ssql2);
                            allnum += p;

                            //LOG用資料
                            Olddate = (tb.Rows[i]["ApproveDate"] == null) ? "" : tb.Rows[i]["ApproveDate"].ToString();
                            UpdateItem += "ApproveDate|";
                            UpdateValue += Olddate.Trim() + "->" + Nowedate.Trim() + "|";
                        }
                        catch (Exception ex)
                        {
                            lbl_Err.Text = "更新失敗!!  原因: " + ex.Message;
                        }
                        if (lbl_Err == null)
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
                            MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "AdjustSalary_Master";
                            MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "U";
                            MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = (UpdateItem.Length * 2 > 255) ? "長度:" + UpdateItem.Length.ToString() : UpdateItem;
                            MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = UpdateValue;
                            MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                            //此時不設定異動結束時間
                            //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                            MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                            _MyDBM.DataChgLog(MyCmd.Parameters);
                            #endregion
                        }
                        //lbl_Msg.Text = "共" + tb.Rows.Count.ToString() + "筆資料須核定";
                    }
                    if (allnum > 0)
                    {
                        lbl_Msg.Text = "共" + allnum.ToString() + " 個資料列 " + "更新成功!!";
                    }
                }
                else
                {
                    lbl_Msg.Text = "查無核定資料";
                }
            }
            else
            {
                lbl_Err.Text = "未選擇生效日期";
            }
        }
        else
        {
            lbl_Err.Text = "未選擇公司編號";
        }
        if (lbl_Err == null)
        {
            #region 完成異動後,更新LOG資訊
            MyCmd.Parameters["@SQLcommand"].Value = (lbl_Err == null) ? "Success" : lbl_Msg.Text;
            MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
            _MyDBM.DataChgLog(MyCmd.Parameters);
            #endregion
        }
    }
    protected void btnClearErrData_Click(object sender, EventArgs e)
    {
        try
        {
            //找出調薪主檔中薪資代碼不存在之資料
            //  FROM [EBOSDB].[dbo].[AdjustSalary_Master] where Not Exists (select * from SalaryStructure_Parameter where SalaryId=AdjustSalaryItem )
            //刪除調薪主檔中調薪為空之資料
            Ssql = "Delete From AdjustSalary_Master where IsNull(OldlSalary,'')='' and IsNull(NewSalary,'')='' ";

            _MyDBM.ExecuteCommand(Ssql);

            lbl_Msg.Text = "已刪除調薪主檔中調薪為空之資料！";
        }
        catch (Exception ex) {
            lbl_Msg.Text = "刪除空之資料時發生錯誤，請將錯誤訊息提供資訊人員：" + ex.Message;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (CompanyList1.SelectValue != "")
        {
            if (!string.IsNullOrEmpty(EffectiveDate.Text) )
            {
                DateTime ed=Convert.ToDateTime(_UserInfo.SysSet.FormatADDate(EffectiveDate.Text));
                if (ed <= DateTime.Now)
                {
                    lbl_Err.Text = "";
                    string edate = _UserInfo.SysSet.FormatADDate(EffectiveDate.Text.ToString());
                    //尋找 SELECT * from AdjustSalary_Master Where (Convert(datetime,ApproveDate)= convert(datetime ,'1912/01/01')) and datediff(day, Convert(datetime,EffectiveDate),convert(datetime ,'2010/10/05'))>=0 
                    Ssql = "SELECT [Company],[EmployeeId],[EffectiveDate] "+
                        " ,[AdjustSalaryItem]+' - '+(select SalaryName from SalaryStructure_Parameter where SalaryId=[AdjustSalaryItem] ) as [AdjustSalaryItem] "+
                        " ,[AdjustSalaryReasonCode]+' - '+(select CodeName from CodeDesc where CodeID='PY#AdjCode' And CodeCode=[AdjustSalaryReasonCode] ) as [AdjustSalaryReasonCode] " +
                        " ,[AdjustSalaryReason],[ApproveDate],[OldlSalary],[NewSalary] "+
                        " FROM [AdjustSalary_Master] " +
                        " Where (ApproveDate is null Or ApproveDate = convert(datetime ,'1912/01/01')) And datediff(day, Convert(datetime,EffectiveDate),convert(datetime ,'" + edate + "'))>=0 And Company='" + CompanyList1.SelectValue + "'";

                    DataTable tb = _MyDBM.ExecuteDataTable(Ssql);

                    if (tb.Rows.Count > 0)
                    {
                        lbl_Msg.Text = "共" + tb.Rows.Count.ToString() + "筆資料須核定";
                        GridView1.DataSource = tb;
                        GridView1.DataBind();
                    }
                    else
                    {
                        lbl_Msg.Text = "查無核定資料";
                    }
                }
                else
                {
                    lbl_Err.Text = "生效日期不可大於系統日期";
                }
            }
            else
            {
                lbl_Err.Text = "未選擇生效日期";
            }
        }
        else
        {
            lbl_Err.Text = "未選擇公司編號";
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "paginationRowEdgeLl";
            e.Row.Cells[e.Row.Cells.Count - 1].CssClass = "paginationRowEdgeRl";

            #region 設定標題列
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {//修改欄位顯示名稱
                DataTable DT = _MyDBM.ExecuteDataTable("Select dbo.GetColumnTitle('AdjustSalary_Master','" + e.Row.Cells[i].Text + "')");
                if (DT.Rows.Count > 0)
                {
                    e.Row.Cells[i].Text = DT.Rows[0][0].ToString().Trim();
                }
            }

            //for (i = 0; i < theRetList.Length; i++)
            //    e.Row.Cells[i].Visible = false;
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string tempValue = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //公司編號
            //員工編號            
            if (DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[1].Text.Trim()) != null) if (DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[1].Text.Trim()) != "") e.Row.Cells[1].Text = e.Row.Cells[1].Text + " - " + DBSetting.PersonalName(CompanyList1.SelectValue, e.Row.Cells[1].Text.Trim());

            //日期
            if (e.Row.Cells[2].Text.Replace("&nbsp;", " ").Trim() != "") e.Row.Cells[2].Text = _UserInfo.SysSet.FormatDate(e.Row.Cells[2].Text.Replace("&nbsp;", " ").Trim());
            if (e.Row.Cells[6].Text.Replace("&nbsp;", " ").Trim() != "") tempValue = _UserInfo.SysSet.FormatDate(e.Row.Cells[6].Text.Replace("&nbsp;", " ").Trim());
            if (tempValue.Trim().Equals("01/01/01") || tempValue.Trim().Equals("1912/01/01"))
            {
                tempValue = "";
            }
            e.Row.Cells[6].Text = tempValue;

            e.Row.Cells[7].Text = py.DeCodeAmount(e.Row.Cells[7].Text).ToString("N0");
            e.Row.Cells[8].Text = py.DeCodeAmount(e.Row.Cells[8].Text).ToString("N0");
        }
    }

}
