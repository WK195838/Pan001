# pagefunction.js 頁面函數規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | pagefunction.js                      |
| 程式名稱     | 頁面函數                               |
| 檔案大小     | 20KB                                 |
| 行數        | 527                                  |
| 功能簡述     | 提供頁面共用功能                        |
| 複雜度       | 高                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/25                           |

## 程式功能概述

pagefunction.js 是泛太總帳系統中的關鍵客戶端腳本文件，為所有網頁提供統一的共用JavaScript函數庫。此腳本提供了豐富的前端功能支援，使系統具有高度互動性和良好的使用者體驗。

主要功能包括：

1. **DOM 操作函數**：提供一系列簡化的DOM元素選取、修改和操作功能
2. **表單處理**：表單驗證、提交、重置和資料處理相關函數
3. **資料格式化**：數字、日期、貨幣等資料的格式化和轉換函數
4. **視窗控制**：彈出視窗、對話框和模態視窗的開啟與管理
5. **AJAX 封裝**：簡化的AJAX請求處理，支援與後端服務的非同步通訊
6. **事件處理**：統一的事件綁定、解除和處理機制
7. **控制項整合**：與系統各種自定義控制項的整合函數
8. **資料驗證**：客戶端資料驗證邏輯，提供即時的使用者反饋
9. **UI效果**：動畫效果、載入指示器等使用者介面增強功能
10. **鍵盤導航**：支援鍵盤快捷鍵和增強的鍵盤操作
11. **訊息處理**：統一的錯誤、警告和資訊訊息顯示機制
12. **列印功能**：頁面列印和列印預覽相關功能
13. **報表操作**：與報表顯示和操作相關的客戶端功能
14. **本地化支援**：多語言和區域設定相關的輔助函數
15. **安全性功能**：基本的XSS防護和資料安全處理機制

pagefunction.js 的設計採用模組化結構，使系統各頁面可以共享一致的功能和使用者體驗，同時簡化前端開發工作。此腳本是泛太總帳系統前端架構的核心組件，所有網頁都依賴於此腳本提供的功能。

## 技術實現

pagefunction.js 採用以下技術實現其功能：

1. **原生 JavaScript**：
   - 使用純JavaScript實現所有功能，確保最大的兼容性
   - 避免對現代JavaScript特性的依賴，兼容舊版瀏覽器
   - 採用立即執行函數表達式(IIFE)封裝所有功能，避免全局污染

2. **jQuery 整合**：
   - 與jQuery庫深度整合，簡化DOM操作和事件處理
   - 使用jQuery的選擇器機制提高代碼可讀性
   - 支援jQuery插件的擴展和使用

3. **瀏覽器兼容性處理**：
   - 內建瀏覽器特性檢測機制
   - 為不同瀏覽器提供統一的功能介面
   - 解決常見的瀏覽器兼容性問題

4. **命名空間設計**：
   - 採用PTA命名空間隔離系統函數
   - 分層次的功能組織，便於維護和擴展
   - 避免與其他腳本庫的衝突

5. **效能優化**：
   - 實現事件委派減少事件綁定數量
   - 使用函數節流和防抖技術處理高頻事件
   - 採用物件池和記憶體管理技術減少資源消耗

6. **模組化結構**：
   - 按功能領域劃分模組
   - 支援延遲載入非核心功能
   - 明確的功能依賴關係管理

7. **錯誤處理機制**：
   - 全局錯誤捕獲和記錄
   - 提供詳細的錯誤訊息和追蹤資訊
   - 優雅降級機制確保核心功能可用

8. **資源管理**：
   - 自動化的記憶體管理和清理
   - 資源載入狀態監控
   - 防止記憶體洩漏的設計模式

9. **安全機制**：
   - 輸入資料的過濾和轉義
   - CSRF防護函數
   - 敏感資料處理的安全實踐

10. **調試支援**：
    - 內建的調試日誌系統
    - 條件式調試代碼
    - 效能監控點

## 函數庫架構

pagefunction.js 採用模組化架構，主要包含以下核心模組：

1. **PTA.Core** - 核心功能模組
   - 基礎工具函數
   - 命名空間管理
   - 初始化和配置

2. **PTA.UI** - 使用者介面模組
   - 元素操作和樣式管理
   - 視窗和對話框控制
   - 使用者介面特效

3. **PTA.Data** - 資料處理模組
   - 資料格式化和轉換
   - 資料驗證和過濾
   - 本地資料管理

4. **PTA.Net** - 網路通訊模組
   - AJAX請求封裝
   - WebService調用
   - 檔案上傳和下載

5. **PTA.Form** - 表單處理模組
   - 表單驗證
   - 表單提交和處理
   - 動態表單控制

6. **PTA.Grid** - 資料表格模組
   - 表格操作和控制
   - 排序和過濾
   - 分頁處理

7. **PTA.Report** - 報表模組
   - 報表顯示和控制
   - 資料匯出
   - 列印控制

8. **PTA.Event** - 事件處理模組
   - 統一事件管理
   - 自定義事件機制
   - 鍵盤和滑鼠事件處理

9. **PTA.Util** - 實用工具模組
   - 日期和時間處理
   - 字符串操作
   - 數學計算工具

10. **PTA.Integration** - 整合模組
    - 與系統控制項整合
    - 第三方庫適配
    - 插件管理

## 主要函數參考

### 核心函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.initialize` | `function(config)` | 初始化系統前端環境 | config: 配置選項物件 | 無 |
| `PTA.ready` | `function(callback)` | 註冊DOM就緒後的回調 | callback: 回調函數 | 無 |
| `PTA.extend` | `function(target, source)` | 擴展物件屬性 | target: 目標物件, source: 來源物件 | 合併後的物件 |
| `PTA.namespace` | `function(ns)` | 創建或獲取命名空間 | ns: 命名空間字串 | 命名空間物件 |
| `PTA.log` | `function(message, level)` | 記錄調試信息 | message: 訊息內容, level: 日誌等級 | 無 |

### UI 操作函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.UI.showDialog` | `function(options)` | 顯示對話框 | options: 對話框選項 | 對話框控制物件 |
| `PTA.UI.hideDialog` | `function(id)` | 隱藏對話框 | id: 對話框ID | 無 |
| `PTA.UI.showMessage` | `function(message, type)` | 顯示訊息提示 | message: 訊息內容, type: 訊息類型 | 無 |
| `PTA.UI.toggleElement` | `function(selector)` | 切換元素顯示狀態 | selector: 元素選擇器 | 元素當前狀態 |
| `PTA.UI.showLoading` | `function(message)` | 顯示載入指示器 | message: 載入訊息 | 無 |

### 資料處理函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.Data.formatCurrency` | `function(value, decimals)` | 格式化貨幣值 | value: 數值, decimals: 小數位數 | 格式化字串 |
| `PTA.Data.formatDate` | `function(date, format)` | 格式化日期 | date: 日期物件, format: 格式模式 | 格式化字串 |
| `PTA.Data.parseDate` | `function(dateStr, format)` | 解析日期字串 | dateStr: 日期字串, format: 格式模式 | Date物件 |
| `PTA.Data.validate` | `function(value, rules)` | 驗證資料 | value: 待驗證值, rules: 驗證規則 | 驗證結果 |
| `PTA.Data.encodeHTML` | `function(str)` | HTML編碼 | str: 原始字串 | 編碼後字串 |

### 表單處理函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.Form.submit` | `function(formId, options)` | 提交表單 | formId: 表單ID, options: 提交選項 | 無 |
| `PTA.Form.validate` | `function(formId)` | 驗證表單 | formId: 表單ID | 驗證結果 |
| `PTA.Form.reset` | `function(formId)` | 重置表單 | formId: 表單ID | 無 |
| `PTA.Form.getValues` | `function(formId)` | 獲取表單值 | formId: 表單ID | 表單值物件 |
| `PTA.Form.setValues` | `function(formId, values)` | 設置表單值 | formId: 表單ID, values: 值物件 | 無 |

### AJAX 函數

| 函數名稱 | 函數簽名 | 功能描述 | 參數說明 | 返回值 |
|---------|---------|---------|---------|-------|
| `PTA.Net.ajax` | `function(url, options)` | 發送AJAX請求 | url: 請求URL, options: 請求選項 | 請求物件 |
| `PTA.Net.getJSON` | `function(url, data, callback)` | 獲取JSON資料 | url: 請求URL, data: 請求參數, callback: 回調函數 | 請求物件 |
| `PTA.Net.post` | `function(url, data, callback)` | 發送POST請求 | url: 請求URL, data: 請求資料, callback: 回調函數 | 請求物件 |
| `PTA.Net.callService` | `function(service, method, params, callback)` | 調用Web服務 | service: 服務名, method: 方法名, params: 參數, callback: 回調函數 | 無 |
| `PTA.Net.upload` | `function(formId, options)` | 上傳檔案 | formId: 表單ID, options: 上傳選項 | 上傳控制物件 |

## 使用範例

以下是pagefunction.js的一些常見使用範例：

### 初始化系統

```javascript
PTA.initialize({
    debug: true,
    baseUrl: '/GLA/',
    dateFormat: 'yyyy/MM/dd',
    currencyFormat: '#,##0.00'
});

PTA.ready(function() {
    // 系統初始化完成後執行
    PTA.log('系統已初始化');
});
```

### 使用對話框

```javascript
// 顯示確認對話框
PTA.UI.showDialog({
    id: 'confirmDialog',
    title: '確認操作',
    content: '確定要刪除此記錄嗎？',
    buttons: [{
        text: '確定',
        click: function() {
            deleteRecord(123);
            PTA.UI.hideDialog('confirmDialog');
        }
    }, {
        text: '取消',
        click: function() {
            PTA.UI.hideDialog('confirmDialog');
        }
    }]
});
```

### 表單處理

```javascript
// 驗證並提交表單
$('#btnSubmit').click(function() {
    if (PTA.Form.validate('mainForm')) {
        PTA.Form.submit('mainForm', {
            success: function(response) {
                PTA.UI.showMessage('提交成功', 'success');
            },
            error: function(error) {
                PTA.UI.showMessage('提交失敗: ' + error, 'error');
            }
        });
    }
});
```

### AJAX資料獲取

```javascript
// 獲取科目列表
PTA.Net.getJSON('/WSGLAcctDefJson.asmx/GetSubjects', {
    companyId: '001',
    yearPeriod: '202501'
}, function(data) {
    if (data.success) {
        buildSubjectList(data.subjects);
    } else {
        PTA.UI.showMessage(data.message, 'warning');
    }
});
```

### 日期處理

```javascript
// 格式化和解析日期
var today = new Date();
var formattedDate = PTA.Data.formatDate(today, 'yyyy年MM月dd日');
console.log('今天是: ' + formattedDate);

var dateStr = '2025/05/01';
var dateObj = PTA.Data.parseDate(dateStr, 'yyyy/MM/dd');
console.log('解析後的月份: ' + (dateObj.getMonth() + 1));
```

## 相依關係

pagefunction.js 依賴以下外部資源：

1. **jQuery**：核心依賴，用於DOM操作和AJAX通訊
2. **jQuery UI**：用於對話框和部分UI元件
3. **Bootstrap**：部分UI組件和樣式依賴

pagefunction.js 被系統中以下檔案直接依賴：

1. **GLA.master**：系統主版面
2. **所有ASPX頁面**：所有功能頁面都引用此腳本
3. **其他JavaScript檔案**：如ModPopFunction.js, Busy.js等

## 安全性考量

pagefunction.js 實現了以下安全措施：

1. **資料驗證**：所有用戶輸入都經過驗證
2. **XSS防護**：自動轉義HTML敏感字符
3. **CSRF防護**：AJAX請求自動附加安全令牌
4. **錯誤處理**：捕獲並安全處理異常
5. **敏感資料處理**：不在前端存儲敏感資訊

## 效能考量

為確保最佳效能，pagefunction.js 採用以下策略：

1. **資源緩存**：緩存常用DOM查詢結果
2. **延遲載入**：非核心功能按需載入
3. **事件委派**：減少直接事件綁定
4. **節流和防抖**：控制高頻事件執行
5. **壓縮版本**：生產環境使用最小化版本

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.1.0 | 2024/03/10 | 系統開發部 | 新增報表相關功能                 |
| 1.2.0 | 2024/05/22 | 系統開發部 | 改進表單驗證功能                 |
| 1.2.1 | 2024/07/05 | 系統開發部 | 修復IE兼容性問題                 |
| 1.3.0 | 2024/09/18 | 系統開發部 | 新增本地化支援                   |
| 1.4.0 | 2025/02/03 | 系統開發部 | 加強安全性機制                   |
| 1.5.0 | 2025/04/12 | 系統開發部 | 優化效能並新增AJAX功能           |

## 待改進事項

1. **模組化改進**：進一步拆分為更小的功能模組，支援按需載入
2. **TypeScript轉換**：考慮使用TypeScript重寫以提高代碼質量和維護性
3. **單元測試**：增加前端單元測試覆蓋
4. **現代化改造**：逐步引入更現代的JavaScript特性
5. **文檔完善**：建立更詳細的API文檔
6. **瀏覽器支援**：評估放棄對舊版瀏覽器的支援可能性
7. **效能優化**：針對大型報表的前端處理進行進一步優化 