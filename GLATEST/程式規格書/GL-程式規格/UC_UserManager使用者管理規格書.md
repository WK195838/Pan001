# UC_UserManager 使用者管理規格書

## 基本資訊

| 項目       | 內容                                    |
|------------|---------------------------------------|
| 程式代號     | UC_UserManager                        |
| 程式名稱     | 使用者管理                             |
| 檔案大小     | 13KB                                 |
| 行數        | ~485                                 |
| 功能簡述     | 提供使用者管理功能                      |
| 複雜度       | 高                                   |
| 規格書狀態   | 已完成                                |
| 負責人員     | Claude AI                           |
| 完成日期     | 2025/5/21                           |

## 程式功能概述

UC_UserManager 是泛太總帳系統中的使用者管理元件，負責處理系統中所有與使用者相關的資料操作和管理功能。此元件提供完整的使用者生命週期管理，包括使用者帳號的建立、修改、刪除、查詢以及權限設定等功能。UC_UserManager 的主要功能包括：

1. 提供使用者資料的新增、修改、刪除和查詢功能
2. 管理使用者的密碼設定和重設
3. 處理使用者與角色的關聯設定
4. 提供使用者權限的查詢和驗證
5. 實現使用者狀態的管理（啟用/停用）
6. 處理使用者登入相關的驗證和記錄
7. 支援多層級的使用者組織結構
8. 提供使用者資料的匯入和匯出功能
9. 實現與 Active Directory 的整合
10. 維護使用者操作的稽核記錄

此元件作為系統安全和使用者管理的核心，確保適當的身份驗證和授權機制，同時提供友好的管理介面。

## 類別結構說明

UC_UserManager 是一個複雜的使用者管理類別，包含多個相關的子類別和結構：

1. **主類別**：包含使用者管理的核心功能和屬性
2. **使用者資料類**：定義使用者資料的結構和屬性
3. **權限管理類**：處理使用者權限的檢查和管理
4. **角色管理類**：管理角色定義和角色關聯
5. **密碼處理類**：提供密碼加密和驗證功能

整體架構採用分層設計，清晰分離不同的功能職責，使系統易於維護和擴展。

## 技術實現

UC_UserManager 基於以下技術實現：

1. **C# 程式語言**：使用 C# 實現所有功能
2. **ADO.NET**：使用 ADO.NET 進行資料庫操作
3. **ASP.NET**：與 ASP.NET Web 頁面整合
4. **加密演算法**：使用 SHA-256 或 PBKDF2 進行密碼加密
5. **工廠模式**：創建不同類型的使用者物件
6. **代理模式**：控制對使用者物件的存取
7. **觀察者模式**：實現使用者狀態變更的通知機制

## 相依類別和元件

UC_UserManager 依賴以下類別與元件：

1. **DBManger**：資料庫操作管理類別
2. **AppAuthority**：應用程式權限管理類別
3. **LoginClass**：使用者登入功能類別
4. **Page_BaseClass**：頁面基底類別
5. **System.DirectoryServices**：Active Directory 整合
6. **System.Security.Cryptography**：密碼加密處理
7. **System.Web.Security**：Web 安全功能
8. **System.Data**：.NET Framework 資料處理命名空間

## 屬性說明

UC_UserManager 提供以下主要公開屬性：

| 屬性名稱 | 資料類型 | 說明 | 存取權限 |
|---------|---------|------|---------|
| ErrorMessage | string | 取得最後的錯誤訊息 | 公開 |
| HasError | bool | 表示是否有錯誤發生 | 公開 |
| CurrentUserID | string | 取得或設定目前操作的使用者ID | 公開 |
| IsAuthenticated | bool | 表示目前使用者是否已通過認證 | 公開 |
| IsAdmin | bool | 表示目前使用者是否為系統管理員 | 公開 |
| UserCount | int | 取得系統中的使用者數量 | 公開 |
| UserStatus | UserStatusEnum | 取得目前使用者的狀態 | 公開 |
| ADEnabled | bool | 表示是否啟用 Active Directory 整合 | 公開 |
| PasswordExpiryDays | int | 取得或設定密碼過期天數 | 公開 |
| LoginAttempts | int | 取得登入嘗試次數 | 公開 |

## 私有成員變數

UC_UserManager 包含以下重要的私有成員變數：

| 變數名稱 | 資料類型 | 說明 |
|---------|---------|------|
| _errMsg | string | 儲存錯誤訊息 |
| _hasError | bool | 標記是否有錯誤 |
| _userID | string | 儲存目前操作的使用者ID |
| _db | DBManger | 資料庫管理器實例 |
| _userCache | Dictionary<string, UserInfo> | 使用者資料快取 |
| _pwdEncryptor | PasswordEncryptor | 密碼加密處理器 |
| _adHelper | ADHelper | Active Directory 輔助類別 |
| _roleManager | RoleManager | 角色管理器 |
| _authManager | AuthorizationManager | 授權管理器 |
| _userSettings | UserSettings | 使用者設定資料 |

## 方法說明

### 建構函式

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| UC_UserManager | 無 | 無 | 預設建構函式，使用預設資料庫連接 |
| UC_UserManager | string adminUserID | 無 | 指定管理員使用者ID的建構函式 |
| UC_UserManager | DBManger dbManager | 無 | 指定資料庫管理器的建構函式 |

### 使用者管理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| CreateUser | UserInfo userInfo | bool | 建立新使用者 |
| UpdateUser | UserInfo userInfo | bool | 更新使用者資料 |
| DeleteUser | string userID | bool | 刪除指定使用者 |
| GetUser | string userID | UserInfo | 取得指定使用者資料 |
| GetAllUsers | 無 | DataTable | 取得所有使用者資料 |
| SearchUsers | string searchCriteria | DataTable | 依條件搜尋使用者 |
| ActivateUser | string userID | bool | 啟用使用者帳號 |
| DeactivateUser | string userID | bool | 停用使用者帳號 |

### 密碼管理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| SetPassword | string userID, string password | bool | 設定使用者密碼 |
| ResetPassword | string userID | string | 重設使用者密碼並返回新密碼 |
| ChangePassword | string userID, string oldPassword, string newPassword | bool | 變更使用者密碼 |
| ValidatePassword | string userID, string password | bool | 驗證使用者密碼 |
| EncryptPassword | string plainPassword | string | 加密密碼 |
| IsPasswordExpired | string userID | bool | 檢查密碼是否過期 |

### 角色管理方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| AssignRole | string userID, string roleID | bool | 指派角色給使用者 |
| RemoveRole | string userID, string roleID | bool | 移除使用者的角色 |
| GetUserRoles | string userID | DataTable | 取得使用者的所有角色 |
| HasRole | string userID, string roleID | bool | 檢查使用者是否擁有特定角色 |
| CreateRole | RoleInfo roleInfo | bool | 建立新角色 |
| UpdateRole | RoleInfo roleInfo | bool | 更新角色資料 |
| DeleteRole | string roleID | bool | 刪除角色 |

### 認證與授權方法

| 方法名稱 | 參數 | 回傳值 | 說明 |
|---------|------|--------|------|
| Authenticate | string userID, string password | bool | 驗證使用者憑證 |
| AuthenticateAD | string domain, string username, string password | bool | 通過Active Directory驗證使用者 |
| IsAuthorized | string userID, string resourceID | bool | 檢查使用者是否有權限存取指定資源 |
| GetUserPermissions | string userID | DataTable | 取得使用者的所有權限 |
| RecordLoginAttempt | string userID, bool success | void | 記錄使用者登入嘗試 |
| LockAccount | string userID | bool | 鎖定使用者帳號 |
| UnlockAccount | string userID | bool | 解鎖使用者帳號 |

## 程式碼說明

### 使用者資料類別

```csharp
public class UserInfo
{
    public string UserID { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastLoginDate { get; set; }
    public UserStatusEnum Status { get; set; }
    public bool IsADUser { get; set; }
    public string ADDomain { get; set; }
    public List<string> AssignedRoles { get; set; }
    
    public UserInfo()
    {
        Status = UserStatusEnum.Active;
        IsADUser = false;
        AssignedRoles = new List<string>();
        CreatedDate = DateTime.Now;
    }
}
```

### 使用者建立方法

```csharp
public bool CreateUser(UserInfo userInfo)
{
    try
    {
        // 驗證必要欄位
        if (string.IsNullOrEmpty(userInfo.UserID) || string.IsNullOrEmpty(userInfo.UserName))
        {
            _errMsg = "使用者ID和姓名為必填欄位";
            _hasError = true;
            return false;
        }
        
        // 檢查使用者ID是否已存在
        if (UserExists(userInfo.UserID))
        {
            _errMsg = "使用者ID已存在";
            _hasError = true;
            return false;
        }
        
        // 開始資料庫交易
        _db.BeginTransaction();
        
        try
        {
            // 新增使用者主檔
            string sql = @"INSERT INTO SysUsers 
                          (UserID, UserName, Email, Department, PhoneNumber, Status, IsADUser, ADDomain, CreatedBy, CreatedDate) 
                          VALUES 
                          (@UserID, @UserName, @Email, @Department, @PhoneNumber, @Status, @IsADUser, @ADDomain, @CreatedBy, @CreatedDate)";
            
            _db.AddParameter("@UserID", userInfo.UserID);
            _db.AddParameter("@UserName", userInfo.UserName);
            _db.AddParameter("@Email", userInfo.Email);
            _db.AddParameter("@Department", userInfo.Department);
            _db.AddParameter("@PhoneNumber", userInfo.PhoneNumber);
            _db.AddParameter("@Status", (int)userInfo.Status);
            _db.AddParameter("@IsADUser", userInfo.IsADUser);
            _db.AddParameter("@ADDomain", userInfo.ADDomain);
            _db.AddParameter("@CreatedBy", _userID);
            _db.AddParameter("@CreatedDate", DateTime.Now);
            
            int result = _db.ExecuteNonQuery(sql);
            
            if (result <= 0)
            {
                _db.RollbackTransaction();
                _errMsg = "新增使用者失敗";
                _hasError = true;
                return false;
            }
            
            // 如果不是AD使用者，則設置初始密碼
            if (!userInfo.IsADUser)
            {
                string initialPassword = GenerateInitialPassword();
                if (!SetPassword(userInfo.UserID, initialPassword))
                {
                    _db.RollbackTransaction();
                    return false;
                }
            }
            
            // 指派角色
            if (userInfo.AssignedRoles != null && userInfo.AssignedRoles.Count > 0)
            {
                foreach (string roleID in userInfo.AssignedRoles)
                {
                    if (!AssignRole(userInfo.UserID, roleID))
                    {
                        _db.RollbackTransaction();
                        return false;
                    }
                }
            }
            
            // 提交交易
            _db.CommitTransaction();
            
            // 更新快取
            if (_userCache.ContainsKey(userInfo.UserID))
                _userCache[userInfo.UserID] = userInfo;
            else
                _userCache.Add(userInfo.UserID, userInfo);
            
            _hasError = false;
            return true;
        }
        catch (Exception ex)
        {
            _db.RollbackTransaction();
            _errMsg = "建立使用者時發生錯誤: " + ex.Message;
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

### 權限檢查方法

```csharp
public bool IsAuthorized(string userID, string resourceID)
{
    try
    {
        // 檢查參數
        if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(resourceID))
        {
            _errMsg = "使用者ID和資源ID不能為空";
            _hasError = true;
            return false;
        }
        
        // 檢查使用者是否為系統管理員
        if (IsAdmin(userID))
        {
            return true; // 系統管理員擁有所有權限
        }
        
        // 檢查使用者狀態
        UserInfo user = GetUser(userID);
        if (user == null || user.Status != UserStatusEnum.Active)
        {
            _errMsg = "使用者不存在或未啟用";
            _hasError = true;
            return false;
        }
        
        // 檢查直接指派的權限
        string sql = @"SELECT COUNT(*) FROM UserPermissions 
                       WHERE UserID = @UserID AND ResourceID = @ResourceID";
        
        _db.AddParameter("@UserID", userID);
        _db.AddParameter("@ResourceID", resourceID);
        
        int directCount = Convert.ToInt32(_db.ExecuteScalar(sql));
        if (directCount > 0)
        {
            return true;
        }
        
        // 檢查通過角色指派的權限
        sql = @"SELECT COUNT(*) FROM UserRoles UR
                INNER JOIN RolePermissions RP ON UR.RoleID = RP.RoleID
                WHERE UR.UserID = @UserID AND RP.ResourceID = @ResourceID";
        
        int roleCount = Convert.ToInt32(_db.ExecuteScalar(sql));
        if (roleCount > 0)
        {
            return true;
        }
        
        // 沒有找到任何權限
        _errMsg = "使用者沒有該資源的權限";
        _hasError = true;
        return false;
    }
    catch (Exception ex)
    {
        _errMsg = "檢查權限時發生錯誤: " + ex.Message;
        _hasError = true;
        return false;
    }
}
```

## 使用者狀態列舉

```csharp
public enum UserStatusEnum
{
    Active = 1,      // 啟用
    Inactive = 2,    // 停用
    Locked = 3,      // 已鎖定
    Deleted = 4      // 已刪除
}
```

## 使用範例

以下是 UC_UserManager 的使用範例：

### 建立新使用者

```csharp
// 建立使用者管理器實例
UC_UserManager userMgr = new UC_UserManager("ADMIN001");

// 建立新使用者資料
UserInfo newUser = new UserInfo
{
    UserID = "USER001",
    UserName = "王小明",
    Email = "user001@example.com",
    Department = "會計部",
    PhoneNumber = "02-12345678",
    IsADUser = false,
    Status = UserStatusEnum.Active,
    AssignedRoles = new List<string> { "ACCOUNTANT", "REPORTER" }
};

// 建立使用者
if (userMgr.CreateUser(newUser))
{
    // 使用者建立成功
    // 可以取得初始密碼發送給使用者
    string initialPassword = userMgr.ResetPassword("USER001");
    // 發送密碼給使用者的實作...
}
else
{
    string errorMsg = userMgr.ErrorMessage;
    // 處理錯誤
}
```

### 驗證使用者並檢查權限

```csharp
// 建立使用者管理器實例
UC_UserManager userMgr = new UC_UserManager();

// 驗證使用者登入
if (userMgr.Authenticate("USER001", "password123"))
{
    // 使用者驗證成功
    
    // 檢查使用者權限
    if (userMgr.IsAuthorized("USER001", "GL_TRANSACTION_ENTRY"))
    {
        // 使用者有權限進行交易錄入
    }
    else
    {
        // 使用者沒有權限
        string errorMsg = userMgr.ErrorMessage;
        // 顯示權限錯誤訊息
    }
}
else
{
    // 使用者驗證失敗
    string errorMsg = userMgr.ErrorMessage;
    // 顯示登入錯誤訊息
    
    // 記錄失敗的登入嘗試
    userMgr.RecordLoginAttempt("USER001", false);
}
```

## 資料結構

### 資料庫表格

UC_UserManager 使用以下資料庫表格：

1. **SysUsers**：使用者主檔表
2. **UserPasswords**：使用者密碼表
3. **UserRoles**：使用者角色關聯表
4. **Roles**：角色定義表
5. **RolePermissions**：角色權限表
6. **UserPermissions**：使用者直接權限表
7. **Resources**：資源定義表
8. **LoginHistory**：登入歷史記錄表

### SysUsers 表結構

| 欄位名稱 | 資料類型 | 說明 |
|---------|---------|------|
| UserID | VARCHAR(50) | 使用者ID (主鍵) |
| UserName | VARCHAR(100) | 使用者姓名 |
| Email | VARCHAR(100) | 電子郵件 |
| Department | VARCHAR(50) | 部門 |
| PhoneNumber | VARCHAR(20) | 電話號碼 |
| Status | INT | 使用者狀態 |
| IsADUser | BIT | 是否為 AD 使用者 |
| ADDomain | VARCHAR(50) | AD 網域名稱 |
| LastLoginDate | DATETIME | 最後登入日期 |
| PasswordLastChanged | DATETIME | 密碼最後變更日期 |
| FailedLoginAttempts | INT | 失敗登入嘗試次數 |
| CreatedBy | VARCHAR(50) | 建立者 |
| CreatedDate | DATETIME | 建立日期 |
| ModifiedBy | VARCHAR(50) | 最後修改者 |
| ModifiedDate | DATETIME | 最後修改日期 |

### UserPasswords 表結構

| 欄位名稱 | 資料類型 | 說明 |
|---------|---------|------|
| UserID | VARCHAR(50) | 使用者ID (主鍵) |
| PasswordHash | VARCHAR(256) | 密碼雜湊值 |
| Salt | VARCHAR(128) | 加密鹽值 |
| ExpiryDate | DATETIME | 密碼過期日期 |
| PasswordResetRequired | BIT | 是否需要重設密碼 |
| CreatedDate | DATETIME | 建立日期 |

## 異常處理

UC_UserManager 實現了完整的異常處理機制：

1. 使用 try-catch 捕獲可能的異常
2. 設置錯誤標記和錯誤訊息
3. 使用資料庫交易確保資料完整性
4. 提供錯誤回復機制
5. 記錄安全相關的異常到審計日誌

## 注意事項與限制

1. **密碼策略**：系統要求密碼符合一定的複雜度（包含大小寫、數字和特殊字元）
2. **帳號鎖定**：連續失敗登入超過設定次數將導致帳號鎖定
3. **密碼過期**：密碼過期後要求使用者變更密碼
4. **權限繼承**：使用者通過角色繼承權限，因此修改角色權限將影響所有指派該角色的使用者
5. **快取管理**：使用者資料有快取機制，可能存在短暫的資料不一致性
6. **AD同步**：AD使用者資料同步可能受網路和域控制器狀態影響

## 效能考量

1. **使用者快取**：使用記憶體快取減少資料庫查詢
2. **批次處理**：使用批次處理進行大量使用者操作
3. **延遲載入**：使用延遲載入模式按需取得使用者詳細資料
4. **查詢優化**：使用索引和最佳化查詢優化使用者搜尋
5. **並行控制**：使用適當的並行控制機制處理多使用者操作

## 安全性考量

1. **密碼存儲**：密碼通過加鹽雜湊存儲，不存儲明文
2. **傳輸保護**：使用SSL/TLS保護密碼傳輸
3. **最小權限**：實施最小權限原則，僅授予必要的權限
4. **稽核追蹤**：記錄所有重要操作的稽核日誌
5. **異常檢測**：監控和檢測異常的登入行為
6. **資料保護**：實施敏感資料的訪問控制和加密

## 待改進事項

1. **雙因素認證**：增加對雙因素認證的支援
2. **OAuth整合**：增加對OAuth和OpenID Connect的支援
3. **權限管理介面**：開發更友好的權限管理介面
4. **報表功能**：增強使用者和權限報表功能
5. **自動化流程**：實現使用者生命週期管理的自動化流程
6. **效能優化**：優化大量使用者的處理效能

## 相關檔案

1. **DBManger.cs**：資料庫操作管理類別
2. **AppAuthority.cs**：應用程式權限管理類別
3. **LoginClass.cs**：使用者登入類別
4. **PasswordEncryptor.cs**：密碼加密類別
5. **ADHelper.cs**：Active Directory 輔助類別

## 維護記錄

| 日期      | 版本   | 變更內容                       | 變更人員    |
|-----------|--------|--------------------------------|------------|
| 2025/5/21 | 1.0    | 首次建立使用者管理規格書        | Claude AI  | 