# Subjects.ascx.cs 科目控制項程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | Subjects.ascx.cs                       |
| 程式名稱     | 科目控制項程式碼                          |
| 檔案大小     | 13KB                                  |
| 行數        | ~250                                  |
| 功能簡述     | 科目選擇後端邏輯                         |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/11                             |

## 程式功能概述

Subjects.ascx.cs 是泛太總帳系統中科目控制項的後端程式碼實現，負責處理科目選擇功能的核心邏輯。該程式碼實現了科目資料的查詢、驗證、選擇和處理等功能，支援會計系統中科目相關操作的各種需求。主要功能包括：

1. 實現科目代碼和名稱的搜尋和驗證邏輯
2. 處理科目層級結構的載入和管理
3. 提供科目有效性檢查和權限驗證功能
4. 處理科目資料的過濾、排序和分頁
5. 實現科目選擇和變更的事件處理機制
6. 整合科目相關的Web服務和資料存取
7. 提供科目歷史記錄和常用科目管理功能
8. 支援自動完成和建議功能的後端邏輯
9. 實現跨頁面科目資料傳遞和保存機制
10. 處理科目相關的異常和錯誤情況

## 程式架構說明

Subjects.ascx.cs 的程式架構包含以下主要部分：

1. **命名空間和引用**：引入必要的系統庫和自定義元件
2. **類別定義**：定義科目控制項類別及其繼承關係
3. **屬性區域**：定義控制項的各種屬性和設定選項
4. **事件定義**：定義控制項支援的各種事件
5. **生命週期方法**：包含頁面載入、初始化等方法
6. **科目處理方法**：實現科目查詢、驗證和選擇功能
7. **資料存取方法**：處理科目資料的讀取和保存
8. **事件處理方法**：處理各種控制項的事件響應
9. **輔助方法**：提供各種工具方法和資料處理功能
10. **私有方法**：實現內部邏輯和輔助功能

## 技術實現

Subjects.ascx.cs 使用以下技術實現功能：

1. **ASP.NET 伺服器控制項**：作為Web頁面的後端處理元件
2. **ADO.NET 資料存取**：實現科目資料的查詢和處理
3. **LINQ 查詢**：提供高效的科目資料查詢和處理
4. **Web 服務整合**：支援科目相關的Web服務呼叫
5. **事件委派模式**：提供豐富的事件處理機制
6. **反射技術**：用於動態處理科目資料和屬性
7. **AJAX 整合**：實現無刷新的科目資料更新
8. **物件導向設計**：採用封裝、繼承、多態等設計原則

## 依賴關係

Subjects.ascx.cs 依賴以下元件和檔案：

1. Subjects.ascx：科目控制項的UI定義檔案
2. System.Web.UI：ASP.NET UI控制項基礎庫
3. System.Data：資料處理和資料庫交互庫
4. System.Linq：LINQ查詢支援
5. WSGLAcctDefJson：科目定義Web服務
6. WSAutoComplete：自動完成Web服務
7. pagefunction.js：前端互動JavaScript檔案

## 主要類別和方法

### 類別定義

```csharp
public partial class Subjects : System.Web.UI.UserControl
{
    // 類別實現
}
```

### 主要屬性

```csharp
// 取得或設定所選科目代碼
public string SelectedSubjectCode { get; set; }

// 取得所選科目名稱
public string SelectedSubjectName { get; }

// 取得所選科目類型
public string SelectedSubjectType { get; }

// 設定科目篩選條件
public string SubjectFilter { get; set; }

// 設定允許選擇的科目類型
public string[] AllowedSubjectTypes { get; set; }

// 指定科目是否為必填
public bool RequiredSubject { get; set; }

// 啟用或禁用自動完成功能
public bool EnableAutoComplete { get; set; }

// 設定控制項是否為唯讀模式
public bool ReadOnly { get; set; }

// 是否顯示科目詳細資訊
public bool DisplaySubjectInfo { get; set; }

// 設定科目所屬公司識別碼
public string CompanyId { get; set; }
```

### 主要事件

```csharp
// 使用者選擇科目時觸發
public event EventHandler<SubjectEventArgs> SubjectSelected;

// 所選科目變更時觸發
public event EventHandler<SubjectChangedEventArgs> SubjectChanged;

// 科目驗證完成時觸發
public event EventHandler<SubjectValidatedEventArgs> SubjectValidated;

// 完成科目搜尋操作時觸發
public event EventHandler<SubjectSearchEventArgs> SubjectSearched;
```

### 主要方法

```csharp
// 頁面生命週期方法
protected void Page_Load(object sender, EventArgs e)
protected void Page_Init(object sender, EventArgs e)

// 科目處理方法
public bool SetSubject(string subjectCode)
public void ClearSubject()
public bool ValidateSubject()
public int SearchSubjects(string searchText, string searchType)
public DataRow GetSubjectDetail(string subjectCode)
public bool IsSubjectActive(string subjectCode)
public DataTable GetRecentSubjects(int count)
public DataTable GetCommonSubjects(int count)

// 事件處理方法
protected void btnSearch_Click(object sender, EventArgs e)
protected void btnAdvanced_Click(object sender, EventArgs e)
protected void btnClear_Click(object sender, EventArgs e)
protected void btnViewDetail_Click(object sender, EventArgs e)
protected void gvSubjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
protected void cvSubjectCode_ServerValidate(object source, ServerValidateEventArgs args)

// 輔助方法
private bool LoadSubjectByCode(string subjectCode)
private void UpdateSubjectDisplay()
private bool CheckSubjectPermissions(string subjectCode)
private void SaveRecentSubject(string subjectCode)
```

## 方法詳細說明

### 生命週期方法

#### Page_Load

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    // 初始化控制項
    if (!IsPostBack)
    {
        // 設定初始狀態
        InitializeControl();
        
        // 設定自動完成功能
        SetupAutoComplete();
        
        // 載入回傳的科目（如果有）
        LoadPostBackSubject();
    }
    
    // 設定控制項屬性
    SetupControlProperties();
}
```

此方法在頁面載入時執行，初始化控制項狀態，設定自動完成功能，並處理回傳資料。

#### Page_Init

```csharp
protected void Page_Init(object sender, EventArgs e)
{
    // 註冊用戶端腳本
    RegisterClientScripts();
    
    // 設定驗證控制項
    ConfigureValidators();
}
```

此方法在頁面初始化階段執行，註冊必要的客戶端腳本並設定驗證控制項。

### 科目處理方法

#### SetSubject

```csharp
public bool SetSubject(string subjectCode)
{
    // 檢查科目代碼是否為空
    if (string.IsNullOrEmpty(subjectCode))
    {
        ClearSubject();
        return false;
    }
    
    // 載入科目資料
    bool success = LoadSubjectByCode(subjectCode);
    
    if (success)
    {
        // 更新控制項顯示
        txtSubjectCode.Text = subjectCode;
        UpdateSubjectDisplay();
        
        // 保存科目到最近使用列表
        SaveRecentSubject(subjectCode);
        
        // 觸發選擇事件
        OnSubjectSelected();
    }
    
    return success;
}
```

此方法設定選擇的科目，載入科目資料並更新控制項顯示。

#### ValidateSubject

```csharp
public bool ValidateSubject()
{
    // 獲取當前科目代碼
    string subjectCode = txtSubjectCode.Text.Trim();
    
    // 如果不是必填且為空，則視為有效
    if (!RequiredSubject && string.IsNullOrEmpty(subjectCode))
    {
        return true;
    }
    
    // 檢查科目代碼是否為空
    if (string.IsNullOrEmpty(subjectCode))
    {
        ShowValidationError("科目代碼為必填欄位");
        return false;
    }
    
    // 檢查科目是否存在
    if (!IsSubjectExists(subjectCode))
    {
        ShowValidationError("科目代碼不存在");
        return false;
    }
    
    // 檢查科目是否有效
    if (!IsSubjectActive(subjectCode))
    {
        ShowValidationError("科目已停用");
        return false;
    }
    
    // 檢查科目類型是否允許
    if (!IsSubjectTypeAllowed(subjectCode))
    {
        ShowValidationError("科目類型不符合要求");
        return false;
    }
    
    // 檢查科目權限
    if (!CheckSubjectPermissions(subjectCode))
    {
        ShowValidationError("無權使用此科目");
        return false;
    }
    
    // 觸發驗證事件
    OnSubjectValidated(true);
    
    return true;
}
```

此方法驗證科目的有效性，檢查科目是否存在、啟用、符合類型要求，以及使用者是否有權限。

#### SearchSubjects

```csharp
public int SearchSubjects(string searchText, string searchType)
{
    // 準備搜尋參數
    Dictionary<string, object> parameters = new Dictionary<string, object>();
    parameters.Add("CompanyId", CompanyId);
    parameters.Add("SearchText", searchText);
    parameters.Add("SearchType", searchType);
    
    // 添加篩選條件
    if (!string.IsNullOrEmpty(SubjectFilter))
    {
        parameters.Add("Filter", SubjectFilter);
    }
    
    // 添加科目類型限制
    if (AllowedSubjectTypes != null && AllowedSubjectTypes.Length > 0)
    {
        parameters.Add("SubjectTypes", string.Join(",", AllowedSubjectTypes));
    }
    
    try
    {
        // 呼叫Web服務執行搜尋
        DataTable results = WSGLAcctDefJson.SearchSubjects(parameters);
        
        // 綁定搜尋結果
        gvSubjects.DataSource = results;
        gvSubjects.DataBind();
        
        // 顯示搜尋結果面板
        pnlSearchResults.Visible = results.Rows.Count > 0;
        
        // 觸發搜尋事件
        OnSubjectSearched(searchText, results.Rows.Count);
        
        return results.Rows.Count;
    }
    catch (Exception ex)
    {
        // 處理搜尋錯誤
        HandleSearchError(ex);
        return 0;
    }
}
```

此方法執行科目搜尋，根據提供的搜尋文字和類型從Web服務獲取科目資料並顯示。

### 事件處理方法

#### btnSearch_Click

```csharp
protected void btnSearch_Click(object sender, EventArgs e)
{
    // 獲取搜尋文字
    string searchText = txtSubjectCode.Text.Trim();
    
    // 執行搜尋
    if (!string.IsNullOrEmpty(searchText))
    {
        SearchSubjects(searchText, "Both"); // 搜尋代碼和名稱
    }
}
```

此方法處理搜尋按鈕的點擊事件，執行科目搜尋。

#### gvSubjects_RowCommand

```csharp
protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
{
    // 處理選擇命令
    if (e.CommandName == "Select")
    {
        // 獲取行索引
        int rowIndex = Convert.ToInt32(e.CommandArgument);
        
        // 獲取科目代碼
        string subjectCode = gvSubjects.DataKeys[rowIndex].Value.ToString();
        
        // 設定選擇的科目
        SetSubject(subjectCode);
        
        // 隱藏搜尋結果面板
        pnlSearchResults.Visible = false;
    }
}
```

此方法處理科目列表中的選擇命令，設定用戶選擇的科目。

#### cvSubjectCode_ServerValidate

```csharp
protected void cvSubjectCode_ServerValidate(object source, ServerValidateEventArgs args)
{
    // 獲取科目代碼
    string subjectCode = args.Value.Trim();
    
    // 檢查科目代碼是否為空
    if (string.IsNullOrEmpty(subjectCode))
    {
        args.IsValid = !RequiredSubject;
        return;
    }
    
    // 驗證科目的有效性
    args.IsValid = IsSubjectExists(subjectCode) && 
                   IsSubjectActive(subjectCode) &&
                   IsSubjectTypeAllowed(subjectCode) &&
                   CheckSubjectPermissions(subjectCode);
}
```

此方法實現伺服器端科目驗證邏輯，檢查科目的存在性、活動狀態、類型和權限。

### 事件觸發機制

#### OnSubjectSelected

```csharp
protected virtual void OnSubjectSelected()
{
    if (SubjectSelected != null)
    {
        // 創建事件參數
        SubjectEventArgs args = new SubjectEventArgs(
            SelectedSubjectCode,
            SelectedSubjectName,
            SelectedSubjectType
        );
        
        // 觸發事件
        SubjectSelected(this, args);
    }
}
```

此方法在科目被選擇時觸發事件，提供選擇的科目資訊。

#### OnSubjectChanged

```csharp
protected virtual void OnSubjectChanged(string oldSubjectCode)
{
    if (SubjectChanged != null)
    {
        // 創建事件參數
        SubjectChangedEventArgs args = new SubjectChangedEventArgs(
            oldSubjectCode,
            SelectedSubjectCode,
            SelectedSubjectName,
            SelectedSubjectType
        );
        
        // 觸發事件
        SubjectChanged(this, args);
    }
}
```

此方法在科目變更時觸發事件，提供舊科目和新科目的相關資訊。

## 自定義類型定義

### SubjectEventArgs

```csharp
public class SubjectEventArgs : EventArgs
{
    public string SubjectCode { get; private set; }
    public string SubjectName { get; private set; }
    public string SubjectType { get; private set; }
    
    public SubjectEventArgs(string code, string name, string type)
    {
        SubjectCode = code;
        SubjectName = name;
        SubjectType = type;
    }
}
```

此類定義科目選擇事件的參數，包含科目代碼、名稱和類型。

### SubjectChangedEventArgs

```csharp
public class SubjectChangedEventArgs : EventArgs
{
    public string OldSubjectCode { get; private set; }
    public string NewSubjectCode { get; private set; }
    public string NewSubjectName { get; private set; }
    public string NewSubjectType { get; private set; }
    
    public SubjectChangedEventArgs(string oldCode, string newCode, string newName, string newType)
    {
        OldSubjectCode = oldCode;
        NewSubjectCode = newCode;
        NewSubjectName = newName;
        NewSubjectType = newType;
    }
}
```

此類定義科目變更事件的參數，包含舊科目代碼和新科目的相關資訊。

## 異常處理

### 資料載入異常處理

```csharp
private bool SafeLoadSubject(string subjectCode)
{
    try
    {
        // 載入科目資料
        return LoadSubjectByCode(subjectCode);
    }
    catch (Exception ex)
    {
        // 記錄錯誤
        LogError("載入科目資料失敗", ex);
        
        // 顯示錯誤訊息
        ShowErrorMessage("無法載入科目資料：" + ex.Message);
        
        return false;
    }
}
```

此方法安全地載入科目資料，捕獲並處理可能發生的異常。

### 搜尋錯誤處理

```csharp
private void HandleSearchError(Exception ex)
{
    // 記錄錯誤
    LogError("科目搜尋失敗", ex);
    
    // 顯示錯誤訊息
    ShowErrorMessage("科目搜尋失敗：" + ex.Message);
    
    // 清空搜尋結果
    gvSubjects.DataSource = null;
    gvSubjects.DataBind();
    pnlSearchResults.Visible = false;
}
```

此方法處理科目搜尋過程中可能發生的錯誤，記錄錯誤並顯示錯誤訊息。

### 錯誤記錄和顯示

```csharp
private void LogError(string message, Exception ex)
{
    // 記錄錯誤到日誌
    System.Diagnostics.Trace.WriteLine(string.Format("Subjects 錯誤: {0}", message));
    System.Diagnostics.Trace.WriteLine(ex.Message);
    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
    
    // 可以整合系統日誌機制
    // Logger.LogError("Subjects", message, ex);
}

private void ShowErrorMessage(string message)
{
    // 顯示錯誤訊息
    ScriptManager.RegisterStartupScript(this, GetType(), "SubjectError",
        string.Format("alert('{0}');", message.Replace("'", "\\'")), true);
}

private void ShowValidationError(string message)
{
    // 設定驗證錯誤訊息
    cvSubjectCode.ErrorMessage = message;
    cvSubjectCode.IsValid = false;
}
```

這些方法處理錯誤記錄和訊息顯示，提供使用者友好的錯誤反饋。

## 使用範例

以下是 Subjects.ascx.cs 的使用範例：

```csharp
// 在頁面載入時設定科目控制項
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 設定科目控制項屬性
        accountSubject.CompanyId = Session["CompanyId"].ToString();
        accountSubject.RequiredSubject = true;
        accountSubject.EnableAutoComplete = true;
        accountSubject.AllowedSubjectTypes = new string[] { "Assets", "Liabilities", "Equities" };
        
        // 註冊事件處理器
        accountSubject.SubjectSelected += new EventHandler<SubjectEventArgs>(accountSubject_SubjectSelected);
        accountSubject.SubjectValidated += new EventHandler<SubjectValidatedEventArgs>(accountSubject_SubjectValidated);
        
        // 如果有初始科目代碼，則設定科目
        if (!string.IsNullOrEmpty(Request.QueryString["AccountCode"]))
        {
            accountSubject.SetSubject(Request.QueryString["AccountCode"]);
        }
    }
}

// 處理科目選擇事件
protected void accountSubject_SubjectSelected(object sender, SubjectEventArgs e)
{
    // 顯示所選科目資訊
    lblSelectedInfo.Text = string.Format(
        "已選擇科目: {0} - {1} ({2})",
        e.SubjectCode,
        e.SubjectName,
        e.SubjectType
    );
    
    // 根據科目類型執行不同處理
    switch (e.SubjectType)
    {
        case "Assets":
            LoadAssetDetails(e.SubjectCode);
            break;
        case "Liabilities":
            LoadLiabilityDetails(e.SubjectCode);
            break;
        case "Equities":
            LoadEquityDetails(e.SubjectCode);
            break;
    }
}

// 處理科目驗證事件
protected void accountSubject_SubjectValidated(object sender, SubjectValidatedEventArgs e)
{
    if (e.IsValid)
    {
        // 科目驗證通過，啟用保存功能
        btnSave.Enabled = true;
    }
    else
    {
        // 科目驗證失敗，禁用保存功能
        btnSave.Enabled = false;
    }
}

// 保存資料
protected void btnSave_Click(object sender, EventArgs e)
{
    // 再次驗證科目
    if (accountSubject.ValidateSubject())
    {
        // 獲取科目資訊
        string subjectCode = accountSubject.SelectedSubjectCode;
        string subjectName = accountSubject.SelectedSubjectName;
        
        // 執行資料保存邏輯
        SaveAccountData(subjectCode, subjectName);
    }
}
```

## 效能考量

1. **科目資料快取**：
   - 實現科目資料的記憶體快取，減少資料庫查詢
   - 針對頻繁存取的科目資料使用應用程式級快取

2. **延遲載入**：
   - 使用延遲載入策略，按需載入科目資料
   - 避免一次性載入全部科目資料，減少啟動時的資源消耗

3. **資料傳輸優化**：
   - 僅傳輸必要的科目欄位資料
   - 使用分頁機制處理大量科目資料

4. **查詢效率**：
   - 使用索引和優化的SQL查詢提高科目查詢效率
   - 合理使用資料篩選，減少後端處理量

## 安全性考量

1. **輸入驗證**：
   - 對科目代碼進行格式和長度驗證
   - 防止SQL注入和跨站腳本攻擊

2. **權限控制**：
   - 整合系統權限框架，檢查使用者對科目的存取權限
   - 限制使用者只能選擇有權限的科目

3. **安全通訊**：
   - 使用安全的Web服務通訊
   - 保護科目資料傳輸的安全性

4. **日誌記錄**：
   - 記錄科目操作的關鍵行為
   - 針對可疑操作進行特別記錄和監控

## 測試情況

1. **單元測試**：
   - 測試科目載入、驗證和搜尋功能的正確性
   - 測試錯誤處理和異常情況

2. **整合測試**：
   - 測試與科目相關Web服務的整合
   - 測試與頁面其他控制項的互動

3. **效能測試**：
   - 測試大量科目資料下的載入和搜尋效能
   - 測試頻繁科目操作下的資源使用

4. **安全測試**：
   - 測試科目權限控制的有效性
   - 測試對惡意輸入的處理能力

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/11 | 1.0    | 首次建立科目控制項程式碼規格書        | Claude AI  | 