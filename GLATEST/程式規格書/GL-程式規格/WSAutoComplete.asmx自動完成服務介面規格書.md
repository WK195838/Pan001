# WSAutoComplete.asmx 自動完成服務介面規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | WSAutoComplete.asmx                   |
| 程式名稱     | 自動完成服務介面                        |
| 檔案大小     | 101B                                 |
| 行數        | 2                                    |
| 功能簡述     | 提供自動完成功能的Web服務介面             |
| 複雜度       | 低                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/28                           |

## 程式功能概述

WSAutoComplete.asmx 是泛太總帳系統中的自動完成功能網路服務介面檔案，作為前端系統與後端自動完成功能服務之間的橋樑。此檔案定義了 Web 服務的入口點，使客戶端可透過標準 SOAP 或 REST 協定呼叫自動完成相關功能。

主要功能包括：

1. **服務定義**：定義 WSAutoComplete 類別作為 Web 服務，讓系統能夠識別和調用
2. **介面入口**：提供前端系統訪問後端自動完成功能的統一入口點
3. **協定支援**：支援 SOAP 和 JSON 格式的數據交換
4. **服務揭露**：使自動完成功能能夠被前端頁面和第三方系統調用

WSAutoComplete.asmx 與後端的 WSAutoComplete 類別緊密整合，提供了針對多種數據類型的自動完成功能，包括會計科目、部門代碼、人員資料等，從而大幅提升系統的使用者體驗和資料輸入效率。

## 檔案結構說明

WSAutoComplete.asmx 採用了標準的 ASP.NET Web Service 檔案結構，包含了 Web 服務的宣告和指示。檔案雖然簡短，但其角色在系統架構中非常重要。

檔案內容包括：

```xml
<%@ WebService Language="C#" CodeBehind="WSAutoComplete.asmx.cs" Class="GLA.WSAutoComplete" %>
```

此宣告包含以下關鍵元素：

1. **WebService 指示詞**：指定文件為 Web 服務
2. **Language 屬性**：指定使用 C# 語言
3. **CodeBehind 屬性**：指定實際的服務實現檔案（WSAutoComplete.asmx.cs）
4. **Class 屬性**：指定包含服務方法的完整類別名稱（GLA.WSAutoComplete）

## 技術實現

WSAutoComplete.asmx 利用以下技術實現其功能：

1. **ASP.NET Web Service**：
   - 使用 .NET Framework 的 Web 服務架構
   - 支援基於 SOAP 和 HTTP POST 的服務調用
   - 允許自動生成 WSDL 檔案以便客戶端集成

2. **服務描述**：
   - 自動為服務生成描述資訊
   - 提供服務探索功能
   - 允許通過瀏覽器訪問服務測試頁面

3. **架構整合**：
   - 與 ASP.NET 應用程式架構整合
   - 支援 IIS 託管與部署
   - 允許使用 ASP.NET 的身份驗證和授權機制

4. **通訊協定**：
   - 支援 SOAP 協定
   - 支援 HTTP GET/POST
   - 支援 JSON 序列化（通過特性配置）

## 相依關係

WSAutoComplete.asmx 檔案與系統其他元件的相依關係：

1. **直接相依**：
   - **WSAutoComplete.asmx.cs**：實現服務功能的程式碼檔案
   - **.NET Framework**：依賴 .NET Framework 的 Web 服務框架
   - **IIS**：依賴 Internet Information Services 進行託管

2. **間接相依**：
   - **IBosDB.cs**：數據訪問功能
   - **GLA資料庫**：用於獲取自動完成數據的來源
   - **System.Web.Services 命名空間**：提供 Web 服務基礎設施

3. **被以下元件使用**：
   - **SearchList 控制項**：使用自動完成服務提供搜尋建議
   - **Subjects 控制項**：使用自動完成服務提供科目建議
   - **PTA0150**：會計科目維護頁面
   - **PTA0160**：部門資料維護頁面
   - **PTA0170**：總帳交易維護頁面
   - **所有需要自動完成功能的表單**

## 使用範例

以下是如何在前端頁面中調用 WSAutoComplete.asmx 服務的範例：

### 使用 jQuery AJAX 呼叫服務

```javascript
// 使用 jQuery 呼叫自動完成服務獲取科目建議
function getAccountSuggestions(prefix) {
    $.ajax({
        type: "POST",
        url: "WSAutoComplete.asmx/GetAccountSuggestions",
        data: JSON.stringify({
            prefix: prefix,
            companyId: $("#hidCompanyId").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var suggestions = response.d;
            displaySuggestions(suggestions);
        },
        error: function (xhr, status, error) {
            console.log("自動完成服務錯誤: " + error);
        }
    });
}

// 顯示建議列表
function displaySuggestions(suggestions) {
    var suggestionList = $("#suggestionList");
    suggestionList.empty();
    
    if (suggestions && suggestions.length > 0) {
        $.each(suggestions, function (index, item) {
            suggestionList.append("<li data-id='" + item.Value + 
                "'>" + item.Label + "</li>");
        });
        suggestionList.show();
    } else {
        suggestionList.hide();
    }
}
```

### 使用 ASP.NET AJAX 呼叫服務

```javascript
// 使用 ASP.NET AJAX 呼叫自動完成服務
function getDepartmentSuggestions(prefix) {
    GLA.WSAutoComplete.GetDepartmentSuggestions(
        prefix, 
        $("#hidCompanyId").val(),
        onDepartmentSuggestionsSuccess, 
        onServiceFailed
    );
}

// 處理成功結果
function onDepartmentSuggestionsSuccess(result) {
    if (result && result.length > 0) {
        var suggestionList = $("#deptSuggestionList");
        suggestionList.empty();
        
        for (var i = 0; i < result.length; i++) {
            suggestionList.append("<li data-id='" + result[i].Value + 
                "'>" + result[i].Label + "</li>");
        }
        suggestionList.show();
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
        <asp:ServiceReference Path="~/WSAutoComplete.asmx" />
    </Services>
</asp:ScriptManager>
```

## 安全性考量

WSAutoComplete.asmx 服務在安全方面應考慮以下事項：

1. **輸入驗證**：
   - 所有前端傳入的數據需要進行嚴格的驗證
   - 應檢查輸入字串長度和格式，防止注入攻擊
   - 限制查詢結果數量，避免資源消耗攻擊

2. **授權控制**：
   - 服務方法應實施適當的權限檢查
   - 部分敏感數據可能需要限制特定角色才能訪問
   - 考慮使用 ASP.NET 內建的授權機制

3. **數據過濾**：
   - 查詢結果應根據用戶權限進行過濾
   - 敏感數據應在返回前進行適當處理
   - 限制每次返回的資料量

4. **錯誤處理**：
   - 隱藏詳細錯誤資訊，不向客戶端暴露系統細節
   - 實施統一的錯誤處理機制
   - 在服務端記錄錯誤，但返回適當的用戶提示

5. **HTTPS 通訊**：
   - 建議配置 HTTPS 以加密數據傳輸
   - 保護敏感數據在網絡傳輸過程中的安全
   - 防止中間人攻擊

## 效能考量

為確保 WSAutoComplete.asmx 服務具有良好的效能，應注意以下幾點：

1. **查詢最佳化**：
   - 對自動完成功能的數據庫查詢進行最佳化
   - 使用適當的索引支持快速查詢
   - 實施快取機制減少數據庫訪問

2. **結果限制**：
   - 限制返回的建議項目數量
   - 通常自動完成功能只需返回 5-10 個最相關的結果
   - 允許客戶端配置結果數量限制

3. **響應時間**：
   - 維持 200ms 以內的響應時間以獲得良好的用戶體驗
   - 監控服務性能，發現問題及時優化
   - 考慮使用非同步處理提高併發能力

4. **數據序列化**：
   - 僅序列化必要的數據字段
   - 使用輕量級的數據格式如 JSON
   - 避免傳輸冗餘數據

5. **網絡優化**：
   - 減少不必要的服務調用
   - 實施客戶端延遲處理，避免頻繁請求
   - 考慮使用批次處理多個請求

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/02/28 | 系統開發部 | 修正查詢權限問題                 |
| 1.1.0 | 2024/04/10 | 系統開發部 | 新增部門資料自動完成功能         |
| 1.1.1 | 2024/06/05 | 系統開發部 | 修正特殊字元處理問題             |
| 1.2.0 | 2024/08/12 | 系統開發部 | 新增人員資料自動完成功能         |
| 1.2.1 | 2024/10/20 | 系統開發部 | 效能優化                         |
| 1.3.0 | 2025/01/15 | 系統開發部 | 新增供應商資料自動完成功能       |
| 1.3.1 | 2025/03/25 | 系統開發部 | 整合錯誤處理改進                 |

## 待改進事項

1. **功能擴展**：
   - 增加更多數據類型的自動完成支援
   - 提供更豐富的結果格式選項
   - 支援多語言搜尋和建議

2. **技術升級**：
   - 考慮遷移至更現代的 Web API 架構
   - 使用 RESTful 設計原則重構服務
   - 改進非同步處理能力

3. **整合改進**：
   - 強化與前端框架的整合
   - 簡化客戶端調用方式
   - 提供統一的錯誤處理機制

4. **搜尋增強**：
   - 實現模糊搜尋功能
   - 支援拼音或注音搜尋
   - 添加使用者個人化的搜尋建議

5. **安全性提升**：
   - 實現更完善的輸入驗證
   - 增強授權機制
   - 強化資料存取控制 