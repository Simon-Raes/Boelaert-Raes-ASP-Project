<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="padding-s">

        <div class="page-title category-title">
            <h1>New Movies</h1>
        </div>

        <ul class="products-grid row" id="row_new_1" runat="server">
        </ul>
        <ul class="products-grid row" id="row_new_2" runat="server">
        </ul>

    </div>
    <br />
    <div class="padding-s">
        <br />
        <div class="page-title category-title">
            <h1>Most Popular</h1>
        </div>

        <ul class="products-grid row" id="row_popular_1" runat="server">
        </ul>
        <ul class="products-grid row" id="row_popular_2" runat="server">
        </ul>

    </div>

</asp:Content>
