<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PayrollControl.aspx.cs" Inherits="Basic_PayrollControl" EnableEventValidation="false" %>

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
<script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
<script language ="javascript">
//特別控制
//Company	公司編號
//SalaryYM	年月
//DraftDate	試算日期
//ConfirmDate	確認日期

</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
&nbsp; <uc2:StyleTitle id="StyleTitle1" title="人事薪資控管" runat="server"></uc2:StyleTitle><uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%"><TBODY>
<TR class="QueryStyle"><TD align="left"><SPAN class="ItemFontStyle">公    司：</SPAN>&nbsp;</TD>
<TD align="left"><asp:TextBox id="txtCompany" runat="server" MaxLength="20"  /><%-- </TD></TR>
<TR class="QueryStyle"><TD align="left"><SPAN class="ItemFontStyle">年月：</SPAN>&nbsp;</TD>
<TD align="left"><asp:TextBox id="txtSalaryYM" runat="server" MaxLength="50"></asp:TextBox>--%> <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton></TD></TR>
<%--<tr class="QueryStyle"><td align="left">批次設定假日</td><td align="left"><asp:Button runat="server" ID="btnSetWeekend" Text="設定年度例假日" OnClick="btnSetYearHoilday_Click" /></td></tr>--%>
<TR><TD colSpan=2><asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR>
<TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
<TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
 <br />查無資料!!</asp:Panel> </TD></TR>
 <TR><TD colSpan=2><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="Company" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleted="GridView1_RowDeleted" OnRowDeleting="GridView1_RowDeleting" GridLines="None" OnRowCreated="GridView1_RowCreated" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowUpdated="GridView1_RowUpdated" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataSourceID="SDS_GridView" ShowFooter="True">
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<%--<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");' L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>--%>

<%--<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle></HeaderStyle>
<ItemStyle></ItemStyle>
</asp:CommandField>--%>
<asp:BoundField DataField="Company" HeaderText="公司" ReadOnly="True" SortExpression="Company"></asp:BoundField>
<asp:BoundField DataField="SalaryYM" HeaderText="年月" SortExpression="SalaryYM"></asp:BoundField>
<asp:BoundField DataField="DraftDate" HeaderText="試算日期" SortExpression="DraftDate"></asp:BoundField>
<asp:BoundField DataField="ConfirmDate" HeaderText="確認日期" SortExpression="ConfirmDate"></asp:BoundField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>公司編號</td><td>年月</td><td>試算日期</td><td class="paginationRowEdgeRI">確認日期</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /><asp:ImageButton id="btnOpen1" runat="server" SkinID="OpenWin1"></asp:ImageButton></td>
<td class="Grid_GridLine"><asp:DropDownList runat="server" id="YearMonth02" /><asp:DropDownList runat="server" id="YearMonth03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td></tr>
</table>
</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE><asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" DeleteCommand="DELETE FROM PayrollControl WHERE Company = @Company" SelectCommand="SELECT PayrollControl.* FROM PayrollControl " InsertCommand="INSERT INTO PayrollControl(Company, SalaryYM, DraftDate, ConfirmDate) VALUES (@Company, @SalaryYM, @DraftDate, @ConfirmDate)" UpdateCommand="UPDATE PayrollControl SET SalaryYM = @SalaryYM, DraftDate = Convert(smalldatetime,@DraftDate), ConfirmDate = Convert(smalldatetime,@ConfirmDate) WHERE Company = @Company">
                 <DeleteParameters>
                     <asp:Parameter Name="Company" />
                 </DeleteParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="SalaryYM" />
                     <asp:Parameter Name="DraftDate" />
                     <asp:Parameter Name="ConfirmDate" />
                 </UpdateParameters>
                 <InsertParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="SalaryYM" />
                     <asp:Parameter Name="DraftDate" />
                     <asp:Parameter Name="ConfirmDate" />
                 </InsertParameters>
             </asp:SqlDataSource><asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>

