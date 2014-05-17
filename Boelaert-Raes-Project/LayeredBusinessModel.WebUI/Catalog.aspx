<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Catalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default">
        <div class="panel-body">

            <asp:Label ID="lblHeader" runat="server" Text="Label" Font-Size="20"></asp:Label>

            <div class="col-lg-4 pull-right">
                <div class="input-group">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch2">
                    <asp:TextBox ID="txtSearchNew" runat="server" CssClass="form-control"></asp:TextBox>
                    <span class="input-group-btn">                           
                        <asp:LinkButton ID="btnSearch2" runat="server" CssClass="btn btn-default" OnClick="btnSearch2_Click"><span class="glyphicon glyphicon-search"></span></asp:LinkButton>         
                     
                    </span>
                        </asp:Panel>
                </div>
            </div>

            <br /><br />

            <div class="row" id="catalogContent" runat="server">
            </div>
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
