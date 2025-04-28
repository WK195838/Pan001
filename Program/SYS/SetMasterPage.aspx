<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="SetMasterPage.aspx.cs" Inherits="SetMasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
Set Master Page：
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" >
        <asp:ListItem Value="EEOC">預設</asp:ListItem>
        <asp:ListItem>Theme_01</asp:ListItem>        
    </asp:RadioButtonList>
    <br />
    Set Theme：
    <asp:RadioButtonList ID="RadioButtonList2" runat="server" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged" >
        <asp:ListItem Value="ePayroll">預設</asp:ListItem>
        <asp:ListItem>Theme_01</asp:ListItem>        
        <asp:ListItem>Theme_09</asp:ListItem>     
        <asp:ListItem Value="EEOCSkin">樹葉版</asp:ListItem>
    </asp:RadioButtonList><br />
    <asp:Button ID="SetPage" runat="server" Text="變更設定" OnClick="SetPage_Click" />
</asp:Content>

