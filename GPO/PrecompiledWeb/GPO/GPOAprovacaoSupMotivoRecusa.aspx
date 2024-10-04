<%@ page language="C#" masterpagefile="~/masterpages/InternoCad.master" autoeventwireup="true" inherits="GPOAprovacaoSupMotivoRecusa, App_Web_l2naqbkc" enableEventValidation="false" stylesheettheme="Padrao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scmPrincipal" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True">
    </asp:ScriptManager>
    <asp:Panel ID="pnlConfirmar" runat="server" DefaultButton="btnConfirmar">
        <h1>
            <asp:Label ID="lblTitulo" runat="server">Motivos de Recusa</asp:Label></h1>
        <br />
        <div id="topo">
            INFORME OS MOTIVOS DAS RECUSAS
        </div>
        <div id="filtro">
            <asp:GridView ID="grvPrincipal" runat="server" AllowPaging="false" PageSize="25"
                AllowSorting="false" AutoGenerateColumns="False" OnRowDataBound="grvPrincipal_RowDataBound"
                DataKeyNames="ID_OBJETIVO" DataSourceID="odsGrvPrincipal" EnableViewState="False"
                Width="100%" ShowFooter="true">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="15px" VerticalAlign="Top"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text="*Objetivo:"></asp:Label>
                            <asp:Label ID="lblObjetivo" runat="server" SkinID="lblDados" Text=""></asp:Label><br />
                            <asp:TextBox ID="MOTIVO_RECUSA" runat="server" TextMode="MultiLine" Rows="5" Height="100px" Width="400px"></asp:TextBox>
                            <br /><br />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    Nenhum registro foi encontrado!
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsGrvPrincipal" runat="server" SelectMethod="ListarOBJETIVOParaMotivoRecusa"
                TypeName="GPO.DsAdmin.OBJETIVO" EnableViewState="False">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ID_EXECUTIVO" Name="ID_EXECUTIVO" Type="String"
                        DefaultValue="-1" />
                    <asp:QueryStringParameter QueryStringField="ANO" Name="ANO" Type="String" DefaultValue="-1" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <i>(*) - Preenchimento obrigatório</i>
        </div>
        <br />
        <div id="botao">
            <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" EnableViewState="False"
                OnClick="btnConfirmar_Click" />
<%--            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                EnableViewState="False" OnClientClick="window.close();return false;" />--%>
        </div>
    </asp:Panel>
</asp:Content>
