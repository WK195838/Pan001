using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.DirectoryServices;
using System.Web.Hosting;
using System.Web.Configuration;

namespace FormsAuth
{
    /// <summary>
    /// LdapAuthentication 的摘要描述
    /// </summary>
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;
        UserInfo User = new UserInfo();
        string defaultSystem = ConfigurationManager.AppSettings["SystemID"].ToString();

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        public string GetAuthSetting()
        {
            return ConfigurationManager.AppSettings["AuthSetting"];
        }

        public bool isADAuth(string AuthSetting)
        {
            return (GetAuthSetting().ToUpper().Equals(AuthSetting.ToUpper()));
        }

        public int IsAuthenticated(string domain, string username, string pwd, string ip, string thisSessionID)
        {
            return IsAuthenticated(domain, username, pwd, ip, thisSessionID, true);
        }

        public int IsAuthenticated(string domain, string username, string pwd, string ip, string thisSessionID, bool CheckPWforAD)
        {
            if (isADAuth("ADAuth"))
            {
                return ADAuth(domain, username, pwd, ip, thisSessionID, CheckPWforAD);
            }

            return FormAuth(domain, username, pwd, ip, thisSessionID);
        }

        //使用DB帳號驗証
        public int FormAuth(string domain, string username, string pwd, string ip, string thisSessionID)
        {            
            return User.CheckLogin(defaultSystem, domain, username, pwd, ip, thisSessionID);
        }

        public int ADAuth(string domain, string username, string pwd, string ip, string thisSessionID)
        {
           return IsAuthenticated(domain, username, pwd, ip, thisSessionID, true);
        }
        public int ADAuth(string domain, string username, string pwd, string ip, string thisSessionID, bool CheckPWforAD)
        {
            int i = -1;
            string strMsg = "";
            try
            {
                string domainAndUsername = domain + @"\" + username;
                DirectoryEntry entry;
                if (CheckPWforAD == true)
                {
                    entry = new DirectoryEntry(_path, domainAndUsername, pwd);
                    //ForBeBug
                    strMsg += entry.Username.ToString() + "<br>";
                }
                else
                {
                    entry = new DirectoryEntry(_path);
                    //ForBeBug
                    strMsg += entry.Path.ToString() + "<br>";
                }                

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                //search.Filter = "(&(cn=" + username + "))";
                //ForBeBug
                strMsg += search.Filter.ToString() + "<br>";
                search.PropertiesToLoad.Add("cn");
                //ForBeBug
                strMsg += search.SearchRoot.Username + "<br>";
                try
                {
                    SearchResult result = search.FindOne();

                    //Bind to the native AdsObject to force authentication.
                    object obj = entry.NativeObject;

                    if (null == result)
                    {
                        return i;
                    }

                    //Update the new path to the user in the directory.
                    _path = result.Path;
                    _filterAttribute = (string)result.Properties["cn"][0];
                }
                catch (Exception ex1)
                {
                    throw new Exception("AD驗証機制未啟用! \r\n<BR>" + strMsg + ex1);
                    return i;
                }
                //於完成檢核後寫入DB,供後續呼叫服務時檢核是否已登入用(EBOSWS為指定登入之系統代號)
                User.AfterLogged(defaultSystem, domain, username, username, ip, thisSessionID);
                i = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("AD驗証失敗! \r\n<BR>" + strMsg + ex);
            }

            return i;
        }

        public string GetGroups()
        {
            StringBuilder groupNames = new StringBuilder();

            if (isADAuth("ADAuth"))
            {
                using (HostingEnvironment.Impersonate())
                {
                    // This code runs as the application pool user
                    int flag = 0;
                    try
                    {
                        DirectorySearcher search = new DirectorySearcher(_path);
                        search.Filter = "(cn=" + _filterAttribute + ")";
                        search.PropertiesToLoad.Add("memberOf");

                        try
                        {
                            SearchResult result = search.FindOne();
                            int propertyCount = result.Properties["memberOf"].Count;
                            string dn;
                            int equalsIndex, commaIndex;

                            for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                            {
                                dn = (string)result.Properties["memberOf"][propertyCounter];
                                equalsIndex = dn.IndexOf("=", 1);
                                commaIndex = dn.IndexOf(",", 1);
                                if (-1 == equalsIndex)
                                {
                                    return null;
                                }
                                groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                                groupNames.Append("|");
                            }

                            return groupNames.ToString();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error obtaining group names.\r\n<BR> " + ex + "\r\n<BR> 1." + search.Filter + "\r\n<BR> 2." + search.SearchRoot.Path + "\r\n<BR> ");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error GetGroups.\r\n<BR> flag=" + flag.ToString() + "\r\n<BR> Error:" + ex + "\r\n<BR> 1._path=" + _path + "\r\n<BR> 2._filterAttribute=" + _filterAttribute + "\r\n<BR> ");
                    }
                }
            }
            else
            {
                return SetGroups(User);
            }

            return "";
        }

        public string SetGroups(UserInfo theUser)
        {
            StringBuilder groupNames = new StringBuilder();
            groupNames.Append(theUser.UData.SystemNo);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.CompanyCode);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.CompanyName);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.UserId);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.UserName);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.Role);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.Company);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.EmployeeId);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.SessionID);
            groupNames.Append("|");
            groupNames.Append(theUser.UData.PWDdueDate.ToString("yyyy/MM/dd"));
            return groupNames.ToString();
        }
    }
}