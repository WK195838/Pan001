using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Management;
using System.Management.Instrumentation;

namespace PanPacificClass
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class UserData
    {
        /// <summary>
        /// 登入系統
        /// </summary>
        public string SystemNo;
        /// <summary>
        /// 登入用:公司代碼
        /// </summary>
        public string CompanyCode;
        /// <summary>
        /// 登入用:公司名稱
        /// </summary>
        public string CompanyName;
        /// <summary>
        /// 登入用:角色
        /// </summary>
        public string Role;
        /// <summary>
        /// 登入用:帳號
        /// </summary>
        public string UserId;
        /// <summary>
        /// 登入用:登入者姓名
        /// </summary>
        public string UserName;
        /// <summary>
        /// ERP系統公司代號
        /// </summary>
        public string Company;
        /// <summary>
        /// ERP系統員工代號
        /// </summary>
        public string EmployeeId;
        /// <summary>
        /// ERP系統員工部門代號
        /// </summary>
        public string DeptId;
        /// <summary>
        /// ERP系統員工部門識別用單一ID
        /// </summary>
        public string DeptUnid;
        /// <summary>
        /// ERP系統員工部門名稱
        /// </summary>
        public string DeptName;
        /// <summary>
        /// 登入IP
        /// </summary>
        public string IP;
        /// <summary>
        /// 登入用:SessionID
        /// </summary>
        public string SessionID;
        /// <summary>
        /// 密碼到期日
        /// </summary>
        public DateTime PWDdueDate;
    }

    /// <summary>
    /// 員工個人資料
    /// </summary>
    public class PersonalData
    {
        /// <summary>
        /// 公司
        /// </summary>
        public string Company;
        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName;
        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmployeeId;
        /// <summary>
        /// 中文姓名
        /// </summary>
        public string EmployeeName;
        /// <summary>
        /// 英文姓名
        /// </summary>
        public string EnglishName;
        /// <summary>
        /// 部門代號
        /// </summary>
        public string DeptId;
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DeptName;
        /// <summary>
        /// 職稱代號
        /// </summary>
        public string TitleCode;
        /// <summary>
        /// 職等編號
        /// </summary>
        public string Grade;
        /// <summary>
        /// 班別代號
        /// </summary>
        public string Shift;
        /// <summary>
        /// 編制別
        /// </summary>
        public string Identify;
        /// <summary>
        /// 計薪代號
        /// </summary>
        public string PayCode;
        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType;
        /// <summary>
        /// 身份証號
        /// </summary>
        public string IDNo;
        /// <summary>
        /// 身份識別
        /// </summary>
        public string IDType;
        /// <summary>
        /// 性別
        /// </summary>
        public string Sex;
        /// <summary>
        /// 國籍
        /// </summary>
        public string Nationality;
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDate;

        /// <summary>
        /// 離職碼
        /// </summary>
        public string ResignCode;
        /// <summary>
        /// 到職日期
        /// </summary>
        public string HireDate;
        /// <summary>
        /// 留職停薪日
        /// </summary>
        public string LeaveWithoutPay;
        /// <summary>
        /// 復職日期
        /// </summary>
        public string ReHireDate;
        /// <summary>
        /// 離職日期
        /// </summary>
        public string ResignDate;
        /// <summary>
        /// 試用期滿日
        /// </summary>
        public string ObserveExpirationDate;
        /// <summary>
        /// 最近調陞日
        /// </summary>
        public string LstPromotionDate;
        /// <summary>
        /// 最近調薪日
        /// </summary>
        public string LstChangeSalaryDate;
        /// <summary>
        /// 福委會加入
        /// </summary>
        public string LWC;
        /// <summary>
        /// 工會加入
        /// </summary>
        public string Union;
        /// <summary>
        /// 特加年資(月數)
        /// </summary>
        public string SpecialSeniority;
        /// <summary>
        /// 婚姻狀況
        /// </summary>
        public string MaritalStatus;
        /// <summary>
        /// 撫養人數
        /// </summary>
        public string DependentsNum;
        /// <summary>
        /// 兵役
        /// </summary>
        public string Military;
        /// <summary>
        /// 出生地
        /// </summary>
        public string BornPlace;
        /// <summary>
        /// 通訊地址
        /// </summary>
        public string Addr;
        /// <summary>
        /// 戶籍地址
        /// </summary>
        public string ResidenceAddr;
        /// <summary>
        /// 通訊電話
        /// </summary>
        public string TEL;
        /// <summary>
        /// 手機No.
        /// </summary>
        public string MobilPhone;
        /// <summary>
        /// E Mail Address
        /// </summary>
        public string Email;
        /// <summary>
        /// 連絡人
        /// </summary>
        public string Contact;
        /// <summary>
        /// 保証人１
        /// </summary>
        public string Guarantor1;
        /// <summary>
        /// 保証人２
        /// </summary>
        public string Guarantor2;
        /// <summary>
        /// 介紹人
        /// </summary>
        public string Introducer;
        /// <summary>
        /// 連絡人電話
        /// </summary>
        public string ContactTEL;
        /// <summary>
        /// 保証人１電話
        /// </summary>
        public string Guarantor1TEL;
        /// <summary>
        /// 保証人２電話
        /// </summary>
        public string Guarantor2TEL;
        /// <summary>
        /// 介紹人電話
        /// </summary>
        public string IntroducerTEL;
        /// <summary>
        /// 成本中心
        /// </summary>
        public string CCN;
        /// <summary>
        /// 學歷代碼
        /// </summary>
        public string EducationCode;
    };

    /// <summary>
    /// PanPacificClass 的摘要描述
    /// </summary>
    public class PanPacificClass
    {
        public PanPacificClass()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }
    }

    // ========================================================================================================
    /// <summary>
    /// 使用到 JavaScript 相關功能之共用函數
    /// </summary>
    // ========================================================================================================
    public class JsUtility
    {
        /// <summary>
        /// 註冊 Client 端 JavaScript 使用 Alert() 彈出訊息視窗
        /// </summary>
        /// <param name="msg">訊息字串</param>
        /// <param name="page">Page</param>
        /// <param name="key">唯一Key的名稱，可以為空白由系統自動產生</param>
        /// <returns></returns>
        /// <remarks>  JsUtility.ClientMsgBox("Message", Page, "");</remarks>
        public static void ClientMsgBox(string msg, Page page, string key)
        {
            string script = "";

            msg = msg.Replace("'", "");
            if (key == "")
            {
                Random autoRand = new Random();
                key = "clientScriptMessage" + Environment.TickCount.ToString() + autoRand.Next() + Environment.TickCount.ToString();
            }
            script += "<script language=JavaScript>";
            script += "alert('" + msg + "');";
            script += "</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), key, script);

        }

        /// <summary>
        /// 註冊 Ajax Client 端 JavaScript 使用 Alert() 彈出訊息視窗
        /// </summary>
        /// <param name="msg">訊息字串</param>
        /// <param name="upPanel">UpdatePanel的ID</param>
        /// <param name="key">唯一Key的名稱，可以為空白由系統自動產生</param>
        /// <returns></returns>
        /// <remarks>  JsUtility.ClientMsgBoxAjax("Message", UpdatePanel的ID, "");</remarks>
        public static void ClientMsgBoxAjax(string msg, System.Web.UI.UpdatePanel upPanel, string key)
        {
            string script = "";

            msg = msg.Replace("'", "");
            if (key == "")
            {
                Random autoRand = new Random();
                key = "clientScriptMessage" + Environment.TickCount.ToString() + autoRand.Next() + Environment.TickCount.ToString();
            }
            script += "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(upPanel, upPanel.GetType(), key, script, true);
        }

        /// <summary>
        /// 關閉執行等待的畫面
        /// </summary>
        /// <param name="upPanel">UpdatePanel的ID</param>
        /// <param name="key">唯一Key的名稱，可以為空白由系統自動產生</param>
        /// <returns></returns>
        /// <remarks>  JsUtility.CloseWaitScreenAjax(UpdatePanel的ID);</remarks>
        public static void CloseWaitScreenAjax(System.Web.UI.UpdatePanel upPanel, string key)
        {
            string script = "";
            if (key == "")
            {
                Random autoRand = new Random();
                key = "clientScriptMessage" + Environment.TickCount.ToString() + autoRand.Next() + Environment.TickCount.ToString();
            }
            script += "closeWait();";
            ScriptManager.RegisterStartupScript(upPanel, upPanel.GetType(), key, script, true);
        }
        /// <summary>
        /// 註冊 Ajax Client 端執行 JavaScript
        /// </summary>
        /// <param name="scriptString">Javascript內容</param>
        /// <param name="upPanel">UpdatePanel的ID</param>
        /// <param name="key">唯一Key的名稱，可以為空白由系統自動產生</param>
        /// <returns></returns>
        /// <remarks>  MyUtlity.SysUtlity doAjax = new MyUtlity.SysUtlity();</remarks>
        /// <remarks>  doAjax.DoJavascriptAjax("Message", UpdatePanel的ID, "");</remarks>
        public static void DoJavascriptAjax(string scriptString, System.Web.UI.UpdatePanel upPanel, string key)
        {
            string script = "";

            scriptString = scriptString.Replace("'", "");
            if (key == "")
            {
                Random autoRand = new Random();
                key = "clientScriptMessage" + System.Environment.TickCount.ToString() + autoRand.Next()
                    + System.Environment.TickCount.ToString();
            }
            script += scriptString;
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(upPanel, upPanel.GetType(), key, script, true);
        }
        /// <summary>
        /// 註冊  Client 端執行 JavaScript
        /// </summary>
        /// <param name="scriptString">Javascript內容</param>
        /// <param name="page">this.page</param>
        /// <param name="key">唯一Key的名稱，可以為空白由系統自動產生</param>
        /// <returns></returns>
        public static void DoJavascript(string scriptString, Page page, string key)
        {
            string script = "";

            scriptString = scriptString.Replace("'", "");
            if (key == "")
            {
                Random autoRand = new Random();
                key = "clientScriptMessage" + System.Environment.TickCount.ToString() + autoRand.Next()
                    + System.Environment.TickCount.ToString();
            }
            script += "<script language=JavaScript>";
            script += scriptString;
            script += "</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), key, script);

        }
    }

    public class ComputerAuth
    {
        /// <summary>
        /// 中央處理器ID
        /// </summary>
        public string CpuID;
        /// <summary>
        /// 網卡位址
        /// </summary>
        public string MacAddress;
        /// <summary>
        /// 硬碟ID
        /// </summary>
        public string DiskID;
        /// <summary>
        /// IP位址
        /// </summary>
        public string IpAddress;
        /// <summary>
        /// OS登入帳號
        /// </summary>
        public string LoginUserName;
        /// <summary>
        /// 電腦名稱
        /// </summary>
        public string ComputerName;
        /// <summary>
        /// 電腦系統類型
        /// </summary>
        public string SystemType;
        /// <summary>
        /// 實體記憶體大小(單位：G)
        /// </summary>
        public string TotalPhysicalMemory;

        private static ComputerAuth _instance;

        public static ComputerAuth Instance()
        {

            if (_instance == null)
                _instance = new ComputerAuth();
            return _instance;
        }

        public ComputerAuth()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();
            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
        }

        public bool CheckComputerAuth()
        {
            bool blComputerAuth = true;
            string[,] AuthList = new string[10, 2];
            AuthList[0, 0] = CpuID;
            AuthList[0, 1] = "1)252021212125251(1(1(1(1(1.231/";//2014/9/29換機器
            AuthList[1, 0] = DiskID;
            AuthList[1, 1] = "1122251)1.201,24";
            AuthList[2, 0] = MacAddress;
            AuthList[2, 1] = "1(1(301(22301*11301010301+25301-1/";
            AuthList[3, 0] = IpAddress;
            AuthList[3, 1] = "1)1(3$1)1(3$1)1(3$1)1*1-";
            AuthList[4, 0] = ComputerName;
            AuthList[4, 1] = "2?202=2C2422273#2<2B2B2@2;";
            AuthList[5, 0] = SystemType;
            AuthList[5, 1] = "2G101.3#2Q2P2b2T2S3t2?22";
            AuthList[6, 0] = TotalPhysicalMemory;
            AuthList[6, 1] = "1*3$1(1(";
            AuthList[7, 0] = "LastVersion20140430";//最新版本資訊
            AuthList[7, 1] = ConfigurationManager.AppSettings["VersionDetail2"].ToString();

            DotNetBinaryTran bt = new DotNetBinaryTran();
            string EnCode = "";
            if (string.IsNullOrEmpty(LoginUserName))
            {
                //指定檢驗設備
                EnCode = bt.rtnCode(ConfigurationManager.AppSettings["VersionDetail3"].ToString());
                switch (EnCode)
                {
                    case "CpuID":
                        EnCode = bt.rtnHash(AuthList[0, 0]);
                        if (!AuthList[0, 1].Equals(EnCode))
                            blComputerAuth = false;
                        break;
                    case "DiskID":
                        EnCode = bt.rtnHash(AuthList[1, 0]);
                        if (!AuthList[1, 1].Equals(EnCode))
                            blComputerAuth = false;
                        break;
                    case "MacAddress":
                        EnCode = bt.rtnHash(AuthList[2, 0]);
                        if (!AuthList[2, 1].Equals(EnCode))
                            blComputerAuth = false;
                        break;
                    case "IpAddress":
                        EnCode = bt.rtnHash(AuthList[3, 0]);
                        if (!AuthList[3, 1].Equals(EnCode))
                            blComputerAuth = false;
                        break;
                    case "ComputerName":
                        EnCode = bt.rtnHash(AuthList[4, 0]);
                        if (!AuthList[4, 1].Equals(EnCode))
                            blComputerAuth = false;
                        break;
                    default:
                        for (int i = 0; i < 8; i++)
                        {//檢查機器是否有換
                            EnCode = bt.rtnHash(AuthList[i, 0]);
                            if (!AuthList[i, 1].Equals(EnCode))
                            {
				//2016-08-08與Sharon確認，最近ServerHost有問題，所以先將其Mark
                                //blComputerAuth = false;
                                //WriteToLogs(String LogFile, String Msg)
                            }
                        }
                        break;
                }
            }
            else
            {
                EnCode = ConfigurationManager.AppSettings["VersionDetail1"].ToString();

                if (!LoginUserName.StartsWith(bt.rtnCode(EnCode)))
                    blComputerAuth = false;
            }

            return blComputerAuth;
        }

        /// <summary>
        /// CPU ID
        /// </summary>
        string GetCpuID()
        {
            try
            {
                //獲取CPU序列號代碼
                string cpuInfo = "";//cpu序列號
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["CPUID"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["CPUID"] = cpuInfo;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return cpuInfo;
            }
            catch(Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }

        /// <summary>
        /// 網卡地址
        /// </summary>        
        string GetMacAddress()
        {
            try
            {
                //獲取網卡地址
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["MacAddress"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["MacAddress"] = mac;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return mac;
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }

        /// <summary>
        /// IP位址
        /// </summary>
        string GetIPAddress()
        {
            try
            {
                //獲取IP位址
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString();
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["IpAddress"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["IpAddress"] = st;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return st;
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }
        
        /// <summary>
        /// 硬碟ID
        /// </summary>
        string GetDiskID()
        {
            String HDid = "";
            try
            {
                //獲取硬碟ID
                ManagementClass mc = new ManagementClass("win32_logicaldisk");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject m in moc)
                {
                    if (m["DeviceID"].ToString() == "C:")
                    {
                        HDid = m["VolumeSerialNumber"].ToString();
                        break;
                    }
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["DiskID"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["DiskID"] = HDid;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return HDid;
            }
            catch (Exception ex)
            {
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        try
                        {
                            System.Array ar;
                            ar = (System.Array)(mo.Properties["Model"].Value);
                            HDid = ar.GetValue(0).ToString();
                        }
                        catch
                        {
                            HDid = (string)mo.Properties["Model"].Value;
                        }
                    }

                    moc = null;
                    mc = null;

                    if (System.Web.HttpContext.Current.Application["DiskID"] == null)
                    {
                        System.Web.HttpContext.Current.Application.Lock();
                        System.Web.HttpContext.Current.Application["DiskID"] = HDid;
                        System.Web.HttpContext.Current.Application.UnLock();
                    }

                    return HDid;
                }
                catch (Exception ex1)
                {
                    return ex1.Message + "<br/>" + ex1.ToString();
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 操作系统的登錄用戶名
        /// </summary>
        string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["UserName"] != null)
                        st = mo["UserName"].ToString();
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["LoginUserName"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["LoginUserName"] = st;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return st;
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }
        
        /// <summary>
        /// PC類型
        /// </summary>
        string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["SystemType"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["SystemType"] = st;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return st;
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }

        /// <summary>
        /// 物理記憶體
        /// </summary>
        string GetTotalPhysicalMemory()
        {
            try
            {
                string st = "";
                float theM = 0;
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");                
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["TotalPhysicalMemory"] != null)
                    {
                        st = mo["TotalPhysicalMemory"].ToString();                        
                        try
                        {
                            theM += float.Parse(st);
                        }
                        catch { }
                    }
                }

                st = (theM / Math.Pow(1024, 3)).ToString("N2");

                moc = null;
                mc = null;

                if (System.Web.HttpContext.Current.Application["TotalPhysicalMemory"] == null)
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["TotalPhysicalMemory"] = st;
                    System.Web.HttpContext.Current.Application.UnLock();
                }

                return st;
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }

        /// <summary>
        /// 電腦名稱
        /// </summary>
        string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch (Exception ex)
            {
                return ex.Message + "<br/>" + ex.ToString();
            }
            finally
            {
            }
        }
    }
}