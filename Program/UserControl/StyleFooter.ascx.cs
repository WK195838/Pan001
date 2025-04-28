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

public partial class StyleFooter : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void Render(HtmlTextWriter writer)
    {
        string sTmp = Request.ApplicationPath.ToLower().Replace("\\", "/") + "/";
        string s = Request.RawUrl.ToLower().Replace(sTmp, "");
        if (s.IndexOf("?") > 0) s = s.Remove(s.IndexOf("?"));
        string sPath;
        if (s.IndexOf("/") < 0)
        {
            sPath = "App_Themes/images/space.gif";
        }
        else
        {
            string[] atemp = s.Replace("//", "/").Split('/');
            sTmp = "";
            for (int i = 0; i < atemp.Length-1; i++)
            {
                sTmp += "../";
            }
            sPath = sTmp + "App_Themes/images/space.gif";
        }
        string style = @"                </td>
                <td class='pageBorderR' style='height: 41px'>
                    <img alt='' height='1' src='{0}' width='1' /></td>
            </tr>
            <tr>
                <td class='pageBorderBL' style='width: 14px'>
                    <img alt=''  src='{0}' width='14' /></td>
                <td class='pageBorderB'>
                    <img alt='' src='{0}' width='14' /></td>
                <td class='pageBorderBR'>
                    <img alt=''  src='{0}' width='14' /></td>
            </tr>
        </table>
";
        base.Render(writer);
        writer.Write(String.Format(style, sPath));
    }

}
