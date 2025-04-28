<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="HealthInsuranceSeriesParameter.aspx.cs" Inherits="HealthInsuranceSeriesParameter" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<uc2:StyleTitle id="StyleTitle1" title="全民健保級數設定" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 

<table id="table1" cellspacing="0" cellpadding="0" width="100%">

<tbody>
<!--    GridView上方區塊   -->
<tr>
<td align="left">
<span class="ItemFontStyle">級　　　數：</span>
<asp:TextBox id="txtRangeNo" runat="server" MaxLength="50"></asp:TextBox>
<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
</td>
</tr>

<tr　style=" width:100%; margin:0 auto; text-align:center;">
<td>
<span class="ItemFontStyle"> 　投保費率： </span>
<asp:Label ID="Rates" runat="server"></asp:Label>
<span class="ItemFontStyle"> 　公司負擔： </span>
<asp:Label ID="CompanyBurdenRate" runat="server"></asp:Label>
<span class="ItemFontStyle"> 　員工負擔： </span>
<asp:Label ID="EmpBurdenRate" runat="server"></asp:Label>
<span class="ItemFontStyle"> 　投保單位負擔金額含本人及平均眷屬人數： </span>
<asp:Label ID="Comp_burden_Ave" runat="server"></asp:Label>
<span class="ItemFontStyle">人</span>
<span class="ItemFontStyle"> 　員工投保(含本人)上限人數： </span>
<asp:Label ID="EmpBurden_upto" runat="server"></asp:Label>
<span class="ItemFontStyle">人</span>
</td>
</tr>

<!-- 錯誤訊息 -->
<tr><td align="center"><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>

<!-- GridView 換頁Bar -->
<tr><td align="center"><uc7:Navigator id="Navigator1" runat="server"/></td></tr>

<!-- 錯誤訊息 -->
<tr><td align="center" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Height="50px" width="250px" Visible="False"><br />查無資料!!</asp:Panel></td></tr>

<!--    GridView    -->
<tr><td colspan="2">
<!--    GridView 屬性設定    -->
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"

 DataKeyNames="RangeNo"

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
<RowStyle HorizontalAlign="Center"></RowStyle>
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
L2PK='<%# DataBinder.Eval(Container, "DataItem.LiAmt")%>' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.RangeNo")%>' 
CausesValidation="False"  __designer:wfdid="w3">
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

<HeaderStyle width="60px"></HeaderStyle>
<ItemStyle width="60px"></ItemStyle>
</asp:CommandField>

<%--                             新增區段                            --%>

<asp:BoundField DataField="RangeNo" HeaderText="級數"   ReadOnly="true" SortExpression="RangeNo"/>
<asp:BoundField DataField="LiAmt"   HeaderText="月投保金額" SortExpression="LiAmt"/>
<asp:BoundField DataField="HIFee" HeaderText="應計保險費"   ReadOnly="true" /> 
<asp:BoundField DataField="Grants" HeaderText="補助差額"  SortExpression="Grants"/> 
<asp:BoundField HeaderText="本　　人"   ReadOnly="true" /> 
<asp:BoundField HeaderText="＋ １ 眷"   ReadOnly="true" /> 
<asp:BoundField HeaderText="＋ ２ 眷"   ReadOnly="true" /> 
<asp:BoundField HeaderText="＋ ３ 眷"   ReadOnly="true" /> 
<asp:BoundField DataField="HIFeeCOM" HeaderText="公　　司"   ReadOnly="true" /> 
<asp:BoundField DataField="HIFeeGOV" HeaderText="政　　府"   ReadOnly="true" /> 
<asp:BoundField HeaderText=""   ReadOnly="true" />
<%--                             新增區段結尾                            --%>

</Columns>

<FooterStyle HorizontalAlign="center"/>
<PagerStyle HorizontalAlign="left"/>


<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell" align="center">
<td class="paginationRowEdgeLl">新增</td>
<td>級　　數</td>
<td>月投保金額</td>
<td>應計保險費</td>
<td>補助差額</td>
<td>本　　人</td>
<td>＋ １ 眷</td>
<td>＋ ２ 眷</td>
<td>＋ ３ 眷</td>
<td>公　　司</td>
<td>政　　府</td>
</tr>

<tr>
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
</tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>

</asp:GridView></td></tr>
 <%------------------------------GridView 設定結束----------------------------------%>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>
<%--///////////////////////////////////////////////////////////////////////////////////////////--%>

<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
  
SelectCommand="SELECT convert(int,[RangeNo]) [RangeNo],[LiAmt],[Grants] FROM HealthInsurance_SeriesParameter " 
DeleteCommand="DELETE FROM HealthInsurance_SeriesParameter WHERE RangeNo = @RangeNo" 
UpdateCommand="UPDATE HealthInsurance_SeriesParameter SET  LiAmt = @LiAmt ,Grants = @Grants  WHERE RangeNo = @RangeNo " 
InsertCommand="INSERT INTO HealthInsurance_SeriesParameter(RangeNo, LiAmt, Grants) VALUES (@RangeNo, @LiAmt, @Grants)" >


<DeleteParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
</DeleteParameters>

<UpdateParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
<asp:Parameter Name="Grants"/>
</UpdateParameters>

<InsertParameters>
<asp:Parameter Name="RangeNo"/>
<asp:Parameter Name="LiAmt"/>
<asp:Parameter Name="Grants"/>
</InsertParameters>

</asp:SqlDataSource>

<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
