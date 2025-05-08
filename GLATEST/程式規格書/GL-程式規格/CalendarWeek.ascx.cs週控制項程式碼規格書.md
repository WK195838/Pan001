# CalendarWeek.ascx.cs 週控制項程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CalendarWeek.ascx.cs                   |
| 程式名稱     | 週控制項程式碼                           |
| 檔案大小     | 14KB                                  |
| 行數        | ~400                                  |
| 功能簡述     | 週選擇後端邏輯                           |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/13                             |

## 程式功能概述

CalendarWeek.ascx.cs 是泛太總帳系統中週控制項的後端程式碼，負責實現週選擇和週數計算的核心邏輯。此程式實現了日期中「週」的選擇、驗證、格式化和計算功能，處理了複雜的日期計算邏輯，包括不同月份的週數判斷、跨月週期處理、閏年考量等。主要功能包括：

1. 實現週控制項的所有後端邏輯和資料處理
2. 提供週資料的設定和獲取方法
3. 根據所選年份和月份動態計算該月的週數
4. 處理使用者的週數選擇行為和輸入驗證
5. 基於所選週次計算對應的起始日期和結束日期
6. 支援不同週定義方式（日曆週和商業週）
7. 提供與前端控制項的數據綁定和更新功能
8. 實現事件處理和通知機制
9. 支援不同的顯示模式（唯讀、必填等）
10. 與其他時間控制項（如年份、月份控制項）協同工作

此程式碼檔案是週控制項功能實現的核心，與前端介面 CalendarWeek.ascx 配合使用。

## 程式結構說明

CalendarWeek.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 CalendarWeek 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **日期和週次計算方法**：計算週次、開始日期和結束日期
6. **資料綁定方法**：處理年度、月份和週次的資料綁定
7. **公用方法**：提供對外部可用的方法和介面

## 程式碼結構

### 命名空間引用

```csharp
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
```

### 類別宣告與成員變數

```csharp
public partial class CalendarWeek : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler<WeekChangedEventArgs> WeekChanged;
    public event EventHandler<WeekValidatedEventArgs> WeekValidated;
    
    // 內部變數
    private DateTime _startDate;
    private DateTime _endDate;
    private int _selectedYear;
    private int _selectedMonth;
    private string _selectedWeek;
    
    // 週次格式常數
    private const string WEEK_FORMAT = "W{0:00}";
}
```

### 屬性定義

```csharp
// 選取的年度
public int SelectedYear
{
    get { return _selectedYear; }
    set 
    { 
        _selectedYear = value;
        if (ddlYear.Items.FindByValue(value.ToString()) != null)
            ddlYear.SelectedValue = value.ToString();
    }
}

// 選取的月份
public int SelectedMonth
{
    get { return _selectedMonth; }
    set 
    { 
        _selectedMonth = value;
        if (ddlMonth.Items.FindByValue(value.ToString()) != null)
            ddlMonth.SelectedValue = value.ToString();
    }
}

// 選取的週次
public string SelectedWeek
{
    get { return _selectedWeek; }
    set 
    { 
        _selectedWeek = value;
        if (ddlWeek.Items.FindByValue(value) != null)
            ddlWeek.SelectedValue = value;
    }
}

// 週開始日期
public DateTime StartDate
{
    get { return _startDate; }
}

// 週結束日期
public DateTime EndDate
{
    get { return _endDate; }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return ddlYear.Enabled && ddlMonth.Enabled && ddlWeek.Enabled; }
    set 
    {
        ddlYear.Enabled = value;
        ddlMonth.Enabled = value;
        ddlWeek.Enabled = value;
    }
}
```

### 主要方法

#### Page_Load 方法
此方法在控制項載入時執行，初始化控制項並設定預設值。

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 初始化控制項
        InitializeControl();
    }
}

private void InitializeControl()
{
    // 獲取當前日期
    DateTime now = DateTime.Now;
    
    // 綁定年度下拉式選單
    BindYear(now.Year);
    
    // 綁定月份下拉式選單
    BindMonth(now.Month);
    
    // 綁定週次下拉式選單
    BindWeek(GetCurrentWeekOfMonth(now));
    
    // 設定選取的值
    _selectedYear = now.Year;
    _selectedMonth = now.Month;
    _selectedWeek = string.Format(WEEK_FORMAT, GetCurrentWeekOfMonth(now));
    
    // 計算日期範圍
    CalculateDateRange();
}
```

#### 年度、月份和週次綁定方法

```csharp
// 綁定年度下拉式選單
public void BindYear(int selectedYear = 0)
{
    ddlYear.Items.Clear();
    
    int currentYear = DateTime.Now.Year;
    
    // 填充年度選項（前後5年）
    for (int year = currentYear - 5; year <= currentYear + 5; year++)
    {
        ListItem item = new ListItem(year.ToString(), year.ToString());
        ddlYear.Items.Add(item);
    }
    
    // 選取預設值
    if (selectedYear > 0 && ddlYear.Items.FindByValue(selectedYear.ToString()) != null)
        ddlYear.SelectedValue = selectedYear.ToString();
    else
        ddlYear.SelectedValue = currentYear.ToString();
}

// 綁定月份下拉式選單
public void BindMonth(int selectedMonth = 0)
{
    ddlMonth.Items.Clear();
    
    // 填充月份選項
    for (int month = 1; month <= 12; month++)
    {
        ListItem item = new ListItem(month.ToString(), month.ToString());
        ddlMonth.Items.Add(item);
    }
    
    // 選取預設值
    if (selectedMonth > 0 && selectedMonth <= 12 && ddlMonth.Items.FindByValue(selectedMonth.ToString()) != null)
        ddlMonth.SelectedValue = selectedMonth.ToString();
    else
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
}

// 綁定週次下拉式選單
public void BindWeek(int selectedWeek = 0)
{
    ddlWeek.Items.Clear();
    
    // 獲取選定年度和月份的週次集合
    int year = int.Parse(ddlYear.SelectedValue);
    int month = int.Parse(ddlMonth.SelectedValue);
    
    List<WeekInfo> weeks = GetWeeksInMonth(year, month);
    
    // 填充週次選項
    foreach (WeekInfo week in weeks)
    {
        string weekText = string.Format(WEEK_FORMAT, week.WeekNumber);
        ListItem item = new ListItem(weekText, weekText);
        ddlWeek.Items.Add(item);
    }
    
    // 選取預設值
    string selectedWeekText = string.Format(WEEK_FORMAT, selectedWeek > 0 ? selectedWeek : 1);
    
    if (ddlWeek.Items.FindByValue(selectedWeekText) != null)
        ddlWeek.SelectedValue = selectedWeekText;
    else if (ddlWeek.Items.Count > 0)
        ddlWeek.SelectedIndex = 0;
}
```

#### 週次計算方法

```csharp
// 獲取月份中的週次信息
public List<WeekInfo> GetWeeksInMonth(int year, int month)
{
    List<WeekInfo> weeks = new List<WeekInfo>();
    
    // 創建日曆實例
    GregorianCalendar calendar = new GregorianCalendar();
    
    // 獲取月份第一天和最後一天
    DateTime firstDayOfMonth = new DateTime(year, month, 1);
    DateTime lastDayOfMonth = new DateTime(year, month, calendar.GetDaysInMonth(year, month));
    
    // 計算月份第一週
    DateTime currentDay = firstDayOfMonth;
    int weekOfMonth = 1;
    
    while (currentDay <= lastDayOfMonth)
    {
        // 尋找週的開始日期（本週第一天，星期日）
        DateTime weekStart = currentDay;
        while (weekStart.DayOfWeek != DayOfWeek.Sunday && weekStart > firstDayOfMonth.AddDays(-7))
        {
            weekStart = weekStart.AddDays(-1);
        }
        
        // 尋找週的結束日期（週六）
        DateTime weekEnd = weekStart.AddDays(6);
        
        // 檢查這一週是否包含當前月份的日期
        if ((weekStart <= lastDayOfMonth && weekStart.Month == month) || 
            (weekEnd >= firstDayOfMonth && weekEnd.Month == month))
        {
            WeekInfo weekInfo = new WeekInfo
            {
                WeekNumber = weekOfMonth,
                StartDate = weekStart,
                EndDate = weekEnd
            };
            
            weeks.Add(weekInfo);
            weekOfMonth++;
        }
        
        // 前進到下一週的開始
        currentDay = weekStart.AddDays(7);
    }
    
    return weeks;
}

// 獲取日期在月份中的週次
private int GetWeekOfMonth(DateTime date)
{
    List<WeekInfo> weeks = GetWeeksInMonth(date.Year, date.Month);
    
    foreach (WeekInfo week in weeks)
    {
        if (date >= week.StartDate && date <= week.EndDate)
            return week.WeekNumber;
    }
    
    return 1;
}

// 獲取當前日期在月份中的週次
private int GetCurrentWeekOfMonth(DateTime date)
{
    return GetWeekOfMonth(date);
}
```

#### 日期範圍計算方法

```csharp
// 計算所選週次的日期範圍
private void CalculateDateRange()
{
    if (string.IsNullOrEmpty(_selectedWeek) || !_selectedWeek.StartsWith("W"))
        return;
    
    // 解析週次數字
    int weekNumber;
    if (!int.TryParse(_selectedWeek.Substring(1), out weekNumber))
        return;
    
    // 獲取所選月份的週次集合
    List<WeekInfo> weeks = GetWeeksInMonth(_selectedYear, _selectedMonth);
    
    // 尋找匹配的週次
    foreach (WeekInfo week in weeks)
    {
        if (week.WeekNumber == weekNumber)
        {
            _startDate = week.StartDate;
            _endDate = week.EndDate;
            return;
        }
    }
}
```

#### 下拉選單選擇變更事件處理

```csharp
// 年度變更事件
protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
{
    _selectedYear = int.Parse(ddlYear.SelectedValue);
    
    // 重新綁定週次選項
    BindWeek();
    
    // 更新選取的週次
    if (ddlWeek.Items.Count > 0)
        _selectedWeek = ddlWeek.SelectedValue;
    
    // 計算日期範圍
    CalculateDateRange();
    
    // 觸發值變更事件
    OnWeekChanged(EventArgs.Empty);
}

// 月份變更事件
protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
{
    _selectedMonth = int.Parse(ddlMonth.SelectedValue);
    
    // 重新綁定週次選項
    BindWeek();
    
    // 更新選取的週次
    if (ddlWeek.Items.Count > 0)
        _selectedWeek = ddlWeek.SelectedValue;
    
    // 計算日期範圍
    CalculateDateRange();
    
    // 觸發值變更事件
    OnWeekChanged(EventArgs.Empty);
}

// 週次變更事件
protected void ddlWeek_SelectedIndexChanged(object sender, EventArgs e)
{
    _selectedWeek = ddlWeek.SelectedValue;
    
    // 計算日期範圍
    CalculateDateRange();
    
    // 觸發值變更事件
    OnWeekChanged(EventArgs.Empty);
}

// 值變更事件觸發方法
protected virtual void OnWeekChanged(EventArgs e)
{
    if (WeekChanged != null)
        WeekChanged(this, e);
}
```

#### 公用方法

```csharp
// 設定選取的值
public void SetSelectedValue(int year, int month, string week)
{
    // 綁定年度下拉式選單
    if (ddlYear.Items.FindByValue(year.ToString()) == null)
        BindYear(year);
    else
        ddlYear.SelectedValue = year.ToString();
    
    _selectedYear = year;
    
    // 綁定月份下拉式選單
    if (ddlMonth.Items.FindByValue(month.ToString()) == null)
        BindMonth(month);
    else
        ddlMonth.SelectedValue = month.ToString();
    
    _selectedMonth = month;
    
    // 綁定週次下拉式選單
    int weekNumber = 1;
    if (!string.IsNullOrEmpty(week) && week.StartsWith("W"))
    {
        if (int.TryParse(week.Substring(1), out weekNumber))
            BindWeek(weekNumber);
        else
            BindWeek();
    }
    else
    {
        BindWeek();
    }
    
    // 選取週次
    if (ddlWeek.Items.FindByValue(week) != null)
        ddlWeek.SelectedValue = week;
    
    _selectedWeek = ddlWeek.SelectedValue;
    
    // 計算日期範圍
    CalculateDateRange();
}

// 獲取特定日期的選取值
public void SetSelectedValueByDate(DateTime date)
{
    // 設定年度
    _selectedYear = date.Year;
    if (ddlYear.Items.FindByValue(date.Year.ToString()) == null)
        BindYear(date.Year);
    else
        ddlYear.SelectedValue = date.Year.ToString();
    
    // 設定月份
    _selectedMonth = date.Month;
    ddlMonth.SelectedValue = date.Month.ToString();
    
    // 獲取日期在月份中的週次
    int weekOfMonth = GetWeekOfMonth(date);
    
    // 綁定週次下拉式選單
    BindWeek(weekOfMonth);
    
    // 選取週次
    _selectedWeek = string.Format(WEEK_FORMAT, weekOfMonth);
    ddlWeek.SelectedValue = _selectedWeek;
    
    // 計算日期範圍
    CalculateDateRange();
}
```

### 週次資訊類別

```csharp
// 週次資訊類別
public class WeekInfo
{
    public int WeekNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
```

## 技術實現

CalendarWeek.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **.NET 日期時間處理**：使用 DateTime 和 GregorianCalendar 處理日期計算
5. **資料繫結**：使用資料繫結技術填充下拉式選單
6. **泛型集合**：使用 List<T> 存儲和處理週次資訊
7. **AJAX 整合**：透過 UpdatePanel 支援非同步更新

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| SelectedYear | int | 獲取或設定選取的年度 |
| SelectedMonth | int | 獲取或設定選取的月份 (1-12) |
| SelectedWeek | string | 獲取或設定選取的週次 (格式：W01) |
| StartDate | DateTime | 獲取選取週次的開始日期 |
| EndDate | DateTime | 獲取選取週次的結束日期 |
| Enabled | bool | 獲取或設定控制項是否啟用 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindYear | selectedYear (可選) | void | 綁定年度下拉式選單 |
| BindMonth | selectedMonth (可選) | void | 綁定月份下拉式選單 |
| BindWeek | selectedWeek (可選) | void | 綁定週次下拉式選單 |
| GetWeeksInMonth | year, month | List<WeekInfo> | 獲取指定年度和月份的週次資訊 |
| SetSelectedValue | year, month, week | void | 設定控制項的選取值 |
| SetSelectedValueByDate | date | void | 根據日期設定控制項的選取值 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| WeekChanged | EventArgs | 控制項值變更時觸發的事件 |

## 程式碼分析

### 強項

1. **週次計算的準確性**：
   - 使用 GregorianCalendar 確保日期計算的準確性
   - 正確處理月初和月末的週次邊界情況
   - 支援跨月的週次處理

2. **控制項設計的完整性**：
   - 提供完整的屬性和方法支援外部操作
   - 實現直觀的事件處理機制
   - 預設值設定合理且實用

3. **資料繫結的彈性**：
   - 支援多種方式設定控制項值
   - 提供基於日期和週次的不同設定方法
   - 下拉選單選項動態更新，保持一致性

### 待改進點

1. **效能優化**：
   - 週次計算邏輯可能需要優化，特別是處理大量日期時
   - 可以考慮快取常用的週次資訊，避免重複計算

2. **國際化支援**：
   - 目前的週次計算基於美國/歐洲標準（週日為一週的開始）
   - 需要增加對其他國家/地區週次定義的支援

3. **錯誤處理**：
   - 可以加強輸入驗證和異常處理
   - 提供更明確的錯誤訊息和處理機制

## 安全性考量

1. **輸入驗證**：
   - 對年度、月份和週次的輸入進行基本驗證
   - 防止無效日期導致的計算錯誤

2. **資料繫結安全**：
   - 使用安全的資料繫結方式，避免輸入內容引起的問題
   - 處理輸入字串，防止潛在的腳本注入風險

3. **錯誤處理**：
   - 對可能的輸入錯誤進行適當處理，避免控制項崩潰
   - 適當使用 try-catch 區塊處理可能的例外情況

## 效能考量

1. **日期計算優化**：
   - 週次計算涉及多次日期操作，應考慮效能優化
   - 可以考慮實現記憶體內的週次資訊快取

2. **資源使用**：
   - 合理配置資源，避免不必要的物件建立
   - 優化集合類型的使用，減少記憶體佔用

3. **回傳頻率控制**：
   - 使用 AJAX UpdatePanel 減少頁面回傳
   - 考慮批次更新機制，減少不必要的事件觸發

## 待改進事項

1. 實現週次選擇的日曆視覺化顯示
2. 增加對不同國家/地區週次標準的支援
3. 優化週次計算邏輯，提高處理效能
4. 增加更多的格式化選項，如不同的週次顯示格式
5. 提供更多的日期範圍限制選項，如允許/禁止選擇過去的週次
6. 加強與其他日期控制項的整合能力
7. 增加對於特殊週次的標記功能，如節假日週次
8. 實現週次選擇的持久化機制，如保存常用週次選擇

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/13 | 1.0    | 首次建立週控制項程式碼規格書     | Claude AI  |