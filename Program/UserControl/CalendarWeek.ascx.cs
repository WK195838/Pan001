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

public partial class UserControl_CalendarWeek : System.Web.UI.UserControl
{
    public string weekShow = "TW";
    public string theCalendarStartDay = "";    
    DayOfWeek theFisrtDayOfWeek = DayOfWeek.Monday;
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
    /// 設定指定日期開始一週之行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theStartDate">指定起始日期</param>   
    public void SetWeek(string Company, DateTime theStartDate)
    {
        SetWeek(weekShow, Company, theStartDate.ToString("yyyy/MM/dd"), "", "", "", "");
    }

    /// <summary>
    /// 設定指定日期開始一週之行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theStartDate">指定起始日期</param>    
    public void SetWeek(string Company, string theStartDate)
    {
        SetWeek(weekShow, Company, theStartDate, "", "", "", "");
    }

    /// <summary>
    /// 設定由指定星期開始且包含指定日期之週行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theDate">指定日期</param>
    /// <param name="theStartDayOfWeek">指定起始星期</param>
    public void SetWeek(string Company, DateTime theDateTime, DayOfWeek theStartDayOfWeek)
    {
        SetWeek(weekShow, Company, theDateTime, "", "", "", "", theStartDayOfWeek);
    }

    /// <summary>
    /// 設定由指定星期開始且包含指定日期之週之行事曆
    /// </summary>
    /// <param name="weekShow">星期的顯示方式()</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>
    /// <param name="theStartDayOfWeek">指定星期</param>
    public void SetWeek(string weekShow, string Company, string theDate, string DepId, string EmployeeId, string Category, string Status, DayOfWeek theStartDayOfWeek)
    {
        DateTime theDateTime = Convert.ToDateTime(theDate);
        SetWeek(weekShow, Company, theDateTime, DepId, EmployeeId, Category, Status, theStartDayOfWeek);
    }

    /// <summary>
    /// 設定由指定星期開始且包含指定日期之週之行事曆
    /// </summary>
    /// <param name="weekShow">星期的顯示方式()</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>
    /// <param name="theStartDayOfWeek">指定星期</param>
    public void SetWeek(string weekShow, string Company, DateTime theDateTime, string DepId, string EmployeeId, string Category, string Status, DayOfWeek theStartDayOfWeek)
    {
        //加了這段的話,沒對應公司的人會看不到日曆
        //if (!string.IsNullOrEmpty(Company))
        {
            HiddenField theHF = new HiddenField();
            theHF = ((HiddenField)this.FindControl("thisShow"));
            if (theHF != null)
            {
                theHF.Value = weekShow;
                ((HiddenField)this.FindControl("thisCompany")).Value = Company;
                ((HiddenField)this.FindControl("thistheDate")).Value = theDateTime.ToString("yyyy/MM/dd"); ;
                ((HiddenField)this.FindControl("thisDepId")).Value = DepId;
                ((HiddenField)this.FindControl("thisEmployeeId")).Value = EmployeeId;
                ((HiddenField)this.FindControl("thisCategory")).Value = Category;
                ((HiddenField)this.FindControl("thisStatus")).Value = Status;
            }
            theFisrtDayOfWeek = theStartDayOfWeek;

            if (!theDateTime.DayOfWeek.Equals(theStartDayOfWeek))
            {
                DateTime theFirstDateTime = theDateTime.AddDays(((int)theStartDayOfWeek) - ((int)theDateTime.DayOfWeek));
                if (theFirstDateTime.CompareTo(theDateTime) > 0)
                    theDateTime = theFirstDateTime.AddDays(-7);
                else
                    theDateTime = theFirstDateTime;
            }

            theCalendarStartDay = theDateTime.ToString("yyyy/MM/dd");
            SetWeek(weekShow, Company, theCalendarStartDay, DepId, EmployeeId, Category, Status);
        }
    }

    /// <summary>
    /// 設定由指定星期開始且包含指定日期之週行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theDate">指定日期</param>
    /// <param name="theStartDayOfWeek">指定起始星期</param>
    public void SetWeek(string Company, string theDate, DayOfWeek theStartDayOfWeek)
    {        
        DateTime theDateTime = Convert.ToDateTime(theDate);
        SetWeek(Company, theDateTime, theStartDayOfWeek);
    }

    /// <summary>
    /// 設定由星期一開始且包含指定日期之週行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theDate">指定日期</param>
    public void SetWeekFromMonday(string Company, DateTime theDate)
    {
        SetWeek(Company, theDate, DayOfWeek.Monday);
    }

    /// <summary>
    /// 設定由星期一開始且包含指定日期之週行事曆
    /// </summary>
    /// <param name="Company">公司</param>
    /// <param name="theDate">指定日期</param>
    public void SetWeekFromMonday(string Company, string theDate)
    {
        DateTime theDateTime = Convert.ToDateTime(theDate);
        SetWeek(Company, theDateTime, DayOfWeek.Monday);
    }

    /// <summary>
    /// 設定指定日期開始一週之行事曆
    /// </summary>
    /// <param name="weekShow">星期的顯示方式()</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>
    public void SetWeek(string weekShow, string Company, string theStartDate, string DepId, string EmployeeId, string Category, string Status)
    {
        theCalendarStartDay = theStartDate;
        DateTime theStartDateTime = Convert.ToDateTime(theCalendarStartDay);

        ASP.usercontrol_calendardate_ascx theDate = new ASP.usercontrol_calendardate_ascx();
        for (int i = 0; i < 7; i++)
        {
            theDate = (ASP.usercontrol_calendardate_ascx)this.FindControl("CalendarDate" + (i + 1).ToString());
            if (theDate != null)
            {
                theDate.TodayColor = (TodayColor.Name.Equals("0")) ? defTodayColor : TodayColor;
                theDate.HoliDayColor = (HoliDayColor.Name.Equals("0")) ? defHoliDayColor : HoliDayColor;
                theDate.DayColor = (DayColor.Name.Equals("0")) ? defDayColor : DayColor;
                theDate.SetDate(weekShow, Company, (theStartDateTime.AddDays(i)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
            }
        }

        //CalendarDate1.SetDate(weekShow, Company, theStartDateTime.ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
        //CalendarDate2.SetDate(weekShow, Company, (theStartDateTime.AddDays(1)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);        
        //CalendarDate3.SetDate(weekShow, Company, (theStartDateTime.AddDays(2)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
        //CalendarDate4.SetDate(weekShow, Company, (theStartDateTime.AddDays(3)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
        //CalendarDate5.SetDate(weekShow, Company, (theStartDateTime.AddDays(4)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
        //CalendarDate6.SetDate(weekShow, Company, (theStartDateTime.AddDays(5)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
        //CalendarDate7.SetDate(weekShow, Company, (theStartDateTime.AddDays(6)).ToString("yyyy/MM/dd"), DepId, EmployeeId, Category, Status);
    }

    /// <summary>
    /// 設定顯示資料行
    /// </summary>
    /// <param name="showCols"></param>
    public void SetShowColumns(int[] showCols)
    {
        CalendarDate1.SetShowColumns(showCols);
        CalendarDate2.SetShowColumns(showCols);
        CalendarDate3.SetShowColumns(showCols);
        CalendarDate4.SetShowColumns(showCols);
        CalendarDate5.SetShowColumns(showCols);
        CalendarDate6.SetShowColumns(showCols);
        CalendarDate7.SetShowColumns(showCols);
    }

    /// <summary>
    /// 預設顯示資料行
    /// </summary>
    public void showDefauleColumns()
    {
        CalendarDate1.showDefauleColumns();
        CalendarDate2.showDefauleColumns();
        CalendarDate3.showDefauleColumns();
        CalendarDate4.showDefauleColumns();
        CalendarDate5.showDefauleColumns();
        CalendarDate6.showDefauleColumns();
        CalendarDate7.showDefauleColumns();        
    }

    /// <summary>
    /// 設定標題顏色
    /// </summary>
    /// <param name="theColor"></param>
    /// <param name="theHolidayColor"></param>
    public void TitleForeColor(Color theColor, Color theHolidayColor)
    {
        CalendarDate1.TitleForeColor(CalendarDate1.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate2.TitleForeColor(CalendarDate2.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate3.TitleForeColor(CalendarDate3.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate4.TitleForeColor(CalendarDate4.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate5.TitleForeColor(CalendarDate5.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate6.TitleForeColor(CalendarDate6.isHoliday().Equals(true) ? theHolidayColor : theColor);
        CalendarDate7.TitleForeColor(CalendarDate7.isHoliday().Equals(true) ? theHolidayColor : theColor);     
    }

    /// <summary>
    /// 設定標題文字大小
    /// </summary>
    /// <param name="DateSize">日期文字大小</param>
    /// <param name="WeekSize">年月與星期文字大小</param>
    public void TitleForeSize(FontUnit DateSize, FontUnit WeekSize)
    {
        CalendarDate1.TitleForeSize(DateSize, WeekSize);
        CalendarDate2.TitleForeSize(DateSize, WeekSize);
        CalendarDate3.TitleForeSize(DateSize, WeekSize);
        CalendarDate4.TitleForeSize(DateSize, WeekSize);
        CalendarDate5.TitleForeSize(DateSize, WeekSize);
        CalendarDate6.TitleForeSize(DateSize, WeekSize);
        CalendarDate7.TitleForeSize(DateSize, WeekSize);
    }

    /// <summary>
    /// 是否顯示當日資料
    /// </summary>
    /// <returns></returns>
    public void ShowDayData ( bool visible )
    {
        CalendarDate1.ShowDayData ( visible );
        CalendarDate2.ShowDayData ( visible );
        CalendarDate3.ShowDayData ( visible );
        CalendarDate4.ShowDayData ( visible );
        CalendarDate5.ShowDayData ( visible );
        CalendarDate6.ShowDayData ( visible );
        CalendarDate7.ShowDayData ( visible );
    }



    /// <summary>
    /// 設定否顯示上下一週
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
    /// 設定上一週
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Last(object sender, ImageClickEventArgs e)
    {
        SetWeek(((HiddenField)this.FindControl("thisShow")).Value, ((HiddenField)this.FindControl("thisCompany")).Value, ((Convert.ToDateTime(((HiddenField)this.FindControl("thistheDate")).Value)).AddDays(-7)), ((HiddenField)this.FindControl("thisDepId")).Value, ((HiddenField)this.FindControl("thisEmployeeId")).Value, ((HiddenField)this.FindControl("thisCategory")).Value, ((HiddenField)this.FindControl("thisStatus")).Value, theFisrtDayOfWeek);
    }

    /// <summary>
    /// 設定下一下
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Next(object sender, ImageClickEventArgs e)
    {
        SetWeek(((HiddenField)this.FindControl("thisShow")).Value, ((HiddenField)this.FindControl("thisCompany")).Value, ((Convert.ToDateTime(((HiddenField)this.FindControl("thistheDate")).Value)).AddDays(7)), ((HiddenField)this.FindControl("thisDepId")).Value, ((HiddenField)this.FindControl("thisEmployeeId")).Value, ((HiddenField)this.FindControl("thisCategory")).Value, ((HiddenField)this.FindControl("thisStatus")).Value, theFisrtDayOfWeek);
    }
}
