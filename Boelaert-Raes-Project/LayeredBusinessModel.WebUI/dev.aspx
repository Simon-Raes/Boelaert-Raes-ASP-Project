<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="dev.aspx.cs" Inherits="LayeredBusinessModel.WebUI.dev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">            
            <asp:Button ID="btnResetCopies" runat="server" OnClick="btnResetCopies_Click" Text="RESET" />
            &nbsp;Zet alle dvdcopies terug op in_stock = true en wist alle shoppingcarts, orders en orderlines.
        </div>
    </div>
    
</asp:Content>
