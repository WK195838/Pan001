# Payroll.cs 程式規格書

## 1. 模組概述

### 1.1 功能說明
薪資計算核心模組（Payroll.cs）是泛太總帳系統的核心組件，負責處理所有與薪資計算相關的業務邏輯。本模組提供完整的薪資計算功能，包括：
- 薪資項目計算
- 破月薪資計算
- 加班費計算
- 請假扣薪計算
- 薪資結構管理

### 1.2 相依性
- IncomeTax.cs：所得稅計算
- LaborInsurance.cs：勞保費計算
- HealthInsurance.cs：健保費計算
- DBManger.cs：資料庫存取
- CacheManager.cs：快取管理
- ErrorLogger.cs：錯誤記錄

## 2. 類別結構

### 2.1 Payroll 類別
```csharp
public static class Payroll
{
    // 靜態建構子
    static Payroll()
    {
        // 初始化快取
        InitializeCache();
    }

    // 主要方法
    public static PayrolList CalculateSalary(string company, string employeeId, string payrollDate)
    public static SalaryItem GetSalaryItem(string company, string employeeId, string itemCode)
    public static float CalculateOvertimePay(string company, string employeeId, string payrollDate)
    public static float CalculateLeaveDeduction(string company, string employeeId, string payrollDate)
    
    // 輔助方法
    private static void InitializeCache()
    private static PayrolList GetEmployeeData(string company, string employeeId)
    private static float GetBaseSalary(string company, string employeeId)
    private static void ValidateParameters(string company, string employeeId, string payrollDate)
}
```

### 2.2 資料結構定義
```csharp
// 薪資資料結構
public struct PayrolList
{
    public string Company;          // 公司代碼
    public string EmployeeId;       // 員工編號
    public string PayrollDate;      // 薪資日期
    public float FixedSalary;       // 固定薪資
    public float VariableSalary;    // 變動薪資
    public float OvertimePay;       // 加班費
    public float LeaveDeduction;    // 請假扣薪
    public float Tax;               // 所得稅
    public float LaborInsurance;    // 勞保費
    public float HealthInsurance;   // 健保費
    public float NetSalary;         // 實發薪資
}

// 薪資項目結構
public struct SalaryItem
{
    public string ItemCode;         // 項目代碼
    public string ItemName;         // 項目名稱
    public float Amount;            // 金額
    public bool IsTaxable;          // 是否計稅
    public bool IsDeductible;       // 是否可扣除
}

// 薪資參數結構
public struct PayrollParameter
{
    public string Company;          // 公司代碼
    public string ParameterCode;    // 參數代碼
    public string ParameterValue;   // 參數值
    public string Description;      // 參數說明
}
```

## 3. 方法實作詳解

### 3.1 CalculateSalary 方法
```csharp
public static PayrolList CalculateSalary(string company, string employeeId, string payrollDate)
{
    // 參數驗證
    ValidateParameters(company, employeeId, payrollDate);

    // 建立回傳物件
    PayrolList result = new PayrolList();
    result.Company = company;
    result.EmployeeId = employeeId;
    result.PayrollDate = payrollDate;

    try
    {
        // 1. 取得員工基本資料
        PayrolList employeeData = GetEmployeeData(company, employeeId);
        
        // 2. 計算固定薪資
        result.FixedSalary = CalculateFixedSalary(employeeData);
        
        // 3. 計算變動薪資
        result.VariableSalary = CalculateVariableSalary(employeeData);
        
        // 4. 計算加班費
        result.OvertimePay = CalculateOvertimePay(company, employeeId, payrollDate);
        
        // 5. 計算請假扣薪
        result.LeaveDeduction = CalculateLeaveDeduction(company, employeeId, payrollDate);
        
        // 6. 計算所得稅
        result.Tax = IncomeTax.GetIncomeTax(result);
        
        // 7. 計算勞健保費
        result.LaborInsurance = LaborInsurance.GetPremium(result);
        result.HealthInsurance = HealthInsurance.GetPremium(result);
        
        // 8. 計算實發薪資
        result.NetSalary = CalculateNetSalary(result);
    }
    catch (Exception ex)
    {
        // 錯誤處理
        LogError(ex);
        throw new PayrollCalculationException("薪資計算失敗", ex);
    }

    return result;
}
```

### 3.2 GetSalaryItem 方法
```csharp
public static SalaryItem GetSalaryItem(string company, string employeeId, string itemCode)
{
    // 參數驗證
    ValidateParameters(company, employeeId, itemCode);

    SalaryItem result = new SalaryItem();
    result.ItemCode = itemCode;

    try
    {
        // 1. 從快取或資料庫取得項目基本資料
        result = GetSalaryItemFromCache(itemCode) ?? GetSalaryItemFromDB(itemCode);
        
        // 2. 計算項目金額
        result.Amount = CalculateItemAmount(company, employeeId, itemCode);
        
        // 3. 設定稅務屬性
        SetTaxAttributes(ref result);
    }
    catch (Exception ex)
    {
        // 錯誤處理
        LogError(ex);
        throw new SalaryItemException("薪資項目取得失敗", ex);
    }

    return result;
}
```

## 4. 資料處理邏輯

### 4.1 固定薪資計算
```csharp
private static float CalculateFixedSalary(PayrolList employeeData)
{
    float result = 0;
    
    // 1. 取得基本薪資
    float baseSalary = GetBaseSalary(employeeData.Company, employeeData.EmployeeId);
    
    // 2. 計算破月薪資
    if (IsPartialMonth(employeeData.PayrollDate))
    {
        result = CalculatePartialMonthSalary(baseSalary, employeeData.PayrollDate);
    }
    else
    {
        result = baseSalary;
    }
    
    // 3. 加計固定津貼
    result += GetFixedAllowances(employeeData);
    
    return result;
}
```

### 4.2 變動薪資計算
```csharp
private static float CalculateVariableSalary(PayrolList employeeData)
{
    float result = 0;
    
    // 1. 取得變動薪資項目
    var variableItems = GetVariableSalaryItems(employeeData.Company, employeeData.EmployeeId);
    
    // 2. 計算各項目金額
    foreach (var item in variableItems)
    {
        result += CalculateVariableItemAmount(item, employeeData);
    }
    
    return result;
}
```

## 5. 快取機制實作

### 5.1 快取初始化
```csharp
private static void InitializeCache()
{
    // 初始化薪資項目快取
    InitializeSalaryItemCache();
    
    // 初始化員工資料快取
    InitializeEmployeeCache();
    
    // 初始化參數快取
    InitializeParameterCache();
}
```

### 5.2 快取更新機制
```csharp
private static void UpdateCache(string cacheKey, object data)
{
    // 檢查快取是否過期
    if (IsCacheExpired(cacheKey))
    {
        // 更新快取
        CacheManager.Update(cacheKey, data);
        
        // 記錄快取更新
        LogCacheUpdate(cacheKey);
    }
}
```

## 6. 錯誤處理實作

### 6.1 自訂例外類別
```csharp
public class PayrollCalculationException : Exception
{
    public string ErrorCode { get; private set; }
    public string ErrorMessage { get; private set; }
    
    public PayrollCalculationException(string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = "E003";
        ErrorMessage = message;
    }
}
```

### 6.2 錯誤記錄機制
```csharp
private static void LogError(Exception ex)
{
    // 建立錯誤記錄
    ErrorLog log = new ErrorLog
    {
        ErrorCode = (ex as PayrollCalculationException)?.ErrorCode ?? "E999",
        ErrorMessage = ex.Message,
        StackTrace = ex.StackTrace,
        Timestamp = DateTime.Now
    };
    
    // 寫入錯誤日誌
    ErrorLogger.Write(log);
}
```

## 7. 效能優化

### 7.1 批次處理機制
```csharp
public static List<PayrolList> BatchCalculateSalary(string company, List<string> employeeIds, string payrollDate)
{
    List<PayrolList> results = new List<PayrolList>();
    
    // 使用平行處理
    Parallel.ForEach(employeeIds, employeeId =>
    {
        try
        {
            var result = CalculateSalary(company, employeeId, payrollDate);
            lock (results)
            {
                results.Add(result);
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    });
    
    return results;
}
```

### 7.2 資料庫查詢優化
```csharp
private static DataTable GetEmployeeDataFromDB(string company, string employeeId)
{
    // 使用參數化查詢
    string sql = @"
        SELECT * FROM EmployeeMaster 
        WHERE Company = @Company 
        AND EmployeeId = @EmployeeId";
        
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Company", company),
        new SqlParameter("@EmployeeId", employeeId)
    };
    
    return DBManager.ExecuteDataTable(sql, parameters);
}
```

## 8. 安全機制實作

### 8.1 權限檢查
```csharp
private static void CheckPermission(string userId, string operation)
{
    if (!PermissionManager.HasPermission(userId, operation))
    {
        throw new UnauthorizedAccessException("使用者無權執行此操作");
    }
}
```

### 8.2 資料驗證
```csharp
private static void ValidateParameters(string company, string employeeId, string payrollDate)
{
    if (string.IsNullOrEmpty(company))
        throw new ArgumentException("公司代碼不得為空");
        
    if (string.IsNullOrEmpty(employeeId))
        throw new ArgumentException("員工編號不得為空");
        
    if (!DateTime.TryParse(payrollDate, out _))
        throw new ArgumentException("薪資日期格式錯誤");
}
```

## 9. 單元測試規範

### 9.1 測試案例範例
```csharp
[TestClass]
public class PayrollTests
{
    [TestMethod]
    public void CalculateSalary_ValidInput_ReturnsCorrectResult()
    {
        // Arrange
        string company = "TEST";
        string employeeId = "EMP001";
        string payrollDate = "2024/04/01";
        
        // Act
        var result = Payroll.CalculateSalary(company, employeeId, payrollDate);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(company, result.Company);
        Assert.AreEqual(employeeId, result.EmployeeId);
        Assert.IsTrue(result.NetSalary > 0);
    }
}
```

### 9.2 測試覆蓋範圍
- 正常流程測試
- 異常流程測試
- 邊界條件測試
- 效能測試
- 安全測試

## 10. 資料庫操作

### 10.1 使用資料表
- PayrollMaster：薪資主檔
- SalaryItemMaster：薪資項目主檔
- EmployeeMaster：員工主檔
- PayrollParameter：薪資參數檔

### 10.2 主要SQL查詢
```sql
-- 取得員工薪資資料
SELECT * FROM PayrollMaster 
WHERE Company = @Company 
AND EmployeeId = @EmployeeId 
AND PayrollDate = @PayrollDate

-- 取得薪資項目資料
SELECT * FROM SalaryItemMaster 
WHERE ItemCode = @ItemCode
```

## 11. 維護注意事項

### 11.1 程式修改
- 修改計算邏輯需同步更新文件
- 新增薪資項目需更新資料表結構
- 修改參數需考慮歷史資料影響

### 11.2 資料維護
- 定期備份薪資資料
- 維護薪資項目主檔
- 更新稅率及費率資料

## 12. 版本歷史

| 版本 | 日期 | 作者 | 說明 |
|------|------|------|------|
| 1.0 | 2024/04/29 | 系統開發團隊 | 初始版本 |
| 1.1 | 2024/04/30 | 系統開發團隊 | 新增批次處理功能 |
| 1.2 | 2024/05/01 | 系統開發團隊 | 優化快取機制 | 