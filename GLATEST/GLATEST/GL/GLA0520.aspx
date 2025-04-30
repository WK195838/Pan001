<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/GLA.master"  CodeFile="GLA0520.aspx.cs" Inherits="GLA520" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
   
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">

 <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>

   <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
    
        
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
        ShowUser="false" Title="年底結轉" />
            <fieldset>
                <uc3:StyleHeader ID="StyleHeader1" runat="server" />
                <legend>&nbsp;年底結轉&nbsp;</legend>
                <table style="width: 99%; vertical-align: middle" border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 122px" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 122px; height: 60px;">
                            公司別：</td>
                        <td style="height: 60px">
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
                        <td style="width: 122px; height: 60px;" align="left">
                            結轉年度：</td>
                        <td style="height: 60px" align="left" valign="middle">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCloseYear" runat="server" Width="100px"></asp:TextBox>
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
                        <td style="width: 122px; height: 60px;" valign="middle">
                            上次年結：</td>
                        <td style="height: 60px" valign="middle">
                            &nbsp;<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblLastYear" runat="server"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 122px" colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 122px; height: 25px;" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnClose" runat="server" ForeColor="Red" Text="本年度結轉" Width="123px"
                                        OnClick="btnClose_Click" />&nbsp;
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
    <uc2:StyleFooter ID="StyleFooter2" runat="server" />
            </asp:Content>
  
