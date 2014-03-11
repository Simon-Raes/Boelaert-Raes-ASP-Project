<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Catalog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    Search:
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br />
    <asp:GridView ID="gvDvdInfo" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDvdInfo_RowCommand">
        <Columns>
            <asp:BoundField DataField="dvd_info_id" HeaderText="dvd_info_id" />
            <asp:BoundField AccessibleHeaderText="Title" DataField="name" HeaderText="Title" />
            <asp:BoundField AccessibleHeaderText="Director" DataField="author" HeaderText="Director" />
            <asp:BoundField AccessibleHeaderText="Year" DataField="code" HeaderText="Year" />
            <asp:HyperLinkField AccessibleHeaderText="Info" HeaderText="Info" NavigateUrl="~/tempMovie.aspx" Text="Info" />
            <asp:CommandField AccessibleHeaderText="Buy" HeaderText="Buy" SelectText="Buy" ShowCancelButton="False" ShowSelectButton="True" />
            <asp:CommandField AccessibleHeaderText="Rent" HeaderText="Rent" SelectText="Rent" ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
