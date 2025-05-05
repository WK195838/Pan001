# 程式規格書

## 1. 基本資訊

| 項目 | 說明 |
|-----|------|
| 程式代號 | Navigator |
| 程式名稱 | 導航控制項 |
| 版本號碼 | 1.0.0 |
| 所屬模組 | 系統核心架構 |
| 檔案位置 | /GLATEST/app/Controls/Navigator.ascx, /GLATEST/app/Controls/Navigator.ascx.cs |
| 程式類型 | 使用者控制項 (User Control) |
| 建立日期 | 2025/05/05 |
| 建立人員 | PanPacific開發團隊 |
| 最後修改日期 | 2025/05/05 |
| 最後修改人員 | Claude AI |

## 2. 功能概述

### 2.1 主要功能

Navigator 是泛太總帳系統的核心導航控制項，提供使用者在不同模組和功能間進行快速導航的功能。該控制項維護了系統所有可用功能的導航結構，並根據使用者權限動態顯示相應選單，確保使用者界面的一致性和易用性。

### 2.2 業務流程

Navigator 在泛太總帳系統中扮演以下角色：
1. 構建系統功能的多層次選單結構
2. 根據使用者權限動態過濾顯示的功能項目
3. 處理使用者的功能選擇，並導航至相應頁面
4. 維護導航路徑和歷史記錄（麵包屑導航）
5. 提供功能收藏與快速訪問入口
6. 整合系統通知與提示

### 2.3 使用頻率

- 極高頻率：系統中的每個頁面都包含此控制項
- 用戶每進入一個新頁面就會載入此控制項
- 平均每位使用者每天使用數十次到上百次

### 2.4 使用者角色

此控制項服務於系統的所有用戶，包括：
- 系統管理員：可以看到所有功能選單
- 財務主管：根據權限看到財務相關功能選單
- 會計人員：看到會計核算相關功能選單
- 一般用戶：僅看到被授權的功能選單

## 3. 系統架構

### 3.1 技術架構

- 開發環境：ASP.NET Web Forms (.NET Framework 4.0)
- 主要技術：
  - HTML/CSS 構建界面
  - JavaScript/jQuery 實現動態效果
  - C# 後端邏輯處理
- 頁面載入機制：動態載入與服務端渲染
- 資料綁定方式：使用後端資料庫查詢與緩存機制

### 3.2 資料表使用

| 資料表名稱 | 用途 | 存取方式 |
|-----------|------|---------|
| SYS_FUNCTION | 系統功能定義表 | 讀取 |
| SYS_FUNCTION_GROUP | 功能分組表 | 讀取 |
| SYS_USER_FAVORITE | 使用者收藏功能表 | 讀取/寫入 |
| SYS_MENU_CONFIG | 選單配置表 | 讀取 |
| SYS_USER_CONFIG | 使用者配置表 | 讀取 |

### 3.3 相依元件

| 元件名稱 | 用途 | 說明 |
|---------|------|------|
| AppAuthority | 權限管理 | 獲取使用者對功能的訪問權限 |
| LoginClass | 登入模組 | 獲取當前登入使用者資訊 |
| Logger | 日誌記錄 | 記錄導航行為與錯誤 |
| IBosDB | 資料庫存取 | 提供資料庫連接與查詢功能 |

## 4. 控制項設計

### 4.1 界面結構

Navigator 控制項的界面主要由以下元素構成：

```
+--------------------------------------------------------------------+
| Logo區域 | 主導航欄 | 用戶資訊區 | 系統功能區                      |
+--------------------------------------------------------------------+
| 次級導航欄 / 功能分組導航                                         |
+--------------------------------------------------------------------+
| 麵包屑導航路徑                                                    |
+--------------------------------------------------------------------+
| 快速功能/收藏區                                                   |
+--------------------------------------------------------------------+
```

### 4.2 屬性設計

| 屬性名稱 | 類型 | 存取修飾詞 | 說明 |
|---------|------|----------|------|
| CurrentModuleId | string | public | 當前所在模組ID |
| CurrentFunctionId | string | public | 當前所在功能ID |
| ShowBreadcrumb | bool | public | 是否顯示麵包屑導航 |
| ShowQuickLinks | bool | public | 是否顯示快速連結 |
| MenuDepth | int | public | 顯示的選單層級深度 |
| IsCollapsed | bool | public | 導航選單是否為折疊狀態 |

### 4.3 參數定義

Navigator 控制項支援以下WebForm參數：

```
<%@ Register Src="~/Controls/Navigator.ascx" TagPrefix="pan" TagName="Navigator" %>
<pan:Navigator ID="pageNavigator" runat="server" 
               CurrentModuleId="GL" 
               CurrentFunctionId="PTA0150" 
               ShowBreadcrumb="true" 
               ShowQuickLinks="true" 
               MenuDepth="3" 
               IsCollapsed="false" />
```

### 4.4 CSS設計

控制項使用以下核心CSS類別：

```css
.nav-container { /* 容器 */ }
.nav-main-bar { /* 主導航欄 */ }
.nav-sub-bar { /* 次級導航欄 */ }
.nav-breadcrumb { /* 麵包屑 */ }
.nav-quick-links { /* 快速連結區 */ }
.nav-dropdown { /* 下拉選單 */ }
.nav-item { /* 導航項目 */ }
.nav-item-active { /* 當前活動項目 */ }
.nav-group { /* 功能組 */ }
.nav-expand-btn { /* 展開/折疊按鈕 */ }
```

## 5. 主要方法

### 5.1 初始化和載入方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| Page_Load | 頁面載入時初始化控制項 | object sender, EventArgs e | void |
| InitializeMenu | 初始化選單結構 | 無 | void |
| LoadUserSettings | 載入使用者設定 | 無 | void |
| LoadFunctionGroups | 載入功能分組 | 無 | DataTable |
| LoadFunctionItems | 載入特定分組的功能項目 | string groupId | DataTable |
| BuildBreadcrumb | 建構麵包屑導航 | 無 | string |

### 5.2 事件處理方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| NavigationItemClick | 處理導航項目點擊事件 | object sender, EventArgs e | void |
| ExpandCollapseClick | 處理展開/折疊按鈕點擊 | object sender, EventArgs e | void |
| AddToFavorites | 添加當前頁面到收藏 | object sender, EventArgs e | void |
| RemoveFromFavorites | 從收藏中移除項目 | object sender, EventArgs e | void |
| SearchFunctionHandler | 處理功能搜尋事件 | object sender, EventArgs e | void |
| UserSettingsChanged | 用戶設定變更處理 | object sender, EventArgs e | void |

### 5.3 工具方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| GetModuleName | 獲取模組名稱 | string moduleId | string |
| GetFunctionName | 獲取功能名稱 | string functionId | string |
| GetFunctionUrl | 獲取功能URL | string functionId | string |
| CheckFunctionAuth | 檢查功能權限 | string functionId | bool |
| GetUserFavorites | 獲取用戶收藏 | 無 | DataTable |
| GetRecentFunctions | 獲取最近使用的功能 | int count | List\<FunctionInfo\> |
| SaveUserSettings | 保存用戶設定 | UserSettings settings | bool |

### 5.4 渲染方法

| 方法名稱 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|-------|
| RenderMainMenu | 渲染主選單 | 無 | void |
| RenderSubMenu | 渲染次級選單 | string parentId | void |
| RenderBreadcrumb | 渲染麵包屑導航 | 無 | void |
| RenderQuickLinks | 渲染快速連結 | 無 | void |
| HighlightCurrentItem | 突顯當前項目 | 無 | void |
| ApplyUserTheme | 應用用戶界面主題 | 無 | void |

## 6. 處理邏輯

### 6.1 初始化流程

```
開始
 ↓
檢查頁面是否為回傳 → 是 → 恢復控制項狀態 → 跳至「驗證使用者」
 ↓
初始化控制項屬性和狀態
 ↓
驗證使用者身份 → 未通過 → 重定向到登入頁面 → 結束
 ↓
載入用戶設定和偏好
 ↓
載入系統功能定義
 ↓
載入功能分組
 ↓
根據使用者權限過濾可訪問功能
 ↓
構建導航選單結構
 ↓
確定當前位置並構建麵包屑
 ↓
渲染導航界面
 ↓
加載用戶收藏和最近訪問
 ↓
應用用戶界面主題
 ↓
註冊客戶端事件處理器
 ↓
結束
```

### 6.2 導航選擇流程

```
開始
 ↓
使用者點擊導航項目
 ↓
獲取選擇的功能ID
 ↓
檢查功能權限 → 無權限 → 顯示無權限訊息 → 結束
 ↓
記錄當前頁面到歷史
 ↓
更新最近訪問記錄
 ↓
構建目標頁面URL
 ↓
導向目標頁面
 ↓
結束
```

### 6.3 例外處理

| 錯誤類型 | 處理方式 | 使用者體驗 |
|---------|---------|-----------|
| 功能定義不存在 | 記錄錯誤，顯示可用功能 | 隱藏不存在的功能項，系統正常運行 |
| 權限驗證失敗 | 記錄事件，顯示權限不足 | 顯示友好的無權限提示，僅顯示有權限的功能 |
| 數據庫連接錯誤 | 嘗試使用缓存，記錄錯誤 | 如有缓存顯示上次結構，否則顯示最小化導航 |
| 用戶設置載入失敗 | 使用默認設置，記錄警告 | 使用系統預設值，用戶體驗保持一致 |
| 頁面不存在錯誤 | 導向錯誤頁，記錄錯誤 | 顯示友好的404頁面，提供返回按鈕 |

## 7. 代碼說明

### 7.1 控制項註冊與頁面使用

```aspx
<%-- 在主版面(GLA.master)中註冊與使用Navigator控制項 --%>
<%@ Register Src="~/Controls/Navigator.ascx" TagPrefix="pan" TagName="Navigator" %>

<div class="page-header">
    <pan:Navigator ID="mainNavigator" runat="server" 
                  CurrentModuleId='<%# GetCurrentModuleId() %>' 
                  CurrentFunctionId='<%# GetCurrentFunctionId() %>' 
                  ShowBreadcrumb="true" />
</div>
```

### 7.2 選單初始化實現

```csharp
/// <summary>
/// 初始化選單結構
/// </summary>
private void InitializeMenu()
{
    try
    {
        // 獲取當前使用者ID
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 載入功能分組
        DataTable groups = LoadFunctionGroups();
        
        // 創建選單結構
        menuItems = new Dictionary<string, List<MenuItem>>();
        
        foreach (DataRow group in groups.Rows)
        {
            string groupId = group["GROUP_ID"].ToString();
            string groupName = group["GROUP_NAME"].ToString();
            
            // 載入此分組下的功能項目
            DataTable functions = LoadFunctionItems(groupId);
            
            // 過濾用戶有權限的功能
            List<MenuItem> groupItems = new List<MenuItem>();
            
            foreach (DataRow func in functions.Rows)
            {
                string functionId = func["FUNCTION_ID"].ToString();
                
                // 檢查用戶是否有權限訪問該功能
                if (CheckFunctionAuth(functionId))
                {
                    MenuItem item = new MenuItem
                    {
                        ID = functionId,
                        Text = func["FUNCTION_NAME"].ToString(),
                        Url = GetFunctionUrl(functionId),
                        ParentID = groupId,
                        Level = Convert.ToInt32(func["MENU_LEVEL"]),
                        SortOrder = Convert.ToInt32(func["SORT_ORDER"]),
                        Icon = func["ICON"].ToString()
                    };
                    
                    groupItems.Add(item);
                }
            }
            
            // 排序功能項目
            groupItems = groupItems.OrderBy(i => i.SortOrder).ToList();
            
            // 如果此分組有可訪問功能，添加到菜單
            if (groupItems.Count > 0)
            {
                menuItems.Add(groupId, groupItems);
                
                // 創建分組菜單項
                MenuItem groupMenuItem = new MenuItem
                {
                    ID = groupId,
                    Text = groupName,
                    ParentID = string.Empty,
                    Level = 0,
                    SortOrder = Convert.ToInt32(group["SORT_ORDER"]),
                    Icon = group["ICON"].ToString()
                };
                
                // 添加到主菜單
                if (!mainMenuItems.ContainsKey(groupId))
                {
                    mainMenuItems.Add(groupId, groupMenuItem);
                }
            }
        }
        
        // 設置緩存
        if (HttpContext.Current != null && HttpRuntime.Cache != null)
        {
            string cacheKey = $"NAV_MENU_{userId}";
            HttpRuntime.Cache.Insert(cacheKey, menuItems, null, 
                                    DateTime.Now.AddMinutes(30), 
                                    System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }
    catch (Exception ex)
    {
        Logger.Error("Navigator.InitializeMenu Error", ex);
        
        // 出錯時載入最小菜單
        LoadMinimalMenu();
    }
}
```

### 7.3 麵包屑導航實現

```csharp
/// <summary>
/// 建構麵包屑導航
/// </summary>
/// <returns>麵包屑HTML</returns>
private string BuildBreadcrumb()
{
    try
    {
        if (!ShowBreadcrumb)
        {
            return string.Empty;
        }
        
        StringBuilder breadcrumb = new StringBuilder();
        breadcrumb.Append("<div class=\"nav-breadcrumb\">");
        
        // 添加首頁
        breadcrumb.Append("<a href=\"" + ResolveUrl("~/Default.aspx") + "\" class=\"breadcrumb-item\">");
        breadcrumb.Append("<i class=\"fa fa-home\"></i> 首頁</a>");
        
        // 添加模組
        if (!string.IsNullOrEmpty(CurrentModuleId))
        {
            string moduleName = GetModuleName(CurrentModuleId);
            breadcrumb.Append("<span class=\"breadcrumb-separator\">&gt;</span>");
            breadcrumb.Append("<span class=\"breadcrumb-item\">" + moduleName + "</span>");
            
            // 添加功能
            if (!string.IsNullOrEmpty(CurrentFunctionId))
            {
                string functionName = GetFunctionName(CurrentFunctionId);
                breadcrumb.Append("<span class=\"breadcrumb-separator\">&gt;</span>");
                breadcrumb.Append("<span class=\"breadcrumb-item breadcrumb-current\">" + functionName + "</span>");
            }
        }
        
        breadcrumb.Append("</div>");
        return breadcrumb.ToString();
    }
    catch (Exception ex)
    {
        Logger.Error("Navigator.BuildBreadcrumb Error", ex);
        return string.Empty;
    }
}
```

### 7.4 權限檢查實現

```csharp
/// <summary>
/// 檢查使用者是否有權限訪問指定功能
/// </summary>
/// <param name="functionId">功能ID</param>
/// <returns>是否有權限</returns>
private bool CheckFunctionAuth(string functionId)
{
    try
    {
        // 檢查參數
        if (string.IsNullOrEmpty(functionId))
        {
            return false;
        }
        
        // 獲取當前使用者
        string userId = LoginClass.Instance.CurrentUser.UserId;
        
        // 使用AppAuthority檢查權限
        // 對於導航，通常只需要查看權限
        bool hasAuth = AppAuthority.Instance.CheckFunctionAuth(userId, functionId, (int)AppAuthority.AuthLevel.View);
        
        return hasAuth;
    }
    catch (Exception ex)
    {
        Logger.Error("Navigator.CheckFunctionAuth Error", ex);
        
        // 出錯時保守處理：拒絕訪問
        return false;
    }
}
```

### 7.5 渲染導航選單

```csharp
/// <summary>
/// 渲染主選單
/// </summary>
private void RenderMainMenu()
{
    try
    {
        StringBuilder menuHtml = new StringBuilder();
        menuHtml.Append("<ul class=\"nav-main-bar\">");
        
        // 排序主選單項目
        var sortedItems = mainMenuItems.Values.OrderBy(i => i.SortOrder);
        
        foreach (MenuItem item in sortedItems)
        {
            string activeClass = (item.ID == CurrentModuleId) ? " nav-item-active" : string.Empty;
            
            menuHtml.Append("<li class=\"nav-item" + activeClass + "\" data-id=\"" + item.ID + "\">");
            
            if (menuItems.ContainsKey(item.ID) && menuItems[item.ID].Count > 0)
            {
                // 有下級選單
                menuHtml.Append("<a href=\"javascript:void(0)\" class=\"nav-item-link\" onclick=\"expandSubMenu('" + item.ID + "')\">");
                
                if (!string.IsNullOrEmpty(item.Icon))
                {
                    menuHtml.Append("<i class=\"" + item.Icon + "\"></i> ");
                }
                
                menuHtml.Append(item.Text);
                menuHtml.Append("<i class=\"fa fa-chevron-down nav-dropdown-icon\"></i>");
                menuHtml.Append("</a>");
                
                // 渲染子選單
                menuHtml.Append("<div id=\"submenu-" + item.ID + "\" class=\"nav-dropdown\">");
                menuHtml.Append("<ul class=\"nav-dropdown-menu\">");
                
                foreach (MenuItem subItem in menuItems[item.ID].OrderBy(i => i.SortOrder))
                {
                    string subActiveClass = (subItem.ID == CurrentFunctionId) ? " nav-item-active" : string.Empty;
                    
                    menuHtml.Append("<li class=\"nav-dropdown-item" + subActiveClass + "\">");
                    menuHtml.Append("<a href=\"" + ResolveUrl(subItem.Url) + "\" class=\"nav-dropdown-link\">");
                    
                    if (!string.IsNullOrEmpty(subItem.Icon))
                    {
                        menuHtml.Append("<i class=\"" + subItem.Icon + "\"></i> ");
                    }
                    
                    menuHtml.Append(subItem.Text);
                    menuHtml.Append("</a></li>");
                }
                
                menuHtml.Append("</ul></div>");
            }
            else
            {
                // 無下級選單
                menuHtml.Append("<a href=\"" + ResolveUrl(item.Url) + "\" class=\"nav-item-link\">");
                
                if (!string.IsNullOrEmpty(item.Icon))
                {
                    menuHtml.Append("<i class=\"" + item.Icon + "\"></i> ");
                }
                
                menuHtml.Append(item.Text);
                menuHtml.Append("</a>");
            }
            
            menuHtml.Append("</li>");
        }
        
        menuHtml.Append("</ul>");
        
        // 輸出到頁面
        litMainMenu.Text = menuHtml.ToString();
    }
    catch (Exception ex)
    {
        Logger.Error("Navigator.RenderMainMenu Error", ex);
        
        // 渲染最小菜單
        RenderMinimalMenu();
    }
}
```

## 8. 測試案例

### 8.1 單元測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UT-001 | 選單載入 | 測試InitializeMenu方法 | 正確載入選單結構 | 使用模擬資料與權限 |
| UT-002 | 權限過濾 | 測試CheckFunctionAuth方法 | 正確過濾無權限功能 | 測試不同權限等級 |
| UT-003 | 麵包屑生成 | 測試BuildBreadcrumb方法 | 正確生成麵包屑 | 測試不同層級功能 |
| UT-004 | 選單渲染 | 測試RenderMainMenu方法 | 選單HTML符合預期 | 測試各種選單結構 |
| UT-005 | URL生成 | 測試GetFunctionUrl方法 | 生成正確的URL | 測試各類型功能 |

### 8.2 整合測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| IT-001 | 與權限系統整合 | 測試與AppAuthority協作 | 權限正確影響導航 | 不同權限用戶測試 |
| IT-002 | 與登入系統整合 | 測試與LoginClass協作 | 用戶資訊正確顯示 | 模擬不同登入狀態 |
| IT-003 | 頁面導航測試 | 測試點擊導航功能 | 正確導航到目標頁面 | 測試多次導航操作 |
| IT-004 | 收藏功能測試 | 測試功能收藏機制 | 正確添加與移除收藏 | 測試收藏CRUD操作 |
| IT-005 | 設置保存測試 | 測試用戶設置保存 | 設置正確保存並恢復 | 測試瀏覽器會話 |

### 8.3 UI測試

| 測試編號 | 測試項目 | 測試方法 | 預期結果 | 測試條件 |
|---------|---------|---------|---------|---------|
| UI-001 | 響應式佈局 | 測試不同螢幕尺寸 | 導航正確調整顯示 | 桌機、平板、手機尺寸 |
| UI-002 | 動畫效果 | 測試展開/折疊動畫 | 平滑動畫效果 | 不同瀏覽器環境 |
| UI-003 | 高亮當前項 | 測試當前項高亮 | 正確突顯當前功能 | 導航不同路徑 |
| UI-004 | 主題適配 | 測試不同系統主題 | 導航與系統主題協調 | 測試暗/亮模式適配 |
| UI-005 | 鍵盤可訪問性 | 測試鍵盤導航 | 支援鍵盤導航操作 | 鍵盤TAB導航測試 |

## 9. 安全性考量

### 9.1 授權控制

1. **權限驗證**
   - 所有導航項目必須通過權限檢查
   - 使用AppAuthority統一權限管理
   - 保守策略：權限不明確時不顯示

2. **動態過濾**
   - 基於使用者角色動態過濾菜單
   - 不向客戶端發送無權限的功能項信息
   - 服務端二次驗證目標頁面權限

3. **深度防護**
   - 服務端與客戶端同時執行權限檢查
   - 記錄異常導航行為並警告
   - 防止URL直接訪問繞過導航

### 9.2 資料安全

1. **選單數據**
   - 避免在客戶端儲存完整選單結構
   - 只緩存經過權限過濾的選單
   - 定期刷新緩存確保權限更新生效

2. **傳輸安全**
   - 使用相對路徑避免洩露系統路徑
   - 防止URL參數注入
   - 避免在URL參數中傳遞敏感信息

3. **防止攻擊**
   - 防止XSS：輸出HTML時進行編碼
   - 防止CSRF：使用表單令牌
   - 輸入驗證：過濾所有輸入參數

## 10. 效能優化

### 10.1 載入優化

1. **延遲載入**
   - 初始只載入主選單結構
   - 子選單按需載入
   - 使用Ajax非同步載入深層級選單

2. **緩存策略**
   - 緩存經過權限過濾的選單結構
   - 用戶特定設置本地儲存
   - 避免重複權限檢查

3. **資源最小化**
   - 合併壓縮CSS/JS文件
   - 圖標使用字體庫減少HTTP請求
   - HTML結構最簡化

### 10.2 渲染優化

1. **代碼優化**
   - 避免過度使用ViewState
   - 使用StringBuilder拼接HTML
   - 減少DOM操作次數

2. **顯示優化**
   - 避免頁面重排
   - 延遲加載非關鍵元素
   - CSS動畫代替JavaScript動畫

3. **數據優化**
   - 選擇性傳輸數據
   - 分批載入大型選單
   - 使用資料壓縮減少傳輸量

### 10.3 使用者體驗優化

1. **響應速度**
   - 點擊響應時間<100ms
   - 選單展開時間<200ms
   - 頁面導航時間<2s

2. **視覺反饋**
   - 點擊時立即視覺反饋
   - 載入狀態指示
   - 平滑過渡動畫

3. **快捷方式**
   - 收藏功能的支持
   - 最近訪問記錄
   - 鍵盤快捷鍵支持

## 11. 維護與擴展

### 11.1 配置管理

1. **選單配置**
   - 通過管理界面配置選單
   - 支持功能項啟用/禁用
   - 支持選單項排序調整

2. **個性化設置**
   - 使用者可自定義收藏
   - 支持拖拽調整選單順序
   - 記住展開/折疊狀態

3. **外觀配置**
   - 支持主題切換
   - 可調整字體大小
   - 亮/暗模式支持

### 11.2 擴展考量

1. **新功能擴展**
   - 預留插件支持接口
   - 模塊化設計便於添加新模塊
   - 預留自定義選單插槽

2. **多語言支持**
   - 文字資源外部化
   - 支持切換語言設置
   - 右到左語言支持(如阿拉伯語)

3. **設備適配**
   - 行動裝置導航模式(漢堡選單)
   - 觸控友好的控制項
   - 針對不同解析度優化

### 11.3 版本升級

1. **向後兼容**
   - 保持API一致性
   - 使用標記屬性支持舊版控制項
   - 平滑升級路徑

2. **功能更新**
   - 支持實時通知集成
   - 添加搜索功能
   - 集成使用者頭像與信息

## 12. 參考資料

### 12.1 設計文件

1. **UI設計規範**
   - 泛太總帳系統UI設計指南
   - 控制項設計標準
   - 色彩與字體規範

2. **系統架構**
   - 系統整體架構文檔
   - 控制項框架說明
   - ASP.NET控制項最佳實踐

3. **選單結構**
   - 系統功能結構圖
   - 使用者角色與權限設計
   - 導航流程圖

### 12.2 參考實現

1. **框架參考**
   - ASP.NET Web Forms控制項開發指南
   - jQuery UI導航選單實現
   - 響應式導航實現範例

2. **UI/UX參考**
   - Microsoft Fluent Design指南
   - Material Design導航元素
   - Bootstrap導航組件

### 12.3 文件變更歷史

| 版本 | 日期 | 變更說明 | 變更人員 |
|-----|------|---------|---------|
| 1.0.0 | 2025/05/05 | 初版文件建立 | Claude AI | 