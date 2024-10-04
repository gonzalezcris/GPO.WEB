Imports System.Web.UI.WebControls
Imports System.Text

Public MustInherit Class Funcoes

#Region "ScriptConfirmarExclusao"
    Public Shared Function ScriptConfirmarExclusao() As String
        Dim stringRetorno As String

        stringRetorno = ScriptConfirmar("Exclusão", "Exclusao")

        Return stringRetorno
    End Function

    Public Shared Function ScriptConfirmar(ByVal Mensagem As String) As String
        Dim stringRetorno As String

        stringRetorno = ScriptConfirmar(Mensagem, Mensagem)

        Return stringRetorno
    End Function

    Public Shared Function ScriptConfirmar(ByVal Mensagem As String, ByVal Funcao As String) As String
        Dim stringRetorno As New StringBuilder
        stringRetorno.Append("<script language='javascript'>")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("<!--")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("function Confirmar" & Funcao & "()")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("{")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("if (confirm('Confirma " & Mensagem & "?')==true) ")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("return true;")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("else")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("return false; ")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("}")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("// -->")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("</script>")
        Return stringRetorno.ToString
    End Function

    Public Shared Function ScriptConfirmarParametro(ByVal Funcao As String) As String
        Dim stringRetorno As New StringBuilder
        stringRetorno.Append("<script language='javascript'>")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("<!--")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("function Confirmar" & Funcao & "(StrExclusao)")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("{")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("if (confirm('Confirma Exclusão ' + StrExclusao + '?')==true) ")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("return true;")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("else")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("return false; ")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("}")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("// -->")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("</script>")
        Return stringRetorno.ToString
    End Function
#End Region

#Region "Alerta"
    Public Shared Sub Alerta(ByRef Pagina As System.Web.UI.Page, ByVal desMensagem As String)
        Dim stringRetorno As New StringBuilder
        stringRetorno.Append("<script language='javascript'>")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("<!--")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("alert('" & desMensagem & "');")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("// -->")
        stringRetorno.Append(vbCrLf)
        stringRetorno.Append("</script>")
        Pagina.ClientScript.RegisterStartupScript(Pagina.GetType, "Inicio", stringRetorno.ToString)
    End Sub
#End Region

#Region "dtgItemDataBoundExclusao"
    Public Shared Sub dtgItemDataBoundExclusao(ByRef Objeto As System.Web.UI.WebControls.DataGridItemEventArgs, ByVal StringMensagem As String)
        Dim l As Object
        If Objeto.Item.ItemType = ListItemType.Item Or _
            Objeto.Item.ItemType = ListItemType.AlternatingItem Then
            l = Objeto.Item.Cells(0).FindControl("btnDelete")
            l.Attributes.Add("onclick", "return confirm('" & StringMensagem & "');")
        End If
    End Sub

    Public Shared Sub dtgItemDataBoundExclusao(ByRef Objeto As System.Web.UI.WebControls.DataGridItemEventArgs)
        Dim l As Object
        If Objeto.Item.ItemType = ListItemType.Item Or _
            Objeto.Item.ItemType = ListItemType.AlternatingItem Then
            l = Objeto.Item.Cells(0).FindControl("btnDelete")
            l.Attributes.Add("onclick", "return confirm('Confirma Exclusão?');")
        End If
    End Sub
#End Region

#Region "RegistraEvento"
    Public Enum Evento
        onkeypress
        onchange
        onblur
    End Enum

    Public Enum Metodo
        DropDownKeyPressBusca
        TextBoxKeyPressFormata
    End Enum

    Public Shared Sub RegistraEvento(ByRef Controle As Object, ByVal EnumEvento As Evento, ByVal EnumMetodo As Metodo)
        Dim strScript As New StringBuilder
        If (Not Controle.Page.ClientScript.IsClientScriptBlockRegistered([Enum].GetName(GetType(Metodo), EnumMetodo))) Then
            If EnumMetodo = Metodo.TextBoxKeyPressFormata Then
                Script.TextBoxKeyPressFormata(strScript)
            Else
                Script.DropDownKeyPressBusca(strScript)
            End If
            Controle.Page.ClientScript.RegisterClientScriptBlock(Controle.Page.GetType, [Enum].GetName(GetType(Metodo), EnumMetodo), "<SCRIPT LANGUAGE=JavaScript>" & vbCrLf & strScript.ToString & vbCrLf & "</SCRIPT>")
        End If
        Controle.Attributes.Add([Enum].GetName(GetType(Evento), EnumEvento), "return " & [Enum].GetName(GetType(Metodo), EnumMetodo) & "()")
    End Sub

    Public Shared Sub RegistraEvento(ByRef Controle As Object, ByVal EnumEvento As Evento, ByVal EnumMetodo As Metodo, ByVal Parametros As String)
        Dim strScript As New StringBuilder
        If EnumMetodo = Metodo.TextBoxKeyPressFormata Then
            If (Not Controle.Page.ClientScript.IsClientScriptBlockRegistered("INSIX.FW.WebControls.Comum.Formata")) Then
                Script.TextBoxKeyPressFormata(strScript)
                Controle.Page.ClientScript.RegisterClientScriptBlock(Controle.Page.GetType, "INSIX.FW.WebControls.Comum.Formata", "<SCRIPT LANGUAGE=JavaScript>" & vbCrLf & strScript.ToString & vbCrLf & "</SCRIPT>")
            End If
        Else
            If (Not Controle.Page.ClientScript.IsClientScriptBlockRegistered([Enum].GetName(GetType(Metodo), EnumMetodo))) Then
                Script.DropDownKeyPressBusca(strScript)
                Controle.Page.ClientScript.RegisterClientScriptBlock(Controle.Page.GetType, [Enum].GetName(GetType(Metodo), EnumMetodo), "<SCRIPT LANGUAGE=JavaScript>" & vbCrLf & strScript.ToString & vbCrLf & "</SCRIPT>")
            End If
        End If
        Controle.Attributes.Add([Enum].GetName(GetType(Evento), EnumEvento), "return " & [Enum].GetName(GetType(Metodo), EnumMetodo) & "('" & Parametros & "')")
    End Sub
#End Region

#Region " UpdateProgress "
    Public Shared Function AtribuiUpdateProgress() As Literal
        Dim lt As New Literal
        Dim st As New StringBuilder
        If System.Web.HttpContext.Current.Request.Browser.Browser.ToUpper.IndexOf("IE") <> -1 AndAlso _
            System.Web.HttpContext.Current.Request.Browser.MajorVersion = 6 Then
            st.Append("<div id=""object1"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px; background-color: gray; filter: alpha(opacity=30); -moz-opacity: 0.3; opacity: 0.3;"">")
            st.Append("<div id=""object3""></div>")
            st.Append("</div>")
            st.Append("<div id=""object2"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px;"">")
            st.Append("<img src=""../img/aguarde.gif"" border=""0"" alt=""Aguarde"" style=""position: absolute; left: 50%; top: 50%; margin-left: 0px; margin-top: 0px;"" />")
            st.Append("</div>")
            st.Append("<script>")
            st.Append("object3.style.width=screen.availWidth;")
            st.Append("object3.style.height=screen.availHeight;")
            st.Append("object3.style.backgroundColor=""Transparent"";")
            st.Append("object2.style.top=""-75px"";")
            st.Append("object2.style.left=""-100px"";")
            st.Append("</script>")
        Else
            st.Append("<div id=""object1"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px; background-color: gray; filter: alpha(opacity=30); -moz-opacity: 0.3; opacity: 0.3;"">")
            st.Append("</div>")
            st.Append("<div id=""object2"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px;"">")
            st.Append("<img src=""../img/aguarde.gif"" border=""0"" alt=""Aguarde"" style=""position: absolute; left: 50%; top: 50%; margin-left: -100px; margin-top: -75px;"" />")
            st.Append("</div>")

        End If
        lt.Text = st.ToString()
        Return lt
    End Function

    Public Shared Function AtribuiUpdateProgress(ByVal PathImage As String) As Literal
        Dim lt As New Literal
        Dim st As New StringBuilder
        If System.Web.HttpContext.Current.Request.Browser.Browser.ToUpper.IndexOf("IE") <> -1 AndAlso _
            System.Web.HttpContext.Current.Request.Browser.MajorVersion = 6 Then
            st.Append("<div id=""object1"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px; background-color: gray; filter: alpha(opacity=30); -moz-opacity: 0.3; opacity: 0.3;"">")
            st.Append("<div id=""object3""></div>")
            st.Append("</div>")
            st.Append("<div id=""object2"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px;"">")
            st.Append("<img src=""" & PathImage & """ border=""0"" alt=""Aguarde"" style=""position: absolute; left: 50%; top: 50%; margin-left: 0px; margin-top: 0px;"" />")
            st.Append("</div>")
            st.Append("<script>")
            st.Append("object3.style.width=screen.availWidth;")
            st.Append("object3.style.height=screen.availHeight;")
            st.Append("object3.style.backgroundColor=""Transparent"";")
            st.Append("object2.style.top=""-75px"";")
            st.Append("object2.style.left=""-100px"";")
            st.Append("</script>")
        Else
            st.Append("<div id=""object1"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px; background-color: gray; filter: alpha(opacity=30); -moz-opacity: 0.3; opacity: 0.3;"">")
            st.Append("</div>")
            st.Append("<div id=""object2"" style=""position: absolute; width: 100%; height: 100%; top: 0px; left: 0px;"">")
            st.Append("<img src=""" & PathImage & """ border=""0"" alt=""Aguarde"" style=""position: absolute; left: 50%; top: 50%; margin-left: -100px; margin-top: -75px;"" />")
            st.Append("</div>")

        End If
        lt.Text = st.ToString()
        Return lt
    End Function

#End Region

#Region "FuncoesJS"
    Public Shared Sub FuncoesJS(ByRef Pagina As System.Web.UI.Page)
        Dim CaminhoJS As String = IIf(Pagina.Request.ApplicationPath() = "/", "/js/insix.js", Pagina.Request.ApplicationPath() & "/js/insix.js")
        Pagina.ClientScript.RegisterClientScriptBlock(Pagina.GetType, "KeyPressEventHandler", "<script language=""javascript"" type=""text/javascript"" src=""" & CaminhoJS & """></script>")
    End Sub
#End Region

End Class