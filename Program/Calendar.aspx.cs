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

public partial class Calendar : System.Web.UI.Page
{
    UserInfo _UserInfo = new UserInfo();
    int iTabCount = 3;
    
    protected void Page_PreInit(object sender, EventArgs e)
    {//頁面初始化前,先設定好系統使用的佈景主題(主要是決定LOGO)
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CalendarMonth1.TodayColor = System.Drawing.Color.Green;
        CalendarMonth1.showDefCols = false;
        if (!Page.IsPostBack)
        {
            CalendarDate1.SetDate(_UserInfo.UData.Company, DateTime.Today);

            CalendarWeek1.SetWeekFromMonday(_UserInfo.UData.Company, DateTime.Today);
            CalendarWeek1.TitleForeSize(30, 10);

            
            CalendarMonth1.SetMonth(_UserInfo.UData.Company, DateTime.Today);
            CalendarMonth1.TitleForeSize(20, 8);

            CalendarDate1.Visible = false;
            CalendarWeek1.Visible = true;
            CalendarMonth1.Visible = false;
            CalendarDate1.ShowLastAndNext = true;
            CalendarWeek1.ShowLastAndNext = true;
            CalendarMonth1.ShowLastAndNext = true;            
        }
        CalendarMonth1.SetShowColumns(new int[] { 0, 10 });
    }

    ////////////////////////////////////////////////////////////////////
    /// <summary>
    /// For Tab INCLUDE FILE
    /// </summary>
    ////////////////////////////////////////////////////////////////////
    protected void Day_Click ( object sender , EventArgs e )
    {
        CalendarDate1.Visible = true;
        CalendarWeek1.Visible = false;
        CalendarMonth1.Visible = false;

        CalendarDate1.SetDate(_UserInfo.UData.Company, DateTime.Today);
        CalendarWeek1.TitleForeSize ( 24 , 9 );
    }
    protected void Week_Click ( object sender , EventArgs e )
    {
        CalendarDate1.Visible = false;
        CalendarWeek1.Visible = true;
        CalendarMonth1.Visible = false;

        CalendarWeek1.SetWeekFromMonday(_UserInfo.UData.Company, DateTime.Today);
        CalendarWeek1.TitleForeSize ( 24 , 9 );

    }
    protected void Months_Click (object sender, EventArgs e)
    {
        CalendarDate1.Visible = false;
        CalendarWeek1.Visible = false;
        CalendarMonth1.Visible = true;
        CalendarMonth1.ShowDayData ( false );
        CalendarMonth1.SetMonth(_UserInfo.UData.Company, DateTime.Today);
        CalendarMonth1.TitleForeSize(24, 9);

    }

    protected void DoTab_Click(int tabNo, object sender, EventArgs e)
    {
    }


}
