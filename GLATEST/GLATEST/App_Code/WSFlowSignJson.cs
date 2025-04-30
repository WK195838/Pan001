using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// WSFlowSignJson 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public class WSFlowSignJson : System.Web.Services.WebService {
    public WSFlowSignJson () {
    //    //如果使用設計的元件，請取消註解下行程式碼 
    //    //InitializeComponent(); 
    }
    /// <summary>
    /// FetchIsCanSignOnGender Web Service
    /// </summary>
    /// <param name="parms"></param>
    /// <returns></returns>
    [WebMethod(EnableSession=true)]
    public string FetchIsCanSignOnGender(string parms)
    {
        char Delimiter = '＠';
        string jsonString = string.Empty;
        string[] str = parms.Split(Delimiter);
        if (str.Length > 0)
        {
            string UserId = str[0].ToString();
            string Company = str[1].ToString();
            string FlowId = str[2].ToString();
            string SignDocUnid = str[3].ToString();
            WSFlowSignManager manage = new WSFlowSignManager();
            DataSet ds = new DataSet();
            int ret = manage.GetCanSignDS(UserId, Company, FlowId, SignDocUnid, out ds);
            jsonString = ret.ToString();
        }

        return jsonString;
    }
    /// <summary>
    /// FetchCanSignOnGender Web Service
    /// </summary>
    /// <param name="parms"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string FetchCanSignOnGender(string parms)
    {
        char Delimiter = '＠';
        string jsonString = string.Empty;
        string[] str = parms.Split(Delimiter);
        if (str.Length > 0)
        {
            string UserId = str[0].ToString();
            string Company = str[1].ToString();
            string FlowId = str[2].ToString();
            string SignDocUnid = str[3].ToString();
            WSFlowSignManager manage = new WSFlowSignManager();
            DataSet ds = manage.GetCanSignDS(UserId, Company, FlowId, SignDocUnid);
            if (ds != null && ds.Tables.Count > 0)
            {
                jsonString = Jtool.ToJson(ds);
            }
        }

        return jsonString;
    }
    /// <summary>
    /// FetchNextStepSignerOnGender Web Service
    /// </summary>
    /// <param name="parms"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string FetchNextStepSignerOnGender(string parms)
    {
        char Delimiter = '＠';
        string jsonString = string.Empty;
        string[] str = parms.Split(Delimiter);
        if (str.Length > 0)
        {
            string Company = str[0].ToString();
            string FlowId = str[1].ToString();
            string SignDocUnid = str[2].ToString();
            string SingerEmid = str[3].ToString();
            int SignerBwseq = Ftool.IntTryParse(str[4].ToString());
            WSFlowSignManager manage = new WSFlowSignManager();
            DataSet ds = manage.GetNextStepSignerDS(Company, FlowId, SignDocUnid, SingerEmid, SignerBwseq);
            if (ds != null && ds.Tables.Count > 0)
            {
                jsonString = Jtool.ToJson(ds);
            }
        }

        return jsonString;
    }
    /// <summary>
    /// FetchReturnStepsOnGender Web Service
    /// </summary>
    /// <param name="parms"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string FetchReturnStepsOnGender(string parms)
    {
        char Delimiter = '＠';
        string jsonString = string.Empty;
        string[] str = parms.Split(Delimiter);
        if (str.Length > 0)
        {
            string Company = str[0].ToString();
            string FlowId = str[1].ToString();
            string SignDocUnid = str[2].ToString();
            WSFlowSignManager manage = new WSFlowSignManager();
            DataSet ds = manage.GetReturnStepsDS(Company, FlowId, SignDocUnid);
            if (ds != null && ds.Tables.Count > 0)
            {
                jsonString = Jtool.ToJson(ds);
            }
        }

        return jsonString;
    }
}
