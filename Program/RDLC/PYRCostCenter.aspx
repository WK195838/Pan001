<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYRCostCenter.aspx.cs" Inherits="PYRCostCenter" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>
--%>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
 <%--�Ω���浥�e��(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
 <%--�Ω���浥�e��(End) --%>
<script language ="javascript" type="text/javascript"�@>
//�S�O����
//Company	���q�s��
//EmployeeId	���u�s��
//DepositBank	�s�J�Ȧ� 
//DepositBankAccount	�s�J�b��
//Period2Depositdate	�U���s�J���
//Period1Depositdate	�W���s�J���
//Company	���q�s��
//EmployeeId	���u�s��
//SalaryItem	�~�궵��
//Amount	���B

</script>    

<%-- <div>--%>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox>
<uc2:StyleTitle id="StyleTitle1" title="�������ߤ��u��" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- �j�M�Ҳ� -->
<tr class="QueryStyle">
    <td align="left" 
        style=" position:relative; height: 84px; top: 0px; left: 0px;">
        <uc9:SearchList id="SearchList1" runat="server" />
        &nbsp;���u�ܳ����G&nbsp;
        <asp:DropDownList ID="DepName" runat="server" Width="165px">
        </asp:DropDownList>
        <br />
        &nbsp;�C�L�ﶵ�G<asp:RadioButton ID="Rab_Sel1" runat="server" AutoPostBack="True" 
            oncheckedchanged="Rab_Sel1_CheckedChanged" Text="�̳���" />
        <asp:RadioButton ID="Rab_Sel2" runat="server" AutoPostBack="True" 
            oncheckedchanged="Rab_Sel2_CheckedChanged" Text="�̳���" />
        <asp:RadioButton ID="Rab_Sel3" runat="server" AutoPostBack="True" 
            oncheckedchanged="Rab_Sel3_CheckedChanged" Text="�̭ӤH" />
        <asp:Button ID="Button1" runat="server" Text="�̳�������C�L" 
            onclick="Button1_Click" />
        &nbsp;&nbsp;
        <asp:Button ID="Sel_DepId" runat="server" Text="����C�L" 
            onclick="Sel_DepId_Click" Width="68px" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="Sel_Employ" runat="server" Text="�̭��u����C�L" 
            onclick="Sel_Employ_Click" Width="111px" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="btnQuery" runat="server" onclick="btnQuery_Click" 
            SkinID="Query1" Visible="False" />
    </td>
</tr>
<!-- �j�M�Ҳ� -->
<%--<tr class="QueryStyle"><td align="left">
<span class="ItemFontStyle">���@�@�q�G</span></td>
<td align="left"><uc1:CompanyList id="CompanyList1" runat="server" ></uc1:CompanyList></td></tr>


<tr class="QueryStyle"><td align="left">
<span class="ItemFontStyle">���@�@�u�G</span></td>
<td align="left"><asp:TextBox id="tbEmployeeId" runat="server" MaxLength="50" width="165"></asp:TextBox>
<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
<asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton> 
</td></tr>--%>
 
 
 <tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
     <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" >
     </rsweb:ReportViewer>
     </td></tr>
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" Visible="False"></uc7:Navigator> 
     </td></tr>
 <tr><td align="left" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
                                <br />�d�L���!!<uc1:CompanyList ID="CompanyList1" runat="server" 
                                    AutoPostBack="True" />
                                <asp:DropDownList ID="Epploy" runat="server" Width="165px">
                                </asp:DropDownList>
     </asp:Panel> </td></tr>
                                <tr><td colspan=4><asp:GridView id="GridView1" runat="server" Width="100%" 
                                        AllowSorting="True" AllowPaging="True" DataSourceID="SDS_GridView" 
                                OnRowCreated="GridView1_RowCreated" 
                                OnRowDeleting="GridView1_RowDeleting" 
                                OnRowDeleted="GridView1_RowDeleted" 
                                OnRowDataBound="GridView1_RowDataBound" 
                                DataKeyNames="Company,EmployeeId,DeptId" 
                                AutoGenerateColumns="False" Visible="False">                                 
                                 <Columns>
                                      <%--  <asp:TemplateField HeaderText="�R��" ShowHeader="False">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
                                            OnClientClick='return confirm("�T�w�R��?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="�s��">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='�s��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>--%>
                                        
                                        <asp:TemplateField HeaderText="�d��">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='�d��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                     <asp:BoundField DataField="Company" HeaderText="���q" ReadOnly="True" SortExpression="Company" Visible="False" />
                                     <asp:BoundField DataField="DeptId" HeaderText="����" SortExpression="DeptId" />
                                     <asp:BoundField DataField="EmployeeId" HeaderText="���u" SortExpression="EmployeeId" />
                                     <asp:BoundField DataField="Balance" HeaderText="�w���u���" SortExpression="Balance" />
                                     <asp:BoundField DataField="Remainder" HeaderText="�Ѿl���u���" SortExpression="Remainder" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table><asp:SqlDataSource id="SDS_GridView" runat="server"  SelectCommand="SELECT Payroll_Master_Heading.* FROM Payroll_Master_Heading" DeleteCommand="DELETE FROM Payroll_Master_Heading WHERE (Company = @Company And EmployeeId = @EmployeeId " ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
                 <DeleteParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="EmployeeId" />
                     <asp:Parameter Name="DepositBank" />
                       <asp:Parameter Name="DepositBankAccount" />
                 </DeleteParameters>
             </asp:SqlDataSource> <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>
        <br />
 <%--</div>--%>
</asp:Content>

