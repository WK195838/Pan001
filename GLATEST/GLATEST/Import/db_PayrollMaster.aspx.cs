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

public partial class db_PayrollMaster : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYI010";
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

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";//清空訊息
        //Navigator1.BindGridView = GridView1;
        
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
            CompanyList1.SelectValue = _UserInfo.UData.Company;
        }
        else
        {
            sCompany = CompanyList1.SelectValue.Trim().Replace("%", "");
        }

        BindData();
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
                   FROM [Personnel_Master_Temp]
                  ";
            if (sCompany.Length > 0)
                Ssql += "where company='" + sCompany + "'";        

            DataTable theDT = _MyDBM.ExecuteDataTable(Ssql);

            if (theDT.Rows.Count > 0)
            {
                GridView1.DataSource = theDT;
                GridView1.DataBind();
                GridView1.Visible = true;

                //Navigator1.BindGridView = GridView1;
                //Navigator1.DataBind();
            }
            else
                GridView1.Visible = false;


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
        SetTrx();
    }

    private void SetTrx()
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        #region 開始匯入前,先寫入LOG
        int result = 0;
        DateTime StartDateTime = DateTime.Now;
        MyCmd.Parameters.Clear();
        MyCmd.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = "Personnel_Master";
        MyCmd.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "I";
        MyCmd.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "ALL";
        MyCmd.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = Ssql;
        MyCmd.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
        //此時不設定異動結束時間
        //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
        MyCmd.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
        _MyDBM.DataChgLog(MyCmd.Parameters);
        #endregion
        try
        {
            //查詢可轉換資料筆數
            DataTable dt = _MyDBM.ExecuteDataTable("Select Count(*) FROM Personnel_Master_TEMP WHERE " + Ssql);
            if (dt != null && ((int)dt.Rows[0][0]) > 0)
            {
                //補零 Grade Rank
                _MyDBM.ExecuteCommand("UPDATE [Personnel_Master_Temp] SET [Grade] = (Case LEN([Grade]) When 1 then '0' + [Grade] else [Grade] end ) ,[Rank] = (Case LEN([Rank]) When 1 then '0' + [Rank] else [Rank] end ) WHERE LEN([Grade])='1' or LEN([Rank])='1'");

                //不可以刪,會連其它相關資料都刪掉
                ////刪除已存在目地資料
                //_MyDBM.ExecuteCommand("DELETE FROM Personnel_Master WHERE Company='01' AND EmployeeId in (SELECT EmployeeId FROM Personnel_Master_Temp)");

                string strSQL = @"UPDATE [dbo].[Personnel_Master]
        SET [EmployeeName] = IsNull((Select EmployeeName from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[EmployeeName])
      ,[EnglishName] = IsNull((Select EnglishName from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[EnglishName])
      ,[DeptId] = IsNull((Select DeptId from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[DeptId])
      ,[TitleCode] = IsNull((Select TitleCode from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[TitleCode])
      ,[Grade] = IsNull((Select Grade from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Grade])
      ,[Shift] = IsNull((Select Shift from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Shift])
      ,[Identify] = IsNull((Select Identify from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Identify])
      ,[PayCode] = IsNull((Select PayCode from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[PayCode])
      ,[ResignCode] = IsNull((Select ResignCode from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ResignCode])
      ,[HireDate] = IsNull((Select HireDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[HireDate])
      ,[LeaveWithoutPay] = IsNull((Select LeaveWithoutPay from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[LeaveWithoutPay])
      ,[ReHireDate] = IsNull((Select ReHireDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ReHireDate])
      ,[ResignDate] = IsNull((Select ResignDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ResignDate])
      ,[ObserveExpirationDate] = IsNull((Select ObserveExpirationDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ObserveExpirationDate])
      ,[LstPromotionDate] = IsNull((Select LstPromotionDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[LstPromotionDate])
      ,[LstChangeSalaryDate] = IsNull((Select LstChangeSalaryDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[LstChangeSalaryDate])
      ,[LWC] = IsNull((Select LWC from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[LWC])
      ,[Union] = IsNull((Select [Union] from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Union])
      ,[SpecialSeniority] = IsNull((Select SpecialSeniority from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[SpecialSeniority])
      ,[BloodType] = IsNull((Select BloodType from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[BloodType])
      ,[IDNo] = IsNull((Select IDNo from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[IDNo])
      ,[IDType] = IsNull((Select IDType from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[IDType])
      ,[Sex] = IsNull((Select Sex from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Sex])
      ,[Nationality] = IsNull((Select Nationality from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Nationality])
      ,[BirthDate] = IsNull((Select BirthDate from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[BirthDate])
      ,[MaritalStatus] = IsNull((Select MaritalStatus from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[MaritalStatus])
      ,[DependentsNum] = IsNull((Select DependentsNum from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[DependentsNum])
      ,[Military] = IsNull((Select Military from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Military])
      ,[BornPlace] = IsNull((Select BornPlace from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[BornPlace])
      ,[Addr] = IsNull((Select Addr from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Addr])
      ,[ResidenceAddr] = IsNull((Select ResidenceAddr from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ResidenceAddr])
      ,[TEL] = IsNull((Select TEL from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[TEL])
      ,[MobilPhone] = IsNull((Select MobilPhone from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[MobilPhone])
      ,[Email] = IsNull((Select Email from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Email])
      ,[Contact] = IsNull((Select Contact from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Contact])
      ,[Guarantor1] = IsNull((Select Guarantor1 from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Guarantor1])
      ,[Guarantor2] = IsNull((Select Guarantor2 from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Guarantor2])
      ,[Introducer] = IsNull((Select Introducer from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Introducer])
      ,[ContactTEL] = IsNull((Select ContactTEL from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ContactTEL])
      ,[Guarantor1TEL] = IsNull((Select Guarantor1TEL from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Guarantor1TEL])
      ,[Guarantor2TEL] = IsNull((Select Guarantor2TEL from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Guarantor2TEL])
      ,[IntroducerTEL] = IsNull((Select IntroducerTEL from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[IntroducerTEL])
      ,[CCN] = IsNull((Select CCN from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[CCN])
      ,[EducationCode] = IsNull((Select EducationCode from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[EducationCode])
      ,[Rank] = IsNull((Select Rank from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[Rank])
      ,[ReportDeptId] = IsNull((Select ReportDeptId from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[ReportDeptId]) 
      ,[LISubsidy] = IsNull((Select LISubsidy from Personnel_Master_TEMP WHERE [Company] = Personnel_Master.Company And [EmployeeId] = Personnel_Master.EmployeeId),[LISubsidy])";
                //更新目的地已存在之資料
                _MyDBM.ExecuteCommand(strSQL + " WHERE " + Ssql);

                //新增目的資料
                _MyDBM.ExecuteCommand("Insert Into Personnel_Master Select * from Personnel_Master_TEMP PMT WHERE " + Ssql + " AND Not EXISTS (SELECT * FROM Personnel_Master Where Company=PMT.Company And EmployeeId=PMT.EmployeeId)");                             

                //刪除來源資料
                result = _MyDBM.ExecuteCommand("DELETE FROM Personnel_Master_Temp WHERE " + Ssql);
            }
            else
            {
                result = 0;
                lbl_Msg.Text = "無轉換資料";
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

        BindData();

        if (result > 0)
            lbl_Msg.Text = "轉換完成";
        else
            lbl_Msg.Text = "無轉換資料";

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (sCompany.Length > 0)
            Ssql = " Company='" + sCompany + "' ";
        else
            Ssql = "1=1";

        //刪除
        int del = _MyDBM.ExecuteCommand("DELETE FROM Personnel_Master_Temp WHERE " + Ssql);

        BindData();

        if (del != 0)
            lbl_Msg.Text = "刪除完成";
        else
            lbl_Msg.Text = "無刪除資料";
    }
}
