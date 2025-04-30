using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class Ftool
{
    /// <summary>
    /// string轉換為int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int IntTryParse(string str)
    {
        int i = 0;
        int.TryParse(str, out i);
        return i;
    }
    /// <summary>
    /// string轉換為decimal
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static decimal DecimalTryParse(string str)
    {

        decimal i = 0.0M;
        decimal.TryParse(str, out i);
        return i;
    }
    /// <summary>
    /// string轉換為float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float FloatTryParse(string str)
    {

        float i = 0.0f;
        float.TryParse(str, out i);
        return i;
    }
    /// <summary>
    /// string轉換為double
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static double DoubleTryParse(string str)
    {

        double i = 0.0d;
        double.TryParse(str, out i);
        return i;
    }
    public static DateTime DatetimeTryParse(string str)
    {

        DateTime i = DateTime.MaxValue;
        DateTime.TryParse(str, out i);
        return i;
    }
    public static string Space(int nb)
    {

        string sp = string.Empty;
        for (int i = 0; i < nb; i++)
        {
            sp += " ";
        }
        return sp;
    }
    /// <summary>
    /// 將本機路徑轉換成相對路徑
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string LUrlConverUrl(string lurl)
    {
        //string rootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());    
        string rootDir = HttpContext.Current.Request.ApplicationPath;
        string url = lurl.Replace(rootDir, ""); 
        url = url.Replace(@"\", @"/");

        return url;
    }

    /// <summary>
    /// 相對路徑轉換成IIS的物理路徑
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string UrlConverLUrl(string url)
    {
        string rootDir = System.Web.HttpContext.Current.Request.ApplicationPath;
        string lurl = rootDir + url.Replace(@"/", @"\"); //轉換成絶對路徑

        return lurl;
    }

    /// <summary>
    /// 取得副檔的MIME格式
    /// </summary>
    /// <param name="extname"></param>
    /// <returns></returns>
    public static string GetMIME(string extname)
    {
        string MIME = string.Empty;
        switch (extname)
        {
            case ".gif":
                MIME = "image/gif";
                break;
            case ".jpg":
                MIME = "image/jpg";
                break;
            case ".png":
                MIME = "image/png";
                break;
            case ".txt":
                MIME = "text/plain";
                break;
            case ".xls":
                MIME = "application/vnd.ms-excel";
                break;
            case ".doc":
                MIME = "application/msword";
                break;
            case ".ppt":
                MIME = "application/vnd.ms-powerpoint";
                break;
            case ".pdf":
                MIME = "application/pdf";
                break;
            case ".zip":
                MIME = "application/zip";
                break;
            case ".rar":
                MIME = "application/rar";
                break;
            default:
                break;
        }

        return MIME;
    }
}
