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
    public class RELATORIO : GPO.FW.dsBase
    {
        #region "Folha Descrição Objetivos"

        public DataSet ListarRELATORIOFolhaDescricaoObjetivos(string pID_EMPRESA, string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine("        OBJ.OBJETIVO,  ");
            stb.AppendLine("        LINHA_MES.DESCRICAO LINHA_MESTRA, ");
            stb.AppendLine("        LINHA_ATU.DESCRICAO LINHA_ATUACAO,  ");
            stb.AppendLine("        OBJ.DESCRICAO, ");
            stb.AppendLine("        F_BUSCA_ORGAO(OBJ.ID_FORMULA, OBJ.ANO) ORGAO, ");
            stb.AppendLine("        F_BUSCA_EXECUTIVO(OBJ.ID_OBJETIVO, 'C')  COORDENADOR, ");
            stb.AppendLine("        F_BUSCA_EXECUTIVO(OBJ.ID_OBJETIVO, 'O')  RESPONSAVEL, ");
            stb.AppendLine("        F_BUSCA_EXECUTIVO(OBJ.ID_OBJETIVO, 'I')  INFORMACAO, ");
            stb.AppendLine("        OBJ.PERIODICIDADE PERIODO,  ");
            stb.AppendLine("        OBJ.SIGLA FORMULA_NOME, ");
            stb.AppendLine("        FAN.UNIDADE FORMULA_UNIDADE,  ");
            stb.AppendLine(" 		CASE    ");
            stb.AppendLine(" 		    WHEN (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = OBJ.ID_OBJETIVO AND OM2.ANO = OBJ.ANO AND OM2.ID_CONCEITO = 5)  ");
            stb.AppendLine(" 		  		  >  ");
            stb.AppendLine(" 		  	     (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = OBJ.ID_OBJETIVO AND OM2.ANO = OBJ.ANO AND OM2.ID_CONCEITO = 1)  ");
            stb.AppendLine(" 		  	     THEN 'Quanto menor melhor'  ");
            stb.AppendLine(" 		  		 ELSE 'Quanto maior melhor' ");
            stb.AppendLine(" 		END META,  ");
            stb.AppendLine("        OBJ.NOME,  ");
            stb.AppendLine("        OBJ.DEFINICAO_INDICADOR, ");
            stb.AppendLine("        OBJ.METODO_CALCULO,  ");
            stb.AppendLine("        OBJ.ID_OBJETIVO,  ");
            stb.AppendLine("        OBJ.ANO ");
            stb.AppendLine("   FROM OBJETIVO OBJ, ");
            stb.AppendLine("        FORMULA_ANO FAN, ");
            stb.AppendLine("        LINHA_ATUACAO LINHA_ATU, ");
            stb.AppendLine("        LINHA_ATUACAO LINHA_MES ");
            stb.AppendLine("  WHERE OBJ.ID_EMPRESA = :P_ID_EMPRESA ");
            stb.AppendLine("    AND OBJ.ANO = :P_ANO ");
            stb.AppendLine("    AND OBJ.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND OBJ.ID_FORMULA = FAN.ID_FORMULA ");
            stb.AppendLine("    AND FAN.ANO = (SELECT MAX(ANO) FROM FORMULA_ANO WHERE FORMULA_ANO.ID_FORMULA = OBJ.ID_FORMULA) ");
            stb.AppendLine("    AND OBJ.ID_ATUACAO = LINHA_ATU.ID_ATUACAO ");
            stb.AppendLine("    AND LINHA_MES.ID_ATUACAO = LINHA_ATU.ID_ATUACAO_PAI ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EMPRESA", pID_EMPRESA));
            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOFolhaDescricaoObjetivosPrj(string pID_EMPRESA, string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT PRO.ID_OBJETIVO,  ");
            stb.AppendLine(" 	   PRO.DESCRICAO,  ");
            stb.AppendLine(" 	   PRO.VALOR ");
            stb.AppendLine("   FROM OBJETIVO_PROJECAO PRO, OBJETIVO OBJ ");
            stb.AppendLine("  WHERE OBJ.ID_EMPRESA = :P_ID_EMPRESA ");
            stb.AppendLine("    AND OBJ.ANO = :P_ANO ");
            stb.AppendLine("    AND OBJ.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND OBJ.ID_OBJETIVO = PRO.ID_OBJETIVO ");
            stb.AppendLine("   ORDER BY PRO.ID_OBJETIVO, PRO.DESCRICAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EMPRESA", pID_EMPRESA));
            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOFolhaDescricaoObjetivosAvl(string pID_EMPRESA, string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine("    MET.ID_OBJETIVO, ");
            stb.AppendLine("    C.DESCRICAO,  ");
            stb.AppendLine("    MET.VALOR_MIN,  ");
            stb.AppendLine("    MET.VALOR_MAX,  ");
            stb.AppendLine("    MET.ANO ");
            stb.AppendLine(" FROM OBJETIVO_META MET, CONCEITO C, OBJETIVO O ");
            stb.AppendLine(" WHERE MET.ID_CONCEITO = C.ID_CONCEITO ");
            stb.AppendLine("    AND MET.ID_OBJETIVO = O.ID_OBJETIVO ");
            stb.AppendLine("    AND O.ID_EMPRESA = :P_ID_EMPRESA ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");
            stb.AppendLine("    AND O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine(" ORDER BY MET.ID_OBJETIVO, MET.ID_CONCEITO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EMPRESA", pID_EMPRESA));
            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOFolhaDescricaoObjetivosLim(string pID_EMPRESA, string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT OL.ID_OBJETIVO_LIMITE,	 ");
            stb.AppendLine(" 	   OL.ID_OBJETIVO,	 ");
            stb.AppendLine(" 	   OL.LIMITE_MAX,	 ");
            stb.AppendLine(" 	   OL.LIMITE_MIN, ");
            stb.AppendLine(" 	   OL.LIMITE_INT, ");
            stb.AppendLine("  	   CASE    ");
            stb.AppendLine("  	   		 WHEN (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = O.ID_OBJETIVO AND OM2.ANO = O.ANO AND OM2.ID_CONCEITO = 5)  ");
            stb.AppendLine("  			 	  >  ");
            stb.AppendLine("  				  (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = O.ID_OBJETIVO AND OM2.ANO = O.ANO AND OM2.ID_CONCEITO = 1)  ");
            stb.AppendLine("  		     THEN 'Quanto menor melhor'  ");
            stb.AppendLine("  			 ELSE 'Quanto maior melhor' ");
            stb.AppendLine("  	   END META ");
            stb.AppendLine(" FROM   OBJETIVO_LIMITE OL, OBJETIVO O ");
            stb.AppendLine(" WHERE OL.ID_OBJETIVO = O.ID_OBJETIVO ");
            stb.AppendLine("    AND O.ID_EMPRESA = :P_ID_EMPRESA ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");
            stb.AppendLine("    AND O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EMPRESA", pID_EMPRESA));
            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region "Fórmula"

        public DataSet ListarRELATORIOFormulas(string pNOME)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT F.ID_FORMULA, F.NOME, FA.FORMULA DES_FORMULA, FA.ANO, F_BUSCA_ORGAO (F.ID_FORMULA, FA.ANO) AS EXECUTIVO ");
            stb.AppendLine("   FROM FORMULA F, FORMULA_ANO FA ");
            stb.AppendLine("  WHERE F.ID_FORMULA = FA.ID_FORMULA AND UPPER (F.NOME) LIKE UPPER(:P_NOME) || '%' ");
            stb.AppendLine(" ORDER BY F.NOME ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_NOME", pNOME));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region "Acompanhamento Mensal Objetivos"

        public DataSet ListarRELATORIOAcompMensalObjetivos(string pID_EXECUTIVO, string pMES, string pANO, bool pPresidencia)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            pMES = "01/" + pMES + "/" + pANO;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine(" 	   EXECUT.POS, ");
            stb.AppendLine("        O.ID_OBJETIVO, ");
            stb.AppendLine("        O.ID_EXECUTIVO, ");
            stb.AppendLine(" 	   E.NOME EXECUTIVO_SIGLA, ");
            stb.AppendLine(" 	   E.DESCRICAO EXECUTIVO_NOME,  ");
            stb.AppendLine(" 	   LA.DESCRICAO ATUACAO, ");
            stb.AppendLine(" 	   O.NOME INDICADOR, ");
            stb.AppendLine(" 	   O.SIGLA, ");
            stb.AppendLine(" 	   O.PERIODICIDADE PERIOD, ");
            stb.AppendLine(" 	   CASE O.PERIODICIDADE  ");
            stb.AppendLine(" 			WHEN 'D' THEN 'Diário' ");
            stb.AppendLine(" 			WHEN 'S' THEN 'Semanal' ");
            stb.AppendLine(" 			WHEN 'Q' THEN 'Quinzenal' ");
            stb.AppendLine(" 			WHEN 'M' THEN 'Mensal' ");
            stb.AppendLine(" 	   		WHEN '2' THEN 'Bimestral' ");
            stb.AppendLine(" 	   		WHEN '3' THEN 'Trimestral' ");
            stb.AppendLine(" 	   		WHEN '4' THEN 'Quadrimestral' ");
            stb.AppendLine(" 	   		WHEN '6' THEN 'Semestral' ");
            stb.AppendLine(" 			WHEN 'A' THEN 'Anual' ");
            stb.AppendLine(" 	   END PERIODICIDADE, ");
            stb.AppendLine(" 	   FA.UNIDADE UNIDADE, ");
            stb.AppendLine(" 	   FA.ID_FORMULA, ");
            stb.AppendLine(" 	   FA.FORMULA, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) VALOR, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) VALOR_ESP, ");
            stb.AppendLine(" 	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC  ");
            stb.AppendLine(" 	        WHERE FA.ID_FORMULA = FC.ID_FORMULA ");
            stb.AppendLine(" 	            AND FA.ANO = FC.ANO ");
            stb.AppendLine(" 	            AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	            AND O.ID_FORMULA = FA.ID_FORMULA) QTD_COMPONENTES, ");
            stb.AppendLine(" 	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN ");
            stb.AppendLine(" 	        WHERE FA.ID_FORMULA = FC.ID_FORMULA ");
            stb.AppendLine(" 	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE ");
            stb.AppendLine(" 	            AND FA.ANO = FC.ANO  ");
            stb.AppendLine(" 	            AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	            AND O.ID_FORMULA = FA.ID_FORMULA ");
            stb.AppendLine(" 	            AND TO_DATE(EN.DATA_REFERENCIA) = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) QTD_ENTRADAS ");
            stb.AppendLine("   FROM OBJETIVO O, ");
            stb.AppendLine("        EXECUTIVO E, ");
            stb.AppendLine("        FORMULA F, ");
            stb.AppendLine("        FORMULA_ANO FA, ");
            stb.AppendLine("        LINHA_ATUACAO LA, ");
            stb.AppendLine(" 	   (select ROWNUM AS POS, lpad('   ',2*(level-1)) || NOME AS NOME, ID_EXECUTIVO ");
            stb.AppendLine(" 		from EXECUTIVO  ");
            if (pPresidencia)
                stb.AppendLine(" 		where ID_SUPERIOR is null ");
            stb.AppendLine(" 		start with ID_EXECUTIVO =: p_ID_EXECUTIVO ");
            stb.AppendLine(" 		connect by NOCYCLE prior ID_EXECUTIVO = ID_SUPERIOR) EXECUT ");
            stb.AppendLine("  WHERE O.ID_EXECUTIVO = E.ID_EXECUTIVO ");
            stb.AppendLine("    AND (    (E.DATA_INICIO <= LAST_DAY (TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) ");
            stb.AppendLine("             ) ");
            stb.AppendLine("         AND (   (E.DATA_FIM IS NULL) ");
            stb.AppendLine("              OR (E.DATA_FIM >= ");
            stb.AppendLine("                               LAST_DAY (TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) ");
            stb.AppendLine("                 ) ");
            stb.AppendLine("             ) ");
            stb.AppendLine("        ) ");
            stb.AppendLine("    AND O.ANO = :P_ANO_PROJECAO ");
            stb.AppendLine("    AND O.ID_FORMULA = F.ID_FORMULA ");
            stb.AppendLine("    AND O.ID_FORMULA = FA.ID_FORMULA AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine("    AND O.ID_ATUACAO = LA.ID_ATUACAO ");
            stb.AppendLine("    AND E.ID_EXECUTIVO = EXECUT.ID_EXECUTIVO ");
            stb.AppendLine(" ORDER BY LA.DESCRICAO, F.NOME ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_MES_ESPERADO", pMES));
            _Command.Parameters.Add(new OracleParameter(":P_ANO_PROJECAO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOAcompMensalObjetivosSimbolos(string pID_EXECUTIVO, string pMES, string pANO, bool pPresidencia)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            pMES = "01/" + pMES + "/" + pANO;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine(" 	   EXECUT.POS, ");
            stb.AppendLine("       O.ID_OBJETIVO, ");
            stb.AppendLine("       O.ID_EXECUTIVO, ");
            stb.AppendLine(" 	   E.NOME EXECUTIVO_SIGLA, ");
            stb.AppendLine(" 	   E.DESCRICAO EXECUTIVO_NOME,  ");
            stb.AppendLine(" 	   LA.DESCRICAO ATUACAO, ");
            stb.AppendLine(" 	   O.NOME INDICADOR, ");
            stb.AppendLine(" 	   O.SIGLA, ");
            stb.AppendLine(" 	   O.PERIODICIDADE PERIOD, ");
            stb.AppendLine(" 	   CASE O.PERIODICIDADE  ");
            stb.AppendLine(" 			WHEN 'D' THEN 'Diário' ");
            stb.AppendLine(" 			WHEN 'S' THEN 'Semanal' ");
            stb.AppendLine(" 			WHEN 'Q' THEN 'Quinzenal' ");
            stb.AppendLine(" 			WHEN 'M' THEN 'Mensal' ");
            stb.AppendLine(" 	   		WHEN '2' THEN 'Bimestral' ");
            stb.AppendLine(" 	   		WHEN '3' THEN 'Trimestral' ");
            stb.AppendLine(" 	   		WHEN '4' THEN 'Quadrimestral' ");
            stb.AppendLine(" 	   		WHEN '6' THEN 'Semestral' ");
            stb.AppendLine(" 			WHEN 'A' THEN 'Anual' ");
            stb.AppendLine(" 	   END PERIODICIDADE, ");
            stb.AppendLine(" 	   FA.UNIDADE UNIDADE, ");
            stb.AppendLine(" 	   FA.ID_FORMULA, ");
            stb.AppendLine(" 	   FA.FORMULA, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) VALOR, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) VALOR_ESP, ");
            stb.AppendLine(" 	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC  ");
            stb.AppendLine(" 	        WHERE FA.ID_FORMULA = FC.ID_FORMULA ");
            stb.AppendLine(" 	            AND FA.ANO = FC.ANO ");
            stb.AppendLine(" 	            AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	            AND O.ID_FORMULA = FA.ID_FORMULA) QTD_COMPONENTES, ");
            stb.AppendLine(" 	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN ");
            stb.AppendLine(" 	        WHERE FA.ID_FORMULA = FC.ID_FORMULA ");
            stb.AppendLine(" 	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE ");
            stb.AppendLine(" 	            AND FA.ANO = FC.ANO  ");
            stb.AppendLine(" 	            AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	            AND O.ID_FORMULA = FA.ID_FORMULA ");
            stb.AppendLine(" 	            AND TO_DATE(EN.DATA_REFERENCIA) = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) QTD_ENTRADAS, ");
            stb.AppendLine(" 	   C.ID_CONCEITO, ");
            stb.AppendLine(" 	   C.DESCRICAO ");
            stb.AppendLine("   FROM OBJETIVO O, ");
            stb.AppendLine("        EXECUTIVO E, ");
            stb.AppendLine("        FORMULA F, ");
            stb.AppendLine("        FORMULA_ANO FA, ");
            stb.AppendLine("        LINHA_ATUACAO LA, ");
            stb.AppendLine("        OBJETIVO_META OM, ");
            stb.AppendLine("        CONCEITO C, ");
            stb.AppendLine(" 	   (select ROWNUM AS POS, lpad('   ',2*(level-1)) || NOME AS NOME, ID_EXECUTIVO ");
            stb.AppendLine(" 		from EXECUTIVO  ");
            if (pPresidencia)
                stb.AppendLine(" 		where ID_SUPERIOR is null ");
            //stb.AppendLine(" 		where substr(upper(descricao),0,3) = 'DIR' or substr(upper(descricao),0,3) = 'PRE' ");
            stb.AppendLine(" 		start with ID_EXECUTIVO =: p_ID_EXECUTIVO ");
            stb.AppendLine(" 		connect by NOCYCLE prior ID_EXECUTIVO = ID_SUPERIOR) EXECUT ");
            stb.AppendLine("  WHERE O.ID_EXECUTIVO = E.ID_EXECUTIVO ");
            stb.AppendLine("    AND (    (E.DATA_INICIO <= LAST_DAY (TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) ");
            stb.AppendLine("             ) ");
            stb.AppendLine("         AND (   (E.DATA_FIM IS NULL) ");
            stb.AppendLine("              OR (E.DATA_FIM >= ");
            stb.AppendLine("                               LAST_DAY (TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) ");
            stb.AppendLine("                 ) ");
            stb.AppendLine("             ) ");
            stb.AppendLine("        ) ");
            stb.AppendLine("    AND O.ANO = :P_ANO_PROJECAO ");
            stb.AppendLine("    AND O.ID_FORMULA = F.ID_FORMULA ");
            stb.AppendLine("    AND O.ID_FORMULA = FA.ID_FORMULA AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine("    AND O.ID_ATUACAO = LA.ID_ATUACAO ");
            stb.AppendLine("    AND E.ID_EXECUTIVO = EXECUT.ID_EXECUTIVO ");
            stb.AppendLine("    AND O.ID_OBJETIVO = OM.ID_OBJETIVO ");
            stb.AppendLine("    AND OM.ID_CONCEITO = C.ID_CONCEITO ");
            stb.AppendLine("    AND (F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) BETWEEN VALOR_MIN AND VALOR_MAX ");
            stb.AppendLine("        OR (F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OM.ID_CONCEITO = 1)) ");
            stb.AppendLine(" ORDER BY LA.DESCRICAO, F.NOME ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_MES_ESPERADO", pMES));
            _Command.Parameters.Add(new OracleParameter(":P_ANO_PROJECAO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region "Resumo dos Objetivos"

        public DataSet ListarRELATORIOResumoObjetivos(string pID_EXECUTIVO, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            //***Legado
            if (Convert.ToInt32(pANO) <= 2013)
            {
                stb.AppendLine(" SELECT  O.ANO,  ");
                stb.AppendLine(" 		 O.ID_OBJETIVO,  ");
                stb.AppendLine(" 		 O.OBJETIVO, ");
                stb.AppendLine("         O.NOME,  ");
                stb.AppendLine(" 		 O.PONTUACAO,  ");
                stb.AppendLine(" 		 CASE WHEN C.ID_CONCEITO IS NULL THEN O.PONTUACAO * 5 ELSE O.PONTUACAO END AS PONTUACAO_CALCULO, ");
                stb.AppendLine(" 		 O.SIGLA, ");
                stb.AppendLine(" 		 FA.UNIDADE,  ");
                stb.AppendLine(" 		 (LM.DESCRICAO) ATUACAO, ");
                //stb.AppendLine(" 		 (LM.DESCRICAO || ' || ' || LA.DESCRICAO) ATUACAO, ");
                stb.AppendLine(" 		 OM.VALOR_MIN, ");
                stb.AppendLine(" 		 OM.VALOR_MAX, ");
                stb.AppendLine(" 		 C.DESCRICAO ");
                stb.AppendLine("     FROM OBJETIVO O, ");
                stb.AppendLine("          LINHA_ATUACAO LA, ");
                stb.AppendLine("          LINHA_ATUACAO LM, ");
                stb.AppendLine("          FORMULA_ANO FA, ");
                stb.AppendLine(" 		  OBJETIVO_META OM, ");
                stb.AppendLine(" 		  CONCEITO C ");
                stb.AppendLine("    WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
                stb.AppendLine("      AND O.ANO = :P_ANO ");
                stb.AppendLine("      AND O.ID_ATUACAO = LA.ID_ATUACAO ");
                stb.AppendLine("      AND LA.ID_ATUACAO_PAI = LM.ID_ATUACAO ");
                stb.AppendLine("      AND O.ID_FORMULA = FA.ID_FORMULA ");
                stb.AppendLine("      AND O.ANO_FORMULA = FA.ANO ");
                stb.AppendLine(" 	  AND O.ID_OBJETIVO = OM.ID_OBJETIVO(+) ");
                stb.AppendLine("      AND O.ANO_FORMULA = OM.ANO(+) ");
                stb.AppendLine(" 	  AND OM.ID_CONCEITO = C.ID_CONCEITO(+) ");
                stb.AppendLine(" ORDER BY LA.ID_ATUACAO, O.SIGLA, C.ID_CONCEITO ");
            }
            else
            {
                stb.AppendLine(" SELECT  O.ANO,  ");
                stb.AppendLine(" 		 O.ID_OBJETIVO,  ");
                stb.AppendLine(" 		 O.OBJETIVO, ");
                stb.AppendLine("         O.NOME,  ");
                stb.AppendLine(" 		 O.PONTUACAO,  ");
                stb.AppendLine(" 		 O.SIGLA, ");
                stb.AppendLine(" 		 FA.UNIDADE,  ");
                stb.AppendLine(" 		 (LM.DESCRICAO) ATUACAO, ");
                //stb.AppendLine(" 		 (LM.DESCRICAO || ' || ' || LA.DESCRICAO) ATUACAO, ");
                stb.AppendLine(" 		 OL.LIMITE_MAX,	 ");
                stb.AppendLine(" 		 OL.LIMITE_MIN, ");
                stb.AppendLine(" 		 OL.LIMITE_INT, ");
                stb.AppendLine(" 		 CASE    ");
                stb.AppendLine(" 		    WHEN (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = O.ID_OBJETIVO AND OM2.ANO = O.ANO AND OM2.ID_CONCEITO = 5)  ");
                stb.AppendLine(" 		  		  >  ");
                stb.AppendLine(" 		  	     (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = O.ID_OBJETIVO AND OM2.ANO = O.ANO AND OM2.ID_CONCEITO = 1)  ");
                stb.AppendLine(" 		  	     THEN 'Quanto menor melhor'  ");
                stb.AppendLine(" 		  		 ELSE 'Quanto maior melhor' ");
                stb.AppendLine(" 		 END META  ");
                stb.AppendLine("     FROM OBJETIVO O, ");
                stb.AppendLine("          LINHA_ATUACAO LA, ");
                stb.AppendLine("          LINHA_ATUACAO LM, ");
                stb.AppendLine("          FORMULA_ANO FA, ");
                stb.AppendLine(" 		  OBJETIVO_LIMITE OL ");
                stb.AppendLine("    WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
                stb.AppendLine("      AND O.ANO = :P_ANO ");
                stb.AppendLine("      AND O.ID_ATUACAO = LA.ID_ATUACAO ");
                stb.AppendLine("      AND LA.ID_ATUACAO_PAI = LM.ID_ATUACAO ");
                stb.AppendLine("      AND O.ID_FORMULA = FA.ID_FORMULA ");
                stb.AppendLine("      AND O.ANO_FORMULA = FA.ANO ");
                stb.AppendLine("      AND O.ID_OBJETIVO = OL.ID_OBJETIVO(+) ");
                stb.AppendLine(" ORDER BY LA.ID_ATUACAO, O.SIGLA ");
            }

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region "Acompanhamento da Avaliação Quantitativa do Órgão"

        public DataSet ListarRELATORIOAvaliacaoQuantitativaOrgao(string pID_EXECUTIVO, string pMES, string pANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            pMES = "01/" + pMES + "/" + pANO;

            stb.AppendLine(" SELECT O.ID_OBJETIVO, ");
            stb.AppendLine(" 	   O.NOME, ");
            stb.AppendLine(" 	   O.DESCRICAO, ");
            stb.AppendLine(" 	   O.SIGLA, ");
            stb.AppendLine(" 	   O.PONTUACAO, ");
            stb.AppendLine(" 	   O.PERIODICIDADE PERIOD, ");
            stb.AppendLine("  	   CASE O.TIPO_FAIXA WHEN 'E' THEN 'Quanto maior melhor' ELSE 'Quanto menor melhor' END SENTIDO_OBJETIVO,   ");            
            stb.AppendLine("  	   CASE O.PERIODICIDADE   ");
            stb.AppendLine("  			WHEN 'D' THEN 'Diário'  ");
            stb.AppendLine("  			WHEN 'S' THEN 'Semanal'  ");
            stb.AppendLine("  			WHEN 'Q' THEN 'Quinzenal'  ");
            stb.AppendLine("  			WHEN 'M' THEN 'Mensal'  ");
            stb.AppendLine("  	   		WHEN '2' THEN 'Bimestral'  ");
            stb.AppendLine("  	   		WHEN '3' THEN 'Trimestral'  ");
            stb.AppendLine("  	   		WHEN '4' THEN 'Quadrimestral'  ");
            stb.AppendLine("  	   		WHEN '6' THEN 'Semestral'  ");
            stb.AppendLine("  			WHEN 'A' THEN 'Anual'  ");
            stb.AppendLine("  	   END PERIODICIDADE,  ");
            stb.AppendLine(" 	   FA.UNIDADE, ");
            stb.AppendLine(" 	   CASE   ");
            stb.AppendLine(" 	   		 WHEN (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = OM.ID_OBJETIVO AND OM2.ANO = OM.ANO AND OM2.ID_CONCEITO = 5) ");
            stb.AppendLine(" 			 	  > ");
            stb.AppendLine(" 				  (SELECT OM2.VALOR_MAX FROM OBJETIVO_META OM2 WHERE OM2.ID_OBJETIVO = OM.ID_OBJETIVO AND OM2.ANO = OM.ANO AND OM2.ID_CONCEITO = 1) ");
            stb.AppendLine(" 		     THEN OM.VALOR_MAX ");
            stb.AppendLine(" 			 ELSE OM.VALOR_MIN  ");
            stb.AppendLine(" 	   END META, ");
            stb.AppendLine(" 	   FESP.VALOR VALOR_ESPERADO, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO, ");
            stb.AppendLine(" 	   OMR.ID_CONCEITO, ");
            stb.AppendLine(" 	   (100 - ((OMR.ID_CONCEITO - 1) * 25)) AS PONTUACAO_MES, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO  ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA) QTD_COMPONENTES,  ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR')) QTD_ENTRADAS, ");
            stb.AppendLine("       PRO.VALOR as PROJECAO_VALOR ");
            stb.AppendLine(" FROM OBJETIVO O, ");
            stb.AppendLine(" 	 OBJETIVO_META OM, ");
            stb.AppendLine(" 	 FORMULA F, ");
            stb.AppendLine(" 	 FORMULA_ANO FA, ");
            stb.AppendLine(" 	 FORMULA_ESPERADO_MENSAL FESP, ");
            stb.AppendLine(" 	 OBJETIVO_META OMR, ");
            stb.AppendLine(" 	 OBJETIVO_PROJECAO PRO ");
            stb.AppendLine(" WHERE O.ID_OBJETIVO = OM.ID_OBJETIVO ");
            stb.AppendLine(" 	  AND O.ANO = OM.ANO ");
            stb.AppendLine(" 	  AND OM.ID_CONCEITO = 3 ");
            stb.AppendLine(" 	  AND O.ID_FORMULA = F.ID_FORMULA ");
            stb.AppendLine(" 	  AND F.ID_FORMULA = FA.ID_FORMULA ");
            stb.AppendLine(" 	  AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	  AND O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("      AND O.ID_FORMULA = FESP.ID_FORMULA(+) ");
            stb.AppendLine("      AND TO_DATE (FESP.DATA_REFERENCIA(+), 'DD/MM/RRRR') = TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR') ");
            stb.AppendLine(" 	  AND O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 	  AND O.ID_OBJETIVO = PRO.ID_OBJETIVO ");
            stb.AppendLine("      AND (F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine("       	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 	  AND O.ANO = :P_ANO ");
            stb.AppendLine(" 	  AND PRO.DESCRICAO = :P_ANO ");
            stb.AppendLine(" ORDER BY O.SIGLA, OM.ID_CONCEITO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_MES_ESPERADO", pMES));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region "Acompanhamento do Objetivo"

        public DataSet ListarRELATORIOAcompanhamentoObjetivo(string pID_EXECUTIVO, string pANO, string pID_OBJETIVO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            string pMES = "01/01/" + pANO;

            stb.AppendLine(" SELECT  ");
            stb.AppendLine(" 	   O.ID_OBJETIVO, ");
            stb.AppendLine(" 	   O.NOME, ");
            stb.AppendLine(" 	   O.DESCRICAO, ");
            stb.AppendLine(" 	   O.SIGLA, ");
            stb.AppendLine(" 	   O.PONTUACAO, ");
            stb.AppendLine(" 	   O.PERIODICIDADE PERIOD, ");
            stb.AppendLine("  	   CASE O.PERIODICIDADE ");
            stb.AppendLine("  			WHEN 'D' THEN 'Diário'  ");
            stb.AppendLine("  			WHEN 'S' THEN 'Semanal'  ");
            stb.AppendLine("  			WHEN 'Q' THEN 'Quinzenal'  ");
            stb.AppendLine("  			WHEN 'M' THEN 'Mensal'  ");
            stb.AppendLine("  	   		WHEN '2' THEN 'Bimestral'  ");
            stb.AppendLine("  	   		WHEN '3' THEN 'Trimestral'  ");
            stb.AppendLine("  	   		WHEN '4' THEN 'Quadrimestral'  ");
            stb.AppendLine("  	   		WHEN '6' THEN 'Semestral'  ");
            stb.AppendLine("  			WHEN 'A' THEN 'Anual'  ");
            stb.AppendLine("  	   END PERIODICIDADE,  ");
            stb.AppendLine(" 	   FA.UNIDADE, ");
            stb.AppendLine(" 	   O.OBS_OBJETIVO, ");
            stb.AppendLine(" 	   EC.NOME NOME_C, ");
            stb.AppendLine(" 	   EI.NOME NOME_I, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO1, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO2, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO3, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO4, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO5, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO6, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO7, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO8, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO9, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO10, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO11, ");
            stb.AppendLine(" 	   F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11), FA.FORMULA, O.ANO_FORMULA) VALOR_REALIZADO12, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0)) VALOR_ESPERADO1, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1)) VALOR_ESPERADO2, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2)) VALOR_ESPERADO3, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3)) VALOR_ESPERADO4, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4)) VALOR_ESPERADO5, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5)) VALOR_ESPERADO6, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6)) VALOR_ESPERADO7, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7)) VALOR_ESPERADO8, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8)) VALOR_ESPERADO9, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9)) VALOR_ESPERADO10, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10)) VALOR_ESPERADO11, ");
            stb.AppendLine(" 	   (SELECT FESP.VALOR FROM FORMULA_ESPERADO_MENSAL FESP WHERE O.ID_FORMULA = FESP.ID_FORMULA AND FESP.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11)) VALOR_ESPERADO12, ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA1,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA2,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA3,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA4,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA5,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA6,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA7,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA8,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA9,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA10,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA11,  ");
            stb.AppendLine(" 	   (LIMITE_INT) VALOR_EXPECTATIVA12, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0)) VALOR_EXPECTATIVA1, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1)) VALOR_EXPECTATIVA2, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2)) VALOR_EXPECTATIVA3, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3)) VALOR_EXPECTATIVA4, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4)) VALOR_EXPECTATIVA5, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5)) VALOR_EXPECTATIVA6, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6)) VALOR_EXPECTATIVA7, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7)) VALOR_EXPECTATIVA8, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8)) VALOR_EXPECTATIVA9, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9)) VALOR_EXPECTATIVA10, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10)) VALOR_EXPECTATIVA11, ");
            //stb.AppendLine(" 	   (SELECT FEM.VALOR FROM FORMULA_EXPECTATIVA_MENSAL FEM WHERE F.ID_FORMULA = FEM.ID_FORMULA AND FEM.DATA_REFERENCIA = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11)) VALOR_EXPECTATIVA12, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO1, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO2, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO3, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO4, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO5, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO6, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO7, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO8, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO9, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO10, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO11, ");
            stb.AppendLine(" 	   (SELECT OMR.ID_CONCEITO FROM OBJETIVO_META OMR  ");
            stb.AppendLine(" 	   		   WHERE O.ID_OBJETIVO = OMR.ID_OBJETIVO ");
            stb.AppendLine(" 	  		   		 AND O.ANO = OMR.ANO ");
            stb.AppendLine(" 				     AND (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11), FA.FORMULA, O.ANO_FORMULA) BETWEEN OMR.VALOR_MIN AND OMR.VALOR_MAX  ");
            stb.AppendLine(" 				      	  OR (F_CALCULA_FORMULA(FA.ID_FORMULA, ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11), FA.FORMULA, O.ANO_FORMULA) IS NULL AND OMR.ID_CONCEITO = 1))  ");
            stb.AppendLine(" 		  ) ID_CONCEITO12, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC   ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO  ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA) QTD_COMPONENTES,  ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 0)) QTD_ENTRADAS1, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 1)) QTD_ENTRADAS2, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 2)) QTD_ENTRADAS3, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 3)) QTD_ENTRADAS4, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 4)) QTD_ENTRADAS5, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 5)) QTD_ENTRADAS6, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 6)) QTD_ENTRADAS7, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 7)) QTD_ENTRADAS8, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 8)) QTD_ENTRADAS9, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 9)) QTD_ENTRADAS10, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 10)) QTD_ENTRADAS11, ");
            stb.AppendLine("  	   (SELECT COUNT(1) FROM FORMULA_ANO FA,FORMULA_COMPONENTE FC,ENTRADA EN  ");
            stb.AppendLine("  	        WHERE FA.ID_FORMULA = FC.ID_FORMULA  ");
            stb.AppendLine("  	            AND FC.ID_COMPONENTE = EN.ID_COMPONENTE  ");
            stb.AppendLine("  	            AND FA.ANO = FC.ANO   ");
            stb.AppendLine("  	            AND O.ANO_FORMULA = FA.ANO  ");
            stb.AppendLine("  	            AND O.ID_FORMULA = FA.ID_FORMULA  ");
            stb.AppendLine("  	            AND TO_DATE(EN.DATA_REFERENCIA) = ADD_MONTHS(TO_DATE (:P_MES_ESPERADO, 'DD/MM/RRRR'), 11)) QTD_ENTRADAS12 ");
            stb.AppendLine(" FROM OBJETIVO O, ");
            stb.AppendLine(" 	 FORMULA F, ");
            stb.AppendLine(" 	 FORMULA_ANO FA, ");
            stb.AppendLine(" 	 OBJETIVO_EXECUTIVO OEC, ");
            stb.AppendLine(" 	 EXECUTIVO EC, ");
            stb.AppendLine(" 	 OBJETIVO_EXECUTIVO OEI, ");
            stb.AppendLine(" 	 EXECUTIVO EI, ");
            stb.AppendLine(" 	 OBJETIVO_LIMITE OL ");
            stb.AppendLine(" WHERE  ");
            stb.AppendLine(" 	  O.ID_FORMULA = F.ID_FORMULA ");
            stb.AppendLine(" 	  AND F.ID_FORMULA = FA.ID_FORMULA ");
            stb.AppendLine(" 	  AND O.ANO_FORMULA = FA.ANO ");
            stb.AppendLine(" 	  AND O.ID_OBJETIVO = OEC.ID_OBJETIVO (+) ");
            stb.AppendLine(" 	  AND OEC.ID_EXECUTIVO = EC.ID_EXECUTIVO (+) ");
            stb.AppendLine(" 	  AND 'C' = OEC.FUNCAO (+) ");
            stb.AppendLine(" 	  AND O.ID_OBJETIVO = OEI.ID_OBJETIVO (+) ");
            stb.AppendLine(" 	  AND OEI.ID_EXECUTIVO = EI.ID_EXECUTIVO (+) ");
            stb.AppendLine(" 	  AND 'I' = OEI.FUNCAO (+) ");
            stb.AppendLine(" 	  AND O.ID_OBJETIVO = OL.ID_OBJETIVO(+) ");
            stb.AppendLine(" 	  AND O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine(" 	  AND O.ANO = :P_ANO ");
            stb.AppendLine(" 	  AND (O.ID_OBJETIVO = :P_ID_OBJETIVO OR :P_ID_OBJETIVO IS NULL) ");
            stb.AppendLine(" ORDER BY O.SIGLA ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_MES_ESPERADO", pMES));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));
            if (pID_OBJETIVO != "-1")
                _Command.Parameters.Add(new OracleParameter(":P_ID_OBJETIVO", pID_OBJETIVO));
            else
                _Command.Parameters.Add(new OracleParameter(":P_ID_OBJETIVO", DBNull.Value));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOAcompanhamentoObjetivoComentarios(string pID_EXECUTIVO, string pANO, string pID_OBJETIVO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            //string pMES = "01/01/" + pANO;

            stb.AppendLine(" SELECT OBJETIVO_OBSERVACAO.ID_OBJETIVO,   ");
            stb.AppendLine("        OBJETIVO_OBSERVACAO.MES as MES_OBS,  ");
            stb.AppendLine(" 	    OBJETIVO_OBSERVACAO.DESCRICAO AS DESCRICAO_OBS");
            stb.AppendLine("   FROM OBJETIVO_OBSERVACAO  ");
            stb.AppendLine("    WHERE OBJETIVO_OBSERVACAO.ID_OBJETIVO IN ( ");
            stb.AppendLine(" 		SELECT O.ID_OBJETIVO ");
            stb.AppendLine(" 		FROM OBJETIVO O ");
            stb.AppendLine(" 		WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine(" 			  AND O.ANO = :P_ANO ");
            stb.AppendLine(" 	          AND (O.ID_OBJETIVO = :P_ID_OBJETIVO OR :P_ID_OBJETIVO IS NULL) ");
            stb.AppendLine("    ) ");
            stb.AppendLine(" ORDER BY OBJETIVO_OBSERVACAO.MES ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", pID_EXECUTIVO));
            //_Command.Parameters.Add(new OracleParameter(":P_MES_ESPERADO", pMES));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", pANO));
            if (pID_OBJETIVO != "-1")
                _Command.Parameters.Add(new OracleParameter(":P_ID_OBJETIVO", pID_OBJETIVO));
            else
                _Command.Parameters.Add(new OracleParameter(":P_ID_OBJETIVO", DBNull.Value));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion

        #region ""

        public DataSet ListarRELATORIOAprovacaoObjetivos(string P_ID_EXECUTIVO, string P_ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine("  SELECT E.ID_EXECUTIVO, E.NOME, ");
            stb.AppendLine("  		SUM(O.PONTUACAO) PONTUACAO, ");
            stb.AppendLine("  		to_char(OA.DATA_APROVACAO, 'dd/mm/yyyy') DATA_APROVACAO, to_char(OA.DATA_APROVACAO_SUP, 'dd/mm/yyyy') DATA_APROVACAO_SUP  ");
            stb.AppendLine("  FROM EXECUTIVO E ");
            stb.AppendLine("  	 LEFT OUTER JOIN OBJETIVO O ON O.ID_EXECUTIVO = E.ID_EXECUTIVO AND O.ANO = :P_ANO   ");
            stb.AppendLine("  	 LEFT OUTER JOIN OBJETIVO_APROVACAO OA ON OA.ID_OBJETIVO = O.ID_OBJETIVO  ");
            stb.AppendLine("  WHERE E.ID_SUPERIOR = :P_ID_EXECUTIVO     ");
            stb.AppendLine("  GROUP BY E.ID_EXECUTIVO, E.NOME, to_char(OA.DATA_APROVACAO, 'dd/mm/yyyy'), to_char(OA.DATA_APROVACAO_SUP, 'dd/mm/yyyy') ");
            stb.AppendLine("  ORDER BY E.NOME ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", P_ID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", P_ANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOAprovadores(string P_ID_OBJETIVO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine("  SELECT OA.ID_APROVACAO,  ");
            stb.AppendLine("    OA.ID_OBJETIVO,  ");
            stb.AppendLine("    OA.ID_USUARIO_APRV,  ");
            stb.AppendLine("    OA.ID_USUARIO_APRV_SUP,  ");
            stb.AppendLine("    OA.RECUSADO_SUP,  ");
            stb.AppendLine("    OA.MOTIVO_RECUSA, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 UA.NICK  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS NOME_APROVADOR, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 OA.DATA_APROVACAO  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS DATA_APROVACAO, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 UAS.NICK  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS NOME_APROVADOR_SUPERIOR, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 OA.DATA_APROVACAO_SUP ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS DATA_APROVACAO_SUP ");
            stb.AppendLine("  FROM OBJETIVO_APROVACAO OA  ");
            stb.AppendLine("      LEFT OUTER JOIN USUARIO UA ON UA.ID_USUARIO = OA.ID_USUARIO_APRV ");
            stb.AppendLine("  	  LEFT OUTER JOIN USUARIO UAS ON UAS.ID_USUARIO = OA.ID_USUARIO_APRV_SUP  ");
            stb.AppendLine("  WHERE OA.ID_OBJETIVO = :P_ID_OBJETIVO  ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_OBJETIVO", P_ID_OBJETIVO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        public DataSet ListarRELATORIOAprovadores(string P_ID_EXECUTIVO, string P_ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine("  SELECT OA.ID_APROVACAO,  e.nome, ");
            stb.AppendLine("    OA.ID_OBJETIVO,  ");
            stb.AppendLine("    OA.ID_USUARIO_APRV,  ");
            stb.AppendLine("    OA.ID_USUARIO_APRV_SUP,  ");
            stb.AppendLine("    OA.RECUSADO_SUP,  ");
            stb.AppendLine("    OA.MOTIVO_RECUSA, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 UA.NICK  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS NOME_APROVADOR, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 OA.DATA_APROVACAO  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS DATA_APROVACAO, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 UAS.NICK  ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS NOME_APROVADOR_SUPERIOR, ");
            stb.AppendLine("    CASE WHEN RECUSADO_SUP = 'N' OR RECUSADO_SUP IS NULL THEN ");
            stb.AppendLine("    			 OA.DATA_APROVACAO_SUP ");
            stb.AppendLine(" 		ELSE ");
            stb.AppendLine(" 			NULL ");
            stb.AppendLine(" 	END AS DATA_APROVACAO_SUP ");
            stb.AppendLine("  FROM OBJETIVO_APROVACAO OA  ");
            stb.AppendLine("  	  INNER JOIN OBJETIVO O ON O.ID_OBJETIVO = OA.ID_OBJETIVO ");
            stb.AppendLine("  	  INNER JOIN EXECUTIVO E ON E.ID_EXECUTIVO = O.ID_EXECUTIVO ");
            stb.AppendLine("      LEFT OUTER JOIN USUARIO UA ON UA.ID_USUARIO = OA.ID_USUARIO_APRV ");
            stb.AppendLine("  	  LEFT OUTER JOIN USUARIO UAS ON UAS.ID_USUARIO = OA.ID_USUARIO_APRV_SUP  ");
            stb.AppendLine("  WHERE O.ID_EXECUTIVO = :P_ID_EXECUTIVO ");
            stb.AppendLine("    AND O.ANO = :P_ANO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":P_ID_EXECUTIVO", P_ID_EXECUTIVO));
            _Command.Parameters.Add(new OracleParameter(":P_ANO", P_ANO));

            lRetorno = ConsultaQueryDataSet(_Command, "RELATORIO");
            return lRetorno;
        }

        #endregion
    }
}
