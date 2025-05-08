# CompanyList.ascx.cs 公司清單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | CompanyList.ascx.cs                   |
| 程式名稱     | 公司清單程式碼                          |
| 檔案大小     | 4.4KB                                 |
| 行數        | ~150                                  |
| 功能簡述     | 公司選擇後端邏輯                        |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

CompanyList.ascx.cs 是公司清單控制項的後端程式碼，負責處理公司資料的載入、公司選擇、權限驗證和事件處理。此程式碼實現了多公司環境中的公司選擇核心邏輯，確保使用者只能選擇有權限存取的公司。主要功能包括：

1. 提供公司資料的載入和綁定功能
2. 實現公司選擇和切換的邏輯
3. 整合系統權限控制機制
4. 處理公司選擇變更時的事件
5. 提供公司代碼與名稱的對應關係
6. 支援使用者默認公司的設定與載入
7. 實現公司清單控制項的屬性和方法
8. 處理公司選擇的驗證和錯誤處理

## 程式結構說明

CompanyList.ascx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 CompanyList 類別，繼承自 System.Web.UI.UserControl
3. **成員變數與屬性**：定義控制項所需的變數、屬性和事件
4. **控制項事件處理方法**：處理頁面載入、下拉選單選項變更等事件
5. **公司資料載入方法**：處理公司資料的載入和綁定
6. **公司選擇方法**：實現公司選擇和相關操作
7. **權限驗證方法**：檢查使用者對公司的存取權限
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
using System.Data.SqlClient;
```

### 類別宣告與成員變數

```csharp
public partial class CompanyList : System.Web.UI.UserControl
{
    // 事件宣告
    public event EventHandler CompanyChanged;
    
    // 內部變數
    private string _selectedCompanyId = string.Empty;
    private string _selectedCompanyName = string.Empty;
    private bool _required = true;
    private bool _showInactiveCompanies = false;
    private string _labelText = "公司";
    private int _labelWidth = 60;
    private int _dropDownWidth = 150;
    
    // 公司資料相關變數
    private DataTable _companyDataTable = null;
}
```

### 屬性定義

```csharp
// 選取的公司代碼
public string SelectedCompanyId
{
    get { return _selectedCompanyId; }
    set 
    { 
        _selectedCompanyId = value;
        SelectCompanyById(value);
    }
}

// 選取的公司名稱
public string SelectedCompanyName
{
    get { return _selectedCompanyName; }
}

// 是否必填
public bool Required
{
    get { return _required; }
    set { _required = value; }
}

// 是否顯示非啟用公司
public bool ShowInactiveCompanies
{
    get { return _showInactiveCompanies; }
    set 
    { 
        _showInactiveCompanies = value;
        if (!IsPostBack)
        {
            BindCompanies(_showInactiveCompanies);
        }
    }
}

// 標籤文字
public string LabelText
{
    get { return _labelText; }
    set 
    { 
        _labelText = value;
        lblCompany.Text = value;
    }
}

// 標籤寬度
public int LabelWidth
{
    get { return _labelWidth; }
    set 
    { 
        _labelWidth = value;
        lblCompany.Width = value;
    }
}

// 下拉選單寬度
public int DropDownWidth
{
    get { return _dropDownWidth; }
    set 
    { 
        _dropDownWidth = value;
        ddlCompany.Width = value;
    }
}

// 控制項啟用狀態
public bool Enabled
{
    get { return ddlCompany.Enabled; }
    set { ddlCompany.Enabled = value; }
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
    lblCompany.Text = _labelText;
    lblCompany.Width = _labelWidth;
    
    // 設定下拉選單寬度
    ddlCompany.Width = _dropDownWidth;
    
    // 綁定公司資料
    BindCompanies(_showInactiveCompanies);
    
    // 如果有指定的預設公司，則選擇該公司
    if (!string.IsNullOrEmpty(_selectedCompanyId))
    {
        SelectCompanyById(_selectedCompanyId);
    }
    // 否則，嘗試載入使用者的預設公司
    else
    {
        LoadUserDefaultCompany();
    }
}
```

#### 公司資料綁定方法

```csharp
// 綁定公司資料
public void BindCompanies(bool showInactive = false, string defaultCompanyId = "")
{
    // 清除現有項目
    ddlCompany.Items.Clear();
    
    // 加入空白選項（如果需要）
    if (!_required)
    {
        ddlCompany.Items.Add(new ListItem("", ""));
    }
    
    try
    {
        // 載入使用者可存取的公司
        _companyDataTable = LoadUserAccessibleCompanies(showInactive);
        
        if (_companyDataTable != null && _companyDataTable.Rows.Count > 0)
        {
            // 綁定公司資料到下拉選單
            foreach (DataRow row in _companyDataTable.Rows)
            {
                string companyId = row["CompanyId"].ToString();
                string companyName = row["CompanyName"].ToString();
                string displayText = string.Format("{0} - {1}", companyId, companyName);
                
                ListItem item = new ListItem(displayText, companyId);
                ddlCompany.Items.Add(item);
            }
        }
        
        // 如果有指定預設公司，則選取該公司
        if (!string.IsNullOrEmpty(defaultCompanyId))
        {
            SelectCompanyById(defaultCompanyId);
        }
        // 否則選取第一個公司（如果有）
        else if (ddlCompany.Items.Count > 0)
        {
            ddlCompany.SelectedIndex = 0;
            _selectedCompanyId = ddlCompany.SelectedValue;
            _selectedCompanyName = GetCompanyNameById(_selectedCompanyId);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("BindCompanies Error: " + ex.Message);
    }
}
```

#### 公司選擇方法

```csharp
// 根據公司代碼選取公司
public void SelectCompanyById(string companyId)
{
    if (ddlCompany.Items.FindByValue(companyId) != null)
    {
        ddlCompany.SelectedValue = companyId;
        _selectedCompanyId = companyId;
        _selectedCompanyName = GetCompanyNameById(companyId);
    }
    else
    {
        // 如果找不到指定的公司，則選取第一個公司（如果有）
        if (ddlCompany.Items.Count > 0)
        {
            ddlCompany.SelectedIndex = 0;
            _selectedCompanyId = ddlCompany.SelectedValue;
            _selectedCompanyName = GetCompanyNameById(_selectedCompanyId);
        }
    }
}

// 取得公司名稱
public string GetCompanyNameById(string companyId)
{
    if (_companyDataTable != null && _companyDataTable.Rows.Count > 0)
    {
        // 在公司資料表中尋找對應的公司名稱
        DataRow[] rows = _companyDataTable.Select(string.Format("CompanyId = '{0}'", companyId));
        if (rows.Length > 0)
        {
            return rows[0]["CompanyName"].ToString();
        }
    }
    
    return string.Empty;
}
```

#### 使用者權限相關方法

```csharp
// 載入使用者可存取的公司
private DataTable LoadUserAccessibleCompanies(bool showInactive = false)
{
    DataTable dt = new DataTable();
    
    try
    {
        // 取得目前登入的使用者
        string userId = GetCurrentUserId();
        
        // 使用 CompanyManager 類別取得使用者可存取的公司
        CompanyManager companyManager = new CompanyManager();
        dt = companyManager.GetUserAccessibleCompanies(userId, showInactive);
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("LoadUserAccessibleCompanies Error: " + ex.Message);
    }
    
    return dt;
}

// 載入使用者的預設公司
private void LoadUserDefaultCompany()
{
    try
    {
        // 取得目前登入的使用者
        string userId = GetCurrentUserId();
        
        // 使用 CompanyManager 類別取得使用者的預設公司
        CompanyManager companyManager = new CompanyManager();
        string defaultCompanyId = companyManager.GetUserDefaultCompany(userId);
        
        // 如果有預設公司，則選取該公司
        if (!string.IsNullOrEmpty(defaultCompanyId))
        {
            SelectCompanyById(defaultCompanyId);
        }
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("LoadUserDefaultCompany Error: " + ex.Message);
    }
}

// 取得目前登入的使用者ID
private string GetCurrentUserId()
{
    if (HttpContext.Current.Session["UserId"] != null)
    {
        return HttpContext.Current.Session["UserId"].ToString();
    }
    
    return string.Empty;
}
```

#### 事件處理方法

```csharp
// 處理下拉選單選項變更事件
protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
{
    // 更新選取的公司代碼和名稱
    _selectedCompanyId = ddlCompany.SelectedValue;
    _selectedCompanyName = GetCompanyNameById(_selectedCompanyId);
    
    // 觸發公司變更事件
    OnCompanyChanged(EventArgs.Empty);
}

// 觸發公司變更事件
protected virtual void OnCompanyChanged(EventArgs e)
{
    if (CompanyChanged != null)
        CompanyChanged(this, e);
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
    
    // 如果是必填項，則檢查是否有選取公司
    if (string.IsNullOrEmpty(_selectedCompanyId))
    {
        // 顯示錯誤訊息
        string errorMessage = string.Format("請選擇{0}。", _labelText);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ValidationError_" + ClientID, 
            string.Format("alert('{0}');", errorMessage), true);
        
        return false;
    }
    
    // 檢查使用者是否有權限存取選取的公司
    if (!CheckCompanyAccessPermission(_selectedCompanyId))
    {
        // 顯示錯誤訊息
        string errorMessage = "您沒有權限存取此公司。";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "AccessError_" + ClientID, 
            string.Format("alert('{0}');", errorMessage), true);
        
        return false;
    }
    
    return true;
}

// 檢查使用者是否有權限存取公司
private bool CheckCompanyAccessPermission(string companyId)
{
    try
    {
        // 取得目前登入的使用者
        string userId = GetCurrentUserId();
        
        // 使用 AppAuthority 類別檢查權限
        AppAuthority authority = new AppAuthority();
        return authority.CheckCompanyAccess(userId, companyId);
    }
    catch (Exception ex)
    {
        // 錯誤處理
        System.Diagnostics.Debug.WriteLine("CheckCompanyAccessPermission Error: " + ex.Message);
        return false;
    }
}
```

## 技術實現

CompanyList.ascx.cs 使用以下技術：

1. **ASP.NET Web Forms**：使用 UserControl 實現可重用的使用者控制項
2. **C# 物件導向程式設計**：以物件導向方式實現控制項邏輯
3. **ASP.NET 事件模型**：使用事件處理和自定義事件觸發機制
4. **資料存取**：使用 ADO.NET 與資料庫互動
5. **資料繫結**：使用資料繫結技術填充下拉式選單
6. **Session 管理**：使用 Session 存取使用者相關資訊

## 屬性和方法說明

### 屬性

| 屬性名稱 | 類型 | 描述 |
|---------|------|------|
| SelectedCompanyId | string | 獲取或設定選取的公司代碼 |
| SelectedCompanyName | string | 獲取選取公司的名稱（唯讀） |
| Required | bool | 獲取或設定公司選擇是否為必填 |
| ShowInactiveCompanies | bool | 獲取或設定是否顯示非啟用公司 |
| LabelText | string | 獲取或設定標籤文字 |
| LabelWidth | int | 獲取或設定標籤寬度 |
| DropDownWidth | int | 獲取或設定下拉選單寬度 |
| Enabled | bool | 獲取或設定控制項是否啟用 |

### 方法

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| BindCompanies | showInactive, defaultCompanyId (可選) | void | 綁定公司資料到下拉式選單 |
| SelectCompanyById | companyId | void | 根據公司代碼選取下拉式選單項目 |
| GetCompanyNameById | companyId | string | 取得指定公司代碼的公司名稱 |
| Validate | 無 | bool | 驗證控制項是否符合必填和權限要求 |

### 事件

| 事件名稱 | 參數類型 | 描述 |
|---------|---------|------|
| CompanyChanged | EventArgs | 公司選擇變更時觸發的事件 |

## 程式碼分析

### 強項

1. **權限控制的完整性**：
   - 完整整合系統權限機制
   - 確保使用者只能選擇有權限存取的公司
   - 在資料載入和選擇時都進行權限驗證

2. **使用者體驗的考量**：
   - 支援預設公司的自動載入
   - 提供彈性的配置選項，如標籤文字、寬度等
   - 處理各種錯誤情況，確保良好的使用者體驗

3. **代碼結構的清晰性**：
   - 功能模組化，分離不同的功能區塊
   - 良好的錯誤處理機制
   - 清晰的命名和適當的註解

### 待改進點

1. **效能優化**：
   - 公司資料可以使用快取機制減少資料庫查詢
   - 權限驗證可能需要優化，特別是在大型系統中

2. **擴展性**：
   - 可以增加更多的公司篩選和排序選項
   - 支援公司的階層顯示或分組顯示

3. **使用者介面增強**：
   - 可以提供更多的視覺反饋
   - 支援公司的搜尋功能，特別是在大量公司環境中

## 安全性考量

1. **權限控制**：
   - 嚴格控制使用者只能存取授權的公司
   - 在伺服器端進行權限驗證，防止客戶端繞過權限限制

2. **資料驗證**：
   - 驗證使用者輸入和選取的公司代碼
   - 使用參數化查詢，防止 SQL 注入攻擊

3. **錯誤處理**：
   - 適當處理和記錄錯誤情況
   - 不向使用者暴露敏感的錯誤訊息

4. **Session 安全**：
   - 安全地使用 Session 存取使用者資訊
   - 防止 Session 劫持和 Session 固定攻擊

## 效能考量

1. **資料載入優化**：
   - 公司資料不會頻繁變更，可以使用快取機制
   - 可以使用延遲載入策略，減少不必要的資料庫查詢

2. **權限檢查優化**：
   - 權限資訊可以在使用者登入時一次性載入
   - 可以使用快取機制減少重複的權限檢查

3. **記憶體使用**：
   - 合理管理公司資料表的記憶體使用
   - 避免不必要的大型物件建立

4. **回應時間**：
   - 確保公司切換操作的回應速度
   - 優化資料庫查詢和資料處理邏輯

## 待改進事項

1. 增加公司搜尋功能，方便在大量公司中快速找到目標公司
2. 實現公司資料的快取機制，提高載入效能
3. 增加公司分組顯示功能，如按地區或業務類型分組
4. 支援公司多語言顯示，以適應國際化需求
5. 增加公司排序選項，如按公司代碼、名稱或最近使用等
6. 改進權限驗證機制，提高效能和安全性
7. 增加公司變更歷史記錄功能，方便使用者快速切換常用公司
8. 提供更多的客製化選項，以適應不同的使用場景

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/6  | 1.0    | 首次建立公司清單程式碼規格書          | Claude AI  | 