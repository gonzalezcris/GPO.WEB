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

public partial class GPOAprovacaoSupMotivoRecusa : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();
        //uppPrincipal.Controls.Add(Funcoes.AtribuiUpdateProgress("img/aguarde.gif"));
        //this.Page.SetFocus(CAMPO);
    }

    #endregion

    #region " Grid "

    protected void grvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lLabel;
            lLabel = ((Label)e.Row.FindControl("lblObjetivo"));
            lLabel.Text = ((DataRowView)e.Row.DataItem).Row["NOME"].ToString();
        }
    }

    #endregion

    #region " Botões "

    protected void btnConfirmar_Click(object sender, System.EventArgs e)
    {
        TextBox lTextBox;
        bool flgRecusa = false;

        using (OBJETIVO_APROVACAO objOBJETIVO_APROVACAO = new OBJETIVO_APROVACAO())
        {
            DataSet lDataSet = objOBJETIVO_APROVACAO.ObterEstruturaOBJETIVO_APROVACAO();
            lDataSet.Tables[0].Rows.RemoveAt(0);
            foreach (GridViewRow lItem in grvPrincipal.Rows)
            {
                if (lItem.RowType == DataControlRowType.DataRow)
                {
                    lTextBox = (TextBox)lItem.FindControl("MOTIVO_RECUSA");
                    if (lTextBox != null && lTextBox.Text.Trim() != "")
                    {
                        lDataSet.Tables[0].Rows.Add(lDataSet.Tables[0].NewRow());
                        lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_OBJETIVO"] = grvPrincipal.DataKeys[lItem.RowIndex].Value;
                        lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["MOTIVO_RECUSA"] = lTextBox.Text.Trim();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "<script>alert('Todos os motivos são obrigatórios!');</script>", false);
                        return;
                    }
                }
            }
            if (lDataSet.Tables[0].Rows.Count > 0)
            {
                objOBJETIVO_APROVACAO.AlterarOBJETIVO_APROVACAOMotivoRecusa(lDataSet);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "<script>alert('Motivos cadastrados com sucesso!');window.close();</script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "alert('Nenhum item foi selecionado!');", true);
            }
        }
        grvPrincipal.DataBind();
    }

    #endregion

    #region " Restrição "

    private void VerificaRestricao()
    {
        if (Session["_MODULO"] == null)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx?mo=sc", true);
        }
        // if (Convert.ToString(Session["_MODULO"]).IndexOf("|1701|") == -1)
        //  Response.Redirect("Default.aspx?mo=s", true);
    }

    #endregion
}
