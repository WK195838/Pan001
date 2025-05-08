# Subjects 科目控制項規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | Subjects                              |
| 程式名稱     | 科目控制項                              |
| 檔案大小     | 2.7KB                                 |
| 行數        | ~70                                   |
| 功能簡述     | 提供科目選擇功能                         |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/11                             |

## 程式功能概述

Subjects 是泛太總帳系統中專門用於會計科目選擇的控制項，為使用者提供直觀便捷的科目查詢與選擇功能。此控制項整合了科目搜尋、選擇、驗證等多項功能，適用於系統中所有需要選擇會計科目的場景，如傳票輸入、報表查詢、科目設定等。主要功能包括：

1. 提供科目代碼和名稱的綜合搜尋功能
2. 支援科目層級結構的瀏覽和選擇
3. 支援科目有效性和權限的即時驗證
4. 整合科目屬性顯示和詳細資訊查詢
5. 提供科目歷史記錄和常用科目快速選擇
6. 支援科目自動完成和智能建議功能
7. 處理科目分類篩選和範圍限制
8. 提供科目選擇後的資料回傳和事件通知
9. 支援科目資料的在地化顯示
10. 整合科目相關的業務規則驗證

## 控制項結構說明

Subjects 控制項的結構按功能可分為以下幾個區域：

1. **科目選擇面板**：包含科目輸入框和輔助功能按鈕
2. **科目搜尋區**：提供科目代碼和名稱的搜尋功能
3. **科目列表區**：顯示符合條件的科目清單
4. **科目詳情區**：顯示選定科目的詳細資訊
5. **控制區**：包含確認、取消等操作按鈕
6. **屬性和方法區**：提供對外部存取和操作的介面
7. **事件處理區**：處理科目選擇和變更事件

## 頁面元素

### ASCX 頁面宣告

```html
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Subjects.ascx.cs" Inherits="Subjects" %>
```

### 頁面結構

頁面包含以下主要部分：

1. **科目輸入區域**：
   ```html
   <div class="subject-input-panel">
     <asp:Label ID="lblSubjectCode" runat="server" Text="科目代碼：" CssClass="subject-label"></asp:Label>
     <asp:TextBox ID="txtSubjectCode" runat="server" CssClass="subject-textbox" MaxLength="20"></asp:TextBox>
     <asp:Button ID="btnSearch" runat="server" Text="搜尋" CssClass="subject-button" OnClick="btnSearch_Click" />
     <asp:Button ID="btnAdvanced" runat="server" Text="進階" CssClass="subject-button" OnClick="btnAdvanced_Click" />
   </div>
   ```

2. **科目資訊顯示區域**：
   ```html
   <div class="subject-info-panel">
     <asp:Label ID="lblSubjectName" runat="server" CssClass="subject-name-label"></asp:Label>
     <asp:Label ID="lblSubjectType" runat="server" CssClass="subject-type-label"></asp:Label>
     <asp:Label ID="lblSubjectStatus" runat="server" CssClass="subject-status-label"></asp:Label>
   </div>
   ```

3. **科目搜尋結果區域**：
   ```html
   <asp:Panel ID="pnlSearchResults" runat="server" Visible="false" CssClass="subject-search-panel">
     <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False" 
       CssClass="subject-grid" AllowPaging="True" PageSize="10"
       OnPageIndexChanging="gvSubjects_PageIndexChanging"
       OnRowCommand="gvSubjects_RowCommand">
       <Columns>
         <asp:BoundField DataField="SubjectCode" HeaderText="科目代碼" />
         <asp:BoundField DataField="SubjectName" HeaderText="科目名稱" />
         <asp:BoundField DataField="SubjectType" HeaderText="科目類型" />
         <asp:ButtonField CommandName="Select" Text="選擇" ButtonType="Button" />
       </Columns>
     </asp:GridView>
   </asp:Panel>
   ```

4. **控制按鈕區域**：
   ```html
   <div class="subject-control-panel">
     <asp:Button ID="btnClear" runat="server" Text="清除" CssClass="subject-button" OnClick="btnClear_Click" />
     <asp:Button ID="btnViewDetail" runat="server" Text="科目詳情" CssClass="subject-button" OnClick="btnViewDetail_Click" />
   </div>
   ```

5. **驗證控制區域**：
   ```html
   <asp:RequiredFieldValidator ID="rfvSubjectCode" runat="server" 
     ControlToValidate="txtSubjectCode" Display="Dynamic"
     ErrorMessage="科目代碼為必填欄位" CssClass="subject-validator"></asp:RequiredFieldValidator>
   <asp:CustomValidator ID="cvSubjectCode" runat="server" 
     ControlToValidate="txtSubjectCode" Display="Dynamic"
     ErrorMessage="科目代碼不存在或無效" CssClass="subject-validator"
     OnServerValidate="cvSubjectCode_ServerValidate"></asp:CustomValidator>
   ```

## 技術實現

Subjects 控制項採用以下技術：

1. ASP.NET Web Forms 使用者控制項架構
2. 資料庫查詢整合，提供科目資料存取
3. AJAX 技術實現即時科目搜尋和驗證
4. JavaScript 客戶端互動提升使用者體驗
5. 事件驅動架構支援與父頁面互動
6. 自訂驗證機制確保科目選擇的有效性

## 依賴關係

Subjects 控制項依賴以下檔案與元件：

1. Subjects.ascx.cs：科目控制項後端程式碼
2. System.Web.UI.WebControls：提供基本網頁控制項功能
3. System.Web.UI.HtmlControls：提供 HTML 控制項功能
4. WSGLAcctDefJson：科目定義 Web 服務
5. WSAutoComplete：自動完成 Web 服務
6. pagefunction.js：頁面共用功能的 JavaScript 檔案

## 使用者介面

控制項的使用者介面由以下部分組成：

1. **科目代碼輸入框**：
   - 允許使用者直接輸入科目代碼
   - 支援科目代碼格式的即時驗證
   - 整合自動完成功能，提供代碼建議

2. **科目資訊顯示區**：
   - 顯示所選科目的名稱、類型和狀態
   - 提供視覺化的科目有效性指示
   - 以不同顏色標示不同類型的科目

3. **搜尋與進階功能按鈕**：
   - 搜尋按鈕：觸發科目搜尋功能
   - 進階按鈕：開啟科目進階搜尋面板
   - 清除按鈕：重置科目選擇狀態
   - 科目詳情按鈕：查看科目詳細資訊

4. **科目搜尋結果區**：
   - 以表格形式顯示符合搜尋條件的科目
   - 支援分頁瀏覽大量搜尋結果
   - 提供科目直接選擇功能

## 控制項屬性

Subjects 控制項提供以下主要屬性：

1. **SelectedSubjectCode**：
   - 類型：string
   - 功能：取得或設定所選科目代碼
   - 預設值：空字串

2. **SelectedSubjectName**：
   - 類型：string
   - 功能：取得所選科目名稱
   - 預設值：空字串

3. **SelectedSubjectType**：
   - 類型：string
   - 功能：取得所選科目類型
   - 預設值：空字串

4. **SubjectFilter**：
   - 類型：string
   - 功能：設定科目篩選條件
   - 預設值：空字串

5. **AllowedSubjectTypes**：
   - 類型：string[]
   - 功能：設定允許選擇的科目類型
   - 預設值：null（允許所有類型）

6. **RequiredSubject**：
   - 類型：bool
   - 功能：指定科目是否為必填
   - 預設值：true

7. **EnableAutoComplete**：
   - 類型：bool
   - 功能：啟用或禁用自動完成功能
   - 預設值：true

8. **ReadOnly**：
   - 類型：bool
   - 功能：設定控制項是否為唯讀模式
   - 預設值：false

9. **DisplaySubjectInfo**：
   - 類型：bool
   - 功能：是否顯示科目詳細資訊
   - 預設值：true

10. **CompanyId**：
    - 類型：string
    - 功能：設定科目所屬公司識別碼
    - 預設值：從系統設定取得

## 控制項方法

控制項提供以下主要方法：

1. **SetSubject**：
   - 功能：設定選擇的科目
   - 參數：subjectCode - 科目代碼
   - 返回：是否成功設定

2. **ClearSubject**：
   - 功能：清除已選擇的科目
   - 參數：無
   - 返回：無

3. **ValidateSubject**：
   - 功能：驗證科目的有效性
   - 參數：無
   - 返回：驗證結果

4. **SearchSubjects**：
   - 功能：搜尋符合條件的科目
   - 參數：searchText - 搜尋文字，searchType - 搜尋類型
   - 返回：搜尋結果數量

5. **GetSubjectDetail**：
   - 功能：取得科目的詳細資訊
   - 參數：subjectCode - 科目代碼
   - 返回：科目詳細資訊

6. **IsSubjectActive**：
   - 功能：檢查科目是否為活動狀態
   - 參數：subjectCode - 科目代碼
   - 返回：是否為活動狀態

7. **GetRecentSubjects**：
   - 功能：取得最近使用的科目列表
   - 參數：count - 數量
   - 返回：最近使用的科目列表

8. **GetCommonSubjects**：
   - 功能：取得常用科目列表
   - 參數：count - 數量
   - 返回：常用科目列表

## 事件處理

控制項包含以下事件處理：

1. **SubjectSelected**：
   - 觸發條件：使用者選擇科目時
   - 事件參數：所選科目代碼、名稱、類型

2. **SubjectChanged**：
   - 觸發條件：所選科目變更時
   - 事件參數：新舊科目代碼、名稱、類型

3. **SubjectValidated**：
   - 觸發條件：科目驗證完成時
   - 事件參數：驗證結果、科目代碼

4. **SubjectSearched**：
   - 觸發條件：完成科目搜尋操作時
   - 事件參數：搜尋文字、結果數量

## 使用範例

以下是 Subjects 控制項的基本使用方式：

```html
<!-- 在 ASPX 頁面中引用控制項 -->
<uc:Subjects ID="accountSubject" runat="server" 
    RequiredSubject="true"
    EnableAutoComplete="true"
    AllowedSubjectTypes="Assets,Liabilities,Equities"
    CompanyId="<%=CurrentCompanyId %>"
    OnSubjectSelected="accountSubject_SubjectSelected" />

<!-- 在程式碼中操作控制項 -->
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 設定預設科目
            if (!string.IsNullOrEmpty(Request.QueryString["AccountCode"]))
            {
                accountSubject.SetSubject(Request.QueryString["AccountCode"]);
            }
        }
    }
    
    protected void accountSubject_SubjectSelected(object sender, SubjectEventArgs e)
    {
        // 處理科目選擇事件
        lblSelectedSubject.Text = string.Format(
            "已選擇科目: {0} - {1} ({2})",
            e.SubjectCode,
            e.SubjectName,
            e.SubjectType
        );
        
        // 根據科目類型執行不同處理
        if (e.SubjectType == "Assets")
        {
            // 處理資產類科目
            LoadAssetDetails(e.SubjectCode);
        }
        else if (e.SubjectType == "Liabilities")
        {
            // 處理負債類科目
            LoadLiabilityDetails(e.SubjectCode);
        }
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // 驗證科目
        if (accountSubject.ValidateSubject())
        {
            // 取得科目資訊
            string subjectCode = accountSubject.SelectedSubjectCode;
            string subjectName = accountSubject.SelectedSubjectName;
            
            // 進行後續處理
            SaveAccountData(subjectCode, subjectName);
        }
    }
</script>
```

## 異常處理

控制項包含以下異常處理機制：

1. **科目驗證錯誤**：
   - 當科目不存在或無效時顯示錯誤訊息
   - 提供視覺化的錯誤指示

2. **科目權限檢查**：
   - 當使用者無權存取特定科目時顯示警告
   - 限制僅能選擇有權限的科目

3. **科目狀態檢查**：
   - 檢查科目是否為活動狀態
   - 對非活動科目提供警告訊息

4. **科目類型限制**：
   - 檢查所選科目是否屬於允許的類型
   - 當科目類型不符合要求時顯示錯誤

## 效能考量

1. **科目資料快取**：
   - 使用資料快取減少資料庫查詢
   - 優化頻繁使用科目的載入速度

2. **延遲載入**：
   - 採用延遲載入策略，按需載入科目資料
   - 減少初始化時的資源消耗

3. **輸入優化**：
   - 使用輸入延遲處理，避免頻繁搜尋
   - 優化科目代碼格式的即時驗證

4. **資源管理**：
   - 合理管理控制項生命週期
   - 釋放不再使用的資源

## 安全性考量

1. **輸入驗證**：
   - 驗證科目代碼的格式和長度
   - 防止惡意輸入和腳本注入

2. **權限控制**：
   - 整合系統權限框架，限制科目選擇範圍
   - 只顯示使用者有權限的科目

3. **資料保護**：
   - 確保敏感科目資訊的安全處理
   - 避免不必要的科目詳細資訊暴露

4. **日誌記錄**：
   - 記錄重要科目操作的歷史
   - 便於追蹤科目使用情況和異常行為

## 測試記錄

測試項目包括：

1. **功能測試**：
   - 科目代碼輸入和驗證功能
   - 科目搜尋和選擇功能
   - 科目資訊顯示功能
   - 事件觸發和處理功能

2. **整合測試**：
   - 與資料庫的整合
   - 與科目相關服務的整合
   - 與父頁面的協作

3. **效能測試**：
   - 大量科目資料下的響應時間
   - 頻繁操作下的資源使用情況

4. **相容性測試**：
   - 不同瀏覽器環境下的表現
   - 與系統其他模組的相容性

## 待改進事項

1. 增強科目搜尋演算法，提供更智能的科目建議
2. 優化科目層級結構的視覺化顯示
3. 增加科目比較和歷史追蹤功能
4. 提供科目資料的匯出和報表功能
5. 增強行動裝置上的使用體驗
6. 提供更全面的科目使用統計和分析
7. 增加科目資料的版本控制功能
8. 優化大量科目資料下的載入效率
9. 擴展科目多語言支援
10. 增加科目變更通知和訂閱功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/11 | 1.0    | 首次建立科目控制項規格書              | Claude AI  |
``` 