<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="LayeredBusinessModel.WebUI.detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-4">
                    <asp:Image ID="imgDvdCoverFocus" runat="server" />
                </div>
                <div class="col-md-8">
                    <%-- <div class="row">--%>

                    <h2>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label><asp:HyperLink ID="linkYear" runat="server"></asp:HyperLink></h2>
                    <hr />
                    Director:
                        <asp:HyperLink ID="linkDirector" runat="server"></asp:HyperLink><br />
                    <asp:Label ID="lblActors" runat="server" Text="Actors: "></asp:Label>
                    <asp:PlaceHolder ID="actorLinks" runat="server"></asp:PlaceHolder>

                    <span id="spanRuntime" runat="server">
                        <span class="glyphicon glyphicon-time"></span>
                        <asp:Label ID="lblDuration" runat="server"></asp:Label>
                        |
                    </span>

                    <asp:PlaceHolder ID="genreLinks" runat="server"></asp:PlaceHolder>
                    <hr />

                    <asp:Label ID="lblPlot" runat="server" CssClass="plot"></asp:Label>
                    <hr />
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Button ID="btnBuy" runat="server" Text="Add to cart" class="btn btn-danger form-control" OnClick="btnBuy_Click" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnRent1" runat="server" Text="Add to cart" class="btn btn-danger form-control" OnClick="btnRent1_Click" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnRent3" runat="server" Text="Add to cart" class="btn btn-danger form-control" OnClick="btnRent3_Click" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnRent7" runat="server" Text="Add to cart" class="btn btn-danger form-control" OnClick="btnRent7_Click" />
                            <!-- CssClass="price-box detail" -->
                        </div>
                    </div>
                    <%--                    </div>--%>
                </div>


            </div>


            <asp:Calendar ID="calRent" runat="server" OnDayRender="calRent_DayRender"></asp:Calendar>


            <div class="row">
                <div class="col-lg-12">
                    <h4>Trailers and Pictures</h4>
                    <div class="mediaList">
                        <div class="scrollrow" runat="server" id="scrollrow">
                        </div>
                    </div>
                </div>
            </div>

            

        </div>
    </div>

    <div class="panel panel-default" runat="server" id="pnlRelatedDvds">
        <div class="panel-body">
            <asp:Label ID="Label3" runat="server" Text="Related Dvd's" Font-Size="20"></asp:Label>
            <asp:HyperLink ID="linkRelated" runat="server" CssClass="pull-right btn btn-primary">See more</asp:HyperLink>
            <br /><br />
            <div class="row" id="relatedDvds" runat="server">
                <!-- DVD cards inserted here-->
            </div>
        </div>
    </div>

</asp:Content>
