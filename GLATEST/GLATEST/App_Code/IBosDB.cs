using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Security.Principal;
using DBClass;

/// <summary>
/// SQL YKKDB
/// </summary>
public class IBosDB : DBClassBass
{
    public IBosDB()
    {
        _err = false;
        _errMsg = "";

        //string contring = DefaultSqlDB2.getDefaultSqlDB(
        //        _env,
        //        DefaultSqlDB2.DEFAULT_DB_DocManagement).getConnectionString();

        string contring = IBosDBCommon.ConnectionString;

        contring += ";Max Pool Size=300;";

        this.Connection = new SqlConnection(contring);
    }
    public IBosDB(object instance)
	{
        _err = false;
        _errMsg = "";

        //string contring = DefaultSqlDB2.getDefaultSqlDB(
        //        _env,
        //        DefaultSqlDB2.DEFAULT_DB_DocManagement).getConnectionString();

        string contring = IBosDBCommon.ConnectionString;

        contring += ";Max Pool Size=300;";

        this.Connection = new SqlConnection(contring);
	}

    public DataSet GetSqlDataSet(SqlCommand cmd)
    {
        string cnn = IBosDBCommon.ConnectionString;
        using (SqlConnection con = new SqlConnection(cnn))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    return ds;
                }
            }
        }
    }
}