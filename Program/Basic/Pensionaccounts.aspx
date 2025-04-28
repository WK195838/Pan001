<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Pensionaccounts.aspx.cs" Inherits="Pensionaccounts" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
   
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"/>
<uc2:StyleTitle id="StyleTitle1" title="�h�����ƺ��@" runat="server"/>
<uc4:StyleContentStart id="StyleContentStart1" runat="server"/>
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>

<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->
<!-- �j�M�Ҳ� -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        <uc9:SearchList id="SearchList1" runat="server" />
        <div>
        <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton>
        </div>
    </td>
</tr>
<!-- �j�M�Ҳ� -->
<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->

<tr><td colspan="2">
<asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
</td></tr>

<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->

<tr><td colspan="2">
<uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator>
</td></tr>

<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->

<tr><td align="left" colspan="2">
<asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"><br />�d�L��ơA�п�ܤ��q�ηs�W���</asp:Panel>
</td></tr>

<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->
<tr><td colspan="4">
<asp:GridView 
Width="100%" 
id="GridView1"
runat="server"
AllowSorting="true"
AllowPaging="true"
DataSourceID="SDS_GridView"
OnRowCreated="GridView1_RowCreated"
OnRowDeleting="GridView1_RowDeleting"
OnRowDeleted="GridView1_RowDeleted"
OnRowDataBound="GridView1_RowDataBound"
DataKeyNames="Company,EmployeeId"
AutoGenerateColumns="False">

<Columns>

<asp:TemplateField HeaderText="�s��">
<ItemTemplate>
<asp:LinkButton 
ID="btnEdit" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='�s��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>                                          
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:TemplateField HeaderText="�d��">
<ItemTemplate>
<asp:LinkButton 
ID="btnSelect" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/select1.gif' border='0' alt='�d��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:BoundField DataField="Company" HeaderText="���@�@�q" SortExpression="Company"></asp:BoundField>
<asp:BoundField DataField="EmployeeId" HeaderText="���@�@�u" SortExpression="EmployeeId"></asp:BoundField>
</Columns></asp:GridView></td></tr></tbody></table>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
SelectCommand="SELECT Pensionaccounts_Master.* FROM Pensionaccounts_Master" 
ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
</asp:SqlDataSource>

<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"/>

</ContentTemplate>
</asp:UpdatePanel>
</div>
</asp:Content>
