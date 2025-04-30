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
using System.Xml;
using System.Text;
using DBClass;

/// <summary>
/// 
/// </summary>
public class AppAuthority : DB_Base
{
    public bool RunApp = false;
    private List<string> _Permission;
    private List<string> _Roles;
    public string MyRoles = "";
    private List<string> _FreeApp;          //免授權程式
    private List<string> _Administrator;    //系統管理員使用者清單

    private string _UserID;
    private int _roleLevel = 0;
    private string _ProgramName;
    private string _ProgramPath;
    private string _ProgramID;
    public string UserID
    {
        get { return _UserID; }
        set { _UserID = value; }
    }
    public string ProgramName
    {
        get { return _ProgramName; }
        set { _ProgramName = value; }
    }
    public string ProgramPath
    {
        get { return _ProgramPath; }
        set { _ProgramPath = value; }
    }
    public string ProgramID
    {
        get { return _ProgramID; }
        set { _ProgramID = value; }
    }
    public int RoleLevel
    {
        get { return _roleLevel; }
    }


    public AppAuthority(CommonData common, string UserID, string ProgramPath)
        : base(common)
    {
        _UserID = UserID;
        _ProgramPath = ProgramPath;
        _ProgramID = GetProgramID(_ProgramPath);
        _ProgramName = GetProgramName(_ProgramID);

        _FreeApp = new List<string>();
        _Administrator = new List<string>();
        //建立免授權程式清單 (要轉成小寫)
        _FreeApp.Add("login.aspx".ToLower());
        _FreeApp.Add("default.aspx");
        _FreeApp.Add("menutree.aspx");
        _FreeApp.Add("pages/apperrormessage.aspx");
        _FreeApp.Add("Upload/CheckAuth.aspx".ToLower());


        //建立管理員清單
        BuildAdministrator();

        ////是否為免授權之程式
        //if (_FreeApp.Contains(ProgramPath.ToLower()))
        //{
        //    //免授權之程式
        //    RunApp = true;
        //}
    }


    //建立管理員清單
    private void BuildAdministrator()
    {
        _Administrator.Add("adm");
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_RoleMember");
        sql.Add("*");
        sql.Where.Add("RoleName", "adm");
        _Common.DR = _Common.DB.Query(sql.ToString());
        MyDataReader myDR = new MyDataReader(_Common.DR);
        while (myDR.Reader())
        {
            string value = _Common.DR["UserID"].ToString();
            if (!_Administrator.Contains(value))
            {
                _Administrator.Add(value);
            }
        }
    }

    public bool CheckAuthority()
    {
        return CheckAuthority("");
    }

    public bool CheckAuthority(string defaultRoles)
    {
        RunApp = false;

        if (!string.IsNullOrEmpty(defaultRoles))
        {
            _Roles = new List<string>();
            string[] aRoles = defaultRoles.Split(',');
            foreach (string role in aRoles)
            {
                _Roles.Add(role);
            }
        }
        else
        {
            //取得使用者付予之角色
            GetRoles(_UserID);
        }

        //該使用者是否為管理員
        if (_Administrator.Contains(_UserID))
        {
            //取得所有授權
            GetAdminPermission();
        }
        else
        {
            //取得該程式之授權
            GetPermission(_ProgramID);
        }

        //是否具有執行該程式之權限
        string realProgramPath = (ProgramPath.Split('?'))[0].ToLower();
        if (_FreeApp.Contains(realProgramPath))
        {
            //免授權之程式
            RunApp = true;
        }
        else
        {
            if (_Permission.Contains("Execute"))
            {
                RunApp = true;
            }

        }
        return RunApp;
    }

    //
    public string GetProgramID(string ProgramPath)
    {
        string sResult = "";
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_Program");
        sql.Add("*");
        sql.Where.Add("ProgramPath", SqlWhere.WhereOperator.And_Like, ProgramPath + "%");
        //sql.Where.Add("ProgramPath",  ProgramPath);

        _Common.DR = _Common.DB.Query(sql.ToString());
        MyDataReader myDR = new MyDataReader(_Common.DR);
        if (myDR.Reader())
        {
            myDR.GetData("ProgramID", ref sResult);
        }
        return sResult;
    }

    public string GetProgramName(string ProgramID)
    {
        string sResult = "";
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_Program");
        sql.Add("*");
        sql.Where.Add("ProgramID", ProgramID);
        //sql.Where.Add("ProgramPath",  ProgramPath);

        _Common.DR = _Common.DB.Query(sql.ToString());
        MyDataReader myDR = new MyDataReader(_Common.DR);
        if (myDR.Reader())
        {
            myDR.GetData("ProgramName", ref sResult);
        }
        return sResult;
    }

    /// <summary>
    /// 取得所有角色
    /// </summary>
    /// <param name="UserID"></param>
    public void GetRoles(string UserID)
    {
        _roleLevel = 0;
        _Roles = new List<string>();

        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "view_UC_UserRole");
        sql.Add("*");
        sql.Where.Add("UserORRole", UserID);
        _Common.DR = _Common.DB.Query(sql.ToString());
        MyDataReader dr = new MyDataReader(_Common.DR);

        while (dr.Reader())
        {
            string value = "";
            int roleLevel = 0;

            dr.GetData("RoleName", ref value);
            dr.GetData("AuthLevel", ref roleLevel);
            if (!_Roles.Contains(value))
            {
                _Roles.Add(value);
            }
            if (roleLevel > _roleLevel)
                _roleLevel = roleLevel;
        }
    }

    /// <summary>
    /// 檢查是有某一角色
    /// </summary>
    /// <param name="RoleName"></param>
    /// <returns></returns>
    public bool CheckRole(string RoleName)
    {
        if (_Roles == null)
        {
            //return false;
            return true;
        }

        if (_Roles.Contains(RoleName))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 取得所付予角色字串
    /// </summary>
    /// <returns></returns>
    public string GetRoleString()
    {
        string sResult = "";
        for (int i = 0; i < _Roles.Count; i++)
        {
            sResult += _Roles[i] + ",";
        }
        if (sResult.Length > 0)
            sResult = sResult.Substring(0, sResult.Length - 1);

        return sResult;
    }

    /// <summary>
    /// 取得授權
    /// </summary>
    /// <param name="ProgramID"></param>
    public void GetPermission(string ProgramID)
    {

        _Permission = new List<string>();

        if (String.IsNullOrEmpty(ProgramID))
            return;

        string sRoles = this.GetRoleString();
        string sUserOrRole = "";

        //是否有角色
        if (sRoles.Length > 0)
        {
            sUserOrRole = "'" + sRoles.Replace(",", "','") + "','" + _Common.User.UserId + "'";
        }
        else
        {
            sUserOrRole = "'" + _Common.User.UserId + "'";
        }
        //加入使用者單位
        sUserOrRole += ",'" + _Common.User.DeptId + "'";

        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_UserAuthority");
        sql.Add("*");
        sql.Where.Add("ProgramID", ProgramID);
        sql.Where.CustomWhere = String.Format(" And UserOrRole in ({0})", sUserOrRole);
        _Common.DR = _Common.DB.Query(sql.ToString());
        while (_Common.DR.Read())
        {
            string value = _Common.DR["RightName"].ToString();
            if (!_Permission.Contains(value))
            {
                _Permission.Add(value);
            }
        }
    }

    /// <summary>
    /// 取得管理員之授權
    /// </summary>
    private void GetAdminPermission()
    {

        _Permission = new List<string>();
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_RightName");
        sql.Add("*");
        _Common.DR = _Common.DB.Query(sql.ToString());
        while (_Common.DR.Read())
        {
            string value = _Common.DR["RightName"].ToString();
            if (!_Permission.Contains(value))
            {
                _Permission.Add(value);
            }
        }
    }

    /// <summary>
    /// 檢查授權
    /// </summary>
    /// <param name="RightName"></param>
    /// <returns></returns>
    public bool CheckPermission(string RightName)
    {
        if (_Permission == null)
            return false;

        if (_Permission.Contains(RightName))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 產生已授權清單字串
    /// </summary>
    /// <returns></returns>
    public string GetPermissionString()
    {
        string sResult = "";
        for (int i = 0; i < _Permission.Count; i++)
        {
            sResult += _Permission[i] + ",";
        }
        if (sResult.Length > 0)
            sResult = sResult.Substring(0, sResult.Length - 1);

        return sResult;
    }


    private void AddAttributes(XmlWriter xml, IDataReader dr)
    {
        string url = "";
        string title = "";
        string icon = "";
        string description = "";
        string target = "";
        string selectAction = "";

        url = "~/" + dr["url"].ToString();
        title = dr["title"].ToString();
        icon = dr["icon"].ToString();
        if (icon == "") { icon = "~/JsCss/ftv2folderclosed.gif"; }
        else { icon = "~/JsCss/" + icon; }
        description = dr["description"].ToString();
        target = dr["target"].ToString();

        xml.WriteAttributeString("url", url);
        xml.WriteAttributeString("title", title);
        xml.WriteAttributeString("icon", icon);
        xml.WriteAttributeString("description", description);
        xml.WriteAttributeString("target", target);
        xml.WriteAttributeString("selectAction", selectAction);
    }

    public string GetRightXml()
    {
        StringBuilder strXml = new StringBuilder();
        _Common.DB.QueryCmdType = CommandType.StoredProcedure;
        _Common.DB.QueryCmdText = "sp_UC_XmlRightTree";
        _Common.DB.QueryCmdParam("UserId", "adm");
        //        _MyCommon.DB.QueryCmdParam("UserId", "nctest");
        _Common.DR = _Common.DB.QueryParam();
        //System.Xml.XmlTextWriter xml= new System.Xml.XmlTextWriter(
        XmlWriterSettings set = new XmlWriterSettings();
        set.Encoding = Encoding.UTF8;
        XmlWriter xml = XmlWriter.Create(strXml, set);
        int iLevel = 0;
        bool needEnd = false;
        xml.WriteStartElement("siteMap");


        while (_Common.DR.Read())
        {
            int level = int.Parse(_Common.DR["level"].ToString());

            if (iLevel != level)
            {
                if (level > iLevel)
                {
                    xml.WriteStartElement("siteMapNode");
                    AddAttributes(xml, _Common.DR);
                    needEnd = true;
                }
                else
                {
                    if (needEnd)
                    {
                        needEnd = false;
                        xml.WriteEndElement();
                    }
                    xml.WriteEndElement();
                    xml.WriteStartElement("siteMapNode");
                    AddAttributes(xml, _Common.DR);
                    needEnd = true;
                }
            }
            else
            {
                if (needEnd)
                {
                    needEnd = false;
                    xml.WriteEndElement();
                }
                xml.WriteStartElement("siteMapNode");
                AddAttributes(xml, _Common.DR);
                xml.WriteEndElement();
            }
            iLevel = level;
        }
        if (needEnd)
        {
            needEnd = false;
            xml.WriteEndElement();
        }

        for (int i = 0; i < iLevel; i++)
        {
            xml.WriteEndElement();
        }
        xml.Flush();

        return strXml.ToString();
    }

    public static DataTable GetUserByRoleName(CommonData common, string roleName)
    {
        DataTable dt = null;
        string roles = "'" + roleName.Replace(",", "','") + "'";
        string sSQL = @"Select * From UC_User Where UserId In(
Select UserID From UC_RoleMember Where RoleName In({0})
)";
        sSQL = string.Format(sSQL, roles);
        dt = common.DB.Query(sSQL, "UC_User");

        return dt;
    }

    /// <summary>
    /// 依角色取得使用者，與Department相關
    /// </summary>
    /// <param name="common"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public static DataTable GetUserByRoleNameWithDept(CommonData common, string roleName)
    {
        DataTable dt = null;
        string roles = "'" + roleName.Replace(",", "','") + "'";
        string sSQL = @"SELECT  UserId, UserName, Department.departmentName, Department.departmentCode, UC_User.department_PK From UC_User INNER JOIN  Department ON UC_User.department_PK = Department.department_PK WHERE  UC_User.UserId IN (SELECT  UserID FROM UC_RoleMember  WHERE RoleName IN ({0})) and UC_User.UserStatus<>'2'";
        sSQL = string.Format(sSQL, roles);
        dt = common.DB.Query(sSQL, "UC_User");

        return dt;
    }

    /// <summary>
    /// 依UserId取得使用者資料，與Department相關
    /// </summary>
    /// <param name="common"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public static DataTable GetUserWithDept(CommonData common, string userId)
    {
        DataTable dt = null;

        string sSQL = @"SELECT  UserId, UserName, Department.departmentName, Department.departmentCode, UC_User.department_PK From UC_User INNER JOIN  Department ON UC_User.department_PK = Department.department_PK Where  UC_User.UserId = '{0}'";
        sSQL = string.Format(sSQL, userId);
        dt = common.DB.Query(sSQL, "UC_User");

        return dt;
    }
}
