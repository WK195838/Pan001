﻿<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="OverTimeTrxStatistics.aspx.cs" Inherits="OverTimeTrxStatistics" Title="" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/PeriodList.ascx" TagName="PeriodList" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc8" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <%--用於執行等畫面(Begin) --%>
    <div id="divWait" style="z-index: 999; width: 0px; position: absolute; height: 0px">
    </div>
    <%--用於執行等畫面(End) --%>
     <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
  
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
        <uc3:StyleTitle ID="StyleTitle1" Title="加班統計表" />
        <uc4:StyleContentStart id="StyleContentStart1" runat="server" />
        <table cellspacing="0" cellpadding="0" width="100%">
        <tr align="left"><td>公　　司：</td><td><uc2:CompanyList ID="CompanyList1" runat="server" /></td></tr>
        <tr align="left"><td>部　　門：</td><td><asp:DropDownList ID="DepList1" runat="server" Width="165" AutoPostBack="true"/></td></tr>
        <tr align="left"><td>員　　工：</td><td><asp:DropDownList ID="EmployeeIdList1" runat="server" Width="165" AutoPostBack="true"/></td></tr>
        <tr align="left"><td>加班年月：</td><td><uc8:SalaryYM id="SalaryYM1" runat="server" />(僅限單一月份：<asp:RadioButtonList ID="CKYM" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ></asp:RadioButtonList>)</td></tr>
        <tr align="left"><td>是否在職：</td><td><asp:CheckBoxList ID="cbResignC" runat="server" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList></td></tr>
        <tr><td colspan="2">
            <asp:Button id="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" /></td></tr>
        <tr><td colspan="2">
            <asp:Label ID="LabMsg" runat="server" Text="" ForeColor="red"></asp:Label></td></tr>
        </table>
        <uc5:StyleContentEnd id="StyleContentEnd1" runat="server"></uc5:StyleContentEnd> 
        </ContentTemplate>
        </asp:UpdatePanel>
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
          EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
         ReportSourceID="cryReportSource" ReuseParameterValuesOnRefresh="True" />
     <CR:CrystalReportSource ID="cryReportSource" runat="server">
         <Report FileName="OverTimeTrxStatistics.rpt">
         </Report>
     </CR:CrystalReportSource>
 </asp:Content>

