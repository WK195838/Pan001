# WSFlowSignJson.asmx 流程簽核服務介面規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | WSFlowSignJson.asmx                   |
| 程式名稱     | 流程簽核服務介面                        |
| 檔案大小     | 101B                                 |
| 行數        | 2                                    |
| 功能簡述     | 提供流程簽核功能的Web服務介面            |
| 複雜度       | 低                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/29                           |

## 程式功能概述

WSFlowSignJson.asmx 是泛太總帳系統中的流程簽核功能網路服務介面檔案，作為前端系統與後端流程簽核服務之間的橋樑。此檔案定義了 Web 服務的入口點，使客戶端可透過標準 SOAP 或 REST 協定呼叫流程簽核相關功能，並使用 JSON 格式傳輸資料。

主要功能包括：

1. **服務定義**：定義 WSFlowSignJson 類別作為 Web 服務，讓系統能夠識別和調用
2. **介面入口**：提供前端系統訪問後端流程簽核功能的統一入口點
3. **JSON 支援**：特別支援 JSON 格式的資料交換，提高與現代前端框架的整合能力
4. **服務揭露**：使流程簽核功能能夠被前端頁面和第三方系統調用

WSFlowSignJson.asmx 與後端的 WSFlowSignManager 類別緊密整合，提供了完整的流程管理、簽核狀態追蹤、權限控制和通知機制，確保公司內部文件和交易處理遵循預定的審批流程。

## 檔案結構說明

WSFlowSignJson.asmx 採用了標準的 ASP.NET Web Service 檔案結構，包含了 Web 服務的宣告和指示。檔案雖然簡短，但其角色在系統架構中非常重要。

檔案內容包括：

```xml
<%@ WebService Language="C#" CodeBehind="WSFlowSignJson.asmx.cs" Class="GLA.WSFlowSignJson" %>
```

此宣告包含以下關鍵元素：

1. **WebService 指示詞**：指定文件為 Web 服務
2. **Language 屬性**：指定使用 C# 語言
3. **CodeBehind 屬性**：指定實際的服務實現檔案（WSFlowSignJson.asmx.cs）
4. **Class 屬性**：指定包含服務方法的完整類別名稱（GLA.WSFlowSignJson）

## 技術實現

WSFlowSignJson.asmx 利用以下技術實現其功能：

1. **ASP.NET Web Service**：
   - 使用 .NET Framework 的 Web 服務架構
   - 支援基於 SOAP 和 HTTP POST 的服務調用
   - 允許自動生成 WSDL 檔案以便客戶端集成

2. **JSON 序列化**：
   - 透過 ScriptService 特性啟用 JSON 傳輸
   - 使用 JavaScriptSerializer 進行物件序列化
   - 支援複雜物件的 JSON 表示

3. **服務配置**：
   - 設定為支援跨網域請求 (CORS)
   - 配置強類型資料傳輸模型
   - 最佳化序列化設定提高效能

4. **通訊協定**：
   - 特別強化 JSON 通訊協定支援
   - 支援 HTTP GET/POST
   - 提供 RESTful 風格的API端點

## 相依關係

WSFlowSignJson.asmx 檔案與系統其他元件的相依關係：

1. **直接相依**：
   - **WSFlowSignJson.asmx.cs**：實現服務功能的程式碼檔案
   - **WSFlowSignManager**：提供流程簽核的核心功能實現
   - **.NET Framework**：依賴 .NET Framework 的 Web 服務框架
   - **IIS**：依賴 Internet Information Services 進行託管

2. **間接相依**：
   - **IBosDB.cs**：數據訪問功能
   - **GLA資料庫**：流程簽核資料的來源和存儲
   - **System.Web.Services 命名空間**：提供 Web 服務基礎設施
   - **System.Web.Script.Services 命名空間**：提供 JSON 序列化支持

3. **被以下元件使用**：
   - **PTA0150**：會計科目維護頁面（需要簽核功能）
   - **PTA0160**：部門資料維護頁面（需要簽核功能）
   - **PTA0170**：總帳交易維護頁面（需要簽核功能）
   - **所有需要審批流程的業務頁面**
   - **ModelPopupDialog**：與模態對話框協同使用

## 服務方法說明

WSFlowSignJson.asmx 服務介面透過其實作檔案 WSFlowSignJson.asmx.cs 提供以下主要方法：

1. **GetFlowStatus**：
   - 功能：獲取特定流程的當前狀態
   - 參數：flowID（流程唯一識別碼）
   - 返回：包含流程狀態、當前處理人、待處理步驟等資訊的 JSON 物件

2. **StartFlow**：
   - 功能：啟動新的流程
   - 參數：flowType（流程類型）、documentID（相關文件）、initiator（發起人）
   - 返回：新建流程的 ID 和初始狀態

3. **ApproveStep**：
   - 功能：核准當前流程步驟
   - 參數：flowID（流程 ID）、stepID（步驟 ID）、approver（核准人）、comments（備註）
   - 返回：更新後的流程狀態

4. **RejectStep**：
   - 功能：拒絕當前流程步驟
   - 參數：flowID（流程 ID）、stepID（步驟 ID）、rejector（拒絕人）、reason（拒絕原因）
   - 返回：更新後的流程狀態

5. **GetPendingFlows**：
   - 功能：獲取待處理的流程列表
   - 參數：userID（用戶 ID）
   - 返回：待該用戶處理的流程列表

6. **GetInitiatedFlows**：
   - 功能：獲取用戶發起的流程列表
   - 參數：userID（用戶 ID）
   - 返回：該用戶發起的流程列表

7. **GetFlowHistory**：
   - 功能：獲取流程歷史記錄
   - 參數：flowID（流程 ID）
   - 返回：流程的完整處理歷史

8. **CancelFlow**：
   - 功能：取消進行中的流程
   - 參數：flowID（流程 ID）、canceler（取消人）、reason（取消原因）
   - 返回：操作結果

## 使用範例

以下是如何在前端頁面中調用 WSFlowSignJson.asmx 服務的範例：

### 使用 jQuery AJAX 呼叫服務獲取流程狀態

```javascript
// 使用 jQuery 呼叫流程簽核服務獲取特定流程狀態
function getFlowStatus(flowId) {
    $.ajax({
        type: "POST",
        url: "WSFlowSignJson.asmx/GetFlowStatus",
        data: JSON.stringify({
            flowID: flowId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var status = response.d;
            displayFlowStatus(status);
        },
        error: function (xhr, status, error) {
            console.log("流程簽核服務錯誤: " + error);
        }
    });
}

// 顯示流程狀態資訊
function displayFlowStatus(status) {
    $("#flowStatusLabel").text(getStatusText(status.StatusCode));
    $("#currentApproverLabel").text(status.CurrentApprover);
    $("#startDateLabel").text(formatDate(status.StartDate));
    
    if (status.StatusCode === "Completed") {
        $("#completeInfo").show();
        $("#completeDateLabel").text(formatDate(status.CompleteDate));
    } else {
        $("#completeInfo").hide();
    }
}

// 獲取狀態文字說明
function getStatusText(statusCode) {
    var statusMap = {
        "Initiated": "已發起",
        "InProgress": "處理中",
        "Completed": "已完成",
        "Rejected": "已拒絕",
        "Canceled": "已取消"
    };
    
    return statusMap[statusCode] || "未知";
}
```

### 使用 jQuery AJAX 呼叫服務核准流程步驟

```javascript
// 使用 jQuery 呼叫流程簽核服務核准流程步驟
function approveFlowStep(flowId, stepId) {
    var comments = $("#commentsTextArea").val();
    
    $.ajax({
        type: "POST",
        url: "WSFlowSignJson.asmx/ApproveStep",
        data: JSON.stringify({
            flowID: flowId,
            stepID: stepId,
            approver: currentUserId,
            comments: comments
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            if (result.Success) {
                showMessage("步驟已核准");
                refreshFlowStatus(flowId);
            } else {
                showError("核准失敗: " + result.ErrorMessage);
            }
        },
        error: function (xhr, status, error) {
            showError("服務錯誤: " + error);
        }
    });
}
```

### 使用 ASP.NET AJAX 呼叫服務獲取待處理流程列表

```javascript
// 使用 ASP.NET AJAX 呼叫流程簽核服務獲取待處理流程
function loadPendingFlows() {
    GLA.WSFlowSignJson.GetPendingFlows(
        $("#hidCurrentUserId").val(),
        onGetPendingFlowsSuccess,
        onServiceFailed
    );
}

// 處理成功結果
function onGetPendingFlowsSuccess(result) {
    var pendingList = $("#pendingFlowsList");
    pendingList.empty();
    
    if (result && result.length > 0) {
        for (var i = 0; i < result.length; i++) {
            var flow = result[i];
            pendingList.append(
                "<li class='flow-item' data-flow-id='" + flow.FlowID + "'>" +
                "<div class='flow-title'>" + flow.Title + "</div>" +
                "<div class='flow-info'>發起人: " + flow.Initiator + 
                " | 發起時間: " + formatDate(flow.StartDate) + "</div>" +
                "<div class='flow-actions'>" +
                "<button class='btn-approve' onclick='showApproveDialog(\"" + 
                flow.FlowID + "\", \"" + flow.CurrentStepID + "\")'>核准</button>" +
                "<button class='btn-reject' onclick='showRejectDialog(\"" + 
                flow.FlowID + "\", \"" + flow.CurrentStepID + "\")'>拒絕</button>" +
                "</div>" +
                "</li>"
            );
        }
    } else {
        pendingList.append("<li class='no-data'>目前沒有待處理的流程</li>");
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
        <asp:ServiceReference Path="~/WSFlowSignJson.asmx" />
    </Services>
</asp:ScriptManager>
```

## 安全性考量

WSFlowSignJson.asmx 服務在安全方面應考慮以下事項：

1. **認證與授權**：
   - 所有流程簽核操作必須先通過認證
   - 實施嚴格的角色和權限控制
   - 防止未授權用戶訪問或修改流程資料

2. **防止請求篡改**：
   - 驗證請求參數的合法性
   - 使用防偽令牌（Anti-Forgery Token）防止跨站請求偽造
   - 在服務端驗證用戶是否有權限執行請求的操作

3. **資料隱私**：
   - 敏感流程資料在傳輸中應加密
   - 流程資料不應暴露給無權限的用戶
   - 限制流程歷史記錄的訪問權限

4. **操作審計**：
   - 記錄所有流程簽核相關操作
   - 包含操作時間、操作人和操作內容
   - 提供完整的審計軌跡以便追蹤問題

5. **HTTPS 通訊**：
   - 強制使用 HTTPS 加密通訊
   - 防止敏感資料在傳輸過程中被攔截
   - 確保流程簽核相關資料的完整性

## 效能考量

為確保 WSFlowSignJson.asmx 服務具有良好的效能，應注意以下幾點：

1. **資料查詢優化**：
   - 針對流程查詢操作進行資料庫最佳化
   - 使用適當的索引支持快速檢索
   - 考慮實施資料快取減少資料庫負載

2. **資料載入策略**：
   - 採用延遲載入策略減少初始載入時間
   - 僅載入必要的流程詳細資訊
   - 分頁加載大量流程記錄

3. **JSON 序列化優化**：
   - 最小化 JSON 結構減少傳輸數據量
   - 只序列化必要的字段
   - 優化複雜物件的序列化處理

4. **並發處理**：
   - 使用異步處理提高服務並發能力
   - 處理多用戶同時操作同一流程的情況
   - 實施鎖定機制防止資料衝突

5. **伺服器配置**：
   - 適當設置 IIS 伺服器參數
   - 調整應用程序池設置
   - 監控服務性能，發現問題及時優化

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/02/25 | 系統開發部 | 修正流程核准權限問題             |
| 1.1.0 | 2024/04/10 | 系統開發部 | 新增郵件通知功能                 |
| 1.1.1 | 2024/05/15 | 系統開發部 | 修正流程取消功能                 |
| 1.2.0 | 2024/07/18 | 系統開發部 | 新增平行簽核支援                 |
| 1.2.1 | 2024/09/22 | 系統開發部 | 優化流程查詢性能                 |
| 1.3.0 | 2024/11/15 | 系統開發部 | 新增流程委派功能                 |
| 1.3.1 | 2025/02/10 | 系統開發部 | 整合移動設備通知功能             |

## 待改進事項

1. **功能擴展**：
   - 實現更靈活的流程規則定義機制
   - 提供圖形化流程監控界面
   - 支援基於條件的流程分支

2. **整合增強**：
   - 與公司郵件系統深度整合
   - 提供移動應用程序通知
   - 與第三方簽核系統的連接能力

3. **使用者體驗**：
   - 改進流程狀態可視化
   - 提供更直觀的流程歷史檢視
   - 支援批量處理待簽核項目

4. **報表功能**：
   - 實現流程統計和分析報表
   - 提供流程效率和瓶頸分析
   - 支援自定義報表生成

5. **架構改進**：
   - 遷移至更現代的 RESTful Web API
   - 實現微服務架構提高可擴展性
   - 改進錯誤處理和失敗恢復機制 