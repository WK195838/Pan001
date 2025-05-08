# CalendarMonth.ascx.cs 月份控制項程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CalendarMonth.ascx.cs                 |
| 程式名稱     | 月份控制項程式碼                          |
| 檔案大小     | 8.4KB                                 |
| 行數        | ~280                                  |
| 功能簡述     | 月份選擇後端邏輯                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/7                              |

## 程式功能概述

CalendarMonth.ascx.cs 是月份控制項的後端程式碼，負責處理月份資料的載入、選擇和事件處理。此程式碼實現了月份選擇的核心邏輯，提供多種顯示格式和範圍限制功能。主要功能包括：

1. 提供月份資料的載入和綁定功能
2. 實現不同月份顯示格式（數字或中文）
3. 提供月份範圍限制功能
4. 處理月份選擇變更時的事件
5. 支援預設月份的設定與驗證
6. 實現月份控制項的屬性和方法
7. 提供月份選擇的驗證和錯誤處理
8. 支援月份的格式轉換和顯示邏輯

## 程式結構說明

CalendarMonth.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 CalendarMonth 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **月份資料載入方法**：處理月份資料的載入和綁定
6. **月份選擇方法**：實現月份選擇和相關操作
7. **月份格式轉換方法**：處理不同格式的月份顯示
8. **公用方法**：提供對外部可用的方法和介面

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
```

### 類別宣告與成員變數

```csharp
public partial class CalendarMonth : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler MonthChanged;
    
    // 月份顯示格式枚舉
    public enum MonthDisplayFormat
    {
        Numeric,           // 數字格式：01, 02, ..., 12
        ChineseCharacter   // 中文格式：一月, 二月, ..., 十二月
    }
    
    // 內部變數
    private int _selectedMonth = DateTime.Now.Month;
    private MonthDisplayFormat _displayFormat = MonthDisplayFormat.Numeric;
    private int _minMonth = 1;
    private int _maxMonth = 12;
    private bool _required = true;
    private bool _showLabel = true;
    private string _labelText = "月份";
    private int _labelWidth = 60;
    private int _dropDownWidth = 80;
}
```

### 屬性定義

```csharp
// 選取的月份
public int SelectedMonth
{
    get { return _selectedMonth; }
    set 
    { 
        // 確保月份在有效範圍內
        if (value < _minMonth) value = _minMonth;
        if (value > _maxMonth) value = _maxMonth;
        
        _selectedMonth = value;
        SelectMonth(value);
    }
}

// 月份顯示格式
public MonthDisplayFormat DisplayFormat
{
    get { return _displayFormat; }
    set 
    { 
        _displayFormat = value;
        if (!IsPostBack)
        {
            BindMonths(_displayFormat, _minMonth, _maxMonth);
        }
    }
}

// 最小月份
public int MinMonth
{
    get { return _minMonth; }
    set 
    { 
        // 確保最小月份在 1-12 範圍內
        if (value < 1) value = 1;
        if (value > 12) value = 12;
        
        _minMonth = value;
        
        // 如果目前選取的月份小於最小月份，則更新選取的月份
        if (_selectedMonth < _minMonth)
        {
            _selectedMonth = _minMonth;
        }
        
        if (!IsPostBack)
        {
            BindMonths(_displayFormat, _minMonth, _maxMonth);
        }
    }
}

// 最大月份
public int MaxMonth
{
    get { return _maxMonth; }
    set 
    { 
        // 確保最大月份在 1-12 範圍內
        if (value < 1) value = 1;
        if (value > 12) value = 12;
        
        _maxMonth = value;
        
        // 如果目前選取的月份大於最大月份，則更新選取的月份
        if (_selectedMonth > _maxMonth)
        {
            _selectedMonth = _maxMonth;
        }
        
        if (!IsPostBack)
        {
            BindMonths(_displayFormat, _minMonth, _maxMonth);
        }
    }
}

// 是否必填
public bool Required
{
    get { return _required; }
    set { _required = value; }
}

// 是否顯示標籤
public bool ShowLabel
{
    get { return _showLabel; }
    set 
    { 
        _showLabel = value;
        lblMonth.Visible = value;
    }
}

// 標籤文字
public string LabelText
{
    get { return _labelText; }
    set 
    { 
        _labelText = value;
        lblMonth.Text = value;
    }
}

// 標籤寬度
public int LabelWidth
{
    get { return _labelWidth; }
    set 
    { 
        _labelWidth = value;
        lblMonth.Width = value;
    }
}

// 下拉選單寬度
public int DropDownWidth
{
    get { return _dropDownWidth; }
    set 
    { 
        _dropDownWidth = value;
        ddlMonth.Width = value;
    }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return ddlMonth.Enabled; }
    set { ddlMonth.Enabled = value; }
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
    // 設定標籤文字和寬度
    lblMonth.Text = _labelText;
    lblMonth.Width = _labelWidth;
    lblMonth.Visible = _showLabel;
    
    // 設定下拉選單寬度
    ddlMonth.Width = _dropDownWidth;
    
    // 綁定月份資料
    BindMonths(_displayFormat, _minMonth, _maxMonth);
    
    // 選擇預設月份
    SelectMonth(_selectedMonth);
}
```

#### 月份資料綁定方法

```csharp
// 綁定月份資料
public void BindMonths(MonthDisplayFormat format = MonthDisplayFormat.Numeric, 
                      int minMonth = 1, int maxMonth = 12)
{
    // 清除現有項目
    ddlMonth.Items.Clear();
    
    // 加入空白選項（如果不是必填項）
    if (!_required)
    {
        ddlMonth.Items.Add(new ListItem("", ""));
    }
    
    try
    {
        // 確保月份範圍有效
        if (minMonth < 1) minMonth = 1;
        if (maxMonth > 12) maxMonth = 12;
        if (minMonth > maxMonth) minMonth = maxMonth;
        
        // 綁定月份資料到下拉選單
        for (int month = minMonth; month <= maxMonth; month++)
        {
            string monthText = GetMonthDisplay(month, format);
            string monthValue = month.ToString();
            
            ListItem item = new ListItem(monthText, monthValue);
            ddlMonth.Items.Add(item);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("BindMonths Error: " + ex.Message);
    }
}

// 根據顯示格式取得月份顯示文字
public string GetMonthDisplay(int month, MonthDisplayFormat format)
{
    // 確保月份在有效範圍內
    if (month < 1 || month > 12)
        return string.Empty;
    
    switch (format)
    {
        case MonthDisplayFormat.Numeric:
            // 數字格式：01, 02, ..., 12
            return month.ToString("00");
            
        case MonthDisplayFormat.ChineseCharacter:
            // 中文格式：一月, 二月, ..., 十二月
            string[] chineseMonths = new string[] 
            { 
                "一月", "二月", "三月", "四月", "五月", "六月", 
                "七月", "八月", "九月", "十月", "十一月", "十二月" 
            };
            return chineseMonths[month - 1];
            
        default:
            return month.ToString();
    }
}
```

#### 月份選擇方法

```csharp
// 根據月份值選取月份
public void SelectMonth(int month)
{
    try
    {
        // 確保月份在有效範圍內
        if (month < _minMonth) month = _minMonth;
        if (month > _maxMonth) month = _maxMonth;
        
        // 選取對應的下拉選單項目
        ListItem item = ddlMonth.Items.FindByValue(month.ToString());
        if (item != null)
        {
            ddlMonth.SelectedValue = month.ToString();
            _selectedMonth = month;
        }
        else
        {
            // 如果找不到指定的月份，則選取第一個月份（如果有）
            if (ddlMonth.Items.Count > 0)
            {
                ddlMonth.SelectedIndex = 0;
                _selectedMonth = int.Parse(ddlMonth.SelectedValue);
            }
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("SelectMonth Error: " + ex.Message);
    }
}
```

#### 事件處理方法

```csharp
// 處理下拉選單選項變更事件
protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
{
    // 更新選取的月份
    if (!string.IsNullOrEmpty(ddlMonth.SelectedValue))
    {
        _selectedMonth = int.Parse(ddlMonth.SelectedValue);
    }
    
    // 觸發月份變更事件
    OnMonthChanged(EventArgs.Empty);
}

// 觸發月份變更事件
protected virtual void OnMonthChanged(EventArgs e)
{
    if (MonthChanged != null)
        MonthChanged(this, e);
}
```

#### 驗證方法

```csharp
// 驗證控制項
public bool Validate()
{
    // 如果不是必填項，則返回 true
    if (!_required)
        return true;
    
    // 如果是必填項，則檢查是否有選取月份
    if (ddlMonth.SelectedIndex < 0 || string.IsNullOrEmpty(ddlMonth.SelectedValue))
    {
        // 顯示錯誤訊息
        string errorMessage = string.Format("請選擇{0}。", _labelText);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ValidationError_" + ClientID, 
            string.Format("alert('{0}');", errorMessage), true);
        
        return false;
    }
    
    return true;
}

// 重設控制項
public void Reset()
{
    // 重設為當前月份或範圍內的有效月份
    int currentMonth = DateTime.Now.Month;
    if (currentMonth < _minMonth) currentMonth = _minMonth;
    if (currentMonth > _maxMonth) currentMonth = _maxMonth;
    
    SelectMonth(currentMonth);
}
```

## 技術實現

CalendarMonth.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **枚舉型別**：定義月份顯示格式的枚舉型別
5. **資料繫結**：使用資料繫結技術填充下拉式選單

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| SelectedMonth | int | 獲取或設定選取的月份（1-12） |
| DisplayFormat | MonthDisplayFormat | 獲取或設定月份顯示格式 |
| MinMonth | int | 獲取或設定可選擇的最小月份 |
| MaxMonth | int | 獲取或設定可選擇的最大月份 |
| Required | bool | 獲取或設定月份選擇是否為必填 |
| ShowLabel | bool | 獲取或設定是否顯示月份標籤 |
| LabelText | string | 獲取或設定標籤文字 |
| LabelWidth | int | 獲取或設定標籤寬度 |
| DropDownWidth | int | 獲取或設定下拉選單寬度 |
| Enabled | bool | 獲取或設定控制項是否啟用 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindMonths | format, minMonth, maxMonth (可選) | void | 綁定月份資料到下拉式選單 |
| SelectMonth | month | void | 根據月份值選取下拉式選單項目 |
| GetMonthDisplay | month, format | string | 根據顯示格式取得月份顯示文字 |
| Validate | 無 | bool | 驗證控制項是否符合必填要求 |
| Reset | 無 | void | 重設控制項為預設狀態 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| MonthChanged | EventArgs | 月份選擇變更時觸發的事件 |

## 程式碼分析

### 強項

1. **範圍限制的完整性**：
   - 實現了完整的月份範圍限制功能
   - 確保選取的月份值在有效範圍內
   - 自動調整超出範圍的月份值

2. **顯示格式的彈性**：
   - 支援多種月份顯示格式（數字和中文）
   - 提供簡潔的格式轉換機制
   - 良好的擴展性，可以輕鬆添加更多顯示格式

3. **使用者體驗的考量**：
   - 提供彈性的配置選項，如標籤文字、寬度等
   - 支援預設月份的自動載入
   - 處理各種錯誤情況，確保良好的使用者體驗

4. **代碼結構的清晰性**：
   - 功能模組化，分離不同的功能區塊
   - 良好的錯誤處理機制
   - 清晰的命名和適當的註解

### 待改進點

1. **功能擴充**：
   - 可以增加更多的月份顯示格式，如英文月份名稱
   - 實現與年度控制項的整合功能

2. **客戶端功能**：
   - 可以增加更多的客戶端功能，如使用 AJAX 避免頁面重新載入
   - 提供更豐富的客戶端驗證和錯誤處理

3. **使用者介面增強**：
   - 可以提供更多的視覺反饋
   - 支援特定月份的高亮顯示或禁用功能

## 安全性考量

1. **輸入驗證**：
   - 驗證使用者選取的月份值
   - 防止非法的月份值被設定

2. **錯誤處理**：
   - 適當處理和記錄錯誤情況
   - 不向使用者暴露敏感的錯誤訊息

## 效能考量

1. **資料載入優化**：
   - 月份資料是固定的（1-12），無需資料庫查詢
   - 控制項初始化時一次性載入所有月份資料

2. **記憶體使用**：
   - 控制項使用的資源較少，不會造成明顯的記憶體負擔
   - 避免不必要的大型物件建立

3. **回應時間**：
   - 確保月份選擇操作的回應速度
   - 減少不必要的計算和處理

## 待改進事項

1. 增加月份與年度的聯動功能，實現完整的年月選擇
2. 增加季度選擇功能，可自動對應到相應的月份
3. 實現更多的月份顯示格式，如英文月份名稱
4. 增加可以同時顯示月份代碼和名稱的選項
5. 優化控制項在移動設備上的使用體驗
6. 增加月份範圍的動態變更功能，以應對特定業務需求
7. 提供更多的客戶端功能，如即時驗證和自動完成
8. 增加多語言支援，以適應國際化需求

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/7  | 1.0    | 首次建立月份控制項程式碼規格書          | Claude AI  | 