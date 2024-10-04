<%@ Page Language="C#" MasterPageFile="~/masterpages/InternoCad.master" AutoEventWireup="true" CodeFile="ErrorCad.aspx.cs" Inherits="ErrorCad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" Width="70px" OnClientClick="JavaScript:window.close();" />
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

</asp:Content>

