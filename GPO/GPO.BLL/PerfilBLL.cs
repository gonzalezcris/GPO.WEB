using System;
using System.Collections.Generic;
using System.Text;
using GPO.DS;
using GPO.ENTIDADES;
using GPO.FW;

namespace GPO.BLL
{
    public class PerfilBLL
    {
        public Perfil ConsultaPerfilUsuario(Int32 a_Id_Usuario)
        {
            PerfilDAL perfilDAL = null;
            Perfil l_Perfil = null;

            try
            {
                perfilDAL = new PerfilDAL();
                perfilDAL.CriaConexao();
                l_Perfil = perfilDAL.ConsultaPerfilUsuario(a_Id_Usuario);

                return l_Perfil;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                perfilDAL.Dispose();                
            }            
        }

    }
}
