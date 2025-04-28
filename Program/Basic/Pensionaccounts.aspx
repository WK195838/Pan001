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
<uc2:StyleTitle id="StyleTitle1" title="退休金資料維護" runat="server"/>
<uc4:StyleContentStart id="StyleContentStart1" runat="server"/>
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>

<!--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@-->
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        <uc9:SearchList id="SearchList1" runat="server" />
        <div>
        <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton>
        </div>
    </td>
</tr>
<!-- 搜尋模組 -->
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
<asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"><br />查無資料，請選擇公司或新增資料</asp:Panel>
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

<asp:TemplateField HeaderText="編輯">
<ItemTemplate>
<asp:LinkButton 
ID="btnEdit" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>                                          
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:TemplateField HeaderText="查詢">
<ItemTemplate>
<asp:LinkButton 
ID="btnSelect" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:BoundField DataField="Company" HeaderText="公　　司" SortExpression="Company"></asp:BoundField>
<asp:BoundField DataField="EmployeeId" HeaderText="員　　工" SortExpression="EmployeeId"></asp:BoundField>
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
