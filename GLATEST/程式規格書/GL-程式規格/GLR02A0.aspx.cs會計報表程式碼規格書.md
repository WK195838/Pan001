# GLR02A0.aspx.cs 會計報表程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02A0.aspx.cs                      |
| 程式名稱     | 會計報表程式碼                            |
| 檔案大小     | 12KB                                  |
| 行數        | ~300                                  |
| 功能簡述     | 會計報表後端邏輯                          |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02A0.aspx.cs 是 GLR02A0.aspx 的後端程式碼檔案，負責處理綜合會計報表的程式邏輯。其主要功能包括：

1. 初始化報表類型選項與報表條件控制項
2. 處理使用者選擇報表類型與查詢條件的請求
3. 依照選定的報表類型動態載入對應的報表控制項
4. 根據查詢條件執行資料庫查詢，取得報表所需數據
5. 進行複雜的會計報表計算與資料處理
6. 實現報表的匯出功能（PDF、Excel、Word）
7. 提供報表設定的儲存與載入功能
8. 處理報表比較期間功能的顯示控制
9. 處理異常情況並提供適當的錯誤訊息

## 主要類別說明

```csharp
public partial class GLR02A0 : Page_BaseClass
{
    // GLR02A0 頁面的主要類別，繼承自自定義的頁面基礎類別
}
```

## 關鍵方法

### 頁面生命週期方法

| 方法名稱 | 功能說明 |
|---------|---------|
| Page_Load | 頁面載入時執行，初始化頁面元件與資料 |
| Page_PreRender | 頁面呈現前執行，確保報表顯示正確 |

### 報表控制方法

| 方法名稱 | 功能說明 |
|---------|---------|
| ddlReportType_SelectedIndexChanged | 處理報表類型選擇變更事件 |
| chkCompare_CheckedChanged | 處理比較功能勾選狀態變更事件 |
| btnQuery_Click | 處理查詢按鈕點擊事件，生成報表 |
| btnReset_Click | 處理重設按鈕點擊事件，清除查詢條件 |
| btnSaveSetting_Click | 處理儲存設定按鈕點擊事件 |

### 報表生成方法

| 方法名稱 | 功能說明 |
|---------|---------|
| LoadReportControl | 根據報表類型載入對應的報表控制項 |
| GenerateReport | 生成指定類型的報表數據 |
| GetReportData | 從資料庫獲取報表所需數據 |
| CalculateReportData | 計算報表數據 |
| FormatReportData | 格式化報表數據 |

### 匯出功能方法

| 方法名稱 | 功能說明 |
|---------|---------|
| btnExportPDF_Click | 處理匯出 PDF 按鈕點擊事件 |
| btnExportExcel_Click | 處理匯出 Excel 按鈕點擊事件 |
| btnExportWord_Click | 處理匯出 Word 按鈕點擊事件 |
| ExportToPDF | 將報表匯出為 PDF 格式 |
| ExportToExcel | 將報表匯出為 Excel 格式 |
| ExportToWord | 將報表匯出為 Word 格式 |

## 程式碼說明

GLR02A0.aspx.cs 的主要程式碼結構與流程如下：

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        if (!IsPostBack)
        {
            // 初始化報表類型下拉清單
            InitializeReportTypes();
            
            // 初始化查詢條件控制項
            InitializeControls();
            
            // 載入使用者儲存的報表設定（如果有）
            LoadUserSettings();
            
            // 設定頁面權限
            SetPagePermission();
            
            // 預設載入第一個報表類型的控制項
            LoadReportControl(ddlReportType.SelectedValue);
        }
    }
    catch (Exception ex)
    {
        // 處理頁面載入異常
        HandleException(ex);
    }
}

protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
{
    try
    {
        // 獲取選擇的報表類型
        string reportType = ddlReportType.SelectedValue;
        
        // 載入對應的報表控制項
        LoadReportControl(reportType);
        
        // 依據報表類型調整顯示的查詢條件
        AdjustQueryConditions(reportType);
        
        // 設定報表標題
        litReportTitle.Text = ddlReportType.SelectedItem.Text;
    }
    catch (Exception ex)
    {
        // 處理報表類型變更異常
        HandleException(ex);
    }
}

private void LoadReportControl(string reportType)
{
    // 清除先前的報表控制項
    phReportContent.Controls.Clear();
    
    // 根據報表類型載入對應的報表控制項
    Control reportControl = null;
    
    switch (reportType)
    {
        case "BALANCE_SHEET":
            reportControl = LoadControl("~/ReportTemplate/BalanceSheet.ascx");
            break;
        case "INCOME_STATEMENT":
            reportControl = LoadControl("~/ReportTemplate/IncomeStatement.ascx");
            break;
        case "CASH_FLOW":
            reportControl = LoadControl("~/ReportTemplate/CashFlow.ascx");
            break;
        case "DEPT_INCOME":
            reportControl = LoadControl("~/ReportTemplate/DepartmentIncome.ascx");
            break;
        case "BUDGET_COMPARE":
            reportControl = LoadControl("~/ReportTemplate/BudgetCompare.ascx");
            break;
        case "TAX_REPORT":
            reportControl = LoadControl("~/ReportTemplate/TaxReport.ascx");
            break;
        case "CUSTOM_REPORT":
            reportControl = LoadControl("~/ReportTemplate/CustomReport.ascx");
            break;
        default:
            reportControl = LoadControl("~/ReportTemplate/BalanceSheet.ascx");
            break;
    }
    
    // 將報表控制項加入到佔位符
    if (reportControl != null)
    {
        phReportContent.Controls.Add(reportControl);
    }
}

protected void btnQuery_Click(object sender, EventArgs e)
{
    try
    {
        // 驗證使用者輸入
        if (!ValidateInput())
            return;
        
        // 獲取查詢條件
        int year = ucYearList.SelectedYear;
        int periodFrom = ucPeriodList.SelectedPeriodFrom;
        int periodTo = ucPeriodList.SelectedPeriodTo;
        string reportType = ddlReportType.SelectedValue;
        string displayLevel = ddlDisplayLevel.SelectedValue;
        int unit = Convert.ToInt32(ddlUnit.SelectedValue);
        string department = ddlDepartment.SelectedValue;
        string reportStyle = ddlReportStyle.SelectedValue;
        string language = ddlLanguage.SelectedValue;
        
        // 比較期間參數
        bool enableCompare = chkCompare.Checked;
        int compareYear = 0;
        int comparePeriodFrom = 0;
        int comparePeriodTo = 0;
        
        if (enableCompare)
        {
            compareYear = ucCompareYearList.SelectedYear;
            comparePeriodFrom = ucComparePeriodList.SelectedPeriodFrom;
            comparePeriodTo = ucComparePeriodList.SelectedPeriodTo;
        }
        
        // 生成報表
        DataSet reportData = GenerateReport(
            year, periodFrom, periodTo, 
            reportType, displayLevel, unit, department, reportStyle, language,
            enableCompare, compareYear, comparePeriodFrom, comparePeriodTo);
        
        // 找到報表控制項並設定其資料
        Control reportControl = phReportContent.Controls[0];
        IReportControl reportInterface = reportControl as IReportControl;
        
        if (reportInterface != null)
        {
            reportInterface.BindReportData(reportData);
        }
        
        // 設定報表資訊
        GenerateReportInfo(
            year, periodFrom, periodTo, 
            displayLevel, unit, department,
            enableCompare, compareYear, comparePeriodFrom, comparePeriodTo);
    }
    catch (Exception ex)
    {
        // 處理查詢過程中的異常
        HandleException(ex);
    }
}

private DataSet GenerateReport(
    int year, int periodFrom, int periodTo, 
    string reportType, string displayLevel, int unit, string department, string reportStyle, string language,
    bool enableCompare, int compareYear, int comparePeriodFrom, int comparePeriodTo)
{
    // 創建回傳用的 DataSet
    DataSet ds = new DataSet();
    
    // 建立資料庫連接和命令
    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GLAConnectionString"].ConnectionString))
    {
        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.Connection = conn;
            
            // 根據報表類型選擇對應的存儲過程
            switch (reportType)
            {
                case "BALANCE_SHEET":
                    cmd.CommandText = "GL_GenerateBalanceSheet";
                    break;
                case "INCOME_STATEMENT":
                    cmd.CommandText = "GL_GenerateIncomeStatement";
                    break;
                case "CASH_FLOW":
                    cmd.CommandText = "GL_GenerateCashFlow";
                    break;
                case "DEPT_INCOME":
                    cmd.CommandText = "GL_GenerateDepartmentIncome";
                    break;
                case "BUDGET_COMPARE":
                    cmd.CommandText = "GL_GenerateBudgetCompare";
                    break;
                case "TAX_REPORT":
                    cmd.CommandText = "GL_GenerateTaxReport";
                    break;
                case "CUSTOM_REPORT":
                    cmd.CommandText = "GL_GenerateCustomReport";
                    break;
                default:
                    cmd.CommandText = "GL_GenerateBalanceSheet";
                    break;
            }
            
            cmd.CommandType = CommandType.StoredProcedure;
            
            // 加入基本參數
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.Parameters.AddWithValue("@PeriodFrom", periodFrom);
            cmd.Parameters.AddWithValue("@PeriodTo", periodTo);
            cmd.Parameters.AddWithValue("@DisplayLevel", displayLevel);
            cmd.Parameters.AddWithValue("@Unit", unit);
            cmd.Parameters.AddWithValue("@Department", !string.IsNullOrEmpty(department) && department != "0" ? department : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ReportStyle", reportStyle);
            cmd.Parameters.AddWithValue("@Language", language);
            
            // 加入比較期間參數（如適用）
            if (enableCompare)
            {
                cmd.Parameters.AddWithValue("@EnableCompare", true);
                cmd.Parameters.AddWithValue("@CompareYear", compareYear);
                cmd.Parameters.AddWithValue("@ComparePeriodFrom", comparePeriodFrom);
                cmd.Parameters.AddWithValue("@ComparePeriodTo", comparePeriodTo);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EnableCompare", false);
            }
            
            // 創建資料適配器
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            
            // 開啟連接並填充資料
            conn.Open();
            adapter.Fill(ds);
            
            return ds;
        }
    }
}

protected void btnExportExcel_Click(object sender, EventArgs e)
{
    try
    {
        // 獲取報表標題
        string reportTitle = string.Format(
            "泛太總帳系統 - {0}",
            ddlReportType.SelectedItem.Text);
        
        // 獲取報表資訊
        string reportInfo = lblReportInfo.Text;
        
        // 取得報表控制項
        Control reportControl = phReportContent.Controls[0];
        IReportControl reportInterface = reportControl as IReportControl;
        
        if (reportInterface != null)
        {
            // 獲取報表資料
            DataSet reportData = reportInterface.GetReportData();
            
            // 使用 ExcelManager 服務匯出資料
            ExcelManager excelMgr = new ExcelManager();
            
            // 根據報表類型選擇對應的 Excel 範本
            string templateName = string.Format(
                "Template_{0}.xlsx",
                ddlReportType.SelectedValue);
            
            // 匯出 Excel 檔案
            byte[] excelFile = excelMgr.ExportToExcelWithTemplate(
                reportData, reportTitle, reportInfo, templateName);
            
            // 將檔案傳送給用戶端
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format(
                "attachment; filename={0}_{1}.xlsx",
                ddlReportType.SelectedValue,
                DateTime.Now.ToString("yyyyMMdd")));
            Response.BinaryWrite(excelFile);
            Response.End();
        }
    }
    catch (Exception ex)
    {
        // 處理匯出過程中的異常
        HandleException(ex);
    }
}
```

## 資料處理

GLR02A0.aspx.cs 的資料處理主要圍繞財務報表數據的擷取與加工：

1. 根據報表類型與查詢條件調用不同的存儲過程獲取原始數據
2. 依照會計原則與報表規範處理數據，計算比率、彙總等
3. 準備報表資料供使用者控制項顯示或匯出
4. 格式化報表數據與標題以符合選定的報表樣式與語言

## 安全性考量

GLR02A0.aspx.cs 的安全性措施：

1. 使用參數化查詢防止 SQL 注入攻擊
   ```csharp
   cmd.Parameters.AddWithValue("@Year", year);
   cmd.Parameters.AddWithValue("@PeriodFrom", periodFrom);
   cmd.Parameters.AddWithValue("@PeriodTo", periodTo);
   cmd.Parameters.AddWithValue("@Department", !string.IsNullOrEmpty(department) && department != "0" ? department : (object)DBNull.Value);
   ```

2. 權限控制機制，確保使用者有權限存取報表
   ```csharp
   private void SetPagePermission()
   {
       // 檢查使用者是否有報表存取權限
       if (!HasPermission("GLR02A0"))
       {
           // 如果沒有權限，則轉導至未授權頁面
           Response.Redirect("~/Unauthorized.aspx");
       }
       
       // 根據用戶權限調整可見的報表類型
       AdjustReportTypesByPermission();
   }
   ```

3. 輸入驗證，確保查詢條件有效
   ```csharp
   private bool ValidateInput()
   {
       // 驗證會計期間
       if (ucPeriodList.SelectedPeriodFrom > ucPeriodList.SelectedPeriodTo)
       {
           ShowMessage("開始期間不能大於結束期間");
           return false;
       }
       
       // 比較期間驗證
       if (chkCompare.Checked)
       {
           if (ucComparePeriodList.SelectedPeriodFrom > ucComparePeriodList.SelectedPeriodTo)
           {
               ShowMessage("比較的開始期間不能大於結束期間");
               return false;
           }
       }
       
       return true;
   }
   ```

4. 敏感財務報表資料的存取控制與日誌記錄

## 錯誤處理

GLR02A0.aspx.cs 的錯誤處理：

```csharp
private void HandleException(Exception ex)
{
    // 記錄錯誤詳情
    LogError(ex);
    
    // 決定顯示給使用者的訊息
    string userMessage = "處理報表時發生錯誤";
    
    if (ex is SqlException)
    {
        userMessage = "資料庫連線發生問題，請稍後再試";
    }
    else if (ex is InvalidOperationException)
    {
        userMessage = "報表操作無效，請確認報表類型與查詢條件是否正確";
    }
    
    // 顯示錯誤訊息
    ShowMessage(userMessage);
    
    // 對於嚴重錯誤，可能轉導至錯誤頁面
    if (ex is SystemException)
    {
        // 儲存錯誤資訊到 Session
        Session["ErrorMessage"] = ex.Message;
        Session["ErrorSource"] = ex.Source;
        Session["ErrorStackTrace"] = ex.StackTrace;
        Session["ErrorType"] = ex.GetType().Name;
        
        // 轉導至錯誤頁面
        Response.Redirect("~/AppErrorMessage.aspx");
    }
}
```

## 效能考量

GLR02A0.aspx.cs 的效能優化：

1. 使用存儲過程執行複雜的報表查詢，提升效能
   ```csharp
   cmd.CommandText = "GL_GenerateBalanceSheet";
   cmd.CommandType = CommandType.StoredProcedure;
   ```

2. 使用動態載入控制項，只載入需要的報表模板
   ```csharp
   Control reportControl = LoadControl("~/ReportTemplate/BalanceSheet.ascx");
   ```

3. 實施報表資料快取機制，減少重複查詢
   ```csharp
   private DataSet GetCachedReportData(string cacheKey)
   {
       // 檢查快取中是否已有報表資料
       if (Cache[cacheKey] != null)
       {
           return (DataSet)Cache[cacheKey];
       }
       
       // 如果快取中沒有，則重新生成報表資料
       DataSet ds = GenerateReportData();
       
       // 將報表資料放入快取
       Cache.Insert(cacheKey, ds, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);
       
       return ds;
   }
   ```

4. 針對大型報表實施分區處理，減少記憶體使用
5. 非同步處理報表匯出功能，避免阻塞使用者介面

## 與前端的互動

GLR02A0.aspx.cs 與前端頁面的互動主要透過：

1. 處理頁面控制項的事件
   ```csharp
   protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
   {
       // 處理報表類型變更
   }
   ```

2. 動態載入報表控制項
   ```csharp
   phReportContent.Controls.Clear();
   Control reportControl = LoadControl("~/ReportTemplate/BalanceSheet.ascx");
   phReportContent.Controls.Add(reportControl);
   ```

3. 設定報表資訊標籤內容
   ```csharp
   private void GenerateReportInfo(int year, int periodFrom, int periodTo, string displayLevel, int unit, string department, bool enableCompare, int compareYear, int comparePeriodFrom, int comparePeriodTo)
   {
       // 設定報表資訊
       lblReportInfo.Text = string.Format(
           "會計年度：{0}    會計期間：{1}至{2}    顯示層級：{3}    報表單位：{4}",
           year,
           GetPeriodName(periodFrom),
           GetPeriodName(periodTo),
           GetDisplayLevelName(displayLevel),
           GetUnitName(unit)
       );
       
       // 如果有部門篩選，加入部門資訊
       if (!string.IsNullOrEmpty(department) && department != "0")
       {
           lblReportInfo.Text += string.Format("    部門：{0}", GetDepartmentName(department));
       }
       
       // 如果啟用比較，加入比較期間資訊
       if (enableCompare)
       {
           lblReportInfo.Text += string.Format(
               "    比較期間：{0}年{1}至{2}",
               compareYear,
               GetPeriodName(comparePeriodFrom),
               GetPeriodName(comparePeriodTo)
           );
       }
   }
   ```

## 與其他元件的整合

GLR02A0.aspx.cs 與系統其他元件的整合：

1. 使用者控制項的整合（YearList、PeriodList）
   ```csharp
   private void InitializeControls()
   {
       // 初始化年度清單控制項
       ucYearList.InitializeList();
       
       // 初始化期間清單控制項
       ucPeriodList.InitializeList();
       
       // 初始化比較期間控制項
       ucCompareYearList.InitializeList();
       ucComparePeriodList.InitializeList();
       
       // 初始化部門下拉清單
       InitializeDepartmentDropDownList();
   }
   ```

2. 報表範本控制項的整合
   ```csharp
   private void InitializeReportTemplate(Control reportControl, string reportType)
   {
       // 設定報表範本的屬性與行為
       if (reportControl is IReportControl)
       {
           IReportControl template = (IReportControl)reportControl;
           template.ReportType = reportType;
           template.LanguageCode = ddlLanguage.SelectedValue;
           template.ReportStyle = ddlReportStyle.SelectedValue;
           template.Initialize();
       }
   }
   ```

3. 匯出服務的整合（PDF、Excel、Word）
   ```csharp
   protected void btnExportPDF_Click(object sender, EventArgs e)
   {
       // 使用 PDFExporter 服務匯出 PDF
       PDFExporter pdfExp = new PDFExporter();
       byte[] pdfFile = pdfExp.ExportToPDF(reportData, reportTitle, reportInfo);
       // ...
   }
   ```

## 測試摘要

GLR02A0.aspx.cs 經過以下測試：

1. 不同報表類型選擇的正確控制項載入測試
2. 查詢條件變更與驗證測試
3. 報表資料查詢與顯示的正確性測試
4. 報表比較功能的正確性測試
5. 多種匯出格式的功能測試
6. 各類型報表的數據計算正確性測試
7. 異常處理與錯誤訊息測試

## 依賴關係

GLR02A0.aspx.cs 依賴以下組件與資源：

1. System.Data：用於資料庫操作與資料表處理
2. System.Configuration：用於讀取設定檔
3. 自定義的 Page_BaseClass：提供頁面共用功能
4. IReportControl 介面：定義報表控制項的通用介面
5. 各類報表範本控制項（BalanceSheet.ascx 等）
6. ExcelManager：提供 Excel 匯出功能
7. PDFExporter：提供 PDF 匯出功能
8. WordExporter：提供 Word 匯出功能

## 維護與擴展性

GLR02A0.aspx.cs 的設計考慮到未來的維護與擴展：

1. 使用控制項架構與介面設計，便於增加新的報表類型
2. 將報表邏輯封裝於獨立的用戶控制項，便於維護與修改
3. 使用配置檔案管理報表類型與參數，減少程式碼變更
4. 使用模板設計模式處理不同類型的報表生成邏輯

## 待改進事項

GLR02A0.aspx.cs 可考慮以下改進：

1. 實現更靈活的報表自訂功能，支援使用者自定義欄位與格式
2. 增強報表快取機制，提高重複查詢的效能
3. 實現非同步報表生成功能，避免長時間查詢阻塞用戶介面
4. 增加報表訂閱與排程功能，支援定期產生與分發報表
5. 優化大型報表的記憶體使用，提升系統穩定性

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 報表介面定義

```csharp
/// <summary>
/// 定義報表控制項必須實現的介面
/// </summary>
public interface IReportControl
{
    /// <summary>
    /// 報表類型
    /// </summary>
    string ReportType { get; set; }
    
    /// <summary>
    /// 語言代碼
    /// </summary>
    string LanguageCode { get; set; }
    
    /// <summary>
    /// 報表樣式
    /// </summary>
    string ReportStyle { get; set; }
    
    /// <summary>
    /// 初始化報表控制項
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// 綁定報表資料
    /// </summary>
    /// <param name="data">報表資料集</param>
    void BindReportData(DataSet data);
    
    /// <summary>
    /// 取得報表資料
    /// </summary>
    /// <returns>報表資料集</returns>
    DataSet GetReportData();
    
    /// <summary>
    /// 準備報表資料供匯出
    /// </summary>
    /// <param name="exportType">匯出格式類型</param>
    /// <returns>準備好供匯出的資料</returns>
    object PrepareExportData(string exportType);
}