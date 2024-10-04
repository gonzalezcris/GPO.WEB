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

using System.IO;

public partial class GPORelResumoObjetivosImp : System.Web.UI.Page
{
    private ReportDocument crReportDocument = new ReportDocument();
    private ReportDocument crReportSubApr = new ReportDocument();

    #region " UnLoad "

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        crReportDocument.Close();
        crReportDocument.Dispose();
        crReportSubApr.Close();
        crReportSubApr.Dispose();
        GC.Collect();
    }

    #endregion

    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();

        this.Unload += new System.EventHandler(this.Page_UnLoad);

        //ReportDocument crReportDocument = new ReportDocument();
        //ReportDocument crReportSubApr = new ReportDocument();

        DataView lDataView = null;
        DataView lDataViewApr = null;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        //*** Legado
        if (Convert.ToInt32(Session["RRO_ANO"].ToString()) <= 2013)
            crReportDocument.Load(Request.MapPath("RELResumoObjetivosLegado.rpt"));
        else
        {
            crReportDocument.Load(Request.MapPath("RELResumoObjetivos.rpt"));
            crReportSubApr = crReportDocument.OpenSubreport("RELAprovadores.rpt");
        }

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataView = objRELATORIO.ListarRELATORIOResumoObjetivos(Session["RRO_ID_EXECUTIVO"].ToString(), Session["RRO_ANO"].ToString()).Tables[0].DefaultView;

            if (lDataView.Table.Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }

            if (Convert.ToInt32(Session["RRO_ANO"].ToString()) > 2013)
                lDataViewApr = objRELATORIO.ListarRELATORIOAprovadores(Session["RRO_ID_EXECUTIVO"].ToString(), Session["RRO_ANO"].ToString()).Tables[0].DefaultView;
        }

        crReportDocument.SetDataSource(lDataView.Table);
        if (Convert.ToInt32(Session["RRO_ANO"].ToString()) > 2013)
        crReportSubApr.SetDataSource(lDataViewApr.Table);
        //crReportDocument.Subreports[0].SetDataSource(lDataViewPrj.Table);

        using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
        {
            lDataView = objEXECUTIVO.ConsultarEXECUTIVO(Convert.ToInt32(Session["RRO_ID_EXECUTIVO"].ToString())).Tables[0].DefaultView;

            Session["RRO_DS_EXECUTIVO"] = Session["RRO_DS_EXECUTIVO"].ToString().Trim() + " " + lDataView.Table.Rows[0]["DESCRICAO"].ToString().Trim();
        }

        if (lDataView.Table.Rows[0]["ORG_NOME"] != DBNull.Value)
        {
            switch (lDataView.Table.Rows[0]["ORG_NOME"].ToString().Trim().ToUpper())
            {
                case "PRESIDÊNCIA":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Presidente");
                    break;
                case "DIRETORIA":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Diretor");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Presidente");
                    break;
                case "DEPARTAMENTO":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Gerente");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Diretor");
                    break;
                case "UNIDADE":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Gestor");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Gerente");
                    break;
                case "EXEC. ESPECIALIZADO":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Exec. Especializado");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Gerente");
                    break;
                case "DEPARTAMENTO/PR":
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Gerente");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Presidente");
                    break;
                default:
                    crReportDocument.ParameterFields["assinatura"].CurrentValues.AddValue("Assinatura do Executivo");
                    crReportDocument.ParameterFields["assinatura2"].CurrentValues.AddValue("Assinatura do Superior");
                    break;
            }
        }

        if (Session["RRO_SG_EMPRESA"].ToString().Trim() == "COELBA")
            Session["RRO_ID_EMPRESA"] = 1;
        else if (Session["RRO_SG_EMPRESA"].ToString().Trim() == "COSERN")
            Session["RRO_ID_EMPRESA"] = 2;
        else if (Session["RRO_SG_EMPRESA"].ToString().Trim() == "CELPE")
            Session["RRO_ID_EMPRESA"] = 3;

        crReportDocument.ParameterFields["ano"].CurrentValues.AddValue(Session["RRO_ANO"].ToString());
        crReportDocument.ParameterFields["orgao"].CurrentValues.AddValue(" " + Session["RRO_DS_EXECUTIVO"].ToString());
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["RRO_ID_EMPRESA"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "ro-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
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

    #region " Métodos Gerais "


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
