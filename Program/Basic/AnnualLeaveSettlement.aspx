<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AnnualLeaveSettlement.aspx.cs" Inherits="AnnualLeaveSettlement" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="年度特休結算" runat="server"></uc2:StyleTitle> 
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
    <td align="left"><span class="ItemFontStyle">特休年度：</span>
        <uc1:YearList ID="qyYearList" runat="server" />
        <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    </td>
</tr>
 <tr>
    <td align="left"><span class="ItemFontStyle">結算特休天數：</span>
        到職基準日：<asp:TextBox id="txtALYear" runat="server" MaxLength="10" width="60"></asp:TextBox>        
        未休特休轉換方式：<uc8:CodeList ID="CL_ALTrans" runat="server" />
        <asp:Button id="btnSetALDays" onclick="btnSetALDays_Click" runat="server" Text="執行" />
    </td>
</tr>
 <tr>
    <td align="left"><span class="ItemFontStyle">是否在職：</span>
<asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList></td></tr>
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
 
 DataKeyNames="Company,EmployeeId,ALYear" 
 
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
L3PK='<%# DataBinder.Eval(Container, "DataItem.ALYear")%>'
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
<asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="ALYear" HeaderText="特休年度" SortExpression="ALYear" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="ALDays" HeaderText="特休天數" SortExpression="ALDays"></asp:BoundField>
<asp:BoundField DataField="LeaveDays" HeaderText="已休天數" SortExpression="LeaveDays"></asp:BoundField>
<asp:BoundField DataField="LYTransDays" HeaderText="去年轉入天數"   SortExpression="LYTransDays"></asp:BoundField>
<asp:BoundField DataField="ConvertibleDays" HeaderText="今年可轉換天數" SortExpression="ConvertibleDays"></asp:BoundField>
<asp:BoundField DataField="TransOrNot" HeaderText="是否已轉換" SortExpression="TransOrNot"></asp:BoundField>
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
<td>特休年度</td>
<td>特休天數</td>
<td>已休天數</td>
<td>去年轉入天數</td>
<td>今年可轉換天數</td>
<td>是否已轉換</td>
</tr>

<tr>
<td class="Grid_GridLine" align="center"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd"/></td>
<%--員工編號--%>
<td class="Grid_GridLine">
<uc8:CodeList ID="tbAddNew01" runat="server" /></td>
<%--特休年度--%>
<td class="Grid_GridLine">
<uc1:YearList ID="tbAddNew02" runat="server" /></td>
<%--特休天數--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" MaxLength="2" /></td>
<%--已休天數--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" MaxLength="2" /></td>
<%--去年轉入天數--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" MaxLength="2" /></td>
<%--今年可轉換天數--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" MaxLength="2" /></td>
<%--是否已轉換--%>
<td class="Grid_GridLine"><asp:CheckBox runat="server" ID="tbAddNew07"></asp:CheckBox></td>
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
 
SelectCommand="SELECT AnnualLeaveSettlement.* FROM AnnualLeaveSettlement "

InsertCommand="INSERT INTO AnnualLeaveSettlement(
Company,
EmployeeId,
ALYear,
ALDays,
LeaveDays,
LYTransDays,
ConvertibleDays,
TransOrNot
) 
VALUES (
@Company,
@EmployeeId,
@ALYear,
@ALDays,
@LeaveDays,
@LYTransDays,
@ConvertibleDays,
@TransOrNot
)" 

UpdateCommand="UPDATE AnnualLeaveSettlement SET
ALDays = @ALDays,
LeaveDays = @LeaveDays,
LYTransDays = @LYTransDays,
ConvertibleDays = @ConvertibleDays,
TransOrNot = @TransOrNot
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
ALYear = @ALYear"
 
DeleteCommand="DELETE FROM AnnualLeaveSettlement 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
ALYear = @ALYear
">
                 <InsertParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
                    <asp:Parameter Name="ALDays"                 />
                    <asp:Parameter Name="LeaveDays"                 />
                    <asp:Parameter Name="LYTransDays"                   />
                    <asp:Parameter Name="ConvertibleDays"           />
                    <asp:Parameter Name="TransOrNot"   />
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
                    <asp:Parameter Name="ALDays"                 />
                    <asp:Parameter Name="LeaveDays"                 />
                    <asp:Parameter Name="LYTransDays"                   />
                    <asp:Parameter Name="ConvertibleDays"           />
                    <asp:Parameter Name="TransOrNot"   />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"                   />
                    <asp:Parameter Name="EmployeeId"                />
                    <asp:Parameter Name="ALYear"             />
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
