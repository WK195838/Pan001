<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0450_M.aspx.cs" Inherits="GLA0450_M" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Src="~/UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>分類帳碼資料維護</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
    <style type="text/css" id="tdr">
     .rtd
     {
       text-align:right;
     }
     .ltd
     {
     text-align:left;
     
     }
     
          
     </style>     
    
</head>
<body>
    <form id="form1" runat="server">
    <div >        
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="分類帳碼資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                        <td   class="rtd">
                            <span style="color: #ff3366">分類帳碼：</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="txtSubLedCode" runat="server"></asp:TextBox></td>
                        <td  class="rtd">
                         <span style="color: #ff3366">
                            </td>
                        <td class="ltd" >
                            </td>
                    </tr>
                    <tr>
                        <td   class="rtd">
                            說明：</td>
                        <td class="ltd" colspan="3"  >
                            <asp:TextBox ID="txtDescription" runat="server" Width="90%"></asp:TextBox><span style="color: #ff3366"></td>
                        <td  class="rtd">
                         <span style="color: #ff3366">
                            </td>
                        <td class="ltd" >
                            </td>
                    </tr>
            <tr>
                <td  class="rtd">
                    控制碼1：</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubledctl1" runat="server" /></td>
                <td  class="rtd" >
                    控制碼2：</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubledctl2" runat="server" /></td>
                <td  class="rtd">
                    控制碼3：</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubledctl3" runat="server" /></td>
            </tr>
            <tr>
                <td   class="rtd">
                    控制碼4：</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubledctl4" runat="server" /></td>
                <td   class="rtd">
                    控制碼5：</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubledctl5" runat="server" /></td>
                <td   class="rtd">
                    </td>
                <td class="ltd" >
                    </td>
            </tr>
            <tr align="center">
                <td colspan="6"  >
                    <asp:ImageButton ID="btnSaveGo" runat="server"
                        SkinID="SG1" OnClick="btnSaveGo_Click" />&nbsp;
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" OnClick="btnSaveExit_Click" />&nbsp;
                    <asp:ImageButton ID="btnCancel" runat="server"
                                SkinID="CC1" /></td>
            </tr>
            <tr align="left">
                <td colspan="6">
                    <asp:Label ID="LabMessage" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>             
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_DepositBank" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
       </div>
      
    </form>
</body>
</html>

