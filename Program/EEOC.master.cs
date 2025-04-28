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

public partial class EEOC : System.Web.UI.MasterPage
{
    public string TmpLastMenuCode = "";
    public string g_str;
    UserInfo _UserInfo = new UserInfo();
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmdQY = new System.Data.SqlClient.SqlCommand();

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";

        //if (Session["Theme"] != null)
        //    Page.Theme = Session["Theme"].ToString();
        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
        //WUCSysTree1.SkinID = "tVnode";
        //WUCSysTree1.SkinId = "tVnode";
        if (!Application["Domain"].Equals(Request.Url.AbsoluteUri.Replace(Request.RawUrl, "/"))) Application["Domain"] = Request.Url.AbsoluteUri.Replace(Request.RawUrl, "/");
    }

    protected void GetMsg()
    {
        String strMsg = "";
        g_str = "";
        if (_UserInfo.UData.PWDdueDate.CompareTo(DateTime.Today.AddDays(30)) < 0)
        {
            g_str = "<font color=red>注意：您的授權將在" + _UserInfo.SysSet.FormatDate(_UserInfo.UData.PWDdueDate.ToString("yyyy/MM/dd")) + "到期!!</font>";
        }

        strMsg = "<div id='msg' style='overflow:hidden;height:20px;'>" + Application["website_Desc"].ToString() + "</div>";
        if (g_str.Length > 0)
            strMsg += "<div id='msg' style='overflow:hidden;height:20px;'>" + g_str + "</div>";

        try
        {
            string SQL = " SELECT [Message] FROM [UC_Marquee] " + 
            " Where GetDate() Between [StartDate] and [DueDate] " +
            " And (IsNull([SiteId],'')= '' Or '" + _UserInfo.UData.SystemNo + "' like '%'+[SiteId]+'%') " +
            " And IsNull([CompanyCode],'') in ('','" + _UserInfo.UData.CompanyCode + "') " +
            " And IsNull([UserId],'') in ('','" + _UserInfo.UData.UserId + "') " + 
            " Order By [SortNo] ";

            DataTable dt= _MyDBM.ExecuteDataTable(SQL);
            //if (dt.Rows.Count > 0)
            //    strMsg = strMsg.Replace("今日消息：無", "");
            for (int i=0;i< dt.Rows.Count;i++)
            {
                strMsg += "<div id='msg" + i.ToString() + "' style='overflow:hidden;height:20px;'>" + dt.Rows[i][0].ToString() + "</div>";
            }
        }
        catch{}

        ann_box.InnerHtml = strMsg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //將Javascript動態引用進MasterPage中(直接寫在頁面上會有路徑問題)        
        //用於JQuery,例如:JQ日曆
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "1", Page.ResolveUrl("~/Scripts/jquery-1.4.4.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "2", Page.ResolveUrl("~/Scripts/jquery-ui-1.8.7.custom.min.js").ToString());
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "3", Page.ResolveUrl("~/Scripts/ui.datepicker.js").ToString());
        if (_UserInfo.SysSet.GetCalendarSetting().Equals("Y"))
        {//決定是否使用民國年
            Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "4", Page.ResolveUrl("~/Scripts/ui.datepicker.tw.js").ToString());
        }
        //圖片旋轉
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "5", Page.ResolveUrl("~/Scripts/jQueryRotate.js").ToString());
        //用於APPLE效果:會和CR衝造成allInOne.js錯誤
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "6", Page.ResolveUrl("~/Scripts/interface.js").ToString());
        //initDock("");

        //用於一般常用JS
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "A", Page.ResolveUrl("~/Pages/pagefunction.js").ToString());
        //用於執行等待畫面
        Page.ClientScript.RegisterClientScriptInclude(this.GetType().ToString() + "B", Page.ResolveUrl("~/Pages/Busy.js").ToString());        
        
        hid_WebSite.Value = Application["WebSite"].ToString();

        CheckAuthorization1.MsgForeColor = LoginUser.ForeColor;

        if (!Page.IsPostBack)
        {
            //lblName.Text = "用戶：" + Context.User.Identity.Name + "　";
            //lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + ".";
            //跑馬燈
            GetMsg();
            WUCSysTree1.ShowListNumber = false;
            WUCSysTree1.TreeMaxDepth = 6;
            WUCSysTree1.iExpandDepth = 0;
            //BuildSysTreeNode單一系統使用;BuildTreeNode多系統使用
            string[] systemList = _UserInfo.UData.SystemNo.EndsWith(",") ? _UserInfo.UData.SystemNo.Remove(_UserInfo.UData.SystemNo.Length - 1).Split(',') : _UserInfo.UData.SystemNo.Split(',');
            bool showSys = (_UserInfo.UData.SystemNo.Contains("System") ? (systemList.Length > 2) : (systemList.Length > 1));            
            //if (showSys.Equals(true))
            //    WUCSysTree1.iExpandDepth = 1;

            if (Request.QueryString.Count > 0)
            {
                WUCSysTree1.BuildTreeNode(Application["WebSite"].ToString(), _UserInfo.UData.CompanyCode, Context.User.Identity.Name, "", Request.QueryString["A"], showSys);
            }
            else
            {
                WUCSysTree1.BuildTreeNode(Application["WebSite"].ToString(), _UserInfo.UData.CompanyCode, Context.User.Identity.Name, "", "", showSys);
            }
            #region 開始查詢前,先寫入LOG
            DateTime StartDateTime = DateTime.Now;
            string strUrl = Request.Url.ToString().Trim();
            strUrl = (strUrl.IndexOf("/" + Application["WebSite"].ToString() + "/") < 0) ? strUrl : strUrl.Substring(strUrl.IndexOf("/" + Application["WebSite"].ToString() + "/"));
            strUrl = (strUrl.Length > 60) ? strUrl.Remove(60) : strUrl;

            MyCmdQY.Parameters.Clear();
            if (strUrl.Length > 0)
            {
                MyCmdQY.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = strUrl;
                MyCmdQY.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "Q";
                MyCmdQY.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "Browse";
                MyCmdQY.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress;
                MyCmdQY.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                //此時不設定異動結束時間
                //MyCmd.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now.AddYears(-100);
                MyCmdQY.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                _MyDBM.DataChgLog(MyCmdQY.Parameters);
            }
            #endregion
        }
        else
        {
            #region 完成查詢後,更新LOG資訊
            if (MyCmdQY != null)
            {             
                DateTime StartDateTime = DateTime.Now;
                string strUrl = Request.Url.ToString().Trim();
                strUrl = (strUrl.IndexOf("/" + Application["WebSite"].ToString() + "/") < 0) ? strUrl : strUrl.Substring(strUrl.IndexOf("/" + Application["WebSite"].ToString() + "/"));
                strUrl = (strUrl.Length > 60) ? strUrl.Remove(60) : strUrl;
                string strUseParaName = "", strUseParaValue = "";

                if (Request.Params.Count > 0)
                {
                    for (int i = 0; i < Request.Params.Count; i++)
                    {
                        strUseParaName = Request.Params[i].ToString().Trim();
                        if (Request.Form[strUseParaName] != null)
                        {
                            if ((strUseParaName.LastIndexOf("$") + 1) > 0)
                                strUseParaName = strUseParaName.Substring(strUseParaName.LastIndexOf("$") + 1);
                            strUseParaValue += ((i > 0) ? "&" : "") + strUseParaName + "=" + Request.Form[Request.Params[i]].ToString().Trim();
                        }
                        if (i > 20)
                            i = Request.Params.Count;
                    }
                    if (strUseParaValue.Length > 1000)
                        strUseParaValue = strUseParaValue.Remove(1000);
                }

                try
                {
                    if (MyCmdQY.Parameters["@TableName"].Value.ToString().Length > 0)
                    {
                        MyCmdQY.Parameters["@ChangItem"].Value = "Query";
                        MyCmdQY.Parameters["@SQLcommand"].Value = "|" + strUseParaValue;
                        MyCmdQY.Parameters.Add("@ChgStopDateTime", SqlDbType.SmallDateTime).Value = DateTime.Now;
                    }
                }
                catch
                {
                    MyCmdQY.Parameters.Clear();
                    MyCmdQY.Parameters.Add("@TableName", SqlDbType.VarChar, 60).Value = strUrl;
                    MyCmdQY.Parameters.Add("@TrxType", SqlDbType.Char, 1).Value = "Q";
                    MyCmdQY.Parameters.Add("@ChangItem", SqlDbType.VarChar, 255).Value = "Query";
                    MyCmdQY.Parameters.Add("@SQLcommand", SqlDbType.VarChar, 2000).Value = "來源IP:" + Request.UserHostAddress + "|" + strUseParaValue;
                    MyCmdQY.Parameters.Add("@ChgStartDateTime", SqlDbType.SmallDateTime).Value = StartDateTime;
                    MyCmdQY.Parameters.Add("@ChgUser", SqlDbType.VarChar, 32).Value = _UserInfo.UData.UserId;
                }

                if (MyCmdQY.Parameters["@TableName"].Value.ToString().Length > 0)
                    _MyDBM.DataChgLog(MyCmdQY.Parameters);
            }
            #endregion
        }
    }

    protected void initDock(string strSiteId)
    {
        string strScript = "<script type='text/javascript'>" +
        " $(document).ready(" +
        " function () {" +
            //"  $('#" + theDock.ClientID + "').Fisheye(" +
        " $(\"[id$='theDock']\").Fisheye( " +
        "  {" +
        "  maxWidth: 100," +
        "  items: 'a'," +
        "  itemsText: 'span'," +
        "  container: '.dock-container'," +
        "  itemWidth: 80," +
        "  proximity: 80," +
        "  halign: 'center'" +
        "  })}); " +
        " </script>";

        string strSQL = "sp_view_Menu";
        string showSite = strSiteId;
        if (Request.QueryString["A"] != null && Request.QueryString["A"].ToString().IndexOf('.') > 0)
            showSite = Request.QueryString["A"].ToString().Remove(Request.QueryString["A"].ToString().IndexOf('.'));
        else if (showSite.Equals("ERP") || showSite.Equals(""))
            showSite = "ePayroll";

        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 1;
        MyCmd.Parameters.Add("@ls_SiteId", SqlDbType.NVarChar, 16).Value = showSite;
        MyCmd.Parameters.Add("@ls_CompanyCode", SqlDbType.NVarChar, 16).Value = _UserInfo.UData.CompanyCode;
        MyCmd.Parameters.Add("@ls_UserId", SqlDbType.NVarChar, 32).Value = Context.User.Identity.Name;
        MyCmd.Parameters.Add("@ls_LevelProgramId", SqlDbType.NVarChar, 16).Value = "";
        MyCmd.Parameters.Add("@ls_DefaultProgramId", SqlDbType.NVarChar, 16).Value = "";

        DataTable dt = _MyDBM.ExecStoredProcedure(strSQL, MyCmd.Parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            theDock.InnerHtml = "<div class='dock-container'> ";
            string strLink = "", strIcon = "";
            foreach (DataRowView theRow in dt.AsDataView())
            {
                if (theRow["SubMenu"].ToString().Equals("Y"))
                {
                    //找出圖片,若無則使用預設
                    strIcon = theRow["ProgramIcon"].ToString().Trim();
                    if (strIcon.ToLower().Equals("style1"))
                        strIcon = "App_Themes/ui-lightness/images/rss2.png";

                    //找出連結,若無則使用預設
                    strLink = theRow["ProgramPath"].ToString().Trim();
                    if (strLink.Length == 0)
                        strLink = "Main.aspx?A=" + showSite + "." + theRow["ProgramId"].ToString().Trim();
                    
                    strLink = (strLink.ToLower().StartsWith("http") ? "" : Application["Domain"].ToString() + Application["WebSite"].ToString() + "/") + strLink;                    
                    
                    strLink = "<a class='dock-item' href='" + strLink + "'>" +
                        "  <img src='" + Application["Domain"].ToString() + Application["WebSite"].ToString() + "/" + strIcon +
                        "' alt='" + theRow["ProgramDesc"].ToString() + "' /><span>" + theRow["ProgramName"].ToString() + "</span> " +
                        "</a>";
                    theDock.InnerHtml += strLink;
                }
            }
            theDock.InnerHtml += "</div>";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), theDock.ClientID + "Jq", strScript);
    }

    //protected void lbmpchange_Click(object sender, EventArgs e)
    //{
    //    Session["MasterPage"] = "test";
    //    Response.Redirect("~/Calendar.aspx");

    //}

    //protected void btBackHome_Click(object sender, EventArgs e)
    //{        
    //    Response.Redirect("~/Calendar.aspx");
    //}
}
