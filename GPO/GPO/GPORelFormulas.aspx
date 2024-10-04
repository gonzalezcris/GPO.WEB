<%@ Page Language="C#" MasterPageFile="~/masterpages/Interno.master" AutoEventWireup="true"
    CodeFile="GPORelFormulas.aspx.cs" Inherits="GPORelFormulas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scmPrincipal" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True" OnAsyncPostBackError="HandleError">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uppPrincipal" runat="server" AssociatedUpdatePanelID="upnPrincipal">
        <progresstemplate>
        </progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upnPrincipal" runat="server">
        <contenttemplate>
    <h1>
        <asp:Label ID="lblTitulo" runat="server" EnableViewState="False">Fórmulas</asp:Label></h1>
    <br />
    <asp:Panel ID="pnlPesquisa" runat="server" DefaultButton="btnImprimir">
        <div id="topo">
            FILTRO</div>
        <div id="filtro">
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="width: 70px">
                            <asp:Label ID="Label22" runat="server" Text="*Fórmula:" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="NOME" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe a fórmula!"
                                ControlToValidate="NOME" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div id="botao">
            <asp:Button ID="btnImprimir" OnClick="btnImprimir_Click" runat="server" Text="Visualizar"
                Width="80px"></asp:Button>
        </div>
        <br />
        * Campos obrigatórios.
    </asp:Panel>
</contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
