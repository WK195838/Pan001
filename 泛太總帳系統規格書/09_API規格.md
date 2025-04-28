# API規格文件

## 1. API概述

### 1.1 基本資訊

- **基礎URL**: `https://api.pantai-gl.com.tw/v1`
- **資料格式**: JSON
- **字元編碼**: UTF-8
- **API版本控制**: 在URL路徑中使用，例如 `/v1/employees`

### 1.2 通訊協定

- **架構**: RESTful
- **HTTP方法**:
  - GET: 查詢資料
  - POST: 新增資料
  - PUT: 更新資料
  - DELETE: 刪除資料

### 1.3 安全機制

- **HTTPS加密**: 所有API通訊必須使用HTTPS協定
- **令牌認證**: 使用JWT (JSON Web Token)進行認證
- **IP白名單**: 可設定限制特定IP存取API
- **請求頻率限制**: 預設每分鐘100次請求限制

## 2. 身份驗證API

### 2.1 獲取授權令牌

- **端點**: `/auth/token`
- **方法**: POST
- **說明**: 取得API存取令牌
- **請求參數**:

```json
{
  "username": "admin",
  "password": "your_password",
  "client_id": "your_client_id"
}
```

- **回應**:

```json
{
  "status": "success",
  "data": {
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expires_in": 3600
  }
}
```

### 2.2 更新令牌

- **端點**: `/auth/refresh`
- **方法**: POST
- **說明**: 使用refresh_token更新access_token
- **請求標頭**:
  - `Authorization: Bearer {refresh_token}`
- **回應**:

```json
{
  "status": "success",
  "data": {
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expires_in": 3600
  }
}
```

## 3. 員工資料API

### 3.1 取得員工列表

- **端點**: `/employees`
- **方法**: GET
- **說明**: 獲取所有員工基本資料
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **查詢參數**:
  - `department`: 部門代碼 (選填)
  - `status`: 員工狀態 (選填，預設為"active")
  - `page`: 頁碼 (選填，預設為1)
  - `limit`: 每頁筆數 (選填，預設為20，最大100)
- **回應**:

```json
{
  "status": "success",
  "data": {
    "total": 150,
    "page": 1,
    "limit": 20,
    "employees": [
      {
        "emp_id": "E001",
        "name": "王大明",
        "department": "IT",
        "position": "系統工程師",
        "hire_date": "2020-01-01"
      },
      // ...更多員工資料
    ]
  }
}
```

### 3.2 取得單一員工資料

- **端點**: `/employees/{emp_id}`
- **方法**: GET
- **說明**: 獲取特定員工的詳細資料
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **回應**:

```json
{
  "status": "success",
  "data": {
    "emp_id": "E001",
    "name": "王大明",
    "id_number": "A123456789",
    "gender": "M",
    "birth_date": "1985-05-15",
    "department": "IT",
    "position": "系統工程師",
    "hire_date": "2020-01-01",
    "contact": {
      "phone": "0912345678",
      "email": "wang@example.com",
      "address": "台北市信義區信義路5段7號"
    },
    "salary_info": {
      "base_salary": 50000,
      "position_allowance": 5000,
      "bank_account": "012-345678-901"
    },
    "insurance": {
      "labor_insurance": true,
      "health_insurance": true,
      "labor_pension": true
    }
  }
}
```

### 3.3 新增員工

- **端點**: `/employees`
- **方法**: POST
- **說明**: 新增員工資料
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **請求參數**: 員工詳細資料
- **回應**:

```json
{
  "status": "success",
  "data": {
    "emp_id": "E050",
    "message": "員工資料建立成功"
  }
}
```

### 3.4 更新員工資料

- **端點**: `/employees/{emp_id}`
- **方法**: PUT
- **說明**: 更新員工資料
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **請求參數**: 需更新的員工資料
- **回應**:

```json
{
  "status": "success",
  "data": {
    "emp_id": "E001",
    "message": "員工資料更新成功"
  }
}
```

## 4. 薪資資料API

### 4.1 取得員工薪資資料

- **端點**: `/payrolls/employees/{emp_id}`
- **方法**: GET
- **說明**: 獲取特定員工的薪資資料
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **查詢參數**:
  - `year`: 年份 (選填，預設為當年)
  - `month`: 月份 (選填，預設為上個月)
- **回應**:

```json
{
  "status": "success",
  "data": {
    "emp_id": "E001",
    "name": "王大明",
    "year": 2025,
    "month": 3,
    "base_salary": 50000,
    "position_allowance": 5000,
    "overtime_pay": 3000,
    "bonus": 0,
    "deductions": {
      "leave_deduction": 0,
      "labor_insurance": 900,
      "health_insurance": 750,
      "labor_pension": 3000
    },
    "tax": 2500,
    "net_salary": 50850,
    "payment_status": "paid",
    "payment_date": "2025-04-05"
  }
}
```

### 4.2 批次薪資計算

- **端點**: `/payrolls/batch`
- **方法**: POST
- **說明**: 觸發批次薪資計算處理
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **請求參數**:

```json
{
  "year": 2025,
  "month": 4,
  "departments": ["IT", "HR"], // 選填，不填則處理所有部門
  "recalculate": false // 是否重新計算，預設false
}
```

- **回應**:

```json
{
  "status": "success",
  "data": {
    "batch_id": "BAT20250428001",
    "message": "批次薪資計算已開始處理",
    "estimated_completion_time": "2025-04-28T10:30:00Z"
  }
}
```

### 4.3 查詢批次處理狀態

- **端點**: `/payrolls/batch/{batch_id}`
- **方法**: GET
- **說明**: 查詢批次薪資計算處理狀態
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **回應**:

```json
{
  "status": "success",
  "data": {
    "batch_id": "BAT20250428001",
    "progress": 75,
    "status": "processing", // processing, completed, failed
    "started_at": "2025-04-28T10:00:00Z",
    "estimated_completion_time": "2025-04-28T10:30:00Z",
    "details": {
      "total_employees": 100,
      "processed_employees": 75,
      "errors": []
    }
  }
}
```

## 5. 報表產生API

### 5.1 產生薪資單

- **端點**: `/reports/payslips`
- **方法**: POST
- **說明**: 產生員工薪資單
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **請求參數**:

```json
{
  "year": 2025,
  "month": 4,
  "format": "pdf", // pdf, excel
  "employees": ["E001", "E002"], // 選填，不填則產生所有員工
  "send_email": false // 是否自動寄送郵件，預設false
}
```

- **回應**:

```json
{
  "status": "success",
  "data": {
    "report_id": "REP20250428001",
    "message": "薪資單產生請求已接受",
    "estimated_completion_time": "2025-04-28T10:15:00Z"
  }
}
```

### 5.2 查詢報表產生狀態

- **端點**: `/reports/{report_id}`
- **方法**: GET
- **說明**: 查詢報表產生狀態
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **回應**:

```json
{
  "status": "success",
  "data": {
    "report_id": "REP20250428001",
    "progress": 100,
    "status": "completed", // processing, completed, failed
    "started_at": "2025-04-28T10:00:00Z",
    "completed_at": "2025-04-28T10:05:00Z",
    "download_url": "https://api.pantai-gl.com.tw/v1/reports/download/REP20250428001",
    "expires_at": "2025-05-05T10:05:00Z"
  }
}
```

### 5.3 下載報表

- **端點**: `/reports/download/{report_id}`
- **方法**: GET
- **說明**: 下載已產生的報表
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **回應**: 二進制檔案流 (PDF或Excel)

## 6. 系統參數API

### 6.1 取得系統參數

- **端點**: `/system/parameters`
- **方法**: GET
- **說明**: 獲取系統參數
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **查詢參數**:
  - `category`: 參數類別 (選填)
- **回應**:

```json
{
  "status": "success",
  "data": {
    "parameters": [
      {
        "category": "salary",
        "key": "min_hourly_wage",
        "value": "168",
        "description": "最低時薪",
        "updated_at": "2025-01-01T00:00:00Z"
      },
      {
        "category": "insurance",
        "key": "labor_insurance_rate",
        "value": "0.11",
        "description": "勞保費率",
        "updated_at": "2025-01-01T00:00:00Z"
      },
      // ...更多參數
    ]
  }
}
```

### 6.2 更新系統參數

- **端點**: `/system/parameters/{key}`
- **方法**: PUT
- **說明**: 更新特定系統參數
- **請求標頭**:
  - `Authorization: Bearer {access_token}`
- **請求參數**:

```json
{
  "value": "175",
  "description": "最低時薪(2025年度)"
}
```

- **回應**:

```json
{
  "status": "success",
  "data": {
    "key": "min_hourly_wage",
    "message": "系統參數更新成功"
  }
}
```

## 7. 共通錯誤碼

以下是API可能返回的常見錯誤碼:

| 錯誤碼 | 說明 |
|--------|------|
| 400 | 請求格式錯誤 |
| 401 | 未授權 (Token無效或過期) |
| 403 | 權限不足 |
| 404 | 資源不存在 |
| 422 | 請求參數驗證失敗 |
| 429 | 請求頻率超過限制 |
| 500 | 伺服器內部錯誤 |

錯誤回應範例:

```json
{
  "status": "error",
  "error": {
    "code": 400,
    "message": "請求格式錯誤",
    "details": "缺少必要參數'year'"
  }
}
```

## 8. API集成範例

### 8.1 完整薪資計算流程

以下是一個完整薪資計算流程的API調用順序:

1. 獲取授權令牌
2. 啟動批次薪資計算
3. 查詢批次處理狀態
4. 產生薪資單
5. 查詢薪資單產生狀態
6. 下載薪資單

### 8.2 JavaScript範例

```javascript
// 使用JavaScript獲取員工薪資資料範例
async function getEmployeePayroll(employeeId, year, month) {
  try {
    // 1. 獲取授權令牌
    const authResponse = await fetch('https://api.pantai-gl.com.tw/v1/auth/token', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        username: 'admin',
        password: 'your_password',
        client_id: 'your_client_id'
      })
    });
    
    const authData = await authResponse.json();
    const token = authData.data.access_token;
    
    // 2. 獲取員工薪資資料
    const payrollResponse = await fetch(
      `https://api.pantai-gl.com.tw/v1/payrolls/employees/${employeeId}?year=${year}&month=${month}`, 
      {
        headers: { 'Authorization': `Bearer ${token}` }
      }
    );
    
    const payrollData = await payrollResponse.json();
    return payrollData;
  } catch (error) {
    console.error('API調用錯誤:', error);
    throw error;
  }
}

// 使用範例
getEmployeePayroll('E001', 2025, 4)
  .then(result => console.log('員工薪資資料:', result))
  .catch(error => console.error('獲取薪資資料失敗:', error));
``` 