# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | CalendarDate |
| 程式名稱 | 日期控制項 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Controls/CalendarDate.ascx, /GLATEST/app/Controls/CalendarDate.ascx.cs |
| 程式類型 | 使用者控制項 (User Control) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

CalendarDate 是泛太總帳系統的日期選擇控制項，提供使用者友善的日期選擇界面，支援各種日期格式與驗證功能。該控制項可用於所有需要日期輸入的表單與查詢畫面，確保日期資料的一致性和準確性。

### 2.2 業務流程

CalendarDate 在系統中扮演以下角色：
1. 提供統一的日期選擇介面
2. 確保日期格式符合系統標準
3. 驗證日期輸入的有效性
4. 支援不同的日期範圍限制
5. 提供日期相關計算功能
6. 整合會計期間概念，支援財務相關日期運算

### 2.3 使用頻率

- 高頻率：系統中的絕大多數表單和查詢頁面均需使用此控制項
- 用戶每次進行資料輸入或查詢時皆會使用
- 平均每位使用者每天使用數十次

### 2.4 使用者角色

此控制項服務於系統的所有用戶，包括：
- 系統管理員：系統設定與管理
- 財務主管：財務報表查詢與設定
- 會計人員：日常交易資料輸入
- 一般用戶：基本資料查詢

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Forms (.NET Framework 4.0)
- 主要技術：
  - HTML/CSS 構建界面
  - JavaScript/jQuery 實現日期選擇器
  - C# 後端日期處理邏輯
- 客戶端整合：使用jQuery UI Datepicker元件
- 伺服器端驗證：確保資料符合業務規則

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_PARAMETER | 系統參數表，讀取日期格式設定 | 讀取 |
| GL_PERIOD | 會計期間表，用於驗證與會計期間相關的日期 | 讀取 |
| SYS_USER_CONFIG | 使用者配置表，讀取用戶日期偏好設定 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| jQuery UI | 提供Datepicker功能 | 版本: 1.12.1 |
| IBosDB | 資料庫存取 | 提供系統參數查詢 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| SysParamManager | 系統參數管理 | 取得日期相關參數設定 |

## 4. 控制項設計

### 4.1 界面結構

CalendarDate 控制項的界面由以下元素構成：

```
+--------------------------------------------+
| [文字標籤] [文字輸入框] [日曆按鈕]          |
+--------------------------------------------+
|           +------------------+             |
|           | 日曆彈出視窗     |             |
|           | +--+--+--+--+--+ |             |
|           | |日|一|二|三|四| |             |
|           | +--+--+--+--+--+ |             |
|           | |五|六|1 |2 |3 | |             |
|           | +--+--+--+--+--+ |             |
|           | |4 |5 |6 |...|  | |             |
|           | +--+--+--+--+--+ |             |
|           +------------------+             |
+--------------------------------------------+
```

### 4.2 屬性設計

| 屬性名稱 | 類型 | 存取修飾詞 | 說明 |
|---------|------|----------|------|
| DateValue | DateTime | public | 控制項的日期值 |
| DateString | string | public | 格式化後的日期字串 |
| DateFormat | string | public | 日期格式(預設: yyyy/MM/dd) |
| IsRequired | bool | public | 是否為必填欄位 |
| MinDate | DateTime | public | 允許的最小日期 |
| MaxDate | DateTime | public | 允許的最大日期 |
| EnablePeriodCheck | bool | public | 啟用會計期間檢查 |
| ShowLabel | bool | public | 是否顯示文字標籤 |
| LabelText | string | public | 標籤文字 |
| LabelWidth | int | public | 標籤寬度 |
| InputWidth | int | public | 輸入框寬度 |
| ValidationGroup | string | public | 驗證群組 |

### 4.3 參數定義

CalendarDate 控制項支援以下WebForm參數：

```
<%@ Register Src="~/Controls/CalendarDate.ascx" TagPrefix="pan" TagName="CalendarDate" %>
<pan:CalendarDate ID="txtTransDate" runat="server" 
                 DateValue='<%# DateTime.Now %>' 
                 DateFormat="yyyy/MM/dd" 
                 IsRequired="true"
                 LabelText="交易日期:"
                 LabelWidth="100"
                 InputWidth="150"
                 EnablePeriodCheck="true"
                 ValidationGroup="vgTrans" />
```

### 4.4 CSS設計

控制項使用以下核心CSS類別：

```css
.cal-container { /* 容器 */ }
.cal-label { /* 文字標籤 */ }
.cal-input { /* 日期輸入框 */ }
.cal-button { /* 日曆按鈕 */ }
.cal-error { /* 錯誤提示 */ }
.cal-required { /* 必填標記 */ }
.cal-datepicker { /* 自定義日曆樣式 */ }
.cal-today { /* 今日樣式 */ }
.cal-selected { /* 已選擇樣式 */ }
.cal-weekend { /* 週末樣式 */ }
.cal-disabled { /* 禁用日期樣式 */ }
```

## 5. 主要方法

### 5.1 初始化和載入方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入時初始化控制項 | object sender, EventArgs e | void |
| InitializeCalendar | 初始化日曆組件 | 無 | void |
| LoadParameters | 載入系統參數設定 | 無 | void |
| RegisterClientScript | 註冊客戶端腳本 | 無 | void |
| SetupValidation | 設置驗證控制項 | 無 | void |
| GetDefaultFormat | 取得預設日期格式 | 無 | string |

### 5.2 事件處理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| DateChanged | 處理日期變更事件 | object sender, EventArgs e | void |
| CalendarButtonClick | 處理日曆按鈕點擊事件 | object sender, EventArgs e | void |
| DateTextChanged | 處理日期文字變更事件 | object sender, EventArgs e | void |
| OnDateSelected | 觸發日期選擇事件 | 無 | void |
| ValidateDate | 驗證日期格式與範圍 | 無 | bool |
| CheckPeriodValidity | 檢查日期是否在有效會計期間內 | DateTime date | bool |

### 5.3 工具方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| FormatDate | 格式化日期為字串 | DateTime date, string format | string |
| ParseDate | 將字串解析為日期 | string dateStr | DateTime |
| IsValidDateString | 檢查字串是否為有效日期格式 | string dateStr | bool |
| AddDays | 增加天數 | int days | void |
| AddMonths | 增加月數 | int months | void |
| CalculateAge | 計算年齡 | DateTime birthdate | int |
| GetFirstDayOfMonth | 取得月份第一天 | DateTime date | DateTime |
| GetLastDayOfMonth | 取得月份最後一天 | DateTime date | DateTime |
| GetWeekday | 取得星期幾 | DateTime date | DayOfWeek |
| IsWeekend | 檢查是否為週末 | DateTime date | bool |

### 5.4 渲染方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| RenderControl | 渲染控制項 | HtmlTextWriter writer | void |
| RenderLabel | 渲染標籤 | HtmlTextWriter writer | void |
| RenderInput | 渲染輸入框 | HtmlTextWriter writer | void |
| RenderButton | 渲染按鈕 | HtmlTextWriter writer | void |
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
設置日期格式
 ↓
初始化日期值 → 未設定值 → 使用今天日期
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
使用者點擊日曆按鈕
 ↓
顯示日曆彈出視窗
 ↓
使用者選擇日期
 ↓
檢查日期有效性 → 無效 → 顯示錯誤訊息 → 結束
 ↓
更新輸入框日期值
 ↓
觸發DateChanged事件
 ↓
隱藏日曆彈出視窗
 ↓
結束
```

### 6.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 日期格式無效 | 顯示格式提示，保留原值 | 提示正確的日期格式，紅色強調輸入框 |
| 日期超出範圍 | 顯示範圍提示，設為最近有效日期 | 提示有效日期範圍，自動修正為有效值 |
| 日期為空但必填 | 顯示必填提示，阻止表單提交 | 提示必須輸入日期，紅色強調輸入框 |
| 日期不在有效會計期間 | 顯示期間提示，允許覆寫 | 黃色警告提示，但允許繼續操作 |
| 客戶端JavaScript錯誤 | 降級為純文字輸入 | 退回到基本文字輸入功能，保持可用性 |

## 7. 代碼說明

### 7.1 控制項註冊與頁面使用

```aspx
<%-- 在頁面中註冊與使用CalendarDate控制項 --%>
<%@ Register Src="~/Controls/CalendarDate.ascx" TagPrefix="pan" TagName="CalendarDate" %>

<div class="form-group">
    <pan:CalendarDate ID="txtDocDate" runat="server" 
                     DateValue='<%# DateTime.Now %>' 
                     IsRequired="true"
                     LabelText="單據日期:"
                     ValidationGroup="vgMain" />
</div>
```

### 7.2 初始化實現

```csharp
/// <summary>
/// 初始化日曆控制項
/// </summary>
private void InitializeCalendar()
{
    try
    {
        // 載入系統參數設定
        LoadParameters();
        
        // 如果未設定日期格式，使用預設格式
        if (string.IsNullOrEmpty(DateFormat))
        {
            DateFormat = GetDefaultFormat();
        }
        
        // 如果未設定日期值，使用今天日期
        if (DateValue == DateTime.MinValue)
        {
            DateValue = DateTime.Today;
        }
        
        // 設置最小與最大日期範圍
        if (MinDate == DateTime.MinValue)
        {
            MinDate = new DateTime(1900, 1, 1);
        }
        
        if (MaxDate == DateTime.MinValue)
        {
            MaxDate = new DateTime(2100, 12, 31);
        }
        
        // 更新日期字串
        DateString = FormatDate(DateValue, DateFormat);
        
        // 設置輸入框ID與屬性
        txtDate.ID = ID + "_Input";
        txtDate.Attributes["data-dateformat"] = DateFormat;
        txtDate.Text = DateString;
        
        // 設置標籤
        if (ShowLabel)
        {
            lblDate.Text = LabelText;
            lblDate.Width = Unit.Pixel(LabelWidth);
            
            if (IsRequired)
            {
                lblDate.Text += "<span class='cal-required'>*</span>";
            }
        }
        else
        {
            lblDate.Visible = false;
        }
        
        // 設置輸入框寬度
        if (InputWidth > 0)
        {
            txtDate.Width = Unit.Pixel(InputWidth);
        }
        
        // 設置日曆按鈕ID
        btnCalendar.ID = ID + "_Button";
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDate.InitializeCalendar Error", ex);
    }
}
```

### 7.3 日期驗證實現

```csharp
/// <summary>
/// 驗證日期格式與範圍
/// </summary>
/// <returns>是否驗證通過</returns>
private bool ValidateDate()
{
    try
    {
        // 檢查必填
        if (IsRequired && string.IsNullOrEmpty(txtDate.Text))
        {
            SetErrorMessage("日期欄位必須填寫");
            return false;
        }
        
        // 如果為空且非必填，視為有效
        if (!IsRequired && string.IsNullOrEmpty(txtDate.Text))
        {
            return true;
        }
        
        // 檢查格式
        DateTime parsedDate;
        if (!DateTime.TryParseExact(txtDate.Text, DateFormat, CultureInfo.InvariantCulture, 
                                   DateTimeStyles.None, out parsedDate))
        {
            SetErrorMessage($"日期格式必須為 {DateFormat}");
            return false;
        }
        
        // 檢查範圍
        if (parsedDate < MinDate)
        {
            SetErrorMessage($"日期不能早於 {FormatDate(MinDate, DateFormat)}");
            return false;
        }
        
        if (parsedDate > MaxDate)
        {
            SetErrorMessage($"日期不能晚於 {FormatDate(MaxDate, DateFormat)}");
            return false;
        }
        
        // 檢查會計期間
        if (EnablePeriodCheck && !CheckPeriodValidity(parsedDate))
        {
            SetErrorMessage("所選日期不在有效的會計期間內");
            return false;
        }
        
        // 更新日期值
        DateValue = parsedDate;
        DateString = FormatDate(parsedDate, DateFormat);
        
        return true;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDate.ValidateDate Error", ex);
        SetErrorMessage("日期驗證過程發生錯誤");
        return false;
    }
}
```

### 7.4 會計期間檢查實現

```csharp
/// <summary>
/// 檢查日期是否在有效會計期間內
/// </summary>
/// <param name="date">要檢查的日期</param>
/// <returns>是否在有效期間內</returns>
private bool CheckPeriodValidity(DateTime date)
{
    try
    {
        // 如果未啟用會計期間檢查，直接返回有效
        if (!EnablePeriodCheck)
        {
            return true;
        }
        
        // 獲取當前用戶與公司
        string userId = LoginClass.Instance.CurrentUser.UserId;
        string companyId = LoginClass.Instance.CurrentUser.CompanyId;
        
        // 獲取資料庫連接
        IBosDB db = DBFactory.GetBosDB();
        
        // 查詢會計期間資料
        string sql = @"
            SELECT 1 
            FROM GL_PERIOD 
            WHERE COMPANY_ID = @CompanyId 
              AND @CheckDate BETWEEN BEGIN_DATE AND END_DATE 
              AND STATUS = 'O'";
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@CompanyId", companyId);
        parameters.Add("@CheckDate", date);
        
        // 執行查詢
        DataTable dt = db.ExecuteDataTable(sql, parameters);
        
        // 檢查結果
        return dt.Rows.Count > 0;
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDate.CheckPeriodValidity Error", ex);
        
        // 出錯時保守策略：返回有效
        return true;
    }
}
```

### 7.5 客戶端腳本註冊

```csharp
/// <summary>
/// 註冊客戶端腳本
/// </summary>
private void RegisterClientScript()
{
    try
    {
        // 確保頁面包含jQuery和jQuery UI
        ClientScriptManager cs = Page.ClientScript;
        
        if (!cs.IsClientScriptIncludeRegistered("jQuery"))
        {
            cs.RegisterClientScriptInclude("jQuery", ResolveUrl("~/Scripts/jquery-3.6.0.min.js"));
        }
        
        if (!cs.IsClientScriptIncludeRegistered("jQueryUI"))
        {
            cs.RegisterClientScriptInclude("jQueryUI", ResolveUrl("~/Scripts/jquery-ui-1.12.1.min.js"));
        }
        
        if (!cs.IsClientScriptIncludeRegistered("jQueryUI_CSS"))
        {
            string cssLink = "<link rel='stylesheet' type='text/css' href='" + 
                             ResolveUrl("~/Content/jquery-ui-1.12.1.min.css") + "' />";
            
            Page.Header.Controls.Add(new LiteralControl(cssLink));
        }
        
        // 生成初始化日期選擇器的腳本
        StringBuilder script = new StringBuilder();
        script.Append("<script type='text/javascript'>\n");
        script.Append("$(document).ready(function() {\n");
        
        script.Append($"  $('#{txtDate.ClientID}').datepicker({{\n");
        script.Append($"    dateFormat: '{DateFormatToJqueryFormat(DateFormat)}',\n");
        script.Append($"    minDate: new Date({MinDate.Year}, {MinDate.Month-1}, {MinDate.Day}),\n");
        script.Append($"    maxDate: new Date({MaxDate.Year}, {MaxDate.Month-1}, {MaxDate.Day}),\n");
        script.Append("    showOtherMonths: true,\n");
        script.Append("    selectOtherMonths: true,\n");
        script.Append("    changeMonth: true,\n");
        script.Append("    changeYear: true,\n");
        script.Append("    yearRange: '1900:2100',\n");
        script.Append("    showOn: 'button',\n");
        script.Append($"    buttonImage: '{ResolveUrl("~/Images/calendar.png")}',\n");
        script.Append("    buttonImageOnly: true,\n");
        script.Append("    buttonText: '選擇日期',\n");
        script.Append($"    onSelect: function(dateText) {{\n");
        script.Append($"      $('#{txtDate.ClientID}').trigger('change');\n");
        script.Append("    }\n");
        script.Append("  });\n");
        
        script.Append($"  $('#{btnCalendar.ClientID}').click(function() {{\n");
        script.Append($"    $('#{txtDate.ClientID}').datepicker('show');\n");
        script.Append("    return false;\n");
        script.Append("  });\n");
        
        script.Append("});\n");
        script.Append("</script>\n");
        
        // 註冊腳本
        cs.RegisterStartupScript(GetType(), "Init_" + ID, script.ToString());
    }
    catch (Exception ex)
    {
        Logger.Error("CalendarDate.RegisterClientScript Error", ex);
    }
}
```

## 8. 測試案例

### 8.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 日期格式 | 測試FormatDate方法 | 正確格式化日期 | 測試多種日期格式 |
| UT-002 | 日期解析 | 測試ParseDate方法 | 正確解析日期字串 | 測試多種格式輸入 |
| UT-003 | 日期驗證 | 測試ValidateDate方法 | 正確驗證日期有效性 | 測試範圍內外日期 |
| UT-004 | 會計期間檢查 | 測試CheckPeriodValidity方法 | 正確驗證會計期間 | 測試各種期間日期 |
| UT-005 | 日期計算 | 測試日期計算方法 | 計算結果正確 | 測試增減天數月數 |

### 8.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 表單整合 | 測試表單頁面整合 | 正確初始化和提交 | 實際表單頁面 |
| IT-002 | 多控制項協作 | 測試多個日期控制項 | 各控制項獨立運作 | 包含多個日期控制項 |
| IT-003 | 跨瀏覽器相容性 | 測試跨瀏覽器行為 | 在主流瀏覽器正常 | IE11, Chrome, Firefox |
| IT-004 | 伺服器端驗證 | 測試表單提交驗證 | 伺服器端正確驗證 | 提交含無效日期的表單 |
| IT-005 | 日期聯動測試 | 測試日期聯動行為 | 聯動邏輯正確執行 | 測試起訖日期聯動 |

### 8.3 UI測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UI-001 | 日曆彈出視窗 | 測試彈出視窗行為 | 正確顯示和隱藏 | 各種鍵盤鼠標操作 |
| UI-002 | 鍵盤操作 | 測試鍵盤日期選擇 | 支援鍵盤導航 | 測試方向鍵和Enter |
| UI-003 | 錯誤提示 | 測試錯誤提示顯示 | 錯誤提示清晰可見 | 各種錯誤情境 |
| UI-004 | 樣式一致性 | 測試與系統樣式整合 | 外觀與系統一致 | 檢查CSS樣式 |
| UI-005 | 響應式佈局 | 測試不同螢幕尺寸 | 正確調整寬度 | 桌機、平板尺寸 |

## 9. 安全性考量

### 9.1 輸入驗證

1. **格式驗證**
   - 嚴格檢查日期格式
   - 防止 SQL 注入攻擊
   - 過濾任何非法字符

2. **範圍檢查**
   - 限制日期在合理範圍內
   - 防止超出範圍的極端值
   - 歷史資料不可早於1900年

3. **跨站腳本防護**
   - 對所有輸出進行HTML編碼
   - 避免執行來自用戶的任何腳本
   - 使用 ASP.NET 內建的防XSS功能

### 9.2 資料安全

1. **資料驗證**
   - 同時在客戶端和伺服器端驗證
   - 不依賴純客戶端驗證
   - 資料傳輸前再次檢查格式

2. **例外處理**
   - 所有方法包含適當的例外處理
   - 不向用戶顯示底層錯誤
   - 維護錯誤日誌但不暴露系統細節

## 10. 效能優化

### 10.1 載入優化

1. **資源重用**
   - 共用 jQuery 和 jQuery UI 庫
   - 樣式表合併以減少請求
   - 使用CDN加速常用資源

2. **代碼優化**
   - 減少DOM操作次數
   - 使用延遲初始化技術
   - 避免不必要的資料庫查詢

3. **緩存策略**
   - 緩存系統參數設定
   - 避免重複查詢固定資料
   - 使用ViewState有效保存狀態

### 10.2 渲染優化

1. **遞延載入**
   - 延遲載入日曆資源
   - 初始只載入基本控制項
   - 按需加載完整日曆功能

2. **選擇性渲染**
   - 根據裝置能力調整功能
   - 在行動裝置使用本地日期選擇器
   - 針對特殊瀏覽器提供備用方案

### 10.3 使用者體驗優化

1. **即時反饋**
   - 日期選擇後立即反饋
   - 驗證錯誤即時提示
   - 輔助說明顯示在適當位置

2. **智能預設**
   - 智能猜測日期格式
   - 年份為兩位數時自動補全
   - 記住用戶常用選擇

## 11. 維護與擴展

### 11.1 配置管理

1. **系統參數**
   - 通過系統參數表配置預設行為
   - 支持公司級日期格式設定
   - 支持用戶個人化設定

2. **樣式設定**
   - 通過CSS自定義外觀
   - 支持主題切換
   - 控制項大小可配置

### 11.2 擴展考量

1. **國際化支持**
   - 多語言文字資源外部化
   - 支持不同地區日期格式
   - 考慮不同文化習慣(中文、英文日期)

2. **功能擴展**
   - 預留擴展接口
   - 支持關聯日期聯動
   - 預留自定義日期驗證擴展點

### 11.3 版本升級

1. **向後兼容**
   - 保持API一致性
   - 使用標記屬性支持舊版控制項
   - 提供遷移指南和工具

2. **功能更新**
   - 預留更現代UI更新的路徑
   - 考慮未來移動裝置適配
   - 支持HTML5日期輸入

## 12. 參考資料

### 12.1 設計文件

1. **UI設計規範**
   - 泛太總帳系統UI設計指南
   - 控制項設計標準
   - 表單輸入設計規範

2. **系統架構**
   - 系統整體架構文檔
   - ASP.NET控制項框架
   - 日期處理最佳實踐

### 12.2 參考實現

1. **框架參考**
   - jQuery UI Datepicker文檔
   - ASP.NET自定義控制項開發指南
   - .NET日期處理類庫

2. **UI/UX參考**
   - 常用系統日期選擇器
   - 考慮到會計系統特性的日期輸入
   - 通用表單元素設計模式

### 12.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 