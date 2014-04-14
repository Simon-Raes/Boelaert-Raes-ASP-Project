<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Spotlight -->
    <div class="panel panel-default">
        <div class="panel-body">
            <asp:Label ID="Label4" runat="server" Text="Spotlight" Font-Size="20"></asp:Label>
            <br /><br />
            <a id="anchorSpotlight" runat="server"><img id="imgSpotlight" runat="server" class="img-responsive" src="http://cdn.wegotthiscovered.com/wp-content/uploads/Expendables-Banner-Poster.jpg" /></a>
        </div>
    </div>

    <!-- Recommendations -->
    <div id="divRecommended" runat="server">
        <div class="panel panel-default">
            <div class="panel-body">
                <asp:Label ID="Label1" runat="server" Text="Recommended for you" Font-Size="20"></asp:Label>
                <a href="Catalog.aspx?type=recommended" class="pull-right btn btn-primary">See more</a>
                <br /><br />
                <div class="row" id="recommened" runat="server">
                    <!-- DVD cards inserted here-->
                </div>
            </div>
        </div>
    </div>

    <!-- Recent releases -->
    <div class="panel panel-default">
        <div class="panel-body">
            <asp:Label ID="Label2" runat="server" Text="Recent releases" Font-Size="20"></asp:Label>
            <a href="Catalog.aspx?type=recent" class="pull-right btn btn-primary">See more</a>
            <br /><br />
            <div class="row" id="newReleases" runat="server">
                <!-- DVD cards inserted here-->
            </div>
        </div>
    </div>

    <!-- Most popular -->
    <div class="panel panel-default">
        <div class="panel-body">
            <asp:Label ID="Label3" runat="server" Text="Most popular" Font-Size="20"></asp:Label>
            <a href="Catalog.aspx?type=popular" class="pull-right btn btn-primary">See more</a>
            <br /><br />
            <div class="row" id="mostPopular" runat="server">
                <!-- DVD cards inserted here-->
            </div>
        </div>
    </div>




</asp:Content>
