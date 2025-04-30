<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0360.aspx.cs" Inherits="GLA0360" %>

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
<uc2:StyleTitle id="StyleTitle1" title="會計期間設定" runat="server" 
        ShowBackToPre="False"></uc2:StyleTitle> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
    <tr class="QueryStyle">
        <td align="left" style="position: relative">
          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
          <ContentTemplate>
            <table>
                <tr>
                    <td>
                        公司別：</td>
                    <td>
                      
                        
                        <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="True" />
                    </td>
                    <td >
                        年度：</td>
                    <td>
                        <asp:DropDownList ID="Drpyear" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button ID="BtnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" /></td>
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
                                <tr><td colspan=4><asp:GridView id="GridView1" runat="server" Width="100%" AllowPaging="True" 
                                OnRowCreated="GridView1_RowCreated" 
                                OnRowDeleting="GridView1_RowDeleting" 
                                OnRowDeleted="GridView1_RowDeleted" 
                                OnRowDataBound="GridView1_RowDataBound" 
                                DataKeyNames="Company,AcctYear" 
                                AutoGenerateColumns="False" ShowFooter="True" DataSourceID="sqlDatasource">
                                 
                                 <Columns>
                                        
                                      <asp:TemplateField HeaderText="刪除" ShowHeader="False">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  L2PK='<%# DataBinder.Eval(Container, "DataItem.AcctYear")%>'
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
                                        
                                     <asp:BoundField DataField="Company" HeaderText="公司別" ReadOnly="True" 
                                          SortExpression="Company" >
                                         <HeaderStyle  />
                                      </asp:BoundField>
                                     <asp:BoundField DataField="AcctYear" HeaderText="年度" 
                                          SortExpression="AcctYear"  />
                                      <asp:BoundField DataField="PeriodBegin01" HeaderText="期間01 起" 
                                          SortExpression="PeriodBegin01" />
                                      <asp:BoundField DataField="PeriodEnd01" HeaderText="期間01 迄" />
                                      <asp:BoundField DataField="PeriodEnd02" HeaderText="期間02 迄" />
                                      <asp:BoundField DataField="PeriodEnd03" HeaderText="期間03 迄" />
                                      <asp:BoundField DataField="PeriodEnd04" HeaderText="期間04 迄" />
                                      <asp:BoundField DataField="PeriodEnd05" HeaderText="期間05 迄" />
                                      <asp:BoundField DataField="PeriodEnd06" HeaderText="期間06 迄" />
                                      <asp:BoundField DataField="PeriodEnd07" HeaderText="期間07 迄" />
                                      <asp:BoundField DataField="PeriodEnd08" HeaderText="期間08 迄" />
                                      <asp:BoundField DataField="PeriodEnd09" HeaderText="期間09 迄" />
                                      <asp:BoundField DataField="PeriodEnd10" HeaderText="期間10 迄" />
                                      <asp:BoundField DataField="PeriodEnd11" HeaderText="期間11 迄" />
                                      <asp:BoundField DataField="PeriodEnd12" HeaderText="期間12 迄" />
                                      <asp:BoundField DataField="PeriodEnd13" HeaderText="期間13 迄" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table>
</ContentTemplate>
        </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB %>"
        ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

