# CompanyManager 公司資料管理規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CompanyManager                       |
| 程式名稱     | 公司資料管理                           |
| 檔案大小     | 1.5KB                                |
| 行數        | ~64                                  |
| 功能簡述     | 管理公司基本資料                        |
| 複雜度       | 低                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/18                           |

## 程式功能概述

CompanyManager 是泛太總帳系統中的公司資料管理元件，負責提供公司基本資料的查詢和處理功能。此元件作為資料存取層的一部分，為系統中需要公司資料的各個模組提供統一的資料存取介面。CompanyManager 主要功能包括：

1. 提供公司資料的查詢功能
2. 封裝資料庫連接和操作細節
3. 處理公司資料查詢過程中的異常情況
4. 提供錯誤訊息管理機制
5. 支援不同資料庫連接模式
6. 獲取格式化的公司資料列表

此元件被設計為可重用的公共服務類別，簡化系統中對公司資料的存取和管理。

## 類別結構說明

CompanyManager 是一個標準的 C# 類別，實現了公司資料的管理功能：

1. **屬性管理**：包含錯誤處理和資料庫連接相關屬性
2. **建構函式**：提供兩種初始化方式，支援預設和指定連接字串
3. **資料存取方法**：提供公司資料查詢功能
4. **錯誤處理機制**：包含完整的異常捕獲和錯誤訊息管理

整體結構遵循面向對象的設計原則，提供清晰的介面和錯誤處理機制。

## 技術實現

CompanyManager 基於以下技術實現：

1. **C# 程式語言**：使用 C# 實現所有功能
2. **ADO.NET**：使用 ADO.NET 進行資料庫操作
3. **封裝設計模式**：封裝資料庫操作細節，提供簡潔的介面
4. **異常處理**：實現完整的 try-catch 異常處理機制
5. **資料表操作**：返回 DataTable 物件，便於上層應用處理

## 相依類別和元件

CompanyManager 依賴以下類別與元件：

1. **DBManger**：自訂的資料庫管理類別，提供資料庫連接和操作功能
2. **System.Data**：.NET Framework 提供的資料處理命名空間

## 屬性說明

CompanyManager 提供以下公開屬性：

| 屬性名稱 | 資料類型 | 說明 | 存取權限 |
|---------|---------|------|---------|
| ErrorMessage | string | 取得或設定錯誤訊息 | 公開 |
| HasError | bool | 取得或設定是否有錯誤 | 公開 |

## 私有成員變數

CompanyManager 包含以下私有成員變數：

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _errMsg | string | 儲存錯誤訊息 |
| _err | bool | 標記是否有錯誤 |
| _db | DBManger | 資料庫管理器實例 |

## 方法說明

### 建構函式

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| CompanyManager | 無 | 無 | 預設建構函式，使用預設資料庫連接 |
| CompanyManager | DBManger.ConnectionString dbenum | 無 | 指定資料庫連接字串的建構函式 |

### 資料存取方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| GetCompanyDT | 無 | DataTable | 取得公司資料表，包含公司代碼、名稱等資訊 |

## 程式碼說明

### 建構函式

CompanyManager 提供兩種初始化方式：

1. **預設建構函式**：使用系統預設的資料庫連接字串

```csharp
public CompanyManager()
{
    _db = new DBManger();
    _db.New(DBManger.ConnectionString.IBosDB);
}
```

2. **指定資料庫連接的建構函式**：允許指定資料庫連接字串

```csharp
public CompanyManager(DBManger.ConnectionString dbenum)
{
    _db = new DBManger();
    _db.New(dbenum);
}
```

### 公司資料查詢方法

GetCompanyDT 方法提供公司資料的查詢功能：

```csharp
public DataTable GetCompanyDT()
{
    DataTable dt = new DataTable();
    string sql = "SELECT Company + ' - ' + CompanyName as CompanyAndName, * FROM Company ORDER BY Company";
    try
    {
        dt = _db.ExecuteDataTable(sql);
        this._err = false;
    }
    catch (Exception ex)
    {
        dt = null;
        this._errMsg = ex.Message;
        this._err = true;
    }

    return dt;
}
```

此方法執行 SQL 查詢以獲取公司資料，並處理可能發生的異常：
- 正常情況下返回包含公司資料的 DataTable
- 出現異常時設置錯誤訊息並返回 null

## 使用範例

以下是 CompanyManager 的使用範例：

### 基本使用方式

```csharp
// 建立公司管理器實例
CompanyManager companyMgr = new CompanyManager();

// 獲取公司資料表
DataTable dtCompany = companyMgr.GetCompanyDT();

// 檢查是否有錯誤
if (companyMgr.HasError)
{
    // 處理錯誤
    string errorMsg = companyMgr.ErrorMessage;
    // 顯示錯誤訊息或執行其他錯誤處理
}
else
{
    // 使用公司資料
    // 例如：繫結到下拉清單
    ddlCompany.DataSource = dtCompany;
    ddlCompany.DataTextField = "CompanyAndName";
    ddlCompany.DataValueField = "Company";
    ddlCompany.DataBind();
}
```

### 使用特定資料庫連接字串

```csharp
// 使用特定資料庫連接字串建立實例
CompanyManager companyMgr = new CompanyManager(DBManger.ConnectionString.ReportDB);

// 獲取公司資料表
DataTable dtCompany = companyMgr.GetCompanyDT();

// 後續處理與基本使用方式相同
```

## SQL 查詢語句說明

GetCompanyDT 方法使用的 SQL 查詢語句：

```sql
SELECT Company + ' - ' + CompanyName as CompanyAndName, * FROM Company ORDER BY Company
```

此 SQL 查詢具有以下特點：
1. 選取所有公司資料欄位
2. 建立一個合併欄位 CompanyAndName，格式為 "公司代碼 - 公司名稱"
3. 結果依公司代碼排序

## 資料結構

### 輸入參數

此類別的方法不需要輸入參數，直接通過類別初始化和方法調用實現功能。

### 輸出資料

| 方法 | 輸出資料類型 | 說明 |
|------|------------|------|
| GetCompanyDT | DataTable | 包含公司代碼、名稱等資訊的資料表 |

## 異常處理

CompanyManager 實現了完整的異常處理機制：

1. 使用 try-catch 捕獲可能的異常
2. 在發生異常時設置錯誤標記 (_err = true)
3. 保存異常訊息以供上層應用使用
4. 出現異常時返回空值 (null)，避免未處理的異常向上傳播

此設計允許上層應用通過檢查 HasError 屬性來確定操作是否成功，並在需要時獲取錯誤訊息。

## 注意事項與限制

1. **資料庫依賴**：需要資料庫中存在 Company 表格，且包含 Company 和 CompanyName 欄位
2. **連接字串**：依賴 DBManger 類別提供的連接字串枚舉
3. **錯誤處理**：上層應用必須檢查 HasError 屬性，否則可能忽略錯誤情況
4. **功能限制**：目前僅提供查詢功能，不支援新增、修改或刪除公司資料

## 效能考量

1. **資料載入**：一次性載入所有公司資料，適合公司資料數量較少的情況
2. **排序處理**：在資料庫層面進行排序，減少應用程式的處理負擔
3. **資源管理**：未實現資源釋放（Dispose）方法，依賴於 .NET 垃圾回收機制
4. **查詢優化**：使用直接的 SQL 語句，減少複雜查詢帶來的效能開銷

## 安全性考量

1. **SQL 注入**：使用直接的 SQL 語句，但由於沒有外部參數輸入，風險較低
2. **資料庫權限**：依賴資料庫連接的權限設置，應確保適當的權限控制
3. **錯誤訊息**：錯誤訊息直接來自資料庫異常，可能包含敏感資訊，上層應用需注意處理

## 待改進事項

1. **參數化查詢**：考慮使用參數化查詢，提高安全性
2. **資源管理**：實現 IDisposable 接口，確保資源適當釋放
3. **查詢條件**：新增支援按條件查詢公司資料的方法
4. **資料操作**：新增公司資料的新增、修改和刪除功能
5. **快取機制**：考慮實現公司資料的快取機制，提高查詢效率
6. **日誌記錄**：加入操作日誌記錄功能，便於追蹤問題

## 相關檔案

1. **DBManger.cs**：提供資料庫連接和操作功能的管理類別
2. **Company 資料表**：資料庫中存儲公司資料的表格

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/18 | 1.0    | 首次建立公司資料管理規格書      | Claude AI  | 