<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CalendarDay.ascx.cs" Inherits="UserControl_CalendarDay" %>
<script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
<asp:GridView ID="gvCalendarDay" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" OnRowCreated="gvCalendarDay_RowCreated" Width="100%">
    <Columns>
        <asp:BoundField DataField="theTime" HeaderText="時間" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="theWeekDay" HeaderText="星期" ItemStyle-Width="20px" ItemStyle-Wrap="false" />        
        <asp:BoundField DataField="DeptId" HeaderText="所屬部門代號" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="DepName" HeaderText="所屬部門" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="EmployeeId" HeaderText="所屬員工代號" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="EmployeeIdName" HeaderText="所屬員工" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="Category" HeaderText="分類代號" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="CategoryDesc" HeaderText="分類" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="Status" HeaderText="狀態代號" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="StatusDesc" HeaderText="狀態" ItemStyle-Width="50px" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="Event" HeaderText="事件" ItemStyle-Width="100%" ItemStyle-Wrap="false" />
        <asp:BoundField DataField="Holiday" HeaderText="假日" ItemStyle-Width="20px" ItemStyle-Wrap="false" />
    </Columns>
</asp:GridView>
