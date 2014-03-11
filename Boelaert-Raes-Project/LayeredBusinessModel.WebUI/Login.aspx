<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><br />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
<br />
<asp:Label ID="lblStatus" runat="server"></asp:Label>
<br />
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    <br />
    Or click here to log-in with your eID.
    <br />
    Don't have an account? Sign up here: <asp:Button ID="btnRegister" runat="server" Text="Sign up" OnClick="btnRegister_Click" />
</asp:Content>
