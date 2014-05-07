<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="LayeredBusinessModel.WebUI.AccountSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            <ul id="myTab" class="nav nav-tabs">
                <li><a href="Overview.aspx">Overview</a></li>
                <li><a href="Orders.aspx">Orders</a></li>
                <li class="active"><a href="Settings.aspx">Settings</a></li>
                <li class="pull-right"><button id="Button1" runat="server" class="btn btn-warning" onserverclick="btnLogOut_Click">Sign out</button></li>

            </ul>

            <br />


            <div class="form-horizontal">
                <fieldset>
                    <legend>Update information</legend>

                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>

                    <div class="form-group">
                        <label for="inputName" class="col-sm-4 control-label">Name</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" id="inputName" placeholder="Name" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="inputName" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail" class="col-sm-4 control-label">Email</label>
                        <div class="col-sm-4">
                            <input type="email" class="form-control" id="inputEmail" placeholder="Email" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="inputEmail" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="valCustEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="An account with that address already exists." OnServerValidate="valCustEmail_ServerValidate" Display="Dynamic" ForeColor="#CC3300"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="inputEmail" ErrorMessage="Not a valid e-mail address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ForeColor="#CC3300"></asp:RegularExpressionValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputOldPassword" class="col-sm-4 control-label">Current password</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputOldPassword" placeholder="Current password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="inputOldPassword" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword" class="col-sm-4 control-label">New password</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputPassword" placeholder="Password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="inputPassword" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPasswordAgain" class="col-sm-4 control-label">New password (again)</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputPasswordAgain" placeholder="Password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="inputPasswordAgain" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="inputPassword" ControlToValidate="inputPasswordAgain" ErrorMessage="Password fields must be equal." Display="Dynamic" ForeColor="#CC3300">Password fields must be equal.</asp:CompareValidator>

                            </p>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label"></div>

                        <div class="col-sm-4">
                            <button class="btn btn-success form-control" id="btnSignUp" runat="server" onserverclick="btnUpdate_Click">Update</button>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
