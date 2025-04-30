<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="Customer.ascx.cs" Inherits="UserControls_Customer" %>

<fieldset style="font-size: 85%;">
    <legend hidden="hidden"></legend>
    <div runat="server" style="float:left;">
        <asp:Label ID="lblQuery" Font-Size="Larger" runat="server" Text="查詢條件：" /></div>
    <div runat="server" style="float:left;">
        <asp:TextBox ID="txtQuery" runat="server" /></div>
    <div>
        <asp:Button ID="btnQuery" runat="server" Text="查詢" /></div><br />
    <div id="divGrid" runat="server">
        <%--<asp:Panel ID="pnlGrid" runat="server">
        </asp:Panel>--%>
    </div>
    <br />
    <div id="divNavigation" runat="server">
        <table>
            <tr>
                <td id="tdFirst">
                    <asp:Image ID="imgFirst" runat="server" ImageUrl="~/images/first.png" ToolTip="第一頁" /></td>
                <td id="tdPrevious">
                    <asp:Image ID="imgPrevious" runat="server" ImageUrl="~/images/previous.png" ToolTip="上一頁" /></td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lbPageNumber" runat="server" /></td>
                <td>
                    &nbsp;</td>
                <td>
                   /</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lbAllPageCount" runat="server" /></td>
                <td>
                    &nbsp;</td>
                <td id="tdNext">
                    <asp:Image ID="imgNext" runat="server" ImageUrl="~/images/next.png" ToolTip="下一頁" /></td>
                <td id="tdLast">
                    <asp:Image ID="imgLast" runat="server" ImageUrl="~/images/last.png" ToolTip="最末頁" /></td>
                <td style="width:50px">
                    &nbsp;</td>
                <td>
                   到</td>
                <td>
                    <asp:DropDownList ID="selectPage" runat="server" Font-Size="9pt" Height="18px">
                    </asp:DropDownList>
                </td>
                <td>
                    頁</td>
            </tr>
        </table>
    </div>
</fieldset>
<%--公司別--%>
<asp:HiddenField ID="hfCompany" runat="server" />
<%--每頁筆數--%>
<asp:HiddenField ID="hfPageSize" runat="server" Value="12" />
<%--總共筆數--%>
<asp:HiddenField ID="hfAllRowCount" runat="server" />
<%--總共頁數--%>
<asp:HiddenField ID="hfAllPageCount" runat="server" />
<%--目前頁次--%>
<asp:HiddenField ID="hfPageNumber" runat="server" Value="1" />
<%--模糊參數--%>
<asp:HiddenField ID="hfQuery" runat="server" />

