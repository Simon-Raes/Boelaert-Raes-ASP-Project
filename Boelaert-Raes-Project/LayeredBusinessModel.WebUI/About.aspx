<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="LayeredBusinessModel.WebUI.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel panel-default">
        <div class="panel-body">
            This website was designed by Serafijn Boelaert and Simon Raes as an assignment for mr. Dewaele.<br />
            <br />
            Development was done with the ASP.NET 4.5 Framework. The database uses Microsoft SQL Server 2008.
            <br />
            The site layout was designed with Twitter Bootstrap.<br />
            <br />
            Hosting provided by SmarterASP.
            <br /><br />
            A digital manual can be found here: 
            
            
        </div>
        <div class="row centeredRow">

            <div class="col-lg-3">
                <a href="http://www.asp.net/">
                    <img src="http://logicum.co/wp-content/uploads/2013/06/asp_net.png" /></a>
            </div>
            <div class="col-lg-3">
                <a href="https://www.microsoft.com/en-us/server-cloud/products/sql-server/#fbid=TXDB2ToB_bo">
                    <img src="http://4thoughtmarketing.com/news/wp-content/uploads/2012/04/sqlserver2.png" /></a>
            </div>
            <div class="col-lg-3">
                <a href="http://getbootstrap.com/">
                    <img src="http://i.imgur.com/1kvtNkr.png" /></a>
            </div>
            <div class="col-lg-3">
                <a href="http://www.smarterasp.net/">
                    <img src="http://i.imgur.com/URIMBq7.png" /></a>
            </div>
        </div>
    </div>
</asp:Content>
