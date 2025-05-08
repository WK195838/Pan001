# Page_BaseClass 頁面基底類別規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | Page_BaseClass                       |
| 程式名稱     | 頁面基底類別                           |
| 檔案大小     | 4.2KB                                |
| 行數        | ~165                                 |
| 功能簡述     | 提供頁面共用功能                        |
| 複雜度       | 中                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/19                           |

## 程式功能概述

Page_BaseClass 是泛太總帳系統中的頁面基底類別，作為系統中所有網頁的基礎類別，提供共用的功能和屬性。這個類別繼承自 System.Web.UI.Page，並擴展了多項功能，讓系統中的各個頁面可以共用相同的基礎邏輯，確保一致性並簡化開發流程。Page_BaseClass 的主要功能包括：

1. 提供統一的頁面初始化和載入邏輯
2. 管理頁面授權和使用者驗證
3. 處理頁面的訊息顯示和通知
4. 提供共用的資料存取方法
5. 管理頁面的狀態和生命週期
6. 提供共用的使用者介面操作方法
7. 處理錯誤和例外狀況
8. 提供多語系支援
9. 封裝瀏覽器相容性處理

通過繼承此基底類別，系統中的各個頁面可以保持一致的行為模式和外觀，同時減少重複代碼。

## 類別結構說明

Page_BaseClass 是一個抽象基底類別，提供了豐富的屬性和方法供子類別使用：

1. **基本屬性**：包括使用者資訊、權限設定、頁面設定等
2. **介面方法**：提供統一的使用者介面操作介面
3. **資料存取**：封裝資料庫存取的共用方法
4. **訊息處理**：統一的訊息顯示和處理機制
5. **頁面生命週期**：管理頁面的初始化、載入和卸載過程
6. **錯誤處理**：提供完整的錯誤捕獲和處理機制

整體設計遵循物件導向的繼承原則，提供適當的虛擬方法和事件點供子類別覆寫和擴展。

## 技術實現

Page_BaseClass 基於以下技術實現：

1. **ASP.NET**：繼承 System.Web.UI.Page 類別，使用 ASP.NET 頁面生命週期
2. **C#**：使用 C# 程式語言實現所有功能
3. **物件導向程式設計**：採用繼承和多型機制設計類別層次結構
4. **事件驅動模型**：利用 ASP.NET 的事件機制處理頁面生命週期
5. **抽象工廠模式**：提供資料存取和業務邏輯的抽象介面
6. **範本方法模式**：定義頁面流程骨架，讓子類別實現特定步驟

## 相依類別和元件

Page_BaseClass 依賴以下類別與元件：

1. **System.Web.UI.Page**：ASP.NET 頁面基礎類別
2. **LoginClass**：用戶登入及認證類別
3. **AppAuthority**：應用程式權限管理類別
4. **DBManger**：資料庫操作管理類別
5. **WebConfigManager**：Web.config 設定管理類別
6. **System.Web.UI.ScriptManager**：JavaScript 和 AJAX 管理

## 屬性說明

Page_BaseClass 提供以下公開屬性：

| 屬性名稱 | 資料類型 | 說明 | 存取權限 |
|---------|---------|------|---------|
| CurrentUser | string | 取得目前登入的使用者帳號 | 公開 |
| UserName | string | 取得目前使用者的顯示名稱 | 公開 |
| UserIP | string | 取得使用者的 IP 地址 | 公開 |
| PageID | string | 取得或設定目前頁面的唯一識別碼 | 公開 |
| PageTitle | string | 取得或設定頁面標題 | 公開 |
| PageMode | PageModeEnum | 取得或設定頁面模式（新增/修改/查詢） | 公開 |
| ErrorMessage | string | 取得最後的錯誤訊息 | 公開 |
| HasError | bool | 表示是否有錯誤發生 | 公開 |
| IsAuthorized | bool | 表示目前用戶是否有權限訪問頁面 | 公開 |
| CurrentCompany | string | 取得目前選取的公司代碼 | 公開 |
| CurrentPeriod | string | 取得目前選取的會計期間 | 公開 |

## 私有成員變數

Page_BaseClass 包含以下重要的私有成員變數：

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _userID | string | 儲存目前使用者 ID |
| _userName | string | 儲存使用者名稱 |
| _pageID | string | 儲存頁面 ID |
| _errorMsg | string | 儲存錯誤訊息 |
| _hasError | bool | 標記是否有錯誤 |
| _isAuthorized | bool | 標記用戶是否有權限 |
| _loginHelper | LoginClass | 登入助手實例 |
| _dbHelper | DBManger | 資料庫操作助手實例 |
| _authHelper | AppAuthority | 權限管理助手實例 |

## 方法說明

### 基本方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Page_Load | object sender, EventArgs e | void | 頁面載入事件處理方法 |
| Page_Init | object sender, EventArgs e | void | 頁面初始化事件處理方法 |
| Page_PreRender | object sender, EventArgs e | void | 頁面預渲染事件處理方法 |
| Page_Unload | object sender, EventArgs e | void | 頁面卸載事件處理方法 |
| CheckUserPermission | string pageID (選填) | bool | 檢查使用者對指定頁面的權限 |

### 資料存取方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| GetConnection | 無 | DBManger | 取得資料庫連接管理器 |
| ExecuteQuery | string sql | DataTable | 執行 SQL 查詢並返回結果集 |
| ExecuteNonQuery | string sql | int | 執行非查詢 SQL 命令並返回影響的行數 |
| ExecuteScalar | string sql | object | 執行查詢並返回第一個結果 |

### 使用者介面方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| ShowMessage | string message | void | 顯示訊息對話框 |
| ShowError | string errorMessage | void | 顯示錯誤訊息 |
| ShowWarning | string warningMessage | void | 顯示警告訊息 |
| ShowConfirm | string message | bool | 顯示確認對話框並返回用戶選擇 |
| RedirectToPage | string pageUrl | void | 重定向到指定頁面 |

### 虛擬方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| InitializePageControls | 無 | void | 虛擬方法，初始化頁面控制項 |
| LoadPageData | 無 | void | 虛擬方法，載入頁面資料 |
| ValidatePageData | 無 | bool | 虛擬方法，驗證頁面資料 |
| SavePageData | 無 | bool | 虛擬方法，保存頁面資料 |
| ClearPageData | 無 | void | 虛擬方法，清除頁面資料 |

## 程式碼說明

### 頁面生命週期處理

```csharp
protected override void OnInit(EventArgs e)
{
    base.OnInit(e);
    
    // 初始化頁面設置
    _loginHelper = new LoginClass();
    _authHelper = new AppAuthority();
    
    // 檢查使用者是否登入
    if (!_loginHelper.IsLoggedIn())
    {
        Response.Redirect("~/AuthAD.aspx", true);
        return;
    }
    
    // 設置基本使用者資訊
    _userID = _loginHelper.GetCurrentUser();
    _userName = _loginHelper.GetUserName(_userID);
    
    // 檢查頁面訪問權限
    _isAuthorized = CheckUserPermission();
    if (!_isAuthorized)
    {
        Response.Redirect("~/AppErrorMessage.aspx?ErrMsg=權限不足，無法訪問此頁面", true);
        return;
    }
    
    // 初始化系統資源
    InitSystemResources();
    
    // 呼叫虛擬方法，讓子類別有機會初始化
    InitializePageControls();
}
```

### 權限檢查

```csharp
protected bool CheckUserPermission(string pageID = "")
{
    if (string.IsNullOrEmpty(pageID))
        pageID = this.PageID;
        
    if (string.IsNullOrEmpty(pageID))
        return true;  // 沒有指定頁面 ID，默認允許訪問
    
    return _authHelper.CanAccessPage(_userID, pageID);
}
```

### 錯誤處理

```csharp
protected void HandleException(Exception ex)
{
    _errorMsg = ex.Message;
    _hasError = true;
    
    // 記錄詳細錯誤資訊
    LogError(ex);
    
    // 顯示適當的錯誤訊息
    ShowError("處理您的請求時發生錯誤，請稍後再試。");
}

private void LogError(Exception ex)
{
    // 記錄錯誤到日誌文件或資料庫
    string logMsg = $"[{DateTime.Now}] User: {_userID}, Page: {_pageID}, Error: {ex.Message}, Stack: {ex.StackTrace}";
    
    // 寫入日誌實現...
}
```

## 使用範例

以下是 Page_BaseClass 的使用範例：

### 繼承基底類別

```csharp
public partial class PTA0150 : Page_BaseClass
{
    // 覆寫屬性
    public override string PageID { get { return "PTA0150"; } }
    
    // 覆寫虛擬方法
    protected override void InitializePageControls()
    {
        // 初始化特定頁面的控制項
    }
    
    protected override void LoadPageData()
    {
        // 載入特定頁面的資料
        try
        {
            string sql = "SELECT * FROM SubjectMaster WHERE Company = @Company";
            // 處理資料載入邏輯...
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }
    
    protected override bool SavePageData()
    {
        // 實現特定頁面的資料保存邏輯
        try
        {
            // 處理資料保存...
            return true;
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return false;
        }
    }
}
```

### 使用頁面基底類別提供的功能

```csharp
protected void btnQuery_Click(object sender, EventArgs e)
{
    try
    {
        // 使用基底類別的資料存取方法
        string sql = "SELECT * FROM SubjectMaster WHERE Subject = @Subject";
        DBManger db = GetConnection();
        db.AddParameter("@Subject", txtSubject.Text);
        DataTable dt = db.ExecuteDataTable(sql);
        
        if (dt.Rows.Count == 0)
        {
            // 使用基底類別的訊息顯示方法
            ShowWarning("查無資料");
            return;
        }
        
        // 處理查詢結果...
    }
    catch (Exception ex)
    {
        // 使用基底類別的錯誤處理
        HandleException(ex);
    }
}
```

## 頁面模式列舉

```csharp
public enum PageModeEnum
{
    Query = 0,    // 查詢模式
    Insert = 1,   // 新增模式
    Update = 2,   // 修改模式
    Delete = 3,   // 刪除模式
    Report = 4    // 報表模式
}
```

## 資料結構

### 輸入參數

作為基底類別，Page_BaseClass 本身不需要輸入參數。子類別通過覆寫虛擬方法和屬性來自定義行為。

### 輸出資料

基底類別提供各種資料存取和使用者介面方法，子類別可以使用這些方法來管理資料流和使用者交互。

## 異常處理

Page_BaseClass 提供了全面的異常處理機制：

1. 使用 try-catch 捕獲可能的異常
2. 提供專用的 HandleException 方法處理異常
3. 實現錯誤日誌記錄功能
4. 向使用者顯示適當的錯誤訊息
5. 將錯誤狀態保存在 HasError 和 ErrorMessage 屬性中

此設計確保了在發生異常時，系統能夠適當地處理並記錄問題，同時為使用者提供友好的錯誤反饋。

## 注意事項與限制

1. **繼承要求**：所有系統頁面都應繼承自此基底類別
2. **權限管理**：頁面必須設置正確的 PageID 才能進行權限檢查
3. **狀態管理**：基底類別使用視圖狀態保存某些資訊，可能對效能有影響
4. **相依關係**：依賴多個其他系統組件，如 LoginClass 和 AppAuthority
5. **生命週期**：子類別必須注意不要干擾基底類別的頁面生命週期處理

## 效能考量

1. **資料庫連接**：管理資料庫連接以確保適當的開啟和關閉
2. **視圖狀態**：控制視圖狀態的大小，避免過多資料保存到視圖狀態中
3. **頁面快取**：適當設置頁面快取策略，提高響應速度
4. **資源釋放**：確保資源在 Page_Unload 中得到適當釋放
5. **異常開銷**：考慮異常處理的效能開銷，避免在非必要情況下使用異常

## 安全性考量

1. **權限驗證**：實施嚴格的權限檢查，防止未授權訪問
2. **資料驗證**：基底類別提供框架，但子類別必須實施資料驗證
3. **SQL 注入**：提供參數化查詢支援，但子類別必須正確使用
4. **跨站腳本攻擊**：提供輸入過濾方法，但子類別必須適當使用
5. **錯誤資訊**：向使用者顯示經過處理的錯誤訊息，防止洩露敏感資訊

## 待改進事項

1. **依賴注入**：考慮使用依賴注入模式，減少硬編碼依賴
2. **日誌機制**：增強日誌記錄機制，支援不同級別的日誌
3. **非同步支援**：增加對非同步操作的支援
4. **模組化擴展**：提供更靈活的擴展點，支援模組化開發
5. **效能監控**：加入頁面效能監控功能，協助識別效能瓶頸
6. **多語系增強**：改進多語系支援，提供更完整的國際化能力

## 相關檔案

1. **LoginClass.cs**：提供使用者登入和認證功能
2. **AppAuthority.cs**：提供權限管理功能
3. **DBManger.cs**：提供資料庫操作功能
4. **WebConfigManager.cs**：提供配置管理功能

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/19 | 1.0    | 首次建立頁面基底類別規格書      | Claude AI  | 