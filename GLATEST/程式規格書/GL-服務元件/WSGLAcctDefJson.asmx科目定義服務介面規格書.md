# WSGLAcctDefJson.asmx 科目定義服務介面規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | WSGLAcctDefJson.asmx                 |
| 程式名稱     | 科目定義服務介面                        |
| 檔案大小     | 103B                                 |
| 行數        | 2                                    |
| 功能簡述     | 提供科目定義功能的Web服務介面            |
| 複雜度       | 低                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/30                           |

## 程式功能概述

WSGLAcctDefJson.asmx 是泛太總帳系統中的科目定義功能網路服務介面檔案，作為前端系統與後端科目定義服務之間的橋樑。此檔案定義了 Web 服務的入口點，使客戶端可透過標準 SOAP 或 REST 協定呼叫科目定義相關功能，並使用 JSON 格式傳輸資料。

主要功能包括：

1. **服務定義**：定義 WSGLAcctDefJson 類別作為 Web 服務，讓系統能夠識別和調用
2. **介面入口**：提供前端系統訪問後端科目定義功能的統一入口點
3. **JSON 支援**：特別支援 JSON 格式的資料交換，提高與現代前端框架的整合能力
4. **服務揭露**：使科目定義功能能夠被前端頁面和第三方系統調用

WSGLAcctDefJson.asmx 與後端的 WSGLAcctDefJson 類別緊密整合，提供了科目結構定義、科目編碼規則管理、科目屬性配置等功能，是泛太總帳系統科目體系管理的重要組件。

## 檔案結構說明

WSGLAcctDefJson.asmx 採用了標準的 ASP.NET Web Service 檔案結構，包含了 Web 服務的宣告和指示。檔案雖然簡短，但其角色在系統架構中非常重要。

檔案內容包括：

```xml
<%@ WebService Language="C#" CodeBehind="WSGLAcctDefJson.asmx.cs" Class="GLA.WSGLAcctDefJson" %>
```

此宣告包含以下關鍵元素：

1. **WebService 指示詞**：指定文件為 Web 服務
2. **Language 屬性**：指定使用 C# 語言
3. **CodeBehind 屬性**：指定實際的服務實現檔案（WSGLAcctDefJson.asmx.cs）
4. **Class 屬性**：指定包含服務方法的完整類別名稱（GLA.WSGLAcctDefJson）

## 技術實現

WSGLAcctDefJson.asmx 利用以下技術實現其功能：

1. **ASP.NET Web Service**：
   - 使用 .NET Framework 的 Web 服務架構
   - 支援基於 SOAP 和 HTTP POST 的服務調用
   - 允許自動生成 WSDL 檔案以便客戶端集成

2. **JSON 序列化**：
   - 透過 ScriptService 特性啟用 JSON 傳輸
   - 使用 JavaScriptSerializer 進行物件序列化
   - 支援複雜科目結構的 JSON 表示

3. **服務配置**：
   - 設定為支援跨網域請求 (CORS)
   - 配置強類型資料傳輸模型
   - 最佳化序列化設定提高效能

4. **通訊協定**：
   - 特別強化 JSON 通訊協定支援
   - 支援 HTTP GET/POST
   - 提供 RESTful 風格的API端點

## 相依關係

WSGLAcctDefJson.asmx 檔案與系統其他元件的相依關係：

1. **直接相依**：
   - **WSGLAcctDefJson.asmx.cs**：實現服務功能的程式碼檔案
   - **.NET Framework**：依賴 .NET Framework 的 Web 服務框架
   - **IIS**：依賴 Internet Information Services 進行託管

2. **間接相依**：
   - **IBosDB.cs**：數據訪問功能
   - **GLA資料庫**：科目定義資料的來源和存儲
   - **System.Web.Services 命名空間**：提供 Web 服務基礎設施
   - **System.Web.Script.Services 命名空間**：提供 JSON 序列化支持

3. **被以下元件使用**：
   - **PTA0150**：會計科目維護頁面
   - **PTA0150_M**：會計科目查詢頁面
   - **Subjects 控制項**：科目選擇控制項
   - **GLR0160**：會計科目查詢報表
   - **所有需要科目定義的頁面和報表**

## 服務方法說明

WSGLAcctDefJson.asmx 服務介面透過其實作檔案 WSGLAcctDefJson.asmx.cs 提供以下主要方法：

1. **GetAccountDefinition**：
   - 功能：獲取特定科目的完整定義
   - 參數：accountCode（科目代碼）、companyID（公司代碼）
   - 返回：包含科目定義的 JSON 物件，包括科目屬性、層級、分類等

2. **GetAccountList**：
   - 功能：獲取符合指定條件的科目列表
   - 參數：filter（過濾條件）、companyID（公司代碼）
   - 返回：科目列表，包含基本科目資訊

3. **GetAccountStructure**：
   - 功能：獲取公司科目結構定義
   - 參數：companyID（公司代碼）
   - 返回：描述科目結構的 JSON 物件，包含層級、分隔符等

4. **SaveAccountDefinition**：
   - 功能：保存科目定義
   - 參數：accountDef（科目定義對象）、companyID（公司代碼）
   - 返回：操作結果，包含成功/失敗狀態和訊息

5. **ValidateAccountCode**：
   - 功能：驗證科目代碼是否符合規則
   - 參數：accountCode（科目代碼）、companyID（公司代碼）
   - 返回：驗證結果，包含有效性和錯誤訊息

6. **GetAccountHierarchy**：
   - 功能：獲取科目層級結構
   - 參數：rootCode（根科目代碼，可選）、companyID（公司代碼）
   - 返回：科目層級結構的樹狀表示

7. **GetAccountAttributes**：
   - 功能：獲取科目可設定的屬性
   - 參數：companyID（公司代碼）
   - 返回：科目屬性定義列表

8. **GetDefaultAccounts**：
   - 功能：獲取系統預設科目列表
   - 參數：companyID（公司代碼）
   - 返回：預設科目列表，包含科目用途和預設值

## 使用範例

以下是如何在前端頁面中調用 WSGLAcctDefJson.asmx 服務的範例：

### 使用 jQuery AJAX 獲取科目定義

```javascript
// 使用 jQuery 呼叫科目定義服務獲取特定科目
function getAccountDefinition(accountCode) {
    $.ajax({
        type: "POST",
        url: "WSGLAcctDefJson.asmx/GetAccountDefinition",
        data: JSON.stringify({
            accountCode: accountCode,
            companyID: $("#hidCompanyId").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var accountDef = response.d;
            displayAccountDefinition(accountDef);
        },
        error: function (xhr, status, error) {
            console.log("科目定義服務錯誤: " + error);
        }
    });
}

// 顯示科目定義
function displayAccountDefinition(accountDef) {
    $("#accountCodeLabel").text(accountDef.AccountCode);
    $("#accountNameLabel").text(accountDef.AccountName);
    $("#accountTypeLabel").text(getAccountTypeText(accountDef.AccountType));
    $("#accountLevelLabel").text(accountDef.Level);
    
    // 顯示科目屬性
    var attributesContainer = $("#attributesContainer");
    attributesContainer.empty();
    
    $.each(accountDef.Attributes, function(key, value) {
        attributesContainer.append(
            "<div class='attribute-item'>" +
            "<span class='attribute-name'>" + key + ":</span> " +
            "<span class='attribute-value'>" + value + "</span>" +
            "</div>"
        );
    });
}

// 獲取科目類型文字說明
function getAccountTypeText(typeCode) {
    var typeMap = {
        "A": "資產",
        "L": "負債",
        "E": "權益",
        "R": "收入",
        "C": "成本",
        "X": "費用"
    };
    
    return typeMap[typeCode] || "未知";
}
```

### 使用 jQuery AJAX 驗證科目代碼

```javascript
// 使用 jQuery 呼叫科目定義服務驗證科目代碼
function validateAccountCode(accountCode) {
    $.ajax({
        type: "POST",
        url: "WSGLAcctDefJson.asmx/ValidateAccountCode",
        data: JSON.stringify({
            accountCode: accountCode,
            companyID: $("#hidCompanyId").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            if (result.IsValid) {
                $("#accountCodeInput").removeClass("invalid").addClass("valid");
                $("#accountCodeError").hide();
            } else {
                $("#accountCodeInput").removeClass("valid").addClass("invalid");
                $("#accountCodeError").text(result.ErrorMessage).show();
            }
        },
        error: function (xhr, status, error) {
            $("#accountCodeError").text("驗證服務錯誤: " + error).show();
        }
    });
}
```

### 使用 ASP.NET AJAX 獲取科目層級結構

```javascript
// 使用 ASP.NET AJAX 呼叫科目定義服務獲取科目層級結構
function loadAccountHierarchy() {
    GLA.WSGLAcctDefJson.GetAccountHierarchy(
        null,  // 從根層級開始
        $("#hidCompanyId").val(),
        onGetAccountHierarchySuccess,
        onServiceFailed
    );
}

// 處理成功結果
function onGetAccountHierarchySuccess(result) {
    if (result) {
        var hierarchyTree = $("#accountHierarchyTree");
        hierarchyTree.empty();
        
        // 建立根節點
        var rootList = $("<ul></ul>");
        hierarchyTree.append(rootList);
        
        // 遞迴建立樹狀結構
        appendAccountNodes(rootList, result);
    }
}

// 遞迴添加科目節點
function appendAccountNodes(parentElement, nodes) {
    for (var i = 0; i < nodes.length; i++) {
        var node = nodes[i];
        var listItem = $("<li></li>");
        listItem.append("<span class='account-code'>" + node.AccountCode + "</span> " +
                        "<span class='account-name'>" + node.AccountName + "</span>");
        
        parentElement.append(listItem);
        
        // 如果有子節點，遞迴添加
        if (node.Children && node.Children.length > 0) {
            var childList = $("<ul></ul>");
            listItem.append(childList);
            appendAccountNodes(childList, node.Children);
        }
    }
}

// 處理服務錯誤
function onServiceFailed(error) {
    alert("服務錯誤: " + error.get_message());
}
```

### 在伺服器端代碼中配置服務

```html
<!-- 在 ASP.NET 頁面中添加 ScriptManager 和 ServiceReference -->
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Services>
        <asp:ServiceReference Path="~/WSGLAcctDefJson.asmx" />
    </Services>
</asp:ScriptManager>
```

## 安全性考量

WSGLAcctDefJson.asmx 服務在安全方面應考慮以下事項：

1. **認證與授權**：
   - 科目定義操作必須先通過認證
   - 實施嚴格的角色和權限控制
   - 特別是修改科目定義的操作需要特殊權限

2. **數據驗證**：
   - 對所有輸入參數進行嚴格的驗證
   - 確保科目代碼符合規則和限制
   - 防止不符合業務規則的數據被存入系統

3. **數據完整性**：
   - 確保科目結構的完整性不被破壞
   - 防止關鍵科目被意外修改或刪除
   - 維護科目層級關係的一致性

4. **審計記錄**：
   - 記錄所有對科目定義的修改操作
   - 包含操作時間、操作人和操作內容
   - 提供完整的審計軌跡以便追蹤問題

5. **HTTPS 通訊**：
   - 使用 HTTPS 加密通訊
   - 保護敏感科目數據在傳輸過程中的安全
   - 防止中間人攻擊

## 效能考量

為確保 WSGLAcctDefJson.asmx 服務具有良好的效能，應注意以下幾點：

1. **快取機制**：
   - 實施科目結構和定義的快取
   - 避免頻繁讀取數據庫中的科目定義
   - 當科目定義變更時清除相關快取

2. **查詢最佳化**：
   - 最佳化科目查詢的 SQL 語句
   - 建立適當的索引支持快速查詢
   - 避免不必要的數據庫連接

3. **數據載入策略**：
   - 實施延遲載入策略
   - 僅在需要時載入詳細科目屬性
   - 分層次載入科目層級結構

4. **JSON 序列化優化**：
   - 最小化 JSON 結構減少傳輸數據量
   - 僅序列化必要的科目屬性
   - 為複雜科目結構建立輕量級表示

5. **並發處理**：
   - 處理多用戶同時操作科目定義的情況
   - 實施適當的鎖定機制
   - 確保數據一致性和完整性

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/10 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/02/25 | 系統開發部 | 修正科目驗證邏輯                 |
| 1.1.0 | 2024/04/15 | 系統開發部 | 新增科目層級結構查詢功能         |
| 1.1.1 | 2024/06/10 | 系統開發部 | 改進科目結構JSON序列化           |
| 1.2.0 | 2024/07/28 | 系統開發部 | 新增科目屬性管理功能             |
| 1.2.1 | 2024/09/15 | 系統開發部 | 優化科目查詢性能                 |
| 1.3.0 | 2024/11/20 | 系統開發部 | 新增多公司科目結構支援           |
| 1.3.1 | 2025/02/05 | 系統開發部 | 整合科目模板功能                 |

## 待改進事項

1. **功能擴展**：
   - 實現科目複製功能
   - 提供科目模板管理
   - 支援科目結構版本控制

2. **使用者體驗**：
   - 提供更直觀的科目結構可視化
   - 改進科目代碼驗證提示
   - 簡化科目屬性配置界面

3. **科目分析**：
   - 添加科目使用情況分析
   - 提供科目關係圖
   - 支援科目結構優化建議

4. **整合增強**：
   - 與 Excel 導入/導出功能集成
   - 提供批量科目處理功能
   - 與外部會計系統的科目映射

5. **架構改進**：
   - 遷移至 Web API 架構
   - 優化科目數據存儲結構
   - 改進錯誤處理和異常恢復機制 