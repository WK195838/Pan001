<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0460_M.aspx.cs" Inherits="GLA0460_M" validaterequest="false" EnableEventValidation="false" %>

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
    <title>���]��ƭȺ��@</title>
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
    <link href="~/App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
    <script language="Javascript" type="text/javascript" src="~/Pages/Busy.js"></script>
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div >  
     <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>      
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title=">���]��ƭȺ��@" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            ���q�s���G</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                <td  class="rtd" >
                    �D�ޮֹ�@�~�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkApprovalCode" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="rtd"  >
                            �֭p������ءG</td>
                        <td class="ltd" >
                            <asp:TextBox ID="txtAccuPLAcctNo" runat="server"></asp:TextBox>
                        </td>
                <td  class="rtd" >
                    �����l�q��ءG</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtPeriodPLAcctNo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
            <tr>
                <td  class="rtd">
                    ����N�X�Шӷ��G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCashCodefrom" runat="server"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    ����N�X�Хγ~�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCashCodeto" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �~��]�w�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpCalendarType" runat="server">
                        <asp:ListItem Value="1">����~</asp:ListItem>
                        <asp:ListItem Value="2">�褸�~</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td   class="rtd">
                    �~�׷|�p��ơG</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpAcctPeriod" runat="server">
                        <asp:ListItem Value="1">�Q�G��</asp:ListItem>
                        <asp:ListItem Value="2">�Q�T��</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    ����榡�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpDateFormat" runat="server">
                        <asp:ListItem Value="1">�~���</asp:ListItem>
                        <asp:ListItem Value="2">���~</asp:ListItem>
                        <asp:ListItem Value="3">���~</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    <span style="color: #ff3366">�w�L�b����G</span></td>
                <td class="ltd" >
                    <asp:TextBox ID="txtLastPostDate" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    <span style="color: #ff3366">�w��������G</span></td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCloseYM" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                   <span style="color: #ff3366"> �w����~�סG</span></td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCloseYYYY" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
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

