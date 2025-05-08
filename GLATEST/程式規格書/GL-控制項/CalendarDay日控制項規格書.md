# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | CalendarDay |
| 程式名稱 | 日控制項 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Controls/CalendarDay.ascx, /GLATEST/app/Controls/CalendarDay.ascx.cs |
| 程式類型 | 使用者控制項 (User Control) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

CalendarDay 是泛太總帳系統的日選擇控制項，專門用於選擇特定日期中的「日」部分。此控制項主要適用於需要單獨選擇日期中「日」的情境，例如設定每月固定日期的帳務處理、系統排程設定或報表產生條件等。相較於完整的日期選擇器，此控制項更加輕量且專注於特定用途。

### 2.2 業務流程

CalendarDay 在系統中扮演以下角色：
1. 提供簡潔的日期「日」選擇介面
2. 支援營業日與非營業日識別
3. 驗證日期範圍合理性（1-31）
4. 與其他日期元素（年、月）協同工作
5. 處理不同月份天數的差異（28/29/30/31天）
6. 支援月結日特殊標記

### 2.3 使用頻率

- 中頻率：用於特定設定頁面和報表參數設定
- 與年月選擇控制項搭配使用，構成日期選擇的完整功能
- 平均每位使用者每週會使用數次，主要在設定固定日期的條件時

### 2.4 使用者角色

此控制項服務於系統的特定角色，包括：
- 系統管理員：設定系統排程和處理日
- 財務主管：設定財務結帳日和報表產出時間
- 會計人員：設定每月固定日期的交易條件
- 一般用戶：查詢特定日的資料

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Forms (.NET Framework 4.0)
- 主要技術：
  - HTML/CSS 構建介面
  - JavaScript 實現互動效果
  - C# 後端邏輯處理
- 客戶端驗證：確保選擇的日期範圍有效
- 與其他日曆控制項的整合：可以與 CalendarMonth、CalendarYear 搭配使用

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_PARAMETER | 系統參數表，讀取日期相關設定 | 讀取 |
| SYS_USER_CONFIG | 使用者配置表，讀取用戶偏好設定 | 讀取 |
| GL_CALENDAR | 會計行事曆表，識別營業日和非營業日 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供系統參數查詢 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| SysParamManager | 系統參數管理 | 取得日期相關參數設定 |
| CalendarMonth | 月份控制項 | 可選整合，提供月份選擇 |

## 4. 控制項設計

### 4.1 界面結構

CalendarDay 控制項的界面由以下元素構成：

```
+-------------------------------------+
| [文字標籤] [下拉選單/輸入框] [選項按鈕] |
+-------------------------------------+
```

### 4.2 屬性設計

| 屬性名稱 | 類型 | 存取修飾詞 | 說明 |
|---------|------|----------|------|
| DayValue | int | public | 控制項的日值（1-31） |
| DayString | string | public | 格式化後的日字串 |
| IsRequired | bool | public | 是否為必填欄位 |
| MinDay | int | public | 允許的最小日（預設: 1） |
| MaxDay | int | public | 允許的最大日（預設: 31） |
| ShowLabel | bool | public | 是否顯示文字標籤 |
| LabelText | string | public | 標籤文字 |
| LabelWidth | int | public | 標籤寬度 |
| InputWidth | int | public | 輸入框寬度 |
| ValidationGroup | string | public | 驗證群組 |
| AutoAdjustToMonth | bool | public | 是否根據所選月份自動調整可選日範圍 |
| LinkedMonthControlID | string | public | 關聯月份控制項ID |
| LinkedYearControlID | string | public | 關聯年份控制項ID |
| DisplayMode | DayDisplayMode | public | 顯示模式（下拉選單/輸入框） |
| BusinessDayOnly | bool | public | 是否只允許選擇營業日 |

### 4.3 參數定義

CalendarDay 控制項支援以下WebForm參數：

```
<%@ Register Src="~/Controls/CalendarDay.ascx" TagPrefix="pan" TagName="CalendarDay" %>
<pan:CalendarDay ID="txtDay" runat="server" 
                DayValue="15" 
                IsRequired="true"
                LabelText="日期(日):"
                LabelWidth="100"
                InputWidth="80"
                AutoAdjustToMonth="true"
                LinkedMonthControlID="txtMonth"
                LinkedYearControlID="txtYear"
                ValidationGroup="vgDate" />
```

### 4.4 CSS設計

控制項使用以下核心CSS類別：

```css
.calday-container { /* 容器 */ }
.calday-label { /* 文字標籤 */ }
.calday-input { /* 日輸入框 */ }
.calday-dropdown { /* 下拉選單 */ }
.calday-button { /* 選項按鈕 */ }
.calday-error { /* 錯誤提示 */ }
.calday-required { /* 必填標記 */ }
.calday-disabled { /* 禁用狀態 */ }
.calday-weekend { /* 週末樣式 */ }
.calday-today { /* 今日樣式 */ }
.calday-holiday { /* 假日樣式 */ }
.calday-business { /* 營業日樣式 */ }
```

## 5. 主要方法

### 5.1 初始化和載入方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入時初始化控制項 | object sender, EventArgs e | void |
| InitializeCalendarDay | 初始化日控制項 | 無 | void |
| LoadParameters | 載入系統參數設定 | 無 | void |
| PopulateDayOptions | 填充日期選項 | 無 | void |
| SetupValidation | 設置驗證控制項 | 無 | void |
| RegisterClientScript | 註冊客戶端腳本 | 無 | void |

### 5.2 事件處理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| DayChanged | 處理日變更事件 | object sender, EventArgs e | void |
| OnMonthChanged | 處理關聯月份變更事件 | object sender, EventArgs e | void |
| OnYearChanged | 處理關聯年份變更事件 | object sender, EventArgs e | void |
| OnDaySelected | 觸發日選擇事件 | 無 | void |
| ValidateDay | 驗證日範圍 | 無 | bool |
| CheckBusinessDay | 檢查是否為營業日 | int day, int month, int year | bool |

### 5.3 工具方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| GetMonthMaxDay | 獲取指定月份的最大天數 | int month, int year | int |
| IsLeapYear | 檢查是否為閏年 | int year | bool |
| AdjustDayToMonth | 根據所選月份調整日 | int day, int month, int year | int |
| GetBusinessDaysInMonth | 獲取指定月份的營業日 | int month, int year | List\<int\> |
| GetRelatedControlValue | 獲取關聯控制項的值 | string controlID, string defaultValue | string |
| FindControl | 查找指定ID的控制項 | string controlID | Control |

### 5.4 渲染方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| RenderControl | 渲染控制項 | HtmlTextWriter writer | void |
| RenderLabel | 渲染標籤 | HtmlTextWriter writer | void |
| RenderInput | 渲染輸入框 | HtmlTextWriter writer | void |
| RenderDropDown | 渲染下拉選單 | HtmlTextWriter writer | void |
| RenderValidator | 渲染驗證控制項 | HtmlTextWriter writer | void |
| ApplyStyles | 應用樣式 | 無 | void |

## 6. 處理邏輯

### 6.1 初始化流程

```
開始
 ↓
檢查頁面是否為回傳 → 是 → 恢復控制項狀態 → 跳至「設置驗證控制項」
 ↓
初始化控制項屬性
 ↓
載入系統參數設定
 ↓
檢查關聯控制項 → 有關聯控制項 → 註冊關聯事件
 ↓
設置最小最大日範圍
 ↓
填充日期選項
 ↓
設置驗證控制項
 ↓
註冊客戶端腳本
 ↓
應用樣式設定
 ↓
結束
```

### 6.2 日期選擇流程

```
開始
 ↓
使用者選擇日
 ↓
檢查日有效性 → 無效 → 顯示錯誤訊息 → 結束
 ↓
更新DayValue值
 ↓
若有月份關聯控制項 → 檢查日是否符合月份天數 → 不符合 → 調整日期
 ↓
檢查營業日設定 → 設為僅營業日 → 檢查所選日是否營業日 → 不是 → 提示
 ↓
觸發DayChanged事件
 ↓
結束
```

### 6.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 日期範圍無效 | 顯示範圍提示，自動調整到有效範圍 | 提示有效範圍，紅色強調輸入框 |
| 月份天數不符 | 自動調整到月份最大天數 | 彈出提示訊息並自動調整 |
| 非營業日選擇 | 顯示營業日提示，可設置是否允許選擇 | 顯示警告提示，營業日使用不同顏色表示 |
| 日期為空但必填 | 顯示必填提示，阻止表單提交 | 提示必須選擇日期，紅色強調輸入框 |
| 客戶端JavaScript錯誤 | 降級為純文字輸入 | 退回到基本輸入功能，保持可用性 |

## 7. 代碼說明

### 7.1 控制項註冊與頁面使用

```aspx
<%-- 在頁面中註冊與使用CalendarDay控制項 --%>
<%@ Register Src="~/Controls/CalendarDay.ascx" TagPrefix="pan" TagName="CalendarDay" %>
<%@ Register Src="~/Controls/CalendarMonth.ascx" TagPrefix="pan" TagName="CalendarMonth" %>
<%@ Register Src="~/Controls/CalendarYear.ascx" TagPrefix="pan" TagName="CalendarYear" %>

<div class="form-group">
    <pan:CalendarYear ID="txtYear" runat="server" 
                    YearValue='<%# DateTime.Now.Year %>' 
                    IsRequired="true"
                    LabelText="年度:"
                    ValidationGroup="vgDateParts" />
                    
    <pan:CalendarMonth ID="txtMonth" runat="server" 
                    MonthValue='<%# DateTime.Now.Month %>' 
                    IsRequired="true"
                    LabelText="月份:"
                    ValidationGroup="vgDateParts" />
                    
    <pan:CalendarDay ID="txtDay" runat="server" 
                    DayValue='<%# DateTime.Now.Day %>' 
                    IsRequired="true"
                    LabelText="日期:"
                    LinkedMonthControlID="txtMonth"
                    LinkedYearControlID="txtYear"
                    ValidationGroup="vgDateParts" />
</div>
```

### 7.2 初始化實現

```csharp
/// <summary>
/// 初始化日控制項
/// </summary>
private void InitializeCalendarDay()
{
    try
    {
        // 載入系統參數設定
        LoadParameters();
        
        // 如果未設定日值，使用今天的日
        if (DayValue <= 0)
        {
            DayValue = DateTime.Today.Day;
        }
        
        // 設置最小與最大日範圍
        if (MinDay <= 0)
        {
            MinDay = 1;
        }
        
        if (MaxDay <= 0 || MaxDay > 31)
        {
            MaxDay = 31;
        }
        
        // 更新日字串
        DayString = DayValue.ToString("00");
        
        // 尋找關聯控制項並註冊事件
        if (!string.IsNullOrEmpty(LinkedMonthControlID))
        {
            Control monthControl = FindMonthControl(LinkedMonthControlID);
            if (monthControl != null && monthControl is CalendarMonth)
            {
                CalendarMonth calMonth = (CalendarMonth)monthControl;
                // 註冊月份變更事件
                calMonth.MonthChanged += new EventHandler(OnMonthChanged);
            }
        }
        
        if (!string.IsNullOrEmpty(LinkedYearControlID))
        {
            Control yearControl = FindYearControl(LinkedYearControlID);
            if (yearControl != null && yearControl is CalendarYear)
            {
                CalendarYear calYear = (CalendarYear)yearControl;
                // 註冊年份變更事件
                calYear.YearChanged += new EventHandler(OnYearChanged);
            }
        }
        
        // 設置控制項ID和屬性
        if (DisplayMode == DayDisplayMode.DropDown)
        {
            ddlDay.ID = ID + "_DropDown";
            ddlDay.Visible = true;
            txtDay.Visible = false;
            
            // 填充下拉選單選項
            PopulateDayOptions();
            
            // 設置選中值
            ddlDay.SelectedValue = DayString;
        }
        else
        {
            txtDay.ID = ID + "_Input";
            txtDay.Text = DayString;
            txtDay.Visible = true;
            ddlDay.Visible = false;
        }
        
        // 設置標籤
        if (ShowLabel)
        {
            lblDay.Text = LabelText;
            lblDay.Width = Unit.Pixel(LabelWidth);
            
            if (IsRequired)
            {
                lblDay.Text += "<span class='calday-required'>*</span>";
            }
        }
        else
        {
            lblDay.Visible = false;
        }
        
        // 設置輸入框寬度
        if (InputWidth > 0)
        {
            if (DisplayMode == DayDisplayMode.DropDown)
            {
                ddlDay.Width = Unit.Pixel(InputWidth);
            }
            else
            {
                txtDay.Width = Unit.Pixel(InputWidth);
            }
        }
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDay.InitializeCalendarDay Error", ex);
    }
}
```

### 7.3 日期驗證實現

```csharp
/// <summary>
/// 驗證日範圍
/// </summary>
/// <returns>是否驗證通過</returns>
private bool ValidateDay()
{
    try
    {
        string dayText = (DisplayMode == DayDisplayMode.DropDown) ? 
                        ddlDay.SelectedValue : txtDay.Text;
        
        // 檢查必填
        if (IsRequired && string.IsNullOrEmpty(dayText))
        {
            SetErrorMessage("日期欄位必須填寫");
            return false;
        }
        
        // 如果為空且非必填，視為有效
        if (!IsRequired && string.IsNullOrEmpty(dayText))
        {
            return true;
        }
        
        // 檢查是否為數字
        int parsedDay;
        if (!int.TryParse(dayText, out parsedDay))
        {
            SetErrorMessage("日期必須為數字");
            return false;
        }
        
        // 檢查範圍
        if (parsedDay < MinDay)
        {
            SetErrorMessage($"日期不能小於 {MinDay}");
            return false;
        }
        
        // 檢查當前月份的最大日
        int maxDayForMonth = MaxDay;
        if (AutoAdjustToMonth)
        {
            int month = GetCurrentMonth();
            int year = GetCurrentYear();
            maxDayForMonth = GetMonthMaxDay(month, year);
        }
        
        if (parsedDay > maxDayForMonth)
        {
            SetErrorMessage($"日期不能大於 {maxDayForMonth}");
            return false;
        }
        
        // 檢查是否為營業日
        if (BusinessDayOnly)
        {
            int month = GetCurrentMonth();
            int year = GetCurrentYear();
            
            if (!CheckBusinessDay(parsedDay, month, year))
            {
                SetErrorMessage($"{parsedDay} 不是營業日");
                return false;
            }
        }
        
        // 更新日值
        DayValue = parsedDay;
        DayString = DayValue.ToString("00");
        
        return true;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDay.ValidateDay Error", ex);
        SetErrorMessage("日期驗證過程發生錯誤");
        return false;
    }
}
```

### 7.4 月份天數調整實現

```csharp
/// <summary>
/// 根據所選月份調整日
/// </summary>
/// <param name="day">當前日</param>
/// <param name="month">月份</param>
/// <param name="year">年份</param>
/// <returns>調整後的日</returns>
private int AdjustDayToMonth(int day, int month, int year)
{
    try
    {
        // 獲取指定月份的最大天數
        int maxDay = GetMonthMaxDay(month, year);
        
        // 如果當前日大於最大天數，調整為最大天數
        if (day > maxDay)
        {
            return maxDay;
        }
        
        return day;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDay.AdjustDayToMonth Error", ex);
        
        // 出錯時返回原值
        return day;
    }
}

/// <summary>
/// 獲取指定月份的最大天數
/// </summary>
/// <param name="month">月份</param>
/// <param name="year">年份</param>
/// <returns>最大天數</returns>
private int GetMonthMaxDay(int month, int year)
{
    switch (month)
    {
        case 2:
            return IsLeapYear(year) ? 29 : 28;
        case 4:
        case 6:
        case 9:
        case 11:
            return 30;
        default:
            return 31;
    }
}

/// <summary>
/// 檢查是否為閏年
/// </summary>
/// <param name="year">年份</param>
/// <returns>是否為閏年</returns>
private bool IsLeapYear(int year)
{
    return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
}
```

### 7.5 關聯控制項變更處理

```csharp
/// <summary>
/// 處理關聯月份變更事件
/// </summary>
/// <param name="sender">事件來源</param>
/// <param name="e">事件參數</param>
protected void OnMonthChanged(object sender, EventArgs e)
{
    try
    {
        if (AutoAdjustToMonth)
        {
            // 獲取當前月份和年份
            int month = GetCurrentMonth();
            int year = GetCurrentYear();
            
            // 重新填充日期選項
            PopulateDayOptions();
            
            // 檢查當前選擇的日期是否需要調整
            int adjustedDay = AdjustDayToMonth(DayValue, month, year);
            
            // 如果需要調整，更新日期值
            if (adjustedDay != DayValue)
            {
                DayValue = adjustedDay;
                DayString = DayValue.ToString("00");
                
                // 更新控制項顯示
                if (DisplayMode == DayDisplayMode.DropDown)
                {
                    ddlDay.SelectedValue = DayString;
                }
                else
                {
                    txtDay.Text = DayString;
                }
                
                // 觸發日期變更事件
                OnDaySelected();
            }
        }
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDay.OnMonthChanged Error", ex);
    }
}
```

## 8. 測試案例

### 8.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 日值範圍 | 測試ValidateDay方法 | 正確驗證日值範圍 | 測試不同範圍的日值 |
| UT-002 | 月份天數 | 測試GetMonthMaxDay方法 | 返回正確的月份天數 | 測試所有月份和閏年 |
| UT-003 | 日期調整 | 測試AdjustDayToMonth方法 | 正確調整日期 | 測試不同月份的日期調整 |
| UT-004 | 閏年計算 | 測試IsLeapYear方法 | 正確判斷閏年 | 測試各種年份 |
| UT-005 | 營業日檢查 | 測試CheckBusinessDay方法 | 正確識別營業日 | 測試營業日和非營業日 |

### 8.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 與月份控制項整合 | 測試月份變更對日的影響 | 日期正確隨月份調整 | 測試31天月份到30天月份變更 |
| IT-002 | 與年份控制項整合 | 測試年份變更對日的影響 | 二月份在閏年正確處理 | 測試閏年和平年切換 |
| IT-003 | 表單整合 | 測試表單頁面整合 | 正確初始化和提交 | 實際表單頁面 |
| IT-004 | 多控制項協作 | 測試多個日控制項 | 各控制項獨立運作 | 包含多個日控制項 |
| IT-005 | 跨瀏覽器相容性 | 測試跨瀏覽器行為 | 在主流瀏覽器正常 | IE11, Chrome, Firefox |

### 8.3 UI測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UI-001 | 下拉選單顯示 | 測試DropDown模式 | 正確顯示日期選項 | DisplayMode=DropDown |
| UI-002 | 輸入框顯示 | 測試TextBox模式 | 正確接受輸入 | DisplayMode=TextBox |
| UI-003 | 營業日標記 | 測試營業日顯示 | 營業日正確標記 | BusinessDayOnly=true |
| UI-004 | 錯誤提示 | 測試錯誤提示顯示 | 錯誤提示清晰可見 | 輸入無效日期 |
| UI-005 | 樣式應用 | 測試CSS樣式應用 | 控制項樣式正確 | 檢查各種狀態樣式 |

## 9. 安全性考量

### 9.1 輸入驗證

1. **格式驗證**
   - 確保輸入為有效數字
   - 範圍限制於1-31
   - 防止任何非法輸入

2. **範圍檢查**
   - 根據月份自動調整最大值
   - 防止超出範圍的值
   - 業務規則驗證

3. **跨站腳本防護**
   - 對所有輸出進行HTML編碼
   - 避免執行來自用戶的腳本
   - 使用 ASP.NET 內建的防XSS功能

### 9.2 資料安全

1. **資料驗證**
   - 同時在客戶端和伺服器端驗證
   - 不依賴純客戶端驗證
   - 資料傳輸前再次檢查

2. **例外處理**
   - 所有方法包含適當的例外處理
   - 不向用戶顯示底層錯誤
   - 維護錯誤日誌但不暴露系統細節

## 10. 效能優化

### 10.1 載入優化

1. **資源重用**
   - 最小化腳本和樣式資源
   - 控制項輕量化設計
   - 按需載入關聯功能

2. **代碼優化**
   - 減少DOM操作次數
   - 使用延遲初始化技術
   - 避免不必要的資料庫查詢

3. **緩存策略**
   - 緩存月份天數計算
   - 緩存營業日資訊
   - 避免重複查詢固定資料

### 10.2 渲染優化

1. **選擇性渲染**
   - 根據DisplayMode選擇渲染方式
   - 根據需要動態調整選項
   - 最小化初始DOM元素數量

2. **事件處理**
   - 使用事件委託減少事件監聽器
   - 延遲處理非關鍵事件
   - 優化事件觸發頻率

### 10.3 使用者體驗優化

1. **智能預設**
   - 根據當前日期設定默認值
   - 自動調整到有效範圍
   - 記住用戶最近選擇

2. **錯誤處理**
   - 即時錯誤反饋
   - 清晰的錯誤訊息
   - 自動修正常見錯誤

## 11. 維護與擴展

### 11.1 配置管理

1. **控制項參數**
   - 通過屬性配置行為
   - 支持不同顯示模式
   - 可根據需要自定義驗證

2. **樣式設定**
   - 通過CSS自定義外觀
   - 支持主題整合
   - 控制項大小可配置

### 11.2 擴展考量

1. **功能擴展**
   - 預留事件接口
   - 支持派生自定義控制項
   - 可實現特殊業務邏輯擴展

2. **整合擴展**
   - 與其他日期控制項協同
   - 提供API供其他控制項調用
   - 可整合到複合日期選擇器

### 11.3 版本升級

1. **向後兼容**
   - 保持API一致性
   - 保留關鍵屬性及方法
   - 提供升級指南

2. **功能更新**
   - 考慮新增營業日曆整合
   - 提升與現代控制項的相容性
   - 改進自動調整邏輯

## 12. 參考資料

### 12.1 設計文件

1. **UI設計規範**
   - 泛太總帳系統UI設計指南
   - 表單輸入控制項設計標準
   - 日期選擇控制項設計規範

2. **系統架構**
   - 系統整體架構文檔
   - ASP.NET控制項框架說明
   - 日期處理類庫參考

### 12.2 參考實現

1. **框架參考**
   - ASP.NET DropDownList控制項
   - 自定義Web控制項開發指南
   - .NET日期處理元件

2. **UI/UX參考**
   - 常用ERP系統日期選擇元件
   - 金融系統日期輸入控制項
   - 營業日曆顯示模式

### 12.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 