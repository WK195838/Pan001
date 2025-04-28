<%@ Page language="c#" AutoEventWireup="true" CodeFile="AuthAD.aspx.cs" Inherits="AuthAD" %>
<%@ Register src="UserControl/CheckAuthorization.ascx" tagname="CheckAuthorization" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head id="Head1" runat="server">
	<title>登入 EEOC Web</title>
	    <style type='text/css'> 

	    #bottom_login {
		    width:100%; 
		    position:absolute; 
		    top:100%;
		    left:0; 
		    margin-top:-20px;  
            background:url(App_Themes/images/MasterTop.png);
		    color: #FFFFFF;
	    }
	    
	    .errorLabel
	    {
	    	display:block;
	    }

        .chkPersist
        {
        	position:relative;
        	top:2px;
        	padding-right: 2px;
        }
        
        .Div
        {
        	padding-top:10px;
        }
        
    </style>
	</head>
	
<body>
<form id="Login" method="post" runat="server">

<div> 
    <div style=" height:80px; background:url(App_Themes/images/MasterTop.png) ;">
    <img alt="EIP" src="App_Themes/images/EIPLogo.png"/>
    </div>
</div>

<div style=" text-align:center; margin-top:10%; ">

    <div>
        <img alt="Title" src="App_Themes/ERP/images/LogoTitle.png" />
    </div>
    
    <div class="Div">
        <asp:Label ID="Label2" Runat="server" >使用者名稱：</asp:Label>
        <asp:TextBox ID="txtUsername" Runat="server" Width="150" BorderColor="#888888" BorderWidth="1px"></asp:TextBox>
    </div>

    <div class="Div">
        <asp:Label ID="Label3" Runat="server" >密　　　碼：</asp:Label>
        <asp:TextBox ID="txtPassword" Runat="server" TextMode="Password" Width="150" BorderColor="#888888" BorderWidth="1px"></asp:TextBox>
    </div>

    <div class="Div">
        <asp:ImageButton ImageUrl="App_Themes/images/login.png" ID="btnLogin" OnClick="Login_Click" runat="server" Width="99"  Height="24" />
    </div>

    <div class="Div">
        <asp:RadioButtonList ID="rbCookieTime" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" >
        <asp:ListItem Value="H" Selected="True"> 1小時 </asp:ListItem>
        <asp:ListItem Value="D"> 1天 </asp:ListItem>
        <asp:ListItem Value="M"> 1月 </asp:ListItem>
        </asp:RadioButtonList>
    </div>

    <div class="Div">
        <asp:CheckBox ID="chkPersist" Runat="server" CssClass="chkPersist"/>
        <span>記住我的登錄狀態</span>
    </div>

    <div>
        <asp:Label ID="Label1" Runat="server" Visible="false" >Domain:</asp:Label>
        <asp:TextBox ID="txtDomain" Runat="server" Visible="false"></asp:TextBox>
    </div>

    <div>
        <asp:Label ID="errorLabel" Runat="server" ForeColor="#ff3300" CssClass="errorLabel"></asp:Label>
    </div>

    <div>
    <br />
    <uc1:CheckAuthorization ID="CheckAuthorization1" runat="server" />
    </div>
</div>

<div id="bottom_login" style=" text-align:center ; color: #FFF; letter-spacing:4px;">版權所有 &copy; 2011 泛太資訊科技開發股份有限公司</div>
    </form>
  </body>
</html>
