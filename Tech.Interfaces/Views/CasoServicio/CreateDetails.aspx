<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.DETALLE_CASO_SERVICIO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CreateDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrar Detalle de Caso de Servicio # <%: ViewData["Co_Caso_Origen"] %></h2>

<% using (Html.BeginForm("CreateDetails", "CasoServicio", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
         <% if ( ViewData["Error"]!=null) { %>
          <p class="text-danger"><strong><%: ViewData["Error"] %></strong></p> 
        <% }
            else { %>
          <p class="text-success"><strong><%: ViewData["Ok"] %></strong></p>
         <% } %>
      </div>
    </div>
    
    <!-- Dropdown input-->
    <div class="form-group required" >
      <label class="col-md-2 control-label">Marca</label>
      <div class="col-md-2">
        <%: Html.DropDownListFor(model => model.EQUIPO.Tx_Marca, (SelectList)ViewBag.dsMarca, "-------Seleccione-------", new { @class = "form-control input-md", @required="" }) %>
      </div>
      <label class="col-md-2 control-label">Modelo</label>  
      <div class="col-md-2">
        <%: Html.DropDownListFor(model => model.EQUIPO.Tx_Modelo, (SelectList)ViewBag.dsModelo, "-------Seleccione-------", new { @class = "form-control input-md", @required="" }) %>
      </div>
    </div>

        <!-- Dropdown input-->
    <div class="form-group required" >
      <label class="col-md-2 control-label">Serie</label>  
      <div class="col-md-2">
        <%: Html.DropDownListFor(model => model.EQUIPO.Tx_Serie, (SelectList)ViewBag.dsSerie, "-------Seleccione-------", new { @class = "form-control input-md", @required="" }) %>
      </div>
      <div class="col-md-3">
        <%: Html.Hidden("Co_Caso_Origen", ViewData["Co_Caso_Origen"]) %>
      </div>
    </div>

    <!-- TextArea input-->
    <div class="form-group">
      <label class="col-md-2 control-label">Fecha Inicial</label>  
      <div class="col-md-2">
        <div class="input-group">
        <%: Html.TextBox("fechaIni", ViewData["fechaIni"], new { @class = "form-control input-md", @readonly="readonly" }) %>
        <a href="#" class="input-group-addon" id="clearfechaIni"><span class="glyphicon glyphicon-remove"></span></a>
        </div>
      </div>
      <label class="col-md-2 control-label">Fecha Final</label> 
      <div class="col-md-2">
        <div class="input-group">
        <%: Html.TextBox("fechaFin", ViewData["fechaFin"], new { @class = "form-control input-md", @readonly="readonly" }) %>
        <a href="#" class="input-group-addon" id="clearfechaFin"><span class="glyphicon glyphicon-remove"></span></a>
        </div>
      </div>
    </div>

    <!-- Text input-->
    <div class="form-group required" >
      <label class="col-md-2 control-label">Tipo Servicio</label>  
      <div class="col-md-2">
        <%: Html.DropDownList("dsTipoServicioList", (SelectList)ViewData["dsTipoServicio"], "-------Seleccione-------", new { @class = "form-control" } )%>
      </div>
    </div>

    <!-- Text input-->
    <div class="form-group" >
      <label class="col-md-2 control-label" for="Tx_Problema">Detalle Servicio</label>  
      <div class="col-md-3">
        <%: Html.TextAreaFor(model => model.Tx_DetalleServicio, new { @class = "form-control input-md", @rows = "3" }) %>
      </div>
      <label class="col-md-1 control-label" for="Nu_Error"># Error</label>
      <div class="col-md-2">
        <%: Html.TextBoxFor(model => model.Nu_Error, new { @class = "form-control input-md" }) %>
      </div>
    </div>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
          <%: Html.Hidden("No_UsuarioIni", Page.User.Identity.Name) %>
      </div>
      <div class="col-md-6">
        <p class="text-danger small">(*) Campos Obligatorios</p>
          <%: Html.Hidden("No_UsuarioMod", Page.User.Identity.Name) %>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-4 control-label"></label>
      <div class="col-md-5">
        <input type="submit" value="Agregar" class = "btn btn-warning"/>
        &nbsp;
        <%: Html.ActionLink("Cancelar", "SearchSolicitud", "CasoServicio", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>
        
    </fieldset>
    
<% } %>
    &nbsp;
    &nbsp;
    &nbsp;
    <div class="container">
        <%: Html.Action("IndexDetails","CasoServicio", new {id = ViewData["Co_Caso_Origen"]} ) %>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
    <%: Scripts.Render("~/bundles/jqueryval") %>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#EQUIPO_Tx_Marca").change(function () {
                llenarModelos($("#EQUIPO_Tx_Marca").val());
            });
        });
        function llenarModelos(value) {
            var selectedValue = value;
            $.ajax({
                url: '<%:Url.Action("GetModelo", "CasoServicio")%>',
                type: 'POST',
                data: { "selectedValue": selectedValue },
                dataType: 'json',
                success: function (response) {
                    var items = "";
                    var items2 = "<option value=\"\">-------Seleccione-------</option>";
                    items += "<option value=\"\">-------Seleccione-------</option>";
                    $.each(response.modelosLista, function (i, item) {
                        items += "<option value=\"" + item + "\">" + item + "</option>";
                    });
                    $("#EQUIPO_Tx_Modelo").html(items);
                    $("#EQUIPO_Tx_Serie").html(items2);
                },
                error: function (error) {
                    alert("error");
                }
            });
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#EQUIPO_Tx_Modelo").change(function () {
                llenarSeries($("#EQUIPO_Tx_Marca").val(), $("#EQUIPO_Tx_Modelo").val());
            });
        });
        function llenarSeries(marca, modelo) {
            var selectedMarca = marca;
            var selectedModelo = modelo;
            $.ajax({
                url: '<%:Url.Action("GetSerie", "CasoServicio")%>',
                type: 'POST',
                data: { "selectedMarca": selectedMarca, "selectedModelo": selectedModelo },
                dataType: 'json',
                success: function (response) {
                    var items = "";
                    items += "<option value=\"\">-------Seleccione-------</option>";
                    $.each(response.seriesLista, function (i, item) {
                        items += "<option value=\"" + item + "\">" + item + "</option>";
                    });
                    $("#EQUIPO_Tx_Serie").html(items);
                },
                error: function (error) {
                    alert("error");
                }
            });
        }
    </script>
    <script type="text/javascript">
        $(function () {
            var myCalendar = new dhtmlXCalendarObject(["fechaIni", "fechaFin"]);
            myCalendar.setDateFormat("%d-%m-%Y %H:%i");
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#clearfechaIni').click(function (event) {
                $('#fechaIni').val('');
            });
            $('#clearfechaFin').click(function (event) {
                $('#fechaFin').val('');
            });
        });
    </script>

</asp:Content>
