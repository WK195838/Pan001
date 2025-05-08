# Busy.js 忙碌指示器規格書

## 基本資訊

| 項目       | 內容                                   |
|------------|--------------------------------------|
| 程式代號     | Busy.js                              |
| 程式名稱     | 忙碌指示器                            |
| 檔案大小     | 2.4KB                               |
| 行數        | 51                                  |
| 功能簡述     | 提供系統處理中的忙碌狀態顯示            |
| 複雜度       | 低                                  |
| 規格書狀態   | 已完成                               |
| 負責人員     | Claude AI                          |
| 完成日期     | 2025/6/3                           |

## 程式功能概述

Busy.js 是泛太總帳系統中的客戶端腳本，用於提供使用者介面的忙碌狀態指示功能。當系統執行較長時間的操作（如資料處理、報表生成或 AJAX 請求等）時，Busy.js 會顯示一個視覺化的忙碌指示器，提供使用者明確的反饋，表明系統正在處理中，避免使用者產生系統無回應的誤解。

主要功能包括：

1. **忙碌狀態顯示**：在長時間操作期間顯示動態視覺效果
2. **自動偵測**：能夠自動偵測 AJAX 請求和表單提交
3. **手動控制**：允許開發人員在需要時手動控制忙碌指示器的顯示和隱藏
4. **客製化選項**：提供多種效果選項和樣式設定
5. **全局遮罩**：防止用戶在處理過程中進行其他操作

Busy.js 通過提供清晰的視覺反饋，大幅提升了系統的用戶體驗，特別是在網路連接速度不穩定或資料處理量大的情況下。

## 技術實現

Busy.js 使用純 JavaScript 和 CSS 實現，無依賴其他第三方庫，確保最小的載入時間和最大的相容性。腳本使用模組模式（Module Pattern）設計，主要包含以下技術實現：

1. **DOM 操作**：
   - 動態建立指示器元素
   - 控制元素顯示和隱藏
   - 調整元素的樣式和位置

2. **事件監聽**：
   - 監聽全局 AJAX 請求開始和結束事件
   - 監聽表單提交事件
   - 監聽頁面載入和卸載事件

3. **CSS 動畫**：
   - 使用 CSS3 動畫效果實現動態指示器
   - 支援多種動畫類型（旋轉、脈動、波紋等）
   - 針對不同瀏覽器優化的視覺效果

4. **計時器控制**：
   - 延遲顯示忙碌指示器以避免閃爍
   - 自動超時機制防止指示器無限顯示

## 程式碼結構

Busy.js 採用模組化設計，主要包含以下核心部分：

```javascript
/**
 * Busy.js - 泛太總帳系統忙碌指示器
 * 提供頁面處理中的視覺反饋
 */
var GlaBusy = (function() {
    // 私有變數
    var _isVisible = false;
    var _container = null;
    var _overlay = null;
    var _indicator = null;
    var _timer = null;
    var _options = {
        type: 'spinner',    // 忙碌指示器類型: spinner, pulse, dots
        text: '處理中，請稍候...',   // 顯示文字
        overlay: true,      // 是否顯示遮罩
        delay: 300,         // 延遲顯示時間（毫秒）
        timeout: 60000,     // 自動隱藏超時（毫秒）
        zIndex: 9999        // z-index 值
    };
    
    // 私有方法
    function _createElements() {
        /* 建立指示器元素的代碼 */
    }
    
    function _applyStyles() {
        /* 套用樣式的代碼 */
    }
    
    // 公開API
    return {
        /**
         * 初始化忙碌指示器
         * @param {Object} options 選項配置
         */
        init: function(options) {
            /* 初始化代碼 */
        },
        
        /**
         * 顯示忙碌指示器
         * @param {string} text 可選，顯示的文字
         */
        show: function(text) {
            /* 顯示指示器的代碼 */
        },
        
        /**
         * 隱藏忙碌指示器
         */
        hide: function() {
            /* 隱藏指示器的代碼 */
        },
        
        /**
         * 檢查忙碌指示器是否可見
         * @return {boolean} 是否可見
         */
        isVisible: function() {
            return _isVisible;
        },
        
        /**
         * 附加到 AJAX 請求
         * @param {boolean} enabled 是否啟用
         */
        attachToAjax: function(enabled) {
            /* 附加到AJAX請求的代碼 */
        },
        
        /**
         * 附加到表單提交
         * @param {string} formSelector 表單選擇器
         */
        attachToForm: function(formSelector) {
            /* 附加到表單提交的代碼 */
        }
    };
})();

// 頁面載入完成後自動初始化
document.addEventListener('DOMContentLoaded', function() {
    GlaBusy.init();
    GlaBusy.attachToAjax(true);
});
```

## 使用方式說明

### 基本使用

```javascript
// 手動顯示忙碌指示器
GlaBusy.show();

// 執行某些操作...
performLongOperation();

// 手動隱藏忙碌指示器
GlaBusy.hide();
```

### 自訂文字

```javascript
// 顯示含自訂文字的忙碌指示器
GlaBusy.show("報表生成中，請稍候...");
```

### 自訂初始化選項

```javascript
// 初始化時自訂選項
GlaBusy.init({
    type: 'pulse',             // 使用脈動效果
    text: '資料載入中...',     // 預設文字
    overlay: true,             // 顯示遮罩
    delay: 500,                // 延遲500毫秒顯示
    timeout: 30000,            // 30秒後自動隱藏
    zIndex: 10000              // 設置z-index值
});
```

### 與 AJAX 整合

```javascript
// 自動顯示於所有 AJAX 請求期間
GlaBusy.attachToAjax(true);

// 發送 AJAX 請求
$.ajax({
    url: 'GetData.aspx',
    success: function(data) {
        processData(data);
    },
    error: function(error) {
        handleError(error);
    }
    // 不需要手動控制 GlaBusy.show/hide，會自動處理
});
```

### 表單提交整合

```javascript
// 附加到特定表單
GlaBusy.attachToForm('#dataForm');

// 提交表單時會自動顯示忙碌指示器
$('#dataForm').submit();
```

## 相依關係

Busy.js 的相依關係非常簡單，確保了其高度獨立性和可移植性：

1. **直接相依**：
   - 無直接依賴外部庫
   - 僅依賴標準 DOM API 和原生 JavaScript

2. **間接相依**：
   - 與 jQuery 相容（如果存在）但不強制依賴
   - 可選擇性地整合 pagefunction.js 提供的功能增強

3. **被以下元件使用**：
   - 所有需要長時間處理的頁面
   - 報表生成頁面（如GLR0220、GLR0230等）
   - 資料匯入匯出功能
   - 批次處理頁面

## CSS 樣式說明

Busy.js 自帶定義了以下主要 CSS 樣式，用於呈現忙碌指示器：

```css
/* 忙碌指示器容器 */
.gla-busy-container {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: none;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

/* 背景遮罩 */
.gla-busy-overlay {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
}

/* 指示器面板 */
.gla-busy-panel {
    position: relative;
    padding: 20px;
    background-color: white;
    border-radius: 5px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
    text-align: center;
    min-width: 200px;
}

/* 旋轉指示器 */
.gla-busy-spinner {
    width: 40px;
    height: 40px;
    margin: 0 auto 10px;
    border: 3px solid #f3f3f3;
    border-top: 3px solid #3498db;
    border-radius: 50%;
    animation: gla-busy-spin 1s linear infinite;
}

/* 指示器文字 */
.gla-busy-text {
    font-family: "Microsoft JhengHei", sans-serif;
    font-size: 14px;
    color: #333;
}

/* 旋轉動畫 */
@keyframes gla-busy-spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
```

## 整合案例

### 整合到報表生成流程

以下示例展示了 Busy.js 如何整合到報表生成流程中：

```javascript
// 在GLR0220.aspx（餘額試算表）中的應用
function generateBalanceReport() {
    // 顯示忙碌指示器
    GlaBusy.show("餘額試算表產生中，請稍候...");
    
    try {
        // 收集報表參數
        var params = {
            companyId: $("#CompanyList").val(),
            year: $("#YearList").val(),
            period: $("#PeriodList").val(),
            accountFrom: $("#AccountFrom").val(),
            accountTo: $("#AccountTo").val(),
            reportType: $("input[name='ReportType']:checked").val()
        };
        
        // 發送報表請求
        $.ajax({
            type: "POST",
            url: "GLR0220.aspx/GenerateReport",
            data: JSON.stringify({ parameters: params }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                // 處理結果
                if (response.d.Success) {
                    displayReport(response.d.ReportData);
                } else {
                    showError(response.d.ErrorMessage);
                }
            },
            error: function(xhr, status, error) {
                showError("報表生成失敗: " + error);
            },
            complete: function() {
                // 隱藏忙碌指示器
                GlaBusy.hide();
            }
        });
    } catch (ex) {
        showError("發生例外: " + ex.message);
        GlaBusy.hide();
    }
}
```

### 整合到資料匯出功能

```javascript
// 在ExcelManger中的應用
function exportToExcel(tableId, fileName) {
    GlaBusy.show("資料匯出中，請稍候...");
    
    setTimeout(function() {
        try {
            // 匯出邏輯
            var table = document.getElementById(tableId);
            if (!table) {
                throw new Error("找不到指定的表格");
            }
            
            // Excel匯出處理...
            var excelData = convertTableToExcel(table);
            
            // 提供下載
            saveExcelFile(excelData, fileName);
        } catch (ex) {
            alert("匯出失敗: " + ex.message);
        } finally {
            GlaBusy.hide();
        }
    }, 100);
}
```

## 效能考量

為確保 Busy.js 不會對系統性能造成負面影響，設計中已考慮以下幾點：

1. **最小化 DOM 操作**：
   - 指示器元素在初始化時創建一次
   - 隱藏和顯示使用 CSS 屬性而非重建元素
   - 避免頻繁的 DOM 查詢

2. **延遲顯示機制**：
   - 短時操作不會顯示指示器，避免閃爍
   - 使用 setTimeout 延遲顯示忙碌指示器

3. **資源佔用最小化**：
   - 使用 CSS 動畫代替 JavaScript 動畫
   - 避免使用大型圖片資源
   - 使用輕量級 SVG 或 CSS 實現視覺效果

4. **防止內存洩漏**：
   - 正確清理事件監聽器
   - 清除定時器
   - 避免循環引用

## 瀏覽器相容性

Busy.js 支援以下瀏覽器：

| 瀏覽器 | 最低版本 | 備註 |
|--------|---------|------|
| Chrome | 49+ | 完全支援 |
| Firefox | 52+ | 完全支援 |
| Edge | 15+ | 完全支援 |
| IE | 11 | 基本支援，部分視覺效果降級 |
| Safari | 10+ | 完全支援 |
| Opera | 36+ | 完全支援 |

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/03/10 | 系統開發部 | 修正IE11相容性問題               |
| 1.1.0 | 2024/05/05 | 系統開發部 | 新增多種視覺效果選項             |
| 1.1.1 | 2024/07/12 | 系統開發部 | 修正AJAX整合的一些邊緣情況       |
| 1.2.0 | 2024/09/20 | 系統開發部 | 新增自動超時機制                 |
| 1.2.1 | 2025/01/10 | 系統開發部 | 效能優化                         |

## 待改進事項

1. **功能擴展**：
   - 增加更多動畫效果選項
   - 支援多個獨立指示器（區域性指示器）
   - 提供進度指示功能

2. **使用者體驗**：
   - 增加可配置的音效提示
   - 支援無障礙功能
   - 提供更精細的狀態提示

3. **技術升級**：
   - 考慮重構為ES6模組
   - 提供TypeScript定義
   - 增強與現代前端框架的整合

4. **國際化支援**：
   - 提供多語言支援
   - 文字符合不同區域習慣

5. **執行環境**：
   - 提高在行動設備上的適應性
   - 優化在低效能設備上的表現 