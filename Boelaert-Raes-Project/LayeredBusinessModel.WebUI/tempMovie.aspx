<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tempMovie.aspx.cs" Inherits="LayeredBusinessModel.WebUI.tempMovie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="css/iThing-min.css" type="text/css" />

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
        <br />
        <br />
        Copy reservation prototype<br />
        <br />
        <div id="slider"></div><br /><br />
        <script src="js/jquery-ui-1.10.4.custom.js"></script>
        <script src="js/jQDateRangeSlider-min.js"></script>
        <script>
            
            $("#slider").dateRangeSlider();
            $("#slider").dateRangeSlider({ arrows: false });
            $("#slider").dateRangeSlider({
                bounds: {
                    min: new Date(2014, 5, 1),
                    max: new Date(2014, 5, 15)
                }
            });
            $("#slider").dateRangeSlider({
                defaultValues: {
                    min: new Date(2014, 5, 6),
                    max: new Date(2014, 5, 10)
                }
            });
            $("#slider").dateRangeSlider({
                step: {
                    days: 1
                }
            });
            
        </script>

        <div class="progress">
            <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100" style="width: 20%"></div>
            <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 30%"></div>
            <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 50%"></div>
        </div>
        <div class="progress">
            <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="10" aria-valuemin="0" aria-valuemax="100" style="width: 10%"></div>
            <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100" style="width: 30%"></div>
            <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 60%"></div>

        </div>

        <br />
        <br />
        <br />


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
