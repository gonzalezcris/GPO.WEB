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
    public class USUARIO : GPO.FW.dsBase
    {

        #region "ObterEstrutura"
        public DataSet ObterEstruturaUSUARIO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("USUARIO");
            return lDataSet;
        }

        #endregion

        #region "Listar"

        public DataSet ListarUSUARIO_EXECPorUsuarioETipo(string ID_USUARIO, string TIPO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT UE.ID_USUARIO, UE.ID_EXECUTIVO,UE.TIPO ");
            stb.AppendLine(" FROM USUARIO_EXEC UE ");
            stb.AppendLine(" WHERE UE.ID_USUARIO = :p_ID_USUARIO ");
            stb.AppendLine("    AND UE.TIPO = :p_TIPO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            if (!String.IsNullOrEmpty(ID_USUARIO))
                _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
            else
                _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
            if (!String.IsNullOrEmpty(TIPO))
                _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
            else
                _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));

            lRetorno = ConsultaQueryDataSet(_Command, "USUARIO_EXEC");
            return lRetorno;
        }

        #endregion

    }
}
