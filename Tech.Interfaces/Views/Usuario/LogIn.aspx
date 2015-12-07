<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.USUARIO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    LogIn - Pacifico
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h3>Ingrese Usuario y Contraseña</h3>

<% using (Html.BeginForm("LogIn", "Usuario", FormMethod.Post, new { @class = "form-horizontal" })) { %>
    <%: Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(true) %>

    <fieldset>

    <!-- Form Name -->
    <legend></legend>

    <!-- Error-->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">
        <p class="text-danger"><strong><%: ViewData["Error"] %></strong></p>
      </div>
    </div>

    <!-- Text input-->
    <div class="form-group required">
      <label class="col-md-2 control-label" for="No_Usuario">Usuario</label>  
      <div class="col-md-2">
       <%: Html.TextBoxFor(model => model.No_Usuario, new { @class = "form-control input-md" , @required="" }) %>
      </div>
    </div>

    <!-- Password input-->
    <div class="form-group required">
      <label class="col-md-2 control-label" for="Tx_Password">Password</label>
      <div class="col-md-2">
        <%: Html.PasswordFor(model => model.Tx_Password, new { @class = "form-control" , @required="" }) %>
      </div>
    </div>

    <!-- Required Fileds Message-->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">
        <p class="text-danger small">(*) Campos Obligatorios</p>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-2 control-label"></label>
      <div class="col-md-4">
        <input type="submit" value="Iniciar Sesión" class = "btn btn-warning"/>
        <%: Html.ActionLink("Cancelar", "Index", "Home", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>
        
    </fieldset>
    
<% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>