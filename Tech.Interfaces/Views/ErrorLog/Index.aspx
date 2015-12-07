<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Tech.DataAccess.LOG_ERROR>" %>
<%@ import Namespace = "System.IO"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Monitor Error Logs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Activar Monitorización de Eventos de Error</h2>

<% using (Html.BeginForm("Index", "ErrorLog", FormMethod.Post, new { @class = "form-horizontal" })) { %>
    <%: Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(true) %>

    <fieldset>

    <!-- Form Name -->
    <legend></legend>

    <!-- Error-->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div id="msg-monitoreo" class="col-md-6">
        <% if ( ViewData["Error"]!=null) { %>
            <p class="text-danger"><strong><%: ViewData["Error"] %></strong></p> 
        <% }
           else { %>
            <p class="text-success"><strong><%: ViewData["Ok"] %></strong></p>
        <% } %>
      </div>
    </div>

    <% if (ViewData["archivos"] != null)
       { %>
        <% var archivos = ViewData["archivos"] as FileInfo[];

           for (int i=0; i < archivos.Length; i++)
           {
               string nombre = archivos[i].Name;
               string path = archivos[i].FullName.Replace("\\", "\\\\");
            %>
            <!-- Upload input-->
            <div class="form-group">
                <label class="col-md-2 control-label">Archivo Log</label>
                <div class="col-md-2">
                    <p class="form-control-static"><%: nombre %></p>
                </div>
                <div class="col-md-2">
                    <button id="btnCargar<%: i %>" class="btn-sm btn-warning" onclick="return cargarArchivo('<%: path %>','<%: nombre %>')">Cargar Archivo</button>
                </div>
            </div>
        <% } %>
    <% } %>

    <!-- Required Fileds Message -->
    <div class="form-group">
      <div class="col-md-2">
      </div>
      <div class="col-md-6">
      </div>
    </div>

    </fieldset>
    
<% } %>

    <div class="form-group">
      <label class="col-md-2 control-label"></label>
       <button id="btnStart" class="btn btn-success" onclick="iniciarMonitoreo()"><span class="glyphicon glyphicon-play"></span> Iniciar</button>
        &nbsp;
        &nbsp;
       <button id="btnStop" class="btn btn-danger" onclick="detenerMonitoreo()"><span class="glyphicon glyphicon-stop"></span> Detener</button>
    </div>

    &nbsp;
    &nbsp;

    <div class="container">
        <div id="LogForm">
            <%: Html.Action("IndexDetails","ErrorLog") %>
        </div>
    </div>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsSection" runat="server">
    <script type="text/javascript">
        function cargarArchivo(fullPath, nombre) {
            window.location = '<%: Url.Action("UploadFile", "ErrorLog")%>?path=' + fullPath +'&nombre=' + nombre+'';
            return false;
        }        
    </script>
    <script type="text/javascript">
        var msjTimer = readCookie('timer');
        if (msjTimer != null) {
            document.getElementById("btnStart").disabled = true;
        }
        else {
            document.getElementById("btnStop").disabled = true;
        }
    </script>
    <script type="text/javascript">
        var timerLog = null;
        function iniciarMonitoreo() {            
            $.ajax({
                url: '<%: Url.Action("Start", "ErrorLog") %>',
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    if (result.resp) {
                        $('#msg-monitoreo').html('<p class="text-success"><strong>' + result.msg + '</strong></p>');

                        document.getElementById("btnStart").disabled = true; //Boton Iniciar Disable
                        document.getElementById("btnStop").disabled = false; //Boton Detener Enable

                        timerLog = setInterval(verificarFolder, 1000); //guarda el timer creado en la misma pagina

                        createCookie('timer', 'start', 1); //guarda cookie de flag de ejecucion del timer
                        createCookie('monitoreo', '<a href="<%: Url.Action("Index", "ErrorLog") %>"><span class="text-primary text-uppercase"><strong>Monitoreando...</strong></span></a>', 1);
                        //guarda cookie de html del mensaje monitoreando
                        document.getElementById('button-alert').innerHTML =
                            '<a href="#"><span class="text-primary text-uppercase"><strong>Monitoreando...</strong></span></a>';
                        //inserta HTML de mensaje monitoreando
                    }
                    else {
                        $('#msg-monitoreo').html('<p class="text-danger"><strong>' + result.msg + '</strong></p>');
                    }
                },
                error: function (error) {
                    alert("Error");
                }
            });
        }

        function detenerMonitoreo() {
            $.ajax({
                url: '<%: Url.Action("Stop", "ErrorLog") %>',
                type: 'POST',
                dataType: 'json',
                success: function (result) {
                    if (result.resp) {
                        $('#msg-monitoreo').html('<p class="text-success"><strong>' + result.msg + '</strong></p>');

                        document.getElementById("btnStart").disabled = false; //Boton Iniciar Enable
                        document.getElementById("btnStop").disabled = true; //Boton Detener Disable

                        if (timerLog != null) { //Verifica si el Timer se activo en esta pagina o en el master
                            clearInterval(timerLog);//borra timer de pagina local
                        }
                        else {
                            endTimer();//borra timer del master
                        }

                        eraseCookie('timer'); //Borra cookie del timer
                        eraseCookie('monitoreo');//Borra cookie del monitoreo

                        //var msjAlerta = document.getElementById('button-alert').innerHTML; //Obtiene HTML del mensaje de alarma
                        //var flag = msjAlerta.search("Monitor"); //Busca si contiene con la palabra Monitor

                        //if (flag != -1) { //Verifica si no encontro algun error, para no cambiar el mensaje de alerta, -1 No encontro coicidencia
                        document.getElementById('button-alert').innerHTML =
                        '<a href="<%: Url.Action("Index", "ErrorLog") %>"><span class="text-primary text-uppercase"><strong>Detenido!</strong></span></a>';
                        //Inserta HTML con mensaje de Detenido
                        //}
                    }
                    else {
                        $('#msg-monitoreo').html('<p class="text-danger"><strong>' + result.msg + '</strong></p>');
                    }
                },
                error: function (error) {
                    alert("Error");
                }
            });
        }
    </script>

</asp:Content>
