using System;
using System.Collections.Generic;
using System.Text;

namespace GPO.ENTIDADES
{
    public class Perfil
    {
               
        #region PROPRIEDADES

        private Int32 ID_PERFIL;
        private String SIGLA;
        private String DESCRICAO;

        #endregion

        #region ACESSO ÀS PROPRIEDADES

        public Int32 Id_Perfil
        {
            get { return ID_PERFIL; }
            set { ID_PERFIL = value; }
        }

        public String Sigla
        {
            get { return SIGLA; }
            set { SIGLA = value; }
        }

        public String Descricao
        {
            get { return DESCRICAO; }
            set { DESCRICAO = value; }
        }

        #endregion

    }
}
