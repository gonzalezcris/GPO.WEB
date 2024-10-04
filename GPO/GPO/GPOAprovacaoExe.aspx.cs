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

public partial class GPOAprovacaoExe : System.Web.UI.Page
{
    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {           
        VerificaRestricao();
        uppPrincipal.Controls.Add(Funcoes.AtribuiUpdateProgress("img/aguarde.gif"));
        //this.Page.SetFocus(CAMPO);
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
            e.Row.Cells[7].Text = "";
            if (((DataRowView)e.Row.DataItem).Row["RECUSADO_SUP"] != DBNull.Value)
            {
                if (((DataRowView)e.Row.DataItem).Row["RECUSADO_SUP"].ToString() == "N")
                    e.Row.Cells[7].Text = "<img src='img/bot16_ok.png' alt='Aprovado pelo superior' />";
                if (((DataRowView)e.Row.DataItem).Row["RECUSADO_SUP"].ToString() == "S")
                    e.Row.Cells[7].Text = "<a href=\"#\" onclick=\"modalWin('GPOAprovacaoExeMotivoRecusa.aspx?ID_APROVACAO=" + ((DataRowView)e.Row.DataItem).Row["ID_APROVACAO"].ToString() + "')\"><img src='img/bot16_no.png' alt='Reprovado pelo superior' border='0' /></a>";
            }

            if (((DataRowView)e.Row.DataItem).Row["DATA_APROVACAO"] != DBNull.Value)
            {
                btnAprovar.Enabled = false;
            }

            if (((DataRowView)e.Row.DataItem).Row["ID_USUARIO_APRV"] != DBNull.Value)
            {
               e.Row.Cells[0].Text = "";
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = ViewState["TOTAL"].ToString();
        }
    }

    #endregion

    #region " Botões "

    protected void btnAprovar_Click(object sender, System.EventArgs e)
    {
        CheckBox chk;

        using (OBJETIVO_APROVACAO objOBJETIVO_APROVACAO = new OBJETIVO_APROVACAO())
        {
            DataSet lDataSet = objOBJETIVO_APROVACAO.ObterEstruturaOBJETIVO_APROVACAO();
            lDataSet.Tables[0].Rows.RemoveAt(0);

            DataSet lDataSetSup = new DataSet();
            if ((bool)ViewState["PR"])
            {
                lDataSetSup = objOBJETIVO_APROVACAO.ObterEstruturaOBJETIVO_APROVACAO();
                lDataSetSup.Tables[0].Rows.RemoveAt(0);
            }

            foreach (GridViewRow lItem in grvPrincipal.Rows)
            {
               if (lItem.RowType == DataControlRowType.DataRow)
               {
                  //chk = (CheckBox)lItem.FindControl("chkSel");
                  //if (chk != null && chk.Checked)
                  //{
                     lDataSet.Tables[0].Rows.Add(lDataSet.Tables[0].NewRow());
                     lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_OBJETIVO"] = grvPrincipal.DataKeys[lItem.RowIndex].Value;
                     lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_USUARIO_APRV"] = Session["_ID_USUARIO"].ToString();

                     if ((bool)ViewState["PR"])
                     {
                        lDataSetSup.Tables[0].Rows.Add(lDataSetSup.Tables[0].NewRow());
                        lDataSetSup.Tables[0].Rows[lDataSetSup.Tables[0].Rows.Count - 1]["ID_OBJETIVO"] = grvPrincipal.DataKeys[lItem.RowIndex].Value;
                        lDataSetSup.Tables[0].Rows[lDataSetSup.Tables[0].Rows.Count - 1]["ID_USUARIO_APRV_SUP"] = Session["_ID_USUARIO"].ToString();
                        lDataSetSup.Tables[0].Rows[lDataSetSup.Tables[0].Rows.Count - 1]["RECUSADO_SUP"] = 'N';
                     }
                  //}
               }
            }
            
            if (lDataSet.Tables[0].Rows.Count > 0)
            {
                objOBJETIVO_APROVACAO.IncluirOBJETIVO_APROVACAOExecutivo(lDataSet);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "<script>alert('Aprovação realizada com sucesso!');</script>", false);

                if ((bool)ViewState["PR"])
                {
                    if (lDataSetSup.Tables[0].Rows.Count > 0)
                    {
                        objOBJETIVO_APROVACAO.AlterarOBJETIVO_APROVACAOSuperior(lDataSetSup);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "conf", "alert('Nenhum item foi selecionado!');", true);
            }
        }
        grvPrincipal.DataBind();
    }

    protected void btnAprovarTodos_Click(object sender, System.EventArgs e)
    {
        using (OBJETIVO_APROVACAO objOBJETIVO_APROVACAO = new OBJETIVO_APROVACAO())
        {
            objOBJETIVO_APROVACAO.IncluirOBJETIVO_APROVACAOExecutivoTodos(Session["_ID_USUARIO"].ToString(), ID_EXECUTIVO.SelectedValue.ToString(), ANO.SelectedValue.ToString());
        }
        grvPrincipal.DataBind();
        ScriptManager.RegisterClientScriptBlock(this.Page, btnAprovar.GetType(), "conf", "<script>alert('Aprovação realizada com sucesso!');</script>", false);
    }

    #endregion

    #region " ODS "

    protected void odsGrvPrincipal_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        ViewState["PR"] = false;
        if (ID_EXECUTIVO.SelectedItem.Text.Trim().ToUpper() == "PR")
            ViewState["PR"] = true;           
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
