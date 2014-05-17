<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default" id="Div1" runat="server">
        <div class="panel-body">
            <asp:Panel ID="pnlStep1" runat="server">


            <h2>Contact</h2>

                <div class="form-group">
                    <label for="txtEmail">Email address</label>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required!" ForeColor="Red" ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtSubject">Subject</label>
                    <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ErrorMessage="Subject is required!" ForeColor="Red" ControlToValidate="txtSubject" Display="Dynamic"></asp:RequiredFieldValidator>
                     <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtMessage">Message</label>
                    <asp:RequiredFieldValidator ID="rfvmessage" runat="server" ErrorMessage="Message is required!" ForeColor="Red" ControlToValidate="txtMessage" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtMessage" runat="server" Rows="3" TextMode="MultiLine" Height="250px" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:LinkButton ID="btnSend" runat="server" CssClass="btn btn-default" OnClick="btnSend_Click">Send</asp:LinkButton>

           </asp:Panel>
            <asp:Panel ID="pnlStep2" runat="server">
                <p>We have received your request. We will contact you as soon as possible.</p>
            </asp:Panel>


        </div>
    </div>
</asp:Content>
