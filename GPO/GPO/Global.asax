<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Parâmetros de inicialização global para o Sistema

        // Nome e empresa do Sistema
        Application["app_Sigla"] = "GPO"; // Sigla da aplicação
        Application["app_Nome"] = "Sistema de Gestão por Objetivo"; // Nome da aplicação
        Application["app_Ano"] = "2014"; // Ano de construção do sistema
        Application["app_Empresa"] = "2"; // 0 - Neoenergia; 1 - Celpe; 2 - Coelba; 3 - Cosern; 4 - PGD

        // Parâmetros para colocar a aplicação em estado de manutenção
        Application["app_Manutencao"] = "0"; // 0 - Aplicação ativa; 1 - Aplicação em manutenção
        Application["app_TxtManutencao"] = "<strong>Sistema em manutenção.<p>&nbsp;</p>Contato<p>&nbsp;</p>PISI: 81-32176079 / 81-32175219</strong>."; //Texto que é apresentado na tela de manutenção

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Captura a exceção em uma application e redireciona para uma página de erro customizada
        // Exception ex = Server.GetLastError().GetBaseException();
        // Application["app_Erro"] = ex.Message;
        // Response.Redirect("erro.aspx");

        GPO.FW.TrataErro objErro;

        objErro = new GPO.FW.TrataErro(Server.GetLastError().GetBaseException(), Server.MapPath(".") + (string)("\\Erros.xml"));

        try
        {
            Session["Erro"] = (object)objErro;
        }
        catch (Exception ex)
        {
        }        

        //if (objErro.TipoErro == INSIX.FW.Excecao.TrataErro.TipoExcecao.NaoTratado) {          
        //}

        //if (Session["MP_TELA"].ToString() == "InternoCad")
          //  Server.Transfer("ErrorCad.aspx");
        //else
          //  Server.Transfer("Error.aspx");

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>

