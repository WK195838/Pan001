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

public partial class DataChangeLog_I : System.Web.UI.Page
{
    String Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB016";
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

        DataTable DT = _MyDBM.ExecuteDataTable("Select ProgramId FROM UC_Program Where SiteId = '" + _UserInfo.UData.SystemNo + "' And RTrim(ProgramPath)='Basic/DataChangeLog.aspx'");
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
                blCheckProgramAuth = _UserInfo.CheckPermission(_ProgramId, "Detail");
                if (blCheckProgramAuth == false)
                    ShowMsgBox1.Message = _UserInfo.SysSet.ErrMsg("NoRight");
            }
        }
    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < DetailsView1.Rows.Count; i++)
        {//修改欄位顯示名稱
            string strName = DetailsView1.Rows[i].Cells[0].Text.Trim();
            switch (strName)
            {
                case "TableName":
                    strName = "目標(表格或網頁)";
                    break;
                case "TrxType":
                    strName = "使用類型";
                    break;
                case "ChangItem":
                    strName = "使用項目";
                    break;
                case "ChgUser":
                    strName = "使用者";
                    break;
                case "SQLcommand":
                    strName = "其它訊息";
                    break;                    
                case "ChgStartDateTime":
                    strName = "開始時間";
                    break;
                case "ChgStopDateTime":
                    strName = "結束時間";
                    break;
            }

            Ssql = "Select dbo.GetColumnTitle('DataChangeLog','" + DetailsView1.Rows[i].Cells[0].Text + "')";
            DataTable DT = _MyDBM.ExecuteDataTable(Ssql);
            
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0].ToString().Trim().Contains("時間"))
                {//數字欄位需靠右對齊
                    TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                    tc.Style.Add("text-align", "right");
                }

                if (DetailsView1.Rows[1].Cells[1].Text.Trim().Equals("Q") || DetailsView1.Rows[1].Cells[1].Text.Trim().Equals("Q-查詢"))
                    DetailsView1.Rows[i].Cells[0].Text = strName;
                else
                    DetailsView1.Rows[i].Cells[0].Text = DT.Rows[0][0].ToString().Trim();

                if (i == 1)
                {
                    switch (DetailsView1.Rows[i].Cells[1].Text.Trim()) 
                    {
                        case "A":
                            strName = "新增";
                            break;
                        case "U":
                            strName = "修改";
                            break;
                        case "D":
                            strName = "刪除";
                            break;
                        case "Q":
                            strName = "查詢";
                            break;
                    }
                    DetailsView1.Rows[i].Cells[1].Text += "-" + strName;
                }

                DetailsView1.Rows[i].Cells[0].Wrap = false;
                if (DT.Rows[0][0].ToString().Trim().Contains("時間"))
                {//將日期欄位格式化
                    try
                    {
                        TableCell tc = (TableCell)DetailsView1.Rows[i].Cells[1];
                        tc.Text = _UserInfo.SysSet.FormatDate(tc.Text) + " " + Convert.ToDateTime(tc.Text).ToString("HH:mm:ss");
                    }
                    catch { }
                }
            }
        }
    }    
}
