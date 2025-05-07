# AppErrorMessage.aspx.cs 錯誤訊息程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | AppErrorMessage.aspx.cs              |
| 程式名稱     | 錯誤訊息程式碼                          |
| 檔案大小     | 1.5KB                                |
| 行數        | 48                                    |
| 功能簡述     | 錯誤訊息後端邏輯                         |
| 複雜度       | 低                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

AppErrorMessage.aspx.cs 是 AppErrorMessage.aspx 的後端程式碼檔案，負責處理錯誤訊息頁面的程式邏輯。其主要功能包括：

1. 處理頁面載入事件並獲取錯誤相關資訊
2. 格式化並顯示錯誤訊息給使用者
3. 處理頁面上的按鈕點擊事件
4. 記錄錯誤資訊到系統日誌
5. 根據錯誤類型提供不同的處理策略
6. 管理頁面元素的顯示狀態
7. 提供導航功能讓使用者可以離開錯誤頁面

## 主要類別說明

```csharp
public partial class AppErrorMessage : System.Web.UI.Page
{
    // AppErrorMessage 頁面的主要類別，繼承自 System.Web.UI.Page
}
```

## 關鍵方法

### 頁面生命週期方法

| 方法名稱 | 功能說明 |
|---------|---------|
| Page_Load | 頁面載入時執行，初始化頁面元件與獲取錯誤資訊 |
| Page_Init | 頁面初始化時執行，設定基本參數 |

### 錯誤處理方法

| 方法名稱 | 功能說明 |
|---------|---------|
| DisplayErrorMessage | 根據錯誤資訊設定並顯示錯誤訊息 |
| GetErrorInfo | 從各種來源獲取錯誤資訊 |
| LogError | 將錯誤資訊記錄到系統日誌 |
| FilterSensitiveInfo | 過濾可能包含敏感資訊的錯誤詳情 |

### 頁面事件處理方法

| 方法名稱 | 功能說明 |
|---------|---------|
| btnBack_Click | 處理「返回上一頁」按鈕點擊事件 |
| btnHome_Click | 處理「返回首頁」按鈕點擊事件 |

## 程式碼說明

AppErrorMessage.aspx.cs 的主要程式碼結構與流程如下：

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 獲取錯誤資訊
        ErrorInfo errorInfo = GetErrorInfo();
        
        // 記錄錯誤到系統日誌
        LogError(errorInfo);
        
        // 顯示錯誤訊息
        DisplayErrorMessage(errorInfo);
        
        // 設定管理員聯絡資訊
        SetAdminContactInfo();
    }
}

private ErrorInfo GetErrorInfo()
{
    ErrorInfo errorInfo = new ErrorInfo();
    
    // 優先從 Session 中獲取錯誤資訊
    if (Session["ErrorMessage"] != null)
    {
        errorInfo.Message = Session["ErrorMessage"].ToString();
        errorInfo.Source = Session["ErrorSource"]?.ToString();
        errorInfo.StackTrace = Session["ErrorStackTrace"]?.ToString();
        errorInfo.ErrorType = Session["ErrorType"]?.ToString();
        
        // 清除 Session 中的錯誤資訊，避免重複顯示
        Session.Remove("ErrorMessage");
        Session.Remove("ErrorSource");
        Session.Remove("ErrorStackTrace");
        Session.Remove("ErrorType");
    }
    // 其次嘗試從 QueryString 獲取
    else if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
    {
        errorInfo.Message = Request.QueryString["msg"];
        errorInfo.ErrorType = Request.QueryString["type"];
    }
    // 最後獲取系統最後一次錯誤
    else if (Server.GetLastError() != null)
    {
        Exception ex = Server.GetLastError();
        errorInfo.Message = ex.Message;
        errorInfo.Source = ex.Source;
        errorInfo.StackTrace = ex.StackTrace;
        errorInfo.ErrorType = ex.GetType().Name;
        
        // 清除系統錯誤
        Server.ClearError();
    }
    // 如果所有來源都沒有錯誤資訊，顯示通用訊息
    else
    {
        errorInfo.Message = "發生未知錯誤，請聯絡系統管理員。";
        errorInfo.ErrorType = "UnknownError";
    }
    
    return errorInfo;
}

private void DisplayErrorMessage(ErrorInfo errorInfo)
{
    // 設定基本錯誤訊息
    lblErrorMessage.Text = errorInfo.Message;
    
    // 根據使用者權限決定是否顯示詳細資訊
    if (IsAdminUser() && !string.IsNullOrEmpty(errorInfo.StackTrace))
    {
        // 過濾敏感資訊
        string safeStackTrace = FilterSensitiveInfo(errorInfo.StackTrace);
        lblErrorDetails.Text = safeStackTrace;
        lblErrorDetails.Visible = true;
    }
    else
    {
        lblErrorDetails.Visible = false;
    }
    
    // 根據錯誤類型提供建議
    lblSuggestion.Text = GetSuggestionByErrorType(errorInfo.ErrorType);
}

protected void btnBack_Click(object sender, EventArgs e)
{
    // 使用 JavaScript 返回上一頁
    ClientScript.RegisterStartupScript(this.GetType(), "GoBack", "history.go(-1);", true);
}

protected void btnHome_Click(object sender, EventArgs e)
{
    // 導航到首頁
    Response.Redirect("~/Default.aspx");
}
```

## 資料處理

AppErrorMessage.aspx.cs 的資料處理主要圍繞錯誤資訊的獲取與顯示：

1. 從多個來源（Session、QueryString、Server.GetLastError）獲取錯誤資訊
2. 根據錯誤類型和使用者權限控制顯示的詳細程度
3. 過濾錯誤資訊中可能包含的敏感資料
4. 根據錯誤類型生成對應的建議訊息

## 安全性考量

AppErrorMessage.aspx.cs 的安全性措施：

1. 權限控制：僅對管理員顯示詳細的錯誤堆疊資訊
   ```csharp
   if (IsAdminUser() && !string.IsNullOrEmpty(errorInfo.StackTrace))
   {
       lblErrorDetails.Text = FilterSensitiveInfo(errorInfo.StackTrace);
       lblErrorDetails.Visible = true;
   }
   ```

2. 敏感資訊過濾：過濾錯誤資訊中可能包含的敏感資料
   ```csharp
   private string FilterSensitiveInfo(string input)
   {
       if (string.IsNullOrEmpty(input))
           return input;
           
       // 過濾連線字串
       input = Regex.Replace(input, "Data Source=.*;", "Data Source=[隱藏]", RegexOptions.IgnoreCase);
       // 過濾密碼
       input = Regex.Replace(input, "password=.*?;", "password=[隱藏];", RegexOptions.IgnoreCase);
       
       return input;
   }
   ```

3. 系統資訊保護：避免向一般使用者暴露系統架構相關資訊
4. 錯誤訊息標準化：使用統一的錯誤訊息格式，避免洩露系統實作細節

## 錯誤處理

AppErrorMessage.aspx.cs 本身也實施了錯誤處理機制：

```csharp
private void LogError(ErrorInfo errorInfo)
{
    try
    {
        // 將錯誤記錄到系統日誌
        System.Diagnostics.EventLog.WriteEntry(
            "泛太總帳系統", 
            $"錯誤類型：{errorInfo.ErrorType}\n" +
            $"錯誤訊息：{errorInfo.Message}\n" +
            $"錯誤來源：{errorInfo.Source}\n" +
            $"堆疊資訊：{errorInfo.StackTrace}\n" +
            $"使用者：{User.Identity.Name}\n" +
            $"時間：{DateTime.Now}"
        );
    }
    catch (Exception ex)
    {
        // 無法寫入日誌時的備用處理
        System.Diagnostics.Debug.WriteLine("無法記錄錯誤：" + ex.Message);
    }
}
```

## 效能考量

AppErrorMessage.aspx.cs 的效能優化：

1. 輕量級的錯誤處理邏輯，避免在錯誤頁面中進行複雜操作
2. 使用 `IsPostBack` 檢查避免重複處理錯誤資訊
3. 清除不再需要的 Session 資料，減少伺服器資源佔用
4. 最小化資料庫操作，僅在必要時寫入日誌

## 與前端的互動

AppErrorMessage.aspx.cs 與前端頁面的互動主要透過：

1. 設定標籤控制項的文字內容與可見性
   ```csharp
   lblErrorMessage.Text = errorInfo.Message;
   lblErrorDetails.Visible = IsAdminUser();
   ```

2. 處理按鈕點擊事件
   ```csharp
   protected void btnHome_Click(object sender, EventArgs e)
   {
       Response.Redirect("~/Default.aspx");
   }
   ```

3. 使用 ClientScript 執行前端 JavaScript
   ```csharp
   ClientScript.RegisterStartupScript(this.GetType(), "GoBack", "history.go(-1);", true);
   ```

## 與其他元件的整合

AppErrorMessage.aspx.cs 與系統其他元件的整合：

1. Global.asax：全局錯誤處理機制將錯誤轉導至此頁面
2. Web.config：錯誤處理設定
3. 日誌系統：記錄錯誤資訊
4. 使用者權限系統：根據權限控制錯誤詳情顯示

## 測試摘要

AppErrorMessage.aspx.cs 經過以下測試：

1. 各種來源的錯誤資訊獲取與顯示測試
2. 不同使用者權限下的錯誤詳情顯示測試
3. 頁面導航功能測試
4. 敏感資訊過濾功能測試
5. 日誌記錄功能測試

## 依賴關係

AppErrorMessage.aspx.cs 依賴以下組件與資源：

1. System.Web.UI：ASP.NET Web Forms 核心功能
2. System.Diagnostics：用於錯誤日誌記錄
3. System.Text.RegularExpressions：用於敏感資訊過濾
4. 自定義的 ErrorInfo 類別：用於封裝錯誤資訊

## 維護與擴展性

AppErrorMessage.aspx.cs 的設計考慮到未來的維護與擴展：

1. 模組化的錯誤處理方法，便於擴展新的錯誤類型處理
2. 清晰的方法命名與適當的註釋
3. 獨立的敏感資訊過濾機制，便於維護安全規則
4. 支援多種錯誤資訊來源，便於系統擴展

## 待改進事項

AppErrorMessage.aspx.cs 可考慮以下改進：

1. 增加錯誤報告機制，讓使用者可以提交錯誤報告
2. 支援多語言錯誤訊息
3. 實作更完善的錯誤分類與處理策略
4. 增加錯誤統計功能，幫助識別常見問題

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 自定義錯誤資訊類別

```csharp
/// <summary>
/// 封裝錯誤資訊的類別
/// </summary>
public class ErrorInfo
{
    public string Message { get; set; }
    public string Source { get; set; }
    public string StackTrace { get; set; }
    public string ErrorType { get; set; }
    
    public ErrorInfo()
    {
        Message = "";
        Source = "";
        StackTrace = "";
        ErrorType = "General";
    }
}
```

### 常見錯誤類型與建議訊息對應

```csharp
private string GetSuggestionByErrorType(string errorType)
{
    switch (errorType)
    {
        case "SqlException":
            return "資料庫連線出現問題，請稍後再試或聯絡系統管理員。";
            
        case "AuthenticationException":
            return "您的登入狀態可能已過期，請重新登入系統。";
            
        case "AccessViolationException":
            return "您沒有執行此操作的權限，請確認您的帳號權限或聯絡系統管理員。";
            
        case "TimeoutException":
            return "系統操作超時，請確認網路連線後再試。";
            
        default:
            return "如果問題持續發生，請重新整理頁面或聯絡系統管理員。";
    }
} 