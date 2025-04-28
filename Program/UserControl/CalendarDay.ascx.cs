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

public partial class UserControl_CalendarDay : System.Web.UI.UserControl
{
    public string theCalendarDay = "";

    protected void Page_Load(object sender, EventArgs e)
    {

    }    

    /// <summary>
    /// 設定指定日期行事曆
    /// </summary>
    /// <param name="Show">星期的顯示方式()</param>
    /// <param name="Company">公司</param>
    /// <param name="theDate">日期</param>
    /// <param name="DepId">部門</param>
    /// <param name="EmployeeId">員工</param>
    /// <param name="Category">分類</param>
    /// <param name="Status">狀態</param>
    public void SetgvCalendarDay(string Show, string Company, string theDate, string SeqNo, string DepId, string EmployeeId, string Category, string Status)
    {
        DataTable CD = DBSetting.CalendarData(Show, Company, theDate, SeqNo, DepId, EmployeeId, Category, Status);
        theCalendarDay = theDate;
        gvCalendarDay.DataSource = CD;
        gvCalendarDay.DataBind();
    }

    /// <summary>
    /// 隱藏標題
    /// </summary>
    public bool HeaderVisible
    {
        get
        {
            return gvCalendarDay.HeaderRow.Visible;
        }
        set
        {
            gvCalendarDay.HeaderRow.Visible = value;
        }
    }

    /// <summary>
    /// 設定各別欄位是否顯示
    /// </summary>
    /// <param name="iColumns"></param>
    /// <param name="blVisible"></param>
    public void SetColumnVisible(int iColumns, bool blVisible)
    {
        try
        {
            gvCalendarDay.Columns[iColumns].Visible = blVisible;
        }
        catch { }
    }

    /// <summary>
    /// 預設顯示資料行
    /// </summary>
    public void showDefauleColumns()
    {
        int[] showCols = new int[] { 0, 3, 5, 7, 9 };//3, 8, 10, 12, 14, 15
        int j = 0;

        gvCalendarDay.Columns[gvCalendarDay.Columns.Count - 1].Visible = false;

        for (int i = 0; i < gvCalendarDay.Columns.Count; i++)
        {            
            while (j < showCols.Length)
            {
                if (i < gvCalendarDay.Columns.Count)
                {
                    gvCalendarDay.Columns[i].Visible = false;
                    if (i == showCols[j])
                    {
                        gvCalendarDay.Columns[i].Visible = true;
                        j += 1;
                    }
                    i += 1;                    
                }                
                else
                {
                    j = showCols.Length;
                }
            }            
        }
    }

    /// <summary>
    /// 設定顯示資料行
    /// </summary>
    /// <param name="showCols"></param>
    public void SetShowColumns(int[] showCols)
    {
        for (int i = 0; i < gvCalendarDay.Columns.Count; i++)
        {
            gvCalendarDay.Columns[i].Visible = false;
        }

        for (int j = 0; j < showCols.Length; j++)
        {
            gvCalendarDay.Columns[showCols[j]].Visible = true;
        }
    }

    /// <summary>
    /// 設定各行樣式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCalendarDay_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItem != null)
            {
                e.Row.Attributes.Add("onClick", "javascript:OpenDayCalendar('" +
                    DataBinder.Eval(e.Row.DataItem, "theDate").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "theCompany").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "theSeqNo").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "DeptId").ToString() +                    
                    "','" + DataBinder.Eval(e.Row.DataItem, "EmployeeId").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "Category").ToString() +
                    "','" + DataBinder.Eval(e.Row.DataItem, "Status").ToString() +
                    "');");
            }            
            e.Row.Attributes.Add("onmouseover", "setnewcolor(this);");
            e.Row.Attributes.Add("onmouseout", "setoldcolor(this);");
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "Grid_GridLine";
                e.Row.Cells[i].Style.Add("text-align", "left");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {            
            e.Row.Visible = false;
        }
    }

    /// <summary>
    /// 是否是假日
    /// </summary>
    /// <returns></returns>
    public bool isHoliday()
    {
        bool blHoliday = false;
        try
        {
            if (gvCalendarDay.Rows[0].Cells[gvCalendarDay.Columns.Count - 1].Text.Replace("&nbsp;", "").Trim().Length > 0)
                blHoliday = true;
        }
        catch { }
        return blHoliday;
    }
}
