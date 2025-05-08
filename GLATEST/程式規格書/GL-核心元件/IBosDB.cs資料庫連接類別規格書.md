# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | IBosDB.cs |
| 程式名稱 | 資料庫連接類別 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 共用元件 |
| 檔案位置 | GLATEST/GLATEST/App_Code/IBosDB.cs |
| 程式類型 | 後端類別庫 |
| 建立日期 | 2025/6/10 |
| 建立人員 | Claude AI |
| 最後修改日期 | 2025/6/10 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

IBosDB 類別是泛太總帳系統的核心資料庫連接類別，主要負責建立、管理與SQL Server資料庫的連接，以及執行SQL指令並返回查詢結果。此類別繼承自基礎資料庫類別(DBClassBass)，提供了一個統一的資料存取介面，允許應用程式的各個模組以一致的方式與資料庫交互。

### 2.2 業務流程

本程式在系統業務流程中扮演關鍵的資料存取層角色：
1. 應用程式層發起資料庫操作請求
2. IBosDB類別建立資料庫連接
3. 執行SQL指令(查詢、新增、修改、刪除)
4. 返回操作結果至應用程式層
5. 管理連接資源釋放

### 2.3 使用頻率

- 使用頻率：極高
- 使用時機：系統中所有需要資料庫存取的操作都會使用此類別

### 2.4 使用者角色

- 系統開發人員：在程式中使用此類別執行資料庫操作
- 系統管理員：透過了解此類別的功能進行系統維護與疑難排解

## 3. 系統架構

### 3.1 技術架構

- 程式語言：C#
- 資料庫連接技術：ADO.NET
- 資料庫：SQL Server
- 相依架構：.NET Framework 4.0

### 3.2 類別結構

IBosDB 類別繼承自 DBClassBass 類別，並與以下類別相互關聯：
- DBClassBass：基礎資料庫類別，提供共用資料庫操作功能
- IBosDBCommon：提供資料庫連接字串等共用參數

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| System.Data | 資料存取 | 提供資料存取功能 |
| System.Data.SqlClient | SQL Server連接 | 提供SQL Server資料庫連接 |
| IBosDBCommon | 共用參數管理 | 提供資料庫連接字串等設定 |
| DBClass.DBClassBass | 基礎資料庫類別 | 提供基礎資料庫操作功能 |

## 4. 程式內容

### 4.1 類別定義

```csharp
public class IBosDB : DBClassBass
{
    // 類別內容
}
```

### 4.2 屬性說明

| 屬性名稱 | 資料型態 | 說明 | 存取修飾詞 |
|---------|---------|------|-----------|
| Connection | SqlConnection | 資料庫連接物件 | 繼承自基底類別 |
| _err | bool | 錯誤狀態標記 | 繼承自基底類別 |
| _errMsg | string | 錯誤訊息 | 繼承自基底類別 |

### 4.3 方法說明

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|-------|------|
| IBosDB() | 無 | 無 | 預設建構子，初始化資料庫連接 |
| IBosDB(object instance) | instance: 物件實例 | 無 | 帶參數建構子，初始化資料庫連接 |
| GetSqlDataSet | cmd: SqlCommand | DataSet | 執行SQL指令並返回DataSet結果集 |

## 5. 處理邏輯

### 5.1 資料庫連接初始化

在建構子中，類別執行以下步驟初始化資料庫連接：
1. 設定錯誤狀態標記(_err)為false
2. 清空錯誤訊息(_errMsg)
3. 從IBosDBCommon取得資料庫連接字串
4. 附加連接池設定(Max Pool Size=300)
5. 初始化SqlConnection物件

```csharp
public IBosDB()
{
    _err = false;
    _errMsg = "";
    string contring = IBosDBCommon.ConnectionString;
    contring += ";Max Pool Size=300;";
    this.Connection = new SqlConnection(contring);
}
```

### 5.2 資料查詢操作

GetSqlDataSet方法執行以下步驟查詢資料：
1. 從IBosDBCommon取得資料庫連接字串
2. 創建SqlConnection物件
3. 創建SqlDataAdapter物件
4. 設定SqlCommand的Connection屬性
5. 設定SqlDataAdapter的SelectCommand屬性
6. 創建DataSet物件
7. 使用SqlDataAdapter填充DataSet
8. 返回填充後的DataSet

```csharp
public DataSet GetSqlDataSet(SqlCommand cmd)
{
    string cnn = IBosDBCommon.ConnectionString;
    using (SqlConnection con = new SqlConnection(cnn))
    {
        using (SqlDataAdapter sda = new SqlDataAdapter())
        {
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            using (DataSet ds = new DataSet())
            {
                sda.Fill(ds);
                return ds;
            }
        }
    }
}
```

## 6. 資源管理

### 6.1 連接池設定

類別使用ADO.NET連接池技術管理資料庫連接，設定最大連接池大小為300：
```csharp
contring += ";Max Pool Size=300;";
```

### 6.2 資源釋放

類別使用C#的using語句確保資源正確釋放：
```csharp
using (SqlConnection con = new SqlConnection(cnn))
{
    // 使用連接
} // 此處連接自動釋放
```

## 7. 錯誤處理

### 7.1 錯誤標記

類別使用_err和_errMsg屬性跟踪錯誤狀態和訊息：
```csharp
_err = false;
_errMsg = "";
```

### 7.2 繼承錯誤處理

IBosDB類別繼承基底類別DBClassBass的錯誤處理機制，應用程式可以查詢錯誤狀態判斷操作是否成功。

## 8. 安全性考慮

### 8.1 連接字串管理

連接字串存儲在Web.config配置檔中，而非直接硬編碼在程式中：
```csharp
string contring = IBosDBCommon.ConnectionString;
```

### 8.2 SQL注入防護

類別使用參數化查詢方式執行SQL指令，應用程式需傳入SqlCommand物件而非直接的SQL字串，減少SQL注入風險。

## 9. 效能優化

### 9.1 連接池

類別使用ADO.NET連接池技術優化資料庫連接管理，設定最大連接池大小為300，適合高並發場景。

### 9.2 資源管理

類別使用using語句確保資源及時釋放，避免資源洩漏問題。

## 10. 擴展性考慮

### 10.1 繼承架構

IBosDB繼承自DBClassBass基礎類別，便於將來擴展其他資料庫功能。

### 10.2 通用設計

類別設計為通用資料訪問接口，可支持系統中各種不同的資料庫操作需求。

## 11. 測試建議

### 11.1 單元測試

測試範圍應包含：
- 連接建立測試
- 查詢操作測試
- 錯誤處理測試
- 連接池效能測試

### 11.2 整合測試

與系統其他模組整合測試，確保資料庫操作正確執行且符合業務需求。

## 12. 維護建議

1. 定期檢查連接池設定是否符合當前系統負載
2. 監控資料庫連接使用情況，防止連接洩漏
3. 若系統擴展，考慮實作讀寫分離策略
4. 考慮添加更詳細的日誌記錄機制追蹤資料庫操作 