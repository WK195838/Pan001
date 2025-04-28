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

public partial class UserControl_CalendarMonth : System.Web.UI.UserControl
{
    public string weekShow = "TW";
    public DayOfWeek theFisrtDayOfWeek = DayOfWeek.Monday;
    public string theCalendarStartDay = "";
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

    public void SetMonth(string Company, string theStartDate)
    {
        SetMonth(weekShow, Company, theStartDate, "", "", "", "", theFisrtDayOfWeek);
    }

    public void SetMonth(string Company, DateTime theStartDate)
    {
        SetMonth(weekShow, Company, theStartDate.ToString("yyyy/MM/dd"), "", "", "", "", theFisrtDayOfWeek);
    }

    public void SetMonth(string weekShow, string Company, string theStartDate, string DepId, string EmployeeId, string Category, string Status, DayOfWeek theDayOfWeek)
    {        
        theCalendarStartDay = theStartDate;
        thisShow.Value = weekShow;
        thisCompany.Value = Company;
        thistheDate.Value = theStartDate;
        thisDepId.Value = DepId;
        thisEmployeeId.Value = EmployeeId;
        thisCategory.Value = Category;
        thisStatus.Value = Status;
        theFisrtDayOfWeek = theDayOfWeek;

        DateTime theStartDateTime = Convert.ToDateTime(theCalendarStartDay);
        labThisMonth.Text = theStartDateTime.Year.ToString() + "年" + theStartDateTime.Month.ToString() + "月";

        theStartDateTime = theStartDateTime.AddDays(1 - theStartDateTime.Day);
        //CalendarWeekine1.SetWeek(weekShow, Company, theStartDateTime.ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        //CalendarWeekine2.SetWeek(weekShow, Company, (theStartDateTime.AddDays(1 * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        //CalendarWeekine3.SetWeek(weekShow, Company, (theStartDateTime.AddDays(2 * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        //CalendarWeekine4.SetWeek(weekShow, Company, (theStartDateTime.AddDays(3 * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        
        //if (theStartDateTime.AddDays(4 * 7).Month.Equals(theStartDateTime.Month))
        //    CalendarWeekine5.SetWeek(weekShow, Company, (theStartDateTime.AddDays(4 * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        //else
        //    CalendarWeekine5.Visible = false;

        //if (theStartDateTime.AddDays(5 * 7).Month.Equals(theStartDateTime.Month))
        //    CalendarWeekine6.SetWeek(weekShow, Company, (theStartDateTime.AddDays(5 * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
        //else
        //    CalendarWeekine6.Visible = false;


        ASP.usercontrol_calendarweekline_ascx theDate = new ASP.usercontrol_calendarweekline_ascx();
        for (int i = 0; i < 6; i++)
        {
            theDate = (ASP.usercontrol_calendarweekline_ascx)this.FindControl("CalendarWeekine" + (i + 1).ToString());
            if (theDate != null)
            {
                theDate.TodayColor = (TodayColor.Name.Equals("0")) ? defTodayColor : TodayColor;
                theDate.HoliDayColor = (HoliDayColor.Name.Equals("0")) ? defHoliDayColor : HoliDayColor;
                theDate.DayColor = (DayColor.Name.Equals("0")) ? defDayColor : DayColor;
                theDate.SetWeek(weekShow, Company, (theStartDateTime.AddDays(i * 7)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status, theDayOfWeek);
                if (!(theStartDateTime.AddDays(i * 7).Month.Equals(theStartDateTime.Month)))
                    theDate.Visible = false;
            }
        }
    }

    /// <summary>
    /// 設定顯示資料行
    /// </summary>
    /// <param name="showCols"></param>
    public void SetShowColumns(int[] showCols)
    {
        CalendarWeekine1.SetShowColumns(showCols);
        CalendarWeekine2.SetShowColumns(showCols);
        CalendarWeekine3.SetShowColumns(showCols);
        CalendarWeekine4.SetShowColumns(showCols);
        CalendarWeekine5.SetShowColumns(showCols);
        CalendarWeekine6.SetShowColumns(showCols);        
    }

    /// <summary>
    /// 預設顯示資料行
    /// </summary>
    public void showDefauleColumns()
    {
        CalendarWeekine1.showDefauleColumns();
        CalendarWeekine2.showDefauleColumns();
        CalendarWeekine3.showDefauleColumns();
        CalendarWeekine4.showDefauleColumns();
        CalendarWeekine5.showDefauleColumns();
        CalendarWeekine6.showDefauleColumns();        
    }


    /// <summary>
    /// 是否顯示當日資料
    /// </summary>
    /// <returns></returns>
    public void ShowDayData ( bool visible )
    {
        CalendarWeekine1.ShowDayData ( visible );
        CalendarWeekine2.ShowDayData ( visible );
        CalendarWeekine3.ShowDayData ( visible );
        CalendarWeekine4.ShowDayData ( visible );
        CalendarWeekine5.ShowDayData ( visible );
        CalendarWeekine6.ShowDayData ( visible );
    }


    /// <summary>
    /// 設定標題顏色
    /// </summary>
    /// <param name="theColor"></param>
    /// <param name="theHolidayColor"></param>
    public void TitleForeColor(Color theColor, Color theHolidayColor)
    {
        CalendarWeekine1.TitleForeColor(theColor, theHolidayColor);
        CalendarWeekine2.TitleForeColor(theColor, theHolidayColor);
        CalendarWeekine3.TitleForeColor(theColor, theHolidayColor);
        CalendarWeekine4.TitleForeColor(theColor, theHolidayColor);
        CalendarWeekine5.TitleForeColor(theColor, theHolidayColor);
        CalendarWeekine6.TitleForeColor(theColor, theHolidayColor);        
    }

    /// <summary>
    /// 設定標題文字大小
    /// </summary>
    /// <param name="DateSize">日期文字大小</param>
    /// <param name="WeekSize">年月與星期文字大小</param>
    public void TitleForeSize(FontUnit DateSize, FontUnit WeekSize)
    {
        CalendarWeekine1.TitleForeSize(DateSize, WeekSize);
        CalendarWeekine2.TitleForeSize(DateSize, WeekSize);
        CalendarWeekine3.TitleForeSize(DateSize, WeekSize);
        CalendarWeekine4.TitleForeSize(DateSize, WeekSize);
        CalendarWeekine5.TitleForeSize(DateSize, WeekSize);
        CalendarWeekine6.TitleForeSize(DateSize, WeekSize);        
    }

    /// <summary>
    /// 設定否顯示上下一月
    /// </summary>
    public bool ShowLastAndNext
    {
        get
        {
            return PanelLastAndNext.Visible;
        }
        set
        {
            PanelLastAndNext.Visible = value;
        }
    }
    
    /// <summary>
    /// 設定上一月
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Last(object sender, ImageClickEventArgs e)
    {
        SetMonth(thisShow.Value, thisCompany.Value, ((Convert.ToDateTime(thistheDate.Value)).AddMonths(-1)).ToString("yyyy/MM/dd"), thisDepId.Value, thisEmployeeId.Value, thisCategory.Value, thisStatus.Value, theFisrtDayOfWeek);
    }

    /// <summary>
    /// 設定下一月
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Next(object sender, ImageClickEventArgs e)
    {
        SetMonth(thisShow.Value, thisCompany.Value, ((Convert.ToDateTime(thistheDate.Value)).AddMonths(1)).ToString("yyyy/MM/dd"), thisDepId.Value, thisEmployeeId.Value, thisCategory.Value, thisStatus.Value, theFisrtDayOfWeek);
    }
}
