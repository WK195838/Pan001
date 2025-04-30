using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBClass;

/// <summary>
/// MyShortcutManager 的摘要描述
/// </summary>
public class MyShortcutManager
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
	public MyShortcutManager()
	{
        _db = new IBosDB(this);
	}
    /// <summary>
    /// 取得資料物件DataTable by userId
    /// </summary>
    /// <returns></returns>
    public DataTable GetMyShortcutDT(string userId)
    {
        DataTable dt = null;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "MyShortcut");
        sql.Add("*");
        sql.Where.Add("SiteId", System.Configuration.ConfigurationManager.AppSettings["SiteID"].ToString());
        sql.Where.Add("Company", System.Configuration.ConfigurationManager.AppSettings["CompanyID"].ToString());
        sql.Where.Add("UserId", userId);
        sql.OrderBy = "SiteId,Company,ShortcutGroup,Sort";
        string strSql = sql.ToString();
        try
        {
            dt = _db.Query(strSql, "MyShortcut");
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
    /// 取得資料物件List by userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<MyShortcutData> GetMyShortcutLT(string userId)
    {
        List<MyShortcutData> datas = new List<MyShortcutData>();
        MyShortcutData data;

        DataTable dt = this.GetMyShortcutDT(userId);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                data = this.GetMyShortcutData(dr);
                datas.Add(data);
            }
        }

        return datas;
    }
    /// <summary>
    /// 取得資料物件
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private MyShortcutData GetMyShortcutData(DataRow row)
    {
        MyShortcutData data = new MyShortcutData();
        data.ShortcutId = row["ShortcutId"].ToString();
        data.SiteId = row["SiteId"].ToString().Trim();
        data.CompanyId = row["Company"].ToString().Trim();
        data.UserId = row["UserId"].ToString().Trim();
        data.ShortcutGroup = row["ShortcutGroup"].ToString();
        data.Sort = Ftool.IntTryParse(row["Sort"].ToString());
        data.ShortcutName = row["ShortcutName"].ToString().Trim();
        data.ShortUrl = row["ShortUrl"].ToString().Trim();

        return data;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Insert(MyShortcutData data)
    {
        int effectcount = 0;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Insert, "MyShortcut");
        sql.Add("ShortcutId", System.Guid.NewGuid().ToString());
        sql.Add("SiteId", System.Configuration.ConfigurationManager.AppSettings["SiteID"].ToString());
        sql.Add("Company", System.Configuration.ConfigurationManager.AppSettings["CompanyID"].ToString());
        sql.Add("UserId", data.UserId);
        sql.Add("ShortcutGroup", data.ShortcutGroup);
        sql.Add("Sort", data.Sort);
        sql.Add("ShortcutName", data.ShortcutName);
        sql.Add("ShortUrl", data.ShortUrl);
        
        string sqlStr = sql.ToString();
        try
        {
            effectcount = _db.Execute(sqlStr);
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
    /// <param name="id"></param>
    /// <returns></returns>
    public int Delete(string id)
    {
        int effectcount = 0;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Delete, "MyShortcut");
        sql.Where.Add("ShortcutId", id);

        string sqlStr = sql.ToString();
        try
        {
            effectcount = _db.Execute(sqlStr);
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
public class MyShortcutData
{
    #region 資料物件屬性
    /// <summary>
    /// GUID
    /// </summary>
    public string ShortcutId { set; get; }
    /// <summary>
    /// 系統 SiteId
    /// </summary>
    public string SiteId { set; get; }
    /// <summary>
    /// 系統 CompanyId
    /// </summary>
    public string CompanyId { set; get; }
    /// <summary>
    /// 用戶 ID
    /// </summary>
    public string UserId { set; get; }
    /// <summary>
    /// 最愛的群組
    /// </summary>
    public string ShortcutGroup { set; get; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { set; get; }
    /// <summary>
    /// 最愛的名稱
    /// </summary>
    public string ShortcutName { set; get; }
    /// <summary>
    /// 最愛的URL
    /// </summary>
    public string ShortUrl { set; get; }
    #endregion
    public MyShortcutData()
    {
    }
}