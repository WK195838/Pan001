using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Principal;
using PanPacificClass;
/// <summary>
/// UserInfo 的摘要描述
/// 2014/02/05 AfterLogged調整：將AD登入後對應方式從EMAIL改為員編[EmployeeId]比對自動產生之登入名稱[UserName]
/// </summary>
public class UserInfo
{    
    private string _SystemNo;
    private string _CompanyCode;
    private string _UserId;
    private string _UserName;
    private string _IP;
    private string _SessionID;
    private string _Company;
    private string _EmployeeId;
    private DateTime _PWDdueDate;
    public bool AuthLogin;
    public SysSetting SysSet = new SysSetting();
    public UserData UData = new UserData();

	public UserInfo()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
        AuthLogin = GetLoginCookie();
	}

    /// <summary>
    /// 檢核登入
    /// </summary>
    public int CheckLogin(string vSystemNo, string vCompanyCode, string vUserId, string vPwd, string vIP, string thisSessionID)
    {
        DBManger dbm = new DBManger();
        dbm.New();
        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "(1=2)";
        if (vSystemNo.Equals("ERP"))
        {
            theSiteList = " SiteId = SiteId ";
        }
        else
        {
            theSiteList = "";
            for (int i = 0; i < strSiteIDList.Length; i++)
            {
                if (i > 0)
                    theSiteList += ",";
                theSiteList += "'" + strSiteIDList[i] + "'";
            }

            theSiteList = " SiteId in (" + theSiteList + ") ";
        }

        //檢核IP
        bool bl_checkIP = true;
        string MySQL = " SELECT IsNull(DnyNetWork,'') As DnyNetWork,IsNull(AlwNetWork,'') As AlwNetWork FROM UC_Company " +
            " Where " + theSiteList +
            " And [Enable] = 1 And CompanyCode='" + vCompanyCode + "' ";
        string ls_AlwNetWork, ls_DnyNetWork, tempIP;

        DataTable result = dbm.ExecuteDataTable(MySQL);
        if (result != null)
        {
            if (result.Rows.Count > 0)
            {
                bl_checkIP = false;

                int tempIndex = 0;
                ls_AlwNetWork = result.Rows[0]["AlwNetWork"].ToString();
                ls_DnyNetWork = result.Rows[0]["DnyNetWork"].ToString();

                if (!string.IsNullOrEmpty(ls_AlwNetWork))
                {
                    string[] aryAlwNetWork = ls_AlwNetWork.Split(new char[] { ';' });

                    for (int i = 0; i < aryAlwNetWork.Length; i++)
                    {
                        tempIndex = aryAlwNetWork[i].IndexOf("*");
                        if (tempIndex > 0)
                        {
                            tempIP = aryAlwNetWork[i].Substring(0, tempIndex);
                        }
                        else
                        {
                            tempIP = aryAlwNetWork[i];
                        }

                        if (!string.IsNullOrEmpty(tempIP))
                        {
                            if (vIP.Substring(0, tempIP.Length).Equals(tempIP))
                            {
                                bl_checkIP = true;
                            }
                        }
                    }
                }
                else
                {
                    bl_checkIP = true;
                }

                if (!string.IsNullOrEmpty(ls_DnyNetWork))
                {
                    string[] aryDnyNetWork = ls_DnyNetWork.Split(new char[] { ';' });

                    for (int i = 0; i < aryDnyNetWork.Length; i++)
                    {
                        tempIndex = aryDnyNetWork[i].IndexOf("*");
                        if (tempIndex > 0)
                        {
                            tempIP = aryDnyNetWork[i].Substring(0, tempIndex);
                        }
                        else
                        {
                            tempIP = aryDnyNetWork[i];
                        }

                        if (!string.IsNullOrEmpty(tempIP))
                        {
                            if (vIP.Substring(0, tempIP.Length).Equals(tempIP))
                            {
                                bl_checkIP = false;
                            }
                        }
                    }
                }
                else
                {
                    bl_checkIP = true;
                }


                if (string.IsNullOrEmpty(ls_DnyNetWork) && string.IsNullOrEmpty(ls_DnyNetWork))
                    bl_checkIP = true; 
            }
        }

        if (bl_checkIP.Equals(false))
        {
            SysSet.WriteToLogs("LoginError", "CompanyCode=" + vCompanyCode + "|UserId=" + vUserId + "|IP=" + vIP + "\n\r theSiteList={" + theSiteList + "}");
            
            return 6;
        }

        MySQL = " Select u.*,Convert(varchar,Password) as chkPW " +
            " ,Company ,EmployeeId " +
            " From [UC_User] u Left Join PersonnelSecurity PS On PS.CompanyCode=u.CompanyCode And PS.ERPID=u.UserId " +
            " Where " + theSiteList +
            " And u.CompanyCode = (Case '" + vCompanyCode + "' When '' Then u.CompanyCode Else '" + vCompanyCode + "' End) " +
            " And UserId='" + vUserId + "' ";
        result = dbm.ExecuteDataTable(MySQL);

        //登入狀態預設為-1,表示無此帳號
        int iState = -1;

        if (result != null)
        {
            if (result.Rows.Count > 0)
            {//找出符合的帳號
                //先將登入狀態設為2,表示密碼檢核未通過
                iState = 2;

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if (result.Rows[i]["chkPW"].ToString().Equals(SysSet.rtnHash(vPwd)))
                    {//找出密碼符合的帳號
                        if (result.Rows[i]["Enable"].ToString().Equals("True"))
                        {//找出有效的帳號
                            if (((int)result.Rows[i]["ErrLoginCnt"]) < 4)
                            {//錯誤次數未超過3次
                                if (((DateTime)result.Rows[i]["PWD_due_Date"]).CompareTo(DateTime.Today) > 0)
                                {//錯誤次數未超過3次

                                    //登入狀態設為正常
                                    iState = 0;
                                    _SystemNo = result.Rows[i]["SiteId"].ToString().Trim() + "," + _SystemNo;
                                    _CompanyCode = result.Rows[i]["CompanyCode"].ToString().Trim();
                                    _UserId = result.Rows[i]["UserID"].ToString().Trim();
                                    _UserName = result.Rows[i]["UserName"].ToString().Trim();
                                    _IP = vIP;
                                    _SessionID = thisSessionID;
                                    _Company = result.Rows[i]["Company"].ToString().Trim();
                                    if (_Company.Length == 0)
                                    {
                                        _Company = SysSet.GetConfigString("DefCompany");
                                    }
                                    _EmployeeId = result.Rows[i]["EmployeeId"].ToString().Trim();
                                    //多系統可用時,密碼到期日以最長者為到期日
                                    if (_PWDdueDate.CompareTo((DateTime)result.Rows[i]["PWD_due_Date"]) == -1)
                                        _PWDdueDate = (DateTime)result.Rows[i]["PWD_due_Date"];

                                    //寫入登入時間與紀錄LOG
                                    AfterLogged();

                                    //改為檢核所有系統,以找出所有符合登入條件的系統
                                    ////停止檢核
                                    //i = result.Rows.Count;
                                }
                                else
                                {                                    
                                    //密碼已過期
                                    if (iState != 0)
                                        iState = 5;
                                }
                            }
                            else 
                            {
                                //錯誤次數已超過3次
                                if (iState != 0)
                                    iState = 4;
                            }
                        }
                        else
                        {//登入狀態設為1,表未啟用
                            if (iState != 0)
                                iState = 1;
                        }
                    }
                }

                if (iState == 0)
                {
                    CheckTransaction dd = new CheckTransaction ( );
                    dd.ct ( );                    
                    _SystemNo += "," + vSystemNo;
                    MySQL = " Update [UC_User] Set ErrLoginCnt=0 Where " + theSiteList + " " +
                        " And CompanyCode = (Case '" + vCompanyCode + "' When '' Then CompanyCode Else '" + vCompanyCode + "' End) " +
                        " And UserId='" + vUserId + "' ";
                    dbm.ExecuteCommand(MySQL);
                }
                else if (iState == 2)
                {
                    MySQL = " Update [UC_User] Set ErrLoginCnt=ErrLoginCnt+1 Where " + theSiteList + " " +
                        " And CompanyCode = (Case '" + vCompanyCode + "' When '' Then CompanyCode Else '" + vCompanyCode + "' End) " +
                        " And UserId='" + vUserId + "' ";
                    dbm.ExecuteCommand(MySQL);
                }

                SysSet.WriteToLogs("Login", "CompanyCode=" + vCompanyCode + "|UserId=" + vUserId + "|State=" + iState.ToString() + "\n\r theSiteList={" + theSiteList + "}");
            }
        }

        if (iState > 0 && vUserId.Equals(SysSet.GetConfigString("testid")) && vPwd.Equals(SysSet.GetConfigString("testpw")) && (vUserId.Trim().Length * vPwd.Trim().Length) != 0)
        {//ForDeBug使用
            SysSet.WriteToLogs("Login", "DeBugCode=" + vUserId + "|IP=" + vIP);
            return 0;
        } 

        return iState;
    }

    /// <summary>
    /// 檢核是否已登入
    /// </summary>
    private void AfterLogged()
    {
        AfterLogged(_SystemNo, _CompanyCode, _UserId, _UserName, _IP, _SessionID);
    }

    /// <summary>
    /// 寫入登入時間與紀錄LOG
    /// </summary>
    public void AfterLogged(string vSystemNo, string vCompanyCode, string vUserId, string vUserName, string vIP, string thisSessionID)
    {        
        DBManger dbm = new DBManger();
        dbm.New();

        UData.SystemNo = vSystemNo;
        UData.CompanyCode = vCompanyCode;
        UData.UserId = vUserId;
        UData.UserName = vUserName;
        UData.IP = vIP;
        UData.SessionID = thisSessionID;
        UData.Company = _Company;
        UData.EmployeeId = _EmployeeId;
        UData.PWDdueDate = _PWDdueDate;

        string MySQL;

        if (SysSet.GetConfigString("AuthSetting1").ToUpper().Equals(vCompanyCode.ToUpper()))
        {
            try
            {
                MySQL = " SELECT [Company],[EmployeeId],[EmployeeName],[EnglishName],[CompanyCode],UC.[UserId],PM.[Email],UC.[Email],IsNull(PM.ResignCode,'N') " +
                " FROM [Personnel_Master] PM left join [UC_User] UC " +
                    //2014/02/05 將對應方式從EMAIL改為員編[EmployeeId]比對自動產生之登入名稱[UserName]
                " On ([EmployeeName]=[UserName] Or [UserName]=[EmployeeId]) and [CreateUser]='SYSDefault' " +
                " Where UC.UserId = '" + vUserId + "' And UC.CompanyCode = 'PAN-PACIFIC' ";
                DataTable dt2 = dbm.ExecuteDataTable(MySQL);

                _Company = dt2.Rows[0]["Company"].ToString().Trim();
                _EmployeeId = dt2.Rows[0]["EmployeeId"].ToString().Trim();
                UData.Company = _Company;
                UData.EmployeeId = _EmployeeId;
                UData.CompanyCode = dt2.Rows[0]["CompanyCode"].ToString().Trim();

                MySQL = " And U.CompanyCode = '" + UData.CompanyCode + "' ";
            }
            catch {
                MySQL = " And U.CompanyCode = '" + vCompanyCode + "' ";
            }
        }
        else
        {
            if (SysSet.GetConfigString("AuthSetting").ToUpper().Equals("ADAuth".ToUpper()))
            {
                MySQL = " And C.CompanyName = '" + vCompanyCode + "' ";
                _SystemNo = vSystemNo;
                _CompanyCode = vCompanyCode;
                _UserId = vUserId;
                _SessionID = thisSessionID;
            }
            else
            {
                MySQL = " And U.CompanyCode = '" + vCompanyCode + "' ";
            }
        }
        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "";
        for (int i = 0; i < strSiteIDList.Length; i++)
        {
            if (i > 0)
                theSiteList += ",";
            theSiteList += "'" + strSiteIDList[i] + "'";
        }

        MySQL = " SELECT U.[CompanyCode],[CompanyName],[RoleName],[UserName] " +
            " FROM [UC_User] U join UC_Company C On U.[SiteId]=C.[SiteId] And U.[CompanyCode]=C.[CompanyCode] " +
            " join UC_RoleMember R On U.[SiteId]=R.[SiteId] And U.[CompanyCode]=R.[CompanyCode] And U.[UserId]=R.[UserORRole] " +
            " Where U.SiteId in (" + theSiteList + ") " + MySQL + " " +
            " And U.UserId = '" + _UserId + "'";
        DataTable dt = dbm.ExecuteDataTable(MySQL);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                UData.CompanyCode = dt.Rows[0]["CompanyCode"].ToString().Trim();
                _CompanyCode = dt.Rows[0]["CompanyCode"].ToString().Trim();
                UData.CompanyName = dt.Rows[0]["CompanyName"].ToString().Trim();
                UData.Role = dt.Rows[0]["RoleName"].ToString().Trim();
                UData.UserName = dt.Rows[0]["UserName"].ToString().Trim();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                        UData.Role += "," + dt.Rows[i]["RoleName"].ToString().Trim();
                }
            }
        }

        //寫入登入紀錄時,只記第一筆符合的帳號
        MySQL = " Update UC_User Set LastLogin=GetDate(),ErrLoginCnt=0 Where SiteId = '" + strSiteIDList[0] + "'" +
            " And CompanyCode = '" + _CompanyCode + "'" +
            " And UserId = '" + _UserId + "'";
        dbm.ExecuteCommand(MySQL);

        //將之前同一使用者以相同SESSION登入之紀錄設為無效
        MySQL = " Update [UC_UserLoginLog] " +
            " Set [OutTime]=GetDate() " +
            " Where [SessionID]='" + _SessionID + "' And [CompanyCode]='" + _CompanyCode + "' And [UserID]='" + _UserId +
            "' And [OutTime] is Null And DateDiff(mi,[InTime],GetDate()) > 2 ";
        dbm.ExecuteCommand(MySQL);

        //將本次登入SESSION寫入登入表中
        MySQL = " INSERT INTO [UC_UserLoginLog] " +
            " ([SessionID],[SiteID],[CompanyCode],[UserID],[LoginIP],[InTime]) VALUES (" +
            " '" + _SessionID + "','" + strSiteIDList[0] + "','" + _CompanyCode + "'" +
            " ,'" + _UserId + "','" + vIP + "',GetDate()" +
            ")";
        dbm.ExecuteCommand(MySQL);
    }

    /// <summary>
    /// Session驗証,24小時內有效
    /// </summary>
    /// <param name="strSession">登入Session</param>
    /// <returns>Session是否有效</returns>
    public bool IsAuthorization(string strSession)
    {
        bool blIsAuthorization = false;
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string strSQL = " Select count(*) from [UC_UserLoginLog] " +
            " Where [SessionID] = '" + strSession.Replace(";", "").Replace(" ", "").Replace("=", "") + "'" +
            " And [OutTime] is null And DateDiff(HH,[InTime],GetDate()) < 24 ";

        DataTable Dt = _MyDBM.ExecuteDataTable(strSQL);
        if (Dt != null)
            if (Dt.Rows.Count > 0)
                if (Convert.ToInt32(Dt.Rows[0][0].ToString()) > 0)
                    blIsAuthorization = true;
        return blIsAuthorization;
    }

    private bool GetLoginCookie()
    {
        string cookieName = FormsAuthentication.FormsCookieName;

        HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

        if (null == authCookie)
        {
            //There is no authentication cookie.
            return false;
        }
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        }
        catch (Exception ex)
        {
            //Write the exception to the Event Log.
            return false;
        }
        if (null == authTicket)
        {
            //Cookie failed to decrypt.
            return false;
        }
        //When the ticket was created, the UserData property was assigned a
        //pipe-delimited string of group names.
        string[] groups = authTicket.UserData.Split(new char[] { '|' });

        _UserId = authTicket.Name;
        try
        {
            if (SysSet.GetConfigString("AuthSetting").ToUpper().Equals("ADAuth".ToUpper()))
            {
                _SystemNo = groups[0];
                _CompanyCode = groups[1];
                _UserName = groups[2];
                UData.SystemNo = (_SystemNo.Equals("ERP")) ? "ePayroll,EBOS" : _SystemNo;
                UData.CompanyCode = (_CompanyCode.Length >= 2) ? _CompanyCode.Substring(0, 2) : _CompanyCode;
                UData.CompanyName = _CompanyCode;
                UData.UserId = _UserId;
                UData.UserName = _UserName;
                if (_UserId.ToLower().Equals("adm01"))
                    UData.Role = "Administrator";
                else
                    UData.Role = "user";
            }
            else
            {                
                UData.SystemNo = groups[0];
                _SystemNo = groups[0];
                UData.CompanyCode = groups[1];
                _CompanyCode = groups[1];
                UData.CompanyName = groups[2];                
                UData.UserId = groups[3];
                UData.UserName = groups[4];
                _UserName = groups[4];
                UData.Role = groups[5];
                UData.Company = groups[6];
                _Company = groups[6];
                UData.EmployeeId = groups[7];
                _EmployeeId = groups[7];
                UData.PWDdueDate = Convert.ToDateTime(groups[9]);
                _PWDdueDate = Convert.ToDateTime(groups[9]);
            }
        }
        catch (Exception ex)
        {
            //Write the exception to the Event Log.
            return false;
        }

        return true;
    }

    /// <summary>
    /// 單一系統使用:取得程式使用權限
    /// </summary>
    /// <param name="vProgramId">程式代號</param>
    /// <returns></returns>
    public bool CheckPermission(string vProgramId)
    {
        return CheckPermission(UData.SystemNo, _CompanyCode, _UserId, vProgramId, "Execute");
    }

    /// <summary>
    /// 單一系統使用:檢核程式功能權限
    /// </summary>
    /// <param name="vProgramId">程式代號</param>
    /// <param name="vRight">功能代號</param>
    /// <returns></returns>
    public bool CheckPermission(string vProgramId, string vRight)
    {
        return CheckPermission(UData.SystemNo, _CompanyCode, _UserId, vProgramId, vRight);
    }

    /// <summary>
    /// 多系統使用:取得程式使用權限
    /// </summary>
    /// <param name="vSystemNo">系統代號</param>
    /// <param name="vProgramId">程式代號</param>
    /// <returns></returns>
    public bool CheckSysPermission(string vSystemNo, string vProgramId)
    {
        return CheckPermission(vSystemNo, _CompanyCode, _UserId, vProgramId, "Execute");
    }

    /// <summary>
    /// 多系統使用:檢核程式功能權限
    /// </summary>
    /// <param name="vSystemNo">系統代號</param>
    /// <param name="vProgramId">程式代號</param>
    /// <param name="vRight">功能代號</param>
    /// <returns></returns>
    public bool CheckPermission(string vSystemNo, string vProgramId, string vRight)
    {
        if (vRight == "") vRight = "Execute";
        return CheckPermission(vSystemNo, _CompanyCode, _UserId, vProgramId, vRight);
    }

    /// <summary>
    /// 多系統使用:檢核程式使用權限
    /// </summary>
    /// <param name="vSystemNo">系統代號</param>
    /// <param name="vCompanyCode">公司代號</param>
    /// <param name="vUserId">使用者代號</param>
    /// <param name="vProgramId">程式代號</param>
    /// <returns></returns>
    public bool CheckPermission(string vSystemNo, string vCompanyCode, string vUserId, string vProgramId)
    {
        return CheckPermission(vSystemNo, vCompanyCode, vUserId, vProgramId, "Execute");
    }

    /// <summary>
    /// 檢核程式使用與功能權限
    /// </summary>
    /// <param name="vSystemNo">系統代號</param>
    /// <param name="vCompanyCode">公司代號</param>
    /// <param name="vUserId">使用者代號</param>
    /// <param name="vProgramId">程式代號</param>
    /// <param name="vRight">功能代號</param>
    /// <returns></returns>
    public bool CheckPermission(string vSystemNo, string vCompanyCode, string vUserId, string vProgramId, string vRight)
    {
        if (string.IsNullOrEmpty(vSystemNo) || string.IsNullOrEmpty(vCompanyCode) || string.IsNullOrEmpty(vUserId) || string.IsNullOrEmpty(vProgramId) || string.IsNullOrEmpty(vRight))
        {
            return false;
        }

        DBManger dbm = new DBManger();
        dbm.New();
        string MySQL = "";
        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "";
        for (int i = 0; i < strSiteIDList.Length; i++)
        {
            if (i > 0)
                theSiteList += ",";
            theSiteList += "'" + strSiteIDList[i] + "'";
        }
        MySQL = " SELECT  * " +
            " FROM UC_UserAuthority " +
            " Where SiteId in (" + theSiteList + ") And CompanyCode = @CompanyCode " +
            " And (UserOrRole in (Select RoleName From UC_RoleMember Where SiteId in (" + ((strSiteIDList.Length > 1) ? theSiteList : "'" + vSystemNo + "'") + ") And CompanyCode = @CompanyCode And UserORRole = @UserId) Or UserOrRole = @UserId)" +
            " And AuthDefn = 1 And ProgramId = @ProgramId And RightName = (Case @Right When 'Execute' Then RightName Else @Right End) " +
            " And SiteId in (select SiteId from [UC_Site] where [Enable] = 1) ";

        SqlCommand MyCmd = new SqlCommand();
        //MyCmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 16).Value = vSystemNo;
        MyCmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 16).Value = vCompanyCode;
        MyCmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 32).Value = vUserId;
        MyCmd.Parameters.Add("@ProgramId", SqlDbType.NVarChar, 32).Value = vProgramId;
        MyCmd.Parameters.Add("@Right", SqlDbType.NVarChar, 32).Value = vRight;

        DataTable result = dbm.ExecuteDataTable(MySQL, MyCmd.Parameters, CommandType.Text);
        if (result != null)
        {
            if (result.Rows.Count > 0)
                return true;
        }
        return false;
    }
}
