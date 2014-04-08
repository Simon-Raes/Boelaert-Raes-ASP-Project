<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
                                    <div class="padding-s" id="new-releases">

                                        <div class="page-title category-title">
                                            <h1>New Movies</h1>
                                        </div>

                                        <ul class="products-grid row">
                                            <li class="item col-3">
                                                <img src="http://ia.media-imdb.com/images/M/MV5BMjIxMjgxNTk0MF5BMl5BanBnXkFtZTgwNjIyOTg2MDE@._V1_SX640_SY720_.jpg" />
                                                <div class="product-shop">
                                                    <h3 class="product-name">
                                                        <a href="#" title="The Wolf Of Wallstreet">The Wolf Of Wallstreet</a>
                                                    </h3>
                                                    <div class="product-author">Martin Scorsese</div>
                                                    <div class="price-box">
                                                        <span class="regular-price">
                                                            <span class="price">€36.86</span>
                                                        </span>
                                                   </div>
                                                    <div class="actions">
                                                        
                                                        <a href="#" class="button btn-cart">Add to cart</a>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>

                                    </div>

</asp:Content>
