<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="LayeredBusinessModel.WebUI.Account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>
        &nbsp;
    </p>
    <table>
        <tr>
            <td style="background-color: azure">
                <asp:Menu ID="menuAccount" runat="server" OnMenuItemClick="menuAccount_MenuItemClick" BackColor="#E3EAEB" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#666666" StaticSubMenuIndent="10px">
                    <DynamicHoverStyle BackColor="#666666" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#E3EAEB" />
                    <DynamicSelectedStyle BackColor="#1C5E55" />
                    <Items>
                        <asp:MenuItem Text="Overview" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Order history" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Settings" Value="2"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#666666" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#1C5E55" />
                </asp:Menu>
            </td>
            <td>
                <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewOverview" runat="server">
                        overview of user info comes here
                    </asp:View>
                    <asp:View ID="viewOrders" runat="server">
                        <asp:GridView ID="gvOrders" runat="server" OnRowCommand="gvOrders_RowCommand">
                            <Columns>
                                <asp:ButtonField CommandName="Details" HeaderText="Details" Text="Details" />
                            </Columns>
                        </asp:GridView>
                        <asp:Panel ID="pnlOrderDetails" runat="server" Visible="False">
                            <br />
                            Order number:
                            <asp:Label ID="lblOrderID" runat="server" Text="Label"></asp:Label>
                            <br />
                            Order status:
                            <asp:Label ID="lblOrderStatus" runat="server" Text="Label"></asp:Label>(1 = new, 2 = paid, 3 = shipped) //todo: text instead of numbers<br />
                            <asp:Label ID="lblOrderStatusDetails" runat="server" Text=""></asp:Label><br />
                            
                            <br />
                            Click here to pay for this order:<asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click" Text="Pay" />
                            &nbsp;//TODO: payment page<br />
                            Your order will be processed and shipped as soon as the payment has been received.<asp:GridView ID="gvOrderDetails" runat="server">

                                
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        Geplande verhuurcopies misschien in een aparte lijst zetten, moeten tot 2 dagen voor verhuur geannuleerd kunnen worden.<br />
                        &nbsp;&nbsp;
                    </asp:View>
                    <asp:View ID="viewSettings" runat="server">

                        <table>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="valReqName" runat="server" ControlToValidate="txtName" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td>Email</td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="valReqEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="valRegEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Not a valid e-mail address." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                </td>

                            </tr>

                            <tr>
                                <td>Password</td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="valReqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Re-enter Password</td>
                                <td>
                                    <asp:TextBox ID="txtPasswordAgain" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="valReqPasswordAgain" runat="server" ControlToValidate="txtPasswordAgain" ErrorMessage="Field can't be empty."></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="valEqualPassword" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtPasswordAgain" ErrorMessage="Password fields must be equal.">Password fields must be equal.</asp:CompareValidator>
                                </td>

                            </tr>
                        </table>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                    </asp:View>
                </asp:MultiView></td>
        </tr>
    </table>


</asp:Content>
