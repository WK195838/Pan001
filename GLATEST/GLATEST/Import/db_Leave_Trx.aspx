<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="db_Leave_Trx.aspx.cs" Inherits="db_Leave_Trx" %>

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
//特別控制
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="請假資料匯入" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">公　　司：</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr class="QueryStyle">
                        <td align="left" style="white-space: nowrap"><span class="ItemFontStyle">假　　別：</span>&nbsp;</td>
                        <td align="left">
                            <asp:CheckBoxList ID="cbLeaveType" runat="server" RepeatColumns="10" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </td>
                    </tr>     
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">排　　序：</span>&nbsp;</td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbOrder" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="請假日期" Value="BeginDateTime" Selected="True" />
                            <asp:ListItem Text="員工代號" Value="EmployeeId"  />
                            <asp:ListItem Text="假別" Value="LeaveType_Id"  />
                            </asp:RadioButtonList>
                        </td>
                    </tr>                                   
                    <tr align="left" class="QueryStyle"><td><span class="ItemFontStyle">計薪年月：</span></td>
                    <td><uc6:SalaryYM id="SalaryYM1" runat="server" />
                    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />
                    </td></tr>    
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
                            <uc7:Navigator ID="Navigator1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" 
                             AllowPaging="True" AllowSorting="True" PageSize="50" >                                 
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工編號" />
                                     <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />
                                     <asp:BoundField DataField="Leave_Desc" HeaderText="假別" />
                                     <asp:BoundField DataField="BeginDateTime" HeaderText="起始日期" />
                                     <asp:BoundField DataField="EndDateTime" HeaderText="終止日期" />
                                     <asp:BoundField DataField="DeptId" HeaderText="部門代號" />
                                     <asp:BoundField DataField="hours" HeaderText="時數" />
                                     <asp:BoundField DataField="days" HeaderText="天數" />                                     
                                     <asp:BoundField DataField="Payroll_Processingmonth" HeaderText="薪資處理月份" />
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

