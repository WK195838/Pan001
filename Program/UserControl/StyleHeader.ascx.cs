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

public partial class StyleHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    string _Width = "100%";
    public string Width
    {
        set
        {
            _Width = value;
        }
    }
    protected override void Render(HtmlTextWriter writer)
    {
        string sTmp = Request.ApplicationPath.ToLower().Replace("\\", "/") + "/";
        string s = Request.RawUrl.ToLower().Replace(sTmp, "");
        if (s.IndexOf("?") > 0) s = s.Remove(s.IndexOf("?"));
        string sPath="";
        if (s.IndexOf("/") < 0)
        {
            sPath = "App_Themes/images/space.gif";
        }
        else
        {
            string[] atemp = s.Replace("//", "/").Split('/');
            sTmp = "";
            for (int i = 0; i < atemp.Length - 1; i++)
            {
                sTmp += "../";
            }
            sPath = sTmp + "App_Themes/images/space.gif";
        }
        string style = @"        <table border='0' cellpadding='0' cellspacing='0' class='borderTable' style='width: {0}'>
            <tr>
                <td class='pageBorderTL' >
                    <img alt=''  src='{1}'  /></td>
                <td class='pageBorderT' >
                    <img alt=''  src='{1}'  /></td>
                <td class='pageBorderTR' >
                    <img alt=''  src='{1}'  /></td>
            </tr>
            <tr>
                <td class='pageBorderL' >
                    <img alt='' height='1' src='space.gif' width='1' /></td>
                <td class='pageBorderC' >
";
        base.Render(writer);
        writer.Write(String.Format(style, _Width, sPath));
    }

}
