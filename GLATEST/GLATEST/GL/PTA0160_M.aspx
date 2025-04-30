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
    <title>�t�Ӱ򥻸�ƺ��@</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="�t�Ӱ򥻸�ƺ��@" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            ���q�s���G</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                <td  class="rtd" >
                    <span style="color: #ff3366"> �t�ӥN���G</span></td>
                <td class="ltd" >
                            <asp:TextBox ID="txtVendorID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="rtd"  >
                            �t��²�١G</td>
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
                            �t�ӦW�١G</td>
                        <td class="ltd" colspan="2" >
                            <asp:TextBox ID="txtVendFName" runat="server" Width="80%"></asp:TextBox>
                        </td>
                <td class="ltd" >
                    &nbsp;</td>
                    </tr>
            <tr>
                <td  class="rtd">
                    �t�Ӧa�}1�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtAddess1" runat="server" Width="95%"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    �t�Ӧa�}2�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtAddess2" runat="server" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �t�d�H�G</td>
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
                    �p���H�@�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtContPsn01" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �q�ܤ@�G</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtTel1" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �p���H�G�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtContPsn02" runat="server"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    �q�ܤG�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtTel2" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �ǯu���X�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtFaxN0" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    �ꤺ�~�O�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpFL" runat="server">
                        <asp:ListItem Value="1">�ꤺ</asp:ListItem>
                        <asp:ListItem Value="2">��~</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �C�L���Y�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpPrintTitle" runat="server">
                        <asp:ListItem>Y</asp:ListItem>
                        <asp:ListItem>N</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    �������O�G</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpCheckType" runat="server">
                        <asp:ListItem Value="1">�䲼</asp:ListItem>
                        <asp:ListItem Value="2">����</asp:ListItem>
                        <asp:ListItem Value="3">�q��</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �Ƶ��G</td>
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

