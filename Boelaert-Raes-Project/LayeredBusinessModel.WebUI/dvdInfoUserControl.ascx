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
        <button id="btnBuyB" runat="server" type="submit" onserverclick="btnBuy_Click"></button>
        <button id="btnRentB" runat="server" type="submit" onserverclick="btnRent_Click"></button>
    </div>
</asp:Panel>

