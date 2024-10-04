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

public partial class Principal : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {        
        //VerificaSessao();
        this.ltAppNome.Text = Application["app_Nome"].ToString();
    }

    #endregion

    #region " Sessão "

    private void VerificaSessao()
    {
        if (Session["_LOGIN"] == null)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx?mo=s", true);
        }
    }

    #endregion

}
