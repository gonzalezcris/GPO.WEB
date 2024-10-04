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

public partial class GPOAprovacaoSup : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();
        uppPrincipal.Controls.Add(Funcoes.AtribuiUpdateProgress("img/aguarde.gif"));
        //this.Page.SetFocus(CAMPO);
        ViewState["APROVA"] = "N";
    }

    #endregion

    #region " Grid "

    protected void grvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ViewState["TOTAL"] = 0;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFECB'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='transparent'");

            ViewState["TOTAL"] = Convert.ToDouble(ViewState["TOTAL"].ToString()) + Convert.ToDouble(((DataRowView)e.Row.DataItem).Row["PONTUACAO"].ToString());

            btnAprovar.Enabled = false;
            if (Convert.ToDouble(ViewState["TOTAL"].ToString()) == Convert.ToDouble(1000))
            {
                btnAprovar.Enabled = true;
            }
            if (((DataRowView)e.Row.DataItem).Row["DATA_APROVACAO"] == DBNull.Value)
            {
               ViewState["APROVA"] = "N";               
            }
            else
            {
               if ((((DataRowView)e.Row.DataItem).Row["RECUSADO_SUP"].ToString() == "S") || (((DataRowView)e.Row.DataItem).Row["RECUSADO_SUP"].ToString() == ""))
               {
                  ViewState["APROVA"] = "S";
               }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = ViewState["TOTAL"].ToString();
        }

        if (ViewState["APROVA"].ToString() == "S") 
           btnAprovar.Enabled = true;
        else
           btnAprovar.Enabled = false;
    }

    #endregion

    #region " Botões "

    protected void btnAprovar_Click(object sender, System.EventArgs e)
    {
        CheckBox chk;
        bool flgRecusa = false;

        using (OBJETIVO_APROVACAO objOBJETIVO_APROVACAO = new OBJETIVO_APROVACAO())
        {
            DataSet lDataSet = objOBJETIVO_APROVACAO.ObterEstruturaOBJETIVO_APROVACAO();
            lDataSet.Tables[0].Rows.RemoveAt(0);
            foreach (GridViewRow lItem in grvPrincipal.Rows)
            {
                if (lItem.RowType == DataControlRowType.DataRow)
                {
                    chk = (CheckBox)lItem.FindControl("chkSel");
                    if (chk != null)
                    {
                        lDataSet.Tables[0].Rows.Add(lDataSet.Tables[0].NewRow());
                        lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_OBJETIVO"] = grvPrincipal.DataKeys[lItem.RowIndex].Value;
                        lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_USUARIO_APRV_SUP"] = Session["_ID_USUARIO"].ToString();
                        if (chk.Checked)
                        {
                            lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["RECUSADO_SUP"] = 'N';
                        }
                        else
                        {
                            lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["RECUSADO_SUP"] = 'S';
                            flgRecusa = true;
                        }
                    }
                }
            }
            if (lDataSet.Tables[0].Rows.Count > 0)
            {                
                objOBJETIVO_APROVACAO.AlterarOBJETIVO_APROVACAOSuperior(lDataSet);
                if (flgRecusa)
                {
                    //Response.Redirect("GPOAprovacaoSupMotivoRecusa.aspx");
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "<script>modalWin('GPOAprovacaoSupMotivoRecusa.aspx?id_executivo=" + ID_EXECUTIVO.SelectedValue.ToString() + "&ano=" + ANO.SelectedValue.ToString() + "');</script>", false);
                    
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "<script>alert('Aprovação realizada com sucesso!');</script>", false);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "alert('Nenhum item foi selecionado!');", true);
            }
        }
        ID_EXECUTIVO.DataBind();
        grvPrincipal.DataBind();
    }

    #endregion

    #region " HandleError "

    protected void HandleError(object sender, AsyncPostBackErrorEventArgs e)
    {
        scmPrincipal.AsyncPostBackErrorMessage = GPO.FW.Util.TrataErro((System.Exception)e.Exception.GetBaseException(), Server, this.Page);
    }

    #endregion

    #region " Restrição "

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
