using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// 頁面的處理方法，是PostBack或AJAX
/// </summary>
public enum PageType
{
    PostBack, AJAX
}
/// <summary>
/// 其它常用的工具
/// </summary>
public static class Stool
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly string PageBind = "pageBind";
    /// <summary>
    /// 穩藏欄位值，__EVENTTARGET
    /// </summary>
    public static readonly string EventTarget = "__EVENTTARGET";
    /// <summary>
    /// 穩藏欄位值，__EVENTARGUMENT
    /// </summary>
    public static readonly string EventArgument = "__EVENTARGUMENT";
    /// <summary>
    /// 穩藏欄位值，ctl
    /// </summary>
    public static readonly string HiddenFieldName = "ctl";
    /// <summary>
    /// 穩藏欄位值，WindowConfirm
    /// </summary>
    public static readonly string WindowConfirm = "WindowConfirm";
    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="attachScrpt">要附加的Script</param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, string attachScrpt,
                                      string fieldValue, bool isWindowClose)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (!string.IsNullOrEmpty(attachScrpt))
        {
            sb.AppendLine(attachScrpt);
        }
        sb.AppendLine("if (window.opener) { ");
        if (pageType == PageType.PostBack)
        {
            sb.AppendFormat("  window.opener.document.getElementById('__EVENTARGUMENT').value = '{0}';", fieldValue).AppendLine();
            sb.AppendLine("  window.opener.document.getElementById('__EVENTTARGET').value = '';");
            sb.AppendLine("  window.opener.document.forms[0].submit();");
        }
        else
        {
            sb.AppendLine("var divId = findId();");
            sb.AppendFormat("window.opener.__doPostBack(divId,'{0}');", fieldValue).AppendLine();
        }
        sb.AppendLine("}");
        if (isWindowClose)
        {
            sb.AppendLine("window.close();");
        }
        sb.AppendLine("function findId(){");
        sb.AppendLine("	var divs = window.opener.document.getElementsByTagName('div');");
        sb.AppendLine("	var divId= '';");
        sb.AppendLine("	for(var i = 0 ;i < divs.length; i++){");
        sb.AppendLine("		var dv = divs[i];");
        sb.AppendLine("		if(dv.id.indexOf('UpdatePanel') >=0){");
        sb.AppendLine("			divId = dv.id;");
        sb.AppendLine("			break;");
        sb.AppendLine("		}");
        sb.AppendLine("	}");
        sb.AppendLine("	return divId;");
        sb.AppendLine("}");
        if (pageType == PageType.PostBack)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "Clsoe", sb.ToString(), true);
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), "close", sb.ToString(), true);
        }
    }
    /// <summary>
    ///  註冊關閉視窗並會產生PostBack的javascript，穩藏欄位的值為pageBind，子視窗為關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    public static void RegisterScript(PageType pageType, Page page)
    {
        RegisterScript(pageType, page, string.Empty, "pageBind", true);
    }
    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，穩藏欄位的值為pageBind
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, bool isWindowClose)
    {
        RegisterScript(pageType, page, string.Empty, "pageBind", isWindowClose);
    }
    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，子視窗關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    public static void RegisterScript(PageType pageType, Page page, string fieldValue)
    {
        RegisterScript(pageType, page, string.Empty, fieldValue, true);
    }
    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，子視窗為關閉
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="attachScrpt">要附加的Script</param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    public static void RegisterScript(PageType pageType, Page page, string attachScrpt, string fieldValue)
    {
        RegisterScript(pageType, page, attachScrpt, fieldValue, true);
    }
    /// <summary>
    /// 註冊關閉視窗並會產生PostBack的javascript，無附加Script
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="fieldValue">穩藏欄位的值</param>
    /// <param name="isWindowClose">子視窗是否關閉</param>
    public static void RegisterScript(PageType pageType, Page page, string fieldValue, bool isWindowClose)
    {
        RegisterScript(pageType, page, string.Empty, fieldValue, isWindowClose);
    }
    /// <summary>
    ///註冊一警告視窗，以作顯示訊息用(樣式套用主題)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    public static void ShowThemeMessage(PageType pageType, Page page, string message)
    {
        ShowThemeMessage(pageType, page, message, string.Empty);
    }
    /// <summary>
    /// 註冊一警告視窗，以作顯示訊息用，並轉至另一網址(樣式套用主題)
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">訊息</param>
    /// <param name="url">網址</param>
    public static void ShowThemeMessage(PageType pageType, Page page, string message, string url)
    {
        message = message.Replace("'", "");
        message = message.Replace("\r\n", "\\n");
        message = message.Replace("<br/>", "\\n");
        string js = "$.messageBox.show('" + message + "');";
        if (!string.IsNullOrEmpty(url))
        {
            //url = url.Replace("~", HttpRuntime.AppDomainAppVirtualPath);
            js += string.Format("__doPostBack('prem', '{0}');", url);
        }
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowMessage001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "ShowMessage001", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    ///註冊一警告視窗，以作顯示訊息用
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    public static void ShowMessage(PageType pageType, Page page, string message)
    {
        ShowMessage(pageType, page, message, string.Empty);
    }
    /// <summary>
    /// 註冊一警告視窗，以作顯示訊息用，並轉至另一網址
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">訊息</param>
    /// <param name="url">網址</param>
    public static void ShowMessage(PageType pageType, Page page, string message, string url)
    {
        message = message.Replace("'", "");
        message = message.Replace("\r\n", "\\n");
        message = message.Replace("<br/>", "\\n");
        string js = "window.alert('" + message + "');";
        if (!string.IsNullOrEmpty(url))
        {
            //url = url.Replace("~", HttpRuntime.AppDomainAppVirtualPath);
            js += string.Format("__doPostBack('prem', '{0}');", url);
        }
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowMessage001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "ShowMessage001", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊一確認視窗，按確認後，並PostBack，欄位名稱為ctl
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message"></param>
    public static void ShowConfirm(PageType pageType, Page page, string message)
    {
        message = message.Replace("\r\n", "\\n");
        Stool.InitFunction(pageType, page);
        string js = string.Format("confirmData('{0}');", message);
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "confirm001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "confirm001", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊一確認視窗，按確認後，並PostBack，欄位名稱為WindowConfirm
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">需要顯示的訊息</param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    public static void ShowConfirm(PageType pageType, Page page, string message, bool isPostBackOnCancel)
    {
        ShowConfirm(pageType, page, message, "y", isPostBackOnCancel);
    }
    /// <summary>
    /// 註冊一確認視窗，按確認後，並PostBack，欄位名稱為WindowConfirm
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">需要顯示的訊息</param>
    /// <param name="definedValue">自定義的值</param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    public static void ShowConfirm(PageType pageType, Page page, string message, string definedValue, bool isPostBackOnCancel)
    {
        message = message.Replace("\r\n", "\\n");
        Stool.InitFunction(pageType, page, definedValue, isPostBackOnCancel);
        string js = string.Format("confirmData('{0}');", message);
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "confirm001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "confirm001", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊一確認視窗，按確認後，並PostBack，欄位名稱為WindowConfirm，可顯示阻擋DIV
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="message">需要顯示的訊息</param>
    /// <param name="definedValue">自定義的值</param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    /// <param name="isShowDiv">是否在PostBack時，顯示阻擋點擊的Div</param>
    public static void ShowConfirm(PageType pageType, Page page, string message, string definedValue, bool isPostBackOnCancel, bool isShowDiv)
    {
        message = message.Replace("\r\n", "\\n");
        Stool.InitFunction(pageType, page, definedValue, isPostBackOnCancel, isShowDiv);
        string js = string.Format("confirmData('{0}');", message);
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "confirm001", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "confirm001", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊window.confirm的javascript
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    private static void InitFunction(PageType pageType, Page page)
    {
        string js = @"function confirmData(message)
                    {
	                    var field = document.getElementById('ctl');
	                    if(window.confirm(message))
	                    {
		                    field.value = 'y';
		                    document.forms[0].submit();
	                    }
                    }";
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterHiddenField("ctl", string.Empty);
                page.ClientScript.RegisterStartupScript(page.GetType(), "hiddenFielD", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterHiddenField(page, "ctl", string.Empty);
                ScriptManager.RegisterStartupScript(page, page.GetType(), "hiddenFielD", js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊window.confirm的javascript
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="definedValue">自定義的值</param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    private static void InitFunction(PageType pageType, Page page, string definedValue, bool isPostBackOnCancel)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("function confirmData(message)");
        sb.AppendLine("{");
        sb.AppendLine("	var field = document.getElementById('WindowConfirm');");
        sb.AppendLine("	if(window.confirm(message))");
        sb.AppendLine("	{");
        sb.AppendFormat("		field.value = '{0}';", definedValue).AppendLine();

        if (!isPostBackOnCancel)
        {
            sb.AppendLine("		document.forms[0].submit();");
        }

        sb.AppendLine("	}");

        if (isPostBackOnCancel)
        {
            sb.AppendLine("else{");
            sb.AppendLine("		field.value = 'n';");
            sb.AppendLine("}");
            sb.AppendLine("		document.forms[0].submit();");
        }
        sb.AppendLine("}");

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterHiddenField("WindowConfirm", string.Empty);
                page.ClientScript.RegisterStartupScript(page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterHiddenField(page, "WindowConfirm", string.Empty);
                ScriptManager.RegisterStartupScript(page, page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 註冊window.confirm的javascript
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    /// <param name="definedValue">自定義的值</param>
    /// <param name="isPostBackOnCancel">是否在按下取消時，產生PostBack</param>
    /// <param name="isShowDiv">是否在PostBack時，顯示阻擋點擊的Div</param>
    private static void InitFunction(PageType pageType, Page page, string definedValue, bool isPostBackOnCancel, bool isShowDiv)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("function confirmData(message)");
        sb.AppendLine("{");
        sb.AppendLine("	var field = document.getElementById('WindowConfirm');");
        sb.AppendLine("	if(window.confirm(message))");
        sb.AppendLine("	{");
        sb.AppendFormat("	field.value = '{0}';", definedValue).AppendLine();
        if (isShowDiv) sb.AppendLine("$.blockUI({message:null});");
        if (!isPostBackOnCancel) sb.AppendLine("	document.forms[0].submit();");
        sb.AppendLine("	}");

        if (isPostBackOnCancel)
        {
            sb.AppendLine("else{");
            if (isShowDiv) sb.AppendLine("$.blockUI({message:null});");
            sb.AppendLine("		field.value = 'n';");
            sb.AppendLine("}");
            sb.AppendLine("		document.forms[0].submit();");
        }
        sb.AppendLine("}");

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterHiddenField("WindowConfirm", string.Empty);
                page.ClientScript.RegisterStartupScript(page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterHiddenField(page, "WindowConfirm", string.Empty);
                ScriptManager.RegisterStartupScript(page, page.GetType(), "hiddenFielD", sb.ToString(), true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 用分隔符號(tag)來組合字串
    /// </summary>
    /// <param name="IdList">要組合的字串陣列</param>
    /// <param name="tag">分隔符號</param>
    /// <returns></returns>
    public static string CombineString(string[] IdList, string tag)
    {
        string ret = "";
        for (int i = 0; i <= IdList.GetUpperBound(0); i++)
        {
            if (i == IdList.GetUpperBound(0))
            {
                ret += IdList[i];
            }
            else
            {
                ret += IdList[i] + tag;
            }
        }
        return ret;
    }
    /// <summary>
    /// 用分隔符號(tag)來組合字串
    /// </summary>
    /// <param name="IdList"></param>
    /// <param name="tag">分隔符號</param>
    /// <returns></returns>
    public static string CombineString(List<string> IdList, string tag)
    {
        if (IdList.Count > 0)
        {
            return Stool.CombineString(IdList.ToArray(), tag);
        }
        else
        {
            return string.Empty;
        }
    }
    /// <summary>
    /// 光棒效果
    /// </summary>
    /// <param name="gv"></param>
    public static void ChangeGridviewOnMouseOverColor(GridView gv)
    {
        foreach (GridViewRow row in gv.Rows)
        {
            row.Attributes["onmouseover"] = "this.className='gvSelectedStyle';";
            switch (row.RowState)
            {
                case DataControlRowState.Alternate:
                    row.Attributes["onmouseout"] = "this.className='gvAlStyle';";
                    break;

                case DataControlRowState.Normal:
                    row.Attributes["onmouseout"] = "this.className='gvRowStyle';";
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 去空白
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Ctrim(object value)
    {
        string ret = value.ToString().Trim();
        if (ret.Length != 0)
        {
            return ret;
        }
        return null;
    }
    /// <summary>
    /// 遞迴尋找指定的控制項
    /// </summary>
    /// <param name="id">控制項的ID</param>
    /// <param name="parentControl"></param>
    /// <param name="type">控制項的形別</param>
    /// <returns></returns>
    public static Control RecurionFindControl(string id, Control parentControl, Type type)
    {
        Control retCon = null;
        retCon = parentControl.FindControl(id);
        if (retCon == null)
        {
            foreach (Control con in parentControl.Controls)
            {
                retCon = Stool.RecurionFindControl(id, con, type);
                if (retCon != null)
                {
                    if (retCon.GetType().ToString() == type.ToString())
                    {
                        break;
                    }
                }
            }
        }

        return retCon;
    }
    /// <summary>
    /// Base64的編碼
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Base64Encode(string str)
    {
        byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(encbuff);
    }
    /// <summary>
    /// Base64的解碼
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Base64Decode(string str)
    {
        byte[] decbuff = Convert.FromBase64String(str);
        return System.Text.Encoding.UTF8.GetString(decbuff);
    }
    #region"拿時間"
    public static string GetYear(string value)
    {
        string year;
        year = value.Substring(0, 4);
        return year;
    }
    public static string GetMonth(string value)
    {
        string month;
        month = value.Substring(5, 2);
        return month;
    }
    public static string GetDay(string value)
    {
        string day;
        day = value.Substring(8, 2);
        return day;
    }
    public static string GetHour(string value)
    {
        string hour;
        hour = value.Substring(0, 2);
        return hour;
    }
    public static string GetMin(string value)
    {
        string min;
        min = value.Substring(3, 2);
        return min;
    }
    public static string GetSec(string value)
    {
        string sec;
        sec = value.Substring(6, 2);
        return sec;
    }
    #endregion
    /// <summary>
    /// 註冊JQuery:在該ClientID下所有同階層的CheckBox單選
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="clientId">控制項的ClientID</param>
    public static void JQuery_CheckBoxOnlyOne(PageType pageType, Page page, string clientID)
    {
        string js;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine(" $(function() {");
        sb.AppendLine(" $('#" + clientID + " :checkbox').bind('click',function(){");
        sb.AppendLine(" $(this).siblings(':checked').attr('checked',false);");
        sb.AppendLine(" })");
        sb.AppendLine(" });");
        js = sb.ToString();

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "JQueryCheckBox001_" + clientID, js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "JQueryCheckBox001_" + clientID, js, true);
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 註冊JQuery:在該ClientID下所有同階層的CheckBox單選
    /// </summary>
    /// <param name="pageType">頁面的處理方法，是PostBack或AJAX</param>
    /// <param name="page"></param>
    /// <param name="clientId">GridView的ClientID</param>
    public static void JQuery_GridviewCheckBoxOnlyOne(PageType pageType, Page page, string clientID)
    {
        string js;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine(" $(function() {");
        sb.AppendLine("   $('#" + clientID + " input[type=checkbox]').bind('click',function(){");
        sb.AppendLine("     $('#" + clientID + " >tbody >tr >td >input:checkbox').attr('checked', false);");
        sb.AppendLine("     $(this).attr('checked', true);");
        sb.AppendLine("   })");
        sb.AppendLine(" });");
        js = sb.ToString();

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "JQueryCheckBox002_" + clientID, js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "JQueryCheckBox002_" + clientID, js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 抑制滑鼠右鍵
    /// </summary>
    /// <param name="pageType"></param>
    /// <param name="page"></param>
    public static void JQuery_NoMouseRightClick(PageType pageType, Page page)
    {
        string js;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine(" $(function() {");
        sb.AppendLine("   $(this).bind('contextmenu', function(e) {");
        sb.AppendLine("     alert('請勿按滑鼠右鍵，以免造成系統異常');");
        sb.AppendLine("     e.preventDefault();");
        sb.AppendLine("   })");
        sb.AppendLine(" });");
        js = sb.ToString();

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "JQueryNoMouseRightClick", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "JQueryNoMouseRightClick", js, true);
                break;
            default:
                break;
        }
    }
    public static string RegisterJQueryBlockUI(Page page, PageType pageType)
    {
        string js = String.Empty;
        //頁面執行 Submit 時顯示 blockUI 同時訊息視窗可移動
        js += "$(function () {";
        js += "    $('#doWait').draggable();";
        js += "});";
        //js += "$.blockUI({ message: $('#doWait'),";
        js += "$.blockUI({ message: $('#doWait'),";
        js += "     overlayCSS: {";
        js += "         backgroundColor: '#000',";
        js += "         opacity: '0.3'";
        js += "     },";
        js += "     css: {";
        js += "         opacity: '0.6',";
        js += "         top: ($(window).height() - 70) / 2 + 'px',";
        js += "         left: ($(window).width() - 360) / 2 + 'px',";
        js += "         width: '0px',";
        js += "         height: '0px',";
        js += "         cursor: 'pointer'";
        js += "     }";
        js += "});";
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterOnSubmitStatement(page.GetType(), "blockUI", js);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterOnSubmitStatement(page, page.GetType(), "blockUI", js);
                break;
            default:
                break;
        }
        //頁面載入時隱藏 blockUI
        js = "$.unblockUI();";
        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "unblockUI", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "unblockUI", js, true);
                break;
            default:
                break;
        }
        //繪製等待視窗
        string rootDir = System.Web.HttpContext.Current.Request.ApplicationPath;
        string imgStr = rootDir + "/" + "images/loading.gif";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("<div id='doWait' style='display:none; text-align:center; border:1px solid #000; width:360px; height:70px; background-color:#fff;'>");
        sb.AppendLine("<table style='font-weight:bold;'>");
        sb.AppendLine(" <tr>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp; &nbsp; &nbsp; </td>");
        sb.AppendLine("    <td rowspan='3' style='vertical-align:middle;'>");
        sb.AppendLine("        &nbsp;<img src='" + imgStr + "' alt='' />&nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine(" </tr>");
        sb.AppendLine(" <tr>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp; 網頁資料載入中．請稍待 ．．．</td>");
        sb.AppendLine(" </tr>");
        sb.AppendLine(" <tr>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine("    <td>");
        sb.AppendLine("        &nbsp;</td>");
        sb.AppendLine(" </tr>");
        sb.AppendLine("</table>");
        sb.AppendLine("</div>");

        return sb.ToString();
    }
    public static string RegisterJQueryTimeout(Page page, PageType pageType)
    {
        Int32 sss = (Int32.Parse(ConfigurationManager.AppSettings["SessionTimeout"].ToString()) * 60 * 1000);
        string js = String.Empty;
        js += "var timeoutAlert;";
        js += "var countTimeOut;";
        js += "var countime = 1 * 60 * 1000;";
        js += "var flag = 60;";
        js += "var cnt = 1;";
        js += "function showTimeoutAlert() {";
        js += "    window.clearTimeout(timeoutAlert);";
        js += "    countTimeOut = setTimeout('showTimeoutCloseAlert()', countime);";
        js += "    var windowWidth = document.documentElement.clientWidth;";
        js += "    var windowHeight = document.documentElement.clientHeight;";
        js += "    var popupHeight = $('#timeoutMessage').height();";
        js += "    var popupWidth = $('#timeoutMessage').width();";
        js += "    $.blockUI({ message: $('#timeoutMessage'),";
        js += "        overlayCSS: {";
        js += "             backgroundColor: '#000',";
        js += "             opacity: '0.3'";
        js += "         },";
        js += "        css: {";
        js += "            width: '455px',";
        js += "            top: windowHeight / 2 - popupHeight / 2,";
        js += "            left: windowWidth / 2 - popupWidth / 2";
        js += "        }";
        js += "    });";
        js += "    showSec();";
        js += "}";
        js += "$('#close_message').click(function () {";
        js += "     $('#timeoutMessage').fadeOut('slow');";
        js += "     window.clearTimeout(countTimeOut);";
        js += "     closeWindow();";
        js += "});";
        js += "$('#redirect_default').click(function () {";
        js += "     $('#timeoutMessage').fadeOut('slow');";
        js += "     window.clearTimeout(countTimeOut);";
        js += "     cnt ++;";
        js += "     showWindowTimeout();";
        js += "     unblockUIWindow();";
        //js += "     redirectURL();";
        js += "});";
        // 倒數讀60秒
        js += "function showSec() {";
        js += "    flag = 60 * cnt;";
        js += "    window.setInterval('OnlineTimes();', 1000);";    //每隔1秒调用OnlineStayTimes
        js += "}";
        js += "function OnlineTimes() {";
        js += "     flag --;";
        js += "     if(flag > 0) {";
        js += "         $('#sec').text(Math.floor(flag/cnt));";
        js += "     } else {";                  // 避免倒數變成負的
        js += "         $('#sec').text(0);";
        js += "     }";
        js += "}";
        js += "function showWindowTimeout() {";
        js += "   timeoutAlert = setTimeout('showTimeoutAlert()', " + sss.ToString() + ");";
        js += "}";
        js += "function showSessionTimeout() {";
        js += "    setTimeout('showTimeoutCloseAlert()', " + (HttpContext.Current.Session.Timeout * 60 * 1000).ToString() + ");";
        js += "}";
        js += "function unblockUIWindow() {";
        js += "    $.unblockUI();";
        js += "}";
        js += "function redirectURL() {";
        js += "    window.location = 'Default.aspx';";
        js += "}";
        js += "function closeWindow() {";
        js += "    opener=null;";
        js += "    window.open('','_self');";
        js += "    window.close();";
        js += "}";
        js += "function showTimeoutCloseAlert() {";
        js += "    var windowWidth = document.documentElement.clientWidth;";
        js += "    var windowHeight = document.documentElement.clientHeight;";
        js += "    var popupHeight = $('#timeoutMessageClose').height();";
        js += "    var popupWidth = $('#timeoutMessageClose').width();";
        js += "    $.blockUI({ message: $('#timeoutMessageClose'),";
        js += "        overlayCSS: {";
        js += "             backgroundColor: '#000',";
        js += "             opacity: '0.3'";
        js += "         },";
        js += "        css: {";
        js += "            width: '450px',";
        js += "            top: windowHeight / 2 - popupHeight / 2,";
        js += "            left: windowWidth / 2 - popupWidth / 2";
        js += "        }";
        js += "    });";
        js += "    $('#close_form').click(function () {";
        js += "        $('#timeoutMessageClose').fadeOut('slow');";
        js += "        closeWindow();";
        js += "    });";
        js += "}";
        js += "$(document).ready(function () {";
        js += "    showWindowTimeout();";
        js += "    showSessionTimeout();";
        js += "});";

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "timeout", js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "timeout", js, true);
                break;
            default:
                break;
        }

        string ttt = ConfigurationManager.AppSettings["SessionTimeout"].ToString();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("<div id='timeoutMessage' style='display:none; font-size:10pt; text-align:left;'>");
        sb.AppendLine("    <table>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2'>");
        sb.AppendLine("                &nbsp;親愛的客戶您好：</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td style='width:20px;'>");
        sb.AppendLine("                &nbsp;</td>");
        sb.AppendLine("            <td>");
        sb.AppendLine("                &nbsp;您未持續使用本系統已超過系統時限" + ttt + "分鐘，為保障  您個人的使用安全，</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2'>");
        sb.AppendLine("                &nbsp;本系統將於<span id='sec'></span>秒後關閉，若  您欲繼續使用，請按<input id='redirect_default' type='button' value='繼續使用' /></td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2'>");
        sb.AppendLine("                &nbsp;或直接按<input id='close_message' type='button' value='我要離開' />，關閉本系統。</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("    </table>");
        sb.AppendLine("</div>");

        sb.AppendLine("<div id='timeoutMessageClose' style='display:none; font-size:10pt; text-align:left;'>");
        sb.AppendLine("    <table>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2'>");
        sb.AppendLine("                &nbsp;親愛的客戶您好：</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td style='width:20px;'>");
        sb.AppendLine("                &nbsp;</td>");
        sb.AppendLine("            <td>");
        sb.AppendLine("                &nbsp;您未持續使用本系統已超過系統時限，為保障  您個人的使用安全，</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2'>");
        sb.AppendLine("                &nbsp;已關閉本系統，欲再使用請重新進(登)入。</td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("        <tr>");
        sb.AppendLine("            <td colspan='2' style='text-align:center;'>");
        sb.AppendLine("                <input id='close_form' type='button' value='確認' /></td>");
        sb.AppendLine("        </tr>");
        sb.AppendLine("    </table>");
        sb.AppendLine("</div>");

        return sb.ToString();
    }
    /// <summary>
    /// 註冊一個javascript 限制 TextBox 輸入字元的 function
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageType"></param>
    /// <param name="clientId"></param>
    public static void RegisterOnKeypress(Page page, PageType pageType, string clientId)
    {
        string js = string.Empty;
        //<物件.ClientID>.Attributes.Add("onkeypress", "javascript:return checkTextBox<物件.ClientID>(event.keyCode,'3');");
        //判斷輸入值
        js += "function checkTextBox" + clientId + "(key, typeNo)";
        js += "{";
        js += "    switch (typeNo)";
        js += "    {";
        js += "       case '1':";    //金額"
        js += "            if (key>=48 && key<=57 || key==44 || key==46) {";
        js += "                 if (key == 46) {";   //判斷小數點不可以重複, 第一個字元不可以是"."
        js += "                     if ($('#" + clientId + "').val().indexOf('.') > -1)";
        js += "			                return false;";
        js += "                     if ($('#" + clientId + "').val().length == 0)";
        js += "			                return false;";
        js += "                 }";
        js += "                 return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為數值(0~9)、逗點(,)、小數點(.)');";
        js += "               return false;";
        js += "            }";
        js += "            break;";
        js += "        case '2':";   //純英文字母"
        js += "            if (key>=65 && key<=90) {";
        js += "                return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為大寫英文字母(A~Z)');";
        js += "                return false;";
        js += "            }";
        js += "            break;";
        js += "        case '3':";   //純數值"
        js += "            if (key>=48 && key<=57) {";
        js += "                return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為數值');";
        js += "                return false;";
        js += "            }";
        js += "            break;";
        js += "        case '4':";   //英文字母&數值"
        js += "            if (key>=48 && key<=57 || key>=65 && key<=90) {";
        js += "                return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為大寫英文字母(A~Z)、數值(0~9)');";
        js += "                return false;";
        js += "            }";
        js += "            break;";
        js += "       case '5':";    //英文字母(X)&數值"
        js += "            if (key>=48 && key<=57 || key==88) {";
        js += "                return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為大寫英文字母(X)、數值(0~9)');";
        js += "                return false;";
        js += "            }";
        js += "            break;";
        js += "       case 'id':";    //第一碼英文字母&其它為數值"
        js += "            if ($('#" + clientId + "').val().length == 0)";
        js += "                 if (key>=65 && key<=90) {";
        js += "                     return true;";
        js += "                 } else {";
        js += "                     return false;";
        js += "                 }";
        js += "             } else {";
        js += "                 if (key>=48 && key<=57) {";
        js += "                     return true;";
        js += "                 } else {";
        js += "                     return false;";
        js += "                 }";
        js += "            }";
        js += "            break;";
        js += "        default:";    //英數及空白"
        js += "            if (key>=48 && key<=57 || key>=65 && key<=90 || key==32) {";
        js += "                return true;";
        js += "            } else {";
        //js += "                alert('此欄位必須為大寫英文字母(A~Z)、數值(0~9)、空白');";
        js += "                return false;";
        js += "            }";
        js += "            break;";
        js += "    }";
        js += "}";

        switch (pageType)
        {
            case PageType.PostBack:
                page.ClientScript.RegisterStartupScript(page.GetType(), "Keypress" + clientId, js, true);
                break;
            case PageType.AJAX:
                ScriptManager.RegisterStartupScript(page, page.GetType(), "Keypress" + clientId, js, true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 設定物件ReadOnly的狀態
    /// </summary>
    /// <param name="ctrl"></param>
    public static void MakeCtrlReadOnly(Control ctrl, bool isReadOnly)
    {
        switch (ctrl.GetType().Name)
        {
            case "TextBox":
                if (isReadOnly)
                {
                    ((TextBox)ctrl).ReadOnly = true;
                    ((TextBox)ctrl).TabIndex = -1;
                }
                else
                {
                    ((TextBox)ctrl).ReadOnly = false;
                }
                break;

            case "CheckBox":
                CheckBox ckb = (CheckBox)ctrl;
                if (isReadOnly)
                {
                    string sChecked = ckb.Checked.ToString().ToLower();
                    ckb.Attributes.Add("onClick", "this.checked=" + sChecked + ";");
                    ckb.TabIndex = -1;
                }
                else
                {
                    ckb.Attributes.Remove("onClick");
                }
                break;

            case "RadioButton":
                RadioButton rdb = (RadioButton)ctrl;
                if (isReadOnly)
                {
                    string sRBChecked = rdb.Checked.ToString().ToLower();
                    rdb.GroupName = String.Empty;
                    rdb.Attributes.Add("onClick", "this.checked=" + sRBChecked + ";");
                }
                else
                {
                    rdb.Attributes.Remove("onClick");
                }
                break;

            case "DropDownList":
                DropDownList ddl = ((DropDownList)ctrl);
                if (isReadOnly)
                {
                    ddl.Attributes.Add("onChange", "this.selectedIndex='" + ddl.SelectedIndex.ToString() + "';");
                }
                else
                {
                    ddl.Attributes.Remove("onChange");
                }
                break;
        }
    }
    /// <summary>
    /// 設定Panel中的物件ReadOnly的狀態
    /// </summary>
    /// <param name="parentControl"></param>
    public static void MakeReadOnly(Control parentControl, bool _isReadOnly)
    {
        foreach (Control ctrl in parentControl.Controls)
        {
            MakeCtrlReadOnly(ctrl, _isReadOnly);

            // Recursively do any child controls as well.
            foreach (Control ctrlChild in ctrl.Controls)
            {
                MakeReadOnly(ctrlChild, _isReadOnly);
            }
        }
    }
}