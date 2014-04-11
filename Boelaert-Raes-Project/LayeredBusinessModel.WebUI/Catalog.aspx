<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Catalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Label ID="lblHeader" runat="server" Text="Label" Font-Size="20"></asp:Label>

    
    <br />
    Search:
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="Search" CausesValidation="False" OnClick="Button1_Click1" />
    Deze button is kapot en ik weet niet waarom.

    <br />
    

    <div class="row" id="catalogContent" runat="server">

    </div>
    
</asp:Content>
