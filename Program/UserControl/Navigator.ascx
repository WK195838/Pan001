<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Navigator.ascx.cs" Inherits="SystemControl_Navigator" %>
<table border="0">
    <tr valign="middle">
        <td valign="middle" style="width: 125px">
            <asp:ImageButton ID="btn_First" runat="server" SkinID="FirstPage" OnClick="btn_First_Click" />
            <asp:ImageButton ID="btn_Prev" runat="server" SkinID="PrevPage" OnClick="btn_Prev_Click" />
            <asp:ImageButton ID="btn_Next" runat="server" SkinID="NextPage" OnClick="btn_Next_Click" />
            <asp:ImageButton ID="btn_Last" runat="server" SkinID="LastPage" OnClick="btn_Last_Click" />
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
            <asp:ImageButton ID="btn_GotoPage" runat="server" OnClick="btn_GotoPage_Click" SkinID="GoPage" />
        </td>
        <td>           
            <asp:DropDownList ID="ddl_GotoPage" runat="server" Visible="False"></asp:DropDownList>
        </td>
        </tr>
</table>
