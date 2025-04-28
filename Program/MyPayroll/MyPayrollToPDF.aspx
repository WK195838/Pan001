<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyPayrollToPDF.aspx.cs" Inherits="MyPayrollToPDF" %>

<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc4" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>個人獎金單下載</title>
<link href="../App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">    
     <div style="text-align:center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
            <uc2:StyleTitle id="StyleTitle1" title="個人獎金單下載" runat="server" /> 
            <uc3:StyleContentStart id="StyleContentStart1" runat="server" />
         <div  style="text-align:center; width:300pt" >
         <ul style=" list-style-type:none;padding:0 0 0 5px; text-align:left">
         <li>
         <uc9:SearchList id="SearchList1" runat="server" />
         </li>

         <li>
         <span>登入帳號：</span>
         <asp:TextBox id="OldId" runat="server" Width="158px" MaxLength="14" ></asp:TextBox>
         </li>

         <li>
         <span>密　　碼：</span>
         <asp:TextBox id="OldPassword" runat="server" Width="157px" TextMode="Password" MaxLength="14" onKeyPress="checkCapsLock( event, this)"></asp:TextBox>
         </li>
         
         <li id="erpadmS1" runat="server" style="display:none; float:left;list-style-type:circle">
             <asp:RadioButtonList ID="rbPDFPwd" runat="server" RepeatDirection="Horizontal">
             <asp:ListItem Value="d" Selected="True">個人薪資密碼</asp:ListItem>
             <asp:ListItem Value="n">不設密碼</asp:ListItem>
             <asp:ListItem Value="s">統一密碼</asp:ListItem>             
             </asp:RadioButtonList>設定批次密碼：<asp:TextBox id="tbPDFPwd" runat="server" Width="157px" TextMode="Password" MaxLength="14"/>
             <asp:CheckBox ID="cbEmail" runat="server" Text="直接寄送" />
         </li>

         <li style=" float:none">
         PDF密碼變更：<asp:TextBox id="NewPassword" runat="server" Width="157px" TextMode="Password" MaxLength="14"></asp:TextBox>
         </li>
         
         <li>
         PDF密碼再確認：<asp:TextBox id="NewPassWord2" runat="server" Width="157px" TextMode="Password" MaxLength="14"></asp:TextBox>
         </li>
         
         <li>
         發放日期：<asp:TextBox runat="server" ID="tbAmtDateS" MaxLength="10" Width="70px" />
         ～<asp:TextBox runat="server" ID="tbAmtDateE" MaxLength="10" Width="70px" />
         </li>

         <li>
         <span>獎　　金：</span>
         <uc8:CodeList ID="CL_Bonus" runat="server" />
         </li>

         <li>
         <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
         <asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">查無資料!!</asp:Panel>
         </li>

         <li id="erpadmS2" runat="server" >
         <asp:Button id="btntopdf" onclick="btntopdf_Click" runat="server" Text="下載檔案"></asp:Button>
         &nbsp;&nbsp; &nbsp; &nbsp;
         <asp:Button id="ChangPassWord" onclick="ChangPassWord_Click" runat="server" Text="修改密碼" ></asp:Button>
         </li>

         <li>
         </li>

         <li>
         使用說明：
         </li>

         <li>
         1、第一次使用時，請先修改PDF密碼；預設值請洽人事管理處。
         </li>

         <li>
         2、下載密碼同PDF解密密碼，且密碼區分大小寫！
         </li>

         <li>
         3、登入帳號為您的員工代號，可利用下拉單挑選或直接輸入。
         </li>

         <li>
         4、密碼修改後若忘記密碼，請洽人事管理處進行重設！
         </li>
         
         </ul>
         </div>                                         
        <asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>
        <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label>        

        <!-- #include file="../SYS/include/HI2.inc" -->

        <uc5:StyleContentEnd id="StyleContentEnd1" runat="server" /> 
        </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btntopdf" />
            </Triggers>
       </asp:UpdatePanel>
    </div>    
    </form>
</body>
</html>

