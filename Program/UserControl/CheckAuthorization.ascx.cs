using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_CheckAuthorization : System.Web.UI.UserControl
{
    SysSetting SysSet = new SysSetting();
    public DateTime AuthDueDate = DateTime.Now.AddDays(-1);
    public bool? blAuth = null;
    public bool AlwaysOpenSetAuth = false;
    public bool AlwaysCloseSetAuth = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        isAuth();
    }

    /// <summary>
    /// 驗証授權，並設定授權到期日期
    /// </summary>
    public void isAuth()
    {        
        string[] getAuthValue = { "", "", "", "", "", "","" };
        getAuthValue[1] = DeCode(hid_Auth01.Value);
        getAuthValue[2] = DeCode(hid_Auth02.Value);
        getAuthValue[3] = DeCode(hid_Auth03.Value);
        getAuthValue[4] = DeCode(hid_Auth04.Value);
        getAuthValue[5] = DeCode(hid_Auth05.Value);

        string strDateKind, strTime;
        strDateKind = getAuthValue[1];
        if (strDateKind.Length > 3)
        {
            strDateKind = strDateKind.Substring(0, 3);
            strTime = strDateKind.Substring(3);
        }
        else
        {
            strTime = "";
        }

        switch (strDateKind)
        {
            case "ymd":
                getAuthValue[0] = (GetCode("y",getAuthValue[2]) + "/" + GetCode("M",getAuthValue[3]) + "/" + GetCode("d",getAuthValue[4]));
                break;
            case "myd":
                getAuthValue[0] = (GetCode("y",getAuthValue[3]) + "/" + GetCode("M",getAuthValue[2]) + "/" + GetCode("d",getAuthValue[4]));
                break;
            case "mdy":
                getAuthValue[0] = (GetCode("y",getAuthValue[4]) + "/" + GetCode("M",getAuthValue[2]) + "/" + GetCode("d",getAuthValue[3]));
                break;
            case "dmy":
                getAuthValue[0] = (GetCode("y",getAuthValue[4]) + "/" + GetCode("M",getAuthValue[3]) + "/" + GetCode("d",getAuthValue[2]));
                break;
            case "dym":
                getAuthValue[0] = (GetCode("y",getAuthValue[3]) + "/" + GetCode("M",getAuthValue[4]) + "/" + GetCode("d",getAuthValue[2]));
                break;
            case "ydm":
                getAuthValue[0] = (GetCode("y",getAuthValue[2]) + "/" + GetCode("M",getAuthValue[4]) + "/" + GetCode("d",getAuthValue[3]));
                break;
            default:
                //getAuthValue[0] = DateTime.Now.ToString("yyyy/MM/dd");
                break;
        }

        switch (strTime)
        {
            case "hm":
                getAuthValue[6] = (" " + GetCode("h",getAuthValue[5]) + ":" + GetCode("m",getAuthValue[6]));
                break;
            case "mh":
                getAuthValue[6] = (" " + GetCode("h",getAuthValue[6]) + ":" + GetCode("m",getAuthValue[5]));
                break;
            default:
                //getAuthValue[0] = DateTime.Now.ToString("yyyy/MM/dd");
                break;
        }

        try
        {
            AuthDueDate = Convert.ToDateTime(getAuthValue[0] + getAuthValue[6]);
            if (AuthDueDate >= DateTime.Now)
                blAuth = true;
            else
                blAuth = false;
        }
        catch { }

        switch (blAuth)
        {
            case true:
                labAuthOK.Visible = true;
                labAuthOK.Text = "授權使用至 " + AuthDueDate.ToString("yyyy/MM/dd");
                break;
            case false:
                labAuthEnd.Visible = true;
                break;
            default:
                labUnAuth.Visible = true;
                break;
        }

        if ((AlwaysCloseSetAuth == false && AuthDueDate.CompareTo(DateTime.Now) == -1) || AlwaysOpenSetAuth == true)
            OpenSetAuth();
        else
            CloseSetAuth();
    }

    /// <summary>
    /// 三重解碼
    /// </summary>
    /// <param name="theCode">加密字串</param>
    /// <returns>解密後字串</returns>
    protected string DeCode(string theCode)
    {
        string retStr = theCode.Trim();
        if (string.IsNullOrEmpty(retStr))
            return retStr;
        retStr = SysSet.rtnTrans(theCode);
        retStr = retStr.ToLower().Replace("/", "").Replace("?", "").Replace("*", "").Replace("^", "").Replace(";", "").Replace("|", "");
        return retStr;
    }

    /// <summary>
    /// 取得正確時間單元
    /// </summary>
    /// <param name="theDatePart">y/M/d/h/m:年/月/日/時/分</param>
    /// <param name="theCode">明碼</param>
    /// <returns>回傳時間單元之數值</returns>
    protected string GetCode(string theDatePart, string theCode)
    {
        string retStr = theCode.Trim();
        try
        {
            int iCode = Convert.ToInt32(retStr);
            retStr = retStr.PadLeft(5, '0');
            switch (theDatePart)
            {
                case "y":
                    retStr = retStr.Substring(retStr.Length - 4, 4);
                    iCode = Convert.ToInt32(retStr);
                    if (iCode > 2100)
                        retStr = "20" + retStr.Substring(retStr.Length - 2, 2);
                    else if (iCode <= 200)
                        retStr = (iCode + 1911).ToString();
                    else if (iCode > 200 && iCode < 2000)
                        retStr = DateTime.Now.Year.ToString();
                    break;
                case "M":
                    retStr = retStr.Substring(retStr.Length - 2, 2);
                    iCode = Convert.ToInt32(retStr);
                    if (iCode > 12)
                        retStr = "12";
                    break;
                case "d":
                    retStr = retStr.Substring(retStr.Length - 2, 2);
                    iCode = Convert.ToInt32(retStr);
                    if (iCode > 31)
                        if (Session["SalaryDayEnd"] != null)
                            retStr = Session["SalaryDayEnd"].ToString();
                        else
                            retStr = "01";
                    break;
                case "h":
                    retStr = retStr.Substring(retStr.Length - 2, 2);
                    iCode = Convert.ToInt32(retStr);
                    if (iCode > 24)
                        retStr = "23";                    
                    break;
                case "m":
                    retStr = retStr.Substring(retStr.Length - 2, 2);
                    iCode = Convert.ToInt32(retStr);
                    if (iCode > 59)
                        retStr = "59";    
                    break;
            }
        }
        catch 
        {
            switch (theDatePart)
            {
                case "y":
                    retStr = DateTime.Now.Year.ToString();
                    break;
                case "M":                
                    retStr = "12";
                    break;
                case "d":
                    if (Session["SalaryDayEnd"] != null)
                        retStr = Session["SalaryDayEnd"].ToString();
                    else
                        retStr = "01";
                    break;
                case "h":
                    retStr = "23";
                    break;
                case "m":
                    retStr = "59";
                    break;
            } 
        }
        return retStr;
    }

    public void OpenSetAuth()
    {        
        //if (AuthDueDate.CompareTo(DateTime.Now) == -1)
        {
            string path, FileName,getFile;
            path = Server.MapPath(Request.Url.AbsolutePath);
            path = path.Remove(path.LastIndexOf("\\")) + "\\UserControl";
            //定義上傳後存檔路徑
            path = Server.MapPath("~/UserControl/");
            FileName = "Include.at";                    
            getFile = "http:\\10.10.10.125\\ERP\\SYS\\DownLoad\\200120101.at";
            ib_UpLoadAuth.OnClientClick = "return UploadTo('','Y','ReCover','" + SysSet.rtnHash(path).Replace('\\', '＼') + "','" + FileName + "');";
            ib_UpLoadAuth.Visible = true;
        }
    }

    public void CloseSetAuth()
    {        
        ib_UpLoadAuth.Visible = false;
    }

    public System.Drawing.Color MsgForeColor
    {
        get 
        {
            return labUnAuth.ForeColor;
        }
        set
        {
            labUnAuth.ForeColor = value;
            labAuthEnd.ForeColor = value;
            labAuthOK.ForeColor = value;
        }
    }
}