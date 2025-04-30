using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;

public partial class GLA : System.Web.UI.MasterPage
{
    FormsAuth.Authorization _adAuth = new FormsAuth.Authorization();    
    PanPacificClass.UserData UD;
    UserInfo _UserInfo = new UserInfo();

    #region Property
    /// <summary>
    /// 資料表UC_User資料物件
    /// </summary>
    private UC_UserData userData
    {
        get { return (UC_UserData)ViewState["UC_User"]; }
        set { ViewState["UC_User"] = value; }
    }
    /// <summary>
    /// 資料表UC_User原資料物件
    /// </summary>
    private UC_UserData userDataOld
    {
        get { return (UC_UserData)ViewState["UC_UserOld"]; }
        set { ViewState["UC_UserOld"] = value; }
    }
    /// <summary>
    /// 資料表UC_User的List集合物件
    /// </summary>
    private List<UC_UserData> userDataLT
    {
        get { return (List<UC_UserData>)ViewState["UC_UserDataLT"]; }
        set { ViewState["UC_UserDataLT"] = value; }
    }

    private UC_UserManager _UC_UserManager;
    /// <summary>
    /// 資料表UC_UserM管理物件
    /// </summary>
    private UC_UserManager uc_userManager
    {
        get
        {
            if (_UC_UserManager == null)
            {
                _UC_UserManager = new UC_UserManager();
            }
            return _UC_UserManager;
        }
    }
    private MenuManager _menuManager;
    /// <summary>
    /// 資料表MenuManager管理物件
    /// </summary>
    private MenuManager menuManager
    {
        get
        {
            if (_menuManager == null)
            {
                _menuManager = new MenuManager();
            }
            return _menuManager;
        }
    }
    private MyShortcutManager _myShortcutManager;
    /// <summary>
    /// 資料表MyShortcutManager管理物件
    /// </summary>
    private MyShortcutManager myShortcutManager
    {
        get
        {
            if (_myShortcutManager == null)
            {
                _myShortcutManager = new MyShortcutManager();
            }
            return _myShortcutManager;
        }
    }
    /// <summary>
    /// 選單資訊List
    /// </summary>
    private List<MenuData> menuDataLT
    {
        get { return (List<MenuData>)ViewState["MenuDataLT"]; }
        set { ViewState["MenuDataLT"] = value; }
    }
    /// <summary>
    /// 側邊選單資訊List
    /// </summary>
    private List<MenuData> sideMenuDataLT
    {
        get { return (List<MenuData>)ViewState["SideMenuDataLT"]; }
        set { ViewState["SideMenuDataLT"] = value; }
    }
    /// <summary>
    /// 上方導覽列資訊List
    /// </summary>
    private List<MenuData> topNavDataLT
    {
        get { return (List<MenuData>)ViewState["TopNavDataLT"]; }
        set { ViewState["TopNavDataLT"] = value; }
    }
    /// <summary>
    /// 上方次導覽列資訊List
    /// </summary>
    //private List<MenuData> subNavDataLT
    //{
    //    get { return (List<MenuData>)ViewState["SubNavDataLT"]; }
    //    set { ViewState["SubNavDataLT"] = value; }
    //}
    /// <summary>
    /// 選單資訊
    /// </summary>
    private MenuData menuData
    {
        get { return (MenuData)ViewState["MenuData"]; }
        set { ViewState["MenuData"] = value; }
    }
    /// <summary>
    /// 側邊選單資訊
    /// </summary>
    private MenuData sideMenuData
    {
        get { return (MenuData)ViewState["SideMenuData"]; }
        set { ViewState["SideMenuData"] = value; }
    }
    /// <summary>
    /// 上方導覽列資訊
    /// </summary>
    private MenuData topNavData
    {
        get { return (MenuData)ViewState["TopNavData"]; }
        set { ViewState["TopNavData"] = value; }
    }

    /// <summary>
    /// 取得一個唯一值
    /// </summary>
    private static string guid = System.Guid.NewGuid().ToString().Replace("-", "_");

    private static string SideMenuIndex;
    private static string TopNavIndex;
    private static string SystemCode;
    private static string MenuSide;
    private static string MenuTop;
    private static string MenuSub;
    
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (_UserInfo.UData != null && !string.IsNullOrEmpty(_UserInfo.UData.UserId))
            UD = _UserInfo.UData;
        if (UD == null || string.IsNullOrEmpty(UD.SessionID) || string.IsNullOrEmpty(UD.UserId))
            UD = _adAuth.UData(Session.SessionID);
        if (string.IsNullOrEmpty(UD.SessionID) || string.IsNullOrEmpty(UD.UserId))
        {
            //修正設定Cookie後.Menu無法顯示之問題
            UserInfo CookieUD = new UserInfo();

            //UD = CookieUD.UData;
            UD.SessionID = CookieUD.UData.SessionID;
            UD.Company = CookieUD.UData.Company;
            UD.EmployeeId = CookieUD.UData.EmployeeId;
            UD.CompanyName = CookieUD.UData.CompanyName;
            UD.UserId = CookieUD.UData.UserId;
        }
        Session["LoginUserId"] = UD.UserId;
        //查詢用預設值
        SearchData SD = new SearchData();
        Session["Company"] = SD.Company;
        if (!IsPostBack)
        {
            if (UD == null || string.IsNullOrEmpty(UD.SessionID) || string.IsNullOrEmpty(UD.UserId))
            {
                Response.Redirect("~/AuthAD.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            }
            else
            {
                txtUser.Text = UD.UserName;
            }
        }

        if (Request.Form[Stool.EventArgument] == "SelectLinkUserData")
        {
            string arg = Request.Form[Stool.EventTarget];
            if (!string.IsNullOrEmpty(arg))
            {
                this.doSetData(arg);
            }
        }
    }
    private void doSetData(string arg)
    {
        arg = arg.Substring(0, arg.Length);
        string[] strEmpId = arg.Split('＠');
        hfUserId.Value = strEmpId[1].Trim();
        hfUserName.Value = strEmpId[0].Trim();
        hfLoginAccount.Value = strEmpId[2].Trim();
        if (hfLoginAccount.Value != null)
        {
            ChangeUser(hfUserId.Value, hfUserName.Value, hfLoginAccount.Value);

            Session["UserId"] = hfUserId.Value;
            Session["LoginUserId"] = hfLoginAccount.Value;
            txtUser.Text = hfUserName.Value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());

        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }        
        //用於執行等待畫面
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());    
        
        if (!IsPostBack)
        {
            InitMenuData();
            RegisterLeftMenu();
            RegisterTopMenu(menuData.ProgId, menuData.location);
        }
    }
    private void InitMenuData()
    {
        menuDataLT = new List<MenuData>();
        menuDataLT = menuManager.CreateLeftMenuLT();
        menuData = new MenuData();
        menuData.ParentProgId = "GLATEST";
        menuData.ProgId = "GLA01";
        menuData.location = "Top1";
        if (Request.QueryString["ProgId"]!=null && !string.IsNullOrEmpty(Request.QueryString["ProgId"].ToString()))
        {
            menuData.ProgId = Request.QueryString["ProgId"].ToString();
        }
    }

    private void ChangeUser(string name, string id, string loginaccount)
    {
        int i;
        string groups = "";
        groups = _adAuth.GetAuthorization(ConfigurationManager.AppSettings["CompanyID"].ToString(), loginaccount, "", Request.UserHostAddress, Session.SessionID, true);

        string url = "";
        i = groups.IndexOf("|");
        if (i > 0)
        {
            int tmpMinutes = 60;
            tmpMinutes = tmpMinutes * 24;

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                        loginaccount, DateTime.Now, DateTime.Now.AddMinutes(tmpMinutes), true, groups);

            //Encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            //Create a cookie, and then add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            authCookie.Expires = authTicket.Expiration;

            //Add the cookie to the outgoing cookies collection.
            Response.Cookies.Add(authCookie);


            //You can redirect now.
            //登入成功導到主頁
            url = Request.Url.ToString().Contains("ReturnUrl=") ? Request["ReturnUrl"].ToString() : System.Web.Security.FormsAuthentication.DefaultUrl;
            Response.Redirect(url);
            //換人後導向行事例頁
            //Response.Redirect("~/LAM/LeaveMasterView.aspx");
        }
    }

    protected void lbLogout_Click(object sender, EventArgs e)
    {
        //移除登入資訊
        Session.Remove("LoginUserId");
        Response.Redirect("~/AuthAD.aspx");
    }

    private void RegisterLeftMenu()
    {
        List<string> lst = new List<string>();
        if (menuDataLT != null && menuDataLT.Count > 0)
        {
            List<MenuData> datas = new List<MenuData>();
            datas = menuDataLT.FindAll(a => (a.ParentProgId == menuData.ParentProgId && a.location == "Left"));
            datas.Sort(delegate(MenuData P1, MenuData P2)
            {
                return P1.ProgSort.CompareTo(P2.ProgSort);
            });
            if (datas != null && datas.Count > 0)
            {
                foreach (MenuData data in datas)
                {
                    //按下左邊MENU  Top與Sub回到"1"
                    data.ProgUrl += "&Side=" + data.ProgSort + "&Top=1&Sub=1";
                    lst.Add("<div class=\"menunokids\">");
                    lst.Add("<a href=\"" + data.ProgUrl + "\" title=\"" + data.ProgDesc + "\" " + data.ProgHtml + ">" + data.ProgName + "</a>");
                    lst.Add("</div>");
                }
            }
        }

        this.ltlLeftMenu.Text = Stool.CombineString(lst, "");
    }
    private void RegisterTopMenu(string parentid, string location)
    {
        List<string> lst = new List<string>();
        if (menuDataLT != null && menuDataLT.Count > 0)
        {
            List<MenuData> datas = new List<MenuData>();
            datas = menuDataLT.FindAll(a => (a.ParentProgId == parentid && a.location == location));
            datas.Sort(delegate(MenuData P1, MenuData P2)
            {
                return P1.ProgSort.CompareTo(P2.ProgSort);
            });
            if (datas != null && datas.Count > 0)
            {
                lst.Add("<ul id=\"topnav\">");
                foreach (MenuData data in datas)
                {
                    if (data.ProgMode == "P")
                    {
                        lst.Add("<li>");
                        lst.Add("<label>");
                        lst.Add("<a href=\"" + data.ProgUrl + "&Side=" + MenuSide + "&Top=" + data.ProgSort + "&Sub=1\" title=\"" + data.ProgDesc + "\" " + data.ProgHtml + ">" + data.ProgName + "</a>");
                        lst.Add("<span></span>");
                        lst.Add("</li>");
                        lst.Add("</label>");
                    }
                    else
                    {
                        MenuSide = menuDataLT.Find(a => (a.ProgId == data.ParentProgId)).ProgSort.ToString();
                        lst.Add("<li>");
                        lst.Add("<label>" + data.ProgName + "</label>");
                        lst.Add("<span class=\"subnav\">");
                        lst.Add(RegisterSubMenu(data.ProgId, "Sub"));
                        lst.Add("</span>");
                        lst.Add("</li>");
                    }
                }
                lst.Add("</ul>");
            }
        }

        this.ltlTop1Menu.Text = Stool.CombineString(lst, "");
    }
    private string RegisterSubMenu(string parentid, string location)
    {
        List<string> lst = new List<string>();
        if (menuDataLT != null && menuDataLT.Count > 0)
        {
            List<MenuData> datas = new List<MenuData>();
            datas = menuDataLT.FindAll(a => (a.ParentProgId == parentid && a.location == location));
            datas.Sort(delegate(MenuData P1, MenuData P2)
            {
                return P1.ProgSort.CompareTo(P2.ProgSort);
            });
            if (datas != null && datas.Count > 0)
            {
                foreach (MenuData data in datas)
                {
                    MenuTop = menuDataLT.Find(a => (a.ProgId == data.ParentProgId)).ProgSort.ToString();
                    lst.Add("<a href=\"" + data.ProgUrl + "&Side=" + MenuSide + "&Top=" + MenuTop + "&Sub=" + data.ProgSort + "\" title=\"" + data.ProgDesc + "\">" + data.ProgName + "</a>");
                }
            }
        }

        return Stool.CombineString(lst, " | ");
    }
    protected void lbhome_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://ibosdvlp/WebSite/Systems.aspx", true);
    }
}
