<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="LayeredBusinessModel.WebUI.signup" %>

<%@ Register Assembly="Arcabase.EID.SDK" Namespace="Arcabase.EID.SDK.Web" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default" id="pnlSignup" runat="server">
        <div class="panel-heading"><h3>Sign up</h3></div>
        <div class="panel-body">            
            

            <div class="panel panel-info">
                <div class="panel-heading">Account</div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Email</span>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ></asp:TextBox>                        
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required!" ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="An account with that address already exists." OnServerValidate="valCustEmail_ServerValidate" Display="Dynamic" ForeColor="#CC3300"></asp:CustomValidator>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Password</span>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required!" ControlToValidate="txtpassword" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPasswords" runat="server" ErrorMessage="Passwords must match!" ControlToCompare="txtPassword2" ControlToValidate="txtPassword"></asp:CompareValidator>
                            </p>
                        </div>
                    </div>
               </div>



                <div class="panel-body">
                     <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Confirm password</span>
                                <asp:TextBox ID="txtpassword2" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ErrorMessage="Password confirmation is required!" ControlToValidate="txtpassword2" Display="Dynamic"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="panel panel-info">
                <div class="panel-heading">Personal </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Name</span>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required!" ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Street</span>
                                <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvStreet" runat="server" ErrorMessage="Street is required!" ControlToValidate="txtStreet" Display="Dynamic"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Postalcode</span>
                                <asp:TextBox ID="txtPostalcode" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvPostalcode" runat="server" ErrorMessage="Postalcode is required!" ControlToValidate="txtPostalcode" Display="Dynamic"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                     <div class="row">
                        <div class="col-md-8">
                            <div class="input-group">
                                <span class="input-group-addon">Municipality</span>
                                <asp:TextBox ID="txtMunicipality" runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                        </div>
                         <div class="col-md-4">
                            <p class="help-block">
                                <asp:RequiredFieldValidator ID="rfvMunicipality" runat="server" ErrorMessage="Municipality is required!" ControlToValidate="txtMunicipality" Display="Dynamic"></asp:RequiredFieldValidator>
                            </p>
                        </div>
                    </div>
                </div>



                <div class="panel-body">
                    <div class="btn-group" style="float:right;">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btneID" PopupControlID="pnleID"></cc1:ModalPopupExtender>

                       <!--
                            <asp:Button ID="btneID" runat="server" Text="eID" CssClass="btn btn-info" OnClick="btneID_Click" CausesValidation="false" UseSubmitBehavior="false" />
                        -->
                    <asp:Panel ID="pnleID" runat="server" Style="display:none;">
                        <cc2:EID_Read ID="EID_Read1" runat="server" />
                    </asp:Panel>

                        
                    </div>
                </div>
                </div>
                <div class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-danger" OnClick="btnReset_Click" UseSubmitBehavior="false" CausesValidation="false"/>
                    </div>
                    
                    <div class="btn-group">
                        <asp:Button ID="btnSignup" runat="server" Text="Sign up" CssClass="btn btn-success" OnClick="btnSignup_Click" UseSubmitBehavior="false" CausesValidation="true"/>
                    </div>
                </div>
        </div>
    </div>

    <div id="pnlSignupCompleted" runat="server">
        Check uw mailbox (binnenkort)
    </div>


</asp:Content>
