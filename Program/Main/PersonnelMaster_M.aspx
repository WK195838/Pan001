<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonnelMaster_M.aspx.cs" Inherits="PersonnelMaster_M" EnableEventValidation="false" %>

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
<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc9" %>
<%@ Register Src="../UserControl/SalaryYM.ascx" TagName="SalaryYM" TagPrefix="uc10" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>人事資料維護</title>
    <base target="_self" />    
    <link href="~/App_Themes/ui-lightness/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/ui-lightness/ui.datepicker.css" rel="stylesheet" type="text/css" />    
<script language="javascript">
///////////////////////////////////////////////////////////////////////////////////////////
//  版面控制
///////////////////////////////////////////////////////////////////////////////////////////
	//頁籤背景變色
	var _oldbg;
	function setnewbg(source)
    {
        _oldbg=source.style.backgroundImage;
        source.style.backgroundImage= "url(../App_Themes/images/Tab3.GIF)";
    }

	function setoldbg(source)
    {
        source.style.backgroundImage= _oldbg
    }	
</script>	    
</head>
<body>
    <form id="form1" runat="server">
    <div>
 
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader0" runat="server" />
        <uc3:StyleTitle ID="StyleTitle0" runat="server" Title="人事資料維護" />
        <uc4:StyleContentStart ID="StyleContentStart0" runat="server" />
        <asp:ScriptManager id="ScriptManager0" runat="server">
        </asp:ScriptManager>  
        <asp:UpdatePanel id="UpdatePanel0" runat="server">
        <ContentTemplate>   
        <table width="100%">
            <tr>
                <td class="Grid_GridLine">
                    <asp:DetailsView ID="DetailsView1" runat="server"  DataKeyNames="Company,EmployeeId"
                        DataSourceID="SqlDataSource1" Width="100%" OnDataBound="DetailsView1_DataBound"
                        OnItemUpdating="DetailsView1_ItemUpdating" OnItemUpdated="DetailsView1_ItemUpdated"
                        OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting"
                        OnItemCommand="DetailsView1_ItemCommand"
                        AutoGenerateRows="False">                        
                        <Fields>
                        <asp:TemplateField>
                        <ItemTemplate>
                        <asp:Panel runat="server" Visible="true" ID="Master">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_Company" runat="server" Text="Company" /></td><td class="Grid_GridLine" style="width:15%"><uc9:CompanyList ID="lbl_CompL_Company" runat="server" /></td>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_EmployeeId" runat="server" Text="EmployeeId" /></td><td class="Grid_GridLine" style="width:15%"><asp:Label ID="lbl_EmployeeId" runat="server" Text='<%# Eval("EmployeeId") %>' /></td>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_DeptId" runat="server" Text="部門" /></td><td class="Grid_GridLine" style="width:15%"><uc8:CodeList ID="lbl_CL_DeptId" runat="server" /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeName" runat="server" Text="EmployeeName" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EnglishName" runat="server" Text="EnglishName" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_EnglishName" runat="server" Text='<%# Eval("EnglishName") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BirthDate" runat="server" Text="BirthDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_BirthDate" runat="server" Text='<%# Eval("BirthDate") %>' /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Sex" runat="server" Text="Sex" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Sex" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MobilPhone" runat="server" Text="MobilPhone" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_MobilPhone" runat="server" Text='<%# Eval("MobilPhone") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Email" runat="server" Text="Email" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Email" runat="server" Text='<%# Eval("Email") %>' /></td>                        
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EducationCode" runat="server" Text="學歷" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_EducationCode" runat="server" /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TitleCode" runat="server" Text="職稱" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_TitleCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Rank" runat="server" Text="職級" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Rank" runat="server" /></td>                        
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignCode" runat="server" Text="是否在職" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_ResignCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_HireDate" runat="server" Text="HireDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_HireDate" runat="server" Text='<%# Eval("HireDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignDate" runat="server" Text="ResignDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ResignDate" runat="server" Text='<%# Eval("ResignDate") %>' /></td>                                                
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isBoss" runat="server" Text="是否雇主" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_isBoss" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isSales" runat="server" Text="是否業務" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_isSales" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ReHireDate" runat="server" Text="復職日期" /></td><td class="Grid_GridLine">
                        <asp:Label ID="lbl_ReHireDate" runat="server" Text='<%# Eval("ReHireDate") %>' />
                        </td>                        
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PlusYear" runat="server" Text="加計年資" />
                        <asp:Label ID="lblTitle_PlusMonth" runat="server" Text="" /></td><td class="Grid_GridLine">　
                        <asp:Label ID="lbl_PlusYear" runat="server" Text='<%# Eval("PlusYear") %>' /> 年 <asp:Label ID="lbl_PlusMonth" runat="server" Text='<%# Eval("PlusMonth") %>' /> 月</td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Year01" runat="server" Text="年齡" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Year01" runat="server" Text='0' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Year02" runat="server" Text="年資" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Year02" runat="server" Text='0' /></td>                        
                        </tr>                                                                                                                                                                                 
                        </table>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible="false" ID="Other">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IDNo" runat="server" Text="IDNo" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_IDNo" runat="server" Text='<%# Eval("IDNo") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IDType" runat="server" Text="IDType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_IDType" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BloodType" runat="server" Text="BloodType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_BloodType" runat="server" /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MaritalStatus" runat="server" Text="MaritalStatus" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_MaritalStatus" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DependentsNum" runat="server" Text="DependentsNum" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_DependentsNum" runat="server" Text='<%# Eval("DependentsNum") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Military" runat="server" Text="Military" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Military" runat="server" /></td>                        
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Nationality" runat="server" Text="Nationality" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Nationality" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BornPlace" runat="server" Text="BornPlace" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_BornPlace" runat="server" Text='<%# Eval("BornPlace") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResidenceAddr" runat="server" Text="ResidenceAddr" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ResidenceAddr" runat="server" Text='<%# Eval("ResidenceAddr") %>' /></td>                        
                        </tr>    
                        <tr>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TEL" runat="server" Text="TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_TEL" runat="server" Text='<%# Eval("TEL") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Addr" runat="server" Text="Addr" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Addr" runat="server" Text='<%# Eval("Addr") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LISubsidy" runat="server" Text="LISubsidy" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_LISubsidy" runat="server" /></td>
                        </tr>                                               
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_SpecialSeniority" runat="server" Text="SpecialSeniority" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_SpecialSeniority" runat="server" Text='<%# Eval("SpecialSeniority") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LWC" runat="server" Text="LWC" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_LWC" runat="server" /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Union" runat="server" Text="Union" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Union" runat="server" /></td>
                        </tr>                         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Identify" runat="server" Text="編制別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Identify" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Grade" runat="server" Text="職等" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Grade" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Shift" runat="server" Text="班別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Shift" runat="server" /></td>                                                
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PayCode" runat="server" Text="PayCode" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_PayCode" runat="server" /></td>                                                                  
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Introducer" runat="server" Text="Introducer" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Introducer" runat="server" Text='<%# Eval("Introducer") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IntroducerTEL" runat="server" Text="IntroducerTEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_IntroducerTEL" runat="server" Text='<%# Eval("IntroducerTEL") %>' /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Contact" runat="server" Text="Contact" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Contact" runat="server" Text='<%# Eval("Contact") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1" runat="server" Text="Guarantor1" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor1" runat="server" Text='<%# Eval("Guarantor1") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2" runat="server" Text="Guarantor2" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor2" runat="server" Text='<%# Eval("Guarantor2") %>' /></td>                        
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ContactTEL" runat="server" Text="ContactTEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ContactTEL" runat="server" Text='<%# Eval("ContactTEL") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1TEL" runat="server" Text="Guarantor1TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor1TEL" runat="server" Text='<%# Eval("Guarantor1TEL") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2TEL" runat="server" Text="Guarantor2TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor2TEL" runat="server" Text='<%# Eval("Guarantor2TEL") %>' /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LeaveWithoutPay" runat="server" Text="LeaveWithoutPay" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LeaveWithoutPay" runat="server" Text='<%# Eval("LeaveWithoutPay") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ObserveExpirationDate" runat="server" Text="ObserveExpirationDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ObserveExpirationDate" runat="server" Text='<%# Eval("ObserveExpirationDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_CCN" runat="server" Text="CCN" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_CCN" runat="server" Text='<%# Eval("CCN") %>' /></td> 
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LstPromotionDate" runat="server" Text="LstPromotionDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LstPromotionDate" runat="server" Text='<%# Eval("LstPromotionDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LstChangeSalaryDate" runat="server" Text="LstChangeSalaryDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LstChangeSalaryDate" runat="server" Text='<%# Eval("LstChangeSalaryDate") %>' /></td>                                               
                        <td></td>
                        </tr>                                              
                        </table>
                        </asp:Panel>
                        </ItemTemplate>
                        <EditItemTemplate> 
                        <asp:Panel runat="server" Visible="true" ID="MasterEdit">                       
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Company" runat="server" Text="Company" /></td><td class="Grid_GridLine"><uc9:CompanyList ID="txt_CompL_Company" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeId" runat="server" Text="EmployeeId" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EmployeeId" runat="server" Text='<%# Eval("EmployeeId") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DeptId" runat="server" Text="DeptId" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_DeptId" runat="server" /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeName" runat="server" Text="EmployeeName" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EnglishName" runat="server" Text="EnglishName" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EnglishName" runat="server" Text='<%# Eval("EnglishName") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BirthDate" runat="server" Text="BirthDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_BirthDate" runat="server" Text='<%# Eval("BirthDate") %>' />                        
                        </td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Sex" runat="server" Text="Sex" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Sex" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MobilPhone" runat="server" Text="MobilPhone" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_MobilPhone" runat="server" Text='<%# Eval("MobilPhone") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Email" runat="server" Text="Email" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Email" runat="server" Text='<%# Eval("Email") %>' MaxLength="100" /></td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EducationCode" runat="server" Text="學歷" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_EducationCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TitleCode" runat="server" Text="職稱" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_TitleCode" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Rank" runat="server" Text="職級" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Rank" runat="server" /></td>                        
                        </tr>                                             
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignCode" runat="server" Text="是否在職" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_ResignCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_HireDate" runat="server" Text="HireDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_HireDate" runat="server" Text='<%# Eval("HireDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignDate" runat="server" Text="ResignDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ResignDate" runat="server" Text='<%# Eval("ResignDate") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isBoss" runat="server" Text="是否雇主" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_isBoss" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isSales" runat="server" Text="是否業務" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_isSales" runat="server" /></td>      
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ReHireDate" runat="server" Text="復職日期" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ReHireDate" runat="server" Text='<%# Eval("ReHireDate") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PlusYear" runat="server" Text="加計年資" /><asp:Label ID="lblTitle_PlusMonth" runat="server" Text="" /></td>
                        <td class="Grid_GridLine">　<asp:TextBox ID="txt_PlusYear" runat="server" Text='<%# Eval("PlusYear") %>' MaxLength="2" /> 年 <asp:TextBox ID="txt_PlusMonth" runat="server" Text='<%# Eval("PlusMonth") %>' MaxLength="2" /> 月</td>
                        </tr>                                                                                                                                                
                        </table>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible="false" ID="OtherEdit">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IDNo" runat="server" Text="IDNo" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_IDNo" runat="server" Text='<%# Eval("IDNo") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IDType" runat="server" Text="IDType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_IDType" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_BloodType" runat="server" Text="BloodType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_BloodType" runat="server" /></td>
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_MaritalStatus" runat="server" Text="MaritalStatus" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_MaritalStatus" runat="server" /></td>								
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DependentsNum" runat="server" Text="DependentsNum" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_DependentsNum" runat="server" Text='<%# Eval("DependentsNum") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Military" runat="server" Text="Military" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Military" runat="server" /></td>
                        </tr>
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Nationality" runat="server" Text="Nationality" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Nationality" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_BornPlace" runat="server" Text="BornPlace" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_BornPlace" runat="server" Text='<%# Eval("BornPlace") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ResidenceAddr" runat="server" Text="ResidenceAddr" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ResidenceAddr" runat="server" Text='<%# Eval("ResidenceAddr") %>' MaxLength="200" /></td>
						</tr>
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_TEL" runat="server" Text="TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_TEL" runat="server" Text='<%# Eval("TEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Addr" runat="server" Text="Addr" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Addr" runat="server" Text='<%# Eval("Addr") %>' MaxLength="200" /></td>						
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LISubsidy" runat="server" Text="LISubsidy" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_LISubsidy" runat="server" /></td>
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_SpecialSeniority" runat="server" Text="SpecialSeniority" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_SpecialSeniority" runat="server" Text='<%# Eval("SpecialSeniority") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LWC" runat="server" Text="LWC" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_LWC" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Union" runat="server" Text="Union" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Union" runat="server" /></td>
						</tr>						
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Identify" runat="server" Text="編制別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Identify" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Grade" runat="server" Text="職等" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Grade" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Shift" runat="server" Text="班別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Shift" runat="server" /></td>						
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_PayCode" runat="server" Text="PayCode" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_PayCode" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Introducer" runat="server" Text="Introducer" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Introducer" runat="server" Text='<%# Eval("Introducer") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IntroducerTEL" runat="server" Text="IntroducerTEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_IntroducerTEL" runat="server" Text='<%# Eval("IntroducerTEL") %>' /></td>						
						</tr>		
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Contact" runat="server" Text="Contact" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Contact" runat="server" Text='<%# Eval("Contact") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1" runat="server" Text="Guarantor1" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor1" runat="server" Text='<%# Eval("Guarantor1") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2" runat="server" Text="Guarantor2" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor2" runat="server" Text='<%# Eval("Guarantor2") %>' /></td>
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ContactTEL" runat="server" Text="ContactTEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ContactTEL" runat="server" Text='<%# Eval("ContactTEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1TEL" runat="server" Text="Guarantor1TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor1TEL" runat="server" Text='<%# Eval("Guarantor1TEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2TEL" runat="server" Text="Guarantor2TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor2TEL" runat="server" Text='<%# Eval("Guarantor2TEL") %>' /></td>
						</tr>					
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LeaveWithoutPay" runat="server" Text="LeaveWithoutPay" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LeaveWithoutPay" runat="server" Text='<%# Eval("LeaveWithoutPay") %>' /></td>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ObserveExpirationDate" runat="server" Text="ObserveExpirationDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ObserveExpirationDate" runat="server" Text='<%# Eval("ObserveExpirationDate") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_CCN" runat="server" Text="CCN" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_CCN" runat="server" Text='<%# Eval("CCN") %>' /></td>
                        </tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LstPromotionDate" runat="server" Text="LstPromotionDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LstPromotionDate" runat="server" Text='<%# Eval("LstPromotionDate") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LstChangeSalaryDate" runat="server" Text="LstChangeSalaryDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LstChangeSalaryDate" runat="server" Text='<%# Eval("LstChangeSalaryDate") %>' /></td>
						
						</tr>
						</table>                        
                        </asp:Panel>                        
                        <center>
                        <asp:ImageButton ID="btnEdit" runat="server" SkinID="SU1" CommandName="Update" />
                        <asp:ImageButton ID="btnCancelEdit" runat="server" SkinID="CC1" CommandName="Cancel" />
                        </center>
                        </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="false" ButtonType="Image" EditImageUrl="~/App_Themes/images/edit1.GIF" EditText="編輯"
                        UpdateImageUrl="~/App_Themes/images/savegoon1.gif" UpdateText="更新" CancelImageUrl="~/App_Themes/images/cancel1.gif"
                        ItemStyle-HorizontalAlign="Center"
                        />                        
                        </Fields>                        
                    </asp:DetailsView>
                </td>
                <td class="Grid_GridLine">                
                <asp:ImageButton ID="ib_Picture" runat="server" Height="135px" Width="115px" />
                </td>
            </tr>
            <tr runat="server" id="trInsert">
                <td align="center">
                <asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG2" CommandName="Insert" OnClick="btnSave_Click" />
                <asp:ImageButton ID="btnSaveGoNext" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
                <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
                <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
                </td>
            </tr>   
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"               
            SelectCommand="SELECT Personnel_Master.* FROM Personnel_Master WHERE (Company = @Company And EmployeeId = @EmployeeId)">           
            <SelectParameters>
                <asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label>
        <asp:HiddenField ID="hid_InsertMode" runat="server" />
        <asp:HiddenField ID="hid_UplodFileStyle" runat="server" />
        </ContentTemplate>
        </asp:UpdatePanel>        
        <uc5:StyleContentEnd ID="StyleContentEnd0" runat="server" />
        <uc6:StyleFooter ID="StyleFooter0" runat="server" />                     
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <ContentTemplate>        
        <table>
            <tr height="40px" align="center">
                <td id="tdTab1" runat="server" class="BgTab1" width="70px">      
                <asp:LinkButton ID="lbTab1" runat="server" OnClick="lbTab1_Click">背&nbsp;&nbsp;景</asp:LinkButton>
                </td>
                <td id="tdTab7" runat="server" class="BgTab1" width="70px">
                    <asp:LinkButton ID="lbTab7" runat="server" OnClick="lbTab7_Click">健&nbsp;&nbsp;保</asp:LinkButton>
                </td>
                <td id="tdTab2" runat="server" class="BgTab1" width="70px">      
                <asp:LinkButton ID="lbTab2" runat="server" OnClick="lbTab2_Click">學&nbsp;&nbsp;歷</asp:LinkButton>
                </td>
                <td id="tdTab3" runat="server" class="BgTab1" width="70px">
                <asp:LinkButton ID="lbTab3" runat="server" OnClick="lbTab3_Click">經&nbsp;&nbsp;歷</asp:LinkButton>
                </td>
                <td id="tdTab4" runat="server" class="BgTab1" width="70px">
                    <asp:LinkButton ID="lbTab4" runat="server" OnClick="lbTab4_Click">專&nbsp;&nbsp;長</asp:LinkButton>
                </td>
                <td id="tdTab5" runat="server" class="BgTab1" width="70px">
                    <asp:LinkButton ID="lbTab5" runat="server" OnClick="lbTab5_Click">語&nbsp;&nbsp;言</asp:LinkButton>
                </td>       
                <td id="tdTab6" runat="server" class="BgTab1" width="70px">
                    <asp:LinkButton ID="lbTab6" runat="server" OnClick="lbTab6_Click">訓&nbsp;&nbsp;練</asp:LinkButton>
                </td>
                <td id="tdTab8" runat="server" class="BgTab1" width="70px">
                    <asp:LinkButton ID="lbTab8" runat="server" OnClick="lbTab8_Click">控&nbsp;&nbsp;管</asp:LinkButton>
                </td>
                <td style="width:400px">
                <asp:Label id="lbl_TabMsg" runat="server" ForeColor="red"></asp:Label>
                </td>                
            </tr>
            <tr>
                <td colspan="9" style="width:100%">
                    <div id="Tab1" runat="server" style="position: absolute;width: 100%; z-index=21;">
                    <!-- #include file="Include01.cp" -->
                    </div>                
                    <div id="Tab2" runat="server" style="position: absolute;width: 100%; z-index=22;">
                    <!-- #include file="Include02.cp" -->
                    </div>
                    <div id="Tab3" runat="server" style="position: absolute;width: 100%; z-index=23;">
                    <!-- #include file="Include03.cp" -->
                    </div>
                    <div id="Tab4" runat="server" style="position: absolute;width: 100%; z-index=24;">
                    <!-- #include file="Include04.cp" -->
                    </div>      
                    <div id="Tab5" runat="server" style="position: absolute;width: 100%; z-index=25;">
                    <!-- #include file="Include05.cp" -->
                    </div>      
                    <div id="Tab6" runat="server" style="position: absolute;width: 100%; z-index=26;">
                    <!-- #include file="Include06.cp" -->
                    </div>
                    <div id="Tab7" runat="server" style="position: absolute;width: 100%; z-index=27;">
                    <!-- #include file="Include07.cp" -->
                    </div>
                    <div id="Tab8" runat="server" style="position: absolute;width: 100%; z-index=28;">
                    <!-- #include file="Include08.cp" -->
                    </div>
                </td>
            </tr>
        </table>  
        </ContentTemplate>
        </asp:UpdatePanel>      
    </div>
    </form>
</body>
</html>

