using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;

namespace Tech.Business
{
    public class EquipoLogica
    {
        DBUNLIMITEDEntities equipoDA = new DBUNLIMITEDEntities();

        public List<EQUIPO> equipoListar()
        {
            List<EQUIPO> equipos = equipoDA.EQUIPO.ToList();

            return equipos;
        }
        public List<EQUIPO> equipoClienteListar(int idCliente)
        {
            List<EQUIPO> equipos = equipoDA.EQUIPO.Where(x => x.Co_Cliente == idCliente).ToList();

            return equipos;
        }

        public EQUIPO ObtenerEquipoXSerie(string idSerie)
        {
            List<EQUIPO> equipos = equipoDA.EQUIPO.ToList();

            EQUIPO equipoObtenida = equipos.Single(delegate(EQUIPO equipo)
            {
                if (equipo.Tx_Serie.Equals(idSerie))
                    return true;
                else
                    return false;
            });
            return equipoObtenida;
        }

        public List<string> marcaListar()
        {
            List<string> marcas = equipoDA.EQUIPO.Select(x => x.Tx_Marca).Distinct().ToList();

            return marcas;
        }

        public List<string> modeloListar(string marca)
        {
            List<string> modelos = equipoDA.EQUIPO.Where(x => x.Tx_Marca == marca).Select(s => s.Tx_Modelo).Distinct().ToList();

            return modelos;
        }

        public List<string> serieListar(string marca, string modelo)
        {
            List<string> series = equipoDA.EQUIPO.Where(x => x.Tx_Marca == marca && x.Tx_Modelo == modelo).Select(s => s.Tx_Serie).Distinct().ToList();

            return series;
        }

    }
}
