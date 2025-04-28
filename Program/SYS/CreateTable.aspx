<%@ Page Language="C#" MasterPageFile="~/EEOC.master" AutoEventWireup="true" CodeFile="CreateTable.aspx.cs" Inherits="CreatTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphEEOC" Runat="Server">
    <table style=" margin: 0 auto; width: 700px;">
        <tr>
            <td style="height: 418px; text-align: center; width: 236px;">
                <span style="font-size: 10pt; font-family: 新細明體">CSV &nbsp;檔案上傳<br />
                </span><asp:FileUpload ID="FileUpload1" runat="server" Height="24px" Width="221px" BorderStyle="None" EnableTheming="True" />
                <br />
                <br />
                <span style="font-size: 10pt; font-family: 新細明體">
                伺服器位置：</span><asp:TextBox ID="TB_Server" runat="server" Font-Size="Small" Height="20px" Width="140px"></asp:TextBox><br />
                <span style="font-size: 10pt; font-family: 新細明體">
                <br />
                資料庫名稱：</span><asp:TextBox ID="TB_DataBase" runat="server" Font-Size="Small" Height="20px" Width="140px"></asp:TextBox><br />
                <br />
                <span style="font-size: 10pt; font-family: 新細明體">
                登 入 帳 號 ：</span><asp:TextBox ID="TB_Name" runat="server" Font-Size="Small" Height="20px" Width="140px"></asp:TextBox><br />
                <br />
                <span style="font-size: 10pt; font-family: 新細明體">
                登 入 密 碼 ：</span><asp:TextBox ID="TB_Password" runat="server" Width="140px" Font-Size="Small" Height="20px"></asp:TextBox>&nbsp;<br />
                <br />
                <asp:CheckBox ID="CB_Cover" runat="server" Font-Names="新細明體"
                    Font-Size="Small" Height="27px" Text=" 資料表存在時重建" ToolTip="勾選時遇到已存在的資料表時刪除新建" Width="221px" /><br />
                <asp:CheckBox ID="CB_HDR" runat="server" Font-Names="新細明體"
                    Font-Size="Small" Height="27px" Text="第一行為欄位名稱" ToolTip="勾選時視第一行為欄位名稱" Width="221px" />
                <br />
                <asp:Button ID="BT_CreateTable" runat="server" Height="34px" Text="新建資料表" Width="100px" OnClick="BT_CreateTable_Click" /><br />
                <asp:Button ID="BT_DropTable" runat="server" Height="34px" Text="刪除新建資料表"
                    Width="100px" OnClick="BT_DropTable_Click" /><br /><asp:Button ID="BT_UpdataTable" runat="server" Height="34px" Text="更新資料表"
                    Width="100px" OnClick="BT_UpdataTable_Click" /><br />
                <asp:Button ID="BT_ReduceTable" runat="server" Height="34px" Text="復原資料表"
                    Width="100px" OnClick="BT_ReduceTable_Click" Visible="False" /><br />
            </td>
             <td style="text-align: center; width: 656px; height: 418px;">
                <asp:TextBox ID="TextBox1" runat="server" Height="473px" TextMode="MultiLine" Width="628px"></asp:TextBox>&nbsp;</td>
        </tr>
    </table>
    <asp:Button ID="clearall" runat="server" OnClick="clearall_Click" Text="清除全部" Visible="False" />
    <asp:Button ID="qset" runat="server" OnClick="qset_Click" Text="快速設定" /><br />
    <asp:TextBox ID="TB_temp1" runat="server" Width="500px" Visible="False"></asp:TextBox><br />
                <asp:TextBox ID="TB_FileName" runat="server" Width="500px" Visible="False"></asp:TextBox><br />
    <asp:TextBox ID="TextBox2" runat="server" Width="500px" Visible="False"></asp:TextBox><br />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <br />
</asp:Content>

