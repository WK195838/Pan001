<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LaborInsurance_U.aspx.cs" Inherits="Basic_LaborInsurance_U" validaterequest="false" EnableEventValidation="false" %>

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
<%--Company	公司編號
EmployeeId	員工編號
TrxType	異動類別
EffectiveDate	生效日期
LI_amount	勞保投保金額
--%>
<head id="Head1" runat="server">
    <title>勞工保險資料修改</title>
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
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="勞工保險資料修改" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" DefaultMode="Edit" Width="100%" DataSourceID="SqlDataSource1" DataKeyNames="Company,EmployeeId" AutoGenerateRows="False" 
                    OnItemCreated="DetailsView1_ItemCreated" 
                    OnItemUpdated="DetailsView1_ItemUpdated" 
                    OnDataBound="DetailsView1_DataBound" 
                    OnItemUpdating="DetailsView1_ItemUpdating">
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditRowStyle HorizontalAlign="Left" />
                        <Fields>                        
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" ReadOnly="true" />
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" ReadOnly="true" />
                            <asp:BoundField DataField="TrxType" HeaderText="TrxType" SortExpression="TrxType" />
                            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" SortExpression="EffectiveDate" />
                            <asp:BoundField DataField="LI_amount" HeaderText="LI_amount" SortExpression="LI_amount" />
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
            SelectCommand="SELECT LaborInsurance.* FROM LaborInsurance WHERE (Company = @Company And EmployeeId=@EmployeeId)"
            UpdateCommand="UPDATE LaborInsurance SET TrxType = @TrxType, EffectiveDate = Convert(smalldatetime,@EffectiveDate), LI_amount = @LI_amount WHERE (Company = @Company And EmployeeId=@EmployeeId)">
            <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="TrxType" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="LI_amount" />
              
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hid_Companyd" runat="server" />        
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
    </div>
       </ContentTemplate>
       </asp:UpdatePanel>
    </form>
</body>
</html>
