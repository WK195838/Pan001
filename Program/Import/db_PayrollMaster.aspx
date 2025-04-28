<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="db_PayrollMaster.aspx.cs" Inherits="db_PayrollMaster" %>

<%@ Register Src="../UserControl/SearchList.ascx" TagName="SearchList" TagPrefix="uc8" %>

<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc6" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
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
             <uc2:StyleTitle ID="StyleTitle1" runat="server" Title="人事資料匯入" />
             <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
             
               <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">公　　司：</span>&nbsp;</td>
                        <td align="left">
                            <uc1:CompanyList ID="CompanyList1" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>               
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">&nbsp;</span></td>
                        <td align="left">                            
                            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="確認轉換資料" />
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="清除轉換資料" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp; &nbsp;&nbsp;</td>
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
                             OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" AllowPaging="True" AllowSorting="True" PageSize="9000" >
                                 
                                 <Columns>
                                     <asp:BoundField DataField="EmployeeId" HeaderText="員工代號" />
                                     <asp:BoundField DataField="EmployeeName" HeaderText="員工姓名" />
                                     <asp:BoundField DataField="EnglishName" HeaderText="英文姓名" />
                                     <asp:BoundField DataField="DeptId" HeaderText="部門代號" />
                                     <asp:BoundField DataField="TitleCode" HeaderText="職稱編號" />
                                     <asp:BoundField DataField="Grade" HeaderText="職等編號" />
                                     <asp:BoundField DataField="Shift" HeaderText="班別代號" />
                                     <asp:BoundField DataField="Identify" HeaderText="編制別" />
                                     <asp:BoundField DataField="PayCode" HeaderText="計薪代號" />
                                     <asp:BoundField DataField="ResignCode" HeaderText="離職碼" />
                                     <asp:BoundField DataField="HireDate" HeaderText="到職日期">
                                     </asp:BoundField>
                                     <asp:BoundField DataField="LeaveWithoutPay" HeaderText="留職停薪日" />
                                     <asp:BoundField DataField="ReHireDate" HeaderText="復職日期" />
                                     <asp:BoundField DataField="ResignDate" HeaderText="離職日期" />
                                     <asp:BoundField DataField="ObserveExpirationDate" HeaderText="試 用 期 滿 日" />
                                     <asp:BoundField DataField="LstPromotionDate" HeaderText="最近調陞日" />
                                     <asp:BoundField DataField="LstChangeSalaryDate" HeaderText="最近調薪日" />
                                     <asp:BoundField DataField="LWC" HeaderText="福委會加入" />
                                     <asp:BoundField DataField="Union" HeaderText="工會加入" />
                                     <asp:BoundField DataField="SpecialSeniority" HeaderText="特加年資(月數)" />
                                     <asp:BoundField DataField="BloodType" HeaderText="血型" />
                                     <asp:BoundField DataField="IDNo" HeaderText="身份証號" />
                                     <asp:BoundField DataField="IDType" HeaderText="身份識別" />
                                     <asp:BoundField DataField="Sex" HeaderText="性別" />
                                     <asp:BoundField DataField="Nationality" HeaderText="國籍" />
                                     <asp:BoundField DataField="BirthDate" HeaderText="出生日期" />
                                     <asp:BoundField DataField="MaritalStatus" HeaderText="婚姻狀況" />
                                     <asp:BoundField DataField="DependentsNum" HeaderText="撫養人數" />
                                     <asp:BoundField DataField="Military" HeaderText="兵役" />
                                     <asp:BoundField DataField="BornPlace" HeaderText="出生地" />
                                     <asp:BoundField DataField="Addr" HeaderText="通訊地址" />
                                     <asp:BoundField DataField="ResidenceAddr" HeaderText="戶籍地址" />
                                     <asp:BoundField DataField="TEL" HeaderText="通訊電話" />
                                     <asp:BoundField DataField="MobilPhone" HeaderText="手機No." />
                                     <asp:BoundField DataField="Email" HeaderText="E Mail Address" />
                                     <asp:BoundField DataField="Contact" HeaderText="連絡人" />
                                     <asp:BoundField DataField="Guarantor1" HeaderText="保証人１" />
                                     <asp:BoundField DataField="Guarantor2" HeaderText="保証人２" />
                                     <asp:BoundField DataField="Introducer" HeaderText="介紹人" />
                                     <asp:BoundField DataField="ContactTEL" HeaderText="連絡人電話" />
                                     <asp:BoundField DataField="Guarantor1TEL" HeaderText="保証人１電話" />
                                     <asp:BoundField DataField="Guarantor2TEL" HeaderText="保証人２電話" />
                                     <asp:BoundField DataField="IntroducerTEL" HeaderText="介紹人電話" />
                                     <asp:BoundField DataField="CCN" HeaderText="成本中心" />
                                     <asp:BoundField DataField="EducationCode" HeaderText="學歷代碼" />
                                     <asp:BoundField DataField="Rank" HeaderText="職級" />
                                     <asp:BoundField DataField="ReportDeptId" HeaderText="節金出表部門代號" />
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

