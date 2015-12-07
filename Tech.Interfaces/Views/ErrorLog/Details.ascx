<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Tech.DataAccess.LOG_ERROR>" %>

<div id="DetailForm" class="container-modal">

    <h2>Detalle de Log Error # <%: Model.Co_LogError %></h2>

    <% using (Html.BeginForm("Details", "ErrorLog", FormMethod.Post, new { @class = "form-horizontal" })) { %>
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
              </div>
            </div>
            
            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Codigo Error</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Co_Error %></p>  
              </div>
              <label class="col-md-2 control-label">Fecha Error</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: String.Format("{0:g}", Model.Fe_LogError) %></p>
              </div>
            </div>

            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Detalle Error</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Detalle %></p>  
              </div>
            </div>
        
            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Voltaje</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Voltaje %>V DC</p>  
              </div>
              <label class="col-md-2 control-label">Alineacion Faja</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Alineacion %>°</p>
              </div>
            </div>

            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Visualizacion</label>  
              <div class="col-md-3">
                <% var visualizacion = "";
                   if (Model.Tx_Visualizacion.Equals("0"))
                       visualizacion = "Mal";
                   else
                       visualizacion = "Bien";
                %>
                <p class="form-control-static"><%: visualizacion %></p>  
              </div>
              <label class="col-md-2 control-label">Estado VGA</label>  
              <div class="col-md-3">
                <% var vga = "";
                   if (Model.Tx_VGA.Equals("0"))
                       vga = "Mal";
                   else
                       vga = "Bien";
                %>
                <p class="form-control-static"><%: vga %></p>
              </div>
            </div>

            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Estado COM</label>  
              <div class="col-md-3">
                <% var com = "";
                   if (Model.Tx_COM.Equals("0"))
                       com = "Mal";
                   else
                       com = "Bien";
                %>
                <p class="form-control-static"><%: com %></p>  
              </div>
              <label class="col-md-2 control-label">Energia Entrada</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Energia %>V AC</p>
              </div>
            </div>

            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Sistema Operativo</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Sistema %></p>  
              </div>
            </div>

            <!-- Text input-->
            <div class="form-group" >
              <label class="col-md-3 control-label">Archivo Log</label>  
              <div class="col-md-3">
                <p class="form-control-static"><%: Model.Tx_Archivo %></p>  
              </div>
            </div>

            <!-- Required Fileds Message -->
            <div class="form-group">
              <div class="col-md-2">
              </div>
              <div class="col-md-7">
              </div>
            </div>
        
        </fieldset>
    
    <% } %>

</div>
