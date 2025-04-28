<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AnnualLeaveSettlement.aspx.cs" Inherits="AnnualLeaveSettlement" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="�~�ׯS�𵲺�" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------�����W��j�M�����϶}�l------------------------------%>
<!-- �j�M�Ҳ� -->
<tr class="QueryStyle">
    <td align="left">
        <uc9:SearchList id="SearchList1" runat="server" />
 
    </td>
</tr>
<!-- �j�M�Ҳ� -->
 <tr>
    <td align="left"><span class="ItemFontStyle">�S��~�סG</span>
        <uc1:YearList ID="qyYearList" runat="server" />
        <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    </td>
</tr>
 <tr>
    <td align="left"><span class="ItemFontStyle">����S��ѼơG</span>
        ��¾��Ǥ�G<asp:TextBox id="txtALYear" runat="server" MaxLength="10" width="60"></asp:TextBox>        
        ����S���ഫ�覡�G<uc8:CodeList ID="CL_ALTrans" runat="server" />
        <asp:Button id="btnSetALDays" onclick="btnSetALDays_Click" runat="server" Text="����" />
    </td>
</tr>
 <tr>
    <td align="left"><span class="ItemFontStyle">�O�_�b¾�G</span>
<asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList></td></tr>
 <%------------------------------�����W��j�M�����ϵ���------------------------------%>
 
 <tr><td colspan="2">
 <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label> 
 </td></tr>
 
 <tr><td colspan="2"></td></tr>
 
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td></tr>
 
 <tr><td align="left" colspan="2">
 <asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />�d�L���!!</asp:Panel>
 </td></tr>
 
 <%------------------------------GridView �]�w�}�l----------------------------------%>
 <tr><td colspan="2">
 <%--------GridView �ݩʳ]�w--------%>
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"
 
 DataKeyNames="Company,EmployeeId,ALYear" 
 
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
<RowStyle horizontalalign="Center"></RowStyle>


<%--------�C�����e�]�w�}�l--------%>
<Columns>
<%--------�R�����s�]�w------------%>
<asp:TemplateField HeaderText="�R��" ShowHeader="False" >
<ItemTemplate>

<asp:LinkButton 
id="btnDelete" 
onclick="btnDelete_Click" 
runat="server" 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
OnClientClick='return confirm("�T�w�R��?");' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
L3PK='<%# DataBinder.Eval(Container, "DataItem.ALYear")%>'
CausesValidation="False" __designer:wfdid="w4">
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

<HeaderStyle width="30px"></HeaderStyle>
<ItemStyle width="30px"></ItemStyle>
</asp:CommandField>
<%--------�n��ܥX�����]�w------%>
<asp:BoundField DataField="EmployeeId" HeaderText="���u" SortExpression="EmployeeId" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="ALYear" HeaderText="�S��~��" SortExpression="ALYear" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="ALDays" HeaderText="�S��Ѽ�" SortExpression="ALDays"></asp:BoundField>
<asp:BoundField DataField="LeaveDays" HeaderText="�w��Ѽ�" SortExpression="LeaveDays"></asp:BoundField>
<asp:BoundField DataField="LYTransDays" HeaderText="�h�~��J�Ѽ�"   SortExpression="LYTransDays"></asp:BoundField>
<asp:BoundField DataField="ConvertibleDays" HeaderText="���~�i�ഫ�Ѽ�" SortExpression="ConvertibleDays"></asp:BoundField>
<asp:BoundField DataField="TransOrNot" HeaderText="�O�_�w�ഫ" SortExpression="TransOrNot"></asp:BoundField>
</Columns>
<%--------�C�����e�]�w����--------%>

<%--------�̫�@�C����������]�w--%>
<FooterStyle horizontalalign="center">  </FooterStyle>
<%--------��������������]�w------%>
<PagerStyle horizontalalign="left">     </PagerStyle>

<%--------�]�w�ťո�ƦC----------%>
<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell" align="center">
<td class="paginationRowEdgeLl" style=" width:30px" >�s�W</td>
<td>���u</td>
<td>�S��~��</td>
<td>�S��Ѽ�</td>
<td>�w��Ѽ�</td>
<td>�h�~��J�Ѽ�</td>
<td>���~�i�ഫ�Ѽ�</td>
<td>�O�_�w�ഫ</td>
</tr>

<tr>
<td class="Grid_GridLine" align="center"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd"/></td>
<%--���u�s��--%>
<td class="Grid_GridLine">
<uc8:CodeList ID="tbAddNew01" runat="server" /></td>
<%--�S��~��--%>
<td class="Grid_GridLine">
<uc1:YearList ID="tbAddNew02" runat="server" /></td>
<%--�S��Ѽ�--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" MaxLength="2" /></td>
<%--�w��Ѽ�--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" MaxLength="2" /></td>
<%--�h�~��J�Ѽ�--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" MaxLength="2" /></td>
<%--���~�i�ഫ�Ѽ�--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" MaxLength="2" /></td>
<%--�O�_�w�ഫ--%>
<td class="Grid_GridLine"><asp:CheckBox runat="server" ID="tbAddNew07"></asp:CheckBox></td>
</tr>


</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>

<tr><td align="left" colspan="2"></td></tr>
</tbody></table>





<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 

ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT AnnualLeaveSettlement.* FROM AnnualLeaveSettlement "

InsertCommand="INSERT INTO AnnualLeaveSettlement(
Company,
EmployeeId,
ALYear,
ALDays,
LeaveDays,
LYTransDays,
ConvertibleDays,
TransOrNot
) 
VALUES (
@Company,
@EmployeeId,
@ALYear,
@ALDays,
@LeaveDays,
@LYTransDays,
@ConvertibleDays,
@TransOrNot
)" 

UpdateCommand="UPDATE AnnualLeaveSettlement SET
ALDays = @ALDays,
LeaveDays = @LeaveDays,
LYTransDays = @LYTransDays,
ConvertibleDays = @ConvertibleDays,
TransOrNot = @TransOrNot
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
ALYear = @ALYear"
 
DeleteCommand="DELETE FROM AnnualLeaveSettlement 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
ALYear = @ALYear
">
                 <InsertParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
                    <asp:Parameter Name="ALDays"                 />
                    <asp:Parameter Name="LeaveDays"                 />
                    <asp:Parameter Name="LYTransDays"                   />
                    <asp:Parameter Name="ConvertibleDays"           />
                    <asp:Parameter Name="TransOrNot"   />
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
                    <asp:Parameter Name="ALDays"                 />
                    <asp:Parameter Name="LeaveDays"                 />
                    <asp:Parameter Name="LYTransDays"                   />
                    <asp:Parameter Name="ConvertibleDays"           />
                    <asp:Parameter Name="TransOrNot"   />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
                 </DeleteParameters>       
                          
</asp:SqlDataSource> 
<asp:HiddenField id="HiddenField1" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
