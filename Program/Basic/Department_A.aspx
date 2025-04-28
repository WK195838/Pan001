<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Department_A.aspx.cs" Inherits="Basic_Department_A" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc7" %>
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
    <title>部門資料新增</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="部門資料新增" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Insert" Width="100%" AutoGenerateRows="False" DataSourceID="SqlDataSource1" 
                    DataKeyNames="Company,DepCode" 
                    OnItemCreated="DetailsView1_ItemCreated" 
                    OnItemInserted="DetailsView1_ItemInserted" 
                    OnItemInserting="DetailsView1_ItemInserting">
                        <Fields> 
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" />
                            <asp:BoundField DataField="DepCode" HeaderText="DepCode" SortExpression="DepCode" />
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
                    <asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
            </td></tr>
        </table>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" 
InsertCommand="INSERT INTO Department([Company],[DepCode],[DepName],[DepNameE],[CostType],[DepType],[ChiefTitle],[ChiefID],[ParentDepCode]) 
VALUES (@Company, @DepCode, @DepName, @DepNameE, @CostType, @DepType, @ChiefTitle, @ChiefID, @ParentDepCode)" 
SelectCommand="SELECT Department.* FROM Department">
            <InsertParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="DepCode" />
                <asp:Parameter Name="DepName" />
                <asp:Parameter Name="DepNameE" />
                <asp:Parameter Name="CostType" />
                <asp:Parameter Name="DepType" />
                <asp:Parameter Name="ChiefTitle" />
                <asp:Parameter Name="ChiefID" />
                <asp:Parameter Name="ParentDepCode" />          
            </InsertParameters>
        </asp:SqlDataSource>        
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_DepCode" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
       </div>
    </form>
</body>
</html>

