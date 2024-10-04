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
    public class STAB : GPO.FW.dsBase
    {
        #region "CONSULTAR"

        public DataSet ConsultarEMPREGADOPorMatricula(string MATR_EMPR)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EM.MATR_EMPR, EM.COD_SGEM, EM.NSEQ_SET, CO.NOM_CLBR, S.DES_REDZ_SET, S.DES_COMT_SET ");
            stb.AppendLine(" FROM STAB.EMPREGADO EM, STAB.COLABORADOR CO, STAB.V_SETORES S ");
            stb.AppendLine(" WHERE EM.SIT_EMPR = 'S' ");
            stb.AppendLine(" 	  AND EM.COD_SGEM IN ('GT','GR','SU') ");
            stb.AppendLine(" 	  AND EM.NUM_CLBR = CO.NUM_CLBR ");
            stb.AppendLine(" 	  AND EM.NSEQ_SET = S.NSEQ_SET ");
            stb.AppendLine(" 	  AND MATR_EMPR = :P_MATR_EMPR ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_MATR_EMPR", MATR_EMPR.ToUpper()));

            lRetorno = ConsultaQueryDataSet(_Command, "EMPREGADO");

            return lRetorno;
        }

        #endregion
    }
}
