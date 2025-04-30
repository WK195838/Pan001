<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0510.aspx.cs" Inherits="GLA0510"  MasterPageFile="~/GLA.master"%>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>




<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="月底轉結" />
    <fieldset>
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
            <legend>&nbsp;月底結轉&nbsp;</legend>&nbsp;
            <table style="width: 99%;" border="1" cellpadding="0" cellspacing="0">
             <tr>
            <td style="width: 122px" colspan="2">
                &nbsp;</td>
            </tr>
             <tr>
            <td style="width: 122px">
                公司別：</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <CompanyList:CompanyList ID="DrpCompanyList" runat="server" />             
            </td>
            </tr> 
             <tr>
            <td style="width: 122px" colspan="2">
                &nbsp;</td>
           </tr> 
             <tr>
            <td style="width: 122px">
                會計年度：</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                <asp:TextBox ID="txtAcctYear" runat="server" Width="83px"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr> 
             <tr>
            <td style="width: 122px">
                會計期間：</td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                <asp:TextBox ID="txtAcctPeriod" runat="server" Width="125px"></asp:TextBox>
                        (
                <asp:Label ID="lblAcctPeriodBegin" runat="server"></asp:Label>
                        -
                        <asp:Label ID="lblAcctPeriodEnd" runat="server"></asp:Label>
                        )
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr> 
             <tr>
            <td style="width: 122px" colspan="2">
                &nbsp;</td>
            </tr> 
             <tr>
            <td style="width: 122px" colspan="2">
                &nbsp;</td>
            </tr> 
             <tr>
            <td style="width: 122px; height: 18px;">
                上次月結：</td>
            <td style="height: 18px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblLastPeriod" runat="server"></asp:Label>
                (
                <asp:Label ID="lblLastDateBegin" runat="server"></asp:Label>
                -
                <asp:Label ID="lblLastDateEnd" runat="server"></asp:Label>
                )
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr> 
             <tr>
            <td style="width: 122px" colspan="2">
                &nbsp;</td>
           </tr> 
             <tr>
            <td style="width: 122px; height: 25px;" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                <asp:Button ID="btnClose" runat="server" ForeColor="Red" Text="本期結轉" Width="123px" OnClick="btnClose_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr> 
            </table> 
    </fieldset>
    </div>
    <uc3:StyleFooter ID="StyleFooter1" runat="server" />
    </asp:Content>

