<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0430.aspx.cs" Inherits="GLA0430" %>

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

<script language ="javascript" type="text/javascript"�@>


</script>    

<div>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox>
<uc2:StyleTitle id="StyleTitle1" title="�R�b�զX��ƺ��@" runat="server"></uc2:StyleTitle> 
<table id="table1" cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- �j�M�Ҳ� -->
    <tr class="QueryStyle">
        <td align="left" style="position: relative">
            <table>
                <tr>
                    <td>
                        ���q�s���G</td>
                    <td>
                        <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="false" />
                    </td>
                    <td >
                        �R�b�N���G</td>
                    <td>
                        <asp:TextBox ID="txtOffsetID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        �����G</td>
                    <td colspan="3">
                        <asp:TextBox ID="TxtDesc" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="BtnQuery" runat="server" Text="�d��" /></td>
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
                                <br />�d�L���!!</asp:Panel> </td></tr>
                                <tr><td colspan=4><asp:GridView id="GridView1" runat="server" Width="100%" AllowPaging="True" 
                                OnRowCreated="GridView1_RowCreated" 
                                OnRowDeleting="GridView1_RowDeleting" 
                                OnRowDeleted="GridView1_RowDeleted" 
                                OnRowDataBound="GridView1_RowDataBound" 
                                DataKeyNames="Company,OffsetID" 
                                AutoGenerateColumns="False" ShowFooter="True" DataSourceID="sqlDatasource">
                                 
                                 <Columns>
                                        <asp:TemplateField HeaderText="�R��" ShowHeader="False">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False"  OnClick="btnDelete_Click" L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>'  L2PK='<%# DataBinder.Eval(Container, "DataItem.OffsetID")%>'
                                            OnClientClick='return confirm("�T�w�R��?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px"  />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="�s��">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/edit1.gif' border='0' alt='�s��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="�Ӷ�">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='�Ӷ�'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                        </asp:TemplateField>
                                        
                                     <asp:BoundField DataField="Company" HeaderText="���q�O" ReadOnly="True" SortExpression="Company" />
                                     <asp:BoundField DataField="OffsetID" HeaderText="�R�b�N��" SortExpression="OffsetID" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="Description" HeaderText="����" />
                                     <asp:BoundField DataField="SeqCtl" HeaderText="�ƧǶ���" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="SubTotCtl" HeaderText="�ƧǱҥα���" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LstChgUser" HeaderText="�̫��s��" >
                                         <ItemStyle HorizontalAlign="Center" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LstChgDateTime" HeaderText="�̫��s�ɶ�" SortExpression="LstChgDateTime" />
                                 </Columns>
                             </asp:GridView> </td></tr></tbody></table>
</ContentTemplate>
        </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlDatasource" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB %>"
        ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

