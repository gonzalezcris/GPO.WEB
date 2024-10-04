using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public string app_Sigla;
    public string app_Nome;
    public string app_Sigla_Nome;
    public string app_Empresa;
    public string app_Manutencao;
    public string app_Ano;
    public string empresa_Nome;
    public string pagina_Nome;

    #region Load

    protected void Page_Load(object sender, EventArgs e)
    {
        App_Nome();
        App_Empresa();
        App_Ano();
        Checa_Manutencao();
        montarmenu();
        Session["MP_TELA"] = "Interno";
        try
        {
            lblNome.Text = Session["_NOME"].ToString() + " - " + Session["_PERFIL"].ToString();
        }
        catch (Exception ex)
        {
            lblNome.Text = "-";
        }
    }

    #endregion

    #region Menu
    void montarmenu()
    {

        Int16 numIteracao = Convert.ToInt16(ConfigurationManager.AppSettings["numIteracao"].ToString());

        StringBuilder stb = new StringBuilder();
        stb.AppendLine("<script>");
        stb.AppendLine("var menuItems = ");
        stb.AppendLine("[");
        //stb.AppendLine("['-'],");

        if (numIteracao >= 1)
        {
            if (Session["_MODULO"].ToString().Contains("|APR|"))
            {
                stb.AppendLine("['Aprovação &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Aprovação Executivo &nbsp;&nbsp;</div>','GPOAprovacaoExe.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Aprovação Superior &nbsp;&nbsp;</div>','GPOAprovacaoSup.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                
                if (Session["_PERFIL"].ToString() == "Administrador")
                   stb.AppendLine("['|<div id=menuitem>&nbsp;Cancela Aprovação &nbsp;&nbsp;</div>','GPOAprovacaoCancel.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                
                stb.AppendLine("['|<div id=menuitem>&nbsp;Relatório de Aprovação dos Objetivos &nbsp;&nbsp;</div>','GPORelAprovacaoObjetivos.aspx','../img/spacer.gif','../img/spacer.gif',''],");
            }

            if (Session["_MODULO"].ToString().Contains("|REL|"))
            {
                stb.AppendLine("['Relatório &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Fórmulas &nbsp;&nbsp;</div>','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Fórmulas Selecionadas &nbsp;&nbsp;</div>','GPORelFormulas.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Objetivos &nbsp;&nbsp;</div>','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Folha de Descrição dos Objetivos &nbsp;&nbsp;</div>','GPORelFolhaDescricaoObjetivos.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Folha Resumo dos Objetivos &nbsp;&nbsp;</div>','GPORelResumoObjetivos.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Acompanhamento &nbsp;&nbsp;</div>','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Acompanhamento dos Objetivos &nbsp;&nbsp;</div>','GPORelAcompObjetivos.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Acompanhamento Mensal dos Objetivos &nbsp;&nbsp;</div>','GPORelAcompMensalObjetivos.aspx','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['|<div id=menuitem>&nbsp;Avaliação Quantitativa &nbsp;&nbsp;</div>','#','../img/spacer.gif','../img/spacer.gif',''],");
                stb.AppendLine("['||<div id=menuitem>&nbsp;Avaliação Quantitativa do Órgão &nbsp;&nbsp;</div>','GPORelAvalQuantitativaOrgao.aspx','../img/spacer.gif','../img/spacer.gif',''],");
            }
        }
        //------------------------------------------------------------------------

        //------------------------------------------------------------------------
        stb.AppendLine("['Sair &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;','Default.aspx','../img/spacer.gif','../img/spacer.gif',''],");
        //------------------------------------------------------------------------

        //stb.AppendLine("['-'],");          
        stb.AppendLine("];");
        stb.AppendLine("</script>");
        Page.ClientScript.RegisterClientScriptBlock(typeof(System.String), "menu", stb.ToString());
    }


    #endregion

    # region App_Nome
    private void App_Nome()
    {
        // Recupera as variáveis globais do Global.asax
        app_Sigla = Application["app_Sigla"].ToString();
        app_Nome = Application["app_Nome"].ToString();
        app_Sigla_Nome = app_Nome + " - " + app_Sigla;

        // Atribui aos literais existentes no design o nome do Sistema
        //ltAppSiglaNomeTitulo.Text = app_Sigla_Nome;
        ltAppSilgaNomeTop.Text = app_Sigla_Nome;
        ltAppSilgaNomeBase.Text = app_Sigla_Nome;

        lblData.Text = DateTime.Now.ToString();
    }
    # endregion

    # region App_Empresa
    private void App_Empresa()
    {
        // Recupera as variáveis globais do Global.asax
        app_Empresa = Application["app_Empresa"].ToString();
        switch (app_Empresa)
        {
            case "0":
                empresa_Nome = "Neoenergia";
                break;
            case "1":
                empresa_Nome = "Celpe";
                break;
            case "2":
                empresa_Nome = "Coelba";
                break;
            case "3":
                empresa_Nome = "Cosern";
                break;
            case "4":
                empresa_Nome = "PGD";
                break;
            default:
                break;
        }
        // Atribui o número da empresa para pegar a Logomarca correta
        //ltImagemNumEmpresa.Text = app_Empresa;

        // Atribui aos literais existentes no design o nome da Empresa
        ltAppNomeEmpresa.Text = empresa_Nome;
        //ltImagemNomeEmpresa.Text = empresa_Nome;
    }
    # endregion

    # region App_Ano
    private void App_Ano()
    {
        // Recupera as variáveis globais do Global.asax
        app_Ano = Application["app_Ano"].ToString();

        // Atribui aos literais existentes no design o ano de construção do Sistema
        ltAppAnoSistema.Text = app_Ano;
    }
    # endregion

    # region Checa_Manutencao
    private void Checa_Manutencao()
    {
        pagina_Nome = Request.ServerVariables["URL"];
        pagina_Nome = pagina_Nome.Remove(0, pagina_Nome.LastIndexOf("/") + 1);
        if (pagina_Nome != "manutencao.aspx")
        {
            app_Manutencao = Application["app_Manutencao"].ToString();
            if (app_Manutencao == "1")
            {
                Response.Redirect("manutencao.aspx");
            }
        }
    }
    # endregion

}