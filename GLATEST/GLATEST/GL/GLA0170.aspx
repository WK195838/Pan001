<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0170.aspx.cs" Inherits="GL_GLA0170" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Src="~/UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
   
<%@ Register src="~/UserControl/CompanyList.ascx" tagname="CompanyList" tagprefix="uc4" %>
   
   
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
 <script type="text/javascript" language="javascript">    
    </script>
     <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>  
    <div id="main" >
      <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="傳票過帳作業" />
      <fieldset>
        <legend>&nbsp;過帳作業&nbsp;</legend>
        <div style=" margin: 0 auto; width:50%;">          
          <table border="0" cellpadding="0" cellspacing="0" >
            <tr>
              <td>
                &nbsp;</td>
              <td style="text-align:right; font-weight:bold;">
                公司別：&nbsp;</td>
              <td>
                &nbsp;<CompanyList:CompanyList ID="DrpCompanyList" runat="server" />
                </td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td style="text-align:right; font-weight:bold;">
                傳票日期：&nbsp;</td>
              <td>
                &nbsp;<asp:TextBox ID="txtPostingDate" runat="server"></asp:TextBox>
                  <%--<asp:ImageButton ID="ibOW_VoncherDate" runat="server" SkinID="Calendar1" />--%>
                  &nbsp;
              </td>
              <td style="font-weight:bold;">
                &nbsp;以前</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td style="text-align:right; font-weight:bold;">
                傳票來源：&nbsp;</td>
            <td><!--//配合傳票查詢改為下拉單--><CodeList:CodeList ID="txtVourSrc" runat="server" /></td>
              <td style="font-weight:bold;">
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td style="text-align:right; font-weight:bold;">
                目前已過帳至&nbsp;</td>
              <td align=center>
                <asp:TextBox ID="txtPostedDate" runat="server"  BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold"></asp:TextBox></td>
              <td style="font-weight:bold;">
               止</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td style="text-align: center" colspan="4">
                &nbsp;<asp:TextBox ID="txtStatusDesc" runat="server" BorderStyle="None" BorderWidth="0px"
                  CssClass="TextAlignLeftBoldBlue" width="100%" style=" color:red"></asp:TextBox></td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
            <tr>
              <td style="font-weight: bold; text-align: center" colspan="4">
                &nbsp;</td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td >
                &nbsp;</td>
            </tr>
            <tr>
              <td style="font-weight: bold; text-align:center" colspan="4">
                <asp:ImageButton ID="imgbtnPosting" runat="server" ImageUrl="~/Image/ButtonPics/ledgerPosting.gif" OnClick="imgbtnPosting_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imgbtnReset" runat="server" ImageUrl="~/Image/ButtonPics/clear_reinput.gif" OnClick="imgbtnReset_Click" />
              </td>
            </tr>
            <tr>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
              <td>
                &nbsp;</td>
            </tr>
          </table>
          </div>
        <asp:TextBox ID="txtTemp" runat="server" Width="500px"></asp:TextBox></fieldset>
        <asp:Panel ID="LoadingPanel" runat="server" BackColor="#ffffff" BorderColor="#0000C0"
            BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Height="150px" Width="300px" 
            style="display:none; font-family:verdana; font-size:12px; font-weight:bold;">
            <span style="color: #ffffff"></span>           
            <br />
            <br />
            &nbsp; &nbsp;<asp:Image ID="imgWorking" ImageUrl="~/Image/PanUI/busy.gif" runat="server" />
            <br />
            <br />
            &nbsp; &nbsp;&nbsp;處理中，請稍候...
            <br />
            <br />
        </asp:Panel>
    </div>
 
</asp:Content>

