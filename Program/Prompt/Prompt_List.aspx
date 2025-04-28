<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prompt_List.aspx.cs" Inherits="Prompt_List" %>

<%@ Register Src="../UserControl/Navigator_GV.ascx" TagName="Navigator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<head id="Head1" runat="server">
		<title>資料選擇</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<META http-equiv="Expires" content="May 31,1900 13:30:15">
		<base target="_self">
		<script language="javascript">
		
		//  將Enter鍵轉換成Tab鍵
		function document.onkeydown()
		    {
		       if (window.event.srcElement.type == "text" && window.event.keyCode == 13)
		         {
		            window.event.keyCode = 9;
		         }
		    }

		</script>		
	</head>
	<body text="#000000" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="FormPromptWin" method="post" runat="server">
		<div style="vertical-align:middle;">
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />        
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="選擇列表" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table id="table1" cellspacing="0" cellpadding="0" width="100%">
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label></span>&nbsp;</td>
                        <td align="left">
                            <asp:TextBox ID="tbQuery1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="QueryStyle">
                        <td align="left"><span class="ItemFontStyle">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>&nbsp;</td>
                        <td align="left">
                            <asp:TextBox ID="tbQuery2" runat="server"></asp:TextBox>                            
                            <asp:ImageButton id="btnQuery" onclick="btnQuery_Click" runat="server" SkinID="Query1" />                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Label ID="lbl_Msg" runat="server" ForeColor="RED"></asp:Label></td>
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
                             <asp:GridView ID="GridView1" runat="server" Width="100%" 
                             OnRowDataBound="GridView1_RowDataBound" 
                              OnRowCreated="GridView1_RowCreated" 
                              AllowPaging="True"                              
                              >
                             </asp:GridView>
                        </td>
                    </tr>
                </table>
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />        
    </div>
        </form>
	</body>
</HTML>
