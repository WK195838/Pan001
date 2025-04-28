<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="ImportSalaryGradeRankData.aspx.cs" Inherits="ImportSalaryGradeRankData" %>

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
    <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="薪職與本薪等級匯入" />
    <asp:Label ID="Label1" runat="server" Text="下載範本檔：" Width="100px"></asp:Label>
    薪職等級範本(<asp:HyperLink ID="HyperLink1" runat="server" SkinID="TemplateExcel" NavigateUrl="~/Import/Template/SalaryGrade01.xls" />)
    本薪等級範本(<asp:HyperLink ID="HyperLink2" runat="server" SkinID="TemplateExcel" NavigateUrl="~/Import/Template/SalaryGrade02.xls" />)
    <br />
    (範本中"級數＼職等"之[級數]欄位為不重覆之整數，[職等]欄位為職等代號；職等代號請參照[核薪標準（職等）資料維護]作業之設定)
    <br />  
    <asp:Label ID="Label2" runat="server" Text="匯入設定檔：" Width="100px"></asp:Label>
    <asp:FileUpload ID="FU_SGRData" runat="server" />
    <br />
    <asp:Button id="btnImportGrade" onclick="btnImportGrade_Click" runat="server" Text="匯入薪職等級表" />
    <asp:Button id="btnImportDefault01" onclick="btnImportDefault01_Click" runat="server" Text="恢復薪職等級預設值" />
    <br />    
    <asp:Button id="btnImportBS" onclick="btnImportBS_Click" runat="server" Text="匯入本薪等級表" />
    <asp:Button id="btnImportDefault02" onclick="btnImportDefault02_Click" runat="server" Text="恢復本薪等級預設值" />
    <br />
    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red" Text="Error" Width="575px"></asp:Label>    
</asp:Content>
