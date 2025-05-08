# WSDialogData_Exception.asmx 對話框例外服務介面規格書

## 基本資訊

| 項目       | 內容                                   |
|------------|--------------------------------------|
| 程式代號     | WSDialogData_Exception.asmx          |
| 程式名稱     | 對話框例外服務介面                     |
| 檔案大小     | 117B                                |
| 行數        | 2                                   |
| 功能簡述     | 提供對話框例外處理的Web服務介面         |
| 複雜度       | 低                                  |
| 規格書狀態   | 已完成                               |
| 負責人員     | Claude AI                          |
| 完成日期     | 2025/6/2                           |

## 程式功能概述

WSDialogData_Exception.asmx 是泛太總帳系統中的對話框例外處理功能網路服務介面檔案，作為前端系統與後端對話框例外處理服務之間的橋樑。此檔案定義了 Web 服務的入口點，使客戶端可透過標準 SOAP 或 REST 協定呼叫對話框例外處理相關功能。

主要功能包括：

1. **服務定義**：定義 WSDialogData_Exception 類別作為 Web 服務，讓系統能夠識別和調用
2. **介面入口**：提供前端系統訪問後端對話框例外處理功能的統一入口點
3. **協定支援**：支援 SOAP 和 JSON 格式的數據交換
4. **服務揭露**：使對話框例外處理功能能夠被前端頁面和第三方系統調用

WSDialogData_Exception.asmx 與後端的 WSDialogData_Exception 類別緊密整合，專門處理對話框操作過程中的異常情況，包括資料驗證錯誤、處理過程例外、系統錯誤等，提供統一的例外處理機制，確保用戶體驗的一致性和系統的穩定性。

## 檔案結構說明

WSDialogData_Exception.asmx 採用了標準的 ASP.NET Web Service 檔案結構，包含了 Web 服務的宣告和指示。檔案雖然簡短，但其角色在系統架構中非常重要。

檔案內容包括：

```xml
<%@ WebService Language="C#" CodeBehind="~/App_Code/WSDialogData_Exception.cs" Class="WSDialogData_Exception" %>
```

此宣告包含以下關鍵元素：

1. **WebService 指示詞**：指定文件為 Web 服務
2. **Language 屬性**：指定使用 C# 語言
3. **CodeBehind 屬性**：指定實際的服務實現檔案（~/App_Code/WSDialogData_Exception.cs）
4. **Class 屬性**：指定包含服務方法的完整類別名稱（WSDialogData_Exception）

## 技術實現

WSDialogData_Exception.asmx 利用以下技術實現其功能：

1. **ASP.NET Web Service**：
   - 使用 .NET Framework 的 Web 服務架構
   - 支援基於 SOAP 和 HTTP POST 的服務調用
   - 允許自動生成 WSDL 檔案以便客戶端集成

2. **例外處理機制**：
   - 提供結構化的例外資訊
   - 支援不同層級的例外分類
   - 實現例外追蹤和記錄功能

3. **資料驗證**：
   - 提供客戶端資料驗證規則
   - 處理後端資料驗證例外
   - 返回友好的錯誤訊息

4. **通訊協定**：
   - 支援 SOAP 協定
   - 支援 HTTP GET/POST
   - 支援 JSON 序列化（通過特性配置）

## 相依關係

WSDialogData_Exception.asmx 檔案與系統其他元件的相依關係：

1. **直接相依**：
   - **WSDialogData_Exception.cs**：實現服務功能的程式碼檔案
   - **.NET Framework**：依賴 .NET Framework 的 Web 服務框架
   - **IIS**：依賴 Internet Information Services 進行託管

2. **間接相依**：
   - **IBosDB.cs**：數據訪問功能
   - **GLA資料庫**：系統例外記錄的儲存位置
   - **System.Web.Services 命名空間**：提供 Web 服務基礎設施
   - **ExceptionHelper**：例外處理輔助類別

3. **被以下元件使用**：
   - **ModelPopupDialog**：模態對話框控制項
   - **PopupDialog**：彈出對話框控制項
   - **WSDialogData**：基礎對話框資料服務
   - **WSDialogData_Table**：對話框表格資料服務
   - **所有需要例外處理的對話框元件**

## 服務方法說明

WSDialogData_Exception.asmx 服務介面透過其實作檔案 WSDialogData_Exception.cs 提供以下主要方法：

1. **LogException**：
   - 功能：記錄例外資訊
   - 參數：exceptionInfo（例外資訊物件）、severity（嚴重程度）、source（來源）
   - 返回：記錄結果，包含記錄ID和處理狀態

2. **GetExceptionDetails**：
   - 功能：獲取例外的詳細資訊
   - 參數：exceptionID（例外識別碼）
   - 返回：包含例外詳情的物件

3. **ValidateDialogInput**：
   - 功能：驗證對話框輸入資料
   - 參數：dialogID（對話框識別碼）、inputData（輸入資料）
   - 返回：驗證結果，包含是否有效和錯誤訊息列表

4. **GetValidationRules**：
   - 功能：獲取對話框的驗證規則
   - 參數：dialogID（對話框識別碼）
   - 返回：可用於客戶端驗證的規則清單

5. **HandleSystemException**：
   - 功能：處理系統層級例外
   - 參數：exceptionInfo（例外資訊）、context（上下文資訊）
   - 返回：處理結果和應對措施

## 使用範例

以下是如何在前端頁面中調用 WSDialogData_Exception.asmx 服務的範例：

### 使用 jQuery AJAX 呼叫服務記錄例外

```javascript
// 使用 jQuery 呼叫對話框例外服務記錄例外
function logException(error, severity) {
    var exceptionInfo = {
        Message: error.message || "未知錯誤",
        StackTrace: error.stack || "",
        Source: "客戶端對話框",
        AdditionalInfo: {
            BrowserInfo: navigator.userAgent,
            Timestamp: new Date().toISOString(),
            URL: window.location.href
        }
    };
    
    $.ajax({
        type: "POST",
        url: "WSDialogData_Exception.asmx/LogException",
        data: JSON.stringify({
            exceptionInfo: exceptionInfo,
            severity: severity || "Error",
            source: "Dialog"
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            console.log("例外已記錄，ID: " + result.ExceptionID);
            
            // 根據嚴重性決定是否顯示錯誤訊息給用戶
            if (severity === "Critical" || severity === "Error") {
                showErrorMessage("系統發生錯誤，請聯繫管理員。錯誤參考碼: " + result.ExceptionID);
            }
        },
        error: function (xhr, status, error) {
            console.log("無法記錄例外: " + error);
            showErrorMessage("系統發生未預期錯誤");
        }
    });
}

// 顯示錯誤訊息
function showErrorMessage(message) {
    $("#errorMessagePanel").text(message).show();
    setTimeout(function() {
        $("#errorMessagePanel").fadeOut();
    }, 5000);
}
```

### 使用 jQuery AJAX 呼叫服務驗證對話框輸入

```javascript
// 使用 jQuery 呼叫對話框例外服務驗證輸入
function validateDialogInput(dialogId, formData) {
    $.ajax({
        type: "POST",
        url: "WSDialogData_Exception.asmx/ValidateDialogInput",
        data: JSON.stringify({
            dialogID: dialogId,
            inputData: formData
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            
            if (result.IsValid) {
                // 驗證通過，提交表單
                submitDialogForm(dialogId, formData);
            } else {
                // 顯示驗證錯誤
                displayValidationErrors(result.Errors);
            }
        },
        error: function (xhr, status, error) {
            console.log("驗證服務錯誤: " + error);
            logException({ message: "驗證服務錯誤: " + error }, "Warning");
        }
    });
}

// 顯示驗證錯誤
function displayValidationErrors(errors) {
    var errorList = $("#validationErrorsList");
    errorList.empty();
    
    if (errors && errors.length > 0) {
        $.each(errors, function(index, error) {
            errorList.append("<li>" + error.Message + "</li>");
        });
        
        $("#validationErrorsPanel").show();
    }
}
```

### 使用 ASP.NET AJAX 呼叫服務獲取驗證規則

```javascript
// 使用 ASP.NET AJAX 呼叫對話框例外服務獲取驗證規則
function loadValidationRules(dialogId) {
    WSDialogData_Exception.GetValidationRules(
        dialogId,
        onGetValidationRulesSuccess,
        onServiceFailed
    );
}

// 處理驗證規則
function onGetValidationRulesSuccess(result) {
    if (result) {
        // 設置客戶端驗證規則
        setupClientValidation(result);
    }
}

// 設置客戶端驗證
function setupClientValidation(rules) {
    if (!rules || rules.length === 0) return;
    
    // 清除現有驗證
    $(".input-validation-error").removeClass("input-validation-error");
    $(".field-validation-error").empty().hide();
    
    // 為每個欄位設置驗證
    $.each(rules, function(index, rule) {
        var field = $("#" + rule.FieldID);
        if (field.length === 0) return;
        
        // 根據規則類型設置驗證
        switch (rule.Type) {
            case "Required":
                field.attr("required", "required");
                field.attr("data-val-required", rule.ErrorMessage || "此欄位為必填");
                break;
                
            case "MaxLength":
                field.attr("maxlength", rule.MaxLength);
                field.attr("data-val-maxlength", rule.ErrorMessage || 
                          "最多允許 " + rule.MaxLength + " 個字元");
                break;
                
            case "Regex":
                field.attr("pattern", rule.Pattern);
                field.attr("data-val-regex", rule.ErrorMessage || "格式不正確");
                break;
                
            case "Range":
                field.attr("min", rule.Min);
                field.attr("max", rule.Max);
                field.attr("data-val-range", rule.ErrorMessage || 
                          "值必須在 " + rule.Min + " 至 " + rule.Max + " 之間");
                break;
        }
        
        // 添加驗證事件
        field.blur(function() {
            validateField($(this), rule);
        });
    });
}

// 驗證單一欄位
function validateField(field, rule) {
    var value = field.val();
    var isValid = true;
    var errorMessage = "";
    
    // 根據規則類型驗證
    switch (rule.Type) {
        case "Required":
            isValid = value !== "";
            errorMessage = rule.ErrorMessage || "此欄位為必填";
            break;
            
        // 其他驗證類型...
    }
    
    // 顯示錯誤或清除錯誤
    var errorSpan = $("#" + field.attr("id") + "_error");
    if (!isValid) {
        field.addClass("input-validation-error");
        errorSpan.text(errorMessage).show();
    } else {
        field.removeClass("input-validation-error");
        errorSpan.empty().hide();
    }
    
    return isValid;
}

// 處理服務錯誤
function onServiceFailed(error) {
    console.log("服務錯誤: " + error.get_message());
    logException({ message: error.get_message() }, "Warning");
}
```

### 在伺服器端代碼中配置服務

```html
<!-- 在 ASP.NET 頁面中添加 ScriptManager 和 ServiceReference -->
<asp:ScriptManager ID="ScriptManager1" runat="server">
    <Services>
        <asp:ServiceReference Path="~/WSDialogData_Exception.asmx" />
    </Services>
</asp:ScriptManager>
```

## 安全性考量

WSDialogData_Exception.asmx 服務在安全方面應考慮以下事項：

1. **例外資訊保護**：
   - 避免向客戶端暴露敏感的例外詳情
   - 過濾可能包含內部系統資訊的例外訊息
   - 使用參考碼代替直接的錯誤訊息

2. **授權控制**：
   - 限制例外詳情的訪問權限
   - 只允許授權用戶查看完整的例外資訊
   - 防止非授權用戶利用例外資訊探測系統

3. **日誌管理**：
   - 實施安全的例外日誌記錄機制
   - 定期審查例外記錄識別潛在攻擊
   - 設置敏感資訊的掩碼處理

4. **輸入驗證**：
   - 對所有輸入的例外資訊進行驗證
   - 防止通過例外記錄功能進行注入攻擊
   - 限制例外訊息的長度和格式

5. **HTTPS 通訊**：
   - 建議配置 HTTPS 以加密例外數據傳輸
   - 保護可能包含敏感資訊的例外數據
   - 防止例外資訊在傳輸過程中被竊取

## 效能考量

為確保 WSDialogData_Exception.asmx 服務具有良好的效能，應注意以下幾點：

1. **例外處理效率**：
   - 優化例外記錄過程，避免影響主要業務流程
   - 使用非同步方式處理例外記錄
   - 避免在高頻操作中過度記錄例外

2. **日誌存儲優化**：
   - 實施高效的例外資訊存儲機制
   - 考慮使用專用的例外日誌存儲系統
   - 定期清理過期的例外記錄

3. **客戶端驗證**：
   - 優先使用客戶端驗證減少伺服器驗證需求
   - 設計高效的驗證規則傳遞機制
   - 使用快取減少重複驗證規則獲取

4. **資源控制**：
   - 限制單個請求的例外記錄數量
   - 防止例外風暴導致系統資源耗盡
   - 監控例外處理服務的資源使用情況

5. **故障恢復**：
   - 確保例外處理服務本身的穩定性
   - 實施例外服務的失敗安全機制
   - 避免因例外處理服務故障而影響主系統

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/03/08 | 系統開發部 | 修正例外記錄問題                 |
| 1.1.0 | 2024/04/25 | 系統開發部 | 新增客戶端驗證規則功能           |
| 1.1.1 | 2024/06/15 | 系統開發部 | 修正驗證規則傳遞問題             |
| 1.2.0 | 2024/08/20 | 系統開發部 | 新增系統例外處理功能             |
| 1.2.1 | 2024/10/28 | 系統開發部 | 效能優化                         |
| 1.3.0 | 2025/01/25 | 系統開發部 | 新增例外分析與報告功能           |
| 1.3.1 | 2025/04/10 | 系統開發部 | 整合安全性改進                   |

## 待改進事項

1. **功能擴展**：
   - 實現更詳細的例外分類與處理機制
   - 提供例外趨勢分析功能
   - 支援自定義的例外處理流程

2. **技術升級**：
   - 考慮遷移至更現代的 Web API 架構
   - 使用 RESTful 設計原則重構服務
   - 強化非同步例外處理能力

3. **整合改進**：
   - 與系統監控工具整合
   - 提供例外通知機制
   - 實現對話框操作的完整審計追蹤

4. **使用者體驗**：
   - 提供更友好的錯誤訊息
   - 實現智能錯誤恢復建議
   - 改進驗證錯誤的視覺呈現

5. **安全增強**：
   - 實現更完善的例外資訊保護
   - 強化例外分析以識別安全威脅
   - 提供更細粒度的例外處理權限控制 