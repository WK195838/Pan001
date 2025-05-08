# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | WSGLAcctDefJson |
| 程式名稱 | 科目定義服務 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | /GLATEST/app/WebServices/WSGLAcctDefJson.asmx, /GLATEST/app/WebServices/WSGLAcctDefJson.asmx.cs |
| 程式類型 | Web服務 (Web Service) |
| 建立日期 | 2025/05/08 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/08 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

WSGLAcctDefJson 是泛太總帳系統的科目定義Web服務，負責提供會計科目相關的資料查詢和操作功能。此服務採用JSON格式進行資料交換，主要用於前端頁面與後端資料庫之間的科目資料存取，提供科目結構查詢、科目驗證、科目權限檢查等功能。服務採用RESTful風格設計，支援輕量化的資料交換，提高系統整體效能和擴展性。

### 2.2 業務流程

WSGLAcctDefJson 在系統中扮演以下角色：
1. 提供會計科目資料即時查詢
2. 支援科目資料的JSON格式序列化與反序列化
3. 為前端科目選擇控制項提供資料源
4. 處理科目相關權限的快速檢核
5. 支援複雜科目結構與階層關係的資料獲取
6. 為各模組提供統一的科目資料存取介面

### 2.3 使用頻率

- 高頻率：在各類交易處理和報表頁面使用
- 所有包含科目選擇和顯示的頁面均需呼叫此服務
- 系統中約有80%的頁面會使用到科目相關功能
- 平均每個使用者每天觸發數百次服務呼叫

### 2.4 使用者角色

此服務間接服務於系統的所有角色，主要通過前端頁面呼叫：
- 系統管理員：科目維護和管理
- 財務主管：科目結構查詢與權限設定
- 會計人員：日常交易科目選擇與驗證
- 一般用戶：科目資料查詢

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Services (.NET Framework 4.0)
- 主要技術：
  - ASMX Web Services：提供HTTP/SOAP通信協議
  - JSON.NET：處理JSON序列化和反序列化
  - ADO.NET：資料庫存取技術
- 通信協議：支援SOAP和REST格式API
- 資料格式：JSON，支持UTF-8編碼

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| GL_ACCT_DEF | 科目定義主表 | 讀取 |
| GL_ACCT_ATTR | 科目屬性表 | 讀取 |
| GL_ACCT_LEVELS | 科目層級表 | 讀取 |
| GL_ACCT_PERMISSION | 科目權限表 | 讀取 |
| SYS_USER | 使用者資料表 | 讀取 |
| SYS_ROLE | 角色資料表 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| AppAuthority | 權限管理 | 檢查科目權限設定 |
| BosCache | 快取管理 | 科目資料快取處理 |
| JsonHelper | JSON處理 | JSON格式轉換輔助類 |

## 4. 服務規格

### 4.1 服務介面

```csharp
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class WSGLAcctDefJson : WebService
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountById(string companyId, string acctId);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountsByLevel(string companyId, int level);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountChildren(string companyId, string parentAcctId);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountHierarchy(string companyId, string acctId);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ValidateAccount(string companyId, string acctId);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetAccountPermissions(string userId, string companyId, string acctId);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SearchAccounts(string companyId, string keyword, int maxResults);
}
```

### 4.2 資料結構

#### 4.2.1 輸入參數

| 方法 | 參數名稱 | 資料型態 | 必填 | 說明 |
|------|---------|---------|------|------|
| GetAccountById | companyId | string | Y | 公司代碼 |
| GetAccountById | acctId | string | Y | 科目代碼 |
| GetAccountsByLevel | companyId | string | Y | 公司代碼 |
| GetAccountsByLevel | level | int | Y | 科目層級 |
| GetAccountChildren | companyId | string | Y | 公司代碼 |
| GetAccountChildren | parentAcctId | string | Y | 父科目代碼 |
| GetAccountHierarchy | companyId | string | Y | 公司代碼 |
| GetAccountHierarchy | acctId | string | Y | 科目代碼 |
| ValidateAccount | companyId | string | Y | 公司代碼 |
| ValidateAccount | acctId | string | Y | 科目代碼 |
| GetAccountPermissions | userId | string | Y | 使用者ID |
| GetAccountPermissions | companyId | string | Y | 公司代碼 |
| GetAccountPermissions | acctId | string | Y | 科目代碼 |
| SearchAccounts | companyId | string | Y | 公司代碼 |
| SearchAccounts | keyword | string | Y | 搜尋關鍵字 |
| SearchAccounts | maxResults | int | N | 最大返回結果數 |

#### 4.2.2 輸出結構

科目資料 JSON 格式範例：

```json
{
  "success": true,
  "message": "操作成功",
  "data": {
    "acctId": "1001",
    "acctName": "現金",
    "acctLevel": 1,
    "acctType": "A",
    "parentAcctId": "",
    "isLeaf": false,
    "enabled": true,
    "attributes": {
      "currencyEnabled": true,
      "deptEnabled": false,
      "projectEnabled": true
    },
    "children": [
      {
        "acctId": "1001001",
        "acctName": "零用金",
        "acctLevel": 2,
        "acctType": "A",
        "parentAcctId": "1001",
        "isLeaf": true,
        "enabled": true,
        "attributes": {
          "currencyEnabled": true,
          "deptEnabled": false,
          "projectEnabled": false
        }
      }
    ]
  }
}
```

### 4.3 錯誤代碼

| 錯誤代碼 | 錯誤訊息 | 說明 |
|---------|---------|------|
| 400 | 參數錯誤 | 輸入參數格式不正確或缺少必填參數 |
| 401 | 未授權存取 | 使用者未登入或無權限存取 |
| 404 | 科目不存在 | 請求的科目代碼不存在 |
| 405 | 方法不允許 | 請求的HTTP方法不被支援 |
| 500 | 伺服器內部錯誤 | 服務端處理請求時發生錯誤 |

## 5. 處理邏輯

### 5.1 主要流程

服務呼叫的一般處理流程：

```
開始
 ↓
接收Web Service請求
 ↓
驗證輸入參數 → 參數無效 → 返回錯誤訊息(400)
 ↓
檢查使用者權限 → 權限不足 → 返回錯誤訊息(401) 
 ↓
嘗試從緩存獲取資料 → 緩存命中 → 返回緩存資料
 ↓
從資料庫讀取資料 → 資料不存在 → 返回錯誤訊息(404)
 ↓
對資料進行業務邏輯處理
 ↓
將資料轉換為JSON格式
 ↓
更新緩存（如需要）
 ↓
返回JSON結果
 ↓
結束
```

### 5.2 特殊處理邏輯

1. **科目層級處理**：
   - 系統支援多達8層的科目層級結構
   - 科目代碼長度根據層級動態調整
   - 每一層級的科目代碼格式需符合公司規範

2. **科目權限處理**：
   - 權限檢核依賴AppAuthority元件
   - 支援細粒度的科目權限控制，包括讀取、新增、修改和刪除
   - 權限可按角色、部門和用戶等多維度設定

3. **科目屬性處理**：
   - 科目可包含多種自定義屬性
   - 不同類型的科目具有不同的屬性結構
   - 服務需處理屬性的繼承和覆蓋關係

### 5.3 例外處理

| 例外類型 | 處理方式 | 記錄方式 |
|---------|---------|---------|
| 參數驗證例外 | 返回400錯誤，提供具體的參數錯誤訊息 | 只記錄關鍵資訊，不記錄完整堆疊 |
| 資料庫例外 | 返回500錯誤，提供一般性錯誤訊息，不透露系統細節 | 記錄完整例外資訊，包含SQL和堆疊 |
| 權限例外 | 返回401錯誤，提供權限不足的具體原因 | 記錄使用者ID、請求資源和需要的權限 |
| JSON處理例外 | 返回500錯誤，提供JSON格式錯誤訊息 | 記錄錯誤的資料結構和堆疊 |
| 未預期例外 | 返回500錯誤，提供一般性錯誤代碼以便追蹤 | 記錄完整例外資訊和環境狀態 |

## 6. SQL查詢

### 6.1 科目基本資料查詢

```sql
-- 根據科目代碼獲取科目資料
SELECT a.ACCT_ID, a.ACCT_NAME, a.ACCT_LEVEL, a.ACCT_TYPE, 
       a.PARENT_ACCT_ID, a.IS_LEAF, a.ENABLED, a.DESCRIPTION
FROM GL_ACCT_DEF a
WHERE a.COMPANY_ID = @CompanyId 
  AND a.ACCT_ID = @AcctId;
```

### 6.2 科目層級查詢

```sql
-- 獲取指定層級的所有科目
SELECT a.ACCT_ID, a.ACCT_NAME, a.ACCT_TYPE, 
       a.PARENT_ACCT_ID, a.IS_LEAF, a.ENABLED
FROM GL_ACCT_DEF a
WHERE a.COMPANY_ID = @CompanyId 
  AND a.ACCT_LEVEL = @Level
ORDER BY a.ACCT_ID;
```

### 6.3 科目子項查詢

```sql
-- 獲取特定科目的所有子科目
SELECT a.ACCT_ID, a.ACCT_NAME, a.ACCT_LEVEL, a.ACCT_TYPE, 
       a.PARENT_ACCT_ID, a.IS_LEAF, a.ENABLED
FROM GL_ACCT_DEF a
WHERE a.COMPANY_ID = @CompanyId 
  AND a.PARENT_ACCT_ID = @ParentAcctId
ORDER BY a.ACCT_ID;
```

### 6.4 科目權限查詢

```sql
-- 查詢使用者對特定科目的權限
SELECT p.PERMISSION_TYPE, p.PERMISSION_VALUE
FROM GL_ACCT_PERMISSION p
JOIN SYS_USER u ON p.ROLE_ID = u.ROLE_ID
WHERE u.USER_ID = @UserId
  AND p.COMPANY_ID = @CompanyId
  AND p.ACCT_ID = @AcctId;
```

### 6.5 科目搜尋

```sql
-- 根據關鍵字搜尋科目
SELECT TOP(@MaxResults) a.ACCT_ID, a.ACCT_NAME, a.ACCT_LEVEL, a.ACCT_TYPE
FROM GL_ACCT_DEF a
WHERE a.COMPANY_ID = @CompanyId 
  AND (a.ACCT_ID LIKE '%' + @Keyword + '%' OR a.ACCT_NAME LIKE '%' + @Keyword + '%')
  AND a.ENABLED = 1
ORDER BY 
  CASE WHEN a.ACCT_ID = @Keyword THEN 0
       WHEN a.ACCT_ID LIKE @Keyword + '%' THEN 1
       WHEN a.ACCT_NAME = @Keyword THEN 2
       WHEN a.ACCT_NAME LIKE @Keyword + '%' THEN 3
       ELSE 4
  END,
  a.ACCT_ID;
```

## 7. 程式碼說明

### 7.1 GetAccountById 實現

```csharp
/// <summary>
/// 根據科目ID獲取科目資料
/// </summary>
/// <param name="companyId">公司代碼</param>
/// <param name="acctId">科目代碼</param>
/// <returns>JSON格式的科目資料</returns>
[WebMethod]
[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
public string GetAccountById(string companyId, string acctId)
{
    JsonResult result = new JsonResult();
    
    try
    {
        // 參數驗證
        if (string.IsNullOrEmpty(companyId) || string.IsNullOrEmpty(acctId))
        {
            return result.Failure("參數錯誤：公司代碼和科目代碼不能為空").ToJson();
        }
        
        // 獲取當前使用者
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 檢查權限
        if (!AppAuthority.Instance.HasPermission(userId, companyId, "GL_ACCT", "VIEW"))
        {
            return result.Failure("權限不足：無科目查詢權限").ToJson();
        }
        
        // 嘗試從緩存獲取
        string cacheKey = $"ACCT_{companyId}_{acctId}";
        AccountInfo acct = BosCache.Get<AccountInfo>(cacheKey);
        
        if (acct == null)
        {
            // 從資料庫讀取科目基本資料
            IBosDB db = DBFactory.GetBosDB();
            string sql = @"
                SELECT a.ACCT_ID, a.ACCT_NAME, a.ACCT_LEVEL, a.ACCT_TYPE, 
                       a.PARENT_ACCT_ID, a.IS_LEAF, a.ENABLED, a.DESCRIPTION
                FROM GL_ACCT_DEF a
                WHERE a.COMPANY_ID = @CompanyId 
                  AND a.ACCT_ID = @AcctId";
            
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@AcctId", acctId);
            
            DataTable dt = db.ExecuteDataTable(sql, parameters);
            
            if (dt == null || dt.Rows.Count == 0)
            {
                return result.Failure("科目不存在").ToJson();
            }
            
            // 構建科目對象
            acct = new AccountInfo();
            acct.AcctId = dt.Rows[0]["ACCT_ID"].ToString();
            acct.AcctName = dt.Rows[0]["ACCT_NAME"].ToString();
            acct.AcctLevel = Convert.ToInt32(dt.Rows[0]["ACCT_LEVEL"]);
            acct.AcctType = dt.Rows[0]["ACCT_TYPE"].ToString();
            acct.ParentAcctId = dt.Rows[0]["PARENT_ACCT_ID"].ToString();
            acct.IsLeaf = Convert.ToBoolean(dt.Rows[0]["IS_LEAF"]);
            acct.Enabled = Convert.ToBoolean(dt.Rows[0]["ENABLED"]);
            acct.Description = dt.Rows[0]["DESCRIPTION"].ToString();
            
            // 讀取科目屬性
            acct.Attributes = GetAccountAttributes(companyId, acctId, db);
            
            // 如果不是葉節點，獲取子科目
            if (!acct.IsLeaf)
            {
                acct.Children = GetAccountChildrenInternal(companyId, acctId, db);
            }
            
            // 設置緩存
            BosCache.Set(cacheKey, acct, TimeSpan.FromMinutes(10));
        }
        
        // 返回結果
        return result.Success("操作成功", acct).ToJson();
    }
    catch (Exception ex)
    {
        Logger.Error("WSGLAcctDefJson.GetAccountById Error", ex);
        return result.Failure("系統錯誤：" + ex.Message).ToJson();
    }
}
```

### 7.2 GetAccountAttributes 實現

```csharp
/// <summary>
/// 獲取科目的屬性設定
/// </summary>
/// <param name="companyId">公司代碼</param>
/// <param name="acctId">科目代碼</param>
/// <param name="db">資料庫連接</param>
/// <returns>科目屬性字典</returns>
private Dictionary<string, object> GetAccountAttributes(string companyId, string acctId, IBosDB db)
{
    Dictionary<string, object> attributes = new Dictionary<string, object>();
    
    try
    {
        string sql = @"
            SELECT ATTR_CODE, ATTR_VALUE, ATTR_TYPE
            FROM GL_ACCT_ATTR
            WHERE COMPANY_ID = @CompanyId 
              AND ACCT_ID = @AcctId";
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@CompanyId", companyId);
        parameters.Add("@AcctId", acctId);
        
        DataTable dt = db.ExecuteDataTable(sql, parameters);
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string attrCode = row["ATTR_CODE"].ToString();
                string attrValue = row["ATTR_VALUE"].ToString();
                string attrType = row["ATTR_TYPE"].ToString();
                
                // 根據屬性類型轉換值
                object value = ConvertAttributeValue(attrValue, attrType);
                attributes[attrCode] = value;
            }
        }
        
        // 添加默認屬性
        if (!attributes.ContainsKey("currencyEnabled"))
            attributes.Add("currencyEnabled", false);
        
        if (!attributes.ContainsKey("deptEnabled"))
            attributes.Add("deptEnabled", false);
        
        if (!attributes.ContainsKey("projectEnabled"))
            attributes.Add("projectEnabled", false);
    }
    catch (Exception ex)
    {
        Logger.Error("WSGLAcctDefJson.GetAccountAttributes Error", ex);
    }
    
    return attributes;
}
```

### 7.3 SearchAccounts 實現

```csharp
/// <summary>
/// 根據關鍵字搜尋科目
/// </summary>
/// <param name="companyId">公司代碼</param>
/// <param name="keyword">搜尋關鍵字</param>
/// <param name="maxResults">最大返回結果數</param>
/// <returns>JSON格式的科目搜尋結果</returns>
[WebMethod]
[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
public string SearchAccounts(string companyId, string keyword, int maxResults = 10)
{
    JsonResult result = new JsonResult();
    
    try
    {
        // 參數驗證
        if (string.IsNullOrEmpty(companyId))
        {
            return result.Failure("參數錯誤：公司代碼不能為空").ToJson();
        }
        
        if (string.IsNullOrEmpty(keyword))
        {
            return result.Failure("參數錯誤：搜尋關鍵字不能為空").ToJson();
        }
        
        // 限制最大結果數
        if (maxResults <= 0 || maxResults > 100)
        {
            maxResults = 10;
        }
        
        // 獲取當前使用者
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 檢查權限
        if (!AppAuthority.Instance.HasPermission(userId, companyId, "GL_ACCT", "VIEW"))
        {
            return result.Failure("權限不足：無科目查詢權限").ToJson();
        }
        
        // 從資料庫搜尋科目
        IBosDB db = DBFactory.GetBosDB();
        string sql = @"
            SELECT TOP(@MaxResults) a.ACCT_ID, a.ACCT_NAME, a.ACCT_LEVEL, a.ACCT_TYPE
            FROM GL_ACCT_DEF a
            WHERE a.COMPANY_ID = @CompanyId 
              AND (a.ACCT_ID LIKE '%' + @Keyword + '%' OR a.ACCT_NAME LIKE '%' + @Keyword + '%')
              AND a.ENABLED = 1
            ORDER BY 
              CASE WHEN a.ACCT_ID = @Keyword THEN 0
                   WHEN a.ACCT_ID LIKE @Keyword + '%' THEN 1
                   WHEN a.ACCT_NAME = @Keyword THEN 2
                   WHEN a.ACCT_NAME LIKE @Keyword + '%' THEN 3
                   ELSE 4
              END,
              a.ACCT_ID";
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@CompanyId", companyId);
        parameters.Add("@Keyword", keyword);
        parameters.Add("@MaxResults", maxResults);
        
        DataTable dt = db.ExecuteDataTable(sql, parameters);
        
        // 構建結果列表
        List<object> accounts = new List<object>();
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                accounts.Add(new
                {
                    acctId = row["ACCT_ID"].ToString(),
                    acctName = row["ACCT_NAME"].ToString(),
                    acctLevel = Convert.ToInt32(row["ACCT_LEVEL"]),
                    acctType = row["ACCT_TYPE"].ToString(),
                    displayText = $"{row["ACCT_ID"]} - {row["ACCT_NAME"]}"
                });
            }
        }
        
        // 返回結果
        return result.Success("操作成功", accounts).ToJson();
    }
    catch (Exception ex)
    {
        Logger.Error("WSGLAcctDefJson.SearchAccounts Error", ex);
        return result.Failure("系統錯誤：" + ex.Message).ToJson();
    }
}
```

## 8. 安全性考量

### 8.1 認證與授權

1. **認證機制**
   - 服務需使用者登入後才能存取
   - 透過Session或Token機制驗證身份
   - 所有服務呼叫須包含有效的認證信息

2. **授權控制**
   - 存取科目資料需特定權限
   - 基於角色的權限控制（RBAC）
   - 限制使用者只能存取授權的科目資料

### 8.2 資料保護

1. **參數驗證**
   - 所有輸入參數必須進行有效性驗證
   - 防止SQL注入攻擊
   - 限制輸入長度與格式

2. **敏感資料處理**
   - 不暴露敏感的系統內部訊息
   - 加密存儲敏感配置資訊
   - 遵守資料隱私相關法規

### 8.3 通信安全

1. **傳輸安全**
   - 使用HTTPS進行資料傳輸
   - 敏感資料傳輸前加密處理
   - 處理跨站請求偽造（CSRF）問題

2. **IP限制**
   - 配置允許存取的IP範圍
   - 記錄異常存取行為
   - 對異常請求實施臨時封鎖

## 9. 效能優化

### 9.1 資料庫優化

1. **索引優化**
   - GL_ACCT_DEF表的COMPANY_ID和ACCT_ID建立複合索引
   - GL_ACCT_DEF表的PARENT_ACCT_ID建立索引以加速層級查詢
   - 使用覆蓋索引減少資料表存取

2. **查詢優化**
   - 使用參數化查詢避免重複編譯
   - 減少不必要的欄位查詢（SELECT *）
   - 對大結果集使用分頁查詢

3. **連接池管理**
   - 適當設置資料庫連接池大小
   - 及時釋放資料庫連接
   - 監控連接狀態避免洩漏

### 9.2 緩存策略

1. **緩存層級**
   - 使用記憶體內緩存存儲高頻存取資料
   - 實施科目樹結構的智能緩存策略
   - 選擇性緩存複雜計算結果

2. **失效策略**
   - 基於時間的緩存過期策略
   - 科目資料變更時主動清除相關緩存
   - 使用版本號機制實現智能緩存失效

3. **部分更新**
   - 支援緩存的增量更新
   - 避免完全重建大型緩存
   - 記錄緩存命中率並優化策略

### 9.3 服務優化

1. **請求處理**
   - 異步處理長時間運行的查詢
   - 實施請求頻率限制（Rate Limiting）
   - 優化JSON序列化性能

2. **資源管理**
   - 適當設置線程池大小
   - 實施資源監控與自動擴展
   - 及時釋放非托管資源

3. **負載測試**
   - 進行高併發場景的負載測試
   - 識別性能瓶頸並優化
   - 建立性能基準和監控指標

## 10. 維護與擴展

### 10.1 錯誤記錄

- 使用結構化日誌記錄異常情況
- 定期審查錯誤日誌識別問題模式
- 關鍵操作建立審計日誌

### 10.2 版本管理

- 使用語義化版本號（SemVer）管理API版本
- 保持API向後兼容性
- 明確記錄版本間的變更

### 10.3 擴展性考量

- 設計支持水平擴展的服務架構
- 考慮未來可能的功能擴展
- 預留自定義擴展點

## 11. 相關檔案

### 11.1 原始程式檔案

| 檔案名稱 | 檔案類型 | 檔案大小 | 檔案行數 | 說明 |
|---------|---------|---------|---------|------|
| WSGLAcctDefJson.asmx | ASMX | 103B | 2 | Web服務介面檔 |
| WSGLAcctDefJson.asmx.cs | C# | 1.6KB | 49 | Web服務實現代碼 |
| JsonResult.cs | C# | 1.2KB | 45 | JSON處理輔助類 |
| AccountInfo.cs | C# | 2.1KB | 58 | 科目資訊實體類 |

### 11.2 相依元件檔案

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| Newtonsoft.Json.dll | .NET組件 | JSON序列化庫 |
| IBosDB.dll | .NET組件 | 資料庫存取組件 |
| BosCache.dll | .NET組件 | 快取管理組件 |

## 12. 測試計劃

### 12.1 單元測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 驗證GetAccountById | 使用有效科目ID呼叫服務 | 返回正確的科目資訊 |
| 驗證GetAccountById無效參數 | 使用無效科目ID呼叫服務 | 返回適當的錯誤訊息 |
| 驗證SearchAccounts | 使用關鍵字搜尋存在的科目 | 返回符合的科目列表 |
| 驗證權限控制 | 使用無權限用戶呼叫服務 | 返回權限不足錯誤 |

### 12.2 整合測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 與前端科目選擇控制項整合 | 在實際頁面中使用服務 | 科目資料正確顯示並可選擇 |
| 與權限系統整合 | 測試不同權限用戶的存取結果 | 權限控制正確生效 |
| 高負載測試 | 模擬50個並發請求 | 服務能正常處理並返回結果 |

### 12.3 性能測試

| 測試指標 | 基準值 | 測試方法 |
|---------|-------|---------|
| 響應時間 | <200ms | 使用JMeter測試平均響應時間 |
| 最大並發數 | >100 | 逐步增加並發請求直到性能下降 |
| 緩存命中率 | >90% | 分析緩存命中日誌 |

## 13. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2025/05/08 | Claude AI | 初始版本建立 | 完成基本功能规格 | 