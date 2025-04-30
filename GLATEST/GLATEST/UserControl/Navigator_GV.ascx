<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Navigator_GV.ascx.cs" Inherits="SystemControl_Navigator_GV" %>
<table border="0">
    <tr valign="middle">
        <td valign="middle" style="width: 125px">
            <asp:ImageButton ID="btn_First" runat="server" ImageUrl="~/images/page_arrow_beg.gif" OnClick="btn_First_Click" />
            <asp:ImageButton ID="btn_Prev" runat="server" ImageUrl="~/images/page_arrow_left.gif" OnClick="btn_Prev_Click" />
            <asp:ImageButton ID="btn_Next" runat="server" ImageUrl="~/images/page_arrow_right.gif" OnClick="btn_Next_Click" />
            <asp:ImageButton ID="btn_Last" runat="server" ImageUrl="~/images/page_arrow_beg.rtl.gif" OnClick="btn_Last_Click" />
        </td>
        <td>           
            每頁
        </td>
        <td>           
            <asp:DropDownList ID="ddl_PageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_PageSize_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td>           
            <asp:Label ID="lbl_Desc" runat="server"
                Text="筆，共ｍ頁筆，第"></asp:Label>
        </td>
        <td>           
            <asp:TextBox ID="txt_GotoPage" runat="server" Width="45px"></asp:TextBox>
        </td>
        <td>           
            <asp:Label ID="lbl_Desc2" runat="server" Text="頁，共ｍ頁"></asp:Label>&nbsp;
        </td>
        <td>           
            <asp:ImageButton ID="btn_GotoPage" runat="server" OnClick="btn_GotoPage_Click" ImageUrl="~/images/go.jpg" />
        </td>
        <td>           
            <asp:DropDownList ID="ddl_GotoPage" runat="server" Visible="False"></asp:DropDownList>
        </td>
        </tr>
</table>
