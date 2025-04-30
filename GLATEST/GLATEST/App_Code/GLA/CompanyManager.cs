using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;

/// <summary>
/// CompanyManager 的摘要描述
/// </summary>
public class CompanyManager
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
    private DBManger _db;
    #endregion
	public CompanyManager()
	{
        _db = new DBManger();
        _db.New(DBManger.ConnectionString.IBosDB);
	}
    public CompanyManager(DBManger.ConnectionString dbenum)
    {
        _db = new DBManger();
        _db.New(dbenum);
    }
    public DataTable GetCompanyDT()
    {
        DataTable dt = new DataTable();
        string sql = "SELECT Company + ' - ' + CompanyName as CompanyAndName, * FROM Company ORDER BY Company";
        try
        {
            dt = _db.ExecuteDataTable(sql);
            this._err = false;
        }
        catch (Exception ex)
        {
            dt = null;
            this._errMsg = ex.Message;
            this._err = true;
        }

        return dt;
    }
}