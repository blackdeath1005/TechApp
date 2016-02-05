<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    TECH - Service Support
</asp:Content>

<asp:Content ID="indexFeatured" ContentPlaceHolderID="FeaturedContent" runat="server">
    <% if (Request.IsAuthenticated) { %> 
    <div class="content">
        <h2>Bienvenido a TECH - Web Service Support 2</h2>
    </div>

    <div class="content">

    </div>
    <% } %>
</asp:Content>