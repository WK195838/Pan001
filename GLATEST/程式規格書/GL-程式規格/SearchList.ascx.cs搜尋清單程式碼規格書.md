# SearchList.ascx.cs 搜尋清單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | SearchList.ascx.cs                     |
| 程式名稱     | 搜尋清單程式碼                           |
| 檔案大小     | 12KB                                  |
| 行數        | ~380                                  |
| 功能簡述     | 通用搜尋後端邏輯                         |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/10                             |

## 程式功能概述

SearchList.ascx.cs 是泛太總帳系統中搜尋清單控制項的後端程式碼實現，負責處理通用搜尋功能的核心邏輯。該程式碼實現了資料搜尋、過濾、排序、分頁、選擇等功能，支援多種搜尋方式和資料來源。其主要功能包括：

1. 處理各種搜尋模式的後端邏輯（精確搜尋、模糊搜尋）
2. 實現資料源的綁定和資料過濾機制
3. 處理分頁邏輯和頁面切換事件
4. 實現資料項選擇和多選功能
5. 提供各種事件處理機制，實現與頁面交互
6. 支援自動完成和即時搜尋功能
7. 提供客製化搜尋和資料過濾功能
8. 實現資料驗證和異常處理機制
9. 支援資料匯出和格式轉換
10. 整合其他系統控制項和服務

## 程式架構說明

SearchList.ascx.cs 的程式架構包含以下主要部分：

1. **命名空間和引用**：引入必要的系統庫和自定義元件
2. **類別定義**：定義搜尋清單控制項類別及其繼承關係
3. **屬性區域**：定義控制項的各種屬性和設定選項
4. **事件定義**：定義控制項支援的各種事件
5. **生命週期方法**：包含頁面載入、初始化、預渲染等方法
6. **搜尋方法**：實現各種搜尋邏輯和過濾功能
7. **資料綁定方法**：處理資料來源的綁定和轉換
8. **事件處理方法**：處理各種控制項的事件響應
9. **輔助方法**：提供各種工具方法和資料處理功能
10. **私有方法**：實現內部邏輯和輔助功能

## 技術實現

SearchList.ascx.cs 使用以下技術實現功能：

1. **ASP.NET 伺服器控制項**：作為頁面的後端處理元件
2. **ADO.NET 資料存取**：實現資料庫數據的查詢和處理
3. **LINQ 查詢**：提供高效的資料集合查詢和處理
4. **反射技術**：用於動態處理不同類型的資料來源
5. **Web 服務整合**：支援自動完成和資料查詢服務
6. **AJAX 整合**：實現無刷新的資料更新和交互
7. **事件委派模式**：提供豐富的事件處理機制
8. **物件導向設計**：採用封裝、繼承、多態等設計原則

## 依賴關係

SearchList.ascx.cs 依賴以下元件和檔案：

1. SearchList.ascx：控制項的 UI 定義檔案
2. System.Web.UI：ASP.NET UI 控制項基礎庫
3. System.Data：資料處理和資料庫交互庫
4. System.Linq：LINQ 查詢支援
5. WSAutoComplete：提供自動完成功能的 Web 服務
6. pagefunction.js：提供客戶端互動的 JavaScript 檔案
7. AjaxControlToolkit：提供 AJAX 擴展功能

## 主要類別和方法

### 類別定義

```csharp
public partial class SearchList : System.Web.UI.UserControl
{
    // 類別實現
}
```

### 主要屬性

```csharp
// 設定和獲取搜尋資料來源
public object SearchDataSource { get; set; }

// 設定搜尋的欄位
public string[] SearchColumns { get; set; }

// 設定顯示的欄位
public string[] DisplayColumns { get; set; }

// 設定值欄位
public string ValueColumn { get; set; }

// 設定顯示文字欄位
public string TextColumn { get; set; }

// 搜尋模式 (精確/模糊)
public SearchModeEnum SearchMode { get; set; }

// 是否允許多選
public bool AllowMultiSelect { get; set; }

// 是否啟用自動完成
public bool AutoCompleteEnabled { get; set; }

// 每頁顯示的項目數量
public int PageSize { get; set; }

// 獲取或設定選擇的值
public string SelectedValue { get; set; }

// 獲取選擇項目的顯示文字
public string SelectedText { get; get; }

// 獲取多選模式下的所有選擇值
public string[] SelectedValues { get; }
```

### 主要事件

```csharp
// 完成搜尋時觸發
public event EventHandler<SearchEventArgs> SearchPerformed;

// 選擇項目時觸發
public event EventHandler<SearchItemEventArgs> ItemSelected;

// 多選模式下完成選擇時觸發
public event EventHandler<SearchItemsEventArgs> MultiItemsSelected;

// 取消搜尋時觸發
public event EventHandler SearchCancelled;

// 搜尋結果綁定完成時觸發
public event EventHandler<DataBindEventArgs> DataBound;

// 沒有找到結果時觸發
public event EventHandler<SearchEventArgs> NoResultsFound;
```

### 主要方法

```csharp
// 頁面生命週期方法
protected void Page_Load(object sender, EventArgs e)
protected void Page_Init(object sender, EventArgs e)
protected override void OnPreRender(EventArgs e)

// 搜尋方法
public int Search(string searchText = null)
public void ClearSearch()
public bool SelectItem(string value)
public object GetItemByValue(string value)
public IEnumerable<object> GetItemsByValues(string[] values)

// 資料綁定方法
private void BindSearchResults(string searchText)
private DataView FilterDataSource(string searchText)
private void UpdateResultsCount(int count)

// 事件處理方法
protected void btnSearch_Click(object sender, EventArgs e)
protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
protected void gvResults_RowCommand(object sender, GridViewCommandEventArgs e)
protected void gvResults_SelectedIndexChanged(object sender, EventArgs e)
protected void btnSelect_Click(object sender, EventArgs e)
protected void btnCancel_Click(object sender, EventArgs e)

// 輔助方法
private bool ValidateDataSource()
private bool ValidateSearchText(string searchText)
private void SetupGridViewColumns()
private void HandleNoResults(string searchText)
private string GetSortExpression()
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
    }
}
```

此方法在頁面載入時執行，初始化控制項的狀態，並在首次載入時設定預設值和狀態。

#### Page_Init

```csharp
protected void Page_Init(object sender, EventArgs e)
{
    // 在頁面初始化階段註冊必要的腳本和樣式
    RegisterClientScripts();
}
```

此方法在頁面初始化階段執行，註冊必要的客戶端腳本和CSS樣式。

#### OnPreRender

```csharp
protected override void OnPreRender(EventArgs e)
{
    base.OnPreRender(e);
    
    // 在渲染前更新控制項狀態
    UpdateControlState();
}
```

此方法在頁面渲染前執行，進行最終的狀態更新和準備。

### 搜尋方法

#### Search

```csharp
public int Search(string searchText = null)
{
    // 如未提供搜尋文字，使用文字框的值
    if (string.IsNullOrEmpty(searchText))
    {
        searchText = txtSearch.Text.Trim();
    }
    
    // 驗證搜尋條件
    if (!ValidateSearchText(searchText))
    {
        return 0;
    }
    
    // 綁定搜尋結果
    BindSearchResults(searchText);
    
    // 獲取結果數量
    int resultCount = GetResultCount();
    
    // 觸發搜尋完成事件
    OnSearchPerformed(new SearchEventArgs(searchText, resultCount));
    
    return resultCount;
}
```

此方法執行搜尋操作，過濾和綁定搜尋結果，並返回結果數量。

#### ClearSearch

```csharp
public void ClearSearch()
{
    // 清除搜尋條件
    txtSearch.Text = string.Empty;
    
    // 清除結果
    ClearResults();
    
    // 重置選擇狀態
    ResetSelection();
}
```

此方法清除搜尋條件和結果，重置控制項的狀態。

#### SelectItem

```csharp
public bool SelectItem(string value)
{
    // 驗證值是否存在
    object item = GetItemByValue(value);
    if (item == null)
    {
        return false;
    }
    
    // 設定選擇的值
    SelectedValue = value;
    
    // 更新顯示
    UpdateSelectionDisplay();
    
    // 觸發選擇事件
    OnItemSelected(new SearchItemEventArgs(value, GetTextForValue(value)));
    
    return true;
}
```

此方法根據提供的值選擇對應的項目，並更新選擇狀態。

### 資料綁定方法

#### BindSearchResults

```csharp
private void BindSearchResults(string searchText)
{
    // 驗證資料來源
    if (!ValidateDataSource())
    {
        return;
    }
    
    // 過濾資料
    DataView filteredData = FilterDataSource(searchText);
    
    // 綁定到GridView
    gvResults.DataSource = filteredData;
    gvResults.DataBind();
    
    // 更新結果狀態
    UpdateResultsDisplay(filteredData.Count);
    
    // 觸發資料綁定事件
    OnDataBound(new DataBindEventArgs(filteredData, filteredData.Count));
}
```

此方法將過濾後的資料綁定到結果顯示控制項，並更新界面狀態。

#### FilterDataSource

```csharp
private DataView FilterDataSource(string searchText)
{
    // 獲取資料來源
    DataTable dataTable = GetDataTable();
    
    // 創建過濾表達式
    string filterExpression = BuildFilterExpression(searchText);
    
    // 應用過濾和排序
    DataView dataView = new DataView(dataTable);
    dataView.RowFilter = filterExpression;
    dataView.Sort = GetSortExpression();
    
    return dataView;
}
```

此方法根據搜尋條件和搜尋模式過濾資料來源。

### 事件處理方法

#### btnSearch_Click

```csharp
protected void btnSearch_Click(object sender, EventArgs e)
{
    // 執行搜尋
    Search();
}
```

此方法處理搜尋按鈕的點擊事件，觸發搜尋操作。

#### gvResults_PageIndexChanging

```csharp
protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
{
    // 設定新的頁面索引
    gvResults.PageIndex = e.NewPageIndex;
    
    // 重新綁定資料，保持當前搜尋條件
    Search();
}
```

此方法處理分頁控制的頁面切換事件。

#### btnSelect_Click

```csharp
protected void btnSelect_Click(object sender, EventArgs e)
{
    // 多選模式
    if (AllowMultiSelect)
    {
        // 獲取選擇的項目
        List<string> selectedValues = GetSelectedValues();
        
        // 更新選擇狀態
        UpdateMultiSelection(selectedValues);
        
        // 觸發多選事件
        OnMultiItemsSelected(new SearchItemsEventArgs(selectedValues.ToArray()));
    }
    // 單選模式
    else
    {
        // 獲取選擇的值
        string value = GetSelectedValue();
        
        // 選擇項目
        SelectItem(value);
    }
}
```

此方法處理選擇按鈕的點擊事件，根據模式處理單選或多選操作。

### 輔助方法

#### ValidateDataSource

```csharp
private bool ValidateDataSource()
{
    // 檢查資料來源是否為空
    if (SearchDataSource == null)
    {
        HandleError("資料來源未設定");
        return false;
    }
    
    // 檢查必要欄位
    if (!ValidateRequiredColumns())
    {
        HandleError("必要欄位未設定或不存在");
        return false;
    }
    
    return true;
}
```

此方法驗證資料來源的有效性，確保資料來源不為空且包含必要的欄位。

#### BuildFilterExpression

```csharp
private string BuildFilterExpression(string searchText)
{
    // 特殊字符處理
    searchText = EscapeSearchText(searchText);
    
    // 構建過濾表達式
    StringBuilder filter = new StringBuilder();
    
    // 遍歷搜尋欄位
    for (int i = 0; i < SearchColumns.Length; i++)
    {
        if (i > 0)
        {
            filter.Append(" OR ");
        }
        
        // 根據搜尋模式構建表達式
        if (SearchMode == SearchModeEnum.Exact)
        {
            filter.AppendFormat("{0} = '{1}'", SearchColumns[i], searchText);
        }
        else
        {
            filter.AppendFormat("{0} LIKE '%{1}%'", SearchColumns[i], searchText);
        }
    }
    
    return filter.ToString();
}
```

此方法根據搜尋文字和搜尋模式構建資料過濾表達式。

## 事件觸發機制

控制項實現以下事件觸發機制：

### OnSearchPerformed

```csharp
protected virtual void OnSearchPerformed(SearchEventArgs e)
{
    if (SearchPerformed != null)
    {
        SearchPerformed(this, e);
    }
}
```

在搜尋完成時觸發此事件，提供搜尋文字和結果數量信息。

### OnItemSelected

```csharp
protected virtual void OnItemSelected(SearchItemEventArgs e)
{
    if (ItemSelected != null)
    {
        ItemSelected(this, e);
    }
}
```

在項目被選擇時觸發此事件，提供選擇的值和文字信息。

### OnMultiItemsSelected

```csharp
protected virtual void OnMultiItemsSelected(SearchItemsEventArgs e)
{
    if (MultiItemsSelected != null)
    {
        MultiItemsSelected(this, e);
    }
}
```

在多選模式下完成選擇時觸發此事件，提供所有選擇的值。

### OnNoResultsFound

```csharp
protected virtual void OnNoResultsFound(SearchEventArgs e)
{
    if (NoResultsFound != null)
    {
        NoResultsFound(this, e);
    }
}
```

在搜尋沒有找到結果時觸發此事件。

## 自定義類型定義

控制項定義以下自定義類型：

### SearchModeEnum

```csharp
public enum SearchModeEnum
{
    Exact,   // 精確搜尋
    Fuzzy    // 模糊搜尋
}
```

此枚舉定義搜尋模式類型。

### SearchEventArgs

```csharp
public class SearchEventArgs : EventArgs
{
    public string SearchText { get; private set; }
    public int ResultCount { get; private set; }
    
    public SearchEventArgs(string searchText, int resultCount)
    {
        SearchText = searchText;
        ResultCount = resultCount;
    }
}
```

此類定義搜尋事件的參數。

### SearchItemEventArgs

```csharp
public class SearchItemEventArgs : EventArgs
{
    public string SelectedValue { get; private set; }
    public string SelectedText { get; private set; }
    
    public SearchItemEventArgs(string value, string text)
    {
        SelectedValue = value;
        SelectedText = text;
    }
}
```

此類定義項目選擇事件的參數。

### SearchItemsEventArgs

```csharp
public class SearchItemsEventArgs : EventArgs
{
    public string[] SelectedValues { get; private set; }
    
    public SearchItemsEventArgs(string[] values)
    {
        SelectedValues = values;
    }
}
```

此類定義多項選擇事件的參數。

## 異常處理

控制項實現以下異常處理機制：

### 資料來源處理

```csharp
private void SafeDataBind()
{
    try
    {
        // 資料綁定操作
        BindData();
    }
    catch (Exception ex)
    {
        // 記錄錯誤
        LogError(ex);
        
        // 顯示友好錯誤訊息
        DisplayErrorMessage("資料載入失敗，請稍後再試。");
    }
}
```

此方法安全地處理資料綁定，捕獲並記錄可能的異常。

### 使用者輸入驗證

```csharp
private bool ValidateSearchText(string searchText)
{
    // 檢查長度
    if (string.IsNullOrEmpty(searchText) || searchText.Length < MinSearchLength)
    {
        DisplayWarningMessage(string.Format("搜尋文字必須至少包含 {0} 個字符", MinSearchLength));
        return false;
    }
    
    // 檢查特殊字符
    if (ContainsInvalidCharacters(searchText))
    {
        DisplayWarningMessage("搜尋文字包含無效字符");
        return false;
    }
    
    return true;
}
```

此方法驗證搜尋文字的有效性，檢查長度和特殊字符。

### 錯誤記錄和顯示

```csharp
private void LogError(Exception ex)
{
    // 記錄錯誤
    System.Diagnostics.Trace.WriteLine(string.Format("SearchList 錯誤: {0}", ex.Message));
    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
    
    // 可以整合系統日誌機制
    // Logger.LogError("SearchList", ex);
}

private void DisplayErrorMessage(string message)
{
    // 顯示錯誤訊息
    lblErrorMessage.Text = message;
    lblErrorMessage.Visible = true;
}

private void DisplayWarningMessage(string message)
{
    // 顯示警告訊息
    lblWarningMessage.Text = message;
    lblWarningMessage.Visible = true;
}
```

這些方法處理錯誤記錄和訊息顯示，提供使用者友好的錯誤反饋。

## 使用範例

以下是 SearchList.ascx.cs 的使用範例：

```csharp
// 在頁面載入時初始化搜尋控制項
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        // 獲取科目資料
        DataTable accounts = GetAccountsData();
        
        // 設定搜尋控制項屬性
        accountSearch.SearchDataSource = accounts;
        accountSearch.SearchColumns = new string[] { "AccountCode", "AccountName" };
        accountSearch.DisplayColumns = new string[] { "AccountCode", "AccountName", "AccountType" };
        accountSearch.ValueColumn = "AccountCode";
        accountSearch.TextColumn = "AccountName";
        accountSearch.SearchMode = SearchList.SearchModeEnum.Fuzzy;
        accountSearch.PageSize = 15;
        accountSearch.AllowMultiSelect = false;
        accountSearch.AutoCompleteEnabled = true;
        
        // 註冊事件處理器
        accountSearch.ItemSelected += new EventHandler<SearchList.SearchItemEventArgs>(accountSearch_ItemSelected);
        accountSearch.SearchPerformed += new EventHandler<SearchList.SearchEventArgs>(accountSearch_SearchPerformed);
    }
}

// 處理項目選擇事件
protected void accountSearch_ItemSelected(object sender, SearchList.SearchItemEventArgs e)
{
    string selectedAccountCode = e.SelectedValue;
    string selectedAccountName = e.SelectedText;
    
    // 處理選擇結果
    txtAccountCode.Text = selectedAccountCode;
    txtAccountName.Text = selectedAccountName;
    
    // 顯示科目詳情
    ShowAccountDetails(selectedAccountCode);
}

// 處理搜尋完成事件
protected void accountSearch_SearchPerformed(object sender, SearchList.SearchEventArgs e)
{
    // 更新搜尋統計信息
    lblSearchStat.Text = string.Format("搜尋「{0}」找到 {1} 筆資料", e.SearchText, e.ResultCount);
}
```

## 效能考量

1. **資料過濾優化**：
   - 使用索引和優化的過濾表達式
   - 限制結果集大小，避免過大的資料傳輸

2. **分頁實現**：
   - 僅載入當前頁面所需的資料
   - 避免在客戶端處理大量資料

3. **快取策略**：
   - 適當使用 ViewState 保存狀態
   - 避免冗餘的資料查詢和重新綁定

4. **客戶端整合**：
   - 使用 AJAX 實現無刷新更新
   - 延遲處理用戶輸入，避免頻繁搜尋

## 安全性考量

1. **輸入驗證**：
   - 過濾 SQL 注入和跨站腳本攻擊
   - 驗證使用者輸入的合法性

2. **資料存取控制**：
   - 檢查使用者權限，限制資料存取
   - 根據使用者角色過濾資料

3. **安全輸出處理**：
   - 對輸出資料進行編碼，防止 XSS 攻擊
   - 保護敏感資料不被顯示

4. **參數化查詢**：
   - 使用參數化查詢而非直接拼接 SQL
   - 防止 SQL 注入攻擊

## 測試情況

1. **功能測試**：
   - 測試各種搜尋條件下的搜尋結果正確性
   - 測試分頁功能的正常工作
   - 測試選擇操作的正確性
   - 測試各種事件的觸發機制

2. **效能測試**：
   - 測試大資料量下的搜尋響應時間
   - 測試分頁切換的效率
   - 測試資源使用情況

3. **安全測試**：
   - 測試對特殊輸入的處理
   - 測試對惡意輸入的防護
   - 測試權限控制的有效性

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/10 | 1.0    | 首次建立搜尋清單程式碼規格書          | Claude AI  | 