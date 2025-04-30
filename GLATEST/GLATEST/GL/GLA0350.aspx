<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0350.aspx.cs" Inherits="GLA0350" %>

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
<uc2:StyleTitle id="StyleTitle1" title="報表格式設定" runat="server"></uc2:StyleTitle> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
    <tr class="QueryStyle">
        <td align="left" style="position: relative">
            <table>
                <tr>
                    <td>
                        公司別：</td>
                    <td>
                        <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="false" />
                    </td>
                    <td >
                        公司簡稱：</td>
                    <td>
                        <asp:TextBox ID="txtcompName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        報表種類：</td>
                    <td>
                        <asp:DropDownList ID="DrpReportType" runat="server">
                        </asp:DropDownList></td>
                    <td >
                        報表代號：</td>
                    <td >
                        <asp:TextBox ID="txtReportID" runat="server"></asp:TextBox></td>
                </tr>
            </table>
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
                                DataKeyNames="Company,ReportType,ReportID" 
                                AutoGenerateColumns="False" ShowFooter="True" DataSourceID="sqlDatasource">
                                 
                                 <Columns>
                                        
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
                                        
                                     <asp:BoundField DataField="Company" HeaderText="公司別" ReadOnly="True" SortExpression="Company" />
                                     <asp:BoundField DataField="companyshortName" HeaderText="公司簡稱" SortExpression="companyshortName" />
                                     <asp:BoundField DataField="ReportType" HeaderText="報表種類" SortExpression="ReportType" />
                                     <asp:BoundField DataField="ReportID" HeaderText="報表代號" SortExpression="ReportID" />
                                     <asp:BoundField DataField="ReportName" HeaderText="報表名稱" SortExpression="ReportName" />
                                     <asp:BoundField DataField="FormatID" HeaderText="格式" SortExpression="FormatID" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table>
</ContentTemplate>
        </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB %>"
        ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

