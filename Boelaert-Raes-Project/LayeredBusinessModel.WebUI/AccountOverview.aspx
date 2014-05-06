<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AccountOverview.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            <ul id="myTab" class="nav nav-tabs">
                <li class="active"><a href="AccountOverview.aspx">Overview</a></li>
                <li><a href="AccountOrders.aspx">Orders</a></li>
                <li><a href="AccountSettings.aspx">Settings</a></li>
                <li class="pull-right"><button id="Button1" runat="server" class="btn btn-warning" onserverclick="btnLogOut_Click">Sign out</button></li>
            </ul>
            <br />

            overview here
    

        </div>
    </div>

</asp:Content>
