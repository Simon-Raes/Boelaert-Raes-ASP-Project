<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usercontroltestpage.aspx.cs" Inherits="LayeredBusinessModel.WebUI.usercontroltestpage" %>

<%@ Register Src="~/dvdInfoUserControl.ascx" TagPrefix="uc1" TagName="dvdInfoUserControl" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:dvdInfoUserControl runat="server" id="dvdInfoUserControl" />
    </div>
    </form>
</body>
</html>
