using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using GPO.FW;
using GPO.DsAdmin;

public partial class GPOAprovacaoExeMotivoRecusa : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();
        if (!Page.IsPostBack)
        {
            DataSet lDataSet;
            using (OBJETIVO_APROVACAO objOBJETIVO_APROVACAO = new OBJETIVO_APROVACAO())
            {
                lDataSet = objOBJETIVO_APROVACAO.ConsultarOBJETIVO_APROVACAO(Convert.ToString(Request["ID_APROVACAO"]));
                //GPO.FW.Util.ManterCamposViaDataView(this.Page, lDataSet.Tables[0].DefaultView, Convert.ToInt16(ConfigurationManager.AppSettings["OrdemControleUtil"]));
                if (lDataSet.Tables[0].Rows.Count > 0)
                {
                    if (lDataSet.Tables[0].Rows[0]["MOTIVO_RECUSA"] != DBNull.Value)
                        MOTIVO_RECUSA.Text = GPO.FW.Util.FormatarTexto(lDataSet.Tables[0].Rows[0]["MOTIVO_RECUSA"].ToString());
                }
            }
        }
    }

    #endregion

    #region " Restrição "

    private void VerificaRestricao()
    {
        if (Session["_MODULO"] == null)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx?mo=sC", true);
        }
        // if (Convert.ToString(Session["_MODULO"]).IndexOf("|1701|") == -1)
        //  Response.Redirect("Default.aspx?mo=s", true);
    }

    #endregion
}
