Imports System.Configuration

Public Class INSIXException
    Inherits ApplicationException

    Public Overrides ReadOnly Property StackTrace() As String
        Get
            Return MyBase.InnerException.StackTrace
        End Get
    End Property

    Public Overrides ReadOnly Property Message() As String
        Get
            Return MyBase.Message
        End Get
    End Property

    Sub New(ByVal Message As String, ByVal innerException As Exception)
        MyBase.New(Message, innerException)
    End Sub
End Class

Public Class TrataErro
    Private dtsErro As New dsErro
    Private Erro As Exception, retErro As Exception
    Private ArquivoErro As String
    Private Separador As String
    Private keyErro As String
    Private iKeyErro, fKeyErro As Integer

    Private lTipoErro As TipoExcecao
    Public Enum TipoExcecao
        Tratado
        NaoTratado
    End Enum

    Public ReadOnly Property TipoErro() As Integer
        Get
            Return lTipoErro
        End Get
    End Property

    Public ReadOnly Property Excecao() As Exception
        Get
            Return retErro
        End Get
    End Property

    Public Sub New(ByVal Erro As Exception, ByVal ArquivoErro As String)
        Separador = Configurationmanager.AppSettings("Separador")
        If Separador = "" Then
            Separador = "'"
        End If
        Me.Erro = Erro
        Me.ArquivoErro = ArquivoErro
        Try
            dtsErro.ReadXml(Me.ArquivoErro)
        Catch ex As Exception

        End Try

        VerificaExcecao()
    End Sub

    'Private Sub VerificaPK()
    '    Dim StringPK As String
    '    StringPK = ConfigurationSettings.AppSettings(StringPK)
    '    If StringPK = "" Then
    '        StringPK = "PKY"
    '    End If
    '    If Erro.Message.ToUpper.IndexOf(StringPK) <> -1 Then
    '        iKeyErro = Erro.Message.ToUpper.IndexOf(StringPK)
    '        fKeyErro = Erro.Message.ToUpper.IndexOf(Separador, iKeyErro)
    '        keyErro = Erro.Message.Substring(iKeyErro, fKeyErro - iKeyErro)
    '    End If
    'End Sub

    'Private Sub VerificaFK()
    '    Dim StringPK As String
    '    StringPK = ConfigurationSettings.AppSettings(StringPK)
    '    If StringPK = "" Then
    '        StringPK = "FKY"
    '    End If
    '    If Erro.Message.ToUpper.IndexOf(StringPK) <> -1 Then
    '        iKeyErro = Erro.Message.ToUpper.IndexOf(StringPK)
    '        fKeyErro = Erro.Message.ToUpper.IndexOf(Separador, iKeyErro)
    '        keyErro = Erro.Message.Substring(iKeyErro, fKeyErro - iKeyErro)
    '    End If
    'End Sub

    'Private Sub VerificaAcessoNegado()
    '    If Erro.Message.IndexOf("Acesso negado!") <> -1 Then
    '        keyErro = "Acesso negado!"
    '    End If
    'End Sub

    Private Sub VerificaExcecao()
        For Each lRow As DataRow In dtsErro.TbErros.Rows
            If Erro.Message.IndexOf(lRow("keyErro")) <> -1 Then
                retErro = New INSIXException(lRow("desErro"), Erro)
                lTipoErro = TipoExcecao.Tratado
                Exit Sub
            End If
        Next
        retErro = Erro
        lTipoErro = TipoExcecao.NaoTratado
    End Sub
End Class