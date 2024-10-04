<%@ page language="C#" masterpagefile="~/masterpages/Interno.master" autoeventwireup="true" inherits="Principal, App_Web_l2naqbkc" enableEventValidation="false" stylesheettheme="Padrao" %>
        

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
