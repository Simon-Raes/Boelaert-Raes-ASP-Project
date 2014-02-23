<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeerList.aspx.cs" Inherits="LayeredBusinessModel.WebUI.BeerList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:DropDownList ID="ddBrewers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddBrewers_SelectedIndexChanged">
        </asp:DropDownList>
    
        <asp:GridView ID="gvBeer" runat="server">
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
