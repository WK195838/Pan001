# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | GLADetail.master |
| 程式名稱 | 詳細內容頁主版面 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | GLATEST/GLATEST/GLADetail.master, GLATEST/GLATEST/GLADetail.master.cs |
| 程式類型 | 主版面程式 |
| 建立日期 | 2023/11/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2023/11/05 |
| 最後修改人員 | PanPacific開發團隊 |

## 2. 功能概述

### 2.1 主要功能

GLADetail.master是泛太總帳系統的詳細內容頁主版面(Master Page)，專用於顯示系統中各功能的詳細資料頁面與表單。此版面提供了更簡單的頁面布局，專注於詳細內容的呈現，適用於新增、編輯、查詢等需要展示詳細資料的操作介面。它與GLA.master的主要區別在於布局結構更為精簡，去除了左側功能選單，僅保留頂部標題與主要內容區域，更適合彈出式視窗和專注型操作介面。

### 2.2 業務流程

本程式作為系統基礎架構，支援以下主要業務流程：
1. 提供資料表單的詳細操作介面
2. 支援彈出式視窗操作，如資料維護與查詢
3. 確保各類詳細頁面的一致性外觀與使用者體驗
4. 處理返回主畫面、資料傳遞等共通功能
5. 提供獨立於主畫面的操作環境

### 2.3 使用頻率

- 使用頻率：極高
- 使用時機：
  - 進行資料新增或編輯作業時
  - 查看詳細資料時
  - 在彈出視窗中顯示表單
  - 需要獨立於主畫面的操作環境時

### 2.4 使用者角色

- 系統管理員：基本資料設定與維護
- 會計人員：資料輸入與編輯作業
- 財務主管：資料審核與核准作業
- 審計人員：詳細資料查閱
- 一般使用者：依權限使用相關功能

## 3. 系統架構

### 3.1 技術架構

- 前端：ASP.NET Web Form、HTML5、CSS3、JavaScript
- 後端：C#、ASP.NET 4.0
- 頁面模板：Master Page
- 用戶端腳本：jQuery 1.4.4、jQuery UI 1.8.7
- 頁面交互：AJAX、ScriptManager

### 3.2 資料表使用

此主版面程式主要不直接存取資料表，資料操作主要由內容頁面實現。

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| jquery-1.4.4.min.js | jQuery函式庫 | 提供DOM操作 |
| jquery-ui-1.8.7.custom.min.js | jQuery UI函式庫 | 提供界面元件 |
| ui.datepicker.js | 日期選擇器 | 提供日期選擇功能 |
| ui.datepicker.tw.js | 民國年日期選擇器 | 提供民國年日期格式支援 |
| pagefunction.js | 頁面函數 | 提供共用的JavaScript功能 |
| Busy.js | 忙碌指示器 | 顯示資料處理中的狀態指示 |
| iBOSSiteStyle.css | 網站樣式 | 定義整體頁面布局樣式 |
| iBosGridStyle.css | 表格樣式 | 定義網格與資料顯示樣式 |
| StyleBtn.css | 按鈕樣式 | 定義系統按鈕外觀 |
| loadingbox.css | 載入框樣式 | 定義資料載入時的視覺效果 |

## 4. 畫面規格

### 4.1 畫面布局

```
+--------------------------------------------------+
|                  系統標題 & LOGO                  |
+--------------------------------------------------+
| 語言選擇                             回首頁按鈕  |
+--------------------------------------------------+
|                                                  |
|                                                  |
|                   詳細標題列                     |
|                                                  |
|                                                  |
|                                                  |
|                                                  |
|                                                  |
|            內容頁面 (ContentPlaceHolder)         |
|                                                  |
|                                                  |
|                                                  |
|                                                  |
|                                                  |
+--------------------------------------------------+
|              系統訊息 & 版權資訊                  |
+--------------------------------------------------+
```

### 4.2 控制項說明

| 控制項名稱 | 控制項類型 | 用途 | 說明 |
|-----------|----------|------|------|
| form1 | Form | 主表單 | 包含所有控制項的主表單 |
| ScriptManager1 | ScriptManager | 腳本管理器 | 管理AJAX與客戶端腳本 |
| lbllanguage | Label | 語言標籤 | 顯示"變更語言"文字 |
| ddllanguage | DropDownList | 語言選擇 | 提供中文/英文的選擇 |
| lbhome | LinkButton | 返回首頁按鈕 | 返回系統首頁 |
| detailtab | HtmlGenericControl | 標題區域 | 包含詳細內容的標題區域 |
| lbldetailTitle | Label | 詳細標題 | 顯示詳細頁面的標題文字 |
| MainContent | ContentPlaceHolder | 內容區域 | 用於承載子頁面內容 |
| hfUserId | HiddenField | 用戶ID隱藏欄位 | 儲存當前用戶ID |
| hfUserName | HiddenField | 用戶名稱隱藏欄位 | 儲存當前用戶名稱 |
| hfLoginAccount | HiddenField | 登入帳號隱藏欄位 | 儲存當前登入帳號 |
| hfSideMenuIndex | HiddenField | 側邊選單索引 | 記錄目前選中的側邊選單項 |
| hfTopNavIndex | HiddenField | 頂部選單索引 | 記錄目前選中的頂部導航項 |
| hfSubNavIndex | HiddenField | 子選單索引 | 記錄目前選中的子導航項 |

### 4.3 事件處理

| 事件名稱 | 觸發條件 | 處理邏輯 |
|---------|---------|---------|
| Page_Load | 頁面載入 | 1. 註冊必要的JavaScript檔案<br>2. 確認日期格式設定<br>3. 初始化UI元件 |
| lbhome_Click | 點擊回首頁按鈕 | 導向至系統首頁(Systems.aspx) |

### 4.4 畫面流程

1. 使用者從主畫面點擊需要查看詳細資料或進行編輯的功能
2. 系統開啟包含GLADetail.master的詳細內容頁
3. 詳細內容頁顯示表單或資料，使用者可進行查看或編輯操作
4. 使用者完成操作後，可點擊"回首頁"按鈕返回系統首頁
5. 若為彈出視窗，則使用者關閉視窗後返回原頁面

## 5. 處理邏輯

### 5.1 主要流程

```
開始
 ↓
頁面初始化
 ↓
載入必要的JavaScript與CSS資源
 ↓
檢查日期格式設定(民國年/西元年)
 ↓
載入對應的日期處理腳本
 ↓
初始化UI元件與事件處理
 ↓
顯示詳細內容頁
 ↓
處理使用者交互操作
 ↓
結束
```

### 5.2 頁面管理機制

1. 頁面初始化：
   - 載入必要的JavaScript庫與CSS樣式表
   - 根據系統參數決定日期格式(民國年/西元年)
   - 初始化頁面控制項與事件處理

2. 日期格式處理：
   - 檢查TWCalendar系統參數
   - 若為"Y"則載入民國年日期處理腳本
   - 設定日期選擇器的本地化參數

3. 首頁導航：
   - 提供"回首頁"按鈕實現快速返回
   - 透過Response.Redirect實現頁面導航

### 5.3 前端互動實現

主版面使用jQuery實現多種前端互動效果：

1. **日期選擇器**：
```javascript
// 日期選擇器設定
$(".datepicker").datepicker({
    changeMonth: true,
    changeYear: true,
    dateFormat: "yy/mm/dd",
    dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
    monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
    showButtonPanel: true,
    closeText: "關閉",
    currentText: "今天"
});
```

2. **選單處理**：
```javascript
// 初始sidemenu定位
doSideMenuInit();
// 初始topnav定位
doTopNavInit();
doSubNavInit();
```

3. **進度顯示**：
```javascript
// Navigation button
$(".btn").live("click", function () {
    $.showprogress();
});
$(".ddl").live("change", function () {
    $.showprogress();
});
```

### 5.4 多語系支援

系統支援繁體中文與英文兩種語言：

1. **語言選擇機制**：
```html
<asp:DropDownList ID="ddllanguage" runat="server" AutoPostBack="True" meta:resourcekey="ddllanguageResource1">
    <asp:ListItem meta:resourcekey="ListItemResource1" Value="zh-TW">中文</asp:ListItem>
    <asp:ListItem meta:resourcekey="ListItemResource2" Value="en-US">英文</asp:ListItem>
</asp:DropDownList>
```

2. **資源標籤**：
```html
<asp:Label ID="lbllanguage" runat="server" meta:resourcekey="lbllanguageResource1" Text="變更語言"></asp:Label>
```

3. **日期格式本地化**：
```csharp
// 決定是否使用民國年
if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
{
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", 
        Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
}
```

## 6. 程式碼說明

### 6.1 重要方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入處理 | sender: 事件源<br>e: 事件參數 | void |
| lbhome_Click | 處理回首頁按鈕點擊 | sender: 事件源<br>e: 事件參數 | void |
| doSideMenuInit | 初始化側邊選單 | - | void (JavaScript) |
| doTopNavInit | 初始化頂部選單 | - | void (JavaScript) |
| doSubNavInit | 初始化子選單 | - | void (JavaScript) |

### 6.2 關鍵程式碼

```csharp
// 頁面載入處理
protected void Page_Load(object sender, EventArgs e)
{
    // 註冊核心JavaScript庫
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", 
        Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", 
        Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", 
        Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
    
    // 註冊功能相關腳本
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", 
        Page.ResolveUrl("~/Pages/pagefunction.js").ToString());

    // 決定是否使用民國年
    if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
    {
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", 
            Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
    }
    
    // 用於執行等待畫面
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", 
        Page.ResolveUrl("~/Pages/Busy.js").ToString());
}

// 處理回首頁按鈕點擊
protected void lbhome_Click(object sender, EventArgs e)
{
    Response.Redirect("http://ibosdvlp/WebSite/Systems.aspx", true);
}
```

### 6.3 JavaScript代碼

```javascript
// 初始化sidemenu
function doSideMenuInit() {
    var sidemenuindex = $("#<%=hfSideMenuIndex.ClientID %>").val();
    var isidemenuindex = 1;
    if (sidemenuindex != "") {
        var isidemenuindex = parseInt(sidemenuindex);
    }
    $("#spanSideMenuIndex").text(isidemenuindex);
    $("div.sidemenu > div > a").removeAttr("style");
    $("div.sidemenu > div > a").hover(function () {
        $(this).css("color", "#fff");
    }, function () {
        $(this).css("color", "#0F2A71");
    });
    $("div.sidemenu > div > a").eq(isidemenuindex - 1).css({ "background-position": "0 0", "color": "#fff" });
    $("div.sidemenu > div > a").eq(isidemenuindex - 1).hover(function () {
        $(this).css("color", "#fff");
    });
}
```

## 7. 使用指南

### 7.1 如何使用此主版面

1. **建立繼承頁面**：
```csharp
<%@ Page Language="C#" MasterPageFile="~/GLADetail.master" AutoEventWireup="true" CodeFile="YourPage.aspx.cs" Inherits="YourPage" %>
```

2. **設定內容區域**：
```html
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- 您的頁面內容 -->
</asp:Content>
```

3. **設定頁面標題**：
在頁面的後台代碼中添加：
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    // 設定頁面標題
    ((Label)Master.FindControl("lbldetailTitle")).Text = "您的頁面標題";
}
```

### 7.2 適用場景

1. **資料維護表單**：
   - 客戶資料維護(PTA0150_M.aspx)
   - 會計科目維護
   - 總帳交易維護

2. **查詢表單**：
   - 明細資料查詢
   - 交易紀錄查詢

3. **彈出視窗**：
   - 選擇性資料查詢
   - 參數設定
   - 批次處理

### 7.3 與GLA.master的區別

1. **布局結構**：
   - GLA.master：包含完整的導航框架與側邊選單
   - GLADetail.master：精簡布局，無側邊選單，適合表單操作

2. **使用情境**：
   - GLA.master：主要功能頁面與系統導航
   - GLADetail.master：詳細資料維護、彈出視窗、表單填寫

3. **導航功能**：
   - GLA.master：提供完整的模組選單與功能導航
   - GLADetail.master：僅提供回首頁功能，專注於當前操作

## 8. 操作指南

### 8.1 表單設計建議

1. **標準布局**：
   - 上方放置標題與主要操作按鈕
   - 中間放置表單欄位（建議使用表格布局）
   - 下方放置確認/取消按鈕

2. **控制項使用**：
   - 文字欄位：使用asp:TextBox
   - 下拉選單：使用asp:DropDownList
   - 日期選擇：使用具有.datepicker類別的文字欄位
   - 功能按鈕：使用帶有.btn類別的asp:Button

3. **驗證設計**：
   - 必填欄位使用asp:RequiredFieldValidator
   - 數值驗證使用asp:RangeValidator與asp:CompareValidator
   - 錯誤訊息統一顯示於表單下方

### 8.2 常見問題與解決方案

1. **頁面標題未顯示**：
   - 確保正確使用FindControl("lbldetailTitle")
   - 在Page_Load階段設定標題文字

2. **日期選擇器無效**：
   - 確認TextBox已添加.datepicker類別
   - 檢查jQuery與jQuery UI是否正確載入
   - 檢查日期格式設定是否符合需求

3. **按鈕點擊無進度指示**：
   - 確認按鈕已添加.btn類別
   - 檢查Busy.js是否正確載入
   - 檢查jQuery是否正確初始化

## 9. 附件

### 9.1 相依檔案與元件

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| GLADetail.master | ASPX | 詳細內容頁主版面 |
| GLADetail.master.cs | C# | 詳細內容頁後端程式碼 |
| jquery-1.4.4.min.js | JavaScript | jQuery核心庫 |
| jquery-ui-1.8.7.custom.min.js | JavaScript | jQuery UI擴展庫 |
| ui.datepicker.js | JavaScript | 日期選擇器腳本 |
| ui.datepicker.tw.js | JavaScript | 民國年日期格式支援 |
| pagefunction.js | JavaScript | 頁面通用函數 |
| Busy.js | JavaScript | 進度指示器 |
| iBOSSiteStyle.css | CSS | 系統主要樣式表 |
| iBosGridStyle.css | CSS | 表格樣式表 |
| StyleBtn.css | CSS | 按鈕樣式表 |
| loadingbox.css | CSS | 載入提示樣式 |

### 9.2 頁面結構示意圖

```
GLADetail.master
  |
  ├─ <head>
  |   └─ ContentPlaceHolder: head (可添加頁面特定樣式與腳本)
  |
  ├─ <body>
  |   ├─ <div class="page">
  |   |   |
  |   |   ├─ <div class="header"> (標題區域)
  |   |   |   └─ 系統標題、語言選擇、回首頁按鈕
  |   |   |
  |   |   ├─ <div class="middleframe"> (內容區域)
  |   |   |   ├─ <div id="detailtab"> (詳細標題)
  |   |   |   └─ ContentPlaceHolder: MainContent (主要內容)
  |   |   |
  |   |   └─ <div class="footer"> (頁尾區域)
  |   |       └─ 版權宣告
  |   |
  |   └─ <script> (JavaScript初始化)
  |
  └─ 頁面結束
```

## 10. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2023/11/05 | PanPacific開發團隊 | 初版建立 | 初次建立程式規格書 |

## 11. 備註與注意事項

### 11.1 開發建議

1. 使用此主版面建立彈出視窗頁面時，應考慮以下因素：
   - 視窗大小適合內容（建議在開啟視窗時指定合適的寬度與高度）
   - 避免使用過多的複雜控制項影響載入速度
   - 提供清晰的操作指引與按鈕（確定/取消）

2. 表單設計遵循一致性：
   - 所有標籤與欄位保持對齊
   - 必填欄位統一標示（如使用*號）
   - 錯誤訊息位置統一
   - 功能按鈕位置一致

3. 資料驗證策略：
   - 簡單驗證使用ASP.NET驗證控制項
   - 複雜邏輯驗證在服務端實現
   - 即時反饋使用JavaScript實現

### 11.2 使用限制

1. 此主版面不適用於需要複雜導航的頁面
2. 不適合用於需要多選項切換的頁面
3. 若需要完整的系統功能選單，應使用GLA.master

### 11.3 相容性注意事項

1. 支援的瀏覽器版本：
   - Internet Explorer 8及以上
   - Firefox、Chrome最新版本
   - 建議使用IE8相容模式以確保最佳體驗

2. 螢幕解析度建議：
   - 最低1024x768
   - 建議1280x1024或更高

3. JavaScript依賴：
   - 必須啟用JavaScript
   - 依賴jQuery 1.4.4與jQuery UI 1.8.7