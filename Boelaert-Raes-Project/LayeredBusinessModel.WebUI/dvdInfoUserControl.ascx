<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dvdInfoUserControl.ascx.cs" Inherits="LayeredBusinessModel.WebUI.dvdInfoUserControl" %>
<asp:Panel ID="pnlDvdInfo" runat="server" CssClass="col-md-3">
    <div class="borderbox">
    <asp:Image ID="imgDvdCover" runat="server" />


    <h4><asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label></h4>
    <asp:Button ID="btnBuy" runat="server" Text="Buy" CssClass="price-box"/> <asp:Button ID="btnRent" runat="server" Text="Rent" CssClass="price-box"/>
    </div>
</asp:Panel>

