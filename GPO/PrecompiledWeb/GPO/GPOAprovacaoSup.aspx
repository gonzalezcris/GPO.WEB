<%@ page language="C#" masterpagefile="~/masterpages/Interno.master" autoeventwireup="true" inherits="GPOAprovacaoSup, App_Web_l2naqbkc" enableEventValidation="false" stylesheettheme="Padrao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scmPrincipal" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True" OnAsyncPostBackError="HandleError">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uppPrincipal" runat="server" AssociatedUpdatePanelID="upnPrincipal">
        <ProgressTemplate>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <script type="text/javascript" language="javascript">
function modalWin(url) {
if (window.showModalDialog) {
window.showModalDialog(url,"name","dialogWidth:700px;dialogHeight:470px");
document.aspnetForm.submit();
}
}
    </script>

    <asp:UpdatePanel ID="upnPrincipal" runat="server">
        <ContentTemplate>
            <h1>
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False">Aprova��o Superior</asp:Label></h1>
            <br />
            <asp:Panel ID="pnlIncluir" runat="server">
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
                                    <asp:Label ID="Label3" runat="server" Text="*�rg�o:" EnableViewState="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ID_EXECUTIVO" runat="server" EnableViewState="False" AppendDataBoundItems="True"
                                        DataValueField="ID_EXECUTIVO" DataTextField="NOME" DataSourceID="odsDDLID_EXECUTIVO">
                                        <asp:ListItem Value="-1">Selecione</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Selecione o �rg�o!"
                                        ControlToValidate="ID_EXECUTIVO" Text="*" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:ObjectDataSource ID="odsDDLID_EXECUTIVO" runat="server" EnableViewState="False"
                                        TypeName="GPO.DsAdmin.EXECUTIVO" SelectMethod="ListarOrgaoMenuSuperior">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ID_EMPRESA" DefaultValue="-1" Name="ID_EMPRESA"
                                                PropertyName="SelectedValue" Type="String" />
                                            <asp:SessionParameter Name="ID_USUARIO" SessionField="_ID_USUARIO" DefaultValue=""
                                                Type="String" />
                                            <asp:Parameter Name="TIPO" DefaultValue="8" Type="String" />
                                            <%--<asp:ControlParameter ControlID="ANO" DefaultValue="-1" Name="ANO"
                                                PropertyName="SelectedValue" Type="String" />--%>
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br />
                <div id="botao">
                    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" Width="80px" />
                    <asp:Button ID="btnAprovar" runat="server" Text="Aprovar" Width="80px" OnClientClick="return(confirm('Confirma a aprova��o dos objetivos selecionados \n e recusa dos objetivos n�o selecionados?'))"
                        OnClick="btnAprovar_Click" />
                </div>
            </asp:Panel>
            <br />
            <div id="topo">
                LISTAGEM</div>
            <div id="grid">
                <asp:GridView ID="grvPrincipal" runat="server" AllowPaging="false" PageSize="25"
                    AllowSorting="false" AutoGenerateColumns="False" OnRowDataBound="grvPrincipal_RowDataBound"
                    DataKeyNames="ID_OBJETIVO" DataSourceID="odsGrvPrincipal" EnableViewState="False"
                    Width="100%" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="15px" VerticalAlign="Top"></HeaderStyle>
                            <ItemStyle Width="15px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderTemplate>
                                <input id="ckbHeader" type="checkbox" runat="server" name="ckbHeader" checked="false"
                                    onclick="JavaScript:MarcarTodos(this);">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSel" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NOME" HeaderText="Nome do Indicador" SortExpression="NOME">
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SIGLA" HeaderText="Sigla" SortExpression="SIGLA">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LIMITE_MIN" HeaderText="Limite M�nimo" SortExpression="LIMITE_MIN">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LIMITE_MAX" HeaderText="Limite M�ximo" SortExpression="LIMITE_MAX">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PONTUACAO" HeaderText="Pondera��o" SortExpression="PONTUACAO">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                            <FooterStyle HorizontalAlign="Center" VerticalAlign="Top" />                            
                        </asp:BoundField>
                        <asp:BoundField DataField="DATA_APROVACAO" HeaderText="Aprova��o do Executivo" SortExpression="DATA_APROVACAO">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                        </asp:BoundField>    
                        <asp:BoundField DataField="RECUSADO_SUP" HeaderText="RECUSADO" SortExpression="RECUSADO_SUP">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="80px" />
                        </asp:BoundField>                                                                   
                    </Columns>
                    <EmptyDataTemplate>
                        Nenhum registro foi encontrado!
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <asp:ObjectDataSource ID="odsGrvPrincipal" runat="server" SelectMethod="ListarOBJETIVOParaAprovacaoExecutivo"
                    TypeName="GPO.DsAdmin.OBJETIVO" EnableViewState="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ID_EXECUTIVO" Name="ID_EXECUTIVO" Type="String" PropertyName="SelectedValue" DefaultValue="-1" />
                        <asp:ControlParameter ControlID="ANO" Name="ANO" Type="String" PropertyName="SelectedValue" DefaultValue="-1" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
