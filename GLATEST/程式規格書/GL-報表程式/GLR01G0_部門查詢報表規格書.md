# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | GLR01G0 |
| 程式名稱 | 部門查詢報表 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | /GLATEST/app/Reports/GLR01G0.aspx, /GLATEST/app/Reports/GLR01G0.aspx.cs |
| 程式類型 | 報表程式 |
| 建立日期 | 2025/05/16 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/16 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

GLR01G0是泛太總帳系統中的部門查詢報表程式，用於查詢和顯示系統中的部門基本資料與相關財務數據。此報表允許用戶根據各種條件篩選部門，並以視覺化方式呈現部門的結構、狀態和相關財務資訊。報表支援多種顯示模式，包括明細清單、匯總表和樹狀結構，可協助用戶了解公司的部門組織結構和各部門的財務狀況。

### 2.2 業務流程

GLR01G0在系統中的業務流程如下：
1. 用戶在總帳模組中選擇「部門查詢報表」功能
2. 系統顯示查詢條件頁面，用戶輸入查詢條件
3. 系統根據條件查詢相關部門資料
4. 用戶可預覽報表結果，並執行以下操作：
   - 調整報表顯示格式
   - 展開或摺疊部門層級（在樹狀模式）
   - 列印報表
   - 匯出報表為各種格式
   - 點選部門代碼以檢視更詳細資訊

### 2.3 使用頻率

- 中頻率：主要在組織調整和預算編制期間使用
- 平均每週使用次數：約10-15次
- 年度預算編制期間：使用頻率可能增加2倍
- 匯出操作：約35%的查詢會進行匯出

### 2.4 使用者角色

此報表主要服務於以下角色：
- 財務主管：了解部門結構和財務狀況
- 會計人員：核對部門編碼與會計明細
- 人資人員：對照人員配置與部門組織
- 部門主管：查詢所轄部門資訊
- 系統管理員：維護部門資料時參考

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET WebForms (.NET Framework 4.0)
- 主要技術：
  - Crystal Reports：報表產生和預覽
  - ADO.NET：資料庫存取技術
  - jQuery：前端交互效果增強
  - Bootstrap：響應式界面設計
  - AJAX：非同步資料載入
- 報表呈現：在網頁中嵌入Crystal Reports Viewer
- 樹狀結構顯示：使用JavaScript樹狀控制項

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| GL_DEPT_DEF | 部門定義表 | 讀取 |
| GL_DEPT_TYPE | 部門類型表 | 讀取 |
| GL_DEPT_RELATION | 部門關聯表 | 讀取 |
| GL_JOURNAL_DETAIL | 傳票明細 | 讀取 |
| GL_SYSTEM_CONFIG | 系統配置表 | 讀取 |
| SYS_USER | 使用者資料表 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |
| LoginClass | 登入模組 | 獲取當前使用者資訊 |
| AppAuthority | 權限管理 | 檢查報表存取權限 |
| TreeViewControl | 樹狀控制項 | 提供樹狀結構顯示功能 |
| Navigator | 導航控制項 | 提供頁面導航功能 |
| CrystalReportsViewer | 報表顯示 | Crystal Reports內建報表顯示元件 |

## 4. 報表規格

### 4.1 查詢條件

| 欄位名稱 | 欄位類型 | 必填 | 預設值 | 說明 |
|---------|---------|------|-------|------|
| txtDeptFrom | 文字 | N | 空白 | 部門代碼起始 |
| txtDeptTo | 文字 | N | 空白 | 部門代碼結束 |
| chkIncludeInactive | 核取方塊 | N | 不勾選 | 是否包含停用部門 |
| ddlDeptType | 下拉式選單 | N | 全部 | 部門類型 |
| ddlDeptLevel | 下拉式選單 | N | 全部 | 部門層級 |
| rdoDisplayMode | 選項按鈕 | Y | 明細模式 | 顯示模式 |
| ddlOutputFormat | 下拉式選單 | Y | 螢幕 | 輸出格式 |

### 4.2 報表欄位

#### 4.2.1 報表頁首

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| CompanyName | GL_SYSTEM_CONFIG | 公司名稱 |
| ReportTitle | 靜態文字 | 報表標題："部門查詢報表" |
| ReportDate | 系統日期 | 列印日期 |
| UserName | SYS_USER | 使用者姓名 |
| QueryCondition | 查詢條件 | 顯示已設定的查詢條件文字 |
| PageNo | Crystal Reports內建 | 頁碼資訊 |

#### 4.2.2 報表主體（明細模式）

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| DeptNo | GL_DEPT_DEF.DEPT_NO | 部門代碼 |
| DeptName | GL_DEPT_DEF.DEPT_NAME | 部門名稱 |
| DeptType | GL_DEPT_TYPE.TYPE_NAME | 部門類型 |
| DeptLevel | GL_DEPT_DEF.DEPT_LEVEL | 部門層級 |
| ParentDeptNo | GL_DEPT_DEF.PARENT_DEPT_NO | 上級部門代碼 |
| ParentDeptName | 關聯查詢 | 上級部門名稱 |
| Manager | GL_DEPT_DEF.MANAGER | 部門主管 |
| Status | GL_DEPT_DEF.STATUS | 部門狀態 |
| CreateDate | GL_DEPT_DEF.CREATE_DATE | 建立日期 |
| UpdateDate | GL_DEPT_DEF.UPDATE_DATE | 最後更新日期 |

#### 4.2.3 報表主體（樹狀模式）

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| DeptTree | 程式計算 | 部門樹狀結構 |
| DeptNo | GL_DEPT_DEF.DEPT_NO | 部門代碼 |
| DeptName | GL_DEPT_DEF.DEPT_NAME | 部門名稱 |
| DeptLevel | GL_DEPT_DEF.DEPT_LEVEL | 部門層級 |
| Status | GL_DEPT_DEF.STATUS | 部門狀態 |

#### 4.2.4 報表頁尾

| 欄位名稱 | 資料來源 | 說明 |
|---------|---------|------|
| TotalDeptCount | 計算欄位 | 部門總數 |
| ActiveDeptCount | 計算欄位 | 啟用部門數 |
| InactiveDeptCount | 計算欄位 | 停用部門數 |

### 4.3 輸出格式

| 格式名稱 | 說明 | 特性 |
|---------|------|------|
| 螢幕 | 在網頁中顯示 | 可互動、支援樹狀展開/摺疊 |
| PDF | PDF檔案 | 符合列印格式、保留格式 |
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
根據顯示模式處理資料結構
 ↓
產生報表資料集
 ↓
根據輸出格式設定Crystal Reports參數
 ↓
Crystal Reports產生報表
 ↓
將報表傳送至用戶
 ↓
結束
```

### 5.2 特殊處理邏輯

1. **樹狀結構處理**：
   - 使用遞迴演算法建立部門樹狀結構
   - 支援多層級部門關係顯示
   - 提供展開/摺疊功能

2. **部門關係處理**：
   - 處理上下級部門關係
   - 顯示部門層級路徑
   - 計算各部門下屬部門數量

3. **狀態代碼轉換**：
   - 將狀態代碼轉換為可讀文字（如：A=啟用，I=停用）
   - 使用不同顏色標示不同狀態的部門

### 5.3 例外處理

| 例外類型 | 處理方式 | 記錄方式 |
|---------|---------|---------|
| 查詢條件無效 | 顯示錯誤訊息，保留已填條件 | 記錄錯誤條件和用戶ID |
| 資料庫連接錯誤 | 顯示友善錯誤頁面，提供重試選項 | 記錄詳細連接錯誤 |
| 樹狀結構錯誤 | 以列表模式顯示結果，提示樹狀結構錯誤 | 記錄部門層級問題 |
| 無符合條件資料 | 顯示「無符合條件的資料」訊息 | 記錄查詢條件 |
| 部門循環參照 | 中斷循環，顯示警告訊息 | 記錄循環參照路徑 |

## 6. SQL查詢

### 6.1 明細查詢

```sql
-- 明細模式查詢SQL語句
SELECT 
    d.DEPT_NO, d.DEPT_NAME, d.DEPT_LEVEL,
    d.PARENT_DEPT_NO, p.DEPT_NAME AS PARENT_DEPT_NAME,
    t.TYPE_NAME AS DEPT_TYPE_NAME,
    d.MANAGER, d.STATUS, d.DESCRIPTION,
    d.CREATE_USER, d.CREATE_DATE, 
    d.UPDATE_USER, d.UPDATE_DATE
FROM GL_DEPT_DEF d
LEFT JOIN GL_DEPT_DEF p ON d.PARENT_DEPT_NO = p.DEPT_NO
LEFT JOIN GL_DEPT_TYPE t ON d.DEPT_TYPE = t.TYPE_CODE
WHERE 
    (@DeptFrom = '' OR d.DEPT_NO >= @DeptFrom)
    AND (@DeptTo = '' OR d.DEPT_NO <= @DeptTo)
    AND (@DeptType = '' OR d.DEPT_TYPE = @DeptType)
    AND (@DeptLevel = '' OR d.DEPT_LEVEL = @DeptLevel)
    AND (@IncludeInactive = 'Y' OR d.STATUS = 'A')
ORDER BY 
    d.DEPT_LEVEL, d.DEPT_NO
```

### 6.2 樹狀查詢

```sql
-- 樹狀模式基礎數據查詢SQL語句
WITH DeptCTE AS (
    -- 先查詢最頂層部門
    SELECT 
        d.DEPT_NO, d.DEPT_NAME, d.DEPT_LEVEL,
        d.PARENT_DEPT_NO, 0 AS TREE_LEVEL,
        CAST(d.DEPT_NO AS VARCHAR(1000)) AS TREE_PATH,
        d.STATUS
    FROM GL_DEPT_DEF d
    WHERE 
        d.PARENT_DEPT_NO IS NULL OR d.PARENT_DEPT_NO = ''
        AND (@DeptType = '' OR d.DEPT_TYPE = @DeptType)
        AND (@DeptLevel = '' OR d.DEPT_LEVEL = @DeptLevel)
        AND (@IncludeInactive = 'Y' OR d.STATUS = 'A')

    UNION ALL

    -- 遞迴查詢子部門
    SELECT 
        d.DEPT_NO, d.DEPT_NAME, d.DEPT_LEVEL,
        d.PARENT_DEPT_NO, p.TREE_LEVEL + 1,
        CAST(p.TREE_PATH + '/' + d.DEPT_NO AS VARCHAR(1000)),
        d.STATUS
    FROM GL_DEPT_DEF d
    INNER JOIN DeptCTE p ON d.PARENT_DEPT_NO = p.DEPT_NO
    WHERE 
        (@DeptType = '' OR d.DEPT_TYPE = @DeptType)
        AND (@DeptLevel = '' OR d.DEPT_LEVEL = @DeptLevel)
        AND (@IncludeInactive = 'Y' OR d.STATUS = 'A')
)
SELECT 
    DEPT_NO, DEPT_NAME, DEPT_LEVEL,
    PARENT_DEPT_NO, TREE_LEVEL, TREE_PATH, STATUS
FROM DeptCTE
WHERE 
    (@DeptFrom = '' OR DEPT_NO >= @DeptFrom)
    AND (@DeptTo = '' OR DEPT_NO <= @DeptTo)
ORDER BY 
    TREE_PATH
```

## 7. 程式碼說明

### 7.1 查詢條件面板

```csharp
/// <summary>
/// 初始化查詢條件控制項
/// </summary>
private void InitializeControls()
{
    // 載入部門類型選項
    this.ddlDeptType.Items.Clear();
    this.ddlDeptType.Items.Add(new ListItem("全部", ""));
    
    IBosDB db = DBFactory.GetBosDB();
    string sql = "SELECT TYPE_CODE, TYPE_NAME FROM GL_DEPT_TYPE ORDER BY TYPE_CODE";
    DataTable dtTypes = db.ExecuteDataTable(sql);
    
    foreach (DataRow row in dtTypes.Rows)
    {
        string typeCode = row["TYPE_CODE"].ToString();
        string typeName = row["TYPE_NAME"].ToString();
        this.ddlDeptType.Items.Add(new ListItem(typeName, typeCode));
    }
    
    // 載入部門層級選項
    this.ddlDeptLevel.Items.Clear();
    this.ddlDeptLevel.Items.Add(new ListItem("全部", ""));
    this.ddlDeptLevel.Items.Add(new ListItem("1-總部", "1"));
    this.ddlDeptLevel.Items.Add(new ListItem("2-事業部", "2"));
    this.ddlDeptLevel.Items.Add(new ListItem("3-部門", "3"));
    this.ddlDeptLevel.Items.Add(new ListItem("4-課", "4"));
    this.ddlDeptLevel.Items.Add(new ListItem("5-小組", "5"));

    // 設定顯示模式預設值
    this.rdoDisplayMode.SelectedValue = "DETAIL";
    
    // 設定輸出格式選項
    this.ddlOutputFormat.Items.Clear();
    this.ddlOutputFormat.Items.Add(new ListItem("螢幕", "SCREEN"));
    this.ddlOutputFormat.Items.Add(new ListItem("PDF", "PDF"));
    this.ddlOutputFormat.Items.Add(new ListItem("Excel", "EXCEL"));
    this.ddlOutputFormat.Items.Add(new ListItem("Word", "WORD"));
    this.ddlOutputFormat.Items.Add(new ListItem("CSV", "CSV"));
    this.ddlOutputFormat.SelectedValue = "SCREEN";
    
    // 預設不包含停用部門
    this.chkIncludeInactive.Checked = false;
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
        
        // 依據顯示模式執行不同的查詢
        DataTable reportData;
        string displayMode = this.rdoDisplayMode.SelectedValue;
        
        if (displayMode == "TREE")
        {
            reportData = GetTreeReportData(parameters);
        }
        else // "DETAIL" 模式
        {
            reportData = GetDetailReportData(parameters);
        }
        
        if (reportData == null || reportData.Rows.Count == 0)
        {
            this.lblMessage.Text = "查無符合條件的資料";
            this.lblMessage.Visible = true;
            return;
        }
        
        // 產生報表
        ReportDocument report = new ReportDocument();
        
        // 根據顯示模式選擇報表樣板
        string reportPath = Server.MapPath("~/Reports/Templates/");
        if (displayMode == "TREE")
        {
            reportPath += "GLR01G0_Tree.rpt";
        }
        else
        {
            reportPath += "GLR01G0_Detail.rpt";
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
        Logger.Error("GLR01G0.btnGenerate_Click", ex);
    }
}
```

### 7.3 樹狀數據處理

```csharp
/// <summary>
/// 處理樹狀顯示的資料
/// </summary>
/// <param name="dtDept">資料庫查詢結果表</param>
/// <returns>處理後的樹狀結構資料表</returns>
private DataTable ProcessTreeData(DataTable dtDept)
{
    // 創建結果表
    DataTable result = new DataTable();
    result.Columns.Add("DEPT_NO", typeof(string));
    result.Columns.Add("DEPT_NAME", typeof(string));
    result.Columns.Add("DEPT_LEVEL", typeof(int));
    result.Columns.Add("PARENT_DEPT_NO", typeof(string));
    result.Columns.Add("TREE_LEVEL", typeof(int));
    result.Columns.Add("TREE_PATH", typeof(string));
    result.Columns.Add("STATUS", typeof(string));
    result.Columns.Add("INDENT_NAME", typeof(string));
    result.Columns.Add("HAS_CHILDREN", typeof(bool));
    
    // 若沒有資料，返回空表
    if (dtDept == null || dtDept.Rows.Count == 0)
    {
        return result;
    }
    
    // 計算各部門的子部門數量
    Dictionary<string, int> childCounts = new Dictionary<string, int>();
    foreach (DataRow row in dtDept.Rows)
    {
        string parentDeptNo = row["PARENT_DEPT_NO"].ToString();
        if (!string.IsNullOrEmpty(parentDeptNo))
        {
            if (!childCounts.ContainsKey(parentDeptNo))
            {
                childCounts[parentDeptNo] = 1;
            }
            else
            {
                childCounts[parentDeptNo]++;
            }
        }
    }
    
    // 處理每一筆資料
    foreach (DataRow srcRow in dtDept.Rows)
    {
        DataRow newRow = result.NewRow();
        
        // 複製基礎欄位
        newRow["DEPT_NO"] = srcRow["DEPT_NO"];
        newRow["DEPT_NAME"] = srcRow["DEPT_NAME"];
        newRow["DEPT_LEVEL"] = srcRow["DEPT_LEVEL"];
        newRow["PARENT_DEPT_NO"] = srcRow["PARENT_DEPT_NO"];
        newRow["TREE_LEVEL"] = srcRow["TREE_LEVEL"];
        newRow["TREE_PATH"] = srcRow["TREE_PATH"];
        newRow["STATUS"] = srcRow["STATUS"];
        
        // 計算縮排後的名稱顯示
        int treeLevel = Convert.ToInt32(srcRow["TREE_LEVEL"]);
        string indentName = new string('　', treeLevel * 2) + srcRow["DEPT_NAME"].ToString();
        newRow["INDENT_NAME"] = indentName;
        
        // 確定是否有子部門
        string deptNo = srcRow["DEPT_NO"].ToString();
        newRow["HAS_CHILDREN"] = childCounts.ContainsKey(deptNo) && childCounts[deptNo] > 0;
        
        result.Rows.Add(newRow);
    }
    
    return result;
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
   - 根據用戶權限控制可見部門範圍
   - 某些敏感部門資訊僅對特定角色顯示

### 8.2 資料保護

1. **SQL注入防護**
   - 使用參數化查詢處理所有SQL
   - 避免直接拼接用戶輸入的SQL語句
   - 對排序欄位進行白名單驗證

2. **敏感資料處理**
   - 部門主管和財務資料僅向有權限的用戶顯示
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
   - 部門表結構查詢使用遞迴CTE提升效率
   - 索引優化支援快速部門層級查詢
   - 樹狀結構查詢使用批次處理避免過度遞迴

2. **資料處理優化**
   - 控制部門層級深度，避免過深遞迴
   - 樹狀結構預先計算，減少動態生成時間
   - 使用快取機制減少重複查詢

### 9.2 報表效能

1. **Crystal Reports優化**
   - 精簡報表設計避免不必要的格式化
   - 減少子報表使用
   - 適當分頁處理大量部門數據

2. **前端優化**
   - 樹狀結構使用延遲載入（懶加載）
   - 動態調整顯示內容，按需載入詳細資訊
   - 使用AJAX技術減少頁面重載

## 10. 使用者介面

### 10.1 查詢條件頁面

查詢條件頁面布局設計：

```
+----------------------------------------+
| 部門查詢報表條件                        |
+----------------------------------------+
| 部門代碼: [         ] 至 [         ]   |
| 部門類型: [下拉選單             v]      |
| 部門層級: [下拉選單             v]      |
|                                        |
| [x] 包含停用部門                        |
|                                        |
| 顯示模式: ( ) 明細模式                 |
|           ( ) 樹狀模式                 |
|                                        |
| 輸出格式: [下拉選單             v]      |
|                                        |
| [產生報表]        [清除條件]           |
+----------------------------------------+
```

### 10.2 報表顯示頁面（明細模式）

明細模式報表顯示頁面布局：

```
+--------------------------------------------------------+
| 部門查詢報表                                   [返回條件] |
+--------------------------------------------------------+
| Crystal Reports Viewer 工具列                          |
| [第一頁][上一頁][頁碼1/3][下一頁][最後頁][匯出v][列印]  |
+--------------------------------------------------------+
|                                                        |
| +-------------------- 報表頁首 ---------------------+  |
| | 泛太國際股份有限公司                頁碼：1/3      |  |
| | 部門查詢報表                        列印日期：2025/05/16 |
| | 查詢條件：部門類型=行政部門                        |  |
| +--------------------------------------------------+  |
| |                                                  |  |
| | +------------- 報表主體 (明細模式) -----------+  |  |
| | | 部門代碼 | 部門名稱  | 層級 | 上級部門 | 主管  |  |  |
| | |----------|-----------|------|----------|-----|  |  |
| | | A100     | 總經理室  | 1    |  -       | 王大明|  |  |
| | | A110     | 秘書室    | 2    | A100     | 李小華|  |  |
| | | A200     | 行政部    | 2    | A100     | 張三豐|  |  |
| | | A210     | 人事課    | 3    | A200     | 陳美美|  |  |
| | | A220     | 總務課    | 3    | A200     | 林大方|  |  |
| | +-------------------------------------------------+  |
| |                                                  |  |
| | +--------------- 報表頁尾 -------------------+  |  |
| | | 部門總數: 5                                |  |  |
| | | 啟用部門: 5    停用部門: 0                 |  |  |
| | +--------------------------------------------+  |  |
| |                                                  |  |
| +--------------------------------------------------+  |
|                                                        |
+--------------------------------------------------------+
```

### 10.3 報表顯示頁面（樹狀模式）

樹狀模式報表顯示頁面布局：

```
+--------------------------------------------------------+
| 部門查詢報表                                   [返回條件] |
+--------------------------------------------------------+
| Crystal Reports Viewer 工具列                          |
| [第一頁][上一頁][頁碼1/2][下一頁][最後頁][匯出v][列印]  |
+--------------------------------------------------------+
|                                                        |
| +-------------------- 報表頁首 ---------------------+  |
| | 泛太國際股份有限公司                頁碼：1/2      |  |
| | 部門查詢報表 (樹狀結構)              列印日期：2025/05/16 |
| | 查詢條件：部門類型=行政部門                        |  |
| +--------------------------------------------------+  |
| |                                                  |  |
| | +-------------- 報表主體 (樹狀模式) -----------+  |  |
| | | 部門層級結構                           | 代碼  |  |  |
| | |----------------------------------------|-----|  |  |
| | | ● 總經理室                             | A100 |  |  |
| | |   ├─● 秘書室                          | A110 |  |  |
| | |   └─● 行政部                          | A200 |  |  |
| | |       ├─● 人事課                      | A210 |  |  |
| | |       └─● 總務課                      | A220 |  |  |
| | +-------------------------------------------------+  |
| |                                                  |  |
| | +--------------- 報表頁尾 -------------------+  |  |
| | | 部門總數: 5                                |  |  |
| | | 啟用部門: 5    停用部門: 0                 |  |  |
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
| 基本查詢功能 | 使用預設條件產生報表 | 顯示符合條件的部門資料 |
| 條件篩選功能 | 使用各種組合條件進行查詢 | 正確篩選出符合條件的部門 |
| 明細/樹狀模式 | 切換顯示模式 | 正確切換報表顯示模式 |
| 部門層級顯示 | 使用樹狀模式顯示多層部門 | 正確顯示部門階層關係 |
| 匯出功能 | 測試各種匯出格式 | 成功產生對應格式的檔案 |

### 11.2 界面測試

| 測試案例 | 測試方法 | 預期結果 |
|---------|---------|---------|
| 條件欄位驗證 | 輸入無效部門代碼 | 顯示適當的錯誤訊息 |
| 報表預覽控制 | 測試報表翻頁、縮放等功能 | 控制項正常運作 |
| 樹狀結構互動 | 測試樹狀結構展開/摺疊 | 正確展開與摺疊節點 |
| 頁面響應性 | 在不同螢幕大小測試 | 頁面元素正確調整佈局 |

### 11.3 效能測試

| 測試指標 | 基準值 | 測試方法 |
|---------|-------|---------|
| 報表產生時間 | <3秒 | 測量不同條件下的報表產生時間 |
| 樹狀結構處理 | <5秒 | 測試大型組織結構的樹狀顯示時間 |
| 匯出檔案時間 | <8秒 | 測量不同格式的匯出時間 |
| 記憶體使用 | <80MB | 監控報表生成過程資源使用 |

## 12. 相關檔案

### 12.1 原始程式檔案

| 檔案名稱 | 檔案類型 | 檔案大小 | 說明 |
|---------|---------|---------|------|
| GLR01G0.aspx | ASPX | 9KB | 報表查詢條件頁面 |
| GLR01G0.aspx.cs | C# | 28KB | 報表後端邏輯 |
| GLR01G0_Detail.rpt | Crystal Reports | 110KB | 明細報表樣板 |
| GLR01G0_Tree.rpt | Crystal Reports | 95KB | 樹狀報表樣板 |

### 12.2 相依元件檔案

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| TreeView.js | JavaScript | 樹狀結構控制項腳本 |
| CrystalDecisions.CrystalReports.Engine.dll | .NET組件 | Crystal Reports引擎 |
| CrystalDecisions.Shared.dll | .NET組件 | Crystal Reports共用元件 |
| CrystalDecisions.Web.dll | .NET組件 | Crystal Reports網頁元件 |

## 13. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2025/05/16 | Claude AI | 初始版本建立 | 完成基本功能規格 | 