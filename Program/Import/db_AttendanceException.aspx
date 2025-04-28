<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="db_AttendanceException.aspx.cs" Inherits="db_AttendanceException" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//�S�O����
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="�X�Բ��`��ƶפJ" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">���@�@�q�G</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>    
                    <tr align="left" class="QueryStyle"><td><span class="ItemFontStyle">�p�~�~��G</span></td>
                    <td><uc6:SalaryYM id="SalaryYM1" runat="server" />
                    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />
                    </td></tr>                                   
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">&nbsp;</span></td>
                        <td align="left">                            
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="�T�{�ഫ���" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="�M���ഫ���" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc7:Navigator ID="Navigator1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" 
                                 AllowPaging="True" AllowSorting="True" PageSize="50" 
                                 >
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeName" HeaderText="���u�m�W" />
                                     <asp:BoundField DataField="EmployeeId" HeaderText="���u�s��" />
                                     <asp:BoundField DataField="AttendanceDate" HeaderText="���" />
                                     <asp:BoundField DataField="DeptId" HeaderText="�����N��" />
                                     <asp:BoundField DataField="CardNo" HeaderText="�d��" />
                                     <asp:BoundField DataField="InTime" HeaderText="�W�Z�ɶ�" />
                                     <asp:BoundField DataField="OutTime" HeaderText="�U�Z�ɶ�" />
                                     <asp:BoundField DataField="OverTime" HeaderText="���" />
                                     <asp:BoundField DataField="AEDesc" HeaderText="�X�Բ��`�X" />
                                     <asp:BoundField DataField="Memo" HeaderText="�X�Ԫ��A" />
                                 </Columns>
                             </asp:GridView>
                        </td>
                    </tr>
                </table>
             <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>

