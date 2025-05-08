# GLA0110TManager 交易管理規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | GLA0110TManager                       |
| 程式名稱     | 交易管理                               |
| 檔案大小     | 25KB                                 |
| 行數        | ~890                                 |
| 功能簡述     | 提供交易處理功能                        |
| 複雜度       | 高                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/20                           |

## 程式功能概述

GLA0110TManager 是泛太總帳系統中的核心交易管理元件，負責處理系統中所有的會計交易資料操作。此元件提供完整的交易生命週期管理，包括交易建立、修改、查詢、審核和過帳等功能。GLA0110TManager 的主要功能包括：

1. 提供傳票資料的新增、修改、刪除和查詢功能
2. 管理交易明細資料的處理和驗證
3. 實現交易審核和核准流程
4. 處理交易過帳和反過帳操作
5. 提供批次交易處理功能
6. 計算和更新會計科目餘額
7. 實現會計期間結轉功能
8. 處理特殊交易類型（例如調整分錄）
9. 支援多幣別交易處理和匯率轉換
10. 提供交易資料的稽核追蹤

此元件作為資料存取層和業務邏輯層的橋樑，封裝了複雜的交易處理邏輯，確保資料的一致性和完整性。

## 類別結構說明

GLA0110TManager 是一個複雜的交易管理類別，包含多個相關的子類別和結構：

1. **主類別**：包含交易處理的核心功能和屬性
2. **交易明細類**：處理交易明細項目的資料結構和操作
3. **交易狀態類**：管理交易狀態的轉換和驗證
4. **交易驗證類**：提供交易資料的驗證規則和方法
5. **餘額計算類**：處理科目餘額的計算和更新

整體架構採用分層設計，清晰分離不同的功能職責，使系統易於維護和擴展。

## 技術實現

GLA0110TManager 基於以下技術實現：

1. **C# 程式語言**：使用 C# 實現所有功能
2. **ADO.NET**：使用 ADO.NET 進行資料庫操作
3. **交易控制模式**：使用資料庫交易確保資料完整性
4. **事件驅動模式**：提供交易生命週期的各種事件
5. **訪問者模式**：實現不同類型交易的處理策略
6. **單例模式**：確保交易管理器的全局唯一性
7. **工廠模式**：創建不同類型的交易物件

## 相依類別和元件

GLA0110TManager 依賴以下類別與元件：

1. **DBManger**：資料庫操作管理類別
2. **CompanyManager**：公司資料管理類別
3. **Page_BaseClass**：頁面基底類別（用於頁面交互）
4. **LoginClass**：使用者登入及權限控制
5. **SubjectManager**：科目管理類別
6. **PeriodManager**：會計期間管理類別
7. **System.Data**：.NET Framework 資料處理命名空間
8. **System.Transactions**：.NET Framework 交易處理命名空間

## 屬性說明

GLA0110TManager 提供以下主要公開屬性：

| 屬性名稱 | 資料類型 | 說明 | 存取權限 |
|---------|---------|------|---------|
| ErrorMessage | string | 取得最後的錯誤訊息 | 公開 |
| HasError | bool | 表示是否有錯誤發生 | 公開 |
| CurrentCompany | string | 取得或設定目前的公司代碼 | 公開 |
| CurrentUser | string | 取得或設定目前的使用者 | 公開 |
| TransactionStatus | TransStatusEnum | 取得目前交易的狀態 | 公開 |
| TransactionDate | DateTime | 取得或設定交易日期 | 公開 |
| TransactionNumber | string | 取得交易編號 | 公開 |
| IsBalanced | bool | 表示交易是否平衡 | 公開 |
| TotalDebit | decimal | 取得借方總金額 | 公開 |
| TotalCredit | decimal | 取得貸方總金額 | 公開 |
| ItemCount | int | 取得交易明細項目數 | 公開 |

## 私有成員變數

GLA0110TManager 包含以下重要的私有成員變數：

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _errMsg | string | 儲存錯誤訊息 |
| _hasError | bool | 標記是否有錯誤 |
| _company | string | 儲存公司代碼 |
| _user | string | 儲存使用者ID |
| _db | DBManger | 資料庫管理器實例 |
| _transItems | List<TransactionItem> | 交易明細項目集合 |
| _transStatus | TransStatusEnum | 交易狀態 |
| _transNumber | string | 交易編號 |
| _transDate | DateTime | 交易日期 |
| _periodCode | string | 會計期間代碼 |
| _description | string | 交易描述 |

## 方法說明

### 建構函式

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| GLA0110TManager | 無 | 無 | 預設建構函式，使用預設資料庫連接 |
| GLA0110TManager | string company, string user | 無 | 指定公司和使用者ID的建構函式 |
| GLA0110TManager | DBManger.ConnectionString dbenum | 無 | 指定資料庫連接字串的建構函式 |

### 交易處理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| CreateTransaction | string description, DateTime transDate | bool | 建立新交易 |
| LoadTransaction | string transNumber | bool | 載入指定編號的交易 |
| SaveTransaction | 無 | bool | 儲存目前的交易 |
| DeleteTransaction | string transNumber | bool | 刪除指定編號的交易 |
| PostTransaction | string transNumber | bool | 過帳指定編號的交易 |
| ReverseTransaction | string transNumber | bool | 反轉指定編號的交易 |
| ApproveTransaction | string transNumber | bool | 核准指定編號的交易 |
| RejectTransaction | string transNumber, string reason | bool | 拒絕指定編號的交易 |

### 交易明細方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| AddTransactionItem | TransactionItem item | bool | 新增交易明細項目 |
| UpdateTransactionItem | int index, TransactionItem item | bool | 更新指定索引的交易明細項目 |
| RemoveTransactionItem | int index | bool | 移除指定索引的交易明細項目 |
| GetTransactionItems | 無 | List<TransactionItem> | 取得所有交易明細項目 |
| ValidateTransaction | 無 | bool | 驗證目前交易的有效性 |
| CalculateBalance | 無 | void | 計算交易的借貸平衡 |

### 查詢方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| GetTransactionByNumber | string transNumber | DataTable | 取得指定編號的交易資料 |
| GetTransactionsByDate | DateTime startDate, DateTime endDate | DataTable | 取得指定日期範圍的交易資料 |
| GetTransactionsByPeriod | string periodCode | DataTable | 取得指定會計期間的交易資料 |
| GetTransactionsByStatus | TransStatusEnum status | DataTable | 取得指定狀態的交易資料 |
| GetTransactionsBySubject | string subjectCode | DataTable | 取得指定科目的交易資料 |

## 程式碼說明

### 交易建立

```csharp
public bool CreateTransaction(string description, DateTime transDate)
{
    try
    {
        // 初始化交易資訊
        _transNumber = GenerateTransactionNumber();
        _transDate = transDate;
        _description = description;
        _transStatus = TransStatusEnum.Draft;
        _periodCode = GetPeriodCodeFromDate(transDate);
        
        // 驗證日期和期間
        if (!ValidateTransactionDate())
        {
            return false;
        }
        
        // 執行資料庫操作
        string sql = "INSERT INTO GLA0110T (TransNumber, TransDate, Description, Status, CreatedBy, CreatedDate, Company, PeriodCode) " +
                    "VALUES (@TransNumber, @TransDate, @Description, @Status, @CreatedBy, GETDATE(), @Company, @PeriodCode)";
        
        _db.AddParameter("@TransNumber", _transNumber);
        _db.AddParameter("@TransDate", _transDate);
        _db.AddParameter("@Description", _description);
        _db.AddParameter("@Status", (int)_transStatus);
        _db.AddParameter("@CreatedBy", _user);
        _db.AddParameter("@Company", _company);
        _db.AddParameter("@PeriodCode", _periodCode);
        
        int result = _db.ExecuteNonQuery(sql);
        _hasError = (result <= 0);
        
        return !_hasError;
    }
    catch (Exception ex)
    {
        _errMsg = ex.Message;
        _hasError = true;
        return false;
    }
}
```

### 交易明細新增

```csharp
public bool AddTransactionItem(TransactionItem item)
{
    try
    {
        // 驗證項目
        if (!ValidateTransactionItem(item))
        {
            return false;
        }
        
        // 執行資料庫操作
        string sql = "INSERT INTO GLA0110TD (TransNumber, LineNumber, SubjectCode, DeptCode, DrAmount, CrAmount, Description, Company) " +
                    "VALUES (@TransNumber, @LineNumber, @SubjectCode, @DeptCode, @DrAmount, @CrAmount, @Description, @Company)";
        
        _db.AddParameter("@TransNumber", _transNumber);
        _db.AddParameter("@LineNumber", GetNextLineNumber());
        _db.AddParameter("@SubjectCode", item.SubjectCode);
        _db.AddParameter("@DeptCode", item.DeptCode);
        _db.AddParameter("@DrAmount", item.DrAmount);
        _db.AddParameter("@CrAmount", item.CrAmount);
        _db.AddParameter("@Description", item.Description);
        _db.AddParameter("@Company", _company);
        
        int result = _db.ExecuteNonQuery(sql);
        if (result > 0)
        {
            _transItems.Add(item);
            CalculateBalance();
        }
        
        _hasError = (result <= 0);
        return !_hasError;
    }
    catch (Exception ex)
    {
        _errMsg = ex.Message;
        _hasError = true;
        return false;
    }
}
```

### 交易過帳

```csharp
public bool PostTransaction(string transNumber)
{
    try
    {
        // 載入交易
        if (!LoadTransaction(transNumber))
        {
            return false;
        }
        
        // 驗證交易狀態
        if (_transStatus != TransStatusEnum.Approved)
        {
            _errMsg = "只有已核准的交易可以進行過帳";
            _hasError = true;
            return false;
        }
        
        // 驗證交易平衡
        if (!IsBalanced)
        {
            _errMsg = "交易借貸不平衡，無法過帳";
            _hasError = true;
            return false;
        }
        
        // 開始資料庫交易
        _db.BeginTransaction();
        
        try
        {
            // 更新交易狀態
            string sql = "UPDATE GLA0110T SET Status = @Status, PostedBy = @PostedBy, PostedDate = GETDATE() WHERE TransNumber = @TransNumber AND Company = @Company";
            
            _db.AddParameter("@Status", (int)TransStatusEnum.Posted);
            _db.AddParameter("@PostedBy", _user);
            _db.AddParameter("@TransNumber", _transNumber);
            _db.AddParameter("@Company", _company);
            
            int result = _db.ExecuteNonQuery(sql);
            
            if (result <= 0)
            {
                _db.RollbackTransaction();
                _errMsg = "更新交易狀態失敗";
                _hasError = true;
                return false;
            }
            
            // 更新科目餘額
            if (!UpdateSubjectBalances())
            {
                _db.RollbackTransaction();
                return false;
            }
            
            // 提交交易
            _db.CommitTransaction();
            _transStatus = TransStatusEnum.Posted;
            
            return true;
        }
        catch (Exception ex)
        {
            _db.RollbackTransaction();
            _errMsg = "過帳處理失敗: " + ex.Message;
            _hasError = true;
            return false;
        }
    }
    catch (Exception ex)
    {
        _errMsg = ex.Message;
        _hasError = true;
        return false;
    }
}
```

## 交易狀態列舉

```csharp
public enum TransStatusEnum
{
    Draft = 0,       // 草稿
    Submitted = 1,   // 已提交
    Approved = 2,    // 已核准
    Rejected = 3,    // 已拒絕
    Posted = 4,      // 已過帳
    Reversed = 5     // 已反轉
}
```

## 交易明細類別

```csharp
public class TransactionItem
{
    public string SubjectCode { get; set; }
    public string DeptCode { get; set; }
    public decimal DrAmount { get; set; }
    public decimal CrAmount { get; set; }
    public string Description { get; set; }
    public int LineNumber { get; set; }
    
    public TransactionItem()
    {
        DrAmount = 0;
        CrAmount = 0;
    }
}
```

## 使用範例

以下是 GLA0110TManager 的使用範例：

### 建立新交易

```csharp
// 建立交易管理器實例
GLA0110TManager transMgr = new GLA0110TManager("COMPANY1", "USER001");

// 建立新交易
if (transMgr.CreateTransaction("5月月結調整分錄", DateTime.Now))
{
    // 新增交易明細
    TransactionItem item1 = new TransactionItem
    {
        SubjectCode = "1001",
        DeptCode = "100",
        DrAmount = 10000,
        CrAmount = 0,
        Description = "現金增加"
    };
    
    TransactionItem item2 = new TransactionItem
    {
        SubjectCode = "4001",
        DeptCode = "100",
        DrAmount = 0,
        CrAmount = 10000,
        Description = "銷貨收入"
    };
    
    transMgr.AddTransactionItem(item1);
    transMgr.AddTransactionItem(item2);
    
    // 儲存交易
    if (transMgr.SaveTransaction())
    {
        string transNumber = transMgr.TransactionNumber;
        // 交易建立成功
    }
    else
    {
        string errorMsg = transMgr.ErrorMessage;
        // 處理錯誤
    }
}
```

### 載入和過帳交易

```csharp
// 建立交易管理器實例
GLA0110TManager transMgr = new GLA0110TManager("COMPANY1", "USER001");

// 載入交易
if (transMgr.LoadTransaction("T202505001"))
{
    // 檢查交易狀態
    if (transMgr.TransactionStatus == GLA0110TManager.TransStatusEnum.Approved)
    {
        // 過帳交易
        if (transMgr.PostTransaction("T202505001"))
        {
            // 過帳成功
        }
        else
        {
            string errorMsg = transMgr.ErrorMessage;
            // 處理錯誤
        }
    }
}
```

## 資料結構

### 資料庫表格

GLA0110TManager 使用以下資料庫表格：

1. **GLA0110T**：交易主表
2. **GLA0110TD**：交易明細表
3. **GLSubjectMaster**：科目主檔
4. **GLDeptMaster**：部門主檔
5. **GLSubjectBalance**：科目餘額表

### GLA0110T 表結構

| 欄位名稱 | 資料類型 | 說明 |
|---------|---------|------|
| TransNumber | VARCHAR(20) | 交易編號 (主鍵) |
| Company | VARCHAR(10) | 公司代碼 (主鍵) |
| TransDate | DATETIME | 交易日期 |
| PeriodCode | VARCHAR(10) | 會計期間代碼 |
| Description | VARCHAR(200) | 交易描述 |
| Status | INT | 交易狀態 |
| CreatedBy | VARCHAR(20) | 建立者 |
| CreatedDate | DATETIME | 建立日期 |
| ApprovedBy | VARCHAR(20) | 核准者 |
| ApprovedDate | DATETIME | 核准日期 |
| PostedBy | VARCHAR(20) | 過帳者 |
| PostedDate | DATETIME | 過帳日期 |
| ReversedBy | VARCHAR(20) | 反轉者 |
| ReversedDate | DATETIME | 反轉日期 |
| ReversedTransNumber | VARCHAR(20) | 反轉交易編號 |

### GLA0110TD 表結構

| 欄位名稱 | 資料類型 | 說明 |
|---------|---------|------|
| TransNumber | VARCHAR(20) | 交易編號 (主鍵) |
| LineNumber | INT | 明細行號 (主鍵) |
| Company | VARCHAR(10) | 公司代碼 (主鍵) |
| SubjectCode | VARCHAR(20) | 科目代碼 |
| DeptCode | VARCHAR(10) | 部門代碼 |
| DrAmount | DECIMAL(18,2) | 借方金額 |
| CrAmount | DECIMAL(18,2) | 貸方金額 |
| Description | VARCHAR(200) | 明細描述 |

## 異常處理

GLA0110TManager 實現了完整的異常處理機制：

1. 使用 try-catch 捕獲可能的異常
2. 設置錯誤標記和錯誤訊息
3. 使用資料庫交易確保資料完整性
4. 提供錯誤回復機制
5. 記錄詳細錯誤日誌

## 注意事項與限制

1. **交易鎖定**：已過帳的交易無法修改，必須通過反轉功能處理
2. **資料驗證**：交易明細必須符合會計原則，借貸必須平衡
3. **會計期間**：交易日期必須在開放的會計期間內
4. **執行權限**：不同操作需要相應的系統權限
5. **批次處理**：大量交易處理可能影響系統效能

## 效能考量

1. **批次處理**：使用批次方式處理大量交易
2. **索引優化**：資料表設有適當索引加速查詢
3. **交易隔離**：使用適當的交易隔離級別
4. **連接池**：利用資料庫連接池提升效能
5. **查詢優化**：優化複雜查詢，減少資料傳輸量

## 安全性考量

1. **授權控制**：嚴格控制不同操作的授權
2. **分工機制**：實現交易建立、審核、過帳的分工
3. **稽核追蹤**：記錄所有交易的操作歷史
4. **參數化查詢**：使用參數化 SQL 查詢防止注入攻擊
5. **資料加密**：敏感資料加密存儲

## 待改進事項

1. **並行處理**：增強並行交易處理能力
2. **擴展性**：增加對不同交易類型的支援
3. **效能優化**：優化大量資料的處理能力
4. **錯誤處理**：提供更詳細的錯誤診斷資訊
5. **跨幣別處理**：強化多幣別交易的處理
6. **API整合**：提供標準 API 介面，便於系統整合

## 相關檔案

1. **DBManger.cs**：資料庫操作管理類別
2. **CompanyManager.cs**：公司資料管理類別
3. **SubjectManager.cs**：科目管理類別
4. **PeriodManager.cs**：會計期間管理類別

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/20 | 1.0    | 首次建立交易管理規格書          | Claude AI  | 