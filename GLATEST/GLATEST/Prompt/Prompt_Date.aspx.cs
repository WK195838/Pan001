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

public partial class Prompt_Prompt_Date : System.Web.UI.Page
{
    public SysSetting SysSet = new SysSetting();
    int BeginYear;
    int EndYear;

    protected override void OnInit(EventArgs e)
    {
        //if (SysSet.isTWCalendar)
        //{
        //    System.Globalization.CultureInfo cag = new System.Globalization.CultureInfo("zh-TW");
        //    cag.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
        //    System.Threading.Thread.CurrentThread.CurrentCulture = cag;
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BeginYear = Convert.ToInt32(Request["BeginYear"].ToString());
            EndYear = Convert.ToInt32(Request["EndYear"].ToString());
        }
        catch
        {
            //
        }

        if (BeginYear > 0 && BeginYear < EndYear)
        {
            //
        }
        else
        {
            BeginYear = SysSet.YearB;
            EndYear = SysSet.YearE;
        }

        if (!Page.IsPostBack)
        {
            if (SysSet.isTWCalendar)
                lbYear.Text = "民國";
            else
                lbYear.Text = "西元";

            if (BeginYear > 0 && BeginYear < EndYear)
            {
                YearList1.SetYearList(BeginYear, EndYear, "");
            }
            else
            {
                BeginYear = SysSet.YearB;
                EndYear = SysSet.YearE;

                YearList1.initList();
            }

            try
            {
                string theDate = Request["theValue"].ToString();
                if (string.IsNullOrEmpty(theDate))
                {
                    Calendar1.SelectedDate = DateTime.Today;   
                }
                else
                {
                    Calendar1.SelectedDate = Convert.ToDateTime(SysSet.FormatADDate(theDate));                    
                }                
            }
            catch {
                Calendar1.SelectedDate = DateTime.Today;
            }
            SetDate();            
        }

        //Calendar1_SelectionChanged
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        if (Calendar1.SelectedDate.Year.ToString() != YearList1.SelectADYear)
        {
            Calendar1.SelectedDate = Convert.ToDateTime(YearList1.SelectADYear.ToString() + "/" + Calendar1.SelectedDate.Month.ToString().PadLeft(2, '0') + "/" + Calendar1.SelectedDate.Day.ToString().PadLeft(2, '0'));
        }

        Session["Date_Select"] = Calendar1.SelectedDate.ToString("yyyy/MM/dd");

        string strDate = "";
        strDate = SysSet.FormatDate(Session["Date_Select"].ToString());

        string strScript = "<script Language=JavaScript>window.returnValue = '" + strDate + ":" + Session["Date_Select"] + "';window.close();</script>";
        this.ClientScript.RegisterStartupScript(this.GetType(), "window.close", strScript);
    }

    protected void Calendar1_Init(object sender, EventArgs e)
    {
        //Calendar1.ShowTitle = false;
       
    }

    protected void YearList1_Load(object sender, EventArgs e)
    {
        int selectYear = Convert.ToInt32(YearList1.SelectADYear);
        if (Calendar1.SelectedDate.Year != selectYear)
        {
            Calendar1.SelectedDate = Calendar1.SelectedDate.AddYears(selectYear - Calendar1.SelectedDate.Year);
            SetDate();
        }
    }

    protected void MonthList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectMounth = MonthList.SelectedIndex + 1;
        if (Calendar1.SelectedDate.Month != selectMounth)
        {
            Calendar1.SelectedDate = Calendar1.SelectedDate.AddMonths(selectMounth - Calendar1.SelectedDate.Month);
            SetDate();
        }
    }

    protected void ibLastM_Click(object sender, ImageClickEventArgs e)
    {
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddMonths(-1);
        SetDate();
    }

    protected void ibNestM_Click(object sender, ImageClickEventArgs e)
    {
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddMonths(1);
        SetDate();
    }

    protected void ibLastY_Click(object sender, ImageClickEventArgs e)
    {
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddYears(-1);
        SetDate();
    }

    protected void ibNestY_Click(object sender, ImageClickEventArgs e)
    {
        Calendar1.SelectedDate = Calendar1.SelectedDate.AddYears(1);
        SetDate();
    }

    protected void SetDate()
    {
        DateTime theVisibleDate = Calendar1.SelectedDate;
        int selectYear = (SysSet.isTWCalendar) ? (theVisibleDate.Year - 1911) : theVisibleDate.Year;
        if (BeginYear > selectYear || EndYear < selectYear)
        {
            //;
        }
        else
        {
            string theDate = SysSet.FormatDate(theVisibleDate.ToString("yyyy/MM/dd"));
            Session["Date_Select"] = theVisibleDate.ToString("yyyy/MM/dd");
            //改變年份下拉單
            YearList1.SelectYear = theDate.Remove(theDate.IndexOf('/'));
            //改變月份下拉單
            MonthList.SelectedIndex = (theVisibleDate.Month - 1);
            Calendar1.VisibleDate = theVisibleDate;
            //滑鼠在月曆上非控制項時,顯示的文字
            Calendar1.ToolTip = theDate;
        }
    }
    protected void btToday_Click(object sender, EventArgs e)
    {
        Calendar1.SelectedDate = DateTime.Today;
        SetDate();
    }
    
    protected void btRetDate_Click(object sender, EventArgs e)
    {
        returnDate();
    }

    protected void returnDate()
    {
        Session["Date_Select"] = Calendar1.SelectedDate.ToString("yyyy/MM/dd");

        string strDate = "";
        strDate = SysSet.FormatDate(Session["Date_Select"].ToString());

        string strScript = "<script Language=JavaScript>window.returnValue = '" + strDate + ":" + Session["Date_Select"] + "';window.close();</script>";
        this.ClientScript.RegisterStartupScript(this.GetType(), "window.close", strScript);
    }
}
