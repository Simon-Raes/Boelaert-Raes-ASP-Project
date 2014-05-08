<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Catalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default">
        <div class="panel-body">

            <asp:Label ID="lblHeader" runat="server" Text="Label" Font-Size="20"></asp:Label>

            <div class="col-lg-4 pull-right">
                <div class="input-group">
                    <input id="txtSearchNew" runat="server" type="text" class="form-control" placeholder="Search" />
                    <span class="input-group-btn">
                        <button id="btnHtml" runat="server" onserverclick="btnSearch_Click2" class="btn btn-default">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </span>
                </div>
            </div>

            <br /><br />

            <div class="row" id="catalogContent" runat="server">
            </div>
            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
