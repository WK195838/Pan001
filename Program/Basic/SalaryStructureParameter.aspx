<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="SalaryStructureParameter.aspx.cs" Inherits="Basic_SalaryStructureParameter" %>

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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp; <uc2:StyleTitle id="StyleTitle1" title="薪資結構參數維護" runat="server"></uc2:StyleTitle>
 <uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart>
  <TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%"><TBODY>
  <TR class="QueryStyle"><TD align=left>
  <SPAN class="ItemFontStyle">薪資代碼：</SPAN>&nbsp;</TD><TD align=left><asp:TextBox id="tbSalaryId" runat="server" MaxLength="20"></asp:TextBox> </TD></TR>
  <TR class="QueryStyle"><TD align=left><SPAN class="ItemFontStyle">薪資名稱：</SPAN>&nbsp;</TD><TD align=left><asp:TextBox id="tbSalaryName" runat="server" MaxLength="50"></asp:TextBox> <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton> <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton> </TD></TR>
  <TR><TD colSpan=2><asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></TD></TR><TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR><TR><TD align=left colSpan=2><asp:Panel id="Panel_Empty" runat="server" Height="50px" Width="250px" Visible="False">
                                <br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel> </TD></TR><TR><TD colSpan=4>
                                <asp:GridView Width="100%" id="GridView1" runat="server" AllowSorting="True" AllowPaging="True" DataSourceID="SDS_GridView" OnRowCreated="GridView1_RowCreated" OnRowDeleting="GridView1_RowDeleting" OnRowDeleted="GridView1_RowDeleted" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="SalaryId" AutoGenerateColumns="False"><Columns>
<asp:TemplateField HeaderText="刪除" ShowHeader="False"><ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.SalaryId")%>'  
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
<asp:TemplateField HeaderText="查詢"><ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            
</ItemTemplate>

<HeaderStyle Width="30px"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="SalaryId" HeaderText="薪資代碼" SortExpression="SalaryId"></asp:BoundField>
<asp:BoundField DataField="SalaryName" HeaderText="薪資名稱" SortExpression="SalaryName"></asp:BoundField>
</Columns>
</asp:GridView> </TD></TR></TBODY></TABLE><asp:SqlDataSource id="SDS_GridView" runat="server" SelectCommand="SELECT SalaryStructure_Parameter.* FROM SalaryStructure_Parameter" DeleteCommand="DELETE FROM SalaryStructure_Parameter WHERE (SalaryId = @SalaryId)" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
                 <DeleteParameters>
                     <asp:Parameter Name="SalaryId" />
                 </DeleteParameters>
             </asp:SqlDataSource> <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>
