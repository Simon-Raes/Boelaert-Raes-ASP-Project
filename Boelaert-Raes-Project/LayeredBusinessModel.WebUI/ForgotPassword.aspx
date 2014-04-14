<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="LayeredBusinessModel.WebUI.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            Enter your e-mail adres or login name to reset your password.
            <div class="input-group col-md-4">
                <input id="txtPassword" runat="server" type="text" class="form-control" />
                <span class="input-group-btn">
                    <button id="btnReset" type="button" class="btn btn-default" runat="server" value="Reset" onserverclick="btnReset_Click">
                        <span>Reset</span>
                    </button>
                </span>
            </div>
            <br />
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
