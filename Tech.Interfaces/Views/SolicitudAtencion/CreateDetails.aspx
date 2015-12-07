<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.DETALLE_SOLICITUD_ATENCION>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CreateDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Registrar Detalle de Solicitud de Atención # <%: ViewData["Co_Solicitud_Origen"] %></h2>

<% using (Html.BeginForm("CreateDetails", "SolicitudAtencion", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
        <%: Html.Hidden("Co_Solicitud_Origen", ViewData["Co_Solicitud_Origen"]) %>
      </div>
    </div>

    <!-- Text input-->
    <div class="form-group">
      <label class="col-md-2 control-label" for="Nu_Error"># Error</label>  
      <div class="col-md-2">
        <%: Html.TextBoxFor(model => model.Nu_Error, new { @class = "form-control input-md" }) %>
      </div>
      <label class="col-md-2 control-label" for="Tx_Problema">Problema</label>  
      <div class="col-md-3">
        <%: Html.TextAreaFor(model => model.Tx_Problema, new { @class = "form-control input-md" , @rows = 4}) %>
      </div>
    </div>

    <!-- TextArea input-->
    <div class="form-group">
      <label class="col-md-2 control-label" for="Tx_Observaciones">Observaciones</label>  
      <div class="col-md-3">
        <%: Html.TextAreaFor(model => model.Tx_Observaciones, new { @class = "form-control input-md" , @rows = 4}) %>
      </div>
    </div>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">
        <p class="text-danger small">(*) Campos Obligatorios</p>
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-4 control-label"></label>
      <div class="col-md-5">
        <input type="submit" value="Agregar" class = "btn btn-warning"/>
        &nbsp;
        <%: Html.ActionLink("Cancelar", "Index", "Home", routeValues: null, htmlAttributes: new { @class="btn btn-default" }) %>
      </div>
    </div>
        
    </fieldset>
    
<% } %>
    &nbsp;
    &nbsp;
    &nbsp;
    <div class="container">
        <%: Html.Action("IndexDetails","SolicitudAtencion", new {id = ViewData["Co_Solicitud_Origen"]} ) %>
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
                url: '<%:Url.Action("GetModelo", "SolicitudAtencion")%>',
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
        function llenarSeries(marca,modelo) {
            var selectedMarca = marca;
            var selectedModelo = modelo;
            $.ajax({
                url: '<%:Url.Action("GetSerie", "SolicitudAtencion")%>',
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
</asp:Content>
