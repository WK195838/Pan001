<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0380_M.aspx.cs" Inherits="GLA0380_M" validaterequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc8" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<%@ Register Src="~/UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>
<%@ Register Src="~/UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>成本中心結構設定</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">   
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="成本中心結構設定" ShowBackToPre="false" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />

         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
          
        <table width="100%">
           
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td align="left">
                    <table width=50%>
                        <tr>
                            <td>
                                公司別：</td>
                            <td >
                                <uc8:CompanyList ID="CompanyList1" runat="server" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                主要部門：</td>
                            <td >
                                <asp:DropDownList ID="DrpHDepartment" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                備註：</td>
                            <td >
                                <asp:TextBox ID="txtRemark" runat="server" Width="95%"></asp:TextBox></td>
                        </tr>
                        </table>
                </td>
            </tr>
            <tr><td>
                    <asp:ImageButton ID="btnMedit" runat="server" CommandName="Edit" OnClick="btnMedit_Click" ImageUrl="~/App_Themes/images/edit1.GIF" />
                <asp:ImageButton ID="btnSaveExit" runat="server" SkinID="SE1" CommandName="Insert" OnClick="btnSave_Click" />
                    <asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" />
            </td></tr>
        </table>
        
        
       
       
        
        <asp:HiddenField ID="hid_Company" runat="server" />
        <asp:HiddenField ID="hid_EmployeeId" runat="server" />
        <asp:HiddenField ID="hid_DepositBank" runat="server" />
        <asp:HiddenField ID="hid_InserMode" runat="server" />
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
        <div id="listview" runat="server">
        <uc2:StyleHeader ID="StyleHeader2" runat="server" />
        <uc3:StyleTitle id="StyleTitle2" title="成本中心結構細項維護" runat="server" ShowBackToPre="false"></uc3:StyleTitle>
        <uc4:StyleContentStart id="StyleContentStart2" runat="server"></uc4:StyleContentStart> 
        <table id="table1" cellspacing="0" cellpadding="0" width="100%">
        <TBODY>
      <TR><TD colSpan=2><asp:Label id="lbl_Msg2" runat="server" ForeColor="red"></asp:Label></TD></TR>
            <TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
            <TR><TD colSpan=2><asp:GridView id="GridView1" runat="server" Width="100%" 
                    AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" 
            DataKeyNames="Company,HDepartment,DDepartment" GridLines="None" 
            OnRowCreated="GridView1_RowCreated"           
            OnRowDataBound="GridView1_RowDataBound" 
            DataSourceID="SDS_GridView" ShowFooter="True">

        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
        <asp:TemplateField HeaderText="刪除" ShowHeader="False" ControlStyle-Width="30px">
        <ItemTemplate>
        <asp:LinkButton id="btnDelete"  runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClick="btnDelete_Click" OnClientClick='return confirm("確定刪除?");' L2PK='<%# DataBinder.Eval(Container, "DataItem.DDepartment")%>' CausesValidation="False"></asp:LinkButton> 
        </ItemTemplate>

<ControlStyle Width="30px"></ControlStyle>

        <HeaderStyle Width="30px"></HeaderStyle>
        </asp:TemplateField>       
        <asp:BoundField DataField="Company" HeaderText="群組公司別" SortExpression="Company"  
                ReadOnly="true"></asp:BoundField>
        
        <asp:BoundField DataField="HDepartment" HeaderText="群組成本中心" 
                SortExpression="HDepartment" ReadOnly="true" ></asp:BoundField>
        <asp:BoundField DataField="DDepartment" HeaderText="明細成本中心" 
                SortExpression="DDepartment" ></asp:BoundField>
        </Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>

<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">
新增</td><td>群組公司別</td>
 <td class="paginationRowEdgeRI">
                        群組成本中心</td>
                    <td class="paginationRowEdgeRI" >
                        明細成本中心</td>
                      
</tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine">
    <asp:Label ID="LabCompany" runat="server"></asp:Label>
    </td>

                    <td class="Grid_GridLine">
                        <asp:Label ID="Hdepartment" runat="server"></asp:Label>
    </td>
                    </td>
                        
                        <td class="Grid_GridLine" >
                            <asp:DropDownList ID="DrpDepAdd" runat="server">
                            </asp:DropDownList>
    </td>
                        
                    
</tr>
</table>
</EmptyDataTemplate>


<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>

<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB  %>" 
DeleteCommand="DELETE FROM  GLDeptSturDetail WHERE Company=@Company AND  HDepartment=@HDepartment
AND DDepartment=@DDepartment"
 
SelectCommand="SELECT * FROM  GLDeptSturDetail WHERE Company=@Company AND  HDepartment=@HDepartment"

 
InsertCommand="INSERT INTO  GLDeptSturDetail (Company,HDepartment,DDepartment,LstChgUser,LstChgDateTime)
VALUES (@Company,@HDepartment,@DDepartment,@LstChgUser,@LstChgDateTime)" 
 
UpdateCommand="" >
                 <DeleteParameters>          
                <asp:Parameter Name="company" />
                <asp:Parameter Name="HDepartment" />
                <asp:Parameter Name="DDepartment" />             
                 </DeleteParameters>                
              
            <InsertParameters>           
            
                <asp:Parameter Name="company" />
                <asp:Parameter Name="HDepartment" />
                <asp:Parameter Name="DDepartment" />   
                <asp:Parameter Name="LstChgUser" />
                <asp:Parameter Name="LstChgDateTime" />          
                
            </InsertParameters>
             </asp:SqlDataSource><asp:HiddenField id="hid_IsInsertExit" runat="server">
             </asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
             <uc5:StyleContentEnd id="StyleContentEnd2" runat="server"></uc5:StyleContentEnd> 
              <uc6:StyleFooter ID="StyleFooter2" runat="server" />
          </div>
           </ContentTemplate>
          
             <triggers>
                 <asp:AsyncPostBackTrigger ControlID="btnSaveExit" EventName="Click" />
                 <asp:AsyncPostBackTrigger ControlID="btnSaveExit" EventName="Click" />
             </triggers>
          
          </asp:UpdatePanel>
       
        
    </form>
</body>
</html>

