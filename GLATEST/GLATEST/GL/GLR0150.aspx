<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLR0150.aspx.cs" MasterPageFile="~/GLA.master" Inherits="GLR0150" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
	 <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
   </div>
   
    <%--用於執行等畫面(End) --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="沖帳明細表" />
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
      <ContentTemplate>
        <div>         
          <table cellpadding="1" cellspacing="0" width="99.5%" class="dialog_body" >
            <tr >
              <td nowrap="noWrap" >
                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
              <td  nowrap="noWrap"  align="left">
                  <uc1:CompanyList ID="DrpcompanyList" runat="server" AutoPostBack="True" />
               </td>
              <td  nowrap="nowrap" width="85">
                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計科目："></asp:Label></td>
              <td nowrap="noWrap"  align="left" >
                <asp:TextBox ID="wrvFromAcno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                  <asp:ImageButton ID="imgbtnAcctNoFrom" runat="server" 
                      ImageUrl="~/Image/ButtonPics/Query.gif" />
                  <asp:TextBox ID="txtAcctNameF" runat="server" BorderStyle="None" 
                      BorderWidth="0px" CssClass="TextAlignLeftBold"></asp:TextBox>
                </td>
              <td align="left" nowrap="nowrap" >
                －</td>
              <td align="left" nowrap="nowrap">
                <asp:TextBox ID="wrvToAcctno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                  <asp:ImageButton ID="imgbtnAcctNoTo" runat="server" 
                      ImageUrl="~/Image/ButtonPics/Query.gif" />
                </td>
              <td nowrap="nowrap" style="height: 27px" width="100%">
                  <asp:TextBox ID="txtAcctNameT" runat="server" BorderStyle="None" 
                      BorderWidth="0px" CssClass="TextAlignLeftBold"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td nowrap="noWrap" width="85">
                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="截止日期："></asp:Label></td>
              <td width="220" nowrap="noWrap">
                  <asp:TextBox ID="txtDateS" runat="server" CssClass="JQCalendar"></asp:TextBox>
                  </td>
              <td width="85">
                <asp:Label ID="Label3" runat="server" Text="列印選擇：" Font-Size="12pt" Font-Names="新細明體"></asp:Label></td>
              <td width="112">
                <asp:DropDownList ID="ddlPrintChoice" runat="server">
						<asp:ListItem Value="4">沖帳明細表</asp:ListItem>
                  <asp:ListItem Value="1">總額法－明細</asp:ListItem>
                  <asp:ListItem Value="2">總額法－彙總</asp:ListItem>
                  <asp:ListItem Value="3">淨額法</asp:ListItem>
                </asp:DropDownList></td>
            <td >
              </td>
             <td >
              </td>           
                </td>
              <td >
              </td>
              <td >
              </td>
            </tr>
				<tr >
				<td nowrap="noWrap"><asp:Label ID="Label5" runat="server" Font-Size="12pt" Text="發票日期："></asp:Label></td>				
						<td class="style5">                                                              
                        <asp:TextBox ID="TxtvoucherSDate" runat="server" Width="67px" MaxLength="10" CssClass="JQCalendar"></asp:TextBox>～
                        <asp:TextBox ID="TxtvoucherEDate" runat="server" Width="67px" MaxLength="10" CssClass="JQCalendar"></asp:TextBox>                                                         
            </td>
				<td nowrap="noWrap"><asp:Label ID="Label7" runat="server" Font-Size="12pt" Text="發票號碼："></asp:Label></td>				
						<td colspan="2" class="style5">
                        <asp:TextBox ID="InvoiceNoStart" runat="server" Width="90px" MaxLength="10"></asp:TextBox>～
                        <asp:TextBox ID="InvoiceNoEnd" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
            </td>

				<td nowrap="noWrap"><asp:Label ID="Label4" runat="server" Font-Size="12pt" Text="客戶別/廠商別"></asp:Label></td>				
						<td colspan="2" class="style5">                                        
                        <asp:TextBox ID="TxtCustomerName" runat="server" Width="90px" MaxLength="10"></asp:TextBox>                                                         
            </td>
					 </tr>
            <tr class="QueryExecute">
              <td nowrap="nowrap" >
                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
              </td>
              <td  nowrap="nowrap" >
                &nbsp;<asp:ImageButton Visible="false" ID="btnClear" runat="server"
                  ImageUrl="~/Image/PanUI/PanUI_Clear.gif" /></td>
              <td  >
                  &nbsp;</td>
              <td nowrap="nowrap" >                
                  &nbsp;</td>
              <td nowrap="nowrap" >
              </td>
              <td nowrap="nowrap" >
               
              </td>
              <td nowrap="nowrap" >
              </td>
            </tr>
          </table>
        </div>
      </ContentTemplate>     
    </asp:UpdatePanel>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
            EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ReportSourceID="CryReportSource" />
        <CR:CrystalReportSource ID="CryReportSource" runat="server">
            <Report FileName="GLR0150A.rpt">
            </Report>
        </CR:CrystalReportSource>
   </asp:Content>  
