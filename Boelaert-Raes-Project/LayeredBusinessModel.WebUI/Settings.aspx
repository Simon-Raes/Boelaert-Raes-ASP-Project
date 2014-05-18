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
                <li class="pull-right">
                    <button id="Button1" runat="server" class="btn btn-warning" onserverclick="btnLogOut_Click" causesvalidation="false">Sign out</button></li>
            </ul>

            <br />

            <div class="form-horizontal">
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSignUp">
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
                                <asp:RequiredFieldValidator ID="valRequiredEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="valCustEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="An account with that address already exists." OnServerValidate="valCustEmail_ServerValidate" Display="Dynamic" ForeColor="#CC3300"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="valRegEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="Not a valid e-mail address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ForeColor="#CC3300"></asp:RegularExpressionValidator>
                            </p>
                            
                        </div>
                        <div class="col-sm-4">Please note: Changing your email address will also change your login name!</div>
                    </div>

                    <div class="form-group">
                        <label for="inputOldPassword" class="col-sm-4 control-label">Current password</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputOldPassword" placeholder="Current password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valRequiredOldPassword" runat="server" ControlToValidate="inputOldPassword" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-4"></div> <!-- for padding left -->
                        <div class="col-sm-4">
                            <asp:CheckBox ID="cbPassword" runat="server" AutoPostBack="True" OnCheckedChanged="cbPassword_CheckedChanged" />
                            Change password
                        </div>
                    </div>
                    
                    <!-- Textfields for new password -->
                    <div id="divNewPass" runat="server">
                        <div class="form-group">
                            <label for="inputPassword" class="col-sm-4 control-label">New password</label>
                            <div class="col-sm-4">
                                <input type="password" class="form-control" id="inputPassword" placeholder="Password" runat="server" />
                                <p class="help-block">
                                    <asp:RequiredFieldValidator ID="valRequiredNewPassword" runat="server" ControlToValidate="inputPassword" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                                </p>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="inputPasswordAgain" class="col-sm-4 control-label">New password (again)</label>
                            <div class="col-sm-4">
                                <input type="password" class="form-control" id="inputPasswordAgain" placeholder="Password" runat="server" />
                                <p class="help-block">
                                    <asp:RequiredFieldValidator ID="valRequiredNewPasswordAgain" runat="server" ControlToValidate="inputPasswordAgain" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="valCompareNewPassword" runat="server" ControlToCompare="inputPassword" ControlToValidate="inputPasswordAgain" ErrorMessage="Password fields must be equal." Display="Dynamic" ForeColor="#CC3300">Password fields must be equal.</asp:CompareValidator>

                                </p>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="form-group">
                        <div class="col-sm-4 control-label"></div>

                        <div class="col-sm-4">                           
                                <asp:Button ID="btnSignUp" runat="server" Text="Update" CssClass="btn btn-success form-control" OnClick="btnSignUp_Click" />
                            
                        </div>
                    </div>
                </fieldset>
                    </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
