# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | GLA.master |
| 程式名稱 | 系統主版面 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 總帳模組 |
| 檔案位置 | GLATEST/GL/GLA.master, GLATEST/GL/GLA.master.cs |
| 程式類型 | 主版面程式 |
| 建立日期 | 2023/10/30 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2023/10/30 |
| 最後修改人員 | PanPacific開發團隊 |

## 2. 功能概述

### 2.1 主要功能

GLA.master 是泛太總帳系統的主版面(Master Page)，提供整個應用程式的基本版面配置，包含系統標題、選單、登入資訊顯示及共用的頁面框架。此版面為所有應用程式頁面提供一致的外觀與操作介面，處理全域的使用者身份驗證、選單權限控制、工作階段管理等基本功能。所有應用程式頁面都繼承此主版面以確保使用者體驗一致性。

### 2.2 業務流程

本程式作為系統基礎架構，支援以下主要業務流程：
1. 使用者登入系統後，透過主版面顯示個人化的選單與功能
2. 使用者在系統內頁面間切換時，主版面保持不變，確保操作一致性
3. 根據使用者權限動態調整可見選單與功能
4. 提供系統訊息通知機制，顯示重要系統狀態
5. 監控使用者工作階段有效性，適時提醒或自動登出

### 2.3 使用頻率

- 使用頻率：極高
- 使用時機：系統中任何頁面的存取都涉及主版面的載入與處理

### 2.4 使用者角色

- 系統管理員：全功能存取權限
- 一般會計人員：依據授權可見適當功能選單
- 財務主管：可存取報表與核准功能
- 審計人員：查詢與報表功能
- 一般使用者：基本功能存取

## 3. 系統架構

### 3.1 技術架構

- 前端：ASP.NET Web Form、HTML5、CSS3、JavaScript
- 後端：C#、ASP.NET 4.0
- 頁面模板：Master Page
- 用戶端腳本：jQuery、Bootstrap
- 頁面交互：AJAX

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_USER | 使用者資料 | 讀取 |
| SYS_GROUP | 使用者群組 | 讀取 |
| SYS_MENU | 系統選單設定 | 讀取 |
| SYS_AUTHORITY | 使用者權限設定 | 讀取 |
| SYS_COMPANY | 公司基本資料 | 讀取 |
| SYS_SYS | 系統參數設定 | 讀取 |
| SYS_MESSAGE | 系統訊息資料 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| LoginClass | 登入模組 | 處理使用者驗證與工作階段管理 |
| CheckAuthorization | 權限檢查 | 檢查使用者操作權限 |
| CryptoHelper | 加密模組 | 提供資料加密與解密功能 |
| Busy.js | 忙碌指示器 | 顯示資料處理中的狀態指示 |
| pagefunction.js | 頁面函數 | 提供共用的JavaScript功能 |
| ModPopFunction.js | 彈出窗口函數 | 提供彈出窗口控制 |
| jquery-1.4.4.min.js | jQuery函式庫 | 提供DOM操作 |
| jquery-ui-1.8.7.custom.min.js | jQuery UI函式庫 | 提供界面元件 |
| StyleBtn.css | 按鈕樣式 | 定義系統按鈕外觀 |
| iBOSSiteStyle.css | 網站樣式 | 定義整體頁面布局樣式 |
| iBosGridStyle.css | 表格樣式 | 定義網格與資料顯示樣式 |

## 4. 畫面規格

### 4.1 畫面布局

```
+--------------------------------------------------+
|                  系統標題 & LOGO                  |
+--------------------------------------------------+
| 使用者資訊 | 語言選擇 | 功能選單 | 登出按鈕 | 幫助  |
+--------------------------------------------------+
|                                                  |
|     |                                            |
|     |                                            |
| 側  |                                            |
| 邊  |                                            |
| 選  |        內容頁面 (ContentPlaceHolder)        |
| 單  |                                            |
|     |                                            |
|     |                                            |
|     |                                            |
+--------------------------------------------------+
|              系統訊息 & 版權資訊                  |
+--------------------------------------------------+
```

### 4.2 控制項說明

| 控制項名稱 | 控制項類型 | 用途 | 說明 |
|-----------|----------|------|------|
| form1 | Form | 主表單 | 包含所有控制項的主表單 |
| lbllanguage | Label | 語言標籤 | 顯示"變更語言"文字 |
| ddllanguage | DropDownList | 語言選擇 | 提供中文/英文的選擇 |
| lblUser | Label | 用戶標籤 | 顯示"用戶:"文字 |
| txtUser | Label | 用戶名稱 | 顯示當前登入的用戶名稱 |
| lbLogout | LinkButton | 登出按鈕 | 用於登出系統 |
| ltlLeftMenu | Literal | 側邊選單 | 動態產生的左側主選單 |
| ltlTop1Menu | Literal | 頂部選單 | 動態產生的頂部子選單 |
| hfUserId | HiddenField | 用戶ID隱藏欄位 | 儲存當前用戶ID |
| hfUserName | HiddenField | 用戶名稱隱藏欄位 | 儲存當前用戶名稱 |
| hfLoginAccount | HiddenField | 登入帳號隱藏欄位 | 儲存當前登入帳號 |
| hfSideMenuIndex | HiddenField | 側邊選單索引 | 記錄目前選中的側邊選單項 |
| hfTopNavIndex | HiddenField | 頂部選單索引 | 記錄目前選中的頂部導航項 |
| hfSubNavIndex | HiddenField | 子選單索引 | 記錄目前選中的子導航項 |
| cphEEOC | ContentPlaceHolder | 內容區域 | 用於承載子頁面內容 |

### 4.3 事件處理

| 事件名稱 | 觸發條件 | 處理邏輯 |
|---------|---------|---------|
| Page_Load | 頁面載入 | 1. 驗證用戶登入狀態<br>2. 註冊必要的JavaScript檔案<br>3. 初始化選單與導航項<br>4. 載入用戶資訊與權限資料 |
| Page_PreRender | 頁面渲染前 | 更新使用者資訊與訊息顯示 |
| lbLogout_Click | 點擊登出按鈕 | 1. 記錄登出活動<br>2. 清除用戶工作階段<br>3. 導向至登入頁面 |
| ddllanguage_SelectedIndexChanged | 語言選擇變更 | 1. 更新UI顯示語言<br>2. 保存語言偏好設定 |
| sidemenu切換 | 點擊側邊選單項 | 1. 更新選單樣式<br>2. 記錄選中索引<br>3. 處理頁面導航 |
| topnav切換 | 點擊頂部導航 | 1. 更新導航樣式<br>2. 記錄選中索引<br>3. 顯示對應子導航 |

### 4.4 畫面流程

1. 使用者從AuthAD.aspx登入系統，驗證通過後進入包含GLA.master的頁面
2. 系統根據用戶權限自動生成左側主選單與頂部功能選單
3. 使用者可透過選單導航至不同功能頁面
4. 內容頁面在主版面框架內顯示，主版面不變
5. 使用者可隨時切換語言或登出系統
6. 工作階段超時前，系統自動顯示提醒
7. 使用者選擇登出或超時後，系統導向至登入頁面

## 5. 處理邏輯

### 5.1 主要流程

```
開始
 ↓
檢查使用者登入狀態 → 未登入 → 重定向至登入頁面(AuthAD.aspx)
 ↓
檢查工作階段有效性 → 已失效 → 重定向至登入頁面
 ↓
載入用戶資訊(UserInfo類別)
 ↓
載入選單資料(MenuManager類別)
 ↓
依據用戶權限產生動態選單(RegisterLeftMenu/RegisterTopMenu方法)
 ↓
顯示主版面與對應內容頁
 ↓
監聽用戶操作事件
 ↓
根據事件執行對應動作
 ↓
結束
```

### 5.2 使用者認證與權限控制

1. 用戶認證流程：
   - 使用FormsAuth.Authorization類負責身份驗證
   - 認證成功後將用戶資訊保存在UserInfo物件與Session變數中
   - 提供AD域集成認證功能(透過LdapAuthentication類)
   - 設定工作階段Cookie來維持用戶登入狀態

2. 選單權限控制：
   - 使用MenuManager類處理選單資料
   - 依據SYS_AUTHORITY表中的權限設定過濾MenuData集合
   - 使用CreateLeftMenuLT方法動態產生左側選單
   - 使用RegisterTopMenu方法動態產生頂部選單

3. 公司切換處理：
   - 公司資料存儲於Session["Company"]
   - 通過公司別篩選各功能模組的資料範圍
   - 切換公司時通知相關頁面更新資料(ICompanyAware介面)

### 5.3 頁面生命週期管理

1. 版面初始化(OnInit/Page_Load)：
   - 檢查用戶登入狀態與工作階段有效性
   - 載入必要的JavaScript與CSS檔案
   - 從Session提取用戶資訊與當前系統設定

2. 內容頁整合：
   - 使用ASP.NET ContentPlaceHolder機制(cphEEOC)
   - 支援子頁面與主版面交互(透過FindControl方法)
   - 維護統一的頁面樣式與操作方式

3. 前端交互處理：
   - 使用jQuery處理DOM操作與事件綁定
   - 支援選單的動態切換與樣式更新
   - 使用jQuery UI提供的datepicker等控制項

### 5.4 例外處理

1. 認證例外：
   - 捕獲未驗證用戶的存取嘗試
   - 使用Response.Redirect重定向至登入頁面
   - 通過URL參數(ReturnUrl)指定登入後返回頁面

2. 權限例外：
   - 檢查用戶對功能的存取權限(CheckAuthorization控制項)
   - 使用try-catch區塊捕獲權限相關異常
   - 記錄權限檢查失敗的情況

3. 系統例外：
   - 實作Application_Error全域例外處理
   - 使用ErrorMessage屬性記錄與顯示錯誤訊息
   - 系統層級錯誤導向至自訂錯誤頁面

## 6. 實作細節

### 6.1 選單生成機制

選單生成使用三層結構：
1. **側邊主選單(LeftMenu)**：對應系統主要功能模組，使用Literal控制項動態產生HTML
2. **頂部導航(TopNav)**：對應當前模組的功能群組，使用ul/li結構實現標籤式設計
3. **次級導航(SubNav)**：提供功能細項的導航，使用嵌套結構動態顯示與隱藏

選單HTML生成方式：
```csharp
// 左側主選單產生範例
private void RegisterLeftMenu()
{
    StringBuilder sbFun = new StringBuilder();
    foreach (MenuData data in menuDataLT.FindAll(p => p.location == "Left"))
    {
        sbFun.AppendFormat("<div><a href=\"{0}\">{1}</a></div>", 
            Page.ResolveUrl("~/" + data.ProgUrl), data.ProgName);
    }
    ltlLeftMenu.Text = sbFun.ToString();
}

// 頂部導航產生範例
private void RegisterTopMenu(string parentid, string location)
{
    StringBuilder sbFun = new StringBuilder();
    sbFun.Append("<ul id=\"topnav\">");
    // 產生一級導航項
    foreach (MenuData data in menuDataLT.FindAll(p => p.ParentProgId == parentid && p.location == location))
    {
        sbFun.AppendFormat("<li><a href=\"{0}\">{1}</a>", 
            Page.ResolveUrl("~/" + data.ProgUrl), data.ProgName);
        // 產生子導航項
        sbFun.Append(RegisterSubMenu(data.ProgId, "Top2"));
        sbFun.Append("</li>");
    }
    sbFun.Append("</ul>");
    ltlTop1Menu.Text = sbFun.ToString();
}
```

### 6.2 前端互動實現

主版面使用jQuery實現多種前端互動效果：

1. **選單反白效果**：根據當前位置更新選單項樣式
```javascript
// sidemenu切換處理
$("div.sidemenu > div").live("click", function() {
    var index = $("div.sidemenu > div").index(this);
    $("#hfSideMenuIndex").val(index + 1);
    $("div.sidemenu > div > a").removeAttr("style");
    $("div.sidemenu > div > a").eq(index).css({"background-position": "0 0", "color": "#fff"});
});
```

2. **頁面大小調整**：動態調整頁面框架元素高度
```javascript
// 框架高度皆相等
$('.menuleft').height($('.content').height());
$('.menuright').height($('.content').height());
$('.contentleft').height($('.content').height());
$('.contentright').height($('.content').height());
```

3. **日期控制項**：使用jQuery UI日期選擇器並支援國際化
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

### 6.3 多語系實現

系統支援繁體中文與英文兩種語言：

1. **語言切換機制**：透過下拉選單與工作階段變數控制
```csharp
protected void ddllanguage_SelectedIndexChanged(object sender, EventArgs e)
{
    // 儲存語言設定至Session
    Session["Language"] = ddllanguage.SelectedValue;
    // 設定頁面文化資訊
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddllanguage.SelectedValue);
    // 重新載入頁面
    Response.Redirect(Request.RawUrl);
}
```

2. **資源文件管理**：使用App_GlobalResources與meta:resourcekey屬性
```xml
<asp:Label ID="lbllanguage" runat="server" meta:resourcekey="lbllanguageResource1"
    Text="變更語言"></asp:Label>
```

3. **日期格式本地化**：根據語言設定調整日期格式
```csharp
if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
{
    // 使用民國年顯示
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", 
        Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
}
```

## 7. 程式碼說明

### 7.1 重要方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| ValidateUserSession | 驗證使用者工作階段 | - | bool: 工作階段是否有效 |
| LoadUserMenu | 載入使用者選單 | UserID: 使用者ID | DataTable: 選單資料 |
| RegisterLeftMenu | 建立左側選單HTML | - | void |
| RegisterTopMenu | 建立頂部選單HTML | parentid: 父選單ID<br>location: 位置代碼 | void |
| RegisterSubMenu | 建立子選單HTML | parentid: 父選單ID<br>location: 位置代碼 | string: HTML代碼 |
| ChangeUser | 切換使用者 | name: 用戶名<br>id: 用戶ID<br>loginaccount: 登入帳號 | void |
| lbLogout_Click | 處理登出事件 | sender: 事件源<br>e: 事件參數 | void |
| GetSystemParameter | 取得系統參數 | paramKey: 參數代碼 | string: 參數值 |

### 7.2 關鍵程式碼

```csharp
// 驗證使用者工作階段
private bool ValidateUserSession()
{
    if (Session["UserID"] == null)
    {
        // 未登入，跳轉至登入頁
        Response.Redirect("~/AuthAD.aspx", true);
        return false;
    }
    
    // 檢查工作階段有效期
    if (Session["LastActivity"] != null)
    {
        DateTime lastActivity = (DateTime)Session["LastActivity"];
        TimeSpan span = DateTime.Now - lastActivity;
        
        int timeoutMinutes = Convert.ToInt32(GetSystemParameter("SESSION_TIMEOUT"));
        if (span.TotalMinutes > timeoutMinutes)
        {
            // 工作階段超時，清除工作階段資料
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/AuthAD.aspx?reason=timeout", true);
            return false;
        }
    }
    
    // 更新最後活動時間
    Session["LastActivity"] = DateTime.Now;
    return true;
}

// 註冊客戶端腳本
protected void Page_Load(object sender, EventArgs e)
{
    // 註冊核心JavaScript庫
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", 
        Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", 
        Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
    
    // 註冊功能相關腳本
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", 
        Page.ResolveUrl("~/Pages/pagefunction.js").ToString());
    Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", 
        Page.ResolveUrl("~/Pages/Busy.js").ToString());
    
    if (!IsPostBack)
    {
        // 初始化選單
        InitMenuData();
        RegisterLeftMenu();
        RegisterTopMenu(menuData.ProgId, menuData.location);
    }
}

// 處理登出
protected void lbLogout_Click(object sender, EventArgs e)
{
    // 記錄登出活動
    if (Session["UserID"] != null)
    {
        LogUserActivity("User logged out");
    }
    
    // 清除工作階段資料
    Session.Clear();
    Session.Abandon();
    
    // 使用FormsAuthentication登出
    FormsAuthentication.SignOut();
    
    // 導航至登入頁面
    Response.Redirect("~/AuthAD.aspx?action=logout", true);
}
```

### 7.3 用戶端腳本

```javascript
// 工作階段監控腳本
function initSessionMonitor() {
    var timeoutWarning = parseInt('<%= GetSystemParameter("SESSION_WARNING") %>') * 60 * 1000;
    var timeoutExpiry = parseInt('<%= GetSystemParameter("SESSION_TIMEOUT") %>') * 60 * 1000;
    var warningTimer;
    var expiryTimer;

    // 重設計時器
    function resetTimers() {
        clearTimeout(warningTimer);
        clearTimeout(expiryTimer);
        
        // 設定新的計時器
        warningTimer = setTimeout(function() {
            showTimeoutWarning();
        }, timeoutWarning);
        
        expiryTimer = setTimeout(function() {
            sessionTimeout();
        }, timeoutExpiry);
    }
    
    // 顯示工作階段即將到期警告
    function showTimeoutWarning() {
        var minutesLeft = Math.round((timeoutExpiry - timeoutWarning) / 60000);
        var warningMsg = '您的工作階段將在 ' + minutesLeft + ' 分鐘後到期。請儲存您的工作。';
        
        // 使用模組化對話框顯示警告
        ModPopFunction.showConfirm(
            '工作階段警告', 
            warningMsg, 
            '繼續工作', 
            '立即登出',
            function() {
                // 用戶選擇繼續，延長工作階段
                extendSession();
            },
            function() {
                // 用戶選擇登出
                document.getElementById('<%= lbLogout.ClientID %>').click();
            }
        );
    }
    
    // 工作階段到期處理
    function sessionTimeout() {
        window.location.href = '<%= ResolveUrl("~/AuthAD.aspx?reason=timeout") %>';
    }
    
    // 延長工作階段
    function extendSession() {
        // 透過AJAX呼叫服務端方法延長工作階段
        $.ajax({
            type: "POST",
            url: '<%= ResolveUrl("~/WSDialogData.asmx/ExtendSession") %>',
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                if (response.d) {
                    // 工作階段已延長，重設計時器
                    resetTimers();
                }
            },
            error: function(xhr, status, error) {
                console.error("延長工作階段失敗：" + error);
            }
        });
    }
    
    // 使用者活動監控
    function setupActivityMonitoring() {
        // 使用者活動重設計時器
        $(document).on("click keypress mousemove", function() {
            resetTimers();
        });
    }
    
    // 初始化
    resetTimers();
    setupActivityMonitoring();
}

// 系統訊息顯示
function showSystemMessage(message, type) {
    var msgContainer = $('#systemMessageContainer');
    var msgClass = 'msg-' + (type || 'info');
    
    // 清除現有訊息
    msgContainer.empty();
    
    // 建立新訊息元素
    var msgElement = $('<div></div>')
        .addClass('system-message')
        .addClass(msgClass)
        .html(message);
    
    // 加入訊息元素
    msgContainer.append(msgElement);
    
    // 顯示訊息並設定自動隱藏
    msgContainer.fadeIn(300).delay(5000).fadeOut(500);
}
```

## 8. 操作手冊

### 8.1 主版面使用說明

1. **選單導航**：
   - 左側主選單用於切換主要功能模組
   - 頂部導航顯示當前模組的功能群組
   - 每個功能模組都有專屬的子頁面，載入於內容區域

2. **系統訊息**：
   - 操作成功或失敗的訊息會顯示在頁面頂部
   - 系統提示訊息會自動消失或需點擊關閉按鈕

3. **用戶設定**：
   - 頂部區域顯示當前登入用戶名稱
   - 可切換系統顯示語言(中文/英文)
   - 登出按鈕用於安全退出系統

### 8.2 維護與擴展指南

1. **新增選單項目**：
   - 在MenuManager.cs的CreateLeftMenuLT方法中添加MenuData物件
   - 指定ParentProgId、ProgId、ProgName和ProgUrl等屬性
   - 依據location屬性分配至左側或頂部選單

2. **修改樣式**：
   - 主要樣式定義在iBOSSiteStyle.css與iBosGridStyle.css
   - 按鈕樣式定義在StyleBtn.css
   - 自定義控制項樣式定義在App_Themes下的主題檔案

3. **系統參數配置**：
   - Web.config中的appSettings區段包含系統配置
   - SYS_SYS表儲存動態系統參數，可在運行時修改

### 8.3 常見問題與解決方案

1. **選單不顯示或顯示不完整**：
   - 檢查用戶權限設定(SYS_AUTHORITY表)
   - 確認MenuManager.cs中的選單定義是否正確
   - 檢查登入用戶的工作階段是否有效

2. **樣式顯示異常**：
   - 確認所有CSS檔案已正確載入
   - 檢查瀏覽器相容性模式設定(推薦IE8相容模式)
   - 清除瀏覽器快取後重新載入頁面

3. **工作階段過期問題**：
   - 檢查Web.config中的工作階段逾時設定
   - 確認SESSION_TIMEOUT參數設定合理
   - 調整SESSION_WARNING以提供更早的提醒

## 9. 附件

### 9.1 資料表結構參考

```sql
-- 系統選單表
CREATE TABLE SYS_MENU (
    MenuID varchar(20) NOT NULL,
    MenuName nvarchar(50) NOT NULL,
    MenuParentID varchar(20) NULL,
    MenuURL varchar(200) NULL,
    MenuOrder int NOT NULL,
    IconCss varchar(50) NULL,
    IsActive bit NOT NULL DEFAULT 1,
    CONSTRAINT PK_SYS_MENU PRIMARY KEY (MenuID)
)

-- 使用者權限表
CREATE TABLE SYS_AUTHORITY (
    AuthorityID int IDENTITY(1,1) NOT NULL,
    GroupID varchar(20) NOT NULL,
    MenuID varchar(20) NOT NULL,
    AuthorityType tinyint NOT NULL, -- 1:可見 2:可讀 3:可寫 4:可刪 5:可執行
    CONSTRAINT PK_SYS_AUTHORITY PRIMARY KEY (AuthorityID)
)

-- 系統參數表
CREATE TABLE SYS_SYS (
    ParamID int IDENTITY(1,1) NOT NULL,
    ParamGroup varchar(20) NOT NULL,
    ParamKey varchar(50) NOT NULL,
    ParamValue nvarchar(500) NOT NULL,
    ParamDesc nvarchar(200) NULL,
    IsActive bit NOT NULL DEFAULT 1,
    CONSTRAINT PK_SYS_SYS PRIMARY KEY (ParamID)
)
```

### 9.2 選單結構設計

主版面使用三級選單結構：

1. **一級選單(Left)**：主要功能模組
   - GLA01: 日常帳務
   - GLA02: 日常作業
   - GLR01: 報表管理1
   - GLR02: 報表管理2
   - GLR03: 報表管理3
   - GLC01: 結帳處理
   - GLD01: 資料設定
   - GLB01: 總帳基本資料

2. **二級選單(Top1)**：功能群組
   - GLA0101: 傳票登錄
   - GLA0102: 傳票更正作業
   - GLA0103: 傳票核准作業
   - GLA0104: 傳票列印
   - GLA0105: 傳票核准取消作業
   - GLA0106: 傳票過帳作業

3. **三級選單(Top2)**：功能細項
   - 根據功能需求動態產生

## 10. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | 2023/10/30 | PanPacific開發團隊 | 初版建立 | 初次建立程式規格書 |
| 1.0.1 | 2023/11/05 | PanPacific開發團隊 | 補充實作細節與操作指引 | 增加實際程式碼與使用說明 |

## 11. 備註與注意事項

### 11.1 已知問題

1. IE11瀏覽器中部分CSS3特效顯示異常，已加入條件性處理
2. 在高並發情況下工作階段管理可能產生額外延遲
3. 複雜的權限結構可能導致選單生成時間增加

### 11.2 未來改進計劃

1. 導入responsive設計，提升行動裝置支援
2. 優化選單載入機制，採用非同步載入方式
3. 改進工作階段管理，實作分散式工作階段
4. 實現主題切換功能，允許用戶自訂介面風格
5. 加強安全性，導入內容安全策略(CSP)

### 11.3 系統配置注意事項

1. web.config需正確設定工作階段逾時參數
2. 依環境不同需調整日誌記錄等級
3. 發布時需確保JavaScript/CSS檔案壓縮與版本控制
4. 資料庫連線字串應依環境設定適當權限

### 11.4 特殊實作說明

1. 選單權限控制採用角色型存取控制(RBAC)模型
2. 工作階段保存使用加密Cookie和伺服器工作階段雙重機制
3. 跨頁面資料傳遞使用工作階段變數而非URL參數
4. 使用自訂的ViewState加密提升安全性
5. 實作頁面國際化支援，可動態切換多種語系 