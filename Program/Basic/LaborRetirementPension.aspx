<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LaborRetirementPension.aspx.cs" Inherits="LaborRetirementPension" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="�봣ú�u����Ū�" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 

<table id="table1" cellspacing="0" cellpadding="0" width="100%">

<tbody>
<!--    GridView�W��϶�   -->

<%--<tr><td align="left">
<span class="ItemFontStyle">�š@�@�@�ơG</span>
<asp:TextBox id="txtRangeNo" runat="server" MaxLength="50"></asp:TextBox>
<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
</td></tr>--%>

<tr><td align="left">
<span class="ItemFontStyle">���~������v�U���G </span>
<asp:Label ID="CompanyBurdenRate" runat="server"></asp:Label>
</td></tr>
<tr><td align="left">
<span class="ItemFontStyle">���u�۴���v�W���G </span>
<asp:Label ID="EmpBurdenRate" runat="server"></asp:Label>
</td></tr>

<!-- ���~�T�� -->
<tr><td align="center"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>

<!-- GridView ����Bar -->
<tr><td align="center"><uc7:Navigator id="Navigator1" runat="server" Visible="false"/></td></tr>

<!-- ���~�T�� -->
<tr><td align="center" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Height="50px" width="250px" Visible="False"><br />�d�L���!!</asp:Panel></td></tr>

<!--    GridView    -->
<tr><td coln="2">
<!--    GridView �ݩʳ]�w    -->
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"

 DataKeyNames="RangeNo"
  
 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowUpdated="GridView1_RowUpdated" 
 OnRowUpdating="GridView1_RowUpdating" 
 OnRowCancelingEdit="GridView1_RowCancelingEdit" 
 OnRowEditing="GridView1_RowEditing" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 OnRowDeleting="GridView1_RowDeleting" 
 OnRowDeleted="GridView1_RowDeleted" 
 AutoGenerateColumns="False" 
 AllowPaging="false" 
 AllowSorting="true"
 >
 
<%--------����������]�w--------%>
<RowStyle HorizontalAlign="Center"></RowStyle>
<%--------�C�����e�]�w�}�l--------%>
<Columns>
<%--------�R�����s�]�w------------%>
<asp:TemplateField HeaderText="�R��" ShowHeader="False">

<ItemTemplate>

<asp:LinkButton 
id="btnDelete"
onclick="btnDelete_Click" 
runat="server" 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" 
OnClientClick='return confirm("�T�w�R��?");' 
L2PK='<%# DataBinder.Eval(Container, "DataItem.LowerLimit")%>' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.RangeNo")%>' 
CausesValidation="False">
</asp:LinkButton> 

</ItemTemplate>

<HeaderStyle width="30px"></HeaderStyle>

</asp:TemplateField>
<%--------�s����s�]�w------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="�s��">

<HeaderStyle width="60px"></HeaderStyle>
<ItemStyle width="60px"></ItemStyle>
</asp:CommandField>

<%--                             �s�W�Ϭq                            --%>

<asp:BoundField DataField="RangeNo" HeaderText="�š@�@��"   ReadOnly="true" SortExpression="RangeNo"/>
<asp:BoundField DataField="LowerLimit"   HeaderText="��ڤu��W��" SortExpression="LowerLimit"/>
<asp:BoundField DataField="UpperLimit" HeaderText="��ڤu��U��" SortExpression="UpperLimit"/>
<asp:BoundField DataField="M_Contribution_Wages"   HeaderText="�봣ú�u��" SortExpression="M_Contribution_Wages"/>

<%--                             �s�W�Ϭq����                            --%>

</Columns>
<FooterStyle horizontalalign="center"/>
<PagerStyle HorizontalAlign="left"/>


<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell" align="center">
<td class="paginationRowEdgeLl">�s�W</td>
<td>�š@�@��</td>
<td>��ڤu��W��</td>
<td>��ڤu��U��</td>
<td>�봣ú�u��</td>>
</tr>

<tr>
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>
</tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>

</asp:GridView></td></tr>
 <%------------------------------GridView �]�w����----------------------------------%>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>
<%--///////////////////////////////////////////////////////////////////////////////////////////--%>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
  
SelectCommand="SELECT LaborRetirementPension.* FROM LaborRetirementPension " 
DeleteCommand="DELETE FROM LaborRetirementPension WHERE RangeNo = @RangeNo" 
UpdateCommand="UPDATE LaborRetirementPension SET  LowerLimit = @LowerLimit,UpperLimit=@UpperLimit,M_Contribution_Wages=@M_Contribution_Wages  WHERE RangeNo = @RangeNo " 
InsertCommand="INSERT INTO LaborRetirementPension(RangeNo, LowerLimit,UpperLimit,M_Contribution_Wages) VALUES (@RangeNo,@LowerLimit,@UpperLimit,@M_Contribution_Wages)" >


<DeleteParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LowerLimit"/>
<asp:Parameter Name="UpperLimit"/>
<asp:Parameter Name="M_Contribution_Wages"/>
</DeleteParameters>

<UpdateParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LowerLimit"/>
<asp:Parameter Name="UpperLimit"/>
<asp:Parameter Name="M_Contribution_Wages"/>
</UpdateParameters>

<InsertParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LowerLimit"/>
<asp:Parameter Name="UpperLimit"/>
<asp:Parameter Name="M_Contribution_Wages"/>
</InsertParameters>

</asp:SqlDataSource>

<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
