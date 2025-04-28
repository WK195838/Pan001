<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="PersonnelSalaryParameter.aspx.cs" Inherits="PersonnelSalaryParameter" %>

<%@ Register Src="../UserControl/CompanyList.ascx" TagName="CompanyList" TagPrefix="uc2" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/CodeList.ascx" TagName="CodeList" TagPrefix="uc8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
<div>
 
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc3:StyleTitle ID="StyleTitle0" runat="server" Title="人事薪資參數設定" />
        <uc4:StyleContentStart ID="StyleContentStart0" runat="server" />
        <asp:ScriptManager id="ScriptManager0" runat="server">
        </asp:ScriptManager>  
        <asp:UpdatePanel id="UpdatePanel0" runat="server">
        <ContentTemplate>
<TABLE width="100%"><TBODY><TR><TD class="Grid_GridLine">公司：<uc2:CompanyList id="CompanyList1" runat="server" AutoPostBack="true"></uc2:CompanyList></TD></TR><TR><TD class="Grid_GridLine">
<asp:DetailsView id="DetailsView1" runat="server" Width="100%" AutoGenerateRows="False" 
OnItemCommand="DetailsView1_ItemCommand" OnItemInserting="DetailsView1_ItemInserting" OnItemInserted="DetailsView1_ItemInserted" 
OnItemUpdated="DetailsView1_ItemUpdated" OnItemUpdating="DetailsView1_ItemUpdating" OnDataBound="DetailsView1_DataBound" 
DataKeyNames="Category,Company">
                        <Fields>
                        <asp:TemplateField>
                        <ItemTemplate>
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine" rowspan="9">加班參數</td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para1" runat="server" Text="OT_Para1" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para1" runat="server" Text='<%# Eval("OT_Para1") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para2" runat="server" Text="OT_Para2" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para2" runat="server" Text='<%# Eval("OT_Para2") %>' /></td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para3" runat="server" Text="OT_Para3" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para3" runat="server" Text='<%# Eval("OT_Para3") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para4" runat="server" Text="OT_Para4" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para4" runat="server" Text='<%# Eval("OT_Para4") %>' /></td>                        
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_1_4" runat="server" Text="OT_Para5_1_4" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para5_1_4" runat="server" Text='<%# Eval("OT_Para5_1_4") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_1_4" runat="server" Text="OT_Para31_1_4" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para31_1_4" runat="server" Text='<%# Eval("OT_Para31_1_4") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_4_8" runat="server" Text="OT_Para5_4_8" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para5_4_8" runat="server" Text='<%# Eval("OT_Para5_4_8") %>' /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_4_8" runat="server" Text="OT_Para31_4_8" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para31_4_8" runat="server" Text='<%# Eval("OT_Para31_4_8") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_8_12" runat="server" Text="OT_Para5_8_12" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para5_8_12" runat="server" Text='<%# Eval("OT_Para5_8_12") %>' /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_8_12" runat="server" Text="OT_Para31_8_12" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para31_8_12" runat="server" Text='<%# Eval("OT_Para31_8_12") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_1_4" runat="server" Text="OT_Para30_1_4" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para30_1_4" runat="server" Text='<%# Eval("OT_Para30_1_4") %>' /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para6" runat="server" Text="OT_Para6" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para6" runat="server" Text='<%# Eval("OT_Para6") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_4_8" runat="server" Text="OT_Para30_4_8" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para30_4_8" runat="server" Text='<%# Eval("OT_Para30_4_8") %>' /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para7" runat="server" Text="OT_Para7" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para7" runat="server" Text='<%# Eval("OT_Para7") %>' /></td>
                        </tr>   
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_8_10" runat="server" Text="OT_Para30_8_10" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para30_8_10" runat="server" Text='<%# Eval("OT_Para30_8_10") %>' /></td>                                                
                        <td></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_10_12" runat="server" Text="OT_Para30_10_12" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_OT_Para30_10_12" runat="server" Text='<%# Eval("OT_Para30_10_12") %>' /></td>                                                
                        <td></td>
                        </tr>  
                        <tr>
                        <td class="Grid_GridLine" rowspan="4">計薪參數</td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para1" runat="server" Text="PY_Para1" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para1" runat="server" Text='<%# Eval("PY_Para1") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_SalaryPoint" runat="server" Text="SalaryPoint" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_SalaryPoint" runat="server" Text='<%# Eval("SalaryPoint") %>' /></td>
                        </tr>    
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para6" runat="server" Text="PY_Para6" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para6" runat="server" Text='0.05' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para8" runat="server" Text="PY_Para8" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para8" runat="server" Text='0' /></td>
                        </tr>           
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para7" runat="server" Text="PY_Para7" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para7" runat="server" Text='0' /></td>
                        <td class="Grid_GridLine"></td>
                        </tr>                      
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para2" runat="server" Text="PY_Para2" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para2" runat="server" Text='<%# Eval("PY_Para2") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para3" runat="server" Text="PY_Para3" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para3" runat="server" Text='0' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para4" runat="server" Text="PY_Para4" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para4" runat="server" Text='0' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para5" runat="server" Text="PY_Para5" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PY_Para5" runat="server" Text='<%# Eval("PY_Para5") %>' /></td>
                        <td></td>
                        </tr>                  
                        <tr>   
                        <td class="Grid_GridLine" rowspan="2">薪資轉存</td>                     
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DepositBank" runat="server" Text="DepositBank" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_DepositBank" runat="server" Text='<%# Eval("DepositBank") %>' /></td>
                        <td></td>
                        </tr>  
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PYBankBranch" runat="server" Text="PYBankBranch" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PYBankBranch" runat="server" Text='<%# Eval("PYBankBranch") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PYBankAccount" runat="server" Text="PYBankAccount" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_PYBankAccount" runat="server" Text='<%# Eval("PYBankAccount") %>' /></td>
                        </tr>                         
                        </table>
                        </ItemTemplate>
                        <EditItemTemplate> 
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine" rowspan="9">加班參數</td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para1" runat="server" Text="OT_Para1" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para1" runat="server" Text='<%# Eval("OT_Para1") %>' MaxLength="3" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para2" runat="server" Text="OT_Para2" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para2" runat="server" Text='<%# Eval("OT_Para2") %>' MaxLength="3" /></td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para3" runat="server" Text="OT_Para3" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para3" runat="server" Text='<%# Eval("OT_Para3") %>' MaxLength="4" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para4" runat="server" Text="OT_Para4" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para4" runat="server" Text='<%# Eval("OT_Para4") %>' MaxLength="4" /></td>                        
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_1_4" runat="server" Text="OT_Para5_1_4" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para5_1_4" runat="server" Text='<%# Eval("OT_Para5_1_4") %>' MaxLength="4" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_1_4" runat="server" Text="OT_Para31_1_4" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para31_1_4" runat="server" Text="1" MaxLength="4" style="display:none;" /><asp:Label ID="lbl_OT_Para31_1_4" runat="server" Text="日薪"/></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_4_8" runat="server" Text="OT_Para5_4_8" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para5_4_8" runat="server" Text='<%# Eval("OT_Para5_4_8") %>' MaxLength="4" /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_4_8" runat="server" Text="OT_Para31_4_8" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para31_4_8" runat="server" Text="1" MaxLength="4" style="display:none;"/><asp:Label ID="lbl_OT_Para31_4_8" runat="server" Text="日薪"/></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para5_8_12" runat="server" Text="OT_Para5_8_12" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para5_8_12" runat="server" Text='<%# Eval("OT_Para5_8_12") %>' MaxLength="4" /></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para31_8_12" runat="server" Text="OT_Para31_8_12" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para31_8_12" runat="server" Text='<%# Eval("OT_Para31_8_12") %>' MaxLength="4" /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_1_4" runat="server" Text="OT_Para30_1_4" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para30_1_4" runat="server" Text="1" MaxLength="4" style="display:none;"/><asp:Label ID="lbl_OT_Para30_1_4" runat="server" Text="日薪"/></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para6" runat="server" Text="OT_Para6" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para6" runat="server" Text='<%# Eval("OT_Para6") %>' MaxLength="4" /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_4_8" runat="server" Text="OT_Para30_4_8" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para30_4_8" runat="server" Text="1" MaxLength="4" style="display:none;"/><asp:Label ID="lbl_OT_Para30_4_8" runat="server" Text="日薪"/></td>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para7" runat="server" Text="OT_Para7" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para7" runat="server" Text='<%# Eval("OT_Para7") %>' MaxLength="4" /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_8_10" runat="server" Text="OT_Para30_8_10" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para30_8_10" runat="server" Text='<%# Eval("OT_Para30_8_10") %>' MaxLength="4" /></td>                                                
                        <td></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_OT_Para30_10_12" runat="server" Text="OT_Para30_10_12" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_OT_Para30_10_12" runat="server" Text='<%# Eval("OT_Para30_10_12") %>' MaxLength="4" /></td>                                                
                        <td></td>
                        </tr> 
                        <tr>    
                        <td class="Grid_GridLine" rowspan="5">計薪參數</td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para1" runat="server" Text="PY_Para1" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para1" runat="server" Text='<%# Eval("PY_Para1") %>' MaxLength="5" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_SalaryPoint" runat="server" Text="SalaryPoint" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_SalaryPoint" runat="server" Text='<%# Eval("SalaryPoint") %>' MaxLength="5" /></td>
                        </tr>   
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para6" runat="server" Text="PY_Para6" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para6" runat="server" Text='<%# Eval("PY_Para6") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para8" runat="server" Text="PY_Para8" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para8" runat="server" Text='<%# Eval("PY_Para8") %>' /></td>
                        </tr>           
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para7" runat="server" Text="PY_Para7" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para7" runat="server" Text='<%# Eval("PY_Para7") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para9" runat="server" Text="PY_Para9" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para9" runat="server" Text='<%# Eval("PY_Para9") %>' /></td>
                        </tr>                                 
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para2" runat="server" Text="PY_Para2" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para2" runat="server" Text='<%# Eval("PY_Para2") %>' MaxLength="5" /></td>   
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para3" runat="server" Text="PY_Para3" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para3" runat="server" Text='<%# Eval("PY_Para3") %>' MaxLength="5" /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para4" runat="server" Text="PY_Para4" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para4" runat="server" Text='<%# Eval("PY_Para4") %>' MaxLength="4" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PY_Para5" runat="server" Text="PY_Para5" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PY_Para5" runat="server" Text='<%# Eval("PY_Para5") %>' MaxLength="4" /></td>
                        <td></td>
                        </tr>        
                        <tr>   
                        <td class="Grid_GridLine" rowspan="2">薪資轉存</td>                     
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DepositBank" runat="server" Text="DepositBank" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_DepositBank" runat="server" Text='<%# Eval("DepositBank") %>' MaxLength="3" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BankCustId" runat="server" Text="BankCustId" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_BankCustId" runat="server" Text='<%# Eval("BankCustId") %>' MaxLength="20" /></td>
                        <td></td>
                        </tr>  
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PYBankBranch" runat="server" Text="PYBankBranch" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PYBankBranch" runat="server" Text='<%# Eval("PYBankBranch") %>' MaxLength="4" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PYBankAccount" runat="server" Text="PYBankAccount" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_PYBankAccount" runat="server" Text='<%# Eval("PYBankAccount") %>' MaxLength="14" /></td>
                        </tr>                         
                        </table>                     
                        </EditItemTemplate>
                        </asp:TemplateField>                      
                        </Fields>                                                
                    </asp:DetailsView> </TD></TR><TR id="trInsert" runat="server"><TD align=center>
                    <asp:ImageButton id="btnSaveGo" onclick="btnSave_Click" runat="server" SkinID="SG2" CommandName="Insert"></asp:ImageButton>
                    <asp:ImageButton id="btnEdit" onclick="btnSave_Click" runat="server" SkinID="SU1" CommandName="Update"></asp:ImageButton> 
                    <asp:ImageButton id="btnCancel" runat="server" SkinID="CC1"></asp:ImageButton> </TD></TR></TBODY></TABLE>
                    <asp:SqlDataSource id="SqlDataSource1" runat="server" 
                    UpdateCommand="UPDATE PersonnelSalary_Parameter SET                             
                    OT_Para1=@OT_Para1,OT_Para2=@OT_Para2,OT_Para3=@OT_Para3,OT_Para4=@OT_Para4,OT_Para5_1_4=@OT_Para5_1_4,OT_Para5_4_8=@OT_Para5_4_8,OT_Para5_4_8=@OT_Para5_8_12
                    ,OT_Para31_1_4=@OT_Para31_1_4,OT_Para31_4_8=@OT_Para31_4_8,OT_Para31_8_12=@OT_Para31_8_12,OT_Para30_1_4=@OT_Para30_1_4,OT_Para30_4_8=@OT_Para30_4_8,OT_Para30_8_10=@OT_Para30_8_10
                    ,OT_Para30_10_12=@OT_Para30_10_12,OT_Para6=@OT_Para6,OT_Para7=@OT_Para7
                    ,PY_Para1=@PY_Para1,PY_Para2=@PY_Para2,PY_Para3=@PY_Para3,PY_Para4=@PY_Para4,PY_Para5=@PY_Para5,PY_Para6=@PY_Para6,PY_Para7=@PY_Para7                    
                    ,DepositBank=@DepositBank,PYBankBranch=@PYBankBranch,PYBankAccount=@PYBankAccount
                    WHERE (Category=@Category And Company = @Company)" 
                    SelectCommand="SELECT PersonnelSalary_Parameter.* FROM PersonnelSalary_Parameter WHERE (Category=@Category And Company = @Company) " 
                    InsertCommand="INSERT INTO [PersonnelSalary_Parameter]           ([Category]           ,[Company]           ,[OT_Para1]           ,[OT_Para2]           ,[OT_Para3]           ,[OT_Para4]           ,[OT_Para5_1_4]           ,[OT_Para5_4_8]           ,[OT_Para5_8_12]           ,[OT_Para31_1_4]           ,[OT_Para31_4_8]           ,[OT_Para31_8_12]           ,[OT_Para30_1_4]           ,[OT_Para30_4_8]           ,[OT_Para31_8_10]           ,[OT_Para31_10_12]           ,[OT_Para6]           ,[OT_Para7]           ,[PY_Para1]           ,[PY_Para2]           ,[PY_Para3]           ,[PY_Para4]           ,[PY_Para5]           ,[PY_Para6]           ,[PY_Para7]           ,[AL_Para1]           ,[AL_Para2]           ,[AL_Para3]           ,[AL_Para4]           ,[AL_Para5]           ,[AL_Para6]           ,[DepositBank]           ,[PYBankBranch]           ,[PYBankAccount])     VALUES           (@Category,@Company,@OT_Para1,@OT_Para2,@OT_Para3,@OT_Para4,@OT_Para5_1_4,@OT_Para5_4_8,@OT_Para5_8_12,@OT_Para31_1_4,@OT_Para31_4_8,@OT_Para31_8_12,@OT_Para30_1_4,@OT_Para30_4_8,@OT_Para31_8_10,@OT_Para31_10_12,@OT_Para6,@OT_Para7,@PY_Para1,@PY_Para2,@PY_Para3,@PY_Para4,@PY_Para5,@PY_Para6,@PY_Para7           ,@AL_Para1,@AL_Para2,@AL_Para3,@AL_Para4,@AL_Para5,@AL_Para6,@DepositBank,@PYBankBranch,@PYBankAccount)" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>">            
            <UpdateParameters>
                <asp:Parameter Name="Category" />
                <asp:Parameter Name="Company" />                
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="Category" />
                <asp:Parameter Name="Company" />
            </SelectParameters>
        </asp:SqlDataSource> <asp:Label id="lbl_Msg" runat="server" ForeColor="red"></asp:Label> <asp:HiddenField id="hid_InsertMode" runat="server"></asp:HiddenField> <asp:HiddenField id="hid_UplodFileStyle" runat="server"></asp:HiddenField> 
</ContentTemplate>
        </asp:UpdatePanel>        
        <uc5:StyleContentEnd ID="StyleContentEnd0" runat="server" />               
    </div>
</asp:Content>