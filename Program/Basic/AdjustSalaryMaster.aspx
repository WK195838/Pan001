<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AdjustSalaryMaster.aspx.cs" Inherits="Basic_AdjustSalaryMaster" %>
<%-- 載入共用變數  --%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language="javascript" type="text/javascript">
//特別控制
</script>    
    <div>
<%-- 畫面標題顯示 start --%>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="調薪資料維護" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart>
<%-- 畫面標題顯示 end --%>
<%-- 搜尋頁面 start --%>
<table id="table1"  cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        <uc9:SearchList id="SearchList1" runat="server" />
        <div>
        <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton>
        </div>
    </td>
</tr>
<!-- 搜尋模組 -->
<%-- 搜尋頁面 end --%>
<%-- 錯誤訊息與資料筆數切換 start --%>
<tr>
<td colspan="2">
    <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
</tr>
<tr>
<td colspan="2">
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td>
</tr>
<%-- 錯誤訊息與資料筆數切換 end --%>
<%-- 無資料時顯示panel start --%>
<tr>
    <td align="left" colspan="2">
        <asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px"><br />查無資料!!
        <asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel></td>
</tr>
<%-- 無資料時顯示panel end --%>
<tr>
<td colspan="4">
<%-- GridView 功能設定 start --%>
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" DataSourceID="SDS_GridView" 
    OnRowCreated="GridView1_RowCreated" 
    OnRowDeleting="GridView1_RowDeleting" 
    OnRowDeleted="GridView1_RowDeleted" 
    OnRowDataBound="GridView1_RowDataBound" 
    DataKeyNames="Company,EmployeeId,EffectiveDate" 
    AutoGenerateColumns="False">          
    <Columns>
<%-- 刪除按鈕設定 start --%>
    <asp:TemplateField HeaderText="刪除" ShowHeader="False">
    <ItemTemplate>
    <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click" 
    L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' 
    L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>' 
    L3PK='<%# DataBinder.Eval(Container, "DataItem.EffectiveDate")%>' 
    OnClientClick='return confirm("確定刪除?");' 
    Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton><%--L4PK='<%# DataBinder.Eval(Container, "DataItem.AdjustSalaryItem")%>'--%>
    </ItemTemplate>
    <HeaderStyle width="30px" cssclass="paginationRowEdgeL" />
    </asp:TemplateField>
<%-- 刪除按鈕設定 end --%>
<%-- 編輯按鈕設定 start --%>                                        
    <asp:TemplateField HeaderText="編輯">
    <ItemTemplate>
    <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
    </ItemTemplate>
    <HeaderStyle width="30px" />
    </asp:TemplateField>
<%-- 編輯按鈕設定 end --%>
<%-- 查詢按鈕設定 start --%>                                        
    <asp:TemplateField HeaderText="查詢">
    <ItemTemplate>
    <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
    Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
    </ItemTemplate>
    <HeaderStyle width="30px" />
    </asp:TemplateField>
<%-- 查詢按鈕設定 end --%>
<%-- 顯示欄位設定 start --%>                                         
    <asp:BoundField DataField="Company" HeaderText="部門" />
    <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" />    
    <asp:BoundField DataField="EffectiveDate" HeaderText="生效日期" SortExpression="EffectiveDate" />
    <asp:BoundField DataField="ApproveDate" HeaderText="核定日期" SortExpression="ApproveDate" />
<%-- 顯示欄位設定 end --%>
    </Columns>
    </asp:GridView></td>
<%-- GridView 功能設定 end --%>  
</tr>
</tbody>
</table>
<%-- Sql語法設定與datasource來源 start --%>
<asp:SqlDataSource id="SDS_GridView" runat="server" 
SelectCommand="SELECT AdjustSalary_Master.* FROM AdjustSalary_Master" 
DeleteCommand="DELETE FROM AdjustSalary_Master WHERE (Company = @Company And EmployeeId = @EmployeeId And Convert(varchar,EffectiveDate,120)=@EffectiveDate) And AdjustSalaryItem=@AdjustSalaryItem" 
ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
<DeleteParameters>
    <asp:Parameter Name="Company" />
    <asp:Parameter Name="EmployeeId" />
    <asp:Parameter Name="EffectiveDate" />
</DeleteParameters>
</asp:SqlDataSource>
<%-- Sql語法設定與datasource來源 end --%>
    <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel><br />
    </div>
</asp:Content>


