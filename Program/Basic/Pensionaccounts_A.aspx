<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pensionaccounts_A.aspx.cs" Inherits="Pensionaccounts_A" EnableEventValidation="false" %>

<!-- @      UserControl     @ -->
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- @      HTML    @ -->
<html xmlns="http://www.w3.org/1999/xhtml" >

<!-- @      Head    @ -->
<head id="Head1" runat="server">
<title>新增退休金資料</title>
<base target="_self" />    
    <script type="text/javascript" src="../Pages/pagefunction.js"></script>
    <link href="~/App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
</head>

<!-- @      Body    @ -->
<body>

<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" >
    <Scripts> 
    <asp:ScriptReference Path="~/Scripts/jquery-1.4.4.min.js" />
    <asp:ScriptReference Path="~/Scripts/jquery-ui-1.8.7.custom.min.js" />
    <asp:ScriptReference Path="~/Scripts/ui.datepicker.js" />
    <asp:ScriptReference Path="~/Scripts/ui.datepicker.tw.js" />
    <asp:ScriptReference Path="~/Scripts/jQueryRotate.js" />
    <asp:ScriptReference Path="~/Pages/pagefunction.js" />
    <asp:ScriptReference Path="~/Pages/Busy.js" />      
    <asp:ScriptReference Path="~/Scripts/jquery.calculation.js" />
    <asp:ScriptReference Path="~/Scripts/jquery.textarea-expander.js" />
    <asp:ScriptReference Path="~/Scripts/jquery.thickbox.js" />
    <asp:ScriptReference Path="~/Pages/InvSysFunction.js" />
    </Scripts>
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UP1" runat="server">
    <ContentTemplate>
<div>

<uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
<uc2:StyleHeader ID="StyleHeader1" runat="server" />
<uc3:StyleTitle ID="StyleTitle1" runat="server" Title="新增退休金資料" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />



<table width="100%">
<tr><td align="center">
<asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label>
</td></tr>

<tr><td>
<!-- @      DetailsView    @ -->
<asp:DetailsView 
ID="DetailsView1"
DataSourceID="SqlDataSource1" 
DataKeyNames="Company,EmployeeId" 
runat="server" 
DefaultMode="Insert" 
Width="100%"
AutoGenerateRows="False" 
OnItemCreated="DetailsView1_ItemCreated" 
OnDataBinding="DetailsView1_DataBound"
OnItemInserted="DetailsView1_ItemInserted" 
OnItemInserting="DetailsView1_ItemInserting">
<Fields>                        
<asp:BoundField DataField="Company"                 HeaderText="Company"                 />
<asp:BoundField DataField="EmployeeId"              HeaderText="EmployeeId"              />
<asp:BoundField DataField="pensionaccount"          HeaderText="pensionaccount"          />
<asp:BoundField DataField="TrxType"                 HeaderText="TrxType"                 />
<asp:BoundField DataField="OldnewCode"              HeaderText="OldnewCode"              />
<asp:BoundField DataField="NewSystem_Date"          HeaderText="NewSystem_Date"          />
<asp:BoundField DataField="Emp_rate"                HeaderText="Emp_rate"                /> 
<asp:BoundField DataField="CompanyRate"             HeaderText="CompanyRate"             /> 
<asp:BoundField DataField="MonthlyActualSalary"     HeaderText="MonthlyActualSalary"     /> 
<asp:BoundField DataField="EmpRate_changeyear"      HeaderText="EmpRate_changeyear"      />   
<asp:BoundField DataField="CompanyRate_changeyear"  HeaderText="CompanyRate_changeyear"  />   
<asp:BoundField DataField="EffectiveDate"           HeaderText="EffectiveDate"           /> 

</Fields>

</asp:DetailsView>
</td></tr>

<tr><td style=" text-align:center">
<asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
<asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
<asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
</td></tr>
</table>

<asp:SqlDataSource
ID="SqlDataSource1" 
runat="server" 
ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
InsertCommand="INSERT INTO Pensionaccounts_Master(
Company
,EmployeeId
,pensionaccount 
,OldnewCode
,NewSystem_Date 
,Emp_rate 
,CompanyRate 
,MonthlyActualSalary 
,EmpRate_changeyear   
,CompanyRate_changeyear
,EffectiveDate
) VALUES (
@Company
,@EmployeeId
,@pensionaccount 
,@OldnewCode
,@NewSystem_Date 
,@Emp_rate 
,@CompanyRate 
,@MonthlyActualSalary 
,@EmpRate_changeyear   
,@CompanyRate_changeyear
,@EffectiveDate
)
INSERT INTO Pensionaccounts_Transaction(
Company
,EmployeeId
,TrxDate
,TrxType
,Emp_rate_change
,MonthlyActualSalary
,CompanyRate_change
) VALUES (
@Company
,@EmployeeId
,@EffectiveDate
,@TrxType
,@Emp_rate
,@MonthlyActualSalary
,@CompanyRate
)
"

SelectCommand="SELECT Pensionaccounts_Master.* FROM Pensionaccounts_Master">
<InsertParameters>
<asp:Parameter Name="Company" />
<asp:Parameter Name="EmployeeId" />
<asp:Parameter Name="pensionaccount" />
<asp:Parameter Name="OldnewCode" />
<asp:Parameter Name="NewSystem_Date" />
<asp:Parameter Name="Emp_rate" />
<asp:Parameter Name="CompanyRate" />
<asp:Parameter Name="MonthlyActualSalary" />
<asp:Parameter Name="EmpRate_changeyear" />
<asp:Parameter Name="CompanyRate_changeyear" />
<asp:Parameter Name="EffectiveDate" />
</InsertParameters>

</asp:SqlDataSource>        

<asp:HiddenField ID="hid_InserMode" runat="server" />
<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
<uc6:StyleFooter ID="StyleFooter1" runat="server" />

<uc8:SalaryYM ID="YM" runat="server"  Visible="false" />
</div>
       </ContentTemplate>
       </asp:UpdatePanel>
</form>
</body>
</html>

