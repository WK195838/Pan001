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
/// </summary>
public class UserInfo
{
    private string _SystemNo;
    private string _CompanyCode;    //帳號公司別(來源：UC_Company)
    private string _UserId;
    private string _UserName;
    private string _UserUnid;       //20121225 + 
    private string _IP;
    private string _SessionID;
    private string _Company;        //職員公司別(來源：Company)
    private string _EmployeeUnid;   //20121225 +
    private string _EmployeeId;
    private string _EmployeeName;   //20121225 +
    private string _Email;          //20121225 +
    /// <summary>
    /// iBOS系統員工部門代號
    /// </summary>
    private string _DeptId;
    /// <summary>
    /// iBOS系統員工部門識別用單一ID
    /// </summary>
    private string _DeptUnid;
    /// <summary>
    /// iBOS系統員工部門名稱
    /// </summary>
    private string _DeptName;
    private DateTime _PWDdueDate;
    public bool AuthLogin;
    public SysSetting SysSet = new SysSetting();
    public UserData UData = new UserData();
    DBManger dbm = new DBManger();
    string defaultSystem = ConfigurationManager.AppSettings["SystemID"].ToString();

    /// <summary>
    /// 傳回非靜態個資
    /// </summary>
    /// <param name="PD">靜態個資</param>
    /// <returns></returns>
    public PersonalData ToWSPD(DBSetting.PersonalData PD)
    {
        PersonalData WSPD = new PersonalData();

        WSPD.Addr = PD.Addr;
        WSPD.BirthDate = PD.BirthDate;
        WSPD.BloodType = PD.BloodType;
        WSPD.BornPlace = PD.BornPlace;
        WSPD.CCN = PD.CCN;
        WSPD.Company = PD.Company;
        WSPD.CompanyName = PD.CompanyName;
        WSPD.Contact = PD.Contact;
        WSPD.ContactTEL = PD.ContactTEL;
        WSPD.DependentsNum = PD.DependentsNum;
        WSPD.DeptId = PD.DeptId;
        WSPD.DeptName = PD.DeptName;
        WSPD.EducationCode = PD.EducationCode;
        WSPD.Email = PD.Email;
        WSPD.EmployeeId = PD.EmployeeId;
        WSPD.EmployeeName = PD.EmployeeName;
        WSPD.EnglishName = PD.EnglishName;
        WSPD.Grade = PD.Grade;
        WSPD.Guarantor1 = PD.Guarantor1;
        WSPD.Guarantor1TEL = PD.Guarantor1TEL;
        WSPD.Guarantor2 = PD.Guarantor2;
        WSPD.Guarantor2TEL = PD.Guarantor2TEL;
        WSPD.HireDate = PD.HireDate;
        WSPD.Identify = PD.Identify;
        WSPD.IDNo = PD.IDNo;
        WSPD.IDType = PD.IDType;
        WSPD.Introducer = PD.Introducer;
        WSPD.IntroducerTEL = PD.IntroducerTEL;
        WSPD.LeaveWithoutPay = PD.LeaveWithoutPay;
        WSPD.LstChangeSalaryDate = PD.LstChangeSalaryDate;
        WSPD.LstPromotionDate = PD.LstPromotionDate;
        WSPD.LWC = PD.LWC;
        WSPD.MaritalStatus = PD.MaritalStatus;
        WSPD.Military = PD.Military;
        WSPD.MobilPhone = PD.MobilPhone;
        WSPD.Nationality = PD.Nationality;
        WSPD.ObserveExpirationDate = PD.ObserveExpirationDate;
        WSPD.PayCode = PD.PayCode;
        WSPD.ReHireDate = PD.ReHireDate;
        WSPD.ResidenceAddr = PD.ResidenceAddr;
        WSPD.ResignCode = PD.ResignCode;
        WSPD.ResignDate = PD.ResignDate;
        WSPD.Sex = PD.Sex;
        WSPD.Shift = PD.Shift;
        WSPD.SpecialSeniority = PD.SpecialSeniority;
        WSPD.TEL = PD.TEL;
        WSPD.TitleCode = PD.TitleCode;
        WSPD.Union = PD.Union;

        return WSPD;
    }

    public UserInfo()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
        AuthLogin = GetLoginCookie();
        dbm.New();
    }

    /// <summary>
    /// 檢核IP
    /// </summary>
    /// <param name="theSiteList">站台列表</param>
    /// <param name="vCompanyCode">公司代碼</param>
    /// <param name="vIP">IP Address</param>
    /// <returns></returns>
    public int checkIp(string theSiteList, string vCompanyCode, string vUserId, string vIP)
    {
        bool bl_checkIP = true;

        string MySQL = " SELECT IsNull(DnyNetWork,'') As DnyNetWork,IsNull(AlwNetWork,'') As AlwNetWork FROM UC_Company " +
            " Where " + theSiteList +
            " And [Enable] = 1 And CompanyCode='" + vCompanyCode + "' ";
        string ls_AlwNetWork, ls_DnyNetWork, tempIP;
        string ErrMsg = "";
        DataTable result = dbm.ExecuteDataTable(MySQL, out ErrMsg);

        if (ErrMsg != "")
        {
            if (ErrMsg.Contains("Server") || ErrMsg.Contains("Error SQL Connect"))
                return -2;
            else
                return -99;
        }

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

        return 0;
    }

    /// <summary>
    /// 檢核登入
    /// </summary>
    public int CheckLogin(string vSystemNo, string vCompanyCode, string vUserId, string vPwd, string vIP, string thisSessionID, bool NoCheckPwd)
    {
        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "(1=2)";
        if (vSystemNo.Equals(defaultSystem))
        {
            theSiteList = " SiteId = SiteId ";
        }
        else
        {
            theSiteList = "'" + defaultSystem + "'";
            for (int i = 0; i < strSiteIDList.Length; i++)
            {
                if (!theSiteList.Contains("'" + strSiteIDList[i] + "'"))
                    theSiteList += ",'" + strSiteIDList[i] + "'";
            }

            theSiteList = " SiteId in (" + theSiteList + ") ";
        }

        //登入狀態
        int iState = 0;

        //檢核IP
        ////iState = checkIp(theSiteList, vCompanyCode, vUserId, vIP);
        //if (iState != 0)
        //{
        //    return iState;
        //}

        ////登入狀態預設為-1,表示無此帳號
        //iState = -1;

       

        string MySQL = " Select u.*,Convert(varchar,Password) as chkPW " +
            " ,Company ,EmployeeId " +
            " From [UC_User] u Left Join PersonnelSecurity PS On PS.CompanyCode=u.CompanyCode And PS.ERPID=u.UserId " +
            " Where " + theSiteList +
            " And u.CompanyCode = (Case '" + vCompanyCode + "' When '' Then u.CompanyCode Else '" + vCompanyCode + "' End) " +
            " And UserId='" + vUserId + "' ";

        DataTable result =dbm.ExecuteDataTable(MySQL);

        if (result != null)
        {
            iState = 0;
            if (result.Rows.Count > 0)
            {//找出符合的帳號
                //先將登入狀態設為2,表示密碼檢核未通過
                //if (NoCheckPwd == false)
                //    iState = 2;
                //else
                //    iState = 0;

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    //if (NoCheckPwd == false)
                    //{
                    if (result.Rows[i]["chkPW"].ToString().Equals(SysSet.rtnHash(vPwd)))
                    {//找出密碼符合的帳號
                        if (result.Rows[i]["Enable"].ToString().Equals("True"))
                        {//找出有效的帳號
                            int ErrLoginCnt = 0;
                            int checkCount = 100;
                            if (result.Rows[i]["ErrLoginCnt"] != null)
                                int.TryParse(result.Rows[i]["ErrLoginCnt"].ToString(), out ErrLoginCnt);
                            if (ConfigurationManager.AppSettings["AuthSetting3"] != null)
                                int.TryParse(ConfigurationManager.AppSettings["AuthSetting3"].ToString(), out checkCount);
                            if (ErrLoginCnt <= checkCount)
                            {//錯誤次數未超過指定次數
                                if (((DateTime)result.Rows[i]["PWD_due_Date"]).CompareTo(DateTime.Today) > 0)
                                {//密碼為過期

                                    //登入狀態設為正常
                                    iState = 0;
                                    try
                                    {
                                        if (_SystemNo == null) _SystemNo = "";
                                        if (result.Rows[i]["SiteId"] != null && !_SystemNo.Contains("," + result.Rows[i]["SiteId"].ToString().Trim() + ","))
                                            _SystemNo = result.Rows[i]["SiteId"].ToString().Trim() + "," + _SystemNo;
                                    }
                                    catch { }
                                    _CompanyCode = result.Rows[i]["CompanyCode"].ToString().Trim();
                                    _UserId = result.Rows[i]["UserID"].ToString().Trim();
                                    _UserName = result.Rows[i]["UserName"].ToString().Trim();
                                    //_UserUnid = result.Rows[i]["UserUnid"].ToString().Trim();   //20121225+
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
                                    try
                                    {//取得部門相關資料
                                        DBSetting.PersonalData PD = DBSetting.PersonalDateList(_Company, _EmployeeId);
                                        _DeptId = PD.DeptId;
                                        _DeptUnid = PD.DeptUnid;
                                        _DeptName = PD.DeptName;
                                        _EmployeeName = PD.EmployeeName;    //20121225+
                                    }
                                    catch { }
                                    //寫入登入時間與紀錄LOG
                                    AfterLogged();

                                    //改為檢核所有系統,以找出所有符合登入條件的系統
                                    //停止檢核
                                    i = result.Rows.Count;
                                }
                                else
                                {
                                    //密碼已過期
                                        iState = 5;
                                }
                            }
                            else
                            {
                                //錯誤次數已超過3次
                                    iState = 4;
                            }
                        }
                        else
                        {//登入狀態設為1,表未啟用
                                iState = 1;
                        }
                    }
                    else
                    {
                        //登入狀態設為2,表密碼不正確
                            iState = 2;
                    }
                    //}
                    //else
                    //{//展示模示時,為假登入
                    //    //登入狀態設為正常
                    //    iState = 0;
                    //    _SystemNo = vSystemNo;
                    //    _CompanyCode = vCompanyCode;
                    //    _UserId = vUserId;
                    //    _UserName = vUserId;
                    //    _IP = vIP;
                    //    _SessionID = thisSessionID;
                    //    _Company = vCompanyCode;

                    //    //寫入登入時間與紀錄LOG
                    //    AfterLogged();

                    //    SysSet.WriteToLogs("Login", "CompanyCode=" + vCompanyCode + "|UserId=" + vUserId + "|State=" + iState.ToString() + "\n\r theSiteList={" + theSiteList + "}");
                    //}
                }
                

                if (iState == 0)
                {
                    //CheckTransaction dd = new CheckTransaction ( );
                    //dd.ct ( );
                    //dd.SettlementUsedAnnualLeave(DateTime.Now.Year.ToString());
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
            else
            {
                iState = -1;
               
            }

            
        }
       
        
       

        //if (iState > 0 && vUserId.Equals(SysSet.GetConfigString("testid")) && vPwd.Equals(SysSet.GetConfigString("testpw")) && (vUserId.Trim().Length * vPwd.Trim().Length) != 0)
        //{//ForDeBug使用
        //    SysSet.WriteToLogs("Login", "DeBugCode=" + vUserId + "|IP=" + vIP);
        //    return 0;
        //}
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
        UData.SystemNo = vSystemNo;
        UData.CompanyCode = vCompanyCode;
        UData.UserId = vUserId;
        UData.UserName = vUserName;
        UData.IP = vIP;
        UData.SessionID = thisSessionID;
        UData.Company = _Company;
        UData.EmployeeId = _EmployeeId;
        UData.PWDdueDate = _PWDdueDate;
        UData.DeptId = _DeptId;
        UData.DeptUnid = _DeptUnid;
        UData.DeptName = _DeptName;

        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "";
        for (int i = 0; i < strSiteIDList.Length; i++)
        {
            if (i > 0)
                theSiteList += ",";
            theSiteList += "'" + strSiteIDList[i] + "'";
        }

        string MySQL;
        if (vCompanyCode.ToUpper() != "DEMO")
        {
            //vCompanyCode = ConfigurationManager.AppSettings["Company"].ToString();
            //if (SysSet.GetConfigString("AuthSetting").ToUpper().Equals("ADAUTH".ToUpper()))
            //{
            //    //MySQL = " And C.CompanyName = '" + vCompanyCode + "' ";
            //}
            //else
            //{
            //    //MySQL = " And U.CompanyCode = '" + ConfigurationManager.AppSettings["CompanyID"].ToString() + "' ";
            //}
            _SystemNo = vSystemNo;
            _CompanyCode = vCompanyCode;
            _UserId = vUserId;
            _SessionID = thisSessionID;

            MySQL = " And U.CompanyCode = '" + vCompanyCode + "' ";

            MySQL = " SELECT U.CompanyCode,CompanyName,IsNull(RoleName,'') RoleName,UserName " +
                " FROM UC_User U join UC_Company C On U.SiteId=C.SiteId And U.CompanyCode=C.CompanyCode " +
                " left join UC_RoleMember R On U.SiteId=R.SiteId And U.CompanyCode=R.CompanyCode And U.UserId=R.UserORRole " +
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
        }

        //寫入登入紀錄時,只記第一筆符合的帳號
        MySQL = " Update UC_User Set LastLogin=GetDate(),ErrLoginCnt=0 Where SiteId = '" + strSiteIDList[0] + "'" +
            " And CompanyCode = '" + _CompanyCode + "'" +
            " And UserId = '" + _UserId + "'";
        dbm.ExecuteCommand(MySQL);

        //[將SESSION紀錄設為無效]
        //if (ConfigurationManager.AppSettings["SignleSignOn"].ToString().ToUpper().Equals("Y"))
        //{
        //    //同一使用者以相同SESSION登入
        //    //1分鐘前以相同SESSION登入
        //    //所有非此次登入者之紀錄設為無效
        //    MySQL = " Update [UC_UserLoginLog] " +
        //        " Set [OutTime]=GetDate() " +
        //        " Where [SessionID]='" + _SessionID + "' And [OutTime] is Null " +
        //        " And ((DateDiff(mi,[InTime],GetDate()) > 1) Or ([UserID]<>'" + _UserId + "')) " +
        //        " Or ([SiteId] = '" + strSiteIDList[0] + "' And [CompanyCode]='" + _CompanyCode + "' And [UserID]='" + _UserId + "' And [OutTime] is Null) " +
        //        "  ";
        //}
        //else
        //{
        //    //同一使用者以相同SESSION登入時            
        //    MySQL = " Update [UC_UserLoginLog] " +
        //        " Set [OutTime]=GetDate() " +
        //        " Where [SessionID]='" + _SessionID + "' And [OutTime] is Null " +
        //        " And (([SiteId] = '" + strSiteIDList[0] + "' And [CompanyCode]='" + _CompanyCode + "' And [UserID]='" + _UserId + "') " +
        //        " )";
        //}
        //dbm.ExecuteCommand(MySQL);

        //將本次登入SESSION寫入登入表中
        MySQL = " INSERT INTO [UC_UserLoginLog] " +
            " ([SessionID],[SiteID],[CompanyCode],[UserID],[LoginIP],[InTime]) VALUES (" +
            " '" + _SessionID + "','" + strSiteIDList[0] + "','" + _CompanyCode + "'" +
            " ,'" + _UserId + "','" + vIP + "',GetDate()" +
            ")";
        dbm.ExecuteCommand(MySQL);
    }


    /// <summary>
    /// 授權驗証
    /// </summary>
    /// <param name="IP">叫用服務之IP</param>
    /// <param name="SessionID">登入Session</param>
    /// <returns></returns>
    public bool IsAuthorization(string IP, string SessionID)
    {
        bool blIsAuthorization = false;
        try
        {
            string[] IPList = ConfigurationManager.AppSettings["AuthIP"].ToString().Split(',');
            foreach (string theIP in IPList)
            {
                if (theIP == IP) return true;
            }

            if (blIsAuthorization == false && !string.IsNullOrEmpty(SessionID))
            {//IP驗証未通過時,使用SESSION驗証
                blIsAuthorization = IsAuthorization(SessionID);
            }
        }
        catch (Exception ex)
        {
            SysSet.WriteToLogs("AuthError", " IP:" + IP + "\n error:" + ex.Message);
        }
        return blIsAuthorization;
    }

    /// <summary>
    /// Session驗証,24小時內有效
    /// </summary>
    /// <param name="strSession">登入Session</param>
    /// <returns>Session是否有效</returns>
    public bool IsAuthorization(string SessionID)
    {
        bool blIsAuthorization = false;
        try
        {
            string strSQL = " Select Top 1 count(*),SiteID,CompanyCode,UserId,LoginIP,SessionID from [UC_UserLoginLog] " +
                " Where [SessionID] = '" + SessionID.Replace(";", "").Replace(" ", "").Replace("=", "") + "'" +
                " And [OutTime] is null And DateDiff(HH,[InTime],GetDate()) < 24 " +
                " group by SiteID,CompanyCode,UserId,LoginIP,SessionID " +
                " Order by Case when CompanyCode='Demo' then 0 else 1 end ";

            DataTable Dt = dbm.ExecuteDataTable(strSQL);
            if (Dt != null)
                if (Dt.Rows.Count > 0)
                    if (Convert.ToInt32(Dt.Rows[0][0].ToString()) > 0)
                    {
                        blIsAuthorization = true;
                        try
                        {
                            _SystemNo = Dt.Rows[0]["SiteID"].ToString();
                            _CompanyCode = Dt.Rows[0]["CompanyCode"].ToString();
                            _UserId = Dt.Rows[0]["UserId"].ToString();
                            _IP = Dt.Rows[0]["LoginIP"].ToString();
                            _SessionID = Dt.Rows[0]["SessionID"].ToString();
                        }
                        catch { }
                    }
        }
        catch (Exception ex)
        {
            SysSet.WriteToLogs("AuthError", " SessionID:" + SessionID + "\n error:" + ex.Message);
        }
        return blIsAuthorization;
    }

    /// <summary>
    /// Session驗証,24小時內有效
    /// </summary>
    /// <param name="DbConnection">指定DB連線</param>
    /// <param name="strSession">登入Session</param>
    /// <returns>Session是否有效</returns>
    public bool IsSSOAuthorization(DBManger.ConnectionString DbConnection, string SessionID)
    {
        return IsSSOAuthorization(dbm.GetConnectionString(DbConnection), SessionID);
    }

    public bool IsSSOAuthorization(string DbConnection, string SessionID)
    {
        bool blIsAuthorization = false;
        DBManger otherdbm = new DBManger();
        otherdbm.New(DbConnection);
        try
        {
            string strSQL = " Select Top 1 count(*),SiteID,CompanyCode,UserId,LoginIP,SessionID from [UC_UserLoginLog] " +
                " Where [SessionID] = '" + SessionID.Replace(";", "").Replace(" ", "").Replace("=", "") + "'" +
                " And [OutTime] is null And DateDiff(HH,[InTime],GetDate()) < 24 " +
                " group by SiteID,CompanyCode,UserId,LoginIP,SessionID " +
                " Order by Case when CompanyCode='Demo' then 0 else 1 end ";

            DataTable Dt = otherdbm.ExecuteDataTable(strSQL);
            if (Dt != null)
                if (Dt.Rows.Count > 0)
                    if (Convert.ToInt32(Dt.Rows[0][0].ToString()) > 0)
                    {
                        blIsAuthorization = true;
                        try
                        {
                            _SystemNo = Dt.Rows[0]["SiteID"].ToString();
                            _CompanyCode = Dt.Rows[0]["CompanyCode"].ToString();
                            _UserId = Dt.Rows[0]["UserId"].ToString();
                            _IP = Dt.Rows[0]["LoginIP"].ToString();
                            _SessionID = Dt.Rows[0]["SessionID"].ToString();
                        }
                        catch { }
                    }
        }
        catch (Exception ex)
        {
            SysSet.WriteToLogs("AuthError", " SessionID:" + SessionID + "\n error:" + ex.Message);
        }
        return blIsAuthorization;
    }

    /// <summary>
    /// 使用者登出
    /// </summary>
    /// <param name="SessionID"></param>
    /// <param name="LogoutUser"></param>
    public void UserLogOut(string SessionID, UserData LogoutUser)
    {
        string theSiteList = "";
        if (LogoutUser.SystemNo != null)
        {
            string[] strSiteIDList = LogoutUser.SystemNo.Split(',');
            for (int i = 0; i < strSiteIDList.Length; i++)
            {
                if (i > 0)
                    theSiteList += ",";
                theSiteList += "'" + strSiteIDList[i] + "'";
            }
        }
        else
            theSiteList = "[SiteId]";

        if (LogoutUser.CompanyCode != null)
            _CompanyCode = LogoutUser.CompanyCode;
        if (LogoutUser.UserId != null)
            _UserId = LogoutUser.UserId;

        string MySQL = "";
        if (ConfigurationManager.AppSettings["SignleSignOn"].ToString().ToUpper().Equals("Y"))
        {
            MySQL = " Update [UC_UserLoginLog] " +
            " Set [OutTime]=GetDate() " +
            " Where [OutTime] is Null " +
            " And (([SessionID]='" + SessionID + "') " +
            "   Or ([SiteId] in (" + theSiteList + ") And [CompanyCode]='" + _CompanyCode + "' And [UserID]='" + _UserId + "') " +
            "   Or (DateDiff(HH,[InTime],GetDate()) > 24*30) " +
            "     ) ";
        }
        else
        {
            MySQL = " Update [UC_UserLoginLog] " +
            " Set [OutTime]=GetDate() " +
            " Where [OutTime] is Null " +
            " And (([SessionID]='" + SessionID + "' and [SiteId] in (" + theSiteList + ") And [CompanyCode]='" + _CompanyCode + "' And [UserID]='" + _UserId + "') " +
            "   Or (DateDiff(HH,[InTime],GetDate()) > 24*30) " +
            "     ) ";
        }

        _CompanyCode = "";
        _UserId = "";

        dbm.ExecuteCommand(MySQL);
        //清除Cookie
        ClearLoginCookie();
    }

    /// <summary>
    /// 取得登入者資訊
    /// </summary>
    /// <param name="SessionID">登入授權ID</param>
    /// <returns></returns>
    public UserData GetUData(string SessionID)
    {
        return GetUData("", SessionID);
    }

    /// <summary>
    /// 取得登入者資訊
    /// </summary>
    /// <param name="DbConnection">指定DB連線</param>
    /// <param name="SessionID">登入授權ID</param>
    /// <returns></returns>
    public UserData GetUData(DBManger.ConnectionString DbConnection, string SessionID)
    {
        return GetUData(dbm.GetConnectionString(DbConnection), SessionID);
    }


    /// <summary>
    /// 取得登入者資訊
    /// </summary>
    /// <param name="DbConnection">指定DB連線</param>
    /// <param name="SessionID">登入授權ID</param>
    /// <returns></returns>
    public UserData GetUData(string DbConnection, string SessionID)
    {
        UserData theUData = new UserData();
        if (IsAuthorization(SessionID))
        {
            string strSQL = " SELECT [SessionID],L.[SiteID],L.[CompanyCode],[CompanyName],L.[UserID],[UserName],[LoginIP],[InTime],[OutTime] " +
                " ,Isnull([RoleName],'') RoleName " +
                " ,[PWD_due_date]" +
                " , IsNull(PS.[Company],'') As Company,IsNull(PS.[EmployeeId],'') As EmployeeId " +
                " From [UC_UserLoginLog] L " +
                " Join [UC_User] U On U.[SiteId]=L.[SiteId] And U.[CompanyCode]=L.[CompanyCode] And U.[UserId]=L.[UserId] " +
                " Join [UC_Company] C On C.[SiteId]=L.[SiteId] And C.[CompanyCode]=L.[CompanyCode] " +
                " Left join [UC_RoleMember] R On R.[SiteId]=L.[SiteId] And R.[CompanyCode]=L.[CompanyCode] And R.[UserORRole]=L.[UserId] " +
                " Left join [Personnel_Master] PS On PS.[Company]=U.[CompanyCode]"+ // And PS.[LoginAccount]=U.[UserId] and IsNull(PS.ByworkSeq,0)=0 " +
                " Where [SessionID] = '" + SessionID.Replace(";", "").Replace(" ", "").Replace("=", "") + "'" +
                " And [OutTime] is null And DateDiff(HH,[InTime],GetDate()) < 24 " +
                " Order by InTime Desc";

            DBManger otherdbm = new DBManger();
            if (DbConnection != "")
                otherdbm.New(DbConnection);
            else
                otherdbm.New();
            DataTable Dt = otherdbm.ExecuteDataTable(strSQL);
            if (Dt != null)
                if (Dt.Rows.Count > 0)
                {
                    theUData.CompanyCode = Dt.Rows[0]["CompanyCode"].ToString().Trim();
                    theUData.UserId = Dt.Rows[0]["UserID"].ToString().Trim();
                    theUData.UserName = Dt.Rows[0]["UserName"].ToString().Trim();
                    theUData.Company = Dt.Rows[0]["Company"].ToString().Trim();
                    theUData.EmployeeId = Dt.Rows[0]["EmployeeId"].ToString().Trim();
                    theUData.IP = Dt.Rows[0]["LoginIP"].ToString().Trim();
                    if (Dt.Rows[0]["PWD_due_date"] != null)
                        theUData.PWDdueDate = (DateTime)Dt.Rows[0]["PWD_due_date"];
                    else
                        theUData.PWDdueDate = DateTime.Today.AddDays(29);
                    theUData.SessionID = SessionID;
                    theUData.SystemNo = Dt.Rows[0]["SiteID"].ToString().Trim();
                    theUData.Role = Dt.Rows[0]["RoleName"].ToString().Trim();
                    for (int i = 1; i < Dt.Rows.Count; i++)
                    {
                        theUData.SystemNo += "," + Dt.Rows[i]["SiteID"].ToString().Trim();
                        theUData.Role += "," + Dt.Rows[i]["RoleName"].ToString().Trim();
                    }
                    try
                    {//取得部門相關資料
                        DBSetting.PersonalData PD = DBSetting.PersonalDateList(theUData.Company, theUData.EmployeeId);
                        theUData.DeptId = PD.DeptId;
                        theUData.DeptUnid = PD.DeptUnid;
                        theUData.DeptName = PD.DeptName;
                    }
                    catch { }
                }
            #region 這一段是不該存在的,只是測試期間為了方便才這樣寫
            //因為IBOS尚未規劃權限，故暫先保留指定角色以便測試一般功能
                //else
                //{
                //    theUData.SystemNo = _SystemNo;
                //    theUData.CompanyCode = _CompanyCode;
                //    theUData.Company = _Company;
                //    theUData.EmployeeId = _UserId;
                //    theUData.UserId = _UserId;
                //    theUData.UserName = _UserId;
                //    theUData.IP = _IP;
                //    theUData.SessionID = _SessionID;                    
                //    theUData.Role = "administrator";
                //    theUData.PWDdueDate = DateTime.Today.AddDays(29);
            //}
            #endregion
        }
        return theUData;
    }

    private bool ClearLoginCookie()
    {
        bool blClear = true;
        string cookieName = FormsAuthentication.FormsCookieName;

        HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

        if (null == authCookie)
        {
            //There is no authentication cookie.
            return blClear;
        }
        FormsAuthenticationTicket authTicket = null;
        try
        {
            authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        }
        catch (Exception ex)
        {
            //Write the exception to the Event Log.
            return blClear;
        }
        if (null == authTicket)
        {
            //Cookie failed to decrypt.
            return blClear;
        }
        else
        {
            authCookie.Expires = DateTime.Now.AddSeconds(-1);
        }
        return blClear;
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
                UData.DeptId = _DeptId;
                UData.DeptUnid = _DeptUnid;
                UData.DeptName = _DeptName;
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
                UData.SessionID = groups[8];
                _SessionID = groups[8];
                //2012/03/27 加入部門資訊
                UData.DeptId = groups[10];
                UData.DeptUnid = groups[11];
                UData.DeptName = groups[12];
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
        bool blCheck = false;
        if (string.IsNullOrEmpty(vSystemNo) || string.IsNullOrEmpty(vCompanyCode) || string.IsNullOrEmpty(vUserId) || string.IsNullOrEmpty(vProgramId) || string.IsNullOrEmpty(vRight))
        {
            return blCheck;
        }

        string MySQL = "";
        string[] strSiteIDList = vSystemNo.Split(new char[] { ',' });
        string theSiteList = "'" + defaultSystem + "'";
        for (int i = 0; i < strSiteIDList.Length; i++)
        {
            if (!theSiteList.Contains("'" + strSiteIDList[i] + "'"))
                theSiteList += ",'" + strSiteIDList[i] + "'";
        }
        MySQL = " SELECT  * " +
            " FROM UC_UserAuthority " +
            " Where SiteId in (" + theSiteList + ") And CompanyCode = @CompanyCode " +
            " And (UserOrRole in (Select RoleName From UC_RoleMember Where UserORRole = @UserId) Or UserOrRole = @UserId)" +
            " And AuthDefn = 1 And ProgramId = @ProgramId And RightName = (Case @Right When 'Execute' Then RightName Else @Right End) ";

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
                blCheck = true;
        }
        return blCheck;
    }
}
