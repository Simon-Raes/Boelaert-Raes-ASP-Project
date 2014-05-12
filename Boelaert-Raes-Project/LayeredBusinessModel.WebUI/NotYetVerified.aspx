<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="NotYetVerified.aspx.cs" Inherits="LayeredBusinessModel.WebUI.NotYetVerified" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default" id="Div1" runat="server">
            <div class="panel-body">
                <h2>This account has not yet been verified</h2>
                You should have received a verification email when you created your Taboelaert Raesa account. <br />Please click the link in the email to verify your email address and complete your registration.<br /><br />
                Lost the email? <asp:LinkButton ID="btnResendVerification" runat="server" OnClick="btnResendVerification_Click">Click here and we'll send you a new one.</asp:LinkButton>
            </div>
        </div>

</asp:Content>
