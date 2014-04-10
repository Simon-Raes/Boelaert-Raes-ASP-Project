<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="padding-s">

        <div class="page-title category-title">
            <h1>New Movies</h1>
        </div>

        <div class="row" id="newReleases" runat="server">           

        </div>
    </div>
    <br />


    <div class="padding-s">
        <br />
        <div class="page-title category-title">
            <h1>Most Popular</h1>
        </div>

        <div class="row" id="mostPopular" runat="server">           

        </div>

    </div>
       
    
    

</asp:Content>
