<%@ Page Language="C#" MasterPageFile="~/masterpages/Interno.master" AutoEventWireup="true"
    CodeFile="Principal.aspx.cs" Inherits="Principal" %>
        

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        &nbsp;Boas Vindas</h1>
    <br />
    Bem-vindo ao 
    <asp:Literal ID="ltAppNome" runat="server"></asp:Literal>.
    <br />
    <br />
    <div id="block">
        Utilize o menu acima para navegar entre as opções disponíveis no seu perfil.
    </div>
</asp:Content>
