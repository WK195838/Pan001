<%@ Page Title="" Language="C#" MasterPageFile="~/ePayroll.master" AutoEventWireup="true" CodeFile="MonthStar.aspx.cs" Inherits="MonthStar" %>
<%@ Register src="UserControl/MonthList.ascx" tagname="MonthList" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <div id="divBG" style=" height:475px; width:850px">
    <div id="divEMailBG" runat="server" style="position:absolute;display:none;z-index:90; bottom:0px; right:0px; background: url(image/PanUI/busybg.png); width:100%; height:100%;">
    <div id="divEMail" runat="server" style="position:absolute;z-index:99; background:#000; width:500px; height:350px; text-align:center">
    <ul style=" width:99%; height:99%; background:#FFF; text-align:center; vertical-align:middle;">
    <li>
    <p style="text-align:left; font-size:large">主題：</p><asp:TextBox ID="txtSub" runat="server" Columns="25" Font-Size="X-Large"></asp:TextBox>
    </li>
    <li>
    <p style="text-align:left; font-size:large">內容：</p><asp:TextBox ID="txtContext" runat="server" Rows="5" TextMode="MultiLine" Columns="22" Font-Size="X-Large"></asp:TextBox>
    </li>
    <li>
    <p style="text-align:left; font-size:large">寄件人署名：</p><asp:TextBox ID="txtMailFromName" runat="server" Columns="35" Font-Size="Large"></asp:TextBox>
    </li>
    <li>
    <p style="text-align:left; font-size:large">寄件人郵箱：</p><asp:TextBox ID="txtMailFrom" runat="server" Columns="35" Font-Size="Large"></asp:TextBox>
    </li>
    <li>
    <asp:Button ID="SentMail" runat="server" SkinID="btnLarge" onclick="SentMail_Click" Text="送出" />
    　
    <asp:Button ID="Clear" runat="server" SkinID="btnLarge" onclick="Clear_Click" Text="取消" />
    </li>   
    </ul> 
    </div>
    </div>
    <div id="divMsg" runat="server" style="position:absolute;top:250px; right:250px;display:none;z-index:80; background:#000; width:300px; height:100px; text-align:center">
     <ul style=" width:99%; height:99%; background:#FFF;">
    <li>
    <p style="text-align:left; color:Red; font-weight:bold;">主旨或訊息未輸入，已使用系統預設祝福語送出!</p>
    </li>
    <li>
    <asp:Button ID="closeMsg" runat="server" SkinID="btnLarge" onclick="Clear_Click"  Text="確認" />
    </li>
    </ul>    
    </div>
    每月壽星：<uc1:MonthList ID="MonthList1" runat="server" AutoPostBack="True" CssClass="buttonLarge" />
    <br>
    <div id="divStar" runat="server">
    </div>
    </div>
    <asp:HiddenField ID="hfMailto" runat="server" />
    <asp:HiddenField ID="hfStarName" runat="server" />
</asp:Content>

