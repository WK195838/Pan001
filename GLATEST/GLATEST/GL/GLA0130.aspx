<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0130.aspx.cs" Inherits="GLA0130" %>

<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>

<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc3" %>

<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc2" %>

<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
 
    <script type="text/javascript" language="javascript">
     function wop(strUrl)
     {
        var newWindow;
        strUrl = strUrl.replace(/%40/g,"'");
        newWindow = window.open(strUrl,"_New", "toolbar=no, status=yes, resizable=yes, scrollbars=yes, width=990, height=680");
        newWindow.focus( );
     }  
    </script>
     <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>   
    
        <div>     
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>&nbsp;
            <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
                ShowUser="false" Title="傳票更正作業" />
            <fieldset>
            <legend>&nbsp;傳票更正作業&nbsp;</legend>
            <asp:SqlDataSource ID="sdVoucher" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:EBosDB %>" 
                    ProviderName="<%$ ConnectionStrings:EBosDB.ProviderName %>"></asp:SqlDataSource>
                        <asp:Label ID="Label1" runat="server" Visible="False" meta:resourcekey="Label1Resource1"></asp:Label>
            <table style="width: 99%;" border="1" cellpadding="0" cellspacing="0">
                <tr>
                    <td align=left >
                        公司別：</td>
                    <td >
                        <uc3:CompanyList ID="DrpCompanyList" runat="server" />
                        <asp:Label ID="lblComp" runat="server" Visible="False" meta:resourcekey="lblCompResource1"></asp:Label></td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td >
                        傳票號碼：</td>
                    <td >
                        <asp:TextBox ID="txtVourNoS" runat="server" Width="130px" meta:resourcekey="txtVourNoSResource1"></asp:TextBox>～<asp:TextBox
                            ID="txtVourNoE" runat="server"  meta:resourcekey="txtVourNoEResource1"></asp:TextBox></td>
                    <td  colspan="2">
                        </td>
                </tr>
                <tr>
                    <td >
                        製單日期：</td>
                    <td >
                        <asp:TextBox ID="txtCreateDateS" runat="server" Width="100px" meta:resourcekey="txtCreateDateSResource1"></asp:TextBox>
                       <%-- <asp:ImageButton ID="ibOW_MakeSDate" runat="server" SkinID="Calendar1" />--%>～<asp:TextBox
                                ID="txtCreateDateE" runat="server" Width="100px" meta:resourcekey="txtCreateDateEResource1"></asp:TextBox>
                        <%--<asp:ImageButton ID="ibOW_MakeEDate" runat="server" SkinID="Calendar1" />--%></td>
                    <td >
                        製單者：</td>
                    <td>
                        <asp:TextBox ID="txtCreateUser" runat="server" meta:resourcekey="txtCreateUserResource1"></asp:TextBox></td>
                </tr>
                <tr>
                    <td >
                        傳票日期：</td>
                    <td >
                        <asp:TextBox ID="txtVourDateS" runat="server" Width="100px" meta:resourcekey="txtVourDateSResource1"></asp:TextBox>
                        <%--<asp:ImageButton ID="ibOW_VoucherSDate" runat="server" SkinID="Calendar1" />--%>～<asp:TextBox
                                ID="txtVourDateE" runat="server" Width="100px" meta:resourcekey="txtVourDateEResource1"></asp:TextBox>
                        <%--<asp:ImageButton ID="ibOW_VoucherEDate" runat="server" SkinID="Calendar1" />--%>
                        &nbsp;
                    </td>
                    <td >
                        傳票來源：</td>
                    <td><!--//配合傳票查詢改為下拉單--><CodeList:CodeList ID="txtVourSrc" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="傳票查詢"  meta:resourcekey="btnQueryResource1" />
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查看選取項目" Visible="False" meta:resourcekey="Button1Resource1" />
                        </td>
                    </tr>
                </table>
                </fieldset>
                <fieldset>
                <legend>&nbsp;查詢結果&nbsp;</legend>
                <table style="width: 99%;" border="1" cellpadding="0" cellspacing="0">
                <tr id="Tr1" runat="server">
                    <td colspan="4">
                            <asp:Button ID="btnCheckAll" runat="server" ForeColor="Red" Text="全部作廢" Width="85px" Visible="False" meta:resourcekey="btnCheckAllResource1" /><asp:Button ID="btnCheck" runat="server" Text="作廢選取項目" OnClick="btnCheck_Click" meta:resourcekey="btnCheckResource1" /></td>
                    </tr>
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <uc2:Navigator_GV ID="Navigator_GV1" runat="server" />
                                <uc1:StyleTitle ID="GLA0130Title" runat="server" ShowHome="false" ShowLogout="false"
                                    Title="傳票更正作業" ShowBackToPre="False" />
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataKeyNames="VoucherNo" DataSourceID="sdVoucher" OnRowDataBound="GridView1_RowDataBound"
                            PageSize="20" 
                            CellPadding="4"  Width="100%" EmptyDataText="無資料" GridLines="None" meta:resourcekey="GridView1Resource1">
                            <Columns>
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbID" runat="server" AutoPostBack="True" OnCheckedChanged="ckbID_CheckedChanged" meta:resourcekey="ckbIDResource1" />
                                        <asp:HiddenField ID="HidID" runat="server" Value='<%# Eval("VoucherNo") %>' />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="linkbtnAll" runat="server" OnClick="linkbtnAll_Click" meta:resourcekey="linkbtnAllResource1"></asp:LinkButton>
                                        <asp:LinkButton ID="linkbtnNo" runat="server" OnClick="linkbtnNo_Click" meta:resourcekey="linkbtnNoResource1"></asp:LinkButton>項次
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="VoucherNo" HeaderText="傳票號碼" meta:resourcekey="BoundFieldResource1" />
                                <asp:BoundField DataField="VoucherOwner" HeaderText="製票者" meta:resourcekey="BoundFieldResource2" />
                                <asp:BoundField DataField="VoucherEntryDate" HeaderText="製票日期" meta:resourcekey="BoundFieldResource3" />
                                <asp:BoundField DataField="VoucherDate" HeaderText="傳票日期" meta:resourcekey="BoundFieldResource4" />
                                <asp:BoundField DataField="RevDate" HeaderText="迴轉日期" meta:resourcekey="BoundFieldResource5" />
                                <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDetail" runat="server" OnClick="btnDetail_Click" Text="更正" Width="44px" meta:resourcekey="btnDetailResource1" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                      <HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
        <asp:HiddenField ID="hidAll" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            </fieldset>
        </div>
    


</asp:Content>

  
  
   
