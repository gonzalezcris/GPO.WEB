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
using System.Xml;
using GPO.BLL;
using GPO.ENTIDADES;
using GPO.DsAdmin;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCLB"].ToString();

            bool https = Convert.ToBoolean(ConfigurationManager.AppSettings["HTTPS"].ToString());
            if (https)
            {
                if (Request.ServerVariables["HTTPS"] != "on")
                {
                    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"] + "/GPO/");
                }
            }

            if ((Request["mo"] != null) && Session["mo"] == null)
            {
                if ((Session["mo"] == null) && (Request["mo"].ToString() == "s"))
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, btLoginOK.GetType(), "conf", "alert('Sua sessão expirou!');", true);
                    Session["mo"] = false;
                }
                else if ((Session["mo"] == null) && (Request["mo"].ToString() == "sc"))
                    ScriptManager.RegisterClientScriptBlock(this.Page, btLoginOK.GetType(), "conf", "window.close();", true);
            }

            //using (DominioRede objDom = new DominioRede())
            //{
            //    try
            //    {
            //        DataView dtDom = objDom.ListarDOMINIOSREDE().Tables[0].DefaultView;
            //        if (dtDom.Table.Rows.Count > 0)
            //        {
            //            DRE_COD.DataSource = dtDom;
            //            DRE_COD.DataTextField = "DRE_NOM";
            //            DRE_COD.DataValueField = "DRE_COD";
            //            DRE_COD.DataBind();
            //            DRE_COD.Items.Insert(0, new ListItem("--- SELECIONE ---", "-1"));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this.Page, btLoginOK.GetType(), "conf", "alert('Conexão com o Banco não estabelecida.');", true);
            //        return;
            //    }
            //}

        }

        Session["_VAZIO"] = "S";
        Session["_MODULO"] = "||";
        //Session["_MODULO_INC"] = "";
        //Session["_MODULO_ALT"] = "";
        //Session["_MODULO_EXC"] = "";
        Session["_ID_USUARIO"] = "";
        Session["_LOGIN"] = "";
        Session["_MATRICULA"] = "";
        Session["_NOME"] = "";
        Session["_PERFIL"] = "";
        Session["_EMPRESA"] = "";
        Session["_ID_EMPRESA"] = "";
        Session["_EXECUTIVO"] = "";
        Session["StringConexaoCripto"] = "";
        Session["ANU_ANO_CORRENTE"] = DateTime.Now.Year;
    }

    protected void btLoginOK_Click(object sender, EventArgs e)
    {
        if (!critica())
        {
            //*** procura usuário no GPO, se não encontrar busca no NEONET
            //*** o false é porque ainda não está logando via NEONET
            if (!BuscaUsuarioGPO() || false)
            {
                string retorno = "";
                //*** comentado para rodar em desenvolvimento
                NTSecureWeb.SecureWebClass objSecure = new NTSecureWeb.SecureWebClass();
                string sUser = txtLoginNome.Text.Trim().ToUpper();
                string sPass = txtLoginSenha.Text.Trim();
                string sDominio = DRE_COD.SelectedItem.Text;

                //*** comentado para rodar em desenvolvimento
                retorno = objSecure.Logon(ref sUser, ref sPass, ref sDominio).ToString();
                bool homologacao = Convert.ToBoolean(ConfigurationManager.AppSettings["Homologacao"].ToString());

                using (WebService objweb = new WebService())
                {
                    String XML = objweb.AutenticaWebService(sUser, sPass, sDominio, "GPO");
                    if ((!(XML.IndexOf("Usuário Inexistente") == 0) && (!(XML.IndexOf("Webservice não encontrado") == 0)) && (!(XML.IndexOf("Conexão não estabelecida com o webservice.") == 0))))
                    {
                        if (homologacao)
                        {
                            retorno = "I1000";
                        }

                        if (retorno != "I1000")
                        {
                            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Usuário ou senha inválido.');", true);
                            return;
                        }

                        carregarUsuarioValido(XML);

                        if (Session["_VAZIO"] == "S")
                        {
                            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Usuário sem acesso ao sistema.');", true);
                            return;
                        }
                        if (Session["StringConexaoCripto"].ToString() == "")
                        {
                            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Não foi possível definir o acesso ao banco de dados. Erro: " + Session["_EMPRESA"].ToString() + "');", true);
                            return;
                        }

                        Session["_MODULO"] = "||";
                        DataView lDataView;

                        //*** consutar subgrupo no stab
                        using (STAB objSTAB = new STAB())
                        {
                            lDataView = objSTAB.ConsultarEMPREGADOPorMatricula(Session["_MATRICULA"].ToString()).Tables[0].DefaultView;
                        }
                        if (lDataView.Table.Rows.Count > 0)
                        {
                            if (lDataView.Table.Rows[0]["COD_SGEM"].ToString().ToUpper() == "GT" ||
                                lDataView.Table.Rows[0]["COD_SGEM"].ToString().ToUpper() == "GR" ||
                                lDataView.Table.Rows[0]["COD_SGEM"].ToString().ToUpper() == "SU")
                            {
                                //*** procurar setor no GPO
                                using (EXECUTIVO objEXECUTIVO = new EXECUTIVO())
                                {
                                    lDataView = objEXECUTIVO.ConsultarEXECUTIVOPorNome(lDataView.Table.Rows[0]["DES_REDZ_SET"].ToString().ToUpper()).Tables[0].DefaultView;
                                }
                                if (lDataView.Table.Rows.Count > 0)
                                {
                                    Session["_EXECUTIVO"] = lDataView.Table.Rows[0]["ID_EXECUTIVO"].ToString();
                                    Session["_MODULO"] = "|APR|";
                                }
                            }
                        }

                        Server.Transfer("Principal.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('" + XML.ToString() + "');", true);
                        //Usuário inexistente
                    }
                }
            }
        }
    }

    protected Boolean BuscaUsuarioGPO()
    {
        Boolean lRetorno = true;
        String vMsgerro = String.Empty;

        //if (String.IsNullOrEmpty(this.txtLoginNome.Text) || String.IsNullOrEmpty(this.txtLoginSenha.Text))
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('Usuário ou senha inválido.');", true);
        //    return false;
        //}

        Session["_MATRICULA"] = this.txtLoginNome.Text.Trim().ToUpper();
        Session["_ID_EMPRESA"] = DRE_COD.SelectedValue.ToString();

        if (Session["_ID_EMPRESA"].ToString() == "1")
        {
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCLB"].ToString();
        }
        else if (Session["_ID_EMPRESA"].ToString() == "3")
        {
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCLP"].ToString();
        }
        else if (Session["_ID_EMPRESA"].ToString() == "2")
        {
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCSN"].ToString();
        }
        else if (Session["_ID_EMPRESA"].ToString() == "4")
        {
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoNEO"].ToString();
        }

        String l_Nome = this.txtLoginNome.Text.Trim().ToUpper();
        String l_Senha = this.txtLoginSenha.Text.Trim().ToUpper();

        Usuario usuario = null;
        Perfil perfil = null;
        UsuarioBLL usuarioBLL = null;
        PerfilBLL perfilBLL = null;

        try
        {

            perfilBLL = new PerfilBLL();
            usuarioBLL = new UsuarioBLL();

            // validação login e senha   
            usuario = usuarioBLL.ValidarUsuario(l_Nome, l_Senha);

            if (usuario != null)
            {
                // Mudar Status Usuario
                usuarioBLL.AtualizaStatusUsuario(usuario.Id_Usuario);

                // Obter Perfil do Usuário
                perfil = perfilBLL.ConsultaPerfilUsuario(usuario.Id_Usuario);
                Session["_NOME"] = usuario.Nick;
                Session["_ID_USUARIO"] = usuario.Id_Usuario;
                //Session["_SG_EMPRESA"] = usuario.Sg_Empresa;
                /*if (usuario.Sg_Empresa.Trim() == "COELBA")
                    Session["_ID_EMPRESA"] = 1;
                else if (usuario.Sg_Empresa.Trim() == "COSERN")
                    Session["_ID_EMPRESA"] = 2;
                else if (usuario.Sg_Empresa.Trim() == "CELPE")
                    Session["_ID_EMPRESA"] = 3;*/
                //Session["_ID_EMPRESA"] = usuario.Id_Empresa;

                if (perfil != null)
                {
                    Session["_PERFIL"] = perfil.Descricao;
                    //*** Verifica o perfil do usuário
                    if ((perfil.Sigla == "P-0") || (perfil.Sigla == "APROVADOR")) 
                        Session["_MODULO"] = "|APR||REL|";
                    else
                    {
                       //*** verifica se tem transação de aprovador(ID_TRANSACAO = 8) 
                        DataView lDataView;
                        using (TRANSACAO objTRANSACAO = new TRANSACAO())
                        {  
                           lDataView = objTRANSACAO.ConsultarTransacaoPorUsuario(Session["_ID_USUARIO"].ToString(), "8").Tables[0].DefaultView;
                        }
                        if (lDataView.Table.Rows.Count > 0)
                            Session["_MODULO"] = "|APR||REL|";
                        else
                            Session["_MODULO"] = "|REL|";
                    }
                }

                Response.Redirect("Principal.aspx", false);
            }
            else
            {
                lRetorno = false;
            }
        }
        catch (Exception ex)
        {
            lRetorno = false;

            vMsgerro = ex.Message;
            vMsgerro = vMsgerro.Replace("'", "");
            vMsgerro = vMsgerro.Replace("\n", " ");

            ScriptManager.RegisterStartupScript(btLoginOK, btLoginOK.GetType(), "int", "alert('" + vMsgerro + "');", true);
        }

        return lRetorno;
    }

    private bool critica()
    {
        string msgError;

        if (txtLoginNome.Text.Trim() == "")
        {
            msgError = "Informe o Login";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('" + msgError + "')", true);
            txtLoginNome.Focus();
            return true;
        }
        else if (txtLoginSenha.Text.Trim() == "")
        {
            msgError = "Informe a Senha";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('" + msgError + "')", true);
            txtLoginSenha.Focus();
            return true;
        }
        else if (DRE_COD.SelectedIndex == 0)
        {
            msgError = "Informe o Domínio";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "GPO", "alert('" + msgError + "')", true);
            DRE_COD.Focus();
            return true;
        }

        return false;
    }

    private void carregarUsuarioValido(string XML)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(XML);

        XmlNodeList xmlMod = doc.GetElementsByTagName("COD_MODULO");
        XmlNodeList xmlModNom = doc.GetElementsByTagName("NOME_MODULO");
        XmlNodeList xmlMatrUsu = doc.GetElementsByTagName("MATR_USUARIO");
        XmlNodeList xmlNomUsu = doc.GetElementsByTagName("NOME_USUARIO");
        XmlNodeList xmlOperInc = doc.GetElementsByTagName("INCLUSAO");
        XmlNodeList xmlOperAlt = doc.GetElementsByTagName("ALTERACAO");
        XmlNodeList xmlOperExc = doc.GetElementsByTagName("EXCLUSAO");
        XmlNodeList xmlOperCons = doc.GetElementsByTagName("CONSULTA");
        XmlNodeList xmlOperImp = doc.GetElementsByTagName("IMPRESSAO");
        XmlNodeList xmlCodEmp = doc.GetElementsByTagName("COD_EMPRESA");

        //Session["_UF"] = "BA";
        Session["_VAZIO"] = "S";
        Session["_MODULO"] = "";
        //Session["_MODULO_INC"] = "";
        //Session["_MODULO_ALT"] = "";
        //Session["_MODULO_EXC"] = "";
        //Session["_MODULO_IMP"] = "";

        for (int i = 0; i < xmlMod.Count; i++)
        {
            Session["_VAZIO"] = "N";

            if ((xmlOperInc[i].FirstChild.InnerText == "S") || (xmlOperAlt[i].FirstChild.InnerText == "S") || (xmlOperExc[i].FirstChild.InnerText == "S") || (xmlOperCons[i].FirstChild.InnerText == "S") || (xmlOperImp[i].FirstChild.InnerText == "S"))
                Session["_MODULO"] += "|" + xmlMod[i].FirstChild.InnerText + "|";
            //if (xmlOperInc[i].FirstChild.InnerText == "S")
            //    Session["_MODULO_INC"] += "|" + xmlMod[i].FirstChild.InnerText + "|";
            //if (xmlOperAlt[i].FirstChild.InnerText == "S")
            //    Session["_MODULO_ALT"] += "|" + xmlMod[i].FirstChild.InnerText + "|";
            //if (xmlOperExc[i].FirstChild.InnerText == "S")
            //    Session["_MODULO_EXC"] += "|" + xmlMod[i].FirstChild.InnerText + "|";
            //if (xmlOperExc[i].FirstChild.InnerText == "S")
            //    Session["_MODULO_IMP"] += "|" + xmlMod[i].FirstChild.InnerText + "|";
        }

        Session["_LOGIN"] = xmlMatrUsu[0].FirstChild.InnerText;
        Session["_NOME"] = xmlNomUsu[0].FirstChild.InnerText;
        Session["_EMPRESA"] = xmlCodEmp[0].FirstChild.InnerText;
        Session["_MATRICULA"] = txtLoginNome.Text.Trim().ToUpper();

        if (txtLoginNome.Text.Trim().ToUpper().Substring(0, 3) == "CLB" ||
            txtLoginNome.Text.Trim().ToUpper().Substring(0, 3) == "CSR")
            Session["_MATRICULA"] = txtLoginNome.Text.Trim().ToUpper().Substring(3, 6);
        else if (txtLoginNome.Text.Trim().ToUpper().Substring(0, 3) == "CLP")
            Session["_MATRICULA"] = "1" + txtLoginNome.Text.Trim().ToUpper().Substring(3, 6);

        //1	    CELPE	
        //2	    COELBA	
        //3	    COSERN	
        //13	TERC. CLP
        //14	TERC. CSR
        //15	TERC. CLB

        if (Session["_EMPRESA"].ToString() == "2" || Session["_EMPRESA"].ToString() == "15")
        {
            Session["_ID_EMPRESA"] = 1;
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCLB"].ToString();
        }
        else if (Session["_EMPRESA"].ToString() == "1" || Session["_EMPRESA"].ToString() == "13")
        {
            Session["_ID_EMPRESA"] = 3;
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCLP"].ToString();
        }
        else if (Session["_EMPRESA"].ToString() == "3" || Session["_EMPRESA"].ToString() == "14")
        {
            Session["_ID_EMPRESA"] = 2;
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoCSN"].ToString();
        }
        else
        {
            Session["_ID_EMPRESA"] = 4;
            Session["StringConexaoCripto"] = ConfigurationManager.AppSettings["StringConexaoCriptoNEO"].ToString();
        }

        CarregaCookie();
    }

    private void CarregaCookie()
    {
        HttpCookie cookie = new HttpCookie(System.Configuration.ConfigurationManager.AppSettings["NomeCookie"]);
        cookie["_MODULO"] = Session["_MODULO"].ToString();
        //cookie["_MODULO_INC"] = Session["_MODULO_INC"].ToString();
        //cookie["_MODULO_ALT"] = Session["_MODULO_ALT"].ToString();
        //cookie["_MODULO_EXC"] = Session["_MODULO_EXC"].ToString();
        //cookie["_MODULO_IMP"] = Session["_MODULO_IMP"].ToString();
        cookie["_ID_USUARIO"] = Session["_ID_USUARIO"].ToString();
        cookie["_LOGIN"] = Session["_LOGIN"].ToString();
        cookie["_MATRICULA"] = Session["_MATRICULA"].ToString();
        cookie["_NOME"] = Session["_NOME"].ToString();
        cookie["_PERFIL"] = Session["_PERFIL"].ToString();
        cookie["_ID_EMPRESA"] = Session["_ID_EMPRESA"].ToString();
        cookie["_EXECUTIVO"] = Session["_EXECUTIVO"].ToString();
        cookie["StringConexaoCripto"] = Session["StringConexaoCripto"].ToString();
        Response.Cookies.Add(cookie);
    }
}
