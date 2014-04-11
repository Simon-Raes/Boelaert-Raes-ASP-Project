<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divRecommended" runat="server">
        <div class="page-title category-title">
            <h1>Recommended for you
            <a href="Catalog.aspx?type=recommended" class="pull-right btn btn-primary">See more</a></h1>
        </div>

        <div class="row" id="recommened" runat="server">
        </div>
    </div>

    <div class="page-title category-title">
        <h1>New releases
            <a href="Catalog.aspx?type=recent" class="pull-right btn btn-primary">See more</a></h1>
    </div>

    <div class="row" id="newReleases" runat="server">

    </div>

    <div class="page-title category-title">
        <h1>Most popular
            <a href="Catalog.aspx?type=popular" class="pull-right btn btn-primary">See more</a></h1>
    </div>

    <div class="row" id="mostPopular" runat="server">

    </div>





</asp:Content>
