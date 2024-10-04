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
    public class OBJETIVO : GPO.FW.dsBase
    {
        #region "OBJETIVO"

        #region "ObterEstrutura"
        public DataSet ObterEstruturaOBJETIVO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("OBJETIVO");
            return lDataSet;
        }

        #endregion

        #region "Listar"

        public DataSet ListarOBJETIVOPorOrgao(string ID_EXECUTIVO, string ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine("   O.ID_OBJETIVO, ");
            stb.AppendLine("   O.DESCRICAO, ");
            stb.AppendLine("   O.ID_FORMULA, ");
            stb.AppendLine("   O.ID_ATUACAO, ");
            stb.AppendLine("   O.ID_EXECUTIVO, ");
            stb.AppendLine("   O.PERIODICIDADE, ");
            //stb.AppendLine("   O.DEFINICAO_INDICADOR, ");
            //stb.AppendLine("   O.METODO_CALCULO, ");
            stb.AppendLine("   O.PONTUACAO, ");
            stb.AppendLine("   O.TIPO_FAIXA, ");
            stb.AppendLine("   O.NOME, ");
            stb.AppendLine("   O.VINCULO, ");
            stb.AppendLine("   O.OBS_OBJETIVO, ");
            stb.AppendLine("   O.OBJETIVO, ");
            stb.AppendLine("   O.SIGLA, ");
            stb.AppendLine("   O.ANO, ");
            stb.AppendLine("   O.ID_EMPRESA, ");
            stb.AppendLine("   O.ANO_FORMULA, ");
            stb.AppendLine("   O.DT_BLOQ ");
            stb.AppendLine(" FROM OBJETIVO O ");
            stb.AppendLine(" WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");
            stb.AppendLine(" ORDER BY O.SIGLA  ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", ID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", ANO));

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO");
            return lRetorno;
        }

        public DataSet ListarOBJETIVOAno()
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT DISTINCT OBJ.ANO ");
            stb.AppendLine(" FROM OBJETIVO OBJ ");
            stb.AppendLine(" ORDER BY OBJ.ANO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO");
            return lRetorno;
        }

        public DataSet ListarOBJETIVOParaAprovacaoExecutivo(string ID_EXECUTIVO, string ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT o.ID_EXECUTIVO, O.ID_OBJETIVO, O.NOME, O.SIGLA, O.PONTUACAO, ");
            stb.AppendLine(" 	   OL.LIMITE_MAX, OL.LIMITE_MIN, OL.LIMITE_INT, ");
            stb.AppendLine(" 	   OA.ID_USUARIO_APRV, UA.NICK AS NICK_A, TO_CHAR(OA.DATA_APROVACAO, 'DD/MM/YYYY') DATA_APROVACAO, ");
            stb.AppendLine(" 	   OA.ID_USUARIO_APRV_SUP, UAS.NICK AS NICK_AS, TO_CHAR(OA.DATA_APROVACAO_SUP, 'DD/MM/YYYY') DATA_APROVACAO_SUP, ");
            stb.AppendLine(" 	   OA.RECUSADO_SUP, OA.MOTIVO_RECUSA, OA.ID_APROVACAO, TO_CHAR(OA.DATA_CANCEL, 'DD/MM/YYYY') DATA_CANCEL ");
            stb.AppendLine(" FROM OBJETIVO O ");
            stb.AppendLine(" 	 LEFT OUTER JOIN OBJETIVO_LIMITE OL ON OL.ID_OBJETIVO = O.ID_OBJETIVO ");
            stb.AppendLine(" 	 LEFT OUTER JOIN OBJETIVO_APROVACAO OA ON OA.ID_OBJETIVO = O.ID_OBJETIVO ");
            stb.AppendLine(" 	 LEFT OUTER JOIN USUARIO UA ON UA.ID_USUARIO = OA.ID_USUARIO_APRV ");
            stb.AppendLine(" 	 LEFT OUTER JOIN USUARIO UAS ON UAS.ID_USUARIO = OA.ID_USUARIO_APRV_SUP ");
            stb.AppendLine(" WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");
            stb.AppendLine(" ORDER BY O.SIGLA  ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", ID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", ANO));

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO");
            return lRetorno;
        }

        public DataSet ListarOBJETIVOParaMotivoRecusa(string ID_EXECUTIVO, string ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine("  SELECT O.ID_OBJETIVO, O.NOME, O.SIGLA, O.PONTUACAO  ");
            stb.AppendLine("  FROM OBJETIVO O   ");
            stb.AppendLine("  	 LEFT OUTER JOIN OBJETIVO_APROVACAO OA ON OA.ID_OBJETIVO = O.ID_OBJETIVO  ");
            stb.AppendLine("  WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO  ");
            stb.AppendLine("     AND O.ANO = :P_ANO ");
            stb.AppendLine(" 	AND OA.RECUSADO_SUP = 'S'  ");
            stb.AppendLine(" 	AND OA.MOTIVO_RECUSA IS NULL  ");
            stb.AppendLine("  ORDER BY O.SIGLA ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", ID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", ANO));

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO");
            return lRetorno;
        }

        #endregion

        #region "Consultar"

        public DataSet ConsultarOBJETIVOPorSiglaExecutivoAno(string pSIGLA, string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine("   O.ID_OBJETIVO, ");
            stb.AppendLine("   O.DESCRICAO, ");
            stb.AppendLine("   O.ID_FORMULA, ");
            stb.AppendLine("   O.ID_ATUACAO, ");
            stb.AppendLine("   O.ID_EXECUTIVO, ");
            stb.AppendLine("   O.PERIODICIDADE, ");
            //stb.AppendLine("   O.DEFINICAO_INDICADOR, ");
            //stb.AppendLine("   O.METODO_CALCULO, ");
            stb.AppendLine("   O.PONTUACAO, ");
            stb.AppendLine("   O.TIPO_FAIXA, ");
            stb.AppendLine("   O.NOME, ");
            stb.AppendLine("   O.VINCULO, ");
            stb.AppendLine("   O.OBS_OBJETIVO, ");
            stb.AppendLine("   O.OBJETIVO, ");
            stb.AppendLine("   O.SIGLA, ");
            stb.AppendLine("   O.ANO, ");
            stb.AppendLine("   O.ID_EMPRESA, ");
            stb.AppendLine("   O.ANO_FORMULA, ");
            stb.AppendLine("   O.DT_BLOQ ");
            stb.AppendLine(" FROM OBJETIVO O ");
            stb.AppendLine(" WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");
            stb.AppendLine("    AND UPPER(TRIM(O.SIGLA)) = :P_SIGLA ");
            stb.AppendLine(" ORDER BY O.SIGLA  ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));
            _Command.Parameters.Add(new OracleParameter(":P_SIGLA", pSIGLA.ToString()));

            lRetorno = ConsultaQueryDataSet(_Command, "OBJETIVO");
            return lRetorno;
        }

        #endregion

        #endregion
    }
}
