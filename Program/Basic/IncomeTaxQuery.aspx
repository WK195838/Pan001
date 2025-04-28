<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="IncomeTaxQuery.aspx.cs" Inherits="Basic_IncomeTaxQuery" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="SalaryYM" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<script language ="javascript">
//特別控制
</script>    
    <div id="DIV1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   
             <uc3:ShowMsgBox ID="ShowMsgBox1" runat="server" />
                &nbsp;
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="所得稅扣繳資料彙整" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left" colspan="2" rowspan="2"><span class="ItemFontStyle"></span>
                            <uc8:SearchList ID="SearchList1" runat="server" />
                        </td>
                    </tr>               
                    <tr class="QueryStyle">
                    </tr>
                    <tr align="left" class="QueryStyle">
                        <td colspan="2"><span class="ItemFontStyle">年　　度：</span>                            
                            <SalaryYM:SalaryYM id="SalaryYM1" runat="server" />～
                            <SalaryYM:SalaryYM id="SalaryYM2" runat="server" />
                        </td>
                    </tr>
                    <tr align="left" class="QueryStyle"><td colspan="2">是否在職：<asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList>                        
                    </td></tr>
                    <tr align="left" class="QueryStyle"><td colspan="2">　　　　　<asp:Button id="btnQuery" runat="server" Text=" 查  詢 " />
                    　<asp:Button id="btnToExcel" runat="server" Text=" 匯出 EXCEL " />
                    </td></tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label>                    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc7:Navigator ID="Navigator1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Panel ID="Panel_Empty" runat="server" Height="50px" Visible="False" Width="250px">
                                <br />查無資料!!</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" AllowPaging="True" AllowSorting="True" >
                                 
                                 <Columns>
                                     <asp:BoundField DataField="SalaryYM" HeaderText="月份" />
                                     <asp:BoundField DataField="DeptId" HeaderText="部門代號" />
                                     <asp:BoundField DataField="DepName" HeaderText="部門名稱" />
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工代號" />
                                     <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />
                                     <asp:BoundField DataField="OtherNWSalary" HeaderText="應稅所得" DataFormatString="{0:N}" />                                     
                                     <asp:BoundField DataField="SalaryAmount" HeaderText="扣繳金額" DataFormatString="{0:N}" />
                                 </Columns>
                             </asp:GridView>
                        </td>
                    </tr>
                </table>
             <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
    </div>
</asp:Content>

