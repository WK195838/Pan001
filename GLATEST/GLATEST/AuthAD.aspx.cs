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
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.IO;
using System.DirectoryServices;

public partial class AuthAD : System.Web.UI.Page
{
    SysSetting SysSet = new SysSetting();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request.RawUrl.Length > (Request.RawUrl.IndexOf("?") + 1))
        {
            string WebKind = Request.RawUrl.Substring(Request.RawUrl.IndexOf("?") + 1);
            switch (WebKind.ToUpper())
            {
                case "ERP":
                    Page.Theme = "ERP";
                    break;
                case "EPAYROLL":
                    Page.Theme = "ePayroll";
                    break;
                case "EBOS":
                    Page.Theme = "EBOS";
                    break;
                case "INVSYS":
                    Page.Theme = "InvSys";
                    break;
                case "FASYS":
                    Page.Theme = "FASys";
                    break;
            }

            Session["Theme"] = Page.Theme;
        }
        else if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();


        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckAuthorization1.AlwaysCloseSetAuth = true;        
        #region 檢核系統授權
        PanPacificClass.ComputerAuth CA = new PanPacificClass.ComputerAuth();
        if (CA.CheckComputerAuth() == false)
        {
            UserInfo _UserInfo = new UserInfo();
            Response.Write(_UserInfo.SysSet.ErrMsg("AuthError"));
            Response.End();
        }
        #endregion
        if (!Page.IsPostBack)
        {            
            if (Context.User.Identity.Name.Contains("PAN-PACIFIC\\"))
            {
                //
            }

            if (!string.IsNullOrEmpty(Request.QueryString["Mode"]))
            {
                if (Request["Mode"].ToString().Equals("LogOut"))
                {
                    Response.Cookies.Clear();
                }
            }
        }        
    }
    
    protected void Login_Click(object sender, EventArgs e)
    {
        if (txtUsername.Text.Trim().Length == 0)
            errorLabel.Text = "請先輸入帳號!";
        else if (txtPassword.Text.Trim().Length == 0)
            errorLabel.Text = "請先輸入密碼!";
        else
            GoLogin(sender, e);
    }
    protected void GoLogin(object sender, EventArgs e)
    {
        GoLogin(true);
    }
    protected void GoLogin(bool checkPWD)
    {
        errorLabel.Text = "";
        string adPath = SysSet.GetConfigString("AuthSetting2").ToString(); //Path to your LDAP directory server
        //string adPath = "LDAP://DC=PAN-PACIFIC"; //Path to your LDAP directory server
        LdapAuthentication adAuth = new LdapAuthentication(adPath);
        try
        {
            int i = adAuth.IsAuthenticated(txtDomain.Text, txtUsername.Text, txtPassword.Text, Request.UserHostAddress, Session.SessionID, checkPWD);
            if (i == 0)
            {
                string groups = adAuth.GetGroups();

                //Create the ticket, and add the groups.
                bool isCookiePersistent = chkPersist.Checked;
                int tmpMinutes = 60;
                switch (rbCookieTime.SelectedValue)
                { 
                    case "D":
                        tmpMinutes = tmpMinutes * 24;
                        break;
                    case "M":
                        tmpMinutes = tmpMinutes * 24 * 30;
                        break;
                }

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                          txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(tmpMinutes), isCookiePersistent, groups);

                //Encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                
                //Create a cookie, and then add the encrypted ticket to the cookie as data.
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                if (true == isCookiePersistent)
                    authCookie.Expires = authTicket.Expiration;

                //Add the cookie to the outgoing cookies collection.
                Response.Cookies.Add(authCookie);

                
                string setTheme = groups.Remove(groups.IndexOf("|")).ToUpper();
                string[] systemList = setTheme.EndsWith(",") ? setTheme.Remove(setTheme.Length - 1).Split(',') : setTheme.Split(',');
                bool showSys = (setTheme.Contains("SYSTEM") ? (systemList.Length > 2) : (systemList.Length > 1));

                string strSys = "ePayroll,EBOS,InvSys,FASys".ToUpper();
                string strThemes = "ePayroll,EBOS,InvSys,FASys";

                string[] strSysList = strSys.Split(',');
                string[] strThemesList = strThemes.Split(',');

                if (showSys.Equals(false) && strSys.Contains(setTheme.Replace("SYSTEM", "").Replace(",", "").ToUpper()))
                {
                    for (int j = 0; j < strThemesList.Length; j++)
                    {
                        if (setTheme.Replace("SYSTEM", "").Replace(",", "").ToUpper() == strThemesList[j].ToString().ToUpper())
                        {
                            Session["Theme"] = strThemesList[j];
                        }
                    }
                }
                else
                {
                    Session["Theme"] = "ERP";
                }
                                
                string sSalaryDay = SysSet.GetConfigString("SalaryDay");
                int iSalaryDay = 1;
                try
                {
                    iSalaryDay = Convert.ToInt32(sSalaryDay);
                }
                catch { iSalaryDay = 1; }
                Session["SalaryDayStart"] = DateTime.Now.ToString("yyyy/MM/") + "01";
                Session["SalaryDayEnd"] = DateTime.Now.AddMonths(1).AddDays(1 - DateTime.Now.Day).ToString("yyyy/MM/dd");
                if (iSalaryDay > 1)
                {
                    Session["SalaryDayStart"] = DateTime.Now.AddMonths(-1).ToString("yyyy/MM/") + (iSalaryDay + 1).ToString().PadLeft(2, '0');
                    Session["SalaryDayEnd"] = DateTime.Now.ToString("yyyy/MM/") + iSalaryDay.ToString().PadLeft(2, '0');
                }

                Session["SoftwareAuth"] = CheckAuthorization1.blAuth;

                //You can redirect now.
                //登入成功導到主頁
                Application["Domain"] = Request.Url.AbsoluteUri.Replace(Request.RawUrl, "/");
                Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
                #region 另開出無工具列之視窗(開發時停用)
                //StringBuilder sBuilder = new StringBuilder();
                //sBuilder.Append("<script language=javascript>");
                //sBuilder.Append("window.open('");
                //sBuilder.Append(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
                //sBuilder.Append("','' , '");
                //sBuilder.Append("height=");
                //sBuilder.Append("window.screen.availHeight");
                //sBuilder.Append(",width=");
                //sBuilder.Append("window.screen.availWidth");
                ////sBuilder.Append(",top='+(screen.height-window.screen.availHeight)/2 +'");
                ////sBuilder.Append(",left='+(screen.width-window.screen.availWidth)/2+'");
                //sBuilder.Append(",scrollbars=yes,toolbar=no,resizable=yes,directories=no,location=no");
                //sBuilder.Append("');");
                //sBuilder.Append("window.close();");
                //sBuilder.Append("</script>");

                //Page.ClientScript.RegisterClientScriptBlock(typeof(string), "msg", sBuilder.ToString());
                #endregion
            }
            else
            {
                switch (i)
                {
                    case -1:
                        errorLabel.Text = "登入失敗. 帳號或密碼錯誤!請重新輸入.";
                        break;
                    case 1:
                        errorLabel.Text = "登入失敗. 帳號已停用!";
                        break;
                    case 2:
                        errorLabel.Text = "登入失敗. 密碼不正確!";
                        break;
                    case 3:
                        errorLabel.Text = "Authentication did not succeed. Check user name and password.";
                        break;
                    case 4:
                        errorLabel.Text = "密碼錯誤已超過3次!請洽系管理員解除!";
                        break;
                    case 5:
                        errorLabel.Text = "您的授權已到期!請洽泛太科技!";
                        break;
                    case 6:
                        errorLabel.Text = "很抱歉，您未授權使用本網站。請洽系管理員!";
                        break;
                }                
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Error SQL Connect"))
            {
                errorLabel.Text = "資料庫連線失敗!!請洽系統管理員! \n\r<BR>";                
            }
            else
            {
                errorLabel.Text = "驗証失敗!!請洽系統管理員! \n\r<BR>";
                errorLabel.Text += "\n " + ex;
            }
        }
    }
}
