<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Pensionaccounts_Detail.aspx.cs" Inherits="Pensionaccounts_Detail" EnableEventValidation="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %> 
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="員工退休金提撥明細檔" runat="server"></uc2:StyleTitle> 
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
<td align="left"><span class="ItemFontStyle">提撥年月：</span>
<asp:DropDownList ID="YearList" runat="server" Width="79" AutoPostBack="true"/>
<asp:DropDownList ID="MonthsList" runat="server" Width="80" AutoPostBack="true"/>
<asp:Label ID="labTotalActualAmount_C" runat="server" />
</td>

</tr>
<tr><td align="left"><span class="ItemFontStyle">是否在職：</span><asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList></td></tr>
<tr>
<td align="left"><span class="ItemFontStyle">試算查詢：</span>
<asp:Button runat="server" ID="QueryWorking" onclick="QueryWorking_Click" Text="查詢薪資試算之退休金" />
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
 
 DataKeyNames="Company,EmployeeId,salary_month" 
 
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
 AllowSorting="False">

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
L3PK='<%# DataBinder.Eval(Container, "DataItem.salary_month")%>'
CausesValidation="False">
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
<asp:BoundField DataField="EmployeeId"              HeaderText="員工編號"   SortExpression="EmployeeId"    HeaderStyle-Wrap="false"    ReadOnly="true">                  </asp:BoundField>
<asp:BoundField DataField="salary_month"            HeaderText="提撥月份"   SortExpression="salary_month"  HeaderStyle-Wrap="false"    ReadOnly="true">                  </asp:BoundField>
<asp:BoundField DataField="ActualDate"              HeaderText="系統計薪日期"   SortExpression="ActualDate" HeaderStyle-Wrap="false">                   </asp:BoundField>
<asp:BoundField DataField="ActualAmount_S"          HeaderText="員工提撥金額"   SortExpression="ActualAmount_S" HeaderStyle-Wrap="false">                   </asp:BoundField>
<asp:BoundField DataField="ActualAmount_C"          HeaderText="企業提繳金額"   SortExpression="ActualAmount_C" HeaderStyle-Wrap="false">                   </asp:BoundField>
<asp:BoundField DataField="HireDate"                HeaderText="到職日期"   SortExpression="HireDate" HeaderStyle-Wrap="false">                   </asp:BoundField>
<asp:BoundField DataField="ResignDate"              HeaderText="離職日期"   SortExpression="ResignDate" HeaderStyle-Wrap="false">                   </asp:BoundField>
</Columns>
<%--------列的內容設定結束--------%>

<%--------最後一列的水平對齊設定--%>
<FooterStyle horizontalalign="center">  </FooterStyle>
<%--------分頁的水平對齊設定------%>
<PagerStyle horizontalalign="left">     </PagerStyle>

<%--------設定空白資料列----------%>


<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>

<tr><td align="left" colspan="2"></td></tr>
</tbody></table>





<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT Pensionaccounts_Detail.* FROM Pensionaccounts_Detail "

InsertCommand="INSERT INTO Pensionaccounts_Detail(
Company,
EmployeeId,
salary_month,
ActualDate,
ActualAmount_S,
ActualAmount_C
) 
VALUES (
@Company,
@EmployeeId,
@salary_month,
@ActualDate,
@ActualAmount_S,
@ActualAmount_C
)" 

UpdateCommand="UPDATE Pensionaccounts_Detail SET 
ActualDate = @ActualDate,
ActualAmount_S = @ActualAmount_S,
ActualAmount_C = @ActualAmount_C
WHERE 
Company= @Company AND 
EmployeeId= @EmployeeId AND
salary_month = @salary_month"
 
DeleteCommand="DELETE FROM Pensionaccounts_Detail 
WHERE 
Company= @Company AND
EmployeeId= @EmployeeId AND
salary_month = @salary_month
">
                 <InsertParameters>
                    <asp:Parameter Name="Company"/>
                    <asp:Parameter Name="EmployeeId"/>
                    <asp:Parameter Name="salary_month"/>
                    <asp:Parameter Name="ActualDate"/>
                    <asp:Parameter Name="ActualAmount_S"/>
                    <asp:Parameter Name="ActualAmount_C"/>
                 </InsertParameters>
                 
                 <UpdateParameters>
                    <asp:Parameter Name="Company"/>
                    <asp:Parameter Name="EmployeeId"/>
                    <asp:Parameter Name="salary_month"/>
                    <asp:Parameter Name="ActualDate"/>
                    <asp:Parameter Name="ActualAmount_S"/>
                    <asp:Parameter Name="ActualAmount_C"/>
                 </UpdateParameters>
                 
                  <DeleteParameters>
                    <asp:Parameter Name="Company"/>
                    <asp:Parameter Name="EmployeeId"/>
                    <asp:Parameter Name="salary_month"/>
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
