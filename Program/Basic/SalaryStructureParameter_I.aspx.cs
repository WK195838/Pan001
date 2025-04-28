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

public partial class Basic_SalaryStructureParameter_I : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB007";
    DBManger _MyDBM;

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/SalaryStructureParameter.aspx'");
        if (DT.Rows.Count > 0)
            _ProgramId = DT.Rows[0][0].ToString();
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
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            Ssql = "Select dbo.GetColumnTitle('SalaryStructure_Parameter','" + DetailsView1.Rows[i].Cells[0].Text + "')";

            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);

            if (DT.Rows.Count > 0)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FixedAmount") || DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryRate") || DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryMasterList") || DetailsView1.Rows[i].Cells[0].Text.Contains("Payroll") || DetailsView1.Rows[i].Cells[0].Text.Contains("P1SalaryMasterList") || DetailsView1.Rows[i].Cells[0].Text.Contains("P1Payroll"))
                {//數字欄位需靠右對齊
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    tc.Style.Add("text-align", "right");
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("NWTax"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "Y")
                    {
                        tc.Text += "- 應稅";
                    }
                    else
                    {
                        tc.Text += "- 免稅";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("PMType"))
                {//下拉式選單
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "A")
                    { 
                        tc.Text += "- 加項";
                    }
                    else
                    {
                        tc.Text += "- 減項";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("ItemType"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "0")
                    {
                        tc.Text += "- 系統設定";
                    }
                    else if (tc.Text == "1")
                    {
                        tc.Text += "- 固定項目";
                    }
                    else
                    {
                        tc.Text += "- 變動項目";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("SalaryType"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "A")
                    {
                        tc.Text += "- 固定金額";
                    }
                    else if (tc.Text == "B")
                    {
                        tc.Text += "- 薪資比率";
                    }
                    else
                    {
                        tc.Text += "變動金額";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("Properties"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "1")
                    {
                        tc.Text += "- 薪資";
                    }
                    else if(tc.Text=="2")
                    {
                        tc.Text += "- 成本";
                    }
                    else if (tc.Text == "3")
                    {
                        tc.Text += "- 公司負擔";
                    }
                    else
                    {
                        tc.Text += "- 公司獎金";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("P1CostSalaryItem"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "A")
                    {
                        tc.Text += "- 上期借支";
                    }
                    else if (tc.Text == "B")
                    {
                        tc.Text += "- 上期其它";
                    }
                    else
                    {
                        tc.Text += "- 非上期項目";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("RegularPay"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text == "Y")
                    {
                        tc.Text += "- 是";
                    }
                    else
                    {
                        tc.Text += "- 否";
                    }
                }
                if (DetailsView1.Rows[i].Cells[0].Text.Contains("FunctionId"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    if (tc.Text.Contains(" ") ||tc.Text=="")
                    {
                        tc.Text += "不選擇公式";
                    }
                }
                //if (DetailsView1.Rows[i].Cells[0].Text.Contains("ParameterList"))
                //{
                //    DetailsView1.Rows[i].Cells[1].Text=DetailsView1.Rows[i].Cells[1].Text.Substring(0, DetailsView1.Rows[i].Cells[1].Text.Trim().Length);
                //}
                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                //if (DT.Rows[0][0].ToString().Trim().Contains("日"))
                //{//將日期欄位格式化
                //    try
                //    {
                //        TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                //        tc.Text = _UserInfo.SysSet.FormatDate(tc.Text);
                //    }
                //    catch { }
                //}
                if (DT.Rows[0][0].ToString().Trim().Contains("金額"))
                {
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    tc.Text = string.Format("{0:0,0}", int.Parse(tc.Text));
                }
            }
        }
    }    
}
