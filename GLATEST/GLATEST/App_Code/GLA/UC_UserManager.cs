using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBClass;

/// <summary>
/// UC_UserManager 的摘要描述
/// </summary>
public class UC_UserManager
{
    #region 管理物件屬性
    private string _errMsg;
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string ErrorMessage
    {
        set { _errMsg = value; }
        get { return _errMsg; }
    }
    private bool _err;
    /// <summary>
    /// 是否有錯誤
    /// </summary>
    public bool HasError
    {
        set { _err = value; }
        get { return _err; }
    }
    /// <summary>
    /// 共用資料庫物件
    /// </summary>
    private IBosDB _db;
    #endregion
    /// <summary>
    /// 管理物件建構式
    /// </summary>
    public UC_UserManager()
    {
        _db = new IBosDB(this);
    }
    /// <summary>
    /// 取得資料物件DataTable
    /// </summary>
    /// <returns></returns>
    public DataTable GetUC_UserDT()
    {
        DataTable dt = null;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_User");
        sql.Add("*");
        string strsql = sql.ToString();
        try
        {
            dt = _db.Query(strsql, "UC_User");
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return dt;
    }
    /// <summary>
    /// 取得資料物件DataTable by Primary Key
    /// </summary>
    /// <param name="SiteId"></param>
    /// <param name="CompanyCode"></param>
    /// <param name="UserId"></param>
    /// <returns></returns>
    public DataTable GetUC_UserDT(string SiteId, string CompanyCode, string UserId)
    {
        DataTable dt = null;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "UC_User");
        sql.Add("*");
        if (!string.IsNullOrEmpty(SiteId))
            sql.Where.Add("SiteId", SiteId);
        if (!string.IsNullOrEmpty(CompanyCode))
            sql.Where.Add("CompanyCode", CompanyCode);
        if (!string.IsNullOrEmpty(UserId))
            sql.Where.Add("UserId", UserId);
        string strsql = sql.ToString();
        try
        {
            dt = _db.Query(strsql, "UC_User");
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return dt;
    }
    /// <summary>
    /// 取得資料物件List集合
    /// </summary>
    /// <returns></returns>
    public List<UC_UserData> GetUC_UserLT()
    {
        UC_UserData data;
        List<UC_UserData> datas = new List<UC_UserData>();
        DataTable dt = this.GetUC_UserDT();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                data = GetUC_UserData(dr);
                datas.Add(data);
            }
        }
        return datas;
    }
    /// <summary>
    /// 取得資料物件List集合
    /// </summary>
    /// <param name="SiteId"></param>
    /// <param name="CompanyCode"></param>
    /// <param name="UserId"></param>
    /// <returns></returns>
    public List<UC_UserData> GetUC_UserLT(string SiteId, string CompanyCode, string UserId)
    {
        UC_UserData data;
        List<UC_UserData> datas = new List<UC_UserData>();
        DataTable dt = this.GetUC_UserDT(SiteId, CompanyCode, UserId);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                data = GetUC_UserData(dr);
                datas.Add(data);
            }
        }
        return datas;
    }
    /// <summary>
    /// 將DataRow轉換成資料物件
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private UC_UserData GetUC_UserData(DataRow dr)
    {
        UC_UserData data = new UC_UserData();
        data.SiteId = dr["SiteId"].ToString();
        data.CompanyCode = dr["CompanyCode"].ToString();
        data.UserId = dr["UserId"].ToString();
        data.UserName = dr["UserName"].ToString();
        //data.Password = int.Parse(dr["Password"].ToString());
        data.PWD_due_date = DateTime.Parse(dr["PWD_due_date"].ToString());
        if (dr["LastLogin"].ToString() != "")
            data.LastLogin = DateTime.Parse(dr["LastLogin"].ToString());
        data.ErrLoginCnt = int.Parse(dr["ErrLoginCnt"].ToString());
        data.Enable = bool.Parse(dr["Enable"].ToString());
        data.UserUnid = dr["UserUnid"].ToString();
        data.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
        data.CreateUser = dr["CreateUser"].ToString();
        data.UpdateTime = DateTime.Parse(dr["UpdateTime"].ToString());
        data.UpdateUser = dr["UpdateUser"].ToString();
        return data;
    }
    /// <summary>
    /// 取得UC_User資料結構陣列
    /// </summary>
    /// <returns></returns>
    public string[] GetUC_UserSchema()
    {
        string[] str = new string[] { "SiteId", "CompanyCode", "UserId", "UserName","Password",
            "PWD_due_date", "LastLogin","ErrLoginCnt","Enable","UserUnid",
            "CreateTime","CreateUser","UpdateTime","UpdateUser"};
        return str;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Insert(UC_UserData data)
    {
        int effectcount = 0;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Insert, "UC_User");
        sql.Add("SiteId", data.SiteId);
        sql.Add("CompanyCode", data.CompanyCode);
        sql.Add("UserId", data.UserId);
        sql.Add("UserName", data.UserName);
        sql.Add("Password", "convert(varbinary," + data.Password + ")");
        sql.Add("PWD_due_date", data.PWD_due_date, SqlString.Date2StrType.DateTime);
        sql.Add("LastLogin", data.LastLogin, SqlString.Date2StrType.DateTime);
        sql.Add("ErrLoginCnt", data.ErrLoginCnt);
        sql.Add("Enable", data.Enable);
        sql.Add("BankCode", data.BankCode);
        sql.Add("SubBankCode", data.SubBankCode);
        sql.Add("SubBankName", data.SubBankName);
        sql.Add("CheckCode", data.CheckCode);
        sql.Add("BankAgentCode", data.BankAgentCode);
        sql.Add("UserUnid", data.UserUnid);
        sql.Add("CreateTime", data.CreateTime, SqlString.Date2StrType.DateTime);
        sql.Add("CreateUser", data.CreateUser);
        sql.Add("UpdateTime", data.UpdateTime, SqlString.Date2StrType.DateTime);
        sql.Add("UpdateUser", data.UpdateUser);
        string strsql = sql.ToString();
        try
        {
            effectcount = _db.Execute(strsql);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return effectcount;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Update(UC_UserData data)
    {
        int effectcount = 0;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Update, "UC_User");
        sql.Add("UserName", data.UserName);
        sql.Add("Password", data.Password);
        sql.Add("PWD_due_date", data.PWD_due_date, SqlString.Date2StrType.DateTime);
        sql.Add("LastLogin", data.LastLogin, SqlString.Date2StrType.DateTime);
        sql.Add("ErrLoginCnt", data.ErrLoginCnt);
        sql.Add("Enable", data.Enable);
        sql.Add("BankCode", data.BankCode);
        sql.Add("SubBankCode", data.SubBankCode);
        sql.Add("SubBankName", data.SubBankName);
        sql.Add("CheckCode", data.CheckCode);
        sql.Add("BankAgentCode", data.BankAgentCode);
        sql.Add("CreateTime", data.CreateTime, SqlString.Date2StrType.DateTime);
        sql.Add("CreateUser", data.CreateUser);
        sql.Add("UpdateTime", data.UpdateTime, SqlString.Date2StrType.DateTime);
        sql.Add("UpdateUser", data.UpdateUser);
        sql.Add("UserUnid", data.UserUnid);
        sql.Where.Add("SiteId", data.SiteId);
        sql.Where.Add("CompanyCode", data.CompanyCode);
        sql.Where.Add("UserId", data.UserId);
        string strsql = sql.ToString();
        try
        {
            effectcount = _db.Execute(strsql);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return effectcount;
    }
    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Delete(UC_UserData data)
    {
        int effectcount = 0;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Delete, "UC_User");
        sql.Where.Add("SiteId", data.SiteId);
        sql.Where.Add("CompanyCode", data.CompanyCode);
        sql.Where.Add("UserId", data.UserId);
        string sqlstr = sql.ToString();
        try
        {
            effectcount = _db.Execute(sqlstr);
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return effectcount;
    }

}


[Serializable]
public class UC_UserData
{
    #region MyRegion
    private string _SiteId;
    /// <summary>
    /// SiteId
    /// </summary>
    public string SiteId
    {
        set { _SiteId = value; }
        get { return _SiteId; }
    }

    private string _CompanyCode;
    /// <summary>
    /// CompanyCode
    /// </summary>
    public string CompanyCode
    {
        set { _CompanyCode = value; }
        get { return _CompanyCode; }
    }

    private string _UserId;
    /// <summary>
    /// UserId
    /// </summary>
    public string UserId
    {
        set { _UserId = value; }
        get { return _UserId; }
    }

    private string _UserName;
    /// <summary>
    /// UserName
    /// </summary>
    public string UserName
    {
        set { _UserName = value; }
        get { return _UserName; }
    }

    private string _Password;
    /// <summary>
    /// Password
    /// </summary>
    public string Password
    {
        set { _Password = value; }
        get { return _Password; }
    }

    private DateTime _PWD_due_date;
    /// <summary>
    /// PWD_due_date
    /// </summary>
    public DateTime PWD_due_date
    {
        set { _PWD_due_date = value; }
        get { return _PWD_due_date; }
    }

    private DateTime _LastLogin;
    /// <summary>
    /// LastLogin
    /// </summary>
    public DateTime LastLogin
    {
        set { _LastLogin = value; }
        get { return _LastLogin; }
    }

    private int _ErrLoginCnt;
    /// <summary>
    /// ErrLoginCnt
    /// </summary>
    public int ErrLoginCnt
    {
        set { _ErrLoginCnt = value; }
        get { return _ErrLoginCnt; }
    }

    private bool _Enable;
    /// <summary>
    /// Enable
    /// </summary>
    public bool Enable
    {
        set { _Enable = value; }
        get { return _Enable; }
    }

    private string _BankCode;
    /// <summary>
    /// BankCode
    /// </summary>
    public string BankCode
    {
        set { _BankCode = value; }
        get { return _BankCode; }
    }

    private string _SubBankCode;
    /// <summary>
    /// SubBankCode
    /// </summary>
    public string SubBankCode
    {
        set { _SubBankCode = value; }
        get { return _SubBankCode; }
    }

    private string _SubBankName;
    /// <summary>
    /// SubBankName
    /// </summary>
    public string SubBankName
    {
        set { _SubBankName = value; }
        get { return _SubBankName; }
    }

    private string _CheckCode;
    /// <summary>
    /// CheckCode
    /// </summary>
    public string CheckCode
    {
        set { _CheckCode = value; }
        get { return _CheckCode; }
    }

    private string _BankAgentCode;
    /// <summary>
    /// BankAgentCode
    /// </summary>
    public string BankAgentCode
    {
        set { _BankAgentCode = value; }
        get { return _BankAgentCode; }
    }
    private string _UserUnid;
    /// <summary>
    /// UserUnid 
    /// </summary>
    public string UserUnid
    {
        set { _UserUnid = value; }
        get { return _UserUnid; }
    }
    private DateTime _CreateTime;
    /// <summary>
    /// CreateTime
    /// </summary>
    public DateTime CreateTime
    {
        set { _CreateTime = value; }
        get { return _CreateTime; }
    }

    private string _CreateUser;
    /// <summary>
    /// CreateUser
    /// </summary>
    public string CreateUser
    {
        set { _CreateUser = value; }
        get { return _CreateUser; }
    }

    private DateTime _UpdateTime;
    /// <summary>
    /// UpdateTime
    /// </summary>
    public DateTime UpdateTime
    {
        set { _UpdateTime = value; }
        get { return _UpdateTime; }
    }

    private string _UpdateUser;
    /// <summary>
    /// UpdateUser
    /// </summary>
    public string UpdateUser
    {
        set { _UpdateUser = value; }
        get { return _UpdateUser; }
    }

    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public UC_UserData()
    {
    }
}