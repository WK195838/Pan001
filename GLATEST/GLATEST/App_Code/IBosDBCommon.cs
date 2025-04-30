using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// 共用參數
/// </summary>
public class IBosDBCommon
{
	public IBosDBCommon()
	{
	}

    static public int GetEnvironmentType
    {
        get
        {
            string sENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENT"];
            int iENVIRONMENT = 10;
            int.TryParse(sENVIRONMENT, out iENVIRONMENT);
            return iENVIRONMENT;
        }
    }

    static public int GetEnvironmentTypeSQL
    {
        get
        {
            string sENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENTSQL"];
            int iENVIRONMENT = 10;
            int.TryParse(sENVIRONMENT, out iENVIRONMENT);
            return iENVIRONMENT;
        }
    }

    //AS400資料Library Name
    /// <summary>
    /// ESPLPRDDTA
    /// </summary>
    static public string DataLibraryName
    {
        get { return "ESPLPRDDTA"; }
    }

    /// <summary>
    /// ESPLPRDSRC
    /// </summary>
    public static string ApLibraryName
    {
        get { return "ESPLPRDSRC"; }
    }

    /// <summary>
    /// 設定的資料庫連線
    /// </summary>
    public static string ConnectionString
    {
        get
        {
            string connstring = string.Empty;

            connstring = ConfigurationManager.ConnectionStrings["IBosDB"].ConnectionString;

            return connstring;
        }
    }
}