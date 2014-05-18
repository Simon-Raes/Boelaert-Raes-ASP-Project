<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="NotYetVerified.aspx.cs" Inherits="LayeredBusinessModel.WebUI.NotYetVerified" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default" id="Div1" runat="server">
        <div class="panel-body">
            <asp:Panel ID="pnlWithEmail" runat="server">
                <h2>This account has not yet been verified</h2>
                You should have received a verification email when you created your Taboelaert Raesa account.
                <br />
                Please click the link in the email to verify your email address and complete your registration.<br />
                <br />
                Lost the email?
                <asp:LinkButton ID="btnResendVerification" runat="server" OnClick="btnResendVerification_Click">Click here and we'll send you a new one.</asp:LinkButton>
            </asp:Panel>
            <asp:Panel ID="pnlNoEmail" runat="server" DefaultButton="btnSend">
                <h2>Enter your emailaddress</h2>
                <p>Enter the emailaddress you've used to create your account. We'll send you an email with a link for account activation.</p>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:LinkButton ID="btnSend" runat="server" OnClick="btnSend_Click" CssClass="btn btn-default">Send email</asp:LinkButton>
                <asp:Label ID="txtEmailError" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>
    </div>

</asp:Content>
