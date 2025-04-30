using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GLADetail : System.Web.UI.MasterPage
{
    UserInfo _UserInfo = new UserInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1a", Page.ResolveUrl("~/Scripts/jquery-1.8.0.min.js").ToString());
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2a", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.23.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());

        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        //用於執行等待畫面
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());
    
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "Ba1", Page.ResolveUrl("~/Scripts/progress.js").ToString());
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "Ba2", Page.ResolveUrl("~/Scripts/jquery-ui-sliderAccess.js").ToString());
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "Ba3", Page.ResolveUrl("~/Scripts/jquery-ui-timepicker-addon.js").ToString());
    }
    protected void lbhome_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://ibosdvlp/WebSite/Systems.aspx", true);
    }
}
