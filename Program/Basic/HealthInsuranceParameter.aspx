<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="HealthInsuranceParameter.aspx.cs" Inherits="HealthInsuranceParameter" %>

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
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
&nbsp; <uc2:StyleTitle id="StyleTitle1" title="全民健保參數檔" runat="server"></uc2:StyleTitle><uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> <TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%"><TBODY>
<TR class="QueryStyle"><TD align=left><SPAN class="ItemFontStyle">版本號：</SPAN>&nbsp;</TD><TD align=left><asp:TextBox id="txtVersionNo" runat="server" MaxLength="20"></asp:TextBox> <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" /></TD></TR>
<TR><TD colSpan=2><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR><TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR><TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False">
 <br />查無資料!!</asp:Panel> </TD></TR><TR><TD colSpan=2><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="VersionNo" DataSourceID="SDS_GridView" ShowFooter="True" OnDataBound="GridView1_DataBound">
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");' L1PK='<%# DataBinder.Eval(Container, "DataItem.VersionNo")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>

<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="VersionNo" HeaderText="VersionNo" SortExpression="VersionNo" ReadOnly="true" ></asp:BoundField>
<asp:BoundField DataField="Rates" HeaderText="Rates" SortExpression="Rates"></asp:BoundField>
<asp:BoundField DataField="EmpBurdenRate" HeaderText="EmpBurdenRate" SortExpression="EmpBurdenRate"></asp:BoundField>
<asp:BoundField DataField="CompanyBurdenRate" HeaderText="CompanyBurdenRate" SortExpression="CompanyBurdenRate"></asp:BoundField>
<asp:BoundField DataField="Comp_burden_Ave" HeaderText="Comp_burden_Ave" SortExpression="Comp_burden_Ave"></asp:BoundField>
<asp:BoundField DataField="EmpBurden_upto" HeaderText="EmpBurden_upto" SortExpression="EmpBurden_upto"></asp:BoundField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td>
<td>版本號</td><td>費率</td><td>員工負擔率</td><td>公司負擔率</td><td>公司負擔平均人數</td><td class="paginationRowEdgeRI">員工最多負擔人數</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" /></td>
</tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE><asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" DeleteCommand="DELETE FROM HealthInsurance_Parameter WHERE VersionNo = @VersionNo " SelectCommand="SELECT HealthInsurance_Parameter.* FROM HealthInsurance_Parameter " InsertCommand="INSERT INTO HealthInsurance_Parameter([VersionNo]&#13;&#10;           ,[Rates]&#13;&#10;           ,[EmpBurdenRate]&#13;&#10;           ,[CompanyBurdenRate]&#13;&#10;           ,[Comp_burden_Ave]&#13;&#10;           ,[EmpBurden_upto])&#13;&#10;  VALUES (@VersionNo, Convert(decimal,@Rates),Convert(decimal,@EmpBurdenRate),Convert(decimal,@CompanyBurdenRate),Convert(decimal,@Comp_burden_Ave),Convert(decimal,@EmpBurden_upto))" UpdateCommand="UPDATE HealthInsurance_Parameter SET Rates=@Rates,EmpBurdenRate=@EmpBurdenRate,CompanyBurdenRate=@CompanyBurdenRate&#13;&#10; ,Comp_burden_Ave=@Comp_burden_Ave,EmpBurden_upto=@EmpBurden_upto WHERE VersionNo = @VersionNo ">
                 <DeleteParameters>
                     <asp:Parameter Name="VersionNo" />                     
                 </DeleteParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="VersionNo" />
                     <asp:Parameter Name="Rates" />
                     <asp:Parameter Name="EmpBurdenRate" />
                     <asp:Parameter Name="CompanyBurdenRate" />
                     <asp:Parameter Name="Comp_burden_Ave" />
                     <asp:Parameter Name="EmpBurden_upto" />                     
                 </UpdateParameters>
                 <InsertParameters>
                     <asp:Parameter Name="VersionNo" />
                     <asp:Parameter Name="Rates" />
                     <asp:Parameter Name="EmpBurdenRate" />
                     <asp:Parameter Name="CompanyBurdenRate" />
                     <asp:Parameter Name="Comp_burden_Ave" />
                     <asp:Parameter Name="EmpBurden_upto" />  
                 </InsertParameters>
             </asp:SqlDataSource> <asp:HiddenField id="hid_VersionNo" runat="server"></asp:HiddenField> <asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
