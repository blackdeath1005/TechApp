<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.DETALLE_SOLICITUD_ATENCION>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Marca</th>
                    <th class="col-md-2 background">Modelo</th>
                    <th class="background">Serie</th>
                    <th class="background">Problema</th>
                    <th class="background">Error</th>
                    <th class="background">Observaciones</th>
                    <th class="col-md-2 background">Acciones</th>
                </tr>
            <% foreach (var item in Model) { %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Marca) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Modelo) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Serie) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Problema) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Nu_Error) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Observaciones) %></td>
                    <td class="vert-align"><ul class="nav nav-pills">
                        <li class="dropdown">
                            <a href="#" data-toggle="dropdown" class="dropdown-toggle table-pills">Acciones <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li>
                                    <a href="<%:Url.Action("EditDetails", "SolicitudAtencion", new { id=item.Co_Solicitud, id2=item.Co_Equipo }) %>" data-toggle="modal" data-backdrop="static" data-target="#EditDetailModal">
                                        <span class="glyphicon glyphicon-edit"></span>
                                        Editar
                                    </a>
                                </li>
                                <li>
                                    <a href="#" data-id="<%:item.Co_Solicitud%>" data-id2="<%:item.Co_Equipo%>" data-toggle="modal" data-backdrop="static" data-target="#DeleteDetailModal">
                                        <span class="glyphicon glyphicon-trash"></span>
                                        Borrar
                                    </a>
                                </li>
                            </ul>
                        </li>
                        </ul>
                    </td>
                </tr>
            <% } %>
    </table>
</div>

<!-- Modal -->
<div class="modal fade" data-refresh="true" id="EditDetailModal" tabindex="-1" role="dialog" aria-labelledby="EditDetailModalLabel">
    <div class="modal-dialog modal-lg" role="document">
    <div class="modal-content">

    </div>
    </div>
</div>
<!-- End Modal -->

<!-- Modal -->
<div class="modal fade" id="DeleteDetailModal" tabindex="-1" role="dialog" aria-labelledby="DeleteDetailModalLabel">
    <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h4 class="modal-title" id="DeleteDetailModalLabel">¿Desea eliminar el equipo de la Solicitud de Atencion?</h4>
      </div>
      <div class="modal-footer">
        <a href="#" id="deleteModalbtn" class="btn btn-primary btn-sm">Aceptar</a>
        <a class="btn btn-default btn-sm" data-dismiss="modal">Cancelar</a>
      </div>
    </div>
    </div>
</div>
<!-- End Modal -->

<script type="text/javascript">
    $("a[data-target=#EditDetailModal]").click(function (ev) {
        ev.preventDefault();
        var target = $(this).attr("href");
        // load the url and show modal on success
        $("#EditDetailModal .modal-content").load(target, function () {
            $("#EditDetailModal").modal("show");
        });
    });
    $(document).on('hidden.bs.modal', function (e) {
        if ($(e.target).attr('data-refresh') == 'true') {
            // Remove and Empty modal data
            $(e.target).removeData("bs.modal").find(".modal-content").empty();
        }
    });
</script>
<script type="text/javascript">
    //triggered when modal is about to be shown
    $('#DeleteDetailModal').on('show.bs.modal', function (e) {
        //get id attribute of the clicked element
        var id = $(e.relatedTarget).data('id');
        var id2 = $(e.relatedTarget).data('id2');
        //create url with parameters and load href
        var url = '<%: Url.Action("DeleteDetails", "SolicitudAtencion")%>?id=' + id + '&id2=' + id2;
        $("#deleteModalbtn").attr("href", url)

    });
</script>

