<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default">
        <div class="panel-body">

            <div class="form-horizontal">
                <fieldset>
                    <legend>Sign up</legend>

                    <div class="form-group">
                        <label for="inputName" class="col-sm-4 control-label">Name</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" id="inputName" placeholder="Name" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqName" runat="server" ControlToValidate="inputName" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputEmail" class="col-sm-4 control-label">Email</label>
                        <div class="col-sm-4">
                            <input type="email" class="form-control" id="inputEmail" placeholder="Email" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="valCustEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="An account with that address already exists." OnServerValidate="valCustEmail_ServerValidate" Display="Dynamic" ForeColor="#CC3300"></asp:CustomValidator>
                                <asp:RegularExpressionValidator ID="valRegEmail" runat="server" ControlToValidate="inputEmail" ErrorMessage="Not a valid e-mail address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ForeColor="#CC3300"></asp:RegularExpressionValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputLogin" class="col-sm-4 control-label">Login name</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" id="inputLogin" placeholder="Login name" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqLogin" runat="server" ControlToValidate="inputLogin" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="valCustLogin" runat="server" ControlToValidate="inputLogin" ErrorMessage="That name is already taken." OnServerValidate="valCustLogin_ServerValidate" ValidateEmptyText="True" Display="Dynamic" ForeColor="#CC3300"></asp:CustomValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPassword" class="col-sm-4 control-label">Password</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputPassword" placeholder="Password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqPassword" runat="server" ControlToValidate="inputPassword" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>

                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="inputPasswordAgain" class="col-sm-4 control-label">Password (again)</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputPasswordAgain" placeholder="Password" runat="server" />
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqPasswordAgain" runat="server" ControlToValidate="inputPasswordAgain" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="valEqualPassword" runat="server" ControlToCompare="inputPassword" ControlToValidate="inputPasswordAgain" ErrorMessage="Password fields must be equal." Display="Dynamic" ForeColor="#CC3300">Password fields must be equal.</asp:CompareValidator>

                            </p>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-4 control-label"></div>

                        <div class="col-sm-4">
                            <button class="btn btn-success form-control" id="btnSignUp" runat="server" onserverclick="btnRegister_Click">Sign up</button>
                            <button class="btn btn-success form-control" id="btnEID" runat="server" onserverclick="btnEID_ServerClick" causesvalidation="false">eID</button>
                            <!--<button class="btn btn-danger" id="btnReset" runat="server">Reset</button>-->
                        </div>
                    </div>
                    <div>
                        <asp:Panel ID="pnlAppletIE" runat="server" Height="100px" Width="50px" Visible="false">
                                    <applet
                                      codebase = "."
                                      archive  = "scripts/eidlib.jar"
                                      name     = "BEIDApplet"
                                      width    = "0"
                                      height   = "0"
                                      type     = "application/x-java-applet;version=1.5.0"
                                      code     = "be.belgium.eid.BEID_Applet.class"
                                    >   
                                </applet>
                                <script type="text/javascript" src="scripts/eidreader.js"></script>
                                <script type="text/javascript">ReadCard();</script>
 </asp:Panel>
 <asp:Panel ID="pnlAppletFF" runat="server" Height="100px" Width="50px" Visible="false">
                                    <applet
                                      codebase = "."
                                      archive  = "scripts/eidlib.jar"
                                      name     = "BEIDApplet"
                                      width    = "0"
                                      height   = "0"
                                      type     = "application/x-java-applet;version=1.5.0"
                                      classid  = "java:be.belgium.eid.BEID_Applet.class"
                                    >
                                </applet>
                                <script type="text/javascript" src="scripts/eidreader.js"></script>
                                <script type="text/javascript">ReadCard();</script> </asp:Panel>
                    </div>
                </fieldset>
            </div>
        </div>
        TODO: gebruikersgegevens laden via eID
    </div>
    BUG: elke button activeert de validators (bv "Sign in" uitvoeren vanaf deze pagina)
</asp:Content>
