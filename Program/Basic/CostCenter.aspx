<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="CostCenter.aspx.cs" Inherits="CostCenter" %>

<%--<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>
--%>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">

<script language ="javascript" type="text/javascript"　>
//特別控制
//Company	公司編號
//EmployeeId	員工編號
//DepositBank	存入銀行 
//DepositBankAccount	存入帳號
//Period2Depositdate	下期存入日期
//Period1Depositdate	上期存入日期
//Company	公司編號
//EmployeeId	員工編號
//SalaryItem	薪資項目
//Amount	金額

</script>    

<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox>
<uc2:StyleTitle id="StyleTitle1" title="成本中心分攤設定維護程式" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" >
        <uc9:SearchList id="SearchList1" runat="server" /><asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton>
        <br/>
        清除選項：<asp:RadioButton ID="RB_Sel1" runat="server" AutoPostBack="True" 
            oncheckedchanged="RB_Sel1_CheckedChanged" Text="依公司" />
        <asp:RadioButton ID="RB_Sel2" runat="server" AutoPostBack="True" 
            oncheckedchanged="RB_Sel2_CheckedChanged" Text="依部門" />
        <asp:Button ID="Clear" runat="server" Text="清除" Width="59px" 
            onclick="Clear_Click" />       
        </td>
</tr>
<tr class="QueryStyle">
    <td align="left">
日　　期：<asp:TextBox ID="txtDateS" runat="server" CssClass="JQCalendar" Width="100px"></asp:TextBox>
～<asp:TextBox ID="txtDateE" runat="server" CssClass="JQCalendar" Width="100px"></asp:TextBox>
</td></tr>    
<tr class="QueryStyle">
    <td align="left"><span class="ItemFontStyle">是否在職：</span>&nbsp;
           <asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow">
           </asp:CheckBoxList>
    </td>
</tr>
<!-- 搜尋模組 -->
 <tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td></tr>
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </td></tr>
 <tr><td align="left" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
                                <br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel> </td></tr>
                                <tr><td colspan=4><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" DataSourceID="SDS_GridView" 
                                OnRowCreated="GridView1_RowCreated" 
                                OnRowDeleting="GridView1_RowDeleting" 
                                OnRowDeleted="GridView1_RowDeleted" 
                                OnRowDataBound="GridView1_RowDataBound" 
                                DataKeyNames="Company,EmployeeId,DeptId" 
                                AutoGenerateColumns="False">
                                 
                                 <Columns>
                                        <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
                                            OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="編輯">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="查詢">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                     <asp:BoundField DataField="Company" HeaderText="公司" ReadOnly="True" SortExpression="Company" Visible="False" />
                                     <asp:BoundField DataField="DeptId" HeaderText="部門" SortExpression="DeptId" />
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" />
                                     <asp:BoundField DataField="Balance" HeaderText="已分攤比例" SortExpression="Balance" />
                                     <asp:BoundField DataField="Remainder" HeaderText="剩餘分攤比例" SortExpression="Remainder" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table><asp:SqlDataSource id="SDS_GridView" runat="server"  SelectCommand="SELECT Payroll_Master_Heading.* FROM Payroll_Master_Heading" DeleteCommand="DELETE FROM Payroll_Master_Heading WHERE (Company = @Company And EmployeeId = @EmployeeId " ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
                 <DeleteParameters>
                     <asp:Parameter Name="Company" />
                     <asp:Parameter Name="EmployeeId" />
                     <asp:Parameter Name="DepositBank" />
                       <asp:Parameter Name="DepositBankAccount" />
                 </DeleteParameters>
             </asp:SqlDataSource> <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>

