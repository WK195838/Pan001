<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYR001PIC.aspx.cs" Inherits="PYR001PIC" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="薪資清冊" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative " colspan="2">
        <uc9:SearchList id="SearchList1" runat="server" />        
    </td>
</tr>
<!-- 搜尋模組 -->
        <tr align="left"><td colspan="2">計薪年月：<uc8:SalaryYM id="SalaryYM1" runat="server" /></td></tr>
        <tr align="left"><td colspan="2">
        <ul>
        <li style="float:left">帳務類別：</li>
        <li style="float:left">
        <asp:RadioButtonList ID="RpSalaryKind" runat="server" RepeatDirection="Horizontal" CellPadding="5" Width="150px">
            <asp:ListItem Value="" Selected="True">轉帳用</asp:ListItem>
            <asp:ListItem Value="S">出帳用</asp:ListItem>
            </asp:RadioButtonList>
        </li>
        </ul>
        </td></tr>
        <tr align="left"><td colspan="2">
        <ul>
        <li style="float:left">資料類別：</li>
        <li style="float:left">
        <asp:RadioButtonList ID="RpKind" runat="server" RepeatDirection="Horizontal" CellPadding="5" Width="150px">
            <asp:ListItem Value="W" Selected="True">試算</asp:ListItem>
            <asp:ListItem Value="H">確認</asp:ListItem>
            </asp:RadioButtonList>
        </li>
        <li>　　
         <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" Visible="false" />
         <asp:Button ID="btReport" runat="server" Text="報表" OnClick="btReport_Click" /></li></ul></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel></td></tr>
        <tr><td colspan="2">
            <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="400px" width="100%">
            <LocalReport ReportPath="RDLC\PYR001PIC.rdlc">
            </LocalReport>
            </rsweb:reportviewer>
        </td></tr>
        <tr><td colspan="2">
        <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
         OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
          DataKeyNames="Company,EmployeeId" AutoGenerateColumns="False"
          OnSorting="GridView1_Sorting">
         <Columns>
             <asp:BoundField DataField="ROWID" HeaderText="項次" />
             <asp:BoundField DataField="EmployeeId" HeaderText="工號" SortExpression="EmployeeId" />
             <asp:BoundField DataField="EmployeeName" HeaderText="姓名" SortExpression="EmployeeName" ItemStyle-Wrap="false" />
             <asp:BoundField DataField="BaseSalary" HeaderText="本薪</BR><font size='0.5px'>(捨)(離進)</font>" HtmlEncode="false" />
             <asp:BoundField DataField="Other01" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Attendance" HeaderText="全勤" />
             <asp:BoundField DataField="OtherWT" HeaderText="其他薪</BR>(應稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="OtherNT" HeaderText="其他</BR>(免稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="WT_Overtime_fee" HeaderText="應稅</BR>加班" HtmlEncode="false" />
             <asp:BoundField DataField="NT_Overtime_fee" HeaderText="免稅</BR>加班" HtmlEncode="false" />             
             <asp:BoundField DataField="PAmount" HeaderText="加項合計" />
             <asp:BoundField DataField="LeaveDeduction" HeaderText="請假小計" HtmlEncode="false" />
             <asp:BoundField DataField="LI_Fee" HeaderText="勞保費" />
             <asp:BoundField DataField="HI_Fee" HeaderText="健保費" />    
             <asp:BoundField DataField="FB_Fee" HeaderText="福利金" />
             <asp:BoundField DataField="TAX" HeaderText="所得稅" />
             <asp:BoundField DataField="Parking_Fee" HeaderText="停車費" />
             <asp:BoundField DataField="WR_Fee" HeaderText="婚喪</BR>禮金" HtmlEncode="false"  />
             <asp:BoundField DataField="OtherNTP" HeaderText="其他</BR>(免稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="MAmount" HeaderText="扣項合計" />
             <asp:BoundField DataField="PayAmount" HeaderText="實領薪資" />
             <asp:BoundField DataField="WT_Amount" HeaderText="應稅金額" />
             <asp:BoundField DataField="Pension" HeaderText="退休金提撥" />
         </Columns>
     </asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>