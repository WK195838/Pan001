using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DBClass;

/// <summary>
/// zfNounManager 的摘要描述
/// </summary>
public class zfNounManager
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

    public zfNounManager()
    {
        _db = new IBosDB(this);
    }

    /// <summary>
    /// 取得zfNoun資料結構陣列
    /// </summary>
    /// <returns></returns>
    public string[] GetzfNounSchema()
    {
        string[] str
            = new string[] { "NounUnid", "NounCode", "NounName", "ParentNounCode", "ParentUnid", "SortValue", 
                             "NounDesc", "Disable", "Reserve1", "Reserve2", "Reserve3", "UseBySystem", 
                             "SystemCode", "CreateUser", "CreateDatetime", "UpdateUser", "UpdateDatetime"
                            };

        return str;
    }

    /// <summary>
    /// 定義行事曆使用之行程項目 (01=預定行程；02=工作記錄；03待辦事項；04=需求單；05=客服單...)
    /// </summary>
    /// <returns></returns>
    public DataTable GetScheduleItemUseByCalendarDT()
    {
        DataTable dtData;
        dtData = this.GetNounDataTable("ScheduleItemUseByCalendarDataTable", "ScheduleItemUseByCalendar");

        return dtData;
    }

    /// <summary>
    /// 取得資料物件List集合 [提醒時間開隔定義]  (NounCode=分鐘數　例如: 5分鐘前 NounCode=5；1小時前 NounCode=60)
    /// </summary>
    /// <returns></returns>
    public List<zfNounData> GetRemindTimeIntervalLT()
    {
        List<zfNounData> dataList = new List<zfNounData>();
        DataTable dtData = this.GetNounDataTable("RemindTimeIntervalDataTable", "RemindTimeInterval");
        if (dtData != null && dtData.Rows.Count > 0)
        {
            foreach (DataRow row in dtData.Rows)
            {
                zfNounData data = this.GetNounDate(row);
                dataList.Add(data);
            }
        }

        return dataList;
    }


    /// <summary>
    /// 取得資料物件List集合 [傳票類別]
    /// </summary>
    /// <returns></returns>
    public List<zfNounData> GetVourTypeLT()
    {
        List<zfNounData> dataList = new List<zfNounData>();
        DataTable dtData = this.GetNounDataTable("VourTypeDataTable", "VourType");
        if (dtData != null && dtData.Rows.Count > 0)
        {
            foreach (DataRow row in dtData.Rows)
            {
                zfNounData data = this.GetNounDate(row);
                dataList.Add(data);
            }
        }

        return dataList;
    }

   
    /// <summary>
    /// 依條件取得相關名詞檔中Reserve1欄位資料
    /// </summary>
    /// <param name="NounUnid">NounUnid</param>
    /// <param name="TableName">TableName</param>
    /// <returns></returns>
    public string GetNounReserve1(string NounUnid, string TableName)
    {
        DataTable dt = null;
        string strSql;

        try
        {
            strSql = string.Format("select dbo.fn_GetNounReserve1('{0}')", NounUnid);
            dt = _db.Query(strSql, TableName);
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt.Rows[0][0].ToString();
    }

    /// <summary>
    /// 依條件取得相關名詞檔中NounName欄位資料
    /// </summary>
    /// <param name="NounUnid">NounUnid</param>
    /// <param name="TableName">TableName</param>
    /// <returns></returns>
    public string GetNounName(string NounUnid, string TableName)
    {
        DataTable dt = null;
        string strSql;

        try
        {
            strSql = string.Format("select dbo.fn_GetNounName('{0}')", NounUnid);
            dt = _db.Query(strSql, TableName);
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt.Rows[0][0].ToString();
    }



    /// <summary>
    /// 依條件取得相關名詞檔資料
    /// </summary>
    /// <param name="datatableName">回傳DataTable的檔案名稱</param>
    /// <param name="parentUnid">父階Unid</param>
    /// <returns></returns>
    private DataTable GetNounDataTable(string datatableName, string parentUnid)
    {
        DataTable dt = null;
        string strSql;

        try
        {
            strSql = string.Format("select * from dbo.fn_GetNounList('{0}')", parentUnid);
            dt = _db.Query(strSql, datatableName);
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt;
    }

    /// <summary>
    /// 取得待辦執行現況資料 (此為子階資料)
    /// </summary>
    /// <returns></returns>
    public DataTable GetTodoCurrentStatusDT()
    {
        DataTable dtData;
        dtData = this.GetNoun2DataTable("TodoCurrentStatusDataTable", "D7CDF7229484B0334825758D000E3CAE");

        return dtData;
    }

    /// <summary>
    /// 依條件取得子階相關名詞檔資料
    /// </summary>
    /// <param name="datatableName">回傳DataTable的檔案名稱</param>
    /// <param name="parentUnid">父階Unid</param>
    /// <returns></returns>
    private DataTable GetNoun2DataTable(string datatableName, string parentUnid)
    {
        DataTable dt = null;
        string strSql;

        try
        {
            strSql = string.Format("select * from dbo.fn_GetNoun2List('{0}')", parentUnid);
            dt = _db.Query(strSql, datatableName);
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return dt;
    }

    /// <summary>
    /// 將DataRow轉換成資料物件
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private zfNounData GetNounDate(DataRow row)
    {
        zfNounData data = new zfNounData();
        data.NounUnid = row["Unid"].ToString().Trim();
        data.NounCode = row["NounCode"].ToString().Trim();
        data.NounName = row["NounName"].ToString().Trim();
        data.NounDesc = row["NounDesc"].ToString().Trim();
        data.Reserve1 = row["Reserve1"].ToString().Trim();
        data.Reserve2 = row["Reserve2"].ToString().Trim();
        
        return data;
    }

    //private zfNounData GetNoun2Date(DataRow row)
    //{
    //    zfNounData data = new zfNounData();
    //    data.NounUnid = row["NounUnid"].ToString();
    //    data.NounCode = row["NounCode"].ToString();
    //    data.NounName = row["NounName"].ToString();
    //    data.ShowName = row["ShowName"].ToString();
    //    data.NounDesc = row["NounDesc"].ToString();
    //    data.Reserve1 = row["Reserve1"].ToString();
    //    data.Reserve2 = row["Reserve2"].ToString();
    //    data.Reserve3 = row["Reserve3"].ToString();

    //    return data;
    //}



    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Insert(zfNounData data)
    {
        SqlString sql;
        int effectcount = 0;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Insert, "zfNoun");
            sql.Add("NounUnid", data.NounUnid);
            sql.Add("NounCode", data.NounCode);
            sql.Add("NounName", data.NounName);
            sql.Add("ParentNounCode", data.ParentNounCode);
            sql.Add("ParentUnid", data.ParentUnid);
            sql.Add("SortValue", data.SortValue);
            sql.Add("NounDesc", data.NounDesc);
            if (data.Disable.Equals("True"))
                sql.Add("Disable", "true");
            else if (data.Disable.Equals("False"))
                sql.Add("Disable", "false");
            sql.Add("Reserve1", data.Reserve1);
            sql.Add("Reserve2", data.Reserve2);
            sql.Add("Reserve3", data.Reserve3);
            sql.Add("UseBySystem", data.UseBySystem);
            sql.Add("SystemCode", data.SystemCode);
            sql.Add("CreateUser", data.CreateUser);
            sql.Add("CreateDatetime", data.CreateDatetime, SqlString.Date2StrType.DateTime);

            effectcount = _db.Execute(sql.ToString());
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return effectcount;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Update(zfNounData data)
    {
        SqlString sql;
        int effectcount = 0;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Update, "zfNoun");
            sql.Add("NounCode", data.NounCode);
            sql.Add("NounName", data.NounName);
            //sql.Add("ParentNounCode", data.ParentNounCode);
            //sql.Add("ParentUnid", data.ParentUnid);
            if (data.Disable.Equals("True"))
                sql.Add("Disable", "False");
            else if (data.Disable.Equals("False"))
                sql.Add("Disable", "true");
            sql.Add("SortValue", data.SortValue);
            sql.Add("NounDesc", data.NounDesc);
            sql.Add("Reserve1", data.Reserve1);
            sql.Add("Reserve2", data.Reserve2);
            sql.Add("Reserve3", data.Reserve3);
            //sql.Add("UseBySystem", data.UseBySystem);
            sql.Add("SystemCode", data.SystemCode);
            sql.Add("UpdateUser", data.UpdateUser);
            sql.Add("UpdateDatetime", data.UpdateDatetime, SqlString.Date2StrType.DateTime);
            sql.Where.Add("NounUnid", data.NounUnid);

            effectcount = _db.Execute(sql.ToString());
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return effectcount;
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Delete(zfNounData data)
    {
        SqlString sql;
        int effectcount = 0;

        try
        {
            sql = new SqlString(SqlString.SqlCommandType.Delete, "zfNoun");
            sql.Where.Add("NounUnid", data.NounUnid);

            effectcount = _db.Execute(sql.ToString());
            _err = false;
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
            _err = true;
        }

        return effectcount;
    }













    //--------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------------

    /// <summary>
    /// 取得資料物件DataTable
    /// </summary>
    /// <returns></returns>
    public DataTable GetzfNounDT()
    {
        DataTable dt = null;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "zfNoun");
        sql.Add("*");
        string strsql = sql.ToString();
        try
        {
            dt = _db.Query(strsql, "zfNoun");
            this._err = false;
        }
        catch (Exception ex)
        {
            this._errMsg = ex.Message;
            this._err = true;
        }
        return dt;
    }

    public DataTable GetzfNounDT(string ParentUnid)
    {
        DataTable dt = null;
        SqlString sql = new SqlString(SqlString.SqlCommandType.Select, "zfNoun");
        sql.Add("*");
        sql.Where.Add("ParentUnid", ParentUnid);
        sql.OrderBy = "NounCode";
        string strsql = sql.ToString();
        try
        {
            dt = _db.Query(strsql, "zfNoun");
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
    public List<zfNounData> GetzfNounLT()
    {
        zfNounData data;
        List<zfNounData> datas = new List<zfNounData>();
        DataTable dt = this.GetzfNounDT();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                data = GetzfNounDate(dr);
                datas.Add(data);
            }
        }
        return datas;
    }
    /// <summary>
    /// 取得資料物件List集合
    /// </summary>
    /// <param name="ParentUnid">父階識別碼</param>
    /// <returns></returns>
    public List<zfNounData> GetzfNounLT(string ParentUnid)
    {
        zfNounData data;
        List<zfNounData> datas = new List<zfNounData>();
        DataTable dt = this.GetzfNounDT(ParentUnid);
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                data = GetzfNounDate(dr);
                datas.Add(data);
            }
        }
        return datas;
    }

    /// <summary>
    /// 查詢資料表zfNoun中欄位NounCode
    /// </summary>
    /// <param name="type">種類：NounUnid/ParentUnid...</param>
    /// <param name="Unid">識別碼</param>
    /// <returns></returns>
    public DataTable GetNounCodeDataDT(string type, string Unid)
    {
        DataTable dt = null;
        string sql = string.Empty;

        try
        {
            sql = string.Format("select NounCode from zfNoun where {0} = '{1}'", type, Unid);
            dt = _db.Query(sql, "zfNoun");
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
    /// 將DataRow轉換成資料物件
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private zfNounData GetzfNounDate(DataRow dr)
    {
        zfNounData data = new zfNounData();
        data.NounUnid = dr["NounUnid"].ToString();
        data.NounCode = dr["NounCode"].ToString();
        data.NounName = dr["NounName"].ToString();
        data.ParentNounCode = dr["ParentNounCode"].ToString();
        data.ParentUnid = dr["ParentUnid"].ToString();
        //data.SortValue = decimal.Parse(dr["SortValue"].ToString());
        data.SortValue = Ftool.DecimalTryParse(dr["SortValue"].ToString());
        data.NounDesc = dr["NounDesc"].ToString();
        data.Disable = dr["Disable"].ToString();
        data.Reserve1 = dr["Reserve1"].ToString();
        data.Reserve2 = dr["Reserve2"].ToString();
        data.Reserve3 = dr["Reserve3"].ToString();
        data.SystemCode = dr["SystemCode"].ToString();
        data.CreateUser = dr["CreateUser"].ToString();
        data.CreateDatetime = Ftool.DatetimeTryParse(dr["CreateDatetime"].ToString());
        data.UpdateUser = dr["UpdateUser"].ToString();
        data.UpdateDatetime = Ftool.DatetimeTryParse(dr["UpdateDatetime"].ToString());
        data.NounCodeNounName = dr["NounCode"].ToString() + "-" + dr["NounName"].ToString();
        return data;
    }

}



[Serializable]
public class zfNounData
{
    #region 資料物件屬性

    private string _NounUnid;
    /// <summary>
    /// NounUnid　名詞Unid
    /// </summary>
    public string NounUnid
    {
        set { _NounUnid = value; }
        get { return _NounUnid; }
    }

    private string _NounCode;
    /// <summary>
    /// NounCode　名詞代碼
    /// </summary>
    public string NounCode
    {
        set { _NounCode = value; }
        get { return _NounCode; }
    }

    private string _NounName;
    /// <summary>
    /// NounName　名詞名稱
    /// </summary>
    public string NounName
    {
        set { _NounName = value; }
        get { return _NounName; }
    }

    private string _ParentNounCode;
    /// <summary>
    /// ParentNameCode　父階完整名詞代碼 (已"\"接續)
    /// </summary>
    public string ParentNounCode
    {
        set { _ParentNounCode = value; }
        get { return _ParentNounCode; }
    }

    private string _ParentUnid;
    /// <summary>
    /// ParentUnid　父階Unid
    /// </summary>
    public string ParentUnid
    {
        set { _ParentUnid = value; }
        get { return _ParentUnid; }
    }

    private decimal _SortValue;
    /// <summary>
    /// SortValue　資料排序值
    /// </summary>
    public decimal SortValue
    {
        set { _SortValue = value; }
        get { return _SortValue; }
    }

    private string _NounDesc;
    /// <summary>
    /// NounDesc　名詞說明
    /// </summary>
    public string NounDesc
    {
        set { _NounDesc = value; }
        get { return _NounDesc; }
    }

    private string _Disable;
    /// <summary>
    /// Disable　停用 (1=停用；0=使用中)
    /// </summary>
    public string Disable
    {
        set { _Disable = value; }
        get { return _Disable; }
    }

    private string _Reserve1;
    /// <summary>
    /// Reserve1　保留欄位1
    /// </summary>
    public string Reserve1
    {
        set { _Reserve1 = value; }
        get { return _Reserve1; }
    }

    private string _Reserve2;
    /// <summary>
    /// Reserve2　保留欄位2
    /// </summary>
    public string Reserve2
    {
        set { _Reserve2 = value; }
        get { return _Reserve2; }
    }

    private string _Reserve3;
    /// <summary>
    /// Reserve3　保留欄位3
    /// </summary>
    public string Reserve3
    {
        set { _Reserve3 = value; }
        get { return _Reserve3; }
    }

    private string _UseBySystem;
    /// <summary>
    /// UseBySystem　系統專用 (1=系統專用名詞，使用只能修改名詞名稱及說明，其餘欄位鎖定；0 or Null 則為一般名詞)
    /// </summary>
    public string UseBySystem
    {
        set { _UseBySystem = value; }
        get { return _UseBySystem; }
    }

    private string _SystemCode;
    /// <summary>
    ///SystemCode　系統內定碼 (對應系統預定之代碼)
    /// </summary>
    public string SystemCode
    {
        set { _SystemCode = value; }
        get { return _SystemCode; }
    }

    private string _CreateUser;
    /// <summary>
    /// CreateUser　建檔人員
    /// </summary>
    public string CreateUser
    {
        set { _CreateUser = value; }
        get { return _CreateUser; }
    }

    private DateTime _CreateDatetime;
    /// <summary>
    ///  CreateDatetime　建檔時間
    /// </summary>
    public DateTime CreateDatetime
    {
        set { _CreateDatetime = value; }
        get { return _CreateDatetime; }
    }

    private string _UpdateUser;
    /// <summary>
    /// UpdateUser　更新人員
    /// </summary>
    public string UpdateUser
    {
        set { _UpdateUser = value; }
        get { return _UpdateUser; }
    }

    private DateTime _UpdateDatetime;
    /// <summary>
    /// UpdateDatetime　更新時間
    /// </summary>
    public DateTime UpdateDatetime
    {
        set { _UpdateDatetime = value; }
        get { return _UpdateDatetime; }
    }

    private string _ShowName;
    /// <summary>
    /// ShowName 顯示名稱 (父階名稱+子階名稱)
    /// </summary>
    public string ShowName
    {
        set { _ShowName = value; }
        get { return _ShowName; }
    }




    private string _NounCodeNounName;

    public string NounCodeNounName
    {
        set { _NounCodeNounName = value; }
        get { return _NounCodeNounName; }
    }
    #endregion

    /// <summary>
    /// 資料物件建構式
    /// </summary>
    public zfNounData()
    {
    }
}