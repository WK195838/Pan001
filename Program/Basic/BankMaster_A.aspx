<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankMaster_A.aspx.cs" Inherits="Basic_BankMaster_A" %>

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
    <title>銀行資料新增</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<%--     
BankHeadOffice
BankBranch
BankName_C
BankName_E
BankAbbreviations
BankAddress_C
BankAddress_E
TEL
Fax
TelexNum
LocalBanks--%>          
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="銀行資料新增" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                <%--08.30 修改--%>
                    <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Insert" Width="100%" AutoGenerateRows="False" DataSourceID="SqlDataSource1" DataKeyNames="BankHeadOffice,BankBranch" OnItemCreated="DetailsView1_ItemCreated" OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting">
                        <Fields> 
                            <asp:BoundField DataField="BankHeadOffice" HeaderText="BankHeadOffice" SortExpression="BankHeadOffice" />
                            <asp:BoundField DataField="BankBranch" HeaderText="BankBranch" SortExpression="BankBranch" />
                            <asp:BoundField DataField="BankName_C" HeaderText="BankName_C" SortExpression="BankName_C" />
                            <asp:BoundField DataField="BankName_E" HeaderText="BankName_E" SortExpression="BankName_E" />
                            <asp:BoundField DataField="BankAbbreviations" HeaderText="BankAbbreviations" SortExpression="BankAbbreviations" />
                            <asp:BoundField DataField="BankAddress_C" HeaderText="BankAddress_C" SortExpression="BankAddress_C" />
                            <asp:BoundField DataField="BankAddress_E" HeaderText="BankAddress_E" SortExpression="BankAddress_E" />
                            <asp:BoundField DataField="TEL" HeaderText="TEL" SortExpression="TEL" />
                            <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax" />
                            <asp:BoundField DataField="TelexNum" HeaderText="TelexNum" SortExpression="TelexNum" />
                            <asp:BoundField DataField="LocalBanks" HeaderText="LocalBanks" SortExpression="LocalBanks" />
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
       <!--//       
,@Identify,@PayCode,@ResignCode,@HireDate,@LeaveWithoutPay,@ReHireDate,@ResignDate,@ObserveExpirationDate
,@LstPromotionDate,@LstChangeSalaryDate,@LWC,Union,@SpecialSeniority,@BloodType,@IDNum,@IDType,@Sex,@Nationality
,@BirthDate,@MaritalStatus,@DependentsNum,@Military,@Registry,@Addr,@ResidenceAddr,@TEL,@Contact,@Guarantor1,@Guarantor2
,@Introducer,@ContactTEL,@Guarantor1TEL,@Guarantor2TEL,@IntroducerTEL,@CCN,@EducationCode
       --> <%--08.30 修改--%>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
            InsertCommand="INSERT INTO Bank_Master([BankHeadOffice]
            ,[BankBranch]
            ,[BankName_C]
            ,[BankName_E]
            ,[BankAbbreviations]
            ,[BankAddress_C]
            ,[BankAddress_E]
            ,[TEL]
            ,[Fax]
            ,[TelexNum]
            ,[LocalBanks]) VALUES (@BankHeadOffice, @BankBranch, @BankName_C, @BankName_E, @BankAbbreviations, @BankAddress_C, @BankAddress_E, @TEL, @Fax, @TelexNum, @LocalBanks)"
            SelectCommand="SELECT Bank_Master.* FROM Bank_Master">
            <InsertParameters>
                <%--08.30 修改--%>
                <asp:Parameter Name="BankHeadOffice" />
                <asp:Parameter Name="BankBranch" />
                <asp:Parameter Name="BankName_C" />
                <asp:Parameter Name="BankName_E" />
                <asp:Parameter Name="BankAbbreviations" />
                <asp:Parameter Name="BankAddress_C" />
                <asp:Parameter Name="BankAddress_E" />
                <asp:Parameter Name="TEL" />
                <asp:Parameter Name="Fax" />
                <asp:Parameter Name="TelexNum" />
                <asp:Parameter Name="LocalBanks" />
                
            </InsertParameters>
        </asp:SqlDataSource>        
        <asp:HiddenField ID="hid_BankHeadOffice" runat="server" />
        <asp:HiddenField ID="hid_BankBranch" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
       </div>
    </form>
</body>
</html>
