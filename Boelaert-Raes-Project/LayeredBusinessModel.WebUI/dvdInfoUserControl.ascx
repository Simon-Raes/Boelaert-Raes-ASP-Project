<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="dvdInfoUserControl.ascx.cs" Inherits="LayeredBusinessModel.WebUI.dvdInfoUserControl" %>
<asp:Panel ID="pnlDvdInfo" runat="server" CssClass="col-md-3">
    <div class="borderbox">
        <asp:HyperLink ID="dvdInfoLink" runat="server">
            <asp:Image ID="imgDvdCover" runat="server" />
        </asp:HyperLink>


        <div class="dvdtitle">
            <h4>
                <asp:HyperLink ID="dvdInfoLink2" runat="server">
                    <asp:Label ID="lblTitle" runat="server" Text="Title" Font-Size="Medium">

                    </asp:Label>

                </asp:HyperLink>

            </h4>
        </div>
        <asp:Button ID="btnBuy" runat="server" Text="Buy"  class="btn btn-success price-box"/>
        <asp:Button ID="btnRent" runat="server" Text="Rent" class="btn btn-success price-box" />
    </div>
</asp:Panel>

