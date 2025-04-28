<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="BonusConfirm.aspx.cs" Inherits="BonusConfirm" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="獎金發放確認作業" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative " colspan="2">
        <uc9:SearchList id="SearchList1" runat="server" />
        獎金名目：<CodeList:CodeList ID="CostId" runat="server" /><br />
        發放日期：<asp:TextBox ID="txtAmtDate" runat="server" CssClass="JQCalendar" />
    </td>         
</tr>
<!-- 搜尋模組 -->
        <tr><td colspan="2"><asp:Button ID="btnConfirmPayroll" runat="server" Text="獎金確認" OnClick="btnConfirmPayroll_Click" />
            <asp:Button ID="btnQuery" runat="server" Text="確認結果查詢" OnClick="btnQuery_Click" /></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!</asp:Panel></td></tr>
        <tr><td colspan="2">
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True"
     ShowFooter="true" AllowPaging="True"
    OnRowCreated="GridView1_RowCreated" 
    OnRowDataBound="GridView1_RowDataBound" 
    OnSorting="GridView1_Sorting"
    DataKeyNames="Company,EmployeeId,CostName,AmtDate,DepName,Row_Count" 
    AutoGenerateColumns="False">          
    <Columns>
    <asp:TemplateField HeaderText="查詢">
    <ItemTemplate>
    <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
    </ItemTemplate>
    <HeaderStyle width="30px" />
    </asp:TemplateField>
        <asp:BoundField DataField="DepName" HeaderText="部門" SortExpression="DepName" />
        <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" />
        <asp:BoundField DataField="CostName" HeaderText="獎金名目" SortExpression="CostName" />
        <asp:BoundField DataField="CostAmt" HeaderText="發放總金額" />
        <asp:BoundField DataField="Pay_AMT" HeaderText="代扣繳金額" />
        <asp:BoundField DataField="HI2" HeaderText="補充保費" SortExpression="HI2" />
        <asp:BoundField HeaderText="實際金額" />
        <asp:BoundField DataField="AmtDate" HeaderText="發放年月" SortExpression="AmtDate" />
        <asp:BoundField DataField="DepositBank" HeaderText="存入銀行" SortExpression="DepositBank" Visible="False" />
        <asp:BoundField DataField="DepositBankAccount" HeaderText="銀行帳號"  SortExpression="DepositBankAccount" Visible="False" />
        <asp:BoundField DataField="ControlDown" HeaderText="是否已發放" />
        <asp:BoundField DataField="Company" HeaderText="公司名稱" SortExpression="Company" Visible="False" />
        <asp:BoundField DataField="Row_Count" HeaderText="項次" 
            SortExpression="Row_Count" Visible="False" />
    </Columns>
    </asp:GridView>
    <asp:GridView id="GridView2" runat="server" Width="100%" AutoGenerateColumns="true"></asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>