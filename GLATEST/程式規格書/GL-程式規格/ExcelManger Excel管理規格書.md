# ExcelManger Excel管理規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | ExcelManger                          |
| 程式名稱     | Excel管理                             |
| 檔案大小     | 30KB                                 |
| 行數        | ~699                                 |
| 功能簡述     | 提供Excel匯出功能                      |
| 複雜度       | 高                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/27                           |

## 程式功能概述

ExcelManger 是泛太總帳系統中的核心組件之一，專門負責 Excel 檔案的動態生成、格式設定和資料匯出功能。此元件能夠將系統中的各種資料（如報表數據、查詢結果、財務分析資料等）轉換為 Excel 格式，並提供高度客製化的功能，使輸出的 Excel 檔案具有專業的格式與美觀的外觀。

主要功能包括：

1. **資料匯出**：將系統各模組的資料匯出為 Excel 檔案，支援多種報表類型
2. **動態樣式**：根據不同報表需求自動套用適合的樣式和格式
3. **格式設定**：支援單元格格式、邊框、字型、背景色等詳細設定
4. **公式處理**：支援基本計算公式及複雜函數的動態生成
5. **圖表生成**：能夠根據資料自動生成各種圖表
6. **多工作表**：支援在單一 Excel 檔案中建立多個工作表
7. **合併輸出**：能夠將多個報表合併輸出到同一個檔案
8. **資料過濾**：提供資料過濾和排序功能
9. **存取控制**：設定檔案密碼保護和編輯限制
10. **自動計算**：生成包含小計、總計、平均值等自動計算功能的報表

ExcelManger 的設計遵循 .NET 組件化開發原則，與系統其他模組協同工作，為泛太總帳系統提供強大而靈活的資料匯出和報表生成能力。此組件廣泛應用於系統的各種財務報表、統計分析、資料匯出等功能中，是企業進行數據分析和決策支援的重要工具。

## 類別結構說明

ExcelManger 的類別結構採用了模組化和責任分離的設計原則，以確保程式碼的可維護性和擴展性。主要類別結構如下：

1. **ExcelManger**：核心管理類別，提供主要的 Excel 操作介面
   - 負責協調各個功能模組
   - 提供整體 Excel 文件的管理功能
   - 處理檔案的保存與讀取

2. **ExcelWorkbook**：Excel 工作簿類別
   - 管理整個工作簿的屬性與設定
   - 控制工作簿層級的操作
   - 負責工作表的新增、刪除與管理

3. **ExcelWorksheet**：Excel 工作表類別
   - 提供單一工作表的操作介面
   - 管理工作表的格式和佈局
   - 處理工作表層級的資料操作

4. **CellStyle**：單元格樣式類別
   - 定義和管理單元格的視覺效果
   - 支援字型、顏色、邊框等樣式設定
   - 提供預設樣式模板

5. **FormulaProcessor**：公式處理類別
   - 生成和解析 Excel 公式
   - 提供常用函數的封裝
   - 處理公式的動態更新

6. **ChartGenerator**：圖表生成類別
   - 創建各種類型的圖表
   - 設定圖表的數據範圍和樣式
   - 管理圖表的位置和大小

7. **DataFormatter**：資料格式處理器
   - 處理不同類型資料的格式轉換
   - 提供貨幣、日期等專用格式處理
   - 轉換數據類型以符合 Excel 要求

8. **ExcelSecurity**：Excel 安全設定類別
   - 管理文件的密碼保護
   - 設定儲存格或工作表的保護級別
   - 控制文件的權限設定

9. **ReportTemplate**：報表模板類別
   - 定義常用報表的標準格式
   - 管理報表頁首、頁尾和標題區
   - 提供統一的報表生成介面

10. **ExcelExporter**：資料匯出處理器
    - 處理資料集到 Excel 的轉換
    - 支援多種資料來源的匯出
    - 提供批次處理大量資料的能力

## 技術實現

ExcelManger 採用以下技術實現其功能：

1. **Excel Interop API**：
   - 使用 Microsoft.Office.Interop.Excel 命名空間提供的 COM 介面
   - 直接操作 Excel 物件模型
   - 支援所有 Excel 的原生功能

2. **Open XML SDK**：
   - 用於處理 Excel 2007+ 格式 (.xlsx)
   - 不依賴 Excel 應用程式的安裝
   - 提供高效能的檔案生成能力

3. **NPOI 庫**：
   - 純 .NET 實現的 Excel 操作庫
   - 支援讀取和生成 Excel 檔案
   - 輕量級且無需安裝 Excel

4. **反射技術**：
   - 用於動態處理不同類型的資料物件
   - 支援資料物件到 Excel 的自動映射
   - 處理動態屬性和匿名類型

5. **資料集處理**：
   - 支援 ADO.NET DataSet/DataTable 轉換
   - 處理資料庫查詢結果的直接轉換
   - 高效處理大量資料行和列

6. **記憶體優化**：
   - 使用緩衝區處理大型資料集
   - 實現分段式資料處理
   - 減少記憶體佔用和提高性能

7. **樣式工廠**：
   - 實現樣式的快取與重用
   - 減少樣式物件的重複創建
   - 提供常用樣式的快速套用

8. **多執行緒處理**：
   - 支援背景執行大型報表生成
   - 避免在 UI 執行緒中進行耗時操作
   - 提供進度回報機制

9. **事件模型**：
   - 提供各種操作階段的事件通知
   - 支援客製化處理和數據轉換
   - 允許外部程式碼介入處理流程

10. **檔案串流處理**：
    - 直接使用檔案串流生成 Excel
    - 避免在大型檔案處理中過度消耗記憶體
    - 支援直接輸出到網頁響應流

## 主要方法參考

### ExcelManger 核心類別方法

| 方法名稱 | 方法簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `CreateWorkbook` | `ExcelWorkbook CreateWorkbook(string name)` | 創建新工作簿 | name: 工作簿名稱 | 工作簿物件 |
| `OpenWorkbook` | `ExcelWorkbook OpenWorkbook(string filePath)` | 開啟現有工作簿 | filePath: 檔案路徑 | 工作簿物件 |
| `SaveWorkbook` | `bool SaveWorkbook(ExcelWorkbook workbook, string filePath)` | 保存工作簿 | workbook: 工作簿, filePath: 保存路徑 | 是否成功 |
| `ExportDataTable` | `bool ExportDataTable(DataTable data, string filePath, ExcelExportOptions options)` | 匯出DataTable | data: 資料表, filePath: 輸出路徑, options: 匯出選項 | 是否成功 |
| `ExportToStream` | `MemoryStream ExportToStream(DataTable data, ExcelExportOptions options)` | 匯出至串流 | data: 資料表, options: 匯出選項 | 記憶體串流 |

### ExcelWorkbook 類別方法

| 方法名稱 | 方法簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `AddWorksheet` | `ExcelWorksheet AddWorksheet(string name)` | 新增工作表 | name: 工作表名稱 | 工作表物件 |
| `GetWorksheet` | `ExcelWorksheet GetWorksheet(string name)` | 獲取工作表 | name: 工作表名稱 | 工作表物件 |
| `DeleteWorksheet` | `bool DeleteWorksheet(string name)` | 刪除工作表 | name: 工作表名稱 | 是否成功 |
| `SetProperties` | `void SetProperties(ExcelWorkbookProperties properties)` | 設定工作簿屬性 | properties: 屬性物件 | 無 |
| `Protect` | `void Protect(string password, ExcelProtectionOptions options)` | 保護工作簿 | password: 密碼, options: 保護選項 | 無 |

### ExcelWorksheet 類別方法

| 方法名稱 | 方法簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `SetValue` | `void SetValue(int row, int col, object value)` | 設定單元格值 | row: 行, col: 列, value: 值 | 無 |
| `GetValue` | `object GetValue(int row, int col)` | 獲取單元格值 | row: 行, col: 列 | 單元格值 |
| `SetFormula` | `void SetFormula(int row, int col, string formula)` | 設定公式 | row: 行, col: 列, formula: 公式 | 無 |
| `ApplyStyle` | `void ApplyStyle(ExcelRange range, CellStyle style)` | 應用樣式 | range: 範圍, style: 樣式 | 無 |
| `MergeCells` | `void MergeCells(ExcelRange range)` | 合併單元格 | range: 要合併的範圍 | 無 |

### ChartGenerator 類別方法

| 方法名稱 | 方法簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `CreateChart` | `ExcelChart CreateChart(ExcelWorksheet sheet, ChartType type, ExcelRange dataRange)` | 創建圖表 | sheet: 工作表, type: 圖表類型, dataRange: 數據範圍 | 圖表物件 |
| `SetChartTitle` | `void SetChartTitle(ExcelChart chart, string title)` | 設定圖表標題 | chart: 圖表, title: 標題 | 無 |
| `AddSeries` | `void AddSeries(ExcelChart chart, string name, ExcelRange values)` | 添加數據系列 | chart: 圖表, name: 系列名, values: 數據範圍 | 無 |
| `SetPosition` | `void SetPosition(ExcelChart chart, int row, int col, int width, int height)` | 設定圖表位置 | chart: 圖表, row/col: 位置, width/height: 尺寸 | 無 |
| `ApplyChartStyle` | `void ApplyChartStyle(ExcelChart chart, ChartStyle style)` | 應用圖表樣式 | chart: 圖表, style: 樣式 | 無 |

### CellStyle 類別方法

| 方法名稱 | 方法簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `SetFont` | `void SetFont(string name, int size, bool bold, bool italic)` | 設定字型 | name: 字體名, size: 大小, bold/italic: 粗體/斜體 | 無 |
| `SetAlignment` | `void SetAlignment(HorizontalAlignment h, VerticalAlignment v)` | 設定對齊方式 | h: 水平對齊, v: 垂直對齊 | 無 |
| `SetBorder` | `void SetBorder(BorderStyle style, Color color)` | 設定邊框 | style: 邊框樣式, color: 顏色 | 無 |
| `SetFillColor` | `void SetFillColor(Color color)` | 設定背景色 | color: 顏色 | 無 |
| `Clone` | `CellStyle Clone()` | 複製樣式 | 無 | 新樣式物件 |

## 使用範例

以下是 ExcelManger 的一些常見使用範例：

### 基本匯出功能

```csharp
// 基本匯出功能，將查詢結果匯出為Excel檔案
public void ExportAccountList(DataTable accountsData, string outputPath)
{
    // 創建匯出選項
    var options = new ExcelExportOptions
    {
        SheetName = "會計科目列表",
        IncludeHeader = true,
        AutoFitColumns = true,
        FreezePanes = new ExcelFreezePaneOptions { FreezePanesRowCount = 1 }
    };
    
    // 使用ExcelManger匯出DataTable
    ExcelManger excelMgr = new ExcelManger();
    bool success = excelMgr.ExportDataTable(accountsData, outputPath, options);
    
    if (success)
    {
        LogManager.Log("會計科目列表已成功匯出至: " + outputPath);
    }
    else
    {
        LogManager.LogError("匯出會計科目列表失敗");
    }
}
```

### 複雜報表生成

```csharp
// 生成包含格式設定的資產負債表
public void GenerateBalanceSheet(DateTime reportDate, string outputPath)
{
    // 獲取報表數據
    DataTable balanceSheetData = AccountingRepository.GetBalanceSheetData(reportDate);
    
    // 創建Excel管理器和工作簿
    ExcelManger excelMgr = new ExcelManger();
    ExcelWorkbook workbook = excelMgr.CreateWorkbook("資產負債表");
    
    // 添加工作表
    ExcelWorksheet sheet = workbook.AddWorksheet("資產負債表");
    
    // 設置標題和報表資訊
    sheet.SetValue(1, 1, "泛太企業資產負債表");
    CellStyle titleStyle = new CellStyle();
    titleStyle.SetFont("Arial", 16, true, false);
    titleStyle.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
    sheet.ApplyStyle(new ExcelRange(1, 1, 1, 8), titleStyle);
    sheet.MergeCells(new ExcelRange(1, 1, 1, 8));
    
    // 設置報表日期
    sheet.SetValue(2, 1, $"報表日期: {reportDate.ToString("yyyy年MM月dd日")}");
    sheet.MergeCells(new ExcelRange(2, 1, 2, 8));
    
    // 填充資料並套用格式
    int rowIndex = 4;
    
    // 標題列樣式
    CellStyle headerStyle = new CellStyle();
    headerStyle.SetFont("Arial", 11, true, false);
    headerStyle.SetFillColor(Color.LightGray);
    headerStyle.SetBorder(BorderStyle.Thin, Color.Black);
    headerStyle.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
    
    // 設置標題列
    string[] headers = new string[] { "科目代碼", "科目名稱", "本期金額", "去年同期金額", "變動金額", "變動百分比" };
    for (int i = 0; i < headers.Length; i++)
    {
        sheet.SetValue(rowIndex, i + 1, headers[i]);
    }
    sheet.ApplyStyle(new ExcelRange(rowIndex, 1, rowIndex, headers.Length), headerStyle);
    
    rowIndex++;
    
    // 數據列樣式
    CellStyle dataStyle = new CellStyle();
    dataStyle.SetFont("Arial", 10, false, false);
    dataStyle.SetBorder(BorderStyle.Thin, Color.Black);
    
    // 貨幣格式樣式
    CellStyle currencyStyle = dataStyle.Clone();
    currencyStyle.SetFormat("#,##0.00");
    currencyStyle.SetAlignment(HorizontalAlignment.Right, VerticalAlignment.Center);
    
    // 填充數據
    foreach (DataRow row in balanceSheetData.Rows)
    {
        sheet.SetValue(rowIndex, 1, row["SubjectCode"]);
        sheet.SetValue(rowIndex, 2, row["SubjectName"]);
        sheet.SetValue(rowIndex, 3, Convert.ToDecimal(row["CurrentAmount"]));
        sheet.SetValue(rowIndex, 4, Convert.ToDecimal(row["LastYearAmount"]));
        sheet.SetValue(rowIndex, 5, Convert.ToDecimal(row["ChangeAmount"]));
        sheet.SetValue(rowIndex, 6, Convert.ToDecimal(row["ChangePercentage"]));
        
        // 套用樣式
        sheet.ApplyStyle(new ExcelRange(rowIndex, 1, rowIndex, 2), dataStyle);
        sheet.ApplyStyle(new ExcelRange(rowIndex, 3, rowIndex, 6), currencyStyle);
        
        rowIndex++;
    }
    
    // 添加圖表
    if (balanceSheetData.Rows.Count > 0)
    {
        ChartGenerator chartGen = new ChartGenerator();
        ExcelChart chart = chartGen.CreateChart(sheet, ChartType.ColumnClustered, 
            new ExcelRange(5, 2, rowIndex - 1, 4));
        chartGen.SetChartTitle(chart, "資產負債比較");
        chartGen.SetPosition(chart, rowIndex + 2, 2, 600, 300);
    }
    
    // 保存工作簿
    excelMgr.SaveWorkbook(workbook, outputPath);
}
```

### 多工作表報表

```csharp
// 創建包含多個工作表的財務報告
public void GenerateFinancialReport(int year, int month, string outputPath)
{
    // 創建Excel管理器和工作簿
    ExcelManger excelMgr = new ExcelManger();
    ExcelWorkbook workbook = excelMgr.CreateWorkbook("財務報告");
    
    // 設定工作簿屬性
    workbook.SetProperties(new ExcelWorkbookProperties
    {
        Title = $"{year}年{month}月財務報告",
        Subject = "月度財務狀況",
        Author = "泛太總帳系統",
        Company = "泛太企業"
    });
    
    // 產生資產負債表
    GenerateBalanceSheetTab(workbook, year, month);
    
    // 產生損益表
    GenerateIncomeStatementTab(workbook, year, month);
    
    // 產生現金流量表
    GenerateCashFlowTab(workbook, year, month);
    
    // 產生部門損益表
    GenerateDepartmentProfitTab(workbook, year, month);
    
    // 保存工作簿
    excelMgr.SaveWorkbook(workbook, outputPath);
}

// 產生資產負債表工作表
private void GenerateBalanceSheetTab(ExcelWorkbook workbook, int year, int month)
{
    // 實現資產負債表工作表生成邏輯...
}
```

### 記憶體串流輸出

```csharp
// 將報表輸出至HTTP響應
public void OutputReportToResponse(DataTable reportData, HttpResponse response)
{
    ExcelManger excelMgr = new ExcelManger();
    
    // 創建匯出選項
    var options = new ExcelExportOptions
    {
        SheetName = "報表數據",
        IncludeHeader = true,
        AutoFitColumns = true
    };
    
    // 匯出至記憶體串流
    using (MemoryStream ms = excelMgr.ExportToStream(reportData, options))
    {
        // 設定響應頭
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("Content-Disposition", "attachment; filename=Report.xlsx");
        
        // 輸出至響應
        ms.WriteTo(response.OutputStream);
        response.Flush();
    }
}
```

## 相依關係

ExcelManger 依賴以下外部資源：

1. **Microsoft.Office.Interop.Excel**：
   - 當使用 Excel Interop 方式時需要此依賴
   - 要求目標機器安裝 Microsoft Excel 應用程式
   - 版本兼容性：Office 2010 或更高版本

2. **DocumentFormat.OpenXml**：
   - 用於 Open XML SDK 方式的 Excel 檔案處理
   - 版本：2.11.0 或更新
   - 不需要安裝 Excel 應用程式

3. **NPOI**：
   - 純 .NET 方式處理 Excel 檔案時使用
   - 版本：2.5.1 或更新
   - 不需要安裝 Excel 應用程式

4. **System.Drawing**：
   - 用於顏色和圖表處理
   - .NET Framework 的標準組件

5. **System.Data**：
   - 用於處理 DataTable 和 DataSet
   - .NET Framework 的標準組件

6. **System.IO**：
   - 用於檔案與串流操作
   - .NET Framework 的標準組件

ExcelManger 被系統中以下檔案直接依賴：

1. **所有報表程式**：如 GLR0150, GLR0160 等系統報表
2. **資料匯出功能**：如 PTA0150_M, PTA0160_M 等查詢畫面
3. **列印模組**：如 GLR02Q0 (傳票列印)
4. **批次處理程式**：用於自動化報表生成 

## 安全性考量

ExcelManger 實現了以下安全措施：

1. **輸入驗證**：
   - 對所有公開方法的輸入參數進行有效性驗證
   - 防止空值引用和類型轉換錯誤
   - 檢查檔案路徑的合法性與安全性

2. **檔案存取控制**：
   - 支援 Excel 檔案的密碼保護功能
   - 提供工作簿與工作表層級的權限控制
   - 防止未授權修改與查看敏感資料

3. **XSS 防護**：
   - 清理並驗證從資料庫讀取的內容
   - 避免在生成 Excel 檔案時引入可能的 XSS 攻擊內容
   - 對特殊字符進行適當處理

4. **路徑注入防護**：
   - 檢查並淨化檔案路徑參數
   - 防止路徑遍歷攻擊
   - 限制輸出目錄在安全範圍內

5. **記憶體保護**：
   - 使用安全的記憶體管理實踐
   - 在處理完畢後釋放大型記憶體物件
   - 避免記憶體洩漏和溢出

6. **COM 互通安全**：
   - 在使用 Excel Interop 時實施安全的 COM 物件生命週期管理
   - 確保 COM 物件正確釋放和終結
   - 最小化 COM 互通的攻擊面

7. **權限檢查**：
   - 檢查使用者對輸出目錄的寫入權限
   - 提供適當的權限不足錯誤訊息
   - 防止權限提升嘗試

8. **錯誤處理**：
   - 捕獲並安全處理所有例外
   - 避免暴露系統細節的錯誤訊息
   - 提供安全的錯誤記錄機制

## 效能考量

為確保 ExcelManger 在處理大型資料集和複雜報表時保持高效能，實現了以下優化策略：

1. **記憶體使用優化**：
   - 使用記憶體串流而非直接在記憶體中建立完整檔案
   - 實現大型資料集的分段處理
   - 即時釋放不再需要的資源

2. **批次處理**：
   - 批次處理資料行和資料列操作
   - 減少單獨的 API 調用次數
   - 合併相同樣式的單元格操作

3. **資料結構優化**：
   - 使用高效的集合類型管理大型資料集
   - 避免不必要的資料複製和轉換
   - 最小化記憶體重複分配

4. **延遲操作**：
   - 實現延遲執行機制，僅在需要時執行實際操作
   - 合併多次樣式更改成單次操作
   - 避免不必要的格式重算

5. **樣式共享**：
   - 實現樣式對象池和重用機制
   - 避免創建重複的樣式物件
   - 減少 Excel 檔案大小和處理時間

6. **多執行緒處理**：
   - 支援背景執行耗時操作
   - 實現報表生成進度通知
   - 避免凍結用戶界面

7. **檔案大小優化**：
   - 僅包含必要的樣式和格式
   - 自動壓縮圖片和嵌入對象
   - 移除冗餘資料和未使用元素

8. **算法優化**：
   - 使用高效算法處理排序和過濾操作
   - 最小化計算複雜度
   - 優化複雜公式生成

9. **快取策略**：
   - 實現智能快取機制
   - 重用頻繁訪問的物件和數據
   - 減少重複計算和處理

10. **平台選擇**：
    - 根據具體需求動態選擇最適合的 Excel 處理方式
    - 小型報表使用 NPOI 以提高速度
    - 複雜報表使用 Open XML SDK 提高兼容性

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/20 | 系統開發部 | 初始版本                         |
| 1.1.0 | 2024/03/15 | 系統開發部 | 新增圖表生成功能                 |
| 1.2.0 | 2024/04/22 | 系統開發部 | 改進大型資料集處理               |
| 1.2.1 | 2024/06/08 | 系統開發部 | 修復日期格式處理問題             |
| 1.3.0 | 2024/08/14 | 系統開發部 | 新增 Open XML SDK 支援           |
| 1.4.0 | 2024/10/30 | 系統開發部 | 加強安全性和改進效能             |
| 1.5.0 | 2025/01/18 | 系統開發部 | 新增 NPOI 支援和多執行緒處理     |
| 2.0.0 | 2025/03/05 | 系統開發部 | 重構核心架構並改進樣式處理       |

## 待改進事項

1. **跨平台支援**：
   - 完全移除對 Excel Interop 的依賴
   - 確保在所有支援 .NET 的平台上運行
   - 增強對各種作業系統的兼容性

2. **效能提升**：
   - 進一步優化大型資料集處理
   - 實現更智能的記憶體管理
   - 減少 CPU 和記憶體佔用

3. **功能擴展**：
   - 新增更多圖表類型支援
   - 擴展樞紐分析表功能
   - 增加更多高級數據分析功能

4. **格式支援**：
   - 增加對 Excel 較新版本特定功能的支援
   - 添加更多進階格式和樣式選項
   - 支援更多自定義功能

5. **整合改進**：
   - 增強與其他系統模組的整合
   - 提供更友好的 API 和文檔
   - 簡化複雜報表的生成流程

6. **錯誤處理**：
   - 改進錯誤診斷和報告
   - 提供更詳細的錯誤資訊
   - 增加自動恢復和錯誤補償機制

7. **國際化支援**：
   - 增強多語言內容處理
   - 支援不同區域設定的日期和數字格式
   - 提供更完善的字元編碼支援

8. **無障礙功能**：
   - 在生成的 Excel 檔案中支援無障礙特性
   - 增加螢幕閱讀器支援
   - 提供更多無障礙文檔功能 