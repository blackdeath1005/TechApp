<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.DETALLE_SOLICITUD_ATENCION>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Marca</th>
                    <th class="col-md-2 background">Modelo</th>
                    <th class="background">Serie</th>
                    <th class="background">Problema</th>
                    <th class="background">Error</th>
                    <th class="background">Garantia</th>
                    <th class="background">Observaciones</th>
                    <th class="background">Acciones</th>
                </tr>
            <% foreach (var item in Model) {
               var garantia = "No";
               var contratos = item.EQUIPO.CONTRATO.ToList();
               
               if (contratos.Count()>0) {
                   foreach (var contrato in contratos) {
                       if(contrato.Tx_Estado.ToString().Equals("Activo")) {
                           garantia = "Si";
                       }
                   }
               } %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Marca) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Modelo) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Serie) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Problema) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Nu_Error) %></td>
                    <td class="vert-align"><%: garantia %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Observaciones) %></td>
                    <td class="vert-align"><a href="#" data-toggle="tooltip" title="Ver Contrato"><span class="glyphicon glyphicon-search" style="color:#808080"></span></a></td>
                </tr>
            <% } %>
    </table>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
</script>