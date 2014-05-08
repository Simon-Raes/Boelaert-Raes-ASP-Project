<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="LayeredBusinessModel.WebUI.signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel panel-default">
        <div class="panel-heading"><h3>Sign up</h3></div>
        <div class="panel-body">            
            

            <div class="panel panel-info">
                <div class="panel-heading">Account</div>
                <div class="panel-body">
                    <div class="input-group">
                        <span class="input-group-addon">Email</span>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                    </div>
                <div class="panel-body">
                    <div class="input-group">
                        <span class="input-group-addon">Password</span>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                    </div>
                <div class="panel-body">
                    <div class="input-group">
                        <span class="input-group-addon">Confirm password</span>
                        <asp:TextBox ID="txtpassword2" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="panel panel-info">
                <div class="panel-heading">Personal</div>
                <div class="panel-body">
                    Panel content
                </div>
            </div>




        </div>

        
    </div>


</asp:Content>
