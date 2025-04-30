using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

/// <summary>
/// WSGLAcctDefJson 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class WSGLAcctDefJson : System.Web.Services.WebService {

    public WSGLAcctDefJson () {
        //如果使用設計的元件，請取消註解下行程式碼 
        //InitializeComponent(); 
    }
    /// <summary>
    /// 取得會計科目GLAcctDef
    /// </summary>
    /// <param name="parms"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetGLAcctDefOnGender(string parms)
    {
        char Delimiter = '＠';
        string jsonString = string.Empty;
        string[] str = parms.Split(Delimiter);
        if (str.Length > 0)
        {
            string PageSize = str[0].ToString();
            string PageNumber = str[1].ToString();
            string TableName = str[2].ToString();
            string KeyName = str[3].ToString();
            string ParmWhere = str[4].ToString();
            TablePagingManager manager = new TablePagingManager();
            DataTable dt = manager.GetTablePagingDT(PageSize, PageNumber, TableName, KeyName, ParmWhere);
            if (dt!=null && dt.Rows.Count>0)
                jsonString = Jtool.ToJson(dt);
        }

        return jsonString;
    }
    
}
