﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/GLA.master" CodeFile="GLR0220.aspx.cs" Inherits="GLR0220" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>


<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

   
    <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    
       <%--用於執行等畫面(Begin) --%>
  <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
         <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="月份損益表" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                  
                    <table cellpadding="1" cellspacing="0" width="99.5%"  class="dialog_body">
                        <tr >
                            <td width="25%" nowrap="noWrap" style="height: 27px">
                                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label>
                                <asp:DropDownList ID="Drpcompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Drpcompany_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td width="150" nowrap="noWrap" style="height: 27px">
                                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計年度："></asp:Label><asp:DropDownList
                                    ID="DrpAcctYear" runat="server">
                                </asp:DropDownList></td>
                            <td nowrap="noWrap" width="150" style="height: 27px">
                                <asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計期間："></asp:Label><asp:DropDownList
                                    ID="DrpAcctMon" runat="server">
                                    <asp:ListItem Value="01">01</asp:ListItem>
                                    <asp:ListItem Value="02">02</asp:ListItem>
                                    <asp:ListItem Value="03">03</asp:ListItem>
                                    <asp:ListItem Value="04">04</asp:ListItem>
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
                            <td nowrap="nowrap" style="height: 27px">
                                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="成本中心："
                                    ForeColor="#FFFFC0"></asp:Label><asp:TextBox ID="tbxDept" runat="server" Width="50px"
                                        Font-Names="新細明體" Font-Size="12pt" Visible="False"></asp:TextBox></td>
                        </tr>
                        <tr >
                            <td width="25%" nowrap="noWrap">
                                <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="報表代碼："></asp:Label>&nbsp;<asp:DropDownList
                                    ID="DrpReportCode" runat="server">
                                </asp:DropDownList></td>
                            <td width="135">
                                <asp:Label ID="Label5" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="含追溯分攤："
                                    Visible="False"></asp:Label>
                               </td>
                            <td width="150">&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr >
                            <td width="25%" nowrap="nowrap" style="height: 27px">
                                <asp:ImageButton OnClick="btnQuery_Click" ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" />&nbsp;
                            </td>
                            <td width="135" nowrap="nowrap" style="height: 27px">&nbsp;<asp:ImageButton Visible="false" ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" /></td>
                            <td nowrap="nowrap" style="height: 27px" width="150">&nbsp;</td>
                            <td nowrap="nowrap" style="height: 27px"></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
      
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
             ReportSourceID="CryReportSource" />
        <CR:CrystalReportSource ID="CryReportSource" runat="server">
            <Report FileName="GLR0220.rpt">
            </Report>
        </CR:CrystalReportSource>
     
 </asp:Content>