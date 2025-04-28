<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchList.ascx.cs" Inherits="SearchList" %>
<div style=" float:left ;">
<span>公　　司：</span>
<asp:DropDownList ID="CompanyList" runat="server" Width="165px" OnSelectedIndexChanged="CompanyList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
<span></span>
<asp:TextBox ID="ComTb" runat="server" Width="100px" BackColor="#FFFFFF" BorderStyle="Solid" BorderColor="#AAAAAA" BorderWidth="1px" Visible="false"></asp:TextBox>
</div>

<div style=" clear:both" />

<div id="DepartmentsRow" runat="server" style=" float:left ;">
<span>部　　門：</span>
<asp:DropDownList ID="DepartmentsList" runat="server" Width="165px" OnSelectedIndexChanged="DepartmentsList_SelectedIndexChanged" AutoPostBack="True" Enabled="false"></asp:DropDownList>
<span></span>
<asp:TextBox ID="DepTb" runat="server" Width="100px" Enabled="false" BackColor="#CCCCCC" BorderStyle="Solid" BorderColor="#AAAAAA" BorderWidth="1px" Visible="false"></asp:TextBox>
</div>

<div style=" clear:both" />

<div id="EmployeeRow" runat="server" style=" float:left ;">
<span>員　　工：</span>
<asp:DropDownList ID="EmployeeList" runat="server" Width="165px" OnSelectedIndexChanged="EmployeeList_SelectedIndexChanged" AutoPostBack="True" Enabled="false"></asp:DropDownList>
<span></span>
<asp:TextBox ID="EmpTb" runat="server" Width="100px" Enabled="false" BackColor="#CCCCCC" BorderStyle="Solid" BorderColor="#AAAAAA" BorderWidth="1px" Visible="false"></asp:TextBox>
</div>

<div style=" clear:both" />

<asp:HiddenField ID="SCompany" runat="server" />
<asp:HiddenField ID="SDepartment" runat="server" />
<asp:HiddenField ID="SEmployee" runat="server" />