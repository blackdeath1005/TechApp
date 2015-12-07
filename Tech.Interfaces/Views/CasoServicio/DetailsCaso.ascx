<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Tech.DataAccess.DETALLE_CASO_SERVICIO>>" %>

<div class="table-responsive">
    <table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th class="background">Marca</th>
                    <th class="background">Modelo</th>
                    <th class="background">Serie</th>
                    <th class="background">Ubicacion</th>
                    <th class="background">Tipo Servicio</th>
                    <th class="background">Tecnico</th>
                    <th class="background">Fecha Inicio</th>
                    <th class="background">Fecha Fin</th>
                    <th class="background"># Error</th>
                    <th class="background">Acciones</th>
                </tr>
            <% foreach (var item in Model) { %>
                <tr>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Marca) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Modelo) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Serie) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.EQUIPO.Tx_Ubicacion) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.TIPO_SERVICIO.No_Nombre) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.TECNICO.Tx_NomCompleto) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Fe_HoraInicial) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Fe_HoraFinal) %></td>
                    <td class="vert-align"><%: Html.DisplayFor(modelItem => item.Nu_Error) %></td>
                    <td class="vert-align">
                        <a href="<%:Url.Action("IndexTecnico", "CasoServicio", new { id=item.Co_Detalle_Caso }) %>">
                        <span class="glyphicon glyphicon-pencil"></span> Asignar
                        </a>
                    </td>
                </tr>
            <% } %>
    </table>
</div>

