<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.TECNICO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index Tecnicos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Asignar Tecnicos a Caso de Servicio</h2>

<% using (Html.BeginForm("IndexGPSTecnico", "CasoServicio", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
          <p class="text-danger"><strong><span id="txtError"></span></strong></p> 
      </div>
    </div>

    <!-- Text input-->
    <div class="form-group">
      <label class="col-md-2 control-label">Codigo Tecnico</label>  
      <div class="col-md-2">
        <%: Html.TextBoxFor(model => model.Co_Tecnico, new { @class = "form-control input-md" }) %>
      </div>
    </div>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">Ingresar código de técnico (1 a 4) luego crear un marcador en el mapa para grabar su posición GPS de ese técnico
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-3 control-label"></label>
      <div class="col-md-5">
        <input type="submit" id="btn_Asignar" value="Grabar GPS" class = "btn btn-warning"/>
        &nbsp;
        <%: Html.ActionLink("Cancelar", "Index", "Home", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>

    </fieldset>
    
<% } %>


    <div class="container">
        <div id="map_canvas" style="width:600px;height:400px;"></div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script type="text/javascript">

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Initialize();
        });

        var gMap;
        var markers = [];
        function Initialize() {
            var mapOptions = {
                center: new google.maps.LatLng(-12.1042770, -77.0269050),
                zoom: 16,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            infoWindow = new google.maps.InfoWindow();
            gMap = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);

            google.maps.event.addListener(gMap, 'click', function (event) {
                placeMarker(event.latLng);
            });
        }

        function placeMarker(location) {
            var marker = new google.maps.Marker({
                position: location,
                map: gMap,
            });
            deleteMarkers();
            markers.push(marker);
            var infowindow = new google.maps.InfoWindow({
                content: 'Latitude: ' + location.lat() + '<br>Longitude: ' + location.lng()
            });
            infowindow.open(gMap, marker);
            grabarGPS(location.lat(), location.lng());
        }

        google.maps.event.addDomListener(window, 'load', Initialize);

        function setMapOnAll(map) {
            for (var i = 0; i < markers.length; i++) {
                markers[i].setMap(map);
            }
        }
        function deleteMarkers() {
            setMapOnAll(null);
            markers = [];
        }
        function grabarGPS(lat, lng) {
            var cod = $("#Co_Tecnico").val();
            $.ajax({
                url: '<%:Url.Action("IndexGPSTecnico", "CasoServicio")%>',
                type: 'POST',
                data: { "latitud": lat, "longitud": lng, "codigo": cod },
                dataType: 'json',
                success: function (response) {
                    if (response.resp) {
                        $('#txtError').html(response.mensaje);
                    }
                    else {
                        $('#txtError').html(response.mensaje);
                    }
                },
                error: function (error) {
                    alert("error");
                }
            });
        }

    </script>


</asp:Content>