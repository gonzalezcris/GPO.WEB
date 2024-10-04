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
   public class TRANSACAO : GPO.FW.dsBase
   {
      #region "Consultar"

      public DataSet ConsultarTransacaoPorUsuario(string ID_USUARIO, string TIPO)
      {
         StringBuilder stb = new StringBuilder();
         DataSet lRetorno;
         DbCommand _Command;

         stb.AppendLine(" SELECT UE.ID_USUARIO, UE.TIPO, T.ID_TRANSACAO, T.DESCRICAO ");
         stb.AppendLine(" FROM USUARIO_EXEC UE, TRANSACAO T                          ");
         stb.AppendLine(" WHERE UE.TIPO = T.ID_TRANSACAO                             ");
         stb.AppendLine("   AND UE.ID_USUARIO = :P_ID_USUARIO                        ");
         stb.AppendLine("   AND T.ID_TRANSACAO = :P_TIPO                             ");

         _Command = (DbCommand)InicializaCommand(stb.ToString());

         _Command.Parameters.Add(new OracleParameter(":P_ID_USUARIO", ID_USUARIO));
         _Command.Parameters.Add(new OracleParameter(":P_TIPO", TIPO));

         lRetorno = ConsultaQueryDataSet(_Command, "TRANSACAO");
         return lRetorno;
      }

      #endregion
   }
}
