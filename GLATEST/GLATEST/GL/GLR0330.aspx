<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR0330.aspx.cs" MasterPageFile="~/GLA.master" Inherits="GLR0330" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
	<script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>

	<%--用於執行等畫面(End) --%>

	<%--用於執行等畫面(Begin) --%>
	<div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
	</div>
	<%--用於執行等畫面(End) --%>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
        ShowUser="false" Title="年度比較報表" />
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<div>
					
					<table cellpadding="1" cellspacing="0"  width="99.5%" class='dialog_body'>
						<tr >
							<td>
								<asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
							<td>
                                <uc1:CompanyList ID="Drpcompany" runat="server" AutoPostBack="true" />
								</td>
							<td >
								<asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計年度："></asp:Label></td>
							<td >
                                <asp:DropDownList ID="DrpAcctYear" runat="server">
                                </asp:DropDownList></td>
							<td >
								<asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="報表選擇："></asp:Label></td>
							<td> <asp:DropDownList ID="DrpYearId" runat="server">
                                <asp:ListItem Value="1">1月~6月</asp:ListItem>
                                <asp:ListItem Value="7">7月~12月</asp:ListItem>
                                </asp:DropDownList></td>
							<td >
                                </td>
						</tr>
						<tr >
							<td >
								<asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="報表代碼："></asp:Label>
							</td>
							<td >
                                <asp:DropDownList ID="DrpReportCode" runat="server">
                                </asp:DropDownList></td>
							<td colspan="5">
								<asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" ForeColor="Black"
									Text="成本中心："></asp:Label>
                                <asp:DropDownList ID="DrpDepart" runat="server">
                                </asp:DropDownList></td>
						</tr>
						<tr >
							<td colspan="7" nowrap="nowrap" style="height: 30px" width="100%">
								<asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
								&nbsp;<asp:ImageButton Visible="false" ID="btnClear" runat="server"
									ImageUrl="~/Image/PanUI/PanUI_Clear.gif" />
								<asp:Label ID="Label5" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="含追溯分攤："
									Visible="False"></asp:Label>&nbsp;
								<asp:CheckBox ID="cbIncludeAloc" runat="server" Font-Names="新細明體" Font-Size="12pt"
									Visible="False" /></td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ReportSourceID="CryReportSource" HasToggleGroupTreeButton="False" />
    <CR:CrystalReportSource ID="CryReportSource" runat="server">
        <Report FileName="GLR0330.rpt">
        </Report>
    </CR:CrystalReportSource>
   
	</asp:Content>
