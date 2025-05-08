# YearList 年度清單規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | YearList                              |
| 程式名稱     | 年度清單                                |
| 檔案大小     | 477B                                  |
| 行數        | ~15                                   |
| 功能簡述     | 提供年度選擇功能                          |
| 複雜度       | 低                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/14                             |

## 程式功能概述

YearList 是泛太總帳系統中的年度選擇控制項，專門設計用於提供統一且一致的年度選擇功能。此控制項主要用於各種報表、查詢和資料維護畫面中，使用者可以通過它選擇特定的會計年度或日曆年度。控制項支援自訂年度範圍、預設值設定、必選項檢查，以及與其他相關控制項（如月份和週次）的協同工作。主要功能包括：

1. 提供年度下拉式選單，預設顯示最近數年（通常是當前年度前後幾年）
2. 支援自訂年度範圍設定
3. 提供年度選擇事件，方便連動更新其他控制項
4. 支援唯讀模式顯示
5. 提供必填/選填設定選項
6. 自動依據系統設定判斷使用會計年度或日曆年度
7. 支援標籤文字自訂
8. 整合系統樣式，提供一致的使用者體驗
9. 支援預設值設定和自動回填功能
10. 提供快速選擇當前年度的功能鈕

此控制項通常與 PeriodList（期間清單）和 MonthList（月份清單）等控制項組合使用，共同構成完整的時間選擇功能。

## 控制項結構說明

YearList 控制項由以下元素組成：

1. **標籤區域**：顯示「年度:」或自訂文字標籤
2. **選擇區域**：包含年度下拉式選單
3. **輔助按鈕**：可選的「本年」快速選擇按鈕

控制項設計簡潔，集中於核心年度選擇功能，遵循系統整體 UI 風格設計。

## 頁面元素

### ASCX 頁面宣告

```html
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YearList.ascx.cs" Inherits="YearList" %>
```

### 頁面結構

```html
<div class="form-group year-list-container">
    <asp:Label ID="lblYear" runat="server" Text="年度:" CssClass="control-label"></asp:Label>
    <div class="control-wrapper">
        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
            AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvYear" runat="server" 
            ControlToValidate="ddlYear" InitialValue="" 
            ErrorMessage="年度為必填欄位" Display="Dynamic" 
            CssClass="validation-error" Enabled="false">
        </asp:RequiredFieldValidator>
    </div>
    <asp:LinkButton ID="btnCurrentYear" runat="server" CssClass="btn btn-link current-year-btn"
        OnClick="btnCurrentYear_Click" ToolTip="選擇當前年度" Text="本年">
    </asp:LinkButton>
</div>
```

## 技術實現

YearList 控制項基於以下技術實現：

1. **ASP.NET Web Forms**：使用 ASP.NET 使用者控制項（.ascx）技術
2. **HTML/CSS**：使用標準 HTML 元素和 CSS 樣式定義界面
3. **C# 程式碼**：在後端實現控制項的核心邏輯（詳見 YearList.ascx.cs）
4. **DropDownList 控制項**：使用 ASP.NET 內建下拉式選單控制項
5. **驗證控制項**：使用 RequiredFieldValidator 實現必填驗證

## 相依控制項和檔案

YearList 控制項的實現依賴以下元件和檔案：

1. **YearList.ascx.cs**：控制項的後端程式碼
2. **System.Web.UI.WebControls**：提供基本網頁控制項功能
3. **System.Web.UI.HtmlControls**：提供 HTML 控制項功能
4. **系統樣式表**：提供統一的外觀和風格

## 使用者介面

YearList 控制項的使用者介面由以下部分組成：

1. **標籤文字**：顯示「年度:」或自訂文字，提示使用者此控制項的用途
2. **下拉式選單**：顯示可選擇的年度清單，默認顯示當前年度的前後五年
3. **驗證提示**：當控制項設為必填但未選擇時顯示錯誤提示
4. **本年按鈕**：可選的快速選擇當前年度的按鈕

整體設計符合系統UI風格，提供清晰的視覺層次和直覺的操作方式。

## 屬性說明

YearList 控制項提供以下可設定的屬性：

| 屬性名稱 | 資料類型 | 說明 | 預設值 |
|---------|---------|------|--------|
| SelectedYear | int | 取得或設定目前選擇的年度 | 當前年度 |
| StartYear | int | 取得或設定年度範圍的起始年 | 當前年度-5 |
| EndYear | int | 取得或設定年度範圍的結束年 | 當前年度+5 |
| Required | bool | 取得或設定控制項是否為必填 | false |
| ReadOnly | bool | 取得或設定控制項是否為唯讀 | false |
| LabelText | string | 取得或設定控制項的標籤文字 | "年度:" |
| ShowCurrentYearButton | bool | 取得或設定是否顯示本年按鈕 | true |
| Width | Unit | 取得或設定控制項的顯示寬度 | 預設寬度 |
| CssClass | string | 取得或設定控制項的 CSS 類別 | 空字串 |
| AutoPostBack | bool | 取得或設定選擇變更時是否自動回傳 | true |
| ValidationGroup | string | 取得或設定控制項的驗證群組 | 空字串 |

## 方法說明

YearList 控制項提供以下方法：

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Initialize | startYear, endYear, selectedYear | void | 初始化年度選單，設定年度範圍和選中值 |
| SetYear | year | void | 設定選擇的年度 |
| GetYear | 無 | int | 取得目前選擇的年度 |
| Clear | 無 | void | 清除年度選擇 |
| RefreshYearList | 無 | void | 重新載入年度清單 |
| SetReadOnly | isReadOnly | void | 設定控制項是否為唯讀 |
| SetRequired | isRequired | void | 設定控制項是否為必填 |
| Validate | 無 | bool | 驗證控制項是否符合必填要求 |

## 事件說明

YearList 控制項支援以下事件：

| 事件名稱 | 事件參數 | 說明 |
|---------|---------|------|
| YearChanged | YearChangedEventArgs | 當選擇的年度變更時觸發 |

### 事件參數說明

**YearChangedEventArgs** 包含：
- OldYear: 變更前的年度值
- NewYear: 變更後的年度值

## 使用範例

以下是 YearList 控制項的使用範例：

### ASPX 頁面引用

```aspx
<%@ Register Src="~/Controls/YearList.ascx" TagName="YearList" TagPrefix="uc" %>

<div class="form-group">
    <uc:YearList ID="yearList" runat="server" Required="true" OnYearChanged="yearList_YearChanged" />
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
        yearList.LabelText = "會計年度:";
        yearList.StartYear = 2020;
        yearList.EndYear = 2030;
        yearList.Required = true;
        
        // 註冊事件處理器
        yearList.YearChanged += new EventHandler<YearChangedEventArgs>(yearList_YearChanged);
        
        // 設定初始值
        yearList.SetYear(2025);
    }
}

// 處理年度變更事件
protected void yearList_YearChanged(object sender, YearChangedEventArgs e)
{
    // 更新其他相關控制項
    periodList.LoadPeriodsByYear(e.NewYear);
    
    // 顯示選擇的年度
    lblResult.Text = string.Format("選擇的年度從 {0} 變更為 {1}", e.OldYear, e.NewYear);
}

// 從控制項獲取值
protected void btnSubmit_Click(object sender, EventArgs e)
{
    if (yearList.Validate())
    {
        int selectedYear = yearList.SelectedYear;
        // 處理選擇的年度
    }
}
```

## 使用情境與最佳實踐

1. **報表查詢頁面**
   ```csharp
   // 設定報表的年度參數
   int reportYear = yearList.SelectedYear;
   reportViewer.Parameters["ReportYear"].Value = reportYear;
   ```

2. **搭配期間清單使用**
   ```csharp
   yearList.YearChanged += (s, e) => {
       periodList.LoadPeriodsByYear(e.NewYear);
   };
   ```

3. **唯讀模式顯示**
   ```csharp
   yearList.ReadOnly = true;
   yearList.SetYear(transaction.FiscalYear);
   ```

4. **設定特定年度範圍**
   ```csharp
   // 只顯示過去五年
   int currentYear = DateTime.Now.Year;
   yearList.StartYear = currentYear - 5;
   yearList.EndYear = currentYear;
   yearList.RefreshYearList();
   ```

## 效能考量

1. **初始化優化**：控制項僅在需要時才載入年度清單
2. **資料傳輸**：年度選擇僅傳輸簡單的數值資料，網路負荷極小
3. **記憶體使用**：控制項使用的記憶體極少，年度清單項目通常不多

## 安全性考量

1. **輸入驗證**：嚴格驗證使用者輸入，確保年度值為有效數字
2. **範圍限制**：可設定合理的年度範圍，防止無效的年度選擇
3. **權限控制**：可與系統權限結合，在適當情況下啟用唯讀模式

## 可訪問性支援

1. **鍵盤導航**：支援使用 Tab 鍵導航和箭頭鍵選擇
2. **螢幕閱讀器支援**：標籤正確關聯到控制項，支援螢幕閱讀器
3. **高對比度支援**：控制項樣式支援高對比度顯示模式

## 已知問題與限制

1. 目前年度範圍需手動設定，未提供自動從資料庫中讀取有效年度的功能
2. 控制項寬度在不同瀏覽器可能略有差異
3. 在某些複雜頁面中，事件處理可能導致性能略微下降

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/14 | 1.0    | 首次建立年度清單規格書          | Claude AI  | 