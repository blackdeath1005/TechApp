using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Tech.Business;
using Tech.DataAccess;

namespace Tech.Interfaces.Controllers
{
    public class ErrorLogController : Controller
    {

        ErrorLogLogica errorLogBL = new ErrorLogLogica();
        EquipoLogica equipoBL = new EquipoLogica();

        public ActionResult Index(string msg)
        {
            if (msg != null)
            {
                if (msg.Equals("ErrorArchivo"))
                    ViewData["Error"] = "Error al leer el archivo o no se encuentra";
                if (msg.Equals("ErrorCarga"))
                    ViewData["Error"] = "Error al cargar archivo";
                if (msg.Equals("ErrorDuplicado"))
                    ViewData["Error"] = "Archivo Log Error ya se encuentra cargado";
                if (msg.Equals("Cargado"))
                    ViewData["Ok"] = "Archivo Cargado Correctamente";
            }

            FileInfo[] archivosLog = errorLogBL.FilesOnLogsFolder();
            
            if (archivosLog.Length != 0)
            {
                ViewData["archivos"]=archivosLog;
            }

            return View();
        }

        public ActionResult IndexDetails()
        {
            List<LOG_ERROR> model = errorLogBL.errorLogListarInvertido();

            return View(model);
        }

        public ActionResult UploadFile(string path, string nombre)
        {
            List<String> datos = errorLogBL.cargarArchivo(path);

            if (datos == null || datos.Count < 10)
            {
                string mensaje = "ErrorArchivo";
                return RedirectToAction("index", new { msg = mensaje });
            }
            else
            {
                List<LOG_ERROR> logError = errorLogBL.ObtenerErrorLogNombre(nombre);

                if (logError.Count==0)
                {
                    LOG_ERROR errorLogNuevo = new LOG_ERROR();

                    string error = datos[0].ToString().Substring(1);

                    string[] split = nombre.Split('_');
                    string serie = split[0];
                    string fecha = split[1];
                    split = fecha.Split('.');
                    fecha = split[0];

                    EQUIPO equipo = equipoBL.ObtenerEquipoXSerie(serie);

                    string formatString = "yyyyMMddHHmmss";
                    DateTime dtFecha = DateTime.ParseExact(fecha, formatString, null);


                    errorLogNuevo.Co_Error = int.Parse(error);
                    errorLogNuevo.Co_Equipo = equipo.Co_Equipo;
                    errorLogNuevo.Fe_LogError = dtFecha;
                    errorLogNuevo.Tx_Detalle = datos[2];
                    errorLogNuevo.Tx_Voltaje = datos[3];
                    errorLogNuevo.Tx_Alineacion = datos[4];
                    errorLogNuevo.Tx_Visualizacion = datos[5];
                    errorLogNuevo.Tx_VGA = datos[6];
                    errorLogNuevo.Tx_COM = datos[7];
                    errorLogNuevo.Tx_Energia = datos[8];
                    errorLogNuevo.Tx_Sistema = datos[9];
                    errorLogNuevo.Tx_Archivo = nombre;

                    Boolean agregado = errorLogBL.AgregarErrorLog(errorLogNuevo);
                    //Boolean agregado = false;
                
                    if (agregado)
                    {
                        errorLogBL.borrarArchivo(path);

                        string mensaje = "Cargado";
                        return RedirectToAction("Index", new { msg = mensaje });
                    }
                    else
                    {
                        string mensaje = "ErrorCarga";
                        return RedirectToAction("Index", new { msg = mensaje });
                    }
                }
                else {
                    string mensaje = "ErrorDuplicado";
                    return RedirectToAction("Index", new { msg = mensaje });
                }
                
            }
        }

        [HttpPost]
        public ActionResult SearchFolder()
        {
            FileInfo[] archivosLog = errorLogBL.FilesOnLogsFolder();

            return Json(new { contador = archivosLog.Length });
        }

        [HttpPost]
        public ActionResult Start()
        {
            string mensaje = "";

            FileSystemWatcher fsw = errorLogBL.StartWatcher();

            if (fsw != null)
            {
                Session["fsw"] = fsw;
                mensaje = "Monitoreo Iniciado";
                return Json(new { resp = true , msg = mensaje });
            }
            else
            {
                mensaje = "Error al Iniciar Monitoreo";
                return Json(new { resp = false, msg = mensaje });
            }
        }
        [HttpPost]
        public ActionResult Stop()
        {
            string mensaje = "";
            FileSystemWatcher fsw = Session["fsw"] as FileSystemWatcher;

            Boolean detenido = errorLogBL.StopWatcher(fsw);

            if (detenido)
            {
                Session["fsw"] = null;
                mensaje = "Monitoreo Terminado";
                return Json(new { resp = detenido, msg = mensaje });
            }
            else
            {
                mensaje = "Error al Detener Monitoreo";
                return Json(new { resp = detenido, msg = mensaje });
            }
        }

        public ActionResult Details(int id)
        {
            LOG_ERROR model = errorLogBL.ObtenerErrorLog(id);

            return View(model);
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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
