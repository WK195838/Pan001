<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdjustSalaryMaster_M.aspx.cs" Inherits="AdjustSalaryMaster_M" validaterequest="false" EnableEventValidation="false" %>

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

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>調薪資料維護</title>
    <base target="_self" />    
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div> 
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="調薪資料維護" />
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
                    DataKeyNames="Company,EmployeeId,EffectiveDate" 
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
                            <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" ReadOnly="true" />                                                   
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
            SelectCommand="SELECT Company,EmployeeId,EffectiveDate,ApproveDate FROM AdjustSalary_Master 
            WHERE (Company = @Company And EmployeeId = @EmployeeId And EffectiveDate = Convert(smalldatetime,@EffectiveDate)) 
            Group By Company,EmployeeId,EffectiveDate,ApproveDate"
            InsertCommand="INSERT INTO AdjustSalary_Master([Company],[EmployeeId],[EffectiveDate],[AdjustSalaryItem])
             VALUES (@Company, @EmployeeId, @EffectiveDate, '00')"
            UpdateCommand="UPDATE AdjustSalary_Master SET EffectiveDate = Convert(smalldatetime,@EffectiveDate)
             WHERE (Company = @Company And EmployeeId = @EmployeeId And EffectiveDate = Convert(smalldatetime,@EffectiveDate))">            
           <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="EffectiveDate" />                
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
                <asp:QueryStringParameter Name="EffectiveDate" QueryStringField="EffectiveDate" />                
            </SelectParameters>
        </asp:SqlDataSource>       
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_EffectiveDate" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        </div>
       <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>  
        <div>        
        <uc2:StyleHeader ID="StyleHeader2" runat="server" />
        <uc3:StyleTitle id="StyleTitle2" title="調薪項目列表" runat="server"></uc3:StyleTitle>
        <uc4:StyleContentStart id="StyleContentStart2" runat="server"></uc4:StyleContentStart> 
        <table id="table1" cellspacing="0" cellpadding="0" width="100%">
        <TBODY>
      <tr><td colspan=2><asp:Label id="lbl_Msg2" runat="server" ForeColor="red"></asp:Label></TD></tr>
            <tr><td colspan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></tr>
            <tr><td colspan=2>
            <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" 
            DataKeyNames="AdjustSalaryItem" GridLines="None" 
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
        <asp:LinkButton id="btnDelete" onclick="btnDelete_Click" runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />"
         OnClientClick='return confirm("確定刪除?");' L3PK='<%# DataBinder.Eval(Container, "DataItem.AdjustSalaryItem")%>'
         CausesValidation="False"></asp:LinkButton>
        </ItemTemplate>

        <HeaderStyle Width="30px"></HeaderStyle>
        </asp:TemplateField>
        <asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF"
         ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯"         
         >
        <HeaderStyle Width="60px"></HeaderStyle>
        <ItemStyle Width="60px"></ItemStyle>
        </asp:CommandField>
        <asp:BoundField DataField="AdjustSalaryItem" HeaderText="調薪項目" ReadOnly="true" SortExpression="AdjustSalaryItem" ></asp:BoundField>
        <asp:BoundField DataField="AdjustSalaryReasonCode" HeaderText="調薪原因" SortExpression="AdjustSalaryReasonCode" ></asp:BoundField>
        <asp:BoundField DataField="OldlSalary" HeaderText="調薪前金額" SortExpression="OldlSalary" ></asp:BoundField>
        <asp:BoundField DataField="NewSalary" HeaderText="調薪後金額" SortExpression="NewSalary" ></asp:BoundField>
        <asp:BoundField DataField="AdjustSalaryReason" HeaderText="調薪說明" SortExpression="AdjustSalaryReason" ></asp:BoundField>
        </Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>
<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">新增</td><td>調薪項目</td><td>調薪原因</td><td>調薪前金額</td><td>調薪後金額</td><td class="paginationRowEdgeRI">調薪說明</td></tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew01" runat="server" AutoPostBack="true" /></td>
<td class="Grid_GridLine"><uc8:CodeList ID="tbAddNew02" runat="server" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew03" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew04" /></td>
<td class="Grid_GridLine"><asp:TextBox runat="server" ID="tbAddNew05" /></td></tr>
</table>
</EmptyDataTemplate>
<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>
<%-- Company = @Company And EmployeeId = @EmployeeId And Convert(varchar,EffectiveDate,111) = @EffectiveDate) AndCompany = @Company And EmployeeId = @EmployeeId And Convert(varchar,EffectiveDate,111) = @EffectiveDate) And--%>
<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>" 
SelectCommand="SELECT [Company],[EmployeeId],[EffectiveDate]
      ,[AdjustSalaryItem]+' - '+(select SalaryName from SalaryStructure_Parameter where SalaryId=[AdjustSalaryItem] ) as [AdjustSalaryItem]
      ,[AdjustSalaryReasonCode]+' - '+(select CodeName from CodeDesc where CodeID='PY#AdjCode' And CodeCode=[AdjustSalaryReasonCode] ) as [AdjustSalaryReasonCode]
      ,[AdjustSalaryReason],[ApproveDate],[OldlSalary],[NewSalary]
FROM [AdjustSalary_Master]
WHERE (Company = @Company And EmployeeId = @EmployeeId And EffectiveDate = Convert(smalldatetime,@EffectiveDate)) " 
InsertCommand="INSERT INTO AdjustSalary_Master([Company],[EmployeeId],[EffectiveDate],[AdjustSalaryItem],[AdjustSalaryReasonCode],[AdjustSalaryReason],[OldlSalary],[NewSalary])
 VALUES (@Company, @EmployeeId, @EffectiveDate, @AdjustSalaryItem, @AdjustSalaryReasonCode, @AdjustSalaryReason, @OldlSalary, @NewSalary)" 
UpdateCommand="UPDATE AdjustSalary_Master SET AdjustSalaryItem = @AdjustSalaryItem, AdjustSalaryReasonCode = @AdjustSalaryReasonCode
            , AdjustSalaryReason = @AdjustSalaryReason, ApproveDate = ApproveDate, OldlSalary = @OldlSalary, NewSalary = @NewSalary
             WHERE (Company = @Company And EmployeeId = @EmployeeId And EffectiveDate = Convert(smalldatetime,@EffectiveDate))" >
            <InsertParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="AdjustSalaryItem" />
                <asp:Parameter Name="AdjustSalaryReasonCode" />
                <asp:Parameter Name="AdjustSalaryReason" />                
                <asp:Parameter Name="OldlSalary" />
                <asp:Parameter Name="NewSalary" />
            </InsertParameters>
           <UpdateParameters>
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="EmployeeId" />
                <asp:Parameter Name="EffectiveDate" />
                <asp:Parameter Name="AdjustSalaryItem" />
                <asp:Parameter Name="AdjustSalaryReasonCode" />
                <asp:Parameter Name="AdjustSalaryReason" />                
                <asp:Parameter Name="OldlSalary" />
                <asp:Parameter Name="NewSalary" />
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

