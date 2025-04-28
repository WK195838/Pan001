<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Department_U.aspx.cs" Inherits="Basic_Department_U" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>部門資料修改</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       
        <uc2:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc1:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="部門資料修改" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Edit" Width="100%" DataSourceID="SqlDataSource1" DataKeyNames="Company,DepCode" AutoGenerateRows="False" 
                    OnItemCreated="DetailsView1_ItemCreated" 
                    OnItemUpdated="DetailsView1_ItemUpdated" 
                    OnDataBound="DetailsView1_DataBound" 
                    OnItemUpdating="DetailsView1_ItemUpdating">
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditRowStyle HorizontalAlign="Left" />
                        <Fields>                        
                            <asp:BoundField DataField="Company" HeaderText="Company" ReadOnly="true" SortExpression="Company" />
                            <asp:BoundField DataField="DepCode" HeaderText="DepCode" ReadOnly="true" SortExpression="DepCode" />
                            <asp:BoundField DataField="DepName" HeaderText="DepName" SortExpression="DepName" />
                            <asp:BoundField DataField="DepNameE" HeaderText="DepNameE" SortExpression="DepNameE" />
                            <asp:BoundField DataField="CostType" HeaderText="CostType" SortExpression="CostType" />
                            <asp:BoundField DataField="DepType" HeaderText="DepType" SortExpression="DepType" />
                            <asp:BoundField DataField="ChiefTitle" HeaderText="ChiefTitle" SortExpression="ChiefTitle" />
                            <asp:BoundField DataField="ChiefID" HeaderText="ChiefID" SortExpression="ChiefID" />
                            <asp:BoundField DataField="ParentDepCode" HeaderText="ParentDepCode" SortExpression="ParentDepCode" /> 
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
            SelectCommand="SELECT Department.* FROM Department WHERE (Company = @Company And DepCode=@DepCode)"
            UpdateCommand="UPDATE Department SET DepName = @DepName, DepNameE = @DepNameE, CostType = @CostType, DepType = @DepType, ChiefTitle = @ChiefTitle, ChiefID = @ChiefID,
             ParentDepCode = @ParentDepCode WHERE (Company = @Company And DepCode=@DepCode)">
            <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="DepCode" />
                <asp:Parameter Name="DepName" />
                <asp:Parameter Name="DepNameE" />
                <asp:Parameter Name="CostType" />
                <asp:Parameter Name="DepType" />
                <asp:Parameter Name="ChiefTitle" />
                <asp:Parameter Name="ChiefID" />
                <asp:Parameter Name="ParentDepCode" />  
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="DepCode" QueryStringField="DepCode" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hid_Company" runat="server" />        
        <asp:HiddenField ID="hid_DepCode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
    </div>
    </form>
</body>
</html>

