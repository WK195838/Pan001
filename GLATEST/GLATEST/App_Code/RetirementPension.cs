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
/// 退休金
/// </summary>
public class RetirementPension
{


    /// <summary>
    /// 月提繳工資
    /// </summary>
    /// <param name="mSalary"></param>
    /// <returns></returns>
    public static Pensionaccounts GetMonthlyActualSalary(Payroll.PayrolList PayrollLt, int LIdays)
    {
        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        string mTime = PayrollLt.SalaryYM.ToString();

        //找退休金主檔
        PensionaccountsList PL = GetPensionaccounts_Master(mCompany, mEmployeeId, mTime);
        Pensionaccounts Pensionaccounts = new Pensionaccounts();

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        bool isBoss = false;
        #region isBoss為Y時,表示其為不需扣勞退之業主
        string strSql = "SELECT IsNull([isBoss],'N') as isBoss,[LISubsidy] " +
             " FROM [Personnel_Master] " +
             " Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
        DataTable DT = _MyDBM.ExecuteDataTable(strSql);
        if (DT != null && DT.Rows.Count == 1)
        {
            if (DT.Rows[0]["isBoss"].ToString().Equals("Y"))
            {
                return Pensionaccounts;
            }
            else if (DT.Rows[0]["isBoss"].ToString().Equals("M"))
            {
                isBoss = true;
            }
        }
        #endregion

        if (PL.OldnewCode)
        {
            //取得離職當月在職率，未離職為 1
            float mResignDayRate = Payroll.GetResignDayRate(mCompany, mEmployeeId, mTime);
            //2011/11/04 應財務要求破月計算
            try
            {
                if (LIdays > 30)
                    mResignDayRate = 1;
                else if (LIdays > 0)
                    mResignDayRate = (float)LIdays / 30;
            }
            catch { }

            //取得該提繳工資
            int mWages;
            
            Payroll mPayroll = new Payroll();
            //取得個人薪資
            mWages = 0;
            try
            {
                mWages = int.Parse(mPayroll.GetPersonalSalaryItem(PayrollLt, "17"));
            }
            catch { }

            //2011/05/17 kaya 依泛太要求,使用薪資參數設定
            try
            {//找出投保用級距之基數
                //2011/07/215 kaya 依財務Ivy確認,勞退提撥同健保費以上月薪資做基準,無則依當時所設薪資項目做計算
                Payroll.PayrolList PayrollLtLast = PayrollLt;
                PayrollLtLast.SalaryYM = (PayrollLtLast.SalaryYM % 100 == 1) ? (PayrollLtLast.SalaryYM - 89) : (PayrollLtLast.SalaryYM - 1);

                string tempPS = mPayroll.GetPersonalSalaryItem(PayrollLtLast, "17", true);
                int temp17 = Convert.ToInt32(Convert.ToDecimal(tempPS));          
                if (temp17 > 0)
                    mWages = temp17;
            }
            catch
            {
            }
            //取得薪資級距
            mWages = GetM_Contribution_Wages((float)mWages);

            Pensionaccounts.Insured = Computer.Round(mWages * mResignDayRate * PL.Emp_rate);
            if (isBoss == false)
                Pensionaccounts.Company = Computer.Round(mWages * mResignDayRate * PL.CompanyRate);
            else
                Pensionaccounts.Company = 0;
            Pensionaccounts.Insured_Amount = mWages;
            Pensionaccounts.OldnewCode = true; //新制
            return Pensionaccounts;
        }
        else
        {
            Pensionaccounts.Insured = 0;
            Pensionaccounts.Company = 0;
            Pensionaccounts.Insured_Amount = 0;
            Pensionaccounts.OldnewCode = false; //舊制

            return Pensionaccounts;
        }
    }


    public static PensionaccountsList GetPensionaccounts_Master(string mCompany, string mEmployeeId, string mTime)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        //2011/05/06 KAYA 改寫,用以預設員工勞退新制
        //2013/05/20 KAYA 改寫,用以在計算非當月薪資時,取得計算薪資月最後變更前參數
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT PM.[Company],PM.[EmployeeId],[IDNo],[pensionaccount],[OldnewCode],[NewSystem_Date] " +
            " ,(Case When IsNull([Emp_rate],0) > 0 And IsNull([Emp_rate],0) < 1 then IsNull([Emp_rate],0) else " +
            "  (Case When IsNull([Emp_rate],0) > 1 then IsNull([Emp_rate],0)/100 else 0.00 end) end) [Emp_rate]" +
            " ,(Case When IsNull([CompanyRate],0) > 0 And IsNull([CompanyRate],0) < 1 then [CompanyRate] else " +
            "  (Case When IsNull([CompanyRate],0) > 1 then ([CompanyRate]/100) else 0.00 end) end) [CompanyRate]" +            
            " ,IsNull((Select [CompanyBurdenRate] From [Pensionaccounts_Parameter]),0) [LastCompanyRate] " +
            " ,IsNull([MonthlyActualSalary],0) [MonthlyActualSalary]" +
            " ,[EmpRate_changeyear],[CompanyRate_changeyear],IsNull([ActualTotalamount_S],0) [ActualTotalamount_S]" +
            " ,IsNull([ActualTotalamount_C],0) [ActualTotalamount_C],[EffectiveDate] "+
            " FROM [Personnel_Master] PM Left Join "+
            @" (select PA.[Company]
      ,PA.[EmployeeId]
      ,[pensionaccount]
      ,[OldnewCode]
      ,[NewSystem_Date]
      ,ISNull(Emp_rate_change,[Emp_rate]) [Emp_rate]
      ,ISNull(CompanyRate_change,[CompanyRate]) [CompanyRate]
      ,ISNull(PT.[MonthlyActualSalary],PA.[MonthlyActualSalary]) [MonthlyActualSalary]
      ,[EmpRate_changeyear]
      ,[CompanyRate_changeyear]
      ,[ActualTotalamount_S]
      ,[ActualTotalamount_C]
      ,[EffectiveDate]
 from Pensionaccounts_Master PA
left join (
SELECT TOP 1 *
  FROM [Pensionaccounts_Transaction]
where EmployeeId='" + mEmployeeId + @"'
and Substring(convert(varchar,[TrxDate],112),1,6)<='" + mTime + @"'
order by [TrxDate] DESC
) PT on PA.[Company]=PT.[Company] and PA.[EmployeeId]=PT.[EmployeeId] 
where PA.EmployeeId='" + mEmployeeId + "' ) PA " +
            " On PM.[Company]=PA.[Company] And PM.[EmployeeId]=PA.[EmployeeId] " +
            " WHERE PM.COMPANY ='" + mCompany + "' and PM.EmployeeId='" + mEmployeeId + "'");
        PensionaccountsList mPL = new PensionaccountsList();

        if (DT.Rows.Count > 0)
        {
            mPL.Company = DT.Rows[0]["Company"].ToString();
            mPL.EmployeeId = DT.Rows[0]["EmployeeId"].ToString();
            if (DT.Rows[0]["pensionaccount"] != null && DT.Rows[0]["pensionaccount"].ToString().Trim() != "")
            {
                mPL.Pensionaccount = DT.Rows[0]["pensionaccount"].ToString();

                mPL.OldnewCode = true;
                if (DT.Rows[0]["OldnewCode"].ToString().ToUpper() != "1")
                    mPL.OldnewCode = false;
            }
            else
            {
                mPL.OldnewCode = true;
                if (DT.Rows[0]["IDNo"] != null) mPL.Pensionaccount = DT.Rows[0]["IDNo"].ToString();
                mPL.Emp_rate = 0;
                mPL.CompanyRate = 0;
                mPL.MonthlyActualSalary = 0;
                mPL.EmpRate_changeyear = "";
                mPL.CompanyRate_changeyear = "";
                mPL.ActualTotalamount_S = "";
                mPL.ActualTotalamount_C = "";
                mPL.EffectiveDate = null;
            }

            if (DT.Rows[0]["NewSystem_Date"] != null) mPL.NewSystem_Date = DT.Rows[0]["NewSystem_Date"].ToString();
            if (DT.Rows[0]["Emp_rate"] != null) mPL.Emp_rate = float.Parse(DT.Rows[0]["Emp_rate"].ToString());

            float CR = 0, LCR = 0;
            if (DT.Rows[0]["CompanyRate"] != null) CR = float.Parse(DT.Rows[0]["CompanyRate"].ToString());
            if (DT.Rows[0]["LastCompanyRate"] != null) LCR = float.Parse(DT.Rows[0]["LastCompanyRate"].ToString());
            mPL.CompanyRate = (CR > LCR) ? CR : LCR;

            if (DT.Rows[0]["MonthlyActualSalary"] != null) mPL.MonthlyActualSalary = int.Parse(DT.Rows[0]["MonthlyActualSalary"].ToString());
            if (DT.Rows[0]["EmpRate_changeyear"] != null) mPL.EmpRate_changeyear = DT.Rows[0]["EmpRate_changeyear"].ToString();
            if (DT.Rows[0]["CompanyRate_changeyear"] != null) mPL.CompanyRate_changeyear = DT.Rows[0]["CompanyRate_changeyear"].ToString();
            if (DT.Rows[0]["ActualTotalamount_S"] != null) mPL.ActualTotalamount_S = DT.Rows[0]["ActualTotalamount_S"].ToString();
            if (DT.Rows[0]["ActualTotalamount_C"] != null) mPL.ActualTotalamount_C = DT.Rows[0]["ActualTotalamount_C"].ToString();
            if (DT.Rows[0]["EffectiveDate"] != null) mPL.EffectiveDate = DT.Rows[0]["EffectiveDate"].ToString();
        }

        return mPL;
    }


    /// <summary>
    /// 取得級數
    /// </summary>
    /// <param name="mCompany"></param>
    /// <param name="mEmployeeId"></param>
    /// <returns></returns>
    public static int GetM_Contribution_Wages(float mSalary)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("SELECT top 1 * FROM LaborRetirementPension where UpperLimit>= " + mSalary.ToString() + " and LowerLimit<= " + mSalary.ToString() + " or LowerLimit<= " + mSalary.ToString() + " ORDER BY  M_Contribution_Wages DESC");
        int intWages = int.Parse(DT.Rows[0]["M_Contribution_Wages"].ToString());
        return intWages;
    }

    /// <summary>
    /// 計算退休金投保
    /// </summary>
    public struct Pensionaccounts
    {
        /// <summary>
        /// 退休金制度選擇碼
        /// </summary>
        public bool OldnewCode;

        /// <summary>
        /// 投保金額
        /// </summary>
        public int Insured_Amount;

        /// <summary>
        /// 公司
        /// </summary>
        public int Company;

        /// <summary>
        /// 被保險人 (員工)
        /// </summary>
        public int Insured;
    }


    /// <summary>
    /// 薪資主檔
    /// </summary>
    public struct PensionaccountsList
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
        /// 勞工退休金個人專戶
        /// </summary>
        public string Pensionaccount;
        /// <summary>
        /// 退休金制度選擇碼 (新1 舊2)
        /// </summary>
        public bool OldnewCode;
        /// <summary>
        /// 新制選擇日期
        /// </summary>
        public string NewSystem_Date;
        /// <summary>
        /// 員工自提率
        /// </summary>
        public float Emp_rate;
        /// <summary>
        /// 企業提撥率
        /// </summary>
        public float CompanyRate;
        /// <summary>
        /// 月提繳工資
        /// </summary>
        public int MonthlyActualSalary;
        /// <summary>
        /// 員工自提率變更年度
        /// </summary>
        public string EmpRate_changeyear;
        /// <summary>
        /// 企業提撥率變更年度
        /// </summary>
        public string CompanyRate_changeyear;
        /// <summary>
        /// 員工提繳累積金額
        /// </summary>
        public string ActualTotalamount_S;
        /// <summary>
        /// 企業提撥累積金額
        /// </summary>
        public string ActualTotalamount_C;
        /// <summary>
        /// 異動生效日期
        /// </summary>
        public string EffectiveDate;
    };
}
