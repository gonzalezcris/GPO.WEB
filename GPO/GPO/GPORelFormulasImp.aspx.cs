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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class GPORelFormulasImp : System.Web.UI.Page
{
    private ReportDocument crReportDocument = new ReportDocument();

    #region " UnLoad "

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        crReportDocument.Close();
        crReportDocument.Dispose();
        GC.Collect();
    }

    #endregion

    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();

        this.Unload += new System.EventHandler(this.Page_UnLoad);

        //ReportDocument crReportDocument = new ReportDocument();

        DataView lDataView = null;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        crReportDocument.Load(Request.MapPath("RELFormulas.rpt"));

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataView = objRELATORIO.ListarRELATORIOFormulas(Session["RF_NOME"].ToString()).Tables[0].DefaultView;

            if (lDataView.Table.Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }
        }

        crReportDocument.SetDataSource(lDataView.Table);
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["_ID_EMPRESA"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "f-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
        string lPath = Server.MapPath(".") + "\\exportacao\\relatorios\\";

        Utilitaria.LimparArquivos(lPath);

        if (Utilitaria.ExportCrystalToPDF(crReportDocument, lPath + lFile))
        {
            Response.Redirect("exportacao/relatorios/" + lFile);
        }
        else
        {
            Response.Write("Erro ao gerar relatório!");
        }
    }

    #endregion

    #region " Restrição "

    private void VerificaRestricao()
    {
        if (Session["_MODULO"] == null)
        {
            if (!Utilitaria.VerificaCookie())
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("Default.aspx?mo=s", true);
            }
        }
        //if (Convert.ToString(Session["_MODULO"]).IndexOf("||") == -1)
        //{
        //    System.Web.Security.FormsAuthentication.SignOut();
        //    Response.Redirect("Default.aspx?mo=s", true);
        //}
    }

    #endregion
}
