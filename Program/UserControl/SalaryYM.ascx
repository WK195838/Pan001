<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalaryYM.ascx.cs" Inherits="UserControl_SalaryYM" %>
<%@ Register Src="YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="MonthList.ascx" TagName="MonthList" TagPrefix="uc2" %>
<asp:Label ID="labCalKind" runat="server" Text=""></asp:Label>
<uc1:YearList ID="YearList1" runat="server" />
<asp:Label ID="labY" runat="server" Text="年"></asp:Label>
<uc2:MonthList ID="MonthList1" runat="server" />
<asp:Label ID="labM" runat="server" Text="月"></asp:Label>
