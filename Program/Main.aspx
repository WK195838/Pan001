<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

<%@ Register Src="UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc1" %>
<%@ Register Src="UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/WUCSysTree.ascx" TagName="WUCSysTree" TagPrefix="uc9" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart> 
<center>
<div id="theMap" runat="server">
<uc9:WUCSysTree id="WUCSysTree1" runat="server"/>
</div>
<div id="the3DList" runat="server" >
</div>
<!--object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
codebase="h--ttp://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0"
 width="700px" height="400px" id="cd-01">
<param name="movie" value="image/EBOS.swf" /> <param name="quality" value="high" /> <param name="bgcolor" value="#FFFFFF" />
<embed src="image/EBOS.swf" quality="high" bgcolor="#FFFF99" name="cd-01" 
 type="application/x-shockwave-flash" pluginspage="h--ttp://www.macromedia.com/go/getflashplayer"></embed>
</object--> 
</center>
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</asp:Content>

