﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// 勞工保險
/// 2011/09/30 kaya 將勞保費破月計算改拆至勞保費函式中
/// 2011/11/09 kaya 為了符合勞保局計算結果(尾數+-1的誤差),修改勞保計算方式:各種費率先分開計算再加總
/// </summary>
public class LaborInsurance
{
    /// <summary>
    /// 保費計算(依 個人薪資項目 所設),整月勞保金額
    /// </summary>
    /// <param name="PayrollLt">個人基本參數</param>
    /// <returns></returns>
    public static Insurance GetPremium(Payroll.PayrolList PayrollLt)
    {
        return GetPremium(PayrollLt, 30);
    }

    /// <summary>
    /// 保費計算(依 個人薪資項目 所設),依計薪天數計算
    /// </summary>
    /// <param name="PayrollLt">個人基本參數</param>
    /// <param name="iPaydays">1~30天</param>
    /// <returns></returns>
    public static Insurance GetPremium(Payroll.PayrolList PayrollLt,int iPaydays)
    {
        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        string mTime = PayrollLt.SalaryYM.ToString();

        Insurance mInsurance = new Insurance();
        LaborInsuranceRateParameter mParameter = GetLaborInsuranceRateParameter();

        mInsurance.Company = 0;
        mInsurance.Insured = 0;
        mInsurance.Insured_Amount = 0;

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        //20110804 kaya 增加勞保補助計算
        float flLISubsidy = 0;
        #region isBoss為Y時,表示其為不需扣勞保之業主;反之則抓出補助比例;退保者亦不需扣
        string strSql = "SELECT count(*) FROM [dbo].[LaborInsurance] where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId +
            "' and [TrxType] = 'A3' and Convert(varchar,[EffectiveDate],112)<" + PayrollLt.SalaryYM.ToString() + "00 ";
        strSql = "SELECT IsNull([isBoss],'N') as isBoss,[LISubsidy] " +
             " ,(" + strSql + ") As [NoLI]" +
             " FROM [Personnel_Master] " +
             " Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
        DataTable DT = _MyDBM.ExecuteDataTable(strSql);
        if (DT != null && DT.Rows.Count == 1)
        {
            if (DT.Rows[0]["isBoss"].ToString().Equals("Y") || DT.Rows[0]["NoLI"].ToString() != "0")
            {
                return mInsurance;
            }
            else
            {
                try
                {
                    flLISubsidy = Convert.ToInt32(DT.Rows[0]["LISubsidy"].ToString());
                }
                catch (Exception ex)
                {
                    SysSetting SysSet = new SysSetting();
                    SysSet.WriteToLogs("PYMsg", "勞保補助比例未設定！使用預設值０％\n\r" + "Comp:" + PayrollLt.Company + "|Emp:" + PayrollLt.EmployeeId + "\n\r");
                    flLISubsidy = 0;
                }
            }
        }
        #endregion
        //計算勞保補助比例
        flLISubsidy = (flLISubsidy >= 100) ? 0 : ((100 - flLISubsidy) / 100);

        //找固定薪資
        Payroll mPayroll = new Payroll();
        int mFixedSalary = 0;
        try
        {
            mFixedSalary = int.Parse(mPayroll.GetPersonalSalaryItem(PayrollLt, "19"));
        }
        catch { }
        //2011/05/17 kaya 依泛太要求,使用薪資參數設定
        try
        {//找出投保用級距之基數
            //2011/07/21 kaya 依財務Ivy確認,勞保費同健保費以上月薪資做基準,無則依當時所設薪資項目做計算
            Payroll.PayrolList PayrollLtLast = PayrollLt;
            PayrollLtLast.SalaryYM = (PayrollLtLast.SalaryYM % 100 == 1) ? (PayrollLtLast.SalaryYM - 89) : (PayrollLtLast.SalaryYM - 1);

            string tempPS = mPayroll.GetPersonalSalaryItem(PayrollLtLast, "19", true);
            int temp19 = Convert.ToInt32(Convert.ToDecimal(tempPS));
            if (temp19 > 0)
                mFixedSalary = temp19;
        }
        catch (Exception ex)
        {
        }
        //找投保金額
        mInsurance.Insured_Amount = GetM_Contribution_Wages(mFixedSalary);

        #region For DeBUG
        if (PayrollLt.EmployeeId == "A1407")
        { 
        }
        //switch (mInsurance.Insured_Amount)
        //{
        //    case 43900:
        //        break;
        //    case 40100:
        //        break;
        //    case 27600:
        //        break;
        //    case 26400:
        //        break;
        //}
        #endregion

        //2011/11/09 kaya 為了符合勞保局計算結果(尾數+-1的誤差),修改勞保計算方式:各種費率先分開計算再加總
        //職業災害保險
        //2011/11/09-- float LISum = mInsurance.Insured_Amount * (mParameter.LI_Premium_Rate + mParameter.EmploymentInsRate);  //投保金額x (勞工保險+就業保險)
        //2011/11/09-- mInsurance.Company = Computer.Round(LISum * mParameter.CompanyBurdenRate + mInsurance.Insured_Amount * mParameter.FatalityRate * iPaydays / thisYMPayDate);
        //20110804 kaya 增加勞保補助計算
        //2011/11/09-- mInsurance.Insured = Computer.Round(LISum * mParameter.EmpBurdenRate * flLISubsidy);

        //2011/11/09++ 投保金額x勞工保險
        float LIPR = mInsurance.Insured_Amount * mParameter.LI_Premium_Rate;
        //2011/11/09++ 投保金額x就業保險
        float LIER = mInsurance.Insured_Amount * mParameter.EmploymentInsRate;
        //2011/09/30++ 將勞保費破月計算改拆至勞保費函式中
        int thisYMPayDate = 30;//基礎計薪天數
        if (iPaydays > thisYMPayDate) iPaydays = thisYMPayDate;
        //2011/11/09-- LISum = LISum * iPaydays / thisYMPayDate;
        //2011/11/09++ Start==========================
        ///計算破月後基本金額-------------
        LIPR = LIPR * iPaydays / thisYMPayDate;
        LIER = LIER * iPaydays / thisYMPayDate;
        ///-------------------------------
        float f1, f2, f3, f4, f5;
        ///「個人」應負擔保險費-------------
        //勞工保險普通事故保險費
        f1 = LIPR * mParameter.EmpBurdenRate;
        //就業保險費
        f2 = LIER * mParameter.EmpBurdenRate;
        ///-------------------------------
        ///「單位」應負擔保險費-------------        
        //勞工保險普通事故保險費
        f3 = LIPR * mParameter.CompanyBurdenRate;
        //就業保險費
        f4 = LIER * mParameter.CompanyBurdenRate;
        //勞工保險職業災害保險費
        f5 = mInsurance.Insured_Amount * mParameter.FatalityRate * iPaydays / thisYMPayDate;
        ///--------------------------------        
        mInsurance.Insured = Computer.Round(Computer.Round(f1) * flLISubsidy + Computer.Round(f2) * flLISubsidy);
        mInsurance.Company = Computer.Round(f3) + Computer.Round(f4) + Computer.Round(f5);
        //2011/11/09++ End==========================

        #region For DeBUG
        //int test1, test2, test3, test4, test5, sumE, sumC;
        //try
        //{
        //    test1 = Computer.Round(f1);
        //    test2 = Computer.Round(f2);
        //    sumE = test1 + test2;
        //    test3 = Computer.Round(f3);
        //    test4 = Computer.Round(f4);
        //    test5 = Computer.Round(f5);
        //    sumC = test3 + test4 + test5;
        //}
        //catch { }
        #endregion

        return mInsurance;
    }

    /// <summary>
    /// 保費計算(依 LaborInsurance 所設)
    /// </summary>
    /// <param name="mSalary"></param>
    /// <returns></returns>
    public static Insurance GetLIPremium(Payroll.PayrolList PayrollLt)
    {
        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        string mTime = PayrollLt.SalaryYM.ToString();

        Insurance mInsurance = new Insurance();
        LaborInsuranceRateParameter mParameter = GetLaborInsuranceRateParameter();

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string strSql = "Select " + PayrollLt.DeCodeKey + "([LI_amount]) From [LaborInsurance] Where [Company] = '" + PayrollLt.Company + "' And [EmployeeId] ='" + PayrollLt.EmployeeId + "'";
        DataTable DT = _MyDBM.ExecuteDataTable(strSql);

        int mFixedSalary = 0;
        if (DT != null && DT.Rows.Count == 1)
        {
            mFixedSalary = Convert.ToInt32(Convert.ToDecimal(DT.Rows[0][0].ToString()));
        }
        //找投保金額
        mInsurance.Insured_Amount = mFixedSalary;

        //職業災害保險
        float LISum = mInsurance.Insured_Amount * (mParameter.LI_Premium_Rate + mParameter.EmploymentInsRate);  //投保金額x (勞工保險+就業保險)

        mInsurance.Company = Computer.Round(LISum * mParameter.CompanyBurdenRate + mInsurance.Insured_Amount * mParameter.FatalityRate);
        mInsurance.Insured = Computer.Round(LISum * mParameter.EmpBurdenRate);

        return mInsurance;
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

        DataTable DT = _MyDBM.ExecuteDataTable("SELECT  *  FROM LaborInsurance_SeriesParameter where liamt >= '" + mSalary.ToString() + "' or liamt>=(SELECT  top 1 max( liamt)  FROM LaborInsurance_SeriesParameter )  ORDER BY liamt");
     
        int RangeNo = int.Parse(DT.Rows[0]["RangeNo"].ToString()); //級距
        int LiAmt = int.Parse(DT.Rows[0]["LiAmt"].ToString());     //月投保金額



        return LiAmt;
    }

    /// <summary>
    /// 取得勞工保險參數
    /// </summary>
    /// <param name="mCompany"></param>
    /// <param name="mEmployeeId"></param>
    /// <returns></returns>
    public static LaborInsuranceRateParameter GetLaborInsuranceRateParameter()
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("SELECT * FROM LaborInsurance_RateParameter");
        LaborInsuranceRateParameter LI = new LaborInsuranceRateParameter();


        if (string.IsNullOrEmpty(DT.Rows[0]["VersionNo"].ToString()) == false) LI.VersionNo = int.Parse(DT.Rows[0]["VersionNo"].ToString());                            //版本號
        if (string.IsNullOrEmpty(DT.Rows[0]["LI_Premium_Rate"].ToString()) == false) LI.LI_Premium_Rate = float.Parse(DT.Rows[0]["LI_Premium_Rate"].ToString());        //勞保費率
        if (string.IsNullOrEmpty(DT.Rows[0]["EmpBurdenRate"].ToString()) == false) LI.EmpBurdenRate = float.Parse(DT.Rows[0]["EmpBurdenRate"].ToString());              //員工負擔比率
        if (string.IsNullOrEmpty(DT.Rows[0]["CompensationRate"].ToString() )== false) LI.CompensationRate = float.Parse(DT.Rows[0]["CompensationRate"].ToString());     //墊償比率
        if (string.IsNullOrEmpty(DT.Rows[0]["CompanyBurdenRate"].ToString()) == false) LI.CompanyBurdenRate = float.Parse(DT.Rows[0]["CompanyBurdenRate"].ToString());  //公司負擔比率
        if (string.IsNullOrEmpty(DT.Rows[0]["FatalityRate"].ToString() )== false) LI.FatalityRate = float.Parse(DT.Rows[0]["FatalityRate"].ToString());                 //職災比率
        if (string.IsNullOrEmpty(DT.Rows[0]["EmploymentInsRate"].ToString()) == false) LI.EmploymentInsRate = float.Parse(DT.Rows[0]["EmploymentInsRate"].ToString());  //就業保險費率




        return LI;
    }




    public struct Insurance
    {
        /// <summary>
        /// 投保金額
        /// </summary>
        public int Insured_Amount;

        /// <summary>
        /// 公司負擔
        /// </summary>
        public int Company;

        /// <summary>
        /// 被保險人 (員工)負擔
        /// </summary>
        public int Insured;
    }

    public struct LaborInsuranceRateParameter
    {
        /// <summary>
        /// 版本號
        /// </summary>
        public int VersionNo;

        /// <summary>
        /// 勞保費率
        /// </summary>
        public float LI_Premium_Rate;

        /// <summary>
        /// 員工負擔比率
        /// </summary>
        public float EmpBurdenRate;

        /// <summary>
        /// 墊償比率
        /// </summary>
        public float CompensationRate;

        /// <summary>
        /// 公司負擔比率
        /// </summary>
        public float CompanyBurdenRate;

        /// <summary>
        /// 職災比率
        /// </summary>
        public float FatalityRate;

        /// <summary>
        /// 就業保險費率
        /// </summary>
        public float EmploymentInsRate;
 


    }

}
