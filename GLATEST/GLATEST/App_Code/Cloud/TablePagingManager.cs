using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// TablePagingManager 的摘要描述
/// </summary>
public class TablePagingManager
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
    public TablePagingManager()
	{
        _db = new DBManger();
        _db.New(DBManger.ConnectionString.IBosDB);
	}
    public TablePagingManager(DBManger.ConnectionString dbenum)
    {
        _db = new DBManger();
        _db.New(dbenum);
    }
    /// <summary>
    /// 取得資料表的分頁資訊及當頁的資料
    /// 全部欄位
    /// </summary>
    /// <param name="company"></param>
    /// <param name="parm">模糊參數</param>
    /// <remarks>
    /// WITH xGLAcctDef AS
    /// (
	///   Select 
	///   AllRowCount=(Select Count(*) FROM GLAcctDef Where Company = '10'),
	///   ROW_NUMBER() OVER (Order BY AcctNo) AS RowNumber,
	///   *
	///   FROM GLAcctDef 
	///   Where Company = '10'
    /// )
    /// Select Top 15 AllPageCount=((AllRowCount-1) / 15),	--需處理進位的問題
    /// * 
    /// From xGLAcctDef
    /// Where RowNumber > ((10-1) * 15)
    /// </remarks>
    /// <returns></returns>
    public DataTable GetTablePagingDT(string pagesize, string pagenumber, string tablename, string keyname, string parmwhere)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TablePaging");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = pagesize;
        sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = pagenumber;
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = tablename;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = keyname;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = parmwhere;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult > 0)
            {
                dt = (DataTable)ds.Tables[0];
                dt.TableName = "TablePaging";
            }
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return dt;
    }
    public DataTable GetTablePagingDT(FixedTreeData data)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TablePaging");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = data.PageSize;
        sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = data.PageNumber;
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = data.TableName;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = data.KeyName;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = data.WhereParm;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult > 0)
            {
                dt = (DataTable)ds.Tables[0];
                dt.TableName = "TablePaging";
            }
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
    /// 取得資料表的分頁資訊及當頁的資料
    /// 指定欄位
    /// </summary>
    /// <param name="pagesize"></param>
    /// <param name="pagenumber"></param>
    /// <param name="tablename"></param>
    /// <param name="keyname"></param>
    /// <param name="parmwhere"></param>
    /// <param name="parmfield"></param>
    /// <returns></returns>
    public DataTable GetTablePagingFixedDT(string pagesize, string pagenumber, string tablename, string keyname, string parmwhere, string parmfield)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TablePagingFixed");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = pagesize;
        sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = pagenumber;
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = tablename;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = keyname;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = parmwhere;
        sqlPars.Add("@ls_Fields", SqlDbType.VarChar, 1000).Value = parmfield;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult > 0)
            {
                dt = (DataTable)ds.Tables[0];
                dt.TableName = "TablePagingFixed";
            }
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }

        return dt;
    }
    public DataTable GetTablePagingFixedDT(FixedTreeData data)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TablePagingFixed");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = data.PageSize.ToString();
        sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = data.PageNumber.ToString();
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = data.TableName;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = data.KeyName;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = data.WhereParm;
        sqlPars.Add("@ls_Fields", SqlDbType.VarChar, 1000).Value = data.FieldName;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult > 0)
            {
                dt = (DataTable)ds.Tables[0];
                dt.TableName = "TablePagingFixed";
            }
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
    /// 取得Tree Table的DataSet
    /// </summary>
    /// <param name="data"></param>
    /// <remarks>
    /// 不含分頁
    /// </remarks>
    /// <returns></returns>
    public DataSet GetTableFixedTreeDS(FixedTreeData data)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TableFixedTree");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        //sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = data.PageSize.ToString();
        //sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = data.RowNumber.ToString();
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = data.TableName;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = data.KeyName;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = data.WhereParm;
        sqlPars.Add("@ls_Fields", SqlDbType.VarChar, 1000).Value = data.FieldName;
        sqlPars.Add("@ls_GroupBy", SqlDbType.VarChar, 1000).Value = data.GroupByName;
        sqlPars.Add("@ls_GroupByFields", SqlDbType.VarChar, 1000).Value = data.GroupByFieldsName;
        sqlPars.Add("@ls_MathFields", SqlDbType.VarChar, 1000).Value = data.MathFieldsName;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult == 0)
                ds = null;
            this._err = false;
        }
        catch (Exception ex)
        {
            ds = null;
            this._errMsg = ex.Message;
            this._err = true;
        }

        return ds;
    }
    /// <summary>
    /// 取得Tree Table的DataSet
    /// </summary>
    /// <param name="data"></param>
    /// <remarks>
    /// 分頁
    /// </remarks>
    /// <returns></returns>
    public DataSet GetTablePagingFixedTreeDS(FixedTreeData data)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iResult = 0;
        string sql = string.Format("SP_TablePagingFixedTree");
        SqlCommand cmd = new SqlCommand();
        SqlParameterCollection sqlPars = cmd.Parameters;
        sqlPars.Add("@ls_PageSize", SqlDbType.VarChar, 5).Value = data.PageSize.ToString();
        sqlPars.Add("@ls_ThePage", SqlDbType.VarChar, 5).Value = data.PageNumber.ToString();
        sqlPars.Add("@ls_TableName", SqlDbType.VarChar, 50).Value = data.TableName;
        sqlPars.Add("@ls_KeyName", SqlDbType.VarChar, 500).Value = data.KeyName;
        sqlPars.Add("@ls_Wher", SqlDbType.VarChar, 1000).Value = data.WhereParm;
        sqlPars.Add("@ls_Fields", SqlDbType.VarChar, 1000).Value = data.FieldName;
        sqlPars.Add("@ls_GroupBy", SqlDbType.VarChar, 1000).Value = data.GroupByName;
        sqlPars.Add("@ls_GroupByFields", SqlDbType.VarChar, 1000).Value = data.GroupByFieldsName;
        sqlPars.Add("@ls_MathFields", SqlDbType.VarChar, 1000).Value = data.MathFieldsName;

        try
        {
            iResult = _db.ExecStoredProcedure(sql, sqlPars, out ds);
            if (iResult == 0)
                ds = null;
            this._err = false;
        }
        catch (Exception ex)
        {
            ds = null;
            this._errMsg = ex.Message;
            this._err = true;
        }

        return ds;
    }
}
[Serializable]
public class FixedTreeData
{
    #region 資料物件屬性
    /// <summary>
    /// 目前頁次
    /// </summary>
    public int PageNumber { set; get; }
    /// <summary>
    /// 每頁筆數
    /// </summary>
    public int PageSize { set; get; }
    /// <summary>
    /// 目前頁次第一筆
    /// </summary>
    public int RowNumber { set; get; }
    /// <summary>
    /// 總共筆數
    /// </summary>
    public int AllRowCount { set; get; }
    /// <summary>
    /// 總共頁數
    /// </summary>
    public int AllPageCount { set; get; }
    /// <summary>
    /// 搜尋條件字串
    /// </summary>
    public string WhereParm { set; get; }
    /// <summary>
    /// 資料表或view的名稱
    /// </summary>
    public string TableName { set; get; }
    /// <summary>
    /// 資料表或view的Key值名稱
    /// </summary>
    public string KeyName { set; get; }
    /// <summary>
    /// Title欄位字串組合
    /// </summary>
    public string HeaderName { set; get; }
    /// <summary>
    /// 資料表顯示欄位字串組合
    /// </summary>
    public string FieldName { set; get; }
    /// <summary>
    /// Group By 欄位字串組合
    /// </summary>
    public string GroupByName { set; get; }
    /// <summary>
    /// Group By Fields 字串組合
    /// </summary>
    public string GroupByFieldsName { set; get; }
    /// <summary>
    /// Math Fields 字串組合 = 需要統計或計算的欄位
    /// </summary>
    public string MathFieldsName { set; get; }
    #endregion
    /// <summary>
    /// 建構式
    /// </summary>
    public FixedTreeData()
    {
        PageNumber = 1;     //初始在第1頁
        PageSize = 15;      //每頁顯示15筆資料
        RowNumber = 0;
        AllRowCount = 0;
        AllPageCount = 0;
        WhereParm = string.Empty;
        TableName = "";
        KeyName = "";
        HeaderName = "";
        FieldName = "";
        GroupByName = "";
        GroupByFieldsName = "";
        MathFieldsName = "";
    }
}