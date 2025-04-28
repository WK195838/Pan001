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

public partial class Basic_PersonnelAdjustment_I : System.Web.UI.Page
{
    #region 變數宣告
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM006";
    DBManger _MyDBM;
    SysSetting SysSet = new SysSetting();
    #endregion

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/PersonnelAdjustment.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        #region @   SqlDataSource 設定  @

        SqlDataSource1.SelectCommand = "SELECT PersonnelAdjustment.* FROM PersonnelAdjustment WHERE Company = @Company and EmployeeId=@EmployeeId and AdjustmentCategory=@AdjustmentCategory and EffectiveDate=@EffectiveDate";
        string[] QName ={ "Company", "EmployeeId", "AdjustmentCategory", "EffectiveDate" };

        for (int i = 0; i < QName.Length; i++)
        {
            QueryStringParameter QData = new QueryStringParameter();
            QData.Name = QName[i];
            QData.QueryStringField = QName[i];
            SqlDataSource1.SelectParameters.Add(QData);
        }

        #endregion
        
        btnCancel.Attributes.Add("onclick", "javascript:window.close();");
        bool blCheckLogin = _UserInfo.AuthLogin;
        if ((blCheckLogin == false) || (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true"))
        {
            bool blCheckProgramAuth = false;
            if (blCheckLogin == false)
                ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("UnLogin");
            else
            {
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.Payroll, _ProgramId, "Detail");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        DataTable DT2 = new DataTable();
        string tmpCompany = "";

        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {
            //修改欄位顯示名稱
            DataTable DT = _MyDBM.ExecuteDataTable( "Select dbo.GetColumnTitle('PersonnelAdjustment','" + DetailsView1.Rows[i].Cells[0].Text + "')" );
            string text = DetailsView1.Rows[i].Cells[0].Text;
            string text2 = DetailsView1.Rows[i].Cells[1].Text;
            
            if (DT.Rows.Count > 0)
            {
                switch(text)
                {
                    //  公司
                    case "Company":
                        tmpCompany = text2;
                        DT2 = _MyDBM.ExecuteDataTable("SELECT Company,CompanyName FROM Company Where Company = '" + text2 + "'");
                        if ( DT2.Rows.Count > 0 ) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["Company"].ToString() + " - " + DT2.Rows[0]["CompanyName"].ToString();
                        break;
                    //  員工
                    case "EmployeeId":
                        DT2 = _MyDBM.ExecuteDataTable("SELECT EmployeeId,EmployeeName FROM Personnel_Master Where Company = '" + tmpCompany + "' and EmployeeId = '" + text2 + "'");
                        if ( DT2.Rows.Count > 0 ) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["EmployeeId"].ToString() + " - " + DT2.Rows[0]["EmployeeName"].ToString();
                        break;

                    //  生效日期
                    case "EffectiveDate":
                        DetailsView1.Rows[i].Cells[1].Text = SysSet.FormatDate(text2);
                        break;

                    //  部門（自）- 部門（至）
                    case "DepCode_F":case "DepCode_T":
                        DT2 = _MyDBM.ExecuteDataTable("SELECT DepCode,DepName FROM Department Where DepCode = '" + text2 + "' and Company = '" + tmpCompany + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["DepName"].ToString();
                        break;

                    //  調動類別
                    case "AdjustmentCategory":
                    //  職稱（自）- 職稱（至）
                    case "Title_F":case "Title_T":
                    //  薪制（自）-  薪制（至）
                    case "SalarySystem_F":case "SalarySystem_T":
                    //  班別（自）- 班別（至）
                    case "Class_F":case "Class_T":
                        string TmpCodeID =
                            text.Contains("AdjustmentCategory") ? "PY#Adjustm" : 
                            text.Contains("Class_") ? "PY#Shift" : 
                            text.Contains("SalarySystem_") ? "PY#PayCode" :
                            text.Contains("Title_") ? "PY#TitleCo" :
                            "";

                        DT2 = _MyDBM.ExecuteDataTable("Select CodeCode,CodeName from CodeDesc Where CodeID ='" + TmpCodeID + "' And CodeCode = '" + text2 + "'");
                        if (DT2.Rows.Count > 0) DetailsView1.Rows[i].Cells[1].Text = DT2.Rows[0]["CodeName"].ToString();
                        break;

                    //  主檔更新
                    case "MasterUpdate":
                        DetailsView1.Rows[i].Cells[1].Text = text2.Replace("N","否").Replace("Y","是");
                        break;

                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
            }
        }
    }    
}
