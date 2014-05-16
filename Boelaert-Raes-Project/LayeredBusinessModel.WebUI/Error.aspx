<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlError" runat="server" Visible="false">

        <div class="panel panel-default">
            <div class="panel-body">
                <asp:Label ID="lblError" runat="server" Text="Oops! An error occurred while performing your request. Sorry for any convenience."></asp:Label><br />
                <br />
                <asp:Label ID="Label1" runat="server" Text="You may want to get back to the previous page and perform other activities."></asp:Label><br />
                <br />
                <asp:HyperLink ID="hlinkPreviousPage" runat="server">Go back</asp:HyperLink>
            </div>
        </div>
    </asp:Panel>



</asp:Content>
