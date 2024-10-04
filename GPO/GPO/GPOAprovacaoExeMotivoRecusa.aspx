<%@ Page Language="C#" MasterPageFile="~/masterpages/InternoCad.master" AutoEventWireup="true"
    CodeFile="GPOAprovacaoExeMotivoRecusa.aspx.cs" Inherits="GPOAprovacaoExeMotivoRecusa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scmPrincipal" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True">
    </asp:ScriptManager>
    <asp:Panel ID="pnlConfirmar" runat="server">
        <h1>
            <asp:Label ID="lblTitulo" runat="server">Motivos de Recusa</asp:Label></h1>
        <br />
        <div id="topo">
            INFORME OS MOTIVOS DAS RECUSAS
        </div>
        <div id="filtro">
            <table border="0" cellpadding="3" cellspacing="0" style="width: 100%">
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server" EnableViewState="False" SkinID="lblDados" Text="Motivo da Recusa:"></asp:Label>
                        <br /><br />
                        <asp:Label ID="MOTIVO_RECUSA" runat="server" EnableViewState="False" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <br />
        <div id="botao">
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                EnableViewState="False" OnClientClick="window.close();return false;" />
        </div>
    </asp:Panel>
</asp:Content>
