<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dvdInfoUserControl.ascx.cs" Inherits="LayeredBusinessModel.WebUI.dvdInfoUserControl" %>
<asp:Panel ID="pnlDvdInfo" runat="server" CssClass="col-md-3">
    <asp:Image ID="imgDvdCover" runat="server" />



    <br />

    <h2><asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label></h2>
    <asp:Button ID="btnBuy" runat="server" Text="Buy" /><asp:Button ID="btnRent" runat="server" Text="Rent" />

</asp:Panel>

