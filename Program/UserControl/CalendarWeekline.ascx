<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarWeek.ascx.cs" Inherits="UserControl_CalendarWeek" %>
<%@ Register Src="CalendarDate.ascx" TagName="CalendarDate" TagPrefix="uc1" %>
<asp:Panel ID="PanelLastAndNext" runat="server" Visible="false" >
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

    <table cellpadding="0" cellspacing="0">
        <tr style="vertical-align:top">
            <td class="CalendarTR">
            <asp:Panel ID="Panel1" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate1" runat="server" PanelDayHeight="100px"/>
            </asp:Panel>          
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel2" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate2" runat="server" PanelDayHeight="100px" />
            </asp:Panel>            
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel3" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate3" runat="server" PanelDayHeight="100px" />
            </asp:Panel>              
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel4" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate4" runat="server" PanelDayHeight="100px" />
            </asp:Panel>               
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel5" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate5" runat="server" PanelDayHeight="100px" />
            </asp:Panel>               
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel6" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate6" runat="server" PanelDayHeight="100px" />
            </asp:Panel> 
            </td>
            <td class="CalendarTR">
            <asp:Panel ID="Panel7" runat="server" ScrollBars="None" CssClass="DCell">
            <uc1:CalendarDate ID="CalendarDate7" runat="server" PanelDayHeight="100px" />
            </asp:Panel>               
            </td>
        </tr>       
    </table>







