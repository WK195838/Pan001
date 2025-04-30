<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="db_PersonnelAdjustment.aspx.cs" Inherits="db_PersonnelAdjustment" %>

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
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="�H�ƽհʸ�ƶפJ" />
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

                                    <asp:BoundField DataField="AdjustmentCategory" HeaderText="���u�N��" />
                                    <asp:BoundField DataField="EffectiveDate" HeaderText="���u�m�W" />
                                    <asp:BoundField DataField="DepCode_F" HeaderText="�հ����O" />
                                    <asp:BoundField DataField="DepCode_T" HeaderText="�ͮĤ��" />
                                    <asp:BoundField DataField="Title_F" HeaderText="�����]�ۡ^��" />
                                    <asp:BoundField DataField="Title_T" HeaderText="�����]�ܡ^��" />
                                    <asp:BoundField DataField="Level_F" HeaderText="¾���]�ۡ^" />
                                    <asp:BoundField DataField="Level_T" HeaderText="¾���]�ܡ^" />
                                    <asp:BoundField DataField="SalarySystem_F" HeaderText="�~��]�ۡ^" />
                                    <asp:BoundField DataField="SalarySystem_T" HeaderText="�~��]�ܡ^" />
                                    <asp:BoundField DataField="Class_F" HeaderText="�Z�O�]�ۡ^" />
                                    <asp:BoundField DataField="Class_T" HeaderText="�Z�O�]�ܡ^" />
                                    <asp:BoundField DataField="ResignReason" HeaderText="��¾��]" />
                                    <asp:BoundField DataField="MasterUpdate" HeaderText="�D�ɧ�s" />




	
		
		
		
			
			
			
			
		
		
			
			
		
		



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

