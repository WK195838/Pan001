<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYRBonus.aspx.cs" Inherits="PYRBonus" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/MonthList.ascx" TagName="MonthList" TagPrefix="uc10" %>
<%-- 載入共用變數  --%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
 <%--用於執行等畫面(End) --%>
    <script language="javascript" type="text/javascript">
//特別控制
</script>    
   <%-- <div>--%>
        <%-- 載入共用變數  --%>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp;<uc2:StyleTitle id="StyleTitle1" title="獎金發放表" runat="server"></uc2:StyleTitle> 
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart>
<%-- 畫面標題顯示 end --%>
<%-- 搜尋頁面 start --%>
<table id="table1"  cellspacing="0" cellpadding="0" width="100%">
<tbody>
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" 
        style=" position:relative; top: 0px; left: 0px; height: 224px;">
        
        <uc9:SearchList ID="SearchList1" runat="server" />
        獎金名目：<asp:DropDownList ID="CostId" runat="server" OnSelectedIndexChanged="CostId_SelectedIndexChanged"
           AutoPostBack="true"   Width="138px">
        </asp:DropDownList><br />
        發放年月：<asp:RadioButton ID="RB_Sel1" runat="server" AutoPostBack="True" OnCheckedChanged="RB_Sel1_CheckedChanged"
            Text="全部" />
        <asp:RadioButton ID="RB_Sel2" runat="server" AutoPostBack="True" OnCheckedChanged="RB_Sel2_CheckedChanged"
            Text="民國" />
        <uc8:YearList ID="YearList1" runat="server" AutoPostBack="True" />
        年<uc10:MonthList ID="MonthList1" runat="server" AutoPostBack="True" />
        月<%--<asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd" Visible="False"></asp:ImageButton>&nbsp;--%><asp:ImageButton ID="btnQuery" runat="server" onclick="btnQuery_Click" SkinID="Query1" />
        <asp:Button ID="PRT_Cor" runat="server" Text="報表列印" Width="67px" 
            onclick="PRT_Cor_Click" />
        <br />
    </td>         
</tr>
<!-- 搜尋模組 -->
<%--<tr class="QueryStyle">
<td align="left">
     <span class="ItemFontStyle">公    司：</span>&nbsp;</td>
<td align="left">
    <uc1:CompanyList id="CompanyList1" runat="server"  ></uc1:CompanyList></td>
</tr>
<tr class="QueryStyle">
<td align="left">
    <span class="ItemFontStyle">員    工：</span>&nbsp;</td>
<td align="left">
    <asp:TextBox id="tbEmployeeId" runat="server" MaxLength="50"></asp:TextBox>
    <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1"></asp:ImageButton>
    <asp:ImageButton id="btnNew" runat="server" SkinID="NewAdd"></asp:ImageButton></td>
</tr>--%>
<%-- 搜尋頁面 end --%>
<%-- 錯誤訊息與資料筆數切換 start --%>
<%-- ReportView--%>
<tr>
<td colspan="2">
    <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
        <localreport reportpath="">
        </localreport>
    </rsweb:ReportViewer>
    <br />
    </td>
</tr>
<tr>
<td colspan="2" style="height: 10px">
    <uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator></td>
</tr>
<%-- 錯誤訊息與資料筆數切換 end --%>
<%-- 無資料時顯示panel start --%>
<tr>
    <td align="left" colspan="2" style="height: 76px">
        <asp:Panel id="Panel_Empty" runat="server" Width="250px" Height="50px"><br />查無資料!!
        <asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel>
    <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" 
            AllowPaging="True" DataSourceID="SDS_GridView" 
    OnRowCreated="GridView1_RowCreated" 
    OnRowDeleting="GridView1_RowDeleting" 
    OnRowDeleted="GridView1_RowDeleted" 
    OnRowDataBound="GridView1_RowDataBound" 
    DataKeyNames="Company,EmployeeId,CostName,AmtDate,DepName,Row_Count" 
    AutoGenerateColumns="False" Height="53px">          
    <Columns>  
        <asp:BoundField DataField="DepName" HeaderText="部門" SortExpression="DepName" />
        <asp:BoundField DataField="EmployeeId" HeaderText="員工" SortExpression="EmployeeId" />
        <asp:BoundField DataField="CostName" HeaderText="獎金名目" SortExpression="CostName" />
        <asp:BoundField DataField="CostAmt" HeaderText="發放總金額(稅前)" 
            SortExpression="CostAmt" />
        <asp:BoundField DataField="Pay_AMT" HeaderText="代扣繳稅額" 
            SortExpression="Pay_AMT" />
        <asp:BoundField DataField="HI2" HeaderText="補充保費" SortExpression="HI2" />
        <asp:BoundField HeaderText="實際金額(稅後)" />
        <asp:BoundField DataField="AmtDate" HeaderText="發放年月" SortExpression="AmtDate" />
        <asp:BoundField DataField="DepositBank" HeaderText="存入銀行" SortExpression="DepositBank" Visible="False" />
        <asp:BoundField DataField="DepositBankAccount" HeaderText="銀行帳號"  SortExpression="DepositBankAccount" Visible="False" />
        <asp:BoundField DataField="ControlDown" HeaderText="是否已發放" SortExpression="ControlDown" />
        <asp:BoundField DataField="Company" HeaderText="公司名稱" SortExpression="Company" Visible="False" />
        <asp:BoundField DataField="Row_Count" HeaderText="項次" 
            SortExpression="Row_Count" Visible="False" />
    </Columns>
    </asp:GridView>
        <br />
    </td>
</tr>
<%-- 無資料時顯示panel end --%>
<tr>
<td colspan="4">
<%-- GridView 功能設定 start --%>
    </td>
<%-- GridView 功能設定 end --%>  
</tr>
</tbody>
</table>
<%-- Sql語法設定與datasource來源 start --%>
<asp:SqlDataSource id="SDS_GridView" runat="server" 
SelectCommand="Select A.Company,(A.DepId+'-'+A.DepName) DepName,(A.EmployeeId+'-'+A.EmployeeName) EmployeeId,A.CostName,A.CostAmt,A.AmtDate,A.DepositBank,A.DepositBankAccount From BonusMaster  Where 1=1 AND A.Company = @Company And A.EmployeeId= @EmployeeId " 
DeleteCommand="DELETE FROM BonusMaster WHERE Company = @Company And EmployeeId = @EmployeeId " 
ConnectionString="<%$ ConnectionStrings:MyConnectionString %>">
<DeleteParameters>
    <asp:Parameter Name="Company" />
    <asp:Parameter Name="EmployeeId" />
    <asp:Parameter Name="DepositBank" />
    <asp:Parameter Name="DepositBankAccount" />
</DeleteParameters>
</asp:SqlDataSource>
<%-- Sql語法設定與datasource來源 end --%>
    <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
</asp:UpdatePanel>
        &nbsp;
        <br />
        <br />
   <%-- </div>--%>
</asp:Content>


