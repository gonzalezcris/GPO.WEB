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

public partial class GPORelFolhaDescricaoObjetivosImp : System.Web.UI.Page
{
    private ReportDocument crReportDocument = new ReportDocument();
   private ReportDocument crReportSubPrj = new ReportDocument();
   private ReportDocument crReportSubAvl = new ReportDocument();
   private ReportDocument crReportSubLim = new ReportDocument();
   private ReportDocument crReportSubApr = new ReportDocument();

    #region " UnLoad "

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        crReportDocument.Close();
        crReportDocument.Dispose();

        crReportSubPrj.Close();
        crReportSubPrj.Dispose();      
        crReportSubLim.Close();
        crReportSubLim.Dispose();
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
        //ReportDocument crReportSubPrj = new ReportDocument();
        //ReportDocument crReportSubAvl = new ReportDocument();
        //ReportDocument crReportSubLim = new ReportDocument();
        //ReportDocument crReportSubApr = new ReportDocument();

        DataView lDataView = null;
        DataView lDataViewPrj = null;
        DataView lDataViewAvl = null;
        DataView lDataViewLim = null;
        DataView lDataViewApr = null;
        //StringBuilder stb = new StringBuilder();
        //string strFiltro = "";
        //string strPeriodo = "";

        //*** Legado
        if (Convert.ToInt32(Session["RFDO_ANO"].ToString()) <= 2013)
            crReportDocument.Load(Request.MapPath("RELFolhaDescricaoObjetivosLegado.rpt"));
        else
            crReportDocument.Load(Request.MapPath("RELFolhaDescricaoObjetivos.rpt"));
        crReportSubPrj = crReportDocument.OpenSubreport("RELFolhaDescricaoObjetivosPrj.rpt");
        crReportSubAvl = crReportDocument.OpenSubreport("RELFolhaDescricaoObjetivosAvl.rpt");
        crReportSubLim = crReportDocument.OpenSubreport("RELFolhaDescricaoObjetivosLim.rpt");
        crReportSubApr = crReportDocument.OpenSubreport("RELAprovadores.rpt");

        using (RELATORIO objRELATORIO = new RELATORIO())
        {
            lDataView = objRELATORIO.ListarRELATORIOFolhaDescricaoObjetivos(Session["RFDO_ID_EMPRESA"].ToString(), Session["RFDO_ID_EXECUTIVO"].ToString(), Session["RFDO_ANO"].ToString()).Tables[0].DefaultView;

            if (lDataView.Table.Rows.Count == 0)
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Nenhum dado encontrado.');window.close();", true);
                return;
            }

            lDataViewPrj = objRELATORIO.ListarRELATORIOFolhaDescricaoObjetivosPrj(Session["RFDO_ID_EMPRESA"].ToString(), Session["RFDO_ID_EXECUTIVO"].ToString(), Session["RFDO_ANO"].ToString()).Tables[0].DefaultView;
            //*** Legado
            if (Convert.ToInt32(Session["RFDO_ANO"].ToString()) <= 2013)
                lDataViewAvl = objRELATORIO.ListarRELATORIOFolhaDescricaoObjetivosAvl(Session["RFDO_ID_EMPRESA"].ToString(), Session["RFDO_ID_EXECUTIVO"].ToString(), Session["RFDO_ANO"].ToString()).Tables[0].DefaultView;
            else
            {
                lDataViewLim = objRELATORIO.ListarRELATORIOFolhaDescricaoObjetivosLim(Session["RFDO_ID_EMPRESA"].ToString(), Session["RFDO_ID_EXECUTIVO"].ToString(), Session["RFDO_ANO"].ToString()).Tables[0].DefaultView;
                lDataViewApr = objRELATORIO.ListarRELATORIOAprovadores(Session["RFDO_ID_EXECUTIVO"].ToString(), Session["RFDO_ANO"].ToString()).Tables[0].DefaultView;
            }
        }

        foreach (DataRow lRow in lDataView.Table.Rows)
        {
            lRow["DEFINICAO_INDICADOR"] = ConvertoRFTtoString(lRow["DEFINICAO_INDICADOR"].ToString());
            lRow["METODO_CALCULO"] = ConvertoRFTtoString(lRow["METODO_CALCULO"].ToString());
        }

        crReportDocument.SetDataSource(lDataView.Table);
        //crReportDocument.Subreports[0].SetDataSource(lDataViewPrj.Table);
        crReportSubPrj.SetDataSource(lDataViewPrj.Table);
        //*** Legado
        if (Convert.ToInt32(Session["RFDO_ANO"].ToString()) <= 2013)
            crReportSubAvl.SetDataSource(lDataViewAvl.Table);
        else
        {
            crReportSubLim.SetDataSource(lDataViewLim.Table);
            crReportSubApr.SetDataSource(lDataViewApr.Table);
        }

        if (Session["RFDO_SG_EMPRESA"].ToString().Trim() == "COELBA")
            Session["RFDO_ID_EMPRESA"] = 1;
        else if (Session["RFDO_SG_EMPRESA"].ToString().Trim() == "COSERN")
            Session["RFDO_ID_EMPRESA"] = 2;
        else if (Session["RFDO_SG_EMPRESA"].ToString().Trim() == "CELPE")
            Session["RFDO_ID_EMPRESA"] = 3;

        crReportDocument.ParameterFields["ano"].CurrentValues.AddValue(Session["RFDO_ANO"].ToString());
        crReportDocument.ParameterFields["orgao"].CurrentValues.AddValue(" " + Session["RFDO_DS_EXECUTIVO"].ToString());
        crReportDocument.ParameterFields["empresa"].CurrentValues.AddValue(Session["RFDO_ID_EMPRESA"].ToString());

        //crReportDocumentAux = crReportDocument;
        //viewReport.ReportSource = crReportDocumentAux;

        string lFile = "fdo-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
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

    private string ConvertoRFTtoString(string pRTF)
    {
        //// If your RTF file isn't in the same folder as the .exe file for the project, 
        //// specify the path to the file in the following assignment statement. 
        //string path = @"test.rtf";

        ////Create the RichTextBox. (Requires a reference to System.Windows.Forms.)
        //System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

        //// Get the contents of the RTF file. When the contents of the file are  
        //// stored in the string (rtfText), the contents are encoded as UTF-16. 
        //string rtfText = System.IO.File.ReadAllText(path);

        //// Display the RTF text. This should look like the contents of your file.
        //System.Windows.Forms.MessageBox.Show(rtfText);

        //// Use the RichTextBox to convert the RTF code to plain text.
        //rtBox.Rtf = rtfText;
        //string plainText = rtBox.Text;

        //// Display the plain text in a MessageBox because the console can't  
        //// display the Greek letters. You should see the following result: 
        ////   The Greek word for "psyche" is spelled ψυχή. The Greek letters are
        ////   encoded in Unicode.
        ////   These characters are from the extended ASCII character set (Windows
        ////   code page 1252): âäӑå
        //System.Windows.Forms.MessageBox.Show(plainText);

        //// Output the plain text to a file, encoded as UTF-8. 
        //System.IO.File.WriteAllText(@"output.txt", plainText);

        string lRetorno = "";

        System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

        try
        {
            rtBox.Rtf = pRTF;
            lRetorno = rtBox.Text;
        }
        catch (Exception ex)
        {
            lRetorno = pRTF;
        }


        return lRetorno;
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
