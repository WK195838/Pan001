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
using PanPacificClass;

namespace FormsAuth
{
    public class Authorization
    {
        DBManger _MyDBM;
        UserInfo _UserData;
        public Authorization()
        {
            _MyDBM = new DBManger();
            _MyDBM.New();
            _UserData = new UserInfo();
        }

        /// <summary>
        /// Session驗証,24小時內有效
        /// </summary>
        /// <param name="strSession">登入Session</param>
        /// <returns>Session是否有效</returns>
        public bool IsAuthorization(string SessionId)
        {
            bool blIsAuthorization = false;
            blIsAuthorization = _UserData.IsAuthorization(SessionId);
            return blIsAuthorization;
        }

        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <param name="SessionId"></param>
        /// <param name="LogoutUser"></param>
        public void UserLogOut(string SessionId, UserData LogoutUser)
        {
            _UserData.UserLogOut(SessionId, LogoutUser);
        }

        /// <summary>
        /// 取得登入者資訊
        /// </summary>
        /// <param name="SessionId"></param>
        /// <returns></returns>
        public UserData UData(string SessionId)
        {
            return _UserData.GetUData(SessionId);
        }

        public string GetAuthorization(string CompanyCode, string UserID, string PWD, string IP, string SessionID, bool NoCheckPwd)
        {
            string path = "";
            LdapAuthentication Ldap = new LdapAuthentication(path);
            return Ldap.GetAuthorization(CompanyCode, UserID, PWD, IP, SessionID, true, NoCheckPwd);
        }
    }
    /// <summary>
    /// LdapAuthentication 的摘要描述
    /// </summary>
    public class LdapAuthentication
    {
        private string _path;
        private string _filterAttribute;
        UserInfo User = new UserInfo();
        string defaultSystem = ConfigurationManager.AppSettings["SystemID"].ToString();

        public int theAuthKind()
        {
            int iAuthKind = 3;
            string theAuth = GetAuthSetting();
            switch (theAuth)
            {
                case "ADAuth":
                    iAuthKind = 1;
                    break;
                case "IntegratedAuth":
                    iAuthKind = 2;
                    break;
                default:
                    break;
            }
            return iAuthKind;
        }

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

        public int IsAuthenticated(string domain, string username, string pwd, string ip, string thisSessionID, bool NoCheckPwd)
        {
            return IsAuthenticated(domain, username, pwd, ip, thisSessionID, true, NoCheckPwd);
        }

        public int IsAuthenticated(string domain, string username, string pwd, string ip, string thisSessionID, bool CheckPWforAD, bool NoCheckPwd)
        {
            int iStatus = -1;
            int iAuthKind = theAuthKind();
            switch (iAuthKind)
            {
                case 1:
                    //iStatus = ADAuth(domain, username, pwd, ip, thisSessionID, CheckPWforAD);
                    DBManger dbm = new DBManger();
                    dbm.New();
                    string MySQL = "";
                    if (iStatus == 0)
                    {
                        //清空錯誤次數
                        MySQL = " Update UC_User Set LastLogin=GetDate(),0 Where UserId = '" + username + "'";
                        dbm.ExecuteCommand(MySQL);
                    }
                    else if (pwd != "" && iStatus != 0)
                    {
                        //寫入錯誤次數
                        MySQL = " Update UC_User Set ErrLoginCnt=ErrLoginCnt+1 Where UserId = '" + username + "'";
                        dbm.ExecuteCommand(MySQL);
                    }
                    break;
                case 2:
                    //iStatus = ADAuth(domain, username, pwd, ip, thisSessionID, CheckPWforAD);
                    if (pwd != "" && iStatus < 0)
                        iStatus = FormAuth(domain, username, pwd, ip, thisSessionID, NoCheckPwd);
                    break;
                default:
                    iStatus = FormAuth(domain, username, pwd, ip, thisSessionID, NoCheckPwd);
                    break;
            }
            return iStatus;
        }
        public string GetAuthorization(string CompanyCode, string UserID, string PWD, string IP, string SessionID, bool NoCheckPwd)
        {
            return GetAuthorization(CompanyCode, UserID, PWD, IP, SessionID, true, NoCheckPwd);
        }

        //使用DB帳號驗証
        public int FormAuth(string domain, string username, string pwd, string ip, string thisSessionID, bool NoCheckPwd)
        {
            return User.CheckLogin(defaultSystem, domain, username, pwd, ip, thisSessionID, NoCheckPwd);
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

        public string GetAuthorization(string CompanyCode, string UserID, string PWD, string IP, string SessionID, bool CheckPWforAD, bool NoCheckPwd)
        {
            string strError = "";

            try
            {
                int i = IsAuthenticated(CompanyCode, UserID, PWD, IP, SessionID, CheckPWforAD, NoCheckPwd);
                if (i == 0)
                {
                    string groups = GetGroups();
                    //登入成功傳回登錄資訊
                    strError = groups;
                }
                else
                {
                    switch (i)
                    {
                        case -99:
                            strError = "資料庫驗証失敗!!請洽系統管理員!";
                            break;
                        case -2:
                            strError = "資料庫連線失敗!!請洽系統管理員!";
                            break;
                        case -1:
                            strError = "登入失敗. 帳號或密碼錯誤!請重新輸入.";
                            break;
                        case 1:
                            strError = "登入失敗. 帳號已停用!";
                            break;
                        case 2:
                            strError = "登入失敗. 密碼不正確!";
                            break;
                        case 3:
                            strError = "Authentication did not succeed. Check user name and password.";
                            break;
                        case 4:
                            strError = "密碼錯誤已超過" + ConfigurationManager.AppSettings["AuthSetting3"].ToString() + "次!請洽系管理員解除!";
                            break;
                        case 5:
                            strError = "您的授權已到期!請洽泛太科技!";
                            break;
                        case 6:
                            strError = "很抱歉，您未授權使用本網站。請洽系管理員!";
                            break;
                        default:
                            strError = i.ToString() + "</br>很抱歉，您未授權使用本網站。請洽系管理員!";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Error SQL Connect"))
                {
                    strError = "資料庫連線失敗!!請洽系統管理員! \n\r<BR>";
                }
                else
                {
                    strError = "驗証失敗!!請洽系統管理員! \n\r<BR>";
                    strError += "\n " + ex;
                }
            }

            return strError;
        }

        
    }
}