<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="BonusMaster.aspx.cs" Inherits="BonusMaster" %>

<%-- ���J����  --%>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language="javascript" type="text/javascript">
//�S�O����
</script>    
    <div>
<%-- �e�����D��� start --%>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="�����o����@�{��" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart>
<%-- �e�����D��� end --%>
<%-- �j�M���� start --%>
<table id="table1"  cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- �j�M�Ҳ� -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        <uc9:SearchList id="SearchList1" runat="server" />
        �����W�ءG<CodeList:CodeList ID="CostId" runat="server" /><br />
        �o��~��G<asp:RadioButton ID="RB_Sel1" runat="server" AutoPostBack="True" OnCheckedChanged="RB_Sel1_CheckedChanged"
            Text="����" />
        <asp:RadioButton ID="RB_Sel2" runat="server" AutoPostBack="True" OnCheckedChanged="RB_Sel2_CheckedChanged"
            Text="����" />
        <uc8:YearList ID="YearList1" runat="server" AutoPostBack="True" />
        �~<uc10:MonthList ID="MonthList1" runat="server" AutoPostBack="True" />
        ��<asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd">
        </asp:ImageButton>&nbsp;<asp:ImageButton ID="btnQuery" runat="server" 
            onclick="btnQuery_Click" SkinID="Query1" />
    </td>         
</tr>
<!-- �j�M�Ҳ� -->
<%--<tr class="QueryStyle">
<td align="left">
     <span class="ItemFontStyle">��    �q�G</span>&nbsp;</td>
<td align="left">
    <uc1:CompanyList id="CompanyList1" runat="server"  ></uc1:CompanyList></td>
</tr>
<tr class="QueryStyle">
<td align="left">
    <span class="ItemFontStyle">��    �u�G</span>&nbsp;</td>
<td align="left">
    <asp:TextBox id="tbEmployeeId" runat="server" MaxLength="50"></asp:TextBox>
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton></td>
</tr>--%>
<%-- �j�M���� end --%>
<%-- ���~�T���P��Ƶ��Ƥ��� start --%>
<tr>
<td colspan="2">
    <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
</tr>
<tr>
<td colspan="2" style="height: 54px">
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td>
</tr>
<%-- ���~�T���P��Ƶ��Ƥ��� end --%>
<%-- �L��Ʈ����panel start --%>
<tr>
    <td align="left" colspan="2" style="height: 76px">
        <asp:Panel id="Panel_Empty" runat="server" Width="250px" Height="50px"><br />�d�L���!!
        <asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel>
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True"
     ShowFooter="true" AllowPaging="True" DataSourceID="SDS_GridView" 
    OnRowCreated="GridView1_RowCreated" 
    OnRowDeleting="GridView1_RowDeleting" 
    OnRowDeleted="GridView1_RowDeleted" 
    OnRowDataBound="GridView1_RowDataBound" 
    DataKeyNames="Company,EmployeeId,CostName,AmtDate,DepName,Row_Count" 
    AutoGenerateColumns="False">          
    <Columns>
    <asp:TemplateField HeaderText="�R��" ShowHeader="False">
    <ItemTemplate>
    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"     
    L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' 
    L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>' 
    L3PK='<%# DataBinder.Eval(Container, "DataItem.CostName")%>'
    L4PK='<%# DataBinder.Eval(Container, "DataItem.AmtDate")%>' 
    L5PK='<%# DataBinder.Eval(Container, "DataItem.DepName")%>'
    L6PK='<%# DataBinder.Eval(Container, "DataItem.Row_Count")%>' 
    OnClientClick='return confirm("�T�w�R��(Y/N)?");' 
    Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton><%--L4PK='<%# DataBinder.Eval(Container, "DataItem.AdjustSalaryItem")%>'--%>
    </ItemTemplate>
    <HeaderStyle width="30px" cssclass="paginationRowEdgeL" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="�s��">
    <ItemTemplate>
    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='�s��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
    </ItemTemplate>
    <HeaderStyle width="30px" />
    </asp:TemplateField>
    <asp:TemplateField HeaderText="�d��">
    <ItemTemplate>
    <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/select1.gif' border='0' alt='�d��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
    </ItemTemplate>
    <HeaderStyle width="30px" />
    </asp:TemplateField>
        <asp:BoundField DataField="DepName" HeaderText="����" SortExpression="DepName" />
        <asp:BoundField DataField="EmployeeId" HeaderText="���u" SortExpression="EmployeeId" />
        <asp:BoundField DataField="CostName" HeaderText="�����W��" SortExpression="CostName" />
        <asp:BoundField DataField="CostAmt" HeaderText="�o���`���B" 
            SortExpression="CostAmt" />
        <asp:BoundField DataField="Pay_AMT" HeaderText="�N��ú���B" 
            SortExpression="Pay_AMT" />
        <asp:BoundField DataField="HI2" HeaderText="�ɥR�O�O" SortExpression="HI2" />
        <asp:BoundField HeaderText="��ڪ��B" />
        <asp:BoundField DataField="AmtDate" HeaderText="�o��~��" SortExpression="AmtDate" />
        <asp:BoundField DataField="DepositBank" HeaderText="�s�J�Ȧ�" SortExpression="DepositBank" Visible="False" />
        <asp:BoundField DataField="DepositBankAccount" HeaderText="�Ȧ�b��"  SortExpression="DepositBankAccount" Visible="False" />
        <asp:BoundField DataField="ControlDown" HeaderText="�O�_�w�o��" SortExpression="ControlDown" />
        <asp:BoundField DataField="Company" HeaderText="���q�W��" SortExpression="Company" Visible="False" />
        <asp:BoundField DataField="Row_Count" HeaderText="����" 
            SortExpression="Row_Count" Visible="False" />
    </Columns>
    </asp:GridView>
        <br />
    </td>
</tr>
<%-- �L��Ʈ����panel end --%>
<tr>
<td colspan="4">
<%-- GridView �\��]�w start --%>
    </td>
<%-- GridView �\��]�w end --%>  
</tr>
</tbody>
</table>
<%-- Sql�y�k�]�w�Pdatasource�ӷ� start --%>
<asp:SqlDataSource id="SDS_GridView" runat="server" 
SelectCommand="Select A.Company,(A.DepId+'-'+A.DepName) DepName,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId,A.CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount From BonusMaster  Where 1=1 AND A.Company = @Company And A.EmployeeId= @EmployeeId " 
DeleteCommand="DELETE FROM BonusMaster WHERE Company = @Company And EmployeeId = @EmployeeId " 
ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
<DeleteParameters>
    <asp:Parameter Name="Company" />
    <asp:Parameter Name="EmployeeId" />
    <asp:Parameter Name="DepositBank" />
    <asp:Parameter Name="DepositBankAccount" />
</DeleteParameters>
</asp:SqlDataSource>
<%-- Sql�y�k�]�w�Pdatasource�ӷ� end --%>
    <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
        &nbsp;
        <br />
        <br />
    </div>
</asp:Content>


