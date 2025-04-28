<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="IncomeTaxRateRefer.aspx.cs" Inherits="IncomeTaxRateRefer" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
 <script language="javascript" type="text/javascript"></script>

<div>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>


<uc2:StyleTitle id="StyleTitle1" title="�ұo�|�v��Ӫ�" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>


 <table id="table1" cellspacing="0" cellpadding="0" width="100%"><tbody>
 
 <%------------------------------�����W��j�M�����϶}�l------------------------------%>
 <tr>
 <td align="left"><span class="ItemFontStyle">�ұo�|�v�G</span>
 <asp:TextBox id="txtIncomeTaxRate" runat="server" MaxLength="50" ></asp:TextBox>
 <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton></td>
 </tr>
 

 <%------------------------------�����W��j�M�����ϵ���------------------------------%>
 
 <tr><td colspan="2">
 <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label> 
 </td></tr>
 
 <tr><td colspan="2"></td></tr>
 
 <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td></tr>
 
 <tr><td align="left" colspan="2">
 <asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />�d�L���!!</asp:Panel>
 </td></tr>
 
 <%------------------------------GridView �]�w�}�l----------------------------------%>
 <tr><td colspan="2">
 <%--------GridView �ݩʳ]�w--------%>
 <asp:GridView 
 id="GridView1" 
 runat="server" 
 Width="100%" 
 ShowFooter="true"
 
 DataKeyNames="IncomeTaxRate" 

 DataSourceID="SDS_GridView"
 OnRowDataBound="GridView1_RowDataBound" 
 OnRowCommand="GridView1_RowCommand" 
 OnRowUpdated="GridView1_RowUpdated" 
 OnRowUpdating="GridView1_RowUpdating" 
 OnRowCancelingEdit="GridView1_RowCancelingEdit" 
 OnRowEditing="GridView1_RowEditing" 
 OnRowCreated="GridView1_RowCreated" 
 GridLines="None" 
 OnRowDeleting="GridView1_RowDeleting" 
 OnRowDeleted="GridView1_RowDeleted" 
 OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
 AutoGenerateColumns="False" 
 AllowPaging="true" 
 AllowSorting="true">

<%--------����������]�w--------%>
<RowStyle horizontalalign="Center"></RowStyle>


<%--------�C�����e�]�w�}�l--------%>
<Columns>
<%--------�R�����s�]�w------------%>
<asp:TemplateField HeaderText="�R��" ShowHeader="False">
<ItemTemplate>

<asp:LinkButton 
id="btnDelete" 
onclick="btnDelete_Click" 
runat="server" 
Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
OnClientClick='return confirm("�T�w�R��?");' 
L1PK='<%# DataBinder.Eval(Container, "DataItem.IncomeTaxRate")%>'
CausesValidation="False" 
__designer:wfdid="w3">
</asp:LinkButton> 

</ItemTemplate>
<HeaderStyle width="30px"></HeaderStyle>
</asp:TemplateField>

<%--------�s����s�]�w------------%>
<asp:CommandField 
CancelImageUrl="~/App_Themes/images/cancel1.gif" 
EditImageUrl="~/App_Themes/images/edit1.GIF" 
ShowEditButton="true" 
UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
ButtonType="Image" 
HeaderText="�s��">

<HeaderStyle width="60px"></HeaderStyle>
<ItemStyle width="60px"></ItemStyle>
</asp:CommandField>



<%--------�n��ܥX�����]�w------%>
<asp:BoundField DataField="IncomeTax_LowerLimit"    HeaderText="�ұo�|�U��" SortExpression="IncomeTax_LowerLimit">                    </asp:BoundField>
<asp:BoundField DataField="IncomeTax_UpperLimit"    HeaderText="�ұo�|�W��" SortExpression="IncomeTax_UpperLimit">                    </asp:BoundField>
<asp:BoundField DataField="IncomeTaxRate"           HeaderText="�ұo�|�v"   SortExpression="IncomeTaxRate"        ReadOnly="true">    </asp:BoundField>
<asp:BoundField DataField="ProgressiveDiffAmount"   HeaderText="�ֶi�t�B"   SortExpression="ProgressiveDiffAmount">                   </asp:BoundField>

</Columns>
<%--------�C�����e�]�w����--------%>

<%--------�̫�@�C����������]�w--%>
<FooterStyle horizontalalign="center">  </FooterStyle>
<%--------��������������]�w------%>
<PagerStyle horizontalalign="left">     </PagerStyle>

<%--------�]�w�ťո�ƦC----------%>
<EmptyDataTemplate>

<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell">
<td class="paginationRowEdgeLl">�s�W</td>
<td>�ұo�|�W��</td>
<td>�ұo�|�U��</td>
<td>�ұo�|�v</td>
<td>�ֶi�t�B</td>

</tr>

<tr>
<td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>

</tr>


</table>

</EmptyDataTemplate>

<HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

</asp:GridView>
</td></tr>
 <%------------------------------GridView �]�w����----------------------------------%>
<%-- <tr>
<td align="left"><asp:TextBox ID="Textbox1" TextMode="MultiLine" Width="500px" Height="300px" runat="server"></asp:TextBox></td>
</tr>--%>
 <%----------------------------------------------------------------%>
<tr><td align="left" colspan="2"></td></tr>
</tbody></table>


<asp:SqlDataSource 
id="SDS_GridView" 
runat="server" 
OnSelecting="SDS_GridView_Selecting"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT IncomeTaxRate_Refer.* FROM IncomeTaxRate_Refer "

InsertCommand="INSERT INTO IncomeTaxRate_Refer(IncomeTax_UpperLimit, IncomeTax_LowerLimit, IncomeTaxRate, ProgressiveDiffAmount ) 
               VALUES (@IncomeTax_UpperLimit, @IncomeTax_LowerLimit, @IncomeTaxRate, @ProgressiveDiffAmount )" 

UpdateCommand="UPDATE IncomeTaxRate_Refer SET 
                IncomeTax_UpperLimit = @IncomeTax_UpperLimit, 
                IncomeTax_LowerLimit = @IncomeTax_LowerLimit, 
                ProgressiveDiffAmount = @ProgressiveDiffAmount
               WHERE IncomeTaxRate = @IncomeTaxRate"
 
DeleteCommand="DELETE FROM IncomeTaxRate_Refer WHERE IncomeTaxRate = @IncomeTaxRate" >
             
                 <InsertParameters>
                     <asp:Parameter Name="IncomeTax_UpperLimit"     />
                     <asp:Parameter Name="IncomeTax_LowerLimit"     />
                     <asp:Parameter Name="IncomeTaxRate"            />
                     <asp:Parameter Name="ProgressiveDiffAmount"    />
                 </InsertParameters>
                 
                 <UpdateParameters>
                     <asp:Parameter Name="IncomeTax_UpperLimit"     />
                     <asp:Parameter Name="IncomeTax_LowerLimit"     />
                     <asp:Parameter Name="IncomeTaxRate"            />
                     <asp:Parameter Name="ProgressiveDiffAmount"    />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                     <asp:Parameter Name="IncomeTaxRate"            />
                 </DeleteParameters>       
                          
</asp:SqlDataSource> 

<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
