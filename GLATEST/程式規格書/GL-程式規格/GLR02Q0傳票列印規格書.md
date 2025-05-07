# GLR02Q0 傳票列印規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02Q0                              |
| 程式名稱     | 傳票列印                               |
| 檔案大小     | 4.7KB                                 |
| 行數        | ~90                                   |
| 功能簡述     | 傳票列印報表                             |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02Q0 是泛太總帳系統中的傳票列印程式，提供使用者依據選定的會計年度、會計期間和公司別，產生並列印傳票列印報表。該報表主要顯示特定期間內的傳票張數統計數據，用於會計管理和審核。主要功能包括：

1. 依照公司別、會計年度和會計期間查詢傳票資料
2. 統計各日期的傳票張數
3. 計算期間內傳票總張數
4. 提供報表查詢和列印功能
5. 支援 Crystal Report 報表展示

## 程式結構說明

GLR02Q0 的結構按功能可分為以下區域：

1. **查詢條件區**：提供使用者設定報表篩選條件的表單
2. **報表控制區**：控制報表顯示和操作的功能
3. **報表顯示區**：使用 Crystal Report Viewer 顯示報表結果
4. **後端邏輯處理區**：處理資料查詢和報表參數設定

## 頁面元素

### ASP.NET 頁面宣告

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR02Q0.aspx.cs" Inherits="GLR02Q0" MasterPageFile="~/GLA.master" %>
```

### 頁面結構

頁面包含以下主要部分：

1. **引用相關控制項和元件**：
   - StyleContentStart 和 StyleTitle 自定義控制項
   - CrystalDecisions.Web 命名空間的 Crystal Report 相關控制項
   - System.Web.Extensions 命名空間的 ASP.NET AJAX 元件

2. **查詢條件區**：
   - 公司別下拉選單 (Drpcompany)
   - 會計年度下拉選單 (DrpAcctYear)
   - 會計期間下拉選單 (DrpAcctperiod)
   - 查詢按鈕 (btnQuery)

3. **報表顯示區**：
   - Crystal Report Viewer (CrystalReportViewer1)
   - Crystal Report Source (CryReportSource)，連結到 GLR02Q0.rpt 報表檔案

## 技術實現

GLR02Q0 採用以下技術：

1. ASP.NET Web Forms 架構
2. Crystal Reports 報表元件 (Version 13.0.2000.0)
3. ASP.NET AJAX 技術提升使用者體驗
4. SQL Server 資料庫存取技術
5. 客戶端 JavaScript 功能（等待畫面顯示）

## 依賴關係

GLR02Q0 依賴以下檔案與元件：

1. GLR02Q0.aspx.cs：傳票列印後端程式碼
2. GLR02Q0.rpt：Crystal Reports 報表設計檔
3. GLA.master：系統主版面
4. Busy.js：等待畫面 JavaScript 函數
5. pagefunction.js：頁面通用 JavaScript 函數
6. StyleTitle.ascx：標題樣式使用者控制項
7. PanPacificClass 命名空間中的類別：
   - UserInfo：使用者資訊類別
   - DBManger：資料庫管理類別
   - JsUtility：JavaScript 工具類別

## 使用者介面

GLR02Q0 的使用者介面主要由以下部分組成：

1. **標題區**：顯示「傳票張數統計」標題
2. **查詢條件區**：
   - 公司別：選擇要查詢的公司
   - 會計年度：選擇會計年度
   - 會計期間：選擇會計期間（01-13）
   - 查詢按鈕：執行查詢並生成報表
3. **報表顯示區**：Crystal Report Viewer 顯示查詢結果

## 資料處理

GLR02Q0 主要處理以下資料：

1. **查詢參數處理**：
   - 從下拉式選單獲取公司別、會計年度和會計期間
   - 驗證輸入參數的有效性

2. **報表參數設定**：
   - 公司名稱 (CompanyName)
   - 關帳註記 (CloseRemark)
   - 會計年度 (AcctYear)
   - 開始日期 (StartDate)
   - 結束日期 (EndDate)
   - 報表名稱 (ReportName)
   - 會計期間 (FromPeriod)

3. **資料查詢**：
   - 執行 SQL 查詢取得傳票日期和張數統計
   - 使用參數化查詢防止 SQL 注入

4. **報表資料處理**：
   - 將查詢結果綁定到 Crystal Report
   - 儲存查詢結果到 ViewState 中，以便於分頁時使用

## 頁面行為

頁面載入時：
1. 初始化頁面控制項
2. 載入公司別下拉式選單
3. 預設選擇第一個公司別
4. 載入對應的會計年度

查詢按鈕點擊時：
1. 顯示等待畫面
2. 驗證查詢條件
3. 取得會計期間的開始和結束日期
4. 設定報表參數
5. 執行 SQL 查詢取得傳票統計數據
6. 將結果綁定到 Crystal Report 顯示
7. 關閉等待畫面

## 異常處理

程式包含以下異常處理機制：

1. **輸入驗證**：
   - 檢查會計年度是否為有效數字範圍 (1900-2099)
   - 當輸入無效時顯示錯誤訊息並終止查詢

2. **查詢結果檢查**：
   - 當查詢結果為空時，顯示「查無相關資料」訊息
   - 隱藏 Crystal Report Viewer 以避免顯示空白報表

3. **使用者提醒**：
   - 使用 JsUtility.ClientMsgBoxAjax 顯示錯誤訊息
   - 使用 JsUtility.CloseWaitScreenAjax 關閉等待畫面

## 安全性考量

1. **參數化查詢**：
   - 使用 SqlCommand 參數化查詢，防止 SQL 注入攻擊
   - 避免直接拼接使用者輸入到 SQL 查詢字串

2. **輸入驗證**：
   - 對用戶輸入進行有效性檢查，防止無效數據

3. **權限控制**：
   - 透過主版面的使用者權限系統控制頁面存取

## 效能考量

1. **資料查詢優化**：
   - 使用條件過濾 (VoucherDate between @StartDate and @endDate)
   - 使用資料分組 (GROUP BY VoucherDate) 減少傳輸資料量

2. **AJAX 更新面板**：
   - 使用 UpdatePanel 實現部分頁面更新，減少頁面重新載入
   - 使用 ScriptManager 管理 AJAX 資源

3. **資料暫存**：
   - 使用 ViewState 保存查詢結果，優化分頁體驗

## 測試記錄

測試項目包括：

1. **功能測試**：
   - 不同公司別和會計期間的查詢結果正確性
   - 報表參數傳遞正確性
   - 空結果處理正確性

2. **介面測試**：
   - 元件布局和樣式正確性
   - 等待畫面顯示和關閉正確性
   - 錯誤訊息顯示正確性

3. **效能測試**：
   - 大量資料查詢響應時間
   - 報表渲染速度

## 待改進事項

1. 優化報表顯示格式，增加更多視覺化元素
2. 增加更多篩選條件，如傳票類型和製單人員
3. 增加匯出功能，支援更多檔案格式
4. 優化大量資料查詢的性能
5. 增加報表參數儲存和載入功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/6  | 1.0    | 首次建立傳票列印規格書                | Claude AI | 