<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="LaborInsuranceSeriesParameter.aspx.cs" Inherits="Basic_LaborInsuranceSeriesParameter" EnableEventValidation="false"  %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
//RangeNo	級數
//LiAmt	月投保金額


</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
&nbsp; <uc2:StyleTitle id="StyleTitle1" title="勞保投保級數參照維護" runat="server"></uc2:StyleTitle><uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
<tr class="QueryStyle"><td align="left"><span class="ItemFontStyle">級數：</span>&nbsp;</td>
<td align="left"><asp:TextBox id="txtRangeNo" runat="server" MaxLength="20" />&nbsp;<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton></td></tr>
<tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>
<tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </td></tr>
<tr><td align="left"colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
 <br />查無資料!!</asp:Panel> </td></tr>
 <tr><td colspan="2"><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="RangeNo" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataSourceID="SDS_GridView" ShowFooter="True">
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");' L1PK='<%# DataBinder.Eval(Container, "DataItem.RangeNo")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>
<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="RangeNo" HeaderText="級數" ReadOnly="True" SortExpression="RangeNo"></asp:BoundField>
<asp:BoundField DataField="LiAmt" HeaderText="月投保金額" SortExpression="LiAmt"></asp:BoundField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>級數</td><td class="paginationRowEdgeRI">月投保金額</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td></tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView>
</td></tr>

<tr>
<td align="left"colspan="2">
</td>
</tr>
</tbody>
</table>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server"
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" 
DeleteCommand="DELETE FROM LaborInsurance_SeriesParameter WHERE RangeNo = @RangeNo" 
SelectCommand="SELECT convert(int,RangeNo) RangeNo,Liamt FROM LaborInsurance_SeriesParameter Where 0=0 "
InsertCommand="INSERT INTO LaborInsurance_SeriesParameter(RangeNo, LiAmt) VALUES (@RangeNo, @LiAmt)" 
UpdateCommand="UPDATE LaborInsurance_SeriesParameter SET LiAmt=@LiAmt WHERE RangeNo = @RangeNo">
                 <DeleteParameters>
                     <asp:Parameter Name="RangeNo" />
                 </DeleteParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="RangeNo" />
                     <asp:Parameter Name="LiAmt" />
                 </UpdateParameters>
                 <InsertParameters>
                     <asp:Parameter Name="RangeNo" />
                     <asp:Parameter Name="LiAmt" />
                 </InsertParameters>
             </asp:SqlDataSource>
             
             
<asp:HiddenField id="hid_IsInsertExit" runat="server"/>
<asp:HiddenField id="Status" runat="server"/>
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label>
<br />
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>
</asp:Content>

