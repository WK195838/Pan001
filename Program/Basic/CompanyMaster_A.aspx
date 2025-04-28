<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyMaster_A.aspx.cs" Inherits="CompanyMaster_A" validateRequest="false"   %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>公司主檔新增</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
</head>
<body>
<form id="form1" runat="server">
<div>

<uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
<uc2:StyleHeader ID="StyleHeader1" runat="server" />
<uc3:StyleTitle ID="StyleTitle1" runat="server" Title="公司主檔新增" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
<table width="100%">
<tr>
<td align="center">
<asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
</tr>
<tr>
<td>
<asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Insert" Width="100%" AutoGenerateRows="False" DataSourceID="SqlDataSource1" DataKeyNames="Company" OnItemCreated="DetailsView1_ItemCreated" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting"></asp:DetailsView>
</td>
</tr>
<tr><td>
<asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
<asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
<asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
</td></tr>
</table>


<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"></asp:SqlDataSource>        
<asp:HiddenField ID="hid_Company" runat="server" />
<asp:HiddenField ID="hid_CompanyShortName" runat="server" />
<asp:HiddenField ID="hid_InserMode" runat="server" />
<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
<uc6:StyleFooter ID="StyleFooter1" runat="server" />

</div>
</form>
</body>
</html>

