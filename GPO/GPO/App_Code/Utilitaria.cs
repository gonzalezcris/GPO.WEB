using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

/// <summary>
/// Summary description for Utilitaria
/// </summary>
public class Utilitaria
{
    public Utilitaria()
    {
    }

    public static bool VerificaCookie()
    {
        bool lRetorno = false;
        HttpCookie cookie = HttpContext.Current.Request.Cookies[System.Configuration.ConfigurationManager.AppSettings["NomeCookie"]];
        if (cookie != null)
        {
            lRetorno = true;
            HttpContext.Current.Session["_ID_USUARIO"] = cookie["_ID_USUARIO"].ToString();
            HttpContext.Current.Session["_MODULO"] = cookie["_MODULO"].ToString();
            HttpContext.Current.Session["_LOGIN"] = cookie["_LOGIN"].ToString();
            HttpContext.Current.Session["_MATRICULA"] = cookie["_MATRICULA"].ToString();
            HttpContext.Current.Session["_NOME"] = cookie["_NOME"].ToString();
            HttpContext.Current.Session["_PERFIL"] = cookie["_PERFIL"].ToString();
            HttpContext.Current.Session["_ID_EMPRESA"] = cookie["_ID_EMPRESA"].ToString();
            HttpContext.Current.Session["_EXECUTIVO"] = cookie["_EXECUTIVO"].ToString();
            HttpContext.Current.Session["StringConexaoCripto"] = cookie["StringConexaoCripto"].ToString();
        }
        return lRetorno;
    }

    public static bool ExportCrystalToPDF(ReportDocument pReport, string pPath)
    {
        bool lRetorno = false;
        try
        {
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = pPath; // HttpContext.Current.Server.MapPath(".") + "csharpnet-informations.pdf";
            CrExportOptions = pReport.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            pReport.Export();
            lRetorno = true;
        }
        catch (Exception ex)
        {
            lRetorno = false;
        }
        return lRetorno;
    }

    public static void LimparArquivos(string pPath)
    {
        string[] lFiles = Directory.GetFiles(pPath);
        foreach (string lFile in lFiles)
        {
            FileInfo lFileInf = new FileInfo(lFile);
            if (((TimeSpan)(DateTime.Now - lFileInf.LastWriteTime)).Days > 5)
            {
                try
                {
                    File.Delete(lFile);
                }
                catch (System.UnauthorizedAccessException ex)
                {
                }
                catch (Exception ex2)
                {
                    throw (ex2);
                }
                
            }
        }
    }
}
