<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FindException.ascx.cs" Inherits="UserControl_FindException" %>

<%@ Register Src="Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc1" %>

       
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
    ></asp:SqlDataSource><%--SelectCommand="SELECT AttendanceException.* FROM AttendanceException"--%>
 