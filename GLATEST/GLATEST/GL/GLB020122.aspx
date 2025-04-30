<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/GLA.master"CodeFile="GLB020122.aspx.cs" Inherits="GLB020122" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

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
<uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
        ShowUser="false" Title="明細帳-科目" />
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>                    
                    <table cellpadding="1" cellspacing="0" width="100%" class='dialog_body'>
                        <tr >
                            <td width=28%>
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label>
                                <uc2:CompanyList ID="DrpCompanyName" runat="server" AutoPostBack="false" TextMode="NumShortName" />
                            </td>
                            <td align=left width=30% >
                                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計科目："></asp:Label>
                                <asp:TextBox ID="TxtAcctno" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox></td>
                            <td >
                                <asp:Label ID="labacctName" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label>
                                </td>
                            <td >
                                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票者："></asp:Label>
                                <asp:TextBox ID="txtcreater" runat="server"></asp:TextBox>&nbsp;
                               </td>
                        </tr>
                        <tr >
                            <td >
                                <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="傳票日期："></asp:Label>
                                <asp:TextBox ID="TxtvoucherSDate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_SDate" runat="server" SkinID="Calendar1" />--%>
                            </td>
                            <td >
                                ~<asp:TextBox ID="TxtvoucherEDate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_EDate" runat="server" SkinID="Calendar1" />--%>
                            </td>
                            <td >
                                &nbsp;</td>
                            <td ></td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Label ID="Label5" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票日期："></asp:Label>
                                <asp:TextBox ID="TxtcreateSDate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_MSDate" runat="server" SkinID="Calendar1" />--%>
                            </td>
                            <td >
                                ~<asp:TextBox ID="TxtcreateEDate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_MEDate" runat="server" SkinID="Calendar1" />--%>
                            </td>
                            <td >
                                &nbsp;</td>
                            <td >
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="DivDisplay" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td >
                                                輸入欄位</td>
                                            <td >
                                                起</td>
                                            <td>
                                                迄</td>
                                            <td>
                                                排序</td>
                                            <td>
                                                小計</td>
                                        </tr>
                                        <tr>
                                            <td >
                                <asp:Label ID="LabTitle1" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label></td>
                                            <td >
                                <asp:TextBox ID="Txtindex1S" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="Txtindex1E" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="txtseq1" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:CheckBox ID="chkindex1" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td >
                                <asp:Label ID="LabTitle2" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label></td>
                                            <td >
                                <asp:TextBox ID="Txtindex2S" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="Txtindex2E" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="txtseq2" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:CheckBox ID="chkindex2" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td >
                                <asp:Label ID="LabTitle3" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label></td>
                                            <td >
                                <asp:TextBox ID="Txtindex3S" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="Txtindex3E" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="txtseq3" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:CheckBox ID="chkindex3" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td >
                                <asp:Label ID="LabTitle4" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label></td>
                                            <td >
                                <asp:TextBox ID="Txtindex4S" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="Txtindex4E" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="txtseq4" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:CheckBox ID="chkindex4" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td >
                                <asp:Label ID="LabTitle5" runat="server" Font-Names="新細明體" Font-Size="12pt"></asp:Label></td>
                                            <td >
                                <asp:TextBox ID="Txtindex5S" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="Txtindex5E" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:TextBox ID="txtseq5" runat="server"></asp:TextBox></td>
                                            <td>
                                <asp:CheckBox ID="chkindex5" runat="server" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr >
                            <td >
                                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
                            </td>
                            <td ></td>
                            <td ></td>
                            <td ></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
   
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
         EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
        ReportSourceID="CryReportSource" />
    <CR:CrystalReportSource ID="CryReportSource" runat="server">
        <Report FileName="GLB020122.rpt">
        </Report>
    </CR:CrystalReportSource>
       
</asp:Content>
