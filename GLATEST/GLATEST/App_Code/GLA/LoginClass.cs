using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using DBClass;

/// <summary>
/// 登入頁面管理
/// </summary>
public class LoginClass : DB_Base
{
    System.Web.UI.Page _Page;
    StateBag _ViewState;
    private int myVar;
    private bool _EnableAuthCheck = true;

    public int MyProperty
    {
        get { return myVar; }
        set { myVar = value; }
    }

    public AppAuthority appAuth;
    public LoginClass(CommonData common, System.Web.UI.Page page, StateBag viewstate)
        : base(common)
    {
        _Page = page;
        _ViewState = viewstate;
    }

    public bool EnableAuthCheck
    {
        get { return _EnableAuthCheck; }
        set { _EnableAuthCheck = value; }
    }
    /// <summary>
    /// 使用者離開系統，記錄Log
    /// </summary>
    public void LogOut()
    {
        if ((_Common.User.UserId != null) && (_Common.User.UserId != ""))
        {
            //-----暫時Mark-----
            //string sSQL = "Update UC_Login set LogoutDate=getdate() Where LogoutDate Is Null And CompanyCode = '{0}' And UserID = '{1}';";
            string sSQL = "Update UC_UserLoginLog Set OutTime=getdate() Where OutTime Is Null And CompanyCode = '{0}' And UserID = '{1}';";
            sSQL = String.Format(sSQL, _Common.User.CompanyCode, _Common.User.UserId);
            _Common.DB.Execute(sSQL);
            //------------------
        }

        _Common.User = new SystemData.UserData();
        _Common.User.SessionID = "";
        _ViewState.Clear();
        //_Page.Request.Cookies.Clear();
        //_Page.Response.Cookies.Clear();
        //HttpCookie a = new HttpCookie("_logkey");
        HttpCookie a = new HttpCookie(_Common.LogKeyName);
        a.Expires = System.DateTime.Now.AddDays(-1D);
        _Page.Response.Cookies.Add(a);
        _Page.Session.Clear();

        //HttpContext.Current.User = null;
        //_Page.Session.Abandon();
        //FormsAuthentication.SignOut();

        
    }
    /// <summary>
    /// 變更Login記錄(暫不使用)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool ChangeLogin(DBClass.SystemData.UserData user)
    {        
        string LogKey = _Common.User.SessionID;
        bool result = false;
        
        return result;
    }

    public bool CheckLogin()
    {
        string LogKey = _Common.User.SessionID;
        //-----暫時回傳true-----
        //return true;
        //----------------------
        bool result = false;
        if ((LogKey != null) && (LogKey != ""))
        {
            SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_UserLoginLog");
            sql.Add("*");
            sql.Where.CustomWhere = " And OutTime is null";
            sql.Where.Add("SessionID", LogKey);
            sql.Where.Add("InTime", SqlWhere.WhereOperator.And_LessEqual, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            _Common.DR = _Common.DB.Query(sql.ToString());
            MyDataReader myDR = new MyDataReader(_Common.DR);
            if (myDR.Reader())
            {
                myDR.GetData("SessionID", ref _Common.User.SessionID);
                myDR.GetData("UserID", ref _Common.User.UserId);
                myDR.GetData("CompanyCode", ref _Common.User.CompanyCode);
                myDR.GetData("LoginIp", ref _Common.User.IP);

                result = true;
            }
        }
        else
        {
            try
            {
                string userid = _Page.Session["LoginUserId"].ToString();
                GetSystemUserDataManager manager = new GetSystemUserDataManager();
                _Common.User = manager.GetSystemUserData(userid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("{0},{1}", ex.Message, ex.StackTrace);                
            }
        }

        string sProgramName = _Page.Request.AppRelativeCurrentExecutionFilePath;
        sProgramName = sProgramName.Substring(2, sProgramName.Length - 2);
        if (!String.IsNullOrEmpty(_Page.Request.QueryString.ToString()))
            sProgramName += "?" + _Page.Request.QueryString.ToString();
        appAuth = new AppAuthority(_Common, _Common.User.UserId, sProgramName);
        appAuth.CheckAuthority(_Common.User.Role);

        if ((!appAuth.RunApp) && _EnableAuthCheck)
        {
            SystemException sysExecption;

            if (result)
            {
                sysExecption = new SystemException(String.Format("您無本程式[{0}]之執行權,請以有權限之帳號重新登入或洽系統管理員!", sProgramName));
            }
            else
            {
                sysExecption = new SystemException("帳號重複登入或以非法方式執行或是通行證過期,請重新登入!!");
            }

            _Page.Session["LastError"] = sysExecption;

            string sTmp = _Page.Request.ApplicationPath.ToLower().Replace("\\", "/") + "/";
            string s = _Page.Request.RawUrl.ToLower().Replace(sTmp, "");
            string sPath;
            if (s.IndexOf("/") < 0)
            {
                sPath = "AppErrorMessage.aspx";
            }
            else
            {
                sPath = "../pages/AppErrorMessage.aspx";
            }

            _Page.Response.Redirect(sPath);
        }
        return result;
    }
    /// <summary>
    /// 轉換身份
    /// </summary>
    /// <param name="employeePk"></param>
    /// <param name="departmentCode"></param>
    public string TransferInUser(string simulateUserId, int employeePk, string departmentCode)
    {
        string sqlStr;
        int affectRow = 0;
        string logKey = CommonData.NewGuid();
        DateTime loginDate = DateTime.Now;

        //string sSQL = String.Format("Update UC_Login set LogoutDate=getdate() Where LogoutDate Is Null And CompanyCode = '{0}' And UserID = '{1}';"
        //                , _Common.User.CompanyCode, _Common.User.UserID);
        //_Common.DB.Execute(sSQL);

        SqlString sql = new SqlString(SqlString.SqlCommandType.Insert, "UC_UserLoginLog");
        sql.Add("SessionID", logKey);
        sql.Add("SiteID", ConfigurationManager.AppSettings["SiteID"].ToString());
        sql.Add("CompanyCode", _Common.User.CompanyCode);
        sql.Add("UserID", _Common.User.UserId);
        sql.Add("LoginIP", _Common.User.IP);
        sql.Add("InTime", loginDate, SqlString.Date2StrType.DateTime);
        sqlStr = sql.ToString();
        affectRow = _Common.DB.Execute(sqlStr);
        if (affectRow == 0)
        {
            return "";
        }
        _Common.User.SessionID = logKey;

        return logKey;
    }

    private bool InsertLogData(DBClass.SystemData.UserData user)
    {
        bool result = false;
        if (user == null)
        {
            return result;
        }
        SqlString sql;
        //將之前的UserLoginLog記錄皆登出
        string sSQL = "Update UC_UserLoginLog set OutTime=getdate() Where OutTime Is Null And CompanyCode = '{0}' And UserID = '{1}';";
        _Common.DB.Execute(String.Format(sSQL, user.CompanyCode, user.UserId));
        //新增一筆登入記錄
        user.SessionID = CommonData.NewGuid();
        sql = new SqlString(SqlString.SqlCommandType.Insert, "UC_UserLoginLog");

        sql.Add("SessionID", user.SessionID);
        sql.Add("SiteID", ConfigurationManager.AppSettings["SiteID"].ToString());
        sql.Add("CompanyCode", user.CompanyCode);
        sql.Add("UserID", user.UserId);
        sql.Add("LoginIP", (user.IP == null) ? "" : user.IP);
        sql.Add("InTime", DateTime.Now,SqlString.Date2StrType.DateTime);
        if (_Common.DB.Execute(sql.ToString()) > 0)
        {
            result = true;
        }
        return result;
    }

    private string EmuLogin(string CompanyCode, string UserID, string LoginIP)
    {
        SystemData.UserData loginUser;
        string logKey = "";

        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "View_UC_User");
        sql.Add("*");
        sql.Where.Add("UserID", UserID);
        sql.Where.Add("CompanyCode", CompanyCode);
        sql.Where.CustomWhere = string.Format(" And Enable={0}", true);

        _Common.DR = _Common.DB.Query(sql.ToString());

        loginUser = this.GetUserData(_Common.DR);

        if (InsertLogData(loginUser))
        {
            _Common.User = loginUser;
            logKey = _Common.User.SessionID;
        }

        return logKey;

    }

    /// <summary>
    ///  無帳號登入
    /// </summary>
    /// <returns></returns>
    public string NoIDLogin(string LoginIP)
    {
        SystemData.UserData loginUser = new SystemData.UserData();
        loginUser.Role = "guest";
        loginUser.UserName = "訪客";
        loginUser.UserId = "guest";
        loginUser.UserName = "訪客";
        loginUser.SessionID = "";
        loginUser.CompanyCode = "";
        loginUser.SystemNo = "";
        loginUser.CompanyCode = "";
        loginUser.CompanyName = "";
        loginUser.Company = "";
        loginUser.DeptId = "";
        loginUser.DeptName = "";
        loginUser.DeptUnid = "";
        loginUser.EmployeeId="";
        loginUser.PWDdueDate = DateTime.Now;
        loginUser.IP = LoginIP;

        return NoIDLogin(loginUser);
    }
    /// <summary>
    ///  無帳號登入
    /// </summary>
    /// <param name="loginUser"></param>
    /// <returns></returns>
    public string NoIDLogin(DBClass.SystemData.UserData loginUser)
    {
        string logKey = "";

        if (InsertLogData(loginUser))
        {
            _Common.User = loginUser;
            logKey = _Common.User.SessionID;
        }

        return logKey;
    }

    public string Login(string CompanyCode, string UserID, string Password, string LoginIP)
    {
        SystemData.UserData loginUser;
        string logKey = "";
        Password = LoginClass.Encryp(Password);

        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "View_UC_User");
        sql.Add("*");
        sql.Where.Add("UserID", UserID);
        sql.Where.Add("Password", Password);
        sql.Where.Add("CompanyCode", CompanyCode);
        sql.Where.CustomWhere = string.Format(" And Enable={0}", true);

        _Common.DR = _Common.DB.Query(sql.ToString());

        loginUser = this.GetUserData(_Common.DR);

        if (loginUser != null)
        {
            loginUser.IP = LoginIP;
            if (InsertLogData(loginUser))
            {
                _Common.User = loginUser;
                logKey = _Common.User.SessionID;
            }
        }
       
        return logKey;
    }

    public string Login(string CompanyCode, string UserID,  string LoginIP)
    {
        SystemData.UserData loginUser;
        string logKey = "";

        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "View_UC_User");
        sql.Add("*");
        sql.Where.Add("UserID", UserID);
        sql.Where.Add("CompanyCode", CompanyCode);
        //sql.Where.CustomWhere = string.Format(" And Enable={0}", true);
        sql.Where.Add("Enable", "1");

        _Common.DR = _Common.DB.Query(sql.ToString());
        loginUser = this.GetUserData(_Common.DR);
        if (loginUser != null)
        {
            loginUser.IP = LoginIP;
            if (InsertLogData(loginUser))
            {
                _Common.User = loginUser;
                logKey = _Common.User.SessionID;
            }
        }

        return logKey;
    }
    /// <summary>
    /// 將DataReader轉成SystemData.UserData物件
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private DBClass.SystemData.UserData GetUserData(IDataReader dr)
    {
        SystemData.UserData data = null;
        MyDataReader myDR = new MyDataReader(dr);
        if (myDR.Reader())
        {
            data = new SystemData.UserData();

            myDR.GetData("CompanyCode", ref data.CompanyCode);
            myDR.GetData("UserId", ref data.UserId);
            myDR.GetData("UserName", ref data.UserName);
            myDR.GetData("Company", ref data.Company);
            myDR.GetData("EmployeeId", ref data.EmployeeId);
            myDR.GetData("DeptId", ref data.DeptId);
            myDR.GetData("DeptName", ref data.DeptName);

            data.SystemNo = ConfigurationManager.AppSettings["SystemID"].ToString();
        }

        return data;
    }
    /// <summary>
    /// 文字加解
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    static public string Encryp(string password)
    {
        string result = "";
        string hashType = System.Configuration.ConfigurationManager.AppSettings["EncryptionWay"];

        if (hashType != "Clear")
        {
            result = FormsAuthentication.HashPasswordForStoringInConfigFile(password, hashType);
        }
        else
        {
            result = password;
        }
        return result;
    }
    /// <summary>
    /// 檢查帳號, 密碼
    /// </summary>
    /// <param name="common"></param>
    /// <param name="companyCode"></param>
    /// <param name="userID"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    static public bool CheckPassword(CommonData common, string companyCode, string userID, string password)
    {
        bool result = false;
        try
        {
            password = LoginClass.Encryp(password);

            SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_User");
            sql.Add("*");
            sql.Where.Add("UserID", userID);
            sql.Where.Add("Password", password);
            sql.Where.Add("CompanyCode", companyCode);
            sql.Where.CustomWhere = string.Format(" And Enable={0}", true);

            common.DR = common.DB.Query(sql.ToString());
            if (common.DR.Read())
            {
                result = true;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return result;
    }
    /// <summary>
    /// 變更密碼
    /// </summary>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <param name="confirmPassword"></param>
    /// <returns></returns>
    public bool ChangePassword(string oldPassword, string newPassword, string confirmPassword)
    {
        bool result = false;
        try
        {
            oldPassword = LoginClass.Encryp(oldPassword);
            newPassword = LoginClass.Encryp(newPassword);
            confirmPassword = LoginClass.Encryp(confirmPassword);

            SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_User");
            sql.Add("*");
            sql.Where.Add("UserID", _Common.User.UserId);
            sql.Where.Add("Password", oldPassword);
            sql.Where.Add("CompanyCode", _Common.User.CompanyCode);
            sql.Where.CustomWhere = string.Format(" And Enable={0}", true);

            _Common.DR = _Common.DB.Query(sql.ToString());
            MyDataReader myDR = new MyDataReader(_Common.DR);
            if (myDR.Reader())
            {
                result = true;
            }
        }
        catch (Exception)
        {

            throw;
        }
        return result;
    }
    /// <summary>
    /// 是否agent已存在
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    //private bool CheckAgentExist(string id)
    //{
    //    bool result = false;
    //    AgentManager am = new AgentManager();

    //    result = am.IsAgentExist(id);
    //    return result;
    //}
    /// <summary>
    /// 檢查UserId是存在於RoleMember中
    /// </summary>
    /// <param name="roleName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool CheckRoleExist(string roleName, string id)
    {
        bool result = false;
        string sql = String.Format("Select * from UC_RoleMember Where RoleName ='{0}' And UserId='{1}'", roleName, id.Replace("'", "''"));
        DataTable dt = _Common.DB.Query(sql, "UC_RoleMember");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    /// <summary>
    /// 是否帳號已存在
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool CheckAccountExist(string userId)
    {
        bool result = false;
        if (!string.IsNullOrEmpty(userId))
        {
            SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_User");
            sql.Add("*");
            sql.Where.Add("UserID", userId);

            DataTable dt = _Common.DB.Query(sql.ToString(), "UC_User");
            if (dt.Rows.Count > 0)
            {
                result = true;
            }
        }

        return result;
    }
  
    //private bool InsertAgent(AgentData agent)
    //{
    //    bool result = true;
    //    AgentManager am = new AgentManager();
    //    int affectRow = 0;
    //    affectRow = am.Insert(agent);

    //    if (affectRow == 0)
    //    {
    //        result = false;
    //    }

    //    return result;
    //}
    
    private bool InsertRole(string roleName, string id)
    {
        SqlString sql = new SqlString(SqlString.SqlCommandType.Insert, "UC_RoleMember");
        bool result = true;
        int affectRow = 0;

        sql.Add("RoleName", roleName);
        sql.Add("UserID", id);
        affectRow = _Common.DB.Execute(sql.ToString());
        if (affectRow == 0)
        {
            result = false;
        }

        return result;
    }

    private bool InsertAccount(SystemData.UserData user)
    {
        SqlString sql = new SqlString(SqlString.SqlCommandType.Insert, "UC_User");
        bool result = true;
        int affectRow = 0;
        user.UserId = user.UserId.ToUpper().Trim();
        sql.Add("CompanyCode", user.CompanyCode);
        sql.Add("SiteId", ConfigurationManager.AppSettings["SiteID"].ToString());
        sql.Add("UserId", user.UserId);
        sql.Add("UserName", user.UserName);
        sql.Add("Password", LoginClass.Encryp(user.UserId));
        sql.Add("RootMenu", "SD1000M");
        sql.Add("Enable", true);
        affectRow = _Common.DB.Execute(sql.ToString());

        if (affectRow == 0)
        {
            result = false;
        }

        return result;
    }

    //public string EnumAccountLogin(SystemData.UserData user, string defaultRoleName)
    //{
    //    string result = string.Empty;
    //    bool ret = true;
    //    AgentData agentData;
    //    AgentManager am = new AgentManager();

    //    //若帳號不存在,新增
    //    if (!this.CheckAccountExist(user.UserID))
    //    {                //若Agent不存在,新增
    //        if (!this.CheckAgentExist(user.ID))
    //        {
    //            agentData = new AgentData();
    //            agentData.AgentID = user.ID;
    //            agentData.Unit = user.DepartmentCode;
    //            agentData.AgentName = user.Name;
    //            agentData.UpdateTime = DateTime.Now;

    //            ret = this.InsertAgent(agentData);

    //            if (!ret)
    //            {
    //                _err = true;
    //                _message = "業務員資料新增失敗";
    //                return string.Empty;
    //            }
    //        }
    //        else
    //        {
    //            _err = true;
    //            _message = "業務員重覆";
    //            return string.Empty;
    //        }

    //        agentData = am.GetAgents(user.ID, user.DepartmentCode);

    //        user.Employee_PK = agentData.AgentNumber;

    //        //新增帳號
    //        ret = this.InsertAccount(user);
    //        if (ret)
    //        {
    //            //若角色不存在,新增
    //            if (!this.CheckRoleExist(defaultRoleName, user.UserID))
    //            {
    //                ret = this.InsertRole(defaultRoleName, user.UserID);
    //                if (!ret)
    //                {
    //                    _err = true;
    //                    _message = "角色新增失敗";
    //                    return string.Empty;
    //                }
    //            }
    //        }
    //    }

    //    //模擬user登入系統
    //    result = this.EmuLogin(user.CompanyCode, user.UserID, user.LoginIp);
    //    return result;
    //}
}
