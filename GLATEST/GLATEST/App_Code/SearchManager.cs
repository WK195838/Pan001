using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// SearchManager 的摘要描述
/// </summary>
public class SearchManager
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

	public SearchManager()
	{
        _db = new DBManger();
        _db.New();
	}
}

[Serializable]
public class SearchData
{
    #region 物件屬性

    /// <summary>
    /// 公司別
    /// </summary>
    public string Company { set; get; }

    /// <summary>
    /// 客戶識別碼
    /// </summary>
    public string CustomerUnid { set; get; }

    /// <summary>
    /// 客戶名稱
    /// </summary>
    public string CustomerName { set; get; }

    /// <summary>
    /// 業務負責人識別碼
    /// </summary>
    public string SalesUnid { set; get; }

    /// <summary>
    /// 受理人識別碼
    /// </summary>
    public string ProcessorUnid { set; get; }

    #endregion

    /// <summary>
    /// 建構式
    /// </summary>
    public SearchData()
    {
        //物件預設值
        Company = ConfigurationManager.AppSettings["CompanyID"].ToString();
        CustomerUnid = string.Empty;    //客戶Unid
        CustomerName = string.Empty;   //客戶名稱
        SalesUnid = string.Empty;       //業務員Unid
        ProcessorUnid = string.Empty;    //受理人Unid
    }
}