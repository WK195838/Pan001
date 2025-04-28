<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LeaveTrx.aspx.cs" Inherits="LeaveTrx" EnableEventValidation="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"/>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc2:StyleTitle id="StyleTitle1" title="請假主檔" runat="server"/>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"/>
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
 <%------------------------------版面上方搜尋部分區開始------------------------------%>
 <!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left">
        <uc9:SearchList id="SearchList1" runat="server" />
 
    </td>
</tr>
<!-- 搜尋模組 -->

<tr><td align="left">
<span class="ItemFontStyle">假　　別：</span>
<uc4:CodeList id="CL_LeaveType" runat="server" AutoPostBack="true" />
</td></tr>
 <%------------------------------版面上方搜尋部分區結束------------------------------%>
 
 <tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>
 
 <tr><td colspan="2"></td></tr>
 
 <tr><td align="center"><uc7:Navigator id="Navigator1" runat="server"/></td></tr>
 
 <tr><td align="center"><asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />查無資料，請選擇其他項目或新增</asp:Panel></td></tr>
 
 <%------------------------------GridView 設定開始----------------------------------%>
 <tr><td colspan="2">
 <%--------GridView 屬性設定--------%>
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"
 
 DataKeyNames="Company,EmployeeId,LeaveType_Id,BeginDateTime" 
 
 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowUpdated="GridView1_RowUpdated" 
 OnRowUpdating="GridView1_RowUpdating" 
 OnRowCancelingEdit="GridView1_RowCancelingEdit" 
 OnRowEditing="GridView1_RowEditing" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 OnRowDeleting="GridView1_RowDeleting" 
 OnRowDeleted="GridView1_RowDeleted" 
 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------欄位水平對齊設定--------%>
<RowStyle horizontalalign="Center"/>
<%--------列的內容設定開始--------%>
<Columns>
<%--------刪除按鈕設定------------%>
<asp:TemplateField HeaderText="刪除" ShowHeader="False">
<ItemTemplate>

<asp:LinkButton 
id="btnDelete" 
onclick="btnDelete_Click" 
runat="server" 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
OnClientClick='return confirm("確定刪除?");' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
L3PK='<%# DataBinder.Eval(Container, "DataItem.LeaveType_Id")%>'
L4PK='<%# DataBinder.Eval(Container, "DataItem.BeginDateTime")%>'
CausesValidation="False">
</asp:LinkButton> 
</ItemTemplate>
<HeaderStyle width="20px"/>
</asp:TemplateField>

<%--------編輯按鈕設定------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="編輯">

<HeaderStyle width="20px"/>
<ItemStyle width="20px"/>
</asp:CommandField>



<%--------要顯示出的欄位設定------%>
<asp:BoundField DataField="EmployeeId"              HeaderText="員工編號"       SortExpression="EmployeeId"                 ReadOnly="true"/>
<asp:BoundField DataField="LeaveType_Id"            HeaderText="假別"           SortExpression="LeaveType_Id"               ReadOnly="true"/>
<asp:BoundField DataField="BeginDateTime"           HeaderText="起始日期"       SortExpression="BeginDateTime"              ReadOnly="true"/>
<asp:BoundField DataField="EndDateTime"             HeaderText="終止日期"       SortExpression="EndDateTime"/>                              
<asp:BoundField DataField="hours"                   HeaderText="時數"           SortExpression="hours"/>                                    
<asp:BoundField DataField="days"                    HeaderText="天數"           SortExpression="days"/>                                                                 
<asp:BoundField DataField="Payroll_Processingmonth" HeaderText="計薪年月"   SortExpression="Payroll_Processingmonth"/>                 
</Columns>
<%--------列的內容設定結束--------%>

<%--------最後一列的水平對齊設定--%>
<FooterStyle horizontalalign="center"/>
<%--------分頁的水平對齊設定------%>
<PagerStyle horizontalalign="left"/> 



<%--------設定空白資料列----------%>
<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell">
<td class="paginationRowEdgeLl">新增</td>
<td>員工</td>
<td>假別</td>
<td>起始日期</td>
<td>終止日期</td>
<td>時數</td>
<td>天數</td>
<td>計薪年月</td>
</tr>

<tr>
<!-- 新增按鈕 -->
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<!-- 員工 -->
<td class="Grid_GridLine"><asp:DropDownList id="TB01" runat="server"></asp:DropDownList></td>
<!-- 假別 -->
<td class="Grid_GridLine"><uc4:CodeList id="TB02" runat="server" /></td>
<!-- 起始日期 -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB03" CssClass="JQCalendar" />
<asp:DropDownList id="TB04" runat="server"></asp:DropDownList></td>
<!-- 終止日期 -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB05"   CssClass="JQCalendar" />
<asp:DropDownList id="TB06" runat="server"></asp:DropDownList></td>
<!-- 時數 -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB07" /></td>
<!-- 天數 -->
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="TB08" />
<!-- 計薪月份 -->
<td class="Grid_GridLine">
<asp:DropDownList id="TB09" runat="server"></asp:DropDownList>
<asp:DropDownList id="TB010" runat="server"></asp:DropDownList></td>
</tr>

</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>
 <%------------------------------GridView 設定結束----------------------------------%>

<tr><td align="left" colspan="2"></td></tr>
</tbody></table>


<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
InsertCommand="INSERT INTO Leave_Trx(
Company,
EmployeeId,
LeaveType_Id,
BeginDateTime,
EndDateTime,
hours,
days,
Payroll_Processingmonth
) 
VALUES (
@Company,
@EmployeeId,
@LeaveType_Id,
@BeginDateTime,
@EndDateTime,
@hours,
@days,
@Payroll_Processingmonth
)" 

UpdateCommand="UPDATE Leave_Trx SET 
EndDateTime = @EndDateTime,
hours = @hours,
days = @days,
Payroll_Processingmonth = @Payroll_Processingmonth 
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
LeaveType_Id= @LeaveType_Id AND
(Convert(varchar,BeginDateTime,120) = @BeginDateTime)"
 
DeleteCommand="DELETE FROM Leave_Trx 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
LeaveType_Id= @LeaveType_Id AND
(Convert(varchar,BeginDateTime,120) = @BeginDateTime)
">
             
                 <InsertParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
                    <asp:Parameter Name="EndDateTime"               />
                    <asp:Parameter Name="hours"                     />
                    <asp:Parameter Name="days"                      />
                    <asp:Parameter Name="Payroll_Processingmonth"   />
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
                    <asp:Parameter Name="EndDateTime"               />
                    <asp:Parameter Name="hours"                     />
                    <asp:Parameter Name="days"                      />
                    <asp:Parameter Name="Payroll_Processingmonth"   />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="LeaveType_Id"              />
                    <asp:Parameter Name="BeginDateTime"             />
                 </DeleteParameters>       
                          
</asp:SqlDataSource> 
<asp:HiddenField id="HiddenField1" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
