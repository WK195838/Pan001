<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarMonth.ascx.cs" Inherits="UserControl_CalendarMonth" %>
<%@ Register Src="CalendarWeekline.ascx" TagName="CalendarWeekline" TagPrefix="uc1" %>
<div style=" width:100%; text-align:center">
<asp:Panel ID="PanelLastAndNext" runat="server" Visible="false" >
    <asp:ImageButton ID="ibLast" runat="server" SkinID="CalendarLM" OnClick="Last" />
    <asp:Label ID="labThisMonth" runat="server"  Font-Size="16px"></asp:Label>
    <asp:ImageButton ID="ibNext" runat="server" SkinID="CalendarNM" OnClick="Next"  />
    <asp:HiddenField ID="thisShow" runat="server" />
    <asp:HiddenField ID="thisCompany" runat="server" />
    <asp:HiddenField ID="thistheDate" runat="server" />
    <asp:HiddenField ID="thisDepId" runat="server" />
    <asp:HiddenField ID="thisEmployeeId" runat="server" />
    <asp:HiddenField ID="thisCategory" runat="server" />
    <asp:HiddenField ID="thisStatus" runat="server" />    
</asp:Panel>
</div>

<div style=" border:1px solid #A0A0A0; border-bottom: 0; border-right:0; ">
<uc1:CalendarWeekline ID="CalendarWeekine1" runat="server" />
<uc1:CalendarWeekline ID="CalendarWeekine2" runat="server" />
<uc1:CalendarWeekline ID="CalendarWeekine3" runat="server" />
<uc1:CalendarWeekline ID="CalendarWeekine4" runat="server" />
<uc1:CalendarWeekline ID="CalendarWeekine5" runat="server" />
<uc1:CalendarWeekline ID="CalendarWeekine6" runat="server" />
</div>