<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="Holiday.aspx.cs" Inherits="Holiday" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc4" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
&nbsp; <uc2:StyleTitle id="StyleTitle1" title="年度休假日維護" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>
 <TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%">
 <TBODY><TR class="QueryStyle"><TD align=left><SPAN class="ItemFontStyle">公司：</SPAN>&nbsp;</TD>
 <TD align=left><uc4:CompanyList id="CompanyList1" runat="server" AutoPostBack="false" /> </TD></TR>
 <TR class="QueryStyle"><TD align=left><SPAN class="ItemFontStyle">年度：</SPAN>&nbsp;</TD><TD align=left><uc1:YearList id="YearList1" runat="server"></uc1:YearList> <asp:TextBox id="txtDate" runat="server" Visible="false" MaxLength="20"></asp:TextBox></TD></TR>
 <TR class="QueryStyle"><TD align=left><SPAN class="ItemFontStyle">假日名稱：</SPAN>&nbsp;</TD><TD align=left><asp:TextBox id="txtDateDesc" runat="server" MaxLength="50"></asp:TextBox> <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton></TD></TR>
 <TR class="QueryStyle"><TD align=left>批次設定假日</TD><TD align=left><asp:Button id="btnSetWeekend" onclick="btnSetYearHoilday_Click" runat="server" Text="設定年度例假日"></asp:Button></TD></TR>
 <TR><TD colSpan=2><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR><TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
 <TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
 <br />查無資料!!</asp:Panel> </TD></TR><TR><TD colSpan=2>
 <asp:GridView id="GridView1" runat="server" Width="100%" ShowFooter="True" GridLines="None" DataSourceID="SDS_GridView"
  OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
  OnRowDeleting="GridView1_RowDeleting" OnRowDeleted="GridView1_RowDeleted"
  OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated"
  OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowCreated="GridView1_RowCreated"    
  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Company,HolidayDate" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True">
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
 OnClientClick='return confirm("確定刪除?");' L2PK='<%# DataBinder.Eval(Container, "DataItem.HolidayDate")%>'
  L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField ShowEditButton="True"
 CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF"
 UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>
<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="HolidayDate" DataFormatString="{0:yyyy/MM/dd}" HeaderText="日期" ReadOnly="True" SortExpression="HolidayDate"></asp:BoundField>
<asp:BoundField DataField="HolidayName" HeaderText="假日名稱" SortExpression="HolidayName"></asp:BoundField>
</Columns>
<FooterStyle HorizontalAlign="Center"></FooterStyle>
<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>日期</td><td class="paginationRowEdgeRI">假日名稱</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine">
<asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td></tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>
<asp:SqlDataSource id="SDS_GridView" runat="server" 
UpdateCommand="UPDATE Holiday SET HolidayName = @HolidayName WHERE Company = @Company And (Convert(varchar,HolidayDate,111) = @HolidayDate)" 
InsertCommand="INSERT INTO Holiday(Company, HolidayDate, HolidayName) VALUES (@Company, @HolidayDate, @HolidayName)" 
SelectCommand="SELECT Holiday.* FROM Holiday " 
DeleteCommand="DELETE FROM Holiday WHERE Company = @Company And (Convert(varchar,HolidayDate,111) = @HolidayDate)" 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>">
     <DeleteParameters>
         <asp:Parameter Name="Company" />
         <asp:Parameter Name="HolidayDate" />
     </DeleteParameters>
     <UpdateParameters>
         <asp:Parameter Name="Company" />
         <asp:Parameter Name="HolidayDate" />
         <asp:Parameter Name="HolidayName" />
     </UpdateParameters>
     <InsertParameters>
         <asp:Parameter Name="Company" />
         <asp:Parameter Name="HolidayDate" />
         <asp:Parameter Name="HolidayName" />
     </InsertParameters>
</asp:SqlDataSource><asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
