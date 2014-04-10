<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dvdInfoUserControl.ascx.cs" Inherits="LayeredBusinessModel.WebUI.dvdInfoUserControl" %>
<asp:Panel ID="pnlDvdInfo" runat="server" CssClass="col-md-3">
    <div class="borderbox">
        <asp:HyperLink ID="dvdInfoLink" runat="server">
            <asp:Image ID="imgDvdCover" runat="server" />
        </asp:HyperLink>


        <h4><asp:HyperLink ID="dvdInfoLink2" runat="server"><asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label></asp:HyperLink></h4>
        <asp:Button ID="btnBuy" runat="server" Text="Buy" CssClass="price-box"/> 
        <asp:Button ID="btnRent" runat="server" Text="Rent" CssClass="price-box"/>
    </div>
</asp:Panel>

