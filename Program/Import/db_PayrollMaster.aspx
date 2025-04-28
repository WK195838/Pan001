<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="db_PayrollMaster.aspx.cs" Inherits="db_PayrollMaster" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc6" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

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
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="�H�Ƹ�ƶפJ" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">���@�@�q�G</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>               
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
                            &nbsp; &nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Panel ID="Panel_Empty" runat="server" Height="50px" Visible="False" Width="250px">
                                <br />�d�L���!!</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" AllowPaging="True" AllowSorting="True" PageSize="9000" >
                                 
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeId" HeaderText="���u�N��" />
                                     <asp:BoundField DataField="EmployeeName" HeaderText="���u�m�W" />
                                     <asp:BoundField DataField="EnglishName" HeaderText="�^��m�W" />
                                     <asp:BoundField DataField="DeptId" HeaderText="�����N��" />
                                     <asp:BoundField DataField="TitleCode" HeaderText="¾�ٽs��" />
                                     <asp:BoundField DataField="Grade" HeaderText="¾���s��" />
                                     <asp:BoundField DataField="Shift" HeaderText="�Z�O�N��" />
                                     <asp:BoundField DataField="Identify" HeaderText="�s��O" />
                                     <asp:BoundField DataField="PayCode" HeaderText="�p�~�N��" />
                                     <asp:BoundField DataField="ResignCode" HeaderText="��¾�X" />
                                     <asp:BoundField DataField="HireDate" HeaderText="��¾���">
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LeaveWithoutPay" HeaderText="�d¾���~��" />
                                     <asp:BoundField DataField="ReHireDate" HeaderText="�_¾���" />
                                     <asp:BoundField DataField="ResignDate" HeaderText="��¾���" />
                                     <asp:BoundField DataField="ObserveExpirationDate" HeaderText="�� �� �� �� ��" />
                                     <asp:BoundField DataField="LstPromotionDate" HeaderText="�̪�հ���" />
                                     <asp:BoundField DataField="LstChangeSalaryDate" HeaderText="�̪���~��" />
                                     <asp:BoundField DataField="LWC" HeaderText="�֩e�|�[�J" />
                                     <asp:BoundField DataField="Union" HeaderText="�u�|�[�J" />
                                     <asp:BoundField DataField="SpecialSeniority" HeaderText="�S�[�~��(���)" />
                                     <asp:BoundField DataField="BloodType" HeaderText="�嫬" />
                                     <asp:BoundField DataField="IDNo" HeaderText="��������" />
                                     <asp:BoundField DataField="IDType" HeaderText="�����ѧO" />
                                     <asp:BoundField DataField="Sex" HeaderText="�ʧO" />
                                     <asp:BoundField DataField="Nationality" HeaderText="���y" />
                                     <asp:BoundField DataField="BirthDate" HeaderText="�X�ͤ��" />
                                     <asp:BoundField DataField="MaritalStatus" HeaderText="�B�ê��p" />
                                     <asp:BoundField DataField="DependentsNum" HeaderText="���i�H��" />
                                     <asp:BoundField DataField="Military" HeaderText="�L��" />
                                     <asp:BoundField DataField="BornPlace" HeaderText="�X�ͦa" />
                                     <asp:BoundField DataField="Addr" HeaderText="�q�T�a�}" />
                                     <asp:BoundField DataField="ResidenceAddr" HeaderText="���y�a�}" />
                                     <asp:BoundField DataField="TEL" HeaderText="�q�T�q��" />
                                     <asp:BoundField DataField="MobilPhone" HeaderText="���No." />
                                     <asp:BoundField DataField="Email" HeaderText="E Mail Address" />
                                     <asp:BoundField DataField="Contact" HeaderText="�s���H" />
                                     <asp:BoundField DataField="Guarantor1" HeaderText="�O���H��" />
                                     <asp:BoundField DataField="Guarantor2" HeaderText="�O���H��" />
                                     <asp:BoundField DataField="Introducer" HeaderText="���ФH" />
                                     <asp:BoundField DataField="ContactTEL" HeaderText="�s���H�q��" />
                                     <asp:BoundField DataField="Guarantor1TEL" HeaderText="�O���H���q��" />
                                     <asp:BoundField DataField="Guarantor2TEL" HeaderText="�O���H���q��" />
                                     <asp:BoundField DataField="IntroducerTEL" HeaderText="���ФH�q��" />
                                     <asp:BoundField DataField="CCN" HeaderText="��������" />
                                     <asp:BoundField DataField="EducationCode" HeaderText="�Ǿ��N�X" />
                                     <asp:BoundField DataField="Rank" HeaderText="¾��" />
                                     <asp:BoundField DataField="ReportDeptId" HeaderText="�`���X�����N��" />
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

