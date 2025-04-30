<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GLA0350_M.aspx.cs" Inherits="GLA0350" validaterequest="false" EnableEventValidation="false" %>

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
    <title>����榡�]�w</title>
    <base target="_self" />    
    <script language="javascript" type="text/javascript" src="~/Pages/pagefunction.js"></script>
</head>
<body>
    <form id="form1" runat="server">   
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="����榡�]�w" ShowBackToPre="false" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td align="center">
                    <asp:Label ID="lbl_Msg" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                ���q�O�G</td>
                            <td >
                                <uc8:CompanyList ID="CompanyList1" runat="server" AutoPostBack="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                �������O�G</td>
                            <td >
                                <asp:DropDownList ID="DrpReportType" runat="server">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                ����N���G</td>
                            <td >
                                <asp:TextBox ID="txtRpeortID" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                ����W�١G</td>
                            <td >
                                <asp:TextBox ID="txtReportName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                �榡�G</td>
                            <td >
                                <asp:DropDownList ID="DrpFromat" runat="server">
                                    <asp:ListItem Value="1">����</asp:ListItem>
                                    <asp:ListItem Value="2">�^��</asp:ListItem>
                                </asp:DropDownList></td>
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
        <uc3:StyleTitle id="StyleTitle2" title="����榡�Ӷ��]�w" runat="server"></uc3:StyleTitle>
        <uc4:StyleContentStart id="StyleContentStart2" runat="server"></uc4:StyleContentStart> 
        <table id="table1" cellspacing="0" cellpadding="0" width="100%">
        <TBODY>
      <TR><TD colSpan=2><asp:Label id="lbl_Msg2" runat="server" ForeColor="red"></asp:Label></TD></TR>
            <TR><TD colSpan=2><uc7:Navigator id="Navigator1" runat="server"></uc7:Navigator> </TD></TR>
            <TR><TD colSpan=2><asp:GridView id="GridView1" runat="server" Width="100%" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False" 
            DataKeyNames="Company, ReportType,ReportID,SeqNo,AcctNoStart" GridLines="None" 
            OnRowCreated="GridView1_RowCreated" 
            OnRowEditing="GridView1_RowEditing" 
            OnRowUpdating="GridView1_RowUpdating" 
            OnRowUpdated="GridView1_RowUpdated" 
            OnRowDataBound="GridView1_RowDataBound" 
            DataSourceID="SDS_GridView" ShowFooter="True">

        <RowStyle HorizontalAlign="Center"></RowStyle>
        <Columns>
        <asp:TemplateField HeaderText="�R��" ShowHeader="False">
        <ItemTemplate>
        <asp:LinkButton id="btnDelete"  runat="server" Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='�R��'  style=&quot; filter:alpha(opacity=50);&quot;  onmouseout=&quot;this.filters['alpha'].opacity=50&quot;  onmouseover=&quot;this.filters['alpha'].opacity=100&quot; />" OnClick="btnDelete_Click" OnClientClick='return confirm("�T�w�R��?");' L2PK='<%# DataBinder.Eval(Container, "DataItem.SeqNo")%>'  L3PK='<%# DataBinder.Eval(Container, "DataItem.AcctNoStart")%>' CausesValidation="False"></asp:LinkButton> 
        </ItemTemplate>

        <HeaderStyle Width="30px"></HeaderStyle>
        </asp:TemplateField>
        <asp:CommandField CancelImageUrl="../App_Themes/images/cancel1.gif" EditImageUrl="../App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="../App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="�s��">
        <HeaderStyle Width="60px"></HeaderStyle>
        <ItemStyle Width="60px"></ItemStyle>
        </asp:CommandField>

       
        <asp:BoundField DataField="SeqNo" HeaderText="�Ǹ�" SortExpression="SeqNo"  ReadOnly="true"></asp:BoundField>
         <asp:TemplateField HeaderText="�[�" SortExpression="PMcode">
                <edititemtemplate>
<asp:DropDownList id="DrpeditPMcode" runat="server"><asp:ListItem Value="1">�[</asp:ListItem>
<asp:ListItem Value="-1">��</asp:ListItem>
</asp:DropDownList>
<asp:HiddenField id="Hideditpmcode" runat="server" Value='<%# Bind("PMCode") %>'></asp:HiddenField>
</edititemtemplate>
                <itemtemplate>
<asp:Label runat="server" Text='<%# Bind("PMcode") %>' id="Label1"></asp:Label>
</itemtemplate>
            </asp:TemplateField>       
        
        <asp:BoundField DataField="AcctNoStart" HeaderText="�|�O��ذ_" SortExpression="AcctNoStart" ReadOnly="true" ></asp:BoundField>
        <asp:BoundField DataField="AcctNoEnd" HeaderText="�|�O��ب�" SortExpression="AcctNoEnd" ></asp:BoundField>
         <asp:TemplateField HeaderText="�X�p�X" SortExpression="SunTotCode">
                    <edititemtemplate>
<asp:CheckBox id="chkeditsuntot" runat="server"></asp:CheckBox>
<asp:HiddenField id="Hideditsuntot" runat="server" Value='<%# Bind("SunTotCode") %>'></asp:HiddenField>
</edititemtemplate>
                    <itemtemplate>
<asp:Label runat="server" Text='<%# Bind("SunTotCode") %>' id="Label2"></asp:Label>
</itemtemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="�C�L����" SortExpression="PrtDetailCode">
                    <edititemtemplate>
<asp:CheckBox id="chkeditPrint" runat="server"></asp:CheckBox>
<asp:HiddenField id="HideditPrint" runat="server" Value='<%# Bind("PrtDetailCode") %>'></asp:HiddenField>
</edititemtemplate>
                    <itemtemplate>
<asp:Label runat="server" Text='<%# Bind("PrtDetailCode") %>' id="Label3"></asp:Label>
</itemtemplate>
                </asp:TemplateField>        
        <asp:BoundField DataField="ItemDesc" HeaderText="���ئW��" SortExpression="ItemDesc" ></asp:BoundField>
        <asp:BoundField DataField="ExpenseCode" HeaderText="�����O�ΧO" SortExpression="ExpenseCode" ></asp:BoundField>      
        </Columns>

<FooterStyle HorizontalAlign="Center"></FooterStyle>

<PagerStyle HorizontalAlign="Left"></PagerStyle>

<EmptyDataTemplate>
<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
<tr class="button_bar_cell"><td class="paginationRowEdgeLl">
�s�W</td><td>�Ǹ�</td><td class="paginationRowEdgeRI">�[��</td>
 <td class="paginationRowEdgeRI">
                        �|�O��ذ_</td>
                    <td class="paginationRowEdgeRI">
                        �|�O��ب�</td>
                    <td class="paginationRowEdgeRI">
                        �X�p�X</td>
                    <td class="paginationRowEdgeRI">
                        �C�L����</td>
                    <td class="paginationRowEdgeRI" >
                        ���ئW��</td>
                    <td class="paginationRowEdgeRI" >
                        �����O�ΧO</td>
                      
</tr>
<tr><td class="Grid_GridLine"><asp:ImageButton id="btnNew" onclick="btnEmptyNew_Click" runat="server" SkinID="NewAdd" /></td>
<td class="Grid_GridLine"><asp:TextBox ID="txtaddseq" runat="server"></asp:TextBox></td>

<td class="Grid_GridLine"><asp:DropDownList ID="DrpPMcode" runat="server">
<asp:ListItem Value="1">�[</asp:ListItem>
                                    <asp:ListItem Value="-1">��</asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    <td class="Grid_GridLine">
                       <asp:TextBox ID="txtaddAcctNoFrom" runat="server"></asp:TextBox></td>
                    <td class="Grid_GridLine">
                        <asp:TextBox id="txtaddAcctNoEnd" runat="server">
                        </asp:TextBox></td>
                        <td class="Grid_GridLine">
                        <asp:CheckBox id="chksumcode" runat="server">
                        </asp:CheckBox></td>
                    <td class="Grid_GridLine">
                        <asp:CheckBox id="chkprintcode" runat="server">
                        </asp:CheckBox></td>                    
                    </td>
                    <td class="Grid_GridLine">
                        <asp:TextBox id="txtadditem" runat="server">
                        </asp:TextBox></td>
                        
                        <td class="Grid_GridLine" >
                        <asp:TextBox id="txtexpensecode" runat="server">
                        </asp:TextBox></td>
                        
                    
</tr>
</table>
</EmptyDataTemplate>


<HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>
</asp:GridView> </TD></TR><TR><TD align=left colSpan=2></TD></TR></TBODY></TABLE>

<asp:SqlDataSource id="SDS_GridView" runat="server" ConnectionString="<%$ ConnectionStrings:EBosDB  %>" 
DeleteCommand="DELETE * FROM GLReportDefDetail 
WHERE Company=@company AND ReportType=@ReportType AND
 ReportID=@ReportID AND SeqNo=@SeqNo AND AcctNoStart=@AcctNoStart" 
SelectCommand="SELECT * FROM GLReportDefDetail WHERE Company=@company AND 
ReportType=@ReportType AND ReportID=@ReportID" 
InsertCommand="INSERT INTO GLReportDefDetail 
(Company,ReportType,ReportID,seqNo,PMCode,AcctNoStart,AcctNoEnd,sunTotcode
 ,PrtDetailCode,ItemDesc,ExpenseCode,LstChgUser,LstChgDateTime ) 
VALUES
( @Company,@ReportType,@ReportID,@seqNo,@PMCode,@AcctNoStart,@AcctNoEnd,@sunTotcode
 ,@PrtDetailCode,@ItemDesc,@ExpenseCode,@LstChgUser,@LstChgDateTime )" 
 
UpdateCommand="UPDATE GLReportDefDetail SET PMCode=@PMCode,
AcctNoEnd=@AcctNoEnd,SunTotcode=@SunTotcode,PrtDetailCode=@PrtDetailCode,
ItemDesc=@ItemDesc,ExpenseCode=@ExpenseCode,LstChgUser=@LstChgUser,
LstChgDateTime=@LstChgDateTime
WHERE  Company=@Company AND ReportType=@ReportType AND ReportID=@ReportID
AND seqNo=@seqNo AND AcctNoStart=@AcctNoStart" >
                 <DeleteParameters>          
                <asp:Parameter Name="company" />
                <asp:Parameter Name="ReportType" />
                <asp:Parameter Name="ReportID" />
                <asp:Parameter Name="SeqNo" />
                <asp:Parameter Name="AcctNoStart" />
                 </DeleteParameters>                 
                 <UpdateParameters>
                <asp:Parameter Name="PMCode" />
                <asp:Parameter Name="AcctNoEnd" />
                <asp:Parameter Name="SunTotcode" />                  
                <asp:Parameter Name="PrtDetailCode" /> 
                <asp:Parameter Name="ItemDesc" /> 
                <asp:Parameter Name="ExpenseCode" />
                <asp:Parameter Name="LstChgUser" />
                <asp:Parameter Name="LstChgDateTime" />
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="ReportType" />                 
                <asp:Parameter Name="ReportID" />
                <asp:Parameter Name="seqNo" />                 
                <asp:Parameter Name="AcctNoStart" />
                
                
                                     
                 </UpdateParameters>
            <InsertParameters>            
            
                <asp:Parameter Name="Company" />
                <asp:Parameter Name="ReportType" />
                <asp:Parameter Name="ReportID" />
                <asp:Parameter Name="seqNo" />
                <asp:Parameter Name="PMCode" />
                <asp:Parameter Name="AcctNoStart" />
                <asp:Parameter Name="AcctNoEnd" />
                <asp:Parameter Name="sunTotcode" />
                <asp:Parameter Name="PrtDetailCode" />
                <asp:Parameter Name="ItemDesc" />
                <asp:Parameter Name="ExpenseCode" />
                <asp:Parameter Name="LstChgUser" />
                <asp:Parameter Name="LstChgDateTime" />          
                
            </InsertParameters>
             </asp:SqlDataSource><asp:HiddenField id="hid_IsInsertExit" runat="server">
             </asp:HiddenField> <asp:Label id="Date_lbl" runat="server" Visible="False"></asp:Label><BR />
             <uc5:StyleContentEnd id="StyleContentEnd2" runat="server"></uc5:StyleContentEnd> 
              <uc6:StyleFooter ID="StyleFooter2" runat="server" />
          </div>
       
        
    </form>
</body>
</html>

