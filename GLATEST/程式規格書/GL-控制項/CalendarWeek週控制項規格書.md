# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | CalendarWeek |
| 程式名稱 | 週控制項 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Controls/CalendarWeek.ascx, /GLATEST/app/Controls/CalendarWeek.ascx.cs |
| 程式類型 | 使用者控制項 (User Control) |
| 建立日期 | 2025/05/07 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/07 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

CalendarWeek 是泛太總帳系統的週選擇控制項，專門用於選擇特定年度中的週次。此控制項主要適用於需要以週為單位進行查詢、統計或設定的情境，例如週報表條件設定、週期性排程任務、週管理等。相較於完整的日期選擇器或月份選擇器，此控制項聚焦於以週為單位的業務場景。

### 2.2 業務流程

CalendarWeek 在系統中扮演以下角色：
1. 提供簡潔的週次選擇介面
2. 支援ISO 8601週次標準與企業週次定義的對應
3. 驗證週次範圍合理性（通常為1-53）
4. 與其他日期元素（年、月、日）協同工作
5. 提供週次數字和文字展示格式（如「第1週」、「W01」等）
6. 支援週次和日期範圍的互相轉換

### 2.3 使用頻率

- 中頻率：用於週報表參數設定和週期性任務配置
- 與年份控制項搭配使用，構成基於週的日期選擇功能
- 主要用於管理層和業務分析報表，以週為單位檢視業務數據

### 2.4 使用者角色

此控制項服務於系統的特定角色，包括：
- 系統管理員：設定系統週期性任務
- 財務經理：檢視週度財務報表
- 業務分析師：分析週度業務資料
- 管理層：查閱週度業績摘要

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Forms (.NET Framework 4.0)
- 主要技術：
  - HTML/CSS 構建介面
  - JavaScript 實現互動效果
  - C# 後端邏輯處理
- 客戶端驗證：確保選擇的週次範圍有效
- 與其他日曆控制項的整合：可以與 CalendarYear、CalendarMonth 搭配使用

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_PARAMETER | 系統參數表，讀取週次相關設定 | 讀取 |
| SYS_USER_CONFIG | 使用者配置表，讀取用戶偏好設定 | 讀取 |
| GL_CALENDAR | 會計行事曆表，獲取週次定義與工作日對應 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供系統參數查詢 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| SysParamManager | 系統參數管理 | 取得週次相關參數設定 |
| CalendarYear | 年份控制項 | 可選整合，提供年份選擇 |

## 4. 控制項設計

### 4.1 界面結構

CalendarWeek 控制項的界面由以下元素構成：

```
+-------------------------------------+
| [文字標籤] [下拉選單/輸入框] [選項按鈕] |
+-------------------------------------+
```

### 4.2 屬性設計

| 屬性名稱 | 類型 | 存取修飾詞 | 說明 |
|---------|------|----------|------|
| WeekValue | int | public | 控制項的週次值（1-53） |
| WeekString | string | public | 格式化後的週次字串 |
| IsRequired | bool | public | 是否為必填欄位 |
| MinWeek | int | public | 允許的最小週次（預設: 1） |
| MaxWeek | int | public | 允許的最大週次（預設: 53） |
| ShowLabel | bool | public | 是否顯示文字標籤 |
| LabelText | string | public | 標籤文字 |
| LabelWidth | int | public | 標籤寬度 |
| InputWidth | int | public | 輸入框寬度 |
| ValidationGroup | string | public | 驗證群組 |
| DisplayType | WeekDisplayType | public | 顯示類型（數字/文字/組合） |
| LinkedYearControlID | string | public | 關聯年份控制項ID |
| DisplayMode | WeekDisplayMode | public | 顯示模式（下拉選單/輸入框） |
| WeekStartDay | DayOfWeek | public | 週起始日（預設: 星期一） |
| ShowWeekRange | bool | public | 是否顯示週日期範圍 |
| CurrentWeekDefault | bool | public | 是否默認當前週次 |
| WeekSystemType | WeekSystemType | public | 週次系統類型（ISO 8601/自訂） |

### 4.3 參數定義

CalendarWeek 控制項支援以下WebForm參數：

```
<%@ Register Src="~/Controls/CalendarWeek.ascx" TagPrefix="pan" TagName="CalendarWeek" %>
<pan:CalendarWeek ID="txtWeek" runat="server" 
                WeekValue="22" 
                IsRequired="true"
                LabelText="週次:"
                LabelWidth="100"
                InputWidth="80"
                DisplayType="TextAndNumber"
                LinkedYearControlID="txtYear"
                ShowWeekRange="true"
                ValidationGroup="vgDate" />
```

### 4.4 CSS設計

控制項使用以下核心CSS類別：

```css
.calweek-container { /* 容器 */ }
.calweek-label { /* 文字標籤 */ }
.calweek-input { /* 週次輸入框 */ }
.calweek-dropdown { /* 下拉選單 */ }
.calweek-button { /* 選項按鈕 */ }
.calweek-range { /* 週日期範圍 */ }
.calweek-error { /* 錯誤提示 */ }
.calweek-required { /* 必填標記 */ }
.calweek-disabled { /* 禁用狀態 */ }
.calweek-current { /* 當前週次樣式 */ }
.calweek-month-start { /* 月初週樣式 */ }
.calweek-month-end { /* 月末週樣式 */ }
.calweek-quarter-start { /* 季初週樣式 */ }
.calweek-quarter-end { /* 季末週樣式 */ }
```

## 5. 主要方法

### 5.1 初始化和載入方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入時初始化控制項 | object sender, EventArgs e | void |
| InitializeCalendarWeek | 初始化週控制項 | 無 | void |
| LoadParameters | 載入系統參數設定 | 無 | void |
| PopulateWeekOptions | 填充週次選項 | 無 | void |
| SetupValidation | 設置驗證控制項 | 無 | void |
| RegisterClientScript | 註冊客戶端腳本 | 無 | void |

### 5.2 事件處理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| WeekChanged | 處理週次變更事件 | object sender, EventArgs e | void |
| OnYearChanged | 處理關聯年份變更事件 | object sender, EventArgs e | void |
| OnWeekSelected | 觸發週次選擇事件 | 無 | void |
| ValidateWeek | 驗證週次範圍 | 無 | bool |
| UpdateWeekRangeDisplay | 更新週日期範圍顯示 | 無 | void |

### 5.3 工具方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| GetWeekDisplayText | 根據顯示類型獲取週次文字 | int week, WeekDisplayType displayType | string |
| GetFirstDayOfWeek | 獲取指定年週的第一天 | int year, int week, DayOfWeek startDay | DateTime |
| GetLastDayOfWeek | 獲取指定年週的最後一天 | int year, int week, DayOfWeek startDay | DateTime |
| GetWeekOfYear | 獲取指定日期的週次 | DateTime date, WeekSystemType weekSystem | int |
| GetTotalWeeksInYear | 獲取指定年份的總週數 | int year, WeekSystemType weekSystem | int |
| IsMonthStartWeek | 檢查是否為月初週 | int year, int week | bool |
| IsMonthEndWeek | 檢查是否為月末週 | int year, int week | bool |
| GetWeekRangeText | 獲取週日期範圍文字 | int year, int week | string |
| GetRelatedControlValue | 獲取關聯控制項的值 | string controlID, string defaultValue | string |
| FindControl | 查找指定ID的控制項 | string controlID | Control |

### 5.4 渲染方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| RenderControl | 渲染控制項 | HtmlTextWriter writer | void |
| RenderLabel | 渲染標籤 | HtmlTextWriter writer | void |
| RenderInput | 渲染輸入框 | HtmlTextWriter writer | void |
| RenderDropDown | 渲染下拉選單 | HtmlTextWriter writer | void |
| RenderWeekRange | 渲染週日期範圍 | HtmlTextWriter writer | void |
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
根據週次系統確定週起始日和最大週次
 ↓
檢查關聯控制項 → 有關聯控制項 → 註冊關聯事件
 ↓
設置最小最大週次範圍
 ↓
填充週次選項
 ↓
設置驗證控制項
 ↓
如果啟用顯示週範圍 → 計算並顯示日期範圍
 ↓
註冊客戶端腳本
 ↓
應用樣式設定
 ↓
結束
```

### 6.2 週次選擇流程

```
開始
 ↓
使用者選擇週次
 ↓
檢查週次有效性 → 無效 → 顯示錯誤訊息 → 結束
 ↓
更新WeekValue值
 ↓
根據所選年份檢查週次是否在有效範圍內 → 不在 → 調整到最近有效週次
 ↓
根據DisplayType格式化週次顯示
 ↓
如果啟用顯示週範圍 → 更新週日期範圍顯示
 ↓
觸發WeekChanged事件
 ↓
結束
```

### 6.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 週次範圍無效 | 顯示範圍提示，自動調整到有效範圍 | 提示有效範圍，紅色強調輸入框 |
| 跨年週次 | 根據週次系統處理跨年週 | 提供明確的跨年週顯示 |
| 週次為空但必填 | 顯示必填提示，阻止表單提交 | 提示必須選擇週次，紅色強調輸入框 |
| 關聯年份無效 | 提示年份限制 | 顯示年份相關限制提示 |
| 客戶端JavaScript錯誤 | 降級為純文字輸入 | 退回到基本輸入功能，保持可用性 |

## 7. 代碼說明

### 7.1 控制項註冊與頁面使用

```aspx
<%-- 在頁面中註冊與使用CalendarWeek控制項 --%>
<%@ Register Src="~/Controls/CalendarWeek.ascx" TagPrefix="pan" TagName="CalendarWeek" %>
<%@ Register Src="~/Controls/CalendarYear.ascx" TagPrefix="pan" TagName="CalendarYear" %>

<div class="form-group">
    <pan:CalendarYear ID="txtYear" runat="server" 
                    YearValue='<%# DateTime.Now.Year %>' 
                    IsRequired="true"
                    LabelText="年度:"
                    ValidationGroup="vgDateParts" />
                    
    <pan:CalendarWeek ID="txtWeek" runat="server" 
                    WeekValue='<%# GetCurrentWeekOfYear() %>' 
                    IsRequired="true"
                    LabelText="週次:"
                    DisplayType="TextAndNumber"
                    LinkedYearControlID="txtYear"
                    ShowWeekRange="true"
                    ValidationGroup="vgDateParts" />
</div>
```

### 7.2 初始化實現

```csharp
/// <summary>
/// 初始化週控制項
/// </summary>
private void InitializeCalendarWeek()
{
    try
    {
        // 載入系統參數設定
        LoadParameters();
        
        // 根據週次系統確定週起始日和最大週次
        SetupWeekSystem();
        
        // 如果未設定週次值，使用當前週次
        if (WeekValue <= 0)
        {
            if (CurrentWeekDefault)
            {
                // 獲取當前日期週次
                DateTime today = DateTime.Today;
                WeekValue = GetWeekOfYear(today, WeekSystemType);
            }
            else
            {
                WeekValue = 1;
            }
        }
        
        // 設置最小與最大週次範圍
        if (MinWeek <= 0)
        {
            MinWeek = 1;
        }
        
        int maxWeeksInYear = GetTotalWeeksInYear(GetCurrentYear(), WeekSystemType);
        if (MaxWeek <= 0 || MaxWeek > maxWeeksInYear)
        {
            MaxWeek = maxWeeksInYear;
        }
        
        // 更新週次字串
        WeekString = GetWeekDisplayText(WeekValue, DisplayType);
        
        // 尋找關聯控制項並註冊事件
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
        if (DisplayMode == WeekDisplayMode.DropDown)
        {
            ddlWeek.ID = ID + "_DropDown";
            ddlWeek.Visible = true;
            txtWeek.Visible = false;
            
            // 填充下拉選單選項
            PopulateWeekOptions();
            
            // 設置選中值
            SetSelectedWeek();
        }
        else
        {
            txtWeek.ID = ID + "_Input";
            txtWeek.Text = WeekString;
            txtWeek.Visible = true;
            ddlWeek.Visible = false;
        }
        
        // 設置標籤
        if (ShowLabel)
        {
            lblWeek.Text = LabelText;
            lblWeek.Width = Unit.Pixel(LabelWidth);
            
            if (IsRequired)
            {
                lblWeek.Text += "<span class='calweek-required'>*</span>";
            }
        }
        else
        {
            lblWeek.Visible = false;
        }
        
        // 設置輸入框寬度
        if (InputWidth > 0)
        {
            if (DisplayMode == WeekDisplayMode.DropDown)
            {
                ddlWeek.Width = Unit.Pixel(InputWidth);
            }
            else
            {
                txtWeek.Width = Unit.Pixel(InputWidth);
            }
        }
        
        // 如果啟用顯示週範圍
        if (ShowWeekRange)
        {
            UpdateWeekRangeDisplay();
        }
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.InitializeCalendarWeek Error", ex);
    }
}
```

### 7.3 週次驗證實現

```csharp
/// <summary>
/// 驗證週次範圍
/// </summary>
/// <returns>是否驗證通過</returns>
private bool ValidateWeek()
{
    try
    {
        string weekText = (DisplayMode == WeekDisplayMode.DropDown) ? 
                        ddlWeek.SelectedValue : txtWeek.Text;
        
        // 檢查必填
        if (IsRequired && string.IsNullOrEmpty(weekText))
        {
            SetErrorMessage("週次欄位必須填寫");
            return false;
        }
        
        // 如果為空且非必填，視為有效
        if (!IsRequired && string.IsNullOrEmpty(weekText))
        {
            return true;
        }
        
        // 下拉選單模式直接取值
        if (DisplayMode == WeekDisplayMode.DropDown)
        {
            int parsedWeek;
            if (int.TryParse(ddlWeek.SelectedValue, out parsedWeek))
            {
                WeekValue = parsedWeek;
                WeekString = GetWeekDisplayText(WeekValue, DisplayType);
                return true;
            }
            
            SetErrorMessage("週次格式無效");
            return false;
        }
        
        // 輸入框模式需要解析
        int week = 0;
        
        // 根據顯示類型不同，解析方式不同
        switch (DisplayType)
        {
            case WeekDisplayType.Number:
                if (!int.TryParse(weekText, out week))
                {
                    SetErrorMessage("週次必須為數字");
                    return false;
                }
                break;
                
            case WeekDisplayType.Text:
                week = ParseWeekText(weekText);
                if (week == 0)
                {
                    SetErrorMessage("週次格式無效，請輸入正確的週次(第n週)");
                    return false;
                }
                break;
                
            case WeekDisplayType.TextAndNumber:
                week = ParseWeekTextAndNumber(weekText);
                if (week == 0)
                {
                    SetErrorMessage("週次格式無效，請輸入正確的週次(第n週或Wnn)");
                    return false;
                }
                break;
        }
        
        // 檢查範圍
        int year = GetCurrentYear();
        int maxWeeksInYear = GetTotalWeeksInYear(year, WeekSystemType);
        
        if (week < MinWeek)
        {
            SetErrorMessage($"週次不能小於 {GetWeekDisplayText(MinWeek, DisplayType)}");
            return false;
        }
        
        if (week > maxWeeksInYear)
        {
            SetErrorMessage($"週次不能大於 {GetWeekDisplayText(maxWeeksInYear, DisplayType)}");
            return false;
        }
        
        // 更新週次值
        WeekValue = week;
        WeekString = GetWeekDisplayText(WeekValue, DisplayType);
        
        // 更新週範圍顯示
        if (ShowWeekRange)
        {
            UpdateWeekRangeDisplay();
        }
        
        return true;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.ValidateWeek Error", ex);
        SetErrorMessage("週次驗證過程發生錯誤");
        return false;
    }
}
```

### 7.4 週次計算與日期範圍實現

```csharp
/// <summary>
/// 獲取指定年週的第一天
/// </summary>
/// <param name="year">年份</param>
/// <param name="week">週次</param>
/// <param name="startDay">週起始日</param>
/// <returns>週第一天日期</returns>
private DateTime GetFirstDayOfWeek(int year, int week, DayOfWeek startDay)
{
    try
    {
        DateTime jan1 = new DateTime(year, 1, 1);
        int daysOffset = (int)startDay - (int)jan1.DayOfWeek;
        
        if (daysOffset > 0)
        {
            daysOffset -= 7;
        }
        
        DateTime firstDayOfFirstWeek = jan1.AddDays(daysOffset);
        
        // 根據ISO 8601週規則處理第1週
        if (WeekSystemType == WeekSystemType.ISO8601)
        {
            // ISO 8601規則：第1週是包含該年第一個星期四的週
            DateTime jan4 = new DateTime(year, 1, 4);
            int daysDiff = (int)startDay - (int)jan4.DayOfWeek;
            if (daysDiff > 0) 
            {
                daysDiff -= 7;
            }
            firstDayOfFirstWeek = jan4.AddDays(daysDiff);
            
            // 如果第1週實際上屬於前一年的最後一週
            if (week == 1 && jan1.DayOfWeek != startDay && jan1.DayOfWeek != startDay.AddDays(1))
            {
                return firstDayOfFirstWeek;
            }
        }
        
        // 計算目標週的第一天
        DateTime result = firstDayOfFirstWeek.AddDays((week - 1) * 7);
        return result;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.GetFirstDayOfWeek Error", ex);
        return DateTime.Today;
    }
}

/// <summary>
/// 獲取指定年週的最後一天
/// </summary>
/// <param name="year">年份</param>
/// <param name="week">週次</param>
/// <param name="startDay">週起始日</param>
/// <returns>週最後一天日期</returns>
private DateTime GetLastDayOfWeek(int year, int week, DayOfWeek startDay)
{
    try
    {
        DateTime firstDay = GetFirstDayOfWeek(year, week, startDay);
        return firstDay.AddDays(6);
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.GetLastDayOfWeek Error", ex);
        return DateTime.Today;
    }
}

/// <summary>
/// 更新週日期範圍顯示
/// </summary>
private void UpdateWeekRangeDisplay()
{
    try
    {
        if (ShowWeekRange && WeekValue > 0)
        {
            int year = GetCurrentYear();
            DateTime startDate = GetFirstDayOfWeek(year, WeekValue, WeekStartDay);
            DateTime endDate = GetLastDayOfWeek(year, WeekValue, WeekStartDay);
            
            string rangeText = $"{startDate.ToString("yyyy/MM/dd")} - {endDate.ToString("yyyy/MM/dd")}";
            lblWeekRange.Text = rangeText;
            lblWeekRange.Visible = true;
        }
        else
        {
            lblWeekRange.Visible = false;
        }
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.UpdateWeekRangeDisplay Error", ex);
        lblWeekRange.Visible = false;
    }
}
```

### 7.5 關聯控制項變更處理

```csharp
/// <summary>
/// 處理關聯年份變更事件
/// </summary>
/// <param name="sender">事件來源</param>
/// <param name="e">事件參數</param>
protected void OnYearChanged(object sender, EventArgs e)
{
    try
    {
        // 獲取當前年份
        int year = GetCurrentYear();
        
        // 計算新年度的最大週數
        int maxWeeksInYear = GetTotalWeeksInYear(year, WeekSystemType);
        
        // 更新最大週數
        MaxWeek = maxWeeksInYear;
        
        // 檢查當前選擇的週次是否需要調整
        if (WeekValue > maxWeeksInYear)
        {
            WeekValue = maxWeeksInYear;
            WeekString = GetWeekDisplayText(WeekValue, DisplayType);
            
            // 更新控制項顯示
            if (DisplayMode == WeekDisplayMode.DropDown)
            {
                // 重新填充週次選項
                PopulateWeekOptions();
                SetSelectedWeek();
            }
            else
            {
                txtWeek.Text = WeekString;
            }
            
            // 更新週範圍顯示
            if (ShowWeekRange)
            {
                UpdateWeekRangeDisplay();
            }
            
            // 觸發週次變更事件
            OnWeekSelected();
        }
        else
        {
            // 年份變更但週次未變更時，仍需更新週範圍顯示
            if (ShowWeekRange)
            {
                UpdateWeekRangeDisplay();
            }
        }
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarWeek.OnYearChanged Error", ex);
    }
}
``` 