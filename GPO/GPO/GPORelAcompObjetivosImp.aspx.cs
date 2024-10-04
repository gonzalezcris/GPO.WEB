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

public partial class GPORelAcompObjetivosImp : System.Web.UI.Page
{
    private ReportDocument crReportDocument = new ReportDocument();
    private ReportDocument crReportSubCom = new ReportDocument();
    private ReportDocument crReportSubGra = new ReportDocument();
    private ReportDocument crReportSubGraAnt = new ReportDocument();

    #region " UnLoad "

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        crReportDocument.Close();
        crReportDocument.Dispose();

        crReportSubCom.Close();
        crReportSubCom.Dispose();

        crReportSubGra.Close();
        crReportSubGra.Dispose();

        crReportSubGraAnt.Close();
        crReportSubGraAnt.Dispose();
        
        GC.Collect();
    }

    #endregion

    #region " Load "

    protected void Page_Load(object sender, EventArgs e)
    {
        VerificaRestricao();

        this.Unload += new System.EventHandler(this.Page_UnLoad);

        //ReportDocument crReportDocument = new ReportDocument();
        //ReportDocument crReportSubCom = new ReportDocument();
        //ReportDocument crReportSubGra = new ReportDocument();
        //ReportDocument crReportSubGraAnt = new ReportDocument();

        DataSet lDataSet = null;
        DataView lDataView = null;
        DataView lDataViewCom = null;
        DataView lDataViewGra = null;
        DataView lDataViewGraAnt = null;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        crReportDocument.Load(Request.MapPath("RELAcompObjetivos.rpt"));
        crReportSubCom = crReportDocument.OpenSubreport("RELAcompObjetivosComentarios.rpt");
        crReportSubGra = crReportDocument.OpenSubreport("RELAcompObjetivosGrafico.rpt");
        crReportSubGraAnt = crReportDocument.OpenSubreport("RELAcompObjetivosGraficoAnt.rpt");

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataSet = objRELATORIO.ListarRELATORIOAcompanhamentoObjetivo(Session["RAO_ID_EXECUTIVO"].ToString(), Session["RAO_ANO"].ToString(), Session["RAO_ID_OBJETIVO"].ToString());

            if (lDataSet.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }
            else
            {
                foreach (DataRow lRow in lDataSet.Tables[0].Rows)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        if (String.IsNullOrEmpty(lRow["VALOR_ESPERADO" + Convert.ToString(i)].ToString()))
                        {
                            lRow["ID_CONCEITO" + Convert.ToString(i)] = -1;//*** NÃO ESPERADO
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(lRow["VALOR_REALIZADO" + Convert.ToString(i)].ToString()))
                            {
                                if (Convert.ToInt32(lRow["QTD_COMPONENTES"].ToString()) != Convert.ToInt32(lRow["QTD_ENTRADAS" + Convert.ToString(i)].ToString()))
                                    lRow["ID_CONCEITO" + Convert.ToString(i)] = -2;//*** NÃO DISPONÍVEL
                                else
                                    lRow["ID_CONCEITO" + Convert.ToString(i)] = -3;//*** INDETERMINADO
                            }
                        }
                    }
                }
            }

            lDataView = lDataSet.Tables[0].DefaultView;

            lDataViewGra = MontaDadosRelatorio(lDataSet).DefaultView;
            lDataViewGraAnt = ((DataSet)Session["lDADOSAnt"]).Tables[0].DefaultView;

            lDataViewCom = objRELATORIO.ListarRELATORIOAcompanhamentoObjetivoComentarios(Session["RAO_ID_EXECUTIVO"].ToString(), Session["RAO_ANO"].ToString(), Session["RAO_ID_OBJETIVO"].ToString()).Tables[0].DefaultView;
        }

        crReportSubGraAnt.SetDataSource(lDataViewGraAnt.Table);
        crReportSubGra.SetDataSource(lDataViewGra.Table);
        crReportSubCom.SetDataSource(lDataViewCom.Table);
        crReportDocument.SetDataSource(lDataView.Table);
        //crReportDocument.Subreports[0].SetDataSource(lDataViewPrj.Table);

        using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
        {
            lDataView = objEXECUTIVO.ConsultarEXECUTIVO(Convert.ToInt32(Session["RAO_ID_EXECUTIVO"].ToString())).Tables[0].DefaultView;

            Session["RAO_DS_EXECUTIVO"] = Session["RAO_DS_EXECUTIVO"].ToString().Trim() + " " + lDataView.Table.Rows[0]["DESCRICAO"].ToString().Trim();
        }

        if (Session["RAO_SG_EMPRESA"].ToString().Trim() == "COELBA")
            Session["RAO_ID_EMPRESA"] = 1;
        else if (Session["RAO_SG_EMPRESA"].ToString().Trim() == "COSERN")
            Session["RAO_ID_EMPRESA"] = 2;
        else if (Session["RAO_SG_EMPRESA"].ToString().Trim() == "CELPE")
            Session["RAO_ID_EMPRESA"] = 3;

        crReportDocument.ParameterFields["ano"].CurrentValues.AddValue(Session["RAO_ANO"].ToString());
        crReportDocument.ParameterFields["orgao"].CurrentValues.AddValue(Session["RAO_DS_EXECUTIVO"].ToString());
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["RAO_ID_EMPRESA"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "ao-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
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
        DataSet lDADOSAnt = new DataSet();
        DataTable DT = new DataTable();
        DataTable DTAnt = new DataTable();
        DataView lDataView;
        String lID_OBJETIVOAnt = "-1";

        lDADOS = pDADOS;

        DT = CriaDataTable();
        DTAnt = CriaDataTable();

        foreach (DataRow lRow in lDADOS.Tables[0].Rows)
        {
            using (OBJETIVO objOBJETIVO = new OBJETIVO())
            {
                lDataView = objOBJETIVO.ConsultarOBJETIVOPorSiglaExecutivoAno(lRow["SIGLA"].ToString().Trim(), Session["RAO_ID_EXECUTIVO"].ToString().Trim(), Convert.ToString(Convert.ToInt32(Session["RAO_ANO"].ToString()) - 1)).Tables[0].DefaultView;
            }

            if (lDataView.Table.Rows.Count > 0)
            {
                lID_OBJETIVOAnt = lDataView.Table.Rows[0]["ID_OBJETIVO"].ToString();

                using (RELATORIO objRELATORIO = new RELATORIO())
                {
                    lDataView = objRELATORIO.ListarRELATORIOAcompanhamentoObjetivo(Session["RAO_ID_EXECUTIVO"].ToString(), Convert.ToString(Convert.ToInt32(Session["RAO_ANO"].ToString()) - 1), lID_OBJETIVOAnt).Tables[0].DefaultView;
                }


                for (int a = 1; a <= 12; a++)
                {
                    DTAnt.Rows.Add(DTAnt.NewRow());
                    DTAnt.Rows[DTAnt.Rows.Count - 1]["ID_OBJETIVO"] = lRow["ID_OBJETIVO"];
                    DTAnt.Rows[DTAnt.Rows.Count - 1]["NUM_MES"] = a;

                    switch (a)
                    {
                        case 1:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Jan";
                            break;
                        case 2:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Fev";
                            break;
                        case 3:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Mar";
                            break;
                        case 4:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Abr";
                            break;
                        case 5:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Mai";
                            break;
                        case 6:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Jun";
                            break;
                        case 7:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Jul";
                            break;
                        case 8:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Ago";
                            break;
                        case 9:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Set";
                            break;
                        case 10:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Out";
                            break;
                        case 11:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Nov";
                            break;
                        case 12:
                            DTAnt.Rows[DTAnt.Rows.Count - 1]["MES"] = "Dez";
                            break;
                    }

                    DTAnt.Rows[DTAnt.Rows.Count - 1]["VALOR_REALIZADO"] = lDataView.Table.Rows[0]["VALOR_REALIZADO" + Convert.ToString(a)];
                    DTAnt.Rows[DTAnt.Rows.Count - 1]["VALOR_ESPERADO"] = lDataView.Table.Rows[0]["VALOR_ESPERADO" + Convert.ToString(a)];
                    DTAnt.Rows[DTAnt.Rows.Count - 1]["VALOR_EXPECTATIVA"] = lDataView.Table.Rows[0]["VALOR_EXPECTATIVA" + Convert.ToString(a)];

                }
            }

            for (int i = 1; i <= 12; i++)
            {

                DT.Rows.Add(DT.NewRow());
                DT.Rows[DT.Rows.Count - 1]["ID_OBJETIVO"] = lRow["ID_OBJETIVO"];
                DT.Rows[DT.Rows.Count - 1]["NUM_MES"] = i;

                switch (i)
                {
                    case 1:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Jan";
                        break;
                    case 2:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Fev";
                        break;
                    case 3:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Mar";
                        break;
                    case 4:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Abr";
                        break;
                    case 5:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Mai";
                        break;
                    case 6:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Jun";
                        break;
                    case 7:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Jul";
                        break;
                    case 8:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Ago";
                        break;
                    case 9:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Set";
                        break;
                    case 10:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Out";
                        break;
                    case 11:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Nov";
                        break;
                    case 12:
                        DT.Rows[DT.Rows.Count - 1]["MES"] = "Dez";
                        break;
                }

                DT.Rows[DT.Rows.Count - 1]["VALOR_REALIZADO"] = lRow["VALOR_REALIZADO" + Convert.ToString(i)];
                DT.Rows[DT.Rows.Count - 1]["VALOR_ESPERADO"] = lRow["VALOR_ESPERADO" + Convert.ToString(i)];
                DT.Rows[DT.Rows.Count - 1]["VALOR_EXPECTATIVA"] = lRow["VALOR_EXPECTATIVA" + Convert.ToString(i)];

            }
        }

        lDADOSAnt.Tables.Add(DTAnt);

        Session["lDADOSAnt"] = lDADOSAnt;

        return DT;

    }

    private DataTable CriaDataTable()
    {
        DataTable mDataTable = new DataTable();
        DataColumn mDataColumn;

        mDataColumn = new DataColumn("ID_OBJETIVO", Type.GetType("System.Int32"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("NUM_MES", Type.GetType("System.Int32"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("MES", Type.GetType("System.String"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_REALIZADO", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_ESPERADO", Type.GetType("System.Double"));
        mDataTable.Columns.Add(mDataColumn);
        mDataColumn = new DataColumn("VALOR_EXPECTATIVA", Type.GetType("System.Double"));
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
