# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | CheckAuthorization |
| 程式名稱 | 權限檢查控制項 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Controls/CheckAuthorization.ascx, /GLATEST/app/Controls/CheckAuthorization.ascx.cs |
| 程式類型 | 使用者控制項 (User Control) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

CheckAuthorization 使用者控制項是泛太總帳系統的權限檢查核心元件，負責檢驗當前使用者對特定功能的存取權限。此控制項可嵌入任何需要權限控制的頁面中，提供一致且安全的授權機制，確保使用者只能存取被授權的功能。

### 2.2 業務流程

CheckAuthorization在泛太總帳系統中扮演以下角色：
1. 檢查使用者對特定功能模組的存取權限
2. 根據權限狀態控制頁面元素的可見性和可使用性
3. 提供多層次授權等級控制（唯讀、可編輯、管理權限）
4. 記錄未授權的存取嘗試
5. 處理授權失敗的情況並顯示適當的訊息

### 2.3 使用頻率

- 高頻率：系統中的每個功能頁面載入時都會呼叫此控制項
- 平均每個使用者每天觸發此控制項100-200次

### 2.4 使用者角色

- 透明使用：使用者無需直接操作此控制項
- 受影響使用者包括：
  - 系統管理員
  - 財務主管
  - 會計人員
  - 稽核人員
  - 一般使用者
  - 唯讀使用者

## 3. 系統架構

### 3.1 技術架構

- 開發框架：ASP.NET Web Forms (.NET Framework 4.0)
- 程式語言：C#, HTML, CSS
- 技術依賴：
  - AppAuthority 權限管理類別
  - LoginClass 登入模組
  - IBosDB 資料庫存取類別

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| USER_AUTH | 使用者權限表 | 讀取 |
| USER_ROLE | 使用者角色表 | 讀取 |
| ROLE_AUTH | 角色權限表 | 讀取 |
| SYS_FUNC | 系統功能表 | 讀取 |
| SYS_LOG | 系統日誌表 | 寫入 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| AppAuthority | 權限管理 | 提供系統權限控制邏輯 |
| LoginClass | 登入模組 | 提供使用者身份識別 |
| IBosDB | 資料庫存取 | 提供資料庫連接和操作 |
| Logger | 日誌記錄 | 提供系統日誌記錄功能 |

## 4. 畫面規格

### 4.1 畫面布局

CheckAuthorization 為隱藏式控制項，不具有可視化界面。在頁面中以標籤形式存在，但不直接顯示在使用者介面上。

### 4.2 控制項標記

```html
<cc1:CheckAuthorization ID="CheckAuth" runat="server" 
    FunctionID="PTA0150" 
    RequiredLevel="Edit" 
    AutoRedirect="true" 
    ErrorMessage="您沒有足夠的權限存取此功能" />
```

### 4.3 屬性說明

| 屬性名稱 | 類型 | 預設值 | 說明 |
|---------|------|-------|------|
| FunctionID | 字串 | 空字串 | 要檢查權限的功能代碼 |
| RequiredLevel | 列舉 | View | 所需權限等級：None, View, Edit, Admin |
| AutoRedirect | 布林值 | true | 權限不足時是否自動導向錯誤頁面 |
| ErrorMessage | 字串 | "權限不足" | 權限不足時顯示的錯誤訊息 |
| AllowAnonymous | 布林值 | false | 是否允許匿名訪問 |
| CheckOnLoad | 布林值 | true | 是否在頁面載入時自動檢查權限 |

### 4.4 錯誤顯示

當權限檢查失敗且AutoRedirect設為false時，控制項會產生一個錯誤面板，顯示如下：

```
+------------------------------------------------------+
|                      錯誤訊息                        |
+------------------------------------------------------+
|                                                      |
| 警告: 您沒有足夠的權限存取此功能                     |
|                                                      |
| 功能代碼: PTA0150                                    |
| 所需權限: 編輯權限                                   |
| 您的權限: 唯讀權限                                   |
|                                                      |
| 請聯絡系統管理員申請權限，或返回上一頁。             |
|                                                      |
| [返回] [首頁] [聯絡管理員]                           |
|                                                      |
+------------------------------------------------------+
```

## 5. 處理邏輯

### 5.1 主要流程

```
開始
 ↓
控制項初始化
 ↓
從頁面中獲取使用者會話資訊 → 使用者未登入 → 重定向至登入頁面 → 結束
 ↓
檢查系統維護狀態 → 系統維護中 → 顯示系統維護訊息 → 結束
 ↓
檢查AllowAnonymous屬性 → 允許匿名訪問 → 授權成功 → 結束
 ↓
從參數取得功能代碼(FunctionID)
 ↓
從參數取得所需權限等級(RequiredLevel)
 ↓
呼叫AppAuthority檢查使用者權限
 ↓
檢查權限結果 → 授權失敗 → 處理授權失敗 → 結束
 ↓
記錄成功授權日誌
 ↓
啟用頁面功能
 ↓
結束
```

### 5.2 權限檢查邏輯

```
開始
 ↓
從Session獲取使用者ID
 ↓
查詢USER_AUTH表檢查使用者直接授權
 ↓
權限存在? → 是 → 返回授權等級 → 結束
 ↓
從USER_ROLE表獲取使用者所屬角色
 ↓
循環檢查每個角色的權限(ROLE_AUTH表)
 ↓
任一角色有權限? → 是 → 返回最高授權等級 → 結束
 ↓
檢查SYS_FUNC表中功能的預設權限
 ↓
返回預設權限或無權限
 ↓
結束
```

### 5.3 授權失敗處理

```
開始
 ↓
檢查AutoRedirect屬性
 ↓
AutoRedirect為true? → 是 → 重定向至錯誤頁面 → 結束
 ↓
生成錯誤面板
 ↓
顯示ErrorMessage內容
 ↓
記錄未授權存取日誌
 ↓
結束
```

### 5.4 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 功能代碼未指定 | 記錄警告並允許訪問 | 不受影響 |
| 使用者會話過期 | 重定向至登入頁面 | 顯示「會話已過期，請重新登入」訊息 |
| 資料庫連接失敗 | 記錄錯誤並允許降級訪問 | 根據設定顯示降級訪問界面 |
| 權限表結構異常 | 記錄嚴重錯誤並通知管理員 | 顯示「系統發生錯誤，請聯絡管理員」 |

## 6. 程式碼說明

### 6.1 重要方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 控制項載入初始化 | object sender, EventArgs e | void |
| CheckUserAuthorization | 檢查使用者權限 | string userId, string funcId, AuthLevel requiredLevel | bool |
| GetUserAuthLevel | 獲取使用者權限等級 | string userId, string funcId | AuthLevel |
| HandleAuthorizationFailure | 處理授權失敗 | string message | void |
| RenderErrorPanel | 渲染錯誤面板 | string message | void |
| LogAuthorizationAttempt | 記錄授權嘗試 | string userId, string funcId, bool isSuccessful | void |
| RedirectToErrorPage | 導向錯誤頁面 | string message | void |

### 6.2 程式碼區塊

#### 6.2.1 控制項初始化

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        // 如果設定為頁面載入時檢查權限
        if (CheckOnLoad)
        {
            // 檢查使用者會話是否存在
            if (Session["UserID"] == null && !AllowAnonymous)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // 檢查系統維護狀態
            if (IsSystemMaintenance())
            {
                HandleSystemMaintenance();
                return;
            }

            // 執行權限檢查
            PerformAuthorizationCheck();
        }
    }
    catch (Exception ex)
    {
        // 記錄錯誤並處理
        Logger.Error("CheckAuthorization Page_Load Error", ex);
        HandleAuthorizationFailure("權限檢查時發生錯誤，請聯絡系統管理員");
    }
}
```

#### 6.2.2 執行權限檢查

```csharp
private void PerformAuthorizationCheck()
{
    try
    {
        // 如果允許匿名訪問，則跳過檢查
        if (AllowAnonymous)
        {
            return;
        }

        // 獲取使用者ID
        string userId = Session["UserID"].ToString();
        
        // 檢查功能代碼是否有效
        if (string.IsNullOrEmpty(FunctionID))
        {
            Logger.Warn("FunctionID is not specified in CheckAuthorization");
            return;
        }

        // 呼叫AppAuthority檢查權限
        AppAuthority auth = new AppAuthority();
        bool isAuthorized = auth.CheckFunctionAuth(userId, FunctionID, (int)RequiredLevel);

        // 記錄授權嘗試
        LogAuthorizationAttempt(userId, FunctionID, isAuthorized);

        // 處理授權結果
        if (!isAuthorized)
        {
            string errorMsg = string.IsNullOrEmpty(ErrorMessage) 
                ? "您沒有足夠的權限存取此功能" 
                : ErrorMessage;
                
            HandleAuthorizationFailure(errorMsg);
        }
        else
        {
            // 授權成功，設定相關狀態
            Page.Items["IsAuthorized"] = true;
            Page.Items["AuthLevel"] = RequiredLevel;
        }
    }
    catch (Exception ex)
    {
        Logger.Error("PerformAuthorizationCheck Error", ex);
        throw;
    }
}
```

#### 6.2.3 處理授權失敗

```csharp
private void HandleAuthorizationFailure(string message)
{
    try
    {
        // 設定頁面授權狀態
        Page.Items["IsAuthorized"] = false;
        
        // 根據AutoRedirect設定決定處理方式
        if (AutoRedirect)
        {
            // 記錄錯誤後導向錯誤頁面
            Logger.Warn($"Authorization failed: {message}, User: {Session["UserID"]}, Function: {FunctionID}");
            RedirectToErrorPage(message);
        }
        else
        {
            // 在當前頁面顯示錯誤面板
            RenderErrorPanel(message);
            
            // 禁用頁面上的功能控件
            DisablePageControls();
        }
    }
    catch (Exception ex)
    {
        Logger.Error("HandleAuthorizationFailure Error", ex);
        throw;
    }
}
```

#### 6.2.4 記錄授權嘗試

```csharp
private void LogAuthorizationAttempt(string userId, string funcId, bool isSuccessful)
{
    try
    {
        using (IBosDB db = GetBOSDB())
        {
            string sql = @"
                INSERT INTO SYS_LOG 
                (LOG_TYPE, USER_ID, FUNCTION_ID, ACTION, ACTION_TIME, IP_ADDRESS, RESULT, REMARKS)
                VALUES 
                ('AUTH', @USER_ID, @FUNC_ID, 'ACCESS', GETDATE(), @IP, @RESULT, @REMARKS)";
                
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@USER_ID", userId);
            parameters.Add("@FUNC_ID", funcId);
            parameters.Add("@IP", Request.UserHostAddress);
            parameters.Add("@RESULT", isSuccessful ? "SUCCESS" : "FAILURE");
            parameters.Add("@REMARKS", isSuccessful 
                ? "授權成功" 
                : $"授權失敗：所需權限 {RequiredLevel}");
            
            db.ExecuteNonQuery(sql, parameters);
        }
    }
    catch (Exception ex)
    {
        // 記錄錯誤但不中斷流程
        Logger.Error("LogAuthorizationAttempt Error", ex);
    }
}
```

### 6.3 關鍵列舉定義

```csharp
/// <summary>
/// 權限等級列舉
/// </summary>
public enum AuthLevel
{
    /// <summary>無權限</summary>
    None = 0,
    
    /// <summary>檢視權限</summary>
    View = 1,
    
    /// <summary>編輯權限</summary>
    Edit = 2,
    
    /// <summary>管理權限</summary>
    Admin = 3
}
```

## 7. 測試案例

### 7.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 未登入存取檢查 | 模擬未登入狀態 | 重定向至登入頁面 | Session["UserID"] = null |
| UT-002 | 權限檢查-授權成功 | 使用具有足夠權限的使用者 | 允許訪問頁面 | 使用者具有Edit權限，RequiredLevel = Edit |
| UT-003 | 權限檢查-授權失敗 | 使用權限不足的使用者 | 顯示錯誤訊息或重定向 | 使用者具有View權限，RequiredLevel = Edit |
| UT-004 | 匿名訪問測試 | 設定AllowAnonymous = true | 允許未登入使用者訪問 | Session["UserID"] = null, AllowAnonymous = true |
| UT-005 | 自定義錯誤訊息 | 設定自定義ErrorMessage | 顯示自定義錯誤訊息 | 權限不足時顯示指定的ErrorMessage |

### 7.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 與頁面控制項整合 | 在頁面中嵌入控制項並測試權限控制 | 根據權限正確控制頁面功能 | 使用不同權限等級測試 |
| IT-002 | 與AppAuthority整合 | 確認控制項正確調用AppAuthority | 權限判斷結果一致 | 跟踪AppAuthority呼叫 |
| IT-003 | 日誌記錄整合 | 檢查授權嘗試日誌記錄 | 成功寫入SYS_LOG表 | 測試授權成功和失敗兩種情況 |
| IT-004 | 多控制項協作 | 在同一頁面使用多個控制項 | 所有控制項獨立工作 | 使用不同FunctionID和RequiredLevel |
| IT-005 | 與主版面整合 | 在GLA.master中使用控制項 | 正確控制整個頁面訪問 | 在主版面層級測試權限控制 |

### 7.3 性能測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| PT-001 | 載入時間測試 | 測量控制項初始化時間 | <10ms | 正常環境下單用戶測試 |
| PT-002 | 資料庫查詢效能 | 測量權限檢查查詢時間 | <50ms | 正常資料庫連接 |
| PT-003 | 高併發測試 | 模擬100個並發請求 | 穩定運行，無錯誤 | 多線程模擬請求 |
| PT-004 | 記憶體使用測試 | 測量控制項記憶體佔用 | <2MB | 在頁面中嵌入控制項 |
| PT-005 | 權限緩存效能 | 測試緩存機制效能 | 緩存後查詢時間<5ms | 啟用權限緩存 |

## 8. 安全性考量

### 8.1 授權安全

1. **深度防禦**
   - 在UI、控制項、服務和資料層都實施權限檢查
   - 不僅依賴單一控制項，確保多層次安全控制
   - 每次關鍵操作都重新驗證權限

2. **權限分離**
   - 明確區分檢視、編輯和管理權限級別
   - 實施最小權限原則
   - 敏感操作需要更高權限

3. **權限繼承機制**
   - 角色權限和個人權限合理組合
   - 防止權限提升攻擊
   - 管理員權限特殊保護

### 8.2 資料安全

1. **授權日誌**
   - 記錄所有授權請求和結果
   - 包含使用者ID、功能ID、結果和時間戳
   - 日誌資料保護防止篡改

2. **安全告警**
   - 連續失敗的授權嘗試會觸發告警
   - 異常授權模式檢測
   - 管理員通知機制

3. **權限資料安全**
   - 權限資料表存取控制嚴格限制
   - 權限變更審計跟踪
   - 定期權限審查機制

### 8.3 程式安全

1. **異常處理**
   - 所有例外都被捕獲並妥善處理
   - 避免洩露敏感錯誤信息給使用者
   - 安全事故降級處理機制

2. **預設安全**
   - 預設拒絕訪問，除非明確授權
   - 未指定權限時的安全處理
   - 系統維護模式特殊處理

3. **代碼安全**
   - 參數化SQL查詢防注入
   - 輸出編碼防止XSS
   - 定期代碼安全審查

## 9. 效能優化

### 9.1 查詢優化

1. **資料庫優化**
   - USER_AUTH, USER_ROLE, ROLE_AUTH表建立適當索引
   - 權限查詢SQL優化
   - 避免多次資料庫查詢

2. **查詢結構**
   - 權限查詢採用階層式結構
   - 先檢查使用者直接權限，再檢查角色權限
   - 減少資料連接操作

### 9.2 緩存策略

1. **權限緩存**
   - 使用者權限資料緩存在記憶體中
   - 權限變更時自動更新緩存
   - 緩存過期策略：15分鐘或權限變更時

2. **緩存實施**
   - 使用ASP.NET Cache對象存儲權限資料
   - 緩存鍵格式："{UserId}_{FunctionId}_Auth"
   - 開發專用權限緩存管理類別

### 9.3 程式碼優化

1. **初始化優化**
   - 延遲載入不立即需要的資源
   - 減少Page_Load中的處理邏輯
   - 頁面事件最佳處理順序

2. **資源使用**
   - 資料庫連接使用using語句確保釋放
   - 最小化記憶體使用
   - 避免不必要的物件創建

## 10. 維護與擴展

### 10.1 維護指南

1. **常見問題處理**
   - 權限未生效的故障排除步驟
   - 資料庫權限不一致修復方法
   - 緩存問題處理

2. **日誌分析**
   - 授權失敗日誌解讀指南
   - 常見授權問題模式識別
   - 安全威脅識別方法

3. **效能監控**
   - 關鍵性能指標監控
   - 判斷權限檢查是否成為瓶頸
   - 資料庫查詢優化建議

### 10.2 擴展指南

1. **新權限級別添加**
   - 在AuthLevel列舉中添加新值
   - 更新權限檢查邏輯
   - 確保與現有權限兼容

2. **整合第三方身份驗證**
   - 與外部身份提供者整合步驟
   - SAML/OAuth整合方案
   - 單點登錄實施指南

3. **權限模型調整**
   - 從角色基礎存取控制(RBAC)擴展到屬性基礎存取控制(ABAC)
   - 添加動態權限規則
   - 基於組織結構的權限控制

### 10.3 升級路徑

1. **.NET升級考量**
   - 遷移至.NET Core/5+的注意事項
   - API兼容性確保
   - 分階段遷移策略

2. **UI框架升級**
   - 從Web Forms遷移至MVC或Blazor
   - 控制項重構指南
   - 向後兼容性維護方案

## 11. 參考資料

### 11.1 相關文件

1. **設計文件**
   - 系統權限架構設計文件
   - 資料庫權限模型設計
   - 使用者角色層次結構圖

2. **操作手冊**
   - 系統管理員權限設定手冊
   - 開發者整合CheckAuthorization指南
   - 權限審計流程文件

3. **相依元件文件**
   - AppAuthority類別參考文件
   - IBosDB使用手冊
   - Logger類別文件

### 11.2 附件

1. 權限模型ER圖
2. 權限查詢流程圖
3. 授權失敗處理流程圖
4. 權限API使用示例

### 11.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 