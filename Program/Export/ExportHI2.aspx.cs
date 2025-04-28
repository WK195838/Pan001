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

public partial class ExportHI2 : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    //FORTEST:測試時尚未授權,暫用已有權之程式
    string _ProgramId = "PYE004";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
    Payroll.PayrolList PayrollLt = new Payroll.PayrolList();

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

            SalaryYM1.defaultlist();
            SalaryYM2.defaultlist();
            BindData();            
        }
        //else
        //{
        //    BindData();
        //}
        //if (Session["ExportDT"] != null)
        //    btnToExcel.Attributes.Add("onclick", "return GVExportToExcel('" + Session["ExportDT"].GetType().Name + "');");
        
        ScriptManager1.RegisterPostBackControl(btnQuery);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        LabMsg.Text = "";

        if (string.IsNullOrEmpty(CompanyList1.SelectValue))
        {
            LabMsg.Text = "請選擇先公司!";
        }
        else
        {                                   
            GridView1.Visible = true;
            DataSet theDs = BindData();

            if (theDs != null && theDs.Tables.Count > 1 && theDs.Tables[1].Rows.Count > 0)
            {
                DataTable theDT = theDs.Tables[1];
                GridView1.DataSource = theDT;
                GridView1.DataBind();
            }
            else
            {
                GridView1.Visible = false;
            }

            if (GridView1.Rows.Count == 0 || GridView1.Visible == false)
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
        else
        {
            DataSet theDs = BindData();

            if (theDs != null && theDs.Tables.Count > 1 && theDs.Tables[1].Rows.Count > 0)
            {
                DataTable theDT = theDs.Tables[1];
                lbl_Msg.Text = "";
                string path, FileName;
                path = Server.MapPath(Request.Url.AbsolutePath);
                path = path.Remove(path.LastIndexOf("\\"));
                try
                {
                    FileName = theDs.Tables[0].Rows[0][0].ToString() + ".txt";
                }
                catch {
                    FileName = "DPR00000000" + ((DateTime.Now.Year - 1911) * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day).ToString() + ".txt";
                }
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

    private DataSet BindData()
    {
        DataSet Ds = new DataSet();
        Payroll py = new Payroll();
        PayrollLt.DeCodeKey = "dbo.ExportHI2"+DateTime.Now.ToString("yyyyMMddHHmm");        
        py.BeforeQuery(PayrollLt.DeCodeKey);
        string Ssql = "";
        try
        {
            Ssql = "SP_PRA_ExportHI2";

            MyCmd.Parameters.Clear();            
            MyCmd.Parameters.Add("@ls_CompanyId", SqlDbType.Char).Value = CompanyList1.SelectValue.Trim();
            MyCmd.Parameters.Add("@ls_YMStart", SqlDbType.Char, 5).Value = (Convert.ToInt32(SalaryYM1.SelectSalaryYM) - 191100).ToString();
            MyCmd.Parameters.Add("@ls_YMEnd", SqlDbType.Char, 5).Value = (Convert.ToInt32(SalaryYM2.SelectSalaryYM) - 191100).ToString();
            MyCmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = PayrollLt.DeCodeKey;
            MyCmd.Parameters.Add("@ls_ChgUser", System.Data.SqlDbType.VarChar).Value = _UserInfo.UData.UserId;

            _MyDBM.ExecStoredProcedure(Ssql, MyCmd.Parameters, out Ds);
        }
        catch { }
        py.AfterQuery(PayrollLt.DeCodeKey);
        return Ds;
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
