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

public partial class Pensionaccounts_I : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYM009";
    DBManger _MyDBM;
    SysSetting SysSet = new SysSetting();

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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/Pensionaccounts_I.aspx'");
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
            Ssql = "Select dbo.GetColumnTitle('Pensionaccounts_Master','" + DetailsView1.Rows[i].Cells[0].Text + "')";
            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            DataTable DT2 = new DataTable();
            if (DT.Rows.Count > 0)
            {
                string[] n = { DetailsView1.Rows[i].Cells[0].Text, DetailsView1.Rows[i].Cells[1].Text };
                DetailsView1.Rows[ i ].Cells[ 1 ].Text = DetailsView1.Rows[ i ].Cells[ 1 ].Text.Replace( "&nbsp;","");
                switch (n[0])
                {
                    case "Company":
                    case "EmployeeId":
                        Ssql = "Select ";
                        Ssql += n[0] == "Company" ? " CompanyName From Company Where Company" : "EmployeeName From Personnel_Master Where EmployeeId";
                        Ssql += " = '" + n[1] + "'";
                        DT2 = _MyDBM.ExecuteDataTable(Ssql);
                        if (DT2.Rows.Count > 0)
                            DetailsView1.Rows[i].Cells[1].Text += " " + DT2.Rows[0][0].ToString().Trim();
                        break;

                    case "OldnewCode":
                        DetailsView1.Rows[i].Cells[1].Text = n[1].Replace("1", "新制").Replace("2", "舊制");
                        break;

                    case "Emp_rate":
                    case "CompanyRate":
                        try {
                            decimal decTemp = 0;
                            decTemp = Convert.ToDecimal(DetailsView1.Rows[i].Cells[1].Text);
                            if (decTemp < 1)
                                DetailsView1.Rows[i].Cells[1].Text = (decTemp * 100).ToString("N0");
                        }
                        catch { }
                        DetailsView1.Rows[i].Cells[1].Text +=  "%";
                        break;

                    case "EffectiveDate":
                    case "NewSystem_Date":
                        DetailsView1.Rows[i].Cells[1].Text = SysSet.FormatDate(n[1]);
                        break;

                    case "MonthlyActualSalary":
                    case "ActualTotalamount_S":
                    case "ActualTotalamount_C":
                        decimal s;
                        if ( decimal.TryParse ( n[ 1 ] , out s ) )
                        {
                            DetailsView1.Rows[ i ].Cells[ 1 ].Text = s.ToString ( "#,0" );
                        }
                        break;
                }

                DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();
                DetailsView1.Rows[i].Cells[0].Width = 200;

            }
           
        }

    }    
}
