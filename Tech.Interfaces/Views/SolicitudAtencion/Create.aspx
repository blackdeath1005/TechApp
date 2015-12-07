<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.SOLICITUD_ATENCION>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrar Solicitud de Atención Nueva</h2>

<% using (Html.BeginForm("Create", "SolicitudAtencion", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
      <label class="col-md-2 control-label" for="Fe_Solicitud">Fecha Solicitud</label>  
      <div class="col-md-2">
           <p class="form-control-static"><%: DateTime.Today.ToShortDateString() %></p>
      </div>
    </div>

    <!-- TextArea input-->
    <div class="form-group">
      <label class="col-md-2 control-label" for="Tx_Observaciones">Observaciones</label>  
      <div class="col-md-2">
       <%: Html.TextAreaFor(model => model.Tx_Observaciones, new { @class = "form-control input-md" }) %>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-2 control-label"></label>
      <div class="col-md-4">
        <input type="submit" value="Agregar" class = "btn btn-warning"/>
        &nbsp;
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
