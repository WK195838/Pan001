<%@ Page Language="C#" AutoEventWireup="true" CodeFile=" CodeDesc.aspx.cs" Inherits="CodeDesc" EnableEventValidation="false" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"  TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <base target="_self" />   
    <link href="~/App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    <!-- 頁面起始 -->
    <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
    <uc2:StyleTitle id="StyleTitle1" title="代碼項目檔" runat="server"></uc2:StyleTitle>
    <uc3:StyleContentStart id="StyleContentStart1" runat="server"></uc3:StyleContentStart> 

    <ul>
        <li style=" text-align :left">

        </li>

        <li style=" text-align :left">
            <span class="ItemFontStyle">項目編號：</span>
            <asp:TextBox id="txtCodeCode" runat="server" MaxLength="50"></asp:TextBox>
            <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
        </li>

        <li style=" text-align :left">
            <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label>
        </li>

        <li>
            <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator>
        </li>

        <li style=" text-align :left">
            <asp:Panel id="Panel_Empty" runat="server" Height="50px" width="250px" Visible="False"><br />查無資料!!</asp:Panel>
        </li>

        <li style=" text-align :left">
             <asp:GridView 
             id="GridView1" 
             runat="server" 
             Width="100%" 
             ShowFooter="true"
 
             DataKeyNames="CodeID,CodeCode" 
 
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
            <asp:TemplateField HeaderText="刪除" ShowHeader="False">

            <ItemTemplate>

            <asp:LinkButton 
            id="btnDelete"
            onclick="btnDelete_Click" 
            runat="server" 
            Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" 
            OnClientClick='return confirm("確定刪除?");' 
            L2PK='<%# DataBinder.Eval(Container, "DataItem.CodeCode")%>' 
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

            <HeaderStyle width="60px"></HeaderStyle>
            <ItemStyle width="60px"></ItemStyle>
            </asp:CommandField>

            <%--                             新增區段                            --%>

            <asp:BoundField DataField="CodeID"      HeaderText="代碼標題" ReadOnly="true" SortExpression="CodeID">      </asp:BoundField>
            <asp:BoundField DataField="CodeCode"    HeaderText="代碼項目編號" ReadOnly="true" SortExpression="CodeCode">    </asp:BoundField>
            <asp:BoundField DataField="CodeName"    HeaderText="代碼項目說明"  SortExpression="CodeName">                    </asp:BoundField>
            <asp:BoundField DataField="Maint"       HeaderText="維 護 碼"  SortExpression="Maint">                      </asp:BoundField>

            <%--                             新增區段結尾                            --%>

            </Columns>

            <FooterStyle horizontalalign="center">  </FooterStyle>
            <PagerStyle horizontalalign="left">     </PagerStyle>


            <EmptyDataTemplate>
            <div id="Box" runat="server">
            </div>
            </EmptyDataTemplate>

            <HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>

            </asp:GridView>
        </li>

        <li style=" text-align :Center">
            <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
        </li>

    </ul>



<%--///////////////////////////////////////////////////////////////////////////////////////////--%>

            <table id="NewTable"  visible="false" runat="server" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
            <tr class="button_bar_cell" align="center">
            <td class="paginationRowEdgeLl">新增</td>
            <td>代碼標題</td>
            <td>代碼項目編號</td>
            <td>代碼項目說明</td>
            <td runat="server" id="EmptyMaint">維 護 碼</td>
            </tr>

            <tr>
            <td class="Grid_GridLine"><asp:ImageButton ID="btAddNew" SkinID="NewAdd" CommandName="Insert" OnClick="btnEmptyNew_Click" runat="server" /></td>
            <td class="Grid_GridLine"><asp:DropDownList runat="server" ID="DropDownList1"></asp:DropDownList></td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew01" /></td>
            <td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>
            <td class="Grid_GridLine"><asp:DropDownList runat="server" ID="DropDownList2"></asp:DropDownList></td>
            </tr>
            </table>

            <div id="Temp" runat="server" visible="false">
            <asp:ImageButton ID="NewAdd" SkinID="NewAdd" CommandName="Insert" OnClick="btnEmptyNew_Click" runat="server" />
            </div>
<asp:SqlDataSource 
id="SDS_GridView" 
runat="server"
 
ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
SelectCommand="SELECT CodeDesc.* FROM CodeDesc " 
DeleteCommand="DELETE FROM CodeDesc WHERE CodeID = @CodeID AND CodeCode  = @CodeCode" 
UpdateCommand="UPDATE CodeDesc SET  CodeName = @CodeName, Maint = @Maint  WHERE CodeID = @CodeID AND CodeCode = @CodeCode" 
InsertCommand="INSERT INTO CodeDesc(CodeID, CodeCode, CodeName, Maint) VALUES (@CodeID, @CodeCode, @CodeName, @Maint)" >


<DeleteParameters>
<asp:Parameter Name="CodeID"></asp:Parameter>
<asp:Parameter Name="CodeCode"></asp:Parameter>
</DeleteParameters>

<UpdateParameters>
<asp:Parameter Name="CodeID"></asp:Parameter>
<asp:Parameter Name="CodeCode"></asp:Parameter>
<asp:Parameter Name="CodeName"></asp:Parameter>
<asp:Parameter Name="Maint"></asp:Parameter>
</UpdateParameters>

<InsertParameters>
<asp:Parameter Name="CodeID"></asp:Parameter>
<asp:Parameter Name="CodeCode"></asp:Parameter>
<asp:Parameter Name="CodeName"></asp:Parameter>
<asp:Parameter Name="Maint"></asp:Parameter>
</InsertParameters>

</asp:SqlDataSource>
<asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label> 
<br /><uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
    <!-- 頁面結束 -->
    </ContentTemplate>
    </asp:UpdatePanel>
<asp:HiddenField id="tempNewscid" runat="server"></asp:HiddenField> 
<asp:HiddenField id="tempNews" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_status" runat="server"></asp:HiddenField>  
<asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>  
    </form>
</body>
</html>

