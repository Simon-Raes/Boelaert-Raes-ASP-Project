﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="NoResult.aspx.cs" Inherits="LayeredBusinessModel.WebUI.NoResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            <asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
