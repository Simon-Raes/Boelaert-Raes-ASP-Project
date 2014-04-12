<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="LayeredBusinessModel.WebUI.detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-4">
            <asp:Image ID="imgDvdCoverFocus" runat="server" />
        </div>
        <div class="col-md-8">
            <div class="row">

                <h2><asp:Label ID="lblTitle" runat="server"></asp:Label><asp:HyperLink ID="linkYear" runat="server"></asp:HyperLink></h2>
                <hr />
                Director: <asp:HyperLink ID="linkDirector" runat="server"></asp:HyperLink><br />
                Actors:   <asp:PlaceHolder ID="actorLinks" runat="server"></asp:PlaceHolder>
                <span class="glyphicon glyphicon-time"></span> <asp:Label ID="lblDuration" runat="server"></asp:Label> | <asp:PlaceHolder ID="genreLinks" runat="server"></asp:PlaceHolder>
                <hr />

                <asp:Label ID="lblPlot" runat="server" CssClass="plot"></asp:Label>
                <hr />
                <div class="col-md-6">
                    <div class="row">
                        <asp:Button ID="btnBuy" runat="server" Text="Add to cart" CssClass="price-box detail"/> 
                    </div>
                    <div class="row">
                       <asp:Button ID="btnRent1" runat="server" Text="Add to cart" CssClass="price-box detail"/>
                    </div>
                    <div class="row">
                     <asp:Button ID="btnRent3" runat="server" Text="Add to cart" CssClass="price-box detail"/>
                    </div>
                    <div class="row">
                       <asp:Button ID="btnRent7" runat="server" Text="Add to cart" CssClass="price-box detail"/>
                    </div>
                </div>
            </div>
            

            

        </div>
    
        
    </div>
    <div class="row">
        <div class="col-lg-12">
            <h4>Trailers and Pictures</h4>
            <div class="mediaList">
                <div class="scrollrow" runat="server" id="scrollrow">
                          
                </div>
            </div>
            </div>
    </div>
</asp:Content>
