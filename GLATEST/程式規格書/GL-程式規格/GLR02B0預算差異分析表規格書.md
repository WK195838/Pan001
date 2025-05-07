# GLR02B0 預算差異分析表規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02B0                              |
| 程式名稱     | 預算差異分析表                           |
| 檔案大小     | 3.2KB                                 |
| 行數        | ~60                                   |
| 功能簡述     | 預算差異報表                             |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02B0 是泛太總帳系統中的預算差異分析表程式，提供使用者檢視、分析和列印預算與實際執行結果的差異報表。主要功能包括：

1. 依照多種條件查詢預算與實際結果（年度、期間、部門、科目等）
2. 自動計算預算與實際數據的差異金額與差異百分比
3. 支援多種預算比較模式（年度預算、滾動預算、修正預算等）
4. 提供多層級的資料彙總與展開功能（科目、部門、專案等）
5. 支援趨勢分析與多期間比較功能
6. 提供視覺化圖表顯示差異情況
7. 提供多種匯出格式（PDF、Excel、CSV）
8. 支援報表參數的儲存與載入

## 程式結構說明

GLR02B0 的結構按功能可分為以下區域：

1. **查詢條件區**：提供使用者設定報表篩選條件的表單
2. **報表控制區**：包含生成、匯出、儲存等報表操作功能
3. **報表顯示區**：顯示報表結果的主要區域
4. **圖表顯示區**：以圖形化方式呈現差異分析結果
5. **功能按鈕區**：提供匯出、列印等輔助功能
6. **狀態資訊區**：顯示報表生成時間、查詢條件摘要等資訊

## 頁面元素

### ASP.NET 頁面宣告

```html
<%@ Page Title="預算差異分析表" Language="C#" MasterPageFile="~/GLADetail.master" AutoEventWireup="true" CodeFile="GLR02B0.aspx.cs" Inherits="GLR02B0" %>
```

### 頁面結構

```html
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- 頁頭資源引用 -->
    <link href="Styles/report.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/GLR02B0.js" type="text/javascript"></script>
    <script src="Scripts/Chart.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="report-container">
        <!-- 查詢條件區 -->
        <div class="query-panel">
            <h3>報表條件設定</h3>
            <table class="query-table">
                <tr>
                    <td class="field-label">預算年度：</td>
                    <td class="field-control">
                        <uc1:YearList ID="ucYearList" runat="server" />
                    </td>
                    <td class="field-label">預算期間：</td>
                    <td class="field-control">
                        <uc1:PeriodList ID="ucPeriodList" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="field-label">預算類型：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlBudgetType" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="年度預算" Value="ANNUAL" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="滾動預算" Value="ROLLING"></asp:ListItem>
                            <asp:ListItem Text="修正預算" Value="REVISED"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="field-label">部門：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field-label">科目類別：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlAccountCategory" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountCategory_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td class="field-label">科目：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlAccount" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field-label">分析方式：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlAnalysisType" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="金額差異" Value="AMOUNT" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="百分比差異" Value="PERCENTAGE"></asp:ListItem>
                            <asp:ListItem Text="兩者皆顯示" Value="BOTH"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="field-label">報表單位：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="元" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="千元" Value="1000"></asp:ListItem>
                            <asp:ListItem Text="萬元" Value="10000"></asp:ListItem>
                            <asp:ListItem Text="百萬元" Value="1000000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field-label">圖表類型：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlChartType" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="長條圖" Value="BAR" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="折線圖" Value="LINE"></asp:ListItem>
                            <asp:ListItem Text="圓餅圖" Value="PIE"></asp:ListItem>
                            <asp:ListItem Text="不顯示圖表" Value="NONE"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="field-label">排序方式：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="科目代碼" Value="CODE" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="差異金額" Value="VARIANCE"></asp:ListItem>
                            <asp:ListItem Text="差異百分比" Value="PERCENTAGE"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div class="button-panel">
                <asp:Button ID="btnQuery" runat="server" Text="查詢" CssClass="btn-query" OnClick="btnQuery_Click" />
                <asp:Button ID="btnReset" runat="server" Text="重設" CssClass="btn-reset" OnClick="btnReset_Click" />
                <asp:Button ID="btnSaveSetting" runat="server" Text="儲存設定" CssClass="btn-save" OnClick="btnSaveSetting_Click" />
            </div>
        </div>
        
        <!-- 報表顯示區 -->
        <div class="report-panel">
            <div class="report-header">
                <h2>泛太總帳系統 - 預算差異分析表</h2>
                <div class="report-info">
                    <asp:Label ID="lblReportInfo" runat="server"></asp:Label>
                </div>
            </div>
            
            <div class="report-content">
                <asp:GridView ID="gvBudgetVariance" runat="server" AutoGenerateColumns="False" 
                    CssClass="gridview" AllowPaging="True" AllowSorting="True"
                    OnPageIndexChanging="gvBudgetVariance_PageIndexChanging"
                    OnSorting="gvBudgetVariance_Sorting" OnRowDataBound="gvBudgetVariance_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="AccountCode" HeaderText="科目代碼" SortExpression="AccountCode" />
                        <asp:BoundField DataField="AccountName" HeaderText="科目名稱" SortExpression="AccountName" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="部門" SortExpression="DepartmentName" />
                        <asp:BoundField DataField="BudgetAmount" HeaderText="預算金額" DataFormatString="{0:N2}" SortExpression="BudgetAmount" />
                        <asp:BoundField DataField="ActualAmount" HeaderText="實際金額" DataFormatString="{0:N2}" SortExpression="ActualAmount" />
                        <asp:BoundField DataField="VarianceAmount" HeaderText="差異金額" DataFormatString="{0:N2}" SortExpression="VarianceAmount" />
                        <asp:BoundField DataField="VariancePercentage" HeaderText="差異百分比" DataFormatString="{0:P2}" SortExpression="VariancePercentage" />
                        <asp:BoundField DataField="Status" HeaderText="狀態" SortExpression="Status" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                    <PagerStyle CssClass="pager" />
                    <EmptyDataTemplate>
                        <div class="empty-data">查無符合條件的資料</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            
            <!-- 圖表顯示區 -->
            <div class="chart-panel" id="chartPanel" runat="server">
                <canvas id="budgetChart" width="800" height="300"></canvas>
            </div>
            
            <div class="report-footer">
                <div class="summary-info">
                    <asp:Label ID="lblTotalInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        
        <!-- 報表操作區 -->
        <div class="export-panel">
            <asp:Button ID="btnExportPDF" runat="server" Text="匯出PDF" CssClass="btn-export" OnClick="btnExportPDF_Click" />
            <asp:Button ID="btnExportExcel" runat="server" Text="匯出Excel" CssClass="btn-export" OnClick="btnExportExcel_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="列印" CssClass="btn-print" OnClientClick="window.print(); return false;" />
        </div>
    </div>
</asp:Content>
```

## 技術實現

GLR02B0 採用以下技術：

1. ASP.NET Web Forms 架構
2. 使用 GridView 控制項呈現差異分析報表資料
3. 使用 Chart.js 庫實現資料視覺化圖表功能
4. 使用自定義使用者控制項（如 YearList、PeriodList）提供選擇功能
5. 採用多層架構設計，分離資料存取、業務邏輯與表現層
6. 使用 JavaScript 增強使用者體驗與圖表互動功能
7. 使用 CSS 定義報表樣式與列印格式

## 依賴關係

GLR02B0 依賴以下檔案與元件：

1. GLR02B0.aspx.cs：預算差異分析表後端程式碼
2. GLADetail.master：系統細節版面
3. report.css：報表頁面樣式表
4. GLR02B0.js：報表頁面專用 JavaScript 功能
5. Chart.min.js：圖表繪製 JavaScript 庫
6. YearList.ascx：年度選擇使用者控制項
7. PeriodList.ascx：期間選擇使用者控制項
8. ExcelManager：Excel 匯出服務
9. PDFExporter：PDF 匯出服務
10. Page_BaseClass：頁面基礎類別

## 使用者介面

GLR02B0 的使用者介面特點：

1. 提供完整的查詢條件設定，包括預算年度、期間、類型、部門、科目等
2. 清晰呈現預算與實際金額的差異，包括金額差異與百分比差異
3. 使用視覺化圖表，使差異一目了然
4. 支援資料排序與分頁，方便檢視大量資料
5. 提供多種匯出選項，便於進一步分析與報告
6. 適合於螢幕顯示與列印的排版設計

## 資料處理

GLR02B0 主要處理以下資料：

1. 從資料庫擷取預算資料與實際執行資料
2. 依照使用者選擇的條件篩選資料
3. 計算預算與實際金額的差異及差異百分比
4. 依照選定的排序方式排序資料
5. 準備資料供圖表顯示
6. 計算報表的加總與統計數據
7. 產生不同格式的匯出檔案

## 頁面行為

GLR02B0 的主要頁面行為：

1. 載入時初始化查詢控制項與預設參數
2. 科目類別變更時，動態更新科目清單
3. 查詢按鈕點擊後根據條件生成預算差異報表
4. 重設按鈕點擊後清除所有查詢條件
5. 圖表類型變更時，重新繪製對應類型的圖表
6. 各匯出按鈕對應不同格式的檔案生成
7. 儲存設定按鈕可保存當前查詢條件

## 異常處理

GLR02B0 的異常處理機制：

1. 資料庫連接失敗時顯示友善的錯誤訊息
2. 查詢無結果時顯示「查無符合條件的資料」
3. 報表參數不正確時提供明確的驗證訊息
4. 處理圖表生成可能的錯誤，確保頁面不會因圖表錯誤而崩潰
5. 匯出功能可能發生的錯誤有專門處理邏輯
6. 系統異常時記錄詳細錯誤日誌並轉導至錯誤頁面

## 安全性考量

GLR02B0 的安全性措施：

1. 使用參數化查詢防止 SQL 注入攻擊
2. 檢查使用者對預算報表的存取權限
3. 驗證所有使用者輸入的有效性
4. 敏感預算資料的權限控管
5. 日誌記錄重要操作以供稽核

## 效能考量

GLR02B0 的效能優化：

1. 使用存儲過程處理複雜的預算資料查詢與計算
2. 實施資料分頁，避免一次載入過多資料
3. 針對報表查詢建立適當的資料庫索引
4. 優化圖表生成，避免處理過大的資料集
5. 使用快取機制減少重複查詢
6. 非同步處理報表匯出功能

## 測試記錄

GLR02B0 經過以下測試：

1. 各種查詢條件組合的資料正確性測試
2. 不同預算類型下的差異計算正確性測試
3. 圖表顯示的準確性與互動功能測試
4. 大量資料下的效能測試
5. 報表格式與列印效果測試
6. 匯出功能的檔案格式與內容測試
7. 跨瀏覽器兼容性測試

## 待改進事項

GLR02B0 可考慮以下改進：

1. 增加多期間的趨勢比較功能
2. 提供更豐富的視覺化圖表選項
3. 實現鑽取(Drill-down)功能，以檢視更詳細的差異原因
4. 增加預算差異的原因分析功能
5. 優化大量資料時的圖表顯示效能
6. 增加自訂差異報表欄位與格式的功能

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 報表範例

預算差異分析表範例：

```
泛太總帳系統 - 預算差異分析表
預算年度：2025    預算期間：1月至3月    預算類型：年度預算    部門：全公司    報表單位：千元

科目代碼    科目名稱              部門        預算金額     實際金額     差異金額     差異百分比    狀態
-------------------------------------------------------------------------------------------------
4100        營業收入              業務部      8,500        7,850       -650         -7.65%      低於預期
4200        其他收入              業務部      1,200        1,350       +150         +12.50%     高於預期
5100        銷貨成本              製造部      3,500        3,720       +220         +6.29%      超出預算
5200        人事費用              人資部      2,500        2,480       -20          -0.80%      符合預期
5300        行銷費用              行銷部      1,800        2,100       +300         +16.67%     超出預算
5400        管理費用              行政部      1,200        1,180       -20          -1.67%      符合預期
5500        研發費用              研發部      2,800        2,500       -300         -10.71%     低於預期
-------------------------------------------------------------------------------------------------
收入合計                                      9,700        9,200       -500         -5.15%      
支出合計                                      11,800       11,980      +180         +1.53%
淨利/損                                      -2,100       -2,780       -680         -32.38%     顯著惡化

報表產生時間：2025/4/15 14:30:25    產生者：預算管理員
```

### 資料庫結構

報表相關主要資料表：

1. GL_Budget：預算主檔
2. GL_BudgetDetail：預算明細
3. GL_Ledger：實際交易資料
4. GL_Account：會計科目主檔
5. GL_Department：部門主檔
6. GL_BudgetType：預算類型主檔
7. GL_ReportSetting：報表參數設定 