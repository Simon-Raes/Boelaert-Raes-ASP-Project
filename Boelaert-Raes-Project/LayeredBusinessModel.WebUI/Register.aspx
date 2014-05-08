<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Register" %>

<%@ Register Assembly="Arcabase.EID.SDK" Namespace="Arcabase.EID.SDK.Web" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                        <label for="txtName" class="col-sm-4 control-label">Name</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="valReqName" runat="server" ControlToValidate="txtName" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtStreet" class="col-sm-4 control-label">Street</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control"></asp:TextBox>
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStreet" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtPostalcode" class="col-sm-4 control-label">Postalcode</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtPostalcode" runat="server" CssClass="form-control"></asp:TextBox>
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPostalcode" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtCountry" class="col-sm-4 control-label">Country</label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control"></asp:TextBox>
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCountry" ErrorMessage="Field can't be empty." Display="Dynamic" ForeColor="#CC3300"></asp:RequiredFieldValidator>
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
                           
                            <asp:Button ID="btnEID" runat="server" Text="eID" CssClass="btn btn-success form-control" OnClick="btnEID_Click" CausesValidation="false" UseSubmitBehavior="false"/>
                            
                            <!--<button class="btn btn-danger" id="btnReset" runat="server">Reset</button>-->
                        </div>
                    </div>
                    

                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <cc1:ModalPopupExtender ID="mpeEid" BackgroundCssClass="bgPopup" PopupControlID="pnleID" TargetControlID="btnDummyButton" runat="server"></cc1:ModalPopupExtender>
                   

                    <asp:Button ID="btnDummyButton" runat="server" Style="display:none;" />
           
                    <asp:Panel ID="pnleID" runat="server" Style="display:none;">
                        <cc2:EID_Read ID="EID_Read1" runat="server" Visible="false"/>
                    </asp:Panel>

                   



                </fieldset>
            </div>
        </div>
        TODO: gebruikersgegevens laden via eID
    </div>
    BUG: elke button activeert de validators (bv "Sign in" uitvoeren vanaf deze pagina)
</asp:Content>
