using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;


namespace Tech.Business
{
    public class UsuarioLogica
    {

        DBUNLIMITEDEntities usuarioDA = new DBUNLIMITEDEntities();

        public List<USUARIO> usuarioListar()
        {
            List<USUARIO> usuarios = usuarioDA.USUARIO.ToList();

            return usuarios;
        }

        public USUARIO ObtenerUsuario(int codigo)
        {
            List<USUARIO> usuarios = usuarioDA.USUARIO.ToList();

            USUARIO usuarioObtenida = usuarios.Single(delegate(USUARIO usuario)
            {
                if (usuario.Co_Usuario == codigo)
                    return true;
                else
                    return false;
            });
            return usuarioObtenida;
        }

        public string ValidarUsuario(string nombre, string password)
        {

            string usuarioValidado = "No Existe";

            List<USUARIO> usuarios = usuarioDA.USUARIO.ToList();

            foreach (USUARIO usuario in usuarios)
            {
                if (usuario.No_Usuario.ToString().Equals(nombre))
                {
                    if (usuario.Tx_Password.ToString().Equals(password))
                    {
                        usuarioValidado = "Valido";
                    }
                    else
                    {
                        usuarioValidado = "No Password";
                    }
                }
            }
            return usuarioValidado;
        }

    }
}
