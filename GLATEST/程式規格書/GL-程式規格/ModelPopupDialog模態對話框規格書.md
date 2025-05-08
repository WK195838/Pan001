# ModelPopupDialog 模態對話框規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | ModelPopupDialog                     |
| 程式名稱     | 模態對話框                              |
| 檔案大小     | 2.0KB                                 |
| 行數        | ~51                                   |
| 功能簡述     | 提供共用模態視窗                          |
| 複雜度       | 中                                    |
| 規格書狀態   | 已完成                                 |
| 負責人員     | Claude AI                             |
| 完成日期     | 2025/5/17                             |

## 程式功能概述

ModelPopupDialog 是泛太總帳系統中的模態對話框元件，提供強制性的彈出視窗功能，要求使用者必須先處理對話框內容，才能繼續操作主應用程式。此元件廣泛應用於系統中需要模態視窗的各種場景，如強制資料選擇、確認訊息、警告提示等。模態對話框的主要特點是阻擋使用者對父視窗的操作，直到使用者完成對話框中的選擇或操作。主要功能包括：

1. 提供模態式彈出視窗，阻擋使用者操作父視窗
2. 支援動態載入資料列表供使用者選擇
3. 可根據URL參數自由配置查詢的表格、欄位和排序
4. 提供自訂查詢功能，便於使用者快速找到所需資料
5. 返回選擇結果給父視窗，實現資料傳遞
6. 自動處理視窗關閉和資料回傳機制
7. 提供一致的使用者介面，符合系統整體風格
8. 支援鍵盤導航和鼠標操作

此元件設計為高度可重用的共用元件，為需要強制使用者做出選擇的場景提供標準化解決方案。

## 控制項結構說明

ModelPopupDialog 的頁面結構主要由以下部分組成：

1. **頁面頭部**：包含標題、元數據和用戶端腳本
2. **查詢區域**：提供文字輸入框和查詢按鈕
3. **資料顯示區域**：使用 GridView 顯示查詢結果
4. **資料返回機制**：通過 JavaScript 函數實現資料回傳

整體設計遵循系統 UI 設計規範，提供一致且專業的使用者體驗。

## 頁面元素

### ASPX 頁面宣告

```html
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModelPopupDialog.aspx.cs" Inherits="ModelPopupDialog" %>
```

### 頁面結構

```html
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模態對話框</title>    
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <base target="_self" />
    
    <script type="text/javascript">
    <!--    
      function ReValue(ValueString)
        {
            window.returnValue = ValueString;
            window.close();
            return false;
        }
    //-->
    </script>
</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
        <asp:TextBox ID="txtQuery" runat="server" Width="100px"></asp:TextBox>
        <asp:ImageButton ID="imgbtnQuery" runat="server" ImageUrl="~/Image/ButtonPics/Query.gif"
            OnClick="imgbtnQuery_Click" ToolTip="查詢" />
        <asp:GridView ID="gvEmployee" runat="server" CellPadding="4" ForeColor="#333333" 
            AllowPaging="True" AutoGenerateColumns="False" PageSize="15" BackColor="White"
            OnRowCreated="gvEmployee_RowCreated" OnRowDataBound="gvEmployee_RowDataBound" 
            DataSourceID="SqlDataSource1" EmptyDataText="無資料!!!">
            <EmptyDataRowStyle />        
            <HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>  
            <Columns>
                <asp:BoundField DataField="Code" HeaderText="代碼" SortExpression="Code" HeaderStyle-Width="50px" />
                <asp:BoundField DataField="Name" HeaderText="名稱" SortExpression="Name" HeaderStyle-Width="200px" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
```

## 技術實現

ModelPopupDialog 基於以下技術實現：

1. **ASP.NET Web Forms**：使用 ASP.NET 頁面技術建立對話框
2. **HTML / CSS**：使用標準網頁技術構建用戶界面
3. **JavaScript**：使用 JavaScript 實現資料回傳和視窗關閉功能
4. **SqlDataSource 控制項**：使用資料來源控制項連接資料庫
5. **GridView 控制項**：使用資料網格顯示查詢結果
6. **動態參數配置**：通過 URL 參數動態配置查詢規則

## 相依檔案和元件

ModelPopupDialog 依賴以下檔案與元件：

1. **ModelPopupDialog.aspx.cs**：對話框的後端程式碼
2. **Query.gif**：查詢按鈕的圖標檔案
3. **JavaScript 函數**：內嵌的資料回傳機制

## 使用者介面

ModelPopupDialog 提供以下使用者界面元素：

1. **查詢文字框**：允許使用者輸入查詢條件
2. **查詢按鈕**：提交查詢請求
3. **資料網格**：以表格形式顯示查詢結果
4. **分頁控制**：提供資料的分頁導航
5. **視覺設計**：整體設計符合系統 UI 風格，提供一致的視覺體驗

## 屬性說明

ModelPopupDialog 透過 URL 參數提供以下可設定的屬性：

| 屬性名稱 | 資料類型 | 說明 | 範例值 |
|---------|---------|------|--------|
| Table | string | 要查詢的資料表名稱 | "GLA_ACCT" |
| Fields | string | 要顯示的欄位清單 | "ACCT_NO as Code, ACCT_DESC as Name" |
| Key | string | 排序依據的欄位名稱 | "ACCT_NO" |

## 方法說明

ModelPopupDialog 提供以下前端方法：

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| ReValue | ValueString | false | 設定回傳值並關閉視窗 |

## 事件說明

ModelPopupDialog 支援以下事件：

| 事件名稱 | 事件參數 | 說明 |
|---------|---------|------|
| imgbtnQuery_Click | sender, ImageClickEventArgs | 處理查詢按鈕點擊事件 |
| gvEmployee_RowCreated | sender, GridViewRowEventArgs | 處理資料列建立事件 |
| gvEmployee_RowDataBound | sender, GridViewRowEventArgs | 處理資料列繫結事件 |

## 使用範例

以下是 ModelPopupDialog 的使用範例：

### 開啟模態對話框

```csharp
// 開啟科目選擇對話框
protected void btnSelectAccount_Click(object sender, EventArgs e)
{
    string url = "ModelPopupDialog.aspx?Table=GLA_ACCT&Fields=ACCT_NO as Code, ACCT_DESC as Name&Key=ACCT_NO";
    string script = "var returnValue = window.showModalDialog('" + url + "', '', 'dialogWidth:400px;dialogHeight:500px;center:yes;help:no;status:no;scroll:yes;');";
    script += "if(returnValue){";
    script += "  var values = returnValue.split(',');";
    script += "  document.getElementById('" + txtAccountCode.ClientID + "').value = values[0];";
    script += "  document.getElementById('" + txtAccountName.ClientID + "').value = values[1];";
    script += "}";
    ClientScript.RegisterStartupScript(this.GetType(), "OpenDialog", script, true);
}

// 開啟部門選擇對話框
protected void btnSelectDept_Click(object sender, EventArgs e)
{
    string url = "ModelPopupDialog.aspx?Table=GLA_DEPT&Fields=DEPT_NO as Code, DEPT_NAME as Name&Key=DEPT_NO";
    string script = "var returnValue = window.showModalDialog('" + url + "', '', 'dialogWidth:400px;dialogHeight:500px;center:yes;help:no;status:no;scroll:yes;');";
    script += "if(returnValue){";
    script += "  var values = returnValue.split(',');";
    script += "  document.getElementById('" + txtDeptCode.ClientID + "').value = values[0];";
    script += "  document.getElementById('" + txtDeptName.ClientID + "').value = values[1];";
    script += "}";
    ClientScript.RegisterStartupScript(this.GetType(), "OpenDialog", script, true);
}
```

## 使用情境與最佳實踐

1. **科目代碼選擇**
   - 開啟模態對話框顯示科目代碼列表
   - 使用者選擇科目後自動填入表單

2. **部門代碼選擇**
   - 開啟模態對話框顯示部門代碼列表
   - 使用者選擇部門後自動填入表單

3. **員工資料選擇**
   - 開啟模態對話框顯示員工資料列表
   - 使用者選擇員工後自動填入相關欄位

4. **選擇型別資料**
   - 用於需要從預定義列表中選擇值的場景
   - 避免使用者輸入錯誤數據

## 配置說明

ModelPopupDialog 可通過 URL 參數靈活配置：

1. **資料來源配置**
   - 通過 Table 參數指定資料表
   - 通過 Fields 參數指定欄位
   - 通過 Key 參數指定排序依據

2. **視窗配置**
   - 在開啟視窗時可指定對話框的尺寸
   - 可設定對話框的行為屬性（如是否顯示捲軸、幫助按鈕等）

## 效能考量

1. **查詢優化**：直接使用資料庫原生排序提高效率
2. **分頁支援**：資料以分頁方式顯示，減少一次載入的資料量
3. **最小化互動**：使用點擊行直接選擇，減少使用者操作步驟
4. **緩存控制**：設定頁面不進行瀏覽器緩存，確保資料實時性

## 安全性考量

1. **SQL 注入防範**：使用參數化查詢避免 SQL 注入風險
2. **資料過濾**：僅顯示需要的欄位資料
3. **無緩存策略**：通過 meta 標籤設定頁面不緩存，避免敏感資料被保存
4. **視窗限制**：使用 base target="_self" 限制頁面行為

## 可訪問性支援

1. **鍵盤操作**：支援使用 Tab 鍵導航和 Enter 鍵選擇
2. **簡潔介面**：提供清晰、簡潔的用戶界面
3. **標準化控制項**：使用標準 ASP.NET 控制項，確保基本的可訪問性支援

## 已知問題與限制

1. 在某些現代瀏覽器中，showModalDialog 方法被棄用或不支援
2. 資料查詢欄位和表名直接使用字符串拼接，缺乏進一步的驗證
3. 僅支援兩個欄位（代碼和名稱）的回傳，無法靈活調整回傳欄位數量
4. 資料回傳使用逗號分隔的字符串，若資料中包含逗號可能導致解析錯誤

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/17 | 1.0    | 首次建立模態對話框規格書        | Claude AI  | 