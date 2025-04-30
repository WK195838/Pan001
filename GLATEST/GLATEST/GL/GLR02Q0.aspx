<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR02Q0.aspx.cs" Inherits="GLR02Q0" MasterPageFile="~/GLA.master" %>

<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>



<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
          <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false" ShowUser="false" Title="傳票張數統計" />
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<div>					
					<table cellpadding="1" cellspacing="0" width="99.5%" class='dialog_body'>
						<tr class="QueryTerm">
							<td width="84" nowrap="noWrap" style="height: 26px; width: 84px;">
								<asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
							<td nowrap="nowrap" style="height: 26px; width: 212px;">
                                <asp:DropDownList ID="Drpcompany" runat="server" >
                                </asp:DropDownList></td>
							<td width="84" nowrap="noWrap" style="height: 26px">
								<asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計年度："></asp:Label></td>
							<td nowrap="nowrap" style="height: 26px" width="80">
                                <asp:DropDownList ID="DrpAcctYear" runat="server">
                                </asp:DropDownList></td>
							<td nowrap="nowrap" style="height: 26px" width="85">
								<asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計期間："></asp:Label></td>
							<td nowrap="noWrap" width="78" style="height: 26px">
                                <asp:DropDownList ID="DrpAcctperiod" runat="server">
                                    <asp:ListItem>01</asp:ListItem>
                                    <asp:ListItem>02</asp:ListItem>
                                    <asp:ListItem>02</asp:ListItem>
                                    <asp:ListItem>03</asp:ListItem>
                                    <asp:ListItem>04</asp:ListItem>
                                    <asp:ListItem>05</asp:ListItem>
                                    <asp:ListItem>06</asp:ListItem>
                                    <asp:ListItem>07</asp:ListItem>
                                    <asp:ListItem>08</asp:ListItem>
                                    <asp:ListItem>09</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    <asp:ListItem>13</asp:ListItem>
                                </asp:DropDownList></td>
							<td nowrap="nowrap" style="height: 26px" width="100%">
								</td>
						</tr>						
						<tr class="QueryExecute">
							<td colspan="7" nowrap="nowrap" style="height: 30px" width="100%">
								<asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
								&nbsp;<asp:ImageButton Visible="false" ID="btnClear" runat="server"
									ImageUrl="~/Image/PanUI/PanUI_Clear.gif" />
                                &nbsp;
                            </td>
						</tr>
					</table>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
          <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
              EnableDatabaseLogonPrompt="False" ReportSourceID="CryReportSource" ReuseParameterValuesOnRefresh="True" />
          <CR:CrystalReportSource ID="CryReportSource" runat="server">
              <Report FileName="GLR02Q0.rpt">
              </Report>
          </CR:CrystalReportSource>
	
</asp:Content>