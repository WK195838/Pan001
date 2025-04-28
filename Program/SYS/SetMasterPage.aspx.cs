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

public partial class SetMasterPage : System.Web.UI.Page
{
    string _ProgramId = "SYS002";
    UserInfo _UserInfo = new UserInfo();
    protected void Page_PreInit(object sender, EventArgs e)
    {
        string orgSetting = "";

        orgSetting = Page.MasterPageFile;
        try
        {
            if (Session["MasterPage"] != null)
                Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
        }
        catch { Page.MasterPageFile = orgSetting; }

        orgSetting = Page.Theme;
        try
        {
            if (Session["Theme"] != null)
                Page.Theme = Session["Theme"].ToString();
        }
        catch { Page.Theme = orgSetting; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
            {
                if (Session["MasterPage"] != null)
                    RadioButtonList1.SelectedValue = Session["MasterPage"].ToString();
                else
                    RadioButtonList1.SelectedIndex = 0;
            }
            catch { RadioButtonList1.SelectedIndex = 0; }

            try
            {
                if (Session["Theme"] != null)
                    RadioButtonList2.SelectedValue = Session["Theme"].ToString();
                else
                    RadioButtonList2.SelectedIndex = 0; 
            }
            catch { RadioButtonList2.SelectedIndex = 0; }
        }
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["MasterPage"] = RadioButtonList1.SelectedValue;
    }
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Theme"] = RadioButtonList2.SelectedValue;
    }
    protected void SetPage_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }
}
