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
/// 員工福利金(特殊計算時)
/// 2013/05/21 即使是破月計算福利金之離職人員,亦不應扣除請假日數,故改使用離職日計算比率
/// 並依是否計薪當月破月到職/破月離職計算在職天數
/// </summary>
public class FringeBenefits
{

    /// <summary>
    /// 員工整月福利金
    /// </summary>
    /// <param name="PayrollLt">員工資料</param>
    /// <returns></returns>
    public static int GetFringeBenefits(Payroll.PayrolList PayrollLt)
    {
        return GetFringeBenefit(PayrollLt, true);
    }

    /// <summary>
    /// 員工破月福利金
    /// </summary>
    /// <param name="PayrollLt">員工資料</param>
    /// <returns></returns>
    public static int GetFringeBenefitUnFullMonth(Payroll.PayrolList PayrollLt)
    {
        return GetFringeBenefit(PayrollLt, false);
        
    }

    /// <summary>
    /// 員工福利金
    /// </summary>
    /// <param name="PayrollLt">員工資料</param>
    /// <param name="fullMounth">是否計算整月</param>
    /// <returns></returns>
    public static int GetFringeBenefit(Payroll.PayrolList PayrollLt, bool fullMounth)
    {
       
        string mCompany = PayrollLt.Company;
        string mEmployeeId = PayrollLt.EmployeeId;
        string mTime = PayrollLt.SalaryYM.ToString();

        DBManger _MyDBM;
        _MyDBM = new DBManger();
        _MyDBM.New();


        DataTable DT = _MyDBM.ExecuteDataTable("Select Company ,EmployeeId , IsNull(LWC,'') LWC from Personnel_Master Where Company='" + mCompany + "' and EmployeeId='" + mEmployeeId + "'");
        string mLWC;

        if (DT.Rows.Count > 0)
        {
            mLWC = DT.Rows[0]["LWC"].ToString();

            //未參加福委會
            if (mLWC != "Y") return 0;

            //找員工福利比
            float FBAmount = 0, FBRate = 0;

            //DT = _MyDBM.ExecuteDataTable("SELECT Company ,FringeBenefits FROM Company WHERE COMPANY ='" + mCompany + "'");
            //float FringeBenefits = float.Parse(DT.Rows[0]["FringeBenefits"].ToString());
            Payroll mPayroll = new Payroll();

            #region 找出福利金的設定(michelle修改)
            //PersonnelSalary_Parameter
            //PY_Para2	福利金固定金額
            //PY_Para3	福利金比率
            DT = _MyDBM.ExecuteDataTable("SELECT Company,IsNull(PY_Para2,0) FBAmount,IsNull(PY_Para3,0) FBRate FROM PersonnelSalary_Parameter WHERE Category='01' And COMPANY ='" + mCompany + "'");
            if (DT != null)
            {//先找[人事薪資參數]是否有設定
                if (DT.Rows.Count > 0)
                {
                    FBAmount = float.Parse(DT.Rows[0]["FBAmount"].ToString());
                    FBRate = float.Parse(DT.Rows[0]["FBRate"].ToString());
                }
            }

            if ((FBAmount == 0) && (FBRate == 0))
            {//如果[人事薪資參數]未設定,則找[薪資項目參數]中的設定
                DT = _MyDBM.ExecuteDataTable("SELECT IsNull(FixedAmount,0) FBAmount,IsNull(SalaryRate,0) FBRate FROM SalaryStructure_Parameter WHERE SalaryId='07' ");
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        string strTheFB = mPayroll.GetPersonalSalaryItem(PayrollLt, "07");
                        try
                        {
                            FBAmount = -1 * float.Parse(strTheFB.ToString());
                            if (FBAmount == 0)
                                return 0;
                            FBRate = 0;
                        }
                        catch {
                            FBAmount = float.Parse(DT.Rows[0]["FBAmount"].ToString());
                            FBRate = float.Parse(DT.Rows[0]["FBRate"].ToString());
                        }
                    }
                }
            }
            #endregion

            //找固定薪資
            int mFixedSalary = mPayroll.GetFixedSalary(PayrollLt);
            float daysRate = 1;
            float ResignDayRate=1;
            if (fullMounth == false)
            {//2013/05/21 即使是破月計算福利金之離職人員,亦不應扣除請假日數,故改使用離職日計算比率
                DateTime theHD = Payroll.GetPayrollDate(PayrollLt.Company, PayrollLt.EmployeeId, "HireDate", PayrollLt.SalaryYM.ToString());
                //2013/12/12 依需求於計薪時加入判斷復職者
                DateTime theRHD = Payroll.GetPayrollDate(PayrollLt.Company, PayrollLt.EmployeeId, "ReHireDate", PayrollLt.SalaryYM.ToString());
                DateTime theRD = Payroll.GetPayrollDate(PayrollLt.Company, PayrollLt.EmployeeId, "ResignDate", PayrollLt.SalaryYM.ToString());
                float theStartDay = 1;
                float theEndDay = 30;
                if (theRHD > theHD) theHD = theRHD;
                //2013/12/13 修正破月計算時,月份溢出的問題
                float theMonthDay = Convert.ToDateTime((theHD.Year + ((theHD.Month == 12) ? 1 : 0)).ToString() + "/" + ((theHD.Month == 12) ? 1 : theHD.Month + 1).ToString() + "/1").AddDays(-1).Day;
                if (theHD.Year * 100 + theHD.Month == PayrollLt.SalaryYM)
                {
                    theEndDay = theMonthDay;
                    theStartDay = (float)theHD.Day;
                }
                if (theRD.Year * 100 + theRD.Month == PayrollLt.SalaryYM)
                    theEndDay = (float)theRD.Day;

                if ((theEndDay - theStartDay + 1) >= theMonthDay)
                {
                    daysRate = 1;//整月在職
                    //取得離職當月在職率，未離職為 1
                    ResignDayRate = Payroll.GetResignDayRate(PayrollLt, mTime);
                }
                else if (theHD == theRD || theStartDay > 30)
                {
                    daysRate = (float)1 / 30;
                    //取得離職當月在職率，未離職為 1
                    ResignDayRate = Payroll.GetResignDayRate(PayrollLt, mTime);
                }
                else
                    daysRate = (theEndDay - theStartDay + 1) / 30;
                //daysRate = ((float)PayrollLt.Paydays) / 30;
            }

           
            if (FBAmount > 0)
                return Computer.Round(FBAmount * ResignDayRate * daysRate);
            else
                return Computer.Round(mFixedSalary * FBRate * ResignDayRate * daysRate);
        }

        return 0;

        
    }

 


}
