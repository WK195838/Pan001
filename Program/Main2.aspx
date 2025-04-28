<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Main2.aspx.cs" Inherits="_Main2" Title="Untitled Page" %>
<%@ Register Src="UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc4" %>

<%@ Register Src="UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc2" %>
<%@ Register Src="UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc1" %>
<%--<%@ Register Src="UserControl/AttendanceGridview.ascx" TagName="AttendanceGridview" TagPrefix="uc1" %>--%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <table style="width: 100%; height: 100%">
        <tr>
            <td>
                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="Black"
                    DayNameFormat="Full" Font-Names="Times New Roman" Font-Size="10pt" ForeColor="Black"
                    Height="392px" NextPrevFormat="FullMonth" Width="641px">
                    <SelectedDayStyle BackColor="#CC3333" ForeColor="White" />
                    <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt"
                        ForeColor="#333333" Width="1%" />
                    <TodayDayStyle BackColor="#CCCC99" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <DayStyle Width="14%" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="White" />
                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" ForeColor="#333333"
                        Height="10pt" />
                    <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" ForeColor="White"
                        Height="14pt" />
                </asp:Calendar>
      
</td><td>
            </td>
            <td>
                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                         <ContentTemplate>
                                <uc3:StyleTitle ID="StyleTitle1" runat="server" title="出勤表" />
                    <uc2:StyleContentStart ID="StyleContentStart1" runat="server" />
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<TBODY>    
                <tr><td>
                    <TR><TD colSpan="2"><uc1:Navigator_GV ID="Navigator1" runat="server" /></TD></TR>
<%--<TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
 <br />查無資料!!</asp:Panel> </TD></TR>--%>
 <TR><TD colSpan="2">

<asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="SDS_GridView" AllowPaging="True" AllowSorting="true" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" >
    <Columns>
        <asp:BoundField DataField="Company" HeaderText="公司編號" SortExpression="Company" />
        <asp:BoundField DataField="EmployeeId" HeaderText="員工編號" SortExpression="EmployeeId" />
        <asp:BoundField DataField="AttendanceDate" HeaderText="日期" SortExpression="AttendanceDate" />
        <asp:BoundField DataField="DeptId" HeaderText="部門代號" SortExpression="DeptId" />
        <asp:BoundField DataField="CardNo" HeaderText="卡號" SortExpression="CardNo" />
        <asp:BoundField DataField="InTime" HeaderText="上班時間" SortExpression="InTime" />
        <asp:BoundField DataField="OutTime" HeaderText="下班時間" SortExpression="OutTime" />
        <asp:BoundField DataField="Memo" HeaderText="出勤狀態" SortExpression="Memo" />
    </Columns>
</asp:GridView>
</TD></TR>
<asp:SqlDataSource ID="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
    ></asp:SqlDataSource>
               </td></tr><tr><td align=left colSpan=2>
                   
               </td></tr></TBODY></TABLE>
               <uc4:StyleContentEnd ID="StyleContentEnd1" runat="server" />
                              </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>

