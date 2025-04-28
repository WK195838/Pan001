<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Calendar.aspx.cs" Inherits="Calendar" Title="行事曆" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="UserControl/CalendarDate.ascx" TagName="CalendarDate" TagPrefix="uc1" %>
<%@ Register Src="UserControl/CalendarWeek.ascx" TagName="CalendarWeek" TagPrefix="uc2" %>
<%@ Register Src="UserControl/CalendarMonth.ascx" TagName="CalendarMonth" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
<script language="javascript" type="text/javascript">
///////////////////////////////////////////////////////////////////////////////////////////
//  版面控制
///////////////////////////////////////////////////////////////////////////////////////////
	//頁籤背景變色
	var _oldbg;
	function setnewbg(source)
    {
        _oldbg=source.style.backgroundImage;
        source.style.backgroundImage= "url(../App_Themes/ePayroll/images/Tab3.GIF)";
    }

	function setoldbg(source)
    {
        source.style.backgroundImage= _oldbg
    }	
</script>
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
   
       
    <table width="750px" cellpadding="0" cellspacing="0">
    
    <tr align="center">
        <td style="background:#444; height:26px;">
        
        <asp:ImageButton id="Day" runat="server" OnClick="Day_Click" SkinID="day" Height="26" CssClass="ImB" ></asp:ImageButton>
   
        <asp:ImageButton id="Week" runat="server" OnClick="Week_Click" SkinID="week" Height="26px" CssClass="ImB"></asp:ImageButton>
   
        <asp:ImageButton id="Months" runat="server" OnClick="Months_Click" SkinID="months"  Height="26px" CssClass="ImB" ></asp:ImageButton>
        
        <asp:Label id="lbl_TabMsg" runat="server" ForeColor="red"></asp:Label>
        </td>                
    </tr>    
    <tr>
        <td colspan="4" style="width:100%">
        <uc1:CalendarDate ID="CalendarDate1" runat="server" />
        <uc2:CalendarWeek ID="CalendarWeek1" runat="server" />
        <uc3:CalendarMonth ID="CalendarMonth1" runat="server" />
        </td>
     </tr>
    </table>    
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

