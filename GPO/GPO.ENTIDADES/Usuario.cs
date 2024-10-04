using System;
using System.Collections.Generic;
using System.Text;

namespace GPO.ENTIDADES
{
    public class Usuario
    {
        #region PROPRIEDADES

        private Int32 ID_USUARIO;
        private String NICK;
        private String SENHA;
        private Int32 ID_EMPRESA;
        private String SG_EMPRESA;
        private String STATUS;
        private Nullable<DateTime> DT_ACESSO;

        #endregion

        #region ACESSO ÀS PROPRIEDADES

        public Int32 Id_Usuario
        {
            get { return ID_USUARIO; }
            set { ID_USUARIO = value; }
        }
     
        public String Nick
        {
            get { return NICK; }
            set { NICK = value; }
        }        

        public String Senha
        {
            get { return SENHA; }
            set { SENHA = value; }
        }
        
        public Int32 Id_Empresa
        {
            get { return ID_EMPRESA; }
            set { ID_EMPRESA = value; }
        }

        public String Sg_Empresa
        {
            get { return SG_EMPRESA; }
            set { SG_EMPRESA = value; }
        }
       
        public String Status
        {
            get { return STATUS; }
            set { STATUS = value; }
        }
      
        public Nullable<DateTime> DataAcesso
        {
            get { return DT_ACESSO; }
            set { DT_ACESSO = value; }
        }

        #endregion
    }
}
