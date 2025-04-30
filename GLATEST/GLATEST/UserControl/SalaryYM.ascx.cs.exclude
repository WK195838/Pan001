using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UserControl_SalaryYM : System.Web.UI.UserControl
{
    SysSetting SysSet = new SysSetting();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //defaultlist();
        }
    }

    /// <summary>
    /// 預設年份下拉單為WEB設定之年份
    /// </summary>
    public void defaultlist()
    {
        SetOtherYMList(SysSet.YearB, SysSet.YearE, "");
        YearList1.SelectADYear = DateTime.Now.Year.ToString();
        MonthList1.SelectedMonth = DateTime.Now.Month;
    }
    
    /// <summary>
    /// 預設指定公司之計薪年月
    /// </summary>
    /// <param name="Company"></param>
    public void SetSalaryYM(string Company)
    {
        int iYear = 0;
        
        Payroll.SalaryYearMonth sYM = new Payroll.SalaryYearMonth();
        sYM = Payroll.GetSalaryYM(Company);

        if (sYM.SalaryYM > 0)
        {
            iYear = (int)(Math.Truncate(((decimal)sYM.SalaryYM) / 100));

            SetOtherYMList(SysSet.YearB, SysSet.YearE, "");
            YearList1.SelectADYear = iYear.ToString();
            MonthList1.SelectedMonth = (int)(sYM.SalaryYM - iYear * 100);
        }
    }

    /// <summary>
    /// 依設定檔預設年份下拉單(無"空值"選項)
    /// </summary>
    public void SetOtherYMList()
    {
        SetOtherYMList(SysSet.YearB, SysSet.YearE, "");
    }

    /// <summary>
    /// 依"起迄年份"設定年份下拉單(無"空值"選項,可指定預選年份)
    /// </summary>
    /// <param name="YearB">起始年份</param>
    /// <param name="YearE">迄止年份</param>
    /// <param name="DefYear">預選年份</param>
    public void SetOtherYMList(int YearB, int YearE, string DefYear)
    {
        int iYear = DateTime.Today.Year;

        if (YearList1.SysSet.GetCalendarSetting().Equals("Y"))
        {
            labCalKind.Text = "民國";
        }
        else
        {
            labCalKind.Text = "西元";
        }

        if ((YearB > iYear) || (YearB > 1911))
        {
            YearList1.SetYearList(YearB - 1911, YearE - 1911, "");
        }
        else
        {
            YearList1.SetYearList(YearB, YearE, "");
        }

        
    }

    /// <summary>
    /// 依"起迄年份"設定年份下拉單(無"空值"選項,可指定預選年份)
    /// </summary>
    /// <param name="YearB">起始年份</param>
    /// <param name="YearE">迄止年份</param>
    /// <param name="DefYear">預選年份</param>
    public void SetInvYMList ( int YearB , int YearE , string DefYear )
    {
        int iYear = DateTime.Today.Year;
        MonthList1.SetInvMonth ( );
        if ( YearList1.SysSet.GetCalendarSetting ( ).Equals ( "Y" ) )
        {
            labCalKind.Text = "民國";
        }
        else
        {
            labCalKind.Text = "西元";
        }

        if ( ( YearB > iYear ) || ( YearB > 1911 ) )
        {
            YearList1.SetYearList ( YearB - 1911 , YearE - 1911 , "" );
        }
        else
        {
            YearList1.SetYearList ( YearB , YearE , "" );
        }

       
    }


    
    /// <summary>
    /// 依設定檔預設年份下拉單(有"空值"選項)
    /// </summary>
    /// <param name="nonChose">設定"空值"選項顯示名稱</param>
    public void SetSpecialYMList(string nonChose)
    {
        SysSetting sysSet = new SysSetting();
        SetSpecialYMList(sysSet.YearB, sysSet.YearE, "", nonChose);
    }

    /// <summary>
    /// 依設定檔預設年份下拉單(有"空值"選項,並可指定預選年份)
    /// </summary>
    /// <param name="DefYear">預選年份</param>
    /// <param name="nonChose">設定"空值"選項顯示名稱</param>
    public void SetSpecialYMList(string DefYear,string nonChose)
    {
        SysSetting sysSet = new SysSetting();
        SetSpecialYMList(sysSet.YearB, sysSet.YearE, DefYear, nonChose);
    }
    
    /// <summary>
    /// 依"起迄年份"設定年份下拉單(有"空值"選項,並可指定預選年份)
    /// </summary>
    /// <param name="YearB">起始年份</param>
    /// <param name="YearE">迄止年份</param>
    /// <param name="DefYear">預選年份</param>
    /// <param name="nonChose">設定"空值"選項顯示名稱</param>
    public void SetSpecialYMList(int YearB, int YearE, string DefYear, string nonChose)
    {
        SysSetting sysSet = new SysSetting();
        int iYear = DateTime.Today.Year;

        if (YearList1.SysSet.GetCalendarSetting().Equals("Y"))
        {
            labCalKind.Text = "民國";
        }
        else
        {
            labCalKind.Text = "西元";
        }

        if ((YearB > iYear) || (YearB > 1911))
        {
            YearList1.SetYearList(YearB - 1911, YearE - 1911, DefYear, nonChose);            
        }
        else
        {
            YearList1.SetYearList(YearB, YearE, DefYear, nonChose);
        }

        if (!string.IsNullOrEmpty(nonChose))
            MonthList1.SetSpecialList("", nonChose);
        else
            MonthList1.SetSpecialList(DateTime.Today.Month.ToString(), "");
    }
    
    /// <summary>
    /// 取得年月值
    /// </summary>
    public string SelectSalaryYM
    {
        get
        {
            int sYM = 0;

            try
            {
                sYM = (int)Convert.ToDecimal(YearList1.SelectADYear);
            }
            catch
            {
                //
            }

            try
            {
                sYM = sYM * 100 + (int)Convert.ToDecimal(MonthList1.SelectedMonth);
            }
            catch
            {
                //
            }

            return (sYM > 0) ? sYM.ToString() : "";
        }
        set
        {
            int sYM;
            int iYear;

            try
            {
                sYM = (int)Convert.ToDecimal(value);
                iYear = (int)(Math.Truncate((Decimal)(sYM / 100)));
                YearList1.SelectADYear = iYear.ToString();
                MonthList1.SelectedMonth = (int)(sYM - iYear * 100);
            }
            catch 
            { 
                //
            }
        }
    }

    /// <summary>
    /// 取得年月名稱(XX年XX月/XXXX年XX月)
    /// </summary>
    public string SelectSalaryYMName
    {
        get
        {
            return YearList1.SelectYear + "年" + MonthList1.SelectMonth + "月";
        }
        set
        {
            int sYM = (int)Convert.ToDecimal(value);
            int iYear = (int)(Math.Truncate((Decimal)(sYM / 100)));

            YearList1.SelectADYear = iYear.ToString();
            MonthList1.SelectedMonth = (int)(sYM - iYear * 100);
        }
    }

    /// <summary>
    /// 取得西元或民國
    /// </summary>
    public string showCalenderName
    {
        get
        {
            return labCalKind.Text;
        }
        set
        {
            labCalKind.Text = value;
        }
    }

    /// <summary>
    /// 設定是否顯示西元或民國等文字
    /// </summary>
    public bool LabVisible
    {
        get {
            return labCalKind.Visible;
        }
        set {            
            labCalKind.Visible = value;
            labY.Visible = value;
            labM.Visible = value;
        }
    }






    /// <summary>
    /// 設定是否可變更下拉單
    /// </summary>
    public bool Enabled
    {
        get
        {
            return this.YearList1.Enabled;
        }
        set
        {
            this.YearList1.Enabled = value;
            this.MonthList1.Enabled = value;
        }
    }
}
