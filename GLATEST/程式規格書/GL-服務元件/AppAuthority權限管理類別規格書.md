# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | AppAuthority |
| 程式名稱 | 權限管理類別 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Common/AppAuthority.cs |
| 程式類型 | 公用類別 (Utility Class) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

AppAuthority 是泛太總帳系統的權限管理核心類別，提供完整的使用者授權與權限管理功能。此類別負責實現角色基礎存取控制(RBAC)模型，協調使用者、角色與功能之間的權限關係，確保系統安全與資料保護。

### 2.2 業務流程

AppAuthority在泛太總帳系統中扮演以下角色：
1. 提供使用者權限資訊查詢與驗證
2. 管理角色與使用者的授權關係
3. 提供功能級別的權限檢查
4. 支援多層權限等級控制機制
5. 維護權限變更記錄與稽核追蹤
6. 提供權限資料緩存機制，提升系統效能

### 2.3 使用頻率

- 高頻率：系統中的每次權限檢查都會調用此類別
- 平均每個使用者每天觸發數百次
- 系統核心元件，被多個模組和控制項依賴

### 2.4 使用者角色

此類別主要服務於：
- 系統管理員：通過管理界面設定權限
- 開發人員：通過API檢查用戶權限
- 系統內部：被CheckAuthorization等控制項調用

最終影響所有系統使用者的權限檢查結果。

## 3. 系統架構

### 3.1 技術架構

- 開發語言：C# (.NET Framework 4.0)
- 架構模式：單例模式 (Singleton Pattern)
- 技術依賴：
  - ADO.NET 資料存取
  - System.Data
  - System.Collections.Generic
  - System.Web.Caching

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| USER_AUTH | 使用者直接授權表 | 讀取/寫入 |
| USER_ROLE | 使用者角色關聯表 | 讀取/寫入 |
| ROLE_AUTH | 角色權限表 | 讀取/寫入 |
| SYS_FUNC | 系統功能定義表 | 讀取 |
| SYS_ROLE | 系統角色定義表 | 讀取/寫入 |
| SYS_LOG | 系統日誌表 | 寫入 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |
| Logger | 日誌記錄 | 記錄權限相關事件與錯誤 |
| CacheManager | 快取管理 | 管理權限資料的快取機制 |

## 4. 類別設計

### 4.1 類別結構

AppAuthority 採用單例模式設計，確保整個應用程式中只有一個權限管理實例：

```csharp
public class AppAuthority
{
    private static AppAuthority _instance;
    private static readonly object _lockObject = new object();
    
    // 單例實例取得方法
    public static AppAuthority Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new AppAuthority();
                    }
                }
            }
            return _instance;
        }
    }
    
    // 私有建構函數，防止外部直接建立實例
    private AppAuthority()
    {
        // 初始化權限管理系統
        InitializeAuthSystem();
    }
    
    // 類別其餘實作...
}
```

### 4.2 主要屬性

| 屬性名稱 | 類型 | 存取修飾詞 | 說明 |
|---------|------|----------|------|
| CacheEnabled | bool | public | 是否啟用權限緩存 |
| CacheExpiration | int | public | 緩存過期時間(分鐘) |
| DefaultAuthLevel | AuthLevel | public | 預設權限等級 |
| SystemMaintenanceMode | bool | public | 系統維護模式狀態 |

### 4.3 關鍵列舉與常數

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

/// <summary>
/// 權限緩存常數
/// </summary>
private const string CACHE_KEY_PREFIX = "AUTH_";
private const int DEFAULT_CACHE_MINUTES = 15;
private const string USER_AUTH_CACHE_KEY = "USER_AUTH_";
private const string ROLE_AUTH_CACHE_KEY = "ROLE_AUTH_";
```

## 5. 主要方法

### 5.1 權限檢查方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| CheckFunctionAuth | 檢查使用者對特定功能的權限 | string userId, string funcId, int requiredLevel | bool |
| GetUserAuthLevel | 獲取使用者對功能的權限等級 | string userId, string funcId | AuthLevel |
| HasDirectPermission | 檢查使用者是否有直接權限 | string userId, string funcId, AuthLevel level | bool |
| HasRolePermission | 檢查使用者透過角色是否有權限 | string userId, string funcId, AuthLevel level | bool |
| GetUserRoles | 獲取使用者所屬角色清單 | string userId | List\<string\> |

### 5.2 權限管理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| AssignUserPermission | 授予使用者直接權限 | string userId, string funcId, AuthLevel level | bool |
| RevokeUserPermission | 撤銷使用者直接權限 | string userId, string funcId | bool |
| AssignRolePermission | 授予角色權限 | string roleId, string funcId, AuthLevel level | bool |
| RevokeRolePermission | 撤銷角色權限 | string roleId, string funcId | bool |
| AssignUserRole | 指派使用者到角色 | string userId, string roleId | bool |
| RemoveUserRole | 移除使用者的角色 | string userId, string roleId | bool |

### 5.3 緩存管理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| GetCachedAuthLevel | 從快取取得權限等級 | string userId, string funcId | AuthLevel? |
| SetCachedAuthLevel | 設置權限等級到快取 | string userId, string funcId, AuthLevel level | void |
| ClearUserCache | 清除特定使用者的權限快取 | string userId | void |
| ClearFunctionCache | 清除特定功能的權限快取 | string funcId | void |
| ClearAllCache | 清除所有權限快取 | 無 | void |

### 5.4 系統管理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| InitializeAuthSystem | 初始化權限系統 | 無 | void |
| IsSystemMaintenance | 檢查系統是否處於維護模式 | 無 | bool |
| SetSystemMaintenance | 設置系統維護模式 | bool value | void |
| GetAllFunctions | 取得所有系統功能 | 無 | DataTable |
| GetAllRoles | 取得所有系統角色 | 無 | DataTable |

## 6. 處理邏輯

### 6.1 權限檢查流程

```
開始
 ↓
檢查系統維護模式 → 維護模式且非管理員 → 返回無權限 → 結束
 ↓
檢查參數有效性 → 參數無效 → 記錄錯誤並返回無權限 → 結束
 ↓
從緩存中檢查權限(如啟用緩存) → 緩存中有權限資料 → 比較權限等級並返回結果 → 結束
 ↓
檢查用戶直接權限 → 有直接權限 → 比較權限等級、更新緩存並返回結果 → 結束
 ↓
獲取用戶角色列表
 ↓
循環檢查各角色權限 → 任一角色有足夠權限 → 更新緩存並返回成功 → 結束
 ↓
檢查功能的預設權限 → 預設權限足夠 → 更新緩存並返回成功 → 結束
 ↓
更新緩存並返回無權限
 ↓
結束
```

### 6.2 權限授予流程

```
開始
 ↓
檢查操作者權限 → 操作者無足夠權限 → 記錄未授權操作並返回失敗 → 結束
 ↓
檢查參數有效性 → 參數無效 → 記錄錯誤並返回失敗 → 結束
 ↓
檢查現有權限 → 權限已存在且相同 → 返回成功 → 結束
 ↓
更新資料庫中的權限設定
 ↓
清除相關緩存
 ↓
記錄權限變更日誌
 ↓
返回操作結果
 ↓
結束
```

### 6.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 資料庫連接失敗 | 記錄錯誤並嘗試使用緩存 | 如有緩存則正常使用，否則顯示連接錯誤 |
| 權限資料不一致 | 記錄錯誤並使用保守策略 | 採用最小權限原則，確保安全 |
| 緩存失效 | 重新從資料庫載入權限 | 輕微性能下降，但功能正常 |
| 參數無效 | 記錄警告並返回無權限 | 確保安全，顯示權限不足訊息 |

## 7. 程式碼說明

### 7.1 權限檢查實現

```csharp
/// <summary>
/// 檢查使用者對特定功能的權限
/// </summary>
/// <param name="userId">使用者ID</param>
/// <param name="funcId">功能ID</param>
/// <param name="requiredLevel">所需權限等級</param>
/// <returns>是否有足夠權限</returns>
public bool CheckFunctionAuth(string userId, string funcId, int requiredLevel)
{
    try
    {
        // 檢查參數
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(funcId))
        {
            Logger.Warn("AppAuthority.CheckFunctionAuth: Invalid parameters");
            return false;
        }
        
        // 系統管理員檢查
        if (IsSystemAdmin(userId))
        {
            return true;
        }
        
        // 系統維護模式檢查
        if (SystemMaintenanceMode && !IsMaintenanceStaff(userId))
        {
            Logger.Info($"AppAuthority.CheckFunctionAuth: System in maintenance mode, access denied for user {userId}");
            return false;
        }
        
        // 獲取使用者權限等級
        AuthLevel userLevel = GetUserAuthLevel(userId, funcId);
        
        // 檢查權限等級是否足夠
        return (int)userLevel >= requiredLevel;
    }
    catch (Exception ex)
    {
        Logger.Error("AppAuthority.CheckFunctionAuth Error", ex);
        return false;
    }
}
```

### 7.2 獲取使用者權限等級

```csharp
/// <summary>
/// 獲取使用者對功能的權限等級
/// </summary>
/// <param name="userId">使用者ID</param>
/// <param name="funcId">功能ID</param>
/// <returns>權限等級</returns>
public AuthLevel GetUserAuthLevel(string userId, string funcId)
{
    try
    {
        // 檢查參數
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(funcId))
        {
            return AuthLevel.None;
        }
        
        // 檢查緩存
        if (CacheEnabled)
        {
            AuthLevel? cachedLevel = GetCachedAuthLevel(userId, funcId);
            if (cachedLevel.HasValue)
            {
                return cachedLevel.Value;
            }
        }
        
        AuthLevel resultLevel = AuthLevel.None;
        
        // 檢查直接權限
        AuthLevel directLevel = GetUserDirectAuthLevel(userId, funcId);
        resultLevel = directLevel;
        
        // 如果沒有直接權限，檢查角色權限
        if (resultLevel == AuthLevel.None)
        {
            AuthLevel roleLevel = GetUserRoleAuthLevel(userId, funcId);
            resultLevel = roleLevel;
        }
        
        // 如果仍然沒有權限，檢查功能的預設權限
        if (resultLevel == AuthLevel.None)
        {
            AuthLevel defaultLevel = GetFunctionDefaultAuthLevel(funcId);
            resultLevel = defaultLevel;
        }
        
        // 更新緩存
        if (CacheEnabled)
        {
            SetCachedAuthLevel(userId, funcId, resultLevel);
        }
        
        return resultLevel;
    }
    catch (Exception ex)
    {
        Logger.Error("AppAuthority.GetUserAuthLevel Error", ex);
        return AuthLevel.None;
    }
}
```

### 7.3 權限緩存實現

```csharp
/// <summary>
/// 從快取取得權限等級
/// </summary>
/// <param name="userId">使用者ID</param>
/// <param name="funcId">功能ID</param>
/// <returns>權限等級，若無則返回null</returns>
private AuthLevel? GetCachedAuthLevel(string userId, string funcId)
{
    string cacheKey = $"{USER_AUTH_CACHE_KEY}{userId}_{funcId}";
    
    try
    {
        object cachedValue = HttpRuntime.Cache[cacheKey];
        if (cachedValue != null)
        {
            return (AuthLevel)cachedValue;
        }
        
        return null;
    }
    catch
    {
        return null;
    }
}

/// <summary>
/// 設置權限等級到快取
/// </summary>
/// <param name="userId">使用者ID</param>
/// <param name="funcId">功能ID</param>
/// <param name="level">權限等級</param>
private void SetCachedAuthLevel(string userId, string funcId, AuthLevel level)
{
    string cacheKey = $"{USER_AUTH_CACHE_KEY}{userId}_{funcId}";
    
    try
    {
        // 添加或更新緩存
        HttpRuntime.Cache.Insert(
            cacheKey,
            level,
            null,
            DateTime.Now.AddMinutes(CacheExpiration),
            System.Web.Caching.Cache.NoSlidingExpiration,
            System.Web.Caching.CacheItemPriority.Normal,
            null);
    }
    catch (Exception ex)
    {
        Logger.Error("AppAuthority.SetCachedAuthLevel Error", ex);
    }
}

/// <summary>
/// 清除特定使用者的權限快取
/// </summary>
/// <param name="userId">使用者ID</param>
public void ClearUserCache(string userId)
{
    try
    {
        // 獲取所有緩存鍵
        List<string> keysToRemove = new List<string>();
        IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
        
        while (cacheEnum.MoveNext())
        {
            string key = cacheEnum.Key.ToString();
            if (key.StartsWith($"{USER_AUTH_CACHE_KEY}{userId}"))
            {
                keysToRemove.Add(key);
            }
        }
        
        // 刪除匹配的緩存項
        foreach (string key in keysToRemove)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
    catch (Exception ex)
    {
        Logger.Error("AppAuthority.ClearUserCache Error", ex);
    }
}
```

### 7.4 權限授予實現

```csharp
/// <summary>
/// 授予使用者直接權限
/// </summary>
/// <param name="userId">使用者ID</param>
/// <param name="funcId">功能ID</param>
/// <param name="level">權限等級</param>
/// <returns>是否成功</returns>
public bool AssignUserPermission(string userId, string funcId, AuthLevel level)
{
    try
    {
        // 檢查參數
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(funcId))
        {
            Logger.Warn("AppAuthority.AssignUserPermission: Invalid parameters");
            return false;
        }
        
        // 檢查當前權限
        AuthLevel currentLevel = GetUserDirectAuthLevel(userId, funcId);
        
        // 如果已有相同權限，直接返回成功
        if (currentLevel == level)
        {
            return true;
        }
        
        using (IBosDB db = GetBOSDB())
        {
            // 準備SQL和參數
            string sql;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@USER_ID", userId);
            parameters.Add("@FUNC_ID", funcId);
            parameters.Add("@AUTH_LEVEL", (int)level);
            parameters.Add("@UPDATE_USER", HttpContext.Current.User.Identity.Name);
            parameters.Add("@UPDATE_TIME", DateTime.Now);
            
            // 如果已存在記錄則更新，否則插入
            if (currentLevel != AuthLevel.None)
            {
                sql = @"
                    UPDATE USER_AUTH 
                    SET AUTH_LEVEL = @AUTH_LEVEL,
                        UPDATE_USER = @UPDATE_USER,
                        UPDATE_TIME = @UPDATE_TIME
                    WHERE USER_ID = @USER_ID 
                      AND FUNC_ID = @FUNC_ID";
            }
            else
            {
                sql = @"
                    INSERT INTO USER_AUTH 
                    (USER_ID, FUNC_ID, AUTH_LEVEL, CREATE_USER, CREATE_TIME, UPDATE_USER, UPDATE_TIME)
                    VALUES 
                    (@USER_ID, @FUNC_ID, @AUTH_LEVEL, @UPDATE_USER, @UPDATE_TIME, @UPDATE_USER, @UPDATE_TIME)";
            }
            
            // 執行SQL
            int result = db.ExecuteNonQuery(sql, parameters);
            
            // 清除緩存
            if (result > 0 && CacheEnabled)
            {
                ClearUserCache(userId);
                ClearFunctionCache(funcId);
            }
            
            // 記錄權限變更
            LogPermissionChange(userId, funcId, currentLevel, level);
            
            return result > 0;
        }
    }
    catch (Exception ex)
    {
        Logger.Error("AppAuthority.AssignUserPermission Error", ex);
        return false;
    }
}
```

## 8. 測試案例

### 8.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 檢查使用者直接權限 | 測試HasDirectPermission方法 | 返回正確權限狀態 | 分別測試有權限和無權限情況 |
| UT-002 | 檢查角色權限 | 測試HasRolePermission方法 | 返回正確權限狀態 | 測試多角色權限繼承情況 |
| UT-003 | 權限等級比較 | 測試CheckFunctionAuth方法 | 正確比較權限等級 | 測試不同等級權限組合 |
| UT-004 | 系統管理員權限 | 測試管理員權限檢查 | 管理員具有所有權限 | 使用管理員帳號測試 |
| UT-005 | 權限緩存功能 | 測試緩存機制 | 正確緩存權限並過期 | 驗證緩存命中率和更新 |

### 8.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 與CheckAuthorization整合 | 測試控制項調用AppAuthority | 權限檢查結果一致 | 模擬網頁請求場景 |
| IT-002 | 資料庫整合 | 測試資料庫操作 | 正確讀寫權限數據 | 測試資料庫連接與操作 |
| IT-003 | 緩存系統整合 | 測試ASP.NET緩存整合 | 緩存機制正常工作 | 測試緩存效能與更新 |
| IT-004 | 多用戶並發測試 | 模擬並發權限檢查 | 處理並發請求無錯誤 | 模擬高負載場景 |
| IT-005 | 權限變更傳播 | 測試權限變更後的系統行為 | 權限變更正確傳播 | 測試緩存失效與更新 |

### 8.3 效能測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| PT-001 | 權限檢查效能 | 測量CheckFunctionAuth方法效能 | <10ms | 正常場景，有緩存 |
| PT-002 | 無緩存權限檢查 | 測量無緩存權限檢查時間 | <50ms | 關閉緩存測試 |
| PT-003 | 緩存命中率 | 分析緩存命中和未命中情況 | >95%命中率 | 標準用戶操作模式 |
| PT-004 | 高負載測試 | 每秒1000次權限檢查 | 穩定處理，響應時間<20ms | 模擬高峰期使用 |
| PT-005 | 記憶體使用 | 測量大量權限緩存的記憶體使用 | <50MB | 100個用戶，1000個功能權限 |

## 9. 安全性考量

### 9.1 權限模型安全

1. **最小權限原則**
   - 預設拒絕所有權限
   - 明確定義每個功能的最小所需權限
   - 權限檢查失敗時默認拒絕訪問

2. **權限隔離**
   - 用戶直接權限與角色權限分離
   - 特殊權限需要明確授予
   - 敏感功能需要更高權限級別

3. **角色階層**
   - 支援角色繼承關係
   - 層級化的權限結構
   - 權限沿角色層級傳遞

### 9.2 授權安全

1. **權限變更控制**
   - 權限變更需要管理權限
   - 變更記錄完整的審計日誌
   - 權限變更需二次認證

2. **權限自保護**
   - 權限管理功能受嚴格保護
   - 系統管理員權限不可通過常規介面撤銷
   - 防止權限提升攻擊

3. **系統維護模式**
   - 維護模式下限制一般用戶訪問
   - 只允許維護人員操作
   - 維護完成後自動解除限制

### 9.3 系統安全

1. **代碼安全**
   - 防止SQL注入
   - 安全的異常處理
   - 參數驗證與過濾

2. **緩存安全**
   - 敏感權限資料保護
   - 緩存密鑰隨機化
   - 緩存過期策略

3. **資料庫安全**
   - 使用參數化查詢
   - 最小資料庫權限原則
   - 資料庫連接保護

## 10. 效能優化

### 10.1 查詢優化

1. **權限查詢優化**
   - 優化SQL查詢
   - 按階層進行權限檢查，減少不必要的查詢
   - 使用索引優化USER_AUTH和ROLE_AUTH表

2. **批次處理**
   - 支援批次權限檢查
   - 減少資料庫連接開銷
   - 一次性載入用戶所有角色

3. **預載入**
   - 預先載入常用功能權限
   - 載入完整的用戶授權樹
   - 減少重複查詢

### 10.2 緩存策略

1. **多級緩存**
   - 用戶權限緩存
   - 角色權限緩存
   - 功能預設權限緩存

2. **緩存失效策略**
   - 主動失效：權限變更時立即清除相關緩存
   - 被動失效：設定合理的過期時間
   - 分層失效：用戶、角色、功能層級的緩存獨立失效

3. **緩存預熱**
   - 系統啟動時預熱常用權限緩存
   - 用戶登入時預載入基本權限
   - 優化冷啟動效能

### 10.3 程式碼優化

1. **類別設計**
   - 使用單例模式減少實例化開銷
   - 惰性載入減少啟動時間
   - 優化物件池管理

2. **資源管理**
   - 減少資料庫連接使用時間
   - 正確釋放資源
   - 優化記憶體使用

3. **非同步處理**
   - 權限日誌非同步記錄
   - 緩存更新非同步處理
   - 減少權限檢查阻塞時間

## 11. 維護與擴展

### 11.1 維護指南

1. **監控與診斷**
   - 緩存命中率監控
   - 權限檢查效能統計
   - 異常權限檢查警告

2. **權限管理**
   - 定期審查權限設置
   - 不使用的權限清理
   - 權限一致性檢查

3. **故障排除**
   - 常見權限問題排查步驟
   - 緩存問題診斷
   - 權限同步失敗處理

### 11.2 擴展指南

1. **功能擴展**
   - 添加新權限等級方法
   - 整合外部驗證系統
   - 增強權限繼承模型

2. **擴展角色模型**
   - 實施更細粒度的權限控制
   - 添加多維度權限評估
   - 支援基於組織結構的權限

3. **安全增強**
   - 添加行為分析防護
   - 實施權限使用審計
   - 強化反特權濫用機制

### 11.3 版本升級計劃

1. **下一版本計劃**
   - 實施屬性基礎存取控制(ABAC)
   - 改進權限決策引擎
   - 整合進階威脅檢測

2. **遷移考量**
   - 向.NET Core / .NET 5+遷移
   - 保持API兼容性
   - 確保權限資料正確遷移

## 12. 參考資料

### 12.1 相關文件

1. **設計文件**
   - 系統整體安全架構設計
   - 權限模型詳細設計
   - 資料庫權限模型

2. **操作手冊**
   - 管理員權限設置指南
   - 開發人員權限API使用指南
   - 權限設計最佳實踐

3. **技術參考**
   - RBAC模型實施指南
   - .NET緩存最佳實踐
   - SQL權限查詢優化

### 12.2 附件

1. 權限資料表結構圖
2. 權限檢查流程圖
3. 角色權限繼承關係圖
4. 效能測試報告

### 12.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 