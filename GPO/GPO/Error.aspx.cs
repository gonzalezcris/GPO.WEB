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

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GPO.FW.TrataErro objErro;
        if (!IsPostBack)
        {
            objErro = (GPO.FW.TrataErro)Session["Erro"];

            if (!(objErro == null))
            {
                lblLocalErro.Text = Request.Url.ToString();
                lblDescricaoErro.Text = objErro.Excecao.Message.ToString();
                lblStackTrace.Text = objErro.Excecao.StackTrace.ToString();
                if ((int)objErro.TipoErro == (int)GPO.FW.TrataErro.TipoExcecao.NaoTratado)
                    btnDetalhes.Visible = true;
                else
                    btnDetalhes.Visible = false;
            }

            ViewState["Retorno"] = "Principal.aspx";
            //Request.UrlReferrer.ToString()

            Session["Erro"] = null;
        }
    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["Retorno"].ToString());
    }

    protected void btnDetalhes_Click(object sender, EventArgs e)
    {
        lblTitStackTrace.Visible = !lblTitStackTrace.Visible;
        lblStackTrace.Visible = !lblStackTrace.Visible;
    }
}
