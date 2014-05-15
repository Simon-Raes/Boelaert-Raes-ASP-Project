<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:panel id="pnlError" runat="server" visible="false">
        <asp:label id="lblError" runat="server" text="Oops! An error occurred while performing your request. Sorry for any convenience."></asp:label><br /><br />
        <asp:label id="Label1" runat="server" text="You may want to get back to the previous page and perform other activities."></asp:label><br /><br />
        <asp:hyperlink id="hlinkPreviousPage" runat="server">Go back</asp:hyperlink>
    </asp:panel>
    


</asp:Content>
