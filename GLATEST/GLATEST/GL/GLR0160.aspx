<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR0160.aspx.cs" MasterPageFile="~/GLA.master" Inherits="GLR0160" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc4" %>

<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc3" %>

<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
	
	
	
	
	<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
	<%--用於執行等畫面(Begin) --%>
   <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
	<%--用於執行等畫面(End) --%>
	
   <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
	<%--用於執行等畫面(Begin) --%>
	<div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
	</div>
		<uc1:StyleTitle id="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false" ShowUser="false" Title="專案明細表"></uc1:StyleTitle>
		<uc2:StyleHeader id="StyleHeader1" runat="server"></uc2:StyleHeader>	
     
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<div>				
				</div>
				<table border="0" cellpadding="0" cellspacing="0" >
					<tr >
						<td>
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td >                     
                                        公司別：</td>
									<td ><uc4:CompanyList ID="DrpCompany" runat="server" />
                                    </td>
									<td>選擇排序：</td>
									<td align="left"><asp:DropDownList ID="ddlSort"
										runat="server" Height="21px" Width="120px">
										<asp:ListItem Value="1">會計科目</asp:ListItem>
										<asp:ListItem Value="2">傳票日期</asp:ListItem>
									</asp:DropDownList></td>
									
								</tr>
							</table>
						</td>
					</tr>
					<tr class="QueryTerm">
						<td >
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td >會計科目：</td>
									<td >
										<asp:TextBox ID="txtAcctNo1" runat="server" AutoPostBack="True" OnTextChanged="txtAcctNo1_TextChanged"></asp:TextBox>－<asp:TextBox
											ID="txtAcctNo2" runat="server" OnTextChanged="txtAcctNo2_TextChanged"></asp:TextBox><br />
										<asp:TextBox ID="txtAcctNo3" runat="server" OnTextChanged="txtAcctNo3_TextChanged"></asp:TextBox>－<asp:TextBox
											ID="txtAcctNo4" runat="server" OnTextChanged="txtAcctNo4_TextChanged"></asp:TextBox>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr >
						<td >
							<table border="0" cellpadding="0" cellspacing="0" width="100%">
								<tr>
									<td >傳票日期：</td>
									<td >
                                        <asp:TextBox ID="txtVoucherDateS" runat="server" AutoPostBack="True"></asp:TextBox>
                                        <%--<asp:ImageButton ID="ibOW_SDate" runat="server" SkinID="Calendar1" />--%>
                                        ~ 
                                        <asp:TextBox ID="txtVoucherDateE" runat="server"></asp:TextBox>
                                        <%--<asp:ImageButton ID="ibOW_EDate" runat="server" SkinID="Calendar1" />--%></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr >
						<td >
							<table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
								<tr>
									<td ></td>
									<td width=20% >輸入欄位</td>
									<td >起</td>
									<td  >訖</td>
                                    <td >排序</td>
									<td >小計</td>
								</tr>
								<tr>
									<td ><asp:Literal ID="lblTip1" runat="server" Visible="False"></asp:Literal></td>
									<td ><asp:DropDownList ID="ddlTip1"
										runat="server" Height="21px" Width="152px" OnSelectedIndexChanged="ddlTip1_SelectedIndexChanged"
										AutoPostBack="True">
									</asp:DropDownList></td>
									<td>
                                        －<asp:TextBox ID="txtStart1" runat="server" Width="91px"
										></asp:TextBox></td>
									<td >
										<asp:TextBox ID="txtEnd1" runat="server" Width="91px"></asp:TextBox>&nbsp;
                                        <input id="HddDdlTipVal1" runat="server" style="width: 38px" type="hidden" /></td>
                                    <td >
                                        <asp:TextBox ID="TxtSeq1" runat="server" Width="91px"></asp:TextBox></td>
									<td ><asp:CheckBox ID="cbxAmt1" runat="server" /></td>
								</tr>
								<tr>
									<td ><asp:Literal ID="lblTip2" runat="server" Visible="False"></asp:Literal></td>
									<td ><asp:DropDownList ID="ddlTip2"
										runat="server" Height="21px" Width="152px" Enabled="False" OnSelectedIndexChanged="ddlTip2_SelectedIndexChanged"
										AutoPostBack="True">
									</asp:DropDownList></td>
									<td >
                                        －<asp:TextBox ID="txtStart2" runat="server"
										Width="91px" Enabled="False"></asp:TextBox></td>
									<td >
										<asp:TextBox ID="txtEnd2" runat="server" Width="91px" Enabled="False"></asp:TextBox>&nbsp;
                                        <input id="HddDdlTipVal2" runat="server" style="width: 38px" type="hidden" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtSeq2" runat="server" Width="91px"></asp:TextBox></td>
									<td ><asp:CheckBox ID="cbxAmt2" runat="server" Enabled="False" /></td>
								</tr>
								<tr>
									<td ><asp:Literal ID="lblTip3" runat="server" Visible="False"></asp:Literal></td>
									<td ><asp:DropDownList ID="ddlTip3"
										runat="server" Height="21px" Width="152px" Enabled="False" AutoPostBack="True"
										OnSelectedIndexChanged="ddlTip3_SelectedIndexChanged">
									</asp:DropDownList></td>
									<td>
                                        －<asp:TextBox ID="txtStart3" runat="server"
										Width="91px" Enabled="False"></asp:TextBox></td>
									<td >
										<asp:TextBox ID="txtEnd3" runat="server" Width="91px" Enabled="False"></asp:TextBox>&nbsp;
                                        <input id="HddDdlTipVal3" runat="server" style="width: 38px" type="hidden" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtSeq3" runat="server" Width="91px"></asp:TextBox></td>
									<td><asp:CheckBox ID="cbxAmt3" runat="server" Enabled="False" /></td>
								</tr>
								<tr>
									<td class="" style="height: 20px; width: 44px;">&nbsp;<asp:Literal ID="lblTip4" runat="server" Visible="False"></asp:Literal></td>
									<td class="" style="width: 156px; height: 20px"><asp:DropDownList ID="ddlTip4"
										runat="server" Height="21px" Width="152px" Enabled="False" AutoPostBack="True"
										OnSelectedIndexChanged="ddlTip4_SelectedIndexChanged">
									</asp:DropDownList></td>
									<td style="width: 123px; height: 20px">
                                        －<asp:TextBox ID="txtStart4" runat="server"
										Width="91px" Enabled="False"></asp:TextBox></td>
									<td >
										<asp:TextBox ID="txtEnd4" runat="server" Width="91px" Enabled="False"></asp:TextBox>&nbsp;
                                        <input id="HddDdlTipVal4" runat="server"  style="width: 38px" type="hidden" /></td>
                                    <td >
                                        <asp:TextBox ID="TxtSeq4" runat="server" Width="91px"></asp:TextBox></td>
									<td ><asp:CheckBox ID="cbxAmt4" runat="server" Enabled="False" /></td>
								</tr>
								<tr>
									<td ><asp:Literal ID="lblTip5" runat="server" Visible="False"></asp:Literal></td>
									<td><asp:DropDownList ID="ddlTip5"
										runat="server" Height="21px" Width="152px" Enabled="False" AutoPostBack="True"
										OnSelectedIndexChanged="ddlTip5_SelectedIndexChanged">
									</asp:DropDownList></td>
									<td >
                                        －<asp:TextBox ID="txtStart5" runat="server"
										Width="91px" Enabled="False"></asp:TextBox></td>
									<td >
										<asp:TextBox ID="txtEnd5" runat="server" Width="91px" Enabled="False"></asp:TextBox>&nbsp;
                                        <input id="HddDdlTipVal5" runat="server" style="width: 38px" type="hidden" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtSeq5" runat="server" Width="91px"></asp:TextBox></td>
									<td ><asp:CheckBox ID="cbxAmt5" runat="server" Enabled="False" /></td>
								</tr>
							</table>
							<asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" /><asp:ImageButton
								Visible="false" OnClick="btnClear_Click" ID="ImageButton2" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" />
                            </td>
					</tr>
					<tr class="QueryTerm">
						<td style="height: 13px; width: 709px;">
                            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                                ReportSourceID="cryReportSource"  EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" Height="777px" Width="1080px" />
						</td>
					</tr>
				</table>
                <CR:CrystalReportSource ID="cryReportSource" runat="server">
                    <Report FileName="GLR0160.rpt">
                    </Report>
                </CR:CrystalReportSource>
			</ContentTemplate>			
		</asp:UpdatePanel>
       <uc3:StyleFooter id="StyleFooter1" runat="server"></uc3:StyleFooter>
	 </asp:Content>  
