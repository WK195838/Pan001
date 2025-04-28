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
/// 全民健保
/// 2012/05/09 kaya 依財務要求,若健保費依投保金額生效晚於調薪日時,以主檔設定之投保金額計算健保費級距
/// 2012/07/26 kaya 修正健保設定後第2個月因不在計薪區間,而未使用[主檔設定之投保金額計算健保費級距]之問題
/// </summary>
public static class HealthInsurance
{
    /// <summary>
    /// 全民健保補助類型
    /// </summary>
    public enum DependentSubsidyEnum : int
    {
        //(1) 身心極重度或重度障礙者，全額由政府補助。
        //(2) 身心中度障礙者，政府補助自己應繳納保險費的1/2。
        //(3) 身心輕度障礙者，政府補助自己應繳納保險費的1/4。

        /// <summary>
        /// 政府全額補助
        /// </summary>
        All = 100,

        /// <summary>
        /// 政府補助1/2
        /// </summary>
        Half = 50,

        /// <summary>
        /// 政府補助1/4
        /// </summary>
        OneFourth = 25,

        /// <summary>
        /// 自付全額
        /// </summary>
        Nothing = 0
    }

    /// <summary>
    /// 全民健保保險費
    /// </summary>
    public struct DependentPremium
    {
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

        /// <summary>
        /// 總投保人數
        /// </summary>
        public int Person;

        /// <summary>
        /// 眷屬
        /// </summary>
        public int Dependent;

        public DependentsEnum[] Dependents;
    };


    public struct DependentsEnum
    {
        /// <summary>
        /// 身分證
        /// </summary>
        public string IDNo;

        /// <summary>
        /// 投保金額
        /// </summary>
        public int Insured_Amount;

        /// <summary>
        /// 補助碼
        /// </summary>
        public DependentSubsidyEnum DependentSubsidy;


    };

    /// <summary>
    /// 保費計算
    /// </summary>
    /// <param name="Amount">薪水</param>
    /// <param name="SubsidyEnum">加保補助類型</param>
    /// <param name="Dependent">眷口加保補助類型</param>
    /// <returns>計算時，是否產生錯誤</returns>
    public static DependentPremium GetPremiumCo(float mSalary, DependentSubsidyEnum mSubsidyEnum, DependentsEnum[] mDependents, bool blBoss)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        DependentPremium mDependentPremium = new DependentPremium();


        //找級距
        //2011/05/05 kaya 加入 Or liamt=(SELECT Max(liamt) FROM HealthInsurance_SeriesParameter) 用以在超過上限時使用
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT TOP 1 *  FROM HealthInsurance_SeriesParameter where liamt >= '" + mSalary.ToString() + "' Or liamt=(SELECT Max(liamt) FROM HealthInsurance_SeriesParameter) ORDER BY liamt");

        int RangeNo = int.Parse(DT.Rows[0]["RangeNo"].ToString()); //級距
        int LiAmt = int.Parse(DT.Rows[0]["LiAmt"].ToString());     //月投保金額
        int Grants = 0;
        
        try
        {
            Grants = Convert.ToInt32(DT.Rows[0]["Grants"].ToString());   //政府補助
        }
        catch
        {

        }
        
        //找參數
        DT = _MyDBM.ExecuteDataTable("SELECT Top 1 * FROM HealthInsurance_Parameter");

        float Rates = float.Parse(DT.Rows[0]["Rates"].ToString());                          //費率
        float EmpBurdenRate = float.Parse(DT.Rows[0]["EmpBurdenRate"].ToString());          //員公負擔比率
        float CompanyBurdenRate = float.Parse(DT.Rows[0]["CompanyBurdenRate"].ToString());  //公司負擔比率
        float Comp_burden_Ave = float.Parse(DT.Rows[0]["Comp_burden_Ave"].ToString());      //含本人及平均眷屬
        
        //原始保費
        double Premium = LiAmt * Rates;
        //投保金額  
        mDependentPremium.Insured_Amount = LiAmt;
        //眷口加保補助類型
        mDependentPremium.Dependents = mDependents;
        //投保人數
        mDependentPremium.Person = (mDependents.Length + 1) + 1; //眷屬+員工本人
        int intAmount = 0;

        if (blBoss == true)
        {//事業主無政府補助及公司補助(以100%計)
            mDependentPremium.Insured = Convert.ToInt32(Premium);
            mDependentPremium.Company = 0;
            //最高繳三個眷口 0 1 2
            for (int i = 0; i < mDependents.Length; i++)
            {
                if (i == 3) break;
                //修正眷屬補助額
                intAmount = Computer.Round(Premium * (100 - (int)mDependents[i].DependentSubsidy) / 100);
                intAmount = (intAmount < 0) ? 0 : intAmount;

                mDependentPremium.Dependent += intAmount;
                mDependents[i].Insured_Amount = intAmount;
            }
        }
        else
        {
            //自付= 原始保費*0.3
            mDependentPremium.Insured = Computer.Round(Premium * EmpBurdenRate * (100 - (int)mSubsidyEnum) / 100) - Grants;
            mDependentPremium.Insured = (mDependentPremium.Insured < 0) ? 0 : mDependentPremium.Insured; //如果員工為全額補助，則不補助差額

            //投保單位=公司負擔 0.6
            mDependentPremium.Company = Computer.Round(Premium * CompanyBurdenRate * Comp_burden_Ave);

            ////眷口排序 取前三最高保費
            ////Array.Sort(mDependents);
            //最高繳三個眷口 0 1 2
            for (int i = 0; i < mDependents.Length; i++)
            {
                if (i == 3) break;
                //修正眷屬補助額
                intAmount = Computer.Round(Premium * EmpBurdenRate * (100 - (int)mDependents[i].DependentSubsidy) / 100) - Grants;
                intAmount = (intAmount < 0) ? 0 : intAmount;

                mDependentPremium.Dependent += intAmount;
                mDependents[i].Insured_Amount = intAmount;
            }
        }

        return mDependentPremium;
    }


    /// <summary>
    /// 保費計算
    /// </summary>
    /// <param name="mCompany"></param>
    /// <param name="mEmployeeId"></param>
    /// <returns></returns>
    public static DependentPremium GetPremium(Payroll.PayrolList PayrollLt)
    {
        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        bool blBoss = false;
        UserInfo _UserInfo = new UserInfo();
        string PayrollTable = "PayrollWorking";
        #region 停保者不代扣健保
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT Count(*)  FROM HealthInsurance_Heading Where IsNull([Suspends],'') != 'Y' And [Company]='" + mCompany + "'  And [EmployeeId] = '" + mEmployeeId + "'");
        if (DT != null && DT.Rows[0].ToString().Equals("0"))
        {
            DependentPremium thStopHI= new DependentPremium();
            thStopHI.Company = 0;
            thStopHI.Dependent = 0;
            thStopHI.Insured = 0;
            thStopHI.Insured_Amount = 0;
            thStopHI.Person = 0;
            return thStopHI;
        }
        #endregion
        #region isBoss為Y時,表示其為業主,無政府補助亦無公司補助
        string strSql = "SELECT IsNull([isBoss],'N') as isBoss FROM [Personnel_Master] Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
        DT = _MyDBM.ExecuteDataTable(strSql);

        if (DT != null && DT.Rows.Count == 1 && DT.Rows[0][0].ToString().Equals("Y"))
        {
            blBoss = true;
        }
        #endregion

        //if (PayrollLt.EmployeeId.Equals("A1008"))
        //{
        //}
        //找固定薪資
        Payroll mPayroll = new Payroll();
        int mFixedSalary = 0;
        try
        {
            mFixedSalary = int.Parse(mPayroll.GetPersonalSalaryItem(PayrollLt, "18"));
        }
        catch { }
        //2011/05/17 kaya 依泛太要求,使用薪資參數設定
        //2011/07/05 kaya 依財務Ivy確認,健保費以上月薪資做基準,無則依當時所設薪資項目做計算
        Payroll.PayrolList PayrollLtLast = PayrollLt;
        PayrollLtLast.SalaryYM = (PayrollLtLast.SalaryYM % 100 == 1) ? (PayrollLtLast.SalaryYM - 89) : (PayrollLtLast.SalaryYM - 1);
        try 
        {//找出投保用級距之基數
            string tempPS = mPayroll.GetPersonalSalaryItem(PayrollLtLast, "18", true);
            int temp18 = Convert.ToInt32(Convert.ToDecimal(tempPS));
            if (temp18 > 0)
                mFixedSalary = temp18;
        }
        catch {
        }
        //2012/05/09 kaya 依財務要求,若健保費依投保金額生效晚於調薪日時,以主檔設定之投保金額計算健保費級距
        //2012/07/26 kaya 修正健保設定後第2個月因不在計薪區間,而未使用[主檔設定之投保金額計算健保費級距]之問題
        //2012/11/23 kaya 修正健保於當月調整下月生效之健保費級距於當月即生效之問題
        try
        {//找出健保主檔設定之投保金額
            string sql = "select "+
                //主檔設定之生效日期不大於計薪當月月底時,方可取用[主檔設定之投保金額計算健保費級距]
                " Case when convert(varchar,h.EffectiveDate,112)<='" + PayrollLt.SalaryYM.ToString() + "31' then " + PayrollLt.DeCodeKey + "([Insured_amount]) else '0' end as Iamt," +
                //主檔設定之生效日期不大於計薪當月月底時,調薪生效日期需大於生效日,方取用調薪金額;若主檔設定之生效日期超過計薪當月月底時,直接取用調薪金額
                " Case when (convert(varchar,h.EffectiveDate,112)<='" + PayrollLt.SalaryYM.ToString() + "31' and convert(varchar,a.EffectiveDate,112) > convert(varchar,h.EffectiveDate,112)) " +
                "  OR (convert(varchar,h.EffectiveDate,112)>'" + PayrollLt.SalaryYM.ToString() + "31') " +
                "  then " + PayrollLt.DeCodeKey + "(IsNull(NewSalary,'0')) else '0' end as Samt," +
                " convert(varchar,a.EffectiveDate,112) iEdate,convert(varchar,h.EffectiveDate,112) hEdate,* from HealthInsurance_Heading h " +
                " left join (select Top 1 * from AdjustSalary_Master where Company='" + mCompany + "' and EmployeeId='" + mEmployeeId + "' " +
                //2013/12/16 加入調薪項目限制為底薪,以免老是抓到其它項目的金額以致算錯
                " and AdjustSalaryItem='01' " +
                " and convert(varchar,EffectiveDate,112) < '" + PayrollLt.SalaryYM.ToString() + "31' order by EffectiveDate desc) a on a.Company=h.Company and a.EmployeeId=h.EmployeeId " +
                //" and convert(varchar,a.EffectiveDate,112) > convert(varchar,h.EffectiveDate,112) " +
                " Where h.Company='" + mCompany + "' and h.EmployeeId='" + mEmployeeId + "' and TrxType='U' " +
                //" and convert(varchar,h.EffectiveDate,112) Between '" + PayrollLtLast.SalaryYM.ToString() + "01' and '" + PayrollLt.SalaryYM.ToString() + "31' " +                
                " ";


            DT = _MyDBM.ExecuteDataTable(sql);
            if (DT != null && DT.Rows.Count > 0)
            {
                int tempInsured_amount = Convert.ToInt32(Convert.ToDecimal(DT.Rows[0]["Iamt"].ToString()));
                int tempAdjustSalary = Convert.ToInt32(Convert.ToDecimal(DT.Rows[0]["Samt"].ToString()));
                if (tempInsured_amount > 0 && tempAdjustSalary == 0)
                {
                    mFixedSalary = tempInsured_amount;
                }
            }
        }
        catch (Exception ex)
        {
            //
        }

        //補助
        DependentSubsidyEnum Employee = GetEmployeeSubsidy(mCompany, mEmployeeId);
        DependentsEnum[] Dependents = GetDependentsSubsidy(mCompany, mEmployeeId);
        #region 2015/04/22 Jennifer 判斷固定薪資是否有變動，若有，則當月勞健保仍是抓取上個月的金額 
        //2015/10/19 Jennifer增加新進職員破月條件判斷
        string newsalaryYM = "";
        if (PayrollLt.SalaryYM.ToString() != "0")
        {
            if (PayrollLt.SalaryYM.ToString().Substring(4) == "01")
                newsalaryYM = Convert.ToInt32(PayrollLt.SalaryYM - 89).ToString();
            else
                newsalaryYM = Convert.ToInt32(PayrollLt.SalaryYM - 1).ToString();
        }
        else
        {
            newsalaryYM = Convert.ToInt32(PayrollLt.SalaryYM).ToString();
        }

        //離職日相關資料
        String strLastYM = "Select DATEPART( year, HireDate ) LastYear,RIGHT(REPLICATE('0', 2) + CAST(DATEPART( month, HireDate ) as NVARCHAR), 2) LastMonth from [Personnel_Master] " +
                                    " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId+"'";
        //復職日相關資料
        String strReHireYM = "Select DATEPART( year, ReHireDate ) LastYear,RIGHT(REPLICATE('0', 2) + CAST(DATEPART( month, ReHireDate ) as NVARCHAR), 2) LastMonth from [Personnel_Master] " +
                                    " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId+"'";
        //前一個月相關資料
        String strCHSQL = "Select * from [" + PayrollTable + "_Heading] " +
         " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + newsalaryYM;
        //前一個月的伙食津貼相關資料
        String strCHSQL2 = "Select * from [" + PayrollTable + "_Detail] " +
            " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + newsalaryYM + " and [SalaryItem]='02'";
        //前一個月請假次數相關資料
        String strCHSQL3 = "select payroll_processingmonth from Leave_Trx where payroll_processingmonth='" + newsalaryYM + "' and employeeid='" + PayrollLt.EmployeeId + "'";
        //前一個月的職務加給相關資料
        String strCHSQLDetail = "Select * from [" + PayrollTable + "_Detail] " +
          " where [Company]='" + PayrollLt.Company + "' and [EmployeeId]='" + PayrollLt.EmployeeId + "' and [SalaryYM]=" + newsalaryYM + " and [SalaryItem]='20'";

         DataTable Check = _MyDBM.ExecuteDataTable(strCHSQL);
         DataTable CHSQL2 = _MyDBM.ExecuteDataTable(strCHSQL2);
         DataTable Check3 = _MyDBM.ExecuteDataTable(strCHSQL3);
         DataTable CheckDetail = _MyDBM.ExecuteDataTable(strCHSQLDetail);
         DataTable CheckLastYM = _MyDBM.ExecuteDataTable(strLastYM);
         DataTable checkReHireYM=_MyDBM.ExecStoredProcedure(strReHireYM);
        
 
         if (Check.Rows.Count > 0)
         {
             //前一個月的伙食津貼 & 底薪不為空
             if (Check.Rows[0]["NT_P"].ToString() != "" && Check.Rows[0]["BaseSalary"].ToString() != "")
             {
                 //前一個月的底薪＝這個月的底薪 & 前一個月的伙食津貼＝這個月的伙食津貼 & 前一個月的職務加給＝這個月的職務加給
                 if (Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["BaseSalary"].ToString())) == PayrollLt.BaseSalary && Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["NT_P"].ToString())) == PayrollLt.NT_P && Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["WT_P_Salary"].ToString() == "" ? "1(" : Check.Rows[0]["WT_P_Salary"].ToString())) == PayrollLt.WT_P_Salary)
                 {
                     return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss); //重新計算
                 }
                 else if (newsalaryYM == CheckLastYM.Rows[0]["LastYear"].ToString() + CheckLastYM.Rows[0]["LastMonth"].ToString()) //前一個月＝離職日
                 {
                     return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss); //重新計算
                 }
                 else if (Check3.Rows.Count > 0) //前一個月請假次數大於0
                 {
                     return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss); //重新計算
                 }
                 else
                 {
                     mFixedSalary = Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["BaseSalary"].ToString())) + Convert.ToInt32(Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(CHSQL2.Rows[0]["SalaryAmount"].ToString()))) + Convert.ToInt32(Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(CheckDetail.Rows[0]["SalaryAmount"].ToString())));
                     return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss); //重新計算
                 }
             }
             else
             {
                 return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss); //重新計算
             }

             #region
             ////前一個月的伙食津貼、底薪
             //if (Check.Rows[0]["NT_P"].ToString() != "" && Check.Rows[0]["BaseSalary"].ToString() != "")
             //{
             //    //前一個月的伙食津貼＝這個月的伙食津貼 &　前一個月的底薪＝這個月的底薪
             //    if (Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["NT_P"].ToString())) == PayrollLt.NT_P && Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["BaseSalary"].ToString())) == PayrollLt.BaseSalary)
             //    {
             //        return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
             //    }
             //    else
             //    {
             //        //前一個月＝離職日
             //        if (newsalaryYM == CheckLastYM.Rows[0]["LastYear"].ToString() + CheckLastYM.Rows[0]["LastMonth"].ToString())
             //        {
             //            return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
             //        }
             //        else 
             //        {
             //            //上個月請假次數，小於等於0 & 前一個月的伙食津貼=這個月的伙食津貼
             //            if (Check3.Rows.Count <= 0 && Convert.ToInt32(Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(CHSQL2.Rows[0]["SalaryAmount"].ToString()))) == PayrollLt.NT_P)
             //            {
             //                //20160118上個月有請假，但這個月沒有請假，不應進入此算式
             //               mFixedSalary = Convert.ToInt32(_UserInfo.SysSet.rtnTransAmount(Check.Rows[0]["BaseSalary"].ToString())) + Convert.ToInt32(Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(CHSQL2.Rows[0]["SalaryAmount"].ToString()))) + Convert.ToInt32(Convert.ToDecimal(_UserInfo.SysSet.rtnTransAmount(CheckDetail.Rows[0]["SalaryAmount"].ToString())));
             //               return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
             //            }
             //            else
             //            {
             //               return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
             //            }

             //        }
             //    }
             //}
             //else
             //{
             //    return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
             //}
             #endregion
         }
         else
         {
            return GetPremiumCo(mFixedSalary, Employee, Dependents, blBoss);
         }
        #endregion
    }

    /// <summary>
    /// 取得員工補助碼
    /// </summary>
    /// <param name="mCompany"></param>
    /// <param name="mEmployeeId"></param>
    /// <returns></returns>
    private static DependentSubsidyEnum GetEmployeeSubsidy(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("select * from HealthInsurance_Heading Where Company='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");



        if (DT.Rows.Count == 0)
            return new DependentSubsidyEnum();
        else
        {
            DependentSubsidyEnum[] mSubsidy = new DependentSubsidyEnum[DT.Rows.Count];
            return ConvertDependent(DT.Rows[0]["Subsidy_code"].ToString());
        }
    }

    /// <summary>
    /// 取得眷屬補助碼
    /// </summary>
    /// <param name="mCompany"></param>
    /// <param name="mEmployeeId"></param>
    /// <returns></returns>
    private static DependentsEnum[] GetDependentsSubsidy(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string strSQL = "select Top 3 * from HealthInsurance_Detail Where Company='" + mCompany + "' and EmployeeId='" + mEmployeeId +
            "' and IsNull(Suspends,'') <> 'Y' order by Subsidy_code desc";

        DataTable DT = _MyDBM.ExecuteDataTable(strSQL);
        DependentsEnum[] mSubsidy = new DependentsEnum[DT.Rows.Count];

        for (int i = 0; i < DT.Rows.Count; i++)
        {//修正眷屬補助額
            mSubsidy[i].DependentSubsidy = ConvertDependent(DT.Rows[i]["Subsidy_code"].ToString());
            mSubsidy[i].IDNo = DT.Rows[i]["IDNo"].ToString();
        }

        return mSubsidy;
    }


    /// <summary>
    /// 轉換為補助類型
    /// </summary>
    /// <param name="mDep"></param>
    /// <returns></returns>
    private static DependentSubsidyEnum ConvertDependent(string mDep)
    {
        DependentSubsidyEnum theDependentSubsidyEnum;
        switch (mDep)
        {
            case "1":
                theDependentSubsidyEnum = DependentSubsidyEnum.All;
                break;

            case "2":
                theDependentSubsidyEnum = DependentSubsidyEnum.Half;
                break;
            case "3":
                theDependentSubsidyEnum = DependentSubsidyEnum.OneFourth;
                break;
            case "4":
                theDependentSubsidyEnum = DependentSubsidyEnum.Nothing;
                break;
            case "5":
                theDependentSubsidyEnum = DependentSubsidyEnum.All;
                break;
            default:    //當無法轉換時，不給予補助
                theDependentSubsidyEnum= DependentSubsidyEnum.Nothing;
                break;
        }
        return theDependentSubsidyEnum;
    }




}
