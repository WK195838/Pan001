<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcctnoPopupDialog.aspx.cs" Inherits="AcctnoPopupDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>彈出式視窗－選擇資料項</title>  
    <meta http-equiv="Expires" content="0 " />
    <meta http-equiv="Cache-Control" content="no-cache " />
    <meta http-equiv="Pragma" content="no-cache " />
    <base target="_self" />
    
    <script language="javascript" type="text/javascript">
    <!--
        function ReValue(ValueString)
        {
            //alert(ValueString);
            window.returnValue = ValueString;
            window.close();
            
            return false;
        }
    //-->
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:TextBox ID="txtQuery" runat="server" Width="130px"></asp:TextBox>&nbsp;
      <asp:ImageButton ID="imgbtnQuery" runat="server" ImageUrl="~/Image/ButtonPics/Query.gif"
        OnClick="imgbtnQuery_Click" />
      <asp:GridView ID="gvPopupDialog" runat="server" AllowPaging="True" CellPadding="4" ForeColor="#333333" 
        DataSourceID="SqlDSPopupDialog" 
        OnRowCreated="gvPopupDialog_RowCreated" 
        OnRowDataBound="gvPopupDialog_RowDataBound" 
        AutoGenerateColumns="False" DataKeyNames="Company,AcctNo" PageSize="15" BackColor="White">
        <HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>  
        <Columns>
          <asp:BoundField DataField="AcctNo" HeaderText="AcctNo" ReadOnly="True" SortExpression="AcctNo" >
            <ItemStyle HorizontalAlign="Left" />
              <HeaderStyle Width="70px" />
          </asp:BoundField>
          <asp:BoundField DataField="AcctDesc1" HeaderText="AcctDesc1" SortExpression="AcctDesc1" >
            <ItemStyle HorizontalAlign="Left" />
              <HeaderStyle Width="200px" />
          </asp:BoundField>
          <asp:BoundField DataField="AcctType" HeaderText="AcctType" SortExpression="AcctType" />
          <asp:BoundField DataField="AcctCtg" HeaderText="AcctCtg" SortExpression="AcctCtg" />
          <asp:BoundField DataField="ASpecialAcct" HeaderText="ASpecialAcct" SortExpression="ASpecialAcct" />
          <asp:BoundField DataField="Idx01" HeaderText="Idx01" SortExpression="Idx01" />
          <asp:BoundField DataField="Idx02" HeaderText="Idx02" SortExpression="Idx02" />
          <asp:BoundField DataField="Idx03" HeaderText="Idx03" SortExpression="Idx03" />
          <asp:BoundField DataField="Idx04" HeaderText="Idx04" SortExpression="Idx04" />
          <asp:BoundField DataField="Idx05" HeaderText="Idx05" SortExpression="Idx05" />
          <asp:BoundField DataField="Idx06" HeaderText="Idx06" SortExpression="Idx06" />
          <asp:BoundField DataField="Idx07" HeaderText="Idx07" SortExpression="Idx07" />
          <asp:BoundField DataField="Inputctl1" HeaderText="Inputctl1" SortExpression="Inputctl1" />
          <asp:BoundField DataField="Inputctl2" HeaderText="Inputctl2" SortExpression="Inputctl2" />
          <asp:BoundField DataField="Inputctl3" HeaderText="Inputctl3" SortExpression="Inputctl3" />
          <asp:BoundField DataField="Inputctl4" HeaderText="Inputctl4" SortExpression="Inputctl4" />
          <asp:BoundField DataField="Inputctl5" HeaderText="Inputctl5" SortExpression="Inputctl5" />
          <asp:BoundField DataField="Inputctl6" HeaderText="Inputctl6" SortExpression="Inputctl6" />
          <asp:BoundField DataField="Inputctl7" HeaderText="Inputctl7" SortExpression="Inputctl7" />
          <asp:BoundField DataField="Idx01Name" HeaderText="Idx01Name" SortExpression="Idx01Name" />
          <asp:BoundField DataField="Idx02Name" HeaderText="Idx02Name" SortExpression="Idx02Name" />
          <asp:BoundField DataField="Idx03Name" HeaderText="Idx03Name" SortExpression="Idx03Name" />
          <asp:BoundField DataField="Idx04Name" HeaderText="Idx04Name" SortExpression="Idx04Name" />
          <asp:BoundField DataField="Idx05Name" HeaderText="Idx05Name" SortExpression="Idx05Name" />
          <asp:BoundField DataField="Idx06Name" HeaderText="Idx06Name" SortExpression="Idx06Name" />
          <asp:BoundField DataField="Idx07Name" HeaderText="Idx07Name" SortExpression="Idx07Name" />
          <asp:BoundField DataField="Idx01YN" HeaderText="Idx01YN" SortExpression="Idx01YN" />
          <asp:BoundField DataField="Idx02YN" HeaderText="Idx02YN" SortExpression="Idx02YN" />
          <asp:BoundField DataField="Idx03YN" HeaderText="Idx03YN" SortExpression="Idx03YN" />
          <asp:BoundField DataField="Idx04YN" HeaderText="Idx04YN" SortExpression="Idx04YN" />
          <asp:BoundField DataField="Idx05YN" HeaderText="Idx05YN" SortExpression="Idx05YN" />
          <asp:BoundField DataField="Idx06YN" HeaderText="Idx06YN" SortExpression="Idx06YN" />
          <asp:BoundField DataField="Idx07YN" HeaderText="Idx07YN" SortExpression="Idx07YN" />
          <asp:BoundField DataField="Company" ReadOnly="True" SortExpression="Company" HeaderText="Company" />
          <asp:BoundField DataField="AcctClass" HeaderText="AcctClass" SortExpression="AcctClass" />
        </Columns>
      </asp:GridView>
    
    </div>
      <asp:SqlDataSource ID="SqlDSPopupDialog" runat="server">
        
      </asp:SqlDataSource>
    </form>
</body>
</html>
