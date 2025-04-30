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

public partial class StyleContentEnd : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected override void Render(HtmlTextWriter writer)
    {
        string style = @"                            </td>
                        </tr>
                    </table>
";
        base.Render(writer);
        writer.Write(style);
    }

}
