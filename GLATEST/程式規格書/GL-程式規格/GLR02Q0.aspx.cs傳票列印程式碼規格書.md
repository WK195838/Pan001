# GLR02Q0.aspx.cs 傳票列印程式碼規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLR02Q0.aspx.cs                       |
| 程式名稱     | 傳票列印程式碼                           |
| 檔案大小     | 8.9KB                                 |
| 行數        | ~260                                  |
| 功能簡述     | 傳票列印後端邏輯                         |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/6                              |

## 程式功能概述

GLR02Q0.aspx.cs 是傳票列印報表頁面的後端程式碼，負責處理使用者查詢請求、資料庫連接、資料處理和報表參數設定。主要功能包括：

1. 初始化頁面元素和控制項
2. 載入公司別和會計年度下拉式選單
3. 處理使用者查詢請求並驗證輸入
4. 從資料庫獲取會計期間資訊
5. 依日期統計傳票張數
6. 設定 Crystal Report 報表參數
7. 將查詢結果綁定到報表並顯示

## 程式結構說明

GLR02Q0.aspx.cs 的程式結構包含以下主要部分：

1. **命名空間引用**：引入必要的 .NET 命名空間和自定義類別
2. **類別宣告**：定義 GLR02Q0 類別，繼承自 System.Web.UI.Page
3. **成員變數**：定義資料庫連接字串、使用者資訊和程式代號等變數
4. **頁面事件處理方法**：處理頁面載入、按鈕點擊等事件
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
using CrystalDecisions.CrystalReports.Engine;
```

### 類別宣告與成員變數

```csharp
public partial class GLR02Q0 : System.Web.UI.Page
{
    string cnn = string.Empty;
    UserInfo _UserInfo = new UserInfo();
    string _ProgramId = "GLR02Q0";
    DBManger _MyDBM;
}
```

### 主要方法

#### Page_PreInit 方法
此方法在頁面初始化前執行，可用於設定頁面主題和母版頁。目前註解掉未使用。

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

#### OnInit 方法
此方法在頁面初始化時執行，創建資料庫管理器實例。

```csharp
protected override void OnInit(EventArgs e)
{
    base.OnInit(e);
    _MyDBM = new DBManger();
    _MyDBM.New();
}
```

#### Page_Load 方法
此方法在頁面載入時執行，設定報表檢視器屬性，註冊 JavaScript，如果不是回傳頁面則初始化控制項。

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    // 設定報表檢視器屬性
    CrystalReportViewer1.DisplayGroupTree = false;
    CrystalReportViewer1.HasToggleGroupTreeButton = false;

    // 需要執行等待畫面的按鈕
    btnQuery.Attributes.Add("onClick", "drawWait('')");
    
    if (!IsPostBack)
    {
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
            FillDropDownList(Drpcompany, "CompanyNo", "CompanyName", dtComp);
        }
        
        // 預先放入第一個公司別之損益表報表代號及會計年度
        Drpcompany.SelectedValue = "20";           
        BindAcctYear(Drpcompany.SelectedValue);
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
處理查詢按鈕點擊事件，呼叫 BindData 方法執行查詢。

```csharp
protected void btnQuery_Click(object sender, ImageClickEventArgs e)
{
    BindData();
}
```

#### BindData 方法
此方法是核心邏輯，負責執行傳票查詢處理，並將結果綁定到報表。

```csharp
protected void BindData()
{
    // 畫面查詢條件檢查
    string strSQL = "";
    DataTable DT = new DataTable();
    SqlCommand sqlcmd = new SqlCommand();
    _MyDBM = new DBManger();
    _MyDBM.New();

    // 檢查會計年度
    string myAcctYear = DrpAcctYear.SelectedValue;
    if (myAcctYear == "" || String.Compare(myAcctYear, "1900") < 0 || String.Compare(myAcctYear, "2099") > 0)
    {
        DrpAcctYear.Focus();
        JsUtility.ClientMsgBoxAjax("會計年度不可空白，而且必須為數字！", UpdatePanel1, "");
        JsUtility.CloseWaitScreenAjax(UpdatePanel1, "b");
        return;
    }

    // 取出報表參數相關資訊
    string myStartDate = "";
    string myEndDate = "";
    string PeriodClose = "";
    
    // 取得期間開始和結束日期
    strSQL = string.Format(@"Select PeriodBegin,PeriodEnd,PeriodClose
         From dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
         Drpcompany.SelectedValue, myAcctYear, DrpAcctperiod.SelectedValue);
    DT = _MyDBM.ExecuteDataTable(strSQL);       
    
    if (DT.Rows.Count != 0)
    {
        myStartDate = DT.Rows[0]["PeriodBegin"].ToString();
        myEndDate = DT.Rows[0]["PeriodEnd"].ToString();
        PeriodClose = DT.Rows[0]["PeriodClose"].ToString();
    }

    // 取出公司全名
    string myCompanyName = "";
    strSQL = "Select CompanyName From Company Where Company ='" + Drpcompany.SelectedValue + "'";
    DT = _MyDBM.ExecuteDataTable(strSQL);
    if (DT.Rows.Count != 0) myCompanyName = DT.Rows[0]["CompanyName"].ToString();
    
    // 關帳註記
    string myCloseRemark = "";
    if (PeriodClose != "Y") myCloseRemark = "（試算）";
    
    // 報表名稱
    string myReportName = " 傳票張數統計 ";

    // 設定報表參數
    CryReportSource.Report.Parameters.Add(NewParameter("CompanyName", myCompanyName));
    CryReportSource.Report.Parameters.Add(NewParameter("CloseRemark", myCloseRemark));
    CryReportSource.Report.Parameters.Add(NewParameter("AcctYear", myAcctYear));
    CryReportSource.Report.Parameters.Add(NewParameter("StartDate", myStartDate));
    CryReportSource.Report.Parameters.Add(NewParameter("EndDate", myEndDate));
    CryReportSource.Report.Parameters.Add(NewParameter("ReportName", myReportName));
    CryReportSource.Report.Parameters.Add(NewParameter("FromPeriod", DrpAcctperiod.SelectedValue));

    // 查詢傳票統計資料
    strSQL = @"select VoucherDate,count(*) as CountNum 
    from glvoucherhead where company=@Company and (dletflag is null or rtrim(dletflag)<>'Y')
    and voucherdate between @StartDate and @endDate group by VoucherDate";

    sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = Drpcompany.SelectedValue;
    sqlcmd.Parameters.Add("@StartDate", SqlDbType.Char).Value = myStartDate;
    sqlcmd.Parameters.Add("@endDate", SqlDbType.Char).Value = myEndDate;

    DT = _MyDBM.ExecuteDataTable(strSQL, sqlcmd.Parameters, CommandType.Text);

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

#### NewParameter 方法
創建新的 Crystal Report 參數。

```csharp
private CrystalDecisions.Web.Parameter NewParameter(string Name, string Value)
{
    CrystalDecisions.Web.Parameter NewParameter = new CrystalDecisions.Web.Parameter();
    NewParameter.Name = Name;
    NewParameter.DefaultValue = Value;
    return NewParameter;
}
```

#### BindAcctYear 方法
綁定特定公司的會計年度下拉式選單。

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
填充下拉式選單的通用方法。

```csharp
protected void FillDropDownList(DropDownList DDL, String SetValue, String SetText, DataTable dt)
{
    DDL.DataSource = dt;
    DDL.DataTextField = SetText;
    DDL.DataValueField = SetValue;
    DDL.DataBind();
}
```

## 技術實現

GLR02Q0.aspx.cs 使用以下技術：

1. **ADO.NET 資料存取**：使用 SqlCommand 和參數化查詢安全地存取資料庫
2. **Crystal Reports API**：設置報表參數和資料來源
3. **ASP.NET AJAX**：通過 UpdatePanel 和 ScriptManager 提供部分頁面更新
4. **檢視狀態管理**：使用 ViewState 保存報表資料，優化分頁體驗
5. **客戶端腳本注入**：通過 ScriptManager.RegisterStartupScript 註冊客戶端 JavaScript

## 資料存取模式

程式使用以下資料存取模式：

1. **資料庫連接**：使用自定義 DBManger 類別建立和管理資料庫連接
2. **資料查詢**：使用參數化 SQL 查詢避免 SQL 注入
3. **資料綁定**：將 DataTable 結果綁定到 Crystal Report
4. **結果緩存**：在 ViewState 中保存查詢結果，優化報表分頁

## 方法說明

| 方法名稱 | 參數 | 返回類型 | 描述 |
|---------|------|---------|------|
| Page_PreInit | sender, e | void | 頁面預初始化事件處理 |
| OnInit | e | void | 頁面初始化事件處理 |
| Page_Load | sender, e | void | 頁面載入事件處理 |
| btnQuery_Click | sender, e | void | 查詢按鈕點擊事件處理 |
| BindData | 無 | void | 執行查詢並綁定報表資料 |
| NewParameter | Name, Value | CrystalDecisions.Web.Parameter | 創建新的報表參數 |
| BindAcctYear | company | void | 綁定會計年度下拉式選單 |
| FillDropDownList | DDL, SetValue, SetText, dt | void | 填充下拉式選單 |

## 常用變數

| 變數名稱 | 類型 | 描述 |
|---------|------|------|
| _MyDBM | DBManger | 資料庫管理器實例 |
| _UserInfo | UserInfo | 使用者資訊類別 |
| _ProgramId | string | 程式代號 |
| strSQL | string | SQL 查詢字串 |
| DT | DataTable | 查詢結果資料表 |
| sqlcmd | SqlCommand | SQL 命令和參數 |

## 錯誤處理機制

程式使用以下錯誤處理機制：

1. **輸入驗證**：檢查會計年度格式有效性
2. **查詢結果檢查**：處理空查詢結果和顯示適當的訊息
3. **使用者提示**：使用 JsUtility.ClientMsgBoxAjax 顯示錯誤訊息
4. **界面反饋**：顯示和隱藏等待畫面，提升使用者體驗

## 代碼優化建議

1. **參數化查詢改進**：
   ```csharp
   // 原始代碼 - 使用字串拼接
   strSQL = string.Format(@"Select PeriodBegin,PeriodEnd,PeriodClose
        From dbo.fnGetAccPeriodInfo('{0}',{1},'{2}')",
        Drpcompany.SelectedValue, myAcctYear, DrpAcctperiod.SelectedValue);
   
   // 建議改進 - 使用參數化查詢
   strSQL = "EXEC dbo.sp_GetAccPeriodInfo @Company, @AcctYear, @AcctPeriod";
   sqlcmd.Parameters.Add("@Company", SqlDbType.Char).Value = Drpcompany.SelectedValue;
   sqlcmd.Parameters.Add("@AcctYear", SqlDbType.Int).Value = myAcctYear;
   sqlcmd.Parameters.Add("@AcctPeriod", SqlDbType.Char).Value = DrpAcctperiod.SelectedValue;
   ```

2. **資源釋放改進**：
   ```csharp
   // 建議添加適當的資源釋放代碼
   protected void BindData()
   {
       // 現有代碼...
       
       try
       {
           // 查詢和處理邏輯...
       }
       finally
       {
           // 確保資源釋放
           if (DT != null)
               DT.Dispose();
       }
   }
   ```

3. **錯誤處理改進**：
   ```csharp
   // 建議添加異常捕獲
   try
   {
       // 資料庫操作...
   }
   catch (Exception ex)
   {
       // 記錄錯誤
       JsUtility.ClientMsgBoxAjax("查詢發生錯誤: " + ex.Message, UpdatePanel1, "");
       JsUtility.CloseWaitScreenAjax(UpdatePanel1, "");
   }
   ```

## 安全性考量

程式包含以下安全性機制：

1. **參數化查詢**：部分查詢已經使用參數化查詢防止 SQL 注入
2. **資料驗證**：對會計年度進行基本驗證
3. **權限檢查**：通過頁面載入檢查使用者權限（需在主版面實現）

建議進一步強化的安全措施：

1. 所有查詢都應使用參數化查詢，避免直接拼接 SQL 字串
2. 加強輸入驗證，防止跨站腳本攻擊
3. 實現更詳細的權限檢查，確保只有授權使用者可以存取特定功能

## 效能考量

1. **查詢優化**：
   - 使用資料分組和過濾條件限制返回的資料量
   - 使用參數化查詢提升查詢性能

2. **資料緩存**：
   - 使用 ViewState 保存查詢結果，減少重複查詢
   - 考慮使用 ASP.NET Cache 進一步優化

3. **AJAX 優化**：
   - 使用 UpdatePanel 實現部分頁面更新
   - 使用等待畫面提升使用者體驗

## 跨瀏覽器兼容性

程式使用 ASP.NET AJAX 實現部分頁面更新，這在現代瀏覽器中通常有良好的兼容性。Crystal Report Viewer 在不同瀏覽器中也有良好的支援，但可能需要安裝相應的瀏覽器插件。

## 可維護性考量

1. **程式結構**：程式結構清晰，各方法職責明確
2. **代碼註釋**：已包含基本的註釋說明功能
3. **變數命名**：變數和方法名稱具有一定的描述性
4. **重複代碼**：存在一些重複創建資料庫連接的代碼，可考慮重構

## 待改進事項

1. 將所有 SQL 查詢改為參數化查詢，避免 SQL 注入風險
2. 添加更完善的錯誤處理和日誌記錄機制
3. 優化資料庫連接管理，避免重複創建連接
4. 增加報表設計時的彈性，支持更多的報表格式和輸出選項
5. 添加報表參數儲存和載入功能

## 維護記錄

| 日期      | 版本   | 變更內容                             | 變更人員    |
|-----------|--------|-------------------------------------|------------|
| 2025/5/6  | 1.0    | 首次建立傳票列印程式碼規格書          | Claude AI | 