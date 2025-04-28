<%@ Page Language="C#"　MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PYR013.aspx.cs" Inherits="RDLC_PYR013" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>

<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList"  TagPrefix="uc9" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>

<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
    <asp:ScriptManager id="ScriptManager1" runat="server">

    </asp:ScriptManager>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="異動查詢表" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
<!-- 搜尋模組 -->
<tr class="QueryStyle">
    <td align="left" style=" position:relative " colspan="2">
        <uc9:SearchList id="SearchList1" runat="server" />        
    </td>
</tr>
<!-- 搜尋模組 -->
        
        <tr align="left"><td colspan="2">
        <ul>
        <li style="float:left">異動期間：</li>
        <li style="float:left">
            <asp:TextBox ID="txtDateS" runat="server" Width="100px"></asp:TextBox>                            
            ～<asp:TextBox ID="txtDateE" runat="server" Width="100px"></asp:TextBox>
        </li>
        </ul>
        </td></tr>
        <tr align="left"><td colspan="2">
        <ul>
        <li style="float:left">查詢條件：</li>
        <li style="float:left">
        <asp:RadioButtonList ID="RDLCCondition" runat="server" RepeatDirection="Horizontal" CellPadding="5" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="SelectChanged" >
            <asp:ListItem Value="P" Selected="True">依個人</asp:ListItem>
            <asp:ListItem Value="C">依異動條件</asp:ListItem>
        </asp:RadioButtonList>
        </li>
        </ul>
        </td></tr>
        <tr align="left"><td colspan="2">
        <ul>
        <li style="float:left">異動條件：</li>
        <li style="float:left">
        <asp:RadioButtonList ID="AdjCondition" runat="server" RepeatDirection="Horizontal" CellPadding="5" Width="300px" Enabled="false" >
            <asp:ListItem Value="All" Selected="True">全部</asp:ListItem>
            <asp:ListItem Value="PA">依職務異動</asp:ListItem>
            <asp:ListItem Value="AM">依調薪異動</asp:ListItem>
            <asp:ListItem Value="PM">依人事異動</asp:ListItem>
        </asp:RadioButtonList>
        </li>
        </ul>
        </td></tr>
        <tr align="left"><td colspan="2">
        <ul>

        <li>　　
         <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" Visible="false" />
         <asp:Button ID="Button1" runat="server" Text="報表" OnClick="Button1_Click" /></li></ul></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        <tr><td colspan="2"><uc7:Navigator id="Navigator1" runat="server" ></uc7:Navigator></td></tr>
        <tr><td colspan="2"><asp:Panel id="Panel_Empty" runat="server" Width="250px" Visible="False" Height="50px">
        <br />查無資料!!<asp:ImageButton id="btnEmptyNew" runat="server" SkinID="NewAdd" /></asp:Panel></td></tr>
        <tr><td colspan="2">
            <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="400px" width="100%">
            </rsweb:reportviewer>
        </td></tr>
        <tr><td colspan="2">
        <asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True"
         OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound" 
          DataKeyNames="Company,EmployeeId" AutoGenerateColumns="False"
          OnSorting="GridView1_Sorting">
         <Columns>
<%--             <asp:BoundField DataField="ROWID" HeaderText="項次" />
             <asp:BoundField DataField="EmployeeId" HeaderText="工號" SortExpression="EmployeeId" />
             <asp:BoundField DataField="EmployeeName" HeaderText="姓名" SortExpression="EmployeeName" ItemStyle-Wrap="false" />
             <asp:BoundField DataField="AdjustmentCategory" HeaderText="調動類別" HtmlEncode="false" />
             <asp:BoundField DataField="EffectiveDate" HeaderText="生效日期" HtmlEncode="false"  />
             <asp:BoundField DataField="DepCode_F" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="DepCode_T" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Title_F" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Title_T" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Level_F" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Level_T" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="SalarySystem_F" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="SalarySystem_T" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Class_F" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Class_T" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="ACompany" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="AEmployeeId" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="AEffectiveDate" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="AdjustSalaryItem" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="AdjustSalaryReasonCode" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="AdjustSalaryReason" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="ApproveDate" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="OldlSalary" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="NewSalary" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             PA.Company,PA.EmployeeId,PA.AdjustmentCategory,PA.EffectiveDate
	,PA.DepCode_F,PA.DepCode_T,PA.Title_F,PA.Title_T,PA.Level_F,PA.Level_T,PA.SalarySystem_F,PA.SalarySystem_T,PA.Class_F,PA.Class_T,PA.ResignReason,PA.MasterUpdate
	,AM.Company as ACompany,AM.EmployeeId as AEmployeeId,AM.EffectiveDate as AEffectiveDate,AM.AdjustSalaryItem,AM.AdjustSalaryReasonCode,AM.AdjustSalaryReason,AM.ApproveDate,(AM.OldlSalary) as OldlSalary,(AM.NewSalary) as NewSalary
             
             <asp:BoundField DataField="BaseSalary" HeaderText="本薪</BR><font size='0.5px'>(捨)(離進)</font>" HtmlEncode="false" />
             <asp:BoundField DataField="Other01" HeaderText="職務</BR>加給" HtmlEncode="false"  />
             <asp:BoundField DataField="Attendance" HeaderText="全勤" />
             <asp:BoundField DataField="OtherWT" HeaderText="其他薪</BR>(應稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="OtherNT" HeaderText="其他</BR>(免稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="WT_Overtime_fee" HeaderText="應稅</BR>加班" HtmlEncode="false" />
             <asp:BoundField DataField="NT_Overtime_fee" HeaderText="免稅</BR>加班" HtmlEncode="false" />             
             <asp:BoundField DataField="PAmount" HeaderText="加項合計" />
             <asp:BoundField DataField="LeaveDeduction" HeaderText="請假小計" HtmlEncode="false" />
             <asp:BoundField DataField="LI_Fee" HeaderText="勞保費" />
             <asp:BoundField DataField="HI_Fee" HeaderText="健保費" />    
             <asp:BoundField DataField="FB_Fee" HeaderText="福利金" />
             <asp:BoundField DataField="TAX" HeaderText="所得稅" />
             <asp:BoundField DataField="Parking_Fee" HeaderText="停車費" />
             <asp:BoundField DataField="WR_Fee" HeaderText="婚喪</BR>禮金" HtmlEncode="false"  />
             <asp:BoundField DataField="OtherNTP" HeaderText="其他</BR>(免稅)" HtmlEncode="false"  />
             <asp:BoundField DataField="MAmount" HeaderText="扣項合計" />
             <asp:BoundField DataField="PayAmount" HeaderText="實領薪資" />
             <asp:BoundField DataField="WT_Amount" HeaderText="應稅金額" />
             <asp:BoundField DataField="Pension" HeaderText="退休金提撥" />--%>
         </Columns>
     </asp:GridView>
     </td></tr>
     </table>
     <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
     </ContentTemplate>
    </asp:UpdatePanel>              
</asp:Content>