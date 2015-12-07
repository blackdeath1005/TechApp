<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.SOLICITUD_ATENCION>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Codigo</th>
                    <th class="background">Fecha</th>
                    <th class="background">Estado</th>
                    <th class="background">Observaciones</th>
                    <th class="col-md-2 background">Acciones</th>
                </tr>
            <% foreach (var item in Model) { %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Co_Solicitud) %></td>
                    <td class="vert-align"><%: String.Format("{0:d}", item.Fe_Solicitud) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Estado) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Observaciones) %></td>
                    <td class="vert-align"><ul class="nav nav-pills">
                        <li class="dropdown">
                            <a href="#" data-toggle="dropdown" class="dropdown-toggle table-pills">Acciones <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li>
                                    <a href="<%:Url.Action("Details", "SolicitudAtencion", new { id=item.Co_Solicitud }) %>" data-toggle="modal" data-target="#DetailModal">
                                        <span class="glyphicon glyphicon-file"></span>
                                        Detalle
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
<div class="modal fade" data-refresh="true" id="DetailModal" tabindex="-1" role="dialog" aria-labelledby="DetailModalLabel">
    <div class="modal-dialog modal-lg" role="document">
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