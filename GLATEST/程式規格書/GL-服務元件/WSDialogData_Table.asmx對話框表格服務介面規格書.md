# WSDialogData_Table.asmx 對話框表格服務介面規格書

## 基本資訊

| 項目       | 內容                                   |
|------------|--------------------------------------|
| 程式代號     | WSDialogData_Table.asmx             |
| 程式名稱     | 對話框表格服務介面                     |
| 檔案大小     | 109B                                |
| 行數        | 2                                   |
| 功能簡述     | 提供對話框表格資料的Web服務介面         |
| 複雜度       | 低                                  |
| 規格書狀態   | 已完成                               |
| 負責人員     | Claude AI                          |
| 完成日期     | 2025/6/1                           |

## 程式功能概述

WSDialogData_Table.asmx 是泛太總帳系統中的對話框表格資料功能網路服務介面檔案，作為前端系統與後端對話框表格資料服務之間的橋樑。此檔案定義了 Web 服務的入口點，使客戶端可透過標準 SOAP 或 REST 協定呼叫對話框表格資料相關功能。

主要功能包括：

1. **服務定義**：定義 WSDialogData_Table 類別作為 Web 服務，讓系統能夠識別和調用
2. **介面入口**：提供前端系統訪問後端對話框表格資料功能的統一入口點
3. **協定支援**：支援 SOAP 和 JSON 格式的數據交換
4. **服務揭露**：使對話框表格資料功能能夠被前端頁面和第三方系統調用

WSDialogData_Table.asmx 與後端的 WSDialogData_Table 類別緊密整合，專門處理對話框中表格式資料的生成、格式轉換、排序與過濾等功能，是泛太總帳系統中對話框組件的重要支援服務。相較於基礎的對話框資料服務，此服務特別針對表格式數據提供更專業的處理功能。

## 檔案結構說明

WSDialogData_Table.asmx 採用了標準的 ASP.NET Web Service 檔案結構，包含了 Web 服務的宣告和指示。檔案雖然簡短，但其角色在系統架構中非常重要。

檔案內容包括：

```xml
<%@ WebService Language="C#" CodeBehind="~/App_Code/WSDialogData_Table.cs" Class="WSDialogData_Table" %>
```

此宣告包含以下關鍵元素：

1. **WebService 指示詞**：指定文件為 Web 服務
2. **Language 屬性**：指定使用 C# 語言
3. **CodeBehind 屬性**：指定實際的服務實現檔案（~/App_Code/WSDialogData_Table.cs）
4. **Class 屬性**：指定包含服務方法的完整類別名稱（WSDialogData_Table）

## 技術實現

WSDialogData_Table.asmx 利用以下技術實現其功能：

1. **ASP.NET Web Service**：
   - 使用 .NET Framework 的 Web 服務架構
   - 支援基於 SOAP 和 HTTP POST 的服務調用
   - 允許自動生成 WSDL 檔案以便客戶端集成

2. **服務描述**：
   - 自動為服務生成描述資訊
   - 提供服務探索功能
   - 允許通過瀏覽器訪問服務測試頁面

3. **資料表格處理**：
   - 專門處理表格式資料結構
   - 支援欄位映射和轉換
   - 提供資料分頁與排序功能

4. **通訊協定**：
   - 支援 SOAP 協定
   - 支援 HTTP GET/POST
   - 支援 JSON 序列化（通過特性配置）

## 相依關係

WSDialogData_Table.asmx 檔案與系統其他元件的相依關係：

1. **直接相依**：
   - **WSDialogData_Table.cs**：實現服務功能的程式碼檔案
   - **.NET Framework**：依賴 .NET Framework 的 Web 服務框架
   - **IIS**：依賴 Internet Information Services 進行託管

2. **間接相依**：
   - **IBosDB.cs**：數據訪問功能
   - **GLA資料庫**：對話框表格資料的資料來源
   - **System.Web.Services 命名空間**：提供 Web 服務基礎設施
   - **WSDialogData.asmx**：基礎對話框資料服務

3. **被以下元件使用**：
   - **ModelPopupDialog**：模態對話框控制項
   - **PopupDialog**：彈出對話框控制項
   - **GridView**：資料網格控制項
   - **DataTable**：資料表控制項
   - **所有需要表格式對話框功能的表單**

## 服務方法說明

WSDialogData_Table.asmx 服務介面透過其實作檔案 WSDialogData_Table.cs 提供以下主要方法：

1. **GetTableData**：
   - 功能：獲取指定對話框的表格資料
   - 參數：tableID（表格識別碼）、parameters（查詢參數）
   - 返回：表格結構的資料集合

2. **GetGridData**：
   - 功能：獲取用於網格控制項的資料
   - 參數：gridID（網格識別碼）、pageIndex（頁碼）、pageSize（頁大小）、sortExpression（排序表達式）
   - 返回：包含分頁和排序信息的網格資料

3. **ExportTableData**：
   - 功能：匯出表格資料為特定格式
   - 參數：tableID（表格識別碼）、exportFormat（匯出格式）、parameters（查詢參數）
   - 返回：匯出格式的資料或下載連結

4. **SaveTableData**：
   - 功能：儲存表格中的資料變更
   - 參數：tableID（表格識別碼）、dataChanges（資料變更內容）
   - 返回：儲存操作的結果和相關訊息

5. **GetTableStructure**：
   - 功能：獲取表格的結構定義
   - 參數：tableID（表格識別碼）
   - 返回：表格的欄位定義、關聯和約束資訊

## 使用範例

以下是如何在前端頁面中調用 WSDialogData_Table.asmx 服務的範例：

### 使用 jQuery AJAX 呼叫服務獲取表格資料

```javascript
// 使用 jQuery 呼叫對話框表格資料服務
function getTableData(tableId, params) {
    $.ajax({
        type: "POST",
        url: "WSDialogData_Table.asmx/GetTableData",
        data: JSON.stringify({
            tableID: tableId,
            parameters: params
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var tableData = response.d;
            renderTable(tableData);
        },
        error: function (xhr, status, error) {
            console.log("對話框表格服務錯誤: " + error);
        }
    });
}

// 渲染表格內容
function renderTable(tableData) {
    if (!tableData || !tableData.Columns || !tableData.Rows) {
        $("#tableContainer").html("<p>無資料可顯示</p>");
        return;
    }
    
    var table = $("<table class='data-table'></table>");
    
    // 添加表頭
    var thead = $("<thead></thead>");
    var headerRow = $("<tr></tr>");
    
    $.each(tableData.Columns, function(index, column) {
        headerRow.append("<th>" + column.DisplayName + "</th>");
    });
    
    thead.append(headerRow);
    table.append(thead);
    
    // 添加表體
    var tbody = $("<tbody></tbody>");
    
    $.each(tableData.Rows, function(rowIndex, row) {
        var dataRow = $("<tr></tr>");
        
        $.each(tableData.Columns, function(colIndex, column) {
            var cellValue = row[column.Name] || "";
            dataRow.append("<td>" + cellValue + "</td>");
        });
        
        tbody.append(dataRow);
    });
    
    table.append(tbody);
    $("#tableContainer").html(table);
}
```

### 使用 jQuery AJAX 呼叫服務獲取分頁網格資料

```javascript
// 使用 jQuery 呼叫對話框表格服務獲取網格資料
function getGridData(gridId, page, pageSize, sortExpr) {
    $.ajax({
        type: "POST",
        url: "WSDialogData_Table.asmx/GetGridData",
        data: JSON.stringify({
            gridID: gridId,
            pageIndex: page || 0,
            pageSize: pageSize || 10,
            sortExpression: sortExpr || ""
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var gridData = response.d;
            renderGrid(gridData);
            updatePagination(gridData.TotalRows, gridData.PageSize, gridData.CurrentPage);
        },
        error: function (xhr, status, error) {
            console.log("網格資料服務錯誤: " + error);
        }
    });
}

// 渲染網格內容
function renderGrid(gridData) {
    // 類似於renderTable函數，但包含分頁和排序功能
    // ...
}

// 更新分頁控制項
function updatePagination(totalRows, pageSize, currentPage) {
    var totalPages = Math.ceil(totalRows / pageSize);
    var pagination = $("#gridPagination");
    pagination.empty();
    
    // 添加頁碼按鈕
    for (var i = 1; i <= totalPages; i++) {
        var pageButton = $("<button class='page-button'>" + i + "</button>");
        
        if (i === currentPage + 1) {
            pageButton.addClass("current");
        }
        
        pageButton.click(function() {
            var page = parseInt($(this).text()) - 1;
            getGridData(currentGridId, page, pageSize, currentSortExpr);
        });
        
        pagination.append(pageButton);
    }
}
```

### 使用 ASP.NET AJAX 呼叫服務匯出表格資料

```javascript
// 使用 ASP.NET AJAX 呼叫對話框表格服務匯出資料
function exportTableData(tableId, format) {
    var params = {
        // 獲取用戶設定的查詢參數
        StartDate: $("#startDate").val(),
        EndDate: $("#endDate").val(),
        AccountCode: $("#accountCode").val()
    };
    
    WSDialogData_Table.ExportTableData(
        tableId,
        format,
        params,
        onExportSuccess,
        onServiceFailed
    );
}

// 處理匯出成功結果
function onExportSuccess(result) {
    if (result && result.DownloadUrl) {
        window.location.href = result.DownloadUrl;
    } else if (result.ErrorMessage) {
        showMessage("匯出失敗: " + result.ErrorMessage);
    }
}

// 處理服務錯誤
function onServiceFailed(error) {
    showMessage("服務錯誤: " + error.get_message());
}
```

### 在伺服器端代碼中配置服務

```html
<!-- 在 ASP.NET 頁面中添加 ScriptManager 和 ServiceReference -->
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Services>
        <asp:ServiceReference Path="~/WSDialogData_Table.asmx" />
    </Services>
</asp:ScriptManager>
```

## 安全性考量

WSDialogData_Table.asmx 服務在安全方面應考慮以下事項：

1. **輸入驗證**：
   - 所有前端傳入的數據需要進行嚴格的驗證
   - 應檢查輸入字串長度和格式，防止注入攻擊
   - 對表格相關參數進行合法性驗證

2. **授權控制**：
   - 服務方法應實施適當的權限檢查
   - 部分敏感數據表格可能需要限制特定角色才能訪問
   - 考慮使用 ASP.NET 內建的授權機制

3. **數據過濾**：
   - 表格數據應根據用戶權限進行過濾
   - 敏感欄位應在返回前進行適當處理
   - 確保用戶只能獲取有權限訪問的表格資料

4. **操作審計**：
   - 對表格資料的重要修改操作進行記錄
   - 保存修改前後的數據狀態
   - 提供完整的資料變更審計軌跡

5. **HTTPS 通訊**：
   - 建議配置 HTTPS 以加密數據傳輸
   - 保護敏感數據在網絡傳輸過程中的安全
   - 防止中間人攻擊

## 效能考量

為確保 WSDialogData_Table.asmx 服務具有良好的效能，應注意以下幾點：

1. **查詢最佳化**：
   - 對表格數據的查詢進行最佳化
   - 使用適當的索引支持快速檢索
   - 實施資料快取減少數據庫負載

2. **分頁處理**：
   - 所有表格數據都應實現有效的分頁機制
   - 避免一次性返回大量數據
   - 使用資料庫層級的分頁功能提高效率

3. **響應時間**：
   - 維持 300ms 以內的響應時間以獲得良好的用戶體驗
   - 監控服務性能，發現問題及時優化
   - 對於複雜表格查詢使用非同步處理

4. **數據序列化**：
   - 僅序列化必要的表格欄位
   - 避免傳輸不必要的元數據
   - 優化序列化過程減少CPU使用

5. **資源利用**：
   - 控制服務資源使用量
   - 釋放不再使用的系統資源
   - 限制並發請求數量防止服務過載

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/03/05 | 系統開發部 | 修正表格分頁功能                 |
| 1.1.0 | 2024/04/20 | 系統開發部 | 新增表格匯出功能                 |
| 1.1.1 | 2024/06/10 | 系統開發部 | 修正欄位排序問題                 |
| 1.2.0 | 2024/08/15 | 系統開發部 | 新增表格結構動態定義功能         |
| 1.2.1 | 2024/10/25 | 系統開發部 | 效能優化                         |
| 1.3.0 | 2025/01/20 | 系統開發部 | 新增表格資料批量更新功能         |
| 1.3.1 | 2025/04/05 | 系統開發部 | 整合資料驗證改進                 |

## 待改進事項

1. **功能擴展**：
   - 增加更多表格格式的匯出選項
   - 提供更靈活的表格結構定義機制
   - 支援複雜的資料篩選條件

2. **技術升級**：
   - 考慮遷移至更現代的 Web API 架構
   - 使用 RESTful 設計原則重構服務
   - 改進非同步處理能力

3. **使用者體驗**：
   - 改進表格資料的即時更新機制
   - 提供更友好的錯誤提示
   - 支援前端表格的快速編輯功能

4. **整合改進**：
   - 與前端框架（如Bootstrap Table）更好地整合
   - 提供更統一的表格格式定義
   - 簡化客戶端調用方式

5. **效能提升**：
   - 實現更高效的大表格處理機制
   - 優化表格數據傳輸格式
   - 強化快取策略減少伺服器負載 