<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayrollMaster_M.aspx.cs" Inherits="PayrollMaster_M" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>

<%--//Company	公司編號
//EmployeeId	員工編號
//DepositBank	存入銀行 
//DepositBankAccount	存入帳號
//Period2DepositDate	下期存入日期
//Period1DepositDate	上期存入日期
//Company	公司編號
//EmployeeId	員工編號
//SalaryItem	薪資項目
//Amount	金額--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>薪資基本資料維護</title>
    <base target="_self" />    
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div> 
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="薪資基本資料維護" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" Width="100%" AutoGenerateRows="False" 
                    DataSourceID="SqlDataSource1" 
                    DataKeyNames="Company,EmployeeId" 
                    OnItemCreated="DetailsView1_ItemCreated"                     
                    OnDataBound="DetailsView1_DataBound"
                    OnItemInserting="DetailsView1_ItemInserting"
                    OnItemInserted="DetailsView1_ItemInserted"                    
                    OnItemUpdating="DetailsView1_ItemUpdating"
                    OnItemUpdated="DetailsView1_ItemUpdated"
                    >
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditRowStyle HorizontalAlign="Left" />
                        <Fields> 
                            <asp:BoundField DataField="Company" HeaderText="Company" SortExpression="Company" ReadOnly="true"/>
                            <asp:BoundField DataField="EmployeeId" HeaderText="EmployeeId" SortExpression="EmployeeId" ReadOnly="true" />
                            <asp:BoundField DataField="DepositBank" HeaderText="DepositBank" SortExpression="DepositBank"  />
                            <asp:BoundField DataField="DepositBankAccount" HeaderText="DepositBankAccount" SortExpression="DepositBankAccount"  />
                            <asp:BoundField DataField="Period2DepositDate" HeaderText="Period2DepositDate" SortExpression="Period2DepositDate" />
                            <asp:BoundField DataField="Period1DepositDate" HeaderText="Period1DepositDate" SortExpression="Period1DepositDate" />
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
            SelectCommand="SELECT Company,EmployeeId,DepositBank,DepositBankAccount,Period2DepositDate=Convert(varchar,Period2DepositDate,120),Period1DepositDate=Convert(varchar,Period1DepositDate,120) FROM Payroll_Master_Heading WHERE (Company = @Company And EmployeeId = @EmployeeId )"
            InsertCommand="INSERT INTO Payroll_Master_Heading([Company]
           ,[EmployeeId]
           ,[DepositBank]
           ,[DepositBankAccount]
           ,[Period2DepositDate]
           ,[Period1DepositDate]) VALUES (@Company, @EmployeeId, @DepositBank, @DepositBankAccount, @Period2DepositDate, @Period1DepositDate)"
            UpdateCommand="UPDATE Payroll_Master_Heading SET Period2DepositDate = Convert(smalldatetime,@Period2DepositDate),Period1DepositDate=Convert(smalldatetime,@Period1DepositDate)  
            ,DepositBank = @DepositBank,DepositBankAccount=@DepositBankAccount
            WHERE (Company = @Company And EmployeeId = @EmployeeId )">
            <%--And DepositBank = @DepositBank And DepositBankAccount=@DepositBankAccount And Convert(varchar,EffectiveDate,111) = @EffectiveDate--%>
           <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="DepositBank" />
                <asp:Parameter Name="DepositBankAccount" />
                <asp:Parameter Name="Period2DepositDate" />
                <asp:Parameter Name="Period1DepositDate" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_DepositBank" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        </div>
       <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
        <div >
        <uc2:StyleHeader ID="StyleHeader3" runat="server" />
        <uc4:StyleContentStart ID="StyleContentStart3" runat="server" />
        <p style="text-align:left">
            <table>
            <tr><td></td><td style="width:100px"></td><td></td><td style="width:100px"></td><td></td><td style="width:100px"></td></tr>
            <tr><td><asp:Label ID="labSalaryLevel" runat="server" Text="　職　　等：" />
            </td><td><asp:Label ID="lblSalaryLevel" runat="server"  />
            </td><td><asp:Label ID="labSalaryRank" runat="server" Text="　級　　數：" />
            </td><td><asp:Label ID="lblSalaryRank" runat="server"  />
            </td></tr>
            <tr><td><asp:Label ID="labSalaryLevel01" runat="server" Text="　核薪下限：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel01" runat="server"  />
            </td><td><asp:Label ID="labSalaryLevel02" runat="server" Text="　核薪上限：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel02" runat="server"  />
            </td><td colspan="2"><asp:Label ID="SalaryMsg01" runat="server" CssClass="Grid_GridLineDetailRed" />            
            </td></tr>
            <tr><td><asp:Label ID="labSalaryLevel03" runat="server" Text="　本　　薪：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel03" runat="server"  />
            </td><td><asp:Label ID="labSalaryLevel04" runat="server" Text="　薪　　點：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel04" runat="server"  />
            </td><td>
            </td><td><asp:Label ID="lbl_SalsryPoint" runat="server" />            
            </td></tr>
            <tr><td><asp:Label ID="labSalaryLevel05" runat="server" Text="已分配金額：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel05" runat="server"  />
            </td><td><asp:Label ID="labSalaryLevel06" runat="server" Text="　薪點餘額：" />
            </td><td style="text-align:right"><asp:Label ID="lblSalaryLevel06" runat="server"  />
            </td><td colspan="2"><asp:Label ID="SalaryMsg02" runat="server" CssClass="Grid_GridLineDetailRed" />            
            </td></tr>
            </table>
        </p>
        <uc5:StyleContentEnd ID="StyleContentEnd3" runat="server" />
        <uc6:StyleFooter ID="StyleFooter3" runat="server" />
        </div>        
        <div>        
        <uc2:StyleHeader ID="StyleHeader2" runat="server" />
        <uc3:StyleTitle id="StyleTitle2" title="薪資基本資料列表" runat="server"></uc3:StyleTitle>
        <uc4:StyleContentStart id="StyleContentStart2" runat="server"></uc4:StyleContentStart> 
        <table id="table1" cellspacing="0" cellpadding="0" width="100%">
        <TBODY>
      <TR><TD colSpan=2><asp:Label id="lbl_Msg2" runat="server" ForeColor="red"></asp:Label></TD></TR>
            <TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
            <TR><TD colSpan=2>
            <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" 
            DataKeyNames="SalaryItem" GridLines="None" 
            OnRowCreated="GridView1_RowCreated" 
            OnRowEditing="GridView1_RowEditing" 
            OnRowCommand="GridView1_RowCommand"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowUpdating="GridView1_RowUpdating" 
            OnRowUpdated="GridView1_RowUpdated"            
            OnRowDataBound="GridView1_RowDataBound" 
            DataSourceID="SDS_GridView" ShowFooter="True"             
            >
        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
        <asp:TemplateField HeaderText="刪除" ShowHeader="False">
        <ItemTemplate>
        <asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClientClick='return confirm("確定刪除?");'  L3PK='<%# DataBinder.Eval(Container, "DataItem.SalaryItem")%>' CausesValidation="False"></asp:LinkButton> <%--L1PK='<%# DataBinder.Eval(Container, "DataItem.Company")%>' L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>''<%# DataBinder.Eval(Container, "DataItem.Company")%>' L2PK='<%# DataBinder.Eval(Container, "DataItem.EmployeeId")%>' L3PK='<%# DataBinder.Eval(Container, "DataItem.EffectiveDate")%>' L4PK=--%>
        </ItemTemplate>

        <HeaderStyle Width="30px"></HeaderStyle>
        </asp:TemplateField>
        <asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF"
         ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯"         
         >
        <HeaderStyle Width="60px"></HeaderStyle>
        <ItemStyle Width="60px"></ItemStyle>
        </asp:CommandField>
        <asp:BoundField DataField="SalaryItem" HeaderText="薪資項目" ReadOnly="true" SortExpression="SalaryItem" ></asp:BoundField>
        <asp:BoundField DataField="Amount" HeaderText="金額" SortExpression="Amount" ></asp:BoundField>
        </Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>薪資項目</td><%--<td>薪資別</td>--%><td class="paginationRowEdgeRI">金額</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew01" runat="server" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew02" /></td></tr>
</table>
</EmptyDataTemplate>
<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>
<%-- Company = @Company And EmployeeId = @EmployeeId And Convert(varchar,EffectiveDate,111) = @EffectiveDate) AndCompany = @Company And EmployeeId = @EmployeeId And Convert(varchar,EffectiveDate,111) = @EffectiveDate) And--%>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" 
SelectCommand="Select (PMD.SalaryItem+''+SSP.SalaryName) as SalaryItem,(SSP.ItemType+''+SSP.SalaryType) As SalaryItem,PMD.Amount,SSP.ItemType FROM Payroll_Master_Detail PMD,SalaryStructure_Parameter SSP where SSP.SalaryId=PMD.SalaryItem" 
InsertCommand="INSERT INTO Payroll_Master_Detail([Company],[EmployeeId],[SalaryItem],[Amount]) VALUES ( @Company, @EmployeeId,@SalaryItem, @Amount)" 
UpdateCommand="UPDATE Payroll_Master_Detail SET Amount=@Amount WHERE SalaryItem=@SalaryItem" >
            <InsertParameters>
                <asp:Parameter Name="SalaryItem" />
                <asp:Parameter Name="Amount" />
            </InsertParameters>
                 <UpdateParameters>
                <asp:Parameter Name="SalaryItem" />
                <asp:Parameter Name="Amount" />      
                 </UpdateParameters>

             </asp:SqlDataSource>
             <asp:HiddenField id="hid_IsInsertExit" runat="server" />
             <asp:HiddenField id="hid_updateid" runat="server" />
             <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
             <uc5:StyleContentEnd id="StyleContentEnd2" runat="server"></uc5:StyleContentEnd> 
             <uc6:StyleFooter ID="StyleFooter2" runat="server" />

       </div>
       </ContentTemplate>
       </asp:UpdatePanel> 
    </form>
</body>
</html>

