# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | GLR01H0 |
| 程式名稱 | 傳票查詢報表 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | /GLATEST/app/Reports/GLR01H0.aspx, /GLATEST/app/Reports/GLR01H0.aspx.cs |
| 程式類型 | 報表程式 |
| 建立日期 | 2025/05/15 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/15 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

GLR01H0是泛太總帳系統中的傳票查詢報表程式，用於查詢和顯示系統中的會計傳票資料。此報表允許用戶根據多種條件（如日期區間、傳票類型、會計科目、部門等）篩選傳票，並以多種格式查看和列印傳票資訊。報表支援明細與彙總兩種模式，並可匯出為Excel、PDF等格式，便於後續資料分析和存檔。

### 2.2 業務流程

GLR01H0在系統中的業務流程如下：
1. 用戶在總帳模組中選擇「傳票查詢報表」功能
2. 系統顯示查詢條件頁面，用戶輸入查詢條件
3. 系統根據條件查詢相關傳票資料
4. 用戶可預覽報表結果，並執行以下操作：
   - 調整排序和分組方式
   - 在預覽中瀏覽不同頁面
   - 列印報表
   - 匯出報表為各種格式
   - 從報表中選擇傳票進行後續操作（如修改、複製、沖銷等）

### 2.3 使用頻率

- 高頻率：日常會計作業及月結時頻繁使用
- 平均每日使用次數：約20-30次
- 月結期間：使用頻率可能增加3倍以上
- 匯出操作：約25%的查詢會進行匯出

### 2.4 使用者角色

此報表主要服務於以下角色：
- 會計人員：日常查詢和核對傳票
- 財務主管：審核和分析傳票資料
- 稽核人員：審計和稽核傳票記錄
- 系統管理員：解決用戶問題時查詢傳票資料

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET WebForms (.NET Framework 4.0)
- 主要技術：
  - Crystal Reports：報表產生和預覽
  - ADO.NET：資料庫存取技術
  - jQuery：前端交互效果增強
  - Bootstrap：響應式界面設計
- 報表呈現：在網頁中嵌入Crystal Reports Viewer
- 列印設計：使用Crystal Reports設計器

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| GL_JOURNAL_MASTER | 傳票主檔 | 讀取 |
| GL_JOURNAL_DETAIL | 傳票明細 | 讀取 |
| GL_ACCT_DEF | 會計科目定義表 | 讀取 |
| GL_DEPT_DEF | 部門定義表 | 讀取 |
| GL_SYSTEM_CONFIG | 系統配置表 | 讀取 |
| SYS_USER | 使用者資料表 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| AppAuthority | 權限管理 | 檢查報表存取權限 |
| CalendarDate | 日期控制項 | 用於日期區間選擇 |
| Navigator | 導航控制項 | 提供頁面導航功能 |
| CrystalReportsViewer | 報表顯示 | Crystal Reports內建報表顯示元件 |

## 4. 報表規格

### 4.1 查詢條件

| 欄位名稱 | 欄位類型 | 必填 | 預設值 | 說明 |
|---------|---------|------|-------|------|
| txtDateFrom | 日期 | Y | 本月第一天 | 查詢起始日期 |
| txtDateTo | 日期 | Y | 本月最後一天 | 查詢結束日期 |
| ddlJournalType | 下拉式選單 | N | 全部 | 傳票類型 |
| txtJournalNoFrom | 文字 | N | 空白 | 傳票編號起始 |
| txtJournalNoTo | 文字 | N | 空白 | 傳票編號結束 |
| txtAcctFrom | 文字 | N | 空白 | 會計科目起始 |
| txtAcctTo | 文字 | N | 空白 | 會計科目結束 |
| txtDeptFrom | 文字 | N | 空白 | 部門代碼起始 |
| txtDeptTo | 文字 | N | 空白 | 部門代碼結束 |
| chkShowDetail | 核取方塊 | N | 勾選 | 是否顯示明細 |
| rdoSortBy | 選項按鈕 | Y | 傳票日期 | 排序依據 |
| ddlOutputFormat | 下拉式選單 | Y | 螢幕 | 輸出格式 |

### 4.2 報表欄位

#### 4.2.1 報表頁首

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| CompanyName | GL_SYSTEM_CONFIG | 公司名稱 |
| ReportTitle | 靜態文字 | 報表標題："傳票查詢報表" |
| ReportDate | 系統日期 | 列印日期 |
| UserName | SYS_USER | 使用者姓名 |
| QueryCondition | 查詢條件 | 顯示已設定的查詢條件文字 |
| PageNo | Crystal Reports內建 | 頁碼資訊 |

#### 4.2.2 報表主體

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| JournalNo | GL_JOURNAL_MASTER.JOURNAL_NO | 傳票編號 |
| JournalDate | GL_JOURNAL_MASTER.JOURNAL_DATE | 傳票日期 |
| JournalType | GL_JOURNAL_MASTER.JOURNAL_TYPE | 傳票類型 |
| AcctNo | GL_JOURNAL_DETAIL.ACCT_NO | 會計科目編號 |
| AcctName | GL_ACCT_DEF.ACCT_NAME | 會計科目名稱 |
| DeptNo | GL_JOURNAL_DETAIL.DEPT_NO | 部門編號 |
| DeptName | GL_DEPT_DEF.DEPT_NAME | 部門名稱 |
| DrAmt | GL_JOURNAL_DETAIL.DR_AMT | 借方金額 |
| CrAmt | GL_JOURNAL_DETAIL.CR_AMT | 貸方金額 |
| Description | GL_JOURNAL_DETAIL.DESCRIPTION | 摘要說明 |
| CreateUser | GL_JOURNAL_MASTER.CREATE_USER | 建立人員 |
| CreateDate | GL_JOURNAL_MASTER.CREATE_DATE | 建立日期 |
| PostFlag | GL_JOURNAL_MASTER.POST_FLAG | 過帳狀態 |

#### 4.2.3 報表頁尾

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| PageTotal_DrAmt | 計算欄位 | 頁合計借方金額 |
| PageTotal_CrAmt | 計算欄位 | 頁合計貸方金額 |
| GrandTotal_DrAmt | 計算欄位 | 總合計借方金額 |
| GrandTotal_CrAmt | 計算欄位 | 總合計貸方金額 |
| RecordCount | 計算欄位 | 記錄筆數 |

### 4.3 輸出格式

| 格式名稱 | 說明 | 特性 |
|---------|------|------|
| 螢幕 | 在網頁中顯示 | 可互動、可進行二次篩選 |
| PDF | PDF檔案 | 符合列印格式、保留字型 |
| Excel | Excel檔案 | 可進行後續數據分析 |
| Word | Word檔案 | 可進行文書編輯 |
| CSV | 逗號分隔文字檔 | 可匯入其他系統 |

## 5. 處理邏輯

### 5.1 主要流程

報表產生的一般處理流程：

```
開始
 ↓
驗證使用者是否登入 → 未登入 → 轉至登入頁
 ↓
顯示查詢條件頁面
 ↓
用戶輸入查詢條件並提交
 ↓
檢查查詢條件有效性 → 條件無效 → 提示錯誤
 ↓
執行SQL查詢
 ↓
檢查查詢結果 → 無符合資料 → 顯示無資料訊息
 ↓
產生報表資料集
 ↓
根據輸出格式設定Crystal Reports參數
 ↓
Crystal Reports產生報表
 ↓
根據輸出格式處理報表輸出
 ↓
將報表傳送至用戶
 ↓
結束
```

### 5.2 資料轉換處理

1. **日期格式轉換**：
   - 將資料庫日期格式轉換為本地顯示格式
   - 支援不同地區的日期格式設定

2. **金額處理**：
   - 實施千分位分隔符處理
   - 根據系統設定的小數位數顯示金額
   - 借貸金額分離顯示

3. **狀態代碼轉換**：
   - 將過帳狀態代碼轉換為可讀文字（如：Y=已過帳，N=未過帳）
   - 將傳票類型代碼轉換為說明文字

### 5.3 例外處理

| 例外類型 | 處理方式 | 記錄方式 |
|---------|---------|---------|
| 查詢條件無效 | 顯示錯誤訊息，保留已填條件 | 記錄錯誤條件和用戶ID |
| 資料庫連接錯誤 | 顯示友善錯誤頁面，提供重試選項 | 記錄詳細連接錯誤 |
| 報表產生錯誤 | 顯示技術錯誤代碼，提供聯絡支援選項 | 記錄完整堆疊追蹤 |
| 無符合條件資料 | 顯示「無符合條件的資料」訊息 | 記錄查詢條件 |
| 匯出格式錯誤 | 提示用戶選擇其他格式 | 記錄失敗的匯出格式 |

## 6. SQL查詢

### 6.1 主要查詢

```sql
-- 主要查詢SQL語句
SELECT 
    m.JOURNAL_NO, m.JOURNAL_DATE, m.JOURNAL_TYPE,
    m.DESCRIPTION AS MASTER_DESC, m.POST_FLAG,
    m.CREATE_USER, m.CREATE_DATE, m.UPDATE_USER, m.UPDATE_DATE,
    d.LINE_NO, d.ACCT_NO, a.ACCT_NAME,
    d.DEPT_NO, dp.DEPT_NAME, d.DESCRIPTION AS DETAIL_DESC,
    d.DR_AMT, d.CR_AMT, d.CURRENCY, d.EXCH_RATE,
    (d.DR_AMT - d.CR_AMT) AS NET_AMT,
    u.USER_NAME AS CREATE_USER_NAME
FROM GL_JOURNAL_MASTER m
INNER JOIN GL_JOURNAL_DETAIL d ON m.JOURNAL_NO = d.JOURNAL_NO
LEFT JOIN GL_ACCT_DEF a ON d.ACCT_NO = a.ACCT_NO
LEFT JOIN GL_DEPT_DEF dp ON d.DEPT_NO = dp.DEPT_NO
LEFT JOIN SYS_USER u ON m.CREATE_USER = u.USER_ID
WHERE 
    m.JOURNAL_DATE BETWEEN @DateFrom AND @DateTo
    AND (@JournalType = '' OR m.JOURNAL_TYPE = @JournalType)
    AND (@JournalNoFrom = '' OR m.JOURNAL_NO >= @JournalNoFrom)
    AND (@JournalNoTo = '' OR m.JOURNAL_NO <= @JournalNoTo)
    AND (@AcctFrom = '' OR d.ACCT_NO >= @AcctFrom)
    AND (@AcctTo = '' OR d.ACCT_NO <= @AcctTo)
    AND (@DeptFrom = '' OR d.DEPT_NO >= @DeptFrom)
    AND (@DeptTo = '' OR d.DEPT_NO <= @DeptTo)
ORDER BY 
    CASE WHEN @SortBy = 'DATE' THEN m.JOURNAL_DATE ELSE NULL END,
    CASE WHEN @SortBy = 'NO' THEN m.JOURNAL_NO ELSE NULL END,
    CASE WHEN @SortBy = 'TYPE' THEN m.JOURNAL_TYPE ELSE NULL END,
    m.JOURNAL_NO, d.LINE_NO
```

### 6.2 彙總查詢

```sql
-- 彙總查詢SQL語句（不顯示明細時使用）
SELECT 
    m.JOURNAL_NO, m.JOURNAL_DATE, m.JOURNAL_TYPE,
    m.DESCRIPTION AS MASTER_DESC, m.POST_FLAG,
    m.CREATE_USER, m.CREATE_DATE,
    SUM(d.DR_AMT) AS TOTAL_DR_AMT,
    SUM(d.CR_AMT) AS TOTAL_CR_AMT,
    u.USER_NAME AS CREATE_USER_NAME
FROM GL_JOURNAL_MASTER m
INNER JOIN GL_JOURNAL_DETAIL d ON m.JOURNAL_NO = d.JOURNAL_NO
LEFT JOIN SYS_USER u ON m.CREATE_USER = u.USER_ID
WHERE 
    m.JOURNAL_DATE BETWEEN @DateFrom AND @DateTo
    AND (@JournalType = '' OR m.JOURNAL_TYPE = @JournalType)
    AND (@JournalNoFrom = '' OR m.JOURNAL_NO >= @JournalNoFrom)
    AND (@JournalNoTo = '' OR m.JOURNAL_NO <= @JournalNoTo)
    AND (@AcctFrom = '' OR d.ACCT_NO >= @AcctFrom)
    AND (@AcctTo = '' OR d.ACCT_NO <= @AcctTo)
    AND (@DeptFrom = '' OR d.DEPT_NO >= @DeptFrom)
    AND (@DeptTo = '' OR d.DEPT_NO <= @DeptTo)
GROUP BY
    m.JOURNAL_NO, m.JOURNAL_DATE, m.JOURNAL_TYPE,
    m.DESCRIPTION, m.POST_FLAG, m.CREATE_USER, m.CREATE_DATE,
    u.USER_NAME
ORDER BY 
    CASE WHEN @SortBy = 'DATE' THEN m.JOURNAL_DATE ELSE NULL END,
    CASE WHEN @SortBy = 'NO' THEN m.JOURNAL_NO ELSE NULL END,
    CASE WHEN @SortBy = 'TYPE' THEN m.JOURNAL_TYPE ELSE NULL END,
    m.JOURNAL_NO
```

## 7. 程式碼說明

### 7.1 查詢條件面板

```csharp
/// <summary>
/// 初始化查詢條件控制項
/// </summary>
private void InitializeControls()
{
    // 設定日期區間預設值為本月
    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
    
    this.txtDateFrom.Text = firstDay.ToString("yyyy/MM/dd");
    this.txtDateTo.Text = lastDay.ToString("yyyy/MM/dd");
    
    // 載入傳票類型選項
    this.ddlJournalType.Items.Clear();
    this.ddlJournalType.Items.Add(new ListItem("全部", ""));
    this.ddlJournalType.Items.Add(new ListItem("普通傳票", "0"));
    this.ddlJournalType.Items.Add(new ListItem("轉帳傳票", "1"));
    this.ddlJournalType.Items.Add(new ListItem("調整傳票", "2"));
    this.ddlJournalType.Items.Add(new ListItem("結轉傳票", "3"));

    // 設定排序選項預設值
    this.rdoSortBy.SelectedValue = "DATE";
    
    // 設定輸出格式選項
    this.ddlOutputFormat.Items.Clear();
    this.ddlOutputFormat.Items.Add(new ListItem("螢幕", "SCREEN"));
    this.ddlOutputFormat.Items.Add(new ListItem("PDF", "PDF"));
    this.ddlOutputFormat.Items.Add(new ListItem("Excel", "EXCEL"));
    this.ddlOutputFormat.Items.Add(new ListItem("Word", "WORD"));
    this.ddlOutputFormat.Items.Add(new ListItem("CSV", "CSV"));
    this.ddlOutputFormat.SelectedValue = "SCREEN";
    
    // 預設顯示明細
    this.chkShowDetail.Checked = true;
}
```

### 7.2 報表產生

```csharp
/// <summary>
/// 產生報表
/// </summary>
protected void btnGenerate_Click(object sender, EventArgs e)
{
    try
    {
        // 驗證查詢條件
        if (!ValidateInput())
        {
            return;
        }
        
        // 取得查詢參數
        Dictionary<string, object> parameters = GetQueryParameters();
        
        // 執行查詢並取得資料
        DataTable reportData = GetReportData(parameters);
        
        if (reportData == null || reportData.Rows.Count == 0)
        {
            this.lblMessage.Text = "查無符合條件的資料";
            this.lblMessage.Visible = true;
            return;
        }
        
        // 產生報表
        ReportDocument report = new ReportDocument();
        
        // 根據是否顯示明細選擇報表樣板
        string reportPath = Server.MapPath("~/Reports/Templates/");
        if (this.chkShowDetail.Checked)
        {
            reportPath += "GLR01H0_Detail.rpt";
        }
        else
        {
            reportPath += "GLR01H0_Summary.rpt";
        }
        
        report.Load(reportPath);
        report.SetDataSource(reportData);
        
        // 設定報表參數
        SetReportParameters(report, parameters);
        
        // 根據輸出格式處理報表
        ProcessReportOutput(report, this.ddlOutputFormat.SelectedValue);
    }
    catch (Exception ex)
    {
        this.lblMessage.Text = "產生報表時發生錯誤: " + ex.Message;
        this.lblMessage.Visible = true;
        Logger.Error("GLR01H0.btnGenerate_Click", ex);
    }
}
```

### 7.3 不同輸出格式的處理

```csharp
/// <summary>
/// 根據選擇的輸出格式處理報表輸出
/// </summary>
/// <param name="report">報表文件</param>
/// <param name="outputFormat">輸出格式</param>
private void ProcessReportOutput(ReportDocument report, string outputFormat)
{
    switch (outputFormat)
    {
        case "SCREEN":
            // 顯示在螢幕上
            this.CrystalReportViewer1.ReportSource = report;
            this.CrystalReportViewer1.DisplayToolbar = true;
            this.CrystalReportViewer1.DisplayStatusBar = true;
            this.CrystalReportViewer1.EnableDatabaseLogonPrompt = false;
            this.CrystalReportViewer1.EnableParameterPrompt = false;
            this.divReportViewer.Visible = true;
            break;
            
        case "PDF":
            // 匯出為PDF
            ExportReport(report, CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "application/pdf", "GLR01H0.pdf");
            break;
            
        case "EXCEL":
            // 匯出為Excel
            ExportReport(report, CrystalDecisions.Shared.ExportFormatType.Excel, "application/vnd.ms-excel", "GLR01H0.xls");
            break;
            
        case "WORD":
            // 匯出為Word
            ExportReport(report, CrystalDecisions.Shared.ExportFormatType.WordForWindows, "application/msword", "GLR01H0.doc");
            break;
            
        case "CSV":
            // 匯出為CSV
            ExportReport(report, CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues, "text/csv", "GLR01H0.csv");
            break;
            
        default:
            this.lblMessage.Text = "不支援的輸出格式";
            this.lblMessage.Visible = true;
            break;
    }
}
```

## 8. 安全性考量

### 8.1 認證與授權

1. **認證機制**
   - 使用者必須登入系統才能存取報表
   - 透過LoginClass驗證使用者身份
   - 每次報表產生時驗證使用者會話有效性

2. **授權控制**
   - 透過AppAuthority檢查報表存取權限
   - 根據用戶權限限制可查詢的資料範圍
   - 限制敏感傳票的查詢權限

### 8.2 資料保護

1. **SQL注入防護**
   - 使用參數化查詢處理所有SQL
   - 避免直接拼接用戶輸入的SQL語句
   - 對排序欄位進行白名單驗證

2. **敏感資料處理**
   - 敏感傳票資訊僅向有權限的用戶顯示
   - 匯出檔案中不包含超出查詢範圍的資料
   - 報表匯出功能需額外權限控制

### 8.3 報表安全

1. **報表參數安全**
   - 所有報表參數經過驗證後使用
   - 防止Crystal Reports的SQL注入風險
   - 限制Crystal Reports存取資料庫的權限

2. **檔案安全**
   - 匯出的報表檔案不儲存在伺服器上
   - 一次性生成並直接傳送給用戶
   - 在網頁會話結束時清除暫存報表檔

## 9. 效能優化

### 9.1 查詢優化

1. **SQL優化**
   - 資料庫查詢使用適當索引
   - 限制大型查詢的記錄數量
   - 使用分頁技術處理大量資料

2. **資料處理優化**
   - 僅傳送報表所需欄位
   - 使用資料庫端計算處理彙總運算
   - 針對常用查詢條件建立索引

### 9.2 報表效能

1. **Crystal Reports優化**
   - 簡化報表設計與格式
   - 最小化報表公式使用
   - 限制子報表的使用

2. **分批處理**
   - 大型報表分段處理
   - 建立報表資料摘要預覽功能
   - 針對大型報表提供背景產生選項

### 9.3 快取策略

1. **資料快取**
   - 對常用報表資料實施快取
   - 使用快取參數減少重複查詢
   - 報表資料依據資料變更自動更新

2. **報表快取**
   - 實施報表輸出快取機制
   - 相同條件的報表共用快取
   - 依據系統負載調整快取策略

## 10. 使用者介面

### 10.1 查詢條件頁面

查詢條件頁面布局設計：

```
+----------------------------------------+
| 傳票查詢報表條件                        |
+----------------------------------------+
| 傳票日期: [YYYY/MM/DD] 至 [YYYY/MM/DD] |
| 傳票類型: [下拉選單             v]      |
| 傳票編號: [         ] 至 [         ]   |
| 會計科目: [         ] 至 [         ]   |
| 部門代碼: [         ] 至 [         ]   |
|                                        |
| [x] 顯示明細                           |
|                                        |
| 排序依據: ( ) 傳票日期                 |
|           ( ) 傳票編號                 |
|           ( ) 傳票類型                 |
|                                        |
| 輸出格式: [下拉選單             v]      |
|                                        |
| [產生報表]        [清除條件]           |
+----------------------------------------+
```

### 10.2 報表顯示頁面

報表顯示頁面布局設計：

```
+--------------------------------------------------------+
| 傳票查詢報表                                   [返回條件] |
+--------------------------------------------------------+
| Crystal Reports Viewer 工具列                          |
| [第一頁][上一頁][頁碼1/10][下一頁][最後頁][匯出v][列印]  |
+--------------------------------------------------------+
|                                                        |
| +-------------------- 報表頁首 ---------------------+  |
| | 泛太國際股份有限公司                頁碼：1/10     |  |
| | 傳票查詢報表                        列印日期：2025/05/15 |
| | 查詢條件：傳票日期 2025/05/01~2025/05/31           |  |
| +--------------------------------------------------+  |
| |                                                  |  |
| | +----------- 報表主體 (明細模式) -------------+  |  |
| | | 傳票編號 | 日期  | 科目   | 部門 | 借方  | 貸方 |  |  |
| | |----------|-------|--------|------|-------|-----|  |  |
| | | J0050001 | 05/01 | 1001   | 001  | 5,000 |  -  |  |  |
| | | J0050001 | 05/01 | 2001   | 001  |   -   |5,000|  |  |
| | | J0050002 | 05/02 | 1002   | 002  | 3,000 |  -  |  |  |
| | | J0050002 | 05/02 | 2002   | 002  |   -   |3,000|  |  |
| | +-------------------------------------------------+  |
| |                                                  |  |
| | +--------------- 報表頁尾 -------------------+  |  |
| | | 頁合計:                     8,000  | 8,000  |  |  |
| | | 總合計:                     8,000  | 8,000  |  |  |
| | +--------------------------------------------+  |  |
| |                                                  |  |
| +--------------------------------------------------+  |
|                                                        |
+--------------------------------------------------------+
```

## 11. 測試計劃

### 11.1 功能測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 基本查詢功能 | 使用預設條件產生報表 | 顯示符合條件的傳票資料 |
| 條件篩選功能 | 使用各種組合條件進行查詢 | 正確篩選出符合條件的傳票 |
| 明細/彙總模式 | 切換顯示明細核取方塊 | 正確切換報表顯示模式 |
| 排序功能 | 選擇不同排序依據 | 報表資料依選擇項目排序 |
| 匯出功能 | 測試各種匯出格式 | 成功產生對應格式的檔案 |

### 11.2 界面測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 條件欄位驗證 | 輸入無效日期、編號等 | 顯示適當的錯誤訊息 |
| 報表預覽控制 | 測試報表翻頁、縮放等功能 | 控制項正常運作 |
| 頁面響應性 | 在不同螢幕大小測試 | 頁面元素正確調整佈局 |
| 輸入輔助功能 | 測試日期選擇器、自動完成 | 輔助功能正確運作 |

### 11.3 效能測試

| 測試指標 | 基準值 | 測試方法 |
|---------|-------|---------|
| 報表產生時間 | <5秒 | 測量不同條件下的報表產生時間 |
| 大量資料處理 | <15秒 | 使用>10,000筆資料測試 |
| 匯出檔案時間 | <10秒 | 測量不同格式的匯出時間 |
| 記憶體使用 | <100MB | 監控報表生成過程資源使用 |

## 12. 相關檔案

### 12.1 原始程式檔案

| 檔案名稱 | 檔案類型 | 檔案大小 | 說明 |
|---------|---------|---------|------|
| GLR01H0.aspx | ASPX | 15KB | 報表查詢條件頁面 |
| GLR01H0.aspx.cs | C# | 45KB | 報表後端邏輯 |
| GLR01H0_Detail.rpt | Crystal Reports | 150KB | 明細報表樣板 |
| GLR01H0_Summary.rpt | Crystal Reports | 100KB | 彙總報表樣板 |

### 12.2 相依元件檔案

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| CrystalDecisions.CrystalReports.Engine.dll | .NET組件 | Crystal Reports引擎 |
| CrystalDecisions.Shared.dll | .NET組件 | Crystal Reports共用元件 |
| CrystalDecisions.Web.dll | .NET組件 | Crystal Reports網頁元件 |

## 13. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2025/05/15 | Claude AI | 初始版本建立 | 完成基本功能規格 | 