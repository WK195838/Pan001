<%@ Page Language="C#"  MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLI01B0.aspx.cs" Inherits="Template_WSingle" %>

<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>

<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc3" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc2" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"  Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language="javascript" type="text/jscript">

    function wop(tValue) {
        __doPostBack('SetSessionPostBack', tValue);
    }
 </script>
   
        <div>
            <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false" ShowUser="false" Title="�ǲ��d�ߧ@�~" />
            <table cellpadding="1" cellspacing="0" width="99.6%">
                <tr class="QueryTerm">
                    <td colspan="3" id="QCdt" runat="server">
                     <fieldset>
        <legend>&nbsp;�ǲ��d�ߧ@�~&nbsp;</legend>                       
                        
                     
                        <table border="1" cellpadding="0" cellspacing="0" style="width: 99%" class='dialog_body'>
                            <tr>
                                <td >
                                    ���q�O�G</td>
                                <td >
                                    <uc3:CompanyList ID="DrpCompany" runat="server" EnableViewState="true" />
                                    <asp:Label ID="lblComp" runat="server" Visible="False"></asp:Label></td>
                                <td colspan="2" style="width: 85px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td >
                                    �ǲ����X�G</td>
                                <td >
                                    <asp:TextBox ID="txtVourNoS" runat="server" Width="130px"></asp:TextBox>��<asp:TextBox
                                        ID="txtVourNoE" runat="server" Width="130px"></asp:TextBox></td>
                                <td colspan="2" style="width: 85px; height: 23px">
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    �s������G</td>
                                <td >
                                    <asp:TextBox ID="txtCreateDateS" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_CreateSDate" runat="server" SkinID="Calendar1" />--%>��<asp:TextBox
                                        ID="txtCreateDateE" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_CreateEDate" runat="server" SkinID="Calendar1" />--%></td>
                                <td >
                                    �s���H�G</td>
                                <td>
                                    <asp:TextBox ID="txtCreateUser" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td >
                                    �ǲ�����G</td>
                                <td >
                                    <asp:TextBox ID="txtVourDateS" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_VoncherSDate" runat="server" SkinID="Calendar1" />--%>��<asp:TextBox
                                        ID="txtVourDateE" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_VoncherEDate" runat="server" SkinID="Calendar1" />--%></td>
                                <td >
                                    �ǲ��ӷ��G</td>
                                <td>
                                    <%--�쬰TextBox�A��2013/08/16��אּ�U�Գ�DropDownList--%>
                                    <%--<asp:TextBox ID="txtVourSrc" runat="server"></asp:TextBox>--%>
                                    <asp:DropDownList ID="txtVourSrc" runat="server">
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    �j�����G</td>
                                <td style="width: 333px">
                                    <asp:TextBox ID="TxtrevSDate" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_RevSDate" runat="server" SkinID="Calendar1" />--%>��<asp:TextBox
                                        ID="TxtrevEDate" runat="server" Width="100px"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibOW_RevEDate" runat="server" SkinID="Calendar1" />--%></td>
                                <td >
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                        <asp:ImageButton ID="btnQuery" runat="server" ImageUrl="~/Image/PanUI/PanUI_Query.gif" OnClick="btnQuery_Click" />
                        <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/Image/PanUI/PanUI_Clear.gif" OnClick="btnClear_Click" />
                        <asp:ImageButton ID="ExportExcel" runat="server" ImageUrl="~/Image/PanUI/PanUI_ExportExcel.gif" Visible="False" /></td>
                            </tr>
                        </table>
                         <asp:ScriptManager ID="ScriptManager1" runat="server">
                         </asp:ScriptManager>
                        </fieldset>
                    </td>
                </tr>
                <tr runat="server" class="QueryExecute">
                    <td colspan="3" nowrap="nowrap" >
                    <fieldset><legend>&nbsp;�d�ߵ��G&nbsp;</legend> 
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <uc2:navigator_gv id="NavigatorPager" runat="server">
</uc2:navigator_gv>
                                <uc1:styletitle id="GLA0140Title" runat="server" showhome="false" showlogout="false"
                                    title="�ǲ��d�ߧ@�~">
</uc1:styletitle>
                                <asp:GridView ID="GridVouncher" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CellPadding="4" DataKeyNames="VoucherNo" EmptyDataText="�L���"
                                    GridLines="None"
                                    PageSize="20" Width="100%" OnRowDataBound="GridVouncher_RowDataBound" 
                                    >
                                    <Columns>
                                        <asp:BoundField HeaderText="���q�O" DataField="Company" />
                                        <asp:BoundField DataField="VoucherNo" HeaderText="�ǲ����X" />
                                        <asp:BoundField DataField="VoucherEntryDate" HeaderText="�s�����" />
                                        <asp:BoundField DataField="VoucherDate" HeaderText="�ǲ����" />
                                        <asp:BoundField DataField="VoucherOwner" HeaderText="�s���H" />
                                        <asp:BoundField HeaderText="�ǲ��ӷ�" DataField="VoucherSource" />
                                        <asp:BoundField HeaderText="�D�ޮֹ�@�~" DataField="ApprovalCode" />
                                        <asp:BoundField HeaderText="�L�b���O" DataField="PostCode" />
                                        <asp:BoundField DataField="RevDate" HeaderText="�j����" />
                                        <asp:BoundField HeaderText="�@�o���O" DataField="DletFlag" />
                                    </Columns>
                                    <HeaderStyle CssClass="button_bar_cell" HorizontalAlign="Center" />
                                    <PagerSettings Visible="False" />
                                </asp:GridView>
                                <asp:HiddenField ID="hidAll" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        </fieldset>
                       
                    </td>
                </tr>
            </table>            
        </div>   
    </asp:Content>


