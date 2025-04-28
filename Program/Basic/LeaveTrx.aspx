<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LeaveTrx.aspx.cs" Inherits="LeaveTrx" EnableEventValidation="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"/>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc2:StyleTitle id="StyleTitle1" title="�а��D��" runat="server"/>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"/>
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
 <%------------------------------�����W��j�M�����϶}�l------------------------------%>
 <!-- �j�M�Ҳ� -->
<tr class="QueryStyle">
    <td align="left">
        <uc9:SearchList id="SearchList1" runat="server" />
 
    </td>
</tr>
<!-- �j�M�Ҳ� -->

<tr><td align="left">
<span class="ItemFontStyle">���@�@�O�G</span>
<uc4:CodeList id="CL_LeaveType" runat="server" AutoPostBack="true" />
</td></tr>
 <%------------------------------�����W��j�M�����ϵ���------------------------------%>
 
 <tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>
 
 <tr><td colspan="2"></td></tr>
 
 <tr><td align="center"><uc7:Navigator id="Navigator1" runat="server"/></td></tr>
 
 <tr><td align="center"><asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />�d�L��ơA�п�ܨ�L���ةηs�W</asp:Panel></td></tr>
 
 <%------------------------------GridView �]�w�}�l----------------------------------%>
 <tr><td colspan="2">
 <%--------GridView �ݩʳ]�w--------%>
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"
 
 DataKeyNames="Company,EmployeeId,LeaveType_Id,BeginDateTime" 
 
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
 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------����������]�w--------%>
<RowStyle horizontalalign="Center"/>
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
L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
L3PK='<%# DataBinder.Eval(Container, "DataItem.LeaveType_Id")%>'
L4PK='<%# DataBinder.Eval(Container, "DataItem.BeginDateTime")%>'
CausesValidation="False">
</asp:LinkButton> 
</ItemTemplate>
<HeaderStyle width="20px"/>
</asp:TemplateField>

<%--------�s����s�]�w------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="�s��">

<HeaderStyle width="20px"/>
<ItemStyle width="20px"/>
</asp:CommandField>



<%--------�n��ܥX�����]�w------%>
<asp:BoundField DataField="EmployeeId"              HeaderText="���u�s��"       SortExpression="EmployeeId"                 ReadOnly="true"/>
<asp:BoundField DataField="LeaveType_Id"            HeaderText="���O"           SortExpression="LeaveType_Id"               ReadOnly="true"/>
<asp:BoundField DataField="BeginDateTime"           HeaderText="�_�l���"       SortExpression="BeginDateTime"              ReadOnly="true"/>
<asp:BoundField DataField="EndDateTime"             HeaderText="�פ���"       SortExpression="EndDateTime"/>                              
<asp:BoundField DataField="hours"                   HeaderText="�ɼ�"           SortExpression="hours"/>                                    
<asp:BoundField DataField="days"                    HeaderText="�Ѽ�"           SortExpression="days"/>                                                                 
<asp:BoundField DataField="Payroll_Processingmonth" HeaderText="�p�~�~��"   SortExpression="Payroll_Processingmonth"/>                 
</Columns>
<%--------�C�����e�]�w����--------%>

<%--------�̫�@�C����������]�w--%>
<FooterStyle horizontalalign="center"/>
<%--------��������������]�w------%>
<PagerStyle horizontalalign="left"/> 



<%--------�]�w�ťո�ƦC----------%>
<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell">
<td class="paginationRowEdgeLl">�s�W</td>
<td>���u</td>
<td>���O</td>
<td>�_�l���</td>
<td>�פ���</td>
<td>�ɼ�</td>
<td>�Ѽ�</td>
<td>�p�~�~��</td>
</tr>

<tr>
<!-- �s�W���s -->
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<!-- ���u -->
<td class="Grid_GridLine"><asp:DropDownList id="TB01" runat="server"></asp:DropDownList></td>
<!-- ���O -->
<td class="Grid_GridLine"><uc4:CodeList id="TB02" runat="server" /></td>
<!-- �_�l��� -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB03" CssClass="JQCalendar" />
<asp:DropDownList id="TB04" runat="server"></asp:DropDownList></td>
<!-- �פ��� -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB05"   CssClass="JQCalendar" />
<asp:DropDownList id="TB06" runat="server"></asp:DropDownList></td>
<!-- �ɼ� -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB07" /></td>
<!-- �Ѽ� -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB08" />
<!-- �p�~��� -->
<td class="Grid_GridLine">
<asp:DropDownList id="TB09" runat="server"></asp:DropDownList>
<asp:DropDownList id="TB010" runat="server"></asp:DropDownList></td>
</tr>

</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>
 <%------------------------------GridView �]�w����----------------------------------%>

<tr><td align="left" colspan="2"></td></tr>
</tbody></table>


<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
InsertCommand="INSERT INTO Leave_Trx(
Company,
EmployeeId,
LeaveType_Id,
BeginDateTime,
EndDateTime,
hours,
days,
Payroll_Processingmonth
) 
VALUES (
@Company,
@EmployeeId,
@LeaveType_Id,
@BeginDateTime,
@EndDateTime,
@hours,
@days,
@Payroll_Processingmonth
)" 

UpdateCommand="UPDATE Leave_Trx SET 
EndDateTime = @EndDateTime,
hours = @hours,
days = @days,
Payroll_Processingmonth = @Payroll_Processingmonth 
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
LeaveType_Id= @LeaveType_Id AND
(Convert(varchar,BeginDateTime,120) = @BeginDateTime)"
 
DeleteCommand="DELETE FROM Leave_Trx 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
LeaveType_Id= @LeaveType_Id AND
(Convert(varchar,BeginDateTime,120) = @BeginDateTime)
">
             
                 <InsertParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
                    <asp:Parameter Name="EndDateTime"               />
                    <asp:Parameter Name="hours"                     />
                    <asp:Parameter Name="days"                      />
                    <asp:Parameter Name="Payroll_Processingmonth"   />
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
                    <asp:Parameter Name="EndDateTime"               />
                    <asp:Parameter Name="hours"                     />
                    <asp:Parameter Name="days"                      />
                    <asp:Parameter Name="Payroll_Processingmonth"   />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
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
