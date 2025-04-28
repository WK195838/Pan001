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

public partial class DownLoad : System.Web.UI.Page
{
    string _ProgramId = "SYS001";
    UserInfo _UserInfo = new UserInfo();
    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
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
        setInitLink();
    }

    private void setInitLink()
    {
        //人事系統操作說明
        dlTable.Rows[0].Cells[0].InnerText = "人事系統操作說明：";
        dlTable.Rows[0].Cells[1].InnerHtml = "<a href='../Query/HRMSHelp.htm' target='_blank' alt=''>下載說明檔</a>";

        if (_UserInfo.CheckPermission(_ProgramId, "UpdatePro"))
        {
            dlTable.Rows[1].Cells[0].InnerText="下載資料庫更新程式：";
            dlTable.Rows[1].Cells[1].InnerHtml = "<a href='DownLoad\\DownLoad01.zip'>下載壓縮檔</a>　<a href='DownLoad\\DownLoad01.exe'>下載自解壓縮檔</a>";
        }
    }
}
