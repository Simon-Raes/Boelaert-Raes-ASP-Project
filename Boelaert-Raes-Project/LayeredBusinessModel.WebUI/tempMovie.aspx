<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tempMovie.aspx.cs" Inherits="LayeredBusinessModel.WebUI.tempMovie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        DIT IS EEN TIJDELIJKE PAGINA!!! Moet vervangen worden door een pagina die dynamisch aangemaakt wordt (klikken op film -&gt; filminfo ophalen en pagina mee opbouwen -&gt; weergeven)
    </p>
    <p>
        <h1>The Shawshank Redemption</h1>
        <asp:Panel ID="pnlLogin" runat="server">log in to access all features</asp:Panel>

    </p>

    <asp:Panel ID="pnlActions" runat="server">

        <br />
        <strong>Buy dvd:</strong>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnBuy" runat="server" Text="Buy" OnClick="btnBuy_Click" />

                </td>
                <td>Availability: 
                    <asp:Label ID="lblBuyAvailability" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>




        <br />
        <strong>Rent dvd:</strong>
        <br />
        <table>
            <tr>
                <td>startdate
                <asp:Calendar ID="calRentStartDate" runat="server" OnDayRender="calRentStartDate_DayRender"></asp:Calendar>
                </td>
                <td>Duration<br />
                    <asp:DropDownList ID="ddlRentDuration" runat="server">
                        <asp:ListItem Value="1">1 day</asp:ListItem>
                        <asp:ListItem Value="2">2 days</asp:ListItem>
                        <asp:ListItem Value="7">7 days</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Button ID="btnRent" runat="server" Text="Rent" OnClick="btnRent_Click" />
                </td>
                <td>Adds rent movie (+start and enddate) to shoppingcart.
                    <br />
                    Availability: 
                    <asp:Label ID="lblRentAvailability" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlReserve" runat="server">
            The DVD is currently unavailable for renting, but will be back in stock on DATUMHIER.
            You can reserve the dvd below, your renting period will start on ZELFDEDATUMHIER<br />
            <strong>Reserve dvd (for renting only):</strong>
            <table>
                <tr>
                    <td>start<asp:Calendar ID="calReservationStartDate" runat="server" OnDayRender="calReservationStartDate_DayRender"></asp:Calendar>
                    </td>
                    <td>
                        <asp:Button ID="btnReserve" runat="server" Text="Reserve" OnClick="btnReserve_Click" />

                    </td>
                    <td>Not yet implemented.
                    </td>
                </tr>
            </table>

        </asp:Panel>



    </asp:Panel>

</asp:Content>
