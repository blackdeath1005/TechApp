<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.CASO_SERVICIO>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Codigo</th>
                    <th class="background">Solicitud</th>
                    <th class="background">Fecha</th>
                    <th class="background">Estado</th>
                    <th class="background">Asignados</th>
                    <th class="background">Observaciones</th>
                    <th class="background">Acciones</th>
                </tr>
            <% foreach (var item in Model) {
                   var flag = item.DETALLE_CASO_SERVICIO.Where(x => x.Co_Tecnico != null).ToList().Count; 
                   %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Co_Caso) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Co_Solicitud) %></td>
                    <td class="vert-align"><%: String.Format("{0:d}", item.Fe_Caso) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Estado) %></td>
                    <td class="vert-align"><%: flag %> / <%: item.DETALLE_CASO_SERVICIO.Count %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Observaciones) %></td>
                    <td class="vert-align">
                        <a href="<%:Url.Action("DetailsCaso", "CasoServicio", new { id=item.Co_Caso }) %>" data-toggle="modal" data-target="#DetailModal">
                            <span class="glyphicon glyphicon-file"></span> Detalle
                        </a>
                    </td>
                </tr>
            <% } %>
    </table>
</div>

<!-- Modal -->
<div class="modal fade" data-refresh="true" id="DetailModal" tabindex="-1" role="dialog" aria-labelledby="DetailModalLabel">
    <div class="modal-dialog custom-lg" role="document">
    <div class="modal-content">
    </div>
    </div>
</div>
<!-- End Modal -->

<script type="text/javascript">
    $("a[data-target=#DetailModal]").click(function (ev) {
        ev.preventDefault();
        var target = $(this).attr("href");
        // load the url and show modal on success
        $("#DetailModal .modal-content").load(target, function () {
            $("#DetailModal").modal("show");
        });
    });
    $(document).on('hidden.bs.modal', function (e) {
        if ($(e.target).attr('data-refresh') == 'true') {
            // Remove and Empty modal data
            $(e.target).removeData("bs.modal").find(".modal-content").empty();
        }
    });
</script>