<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="ExportHI2.aspx.cs" Inherits="ExportHI2" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc9" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="各類所得(收入)扣繳補充保險費明細申報媒體格式" />
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
    公　　司：<uc9:CompanyList ID="CompanyList1" runat="server" />
    </li>

    <li>
    所得給付起始年月：<uc8:SalaryYM id="SalaryYM1" runat="server" />
    </li>
    
    <li>
    所得給付結束年月：<uc8:SalaryYM id="SalaryYM2" runat="server" />
    </li>

    <li>    
    <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
    <asp:Button ID="btnToTXT" runat="server" Text="匯出" onclick="btnToTXT_Click"/>
    <asp:HyperLink ID="HLMemo" runat="server" NavigateUrl="ReadMe.htm" Text="匯出檔常見問題" Target="_blank" />
    </li>

    <li>
     <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red" />
    </li>

    <li>
    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text="Error" Width="575px"></asp:Label>    
    </li>
    <li>
    </li>
    </ul>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" Width="100%"
     OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
     AllowPaging="True" AllowSorting="True" PageSize="9000" 
     >    
    </asp:GridView>
</asp:Content>
