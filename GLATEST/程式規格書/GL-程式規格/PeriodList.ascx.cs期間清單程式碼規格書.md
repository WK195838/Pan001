# PeriodList.ascx.cs 期間清單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | PeriodList.ascx.cs                    |
| 程式名稱     | 期間清單程式碼                          |
| 檔案大小     | 1.7KB                                 |
| 行數        | ~80                                   |
| 功能簡述     | 會計期間選擇後端邏輯                     |
| 複雜度       | 低                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/9                              |

## 程式功能概述

PeriodList.ascx.cs 是期間清單控制項的後端程式碼，負責處理會計期間資料的載入、選擇和事件處理。此程式碼實現了期間選擇的核心邏輯，提供彈性的期間範圍設定和顯示格式。主要功能包括：

1. 提供期間資料的動態產生和綁定功能
2. 實現數字、文字和混合三種期間顯示格式
3. 提供期間範圍限制和動態調整功能
4. 處理期間選擇變更時的事件
5. 支援預設期間的設定與驗證
6. 實現期間排序（升序或降序）的功能
7. 提供期間選擇的驗證和錯誤處理
8. 支援與年度控制項的整合

## 程式結構說明

PeriodList.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 PeriodList 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **期間資料載入方法**：處理期間資料的產生、格式轉換和綁定
6. **期間選擇方法**：實現期間選擇和相關操作
7. **期間格式轉換方法**：處理不同格式的期間顯示
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
public partial class PeriodList : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler PeriodChanged;
    
    // 期間顯示格式枚舉
    public enum PeriodDisplayFormat
    {
        Numeric,     // 數字格式：1, 2, 3, ...
        Text,        // 文字格式：一月, 二月, ...
        Mixed        // 混合格式：1-一月, 2-二月, ...
    }
    
    // 期間排序方式枚舉
    public enum PeriodOrderType
    {
        Ascending,   // 升序排列：1, 2, 3, ...
        Descending   // 降序排列：12, 11, 10, ...
    }
    
    // 內部變數
    private int _selectedPeriod = DateTime.Now.Month;
    private PeriodDisplayFormat _displayFormat = PeriodDisplayFormat.Numeric;
    private int _minPeriod = 1;
    private int _maxPeriod = 12;
    private bool _required = true;
    private bool _showLabel = true;
    private string _labelText = "期間";
    private int _labelWidth = 60;
    private int _dropDownWidth = 80;
    private PeriodOrderType _periodOrder = PeriodOrderType.Ascending;
    private YearList _linkedYearList = null;
}
```

### 屬性定義

```csharp
// 選取的期間
public int SelectedPeriod
{
    get { return _selectedPeriod; }
    set 
    { 
        // 確保期間在有效範圍內
        if (value < _minPeriod) value = _minPeriod;
        if (value > _maxPeriod) value = _maxPeriod;
        
        _selectedPeriod = value;
        SelectPeriod(value);
    }
}

// 期間顯示格式
public PeriodDisplayFormat DisplayFormat
{
    get { return _displayFormat; }
    set 
    { 
        _displayFormat = value;
        if (!IsPostBack)
        {
            BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
        }
    }
}

// 最小期間
public int MinPeriod
{
    get { return _minPeriod; }
    set 
    { 
        _minPeriod = value;
        
        // 如果目前選取的期間小於最小期間，則更新選取的期間
        if (_selectedPeriod < _minPeriod)
        {
            _selectedPeriod = _minPeriod;
        }
        
        if (!IsPostBack)
        {
            BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
        }
    }
}

// 最大期間
public int MaxPeriod
{
    get { return _maxPeriod; }
    set 
    { 
        _maxPeriod = value;
        
        // 如果目前選取的期間大於最大期間，則更新選取的期間
        if (_selectedPeriod > _maxPeriod)
        {
            _selectedPeriod = _maxPeriod;
        }
        
        if (!IsPostBack)
        {
            BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
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
        lblPeriod.Visible = value;
    }
}

// 標籤文字
public string LabelText
{
    get { return _labelText; }
    set 
    { 
        _labelText = value;
        lblPeriod.Text = value;
    }
}

// 標籤寬度
public int LabelWidth
{
    get { return _labelWidth; }
    set 
    { 
        _labelWidth = value;
        lblPeriod.Width = value;
    }
}

// 下拉選單寬度
public int DropDownWidth
{
    get { return _dropDownWidth; }
    set 
    { 
        _dropDownWidth = value;
        ddlPeriod.Width = value;
    }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return ddlPeriod.Enabled; }
    set { ddlPeriod.Enabled = value; }
}

// 期間排序方式
public PeriodOrderType PeriodOrder
{
    get { return _periodOrder; }
    set 
    { 
        _periodOrder = value;
        if (!IsPostBack)
        {
            BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
        }
    }
}

// 關聯的年度控制項
public YearList LinkedYearList
{
    get { return _linkedYearList; }
    set 
    { 
        // 先解除之前的關聯
        if (_linkedYearList != null)
        {
            _linkedYearList.YearChanged -= YearList_YearChanged;
        }
        
        _linkedYearList = value;
        
        // 設定新的關聯
        if (_linkedYearList != null)
        {
            _linkedYearList.YearChanged += YearList_YearChanged;
        }
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
    // 設定標籤文字和寬度
    lblPeriod.Text = _labelText;
    lblPeriod.Width = _labelWidth;
    lblPeriod.Visible = _showLabel;
    
    // 設定下拉選單寬度
    ddlPeriod.Width = _dropDownWidth;
    
    // 綁定期間資料
    BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
    
    // 選擇預設期間
    SelectPeriod(_selectedPeriod);
}
```

#### 期間資料綁定方法

```csharp
// 綁定期間資料
public void BindPeriods(PeriodDisplayFormat format = PeriodDisplayFormat.Numeric, 
                      int minPeriod = 0, int maxPeriod = 0, 
                      PeriodOrderType orderType = PeriodOrderType.Ascending)
{
    // 清除現有項目
    ddlPeriod.Items.Clear();
    
    // 加入空白選項（如果不是必填項）
    if (!_required)
    {
        ddlPeriod.Items.Add(new ListItem("", ""));
    }
    
    try
    {
        // 如果未指定期間範圍，則使用控制項的預設範圍
        if (minPeriod <= 0) minPeriod = _minPeriod;
        if (maxPeriod <= 0) maxPeriod = _maxPeriod;
        
        // 確保期間範圍有效
        if (minPeriod > maxPeriod)
        {
            int temp = minPeriod;
            minPeriod = maxPeriod;
            maxPeriod = temp;
        }
        
        // 準備期間資料
        List<int> periods = new List<int>();
        for (int period = minPeriod; period <= maxPeriod; period++)
        {
            periods.Add(period);
        }
        
        // 根據排序方式排序期間
        if (orderType == PeriodOrderType.Descending)
        {
            periods.Sort((a, b) => b.CompareTo(a));
        }
        else
        {
            periods.Sort();
        }
        
        // 綁定期間資料到下拉選單
        foreach (int period in periods)
        {
            string periodText = GetPeriodDisplay(period, format);
            string periodValue = period.ToString();
            
            ListItem item = new ListItem(periodText, periodValue);
            ddlPeriod.Items.Add(item);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("BindPeriods Error: " + ex.Message);
    }
}

// 根據顯示格式取得期間顯示文字
public string GetPeriodDisplay(int period, PeriodDisplayFormat format)
{
    string[] periodTexts = new string[] 
    { 
        "一月", "二月", "三月", "四月", "五月", "六月", 
        "七月", "八月", "九月", "十月", "十一月", "十二月" 
    };
    
    // 確保期間值在有效範圍內
    if (period < 1 || period > 12)
        return period.ToString();
    
    switch (format)
    {
        case PeriodDisplayFormat.Numeric:
            // 數字格式：1, 2, 3, ...
            return period.ToString();
            
        case PeriodDisplayFormat.Text:
            // 文字格式：一月, 二月, ...
            return periodTexts[period - 1];
            
        case PeriodDisplayFormat.Mixed:
            // 混合格式：1-一月, 2-二月, ...
            return string.Format("{0}-{1}", period, periodTexts[period - 1]);
            
        default:
            return period.ToString();
    }
}

// 新增一段期間範圍
public void AddPeriodRange(int startPeriod, int endPeriod)
{
    try
    {
        // 確保期間範圍有效
        if (startPeriod > endPeriod)
        {
            int temp = startPeriod;
            startPeriod = endPeriod;
            endPeriod = temp;
        }
        
        // 更新最小期間和最大期間（如果需要）
        if (startPeriod < _minPeriod)
        {
            _minPeriod = startPeriod;
        }
        
        if (endPeriod > _maxPeriod)
        {
            _maxPeriod = endPeriod;
        }
        
        // 重新綁定期間資料
        BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
        
        // 如果目前選取的期間不在範圍內，則重新選擇
        if (_selectedPeriod < _minPeriod || _selectedPeriod > _maxPeriod)
        {
            _selectedPeriod = _minPeriod;
            SelectPeriod(_selectedPeriod);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("AddPeriodRange Error: " + ex.Message);
    }
}
```

#### 期間選擇方法

```csharp
// 根據期間值選取期間
public void SelectPeriod(int period)
{
    try
    {
        // 確保期間在有效範圍內
        if (period < _minPeriod) period = _minPeriod;
        if (period > _maxPeriod) period = _maxPeriod;
        
        // 選取對應的下拉選單項目
        ListItem item = ddlPeriod.Items.FindByValue(period.ToString());
        if (item != null)
        {
            ddlPeriod.SelectedValue = period.ToString();
            _selectedPeriod = period;
        }
        else
        {
            // 如果找不到指定的期間，則選取第一個期間（如果有）
            if (ddlPeriod.Items.Count > 0)
            {
                ddlPeriod.SelectedIndex = 0;
                if (!string.IsNullOrEmpty(ddlPeriod.SelectedValue))
                {
                    _selectedPeriod = int.Parse(ddlPeriod.SelectedValue);
                }
            }
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("SelectPeriod Error: " + ex.Message);
    }
}
```

#### 事件處理方法

```csharp
// 處理下拉選單選項變更事件
protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
{
    // 更新選取的期間
    if (!string.IsNullOrEmpty(ddlPeriod.SelectedValue))
    {
        _selectedPeriod = int.Parse(ddlPeriod.SelectedValue);
    }
    
    // 觸發期間變更事件
    OnPeriodChanged(EventArgs.Empty);
}

// 觸發期間變更事件
protected virtual void OnPeriodChanged(EventArgs e)
{
    if (PeriodChanged != null)
        PeriodChanged(this, e);
}

// 處理年度控制項的年度變更事件
private void YearList_YearChanged(object sender, EventArgs e)
{
    // 可以根據年度變更來調整期間範圍
    // 例如：對於非當前年度，可能需要顯示所有12個期間
    // 對於當前年度，可能只顯示到當前月份
    
    if (_linkedYearList != null)
    {
        int currentYear = DateTime.Now.Year;
        int selectedYear = _linkedYearList.SelectedYear;
        
        if (selectedYear == currentYear)
        {
            // 當前年度，可以限制期間到當前月份
            MaxPeriod = DateTime.Now.Month;
        }
        else
        {
            // 非當前年度，顯示所有12個期間
            MaxPeriod = 12;
        }
        
        // 重新綁定期間資料
        BindPeriods(_displayFormat, _minPeriod, _maxPeriod, _periodOrder);
        
        // 選擇適當的期間
        if (_selectedPeriod > _maxPeriod)
        {
            _selectedPeriod = _maxPeriod;
        }
        SelectPeriod(_selectedPeriod);
    }
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
    
    // 如果是必填項，則檢查是否有選取期間
    if (ddlPeriod.SelectedIndex < 0 || string.IsNullOrEmpty(ddlPeriod.SelectedValue))
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
    // 重設為當前月份或範圍內的有效期間
    int currentMonth = DateTime.Now.Month;
    if (currentMonth < _minPeriod) currentMonth = _minPeriod;
    if (currentMonth > _maxPeriod) currentMonth = _maxPeriod;
    
    SelectPeriod(currentMonth);
}

// 將期間控制項與年度控制項關聯
public bool LinkToYearList(YearList yearList)
{
    try
    {
        LinkedYearList = yearList;
        return true;
    }
    catch
    {
        return false;
    }
}
```

## 技術實現

PeriodList.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **枚舉型別**：定義期間顯示格式和排序方式的枚舉型別
5. **泛型集合**：使用 List<T> 處理期間資料集合和排序
6. **LINQ**：使用 LINQ 進行期間資料的排序和過濾

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| SelectedPeriod | int | 獲取或設定選取的期間 |
| DisplayFormat | PeriodDisplayFormat | 獲取或設定期間顯示格式 |
| MinPeriod | int | 獲取或設定可選擇的最小期間 |
| MaxPeriod | int | 獲取或設定可選擇的最大期間 |
| Required | bool | 獲取或設定期間選擇是否為必填 |
| ShowLabel | bool | 獲取或設定是否顯示期間標籤 |
| LabelText | string | 獲取或設定標籤文字 |
| LabelWidth | int | 獲取或設定標籤寬度 |
| DropDownWidth | int | 獲取或設定下拉選單寬度 |
| Enabled | bool | 獲取或設定控制項是否啟用 |
| PeriodOrder | PeriodOrderType | 獲取或設定期間排序方式 |
| LinkedYearList | YearList | 獲取或設定與之關聯的年度控制項 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindPeriods | format, minPeriod, maxPeriod, orderType (可選) | void | 綁定期間資料到下拉式選單 |
| SelectPeriod | period | void | 根據期間值選取下拉式選單項目 |
| GetPeriodDisplay | period, format | string | 根據顯示格式取得期間顯示文字 |
| AddPeriodRange | startPeriod, endPeriod | void | 新增一段期間範圍 |
| Validate | 無 | bool | 驗證控制項是否符合必填要求 |
| Reset | 無 | void | 重設控制項為預設狀態 |
| LinkToYearList | yearList | bool | 將期間控制項與年度控制項關聯 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| PeriodChanged | EventArgs | 期間選擇變更時觸發的事件 |

## 程式碼分析

### 強項

1. **期間範圍的彈性**：
   - 提供最小期間和最大期間的設定
   - 支援動態添加期間範圍
   - 確保選取的期間在有效範圍內

2. **顯示格式的多樣性**：
   - 支援數字、文字和混合三種顯示格式
   - 提供簡潔的格式轉換機制
   - 良好的擴展性，可以輕鬆添加更多顯示格式

3. **排序方式的彈性**：
   - 支援升序和降序兩種排序方式
   - 使用 LINQ 實現高效的排序邏輯

4. **使用者體驗的考量**：
   - 提供彈性的配置選項，如標籤文字、寬度等
   - 支援預設期間的自動載入
   - 處理各種錯誤情況，確保良好的使用者體驗

5. **年度整合**：
   - 提供與年度控制項的整合機制
   - 處理年度變更時期間範圍的自動調整

### 待改進點

1. **功能擴充**：
   - 實現更多的期間顯示格式，如季度顯示格式
   - 支援不同的期間定義系統（如財政年度的期間定義）

2. **客戶端功能**：
   - 增加更多的客戶端功能，如即時期間範圍調整
   - 提供更豐富的客戶端驗證

3. **使用者介面增強**：
   - 提供更多的視覺反饋
   - 支援期間的快捷選項，如「本期」、「上期」等

## 安全性考量

1. **輸入驗證**：
   - 驗證使用者選取的期間值
   - 防止非法的期間值被設定

2. **錯誤處理**：
   - 適當處理和記錄錯誤情況
   - 不向使用者暴露敏感的錯誤訊息

## 效能考量

1. **資料載入優化**：
   - 期間資料由控制項動態產生，無需資料庫查詢
   - 控制項初始化時一次性載入所有期間資料

2. **記憶體使用**：
   - 控制項使用的資源較少，不會造成明顯的記憶體負擔
   - 避免不必要的大型物件建立

3. **回應時間**：
   - 確保期間選擇操作的響應速度
   - 使用 LINQ 和泛型集合提高資料處理效率

## 待改進事項

1. 實現會計期間選擇功能（如季度期間、特殊期間等）
2. 增加期間與年度的聯動功能，實現更完整的期間選擇
3. 增加更多的期間顯示格式，如財政年度格式
4. 增加期間選擇的快捷方式，如「本期」、「上期」等選項
5. 優化控制項在移動設備上的使用體驗
6. 增加多語言支援，以適應國際化需求
7. 提供期間範圍的自動更新功能，避免跨年時需要手動更新期間範圍
8. 增加期間有效性的驗證，確保選擇的期間符合業務規則

## 維護記錄

| 日期      | 版本   | 變更內容                               | 變更人員    |
|-----------|--------|--------------------------------------|------------|
| 2025/5/9  | 1.0    | 首次建立期間清單程式碼規格書           | Claude AI  | 
| 2025/5/6  | 1.0    | 首次建立期間清單程式碼規格書          | Claude AI  | 