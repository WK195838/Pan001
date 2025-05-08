# SearchList 搜尋清單規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | SearchList                            |
| 程式名稱     | 搜尋清單                               |
| 檔案大小     | 1.7KB                                 |
| 行數        | ~30                                   |
| 功能簡述     | 提供通用搜尋功能                         |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/10                             |

## 程式功能概述

SearchList 是泛太總帳系統中的通用搜尋控制項，提供使用者快速查詢和選擇資料的功能。此控制項廣泛應用於需要資料查詢和選擇的場景，如科目選擇、部門選擇、供應商選擇等。透過整合資料庫查詢和前端互動，實現高效的資料搜尋體驗。主要功能包括：

1. 提供即時搜尋和篩選功能
2. 支援多種資料來源（資料表、自定義資料集等）
3. 可配置搜尋條件和顯示欄位
4. 提供搜尋建議和自動完成功能
5. 支援精確搜尋和模糊搜尋兩種模式
6. 提供資料分頁和排序功能
7. 支援單選和多選模式
8. 可與其他控制項整合，實現資料聯動
9. 提供搜尋結果的資料驗證和過濾
10. 支援自定義的搜尋邏輯和顯示格式

## 控制項結構說明

SearchList 的結構按功能可分為以下區域：

1. **使用者控制項宣告區**：定義控制項的基本屬性和行為
2. **搜尋介面區**：包含搜尋輸入框、按鈕和下拉選單
3. **搜尋結果區**：顯示篩選後的資料列表
4. **分頁控制區**：處理搜尋結果的分頁顯示
5. **屬性和方法區**：提供對外部存取和操作的介面
6. **事件處理區**：處理搜尋、選擇等事件
7. **資料綁定區**：處理資料來源和資料顯示的綁定關係

## 頁面元素

### ASCX 頁面宣告

```html
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchList.ascx.cs" Inherits="SearchList" %>
```

### 頁面結構

頁面包含以下主要部分：

1. **引用相關控制項和元件**：
   ```html
   <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
   ```

2. **搜尋介面元素**：
   - 搜尋標籤 (lblSearch)
   - 搜尋輸入框 (txtSearch)
   - 搜尋按鈕 (btnSearch)
   - 搜尋模式選擇器 (ddlSearchMode)
   - 自動完成面板 (pnlAutoComplete)

3. **搜尋結果元素**：
   - 結果顯示面板 (pnlResults)
   - 資料表格 (gvResults)
   - 分頁控制 (pager)
   - 無結果提示 (lblNoResults)

4. **選擇結果元素**：
   - 選擇按鈕 (btnSelect)
   - 取消按鈕 (btnCancel)
   - 已選擇項目顯示 (lblSelectedItems)

5. **CSS 樣式區**：
   - 控制項外觀的樣式定義
   - 搜尋框和結果列表的樣式設定
   - 分頁控制的樣式設定

## 技術實現

SearchList 採用以下技術：

1. ASP.NET Web Forms 使用者控制項架構
2. AJAX 技術實現即時搜尋和資料刷新
3. 客戶端 JavaScript 提供互動體驗
4. 資料庫查詢實現資料篩選和分頁
5. Web 服務支援自動完成功能
6. 自訂事件處理使資料選擇和界面互動一致

## 依賴關係

SearchList 依賴以下檔案與元件：

1. SearchList.ascx.cs：搜尋清單後端程式碼
2. System.Web.UI.WebControls：提供基本網頁控制項功能
3. System.Web.UI.HtmlControls：提供 HTML 控制項功能
4. AjaxControlToolkit：提供 AJAX 擴充功能
5. WSAutoComplete：自動完成 Web 服務
6. pagefunction.js：提供前端互動功能

## 使用者介面

SearchList 的使用者介面由以下部分組成：

1. **搜尋區域**：
   - 搜尋標籤和輸入框，方便使用者輸入查詢條件
   - 搜尋按鈕，觸發搜尋操作
   - 搜尋模式選擇器，切換精確或模糊搜尋
   - 自動完成下拉清單，顯示相符的搜尋建議

2. **結果區域**：
   - 資料表格，顯示搜尋結果，支援多欄位顯示
   - 分頁控制，提供大量結果的分頁瀏覽
   - 排序控制，允許使用者按不同欄位排序
   - 選擇控制，提供勾選或點擊選取功能

3. **控制區域**：
   - 確認按鈕，確認使用者的選擇並觸發相應事件
   - 取消按鈕，取消操作並重置控制項
   - 已選擇項目區域，顯示目前已選擇的項目

## 控制項屬性

SearchList 提供以下主要屬性：

1. **SearchDataSource**：
   - 類型：Object
   - 功能：設定搜尋的資料來源
   - 預設值：null

2. **SearchColumns**：
   - 類型：string[]
   - 功能：設定要搜尋的欄位名稱
   - 預設值：空陣列

3. **DisplayColumns**：
   - 類型：string[]
   - 功能：設定要顯示的欄位名稱
   - 預設值：空陣列

4. **ValueColumn**：
   - 類型：string
   - 功能：設定作為回傳值的欄位名稱
   - 預設值：空字串

5. **TextColumn**：
   - 類型：string
   - 功能：設定作為顯示文字的欄位名稱
   - 預設值：空字串

6. **SearchMode**：
   - 類型：enum (Exact, Fuzzy)
   - 功能：設定搜尋模式
   - 預設值：Fuzzy

7. **AllowMultiSelect**：
   - 類型：bool
   - 功能：指定是否允許多選
   - 預設值：false

8. **AutoCompleteEnabled**：
   - 類型：bool
   - 功能：指定是否啟用自動完成功能
   - 預設值：true

9. **PageSize**：
   - 類型：int
   - 功能：設定每頁顯示的結果數量
   - 預設值：10

10. **MinSearchLength**：
    - 類型：int
    - 功能：設定觸發搜尋的最小字符數
    - 預設值：2

11. **SelectedValue**：
    - 類型：string
    - 功能：取得或設定選取的值
    - 預設值：空字串

12. **SelectedText**：
    - 類型：string
    - 功能：取得選取項目的顯示文字
    - 預設值：空字串

13. **SelectedValues**：
    - 類型：string[]
    - 功能：取得多選模式下的所有選取值
    - 預設值：空陣列

14. **SearchDelay**：
    - 類型：int
    - 功能：設定自動搜尋的延遲時間（毫秒）
    - 預設值：500

15. **LabelText**：
    - 類型：string
    - 功能：設定搜尋標籤文字
    - 預設值：「搜尋」

## 控制項方法

控制項提供以下主要方法：

1. **Search**：
   - 功能：執行搜尋操作
   - 參數：searchText（可選）
   - 返回：搜尋結果數量

2. **ClearSearch**：
   - 功能：清除搜尋條件和結果
   - 參數：無
   - 返回：無

3. **SelectItem**：
   - 功能：根據值選取項目
   - 參數：value
   - 返回：是否成功選取

4. **GetItemByValue**：
   - 功能：根據值取得項目資料
   - 參數：value
   - 返回：項目資料（資料行或物件）

5. **GetItemsByValues**：
   - 功能：根據多個值取得多個項目資料
   - 參數：values（陣列）
   - 返回：項目資料集合

6. **RefreshDataSource**：
   - 功能：重新整理資料來源
   - 參數：無
   - 返回：無

7. **SetCustomFilter**：
   - 功能：設定自訂的資料篩選條件
   - 參數：filterExpression
   - 返回：無

8. **ExportToExcel**：
   - 功能：將搜尋結果匯出為 Excel
   - 參數：無
   - 返回：無

## 事件處理

控制項包含以下事件處理：

1. **SearchPerformed**：
   - 觸發條件：完成搜尋操作時
   - 事件參數：搜尋文字、結果數量

2. **ItemSelected**：
   - 觸發條件：使用者選取項目時
   - 事件參數：選取的值、選取的文字

3. **MultiItemsSelected**：
   - 觸發條件：多選模式下完成選取時
   - 事件參數：選取的值集合

4. **SearchCancelled**：
   - 觸發條件：使用者取消搜尋時
   - 事件參數：無

5. **DataBound**：
   - 觸發條件：搜尋結果綁定完成時
   - 事件參數：資料來源、結果數量

6. **NoResultsFound**：
   - 觸發條件：搜尋沒有找到結果時
   - 事件參數：搜尋文字

## 使用範例

以下是 SearchList 控制項的基本使用方式：

```html
<!-- 在 ASPX 頁面中引用控制項 -->
<uc:SearchList ID="accountSearch" runat="server" 
    LabelText="搜尋科目" 
    SearchMode="Fuzzy"
    AllowMultiSelect="false"
    AutoCompleteEnabled="true"
    OnItemSelected="accountSearch_ItemSelected" />

<!-- 在程式碼中操作控制項 -->
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 設定資料來源
            DataTable accounts = GetAccountsData();
            accountSearch.SearchDataSource = accounts;
            
            // 設定搜尋和顯示欄位
            accountSearch.SearchColumns = new string[] { "AccountCode", "AccountName" };
            accountSearch.DisplayColumns = new string[] { "AccountCode", "AccountName", "AccountType" };
            accountSearch.ValueColumn = "AccountCode";
            accountSearch.TextColumn = "AccountName";
            
            // 設定每頁顯示數量
            accountSearch.PageSize = 15;
        }
    }
    
    protected void accountSearch_ItemSelected(object sender, SearchItemEventArgs e)
    {
        // 處理選取結果
        string selectedAccountCode = e.SelectedValue;
        string selectedAccountName = e.SelectedText;
        
        // 執行後續操作...
        LoadAccountDetails(selectedAccountCode);
    }
    
    private DataTable GetAccountsData()
    {
        // 取得科目資料
        // ...實現資料載入邏輯...
        return accountTable;
    }
    
    private void LoadAccountDetails(string accountCode)
    {
        // 載入科目詳細資料
        // ...實現科目資料載入邏輯...
    }
</script>
```

## 異常處理

控制項包含以下異常處理機制：

1. **資料來源驗證**：
   - 確保資料來源有效且包含指定的欄位
   - 處理資料來源為空的情況

2. **搜尋條件驗證**：
   - 確保搜尋條件的有效性
   - 防止無效的搜尋條件導致效能問題

3. **結果處理**：
   - 處理搜尋結果為空的情況
   - 提供適當的使用者反饋

4. **分頁處理**：
   - 確保分頁索引在有效範圍內
   - 處理頁面切換時的資料載入

5. **選取驗證**：
   - 驗證選取項目的有效性
   - 處理無效選取的情況

## 效能考量

1. **查詢優化**：
   - 限制搜尋條件長度，避免過於廣泛的搜尋
   - 使用分頁和延遲載入機制減少資料傳輸

2. **快取機制**：
   - 實施適當的資料快取策略
   - 減少重複的資料庫查詢

3. **使用者體驗優化**：
   - 使用輸入延遲機制，避免頻繁搜尋
   - 提供視覺反饋，顯示搜尋處理狀態

4. **資源使用**：
   - 合理限制結果集大小
   - 釋放不再使用的資源

## 安全性考量

1. **輸入驗證**：
   - 過濾和驗證使用者輸入，防止注入攻擊
   - 限制搜尋條件的複雜度和長度

2. **授權檢查**：
   - 確保使用者有權存取搜尋的資料
   - 根據使用者權限過濾搜尋結果

3. **資料保護**：
   - 保護敏感資料不被不當顯示
   - 加密傳輸中的敏感資訊

4. **跨站腳本防護**：
   - 對輸出進行適當的編碼
   - 防止惡意腳本注入

## 測試記錄

測試項目包括：

1. **功能測試**：
   - 搜尋功能的準確性和完整性
   - 分頁功能的正確性
   - 項目選擇功能的可靠性
   - 各種搜尋模式下的行為一致性

2. **效能測試**：
   - 大量資料下的搜尋響應時間
   - 資源使用情況
   - 分頁切換的效率

3. **介面測試**：
   - 控制項在各種瀏覽器下的渲染一致性
   - 響應式設計在不同螢幕尺寸下的表現
   - 使用者互動體驗

4. **整合測試**：
   - 與其他控制項的協同工作
   - 在各種資料情境下的表現

## 待改進事項

1. 增加更多的資料源支援，如 Web 服務、JSON 資料等
2. 優化大資料量下的搜尋效率
3. 增強自動完成功能，提供更智能的搜尋建議
4. 提供更多自定義的搜尋邏輯選項
5. 增加異步載入功能，改善使用者體驗
6. 增強過濾和排序功能
7. 改進行動裝置上的使用體驗
8. 增加搜尋歷史記錄功能
9. 提供更多的匯出格式選項
10. 實現搜尋條件的保存和載入功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/10 | 1.0    | 首次建立搜尋清單規格書                | Claude AI  | 