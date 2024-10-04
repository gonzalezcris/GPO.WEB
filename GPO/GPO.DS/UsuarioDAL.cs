using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using GPO.ENTIDADES;

namespace GPO.DS
{
    public class UsuarioDAL : GPO.FW.dsBase
    {

        public DataSet ObterEstruturaUSUARIO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("USUARIO");
            return lDataSet;
        }

        public Usuario validarUsuario(String a_Nome, String a_Senha)
        {
            Usuario l_Usuario = null;
            String l_Senha = String.Empty;
            StringBuilder stb = new StringBuilder();
            DataSet l_dsUsuario = null;
            DataTable l_dtUsuario = null;
            DbCommand l_Comando = null;

            try
            {                
                l_dsUsuario = new DataSet();
                l_dtUsuario = new DataTable();

                stb.AppendLine(" select U.ID_USUARIO, U.NICK, U.SENHA, U.ID_EMPRESA, U.STATUS, U.DT_ACESSO, E.SIGLA ");
                stb.AppendLine(" from gpo.USUARIO U ");
                stb.AppendLine(" INNER JOIN EMPRESA E ON E.ID_EMPRESA = U.ID_EMPRESA ");
                stb.AppendLine(String.Format("where TRIM(NICK) like '{0}'", a_Nome)); 
                l_Comando = (DbCommand)InicializaCommand(stb.ToString());
                l_dsUsuario = ConsultaQueryDataSet(l_Comando, "gpo.USUARIO");

                if ((l_dsUsuario != null) && (l_dsUsuario.Tables.Count == 1))
                {
                    l_dtUsuario = l_dsUsuario.Tables[0];
                    if (l_dtUsuario.Rows.Count == 1)
                    {
                        l_Senha = l_dsUsuario.Tables[0].Rows[0].ItemArray[2].ToString();
                        l_Senha = l_Senha.Trim();

                        if (String.Equals(l_Senha, a_Senha))
                        {
                            l_Usuario = PreencherUsuario(l_dsUsuario);
                            return l_Usuario;
                        }                        
                    }                   
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                l_Comando.Dispose();
                l_dsUsuario.Dispose();
                l_dtUsuario.Dispose();
            }
        }

        public Int32 AlteraStatusUsuario(Int32 a_ID_Usuario)
        {
            StringBuilder stb = new StringBuilder();
            DbCommand l_Comando = null;
            Int32 flagUpdate = 0;

            try
            {
                stb.AppendLine(String.Format("update USUARIO set STATUS='IN' where ID_USUARIO = {0}", a_ID_Usuario));
                l_Comando = (DbCommand)InicializaCommand(stb.ToString());
                flagUpdate = ExecutaNonQuery(l_Comando);
                return flagUpdate;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                l_Comando.Dispose();
            }
        }

        public Usuario PreencherUsuario(DataSet a_dsUsuario)
        {
            Usuario l_Usuario = new Usuario();

            l_Usuario.Id_Usuario = Int32.Parse(a_dsUsuario.Tables[0].Rows[0]["ID_USUARIO"].ToString());
            l_Usuario.Nick = a_dsUsuario.Tables[0].Rows[0]["NICK"].ToString().Trim().ToUpper();
            l_Usuario.Senha = a_dsUsuario.Tables[0].Rows[0]["SENHA"].ToString().Trim().ToUpper();
            l_Usuario.Id_Empresa = Int32.Parse(a_dsUsuario.Tables[0].Rows[0]["ID_EMPRESA"].ToString());
            l_Usuario.Sg_Empresa = a_dsUsuario.Tables[0].Rows[0]["SIGLA"].ToString();
            l_Usuario.Status  = a_dsUsuario.Tables[0].Rows[0]["STATUS"].ToString();

            //String l_status = a_dsUsuario.Tables[0].Rows[0].ItemArray[4].ToString();

            //if (!String.IsNullOrEmpty(l_status))
            //    l_Usuario.Status = Boolean.Parse(l_status.ToString());


            Object l_dataAcesso = a_dsUsuario.Tables[0].Rows[0].ItemArray[5];;
             
            if (! l_dataAcesso.GetType().ToString().Equals("System.DBNull"))
                l_Usuario.DataAcesso = DateTime.Parse(a_dsUsuario.Tables[0].Rows[0].ItemArray[5].ToString());
            
            return l_Usuario;
        }

    }
}
