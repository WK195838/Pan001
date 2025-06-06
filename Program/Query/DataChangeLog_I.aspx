﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataChangeLog_I.aspx.cs" Inherits="DataChangeLog_I" %>

<%@ Register Src="../UserControl/ShowMsgBox.ascx" TagName="ShowMsgBox" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/StyleHeader.ascx" TagName="StyleHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/StyleTitle.ascx" TagName="StyleTitle" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/StyleContentStart.ascx" TagName="StyleContentStart"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/StyleContentEnd.ascx" TagName="StyleContentEnd"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControl/StyleFooter.ascx" TagName="StyleFooter" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>資料異動紀錄查詢</title>
    <base target="_self" />    
</head>
<body>
    <form id="form1" runat="server">
    <div>
 
        <uc1:ShowMsgBox ID="ShowMsgBox1" runat="server" />
        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="資料異動紀錄查詢" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td>
                    <asp:DetailsView ID="DetailsView1" runat="server" DataKeyNames="TableName,TrxType,ChangItem,ChgUser,ChgStartDateTime"
                        DataSourceID="SqlDataSource1" Width="100%" OnDataBound="DetailsView1_DataBound">                                                                      
                    </asp:DetailsView>
                 </td>
            </tr>
            <tr>
                <td align="center"><asp:ImageButton ID="btnCancel" runat="server" SkinID="CC1" /></td>
            </tr>         
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
            SelectCommand="SELECT DataChangeLog.* FROM DataChangeLog WHERE (LTRIM(RTRIM(TableName)) = @TableName And TrxType = @TrxType And ChangItem = @ChangItem And ChgUser = @ChgUser And Convert(varchar,ChgStartDateTime,120) = @ChgStartDateTime )">
            <SelectParameters>
                <asp:QueryStringParameter Name="TableName" QueryStringField="TableName" />
                <asp:QueryStringParameter Name="TrxType" QueryStringField="TrxType" />
                <asp:QueryStringParameter Name="ChangItem" QueryStringField="ChangItem" />
                <asp:QueryStringParameter Name="ChgUser" QueryStringField="ChgUser" />
                <asp:QueryStringParameter Name="ChgStartDateTime" QueryStringField="ChgStartDateTime" />
            </SelectParameters>
        </asp:SqlDataSource>
        <uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
        <uc6:StyleFooter ID="StyleFooter1" runat="server" />
        
    </div>
    </form>
</body>
</html>

