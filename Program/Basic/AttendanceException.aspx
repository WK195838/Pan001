<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AttendanceException.aspx.cs" Inherits="Basic_AttendanceException" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>

    
<script type="text/javascript" language ="javascript">

</script>    
     <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
<uc2:StyleTitle id="StyleTitle1" title="出勤異常查詢表" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>
<TABLE id="table1" cellSpacing="0" cellPadding="0" width="100%">
<TBODY>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        <uc9:SearchList id="SearchList1" runat="server" />
        <div>
        </div>
        <SPAN>指定日期：</SPAN>
        <asp:TextBox id="tbAttendanceDate" runat="server" MaxLength="50" />        
        <br/>
        <SPAN>計薪區間：</SPAN>
        <uc6:SalaryYM id="SalaryYMS" runat="server" />～<uc6:SalaryYM id="SalaryYME" runat="server" />
        <br/>
        <SPAN>出勤狀態：</SPAN>
        <asp:RadioButtonList id="Memo" runat="server" RepeatLayout="Flow"  AutoPostBack="true" RepeatDirection="Horizontal" >
        <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
        <asp:ListItem Value="1">遲到</asp:ListItem>
        <asp:ListItem Value="2">早退</asp:ListItem>
        <asp:ListItem Value="5">忘刷</asp:ListItem>
        <asp:ListItem Value="9">曠職</asp:ListItem>
        </asp:RadioButtonList>
        <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"/>
    </td>
</tr>
<tr>
    <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
</tr>
<TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Width="250px" Height="50px" Visible="False" >
 <br />查無資料!!</asp:Panel> </TD></TR>
 <TR><TD colSpan=2><TR><TD colSpan="2"><uc4:Navigator_GV ID="Navigator1" runat="server" /></TD></TR>
 <TR><TD colSpan="2">
<asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="SDS_GridView" AllowPaging="True" AllowSorting="true" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" >
    <Columns>
        <%--<asp:BoundField DataField="Company" HeaderText="公司編號" SortExpression="Company" />--%>
        <asp:BoundField DataField="EmployeeId" HeaderText="員工編號" SortExpression="EmployeeId" />
        <asp:BoundField DataField="AttendanceDate" HeaderText="日期" SortExpression="AttendanceDate" />
        <asp:BoundField DataField="DeptId" HeaderText="部門代號" SortExpression="DeptId" />
        <asp:BoundField DataField="CardNo" HeaderText="卡號" SortExpression="CardNo" />
        <asp:BoundField DataField="InTime" HeaderText="上班時間" SortExpression="InTime" />
        <asp:BoundField DataField="OutTime" HeaderText="下班時間" SortExpression="OutTime" />
        <asp:BoundField DataField="AEDesc" HeaderText="出勤狀態" SortExpression="AECODE" />
        <asp:BoundField DataField="ShowMeno" HeaderText="備註說明" SortExpression="Memo" />
    </Columns>
</asp:GridView>
</TD></TR>
<asp:SqlDataSource ID="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
    ></asp:SqlDataSource></TD></TR>
 <TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>
 <asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> 
 <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
 <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
          </asp:UpdatePanel>

    </div>
</asp:Content>
