<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="db_LeaveType_Basic.aspx.cs" Inherits="db_LeaveType_Basic" %>

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
//疭北
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="安戈蹲" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">そ</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>               
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">&nbsp;</span></td>
                        <td align="left">                            
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="絋粄锣传戈" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="睲埃锣传戈" /></td>
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
                                <br />琩礚戈!!</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" 
                                 AllowPaging="True" AllowSorting="True" PageSize="100" 
                                 EnableModelValidation="True" >
                                 <Columns>
                                     <asp:BoundField DataField="Leave_Id" HeaderText="安腹" />
                                     <asp:BoundField DataField="Leave_Desc" HeaderText="安磞瓃" />
                                     <asp:BoundField DataField="SalaryType" HeaderText="璸羱摸" />
                                     <asp:BoundField DataField="Pay_days" HeaderText="璸羱ぱ计" />
                                     <asp:BoundField DataField="Pay_rate" HeaderText="璸羱ゑㄒ" />
                                     <asp:BoundField DataField="Annual_LeaveDays" HeaderText="ヰぱ计" />
                                     <asp:BoundField DataField="Attendance" HeaderText="琌对" />
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

