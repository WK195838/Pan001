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

public partial class ShowMsgBox : System.Web.UI.UserControl
{
    /// <summary>
    ///  MessageBox控制項,適合在採用Altas時使用
    /// </summary>

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string sJscript = @"<script language='javascript'>
<!--
function MsgBox()
[[
    //    alert('test');
    if(document.all.{0} != null)
    [[
    if(document.all.{0}.value != document.all.{1}.value)
    [[
        if (document.all.{0}.value == '')
            return;
        alert(document.all.{0}.value);
        document.all.{0}.value = document.all.{1}.value;
    ]]
    ]]
]]
function InitMsgBox()
[[
    setTimeout('MsgBox()',{2});
]]
document.onclick=InitMsgBox;
setTimeout('MsgBox()',1000);
//setInterval('MsgBox()',{2});

// -->
</script>";
            sJscript = String.Format(sJscript, Msg1.UniqueID, Msg2.UniqueID, _Interval.ToString());
            sJscript = sJscript.Replace("[[", "{");
            sJscript = sJscript.Replace("]]", "}");
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "MsgBox", sJscript);
            //Page.ClientScript.RegisterHiddenField("MsgBox_Msg1", _msg.Value);
        }
        else
        {
            //_msg = (HtmlInputHidden)FindControl("MsgBox_Msg1");
        }
    }

    private int _Interval = 1000;
    public int Interval
    {
        set { _Interval = value; }
        get { return _Interval; }
    }
	
    public string Message
    {
        get 
        { 
            return Msg1.Value; 
        }
        set 
        { 
            Msg1.Value = value;
        }
    }
	
}
