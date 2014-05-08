<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderPayment.aspx.cs" Inherits="LayeredBusinessModel.WebUI.OrderPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            Order:
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label><br />
            <asp:GridView ID="gvOrderDetails"  CssClass="gridViewStyle" runat="server"></asp:GridView>

            <br />
            Total:
        <asp:Label ID="lblCost" runat="server" Text=""></asp:Label>
            <br />
            <br />
            Payment options:<br />
            Credit card, paysafecard, iDeal, etc..
    <asp:Button ID="btnPay" runat="server" Text="Pay" OnClick="btnPay_Click" />
        </div>
    </div>
</asp:Content>
