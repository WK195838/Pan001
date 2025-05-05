# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | GLA.master |
| 程式名稱 | 泛太總帳系統主版面 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/GLA.master, /GLATEST/app/GLA.master.cs |
| 程式類型 | 主版面(Master Page) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

泛太總帳系統主版面(GLA.master)是整個系統的容器頁面，提供統一的外觀和導航結構，並包含所有頁面共用的元素。它定義了系統的整體布局、標題區、功能選單、登入資訊顯示、系統訊息通知區域等共用元素，所有內容頁面都嵌入於此主版面中，確保使用者體驗的一致性。

### 2.2 業務流程

GLA.master在泛太總帳系統中扮演以下角色：
1. 提供統一的使用者介面框架
2. 處理全域性的使用者驗證和授權
3. 管理系統導航和功能選單
4. 顯示系統公告和重要訊息
5. 提供共用功能如語系切換、密碼變更、登出等
6. 整合各子系統模組，形成一致的操作環境

### 2.3 使用頻率

- 持續使用：主版面在系統運行期間始終存在，所有頁面都嵌入於此
- 每次頁面切換時重新加載主版面的部分功能

### 2.4 使用者角色

- 系統所有使用者，包括：
  - 系統管理員
  - 財務主管
  - 會計人員
  - 稽核人員
  - 一般使用者
  - 唯讀使用者

## 3. 系統架構

### 3.1 技術架構

- 前端：HTML5, CSS3, JavaScript, jQuery 1.4.4, jQuery UI 1.8.7
- 框架：ASP.NET Web Forms (.NET Framework 4.0)
- 後端：C#, ADO.NET
- 視覺元素：Bootstrap 3.3.7
- 字型圖示：Font Awesome 4.7.0

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| USER_INFO | 使用者資訊表 | 讀取 |
| USER_AUTH | 使用者權限表 | 讀取 |
| SYS_MSG | 系統訊息表 | 讀取 |
| SYS_MENU | 系統選單表 | 讀取 |
| USER_SETTING | 使用者設定表 | 讀取/寫入 |
| SYS_LOG | 系統日誌表 | 寫入 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| CheckAuthorization.ascx | 權限檢查 | 檢查使用者權限 |
| Navigator.ascx | 導航控制項 | 提供頁面導航功能 |
| LoginClass | 登入模組 | 提供系統登入功能 |
| AppAuthority | 權限管理 | 提供系統權限控制 |
| pagefunction.js | 頁面函數 | 提供頁面共用功能 |
| GLA.css | 主要樣式表 | 定義系統外觀樣式 |

## 4. 畫面規格

### 4.1 畫面布局

```
+------------------------------------------------------------------------------+
|                            泛太總帳系統 HEADER                                |
+-------------------------------+----------------------------------------------+
| 公司LOGO                      | 使用者資訊  |  語系  |  設定  |  登出        |
+-------------------------------+----------------------------------------------+
|                      系統主選單 (功能模組導航)                               |
+-------------------------------+----------------------------------------------+
| 子選單 (左側導航區)           |                                              |
|                               |                                              |
| - 基本資料維護                |                                              |
|   - 會計科目                  |                                              |
|   - 部門資料                  |                                              |
|   - 總帳交易                  |                                              |
|                               |                                              |
| - 報表功能                    |            內容頁面 (ContentPlaceHolder)     |
|   - 餘額試算表                |                                              |
|   - 損益表                    |                                              |
|   - 資產負債表                |                                              |
|   - 部門損益表                |                                              |
|                               |                                              |
| - 系統管理                    |                                              |
|   - 使用者管理                |                                              |
|   - 權限設定                  |                                              |
|   - 系統參數                  |                                              |
|                               |                                              |
+-------------------------------+----------------------------------------------+
|                       系統訊息區 (通知與錯誤訊息)                           |
+------------------------------------------------------------------------------+
|                  頁尾資訊 (版權、版本號、聯絡資訊等)                        |
+------------------------------------------------------------------------------+
```

### 4.2 元素說明

| 元素名稱 | 類型 | 功能說明 | 行為描述 |
|---------|------|---------|---------|
| 系統標題 | 文字 | 顯示系統名稱 | 固定顯示「泛太總帳系統」 |
| 公司LOGO | 圖片 | 顯示公司標誌 | 點擊後導向系統首頁 |
| 使用者資訊 | 文字區塊 | 顯示當前登入使用者資訊 | 顯示格式：「使用者: [姓名] ([帳號]) 部門: [部門名稱]」 |
| 語系切換 | 下拉選單 | 切換系統顯示語言 | 提供繁體中文、簡體中文、英文選項 |
| 設定按鈕 | 按鈕 | 提供個人設定功能 | 點擊開啟個人設定選單 |
| 登出按鈕 | 按鈕 | 登出系統 | 點擊後登出並返回登入頁面 |
| 系統主選單 | 選單列 | 提供主要功能模組導航 | 滑鼠懸停時顯示子選單 |
| 子選單 | 樹狀選單 | 提供功能頁面導航 | 點擊項目導向對應功能頁面 |
| 內容區塊 | ContentPlaceHolder | 顯示子頁面內容 | 依據導航載入不同功能頁面 |
| 系統訊息區 | 文字區塊 | 顯示系統通知和錯誤訊息 | 可自動淡出或手動關閉 |
| 頁尾資訊 | 文字區塊 | 顯示版權與系統資訊 | 固定顯示版權聲明及系統版本 |

### 4.3 樣式規格

| 元素 | 字型 | 顏色 | 尺寸/邊距 | 其他 |
|------|------|------|----------|------|
| 系統標題 | Arial, 粗體 | #003366 | 20px | 置中對齊 |
| 使用者資訊 | Arial | #333333 | 12px | 靠右對齊 |
| 主選單 | Arial, 粗體 | #FFFFFF 背景:#003366 | 14px | 水平排列, 5px內部間距 |
| 子選單 | Arial | #333333 背景:#F5F5F5 | 12px | 垂直排列, 懸停效果 |
| 系統訊息 | Arial | 錯誤:#FF0000 警告:#FF9900 資訊:#006699 | 12px | 附帶圖示 |
| 頁尾 | Arial | #666666 | 11px | 置中對齊, 1px 上邊框線 |

### 4.4 響應式設計

GLA.master支援下列解析度的響應式設計調整：

| 解析度範圍 | 調整方式 |
|-----------|---------|
| 寬度 > 1200px | 完整兩欄布局，左側導航固定寬度240px |
| 768px < 寬度 <= 1200px | 兩欄布局，左側導航縮小至180px |
| 480px < 寬度 <= 768px | 單欄布局，左側導航可收合顯示 |
| 寬度 <= 480px | 最小化布局，所有選單折疊，使用下拉按鈕顯示 |

## 5. 處理邏輯

### 5.1 主要流程

```
開始
 ↓
使用者請求頁面
 ↓
檢查Session是否存在 → 不存在 → 重定向至登入頁面 → 結束
 ↓
檢查使用者授權  → 未授權 → 顯示錯誤訊息 → 結束
 ↓
載入主版面框架與樣式
 ↓
根據使用者角色載入對應選單
 ↓
載入使用者設定 (語系、界面設定等)
 ↓
檢查系統訊息是否需顯示
 ↓
載入請求的內容頁面
 ↓
顯示完整頁面
 ↓
結束
```

### 5.2 選單載入邏輯

```
開始
 ↓
從Session獲取使用者資訊
 ↓
從SYS_MENU資料表載入所有選單項目
 ↓
從USER_AUTH資料表載入使用者權限
 ↓
依據權限篩選允許的選單項目
 ↓
組織選單層級結構(父子關係)
 ↓
依據使用者語系顯示對應的選單文字
 ↓
渲染選單項目
 ↓
結束
```

### 5.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| Session超時 | 重定向至登入頁面 | 顯示「會話已過期，請重新登入」訊息 |
| 權限不足 | 記錄錯誤並顯示授權錯誤頁面 | 顯示「無權限存取此功能」訊息 |
| 資料庫連接失敗 | 記錄錯誤並顯示系統錯誤頁面 | 顯示「系統暫時無法使用，請稍後再試」訊息 |
| JavaScript錯誤 | 捕獲錯誤並記錄到日誌 | 顯示使用者友好的錯誤訊息 |
| 頁面載入超時 | 設定載入超時處理 | 顯示「頁面載入超時，請檢查網絡連接」訊息 |

### 5.4 權限控制邏輯

權限控制採用角色基礎存取控制 (RBAC) 機制：

1. 依據使用者角色控制選單項目顯示
2. 使用AppAuthority類別檢查細部功能權限
3. 採用三級權限控制：
   - 可讀權限：允許查看資料
   - 可寫權限：允許修改資料
   - 管理權限：允許執行管理操作
4. 功能權限檢查位於CheckAuthorization.ascx控制項中實現

## 6. 程式碼說明

### 6.1 重要方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入處理 | object sender, EventArgs e | void |
| InitMenu | 初始化選單 | 無 | void |
| LoadUserInfo | 載入使用者資訊 | 無 | void |
| CheckPermission | 檢查使用者權限 | string funcId | bool |
| ShowMessage | 顯示系統訊息 | string message, MessageType type | void |
| LoadUserSettings | 載入使用者設定 | 無 | void |
| LogUserActivity | 記錄使用者活動 | string activity | void |
| SetLanguage | 設定介面語言 | string langCode | void |
| LogOut | 使用者登出處理 | 無 | void |

### 6.2 程式碼區塊

#### 6.2.1 頁面載入處理

```csharp
protected void Page_Load(object sender, EventArgs e)
{
    try
    {
        // 檢查Session是否存在
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            // 初始化頁面
            LoadUserInfo();
            InitMenu();
            LoadUserSettings();
            CheckSystemMessages();
        }
        
        // 記錄使用者活動
        LogUserActivity("Access Page: " + Request.Url.AbsolutePath);
    }
    catch (Exception ex)
    {
        // 記錄錯誤日誌
        Logger.Error("GLA.master Page_Load Error", ex);
        ShowMessage("系統錯誤，請聯絡管理員", MessageType.Error);
    }
}
```

#### 6.2.2 選單初始化

```csharp
private void InitMenu()
{
    try
    {
        using (IBosDB db = GetBOSDB())
        {
            // 從資料庫載入選單項目
            string sql = @"
                SELECT m.MENU_ID, m.PARENT_ID, m.MENU_NAME, m.MENU_URL, m.MENU_ICON,
                       m.DISPLAY_ORDER, m.FUNC_ID
                FROM SYS_MENU m
                INNER JOIN USER_AUTH a ON m.FUNC_ID = a.FUNC_ID
                WHERE a.USER_ID = @USER_ID
                  AND m.IS_ACTIVE = 1
                ORDER BY m.PARENT_ID, m.DISPLAY_ORDER";
                
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@USER_ID", Session["UserID"].ToString());
            
            DataTable dtMenu = db.FillDataTable(sql, parameters);
            
            // 組織選單結構
            BuildMenuStructure(dtMenu);
            
            // 繫結選單資料
            rptMainMenu.DataSource = GetTopLevelMenuItems(dtMenu);
            rptMainMenu.DataBind();
            
            rptSideMenu.DataSource = dtMenu;
            rptSideMenu.DataBind();
        }
    }
    catch (Exception ex)
    {
        Logger.Error("InitMenu Error", ex);
        throw;
    }
}
```

#### 6.2.3 使用者資訊載入

```csharp
private void LoadUserInfo()
{
    try
    {
        string userId = Session["UserID"].ToString();
        string userName = Session["UserName"].ToString();
        string deptName = Session["DeptName"].ToString();
        
        // 設定使用者資訊顯示
        lblUserInfo.Text = string.Format("使用者: {0} ({1}) 部門: {2}", 
                                         userName, userId, deptName);
        
        // 載入使用者頭像
        imgUserAvatar.ImageUrl = GetUserAvatar(userId);
        
        // 取得使用者未讀訊息數量
        int unreadCount = GetUserUnreadMsgCount(userId);
        lblMsgCount.Text = unreadCount.ToString();
        pnlMsgBadge.Visible = (unreadCount > 0);
    }
    catch (Exception ex)
    {
        Logger.Error("LoadUserInfo Error", ex);
        throw;
    }
}
```

#### 6.2.4 權限檢查

```csharp
private bool CheckPermission(string funcId)
{
    try
    {
        // 使用AppAuthority類別檢查權限
        AppAuthority auth = new AppAuthority();
        return auth.CheckFunctionAuth(Session["UserID"].ToString(), funcId);
    }
    catch (Exception ex)
    {
        Logger.Error("CheckPermission Error", ex);
        return false;
    }
}
```

### 6.3 關鍵JavaScript功能

```javascript
// 選單動態效果
$(document).ready(function() {
    // 主選單懸停效果
    $('.main-menu > li').hover(
        function() { $(this).find('.submenu').show(); },
        function() { $(this).find('.submenu').hide(); }
    );
    
    // 側邊選單收合展開
    $('.side-menu-group > .group-header').click(function() {
        $(this).next('.group-items').slideToggle();
        $(this).find('.toggle-icon').toggleClass('expanded');
    });
    
    // 設定當前選單項目高亮
    highlightCurrentMenuItem();
    
    // 系統訊息自動關閉
    setTimeout(function() {
        $('.system-message').fadeOut('slow');
    }, 5000);
    
    // 響應式選單控制
    $('.menu-toggle-btn').click(function() {
        $('.side-menu-container').toggleClass('collapsed');
        $('.content-container').toggleClass('expanded');
    });
});

// 高亮當前選單項目
function highlightCurrentMenuItem() {
    var currentUrl = window.location.pathname;
    $('.side-menu a').each(function() {
        var menuUrl = $(this).attr('href');
        if (menuUrl && currentUrl.indexOf(menuUrl) >= 0) {
            $(this).addClass('active');
            $(this).parents('.group-items').show();
            $(this).parents('.side-menu-group').find('.group-header .toggle-icon').addClass('expanded');
        }
    });
}
```

## 7. 測試案例

### 7.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 選單權限測試 | 測試InitMenu方法 | 根據使用者權限正確顯示選單 | 使用不同權限的測試用戶 |
| UT-002 | Session檢查測試 | 測試Page_Load方法 | Session無效時重定向至登入頁面 | 模擬Session過期情況 |
| UT-003 | 使用者資訊顯示測試 | 測試LoadUserInfo方法 | 正確顯示使用者及部門資訊 | 使用有效使用者登入 |
| UT-004 | 介面語言設定測試 | 測試SetLanguage方法 | 切換後正確顯示對應語言 | 測試中英語言切換 |
| UT-005 | 使用者活動記錄測試 | 測試LogUserActivity方法 | 正確記錄使用者活動到日誌 | 模擬各種使用者操作 |

### 7.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 主版面與內容頁整合測試 | 載入不同內容頁面 | 內容頁正確嵌入主版面中 | 測試多種內容頁面組合 |
| IT-002 | 使用者控制項整合測試 | 與CheckAuthorization等控制項整合 | 控制項正常工作並與主版面互動 | 測試各控制項功能 |
| IT-003 | 導航功能測試 | 透過選單進行頁面導航 | 正確導航到目標頁面並保持主版面狀態 | 測試不同導航路徑 |
| IT-004 | 系統訊息整合測試 | 從不同頁面觸發系統訊息 | 訊息正確在主版面中顯示 | 測試不同類型訊息顯示 |
| IT-005 | 選單狀態保持測試 | 頁面間導航時的選單狀態 | 選單高亮狀態正確對應當前頁面 | 測試多頁面導航序列 |

### 7.3 UI/UX測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UX-001 | 響應式布局測試 | 調整瀏覽器視窗大小 | 界面正確調整以適應不同解析度 | 測試多種螢幕尺寸 |
| UX-002 | 視覺一致性測試 | 在不同頁面檢查視覺元素 | 所有頁面保持視覺風格一致 | 檢查字體、顏色、間距等 |
| UX-003 | 操作便利性測試 | 進行常見操作序列 | 操作流程符合使用者預期 | 測試主要使用案例 |
| UX-004 | 瀏覽器相容性測試 | 在主要瀏覽器測試 | 在所有目標瀏覽器中正常顯示 | 測試IE11, Edge, Chrome, Firefox |
| UX-005 | 錯誤訊息易讀性測試 | 觸發各類錯誤訊息 | 訊息內容清晰易懂且提供解決方向 | 測試常見錯誤情境 |

## 8. 安全性考量

### 8.1 登入與授權安全

1. **使用者認證**
   - 採用Forms Authentication進行使用者認證
   - 密碼經過加密存儲，不在Session中保存明文密碼
   - 設定適當的Session超時時間 (20分鐘)
   - 支援密碼複雜度政策

2. **授權控制**
   - 實作多層次權限控制機制
   - 所有功能訪問都需要進行權限檢查
   - 使用CheckAuthorization控制項統一管理權限檢查
   - 頁面和資料層級都實施權限控制

3. **Session安全**
   - Session ID加密傳輸
   - 定期重新產生Session ID防止Session劫持
   - 使用者登出時清除Session資料
   - 避免在URL中傳遞敏感資訊

### 8.2 資料安全

1. **資料傳輸安全**
   - 使用HTTPS加密傳輸所有資料
   - 敏感資料在傳輸層加密
   - 避免將敏感資料傳輸到客戶端

2. **防止常見攻擊**
   - 防止XSS攻擊：輸出資料進行HTML編碼
   - 防止SQL注入：使用參數化查詢
   - 防止CSRF攻擊：實作防偽令牌
   - 防止點擊劫持：設定適當的X-Frame-Options

3. **日誌與審計**
   - 記錄使用者登入/登出事件
   - 記錄關鍵操作日誌
   - 審計敏感資料存取
   - 系統錯誤記錄與監控

### 8.3 客戶端安全

1. **客戶端腳本安全**
   - 實施內容安全策略 (CSP)
   - 避免使用eval()和其他不安全的JavaScript函數
   - 第三方JavaScript庫進行安全評估
   - 適當的錯誤處理避免洩露系統資訊

2. **瀏覽器安全配置**
   - 設定適當的HTTP安全標頭
   - 設定Secure和HttpOnly cookie標誌
   - 強制執行HTTPS通訊
   - 實施子資源完整性檢查

## 9. 效能優化

### 9.1 頁面載入優化

1. **資源最小化**
   - CSS和JavaScript文件壓縮
   - 圖片最佳化
   - 合併多個CSS和JavaScript文件
   - 設定適當的快取策略

2. **延遲載入**
   - 非關鍵JavaScript延遲載入
   - 圖片延遲載入
   - 選單資料按需載入

3. **頁面渲染優化**
   - 減少DOM操作
   - 避免複雜的CSS選擇器
   - 優化頁面結構以加速渲染
   - 使用CSS動畫代替JavaScript動畫

### 9.2 伺服器效能

1. **資料存取優化**
   - 使用資料緩存
   - 最小化資料庫查詢
   - 實施高效的選單載入策略
   - 避免重複資料載入

2. **程式碼優化**
   - 最小化Page_Load處理時間
   - 避免不必要的頁面刷新
   - 使用非同步處理長時間運行的任務
   - 減少ViewState大小

3. **緩存策略**
   - 實施輸出緩存
   - 部分常用頁面元素進行片段緩存
   - 選單結構緩存
   - 使用者設定緩存

### 9.3 監控與調優

1. **效能監控**
   - 記錄頁面載入時間
   - 監控資料庫查詢效能
   - 追踪記憶體使用情況
   - 設定警示閾值

2. **系統調優參數**
   - 視圖狀態壓縮：啟用
   - 頁面載入超時：60秒
   - 同時使用者數上限：200
   - 記憶體使用閾值：80%

## 10. 維護與擴展

### 10.1 一般維護指南

1. **定期維護任務**
   - 檢查日誌文件大小並進行歸檔
   - 監控資料庫連接使用情況
   - 檢查和清理暫存文件
   - 更新第三方組件和安全補丁

2. **問題排除流程**
   - 診斷常見錯誤的步驟
   - 日誌分析方法
   - 效能問題排查技巧
   - 使用者報告問題的處理流程

3. **備份與恢復**
   - 主版面配置文件備份策略
   - 樣式和腳本文件備份
   - 測試環境中的變更測試
   - 版本回滾機制

### 10.2 擴展指南

1. **新增功能模組**
   - 在SYS_MENU資料表中添加新選單項目
   - 遵循既有的導航結構
   - 設定適當的權限控制
   - 使用現有樣式和控制項

2. **修改視覺樣式**
   - 自定義CSS檔案位置
   - 品牌顏色定義和使用規範
   - 主題切換實施指導
   - 響應式設計修改指南

3. **新增使用者控制項**
   - 控制項整合最佳實踐
   - 在主版面中加載控制項的方法
   - 控制項與主版面交互機制
   - 權限控制整合方式

### 10.3 版本升級規劃

1. **未來版本計劃**
   - 從WebForms遷移至MVC或Blazor的路徑
   - 整合現代UI框架
   - 移動裝置支援增強
   - API擴展

2. **相容性考量**
   - 升級時資料庫相容性
   - 客戶端瀏覽器支援策略
   - 舊內容頁面相容性維護
   - 第三方元件升級策略

## 11. 參考資料與附件

### 11.1 相關文件

1. **設計文件**
   - 系統整體架構文件
   - UI/UX設計規範
   - 資料庫設計文件

2. **操作手冊**
   - 系統管理員手冊
   - 開發者手冊
   - 版面配置修改指南

3. **相依元件文件**
   - CheckAuthorization.ascx使用文件
   - LoginClass類別參考

### 11.2 附件

1. 主版面結構圖
2. 選單關係ER圖
3. 權限控制流程圖
4. 樣式色彩配置表

### 11.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 