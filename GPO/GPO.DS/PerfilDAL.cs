using System;
using System.Collections.Generic;
using System.Text;
using GPO.ENTIDADES;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace GPO.DS
{
    public class PerfilDAL : GPO.FW.dsBase
    {
        public Perfil ConsultaPerfilUsuario(Int32 a_Id_Usuario)
        {
            StringBuilder stb = new StringBuilder();
            DataSet l_dsPerfil = null;
            DataTable l_dtPerfil = null;
            DbCommand l_Comando = null;
            Perfil l_PerfilUsuario = null;

            try
            {
                l_dsPerfil = new DataSet();
                l_dtPerfil = new DataTable();

                stb.AppendLine("select PERFIL.ID_PERFIL, PERFIL.SIGLA, PERFIL.DESCRICAO from PERFIL_USUARIO ");
                stb.AppendLine("INNER JOIN PERFIL ON PERFIL_USUARIO.ID_PERFIL = PERFIL.ID_PERFIL ");
                stb.AppendLine(String.Format("WHERE PERFIL_USUARIO.ID_USUARIO = {0}", a_Id_Usuario));
                l_Comando = (DbCommand)InicializaCommand(stb.ToString());
                l_dsPerfil = ConsultaQueryDataSet(l_Comando, "gpo.PERFIL_USUARIO");

                if ((l_dsPerfil != null) && (l_dsPerfil.Tables.Count == 1))
                {
                    l_dtPerfil = l_dsPerfil.Tables[0];
                    if (l_dtPerfil.Rows.Count == 1)
                    {
                        l_PerfilUsuario = new Perfil();
                        l_PerfilUsuario = PreencherPerfil(l_dsPerfil);
                    }                    
                }

                return l_PerfilUsuario;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                l_Comando.Dispose();
                l_dsPerfil.Dispose();
                l_dtPerfil.Dispose();
            }
        }

        public Perfil PreencherPerfil(DataSet a_dsPerfil)
        {
            Perfil l_Perfil = new Perfil();

            l_Perfil.Id_Perfil = Int32.Parse(a_dsPerfil.Tables[0].Rows[0].ItemArray[0].ToString());
            l_Perfil.Sigla = a_dsPerfil.Tables[0].Rows[0].ItemArray[1].ToString();
            l_Perfil.Descricao = a_dsPerfil.Tables[0].Rows[0].ItemArray[2].ToString();

            return l_Perfil;
        }
    }
}
