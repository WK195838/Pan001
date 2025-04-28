<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="DataChangeLog.aspx.cs" Inherits="DataChangeLog" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="資料異動紀錄" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">使用類型：</span>&nbsp;</td>
                        <td align="left">                            
                            <uc1:CodeList id="CodeList1" runat="server" />
                        </td>
                    </tr>
                     <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">使用項目：</span>&nbsp;</td>
                        <td align="left">                            
                            <uc1:CodeList id="CodeList2" runat="server" />
                        </td>
                    </tr>
                     <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">使用期間：</span>&nbsp;</td>
                        <td align="left">                            
                            <asp:TextBox ID="txtDateS" runat="server" Width="100px"></asp:TextBox>                            
                            ～<asp:TextBox ID="txtDateE" runat="server" Width="100px"></asp:TextBox>                            
                        </td>
                    </tr>                    
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">使用目標：</span>&nbsp;</td>
                        <td align="left">
                            <asp:TextBox ID="tbQuery" runat="server" MaxLength="50"></asp:TextBox>                            
                            <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />
                            <asp:ImageButton id="btnToExcel" onclick="btnToExcel_Click" runat="server" SkinID="Excel1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc7:Navigator ID="Navigator1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Panel ID="Panel_Empty" runat="server" Height="50px" Visible="False" Width="250px">
                                <br />查無資料!!</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="TableName,TrxType" Width="100%" 
                             OnRowDataBound="GridView1_RowDataBound" 
                              PageSize="20" OnRowCreated="GridView1_RowCreated" GridLines="None"
                              DataSourceID="SDS_GridView" AllowPaging="True" AllowSorting="True"
                              >                              
                                 <Columns>                                 
                                        <asp:TemplateField HeaderText="查詢" HeaderStyle-Wrap="false">
                                            <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="False"
                                            Text="<img src='../App_Themes/images/select1.gif' border='0' alt='查詢'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" ></asp:LinkButton>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                        
                                     <asp:BoundField DataField="TableName" HeaderText="目標(表格或網頁)" ReadOnly="True" SortExpression="TableName" />
                                     <asp:BoundField DataField="TrxType" HeaderText="使用類型" SortExpression="TrxType" HeaderStyle-Wrap="false" />
                                     <asp:BoundField DataField="ChangItem" HeaderText="使用項目" SortExpression="ChangItem" />
                                     <asp:BoundField DataField="ChgUser" HeaderText="使用者" SortExpression="ChgUser" />
                                     <asp:BoundField DataField="ChgStartDateTime" HeaderText="使用時間" SortExpression="ChgStartDateTime" />
                                 </Columns>
                             </asp:GridView>
                        </td>
                    </tr>
                </table>            
             <asp:SqlDataSource ID="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
                  SelectCommand="SELECT DataChangeLog.* FROM DataChangeLog Order By TableName,TrxType,ChgStartDateTime DESC">
             </asp:SqlDataSource>
             <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>

