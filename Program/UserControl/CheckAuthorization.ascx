<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CheckAuthorization.ascx.cs" Inherits="UserControl_CheckAuthorization" %>
<asp:Label ID="labUnAuth" runat="server" ForeColor="Red" Text="無授權!" Visible="false"></asp:Label>
<asp:Label ID="labAuthEnd" runat="server" ForeColor="Red" Text="授權已到期!" Visible="false"></asp:Label>
<asp:Label ID="labAuthOK" runat="server" Text="授權使用" Visible="false"></asp:Label>
<asp:ImageButton ID="ib_UpLoadAuth" runat="server" SkinID="SU1" Visible="false" />
<asp:HiddenField ID="hid_UplodFileStyle" runat="server" />
<!-- #include file="../SYS/include/Include.at" -->
