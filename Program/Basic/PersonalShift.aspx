<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PersonalShift.aspx.cs" Inherits="Basic_PersonalShift" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>&nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="個人排班表" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>
<TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">公    司：</SPAN>&nbsp;</TD>
<TD align=left>
    <uc8:CompanyList id="CompanyList1" runat="server" AutoPostBack="true"/></TD></TR>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">部    門：</SPAN>&nbsp;</TD>
<TD align=left>
    <asp:DropDownList id="DepList1" runat="server" Width="165" AutoPostBack="true" /> </TD></TR>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">員    工：</SPAN>&nbsp;</TD>
<TD align=left>
    <asp:DropDownList id="EmployeeIdList1" runat="server" Width="165" AutoPostBack="true" /> 
    <%--<asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />--%></TD></TR>
<TR><TD colSpan=2>
    <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR>
<TR><TD colSpan=2>
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
<TR><TD align=left colSpan=2>
    <asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"><br />查無資料!!</asp:Panel> </TD></TR>
<TR><TD colSpan=2>
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" 
    AutoGenerateColumns="False" 
    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
    OnRowDeleted="GridView1_RowDeleted" 
    OnRowDeleting="GridView1_RowDeleting" 
    GridLines="None" 
    OnRowCreated="GridView1_RowCreated" 
    OnRowEditing="GridView1_RowEditing" 
    OnRowCancelingEdit="GridView1_RowCancelingEdit" 
    OnRowUpdating="GridView1_RowUpdating" 
    OnRowUpdated="GridView1_RowUpdated" 
    OnRowCommand="GridView1_RowCommand" 
    OnRowDataBound="GridView1_RowDataBound" 
    DataKeyNames="Company,EmployeeId,DateStart" 
    DataSourceID="SDS_GridView" 
    ShowFooter="True" 
    ><%--OnDataBound="GridView1_DataBound"--%>
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");' L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>' L3PK='<%# DataBinder.Eval(Container, "DataItem.DateStart")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>
<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
<%--<asp:BoundField DataField="DeptId" HeaderText="部門代號" SortExpression="DeptId" ></asp:BoundField>--%>
<asp:BoundField DataField="EmployeeId" HeaderText="員工編號" SortExpression="EmployeeId" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="DateStart" HeaderText="起始日期" SortExpression="DateStart" ReadOnly="true"></asp:BoundField>
<asp:BoundField DataField="DateEnd" HeaderText="迄止日期" SortExpression="DateEnd"></asp:BoundField>
<asp:BoundField DataField="ShiftCode" HeaderText="班別編號" SortExpression="ShiftCode"  ></asp:BoundField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td>
<%--<td>部門代號</td>--%><td>員工編號</td><td>起始日期</td><td>迄止日期</td><td class="paginationRowEdgeRI">班別編號</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<%--<td class="Grid_GridLine"><asp:DropDownList runat="server" ID="ddl01" OnSelectedIndexChanged="ddl01_SelectedChanged" /></td>--%>
<td class="Grid_GridLine"><asp:DropDownList runat="server" ID="ddl01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:DropDownList runat="server" ID="ddl04" /></td>
</tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> 
</TD></TR>
<TR><TD align=left colSpan=2></TD></TR>
</TBODY>
</TABLE>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" 
DeleteCommand="DELETE FROM PersonalShift WHERE Company = @Company and EmployeeId=@EmployeeId and DateStart=@DateStart" 
SelectCommand="SELECT PersonalShift.* FROM PersonalShift " 
InsertCommand="INSERT INTO PersonalShift(Company,DeptId,EmployeeId,DateStart,DateEnd,ShiftCode) VALUES (@Company,@DeptId,@EmployeeId,@DateStart,@DateEnd,@ShiftCode)" 
UpdateCommand="UPDATE PersonalShift SET DateEnd=@DateEnd,DateStart=@DateStart WHERE Company = @Company and EmployeeId=@EmployeeId and DateStart=@DateStart">
                 <DeleteParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="EmployeeId" /> 
                     <asp:Parameter Name="DateStart" />                
                 </DeleteParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="DeptId" />
                     <asp:Parameter Name="EmployeeId" />
                     <asp:Parameter Name="DateStart" />
                     <asp:Parameter Name="DateEnd" />
                     <asp:Parameter Name="ShiftCode" />                     
                 </UpdateParameters>
                 <InsertParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="DeptId" />
                     <asp:Parameter Name="EmployeeId" />
                     <asp:Parameter Name="DateStart" />
                     <asp:Parameter Name="DateEnd" />
                     <asp:Parameter Name="ShiftCode" />  
                 </InsertParameters>
             </asp:SqlDataSource>
<asp:HiddenField id="hid_Company" runat="server"></asp:HiddenField> 
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> 
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
