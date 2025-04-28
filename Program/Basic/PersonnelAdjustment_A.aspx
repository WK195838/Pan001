<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonnelAdjustment_A.aspx.cs" Inherits="Basic_PersonnelAdjustment_A" validaterequest="false" EnableEventValidation="false" %>

<!-- @      UserControl     @ -->
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- @      HTML    @ -->
<html xmlns="http://www.w3.org/1999/xhtml" >

<!-- @      Head    @ -->
<head id="Head1" runat="server">
    <title>新增人事調動資料</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="新增人事調動資料" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        
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
                    DataKeyNames="Company,EmployeeId,AdjustmentCategory,EffectiveDate" 
                    runat="server" 
                    DefaultMode="Insert" 
                    Width="100%"
                    AutoGenerateRows="False" 
                    OnItemCreated="DetailsView1_ItemCreated" 
                    OnItemInserted="DetailsView1_ItemInserted" 
                    OnItemInserting="DetailsView1_ItemInserting">
                        <Fields>                        
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" />
                            <asp:BoundField DataField="AdjustmentCategory" HeaderText="AdjustmentCategory" SortExpression="AdjustmentCategory" />
                            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" />
                            <asp:BoundField DataField="DepCode_F" HeaderText="DepCode_F" SortExpression="DepCode_F" />
                            <asp:BoundField DataField="DepCode_T" HeaderText="DepCode_T" SortExpression="DepCode_T" />
                            <asp:BoundField DataField="Title_F" HeaderText="Title_F" SortExpression="Title_F" />
                            <asp:BoundField DataField="Title_T" HeaderText="Title_T" SortExpression="Title_T" />
                            <asp:BoundField DataField="Level_F" HeaderText="Level_F" SortExpression="Level_F" />
                            <asp:BoundField DataField="Level_T" HeaderText="Level_T" SortExpression="Level_T" />
                            <asp:BoundField DataField="SalarySystem_F" HeaderText="SalarySystem_F" SortExpression="SalarySystem_F" />
                            <asp:BoundField DataField="SalarySystem_T" HeaderText="SalarySystem_T" SortExpression="SalarySystem_T" />
                            <asp:BoundField DataField="Class_F" HeaderText="Class_F" SortExpression="Class_F" />
                            <asp:BoundField DataField="Class_T" HeaderText="Class_T" SortExpression="Class_T" />
                            <asp:BoundField DataField="ResignReason" HeaderText="ResignReason" SortExpression="ResignReason" />
                            <asp:BoundField DataField="MasterUpdate" HeaderText="MasterUpdate" SortExpression="MasterUpdate" ReadOnly="true" />
                        </Fields>
                    </asp:DetailsView>
                    
                </td>
            </tr>
            <tr><td style=" text-align:center">
                    <asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
            </td></tr>
        </table>


        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
            InsertCommand="INSERT INTO PersonnelAdjustment(
            Company
           ,EmployeeId
           ,AdjustmentCategory
           ,EffectiveDate
           ,DepCode_F
           ,DepCode_T
           ,Title_F
           ,Title_T
           ,Level_F
           ,Level_T
           ,SalarySystem_F
           ,SalarySystem_T
           ,Class_F
           ,Class_T
           ,ResignReason
           ,MasterUpdate
           ) VALUES (
           @Company 
           ,@EmployeeId 
           ,@AdjustmentCategory 
           ,@EffectiveDate 
           ,@DepCode_F 
           ,@DepCode_T 
           ,@Title_F 
           ,@Title_T
           ,@Level_F 
           ,@Level_T
           ,@SalarySystem_F 
           ,@SalarySystem_T 
           ,@Class_F
           ,@Class_T
           ,@ResignReason 
           ,@MasterUpdate
           )"
           
            SelectCommand="SELECT PersonnelAdjustment.* FROM PersonnelAdjustment">
            <InsertParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="AdjustmentCategory" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="DepCode_F" />
                <asp:Parameter Name="DepCode_T" />
                <asp:Parameter Name="Title_F" />
                <asp:Parameter Name="Title_T" />
                <asp:Parameter Name="Level_F" />
                <asp:Parameter Name="Level_T" />
                <asp:Parameter Name="SalarySystem_F" />
                <asp:Parameter Name="SalarySystem_T" />
                <asp:Parameter Name="Class_F" />
                <asp:Parameter Name="Class_T" />
                <asp:Parameter Name="ResignReason" />
                <asp:Parameter Name="MasterUpdate" />  
            </InsertParameters>
            
        </asp:SqlDataSource>        

        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
       </div>
       </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</body>
</html>

