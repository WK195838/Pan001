<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="SalaryGradeRankData.aspx.cs" Inherits="SalaryGradeRankData" EnableEventValidation="false" %>

<%--<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>--%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>&nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="薪職等級與本薪維護" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<tr class="QueryStyle"><td align="left">
    <span class="ItemFontStyle">職等：</span>&nbsp;</td>
<td align="left">
    <uc8:CodeList ID="CL_Level" runat="server" />
</td></tr>
<tr class="QueryStyle"><td align="left">
    <span class="ItemFontStyle">級數：</span>&nbsp;</td>
<td align="left">
    <uc8:CodeList ID="CL_Rank" runat="server" />
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
</td></tr>
<tr class="QueryStyle"><td align="left">
    <span class="ItemFontStyle">批次設定：</span>&nbsp;</td>
<td align="left">本薪職等：<uc8:CodeList ID="CL_LevelRank" runat="server" />
１級本薪：<asp:TextBox ID="txt_BaseSalary" runat="server" Width="100px" MaxLength="7"/>
級差：<asp:TextBox ID="txt_Bracket" runat="server" Width="50px" MaxLength="4" />
<asp:Button id="btnSetBaseSalary" onclick="btnSetBaseSalary_Click" runat="server" Text="設定單一職等本薪" />
</td></tr>
<tr><td colspan="2">
    <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></td></tr>
<tr><td colspan="2">
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td></tr>
<tr><td colspan="2" align="right">
    <asp:Label id="lbl_show" runat="server" ><asp:Label ID="lbl_SalsryPoint" runat="server" Text="" /></asp:Label></td>
    </tr>
<tr><td align="left" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
 <br />查無資料!!</asp:Panel> </td></tr>
 <tr><td colspan="2"><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
  AutoGenerateColumns="False" DataKeyNames="Level,Rank" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
  OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting" GridLines="None"
  OnRowCreated="GridView1_RowCreated" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" 
  OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated"
  OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataSourceID="SDS_GridView" ShowFooter="True">
<RowStyle horizontalalign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" 
 L1PK='<%# DataBinder.Eval(Container, "DataItem.Level")%>' L2PK='<%# DataBinder.Eval(Container, "DataItem.Rank")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>

<HeaderStyle width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle width="60px"></HeaderStyle>
<ItemStyle width="60px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="Level" HeaderText="職等" ReadOnly="True" SortExpression="Level"></asp:BoundField>
<asp:BoundField DataField="Rank" HeaderText="級數" ReadOnly="True" SortExpression="Rank"></asp:BoundField>
<asp:BoundField DataField="SalaryPoint" HeaderText="薪點" SortExpression="SalaryPoint"></asp:BoundField>
<asp:BoundField DataField="BaseSalary" HeaderText="本薪" SortExpression="BaseSalary"></asp:BoundField>
</Columns>

<FooterStyle horizontalalign="Center"></FooterStyle>

<PagerStyle horizontalalign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>職等</td><td>級數</td><td>薪點</td><td class="paginationRowEdgeRI">本薪</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew01" runat="server" /></td>
<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew02" runat="server" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td></tr>
</table>
</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>
</asp:GridView> </td></tr><tr><td align="left" colspan="2"></td></tr></tbody></table>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" 
DeleteCommand="DELETE FROM SalaryGradeRankData WHERE Level=@Level And Rank=@Rank" 
SelectCommand="SELECT SalaryGradeRankData.* FROM SalaryGradeRankData " 
InsertCommand="INSERT INTO SalaryGradeRankData(Level,Rank, SalaryPoint, BaseSalary) VALUES (@Level,@Rank, @SalaryPoint, @BaseSalary)" 
UpdateCommand="UPDATE SalaryGradeRankData SET SalaryPoint=@SalaryPoint, BaseSalary=@BaseSalary WHERE Level=@Level And Rank=@Rank">
                 <DeleteParameters>
                     <asp:Parameter Name="Level" />
                     <asp:Parameter Name="Rank" />
                 </DeleteParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="Level" />
                     <asp:Parameter Name="Rank" />
                     <asp:Parameter Name="SalaryPoint" />
                     <asp:Parameter Name="BaseSalary" />
                 </UpdateParameters>
                 <InsertParameters>
                     <asp:Parameter Name="Level" />
                     <asp:Parameter Name="Rank" />
                     <asp:Parameter Name="SalaryPoint" />
                     <asp:Parameter Name="BaseSalary" />
                 </InsertParameters>
             </asp:SqlDataSource><asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>

