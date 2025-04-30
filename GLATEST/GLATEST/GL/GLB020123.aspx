<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLB020123.aspx.cs" MasterPageFile="~/GLA.master" Inherits="GLR0150" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <%--用於執行等畫面(Begin) --%>
    <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/ModPopFunction.js"></script>  
    <%--用於執行等畫面(End) --%><%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
   </div>
   
    <%--用於執行等畫面(End) --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="明細帳-區間" />
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
      <ContentTemplate>
        <div>         
          <table cellpadding="1" cellspacing="0" width="99.5%" class="dialog_body" >
            <tr >
              <td nowrap="noWrap" align=right width=15%  >
                <asp:Label ID="Label1" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="公司別："></asp:Label></td>
              <td  nowrap="noWrap"  align="left" width=18%>
                  <uc1:CompanyList ID="DrpcompanyList" runat="server" AutoPostBack="True" />
               </td>
              <td  >
                </td>
              <td nowrap="noWrap"  align="left" >
                </td>
              <td align="left" nowrap="nowrap" >
                </td>
              <td align="left" nowrap="nowrap">
                </td>
            </tr>
              <tr>
                  <td nowrap="nowrap">
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
                  <td nowrap="nowrap" >
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
              </tr>
              <tr>
                  <td nowrap="nowrap" align=right>
                <asp:Label ID="Label2" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="會計科目："></asp:Label></td>
                  <td align="left" nowrap="nowrap" width=10%>
                <asp:TextBox ID="wrvFromAcno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                      <asp:ImageButton ID="imgbtnAcctNo" runat="server" 
                          ImageUrl="~/Image/ButtonPics/Query.gif" />
                      <asp:TextBox ID="txtAcctName" runat="server" BorderStyle="None" 
                          BorderWidth="0px" CssClass="TextAlignLeftBold" Width="150px"></asp:TextBox>
                  </td>
                  <td nowrap="nowrap" width=2% >
                      －</td>
                  <td align="left" nowrap="nowrap">
                <asp:TextBox ID="wrvToAcctno" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                      <asp:ImageButton ID="imgbtnAcctNoEnd" runat="server" 
                          ImageUrl="~/Image/ButtonPics/Query.gif" />
                      <asp:TextBox ID="txtAcctNameTo" runat="server" BorderStyle="None" 
                          BorderWidth="0px" CssClass="TextAlignLeftBold" Width="150px"></asp:TextBox>
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
                  <td align="left" nowrap="nowrap">
                  </td>
              </tr>
            <tr >
              <td nowrap="noWrap" align=right>
                <asp:Label ID="Label6" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="傳票日期："></asp:Label></td>
              <td  nowrap="noWrap">
                  <asp:TextBox ID="txtDateS" runat="server" Width="100px"></asp:TextBox>
                  <%--<asp:ImageButton ID="ibOW_StartDate" runat="server" SkinID="Calendar1" />--%></td>
              <td>
                  －</td>
              <td>
                  <asp:TextBox ID="txtDateE" runat="server" Width="100px"></asp:TextBox>
                  <%--<asp:ImageButton ID="ibOW_EndDate" runat="server" SkinID="Calendar1" />--%></td>
              <td >
              </td>
              <td >
              </td>
            </tr>
              <tr>
                  <td nowrap="nowrap" align=right >
                      <asp:Label ID="Label3" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票日期："></asp:Label></td>
                  <td nowrap="nowrap" >
                      <asp:TextBox ID="txtMakeDateS" runat="server" Width="100px"></asp:TextBox>
                      <%--<asp:ImageButton ID="ibOW_MakeStartDate" runat="server" SkinID="Calendar1" />--%></td>
                  <td >
                      －</td>
                  <td >
                      <asp:TextBox ID="txtMakeDateE" runat="server" Width="100px"></asp:TextBox>
                      <%--<asp:ImageButton ID="ibOW_MakeEndDate" runat="server" SkinID="Calendar1" />--%></td>
                  <td>
                  </td>
                  <td>
                  </td>
              </tr>
              <tr>
                  <td nowrap="nowrap" align=right  >
                      <asp:Label ID="Label4" runat="server" Font-Names="新細明體" Font-Size="12pt" Text="製票者："></asp:Label></td>
                  <td nowrap="nowrap" >
                      <asp:TextBox ID="txtCreater" runat="server" Width="100px"></asp:TextBox></td>
                  <td >
                  </td>
                  <td >
                  </td>
                  <td>
                  </td>
                  <td>
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
              </td>
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
            <Report FileName="GLB020123.rpt">
            </Report>
        </CR:CrystalReportSource>
   </asp:Content>  
