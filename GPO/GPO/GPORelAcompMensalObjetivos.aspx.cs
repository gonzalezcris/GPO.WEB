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

public partial class GPORelAcompMensalObjetivos : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();
        uppPrincipal.Controls.Add(Funcoes.AtribuiUpdateProgress("img/aguarde.gif"));
        //this.Page.SetFocus(NUMATRICULA);
    }

    #endregion

    #region " Bot�es "

    protected void btnImprimir_Click(object sender, System.EventArgs e)
    {
        //string msg = "";

        //if (!(ValidaForm(ref msg)))
        //{
        //    ScriptManager.RegisterClientScriptBlock(this.Page, btnImprimir.GetType(), "conf", "alert('" + msg + "');", true);
        //    return;
        //}

        Session["RAMO_ID_EMPRESA"] = ID_EMPRESA.SelectedValue;
        Session["RAMO_SG_EMPRESA"] = ID_EMPRESA.SelectedItem.Text;
        Session["RAMO_ID_EXECUTIVO"] = ID_EXECUTIVO.SelectedValue;
        Session["RAMO_DS_EXECUTIVO"] = ID_EXECUTIVO.SelectedItem.Text;
        Session["RAMO_MES"] = MES.SelectedValue;
        Session["RAMO_DS_MES"] = MES.SelectedItem.Text;
        Session["RAMO_ANO"] = ANO.SelectedValue;
        //Session["RFDO_"] = ANO.SelectedItem.Text;

        if (TIPO.SelectedValue.ToString() == "V")
            ScriptManager.RegisterClientScriptBlock(this.Page, btnImprimir.GetType(), "conf", "<script>window.open('GPORelAcompMensalObjetivosImp.aspx');</script>", false);
        else if (TIPO.SelectedValue.ToString() == "S")
            ScriptManager.RegisterClientScriptBlock(this.Page, btnImprimir.GetType(), "conf", "<script>window.open('GPORelAcompMensalObjetivosSimbolosImp.aspx');</script>", false);
    }

    #endregion

    #region " M�todos Gerais "

    private bool ValidaForm(ref string Msg)
    {
        Int32 i = 0;
        DateTime dt;

        //try
        //{
        //    if (NUMATRICULA.Text.Trim() != "")
        //        i = Convert.ToInt32(NUMATRICULA.Text.Trim());
        //}
        //catch (Exception ex)
        //{
        //    Msg = "N�mero de matr�cula inv�lido!";
        //    return false;
        //}

        //try
        //{
        //    if (REFINICIAL.Text.Trim() != "__/____")
        //        dt = Convert.ToDateTime("01/" + REFINICIAL.Text.Trim());


        //}
        //catch (Exception ex)
        //{
        //    Msg = "Refer�ncia inicial inv�lida!";
        //    return false;
        //}

        //try
        //{
        //    if (REFFINAL.Text.Trim() != "__/____")
        //        dt = Convert.ToDateTime("01/" + REFFINAL.Text.Trim());


        //}
        //catch (Exception ex)
        //{
        //    Msg = "Refer�ncia final inv�lida!";
        //    return false;
        //}


        return true;
    }

    #endregion

    #region " HandleError "

    protected void HandleError(object sender, AsyncPostBackErrorEventArgs e)
    {
        scmPrincipal.AsyncPostBackErrorMessage = GPO.FW.Util.TrataErro((System.Exception)e.Exception.GetBaseException(), Server, this.Page);
    }

    #endregion

    #region " Restri��o "

    private void VerificaRestricao()
    {
        if (Session["_MODULO"] == null)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx?mo=s", true);
        }
        // if (Convert.ToString(Session["_MODULO"]).IndexOf("|1701|") == -1)
        //  Response.Redirect("Default.aspx?mo=s", true);
    }

    #endregion
}
