<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LaborDataDetail.aspx.cs" Inherits="LaborDataDetail" EnableEventValidation="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %> 
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="�Ҥu�O�I��Ƭd��" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------�j�M����]�w------------------------------%>
<tr>
    <td align="left">
    <span class="ItemFontStyle">���@�@�q�G</span>
    <uc6:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true"/><span>�@</span>
    <span class="ItemFontStyle">���@�@���G</span>
    <asp:DropDownList ID="DepList1" runat="server" Width="180" AutoPostBack="true"/><span>�@</span>
    <span class="ItemFontStyle">���@�@�u�G</span>
    <asp:DropDownList ID="EmployeeIdList1" runat="server" Width="184" AutoPostBack="true"/>
    </td>
</tr>
    
<tr>
    <td align="left">
    <span class="ItemFontStyle">���������G</span>
    <asp:TextBox id="IDNo1" runat="server" Width="140" AutoPostBack="true" ></asp:TextBox>
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    <span>�@</span>
    <span class="ItemFontStyle">�������O�G</span>
    <asp:RadioButtonList id="TrxType" runat="server"  Width="180"  RepeatLayout="Flow"  AutoPostBack="true" /><span>�@</span>
    <span class="ItemFontStyle">�ͮĤ���G</span>
    <asp:TextBox id="EffectiveDateSt" runat="server" Width="80" CssClass="JQCalendar" ></asp:TextBox>
    <span>��</span>
    <asp:TextBox id="EffectiveDateEd" runat="server" Width="80"  CssClass="JQCalendar"  ></asp:TextBox>
    <asp:ImageButton id="btnQuery2" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    </td>
</tr>


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
 ShowFooter="false"
 DataKeyNames="Company,EmployeeId" 
 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------����������]�w--------%>
<RowStyle horizontalalign="Center"></RowStyle>


<%--------�C�����e�]�w�}�l--------%>
<Columns>
<asp:BoundField DataField="EmployeeId"  HeaderText="���u"   SortExpression="EmployeeId" >   </asp:BoundField>
<asp:BoundField DataField="DeptId"  HeaderText="����"   SortExpression="DeptId"   >   </asp:BoundField>
<asp:BoundField DataField="IDNo"  HeaderText="��������"   SortExpression="IDNo"   >   </asp:BoundField>
<asp:BoundField DataField="TrxType"  HeaderText="�������O"   SortExpression="TrxType"   >   </asp:BoundField>
<asp:BoundField DataField="EffectiveDate"  HeaderText="�ͮĤ��"   SortExpression="EffectiveDate"   >   </asp:BoundField>
<asp:BoundField DataField="LI_amount"  HeaderText="���B"   SortExpression="LI_amount"   >   </asp:BoundField>
</Columns>
<%--------�C�����e�]�w����--------%>


<%--------��������������]�w------%>
<PagerStyle horizontalalign="left">     </PagerStyle>
<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>
</asp:GridView>
</td></tr>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server"  
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
>      
</asp:SqlDataSource> 
<asp:HiddenField id="HiddenField1" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
