<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CostCenter_M.aspx.cs" Inherits="CostCenter_M" validaterequest="false" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"   Namespace="System.Web.UI" TagPrefix="asp" %>    
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>

<%@ Register src="../UserControl/CompanyList.ascx" tagname="CompanyList" tagprefix="uc9" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>成本中心分攤設定維護程式</title>
    <base target="_self" />    
       <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
       <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
        .style1
        {
            height: 142px;
        }
        .style2
        {
            width: 1498px;
            height: 17px;
        }
        .style4
        {
            width: 102px;
        }
        .style5
        {
            width: 219px;
        }
        .ui-accordion
        {
            width: 394px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="成本中心分攤設定維護程式" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table width="100%">
                <tr><td align="center" class="style2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td></tr>
                    <tr>
                        <td class="style1">                            
                            <table class="ui-accordion">
                                <tr>
                                    <td class="style5" align="right">
                                        <asp:Label ID="Label2" runat="server" Text="公司別："></asp:Label>
                                    </td><td><uc9:CompanyList align="left" ID="CompanyList1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right"><asp:Label ID="Label3" runat="server" Text="部門別："></asp:Label>
                                    </td> <td><uc8:CodeList align="left" ID="DeptCode" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td class="style5" align="right"> <asp:Label ID="Label4" runat="server" Text="員工："></asp:Label></td>
                                    <td><uc8:CodeList align="left" ID="EmpleeoyCode" runat="server" />
                                    </td>
                                </tr>
                                <tr><td class="style5" align="right"><asp:Label ID="Balance_2" runat="server" ForeColor="Red" Text="總分攤比例："></asp:Label></td>                                    
                                    <td>
                                        <asp:Label ID="Balance_3" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr><tr>
                   <td><asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" 
                        CommandName="Insert" OnClick="btnSave_Click" />
                            <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" 
                        CommandName="Insert" OnClick="btnSave_Click" />
                            <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
            SelectCommand="SELECT Company, DeptId, DeptName, EmployeeId, EmployeeName, ApportionId, ApportionName, Balance, B_effective, (SELECT SUM(CAST(Balance AS int)) AS Balance_2 FROM CostCenter WHERE (Company = @Company) AND (EmployeeId = @EmployeeId)) AS Balance_2 FROM CostCenter AS CostCenter_1"
            InsertCommand="INSERT INTO CostCenter(Company, DeptId, DeptName, EmployeeId, EmployeeName, ApportionId, ApportionName, Balance, B_effective) VALUES (@Company, @DeptId, @DeptName, @EmployeeId, @EmployeeName, @ApporTionId, @ApportionName, @Balance_2, CAST(@B_effective AS smallDatetime))" 
            
            UpdateCommand="UPDATE CostCenter SET Balance = @Balance, ApportionId = @ApportionId, ApportionName = @ApportionName">
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
            </SelectParameters>
           <UpdateParameters>
               <asp:QueryStringParameter Name="Balance" QueryStringField="Balance_2" />
               <asp:QueryStringParameter Name="ApportionId" QueryStringField="ApportionId" />
               <asp:QueryStringParameter Name="ApportionName" 
                   QueryStringField="ApportionName" />
            </UpdateParameters>
            <InsertParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Commapny" />
                <asp:QueryStringParameter DefaultValue="" Name="DeptId" QueryStringField="DeptId" />
                <asp:QueryStringParameter DefaultValue="" Name="DeptName" QueryStringField="DeptName" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
                <asp:QueryStringParameter Name="EmployeeName" QueryStringField="EmployeeName" />
                <asp:QueryStringParameter Name="ApporTionId" QueryStringField="ApporTionId" />
                <asp:QueryStringParameter Name="ApportionName" QueryStringField="ApporTionName" />
                <asp:QueryStringParameter Name="B_effective" QueryStringField="B_effective" />
                <asp:QueryStringParameter Name="Balance_2" QueryStringField="Balance_2" />
            </InsertParameters>
        </asp:SqlDataSource>       
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_DepositBank" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
       <ContentTemplate>
       <uc2:StyleHeader ID="StyleHeader2" runat="server" />
        <uc3:StyleTitle id="StyleTitle2" title="成本中心分攤設定基本資料列表" runat="server"></uc3:StyleTitle>
        <uc4:StyleContentStart id="StyleContentStart2" runat="server"></uc4:StyleContentStart> 
        <table id="table1" cellspacing="0" cellpadding="0" width="100%" style="">
        <tbody>
           <tr><td>
               <asp:Label ID="lbl_Msg3" runat="server" Font-Bold="True" ForeColor="Red" 
                   Text="lbl_Msg3"></asp:Label>
               <br />
               <br />
               <asp:Label id="lbl_Msg2" runat="server" ForeColor="red"></asp:Label></td></tr>
            <tr><td><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </td></tr>
            <tr><td class="style1">
                <asp:GridView id="GridView1" runat="server" Width="99%" AllowSorting="True" 
                    AllowPaging="True" AutoGenerateColumns="False" 
            DataKeyNames="Company,DeptId,EmployeeId,ApportionId,B_effective,Balance" GridLines="None" 
            OnRowCreated="GridView1_RowCreated" 
            OnRowEditing="GridView1_RowEditing" 
            OnRowCommand="GridView1_RowCommand"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"                     
            OnRowDataBound="GridView1_RowDataBound" 
            DataSourceID="SDS_GridView" ShowFooter="True" Height="56px">
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
        <asp:TemplateField HeaderText="刪除" ShowHeader="False">
        <ItemTemplate>
        <asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");'  
             L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' 
             L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>'
             L3PK='<%# DataBinder.Eval(Container, "DataItem.DeptId")%>' 
             L4PK='<%# DataBinder.Eval(Container, "DataItem.B_effective")%>'
             L5PK='<%# DataBinder.Eval(Container, "DataItem.ApportionId")%>' 
             L6PK='<%# DataBinder.Eval(Container, "DataItem.Balance")%>' 
             CausesValidation="False"></asp:LinkButton> 
        </ItemTemplate>

        <HeaderStyle Width="30px"></HeaderStyle>
        </asp:TemplateField>
        <asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯" >
        <HeaderStyle Width="60px"></HeaderStyle>
        <ItemStyle Width="60px"></ItemStyle>
        </asp:CommandField>
        <asp:BoundField DataField="DeptId" HeaderText="部門" ReadOnly="True" SortExpression="DeptId" ></asp:BoundField>
        <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" ></asp:BoundField>
            <asp:BoundField DataField="ApportionId" HeaderText="分攤至部門" SortExpression="ApportionId" />
            <asp:BoundField DataField="Balance" HeaderText="分攤比例" SortExpression="Balance" />
            <asp:BoundField DataField="B_effective" HeaderText="生效日" 
                SortExpression="B_effective" />
        </Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>
<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell">
<td class="paginationRowEdgeLl"></td>
<%--<td>部門</td>
<td>員工</td>--%>
<td>分攤至部門</td>
<td>分攤比例</td>
<td class="paginationRowEdgeRI">生效日</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" OnClick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<%--<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew01" runat="server" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td>--%>
<td class="Grid_GridLine"><uc8:CodeList runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" /></td>
</tr>
</table>
</EmptyDataTemplate>
<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </td></tr><tr><td align="left" style="height: 9px"></td></tr></tbody></table>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>" 
SelectCommand="SELECT CostCenter.* FROM CostCenter" 
InsertCommand="INSERT INTO CostCenter(DeptId, EmployeeId, ApportionId, ApportionName, Balance, B_effective, Company, DeptName, EmployeeName) VALUES (@DeptId, @EmployeeId, @ApportionId, @ApportionName, CAST(@Balance AS char), @B_effective, @Company, @DeptName, @EmployeeName)"                 

                UpdateCommand="UPDATE Payroll_Master_Detail SET Amount=@Amount WHERE SalaryItem=@SalaryItem" >
            <InsertParameters>
                <asp:QueryStringParameter Name="DeptId" QueryStringField="DeptId" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
                <asp:QueryStringParameter Name="ApportionId" QueryStringField="ApportionId" />
                <asp:QueryStringParameter Name="ApportionName" 
                    QueryStringField="ApportionName" />
                <asp:QueryStringParameter Name="Balance" QueryStringField="Balance" />
                <asp:QueryStringParameter Name="B_effective" QueryStringField="B_effective" />
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="DeptName" QueryStringField="DeptName" />
                <asp:QueryStringParameter Name="EmployeeName" QueryStringField="EmployeeName" />
            </InsertParameters>
                 <UpdateParameters>
                <asp:Parameter Name="SalaryItem" />
                <asp:Parameter Name="Amount" />      
                 </UpdateParameters>

             </asp:SqlDataSource>
             <asp:HiddenField id="hid_IsInsertExit" runat="server" />
             <asp:HiddenField id="hid_updateid" runat="server" />
             <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><br/>
             <uc5:StyleContentEnd id="StyleContentEnd2" runat="server"></uc5:StyleContentEnd> 
             <uc6:StyleFooter ID="StyleFooter2" runat="server" />
       </ContentTemplate>
       </asp:UpdatePanel >
    </form>
</body>
</html>

