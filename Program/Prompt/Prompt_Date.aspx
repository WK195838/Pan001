<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prompt_Date.aspx.cs" Inherits="Prompt_Prompt_Date" %>


<%@ Register Src="../UserControl/YearList.ascx" TagName="YearList" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<head id="Head1" runat="server">
		<title>日期選擇</title>
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
		<form id="FormPrompt_Date" method="post" runat="server">
		<center>		
        <div style="vertical-align:top; left:0px;"> 
            <table style="font-family:Verdana; font-size:8pt; color:Black">
                <tr class="trCalender">
                    <td>
                    <asp:ImageButton ID="ibLastY" runat="server" SkinID="LY" OnClick="ibLastY_Click" AutoPostBack="True"/>
                        <asp:ImageButton ID="ibLastM" runat="server" SkinID="LM" OnClick="ibLastM_Click" AutoPostBack="True"/>
                    </td>
                    <td>
                    <asp:Label ID="lbYear" runat="server" /><uc1:YearList ID="YearList1" runat="server" OnLoad="YearList1_Load" />年                    
            <asp:DropDownList ID="MonthList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="MonthList_SelectedIndexChanged">
            <asp:ListItem>01</asp:ListItem>
            <asp:ListItem>02</asp:ListItem>
            <asp:ListItem>03</asp:ListItem>
            <asp:ListItem>04</asp:ListItem>
            <asp:ListItem>05</asp:ListItem>
            <asp:ListItem>06</asp:ListItem>
            <asp:ListItem>07</asp:ListItem>
            <asp:ListItem>08</asp:ListItem>
            <asp:ListItem>09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>            
            </asp:DropDownList>月                    
                    </td>
                    <td>
                    <asp:ImageButton ID="ibNestM" runat="server" SkinID="NM" OnClick="ibNestM_Click" AutoPostBack="True"/>
                    <asp:ImageButton ID="ibNestY" runat="server" SkinID="NY" OnClick="ibNestY_Click" AutoPostBack="True"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
            <asp:Calendar ID="Calendar1" runat="server" ShowTitle="false" 
             OnSelectionChanged="Calendar1_SelectionChanged" OnInit="Calendar1_Init">
            </asp:Calendar>	                                     
                    <asp:ImageButton ID="ibToday" runat="server" SkinID="GetTodayCal" OnClick="btToday_Click" />
                    <asp:ImageButton ID="ibRetDate" runat="server" SkinID="RetDateEnd" OnClick="btRetDate_Click" />
                    </td>
                </tr>
            </table>
            &nbsp;
        </div>
        </center>
        </form>
	</body>
</HTML>
