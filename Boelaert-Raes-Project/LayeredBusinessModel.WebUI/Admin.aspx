<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            Add 10 buy copies and 2 rent copies to a dvdInfo. (enter dvdInfo id number) <br />
            <asp:TextBox ID="txtDvdInfo" runat="server"></asp:TextBox>
            <button class="btn btn-success" id="Button2" runat="server" onserverclick="Button1_Click">Add copies to dvdInfo</button>
        </div>
    </div>


    <div class="panel panel-default">
        <div class="panel-body">


            <div class="form-horizontal">
                <fieldset>
                    <legend>Add new DVD</legend>
                    TODO: meerdere genres, media

                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>

                    <div class="form-group">
                        <label for="inputGenre" class="col-sm-2 control-label">Genre</label>
                        <div class="col-sm-8">
                            <select runat="server" id="inputGenre" class="form-control">
                            </select>
                            <p class="help-block">
                                The genre of the DVD.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputName" class="col-sm-2 control-label">Name</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputName" placeholder="Name" runat="server" />
                            <p class="help-block">
                                The name of the DVD.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputYear" class="col-sm-2 control-label">Year</label>
                        <div class="col-sm-8">
                            <input type="email" class="form-control" id="inputYear" placeholder="Year" runat="server" />
                            <p class="help-block">
                                The year in which the DVD was released.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputAuthor" class="col-sm-2 control-label">Author</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputAuthor" placeholder="Author" runat="server" />
                            <p class="help-block">
                                The director or creator of the DVD.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputDescription" class="col-sm-2 control-label">Description</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputDescription" placeholder="Description" runat="server" />
                            <p class="help-block">
                                The description of the DVD (plot, summary,...).
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputRent" class="col-sm-2 control-label">Rent price/day</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputRent" placeholder="Rent price/day" runat="server" />
                            <p class="help-block">
                                The price to rent this DVD for 1 day. Use commas(','), not points ('.').
                            </p>
                        </div>
                    </div>


                    <div class="form-group">
                        <label for="inputBuy" class="col-sm-2 control-label">Buy price</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputBuy" placeholder="Buy price" runat="server" />
                            <p class="help-block">
                                The price to buy this DVD. Use commas(','), not points ('.'). 
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputDate" class="col-sm-2 control-label">Date added</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputDate" placeholder="Date added" runat="server" />
                            <p class="help-block">
                                The date when this DVD was entered into the system. Use a YYYY-MM-DD format.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputAmountSold" class="col-sm-2 control-label">Amount sold</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputAmountSold" placeholder="Amount sold" runat="server" />
                            <p class="help-block">
                                The number of copies that have been sold. Must be at least 0 (can not be NULL).
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputActors" class="col-sm-2 control-label">Actors</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputActors" placeholder="Actors" runat="server" />
                            <p class="help-block">
                                The actors that are featured on this DVD. Separate multiple actors with a comma (',').
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputDuration" class="col-sm-2 control-label">Duration</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="inputDuration" placeholder="Duration" runat="server" />
                            <p class="help-block">
                                Duration in minutes. Must be an integer value.
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-4 control-label"></div>
                        <div class="col-sm-4">
                            <button class="btn btn-success form-control" id="btnAdd" runat="server" onserverclick="btnAdd_Click">Add</button>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
