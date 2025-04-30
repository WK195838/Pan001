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
| 建立日期 | [初次建立日期] |
| 建立人員 | [初次建立人員] |
| 最後修改日期 | [最後修改日期] |
| 最後修改人員 | [最後修改人員] |

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

## 4. 畫面規格

### 4.1 畫面布局

```
+--------------------------------------------------+
|                  系統標題 & LOGO                  |
+--------------------------------------------------+
| 使用者資訊 | 公司別 | 功能選單 | 登出按鈕 | 幫助  |
+--------------------------------------------------+
|                                                  |
|                                                  |
|                                                  |
|                                                  |
|             內容頁面 (ContentPlaceHolder)         |
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
| HeaderLogo | Image | 系統標誌 | 顯示系統LOGO |
| lblSysName | Label | 系統名稱 | 顯示系統標題 |
| lblUserName | Label | 使用者名稱 | 顯示目前登入使用者 |
| ddlCompany | DropDownList | 公司別選擇 | 允許切換作業公司 |
| menuMain | Menu | 主選單 | 顯示主要功能選單 |
| btnLogout | Button | 登出按鈕 | 登出系統 |
| btnHelp | Button | 幫助按鈕 | 顯示幫助資訊 |
| MainContent | ContentPlaceHolder | 內容區域 | 顯示子頁面內容 |
| lblMessage | Label | 系統訊息 | 顯示系統訊息和提示 |
| lblVersion | Label | 版本資訊 | 顯示系統版本號 |
| lblCopyright | Label | 版權資訊 | 顯示版權聲明 |

### 4.3 事件處理

| 事件名稱 | 觸發條件 | 處理邏輯 |
|---------|---------|---------|
| Page_Load | 頁面載入 | 驗證用戶登入狀態，載入選單權限 |
| Page_PreRender | 頁面渲染前 | 更新使用者資訊與訊息顯示 |
| ddlCompany_SelectedIndexChanged | 公司別選擇變更 | 切換作業公司，重載相關頁面資料 |
| btnLogout_Click | 點擊登出按鈕 | 執行登出處理，清除工作階段 |
| menuMain_MenuItemClick | 點擊選單項目 | 導航至相應功能頁面 |
| btnHelp_Click | 點擊幫助按鈕 | 開啟幫助文件或說明 |

### 4.4 畫面流程

1. 使用者登入系統後進入含主版面的頁面
2. 系統驗證登入狀態及工作階段有效性
3. 載入使用者權限與可用選單配置
4. 顯示主版面與對應內容頁
5. 使用者可透過選單導航至不同功能
6. 使用者可切換作業公司
7. 使用者可登出系統回到登入頁面

## 5. 處理邏輯

### 5.1 主要流程

```
開始
 ↓
檢查使用者登入狀態 → 未登入 → 重定向至登入頁面
 ↓
檢查工作階段有效性 → 已失效 → 重定向至登入頁面
 ↓
從 Session 讀取用戶資訊
 ↓
載入用戶權限資料
 ↓
依據用戶權限產生主選單
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
   - 檢查工作階段中的使用者識別資訊
   - 若不存在有效識別資訊則跳轉至登入頁
   - 定期驗證工作階段有效性並自動延長
   - 閒置超時自動登出處理

2. 選單權限控制：
   - 讀取用戶所屬群組權限設定
   - 依權限過濾可見選單項目
   - 動態產生符合權限的選單結構
   - 儲存選單結構到工作階段以提升效能

3. 公司切換處理：
   - 保存當前公司設定到工作階段
   - 依據公司別過濾資料存取範圍
   - 切換公司時更新相關頁面資料

### 5.3 頁面生命週期管理

1. 版面初始化：
   - 檢查瀏覽器相容性
   - 設定全域JavaScript變數
   - 載入所需CSS與JavaScript檔案
   - 註冊AJAX控制項

2. 內容頁整合：
   - 將內容頁面嵌入主版面的ContentPlaceHolder
   - 提供共用的頁面方法給內容頁使用
   - 處理頁面間資料傳遞邏輯

3. 工作階段管理：
   - 檢查工作階段有效期
   - 使用者活動時自動延長工作階段
   - 工作階段即將到期時顯示警告提示
   - 支援平行工作階段處理

### 5.4 例外處理

1. 認證例外：
   - 捕獲未驗證用戶的存取嘗試
   - 記錄存取資訊到安全日誌
   - 重定向至登入頁面並顯示錯誤訊息

2. 權限例外：
   - 攔截無權限的功能存取嘗試
   - 記錄存取嘗試資訊到安全日誌
   - 顯示「權限不足」訊息頁面

3. 系統例外：
   - 全域例外攔截處理
   - 記錄例外詳情到錯誤日誌
   - 顯示使用者友善的錯誤頁面
   - 嚴重錯誤自動通知系統管理員

## 6. SQL查詢

### 6.1 主要查詢

```sql
-- 查詢使用者選單權限
SELECT 
    M.MenuID, M.MenuName, M.MenuParentID, M.MenuURL, M.MenuOrder, M.IconCss
FROM 
    SYS_MENU M
INNER JOIN 
    SYS_AUTHORITY A ON M.MenuID = A.MenuID
INNER JOIN 
    SYS_GROUP G ON A.GroupID = G.GroupID
INNER JOIN 
    SYS_USER U ON U.GroupID = G.GroupID
WHERE 
    U.UserID = @UserID
    AND M.IsActive = 1
    AND A.AuthorityType >= 1
ORDER BY 
    M.MenuParentID, M.MenuOrder
```

```sql
-- 查詢使用者可存取公司
SELECT 
    C.CompanyID, C.CompanyName, C.CompanyShortName
FROM 
    SYS_COMPANY C
INNER JOIN 
    SYS_USER_COMPANY UC ON C.CompanyID = UC.CompanyID
WHERE 
    UC.UserID = @UserID
    AND C.IsActive = 1
ORDER BY 
    C.CompanyID
```

```sql
-- 查詢系統參數
SELECT 
    S.ParamKey, S.ParamValue, S.ParamDesc
FROM 
    SYS_SYS S
WHERE 
    S.ParamGroup = 'SYSTEM'
    AND S.IsActive = 1
```

```sql
-- 查詢系統訊息
SELECT TOP 5
    M.MessageID, M.MessageTitle, M.MessageContent, 
    M.MessageType, M.CreateDate
FROM 
    SYS_MESSAGE M
WHERE 
    (M.TargetUserID = @UserID OR M.TargetUserID IS NULL)
    AND M.ExpireDate >= GETDATE()
    AND M.IsRead = 0
ORDER BY 
    M.CreateDate DESC
```

### 6.2 資料新增

(主版面程式主要不涉及資料新增操作)

### 6.3 資料更新

```sql
-- 更新使用者最後活動時間
UPDATE SYS_USER
SET 
    LastActiveTime = GETDATE()
WHERE 
    UserID = @UserID
```

```sql
-- 更新訊息已讀狀態
UPDATE SYS_MESSAGE
SET 
    IsRead = 1,
    ReadTime = GETDATE()
WHERE 
    MessageID = @MessageID
    AND (TargetUserID = @UserID OR TargetUserID IS NULL)
```

### 6.4 資料刪除

(主版面程式主要不涉及資料刪除操作)

## 7. 程式碼說明

### 7.1 重要方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| ValidateUserSession | 驗證使用者工作階段 | - | bool: 工作階段是否有效 |
| LoadUserMenu | 載入使用者選單 | UserID: 使用者ID | DataTable: 選單資料 |
| BuildMenu | 建立選單結構 | menuData: 選單資料表 | void |
| ChangeCompany | 切換作業公司 | companyID: 公司編號 | bool: 切換是否成功 |
| CheckAuthorization | 檢查功能權限 | menuID: 選單ID | bool: 是否具有權限 |
| LogUserActivity | 記錄使用者活動 | activity: 活動描述 | void |
| ShowMessage | 顯示系統訊息 | message: 訊息內容, type: 訊息類型 | void |
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
    
    // 定期更新資料庫中的使用者活動時間
    TimeSpan updateSpan = DateTime.Now - (DateTime)(Session["LastDBUpdate"] ?? DateTime.MinValue);
    if (updateSpan.TotalMinutes > 5)
    {
        UpdateUserLastActiveTime(Session["UserID"].ToString());
        Session["LastDBUpdate"] = DateTime.Now;
    }
    
    return true;
}

// 建立使用者選單
private void BuildMenu(DataTable menuData)
{
    // 清除現有選單
    menuMain.Items.Clear();
    
    if (menuData == null || menuData.Rows.Count == 0)
    {
        return;
    }
    
    // 先建立根選單項目
    DataRow[] rootMenus = menuData.Select("MenuParentID IS NULL OR MenuParentID = ''");
    foreach (DataRow rootMenu in rootMenus)
    {
        string menuID = rootMenu["MenuID"].ToString();
        string menuName = rootMenu["MenuName"].ToString();
        string menuUrl = rootMenu["MenuURL"].ToString();
        string iconCss = rootMenu["IconCss"].ToString();
        
        MenuItem menuItem = new MenuItem();
        menuItem.Text = menuName;
        menuItem.Value = menuID;
        
        if (!string.IsNullOrEmpty(menuUrl))
        {
            menuItem.NavigateUrl = menuUrl;
        }
        
        // 設定圖示CSS類別
        if (!string.IsNullOrEmpty(iconCss))
        {
            menuItem.ImageUrl = iconCss;
        }
        
        // 遞迴建立子選單
        BuildSubMenu(menuItem, menuData, menuID);
        
        // 加入主選單
        menuMain.Items.Add(menuItem);
    }
}

// 遞迴建立子選單
private void BuildSubMenu(MenuItem parentMenuItem, DataTable menuData, string parentMenuID)
{
    // 查找子選單項目
    DataRow[] childMenus = menuData.Select("MenuParentID = '" + parentMenuID + "'");
    
    foreach (DataRow childMenu in childMenus)
    {
        string menuID = childMenu["MenuID"].ToString();
        string menuName = childMenu["MenuName"].ToString();
        string menuUrl = childMenu["MenuURL"].ToString();
        string iconCss = childMenu["IconCss"].ToString();
        
        MenuItem menuItem = new MenuItem();
        menuItem.Text = menuName;
        menuItem.Value = menuID;
        
        if (!string.IsNullOrEmpty(menuUrl))
        {
            menuItem.NavigateUrl = menuUrl;
        }
        
        // 設定圖示CSS類別
        if (!string.IsNullOrEmpty(iconCss))
        {
            menuItem.ImageUrl = iconCss;
        }
        
        // 遞迴處理下一層子選單
        BuildSubMenu(menuItem, menuData, menuID);
        
        // 加入父選單
        parentMenuItem.ChildItems.Add(menuItem);
    }
}

// 切換公司處理
protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
{
    string selectedCompanyID = ddlCompany.SelectedValue;
    
    // 檢查是否有權限存取該公司
    DataTable companyTable = GetUserCompanies(Session["UserID"].ToString());
    bool hasAccess = false;
    
    foreach (DataRow row in companyTable.Rows)
    {
        if (row["CompanyID"].ToString() == selectedCompanyID)
        {
            hasAccess = true;
            break;
        }
    }
    
    if (!hasAccess)
    {
        ShowMessage("您沒有存取此公司的權限", "error");
        return;
    }
    
    // 更新工作階段中的公司資訊
    Session["CompanyID"] = selectedCompanyID;
    Session["CompanyName"] = ddlCompany.SelectedItem.Text;
    
    // 如果在內容頁面中有實作ICompanyAware介面，則通知公司變更
    ContentPlaceHolder contentPlaceHolder = FindControl("MainContent") as ContentPlaceHolder;
    if (contentPlaceHolder != null && contentPlaceHolder.Page != null)
    {
        ICompanyAware companyAware = contentPlaceHolder.Page as ICompanyAware;
        if (companyAware != null)
        {
            companyAware.OnCompanyChanged(selectedCompanyID);
        }
    }
    
    // 重新整理頁面以反映公司變更
    Response.Redirect(Request.RawUrl);
}

// 處理登出
protected void btnLogout_Click(object sender, EventArgs e)
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
                document.getElementById('<%= btnLogout.ClientID %>').click();
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

// 頁面載入完成後初始化工作階段監控
$(document).ready(function() {
    initSessionMonitor();
});

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

## 8. 測試規格

### 8.1 單元測試

| 測試項目 | 測試內容 | 預期結果 |
|---------|---------|---------|
| 工作階段驗證 | 測試未登入狀態或工作階段過期的自動重定向 | 自動重定向至登入頁面 |
| 選單權限控制 | 以不同權限角色登入，驗證選單顯示差異 | 僅顯示具有權限的選單項目 |
| 公司切換功能 | 測試切換公司時的資料更新與畫面刷新 | 成功切換公司，相關資料正確更新 |
| 工作階段管理 | 測試閒置超時警告與自動登出功能 | 閒置時間到達警告期間，顯示警告；超過時間自動登出 |
| 多語系支援 | 測試語系切換功能，確認界面顯示正確語系 | 正確顯示所選語系的標籤與訊息 |

### 8.2 整合測試

| 測試項目 | 測試內容 | 預期結果 |
|---------|---------|---------|
| 內容頁整合 | 測試內容頁面在主版面中的正確顯示 | 內容頁面正確嵌入主版面的ContentPlaceHolder |
| 主版面與內容頁互動 | 測試主版面與內容頁面間的資料與事件交換 | 資料正確傳遞，事件正確觸發 |
| 主版面事件透傳 | 測試主版面事件(如公司變更)對內容頁的通知 | 內容頁正確接收事件並作出反應 |
| 共用功能使用 | 測試內容頁對主版面共用功能的呼叫 | 共用功能正確執行 |
| 選單導航功能 | 測試選單項目點擊時的頁面導航 | 正確導航至目標頁面 |

### 8.3 效能測試

| 測試項目 | 測試內容 | 預期結果 |
|---------|---------|---------|
| 載入時間 | 測量主版面及選單載入時間 | 載入時間<1秒 |
| 記憶體使用 | 監控主版面元件的記憶體使用情況 | 記憶體使用維持在合理範圍 |
| 瀏覽器相容性 | 測試主版面在各主流瀏覽器中的顯示與功能 | 在所有目標瀏覽器中正確顯示與運作 |
| 並發處理 | 測試多用戶同時登入使用系統時的效能 | 系統保持穩定運作，無明顯延遲 |
| 資源使用效率 | 測試CSS和JavaScript檔案的載入與處理效率 | 資源載入最佳化，無阻塞頁面渲染 |

### 8.4 使用者驗收測試

| 測試項目 | 測試內容 | 預期結果 |
|---------|---------|---------|
| 功能完整性 | 依據規格全面測試主版面功能 | 所有功能正常運作 |
| 易用性評估 | 評估使用者介面的易用性與人體工學 | 使用者可輕鬆掌握界面操作 |
| 外觀一致性 | 檢查各頁面在主版面下的外觀一致性 | 所有頁面保持一致的外觀風格 |
| 錯誤處理 | 測試各種錯誤情境下的系統反應 | 系統優雅處理錯誤，提供清晰訊息 |
| 整體使用體驗 | 評估整體系統使用流暢度 | 操作流暢，無明顯卡頓或不便之處 |

## 9. 相關檔案

### 9.1 原始程式檔案

| 檔案名稱 | 檔案類型 | 檔案大小 | 檔案行數 | 說明 |
|---------|---------|---------|---------|------|
| GLA.master | ASPX | 11KB | 245 | 系統主版面頁面 |
| GLA.master.cs | C# | 14KB | 397 | 系統主版面後端程式碼 |
| MasterStyles.css | CSS | 5.6KB | 189 | 主版面樣式表 |
| MasterScripts.js | JavaScript | 3.2KB | 98 | 主版面專用腳本 |
| ICompanyAware.cs | C# | 1KB | 20 | 公司變更通知介面 |

### 9.2 相依元件檔案

| 檔案名稱 | 檔案類型 | 說明 |
|---------|---------|------|
| LoginClass.cs | C# | 登入處理類別 |
| AppAuthority.cs | C# | 權限管理類別 |
| CryptoHelper.cs | C# | 加密處理類別 |
| pagefunction.js | JavaScript | 共用頁面函數 |
| bootstrap.min.css | CSS | Bootstrap框架樣式 |
| jquery.min.js | JavaScript | jQuery函式庫 |
| ModPopFunction.js | JavaScript | 彈出窗口控制 |
| Busy.js | JavaScript | 忙碌指示器 |

## 10. 修改歷史

| 版本號 | 修改日期 | 修改人員 | 修改內容 | 備註 |
|-------|---------|---------|---------|------|
| 1.0.0 | [日期] | [人員] | 初版建立 | 初次建立程式規格書 |

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