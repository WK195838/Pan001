# ModelPopupDialog.aspx.cs 模態對話框程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | ModelPopupDialog.aspx.cs              |
| 程式名稱     | 模態對話框程式碼                          |
| 檔案大小     | 4.5KB                                 |
| 行數        | ~139                                  |
| 功能簡述     | 模態視窗後端邏輯                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/17                             |

## 程式功能概述

ModelPopupDialog.aspx.cs 是泛太總帳系統中模態對話框元件的後端程式碼檔案，負責處理模態視窗的資料載入、事件處理和資料選擇邏輯。此程式碼實現了模態對話框的核心功能，包括資料查詢、動態SQL構建、資料顯示和結果回傳等。主要功能包括：

1. 處理對話框頁面的初始化和載入
2. 根據URL參數動態構建SQL查詢語句
3. 從資料庫查詢資料並繫結到GridView控制項
4. 處理使用者在對話框中的選擇和搜尋操作
5. 設定資料列的樣式和互動效果
6. 實現資料選擇的回傳機制
7. 優化用戶體驗，如滑鼠懸停效果和游標樣式

此程式碼與ModelPopupDialog.aspx頁面搭配，提供完整的模態對話框解決方案，廣泛應用於系統中需要使用者強制選擇資料的場景。

## 技術實現

ModelPopupDialog.aspx.cs 主要基於以下技術實現：

1. **ASP.NET Web Forms**：使用 System.Web.UI 命名空間的類別實現Web頁面功能
2. **C# 程式語言**：使用 C# 實現後端邏輯
3. **ADO.NET**：使用 SqlDataAdapter 處理資料存取
4. **動態SQL**：根據URL參數動態構建SQL查詢語句
5. **GridView 控制項**：使用資料網格顯示查詢結果
6. **事件處理**：實現各種頁面和控制項事件的處理
7. **用戶端整合**：設定資料列的客戶端事件處理

## 命名空間與參考

ModelPopupDialog.aspx.cs 使用以下命名空間：

```csharp
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
```

## 類別結構

ModelPopupDialog 類別繼承自 System.Web.UI.Page 類別，提供模態對話框頁面的功能：

```csharp
public partial class ModelPopupDialog : System.Web.UI.Page 
{
    // 成員變數和方法
}
```

### 成員變數

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| cnn | string | 資料庫連接字串 |
| _UserInfo | UserInfo | 使用者資訊物件 |
| _ProgramId | string | 程式識別碼 |
| _MyDBM | DBManger | 資料庫管理器實例 |

## 方法說明

### 頁面事件處理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Page_Load | object sender, EventArgs e | void | 頁面載入事件處理方法，初始化資料庫連接並呼叫 BindData 方法 |
| BindData | 無 | void | 根據URL參數構建SQL查詢語句並繫結資料到GridView |
| gvEmployee_RowCreated | object sender, GridViewRowEventArgs e | void | 處理資料列建立事件，設定資料列樣式和屬性 |
| gvEmployee_RowDataBound | object sender, GridViewRowEventArgs e | void | 處理資料列繫結事件，設定選擇功能和回傳值 |
| imgbtnQuery_Click | object sender, ImageClickEventArgs e | void | 處理查詢按鈕點擊事件，重新繫結資料 |

### Page_Load 方法

Page_Load 方法負責初始化頁面，主要包括：

1. 建立資料庫管理器實例
2. 初始化資料庫連接
3. 呼叫 BindData 方法載入資料

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    _MyDBM = new DBManger();
    _MyDBM.New();      

    cnn = _MyDBM.GetConnectionString(); 
    BindData();
}
```

### BindData 方法詳細說明

BindData 方法負責根據URL參數構建SQL查詢語句並繫結資料，主要流程為：

1. 從URL參數取得表名、欄位和排序依據
2. 構建動態SQL查詢語句
3. 設定SqlDataSource的連接字串和查詢語句
4. 繫結資料到GridView控制項

```csharp
private void BindData()
{
    string sql = "SELECT";
    sql += " " + Request["Fields"];
    sql += " FROM";
    sql += " " + Request["Table"];
    sql += " ORDER BY";
    sql += " " + Request["Key"];
    
    try
    {
        SqlDataSource1.ConnectionString = cnn;
        SqlDataSource1.SelectCommand = sql;
        SqlDataSource1.DataBind();

        gvEmployee.DataBind();
    }
    catch (InvalidCastException e)
    {
        throw (e);
    }
    finally
    {
        // 資源釋放程式碼
    }
}
```

## GridView 事件處理

ModelPopupDialog.aspx.cs 使用 GridView 控制項顯示資料，並實現資料列樣式和選擇功能：

### gvEmployee_RowCreated 方法

此方法用於處理資料列建立事件，主要設定資料列的樣式和互動效果：

1. 定義滑鼠事件樣式
2. 設定資料儲存格的CSS類別
3. 加入滑鼠懸停和離開效果
4. 設定游標樣式為手形，表示可點擊

```csharp
protected void gvEmployee_RowCreated(object sender, GridViewRowEventArgs e)
{
    string OnMouseOverStyle = "this.style.backgroundColor='silver';";
    string OnMouseOutStyle = "this.style.backgroundColor='@BackColor';";
    string rowBackColor = "";
    
    if (e.Row.RowType == DataControlRowType.DataRow)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            e.Row.Cells[i].CssClass = "Grid_GridLine";
        }

        e.Row.Attributes.Add("onmouseover", OnMouseOverStyle);
        e.Row.Attributes.Add("onmouseout", OnMouseOutStyle.Replace("@BackColor", rowBackColor));
        e.Row.Attributes.Add("style", "cursor:hand;");
    }
}
```

### gvEmployee_RowDataBound 方法

此方法用於處理資料列繫結事件，主要設定標題列樣式和資料列選擇功能：

1. 設定標題列的字體樣式
2. 組合資料列的前兩個儲存格的值，作為回傳值
3. 設定點擊事件，調用客戶端函數 ReValue，傳回所選資料

```csharp
protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
{
    if (e.Row.RowType == DataControlRowType.Header)
    {
        foreach (TableCell cell in e.Row.Cells)
        {
            cell.Attributes.Add("style", "FONT-WEIGHT:normal;");
        }
    }
    else if (e.Row.RowType == DataControlRowType.DataRow)
    {
        string rValue = "";
        for (int i = 0; i <= 1; i++)
        {
            rValue += e.Row.Cells[i].Text.Trim() + ",";
        }

        e.Row.Attributes.Add("onclick", "return ReValue('" + rValue + "');");
    }
}
```

## 查詢功能

imgbtnQuery_Click 方法處理查詢按鈕點擊事件，重新載入資料：

```csharp
protected void imgbtnQuery_Click(object sender, ImageClickEventArgs e)
{
    BindData();
}
```

## 程式碼流程

1. 頁面載入時，觸發 Page_Load 事件
2. Page_Load 初始化資料庫連接並呼叫 BindData 方法
3. BindData 根據URL參數構建SQL語句並載入資料
4. 資料繫結到 GridView 控制項
5. GridView 的資料列事件處理用於設定視覺效果和選擇功能
6. 使用者點擊資料列時，觸發客戶端函數回傳選擇值
7. 使用者點擊查詢按鈕時，重新載入資料

## 使用情境與互動方式

ModelPopupDialog.aspx.cs 處理以下互動情境：

1. **初始載入**：
   - 頁面載入時自動從資料庫載入資料
   - 根據 URL 參數動態構建SQL查詢語句

2. **資料查詢**：
   - 使用者輸入查詢條件，點擊查詢按鈕
   - 後端重新載入符合條件的資料

3. **資料選擇**：
   - 使用者點擊GridView中的資料列
   - 系統將選中的資料作為回傳值傳遞給父頁面
   - 父頁面獲取選擇結果並進行後續處理

## 資料結構

### 輸入參數

| 參數名稱 | 資料來源 | 說明 |
|---------|---------|------|
| Table | URL 參數 | 要查詢的資料表名稱 |
| Fields | URL 參數 | 要顯示的欄位清單 |
| Key | URL 參數 | 排序依據的欄位名稱 |

### 輸出資料

| 資料名稱 | 資料類型 | 說明 |
|---------|---------|------|
| 回傳值 | 字串 | 選中資料行的前兩個儲存格值，逗號分隔 |

## 異常處理

程式包含以下異常處理：

1. 使用 try-catch-finally 結構處理資料載入過程中的異常
2. 捕獲 InvalidCastException 並重新擲出，讓上層處理

```csharp
try
{
    // 資料載入程式碼...
}
catch (InvalidCastException e)
{
    throw (e);
}
finally
{
    // 清理資源...
}
```

## 注意事項與限制

1. **SQL注入風險**：動態構建SQL語句未使用參數化查詢，可能存在SQL注入風險
2. **錯誤處理**：只捕獲了 InvalidCastException，可能需要更全面的錯誤處理
3. **資源管理**：程式註釋掉了資源釋放程式碼，可能導致資源洩漏
4. **回傳機制**：回傳值使用逗號分隔的字串，若資料中包含逗號可能導致解析錯誤
5. **URL參數驗證**：缺乏對URL參數的驗證，可能導致安全問題

## 效能考量

1. **查詢優化**：直接使用資料庫原生排序提高效率
2. **分頁支援**：使用GridView的分頁功能減少一次載入的資料量
3. **資源釋放**：應考慮適當釋放資料庫相關資源
4. **用戶端處理**：點擊事件直接在用戶端處理，減少服務器負擔

## 安全性考量

1. **SQL注入預防**：應改用參數化查詢防止SQL注入
2. **參數驗證**：應增加對URL參數的驗證
3. **權限檢查**：考慮增加權限檢查，確保只有授權用戶可執行特定操作
4. **資料過濾**：確保只返回用戶有權訪問的資料

## 待改進事項

1. **參數化查詢**：改用參數化查詢防止SQL注入
2. **參數驗證**：增加對URL參數的驗證和過濾
3. **錯誤處理**：增加更全面的例外捕獲與處理
4. **資源管理**：添加適當的資源釋放程式碼
5. **回傳機制**：改善資料回傳機制，使用JSON格式提高相容性和安全性
6. **查詢條件**：增強查詢功能，支援更複雜的查詢條件
7. **多欄位選擇**：支援動態設定可選擇和回傳的欄位數量

## 相關檔案

1. **ModelPopupDialog.aspx**：對話框的ASPX頁面文件
2. **DBManger**：資料庫管理類別，提供資料庫連接功能
3. **UserInfo**：使用者資訊類別，存儲用戶相關資訊

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/17 | 1.0    | 首次建立模態對話框程式碼規格書         | Claude AI  |
``` 
</rewritten_file>