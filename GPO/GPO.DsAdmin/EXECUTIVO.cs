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
    public class EXECUTIVO : GPO.FW.dsBase
    {
        #region "EXECUTIVO"

        #region "ObterEstrutura"
        public DataSet ObterEstruturaEXECUTIVO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("EXECUTIVO");
            return lDataSet;
        }

        #endregion

        #region "Listar"

        public DataSet ListarEXECUTIVO()
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EXC.ID_EXECUTIVO, EXC.NOME, EXC.ID_ORGANOGRAMA, EXC.ID_SUPERIOR, EXC.DESCRICAO, EXC.STATUS, ");
            stb.AppendLine("    EXC.ANO, EXC.ID_EMPRESA, EXC.DATA_INICIO, EXC.DATA_FIM, EXC.ID_EXEC_ATUAL, EXC.NSEQ_SET1, EXC.NSEQ_SET, ");
            stb.AppendLine("    EMP.DESCRICAO AS EMP_DESCRICAO, EMP.SIGLA, ORG.NOME AS ORG_NOME ");
            stb.AppendLine(" FROM EXECUTIVO EXC ");
            stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
            stb.AppendLine("    INNER JOIN ORGANOGRAMA ORG ON EXC.ID_ORGANOGRAMA = ORG.ID_ORGANOGRAMA ");
            stb.AppendLine(" ORDER BY EXC.NOME, EXC.DESCRICAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
            return lRetorno;
        }

        public DataSet ListarEXECUTIVO(string ID_EMPRESA)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EXC.ID_EXECUTIVO, EXC.NOME, EXC.ID_ORGANOGRAMA, EXC.ID_SUPERIOR, EXC.DESCRICAO, EXC.STATUS, ");
            stb.AppendLine("    EXC.ANO, EXC.ID_EMPRESA, EXC.DATA_INICIO, EXC.DATA_FIM, EXC.ID_EXEC_ATUAL, EXC.NSEQ_SET1, EXC.NSEQ_SET, ");
            stb.AppendLine("    EMP.DESCRICAO AS EMP_DESCRICAO, EMP.SIGLA, ORG.NOME AS ORG_NOME ");
            stb.AppendLine(" FROM EXECUTIVO EXC ");
            stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
            stb.AppendLine("    INNER JOIN ORGANOGRAMA ORG ON EXC.ID_ORGANOGRAMA = ORG.ID_ORGANOGRAMA ");
            stb.AppendLine(" WHERE EXC.ID_EMPRESA = :p_ID_EMPRESA ");
            stb.AppendLine(" ORDER BY EXC.NOME, EXC.DESCRICAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            if (ID_EMPRESA != "-1")
                _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
            else
                _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));

            lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
            return lRetorno;
        }

        //public DataSet ListarEXECUTIVO(string ID_EMPRESA, string ID_USUARIO, string TIPO)
        //{
        //    StringBuilder stb = new StringBuilder();
        //    DataSet lRetorno;
        //    DbCommand _Command;

        //    stb.AppendLine(" SELECT EXC.ID_EXECUTIVO, EXC.NOME, EXC.ID_ORGANOGRAMA, EXC.ID_SUPERIOR, EXC.DESCRICAO, EXC.STATUS, ");
        //    stb.AppendLine("    EXC.ANO, EXC.ID_EMPRESA, EXC.DATA_INICIO, EXC.DATA_FIM, EXC.ID_EXEC_ATUAL, EXC.NSEQ_SET1, EXC.NSEQ_SET, ");
        //    stb.AppendLine("    EMP.DESCRICAO AS EMP_DESCRICAO, EMP.SIGLA, ORG.NOME AS ORG_NOME ");
        //    stb.AppendLine(" FROM EXECUTIVO EXC ");
        //    stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
        //    stb.AppendLine("    INNER JOIN ORGANOGRAMA ORG ON EXC.ID_ORGANOGRAMA = ORG.ID_ORGANOGRAMA ");
        //    stb.AppendLine("    INNER JOIN USUARIO_EXEC UE ON EXC.ID_EXECUTIVO = UE.ID_EXECUTIVO  ");
        //    stb.AppendLine(" WHERE EXC.ID_EMPRESA = :p_ID_EMPRESA ");
        //    stb.AppendLine("    AND EXC.ID_EMPRESA = :p_ID_EMPRESA ");
        //    stb.AppendLine("    AND UE.ID_USUARIO = :p_ID_USUARIO ");
        //    stb.AppendLine("    AND UE.TIPO = :p_TIPO ");
        //    stb.AppendLine(" ORDER BY EXC.NOME, EXC.DESCRICAO ");

        //    _Command = (DbCommand)InicializaCommand(stb.ToString());

        //    if (ID_EMPRESA != "-1")
        //        _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
        //    else
        //        _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));
        //    if (!String.IsNullOrEmpty(ID_USUARIO))
        //        _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
        //    else
        //        _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
        //    if (!String.IsNullOrEmpty(TIPO))
        //        _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
        //    else
        //        _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));

        //    lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
        //    return lRetorno;
        //}
        
       public DataSet ListarEXECUTIVO(string ID_EMPRESA, string ID_USUARIO, string TIPO)
       {
          StringBuilder stb = new StringBuilder();
          DataSet lRetorno;
          DbCommand _Command;

          stb.AppendLine(" SELECT distinct E.id_executivo, E.nome, E.id_superior ");
          stb.AppendLine("    FROM EXECUTIVO E ");
          stb.AppendLine("    WHERE E.ID_EMPRESA = :p_ID_EMPRESA ");
          stb.AppendLine(" AND E.DATA_FIM IS NULL ");
          stb.AppendLine(" START WITH ID_SUPERIOR in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
          stb.AppendLine("    CONNECT BY NOCYCLE PRIOR ID_EXECUTIVO = ID_SUPERIOR");
          stb.AppendLine("    UNION ");
          stb.AppendLine(" SELECT E.id_executivo, E.nome, E.id_superior ");
          stb.AppendLine("  FROM EXECUTIVO E ");
          stb.AppendLine("  WHERE E.ID_EXECUTIVO  in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
          stb.AppendLine("    AND E.ID_EMPRESA = :p_ID_EMPRESA ");
          stb.AppendLine("   order by nome ");

          _Command = (DbCommand)InicializaCommand(stb.ToString());

          if (ID_EMPRESA != "-1")
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));
          if (!String.IsNullOrEmpty(ID_USUARIO))
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
          if (!String.IsNullOrEmpty(TIPO))
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));

          lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
          return lRetorno;
       }

        public DataSet ListarEXECUTIVOParaAprovacaoSuperior(string ID_EMPRESA, string ID_USUARIO, string TIPO, string ANO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT FILHO.ID_EXECUTIVO, FILHO.NOME, FILHO.DESCRICAO ");
            stb.AppendLine(" FROM EXECUTIVO EXC ");
            stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
            stb.AppendLine("    INNER JOIN USUARIO_EXEC UE ON EXC.ID_EXECUTIVO = UE.ID_EXECUTIVO  ");
            stb.AppendLine("    INNER JOIN EXECUTIVO FILHO ON FILHO.ID_SUPERIOR = EXC.ID_EXECUTIVO ");
            stb.AppendLine(" WHERE EXC.ID_EMPRESA = :p_ID_EMPRESA ");
            stb.AppendLine("    AND EXC.ID_EMPRESA = :p_ID_EMPRESA ");
            stb.AppendLine("    AND UE.ID_USUARIO = :p_ID_USUARIO ");
            stb.AppendLine("    AND UE.TIPO = :p_TIPO ");
            stb.AppendLine("    AND (SELECT count(1) FROM OBJETIVO O WHERE O.ID_EXECUTIVO = FILHO.ID_EXECUTIVO AND O.ANO = :p_ANO) > 0 ");
            stb.AppendLine("    AND (SELECT count(1) FROM OBJETIVO O WHERE O.ID_EXECUTIVO = FILHO.ID_EXECUTIVO AND O.ANO = :p_ANO) = ");
            stb.AppendLine("        (SELECT count(1) FROM OBJETIVO_APROVACAO OA WHERE OA.ID_USUARIO_APRV IS NOT NULL AND OA.RECUSADO_SUP IS NULL AND OA.ID_OBJETIVO IN ");
            stb.AppendLine("            (SELECT ID_OBJETIVO FROM OBJETIVO O WHERE O.ID_EXECUTIVO = FILHO.ID_EXECUTIVO AND O.ANO = :p_ANO)) ");
            stb.AppendLine(" ORDER BY FILHO.NOME, FILHO.DESCRICAO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            if (ID_EMPRESA != "-1")
                _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
            else
                _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));
            if (!String.IsNullOrEmpty(ID_USUARIO))
                _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
            else
                _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
            if (!String.IsNullOrEmpty(TIPO))
                _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
            else
                _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));
            if (!String.IsNullOrEmpty(ANO))
                _Command.Parameters.Add(new OracleParameter(":p_ANO", ANO));
            else
                _Command.Parameters.Add(new OracleParameter(":p_ANO", DBNull.Value));

            lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
            return lRetorno;
        }

       public DataSet ListarOrgaoMenuExecutivo(string ID_EMPRESA, string ID_USUARIO, string TIPO)
       {
          StringBuilder stb = new StringBuilder();
          DataSet lRetorno;
          DbCommand _Command;

          stb.AppendLine(" SELECT p.id_perfil ");
          stb.AppendLine(" FROM PERFIL P, PERFIL_USUARIO PU ");
          stb.AppendLine(" WHERE P.ID_PERFIL = PU.ID_PERFIL ");          
          stb.AppendLine(" AND P.sigla = 'P-0' ");
          stb.AppendLine(" AND PU.ID_USUARIO =  :p_ID_USUARIO ");

          _Command = (DbCommand)InicializaCommand(stb.ToString());
          
          if (!String.IsNullOrEmpty(ID_USUARIO))
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));

          lRetorno = ConsultaQueryDataSet(_Command, "PERFIL");

          stb.Remove(0, stb.Length);
          
          // usuario com perfil de admistrador exibe todos os orgão vinculado a eles e mais a sua hierarquia completa
          if (lRetorno.Tables[0].Rows.Count != 0)
          {          
             stb.AppendLine(" SELECT distinct E.id_executivo, E.nome, E.id_superior ");
             stb.AppendLine("    FROM EXECUTIVO E ");
             stb.AppendLine("    WHERE E.ID_EMPRESA = :p_ID_EMPRESA ");
             stb.AppendLine(" AND E.DATA_FIM IS NULL ");
             stb.AppendLine(" START WITH ID_SUPERIOR in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
             stb.AppendLine("    CONNECT BY NOCYCLE PRIOR ID_EXECUTIVO = ID_SUPERIOR");
             stb.AppendLine("    UNION ");
             stb.AppendLine(" SELECT E.id_executivo, E.nome, E.id_superior ");
             stb.AppendLine("  FROM EXECUTIVO E ");
             stb.AppendLine("  WHERE E.ID_EXECUTIVO  in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
             stb.AppendLine("    AND E.ID_EMPRESA = :p_ID_EMPRESA ");
             stb.AppendLine("   order by nome ");
          }
          else // exibe apenas o proprio executivo sem hierarquia
          {
             stb.AppendLine(" SELECT E.id_executivo, E.nome, E.id_superior ");
             stb.AppendLine("    FROM EXECUTIVO E ");            
             stb.AppendLine("  WHERE E.ID_EXECUTIVO  in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
             stb.AppendLine("    AND E.ID_EMPRESA = :p_ID_EMPRESA ");
             stb.AppendLine("   order by nome ");          
          }

          _Command = (DbCommand)InicializaCommand(stb.ToString());

          if (ID_EMPRESA != "-1")
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));
          if (!String.IsNullOrEmpty(ID_USUARIO))
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
          if (!String.IsNullOrEmpty(TIPO))
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));

          lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
          return lRetorno;
       }

       public DataSet ListarOrgaoMenuSuperior(string ID_EMPRESA, string ID_USUARIO, string TIPO)
       {
          StringBuilder stb = new StringBuilder();
          DataSet lRetorno;
          DbCommand _Command;

          stb.AppendLine(" SELECT p.id_perfil ");
          stb.AppendLine(" FROM PERFIL P, PERFIL_USUARIO PU ");
          stb.AppendLine(" WHERE P.ID_PERFIL = PU.ID_PERFIL ");
          stb.AppendLine(" AND P.sigla = 'P-0' ");
          stb.AppendLine(" AND PU.ID_USUARIO =  :p_ID_USUARIO ");

          _Command = (DbCommand)InicializaCommand(stb.ToString());

          if (!String.IsNullOrEmpty(ID_USUARIO))
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));

          lRetorno = ConsultaQueryDataSet(_Command, "PERFIL");

          stb.Remove(0, stb.Length);

          // usuario com perfil de admistrador exibe todos os orgão vinculado a eles e mais a sua hierarquia completa
          if (lRetorno.Tables[0].Rows.Count != 0)
          {
             stb.AppendLine(" SELECT distinct E.id_executivo, E.nome, E.id_superior ");
             stb.AppendLine("    FROM EXECUTIVO E ");
             stb.AppendLine("    WHERE E.ID_EMPRESA = :p_ID_EMPRESA ");
             stb.AppendLine(" AND E.DATA_FIM IS NULL ");
             stb.AppendLine(" START WITH ID_SUPERIOR in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
             stb.AppendLine("    CONNECT BY NOCYCLE PRIOR ID_EXECUTIVO = ID_SUPERIOR");
             stb.AppendLine("    UNION ");
             stb.AppendLine(" SELECT E.id_executivo, E.nome, E.id_superior ");
             stb.AppendLine("  FROM EXECUTIVO E ");
             stb.AppendLine("  WHERE E.ID_EXECUTIVO  in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO) ");
             stb.AppendLine("    AND E.ID_EMPRESA = :p_ID_EMPRESA ");
             stb.AppendLine("   order by nome ");
          }
          else //exibe apenas a hierarquia no primeiro nivel
          {
             stb.AppendLine(" SELECT E.id_executivo, E.nome, E.id_superior, level ");
             stb.AppendLine("    FROM EXECUTIVO E ");
             stb.AppendLine("  WHERE E.ID_EMPRESA = :p_ID_EMPRESA  ");
             stb.AppendLine("    AND E.DATA_FIM IS NULL ");
             stb.AppendLine("   AND LEVEL = 1 ");
             stb.AppendLine("   START WITH ID_SUPERIOR in (select ID_EXECUTIVO from USUARIO_EXEC where id_usuario = :p_ID_USUARIO and tipo = :p_TIPO)  ");
             stb.AppendLine("   CONNECT BY NOCYCLE PRIOR ID_EXECUTIVO = ID_SUPERIOR ");
             stb.AppendLine("   ORDER BY NOME ");
          }

          _Command = (DbCommand)InicializaCommand(stb.ToString());

          if (ID_EMPRESA != "-1")
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", ID_EMPRESA));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_EMPRESA", DBNull.Value));
          if (!String.IsNullOrEmpty(ID_USUARIO))
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", ID_USUARIO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_ID_USUARIO", DBNull.Value));
          if (!String.IsNullOrEmpty(TIPO))
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", TIPO));
          else
             _Command.Parameters.Add(new OracleParameter(":p_TIPO", DBNull.Value));

          lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
          return lRetorno;
       }
       
        #endregion

        #region "Consultar"

        public DataSet ConsultarEXECUTIVO(Int32 ID_EXECUTIVO)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EXC.ID_EXECUTIVO, EXC.NOME, EXC.ID_ORGANOGRAMA, EXC.ID_SUPERIOR, EXC.DESCRICAO, EXC.STATUS, ");
            stb.AppendLine("    EXC.ANO, EXC.ID_EMPRESA, EXC.DATA_INICIO, EXC.DATA_FIM, EXC.ID_EXEC_ATUAL, EXC.NSEQ_SET1, EXC.NSEQ_SET, ");
            stb.AppendLine("    EMP.DESCRICAO AS EMP_DESCRICAO, EMP.SIGLA, ORG.NOME AS ORG_NOME, SUP.ID_EXECUTIVO AS ID_EXECUTIVO_SUP, SUP.NOME AS NOME_SUP, SUP.DESCRICAO AS DESCRICAO_SUP ");
            stb.AppendLine(" FROM EXECUTIVO EXC ");
            stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
            stb.AppendLine("    INNER JOIN ORGANOGRAMA ORG ON EXC.ID_ORGANOGRAMA = ORG.ID_ORGANOGRAMA ");
            stb.AppendLine("    LEFT OUTER JOIN EXECUTIVO SUP ON EXC.ID_SUPERIOR = SUP.ID_EXECUTIVO ");
            stb.AppendLine(" WHERE EXC.ID_EXECUTIVO = :p_ID_EXECUTIVO ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_ID_EXECUTIVO", ID_EXECUTIVO));

            lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
            return lRetorno;
        }

        public DataSet ConsultarEXECUTIVOPorNome(string NOME)
        {
            StringBuilder stb = new StringBuilder();
            DataSet lRetorno;
            DbCommand _Command;

            stb.AppendLine(" SELECT EXC.ID_EXECUTIVO, EXC.NOME, EXC.ID_ORGANOGRAMA, EXC.ID_SUPERIOR, EXC.DESCRICAO, EXC.STATUS, ");
            stb.AppendLine("    EXC.ANO, EXC.ID_EMPRESA, EXC.DATA_INICIO, EXC.DATA_FIM, EXC.ID_EXEC_ATUAL, EXC.NSEQ_SET1, EXC.NSEQ_SET, ");
            stb.AppendLine("    EMP.DESCRICAO AS EMP_DESCRICAO, EMP.SIGLA, ORG.NOME AS ORG_NOME, SUP.ID_EXECUTIVO AS ID_EXECUTIVO_SUP, SUP.NOME AS NOME_SUP, SUP.DESCRICAO AS DESCRICAO_SUP ");
            stb.AppendLine(" FROM EXECUTIVO EXC ");
            stb.AppendLine("    INNER JOIN EMPRESA EMP ON EXC.ID_EMPRESA = EMP.ID_EMPRESA ");
            stb.AppendLine("    INNER JOIN ORGANOGRAMA ORG ON EXC.ID_ORGANOGRAMA = ORG.ID_ORGANOGRAMA ");
            stb.AppendLine("    LEFT OUTER JOIN EXECUTIVO SUP ON EXC.ID_SUPERIOR = SUP.ID_EXECUTIVO ");
            stb.AppendLine(" WHERE upper(EXC.NOME) = :p_NOME ");

            _Command = (DbCommand)InicializaCommand(stb.ToString());

            _Command.Parameters.Add(new OracleParameter(":p_NOME", NOME.ToUpper()));

            lRetorno = ConsultaQueryDataSet(_Command, "EXECUTIVO");
            return lRetorno;
        }

        #endregion

        #endregion
    }
}
