<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Catalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    Search:
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <br />
    Category:
    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
    </asp:DropDownList>
    &nbsp;hier nog een bug als je een categorie maar GEEN genre kiest<br />
    Genre:
    <asp:DropDownList ID="ddlGenre" runat="server">
    </asp:DropDownList>
    &nbsp;<br />
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br />
    <asp:GridView ID="gvDvdInfo" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDvdInfo_RowCommand">
        <Columns>
            <asp:BoundField DataField="dvd_info_id" HeaderText="dvd_info_id" />
            <asp:BoundField AccessibleHeaderText="Title" DataField="name" HeaderText="Title" />
            <asp:BoundField AccessibleHeaderText="Director" DataField="author" HeaderText="Director" />
            <asp:BoundField AccessibleHeaderText="Year" DataField="year" HeaderText="Year" />
            <asp:BoundField AccessibleHeaderText="Barcode" DataField="barcode" HeaderText="Barcode" />
            <asp:HyperLinkField AccessibleHeaderText="Info" HeaderText="Info" NavigateUrl="~/tempMovie.aspx" Text="Info" />
            <asp:ButtonField ButtonType="Button" Text="Buy" CommandName="Buy" />

        </Columns>
    </asp:GridView>
</asp:Content>
