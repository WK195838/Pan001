using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;

/// <summary>
/// WSAutoComplete 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class WSAutoComplete : System.Web.Services.WebService
{
    string customerUnid = string.Empty;
    string customerName = string.Empty;
    string contactUnid = string.Empty;
    string contactName = string.Empty;
    string zipcode = string.Empty;
    string city = string.Empty;
    string town = string.Empty;
    string address = string.Empty;
    string tel = string.Empty;
    string fax = string.Empty;
    int accountDay = 0;
    int payDay = 0;
    string paymentTerm = string.Empty;
    string invoiceZipCode = string.Empty;
    string invoiceAddress = string.Empty;
    string invoiceCity = string.Empty;
    string invoiceTown = string.Empty;
    string vatNumber = string.Empty;

    public WSAutoComplete()
    {

        //如果使用設計的元件，請取消註解下行程式碼 
        //InitializeComponent(); 
    }

    [WebMethod]
    public IList<string> GetCustomers(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 20 CustomerUnid,CustomerNo,CustomerName From vCustomer Where CustomerName Like '%{0}%' or CustomerShortName like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["CustomerName"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    /// <summary>
    /// 選單取得客戶資料
    /// </summary>
    /// <param name="keywords">客戶識別碼</param>
    /// <returns>客戶識別碼/聯絡人識別碼/聯絡人/郵遞區號/縣市/鄉鎮/地址/電話/傳真/統一編號/結帳日/付款日/付款條件/郵遞區號_發票/縣市_發票/鄉鎮_發票/地址_發票</returns>
    [WebMethod]
    public IList<string> GetCustomerUNID(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 * From vCustomer Where CustomerName = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    customerUnid = dr["CustomerUnid"].ToString();
                    contactUnid = dr["ContactUnid"].ToString();
                    contactName = dr["Contact"].ToString();
                    zipcode = dr["ZipCode"].ToString();
                    city = dr["CityName"].ToString();
                    town = dr["TownName"].ToString();
                    address = dr["Address"].ToString();
                    tel = dr["Telephone"].ToString();
                    fax = dr["Fax"].ToString();
                    vatNumber = dr["VatNumber"].ToString();
                    //----客戶明細檔資料----
                    accountDay = Ftool.IntTryParse(dr["AccountDay"].ToString());
                    payDay = Ftool.IntTryParse(dr["PayDay"].ToString());
                    paymentTerm = dr["PaymentTerm"].ToString();
                    invoiceZipCode = dr["InvoiceZipCode"].ToString();
                    invoiceCity = dr["InvoiceCityName"].ToString();
                    invoiceTown = dr["InvoiceTownName"].ToString();
                    invoiceAddress = dr["InvoiceAddress"].ToString();
                }
                result.Add(customerUnid + "＠" + contactUnid + "＠" + contactName + "＠" + zipcode + "＠" + city + "＠" + town + "＠" + address + "＠" + tel + "＠" + fax + "＠" + vatNumber + "＠" + accountDay + "＠" + payDay + "＠" + paymentTerm + "＠" + invoiceZipCode + "＠" + invoiceCity + "＠" + invoiceTown + "＠" + invoiceAddress);
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    [WebMethod]
    public IList<string> GetzfByWorkUNID(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 EmployeeName,EmployeeUnid from vzfEmployee where EmployeeName like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["EmployeeName"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }
    
    [WebMethod]
    public IList<string> GetEmployeeNameEx(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 EmployeeName,EnglishName,EmployeeUnid from vzfEmployee where EmployeeName like '%{0}%' Or EnglishName like '%{1}%'", keywords, keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["EmployeeName"].ToString() + "（" + dr["EnglishName"].ToString() + "）");
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public IList<string> GetDepartmentName(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 DepartmentUnid,DeptName,HeadName from vHMA_Department where DeptName like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["DeptName"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public IList<string> GetDepartmentUNID(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 DepartmentUnid From Department Where DeptName = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["DepartmentUnid"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    [WebMethod]
    public IList<string> GetDateCondition(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Empty;
        string[] tmp = keywords.Split('@');
        switch (tmp[0])
        {
            case "Custom":
            case "preWeek":
                sql = string.Format("select DATEADD(day,-7,'{0}') as EndDate ", tmp[1].ToString());
                break;
            case "preMonth":
                sql = string.Format("select DATEADD(day,1,DATEADD(month,-1,'{0}'))as EndDate ", tmp[1].ToString());
                break;
            case "preQuarter":
                sql = string.Format("select DATEADD(day,1,DATEADD(month,-3,'{0}'))as EndDate ", tmp[1].ToString());
                break;
            case "Week":
                sql = string.Format("select Dateadd(Day, 6-dbo.fnzf_WeekDay('{0}'), '{0}') as EndDate, Dateadd(Day, 0-dbo.fnzf_WeekDay('{0}'), '{0}') as StartDate", tmp[1].ToString());
                break;
            case "Month":
                sql = string.Format("select Dateadd(Day, 0-Datepart(day, '{0}')+1, '{0}') as StartDate,dbo.fnzf_DateMonthEnd('{0}') as EndDate", tmp[1].ToString());
                break;
            case "Quarter":
                sql = string.Format("SELECT DATEPART(month, '{0}') ", tmp[1].ToString());
                break;
        }
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                switch (tmp[0])
                {
                    case "preWeek":
                    case "preMonth":
                    case "preQuarter":
                        foreach (DataRow dr in dt.Rows)
                        {
                            string endday = string.Format("{0:yyyy/MM/dd}", Ftool.DatetimeTryParse(dr["EndDate"].ToString()));
                            result.Add(endday + "@" + tmp[1].ToString());
                        }
                        break;
                    case "Week":
                    case "Month":
                        foreach (DataRow dr in dt.Rows)
                        {
                            string starday = string.Format("{0:yyyy/MM/dd}", Ftool.DatetimeTryParse(dr["StartDate"].ToString()));
                            string endday = string.Format("{0:yyyy/MM/dd}", Ftool.DatetimeTryParse(dr["EndDate"].ToString()));
                            result.Add(starday + "@" + endday);
                        }
                        break;
                    case "Custom":
                        result.Add(tmp[1].ToString() + "@" + tmp[1].ToString());
                        break;
                    case "Quarter":
                        string tmps = tmp[1].ToString();
                        string[] d = tmp[1].ToString().Split('/');
                        switch (dt.Rows[0][0].ToString())
                        {
                            case "1":
                            case "2":
                            case "3":
                                result.Add(d[0].ToString() + "/01/31" + "@" + d[0].ToString() + "/03/31");
                                break;
                            case "4":
                            case "5":
                            case "6":
                                result.Add(d[0].ToString() + "/04/30" + "@" + d[0].ToString() + "/06/30");
                                break;
                            case "7":
                            case "8":
                            case "9":
                                result.Add(d[0].ToString() + "/07/31" + "@" + d[0].ToString() + "/09/30");
                                break;
                            case "10":
                            case "11":
                            case "12":
                                result.Add(d[0].ToString() + "/10/31" + "@" + d[0].ToString() + "/12/31");
                                break;
                        }
                        break;
                }
            }
            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public IList<string> GetProductCategoryName(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 * From dbo.fn_GetNounChildAndGrandchildList('7750A174F41FDEE7482574B00011EBC1') where NounName like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["ShowName"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public IList<string> GetProductCategoryUNID(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 NounUnid From dbo.fn_GetNounChildAndGrandchildList('7750A174F41FDEE7482574B00011EBC1') Where ShowName = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["NounUnid"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    #region 會議名稱
    
    [WebMethod]
    public IList<string> GetMeetingSetName(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 20 MeetingSetUnid, MeetingName From MMS_MeetingSet Where MeetingName like '%{0}%' And ClosedDateTime is null Order by MeetingName", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["MeetingName"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    [WebMethod]
    public IList<string> GetMeetingSetUnid(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 * From MMS_MeetingSet Where MeetingName = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string meetingSetUnid = dr["MeetingSetUnid"].ToString();
                    string meetingName = dr["MeetingName"].ToString();
                    result.Add(meetingSetUnid + "＠" + meetingName);
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }

    #endregion

    #region 會議室名稱
    [WebMethod]
    public IList<string> GetMeetingRoomName(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 RoomName From vMMS_MeetingRoom  Where RoomName Like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["RoomName"].ToString().Trim());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public string GetMeetingRoomUnid(string keywords)
    {
        IList<object> result = new List<object>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 * From vMMS_MeetingRoom Where RoomName = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(
                        new { MeettingRoomUnid = dr["MeetingRoomUnid"].ToString().Trim(), RoomId = dr["RoomId"].ToString().Trim(), CreatorUnid = dr["CreatorUnid"].ToString().Trim() }
                        );
                }
            }
            else
            {
                return null;
            }

            return result[0].ObjectToJson();
        }
        catch
        {
            return null;
        }
    } 
    #endregion

    #region 待辦事項
    [WebMethod]
    public IList<string> GetTodoName(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select top 20 * From dbo.WFA_Todo Where Subject like '%{0}%'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["Subject"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }
    }

    [WebMethod]
    public IList<string> GetTodoUnid(string keywords)
    {
        IList<string> result = new List<string>();
        DBManger _db = new DBManger();
        _db.New();
        string sql = string.Format("Select Top 1 TodoUnid From WFA_Todo Where Subject = '{0}'", keywords);
        DataTable dt = new DataTable();

        try
        {
            dt = _db.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["TodoUnid"].ToString());
                }
            }
            else
            {
                return null;
            }

            return result;
        }
        catch
        {
            return null;
        }

    }
    #endregion
}
