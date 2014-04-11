<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="LayeredBusinessModel.WebUI.detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-4">
            <asp:Image ID="imgDvdCoverFocus" runat="server" />
            <div class="row">
                <div class="col-md-4 tumbnail">
                    <asp:Image ID="imgDvdCover1" runat="server" />
                </div>
                <div class="col-md-4 tumbnail">
                    <asp:Image ID="imgDvdCover2" runat="server" />
                </div>
                <div class="col-md-4 tumbnail">
                    <asp:Image ID="imgDvdCover3" runat="server" />
                </div>


        
            </div>
        </div>
        <div class="col-md-8">
            <asp:Label ID="lblID" runat="server" Text="Label"></asp:Label>
        </div>
    
        
    </div>
</asp:Content>
