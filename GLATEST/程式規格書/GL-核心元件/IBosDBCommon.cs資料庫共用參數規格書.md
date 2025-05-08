# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | IBosDBCommon.cs |
| 程式名稱 | 資料庫共用參數 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 共用元件 |
| 檔案位置 | GLATEST/GLATEST/App_Code/IBosDBCommon.cs |
| 程式類型 | 後端類別庫 |
| 建立日期 | 2025/6/10 |
| 建立人員 | Claude AI |
| 最後修改日期 | 2025/6/10 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

IBosDBCommon 類別是泛太總帳系統的資料庫共用參數管理類別，主要負責集中管理系統的資料庫連接字串及環境設定參數，提供系統統一的資料庫連接配置。此類別為整個系統提供了一個集中式的參數管理機制，確保所有應用程式元件能夠一致地存取資料庫，並在需要時便於更改資料庫設定。

### 2.2 業務流程

本程式在系統業務流程中扮演配置管理的角色：
1. 系統初始化時從配置檔讀取資料庫相關配置
2. IBosDBCommon類別提供靜態屬性供其他元件存取配置參數
3. 系統各模組通過此類別獲取資料庫連接字串
4. 確保系統中所有資料庫連接使用相同的配置

### 2.3 使用頻率

- 使用頻率：極高
- 使用時機：系統中每次需要資料庫連接時都會使用此類別獲取連接字串

### 2.4 使用者角色

- 系統開發人員：在程式中使用此類別存取數據庫配置
- 系統管理員：通過修改Web.config配置檔間接修改此類別提供的參數

## 3. 系統架構

### 3.1 技術架構

- 程式語言：C#
- 配置管理：ASP.NET Configuration
- 相依架構：.NET Framework 4.0

### 3.2 類別結構

IBosDBCommon 類別設計為一個純靜態類別，不需要實例化，直接通過類別名稱存取其靜態屬性。該類別與IBosDB類別密切相關，後者依賴此類別獲取資料庫連接字串。

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| System.Configuration | 配置管理 | 提供配置檔存取功能 |
| Web.config | 配置檔 | 存儲系統配置和連接字串 |

## 4. 程式內容

### 4.1 類別定義

```csharp
public class IBosDBCommon
{
    // 類別內容
}
```

### 4.2 屬性說明

| 屬性名稱 | 資料型態 | 說明 | 存取修飾詞 |
|---------|---------|------|-----------|
| GetEnvironmentType | int | 取得環境類型 | public static |
| GetEnvironmentTypeSQL | int | 取得SQL環境類型 | public static |
| DataLibraryName | string | AS400資料庫庫名稱 | public static |
| ApLibraryName | string | AS400應用庫名稱 | public static |
| ConnectionString | string | 資料庫連接字串 | public static |

### 4.3 方法說明

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|-------|------|
| IBosDBCommon() | 無 | 無 | 預設建構子 |

## 5. 處理邏輯

### 5.1 環境類型取得

GetEnvironmentType屬性從Web.config讀取ENVIRONMENT設定值，並轉換為整數：

```csharp
static public int GetEnvironmentType
{
    get
    {
        string sENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENT"];
        int iENVIRONMENT = 10;
        int.TryParse(sENVIRONMENT, out iENVIRONMENT);
        return iENVIRONMENT;
    }
}
```

### 5.2 SQL環境類型取得

GetEnvironmentTypeSQL屬性從Web.config讀取ENVIRONMENTSQL設定值，並轉換為整數：

```csharp
static public int GetEnvironmentTypeSQL
{
    get
    {
        string sENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENTSQL"];
        int iENVIRONMENT = 10;
        int.TryParse(sENVIRONMENT, out iENVIRONMENT);
        return iENVIRONMENT;
    }
}
```

### 5.3 AS400庫名稱

DataLibraryName屬性提供AS400資料庫庫名稱常量：

```csharp
static public string DataLibraryName
{
    get { return "ESPLPRDDTA"; }
}
```

### 5.4 AS400應用庫名稱

ApLibraryName屬性提供AS400應用庫名稱常量：

```csharp
public static string ApLibraryName
{
    get { return "ESPLPRDSRC"; }
}
```

### 5.5 資料庫連接字串取得

ConnectionString屬性從Web.config的ConnectionStrings節點讀取IBosDB連接字串：

```csharp
public static string ConnectionString
{
    get
    {
        string connstring = string.Empty;
        connstring = ConfigurationManager.ConnectionStrings["IBosDB"].ConnectionString;
        return connstring;
    }
}
```

## 6. 配置管理

### 6.1 配置檔結構

類別依賴Web.config中的以下配置節點：

```xml
<appSettings>
    <add key="ENVIRONMENT" value="10" />
    <add key="ENVIRONMENTSQL" value="10" />
</appSettings>

<connectionStrings>
    <add name="IBosDB" connectionString="Data Source=SERVER;Initial Catalog=DATABASE;User ID=USER;Password=PASSWORD;" providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 6.2 環境區分

類別通過ENVIRONMENT和ENVIRONMENTSQL參數實現不同環境(開發/測試/生產)的配置區分，便於部署時的環境切換。

## 7. 程式碼安全

### 7.1 敏感資訊管理

類別將敏感資訊(如資料庫連接字串)存儲在外部配置檔中，而非直接硬編碼在程式中，提高安全性。

### 7.2 唯讀設計

所有屬性設計為只讀(get-only)，防止運行時修改配置參數，增強系統穩定性。

## 8. 擴展性考慮

### 8.1 集中配置管理

將所有資料庫相關配置集中在一個類別中管理，便於未來擴展額外的配置項目。

### 8.2 靜態設計

使用靜態類別設計，確保系統中只有一份配置資訊，避免配置不一致的問題。

## 9. 維護建議

1. 定期檢查配置項目是否符合當前系統需求
2. 考慮將硬編碼的庫名常量(如ESPLPRDDTA)也移至配置檔管理
3. 在更改配置時確保系統所有相依組件同步更新
4. 為不同環境(開發、測試、生產)建立專用的配置檔，以便於配置管理和部署

## 10. 應用案例

### 10.1 在IBosDB類別中使用

IBosDB類別使用此類別取得連接字串：

```csharp 
string contring = IBosDBCommon.ConnectionString;
contring += ";Max Pool Size=300;";
this.Connection = new SqlConnection(contring);
```

### 10.2 在應用程式中直接使用

業務邏輯類別可以直接使用此類別判斷環境類型：

```csharp
if (IBosDBCommon.GetEnvironmentType == 10) // 生產環境
{
    // 生產環境特定邏輯
}
else // 開發或測試環境
{
    // 開發環境特定邏輯
}
```

## 11. 測試建議

### 11.1 單元測試

測試範圍應包含：
- 檢查各屬性是否能正確讀取配置值
- 測試不同配置條件下的系統行為
- 驗證連接字串格式的正確性

### 11.2 配置檔測試

- 模擬不同環境的配置檔，確保系統能正確切換環境
- 測試配置項缺失情況下的系統處理機制