<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarWeek.ascx.cs" Inherits="UserControl_CalendarWeek" %>
<%@ Register Src="CalendarDate.ascx" TagName="CalendarDate" TagPrefix="uc1" %>

<div style=" width:100%; text-align:center">
<asp:Panel ID="PanelLastAndNext" runat="server" Visible="false">
    <asp:ImageButton ID="ibLast" runat="server" SkinID="CalendarLW" OnClick="Last" />
    <asp:ImageButton ID="ibNext" runat="server" SkinID="CalendarNW" OnClick="Next"  />    
    <asp:HiddenField ID="thisShow" runat="server" />
    <asp:HiddenField ID="thisCompany" runat="server" />
    <asp:HiddenField ID="thistheDate" runat="server" />
    <asp:HiddenField ID="thisDepId" runat="server" />
    <asp:HiddenField ID="thisEmployeeId" runat="server" />
    <asp:HiddenField ID="thisCategory" runat="server" />
    <asp:HiddenField ID="thisStatus" runat="server" />
</asp:Panel>
</div>

<div style="width:100%">

    <uc1:CalendarDate ID="CalendarDate1" runat="server"  />

    <uc1:CalendarDate ID="CalendarDate2" runat="server"  />


    <uc1:CalendarDate ID="CalendarDate3" runat="server"  />  
    
    <uc1:CalendarDate ID="CalendarDate4" runat="server"  />

    <uc1:CalendarDate ID="CalendarDate5" runat="server"  />    

    <uc1:CalendarDate ID="CalendarDate6" runat="server"  />

    <uc1:CalendarDate ID="CalendarDate7" runat="server"  /> 
</div>






