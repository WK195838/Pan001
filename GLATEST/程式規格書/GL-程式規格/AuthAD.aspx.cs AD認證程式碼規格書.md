# AuthAD.aspx.cs AD認證程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | AuthAD.aspx.cs                       |
| 程式名稱     | AD認證程式碼                            |
| 檔案大小     | 10KB                                  |
| 行數        | 249                                   |
| 功能簡述     | AD認證後端邏輯                          |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

AuthAD.aspx.cs 是 AuthAD.aspx 的後端程式碼檔案，負責處理 Active Directory 認證頁面的程式邏輯。其主要功能包括：

1. 與 Active Directory 伺服器進行通訊並驗證使用者身份
2. 處理登入表單提交事件並執行認證流程
3. 驗證使用者在系統中的權限與角色設定
4. 處理各種認證相關的例外狀況
5. 管理使用者的 Session 與 Cookie 資訊
6. 實作安全性機制防範常見登入攻擊
7. 執行認證成功後的導航邏輯

## 主要類別說明

```csharp
public partial class AuthAD : System.Web.UI.Page
{
    // AuthAD 頁面的主要類別，繼承自 System.Web.UI.Page
}
```

## 關鍵方法

### 頁面生命週期方法

| 方法名稱 | 功能說明 |
|---------|---------|
| Page_Load | 頁面載入時執行，初始化頁面元件與資料 |
| Page_Init | 頁面初始化時執行，設定基本參數 |

### 認證相關方法

| 方法名稱 | 功能說明 |
|---------|---------|
| btnLogin_Click | 處理登入按鈕點擊事件，開始認證流程 |
| ValidateCredentials | 驗證使用者憑證是否有效 |
| ValidateADCredentials | 透過 Active Directory 驗證使用者身份 |
| GetUserInfoFromAD | 從 Active Directory 獲取使用者詳細資訊 |
| SetupUserSession | 設定使用者的 Session 資訊 |
| RedirectAfterLogin | 登入成功後導航至適當頁面 |
| HandleLoginFailure | 處理登入失敗的情況並顯示錯誤訊息 |

## 程式碼說明

AuthAD.aspx.cs 的主要程式碼結構與流程如下：

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    // 檢查是否已經登入
    if (Session["UserID"] != null)
    {
        Response.Redirect("~/Default.aspx");
        return;
    }

    // 檢查是否有來自 Cookie 的記住帳號
    if (!IsPostBack)
    {
        if (Request.Cookies["RememberUser"] != null)
        {
            txtUserName.Text = Request.Cookies["RememberUser"].Value;
            chkRemember.Checked = true;
        }
    }
}

protected void btnLogin_Click(object sender, EventArgs e)
{
    // 清空之前的錯誤訊息
    lblMessage.Text = "";
    
    // 檢查輸入的有效性
    if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
    {
        lblMessage.Text = "請輸入使用者名稱和密碼";
        return;
    }
    
    try
    {
        // 驗證 AD 認證
        if (ValidateADCredentials(txtUserName.Text, txtPassword.Text))
        {
            // 從 AD 獲取使用者資訊
            UserInfo userInfo = GetUserInfoFromAD(txtUserName.Text);
            
            // 檢查使用者是否有權限使用系統
            if (ValidateUserPermission(userInfo))
            {
                // 設定使用者 Session
                SetupUserSession(userInfo);
                
                // 處理「記住帳號」選項
                HandleRememberMe();
                
                // 導航至首頁或之前嘗試訪問的頁面
                RedirectAfterLogin();
            }
            else
            {
                lblMessage.Text = "您沒有使用此系統的權限";
            }
        }
        else
        {
            lblMessage.Text = "使用者名稱或密碼不正確";
            LogFailedLoginAttempt(txtUserName.Text);
        }
    }
    catch (Exception ex)
    {
        // 處理認證過程中的異常
        HandleLoginException(ex);
    }
}

private bool ValidateADCredentials(string username, string password)
{
    try
    {
        // 設定 LDAP 路徑
        string ldapPath = ConfigurationManager.AppSettings["LDAPPath"];
        
        // 建立目錄連結
        using (DirectoryEntry entry = new DirectoryEntry(ldapPath, username, password))
        {
            // 檢索目錄對象以驗證憑證
            using (DirectorySearcher searcher = new DirectorySearcher(entry))
            {
                searcher.Filter = String.Format("(SAMAccountName={0})", username);
                SearchResult result = searcher.FindOne();
                return (result != null);
            }
        }
    }
    catch (Exception)
    {
        // 認證失敗
        return false;
    }
}
```

## 資料處理

AuthAD.aspx.cs 的資料處理邏輯主要圍繞使用者認證與權限管理：

1. 收集用戶登入表單數據
2. 連接 Active Directory 進行身份驗證
3. 查詢使用者在 AD 中的詳細資訊
4. 在系統資料庫中驗證使用者的存取權限
5. 將使用者資料和權限資訊儲存到 Session

## 安全性考量

AuthAD.aspx.cs 實施了多層安全措施：

1. 使用 LDAPS (LDAP over SSL) 加密 AD 通訊
2. 防範暴力破解攻擊的機制
   ```csharp
   private void CheckBruteForceAttempts(string username)
   {
       // 從資料庫獲取失敗嘗試次數與時間
       int failedAttempts = GetFailedLoginAttempts(username);
       
       // 超過嘗試次數限制時，暫時鎖定帳號
       if (failedAttempts >= MaxFailedAttempts)
       {
           DateTime lastFailure = GetLastFailedAttemptTime(username);
           TimeSpan timeSinceLastFailure = DateTime.Now - lastFailure;
           
           if (timeSinceLastFailure.TotalMinutes < LockoutDurationMinutes)
           {
               int remainingMinutes = LockoutDurationMinutes - (int)timeSinceLastFailure.TotalMinutes;
               throw new SecurityException(String.Format("帳號已被暫時鎖定，請 {0} 分鐘後再試", remainingMinutes));
           }
           else
           {
               ResetFailedLoginAttempts(username);
           }
       }
   }
   ```

3. 安全地處理錯誤訊息，避免資訊洩露
4. 超時處理與自動登出機制
5. 敏感資訊的安全儲存

## 錯誤處理

AuthAD.aspx.cs 的錯誤處理相當全面：

```csharp
private void HandleLoginException(Exception ex)
{
    // 記錄異常
    LogError(ex);
    
    // 決定顯示給使用者的訊息
    if (ex is DirectoryServicesCOMException)
    {
        lblMessage.Text = "無法連接至 Active Directory 服務，請稍後再試";
    }
    else if (ex is SecurityException)
    {
        lblMessage.Text = ex.Message;
    }
    else
    {
        lblMessage.Text = "登入過程中發生錯誤，請聯絡系統管理員";
    }
}
```

## 效能考量

AuthAD.aspx.cs 的效能優化措施：

1. 快取常用的 AD 連接設定
2. 最佳化 LDAP 查詢以減少網路延遲
3. 使用連接池管理 AD 連接
4. 減少資料庫操作的次數

## 與前端的互動

AuthAD.aspx.cs 與前端頁面的互動主要透過：

1. 表單提交事件處理
   ```csharp
   protected void btnLogin_Click(object sender, EventArgs e)
   {
       // 處理登入邏輯
   }
   ```

2. 錯誤訊息顯示
   ```csharp
   lblMessage.Text = "使用者名稱或密碼不正確";
   ```

3. 頁面導航控制
   ```csharp
   Response.Redirect("~/Default.aspx");
   ```

## 與其他元件的整合

AuthAD.aspx.cs 與系統其他元件的整合：

1. LoginClass：用於管理使用者登入狀態
2. AppAuthority：用於驗證使用者權限
3. 系統設定檔：用於獲取 AD 連接設定
4. 事件日誌系統：記錄登入嘗試與錯誤

## 測試摘要

AuthAD.aspx.cs 經過以下測試：

1. 有效與無效憑證的認證測試
2. AD 連接異常處理測試
3. 使用者權限驗證測試
4. 安全機制測試，包括暴力破解防護
5. Session 與 Cookie 管理測試

## 依賴關係

AuthAD.aspx.cs 依賴以下組件與資源：

1. System.DirectoryServices：用於 AD 連接與查詢
2. System.Web.Security：用於 Forms 認證
3. System.Configuration：用於讀取設定檔
4. 自定義的 LoginClass 與 AppAuthority 類別

## 維護與擴展性

AuthAD.aspx.cs 的設計考慮到未來的維護與擴展：

1. 清晰的方法命名與適當的註釋
2. 模組化的認證與權限驗證邏輯
3. 可配置的設定項，便於調整認證行為
4. 明確的錯誤處理機制

## 待改進事項

AuthAD.aspx.cs 可考慮以下改進：

1. 實作多因素認證 (MFA) 功能
2. 支援 OAuth 或 SAML 等現代認證協議
3. 優化 AD 查詢效能與錯誤處理
4. 增加更詳細的安全日誌與監控

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 主要方法簡介

以下是 AuthAD.aspx.cs 中主要方法的簡要說明：

1. **ValidateADCredentials**：透過 LDAP 連接 AD 伺服器驗證使用者憑證
2. **GetUserInfoFromAD**：從 AD 中獲取使用者詳細資訊，包括部門、職稱等
3. **ValidateUserPermission**：確認使用者是否有權限使用系統
4. **SetupUserSession**：設置使用者的登入 Session
5. **HandleRememberMe**：處理記住使用者登入名稱的 Cookie 設定
6. **LogFailedLoginAttempt**：記錄失敗的登入嘗試
7. **CheckBruteForceAttempts**：檢查是否存在暴力破解攻擊 