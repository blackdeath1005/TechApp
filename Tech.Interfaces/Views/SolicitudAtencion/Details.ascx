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
                </tr>
            <% foreach (var item in Model) { %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Marca) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Modelo) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Serie) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Problema) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Nu_Error) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Tx_Observaciones) %></td>
                </tr>
            <% } %>
    </table>
</div>