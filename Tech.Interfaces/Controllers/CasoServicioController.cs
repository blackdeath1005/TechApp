using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech.Business;
using Tech.DataAccess;

namespace Tech.Interfaces.Controllers
{
    public class CasoServicioController : Controller
    {

        CasoServicioLogica casoServicioBL = new CasoServicioLogica();
        SolicitudAtencionLogica solicitudAtencionBL = new SolicitudAtencionLogica();
        EquipoLogica equipoBL = new EquipoLogica();
        TecnicoLogica tecnicoBL = new TecnicoLogica();
        TipoServicioLogica tipoServicioBL = new TipoServicioLogica();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexGPSTecnico()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexGPSTecnico(string latitud, string longitud, string codigo)
        {
            try
            {
                GPS_TECNICO gpsTecnicoNuevo = new GPS_TECNICO();

                string lat = (Math.Round(Convert.ToDecimal(latitud), 7)).ToString("F7", CultureInfo.InvariantCulture);
                string lng = (Math.Round(Convert.ToDecimal(longitud), 7)).ToString("F7", CultureInfo.InvariantCulture);

                gpsTecnicoNuevo.Co_Tecnico = int.Parse(codigo);
                gpsTecnicoNuevo.Fe_Gps = DateTime.Now;
                gpsTecnicoNuevo.Tx_Latitud = lat;
                gpsTecnicoNuevo.Tx_Longitud = lng;

                Boolean agregado = tecnicoBL.AgregarGPSTecnico(gpsTecnicoNuevo);
                //Boolean agregado = true;
                
                if (agregado)
                {
                    string msg = "Coordenada Grabada Existosamente";

                    return Json(new { resp = true, mensaje = msg });
                }
                else
                {
                    string msg = "Error al grabar";

                    return Json(new { resp = false, mensaje = msg });
                }
            }
            catch
            {
                string msg = "Error al grabar";

                return Json(new { resp = false, mensaje = msg });
            }
            
        }
        public ActionResult IndexTecnico(int id, string msg)
        {
            if (msg != null)
            {
                ViewData["Error"] = msg;
            }

            DETALLE_CASO_SERVICIO detalleCaso = casoServicioBL.ObtenerDetalleCasoServicio(id);
            List<TECNICO> tecnicos = tecnicoBL.tecnicoListar();
            List<Ruta> rutas = new List<Ruta>();

            string latTecnico = "";
            string lngTecnico = "";
            string latEquipo = detalleCaso.EQUIPO.Tx_Latitud;
            string lngEquipo = detalleCaso.EQUIPO.Tx_Longitud;

            if (!string.IsNullOrEmpty(latEquipo) && !string.IsNullOrEmpty(lngEquipo))
            {
                foreach (TECNICO tecnico in tecnicos)
                {
                    List<GPS_TECNICO> gps = tecnicoBL.gpsTecnicoListarXTecnicoInvertidoFecha(tecnico.Co_Tecnico);
                    GPS_TECNICO gpstecnico = gps.First();

                    latTecnico = gpstecnico.Tx_Latitud;
                    lngTecnico = gpstecnico.Tx_Longitud;

                    string dist = casoServicioBL.Distancia(latTecnico, lngTecnico, latEquipo, lngEquipo).Result;

                    rutas.Add(new Ruta { equipo = detalleCaso.EQUIPO, tecnico = tecnico, distancia = dist });
                }
            }

            if (rutas.Count > 0)
            {
                Ruta rutaMenor = tecnicoBL.EncontrarRutaMenor(rutas);

                List<GPS_TECNICO> gps = tecnicoBL.gpsTecnicoListarXTecnicoInvertidoFecha(rutaMenor.tecnico.Co_Tecnico);
                GPS_TECNICO gpstecnico = gps.First();
                latTecnico = gpstecnico.Tx_Latitud;
                lngTecnico = gpstecnico.Tx_Longitud;

                ViewData["NomTecnico"] = rutaMenor.tecnico.Tx_NomCompleto;
                ViewData["Distancia"] = rutaMenor.distancia;
                ViewData["LatOrigen"] = latTecnico;
                ViewData["LngOrigen"] = lngTecnico;
                ViewData["LatDestino"] = latEquipo;
                ViewData["LngDestino"] = lngEquipo;

                ViewBag.dsTecnico = new SelectList(tecnicos, "Co_Tecnico", "Tx_NomCompleto", rutaMenor.tecnico.Co_Tecnico);
            }
            else
            {
                ViewData["EquipoFuera"] = "Equipo se ubica fuera de Lima metropolitana";

                ViewBag.dsTecnico = new SelectList(tecnicos,"Co_Tecnico","Tx_NomCompleto");
            }

            ViewData["CodDetalleCaso"] = detalleCaso.Co_Detalle_Caso;
            ViewData["CodCaso"] = detalleCaso.Co_Caso;

            return View();
        }
        [HttpPost]
        public ActionResult IndexTecnico(FormCollection collection)
        {
            int id = int.Parse(collection["Co_Tecnico"]);
            string idCaso = collection["CodCaso"];
            int idDetalleCaso = int.Parse(collection["CodDetalleCaso"]);

            try
            {
                DETALLE_CASO_SERVICIO detalleCasoServicioModificar = new DETALLE_CASO_SERVICIO();

                detalleCasoServicioModificar.Co_Tecnico = id;
                detalleCasoServicioModificar.Fe_RegistroMod = DateTime.Now;
                detalleCasoServicioModificar.No_UsuarioMod = collection["No_UsuarioMod"];

                Boolean asignado = casoServicioBL.EditarDetalleCasoServicioTecnico(idDetalleCaso, detalleCasoServicioModificar);

                if (asignado)
                {
                    CASO_SERVICIO casoServicios = casoServicioBL.ObtenerCasoServicio(idCaso);
                    int flag = casoServicios.DETALLE_CASO_SERVICIO.Where(x => x.Co_Tecnico == null).ToList().Count;
                    if (flag == 0)
                    {
                        CASO_SERVICIO casoServicioModificar = new CASO_SERVICIO();

                        casoServicioModificar.Tx_Estado = "Asignado";
                        casoServicioModificar.Fe_RegistroMod = DateTime.Now;
                        casoServicioModificar.No_UsuarioMod = collection["No_UsuarioMod"];

                        casoServicioBL.EditarCasoServicioEstado(idCaso, casoServicioModificar);
                    }

                    string msg = "Tecnico asignado al detalle satisfactoriamente";

                    return RedirectToAction("SearchCaso", new { msg = msg });
                }
                else
                {
                    string msg = "Error al asignar tecnico al detalle del caso";

                    return RedirectToAction("IndexTecnico", new { id = idDetalleCaso, msg = msg });
                }
            }
            catch
            {
                string msg = "Error al asignar tecnico al detalle del caso";

                return RedirectToAction("IndexTecnico", new { id = idDetalleCaso, msg = msg });
            }
        }
        public ActionResult IndexCaso()
        {
            List<CASO_SERVICIO> model = casoServicioBL.casoServicioListarInvertido();

            return View(model);
        }
        public ActionResult IndexSolicitud()
        {
            List<SOLICITUD_ATENCION> model = solicitudAtencionBL.solicitudAtencionListarInvertido();

            return View(model);
        }
        public ActionResult IndexDetails(string id)
        {
            List<DETALLE_CASO_SERVICIO> model = casoServicioBL.detalleCasoServicioXCasoListar(id);
            
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult DetailsCaso(string id)
        {
            List<DETALLE_CASO_SERVICIO> model = casoServicioBL.detalleCasoServicioXCasoListar(id);

            return View(model);
        }
        public ActionResult DetailsSolicitud(int id)
        {
            List<DETALLE_SOLICITUD_ATENCION> model = solicitudAtencionBL.detalleSolicitudAtencionListar(id);

            return View(model);
        }

        private string RenderPartialView(string viewName, object model)
        {
            ViewData.Model = model;
            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer);
                viewResult.View.Render(viewContext, writer);

                return writer.GetStringBuilder().ToString();
            }
        }

        public ActionResult SearchCaso(string msg)
        {
            if (msg != null)
            {
                ViewData["Ok"] = msg;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SearchCaso(FormCollection collection)
        {
            string codigo = collection["codigo"];
            string fechaIni = collection["fechaIni"];
            string fechaFin = collection["fechaFin"];
            string estado = collection["estado"];

            try
            {
                if (!string.IsNullOrEmpty(codigo) || !string.IsNullOrEmpty(fechaIni) || !string.IsNullOrEmpty(fechaFin) || !string.IsNullOrEmpty(estado))
                {
                    List<CASO_SERVICIO> casoServicios = casoServicioBL.casoServicioListarInvertidoFiltrado(codigo, fechaIni, fechaFin, estado);

                    if (casoServicios.Count() == 0)
                    {
                        string msg = "No hay registros coicidentes";

                        return Json(new { msg = msg });
                    }
                    else
                    {
                        return Json(new { resp = true, Html = RenderPartialView("IndexCaso", casoServicios) });
                    }

                }
                else
                {
                    List<CASO_SERVICIO> model = casoServicioBL.casoServicioListarInvertido();

                    return Json(new { resp = true, Html = RenderPartialView("IndexCaso", model) });
                }
            }
            catch
            {
                string msg = "Error al Consultar Caso de Servicio";

                return Json(new { msg = msg });
            }
        }
        public ActionResult SearchSolicitud()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchSolicitud(FormCollection collection)
        {
            string codigo = collection["codigo"];
            string fechaIni = collection["fechaIni"];
            string fechaFin = collection["fechaFin"];
            string estado = collection["estado"];

            try
            {
                if (!string.IsNullOrEmpty(codigo) || !string.IsNullOrEmpty(fechaIni) || !string.IsNullOrEmpty(fechaFin) || !string.IsNullOrEmpty(estado))
                {
                    List<SOLICITUD_ATENCION> solicitudAtenciones = solicitudAtencionBL.solicitudAtencionListarInvertidoFiltrado(codigo, fechaIni, fechaFin, estado);

                    if (solicitudAtenciones.Count() == 0)
                    {
                        string msg = "No hay registros coicidentes";

                        return Json(new { msg = msg });
                    }
                    else
                    {
                        return Json(new { resp = true, Html = RenderPartialView("IndexSolicitud", solicitudAtenciones) });
                    }

                }
                else
                {
                    List<SOLICITUD_ATENCION> model = solicitudAtencionBL.solicitudAtencionListarInvertido();

                    return Json(new { resp = true, Html = RenderPartialView("IndexSolicitud", model) });
                }
            }
            catch
            {
                string msg = "Error al Consultar Solicitud de Atencion";

                return Json(new { msg = msg });
            }
        }

        [HttpPost]
        public ActionResult GetModelo(string selectedValue)
        {
            if (selectedValue.Equals(""))
            {
                List<string> modelos = new List<string>();
                return Json(new { modelosLista = modelos });
            }
            else
            {
                List<string> modelos = equipoBL.modeloListar(selectedValue);
                return Json(new { modelosLista = modelos });
            }
        }
        [HttpPost]
        public ActionResult GetSerie(string selectedMarca, string selectedModelo)
        {
            if (selectedModelo.Equals(""))
            {
                List<string> series = new List<string>();
                return Json(new { seriesLista = series });
            }
            else
            {
                List<string> series = equipoBL.serieListar(selectedMarca, selectedModelo);
                return Json(new { seriesLista = series });
            }
        }

        public ActionResult Create(string codSolicitud)
        {

            if (codSolicitud != null)
            {
                List<DETALLE_SOLICITUD_ATENCION> detallesSolicitud = solicitudAtencionBL.detalleSolicitudAtencionListarGarantia(int.Parse(codSolicitud));

                ViewData["codSolicitud"] = codSolicitud;
                ViewData["nEquipos"] = detallesSolicitud.Count;
            }

            return View();
        }
        [HttpPost]
        public ActionResult Create(CASO_SERVICIO casoServicio, FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(collection["codSolicitud"].ToString()))
                {
                    CASO_SERVICIO casoUltimo = casoServicioBL.casoServicioUltimo();
                    string codigo = casoUltimo.Co_Caso.ToString().Substring(3);
                    int cod = int.Parse(codigo) + 1;
                    string año = DateTime.Now.Year.ToString().Substring(2);
                    string codCaso = "";

                    if (cod < 10)
                    {
                        codCaso = "00" + cod;
                    }
                    if (cod < 100)
                    {
                        codCaso = "0" + cod;
                    }

                    CASO_SERVICIO casoServicioNuevo = new CASO_SERVICIO();

                    casoServicioNuevo.Co_Caso = año + "-" + codCaso;
                    casoServicioNuevo.Co_Solicitud = null;
                    casoServicioNuevo.Fe_Caso = DateTime.Today;
                    casoServicioNuevo.Fl_Gasto = "No";
                    casoServicioNuevo.Ss_Gasto = 0;
                    casoServicioNuevo.Fl_Facturado = "No";
                    casoServicioNuevo.Ss_Facturado = 0;
                    casoServicioNuevo.Tx_Estado = "Pendiente Asignacion";
                    casoServicioNuevo.Tx_Observaciones = collection["Tx_Observaciones"];
                    casoServicioNuevo.Fl_Informe = "No";
                    casoServicioNuevo.Fe_RegistroIni = DateTime.Now;
                    casoServicioNuevo.No_UsuarioIni = collection["No_UsuarioIni"];
                    casoServicioNuevo.Fe_RegistroMod = DateTime.Now;
                    casoServicioNuevo.No_UsuarioMod = collection["No_UsuarioMod"];

                    Boolean agregado = casoServicioBL.AgregarCasoServicio(casoServicioNuevo);

                    if (agregado)
                    {
                        //CASO_SERVICIO casoUltimoNuevo = casoServicioBL.casoServicioUltimo();
                        //return RedirectToAction("CreateDetails", new { id = collection["Tx_Observaciones"] });
                        return RedirectToAction("CreateDetails", new { id = casoServicioNuevo.Co_Caso });
                        //return RedirectToAction("Index", new { codigoCasoServicio = casoUltimoNuevo.Co_Caso.ToString() });
                    }
                    else
                    {
                        ViewData["Error"] = "Error al Crear Caso de Servicio";

                        return View(casoServicio);
                    }
                }
                else
                {
                    if (int.Parse(collection["nEquipos"].ToString()) != 0)
                    {
                        CASO_SERVICIO casoUltimo = casoServicioBL.casoServicioUltimo();
                        string codigo = casoUltimo.Co_Caso.ToString().Substring(3);
                        int cod = int.Parse(codigo) + 1;
                        string año = DateTime.Now.Year.ToString().Substring(2);
                        string codCaso = "";

                        if (cod < 10)
                        {
                            codCaso = "00" + cod;
                        }
                        if (cod < 100)
                        {
                            codCaso = "0" + cod;
                        }

                        CASO_SERVICIO casoServicioNuevo = new CASO_SERVICIO();

                        casoServicioNuevo.Co_Caso = año + "-" + codCaso;
                        casoServicioNuevo.Co_Solicitud = int.Parse(collection["codSolicitud"].ToString());
                        casoServicioNuevo.Fe_Caso = DateTime.Today;
                        casoServicioNuevo.Fl_Gasto = "No";
                        casoServicioNuevo.Ss_Gasto = 0;
                        casoServicioNuevo.Fl_Facturado = "No";
                        casoServicioNuevo.Ss_Facturado = 0;
                        casoServicioNuevo.Tx_Estado = "Pendiente Asignacion";
                        casoServicioNuevo.Tx_Observaciones = collection["Tx_Observaciones"];
                        casoServicioNuevo.Fl_Informe = "No";
                        casoServicioNuevo.Fe_RegistroIni = DateTime.Now;
                        casoServicioNuevo.No_UsuarioIni = collection["No_UsuarioIni"];
                        casoServicioNuevo.Fe_RegistroMod = DateTime.Now;
                        casoServicioNuevo.No_UsuarioMod = collection["No_UsuarioMod"];

                        Boolean agregado = casoServicioBL.AgregarCasoServicio(casoServicioNuevo);

                        if (agregado)
                        {
                            int flag = 0;
                            int flagError = 0;

                            List<DETALLE_SOLICITUD_ATENCION> detallesSolicitud = solicitudAtencionBL.detalleSolicitudAtencionListarGarantia(int.Parse(collection["codSolicitud"].ToString()));

                            foreach (DETALLE_SOLICITUD_ATENCION detalle in detallesSolicitud)
                            {
                                DETALLE_CASO_SERVICIO detalleCasoServicioNuevo = new DETALLE_CASO_SERVICIO();

                                detalleCasoServicioNuevo.Co_Caso = casoServicioNuevo.Co_Caso;
                                detalleCasoServicioNuevo.Co_Equipo = detalle.Co_Equipo;
                                detalleCasoServicioNuevo.Co_Tipo = null;
                                detalleCasoServicioNuevo.Fe_HoraInicial = null;
                                detalleCasoServicioNuevo.Fe_HoraFinal = null;
                                detalleCasoServicioNuevo.Co_Tecnico = null;
                                detalleCasoServicioNuevo.Tx_DetalleServicio = detalle.Tx_Problema;
                                detalleCasoServicioNuevo.Nu_Error = detalle.Nu_Error;
                                detalleCasoServicioNuevo.Fl_Reporte = "No";
                                detalleCasoServicioNuevo.Fe_RegistroIni = DateTime.Now;
                                detalleCasoServicioNuevo.No_UsuarioIni = collection["No_UsuarioIni"];
                                detalleCasoServicioNuevo.Fe_RegistroMod = DateTime.Now;
                                detalleCasoServicioNuevo.No_UsuarioMod = collection["No_UsuarioMod"];

                                Boolean llenado = casoServicioBL.AgregarDetalleCasoServicio(detalleCasoServicioNuevo);

                                if (llenado)
                                {
                                    flag++;
                                }
                                else
                                {
                                    flagError++;
                                }
                            }

                            return RedirectToAction("CreateDetails", new { id = casoServicioNuevo.Co_Caso });
                        }
                        else
                        {
                            ViewData["Error"] = "Error al Crear Caso de Servicio" + collection["nEquipos"];

                            ViewData["codSolicitud"] = collection["codSolicitud"].ToString();
                            ViewData["nEquipos"] = collection["nEquipos"].ToString();

                            return View(casoServicio);
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Error al Crear Caso de Servicio. Solicitud no presenta equipos con garantias activas";

                        ViewData["codSolicitud"] = collection["codSolicitud"].ToString();
                        ViewData["nEquipos"] = collection["nEquipos"].ToString();

                        return View(casoServicio);
                    }
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Crear Caso de Servicio";

                ViewData["codSolicitud"] = collection["codSolicitud"].ToString();
                ViewData["nEquipos"] = collection["nEquipos"].ToString();

                return View(casoServicio);
            }
        }

        public ActionResult CreateDetails(string id, string msg)
        {
            if (msg != null)
            {
                if (msg.Equals("Agregado"))
                {
                    ViewData["Ok"] = "Equipo Agregado al Caso de Servicio";
                }
                if (msg.Equals("Editado"))
                {
                    ViewData["Ok"] = "Detalle del Caso de Servicio Editado";
                }
                if (msg.Equals("Eliminado"))
                {
                    ViewData["Ok"] = "Equipo Eliminado del Caso de Servicio";
                }
            }
            List<string> Marcas = equipoBL.marcaListar();
            ViewBag.dsMarca = new SelectList(Marcas);
            ViewBag.dsModelo = new SelectList(new List<string>());
            ViewBag.dsSerie = new SelectList(new List<string>());
            ViewData["Co_Caso_Origen"] = id;

            List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
            ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

            return View();
        }
        [HttpPost]
        public ActionResult CreateDetails(DETALLE_CASO_SERVICIO detalleCasoServicio, FormCollection collection, string dsTipoServicioList)
        {
            string id = collection["Co_Caso_Origen"];
            string marca = collection["EQUIPO.Tx_Marca"];
            string modelo = collection["EQUIPO.Tx_Modelo"];
            string serie = collection["EQUIPO.Tx_Serie"];

            try
            {
                if (string.IsNullOrEmpty(dsTipoServicioList))
                {
                    ViewData["Error"] = "Debe de llenar todos los campos obligatorios";

                    List<string> Marcas = equipoBL.marcaListar();
                    ViewBag.dsMarca = new SelectList(Marcas);
                    List<string> modelos = equipoBL.modeloListar(marca);
                    ViewBag.dsModelo = new SelectList(modelos);
                    List<string> series = equipoBL.serieListar(marca, modelo);
                    ViewBag.dsSerie = new SelectList(series);
                    ViewData["Co_Caso_Origen"] = id;

                    List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
                    ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

                    return View(detalleCasoServicio);
                }
                else
                {
                    DETALLE_CASO_SERVICIO detalleCasoServicioNuevo = new DETALLE_CASO_SERVICIO();

                    detalleCasoServicioNuevo.Co_Caso = id;
                    int idSerie = equipoBL.ObtenerEquipoXSerie(serie).Co_Equipo;
                    detalleCasoServicioNuevo.Co_Equipo = idSerie;
                    detalleCasoServicioNuevo.Co_Tipo = int.Parse(dsTipoServicioList);
                    if (string.IsNullOrEmpty(collection["fechaIni"].ToString()))
                    {
                        detalleCasoServicioNuevo.Fe_HoraInicial = null;
                    }
                    else
                    {
                        detalleCasoServicioNuevo.Fe_HoraInicial = DateTime.Parse(collection["fechaIni"].ToString());
                    }
                    if (string.IsNullOrEmpty(collection["fechaFin"].ToString()))
                    {
                        detalleCasoServicioNuevo.Fe_HoraFinal = null;
                    }
                    else
                    {
                        detalleCasoServicioNuevo.Fe_HoraFinal = DateTime.Parse(collection["fechaFin"].ToString());
                    }
                    detalleCasoServicioNuevo.Co_Tecnico = null;
                    detalleCasoServicioNuevo.Tx_DetalleServicio = collection["Tx_DetalleServicio"];
                    detalleCasoServicioNuevo.Nu_Error = collection["Nu_Error"];
                    detalleCasoServicioNuevo.Fl_Reporte = "No";
                    detalleCasoServicioNuevo.Fe_RegistroIni = DateTime.Now;
                    detalleCasoServicioNuevo.No_UsuarioIni = collection["No_UsuarioIni"];
                    detalleCasoServicioNuevo.Fe_RegistroMod = DateTime.Now;
                    detalleCasoServicioNuevo.No_UsuarioMod = collection["No_UsuarioMod"];

                    Boolean agregado = casoServicioBL.AgregarDetalleCasoServicio(detalleCasoServicioNuevo);

                    if (agregado)
                    {
                        string msg = "Agregado";

                        return RedirectToAction("CreateDetails", new { id = id, msg = msg });
                    }
                    else
                    {
                        ViewData["Error"] = "Error al Registrar Detalle de Caso de Servicio";

                        List<string> Marcas = equipoBL.marcaListar();
                        ViewBag.dsMarca = new SelectList(Marcas);
                        List<string> modelos = equipoBL.modeloListar(marca);
                        ViewBag.dsModelo = new SelectList(modelos);
                        List<string> series = equipoBL.serieListar(marca, modelo);
                        ViewBag.dsSerie = new SelectList(series);
                        ViewData["Co_Caso_Origen"] = id;

                        List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
                        ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

                        return View(detalleCasoServicio);
                    }
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Registrar Detalle de Caso de Servicio";

                List<string> Marcas = equipoBL.marcaListar();
                ViewBag.dsMarca = new SelectList(Marcas);
                List<string> modelos = equipoBL.modeloListar(marca);
                ViewBag.dsModelo = new SelectList(modelos);
                List<string> series = equipoBL.serieListar(marca, modelo);
                ViewBag.dsSerie = new SelectList(series);
                ViewData["Co_Caso_Origen"] = id;
                
                List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
                ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

                return View(detalleCasoServicio);
            }
        }

        public ActionResult EditDetails(int id, string idCaso, string msg)
        {
            if (msg != null)
            {
                ViewData["Error"] = msg;
            }

            DETALLE_CASO_SERVICIO model = casoServicioBL.ObtenerDetalleCasoServicio(id);
            
            List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
            if (model.Co_Tipo == null)
            {
                ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");
            }
            else {
                ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre", model.TIPO_SERVICIO.Co_Tipo);
            }      
            
            ViewData["fechaIni"] = model.Fe_HoraInicial;
            ViewData["fechaFin"] = model.Fe_HoraFinal;
            ViewData["Co_Caso_Origen"] = idCaso;

            return PartialView(model);
        }
        [HttpPost]
        public ActionResult EditDetails(int id, DETALLE_CASO_SERVICIO detalleCasoServicio, FormCollection collection, string dsTipoServicioList)
        {
            string idCaso = collection["Co_Caso_Origen"];

            try
            {
                DETALLE_CASO_SERVICIO detalleCasoServicioModificar = new DETALLE_CASO_SERVICIO();

                detalleCasoServicioModificar.Co_Tipo = int.Parse(dsTipoServicioList);
                if (string.IsNullOrEmpty(collection["fechaIni2"].ToString()))
                {
                    detalleCasoServicioModificar.Fe_HoraInicial = null;
                }
                else
                {
                    detalleCasoServicioModificar.Fe_HoraInicial = DateTime.Parse(collection["fechaIni2"].ToString());
                }
                if (string.IsNullOrEmpty(collection["fechaFin2"].ToString()))
                {
                    detalleCasoServicioModificar.Fe_HoraFinal = null;
                }
                else
                {
                    detalleCasoServicioModificar.Fe_HoraFinal = DateTime.Parse(collection["fechaFin2"].ToString());
                }
                detalleCasoServicioModificar.Tx_DetalleServicio = collection["Tx_DetalleServicio"];
                detalleCasoServicioModificar.Nu_Error = collection["Nu_Error"];
                detalleCasoServicioModificar.Fe_RegistroMod = DateTime.Now;
                detalleCasoServicioModificar.No_UsuarioMod = collection["No_UsuarioMod"];

                Boolean modificado = casoServicioBL.EditarDetalleCasoServicio(id, detalleCasoServicioModificar);

                if (modificado)
                {
                    string msg = "Editado";

                    return Json(new { id = idCaso, msg = msg });
                }
                else
                {
                    string msg = "Error al Editar Detalle de Solicitud de Atencion";

                    return RedirectToAction("EditDetails", new { id = id, idCaso = idCaso, msg = msg });
                }
            }
            catch
            {
                string msg = "Error al Editar Detalle de Solicitud de Atencion";

                return RedirectToAction("EditDetails", new { id = id, idCaso = idCaso, msg = msg });
            }
        }

        public ActionResult DeleteDetails(int id, string idCaso)
        {
            try
            {
                Boolean eliminado = casoServicioBL.EliminarDetalleCasoServicio(id);
                
                if (eliminado)
                {
                    string msg = "Eliminado";

                    return RedirectToAction("CreateDetails", new { id = idCaso, msg = msg });
                }
                else
                {
                    ViewData["Error"] = "Error al Eliminar Detalle de Caso de Servicio";

                    List<string> Marcas = equipoBL.marcaListar();
                    ViewBag.dsMarca = new SelectList(Marcas);
                    ViewBag.dsModelo = new SelectList(new List<string>());
                    ViewBag.dsSerie = new SelectList(new List<string>());
                    ViewData["Co_Caso_Origen"] = idCaso;

                    List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
                    ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

                    return View("CreateDetails");
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Eliminar Detalle de Caso de Servicio";

                List<string> Marcas = equipoBL.marcaListar();
                ViewBag.dsMarca = new SelectList(Marcas);
                ViewBag.dsModelo = new SelectList(new List<string>());
                ViewBag.dsSerie = new SelectList(new List<string>());
                ViewData["Co_Caso_Origen"] = idCaso;

                List<TIPO_SERVICIO> tipoServicios = tipoServicioBL.tipoServicioListar();
                ViewData["dsTipoServicio"] = new SelectList(tipoServicios, "Co_Tipo", "No_Nombre");

                return View("CreateDetails");
            }
        }


        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
