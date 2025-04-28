<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="DepartmentBasic.aspx.cs" Inherits="DepartmentBasic" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc4" %>
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
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
<%-- 08/23 修改部分start --%>&nbsp; <uc2:StyleTitle id="StyleTitle1" title="部門資料維護" runat="server"></uc2:StyleTitle>
 <uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>
  <TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%"><TBODY><TR class="QueryStyle"><TD style="WIDTH: 67px" align=left><SPAN class="ItemFontStyle">部門代號：</SPAN>&nbsp;</TD>
  <TD align=left><asp:TextBox id="txtDepCode" runat="server" MaxLength="20"></asp:TextBox></TD></TR><TR class="QueryStyle"><TD style="WIDTH: 67px" align=left><SPAN class="ItemFontStyle">部門名稱：</SPAN>&nbsp;</TD><TD align=left><asp:TextBox id="txtDepName" runat="server" MaxLength="50"></asp:TextBox> <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton></TD></TR><TR><TD colSpan=2><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR><TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR><TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"> <br />查無資料!!</asp:Panel> </TD></TR><TR><TD colSpan=2><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="Company,DepCode" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataSourceID="SDS_GridView" ShowFooter="True">
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");' L2PK='<%# DataBinder.Eval(Container, "DataItem.DepCode")%>' L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>

<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="Company" HeaderText="公司編號" ReadOnly="True" SortExpression="Company"></asp:BoundField>
<asp:BoundField DataField="DepCode" HeaderText="部門代號" ReadOnly="True" SortExpression="DepCode"></asp:BoundField>
<asp:BoundField DataField="DepName" HeaderText="部門名稱" SortExpression="DepName"></asp:BoundField>
<asp:BoundField DataField="ChiefTitle" HeaderText="主管職稱" SortExpression="ChiefTitle"></asp:BoundField>
<asp:BoundField DataField="ChiefID" HeaderText="主管" SortExpression="ChiefID"></asp:BoundField>
<asp:BoundField DataField="ParentDepCode" HeaderText="父階部門代號" SortExpression="ParentDepCode"></asp:BoundField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl" style="width: 33px">新增</td><td style="width: 9px">公司編號</td><td>部門代號</td><td>部門名稱</td><td>主管職稱</td><td>主管</td><td class="paginationRowEdgeRI">父階部門代號</td></tr>
<tr>
<td class="Grid_GridLine" style="width: 33px"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine" style="width: 9px">
</td>

<%--<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" /></td>--%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td> 
<%-- 08/23 修改部分start --%>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>

<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" /></td>

<%--<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew06" /></td>--%>
 <%-- 08/23 修改部分end --%>
</tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE><%-- 08/23 修改部分start --%><asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" DeleteCommand="DELETE FROM Department_Basic WHERE DepCode = @DepCode And Company = @Company" SelectCommand="SELECT Department_Basic.* FROM Department_Basic " InsertCommand="INSERT INTO Department_Basic(Company, DepCode, DepName, ChiefTitle, ChiefID, ParentDepCode) VALUES (@Company, @DepCode, @DepName, @ChiefTitle, @ChiefID, @ParentDepCode)" UpdateCommand="UPDATE Department_Basic SET DepName=@DepName, ChiefTitle=@ChiefTitle, ChiefID=@ChiefID, ParentDepCode=@ParentDepCode WHERE DepCode = @DepCode And Company=@Company">
<DeleteParameters>
<asp:Parameter Name="Company"></asp:Parameter>
<asp:Parameter Name="DepCode"></asp:Parameter>
</DeleteParameters>
<UpdateParameters>
<asp:Parameter Name="Company"></asp:Parameter>
<asp:Parameter Name="DepCode"></asp:Parameter>
<asp:Parameter Name="DepName"></asp:Parameter>
<asp:Parameter Name="ChiefTitle"></asp:Parameter>
<asp:Parameter Name="ChiefID"></asp:Parameter>
<asp:Parameter Name="ParentDepCode"></asp:Parameter>
</UpdateParameters>
<InsertParameters>
<asp:Parameter Name="Company"></asp:Parameter>
<asp:Parameter Name="DepCode"></asp:Parameter>
<asp:Parameter Name="DepName"></asp:Parameter>
<asp:Parameter Name="ChiefTitle"></asp:Parameter>
<asp:Parameter Name="ChiefID"></asp:Parameter>
<asp:Parameter Name="ParentDepCode"></asp:Parameter>
 <%-- 08/23 修改部分end --%>
</InsertParameters>
</asp:SqlDataSource> <%-- 08/23 修改部分start --%><asp:HiddenField id="hid_DepName" runat="server"></asp:HiddenField><asp:HiddenField id="hid_DepCode" runat="server"></asp:HiddenField> <%-- 08/23 修改部分end --%><asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label>&nbsp;
<BR /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>