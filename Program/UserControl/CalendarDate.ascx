<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarDate.ascx.cs" Inherits="UserControl_CalendarDate" %>
<%@ Register Src="CalendarDay.ascx" TagName="CalendarDay" TagPrefix="uc1" %>
<div style=" width:100%; text-align:center">
<asp:Panel ID="PanelLastAndNext" runat="server" Visible="false">
    <asp:ImageButton ID="ibLast" runat="server" SkinID="CalendarLD" OnClick="Last" />
    <asp:ImageButton ID="ibNext" runat="server" SkinID="CalendarND" OnClick="Next"  />
    <asp:HiddenField ID="thisShow" runat="server" />
    <asp:HiddenField ID="thisCompany" runat="server" />
    <asp:HiddenField ID="thistheDate" runat="server" />
    <asp:HiddenField ID="thisDepId" runat="server" />
    <asp:HiddenField ID="thisEmployeeId" runat="server" />
    <asp:HiddenField ID="thisCategory" runat="server" />
    <asp:HiddenField ID="thisStatus" runat="server" />   
</asp:Panel>
</div>
<table cellpadding="0" cellspacing="0" border="0">

<tr>
<td>
<table cellpadding="0" cellspacing="0" border="0">
<tr>
<td>
<div style="height:30px;vertical-align:bottom;">
<asp:Label ID="labDate" runat="server" Font-Bold="True" Font-Size="18px"  ></asp:Label>
<asp:Literal ID="labDate2" runat="server" />

<div style="height:12px; float:right; line-height:12px;">
<asp:Label ID="labWeek" runat="server" Font-Bold="true" ></asp:Label>
</div>

<div style="height:12px;float:right;line-height:12px">
<asp:Label ID="labYM" runat="server" Font-Bold="true" ></asp:Label>
</div>
</div>
</td>
</tr>
</table>
</td>
</tr>

<tr><td>
<div>
<asp:Panel ID="PanelDay" runat="server" ScrollBars="Auto" Width="100%" CssClass="PanelDay">
<uc1:CalendarDay ID="CalendarDay1" runat="server" />
</asp:Panel>
</div>
</td></tr>

</table>