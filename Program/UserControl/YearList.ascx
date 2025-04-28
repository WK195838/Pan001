<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YearList.ascx.cs" Inherits="UserControl_YearList" %>
<script>
function abgne(post){
if(event.keyCode=="13"){
//document.getElementById("YearList1_btn").setfocus(1);
document.getElementById(post).click();
}
}
</script>
<asp:DropDownList ID="ddlYear" runat="server" OnTextChanged="ddlYear_TextChanged" >
</asp:DropDownList>
<div style="display:none"><asp:Button ID="btn" runat="server" Text="GO" /></div>
