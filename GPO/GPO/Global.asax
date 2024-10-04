<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Par�metros de inicializa��o global para o Sistema

        // Nome e empresa do Sistema
        Application["app_Sigla"] = "GPO"; // Sigla da aplica��o
        Application["app_Nome"] = "Sistema de Gest�o por Objetivo"; // Nome da aplica��o
        Application["app_Ano"] = "2014"; // Ano de constru��o do sistema
        Application["app_Empresa"] = "2"; // 0 - Neoenergia; 1 - Celpe; 2 - Coelba; 3 - Cosern; 4 - PGD

        // Par�metros para colocar a aplica��o em estado de manuten��o
        Application["app_Manutencao"] = "0"; // 0 - Aplica��o ativa; 1 - Aplica��o em manuten��o
        Application["app_TxtManutencao"] = "<strong>Sistema em manuten��o.<p>&nbsp;</p>Contato<p>&nbsp;</p>PISI: 81-32176079 / 81-32175219</strong>."; //Texto que � apresentado na tela de manuten��o

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Captura a exce��o em uma application e redireciona para uma p�gina de erro customizada
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

