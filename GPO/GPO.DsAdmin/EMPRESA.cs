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
    public class EMPRESA : GPO.FW.dsBase
    {
        #region "EMPRESA"

        #region "ObterEstrutura"
        public DataSet ObterEstruturaEMPRESA()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("EMPRESA");
            return lDataSet;
        }

        #endregion

        #region "Listar"

        public DataSet ListarEMPRESA()
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EMP.ID_EMPRESA, EMP.DESCRICAO, EMP.SIGLA ");
            stb.AppendLine(" FROM EMPRESA EMP ");
            stb.AppendLine(" ORDER BY EMP.SIGLA, EMP.DESCRICAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            lRetorno = ConsultaQueryDataSet(_Command, "EMPRESA");
            return lRetorno;
        }

        #endregion

        #region "Consultar"

        public DataSet ConsultarEMPRESA(Int32 ID_EMPRESA)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EMP.ID_EMPRESA, EMP.DESCRICAO, EMP.SIGLA ");
            stb.AppendLine(" FROM EMPRESA EMP ");
            stb.AppendLine(" WHERE ID_EMPRESA = :p_ID_EMPRESA ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));

            lRetorno = ConsultaQueryDataSet(_Command, "EMPRESA");
            return lRetorno;
        }

        #endregion

        #endregion
    }
}
