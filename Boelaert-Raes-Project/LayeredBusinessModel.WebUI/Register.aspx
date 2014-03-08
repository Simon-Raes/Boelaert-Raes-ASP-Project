<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <p>
        Register:
    </p>

    <table>
        <tr>
            <td>Name</td>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
            <td></td>

        </tr>
        <tr>
            <td>Email</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
            <td></td>

        </tr>
        <tr>
            <td>Login name</td>
            <td>
                <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
            </td>
            <td></td>

        </tr>
        <tr>
            <td>Password</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            </td>
            <td></td>
        </tr>

        <tr>
            <td>Re-enter Password</td>
            <td>
                <asp:TextBox ID="txtPasswordAgain" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:CompareValidator ID="valEqualPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtPasswordAgain" ErrorMessage="CompareValidator">Password fields must be equal.</asp:CompareValidator>
            </td>

        </tr>
    </table>



    <p>
        <asp:Button ID="btnRegister" runat="server" Text="Sign up" OnClick="btnRegister_Click" />
    </p>
</asp:Content>
