using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Collections.Generic;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string AutenticaWebService(String sLogin, String sSenha, String sDomain, String sSistema)
    {
        String retorno = "";
        string xml = "";
        //*** comentado para rodar em desenvolvimento
        Neocriptografia.Criptografia objNeoCon = new Neocriptografia.Criptografia();
        string sSenhaCript = objNeoCon.Encripta(sSenha);
        //string sSenhaCript = sSenha;
        retorno = System.Configuration.ConfigurationManager.AppSettings["HTTP_HOSTXML"].ToString() + "?txtLogin=" + sLogin.Trim() + "&txtSenha=" + sSenhaCript.Trim() + "&cbxDominio=" + sDomain.Trim().ToUpper() + "&sistema=" + sSistema;
        XmlTextReader reader = new XmlTextReader(retorno);

        XmlDocument docXml = new XmlDocument();
        bool carregado = false;

        try
        {
            docXml.Load(retorno);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.DocumentType:
                        xml += "</" + reader.Name;
                        break;
                    case XmlNodeType.Element:
                        xml += "<" + reader.Name;
                        while (reader.MoveToNextAttribute())
                            xml += " " + reader.Name + "='" + reader.Value + "'";
                        xml += ">";
                        break;
                    case XmlNodeType.Text:
                        xml += reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        xml += "</" + reader.Name;
                        xml += ">";
                        break;

                    case XmlNodeType.Attribute:
                        xml += "</" + reader.Name;
                        break;

                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                docXml.Load(retorno);
            }
            catch (Exception exx)
            {
                try
                {
                    docXml.Load(retorno);
                }
                catch (Exception ex3)
                {
                    if (ex3.Message.Contains("404"))
                    {
                        xml += "Webservice não encontrado";
                    }
                    else if (ex3.Message.Contains("500"))
                    {
                        xml += "Conexão não estabelecida com o webservice.";
                    }
                    else if (ex3.Message.Contains("Unable to connect to the remote server"))
                    {
                        xml += "Webservice não encontrado";
                    }
                    else
                    {
                        xml += "Usuário Inexistente";
                    }
                }
            }
        }

        return xml;

    }

}

