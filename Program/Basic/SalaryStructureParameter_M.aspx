<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalaryStructureParameter_M.aspx.cs" Inherits="SalaryStructureParameter_M" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>薪資結構參數修改</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="../Pages/pagefunction.js"></script>
    <script language="javascript" type="text/javascript">
	     function GetPromptWin3(objValue1,dHeight,dWidth,theValue1,theValue2,theValue3,theValue4)
		{ 
			var temp_data1,temp_data2; 
			var index;
			var RealURL;
			var index2;				
			
			RealURL="../Prompt/Prompt_List.aspx?theTable=" + encodeURI(theValue1) + "&theRetColum=" + encodeURI(theValue2) + "&theShowColums=" + encodeURI(theValue3) + "&theOrderColums=" + encodeURI(theValue4)+ "&theValue=" + encodeURI(objValue1.value) + "";
		    ReturnValue = window.showModalDialog(RealURL, "", "dialogHeight: " + dHeight + "px; dialogWidth: " + dWidth + "px; dialogTop: px; dialogLeft: px; edge: Sunken; center: Yes; help: No; resizable: yes; status: No; scroll: auto;");
		    if (ReturnValue != null)
               {               
                  index = ReturnValue.indexOf(':');
  			      if (index>-1) 
			         {
			  	 	    temp_data1 =ReturnValue.substring(0, index);
					    temp_data2 =ReturnValue.substring(index+1, ReturnValue.length);
				        if(temp_data2.search(temp_data1)==-1 )
				        {
			 		        objValue1.value += "$"+temp_data1+",";
			 		        session(temp_data2)="";
			 		    }
			 		    else
			 		    {
			 		        objValue1.value += "";
			 		    }
			         }
			      else
				     {
			 		    objValue.value = ReturnValue;
				     }				
                 }
           return false;
	    }</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       
        <uc2:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc1:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="薪資結構參數修改" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <br />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" Width="100%" DataSourceID="SqlDataSource1" DataKeyNames="SalaryId" AutoGenerateRows="False"
                     OnItemInserting="DetailsView1_ItemInserting" OnItemInserted="DetailsView1_ItemInserted" 
                     OnItemCreated="DetailsView1_ItemCreated" OnDataBound="DetailsView1_DataBound"
                     OnItemUpdating="DetailsView1_ItemUpdating" OnItemUpdated="DetailsView1_ItemUpdated">
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditRowStyle HorizontalAlign="Left" />
                        <Fields>                        
                            <asp:BoundField DataField="SalaryId" HeaderText="SalaryId" SortExpression="SalaryId" ReadOnly="true" />
                            <asp:BoundField DataField="SalaryName" HeaderText="SalaryName" SortExpression="SalaryName" />
                            <asp:BoundField DataField="NWTax" HeaderText="NWTax" SortExpression="NWTax" />
                            <asp:BoundField DataField="PMType" HeaderText="PMType" SortExpression="PMType" />
                            <asp:BoundField DataField="ItemType" HeaderText="ItemType" SortExpression="ItemType" />
                            <asp:BoundField DataField="SalaryType" HeaderText="SalaryType" SortExpression="SalaryType" />
                            <asp:BoundField DataField="FixedAmount" HeaderText="FixedAmount" SortExpression="FixedAmount" />
                            <asp:BoundField DataField="SalaryRate" HeaderText="SalaryRate" SortExpression="SalaryRate" />
                            <asp:BoundField DataField="BaseItem" HeaderText="BaseItem" SortExpression="BaseItem" />
                            <asp:BoundField DataField="Properties" HeaderText="Properties" SortExpression="Properties" />
                            <asp:BoundField DataField="SalaryMasterList" HeaderText="SalaryMasterList" SortExpression="SalaryMasterList" />
                            <asp:BoundField DataField="Payroll" HeaderText="Payroll" SortExpression="Payroll" />
                            <asp:BoundField DataField="AcctNo" HeaderText="AcctNo" SortExpression="AcctNo" />
                            <asp:BoundField DataField="P1SalaryMasterList" HeaderText="P1SalaryMasterList" SortExpression="P1SalaryMasterList" />
                            <asp:BoundField DataField="P1Payroll" HeaderText="P1Payroll" SortExpression="P1Payroll" />
                            <asp:BoundField DataField="P1CostSalaryItem" HeaderText="P1CostSalaryItem"
                                SortExpression="P1CostSalaryItem" />
                            <asp:BoundField DataField="RegularPay" HeaderText="RegularPay"
                                SortExpression="RegularPay" />
                            <asp:BoundField DataField="FunctionId" HeaderText="FunctionId"
                                SortExpression="FunctionId" />
                            <asp:BoundField DataField="ParameterList" HeaderText="ParameterList"
                                SortExpression="ParameterList" />    
                        </Fields>
                    </asp:DetailsView>
                </td>
            </tr>
            <tr><td>
                    <asp:ImageButton ID="btnSaveGo" runat="server" SkinID="SG1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Update" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
            </td></tr>
        </table>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>"
            SelectCommand="SELECT SalaryStructure_Parameter.* FROM SalaryStructure_Parameter WHERE (SalaryId = @SalaryId)"
            InsertCommand="INSERT INTO SalaryStructure_Parameter([SalaryId]
           ,[SalaryName]
           ,[NWTax]
           ,[PMType]
           ,[ItemType]
           ,[SalaryType]
           ,[FixedAmount]
           ,[SalaryRate]
           ,[BaseItem]
           ,[Properties]
           ,[SalaryMasterList]
           ,[Payroll]
           ,[AcctNo]
           ,[P1SalaryMasterList]
           ,[P1Payroll]
           ,[P1CostSalaryItem],[RegularPay],[FunctionId],[ParameterList]) VALUES (@SalaryId, @SalaryName, @NWTax, @PMType, @ItemType, @SalaryType, @FixedAmount, @SalaryRate, @BaseItem, @Properties, @SalaryMasterList, @Payroll, @AcctNo, @P1SalaryMasterList, @P1Payroll, @P1CostSalaryItem, @RegularPay, @FunctionId, @ParameterList)"
            UpdateCommand="UPDATE SalaryStructure_Parameter SET SalaryName = @SalaryName, NWTax = @NWTax, PMType = @PMType, ItemType = @ItemType, SalaryType = @SalaryType, FixedAmount = @FixedAmount,
             SalaryRate = @SalaryRate, BaseItem = @BaseItem, Properties = @Properties, SalaryMasterList = @SalaryMasterList, Payroll = @Payroll, AcctNo = @AcctNo, P1SalaryMasterList = @P1SalaryMasterList, P1Payroll = @P1Payroll, P1CostSalaryItem = @P1CostSalaryItem, RegularPay=@RegularPay, FunctionId = @FunctionId, ParameterList = @ParameterList WHERE (SalaryId = @SalaryId)">
             <InsertParameters>
                <asp:Parameter Name="SalaryId" />
                <asp:Parameter Name="SalaryName" />
                <asp:Parameter Name="NWTax" />
                <asp:Parameter Name="PMType" />
                <asp:Parameter Name="ItemType" />
                <asp:Parameter Name="SalaryType" />
                <asp:Parameter Name="FixedAmount" />
                <asp:Parameter Name="SalaryRate" />
                <asp:Parameter Name="BaseItem" />
                <asp:Parameter Name="Properties" />
                <asp:Parameter Name="SalaryMasterList" />
                <asp:Parameter Name="Payroll" />
                <asp:Parameter Name="AcctNo" />
                <asp:Parameter Name="P1SalaryMasterList" />
                <asp:Parameter Name="P1Payroll" />
                <asp:Parameter Name="P1CostSalaryItem" />
                <asp:Parameter Name="RegularPay" /> 
                <asp:Parameter Name="FunctionId" />
                <asp:Parameter Name="ParameterList" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="SalaryId" />
                <asp:Parameter Name="SalaryName" />
                <asp:Parameter Name="NWTax" />
                <asp:Parameter Name="PMType" />
                <asp:Parameter Name="ItemType" />
                <asp:Parameter Name="SalaryType" />
                <asp:Parameter Name="FixedAmount" />
                <asp:Parameter Name="SalaryRate" />
                <asp:Parameter Name="BaseItem" />
                <asp:Parameter Name="Properties" />
                <asp:Parameter Name="SalaryMasterList" />
                <asp:Parameter Name="Payroll" />
                <asp:Parameter Name="AcctNo" />
                <asp:Parameter Name="P1SalaryMasterList" />
                <asp:Parameter Name="P1Payroll" />
                <asp:Parameter Name="P1CostSalaryItem" />
                <asp:Parameter Name="RegularPay" />
                <asp:Parameter Name="FunctionId" />
                <asp:Parameter Name="ParameterList" />
                
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="SalaryId" QueryStringField="SalaryId" />
           <%--     <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />--%>
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hid_SalaryId" runat="server" />      
        <asp:HiddenField ID="hid_InserMode" runat="server" />  
     <%--   <asp:HiddenField ID="hid_EmployeeId" runat="server" />--%>
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
    </div>
    </form>
</body>
</html>

