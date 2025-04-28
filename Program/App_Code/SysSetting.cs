using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using BinaryTran;

/// <summary>
/// SysSetting 的摘要描述
/// </summary>
public class SysSetting
{

    public static class SystemName
    {
        /// <summary>
        /// 系統共用
        /// </summary>
        public const string SYS = "System";
        /// <summary>
        /// 薪資系統
        /// </summary>
        public const string Payroll ="ePayroll";
        /// <summary>
        /// 總帳系統
        /// </summary>
        public const string GL = "EBOSGL";
        /// <summary>
        /// 應收系統
        /// </summary>
        public const string AR = "AR";
        /// <summary>
        /// 應付系統
        /// </summary>
        public const string AP = "AP";
        /// <summary>
        /// iBOS雲端系統
        /// </summary>
        public const string iBOS = "EBOSWS";
    }

    public bool isTWCalendar;
    /// <summary>
    /// 今天日期(民國年)
    /// </summary>
    public string TWToday;
    public int YearB;
    public int YearE;
	public SysSetting()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
        isTWCalendar = GetCalendarSetting().ToUpper().Equals("Y");
        TWToday = ToTWDate(DateTime.Today);
        YearB = int.Parse(ConfigurationManager.AppSettings["YearBegin"].ToString());
        YearE = int.Parse(ConfigurationManager.AppSettings["YearEnd"].ToString());
	}
    
    //取得各種設定字串
    public string GetConfigString(string kind)
    {
        string retString = "";
        try
        {
            retString = ConfigurationManager.AppSettings[kind];
        }
        catch { }

        if (retString == null)
        {
            switch (kind)
            {
                case "DefCompany":
                    retString = ConfigurationManager.AppSettings["AdminPara"];
                    break;
                //case "testid":
                //    retString = ConfigurationManager.AppSettings["Test01"];
                //    break;
                //case "testpw":
                //    retString = ConfigurationManager.AppSettings["Test02"];
                //    break;
                case "VersionID":
                    retString = ConfigurationManager.AppSettings["VersionData"];
                    if (!string.IsNullOrEmpty(retString))
                        retString = rtnTrans(retString);
                    break;
                case "VersionNo":
                    retString = ConfigurationManager.AppSettings["VersionData"];
                    if (!string.IsNullOrEmpty(retString))
                        retString = rtnTrans(retString);
                    break;
                case "SYSMode":
                    retString = ConfigurationManager.AppSettings["VersionMode"];
                    if (!string.IsNullOrEmpty(retString))
                        retString = rtnTrans(retString).Replace("*", "");
                    break;
                case "picture":
                    retString = ConfigurationManager.AppSettings["FileKind01"];
                    break;
                case "skill":
                    retString = ConfigurationManager.AppSettings["FileKind02"];
                    break;
                case "BaseSalary":
                    try
                    {
                        retString = ConfigurationManager.AppSettings["BSName"];
                    }
                    catch { }

                    if (string.IsNullOrEmpty(retString))
                        retString = "底薪";
                    break;
            }
        }
        return retString;
    }

    /// <summary>
    /// 取得日曆設定
    /// </summary>
    /// <returns></returns>
    public string GetCalendarSetting()
    {
        return ConfigurationManager.AppSettings["TWCalendar"];
    }

    /// <summary>
    /// 西元日期轉為西元yyyy/MM/dd或民國yy/MM/dd格式後回傳
    /// </summary>
    /// <param name="theDate">西元日期</param>
    /// <returns>西元yyyy/MM/dd或民國yy/MM/dd</returns>
    public string FormatDate(string theDate)
    {
        if (isTWCalendar)
        {
            return ToTWDate(theDate);
        }
        else
        {
            try
            {
                return Convert.ToDateTime(theDate).ToString("yyyy/MM/dd");
            }
            catch 
            {
                return theDate;
            }
        }
    }

    /// <summary>
    /// 西元日期或民國日期轉為西元日期格式後回傳
    /// </summary>
    /// <param name="theDate">西元日期或民國日期</param>
    /// <returns>西元日期</returns>
    public string FormatADDate(string theDate)
    {
        if (isTWCalendar)
        {
            return ToADDate(theDate);
        }
        else
        {
            try
            {
                return Convert.ToDateTime(theDate).ToString("yyyy/MM/dd");
            }
            catch
            {
                return theDate;
            }
        }
    }

    /// <summary>
    /// 西元日期轉為民國日期
    /// </summary>
    /// <param name="theDate">西元日期</param>
    /// <returns>民國日期</returns>
    public string ToTWDate(DateTime theDate)
    {
        return (theDate.Year - 1911).ToString().PadLeft(2, '0') + theDate.ToString("/MM/dd");
    }

    /// <summary>
    /// 西元日期轉為民國日期
    /// </summary>
    /// <param name="theDate">西元日期</param>
    /// <returns>民國日期</returns>
    public string ToTWDate(string theDate)
    {
        string TWDate = theDate;

        if (theDate.Contains("/") == false && theDate.Length == 8)
        { 
         //將西元加上分隔符號
            theDate = theDate.Substring(0, 4) + "/" + theDate.Substring(4, 2) + "/" + theDate.Substring(6, 2);        
        }
        try
        {
            DateTime toDate = Convert.ToDateTime(theDate);
            TWDate = ToTWDate(toDate);
        }
        catch (Exception ex)
        {
            TWDate = ex.Message;
        }
        return TWDate;
    }

    /// <summary>
    /// 民國日期轉為西元日期
    /// </summary>
    /// <param name="theDate">民國日期</param>
    /// <returns>西元日期</returns>
    public string ToADDate(string theDate)
    {
        return GetADDate(theDate).ToString("yyyy/MM/dd");
    }

    /// <summary>
    /// 民國日期轉為西元日期
    /// </summary>
    /// <param name="theDate">民國日期</param>
    /// <returns>西元日期</returns>
    public DateTime GetADDate(string theDate)
    {   
        try
        {
            string[] listDate = theDate.Split(new char[] { '/' });
            theDate = (Convert.ToInt32(listDate[0]) + 1911).ToString() + "/" + listDate[1].PadLeft(2, '0') + "/" + listDate[2].PadLeft(2, '0');
            DateTime toDate = Convert.ToDateTime(theDate);
            return toDate;            
        }
        catch (Exception ex)
        {
            //ex.Message;
        }
        return Convert.ToDateTime("1912/01/01");
    }

    public string WeekShow(DayOfWeek theDayOfWeek)
    {
        string Message = theDayOfWeek.ToString();
        switch (theDayOfWeek)
        {
            case DayOfWeek.Monday:
                Message = "一";
                break;
            case DayOfWeek.Tuesday:
                Message = "二";
                break;
            case DayOfWeek.Wednesday:
                Message = "三";
                break;
            case DayOfWeek.Thursday:
                Message = "四";
                break;
            case DayOfWeek.Friday:
                Message = "五";
                break;
            case DayOfWeek.Saturday:
                Message = "六";
                break;
            case DayOfWeek.Sunday:
                Message = "日";
                break;
        }
        return Message;
    }

    /// <summary>
    /// 取得錯誤訊息描述
    /// </summary>
    /// <param name="ErrCode"></param>
    /// <returns></returns>
    public string ErrMsg(string ErrCode)
    {
        string Message = ErrCode;
        switch (ErrCode)
        {
            case "AuthError":
                Message = "系統授權檢核失敗!請洽泛太資訊!";
                break;
            case "NoRight":
                Message = "無使用此功能之權限!";
                break;
            case "UserNoEID":
                Message = "權限不足或無可對應之員工代號!";
                break;
            case "UnLogin":
                Message = "連線逾時或尚未登入!!";
                break;
            case "TimeOut":
                Message = "連線逾時!";
                break;
            case "Parameterless":
                Message = "參數不足!";
                break;
            case "DataError":
                Message = "資料錯誤!";
                break;
            case "ProgramDevelopment":
                Message = "程式開發中!";
                break;
        }
        return Message;
    }

    /// <summary>
    /// 取得DB錯誤訊息描述
    /// </summary>
    /// <param name="ErrCode"></param>
    /// <returns></returns>
    public string DBReturnMsg(int ReturnCode)
    {
        string Message = "";

        Message = "DB指令執行成功!共影響資料列數 " + ReturnCode.ToString();

        switch (ReturnCode)
        {
            case -1:
                Message = "DB指令執行失敗!請查閱錯誤LOG";
                break;
            case 0:
                Message = "DB指令執行成功!但未影響任何資料列";
                break;
        }
        return Message;
    }

    public string PYReturnMsg(int ReturnCode)
    {
        string Message = "";

        Message = "確認試算完成!請查閱紀錄";

        switch (ReturnCode)
        {
            case 13:
                Message = "確認成功完成!";
                break;
            case -99:
                Message = "無確認試算的權限!";
                break;
            case -1:
                Message = "DB指令執行失敗!請查閱錯誤LOG";
                break;
            case -2:
                Message = "指定之計算年月未經控管!";
                break;
            case -3:
                Message = "指定之計薪年月尚未試算";
                break;
            case -4:
                Message = "指定之計薪年月已完成確認,不可再進行確認";
                break;
            case -6:
                Message = "指定之計薪年月非待確認之年月,請先試算並確認正確年月薪資";
                break;
            case -7:
                Message = "指定之計薪年月最後試算時間早於上月份薪資試算或確認時間,請重新試算!";
                break;
            case -10:
                Message = "指定之計薪年月已完成確認,不可再進行試算";
                break;
            case -11:
                Message = "指定之計薪年月早於已完成確認薪資之年月,不可再進行試算或確認";
                break;                
            case -5:
                Message = "DB指令執行失敗!請查閱錯誤LOG";
                break;
        }

        return Message;
    }

    /////<summary>
    /////加密(使用加密元件)
    /////</summary>
    /////<param name="Pswd">未加密字串</param>
    /////<returns>已加密字串</returns>
    /////<remarks>字串加密</remarks>
    //public string rtnHash(string Pswd)
    //{
    //    object HashStr;
    //    BinTranClass1 BT = new BinTranClass1();

    //    HashStr = Pswd;

    //    BT.PasswordTranA(Pswd,ref HashStr);
    //    //'BT.PasswordTranB(Pswd, HashStr)
    //    //'BT.BinaryTransform()
    //    //'BT.TransToBinary()
    //    HashStr = HashStr.ToString().Substring(Pswd.Length);

    //    return HashStr.ToString();
    //}
    /////<summary>
    /////解密(使用加密元件)
    /////</summary>
    /////<param name="Pswd">加密字串</param>
    /////<returns>解密後字串</returns>
    /////<remarks>字串解密</remarks>
    //public string rtnTrans(string Pswd)
    //{
    //    object HashStr;
    //    BinTranClass1 BT = new BinTranClass1();

    //    HashStr = Pswd;

    //    try
    //    {
    //        BT.PasswordTranB(Pswd, ref HashStr);
    //        //'BT.PasswordTranB(Pswd, HashStr)
    //        //'BT.BinaryTransform()
    //        //'BT.TransToBinary()
    //    }
    //    catch
    //    {
    //        return Pswd;
    //    }
    //    HashStr = HashStr.ToString().Substring(Pswd.Length);

    //    return HashStr.ToString();
    //}

    ///// <summary>
    ///// 加密為Binary
    ///// </summary>
    ///// <param name="Pswd">未加密字串</param>
    ///// <returns>Binary</returns>
    ///// <remarks>加密為Binary</remarks>
    //public byte[] rtnTransB(String Pswd)
    //{
    //    object HashStr;
    //    BinTranClass1 BT = new BinTranClass1();

    //    HashStr = Pswd;
    //    BT.TransToBinary(Pswd, ref HashStr);
    //    //HashStr = HashStr.Substring(Pswd.Length)

    //    return (Byte[])HashStr;
    //}
    
    ///<summary>
    ///加密
    ///</summary>
    ///<param name="Pswd">未加密字串</param>
    ///<returns>已加密字串</returns>
    ///<remarks>字串加密</remarks>
    public string rtnHash(string Pswd)
    {
        string HashStr;
        DotNetBinaryTran DNBT = new DotNetBinaryTran();
        HashStr = DNBT.rtnHash(Pswd);
        return HashStr;
    }

    ///<summary>
    ///解密
    ///</summary>
    ///<param name="Pswd">加密字串</param>
    ///<returns>解密後字串</returns>
    ///<remarks>字串解密</remarks>
    public string rtnTrans(string Pswd)
    {
        string HashStr;
        HashStr = Pswd;
        DotNetBinaryTran DNBT = new DotNetBinaryTran();
        HashStr = DNBT.rtnCode(Pswd);
        return HashStr;
    }

    ///<summary>
    ///數字解密
    ///</summary>
    ///<param name="Pswd">加密字串</param>
    ///<returns>解密後字串</returns>
    ///<remarks>字串解密</remarks>
    public string rtnTransAmount(string Pswd)
    {
        string HashStr;
        HashStr = rtnTrans(Pswd);
        HashStr = HashStr.Replace("h", "00");
        HashStr = HashStr.Replace("t", "000");
        return HashStr;
    }


    /// <summary>
    /// 加密為Binary
    /// </summary>
    /// <param name="Pswd">未加密字串</param>
    /// <returns>Binary</returns>
    /// <remarks>加密為Binary</remarks>
    public byte[] rtnTransB(String Pswd)
    {
        object HashStr;
        HashStr = Pswd;
        DotNetBinaryTran DNBT = new DotNetBinaryTran();
        HashStr = DNBT.rtnBinary(Pswd);
        return (Byte[])HashStr;
    }

    /////<summary>
    ///// Binary解密
    ///// </summary>
    ///// <param name="Pswd">Binary</param>
    ///// <returns>解密後字串</returns>
    ///// <remarks>Binary解密</remarks>
    //public string rtnB2S(Byte[] Pswd)
    //{
    //    object HashStr;
    //    BinTranClass1 BT = new BinTranClass1();

    //    HashStr = Pswd.ToString();
    //    BT.BinaryTransform(Pswd, ref HashStr);

    //    return HashStr.ToString();
    //}

    public string chekPic(string theFileName)
    {
        string[] FileStyleList = { ".bmp", ".jpg", ".jpeg", ".gif", ".png" };
        string tempFileName = ""; ;
        for (int i = 0; i < FileStyleList.Length; i++)
        {
            tempFileName = theFileName + FileStyleList[i];
            if (System.IO.File.Exists(tempFileName))
                return FileStyleList[i];
        }
        return "";
    }

    public string WriteToLogs(String LogFile, String Msg)
    {
        LogFile = LogFile.Replace(":", "_");
        String path = HttpContext.Current.Application["DeBugLog"].ToString() + String.Format("Log_{0}_{1}.txt", LogFile, DateTime.Now.ToString("yyyyMMdd"));        
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();
            //fileInfo = new System.IO.FileInfo("C:\\" + String.Format("Log_{0}_{1}.txt", LogFile, DateTime.Now.ToString("yyyyMMdd")));
        }

        try
        {
            System.IO.StreamWriter sw = fileInfo.AppendText();
            sw.WriteLine("" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "----");
            sw.WriteLine(Msg);
            sw.WriteLine("-------------------------------------------");
            sw.Flush();
            sw.Close();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("拒絕存取"))
            {
                try
                {
                    fileInfo.IsReadOnly = false;
                }
                catch { }
                if (!LogFile.Contains("_WTLtemp"))
                    WriteToLogs(LogFile + "_WTLtemp", Msg);
            }
        }
        return path;
    }

    public string WriteTofiles(String path ,String FileName, String Msg)
    {
        FileName = FileName.Replace(":", "_");
        string theFullPath = path + (path.EndsWith("\\") ? "" : "\\") + FileName;
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(theFullPath);
        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();            
        }

        try
        {
            System.IO.StreamWriter sw = fileInfo.AppendText();
            //因為本網站為UTF-8編碼,存成ANSI中文會亂碼,故需ENDUSER行另存
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(theFullPath, true, System.Text.Encoding.ASCII);
            sw.WriteLine(Msg);            
            sw.Flush();
            sw.Close();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("拒絕存取"))
            {
                try
                {
                    fileInfo.IsReadOnly = false;
                }
                catch { }
            }
        }
        return path;
    }

    /// <summary>
    /// 檢核中文字/全形字長度,如:一個中文長為2
    /// </summary>
    /// <param name="Source">原始字串</param>
    /// <param name="ChineseLen">中文/全形字以長度1計或以2計</param>
    /// <returns></returns>
    public int CheckChineseLen(string Source, int ChineseLen)
    {
        int Result = 0;
        if (Source == "")
            return 0;
        else
        {            
            string tempChar = Source.Substring(0, 1);
            int iCheck = 0;
            if (Int32.TryParse(tempChar, out iCheck) != true)
            {
                iCheck = -1;
            }

            if (iCheck != -1 && iCheck < 128)
            {
                Result = 1 + CheckChineseLen(Source.Substring(1, Source.Length - 1), ChineseLen);
            }
            else
            {
                Result = ChineseLen + CheckChineseLen(Source.Substring(1, Source.Length - 1), ChineseLen);
            }
        }
        return Result;
    }

    /// <summary>
    /// 判斷是否含非法字元
    /// </summary>
    /// <param name="mString"></param>
    /// <returns></returns>
    public object CleanSQL(object mmStr)
    {
        string mString = mmStr.ToString();

        if (mString == null)
            mString = "";
        else
        {
            mString = mString.Replace("'", "''");
            mString = mString.Replace("--", "");
            mString = mString.Replace("/*", "");
            mString = mString.Replace("*/", "");
            mString = mString.Replace(";", "；");
        }
        return mString;
    }
}
