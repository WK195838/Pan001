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
/// 所得稅
/// </summary>
public class IncomeTax
{
    /// <summary>
    /// 所得稅計算
    /// </summary>
    /// <param name="mSalary"></param>
    /// <returns></returns>
    public static Tax GetIncomeTax(decimal fSalary, Payroll.PayrolList PayrollLt)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        string mTime = PayrollLt.SalaryYM.ToString();
        int iMonths = 1;
        
        string strSQL = "";
        DataTable DT = null;
        SysSetting SysSet = new SysSetting();
        string strVersionID = SysSet.GetConfigString("VersionID");

        Tax mTax = new Tax();
        decimal dMinTax = 0, dMinBonusAmount = 0;
        
        if ((strVersionID.Contains("PICCustomization")) && (PayrollLt.PeriodCode.Equals("B")))
        {
            mTax.Rate = 0.05F;

            #region 找出獎金稅率的設定
            DT = _MyDBM.ExecuteDataTable("SELECT IsNull(PY_Para6,0) TaxRate,IsNull(PY_Para7,0) mTax,IsNull(PY_Para8,0) mBonusAmount FROM PersonnelSalary_Parameter WHERE Category='01' And COMPANY ='" + mCompany + "'");
            if (DT != null)
            {//先找[人事薪資參數]是否有設定
                if (DT.Rows.Count > 0)
                {
                    //獎金稅率
                    mTax.Rate = float.Parse(DT.Rows[0]["TaxRate"].ToString());
                    //獎金最低代扣稅額
                    dMinTax = decimal.Parse(DT.Rows[0]["mTax"].ToString());
                    //獎金應扣標準
                    dMinBonusAmount = decimal.Parse(DT.Rows[0]["mBonusAmount"].ToString());
                }
            }
            #endregion
            //本月份非薪資所得稅
            mTax.Amount = Convert.ToInt32(fSalary * Convert.ToDecimal(mTax.Rate));
            
            int iMonth = Convert.ToInt32(PayrollLt.SalaryYM.ToString().Substring(4, 2));
            if (iMonth == 2 || iMonth == 5 || iMonth == 8 || iMonth == 11)
            {
                #region 找出前二月的代扣所得稅並扣除
                if (iMonth == 2)
                {
                    strSQL = (PayrollLt.SalaryYM - 1).ToString() + "," + (PayrollLt.SalaryYM - 100).ToString().Remove(4) + "12";
                }
                else if (iMonth == 1)
                {
                    strSQL = (PayrollLt.SalaryYM - 100).ToString().Remove(4) + "12" + "," + (PayrollLt.SalaryYM - 100).ToString().Remove(4) + "11";
                }
                strSQL = "Select IsNull(Sum(" + PayrollLt.DeCodeKey + "(SalaryAmount)),0) FROM [PayrollWorking_Detail] " +
                    " Where [Company]='" + mCompany + "' And [EmployeeId]='" + mEmployeeId + "' " +
                    " And [SalaryItem]='03' And [PeriodCode]='B' And [SalaryYM] in (" + strSQL + ")";
                DT = _MyDBM.ExecuteDataTable(strSQL);
                if (DT != null)
                {//取出前二個月代扣稅額並扣除
                    if (DT.Rows.Count > 0)
                    {
                        mTax.Amount -= (int)Convert.ToDecimal(DT.Rows[0][0].ToString());
                    }
                }
                #endregion                
            }

            #region 依法比對應代扣標準
            if ((dMinBonusAmount > 0) && (dMinBonusAmount >= fSalary))
            {//當[獎金應扣標準]有設定時,若當月核發獎金未達[獎金應扣標準],不代扣稅
                mTax.Amount = 0;
            }
            else if ((dMinBonusAmount == 0) && (dMinTax > 0) && (dMinTax > mTax.Amount))
            {//當[獎金應扣標準]未設定且[獎金最低代扣稅額]有設定時,若當月獎金代扣稅額未達[獎金最低代扣稅額],不代扣稅
                mTax.Amount = 0;
            }
            #endregion
        }
        else
        {
            ///*
            //每月薪資所得×12＝估計全年薪資所得。
            //估計全年薪資所得－（免稅額＋標準扣除額＋薪資所得特別扣除額）＝估計全年應稅薪資所得。
            //估計全年應稅薪資所得×適用九十一年度之該級距稅率－累進差額＝估計應稅薪資所得全年應納稅額。
            //估計應稅薪資所得全年應納稅額÷12＝每月應扣繳稅額。

            IncomeTaxRateParameter mParameter = GetIncomeTaxRate_Parameter();

            //扣除額
            float mAllDeduction = mParameter.PayrollDeduction + mParameter.PersonalNTAmount + mParameter.StandardDeduction;
            int mFamilySupport = GetFamilySupport(mCompany, mEmployeeId);
            float mYear;

            //判斷是否扣除 配偶免稅額
            if (Payroll.GetMaritalStatus(mCompany, mEmployeeId))
                mAllDeduction += mParameter.PartnerNTAmount;


            //扣除撫養親屬
            mAllDeduction += mParameter.FamilySupportConcession * mFamilySupport;

            mYear = ((float)fSalary) * (12 / iMonths) - mAllDeduction;

            IncomeTaxRate_Refer mRefer = GetIncomeTaxRate_Refer(mYear);

            //全年應稅薪資所得××適用九十一年度之該級距稅率
            mYear = mYear * mRefer.Rate / 100;//Michelle加入:似乎原先漏掉了

            mTax.Amount = Computer.Round((mYear - mRefer.ProgressiveDiffAmount) / (12 / iMonths));
            mTax.Rate = mRefer.Rate;

        }
        //找出最低代扣稅額
        int iLastTax = 0;
        try
        {
            iLastTax = int.Parse(DBSetting.GetPSParaValue(PayrollLt.Company, "PY_Para9"));
        }
        catch { }
        if (mTax.Amount < 0) mTax.Amount = 0;
        else if (mTax.Amount < iLastTax) mTax.Amount = 0;
        return mTax;
    }

    /// <summary>
    /// 取得所得稅參數
    /// </summary>
    public static IncomeTaxRateParameter GetIncomeTaxRate_Parameter()
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

        DataTable DT = _MyDBM.ExecuteDataTable("SELECT * FROM IncomeTaxRate_Parameter");
        IncomeTaxRateParameter LI = new IncomeTaxRateParameter();
        if (DT.Rows.Count > 0)
        {
            if (string.IsNullOrEmpty(DT.Rows[0]["VersionNo"].ToString()) == false) LI.VersionNo = int.Parse(DT.Rows[0]["VersionNo"].ToString());                            //版本號
            if (string.IsNullOrEmpty(DT.Rows[0]["PersonalNTAmount"].ToString()) == false) LI.PersonalNTAmount = float.Parse(DT.Rows[0]["PersonalNTAmount"].ToString());        //個人免稅額
            if (string.IsNullOrEmpty(DT.Rows[0]["PartnerNTAmount"].ToString()) == false) LI.PartnerNTAmount = float.Parse(DT.Rows[0]["PartnerNTAmount"].ToString());              //配偶免稅額
            if (string.IsNullOrEmpty(DT.Rows[0]["FamilySupportConcession"].ToString()) == false) LI.FamilySupportConcession = float.Parse(DT.Rows[0]["FamilySupportConcession"].ToString());     //撫養親屬寬減額
            if (string.IsNullOrEmpty(DT.Rows[0]["StandardDeduction"].ToString()) == false) LI.StandardDeduction = float.Parse(DT.Rows[0]["StandardDeduction"].ToString());  //標準扣除額
            if (string.IsNullOrEmpty(DT.Rows[0]["PayrollDeduction"].ToString()) == false) LI.PayrollDeduction = float.Parse(DT.Rows[0]["PayrollDeduction"].ToString());                 //薪資扣除額
            if (string.IsNullOrEmpty(DT.Rows[0]["BonusRate"].ToString()) == false) LI.BonusRate = float.Parse(DT.Rows[0]["BonusRate"].ToString());  //獎金稅率
        }

        return LI;
    }


    /// <summary>
    /// 取得級距表
    /// </summary>
    /// <param name="mSalary">所得</param>
    /// <returns></returns>
    public static IncomeTaxRate_Refer GetIncomeTaxRate_Refer(float mSalary)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();
        string sSalary = ((int)mSalary).ToString();
        DataTable DT = _MyDBM.ExecuteDataTable("select top 1 * from IncomeTaxRate_Refer where IncomeTax_UpperLimit>='" + sSalary + "' and IncomeTax_LowerLimit<='" + sSalary + "' or IncomeTax_LowerLimit>= '" + sSalary + "'");
        IncomeTaxRate_Refer mRefer = new IncomeTaxRate_Refer();

        mRefer.LowerLimit  = int.Parse(DT.Rows[0]["IncomeTax_UpperLimit"].ToString());
        mRefer.UpperLimit = int.Parse(DT.Rows[0]["IncomeTax_LowerLimit"].ToString());
        mRefer.Rate  = float.Parse(DT.Rows[0]["IncomeTaxRate"].ToString());
        mRefer.ProgressiveDiffAmount = int.Parse(DT.Rows[0]["ProgressiveDiffAmount"].ToString());

        return mRefer;
        
    }

    /// <summary>
    /// 傳回撫養親屬人數
    /// </summary>
    /// <returns>傳回撫養親屬人數</returns>
    /// Johnny
    public static int GetFamilySupport(string mCompany, string mEmployeeId)
    {
        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();

 
        DataTable DT = _MyDBM.ExecuteDataTable("SELECT Company ,EmployeeId,DependentsNum FROM Personnel_Master  WHERE COMPANY ='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");

        if (DT.Rows.Count > 0)
            return int.Parse(DT.Rows[0]["DependentsNum"].ToString());
        else
            return 0;
    }

    public struct Tax
    {
        /// <summary>
        /// 月繳金額
        /// </summary>
        public int Amount;

        /// <summary>
        /// 所得稅率
        /// </summary>
        public float Rate;
    };

    public struct IncomeTaxRateParameter
    {
        /// <summary>
        /// 版本號
        /// </summary>
        public int VersionNo;

        /// <summary>
        /// 個人免稅額
        /// </summary>
        public float PersonalNTAmount;

        /// <summary>
        /// 配偶免稅額
        /// </summary>
        public float PartnerNTAmount;

        /// <summary>
        /// 撫養親屬寬減額
        /// </summary>
        public float FamilySupportConcession;

        /// <summary>
        /// 標準扣除額
        /// </summary>
        public float StandardDeduction;

        /// <summary>
        /// 薪資扣除額
        /// </summary>
        public float PayrollDeduction;

        /// <summary>
        /// 獎金稅率
        /// </summary>
        public float BonusRate;
    }

    /// <summary>
    /// 所得稅級距
    /// </summary>
    public struct IncomeTaxRate_Refer
    {
        /// <summary>
        /// 所得稅上限
        /// </summary>
        public int UpperLimit;
        /// <summary>
        /// 所得稅下限
        /// </summary>
        public int LowerLimit;
        /// <summary>
        /// 所得稅率
        /// </summary>
        public float Rate;
        /// <summary>
        /// 累進差額
        /// </summary>
        public int ProgressiveDiffAmount;

    }    
}
