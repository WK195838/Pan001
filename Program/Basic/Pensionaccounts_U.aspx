<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pensionaccounts_U.aspx.cs" Inherits="Basic_SalaryStructureParameter_U" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>退休金資料修改</title>
<base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
    <link href="~/App_Themes/ePayroll/ePayroll.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />
</head>
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

<uc2:ShowMsgBox ID="ShowMsgBox1" runat="server" />
<uc1:StyleHeader ID="StyleHeader1" runat="server" />
<uc3:StyleTitle ID="StyleTitle1" runat="server" Title="退休金資料修改" />
<uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
<br />
<table width="100%">
<tr>
<td align="center">
<asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
</tr>
<tr>
<td>
<!-- @      DetailsView    @ -->
<asp:DetailsView 
ID="DetailsView1"
DataSourceID="SqlDataSource1" 
DataKeyNames="Company,EmployeeId" 
runat="server" 
DefaultMode="Edit" 
Width="100%"
AutoGenerateRows="False" 
OnItemCreated="DetailsView1_ItemCreated" 
OnItemUpdated="DetailsView1_ItemUpdated" 
OnDataBound="DetailsView1_DataBound" 
OnItemUpdating="DetailsView1_ItemUpdating"
>
<Fields>                        
<asp:BoundField DataField="Company"                 HeaderText="Company"                SortExpression="Company"                ReadOnly="true" />
<asp:BoundField DataField="EmployeeId"              HeaderText="EmployeeId"             SortExpression="EmployeeId"             ReadOnly="true" />
<asp:BoundField DataField="pensionaccount"          HeaderText="pensionaccount"         SortExpression="pensionaccount" />
<asp:BoundField DataField="TrxType"                 HeaderText="TrxType"                SortExpression="TrxType" />
<asp:BoundField DataField="OldnewCode"              HeaderText="OldnewCode"             SortExpression="OldnewCode" />
<asp:BoundField DataField="NewSystem_Date"          HeaderText="NewSystem_Date"         SortExpression="NewSystem_Date" />
<asp:BoundField DataField="Emp_rate"                HeaderText="Emp_rate"               SortExpression="Emp_rate" /> 
<asp:BoundField DataField="CompanyRate"             HeaderText="CompanyRate"            SortExpression="CompanyRate" /> 
<asp:BoundField DataField="MonthlyActualSalary"     HeaderText="MonthlyActualSalary"    SortExpression="MonthlyActualSalary" /> 
<asp:BoundField DataField="EmpRate_changeyear"      HeaderText="EmpRate_changeyear"     SortExpression="EmpRate_changeyear" />   
<asp:BoundField DataField="CompanyRate_changeyear"  HeaderText="CompanyRate_changeyear" SortExpression="CompanyRate_changeyear" />
<asp:BoundField DataField="ActualTotalamount_S"     HeaderText="ActualTotalamount_S"    SortExpression="ActualTotalamount_S"    ReadOnly="true"    />   
<asp:BoundField DataField="ActualTotalamount_C"     HeaderText="ActualTotalamount_C"    SortExpression="ActualTotalamount_C"    ReadOnly="true"/> 
<asp:BoundField DataField="EffectiveDate"           HeaderText="EffectiveDate"          SortExpression="EffectiveDate" /> 
</Fields>
</asp:DetailsView>
</td>
</tr>
<tr><td>
<asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Update" OnClick="btnSave_Click" />
<asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
</td></tr>
</table>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
SelectCommand="SELECT A.*,IsNull(B.TrxType,'1') TrxType FROM Pensionaccounts_Master A Left Join Pensionaccounts_Transaction B 
On A.Company = B.Company and A.EmployeeId = B.EmployeeId 
WHERE ( A.Company = @Company and A.EmployeeId= @EmployeeId)"

UpdateCommand="
UPDATE Pensionaccounts_Master SET 
Company = @Company
,EmployeeId = @EmployeeId
,pensionaccount = @pensionaccount
,OldnewCode = @OldnewCode
,NewSystem_Date = @NewSystem_Date
,Emp_rate = @Emp_rate
,CompanyRate = @CompanyRate
,MonthlyActualSalary = @MonthlyActualSalary
,EmpRate_changeyear = @EmpRate_changeyear
,CompanyRate_changeyear = @CompanyRate_changeyear
,EffectiveDate = @EffectiveDate
 WHERE (Company = @Company and EmployeeId= @EmployeeId)
 UPDATE Pensionaccounts_Transaction SET 
 Company =@Company
,EmployeeId =@EmployeeId
,TrxDate =@EffectiveDate
,TrxType =@TrxType
,Emp_rate_change =@Emp_rate
,MonthlyActualSalary =@MonthlyActualSalary
,CompanyRate_change =@CompanyRate
WHERE (Company = @Company and EmployeeId= @EmployeeId)
 ">
<UpdateParameters>
<asp:Parameter Name="Company" />
<asp:Parameter Name="EmployeeId" />
<asp:Parameter Name="pensionaccount" />
<asp:Parameter Name="OldnewCode" />
<asp:Parameter Name="NewSystem_Date" />
<asp:Parameter Name="Emp_rate" />
<asp:Parameter Name="TrxType" />
<asp:Parameter Name="CompanyRate" />
<asp:Parameter Name="MonthlyActualSalary" />
<asp:Parameter Name="EmpRate_changeyear" />
<asp:Parameter Name="CompanyRate_changeyear" />
<asp:Parameter Name="EffectiveDate" />

</UpdateParameters>
<SelectParameters>
<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
<asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
</SelectParameters>
</asp:SqlDataSource>
<asp:HiddenField ID="hid_SalaryId" runat="server" />        
<%--   <asp:HiddenField ID="hid_EmployeeId" runat="server" />--%>
<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
<uc6:StyleFooter ID="StyleFooter1" runat="server" />

</div>
       </ContentTemplate>
       </asp:UpdatePanel>
</form>
</body>
</html>

