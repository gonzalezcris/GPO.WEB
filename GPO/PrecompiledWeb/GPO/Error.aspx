<%@ page language="C#" autoeventwireup="true" inherits="Error, App_Web_l2naqbkc" enableEventValidation="false" stylesheettheme="Padrao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GPO - Sistema Gestão de Programa de Objetivo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <asp:ScriptManager ID="scmPrincipal" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True">
    </asp:ScriptManager>
    <h1>
        <asp:Label ID="lblTitulo" runat="server" EnableViewState="False">Erro</asp:Label></h1>
    <br />
    <div id="topo">
    </div>
    <asp:UpdatePanel ID="upnPrincipal" runat="server">
        <ContentTemplate>
            <div id="grid">
                <asp:Label ID="Label1" runat="server">Erro em:</asp:Label><br>
                <asp:Label ID="lblLocalErro" runat="server" SkinID="lblForm"></asp:Label><br>
                <br>
                <asp:Label ID="Label3" runat="server">Dercrição do erro:</asp:Label><br>
                <asp:Label ID="lblDescricaoErro" runat="server" SkinID="lblForm"></asp:Label><br>
                <br>
            </div>
            <br>
            <div id="botao">
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" Width="70px" OnClick="btnVoltar_Click" />
                <asp:Button ID="btnDetalhes" runat="server" Text="Detalhes" Width="70px" OnClick="btnDetalhes_Click" />
            </div>
            <br><br>
            <div id="Div1">
                <asp:Label ID="lblTitStackTrace" runat="server" Visible="False">Stack Trace:</asp:Label>
                <br>
                <asp:Label ID="lblStackTrace" runat="server" SkinID="lblForm" CssClass="lblForm"
                    Visible="False"></asp:Label>
                <br>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
