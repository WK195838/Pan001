<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="CodeMaster.aspx.cs" Inherits="CodeMaster" EnableEventValidation="false"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"   TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<!-- 頁面內容開始 -->
<uc2:StyleTitle id="StyleTitle1" title="代碼基本資料檔" runat="server"></uc2:StyleTitle> 
<uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart>

<ul>
    <li style=" text-align:left">
        <span class="ItemFontStyle">代碼欄位：</span>
        <asp:TextBox id="txtCodeID" runat="server" MaxLength="50"></asp:TextBox>
    </li>

    <li style=" text-align:left">
        <span class="ItemFontStyle">代碼名稱：</span>
        <asp:TextBox id="txtCodeDecs" runat="server" MaxLength="50"></asp:TextBox>
        <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    </li>

    <li>
        <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label> 
    </li>

    <li>
         <asp:Panel id="Panel_Empty" runat="server" Height="50px" Visible="False" width="250px"><br />查無資料!!</asp:Panel>
    </li>

    <li>
        <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator>
    </li>

    <li>
         <%--------GridView 屬性設定--------%>
         <asp:GridView 
         id="GridView1" runat="server" 
         Width="100%" 
         ShowFooter="true"
         DataKeyNames="CodeID" 
         DataSourceID="SDS_GridView"
         OnRowDataBound="GridView1_RowDataBound" 
         OnRowCommand="GridView1_RowCommand" 
         OnRowUpdated="GridView1_RowUpdated" 
         OnRowUpdating="GridView1_RowUpdating" 
         OnRowCancelingEdit="GridView1_RowCancelingEdit" 
         OnRowEditing="GridView1_RowEditing" 
         OnRowCreated="GridView1_RowCreated" 
         GridLines="None" 
         AutoGenerateColumns="False" 
         AllowPaging="true" 
         AllowSorting="true">

        <%--------欄位水平對齊設定--------%>
        <RowStyle horizontalalign="Center"></RowStyle>


        <%--------列的內容設定開始--------%>
        <Columns>
        <%--------刪除按鈕設定------------%>
        <asp:TemplateField HeaderText="刪除" ShowHeader="False" >
        <ItemTemplate>

        <asp:LinkButton 
        id="btnDelete" 
        onclick="btnDelete_Click" 
        runat="server" 
        Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
        OnClientClick='return confirm("確定刪除?");' 
        L1PK='<%# DataBinder.Eval(Container, "DataItem.CodeID")%>'

        CausesValidation="False">
        </asp:LinkButton> 

        </ItemTemplate>
        <HeaderStyle width="30px"></HeaderStyle>
        </asp:TemplateField>

        <%--------編輯按鈕設定------------%>
        <asp:CommandField 
        CancelImageUrl="~/App_Themes/images/cancel1.gif" 
        EditImageUrl="~/App_Themes/images/edit1.GIF" 
        ShowEditButton="true" 
        UpdateImageUrl="~/App_Themes/images/saveexit1.gif" 
        ButtonType="Image" 
        HeaderText="編輯">

        <HeaderStyle width="30px"></HeaderStyle>
        <ItemStyle width="30px"></ItemStyle>
        </asp:CommandField>

        <asp:TemplateField HeaderText="明　　細">
        <ItemTemplate>
        <asp:Button ID="ViewDesc" runat="server" Text="檢視項目" Code='<%# DataBinder.Eval(Container, "DataItem.CodeID")%>'/>
        </ItemTemplate>
        </asp:TemplateField>



        <%--------要顯示出的欄位設定------%>
            <asp:BoundField DataField="CodeID"      HeaderText="代碼欄位" ReadOnly="true" SortExpression="CodeID"/>
            <asp:BoundField DataField="CodeDecs"    HeaderText="代碼名稱"  SortExpression="CodeDecs"/>                   
            <asp:BoundField DataField="CodeLen"     HeaderText="代碼長度"  SortExpression="CodeLen"/>                    
            <asp:BoundField DataField="CodeDescLen" HeaderText="說明長度"  SortExpression="CodeDescLen"/>                
            <asp:BoundField DataField="Maint"       HeaderText="維 護 碼"  SortExpression="Maint" ItemStyle-Width="30px" />
            
            
        </Columns>
        <%--------列的內容設定結束--------%>


        <FooterStyle horizontalalign="center" />

        <PagerStyle horizontalalign="left" />
        
        <EmptyDataTemplate>

        </EmptyDataTemplate>

        <HeaderStyle horizontalalign="Center" cssclass="button_bar_cell"></HeaderStyle>

        </asp:GridView>
    </li>

    <li>
        <table id="NewTable" runat="server" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;" >
        <tr class="button_bar_cell" align="center">
        <td class="paginationRowEdgeLl" style=" width:30px" >新增</td>
        <td>代碼欄位</td>
        <td>代碼名稱</td>
        <td>代碼長度</td>
        <td>說明長度</td>
        <td runat="server" id="EmptyMaint">維 護 碼</td>
        </tr>

        <tr>
        <td class="Grid_GridLine" align="center">
        <asp:ImageButton id="btnNew" onclick="CodeMasterbtnNew_Click" runat="server" SkinID="NewAdd"/>
        </td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="CodeMasterNew01" /></td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="CodeMasterNew02"/></td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="CodeMasterNew03"/></td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="CodeMasterNew04"/></td>
            <td class="Grid_GridLine">
            <asp:DropDownList runat="server" ID="CodeMasterNew05">
            <asp:ListItem Text="啟用" Value="Y"></asp:ListItem>
            <asp:ListItem Text="關閉" Value="N"></asp:ListItem>
            </asp:DropDownList>
            </td>
            <td></td>
        </tr>
        </table>

    </li>
    
</ul>

<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 





</ContentTemplate>
</asp:UpdatePanel>

<asp:SqlDataSource id="SDS_GridView" runat="server" 
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
 
SelectCommand="SELECT CodeMaster.* FROM CodeMaster "

InsertCommand="INSERT INTO CodeMaster(
CodeID, 
CodeDecs, 
CodeLen, 
CodeDescLen, 
Maint
) 
VALUES (
@CodeID, 
@CodeDecs, 
@CodeLen, 
@CodeDescLen, 
@Maint)" 

UpdateCommand="UPDATE CodeMaster SET
CodeDecs = @CodeDecs,
CodeLen = @CodeLen,
CodeDescLen = @CodeDescLen,
Maint = @Maint
WHERE 
CodeID= @CodeID
"
 
DeleteCommand="DELETE FROM CodeMaster 
WHERE 
CodeID= @CodeID)
">
             
                 <InsertParameters>
                         <asp:Parameter Name="CodeID"       />
                         <asp:Parameter Name="CodeDecs"     />
                         <asp:Parameter Name="CodeLen"      />
                         <asp:Parameter Name="CodeDescLen"  />
                         <asp:Parameter Name="Maint"        />
                 </InsertParameters>
                 
                 <UpdateParameters>
                         <asp:Parameter Name="CodeID"       />
                         <asp:Parameter Name="CodeDecs"     />
                         <asp:Parameter Name="CodeLen"      />
                         <asp:Parameter Name="CodeDescLen"  />
                         <asp:Parameter Name="Maint"        />
                 </UpdateParameters>
                 
                  <DeleteParameters>
                         <asp:Parameter Name="CodeID"       />
                 </DeleteParameters>       
                          
</asp:SqlDataSource> 

<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
</asp:Content>
 
