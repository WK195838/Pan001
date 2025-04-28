<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="MyPayrollToPDF.aspx.cs" Inherits="MyPayrollToPDF" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server"> 
     <div style="text-align:center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:StyleTitle id="StyleTitle1" title="個人獎金單下載" runat="server" /> 
            <uc3:StyleContentStart id="StyleContentStart1" runat="server" />
         <div  style="text-align:center; width:300pt" >
         <ul style=" list-style-type:none;padding:0 0 0 5px; text-align:left">
         <li>
         <uc9:SearchList id="SearchList1" runat="server" />
         </li>

         <li>
         <span>是否在職：</span>
         <asp:RadioButtonList ID="rbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow">
         <asp:ListItem Text="是" Value="Y" Selected="True"></asp:ListItem>
         <asp:ListItem Text="否" Value="N"></asp:ListItem>
         <asp:ListItem Text="全部" Value=""></asp:ListItem>
         </asp:RadioButtonList>
         </li>

         <li id="showPrint" runat="server" style="display:none;">
         <span>是否允許列印：</span>
         <asp:RadioButtonList ID="rbPrint" runat="server" RepeatColumns="10" RepeatLayout="Flow">
         <asp:ListItem Text="是" Value="Y"></asp:ListItem>
         <asp:ListItem Text="否" Value="N" Selected="True"></asp:ListItem>
         </asp:RadioButtonList>
         </li>

         <li>
         <span>登入帳號：</span>
         <asp:TextBox id="OldId" runat="server" Width="158px" MaxLength="14" ></asp:TextBox>
         </li>

         <li>
         <span>密　　碼：</span>
         <asp:TextBox id="OldPassword" runat="server" Width="157px" TextMode="Password" MaxLength="14"></asp:TextBox>
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
         </ul>
         </div>                                         
        <asp:HiddenField id="hid_IsInsertExit" runat="server"></asp:HiddenField>
        <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label>        
        <uc5:StyleContentEnd id="StyleContentEnd1" runat="server" /> 
        </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btntopdf" />
            </Triggers>
       </asp:UpdatePanel>
    </div>    
</asp:Content>
