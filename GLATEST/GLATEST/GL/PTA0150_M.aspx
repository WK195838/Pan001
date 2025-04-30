<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTA0150_M.aspx.cs" Inherits="PTA0150_M" validaterequest="false" EnableEventValidation="false" %>

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
    <title>���u�򥻸�ƺ��@</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="�Ȥ�򥻸�ƺ��@" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            ���q�s���G</span></td>
                        <td class="ltd" >                          
                                    <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="False" />
                               
                        </td>
                <td  class="rtd" >
                    <span style="color: #ff3366">�Ȥ�N���G</span></td>
                <td class="ltd" >
                            <asp:TextBox ID="txtCustomerCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
            <tr>
                <td  class="rtd">
                    �Ȥ�²�١G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerSname" runat="server"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    �Ȥ�W�١G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerFname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �o���a�}�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtBilltoAddress" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �e�f�a�}�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtShiptoAddress" runat="server" Width="90%"></asp:TextBox>
                        </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �t�d�H�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtResponser" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �p���H�@�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContact01" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �q�ܤ@�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTelno01" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    �p���H�G�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContact02" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �q�ܤG�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTelno02" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    �ǯu���X�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtFaxno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �ꤺ�~�O�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpFL" runat="server">
                        <asp:ListItem Value="1">�ꤺ</asp:ListItem>
                        <asp:ListItem Value="2">��~</asp:ListItem>
                    </asp:DropDownList>
                  </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td   class="rtd">
                    �I�ڱ���G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtPayment" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �Τ@�s���G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtChopNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �`���q�s���G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtHQCode" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �~�ȭ��s���G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtSalesID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    ��b����G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtStatementDay" runat="server"></asp:TextBox>
                &nbsp;(1~31)</td>
                <td   class="rtd">
                    �^��W�١G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerFname01" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �o���p���G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpInvoiceType" runat="server">
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    �ҵ|�O�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpTaxCode" runat="server">
                        <asp:ListItem Value="1">�O</asp:ListItem>
                        <asp:ListItem Value="">�_</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �Τ@�s���ˬd�X�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpCheckNo" runat="server">
                        <asp:ListItem Value="1">�O</asp:ListItem>
                        <asp:ListItem Value="">�_</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    �H���B�סG</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCreditLimit" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �Ƶ��G</td>
                <td class="ltd" colspan="3" >
                    <asp:TextBox ID="txtRemark" runat="server" Width="80%"></asp:TextBox>
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
      
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
       </div>
      
    </form>
</body>
</html>

