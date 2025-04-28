<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="ExportOBTDTABonus.aspx.cs" Inherits="ExportOBTDTABonus" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="匯出公司獎金資料-世華格式" />
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
    公　　司：<uc1:CompanyList ID="CompanyList1" runat="server" />
    </li>
    
    <li>
    獎金名目：<uc8:CodeList ID="CostId" runat="server" />
    </li>

    <li>
    轉帳日期：<asp:TextBox runat="server" ID="tbTransDate" MaxLength="10" />
    </li>

    <li>
    公司備註：<asp:TextBox runat="server" ID="tbCompanyMemo" MaxLength="12" />*.未填則預設為空白,最長可輸入6個中文字/全形字或12個半形字
    </li>

    <li>
    行員備註：<asp:TextBox runat="server" ID="tbBankMemo" MaxLength="12" />*.未填則預設為空格,最長可輸入6個中文字/全形字或12個半形字
    </li>

    <li>
    權　　數：<asp:TextBox runat="server" ID="tbWeights" MaxLength="2" />
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
