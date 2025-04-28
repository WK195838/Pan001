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
using FormsAuth;
using System.Text;

public partial class AuthADLogin : System.Web.UI.Page
{
    SysSetting SysSet = new SysSetting();
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        if (!Login1.UserName.Contains("@"))
        {
            string strDomain = SysSet.GetConfigString("AuthSetting2").ToString();
            try
            {
                if (strDomain.IndexOf("://") > 0)
                    strDomain = strDomain.Substring(strDomain.IndexOf("://")+3);
                if (strDomain.IndexOf("/") > 0)
                    strDomain = strDomain.Remove(strDomain.IndexOf("/"));
            }
            catch { }
            Login1.UserName = Login1.UserName + "@" + strDomain;
        }
    }

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {   
        string adPath = SysSet.GetConfigString("AuthSetting2").ToString(); //Path to your LDAP directory server
        LdapAuthentication adAuth = new LdapAuthentication(adPath);
        UserInfo _UC = new UserInfo();
        _UC.AfterLogged(SysSet.GetConfigString("SystemID").ToString(), SysSet.GetConfigString("AuthSetting1").ToString(), Login1.UserName.Remove(Login1.UserName.IndexOf("@")), Login1.Password, Request.UserHostAddress, Session.SessionID);
        if (_UC.UData.UserId != null)
        {
            #region 寫入Cookie
            string groups = adAuth.SetGroups(_UC);

            //Create the ticket, and add the groups.
            bool isCookiePersistent = false; //Login1.RememberMe.Checked;
            int tmpMinutes = 60;
            //switch (rbCookieTime.SelectedValue)
            //{
            //    case "D":
            //        tmpMinutes = tmpMinutes * 24;
            //        break;
            //    case "M":
            //        tmpMinutes = tmpMinutes * 24 * 30;
            //        break;
            //}

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                      Login1.UserName, DateTime.Now, DateTime.Now.AddMinutes(tmpMinutes), isCookiePersistent, groups);

            //Encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //Create a cookie, and then add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            if (true == isCookiePersistent)
                authCookie.Expires = authTicket.Expiration;

            //Add the cookie to the outgoing cookies collection.
            Response.Cookies.Add(authCookie);
            #endregion
            Response.Redirect("MyPayroll/PayrollToPDF.aspx");
        }
    }
}