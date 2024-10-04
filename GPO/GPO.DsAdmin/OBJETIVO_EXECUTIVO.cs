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
    public class OBJETIVO_EXECUTIVO : GPO.FW.dsBase
    {

        #region "ObterEstrutura"
        public DataSet ObterEstruturaOBJETIVO_EXECUTIVO()
        {
            DataSet lDataSet;
            lDataSet = base.ObterEstrutura("OBJETIVO_EXECUTIVO");
            return lDataSet;
        }

        #endregion

    }
}
