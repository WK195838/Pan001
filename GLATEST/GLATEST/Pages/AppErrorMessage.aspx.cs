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

public partial class AppErrorMessage : System.Web.UI.Page
{
    SysSetting SysSet = new SysSetting();

    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();

        //if (Session["MasterPage"] != null)
        //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       Label1.Text = "";
       Label2.Text = "";
       if (Session["LastError"] != null)
       {
           SystemException se = (SystemException)Session["LastError"];
           Label1.Text = Page.Request.QueryString["aspxerrorpath"];
           Response.Write(se.Message);
           Session["LastError"] = null;
       }
       else
       {
           if (Request.QueryString["ErrMsg"] != null)
           {
               string Code = Request.QueryString["ErrMsg"].ToString();
               string Message = Code;
               //Response.Write(Request.QueryString["ErrMsg"].ToString());
               Label1.Text = Code + ":" + SysSet.ErrMsg(Code); ;
           }
       }
    }
}
