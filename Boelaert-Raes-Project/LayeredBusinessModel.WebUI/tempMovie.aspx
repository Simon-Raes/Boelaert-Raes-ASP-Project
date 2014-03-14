<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="tempMovie.aspx.cs" Inherits="LayeredBusinessModel.WebUI.tempMovie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        DIT IS EEN TIJDELIJKE PAGINA!!! Moet vervangen worden door een pagina die dynamisch aangemaakt wordt (klikken op film -&gt; filminfo ophalen en pagina mee opbouwen -&gt; weergeven)</p>
    <p>
        <h1>The Shawshank Redemption</h1>
    log in to access all features</p>

    <asp:Panel ID="pnlActions" runat="server">

         <strong>
    <br />
    Rent movie:</strong><br />
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
            <td>
                Adds rent movie (+start and enddate) to shoppingcart.
            </td>
        </tr>
    </table>



    <br />
         Is reserveren niet hetzelfde als huren vanaf een bepaalde datum?<br />
         <br />
         <strong>Reserve movie (for renting only):</strong>
        <table>
            <tr>
                <td>start<asp:Calendar ID="calReservationStartDate" runat="server" OnDayRender="calReservationStartDate_DayRender"></asp:Calendar>
                </td>
                <td>
                    <asp:Button ID="btnReserve" runat="server" Text="Reserve" OnClick="btnReserve_Click" />

                </td>
                <td>
                    Not yet implemented.
                </td>
            </tr>
        </table>
    </p>
    </asp:Panel>
   
</asp:Content>
