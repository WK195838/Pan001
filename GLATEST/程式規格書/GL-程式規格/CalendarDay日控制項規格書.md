# CalendarDay 日控制項規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CalendarDay                            |
| 程式名稱     | 日控制項                                |
| 檔案大小     | 1.8KB                                 |
| 行數        | ~50                                   |
| 功能簡述     | 提供日選擇功能                           |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/12                             |

## 程式功能概述

CalendarDay 是泛太總帳系統中的日選擇控制項，專門用於處理日期中「日」的選擇功能。此控制項提供使用者友好的介面，允許使用者從下拉式選單或直接輸入方式選擇日期中的「日」部分，同時具備輸入驗證和互動響應能力。主要功能包括：

1. 提供 1-31 日期範圍選擇（根據月份自動調整可選範圍）
2. 支援兩種選擇方式：下拉選單和直接輸入
3. 自動根據所選月份調整最大日數（例如：2月份為28/29日，4月為30日等）
4. 支援閏年計算，正確顯示2月天數
5. 提供必填/選填設定選項
6. 支援唯讀模式顯示
7. 整合系統樣式，提供一致的使用者體驗
8. 支援標籤文字自訂
9. 提供資料驗證功能
10. 支援事件通知，方便與其他控制項互動

此控制項通常與 CalendarMonth 和 CalendarYear 等其他日期相關控制項組合使用，共同構成完整的日期選擇功能。

## 使用者介面說明

CalendarDay 控制項的使用者介面由以下元素組成：

1. **標籤文字**：顯示「日」或自訂文字，提示使用者此控制項的用途
2. **輸入文字框**：允許使用者直接輸入日數值（範圍1-31，根據月份不同有所調整）
3. **下拉選單按鈕**：點擊後顯示可選的日數值清單
4. **下拉選單**：顯示當月可選的所有日數值，使用者可直接點選
5. **驗證提示**：當輸入無效日期時顯示錯誤提示

整體設計符合系統UI風格，提供清晰的視覺層次和直覺的操作方式。控制項在唯讀模式下僅顯示當前選擇的日期，不提供編輯功能。

## 技術實現

CalendarDay 控制項基於以下技術實現：

1. **ASP.NET Web Forms**：使用 ASP.NET 使用者控制項（.ascx）技術
2. **HTML/CSS**：使用標準 HTML 元素和 CSS 樣式定義界面
3. **JavaScript**：提供客戶端的互動功能和初步驗證
4. **C# 程式碼**：在後端實現控制項的核心邏輯（詳見 CalendarDay.ascx.cs）
5. **AJAX**：支援無刷新更新控制項狀態

## 相依控制項和檔案

CalendarDay 控制項的實現依賴以下元件和檔案：

1. **CalendarDay.ascx.cs**：控制項的後端程式碼
2. **系統樣式表**：提供統一的外觀和風格
3. **pagefunction.js**：系統共用的 JavaScript 函數庫
4. **CalendarMonth**：通常與月份控制項協同工作
5. **CalendarYear**：通常與年份控制項協同工作，特別是閏年判斷

## 檔案結構

CalendarDay.ascx 的檔案結構如下：

```html
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarDay.ascx.cs" Inherits="CalendarDay" %>

<div class="calendar-control calendar-day-control">
    <asp:Label ID="lblDay" runat="server" Text="日:" CssClass="control-label"></asp:Label>
    <div class="control-container">
        <asp:TextBox ID="txtDay" runat="server" CssClass="form-control day-input" 
            AutoPostBack="true" OnTextChanged="txtDay_TextChanged" MaxLength="2"></asp:TextBox>
        <asp:DropDownList ID="ddlDay" runat="server" CssClass="form-control day-dropdown" 
            AutoPostBack="true" OnSelectedIndexChanged="ddlDay_SelectedIndexChanged"></asp:DropDownList>
        <asp:CustomValidator ID="cvDay" runat="server" 
            ControlToValidate="txtDay" 
            OnServerValidate="cvDay_ServerValidate"
            ErrorMessage="請輸入有效的日期" 
            Display="Dynamic" 
            CssClass="validation-error"></asp:CustomValidator>
    </div>
</div>
```

## 屬性說明

CalendarDay 控制項提供以下可設定的屬性：

| 屬性名稱 | 資料類型 | 說明 | 預設值 |
|---------|---------|------|--------|
| SelectedDay | int | 取得或設定目前選擇的日 | 0 |
| Required | bool | 取得或設定控制項是否為必填 | false |
| ReadOnly | bool | 取得或設定控制項是否為唯讀 | false |
| SelectedMonth | int | 取得或設定所選月份（用於計算當月最大天數） | 0 |
| SelectedYear | int | 取得或設定所選年份（用於閏年計算） | 0 |
| LabelText | string | 取得或設定控制項的標籤文字 | "日:" |
| Width | Unit | 取得或設定控制項的顯示寬度 | 預設寬度 |
| CssClass | string | 取得或設定控制項的 CSS 類別 | 空字串 |
| IsValid | bool | 取得控制項是否通過驗證（唯讀） | true |

## 事件說明

CalendarDay 控制項支援以下事件：

| 事件名稱 | 事件參數 | 說明 |
|---------|---------|------|
| DayChanged | DayChangedEventArgs | 當選擇的日變更時觸發 |
| DayValidated | DayValidatedEventArgs | 當控制項驗證完成時觸發 |

### 事件參數說明

**DayChangedEventArgs** 包含：
- OldDay: 變更前的日值
- NewDay: 變更後的日值

**DayValidatedEventArgs** 包含：
- IsValid: 指示日值是否有效

## 使用範例

以下是 CalendarDay 控制項的使用範例：

### ASPX 頁面引用

```aspx
<%@ Register Src="~/Controls/CalendarDay.ascx" TagName="CalendarDay" TagPrefix="uc" %>

<div class="form-group">
    <uc:CalendarDay ID="dayControl" runat="server" Required="true" />
</div>
```

### 程式碼設定

```csharp
// 初始化設定
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 設定控制項屬性
        dayControl.LabelText = "日期:";
        dayControl.SelectedMonth = 5; // 5月
        dayControl.SelectedYear = 2025;
        dayControl.Required = true;
        
        // 註冊事件處理器
        dayControl.DayChanged += new EventHandler<DayChangedEventArgs>(dayControl_DayChanged);
        
        // 更新最大日值
        dayControl.UpdateMaxDayForMonth();
        
        // 設定初始值
        dayControl.SetDay(15);
    }
}

// 處理日變更事件
protected void dayControl_DayChanged(object sender, DayChangedEventArgs e)
{
    // 顯示選擇的日
    lblResult.Text = string.Format("選擇的日從 {0} 變更為 {1}", e.OldDay, e.NewDay);
}

// 從控制項獲取值
protected void btnSubmit_Click(object sender, EventArgs e)
{
    if (dayControl.Validate())
    {
        int selectedDay = dayControl.SelectedDay;
        // 處理選擇的日值
    }
}
```

## 使用情境與最佳實踐

1. **搭配其他日期控制項使用**
   ```csharp
   yearControl.YearChanged += (s, e) => {
       dayControl.SelectedYear = e.NewYear;
       dayControl.UpdateMaxDayForMonth();
   };
   
   monthControl.MonthChanged += (s, e) => {
       dayControl.SelectedMonth = e.NewMonth;
       dayControl.UpdateMaxDayForMonth();
   };
   ```

2. **在唯讀表單中使用**
   ```csharp
   dayControl.ReadOnly = true;
   dayControl.SetDay(transactionDate.Day);
   ```

3. **動態設定必填性**
   ```csharp
   dayControl.Required = (dateTypeDropDown.SelectedValue == "FullDate");
   ```

## 效能考量

1. **客戶端驗證**：控制項使用客戶端 JavaScript 驗證，減少伺服器回傳
2. **懶加載**：下拉選單選項僅在需要時才載入
3. **事件觸發優化**：只在值真正變更時才觸發事件

## 安全性考量

1. **輸入驗證**：嚴格驗證使用者輸入，僅接受有效的日期值
2. **跨站腳本預防**：所有使用者輸入內容在顯示前經過編碼處理
3. **權限控制**：遵循系統權限設定，在適當情況下啟用唯讀模式

## 可訪問性支援

1. **鍵盤導航**：支援使用 Tab 鍵導航和箭頭鍵選擇
2. **螢幕閱讀器支援**：使用適當的 ARIA 標籤提升可訪問性
3. **高對比度支援**：控制項樣式支援高對比度顯示模式

## 已知問題與限制

1. 在某些較舊的瀏覽器中，可能出現樣式不一致的情況
2. 年份和月份必須先設定，才能正確限制可選日數範圍
3. 事件處理在某些複雜頁面可能導致性能略微下降

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/12 | 1.0    | 首次建立日控制項規格書          | Claude AI  | 