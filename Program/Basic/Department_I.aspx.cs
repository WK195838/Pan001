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

public partial class Basic_Department_I : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB003";
    DBManger _MyDBM;
    String sCompany;

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
                blCheckProgramAuth = _UserInfo.CheckPermission(SysSetting.SystemName.SYS, _ProgramId, "Detail");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
        try
        {
            sCompany = DetailsView1.DataKey[0].ToString().Trim();
        }
        catch
        {
            if (Request["Company"] != null)
                sCompany = Request["Company"].Trim();
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('Department','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("FixedAmount") || DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryRate") || DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryMasterList") || DetailsView1.Rows[i].Cells[0].Text.Contains("Payroll") || DetailsView1.Rows[i].Cells[0].Text.Contains("P1SalaryMasterList") || DetailsView1.Rows[i].Cells[0].Text.Contains("P1Payroll"))
                //{//數字欄位需靠右對齊
                //    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                //    tc.Style.Add("text-align", "right");
                //}
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                //{
                //    DetailsView1.Rows[i].Cells[1].Text=DetailsView1.Rows[i].Cells[1].Text.Substring(0, DetailsView1.Rows[i].Cells[1].Text.Trim().Length);
                //}
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ChiefID") || DetailsView1.Rows[i].Cells[0].Text.Contains("ParentDepCode"))
                {
                    if (DetailsView1.Rows[i].Cells[1].Text.Trim() == "")
                    {
                        DetailsView1.Rows[i].Cells[1].Text = "無";
                    }
                }
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                if (DT.Rows[0][0].ToString().Trim().Contains("公司"))
                {
                    DetailsView1.Rows[0].Cells[1].Text = DetailsView1.Rows[0].Cells[1].Text.Trim() + " - " + DBSetting.CompanyName(sCompany).ToString();
                }
                if (DT.Rows[0][0].ToString().Trim().Contains("部門類型"))
                {
                    Ssql = "select CodeName from CodeDesc where CodeID='DeptType' and CodeCode='" + DetailsView1.Rows[i].Cells[1].Text.Trim() + "'";
                    DataTable dt = _MyDBM.ExecuteDataTable(Ssql);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DetailsView1.Rows[i].Cells[1].Text = DetailsView1.Rows[i].Cells[1].Text.Trim() + " - " + dt.Rows[0]["CodeName"].ToString().Trim();
                        }
                    }

                }
                //if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                //{//將日期欄位格式化
                //    try
                //    {
                //        TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                //        tc.Text = _UserInfo.SysSet.FormatDate(tc.Text);
                //    }
                //    catch { }
                //}
                //if (DT.Rows[0][0].ToString().Trim().Contains("金額"))
                //{
                //    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                //    tc.Text = string.Format("{0:0,0}", int.Parse(tc.Text));
                //}
            }
        }
    }   
}
