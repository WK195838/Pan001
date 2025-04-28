<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Department.aspx.cs" Inherits="Basic_Department" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="部門資料維護" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart> 
<TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR class="QueryStyle"><TD align=left>
<SPAN class="ItemFontStyle">公司：</SPAN>&nbsp;</TD>
<TD align=left>
    <uc1:CompanyList id="CompanyList1" runat="server" ></uc1:CompanyList> </TD></TR>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">部門代號：</SPAN>&nbsp;</TD>
<TD align=left>
    <asp:TextBox id="tbDepCode" runat="server" MaxLength="50"></asp:TextBox> 
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton> 
    <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton> </TD></TR>
<TR><TD colSpan=2>
    <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label> </TD></TR>
<TR><TD colSpan=2>
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
<TR><TD align=left colSpan=2>
    <asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px"><br />查無資料!!
    <asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel> </TD></TR>
<TR><TD colSpan=4>
    <asp:GridView id="GridView1" runat="server" Width="100%" AutoGenerateColumns="False" 
    DataKeyNames="Company,DepCode" 
    OnRowDataBound="GridView1_RowDataBound" 
    OnRowDeleted="GridView1_RowDeleted" 
    OnRowDeleting="GridView1_RowDeleting" 
    OnRowCreated="GridView1_RowCreated" 
    DataSourceID="SDS_GridView" 
    AllowPaging="True" AllowSorting="True">
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate><%--L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'--%>
    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click"  L2PK='<%# DataBinder.Eval(Container, "DataItem.DepCode")%>'  
     OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>                                          
</ItemTemplate>

<HeaderStyle CssClass="paginationRowEdgeL" Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:TemplateField HeaderText="編輯"><ItemTemplate>
    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
     Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>                                        
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:TemplateField HeaderText="查詢">
<ItemTemplate>
    <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<%--<asp:BoundField DataField="Company" HeaderText="公司" SortExpression="Company"></asp:BoundField>--%>
<asp:BoundField DataField="DepCode" HeaderText="部門代號" SortExpression="DepCode"></asp:BoundField>
<asp:BoundField DataField="DepName" HeaderText="部門名稱" SortExpression="DepName"></asp:BoundField>
<asp:BoundField DataField="DepNameE" HeaderText="部門英文名稱" SortExpression="DepNameE"></asp:BoundField>
</Columns>
</asp:GridView></TD></TR></TBODY></TABLE>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" 
DeleteCommand="DELETE FROM Departmnet WHERE (Company = @Company And DepCode=@DepCode)" 
SelectCommand="SELECT Department.* FROM Department order by DepCode">
<DeleteParameters>
    <asp:Parameter Name="Company" />
    <asp:Parameter Name="DepCode" />
</DeleteParameters>
</asp:SqlDataSource> <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
<br />
    </div>
</asp:Content>
