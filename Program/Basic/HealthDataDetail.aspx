<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="HealthDataDetail.aspx.cs" Inherits="Basic_HealthDataDetail" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %> 
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="全民健保資料查詢" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------搜尋條件設定------------------------------%>
<tr>
    <td align="left">
    <span class="ItemFontStyle">公　　司：</span>
    <uc6:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true"/><span>　</span>
    <span class="ItemFontStyle">部　　門：</span>
    <asp:DropDownList ID="DepList1" runat="server" Width="180" AutoPostBack="true"/><span>　</span>
    <span class="ItemFontStyle">員　　工：</span>
    <asp:DropDownList ID="EmployeeIdList1" runat="server" Width="184" AutoPostBack="true"/>
    </td>
</tr>
    
<tr>
    <td align="left">
    <span class="ItemFontStyle">身份証號：</span>
    <asp:TextBox id="IDNo1" runat="server" Width="140" AutoPostBack="true" ></asp:TextBox>
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    <span>　</span>
<%--    <span class="ItemFontStyle">異動類別：</span>
    <asp:RadioButtonList id="TrxType" runat="server"  Width="180"  RepeatLayout="Flow"  AutoPostBack="true" /><span>　</span>--%>
    <span class="ItemFontStyle">生效日期：</span>
    <asp:TextBox id="EffectiveDateSt" runat="server" Width="50" ></asp:TextBox>
    <%--<asp:ImageButton id="btnCalendarSt" SkinID="Calendar1" runat="server" />--%>
    <span>至</span>
    <asp:TextBox id="EffectiveDateEd" runat="server" Width="50" ></asp:TextBox>
    <%--<asp:ImageButton id="btnCalendarEd" SkinID="Calendar1" runat="server"/>--%>
    <asp:ImageButton id="btnQuery2" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    </td>
</tr>


 <%------------------------------版面上方搜尋部分區結束------------------------------%>
 
 <tr><td colspan="2">
 <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label> 
 </td></tr>
 
 <tr><td colspan="2"></td></tr>
 
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td></tr>
 
 <tr><td align="left" colspan="2">
 <asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />查無資料!!</asp:Panel>
 </td></tr>
 
 <%------------------------------GridView 設定開始----------------------------------%>
 <tr><td colspan="2">
 <%--------GridView 屬性設定--------%>
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="false"
 DataKeyNames="Company,EmployeeId" 
 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------欄位水平對齊設定--------%>
<RowStyle horizontalalign="Center"></RowStyle>


<%--------列的內容設定開始--------%>
<Columns>
<asp:BoundField DataField="EmployeeId"  HeaderText="員工"   SortExpression="EmployeeId" >   </asp:BoundField>
<asp:BoundField DataField="DeptId"  HeaderText="部門"   SortExpression="DeptId"   >   </asp:BoundField>
<asp:BoundField DataField="IDNo"  HeaderText="身份証號"   SortExpression="IDNo"   >   </asp:BoundField>
<asp:BoundField DataField="EffectiveDate"  HeaderText="生效日期"   SortExpression="EffectiveDate" ></asp:BoundField>
<asp:BoundField DataField="Insured_amount"  HeaderText="投保金額"   SortExpression="Insured_amount"   >   </asp:BoundField>
<asp:BoundField DataField="Insured_person"  HeaderText="投保人數"   SortExpression="Insured_person"   >   </asp:BoundField>
<asp:BoundField DataField="Suspends"  HeaderText="異動別"   SortExpression="Suspends"   >   </asp:BoundField>
<asp:BoundField DataField="Subsidy_code"  HeaderText="補助碼"   SortExpression="Subsidy_code"   >   </asp:BoundField>
<asp:BoundField DataField="DependentsSuspends"  HeaderText="眷屬停保"   SortExpression="DependentsSuspends"   >   </asp:BoundField>
<asp:BoundField DataField="Subsidy"  HeaderText="補助"   SortExpression="Subsidy"   >   </asp:BoundField>
<asp:BoundField DataField="DependentsName"  HeaderText="眷屬姓名"   SortExpression="DependentsName"   >   </asp:BoundField>
<asp:BoundField DataField="DependentsIDNo"  HeaderText="身份証號"   SortExpression="DependentsIDNo"   >   </asp:BoundField>
<asp:BoundField DataField="BirthDate"  HeaderText="出生日期"   SortExpression="BirthDate"   >   </asp:BoundField>
<asp:BoundField DataField="Dependent_title"  HeaderText="眷屬稱謂"   SortExpression="Dependent_title"   >   </asp:BoundField>
<asp:BoundField DataField="DependentsEffectiveDate"  HeaderText="生效日期"   SortExpression="DependentsEffectiveDate" ></asp:BoundField>

</Columns>
<%--------列的內容設定結束--------%>

<%--------分頁的水平對齊設定------%>
<PagerStyle horizontalalign="left">     </PagerStyle>
<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>
</asp:GridView>
</td></tr>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server"  
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
>      
</asp:SqlDataSource> 
<asp:HiddenField id="HiddenField1" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>