<%@ Page Title="" Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="HI2Rules.aspx.cs" Inherits="SYS_HI2Rules" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<uc3:StyleTitle ID="StyleTitle1" runat="server" Title="健保所得類別說明" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <!-- #include file="../SYS/include/HI2.inc" -->
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd>      
</asp:Content>

