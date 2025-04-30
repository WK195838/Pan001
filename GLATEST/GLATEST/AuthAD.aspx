<%@ Page language="c#" AutoEventWireup="true" CodeFile="AuthAD.aspx.cs" Inherits="AuthAD" %>
<%@ Register src="UserControl/CheckAuthorization.ascx" tagname="CheckAuthorization" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/iBOSSiteStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/iBosGridStyle.css" rel="stylesheet" type="text/css" />
    <link href="http://localhost:59077/GLAWeb/Styles/StyleBtn.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        //取消上一頁功能
        window.history.forward(1);

        var _oldcolor;
        function setnewcolor(source) {
            _oldcolor = source.style.backgroundColor;
            source.style.backgroundColor = '#cccccc';
        }
        function setoldcolor(source) {
            source.style.backgroundColor = _oldcolor;
        }
        function maxsize() {
            self.moveTo(0, 0);
            self.resizeTo(screen.availWidth, screen.availHeight);
        }
        function doWait(btn) {
            var result = true;
            btn.disabled = true;
            __doPostBack(btn.id, '');
            return result;
        }
    </script>
</head>
<body>
    <div class="page">
        <form id="form1" runat="server"  defaultbutton="btnLogin" defaultfocus="txtUserId">
        <div class="header">
            <div class="title">
                
            </div>
        </div>
        <div class="topframe">
            <div class="detaillefttop">
            </div>
            <div class="detailtop">
            </div>
            <div class="detailrighttop">
            </div>
        </div>
        <div class="middleframe">
            <div class="detailleft">
            </div>
            <div class="detailcontent">
                <div style="display: none">
                    <asp:Label ID="Label2" runat="server">AD Domain: </asp:Label>
                    <asp:TextBox ID="txtDomain" runat="server" />
                </div>
                <div class="Div" style="display: none">
                    <asp:RadioButtonList ID="rbCookieTime" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow">
                        <asp:ListItem Value="H" Selected="True"> 1 Hour </asp:ListItem>
                        <asp:ListItem Value="D"> 1 Day </asp:ListItem>
                        <asp:ListItem Value="M"> 1 Month </asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="Div" style="display: none">
                    <asp:CheckBox ID="chkPersist" runat="server" CssClass="chkPersist" Checked="True" />
                    <span>Keep my sign in session</span>
                </div>
                <div class="logindialog">
                    <div class="grid">
                        <div class="rounded">
                            <div class="top-outer">
                                <div class="top-inner">
                                    <div class="top">
                                        <h2><asp:Label ID="Label3" runat="server" Text="使用者登錄" /></h2>
                                    </div>
                                </div>
                            </div>
                            <div class="mid-outer">
                                <div class="mid-inner">
                                    <div class="mid" style="height: 180px">
                                        <div class="row1">
                                            <span>使用者帳號</span>
                                            <asp:TextBox ID="txtUsername" runat="server" Width="150px" AutoCompleteType="Disabled" />
                                        </div>
                                        <div class="row1">
                                            <span>使用者密碼</span>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" />
                                        </div>
                                        <div class="logincenter">
                                            <%--<asp:imagebutton ID="btLogin" runat="server" ImageUrl="images/btnimg/btnLogin95_O.gif"  OnClick="Login_Click" /><br />--%>
                                            <asp:Button ID="btnLogin" runat="server" Text="" SkinID="btnLogin" OnClick="Login_Click"/><br/>
                                            <asp:Label ID="errorLabel" runat="server" ForeColor="Red" Width="250px" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="bottom-outer">
                                <div class="bottom-inner">
                                    <div class="bottom">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="detailright">
            </div>
            <div style="position:relative;left:20%;bottom:25%;">
                <asp:Label ID="lblMaintain" runat="server" ForeColor="Red" /> <!--- 顯示系統維護作業訊息 -->
                <div style="display:none">
                <uc1:CheckAuthorization ID="CheckAuthorization1" runat="server" />
                </div>
            </div>
        </div>
        <div class="bottomframe">
            <div class="detailleftbottom">
            </div>
            <div class="detailbottom">
            </div>
            <div class="detailrightbottom">
            </div>
            <div class="footer">
                <%--PanPacific Copyright © 2012--%>
                泛太資訊科技開發股份有限公司 版權所有 © 2012
            </div>
        </div>
        </form>
    </div>
</body>
</html>
