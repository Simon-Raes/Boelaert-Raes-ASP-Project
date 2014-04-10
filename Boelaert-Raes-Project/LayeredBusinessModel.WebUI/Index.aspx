<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divRecommended" runat="server">
        <div class="page-title category-title">
            <h1>Recommended for you</h1>
        </div>

        <div class="row" id="recommened" runat="server">
        </div>
    </div>

    <div class="page-title category-title">
        <h1>New releases</h1>
    </div>

    <div class="row" id="newReleases" runat="server">
    </div>

    <div class="page-title category-title">
        <h1>Most popular</h1>
    </div>

    <div class="row" id="mostPopular" runat="server">
    </div>





</asp:Content>
