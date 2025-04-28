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
using FormsAuth;


public partial class DataChangeLog : System.Web.UI.Page
{
    string Ssql = "";
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "PYB016";
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();

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

    private void AuthRight()
    {
        //驗證權限
        bool Find = false;
        bool SetCss = false;
        int i = 0;

        //string[] Auth = { "Delete", "Modify", "Detail", "Add" };
        string[] Auth = { "Detail" };

        if (ConfigurationManager.AppSettings["EnableProgramAuth"] != null && ConfigurationManager.AppSettings["EnableProgramAuth"].ToString() == "true")
        {            
            for (i = 0; i < Auth.Length; i++)
            {
                Find = _UserInfo.CheckPermission(_ProgramId, Auth[i]);
                if (i < (Auth.Length - 1))
                {//刪/修/詳
                    GridView1.Columns[i].Visible = Find;
                    //設定標題樣式
                    if (Find && (SetCss == false))
                    {
                        SetCss = true;
                        GridView1.Columns[i].HeaderStyle.CssClass = "paginationRowEdgeLl";
                    }
                }
                //else
                //{//新增
                //    btnNew.Visible = Find;
                //    btnEmptyNew.Visible = Find;
                //}
            }

            //查詢(執行)
            if ((_UserInfo.CheckPermission(_ProgramId)) || Find)
            {
                Find = true;
            }
            else
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }

            //版面樣式調整
            if (SetCss == false)
            {
                GridView1.Columns[(Auth.Length - 1)].HeaderStyle.CssClass = "paginationRowEdgeLl";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", @"JQ();", true);
        lbl_Msg.Text = "";//清空訊息

        Navigator1.BindGridView = GridView1;

        try
        {
            if (CodeList1.SelectedCode == null)
            {
                CodeList1.SetCodeList("SYS#Trx", 2);
            }
        }
        catch
        {
            CodeList1.SetCodeList("SYS#Trx", 2);
        }

        try
        {
            if (CodeList2.SelectedCode == null)
            {
                CodeList2.SetCodeList("SYS#CgItem", 2);
            }
        }
        catch
        {
            CodeList2.SetCodeList("SYS#CgItem", 2);
        } 

        if (!Page.IsPostBack)
        {
            CodeList1.SetCodeList("SYS#Trx", 2);
            CodeList2.SetCodeList("SYS#CgItem", 2);

            if (_UserInfo.AuthLogin == false)
            {
                Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=UnLogin");
            }
            else
            {
                if (_UserInfo.CheckPermission(_ProgramId) == false)
                    Response.Redirect("\\" + Application["WebSite"].ToString() + "\\" + Application["ErrorPage"].ToString() + "?ErrMsg=NoRight");
            }
            BindData();
            AuthRight();            
        }
        else
        {
            BindData();
        }
        //"資料報表標題", "條件名稱1,條件內容1,條件名稱2,條件內容2...", "資料欄位名稱,若未傳入則使用DB中的欄位描述,(必須是單一資料表才找得出)", "SELECT * FROM HOLIDAY"
        string strHeaderLine, strQueryLine, strDataTitel, strDataBody;
        strHeaderLine = "'資料異動紀錄'";
        //strQueryLine = "'異動別：,'+document.getElementById('" + tbTrxType.ClientID + "').value+',異動目標：,'+document.getElementById('" + tbQuery.ClientID + "').value";
        strQueryLine = "'使用類型：," + CodeList1.SelectedCodeName.Trim() + "";
        strQueryLine += ",使用項目：," + CodeList2.SelectedCodeName.Trim() + "";
        strQueryLine += ",使用期間：," + txtDateS.Text + "~" + txtDateE.Text + "";
        strQueryLine += ",使用目標：," + tbQuery.Text + "'";
        strDataTitel = "''";
        strDataBody = "'" + _UserInfo.SysSet.rtnHash(SDS_GridView.SelectCommand).Replace('\\', '＼') + "'";
        btnToExcel.Attributes.Add("onclick", "return ExportExcel(" + strHeaderLine + "," + strQueryLine + "," + strDataTitel + "," + strDataBody + ");");
        //btnToExcel.Attributes.Add("onclick", "return ExportExcel('資料異動紀錄','異動別：,,異動目標：,','','" + _UserInfo.SysSet.rtnHash(SDS_GridView.SelectCommand).Replace('\\', '＼') + "');");

        //設定日期開窗元件        
        txtDateS.CssClass = "JQCalendar";
        txtDateE.CssClass = "JQCalendar";
    }
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton link;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                string strDateTime = e.Row.Cells[5].Text.ToString().Trim();
                try
                {
                    strDateTime = Convert.ToDateTime(strDateTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch { }

                link = (LinkButton)e.Row.FindControl("btnSelect");
                link.Attributes.Add("onclick", "javascript:var win =window.open('DataChangeLog_I.aspx?" +
                    "TableName=" + GridView1.DataKeys[e.Row.RowIndex].Values["TableName"].ToString().Trim() +
                    "&TrxType=" + GridView1.DataKeys[e.Row.RowIndex].Values["TrxType"].ToString().Trim() +
                    "&ChangItem=" + e.Row.Cells[3].Text.ToString().Trim() +
                    "&ChgUser=" + e.Row.Cells[4].Text.ToString().Trim() +
                    "&ChgStartDateTime=" + strDateTime +
                    "','','height=400px,width=600px,scrollbars=yes,top=100px,left=100px,resizable=yes');");

                if ((e.Row.RowState == System.Web.UI.WebControls.DataControlRowState.Edit)
                || (e.Row.RowState == (System.Web.UI.WebControls.DataControlRowState.Alternate | System.Web.UI.WebControls.DataControlRowState.Edit)))
                {
                    //修改時
                }
                else
                {
                    //目標
                    string strName = "";
                    strName = e.Row.Cells[1].Text.Trim();
                    strName = DBSetting.TableName(strName);
                    if (strName.Length > 0)
                        e.Row.Cells[1].Text = strName + "<BR>&nbsp;&nbsp;&nbsp;&nbsp;(" + e.Row.Cells[1].Text.Trim() + ")";

                    //使用類型
                    strName = "";
                    switch (e.Row.Cells[2].Text.Trim())
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
                    e.Row.Cells[2].Text += "-" + strName;

                    //使用項目
                    strName = "";
                    switch (e.Row.Cells[3].Text.Trim())
                    {
                        case "Browse":
                            strName = "瀏覽頁面";
                            break;
                        case "Query":
                            strName = "使用查詢功能";
                            break;
                        case "ALL":
                            strName = "所有欄位";
                            break;
                    }
                    if (strName.Length > 0)
                        e.Row.Cells[3].Text += "-" + strName;

                    //使用者                    
                    strName = DBSetting.ERPIDName("", e.Row.Cells[4].Text.Trim());
                    //if ((strName.Length > 0) && (!e.Row.Cells[4].Text.Trim().Equals(strName)))
                    if (strName.Length > 0)
                        e.Row.Cells[4].Text += "-" + strName;

                    //時間換行
                    //e.Row.Cells[e.Row.Cells.Count - 1].Text = _UserInfo.SysSet.FormatDate(strDateTime) + "<BR>&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToDateTime(strDateTime).ToString("HH:mm:ss");
                    //不換行
                    e.Row.Cells[e.Row.Cells.Count - 1].Text = _UserInfo.SysSet.FormatDate(strDateTime) + Convert.ToDateTime(strDateTime).ToString(" HH:mm:ss");                    
                    //查詢時
                    for (int i = 1; i < e.Row.Cells.Count; i++)
                    {
                        
                        //文字靠左,日期與數字欄位靠右對齊補空格
                        if (i == e.Row.Cells.Count - 1)
                            e.Row.Cells[i].Text = e.Row.Cells[i].Text + "&nbsp;&nbsp;&nbsp;&nbsp;";
                        else
                            e.Row.Cells[i].Text = "&nbsp;&nbsp;&nbsp;&nbsp;" + e.Row.Cells[i].Text;
                    }
                }

                break;

            case DataControlRowType.Header:
                break;
        }
    }

    private void BindData()
    {
        string tVoucherSDate = "", tVoucherEDate = "";

        Ssql = "SELECT * FROM DataChangeLog Where 0=0";
        //if (tbTrxType.Text.Length > 0)
        //{
        //    Ssql += string.Format(" And TrxType like '%{0}%'", tbTrxType.Text.Trim());
        //}
        if (!CodeList1.SelectedCode.Trim().Equals("0"))
        {
            Ssql += string.Format(" And TrxType like '%{0}%'", CodeList1.SelectedCode.Trim());
        }

        if (CodeList2.SelectedCode.Trim().Equals("Other"))
        {
            Ssql += " And Not(ChangItem in ('ALL','Query','Browse'))";
        }
        else if (!CodeList2.SelectedCode.Trim().Equals(""))
        {
            Ssql += string.Format(" And ChangItem like '%{0}%'", CodeList2.SelectedCode.Trim());
        }
        
        if (txtDateS.Text.Trim().Length > 0)
        {
            tVoucherSDate = _UserInfo.SysSet.ToADDate(txtDateS.Text.Trim());
        }
        else
        {
            tVoucherSDate = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
            txtDateS.Text = _UserInfo.SysSet.FormatDate(tVoucherSDate);
        }

        if (txtDateE.Text.Trim().Length > 0)
        {
            tVoucherEDate = _UserInfo.SysSet.ToADDate(txtDateE.Text.Trim());
        }
        else
        {
            tVoucherEDate = DateTime.Today.ToString("yyyy/MM/dd");
            txtDateE.Text = _UserInfo.SysSet.FormatDate(tVoucherEDate);
        }

        Ssql += string.Format(" And Convert(varchar,ChgStartDateTime,111) Between '{0}' And '{1}' ", tVoucherSDate, tVoucherEDate);

        if (tbQuery.Text.Trim().Length > 0)
        {
            Ssql += string.Format(" And (TableName like '%{0}%' Or ChgUser like '%{0}%')", tbQuery.Text.Trim());
        }

        SDS_GridView.SelectCommand = Ssql + " Order By TableName,TrxType,ChgStartDateTime DESC";
        GridView1.DataBind();
        showPanel();

        Navigator1.BindGridView = GridView1;
        //Navigator1.SQL = SDS_GridView.SelectCommand;
        Navigator1.DataBind();
    }

    private void showPanel()
    {
        if (GridView1.Rows.Count > 0)
        {
            Panel_Empty.Visible = false;
        }
        else
        {
            Panel_Empty.Visible = true;
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnToExcel_Click(object sender, EventArgs e)
    {
        //BindData();
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

                //文字靠左,日期與數字欄位靠右對齊
                if (i == e.Row.Cells.Count-1)                
                    e.Row.Cells[i].Style.Add("text-align", "right");
                else
                    e.Row.Cells[i].Style.Add("text-align", "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            //e.Row.Style.Add("display", "none");
            e.Row.Visible = false;
        }
    }
}
