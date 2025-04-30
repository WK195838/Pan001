<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLI01C0.aspx.cs" Inherits="GL_GLI01C0" %>

<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc4" %>

<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc3" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>


<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc1" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

 <%@ Register src="~/UserControl/CompanyList.ascx" tagname="CompanyList" tagprefix="uc5" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">  
<script language="javascript" type="text/jscript">   
　　
    function wop(tValue) {
        __doPostBack('SetSessionPostBack', tValue);
    }   
</script>
 <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
 <script language="javascript" type="text/javascript" src="~/Pages/ModPopFunction.js"></script>  
      
    <div>
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc2:StyleTitle ID="StyleTitle2" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="分類帳查詢作業" />
        <fieldset>
        <legend>&nbsp;分類帳查詢作業&nbsp;</legend>
            
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                <tr>
                    <td style="text-align:right; width:10%;">
                        公司別：</td>
                    <td style="width:30%;">
                        <uc5:CompanyList ID="CompanyList1" runat="server" AutoPostBack="False" />
                    </td>
                    <td style="text-align:right; width:10%;">
                        會計科目：</td>
                    <td style="width:50%;">
                        <asp:TextBox ID="txtAcctNo" runat="server" Width="100px"></asp:TextBox>
                        <asp:ImageButton ID="imgbtnAcctNo" runat="server" ImageUrl="~/Image/ButtonPics/Query.gif" />
                        <asp:TextBox ID="txtAcctName" runat="server" BorderStyle="None" BorderWidth="0px" CssClass="TextAlignLeftBold" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        傳票日期：</td>
                    <td>
                        <asp:TextBox ID="txtVoucherSDate" runat="server" Width="100px"></asp:TextBox>
                        <%--<asp:ImageButton ID="ibOW_VoucherSDate" runat="server" SkinID="Calendar1" />--%></td>
                    <td style="text-align:right;">
                        </td>
                    <td>
                        </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        選擇內容：</td>
                    <td>
                        <asp:DropDownList ID="ddlContent" runat="server">
                          <asp:ListItem Value="1">1.已過帳</asp:ListItem>
                          <asp:ListItem Value="2">2.已簽核</asp:ListItem>
                          <asp:ListItem Selected="True" Value="3">3.全部</asp:ListItem>
                        </asp:DropDownList></td>
                    <td style="text-align:right;">
                        追溯分攤：</td>
                    <td>
                        <asp:DropDownList ID="ddlAllocation" runat="server">
                          <asp:ListItem Value="Y">Y.含</asp:ListItem>
                          <asp:ListItem Selected="True" Value="N">N.不含</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="height: 20px">
                        </td>
                    <td style="height: 20px">
                        &nbsp;</td>
                    <td style="height: 20px">
                       <asp:TextBox ID="txtValue" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    <td style="height: 20px">
                        </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;<asp:ImageButton ID="imgbtnPosting" runat="server" ImageUrl="~/Image/ButtonPics/query_button.gif" OnClick="imgbtnPosting_Click" /></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </fieldset>
    </div>
     <asp:SqlDataSource ID="GridDataSource" runat="server" 
         ConnectionString="<%$ ConnectionStrings:EBosDB %>" 
         ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
    <br />
    <div>
        <fieldset>
        <legend>&nbsp;查詢結果&nbsp;</legend>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc1:navigator_gv id="NavigatorPager" runat="server">
</uc1:navigator_gv>
                      <uc2:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="False" 
                        ShowHome="False" ShowUser="False" Title="分類帳查詢作業" />
                    <asp:GridView ID="GridMaster" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        BorderStyle="None" CellPadding="4" DataKeyNames="VoucherSeqNo"
                        EmptyDataText="無資料" GridLines="Vertical" PageSize="20" Width="99%" 
                        OnRowDataBound="GridMaster_RowDataBound" DataSourceID="GridDataSource">
                        <Columns>
                            <asp:BoundField DataField="JournalDate" HeaderText="傳票日期" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VoucherNo" HeaderText="傳票號碼" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VoucherSeqNo" HeaderText="序號" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DBAmt" HeaderText="借方金額" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CRAmt" HeaderText="貸方金額" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BalanceAmt" HeaderText="餘額" >
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApvlFlag" HeaderText="核准" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PostFlag" HeaderText="過帳" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass="button_bar_cell" HorizontalAlign="Center" />
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                 
                    <asp:HiddenField ID="hidAll" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
           
        </fieldset>   
    </div>   
     </asp:Content>