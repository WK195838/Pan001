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

public partial class Main : System.Web.UI.Page
{
    #region 變數宣告

    UserInfo _UserInfo = new UserInfo();
    DBManger _MyDBM;
    System.Data.SqlClient.SqlCommand MyCmdQY = new System.Data.SqlClient.SqlCommand();

    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _MyDBM = new DBManger();
        _MyDBM.New();
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //Page.Theme = "Theme_09";
        if (Session["Theme"] != null)
            Page.Theme = Session["Theme"].ToString();

        if (Session["MasterPage"] != null)
            Page.MasterPageFile = "~/" + Session["MasterPage"].ToString() + ".master";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Title = Application["website_Title"].ToString();
        string theMenu = "", tmp = "";        
        if (Request.QueryString["A"] != null && Request.QueryString["A"].ToString().Contains("."))
        {
            string[] theItem = Request.QueryString["A"].ToString().Split('.');            
            theMenu = theItem[0];
            tmp = theItem[1];
        }        
        //WUCSysTree1.BuildSysTreeNode(theMenu, _UserInfo.UData.Company.Trim(), _UserInfo.UData.UserId.Trim(), tmp);
        initMap(theMenu, tmp);
    }

    protected void initMap(string strSiteId, string theMenu)
    {
        string strScript = "";
        //3D旋轉 
        #region
        strScript = "<script type='text/javascript'>" +
        " $(document).ready(function () { " +
        " var element = $('#" + the3DList.ClientID + " a');" +
        " var elementImg = $('#" + the3DList.ClientID + " img');" +
        " var offset = 0;" +
        " var stepping = 0.03;" +
        " var list = $('#" + the3DList.ClientID + "');" +
        " var $list = $(list);" +
        " $list.mousemove(function (e) {" +
        "    var LeftOfList = $list.eq(0).offset().left;" +
        "    var listWidth = $list.width();" +
        "    stepping = (e.clientX - LeftOfList) / listWidth * 0.2 - 0.1;" +
        "    });" +
        " for (var i = element.length - 1; i >= 0; i--) {" +
        "     element[i].elemAngle = i * Math.PI * 2 / element.length;" +
        "     }" +
        " setInterval(render, 1200);" +
        " function render() {" +
        "     for (var i = element.length - 1; i >= 0; i--) {" +
        "        var angle = element[i].elemAngle + offset;" +
        "        x = 40 + Math.cos(angle) * 40;" +
        "        y = 45 + Math.sin(angle) * 50;" +
        "        size = Math.round(Math.sin(angle) * 40) + 50;" +
        "        var elementCenter = $(element[i]).width() / 2;" +
        "        var elementCenterH = $(element[i]).height() / 2;" +
        "        var leftValue = x + '%';" +
        "        var topValue = (($list.height() / 2) * y / 100 + elementCenterH) + 'px';" +
        //"        $(element[i]).css('fontSize', (size - 50)/5 + 'pt');" +
        "        $(element[i]).css('opacity', size / 10);" +
        "        $(element[i]).css('zIndex', size);" +
        "        $(element[i]).css('left', leftValue);" +
        "        $(element[i]).css('top', topValue);" +
        "        $(elementImg[i]).css('opacity', size / 20);" +
        "        $(elementImg[i]).css('zIndex', size);" +
        "        $(elementImg[i]).css('left', leftValue);" +
        "        $(elementImg[i]).css('top', topValue);" +
        "        }" +
        "     offset += stepping;" +
        "     }    });" +
        " </script>";
        #endregion

        string strSQL = "sp_view_Menu";
        string showSite = strSiteId;
        if (Request.QueryString["A"] != null && Request.QueryString["A"].ToString().IndexOf('.') > 0)
            showSite = Request.QueryString["A"].ToString().Remove(Request.QueryString["A"].ToString().IndexOf('.'));
        else if (showSite.Equals("ERP") || showSite.Equals(""))
            showSite = "ePayroll";

        System.Data.SqlClient.SqlCommand MyCmd = new System.Data.SqlClient.SqlCommand();
        MyCmd.Parameters.Add("@ls_level", SqlDbType.Int).Value = 2;
        MyCmd.Parameters.Add("@ls_SiteId", SqlDbType.NVarChar, 16).Value = showSite;
        MyCmd.Parameters.Add("@ls_CompanyCode", SqlDbType.NVarChar, 16).Value = _UserInfo.UData.CompanyCode;
        MyCmd.Parameters.Add("@ls_UserId", SqlDbType.NVarChar, 32).Value = Context.User.Identity.Name;
        MyCmd.Parameters.Add("@ls_LevelProgramId", SqlDbType.NVarChar, 16).Value = theMenu;
        MyCmd.Parameters.Add("@ls_DefaultProgramId", SqlDbType.NVarChar, 16).Value = "";

        DataTable dt = _MyDBM.ExecStoredProcedure(strSQL, MyCmd.Parameters);
        if (dt != null && dt.Rows.Count > 0)
        {            
            #region 3D旋轉
            the3DList.Style.Clear();
            the3DList.Style.Value = "margin:0 auto;height:600px;width:800px;overflow:hidden;position:relative;";
            the3DList.InnerHtml = "<ul style='list-style:none;margin:0;padding:0;'> ";
            string strLink = "", strIcon = "";
            int iCount = 0;
            foreach (DataRowView theRow in dt.AsDataView())
            {
                iCount++;
                //if (theRow["SubMenu"].ToString().Equals("Y"))
                {
                    //找出圖片,若無則使用預設
                    strIcon = theRow["ProgramIcon"].ToString().Trim();
                    if (strIcon.ToLower().Equals("style1"))
                        strIcon = "App_Themes/images/bee0" + (iCount % 7 + 1) + ".gif";

                    //找出連結,若無則使用預設
                    strLink = theRow["ProgramPath"].ToString().Trim();
                    if (strLink.Length == 0)
                        strLink = "Main.aspx?A=" + showSite + "." + theRow["ProgramId"].ToString().Trim();

                    strLink = (strLink.ToLower().StartsWith("http") ? "" : Application["Domain"].ToString() + Application["WebSite"].ToString() + "/") + strLink;

                    strLink = "<li style='list-style:none;margin:0;padding:0;'><a  style='position:absolute;text-decoration: none;color:#666;' href='" + strLink + "'>" +
                        "  <img src='" + Application["Domain"].ToString() + Application["WebSite"].ToString() + "/" + strIcon +
                        "' alt='" + theRow["ProgramDesc"].ToString() + "' /><p>" + theRow["ProgramName"].ToString() + "</p>" +
                        "</a></li>";
                    the3DList.InnerHtml += strLink;
                }
            }
            the3DList.InnerHtml += "</ul>";
            #endregion
                        
            #region 矩陣列圖
            theMap.InnerHtml = "<div class=''> ";
            strLink = "";
            strIcon = "";
            iCount = 0;
            foreach (DataRowView theRow in dt.AsDataView())
            {
                iCount++;
                //if (theRow["SubMenu"].ToString().Equals("Y"))
                {
                    //找出圖片,若無則使用預設
                    strIcon = theRow["ProgramIcon"].ToString().Trim();
                    if (strIcon.ToLower().Equals("style1"))
                        strIcon = "App_Themes/images/bee0" + (iCount % 7 + 1) + ".gif";

                    //找出連結,若無則使用預設
                    strLink = theRow["ProgramPath"].ToString().Trim();
                    if (strLink.Length == 0)
                        strLink = "Main.aspx?A=" + showSite + "." + theRow["ProgramId"].ToString().Trim();

                    strLink = (strLink.ToLower().StartsWith("http") ? "" : Application["Domain"].ToString() + Application["WebSite"].ToString() + "/") + strLink;

                    strLink = "<a class='' href='" + strLink + "'>" +
                        "  <img src='" + Application["Domain"].ToString() + Application["WebSite"].ToString() + "/" + strIcon +
                        "' alt='" + theRow["ProgramDesc"].ToString() + "' /><span>" + theRow["ProgramName"].ToString() + "</span> " +
                        "</a>";
                    theMap.InnerHtml += strLink;
                }
            }
            theMap.InnerHtml += "</div>";
            #endregion
        }
        //先拿掉3D旋轉的特效及所佔空間
        the3DList.Style.Clear();
        the3DList.InnerHtml = "";
        Page.ClientScript.RegisterStartupScript(this.GetType(), theMap.ClientID + "Jq3D", strScript);
    }
}
