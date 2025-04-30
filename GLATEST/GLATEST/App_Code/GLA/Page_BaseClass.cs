using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using DBClass;

/// <summary>
/// Page_BaseClass 的摘要描述
/// </summary>
public class Page_BaseClass : Page_Base
{
    public Page_BaseClass()
    {
        this.PreInit += new EventHandler(Page_BaseClass_PreInit);
    }
    void Page_BaseClass_PreInit(object sender, EventArgs e)
    {
        _MyCommon = new CommonData();
        _MyLogin = new LoginClass(_MyCommon, this.Page, ViewState);
        CheckLogin();
    }
}
/// <summary>
/// Page_BaseClass 網頁基礎類別,擁有權限認證功能,但不需程式授權
/// </summary>
public class Page_BaseClass2 : Page_Base
{
    public Page_BaseClass2()
    {
        this.PreInit += new EventHandler(Page_BaseClass_PreInit);
    }
    void Page_BaseClass_PreInit(object sender, EventArgs e)
    {
        _MyCommon = new CommonData();
        _MyLogin = new LoginClass(_MyCommon, this.Page, ViewState);
        _MyLogin.EnableAuthCheck = false;
        CheckLogin();
    }
}

public class Page_Base : System.Web.UI.Page
{
    /// <summary>
    /// 權限物件
    /// </summary>
    protected LoginClass _MyLogin;

    public LoginClass MyLogin
    {
        get { return _MyLogin; }
        set { _MyLogin = value; }
    }
    /// <summary>
    /// 資料庫連線及共用物件
    /// </summary>
    protected CommonData _MyCommon;

    public CommonData MyCommon
    {
        get { return _MyCommon; }
    }

    public Page_Base()
    {
        this.Unload += new EventHandler(Page_Base_Unload);
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["userId"] = this._MyCommon.User.UserId;
        }
        base.OnLoad(e);
    }

    protected override void OnLoadComplete(EventArgs e)
    {
        ////產生ltlDoWait物件
        //Literal ltlDoWait = new Literal();
        //ltlDoWait.ID = "ltlDoWait";
        //ltlDoWait.Text = Stool.RegisterJQueryBlockUI(this.Page, PageType.AJAX);
        //this.Page.Controls.Add(ltlDoWait);
        ////產生ltlDoTimeout物件
        //Literal ltlDoTimeout = new Literal();
        //ltlDoTimeout.ID = "ltlDoTimeout";
        //ltlDoTimeout.Text = Stool.RegisterJQueryTimeout(this.Page, PageType.AJAX);
        //this.Page.Controls.Add(ltlDoTimeout);

        base.OnLoadComplete(e);
    }

    void Page_Base_Unload(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["DBClass"] != null)
        {
            OleDbConnection OLEDB = (OleDbConnection)HttpContext.Current.Session["DBClass"];
            try
            {
                OLEDB.Close();
                OLEDB.Dispose();
            }
            catch (Exception ex)
            {

            }
            Session.Remove("DBClass");
        }
    }
    /// <summary>
    /// 驗證Login
    /// </summary>
    protected void CheckLogin()
    {
        string s = Request.RawUrl;
        string sLogkey = "";

        try
        {
            //sLogkey = Request.Cookies["_logkey"].Value;
            sLogkey = Request.Cookies[_MyCommon.LogKeyName].Value;
        }
        catch { }

        if (sLogkey == "")
        {
            try
            {
                //sLogkey = ViewState["_logkey"].ToString();
                sLogkey = ViewState[_MyCommon.LogKeyName].ToString();
            }
            catch { }
        }
        if (sLogkey == "")
        {
            try
            {
                //sLogkey = Session["_logkey"].ToString();
                sLogkey = Session[_MyCommon.LogKeyName].ToString();
            }
            catch { }

        }
        else
        {
            //Session["_logkey"] = sLogkey;
            Session[_MyCommon.LogKeyName] = sLogkey;
        }
        _MyCommon.User.SessionID = sLogkey;

        if (_MyLogin.CheckLogin())
        {
            //認證成功
        }
        else
        {
        }

        //ViewState["_logkey"] = _MyCommon.User.LogKey;
        ViewState[_MyCommon.LogKeyName] = _MyCommon.User.SessionID;
    }
}
