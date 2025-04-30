<%@ Page Title="�|�p��ظ�ƺ��@" Language="C#" MasterPageFile="../GLADetail.master" AutoEventWireup="true" CodeFile="GLA0410_M.aspx.cs" Inherits="GLA0410_M"
validaterequest="false" EnableEventValidation="false" 
 %>
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

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div >        
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="�|�p��ظ�ƺ��@" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            ���q�s���G</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                        <td   class="rtd">
                         <span style="color: #ff3366">
                            �|�p��ءG</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="TxtAcctNo" runat="server"></asp:TextBox></td>
                        <td  class="rtd">
                         <span style="color: #ff3366">
                            ��ؤ���W�١G</td>
                        <td class="ltd" >
                            <asp:TextBox ID="TxtAcctCName" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td   class="rtd">
                            ��ح^��W�١G</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="TxtAcctEName" runat="server"></asp:TextBox></td>
                        <td   class="rtd">
                         <span style="color: #ff3366">
                            ������O�G</td>
                        <td class="ltd"  >
                            <asp:DropDownList ID="DrpAcctType" runat="server">
                                <asp:ListItem Value="1">��</asp:ListItem>
                                <asp:ListItem Value="2">��</asp:ListItem>
                            </asp:DropDownList></td>
                        <td  class="rtd">
                         <span style="color: #ff3366">
                            ����ݩʡG</td>
                        <td class="ltd" >
                            <asp:DropDownList ID="DrpAcctCtg" runat="server">
                                <asp:ListItem Value="1">��</asp:ListItem>
                                <asp:ListItem Value="2">�U</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td  class="rtd" >
                         <span style="color: #ff3366">
                            ��ص��šG</td>
                        <td class="ltd" >
                            <asp:TextBox ID="txtAcctClass" runat="server"></asp:TextBox></td>
                        <td  class="rtd">
                         <span style="color: #ff3366">
                            �S�w��ءG</td>
                        <td class="ltd" >
                            <asp:TextBox ID="TxtASpecialAcct" runat="server"></asp:TextBox></td>
                        <td  class="rtd" >
                            �R�b�N�X�G</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="txtOffsetID" runat="server"></asp:TextBox></td>
                    </tr>
            <tr>
                <td   class="rtd">
                    �������ߡG</td>
                <td class="ltd"  >
                    <asp:TextBox ID="txtIdx01" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    ��H�O�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtIdx02" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    �������X1�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtIdx03" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td   class="rtd" >
                    �������X2�G</td>
                <td  class="ltd" >
                    <asp:TextBox ID="TxtIdx04" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    ����G</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtIdx05" runat="server"></asp:TextBox></td>
                <td   class="rtd" >
                    �ƶq�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtIdx06" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td  class="rtd" >
                    ��L�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="txtIdx07" runat="server"></asp:TextBox></td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    ��J����X1�G</td>
                <td class="ltd"  >
                    <asp:CheckBox ID="ChkInputctl1" runat="server" /></td>
                <td  class="rtd">
                    ��J����X2�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkInputctl2" runat="server" /></td>
                <td  class="rtd">
                    ��J����X3�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkInputctl3" runat="server" /></td>
            </tr>
            <tr>
                <td  class="rtd">
                    ��J����X4�G</td>
                <td class="ltd"  >
                    <asp:CheckBox ID="ChkInputctl4" runat="server" /></td>
                <td   class="rtd">
                    ��J����X5�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkInputctl5" runat="server" /></td>
                <td   class="rtd">
                    ��J����X6�G</td>
                <td  class="ltd">
                    <asp:CheckBox ID="ChkInputctl6" runat="server" /></td>
            </tr>
            <tr>
                <td  class="rtd">
                    ��J����X7�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkInputctl7" runat="server" /></td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td   class="rtd">
                    �ƧǱ���X1�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtSeqctl1" runat="server"></asp:TextBox></td>
                <td  class="rtd">
                    �ƧǱ���X2�G</td>
                <td  class="ltd" >
                    <asp:TextBox ID="TxtSeqctl2" runat="server"></asp:TextBox></td>
                <td  class="rtd">
                    �ƧǱ���X3�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtSeqctl3" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td  class="rtd">
                    �ƧǱ���X4�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtSeqctl4" runat="server"></asp:TextBox></td>
                <td   class="rtd">
                    �ƧǱ���X5�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtSeqctl5" runat="server"></asp:TextBox></td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td  class="rtd">
                    �p�p����X1�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubtotctl1" runat="server" /></td>
                <td  class="rtd" >
                    �p�p����X2�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubtotctl2" runat="server" /></td>
                <td  class="rtd">
                    �p�p����X3�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubtotctl3" runat="server" /></td>
            </tr>
            <tr>
                <td   class="rtd">
                    �p�p����X4�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubtotctl4" runat="server" /></td>
                <td   class="rtd">
                    �p�p����X5�G</td>
                <td class="ltd" >
                    <asp:CheckBox ID="ChkSubtotctl5" runat="server" /></td>
                <td   class="rtd">
                    �����b�X�G</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtSubledCode" runat="server"></asp:TextBox></td>
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
</asp:Content>

