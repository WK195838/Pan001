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

public partial class GL_GLA0110T : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strlocate = "";
        Session["VoucherNo"] = "";
        Session["OPMode"] = "New";
        Session["FromURL"] = "";

        if (Request["A"] != null)
        {
            strlocate ="?A="+ Request["A"].ToString();        
        }

        //Response.Redirect("~/GL/GLA0110N.aspx");
        Server.Transfer("~/GL/GLA0110N.aspx"+strlocate, false);
    }
}
