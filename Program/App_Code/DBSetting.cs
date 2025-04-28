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
/// DBSetting 的摘要描述
/// </summary>
public static class DBSetting
{
    /// <summary>
    /// 取得[代碼檔]下拉單之代碼與名稱
    /// </summary>
    /// <param name="CodeID"></param>
    /// <returns></returns>
    public static DataTable CodeList(string CodeID)
    {
        return CodeList(DBManger.ConnectionString.EBOSDB, CodeID);
    }

    /// <summary>
    /// 取得[代碼檔]下拉單之代碼與名稱
    /// </summary>
    /// <param name="CodeID"></param>
    /// <returns></returns>
    public static DataTable CodeList(DBManger.ConnectionString Connect, string CodeID)
	{
        DBManger _MyDBM;
        _MyDBM = new DBManger();        
        _MyDBM.New(Connect);

        string strSQL = "";
        //加入 [.Replace(";", "")] 去除可能之攻擊
        CodeID = CodeID.Replace(";", "").Trim();
        strSQL = "SELECT CodeCode,CodeName,LTrim(CodeCode+'-'+CodeName) as Show FROM dbo.CodeDesc WHERE CodeID='" + CodeID + "' Order By CodeCode";
        return _MyDBM.ExecuteDataTable(strSQL);
	}

    public static DataTable DTList(string TableName, string CodeField, string showField, string strQueryFor)
    {
        return DTList(TableName, CodeField, showField, strQueryFor, "");
    }

    public static DataTable DTList(DBManger.ConnectionString Connect, string TableName, string CodeField, string showField, string strQueryFor)
    {
        return DTList(Connect, TableName, CodeField, showField, strQueryFor, "", "");
    }

    /// <summary>
    /// 取得指定資料下拉單之代碼與名稱
    /// </summary>
    /// <param name="TableName">資料表</param>
    /// <param name="CodeField">代碼欄位</param>
    /// <param name="showField">顯示欄位</param>
    /// <param name="strQueryFor">查詢條件</param>
    /// <returns></returns>
    public static DataTable DTList(string TableName, string CodeField, string showField, string strQueryFor, string strGroupBy)
    {
        return DTList(DBManger.ConnectionString.EBOSDB, TableName, CodeField, showField, strQueryFor, "", strGroupBy);
    }

    /// <summary>
    /// 取得指定資料下拉單之代碼與名稱
    /// </summary>
    /// <param name="TableName">資料表</param>
    /// <param name="CodeField">代碼欄位</param>
    /// <param name="showField">顯示欄位</param>
    /// <param name="strQueryFor">查詢條件</param>
    /// <param name="strOrderFor">排序條件</param>    
    /// <param name="strGroupBy">群組條件</param>
    /// <returns></returns>
    public static DataTable DTList(DBManger.ConnectionString Connect, string TableName, string CodeField, string showField, string strQueryFor, string strOrderFor, string strGroupBy)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New(Connect);

        string strSQL = "";        
        //加入 [.Replace(";", "")] 去除可能之攻擊
        CodeField = CodeField.Replace(";", "").Trim();
        showField = showField.Replace(";", "").Trim();
        TableName = TableName.Replace(";", "").Trim();
        strGroupBy = strGroupBy.Replace(";", "").Trim();
        strQueryFor = strQueryFor.Replace(";", "").Trim();

        //表格名稱前加 [dbo.] 避免多表格Join及相關之攻擊
        strSQL = "SELECT (" + CodeField + ") as CodeCode, (" + showField + ") as CodeName " +
            " ,LTrim(convert(varchar," + CodeField + ")+'-'+convert(varchar," + showField + ")) as Show " +
            " FROM dbo." + TableName +
            " Where (" + ((strQueryFor.Length == 0) ? "1=1" : strQueryFor) + ") ";
        if (strGroupBy.Length > 0)
            strSQL += " Group By " + strGroupBy + " ";

        if (strOrderFor.Length > 0)
            strSQL += " Order By " + strOrderFor + " ";
        else
            strSQL += " Order by " + CodeField + " ";

        return _MyDBM.ExecuteDataTable(strSQL);
    }

    /// <summary>
    /// 員工個人資料
    /// </summary>
    public struct PersonalData
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
        /// 部門單一ID
        /// </summary>
        public string DeptUnid;
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
    /// 取得員工表格
    /// </summary>
    /// <param name="Company">公司代碼</param>
    /// <param name="personalID">員工代碼</param>
    /// <returns></returns>
    public static DataTable Personal(string Company, string personalID)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        return _MyDBM.ExecuteDataTable("SELECT * FROM Personnel_Master WHERE Company='" + Company.Trim() + "' And EmployeeId='" + personalID.Trim() + "' ");
    }

    /// <summary>
    /// 找出員工個人資料,並傳回 PersonalData 
    /// </summary>
    /// <param name="Company">公司代碼</param>
    /// <param name="personalID">員工代碼</param>
    /// <returns>PersonalData</returns>
    public static PersonalData PersonalDateList(string Company, string personalID)
    {
        DataTable DT = Personal(Company, personalID);
        PersonalData PD = new PersonalData();
        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                try
                {
                    PD.Company = DT.Rows[0]["Company"].ToString().Trim();
                    PD.CompanyName = CompanyName(PD.Company);
                }
                catch { }

                try
                {
                    PD.DeptId = DT.Rows[0]["DeptId"].ToString().Trim();                    
                    PD.DeptName = DepartmentName(PD.Company, PD.DeptId);
                }
                catch { }

                try
                {
                    PD.DeptUnid = DT.Rows[0]["ID"].ToString().Trim();               
                }
                catch { }

                try
                {
                    PD.EmployeeName = DT.Rows[0]["EmployeeName"].ToString().Trim();
                }
                catch { }
                try
                {
                    PD.EnglishName = DT.Rows[0]["EnglishName"].ToString().Trim();
                }
                catch { }


                try
                {
                    PD.IDNo = DT.Rows[0]["IDNo"].ToString().Trim();
                }
                catch { }
                try
                {
                    PD.IDType = DT.Rows[0]["IDType"].ToString().Trim();
                }
                catch { }
                try
                {
                    PD.Identify = DT.Rows[0]["Identify"].ToString().Trim();
                }
                catch { }
                try
                {
                    PD.Sex = DT.Rows[0]["Sex"].ToString().Trim();
                }
                catch { }

                try
                {
                    PD.Email = DT.Rows[0]["Email"].ToString().Trim();
                }
                catch { }
                try
                {
                    PD.MobilPhone = DT.Rows[0]["MobilPhone"].ToString().Trim();
                }
                catch { }
            }
        }
        DT.Dispose();
        return PD;
    }

    /// <summary>
    /// 取得中文姓名
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalName(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.EmployeeName;
    }

    /// <summary>
    /// 取得英文姓名
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalEName(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.EnglishName;
    }

    /// <summary>
    /// 取得Email
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalEMail(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.Email;
    }

    /// <summary>
    /// 取得ID
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalDeptId(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.DeptId;
    }

    /// <summary>
    /// 取得部門識別用單一ID
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalDeptUnId(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.DeptUnid;
    }


    /// <summary>
    /// 取得部門名稱
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="personalID"></param>
    /// <returns></returns>
    public static String PersonalDeptName(string Company, string personalID)
    {
        PersonalData PD = PersonalDateList(Company, personalID);
        return PD.DeptName;
    }

    public static DataTable CompanyData(string Company)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        return _MyDBM.ExecuteDataTable("SELECT * FROM Company WHERE Company='" + Company.Trim() + "' ");
    }

    public static DataTable DepartmentData(string Company, string DepCode)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        return _MyDBM.ExecuteDataTable("SELECT * FROM Department WHERE Company='" + Company.Trim() + "' And DepCode='" + DepCode.Trim() + "' ");
    }
    
    /// <summary>
    /// 取得公司名稱
    /// </summary>
    /// <param name="Company"></param>
    /// <returns></returns>
    public static String CompanyName(string Company)
    {
        string CompanyName = "";
        DataTable DT = CompanyData(Company);
        
        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                CompanyName = DT.Rows[0]["CompanyName"].ToString().Trim();                
            }
        }
        DT.Dispose();
        return CompanyName;
    }

    /// <summary>
    /// 取得公司簡稱
    /// </summary>
    /// <param name="Company"></param>
    /// <returns></returns>
    public static String CompanyShortName(string Company)
    {
        string CompanyName = "";
        DataTable DT = CompanyData(Company);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                CompanyName = DT.Rows[0]["CompanyShortName"].ToString().Trim();
            }
        }
        DT.Dispose();
        return CompanyName;
    }

    /// <summary>
    /// 取得部門名稱
    /// </summary>
    /// <param name="DepartmentID"></param>
    /// <returns></returns>
    public static String DepartmentName(string Company, string DepCode)
    {
        string DepartmentName = "";
        DataTable DT = DepartmentData(Company, DepCode);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                DepartmentName = DT.Rows[0]["DepName"].ToString().Trim();
            }
        }
        DT.Dispose();
        return DepartmentName;
    }

    public static String PersonalDepartmentName(string Company, string personalID)
    {
        string DepartmentName = "";
        PersonalData PD = PersonalDateList(Company, personalID);
        DepartmentName = PD.DeptId + "-" + PD.DeptName;
        return DepartmentName;
    }

    /// <summary>
    /// 找出資料表描述或網頁程式名稱
    /// </summary>
    /// <param name="TableName">TableName或網頁路徑</param>
    /// <returns></returns>
    public static String TableName(string TableName)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string theTableName = "";

        DataTable DT = _MyDBM.ExecuteDataTable("Select dbo.GetTableName('" + TableName + "') As Name");
        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                theTableName = DT.Rows[0]["Name"].ToString().Trim();
            }
        }
        DT.Dispose();
        return theTableName;
    }

    /// <summary>
    /// 找出系統使用者帳號的名稱與相關資訊
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="ERPID"></param>
    /// <returns></returns>
    public static DataTable ERPIDData(string Company, string ERPID)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string SQL = "SELECT IsNull([EmployeeName],[UserName]) As [Name],[EmployeeName],[UserName] " +
            " FROM [PersonnelSecurity] PS " +
            " Left Join [Personnel_Master] PM On PS.[Company]=PM.[Company] And PS.[EmployeeId]=PM.[EmployeeId] " +
            " RIGHT Join [UC_User] U On PS.[CompanyCode]=U.[CompanyCode] And PS.[ERPID]=U.[UserId] " +        
            " WHERE U.[UserId]='" + ERPID.Trim() + "'";
        if (Company.Trim().Length > 0)
            SQL += " And U.[CompanyCode]='" + Company.Trim() + "' ";

        return _MyDBM.ExecuteDataTable(SQL);
    }

    /// <summary>
    /// 取得相應系統使用者帳號的名稱(若存在人事資料則顯示[人事資料中的姓名],否則顯示[系統使用者帳號名稱])
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="ERPID"></param>
    /// <returns></returns>
    public static String ERPIDName(string Company, string ERPID)
    {
        string ERPIDName = "";
        DataTable DT = ERPIDData(Company, ERPID);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                ERPIDName = DT.Rows[0]["Name"].ToString().Trim();
            }
        }
        DT.Dispose();
        return ERPIDName;
    }

    /// <summary>
    /// 取得指定公司之行事曆(可限制/不限：日期(yyyy/MM/dd)／部門／員工／分類／狀態)
    /// </summary>
    /// <param name="Show">顯示設定(TW/TW-S/EN/EN-S:星期一/一/Monday/Mon)</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期(yyyy/MM/dd)</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>    
    /// <returns>行事曆</returns>
    public static DataTable CalendarData(string Show, string Company, string theDate, string SeqNo, string DepId, string EmployeeId, string Category, string Status)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
              
        string SQL = "dbo.sp_view_Calendar";

        SqlCommand MyCmd = new SqlCommand("");
        MyCmd.Parameters.Add("@ls_Show", SqlDbType.Char, 4).Value = Show.Trim().ToUpper();
        MyCmd.Parameters.Add("@ls_Company", SqlDbType.Char, 2).Value = Company.Trim();
        MyCmd.Parameters.Add("@ls_theDate", SqlDbType.Char, 10).Value = theDate.Trim();
        MyCmd.Parameters.Add("@ls_SeqNo", SqlDbType.Char, 10).Value = SeqNo.Trim();        
        MyCmd.Parameters.Add("@ls_DeptId", SqlDbType.Char, 5).Value = DepId.Trim();
        MyCmd.Parameters.Add("@ls_EmployeeId", SqlDbType.Char, 10).Value = EmployeeId.Trim();
        MyCmd.Parameters.Add("@ls_Category", SqlDbType.Char, 2).Value = Category.Trim();
        MyCmd.Parameters.Add("@ls_Status", SqlDbType.Char, 1).Value = Status.Trim();

        return _MyDBM.ExecStoredProcedure(SQL, MyCmd.Parameters);
    }

    /// <summary>
    /// 取得人事薪資參數
    /// </summary>
    /// <param name="Category">類別</param>
    /// <param name="Company">公司代碼</param>
    /// <returns></returns>
    public static DataTable PersonnelSalaryParameter(string Category, string Company)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string SQL = "SELECT * " +
            " FROM [PersonnelSalary_Parameter] Where [Category]='" + Category + "' And [Company]='" + Company + "'" +
            " Order By [Company]";

        return _MyDBM.ExecuteDataTable(SQL);
    }

    /// <summary>
    /// 取得人事薪資參數(預設類別:01)
    /// </summary>
    /// <param name="Company">公司代碼</param>
    /// <returns></returns>
    public static DataTable PersonnelSalaryParameter(string Company)
    {
        return PersonnelSalaryParameter("01", Company);
    }

    /// <summary>
    /// 取得人事薪資參數值
    /// </summary>
    /// <param name="Company">公司代碼</param>
    /// <param name="ParaName">參數名稱</param>
    /// <returns></returns>
    public static String GetPSParaValue(string Company, string ParaName)
    {
        string ParaValue = "";
        DataTable DT = PersonnelSalaryParameter(Company);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][ParaName] != null)
                    ParaValue = DT.Rows[0][ParaName].ToString().Trim();
            }
        }
        DT.Dispose();
        return ParaValue;
    }

    public static DataTable StarData(string showDate)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string Query = "";
        if (!string.IsNullOrEmpty(showDate))
        {
            Query = " And (" + showDate + " Between [DateStart] And [DateEnd] " +
                " Or [DateStart]= (SELECT (Case When (" + showDate + "  >= Max([DateStart]) OR " + showDate + " <= Min([DateEnd])) Then Max([DateStart]) else " + showDate + "  end) From [Constellation])) ";            
        }
        Query = "SELECT * From [Constellation] Where 1=1 " + Query;
        return _MyDBM.ExecuteDataTable(Query);
    }

    /// <summary>
    /// 取得星座各項說明
    /// </summary>
    /// <param name="showDate">日期(MMdd)</param>
    /// <param name="ParaName">參數名稱</param>
    /// <returns></returns>
    public static String GetStarData(string showDate, string ParaName)
    {
        string ParaValue = "";
        DataTable DT = StarData(showDate);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][ParaName] != null)
                    ParaValue = DT.Rows[0][ParaName].ToString().Trim();
            }
        }
        DT.Dispose();
        return ParaValue;
    }

    /// <summary>
    /// 取得各系統設定之新編號
    /// </summary>
    /// <param name="SysNo">號碼別=>[SysNo](2)+[CodeHead](2)=Len(4):ARCO-應收帳款收款單號,ARAV-應收帳款預收單號 OR ,INVNO-電子發票號碼,IN-人工發票號碼</param>
    /// <param name="CompanyId">公司代號</param>
    /// <param name="CustomerNo">客戶代碼</param>
    /// <returns>各系統新單號</returns>
    public static String GetEIPSeqNo(string Kind, string CompanyId, string CustomerNo)
    {
         DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string Query = "";
        Query = "SELECT dbo.[fn_EIP_GetSeqNo]('" + Kind.Trim() + "','" + CompanyId.Trim() + "','" + CustomerNo.Trim() + "') ";
        
        string ParaValue = "";
        DataTable DT = _MyDBM.ExecuteDataTable(Query);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0] != null)
                    ParaValue = DT.Rows[0][0].ToString().Trim();
            }
        }
        DT.Dispose();
        return ParaValue;
    }

    /// <summary>
    /// 取得各系統設定之新編號
    /// </summary>
    /// <param name="SysNo">號碼別=>[SysNo](2)+[CodeHead](2)=Len(4):ARCO-應收帳款收款單號,ARAV-應收帳款預收單號 OR ,INVNO-電子發票號碼,IN-人工發票號碼</param>
    /// <param name="CompanyId">公司代號</param>
    /// <param name="CustomerNo">客戶代碼</param>
     /// <param name="SeqNo">指定單號</param>
    /// <returns>各系統新單號</returns>
    public static String SetEIPSeqNo(string Kind, string CompanyId, string CustomerNo, string SeqNo)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string Query = "sp_EIP_GetSeqNo";
        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        MyCmd.Parameters.Add("@ls_Kind", SqlDbType.Char, 1).Value = Kind.Trim();
        MyCmd.Parameters.Add("@ls_CompanyId", SqlDbType.Char, 2).Value = CompanyId.Trim();
        MyCmd.Parameters.Add("@ls_CustomerNo", SqlDbType.VarChar, 10).Value = CustomerNo.Trim();
        MyCmd.Parameters.Add("@ls_SeqNo", SqlDbType.VarChar, 10).Value = SeqNo.Trim();
        string ParaValue = "";
        DataTable DT = _MyDBM.ExecStoredProcedure(Query, MyCmd.Parameters);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0] != null)
                    ParaValue = DT.Rows[0][0].ToString().Trim();
            }
        }
        DT.Dispose();
        return ParaValue;
    }

    /// <summary>
    /// 取得指定會計科目欄位之顯示值;若回傳值為null,則表示輸入值不在可用範圍中
    /// </summary>
    /// <param name="theAcctsCheck">會計科目欄位檢核字串</param>
    /// <param name="theInput">輸入值</param>
    /// <returns>會計科目欄位之顯示值</returns>
    public static String GetAcctShow(string theAcctsCheck, string theInput)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string Query = "";
        string[] theCeckList = theAcctsCheck.Split('|');
        //員工代號|CodeDesc|EMPLOYEE|CodeID|CodeCode|CodeName
        //部門別|Department|       |DepCode|DepCode|DepName        
        Query = "Select " + theCeckList[5] + " From " + theCeckList[1] + " Where " + theCeckList[4] + "='" + theInput.Trim() + "' ";

        if (theCeckList[2] != "")
            Query += " And " + theCeckList[3] + "='" + theCeckList[2] + "' ";

        string ParaValue = null;
        DataTable DT = _MyDBM.ExecuteDataTable(Query);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0] != null)
                    ParaValue = DT.Rows[0][0].ToString().Trim();
                else
                    ParaValue = "";
            }
        }
        DT.Dispose();
        return ParaValue;
    }

    /// <summary>
    /// 傳回代碼檔中指定代碼之代表值/描述
    /// </summary>
    /// <param name="CodeID">代碼識別</param>
    /// <param name="CodeCode">項目編碼</param>
    /// <returns>代碼描述/代表值</returns>
    public static String CodeDesc(string CodeID, string CodeCode)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string Query = "";
        //加入 [.Replace(";", "")] 去除可能之攻擊
        CodeID = CodeID.Replace(";", "").Trim();
        CodeCode = CodeCode.Replace(";", "").Trim();
        Query = "SELECT CodeName FROM dbo.CodeDesc WHERE CodeID='" + CodeID + "' And CodeCode='" + CodeCode + "'";
        string ParaValue = null;
        DataTable DT = _MyDBM.ExecuteDataTable(Query);

        if (DT != null)
        {
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0] != null)
                    ParaValue = DT.Rows[0][0].ToString().Trim();
                else
                    ParaValue = "";
            }
        }
        DT.Dispose();
        return ParaValue;        
    }
}
