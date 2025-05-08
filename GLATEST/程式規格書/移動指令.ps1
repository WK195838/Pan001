# 規格書文件整理腳本
# 按類型將規格書移動到適當的目錄

# 確保目錄存在
$baseDir = $PWD.Path
$mainScreenDir = "$baseDir\GL-主要畫面程式"
$transactionDir = "$baseDir\GL-交易處理"
$reportDir = "$baseDir\GL-報表程式"
$clientScriptDir = "$baseDir\GL-用戶端腳本"
$coreComponentDir = "$baseDir\GL-核心元件"
$serviceComponentDir = "$baseDir\GL-服務元件"
$controlDir = "$baseDir\GL-控制項"

# 檢查並創建目錄
function Ensure-Directory {
    param (
        [string]$path
    )
    if (-not (Test-Path -Path $path)) {
        New-Item -ItemType Directory -Path $path | Out-Null
        Write-Host "創建目錄: $path"
    }
}

Ensure-Directory -path $mainScreenDir
Ensure-Directory -path $transactionDir
Ensure-Directory -path $reportDir
Ensure-Directory -path $clientScriptDir
Ensure-Directory -path $coreComponentDir
Ensure-Directory -path $serviceComponentDir
Ensure-Directory -path $controlDir

# 移動文件函數
function Move-File {
    param (
        [string]$source,
        [string]$destination
    )
    
    if (Test-Path -Path $source) {
        $fileName = Split-Path $source -Leaf
        $destFile = Join-Path -Path $destination -ChildPath $fileName
        
        # 檢查目標文件是否已存在
        if (Test-Path -Path $destFile) {
            Write-Host "警告: 目標位置已存在文件: $destFile" -ForegroundColor Yellow
        } else {
            Move-Item -Path $source -Destination $destination
            Write-Host "移動: $source -> $destination"
        }
    } else {
        Write-Host "錯誤: 源文件不存在: $source" -ForegroundColor Red
    }
}

# 1. 移動主要畫面程式規格書
Write-Host "===== 移動主要畫面程式規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\GLA.master主版面規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\GLA.master.cs主版面程式碼規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\GLADetail.master細節版面規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\GLADetail.master.cs細節版面程式碼規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\Default.aspx預設首頁規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\Default.aspx.cs預設首頁程式碼規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\AuthAD.aspx AD認證規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\AuthAD.aspx.cs AD認證程式碼規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\AppErrorMessage.aspx錯誤訊息規格書.md" -destination $mainScreenDir
Move-File -source "$baseDir\GL-程式規格\AppErrorMessage.aspx.cs錯誤訊息程式碼規格書.md" -destination $mainScreenDir

# 2. 移動交易處理程式規格書
Write-Host "===== 移動交易處理程式規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\PTA0150_M會計科目查詢程式規格書.md" -destination $transactionDir
Move-File -source "$baseDir\PTA0160部門資料維護程式規格書.md" -destination $transactionDir
Move-File -source "$baseDir\PTA0160_M部門資料查詢程式規格書.md" -destination $transactionDir
Move-File -source "$baseDir\PTA0170_M總帳交易查詢程式規格書.md" -destination $transactionDir

# 3. 移動報表程式規格書
Write-Host "===== 移動報表程式規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\GLR0160_會計科目查詢報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR01G0_部門查詢報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR01H0_傳票查詢報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR01J0_現金流量表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR0210_總帳明細報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR0250_部門損益表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GLR0260_現金日報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0150總帳報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0150.aspx.cs總帳報表程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0270銀行存款日報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0270.aspx.cs銀行存款日報表程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02A0會計報表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02A0.aspx.cs會計報表程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02B0預算差異分析表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02Q0傳票列印規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02Q0.aspx.cs傳票列印程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02R0科目對帳單規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR02R0.aspx.cs科目對帳單程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0300交易明細表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0300.aspx.cs交易明細表程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0310交易彙總表規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0310.aspx.cs交易彙總表程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0320總分類帳規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0320.aspx.cs總分類帳程式碼規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0330明細分類帳規格書.md" -destination $reportDir
Move-File -source "$baseDir\GL-程式規格\GLR0330.aspx.cs明細分類帳程式碼規格書.md" -destination $reportDir

# 4. 移動用戶端腳本規格書
Write-Host "===== 移動用戶端腳本規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\GL-程式規格\pagefunction.js 頁面函數規格書.md" -destination $clientScriptDir
Move-File -source "$baseDir\GL-程式規格\ModPopFunction.js 彈出窗口函數規格書.md" -destination $clientScriptDir
Move-File -source "$baseDir\GL-程式規格\Busy.js忙碌指示器規格書.md" -destination $clientScriptDir
Move-File -source "$baseDir\GL-程式規格\InvSysFunction.js庫存系統函數規格書.md" -destination $clientScriptDir

# 5. 移動核心元件規格書
Write-Host "===== 移動核心元件規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\GL-程式規格\IBosDB.cs資料庫連接類別規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\IBosDBCommon.cs資料庫共用參數規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\Page_BaseClass頁面基底類別規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\GLA0110TManager交易管理規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\CompanyManager公司資料管理規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\ExcelManger Excel管理規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\CheckAuthorization權限檢查程式規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\AppAuthority權限管理類別規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\LoginClass登入模組規格書.md" -destination $coreComponentDir
Move-File -source "$baseDir\GL-程式規格\UC_UserManager使用者管理規格書.md" -destination $coreComponentDir

# 6. 移動服務元件規格書
Write-Host "===== 移動服務元件規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\WSDialogData對話框資料服務規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\WSDialogData_Table對話框表格服務規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\WSGLAcctDefJson科目定義服務規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSAutoComplete自動完成服務規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSFlowSignManager流程簽核管理規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSDialogData.asmx對話框資料服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSDialogData_Table.asmx對話框表格服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSDialogData_Exception.asmx對話框例外服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSGLAcctDefJson.asmx科目定義服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSFlowSignJson.asmx流程簽核服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\WSAutoComplete.asmx自動完成服務介面規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\PopupDialog彈出對話框規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\PopupDialog.aspx.cs彈出對話框程式碼規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\ModelPopupDialog模態對話框規格書.md" -destination $serviceComponentDir
Move-File -source "$baseDir\GL-程式規格\ModelPopupDialog.aspx.cs模態對話框程式碼規格書.md" -destination $serviceComponentDir

# 7. 移動控制項規格書
Write-Host "===== 移動控制項規格書 =====" -ForegroundColor Green
Move-File -source "$baseDir\CalendarDate日期控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\CalendarDay日控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\CalendarMonth月份控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\CalendarWeek週控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\Navigator導航控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CalendarDay.ascx.cs日控制項程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CalendarMonth.ascx.cs月份控制項程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CalendarWeek.ascx.cs週控制項程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CalendarWeek週控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\YearList年度清單規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\YearList.ascx.cs年度清單程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CodeList代碼清單規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CodeList.ascx.cs代碼清單程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CompanyList公司清單規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\CompanyList.ascx.cs公司清單程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\PeriodList期間清單規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\PeriodList.ascx.cs期間清單程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\SearchList搜尋清單規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\SearchList.ascx.cs搜尋清單程式碼規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\Subjects科目控制項規格書.md" -destination $controlDir
Move-File -source "$baseDir\GL-程式規格\Subjects.ascx.cs科目控制項程式碼規格書.md" -destination $controlDir

Write-Host "規格書整理完成！" -ForegroundColor Green 