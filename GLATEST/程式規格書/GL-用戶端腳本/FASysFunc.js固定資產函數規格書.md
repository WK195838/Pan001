# FASysFunc.js 固定資產函數規格書

## 基本資訊

| 項目       | 內容                                   |
|------------|--------------------------------------|
| 程式代號     | FASysFunc.js                        |
| 程式名稱     | 固定資產函數                          |
| 檔案大小     | 7.8KB                               |
| 行數        | 213                                 |
| 功能簡述     | 提供固定資產相關功能的客戶端函數庫       |
| 複雜度       | 高                                  |
| 規格書狀態   | 已完成                               |
| 負責人員     | Claude AI                          |
| 完成日期     | 2025/6/5                           |

## 程式功能概述

FASysFunc.js 是泛太總帳系統中專門用於處理固定資產相關功能的客戶端 JavaScript 函數庫。此檔案提供了一系列用於操作、計算和管理固定資產資料的公用函數，使系統中的固定資產相關頁面能夠統一呼叫這些功能，確保一致的使用者體驗和業務邏輯實現。

主要功能包括：

1. **資產資訊管理**：提供固定資產基本資料的處理功能
2. **折舊計算**：實現各種折舊方法的計算函數（直線法、餘額遞減法、年數合計法等）
3. **資產評估**：提供資產重估和減損的處理函數
4. **資產清理**：實現資產報廢和處分的相關功能
5. **資產轉移**：處理資產部門間轉移的相關功能
6. **與總帳整合**：實現固定資產系統與總帳系統的整合函數
7. **報表計算**：提供固定資產報表相關的計算功能

FASysFunc.js 與泛太總帳系統的固定資產模組緊密整合，提供了前端介面與後端資料處理之間的橋樑，確保固定資產操作的準確性和一致性，特別是在複雜的折舊計算和會計處理方面。

## 技術實現

FASysFunc.js 使用原生 JavaScript 實現，採用模組化設計，並兼容主要瀏覽器。由於固定資產功能的複雜性，特別是折舊計算和財務處理，此檔案的技術實現較為複雜：

1. **命名空間模式**：
   - 使用 JavaScript 命名空間設計，避免全局變數污染
   - 採用 FA 命名空間封裝所有固定資產相關函數

2. **模組化組織**：
   - 按功能領域（基本資訊、折舊、評估等）分組組織函數
   - 使用 IIFE 模式確保模組獨立性

3. **計算引擎**：
   - 高精度數值計算，避免 JavaScript 浮點數精度問題
   - 實現多種折舊方法的計算算法
   - 提供靈活的折舊參數配置

4. **資料處理**：
   - 資產資料的格式轉換和驗證
   - 資產歷史記錄的追蹤和處理
   - 配置文件解析和應用

5. **與後端整合**：
   - AJAX 請求封裝和標準化
   - 資料更新和同步機制
   - 異常處理和錯誤報告

6. **使用者介面支援**：
   - 提供資產資料的視覺化配置
   - 動態表單生成和驗證
   - 互動式資產操作輔助功能

## 程式碼結構

FASysFunc.js 採用命名空間和模組化設計，主要結構如下：

```javascript
/**
 * FASysFunc.js - 泛太總帳系統固定資產函數庫
 * 提供固定資產系統相關的客戶端功能
 */
var FA = (function(FA, $) {
    // 私有變數和函數
    var _settings = {
        defaultDepreciationMethod: 'SL',  // 預設折舊方法：直線法(SL)
        defaultLife: 60,                  // 預設使用年限(月)
        defaultSalvageRate: 0.1,          // 預設殘值率(10%)
        roundingPrecision: 2,             // 計算精度(小數位)
        depreciationRoundingMethod: 'round' // 折舊金額進位方式
    };
    
    // 私有工具函數
    function _formatCurrency(amount) {
        // 格式化金額為貨幣格式
    }
    
    function _calculateMonthsBetween(startDate, endDate) {
        // 計算兩日期間的月數
    }
    
    // 公開 API - 基本功能
    FA.Base = {
        /**
         * 初始化固定資產函數庫
         * @param {Object} options 配置選項
         */
        init: function(options) {
            _settings = $.extend({}, _settings, options || {});
        },
        
        /**
         * 取得資產基本資訊
         * @param {string} assetId 資產編號
         * @return {Promise} 資產資訊Promise物件
         */
        getAssetInfo: function(assetId) {
            // 實現資產資訊獲取邏輯
        },
        
        /**
         * 格式化資產金額
         * @param {number} amount 金額
         * @param {boolean} includeCurrency 是否包含貨幣符號
         * @return {string} 格式化後的金額字串
         */
        formatAmount: function(amount, includeCurrency) {
            // 實現金額格式化邏輯
        }
    };
    
    // 公開 API - 折舊計算
    FA.Depreciation = {
        /**
         * 計算直線法折舊
         * @param {number} cost 成本
         * @param {number} salvageValue 殘值
         * @param {number} life 使用年限(月)
         * @param {number} usedMonths 已使用月數
         * @return {number} 當期折舊金額
         */
        calculateStraightLine: function(cost, salvageValue, life, usedMonths) {
            // 實現直線法折舊計算邏輯
        },
        
        /**
         * 計算餘額遞減法折舊
         * @param {number} bookValue 帳面價值
         * @param {number} rate 折舊率
         * @param {number} salvageValue 殘值
         * @return {number} 當期折舊金額
         */
        calculateDecliningBalance: function(bookValue, rate, salvageValue) {
            // 實現餘額遞減法折舊計算邏輯
        },
        
        /**
         * 計算年數合計法折舊
         * @param {number} cost 成本
         * @param {number} salvageValue 殘值
         * @param {number} life 使用年限(月)
         * @param {number} remainingLife 剩餘使用年限(月)
         * @return {number} 當期折舊金額
         */
        calculateSumOfYearsDigits: function(cost, salvageValue, life, remainingLife) {
            // 實現年數合計法折舊計算邏輯
        },
        
        /**
         * 產生折舊排程
         * @param {Object} assetData 資產資料
         * @return {Array} 折舊排程陣列
         */
        generateSchedule: function(assetData) {
            // 實現折舊排程邏輯
        }
    };
    
    // 公開 API - 資產評估
    FA.Valuation = {
        /**
         * 計算資產重估價值
         * @param {number} currentValue 目前價值
         * @param {number} revaluationRate 重估率
         * @return {number} 重估後價值
         */
        calculateRevaluation: function(currentValue, revaluationRate) {
            // 實現資產重估計算邏輯
        },
        
        /**
         * 計算資產減損
         * @param {number} bookValue 帳面價值
         * @param {number} recoverableAmount 可回收金額
         * @return {number} 減損金額
         */
        calculateImpairment: function(bookValue, recoverableAmount) {
            // 實現資產減損計算邏輯
        }
    };
    
    // 公開 API - 資產處分
    FA.Disposal = {
        /**
         * 計算資產處分損益
         * @param {number} bookValue 帳面價值
         * @param {number} disposalProceeds 處分收入
         * @param {number} disposalCosts 處分成本
         * @return {Object} 處分結果
         */
        calculateDisposalGainLoss: function(bookValue, disposalProceeds, disposalCosts) {
            // 實現處分損益計算邏輯
        },
        
        /**
         * 產生資產處分會計分錄
         * @param {Object} disposalData 處分資料
         * @return {Object} 會計分錄資料
         */
        generateDisposalEntries: function(disposalData) {
            // 實現處分分錄生成邏輯
        }
    };
    
    // 公開 API - 總帳整合
    FA.GLIntegration = {
        /**
         * 產生折舊會計分錄
         * @param {Object} depreciationData 折舊資料
         * @return {Object} 會計分錄資料
         */
        generateDepreciationEntries: function(depreciationData) {
            // 實現折舊分錄生成邏輯
        },
        
        /**
         * 檢查固定資產科目設定
         * @param {string} accountCode 科目代碼
         * @return {boolean} 是否為固定資產相關科目
         */
        isFixedAssetAccount: function(accountCode) {
            // 實現科目檢查邏輯
        }
    };
    
    // 公開 API - 報表功能
    FA.Reporting = {
        /**
         * 計算資產總價值
         * @param {Array} assets 資產清單
         * @return {Object} 資產價值彙總
         */
        calculateAssetsTotalValue: function(assets) {
            // 實現資產總值計算邏輯
        },
        
        /**
         * 計算折舊費用彙總
         * @param {string} periodStart 期間開始日期
         * @param {string} periodEnd 期間結束日期
         * @param {Array} assets 資產清單
         * @return {Object} 折舊費用彙總
         */
        calculateDepreciationSummary: function(periodStart, periodEnd, assets) {
            // 實現折舊彙總計算邏輯
        }
    };
    
    return FA;
})(FA || {}, jQuery);

// 頁面載入時初始化
$(document).ready(function() {
    FA.Base.init();
});

## 使用方式說明

### 初始化固定資產函數庫

```javascript
// 頁面載入後初始化固定資產函數庫
$(document).ready(function() {
    // 自訂配置選項
    FA.Base.init({
        defaultDepreciationMethod: 'DB',  // 變更預設折舊方法為餘額遞減法
        roundingPrecision: 0,             // 取整數不保留小數
        depreciationRoundingMethod: 'floor' // 折舊金額無條件捨去
    });
});
```

### 取得資產資訊

```javascript
// 取得特定資產資訊
FA.Base.getAssetInfo("A202401001")
    .then(function(assetInfo) {
        console.log("資產名稱:", assetInfo.Name);
        console.log("取得成本:", FA.Base.formatAmount(assetInfo.Cost, true));
        console.log("啟用日期:", assetInfo.AcquisitionDate);
        console.log("使用年限:", assetInfo.UsefulLife, "月");
    })
    .catch(function(error) {
        console.error("取得資產資訊失敗:", error);
    });
```

### 折舊計算

```javascript
// 直線法折舊計算
var cost = 120000;           // 成本 $120,000
var salvageValue = 12000;    // 殘值 $12,000
var lifeInMonths = 60;       // 使用年限 60 個月(5年)
var usedMonths = 10;         // 已使用 10 個月

// 計算當期折舊
var depreciation = FA.Depreciation.calculateStraightLine(
    cost, salvageValue, lifeInMonths, usedMonths
);

console.log("當期折舊金額:", FA.Base.formatAmount(depreciation, true));
// 輸出: 當期折舊金額: $1,800.00

// 產生完整折舊排程
var assetData = {
    AssetId: "A202401001",
    Cost: 120000,
    SalvageValue: 12000,
    AcquisitionDate: "2024-01-15",
    DepreciationMethod: "SL",
    UsefulLife: 60
};

var schedule = FA.Depreciation.generateSchedule(assetData);
console.log("折舊排程:", schedule);
// 返回完整的折舊排程陣列
```

### 資產評估

```javascript
// 資產重估價值計算
var currentValue = 80000;        // 目前帳面價值 $80,000
var revaluationRate = 0.05;      // 上升 5%

var newValue = FA.Valuation.calculateRevaluation(currentValue, revaluationRate);
console.log("重估後價值:", FA.Base.formatAmount(newValue, true));
// 輸出: 重估後價值: $84,000.00

// 資產減損計算
var bookValue = 75000;           // 帳面價值 $75,000
var recoverableAmount = 65000;   // 可回收金額 $65,000

var impairment = FA.Valuation.calculateImpairment(bookValue, recoverableAmount);
console.log("減損金額:", FA.Base.formatAmount(impairment, true));
// 輸出: 減損金額: $10,000.00
```

### 資產處分

```javascript
// 計算資產處分損益
var bookValue = 35000;          // 帳面價值 $35,000
var disposalProceeds = 40000;   // 處分收入 $40,000
var disposalCosts = 2000;       // 處分成本 $2,000

var result = FA.Disposal.calculateDisposalGainLoss(bookValue, disposalProceeds, disposalCosts);
console.log("處分結果:", result);
// 輸出: {gainLoss: 3000, isGain: true}

// 產生資產處分會計分錄
var disposalData = {
    AssetId: "A202205001",
    BookValue: 35000,
    DisposalProceeds: 40000,
    DisposalCosts: 2000,
    DisposalDate: "2024-06-10",
    DisposalReason: "設備更新",
    AssetAccountCode: "1621",
    AccumDepAccountCode: "1622",
    DisposalAccountCode: "4321"
};

var entries = FA.Disposal.generateDisposalEntries(disposalData);
console.log("處分分錄:", entries);
// 返回會計分錄資料物件
```

### 報表計算

```javascript
// 計算資產總價值
var assets = [
    { AssetId: "A001", Cost: 100000, AccumulatedDepreciation: 40000 },
    { AssetId: "A002", Cost: 150000, AccumulatedDepreciation: 30000 },
    { AssetId: "A003", Cost: 75000, AccumulatedDepreciation: 50000 }
];

var valuesSummary = FA.Reporting.calculateAssetsTotalValue(assets);
console.log("總成本:", FA.Base.formatAmount(valuesSummary.totalCost, true));
console.log("總折舊:", FA.Base.formatAmount(valuesSummary.totalDepreciation, true));
console.log("淨帳面價值:", FA.Base.formatAmount(valuesSummary.netBookValue, true));
// 輸出資產價值彙總資訊

// 計算折舊費用彙總
var depreciationSummary = FA.Reporting.calculateDepreciationSummary(
    "2024-01-01", "2024-03-31", assets
);
console.log("期間折舊彙總:", depreciationSummary);
// 返回按部門/類別等分組的折舊費用彙總
```

### 整合總帳

```javascript
// 產生折舊會計分錄
var depreciationData = {
    Period: "202403",
    PeriodStart: "2024-03-01",
    PeriodEnd: "2024-03-31",
    Assets: [
        { AssetId: "A001", CurrentMonthDepreciation: 1800, DepartmentCode: "D001", CategoryCode: "C001" },
        { AssetId: "A002", CurrentMonthDepreciation: 2500, DepartmentCode: "D002", CategoryCode: "C002" }
    ],
    DepreciationExpenseAccounts: {
        "C001": "5611",
        "C002": "5612"
    },
    AccumulatedDepreciationAccount: "1622"
};

var journalEntries = FA.GLIntegration.generateDepreciationEntries(depreciationData);
console.log("折舊分錄:", journalEntries);
// 返回可提交給總帳系統的分錄資料

// 檢查科目是否為固定資產相關科目
var isAssetAccount = FA.GLIntegration.isFixedAssetAccount("1621");
console.log("是否為固定資產科目:", isAssetAccount);
// 輸出: true
``` 

## 相依關係

FASysFunc.js 的相依關係：

1. **直接相依**：
   - **jQuery**：依賴 jQuery 進行 DOM 操作和 AJAX 請求
   - **pagefunction.js**：依賴系統共用的頁面函數
   - **Busy.js**：使用忙碌指示器展示處理狀態
   - **moment.js**：用於日期處理和計算

2. **間接相依**：
   - **Bootstrap**：與 Bootstrap 組件相容但不強制依賴
   - **ExcelManger**：與 Excel 匯出功能協同工作
   - **WSAutoComplete**：使用自動完成服務進行資產項目搜尋
   - **GLA0110TManager**：與總帳交易管理系統整合

3. **被以下元件使用**：
   - **PTA0210**：固定資產主檔維護頁面
   - **PTA0220**：折舊處理頁面
   - **PTA0230**：資產報廢處理頁面
   - **FAR0150**：固定資產報表頁面
   - **FAR0160**：折舊費用報表頁面
   - **FAR0170**：資產明細帳頁面
   - **所有處理固定資產相關功能的頁面和控制項**

## 主要函數說明

### 基本功能函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.Base.init()` | options: 配置選項 | 無 | 初始化固定資產函數庫 |
| `FA.Base.getAssetInfo()` | assetId: 資產編號 | Promise | 非同步獲取資產資訊 |
| `FA.Base.formatAmount()` | amount: 金額<br>includeCurrency: 包含貨幣符號 | string | 格式化金額顯示 |
| `FA.Base.parseDate()` | dateString: 日期字串 | Date | 解析日期字串為日期物件 |

### 折舊計算函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.Depreciation.calculateStraightLine()` | cost: 成本<br>salvageValue: 殘值<br>life: , usedMonths: 已使用月數 | number | 計算直線法折舊 |
| `FA.Depreciation.calculateDecliningBalance()` | bookValue: 帳面價值<br>rate: 折舊率<br>salvageValue: 殘值 | number | 計算餘額遞減法折舊 |
| `FA.Depreciation.calculateSumOfYearsDigits()` | cost: 成本<br>salvageValue: 殘值<br>life: 使用年限<br>remainingLife: 剩餘年限 | number | 計算年數合計法折舊 |
| `FA.Depreciation.generateSchedule()` | assetData: 資產資料 | Array | 產生折舊排程 |

### 資產評估函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.Valuation.calculateRevaluation()` | currentValue: 目前價值<br>revaluationRate: 重估率 | number | 計算資產重估價值 |
| `FA.Valuation.calculateImpairment()` | bookValue: 帳面價值<br>recoverableAmount: 可回收金額 | number | 計算資產減損 |

### 資產處分函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.Disposal.calculateDisposalGainLoss()` | bookValue: 帳面價值<br>disposalProceeds: 處分收入<br>disposalCosts: 處分成本 | Object | 計算資產處分損益 |
| `FA.Disposal.generateDisposalEntries()` | disposalData: 處分資料 | Object | 產生資產處分會計分錄 |

### 總帳整合函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.GLIntegration.generateDepreciationEntries()` | depreciationData: 折舊資料 | Object | 產生折舊會計分錄 |
| `FA.GLIntegration.isFixedAssetAccount()` | accountCode: 科目代碼 | boolean | 檢查是否為固定資產相關科目 |

### 報表功能函數

| 函數名稱 | 參數 | 返回值 | 說明 |
|---------|------|-------|------|
| `FA.Reporting.calculateAssetsTotalValue()` | assets: 資產清單 | Object | 計算資產總價值 |
| `FA.Reporting.calculateDepreciationSummary()` | periodStart: 期間開始日期<br>periodEnd: 期間結束日期<br>assets: 資產清單 | Object | 計算折舊費用彙總 |

## 使用案例

### 固定資產月末折舊處理

```javascript
// 於PTA0220.aspx（折舊處理）中使用
function processMonthlyDepreciation() {
    // 收集處理參數
    var processingParams = {
        year: $("#YearList").val(),
        period: $("#PeriodList").val(),
        companyId: $("#CompanyList").val(),
        departmentFilter: $("#DepartmentFilter").val() || null,
        categoryFilter: $("#CategoryFilter").val() || null
    };
    
    // 顯示處理中
    Busy.show("處理折舊計算中，請稍候...");
    
    // 取得期間起訖日期
    var periodStartDate = getPeriodStartDate(processingParams.year, processingParams.period);
    var periodEndDate = getPeriodEndDate(processingParams.year, processingParams.period);
    
    // 取得需要折舊的資產清單
    $.ajax({
        type: "POST",
        url: "PTA0220.aspx/GetDepreciableAssets",
        data: JSON.stringify({ parameters: processingParams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            var assets = response.d;
            
            if (assets.length === 0) {
                Busy.hide();
                showWarning("沒有需要折舊的資產");
                return;
            }
            
            // 計算每個資產的折舊
            var depreciationResults = [];
            var depreciationData = {
                Period: processingParams.year + processingParams.period,
                PeriodStart: periodStartDate,
                PeriodEnd: periodEndDate,
                Assets: [],
                DepreciationExpenseAccounts: {},
                AccumulatedDepreciationAccount: "1622"  // 累計折舊科目
            };
            
            // 處理每個資產的折舊
            assets.forEach(function(asset) {
                var depreciation = 0;
                
                // 根據折舊方法計算
                switch (asset.DepreciationMethod) {
                    case "SL":  // 直線法
                        depreciation = FA.Depreciation.calculateStraightLine(
                            asset.Cost, asset.SalvageValue, asset.UsefulLife, asset.UsedMonths
                        );
                        break;
                    case "DB":  // 餘額遞減法
                        depreciation = FA.Depreciation.calculateDecliningBalance(
                            asset.BookValue, asset.DepreciationRate, asset.SalvageValue
                        );
                        break;
                    case "SYD":  // 年數合計法
                        depreciation = FA.Depreciation.calculateSumOfYearsDigits(
                            asset.Cost, asset.SalvageValue, asset.UsefulLife, asset.RemainingLife
                        );
                        break;
                }
                
                // 儲存資產折舊結果
                depreciationResults.push({
                    AssetId: asset.AssetId,
                    AssetName: asset.AssetName,
                    Cost: asset.Cost,
                    BookValue: asset.BookValue,
                    DepreciationAmount: depreciation,
                    NewBookValue: asset.BookValue - depreciation
                });
                
                // 添加至分錄生成資料
                depreciationData.Assets.push({
                    AssetId: asset.AssetId,
                    CurrentMonthDepreciation: depreciation,
                    DepartmentCode: asset.DepartmentCode,
                    CategoryCode: asset.CategoryCode
                });
                
                // 設置對應的費用科目
                if (!depreciationData.DepreciationExpenseAccounts[asset.CategoryCode]) {
                    depreciationData.DepreciationExpenseAccounts[asset.CategoryCode] = 
                        getDepreciationExpenseAccount(asset.CategoryCode);
                }
            });
            
            // 顯示折舊結果
            displayDepreciationResults(depreciationResults);
            
            // 詢問使用者是否要生成分錄
            if (confirm("已完成折舊計算，是否要產生折舊分錄？")) {
                // 生成折舊分錄
                var journalEntries = FA.GLIntegration.generateDepreciationEntries(depreciationData);
                
                // 儲存分錄至總帳
                $.ajax({
                    type: "POST",
                    url: "PTA0220.aspx/SaveDepreciationEntries",
                    data: JSON.stringify({ entries: journalEntries }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(saveResponse) {
                        showSuccess("折舊分錄已成功儲存，傳票號碼：" + saveResponse.d);
                    },
                    error: function(xhr, status, error) {
                        showError("儲存折舊分錄失敗: " + error);
                    }
                });
            }
        },
        error: function(xhr, status, error) {
            showError("折舊處理失敗: " + error);
        },
        complete: function() {
            Busy.hide();
        }
    });
}
```

### 固定資產報表

```javascript
// 於FAR0150.aspx（固定資產報表）中使用
function generateFixedAssetsReport() {
    // 獲取報表參數
    var reportParams = {
        asOfDate: $("#AsOfDate").val(),
        companyId: $("#CompanyList").val(),
        departmentFilter: $("#DepartmentFilter").val() || null,
        categoryFilter: $("#CategoryFilter").val() || null,
        includeFullyDepreciated: $("#IncludeFullyDepreciated").is(":checked")
    };
    
    // 顯示處理中
    Busy.show("產生固定資產報表中...");
    
    // 取得報表資料
    $.ajax({
        type: "POST",
        url: "FAR0150.aspx/GetReportData",
        data: JSON.stringify({ parameters: reportParams }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(response) {
            var reportData = response.d;
            
            // 計算報表彙總
            var valuesSummary = FA.Reporting.calculateAssetsTotalValue(reportData.Assets);
            
            // 顯示報表
            displayFixedAssetsReport(reportData, valuesSummary);
        },
        error: function(xhr, status, error) {
            showError("報表產生失敗: " + error);
        },
        complete: function() {
            Busy.hide();
        }
    });
}

function displayFixedAssetsReport(reportData, valuesSummary) {
    var table = $("#tblFixedAssets");
    table.empty();
    
    // 產生表頭
    var thead = $("<thead></thead>");
    var headerRow = $("<tr></tr>");
    headerRow.append("<th>資產編號</th>");
    headerRow.append("<th>資產名稱</th>");
    headerRow.append("<th>類別</th>");
    headerRow.append("<th>部門</th>");
    headerRow.append("<th>啟用日期</th>");
    headerRow.append("<th>取得成本</th>");
    headerRow.append("<th>累計折舊</th>");
    headerRow.append("<th>帳面價值</th>");
    headerRow.append("<th>使用年限(月)</th>");
    headerRow.append("<th>已使用(月)</th>");
    thead.append(headerRow);
    table.append(thead);
    
    // 產生資產明細資料
    var tbody = $("<tbody></tbody>");
    var currentCategory = null;
    var categoryTotal = {
        cost: 0,
        accumulatedDepreciation: 0,
        bookValue: 0
    };
    
    $.each(reportData.Assets, function(index, asset) {
        // 如果是新類別，新增類別小計行
        if (currentCategory !== null && currentCategory !== asset.CategoryCode) {
            // 新增小計行
            var subtotalRow = $("<tr class='category-subtotal'></tr>");
            subtotalRow.append("<td colspan='5'><strong>" + currentCategory + " 小計</strong></td>");
            subtotalRow.append("<td class='text-right'><strong>" + 
                FA.Base.formatAmount(categoryTotal.cost, false) + "</strong></td>");
            subtotalRow.append("<td class='text-right'><strong>" + 
                FA.Base.formatAmount(categoryTotal.accumulatedDepreciation, false) + "</strong></td>");
            subtotalRow.append("<td class='text-right'><strong>" + 
                FA.Base.formatAmount(categoryTotal.bookValue, false) + "</strong></td>");
            subtotalRow.append("<td colspan='2'></td>");
            tbody.append(subtotalRow);
            
            // 重置類別小計
            categoryTotal = {
                cost: 0,
                accumulatedDepreciation: 0,
                bookValue: 0
            };
        }
        
        // 更新當前類別
        currentCategory = asset.CategoryCode;
        
        // 累加類別小計
        categoryTotal.cost += asset.Cost;
        categoryTotal.accumulatedDepreciation += asset.AccumulatedDepreciation;
        categoryTotal.bookValue += (asset.Cost - asset.AccumulatedDepreciation);
        
        // 新增資產明細行
        var row = $("<tr></tr>");
        row.append("<td>" + asset.AssetId + "</td>");
        row.append("<td>" + asset.AssetName + "</td>");
        row.append("<td>" + asset.CategoryName + "</td>");
        row.append("<td>" + asset.DepartmentName + "</td>");
        row.append("<td>" + asset.AcquisitionDate + "</td>");
        row.append("<td class='text-right'>" + FA.Base.formatAmount(asset.Cost, false) + "</td>");
        row.append("<td class='text-right'>" + FA.Base.formatAmount(asset.AccumulatedDepreciation, false) + "</td>");
        row.append("<td class='text-right'>" + FA.Base.formatAmount(asset.Cost - asset.AccumulatedDepreciation, false) + "</td>");
        row.append("<td class='text-right'>" + asset.UsefulLife + "</td>");
        row.append("<td class='text-right'>" + asset.UsedMonths + "</td>");
        tbody.append(row);
    });
    
    // 新增最後一個類別的小計行
    if (currentCategory !== null) {
        var subtotalRow = $("<tr class='category-subtotal'></tr>");
        subtotalRow.append("<td colspan='5'><strong>" + currentCategory + " 小計</strong></td>");
        subtotalRow.append("<td class='text-right'><strong>" + 
            FA.Base.formatAmount(categoryTotal.cost, false) + "</strong></td>");
        subtotalRow.append("<td class='text-right'><strong>" + 
            FA.Base.formatAmount(categoryTotal.accumulatedDepreciation, false) + "</strong></td>");
        subtotalRow.append("<td class='text-right'><strong>" + 
            FA.Base.formatAmount(categoryTotal.bookValue, false) + "</strong></td>");
        subtotalRow.append("<td colspan='2'></td>");
        tbody.append(subtotalRow);
    }
    
    table.append(tbody);
    
    // 產生報表總計
    var tfoot = $("<tfoot></tfoot>");
    var footerRow = $("<tr class='report-total'></tr>");
    footerRow.append("<td colspan='5'><strong>報表總計</strong></td>");
    footerRow.append("<td class='text-right'><strong>" + 
        FA.Base.formatAmount(valuesSummary.totalCost, false) + "</strong></td>");
    footerRow.append("<td class='text-right'><strong>" + 
        FA.Base.formatAmount(valuesSummary.totalDepreciation, false) + "</strong></td>");
    footerRow.append("<td class='text-right'><strong>" + 
        FA.Base.formatAmount(valuesSummary.netBookValue, false) + "</strong></td>");
    footerRow.append("<td colspan='2'></td>");
    tfoot.append(footerRow);
    table.append(tfoot);
    
    // 顯示報表摘要
    $("#reportSummary").html(
        "<p>報表日期: <strong>" + reportParams.asOfDate + "</strong></p>" +
        "<p>總資產數: <strong>" + reportData.Assets.length + "</strong></p>" +
        "<p>資產總成本: <strong>" + FA.Base.formatAmount(valuesSummary.totalCost, true) + "</strong></p>" +
        "<p>淨帳面價值: <strong>" + FA.Base.formatAmount(valuesSummary.netBookValue, true) + "</strong></p>"
    );
}
```

## 維護記錄

| 版本  | 日期       | 修改者     | 變更內容                         |
|------ |------------|------------|----------------------------------|
| 1.0.0 | 2024/01/15 | 系統開發部 | 初始版本                         |
| 1.0.1 | 2024/02/05 | 系統開發部 | 修正折舊計算邏輯                 |
| 1.1.0 | 2024/04/12 | 系統開發部 | 新增資產處分功能                 |
| 1.1.1 | 2024/06/23 | 系統開發部 | 修正資產評估計算                 |
| 1.2.0 | 2024/09/15 | 系統開發部 | 新增更多折舊方法                 |
| 1.2.1 | 2025/01/08 | 系統開發部 | 效能優化和程式碼重構             |

## 待改進事項

1. **功能擴展**：
   - 增加更多折舊方法，如單位產量法和複合折舊法
   - 增加批次處理功能，提高大量資產處理效能
   - 實作資產元件化管理，支援複合資產結構

2. **使用者體驗**：
   - 提供折舊視覺化圖表
   - 改進資產成本計算助手
   - 增加互動式資產生命週期視圖

3. **技術改進**：
   - 重構為 ES6 模組格式
   - 提高精確計算效能
   - 增強與現代前端框架的整合

4. **整合改進**：
   - 加強與採購模組的整合
   - 實現與條碼掃描系統的整合
   - 改進與預算系統的連接

5. **安全性增強**：
   - 增加資產處理的審計追蹤功能
   - 強化敏感操作的權限控制
   - 提供資產異動通知機制 