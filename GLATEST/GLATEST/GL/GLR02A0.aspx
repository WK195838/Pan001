<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/GLA.master" CodeFile="GLR02A0.aspx.cs" Inherits="GLR02A0" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
  <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
      
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
      
    <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
      <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
          ShowUser="true" Title="分類帳" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>                   
                    <table cellpadding="1" cellspacing="0" width="99.5%"  class="dialog_body">
                        <tr class="QueryTerm">
                            <td nowrap="nowrap" align="right">
                                <asp:Label ID="Label1" runat="server" Font-Names="細明體" Font-Size="12pt" 
                                    Text="公司別："></asp:Label></td>
                            <td align="left" nowrap="nowrap" width="25%" >
                                <asp:DropDownList ID="Drpcompany" runat="server">
                                </asp:DropDownList></td>
                            <td nowrap="nowrap" align="right"  >
                                <asp:Label ID="Label3" runat="server" Font-Names="細明體" Font-Size="12pt" 
                                    Text="選擇內容："></asp:Label></td>
                            <td align="left" nowrap="nowrap" width="10%" >
                                <asp:DropDownList ID="ddlPrintChoice" runat="server" AutoPostBack="True">
                                    <asp:ListItem Value="1">總分類帳－日計表</asp:ListItem>
                                    <asp:ListItem Value="2">明細分類帳</asp:ListItem>
                                </asp:DropDownList></td>
                            <td align="left" nowrap="nowrap" >
                            </td>
                            <td align="left" nowrap="nowrap" >
                            </td>
                            <td nowrap="nowrap" >                                
                            </td>
                        </tr>
                        <tr class="QueryTerm" >
                            <td align="right" >
                                <asp:Label ID="Label2" runat="server" Font-Names="細明體" Font-Size="12pt" 
                                    Text="會計科目："></asp:Label></td>
                            <td nowrap="nowrap" >
                               <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                               <ContentTemplate>
                                <asp:TextBox ID="wrvFromAcno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                                <asp:ImageButton ID="imgbtnAcctNoStart" runat="server" ImageUrl="~/Image/ButtonPics/Query.gif" />
                                －&nbsp;<asp:TextBox ID="wrvToAcno" runat="server" MaxLength="8" Width="80px"></asp:TextBox><asp:ImageButton ID="imgbtnAcctNoEnd" runat="server" 
                                    ImageUrl="~/Image/ButtonPics/Query.gif" />
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label6" runat="server" Font-Names="細明體" Font-Size="12pt" 
                                    Text="截止日期："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="dtStartDate" runat="server" Width="100px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_SDate" runat="server" SkinID="Calendar1" />--%></td>
                            <td >
                                －</td>
                            <td >
                                <asp:TextBox ID="dtEndDate" runat="server" Width="100px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_EDate" runat="server" SkinID="Calendar1" />--%></td>
                            <td >
                            </td>
                        </tr>
                        <tr class="QueryExecute">
                        <td nowrap="nowrap" >                              
                                </td>
                            <td nowrap="nowrap" >
                                <asp:ImageButton ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif"
                                    OnClick="btnQuery_Click" />&nbsp;
                            </td>
                            <td nowrap="nowrap" >
                                &nbsp;<asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" Visible="false" /></td>
                            <td nowrap="nowrap" >
                                <asp:HiddenField ID="hidacct1" runat="server" />
                                <asp:HiddenField ID="hidacct2" runat="server" />
                                </td>
                            <td nowrap="nowrap" >
                            </td>                            
                            <td nowrap="nowrap">
                              
                            </td>
                            <td nowrap="nowrap" >
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
      <CR:CrystalReportSource ID="Crysource" runat="server">
          <Report FileName="GLR02A0A.rpt">
          </Report>
      </CR:CrystalReportSource>
      <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
          Height="1106px" ReportSourceID="Crysource" ReuseParameterValuesOnRefresh="True"
          Width="751px" Visible="true" />
      
</asp:Content>
