<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="PopupDialog.aspx.cs" Inherits="PopupDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>彈出式視窗－選擇資料項</title> 
    
    <meta http-equiv="Expires" content="0 " />
    <meta http-equiv="Cache-Control" content="no-cache " />
    <meta http-equiv="Pragma" content="no-cache " />
    <base target="_self" />
    
    <script type="text/javascript">
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
<body leftmargin="0">
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
      <asp:TextBox ID="txtQuery" runat="server" Width="100px"></asp:TextBox>
      <asp:ImageButton ID="imgbtnQuery" runat="server" ImageUrl="~/Image/ButtonPics/Query.gif"
        OnClick="imgbtnQuery_Click" ToolTip="查詢" />
        <asp:GridView ID="gvEmployee" runat="server" CellPadding="4"  
          AllowPaging="True"
          AutoGenerateColumns="False" PageSize="15"
          OnRowCreated="gvEmployee_RowCreated" 
          OnRowDataBound="gvEmployee_RowDataBound" 
          DataSourceID="SqlDataSource1" EmptyDataText="無任何資料!!!" BackColor="White">
          <HeaderStyle HorizontalAlign="Center" CssClass="button_bar_cell"></HeaderStyle>       
          <Columns>
            <asp:BoundField DataField="Code" HeaderText="代號" SortExpression="Code" >
<HeaderStyle Width="20%"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="名稱" SortExpression="Name" >
<HeaderStyle Width="300px"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
          </Columns>
        </asp:GridView>      
    </div>
    </form>
</body>
</html>
