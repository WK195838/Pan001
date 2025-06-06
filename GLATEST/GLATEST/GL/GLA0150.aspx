﻿<%@ Page Language="C#" MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLA0150.aspx.cs" Inherits="GLA0150" %>

<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>

<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator_GV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">  
    <script type="text/javascript" language="javascript">
    function SelectAllCheckboxes(spanChk) 
    {     
         elm=document.forms[0];  
         for(i=0;i<elm.length;i++)
          { 
          if(elm[i].type=="checkbox" && elm[i].id!=spanChk.id)     
             {      
              if(elm.elements[i].checked!=spanChk.checked)     
                 {
                 elm.elements[i].click();    
                 }
             } 
          } 
    }
    
    function wop(strUrl){
        var newWindow;
        strUrl = strUrl.replace(/%40/g,"'");
        newWindow = window.open(strUrl,"_New", "toolbar=no, status=yes, resizable=yes, scrollbars=yes, width=990, height=680");
        newWindow.focus( );
        //Bring window to front
        //window.showModelessDialog(strUrl, "_New", "dialogWidth:1000px; dialogHeight:720px; center:yes; resizable:yes; status:no");
        //window.location="GLA0350.aspx";
    } 
    </script>    
     <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>  
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
                ShowUser="false" Title="傳票取消核准作業" />
            <fieldset>
                <legend>&nbsp;取消核准作業&nbsp;</legend>
               
                <asp:SqlDataSource ID="sdVoucher" runat="server"></asp:SqlDataSource>
                <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>
                <table style="width: 99%;" border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td >
                            公司別：</td>
                        <td >
                            <uc3:CompanyList ID="DrpCompanyList" runat="server" />
                            <asp:Label ID="lblComp" runat="server" Visible="False"></asp:Label></td>
                        <td colspan="2">
                            </td>
                    </tr>
                    <tr>
                        <td >
                            傳票號碼：</td>
                        <td >
                            <asp:TextBox ID="txtVourNoS" runat="server" Width="130px"></asp:TextBox>～<asp:TextBox
                                ID="txtVourNoE" runat="server" Width="130px"></asp:TextBox></td>
                        <td  colspan="2">
                            </td>
                    </tr>
                    <tr>
                        <td>
                            製單日期：</td>
                        <td >
                            <asp:TextBox ID="txtCreateDateS" runat="server" Width="100px"></asp:TextBox>
                            <%--<asp:ImageButton ID="ibOW_CreateSDate" runat="server" SkinID="Calendar1" />--%>～<asp:TextBox
                                    ID="txtCreateDateE" runat="server" Width="100px"></asp:TextBox>
                            <%--<asp:ImageButton ID="ibOW_CreateEDate" runat="server" SkinID="Calendar1" />--%></td>
                        <td>
                            製單者：</td>
                        <td>
                            <asp:TextBox ID="txtCreateUser" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td >
                            傳票日期：</td>
                        <td>
                            <asp:TextBox ID="txtVourDateS" runat="server" Width="100px"></asp:TextBox>
                            <%--<asp:ImageButton ID="ibOW_VoncherSDate" runat="server" SkinID="Calendar1" />--%>～<asp:TextBox
                                    ID="txtVourDateE" runat="server" Width="100px"></asp:TextBox>
                            <%--<asp:ImageButton ID="ibOW_VoncherEDate" runat="server" SkinID="Calendar1" />--%></td>
                        <td>
                            傳票來源：</td>
                    <td><!--//配合傳票查詢改為下拉單--><CodeList:CodeList ID="txtVourSrc" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="傳票查詢" Width="88px" /></td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>&nbsp;查詢結果&nbsp;</legend>
                <table style="width: 99%;" border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnCheckAll" runat="server" ForeColor="Red" Text="全部核准" Visible="False"
                                Width="85px" /><asp:Button ID="btnCheck" runat="server" OnClick="btnCheck_Click"
                                    Text="取消核准選取項目" Width="133px" /></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <uc2:Navigator_GV ID="NavigatorPager" runat="server" />
                                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        CellPadding="4"  DataKeyNames="VoucherNo" DataSourceID="sdVoucher"
                                        EmptyDataText="無資料"  GridLines="None" OnPageIndexChanging="GridView1_PageIndexChanging"
                                        OnRowDataBound="GridView1_RowDataBound" PageSize="20" Width="100%">                                    
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="linkbtnAll" runat="server" OnClick="linkbtnAll_Click">全選</asp:LinkButton>／<asp:LinkButton
                                                        ID="linkbtnNo" runat="server" OnClick="linkbtnNo_Click">無</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ckbID" runat="server" AutoPostBack="True" OnCheckedChanged="ckbID_CheckedChanged" />
                                                    <asp:HiddenField ID="HidID" runat="server" Value='<%# Eval("VoucherNo") %>' />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VoucherNo" HeaderText="傳票號碼" />
                                            <asp:BoundField DataField="VoucherOwner" HeaderText="製票者" />
                                            <asp:BoundField DataField="VoucherEntryDate" HeaderText="製票日期" />
                                            <asp:BoundField DataField="VoucherDate" HeaderText="傳票日期" />
                                            <asp:BoundField DataField="RevDate" HeaderText="迴轉日期" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDetail" runat="server" Text="明細" OnClick="btnDetail_Click" />
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
