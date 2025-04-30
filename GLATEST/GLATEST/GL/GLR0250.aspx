<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR0250.aspx.cs"  MasterPageFile="~/GLA.master" Inherits="GLR0250" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


   
    <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="資產負債表" />
    <%--用於執行等畫面(Begin) --%>
  <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>                  
                    <table cellpadding="1" cellspacing="0" width="99.5%"  class="dialog_body">
                        <tr >
                            <td >
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label>
                              </td>
                            <td >
                                <asp:DropDownList ID="DrpCompany" runat="server" OnSelectedIndexChanged="DrpCompany_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td >                              
                                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="日期："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="Txtdate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_Date" runat="server" SkinID="Calendar1" />--%></td>
                            <td  >
                                <asp:Label ID="Label3" runat="server" Text="報表樣式：" Font-Size="12pt" Font-Names="新細明體"></asp:Label></td>
                            <td >
                                <asp:DropDownList ID="ddlReportStyle" runat="server">
                                    <asp:ListItem Value="1">帳戶式</asp:ListItem>
                                    <asp:ListItem Value="2">報告式</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr >
                            <td >
                                <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="報表代碼："></asp:Label>
                                </td>
                            <td>
                                <asp:DropDownList ID="DrpReportType" runat="server">
                                </asp:DropDownList></td>
                            <td >
                                </td>
                            <td>
                            </td>
                            <td >
                            </td>
                            <td ><asp:Label ID="Label2" runat="server" Font-Names="新細明體"
                                Font-Size="12pt" Text="成本中心：" ForeColor="#FFFFC0"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="tbxDept" runat="server" Width="50px" Font-Names="新細明體" Font-Size="12pt"
                                    Visible="False"></asp:TextBox></td>
                        </tr>
                        <tr >
                            <td >
                                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
                            </td>
                            <td >&nbsp;</td>
                            <td ></td>
                            <td >
                            </td>
                            <td >
                            </td>
                            <td >&nbsp;</td>
                            <td ></td>
                        </tr>
                    </table>
                </div>
                &nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                     EnableParameterPrompt="False"
                    ReportSourceID="Cryrptsource" />
                <CR:CrystalReportSource ID="Cryrptsource" runat="server">
                    <Report FileName="GLR0250A.rpt">
                    </Report>
                </CR:CrystalReportSource>
      
 </asp:Content>
