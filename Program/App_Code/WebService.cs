using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Script.Services;

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Reflection;

[WebService ( Namespace = "http://tempuri.org/" )]
[WebServiceBinding ( ConformsTo = WsiProfiles.None )]
[System.ComponentModel.ToolboxItem ( false )]


// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
// [System.Web.Script.Services.ScriptService]
[ScriptService]
public class WebService : System.Web.Services.WebService {

    public WebService () {

        //如果使用設計的元件，請取消註解下行程式碼 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public List<clsCompany> FetchCompanyList ( string Company )
    {
        clsCompany emp = new clsCompany ( );
        return GetCompanyList ( Company.ToLower ( ).Trim ( ) ).ToList ( );
    }

    protected List<clsCompany> GetCompanyList ( string mQ )
    {
        DBManger _MyDBM = new DBManger ( );
        string sql;

        _MyDBM.New ( );
        sql = "SELECT TOP 10 Company ,Address ,Contact,Phone FROM " +
                "( " +
                "SELECT TOP 10 *, 1 AS o FROM Company WHERE Company LIKE '%[mQ]%' UNION " +
                "SELECT TOP 10 *, 2 AS o FROM Company WHERE CompanyShortName LIKE '[mQ]%' UNION " +
                "SELECT TOP 10 *, 3 AS o FROM Company WHERE Address LIKE '%[mQ]%' " +
                ") s ORDER BY o";

        sql = sql.Replace ( "[mQ]" , mQ );

        DataTable DT = _MyDBM.ExecuteDataTable ( sql );
        List<clsCompany> empList = new List<clsCompany> ( );

        for ( int i = 0 ; i < 1 ; i++ )
        {
            empList.Add ( new clsCompany ( )
            {
                ID = i ,
                Company = "A" ,
                Phone = "B" ,
                Address = "C" ,
                Contact = "D"

                //Company = DT.Rows [ i ] [ "Company" ].ToString ( ).Trim ( ) ,
                //Phone = DT.Rows [ i ] [ "Phone" ].ToString ( ).Trim ( ) ,
                //Address = DT.Rows [ i ] [ "Address" ].ToString ( ).Trim ( ) ,
                //Contact = DT.Rows [ i ] [ "Contact" ].ToString ( ).Trim ( )
            } );
        }
        _MyDBM = null;
        return empList;
    }
}

public class clsCompany
{
    public int ID { get; set; }

    /// <summary>
    /// 公司
    /// </summary>
    public string Company { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// 聯絡人
    /// </summary>
    public string Contact { get; set; }
    /// <summary>
    /// 連絡電話
    /// </summary>
    public string Phone { get; set; }

    public clsCompany ( )
    {
        //
        // TODO: Add constructor logic here
        //
    }




    public string CompanyShortName { get; set; }
}
