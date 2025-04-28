<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonnelAdjustment_U.aspx.cs" Inherits="Basic_PersonnelAdjustment_U" %>
<!-- UserControl -->
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- @      HTML    @ -->
<html xmlns="http://www.w3.org/1999/xhtml" >

<!-- @      Head    @ -->
<head id="Head1" runat="server">
    <title>人事調動資料修改</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
</head>

<!-- @      Body    @ -->
<body>
    <form id="form1" runat="server">
    <div>
        <uc2:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc1:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="人事調動資料修改" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" 
                    DefaultMode="Edit" Width="100%" DataSourceID="SqlDataSource1" 
                    DataKeyNames="Company,EmployeeId,AdjustmentCategory,EffectiveDate" 
                    AutoGenerateRows="False" OnItemCreated="DetailsView1_ItemCreated" 
                    OnItemUpdated="DetailsView1_ItemUpdated" OnDataBound="DetailsView1_DataBound" OnItemUpdating="DetailsView1_ItemUpdating">
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditRowStyle HorizontalAlign="Left" />
                        <Fields>                        
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" ReadOnly="true" />
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" ReadOnly="true" />
                            <asp:BoundField DataField="AdjustmentCategory" HeaderText="AdjustmentCategory" SortExpression="AdjustmentCategory" ReadOnly="true" />
                            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" ReadOnly="true" />
                            <asp:BoundField DataField="DepCode_F" HeaderText="DepCode_F" SortExpression="DepCode_F" ReadOnly="true"/>
                            <asp:BoundField DataField="DepCode_T" HeaderText="DepCode_T" SortExpression="DepCode_T" />
                            <asp:BoundField DataField="Title_F" HeaderText="Title_F" SortExpression="Title_F" ReadOnly="true"/>
                            <asp:BoundField DataField="Title_T" HeaderText="Title_T" SortExpression="Title_T" />
                            <asp:BoundField DataField="Level_F" HeaderText="Level_F" SortExpression="Level_F" ReadOnly="true"/>
                            <asp:BoundField DataField="Level_T" HeaderText="Level_T" SortExpression="Level_T" />
                            <asp:BoundField DataField="SalarySystem_F" HeaderText="SalarySystem_F" SortExpression="SalarySystem_F" ReadOnly="true"/>
                            <asp:BoundField DataField="SalarySystem_T" HeaderText="SalarySystem_T" SortExpression="SalarySystem_T" />
                            <asp:BoundField DataField="Class_F" HeaderText="Class_F" SortExpression="Class_F" ReadOnly="true"/>
                            <asp:BoundField DataField="Class_T" HeaderText="Class_T" SortExpression="Class_T" />
                            <asp:BoundField DataField="ResignReason" HeaderText="ResignReason" SortExpression="ResignReason" />
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            
            <tr>
                <td style=" text-align:center">
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Update" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
                </td>
            </tr>
        </table>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" 
        
        SelectCommand="SELECT PersonnelAdjustment.* FROM PersonnelAdjustment WHERE Company = @Company AND EmployeeId = @EmployeeId And AdjustmentCategory = @AdjustmentCategory And EffectiveDate = @EffectiveDate"
        UpdateCommand = "UPDATE PersonnelAdjustment SET 
        DepCode_T = @DepCode_T,
        Title_T = @Title_T,
        Level_T = @Level_T,
        SalarySystem_T = @SalarySystem_T,
        Class_T = @Class_T,
        ResignReason = @ResignReason
        Where (
        Company = @Company 
        And 
        EmployeeId = @EmployeeId 
        And 
        AdjustmentCategory = @AdjustmentCategory
        And 
        EffectiveDate = @EffectiveDate)"
        > 
             <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
                <asp:QueryStringParameter Name="AdjustmentCategory" QueryStringField="AdjustmentCategory" />
                <asp:QueryStringParameter Name="EffectiveDate" QueryStringField="EffectiveDate" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="AdjustmentCategory" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="DepCode_T" />
                <asp:Parameter Name="Title_T" />
                <asp:Parameter Name="Level_T" />
                <asp:Parameter Name="SalarySystem_T" />
                <asp:Parameter Name="Class_T" />
                <asp:Parameter Name="ResignReason" />
            </UpdateParameters>
        
        
        </asp:SqlDataSource>
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
    </div>
    </form>
</body>
</html>

