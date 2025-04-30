<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/GLA.master" CodeFile="GLR01G0.aspx.cs" Inherits="GLR01G0" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


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
         ShowUser="false" Title="日計表" />
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
          
            <ContentTemplate>
                <div>
                   
                    <table cellpadding="1" cellspacing="0" width='99%'  class='dialog_body'>
                        <tr class="QueryTerm" >
                            <td align="right" width="13%" >
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
                            <td  align="left">
                                &nbsp;<CompanyList:CompanyList ID="DrpCompanyList" runat="server" /></td>
                        </tr>
                        <tr class="QueryTerm">
                            <td align="right" >
                                <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="日期："></asp:Label>
                            </td>
                            <td>
                                &nbsp;<asp:TextBox ID="txtDateS" runat="server" Width="100px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_StartDate" runat="server" SkinID="Calendar1" />--%>～<asp:TextBox
                                        ID="txtDateE" runat="server" Width="100px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_EndDate" runat="server" SkinID="Calendar1" />--%>
                              
                            </td>
                        </tr>
                        <tr class="QueryTerm">
                            <td align="right" >
                                <asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="選擇內容："></asp:Label></td>
                            <td >
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1">已過帳</asp:ListItem>
                                    <asp:ListItem Value="2">已核准</asp:ListItem>
                                    <asp:ListItem Value="3">全部</asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                        <tr class="QueryTerm">
                            <td align="right" >
                                <asp:Label ID="Label5" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="含追溯分攤："></asp:Label></td>
                            <td >
                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="Y">含</asp:ListItem>
                                    <asp:ListItem Value="N">不含</asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                        <tr class="QueryExecute">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:ImageButton ID="btnQuery" runat="server" 
                                    ImageUrl="~/Image/PanUI/PanUI_Query.gif" OnClick="btnQuery_Click" />
                                <asp:ImageButton Visible="false" ID="btnClear" runat="server"
                                    ImageUrl="~/Image/PanUI/PanUI_Clear.gif" /></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
         EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
         ReportSourceID="cryReportSource" ReuseParameterValuesOnRefresh="True" />
     <CR:CrystalReportSource ID="cryReportSource" runat="server">
         <Report FileName="GLR01G0.rpt">
         </Report>
     </CR:CrystalReportSource>
        </asp:Content>
 