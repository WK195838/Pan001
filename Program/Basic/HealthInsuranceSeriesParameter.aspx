<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="HealthInsuranceSeriesParameter.aspx.cs" Inherits="HealthInsuranceSeriesParameter" %>
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

<uc2:StyleTitle id="StyleTitle1" title="�������O�żƳ]�w" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 

<table id="table1" cellspacing="0" cellpadding="0" width="100%">

<tbody>
<!--    GridView�W��϶�   -->
<tr>
<td align="left">
<span class="ItemFontStyle">�š@�@�@�ơG</span>
<asp:TextBox id="txtRangeNo" runat="server" MaxLength="50"></asp:TextBox>
<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
</td>
</tr>

<tr�@style=" width:100%; margin:0 auto; text-align:center;">
<td>
<span class="ItemFontStyle"> �@��O�O�v�G </span>
<asp:Label ID="Rates" runat="server"></asp:Label>
<span class="ItemFontStyle"> �@���q�t��G </span>
<asp:Label ID="CompanyBurdenRate" runat="server"></asp:Label>
<span class="ItemFontStyle"> �@���u�t��G </span>
<asp:Label ID="EmpBurdenRate" runat="server"></asp:Label>
<span class="ItemFontStyle"> �@��O���t����B�t���H�Υ������ݤH�ơG </span>
<asp:Label ID="Comp_burden_Ave" runat="server"></asp:Label>
<span class="ItemFontStyle">�H</span>
<span class="ItemFontStyle"> �@���u��O(�t���H)�W���H�ơG </span>
<asp:Label ID="EmpBurden_upto" runat="server"></asp:Label>
<span class="ItemFontStyle">�H</span>
</td>
</tr>

<!-- ���~�T�� -->
<tr><td align="center"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>

<!-- GridView ����Bar -->
<tr><td align="center"><uc7:Navigator id="Navigator1" runat="server"/></td></tr>

<!-- ���~�T�� -->
<tr><td align="center" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Height="50px" width="250px" Visible="False"><br />�d�L���!!</asp:Panel></td></tr>

<!--    GridView    -->
<tr><td colspan="2">
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
 AllowPaging="true" 
 AllowSorting="true">
 
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
L2PK='<%# DataBinder.Eval(Container, "DataItem.LiAmt")%>' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.RangeNo")%>' 
CausesValidation="False"  __designer:wfdid="w3">
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

<asp:BoundField DataField="RangeNo" HeaderText="�ż�"   ReadOnly="true" SortExpression="RangeNo"/>
<asp:BoundField DataField="LiAmt"   HeaderText="���O���B" SortExpression="LiAmt"/>
<asp:BoundField DataField="HIFee" HeaderText="���p�O�I�O"   ReadOnly="true" /> 
<asp:BoundField DataField="Grants" HeaderText="�ɧU�t�B"  SortExpression="Grants"/> 
<asp:BoundField HeaderText="���@�@�H"   ReadOnly="true" /> 
<asp:BoundField HeaderText="�� �� ��"   ReadOnly="true" /> 
<asp:BoundField HeaderText="�� �� ��"   ReadOnly="true" /> 
<asp:BoundField HeaderText="�� �� ��"   ReadOnly="true" /> 
<asp:BoundField DataField="HIFeeCOM" HeaderText="���@�@�q"   ReadOnly="true" /> 
<asp:BoundField DataField="HIFeeGOV" HeaderText="�F�@�@��"   ReadOnly="true" /> 
<asp:BoundField HeaderText=""   ReadOnly="true" />
<%--                             �s�W�Ϭq����                            --%>

</Columns>

<FooterStyle HorizontalAlign="center"/>
<PagerStyle HorizontalAlign="left"/>


<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell" align="center">
<td class="paginationRowEdgeLl">�s�W</td>
<td>�š@�@��</td>
<td>���O���B</td>
<td>���p�O�I�O</td>
<td>�ɧU�t�B</td>
<td>���@�@�H</td>
<td>�� �� ��</td>
<td>�� �� ��</td>
<td>�� �� ��</td>
<td>���@�@�q</td>
<td>�F�@�@��</td>
</tr>

<tr>
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
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
  
SelectCommand="SELECT convert(int,[RangeNo]) [RangeNo],[LiAmt],[Grants] FROM HealthInsurance_SeriesParameter " 
DeleteCommand="DELETE FROM HealthInsurance_SeriesParameter WHERE RangeNo = @RangeNo" 
UpdateCommand="UPDATE HealthInsurance_SeriesParameter SET  LiAmt = @LiAmt ,Grants = @Grants  WHERE RangeNo = @RangeNo " 
InsertCommand="INSERT INTO HealthInsurance_SeriesParameter(RangeNo, LiAmt, Grants) VALUES (@RangeNo, @LiAmt, @Grants)" >


<DeleteParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
</DeleteParameters>

<UpdateParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
<asp:Parameter Name="Grants"/>
</UpdateParameters>

<InsertParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
<asp:Parameter Name="Grants"/>
</InsertParameters>

</asp:SqlDataSource>

<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
