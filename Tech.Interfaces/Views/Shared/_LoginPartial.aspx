<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<% if (Request.IsAuthenticated) { %>
    <small>Bienvenido, <b><%: Page.User.Identity.Name %></b></small>
    <p><small>&nbsp;
    &nbsp;&nbsp;
    <%: Html.ActionLink("Cerrar sesión", "LogOff", "Usuario", routeValues: null, htmlAttributes: new { onclick = "return confirm('¿Desea cerrar sesión?')" })%></small></p>
<% } else { %>
        <p><small><%: Html.ActionLink("Iniciar Sesión", "LogIn", "Usuario")%>
        &nbsp;|&nbsp;
        <a href="#">Registrarse</a></small></p>
<% } %>