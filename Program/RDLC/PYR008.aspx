<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYR008.aspx.cs" Inherits="PYR008" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc5" %>

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


    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    <%--頁面開始 --%>
    
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="勞健保證明書" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
                <ul>
       
            <li style=" text-align:left"><!-- 搜尋模組 -->
                <uc9:SearchList id="SearchList1" runat="server" />   
            </li>

            <li style=" text-align:left">
                <span>計薪年度：民國 </span><asp:DropDownList ID="SalaryY" runat="server" Width="50" AutoPostBack="true" OnSelectedIndexChanged="YearChange"/><span>年</span>
            </li>

            <li>
                <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
            </li>

            <li>
                <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label>
            </li>

            <li>
                <asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px"><br />查無資料!!</asp:Panel>
            </li>

            <li>
                <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="400px" width="100%">
                <LocalReport ReportPath="RDLC\PYR008.rdlc"></LocalReport>
                </rsweb:reportviewer>
            </li>
        </ul>

     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>