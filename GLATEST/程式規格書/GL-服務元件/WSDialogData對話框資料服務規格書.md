# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | WSDialogData |
| 程式名稱 | 對話框資料服務 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | /GLATEST/app/WebServices/WSDialogData.asmx, /GLATEST/app/WebServices/WSDialogData.asmx.cs |
| 程式類型 | Web服務 (Web Service) |
| 建立日期 | 2025/05/09 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/09 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

WSDialogData 是泛太總帳系統的對話框資料服務，負責提供系統各種彈出對話框和模態視窗所需的資料。此服務採用標準 ASMX Web Service 實現，以 XML 和 JSON 格式提供資料交換功能，主要用於前端對話框元件與後端資料庫之間的資料存取。服務支援多種查詢條件和篩選機制，能夠為不同類型的對話框提供客製化的資料結構。

### 2.2 業務流程

WSDialogData 在系統中扮演以下角色：
1. 提供彈出對話框需要的資料查詢
2. 支援模態視窗的資料載入和刷新
3. 處理對話框中資料的排序和分頁
4. 為各種選擇性對話框提供資料源
5. 減少頁面載入時間，提供非同步資料載入
6. 維護對話框元件的狀態一致性

### 2.3 使用頻率

- 中高頻率：在使用者操作需要選擇或查詢資料時呼叫
- 系統中約有60%的頁面會使用到對話框功能
- 使用高峰期每秒可能有數十次服務呼叫
- 典型用戶每天觸發上百次對話框操作

### 2.4 使用者角色

此服務間接服務於系統的所有角色：
- 系統管理員：系統設定和管理對話框
- 財務主管：查詢和選擇核准相關資料
- 會計人員：日常交易處理中的資料選擇
- 一般用戶：基本資料查詢和選擇

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Services (.NET Framework 4.0)
- 主要技術：
  - ASMX Web Services：提供HTTP/SOAP通信協議
  - ADO.NET：資料庫存取技術
  - Newtonsoft.Json：處理JSON序列化和反序列化
  - jQuery：前端整合技術
- 通信協議：支援SOAP和REST格式API
- 資料格式：XML和JSON，支持UTF-8編碼

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_DIALOG_DEF | 對話框定義表 | 讀取 |
| SYS_DIALOG_PARAM | 對話框參數表 | 讀取 |
| SYS_CODE | 系統代碼表 | 讀取 |
| GL_ACCT_DEF | 會計科目表 | 讀取 |
| GL_DEPT | 部門資料表 | 讀取 |
| GL_COMPANY | 公司資料表 | 讀取 |
| SYS_USER | 使用者資料表 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| AppAuthority | 權限管理 | 檢查資料存取權限 |
| JsonHelper | JSON處理 | JSON格式轉換輔助類 |
| BosCache | 快取管理 | 資料快取處理 |

## 4. 服務規格

### 4.1 服務介面

```csharp
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class WSDialogData : WebService
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetDialogData(string dialogId, string parameters);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCodeData(string codeType, string parentCode, string filter);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCompanyData(string filter);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetDepartmentData(string companyId, string filter);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetEmployeeData(string companyId, string deptId, string filter);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetPaginatedData(string dialogId, string parameters, int pageIndex, int pageSize);
    
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ValidateDialogSelection(string dialogId, string selectedValue);
}
```

### 4.2 資料結構

#### 4.2.1 輸入參數

| 方法 | 參數名稱 | 資料型態 | 必填 | 說明 |
|------|---------|---------|------|------|
| GetDialogData | dialogId | string | Y | 對話框代碼 |
| GetDialogData | parameters | string | N | JSON格式參數字串 |
| GetCodeData | codeType | string | Y | 代碼類型 |
| GetCodeData | parentCode | string | N | 父代碼 |
| GetCodeData | filter | string | N | 篩選條件 |
| GetCompanyData | filter | string | N | 篩選條件 |
| GetDepartmentData | companyId | string | Y | 公司代碼 |
| GetDepartmentData | filter | string | N | 篩選條件 |
| GetEmployeeData | companyId | string | Y | 公司代碼 |
| GetEmployeeData | deptId | string | N | 部門代碼 |
| GetEmployeeData | filter | string | N | 篩選條件 |
| GetPaginatedData | dialogId | string | Y | 對話框代碼 |
| GetPaginatedData | parameters | string | N | JSON格式參數字串 |
| GetPaginatedData | pageIndex | int | Y | 頁碼(0開始) |
| GetPaginatedData | pageSize | int | Y | 每頁記錄數 |
| ValidateDialogSelection | dialogId | string | Y | 對話框代碼 |
| ValidateDialogSelection | selectedValue | string | Y | 選擇的值 |

#### 4.2.2 輸出結構

對話框資料 JSON 格式範例：

```json
{
  "success": true,
  "message": "操作成功",
  "data": {
    "totalCount": 100,
    "pageIndex": 0,
    "pageSize": 10,
    "items": [
      {
        "id": "1001",
        "code": "D001",
        "name": "研發部",
        "description": "負責產品研發",
        "attributes": {
          "manager": "張三",
          "location": "台北"
        }
      },
      {
        "id": "1002",
        "code": "D002",
        "name": "行銷部",
        "description": "負責產品行銷",
        "attributes": {
          "manager": "李四",
          "location": "台北"
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
| 404 | 對話框定義不存在 | 請求的對話框ID不存在 |
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
檢查對話框定義 → 定義不存在 → 返回錯誤訊息(404)
 ↓
檢查使用者權限 → 權限不足 → 返回錯誤訊息(401) 
 ↓
解析對話框參數
 ↓
構建SQL查詢
 ↓
執行資料庫查詢
 ↓
處理查詢結果
 ↓
將資料轉換為JSON格式
 ↓
返回JSON結果
 ↓
結束
```

### 5.2 特殊處理邏輯

1. **對話框參數處理**：
   - 支援複雜的參數結構，包括巢狀參數
   - 參數可包含條件表達式和函數調用
   - 支援動態參數和靜態參數混合使用

2. **分頁處理**：
   - 採用SQL Server分頁查詢優化
   - 支援客戶端和伺服器端分頁
   - 提供總記錄數和頁碼資訊

3. **多來源資料整合**：
   - 支援跨資料表查詢和整合
   - 處理不同資料源的格式一致性
   - 合併多個查詢結果到單一響應

### 5.3 例外處理

| 例外類型 | 處理方式 | 記錄方式 |
|---------|---------|---------|
| 參數驗證例外 | 返回400錯誤，提供具體的參數錯誤訊息 | 只記錄關鍵資訊，不記錄完整堆疊 |
| 資料庫例外 | 返回500錯誤，提供一般性錯誤訊息，隱藏技術細節 | 記錄完整例外資訊，包含SQL和堆疊 |
| 權限例外 | 返回401錯誤，提供權限不足的具體原因 | 記錄使用者ID、請求資源和需要的權限 |
| 對話框定義例外 | 返回404錯誤，提供對話框不存在的訊息 | 記錄請求的對話框ID和參數 |
| 未預期例外 | 返回500錯誤，提供一般性錯誤代碼以便追蹤 | 記錄完整例外資訊和環境狀態 |

## 6. SQL查詢

### 6.1 對話框定義查詢

```sql
-- 獲取對話框定義
SELECT d.DIALOG_ID, d.DIALOG_NAME, d.DIALOG_TYPE, d.SQL_STATEMENT,
       d.DISPLAY_FIELDS, d.VALUE_FIELD, d.TEXT_FIELD
FROM SYS_DIALOG_DEF d
WHERE d.DIALOG_ID = @DialogId;
```

### 6.2 對話框參數查詢

```sql
-- 獲取對話框參數定義
SELECT p.PARAM_ID, p.PARAM_NAME, p.PARAM_TYPE, p.DEFAULT_VALUE,
       p.IS_REQUIRED, p.VALIDATION_RULE
FROM SYS_DIALOG_PARAM p
WHERE p.DIALOG_ID = @DialogId
ORDER BY p.SEQUENCE;
```

### 6.3 代碼資料查詢

```sql
-- 獲取系統代碼資料
SELECT c.CODE_ID, c.CODE_NAME, c.PARENT_CODE, c.SORT_ORDER
FROM SYS_CODE c
WHERE c.CODE_TYPE = @CodeType
  AND (c.PARENT_CODE = @ParentCode OR @ParentCode IS NULL)
  AND (c.CODE_ID LIKE '%' + @Filter + '%' OR 
       c.CODE_NAME LIKE '%' + @Filter + '%' OR 
       @Filter IS NULL)
ORDER BY c.SORT_ORDER, c.CODE_ID;
```

### 6.4 部門資料查詢

```sql
-- 獲取部門資料
SELECT d.DEPT_ID, d.DEPT_NAME, d.PARENT_DEPT_ID, d.MANAGER_ID,
       d.DESCRIPTION, d.IS_ACTIVE
FROM GL_DEPT d
WHERE d.COMPANY_ID = @CompanyId
  AND (d.DEPT_ID LIKE '%' + @Filter + '%' OR 
       d.DEPT_NAME LIKE '%' + @Filter + '%' OR 
       @Filter IS NULL)
  AND d.IS_ACTIVE = 1
ORDER BY d.DEPT_ID;
```

### 6.5 分頁資料查詢

```sql
-- 獲取分頁資料總數
DECLARE @TotalCount INT;

SELECT @TotalCount = COUNT(*)
FROM ({動態SQL查詢}) AS CountQuery;

-- 獲取分頁資料
WITH PagedData AS (
    SELECT ROW_NUMBER() OVER (ORDER BY {排序欄位}) AS RowNum,
           {查詢欄位}
    FROM ({動態SQL查詢}) AS DataQuery
)
SELECT *
FROM PagedData
WHERE RowNum BETWEEN (@PageIndex * @PageSize) + 1 
                  AND (@PageIndex + 1) * @PageSize;

-- 返回總數和資料
SELECT @TotalCount AS TotalCount;
```

## 7. 程式碼說明

### 7.1 GetDialogData 實現

```csharp
/// <summary>
/// 獲取對話框資料
/// </summary>
/// <param name="dialogId">對話框代碼</param>
/// <param name="parameters">JSON格式參數字串</param>
/// <returns>JSON格式的對話框資料</returns>
[WebMethod]
[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
public string GetDialogData(string dialogId, string parameters)
{
    JsonResult result = new JsonResult();
    
    try
    {
        // 參數驗證
        if (string.IsNullOrEmpty(dialogId))
        {
            return result.Failure("參數錯誤：對話框代碼不能為空").ToJson();
        }
        
        // 獲取當前使用者
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 檢查權限
        if (!AppAuthority.Instance.HasPermission(userId, "SYS_DIALOG", "VIEW"))
        {
            return result.Failure("權限不足：無對話框資料查詢權限").ToJson();
        }
        
        // 獲取對話框定義
        DialogInfo dialogInfo = GetDialogDefinition(dialogId);
        
        if (dialogInfo == null)
        {
            return result.Failure("對話框定義不存在").ToJson();
        }
        
        // 解析參數
        Dictionary<string, object> paramDict = ParseParameters(parameters, dialogInfo);
        
        // 獲取資料
        IBosDB db = DBFactory.GetBosDB();
        DataTable dt = null;
        
        if (dialogInfo.DialogType == "SQL")
        {
            // 基於SQL模板的對話框
            string sql = dialogInfo.SqlStatement;
            dt = db.ExecuteDataTable(sql, paramDict);
        }
        else if (dialogInfo.DialogType == "CODE")
        {
            // 基於代碼表的對話框
            string codeType = dialogInfo.CodeType;
            string parentCode = paramDict.ContainsKey("PARENT_CODE") ? 
                                paramDict["PARENT_CODE"].ToString() : null;
            string filter = paramDict.ContainsKey("FILTER") ? 
                           paramDict["FILTER"].ToString() : null;
            
            dt = GetCodeTableData(db, codeType, parentCode, filter);
        }
        else if (dialogInfo.DialogType == "CUSTOM")
        {
            // 自定義處理的對話框
            dt = ProcessCustomDialog(dialogInfo, paramDict, db);
        }
        
        // 處理結果
        if (dt == null || dt.Rows.Count == 0)
        {
            return result.Success("無符合條件的資料", new { items = new object[0] }).ToJson();
        }
        
        // 轉換為JSON結果
        List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
        
        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            
            foreach (DataColumn col in dt.Columns)
            {
                item[col.ColumnName.ToLower()] = row[col] == DBNull.Value ? null : row[col];
            }
            
            items.Add(item);
        }
        
        return result.Success("操作成功", new { items = items }).ToJson();
    }
    catch (Exception ex)
    {
        Logger.Error("WSDialogData.GetDialogData Error", ex);
        return result.Failure("系統錯誤：" + ex.Message).ToJson();
    }
}
```

### 7.2 GetPaginatedData 實現

```csharp
/// <summary>
/// 獲取分頁對話框資料
/// </summary>
/// <param name="dialogId">對話框代碼</param>
/// <param name="parameters">JSON格式參數字串</param>
/// <param name="pageIndex">頁碼(0開始)</param>
/// <param name="pageSize">每頁記錄數</param>
/// <returns>JSON格式的分頁對話框資料</returns>
[WebMethod]
[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
public string GetPaginatedData(string dialogId, string parameters, int pageIndex, int pageSize)
{
    JsonResult result = new JsonResult();
    
    try
    {
        // 參數驗證
        if (string.IsNullOrEmpty(dialogId))
        {
            return result.Failure("參數錯誤：對話框代碼不能為空").ToJson();
        }
        
        if (pageIndex < 0)
        {
            pageIndex = 0;
        }
        
        if (pageSize <= 0 || pageSize > 100)
        {
            pageSize = 10;
        }
        
        // 獲取當前使用者
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 檢查權限
        if (!AppAuthority.Instance.HasPermission(userId, "SYS_DIALOG", "VIEW"))
        {
            return result.Failure("權限不足：無對話框資料查詢權限").ToJson();
        }
        
        // 獲取對話框定義
        DialogInfo dialogInfo = GetDialogDefinition(dialogId);
        
        if (dialogInfo == null)
        {
            return result.Failure("對話框定義不存在").ToJson();
        }
        
        // 解析參數
        Dictionary<string, object> paramDict = ParseParameters(parameters, dialogInfo);
        paramDict.Add("@PageIndex", pageIndex);
        paramDict.Add("@PageSize", pageSize);
        
        // 構建分頁查詢
        string sql = BuildPaginatedQuery(dialogInfo, paramDict);
        
        // 執行查詢
        IBosDB db = DBFactory.GetBosDB();
        DataSet ds = db.ExecuteDataSet(sql, paramDict);
        
        if (ds == null || ds.Tables.Count < 2)
        {
            return result.Failure("查詢執行錯誤").ToJson();
        }
        
        // 獲取總記錄數
        int totalCount = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            totalCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
        }
        
        // 獲取資料記錄
        DataTable dt = ds.Tables[1];
        
        // 轉換為JSON結果
        List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
        
        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            
            foreach (DataColumn col in dt.Columns)
            {
                item[col.ColumnName.ToLower()] = row[col] == DBNull.Value ? null : row[col];
            }
            
            items.Add(item);
        }
        
        var resultData = new
        {
            totalCount = totalCount,
            pageIndex = pageIndex,
            pageSize = pageSize,
            pageCount = (totalCount + pageSize - 1) / pageSize,
            items = items
        };
        
        return result.Success("操作成功", resultData).ToJson();
    }
    catch (Exception ex)
    {
        Logger.Error("WSDialogData.GetPaginatedData Error", ex);
        return result.Failure("系統錯誤：" + ex.Message).ToJson();
    }
}
```

### 7.3 獲取對話框定義

```csharp
/// <summary>
/// 獲取對話框定義
/// </summary>
/// <param name="dialogId">對話框代碼</param>
/// <returns>對話框定義對象</returns>
private DialogInfo GetDialogDefinition(string dialogId)
{
    try
    {
        // 嘗試從緩存獲取
        string cacheKey = $"DLG_DEF_{dialogId}";
        DialogInfo dialogInfo = BosCache.Get<DialogInfo>(cacheKey);
        
        if (dialogInfo != null)
        {
            return dialogInfo;
        }
        
        // 從資料庫讀取
        IBosDB db = DBFactory.GetBosDB();
        
        string sql = @"
            SELECT d.DIALOG_ID, d.DIALOG_NAME, d.DIALOG_TYPE, d.SQL_STATEMENT,
                   d.DISPLAY_FIELDS, d.VALUE_FIELD, d.TEXT_FIELD, d.CODE_TYPE,
                   d.DEFAULT_ORDER_BY, d.CUSTOM_TYPE
            FROM SYS_DIALOG_DEF d
            WHERE d.DIALOG_ID = @DialogId";
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@DialogId", dialogId);
        
        DataTable dt = db.ExecuteDataTable(sql, parameters);
        
        if (dt == null || dt.Rows.Count == 0)
        {
            return null;
        }
        
        // 構建對話框定義
        dialogInfo = new DialogInfo();
        dialogInfo.DialogId = dt.Rows[0]["DIALOG_ID"].ToString();
        dialogInfo.DialogName = dt.Rows[0]["DIALOG_NAME"].ToString();
        dialogInfo.DialogType = dt.Rows[0]["DIALOG_TYPE"].ToString();
        dialogInfo.SqlStatement = dt.Rows[0]["SQL_STATEMENT"].ToString();
        dialogInfo.DisplayFields = dt.Rows[0]["DISPLAY_FIELDS"].ToString();
        dialogInfo.ValueField = dt.Rows[0]["VALUE_FIELD"].ToString();
        dialogInfo.TextField = dt.Rows[0]["TEXT_FIELD"].ToString();
        dialogInfo.CodeType = dt.Rows[0]["CODE_TYPE"].ToString();
        dialogInfo.DefaultOrderBy = dt.Rows[0]["DEFAULT_ORDER_BY"].ToString();
        dialogInfo.CustomType = dt.Rows[0]["CUSTOM_TYPE"].ToString();
        
        // 獲取參數定義
        sql = @"
            SELECT p.PARAM_ID, p.PARAM_NAME, p.PARAM_TYPE, p.DEFAULT_VALUE,
                   p.IS_REQUIRED, p.VALIDATION_RULE
            FROM SYS_DIALOG_PARAM p
            WHERE p.DIALOG_ID = @DialogId
            ORDER BY p.SEQUENCE";
        
        DataTable paramDt = db.ExecuteDataTable(sql, parameters);
        
        if (paramDt != null && paramDt.Rows.Count > 0)
        {
            dialogInfo.Parameters = new List<DialogParamInfo>();
            
            foreach (DataRow row in paramDt.Rows)
            {
                DialogParamInfo paramInfo = new DialogParamInfo();
                paramInfo.ParamId = row["PARAM_ID"].ToString();
                paramInfo.ParamName = row["PARAM_NAME"].ToString();
                paramInfo.ParamType = row["PARAM_TYPE"].ToString();
                paramInfo.DefaultValue = row["DEFAULT_VALUE"].ToString();
                paramInfo.IsRequired = Convert.ToBoolean(row["IS_REQUIRED"]);
                paramInfo.ValidationRule = row["VALIDATION_RULE"].ToString();
                
                dialogInfo.Parameters.Add(paramInfo);
            }
        }
        
        // 設置緩存
        BosCache.Set(cacheKey, dialogInfo, TimeSpan.FromMinutes(30));
        
        return dialogInfo;
    }
    catch (Exception ex)
    {
        Logger.Error("WSDialogData.GetDialogDefinition Error", ex);
        return null;
    }
}
```

## 8. 安全性考量

### 8.1 認證與授權

1. **認證機制**
   - 服務需使用者登入後才能存取
   - 透過LoginClass驗證使用者身份
   - 每次服務呼叫驗證使用者會話有效性

2. **授權控制**
   - 使用AppAuthority檢查存取權限
   - 對話框資料依據使用者權限進行篩選
   - 敏感對話框需特殊權限才能存取

### 8.2 資料保護

1. **SQL注入防護**
   - 使用參數化查詢處理所有SQL
   - 避免動態拼接SQL語句
   - 驗證所有查詢參數

2. **資料過濾**
   - 對敏感資料實施基於使用者角色的過濾
   - 限制查詢結果數量
   - 隱藏不必要的敏感欄位

### 8.3 通信安全

1. **傳輸安全**
   - 使用HTTPS進行資料傳輸
   - 保護查詢參數和返回資料
   - 實施適當的快取策略

2. **輸入驗證**
   - 驗證所有輸入參數
   - 限制特殊字符和腳本注入
   - 檢查參數長度和格式

## 9. 效能優化

### 9.1 資料庫優化

1. **查詢優化**
   - 為常用查詢建立索引
   - 使用資料庫分頁機制
   - 限制返回資料量

2. **連接池管理**
   - 適當設置連接池大小
   - 盡快釋放資料庫連接
   - 監控連接使用情況

### 9.2 緩存策略

1. **對話框定義緩存**
   - 對話框定義資料緩存30分鐘
   - 系統代碼資料緩存60分鐘
   - 常用查詢結果緩存5分鐘

2. **緩存失效**
   - 對話框定義修改時主動失效相關緩存
   - 實施分層緩存策略
   - 定期刷新長期緩存

### 9.3 響應優化

1. **資料壓縮**
   - 啟用HTTP壓縮
   - 最小化JSON響應大小
   - 移除不必要的空白字符

2. **異步處理**
   - 使用異步處理大型查詢
   - 實施請求節流
   - 優化前端資料處理

## 10. 相關檔案

### 10.1 原始程式檔案

| 檔案名稱 | 檔案類型 | 檔案大小 | 檔案行數 | 說明 |
|---------|---------|---------|---------|------|
| WSDialogData.asmx | ASMX | 97B | 2 | Web服務介面檔 |
| WSDialogData.asmx.cs | C# | 1.6KB | 52 | Web服務實現代碼 |
| DialogInfo.cs | C# | 2.0KB | 61 | 對話框資訊實體類 |
| DialogParamInfo.cs | C# | 1.3KB | 38 | 對話框參數實體類 |

### 10.2 相依元件檔案

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| IBosDB.dll | .NET組件 | 資料庫存取組件 |
| BosCache.dll | .NET組件 | 緩存管理組件 |
| Newtonsoft.Json.dll | .NET組件 | JSON處理組件 |

## 11. 測試計劃

### 11.1 單元測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 對話框定義測試 | 測試GetDialogDefinition方法 | 正確返回對話框定義 |
| 參數解析測試 | 測試ParseParameters方法 | 正確解析JSON參數 |
| 代碼資料獲取測試 | 測試GetCodeData方法 | 正確返回代碼資料 |
| 分頁查詢測試 | 測試GetPaginatedData方法 | 正確返回分頁資料和總數 |

### 11.2 整合測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 前端整合測試 | 使用典型對話框頁面測試 | 對話框正確顯示資料 |
| 權限整合測試 | 使用不同權限用戶測試 | 權限控制正確生效 |
| 分頁與排序測試 | 測試分頁和排序功能 | 分頁和排序正確工作 |

### 11.3 效能測試

| 測試指標 | 基準值 | 測試方法 |
|---------|-------|---------|
| 響應時間 | <150ms | 使用JMeter測試平均響應時間 |
| 最大並發數 | >50 | 逐步增加並發請求直到性能下降 |
| 緩存命中率 | >80% | 分析緩存命中日誌 |

## 12. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2025/05/09 | Claude AI | 初始版本建立 | 完成基本功能規格 | 