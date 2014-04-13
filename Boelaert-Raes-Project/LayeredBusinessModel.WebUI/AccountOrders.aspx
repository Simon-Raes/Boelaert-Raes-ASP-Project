<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AccountOrders.aspx.cs" Inherits="LayeredBusinessModel.WebUI.AccountOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">

            <ul id="myTab" class="nav nav-tabs">
                <li><a href="AccountOverview.aspx">Overview</a></li>
                <li class="active"><a href="AccountOrders.aspx">Orders</a></li>
                <li><a href="AccountSettings.aspx">Settings</a></li>
            </ul>

            <br />

            <asp:GridView ID="gvOrders" runat="server" OnRowCommand="gvOrders_RowCommand">
                <Columns>
                    <asp:ButtonField CommandName="Details" HeaderText="Details" Text="Details" />
                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlOrderDetails" runat="server" Visible="False">
                <br />
                Order number:
                            <asp:Label ID="lblOrderID" runat="server" Text="Label"></asp:Label>
                <br />
                Order status:
                            <asp:Label ID="lblOrderStatus" runat="server" Text="Label"></asp:Label>
                (1 = new, 2 = paid, 3 = shipped)<br />
                <asp:Label ID="lblOrderStatusDetails" runat="server" Text=""></asp:Label>

                <br />
                <asp:Label ID="lblPay" runat="server" Text="Click here to pay for this order:"></asp:Label><asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click" Text="Go to payment page" />
                <br />
                <asp:GridView ID="gvOrderDetails" runat="server" OnRowCommand="gvOrderDetails_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="CancelOrderLine" Text="Cancel" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br />
            Geplande verhuurcopies misschien in een aparte lijst zetten, moeten tot 2 dagen voor verhuur geannuleerd kunnen worden.<br />
            &nbsp;&nbsp;
        </div>
    </div>
</asp:Content>
