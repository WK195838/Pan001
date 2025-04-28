<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StyleTitle.ascx.cs" Inherits="StyleTitle" %>
<table id="StyleTitle" border="0" cellpadding="0" cellspacing="0" style="height: 6px; width: 100%;">
    <tr>
        <td class="Header_L" align="left"></td>
        <td class="Header_Body" style=" font-size:12pt; line-height:30px">
                <asp:Label ID="lblTitle" runat="server" Text="標題"></asp:Label>
                <asp:Label ID="lblSubTitle" runat="server" Text="副標題"></asp:Label>
        </td>
        <td class="Header_Body" style="width:120px">
            <asp:Label ID="lbl_User" runat="server" Text=""></asp:Label>
        </td>
        <td class="Header_Body" style="width:40px; display:none">
            <asp:HyperLink ID="btn_Home" runat="server" Target="_parent" NavigateUrl="~/MonthStar.aspx"
                CssClass="button_link" ToolTip="回首頁">回首頁</asp:HyperLink>
        </td>
        <td class="Header_Body"  style="width:40px; display:none ">           
           <asp:HyperLink ID="btn_back" runat="server" Target="_parent" NavigateUrl="javascript:window.history.go(-1);"
                CssClass="button_link" ToolTip="回上一頁">上一頁</asp:HyperLink>
        </td>
        <td class="Header_R" style="height: 29px; width: 30px;" align="left">
        </td>
    </tr>
</table>
