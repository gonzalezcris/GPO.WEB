Imports System.Configuration
Imports System.Data
Imports System.Data.Common
Imports System.Data.OracleClient
Imports System.IO

Public MustInherit Class dsBase
    Implements IDisposable

    Public Overloads Sub Dispose() Implements System.IDisposable.Dispose
        Dim tc As TransactionContext = TransactionManager.GetTransactionContext(_originalMethodBackOffset)
        If (tc Is Nothing) Then
            If Not lTransacao Is Nothing Then
                If Not lTransacao.Connection Is Nothing Then
                    Commit()
                End If
            End If
            If Not lConexao Is Nothing Then
                lConexao.Close()
                lConexao.Dispose()
            End If
        End If
        GC.SuppressFinalize(Me)
    End Sub

#Region "Enum"
    Protected Enum ThreeValueBool
        Unspecified = 0
        Falso
        Verdadeiro
    End Enum
    Public Enum tipoBanco
        SqlDB = 1
        OleDB = 2
        OraDB = 3
        MySql = 4
    End Enum
    Public Enum tipoAcessoBase
        WebConfig
        Conexao
        ConexaoTransacional
    End Enum
#End Region

#Region "Variaveis de Transacao"
    Private _originalMethodBackOffset As Integer = 2
    Private _transactionDetails As TransactionDetails = Nothing
#End Region

#Region "Propriedades"
    Private lTipoBanco As tipoBanco
    Private lConexao As IDbConnection
    Private lTransacao As IDbTransaction
    Private lStringConexao As String
    Private lTipoAcessoBase As tipoAcessoBase
    Private lBanco As Object

    Public WriteOnly Property TipoConexao(ByVal tipoAcesso As tipoAcessoBase) As tipoAcessoBase
        Set(ByVal Value As tipoAcessoBase)
            lTipoAcessoBase = Value
        End Set
    End Property

    Public Property StringConexao() As String
        Get
            Return lStringConexao
        End Get
        Set(ByVal Value As String)
            lStringConexao = Value
        End Set
    End Property

    Public Property tipoObjBanco() As tipoBanco
        Get
            Return lTipoBanco
        End Get
        Set(ByVal Value As tipoBanco)
            lTipoBanco = Value
            InicializaBanco()
            CriaConexao()
        End Set
    End Property

    Public Property Conexao() As DbConnection
        Get
            Return lConexao
        End Get
        Set(ByVal Value As DbConnection)
            lConexao = Value
        End Set
    End Property

    Public WriteOnly Property Transacao() As DbTransaction
        Set(ByVal Value As DbTransaction)
            lTransacao = Value
            lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional
        End Set
    End Property
#End Region

#Region "New"
    Private Sub InicializaBanco()
        If lTipoBanco = tipoBanco.OleDB Then
            'lBanco = New OLEDB
        ElseIf lTipoBanco = tipoBanco.SqlDB Then
            'lBanco = New SQLDB
        ElseIf lTipoBanco = tipoBanco.OraDB Then
            lBanco = New OraDb
            'ElseIf lTipoBanco = tipoBanco.MySql Then
            '    lBanco = New MySql
        Else
            Throw New ApplicationException("Tipo de Banco Inválido: " & lTipoBanco & "!")
        End If
        If lStringConexao = String.Empty Then
            lStringConexao = lBanco.StringConexao
        End If
    End Sub

    Public Sub New()
        lTipoBanco = ConfigurationManager.AppSettings("tipoBanco")
        InicializaBanco()
    End Sub

    Public Sub New(ByVal tipoObjBanco As tipoBanco, ByVal StringConexao As String)
        lTipoBanco = tipoObjBanco
        InicializaBanco()
        lStringConexao = StringConexao
    End Sub
#End Region

#Region "Log"

    Private Sub VerificaLog(ByVal cmdDados As IDbCommand)
        'LogManager.RegistraLog(_originalMethodBackOffset, cmdDados, lUsuario)
        If Not System.Web.HttpContext.Current Is Nothing Then
            If Not System.Web.HttpContext.Current.Session Is Nothing Then
                LogManager.RegistraLog(_originalMethodBackOffset, cmdDados, System.Web.HttpContext.Current.Session("cod_usuario_usu"), lTipoBanco, lStringConexao)
            End If
        End If
    End Sub

#End Region

#Region "Transaction"

    Private Sub VerificaContexto()
        Dim tc As TransactionContext = TransactionManager.GetTransactionContext(_originalMethodBackOffset)
        If (Not tc Is Nothing) Then
            _transactionDetails = tc.GetTransactionDetails(Me)
            lConexao = _transactionDetails.Connection
            lTransacao = _transactionDetails.Transaction
            lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional
        ElseIf lConexao Is Nothing Then
            CriaConexao()
        ElseIf lConexao.State = ConnectionState.Closed Then
            CriaConexao()
        End If
    End Sub

    Public Function CriaConexao() As IDbConnection
        If Not lConexao Is Nothing Then
            If lConexao.State = ConnectionState.Closed Then
                lConexao.Dispose()
            ElseIf lTipoAcessoBase <> tipoAcessoBase.ConexaoTransacional Then
                lConexao.Dispose()
            End If
        End If
        If lStringConexao = String.Empty Then
            lConexao = lBanco.CriaConexao()
        Else
            lConexao = lBanco.CriaConexao(lStringConexao)
        End If
        lStringConexao = lConexao.ConnectionString
        lTipoAcessoBase = tipoAcessoBase.Conexao
        Return lConexao
    End Function

    Public Function IniciaTransacao() As Object
        If lTransacao Is Nothing Then
            lTransacao = lBanco.IniciaTransacao(lConexao)
        ElseIf lTransacao.Connection Is Nothing Then
            lTransacao = lBanco.IniciaTransacao(lConexao)
        End If
        lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional
        Return lTransacao
    End Function

    Public Sub Commit()
        lTransacao.Commit()
        lTipoAcessoBase = tipoAcessoBase.Conexao
        lTransacao.Dispose()
        lConexao.Dispose()
    End Sub

    Public Sub Rollback()
        Dim tc As TransactionContext = TransactionManager.GetTransactionContext(_originalMethodBackOffset)
        If (tc Is Nothing) Then
            If lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
                lTransacao.Rollback()
                lTipoAcessoBase = tipoAcessoBase.Conexao
                lTransacao.Dispose()
            End If
        End If
    End Sub
#End Region

#Region "Query's"
    Protected Function ConsultaQueryDataSet(ByVal StringConsulta As String, ByVal Tabela As String) As DataSet
        Dim lDataSet As DataSet = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lDataSet = lBanco.PreencheDataSet(StringConsulta, Tabela, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lDataSet = lBanco.PreencheDataSet(StringConsulta, Tabela, lConexao, lTransacao)
        End If

        Return lDataSet
    End Function

    Protected Function ConsultaQueryDataSet(ByVal Command As IDbCommand, ByVal Tabela As String) As DataSet
        Dim lDataSet As DataSet
        VerificaContexto()
        lDataSet = lBanco.PreencheDataSet(Command, Tabela)
        Return lDataSet
    End Function

    Protected Function ConsultaQueryDataReader(ByVal Command As IDbCommand) As IDataReader
        Dim lDataReader As IDataReader
        VerificaContexto()
        lDataReader = lBanco.ExecutaQuery(Command)
        Return lDataReader
    End Function

    Protected Function ConsultaQueryDataReader(ByVal StringConsulta As String) As IDataReader
        Dim lDataReader As IDataReader = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lDataReader = lBanco.ExecutaQuery(StringConsulta, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lDataReader = lBanco.ExecutaQuery(StringConsulta, lConexao, lTransacao)
        End If
        Return lDataReader
    End Function

    Protected Function ExecutaNonQuery(ByVal Command As IDbCommand) As Integer
        Dim qtdQuery As Integer
        VerificaContexto()
        Try
            If Command.CommandText.IndexOf("Excluir") <> -1 Then
                VerificaLog(Command)
                qtdQuery = lBanco.ExecutaNonQuery(Command)
            Else
                qtdQuery = lBanco.ExecutaNonQuery(Command)
                VerificaLog(Command)
            End If
        Catch ex As Exception
            Rollback()
            Throw ex
        End Try
        Return qtdQuery
    End Function

    'Somente para Conexões SQL
    Protected Function ExecutaNonQuery(ByVal Procedure As String, ByVal Tabela As DataTable) As Integer
        Dim qtdQuery As Integer
        Try
            _originalMethodBackOffset += 1
            Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
            lCommand = InicializaProcedure(Procedure)

            For Each lRow As DataRow In Tabela.Rows
                For Each lParameter In lCommand.Parameters
                    lParameter.Value = Tabela.Rows(0)(Mid(lParameter.ParameterName(), 2))
                Next
                qtdQuery = ExecutaNonQuery(lCommand)
                lCommand.Dispose()
            Next
        Finally
            _originalMethodBackOffset -= 1
        End Try
        Return qtdQuery
    End Function

    Protected Sub PreencheParameter(ByRef Command As IDbCommand, ByVal Linha As DataRow)
        Dim lParameter As IDataParameter
        For Each lParameter In Command.Parameters
            If lParameter.Direction = ParameterDirection.Input Or lParameter.Direction = ParameterDirection.InputOutput Then
                lParameter.Value = Linha(Mid(lParameter.ParameterName(), 2))
            End If
        Next
    End Sub

    Protected Function ExecutaNonQuery(ByVal StringQuery As String) As Integer
        Dim qtdQuery As Integer
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            qtdQuery = lBanco.ExecutaNonQuery(StringQuery, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            Try
                qtdQuery = lBanco.ExecutaNonQuery(StringQuery, lConexao, lTransacao)
            Catch ex As Exception
                Rollback()
                Throw ex
            End Try
        End If
        Return qtdQuery
    End Function

    Protected Function ExecutaScalar(ByVal StringQuery As String) As Object
        Dim lRetorno As Object = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lRetorno = lBanco.ExecutaScalar(StringQuery, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            Try
                lRetorno = lBanco.ExecutaScalar(StringQuery, lConexao, lTransacao)
            Catch ex As Exception
                Rollback()
                Throw ex
            End Try
        End If
        Return lRetorno
    End Function

    Protected Function ExecutaScalar(ByVal Command As IDbCommand) As Object
        Dim lRetorno As Object
        Try
            lRetorno = lBanco.ExecutaScalar(Command)
        Catch ex As Exception
            Rollback()
            Throw ex
        End Try
        Return lRetorno
    End Function

    Protected Function InicializaProcedure(ByVal StoredProcedure As String) As IDbCommand
        Dim lCommand As IDbCommand = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lCommand = lBanco.CriaCommand(StoredProcedure, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lCommand = lBanco.CriaCommand(StoredProcedure, lConexao, lTransacao)
        End If
        lCommand.CommandType = CommandType.StoredProcedure
        If lTipoBanco = tipoBanco.SqlDB Then
            If lTipoAcessoBase = tipoAcessoBase.Conexao Then
                lCommand = lBanco.InicializaParametros(lCommand, lConexao)
            ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
                lCommand = lBanco.InicializaParametros(lCommand, lConexao, lTransacao)
            End If
        End If
        Return lCommand
    End Function

    Protected Function InicializaCommand(ByVal StringSQL As String) As IDbCommand
        Dim lCommand As IDbCommand = Nothing
        VerificaContexto()

        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lCommand = lBanco.CriaCommand(StringSQL, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lCommand = lBanco.CriaCommand(StringSQL, lConexao, lTransacao)
        End If

        Return lCommand
    End Function
#End Region

#Region "Funções de SQL-Server"
    ''' -----------------------------------------------------------------------------
    ''' <summary>Consulta tabela</summary>
    ''' <param name="Tabela">Nome da tabela</param>
    ''' <returns>DataReader com dados da tabela</returns>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function ConsultarTabela(ByVal Tabela As String) As IDataReader
        Dim lDataReader As IDataReader = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lDataReader = lBanco.ConsultarTabela(Tabela, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lDataReader = lBanco.ConsultarTabela(Tabela, lConexao, lTransacao)
        End If
        Return lDataReader
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>Obtem estrutura da tabela</summary>
    ''' <param name="Tabela">Nome da tabela</param>
    ''' <returns>DataSet com estrutura da tabela</returns>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function ObterEstrutura(ByVal Tabela As String) As DataSet
        Dim lDataSet As DataSet = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lDataSet = lBanco.ObterEstrutura(Tabela, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lDataSet = lBanco.ObterEstrutura(Tabela, lConexao, lTransacao)
        End If
        Return lDataSet
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>Lista de primary keys de uma tabela</summary>
    ''' <param name="Tabela">Nome da tabela</param>
    ''' <returns>Array de strings</returns>
    ''' <remarks>Somente disponível para SQL Server
    ''' </remarks>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function ObterPrimaryKey(ByVal Tabela As String) As String()
        Dim lIndices As String() = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            lIndices = lBanco.ObterPrimaryKey(Tabela, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            lIndices = lBanco.ObterPrimaryKey(Tabela, lConexao, lTransacao)
        End If
        Return lIndices
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>Lista de foreign keys de uma tabela</summary>
    ''' <param name="Tabela">Nome da tabela</param>
    ''' <returns>Array de strings</returns>
    ''' <remarks>Somente disponível para SQL Server
    ''' </remarks>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function ObterForeignKey(ByVal Tabela As String) As DataView
        Dim dvFK As DataView = Nothing
        VerificaContexto()
        If lTipoAcessoBase = tipoAcessoBase.Conexao Then
            dvFK = lBanco.ObterForeignKey(Tabela, lConexao)
        ElseIf lTipoAcessoBase = tipoAcessoBase.ConexaoTransacional Then
            dvFK = lBanco.ObterForeignKey(Tabela, lConexao, lTransacao)
        End If
        Return dvFK
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>Lista os dados de uma tabela</summary>
    ''' <param name="Tabela"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function Listar(ByVal Tabela As String) As DataSet
        Dim lCommand As SqlClient.SqlCommand
        Dim lDataSet As DataSet, lStrAux As String(), lNomeTabela As String = Nothing
        lStrAux = Tabela.Split("_")
        For i As Integer = 2 To lStrAux.Length - 1
            lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
        Next
        lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Listar" & lNomeTabela)
        lDataSet = ConsultaQueryDataSet(lCommand, Tabela)
        Return lDataSet
    End Function

    Protected Function Listar(ByVal Tabela As DataTable) As DataSet
        Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
        Dim lDataSet As DataSet, lStrAux As String(), lNomeTabela As String = Nothing
        lStrAux = Tabela.TableName.Split("_")
        For i As Integer = 2 To lStrAux.Length - 1
            lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
        Next
        lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Listar" & lNomeTabela)
        For Each lParameter In lCommand.Parameters
            lParameter.Value = Tabela.Rows(0)(Mid(lParameter.ParameterName(), 2))
        Next
        lDataSet = ConsultaQueryDataSet(lCommand, Tabela.TableName)
        Return lDataSet
    End Function

    Protected Function Consultar(ByVal Tabela As DataTable) As DataSet
        Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
        Dim lDataSet As DataSet, lStrAux As String(), lNomeTabela As String = Nothing
        lStrAux = Tabela.TableName.Split("_")
        For i As Integer = 2 To lStrAux.Length - 1
            lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
        Next
        lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Consultar" & lNomeTabela)
        For Each lParameter In lCommand.Parameters
            lParameter.Value = Tabela.Rows(0)(Mid(lParameter.ParameterName(), 2))
        Next
        lDataSet = ConsultaQueryDataSet(lCommand, Tabela.TableName)
        Return lDataSet
    End Function

    Protected Function Incluir(ByVal Tabela As DataTable) As Integer
        Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
        Dim lStrAux As String(), lNomeTabela As String = Nothing
        Dim qtdQuery As Integer = 0
        Try
            _originalMethodBackOffset += 1
            lStrAux = Tabela.TableName.Split("_")
            For i As Integer = 2 To lStrAux.Length - 1
                lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
            Next
            For Each lRow As DataRow In Tabela.Rows
                lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Incluir" & lNomeTabela)
                For Each lParameter In lCommand.Parameters
                    lParameter.Value = lRow(Mid(lParameter.ParameterName(), 2))
                Next
                qtdQuery = ExecutaNonQuery(lCommand)
                lCommand.Dispose()
            Next
        Finally
            _originalMethodBackOffset -= 1
        End Try
        Return qtdQuery
    End Function

    Protected Function Alterar(ByVal Tabela As DataTable) As Integer
        Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
        Dim lStrAux As String(), lNomeTabela As String = Nothing
        Dim qtdQuery As Integer = 0
        Try
            _originalMethodBackOffset += 1
            lStrAux = Tabela.TableName.Split("_")
            For i As Integer = 2 To lStrAux.Length - 1
                lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
            Next
            For Each lRow As DataRow In Tabela.Rows
                lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Alterar" & lNomeTabela)
                For Each lParameter In lCommand.Parameters
                    lParameter.Value = lRow(Mid(lParameter.ParameterName(), 2))
                Next
                qtdQuery = ExecutaNonQuery(lCommand)
                lCommand.Dispose()
            Next
        Finally
            _originalMethodBackOffset -= 1
        End Try
        Return qtdQuery
    End Function

    Protected Function Excluir(ByVal Tabela As DataTable) As Integer
        Dim lCommand As SqlClient.SqlCommand, lParameter As SqlClient.SqlParameter
        Dim lStrAux As String(), lNomeTabela As String = Nothing
        Dim qtdQuery As Integer = 0
        Try
            _originalMethodBackOffset += 1
            lStrAux = Tabela.TableName.Split("_")
            For i As Integer = 2 To lStrAux.Length - 1
                lNomeTabela &= UCase(Mid(lStrAux(i), 1, 1)) & Mid(lStrAux(i), 2)
            Next
            For Each lRow As DataRow In Tabela.Rows
                lCommand = InicializaProcedure("USP_" & lStrAux(1) & "_Excluir" & lNomeTabela)
                For Each lParameter In lCommand.Parameters
                    lParameter.Value = lRow(Mid(lParameter.ParameterName(), 2))
                Next
                qtdQuery = ExecutaNonQuery(lCommand)
                lCommand.Dispose()
            Next
        Finally
            _originalMethodBackOffset -= 1
        End Try
        Return qtdQuery
    End Function
#End Region

#Region "Funções de BLOB"

    Protected Function GravarBlob(ByVal caminho As String, ByVal idArquivo As Int32, ByVal Tipo As Int32) As Integer
        'Dim conn As New OracleConnection("server=CLBD.COELBANET;Uid=gaps;pwd=coelba")
        Dim conn As New OracleConnection(lBanco.StringConexao)
        Dim filePath As String
        '        Dim bigData As Byte()
        Dim t As Date

        t = Now

        filePath = caminho
        If Not File.Exists(filePath) Then
            ' handle error
        End If

        Dim fs As Stream = _
                    File.OpenRead(filePath)
        Dim tempBuff(fs.Length) As Byte

        fs.Read(tempBuff, 0, fs.Length)
        fs.Close()
        conn.Open()

        Dim tx As OracleTransaction
        tx = conn.BeginTransaction()

        Dim cmd As New OracleCommand()
        cmd = conn.CreateCommand()

        cmd.Transaction = tx

        cmd.CommandText = "declare xx blob; begin dbms_lob.createtemporary(xx, false, 0); :tempblob := xx; end;"
        cmd.Parameters.Add(New OracleParameter("tempblob", OracleType.Blob)).Direction = ParameterDirection.Output
        cmd.ExecuteNonQuery()

        Dim tempLob As OracleLob
        tempLob = cmd.Parameters(0).Value
        tempLob.BeginBatch(OracleLobOpenMode.ReadWrite)
        tempLob.Write(tempBuff, 0, tempBuff.Length)
        tempLob.EndBatch()

        cmd.Parameters.Clear()
        cmd.CommandText = "InsertBlob.TestBlobInsert"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add(New OracleParameter("BlobParam", OracleType.Blob)).Value = tempLob
        cmd.Parameters.Add(New OracleParameter("p_CDARQUIVO", OracleType.Number)).Value = idArquivo
        cmd.Parameters.Add(New OracleParameter("p_tipo", OracleType.Number)).Value = Tipo
        Try
            cmd.ExecuteNonQuery()
        Catch myex As Exception
            MsgBox(myex.Message)
        End Try
        tx.Commit()

    End Function
#End Region

End Class