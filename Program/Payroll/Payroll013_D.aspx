<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll013_D.aspx.cs" Inherits="Payroll013_D" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc9" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc10" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>薪資作業查詢</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
<script language="javascript">
///////////////////////////////////////////////////////////////////////////////////////////
</script>	    
</head>
<body>
    <form id="form1" runat="server">
<div>
    <uc9:StyleHeader ID="StyleHeader1" runat="server" />
    <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="薪資試算作業查詢" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
        <tr align="left"><td>公　　司：</td><td><uc2:CompanyList ID="CompanyList1" runat="server" /></td></tr>
        <tr align="left"><td>計薪年月：</td><td><uc8:SalaryYM id="SalaryYM1" runat="server" /></td></tr>
        <tr align="left"><td>期　　別：</td><td><uc6:PeriodList id="PeriodList1" runat="server" />日</td></tr>
        <tr align="left"><td>員　　工：</td><td><asp:Label ID="lblEmpID" runat="server" /></td></tr>        
        <tr align="left"><td colspan="2"><hr /></td></tr>
        <tr align="left"><td colspan="2">
           <table width="100%" cellspacing="0" cellpadding="5px" border="0" style="width:100%;border-collapse:collapse;">
            <tr>            
            <td class="Grid_GridLine" style="width:15%;"><asp:Label ID="lblTitle_01" runat="server" Text="基本薪俸" /></td><td class="Grid_GridLineDetailBlack" style="width:15%;text-align:right;"><asp:Label ID="lbl_01" runat="server" /></td>
            <td class="Grid_GridLine" style="width:15%;"><asp:Label ID="lblTitle_03" runat="server" Text="職務加給" /></td><td class="Grid_GridLineDetailBlack" style="width:15%;text-align:right;"><asp:Label ID="lbl_20" runat="server" /></td>
            <td class="Grid_GridLine" style="width:15%;"><asp:Label ID="lblTitle_05" runat="server" Text="代扣所得稅" /></td><td class="Grid_GridLineDetailRed" style="width:15%;text-align:right;"><asp:Label ID="lbl_03" runat="server" /></td>
            </tr>         
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_02" runat="server" Text="伙食津貼" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_02" runat="server" /></td>                               
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_12" runat="server" Text="其它應稅調整" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_22" runat="server" /></td>            
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_07" runat="server" Text="勞保費" /></td><td class="Grid_GridLineDetailRed" style="text-align:right;"><asp:Label ID="lbl_05" runat="server" /></td>
            </tr>         
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_WT_Overtime_Fee" runat="server" Text="應稅加班費" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_08" runat="server" /></td>                        
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_13" runat="server" Text="其它免稅調整" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_23" runat="server" /></td>            
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_06" runat="server" Text="健保費" /></td><td class="Grid_GridLineDetailRed" style="text-align:right;"><asp:Label ID="lbl_04" runat="server" /></td>            
            </tr>         
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_NT_Overtime_Fee" runat="server" Text="免稅加班費" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_09" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_04" runat="server" Text="其他扣款" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_21" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_08" runat="server" Text="福利金" /></td><td class="Grid_GridLineDetailRed" style="text-align:right;"><asp:Label ID="lbl_07" runat="server" /></td>
            </tr>
            <tr><td colspan="6"><hr /></td></tr>
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_RegularPay" runat="server" Text="固定薪資" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_RegularPay" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_WT_Overtime" runat="server" Text="應稅加班時數" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_WT_Overtime" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_LeaveHours_deduction" runat="server" Text="請假時數" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_LeaveHours_deduction" runat="server" /></td>
            </tr>
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_22" runat="server" Text="非固定金額" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_NonFixedSalsry" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_NT_Overtime" runat="server" Text="免稅加班時數" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_NT_Overtime" runat="server" /></td>            
            </tr>
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_NT_P" runat="server" Text="免稅所得" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_NT_P" runat="server" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_OnWatch" runat="server" Text="值班時數" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lbl_OnWatch" runat="server" /></td>            
            </tr>
            <tr>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_23" runat="server" Text="應扣金額" /></td><td class="Grid_GridLineDetailRed" style="text-align:right;"><asp:Label ID="lblMAmount" runat="server" /></td>                        
            </tr>
            <tr>            
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_24" runat="server" Text="應發金額" /></td><td class="Grid_GridLineDetailBlack" style="text-align:right;"><asp:Label ID="lblSAmount" runat="server" /></td>            
            </tr>            
            </table>
        </td></tr>
        <tr align="left"><td colspan="2"><hr /></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>        
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!</asp:Panel></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2">
        <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="false" AllowPaging="False"
         OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
          DataKeyNames="Company,EmployeeId" AutoGenerateColumns="False">
         <Columns>
             <asp:BoundField DataField="SalaryItem" HeaderText="薪資項目" SortExpression="SalaryItem" />
             <asp:BoundField DataField="SalaryName" HeaderText="項目名稱" SortExpression="SalaryName" />
             <asp:BoundField DataField="PMType" HeaderText="加減項" SortExpression="PMType" />
             <asp:BoundField DataField="TotalAmount" HeaderText="金額" SortExpression="TotalAmount" />
         </Columns>
     </asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>
    <uc10:StyleFooter ID="StyleFooter" runat="server" />     
    </div>
    </form>
</body>
</html>