<%@ Page Language="C#"  MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLR01J0.aspx.cs" Inherits=" GLR01J0
" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" Runat="Server">
   <%--用於執行等畫面(Begin) --%>
    <script type="text/javascript" id="s1" src="<%=ResolveUrl("~/Pages/Busy.js") %>"></script>
    <script type="text/javascript" id="s2" src="<%=ResolveUrl("~/Pages/pagefunction.js") %>"></script>
   <%--用於執行等畫面(End) --%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>  
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
        ShowUser="false" Title="傳票列印" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>                    
                    <table cellpadding="1" cellspacing="0" width=40% >
                        <tr  class="QueryStyle" >
                            <td align='right'>
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
                            <td >
                                <uc4:CompanyList ID="DrpCompany" runat="server" EnableViewState="true" /></td>
                           <td >
                               </td>
                            <td >
                                </td>
                        </tr>
                        <tr class="QueryStyle" >
                            <td align='right' >
                                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票日期："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="txtEntrySdate" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_SDate" runat="server" SkinID="Calendar1" />--%></td>
                            <td >
                                －</td>
                            <td >
                                <asp:TextBox ID="txtEntryEdate" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_EDate" runat="server" SkinID="Calendar1" />--%></td>
                        </tr>
                        <tr  class="QueryStyle" >
                            <td  align='right' >
                                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="傳票日期："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="txtVouncherSDate" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_VouncherSDate" runat="server" SkinID="Calendar1" />--%></td>
                            <td>
                                －</td>
                            <td>
                                <asp:TextBox ID="txtVouncherEDate" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_VouncherEDate" runat="server" SkinID="Calendar1" />--%></td>
                        </tr>
                        <tr class="QueryStyle" >
                            <td align='right' >
                                <asp:Label ID="Label7" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="傳票號碼："></asp:Label></td>
                            <td >
                                <asp:TextBox ID="txtVoucherSNo" runat="server" Width="120px" MaxLength="10"></asp:TextBox></td>
                                    <td>
                                －</td>
                            <td >
                                <asp:TextBox
                                    ID="txtVoucherENo" runat="server" Width="120px" MaxLength="10"></asp:TextBox></td>
                        </tr>
                        <tr  class="QueryStyle" >
                            <td align='right' >
                                <asp:Label ID="Label9" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票者："></asp:Label></td>
                           <td >
                               <asp:TextBox ID="VoucherCreatorS" runat="server" Width="120px"></asp:TextBox></td>
                           <td >
                               －</td>
                            <td >
                                <asp:TextBox ID="VoucherCreatorE" runat="server" MaxLength="10" Width="120px"></asp:TextBox></td>
                        </tr>
                         <tr  class="QueryStyle" >
                            <td align='right' >
                                <asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="傳票來源："></asp:Label></td>                           
                            <td><!--//配合傳票查詢改為下拉單--><CodeList:CodeList ID="VoucherSourceS" runat="server" /></td>
                            <td >
                                </td>
                            <td >
                                </td>
                        </tr>
                        <tr  class="QueryStyle">
                            <td >
                                <asp:ImageButton ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif"
                                    OnClick="btnQuery_Click" />
                            </td>
                            <td >
                                <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" Visible="false" /></td>
                                    <td >
                                </td>
                            <td >
                                </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
           
        </asp:UpdatePanel>
    &nbsp;&nbsp;
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ReportSourceID="CryReportSource" />
    <CR:CrystalReportSource ID="CryReportSource" runat="server">
        <Report FileName="GLR01J0.rpt">
        </Report>
    </CR:CrystalReportSource>
        </asp:Content>
  