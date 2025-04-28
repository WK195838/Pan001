<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="CompanyMaster.aspx.cs" Inherits="CompanyMaster" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">    
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
<uc2:StyleTitle ID="StyleTitle1" runat="server" Title="公司主檔維護" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />

<table id="table1" cellspacing="0" cellpadding="0" width="100%">

<%-----------------------------------------------------------%>

<tr class="QueryStyle"><td align="left">
<span class="ItemFontStyle">公司編號：</span>
<asp:TextBox ID="Company" runat="server" MaxLength="20"></asp:TextBox>
</td></tr>

<%-----------------------------------------------------------%>

<tr class="QueryStyle"><td align="left">
<span class="ItemFontStyle">公司簡稱：</span>
<asp:TextBox ID="CompanyShortName" runat="server" MaxLength="50"></asp:TextBox>  
<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />
<asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd" />
</td></tr>

<%-----------------------------------------------------------%>

<tr><td colspan="2">
<asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
</td></tr>

<%-----------------------------------------------------------%>

<tr><td colspan="2">
<uc7:Navigator ID="Navigator1" runat="server" />
</td></tr>

<%-----------------------------------------------------------%>

<tr><td align="left" colspan="2">
<asp:Panel ID="Panel_Empty" runat="server" Height="50px" Visible="False" Width="250px">
<br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel>
</td></tr>

<%-----------------------------------------------------------%>

<tr><td colspan="4">
<asp:GridView 
ID="GridView1" 
runat="server" 
AutoGenerateColumns="False" 
DataKeyNames="Company"
OnRowDataBound="GridView1_RowDataBound" 
OnRowDeleted="GridView1_RowDeleted"
OnRowDeleting="GridView1_RowDeleting"
OnRowCreated="GridView1_RowCreated"
DataSourceID="SDS_GridView" 
AllowPaging="True" 
AllowSorting="True"
Width="100%"
>

<%-----------------------------------------------------------%>

<Columns>

<%-----------------------------------------------------------%>

<asp:TemplateField HeaderText="刪除" ShowHeader="False">
<ItemTemplate>
<asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  L2PK='<%# DataBinder.Eval(Container, "DataItem.CompanyShortName")%>'
OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
</asp:TemplateField>

<%-----------------------------------------------------------%>

<asp:TemplateField HeaderText="編輯">
<ItemTemplate>
<asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px" />
</asp:TemplateField>

<%-----------------------------------------------------------%>

<asp:TemplateField HeaderText="查詢">
<ItemTemplate>
<asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px" />
</asp:TemplateField>

<%-----------------------------------------------------------%>

<asp:BoundField DataField="Company" HeaderText="公司編號" ReadOnly="True" SortExpression="Company" />
<asp:BoundField DataField="CompanyShortName" HeaderText="公司簡稱" SortExpression="CompanyShortName" />
<asp:BoundField DataField="CompanyName" HeaderText="公司全名" SortExpression="CompanyName" />


<%-----------------------------------------------------------%>

</Columns>
</asp:GridView>
</td>
</tr>
</table>

<%-----------------------------------------------------------%>


<asp:SqlDataSource ID="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"></asp:SqlDataSource>
<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<br />
</div>
</asp:Content>

