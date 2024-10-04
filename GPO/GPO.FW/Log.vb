Imports System
Imports System.Diagnostics
Imports System.Reflection
Imports System.Data.Common
'Imports INSIX.FW.Banco

#Region "LogContextAttribute"
<AttributeUsage(AttributeTargets.Method Or AttributeTargets.Constructor)> _
Public Class LogContextAttribute
    Inherits Attribute
    Public Evento As String
    Public GeraErro As Boolean

    Public Sub New()
        Me.Evento = ""
    End Sub
    Public Sub New(ByVal Evento As String)
        Me.Evento = Evento
    End Sub
    Public Sub New(ByVal Evento As String, ByVal GeraErro As Boolean)
        Me.Evento = Evento
        Me.GeraErro = GeraErro
    End Sub

End Class
#End Region

#Region "LogContext class"
Friend Class LogContext
    Inherits dsBase

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal tipBanco As Int16, ByVal strCon As String)
        MyBase.New(tipBanco, strCon)
    End Sub

    <TransactionContext(Transaction.Supported, IsolationLevel.ReadCommitted)> _
    Private Function BuscarValorTabela(ByVal nom_auxiliar_tca As String, ByVal nom_campo_pes As String, ByVal val_codigo_aux As Object) As String
        Dim TabelaCampo() As String
        Dim dsTabela As DataSet
        Dim dsRetorno As DataSet

        TabelaCampo = nom_auxiliar_tca.Split(".")
        Try
            dsTabela = MyBase.ObterEstrutura(TabelaCampo(0))
            dsTabela.Tables(0).Rows(0)(nom_campo_pes.TrimEnd) = val_codigo_aux
            dsRetorno = MyBase.Consultar(dsTabela.Tables(0))

            If dsRetorno.Tables(0).Rows.Count > 0 Then
                TransactionManager.SetComplete()
                Return dsRetorno.Tables(0).Rows(0)(TabelaCampo(1).TrimEnd).ToString.TrimEnd
            Else
                Throw New INSIXLogException("Campo auxiliar não foi encontrado.")
            End If
        Catch ex As Exception
            TransactionManager.SetAbort()
            Throw ex
        End Try

    End Function

    <TransactionContext(Transaction.Supported, IsolationLevel.ReadCommitted)> _
    Friend Sub RegistraLog(ByVal des_transacao_tra As String, ByVal cmdDados As IDbCommand, ByVal _usuario As String, ByVal GeraErro As Boolean)
        Try
            Dim dtTransacao As DataTable
            Dim lCommand As DbCommand = InicializaProcedure("usp_fw_ListarCamposPorDescricao")
            Dim strLog As String = ""
            Dim strAux As String
            Dim strIdentificacao As String = ""
            Dim cod_transacao_tra As Integer

            lCommand.Parameters("@des_transacao_tra").Value = des_transacao_tra
            '*** Consulta campos da transação que estarão no log
            dtTransacao = ConsultaQueryDataSet(lCommand, "tb_fw_log").Tables("tb_fw_log")

            '*** Se tiver campos
            If dtTransacao.Rows.Count > 0 Then
                cod_transacao_tra = dtTransacao.Rows(0)("cod_transacao_tra")
                '*** Se a transação estiver ativa
                If dtTransacao.Rows(0)("flg_ativo_tra") Then
                    For Each lRow As DataRow In dtTransacao.Rows
                        With cmdDados
                            '*** Caso o campo seja parte da primary key
                            If lRow("flg_pk_tca") Then
                                strIdentificacao &= .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value.ToString() & "|"
                            Else
                                If lRow("nom_auxiliar_tca").ToString.TrimEnd = "" Then
                                    strAux = .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value.ToString()
                                Else
                                    If Not .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value Is DBNull.Value Then
                                        strAux = BuscarValorTabela(lRow("nom_auxiliar_tca"), lRow("nom_transacao_tca"), .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value.ToString())
                                    Else
                                        strAux = ""
                                    End If
                                End If
                                If strAux.Length > lRow("num_tamanho_tca") Then strAux = strAux.Substring(0, lRow("num_tamanho_tca"))
                                strAux &= Space(lRow("num_tamanho_tca") - strAux.Length)
                                strLog &= strAux
                            End If
                        End With
                    Next
                    dtTransacao.Dispose()

                    Dim LogCommand As DbCommand = InicializaProcedure("usp_fw_IncluirLog")
                    LogCommand.Parameters("@cod_transacao_tra").Value = cod_transacao_tra
                    LogCommand.Parameters("@cod_usuario_usu").Value = _usuario
                    LogCommand.Parameters("@cod_identificacao_log").Value = strIdentificacao
                    LogCommand.Parameters("@des_dados_log").Value = strLog
                    ExecutaNonQuery(LogCommand)
                End If
            Else
                If GeraErro Then
                    Throw New INSIXLogException("Evento não foi encontrado.")
                End If
            End If
            TransactionManager.SetComplete()
        Catch ex As Exception
            If GeraErro Then
                TransactionManager.SetAbort()
                Throw ex
            Else
                TransactionManager.SetComplete()
            End If
        End Try

    End Sub

    <TransactionContext(Transaction.Supported, IsolationLevel.ReadCommitted)> _
    Friend Sub RegistraLog(ByVal des_transacao_tra As String, ByVal cmdDados As IDbCommand, ByVal _usuario As String, ByVal strIdentificacao As String)
        Try
            Dim dtTransacao As DataTable
            Dim lCommand As DbCommand = InicializaProcedure("usp_fw_ListarCamposPorDescricao")
            Dim strLog As String = ""
            Dim strAux As String
            Dim cod_transacao_tra As Integer

            lCommand.Parameters("@des_transacao_tra").Value = des_transacao_tra
            '*** Consulta campos da transação que estarão no log
            dtTransacao = ConsultaQueryDataSet(lCommand, "tb_fw_log").Tables("tb_fw_log")

            '*** Se tiver campos
            If dtTransacao.Rows.Count > 0 Then
                cod_transacao_tra = dtTransacao.Rows(0)("cod_transacao_tra")
                '*** Se a transação estiver ativa
                If dtTransacao.Rows(0)("flg_ativo_tra") Then
                    For Each lRow As DataRow In dtTransacao.Rows
                        With cmdDados
                            '*** Caso o campo não seja parte da primary key
                            If Not lRow("flg_pk_tca") Then
                                If lRow("nom_auxiliar_tca").ToString.TrimEnd = "" Then
                                    strAux = .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value.ToString()
                                Else
                                    strAux = BuscarValorTabela(lRow("nom_auxiliar_tca"), lRow("nom_transacao_tca"), .Parameters("@" & lRow("nom_transacao_tca").ToString.TrimEnd).Value.ToString())
                                End If
                                If strAux.Length > lRow("num_tamanho_tca") Then strAux = strAux.Substring(0, lRow("num_tamanho_tca"))
                                strAux &= Space(lRow("num_tamanho_tca") - strAux.Length)
                                strLog &= strAux
                            End If
                        End With
                    Next
                    dtTransacao.Dispose()

                    Dim LogCommand As DbCommand = InicializaProcedure("usp_fw_IncluirLog")
                    LogCommand.Parameters("@cod_transacao_tra").Value = cod_transacao_tra
                    LogCommand.Parameters("@cod_usuario_usu").Value = _usuario
                    LogCommand.Parameters("@cod_identificacao_log").Value = strIdentificacao
                    LogCommand.Parameters("@des_dados_log").Value = strLog
                    ExecutaNonQuery(LogCommand)
                End If
            Else
                Throw New INSIXLogException("Evento não foi encontrado.")
            End If
            TransactionManager.SetComplete()
        Catch ex As Exception
            TransactionManager.SetAbort()
            Throw ex
        End Try

    End Sub

End Class
#End Region

#Region "LogManager"
Public Class LogManager

#Region "Enum"
    Public Enum tipoBanco
        SqlDB = 1
        OleDB = 2
        OraDB = 3
        MySql = 4
    End Enum
#End Region

    Friend Shared Function RegistraLog(ByVal originalMethodBackOffset As Integer, ByVal cmdDados As IDbCommand, ByVal _usuario As String, ByVal tipBanco As tipoBanco, ByVal strCon As String) As Boolean
        Dim stackTrace As stackTrace = New stackTrace, NomeLogAtributo As String
        originalMethodBackOffset += 1
        Dim prevMethodInfo As MemberInfo = stackTrace.GetFrame(originalMethodBackOffset).GetMethod()
        Dim LogCustomAttributes As Object() = prevMethodInfo.GetCustomAttributes(GetType(LogContextAttribute), False)
        Dim _LogContext As LogContext

        Dim LogAttrib As LogContextAttribute = Nothing
        If (LogCustomAttributes.Length > 0) Then
            LogAttrib = LogCustomAttributes(0)
            If LogAttrib.Evento = "" Then
                NomeLogAtributo = prevMethodInfo.Name
            Else
                NomeLogAtributo = LogAttrib.Evento
            End If

            _LogContext = New LogContext(tipBanco, strCon)
            _LogContext.RegistraLog(NomeLogAtributo, cmdDados, _usuario, LogAttrib.GeraErro)
        End If

        Return True
    End Function

    Public Shared Function RegistraLog(ByVal _usuario As String, ByVal funcao As String, ByVal cmdDados As IDbCommand) As Boolean

        Dim _LogContext As LogContext
        _LogContext = New LogContext
        _LogContext.RegistraLog(funcao, cmdDados, _usuario, True)

        Return True
    End Function
End Class
#End Region

#Region "Eventos"
Public Class INSIXLogException
    Inherits ApplicationException

    Sub New()

    End Sub

    Sub New(ByVal Mensagem As String)
        MyBase.New(Mensagem)
    End Sub
End Class
#End Region