<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prompt_Upload.aspx.cs" Inherits="Prompt_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>上傳</title>
</head>
<base target="_self">
<body>
    <form id="form1" runat="server" target="">
    <div>
        <asp:Label ID="lbl_UploadPicture" runat="server" />
        <asp:FileUpload ID="fu_Picture" runat="server" />
        <asp:Button ID="btUpload" runat="server" OnClick="btUpload_Click" Text="上傳" /><br />
        <asp:Label ID="lbl_Msg" runat="server" Text=""></asp:Label></div>
    </form>
</body>
</html>
