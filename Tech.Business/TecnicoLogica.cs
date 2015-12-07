using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;

namespace Tech.Business
{
    public class TecnicoLogica
    {

        DBUNLIMITEDEntities tecnicoDA = new DBUNLIMITEDEntities();

        public List<TECNICO> tecnicoListar()
        {
            List<TECNICO> tecnicos = tecnicoDA.TECNICO.ToList();

            return tecnicos;
        }


        public List<GPS_TECNICO> gpsTecnicoListarXTecnicoInvertidoFecha(int codigo)
        {
            List<GPS_TECNICO> gpsTecnicos = tecnicoDA.GPS_TECNICO.ToList();

            List<GPS_TECNICO> gpsTecnicosObtenidas = gpsTecnicos.Where(x => x.Co_Tecnico == codigo).OrderByDescending(x => x.Fe_Gps).ToList();

            return gpsTecnicosObtenidas;
        }
        public Boolean AgregarGPSTecnico(GPS_TECNICO gpsTecnico)
        {
            try
            {
                tecnicoDA.GPS_TECNICO.Add(gpsTecnico);
                tecnicoDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Ruta EncontrarRutaMenor(List<Ruta> rutas)
        {
            Ruta rutaMenor = rutas[0];

            for (int i = 0; i < rutas.Count; i++)
            {
                if ((int.Parse(rutas[i].distancia)) < (int.Parse(rutaMenor.distancia)))
                {
                    rutaMenor = rutas[i];
                }
            }

            return rutaMenor;
        }
    }
}