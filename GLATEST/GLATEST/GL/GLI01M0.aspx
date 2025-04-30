<%@ Page Language="C#"  MasterPageFile="~/GLA.master" AutoEventWireup="true" CodeFile="GLI01M0.aspx.cs" Inherits="GLI01M0" %>

<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="CompanyList" %>
<%@ Register Src="~/UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="CodeList" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc2" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc1:StyleTitle ID="StyleTitle1" runat="server" ShowBackToPre="false" ShowHome="false"
            ShowUser="false" Title="科目月份餘額查詢" />
       <fieldset>
        <legend>&nbsp;科目月份餘額查詢&nbsp;</legend>
        <table width="99%" border="1" cellspacing="1">
        <tr>
        <td style="width: 72px">
            公司別：</td>
        <td style="width: 261px">
        <CompanyList:CompanyList ID="DrpCompanyList" runat="server" AutoPostBack="True" />
        </td>
        <td style="width: 105px" colspan="2">
        </td>
        </tr> 
        <tr>
        <td style="width: 72px">
            會計科目：</td>
        <td style="width: 261px">
        <CodeList:CodeList ID="ddlAcc" runat="server"  />
        </td>
        <td style="width: 105px" colspan="2">
        </td>
        </tr> 
        <tr>
        <td style="width: 72px">
            年度：</td>
        <td style="width: 261px">
            <asp:TextBox ID="txtAcctYear" runat="server" Width="100px" MaxLength="4"></asp:TextBox></td>
        <td style="width: 105px">
            追溯分攤資料：</td>
        <td>
            <asp:DropDownList ID="ddlAlocFlag" runat="server">
                <asp:ListItem Value=" ">不含</asp:ListItem>
                <asp:ListItem Value="Y">含</asp:ListItem>
            </asp:DropDownList></td>
        </tr> 
        <tr>
        <td style="width: 72px" colspan="4"></td>
            </tr> 
        <tr>
        <td style="width: 72px">
            <asp:Button ID="btnQuery" runat="server" Text="查詢餘額" Width="72px" OnClick="btnQuery_Click" /></td>
        <td style="width: 204px" colspan="3">
            &nbsp;<asp:Label ID="Label2" runat="server"></asp:Label></td>
        </tr> 
        </table>

        <asp:SqlDataSource ID="sdCompany" runat="server"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdAccDef" runat="server"></asp:SqlDataSource>
        </fieldset>
        <fieldset>
        <legend> 查詢結果</legend>
           <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>                
        <table id="showQuery" width="99%" border="1" cellspacing="0">
        <tr style="background-color: #dcdcdc">
        <td  align="center">
            上期餘額</td>
        <td align="center" >
            借方</td>
        <td align="center" >
            貸方</td>
        <td align="right">
            <asp:Label ID="Label1" runat="server" Text="$"></asp:Label>
            <asp:Label ID="lbl_LastAmt" runat="server">0.00</asp:Label>
        </td> 
        </tr> 
       <tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            1</td>
        <td align="right">
            <asp:Label ID="lbl_M01db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M01cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M01" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            2</td>
        <td align="right">
            <asp:Label ID="lbl_M02db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M02cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M02" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            3</td>
        <td align="right">
            <asp:Label ID="lbl_M03db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M03cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M03" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            4</td>
        <td align="right">
            <asp:Label ID="lbl_M04db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M04cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M04" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            5</td>
        <td align="right">
            <asp:Label ID="lbl_M05db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M05cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M05" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            6</td>
        <td align="right">
            <asp:Label ID="lbl_M06db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M06cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M06" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            7</td>
        <td align="right">
            <asp:Label ID="lbl_M07db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M07cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M07" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            8</td>
        <td align="right">
            <asp:Label ID="lbl_M08db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M08cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M08" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            9</td>
        <td align="right">
            <asp:Label ID="lbl_M09db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M09cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M09" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            10</td>
        <td align="right">
            <asp:Label ID="lbl_M10db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M10cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M10" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            11</td>
        <td align="right">
            <asp:Label ID="lbl_M11db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M11cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M11" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            12</td>
        <td align="right">
            <asp:Label ID="lbl_M12db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M12cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M12" runat="server">0.00</asp:Label></td> 
        </tr> 
<tr onmouseover="this.style.backgroundColor='silver'" onmouseout="this.style.backgroundColor=''">
        <td style="width: 72px; font-weight: bold; height: 18px;" align="center">
            13</td>
        <td align="right">
            <asp:Label ID="lbl_M13db" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M13cr" runat="server">0.00</asp:Label></td>
        <td align="right">
            <asp:Label ID="lbl_M13" runat="server">0.00</asp:Label></td> 
        </tr> 

        </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
    </asp:Content>
 
