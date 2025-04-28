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



public partial class Payroll_Johnny_Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        //int ff = GetOnePayroll("01", "001");
        ExcelManger mExcel = new ExcelManger();
        mExcel.ImportPanTimeClock("D:\\當日出勤異常(匯入系統)1.xls", "Sheet4", 1, 0, 7, "AttendanceSheet", "01");

    }
}


