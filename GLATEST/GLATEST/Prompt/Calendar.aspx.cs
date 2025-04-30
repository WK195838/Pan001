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
    string theDate = "", theCompany = "", theSeqNo = "", theDeptId = "", theEmployeeId = "", theCategory = "", theStatus = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime theDate = DateTime.Today;
        //theDate,theCompany,theSeqNo,theDeptId,theEmployeeId,theCategory,theStatus
        try
        {
            if (Request["theDate"] != null)
            {
                theDate = Convert.ToDateTime(Request["theDate"].ToString());
            }

            if (Request["theCompany"] != null)
            {
                theCompany = Request["theCompany"].ToString();
            }

            if (Request["theSeqNo"] != null)
            {
                theSeqNo = Request["theSeqNo"].ToString();
            }

            if (Request["theDeptId"] != null)
            {
                theDeptId = Request["theDeptId"].ToString();
            }

            if (Request["theEmployeeId"] != null)
            {
                theEmployeeId = Request["theEmployeeId"].ToString();
            }

            if (Request["theCategory"] != null)
            {
                theCategory = Request["theCategory"].ToString();
            }

            if (Request["theStatus"] != null)
            {
                theStatus = Request["theStatus"].ToString();
            }
        }

        catch { }
        Calendar1.SelectedDate = theDate;
        CalendarDay1.SetgvCalendarDay("TW-S", theCompany, theDate.ToString("yyyy/MM/dd"), theSeqNo, theDeptId, theEmployeeId, theCategory, theStatus);
    }
}
