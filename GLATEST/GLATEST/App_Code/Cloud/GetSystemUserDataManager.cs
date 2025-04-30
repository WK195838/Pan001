using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBClass;

/// <summary>
/// GetSystemUserDataManager 的摘要描述
/// </summary>
public class GetSystemUserDataManager
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
    /// 建構式
    /// </summary>
	public GetSystemUserDataManager()
	{
        _db = new IBosDB(this);
	}
    /// <summary>
    /// 取得資料物件DataTable
    /// </summary>
    /// <returns></returns>
    public DataTable GetView_UC_UserDT(string userid)
    {
        DataTable dt = new DataTable();
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "View_UC_User");
        sql.Add("*");
        sql.Where.Add("UserId", userid);
        string strsql = sql.ToString();
        try
        {
            dt = _db.Query(strsql, "View_UC_User");
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return dt;
    }
    public SystemData.UserData GetSystemUserData(string userid)
    {
        SystemData.UserData data = new SystemData.UserData();
        DataTable dt = GetView_UC_UserDT(userid);
        if (dt != null && dt.Rows.Count > 0)
        {
            data.SessionID = Guid.NewGuid().ToString();
            data.SystemNo = dt.Rows[0]["SiteId"].ToString();
            data.Company = dt.Rows[0]["Company"].ToString();
            data.CompanyCode = dt.Rows[0]["CompanyCode"].ToString();
            data.CompanyName = dt.Rows[0]["CompanyName"].ToString();
            data.UserId = dt.Rows[0]["UserId"].ToString();
            data.UserUnid = dt.Rows[0]["UserUnid"].ToString();
            data.UserName = dt.Rows[0]["UserName"].ToString();
            data.EmployeeId = dt.Rows[0]["EmployeeId"].ToString();
            data.DeptId = dt.Rows[0]["DeptId"].ToString();
            data.DeptName = dt.Rows[0]["DeptName"].ToString();
            data.EmployeeUnid = dt.Rows[0]["EmployeeUnid"].ToString();
            data.DeptUnid = dt.Rows[0]["DepartmentUnid"].ToString();
            data.ByWorkUnid = dt.Rows[0]["ByWorkUnid"].ToString();
            data.IP = HttpContext.Current.Request.UserHostAddress;
            data.Role = "";
            data.PWDdueDate = DateTime.MaxValue;
        }

        return data;
    }
}