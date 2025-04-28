<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LeaveTypeBasic.aspx.cs" Inherits="LeaveTypeBasic" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
 <script language="javascript" type="text/javascript" ></script>

<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="假別基本資料檔" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------版面上方搜尋部分區開始------------------------------%>
 <tr>
 <td align="left"><span class="ItemFontStyle">公　　司：</span>
 <uc1:CompanyList id="CompanyList1" runat="server" ></uc1:CompanyList>
 </tr>
<tr>
<td align="left"><span class="ItemFontStyle">假別代碼：</span>
<asp:TextBox id="txtLeave_Id" runat="server" MaxLength="50" ></asp:TextBox>
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
 
 DataKeyNames="Company,Leave_Id" 
 
 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowUpdated="GridView1_RowUpdated" 
 OnRowUpdating="GridView1_RowUpdating" 
 OnRowCancelingEdit="GridView1_RowCancelingEdit" 
 OnRowEditing="GridView1_RowEditing" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 OnRowDeleted="GridView1_RowDeleted" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------欄位水平對齊設定--------%>
<RowStyle horizontalalign="Center"></RowStyle>


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
L2PK='<%# DataBinder.Eval(Container, "DataItem.Leave_Id")%>'

CausesValidation="False">
</asp:LinkButton> 

</ItemTemplate>
<HeaderStyle width="20px"></HeaderStyle>
</asp:TemplateField>

<%--------編輯按鈕設定------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="編輯">

<HeaderStyle width="20px"></HeaderStyle>
<ItemStyle width="20px"></ItemStyle>
</asp:CommandField>



<%--------要顯示出的欄位設定------%>
<asp:BoundField DataField="Leave_Id"            HeaderText="假別代碼"       SortExpression="Leave_Id"           ReadOnly="true">    </asp:BoundField>
<asp:BoundField DataField="Leave_Desc"          HeaderText="假別名稱"       SortExpression="Leave_Desc"         >                   </asp:BoundField>
<asp:BoundField DataField="SalaryType"          HeaderText="薪別"           SortExpression="SalaryType"         >                   </asp:BoundField>
<asp:BoundField DataField="Pay_days"            HeaderText="計薪天數"       SortExpression="Pay_days"           >                   </asp:BoundField>
<asp:BoundField DataField="Pay_rate"            HeaderText="計薪比率"       SortExpression="Pay_rate"           >                   </asp:BoundField>
<asp:BoundField DataField="Annual_LeaveDays"    HeaderText="全年可請"       SortExpression="Annual_LeaveDays"   >                   </asp:BoundField>
<asp:BoundField DataField="Attendance"          HeaderText="全勤"           SortExpression="Attendance"         >                   </asp:BoundField>
<asp:BoundField DataField="Company"            HeaderText="公司"       Visible="false"           ReadOnly="true">    </asp:BoundField>

</Columns>
<%--------列的內容設定結束--------%>

<%--------最後一列的水平對齊設定--%>
<FooterStyle horizontalalign="center">  </FooterStyle>
<%--------分頁的水平對齊設定------%>
<PagerStyle horizontalalign="left">     </PagerStyle>

<%--------設定空白資料列----------%>
<EmptyDataTemplate>


<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell">
<td class="paginationRowEdgeLl">新增</td>
<td>假別代碼</td>
<td>假別名稱</td>
<td>薪別</td>
<td>計薪天數</td>
<td>計薪比率</td>
<td>全年可請</td>
<td>全勤</td>
</tr>

<tr>
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:DropDownList runat="server" id="tbAddNew03" ></asp:DropDownList></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" /></td>
<td class="Grid_GridLine"><asp:DropDownList runat="server" id="tbAddNew07" ></asp:DropDownList></td>
</tr>


</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>
 <%------------------------------GridView 設定結束----------------------------------%>
<%-- <tr>
<td align="left"><asp:TextBox ID="Textbox1" TextMode="MultiLine" Width="500px" Height="300px" runat="server"></asp:TextBox></td>
</tr>--%>
 <%------------------------------    ----------------------------------%>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>


<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT LeaveType_Basic.* FROM LeaveType_Basic "

InsertCommand="INSERT INTO LeaveType_Basic(
Company,
Leave_Id,
Leave_Desc,
SalaryType,
Pay_days,
Pay_rate,
Annual_LeaveDays,
Attendance
) 
VALUES (
@Company,
@Leave_Id,
@Leave_Desc,
@SalaryType,
@Pay_days,
@Pay_rate,
@Annual_LeaveDays,
@Attendance
)" 

UpdateCommand="UPDATE LeaveType_Basic SET 
Leave_Desc = @Leave_Desc,
SalaryType = @SalaryType,
Pay_days = @Pay_days, 
Pay_rate = @Pay_rate,
Annual_LeaveDays = @Annual_LeaveDays,
Attendance = @Attendance
WHERE 
Company = @Company And
Leave_Id= @Leave_Id
"
 
DeleteCommand="DELETE FROM LeaveType_Basic WHERE Leave_Id= @Leave_Id ">
      
                 <InsertParameters>
                 <asp:Parameter Name="Company"              />
                    <asp:Parameter Name="Leave_Id"              />
                    <asp:Parameter Name="Leave_Desc"            />
                    <asp:Parameter Name="SalaryType"            />
                    <asp:Parameter Name="Pay_days"              />
                    <asp:Parameter Name="Pay_rate"              />
                    <asp:Parameter Name="Annual_LeaveDays"      />
                    <asp:Parameter Name="Attendance"            />
                 </InsertParameters>
                 
                 <UpdateParameters>
                 <asp:Parameter Name="Company"              />
                    <asp:Parameter Name="Leave_Id"              />
                    <asp:Parameter Name="Leave_Desc"            />
                    <asp:Parameter Name="SalaryType"            />
                    <asp:Parameter Name="Pay_days"              />
                    <asp:Parameter Name="Pay_rate"              />
                    <asp:Parameter Name="Annual_LeaveDays"      />
                    <asp:Parameter Name="Attendance"            />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Leave_Id"                   />
                 </DeleteParameters>       
                          
</asp:SqlDataSource> 
<asp:HiddenField id="UpdataID" runat="server"></asp:HiddenField>
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>