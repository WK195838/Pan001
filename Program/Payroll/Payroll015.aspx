<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Payroll015.aspx.cs" Inherits="Payroll_Payroll015" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>

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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="薪資確認作業" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
        <tr align="left"><td>公　　司：</td><td><uc2:CompanyList ID="CompanyList1" runat="server" /></td></tr>
        <tr align="left"><td>計薪年月：</td><td><uc8:SalaryYM id="SalaryYM1" runat="server" /></td></tr>
        <tr align="left"><td>期　　別：</td><td><uc6:PeriodList id="PeriodList1" runat="server" />日</td></tr>
        <tr><td colspan="2"><asp:Button ID="btnConfirmPayroll" runat="server" Text="薪資確認" OnClick="btnConfirmPayroll_Click" />
            <asp:Button ID="btnQuery" runat="server" Text="確認結果查詢" OnClick="btnQuery_Click" /></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel></td></tr>
        <tr><td colspan="2">
         <asp:GridView id="gvMsg" runat="server" Width="100%" AutoGenerateColumns="False">
          <Columns>    
             <asp:BoundField DataField="ChangItem" HeaderText="確認步驟" ItemStyle-HorizontalAlign="Left" />
             <asp:BoundField DataField="SQLcommand" HeaderText="執行結果" ItemStyle-HorizontalAlign="Left" />
             <asp:BoundField DataField="ChgStartDateTime" HeaderText="執行時間" />
          </Columns>
         </asp:GridView>
        <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
         OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
          DataKeyNames="Company,EmployeeId" AutoGenerateColumns="False"
          OnSorting="GridView1_Sorting">
         <Columns>                                       
                <asp:TemplateField HeaderText="查詢">
                    <ItemTemplate>
                    <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                    Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="30px" />
                </asp:TemplateField>
             <asp:BoundField DataField="DeptId" HeaderText="部門" SortExpression="DeptId" />
             <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" />
             <asp:BoundField DataField="BaseSalary" HeaderText="底薪" />
             <asp:BoundField DataField="Paydays" HeaderText="計薪天數" />
             <asp:BoundField DataField="LI_Fee" HeaderText="勞保費" />
             <asp:BoundField DataField="HI_Fee" HeaderText="健保費" />             
             <asp:BoundField DataField="NT_Overtime" HeaderText="免稅加班" />
             <asp:BoundField DataField="WT_Overtime" HeaderText="應稅加班" />                        
         </Columns>
     </asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>