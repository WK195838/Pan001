<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="ImportTimeClock.aspx.cs" Inherits="Basic_ImportTimeClock" %>




<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc6" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
     <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="卡鐘資料上傳" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">     </asp:UpdatePanel>
    <ContentTemplate>
    <ul>
    <li>
    <asp:Label ID="Label1" runat="server" Text="公　　司：" Width="65px"></asp:Label>
    <uc1:CompanyList ID="CompanyList1" runat="server" />
    </li>

    <li>
    <asp:Label ID="Label2" runat="server" Text="卡鐘資料：" Width="65px"></asp:Label>
    <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
    </li>

    <li>
    <center><asp:Button ID="bunOK" runat="server" OnClick="bunOK_Click" Text="上傳" /></center>
    </li>

    <li>
    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text="Error" Width="575px"></asp:Label><br />    
    </li>

    </ul>
    </ContentTemplate>
    <asp:HyperLink ID="AE_alert" runat="server" NavigateUrl="~\Basic\AttendanceException.aspx" Target="_blank" visible="false">請使用出勤異常總結</asp:HyperLink><span
    <br /> 
    <asp:Label ID="Label3" runat="server" Text="下載範本檔：" Width="100px"></asp:Label>
    卡鐘資料範本(<asp:HyperLink ID="HyperLink2" runat="server" SkinID="TemplateExcel" NavigateUrl="Template/TimeClock.xls" Target="_blank" />)
    <br />
</asp:Content>
