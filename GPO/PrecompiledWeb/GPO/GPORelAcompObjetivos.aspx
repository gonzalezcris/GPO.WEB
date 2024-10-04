<%@ page language="C#" masterpagefile="~/masterpages/Interno.master" autoeventwireup="true" inherits="GPORelAcompObjetivos, App_Web_l2naqbkc" enableEventValidation="false" stylesheettheme="Padrao" %>

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
        <asp:Label ID="lblTitulo" runat="server" EnableViewState="False">Acompanhamento dos Objetivos</asp:Label></h1>
    <br />
    <asp:Panel ID="pnlPesquisa" runat="server" DefaultButton="btnImprimir">
        <div id="topo">
            FILTRO</div>
        <div id="filtro">
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="width: 70px">
                            <asp:Label ID="Label22" runat="server" Text="*Empresa:" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ID_EMPRESA" runat="server" EnableViewState="False" AppendDataBoundItems="True"
                                DataValueField="ID_EMPRESA" DataTextField="SIGLA" DataSourceID="odsDDLID_EMPRESA"
                                AutoPostBack="true">
                                <asp:ListItem Value="-1">Selecione</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Selecione a Empresa!"
                                ControlToValidate="ID_EMPRESA" Text="*" InitialValue="-1"></asp:RequiredFieldValidator>
                            <asp:ObjectDataSource ID="odsDDLID_EMPRESA" runat="server" EnableViewState="False"
                                TypeName="GPO.DsAdmin.EMPRESA" SelectMethod="ListarEMPRESA"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label31" runat="server" Text="*Órgão:" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ID_EXECUTIVO" runat="server" EnableViewState="False" AppendDataBoundItems="True"
                                DataValueField="ID_EXECUTIVO" DataTextField="NOME" DataSourceID="odsDDLID_EXECUTIVO" 
                                AutoPostBack="true">
                                <asp:ListItem Value="-1">Selecione</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator123" runat="server" ErrorMessage="Selecione o Órgão!"
                                ControlToValidate="ID_EXECUTIVO" Text="*" InitialValue="-1"></asp:RequiredFieldValidator>
                            <asp:ObjectDataSource ID="odsDDLID_EXECUTIVO" runat="server" EnableViewState="False"
                                TypeName="GPO.DsAdmin.EXECUTIVO" SelectMethod="ListarEXECUTIVO">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ID_EMPRESA" DefaultValue="-1" Name="ID_EMPRESA"
                                        PropertyName="SelectedValue" Type="String" />
                                        <asp:SessionParameter Name="ID_USUARIO" SessionField="_ID_USUARIO" DefaultValue="" Type="String" />
                                        <asp:Parameter Name="TIPO" DefaultValue="5" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="*Ano:" EnableViewState="False"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ANO" runat="server" EnableViewState="False" AppendDataBoundItems="True"
                                DataValueField="ANO" DataTextField="ANO" DataSourceID="odsDDLOBJETIVO" AutoPostBack="true">
                                <asp:ListItem Value="-1">Selecione</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Selecione o Ano!"
                                ControlToValidate="ANO" Text="*" InitialValue="-1"></asp:RequiredFieldValidator>
                            <asp:ObjectDataSource ID="odsDDLOBJETIVO" runat="server" EnableViewState="False"
                                TypeName="GPO.DsAdmin.OBJETIVO" SelectMethod="ListarOBJETIVOAno"></asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Objetivo:" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ID_OBJETIVO" runat="server" EnableViewState="False" AppendDataBoundItems="True"
                                DataValueField="ID_OBJETIVO" DataTextField="NOME" DataSourceID="odsDDLID_OBJETIVO">
                                <asp:ListItem Value="-1">Todos</asp:ListItem>
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsDDLID_OBJETIVO" runat="server" EnableViewState="False"
                                TypeName="GPO.DsAdmin.OBJETIVO" SelectMethod="ListarOBJETIVOPorOrgao">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ID_EXECUTIVO" DefaultValue="-1" Name="ID_EXECUTIVO"
                                        PropertyName="SelectedValue" Type="String" />
                                    <asp:ControlParameter ControlID="ANO" DefaultValue="-1" Name="ANO"
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
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
