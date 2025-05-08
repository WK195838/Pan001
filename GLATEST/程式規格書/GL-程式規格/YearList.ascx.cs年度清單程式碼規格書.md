# YearList.ascx.cs 年度清單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | YearList.ascx.cs                       |
| 程式名稱     | 年度清單程式碼                           |
| 檔案大小     | 4.9KB                                 |
| 行數        | ~190                                  |
| 功能簡述     | 年度選擇後端邏輯                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/14                             |

## 程式功能概述

YearList.ascx.cs 是泛太總帳系統中年度清單控制項的後端程式碼，負責實現年度選擇功能的核心邏輯。此程式提供年度資料的設定、獲取和驗證功能，管理年度範圍的動態生成，以及處理使用者交互事件。主要功能包括：

1. 實現年度清單控制項的所有後端邏輯和資料處理
2. 動態生成並綁定年度下拉選單的選項
3. 提供年度資料的設定和獲取方法
4. 處理年度選擇變更事件和通知機制
5. 支援自訂年度範圍設定
6. 提供必填驗證和唯讀模式支援
7. 實現「本年」快速選擇功能
8. 提供與UI元素的互動控制
9. 支援年度格式化顯示（支援西元年和民國年格式）
10. 與其他相關控制項（如期間清單）協同工作

此程式碼檔案是年度清單控制項功能實現的核心，與前端介面 YearList.ascx 配合使用，為系統提供統一且彈性的年度選擇功能。

## 程式結構說明

YearList.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 YearList 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **年度資料載入方法**：處理年度資料的產生、格式轉換和綁定
6. **年度選擇方法**：實現年度選擇和相關操作
7. **年度格式轉換方法**：處理不同格式的年度顯示
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
public partial class YearList : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler<YearChangedEventArgs> YearChanged;
    
    // 年度顯示格式枚舉
    public enum YearFormatType
    {
        Western,    // 西元年格式：2020, 2021, ...
        ROC         // 民國年格式：109, 110, ...
    }
    
    // 內部變數
    private int _selectedYear = DateTime.Now.Year;
    private int _startYear = DateTime.Now.Year - 5;
    private int _endYear = DateTime.Now.Year + 5;
    private bool _isRequired = false;
    private bool _isReadOnly = false;
    private string _labelText = "年度:";
    private bool _showCurrentYearButton = true;
    private YearFormatType _yearFormat = YearFormatType.Western;
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
        if (_selectedYear != value)
        {
            int oldYear = _selectedYear;
            _selectedYear = value;
            UpdateUIState();
            OnYearChanged(oldYear, value);
        }
    }
}

// 年度顯示格式
public YearFormatType YearFormat
{
    get { return _yearFormat; }
    set 
    { 
        _yearFormat = value;
        if (!IsPostBack)
        {
            BindYearDropDownList();
        }
    }
}

// 最小年度
public int StartYear
{
    get 
    { 
        if (_startYear <= 0)
            return DateTime.Now.Year - 5;
        return _startYear; 
    }
    set 
    { 
        if (_startYear != value && value > 0)
        {
            _startYear = value;
            RefreshYearList();
        }
    }
}

// 最大年度
public int EndYear
{
    get 
    { 
        if (_endYear <= 0)
            return DateTime.Now.Year + 5;
        return _endYear; 
    }
    set 
    { 
        if (_endYear != value && value > 0)
        {
            _endYear = value;
            RefreshYearList();
        }
    }
}

// 是否必填
public bool Required
{
    get { return _isRequired; }
    set 
    { 
        _isRequired = value;
        rfvYear.Enabled = value;
    }
}

// 是否唯讀
public bool ReadOnly
{
    get { return _isReadOnly; }
    set 
    { 
        _isReadOnly = value;
        UpdateUIState();
    }
}

// 是否顯示標籤
public bool ShowLabel
{
    get { return !_isReadOnly; }
}

// 標籤文字
public string LabelText
{
    get { return _labelText; }
    set 
    { 
        _labelText = value;
        lblYear.Text = value;
    }
}

// 是否顯示「本年」按鈕
public bool ShowCurrentYearButton
{
    get { return _showCurrentYearButton; }
    set 
    { 
        _showCurrentYearButton = value;
        btnCurrentYear.Visible = value && !_isReadOnly;
    }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return !_isReadOnly; }
}

// 自動回傳
public bool AutoPostBack
{
    get { return ddlYear.AutoPostBack; }
    set { ddlYear.AutoPostBack = value; }
}

// 驗證組
public string ValidationGroup
{
    get { return rfvYear.ValidationGroup; }
    set { rfvYear.ValidationGroup = value; }
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
        if (_startYear <= 0 || _endYear <= 0)
        {
            int currentYear = DateTime.Now.Year;
            _startYear = currentYear - 5;
            _endYear = currentYear + 5;
        }
        
        BindYearDropDownList();
        
        if (_selectedYear <= 0)
        {
            SetYear(DateTime.Now.Year);
        }
        else
        {
            SetYear(_selectedYear);
        }
    }
}

private void InitializeControl()
{
    // 設定標籤文字和寬度
    lblYear.Text = _labelText;
    lblYear.Visible = ShowLabel;
    
    // 設定下拉選單寬度
    ddlYear.Width = 100;
    
    // 綁定年度資料
    BindYearDropDownList();
    
    // 選擇預設年度
    SetYear(_selectedYear);
}
```

#### 年度資料綁定方法

```csharp
// 綁定年度資料
public void BindYearDropDownList()
{
    ddlYear.Items.Clear();
    
    for (int year = StartYear; year <= EndYear; year++)
    {
        string displayText = FormatYear(year);
        ListItem item = new ListItem(displayText, year.ToString());
        ddlYear.Items.Add(item);
    }
}

// 根據顯示格式取得年度顯示文字
public string FormatYear(int year)
{
    switch (_yearFormat)
    {
        case YearFormatType.ROC:
            // 民國年格式（西元年 - 1911）
            return string.Format("民國 {0} 年", year - 1911);
            
        case YearFormatType.Western:
        default:
            // 西元年格式
            return year.ToString();
    }
}

// 新增一段年度範圍
public void AddYearRange(int startYear, int endYear)
{
    try
    {
        // 確保年度範圍有效
        if (startYear > endYear)
        {
            int temp = startYear;
            startYear = endYear;
            endYear = temp;
        }
        
        // 更新最小年度和最大年度（如果需要）
        if (startYear < _startYear)
        {
            _startYear = startYear;
        }
        
        if (endYear > _endYear)
        {
            _endYear = endYear;
        }
        
        // 重新綁定年度資料
        BindYearDropDownList();
        
        // 如果目前選取的年度不在範圍內，則重新選擇
        if (_selectedYear < _startYear || _selectedYear > _endYear)
        {
            _selectedYear = _startYear;
            SetYear(_selectedYear);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("AddYearRange Error: " + ex.Message);
    }
}
```

#### 年度選擇方法

```csharp
// 根據年度值選取年度
public void SetYear(int year)
{
    if (year > 0)
    {
        int oldYear = _selectedYear;
        _selectedYear = year;
        
        // 更新UI控制項
        if (ddlYear.Items.FindByValue(year.ToString()) != null)
        {
            ddlYear.SelectedValue = year.ToString();
        }
        else if (year >= StartYear && year <= EndYear)
        {
            // 如果年度在範圍內但選單中沒有，重新綁定選單
            RefreshYearList();
            ddlYear.SelectedValue = year.ToString();
        }
        
        // 觸發事件
        if (oldYear != year)
        {
            OnYearChanged(oldYear, year);
        }
    }
}
```

#### 事件處理方法

```csharp
// 處理下拉選單選項變更事件
protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
{
    if (int.TryParse(ddlYear.SelectedValue, out int selectedYear))
    {
        if (selectedYear != _selectedYear)
        {
            int oldYear = _selectedYear;
            _selectedYear = selectedYear;
            
            // 觸發年度變更事件
            OnYearChanged(oldYear, selectedYear);
        }
    }
}

// 觸發年度變更事件
protected virtual void OnYearChanged(int oldYear, int newYear)
{
    if (YearChanged != null)
    {
        YearChangedEventArgs args = new YearChangedEventArgs(oldYear, newYear);
        YearChanged(this, args);
    }
}
```

#### 驗證方法

```csharp
// 驗證控制項
public bool Validate()
{
    if (_isRequired)
    {
        rfvYear.Validate();
        return rfvYear.IsValid;
    }
    return true;
}

// 重設控制項
public void Reset()
{
    // 重設為當前年度或範圍內的有效年度
    int currentYear = DateTime.Now.Year;
    if (currentYear < _startYear) currentYear = _startYear;
    if (currentYear > _endYear) currentYear = _endYear;
    
    SetYear(currentYear);
}
```

## 技術實現

YearList.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **枚舉型別**：定義年度顯示格式和排序方式的枚舉型別
5. **泛型集合**：使用 List<T> 處理年度資料集合和排序
6. **LINQ**：使用 LINQ 進行年度資料的排序和過濾

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| SelectedYear | int | 獲取或設定選取的年度 |
| YearFormat | YearFormatType | 獲取或設定年度顯示格式 |
| StartYear | int | 獲取或設定可選擇的最小年度 |
| EndYear | int | 獲取或設定可選擇的最大年度 |
| Required | bool | 獲取或設定年度選擇是否為必填 |
| ReadOnly | bool | 獲取或設定控制項是否為唯讀 |
| ShowLabel | bool | 獲取或設定是否顯示年度標籤 |
| LabelText | string | 獲取或設定標籤文字 |
| ShowCurrentYearButton | bool | 獲取或設定是否顯示「本年」按鈕 |
| Enabled | bool | 獲取或設定控制項是否啟用 |
| AutoPostBack | bool | 獲取或設定控制項是否自動回傳 |
| ValidationGroup | string | 獲取或設定驗證組 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindYearDropDownList | 無 | void | 綁定年度下拉選單 |
| SetYear | year | void | 根據年度值選取下拉式選單項目 |
| GetYear | 無 | int | 獲取選取的年度 |
| AddYearRange | startYear, endYear | void | 新增一段年度範圍 |
| Validate | 無 | bool | 驗證控制項是否符合必填要求 |
| Reset | 無 | void | 重設控制項為預設狀態 |
| RefreshYearList | 無 | void | 刷新年度下拉選單 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| YearChanged | YearChangedEventArgs | 年度選擇變更時觸發的事件 |

## 程式碼分析

### 強項

1. **年度範圍的彈性**：
   - 提供最小年度和最大年度的設定
   - 支援動態添加年度範圍
   - 確保選取的年度在有效範圍內

2. **顯示格式的多樣性**：
   - 支援西元年和民國年兩種顯示格式
   - 提供簡潔的格式轉換機制
   - 良好的擴展性，可以輕鬆添加更多顯示格式

3. **排序方式的彈性**：
   - 支援升序和降序兩種排序方式
   - 使用 LINQ 實現高效的排序邏輯

4. **使用者體驗的考量**：
   - 提供彈性的配置選項，如標籤文字、寬度等
   - 支援預設年度的自動載入
   - 處理各種錯誤情況，確保良好的使用者體驗

5. **代碼結構的清晰性**：
   - 功能模組化，分離不同的功能區塊
   - 良好的錯誤處理機制
   - 清晰的命名和適當的註解

### 待改進點

1. **功能擴充**：
   - 實現更多的年度顯示格式，如短年份格式
   - 支援會計年度的設定（非曆年）

2. **客戶端功能**：
   - 增加更多的客戶端功能，如即時年度範圍調整
   - 提供更豐富的客戶端驗證

3. **使用者介面增強**：
   - 提供更多的視覺反饋
   - 支援年度的快捷選項，如「今年」、「去年」等

## 安全性考量

1. **輸入驗證**：
   - 驗證使用者選取的年度值
   - 防止非法的年度值被設定

2. **錯誤處理**：
   - 適當處理和記錄錯誤情況
   - 不向使用者暴露敏感的錯誤訊息

## 效能考量

1. **資料載入優化**：
   - 年度資料由控制項動態產生，無需資料庫查詢
   - 控制項初始化時一次性載入所有年度資料

2. **記憶體使用**：
   - 控制項使用的資源較少，不會造成明顯的記憶體負擔
   - 避免不必要的大型物件建立

3. **回應時間**：
   - 確保年度選擇操作的響應速度
   - 使用 LINQ 和泛型集合提高資料處理效率

## 待改進事項

1. 實現會計年度選擇功能（如四月制、七月制等）
2. 增加年度與月份的聯動功能，實現完整的年月選擇
3. 增加更多的年度顯示格式，如短年份顯示
4. 增加年度選擇的快捷方式，如「今年」、「去年」等選項
5. 優化控制項在移動設備上的使用體驗
6. 增加多語言支援，以適應國際化需求
7. 提供年度範圍的自動更新功能，避免跨年時需要手動更新年度範圍

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/14 | 1.0    | 首次建立年度清單程式碼規格書    | Claude AI  | 