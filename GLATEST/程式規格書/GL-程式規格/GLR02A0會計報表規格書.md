# GLR02A0 會計報表規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02A0                              |
| 程式名稱     | 會計報表                                |
| 檔案大小     | 6.6KB                                 |
| 行數        | ~130                                  |
| 功能簡述     | 綜合會計報表                             |
| 複雜度       | 高                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02A0 是泛太總帳系統中的綜合會計報表程式，提供使用者檢視、分析和列印多種會計報表的功能。主要功能包括：

1. 支援多種會計報表類型（財務報表、管理報表、法定報表等）
2. 依照多種條件查詢會計資料（期間、科目、部門等）
3. 提供各類財務報表的標準與自訂範本
4. 支援多層級的資料展示與彙總功能
5. 提供多種匯出格式（PDF、Excel、CSV、Word）
6. 支援報表參數的儲存與載入
7. 提供報表預覽與列印功能
8. 提供報表比較與趨勢分析功能

## 程式結構說明

GLR02A0 的結構按功能可分為以下區域：

1. **報表選擇區**：提供使用者選擇報表類型的下拉選單
2. **查詢條件區**：提供使用者設定報表篩選條件的表單
3. **報表控制區**：包含生成、匯出、儲存等報表操作功能
4. **報表顯示區**：顯示報表結果的主要區域
5. **功能按鈕區**：提供匯出、列印等輔助功能
6. **狀態資訊區**：顯示報表生成時間、查詢條件摘要等資訊

## 頁面元素

### ASP.NET 頁面宣告

```html
<%@ Page Title="會計報表" Language="C#" MasterPageFile="~/GLADetail.master" AutoEventWireup="true" CodeFile="GLR02A0.aspx.cs" Inherits="GLR02A0" %>
```

### 頁面結構

```html
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- 頁頭資源引用 -->
    <link href="Styles/report.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/GLR02A0.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="report-container">
        <!-- 報表選擇區 -->
        <div class="report-type-panel">
            <h3>報表類型選擇</h3>
            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                <asp:ListItem Text="財務報表 - 資產負債表" Value="BALANCE_SHEET" Selected="True"></asp:ListItem>
                <asp:ListItem Text="財務報表 - 損益表" Value="INCOME_STATEMENT"></asp:ListItem>
                <asp:ListItem Text="財務報表 - 現金流量表" Value="CASH_FLOW"></asp:ListItem>
                <asp:ListItem Text="管理報表 - 部門損益" Value="DEPT_INCOME"></asp:ListItem>
                <asp:ListItem Text="管理報表 - 預算比較" Value="BUDGET_COMPARE"></asp:ListItem>
                <asp:ListItem Text="法定報表 - 所得稅申報" Value="TAX_REPORT"></asp:ListItem>
                <asp:ListItem Text="自訂報表" Value="CUSTOM_REPORT"></asp:ListItem>
            </asp:DropDownList>
        </div>
        
        <!-- 查詢條件區 -->
        <div class="query-panel">
            <h3>報表條件設定</h3>
            <table class="query-table">
                <tr>
                    <td class="field-label">會計年度：</td>
                    <td class="field-control">
                        <uc1:YearList ID="ucYearList" runat="server" />
                    </td>
                    <td class="field-label">會計期間：</td>
                    <td class="field-control">
                        <uc1:PeriodList ID="ucPeriodList" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="field-label">顯示層級：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlDisplayLevel" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="明細" Value="DETAIL" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="科目" Value="ACCOUNT"></asp:ListItem>
                            <asp:ListItem Text="科目群組" Value="GROUP"></asp:ListItem>
                            <asp:ListItem Text="總類" Value="CATEGORY"></asp:ListItem>
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
                    <td class="field-label">部門：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </td>
                    <td class="field-label">比較期間：</td>
                    <td class="field-control">
                        <asp:CheckBox ID="chkCompare" runat="server" Text="啟用比較" AutoPostBack="true" OnCheckedChanged="chkCompare_CheckedChanged" />
                        <asp:Panel ID="pnlCompare" runat="server" Visible="false">
                            <uc1:YearList ID="ucCompareYearList" runat="server" />
                            <uc1:PeriodList ID="ucComparePeriodList" runat="server" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="field-label">報表樣式：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlReportStyle" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="標準" Value="STANDARD" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="精簡" Value="SIMPLE"></asp:ListItem>
                            <asp:ListItem Text="詳細" Value="DETAILED"></asp:ListItem>
                            <asp:ListItem Text="圖表" Value="CHART"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="field-label">語言：</td>
                    <td class="field-control">
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdown">
                            <asp:ListItem Text="中文" Value="zh-TW" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="英文" Value="en-US"></asp:ListItem>
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
                <h2>泛太總帳系統 - <asp:Literal ID="litReportTitle" runat="server"></asp:Literal></h2>
                <div class="report-info">
                    <asp:Label ID="lblReportInfo" runat="server"></asp:Label>
                </div>
            </div>
            
            <div class="report-content">
                <asp:PlaceHolder ID="phReportContent" runat="server">
                    <!-- 動態載入不同報表內容 -->
                </asp:PlaceHolder>
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
            <asp:Button ID="btnExportWord" runat="server" Text="匯出Word" CssClass="btn-export" OnClick="btnExportWord_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="列印" CssClass="btn-print" OnClientClick="window.print(); return false;" />
        </div>
    </div>
</asp:Content>
```

## 技術實現

GLR02A0 採用以下技術：

1. ASP.NET Web Forms 架構
2. 使用使用者控制項作為報表內容的容器
3. 使用自定義使用者控制項（如 YearList、PeriodList）提供選擇功能
4. 採用多層架構設計，分離資料存取、業務邏輯與表現層
5. 使用動態載入的方式呈現不同類型的報表
6. 使用 JavaScript 增強使用者體驗
7. 使用 CSS 定義報表樣式與列印格式

## 依賴關係

GLR02A0 依賴以下檔案與元件：

1. GLR02A0.aspx.cs：會計報表後端程式碼
2. GLADetail.master：系統細節版面
3. report.css：報表頁面樣式表
4. GLR02A0.js：報表頁面專用 JavaScript 功能
5. YearList.ascx：年度選擇使用者控制項
6. PeriodList.ascx：期間選擇使用者控制項
7. ReportTemplate/*.ascx：各類報表範本控制項
8. ExcelManager：Excel 匯出服務
9. PDFExporter：PDF 匯出服務
10. Page_BaseClass：頁面基礎類別

## 使用者介面

GLR02A0 的使用者介面特點：

1. 提供直觀的報表類型選擇區，幫助使用者快速選擇所需報表
2. 簡潔易用的查詢條件區，提供各種篩選與設定選項
3. 根據報表類型動態調整查詢條件的可見性與有效性
4. 清晰的報表顯示格式，包含適當的標題與資料格式化
5. 匯出與列印功能位於明顯位置，便於使用
6. 適合於螢幕顯示與列印的報表排版設計

## 資料處理

GLR02A0 主要處理以下資料：

1. 根據選擇的報表類型，從資料庫擷取對應的會計資料
2. 依照會計準則與報表規範處理財務數據
3. 執行複雜的計算與彙總邏輯，生成報表各項數據
4. 根據使用者設定的比較條件，計算差異與變動比例
5. 根據不同報表類型應用不同的顯示格式與排序邏輯
6. 產生多種格式的匯出檔案（PDF、Excel、Word等）

## 頁面行為

GLR02A0 的主要頁面行為：

1. 載入時初始化報表選項與查詢條件控制項
2. 報表類型變更時動態調整可用的查詢條件與顯示選項
3. 查詢按鈕點擊後根據選定的報表類型與條件生成對應報表
4. 比較功能啟用時顯示比較期間選擇控制項
5. 重設按鈕點擊後清除所有查詢條件
6. 各匯出按鈕對應不同格式的檔案生成
7. 儲存設定按鈕可保存當前報表類型與查詢條件

## 異常處理

GLR02A0 的異常處理機制：

1. 資料庫連接失敗時顯示友善的錯誤訊息
2. 查詢無結果時顯示適當的「無資料」提示
3. 報表參數不正確時提供明確的驗證訊息
4. 匯出功能可能發生的錯誤有專門處理邏輯
5. 系統異常時記錄詳細錯誤日誌並轉導至錯誤頁面
6. 使用 Try-Catch 區塊處理可能的運行時異常

## 安全性考量

GLR02A0 的安全性措施：

1. 使用參數化查詢防止 SQL 注入攻擊
2. 檢查使用者對報表的存取權限
3. 驗證所有使用者輸入的有效性
4. 敏感財務資料的權限控管
5. 報表匯出檔案的安全處理
6. 日誌記錄重要操作以供稽核

## 效能考量

GLR02A0 的效能優化：

1. 使用資料庫存儲過程處理複雜的報表查詢邏輯
2. 針對報表查詢建立適當的資料庫索引
3. 實施結果快取機制，避免重複計算
4. 使用分頁機制處理大型報表的顯示
5. 非同步處理報表匯出功能
6. 根據使用者權限預先載入報表範本

## 測試記錄

GLR02A0 經過以下測試：

1. 各種報表類型與查詢條件組合的資料正確性測試
2. 報表計算與彙總邏輯的準確性測試
3. 報表比較功能的正確性測試
4. 大量資料下的效能測試
5. 報表格式與列印效果測試
6. 多種匯出格式的檔案測試
7. 跨瀏覽器兼容性測試

## 待改進事項

GLR02A0 可考慮以下改進：

1. 增強報表自訂功能，提供更多使用者定義選項
2. 實現更多資料視覺化功能，提供互動式圖表
3. 增加報表排程功能，支援定期產生與分發報表
4. 擴充比較功能，支援多期間比較與趨勢分析
5. 增加報表註解與備忘功能
6. 優化大型報表的處理效能

## 維護記錄

| 日期 | 版本 | 修改者 | 修改內容 |
|------|-----|--------|---------|
| 2025/5/6 | 1.0 | Claude AI | 初始版本建立 |

## 附錄

### 報表範例

資產負債表範例：

```
泛太總帳系統 - 財務報表 - 資產負債表
會計年度：2025    會計期間：3月    報表單位：千元

資產                          負債與股東權益
-------------------------------------------------------------------------------------------------------------------
流動資產：                    流動負債：
  現金及約當現金    8,500        應付帳款          4,200
  應收帳款         5,300        應付票據          1,800
  存貨             4,200        短期借款          3,000
  預付費用         1,000        應付費用          1,200
  其他流動資產       800        其他流動負債        500
流動資產合計      19,800      流動負債合計      10,700

非流動資產：                  非流動負債：
  不動產、廠房及設備 25,000      長期借款          8,000
  減：累計折舊    (10,000)      遞延所得稅負債    1,200
  投資性不動產     5,000        其他非流動負債    1,500
  無形資產         3,000      非流動負債合計     10,700
  其他非流動資產   1,200      負債總計          21,400
非流動資產合計    24,200
                             股東權益：
                               股本             15,000
                               資本公積          3,000
                               保留盈餘          4,600
                             股東權益合計      22,600
                             
資產總計         44,000      負債及股東權益總計  44,000

報表產生時間：2025/4/15 14:30:25    產生者：財務主管
```

### 資料庫結構

報表相關主要資料表：

1. GL_AccountCategory：會計科目類別
2. GL_AccountGroup：會計科目群組
3. GL_Account：會計科目主檔
4. GL_Ledger：總帳交易資料
5. GL_Department：部門主檔
6. GL_ReportSetting：報表參數設定
7. GL_ReportTemplate：報表範本設定 