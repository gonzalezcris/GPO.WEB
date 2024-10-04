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
using GPO.DsAdmin;

public partial class GPOAprovacaoCancel : System.Web.UI.Page
{
   #region "Load"
   
   protected void Page_Load(object sender, EventArgs e)
   {
      
   }
   
   #endregion

   #region "Button"

   protected void btnCancel_click(object sender, System.EventArgs e)
   {
      using (OBJETIVO_APROVACAO objAprov = new OBJETIVO_APROVACAO())
      {
         DataSet lDataSet;
         CheckBox chk;
         
         lDataSet = objAprov.ObterEstruturaOBJETIVO_APROVACAO();
         
         foreach (GridViewRow lItem in grvPrincipal.Rows)
         {    
            if (lItem.RowType == DataControlRowType.DataRow)
            {
               chk = (CheckBox)lItem.FindControl("chkSel");
               if (chk.Checked )
               {
                  lDataSet.Tables[0].Rows.Add(lDataSet.Tables[0].NewRow());
                  lDataSet.Tables[0].Rows[lDataSet.Tables[0].Rows.Count - 1]["ID_APROVACAO"] = grvPrincipal.DataKeys[lItem.RowIndex].Value;               
               }
            }
         }
         
         objAprov.CancelarOBJETIVO_APROVACAO(lDataSet);
         ScriptManager.RegisterClientScriptBlock(this.Page, btnCancel.GetType(), "conf", "alert('Cancelamento realizado com sucesso!');", true);

         grvPrincipal.DataBind();
      }          
      
   }
   
   #endregion
      
   #region "Grid"

   protected void grvPrincipal_RowDataBound(object sender, GridViewRowEventArgs e)
   {

      if (e.Row.RowType == DataControlRowType.DataRow)
      {
         e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFECB'");
         e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='transparent'");

         btnCancel.Enabled = true;
      }
   }

   #endregion

   #region " HandleError "

   protected void HandleError(object sender, AsyncPostBackErrorEventArgs e)
   {
      scmPrincipal.AsyncPostBackErrorMessage = GPO.FW.Util.TrataErro((System.Exception)e.Exception.GetBaseException(), Server, this.Page);
   }

   #endregion
}
