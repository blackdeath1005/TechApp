<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.LOG_ERROR>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Codigo</th>
                    <th class="background">Marca</th>
                    <th class="background">Modelo</th>
                    <th class="background">Serie</th>
                    <th class="background">Cliente</th>
                    <th class="background">Ubicacion</th>
                    <th class="background">Fecha</th>
                    <th class="background">Log Origen</th>
                    <th class="col-md-2 background">Acciones</th>
                </tr>
            <% foreach (var item in Model) { %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Co_Error) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Marca) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Modelo) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Serie) %></td>                    
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.CLIENTE.Tx_RazonSocial) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Ubicacion) %></td>
                    <td class="vert-align"><%: String.Format("{0:g}", item.Fe_LogError) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Archivo) %></td>
                    <td class="vert-align"><ul class="nav nav-pills">
                        <li class="dropdown">
                            <a href="#" data-toggle="dropdown" class="dropdown-toggle table-pills">Acciones <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li>
                                    <a href="<%:Url.Action("Details", "ErrorLog", new { id=item.Co_LogError }) %>" data-toggle="modal" data-target="#DetailModal">
                                        <span class="glyphicon glyphicon-file"></span>
                                        Ver Detalle
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