using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;

namespace Tech.Business
{
    public class SolicitudAtencionLogica
    {
        DBUNLIMITEDEntities solicitudAtencionDA = new DBUNLIMITEDEntities();

        public List<SOLICITUD_ATENCION> solicitudAtencionListarInvertido()
        {
            List<SOLICITUD_ATENCION> solicitudAtenciones = solicitudAtencionDA.SOLICITUD_ATENCION.OrderByDescending(x => x.Co_Solicitud).ToList();

            return solicitudAtenciones;
        }
        public List<SOLICITUD_ATENCION> solicitudAtencionListarInvertidoFiltrado(string codigo, string fechaIni, string fechaFin, string estado)
        {

            List<SOLICITUD_ATENCION> solicitudAtenciones = solicitudAtencionListarInvertido();

            List<SOLICITUD_ATENCION> solicitudAtencionesObtenidas = new List<SOLICITUD_ATENCION>();

            IEnumerable<SOLICITUD_ATENCION> solicitudAtencionesFiltrado = from p in solicitudAtenciones select p;

            if (!string.IsNullOrEmpty(codigo))
            {
                solicitudAtencionesFiltrado = solicitudAtencionesFiltrado.Where(x => x.Co_Solicitud == int.Parse(codigo));
            }
            if (!string.IsNullOrEmpty(estado))
            {
                solicitudAtencionesFiltrado = solicitudAtencionesFiltrado.Where(x => x.Tx_Estado == estado);
            }
            if (!string.IsNullOrEmpty(fechaIni))
            {
                solicitudAtencionesFiltrado = solicitudAtencionesFiltrado.Where(x => x.Fe_Solicitud >= DateTime.Parse(fechaIni));
            }
            if (!string.IsNullOrEmpty(fechaFin))
            {
                solicitudAtencionesFiltrado = solicitudAtencionesFiltrado.Where(x => x.Fe_Solicitud <= DateTime.Parse(fechaFin));
            }

            foreach (SOLICITUD_ATENCION solicitudAtencion in solicitudAtencionesFiltrado)
            {
                solicitudAtencionesObtenidas.Add(solicitudAtencion);
            }

            //solicitudAtencionesObtenidas = solicitudAtencionesObtenidas.OrderByDescending(x => x.Co_Solicitud).ToList();

            return solicitudAtencionesObtenidas;
        }
        public SOLICITUD_ATENCION ObtenerSolicitudAtencion(int codigo)
        {
            List<SOLICITUD_ATENCION> solicitudAtenciones = solicitudAtencionDA.SOLICITUD_ATENCION.ToList();

            SOLICITUD_ATENCION solicitudAtencionObtenida = solicitudAtenciones.Single(delegate(SOLICITUD_ATENCION solicitudAtencion)
            {
                if (solicitudAtencion.Co_Solicitud == codigo)
                    return true;
                else
                    return false;
            });
            return solicitudAtencionObtenida;
        }
        public SOLICITUD_ATENCION solicitudAtencionUltimo()
        {
            SOLICITUD_ATENCION solicitudAtencion = solicitudAtencionDA.SOLICITUD_ATENCION.ToList().Last();

            return solicitudAtencion;
        }
        public Boolean AgregarSolicitudAtencion(SOLICITUD_ATENCION solicitudAtencionNuevo)
        {
            try
            {
                solicitudAtencionDA.SOLICITUD_ATENCION.Add(solicitudAtencionNuevo);
                solicitudAtencionDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtencionListar(int id)
        {
            List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtenciones = solicitudAtencionDA.DETALLE_SOLICITUD_ATENCION.Where(x => x.Co_Solicitud == id).ToList();

            return detalleSolicitudAtenciones;
        }
        public DETALLE_SOLICITUD_ATENCION ObtenerDetalleSolicitudAtencion(int idCodigo, int idEquipo)
        {
            List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtenciones = solicitudAtencionDA.DETALLE_SOLICITUD_ATENCION.ToList();

            DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionObtenida = detalleSolicitudAtenciones.Single(delegate(DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencion)
            {
                if (detalleSolicitudAtencion.Co_Solicitud == idCodigo && detalleSolicitudAtencion.Co_Equipo == idEquipo)
                    return true;
                else
                    return false;
            });
            return detalleSolicitudAtencionObtenida;
        }
        public List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtencionListarGarantia(int id)
        {
            List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtencionesGarantia = new List<DETALLE_SOLICITUD_ATENCION>();
            List<DETALLE_SOLICITUD_ATENCION> detalleSolicitudAtenciones = solicitudAtencionDA.DETALLE_SOLICITUD_ATENCION.Where(x => x.Co_Solicitud == id).ToList();

            foreach (DETALLE_SOLICITUD_ATENCION detalleSolicitud in detalleSolicitudAtenciones)
            {
                List<CONTRATO> contratos = detalleSolicitud.EQUIPO.CONTRATO.ToList();
                if (contratos.Count() > 0)
                {
                    foreach (CONTRATO contrato in contratos)
                    {
                        if (contrato.Tx_Estado.ToString().Equals("Activo"))
                        {
                            detalleSolicitudAtencionesGarantia.Add(detalleSolicitud);
                        }
                    }
                }
            }

            return detalleSolicitudAtencionesGarantia;
        }
        public Boolean AgregarDetalleSolicitudAtencion(DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionNuevo)
        {
            try
            {
                solicitudAtencionDA.DETALLE_SOLICITUD_ATENCION.Add(detalleSolicitudAtencionNuevo);
                solicitudAtencionDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EditarDetalleSolicitudAtencion(int idCodigo, int idEquipo, DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionModificar)
        {
            try
            {
                DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionEditar = ObtenerDetalleSolicitudAtencion(idCodigo, idEquipo);

                detalleSolicitudAtencionEditar.Nu_Error = detalleSolicitudAtencionModificar.Nu_Error;
                detalleSolicitudAtencionEditar.Tx_Problema = detalleSolicitudAtencionModificar.Tx_Problema;
                detalleSolicitudAtencionEditar.Tx_Observaciones = detalleSolicitudAtencionModificar.Tx_Observaciones;

                solicitudAtencionDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EliminarDetalleSolicitudAtencion(int idCodigo, int idEquipo)
        {
            try
            {
                DETALLE_SOLICITUD_ATENCION detalleSolicitudAtencionEliminar = ObtenerDetalleSolicitudAtencion(idCodigo, idEquipo);

                solicitudAtencionDA.DETALLE_SOLICITUD_ATENCION.Remove(detalleSolicitudAtencionEliminar);
                solicitudAtencionDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
