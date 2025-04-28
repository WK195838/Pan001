<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PRA_IncomHI2.aspx.cs" Inherits="PRA_Incomer" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc9" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>&nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="所得人補充保費維護" runat="server"></uc2:StyleTitle>
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>
<TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
 <%------------------------------版面上方搜尋部分區開始------------------------------%>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left"><span class="ItemFontStyle">公　　司：</span>
        <CompanyList:CompanyList id="SearchList1" runat="server" /> 
    </td>
</tr>
<!-- 搜尋模組 -->
 <tr>
    <td align="left"><span class="ItemFontStyle">身分證號：</span>
    <asp:TextBox ID="txtIDNo" runat="server" />    
    </td>
</tr>
 <tr>
    <td align="left"><span class="ItemFontStyle">所得日期：</span>
    <asp:TextBox id="txtDate" runat="server" MaxLength="10"/>
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>    
    </td>
</tr>
 <%------------------------------版面上方搜尋部分區結束------------------------------%>
<TR><TD colSpan=2>
    <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label></TD></TR>
<TR><TD colSpan=2>
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
<TR><TD align=left colSpan=2>
    <asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False"><br />查無資料!!</asp:Panel> </TD></TR>
<TR><TD colSpan=2>
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" 
    AutoGenerateColumns="false" 
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
    DataSourceID="SDS_GridView" 
    ShowFooter="True" 
    ><%--OnDataBound="GridView1_DataBound"--%>
<RowStyle HorizontalAlign="Center"></RowStyle>
<Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
<asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server"
 Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除' style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
  OnClientClick='return confirm("確定刪除?");' L1PK='<%# DataBinder.Eval(Container, "DataItem.SeqId")%>' L2PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'
 CausesValidation="False"></asp:LinkButton> 
</ItemTemplate>
<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>

<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
<HeaderStyle Width="60px"></HeaderStyle>
<ItemStyle Width="60px"></ItemStyle>
</asp:CommandField>
</Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>

</EmptyDataTemplate>

<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> 
</TD></TR>
<TR><TD align=left colSpan=2></TD></TR>
</TBODY>
</TABLE>
<asp:SqlDataSource id="SDS_GridView" runat="server"></asp:SqlDataSource>
<asp:HiddenField id="hid_Company" runat="server"></asp:HiddenField> 
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField> 
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
