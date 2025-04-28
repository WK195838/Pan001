using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;






/// <summary>
/// ImportData 的摘要描述
/// </summary>
public class ExcelManger
{
    SysSetting _SysSet = new SysSetting();

    DBManger _MyDBM;
   

    public ExcelManger()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }



    public enum Status : int
    {
        /// <summary>
        /// 不明錯誤
        /// </summary>
        UnknownError = -1,

        /// <summary>
        /// 找不到檔案
        /// </summary>
        FileNotFound = -2,

        /// <summary>
        /// 資料庫發生錯誤
        /// </summary>
        DB_Error = -3,

        /// <summary>
        /// 處理試算狀態，不處理
        /// </summary>
        Draft = -4,

        /// <summary>
        /// 正確
        /// </summary>
        OK = 1,

        /// <summary>
        /// 更新資料
        /// </summary>
        Update = 2,
    }


    /// <summary>
    /// 匯入卡鐘資料至DB
    /// </summary>
    /// <param name="ExcelFile">Excel 路徑</param>
    /// <param name="Sheet">工作表名稱</param>
    /// <param name="RowStart">讀取列起點 (第一列為1)</param>
    /// 
    /// <param name="ColumnsStart">讀取欄位起點 (A欄為0)</param>
    /// <param name="ColumnsEnd">讀取欄位終點</param>
    /// <param name="DB_Table"></param>
    public string[] ImportPanTimeClock(string ExcelFile, string Sheet, int RowStart, int ColumnsStart, int ColumnsEnd, string DB_Table, string CompanyID)
    {
        string logFile;
        string mStatus;
        string AE;
        //檢查檔案是否存在
        if (System.IO.File.Exists(ExcelFile) == false)
        {
            logFile = _SysSet.WriteToLogs("卡鐘", "匯入檔案找不到" + ExcelFile);
            return new string[] { "匯入檔案找不到" + ExcelFile, logFile };
        }

        _MyDBM = new DBManger();
        _MyDBM.New();

        logFile = _SysSet.WriteToLogs("卡鐘", "開始新增" + "File:" + ExcelFile);

      
        string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + ExcelFile + ";Extended Properties='Excel 8.0;HDR=YES'";
        OleDbConnection objConn = new OleDbConnection(strCon);

        string strCom = "SELECT * FROM [" + Sheet + "$] ";
        objConn.Open();

        OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
        DataSet objDS = new DataSet();

        try
        {
            objCmd.Fill(objDS);
            objConn.Close();            
            objConn.Dispose();
            
            int mOKCount = 0, mDraftCount = 0, mUpdate = 0, mError = 0;

            for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
            {
                try
                {
                    System.Windows.Forms.Application.DoEvents();

                    string[] a = new string[ColumnsEnd - ColumnsStart + 1];


                    for (int g = 0; g < a.Length; g++)
                        a[g] = objDS.Tables[0].Rows[i][ColumnsStart + g].ToString().Replace("'", "''").Trim();


                    _MyDBM = new DBManger();
                    _MyDBM.New();

                    string InTime = "";
                    string OutTime = "";

                    if (string.IsNullOrEmpty(a[6]) == false)
                    {
                        try
                        {
                            InTime = a[6].Trim().Replace(":", "");
                            if (InTime.Length > 4)
                                InTime = InTime.Substring(0, 4);
                            else
                                InTime = InTime.PadLeft(4, '0');
                        }
                        catch { }
                    }
                    if (string.IsNullOrEmpty(a[7]) == false)
                    {
                        try
                        {
                            OutTime = a[7].Trim().Replace(":", "");
                            if (OutTime.Length > 4)
                                OutTime = OutTime.Substring(0, 4);
                            else
                                OutTime = OutTime.PadLeft(4, '0');
                        }
                        catch { }
                    }

                    string DepCode = "";
                    DataTable DT = null;
                    //DataTable DT = _MyDBM.ExecuteDataTable("SELECT Top 1 DepCode FROM Department Where Company='" + CompanyID + "' and DepName='" + a[3] + "'");

                    //if (DT.Rows.Count > 0 && string.IsNullOrEmpty(DT.Rows[0]["DepCode"].ToString()) == false)
                    //    DepCode = DT.Rows[0]["DepCode"].ToString();

                    string mEmployeeId = a[2];
                    string mAttendanceDate = a[4].Substring(0, 10).Replace("-", "/");
                    string mCardNo = a[1];
                    string mMemo = a[5];
                    string mPayroll_Processingmonth = a[8];
                    int testPayroll_Processingmonth = 0;
                    //未設定計薪年月時,預設為差勤日期之年月
                    if (int.TryParse(mPayroll_Processingmonth, out testPayroll_Processingmonth) == false || testPayroll_Processingmonth < 199000)
                        mPayroll_Processingmonth = mAttendanceDate.Replace("/", "").Substring(0, 6);

                    string Ssql = "";

                    //查看是否有記錄
                    Ssql = "Select * from AttendanceSheet Where company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And AttendanceDate='" + mAttendanceDate + "'";
                    DT = _MyDBM.ExecuteDataTable(Ssql);

                    if (DT.Rows.Count > 0) //覆蓋資料
                        Ssql = "UPDATE AttendanceSheet SET DeptId=IsNull((SELECT Top 1 DepCode FROM Department Where Company='" + CompanyID + "' and DepName='" + a[3] +
                            "'),'') ,CardNo='" + mCardNo + "' ,InTime='" + InTime +"' ,OutTime='" + OutTime + "' ,Memo='" + mMemo + "' ,Payroll_Processingmonth='" + mPayroll_Processingmonth +
                            "' WHERE Company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And Convert(varchar,AttendanceDate,111)='" + mAttendanceDate + "'";
                    else
                    {
                        Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
                        sYM = Payroll.CheckSalaryYM(CompanyID, mAttendanceDate.Replace(@"/", ""), "0");

                        //完成確認試算
                        if (string.IsNullOrEmpty(sYM.ConfirmDate) == false)
                            mDraftCount++;
                        else
                            Ssql = "Insert Into AttendanceSheet (Company,EmployeeId,AttendanceDate,DeptId,CardNo,InTime,OutTime,Memo,Payroll_Processingmonth) Values('" +
                                CompanyID + "','" + mEmployeeId + "','" + mAttendanceDate + "','" + DepCode + "','" + mCardNo +
                                "','" + InTime + "','" + OutTime + "','" + mMemo + "','" + mPayroll_Processingmonth + "')";
                    }

                    //無法新增至資料庫
                    if (string.IsNullOrEmpty(Ssql) == false && _MyDBM.ExecuteCommand(Ssql) != 1)
                    {
                        logFile = _SysSet.WriteToLogs("卡鐘", "無法新增至資料庫" + "File:" + ExcelFile + "\n\r Sql:" + Ssql + "\n\r");
                        mError++;
                    }
                    else
                    {
                        if (Ssql.Substring(0, 2).ToUpper() == "IN")
                            mOKCount++;
                        else
                        {
                            mUpdate++;

                            //Ssql = "DELETE FROM AttendanceException Where company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And AttendanceDate='" + mAttendanceDate + "'";
                            //_MyDBM.ExecuteCommand(Ssql);

                            logFile = _SysSet.WriteToLogs("卡鐘", "更新資料" + "File:" + ExcelFile + "\n\r 欄:" + i.ToString() + "\n\r Sql:" + Ssql + "\n\r");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logFile = _SysSet.WriteToLogs("卡鐘", "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r 欄:" + i.ToString() + "\n\r ExcelFile:" + ExcelFile + "\n\r");
                    mError++;
                }
            }
            AE = "true";

            mStatus = "新增: " + mOKCount + " 更新: " + mUpdate + " 失敗: " + (mError + mDraftCount);

            logFile = _SysSet.WriteToLogs("卡鐘", "完成" + "File:" + ExcelFile + "\n\r " + mStatus);

            return new string[] { mStatus, logFile, AE };
        }
        catch (Exception ex)
        {
            AE = "false";
            mStatus = "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\rExcelFile:" + ExcelFile + "\n\r";
            logFile = _SysSet.WriteToLogs("卡鐘", mStatus);
            return new string[] { mStatus, logFile, AE };
        }
        finally
        {
            objDS.Dispose();
        }
    }

    /// <summary>
    /// 匯入加班資料至DB
    /// </summary>
    /// <param name="ExcelFile">Excel 路徑</param>
    /// <param name="Sheet">工作表名稱</param>
    /// <param name="RowStart">讀取列起點 (第一列為1)</param>
    /// <param name="ColumnsStart">讀取欄位起點 (A欄為0)</param>
    /// <param name="ColumnsEnd">讀取欄位終點</param>
    /// <param name="DB_Table"></param>
    public string[] ImportOverTimeTrx(string ExcelFile, string Sheet, int RowStart, int ColumnsStart, int ColumnsEnd, string DB_Table, string CompanyID)
    {
        string logFile;
        string mStatus;
        //檢查檔案是否存在
        if (System.IO.File.Exists(ExcelFile) == false)
        {
            logFile = _SysSet.WriteToLogs("加班", "匯入檔案找不到" + ExcelFile);
            mStatus = "匯入檔案找不到" + ExcelFile;
            
            return new string[] { mStatus, logFile };
        }

        _MyDBM = new DBManger();
        _MyDBM.New();

        logFile = _SysSet.WriteToLogs("加班", "開始新增" + "File:" + ExcelFile);

 
      

        string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + ExcelFile + ";Extended Properties='Excel 8.0;HDR=YES'";
        OleDbConnection objConn = new OleDbConnection(strCon);

        string strCom = "SELECT * FROM [" + Sheet + "$] ";
        objConn.Open();

        OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
        DataSet objDS = new DataSet();

        try
        {
            objCmd.Fill(objDS);
            objConn.Close();

            int mCount = 0, mOKCount = 0, mDraftCount = 0, mUpdate = 0, mError = 0;

            for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
            {
                try
                {
                    System.Windows.Forms.Application.DoEvents();

                    string[] a = new string[ColumnsEnd - ColumnsStart + 1];


                    for (int g = 0; g < a.Length; g++)
                        a[g] = objDS.Tables[0].Rows[i][ColumnsStart + g].ToString().Replace("'", "''").Trim();


                    _MyDBM = new DBManger();
                    _MyDBM.New();

                    string mEmployeeId = a[1] == null ? "" : a[1];
                    string mOverTime_Date = a[2] == null ? "" : DateTime.Parse(a[2]).ToString("yyyy-MM-dd HH:mm:ss");
                    string mBeginTime = a[3] == null ? "" : a[3];
                    string mEndTime = a[4] == null ? "" : a[4];
                    string mClass_Code = a[5] == null ? "" : a[5];
                    string mOvertime_pay = a[6] == null ? "" : a[6];
                    string mOverTime1 = a[7] == null ? "" : a[7];
                    string mOverTime2 = a[8] == null ? "" : a[8];


                    string mHoliday = a[9] == null ? "" : a[9];
                    string mNationalHoliday = a[10] == null ? "" : a[10];
                    string mApproveDate = (string.IsNullOrEmpty(a[11])) ? "" : DateTime.Parse(a[11]).ToString("yyyy-MM-dd HH:mm:ss");
                    string mPayroll_Processingmonth = a[12] == null ? "" : DateTime.Parse(a[12]).ToString("yyyyMM");

                    DataTable DT;
                    string SqlWhere = " Where company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And OverTime_Date='" + mOverTime_Date + "' And BeginTime='" + mBeginTime + "'";
                    SqlWhere = SqlWhere.Replace("=''", " is NULL");
                    
                    string Ssql = "Select * from OverTime_Trx" + SqlWhere;
                    Ssql = Ssql.Replace("''", "NULL");
                

                    //查看是否有記錄


                    DT = _MyDBM.ExecuteDataTable(Ssql);

                    if (DT.Rows.Count > 0) //覆蓋資料
                    {
                        Ssql = "UPDATE OverTime_Trx SET Company='" + CompanyID + "',EmployeeId='" + mEmployeeId + "',OverTime_Date='" + mOverTime_Date + "',BeginTime='" + mBeginTime + "',EndTime='" + mEndTime + "',Class_Code='" + mClass_Code + "',Overtime_pay='" + mOvertime_pay + "',OverTime1='" + mOverTime1 + "',OverTime2='" + mOverTime2 + "',Holiday='" + mHoliday + "',NationalHoliday='" + mNationalHoliday + "',ApproveDate='" + mApproveDate + "',Payroll_Processingmonth='" + mPayroll_Processingmonth + "'" + SqlWhere;
                    }
                    else
                    {
                        Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();

                        sYM = Payroll.CheckSalaryYM(CompanyID, mPayroll_Processingmonth.Replace("/", "").Substring(0, 5), "0");

                        //完成確認試算
                        if (string.IsNullOrEmpty(sYM.ConfirmDate) == false)
                            mDraftCount++;
                        else
                            Ssql = "INSERT INTO OverTime_Trx " +
                                    "([Company],[EmployeeId],[OverTime_Date],[DeptId],[BeginTime],[EndTime],[Class_Code],[Overtime_pay],[OverTime1],[OverTime2],[Holiday],[NationalHoliday],[ApproveDate],[Payroll_ProcessingMonth]) " +
                                    "VALUES " +
                                    "('" + CompanyID + "','" + mEmployeeId + "','" + mOverTime_Date + "','" + "" + "','" + mBeginTime + "','" + mEndTime + "','" + mClass_Code + "','" + mOvertime_pay + "','" + mOverTime1 + "','" + mOverTime2 + "','" + mHoliday + "','" + mNationalHoliday + "','" + mApproveDate + "','" + mPayroll_Processingmonth + "')";
                    }

                    Ssql = Ssql.Replace("''", "NULL");
               

                    //無法新增至資料庫
                    if (string.IsNullOrEmpty(Ssql) == false && _MyDBM.ExecuteCommand(Ssql) < 0)
                    {
                        _SysSet.WriteToLogs("加班", "無法新增至資料庫" + "File:" + ExcelFile + "\n\r Sql:" + Ssql + "\n\r");
                        mError++;
                    }
                    else
                    {
                        if (Ssql.Substring(0, 2).ToUpper() == "IN")
                            mOKCount++;
                        else
                        {
                            mUpdate++;
                            _SysSet.WriteToLogs("加班", "更新資料" + "File:" + ExcelFile + "\n\r 欄:" + i.ToString() + "\n\r Sql:" + Ssql + "\n\r");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _SysSet.WriteToLogs("加班", "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r 欄:" + i.ToString() + "\n\r ExcelFile:" + ExcelFile + "\n\r");
                    mError++;
                }
            }

            mStatus = "新增: " + mOKCount + " 更新: " + mUpdate + " 失敗: " + (mError + mDraftCount);

            logFile = _SysSet.WriteToLogs("加班", "完成" + "File:" + ExcelFile + "\n\r " + mStatus);

            return new string[] { mStatus, logFile };
        }
        catch (Exception ex)
        {
           logFile = _SysSet.WriteToLogs("加班", "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\rExcelFile:" + ExcelFile + "\n\r");

            mStatus = "匯入檔案時發生錯誤";

            return new string[] { mStatus, logFile };
        }
        finally
        {
            objDS.Dispose();
            objConn.Close();
            objConn.Dispose();
        }
    }

    /// <summary>
    /// 匯入請假資料至DB
    /// </summary>
    /// <param name="ExcelFile">Excel 路徑</param>
    /// <param name="Sheet">工作表名稱</param>
    /// <param name="RowStart">讀取列起點 (第一列為1)</param>
    /// <param name="ColumnsStart">讀取欄位起點 (A欄為0)</param>
    /// <param name="ColumnsEnd">讀取欄位終點</param>
    /// <param name="DB_Table"></param>
    public string[] ImportLeaveTrx(string ExcelFile, string Sheet, int RowStart, int ColumnsStart, int ColumnsEnd, string DB_Table, string CompanyID)
    {
        string logFile;
        string mStatus;

        //檢查檔案是否存在
        if (System.IO.File.Exists(ExcelFile) == false)
        {
            logFile = _SysSet.WriteToLogs("請假", "匯入檔案找不到" + ExcelFile);
            mStatus = "匯入檔案找不到" + ExcelFile;

            return new string[] { mStatus, logFile };
        }

        _MyDBM = new DBManger();
        _MyDBM.New();


        logFile = _SysSet.WriteToLogs("請假", "開始新增" + "File:" + ExcelFile);

       

        string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + ExcelFile + ";Extended Properties='Excel 8.0;HDR=YES'";
        OleDbConnection objConn = new OleDbConnection(strCon);

        string strCom = "SELECT * FROM [" + Sheet + "$] ";
        objConn.Open();

        OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
        DataSet objDS = new DataSet();

        try
        {
            objCmd.Fill(objDS);
            objConn.Close();

            int mCount = 0, mOKCount = 0, mDraftCount = 0, mUpdate = 0, mError = 0;

            for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
            {
                try
                {
                    System.Windows.Forms.Application.DoEvents();

                    string[] a = new string[ColumnsEnd - ColumnsStart + 1];


                    for (int g = 0; g < a.Length; g++)
                        a[g] = objDS.Tables[0].Rows[i][ColumnsStart + g].ToString().Replace("'", "''").Trim();


                    _MyDBM = new DBManger();
                    _MyDBM.New();

                    string mEmployeeId = a[1];
                    string mBeginDateTime = DateTime.Parse(a[3]).ToString("yyyy-MM-dd HH:mm:ss");
                    string mEndDateTime = DateTime.Parse(a[4]).ToString("yyyy-MM-dd HH:mm:ss");

                    string mLeaveType_Id = "";

                    string mHours = a[5];
                    string mDays = a[6];
                    string mApproveDate = DateTime.Parse(a[7]).ToString("yyyy-MM-dd HH:mm:ss");
                    string mPayroll_Processingmonth = DateTime.Parse(a[7]).ToString("yyyyMM");


                    DataTable DT;

                    //轉換假別代碼
                    DT = _MyDBM.ExecuteDataTable("select Leave_Id from LeaveType_Basic where Company='" + CompanyID + "' And Leave_Desc='" + a[2] + "'");
                    if (DT.Rows.Count > 0) mLeaveType_Id = DT.Rows[0]["Leave_Id"].ToString();




                    string Ssql = "";

                    //查看是否有記錄
                    DT = _MyDBM.ExecuteDataTable("Select * from Leave_Trx Where company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And BeginDateTime='" + mBeginDateTime + "'");

                    if (DT.Rows.Count > 0) //覆蓋資料
                    {
                        Ssql = "UPDATE Leave_Trx SET LeaveType_Id='" + mLeaveType_Id + "' ,BeginDateTime='" + mBeginDateTime + "' ,EndDateTime='" + mEndDateTime + "' ,hours='" + mHours + "' ,Days='" + mDays + "' ,ApproveDate='" + mApproveDate + "' WHERE Company='" + CompanyID + "' And EmployeeId='" + mEmployeeId + "' And BeginDateTime='" + mBeginDateTime + "' And Payroll_Processingmonth='" + mPayroll_Processingmonth + "'";
                    }
                    else
                    {
                        Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
                        sYM = Payroll.CheckSalaryYM(CompanyID, mPayroll_Processingmonth.Replace("/", "").Substring(0, 5), "0");

                        //完成確認試算
                        if (string.IsNullOrEmpty(sYM.ConfirmDate) == false)
                            mDraftCount++;
                        else
                            Ssql = "Insert Into Leave_Trx (Company,EmployeeId,LeaveType_Id,BeginDateTime,EndDateTime,Hours,Days,ApproveDate,Payroll_Processingmonth) Values('" + CompanyID + "','" + mEmployeeId + "','" + mLeaveType_Id + "','" + mBeginDateTime + "','" + mEndDateTime + "','" + mHours + "','" + mDays + "','" + mApproveDate + "','" + mPayroll_Processingmonth + "')";


                    }

                    //無法新增至資料庫
                    if (string.IsNullOrEmpty(Ssql) == false && _MyDBM.ExecuteCommand(Ssql) != 1)
                    {
                        logFile = _SysSet.WriteToLogs("請假", "無法新增至資料庫" + "File:" + ExcelFile + "\n\r Sql:" + Ssql + "\n\r");
                        mError++;
                    }
                    else
                    {
                        if (Ssql.Substring(0, 2).ToUpper() == "IN")
                            mOKCount++;
                        else
                        {
                            mUpdate++;
                            logFile = _SysSet.WriteToLogs("請假", "更新資料" + "File:" + ExcelFile + "\n\r 欄:" + i.ToString() + "\n\r Sql:" + Ssql + "\n\r");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string ErrorLog = "匯入檔案時發生錯誤" + "請假 Error Message:" + ex.Message + "\n\r 欄:" + i.ToString() + "\n\r ExcelFile:" + ExcelFile + "\n\r";

                    logFile = _SysSet.WriteToLogs("請假", ErrorLog);


     
                    mError++;
                }
            }

            mStatus = "新增: " + mOKCount + " 更新: " + mUpdate + " 失敗: " + (mError + mDraftCount);

            logFile = _SysSet.WriteToLogs("請假", "完成" + "File:" + ExcelFile + "\n\r " + mStatus);

            return new string[] { mStatus, logFile };
        }
        catch (Exception ex)
        {
           
            logFile = _SysSet.WriteToLogs("請假", "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\rExcelFile:" + ExcelFile + "\n\r");
            mStatus  = "匯入檔案時發生錯誤";

            return new string[] { mStatus, logFile };
        }
        finally
        {
            objDS.Dispose();
            objConn.Close();
            objConn.Dispose();
        }
    }

    /// <summary>
    /// 匯入薪職/本薪等級資料至DB
    /// </summary>
    /// <param name="ExcelFile">Excel路徑</param>
    /// <param name="Sheet">工作表名稱</param>
    /// <param name="RowStart">讀取列起點 (第一列為1)</param>
    /// <param name="ColumnsStart">讀取欄位起點 (A欄為0)</param>
    /// <param name="ColumnsEnd">讀取欄位終點</param>
    /// <param name="DTKind">儲存欄位(薪職:BaseSalary/本薪:SalaryPoint)</param>
    public string[] ImportSalaryGradeRankData(string ExcelFile, string Sheet, int RowStart, int ColumnsStart, int ColumnsEnd, string DTKind)
    {
        string logFile;
        string mStatus;
        string strDTKind = DTKind;
        switch (DTKind)
        {
            case "BaseSalary":
                strDTKind = "本薪等級";
                break;
            case "SalaryPoint":
                strDTKind = "薪職等級";
                break;
        }

        //檢查檔案是否存在
        if (System.IO.File.Exists(ExcelFile) == false)
        {
            logFile = _SysSet.WriteToLogs(strDTKind, "匯入檔案找不到" + ExcelFile);
            return new string[] { "匯入檔案找不到" + ExcelFile, logFile };
        }

        _MyDBM = new DBManger();
        _MyDBM.New();

        logFile = _SysSet.WriteToLogs(strDTKind, "開始新增" + "File:" + ExcelFile);


        string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + ExcelFile + ";Extended Properties='Excel 8.0;HDR=YES'";
        OleDbConnection objConn = new OleDbConnection(strCon);

        string strCom = "SELECT * FROM [" + Sheet + "$] Order By 1";
        objConn.Open();

        OleDbDataAdapter objCmd = new OleDbDataAdapter(strCom, objConn);
        DataSet objDS = new DataSet();

        try
        {
            objCmd.Fill(objDS);
            objConn.Close();

            int mOKCount = 0, mDraftCount = 0, mUpdate = 0, mError = 0;

            System.Windows.Forms.Application.DoEvents();

            string[] a = new string[ColumnsEnd - ColumnsStart + 1];
            string strTempLevel = "";
            string Ssql = "";
            string tempIndex = "";
            string tempValue = "";

            //DataTable DT = _MyDBM.ExecuteDataTable("SELECT [Level] FROM [SalaryLevel_CheckStandard] Order By [Level] ");
            DataTable DT = objDS.Tables[0];
            strTempLevel = "";
            Ssql = "";

            if (DT != null)
                for (int i = 0; i < DT.Rows.Count; i++)
                {//for (int i = 0; i < objDS.Tables[0].Rows.Count; i++)
                    try
                    {
                        strTempLevel = DT.Rows[i][0].ToString().Trim();
                        for (int g = 0; g < a.Length; g++)
                        {
                            a[g] = objDS.Tables[0].Rows[i][ColumnsStart + g].ToString().Replace("'", "''").Trim();
                            tempValue = (a[g].Length > 0) ? _SysSet.rtnHash(a[g]) : a[g];
                            tempIndex = (ColumnsStart + g).ToString().PadLeft(2, '0');
                            //查看是否有記錄
                            DataTable checkDT = _MyDBM.ExecuteDataTable("Select * From SalaryGradeRankData WHERE Level = '" + strTempLevel + "' And Rank = '" + tempIndex + "'");

                            if (checkDT.Rows.Count > 0) //覆蓋資料
                                Ssql = "Update SalaryGradeRankData Set [" + DTKind + "]='" + tempValue + "' WHERE Level = '" + strTempLevel + "' And Rank = '" + tempIndex + "'";
                            else
                            {
                                Ssql = "Insert Into SalaryGradeRankData ([Level],[Rank],[" + DTKind + "]) Values ('" + strTempLevel + "','" + tempIndex + "','" + tempValue + "')";
                            }

                            //無法新增至資料庫
                            if (string.IsNullOrEmpty(Ssql) == false && _MyDBM.ExecuteCommand(Ssql) != 1)
                            {
                                logFile = _SysSet.WriteToLogs(strDTKind, "無法新增至資料庫" + "File:" + ExcelFile + "\n\r Sql:" + Ssql + "\n\r");
                                mError++;
                            }
                            else
                            {
                                if (Ssql.Substring(0, 2).ToUpper() == "IN")
                                    mOKCount++;
                                else
                                {
                                    mUpdate++;
                                    logFile = _SysSet.WriteToLogs(strDTKind, "更新資料" + "File:" + ExcelFile + "\n\r 欄:" + i.ToString() + "\n\r Sql:" + Ssql + "\n\r");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logFile = _SysSet.WriteToLogs(strDTKind, "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\r 欄:" + i.ToString() + "\n\r ExcelFile:" + ExcelFile + "\n\r");
                        mError++;
                    }
                }

            mStatus = "新增: " + mOKCount + " 更新: " + mUpdate + " 失敗: " + (mError + mDraftCount);

            logFile = _SysSet.WriteToLogs(strDTKind, "完成" + "File:" + ExcelFile + "\n\r " + mStatus);

            return new string[] { mStatus, logFile };
        }
        catch (Exception ex)
        {
            mStatus = "匯入檔案時發生錯誤" + "Error Message:" + ex.Message + "\n\rExcelFile:" + ExcelFile + "\n\r";
            logFile = _SysSet.WriteToLogs(strDTKind, mStatus);
            return new string[] { mStatus, logFile };
        }
        finally
        {
            objDS.Dispose();
            objConn.Close();
            objConn.Dispose();
        }    
    }
}
