<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.TECNICO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index Tecnicos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Asignar Tecnico a Detalle #<%: ViewData["CodDetalleCaso"] %> del Caso de Servicio #<%: ViewData["CodCaso"] %></h2>

<% using (Html.BeginForm("IndexTecnico", "CasoServicio", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
        <% if (ViewData["EquipoFuera"] != null) { %>
            <div class="alert alert-info">
                <strong><%: ViewData["EquipoFuera"] %></strong>
            </div>
        <% }
            if ( ViewData["Error"]!=null) { %>
                <p class="text-danger"><strong><%: ViewData["Error"] %></strong></p> 
            <% }
            else { %>
                <p class="text-success"><strong><%: ViewData["Ok"] %></strong></p>
            <% } %>
      </div>
    </div>

    <% if ( ViewData["EquipoFuera"]!=null) { %>
        <!-- Text input-->
        <div class="form-group required">
            <label class="col-md-2 control-label">Tecnico</label>
            <div class="col-md-3">
                <%: Html.DropDownListFor(model => model.Co_Tecnico, (SelectList)ViewBag.dsTecnico, "------------Seleccione------------", new { @class = "form-control input-md", @required="" }) %>
            </div>
        </div>
    <% }
    else { %>
        <!-- Text input-->
        <div class="form-group">
            <label class="col-md-2 control-label">Tecnico Propuesto</label>
            <div class="col-md-2">
                <p class="form-control-static"><%: ViewData["NomTecnico"] %></p>
            </div>

        </div>

        <!-- Text input-->
        <div class="form-group">
            <label class="col-md-2 control-label">Distancia al Servicio<br/> (A -> B)</label>
            <div class="col-md-2">
                <p class="form-control-static"><%: ViewData["Distancia"] %> metros</p>
            </div>
        </div>

        <!-- Text input-->
        <div class="form-group required">
            <label class="col-md-2 control-label">Tecnico</label>
            <div class="col-md-3">
                <%: Html.DropDownListFor(model => model.Co_Tecnico, (SelectList)ViewBag.dsTecnico, "------------Seleccione------------", new { @class = "form-control input-md", @required="" }) %>
            </div>
        </div>
    <% } %>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
            <%: Html.Hidden("No_UsuarioMod", Page.User.Identity.Name) %>
      </div>
      <div class="col-md-6">
          <p class="text-danger small">(*) Campos Obligatorios</p>
            <%: Html.Hidden("CodDetalleCaso", ViewData["CodDetalleCaso"]) %>
            <%: Html.Hidden("CodCaso", ViewData["CodCaso"]) %>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-3 control-label"></label>
      <div class="col-md-5">
        <input type="submit" value="Asignar" class = "btn btn-warning"/>
        &nbsp;
        <%: Html.ActionLink("Cancelar", "SearchCaso", "CasoServicio", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>

    </fieldset>
    
<% } %>
    &nbsp;
    &nbsp;
    <% if ( ViewData["EquipoFuera"]==null) { %>
        <div class="container">
            <div id="map" style="width:600px;height:400px;"></div>
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            Initialize();
        });

        function Initialize() {
            var directionsDisplay = new google.maps.DirectionsRenderer;
            var directionsService = new google.maps.DirectionsService;
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 16,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            directionsDisplay.setMap(map);

            calculateAndDisplayRoute(directionsService, directionsDisplay);
        }

        function calculateAndDisplayRoute(directionsService, directionsDisplay) {   
            var latOri = '<%: ViewData["LatOrigen"] %>';
            var lngOri = '<%: ViewData["LngOrigen"] %>';
            var latDes = '<%: ViewData["LatDestino"] %>';
            var lngDes = '<%: ViewData["LngDestino"] %>';
            var start = new google.maps.LatLng(latOri, lngOri);
            var end = new google.maps.LatLng(latDes, lngDes);

            directionsService.route({
                origin: start,
                destination: end,
                travelMode: google.maps.TravelMode.DRIVING
            }, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                } else {
                    window.alert('Pedido de Direccion fallo por ' + status);
                }
            });
        }
        
    </script>
    <script type="text/javascript">

    </script>
</asp:Content>