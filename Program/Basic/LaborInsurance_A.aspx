<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LaborInsurance_A.aspx.cs" Inherits="Basic_LaborInsurance_A" validaterequest="false" EnableEventValidation="false" %>


<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>勞工保險資料新增</title>
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
    
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="勞工保險資料新增" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Insert" Width="100%" AutoGenerateRows="False" DataSourceID="SqlDataSource1" DataKeyNames="SalaryId" OnItemCreated="DetailsView1_ItemCreated" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting">
                        <Fields> 
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" />
                            <asp:BoundField DataField="TrxType" HeaderText="TrxType" SortExpression="TrxType" />
                            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" />
                            <asp:BoundField DataField="LI_amount" HeaderText="LI_amount" SortExpression="LI_amount" />
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            <tr><td>
                    <asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
            </td></tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
            InsertCommand="INSERT INTO LaborInsurance([Company],[EmployeeId],[TrxType],[EffectiveDate],[LI_amount]) VALUES (@Company, @EmployeeId, @TrxType, @EffectiveDate, @LI_amount)"
            SelectCommand="SELECT LaborInsurance.* FROM LaborInsurance">
            <InsertParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="TrxType" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="LI_amount" />
            </InsertParameters>
        </asp:SqlDataSource>        
        <asp:HiddenField ID="hid_Company" runat="server" />
         <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
       </div>
       </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</body>
</html>