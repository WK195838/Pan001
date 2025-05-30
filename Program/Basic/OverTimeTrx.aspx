<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="OverTimeTrx.aspx.cs" Inherits="OverTimeTrx" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="加班主檔" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------版面上方搜尋部分區開始------------------------------%>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left">
        <uc9:SearchList id="SearchList1" runat="server" />
 
    </td>
</tr>
<!-- 搜尋模組 -->
 <tr>
    <td align="left"><span class="ItemFontStyle">加班日期：</span>
        <asp:TextBox id="txtOverTime_Date" runat="server" MaxLength="8" width="60" CssClass="JQCalendar"></asp:TextBox>        
        <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
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
 ShowFooter="true"
 
 DataKeyNames="Company,EmployeeId,OverTime_Date" 
 
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
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------欄位水平對齊設定--------%>
<RowStyle horizontalalign="Center"></RowStyle>


<%--------列的內容設定開始--------%>
<Columns>
<%--------刪除按鈕設定------------%>
<asp:TemplateField HeaderText="刪除" ShowHeader="False" >
<ItemTemplate>

<asp:LinkButton 
id="btnDelete" 
onclick="btnDelete_Click" 
runat="server" 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
OnClientClick='return confirm("確定刪除?");' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
L3PK='<%# DataBinder.Eval(Container, "DataItem.OverTime_Date")%>'
CausesValidation="False" __designer:wfdid="w4">
</asp:LinkButton> 

</ItemTemplate>
<HeaderStyle width="30px"></HeaderStyle>
</asp:TemplateField>

<%--------編輯按鈕設定------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="編輯">

<HeaderStyle width="30px"></HeaderStyle>
<ItemStyle width="30px"></ItemStyle>
</asp:CommandField>


<%--------要顯示出的欄位設定------%>
<asp:BoundField DataField="EmployeeId"              HeaderText="員工"   SortExpression="EmployeeId"                 ReadOnly="true">                  </asp:BoundField>
<asp:BoundField DataField="OverTime_Date"           HeaderText="加班日期"   SortExpression="OverTime_Date"              ReadOnly="true">                  </asp:BoundField>
<asp:BoundField DataField="OverTime1"               HeaderText="平日加班"   SortExpression="OverTime1">                   </asp:BoundField>
<asp:BoundField DataField="OverTime2"               HeaderText="超時加班"   SortExpression="OverTime2">                   </asp:BoundField>
<asp:BoundField DataField="Holiday"                 HeaderText="假日加班"   SortExpression="Holiday">                   </asp:BoundField>
<asp:BoundField DataField="NationalHoliday"         HeaderText="國定假日"   SortExpression="NationalHoliday">                   </asp:BoundField>
<asp:BoundField DataField="Payroll_ProcessingMonth" HeaderText="計薪年月"   SortExpression="Payroll_ProcessingMonth">   </asp:BoundField>
</Columns>
<%--------列的內容設定結束--------%>

<%--------最後一列的水平對齊設定--%>
<FooterStyle horizontalalign="center">  </FooterStyle>
<%--------分頁的水平對齊設定------%>
<PagerStyle horizontalalign="left">     </PagerStyle>

<%--------設定空白資料列----------%>
<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell" align="center">
<td class="paginationRowEdgeLl" style=" width:30px" >新增</td>
<td>員工</td>
<td>加班日期</td>
<td>平日加班</td>
<td>超時加班</td>
<td>假日加班</td>
<td>國定假日</td>
<td>計薪年月</td>
</tr>

<tr>
<td class="Grid_GridLine" align="center"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" style="display:none;"/></td>
<%--員工編號--%>
<td class="Grid_GridLine">
<asp:DropDownList id="tbAddNew00" runat="server"></asp:DropDownList></td>
<%--加班日期--%>
<td class="Grid_GridLine">
<asp:TextBox runat="server" ID="tbAddNew01" /></td>
<%--平日加班--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" Text="0" /></td>
<%--假日加班--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" Text="0" /></td>
<%--國定假日--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" Text="0" /></td>
<%--加班日期--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" Text="0" /></td>
<%--薪資處理月份--%>
<td class="Grid_GridLine"><asp:DropDownList id="tbAddNew06" runat="server"></asp:DropDownList>
                          <asp:DropDownList id="tbAddNew07" runat="server"></asp:DropDownList></td>
</tr>


</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>

<tr><td align="left" colspan="2"></td></tr>
</tbody></table>





<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 

ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT OverTime_Trx.* FROM OverTime_Trx "

InsertCommand="INSERT INTO OverTime_Trx(
Company,
EmployeeId,
OverTime_Date,
OverTime1,
OverTime2,
Holiday,
NationalHoliday,
Payroll_ProcessingMonth,
ApproveDate,
Overtime_pay,
Completed
) 
VALUES (
@Company,
@EmployeeId,
@OverTime_Date,
@OverTime1,
@OverTime2,
@Holiday,
@NationalHoliday,
@Payroll_ProcessingMonth,
GetDate(),
'Y',
'N'
)" 

UpdateCommand="UPDATE OverTime_Trx SET
OverTime1 = @OverTime1,
OverTime2 = @OverTime2,
Holiday = @Holiday,
NationalHoliday = @NationalHoliday,
Payroll_ProcessingMonth = @Payroll_ProcessingMonth
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
(Convert(varchar,OverTime_Date,120) = @OverTime_Date)"
 
DeleteCommand="DELETE FROM OverTime_Trx 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
(Convert(varchar,OverTime_Date,120) = @OverTime_Date)
">
                 <InsertParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="OverTime_Date"             />
                    <asp:Parameter Name="OverTime1"                 />
                    <asp:Parameter Name="OverTime2"                 />
                    <asp:Parameter Name="Holiday"                   />
                    <asp:Parameter Name="NationalHoliday"           />
                    <asp:Parameter Name="Payroll_ProcessingMonth"   />
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="OverTime_Date"             />
                    <asp:Parameter Name="OverTime1"                 />
                    <asp:Parameter Name="OverTime2"                 />
                    <asp:Parameter Name="Holiday"                   />
                    <asp:Parameter Name="NationalHoliday"           />
                    <asp:Parameter Name="Payroll_ProcessingMonth"   />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="OverTime_Date"             />
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
