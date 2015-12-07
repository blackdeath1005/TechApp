<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Tech.DataAccess.DETALLE_SOLICITUD_ATENCION>" %>

<div id="editFormResult"> </div>

<div id="editForm" class="container-modal">

    <h2>Editar Detalle de Solicitud de Atención</h2>

    <% using (Html.BeginForm("EditDetails", "SolicitudAtencion", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
          <label class="col-md-2 control-label">Modelo</label>  
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
            <%: Html.Hidden("Co_Equipo_Origen", ViewData["Co_Equipo_Origen"]) %>
          </div>
        </div>

        <!-- Text input-->
        <div class="form-group">
          <label class="col-md-2 control-label" for="Nu_Error"># Error</label>  
          <div class="col-md-3">
            <%: Html.TextBoxFor(model => model.Nu_Error, new { @class = "form-control input-md" }) %>
          </div>
          <label class="col-md-2 control-label" for="Tx_Problema">Problema</label>  
          <div class="col-md-4">
            <%: Html.TextAreaFor(model => model.Tx_Problema, new { @class = "form-control input-md" , @rows = 4}) %>
          </div>
        </div>

        <!-- TextArea input-->
        <div class="form-group">
          <label class="col-md-2 control-label" for="Tx_Observaciones">Observaciones</label>  
          <div class="col-md-4">
            <%: Html.TextAreaFor(model => model.Tx_Observaciones, new { @class = "form-control input-md" , @rows = 4}) %>
          </div>
        </div>

        <!-- Required Fileds Message -->
        <div class="form-group">
          <div class="col-md-2">
          </div>
          <div class="col-md-7">
          </div>
        </div>

        <!-- Button (Double) -->
        <div class="form-group">
          <label class="col-md-4 control-label"></label>
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
                            window.location = '<%: Url.Action("CreateDetails", "SolicitudAtencion")%>?id=' + result.id + '&msg=' + result.msg;
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