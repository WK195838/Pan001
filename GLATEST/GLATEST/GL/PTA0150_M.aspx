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
    <title>員工基本資料維護</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="客戶基本資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >                          
                                    <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="False" />
                               
                        </td>
                <td  class="rtd" >
                    <span style="color: #ff3366">客戶代號：</span></td>
                <td class="ltd" >
                            <asp:TextBox ID="txtCustomerCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
            <tr>
                <td  class="rtd">
                    客戶簡稱：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerSname" runat="server"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    客戶名稱：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerFname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    發票地址：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtBilltoAddress" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td   class="rtd">
                    送貨地址：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtShiptoAddress" runat="server" Width="90%"></asp:TextBox>
                        </td>
            </tr>
            <tr>
                <td   class="rtd">
                    負責人：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtResponser" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    聯絡人一：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContact01" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    電話一：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTelno01" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    聯絡人二：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContact02" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    電話二：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTelno02" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    傳真號碼：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtFaxno" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    國內外別：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpFL" runat="server">
                        <asp:ListItem Value="1">國內</asp:ListItem>
                        <asp:ListItem Value="2">國外</asp:ListItem>
                    </asp:DropDownList>
                  </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td   class="rtd">
                    付款條件：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtPayment" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    統一編號：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtChopNo" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    總公司編號：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtHQCode" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    業務員編號：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtSalesID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    對帳日期：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtStatementDay" runat="server"></asp:TextBox>
                &nbsp;(1~31)</td>
                <td   class="rtd">
                    英文名稱：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCustomerFname01" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    發票聯式：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpInvoiceType" runat="server">
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    課稅別：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpTaxCode" runat="server">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="">否</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    統一編號檢查碼：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpCheckNo" runat="server">
                        <asp:ListItem Value="1">是</asp:ListItem>
                        <asp:ListItem Value="">否</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    信用額度：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCreditLimit" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    備註：</td>
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

