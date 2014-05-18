<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            <ul id="myTab" class="nav nav-tabs">
                <li class="active"><a href="Overview.aspx">Overview</a></li>
                <li><a href="Orders.aspx">Orders</a></li>
                <li><a href="Settings.aspx">Settings</a></li>
                <li class="pull-right"><button id="Button1" runat="server" class="btn btn-warning" onserverclick="btnLogOut_Click" causesvalidation="false">Sign out</button></li>
            </ul>

            <br />

            <asp:Label ID="lblOrderTotal" runat="server" Text="Label"></asp:Label><br /><br />         
            <asp:Label ID="lblActiveRentCopies" runat="server" Text="Label"></asp:Label>
            <asp:GridView ID="gvActiveRent" CssClass="gridViewStyle" runat="server" OnRowDataBound="gvActiveRent_RowDataBound"></asp:GridView>       

        </div>
    </div>

</asp:Content>
