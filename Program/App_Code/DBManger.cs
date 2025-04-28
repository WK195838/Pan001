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

    /// <summary>
    /// DBManger 的摘要描述
    /// </summary>
public class DBManger
{
    SysSetting _SysSet = new SysSetting();
    string _ConnString = "";
     public enum ConnectionString : int 
    {
        MyConnectionString=0,
        EBOSDB = 1,
        DotNetTestDB = 2,
        EbosFlow = 3,
        UNIPLAN_EBOS=4,
    }

    public DBManger()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }
           
    /// <summary>
    /// 新增資料庫連線(使用預設連線)
    /// </summary>
    public void New()
    {
        _ConnString = GetConnectionString();
    }

    /// <summary>
    /// 指定資料庫連線(使用指定之DB連線)
    /// </summary>
    public void New(ConnectionString Connect)
    {
        _ConnString = GetConnectionString(Connect);
    }

    /// <summary>
    /// 新增資料庫連線(使用傳入之指定連線)
    /// </summary>
    public void New(string ConnString)
    {
        _ConnString = ConnString;
    }

    public string GetConnectionString(ConnectionString Connect)
    {
        string strConn="";
        switch(Connect)
        {
            case ConnectionString.MyConnectionString:
                strConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
                break;
            case ConnectionString.EBOSDB:
                strConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["EBOSDB"].ConnectionString;
                break;
            case ConnectionString.DotNetTestDB:
                strConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DotNetTestDB"].ConnectionString;
                break;
            case ConnectionString.EbosFlow:
                strConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["EbosFlow"].ConnectionString;
                break;
            case ConnectionString.UNIPLAN_EBOS:
                strConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["UNIPLAN_EBOS"].ConnectionString;
                break;
                
        }
        return strConn;
    }

    public string GetConnectionString()
    {
        return GetConnectionString(ConnectionString.MyConnectionString);
    }

    /// <summary>
    /// 取得資料庫版本
    /// </summary>
    /// <returns>80../90../10..</returns>
    public string ServerVersion()
    {
        SqlConnection SqlConn = new SqlConnection(_ConnString);
        SqlConn.Open();
        string strServerVersion = SqlConn.ServerVersion;
        SqlConn.Close();
        return strServerVersion;
    }

    /// <summary>
    /// 傳回資料集
    /// </summary>
    /// <param name="dt">資料表</param>
    /// <returns></returns>
    public DataSet retDataSet(DataTable dt)
    {
        DataSet ds = new DataSet();
        if (dt != null)
            ds.Tables.Add(dt);
        return ds;
    }

    /// <summary>
    /// 回傳資料表或檢視表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <returns></returns>
    public DataTable ExecStoredProcedure(String sql)
    {
        SqlCommand MyCmd = new SqlCommand("");        
        return ExecStoredProcedure(sql, MyCmd.Parameters);
    }

    /// <summary>
    /// 回傳資料表或檢視表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <param name="sqlPar">T-SQL參數</param>
    /// <returns></returns>
    public DataTable ExecStoredProcedure(String sql, SqlParameterCollection sqlPar)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sda = new SqlDataAdapter(sql, _ConnString);
        sda.SelectCommand.CommandType = CommandType.StoredProcedure;

        try
        {            
            foreach(SqlParameter theSqlPar in sqlPar)
            {
                sda.SelectCommand.Parameters.Add(theSqlPar.ParameterName, theSqlPar.SqlDbType).Value = theSqlPar.Value;
            }
            int LastInsertedID = sda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            _SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(ExecStoredProcedure)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + sql + "\n\r");
            try
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);
                if (ex.Message.Contains("Server") || sqlEx.Number.Equals(53) || sqlEx.Number.Equals(18456))
                    throw new Exception("Error SQL Connect. \r\n<BR>" + ex);
            }
            catch
            {
            }
        }
        finally
        {
            sda.Dispose();
            dt.Dispose();
        }
        return null;
    }

    /// <summary>
    /// 回傳整數值,並接收資料集
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <param name="sqlPar">T-SQL參數</param>
    /// <param name="dt">回傳資料表或檢視表</param>
    /// <returns></returns>
    public int ExecStoredProcedure(String sql, SqlParameterCollection sqlPar, out DataSet ds )
    {
        ds = new DataSet();
        int LastInsertedID = 0;
        SqlDataAdapter sda = new SqlDataAdapter(sql, _ConnString);
        sql = sql.ToLower();
        if (sql.StartsWith("select") || sql.Contains("from"))
            sda.SelectCommand.CommandType = CommandType.Text;
        else
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;

        try
        {
            foreach (SqlParameter theSqlPar in sqlPar)
            {
                sda.SelectCommand.Parameters.Add(theSqlPar.ParameterName, theSqlPar.SqlDbType).Value = theSqlPar.Value;
            }
            LastInsertedID = sda.Fill(ds);
            return LastInsertedID;
        }
        catch (Exception ex)
        {
            _SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(ExecStoredProcedure)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + sql + "\n\r");
            try
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);
                if (ex.Message.Contains("Server") || sqlEx.Number.Equals(53) || sqlEx.Number.Equals(18456))
                    throw new Exception("Error SQL Connect. \r\n<BR>" + ex);
            }
            catch
            {
            }
        }
        finally
        {
            sda.Dispose();
            ds.Dispose();
        }
        return LastInsertedID;
    }

    /// <summary>
    /// 回傳資料表或檢視表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <returns></returns>
    public DataTable ExecuteDataTable(String sql)
    {
        SqlCommand MyCmd = new SqlCommand("");
        CommandType CommType = CommandType.Text;
        return ExecuteDataTable(sql, MyCmd.Parameters, CommType);
    }

    public DataTable ExecuteDataTable(String sql, out String ErrMsg)
    {
        SqlCommand MyCmd = new SqlCommand("");
        CommandType CommType = CommandType.Text;
        return ExecuteDataTable(sql, MyCmd.Parameters, CommType, out ErrMsg);
    }

    public DataTable ExecuteDataTable(String sql, SqlParameterCollection sqlPar, CommandType CommType)
    {   
        string ErrMsg = "";
        return ExecuteDataTable(sql, sqlPar, CommType, out ErrMsg);
    }

    /// <summary>
    /// 回傳資料表或檢視表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <param name="sqlPar">T-SQL參數</param>
    /// <param name="CommType">CommType</param>
    /// <param name="ErrMsg">錯誤訊息</param>
    /// <returns></returns>
    public DataTable ExecuteDataTable(String sql, SqlParameterCollection sqlPar, CommandType CommType, out String ErrMsg)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter sda = new SqlDataAdapter(sql, _ConnString);
        sda.SelectCommand.CommandType = CommType;
        ErrMsg = "";
        try
        {
            foreach(SqlParameter theSqlPar in sqlPar)
            {
                sda.SelectCommand.Parameters.Add(theSqlPar.ParameterName, theSqlPar.SqlDbType).Value = theSqlPar.Value;
            }
            sda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {            
            _SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(ExecuteDataTable)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + sql + "\n\r");
            ErrMsg = ex.Message;
            try
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);
                if (ex.Message.Contains("Server") || sqlEx.Number.Equals(53) || sqlEx.Number.Equals(18456))
                {
                    ErrMsg = "Error SQL Connect. \r\n<BR>" + ex.Message;
                    throw new Exception("Error SQL Connect. \r\n<BR>" + ex);
                }
            }
            catch
            {
            }
        }
        finally
        {
            sda.Dispose();
            dt.Dispose();
        }
        return null;
    }

    /// <summary>
    /// 執行命令無回傳資料表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <returns></returns>
    public int ExecuteCommand(String sql)
    {
        SqlCommand MyCmd = new SqlCommand("");
        CommandType CommType = CommandType.Text;
        return ExecuteCommand(sql, MyCmd.Parameters, CommType);
    }

    /// <summary>
    /// 執行命令無回傳資料表
    /// </summary>
    /// <param name="sql">T-SQL</param>
    /// <param name="sqlPar">T-SQL參數</param>
    /// <param name="CommType">CommType</param>
    /// <returns></returns>
    public int ExecuteCommand(String sql, SqlParameterCollection sqlPar, CommandType CommType)
    {
        SqlConnection conn = new SqlConnection(_ConnString);
        SqlCommand sc = new SqlCommand(sql, conn);
        sc.CommandType = CommType;

        int result = -1;

        try
        {
            conn.Open();

            foreach (SqlParameter theSqlPar in sqlPar)
            {
                sc.Parameters.Add(theSqlPar.ParameterName, theSqlPar.SqlDbType).Value = theSqlPar.Value;
            }

            result = sc.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(ExecuteCommand)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + sql + "\n\r");
            try
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);
                if (ex.Message.Contains("Server") || sqlEx.Number.Equals(53) || sqlEx.Number.Equals(18456))
                    throw new Exception("Error SQL Connect. \r\n<BR>" + ex);
            }
            catch
            {
            }
        }
        finally
        {
            conn.Dispose();
            sc.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 寫入或更新異動LOG
    /// </summary>
    /// <param name="sqlPar">T-SQL參數</param>
    /// <returns></returns>
    public int DataChgLog(SqlParameterCollection sqlPar)
    {
        string LogStep = "";



        if (sqlPar.Contains("@ChgStopDateTime"))
        {
            LogStep = "1";
            if (sqlPar["@SQLcommand"].ToString().Equals("DeleteThisLog"))
                LogStep = "2";
        }
        else
            LogStep = "0";


        string sql = "";
        sql = "SELECT @SQLcommand FROM DataChangeLog Where TableName=@TableName And TrxType=@TrxType And ChangItem=@ChangItem " +
            " And ChgStartDateTime=@ChgStartDateTime And ChgUser=@ChgUser ";
        
        switch (LogStep)
        {
            case "0":
                sql = "INSERT INTO DataChangeLog(TableName,TrxType,ChangItem,SQLcommand,ChgStartDateTime,ChgUser)" +
                    "  VALUES(@TableName,@TrxType,@ChangItem,@SQLcommand,@ChgStartDateTime,@ChgUser)";                
                break;
            case "1":
                sql = "Update DataChangeLog Set ChgStopDateTime=@ChgStopDateTime, SQLcommand=@SQLcommand+':'+SQLcommand" +
                    "  Where TableName=@TableName And TrxType=@TrxType And ChangItem=@ChangItem" +
                    "  And ChgStartDateTime=@ChgStartDateTime And ChgUser=@ChgUser ";
                break;
            case "2":
                sql = "Delete From DataChangeLog " +
                    "  Where TableName=@TableName And TrxType=@TrxType And ChangItem=@ChangItem" +
                    "  And ChgStartDateTime=@ChgStartDateTime And ChgUser=@ChgUser ";
                break;
        }

        SqlConnection conn = new SqlConnection(_ConnString);
        SqlCommand sc = new SqlCommand(sql, conn);
        
        int result = 0;

        try
        {
            conn.Open();

            foreach (SqlParameter theSqlPar in sqlPar)
            {
                sc.Parameters.Add(theSqlPar.ParameterName, theSqlPar.SqlDbType).Value = theSqlPar.Value;
            }
            //結束時間預設為10秒後
            if (LogStep.Equals("0"))
                sc.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddSeconds(10);

            result = sc.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(DataChgLog)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + sql + "\n\r");
            try
            {
                System.Data.SqlClient.SqlException sqlEx = ((System.Data.SqlClient.SqlException)ex);
                if (ex.Message.Contains("Server") || sqlEx.Number.Equals(53) || sqlEx.Number.Equals(18456))
                    throw new Exception("Error SQL Connect. \r\n<BR>" + ex);
            }
            catch
            {
            }
        }
        finally
        {
            conn.Dispose();
            sc.Dispose();
        }
        return result;
    }
    
}
