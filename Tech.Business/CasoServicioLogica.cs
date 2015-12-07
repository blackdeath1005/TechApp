using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using Tech.DataAccess;

namespace Tech.Business
{
    public class CasoServicioLogica
    {

        static string googleMap = "https://maps.googleapis.com/maps/api/distancematrix/";
        static string keyAPI = "AIzaSyAW2_ozfDHF0FOJ0U6fdwiHfCEN1rI5a88";
        DBUNLIMITEDEntities casoServicioDA = new DBUNLIMITEDEntities();

        public List<CASO_SERVICIO>casoServicioListarInvertido()
        {
            List<CASO_SERVICIO> casoServicios = casoServicioDA.CASO_SERVICIO.ToList().OrderByDescending(x => x.Co_Caso).ToList();

            return casoServicios;
        }
        public List<CASO_SERVICIO> casoServicioListarInvertidoFiltrado(string codigo, string fechaIni, string fechaFin, string estado)
        {
            List<CASO_SERVICIO> casoServicios = casoServicioListarInvertido();

            List<CASO_SERVICIO> casoServiciosObtenidas = new List<CASO_SERVICIO>();

            IEnumerable<CASO_SERVICIO> casoServiciosFiltrado = from p in casoServicios select p;

            if (!string.IsNullOrEmpty(codigo))
            {
                casoServiciosFiltrado = casoServiciosFiltrado.Where(x => x.Co_Caso == codigo);
            }
            if (!string.IsNullOrEmpty(estado))
            {
                casoServiciosFiltrado = casoServiciosFiltrado.Where(x => x.Tx_Estado == estado);
            }
            if (!string.IsNullOrEmpty(fechaIni))
            {
                casoServiciosFiltrado = casoServiciosFiltrado.Where(x => x.Fe_Caso >= DateTime.Parse(fechaIni));
            }
            if (!string.IsNullOrEmpty(fechaFin))
            {
                casoServiciosFiltrado = casoServiciosFiltrado.Where(x => x.Fe_Caso <= DateTime.Parse(fechaFin));
            }

            foreach (CASO_SERVICIO casoServicio in casoServiciosFiltrado)
            {
                casoServiciosObtenidas.Add(casoServicio);
            }

            //solicitudAtencionesObtenidas = solicitudAtencionesObtenidas.OrderByDescending(x => x.Co_Solicitud).ToList();

            return casoServiciosObtenidas;
        }
        public CASO_SERVICIO ObtenerCasoServicio(string codigo)
        {
            List<CASO_SERVICIO> casoServicios = casoServicioDA.CASO_SERVICIO.ToList();

            CASO_SERVICIO casoServicioObtenida = casoServicios.Single(delegate(CASO_SERVICIO casoServicio)
            {
                if (casoServicio.Co_Caso.Equals(codigo))
                    return true;
                else
                    return false;
            });
            return casoServicioObtenida;
        }
        public CASO_SERVICIO casoServicioUltimo()
        {
            CASO_SERVICIO casoServicio = casoServicioDA.CASO_SERVICIO.ToList().Last();

            return casoServicio;
        }
        public Boolean AgregarCasoServicio(CASO_SERVICIO casoServicioNuevo)
        {
            try
            {
                casoServicioDA.CASO_SERVICIO.Add(casoServicioNuevo);
                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EditarCasoServicioEstado(string id, CASO_SERVICIO casoServicioModificar)
        {
            try
            {
                CASO_SERVICIO casoServicioEditar = ObtenerCasoServicio(id);

                casoServicioEditar.Tx_Estado = casoServicioModificar.Tx_Estado;
                casoServicioEditar.Fe_RegistroMod = casoServicioModificar.Fe_RegistroMod;
                casoServicioEditar.No_UsuarioMod = casoServicioModificar.No_UsuarioMod;

                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public List<DETALLE_CASO_SERVICIO> detalleCasoServicioXCasoListar(string idCaso)
        {
            List<DETALLE_CASO_SERVICIO> detalleCasoServicios = casoServicioDA.DETALLE_CASO_SERVICIO.Where(x => x.Co_Caso == idCaso).ToList();

            return detalleCasoServicios;
        }
        public DETALLE_CASO_SERVICIO ObtenerDetalleCasoServicio(int id)
        {
            List<DETALLE_CASO_SERVICIO> detalleCasoServicios = casoServicioDA.DETALLE_CASO_SERVICIO.ToList();

            DETALLE_CASO_SERVICIO detalleCasoServicioObtenida = detalleCasoServicios.Single(delegate(DETALLE_CASO_SERVICIO detalleCasoServicio)
            {
                if (detalleCasoServicio.Co_Detalle_Caso == id)
                    return true;
                else
                    return false;
            });
            return detalleCasoServicioObtenida;
        }
        public Boolean AgregarDetalleCasoServicio(DETALLE_CASO_SERVICIO detalleCasoServicioNuevo)
        {
            try
            {
                casoServicioDA.DETALLE_CASO_SERVICIO.Add(detalleCasoServicioNuevo);
                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EditarDetalleCasoServicio(int id, DETALLE_CASO_SERVICIO detalleCasoServicioModificar)
        {
            try
            {
                DETALLE_CASO_SERVICIO detalleCasoServicioEditar = ObtenerDetalleCasoServicio(id);

                detalleCasoServicioEditar.Co_Tipo = detalleCasoServicioModificar.Co_Tipo;
                detalleCasoServicioEditar.Fe_HoraInicial = detalleCasoServicioModificar.Fe_HoraInicial;
                detalleCasoServicioEditar.Fe_HoraFinal = detalleCasoServicioModificar.Fe_HoraFinal;
                detalleCasoServicioEditar.Tx_DetalleServicio = detalleCasoServicioModificar.Tx_DetalleServicio;
                detalleCasoServicioEditar.Nu_Error = detalleCasoServicioModificar.Nu_Error;
                detalleCasoServicioEditar.Fe_RegistroMod = detalleCasoServicioModificar.Fe_RegistroMod;
                detalleCasoServicioEditar.No_UsuarioMod = detalleCasoServicioModificar.No_UsuarioMod;

                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EditarDetalleCasoServicioTecnico(int id, DETALLE_CASO_SERVICIO detalleCasoServicioModificar)
        {
            try
            {
                DETALLE_CASO_SERVICIO detalleCasoServicioEditar = ObtenerDetalleCasoServicio(id);

                detalleCasoServicioEditar.Co_Tecnico = detalleCasoServicioModificar.Co_Tecnico;
                detalleCasoServicioEditar.Fe_RegistroMod = detalleCasoServicioModificar.Fe_RegistroMod;
                detalleCasoServicioEditar.No_UsuarioMod = detalleCasoServicioModificar.No_UsuarioMod;

                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Boolean EliminarDetalleCasoServicio(int id)
        {
            try
            {
                DETALLE_CASO_SERVICIO detalleCasoServicioEliminar = ObtenerDetalleCasoServicio(id);

                casoServicioDA.DETALLE_CASO_SERVICIO.Remove(detalleCasoServicioEliminar);
                casoServicioDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> Distancia(string LatOrigen, string LngOrigen, string LatDestino, string LngDestino)
        {

            string webServiceGoogle = "json?origins="+LatOrigen+","+LngOrigen+"&destinations="+LatDestino+","+LngDestino+"&key="+keyAPI;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(googleMap);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(webServiceGoogle, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string resp = await response.Content.ReadAsStringAsync();

                    MapJson mapJson = JsonConvert.DeserializeObject<MapJson>(resp);
                    foreach (var row in mapJson.rows)
                    {
                        foreach (var element in row.elements)
                        {
                            resp = element.distance.value+"";
                        }
                    }

                    return resp;
                }

                return null;
            }
        }

    }

    public class Ruta
    {
        public EQUIPO equipo { get; set; }
        public TECNICO tecnico { get; set; }
        public string distancia { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }
    public class Row
    {
        public List<Element> elements { get; set; }
    }
    public class MapJson
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }

}
