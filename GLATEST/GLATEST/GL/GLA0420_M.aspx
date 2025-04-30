<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0420_M.aspx.cs" Inherits="GLA0420_M" validaterequest="false" EnableEventValidation="false" %>

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
    <title>欄位名稱資料維護</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="欄位名稱資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                        <td   class="rtd">
                         <span style="color: #ff3366">欄位代碼：</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="txtColID" runat="server"></asp:TextBox></td>
                    </tr>
            <tr>
                <td   class="rtd">
                    欄位名稱：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtColName" runat="server"></asp:TextBox></td>
                <td  class="rtd">
                    檢核程式：</td>
                <td  class="ltd" >
                    <asp:TextBox ID="TxtChkPro" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td  class="rtd">
                    傳入參數：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtParmIn" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    </td>
                <td class="ltd" >
                    </td>
            </tr>
            <tr>
                <td  class="rtd">
                    鍵值欄位1：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtKeyFieldName1" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    鍵值欄位2：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtKeyFieldName2" runat="server"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td  class="rtd">
                    資料欄位1：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtDataFieldName1" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    資料欄位2：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtDataFieldName2" runat="server"></asp:TextBox>
                    </td>
            </tr>
            <tr align="center">
                <td colspan="4"  >
                    <asp:ImageButton ID="btnSaveGo" runat="server"
                        SkinID="SG1" OnClick="btnSaveGo_Click" />&nbsp;
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" OnClick="btnSaveExit_Click" />&nbsp;
                    <asp:ImageButton ID="btnCancel" runat="server"
                                SkinID="CC1" /></td>
            </tr>
            <tr align="left">
                <td colspan="4">
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

