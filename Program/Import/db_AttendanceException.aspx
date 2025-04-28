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
//特別控制
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="出勤異常資料匯入" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">公　　司：</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
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
                                 AllowPaging="True" AllowSorting="True" PageSize="50" 
                                 >
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工編號" />
                                     <asp:BoundField DataField="AttendanceDate" HeaderText="日期" />
                                     <asp:BoundField DataField="DeptId" HeaderText="部門代號" />
                                     <asp:BoundField DataField="CardNo" HeaderText="卡號" />
                                     <asp:BoundField DataField="InTime" HeaderText="上班時間" />
                                     <asp:BoundField DataField="OutTime" HeaderText="下班時間" />
                                     <asp:BoundField DataField="OverTime" HeaderText="遲到" />
                                     <asp:BoundField DataField="AEDesc" HeaderText="出勤異常碼" />
                                     <asp:BoundField DataField="Memo" HeaderText="出勤狀態" />
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

