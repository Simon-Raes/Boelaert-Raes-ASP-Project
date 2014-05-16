<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="LayeredBusinessModel.WebUI.AccountOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">


            <ul id="myTab" class="nav nav-tabs">
                <li><a href="Overview.aspx">Overview</a></li>
                <li class="active"><a href="Orders.aspx">Orders</a></li>
                <li><a href="Settings.aspx">Settings</a></li>
                <li class="pull-right">
                    <button runat="server" class="btn btn-warning" onserverclick="btnLogOut_Click" causesvalidation="false">Sign out</button>
                </li>
            </ul>


            <br />
            <asp:Label ID="lblNoOrders" runat="server" Text="You have not placed any orders yet." Visible="false"></asp:Label>

            <asp:GridView ID="gvOrders" CssClass="gridViewStyle" runat="server" OnRowCommand="gvOrders_RowCommand">
                <Columns>
                    <asp:ButtonField CommandName="Details" HeaderText="Details" Text="Details" />
                </Columns>
            </asp:GridView>

        </div>
    </div>

    <asp:Panel ID="pnlOrderDetails" runat="server" Visible="False">
        <div class="panel panel-default">
            <div class="panel-body">

                <h2>Order
                    <asp:Label ID="lblOrderID" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="lblOrderStatus" runat="server" Text="Label"></asp:Label>
                </h2>
                <asp:Label ID="lblOrderStatusDetails" runat="server" Text=""></asp:Label>

               <br />
                <asp:GridView ID="gvOrderDetails" CssClass="gridViewStyle" runat="server" OnRowCommand="gvOrderDetails_RowCommand">
                    <Columns>
                        <asp:ButtonField CommandName="CancelOrderLine" Text="Cancel" />
                    </Columns>
                </asp:GridView>
                Total cost: <asp:Label ID="lblTotalCost" runat="server" Text=""></asp:Label>
                 <br />
                <asp:Label ID="lblPay" runat="server" Text="This order is still awaiting payment. Click here to go to the payment page: "></asp:Label><asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click" Text="Go to payment page" />
                
            </div>
        </div>
    </asp:Panel>

</asp:Content>
