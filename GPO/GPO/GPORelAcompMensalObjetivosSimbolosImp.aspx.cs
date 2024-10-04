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

public partial class GPORelAcompMensalObjetivosSimbolosImp : System.Web.UI.Page
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
        bool lPresidencia = false;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        crReportDocument.Load(Request.MapPath("RelAcompMensalObjetivosSimbolos.rpt"));

        using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
        {
            lDataView = objEXECUTIVO.ConsultarEXECUTIVO(Convert.ToInt32(Session["RAMO_ID_EXECUTIVO"].ToString())).Tables[0].DefaultView;
            
            if (lDataView.Table.Rows.Count > 0 && lDataView.Table.Rows[0]["ID_SUPERIOR"] == DBNull.Value )
                lPresidencia = true;
        }

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataSet = objRELATORIO.ListarRELATORIOAcompMensalObjetivosSimbolos(Session["RAMO_ID_EXECUTIVO"].ToString(), Session["RAMO_MES"].ToString(), Session["RAMO_ANO"].ToString(), lPresidencia);

            lDataView = lDataSet.Tables[0].DefaultView;

            if (lDataView.Table.Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }
        }

        crReportDocument.SetDataSource(MontaDadosRelatorio(lDataSet).DefaultView);

        crReportDocument.ParameterFields["sigla_1"].CurrentValues.AddValue(Session["RAMO_DS_EXECUTIVO"].ToString().Trim());

        using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
        {
            lDataView = objEXECUTIVO.ConsultarEXECUTIVO(Convert.ToInt32(Session["RAMO_ID_EXECUTIVO"].ToString())).Tables[0].DefaultView;

            Session["RAMO_DS_EXECUTIVO_COMPL"] = "";

            if (lDataView.Table.Rows.Count > 0)
                Session["RAMO_DS_EXECUTIVO_COMPL"] = lDataView.Table.Rows[0]["DESCRICAO"].ToString().Trim();
        }

        foreach (DataRow lRow in lDataSet.Tables[0].Rows)
        {
            if (Convert.ToInt32(lRow["POS"].ToString()) <= 16)
            {
                crReportDocument.ParameterFields["sigla_" + lRow["POS"].ToString()].CurrentValues.AddValue(lRow["EXECUTIVO_SIGLA"].ToString().Trim());
            }
        }

        for (int i = 1; i <= 16; i++)
        {
            crReportDocument.ParameterFields["sigla_" + Convert.ToString(i)].CurrentValues.AddValue("");
        }

        if (Session["RAMO_SG_EMPRESA"].ToString().Trim() == "COELBA")
            Session["RAMO_ID_EMPRESA"] = 1;
        else if (Session["RAMO_SG_EMPRESA"].ToString().Trim() == "COSERN")
            Session["RAMO_ID_EMPRESA"] = 2;
        else if (Session["RAMO_SG_EMPRESA"].ToString().Trim() == "CELPE")
            Session["RAMO_ID_EMPRESA"] = 3;

        crReportDocument.ParameterFields["mes"].CurrentValues.AddValue(Session["RAMO_DS_MES"].ToString() + " / " + Session["RAMO_ANO"].ToString());
        crReportDocument.ParameterFields["orgao"].CurrentValues.AddValue(Session["RAMO_DS_EXECUTIVO_COMPL"].ToString() + " - " + Session["RAMO_DS_EXECUTIVO"].ToString());
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["RAMO_ID_EMPRESA"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "amos-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
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

    protected DataTable MontaDadosRelatorio(DataSet pDADOS)
    {
        //Session["DADOS"] = (DataView)odsGrvPrincipal.Select();

        DataSet lDADOS;
        DataTable DT = new DataTable();

        lDADOS = pDADOS;

        DT = CriaDataTable();

        string atuacao_atual = "";
        string sigla_atual = "";

        foreach (DataRow lRow in lDADOS.Tables[0].Rows)
        {
            if (lRow["ATUACAO"].ToString() != atuacao_atual ||
                lRow["SIGLA"].ToString() != sigla_atual)
            {
                DT.Rows.Add(DT.NewRow());
                DT.Rows[DT.Rows.Count - 1]["ATUACAO"] = lRow["ATUACAO"];
                DT.Rows[DT.Rows.Count - 1]["INDICADOR"] = lRow["INDICADOR"];
                DT.Rows[DT.Rows.Count - 1]["SIGLA"] = lRow["SIGLA"];
                DT.Rows[DT.Rows.Count - 1]["PERIODICIDADE"] = lRow["PERIODICIDADE"];
                DT.Rows[DT.Rows.Count - 1]["UNIDADE"] = lRow["UNIDADE"];

                if (String.IsNullOrEmpty(lRow["VALOR_ESP"].ToString()))
                {
                    //*** NÃO ESPERADO
                    DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -1;
                }
                else
                {
                    if (String.IsNullOrEmpty(lRow["VALOR"].ToString()))
                    {
                        if (Convert.ToInt32(lRow["QTD_COMPONENTES"].ToString()) != Convert.ToInt32(lRow["QTD_ENTRADAS"].ToString()))
                            DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -2;//*** NÃO DISPONÍVEL
                        else
                            DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -3;//*** INDETERMINADO
                    }
                    else
                    {
                        DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = lRow["ID_CONCEITO"];
                    }
                }

                atuacao_atual = lRow["ATUACAO"].ToString();
                sigla_atual = lRow["SIGLA"].ToString();
            }
            else
            {
                if (String.IsNullOrEmpty(lRow["VALOR_ESP"].ToString()))
                {
                    //*** NÃO ESPERADO
                    DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -1;
                }
                else
                {
                    if (String.IsNullOrEmpty(lRow["VALOR"].ToString()))
                    {
                        if (Convert.ToInt32(lRow["QTD_COMPONENTES"].ToString()) != Convert.ToInt32(lRow["QTD_ENTRADAS"].ToString()))
                            DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -2;//*** NÃO DISPONÍVEL
                        else
                            DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = -3;//*** INDETERMINADO
                    }
                    else
                    {
                        DT.Rows[DT.Rows.Count - 1]["VALOR_" + lRow["POS"].ToString()] = lRow["ID_CONCEITO"];
                    }
                }
            }
        }

        return DT;

    }

    private DataTable CriaDataTable()
    {
        DataTable mDataTable = new DataTable();
        DataColumn mDataColumn;

        mDataColumn = new DataColumn("ATUACAO", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("INDICADOR", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("SIGLA", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("PERIODICIDADE", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("UNIDADE", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);

        mDataColumn = new DataColumn("VALOR_1", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_2", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_3", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_4", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_5", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_6", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_7", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_8", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_9", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_10", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_11", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_12", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_13", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_14", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_15", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_16", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);

        return mDataTable;
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
