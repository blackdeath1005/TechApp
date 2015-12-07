<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.CASO_SERVICIO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrar Caso de Servicio Nuevo</h2>

<% using (Html.BeginForm("Create", "CasoServicio", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
    <div class="form-group">
      <label class="col-md-2 control-label" for="Fe_Caso">Fecha Caso</label>  
      <div class="col-md-2">
           <p class="form-control-static"><%: DateTime.Today.ToShortDateString() %></p>
      </div>
      <label class="col-md-2 control-label">Cod Solicitud</label>  
      <div class="col-md-2">
        <div class="input-group">
           <%: Html.TextBox("codSolicitud", ViewData["codSolicitud"], new { @class = "form-control input-md", @readonly="readonly" }) %>
        </div>
      </div>
    </div>

    <!-- TextArea input-->
    <div class="form-group">
      <label class="col-md-2 control-label" for="Tx_Observaciones">Observaciones</label>
      <div class="col-md-2">
       <%: Html.TextAreaFor(model => model.Tx_Observaciones, new { @class = "form-control input-md" }) %>
      </div>
      <label class="col-md-2 control-label">N° Equipos</label>  
      <div class="col-md-2">
        <div class="input-group">
           <%: Html.TextBox("nEquipos", ViewData["nEquipos"], new { @class = "form-control input-md", @readonly="readonly" }) %>
        </div>
      </div>
    </div>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
          <%: Html.Hidden("No_UsuarioIni", Page.User.Identity.Name) %>
      </div>
      <div class="col-md-6">
          <%: Html.Hidden("No_UsuarioMod", Page.User.Identity.Name) %>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-2 control-label"></label>
      <div class="col-md-4">
        <input type="submit" value="Crear" class = "btn btn-warning"/>
        &nbsp;
        <%: Html.ActionLink("Cancelar", "SearchSolicitud", "CasoServicio", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>
        
    </fieldset>
    
<% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
</asp:Content>
