<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BonusMaster_U.aspx.cs" Inherits="BonusMaster_U" validaterequest="false" EnableEventValidation="false" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList" TagPrefix="uc7" %>

<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>新增獎金發放</title>
<base target="_self" />    
<script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
   <%-- <link href="../App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />--%>
     <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
     <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />  
    <style type="text/css">
        .style2
        {
            width: 524px;
            height: 29px;
        }
        .style4
        {
            width: 524px;
            height: 35px;
        }
        .style5
        {
            width: 524px;
            height: 28px;
        }
        .style7
        {
            width: 1451px;
            height: 14px;
        }
        .style9
        {
            width: 524px;
            height: 25px;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div>
<uc2:ShowMsgBox ID="ShowMsgBox1" runat="server" />
<uc1:StyleHeader ID="StyleHeader1" runat="server" />
<uc3:StyleTitle ID="StyleTitle1" runat="server" Title="新增獎金發放" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
<asp:HiddenField ID="hid_InserMode" runat="server" />
</div>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table style="height: 394px; width: 100%;">
            <tr>
                <td align="center" class="style7">
                    <table style="width: 564px; height: 157px;">
                        <tr>
                            <td style="height: 7px; width: 524px;" align="left" id="DetailsView1">
                                &nbsp;<uc7:SearchList ID="SearchList1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style2">
                                獎金名目：<asp:DropDownList ID="Money_name" runat="server" Width="163px" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9">
                                獎金金額：<asp:RadioButton ID="RB_standard" runat="server" Text="定額：" 
                    AutoPostBack="true"  OnCheckedChanged="RB_standard_CheckedChanged" />
                                <asp:TextBox ID="Tx_Amount" runat="server" Width="107px"></asp:TextBox>
                                &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;<br /> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:RadioButton ID="RB_Person" runat="server" Text="比例：" AutoPostBack="true" 
                    OnCheckedChanged="RB_Person_CheckedChanged" Width="72px" />
                                <asp:DropDownList ID="DP_Person" runat="server" Width="115px" 
                    OnSelectedIndexChanged="DP_Person_SelectedIndexChanged">
                                </asp:DropDownList>
                                Ｘ<asp:TextBox ID="Tx_multiple" runat="server" Width="37px" 
                    AutoPostBack="True" MaxLength="6" OnTextChanged="Tx_multiple_TextChanged"></asp:TextBox>
                                倍<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="計算" />
                                <asp:Label ID="lb_money" runat="server" Font-Bold="True" ForeColor="Red"
                    Width="137px" Height="46px"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" id="DetailsView4" runat ="server" class="style4" >
                                獎金發放日期：<asp:TextBox ID="TxDate" runat="server" Width="133px" MaxLength="10" ></asp:TextBox>
                                &nbsp;
                                <asp:HiddenField ID="LastPayDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" id="DetailsView5" runat ="server" class="style5">
                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 存入銀行：<asp:TextBox ID="BankCode" runat="server" Width="129px" MaxLength="3" 
                    Text =""></asp:TextBox>
                                &nbsp;
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*.供轉帳使用,發放現金時免填"
                    Width="230px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 23px; width: 524px;" align="left">
                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 存入帳號：<asp:TextBox ID="BankNumber" runat="server" Width="127px" 
                                    MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 23px; width: 524px;" align="left">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 是否已發放：<asp:DropDownList ID="ControlDown" runat="server">
                                    <asp:ListItem Value="Y">Y-是</asp:ListItem>
                                    <asp:ListItem Value="N" Selected="True">N-否</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label>
                    <tr><td align="center" style="width: 1451px">
    &nbsp;<asp:ImageButton ID="btnAdd" runat="server" CommandName="Insert" OnClick="btnAdd_Click " SkinID="SG1" />
          <asp:ImageButton ID="btnUpdate" runat="server" SkinID="SE1" CommandName="Update" OnClick="btnUpdate_Click" />
          <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" OnClick="btnCancel_Click" />
                        <uc2:ShowMsgBox ID="ShowMsgBox2" runat="server" />
</td></tr>
                </td>
            </tr>
            <tr>
            <td>
            <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="True" 
            GridLines="None" 
            ShowFooter="True"  
            OnRowCreated="GridView1_RowCreated"  
            OnRowDataBound="GridView1_RowDataBound"          
            >
        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<EmptyDataTemplate>

</EmptyDataTemplate>
</asp:GridView>             
            </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hid_SalaryId" runat="server" />  
<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
<uc6:StyleFooter ID="StyleFooter1" runat="server" />
    <asp:TextBox ID="MessageBox1" runat="server" Visible="False"></asp:TextBox><br />
</form>
</body>
</html>

