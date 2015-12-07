<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.SOLICITUD_ATENCION>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Consultar Solicitudes de Atención</h2>

<% using (Html.BeginForm("Search", "SolicitudAtencion", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
      <label class="col-md-2 control-label">Codigo</label>  
      <div class="col-md-2">
        <%: Html.TextBox("codigo", ViewData["codigo"], new { @class = "form-control input-md" }) %>
      </div>
    </div>

    <!-- Text input-->
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
    <div class="form-group">
      <label class="col-md-2 control-label">Estado</label>
      <div class="col-md-2">
        <%: Html.DropDownList("estado", new SelectList( new List<string>{"Atendido","Cotizado","Observado","Pendiente","Rechazado"} ), "-------Seleccione-------", new { @class = "form-control input-md" }) %>
      </div>
    </div>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">
      </div>
    </div>

    <!-- Button (Double) -->
    <div class="form-group">
      <label class="col-md-3 control-label"></label>
      <div class="col-md-5">
        <input type="submit" id="btn_Buscar" value="Buscar" class = "btn btn-warning"/>
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
        <div id="searchFormResult"> </div>
        <div id="searchForm">
            <%: Html.Action("Index","SolicitudAtencion") %>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script type="text/javascript">
        $(function () {
            $('form').submit(function () {
                if ($(this).valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.resp) {
                                $('#searchForm').html('');
                                $('#txtError').html('');
                                $('#searchFormResult').html(result.Html);
                            }
                            else {
                                $('#txtError').html(result.msg);
                            }
                        }
                    });
                }
                return false;
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            var myCalendar = new dhtmlXCalendarObject(["fechaIni", "fechaFin"]);
            myCalendar.setDateFormat("%d-%m-%Y");
            myCalendar.hideTime();
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function() {                        
            $('#clearfechaIni').click(function (event) {
                $('#fechaIni').val('');
            });
            $('#clearfechaFin').click(function (event) {
                $('#fechaFin').val('');
            });
        });
    </script>


</asp:Content>
