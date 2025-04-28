using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ExportOBTDTABonus : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYE003";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();
    int DLShowKind = 2;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        lbl_Msg.Text = "";
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        // 需要執行等待畫面的按鈕
        btnQuery.Attributes.Add("onClick", "drawWait('')");
        
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
            try
            {//設定下拉單是否顯示代碼或預設未選項
                DLShowKind = int.Parse(_UserInfo.SysSet.GetConfigString("DLShowKind"));
            }
            catch { }

            CostId.SetCodeList("PY#Bonus", DLShowKind, "全部");

            BindData();            
        }
        //else
        //{
        //    BindData();
        //}
        //if (Session["ExportDT"] != null)
        //    btnToExcel.Attributes.Add("onclick", "return GVExportToExcel('" + Session["ExportDT"].GetType().Name + "');");               

        
        ScriptManager1.RegisterPostBackControl(btnQuery);
        tbTransDate.CssClass = "JQCalendar";
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else if (string.IsNullOrEmpty(tbTransDate.Text))
        {
            LabMsg.Text = "請輸入獎金發放日期";            
        }
        else if (_UserInfo.SysSet.CheckChineseLen(tbCompanyMemo.Text.Trim(), 2) > 12)
        {
            LabMsg.Text = "公司備註字數過長!最長可輸入6個中文字/全形字或12個半形字(可混用)";
        }
        else if (_UserInfo.SysSet.CheckChineseLen(tbBankMemo.Text.Trim(), 2) > 12)
        {
            LabMsg.Text = "行員備註字數過長!最長可輸入6個中文字/全形字或12個半形字(可混用)";
        }
        else
        {                                   
            GridView1.Visible = true;
            DataTable theDT = BindData();
            if (theDT != null && theDT.Rows.Count > 0)
            {
                GridView1.DataSource = theDT;
                GridView1.DataBind();
            }
            else
            {
                GridView1.Visible = false;
            }

            if (GridView1.Rows.Count == 0)
                lbl_Msg.Text = "查無資料!!";
            else
                lbl_Msg.Text = "";
        }
    }


    protected void btnToTXT_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else if (_UserInfo.SysSet.CheckChineseLen(tbCompanyMemo.Text.Trim(), 2) > 12)
        {
            LabMsg.Text = "公司備註字數過長!最長可輸入6個中文字/全形字或12個半形字(可混用)";
        }
        else if (_UserInfo.SysSet.CheckChineseLen(tbBankMemo.Text.Trim(), 2) > 12)
        {
            LabMsg.Text = "行員備註字數過長!最長可輸入6個中文字/全形字或12個半形字(可混用)";
        }
        else
        {
            DataTable theDT = BindData();
            if (theDT != null && theDT.Rows.Count > 0)
            {
                lbl_Msg.Text = "";
                string path, FileName;
                path = Server.MapPath(Request.Url.AbsolutePath);
                path = path.Remove(path.LastIndexOf("\\"));
                FileName = "OBTDTA_Bonus.txt";
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(path + (path.EndsWith("\\") ? "" : "\\") + FileName);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                for (int i = 0; i < theDT.Rows.Count; i++)
                {//將轉匯資料寫入文字檔
                    _UserInfo.SysSet.WriteTofiles(path, FileName, theDT.Rows[i][0].ToString());
                    //Request.Url.ToString().Trim()
                }

                //網頁轉址
                //this.Response.Redirect(Request.Url.ToString().Trim().Remove(Request.Url.ToString().Trim().LastIndexOf("/") + 1) + "OBTDTA.txt");
                //另開視窗
                string strUrl = "window.open('" + Request.Url.ToString().Trim().Remove(Request.Url.ToString().Trim().LastIndexOf("/") + 1) + FileName + "');";
                ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "GetTXT", strUrl, true);
            }
            else if (GridView1.Rows.Count == 0)
                lbl_Msg.Text = "查無資料!!";
        }        
    }

    private DataTable BindData()
    {
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.ExportOBTDTAKey";        
        py.BeforeQuery(PayrollLt.DeCodeKey);
        string Ssql = "";
        DataTable theDT = null;
        try
        {
            Ssql = "sp_PY_OBTDTA_Bonus";

            //獎金名目
            MyCmd.Parameters.Clear();
            MyCmd.Parameters.Add("@ls_CompanyId", SqlDbType.Char, 2).Value = CompanyList1.SelectValue.Trim();
            MyCmd.Parameters.Add("@ls_CostId", SqlDbType.VarChar, 20).Value = CostId.SelectedCode.Trim();
            MyCmd.Parameters.Add("@ls_TransDate", SqlDbType.Char, 8).Value = _UserInfo.SysSet.FormatADDate(tbTransDate.Text.Trim()).Replace("/", "");
            MyCmd.Parameters.Add("@ls_CompanyMemo", SqlDbType.VarChar, 12).Value = tbCompanyMemo.Text.Trim();
            MyCmd.Parameters.Add("@ls_BankMemo", SqlDbType.VarChar, 12).Value = tbBankMemo.Text.Trim();
            MyCmd.Parameters.Add("@ls_A1", SqlDbType.Char, 2).Value = (tbWeights.Text.Trim().Length != 0) ? tbWeights.Text.Trim() : "20";
            MyCmd.Parameters.Add("@ls_A2", SqlDbType.Char, 2).Value = (tbWeights.Text.Trim().Length != 0) ? tbWeights.Text.Trim() : "20";

            theDT = _MyDBM.ExecStoredProcedure(Ssql, MyCmd.Parameters);
        }
        catch { }
        py.AfterQuery(PayrollLt.DeCodeKey);
        return theDT;
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

                //if ((i + 1) == e.Row.Cells.Count)
                if (i > 4)
                {
                    e.Row.Cells[i].Style.Add("text-align", "right");
                }
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
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時                    
                }
                else
                {
                    //查詢時                   
                    ////DataRowView tehDRV = (DataRowView)DataBinder.GetDataItem(e.Row);
                }

                break;

            case DataControlRowType.Header:

                 break;
        }
    }
}
