using GPO.FW;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Web;


namespace GPO.DsAdmin
{
    public class OBJETIVO_APROVACAO : GPO.FW.dsBase
    {

        #region "ObterEstrutura"
        public DataSet ObterEstruturaOBJETIVO_APROVACAO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("OBJETIVO_APROVACAO");
            return lDataSet;
        }

        #endregion

        #region "Incluir"

        public void IncluirOBJETIVO_APROVACAOExecutivoTodos(string P_ID_USUARIO_APRV, string P_ID_EXECUTIVO, string P_ANO)
        {
            try
            {
                IDbCommand _Command = InicializaProcedure("USP_OBJ_APR_I_EXE_TODOS");

                _Command.Parameters.Add(new OracleParameter("P_ID_USUARIO_APRV", P_ID_USUARIO_APRV));
                _Command.Parameters.Add(new OracleParameter("P_ID_EXECUTIVO", P_ID_EXECUTIVO));
                _Command.Parameters.Add(new OracleParameter("P_ANO", P_ANO));

                ExecutaNonQuery(_Command);
                TransactionManager.SetComplete();
            }
            catch (Exception ex)
            {
                TransactionManager.SetAbort();
                throw ex;
            }
        }

        public void IncluirOBJETIVO_APROVACAOExecutivo(DataSet vDataSet)
        {
            try
            {
                IDbCommand _Command = InicializaProcedure("USP_OBJ_APR_I_EXE");

                foreach (DataRow lRow in vDataSet.Tables[0].Rows)
                {
                    _Command.Parameters.Clear();

                    _Command.Parameters.Add(new OracleParameter("P_ID_USUARIO_APRV", lRow["ID_USUARIO_APRV"]));
                    _Command.Parameters.Add(new OracleParameter("P_ID_OBJETIVO", lRow["ID_OBJETIVO"]));

                    ExecutaNonQuery(_Command);
                }

                TransactionManager.SetComplete();
            }
            catch (Exception ex)
            {
                TransactionManager.SetAbort();
                throw ex;
            }
        }

        #endregion

        #region "Alterar"

        public void AlterarOBJETIVO_APROVACAOSuperior(DataSet vDataSet)
        {
            try
            {
                IDbCommand _Command = InicializaProcedure("USP_OBJ_APR_U_SUP");

                foreach (DataRow lRow in vDataSet.Tables[0].Rows)
                {
                    _Command.Parameters.Clear();

                    _Command.Parameters.Add(new OracleParameter("P_ID_USUARIO_APRV_SUP", lRow["ID_USUARIO_APRV_SUP"]));
                    _Command.Parameters.Add(new OracleParameter("P_ID_OBJETIVO", lRow["ID_OBJETIVO"]));
                    _Command.Parameters.Add(new OracleParameter("P_RECUSADO_SUP", lRow["RECUSADO_SUP"]));

                    ExecutaNonQuery(_Command);
                }

                TransactionManager.SetComplete();
            }
            catch (Exception ex)
            {
                TransactionManager.SetAbort();
                throw ex;
            }
        }

        public void AlterarOBJETIVO_APROVACAOMotivoRecusa(DataSet vDataSet)
        {
            try
            {
                IDbCommand _Command = InicializaProcedure("USP_OBJ_APR_U_MOT");

                foreach (DataRow lRow in vDataSet.Tables[0].Rows)
                {
                    _Command.Parameters.Clear();

                    _Command.Parameters.Add(new OracleParameter("P_ID_OBJETIVO", lRow["ID_OBJETIVO"]));
                    _Command.Parameters.Add(new OracleParameter("P_MOTIVO_RECUSA", lRow["MOTIVO_RECUSA"]));

                    ExecutaNonQuery(_Command);
                }

                TransactionManager.SetComplete();
            }
            catch (Exception ex)
            {
                TransactionManager.SetAbort();
                throw ex;
            }
        }

       public void CancelarOBJETIVO_APROVACAO(DataSet vDataSet)
       {
          try
          {
             IDbCommand _Command = InicializaProcedure("SP_OBJ_APR_CANCEL_U");

             foreach (DataRow lRow in vDataSet.Tables[0].Rows)
             {
                _Command.Parameters.Clear();

                _Command.Parameters.Add(new OracleParameter("P_ID_APROVACAO", lRow["ID_APROVACAO"]));
                _Command.Parameters.Add(new OracleParameter("P_ID_USER_CANCEL", HttpContext.Current.Session["_ID_USUARIO"].ToString()));

                ExecutaNonQuery(_Command);
             }

             TransactionManager.SetComplete();
          }
          catch (Exception ex)
          {
             TransactionManager.SetAbort();
             throw ex;
          }
       }

        #endregion

        #region "Listar"

        #endregion

        #region "Consultar"

        public DataSet ConsultarOBJETIVO_APROVACAO(string P_ID_APROVACAO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT OA.ID_APROVACAO, ");
            stb.AppendLine("   OA.ID_OBJETIVO, ");
            stb.AppendLine("   OA.ID_USUARIO_APRV, ");
            stb.AppendLine("   OA.DATA_APROVACAO, ");
            stb.AppendLine("   OA.ID_USUARIO_APRV_SUP, ");
            stb.AppendLine("   OA.DATA_APROVACAO_SUP,  ");
            stb.AppendLine("   OA.RECUSADO_SUP, ");
            stb.AppendLine("   OA.MOTIVO_RECUSA ");
            stb.AppendLine(" FROM OBJETIVO_APROVACAO OA ");
            stb.AppendLine(" WHERE OA.ID_APROVACAO = :P_ID_APROVACAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_APROVACAO", P_ID_APROVACAO));

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO_APROVACAO");
            return lRetorno;
        }

        #endregion

    }
}
