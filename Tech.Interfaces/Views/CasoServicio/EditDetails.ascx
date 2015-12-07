<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Tech.DataAccess.DETALLE_CASO_SERVICIO>" %>

<div id="editFormResult"> </div>

<div id="editForm" class="container-modal">

    <h2>Editar Detalle de Caso de Servicio</h2>

    <% using (Html.BeginForm("EditDetails", "CasoServicio", FormMethod.Post, new { @class = "form-horizontal" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>

        <!-- Form Name -->
        <legend></legend>

        <!-- Error-->
        <div class="form-group">
          <div class="col-md-2">
          </div>
          <div class="col-md-7">
             <% if ( ViewData["Error"]!=null) { %>
              <p class="text-danger"><strong><%: ViewData["Error"] %></strong></p> 
            <% }
                else { %>
              <p class="text-success"><strong><%: ViewData["Ok"] %></strong></p>
             <% } %>
          </div>
        </div>
            
        <!-- Text input-->
        <div class="form-group" >
          <label class="col-md-2 control-label">Marca</label>  
          <div class="col-md-3">
            <p class="form-control-static"><%: Model.EQUIPO.Tx_Marca %></p>  
          </div>
          <label class="col-md-3 control-label">Modelo</label>  
          <div class="col-md-3">
            <p class="form-control-static"><%: Model.EQUIPO.Tx_Modelo %></p>
          </div>
        </div>

        <!-- Text input-->
        <div class="form-group" >
          <label class="col-md-2 control-label">Serie</label>
          <div class="col-md-3">
            <p class="form-control-static"><%: Model.EQUIPO.Tx_Serie %></p>
          </div>  
          <div class="col-md-3">
            <%: Html.Hidden("Co_Caso_Origen", ViewData["Co_Caso_Origen"]) %>
          </div>
        </div>

        <!-- TextArea input-->
        <div class="form-group">
          <label class="col-md-2 control-label">Fecha Inicial</label>  
          <div class="col-md-3">
            <div class="input-group">
            <%: Html.TextBox("fechaIni2", ViewData["fechaIni"], new { @class = "form-control input-md", @readonly="readonly" }) %>
            <a href="#" class="input-group-addon" id="clearfechaIni2"><span class="glyphicon glyphicon-remove"></span></a>
            </div>
          </div>
          <label class="col-md-3 control-label">Fecha Final</label>  
          <div class="col-md-3">
            <div class="input-group">
            <%: Html.TextBox("fechaFin2", ViewData["fechaFin"], new { @class = "form-control input-md", @readonly="readonly" }) %>
            <a href="#" class="input-group-addon" id="clearfechaFin2"><span class="glyphicon glyphicon-remove"></span></a>
            </div>  
          </div>
        </div>

        <!-- Text input-->
        <div class="form-group" >
          <label class="col-md-2 control-label">Tipo Servicio</label>  
          <div class="col-md-3">
            <%: Html.DropDownList("dsTipoServicioList", (SelectList)ViewData["dsTipoServicio"], new { @class = "form-control" } )%>
          </div>
        </div>

        <!-- Text input-->
        <div class="form-group" >
          <label class="col-md-2 control-label" for="Tx_Problema">Detalle Servicio</label>  
          <div class="col-md-4">
            <%: Html.TextAreaFor(model => model.Tx_DetalleServicio, new { @class = "form-control input-md", @rows = "3" }) %>
          </div>
          <label class="col-md-2 control-label" for="Nu_Error"># Error</label>
          <div class="col-md-3">
            <%: Html.TextBoxFor(model => model.Nu_Error, new { @class = "form-control input-md" }) %>
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
          <label class="col-md-5 control-label"></label>
          <div class="col-md-5">
            <input type="submit" id="btn_Edit" value="Grabar" class = "btn btn-warning"/>
            &nbsp;
            <a class="btn btn-default" data-dismiss="modal">Cancelar</a>
          </div>
        </div>
        
        </fieldset>
    
    <% } %>

</div>

<script type="text/javascript">
    $(function () {
        $('form').submit(function () {
            if ($(this).valid()) {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.id) {
                            window.location = '<%: Url.Action("CreateDetails", "CasoServicio")%>?id=' + result.id + '&msg=' + result.msg;
                        }
                        else {
                            $('#editForm').html('');
                            $('#editFormResult').html(result);
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
        $('#fechaIni2').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            ignoreReadonly: true
        });
    });
</script>
<script type="text/javascript">
    $(function () {
        $('#fechaFin2').datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            ignoreReadonly: true
        });
    });
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#clearfechaIni2').click(function (event) {
            $('#fechaIni2').val('');
        });
        $('#clearfechaFin2').click(function (event) {
            $('#fechaFin2').val('');
        });
    });
</script>
