<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        show content of shopping cart here:</p>
    <p>
        <asp:GridView ID="gvCart" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvCart_RowDeleting">
            <Columns>
                <asp:BoundField AccessibleHeaderText="dvd_copy_id" DataField="dvd_copy_id" HeaderText="dvd_copy_id" />
                <asp:BoundField AccessibleHeaderText="Serialnumber" DataField="serialnumber" HeaderText="Serialnumber" />
                <asp:CommandField AccessibleHeaderText="Remove" DeleteText="Remove" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:Button ID="btnCheckout" runat="server" OnClick="btnCheckout_Click" Text="Checkout" />
    </p>
    <p>
        todo: echte titels ipv codes + joins met andere tabellen om zaken als start/enddate te krijgen + remove gridview row on click</p>
    <p>
        &nbsp;</p>
</asp:Content>
