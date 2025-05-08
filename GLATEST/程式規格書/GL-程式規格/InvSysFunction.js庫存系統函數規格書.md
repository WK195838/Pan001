# InvSysFunction.js 庫存系統函數規格書

## 基本資訊

| 項目       | 內容                                   |
|------------|--------------------------------------|
| 程式代號     | InvSysFunction.js                   |
| 程式名稱     | 庫存系統函數                          |
| 檔案大小     | 2.9KB                               |
| 行數        | 77                                  |
| 功能簡述     | 提供庫存系統相關功能的客戶端函數庫       |
| 複雜度       | 中                                  |
| 規格書狀態   | 已完成                               |
| 負責人員     | Claude AI                          |
| 完成日期     | 2025/6/4                           |

## 程式功能概述

InvSysFunction.js 是泛太總帳系統中專門用於處理庫存系統相關功能的客戶端 JavaScript 函數庫。此檔案提供了一系列用於操作、計算和管理庫存資料的公用函數，使系統中的庫存相關頁面能夠統一呼叫這些功能，確保一致的使用者體驗和業務邏輯實現。

主要功能包括：

1. **庫存查詢與篩選**：提供快速查詢和篩選庫存資料的函數
2. **庫存計算**：實現庫存均價、總價值等各種計算函數
3. **庫存調整**：提供調整庫存數量和金額的功能
4. **庫存轉換**：實現不同單位間的庫存數量轉換
5. **庫存效期管理**：提供處理庫存有效期的相關功能
6. **與總帳整合**：實現庫存系統與總帳系統的整合函數
7. **表單驗證**：提供庫存相關表單的驗證功能

InvSysFunction.js 與泛太總帳系統的庫存模組緊密整合，提供了前端介面與後端資料處理之間的橋樑，確保庫存操作的準確性和一致性。

## 技術實現

InvSysFunction.js 主要使用原生 JavaScript 實現，採用模組化設計，並兼容主要瀏覽器。技術實現包括：

1. **命名空間模式**：
   - 使用 JavaScript 命名空間設計，避免全局變數污染
   - 採用 INV 命名空間封裝所有庫存相關函數

2. **函數組織**：
   - 按功能分組組織函數
   - 使用函數註釋清晰標記每個函數的用途和參數

3. **資料處理**：
   - 實現客戶端資料格式轉換
   - 提供資料驗證和錯誤處理

4. **AJAX 整合**：
   - 與服務端 API 整合的輔助函數
   - 處理非同步請求和響應

5. **DOM 互動**：
   - 提供操作庫存相關 DOM 元素的輔助函數
   - 實現動態表格生成和更新

## 程式碼結構

InvSysFunction.js 採用命名空間模式組織程式碼，主要結構如下：

```javascript
/**
 * InvSysFunction.js - 泛太總帳系統庫存函數庫
 * 提供庫存系統相關的客戶端功能
 */
var INV = INV || {};

/**
 * 庫存基本功能
 */
INV.Base = {
    /**
     * 初始化庫存系統函數
     */
    init: function() {
        // 初始化代碼
    },
    
    /**
     * 格式化庫存數量
     * @param {number} quantity 庫存數量
     * @param {string} unit 單位代碼
     * @return {string} 格式化後的數量
     */
    formatQuantity: function(quantity, unit) {
        // 數量格式化代碼
    },
    
    /**
     * 轉換庫存單位
     * @param {number} quantity 數量
     * @param {string} fromUnit 來源單位
     * @param {string} toUnit 目標單位
     * @return {number} 轉換後的數量
     */
    convertUnit: function(quantity, fromUnit, toUnit) {
        // 單位轉換代碼
    }
};

/**
 * 庫存計算功能
 */
INV.Calculation = {
    /**
     * 計算移動平均成本
     * @param {number} oldQuantity 原始數量
     * @param {number} oldCost 原始成本
     * @param {number} newQuantity 新增數量
     * @param {number} newCost 新增成本
     * @return {number} 計算後的移動平均成本
     */
    calculateAvgCost: function(oldQuantity, oldCost, newQuantity, newCost) {
        // 移動平均成本計算代碼
    },
    
    /**
     * 計算庫存總價值
     * @param {Array} inventoryItems 庫存項目陣列
     * @return {number} 總價值
     */
    calculateTotalValue: function(inventoryItems) {
        // 總價值計算代碼
    }
};

/**
 * 庫存驗證功能
 */
INV.Validation = {
    /**
     * 驗證庫存數量
     * @param {number} quantity 數量
     * @param {boolean} allowNegative 是否允許負值
     * @return {boolean} 驗證結果
     */
    validateQuantity: function(quantity, allowNegative) {
        // 數量驗證代碼
    },
    
    /**
     * 驗證庫存成本
     * @param {number} cost 成本金額
     * @return {boolean} 驗證結果
     */
    validateCost: function(cost) {
        // 成本驗證代碼
    }
};

/**
 * 庫存與總帳整合功能
 */
INV.GLIntegration = {
    /**
     * 生成庫存調整的會計分錄
     * @param {Object} adjustmentData 調整資料
     * @return {Object} 會計分錄資料
     */
    generateAdjustmentEntry: function(adjustmentData) {
        // 分錄生成代碼
    },
    
    /**
     * 檢查庫存科目設定
     * @param {string} accountCode 科目代碼
     * @return {boolean} 是否為庫存相關科目
     */
    isInventoryAccount: function(accountCode) {
        // 科目檢查代碼
    }
};
```

## 使用方式說明

### 初始化庫存功能

```javascript
// 頁面載入後初始化庫存函數
$(document).ready(function() {
    INV.Base.init();
});
```

### 庫存數量格式化

```javascript
// 格式化庫存數量
var quantity = 1234.56;
var unit = "PCS";
var formattedQty = INV.Base.formatQuantity(quantity, unit);
// 結果: "1,234.56 PCS"
```

### 庫存單位轉換

```javascript
// 庫存單位轉換
var boxQuantity = 10;
var pcsPerBox = 24;
var pcsQuantity = INV.Base.convertUnit(boxQuantity, "BOX", "PCS", pcsPerBox);
// 結果: 240 (10箱 x 24個/箱)
```

### 計算移動平均成本

```javascript
// 計算移動平均成本
var oldQty = 100;    // 原有庫存 100 單位
var oldCost = 50;    // 原有單位成本 $50
var newQty = 50;     // 新增 50 單位
var newCost = 60;    // 新增單位成本 $60

var avgCost = INV.Calculation.calculateAvgCost(oldQty, oldCost, newQty, newCost);
// 結果: 約 $53.33 ((100*50 + 50*60) / (100+50))
```

### 驗證庫存數據

```javascript
// 驗證庫存數量
var isValidQty = INV.Validation.validateQuantity(100.5, false);
// 結果: true (數量有效)

var isValidNegativeQty = INV.Validation.validateQuantity(-10, false);
// 結果: false (負數量，但不允許負值)

// 驗證庫存成本
var isValidCost = INV.Validation.validateCost(150.75);
// 結果: true (成本有效)
```

### 庫存調整生成會計分錄

```javascript
// 生成庫存調整的會計分錄
var adjustmentData = {
    date: "2024-06-04",
    items: [
        { itemId: "I001", quantity: 10, unitCost: 50 },
        { itemId: "I002", quantity: -5, unitCost: 40 }
    ],
    reason: "庫存盤點調整"
};

var journalEntry = INV.GLIntegration.generateAdjustmentEntry(adjustmentData);
// 返回可提交給總帳系統的分錄資料
```

## 相依關係

InvSysFunction.js 的相依關係：

1. **直接相依**：
   - **jQuery**：依賴 jQuery 進行 DOM 操作和 AJAX 請求
   - **pagefunction.js**：依賴系統共用的頁面函數
   - **Busy.js**：使用忙碌指示器展示處理狀態

2. **間接相依**：
   - **Bootstrap**：與 Bootstrap 組件相容但不強制依賴
   - **ExcelManger**：與 Excel 匯出功能協同工作
   - **WSAutoComplete**：使用自動完成服務進行項目搜尋

3. **被以下元件使用**：
   - **PTA0180**：庫存品項維護頁面
   - **PTA0190**：庫存交易維護頁面
   - **INR0150**：庫存報表頁面
   - **INR0160**：庫存盤點報表頁面
   - **所有處理庫存相關功能的頁面和控制項**

## 主要函數說明

### 基本功能函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `INV.Base.init()` | 無 | 無 | 初始化庫存系統函數 |
| `INV.Base.formatQuantity(quantity, unit)` | quantity: 數量<br>unit: 單位 | string | 格式化庫存數量顯示 |
| `INV.Base.convertUnit(quantity, fromUnit, toUnit, conversionFactor)` | quantity: 數量<br>fromUnit: 來源單位<br>toUnit: 目標單位<br>conversionFactor: 轉換係數 | number | 在不同單位間轉換數量 |
| `INV.Base.getItemInfo(itemCode)` | itemCode: 品項代碼 | Promise | 非同步獲取品項資訊 |

### 計算功能函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `INV.Calculation.calculateAvgCost(oldQty, oldCost, newQty, newCost)` | oldQty: 原始數量<br>oldCost: 原始成本<br>newQty: 新增數量<br>newCost: 新增成本 | number | 計算移動平均成本 |
| `INV.Calculation.calculateTotalValue(inventoryItems)` | inventoryItems: 庫存項目陣列 | number | 計算庫存總價值 |
| `INV.Calculation.calculateReorderPoint(avgUsage, leadTime, safetyStock)` | avgUsage: 平均用量<br>leadTime: 前置時間<br>safetyStock: 安全庫存 | number | 計算再訂購點 |

### 驗證功能函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `INV.Validation.validateQuantity(quantity, allowNegative)` | quantity: 數量<br>allowNegative: 允許負值 | boolean | 驗證庫存數量 |
| `INV.Validation.validateCost(cost)` | cost: 成本 | boolean | 驗證庫存成本 |
| `INV.Validation.validateTransaction(transactionData)` | transactionData: 交易資料 | object | 驗證庫存交易資料 |

### 總帳整合函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `INV.GLIntegration.generateAdjustmentEntry(adjustmentData)` | adjustmentData: 調整資料 | object | 生成庫存調整的會計分錄 |
| `INV.GLIntegration.isInventoryAccount(accountCode)` | accountCode: 科目代碼 | boolean | 檢查是否為庫存相關科目 |
| `INV.GLIntegration.postTransaction(transactionData)` | transactionData: 交易資料 | Promise | 過帳庫存交易 |

## 使用案例

### 庫存品項維護

```javascript
// 於PTA0180.aspx（庫存品項維護）中使用
function loadInventoryItem(itemCode) {
    // 顯示處理中訊息
    Busy.show("載入庫存品項中...");
    
    // 使用庫存函數獲取品項資訊
    INV.Base.getItemInfo(itemCode)
        .then(function(itemInfo) {
            // 填充表單
            $("#txtItemName").val(itemInfo.Name);
            $("#txtCategory").val(itemInfo.Category);
            $("#txtUnitCode").val(itemInfo.UnitCode);
            $("#txtStdCost").val(itemInfo.StandardCost);
            $("#txtCurrentQty").val(INV.Base.formatQuantity(itemInfo.Quantity, itemInfo.UnitCode));
            $("#txtReorderPoint").val(itemInfo.ReorderPoint);
            
            // 設置庫存會計科目
            $("#txtInvAccount").val(itemInfo.InventoryAccountCode);
        })
        .catch(function(error) {
            showError("無法載入庫存品項: " + error.message);
        })
        .finally(function() {
            Busy.hide();
        });
}
```

### 庫存交易處理

```javascript
// 於PTA0190.aspx（庫存交易維護）中使用
function processInventoryTransaction() {
    // 收集表單資料
    var transactionData = {
        transactionType: $("#ddlTransType").val(),
        date: $("#txtTransDate").val(),
        reference: $("#txtReference").val(),
        description: $("#txtDescription").val(),
        items: []
    };
    
    // 收集明細行項目
    $(".inv-transaction-item").each(function() {
        var row = $(this);
        var item = {
            itemCode: row.find(".item-code").val(),
            quantity: parseFloat(row.find(".item-quantity").val()),
            unitCost: parseFloat(row.find(".item-unit-cost").val()),
            locationCode: row.find(".item-location").val()
        };
        
        transactionData.items.push(item);
    });
    
    // 驗證交易資料
    var validationResult = INV.Validation.validateTransaction(transactionData);
    if (!validationResult.isValid) {
        showError("交易資料錯誤: " + validationResult.message);
        return;
    }
    
    // 處理庫存交易
    Busy.show("處理庫存交易中...");
    
    // 過帳到總帳
    INV.GLIntegration.postTransaction(transactionData)
        .then(function(result) {
            showSuccess("交易處理完成, 編號: " + result.transactionId);
            resetForm();
        })
        .catch(function(error) {
            showError("交易處理失敗: " + error.message);
        })
        .finally(function() {
            Busy.hide();
        });
}
```

### 庫存報表生成

```javascript
// 於INR0150.aspx（庫存報表）中使用
function generateInventoryReport() {
    // 獲取報表參數
    var reportParams = {
        asOfDate: $("#txtAsOfDate").val(),
        categoryFilter: $("#ddlCategory").val(),
        locationFilter: $("#ddlLocation").val(),
        showZeroQty: $("#chkShowZero").is(":checked")
    };
    
    // 顯示處理中
    Busy.show("生成庫存報表中...");
    
    // 取得報表資料
    $.ajax({
        type: "POST",
        url: "INR0150.aspx/GetReportData",
        data: JSON.stringify({ parameters: reportParams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            var reportData = response.d;
            
            // 計算報表總計值
            var totalValue = INV.Calculation.calculateTotalValue(reportData.Items);
            
            // 顯示報表
            displayInventoryReport(reportData, totalValue);
        },
        error: function(xhr, status, error) {
            showError("報表生成失敗: " + error);
        },
        complete: function() {
            Busy.hide();
        }
    });
}

function displayInventoryReport(reportData, totalValue) {
    var table = $("#tblInventory");
    table.empty();
    
    // 生成表頭
    var thead = $("<thead></thead>");
    var headerRow = $("<tr></tr>");
    headerRow.append("<th>品項代碼</th>");
    headerRow.append("<th>品項名稱</th>");
    headerRow.append("<th>類別</th>");
    headerRow.append("<th>位置</th>");
    headerRow.append("<th>單位</th>");
    headerRow.append("<th>數量</th>");
    headerRow.append("<th>單位成本</th>");
    headerRow.append("<th>總價值</th>");
    thead.append(headerRow);
    table.append(thead);
    
    // 生成表身
    var tbody = $("<tbody></tbody>");
    $.each(reportData.Items, function(index, item) {
        var row = $("<tr></tr>");
        row.append("<td>" + item.ItemCode + "</td>");
        row.append("<td>" + item.ItemName + "</td>");
        row.append("<td>" + item.Category + "</td>");
        row.append("<td>" + item.Location + "</td>");
        row.append("<td>" + item.UnitCode + "</td>");
        row.append("<td class='text-right'>" + INV.Base.formatQuantity(item.Quantity, item.UnitCode) + "</td>");
        row.append("<td class='text-right'>" + item.UnitCost.toFixed(2) + "</td>");
        row.append("<td class='text-right'>" + (item.Quantity * item.UnitCost).toFixed(2) + "</td>");
        tbody.append(row);
    });
    table.append(tbody);
    
    // 生成表尾
    var tfoot = $("<tfoot></tfoot>");
    var footerRow = $("<tr></tr>");
    footerRow.append("<td colspan='7' class='text-right'><strong>總庫存價值:</strong></td>");
    footerRow.append("<td class='text-right'><strong>" + totalValue.toFixed(2) + "</strong></td>");
    tfoot.append(footerRow);
    table.append(tfoot);
}
```

## 安全性考量

InvSysFunction.js 在設計時考慮了以下安全事項：

1. **輸入驗證**：
   - 所有使用者輸入進行有效性驗證
   - 防止非數值資料輸入到數值欄位
   - 避免XSS攻擊的數據清理

2. **授權檢查**：
   - 配合系統權限機制進行操作授權檢查
   - 敏感操作（如成本調整）需要額外確認

3. **數據保護**：
   - 敏感數據（如成本資訊）的顯示控制
   - 使用適當的資料加密和解密措施

4. **錯誤處理**：
   - 全面的例外處理
   - 用戶友好的錯誤訊息
   - 防止資訊洩漏的錯誤處理

## 效能考量

為確保 InvSysFunction.js 的良好效能，設計中考慮了以下幾點：

1. **計算優化**：
   - 複雜計算使用優化算法
   - 避免重複計算相同結果
   - 使用快取儲存常用結果

2. **資料處理**：
   - 僅處理必要資料，減少記憶體使用
   - 大量資料分批處理，避免瀏覽器阻塞
   - 適時清理不再需要的資料

3. **AJAX 優化**：
   - 使用非同步請求，不阻塞使用者介面
   - 批量處理多個請求，減少伺服器往返
   - 實施適當的請求節流和防抖

4. **DOM 操作**：
   - 減少直接 DOM 操作
   - 使用文檔片段批量更新 DOM
   - 避免頻繁重繪和重排版

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/03/20 | 系統開發部 | 修正數量轉換功能                 |
| 1.1.0 | 2024/05/10 | 系統開發部 | 新增庫存效期管理功能             |
| 1.1.1 | 2024/07/18 | 系統開發部 | 修正庫存成本計算邏輯             |
| 1.2.0 | 2024/09/25 | 系統開發部 | 新增庫存預警功能                 |
| 1.2.1 | 2025/01/15 | 系統開發部 | 效能優化與程式碼重構             |

## 待改進事項

1. **功能擴展**：
   - 增加批次處理庫存調整功能
   - 實作庫存預測分析功能
   - 支援多倉庫管理和庫存轉移功能

2. **使用者體驗**：
   - 提供更直觀的數據可視化
   - 改進表單驗證提示
   - 增加即時庫存狀態指示器

3. **技術改進**：
   - 重構為 ES6 模組格式
   - 增加單元測試覆蓋
   - 分離核心邏輯和 UI 操作

4. **整合改進**：
   - 加強與採購模組的整合
   - 改進與銷售訂單系統的連接
   - 實現與行動裝置庫存掃描系統的整合

5. **文檔與維護**：
   - 完善函數文檔
   - 增加使用示例
   - 標準化錯誤處理機制 