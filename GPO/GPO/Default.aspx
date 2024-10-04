<%@ Page Language="C#" MasterPageFile="~/masterpages/Externo.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        Autenticação</h1>
    <br />
    <div id="block">
        &nbsp;<asp:Label ID="lbLoginNome" runat="server" Text="Nome:"></asp:Label>
        &nbsp;&nbsp;<asp:TextBox ID="txtLoginNome" runat="server" Width="130px" MaxLength="20"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginNome"
            ErrorMessage="Informe o Login" ForeColor="#39509E">*</asp:RequiredFieldValidator>
        &nbsp;&nbsp;&nbsp;<asp:Label ID="lbLoginSenha" runat="server" Text="Senha: "></asp:Label>
        <asp:TextBox ID="txtLoginSenha" runat="server" TextMode="Password" Width="130px"
            MaxLength="20"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLoginSenha"
            ErrorMessage="Informe a Senha" ForeColor="#39509E">*</asp:RequiredFieldValidator>
        &nbsp;&nbsp;&nbsp;<asp:Label ID="lbLoginDominio" runat="server" Text="Domínio:"></asp:Label>
        <asp:DropDownList ID="DRE_COD" runat="server">
            <asp:ListItem Text="Selecione" Value="-1" Selected="True"></asp:ListItem>
            <asp:ListItem Text="CELPE" Value="3"></asp:ListItem>
            <asp:ListItem Text="WNT_ADM" Value="1"></asp:ListItem>
            <asp:ListItem Text="WNT_ADM_RN" Value="2"></asp:ListItem>
            <asp:ListItem Text="HOLDINGNET.INT" Value="4"></asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DRE_COD"
            ErrorMessage="Selecione o Domínio" InitialValue="-1" ForeColor="#39509E">*</asp:RequiredFieldValidator>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btLoginOK" runat="server" Text="OK" Width="60px" OnClick="btLoginOK_Click" />
    </div>
</asp:Content>
