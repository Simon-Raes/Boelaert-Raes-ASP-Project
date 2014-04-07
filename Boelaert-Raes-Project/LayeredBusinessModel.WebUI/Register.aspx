<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <p>
        Register:
    </p>

    <table>
        <tr>
            <td>Name*</td>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="valReqName" runat="server" ControlToValidate="txtName" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
            </td>

        </tr>
        <tr>
            <td>Email*</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="valReqEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="valCustEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="An account with that address already exists." OnServerValidate="valCustEmail_ServerValidate"></asp:CustomValidator>
                <asp:RegularExpressionValidator ID="valRegEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Not a valid e-mail address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>

        </tr>
        <tr>
            <td class="auto-style1">Login name*</td>
            <td class="auto-style1">
                <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
            </td>
            <td class="auto-style1">
                <asp:RequiredFieldValidator ID="valReqLogin" runat="server" ControlToValidate="txtLogin" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="valCustLogin" runat="server" ControlToValidate="txtLogin" ErrorMessage="That name is already taken." OnServerValidate="valCustLogin_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator>
            </td>

        </tr>
        <tr>
            <td>Password*</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="valReqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td>Re-enter Password*</td>
            <td>
                <asp:TextBox ID="txtPasswordAgain" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="valReqPasswordAgain" runat="server" ControlToValidate="txtPasswordAgain" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="valEqualPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtPasswordAgain" ErrorMessage="Password fields must be equal.">Password fields must be equal.</asp:CompareValidator>
            </td>

        </tr>
    </table>



    
        <asp:Button ID="btnRegister" runat="server" Text="Sign up" OnClick="btnRegister_Click" />
    
</asp:Content>
