<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        show content of shopping cart here:</p>
    <p>
        <asp:GridView ID="gvCart" runat="server">
        </asp:GridView>
    </p>
    <p>
        todo: SQL joins om hier titels te krijgen in plaats van id nummers</p>
    <p>
        todo: optie om items terug uit cart te halen</p>
</asp:Content>
