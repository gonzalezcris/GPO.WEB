using System;
using System.Collections.Generic;
using System.Text;
using GPO.DS;
using GPO.ENTIDADES;
using GPO.FW;

namespace GPO.BLL
{
    public class UsuarioBLL
    {
        public Usuario ValidarUsuario(String a_Nick, String a_Senha)
        {
            UsuarioDAL usuarioDAL = null;
            Usuario l_Usuario = null;

            try
            {
                usuarioDAL = new UsuarioDAL();
                usuarioDAL.CriaConexao();
                l_Usuario = usuarioDAL.validarUsuario(a_Nick, a_Senha);

                return l_Usuario;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                usuarioDAL.Dispose();                
            }            
        }

        public void AtualizaStatusUsuario(Int32 a_Id_Usuario)
        {
            UsuarioDAL usuarioDAL = null;
            int flagUpdate = 0;

            try
            {
                usuarioDAL = new UsuarioDAL();
                flagUpdate = usuarioDAL.AlteraStatusUsuario(a_Id_Usuario);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                usuarioDAL.Dispose();
            }
        }
        
    }
}
