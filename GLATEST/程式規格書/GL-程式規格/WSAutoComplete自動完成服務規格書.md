# WSAutoComplete 自動完成服務規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | WSAutoComplete                        |
| 程式名稱     | 自動完成服務                             |
| 檔案大小     | 20KB                                 |
| 行數        | ~630                                 |
| 功能簡述     | 提供自動完成功能                          |
| 複雜度       | 高                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/22                           |

## 程式功能概述

WSAutoComplete 是泛太總帳系統中的自動完成服務元件，提供前端頁面輸入控制項的資料自動完成功能。此服務以 Web Service (ASMX) 形式實現，透過 AJAX 技術向用戶端提供即時的資料查詢與建議功能。WSAutoComplete 支援多種資料類型的自動完成，包括：

1. 科目代碼與名稱的自動完成
2. 部門代碼與名稱的自動完成
3. 專案代碼與名稱的自動完成
4. 供應商/客戶代碼與名稱的自動完成
5. 員工代碼與姓名的自動完成
6. 商品代碼與名稱的自動完成
7. 交易類型與描述的自動完成
8. 公司代碼與名稱的自動完成

此元件透過提供精確且即時的自動完成功能，大幅提升了使用者輸入資料的效率和準確性，減少了資料輸入錯誤的可能性，同時也改善了系統整體的使用者體驗。

## 類別結構說明

WSAutoComplete 服務主要包含下列類別結構：

1. **WSAutoComplete**：主要的 Web Service 類別，繼承自 System.Web.Services.WebService，提供各種自動完成方法的公開介面。

2. **AutoCompleteItem**：自動完成項目的資料結構類別，包含:
   - Value：實際值（如代碼）
   - Label：顯示值（如名稱或代碼+名稱的組合）
   - Category：分類（可選）
   - Desc：額外描述（可選）

3. **AutoCompleteConfig**：自動完成服務的配置類別，管理如搜尋延遲、最小輸入字元數、最大結果數等設定。

4. **DataCache**：內部快取機制類別，用於快取常用的自動完成資料，提升回應速度。

## 技術實現

WSAutoComplete 採用以下技術實現：

1. **ASP.NET Web Service (ASMX)**：作為自動完成服務的基礎架構
2. **JSON**：用於資料交換的格式，實現輕量級通訊
3. **ADO.NET**：用於資料庫存取和查詢
4. **jQuery UI Autocomplete**：前端實現自動完成功能的 UI 庫
5. **資料快取**：使用記憶體快取技術，提升查詢效能
6. **SQL 參數化查詢**：防止 SQL 注入攻擊
7. **非同步處理**：支援非同步方法調用，避免阻塞 UI 線程
8. **超時控制**：實現查詢超時機制，確保響應及時性

## 相依類別和元件

WSAutoComplete 依賴以下類別和元件：

1. **System.Web.Services**：提供 Web Service 基礎功能
2. **System.Web.Script.Services**：提供 JSON 序列化支援
3. **System.Web.Script.Serialization**：用於 .NET 對象與 JSON 轉換
4. **System.Data**：提供資料存取功能
5. **DBManger**：自定義資料庫管理類別，處理資料庫連接和操作
6. **System.Web.Caching**：提供快取功能
7. **appSettings**：從 Web.config 讀取配置設定
8. **jQuery**：前端 JavaScript 庫（客戶端相依）
9. **jQuery UI**：提供自動完成UI組件（客戶端相依）

## 屬性說明

WSAutoComplete 提供以下主要公開屬性：

| 屬性名稱 | 資料類型 | 說明 | 存取權限 |
|---------|---------|------|---------|
| MinChars | int | 觸發自動完成的最小字元數 | 公開 |
| MaxResults | int | 返回結果的最大數量 | 公開 |
| CacheEnabled | bool | 是否啟用資料快取 | 公開 |
| CacheExpiration | int | 快取過期時間(分鐘) | 公開 |
| SearchDelay | int | 搜尋延遲時間(毫秒) | 公開 |
| Timeout | int | 查詢超時時間(秒) | 公開 |
| ErrorMessage | string | 最後的錯誤訊息 | 公開 |

## 私有成員變數

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _db | DBManger | 資料庫管理器實例 |
| _cache | DataCache | 資料快取管理器 |
| _config | AutoCompleteConfig | 自動完成配置 |
| _errorMsg | string | 錯誤訊息儲存 |
| _companyId | string | 目前操作的公司ID |
| _logger | Logger | 日誌記錄器 |
| _defaultExpiration | TimeSpan | 預設快取過期時間 |
| _searchHistory | Dictionary<string, DateTime> | 搜尋歷史記錄 |

## 方法說明

### 建構函式

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| WSAutoComplete | 無 | 無 | 預設建構函式，初始化服務和相關設定 |

### Web 方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| GetSubjects | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的科目清單 |
| GetDepartments | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的部門清單 |
| GetProjects | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的專案清單 |
| GetVendors | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的供應商清單 |
| GetCustomers | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的客戶清單 |
| GetEmployees | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的員工清單 |
| GetProducts | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的商品清單 |
| GetTransactionTypes | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的交易類型清單 |
| GetCompanies | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的公司清單 |
| GetCurrencies | string prefix, int maxResults | List<AutoCompleteItem> | 取得符合前綴的幣別清單 |

### 資料處理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| ExecuteQuery | string sql, Dictionary<string, object> parameters, string cacheKey | DataTable | 執行SQL查詢並返回結果 |
| FormatResult | DataTable data, string valueField, string displayField, string descField | List<AutoCompleteItem> | 將查詢結果格式化為自動完成項目清單 |
| TrimResults | List<AutoCompleteItem> items, int maxResults | List<AutoCompleteItem> | 裁剪結果數量至指定上限 |
| SortResults | List<AutoCompleteItem> items, string prefix | List<AutoCompleteItem> | 依相關性排序結果 |
| GetCacheKey | string methodName, string prefix, int maxResults | string | 生成快取鍵值 |
| LogSearch | string term, string entity, string username | void | 記錄搜尋歷史 |

## 程式碼說明

### 主要類別定義

```csharp
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class WSAutoComplete : System.Web.Services.WebService
{
    private DBManger _db;
    private DataCache _cache;
    private AutoCompleteConfig _config;
    private string _errorMsg;
    private string _companyId;
    private Logger _logger;
    
    public WSAutoComplete()
    {
        _db = new DBManger();
        _cache = new DataCache();
        _config = new AutoCompleteConfig();
        _logger = new Logger("WSAutoComplete");
        
        // 從配置中讀取設定
        LoadConfig();
    }
    
    private void LoadConfig()
    {
        try
        {
            _config.MinChars = Convert.ToInt32(ConfigurationManager.AppSettings["AutoComplete_MinChars"] ?? "1");
            _config.MaxResults = Convert.ToInt32(ConfigurationManager.AppSettings["AutoComplete_MaxResults"] ?? "10");
            _config.CacheEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["AutoComplete_CacheEnabled"] ?? "true");
            _config.CacheExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["AutoComplete_CacheExpiration"] ?? "30");
            _config.SearchDelay = Convert.ToInt32(ConfigurationManager.AppSettings["AutoComplete_SearchDelay"] ?? "300");
            _config.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["AutoComplete_Timeout"] ?? "5");
        }
        catch (Exception ex)
        {
            _errorMsg = "載入自動完成設定時發生錯誤: " + ex.Message;
            _logger.LogError(_errorMsg);
            
            // 使用預設值
            _config.MinChars = 1;
            _config.MaxResults = 10;
            _config.CacheEnabled = true;
            _config.CacheExpiration = 30;
            _config.SearchDelay = 300;
            _config.Timeout = 5;
        }
    }
    
    // 公開屬性
    public int MinChars
    {
        get { return _config.MinChars; }
        set { _config.MinChars = value; }
    }
    
    public int MaxResults
    {
        get { return _config.MaxResults; }
        set { _config.MaxResults = value; }
    }
    
    public bool CacheEnabled
    {
        get { return _config.CacheEnabled; }
        set { _config.CacheEnabled = value; }
    }
    
    public int CacheExpiration
    {
        get { return _config.CacheExpiration; }
        set { _config.CacheExpiration = value; }
    }
    
    public int SearchDelay
    {
        get { return _config.SearchDelay; }
        set { _config.SearchDelay = value; }
    }
    
    public int Timeout
    {
        get { return _config.Timeout; }
        set { _config.Timeout = value; }
    }
    
    public string ErrorMessage
    {
        get { return _errorMsg; }
    }
    
    // 其他成員和方法...
}
```

### 自動完成項目類別

```csharp
public class AutoCompleteItem
{
    public string Value { get; set; }
    public string Label { get; set; }
    public string Category { get; set; }
    public string Desc { get; set; }
    
    public AutoCompleteItem()
    {
    }
    
    public AutoCompleteItem(string value, string label)
    {
        Value = value;
        Label = label;
    }
    
    public AutoCompleteItem(string value, string label, string desc)
    {
        Value = value;
        Label = label;
        Desc = desc;
    }
    
    public AutoCompleteItem(string value, string label, string category, string desc)
    {
        Value = value;
        Label = label;
        Category = category;
        Desc = desc;
    }
}
```

### 科目自動完成方法實現

```csharp
[WebMethod]
[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
public List<AutoCompleteItem> GetSubjects(string prefix, int maxResults = 0)
{
    if (string.IsNullOrEmpty(prefix) || prefix.Length < _config.MinChars)
    {
        return new List<AutoCompleteItem>();
    }
    
    if (maxResults <= 0)
    {
        maxResults = _config.MaxResults;
    }
    
    // 取得公司ID
    string companyId = HttpContext.Current.Session["CompanyID"] as string;
    if (string.IsNullOrEmpty(companyId))
    {
        _errorMsg = "未找到有效的公司ID";
        _logger.LogWarning(_errorMsg);
        return new List<AutoCompleteItem>();
    }
    
    // 檢查快取
    string cacheKey = GetCacheKey("GetSubjects", prefix, maxResults);
    if (_config.CacheEnabled)
    {
        List<AutoCompleteItem> cachedResult = _cache.Get<List<AutoCompleteItem>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }
    }
    
    try
    {
        // 準備SQL查詢
        string sql = @"
            SELECT 
                SUB_NO as Value, 
                SUB_NAME as Label,
                SUB_TYPE as Category, 
                SUB_DESC as Description 
            FROM 
                GL_SUBJECTS 
            WHERE 
                COMPANY_ID = @CompanyID AND
                (SUB_NO LIKE @Prefix OR SUB_NAME LIKE @Prefix) AND
                SUB_STATUS = 'A'
            ORDER BY 
                CASE 
                    WHEN SUB_NO LIKE @ExactPrefix THEN 0
                    WHEN SUB_NAME LIKE @ExactPrefix THEN 1
                    ELSE 2
                END,
                SUB_NO";
        
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CompanyID", companyId },
            { "@Prefix", prefix + "%" },
            { "@ExactPrefix", prefix + "%" }
        };
        
        // 執行查詢
        DataTable result = ExecuteQuery(sql, parameters, null);
        
        // 格式化結果
        List<AutoCompleteItem> items = FormatResult(result, "Value", "Label", "Description");
        
        // 裁剪和排序結果
        items = TrimResults(items, maxResults);
        items = SortResults(items, prefix);
        
        // 儲存到快取
        if (_config.CacheEnabled)
        {
            _cache.Set(cacheKey, items, TimeSpan.FromMinutes(_config.CacheExpiration));
        }
        
        // 記錄搜尋
        LogSearch(prefix, "Subjects", HttpContext.Current.User.Identity.Name);
        
        return items;
    }
    catch (Exception ex)
    {
        _errorMsg = "執行科目自動完成查詢時發生錯誤: " + ex.Message;
        _logger.LogError(_errorMsg);
        return new List<AutoCompleteItem>();
    }
}
```

### 格式化結果的方法

```csharp
private List<AutoCompleteItem> FormatResult(DataTable data, string valueField, string displayField, string descField)
{
    List<AutoCompleteItem> items = new List<AutoCompleteItem>();
    
    if (data == null || data.Rows.Count == 0)
    {
        return items;
    }
    
    foreach (DataRow row in data.Rows)
    {
        string value = row[valueField].ToString();
        string label = row[displayField].ToString();
        
        // 組合顯示文字 (代碼 - 名稱)
        if (valueField != displayField)
        {
            label = string.Format("{0} - {1}", value, label);
        }
        
        string desc = "";
        if (!string.IsNullOrEmpty(descField) && data.Columns.Contains(descField))
        {
            desc = row[descField].ToString();
        }
        
        string category = "";
        if (data.Columns.Contains("Category"))
        {
            category = row["Category"].ToString();
        }
        
        items.Add(new AutoCompleteItem(value, label, category, desc));
    }
    
    return items;
}
```

## 自動完成項目資料結構

```csharp
public class AutoCompleteConfig
{
    public int MinChars { get; set; }
    public int MaxResults { get; set; }
    public bool CacheEnabled { get; set; }
    public int CacheExpiration { get; set; }
    public int SearchDelay { get; set; }
    public int Timeout { get; set; }
    
    public AutoCompleteConfig()
    {
        // 預設值
        MinChars = 1;
        MaxResults = 10;
        CacheEnabled = true;
        CacheExpiration = 30;
        SearchDelay = 300;
        Timeout = 5;
    }
}
```

## 使用範例

### 前端實現

```html
<!-- 科目選擇控制項 -->
<div class="form-group">
    <label for="txtSubject">科目:</label>
    <input type="text" id="txtSubject" class="form-control autocomplete" 
           data-autocomplete-source="WSAutoComplete.asmx/GetSubjects" 
           data-value-field="subjectValue" />
    <input type="hidden" id="subjectValue" name="subjectValue" />
</div>

<!-- 部門選擇控制項 -->
<div class="form-group">
    <label for="txtDepartment">部門:</label>
    <input type="text" id="txtDepartment" class="form-control autocomplete" 
           data-autocomplete-source="WSAutoComplete.asmx/GetDepartments" 
           data-value-field="departmentValue" />
    <input type="hidden" id="departmentValue" name="departmentValue" />
</div>
```

### jQuery 實現

```javascript
$(document).ready(function() {
    // 初始化所有自動完成控制項
    $(".autocomplete").each(function() {
        var $this = $(this);
        var source = $this.data("autocomplete-source");
        var valueField = $this.data("value-field");
        
        $this.autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: source,
                    data: "{ 'prefix': '" + request.term + "', 'maxResults': 10 }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                label: item.Label,
                                value: item.Label,
                                item: item
                            };
                        }));
                    },
                    error: function(xhr, textStatus, errorThrown) {
                        console.error("自動完成請求失敗: " + textStatus);
                    }
                });
            },
            minLength: 1,
            delay: 300,
            select: function(event, ui) {
                // 當選擇項目時，設置隱藏欄位的值
                $("#" + valueField).val(ui.item.item.Value);
                
                // 觸發 change 事件
                $("#" + valueField).trigger("change");
            }
        });
    });
});
```

## 資料結構

### Web Service 介面

WSAutoComplete 提供以下 Web Service 介面：

```
WSAutoComplete.asmx
```

此 Web Service 檔案內容為：

```xml
<%@ WebService Language="C#" CodeBehind="WSAutoComplete.cs" Class="GLA.WebServices.WSAutoComplete" %>
```

### 主要資料表

WSAutoComplete 服務存取以下資料表：

1. **GL_SUBJECTS**：會計科目資料表
2. **GL_DEPARTMENTS**：部門資料表
3. **GL_PROJECTS**：專案資料表
4. **GL_VENDORS**：供應商資料表
5. **GL_CUSTOMERS**：客戶資料表
6. **GL_EMPLOYEES**：員工資料表
7. **GL_PRODUCTS**：商品資料表
8. **GL_TRANSACTION_TYPES**：交易類型資料表
9. **GL_COMPANIES**：公司資料表
10. **GL_CURRENCIES**：幣別資料表

## 異常處理

WSAutoComplete 實現了以下異常處理機制：

1. **SQL 查詢異常**：捕獲並記錄所有資料庫查詢異常
2. **參數驗證**：驗證輸入參數，防止無效調用
3. **超時處理**：實現查詢超時機制，避免長時間等待
4. **空結果處理**：優雅處理無匹配項的情況
5. **快取讀取失敗**：處理快取讀取失敗的情況
6. **配置讀取異常**：使用預設值處理配置讀取失敗

## 注意事項與限制

1. **效能考量**：
   - 大量並發請求可能影響系統效能
   - 快取過期時間需根據資料變更頻率調整

2. **查詢限制**：
   - 預設限制返回最多 10 筆資料
   - 最少需輸入 1 個字元才觸發搜尋

3. **相容性**：
   - 需要 jQuery 和 jQuery UI 支援
   - 前端需支援 AJAX 和 JSON

4. **安全性**：
   - 使用參數化查詢防止 SQL 注入
   - 需要進行權限檢查

## 效能考量

為了確保 WSAutoComplete 服務具有良好的效能，採取了以下措施：

1. **資料快取**：使用記憶體快取減少資料庫查詢
2. **查詢最佳化**：使用索引和最佳化 SQL 查詢
3. **結果限制**：限制返回的結果數量
4. **延遲搜尋**：實現前端輸入延遲，減少不必要的請求
5. **資料預處理**：預處理常用的搜尋資料
6. **非同步處理**：使用非同步 AJAX 請求

## 安全性考量

WSAutoComplete 服務實現了以下安全措施：

1. **SQL 注入防護**：使用參數化查詢
2. **XSS 防護**：對輸出資料進行適當編碼
3. **會話驗證**：驗證使用者會話是否有效
4. **權限檢查**：確保使用者有權存取相關資料
5. **輸入驗證**：驗證和清理所有輸入參數

## 待改進事項

1. **增強搜尋功能**：
   - 支援模糊匹配和拼音搜尋
   - 實現更智能的排序演算法

2. **優化效能**：
   - 實現分散式快取
   - 改進資料庫查詢效率

3. **擴展功能**：
   - 支援更多資料類型的自動完成
   - 增加自定義搜尋條件

4. **提升 UI/UX**：
   - 增強結果顯示格式
   - 實現更現代化的界面

5. **改善安全性**：
   - 實現更嚴格的權限控制
   - 增加日誌審計功能

## 相關檔案

1. **WSAutoComplete.cs**：自動完成服務主要程式碼
2. **WSAutoComplete.asmx**：自動完成服務 Web 介面
3. **autocomplete.js**：前端自動完成 JavaScript 封裝
4. **Web.config**：服務配置文件
5. **DBManger.cs**：資料庫管理類別

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/22 | 1.0    | 首次建立自動完成服務規格書      | Claude AI  | 