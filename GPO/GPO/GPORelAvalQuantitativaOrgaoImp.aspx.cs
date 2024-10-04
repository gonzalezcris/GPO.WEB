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

public partial class GPORelAvalQuantitativaOrgaoImp : System.Web.UI.Page
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

        DataSet lDataSet = null;
        DataView lDataView = null;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        crReportDocument.Load(Request.MapPath("RELAvalQuantitativaOrgao.rpt"));

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataSet = objRELATORIO.ListarRELATORIOAvaliacaoQuantitativaOrgao(Session["RAQO_ID_EXECUTIVO"].ToString(), Session["RAQO_MES"].ToString(), Session["RAQO_ANO"].ToString());

            if (lDataSet.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }
            else
            {
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    if (String.IsNullOrEmpty(lRow["VALOR_ESPERADO"].ToString()))
                    {
                        lRow["ID_CONCEITO"] = -1;//*** NÃO ESPERADO
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(lRow["VALOR_REALIZADO"].ToString()))
                        {
                            if (Convert.ToInt32(lRow["QTD_COMPONENTES"].ToString()) != Convert.ToInt32(lRow["QTD_ENTRADAS"].ToString()))
                                lRow["ID_CONCEITO"] = -2;//*** NÃO DISPONÍVEL
                            else
                                lRow["ID_CONCEITO"] = -3;//*** INDETERMINADO
                        }
                    }
                }
            }

            lDataView = lDataSet.Tables[0].DefaultView;
        }

        crReportDocument.SetDataSource(lDataView);

        using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
        {
            lDataView = objEXECUTIVO.ConsultarEXECUTIVO(Convert.ToInt32(Session["RAQO_ID_EXECUTIVO"].ToString())).Tables[0].DefaultView;

            Session["RAQO_DS_EXECUTIVO"] = lDataView.Table.Rows[0]["DESCRICAO"].ToString().Trim() + " - " + Session["RAQO_DS_EXECUTIVO"].ToString().Trim();

            if (lDataView.Table.Rows[0]["ID_EXECUTIVO_SUP"] != DBNull.Value)
                Session["RAQO_DS_EXECUTIVO_SUP"] = lDataView.Table.Rows[0]["DESCRICAO_SUP"].ToString().Trim() + " - " + lDataView.Table.Rows[0]["NOME_SUP"].ToString().Trim();
            else
                Session["RAQO_DS_EXECUTIVO_SUP"] = "";
        }

        if (Session["RAQO_SG_EMPRESA"].ToString().Trim() == "COELBA")
            Session["RAQO_ID_EMPRESA"] = 1;
        else if (Session["RAQO_SG_EMPRESA"].ToString().Trim() == "COSERN")
            Session["RAQO_ID_EMPRESA"] = 2;
        else if (Session["RAQO_SG_EMPRESA"].ToString().Trim() == "CELPE")
            Session["RAQO_ID_EMPRESA"] = 3;

        crReportDocument.ParameterFields["mes"].CurrentValues.AddValue(Session["RAQO_DS_MES"].ToString() + " / " + Session["RAQO_ANO"].ToString());
        crReportDocument.ParameterFields["orgao"].CurrentValues.AddValue(Session["RAQO_DS_EXECUTIVO"].ToString());
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["RAQO_ID_EMPRESA"].ToString());
        crReportDocument.ParameterFields["orgaosuperior"].CurrentValues.AddValue(Session["RAQO_DS_EXECUTIVO_SUP"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "aqo-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
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
