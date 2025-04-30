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
//特別控制
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="人事調動資料匯入" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">公　　司：</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>               
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">&nbsp;</span></td>
                        <td align="left">                            
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="確認轉換資料" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="清除轉換資料" /></td>
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
                                <br />查無資料!!</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" AllowPaging="True" AllowSorting="True" PageSize="9000" >
                                 
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工代號" />
                                     <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />

                                    <asp:BoundField DataField="AdjustmentCategory" HeaderText="員工代號" />
                                    <asp:BoundField DataField="EffectiveDate" HeaderText="員工姓名" />
                                    <asp:BoundField DataField="DepCode_F" HeaderText="調動類別" />
                                    <asp:BoundField DataField="DepCode_T" HeaderText="生效日期" />
                                    <asp:BoundField DataField="Title_F" HeaderText="部門（自）１" />
                                    <asp:BoundField DataField="Title_T" HeaderText="部門（至）１" />
                                    <asp:BoundField DataField="Level_F" HeaderText="職等（自）" />
                                    <asp:BoundField DataField="Level_T" HeaderText="職等（至）" />
                                    <asp:BoundField DataField="SalarySystem_F" HeaderText="薪制（自）" />
                                    <asp:BoundField DataField="SalarySystem_T" HeaderText="薪制（至）" />
                                    <asp:BoundField DataField="Class_F" HeaderText="班別（自）" />
                                    <asp:BoundField DataField="Class_T" HeaderText="班別（至）" />
                                    <asp:BoundField DataField="ResignReason" HeaderText="離職原因" />
                                    <asp:BoundField DataField="MasterUpdate" HeaderText="主檔更新" />




	
		
		
		
			
			
			
			
		
		
			
			
		
		



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

