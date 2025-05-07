# AuthAD.aspx AD認證規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | AuthAD.aspx                          |
| 程式名稱     | AD認證                                |
| 檔案大小     | 6.2KB                                |
| 行數        | 135                                   |
| 功能簡述     | Active Directory認證頁面               |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

AuthAD.aspx 是泛太總帳系統的 Active Directory 認證頁面，主要功能為：

1. 提供使用者透過 Active Directory 進行身份驗證
2. 連結企業內部的使用者資料庫進行使用者權限確認
3. 將使用者登入資訊儲存在 Session 中
4. 檢查使用者帳號狀態與系統存取權限
5. 處理認證成功後的頁面重新導向

## 程式結構說明

AuthAD.aspx 的結構主要包含以下區域：

1. **登入表單區**：提供使用者輸入帳號密碼的介面
2. **認證資訊區**：顯示認證狀態與相關訊息
3. **錯誤訊息區**：顯示認證失敗的原因與建議
4. **輔助選項區**：提供密碼重設或其他輔助功能的連結

## 頁面元素

### ASP.NET 頁面宣告

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthAD.aspx.cs" Inherits="AuthAD" %>
```

### 頁面結構

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>泛太總帳系統 - AD認證</title>
    <link href="Styles/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmLogin" runat="server">
        <div class="login-container">
            <div class="login-header">
                <h1>泛太總帳系統</h1>
                <h2>Active Directory 認證</h2>
            </div>
            
            <div class="login-form">
                <div class="form-group">
                    <asp:Label ID="lblUserName" runat="server" Text="使用者名稱："></asp:Label>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <asp:Label ID="lblPassword" runat="server" Text="密碼："></asp:Label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                </div>
                
                <div class="form-group">
                    <asp:CheckBox ID="chkRemember" runat="server" Text="記住帳號" />
                </div>
                
                <div class="form-actions">
                    <asp:Button ID="btnLogin" runat="server" Text="登入" CssClass="btn-login" OnClick="btnLogin_Click" />
                </div>
            </div>
            
            <div class="login-messages">
                <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
            </div>
            
            <div class="login-footer">
                <p>若忘記密碼請洽系統管理員</p>
                <p>系統版本：1.0.0</p>
            </div>
        </div>
    </form>
</body>
</html>
```

## 技術實現

AuthAD.aspx 採用以下技術：

1. ASP.NET Web Forms 架構
2. Active Directory 驗證機制
3. Forms Authentication 驗證模式
4. 標準 HTML 與 CSS 定義使用者界面
5. Session 管理使用者登入狀態

## 依賴關係

AuthAD.aspx 依賴以下檔案與元件：

1. AuthAD.aspx.cs：AD認證後端程式碼
2. login.css：登入頁面樣式表
3. LoginClass：系統登入邏輯模組
4. AppAuthority：權限管理模組

## 使用者介面

AuthAD.aspx 的使用者介面設計簡潔明確：

1. 清晰的表單標題與說明
2. 明確標示的輸入欄位
3. 顯眼的登入按鈕
4. 適當的錯誤訊息顯示區域
5. 簡潔的頁面佈局與配色

## 資料處理

AuthAD.aspx 的資料處理集中在使用者認證過程：

1. 收集使用者輸入的帳號與密碼
2. 透過 Active Directory 驗證使用者身份
3. 取得使用者在 AD 中的相關資料（姓名、部門、職稱等）
4. 將認證結果與使用者資料儲存在 Session 中

## 頁面行為

AuthAD.aspx 的主要頁面行為：

1. 用戶輸入帳號密碼後點擊登入按鈕
2. 系統進行 AD 認證與權限驗證
3. 根據認證結果顯示相應訊息或轉導至系統首頁
4. 依據「記住帳號」選項設定 Cookie

## 異常處理

AuthAD.aspx 的異常處理機制：

1. 在認證失敗時提供明確的錯誤訊息
2. 處理 AD 連線失敗的情況
3. 提供重試與備援登入選項
4. 記錄登入失敗的原因與相關資訊

## 安全性考量

AuthAD.aspx 的安全性措施：

1. 密碼採用 TextMode="Password" 避免明文顯示
2. 透過 SSL 加密傳輸認證資料
3. 防止 SQL 注入與 XSS 攻擊
4. 設定登入嘗試次數限制
5. 登入完成後立即清除表單中的密碼資訊

## 效能考量

AuthAD.aspx 的效能優化：

1. 頁面大小控制在 6.2KB 內，確保快速載入
2. 最小化外部資源依賴
3. 避免不必要的頁面重新載入
4. 設定適當的超時與 Session 管理

## 測試記錄

AuthAD.aspx 經過以下測試：

1. AD 連線與認證功能測試
2. 不同類型帳號的權限測試
3. 表單驗證與錯誤處理測試
4. 認證後導航邏輯測試

## 待改進事項

AuthAD.aspx 的可能改進項目：

1. 增加雙因素認證選項
2. 優化行動裝置上的顯示效果
3. 增加「使用者最後登入時間」顯示
4. 考慮整合單一登入 (SSO) 機制

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 相關檔案目錄結構

```
/
├── AuthAD.aspx
├── AuthAD.aspx.cs
├── Login.aspx
├── Default.aspx
├── Styles/
│   └── login.css
└── App_Code/
    ├── LoginClass.cs
    └── AppAuthority.cs
``` 