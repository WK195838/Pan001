# PopupDialog 彈出對話框規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | PopupDialog                           |
| 程式名稱     | 彈出對話框                              |
| 檔案大小     | 2.1KB                                 |
| 行數        | ~58                                   |
| 功能簡述     | 提供共用彈出視窗                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/15                             |

## 程式功能概述

PopupDialog 是泛太總帳系統中的彈出對話框元件，提供統一且一致的彈出視窗功能。此元件廣泛應用於系統中需要彈出視窗的各種場景，如資料選擇、確認訊息、警告提示和錯誤顯示等。彈出對話框可以動態載入不同的內容頁面，支援各種互動方式，並提供豐富的自訂選項。主要功能包括：

1. 提供統一的彈出視窗界面，保持系統用戶體驗一致性
2. 支援動態載入各種內容頁面（如查詢、選擇、編輯頁面）
3. 可自訂對話框標題、尺寸、按鈕和顯示模式
4. 提供模態和非模態兩種顯示方式
5. 支援多層彈出視窗（對話框中開啟另一個對話框）
6. 整合資料傳遞機制，支援父頁面和對話框頁面間的雙向資料傳遞
7. 提供對話框關閉事件，允許父頁面接收對話框操作結果
8. 自動處理視窗大小調整和位置計算，確保在不同解析度下的正確顯示
9. 支援多種開啟和關閉動畫效果
10. 提供完整的錯誤處理和例外情況管理

此元件設計為高度可重用的共用元件，為整個系統提供統一的彈出視窗解決方案。

## 控制項結構說明

PopupDialog 的頁面結構主要由以下部分組成：

1. **對話框容器**：最外層的容器，控制對話框的位置和覆蓋範圍
2. **對話框主體**：包含標題列、內容區域和按鈕區域
3. **標題列**：顯示對話框標題和關閉按鈕
4. **內容區域**：動態載入的頁面內容顯示區域
5. **按鈕區域**：包含預設的確定、取消等按鈕，也可自訂按鈕

整體設計遵循系統 UI 設計規範，提供一致且專業的使用者體驗。

## 頁面元素

### ASPX 頁面宣告

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopupDialog.aspx.cs" Inherits="PopupDialog" %>
```

### 頁面結構

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>彈出對話框</title>
    <link href="../Styles/dialog.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/ModPopFunction.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="popup-overlay" id="dialogOverlay" runat="server">
            <div class="popup-container" id="dialogContainer" runat="server">
                <div class="popup-header">
                    <div class="popup-title" id="dialogTitle" runat="server">對話框標題</div>
                    <div class="popup-close">
                        <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" CssClass="close-btn">&times;</asp:LinkButton>
                    </div>
                </div>
                <div class="popup-content" id="dialogContent" runat="server">
                    <asp:PlaceHolder ID="phContent" runat="server"></asp:PlaceHolder>
                </div>
                <div class="popup-footer" id="dialogFooter" runat="server">
                    <asp:Button ID="btnOK" runat="server" Text="確定" CssClass="btn btn-primary" OnClick="btnOK_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btn btn-default" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
```

## 技術實現

PopupDialog 基於以下技術實現：

1. **ASP.NET Web Forms**：使用 ASP.NET 頁面技術建立對話框
2. **HTML5 / CSS3**：使用現代化的 HTML5 和 CSS3 技術實現界面布局和特效
3. **JavaScript / jQuery**：使用 jQuery 實現動態顯示、關閉和位置計算等前端功能
4. **C# 程式碼**：在後端實現對話框的邏輯控制和資料處理
5. **Placeholder 控制項**：使用 PlaceHolder 動態載入對話框內容

## 相依檔案和元件

PopupDialog 依賴以下檔案與元件：

1. **PopupDialog.aspx.cs**：對話框的後端程式碼
2. **dialog.css**：對話框的樣式表
3. **ModPopFunction.js**：處理對話框顯示和互動的 JavaScript 函數集
4. **jQuery 函式庫**：提供基礎 DOM 操作和動畫效果

## 使用者介面

PopupDialog 提供以下使用者界面元素：

1. **背景遮罩**：顯示半透明遮罩覆蓋整個頁面，凸顯對話框
2. **對話框邊框**：清晰界定對話框範圍，提供圓角和陰影效果
3. **標題列**：顯示對話框標題，並提供關閉按鈕
4. **內容區域**：顯示動態載入的頁面內容，支援捲動條
5. **按鈕區域**：提供標準按鈕（確定、取消等）或自訂按鈕
6. **視覺設計**：整體設計符合系統 UI 風格，提供一致的視覺體驗

## 屬性說明

PopupDialog 提供以下可設定的屬性：

| 屬性名稱 | 資料類型 | 說明 | 預設值 |
|---------|---------|------|--------|
| Title | string | 取得或設定對話框標題 | "對話框" |
| Width | string | 取得或設定對話框寬度 | "500px" |
| Height | string | 取得或設定對話框高度 | "400px" |
| ContentUrl | string | 取得或設定內容頁面的 URL | 空字串 |
| IsModal | bool | 取得或設定是否為模態對話框 | true |
| ShowButtons | bool | 取得或設定是否顯示按鈕區域 | true |
| ShowCloseButton | bool | 取得或設定是否顯示關閉按鈕 | true |
| OkButtonText | string | 取得或設定確定按鈕文字 | "確定" |
| CancelButtonText | string | 取得或設定取消按鈕文字 | "取消" |
| ReturnValue | object | 取得或設定對話框回傳值 | null |
| DataSource | object | 取得或設定傳遞給內容頁面的資料 | null |
| CenterScreen | bool | 取得或設定是否置中顯示 | true |
| CloseOnEsc | bool | 取得或設定是否按 ESC 關閉 | true |
| ShowAnimation | string | 取得或設定顯示動畫效果 | "fade" |
| CloseAnimation | string | 取得或設定關閉動畫效果 | "fade" |

## 方法說明

PopupDialog 提供以下方法：

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Show | 無 | void | 顯示對話框 |
| Close | dialogResult (選填) | void | 關閉對話框，可以指定對話框結果 |
| LoadContent | url | void | 載入指定 URL 的內容到對話框 |
| SetTitle | title | void | 設定對話框標題 |
| SetSize | width, height | void | 設定對話框尺寸 |
| SetButtons | showButtons, okText, cancelText | void | 設定按鈕顯示和文字 |
| SetPosition | x, y | void | 設定對話框位置 |
| CenterToScreen | 無 | void | 將對話框置中顯示 |
| SetReturnValue | value | void | 設定對話框回傳值 |
| GetDataSource | 無 | object | 取得傳遞給對話框的資料來源 |
| Maximize | 無 | void | 最大化對話框 |
| Restore | 無 | void | 還原對話框原始大小 |

## 事件說明

PopupDialog 支援以下事件：

| 事件名稱 | 事件參數 | 說明 |
|---------|---------|------|
| DialogLoaded | EventArgs | 對話框載入完成時觸發 |
| DialogClosing | DialogClosingEventArgs | 對話框關閉前觸發，可以取消關閉操作 |
| DialogClosed | DialogResultEventArgs | 對話框關閉後觸發，包含對話框結果 |
| OkButtonClick | EventArgs | 確定按鈕點擊時觸發 |
| CancelButtonClick | EventArgs | 取消按鈕點擊時觸發 |

### 事件參數說明

**DialogClosingEventArgs** 包含：
- DialogResult: 對話框結果
- Cancel: 可設為 true 取消關閉操作

**DialogResultEventArgs** 包含：
- DialogResult: 對話框結果
- ReturnValue: 對話框回傳值

## 使用範例

以下是 PopupDialog 的使用範例：

### 開啟對話框

```csharp
// 簡單開啟對話框
protected void btnOpenDialog_Click(object sender, EventArgs e)
{
    string url = "UserSelector.aspx";
    string script = string.Format("openPopupDialog('{0}', '選擇使用者', 600, 400);", url);
    ClientScript.RegisterStartupScript(this.GetType(), "OpenDialog", script, true);
}

// 開啟對話框並傳遞參數
protected void btnEditRecord_Click(object sender, EventArgs e)
{
    string url = string.Format("RecordEditor.aspx?id={0}", recordId);
    string script = string.Format("openPopupDialog('{0}', '編輯記錄', 800, 500, true, '{1}');",
        url, Resources.UI.EditRecord);
    ClientScript.RegisterStartupScript(this.GetType(), "OpenDialog", script, true);
}
```

### 處理對話框結果

```csharp
// 在父頁面中處理對話框結果
<script type="text/javascript">
    function onDialogClosed(result, returnValue) {
        if (result == 'ok') {
            // 處理確定按鈕的情況
            if (returnValue) {
                // 使用回傳值更新頁面資料
                document.getElementById('<%= txtSelectedUser.ClientID %>').value = returnValue.UserName;
                document.getElementById('<%= hdnUserId.ClientID %>').value = returnValue.UserId;
            }
        }
    }
</script>
```

### 在內容頁面中設定回傳值

```csharp
// 在對話框內容頁面中
protected void btnSelect_Click(object sender, EventArgs e)
{
    // 建立回傳資料
    var userData = new {
        UserId = selectedUserId,
        UserName = selectedUserName
    };
    
    // 轉換為 JSON 並設為回傳值
    string returnValue = Newtonsoft.Json.JsonConvert.SerializeObject(userData);
    
    // 關閉對話框並回傳值
    string script = string.Format("closeDialogWithResult('ok', {0});", returnValue);
    ClientScript.RegisterStartupScript(this.GetType(), "CloseDialog", script, true);
}
```

## 使用情境與最佳實踐

1. **資料選擇對話框**
   - 開啟對話框顯示資料選擇列表
   - 使用者選擇後回傳選取的資料到父頁面
   - 父頁面更新相關欄位

2. **確認對話框**
   - 執行重要操作前顯示確認對話框
   - 確認後繼續操作，取消則停止

3. **資料編輯對話框**
   - 開啟對話框顯示編輯表單
   - 完成編輯後回傳更新的資料
   - 父頁面使用回傳資料更新界面

4. **多層對話框**
   - 在主對話框中開啟次級對話框
   - 各層對話框間保持獨立的資料傳遞

## 效能考量

1. **延遲載入策略**：對話框內容採用延遲載入，避免一次載入過多資源
2. **對話框池化**：重複使用已建立的對話框實例，減少 DOM 操作
3. **資源管理**：關閉對話框時釋放相關資源，避免記憶體洩漏
4. **樣式隔離**：使用獨立的樣式表，避免對主頁面樣式造成影響

## 安全性考量

1. **內容驗證**：驗證載入到對話框的內容來源
2. **參數過濾**：對傳遞給對話框的參數進行過濾和驗證
3. **權限檢查**：在開啟包含敏感功能的對話框前進行權限檢查
4. **資料傳遞加密**：對傳遞的敏感資料進行加密處理

## 可訪問性支援

1. **鍵盤操作**：支援使用 Tab 鍵導航和 Escape 鍵關閉
2. **ARIA 標籤**：使用適當的 ARIA 屬性提升可訪問性
3. **屏幕閱讀器支援**：對話框內容和按鈕均支援屏幕閱讀器

## 已知問題與限制

1. 在某些舊版瀏覽器中，動畫效果可能不完全支援
2. 多層對話框時，若資源處理不當可能導致效能下降
3. 動態高度調整在某些情境下可能計算不準確
4. 高解析度大螢幕下，預設尺寸可能顯得較小

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/15 | 1.0    | 首次建立彈出對話框規格書        | Claude AI  | 