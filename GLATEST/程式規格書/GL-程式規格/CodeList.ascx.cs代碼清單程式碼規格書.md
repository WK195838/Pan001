# CodeList.ascx.cs 代碼清單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CodeList.ascx.cs                      |
| 程式名稱     | 代碼清單程式碼                          |
| 檔案大小     | 7.5KB                                 |
| 行數        | ~250                                  |
| 功能簡述     | 代碼選擇後端邏輯                        |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

CodeList.ascx.cs 是代碼清單控制項的後端程式碼，負責處理代碼選擇、資料載入、事件觸發和資料處理。此程式碼實現了代碼清單的核心邏輯，支援各種代碼類型的動態載入和處理。主要功能包括：

1. 提供各種代碼類型的資料載入和綁定功能
2. 實現代碼的搜尋和過濾功能
3. 處理代碼選擇變更時的事件
4. 提供代碼與描述的對應關係
5. 支援從不同資料來源載入代碼
6. 實現代碼清單控制項的屬性和方法
7. 處理代碼選擇的驗證和錯誤處理
8. 提供與資料庫的交互功能

## 程式結構說明

CodeList.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 CodeList 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **代碼資料載入方法**：處理不同代碼類型的資料載入和綁定
6. **搜尋和過濾方法**：實現代碼搜尋和過濾功能
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
using System.Data.SqlClient;
```

### 類別宣告與成員變數

```csharp
public partial class CodeList : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler CodeChanged;
    
    // 內部變數
    private string _codeType = string.Empty;
    private string _selectedCode = string.Empty;
    private string _selectedDescription = string.Empty;
    private bool _required = false;
    private bool _searchEnabled = true;
    private string _labelText = "代碼";
    private int _labelWidth = 80;
    private int _dropDownWidth = 150;
    
    // 資料相關變數
    private DataTable _codeDataTable = null;
}
```

### 屬性定義

```csharp
// 代碼類型
public string CodeType
{
    get { return _codeType; }
    set 
    { 
        _codeType = value;
        if (!IsPostBack)
        {
            BindCode(_codeType);
        }
    }
}

// 選取的代碼
public string SelectedCode
{
    get { return _selectedCode; }
    set 
    { 
        _selectedCode = value;
        SelectByCode(value);
    }
}

// 選取的代碼描述
public string SelectedDescription
{
    get { return _selectedDescription; }
}

// 是否必填
public bool Required
{
    get { return _required; }
    set { _required = value; }
}

// 是否啟用搜尋
public bool SearchEnabled
{
    get { return _searchEnabled; }
    set 
    { 
        _searchEnabled = value;
        txtSearch.Visible = value;
    }
}

// 標籤文字
public string LabelText
{
    get { return _labelText; }
    set 
    { 
        _labelText = value;
        lblCode.Text = value;
    }
}

// 標籤寬度
public int LabelWidth
{
    get { return _labelWidth; }
    set 
    { 
        _labelWidth = value;
        lblCode.Width = value;
    }
}

// 下拉選單寬度
public int DropDownWidth
{
    get { return _dropDownWidth; }
    set 
    { 
        _dropDownWidth = value;
        ddlCode.Width = value;
    }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return ddlCode.Enabled; }
    set 
    {
        ddlCode.Enabled = value;
        txtSearch.Enabled = value;
        btnCodeList.Enabled = value;
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
    lblCode.Text = _labelText;
    lblCode.Width = _labelWidth;
    
    // 設定下拉選單寬度
    ddlCode.Width = _dropDownWidth;
    
    // 設定搜尋框可見性
    txtSearch.Visible = _searchEnabled;
    
    // 如果有代碼類型，則綁定代碼資料
    if (!string.IsNullOrEmpty(_codeType))
    {
        BindCode(_codeType);
    }
}
```

#### 代碼綁定方法

```csharp
// 綁定代碼資料
public void BindCode(string codeType, string defaultCode = "")
{
    _codeType = codeType;
    
    // 清除現有項目
    ddlCode.Items.Clear();
    
    // 加入空白選項
    ddlCode.Items.Add(new ListItem("", ""));
    
    try
    {
        // 從資料庫載入代碼資料
        _codeDataTable = GetCodeDataFromDB(codeType);
        
        if (_codeDataTable != null && _codeDataTable.Rows.Count > 0)
        {
            // 綁定代碼資料到下拉選單
            foreach (DataRow row in _codeDataTable.Rows)
            {
                string code = row["CodeValue"].ToString();
                string description = row["CodeDesc"].ToString();
                string displayText = string.Format("{0} - {1}", code, description);
                
                ListItem item = new ListItem(displayText, code);
                ddlCode.Items.Add(item);
            }
        }
        
        // 如果有預設代碼，則選取該代碼
        if (!string.IsNullOrEmpty(defaultCode))
        {
            SelectByCode(defaultCode);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("BindCode Error: " + ex.Message);
    }
}

// 從指定資料來源綁定代碼資料
public void BindCodeFromDataSource(DataTable dataSource, string valueField, string textField, string defaultCode = "")
{
    // 清除現有項目
    ddlCode.Items.Clear();
    
    // 加入空白選項
    ddlCode.Items.Add(new ListItem("", ""));
    
    try
    {
        _codeDataTable = dataSource;
        
        if (dataSource != null && dataSource.Rows.Count > 0)
        {
            // 綁定資料到下拉選單
            foreach (DataRow row in dataSource.Rows)
            {
                string code = row[valueField].ToString();
                string description = row[textField].ToString();
                string displayText = string.Format("{0} - {1}", code, description);
                
                ListItem item = new ListItem(displayText, code);
                ddlCode.Items.Add(item);
            }
        }
        
        // 如果有預設代碼，則選取該代碼
        if (!string.IsNullOrEmpty(defaultCode))
        {
            SelectByCode(defaultCode);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("BindCodeFromDataSource Error: " + ex.Message);
    }
}
```

#### 代碼選擇方法

```csharp
// 根據代碼選取項目
public void SelectByCode(string code)
{
    if (ddlCode.Items.FindByValue(code) != null)
    {
        ddlCode.SelectedValue = code;
        _selectedCode = code;
        _selectedDescription = GetCodeDescription(code);
    }
    else
    {
        // 如果找不到代碼，則選取第一個項目
        if (ddlCode.Items.Count > 0)
        {
            ddlCode.SelectedIndex = 0;
            _selectedCode = ddlCode.SelectedValue;
            _selectedDescription = GetCodeDescription(_selectedCode);
        }
    }
}

// 根據索引選取項目
public void SelectByIndex(int index)
{
    if (index >= 0 && index < ddlCode.Items.Count)
    {
        ddlCode.SelectedIndex = index;
        _selectedCode = ddlCode.SelectedValue;
        _selectedDescription = GetCodeDescription(_selectedCode);
    }
}

// 清除選擇
public void ClearSelection()
{
    if (ddlCode.Items.Count > 0)
    {
        ddlCode.SelectedIndex = 0;
        _selectedCode = ddlCode.SelectedValue;
        _selectedDescription = GetCodeDescription(_selectedCode);
    }
}

// 取得代碼描述
public string GetCodeDescription(string code)
{
    if (_codeDataTable != null && _codeDataTable.Rows.Count > 0)
    {
        // 在代碼資料表中尋找對應的描述
        DataRow[] rows = _codeDataTable.Select(string.Format("CodeValue = '{0}'", code));
        if (rows.Length > 0)
        {
            return rows[0]["CodeDesc"].ToString();
        }
    }
    
    return string.Empty;
}
```

#### 資料庫相關方法

```csharp
// 從資料庫取得代碼資料
private DataTable GetCodeDataFromDB(string codeType)
{
    DataTable dt = new DataTable();
    
    try
    {
        // 建立資料庫連接和命令
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GLAConnectionString"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("sp_GetCodeList", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodeType", codeType);
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("GetCodeDataFromDB Error: " + ex.Message);
    }
    
    return dt;
}

// 重新載入代碼清單
public void ReloadCodeList(string codeType = "")
{
    // 如果未指定代碼類型，則使用目前的代碼類型
    if (string.IsNullOrEmpty(codeType))
    {
        codeType = _codeType;
    }
    
    // 記住目前選取的代碼
    string currentCode = _selectedCode;
    
    // 重新綁定代碼資料
    BindCode(codeType, currentCode);
}
```

#### 搜尋和過濾方法

```csharp
// 處理搜尋框文字變更事件
protected void txtSearch_TextChanged(object sender, EventArgs e)
{
    // 取得搜尋文字
    string searchText = txtSearch.Text.Trim();
    
    // 如果搜尋文字為空，則顯示所有代碼
    if (string.IsNullOrEmpty(searchText))
    {
        ReloadCodeList();
        return;
    }
    
    // 過濾代碼清單
    FilterCodeList(searchText);
}

// 過濾代碼清單
private void FilterCodeList(string searchText)
{
    // 如果沒有代碼資料，則返回
    if (_codeDataTable == null || _codeDataTable.Rows.Count == 0)
        return;
    
    // 清除現有項目
    ddlCode.Items.Clear();
    
    // 加入空白選項
    ddlCode.Items.Add(new ListItem("", ""));
    
    // 過濾符合搜尋條件的代碼
    foreach (DataRow row in _codeDataTable.Rows)
    {
        string code = row["CodeValue"].ToString();
        string description = row["CodeDesc"].ToString();
        
        // 檢查代碼或描述是否符合搜尋條件
        if (code.ToLower().Contains(searchText.ToLower()) || 
            description.ToLower().Contains(searchText.ToLower()))
        {
            string displayText = string.Format("{0} - {1}", code, description);
            ListItem item = new ListItem(displayText, code);
            ddlCode.Items.Add(item);
        }
    }
    
    // 如果有過濾結果，則選取第一個項目
    if (ddlCode.Items.Count > 1)
    {
        ddlCode.SelectedIndex = 1;
        _selectedCode = ddlCode.SelectedValue;
        _selectedDescription = GetCodeDescription(_selectedCode);
    }
    else
    {
        ddlCode.SelectedIndex = 0;
        _selectedCode = ddlCode.SelectedValue;
        _selectedDescription = GetCodeDescription(_selectedCode);
    }
    
    // 觸發代碼變更事件
    OnCodeChanged(EventArgs.Empty);
}
```

#### 事件處理方法

```csharp
// 處理下拉選單選項變更事件
protected void ddlCode_SelectedIndexChanged(object sender, EventArgs e)
{
    // 更新選取的代碼和描述
    _selectedCode = ddlCode.SelectedValue;
    _selectedDescription = GetCodeDescription(_selectedCode);
    
    // 觸發代碼變更事件
    OnCodeChanged(EventArgs.Empty);
}

// 處理代碼清單按鈕點擊事件
protected void btnCodeList_Click(object sender, EventArgs e)
{
    // 顯示代碼清單彈出視窗
    string script = string.Format("ShowCodeListPopup('{0}', '{1}');", _codeType, ClientID);
    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowCodeListPopup_" + ClientID, script, true);
}

// 觸發代碼變更事件
protected virtual void OnCodeChanged(EventArgs e)
{
    if (CodeChanged != null)
        CodeChanged(this, e);
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
    
    // 如果是必填項，則檢查是否有選取代碼
    if (string.IsNullOrEmpty(_selectedCode))
    {
        // 顯示錯誤訊息
        string errorMessage = string.Format("請選擇{0}。", _labelText);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ValidationError_" + ClientID, 
            string.Format("alert('{0}');", errorMessage), true);
        
        return false;
    }
    
    return true;
}
```

## 技術實現

CodeList.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **資料存取**：使用 ADO.NET 與資料庫互動
5. **資料繫結**：使用資料繫結技術填充下拉式選單
6. **用戶端腳本整合**：使用 JavaScript 實現進階功能

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| CodeType | string | 獲取或設定代碼類型 |
| SelectedCode | string | 獲取或設定選取的代碼值 |
| SelectedDescription | string | 獲取選取代碼的描述（唯讀） |
| Required | bool | 獲取或設定代碼選擇是否為必填 |
| SearchEnabled | bool | 獲取或設定是否啟用搜尋功能 |
| LabelText | string | 獲取或設定標籤文字 |
| LabelWidth | int | 獲取或設定標籤寬度 |
| DropDownWidth | int | 獲取或設定下拉選單寬度 |
| Enabled | bool | 獲取或設定控制項是否啟用 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindCode | codeType, defaultCode (可選) | void | 綁定指定類型的代碼資料 |
| BindCodeFromDataSource | dataSource, valueField, textField, defaultCode (可選) | void | 從指定資料來源綁定代碼資料 |
| SelectByCode | code | void | 根據代碼值選取項目 |
| SelectByIndex | index | void | 根據索引選取項目 |
| ClearSelection | 無 | void | 清除目前選取的代碼 |
| GetCodeDescription | code | string | 取得指定代碼的描述 |
| ReloadCodeList | codeType (可選) | void | 重新載入代碼清單 |
| Validate | 無 | bool | 驗證控制項是否符合必填要求 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| CodeChanged | EventArgs | 代碼值變更時觸發的事件 |

## 程式碼分析

### 強項

1. **介面設計的彈性**：
   - 提供多種自訂選項，如標籤文字、寬度等
   - 支援不同來源的代碼資料載入
   - 搜尋功能可依需求啟用或停用

2. **資料處理的完整性**：
   - 完整處理代碼與描述的對應關係
   - 支援從資料庫或其他資料來源載入代碼
   - 實現代碼資料的過濾和搜尋功能

3. **事件處理的靈活性**：
   - 提供代碼變更事件供外部訂閱
   - 妥善處理各種用戶交互事件
   - 彈出視窗的整合實現較為完善

### 待改進點

1. **效能優化**：
   - 大量代碼資料的處理可能需要優化
   - 搜尋和過濾邏輯可以改進，以提高效率

2. **錯誤處理**：
   - 資料庫連接和查詢錯誤的處理可以更完善
   - 使用更多的參數驗證和錯誤提示

3. **用戶體驗**：
   - 可以增加更多的視覺反饋和提示
   - 搜尋功能可以實現更智能的匹配和排序

## 安全性考量

1. **SQL 注入防護**：
   - 使用參數化查詢和存儲過程，防止 SQL 注入攻擊
   - 對輸入的代碼類型和代碼值進行適當驗證

2. **資料驗證**：
   - 對所有輸入和選取的值進行驗證
   - 處理可能的無效輸入和特殊字符

3. **錯誤資訊處理**：
   - 避免向用戶顯示詳細的錯誤訊息
   - 使用適當的錯誤記錄機制

4. **權限控制**：
   - 根據用戶權限顯示或隱藏特定代碼
   - 確保敏感代碼資訊的安全處理

## 效能考量

1. **資料載入優化**：
   - 使用快取機制減少資料庫查詢
   - 實現延遲載入，僅在需要時才載入代碼資料

2. **資源使用**：
   - 妥善處理資料庫連接，確保及時釋放資源
   - 適當使用資料結構，減少記憶體使用

3. **用戶端效能**：
   - 使用 AJAX 實現非同步資料載入
   - 最小化回傳次數，提高響應速度

4. **搜尋效能**：
   - 實現高效的字串匹配和過濾算法
   - 考慮使用索引和排序優化搜尋效率

## 待改進事項

1. 實現多選功能，允許同時選擇多個代碼
2. 改進搜尋算法，提供更準確的模糊匹配
3. 增加代碼資料的快取機制，提高效能
4. 實現分頁顯示大量代碼的功能
5. 增加代碼資料的自動更新機制
6. 提供更多的自訂選項，如顯示格式、排序方式等
7. 增加與其他控制項的整合功能
8. 實現階層式代碼顯示功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/6  | 1.0    | 首次建立代碼清單程式碼規格書          | Claude AI  | 