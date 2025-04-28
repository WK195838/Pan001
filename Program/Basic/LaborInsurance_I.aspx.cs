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

public partial class Basic_LaborInsurance_I : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM051";
    DBManger _MyDBM;
    String sCompany, sEmployeeId;
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

        //DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        //if (DT.Rows.Count > 0)
        //    _ProgramId = DT.Rows[0][0].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
            sEmployeeId = DetailsView1.DataKey[1].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
            if (Request["EmployeeId"] != null)
                sEmployeeId = Request["EmployeeId"].Trim();
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('LaborInsurance','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("EffectiveDate") || DetailsView1.Rows[i].Cells[0].Text.Contains("LI_amount"))
                {//數字欄位需靠右對齊
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    tc.Style.Add("text-align", "right");
                }
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                //{
                //    DetailsView1.Rows[i].Cells[1].Text=DetailsView1.Rows[i].Cells[1].Text.Substring(0, DetailsView1.Rows[i].Cells[1].Text.Trim().Length);
                //}
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                {//將日期欄位格式化
                    try
                    {
                        TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                        tc.Text = _UserInfo.SysSet.FormatDate(tc.Text);
                    }
                    catch { }
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("金額"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];                    
                    tc.Text = py.DeCodeAmount(tc.Text).ToString("N0");
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("員工"))
                {
                    if (DBSetting.PersonalName(sCompany, sEmployeeId) != null)
                    {
                        DetailsView1.Rows[1].Cells[1].Text = DetailsView1.Rows[1].Cells[1].Text.Trim() + " - " + DBSetting.PersonalName(sCompany, sEmployeeId).ToString();
                    }
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("公司"))
                {
                    DetailsView1.Rows[0].Cells[1].Text = DetailsView1.Rows[0].Cells[1].Text.Trim() + " - " + DBSetting.CompanyName(sCompany).ToString();
                }
            }
        }
    }    
}
