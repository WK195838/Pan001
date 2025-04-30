<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR02B0.aspx.cs" MasterPageFile="~/GLA.master" Inherits="GLR02B0" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>


      <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
     <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
          </asp:ScriptManager>
          <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
              ShowUser="false" Title="試算表" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate>
                <div>                   
                    <table cellpadding="1" cellspacing="0" width=100% class="dialog_body">
                        <tr >
                            <td width="12%" align=right >
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
                            <td width="12%" >
                                <asp:DropDownList ID="DrpCompany" runat="server">
                                </asp:DropDownList></td>
                            <td width="7%">
                                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="日期："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="txtBaseDate" runat="server" Width="100px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_BaseDate" runat="server" SkinID="Calendar1" />--%></td>
                        </tr>
                        <tr>
                            <td  >
                            </td>
                            <td  >
                                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" /></td>
                            <td  >
                            <asp:ImageButton Visible="false" ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" /></td>
                            <td >
                            </td>
                        </tr>
                    </table>
                </div>
            </contenttemplate>
        </asp:UpdatePanel>       

        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ReportSourceID="CryReportSource" ReuseParameterValuesOnRefresh="True"  Width="100%" />
          <CR:CrystalReportSource ID="CryReportSource" runat="server">
              <Report FileName="GLR02B0.rpt">
              </Report>
          </CR:CrystalReportSource>
  </asp:Content>
