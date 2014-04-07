﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        Cart content:</p>
    <p>
        <asp:GridView ID="gvCart" runat="server" OnRowDeleting="gvCart_RowDeleting">
            <Columns>
                <asp:CommandField AccessibleHeaderText="Remove" DeleteText="Remove" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:Button ID="btnCheckout" runat="server" OnClick="btnCheckout_Click" Text="Checkout" />
    </p>
    <p>
        &nbsp;</p>
</asp:Content>