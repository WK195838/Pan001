<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0460.aspx.cs" Inherits="GLA0460" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">

<script language ="javascript" type="text/javascript"　>


</script>    

<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox>
<uc2:StyleTitle id="StyleTitle1" title="內設資料值維護" runat="server"></uc2:StyleTitle> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
    <tr class="QueryStyle">
        <td align="left" style="position: relative">
            <table>
                <tr>
                    <td>
                        公司編號：</td>
                    <td>
                        <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="false" />
                    </td>
                    <td >
                        累積科目盈虧：</td>
                    <td><asp:TextBox ID="txtAccuPLAcctNo" runat="server"></asp:TextBox>
                        </td>
                    <td>
                        本期損益科目：</td>
                    <td><asp:TextBox ID="txtPeriodPLAcctNo" runat="server"></asp:TextBox>
                        </td>
                </tr>
            </table>
            <asp:Button ID="BtnQuery" runat="server" Text="查詢" /></td>
    </tr>
<tr class="QueryStyle">
    <td align="left" style=" position:relative ">
        &nbsp;
        <div style="position:absolute; bottom:0px; left:4px">
        <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton>
        </div>
    </td>
</tr>

   
 
 <tr><td colspan="2"><asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td></tr>
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </td></tr>
 <tr><td align="left" colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
                                <br />查無資料!!</asp:Panel> </td></tr>
                                <tr><td colspan=4>
                                    <asp:GridView id="GridView1" runat="server" Width="100%" AllowPaging="True" 
                                OnRowCreated="GridView1_RowCreated" 
                                OnRowDeleting="GridView1_RowDeleting" 
                                OnRowDeleted="GridView1_RowDeleted" 
                                OnRowDataBound="GridView1_RowDataBound" 
                                DataKeyNames="Company" 
                                AutoGenerateColumns="False" ShowFooter="True" DataSourceID="sqlDatasource">
                                 
                                 <Columns>
                                        <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  
                                            OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px"  />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="編輯">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='編輯'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="細項">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='細項'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                     <asp:BoundField DataField="Company" HeaderText="公司編號" ReadOnly="True" 
                                            SortExpression="Company" />
                                     <asp:BoundField DataField="ApprovalCode" HeaderText="主管核對資料" 
                                            SortExpression="ApprovalCode" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="AccuPLAcctNo" HeaderText="累計盈虧科目" 
                                       SortExpression="AccuPLAcctNo" >                                        
                                        </asp:BoundField>
                                     <asp:BoundField DataField="PeriodPLAcctNo" HeaderText="本期損益科目" 
                                            SortExpression="PeriodPLAcctNo" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                        <asp:BoundField DataField="CashCodefrom" HeaderText="資金代碼－來源" 
                                            SortExpression="CashCodefrom" />
                                        <asp:BoundField DataField="CashCodeto" HeaderText="資金代碼－用途" 
                                            SortExpression="CashCodeto" />
                                        <asp:BoundField DataField="CalendarType" HeaderText="年制設定" 
                                            SortExpression="CalendarType" />
                                        <asp:BoundField DataField="AcctPeriod" HeaderText="年度會計月數" 
                                            SortExpression="AcctPeriod" />
                                        <asp:BoundField DataField="DateFormat" HeaderText="日期格式" 
                                            SortExpression="DateFormat" />
                                        <asp:BoundField DataField="LastPostDate" HeaderText="已過帳日期" 
                                            SortExpression="LastPostDate" />
                                        <asp:BoundField DataField="CloseYYYY" HeaderText="已轉結年度" 
                                            SortExpression="CloseYYYY" />
                                        <asp:BoundField DataField="CloseYM" HeaderText="已結轉期間" 
                                            SortExpression="CloseYM" />
                                        <asp:BoundField DataField="PostCtlCode" HeaderText="過帳控制" 
                                            SortExpression="PostCtlCode" />
                                        <asp:BoundField DataField="JournalNoType" HeaderText="傳票編碼方式" 
                                            SortExpression="JournalNoType" />
                                        <asp:BoundField DataField="PostCtlType" HeaderText="結帳方式" 
                                            SortExpression="PostCtlType" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table>
</ContentTemplate>
        </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB %>"
        ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

