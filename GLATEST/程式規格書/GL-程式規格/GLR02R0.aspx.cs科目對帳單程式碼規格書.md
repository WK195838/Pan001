# GLR02R0.aspx.cs 科目對帳單程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02R0.aspx.cs                       |
| 程式名稱     | 科目對帳單程式碼                        |
| 檔案大小     | 9.8KB                                 |
| 行數        | ~300                                  |
| 功能簡述     | 科目對帳單後端邏輯                      |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02R0.aspx.cs 是科目對帳單報表頁面的後端程式碼，負責處理使用者查詢請求、資料庫連接、資料處理和報表參數設定。主要功能包括：

1. 初始化頁面元素和控制項
2. 載入公司別和會計年度下拉式選單
3. 處理使用者查詢請求並驗證輸入參數
4. 從資料庫取得會計期間資訊
5. 查詢已刪除傳票的詳細資料
6. 設定 Crystal Report 報表參數
7. 將查詢結果綁定到報表並顯示
8. 處理異常情況和錯誤訊息

## 程式結構說明

GLR02R0.aspx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 GLR02R0 類別，繼承自 System.Web.UI.Page
3. **成員變數**：定義資料庫連接字串、使用者資訊和程式代號等變數
4. **頁面事件處理方法**：處理頁面載入、按鈕點擊和下拉選單選項變更等事件
5. **資料處理方法**：執行資料查詢、處理和綁定
6. **輔助方法**：如控制項填充、參數設定等

## 程式碼結構

### 命名空間引用

```csharp
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PanPacificClass;
```

### 類別宣告與成員變數

```csharp
public partial class GLR02R0 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02R0";
    DBManger _MyDBM;
}
```

### 主要方法

#### Page_PreInit 方法
此方法在頁面初始化前執行，可用於設定頁面主題和母版頁。此部分目前已被註解，未實際使用。

```csharp
protected void Page_PreInit(object sender, EventArgs e)
{
    //Page.Theme = "Theme_09";
    //if (Session["Theme"] != null)
    //    Page.Theme = Session["Theme"].ToString();

    //if (Session["MasterPage"] != null)
    //    Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
}
```

#### Page_Load 方法
此方法在頁面載入時執行，設置報表檢視器屬性，註冊查詢按鈕的等待畫面 JavaScript，並根據頁面狀態（首次載入或回傳）執行不同的初始化操作。

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    // 設定等待畫面和報表檢視器屬性
    btnQuery.Attributes.Add("onClick", "drawWait('')");
    CrystalReportViewer1.DisplayGroupTree = false;
    CrystalReportViewer1.HasToggleGroupTreeButton = false;

    if (!IsPostBack)
    {
        // 首次載入時初始化控制項
        // 載入公司資料
        string strSQL = "SELECT Company, Company + '-' + CompanyShortName AS CompanyName FROM Company";
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable dtComp = new DataTable();
        dtComp = _MyDBM.ExecuteDataTable(strSQL);
        dtComp.Columns[0].ColumnName = "CompanyNo";
        dtComp.Columns[1].ColumnName = "CompanyName";

        if (dtComp.Columns.Count != 0)
        {
            FillDropDownList(DrpCompany, "CompanyNo", "CompanyName", dtComp);
        }
        
        // 設定預設公司和載入對應的會計年度
        DrpCompany.SelectedValue = "20";
        BindAcctYear(DrpCompany.SelectedValue);
    }
    else
    {
        // 回傳頁面時重新綁定報表資料
        if (ViewState["Sourcedata"] != null)
        {
            CryReportSource.ReportDocument.SetDataSource((DataTable)ViewState["Sourcedata"]);
            CrystalReportViewer1.DataBind();
        }
    }

    // 註冊查詢按鈕為 AJAX PostBack 控制項
    ScriptManager1.RegisterPostBackControl(btnQuery);
}
```

#### btnQuery_Click 方法
處理查詢按鈕點擊事件，執行主要查詢邏輯，包括參數驗證、資料擷取和報表生成。

```csharp
protected void btnQuery_Click(object sender, ImageClickEventArgs e)
{
    // 畫面查詢條件檢查
    string strSQL = "";
    DataTable DT;
    SqlCommand sqlcmd = new SqlCommand();
    _MyDBM = new DBManger();
    _MyDBM.New();

    // 檢查會計年度
    string myAcctYear = "";
    if (DrpAcctYear.SelectedValue == "" || String.Compare(DrpAcctYear.SelectedValue, "1900") < 0 || String.Compare(DrpAcctYear.SelectedValue, "2099") > 0)
    {
        JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
        return;
    }

    myAcctYear = DrpAcctYear.SelectedValue;

    // 檢查會計期數
    if (String.Compare(DrpFromperiod.SelectedValue, DrpToperiod.SelectedValue) > 0)
    {
        JsUtility.ClientMsgBoxAjax("起始會計期間不可大於迄止會計期間！", UpdatePanel1, "");
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
        return;
    }

    // 取出報表參數相關資訊
    string myStartDate = "";
    string myEndDate = "";
    string PeriodClose = "";
    
    // 取得起始期間的開始日期
    strSQL = string.Format(@"Select PeriodBegin From 
         dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
         DrpCompany.SelectedValue, myAcctYear, DrpFromperiod.SelectedValue);
    DT = _MyDBM.ExecuteDataTable(strSQL);       
    
    if (DT.Rows.Count != 0)
        myStartDate = DT.Rows[0]["PeriodBegin"].ToString();

    // 取得結束期間的結束日期和關帳狀態
    strSQL = string.Format(@"Select PeriodEnd,PeriodClose From 
         dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
         DrpCompany.SelectedValue, myAcctYear, DrpToperiod.SelectedValue);
    DT = _MyDBM.ExecuteDataTable(strSQL);       
    
    if (DT.Rows.Count != 0)
    {
        myEndDate = DT.Rows[0]["PeriodEnd"].ToString();
        PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
    }

    // 取出公司全名
    string myCompanyName = "";
    strSQL = "SELECT CompanyName From Company Where Company='" + DrpCompany.SelectedValue + "'";
    DT = _MyDBM.ExecuteDataTable(strSQL);       
    if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
    
    // 設定關帳註記
    string myCloseRemark = "";
    if (PeriodClose != "Y") myCloseRemark = "（試算）";
    
    // 設定報表名稱
    string myReportName = " 傳票刪除統計 ";

    // 設定報表參數
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("CompanyName", myCompanyName));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("CloseRemark", myCloseRemark));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("AcctYear", myAcctYear));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("FromPeriod", DrpFromperiod.SelectedValue));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("ToPeriod", DrpToperiod.SelectedValue));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("StartDate", myStartDate));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("EndDate", myEndDate));
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("ReportName", myReportName));

    // 執行查詢 - 取得已刪除傳票資料
    strSQL = @"select VoucherDate,VoucherNo,LstChgDateTime from glvoucherhead
       where company=@company AND RTRIM(dletflag)='Y'
       and voucherdate between @Startdate AND @Enddate ";

    sqlcmd.Parameters.Add("@company", SqlDbType.Char).Value = DrpCompany.SelectedValue;
    sqlcmd.Parameters.Add("@Startdate", SqlDbType.Char).Value = myStartDate;
    sqlcmd.Parameters.Add("@Enddate", SqlDbType.Char).Value = myEndDate;

    DT = _MyDBM.ExecuteDataTable(strSQL, sqlcmd.Parameters, CommandType.Text);
    
    // 設定刪除筆數參數
    string myCountNum = DT.Rows.Count.ToString();
    CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("CountNum", myCountNum));

    // 處理查詢結果
    if (DT.Rows.Count > 0)
    {
        CryReportSource.ReportDocument.SetDataSource(DT);
        CrystalReportViewer1.DataBind();
        ViewState["Sourcedata"] = DT;
        CrystalReportViewer1.Visible = true;
    }
    else
    {
        JsUtility.ClientMsgBoxAjax("查無相關資料！", UpdatePanel1, "");
        CrystalReportViewer1.Visible = false;
    }
           
    JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");    //關閉執行等待畫面
}
```

#### BindAcctYear 方法
根據選擇的公司別，載入對應的會計年度下拉式選單。

```csharp
protected void BindAcctYear(string company)
{
    string strSQL;
    _MyDBM = new DBManger();
    _MyDBM.New();

    DataTable dt = new DataTable();
    strSQL = @"SELECT AcctYear 
                 FROM GLAcctPeriod WHERE company='" + company + "'";
    dt.Clear();
    dt = _MyDBM.ExecuteDataTable(strSQL);

    if (dt.Columns.Count != 0)
    {
        FillDropDownList(DrpAcctYear, "AcctYear", "AcctYear", dt);
    }
}
```

#### FillDropDownList 方法
通用方法，用於填充下拉式選單的資料。

```csharp
protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
{
    DDL.DataSource = dt;
    DDL.DataTextField = SetText;
    DDL.DataValueField = SetValue;
    DDL.DataBind();
}
```

#### DrpCompany_SelectedIndexChanged 方法
處理公司別下拉選單值變更事件，當選擇不同公司時更新會計年度選項。

```csharp
protected void DrpCompany_SelectedIndexChanged(object sender, EventArgs e)
{
    BindAcctYear(DrpCompany.SelectedValue);
}
```

## 技術實現

GLR02R0.aspx.cs 使用以下技術：

1. **ADO.NET 資料存取**：使用 SqlCommand 和參數化查詢安全地存取資料庫
2. **Crystal Reports API**：設置報表參數和資料來源
3. **ASP.NET AJAX**：通過 UpdatePanel 和 ScriptManager 提供部分頁面更新
4. **檢視狀態管理**：使用 ViewState 保存報表資料，優化報表顯示體驗
5. **客戶端腳本注入**：通過 JavaScript 提供等待畫面功能

## 資料存取模式

程式使用以下資料存取模式：

1. **資料庫連接**：使用自定義 DBManger 類別建立和管理資料庫連接
2. **資料查詢**：使用參數化 SQL 查詢保護資料庫免受 SQL 注入
3. **資料轉換**：將查詢結果轉換為適合報表顯示的格式
4. **結果緩存**：在 ViewState 中保存查詢結果，提升分頁效能

## 方法說明

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| Page_PreInit | sender, e | void | 頁面預初始化事件處理 |
| Page_Load | sender, e | void | 頁面載入事件處理 |
| btnQuery_Click | sender, e | void | 查詢按鈕點擊事件處理 |
| BindAcctYear | company | void | 載入指定公司的會計年度 |
| FillDropDownList | DDL, SetValue, SetText, dt | void | 填充下拉式選單控制項 |
| DrpCompany_SelectedIndexChanged | sender, e | void | 公司別選擇變更事件處理 |

## 常用變數

| 變數名稱 | 類型 | 描述 |
|---------|------|------|
| _MyDBM | DBManger | 資料庫管理器實例 |
| _UserInfo | UserInfo | 使用者資訊類別 |
| _ProgramId | string | 程式代號 |
| strSQL | string | SQL 查詢字串 |
| DT | DataTable | 查詢結果資料表 |
| sqlcmd | SqlCommand | SQL 命令和參數 |
| myStartDate | string | 查詢起始日期 |
| myEndDate | string | 查詢結束日期 |
| myAcctYear | string | 會計年度 |
| myCompanyName | string | 公司名稱 |
| myCountNum | string | 刪除傳票數量 |

## 錯誤處理機制

程式使用以下錯誤處理機制：

1. **輸入驗證**：
   - 檢查會計年度格式與有效範圍
   - 驗證起始會計期間不大於結束會計期間

2. **結果檢查**：
   - 處理查詢結果為空的情況
   - 顯示適當的錯誤訊息

3. **介面反饋**：
   - 使用 JsUtility.ClientMsgBoxAjax 顯示警告或錯誤訊息
   - 根據查詢結果控制報表檢視器的可見性

## 代碼優化建議

1. **參數化查詢改進**：
   ```csharp
   // 原始代碼 - 使用字串格式化
   strSQL = string.Format(@"Select PeriodBegin From 
        dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
        DrpCompany.SelectedValue, myAcctYear, DrpFromperiod.SelectedValue);
   
   // 建議改進 - 使用參數化查詢（自定義存儲過程）
   strSQL = "EXEC sp_GetPeriodInfo @Company, @AcctYear, @Period, @InfoType='Begin'";
   sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = DrpCompany.SelectedValue;
   sqlcmd.Parameters.Add("@AcctYear", SqlDbType.Int).Value = myAcctYear;
   sqlcmd.Parameters.Add("@Period", SqlDbType.Char).Value = DrpFromperiod.SelectedValue;
   ```

2. **報表參數設定改進**：
   ```csharp
   // 原始多行代碼
   CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("CompanyName", myCompanyName));
   CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter("CloseRemark", myCloseRemark));
   // ...
   
   // 建議改進 - 使用共用方法
   private void AddReportParameter(string name, string value)
   {
       CryReportSource.Report.Parameters.Add(new CrystalDecisions.Web.Parameter(name, value));
   }
   
   // 使用方式
   AddReportParameter("CompanyName", myCompanyName);
   AddReportParameter("CloseRemark", myCloseRemark);
   ```

3. **錯誤處理改進**：
   ```csharp
   // 建議添加 try-catch 區塊
   try
   {
       // 資料庫操作...
   }
   catch (Exception ex)
   {
       JsUtility.ClientMsgBoxAjax("查詢發生錯誤: " + ex.Message, UpdatePanel1, "");
       // 記錄詳細錯誤日誌
       LogError(ex);
       JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
   }
   ```

## 安全性考量

程式包含以下安全性機制：

1. **參數化查詢**：部分查詢使用參數化 SQL 查詢，降低 SQL 注入風險
2. **輸入驗證**：對會計年度和會計期間進行基本驗證
3. **權限控制**：通過系統整體權限機制控制頁面存取

建議改進的安全性措施：

1. 擴展所有直接字串拼接的 SQL 查詢為參數化查詢
2. 增加更全面的輸入驗證，包括特殊字元過濾
3. 實作更詳細的權限控制，如報表層級的存取權限

## 效能考量

1. **查詢優化**：
   - 使用條件過濾限制返回的資料量
   - 只查詢已刪除的傳票記錄 (RTRIM(dletflag)='Y')

2. **資料緩存**：
   - 使用 ViewState 保存查詢結果，避免重複查詢

3. **頁面更新優化**：
   - 使用 UpdatePanel 實現部分頁面更新
   - 註冊查詢按鈕為 PostBack 控制項，確保等待畫面正確顯示

## 跨瀏覽器兼容性

程式使用標準的 ASP.NET Web Forms 和 AJAX 技術，在大多數瀏覽器中應該能正常運作。Crystal Report Viewer 通常在主流瀏覽器中有良好的支援，但可能需要安裝相應的瀏覽器插件。

## 可維護性考量

1. **程式結構**：
   - 程式結構清晰，各方法有明確的職責
   - 變數命名具有描述性，便於理解

2. **代碼重用**：
   - 使用通用方法 FillDropDownList 填充下拉式選單
   - 多次創建資料庫連接的代碼可以進一步優化

3. **註釋和文檔**：
   - 程式碼中包含基本的註釋說明
   - 可增加更詳細的方法功能描述和參數說明

## 待改進事項

1. 將所有直接拼接的 SQL 查詢改為參數化查詢
2. 添加完整的錯誤處理機制，包括日誌記錄
3. 優化資料庫連接管理，避免重複創建連接
4. 增加報表設計的靈活性，如支援不同格式的輸出選項
5. 改進報表參數的設置方式，使用更具結構性的方法
6. 增加資料匯出功能，便於進一步分析和處理

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/6  | 1.0    | 首次建立科目對帳單程式碼規格書        | Claude AI | 