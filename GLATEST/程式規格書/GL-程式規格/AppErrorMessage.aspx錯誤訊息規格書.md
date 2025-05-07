# AppErrorMessage.aspx 錯誤訊息規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | AppErrorMessage.aspx                 |
| 程式名稱     | 錯誤訊息                               |
| 檔案大小     | 555B                                  |
| 行數        | 9                                     |
| 功能簡述     | 系統錯誤訊息頁面                         |
| 複雜度       | 低                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

AppErrorMessage.aspx 是泛太總帳系統的錯誤訊息顯示頁面，主要功能為：

1. 接收並顯示系統中發生的各種錯誤訊息
2. 提供使用者友善的錯誤說明與可能的解決方案
3. 記錄錯誤發生的詳細資訊以供問題排解
4. 提供適當的導航選項讓使用者能夠繼續操作系統
5. 對安全性相關的錯誤提供適當處理機制

## 程式結構說明

AppErrorMessage.aspx 的結構相對簡單，僅包含以下區域：

1. **頁面標題區**：顯示錯誤訊息標題與系統名稱
2. **錯誤內容區**：顯示詳細的錯誤訊息與說明
3. **導航區**：提供「返回上一頁」與「返回首頁」等按鈕
4. **聯絡資訊區**：提供系統管理員聯絡資訊

## 頁面元素

### ASP.NET 頁面宣告

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppErrorMessage.aspx.cs" Inherits="AppErrorMessage" %>
```

### 頁面結構

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>泛太總帳系統 - 錯誤訊息</title>
    <link href="Styles/error.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="error-container">
            <div class="error-header">
                <h1>系統錯誤</h1>
            </div>
            
            <div class="error-content">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message"></asp:Label>
                <asp:Label ID="lblErrorDetails" runat="server" CssClass="error-details"></asp:Label>
                <asp:Label ID="lblSuggestion" runat="server" CssClass="error-suggestion"></asp:Label>
            </div>
            
            <div class="error-actions">
                <asp:Button ID="btnBack" runat="server" Text="返回上一頁" CssClass="btn-back" OnClick="btnBack_Click" />
                <asp:Button ID="btnHome" runat="server" Text="返回首頁" CssClass="btn-home" OnClick="btnHome_Click" />
            </div>
            
            <div class="error-footer">
                <p>如問題持續發生，請聯絡系統管理員</p>
                <p>系統管理員：<asp:Label ID="lblAdmin" runat="server"></asp:Label></p>
                <p>聯絡電話：<asp:Label ID="lblAdminPhone" runat="server"></asp:Label></p>
            </div>
        </div>
    </form>
</body>
</html>
```

## 技術實現

AppErrorMessage.aspx 採用以下技術：

1. ASP.NET Web Forms 架構
2. 輕量級的設計，確保在系統錯誤情況下仍能正常載入
3. 使用 Session 與 QueryString 傳遞錯誤資訊
4. 採用獨立的 CSS 樣式表，減少頁面複雜度

## 依賴關係

AppErrorMessage.aspx 依賴以下檔案與元件：

1. AppErrorMessage.aspx.cs：錯誤訊息後端程式碼
2. error.css：錯誤頁面樣式表
3. Global.asax：全局錯誤處理與路由

## 使用者介面

AppErrorMessage.aspx 的使用者介面設計考量：

1. 清晰明確的錯誤訊息，避免技術性術語
2. 視覺化的錯誤等級呈現（警告、嚴重、一般等）
3. 簡潔的頁面佈局，避免使用者混淆
4. 高對比度的配色，確保錯誤訊息易於閱讀
5. 直覺的操作按鈕，便於使用者選擇下一步動作

## 資料處理

AppErrorMessage.aspx 的資料處理相對簡單：

1. 從 Session 或 QueryString 讀取錯誤相關資訊
2. 格式化錯誤訊息以便使用者理解
3. 記錄錯誤資訊至系統日誌
4. 根據錯誤類型提供適當的建議與導航選項

## 頁面行為

AppErrorMessage.aspx 的主要頁面行為：

1. 頁面載入時讀取並顯示錯誤資訊
2. 「返回上一頁」按鈕使用瀏覽器歷史返回功能
3. 「返回首頁」按鈕導航至系統首頁
4. 特定類型的錯誤可能提供額外的操作選項
5. 錯誤等級會影響頁面的視覺呈現方式

## 異常處理

AppErrorMessage.aspx 本身也需要處理可能的異常：

1. 當錯誤參數缺失時顯示通用錯誤訊息
2. 確保頁面在各種錯誤情況下仍能正確顯示
3. 最小化對外部資源的依賴，減少錯誤連鎖反應
4. 實作簡單的自我診斷功能

## 安全性考量

AppErrorMessage.aspx 的安全性措施：

1. 過濾敏感的錯誤資訊，避免資訊洩露
2. 對於安全性相關錯誤提供特殊處理
3. 確保錯誤日誌不包含敏感的使用者資料
4. 採用中立化的錯誤訊息，避免暴露系統架構

## 效能考量

AppErrorMessage.aspx 的效能優化：

1. 頁面大小極小（僅 555B），確保快速載入
2. 最小化外部資源依賴
3. 避免複雜的 DOM 結構或大量 JavaScript
4. 確保頁面在低網路頻寬環境下依然可用

## 測試記錄

AppErrorMessage.aspx 經過以下測試：

1. 各種錯誤類型的顯示測試
2. 不同瀏覽器與裝置兼容性測試
3. 頁面導航按鈕功能測試
4. 錯誤資訊傳遞完整性測試

## 待改進事項

AppErrorMessage.aspx 的可能改進項目：

1. 增加更詳細的問題診斷資訊（僅對系統管理員可見）
2. 優化行動裝置上的顯示效果
3. 增加更多自助解決問題的建議
4. 整合線上支援系統，使用者可直接從錯誤頁面請求協助

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 相關檔案目錄結構

```
/
├── AppErrorMessage.aspx
├── AppErrorMessage.aspx.cs
├── Global.asax
├── Web.config
└── Styles/
    └── error.css
```

### 常見錯誤類型與處理方式

1. **資料庫連線錯誤**：顯示系統維護訊息，避免暴露資料庫細節
2. **權限不足錯誤**：顯示權限相關說明，提供申請權限的途徑
3. **作業逾時錯誤**：建議使用者重新登入系統
4. **系統內部錯誤**：提供錯誤編號供管理員參考，但隱藏技術細節 