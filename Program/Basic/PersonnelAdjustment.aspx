<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PersonnelAdjustment.aspx.cs" Inherits="PersonnelAdjustment" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"  TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox>
<uc2:StyleTitle id="StyleTitle1" title="人事調動資料檔" runat="server"></uc2:StyleTitle>
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart>
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>

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

<tr><td colspan="2">
<asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
</td></tr>

<tr><td colspan="4">
<uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator>
</td></tr>

<tr><td align="left" colspan="2">
<asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"><br />查無資料，請選擇其他項目或新增</asp:Panel>
</td></tr>

<tr><td colspan="4">

<asp:GridView 
id="GridView1"

DataKeyNames="Company,EmployeeId,AdjustmentCategory,EffectiveDate"  
Width="100%"

runat="server" 
AllowSorting="true" 
AllowPaging="true" 
DataSourceID="SDS_GridView" 
OnRowCreated="GridView1_RowCreated" 
OnRowDeleting="GridView1_RowDeleting" 
OnRowDeleted="GridView1_RowDeleted" 
OnRowDataBound="GridView1_RowDataBound" 
AutoGenerateColumns="False">

<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False">
<HeaderStyle CssClass="paginationRowEdgeL" Width="30px"></HeaderStyle>
<ItemTemplate>
<asp:LinkButton 
ID="btnDelete" 
runat="server" 
CausesValidation="False"  
OnClick="btnDelete_Click" 
L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
L3PK='<%# DataBinder.Eval(Container, "DataItem.AdjustmentCategory")%>'
L4PK='<%# DataBinder.Eval(Container, "DataItem.EffectiveDate")%>'
OnClientClick='return confirm("確定刪除?");' 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />">
</asp:LinkButton>                                
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="編輯">
<HeaderStyle Width="30px"></HeaderStyle>
<ItemTemplate>
<asp:LinkButton 
ID="btnEdit" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>                                   
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="查詢">
<HeaderStyle Width="30px"></HeaderStyle>
<ItemTemplate>
<asp:LinkButton 
ID="btnSelect" 
runat="server" 
CausesValidation="False"
Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" >
</asp:LinkButton>                         
</ItemTemplate>
</asp:TemplateField>

</Columns></asp:GridView></td></tr></tbody></table>
 
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"></asp:SqlDataSource> 

<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 

</ContentTemplate></asp:UpdatePanel><br /></div></asp:Content>
