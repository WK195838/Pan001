<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="AdjustSalaryCheck.aspx.cs" Inherits="Basic_AdjustSalaryCheck" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
//Company	公司編號
//EmployeeId	員工編號
//EffectiveDate	生效日期
//AdjustSalaryItem	調薪項目
//AdjustSalaryReasonCode	調薪原因代碼
//AdjustSalaryReason	調薪原因
//ApproveDate	核定日期
//OldlSalary	調薪前金額
//NewSalary	調薪後金額

</script>    
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
<uc3:ShowMsgBox id="ShowMsgBox1" runat="server"></uc3:ShowMsgBox> &nbsp; 
<uc2:StyleTitle id="StyleTitle1" title="調薪資料核定" runat="server"></uc2:StyleTitle>
<uc4:StyleContentStart id="StyleContentStart1" runat="server"></uc4:StyleContentStart> 
<TABLE id="table1" cellSpacing=0 cellPadding=0 width="100%">
<TBODY>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">公    司：</SPAN>&nbsp;</TD>
<TD align=left> 
    <uc1:CompanyList id="CompanyList1" runat="server" ></uc1:CompanyList></TD></TR>
<TR class="QueryStyle"><TD align=left>
    <SPAN class="ItemFontStyle">生效日期：</SPAN>&nbsp;</TD>
<TD align=left>
    <asp:TextBox id="EffectiveDate" runat="server" MaxLength="50"></asp:TextBox> 
    <%--<asp:ImageButton id="btnCalendar1" runat="server" SkinID="Calendar1"></asp:ImageButton>--%> </TD></TR>
<TR><TD colSpan=2>
    <asp:Label id="lbl_Err" runat="server" ForeColor="RED"></asp:Label>
    <asp:Label id="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></TD></TR>
<TR><TD style="HEIGHT: 20px" colSpan=2>
    <asp:Button id="btnSearch" onclick="btnSearch_Click" runat="server" Text="搜尋符合資料"></asp:Button> 
    <asp:Button id="btClearErrData" onclick="btnClearErrData_Click" runat="server" Text="清除空白資料"></asp:Button>
    <asp:Button id="btnCheck" onclick="btnCheck_Click" runat="server" Text="開始核定"></asp:Button></TD></TR>
</TBODY>
</TABLE>
 <asp:GridView ID="GridView1" runat="server"
  Width="100%" 
 ShowFooter="false"
 OnRowCreated="GridView1_RowCreated"
 OnRowDataBound="GridView1_RowDataBound" 
  GridLines="None" 
 >
 </asp:GridView>
<uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
</ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>
