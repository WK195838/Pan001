# CalendarDay.ascx.cs 日控制項程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CalendarDay.ascx.cs                    |
| 程式名稱     | 日控制項程式碼                           |
| 檔案大小     | 5.3KB                                 |
| 行數        | ~180                                  |
| 功能簡述     | 日選擇控制項後端邏輯                      |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/12                             |

## 程式功能概述

CalendarDay.ascx.cs 是泛太總帳系統中提供「日選擇」功能的使用者控制項後端程式碼。此控制項專門設計用於處理日期中「日」的部分，提供一個簡單且統一的介面，讓使用者能夠從預設範圍內選擇特定日期中的日。主要功能包括：

1. 提供 1-31 數字範圍供選擇，對應每月的日
2. 根據所選月份自動調整可選擇的最大日數
3. 支援預設值設定和自動回填
4. 支援可選/必填模式設定
5. 提供使用者輸入驗證功能
6. 可透過程式碼或使用者介面進行日期設定
7. 支援唯讀模式的顯示
8. 提供變更事件通知機制
9. 整合系統樣式和顯示標準
10. 支援日期格式化顯示

## 程式架構說明

CalendarDay.ascx.cs 的架構包含以下主要部分：

1. **命名空間和引用**：包含系統必要的命名空間引用
2. **類別定義**：定義 CalendarDay 控制項類別及其繼承關係
3. **屬性定義**：定義控制項的各種屬性和設定
4. **控制項變數**：包含頁面上各個 UI 元素的變數定義
5. **生命週期方法**：包含控制項初始化和載入邏輯
6. **事件處理方法**：處理使用者互動事件
7. **資料驗證方法**：處理使用者輸入的驗證邏輯
8. **輔助方法**：提供各種功能支援的輔助方法
9. **資料轉換方法**：處理日期格式轉換和資料型態的轉換
10. **事件定義和觸發機制**：定義和處理控制項的事件通知

## 技術實現

CalendarDay.ascx.cs 使用以下技術實現其功能：

1. **ASP.NET Web Forms**：基於 ASP.NET Web Forms 架構開發
2. **使用者控制項 (UserControl)**：繼承 System.Web.UI.UserControl 類別
3. **事件委派**：使用 .NET 事件委派機制實現事件通知
4. **伺服器控制項**：使用 ASP.NET 內建的伺服器控制項
5. **驗證功能**：使用 ASP.NET 驗證控制項和自定義驗證邏輯
6. **JavaScript 整合**：整合前端 JavaScript 提升使用者體驗
7. **CSS 樣式整合**：套用一致的系統 CSS 樣式

## 依賴關係

CalendarDay.ascx.cs 依賴以下元件和檔案：

1. **CalendarDay.ascx**：控制項的 UI 定義文件
2. **System.Web.UI 命名空間**：ASP.NET 基礎元件
3. **pagefunction.js**：系統共用的 JavaScript 函數庫
4. **系統樣式表**：提供統一的外觀和風格
5. **其他月份相關控制項**：通常與 CalendarMonth 等控制項協同工作

## 主要類別和方法

### 類別定義

```csharp
public partial class CalendarDay : System.Web.UI.UserControl
{
    // 類別實現
}
```

### 主要屬性

```csharp
// 取得或設定目前選擇的日
public int SelectedDay { get; set; }

// 取得或設定控制項是否為必填
public bool Required { get; set; }

// 取得或設定控制項是否為唯讀
public bool ReadOnly { get; set; }

// 取得或設定所選月份 (用於計算當月最大天數)
public int SelectedMonth { get; set; }

// 取得或設定所選年份 (用於閏年計算)
public int SelectedYear { get; set; }

// 取得或設定控制項的標籤文字
public string LabelText { get; set; }

// 取得或設定控制項的顯示寬度
public Unit Width { get; set; }

// 取得或設定控制項的 CSS 類別
public string CssClass { get; set; }

// 取得控制項是否通過驗證
public bool IsValid { get; }
```

### 主要事件

```csharp
// 當選擇的日變更時觸發
public event EventHandler<DayChangedEventArgs> DayChanged;

// 當控制項驗證完成時觸發
public event EventHandler<DayValidatedEventArgs> DayValidated;
```

### 主要方法

```csharp
// 頁面生命週期方法
protected void Page_Load(object sender, EventArgs e)
protected void Page_Init(object sender, EventArgs e)

// 使用者互動處理方法
protected void ddlDay_SelectedIndexChanged(object sender, EventArgs e)
protected void txtDay_TextChanged(object sender, EventArgs e)

// 驗證方法
protected void cvDay_ServerValidate(object source, ServerValidateEventArgs args)
public bool Validate()

// 資料相關方法
public void SetDay(int day)
public void Clear()
public void UpdateMaxDayForMonth()

// 事件觸發方法
protected virtual void OnDayChanged(int oldDay, int newDay)
protected virtual void OnDayValidated(bool isValid)

// 輔助方法
private void InitializeControl()
private int GetMaxDayForMonth()
private void UpdateDaySelection()
```

## 方法詳細說明

### 生命週期方法

#### Page_Load

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 初始化日選擇下拉清單
        InitializeDayDropDownList();
        
        // 設定控制項的初始狀態
        SetControlState();
        
        // 如果有預設值，設定選擇的日
        if (SelectedDay > 0)
        {
            UpdateDaySelection();
        }
    }
    
    // 設定控制項的可見性和唯讀狀態
    UpdateControlDisplay();
}
```

此方法在頁面載入時執行，初始化控制項並設定其狀態。

#### Page_Init

```csharp
protected void Page_Init(object sender, EventArgs e)
{
    // 註冊客戶端驗證腳本
    RegisterClientValidationScript();
    
    // 設定驗證控制項
    ConfigureValidators();
}
```

此方法在頁面初始化階段執行，註冊必要的客戶端腳本和設定驗證控制項。

### 使用者互動處理方法

#### ddlDay_SelectedIndexChanged

```csharp
protected void ddlDay_SelectedIndexChanged(object sender, EventArgs e)
{
    // 獲取舊值
    int oldDay = SelectedDay;
    
    // 更新選擇的日
    if (int.TryParse(ddlDay.SelectedValue, out int newDay))
    {
        SelectedDay = newDay;
        txtDay.Text = newDay.ToString();
        
        // 觸發變更事件
        OnDayChanged(oldDay, newDay);
    }
}
```

此方法處理使用者從下拉選單選擇日時的事件，更新選擇的日並觸發變更事件。

#### txtDay_TextChanged

```csharp
protected void txtDay_TextChanged(object sender, EventArgs e)
{
    // 獲取舊值
    int oldDay = SelectedDay;
    
    // 嘗試解析輸入的日
    if (int.TryParse(txtDay.Text, out int newDay))
    {
        int maxDay = GetMaxDayForMonth();
        
        // 驗證日值的範圍
        if (newDay >= 1 && newDay <= maxDay)
        {
            SelectedDay = newDay;
            
            // 更新下拉選單
            ListItem item = ddlDay.Items.FindByValue(newDay.ToString());
            if (item != null)
            {
                ddlDay.SelectedValue = newDay.ToString();
            }
            
            // 觸發變更事件
            OnDayChanged(oldDay, newDay);
        }
        else
        {
            // 輸入無效，恢復舊值
            txtDay.Text = oldDay > 0 ? oldDay.ToString() : string.Empty;
        }
    }
    else
    {
        // 輸入無效，恢復舊值
        txtDay.Text = oldDay > 0 ? oldDay.ToString() : string.Empty;
    }
}
```

此方法處理使用者直接在文字框輸入日時的事件，驗證輸入值並相應更新控制項。

### 驗證方法

#### cvDay_ServerValidate

```csharp
protected void cvDay_ServerValidate(object source, ServerValidateEventArgs args)
{
    string dayValue = args.Value.Trim();
    
    // 檢查是否為必填
    if (string.IsNullOrEmpty(dayValue))
    {
        args.IsValid = !Required;
        return;
    }
    
    // 嘗試解析日值
    if (int.TryParse(dayValue, out int day))
    {
        int maxDay = GetMaxDayForMonth();
        
        // 檢查日值範圍
        args.IsValid = day >= 1 && day <= maxDay;
    }
    else
    {
        args.IsValid = false;
    }
    
    // 觸發驗證事件
    OnDayValidated(args.IsValid);
}
```

此方法實現伺服器端日值驗證邏輯，檢查是否符合有效範圍。

#### Validate

```csharp
public bool Validate()
{
    // 如果控制項隱藏或唯讀，則視為有效
    if (!Visible || ReadOnly)
    {
        return true;
    }
    
    // 執行驗證控制項
    cvDay.Validate();
    
    return cvDay.IsValid;
}
```

此方法提供公開的驗證介面，允許外部代碼手動觸發驗證。

### 資料相關方法

#### SetDay

```csharp
public void SetDay(int day)
{
    // 獲取當前月份的最大日
    int maxDay = GetMaxDayForMonth();
    
    // 驗證日值範圍
    if (day >= 1 && day <= maxDay)
    {
        int oldDay = SelectedDay;
        SelectedDay = day;
        
        // 更新控制項顯示
        txtDay.Text = day.ToString();
        
        // 更新下拉選單選擇
        ListItem item = ddlDay.Items.FindByValue(day.ToString());
        if (item != null)
        {
            ddlDay.SelectedValue = day.ToString();
        }
        
        // 觸發變更事件
        if (oldDay != day)
        {
            OnDayChanged(oldDay, day);
        }
    }
}
```

此方法設定控制項的選擇日值，更新UI並觸發相應事件。

#### UpdateMaxDayForMonth

```csharp
public void UpdateMaxDayForMonth()
{
    // 獲取目前月份的最大日
    int maxDay = GetMaxDayForMonth();
    
    // 暫存目前選擇的日
    int currentDay = SelectedDay;
    
    // 清除並重建下拉選單項目
    ddlDay.Items.Clear();
    
    // 添加空白選項
    if (!Required)
    {
        ddlDay.Items.Add(new ListItem("", "0"));
    }
    
    // 添加 1 到最大日的選項
    for (int i = 1; i <= maxDay; i++)
    {
        ddlDay.Items.Add(new ListItem(i.ToString(), i.ToString()));
    }
    
    // 嘗試恢復原來選擇的日
    if (currentDay > 0 && currentDay <= maxDay)
    {
        ddlDay.SelectedValue = currentDay.ToString();
    }
    else if (currentDay > maxDay && maxDay > 0)
    {
        // 如果原來的日超過新的最大日，則設為最大日
        ddlDay.SelectedValue = maxDay.ToString();
        txtDay.Text = maxDay.ToString();
        
        // 如果原來有選擇日，則觸發變更事件
        if (currentDay > 0)
        {
            OnDayChanged(currentDay, maxDay);
        }
        
        SelectedDay = maxDay;
    }
}
```

此方法根據當前選擇的月份和年份更新可選擇的最大日數，保持控制項狀態一致。

#### GetMaxDayForMonth

```csharp
private int GetMaxDayForMonth()
{
    if (SelectedMonth <= 0 || SelectedYear <= 0)
    {
        // 如果月份或年份未設定，返回預設的31天
        return 31;
    }
    
    switch (SelectedMonth)
    {
        case 2: // 二月
            // 檢查是否為閏年
            if ((SelectedYear % 4 == 0 && SelectedYear % 100 != 0) || SelectedYear % 400 == 0)
            {
                return 29;
            }
            else
            {
                return 28;
            }
        case 4: // 四月
        case 6: // 六月
        case 9: // 九月
        case 11: // 十一月
            return 30;
        default:
            return 31;
    }
}
```

此方法根據選擇的月份和年份計算當月的最大天數，處理閏年等特殊情況。

### 事件觸發方法

#### OnDayChanged

```csharp
protected virtual void OnDayChanged(int oldDay, int newDay)
{
    if (DayChanged != null)
    {
        // 創建事件參數
        DayChangedEventArgs args = new DayChangedEventArgs(oldDay, newDay);
        
        // 觸發事件
        DayChanged(this, args);
    }
}
```

此方法在選擇的日變更時觸發事件，提供舊值和新值。

#### OnDayValidated

```csharp
protected virtual void OnDayValidated(bool isValid)
{
    if (DayValidated != null)
    {
        // 創建事件參數
        DayValidatedEventArgs args = new DayValidatedEventArgs(isValid);
        
        // 觸發事件
        DayValidated(this, args);
    }
}
```

此方法在日值驗證完成時觸發事件，提供驗證結果。

## 自定義類型定義

### DayChangedEventArgs

```csharp
public class DayChangedEventArgs : EventArgs
{
    public int OldDay { get; private set; }
    public int NewDay { get; private set; }
    
    public DayChangedEventArgs(int oldDay, int newDay)
    {
        OldDay = oldDay;
        NewDay = newDay;
    }
}
```

此類定義日變更事件的參數，包含舊日值和新日值。

### DayValidatedEventArgs

```csharp
public class DayValidatedEventArgs : EventArgs
{
    public bool IsValid { get; private set; }
    
    public DayValidatedEventArgs(bool isValid)
    {
        IsValid = isValid;
    }
}
```

此類定義日驗證事件的參數，包含驗證結果。

## 異常處理

### 輸入驗證處理

```csharp
private bool TryParseDay(string input, out int day)
{
    // 嘗試轉換輸入為整數
    if (int.TryParse(input, out day))
    {
        // 檢查範圍
        int maxDay = GetMaxDayForMonth();
        return day >= 1 && day <= maxDay;
    }
    
    day = 0;
    return false;
}
```

此方法嘗試解析輸入字串為有效的日值，處理可能的錯誤。

### 異常恢復

```csharp
private void RecoverFromInvalidInput()
{
    // 恢復為選擇的日值
    if (SelectedDay > 0)
    {
        txtDay.Text = SelectedDay.ToString();
        
        ListItem item = ddlDay.Items.FindByValue(SelectedDay.ToString());
        if (item != null)
        {
            ddlDay.SelectedValue = SelectedDay.ToString();
        }
    }
    else
    {
        // 清除輸入
        txtDay.Text = string.Empty;
        ddlDay.SelectedIndex = 0;
    }
}
```

此方法從無效輸入恢復，確保控制項狀態一致。

## 使用範例

以下是 CalendarDay.ascx.cs 的使用範例：

```csharp
// 在頁面載入時設定日控制項
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 設定日控制項屬性
        dayControl.Required = true;
        dayControl.ReadOnly = false;
        dayControl.LabelText = "日:";
        
        // 設定月份和年份以更新可選擇的最大日
        dayControl.SelectedMonth = int.Parse(monthControl.SelectedValue);
        dayControl.SelectedYear = int.Parse(yearControl.SelectedValue);
        
        // 註冊事件處理器
        dayControl.DayChanged += new EventHandler<DayChangedEventArgs>(dayControl_DayChanged);
        
        // 更新最大日值
        dayControl.UpdateMaxDayForMonth();
        
        // 如果有初始日值，則設定日
        if (!string.IsNullOrEmpty(Request.QueryString["Day"]))
        {
            int day;
            if (int.TryParse(Request.QueryString["Day"], out day))
            {
                dayControl.SetDay(day);
            }
        }
    }
}

// 處理日變更事件
protected void dayControl_DayChanged(object sender, DayChangedEventArgs e)
{
    // 顯示選擇的日
    lblSelectedDay.Text = string.Format("已選擇日: {0}", e.NewDay);
    
    // 執行其他相關邏輯
    UpdateSelectedDate();
}

// 處理月份變更事件
protected void monthControl_MonthChanged(object sender, MonthChangedEventArgs e)
{
    // 更新日控制項的月份
    dayControl.SelectedMonth = e.NewMonth;
    
    // 更新可選擇的最大日
    dayControl.UpdateMaxDayForMonth();
}

// 處理年份變更事件
protected void yearControl_YearChanged(object sender, YearChangedEventArgs e)
{
    // 更新日控制項的年份
    dayControl.SelectedYear = e.NewYear;
    
    // 更新可選擇的最大日（針對閏年）
    dayControl.UpdateMaxDayForMonth();
}

// 保存日期
protected void btnSave_Click(object sender, EventArgs e)
{
    // 驗證控制項
    if (dayControl.Validate())
    {
        // 獲取選擇的日
        int day = dayControl.SelectedDay;
        
        // 執行保存邏輯
    }
}
```

## 效能考量

1. **資料載入優化**：
   - 只在需要時才更新下拉選單內容
   - 避免不必要的頁面回傳

2. **事件處理效率**：
   - 只有在值真正變更時才觸發變更事件
   - 避免重複驗證和更新

3. **控制項渲染優化**：
   - 適當使用客戶端驗證減少伺服器處理
   - 最小化輸出的 HTML 大小

4. **計算效能**：
   - 快取計算結果例如月份的最大天數
   - 使用高效的演算法計算日期相關值

## 安全性考量

1. **輸入驗證**：
   - 對日值進行嚴格的格式和範圍驗證
   - 防止潛在的跨站腳本攻擊

2. **資料處理安全**：
   - 處理所有可能的輸入錯誤和異常情況
   - 確保控制項狀態一致性

3. **狀態管理**：
   - 妥善處理頁面回傳時的狀態保存和恢復
   - 防止狀態操縱攻擊

## 測試情況

1. **功能測試**：
   - 測試日選擇的正確性
   - 測試與其他日期控制項的整合
   - 測試月份變更時的最大日更新

2. **界面測試**：
   - 測試不同瀏覽器下的顯示一致性
   - 測試唯讀和必填模式下的行為

3. **邊界測試**：
   - 測試月份邊界值（如 2 月 28/29 日）
   - 測試閏年特殊情況

4. **錯誤處理測試**：
   - 測試無效輸入的處理
   - 測試異常恢復功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/12 | 1.0    | 首次建立日控制項程式碼規格書          | Claude AI  | 