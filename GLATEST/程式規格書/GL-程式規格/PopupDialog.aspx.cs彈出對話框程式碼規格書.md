# PopupDialog.aspx.cs 彈出對話框程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | PopupDialog.aspx.cs                   |
| 程式名稱     | 彈出對話框程式碼                          |
| 檔案大小     | 3.9KB                                 |
| 行數        | ~125                                  |
| 功能簡述     | 彈出視窗後端邏輯                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/16                             |

## 程式功能概述

PopupDialog.aspx.cs 是泛太總帳系統中彈出對話框元件的後端程式碼檔案，負責處理彈出視窗的資料載入、事件處理和業務邏輯。此程式碼文件實現了對話框的後端功能，包括資料繫結、選擇項目處理、參數傳遞和結果回傳等核心功能。主要功能包括：

1. 處理對話框頁面的初始化和載入
2. 從資料庫動態載入對話框內容資料
3. 處理用戶在對話框中的選擇和操作
4. 實現對話框的確認和取消按鈕邏輯
5. 管理對話框與父頁面間的資料傳遞和結果回傳
6. 提供自訂對話框屬性和動態調整機制
7. 處理對話框事件和例外狀況

此程式碼與PopupDialog.aspx頁面搭配，提供完整的彈出對話框解決方案，廣泛應用於系統中需要彈出視窗的各種場景。

## 技術實現

PopupDialog.aspx.cs 主要基於以下技術實現：

1. **ASP.NET Web Forms**：使用 System.Web.UI 命名空間的類別實現Web頁面功能
2. **C# 程式語言**：使用 C# 實現後端邏輯
3. **ADO.NET**：使用 SqlDataAdapter、DataTable 等類別處理資料存取
4. **Stored Procedure 呼叫**：使用預存程序執行資料庫操作
5. **GridView 控制項**：使用資料網格顯示查詢結果
6. **事件處理**：實現各種頁面和控制項事件的處理
7. **用戶端腳本整合**：使用 ClientScript 註冊客戶端腳本

## 命名空間與參考

PopupDialog.aspx.cs 使用以下命名空間：

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

PopupDialog 類別繼承自 System.Web.UI.Page 類別，提供彈出對話框頁面的功能：

```csharp
public partial class PopupDialog : System.Web.UI.Page 
{
    // 成員變數和方法
}
```

### 成員變數

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _MyDBM | DBManger | 資料庫管理器實例 |
| cnn | string | 資料庫連接字串 |

## 方法說明

### 頁面事件處理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Page_Load | object sender, EventArgs e | void | 頁面載入事件處理方法，呼叫 BindData 進行資料繫結 |
| BindData | 無 | void | 從資料庫載入資料並繫結到 GridView |
| gvEmployee_RowCreated | object sender, GridViewRowEventArgs e | void | 處理資料列建立事件，設定資料列樣式和屬性 |
| gvEmployee_RowDataBound | object sender, GridViewRowEventArgs e | void | 處理資料列繫結事件，設定選擇功能和回傳值 |
| imgbtnQuery_Click | object sender, ImageClickEventArgs e | void | 處理查詢按鈕點擊事件，重新繫結資料 |

### BindData 方法詳細說明

BindData 方法負責從資料庫載入對話框所需的資料，主要流程為：

1. 建立資料庫管理器實例
2. 取得資料庫連接字串
3. 執行預存程序 sp_GetAcnoIdxPrompt，傳入必要參數
4. 取得結果中的 SQL 查詢語句
5. 設定 SqlDataSource 的連接字串和查詢語句
6. 繫結資料到 GridView 控制項

```csharp
private void BindData()
{
    _MyDBM= new DBManger();
    _MyDBM.New();

    cnn = _MyDBM.GetConnectionString();
    string sql = "exec dbo.sp_GetAcnoIdxPrompt @Company,@CodeID,@CodeCode";
    DataTable dt = new DataTable();
    SqlDataAdapter adpt = new SqlDataAdapter(sql, cnn); 
    try
    {
        adpt.SelectCommand.Parameters.Add("@Company", SqlDbType.Char, 2).Value = Request["Company"];
        adpt.SelectCommand.Parameters.Add("@CodeID", SqlDbType.Char, 2).Value = Request["CodeID"];
        adpt.SelectCommand.Parameters.Add("@CodeCode", SqlDbType.VarChar, 10).Value = txtQuery.Text.Trim();
        
        adpt.Fill(dt);
        string sqlReturn = dt.Rows[0]["SQLString"].ToString();

        SqlDataSource1.ConnectionString = cnn;
        SqlDataSource1.SelectCommand = sqlReturn;
        SqlDataSource1.DataBind();

        gvEmployee.DataBind();
    }
    catch (InvalidCastException e)
    {
        throw (e);
    }
    finally
    {
        //dt.Dispose();
        //adpt.Dispose();
    }
}
```

## GridView 事件處理

PopupDialog 使用 GridView 控制項顯示資料，並實現資料列樣式和選擇功能：

### gvEmployee_RowCreated 方法

此方法用於處理資料列建立事件，主要設定資料列的樣式和互動效果：

1. 定義滑鼠事件樣式
2. 設定資料儲存格的 CSS 類別和對齊方式
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
            e.Row.Cells[i].Style.Add("text-align", "left");
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
            rValue += e.Row.Cells[i].Text.ToString().Trim() + ",";
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
2. Page_Load 呼叫 BindData 方法從資料庫載入資料
3. BindData 執行預存程序並取得資料
4. 資料繫結到 GridView 控制項
5. GridView 的資料列事件處理用於設定視覺效果和選擇功能
6. 使用者點擊資料列時，觸發客戶端函數回傳選擇值
7. 使用者點擊查詢按鈕時，重新載入資料

## 使用情境與互動方式

PopupDialog.aspx.cs 處理以下互動情境：

1. **初始載入**：
   - 頁面載入時自動從資料庫載入資料
   - 根據 URL 參數 Company 和 CodeID 過濾資料

2. **資料查詢**：
   - 使用者輸入查詢條件，點擊查詢按鈕
   - 後端重新從資料庫載入過濾後的資料

3. **資料選擇**：
   - 使用者點擊資料列
   - 後端將選中的資料作為回傳值傳遞給父頁面
   - 父頁面取得選擇結果並更新相關欄位

## 資料結構

### 輸入參數

| 參數名稱 | 資料來源 | 說明 |
|---------|---------|------|
| Company | URL 參數 | 公司代碼 |
| CodeID | URL 參數 | 代碼類型 |
| CodeCode | 文字輸入框 | 查詢條件 |

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

1. 資源處理：程式註釋掉了 DataTable 和 SqlDataAdapter 的釋放程式碼，應考慮適當釋放資源以避免記憶體洩漏
2. 錯誤處理：只捕獲了 InvalidCastException，可能需要更全面的錯誤處理
3. 資料安全：從請求取得參數時未進行充分的驗證，可能存在安全風險
4. 回傳機制：回傳值使用逗號分隔的字串，可能存在解析問題，特別是當資料本身包含逗號時

## 效能考量

1. **資料載入效率**：使用預存程序提高資料庫查詢效率
2. **資源釋放**：注意適當釋放資料庫相關資源
3. **資料量限制**：考慮在資料量大時增加分頁功能
4. **用戶端互動**：點擊事件直接在用戶端處理，減少服務器負擔

## 安全性考量

1. **SQL 注入預防**：使用參數化查詢防止 SQL 注入
2. **輸入驗證**：應增加對輸入參數的驗證
3. **權限檢查**：考慮增加權限檢查，確保只有授權用戶可執行特定操作
4. **資料過濾**：確保只返回用戶有權訪問的資料

## 待改進事項

1. 增加資源適當釋放機制，避免記憶體洩漏
2. 改進錯誤處理，增加更全面的例外捕獲與處理
3. 增加資料分頁功能，處理大量資料的情況
4. 改善資料回傳機制，使用 JSON 格式提高相容性
5. 增加更完善的輸入驗證和安全檢查
6. 考慮增加快取機制，提高重複查詢的效率
7. 改進用戶介面互動，提供更好的用戶體驗

## 相關檔案

1. **PopupDialog.aspx**：對話框的 ASPX 頁面文件
2. **ModPopFunction.js**：提供對話框用戶端功能的 JavaScript 檔案
3. **dialog.css**：對話框樣式表文件
4. **sp_GetAcnoIdxPrompt**：資料庫預存程序，提供資料查詢功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/16 | 1.0    | 首次建立彈出對話框程式碼規格書         | Claude AI  | 