<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTA0160_M.aspx.cs" Inherits="PTA0160_M" validaterequest="false" EnableEventValidation="false" %>

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
    <title>廠商基本資料維護</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="廠商基本資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                <td  class="rtd" >
                    <span style="color: #ff3366"> 廠商代號：</span></td>
                <td class="ltd" >
                            <asp:TextBox ID="txtVendorID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rtd"  >
                            廠商簡稱：</td>
                        <td class="ltd" >
                            <asp:TextBox ID="txtVendSName" runat="server"></asp:TextBox>
                        </td>
                <td  class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="rtd"  >
                            廠商名稱：</td>
                        <td class="ltd" colspan="2" >
                            <asp:TextBox ID="txtVendFName" runat="server" Width="80%"></asp:TextBox>
                        </td>
                <td class="ltd" >
                    &nbsp;</td>
                    </tr>
            <tr>
                <td  class="rtd">
                    廠商地址1：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtAddess1" runat="server" Width="95%"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    廠商地址2：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtAddess2" runat="server" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    負責人：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtResponsor" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td   class="rtd">
                    聯絡人一：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContPsn01" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    電話一：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtTel1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    聯絡人二：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtContPsn02" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    電話二：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTel2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    傳真號碼：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtFaxN0" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    國內外別：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpFL" runat="server">
                        <asp:ListItem Value="1">國內</asp:ListItem>
                        <asp:ListItem Value="2">國外</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    列印抬頭：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpPrintTitle" runat="server">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    票據類別：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpCheckType" runat="server">
                        <asp:ListItem Value="1">支票</asp:ListItem>
                        <asp:ListItem Value="2">本票</asp:ListItem>
                        <asp:ListItem Value="3">電匯</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    備註：</td>
                <td class="ltd" colspan="3" >
                    <asp:TextBox ID="txtRemark" runat="server" Width="90%"></asp:TextBox>
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

