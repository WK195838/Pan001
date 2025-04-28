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

public partial class Import_PayrollMaster : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM041";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

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
        //Navigator1.BindGridView = GridView1;

    

     
        if (!Page.IsPostBack)
        {
            //Navigator1.Visible = true;
            GridView1.Visible = true;
            BindData();

            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            //Navigator1.Visible = false;

        }
        else
        {
            //if (Navigator1.Visible)
                BindData();
        }



    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = 'Payroll' And RTrim(ProgramPath)='Import/IncomePayrollMaster.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }



    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;

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
                    //DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);

                    for (int i = 3; i < e.Row.Cells.Count - 1; i++)
                        e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;


                    //((LiteralControl)e.Row.Cells[5].Controls[0]).Text += "&nbsp;&nbsp;&nbsp;&nbsp;";
                }

                //link = (LinkButton)e.Row.FindControl("btnSelect");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Kind=Query&Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");

                //link = (LinkButton)e.Row.FindControl("btnEdit");
                //link.Attributes.Add("onclick", "javascript:var win =window.open('PersonnelMaster_M.aspx?Company=" + GridView1.DataKeys[e.Row.RowIndex].Values["Company"].ToString().Trim() + "&EmployeeId=" + GridView1.DataKeys[e.Row.RowIndex].Values["EmployeeId"].ToString().Trim() + "','','width=1010px,height=730px,scrollbars=yes,resizable=yes');");
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
     

        try
        {
            Ssql = @"SELECT [Company]
                  ,[EmployeeId]
                  ,[EmployeeName]
                  ,[EnglishName]
                  ,[DeptId]
                  ,[TitleCode]
                  ,[Grade]
                  ,[Shift]
                  ,[Identify]
                  ,[PayCode]
                  ,[ResignCode]
                  ,[HireDate]
                  ,[LeaveWithoutPay]
                  ,[ReHireDate]
                  ,[ResignDate]
                  ,[ObserveExpirationDate]
                  ,[LstPromotionDate]
                  ,[LstChangeSalaryDate]
                  ,(Case [LWC] When 'Y' then '是' else '否' end ) as [LWC]
                  ,[Union]
                  ,[SpecialSeniority]
                  ,[BloodType]
                  ,[IDNo]
                  ,[IDType]
                  , (Case [Sex] When 'M' then '男' else '女' end ) as [Sex]
                  ,[Nationality]
                  ,[BirthDate]
                  ,[MaritalStatus]
                  ,[DependentsNum]
                  ,[Military]
                  ,[BornPlace]
                  ,[Addr]
                  ,[ResidenceAddr]
                  ,[TEL]
                  ,[MobilPhone]
                  ,[Email]
                  ,[Contact]
                  ,[Guarantor1]
                  ,[Guarantor2]
                  ,[Introducer]
                  ,[ContactTEL]
                  ,[Guarantor1TEL]
                  ,[Guarantor2TEL]
                  ,[IntroducerTEL]
                  ,[CCN]
                  ,[EducationCode]
                  ,[Rank]
                  ,[ReportDeptId]
                   FROM [EBOSDB].[dbo].[Personnel_Master_Temp]
                  ";





   
          
            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);
            
            if (theDT.Rows.Count > 0)
            {
                GridView1.DataSource = theDT;
                GridView1.DataBind();
                GridView1.Visible = true;

                //Navigator1.BindGridView = GridView1;
                //Navigator1.DataBind();
                GridView1.Visible = true;
            }


            if (GridView1.Rows.Count == 0)
                lbl_Msg.Text = "查無資料!!";
            else
                lbl_Msg.Text = "";


        }
        catch { }
       
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";

        //if (string.IsNullOrEmpty(SearchList1.CompanyValue))
        //{
        //    lbl_Msg.Text = "請選擇先公司!";
        //}
        //else
        //{
        //    BindData();
        //}
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

    void CompanyList1_SelectedIndex(object sender, UserControl_CompanyList.SelectEventArgs e)
    {

    }


    protected void Button2_Click(object sender, EventArgs e)
    {
       //刪除已存在目地資料
        _MyDBM.ExecuteCommand("DELETE FROM Personnel_Master WHERE Company='A1' AND EmployeeId in (SELECT EmployeeId FROM Personnel_Master_Temp)");
       //新增目的資料
        _MyDBM.ExecuteCommand("Insert Into Personnel_Master Select * from Personnel_Master_TEMP WHERE Company='A1'");
       //刪除來源資料
        _MyDBM.ExecuteCommand("DELETE FROM Personnel_Master_Temp WHERE Company='A1'");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //刪除
        _MyDBM.ExecuteCommand("DELETE FROM Personnel_Master_Temp WHERE Company='A1'");
    }
}
