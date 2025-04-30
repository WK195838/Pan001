<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PTA0170_M.aspx.cs" Inherits="PTA0170_M" validaterequest="false" EnableEventValidation="false" %>

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
    <link href="~/App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div >        
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="員工基本資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="True" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                <td  class="rtd" >
                    <span style="color: #ff3366">員工代號：</span></td>
                <td class="ltd" >
                            <asp:TextBox ID="txtEmployeeID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
            <tr>
                <td  class="rtd">
                    員工姓名：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtEmployeeName" runat="server"></asp:TextBox>
                </td>
                <td  class="rtd" >
                    英文姓名：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtEmployeeEName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    血型：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpBlood" runat="server">
                        <asp:ListItem Value="">-請選擇-</asp:ListItem>
                        <asp:ListItem Value="A">A</asp:ListItem>
                        <asp:ListItem Value="B">B</asp:ListItem>
                        <asp:ListItem Value="O">O</asp:ListItem>
                        <asp:ListItem Value="AB" >AB</asp:ListItem>                       
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    身分證號：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtIDNo" runat="server"></asp:TextBox>
                        </td>
            </tr>
            <tr>
                <td   class="rtd">
                    性別：</td>
                <td class="ltd" >
                    <asp:DropDownList ID="DrpSex" runat="server">
                        <asp:ListItem Value="M">男</asp:ListItem>
                        <asp:ListItem Value="F">女</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td   class="rtd">
                    出生日期：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtBirthday" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_Birthday" runat="server" SkinID="Calendar1" />--%>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    通訊地址：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCommAddress" runat="server" Width="95%"></asp:TextBox>
                  </td>
                <td   class="rtd">
                    戶籍地址：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtPermAddress" runat="server" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    通訊電話：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtCommTEL" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    戶籍電話：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtPermTel" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    聯絡人：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtConctPsn" runat="server"></asp:TextBox>
                </td>
                <td   class="rtd">
                    聯絡人電話：</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtContTel" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    到職日期：</td>
                <td class="ltd" >
                            <asp:TextBox ID="txtOnBoardDate" runat="server"></asp:TextBox>
                                <%--<asp:ImageButton ID="ibOW_onBoard" runat="server" SkinID="Calendar1" />--%>
                </td>
                <td   class="rtd">
                    部門：</td>
                <td class="ltd" >
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="DrpDepartment" runat="server">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

