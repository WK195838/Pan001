<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYR015.aspx.cs" Inherits="RDLCPYR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="~/UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc5" %>

<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>

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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="生日人員明細表" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
        <!-- 搜尋模組 -->
        <tr class="QueryStyle">
            <td align="left" style=" position:relative " colspan="2">
            <uc9:SearchList id="SearchList1" runat="server" />        
            </td>
        </tr>
        <!-- 搜尋模組 -->   
        <tr align="left"><td colspan="2">
        月　　份：<uc5:MonthList ID="MLSelect" runat="server" />    
        </td></tr>         
        <tr><td colspan="2">
            <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click"/>
            <asp:Button ID="btReport" runat="server" Text="報表" OnClick="btReport_Click" Visible="true" /></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!</asp:Panel></td></tr>
        <tr><td colspan="2">
            <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="400px" width="100%">
            <LocalReport ReportPath="RDLC\PYR015.rdlc">
            </LocalReport>
            </rsweb:reportviewer>
        </td></tr>
        <tr><td colspan="2">
        <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
         OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
          DataKeyNames="EmployeeId" AutoGenerateColumns="False"
          OnSorting="GridView1_Sorting">
         <Columns>             
             <asp:BoundField DataField="DeptId" HeaderText="部門" SortExpression="DeptId" ItemStyle-Wrap="false" />
             <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" ItemStyle-Wrap="false" />
             <asp:BoundField DataField="Sex" HeaderText="性別" />         
             <asp:BoundField DataField="BirthDate" HeaderText="出生日期" />          
         </Columns>
     </asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>