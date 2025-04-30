using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Payroll 的摘要
/// 2010/12/17 加入儀測客製化與二期計算之相關判斷
/// public OverTimes GetWTOverTime(PayrolList PayrollLt)    => 配合時薪修改加班費
/// public int GetPersonalHoursSalary(PayrolList PayrollLt) => 上下期時薪計算
/// public int GetOnePayroll(PayrolList PayrollLt)
/// =>上下期二者皆有時,應寫入之薪資(按比例)
/// 1.計算固定薪資
/// 2.計算其它薪資
/// 3.取得代扣稅款金額與所得稅率
/// public decimal GetAttendanceFee(PayrolList PayrollLt, decimal dAttendanceFee) =>全勤計算與判斷(目前為儀測定義之規則)
/// 2011/08/23 加入泛太財務要求
/// 時薪計算時取到小數第二位Math.Round(X,2)。時薪*時數後，無條件進位到整數Math.Ceiling(X)。
/// 請假扣薪及遲到扣薪延用原設定Math.Round
/// 勞保費破月計算時改成,改成4捨5入 Math.Round
/// 2011/09/30 將勞保費破月計算改拆至勞保費函式中
/// 2011/11/29 增修:依規定凡當月離職者,一律不扣健保費;如這名員工服務到當月30日(或31日)離職健保是隔月一號才能轉出
/// 2011/12/26 
/// A.修正計薪跨月時天數不正確問題;
/// B.新進人員上月未計之伙食費已於請假扣薪時按比例加入;
/// C.新進人員加入上月未扣福利金金額
/// 2011/12/28 A.新進人員上月未付底薪已於請假扣薪時按比例加入
/// 2012/02/07 遲到扣款增修:201202開始施行之差勤規定
/// 2012/02/14 kaya ConfirmDraftPayroll:改為使用DB SP[dbo.sp_PY_ConfirmDraftPayroll]執行以便控管及ROLLBACK
/// 2012/03/01 2月份計薪時,以30天為基數
/// 2012/03/08 依財務要求修改:破月到職者，日薪是以30天為基數計算，例: 2/10到職，則發放( 30-10+1)=21天薪資
/// 2012/03/08 與財務確認若為計薪當月月底前離職者，日薪基數雖仍為３０天，但在職天數以實際在職天數計算
/// 2012/03/09 增修勞保計算天數:同基本在職天數(未扣差勤)
/// 2012/03/27 依財務要求修改:〔3/26 到職者，實際上班天數應為(31-26+1)天〕經確認， 2月到職之天數計算為純屬２月之特例，其它大小月於破月計薪時皆依實際天數計算之(故若整月在職則,在職天數必為30天)
/// 財務人員說明如下：
/// 標準沒有改變。就是以實際上班天數計薪。
/// 以上例，2月之所以以30天減到職日加一天，起因是12個月日薪是均以30天計。
/// 且2月小於30天，公司已加計當月上班天數，以邏輯論不會以31天去減到職日。
/// 所以，其它大小月皆依實際天數計算是沒錯的。
/// 20120604 kaya 修正:男免稅加班時數因性別判斷有誤問題
/// 2012/07/27 kaya 修正在職天數大於計薪基本天數問題
/// 20120920 遲到扣薪計算調整=>遲到扣薪規則: 除逢三倍數需扣薪外，遲到超過9:31(含)亦需扣半日薪；且超過30分之遲到不列入累計次數中
/// 2013/01/25 依財務要求，所有破月到職與破月離職之員工福利金皆以破月計算(當破月到職或破月離職時，FullLIFee＝N，即表示應破月計算金額)
/// 2013/04/23 依財務通知，健保費計算程式需修改如下:
/// 健保局確認現行代扣方式
/// 1.如當日報到並離職，其健保費個人部份毋需代扣且公司也不必負擔
/// 2.如當月報到當月離職(不含報到當日)，個人需代扣健保費且公司也需負擔
/// 3.如前一個月之前報到，本月份離職，則個人部份毋需代扣且公司也不必負擔
/// 2013/05/21 即使是破月計算福利金之離職人員,亦不應扣除請假日數,故改使用離職日計算比率
/// 此項修改未異動 Payroll 僅修改 FringeBenefits
/// 2013/05/30 修正上月26後報到新人之上月天數因大小月而少計1日之問題
/// </summary>
public class Payroll
{
    UserInfo _UserInfo = new UserInfo();
    string strSQL;
    string PayrollTable = "PayrollWorking";
    int thisYMPayDate = 30;
    DateTime thisYMStratDate = DateTime.Today.AddDays(1 - DateTime.Today.Day);
    DateTime thisYMEndDate = DateTime.Today.AddDays(0 - DateTime.Today.Day).AddMonths(1);
    string testEmpId = "A1700";

	public Payroll()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
        if (!_UserInfo.SysSet.GetConfigString("SYSMode").Contains("OfficialVersion"))
            PayrollTable = "PayrolltestWorking";
	}

    public bool CheckLicense(string strLicense)
    {
        return true;
    }

    /// <summary>
    /// 薪資主表
    /// </summary>
    public struct PayrolList
    {
        /// <summary>
        /// 公司
        /// </summary>
        public string Company;
        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmployeeId;
        /// <summary>
        /// 計薪別
        /// </summary>
        public string PayCode;
        /// <summary>
        /// 年月
        /// </summary>
        public int SalaryYM;
        /// <summary>
        /// 期別
        /// </summary>
        public string PeriodCode;
        /// <summary>
        /// 底薪
        /// </summary>
        public int BaseSalary;
        /// <summary>
        /// 底薪(未扣除扣薪假)
        /// </summary>
        public int MonthlyBaseSalary;
        /// <summary>
        /// 計薪天數
        /// </summary>
        public decimal Paydays;
        /// <summary>
        /// 請假扣薪時數
        /// </summary>
        public int LeaveHours_deduction;
        /// <summary>
        /// 所得稅率
        /// </summary>
        public decimal TaxRate;
        /// <summary>
        /// 免稅加項
        /// </summary>
        public int NT_P;
        /// <summary>
        /// 應稅加項（薪資）
        /// </summary>
        public int WT_P_Salary;
        /// <summary>
        /// 免稅減項
        /// </summary>
        public int NT_M;
        /// <summary>
        /// 勞保費
        /// </summary>
        public int LI_Fee;
        /// <summary>
        /// 勞保費(公司負擔)
        /// </summary>
        public int LI_CompanyBurden;
        /// <summary>
        /// 健保費
        /// </summary>
        public int HI_Fee;
        /// <summary>
        /// 健保費(公司負擔)
        /// </summary>
        public int HI_CompanyBurden;
        /// <summary>
        /// 健保投保人數
        /// </summary>
        public int HI_Person;
        /// <summary>
        /// 眷屬1身份証號
        /// </summary>
        public string Dependent1_IDNo;
        /// <summary>
        /// 眷屬1健保費
        /// </summary>
        public int Dependent1_HI_Fee;
        /// <summary>
        /// 眷屬2身份証號
        /// </summary>
        public string Dependent2_IDNo;
        /// <summary>
        /// 眷屬2健保費
        /// </summary>
        public int Dependent2_HI_Fee;
        /// <summary>
        /// 眷屬3身份証號
        /// </summary>
        public string Dependent3_IDNo;
        /// <summary>
        /// 眷屬3健保費
        /// </summary>
        public int Dependent3_HI_Fee;
        /// <summary>
        /// 退休金自提
        /// </summary>
        public int RetirementPension;
        /// <summary>
        /// 退休金(公司負擔)
        /// </summary>
        public int RP_CompanyBurden;
        /// <summary>
        /// 離職碼
        /// </summary>
        public string ResignCode;
        /// <summary>
        /// 應稅加項（獎金）
        /// </summary>
        public int WT_P_Bonus;
        /// <summary>
        /// 應稅減項（薪資）
        /// </summary>
        public int WT_M_Salary;
        /// <summary>
        /// 應稅減項（獎金）
        /// </summary>
        public int WT_M_Bonus;
        /// <summary>
        /// 上期借支
        /// </summary>
        public int P1_borrowing;
        /// <summary>
        /// 應稅加班時數
        /// </summary>
        public decimal WT_Overtime;
        /// <summary>
        /// 免稅加班時數
        /// </summary>
        public decimal NT_Overtime;
        /// <summary>
        /// 值班時數
        /// </summary>
        public decimal OnWatch;
        /// <summary>
        /// 應稅加班費
        /// </summary>
        public decimal WT_Overtime_Fee;
        /// <summary>
        /// 免稅加班費
        /// </summary>
        public decimal NT_Overtime_Fee;
        /// <summary>
        /// 值班費
        /// </summary>
        public decimal OnWatch_Fee;
        /// <summary>
        /// DeCodeKey(For DeCode DB Data)
        /// </summary>
        public String DeCodeKey;
    };

    public struct SalaryYearMonth
    {
        /// <summary>
        /// 公司編號
        /// </summary>
        public string Company;
        /// <summary>
        /// 年月
        /// </summary>
        public int SalaryYM;
        /// <summary>
        /// 期別
        /// </summary>
        public string PeriodCode;
        /// <summary>
        /// 最後一次確認年月
        /// </summary>
        public int LastConfirmYM;
        /// <summary>
        /// 試算日期
        /// </summary>
        public string DraftDate;
        /// <summary>
        /// 確認日期
        /// </summary>
        public string ConfirmDate;
        /// <summary>
        /// 是否控管中
        /// </summary>
        public bool isControl;
        /// <summary>
        /// 下一個待確認年月
        /// </summary>
        public int NextConfirmYM;
        /// <summary>
        /// 是否應重新試算
        /// </summary>
        public bool beReDraft;
    }

    public struct OverTimes
    {
        /// <summary>
        /// 加班基礎項目總額
        /// </summary>
        public decimal OvertimeBasic;
        /// <summary>
        /// 加班基礎項目工資總額
        /// </summary>
        public decimal OvertimeBasic1;
        /// <summary>
        /// 加班基礎項目非工資總額
        /// </summary>
        public decimal OvertimeBasic2;
        /// <summary>
        /// 應稅加班時數
        /// </summary>
        public decimal WT_Overtime;
        /// <summary>
        /// 免稅加班時數
        /// </summary>
        public decimal NT_Overtime;
        /// <summary>
        /// 應稅加班費
        /// </summary>
        public decimal WT_Overtime_Fee;
        /// <summary>
        /// 免稅加班費
        /// </summary>
        public decimal NT_Overtime_Fee;
    }

    /// <summary>
    /// 計算傳入的運算式
    /// </summary>
    /// <param name="strFormula">運算式</param>
    /// <returns>整數結果</returns>
    public string computer(string strFormula)
    {
        decimal tempDec = 0;
        string strMsg="";
        tempDec = Computer.ComputerFormula(strFormula, out strMsg);
        return tempDec.ToString("F0");
    }

    /// <summary>
    /// 解析公式
    /// </summary>
    /// <param name="strFunction">公式</param>
    /// <param name="strParameter">參數陣列</param>
    /// <returns>解析後的運算式</returns>
    public string GetFormula(string strFunction, string[] strParameter)
    {
        int iParaLen = strParameter.Length;
        strFunction = strFunction.Trim();        
        string strFormula = "";
        string jFunction, jSymbol;
        int i = 0, j = 0;
                
        while (j < strFunction.Length)
        {
            jSymbol = "";
            jFunction = strFunction.Substring(j, 1);

            string OperaList = "()+-*/0123456789.%";                        

            if (OperaList.Contains(jFunction))
            {
                jSymbol = jFunction;
            }
            else
            {
                if (i < iParaLen)
                    jSymbol = strParameter[i];
                else
                    jSymbol = jFunction;
                i++;
            }
            strFormula += jSymbol;
            j++;
        }

        return strFormula;
    }

    /// <summary>
    /// 解析薪資項目
    /// </summary>
    /// <param name="SalaryId">薪資項目代號</param>
    /// <returns>薪資項目對應之數值</returns>
    public string GetSalaryItem(PayrolList PayrollLt, string SalaryId)
    {
        return GetSalaryItem(PayrollLt, SalaryId, 0);
    }
    
    /// <summary>
    /// 取得個人各別薪資項目值(不重新計算,若已試算過則直接使用;用於出薪資單或報表時)
    /// </summary>
    /// <param name="PayrollLt">個人薪資單</param>
    /// <param name="SalaryId">薪資項目</param>
    /// <returns></returns>
    public string GetPersonalSalaryItem(PayrolList PayrollLt, string SalaryId)
    {
        return GetPersonalSalaryItem(PayrollLt, SalaryId, false);
    }

    /// <summary>
    /// 取得個人各別薪資項目值
    /// </summary>
    /// <param name="PayrollLt">個人薪資單</param>
    /// <param name="SalaryId">薪資項目</param>
    /// <param name="reCounter">重新計算,不使用已計算之數字</param>
    /// <returns></returns>
    public string GetPersonalSalaryItem(PayrolList PayrollLt, string SalaryId, bool reCounter)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string strErr = "";
        Decimal decSalaryItem = 0, decPersonalFixedAmount = 0;

        if (PayrollLt.DeCodeKey.Length == 0)
        {
            strErr = "未設定薪資解密金鑰,無法解析薪資";
            _UserInfo.SysSet.WriteToLogs("PYMsg", strErr + "\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "|SalaryYM:" + PayrollLt.SalaryYM.ToString() + "\n\r");            
        }
        else if (!PayrollLt.DeCodeKey.ToLower().StartsWith("dbo."))
        {
            strErr = "薪資解密金鑰設定錯誤,無法解析薪資";
            _UserInfo.SysSet.WriteToLogs("PYMsg", strErr + "\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "|SalaryYM:" + PayrollLt.SalaryYM.ToString() + "\n\r");
        }
        else
        {
            if (reCounter == false)
            {
                strSQL = "Select count(*) from " + PayrollTable + "_Detail Where Company='" + PayrollLt.Company +
                    "' And EmployeeId='" + PayrollLt.EmployeeId + "' And SalaryYM=" + PayrollLt.SalaryYM.ToString() +
                    " And PeriodCode='" + PayrollLt.PeriodCode + "' And SalaryItem='" + SalaryId + "'";
                DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
                try
                {
                    if (theDT.Rows[0][0].ToString() == "0")
                        reCounter = true;
                }
                catch { reCounter = true; }
            }

            if (reCounter == false)
            {
                #region 已計薪時
                try
                {//取得薪資項目之金額
                    decSalaryItem += GetPayrollSalary(PayrollLt, SalaryId);
                }
                catch
                {
                    _UserInfo.SysSet.WriteToLogs("PYMsg", "個人薪資項目未設定此項目金額\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "\n\r");
                }
                #endregion
            }
            else
            {
                #region 尚未計薪時
                try
                {//先取得個人薪資項目設定之金額
                    strSQL = "Select " + PayrollLt.DeCodeKey + "(Amount) As Amount from Payroll_Master_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And SalaryItem ='" + SalaryId + "'";
                    DataTable theItemAmount = _MyDBM.ExecuteDataTable(strSQL);
                    if (theItemAmount != null && theItemAmount.Rows.Count > 0)
                        decPersonalFixedAmount = (Decimal)theItemAmount.Rows[0][0];
                    else
                        decPersonalFixedAmount = 0;

                    strSQL = "Select (Case When SubString(Convert(varchar,[EffectiveDate],112),1,6) ='" + PayrollLt.SalaryYM.ToString() + "' Then datepart(d,[EffectiveDate]) Else 0 End) Days " +
                        " ,(Case When SubString(Convert(varchar,[EffectiveDate],112),1,6) > '" + PayrollLt.SalaryYM.ToString() + "' then 1 Else (Case When Convert(varchar,[EffectiveDate],112) > '" + PayrollLt.SalaryYM.ToString() + "01' then 0 Else -1 end) end) ADate " +
                        " ,"+ PayrollLt.DeCodeKey + "(OldlSalary) As OldlSalary, " + PayrollLt.DeCodeKey + "(NewSalary) As NewSalary " +
                        " from [AdjustSalary_Master] Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And [AdjustSalaryItem] ='" + SalaryId + "'" +
                        " And Convert(varchar,[ApproveDate],112)<>'19120101' And [EffectiveDate]<=Getdate() " +
                        " Order By (Case When SubString(Convert(varchar,[EffectiveDate],112),1,6)>'" + PayrollLt.SalaryYM.ToString() + "' then Convert(varchar,[EffectiveDate],112) else 0 end),EffectiveDate Desc";
                    DataTable ASDT = _MyDBM.ExecuteDataTable(strSQL);

                    try
                    {
                        if (ASDT != null)
                        {
                            if (ASDT.Rows.Count > 0)
                            {//破月計算調薪
                                int iDays = (int)ASDT.Rows[0]["Days"];
                                int iADate = (int)ASDT.Rows[0]["ADate"];
                                decimal decAD = 0;
                                if ((iADate == 0 && iDays >= 30) || iADate > 0)
                                {
                                    decAD = (Decimal)ASDT.Rows[0]["OldlSalary"];
                                }
                                else if ((iADate == 0 && iDays == 1) || iADate < 0)
                                {
                                    decAD = (Decimal)ASDT.Rows[0]["NewSalary"];
                                }
                                else
                                {
                                    decAD = (Decimal)ASDT.Rows[0]["OldlSalary"] * ((Decimal)iDays / 30) + (Decimal)ASDT.Rows[0]["NewSalary"] * ((Decimal)(30 - iDays) / 30);
                                }

                                decPersonalFixedAmount = Math.Round(decAD, 0);
                            }
                        }
                    }
                    catch
                    {
                        _UserInfo.SysSet.WriteToLogs("PYMsg", "調薪資料有誤\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "|SalaryYM:" + PayrollLt.SalaryYM.ToString() + "\n\r");
                    }

                    try
                    {//使用薪資項目參數計算實際金額
                        decSalaryItem = Convert.ToDecimal(GetSalaryItem(PayrollLt, SalaryId, decPersonalFixedAmount));
                    }
                    catch
                    {
                        strErr = "薪資項目參數設定有誤,無法解析薪資";
                        _UserInfo.SysSet.WriteToLogs("PYErr", strErr + "\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "\n\r");
                    }
                }
                catch
                {
                    strErr = "個人薪資項目未設定金額,無法解析薪資";
                    _UserInfo.SysSet.WriteToLogs("PYMsg", strErr + "\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "\n\r");
                }
                #endregion

                #region 最後加上其它金額:尚未計薪時才要加,否則會重覆計算
                try
                {//不固定在薪資項目中之其它金額
                    decSalaryItem += GetOtherSalary(PayrollLt, SalaryId);
                }
                catch
                {
                    _UserInfo.SysSet.WriteToLogs("PYMsg", "個人薪資項目未設定其它金額\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "\n\r");
                }
                #endregion
            }
        }
        return (strErr.Length > 0) ? strErr : decSalaryItem.ToString();
    }

    /// <summary>
    /// 取得個人經常性支付項目總額(即時)
    /// </summary>
    /// <param name="PayrollLt">個人薪資單</param>
    /// <returns></returns>
    public Decimal GetPersonalRegularPay(PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        Decimal decSalaryItem = 0;

        strSQL = "SELECT SalaryId FROM SalaryStructure_Parameter Where RegularPay = 'Y' And PMType='A'";
        DataTable theRegularPay = _MyDBM.ExecuteDataTable(strSQL);

        if (theRegularPay != null)
        {
            for (int i = 0; i < theRegularPay.Rows.Count; i++)
            {
                try
                {
                    decSalaryItem += Convert.ToDecimal(GetPersonalSalaryItem(PayrollLt, theRegularPay.Rows[i][0].ToString().Trim()));
                }
                catch { }
            }
        }

        return decSalaryItem;
    }

    /// <summary>
    /// 取得個人經常性支付項目總額(試算)
    /// </summary>
    /// <param name="PayrollLt">個人薪資單</param>
    /// <returns></returns>
    public Decimal GetPersonalYMRegularPay(PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        Decimal decSalaryItem = 0;

        strSQL = "Select IsNull(Sum(" + PayrollLt.DeCodeKey + "([SalaryAmount])),0) From ["+PayrollTable+"_Detail] " +
            " Where [SalaryItem] in (SELECT SalaryId FROM SalaryStructure_Parameter Where RegularPay = 'Y' And PMType='A') " +
            " And [Company] = '" + PayrollLt.Company + "'" +
            " And [EmployeeId] ='" + PayrollLt.EmployeeId + "'" +
            " And [SalaryYM] =" + PayrollLt.SalaryYM.ToString() +
            " And [PeriodCode] ='" + PayrollLt.PeriodCode + "'";
        DataTable theRegularPay = _MyDBM.ExecuteDataTable(strSQL);

        if (theRegularPay != null)
        {
            if (theRegularPay.Rows.Count > 0)
            {
                decSalaryItem = (decimal)theRegularPay.Rows[0][0];
            }
        }

        return decSalaryItem;
    }    

    /// <summary>
    /// 解析薪資項目
    /// </summary>
    /// <param name="SalaryId">薪資項目代號</param>
    /// <param name="decPersonalFixedAmount">個人固定薪資項目</param>
    /// <returns>薪資項目對應之數值</returns>
    public string GetSalaryItem(PayrolList PayrollLt, string SalaryId, Decimal decPersonalFixedAmount)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        Decimal decSalaryItem = 0;
        string sItemType, sSalaryType;
        if (PayrollLt.EmployeeId == testEmpId)
        {
        }
        strSQL = "Select  * from SalaryStructure_Parameter Where SalaryId='" + SalaryId + "'";
        DataTable theItemDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theItemDT != null)
        {
            if (theItemDT.Rows.Count > 0)
            {
                sItemType = theItemDT.Rows[0]["ItemType"].ToString().Trim();
                try
                {
                    #region
                    //項目別(ItemType):
                    //分為0.系統設定,1.固定項目,2.變動項目
                    //(通用設定)
                    //此項若設定為[0.系統設定]時,不可刪除
                    //此項若設定為[1.固定項目]時,通用的薪資項目
                    //=>設定為0或1時:不論是否在薪資基本資料中設定此項目,都將於計算薪資時自動加入此項目
                    //此項若設定為[2.變動項目]時,須在員工之薪資基本資料中設定此項目,才會計算此項目薪資
                    //(儀測專用)
                    //此項若設定為[0.系統設定]時,不可刪除
                    //此項若設定為[1.固定項目]時,通用的薪資項目
                    //=>設定為1時:不論是否在薪資基本資料中設定此項目,都將於計算薪資時自動加入此項目
                    //此項若設定為[2.變動項目]時,
                    //=>設定為0或1時:須在員工之薪資基本資料中設定此項目,才會計算此項目薪資

                    //薪資別(SalaryType):
                    //分為A.固定金額,B.薪資比率,C.變動金額
                    //(通用設定)
                    //此項若設定為[A.固定金額]時,
                    //若項目別(ItemType)為[0.系統設定],則金額一率為薪資結構參數中的[固定金額]欄設定之數值
                    //                      若薪資結構參數中的[固定金額]欄設定之數值為0時,才使用員工之薪資基本資料中設定的金額
                    //若項目別(ItemType)為[1.固定項目],則直接使用在員工之薪資基本資料中設定的金額

                    //此項若設定為[B.薪資比率]時,不論是否在薪資基本資料中設定金額
                    //都將於計算薪資時使用薪資結構參數中[基礎薪資項目]與[薪資比率]二欄之設定
                    //計算出[基礎薪資項目]X[薪資比率]之數值

                    //此項若設定為[C.變動金額]時
                    //若項目別為[0.系統設定],則使用系統內建公式計算(例如:勞健保、所得稅)
                    //若項目別為[1.固定項目]或[2.變動項目],
                    //將依使用薪資結構參數中[公式]與[參數]中之設定進行運算
                    //若[公式]選擇為[不選擇公式],則依系統內建之函式運算
                    //(若非[勞健保]、[所得稅]等固定計算方式,則需客製化)
                    #endregion
                    //[項目別]
                    sSalaryType = theItemDT.Rows[0]["SalaryType"].ToString().Trim();
                    if (sSalaryType.Equals("A"))
                    {//A- 固定金額
                        if (sItemType.Equals("0"))
                            decSalaryItem = Convert.ToDecimal(theItemDT.Rows[0]["FixedAmount"].ToString());
                        if (decSalaryItem == 0)
                        {
                            try
                            {
                                ////先取得個人薪資項目設定之金額
                                //strSQL = "Select " + PayrollLt.DeCodeKey + "(Amount) As Amount from Payroll_Master_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And SalaryItem ='" + SalaryId + "'";
                                //DataTable theItemAmount = _MyDBM.ExecuteDataTable(strSQL);
                                //decPersonalFixedAmount = (Decimal)theItemAmount.Rows[0][0];

                                #region
                                strSQL = "Select " + PayrollLt.DeCodeKey + "(Amount) As Amount from Payroll_Master_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And SalaryItem ='" + SalaryId + "'";
                                DataTable theItemAmount = _MyDBM.ExecuteDataTable(strSQL);
                                if (theItemAmount != null && theItemAmount.Rows.Count > 0)
                                    decPersonalFixedAmount = (Decimal)theItemAmount.Rows[0][0];
                                else
                                    decPersonalFixedAmount = 0;

                                strSQL = "Select (Case When SubString(Convert(varchar,[EffectiveDate],112),1,6) ='" + PayrollLt.SalaryYM.ToString() + "' Then datepart(d,[EffectiveDate]) Else 0 End) Days " +
                                    " ,(Case When SubString(Convert(varchar,[EffectiveDate],112),1,6) > '" + PayrollLt.SalaryYM.ToString() + "' then 1 Else (Case When Convert(varchar,[EffectiveDate],112) > '" + PayrollLt.SalaryYM.ToString() + "01' then 0 Else -1 end) end) ADate " +
                                    " ," + PayrollLt.DeCodeKey + "(OldlSalary) As OldlSalary, " + PayrollLt.DeCodeKey + "(NewSalary) As NewSalary " +
                                    " from [AdjustSalary_Master] Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And [AdjustSalaryItem] ='" + SalaryId + "'" +
                                    " And Convert(varchar,[ApproveDate],112)<>'19120101' And [EffectiveDate]<=Getdate() " +
                                    " Order By (Case When SubString(Convert(varchar,[EffectiveDate],112),1,6)>'" + PayrollLt.SalaryYM.ToString() + "' then Convert(varchar,[EffectiveDate],112) else 0 end),EffectiveDate Desc";
                                DataTable ASDT = _MyDBM.ExecuteDataTable(strSQL);

                                try
                                {
                                    if (ASDT != null)
                                    {
                                        if (ASDT.Rows.Count > 0)
                                        {//破月計算調薪
                                            int iDays = (int)ASDT.Rows[0]["Days"];
                                            int iADate = (int)ASDT.Rows[0]["ADate"];
                                            decimal decAD = 0;
                                            if ((iADate == 0 && iDays >= 30) || iADate > 0)
                                            {
                                                decAD = (Decimal)ASDT.Rows[0]["OldlSalary"];
                                            }
                                            else if ((iADate == 0 && iDays == 1) || iADate < 0)
                                            {
                                                decAD = (Decimal)ASDT.Rows[0]["NewSalary"];
                                            }
                                            else 
                                            {
                                                decAD = (Decimal)ASDT.Rows[0]["OldlSalary"] * ((Decimal)iDays / 30) + (Decimal)ASDT.Rows[0]["NewSalary"] * ((Decimal)(30 - iDays) / 30);
                                            }

                                            decPersonalFixedAmount = Math.Round(decAD, 0);
                                        }
                                    }
                                }
                                catch
                                {
                                    _UserInfo.SysSet.WriteToLogs("PYMsg", "調薪資料有誤\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + SalaryId + "|SalaryYM:" + PayrollLt.SalaryYM.ToString() + "\n\r");
                                }
                                #endregion
                            }
                            catch { }
                            decSalaryItem = decPersonalFixedAmount;
                        }
                    }
                    else if (sSalaryType.Equals("B"))
                    {//B- 薪資比率
                        decSalaryItem = Convert.ToDecimal(GetSalaryItem(PayrollLt, theItemDT.Rows[0]["BaseItem"].ToString().Trim(), decPersonalFixedAmount)) * Convert.ToDecimal(theItemDT.Rows[0]["SalaryRate"].ToString().Trim());
                    }
                    else
                    {//C- 變動金額              
                        if (theItemDT.Rows[0]["ParameterList"].ToString().Contains("$" + SalaryId))
                        {
                            _UserInfo.SysSet.WriteToLogs("GetSalaryItem", "薪資項目[" + SalaryId + "]計算公式之參數(" + theItemDT.Rows[0]["ParameterList"].ToString().Trim() + ")設定有誤!\n於參數中將薪資項目設為參數之一，將造成無窮迴圈!!");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(theItemDT.Rows[0]["FunctionId"].ToString().Trim()))
                                decSalaryItem = Convert.ToDecimal(GetSalaryFormula(PayrollLt, theItemDT.Rows[0]["FunctionId"].ToString().Trim(), theItemDT.Rows[0]["ParameterList"].ToString().Trim()));
                            else
                            {
                                //計算在職天數
                                int LIdays = Convert.ToInt32(GetPaydays(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString()));
                                switch (SalaryId)
                                {
                                    case "03"://代扣稅款金額
                                        IncomeTax.Tax mTax = IncomeTax.GetIncomeTax(decPersonalFixedAmount, PayrollLt);
                                        decSalaryItem = Convert.ToDecimal(mTax.Amount);
                                        break;
                                    case "04"://健保
                                        HealthInsurance.DependentPremium HI = HealthInsurance.GetPremium(PayrollLt);
                                        decSalaryItem = Convert.ToDecimal(HI.Insured + HI.Dependent);
                                        break;
                                    case "05"://勞保
                                        LaborInsurance.Insurance LI = LaborInsurance.GetPremium(PayrollLt);
                                        decSalaryItem = Convert.ToDecimal(LI.Insured);
                                        break;
                                    case "06"://退休金自提
                                        RetirementPension.Pensionaccounts RP = RetirementPension.GetMonthlyActualSalary(PayrollLt, LIdays);
                                        decSalaryItem = Convert.ToDecimal(RP.Insured);
                                        break;
                                    case "07"://福利金
                                        decSalaryItem = Convert.ToDecimal(FringeBenefits.GetFringeBenefits(PayrollLt));
                                        break;
                                    case "08"://應稅加班
                                        OverTimes OverTimeFee8 = GetWTOverTime(PayrollLt);
                                        PayrollLt.WT_Overtime = OverTimeFee8.WT_Overtime;
                                        PayrollLt.WT_Overtime_Fee = OverTimeFee8.WT_Overtime_Fee;
                                        decSalaryItem = OverTimeFee8.WT_Overtime_Fee;
                                        break;
                                    case "09"://免稅加班
                                        OverTimes OverTimeFee9 = GetWTOverTime(PayrollLt);
                                        PayrollLt.NT_Overtime = OverTimeFee9.NT_Overtime;
                                        PayrollLt.NT_Overtime_Fee = OverTimeFee9.NT_Overtime_Fee;
                                        decSalaryItem = OverTimeFee9.NT_Overtime_Fee;
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ex.Message
                    _UserInfo.SysSet.WriteToLogs("GetSalaryItem", ex.Message);
                    decSalaryItem = 0;
                }

                if (theItemDT.Rows[0]["PMType"].ToString().Equals("B"))
                {//加項:A; 減項:B                           
                    decSalaryItem = 0 - decSalaryItem;
                }
            }
        }

        return decSalaryItem.ToString();
    }
    
    /// <summary>
    /// 解析薪資項目公式
    /// </summary>
    /// <param name="strFunction">公式</param>
    /// <param name="strParameter">參數陣列</param>
    /// <returns>解析後的運算式</returns>
    public string GetSalaryFormula(PayrolList PayrollLt, string strFunctionId, string strParameter)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        strSQL = "Select CodeName from CodeDesc Where CodeID='Function' And CodeCode='" + strFunctionId + "' ";
        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        string strFunction = "", strFormula = "";

        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                strFunction = theDT.Rows[0][0].ToString();
                if (strFunction.IndexOf(":") > 0)
                {//區分出公式(公式說明:公式)
                    strFunction = strFunction.Substring(strFunction.IndexOf(":") + 1);
                }

                string[] strParaList = strParameter.Split(',');

                for (int i = 0; i < strParaList.Length; i++)
                {
                    if (strParaList[i].Contains("$"))
                    {
                        strParaList[i] = GetSalaryItem(PayrollLt, strParaList[i].Replace("$", ""));
                    }
                }

                //傳回計算結果
                return computer(GetFormula(strFunction, strParaList));
            }
        }

        return strFormula;
    }

    /// <summary>
    /// 在職天數(未扣除請假天數;本月天數大於實際在職天數時,需扣除假日,僅以實際上班天數計算)
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <param name="YearMonth">計薪年月</param>
    /// <returns>在職天數(未扣除請假天數)</returns>
    private Decimal GetPaydays(string Company, string personalID, string YearMonth)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        Decimal iPaydays = 0;

        strSQL = "Select * " +
            "from Personnel_Master Where Company='" + Company + "' And EmployeeId='" + personalID + "' And IsNull(ResignCode,'')<>'Y' ";
        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                DateTime tempStratDate = thisYMStratDate;
                DateTime tempEndDate = thisYMEndDate;
                string tempResignCode = theDT.Rows[0]["ResignCode"].ToString();
                int Adyear = 0;
                iPaydays = thisYMPayDate;
                //2012/03/01 2月份計薪時,以30天為基數
                int theMounthdays = tempEndDate.Day;

                try
                {
                    DateTime theHireDate = Convert.ToDateTime(theDT.Rows[0]["HireDate"].ToString());
                    Adyear = (theHireDate.Year < 1000) ? 1911 : 0;
                    //本月到職
                    if (((theHireDate.Year + Adyear).ToString() + theHireDate.Month.ToString().PadLeft(2, '0')).Equals(YearMonth))
                    {
                        tempStratDate = Convert.ToDateTime((theHireDate.Year + Adyear).ToString() + "/" + theHireDate.Month.ToString().PadLeft(2, '0') + "/" + theHireDate.Day.ToString().PadLeft(2, '0'));
                    }
                }
                catch (Exception ex)
                {
                    //ex.Message
                    _UserInfo.SysSet.WriteToLogs("Error", "GetPaydays:本月到職日\n\r" + "Comp:" + Company + "|Emp:" + personalID + "|YM:" + YearMonth + "\n\r" + ex.Message);
                }

                if (tempResignCode != "N")
                {//已離職或離職待生效才要找出離職日
                    try
                    {
                        DateTime theResignDate = Convert.ToDateTime(theDT.Rows[0]["ResignDate"].ToString());
                        Adyear = (theResignDate.Year < 1000) ? 1911 : 0;
                        if (((theResignDate.Year + Adyear) * 100 + theResignDate.Month) <= Convert.ToInt32(YearMonth))
                        {//本月或之前月份離職
                            tempEndDate = Convert.ToDateTime((theResignDate.Year + Adyear).ToString() + "/" + theResignDate.Month.ToString().PadLeft(2, '0') + "/" + theResignDate.Day.ToString().PadLeft(2, '0'));
                        }
                    }
                    catch (Exception ex)
                    {
                        //ex.Message
                        //已離職卻沒有離職日
                        _UserInfo.SysSet.WriteToLogs("Error", "GetPaydays:本月到職日\n\r" + "Comp:" + Company + "|Emp:" + personalID + "|YM:" + YearMonth + "\n\r已離職但查無離職日!" + ex.Message);
                    }
                }

                if (tempStratDate.Month != tempEndDate.Month)
                {//2011/12/26 A.修正計薪跨月時天數不正確問題
                    tempEndDate = tempStratDate.AddMonths(1);
                    tempEndDate = tempEndDate.AddDays(0 - tempEndDate.Day);
                }

                //if (iPaydays > (tempEndDate.DayOfYear - tempStratDate.DayOfYear + 1))
                if (theMounthdays > (tempEndDate.DayOfYear - tempStratDate.DayOfYear + 1))
                {//本月天數大於實際在職天數時
                    int iHolidayDays = 0;                    
                    //2011/08/05 依勞基法規定,不扣除假日
                    //扣除假日,僅以實際上班天數計算
                    //strSQL = "Select Count(*) from Holiday Where Company='" + Company + "' " +
                    //" And Convert(varchar,HolidayDate,111) Between '" + tempStratDate.ToString("yyyy/MM/dd") + "' And '" + tempEndDate.ToString("yyyy/MM/dd") + "' ";
                    //DataTable theTempDT = _MyDBM.ExecuteDataTable(strSQL);                    
                    //if (theTempDT!=null)
                    //    if (theTempDT.Rows.Count>0)
                    //        iHolidayDays = (int)theTempDT.Rows[0][0];

                    //2012/03/08 依財務要求修改:破月到職者，日薪是以30天為基數計算，例: 2/10到職，則發放( 30-10+1)=21天薪資
                    //2012/03/08 與財務確認若為計薪當月月底前離職者，日薪基數雖仍為３０天，但在職天數以實際在職天數計算
                    //iPaydays = (tempEndDate.DayOfYear - tempStratDate.DayOfYear + 1) - iHolidayDays;
                    iPaydays = (tempEndDate.DayOfYear - tempStratDate.DayOfYear + 1); 
                    if (iPaydays > thisYMEndDate.Day)
                        iPaydays = 30 - tempStratDate.Day + 1;
                    if (iPaydays < 0) iPaydays = 0;
                }
            }          
        }

        return iPaydays;
    }

    /// <summary>
    /// 取得本月未確認的計薪控管年月,若無則新增
    /// </summary>
    /// <param name="Company">公司</param>
    /// <returns>計薪控管年月</returns>
    public static SalaryYearMonth GetSalaryYM(string Company)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        SalaryYearMonth sYM = new SalaryYearMonth();

        sYM.Company = Company;
        sYM.SalaryYM = Convert.ToInt32(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0'));
        if (!string.IsNullOrEmpty(Company))
        {
            string strSQL = "Select * from PayrollControl Where Company='" + Company + "' And SalaryYM=" + sYM.SalaryYM.ToString() + " Order By PeriodCode";
            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                sYM.isControl = true;
                if (theDT.Rows.Count == 0)
                {
                    sYM.PeriodCode = "0";
                    strSQL = "Insert Into PayrollControl (Company,SalaryYM,PeriodCode) Select '" + Company + "'," + sYM.SalaryYM.ToString() + ",'" + sYM.PeriodCode + "'";
                    _MyDBM.ExecuteCommand(strSQL);
                }
                else
                {
                    for (int i = 0; i < theDT.Rows.Count; i++)
                    {
                        sYM.PeriodCode = theDT.Rows[i]["PeriodCode"].ToString();
                        try
                        {
                            sYM.DraftDate = ((DateTime)theDT.Rows[i]["DraftDate"]).ToString("yyyy/MM/dd");
                        }
                        catch { }
                        try
                        {
                            sYM.ConfirmDate = ((DateTime)theDT.Rows[i]["ConfirmDate"]).ToString("yyyy/MM/dd");
                        }
                        catch
                        {
                            //取得未確認的資料即可
                            i = theDT.Rows.Count;
                        }
                    }
                }
            }
        }
        return sYM;
    }

    /// <summary>
    /// 確認控管中的計薪年月
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="SalaryYM"></param>
    /// <param name="Period"></param>
    /// <returns></returns>
    public static SalaryYearMonth CheckSalaryYM(string Company, string SalaryYM, string Period)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        SalaryYearMonth sYM = new SalaryYearMonth();

        sYM.Company = Company;
        sYM.SalaryYM = Convert.ToInt32(SalaryYM);
        sYM.PeriodCode = Period;
        sYM.isControl = false;
        sYM.LastConfirmYM = 0;
        sYM.NextConfirmYM = 0;

        string strSQL = "Select IsNull(Max([SalaryYM]),0) from PayrollControl Where [Company] = '" + Company + "' And Not([ConfirmDate] is Null)";
        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                sYM.LastConfirmYM = Convert.ToInt32(theDT.Rows[0][0].ToString());                
                if (sYM.LastConfirmYM.ToString().EndsWith("12"))
                    sYM.NextConfirmYM = sYM.LastConfirmYM + 89;
                else
                    sYM.NextConfirmYM = sYM.LastConfirmYM + 1;
            }
        }

        strSQL = "Select *,(Case When IsNull((Select Top 1 [DraftDate] From [PayrollControl] LPC where LPC.[Company]=PC.[Company] And LPC.[SalaryYM] in (PC.[SalaryYM]-1,PC.[SalaryYM]-89) And LPC.[PeriodCode]=PC.[PeriodCode] " +
            " Order by [SalaryYM] Desc),[DraftDate]) >= [DraftDate] OR " +
            " IsNull((Select Top 1 [ConfirmDate] From [PayrollControl] LPC where LPC.[Company]=PC.[Company] And LPC.[SalaryYM] in (PC.[SalaryYM]-1,PC.[SalaryYM]-89) And LPC.[PeriodCode]=PC.[PeriodCode] " +
            " Order by [SalaryYM] Desc),[DraftDate]) >= [DraftDate] " +
            " then (Case When (Select Count(*) From [PayrollControl] LPC where LPC.[Company]=PC.[Company] And LPC.[SalaryYM] < PC.[SalaryYM] ) > 0 " +
            " then 'Y' else 'N' End ) else 'N' End) As beReDraft " +
            " from [PayrollControl] PC Where Company='" + Company + "' And SalaryYM=" + SalaryYM + " And PeriodCode='" + Period + "' ";
        theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                sYM.beReDraft = theDT.Rows[0]["beReDraft"].ToString().Equals("Y");
                try
                {
                    sYM.DraftDate = ((DateTime)theDT.Rows[0]["DraftDate"]).ToString("yyyy/MM/dd");
                }
                catch { }
                try
                {
                    sYM.ConfirmDate = ((DateTime)theDT.Rows[0]["ConfirmDate"]).ToString("yyyy/MM/dd");
                }
                catch { }
                sYM.isControl = true;
            }
        }

        return sYM;
    }

    /// <summary>
    /// 取得最後一筆計薪年月
    /// </summary>
    /// <param name="Company"></param>
    /// <returns></returns>
    public static SalaryYearMonth GetLastSalaryYM(string Company)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        SalaryYearMonth sYM = new SalaryYearMonth();

        sYM.Company = Company;
        sYM.SalaryYM = 0;
        sYM.isControl = false;

        string strSQL = "Select * from PayrollControl Where Company='" + Company + "' And Not(DraftDate is Null)  order by SalaryYM Desc, Period Desc";
        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                sYM.SalaryYM = (int)theDT.Rows[0]["SalaryYM"];
                sYM.PeriodCode = theDT.Rows[0]["Period"].ToString();
                sYM.DraftDate = ((DateTime)theDT.Rows[0]["DraftDate"]).ToString("yyyy/MM/dd");
                sYM.ConfirmDate = ((DateTime)theDT.Rows[0]["ConfirmDate"]).ToString("yyyy/MM/dd");
                sYM.isControl = true;
            }
        }

        return sYM;
    }

    /// <summary>
    /// 請假扣薪時數(已換算基數)
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <param name="YearMonth">薪資處理月份</param>
    /// <returns>請假扣薪時數(已換算基數)</returns>
    public int GetLeaveHours(string Company, string personalID, string YearMonth)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        int iLeaveHours = 0;
        #region 取得請假時數
        //薪資處理月份:Payroll_Processingmonth=>由輸入請假資料時控管
        //2011/08/01 kaya 加入天數控管：計薪天數-IsNull([Pay_days],0),計薪比例-IsNull([Pay_rate],100),全年可請-IsNull([Annual_LeaveDays],999)
        string strPayDays = " (IsNull([Annual_LeaveDays],0)*8) ";
        string strPayRate = " (Case When IsNull(Pay_rate,0)=0 Then 0 Else Pay_rate/100 End) ";

        //計薪方式為[不計薪]時：未超過可請天數時，依計薪比例計算；超過後扣全薪
        //計薪方式為[計薪]時：未超過可請天數時，不扣薪；超過後依計薪比例計算
        //原本使用[ApproveDate]推算是否已發薪,但實際上不太可行,故仍作為是否核準之用
        strSQL = "Select Sum((Case SalaryType When 'N' "+
            " Then (Case When TotalLHs > " + strPayDays +
            "  Then (Case When bfLHs < " + strPayDays +
            "        Then (TotalLHs-" + strPayDays + ")+(" + strPayDays + "-bfLHs)*" + strPayRate +
            "        Else LHs"+
            "        End) " +
            "  Else LHs*" + strPayRate +
            "  End) " +

            " Else (Case When TotalLHs > " + strPayDays +
            "  Then (Case When bfLHs < " + strPayDays +
            "        Then (TotalLHs-" + strPayDays + ")*" + strPayRate +
            "        Else LHs*" + strPayRate +
            "        End) " +
            "  Else 0 "+
            "  End)" +
            " End)) As [LeaveHours] " +
            " from (" +
            " Select IsNull(thisYM.LeaveHours,0) LHs,IsNull(bfLeaveHours,0) bfLHs, (IsNull(thisYM.LeaveHours,0)+IsNull(bfLeaveHours,0)) TotalLHs,LB.* from " +
            //計薪月份當月所請天數
            " (select Company,EmployeeId,LeaveType_Id,(sum(hours)+sum(days)*8) LeaveHours " +
            " from Leave_Trx Where Company='" + Company + "' And EmployeeId='" + personalID + "' " +
            " And Payroll_Processingmonth=" + YearMonth +
            " And Not(ApproveDate is null) " +
            " group by Company,EmployeeId,LeaveType_Id) thisYM" +
            " Left Join" +
            //計薪月份之前當年度已請天數
            " (select Company,EmployeeId,LeaveType_Id,(sum(hours)+sum(days)*8) bfLeaveHours " +
            " from Leave_Trx Where Company='" + Company + "' And EmployeeId='" + personalID + "' " +
            " And Payroll_Processingmonth Between " + YearMonth.Substring(0,4) +"00 And " + YearMonth +
            " And Not(ApproveDate is null) " +
            " group by Company,EmployeeId,LeaveType_Id) BfYM " +
            " On thisYM.Company=BfYM.Company and thisYM.EmployeeId=BfYM.EmployeeId and thisYM.LeaveType_Id=BfYM.LeaveType_Id " +
            " Left join LeaveType_Basic LB on thisYM.Company=LB.Company And thisYM.LeaveType_Id=LB.Leave_Id " +
            " ) LT";

        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                    iLeaveHours = (int)Convert.ToDecimal(theDT.Rows[0][0].ToString());
            }
        }
        #endregion

        #region 取得曠職時數
        strSQL = "Select count(*) " +
        " from AttendanceException " +
        " Where Company='" + Company + "' And EmployeeId='" + personalID + "' " +
        " And SubString(Convert(char,AttendanceDate,112),1,6)='" + YearMonth + "'" +
        " AND [AECode]='9' ";

        theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                    if (Convert.ToDecimal(theDT.Rows[0][0].ToString()) > 0)
                        iLeaveHours += Convert.ToInt32(8 * Convert.ToDecimal(theDT.Rows[0][0].ToString()));
            }
        }
        #endregion

        return iLeaveHours;
    }

    /// <summary>
    /// 實際請假時數
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <param name="YearMonth">薪資處理月份</param>
    /// <param name="kind">N:扣薪時數;Y:不扣薪時數</param>
    /// <returns>實際請假時數(扣薪或不扣薪)</returns>
    private int GetLeaveHoursDeduction(string Company, string personalID, string YearMonth, string kind)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        int iLeaveHours = 0;        

        //薪資處理月份:Payroll_Processingmonth=>由輸入請假資料時控管
        strSQL = "Select SUM(Leavedays * (Case SalaryType When '" + kind + "' Then 1 Else 0 End)) Leavedays " +
                " from  (select Company,EmployeeId,LeaveType_Id,(sum(hours)+sum(days)*8) Leavedays " +
                "        from Leave_Trx Where Company='" + Company + "' And EmployeeId='" + personalID + "' " +
                "        And Payroll_Processingmonth=" + YearMonth + " And Not(ApproveDate is null) " +
                "        group by Company,EmployeeId,LeaveType_Id) LT " +
                " Left join LeaveType_Basic LB on LT.Company=LB.Company And LT.LeaveType_Id=LB.Leave_Id ";

        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                    iLeaveHours = (int)Convert.ToDecimal(theDT.Rows[0][0].ToString());
            }
        }

        return iLeaveHours;
    }

    /// <summary>
    /// 計算加班時薪基數(實際加班時數*薪資比率) 依先進先出法計算免稅時數
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <param name="YearMonth">薪資處理月份</param>
    /// <returns>加班時薪基數(實際加班時數*薪資比率)</returns>
    public OverTimes GetWTOverTime1(PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        decimal[] iOverTime = { 0, 0, 0, 0 };

        //薪資處理月份:Payroll_Processingmonth=>由輸入請假資料時控管
        strSQL = " Select (OT1+OT2+OT3+OT4) OTHs, " +
                " (Case When (OT1+OT2+OT3+OT4) < (Case Sex When 'M' then OT_Para1 else OT_Para2 end) " +
                " Then (OT1*OT_Para3+OT2*OT_Para4+OT3*OT_Para5+OT4*OT_Para6) Else -1 end) OverLT " +
                " From ( " +
                " Select Company,EmployeeId, " +
                " IsNull(Sum([OverTime1]),0) OT1,IsNull(Sum([OverTime2]),0) OT2, IsNull(Sum([Holiday]),0) OT3, IsNull(Sum([NationalHoliday]),0) OT4 " +
                " from  OverTime_Trx " +
                " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                " And (Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
                "      Or (IsNull(Payroll_Processingmonth,0)<" + PayrollLt.SalaryYM.ToString() + " And IsNull(Completed,'')!='Y' )) " +
                " And Not(ApproveDate is null) And Overtime_pay = 'Y' " +
                " group by Company,EmployeeId " +
                " ) OT Left Join PersonnelSalary_Parameter PP On OT.Company = PP.Company " +
                " Left join Personnel_Master PM On OT.Company = PM.Company And OT.EmployeeId = PM.EmployeeId "; 

        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                {
                    //免稅加班時薪基數
                    iOverTime[0] = (decimal)Convert.ToDecimal(theDT.Rows[0]["OverLT"].ToString());
                    //免稅加班時數
                    iOverTime[1] = (decimal)Convert.ToDecimal(theDT.Rows[0]["OTHs"].ToString());

                    if (iOverTime[0] == -1)
                    {//超過免稅加班時數.須另計
                        strSQL = " Select " +
                               " OverTime_Date ,(Case Sex When 'M' then OT_Para1 else OT_Para2 end) Limit " +
                               " ,OT1,OT2,OT3,OT4 " +
                               " ,OT_Para3 As OT1_P,OT_Para4 As OT2_P,OT_Para5 As OT3_P,OT_Para6 As OT4_P " +
                               " From ( " +
                               " Select Company,EmployeeId,OverTime_Date, " +
                               " IsNull(Sum([OverTime1]),0) OT1,IsNull(Sum([OverTime2]),0) OT2, IsNull(Sum([Holiday]),0) OT3, IsNull(Sum([NationalHoliday]),0) OT4 " +
                               " from  OverTime_Trx " +
                               " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                               " And (Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
                               "      Or (IsNull(Payroll_Processingmonth,0)<" + PayrollLt.SalaryYM.ToString() + " And IsNull(Completed,'')!='Y' )) " +
                               " And Not(ApproveDate is null) And Overtime_pay = 'Y' " +
                               " group by Company,EmployeeId,OverTime_Date " +
                               " ) OT Left Join PersonnelSalary_Parameter PP On OT.Company = PP.Company " +
                               " Left join Personnel_Master PM On OT.Company = PM.Company And OT.EmployeeId = PM.EmployeeId " +
                               " Order By Convert(varchar,OverTime_Date,120) ";

                        DataTable theDTOver = _MyDBM.ExecuteDataTable(strSQL);
                        if (theDTOver != null)
                        {
                            decimal Limit = (decimal)theDTOver.Rows[0]["Limit"];
                            decimal tempOT = 0, sumOT = 0;
                            iOverTime[0] = 0;
                            iOverTime[1] = 0;

                            for (int i = 0; i < theDTOver.Rows.Count; i++)
                            {
                                tempOT = (decimal)theDTOver.Rows[i]["OT1"] + (decimal)theDTOver.Rows[i]["OT2"] + (decimal)theDTOver.Rows[i]["OT3"] + (decimal)theDTOver.Rows[i]["OT4"];
                                if ((iOverTime[1] + tempOT) <= Limit)
                                {
                                    //免稅加班時薪基數
                                    for (int otI = 1; otI <= 4; otI++)
                                        iOverTime[0] += (decimal)theDTOver.Rows[i]["OT" + otI.ToString()] * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                    //免稅加班時數
                                    iOverTime[1] += tempOT;
                                }
                                else
                                {
                                    if (iOverTime[1] == Limit)
                                    {//應稅時數
                                        //應稅加班時薪基數
                                        for (int otI = 1; otI <= 4; otI++)
                                            iOverTime[2] += (decimal)theDTOver.Rows[i]["OT" + otI.ToString()] * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                        //應稅加班時數
                                        iOverTime[3] += tempOT;
                                    }
                                    else
                                    {//免稅時數未滿將滿
                                        for (int otI = 1; otI <= 4; otI++)
                                        {
                                            tempOT = (decimal)theDTOver.Rows[i]["OT" + otI.ToString()];
                                            if ((iOverTime[1] + tempOT) <= Limit)
                                            {
                                                //免稅加班時薪基數
                                                iOverTime[0] += tempOT * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                                //免稅加班時數
                                                iOverTime[1] += tempOT;
                                            }
                                            else
                                            {
                                                if (iOverTime[1] == Limit)
                                                {//應稅時數
                                                    //應稅加班時薪基數
                                                    iOverTime[2] += (decimal)theDTOver.Rows[i]["OT" + otI.ToString()] * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                                    //應稅加班時數
                                                    iOverTime[3] += tempOT;
                                                }
                                                else
                                                {//免稅時數未滿將滿
                                                    tempOT = tempOT - (Limit - iOverTime[1]);
                                                    //免稅加班時薪基數
                                                    iOverTime[0] += (Limit - iOverTime[1]) * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                                    //免稅加班時數
                                                    iOverTime[1] = Limit;
                                                    //應稅加班時薪基數
                                                    iOverTime[2] += tempOT * (decimal)theDTOver.Rows[i]["OT" + otI.ToString() + "_P"];
                                                    //應稅加班時數
                                                    iOverTime[3] += tempOT;
                                                }
                                            } 
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        OverTimes OTs= new OverTimes();
        OTs.OvertimeBasic = 0;

        //取得時薪
        OTs.OvertimeBasic = GetPersonalHoursSalary(PayrollLt);
        //免稅加班
        OTs.NT_Overtime = iOverTime[0];
        OTs.NT_Overtime_Fee = Math.Ceiling(OTs.OvertimeBasic * iOverTime[0]);
        //應稅加班
        OTs.WT_Overtime = iOverTime[2];
        OTs.WT_Overtime_Fee = Math.Ceiling(OTs.OvertimeBasic * iOverTime[2]);

        return OTs;
    }

    /// <summary>
    /// 計算加班時薪基數(實際加班時數*薪資比率) 依優惠計法計算免稅時數
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <param name="YearMonth">薪資處理月份</param>
    /// <returns>加班時薪基數(實際加班時數*薪資比率)</returns>
    public OverTimes GetWTOverTime(PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        decimal[] iOverTime = { 0, 0, 0, 0 };

        //薪資處理月份:Payroll_Processingmonth=>由輸入請假資料時控管
        strSQL = " Select (OT1+OT2+OT3+OT4) OTHs, " +
            //20120604 kaya 修正:男免稅加班時數因性別判斷有誤問題
                " (Case When (OT1+OT2+OT3+OT4) < (Case When Sex ='M' Or Sex ='1' then OT_Para1 else OT_Para2 end) " +
                " Then (OT1*OT_Para3+OT2*OT_Para4+OT3*OT_Para5+OT4*OT_Para6) Else -1 end) OverLT " +
                " From ( " +
                " Select Company,EmployeeId, " +
                " IsNull(Sum([OverTime1]),0) OT1,IsNull(Sum([OverTime2]),0) OT2, IsNull(Sum([Holiday]),0) OT3, IsNull(Sum([NationalHoliday]),0) OT4 " +
                " from  OverTime_Trx " +
                " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                " And (Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
                "      Or (IsNull(Payroll_Processingmonth,0)<" + PayrollLt.SalaryYM.ToString() + " And IsNull(Completed,'')!='Y' )) " +
                " And Not(ApproveDate is null) And Overtime_pay = 'Y' " +
                " group by Company,EmployeeId " +
                " ) OT Left Join PersonnelSalary_Parameter PP On OT.Company = PP.Company " +
                " Left join Personnel_Master PM On OT.Company = PM.Company And OT.EmployeeId = PM.EmployeeId ";

        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                {
                    //免稅加班時薪基數
                    iOverTime[0] = (decimal)Convert.ToDecimal(theDT.Rows[0]["OverLT"].ToString());
                    //免稅加班時數
                    iOverTime[1] = (decimal)Convert.ToDecimal(theDT.Rows[0]["OTHs"].ToString());

                    if (iOverTime[0] == -1)
                    {//超過免稅加班時數.須另計
                        strSQL = " Select " +
                               " (Case Sex When 'M' then OT_Para1 else OT_Para2 end) Limit " +
                               " , OT1, OT2, OT3, OT4, (OT1+OT2+OT3+OT4) as OTsum " +
                               " ,OT_Para3 As OT1_P,OT_Para4 As OT2_P,OT_Para5 As OT3_P,OT_Para6 As OT4_P " +
                               " From ( " +
                               " Select Company,EmployeeId, " +
                               " IsNull(Sum([OverTime1]),0) OT1,IsNull(Sum([OverTime2]),0) OT2, IsNull(Sum([Holiday]),0) OT3, IsNull(Sum([NationalHoliday]),0) OT4 " +
                               " from  OverTime_Trx " +
                               " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                               " And (Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
                               "      Or (IsNull(Payroll_Processingmonth,0)<" + PayrollLt.SalaryYM.ToString() + " And IsNull(Completed,'')!='Y' )) " +
                               " And Not(ApproveDate is null) And Overtime_pay = 'Y' " +
                               " group by Company,EmployeeId " +
                               " ) OT Left Join PersonnelSalary_Parameter PP On OT.Company = PP.Company " +
                               " Left join Personnel_Master PM On OT.Company = PM.Company And OT.EmployeeId = PM.EmployeeId " +
                               " Order By Limit ";

                        DataTable theDTOver = _MyDBM.ExecuteDataTable(strSQL);
                        if (theDTOver != null)
                        {
                            //取得加班時數上限
                            decimal Limit = (decimal)theDTOver.Rows[0]["Limit"];
                            decimal sumOT = 0, tempOT = 0;
                            
                            int[] thePlist = new int[] { 1, 2, 3, 4 };
                            int tempOTP = 0;

                            iOverTime[0] = 0;
                            iOverTime[1] = 0;

                            //排列加班時薪倍數的大小
                            for (int i = 0; i < 3; i++)                            
                            {
                                for (int j = i + 1; j < 4; j++)
                                {
                                    if ((decimal)theDTOver.Rows[0]["OT" + thePlist[i].ToString() + "_P"] < (decimal)theDTOver.Rows[0]["OT" + thePlist[j].ToString() + "_P"])
                                    {
                                        tempOTP = thePlist[i];
                                        thePlist[i] = thePlist[j];
                                        thePlist[j] = tempOTP;
                                    }
                                }
                            }
                                                                                    
                            //取得剩餘加班時數                            
                            sumOT = Limit;

                            for (int i = 0; i < thePlist.Length; i++)
                            {
                                //取得各基數之加班時數
                                tempOT = (decimal)theDTOver.Rows[0]["OT" + thePlist[i].ToString()];
                                if (sumOT > tempOT)
                                {
                                    sumOT -= tempOT;
                                    //免稅加班時薪基數
                                    iOverTime[0] += tempOT * (decimal)theDTOver.Rows[0]["OT" + thePlist[i].ToString() + "_P"];
                                    //免稅加班時數
                                    iOverTime[1] += tempOT;
                                }
                                else
                                {
                                    //免稅加班時薪基數
                                    iOverTime[0] += sumOT * (decimal)theDTOver.Rows[0]["OT" + thePlist[i].ToString() + "_P"];
                                    //免稅加班時數
                                    iOverTime[1] += sumOT;
                                    
                                    tempOT -= sumOT; 
                                    sumOT = 0;                                   

                                    //應稅加班時薪基數
                                    iOverTime[2] += tempOT * (decimal)theDTOver.Rows[0]["OT" + thePlist[i].ToString() + "_P"]; 
                                    //應稅加班時數
                                    iOverTime[3] += tempOT;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        OverTimes OTs = new OverTimes();
        //取得時薪        
        PayrollLt.PeriodCode = "A";
        OTs.OvertimeBasic1 = GetPersonalHoursSalary(PayrollLt);
        PayrollLt.PeriodCode = "B";
        OTs.OvertimeBasic2 = GetPersonalHoursSalary(PayrollLt);
        PayrollLt.PeriodCode = "0";
        if ((OTs.OvertimeBasic1 + OTs.OvertimeBasic2) > 0)
        {
            if (OTs.OvertimeBasic1 < 0)
                OTs.OvertimeBasic1 = 0;
            if (OTs.OvertimeBasic2 < 0)
                OTs.OvertimeBasic2 = 0;
            OTs.OvertimeBasic = (OTs.OvertimeBasic1 + OTs.OvertimeBasic2);
        }
        else
        {
            OTs.OvertimeBasic = GetPersonalHoursSalary(PayrollLt);
        }
        //免稅加班
        if (iOverTime[1] > 0)
        {
            OTs.NT_Overtime = iOverTime[1];
            OTs.NT_Overtime_Fee = Math.Ceiling(OTs.OvertimeBasic * iOverTime[0]);
        }
        //應稅加班
        if (iOverTime[3] > 0)
        {
            OTs.WT_Overtime = iOverTime[3];
            OTs.WT_Overtime_Fee = Math.Ceiling(OTs.OvertimeBasic * iOverTime[2]);
        }
        return OTs;
    }

    /// <summary>
    /// 個人時薪
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <returns>回傳個人時薪</returns>
    public decimal GetPersonalHoursSalary(PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        decimal iOvertimeBasic = 0;

        string tempValue = PayrollLt.PeriodCode;
        string sqlDefItem = _UserInfo.SysSet.GetConfigString("VersionID");
        if (sqlDefItem.Contains("PICCustomization"))
        {
            if (tempValue.Equals("A"))
            {
                tempValue = " And [P1CostSalaryItem] <> 'B'";
            }
            else if (tempValue.Equals("B"))
            {
                tempValue = " And [P1CostSalaryItem] <> 'A'";
            }
            else
            {
                tempValue = "";
            }
        }
        else
        {
            if (tempValue.Equals("0") || tempValue.Equals("C"))
            {
                tempValue = "";
            }
            else
            {
                tempValue = "And 0=1";
            }
        }

        strSQL = " select * from Payroll_Master_Detail " +
            " where SalaryItem in (select CodeCode from CodeDesc join SalaryStructure_Parameter On CodeCode=[SalaryId] " +
            "                      where CodeID='PY#HoursBI' " + tempValue + ") " +
            " And Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' ";
        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            string sSalaryItem = "";            
            for (int i = 0; i < theDT.Rows.Count; i++)
            {
                sSalaryItem = theDT.Rows[i]["SalaryItem"].ToString().Trim();
                //使用薪資參數結構找出實際的值(含破月計算)
                try
                {
                    iOvertimeBasic += (int)Convert.ToDecimal(GetPersonalSalaryItem(PayrollLt, sSalaryItem));
                }
                catch (Exception ex)
                {
                    _UserInfo.SysSet.WriteToLogs("PYErr", "取得時薪資料有誤\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|SalaryItem:" + sSalaryItem + "|SalaryYM:" + PayrollLt.SalaryYM.ToString() + "\n\r" + ex.Message);
                }
            }

            //目前每月時數固定為除以30天,每天8小時;即240小時
            iOvertimeBasic = Math.Round(iOvertimeBasic / 240, 2);
        }        
        return iOvertimeBasic;
    }

    /// <summary>
    /// 取得全勤可領金額:儀測專用
    /// </summary>
    /// <param name="PayrollLt"></param>
    /// <param name="dAttendanceFee"></param>
    /// <returns></returns>
    public decimal GetAttendanceFee(PayrolList PayrollLt, decimal dAttendanceFee)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        
        decimal iAttendanceFee = dAttendanceFee;
        
        if (iAttendanceFee > 0)
        {//判斷是否整月在職
            string sY, sM;
            sY = PayrollLt.SalaryYM.ToString().Substring(0, 4);
            sM = PayrollLt.SalaryYM.ToString().Substring(4, 2);       
        
            DateTime tempStratDate = Convert.ToDateTime(sY + "/" + sM + "/01");

            if (PayrollLt.Paydays < tempStratDate.Day)
                iAttendanceFee = 0;          
        }

        if (iAttendanceFee > 0)
        {//判斷是否請假
            //薪資處理月份:Payroll_Processingmonth=>由輸入請假資料時控管
            strSQL = "Select SUM(Leavedays) Leavedays " +
                    " from  (select Company,EmployeeId,LeaveType_Id,(sum(hours)+sum(days)*8) Leavedays " +
                    "        from Leave_Trx Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                    "        And Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() + " And Not(ApproveDate is null) " +
                    "        group by Company,EmployeeId,LeaveType_Id) LT " +
                    " Left join LeaveType_Basic LB on LT.Company=LB.Company And LT.LeaveType_Id=LB.Leave_Id " +
                    " Where LB.[Attendance]='Y' ";

            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                        if (Convert.ToDecimal(theDT.Rows[0][0].ToString()) > 0)
                            iAttendanceFee = 0;
                }
            }
        }

        if (iAttendanceFee > 0)
        {//判斷是否遲到(1)或忘刷(5)
            strSQL = "Select count(*) " +
            " from AttendanceException " +
            " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
            " And Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
            " AND [AECode] in ('1','5') ";

            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    switch ((int)theDT.Rows[0][0])
                    {
                        case 0://全勤                            
                            break;
                        case 1://遲到一次扣600
                            iAttendanceFee -= 600;
                            break;
                        default://遲到二次以上無全勤
                            iAttendanceFee = 0;
                            break;
                    }                            
                }
            }
        }

        if (iAttendanceFee > 0)
        {//判斷是否曠職(9)
            strSQL = "Select count(*) " +
            " from AttendanceException " +
            " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
            " And Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
            " AND [AECode]='9' ";

            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                        if (Convert.ToDecimal(theDT.Rows[0][0].ToString()) > 0)
                            iAttendanceFee = 0;
                }
            }
        }

        return iAttendanceFee;
    }

    /// <summary>
    /// 取得其它扣款:泛太專用,目前用來扣遲到
    /// </summary>
    /// <param name="PayrollLt"></param>
    /// <param name="dOtherCharge">其它扣款</param>
    /// <returns></returns>
    public decimal GetOtherCharge(PayrolList PayrollLt, decimal dOtherCharge)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        int iOver0900 = 0, iOver0930 = 0, iOverTimes = 0;

        #region
        //取得遲到次數
        strSQL = "Select (Case When IsNull(OverTime,0) > 30 then 4 else (Case When IsNull(OverTime,0) > 0 then 1 else 0 end) end) " +
        " from AttendanceException " +
        " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
        " And Payroll_Processingmonth=" + PayrollLt.SalaryYM.ToString() +
        //2011/08/11原使用差勤代碼,但可能有請半天假或公出等情況;故改為依遲到分數做計算
        " AND [OverTime] > 0 ";

        try
        {
            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null && theDT.Rows.Count > 0)
            {
                for (int i = 0; i < theDT.Rows.Count; i++)
                {
                    switch ((int)theDT.Rows[0][0])
                    {
                        case 1:
                            iOver0900++;
                            break;
                        case 4:
                            iOver0930++;
                            break;
                        default:
                            break;
                    }
                }
                //取得遲到扣薪的基數(只算本薪,不計伙食)
                decimal dHourChage = Convert.ToDecimal(GetSalaryItem(PayrollLt, "01")) / 240;

                //2012/02/07 差勤規定:遲到扣款計算方式
                if (PayrollLt.SalaryYM < 201202)
                {
                    #region 201201及之前施行之差勤規定
                    if (iOver0930 == 0)
                    {//遲到皆未超過30分鐘時:遲到超過3次時,前2次不扣,但第3次扣4小時薪(半日薪),其後各扣1小時薪
                        if (iOver0900 < 3)
                            iOverTimes = 0;//遲到未達3次時,前2次不扣
                        else if (iOver0900 == 3)
                            iOverTimes = 4;//遲到超過3次時,前2次不扣,但第3次扣4小時薪(半日薪)
                        else
                            iOverTimes = iOver0900 + 1;//相當於每次遲到各扣1小時薪,並額外加扣1小時薪
                    }
                    else if (iOver0930 > 0)
                    {//遲到有超過30分鐘時:
                        if (iOver0900 < 3)
                            iOverTimes = iOver0930 * 4;//遲到未超過30分鐘未達3次時,只扣超過30分鐘的部分(每次扣4小時薪)
                        else
                            iOverTimes = iOver0930 * 4 + iOver0900 - 2;//遲到未超過30分鐘達3次時,減去2次不扣薪
                    }
                    #endregion
                }
                else
                {
                    #region 201202開始施行之差勤規定
                    if (PayrollLt.SalaryYM > 201202)
                    {//因為201202才開始,所以不累計201201的遲到次數
                        //找出上個月沒扣的遲到次數
                        PayrolList NewInPayrollLt = PayrollLt;
                        if (Convert.ToInt32(PayrollLt.SalaryYM.ToString().Substring(4)) > 1)
                            NewInPayrollLt.SalaryYM = PayrollLt.SalaryYM - 1;
                        else
                            NewInPayrollLt.SalaryYM = PayrollLt.SalaryYM - 89;
                        strSQL = "Select Count(Case When IsNull(OverTime,0) > 0 then 1 else 0 end) " +
                        " from AttendanceException " +
                        " Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                        " And Payroll_Processingmonth=" + NewInPayrollLt.SalaryYM.ToString() +
                            //2011/08/11原使用差勤代碼,但可能有請半天假或公出等情況;故改為依遲到分數做計算
                        " AND [OverTime] > 0 ";
                        DataTable theLastDT = _MyDBM.ExecuteDataTable(strSQL);
                        try
                        {
                            if ((int)theLastDT.Rows[0][0] > 3)
                                iOver0900 += (int)theLastDT.Rows[0][0] % 3;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    
                    //20120920 遲到扣薪計算調整
                    //遲到扣薪規則: 除逢三倍數需扣薪外，遲到超過9:31(含)亦需扣半日薪；且超過30分之遲到不列入累計次數中
                    
                    //先單獨計算遲到30分之內之次數及扣薪
                    //移除:iOverTimes = (iOver0900 + iOver0930);
                    if (iOverTimes > 2)
                    {//遲到3次以上時,每3次扣半日薪(4小時),餘數累計至下個月                        
                        iOverTimes = (iOverTimes - iOverTimes % 3) / 3 * 4;
                    }
                    else
                    {//遲到2次以內不扣薪
                        iOverTimes = 0;
                    }

                    //加計遲到有超過30分鐘的部份
                    iOverTimes += iOver0930 * 4;
                    #endregion
                }
                if (iOverTimes > 0)
                { 
                }
                //將遲到扣薪加入[其他扣薪金額中]
                dOtherCharge += dHourChage * iOverTimes;
            }
        }
        catch (Exception ex) 
        {
            _UserInfo.SysSet.WriteToLogs("Error", "執行遲到扣款時發生錯誤(GetOtherCharge)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + strSQL + "\n\r");
        }
        #endregion
        return dOtherCharge;
    }

    /// <summary>
    /// 計算每月或多月薪水總額
    /// </summary>
    /// <param name="PayrollLt"></param>
    /// <param name="iMonthCount"></param>
    /// <returns></returns>
    public int GetTaxSum(PayrolList PayrollLt, int iMonthCount)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        int iTaxSum = 0;
        int iMonth = 0;
        strSQL = " And [SalaryYM]=" + PayrollLt.SalaryYM.ToString() + " ";
        string strVersionID = _UserInfo.SysSet.GetConfigString("VersionID");

        if ((strVersionID.Contains("PICCustomization")) && (PayrollLt.PeriodCode.Equals("B")))
        {
            iMonth = Convert.ToInt32(PayrollLt.SalaryYM.ToString().Substring(4, 2));
            if (iMonth == 5 || iMonth == 8 || iMonth == 11)
            {
                strSQL = " And [SalaryYM] in (" + PayrollLt.SalaryYM.ToString() + "," + (PayrollLt.SalaryYM - 1).ToString() + "," + (PayrollLt.SalaryYM - 2).ToString() + ") ";
                iMonth = 3;
            }
            else if (iMonth == 2)
            {
                strSQL = " And [SalaryYM] in (" + PayrollLt.SalaryYM.ToString() + "," + (PayrollLt.SalaryYM - 1).ToString() + "," + (PayrollLt.SalaryYM - 100).ToString().Remove(4) + "12" + ") ";
                iMonth = 3;
            }
            else
            {
                iMonth = 1;
            }
        }
        else
        {
            iMonth = 1;
        }

        //找出應稅總額
        strSQL = "Select Sum(Case [PMType] When 'A' Then " + PayrollLt.DeCodeKey + "([SalaryAmount]) Else (0-" + PayrollLt.DeCodeKey + "([SalaryAmount])) End) " +
            " From (select * from ["+PayrollTable+"_Detail] Union select * from [Payroll_Master_OtherDetail] ) PWD " +
            " Join SalaryStructure_Parameter SSP On [SalaryId]=[SalaryItem] " +
            " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' " +
            strSQL +
            " and [PeriodCode]='" + PayrollLt.PeriodCode + "' And [NWTax]='Y'";

        DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
        if (theDT != null)
        {
            if (theDT.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                    iTaxSum = (int)Convert.ToDecimal(theDT.Rows[0][0].ToString());
            }
        }

        //2011/02/17 原本要手動扣掉伙食費,今與PIC詹課長確認,由薪資項目控管即可
        //return iTaxSum - (1800 * iMonth);
        return iTaxSum;
    }

    /// <summary>
    /// 取得其它薪資金額
    /// </summary>
    /// <param name="PayrollLt">個人資料</param>
    /// <param name="SalaryItem">薪資項目</param>
    /// <returns>其它薪資金額</returns>
    private Decimal GetOtherSalary(PayrolList PayrollLt, string SalaryItem)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        decimal dOtherSalary = 0;

        strSQL = "Select " + PayrollLt.DeCodeKey + "(PMO.[SalaryAmount]) SalaryAmount " +
                " from   " +
                "   (Select * from Payroll_Master_OtherDetail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                "    And SalaryYM=" + PayrollLt.SalaryYM.ToString() + " And PeriodCode='" + PayrollLt.PeriodCode + "' And SalaryItem='" + SalaryItem + "'" +
                "   ) PMO " +
                " Left join SalaryStructure_Parameter SP On PMO.SalaryItem = SP.SalaryId ";

        try
        {
            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                        dOtherSalary = (decimal)Convert.ToDecimal(theDT.Rows[0][0].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            string temp = ex.Message;
        }

        return dOtherSalary;
    }

    /// <summary>
    /// 取得已計算個人之薪資金額
    /// </summary>
    /// <param name="PayrollLt">個人資料</param>
    /// <param name="SalaryItem">薪資項目</param>
    /// <returns>已計算個人之薪資金額</returns>
    private Decimal GetPayrollSalary(PayrolList PayrollLt, string SalaryItem)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        decimal dtheSalary = 0;

        strSQL = "Select " + PayrollLt.DeCodeKey + "(PMO.[SalaryAmount]) SalaryAmount " +
                " from   " +
                "   (Select * from "+PayrollTable+"_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                "    And SalaryYM=" + PayrollLt.SalaryYM.ToString() + " And PeriodCode='" + PayrollLt.PeriodCode + "' And SalaryItem='" + SalaryItem + "'" +
                "   ) PMO " +
                " Left join SalaryStructure_Parameter SP On PMO.SalaryItem = SP.SalaryId ";

        try
        {
            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                        dtheSalary = (decimal)Convert.ToDecimal(theDT.Rows[0][0].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            string temp = ex.Message;
        }

        return dtheSalary;
    }

    /// <summary>
    /// 取得已計算個人之應稅/免稅薪資金額
    /// </summary>
    /// <param name="PayrollLt">個人資料</param>
    /// <param name="NTorWT">應稅/免稅:Y/N</param>
    /// <returns>已計算個人之應稅/免稅薪資金額</returns>
    public Decimal GetPayrollNWTSalary(PayrolList PayrollLt, string NTorWT)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        decimal dtheSalary = 0;
        string strVersionID = _UserInfo.SysSet.GetConfigString("VersionID");
        if (strVersionID.Contains("PICCustomization") || strVersionID.Contains("PanPacific"))        
            strSQL = "21";        
        else
            strSQL = "20";

        //strSQL = "Select Sum(SalaryAmount) " +
        //        " from " +
        //        " (Select PWD.SalaryItem,(Case PMType When 'A' then (" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)+" + PayrollLt.DeCodeKey + "(PMO.SalaryAmount)) else (0-(" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)+" + PayrollLt.DeCodeKey + "(PMO.SalaryAmount))) end) As SalaryAmount " +
        //        " from "+PayrollTable+"_Detail PWD " +
        //        " Left join Payroll_Master_OtherDetail PMO On  " +
        //        " PWD.Company = PMO.Company And PWD.EmployeeId = PMO.EmployeeId And PWD.SalaryYM = PMO.SalaryYM And PWD.PeriodCode = PMO.PeriodCode And PWD.SalaryItem = PMO.SalaryItem " +
        //        " Left join SalaryStructure_Parameter SP On PWD.SalaryItem = SP.SalaryId " +
        //        " Where PWD.Company='" + PayrollLt.Company + "' And PWD.EmployeeId='" + PayrollLt.EmployeeId + "' " +
        //        " And PWD.SalaryYM=" + PayrollLt.SalaryYM.ToString() + " And PWD.PeriodCode='" + PayrollLt.PeriodCode + "'" +
        //        " And NWTax='" + NTorWT + "' And (SalaryId > '" + strSQL + "' Or SalaryId in ('08','09'))) PMO ";

        strSQL = "Select Sum(SalaryAmount) " +
        " from " +
        " (Select PWD.SalaryItem,(Case PMType When 'A' then (" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount)) else (0-(" + PayrollLt.DeCodeKey + "(PWD.SalaryAmount))) end) As SalaryAmount " +
        " from " + PayrollTable + "_Detail PWD " +                
        " Left join SalaryStructure_Parameter SP On PWD.SalaryItem = SP.SalaryId " +
        " Where PWD.Company='" + PayrollLt.Company + "' And PWD.EmployeeId='" + PayrollLt.EmployeeId + "' " +
        " And PWD.SalaryYM=" + PayrollLt.SalaryYM.ToString() + " And PWD.PeriodCode='" + PayrollLt.PeriodCode + "'" +
        " And NWTax='" + NTorWT + "' And (SalaryId > '" + strSQL + "' Or SalaryId in ('08','09'))) PMO ";
        try
        {
            DataTable theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (theDT.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(theDT.Rows[0][0].ToString()))
                        dtheSalary = (decimal)Convert.ToDecimal(theDT.Rows[0][0].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            string temp = ex.Message;
        }

        return dtheSalary;
    }

    /// <summary>
    /// 薪資試算作業
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="YearMonth">計薪年月</param>
    /// <param name="Period">計薪期別</param>
    /// <returns></returns>
    public int DraftPayroll(string Company, string YearMonth, string PeriodCode)
    {
        bool blPermission = false;
        int retResult = -99;
        DataTable theDT = null;
        string strSQLWhere = " Where Company='" + Company.Trim() + "' And SalaryYM=" + YearMonth.Trim() + " And PeriodCode='" + PeriodCode.Trim() + "'";

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //確認是否有試算權限
        blPermission = _UserInfo.CheckSysPermission("ePayroll", "PYM013");

        if (blPermission == true)
        {
             SalaryYearMonth sYM = new SalaryYearMonth();
            sYM = CheckSalaryYM(Company, YearMonth, PeriodCode);
            if (PayrollTable.Contains("test"))
                blPermission = true;
            else
                blPermission = false;
            if (!string.IsNullOrEmpty(sYM.ConfirmDate) && blPermission == false)
            {
                //指定公司於指定計薪年月已完成確認試算,不可再進行試算
                retResult = -10;
            }
            else if (sYM.LastConfirmYM > Convert.ToInt32(YearMonth) && blPermission == false)
            {
                //指定公司之待試算薪資年月早於已完成確認薪資之年月
                retResult = -11;
            }
            else
            {
                //先刪除已存在之試算資料
                retResult = DeletePayrollWorkingData(Company, YearMonth, PeriodCode);

                //開始取得薪資項目
                retResult = 0;

                strSQL = "Select * from Personnel_Master Where Company='" + Company + "' And IsNull(ResignCode,'')<>'Y' And PayCode<>'4' ";
                strSQL += " and Substring(Convert(varchar,HireDate,112),1,6)<='" + YearMonth + "'";
                theDT = _MyDBM.ExecuteDataTable(strSQL);
                if (theDT != null)
                {
                    if (theDT.Rows.Count > 0)
                    {
                        string DeCodeKey = "dbo.DraftPayroll" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        BeforeQuery(DeCodeKey);
                        try
                        {
                            #region 計算本月計薪天數
                            string sY, sM;
                            if (YearMonth.Length < 5)
                            {
                                sM = null;
                            }
                            else if (YearMonth.Length == 5)
                            {
                                sM = YearMonth.Substring(4, 1);
                            }
                            else
                            {
                                sM = YearMonth.Substring(4, 2);
                            }
                            sY = YearMonth.Substring(0, 4);
                            thisYMStratDate = Convert.ToDateTime(sY + "/" + sM + "/01");
                            thisYMEndDate = thisYMStratDate.AddMonths(1).AddDays(-1);
                            //不論大小月,皆以30天計薪
                            //thisYMPayDate = thisYMEndDate.DayOfYear - thisYMStratDate.DayOfYear + 1;                            
                            #endregion

                            PayrolList PayrollLt;
                            for (int i = 0; i < theDT.Rows.Count; i++)
                            {
                                //if (theDT.Rows[i]["EmployeeId"].ToString().Trim() == testEmpId)
                                {
                                    PayrollLt = new PayrolList();
                                    PayrollLt.Company = theDT.Rows[i]["Company"].ToString().Trim();
                                    PayrollLt.EmployeeId = theDT.Rows[i]["EmployeeId"].ToString().Trim();
                                    PayrollLt.SalaryYM = Convert.ToInt32(YearMonth);
                                    PayrollLt.PeriodCode = PeriodCode.Trim();
                                    PayrollLt.ResignCode = theDT.Rows[i]["ResignCode"].ToString().Trim();
                                    PayrollLt.DeCodeKey = DeCodeKey;
                                    PayrollLt.PayCode = theDT.Rows[i]["PayCode"].ToString().Trim();
                                    //計算個人薪資
                                    retResult += GetOnePayroll(PayrollLt);
                                }
                            }
                        }
                        catch (Exception ex){
                            _UserInfo.SysSet.WriteToLogs("Error", "DraftPayroll:薪資試算發生錯誤\n\r" + "Comp:" + Company + "|YM:" + YearMonth + "\n\r" + ex.Message);
                        }
                        AfterQuery(DeCodeKey);
                    }
                }

                //將試算日期寫入控管檔
                sYM = new SalaryYearMonth();
                sYM = CheckSalaryYM(Company, YearMonth, PeriodCode);
                if (sYM.isControl == false)
                {
                    strSQL = "Insert Into PayrollControl (Company,SalaryYM,PeriodCode,DraftDate) " +
                    "  Select '" + Company + "'," + YearMonth.Trim() + ",'" + PeriodCode.Trim() + "',GetDate() ";
                }
                else
                {
                    strSQL = "Update PayrollControl " +
                    "  Set DraftDate=GetDate() " + strSQLWhere + "";
                }
                _MyDBM.ExecuteCommand(strSQL);
            }
        }       

        return retResult;
    }

    /// <summary>
    /// 將計算後的薪資項目寫入暫存檔
    /// </summary>
    /// <param name="PayrollLt"></param>
    /// <param name="sSalaryItem"></param>
    /// <param name="decSalaryItem"></param>
    /// <param name="isReWrite"></param>
    /// <returns></returns>
    private int InsertPWDetail(PayrolList PayrollLt, string sSalaryItem, decimal decSalaryItem, bool isReWrite)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable tempDT = null;

        string strTempValue = _UserInfo.SysSet.GetConfigString("VersionID");
        strTempValue = "";
        string strVersionID = _UserInfo.SysSet.GetConfigString("VersionID");
        if (strVersionID.Contains("PICCustomization"))
        {
            if (string.IsNullOrEmpty(PayrollLt.PeriodCode) || PayrollLt.PeriodCode.Equals("0"))
            {
                strSQL = " Select [P1CostSalaryItem] From SalaryStructure_Parameter " +
                    " Where [SalaryId]='" + sSalaryItem + "'";
                tempDT = _MyDBM.ExecuteDataTable(strSQL);
                strTempValue = "";
                if (tempDT != null)
                {
                    if (tempDT.Rows.Count > 0)
                    {
                        strTempValue = tempDT.Rows[0][0].ToString();
                    }
                }
            }
        }

        //將計算後的薪資項目寫入暫存檔
        strSQL = "Select (" + PayrollLt.DeCodeKey + "(SalaryAmount)) from [" + PayrollTable + "_Detail] " +
        " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() +
        " and [PeriodCode]='" + (string.IsNullOrEmpty(strTempValue) ? PayrollLt.PeriodCode : strTempValue) + "' " +
        " and [SalaryItem]='" + sSalaryItem + "'";
        tempDT = _MyDBM.ExecuteDataTable(strSQL);
        try
        {
            if (tempDT != null & tempDT.Rows.Count > 0)
            {
                decSalaryItem += Convert.ToDecimal(tempDT.Rows[0][0]);
                tempDT.Dispose();
                ////修改[薪資工作檔DETAIL]
                //strSQL = "Update ["+PayrollTable+"_Detail] Set [SalaryAmount]=" + (isReWrite ? "" : "[SalaryAmount]+") + decSalaryItem.ToString() +
                //" where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() + " and [PeriodCode]='" + PayrollLt.PeriodCode + "' " +
                //" and [SalaryItem]='" + sSalaryItem + "'";
                strSQL = " Delete From [" + PayrollTable + "_Detail]  " +
                    " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() +
                    " and [PeriodCode]='" + (string.IsNullOrEmpty(strTempValue) ? PayrollLt.PeriodCode : strTempValue) + "' " +
                    " and [SalaryItem]='" + sSalaryItem + "'";
                _MyDBM.ExecuteCommand(strSQL);
            }
        }
        catch { }
        //else
        //{
        //    //寫入[薪資工作檔DETAIL]
        //    strSQL = "INSERT INTO ["+PayrollTable+"_Detail] ([Company],[EmployeeId],[SalaryYM],[PeriodCode],[SalaryItem],[SalaryAmount]) " +
        //        " Select '" + PayrollLt.Company + "','" + PayrollLt.EmployeeId + "'," + PayrollLt.SalaryYM.ToString() +
        //        ",'" + PayrollLt.PeriodCode + "','" + sSalaryItem + "','" + _UserInfo.SysSet.rtnHash(decSalaryItem.ToString()) + "'";
        //}

        //寫入[薪資工作檔DETAIL]
        strSQL = "INSERT INTO ["+PayrollTable+"_Detail] ([Company],[EmployeeId],[SalaryYM],[PeriodCode],[SalaryItem],[SalaryAmount]) " +
            " Select '" + PayrollLt.Company + "','" + PayrollLt.EmployeeId + "'," + PayrollLt.SalaryYM.ToString() +
            ",'" + (string.IsNullOrEmpty(strTempValue) ? PayrollLt.PeriodCode : strTempValue) + "','" + sSalaryItem + "','" + _UserInfo.SysSet.rtnHash(decSalaryItem.ToString()) + "'";

        return _MyDBM.ExecuteCommand(strSQL);
    }

    /// <summary>
    /// 刪除試算資料(BY公司)
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="YearMonth">計薪年月</param>
    /// <param name="PeriodCode">計薪期別</param>
    /// <returns>刪除筆數</returns>
    private int DeletePayrollWorkingData(string Company, string YearMonth, string PeriodCode)
    {
        return DeletePayrollWorkingData(Company, YearMonth, PeriodCode, "");
    }

    /// <summary>
    /// 刪除試算資料(BY員工)
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="YearMonth">計薪年月</param>
    /// <param name="PeriodCode">計薪期別</param>
    /// <param name="personalID">員工帳號</param>
    /// <returns>刪除筆數</returns>
    private int DeletePayrollWorkingData(string Company, string YearMonth, string PeriodCode, string personalID)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        int i = 0;
        string tempQuery = " and [PeriodCode]='" + PeriodCode.Trim() + "' ";
        string strVersionID = _UserInfo.SysSet.GetConfigString("VersionID");
        if (strVersionID.Contains("PICCustomization") || PeriodCode.Equals("0"))
        {
            tempQuery = "";
        }
        //刪除薪資項目試算資料
        strSQL = "Delete from ["+PayrollTable+"_Detail] " +
            " where [Company]='" + Company.Trim() + "' and [SalaryYM]=" + YearMonth.Trim() + tempQuery + ((personalID.Trim().Length == 0) ? "" : " And EmployeeId='" + personalID.Trim() + "' ");
        i = _MyDBM.ExecuteCommand(strSQL);

        //刪除薪資主檔試算資料
        strSQL = "Delete from ["+PayrollTable+"_Heading] " +
        " where [Company]='" + Company + "' and [SalaryYM]=" + YearMonth + tempQuery + ((personalID.Trim().Length == 0) ? "" : " And EmployeeId='" + personalID.Trim() + "' ");
        i += _MyDBM.ExecuteCommand(strSQL);
        
        return i;
    }

    /// <summary>
    /// 試算個人薪資
    /// </summary>
    /// <returns></returns>
    public int GetOnePayroll(PayrolList PayrollLt)
    {
        bool blPermission = false;
        int retResult = -1;
        DataTable theDT = null;

        int NewInCode = 0;
        PayrolList NewInPayrollLt = PayrollLt;

        if (Convert.ToInt32(PayrollLt.SalaryYM.ToString().Substring(4)) > 1)
            NewInPayrollLt.SalaryYM = PayrollLt.SalaryYM - 1;
        else
            NewInPayrollLt.SalaryYM = PayrollLt.SalaryYM - 89;

        float fSalary = 0;
        decimal totalSalary = 0, decSalaryItem = 0;
        string sSalaryItem = "", sRegularPay = "", sLeaveBI = "";
        int iLeaveHours = 0, i = 0;

        #region 應免稅加減項
        //應稅加項（薪資）
        PayrollLt.WT_P_Salary = 0;
        //應稅加項（獎金）
        PayrollLt.WT_P_Bonus = 0;
        //應稅減項（薪資）
        PayrollLt.WT_M_Salary = 0;
        //應稅減項（獎金）
        PayrollLt.WT_M_Bonus = 0;
        //免稅加項
        PayrollLt.NT_P = 0;
        //免稅減項
        PayrollLt.NT_M = 0;
        #endregion

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //確認是否有試算權限
        blPermission = _UserInfo.CheckSysPermission("ePayroll", "PYM013");

        if (blPermission == true)
        {
            //開始取得薪資項目
            retResult = 0;
            
            #region 找出預設薪資項目之SQL
            string strVersionID = _UserInfo.SysSet.GetConfigString("VersionID");
            if (strVersionID.Contains("PICCustomization"))
                strSQL = "select [SalaryId] from [SalaryStructure_Parameter] Where [ItemType] = '1'";
            else
                strSQL = "select [SalaryId] from [SalaryStructure_Parameter] Where [ItemType] in ('0','1')";

            strSQL = "Insert into Payroll_Master_Detail(Company,EmployeeId,SalaryItem,Amount) " +
                " Select '" + PayrollLt.Company + "','" + PayrollLt.EmployeeId + "',SalaryId,'" + EnCodeAmount("0") + "' " +
                " From (select * from SalaryStructure_Parameter Where SalaryId In (" + strSQL + ")) SSP " +
                " Where NOT EXISTS (Select * from Payroll_Master_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' And SalaryItem=SalaryId )";

            i = _MyDBM.ExecuteCommand(strSQL);
            if (i > 0)
            {
                _UserInfo.SysSet.WriteToLogs("薪資項目", "於薪資試算時，系統自動預設" + PayrollLt.Company + "-" + PayrollLt.EmployeeId + "[薪資項目]共" + i.ToString() + "筆");
            }
            #endregion

            //本月原始計薪天數
            decimal thePaydays = thisYMPayDate;
            thePaydays = GetPaydays(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString());

            strSQL = "Select PMD.* " +
                " ,SSP.[P1CostSalaryItem],IsNull(RegularPay,'') RegularPay,IsNull(NWTax,'') NWTax,IsNull(PMType,'') PMType,IsNull(Properties,'') Properties " +
                " ,(Case When dbo.PadLeft(CAST(Year([HireDate]) AS VARCHAR),'0',4)+dbo.PadLeft(CAST(MONTH([HireDate]) AS VARCHAR),'0',2)='" + NewInPayrollLt.SalaryYM.ToString() + "' Then ((Select count(*) From [NewInEmployee] NI Where PMD.Company=Company And PMD.EmployeeId=EmployeeId )) Else 0 End) As [NewInCode] " +
                //判斷是否不計健保費(非計薪當月到職且計薪當月於月底前離職時)
                //2011/11/29 增修:依規定凡當月離職者,一律不扣健保費;如這名員工服務到當月30日(或31日)離職健保是隔月一號才能轉出
                " ,(Case When IsNull([ResignCode],'N') != 'N' " +
                //" and (Substring(Convert(varchar,HireDate,112),1,6) !='" + PayrollLt.SalaryYM.ToString() + "'" +
                //"     OR  (Substring(Convert(varchar,HireDate,112),1,6) ='" + PayrollLt.SalaryYM.ToString() + "'" +
                //"          and Substring(Convert(varchar,ResignDate,112),1,6) ='" + PayrollLt.SalaryYM.ToString() + "')) " +                
                " and (Substring(Convert(varchar,ResignDate,112),1,6) ='" + PayrollLt.SalaryYM.ToString() + "')" +
                " and (ResignDate != Convert(datetime,'" + thisYMEndDate.ToString("yyyy/MM/dd") + "')) " +
                " Then 'Y' else 'N' End) ResignNoHIFee " +
                /// 2013/04/23 依財務通知，健保費計算程式需修改如下:
                /// 健保局確認現行代扣方式
                /// 1.如當日報到並離職，其健保費個人部份毋需代扣且公司也不必負擔
                /// 2.如當月報到當月離職(不含報到當日)，個人需代扣健保費且公司也需負擔
                /// 3.如前一個月之前報到，本月份離職，則個人部份毋需代扣且公司也不必負擔
                " ,(Case When IsNull([ResignCode],'N') != 'N' " +               
                " and ((Convert(varchar,HireDate,112)!=Convert(varchar,ResignDate,112)) " +
                " and (Substring(Convert(varchar,HireDate,112),1,6)='" + PayrollLt.SalaryYM.ToString() + "' " +
                "      and Substring(Convert(varchar,ResignDate,112),1,6)='" + PayrollLt.SalaryYM.ToString() + "')) " +
                " Then 'Y' else 'N' End) ResignHasHIFee " +
                //判斷是否要破月計算勞保費
                " ,(Case When ((Substring(Convert(varchar,ResignDate,112),1,6) !='" + PayrollLt.SalaryYM.ToString() + "') " +
                " and (Substring(Convert(varchar,HireDate,112),1,6) !='" + PayrollLt.SalaryYM.ToString() + "')) " +
                " Or (Convert(varchar,HireDate,112)='" + PayrollLt.SalaryYM.ToString() + "01' and ( IsNull(ResignDate,dateadd(d,-1,HireDate)) < HireDate Or " +
                " Convert(varchar,ResignDate,112)=Convert(varchar,dateadd(d,-1,dateadd(m,1,Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01'))),112))) " +
                " Then 'Y' else 'N' End) FullLIFee " +
                ////判斷若需破月計算勞保費時,其勞保天數
                //" ,(DateDiff(d,(Case When HireDate >= Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01') then HireDate else Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01') end)," +
                //"  (Case When Substring(Convert(varchar,ResignDate,112),1,6)='" + PayrollLt.SalaryYM.ToString() + "' " +                
                //"  then ResignDate else Convert(varchar,dateadd(d,-1,dateadd(m,1,Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01'))),112) end))+1) LIdays" +
                //2012/03/09 增修勞保計算天數:同基本在職天數(未扣差勤)
                //" ," + thePaydays.ToString() + " as LIdays" +
                //2012/03/27 依財務要求修改:〔3/26 到職者，實際上班天數應為(31-26+1)天〕經確認， 2月到職之天數計算為純屬２月之特例，其它大小月於破月計薪時皆依實際天數計算之(故若整月在職則,在職天數必為30天)
                " ,Case when 2=" + (PayrollLt.SalaryYM.ToString().EndsWith("02") ? "2" : "0") + " then " + thePaydays.ToString() + " else " +
                "  (DateDiff(d,(Case When HireDate >= Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01') then HireDate else Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01') end)," +
                "  (Case When Substring(Convert(varchar,ResignDate,112),1,6)='" + PayrollLt.SalaryYM.ToString() + "' " +
                "  then ResignDate else Convert(varchar,dateadd(d,-1,dateadd(m,1,Convert(datetime,'" + PayrollLt.SalaryYM.ToString() + "01'))),112) end))+1) end" +
                " LIdays" +
                //判斷薪資項目是否是請假扣薪基數
                " ,IsNull((Select CodeCode From CodeDesc where CodeID='PY#LeaveBI' and CodeCode=SalaryItem),'') LeaveBI " +
                " from Payroll_Master_Detail PMD "+
                " Left Join [Personnel_Master] PM On PMD.Company=PM.Company And PMD.EmployeeId=PM.EmployeeId " +
                " Left Join [SalaryStructure_Parameter] SSP On SSP.[SalaryId] = PMD.SalaryItem " +
                " Where PMD.Company='" + PayrollLt.Company + "' And PMD.EmployeeId='" + PayrollLt.EmployeeId + "' " +
                " Order By PMD.SalaryItem";
            theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (PayrollLt.EmployeeId == testEmpId)
                {
                }                                
                //取得請假扣薪基數
                iLeaveHours = GetLeaveHours(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString());
                //計算出個人當月計薪天數            
                try
                {
                    if (theDT.Rows[i]["FullLIFee"].ToString().Equals("N"))
                    {//勞保應破月計算時,離職當月之天數應從當月1日計至離職日
                        decimal truedate = Convert.ToDecimal(theDT.Rows[i]["LIdays"].ToString());
                        //2012/07/27 kaya 修正在職天數大於計薪基本天數問題
                        thePaydays = (truedate > 30) ? thePaydays : truedate;
                    }
                }
                catch { }
                PayrollLt.Paydays = thePaydays - ((decimal)iLeaveHours / 8);
                                
                #region 取得加班金額
                //取得加班費
                OverTimes OverTimeFee = GetWTOverTime(PayrollLt);
                //應稅加班
                PayrollLt.WT_Overtime = OverTimeFee.WT_Overtime;
                PayrollLt.WT_Overtime_Fee = OverTimeFee.WT_Overtime_Fee;
                totalSalary += PayrollLt.WT_Overtime_Fee;
                //因為應稅,所以要加到總計去
                fSalary += (float)PayrollLt.WT_Overtime_Fee;
                //免稅加班                
                PayrollLt.NT_Overtime = OverTimeFee.NT_Overtime;
                PayrollLt.NT_Overtime_Fee = OverTimeFee.NT_Overtime_Fee;
                totalSalary += PayrollLt.NT_Overtime_Fee;
                //將計算後的薪資項寫入暫存檔
                if (strVersionID.Contains("PICCustomization"))
                {                    
                    if (PayrollLt.PayCode.Equals("3"))
                    {//二者皆有                        
                        PayrollLt.PeriodCode = "A";
                        decSalaryItem = 0;
                        if (OverTimeFee.OvertimeBasic > 0)
                            decSalaryItem = OverTimeFee.WT_Overtime_Fee * OverTimeFee.OvertimeBasic1 / OverTimeFee.OvertimeBasic;
                        decSalaryItem = Math.Ceiling(decSalaryItem);
                        retResult = InsertPWDetail(PayrollLt, "08", decSalaryItem, true);
                        decSalaryItem = 0;
                        if (OverTimeFee.OvertimeBasic > 0)
                            decSalaryItem = OverTimeFee.NT_Overtime_Fee * OverTimeFee.OvertimeBasic1 / OverTimeFee.OvertimeBasic;
                        decSalaryItem = Math.Ceiling(decSalaryItem);
                        retResult = InsertPWDetail(PayrollLt, "09", decSalaryItem, true);

                        PayrollLt.PeriodCode = "B";
                        decSalaryItem = 0;
                        if (OverTimeFee.OvertimeBasic > 0)
                            decSalaryItem = OverTimeFee.WT_Overtime_Fee * OverTimeFee.OvertimeBasic2 / OverTimeFee.OvertimeBasic;
                        decSalaryItem = Math.Ceiling(decSalaryItem);
                        retResult = InsertPWDetail(PayrollLt, "08", decSalaryItem, true);
                        decSalaryItem = 0;
                        if (OverTimeFee.OvertimeBasic > 0)
                            decSalaryItem = OverTimeFee.NT_Overtime_Fee * OverTimeFee.OvertimeBasic2 / OverTimeFee.OvertimeBasic;
                        decSalaryItem = Math.Ceiling(decSalaryItem);
                        retResult = InsertPWDetail(PayrollLt, "09", decSalaryItem, true);
                    }
                    else if (PayrollLt.PayCode.Equals("1") || PayrollLt.PayCode.Equals("2"))
                    {
                        //工資/非工資           
                        PayrollLt.PeriodCode = PayrollLt.PayCode.Equals("1") ? "A" : "B";
                        retResult = InsertPWDetail(PayrollLt, "08", OverTimeFee.WT_Overtime_Fee, true);
                        retResult = InsertPWDetail(PayrollLt, "09", OverTimeFee.NT_Overtime_Fee, true);
                    }
                    PayrollLt.PeriodCode = "0";
                }
                else
                {
                    retResult = InsertPWDetail(PayrollLt, "08", OverTimeFee.WT_Overtime_Fee, true);
                    retResult = InsertPWDetail(PayrollLt, "09", OverTimeFee.NT_Overtime_Fee, true);
                }
                #endregion

                if (PayrollLt.EmployeeId == testEmpId)
                { 
                }

                #region 儀測:底薪-請假扣薪金額(時薪*請假扣薪基數);其它於各項目中按比例扣除
                decimal iBaseSalary = OverTimeFee.OvertimeBasic;
                if (strVersionID.Contains("PICCustomization"))
                {
                    iBaseSalary = OverTimeFee.OvertimeBasic1;
                    PayrollLt.BaseSalary -= (int)Math.Round(iBaseSalary * iLeaveHours);
                }
                #endregion

                //請假扣薪實際時數
                PayrollLt.LeaveHours_deduction = GetLeaveHoursDeduction(PayrollLt.Company, PayrollLt.EmployeeId, PayrollLt.SalaryYM.ToString(), "N");
                
                //有特殊計算之固定薪資列表                
                string SIList = "'03','04','05','06','07','08','09','10','14','15','16','21'";
                if (strVersionID.Contains("PICCustomization") || strVersionID.Contains("PanPacific"))
                    SIList += ",'21'";
                #region 計算固定薪資
                if (theDT.Rows.Count > 0)
                {
                    //若為新進人員則會加上上個月未計薪的天數比例
                    NewInCode = (int)theDT.Rows[0]["NewInCode"];                    
                    if (NewInCode > 0)
                    {
                        NewInPayrollLt.Paydays = GetPaydays(PayrollLt.Company, PayrollLt.EmployeeId, NewInPayrollLt.SalaryYM.ToString());                        
                        //GetOnePayroll(NewInPayrollLt);
                        PayrollLt.Paydays += NewInPayrollLt.Paydays;
                    }

                    if (PayrollLt.Paydays > 0)
                    {
                        for (i = 0; i < theDT.Rows.Count; i++)
                        {
                            sSalaryItem = theDT.Rows[i]["SalaryItem"].ToString().Trim();

                            if (SIList.Contains("'" + sSalaryItem + "'"))
                            {
                                if (PayrollLt.EmployeeId.Equals(testEmpId))
                                {
                                }
                                //固定算法,不使用薪資項目公式計算
                                PayrollLt.PeriodCode = "";
                                int LIdays = 30;
                                #region
                                switch (sSalaryItem)
                                {
                                    case "03":
                                        break;
                                    case "04":
                                        #region 取得健保金額與投保人數
                                        HealthInsurance.DependentPremium HI = HealthInsurance.GetPremium(PayrollLt);
                                        PayrollLt.Dependent1_IDNo = "";
                                        PayrollLt.Dependent1_HI_Fee = 0;
                                        PayrollLt.Dependent2_IDNo = "";
                                        PayrollLt.Dependent2_HI_Fee = 0;
                                        PayrollLt.Dependent3_IDNo = "";
                                        PayrollLt.Dependent3_HI_Fee = 0;
                                        if (theDT.Rows[i]["ResignNoHIFee"].ToString().Equals("Y") && theDT.Rows[i]["ResignHasHIFee"].ToString().Equals("N"))
                                        {
                                            HI.Company = 0;
                                            HI.Person = 0;
                                            HI.Insured = 0;
                                            HI.Dependent = 0;
                                        }
                                        else
                                        {
                                            if (HI.Dependents.Length > 0)
                                            {
                                                PayrollLt.Dependent1_IDNo = HI.Dependents[0].IDNo;
                                                PayrollLt.Dependent1_HI_Fee = HI.Dependents[0].Insured_Amount;
                                            }
                                            if (HI.Dependents.Length > 1)
                                            {
                                                PayrollLt.Dependent2_IDNo = HI.Dependents[1].IDNo;
                                                PayrollLt.Dependent2_HI_Fee = HI.Dependents[1].Insured_Amount;
                                            }
                                            if (HI.Dependents.Length > 2)
                                            {
                                                PayrollLt.Dependent3_IDNo = HI.Dependents[2].IDNo;
                                                PayrollLt.Dependent3_HI_Fee = HI.Dependents[2].Insured_Amount;
                                            }
                                        }
                                        PayrollLt.HI_CompanyBurden = HI.Company;//公司負擔
                                        PayrollLt.HI_Person = HI.Person;
                                        PayrollLt.HI_Fee = HI.Insured + HI.Dependent; //員工+眷屬保費                                        
                                        //存入投保級距
                                        retResult = InsertPWDetail(PayrollLt, "18", HI.Insured_Amount, true);

                                        totalSalary -= PayrollLt.HI_Fee;
                                        //新進人員加入上月未扣健保費
                                        if (NewInCode > 0)
                                        {
                                            HealthInsurance.DependentPremium NewInHI = HealthInsurance.GetPremium(NewInPayrollLt);
                                            PayrollLt.HI_CompanyBurden += NewInHI.Company;//公司負擔
                                            PayrollLt.HI_Fee += NewInHI.Insured + NewInHI.Dependent; //員工+眷屬保費
                                            if (NewInHI.Dependents.Length > 0)
                                            {
                                                if (PayrollLt.Dependent1_IDNo == NewInHI.Dependents[0].IDNo)
                                                    PayrollLt.Dependent1_HI_Fee += NewInHI.Dependents[0].Insured_Amount;
                                            }
                                            if (NewInHI.Dependents.Length > 1)
                                            {
                                                if (PayrollLt.Dependent2_IDNo == NewInHI.Dependents[1].IDNo)
                                                    PayrollLt.Dependent2_HI_Fee += NewInHI.Dependents[1].Insured_Amount;
                                            }
                                            if (NewInHI.Dependents.Length > 2)
                                            {
                                                if (PayrollLt.Dependent3_IDNo == NewInHI.Dependents[2].IDNo)
                                                    PayrollLt.Dependent3_HI_Fee = NewInHI.Dependents[2].Insured_Amount;
                                            }
                                            totalSalary -= (NewInHI.Insured + NewInHI.Dependent);
                                        }
                                        //將計算後的薪資項寫入暫存檔
                                        if (strVersionID.Contains("PICCustomization"))
                                            PayrollLt.PeriodCode = "A";
                                        else
                                            PayrollLt.PeriodCode = "0";
                                        retResult = InsertPWDetail(PayrollLt, "04", PayrollLt.HI_Fee, true);
                                        //公司負擔
                                        PayrollLt.PeriodCode = "C";
                                        retResult = InsertPWDetail(PayrollLt, "14", PayrollLt.HI_CompanyBurden, true);
                                        #endregion
                                        break;
                                    case "05":
                                        #region 取得勞保金額
                                        LaborInsurance.Insurance LI = new LaborInsurance.Insurance();

                                        if (theDT.Rows[i]["FullLIFee"].ToString().Equals("N"))
                                        {//勞保應破月計算時,離職當月之天數應從當月1日計至離職日
                                            LIdays = (int)theDT.Rows[i]["LIdays"];
                                            if (LIdays > thisYMPayDate) LIdays = thisYMPayDate;

                                            LI = LaborInsurance.GetPremium(PayrollLt, LIdays);
                                        }
                                        else
                                            LI = LaborInsurance.GetPremium(PayrollLt);

                                        //存入投保級距
                                        retResult = InsertPWDetail(PayrollLt, "19", LI.Insured_Amount, true);

                                        //公司負擔
                                        PayrollLt.LI_CompanyBurden = LI.Company;
                                        //員工負擔
                                        PayrollLt.LI_Fee = LI.Insured;
       
                                        totalSalary -= PayrollLt.LI_Fee;
                                        //新進人員加入上月未扣勞保費
                                        if (NewInCode > 0)
                                        {
                                            LaborInsurance.Insurance NewInLI = new LaborInsurance.Insurance();                                            
                                            #region 計算上月到職天數
                                            decimal theNewPaydays = 5, iNewLeaveHours = 0, theLastPaydays = 30;
                                            try
                                            {
                                                #region 計算本月計薪天數
                                                string sY, sM, YearMonth;
                                                YearMonth = NewInPayrollLt.SalaryYM.ToString();
                                                if (YearMonth.Length == 5)
                                                {
                                                    sM = YearMonth.Substring(4, 1);
                                                }
                                                else
                                                {
                                                    sM = YearMonth.Substring(4, 2);
                                                }
                                                sY = YearMonth.Substring(0, 4);
                                                DateTime theNewYMStratDate = Convert.ToDateTime(sY + "/" + sM + "/01");
                                                DateTime theNewYMEndDate = thisYMStratDate.AddMonths(1).AddDays(-1);
                                                theLastPaydays = theNewYMEndDate.DayOfYear - theNewYMStratDate.DayOfYear + 1;
                                                #endregion
                                                theNewPaydays = GetPaydays(PayrollLt.Company, PayrollLt.EmployeeId, NewInPayrollLt.SalaryYM.ToString());
                                                //取得請假扣薪基數
                                                iNewLeaveHours = GetLeaveHours(PayrollLt.Company, PayrollLt.EmployeeId, NewInPayrollLt.SalaryYM.ToString());
                                                //計算出個人上月計薪天數              
                                                NewInPayrollLt.Paydays = theNewPaydays - ((decimal)iNewLeaveHours / 8);
                                            }
                                            catch(Exception ex){
                                                _UserInfo.SysSet.WriteToLogs("Error", "計算上月到職天數發生錯誤\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "|YM:" + NewInPayrollLt.SalaryYM.ToString() + "\n\r" + ex.Message);
                                            }
                                            #endregion
                                            NewInLI = LaborInsurance.GetPremium(NewInPayrollLt, (int)NewInPayrollLt.Paydays);
                                            PayrollLt.LI_CompanyBurden += NewInLI.Company;//公司負擔
                                            PayrollLt.LI_Fee += NewInLI.Insured;
                                            totalSalary -= NewInLI.Insured;
                                        }
                                        //將計算後的薪資項寫入暫存檔
                                        if (strVersionID.Contains("PICCustomization"))
                                            PayrollLt.PeriodCode = "A";
                                        else
                                            PayrollLt.PeriodCode = "0";
                                        retResult = InsertPWDetail(PayrollLt, "05", PayrollLt.LI_Fee, true);
                                        //公司負擔
                                        PayrollLt.PeriodCode = "C";
                                        retResult = InsertPWDetail(PayrollLt, "15", PayrollLt.LI_CompanyBurden, true);                                                                  
                                        #endregion
                                        break;
                                    case "06":
                                        #region 取得退休金自提金額
                                        RetirementPension.Pensionaccounts RP = new RetirementPension.Pensionaccounts();                                        
                                        if (theDT.Rows[i]["FullLIFee"].ToString().Equals("N"))
                                        {//勞保應破月計算時,離職當月之天數應從當月1日計至離職日
                                            LIdays = (int)theDT.Rows[i]["LIdays"];
                                            if (LIdays > thisYMPayDate) LIdays = thisYMPayDate;
                                        }
                                        RP = RetirementPension.GetMonthlyActualSalary(PayrollLt,LIdays);//Johnny加
                                        PayrollLt.RP_CompanyBurden = RP.Company;//公司負擔
                                        PayrollLt.RetirementPension = RP.Insured;
                                        //存入投保級距
                                        retResult = InsertPWDetail(PayrollLt, "17", RP.Insured_Amount, true);

                                        totalSalary -= PayrollLt.RetirementPension;
                                        fSalary -= PayrollLt.RetirementPension;
                                        //新進人員加入上月未扣退休金自提金額
                                        if (NewInCode > 0)
                                        {
                                            //計算上月在職天數
                                            LIdays = Convert.ToInt32(GetPaydays(PayrollLt.Company, PayrollLt.EmployeeId, NewInPayrollLt.SalaryYM.ToString()));                                            
                                            RetirementPension.Pensionaccounts NewInRP = new RetirementPension.Pensionaccounts();
                                            NewInRP = RetirementPension.GetMonthlyActualSalary(NewInPayrollLt, LIdays);//Johnny加
                                            PayrollLt.RP_CompanyBurden += NewInRP.Company;//公司負擔
                                            PayrollLt.RetirementPension += NewInRP.Insured;
                                            totalSalary -= NewInRP.Insured;
                                            fSalary -= NewInRP.Insured;
                                            RP.Insured += NewInRP.Insured;
                                        }
                                        //將計算後的薪資項寫入暫存檔
                                        if (strVersionID.Contains("PICCustomization"))
                                            PayrollLt.PeriodCode = "A";
                                        else
                                            PayrollLt.PeriodCode = "0";
                                        retResult = InsertPWDetail(PayrollLt, "06", PayrollLt.RetirementPension, true);
                                        //公司負擔
                                        PayrollLt.PeriodCode = "C";
                                        retResult = InsertPWDetail(PayrollLt, "16", PayrollLt.RP_CompanyBurden, true);
                                        #endregion
                                        break;
                                    case "07":
                                        #region 取得福利金金額
                                        //2013/01/25 依財務要求，所有破月到職與破月離職之員工福利金皆以破月計算
                                        if (theDT.Rows[i]["FullLIFee"].ToString().Equals("N"))
                                            decSalaryItem = FringeBenefits.GetFringeBenefitUnFullMonth(PayrollLt);
                                        else
                                            decSalaryItem = FringeBenefits.GetFringeBenefits(PayrollLt);
                                        //2011/12/26 C.新進人員加入上月未扣福利金金額
                                        if (NewInCode > 0)
                                        {
                                            decSalaryItem += FringeBenefits.GetFringeBenefitUnFullMonth(NewInPayrollLt);
                                        }
                                        totalSalary -= decSalaryItem;
                                        //將計算後的薪資項寫入暫存檔
                                        if (strVersionID.Contains("PICCustomization"))
                                            PayrollLt.PeriodCode = "A";
                                        else
                                            PayrollLt.PeriodCode = "0";
                                        retResult = InsertPWDetail(PayrollLt, "07", decSalaryItem, true);
                                        #endregion
                                        break;
                                    case "08":
                                        break;
                                    case "09":
                                        break;
                                    case "10"://請假扣薪使用
                                        break;
                                    case "14"://公司負擔:健保(於個人項目時計算)
                                        break;
                                    case "15"://公司負擔:勞保(於個人項目時計算)
                                        break;
                                    case "16"://公司負擔:退休金(於個人項目時計算)
                                        break;
                                    case "21":
                                        if (strVersionID.Contains("PanPacific"))
                                        {
                                            PayrollLt.PeriodCode = "0";
                                            #region 其他扣款,主要用來算遲到扣薪
                                            try
                                            {
                                                decSalaryItem = Convert.ToDecimal(GetSalaryItem(PayrollLt, "21"));
                                            }
                                            catch
                                            {
                                                decSalaryItem = 0;
                                            }
                                            decSalaryItem = GetOtherCharge(PayrollLt, decSalaryItem);
                                            decSalaryItem = Math.Round(decSalaryItem);
                                            totalSalary += decSalaryItem;
                                            //將計算後的薪資項寫入暫存檔
                                            retResult = InsertPWDetail(PayrollLt, "21", decSalaryItem, true);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region 全勤
                                            decSalaryItem = GetAttendanceFee(PayrollLt, decSalaryItem);
                                            totalSalary += decSalaryItem;
                                            //將計算後的薪資項寫入暫存檔
                                            retResult = InsertPWDetail(PayrollLt, "21", decSalaryItem, true);
                                            #endregion
                                        }
                                        break;
                                }
                                #endregion
                                PayrollLt.PeriodCode = "0";
                            }
                            else
                            {
                                if (sSalaryItem == "01" && (PayrollLt.EmployeeId.Equals(testEmpId) || PayrollLt.EmployeeId.Equals("A1655")))
                                {
                                }
                                //取得個人各別薪資項目值(減項會是乘上負值)                            
                                string strPersonalSalaryItem = GetPersonalSalaryItem(PayrollLt, sSalaryItem);
                                try
                                {
                                    decSalaryItem = Convert.ToDecimal(strPersonalSalaryItem);
                                }
                                catch { }
                                #region 2011/12/28 A.新進人員上月未付底薪已於請假扣薪時按比例加入
                                if (sSalaryItem.Equals("01"))
                                {//底薪BaseSalary固定代碼為01
                                    //底薪:
                                    PayrollLt.MonthlyBaseSalary = (int)decSalaryItem;
                                }
                                #endregion
                                #region 請假扣薪(時薪*請假扣薪基數)於[加班計時]項目中按比例扣除(計算底薪/伙食費時,含上月未付比例)
                                sLeaveBI = theDT.Rows[i]["LeaveBI"].ToString().Trim();
                                if (sLeaveBI.Equals(sSalaryItem))
                                    decSalaryItem = Math.Round(decSalaryItem * (PayrollLt.Paydays * 8) / (thisYMPayDate * 8));
                                #endregion
                                totalSalary += decSalaryItem;
                                #region 下面是找出每個薪資項目的屬性設定
                                //strSQL = "Select  * from SalaryStructure_Parameter Where SalaryId='" + sSalaryItem + "'";
                                //DataTable theItemDT = _MyDBM.ExecuteDataTable(strSQL);                                
                                //if (theItemDT != null && theItemDT.Rows.Count > 0)
                                {
                                    //sRegularPay = theItemDT.Rows[0]["RegularPay"].ToString().Trim();
                                    sRegularPay = theDT.Rows[i]["RegularPay"].ToString().Trim();
                                    if (sRegularPay.Equals("Y"))
                                    {//新制提撥退休金使用:Y-是 N-否 
                                        fSalary += (float)decSalaryItem;
                                    }

                                    if (sSalaryItem.Equals("01"))
                                    {//底薪BaseSalary固定代碼為01
                                        //底薪(實領)
                                        PayrollLt.BaseSalary += (int)decSalaryItem;
                                        #region 2011/12/28 A.刪除:已於請假扣薪時按比例加入
                                        ////新進人員按比例加入上月未付底薪
                                        //if (NewInCode > 0)
                                        //    PayrollLt.BaseSalary += (int)((decSalaryItem * NewInPayrollLt.Paydays) / 30);
                                        //因為可能有請假扣款,所以要覆寫回去
                                        #endregion
                                        decSalaryItem = PayrollLt.BaseSalary;
                                    }
                                    else
                                    {
                                        #region 2011/12/26 B.刪除:已於請假扣薪時按比例加入
                                        //新進人員按比例加入上月未付伙食費
                                        //if (sSalaryItem.Equals("02") && (NewInCode > 0))
                                        //    decSalaryItem += (int)((decSalaryItem * NewInPayrollLt.Paydays) / 30);
                                        #endregion

                                        #region 應免稅加減項
                                        //NWTax  應免稅別
                                        if (theDT.Rows[i]["NWTax"].ToString().Trim().Equals("Y"))
                                        {//Y- 應稅
                                            //PMType 加減項別
                                            if (theDT.Rows[i]["PMType"].ToString().Trim().Equals("A"))
                                            {//A-加項
                                                //Properties 項目屬性:1- 薪資     2- 成本     3- 公司負擔 5- 公司獎金
                                                if (theDT.Rows[i]["Properties"].ToString().Trim().Equals("1"))
                                                {//應稅加項（薪資）
                                                    PayrollLt.WT_P_Salary += (int)decSalaryItem;
                                                }
                                                else if (theDT.Rows[i]["Properties"].ToString().Trim().Equals("5"))
                                                {//應稅加項（獎金）
                                                    PayrollLt.WT_P_Bonus += (int)decSalaryItem;
                                                }
                                            }
                                            else
                                            {//B- 減項
                                                //Properties 項目屬性:1- 薪資     2- 成本     3- 公司負擔 5- 公司獎金
                                                if (theDT.Rows[i]["Properties"].ToString().Trim().Equals("1"))
                                                {//應稅減項（薪資）
                                                    PayrollLt.WT_M_Salary += (int)decSalaryItem;
                                                }
                                                else if (theDT.Rows[i]["Properties"].ToString().Trim().Equals("5"))
                                                {//應稅減項（獎金）
                                                    PayrollLt.WT_M_Bonus += (int)decSalaryItem;
                                                }
                                            }
                                        }
                                        else
                                        {//N- 免稅 
                                            if (theDT.Rows[i]["PMType"].ToString().Trim().Equals("A"))
                                            {//A-加項　　 
                                                //免稅加項
                                                PayrollLt.NT_P += (int)decSalaryItem;
                                            }
                                            else
                                            {//B- 減項
                                                //免稅減項
                                                PayrollLt.NT_M += (int)decSalaryItem;
                                            }
                                        }
                                        #endregion
                                    }
                                    //將計算後的薪資項寫入暫存檔
                                    if (strVersionID.Contains("PICCustomization"))
                                    {
                                        PayrollLt.PeriodCode = theDT.Rows[i]["P1CostSalaryItem"].ToString();
                                        if (PayrollLt.PayCode.Equals("3") && PayrollLt.PeriodCode.Equals("C"))
                                        {//二者皆有
                                            decimal tempDecimal = 0;
                                            PayrollLt.PeriodCode = "A";
                                            if (OverTimeFee.OvertimeBasic > 0)
                                                tempDecimal = decSalaryItem * OverTimeFee.OvertimeBasic1 / OverTimeFee.OvertimeBasic;
                                            retResult = InsertPWDetail(PayrollLt, sSalaryItem, tempDecimal, true);
                                            tempDecimal = 0;
                                            PayrollLt.PeriodCode = "B";
                                            if (OverTimeFee.OvertimeBasic > 0)
                                                tempDecimal = decSalaryItem * OverTimeFee.OvertimeBasic2 / OverTimeFee.OvertimeBasic;
                                            retResult = InsertPWDetail(PayrollLt, sSalaryItem, tempDecimal, true);
                                        }
                                        else if (PayrollLt.PayCode.Equals("1") || PayrollLt.PayCode.Equals("2") || PayrollLt.PayCode.Equals("3"))
                                        {//工資/非工資           
                                            PayrollLt.PeriodCode = (((PayrollLt.PayCode.Equals("1") && PayrollLt.PeriodCode.Equals("A"))
                                                || (PayrollLt.PayCode.Equals("1") && PayrollLt.PeriodCode.Equals("C"))) ? "A" : (
                                                ((PayrollLt.PayCode.Equals("2") && PayrollLt.PeriodCode.Equals("B"))
                                                || (PayrollLt.PayCode.Equals("2") && PayrollLt.PeriodCode.Equals("C"))) ? "B" : PayrollLt.PeriodCode));
                                            if (!string.IsNullOrEmpty(PayrollLt.PeriodCode))
                                                retResult = InsertPWDetail(PayrollLt, sSalaryItem, decSalaryItem, true);
                                        }
                                        PayrollLt.PeriodCode = "0";
                                    }
                                    else
                                    {
                                        retResult = InsertPWDetail(PayrollLt, sSalaryItem, decSalaryItem, true);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
                
                #region 計算其它薪資
                strSQL = "Select SalaryItem from Payroll_Master_OtherDetail PMO Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' " +
                "    And SalaryYM=" + PayrollLt.SalaryYM.ToString() +
                "    AND NOT EXISTS (select * from Payroll_Master_Detail PMD "+
                "        Where PMD.Company='" + PayrollLt.Company + "' And PMD.EmployeeId='" + PayrollLt.EmployeeId + "' AND PMD.SalaryItem = PMO.SalaryItem"+
                " And Not PMD.SalaryItem in (" + SIList + "))";
                DataTable theDTOher = _MyDBM.ExecuteDataTable(strSQL);

                if (theDTOher.Rows.Count > 0)
                {
                    for (i = 0; i < theDTOher.Rows.Count; i++)
                    {
                        //取得薪資項目代號
                        sSalaryItem = theDTOher.Rows[i][0].ToString().Trim();
                        //把其它薪資項目的金額加入
                        decSalaryItem = GetOtherSalary(PayrollLt, sSalaryItem);

                        totalSalary += decSalaryItem;
                        //下面是找出每個薪資項目的屬性設定
                        strSQL = "Select  * from SalaryStructure_Parameter Where SalaryId='" + sSalaryItem + "'";
                        DataTable theItemDT = _MyDBM.ExecuteDataTable(strSQL);
                        if (theItemDT != null && theItemDT.Rows.Count > 0)
                        {
                            sRegularPay = theItemDT.Rows[0]["RegularPay"].ToString().Trim();
                            if (sRegularPay.Equals("Y"))
                            {//新制提撥退休金使用:Y-是 N-否 
                                fSalary += (float)decSalaryItem;
                            }

                            if (sSalaryItem.Equals("01"))
                            {//底薪BaseSalary固定代號為01
                                PayrollLt.BaseSalary = (int)decSalaryItem;
                            }
                            else
                            {
                                #region 應免稅加減項
                                //NWTax  應免稅別
                                if (theItemDT.Rows[0]["NWTax"].ToString().Trim().Equals("Y"))
                                {//Y- 應稅
                                    //PMType 加減項別
                                    if (theItemDT.Rows[0]["PMType"].ToString().Trim().Equals("A"))
                                    {//A-加項
                                        //Properties 項目屬性:1- 薪資     2- 成本     3- 公司負擔 5- 公司獎金
                                        if (theItemDT.Rows[0]["Properties"].ToString().Trim().Equals("1"))
                                        {//應稅加項（薪資）
                                            PayrollLt.WT_P_Salary += (int)decSalaryItem;
                                        }
                                        else if (theItemDT.Rows[0]["Properties"].ToString().Trim().Equals("5"))
                                        {//應稅加項（獎金）
                                            PayrollLt.WT_P_Bonus += (int)decSalaryItem;
                                        }
                                    }
                                    else
                                    {//B- 減項
                                        //Properties 項目屬性:1- 薪資     2- 成本     3- 公司負擔 5- 公司獎金
                                        if (theItemDT.Rows[0]["Properties"].ToString().Trim().Equals("1"))
                                        {//應稅減項（薪資）
                                            PayrollLt.WT_M_Salary += (int)decSalaryItem;
                                        }
                                        else if (theItemDT.Rows[0]["Properties"].ToString().Trim().Equals("5"))
                                        {//應稅減項（獎金）
                                            PayrollLt.WT_M_Bonus += (int)decSalaryItem;
                                        }
                                    }
                                }
                                else
                                {//N- 免稅 
                                    if (theItemDT.Rows[0]["PMType"].ToString().Trim().Equals("A"))
                                    {//A-加項　　 
                                        //免稅加項
                                        PayrollLt.NT_P += (int)decSalaryItem;
                                    }
                                    else
                                    {//B- 減項
                                        //免稅減項
                                        PayrollLt.NT_M += (int)decSalaryItem;
                                    }
                                }
                                #endregion
                            }
                            //將計算後的薪資項寫入暫存檔                        
                            if (strVersionID.Contains("PICCustomization"))
                            {
                                PayrollLt.PeriodCode = theItemDT.Rows[0]["P1CostSalaryItem"].ToString();
                                if (PayrollLt.PayCode.Equals("3") && PayrollLt.PeriodCode.Equals("C"))
                                {//二者皆有
                                    decimal tempDecimal = 0;
                                    PayrollLt.PeriodCode = "A";
                                    if (OverTimeFee.OvertimeBasic > 0)
                                        tempDecimal = decSalaryItem * OverTimeFee.OvertimeBasic1 / OverTimeFee.OvertimeBasic;
                                    retResult = InsertPWDetail(PayrollLt, sSalaryItem, tempDecimal, true);
                                    tempDecimal = 0;
                                    PayrollLt.PeriodCode = "B";
                                    if (OverTimeFee.OvertimeBasic > 0)
                                        tempDecimal = decSalaryItem * OverTimeFee.OvertimeBasic2 / OverTimeFee.OvertimeBasic;
                                    retResult = InsertPWDetail(PayrollLt, sSalaryItem, tempDecimal, true);

                                }
                                else if (PayrollLt.PayCode.Equals("1") || PayrollLt.PayCode.Equals("2"))
                                {//工資/非工資           
                                    PayrollLt.PeriodCode = (((PayrollLt.PayCode.Equals("1") && PayrollLt.PeriodCode.Equals("A"))
                                        || (PayrollLt.PayCode.Equals("1") && PayrollLt.PeriodCode.Equals("C"))) ? "A" : (
                                        ((PayrollLt.PayCode.Equals("2") && PayrollLt.PeriodCode.Equals("B"))
                                        || (PayrollLt.PayCode.Equals("2") && PayrollLt.PeriodCode.Equals("C"))) ? "B" : ""));
                                    if (!string.IsNullOrEmpty(PayrollLt.PeriodCode))
                                        retResult = InsertPWDetail(PayrollLt, sSalaryItem, decSalaryItem, true);
                                }
                                PayrollLt.PeriodCode = "0";
                            }
                            else
                            {                                
                                retResult = InsertPWDetail(PayrollLt, sSalaryItem, decSalaryItem, true);
                            }
                        }
                    }
                }
                #endregion

                //減項
                #region 取得代扣稅款金額與所得稅率
                int TaxSum;
                if (strVersionID.Contains("PICCustomization"))
                {
                    if (PayrollLt.PayCode.Equals("1") || PayrollLt.PayCode.Equals("3"))
                    {
                        //薪資
                        PayrollLt.PeriodCode = "A";
                        TaxSum = GetTaxSum(PayrollLt, 1);
                        IncomeTax.Tax mTax = IncomeTax.GetIncomeTax(TaxSum, PayrollLt);

                        totalSalary -= mTax.Amount;
                        PayrollLt.TaxRate = (decimal)mTax.Rate;
                        //將計算後的薪資項寫入暫存檔
                        retResult = InsertPWDetail(PayrollLt, "03", mTax.Amount, true);
                    }
                    
                    if (PayrollLt.PayCode.Equals("2") || PayrollLt.PayCode.Equals("3"))
                    {
                        //非薪資
                        PayrollLt.PeriodCode = "B";
                        TaxSum = GetTaxSum(PayrollLt, 1);
                        IncomeTax.Tax mTax = IncomeTax.GetIncomeTax(TaxSum, PayrollLt);

                        totalSalary -= mTax.Amount;
                        //將計算後的薪資項寫入暫存檔
                        retResult = InsertPWDetail(PayrollLt, "03", mTax.Amount, true);
                    }
                    //---------------------------
                    PayrollLt.PeriodCode = "0";
                }
                else
                {
                    TaxSum = GetTaxSum(PayrollLt, 1);
                    IncomeTax.Tax mTax = IncomeTax.GetIncomeTax(TaxSum, PayrollLt);

                    totalSalary -= mTax.Amount;
                    PayrollLt.TaxRate = (decimal)mTax.Rate;
                    //將計算後的薪資項寫入暫存檔
                    retResult = InsertPWDetail(PayrollLt, "03", mTax.Amount, true);
                }
                #endregion

                #region FOR第二期時請假扣薪使用
                if (PayrollLt.LeaveHours_deduction > 0)
                {
                    if (strVersionID.Contains("PICCustomization"))
                    {
                        strSQL = "Select Sum(" + PayrollLt.DeCodeKey + "([SalaryAmount])) from ["+PayrollTable+"_Detail] " +
                        " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId +
                        "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() + " and [PeriodCode]='B'";
                        theDT = _MyDBM.ExecuteDataTable(strSQL);

                        if (Convert.ToDecimal(theDT.Rows[0][0].ToString()) > 0)
                        {
                            PayrollLt.PeriodCode = "B";
                            decSalaryItem = Convert.ToDecimal(Convert.ToDecimal(theDT.Rows[0][0].ToString()) * PayrollLt.LeaveHours_deduction / 240);
                            retResult = InsertPWDetail(PayrollLt, "10", decSalaryItem, true);
                            PayrollLt.PeriodCode = "0";
                        }
                    }
                }
                #endregion

                #region 寫入[薪資工作主檔]
                strSQL = "Select count(*) from ["+PayrollTable+"_Heading] " +
                " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() + " and [PeriodCode]='" + PayrollLt.PeriodCode + "'";
                
                DataTable tempDTM = _MyDBM.ExecuteDataTable(strSQL);
                if (((int)tempDTM.Rows[0][0]) > 0)
                {
                    //修改[薪資工作主檔]
                    strSQL = "Update ["+PayrollTable+"_Heading] Set [BaseSalary] = @BaseSalary " +
                        "      ,[Paydays] = @Paydays " +
                        "      ,[LeaveHours_deduction] = @LeaveHours_deduction " +
                        "      ,[TaxRate] = @TaxRate " +
                        "      ,[NT_P] = @NT_P " +
                        "      ,[WT_P_Salary] = @WT_P_Salary " +
                        "      ,[NT_M] = @NT_M " +
                        "      ,[LI_Fee] = @LI_Fee " +
                        "      ,[ResignCode] = @ResignCode " +
                        "      ,[HI_Fee] = @HI_Fee " +
                        "      ,[HI_Person] = @HI_Person " +
                        "      ,[WT_P_Bonus] = @WT_P_Bonus " +
                        "      ,[WT_M_Salary] =@WT_M_Salary " +
                        "      ,[WT_M_Bonus] = @WT_M_Bonus " +
                        "      ,[P1_borrowing] = @P1_borrowing " +
                        "      ,[WT_Overtime] = @WT_Overtime " +
                        "      ,[NT_Overtime] = @NT_Overtime " +
                        "      ,[OnWatch] = @OnWatch" +
                        "      ,[WT_Overtime_Fee] = @WT_Overtime_Fee " +
                        "      ,[NT_Overtime_Fee] = @NT_Overtime_Fee " +
                        "      ,[OnWatch_Fee] = @OnWatch_Fee" +
                        "      ,[Dependent1_IDNo] = @Dependent1_IDNo" +
                        "      ,[Dependent1_HI_Fee] = @Dependent1_HI_Fee" +
                        "      ,[Dependent2_IDNo] = @Dependent2_IDNo" +
                        "      ,[Dependent2_HI_Fee] = @Dependent2_HI_Fee" +
                        "      ,[Dependent3_IDNo] = @Dependent3_IDNo" +
                        "      ,[Dependent3_HI_Fee] = @Dependent3_HI_Fee" +
                        " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId +
                        "' and [SalaryYM]=" + PayrollLt.SalaryYM.ToString() + " and [PeriodCode]='" + PayrollLt.PeriodCode + "'";
                }
                else
                {
                    //寫入[薪資工作主檔]
                    strSQL = "INSERT INTO ["+PayrollTable+"_Heading] ([Company],[EmployeeId],[SalaryYM],[PeriodCode],[BaseSalary],[Paydays],[LeaveHours_deduction],[TaxRate],[NT_P]" +
                        ",[WT_P_Salary],[NT_M],[LI_Fee],[ResignCode],[HI_Fee],[HI_Person],[WT_P_Bonus],[WT_M_Salary],[WT_M_Bonus],[P1_borrowing],[WT_Overtime],[NT_Overtime],[OnWatch],[WT_Overtime_Fee],[NT_Overtime_Fee],[OnWatch_Fee]" +
                        ",[Dependent1_IDNo],[Dependent1_HI_Fee],[Dependent2_IDNo],[Dependent2_HI_Fee],[Dependent3_IDNo],[Dependent3_HI_Fee])" +
                        " Select @Company,@EmployeeId,@SalaryYM,@PeriodCode,@BaseSalary,@Paydays,@LeaveHours_deduction,@TaxRate,@NT_P,@WT_P_Salary,@NT_M,@LI_Fee,@ResignCode" +
                        ",@HI_Fee,@HI_Person,@WT_P_Bonus,@WT_M_Salary,@WT_M_Bonus,@P1_borrowing,@WT_Overtime,@NT_Overtime,@OnWatch,@WT_Overtime_Fee,@NT_Overtime_Fee,@OnWatch_Fee" +
                        ",@Dependent1_IDNo,@Dependent1_HI_Fee,@Dependent2_IDNo,@Dependent2_HI_Fee,@Dependent3_IDNo,@Dependent3_HI_Fee ";
                }

                if (string.IsNullOrEmpty(PayrollLt.PeriodCode) == false)
                {
                    System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
                    MyCmd.Parameters.Add("@Company", SqlDbType.Char, 2).Value = PayrollLt.Company;
                    MyCmd.Parameters.Add("@EmployeeId", SqlDbType.Char, 10).Value = PayrollLt.EmployeeId;
                    MyCmd.Parameters.Add("@SalaryYM", SqlDbType.Decimal, 6).Value = PayrollLt.SalaryYM;
                    MyCmd.Parameters.Add("@PeriodCode", SqlDbType.Char, 1).Value = PayrollLt.PeriodCode;
                    MyCmd.Parameters.Add("@BaseSalary", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.MonthlyBaseSalary);
                    MyCmd.Parameters.Add("@Paydays", SqlDbType.Decimal, 5).Value = PayrollLt.Paydays;
                    MyCmd.Parameters.Add("@LeaveHours_deduction", SqlDbType.Decimal, 5).Value = PayrollLt.LeaveHours_deduction;
                    MyCmd.Parameters.Add("@TaxRate", SqlDbType.Decimal, 5).Value = PayrollLt.TaxRate;
                    MyCmd.Parameters.Add("@NT_P", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.NT_P);
                    MyCmd.Parameters.Add("@WT_P_Salary", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.WT_P_Salary);
                    MyCmd.Parameters.Add("@NT_M", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.NT_M);
                    MyCmd.Parameters.Add("@LI_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.LI_Fee);
                    MyCmd.Parameters.Add("@ResignCode", SqlDbType.Char, 1).Value = PayrollLt.ResignCode;
                    MyCmd.Parameters.Add("@HI_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.HI_Fee);
                    MyCmd.Parameters.Add("@HI_Person", SqlDbType.Decimal, 1).Value = PayrollLt.HI_Person;
                    MyCmd.Parameters.Add("@WT_P_Bonus", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.WT_P_Bonus);
                    MyCmd.Parameters.Add("@WT_M_Salary", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.WT_M_Salary);
                    MyCmd.Parameters.Add("@WT_M_Bonus", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.WT_M_Bonus);
                    MyCmd.Parameters.Add("@P1_borrowing", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.P1_borrowing);
                    MyCmd.Parameters.Add("@WT_Overtime", SqlDbType.Decimal, 5).Value = PayrollLt.WT_Overtime;
                    MyCmd.Parameters.Add("@NT_Overtime", SqlDbType.Decimal, 5).Value = PayrollLt.NT_Overtime;
                    MyCmd.Parameters.Add("@OnWatch", SqlDbType.Decimal, 5).Value = PayrollLt.OnWatch;
                    MyCmd.Parameters.Add("@WT_Overtime_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.WT_Overtime_Fee);
                    MyCmd.Parameters.Add("@NT_Overtime_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.NT_Overtime_Fee);
                    MyCmd.Parameters.Add("@OnWatch_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.OnWatch_Fee);
                    MyCmd.Parameters.Add("@Dependent1_IDNo", SqlDbType.Char, 10).Value = (PayrollLt.Dependent1_IDNo == null) ? "" : PayrollLt.Dependent1_IDNo;
                    MyCmd.Parameters.Add("@Dependent1_HI_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.Dependent1_HI_Fee);
                    MyCmd.Parameters.Add("@Dependent2_IDNo", SqlDbType.Char, 10).Value = (PayrollLt.Dependent2_IDNo == null) ? "" : PayrollLt.Dependent2_IDNo;
                    MyCmd.Parameters.Add("@Dependent2_HI_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.Dependent2_HI_Fee);
                    MyCmd.Parameters.Add("@Dependent3_IDNo", SqlDbType.Char, 10).Value = (PayrollLt.Dependent3_IDNo == null) ? "" : PayrollLt.Dependent3_IDNo;
                    MyCmd.Parameters.Add("@Dependent3_HI_Fee", SqlDbType.VarChar, 100).Value = EnCodeAmount(PayrollLt.Dependent3_HI_Fee);
                    retResult = _MyDBM.ExecuteCommand(strSQL, MyCmd.Parameters, CommandType.Text);
                }
                #endregion
            }
        }
    

        return retResult;
    }

    /// <summary>
    /// 確認試算資料
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="YearMonth">計薪年月</param>
    /// <param name="PeriodCode">計薪期別</param>
    /// <returns></returns>
    public int ConfirmDraftPayroll(string Company, string YearMonth, string PeriodCode,out DataTable Dt)
    {      
        bool blPermission = false;
        int retResult = -99;
        Dt = null;

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //確認是否有確認試算的權限
        blPermission = _UserInfo.CheckSysPermission("ePayroll", "PYM015");

        if (blPermission == true)
        {
            //將試算表中的資料搬到歷史區
            retResult = 0;

            SalaryYearMonth sYM = new SalaryYearMonth();
            sYM = CheckSalaryYM(Company, YearMonth, PeriodCode);
            if (sYM.isControl == false) 
            {//未經控管
                retResult = -2;
            }
            else if (string.IsNullOrEmpty(sYM.DraftDate))
            {
                //指定公司於指定計薪年月未經試算
                retResult = -3;
            }
            else if (!string.IsNullOrEmpty(sYM.ConfirmDate))
            {
                //指定公司於指定計薪年月已完成確認試算,不可再進行確認
                retResult = -4;
            }
            else if (sYM.NextConfirmYM > 100000 && sYM.NextConfirmYM.ToString() != YearMonth)
            {
                //指定之計薪年月,不是待確認之年月
                retResult = -6;
            }
            else if (sYM.beReDraft)
            {
                //指定之計薪年月,須重新試算
                retResult = -7;
            }
            else
            {
                string DeCodeKey = "dbo.DCK" + DateTime.Today.ToString("yyyyMMddmmss");
                BeforeQuery(DeCodeKey);
                try
                {//2012/02/14 kaya ConfirmDraftPayroll:改為使用DB SP[dbo.sp_PY_ConfirmDraftPayroll]執行以便控管及ROLLBACK
                    strSQL = "dbo.sp_PY_ConfirmDraftPayroll";
                    //參數                   
                    System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand();                    
                    sqlcmd.Parameters.Add("@ls_CompanyId", System.Data.SqlDbType.VarChar).Value = Company.Trim();
                    sqlcmd.Parameters.Add("@ls_SalaryYM", System.Data.SqlDbType.VarChar).Value = YearMonth.Trim();
                    sqlcmd.Parameters.Add("@ls_PeriodCode", System.Data.SqlDbType.VarChar).Value = PeriodCode.Trim();
                    sqlcmd.Parameters.Add("@ls_Key", System.Data.SqlDbType.VarChar).Value = DeCodeKey;
                    sqlcmd.Parameters.Add("@ls_UserID", System.Data.SqlDbType.VarChar).Value = _UserInfo.UData.UserId;
                    DataSet ds;
                    retResult = _MyDBM.ExecStoredProcedure(strSQL, sqlcmd.Parameters, out ds);
                    if (ds != null && ds.Tables.Count > 0)
                        Dt = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    retResult = -5;
                    _UserInfo.SysSet.WriteToLogs("Error", "執行SQL指令時發生錯誤(ConfirmDraftPayroll)\n\r" + "Error Message:" + ex.Message + "\n\rSQLCommand:" + strSQL + "\n\r");
                }
                AfterQuery(DeCodeKey);
            }
        }

        return retResult;
    }

    /// <summary>
    /// 到職日
    /// </summary>
    /// <returns>傳回到職日</returns>
    /// Johnny
    public static DateTime GetHireDate(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //找員工到職 離職
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT Company ,EmployeeId,HireDate,ResignDate FROM Personnel_Master  WHERE COMPANY ='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");
        DateTime HireDate = DateTime.Parse(DT.Rows[0]["HireDate"].ToString());

        return HireDate;

    }

    /// <summary>
    /// 結婚
    /// </summary>
    /// <returns>傳回是否結婚</returns>
    /// Johnny
    public static bool GetMaritalStatus(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //找員工到職 離職
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT Company ,EmployeeId,MaritalStatus FROM Personnel_Master  WHERE COMPANY ='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");

        if (DT.Rows.Count > 0 && DT.Rows[0]["MaritalStatus"].ToString() == "1")
            return true;
        else
            return false;
        
    

    }


    /// <summary>
    /// 離職日
    /// </summary>
    /// <returns>傳回離職日</returns>
    /// Johnny
    public static DateTime GetResignDate(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //找員工到職 離職
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT Company ,EmployeeId,HireDate,ResignDate FROM Personnel_Master  WHERE COMPANY ='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");

        if (DT.Rows.Count > 0 && string.IsNullOrEmpty ( DT.Rows[0]["ResignDate"].ToString()) == false)
        {
            DateTime ResignDate = DateTime.Parse(DT.Rows[0]["ResignDate"].ToString());
            return ResignDate;
        }
        else
            return DateTime.MinValue;
    }

    /// <summary>
    /// 固定薪資
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="personalID">員工帳號</param>
    /// <returns>傳回固定薪資</returns>
    /// Johnny
    public int GetFixedSalary(PayrolList PayrollLt)
    {
        bool blPermission = false;

        DataTable theDT = null;

        float fSalary = 0;
        decimal totalSalary = 0, decSalaryItem = 0;
        string sSalaryItem = "", sRegularPay = "";        

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //確認是否有試算權限
        blPermission = _UserInfo.CheckSysPermission("ePayroll", "PYM013");

        if (blPermission == true)
        {
            //開始取得薪資項目
            strSQL = "Select * from Payroll_Master_Detail Where Company='" + PayrollLt.Company + "' And EmployeeId='" + PayrollLt.EmployeeId + "' ";
            theDT = _MyDBM.ExecuteDataTable(strSQL);
            if (theDT != null)
            {
                if (string.IsNullOrEmpty(PayrollLt.PeriodCode))
                    PayrollLt.PeriodCode = "0";

                if (theDT.Rows.Count > 0)
                {
                    for (int i = 0; i < theDT.Rows.Count; i++)
                    {
                        sSalaryItem = theDT.Rows[i]["SalaryItem"].ToString().Trim();
                        //使用薪資參數結構找出實際的值(含破月計算)
                        if (!(sSalaryItem.Equals("03") || sSalaryItem.Equals("04") || sSalaryItem.Equals("05") || sSalaryItem.Equals("06")))
                        {//跳過特殊計算項目不計
                            string strPersonalSalaryItem = GetPersonalSalaryItem(PayrollLt, sSalaryItem);
                            try
                            {
                                decSalaryItem = Convert.ToDecimal(strPersonalSalaryItem);
                            }
                            catch { }
                            totalSalary += decSalaryItem;
                            strSQL = "Select * from SalaryStructure_Parameter Where SalaryId='" + sSalaryItem + "'";
                            DataTable theItemDT = _MyDBM.ExecuteDataTable(strSQL);
                            if (theItemDT != null && theItemDT.Rows.Count > 0)
                            {
                                if (sSalaryItem.Equals("01"))
                                {
                                    PayrollLt.BaseSalary = (int)decSalaryItem;
                                }

                                sRegularPay = theItemDT.Rows[0]["RegularPay"].ToString().Trim();
                                if (sRegularPay.Equals("Y"))
                                {//新制提撥退休金使用:Y-是 N-否 
                                    fSalary += (float)decSalaryItem;
                                }
                            }
                        }
                    }
                }
            }
        }

        return (int)fSalary;
    }

    /// <summary>
    /// 傳回資料庫中代表空值日期
    /// </summary>
    /// <returns></returns>
    private static DateTime GetDateNull()
    {
        //1912/1/1 上午 12:00:00
        return new DateTime(1912, 1, 1, 12, 0, 0);
    }

    /// <summary>
    /// 取得離職當月在職率
    /// </summary>
    /// <returns></returns>
    /// Johnny
    public static float GetResignDayRate(string mCompany, string mEmployeeId, string YearDay)
    {
        DateTime mResignDate = Payroll.GetResignDate(mCompany, mEmployeeId);

        string CalculationYear = YearDay.Substring(0, 4);
        string CalculationMonths = YearDay.Substring(4, 2);


        if (mResignDate.Year.ToString() == CalculationYear && mResignDate.Month.ToString() == CalculationMonths)
        {
            if (mResignDate.Day >= 30)
                return 1.0f;
            else
                return (float)mResignDate.Day / 30;
        }
        else
            return 1.0f;


    }

    /// <summary>
    /// 查詢開始前,設定查詢專用關鍵函式
    /// </summary>
    /// <param name="strQueryKey">函式名稱</param>
    /// <returns></returns>
    public int BeforeQuery(string strQueryKey)
    {
        string strSqlCommand = "";
        int i = -1;
        DBManger _MyDBM = new DBManger();
        _MyDBM.New();

        #region 加密
        strSqlCommand = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strQueryKey + "To') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) DROP FUNCTION " + strQueryKey + "To ";
        i = _MyDBM.ExecuteCommand(strSqlCommand);

        strSqlCommand = " CREATE FUNCTION " + strQueryKey + "To(@UnDeCode varchar(50)) " +
            " RETURNS varchar(100) " +
            " AS " +
            " BEGIN " +
            "	DECLARE @i			int " +
            "	DECLARE @tempStr	varchar(1) " +
            "	DECLARE @tempString	varchar(100) " +
            "	set @UnDeCode=Case When IsNull(@UnDeCode,'')='' then '' else Ltrim(Rtrim(@UnDeCode)) End " +
            "	set @tempStr='' " +
            "	set @tempString='' " +
            "	set @i=1 " +
            "	If Len(@UnDeCode)<1 RETURN '' " +
            "	WHILE (@i <= Len(@UnDeCode)) " +
            "	BEGIN " +
            "		set @tempStr=Substring(@UnDeCode,@i,1)" +
            "		set @tempString= " +
            "		 CASE @tempStr " +
            "         WHEN '-' THEN @tempString+'3#' " +
            "         WHEN '.' THEN @tempString+'3$' " +
            "         WHEN '1' THEN @tempString+'1)' " +
            "         WHEN '2' THEN @tempString+'1*' " +
            "         WHEN '3' THEN @tempString+'1+' " +
            "         WHEN '4' THEN @tempString+'1,' " +
            "         WHEN '5' THEN @tempString+'1-' " +
            "         WHEN '6' THEN @tempString+'1.' " +
            "         WHEN '7' THEN @tempString+'1/' " +
            "         WHEN '8' THEN @tempString+'10' " +
            "         WHEN '9' THEN @tempString+'11' " +
            "         WHEN '0' THEN @tempString+'1(' " +
            "         ELSE @tempString " +
            "		END " +
            "		set @i=@i+1 " +
            "	End " +
            "	 " +
            "	If @tempString='1(' RETURN '' " +
            "	set @tempString=Replace(@tempString,'1(1(1(','2c') " +
            "	set @tempString=Replace(@tempString,'1(1(','2W') " +
            "	RETURN @tempString " +
            " END " +
            " ";
        i = _MyDBM.ExecuteCommand(strSqlCommand);
        #endregion

        #region 解密
        strSqlCommand = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strQueryKey + "') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) DROP FUNCTION " + strQueryKey + "";
        i = _MyDBM.ExecuteCommand(strSqlCommand);

        strSqlCommand = " CREATE FUNCTION " + strQueryKey + "(@UnDeCode varchar(100)) " +
            " RETURNS decimal(38,2) " +
            " AS " +
            " BEGIN " +
            "	DECLARE @i			int			" +
            "	DECLARE @tempStr	varchar(2)  " +
            "	DECLARE @tempString	varchar(38) " +
            "	set @UnDeCode=Case When IsNull(@UnDeCode,'')='' then '' else Ltrim(Rtrim(@UnDeCode)) End " +
            "	set @tempStr='' " +
            "	set @tempString='' " +
            "	set @i=1 " +
            "   If Len(@UnDeCode)<=1 RETURN 0 " +
            "	WHILE (@i < Len(@UnDeCode)) " +
            "	BEGIN " +
            "	 set @tempStr=Substring(@UnDeCode,@i,2) " +
            "	 set @tempString= " +
            "	  CASE @tempStr " +
            "      WHEN '3#' THEN @tempString+'-' " +
            "      WHEN '3$' THEN @tempString+'.' " +
            "      WHEN '1)' THEN @tempString+'1' " +
            "      WHEN '1*' THEN @tempString+'2' " +
            "      WHEN '1+' THEN @tempString+'3' " +
            "      WHEN '1,' THEN @tempString+'4' " +
            "      WHEN '1-' THEN @tempString+'5' " +
            "      WHEN '1.' THEN @tempString+'6' " +
            "      WHEN '1/' THEN @tempString+'7' " +
            "      WHEN '10' THEN @tempString+'8' " +
            "      WHEN '11' THEN @tempString+'9' " +
            "      WHEN '1(' THEN @tempString+'0' " +
            "      WHEN '2W' THEN @tempString+'00' " +
            "      WHEN '2c' THEN @tempString+'000' " +
            "      ELSE @tempString+'0' " +
            "	  END " +
            "	 set @i=@i+2 " +
            "	End " +
            "	RETURN CAST(@tempString AS decimal(20,2)) " +
            " END " +
            " ";
        i = _MyDBM.ExecuteCommand(strSqlCommand);
        #endregion
        return i;
    }

    /// <summary>
    /// 查詢結束後,刪除查詢專用關鍵函式
    /// </summary>
    /// <param name="strQueryKey">函式名稱</param>
    /// <returns></returns>
    public int AfterQuery(string strQueryKey)
    {
        string strSqlCommand = "";
        int i = -1;
        DBManger _MyDBM = new DBManger();
        _MyDBM.New();

        strSqlCommand = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strQueryKey + "To') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) DROP FUNCTION " + strQueryKey + "To ";
        i = _MyDBM.ExecuteCommand(strSqlCommand);

        strSqlCommand = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'" + strQueryKey + "') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) DROP FUNCTION " + strQueryKey + "";        
        i = _MyDBM.ExecuteCommand(strSqlCommand);

        return i;
    }

    /// <summary>
    /// For薪資二重加密
    /// </summary>
    /// <param name="dAmount">數字</param>
    /// <returns></returns>
    public string EnCodeAmount(Decimal dAmount)
    {
        string strEnCode = "";
        if (dAmount != 0)
        {
            strEnCode = _UserInfo.SysSet.rtnHash(dAmount.ToString());
            strEnCode = strEnCode.Replace("1(1(1(", "2c");
            strEnCode = strEnCode.Replace("1(1(", "2W");
        }
        return strEnCode;
    }

    /// <summary>
    /// For薪資二重加密
    /// </summary>
    /// <param name="dAmount">數字之文字格式</param>
    /// <returns></returns>
    public string EnCodeAmount(string sAmount)
    {
        string strEnCode = "";
        if (!string.IsNullOrEmpty(sAmount))
        {
            strEnCode = _UserInfo.SysSet.rtnHash(sAmount);
            strEnCode = strEnCode.Replace("1(1(1(", "2c");
            strEnCode = strEnCode.Replace("1(1(", "2W");
        }
        return strEnCode;
    }

    /// <summary>
    /// For薪資二重解密
    /// </summary>
    /// <param name="sAmount">已加密字串</param>
    /// <returns>解密後之數字</returns>
    public decimal DeCodeAmount(string sAmount)
    {
        string strEnCode = sAmount;
        decimal dDeCode = 0;
        if (!string.IsNullOrEmpty(sAmount))
        {
            strEnCode = strEnCode.Replace("2W", "1(1(");
            strEnCode = strEnCode.Replace("2c", "1(1(1(");
            strEnCode = _UserInfo.SysSet.rtnTrans(strEnCode);
            try
            {
                dDeCode = Convert.ToDecimal(strEnCode);
            }
            catch
            {
                dDeCode = 0;
            }
        }
        return dDeCode;
    }
}