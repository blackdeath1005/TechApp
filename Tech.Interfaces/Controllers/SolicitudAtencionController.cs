using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech.Business;
using Tech.DataAccess;

namespace Tech.Interfaces.Controllers
{
    public class SolicitudAtencionController : Controller
    {

        SolicitudAtencionLogica solicitudAtencionBL = new SolicitudAtencionLogica();
        EquipoLogica equipoBL = new EquipoLogica();

        public ActionResult Details(int id)
        {
            List<DETALLE_SOLICITUD_ATENCION> model = solicitudAtencionBL.detalleSolicitudAtencionListar(id);

            return View(model);
        }

        public ActionResult Index()
        {
            List<SOLICITUD_ATENCION> model = solicitudAtencionBL.solicitudAtencionListarInvertido();

            return View(model);
        }
        public ActionResult IndexDetails(int id)
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

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection collection)
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
                        return Json(new { resp = true, Html = RenderPartialView("Index", solicitudAtenciones) });
                    }

                }
                else
                {
                    List<SOLICITUD_ATENCION> model = solicitudAtencionBL.solicitudAtencionListarInvertido();

                    return Json(new { resp = true, Html = RenderPartialView("Index", model) });
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

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SOLICITUD_ATENCION solicitudAtencion, FormCollection collection)
        {
            try
            {
                SOLICITUD_ATENCION solicitudAtencionNuevo = new SOLICITUD_ATENCION();

                //solicitudAtencionNuevo.Co_Solicitud = null;
                solicitudAtencionNuevo.Fe_Solicitud = DateTime.Today;
                solicitudAtencionNuevo.Tx_Estado = "Pendiente";
                solicitudAtencionNuevo.Tx_Observaciones = collection["Tx_Observaciones"];

                //Boolean agregado = true;
                Boolean agregado = solicitudAtencionBL.AgregarSolicitudAtencion(solicitudAtencionNuevo);

                if (agregado)
                {
                    SOLICITUD_ATENCION solicitudAtencionUltimo = solicitudAtencionBL.solicitudAtencionUltimo();
                    //return View("CreateDetails"); //Mantiene su misma URL y el model que utiliza tambien se mantiene
                    //return RedirectToAction("CreateDetails", new { id = int.Parse(collection["Tx_Observaciones"]) });
                    return RedirectToAction("CreateDetails", new { id = solicitudAtencionUltimo.Co_Solicitud });
                }
                else
                {
                    ViewData["Error"] = "Error al Registrar Solicitud de Atencion";

                    return View(solicitudAtencion);
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Registrar Solicitud de Atencion";

                return View(solicitudAtencion);
            }
        }

        public ActionResult CreateDetails(int id, string msg)
        {
            if (msg != null)
            {
                if(msg.Equals("Agregado")) {
                    ViewData["Ok"] = "Equipo Agregado a la Solicitud de Atencion";
                }
                if(msg.Equals("Editado")) {
                    ViewData["Ok"] = "Detalle de la Solicitud Editada";
                }
                if(msg.Equals("Eliminado")) {
                    ViewData["Ok"] = "Equipo Eliminado de la Solicitud";
                }
            }
            List<string> Marcas = equipoBL.marcaListar();
            ViewBag.dsMarca = new SelectList(Marcas);
            ViewBag.dsModelo = new SelectList(new List<string>());
            ViewBag.dsSerie = new SelectList(new List<string>());
            ViewData["Co_Solicitud_Origen"] = id;

            return View();
        }
        [HttpPost]
        public ActionResult CreateDetails(DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencion, FormCollection collection)
        {
            int id = int.Parse(collection["Co_Solicitud_Origen"]);
            string marca = collection["EQUIPO.Tx_Marca"];
            string modelo = collection["EQUIPO.Tx_Modelo"];
            string serie = collection["EQUIPO.Tx_Serie"];

            try
            {
                DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionNuevo = new DETALLE_SOLICITUD_ATENCION();

                detalleSolicitudAtencionNuevo.Co_Solicitud = id;
                int idSerie = equipoBL.ObtenerEquipoXSerie(serie).Co_Equipo;
                detalleSolicitudAtencionNuevo.Co_Equipo = idSerie;
                detalleSolicitudAtencionNuevo.Nu_Error = collection["Nu_Error"]; 
                detalleSolicitudAtencionNuevo.Tx_Problema = collection["Tx_Problema"];
                detalleSolicitudAtencionNuevo.Tx_Observaciones = collection["Tx_Observaciones"];

                Boolean agregado = solicitudAtencionBL.AgregarDetalleSolicitudAtencion(detalleSolicitudAtencionNuevo);

                if (agregado)
                {
                    string msg = "Agregado";

                    return RedirectToAction("CreateDetails", new { id = id, msg = msg });
                }
                else
                {
                    ViewData["Error"] = "Error al Registrar Detalle de Solicitud de Atencion";

                    List<string> Marcas = equipoBL.marcaListar();
                    ViewBag.dsMarca = new SelectList(Marcas);
                    List<string> modelos = equipoBL.modeloListar(marca);
                    ViewBag.dsModelo = new SelectList(modelos);
                    List<string> series = equipoBL.serieListar(marca, modelo);
                    ViewBag.dsSerie = new SelectList(series);
                    ViewData["Co_Solicitud_Origen"] = id;

                    return View(detalleSolicitudAtencion);
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Registrar Detalle de Solicitud de Atencion";

                List<string> Marcas = equipoBL.marcaListar();
                ViewBag.dsMarca = new SelectList(Marcas);
                List<string> modelos = equipoBL.modeloListar(marca);
                ViewBag.dsModelo = new SelectList(modelos);
                List<string> series = equipoBL.serieListar(marca, modelo);
                ViewBag.dsSerie = new SelectList(series);
                ViewData["Co_Solicitud_Origen"] = id;

                return View(detalleSolicitudAtencion);
            }
        }

        public ActionResult EditDetails(int id,int id2,string msg)
        {
            if (msg != null)
            {
                ViewData["Error"] = msg;
            }

            DETALLE_SOLICITUD_ATENCION model = solicitudAtencionBL.ObtenerDetalleSolicitudAtencion(id,id2);

            ViewData["Co_Equipo_Origen"] = id2;

            return PartialView(model);
        }
        [HttpPost]
        public ActionResult EditDetails(int id, DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencion, FormCollection collection)
        {
            int id2 = int.Parse(collection["Co_Equipo_Origen"]);

            try
            {
                DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionModificar = new DETALLE_SOLICITUD_ATENCION();

                detalleSolicitudAtencionModificar.Nu_Error = collection["Nu_Error"]; 
                detalleSolicitudAtencionModificar.Tx_Problema = collection["Tx_Problema"];
                detalleSolicitudAtencionModificar.Tx_Observaciones = collection["Tx_Observaciones"];

                Boolean modificado = solicitudAtencionBL.EditarDetalleSolicitudAtencion(id, id2, detalleSolicitudAtencionModificar);

                if (modificado)
                {
                    string msg = "Editado";

                    return Json(new { id = id , msg= msg});
                }
                else
                {
                    string msg = "Error al Editar Detalle de Solicitud de Atencion";

                    return RedirectToAction("EditDetails", new { id = id, id2 = id2 , msg = msg});
                }
            }
            catch
            {
                string msg = "Error al Editar Detalle de Solicitud de Atencion";

                return RedirectToAction("EditDetails", new { id = id, id2 = id2, msg = msg });
            }
        }

        public ActionResult DeleteDetails(int id, int id2)
        {
            try
            {
                Boolean eliminado = solicitudAtencionBL.EliminarDetalleSolicitudAtencion(id,id2);

                if (eliminado)
                {
                    string msg = "Eliminado";

                    return RedirectToAction("CreateDetails", new { id = id, msg = msg });
                }
                else
                {
                    ViewData["Error"] = "Error al Eliminar Detalle de Solicitud de Atencion";

                    List<string> Marcas = equipoBL.marcaListar();
                    ViewBag.dsMarca = new SelectList(Marcas);
                    ViewBag.dsModelo = new SelectList(new List<string>());
                    ViewBag.dsSerie = new SelectList(new List<string>());
                    ViewData["Co_Solicitud_Origen"] = id;

                    return View("CreateDetails");
                }
            }
            catch
            {
                ViewData["Error"] = "Error al Eliminar Detalle de Solicitud de Atencion";

                List<string> Marcas = equipoBL.marcaListar();
                ViewBag.dsMarca = new SelectList(Marcas);
                ViewBag.dsModelo = new SelectList(new List<string>());
                ViewBag.dsSerie = new SelectList(new List<string>());
                ViewData["Co_Solicitud_Origen"] = id;

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
