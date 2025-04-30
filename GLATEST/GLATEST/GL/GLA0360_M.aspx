<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0360_M.aspx.cs" Inherits="GLA0360_M" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Src="~/UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>會計科目資料維護</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
<link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
    <style type="text/css" id="tdr">
     .rtd
     {
       text-align:right;
     }
     .ltd
     {
     text-align:left;
     
     }
     
          
     </style>     
    
</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server">
           </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
           <ContentTemplate>
       <div >        
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
      
     
         
      
     
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="會計科目資料維護" ShowBackToPre="false" />       
        <table width="100%">
                    <tr>
                        <td class="rtd"  >
                            <span style="color: #ff3366">
                            公司編號：</span></td>
                        <td class="ltd" >
                            <uc8:CompanyList ID="DrpCompany" runat="server" AutoPostBack="false" />
                        </td>
                        <td   class="rtd">
                            <span style="color: #ff3366">
                            年度：</td>
                        <td class="ltd"  >
                            <asp:TextBox ID="TxtAcctYear" runat="server" MaxLength="3"></asp:TextBox></td>
                        <td  class="rtd">
                            </td>
                        <td class="ltd" >
                            </td>
                    </tr>
                    <tr>
                        <td class="rtd"  >
                            期間：</td>
                        <td class="ltd" >
                            起</td>
                        <td   class="rtd">
                            &nbsp;</td>
                        <td class="ltd"  >
                            迄</td>
                        <td  class="rtd">
                            &nbsp;</td>
                        <td class="ltd" >
                            結帳與否</td>
                    </tr>
            <tr>
                <td   class="rtd">
                    01：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodBegin01" runat="server" CssClass="JQCalendar"></asp:TextBox>
                              
                            </td>
                <td   class="rtd">
                    01：</td>
                <td class="ltd" >
                    <asp:TextBox ID="TxtPeriodEnd01" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd01_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd">
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    02：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin2" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    02：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd02" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd02_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    03：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin3" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    03：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd03" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd03_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose3" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    04：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin4" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    04：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd04" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd04_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose4" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    05：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin5" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    05：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd05" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd05_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose5" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    06：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin6" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    06：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd06" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd06_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose6" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    07：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin7" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    07：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd07" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd07_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose7" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    08：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin8" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    08：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd08" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd08_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose8" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    09：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin9" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    09：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd09" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd09_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose9" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    10：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin10" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    10：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd10" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd10_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose10" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    11：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin11" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    11：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd11" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd11_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose11" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    12：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin12" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    12：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd12" runat="server" AutoPostBack="True"  CssClass="JQCalendar"
                        ontextchanged="TxtPeriodEnd12_TextChanged"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose12" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td   class="rtd" >
                    13：</td>
                <td  class="ltd" >
                    <asp:Label ID="LabPeriodBegin13" runat="server"></asp:Label>
                </td>
                <td   class="rtd">
                    13：</td>
                <td class="ltd"  >
                    <asp:TextBox ID="TxtPeriodEnd13" runat="server"></asp:TextBox>
                              
                            </td>
                <td   class="rtd" >
                    &nbsp;</td>
                <td class="ltd" >
                    <asp:Label ID="LabPeriodClose13" runat="server"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td colspan="6"  >
                    <asp:ImageButton ID="btnSaveGo" runat="server"
                        SkinID="SG1" OnClick="btnSaveGo_Click" />&nbsp;
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" OnClick="btnSaveExit_Click" />&nbsp;
                    <asp:ImageButton ID="btnCancel" runat="server"
                                SkinID="CC1" /></td>
            </tr>
            <tr align="left">
                <td colspan="6">
                    <asp:Label ID="LabMessage" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>             
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_DepositBank" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
       </div>
    </ContentTemplate>
       </asp:UpdatePanel>
      
    </form>
</body>
</html>

