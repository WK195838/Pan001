<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AppErrorMessage.aspx.cs" Inherits="AppErrorMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <div>
        <asp:HyperLink ID="btn_Logout" runat="server" CssClass="button_link" NavigateUrl="~/AuthAD.aspx" Target="_parent">重新登入</asp:HyperLink>
        
        <asp:Label ID="Label1" runat="server"></asp:Label>&nbsp;
        <br />
        <asp:Label ID="Label2" runat="server"></asp:Label></div>
</asp:Content>