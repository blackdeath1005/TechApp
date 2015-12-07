using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;

namespace Tech.Business
{
    public class TipoServicioLogica
    {
        DBUNLIMITEDEntities tipoServicioDA = new DBUNLIMITEDEntities();

        public List<TIPO_SERVICIO> tipoServicioListar()
        {
            List<TIPO_SERVICIO> tipoServicios = tipoServicioDA.TIPO_SERVICIO.ToList();

            return tipoServicios;
        }
    }
}
