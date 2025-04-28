<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="ExportCompanyBurden.aspx.cs" Inherits="ExportCompanyBurden" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc6" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="公司負擔資料" />
        <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
    <ul>
    <li>
    <uc9:SearchList id="SearchList1" runat="server" />  
    </li>

    <li>
    計薪年月：<uc8:SalaryYM id="SalaryYM1" runat="server" />
    </li>
    
    <li>    
    <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
    <asp:Button ID="btReport" runat="server" Text="報表" OnClick="btReport_Click" />
    <asp:Button ID="btnToExcel" runat="server" Text="匯出"/>
    </li>

    <li>
     <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red" />
    </li>

    <li>
        <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="400px" width="100%">
        <LocalReport ReportPath="Export\CompanyBurdenReport.rdlc">
        </LocalReport>
        </rsweb:reportviewer>
    </li>
    <li>
    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text="Error" Width="575px"></asp:Label>    
    </li>
    <li>
    </li>
    </ul>
    </ContentTemplate>
    </asp:UpdatePanel>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%"
     OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
     AllowPaging="True" AllowSorting="True" PageSize="9000" 
     onpageindexchanging="GridView1_PageIndexChanging" 
     onsorting="GridView1_Sorting" >    
    <Columns>	
        <asp:BoundField DataField="Company" HeaderText="公司別" HtmlEncode="false" />
        <asp:BoundField DataField="DeptId" HeaderText="<font color='#FFFFFF'>部門</font>" SortExpression="DeptId" HtmlEncode="false" />
        <asp:BoundField DataField="SalaryYM" HeaderText="<font color='#FFFFFF'>年月</font>" SortExpression="SalaryYM" HtmlEncode="false"  />
        <asp:BoundField DataField="EmployeeId" HeaderText="<font color='#FFFFFF'>員工編號</font>" SortExpression="EmployeeId" HtmlEncode="false" />
        <asp:BoundField DataField="EmployeeName" HeaderText="<font color='#FFFFFF'>姓名</font>" SortExpression="EmployeeName" ItemStyle-Wrap="false" HtmlEncode="false" />

        <asp:BoundField DataField="BaseSalary" HeaderText="<font color='#FFFFFF'>薪資</font>" SortExpression="SalaryYM" HtmlEncode="false" DataFormatString="{0:N0}" />        
        <asp:BoundField DataField="SalaryItem02" HeaderText="伙食費" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="LIS" SortExpression="LIS" HeaderText="<font color='#FFFFFF'>勞保投保級距</font>" HtmlEncode="false" DataFormatString="{0:N0}"  />
        <asp:BoundField DataField="SalaryItem15" HeaderText="勞保公司負擔金額" HtmlEncode="false" DataFormatString="{0:N0}"  />
        <asp:BoundField DataField="SalaryItem05" HeaderText="勞保員工自付額" HtmlEncode="false" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="HIS" SortExpression="HIS" HeaderText="<font color='#FFFFFF'>健保投保級距</font>" HtmlEncode="false" DataFormatString="{0:N0}" />             
        <asp:BoundField DataField="SalaryItem14" HeaderText="健保公司負擔金額" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="SalaryItem04" HeaderText="健保員工自付額" HtmlEncode="false" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="HI2" HeaderText="健保補充保費" HtmlEncode="false" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="RPS" SortExpression="RPS" HeaderText="<font color='#FFFFFF'>勞退投保級距</font>" HtmlEncode="false" DataFormatString="{0:N0}" />
        <asp:BoundField DataField="SalaryItem16" HeaderText="勞退公司負擔金額" DataFormatString="{0:N0}" />    
    </Columns>
    </asp:GridView>
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" Width="100%">
    <Columns>
    <asp:BoundField DataField="YMTotal" HeaderText="本月薪資總額" DataFormatString="{0:N0}" />
    <asp:BoundField DataField="IncomeAmount" HeaderText="本月受雇者投保總額" DataFormatString="{0:N0}" />
    <asp:BoundField DataField="HI2" HeaderText="雇主應繳納補充保費" DataFormatString="{0:N0}" />
    </Columns>
    </asp:GridView>
</asp:Content>
