using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// WSDialogData_Table 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class WSDialogData_Table : System.Web.Services.WebService
{
    public WSDialogData_Table()
    {
        //如果使用設計的元件，請取消註解下行程式碼 
        //InitializeComponent(); 
    }
    /// <summary>
    /// 取得Dialog資料
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetDialogDataTable()
    {
        string jsonString = string.Empty;

        zfNounManager manager = new zfNounManager();
        DataTable dt = manager.GetTodoCurrentStatusDT();

        if (dt != null && dt.Rows.Count > 0)
            jsonString = Jtool.ToJson(dt);

        return jsonString;
    }

}
