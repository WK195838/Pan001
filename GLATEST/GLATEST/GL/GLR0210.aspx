<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/GLA.master" CodeFile="GLR0210.aspx.cs" Inherits="Template_WebReport" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
     
      
      <%--用於執行等畫面(Begin) --%>
    <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
    <%--用於執行等畫面(End) --%>
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
   </div>
   
    <%--用於執行等畫面(End) --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>      
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="損益表" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>                    
                    <table cellpadding="1" cellspacing="0" width="99.5%"  class="dialog_body">
                        <tr class="QueryTerm">
                            <td width="25%" nowrap="noWrap">
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label>
                                <asp:DropDownList ID="DrpCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DrpCompany_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td nowrap="noWrap" style="width: 271px">
                                <asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計期間："></asp:Label><asp:TextBox
                                    ID="tbxStartPeriod" runat="server" Width="25px" Font-Names="新細明體" Font-Size="12pt"></asp:TextBox>&nbsp;&nbsp;－
                                <asp:TextBox ID="TxtAccendDate" runat="server" Width="100px" Font-Names="新細明體" Font-Size="12pt"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_endDate" runat="server" SkinID="Calendar1" />--%></td>
                            <td nowrap="nowrap">
                                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="成本中心："
                                    ForeColor="#FFFFC0"></asp:Label><asp:TextBox ID="tbxDept" runat="server" Width="50px"
                                        Font-Names="新細明體" Font-Size="12pt" Visible="False"></asp:TextBox></td>
                        </tr>
                        <tr class="QueryTerm">
                            <td width="25%" nowrap="noWrap">
                                <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="報表代碼："></asp:Label>
                                <asp:DropDownList ID="DrpReportCode" runat="server">
                                </asp:DropDownList></td>
                            <td style="width: 271px">
                                <asp:Label ID="Label5" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="含追溯分攤："
                                    Visible="False"></asp:Label>
                                <asp:CheckBox ID="cbIncludeAloc" runat="server" Font-Names="新細明體" Font-Size="12pt"
                                    Visible="False" /></td>
                            <td></td>
                        </tr>
                        <tr class="QueryExecute">
                            <td width="25%" nowrap="nowrap" style="height: 27px">
                                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
                            </td>
                            <td  nowrap="nowrap" style="height: 27px; width: 271px;">&nbsp;<asp:ImageButton Visible="false" ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" /></td>
                            <td ></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
     
                <CR:CrystalReportSource ID="cryrptsrc" runat="server">
                    <Report FileName="GLR0210.rpt">
                    </Report>
                </CR:CrystalReportSource>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                    ReportSourceID="cryrptsrc" Height="1106px" Width="751px"  ReuseParameterValuesOnRefresh="True" />
    </asp:Content>
