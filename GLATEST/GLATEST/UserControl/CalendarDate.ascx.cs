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
using System.Drawing;

public partial class UserControl_CalendarDate : System.Web.UI.UserControl
{
    SysSetting Setting = new SysSetting();
    public string weekShow = "TW";
    public string theCalendarDay = "";
    public bool showDefCols = true;
    /// <summary>
    /// 系統日期的預設顏色
    /// </summary>
    Color defTodayColor = Color.Navy;
    /// <summary>
    /// 假日的預設顏色
    /// </summary>
    Color defHoliDayColor = Color.Red;
    /// <summary>
    /// 一般日期的預設顏色
    /// </summary>
    Color defDayColor = Color.Black;
    /// <summary>
    /// 系統日期的顏色
    /// </summary>
    public Color TodayColor;
    /// <summary>
    /// 假日的顏色
    /// </summary>
    public Color HoliDayColor;
    /// <summary>
    /// 一般日期的顏色
    /// </summary>
    public Color DayColor;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 設定指定日期行事曆
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="theDate"></param>
    public void SetDate(string Company, DateTime theDate)
    {
        SetDate(weekShow, Company, theDate.ToString("yyyy/MM/dd"), "", "", "", "");
    }

    /// <summary>
    /// 設定指定日期行事曆
    /// </summary>
    /// <param name="Company"></param>
    /// <param name="theDate"></param>
    public void SetDate(string Company, string theDate)
    {
        SetDate(weekShow, Company, theDate, "", "", "", "");
    }

    /// <summary>
    /// 設定指定日期行事曆及標題文字
    /// </summary>
    /// <param name="Show">星期的顯示方式()</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>
    public void SetDate(string Show, string Company, string theDate, string DepId, string EmployeeId, string Category, string Status)
    {
        string strWeek = "";
        theCalendarDay = theDate;

        thisShow.Value = Show;
        thisCompany.Value = Company;
        thistheDate.Value = theDate;
        thisDepId.Value = DepId;
        thisEmployeeId.Value = EmployeeId;
        thisCategory.Value = Category;
        thisStatus.Value = Status;

        try
        {
            weekShow = Show;
            DateTime theDateTime = Convert.ToDateTime(theDate);
            labDate.Text = theDateTime.Day.ToString();
            //labDate2.Text = theDateTime.Day.ToString();

            strWeek = theDateTime.DayOfWeek.ToString();
            switch (Show)
            { 
                case "TW":
                    strWeek = "星期" + Setting.WeekShow(theDateTime.DayOfWeek);
                    break;
                case "TW-S":
                    strWeek = Setting.WeekShow(theDateTime.DayOfWeek);
                    break;
                case "EN":                   
                    break;
                case "EN-S":
                    strWeek = strWeek.Substring(0, 3);
                    break;
            }
            labWeek.Text = strWeek;
            
            labYM.Text = theDateTime.Year.ToString() + "年" + theDateTime.Month.ToString() + "月";
            
            CalendarDay1.SetgvCalendarDay(Show, Company, theDate, "", DepId, EmployeeId, Category, Status);
            if (theDateTime.DayOfWeek == DayOfWeek.Saturday || theDateTime.DayOfWeek == DayOfWeek.Sunday || CalendarDay1.isHoliday())
                TitleForeColor((HoliDayColor.Name.Equals("0")) ? defHoliDayColor : HoliDayColor);
            else if (theDateTime.Equals(DateTime.Today))
                TitleForeColor((TodayColor.Name.Equals("0")) ? defTodayColor : TodayColor);
            else
                TitleForeColor((DayColor.Name.Equals("0")) ? defDayColor : DayColor);
            CalendarDay1.HeaderVisible = false;
            if (showDefCols == true)
                CalendarDay1.showDefauleColumns();
        }
        catch(Exception ex)
        {
            //
        }
    }

    /// <summary>
    /// 設定顯示資料行
    /// </summary>
    /// <param name="showCols"></param>
    public void SetShowColumns(int[] showCols)
    {
        showDefCols = false;
        CalendarDay1.SetShowColumns(showCols);
    }

    /// <summary>
    /// 預設顯示資料行
    /// </summary>
    public void showDefauleColumns()
    {
        CalendarDay1.showDefauleColumns();
    }

    /// <summary>
    /// 設定標題顏色
    /// </summary>
    /// <param name="theColor"></param>
    public void TitleForeColor(Color theColor)
    {
        labDate.ForeColor = theColor;
        labWeek.ForeColor = theColor;
        labYM.ForeColor = theColor;
    }

    /// <summary>
    /// 設定標題文字大小
    /// </summary>
    /// <param name="DateSize">日期文字大小</param>
    /// <param name="WeekSize">年月與星期文字大小</param>
    public void TitleForeSize(FontUnit DateSize, FontUnit WeekSize)
    {
        labDate.Font.Size = DateSize;
        labWeek.Font.Size = WeekSize;
        labYM.Font.Size = WeekSize;
    }

    /// <summary>
    /// 是否是假日
    /// </summary>
    /// <returns></returns>
    public bool isHoliday()
    {
        return CalendarDay1.isHoliday();
    }

    /// <summary>
    /// 是否顯示當日資料
    /// </summary>
    /// <returns></returns>
    public void ShowDayData ( bool visible )
    {
        CalendarDay1.Visible = visible;
    }

    public Unit PanelDayHeight
    {
        get
        {
            return PanelDay.Height;
        }
        set
        {
            PanelDay.Height = value;
        }
    }

    /// <summary>
    /// 設定或取得否顯示上下一日
    /// </summary>
    public bool ShowLastAndNext
    {
        get {
            return PanelLastAndNext.Visible;
        }
        set {
            PanelLastAndNext.Visible = value;
        }
    }

    /// <summary>
    /// 設定上一日
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Last(object sender, ImageClickEventArgs e)
    {
        SetDate(thisShow.Value, thisCompany.Value, ((Convert.ToDateTime(thistheDate.Value)).AddDays(-1)).ToString("yyyy/MM/dd"), thisDepId.Value, thisEmployeeId.Value, thisCategory.Value, thisStatus.Value);
    }

    /// <summary>
    /// 設定下一日
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Next(object sender, ImageClickEventArgs e)
    {
        SetDate(thisShow.Value, thisCompany.Value, ((Convert.ToDateTime(thistheDate.Value)).AddDays(1)).ToString("yyyy/MM/dd"), thisDepId.Value, thisEmployeeId.Value, thisCategory.Value, thisStatus.Value);
    }
}
