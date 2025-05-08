# ModPopFunction.js 彈出窗口函數規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | ModPopFunction.js                    |
| 程式名稱     | 彈出窗口函數                           |
| 檔案大小     | 2.6KB                                |
| 行數        | 82                                   |
| 功能簡述     | 提供彈出窗口控制                        |
| 複雜度       | 中                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/26                           |

## 程式功能概述

ModPopFunction.js 是泛太總帳系統中專門負責彈出視窗和模態對話框控制的客戶端腳本。此腳本提供了一套統一且靈活的彈出窗口管理機制，用於系統中需要顯示額外內容或進行使用者互動的場景。

主要功能包括：

1. **模態窗口管理**：創建、顯示和關閉模態對話框
2. **非模態窗口控制**：管理非模態的彈出視窗
3. **視窗位置計算**：自動計算並調整彈出窗口的最佳位置
4. **尺寸控制**：設定和調整彈出窗口的大小
5. **參數傳遞**：在開啟視窗時傳遞參數，並在關閉時返回結果
6. **多層次窗口**：支援多層次的視窗堆疊和管理
7. **拖曳支援**：提供視窗拖曳移動功能
8. **焦點管理**：控制視窗間的焦點轉換
9. **事件處理**：提供窗口開啟、關閉、調整大小等事件的處理機制
10. **動畫效果**：提供視窗顯示和隱藏的動畫效果

ModPopFunction.js 的設計目標是提供一個統一的彈出視窗處理機制，確保系統中所有彈出視窗具有一致的行為和外觀，同時簡化開發人員實現彈出視窗功能的工作。此腳本與系統中的 PopupDialog.aspx 和 ModelPopupDialog.aspx 頁面緊密結合，為泛太總帳系統提供全方位的對話框和彈出視窗支援。

## 技術實現

ModPopFunction.js 採用以下技術實現其功能：

1. **jQuery 整合**：
   - 使用 jQuery 簡化 DOM 操作和事件處理
   - 利用 jQuery 的選擇器機制精確定位元素
   - 與 jQuery UI 對話框元件整合

2. **原生 JavaScript**：
   - 使用純 JavaScript 實現核心功能邏輯
   - 確保最大兼容性和效能
   - 採用閉包保護內部變數和狀態

3. **命名空間管理**：
   - 使用 PTA.PopWindow 命名空間避免全局污染
   - 提供清晰的函數分類和組織
   - 與 pagefunction.js 的命名空間協調一致

4. **視窗狀態管理**：
   - 使用內部狀態物件追蹤所有開啟的視窗
   - 實現視窗的生命週期管理
   - 處理多視窗間的互動和依賴關係

5. **事件委派**：
   - 使用事件委派技術減少事件監聽器數量
   - 提高系統效能和降低記憶體佔用
   - 統一處理視窗相關的事件

6. **CSS 整合**：
   - 與系統 CSS 樣式整合提供一致的視覺體驗
   - 動態控制視窗樣式和外觀
   - 支援主題切換和客製化外觀

7. **響應式設計**：
   - 支援不同螢幕尺寸下的自適應表現
   - 在視窗尺寸變化時自動調整
   - 確保在各種裝置上的可用性

8. **參數序列化**：
   - 實現複雜物件的序列化和反序列化
   - 支援跨頁面的參數傳遞
   - 處理特殊資料類型和格式

9. **錯誤處理**：
   - 實現全面的錯誤捕獲和處理機制
   - 提供友好的錯誤提示
   - 確保視窗操作的穩定性

10. **延遲載入**：
    - 支援視窗內容的延遲載入
    - 減少初始載入時間
    - 提高使用者體驗

## 函數庫架構

ModPopFunction.js 的函數庫架構如下：

1. **PTA.PopWindow** - 主命名空間
   - 封裝所有彈出窗口相關功能
   - 管理內部狀態和配置

2. **PTA.PopWindow.Config** - 配置模組
   - 預設視窗配置
   - 客製化選項
   - 系統級設定

3. **PTA.PopWindow.Modal** - 模態窗口模組
   - 創建和管理模態視窗
   - 控制模態行為
   - 處理使用者交互

4. **PTA.PopWindow.Popup** - 非模態窗口模組
   - 創建和管理彈出視窗
   - 控制視窗位置和大小
   - 管理多視窗顯示

5. **PTA.PopWindow.State** - 狀態管理模組
   - 追蹤所有開啟的視窗
   - 管理視窗間的關係
   - 處理視窗堆疊順序

6. **PTA.PopWindow.Event** - 事件處理模組
   - 註冊和處理視窗事件
   - 提供事件回調機制
   - 執行自定義事件處理

7. **PTA.PopWindow.Util** - 工具函數模組
   - 提供輔助功能
   - 處理參數和返回值
   - 實現通用功能

## 主要函數參考

### 核心函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.PopWindow.initialize` | `function(config)` | 初始化彈出窗口系統 | config: 配置選項物件 | 無 |
| `PTA.PopWindow.getDefaults` | `function()` | 獲取預設配置 | 無 | 配置物件 |
| `PTA.PopWindow.extend` | `function(options)` | 擴展預設配置 | options: 擴充選項 | 合併後設定 |
| `PTA.PopWindow.getActiveWindows` | `function()` | 獲取所有活動視窗 | 無 | 視窗陣列 |
| `PTA.PopWindow.closeAll` | `function()` | 關閉所有開啟的視窗 | 無 | 布林值 |

### 模態窗口函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.PopWindow.Modal.open` | `function(url, options)` | 開啟模態視窗 | url: 視窗頁面路徑, options: 選項物件 | 視窗ID |
| `PTA.PopWindow.Modal.close` | `function(id, result)` | 關閉模態視窗 | id: 視窗ID, result: 返回結果 | 布林值 |
| `PTA.PopWindow.Modal.setTitle` | `function(id, title)` | 設定視窗標題 | id: 視窗ID, title: 標題文字 | 無 |
| `PTA.PopWindow.Modal.resize` | `function(id, width, height)` | 調整視窗大小 | id: 視窗ID, width: 寬度, height: 高度 | 無 |
| `PTA.PopWindow.Modal.center` | `function(id)` | 視窗置中 | id: 視窗ID | 無 |

### 彈出窗口函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.PopWindow.Popup.open` | `function(url, options)` | 開啟非模態視窗 | url: 視窗頁面路徑, options: 選項物件 | 視窗ID |
| `PTA.PopWindow.Popup.close` | `function(id)` | 關閉非模態視窗 | id: 視窗ID | 布林值 |
| `PTA.PopWindow.Popup.moveToTop` | `function(id)` | 將視窗移至頂層 | id: 視窗ID | 無 |
| `PTA.PopWindow.Popup.setPosition` | `function(id, x, y)` | 設定視窗位置 | id: 視窗ID, x: X座標, y: Y座標 | 無 |
| `PTA.PopWindow.Popup.minimize` | `function(id)` | 最小化視窗 | id: 視窗ID | 無 |

### 事件處理函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.PopWindow.Event.register` | `function(eventName, handler)` | 註冊事件處理函數 | eventName: 事件名稱, handler: 處理函數 | 無 |
| `PTA.PopWindow.Event.unregister` | `function(eventName, handler)` | 移除事件處理函數 | eventName: 事件名稱, handler: 處理函數 | 無 |
| `PTA.PopWindow.Event.trigger` | `function(eventName, args)` | 觸發特定事件 | eventName: 事件名稱, args: 事件參數 | 無 |
| `PTA.PopWindow.Event.onBeforeOpen` | `function(handler)` | 設定開啟前事件處理 | handler: 處理函數 | 無 |
| `PTA.PopWindow.Event.onAfterClose` | `function(handler)` | 設定關閉後事件處理 | handler: 處理函數 | 無 |

## 使用範例

以下是 ModPopFunction.js 的一些常見使用範例：

### 初始化窗口系統

```javascript
// 初始化彈出窗口系統並設定預設值
$(document).ready(function() {
    PTA.PopWindow.initialize({
        defaultWidth: 600,
        defaultHeight: 400,
        showAnimation: true,
        animationDuration: 300,
        modalBackgroundColor: 'rgba(0, 0, 0, 0.5)',
        draggable: true
    });
});
```

### 開啟模態對話框

```javascript
// 開啟模態對話框並處理返回結果
function openUserSelector() {
    var windowId = PTA.PopWindow.Modal.open('/ModelPopupDialog.aspx', {
        url: '/Dialogs/UserSelector.aspx',
        title: '選擇使用者',
        width: 500,
        height: 600,
        parameters: {
            departmentId: '001',
            multiSelect: true
        },
        callback: function(result) {
            if (result && result.selected) {
                handleSelectedUsers(result.users);
            }
        }
    });
    
    return windowId;
}
```

### 開啟非模態彈出視窗

```javascript
// 開啟非模態彈出視窗用於顯示輔助資訊
function showAccountDetail(accountId) {
    var posX = event.clientX + 10;
    var posY = event.clientY + 10;
    
    PTA.PopWindow.Popup.open('/PopupDialog.aspx', {
        url: '/Dialogs/AccountDetail.aspx',
        title: '科目明細',
        width: 400,
        height: 300,
        position: { x: posX, y: posY },
        parameters: { id: accountId },
        showClose: true,
        resizable: true
    });
}
```

### 處理視窗事件

```javascript
// 註冊視窗事件處理
PTA.PopWindow.Event.onBeforeOpen(function(windowInfo) {
    console.log('即將開啟視窗: ' + windowInfo.title);
    
    // 可以在此進行開啟前的驗證或預處理
    if (userPermission.level < 3 && windowInfo.url.indexOf('Admin') > -1) {
        alert('您沒有足夠權限開啟此視窗');
        return false; // 阻止視窗開啟
    }
    return true; // 允許視窗開啟
});

PTA.PopWindow.Event.onAfterClose(function(windowInfo) {
    console.log('視窗已關閉: ' + windowInfo.id);
    refreshDataIfNeeded(windowInfo.parameters);
});
```

### 動態調整視窗

```javascript
// 動態調整已開啟視窗的尺寸和位置
function resizeDialogBasedOnContent(windowId, content) {
    var contentSize = calculateContentSize(content);
    var newWidth = Math.min(800, contentSize.width + 40);
    var newHeight = Math.min(600, contentSize.height + 80);
    
    PTA.PopWindow.Modal.resize(windowId, newWidth, newHeight);
    PTA.PopWindow.Modal.center(windowId);
}
```

## 相依關係

ModPopFunction.js 依賴以下外部資源：

1. **jQuery**：核心依賴，用於DOM操作和事件處理
2. **jQuery UI**：用於對話框和拖曳功能實現
3. **pagefunction.js**：系統核心腳本，提供基礎功能支援

ModPopFunction.js 被系統中以下檔案直接依賴：

1. **PopupDialog.aspx**：非模態彈出對話框頁面
2. **ModelPopupDialog.aspx**：模態對話框頁面
3. **所有需要開啟彈出視窗的功能頁面**

## 安全性考量

ModPopFunction.js 實現了以下安全措施：

1. **參數驗證**：對所有函數參數進行類型和範圍驗證
2. **URL檢查**：對開啟視窗的URL進行合法性檢查
3. **參數清理**：在傳遞參數前進行清理，避免XSS攻擊
4. **同源檢查**：確保只載入同源的視窗內容
5. **權限控制**：與系統權限機制整合，控制視窗的開啟權限

## 效能考量

為確保最佳效能，ModPopFunction.js 採用以下策略：

1. **延遲初始化**：只在實際需要時初始化視窗系統
2. **資源回收**：關閉視窗時確保釋放所有資源
3. **事件限制**：使用事件委派，減少事件監聽器數量
4. **DOM操作優化**：最小化DOM操作次數，優化重繪效能
5. **樣式分離**：將視窗樣式集中管理，減少樣式計算開銷

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/02/10 | 系統開發部 | 初始版本                         |
| 1.1.0 | 2024/04/05 | 系統開發部 | 新增拖曳功能                     |
| 1.2.0 | 2024/06/18 | 系統開發部 | 改進多視窗管理                   |
| 1.2.1 | 2024/08/22 | 系統開發部 | 修復跨瀏覽器兼容性問題           |
| 1.3.0 | 2024/11/03 | 系統開發部 | 新增視窗動畫效果                 |
| 1.4.0 | 2025/01/15 | 系統開發部 | 加入響應式支援                   |
| 1.5.0 | 2025/03/20 | 系統開發部 | 優化效能並加強與pagefunction整合 |

## 待改進事項

1. **切換至原生對話框**：考慮使用HTML5原生對話框元素替換jQuery UI
2. **改善動畫效能**：使用CSS過渡效果替代JavaScript動畫
3. **增強無障礙功能**：改善視窗的無障礙支援
4. **模組化重構**：進一步模組化程式碼，提高可維護性
5. **深色主題支援**：增加對系統深色主題的適配
6. **記憶視窗位置**：實現對視窗位置和大小的記憶功能
7. **觸控優化**：加強在觸控設備上的操作體驗 