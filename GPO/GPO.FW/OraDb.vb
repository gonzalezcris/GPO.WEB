Imports System.Configuration
Imports System.Data.OracleClient
Imports CELPE.CRIPTO
Imports System.Web.HttpContext


Public Class OraDb

#Region "Propriedades"
    Private lStringConexao As String
    Private objCripto As New CELPE.CRIPTO.neoSchemasOracle

    Public Property StringConexao() As String
        Get
            Return lStringConexao
        End Get
        Set(ByVal Value As String)
            lStringConexao = Value
        End Set
    End Property
#End Region

#Region "New"
    Public Sub New()
        'lStringConexao = ConfigurationManager.AppSettings("StringConexao")
        If Not Current.Session("StringConexaoCripto") Is Nothing Then
            lStringConexao = objCripto.Abrir(Convert.ToString(Current.Session("StringConexaoCripto")))
        Else
            lStringConexao = ConfigurationManager.AppSettings("StringConexao")
        End If
    End Sub
    Public Sub New(ByVal StringConexao As String)
        lStringConexao = StringConexao
    End Sub
#End Region

#Region "Transaction"
    Public Function IniciaTransacao(ByRef Conexao As OracleConnection) As OracleTransaction
        Dim lTransacao As OracleTransaction
        lTransacao = Conexao.BeginTransaction()
        Return lTransacao
    End Function
#End Region

#Region "Conexao"
    Public Function CriaConexao() As OracleConnection
        Dim Conexao As OracleConnection = CriaConexao(StringConexao)
        Return Conexao
    End Function
    Public Function CriaConexao(ByVal StringConexao As String) As OracleConnection
        Dim Conexao As New OracleConnection(StringConexao)
        Conexao.Open()
        Return Conexao
    End Function
    Public Sub FechaConexao(ByRef Conexao As OracleConnection)
        Conexao.Dispose()
    End Sub
#End Region

#Region "DataAdapter"
    Public Function CriaDataAdapter(ByVal StringSql As String, ByRef Conexao As OracleConnection) As OracleDataAdapter
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao)
        Dim lDataAdapter As OracleDataAdapter = CriaDataAdapter(lCommand)
        Return lDataAdapter
    End Function
    Public Function CriaDataAdapter(ByVal StringSql As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As OracleDataAdapter
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao, Transacao)
        Dim lDataAdapter As OracleDataAdapter = CriaDataAdapter(lCommand)
        Return lDataAdapter
    End Function
    Public Function CriaDataAdapter(ByRef Command As OracleCommand) As OracleDataAdapter
        Dim lDataAdapter As New OracleDataAdapter(Command)
        Return lDataAdapter
    End Function
#End Region

#Region "Command"
    Public Function CriaCommand(ByVal StringSql As String, ByRef Conexao As OracleConnection) As OracleCommand
        Dim lCommand As New OracleCommand(StringSql, Conexao)
        Return lCommand
    End Function
    Public Function CriaCommand(ByVal StringSql As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As OracleCommand
        Dim lCommand As New OracleCommand(StringSql, Conexao, Transacao)
        Return lCommand
    End Function
#End Region

#Region "PreencheDataSet"
    Public Function PreencheDataSet(ByRef DataAdapter As OracleDataAdapter, ByVal Tabela As String) As DataSet
        Dim lDataSet As New DataSet
        DataAdapter.Fill(lDataSet, Tabela)
        Return lDataSet
    End Function

    Public Function PreencheDataSet(ByVal StringSql As String, ByVal lTabela As String, ByRef Conexao As OracleConnection) As DataSet
        Dim lDataAdapter As OracleDataAdapter = CriaDataAdapter(StringSql, Conexao)
        Dim lDataSet As DataSet = PreencheDataSet(lDataAdapter, lTabela)
        lDataAdapter.Dispose()
        Return lDataSet
    End Function
    Public Function PreencheDataSet(ByVal StringSql As String, ByVal lTabela As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As DataSet
        Dim lDataAdapter As OracleDataAdapter = CriaDataAdapter(StringSql, Conexao, Transacao)
        Dim lDataSet As DataSet = PreencheDataSet(lDataAdapter, lTabela)
        lDataAdapter.Dispose()
        Return lDataSet
    End Function

    Public Function PreencheDataSet(ByRef Command As OracleCommand, ByVal lTabela As String) As DataSet
        Dim lDataAdapter As OracleDataAdapter = CriaDataAdapter(Command)
        Dim lDataSet As DataSet = PreencheDataSet(lDataAdapter, lTabela)
        lDataAdapter.Dispose()
        Return lDataSet
    End Function
#End Region

#Region "ExecutaCommand"
    Public Function ExecutaQuery(ByVal StringSql As String, ByRef Conexao As OracleConnection) As OracleDataReader
        Dim lDataReader As OracleDataReader
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao)
        lDataReader = lCommand.ExecuteReader()
        Return lDataReader
    End Function
    Public Function ExecutaQuery(ByVal StringSql As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As OracleDataReader
        Dim lDataReader As OracleDataReader
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao, Transacao)
        lDataReader = lCommand.ExecuteReader()
        Return lDataReader
    End Function
    Public Function ExecutaQuery(ByRef Command As OracleCommand) As OracleDataReader
        Dim lDataReader As OracleDataReader
        lDataReader = Command.ExecuteReader()
        Return lDataReader
    End Function

    Public Function ExecutaNonQuery(ByVal StringSql As String, ByRef Conexao As OracleConnection) As Integer
        Dim Retorno As Integer
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao)
        Retorno = lCommand.ExecuteNonQuery()
        Return Retorno
    End Function
    Public Function ExecutaNonQuery(ByVal StringSql As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As Integer
        Dim Retorno As Integer
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao, Transacao)
        Retorno = lCommand.ExecuteNonQuery()
        Return Retorno
    End Function

    Public Function ExecutaNonQuery(ByRef Command As OracleCommand) As Integer
        Dim Retorno As Integer
        Retorno = Command.ExecuteNonQuery()
        Return Retorno
    End Function

    Public Function ExecutaScalar(ByVal StringSql As String, ByRef Conexao As OracleConnection) As Object
        Dim Retorno As Object
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao)
        Retorno = lCommand.ExecuteScalar()
        Return Retorno
    End Function
    Public Function ExecutaScalar(ByVal StringSql As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As Object
        Dim Retorno As Object
        Dim lCommand As OracleCommand = CriaCommand(StringSql, Conexao, Transacao)
        Retorno = lCommand.ExecuteScalar()
        Return Retorno
    End Function
    Public Function ExecutaScalar(ByRef Command As OracleCommand) As Object
        Dim Retorno As Object
        Retorno = Command.ExecuteScalar()
        Return Retorno
    End Function

#End Region

#Region "Procedure"
    Public Function InicializaParametros(ByVal Command As OracleCommand, ByRef Conexao As OracleConnection) As OracleCommand
        Dim dr As OracleDataReader
        Dim scsb As New OracleClient.OracleConnectionStringBuilder(Conexao.ConnectionString)
        Dim str As String
        Dim banco As String = scsb.UserID

        banco = scsb.UserID.ToUpper
        str = "select argument_name column_name, data_type column_type, data_length length, in_out from SYS.ALL_ARGUMENTS WHERE OWNER = '" & banco & "' and object_name = '" & Command.CommandText & "'"
        dr = ExecutaQuery(str, Conexao)
        If Not dr.HasRows Then
            str = "select argument_name column_name, data_type column_type, data_length length, in_out from SYS.ALL_ARGUMENTS WHERE OWNER = '" & ConfigurationManager.AppSettings("OwnerSynonms") & "' and object_name = '" & Command.CommandText & "'"
            dr = ExecutaQuery(str, Conexao)
        End If

        Command = InicializaParametros(Command, dr)
        Return Command
    End Function
    Public Function InicializaParametros(ByVal Command As OracleCommand, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As OracleCommand
        Dim dr As OracleDataReader
        Dim scsb As New OracleClient.OracleConnectionStringBuilder(Conexao.ConnectionString)
        Dim str As String
        Dim banco As String = scsb.UserID

        banco = scsb.UserID.ToUpper
        str = "select argument_name column_name, data_type column_type, data_length length, in_out from SYS.ALL_ARGUMENTS WHERE OWNER = '" & banco & "' and object_name = '" & Command.CommandText & "'"
        dr = ExecutaQuery(str, Conexao, Transacao)
        If Not dr.HasRows Then
            str = "select argument_name column_name, data_type column_type, data_length length, in_out from SYS.ALL_ARGUMENTS WHERE OWNER = '" & ConfigurationManager.AppSettings("OwnerSynonms") & "' and object_name = '" & Command.CommandText & "'"
            dr = ExecutaQuery(str, Conexao, Transacao)
        End If

        Command = InicializaParametros(Command, dr)
        Return Command
    End Function
    Private Function InicializaParametros(ByVal Command As OracleCommand, ByRef lDataReader As OracleDataReader) As OracleCommand
        Dim OracleParametros As OracleParameterCollection = Command.Parameters
        Dim lParametro As OracleParameter
        If lDataReader.HasRows Then
            While lDataReader.Read
                lParametro = New OracleParameter
                lParametro.ParameterName = CType(lDataReader("column_name"), String)
                lParametro.OracleType = BuscaTipoSql(lDataReader("type_name"))
                lParametro.Direction = IIf(lDataReader("type_name") = "IN", ParameterDirection.Input, ParameterDirection.Output)
                If lDataReader("type_name") = "char" Or lDataReader("type_name") = "varchar" Then
                    lParametro.Size = lDataReader("length")
                End If
                OracleParametros.Add(lParametro)
                lParametro = Nothing
            End While
        End If
        lDataReader.Close()
        Return Command
    End Function
#End Region

#Region "Obter Estrutura"
    Public Function ObterEstrutura(ByVal Tabela As String, ByRef Conexao As OracleConnection) As DataSet
        Dim drTabela As OracleDataReader = ConsultarTabela(Tabela, Conexao)
        Dim lDataSet As DataSet

        lDataSet = ObterEstrutura(drTabela, Tabela)

        Return lDataSet
    End Function

    Public Function ObterEstrutura(ByVal Tabela As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As DataSet
        Dim drTabela As OracleDataReader = ConsultarTabela(Tabela, Conexao, Transacao)
        Dim lDataSet As DataSet

        lDataSet = ObterEstrutura(drTabela, Tabela)

        Return lDataSet
    End Function

    Public Function ObterEstrutura(ByVal drTabela As OracleDataReader, ByVal NomeTabela As String) As DataSet
        If drTabela.HasRows Then
            Dim dt As DataTable = New DataTable(NomeTabela)
            Dim ds As DataSet = New DataSet
            dt.Rows.Add(dt.NewRow)
            While drTabela.Read
                dt.Columns.Add(New DataColumn(drTabela("nome"), BuscaTipo(drTabela("tipo"))))
            End While
            ds.Tables.Add(dt)
            drTabela.Close()
            Return ds
        Else
            drTabela.Close()
            Return Nothing
        End If
    End Function

    Public Function ConsultarTabela(ByVal Tabela As String, ByRef Conexao As OracleConnection) As OracleDataReader
        Dim dr As OracleDataReader
        Dim scsb As New OracleClient.OracleConnectionStringBuilder(Conexao.ConnectionString)
        Dim str As String
        Dim banco As String = scsb.UserID

        banco = scsb.UserID.ToUpper
        str = "SELECT COLUMN_NAME nome, data_type tipo, data_length tamanho, nullable  FROM SYS.ALL_TAB_COLUMNS WHERE OWNER = '" & banco & "' and table_name = '" & Tabela & "'"
        dr = ExecutaQuery(str, Conexao)
        If Not dr.HasRows Then
            str = "select TABLE_OWNER from SYS.ALL_SYNONYMS WHERE OWNER = '" & banco & "' and table_name = '" & Tabela & "'"
            dr = ExecutaQuery(str, Conexao)
            While dr.Read
                str = "SELECT COLUMN_NAME nome, data_type tipo, data_length tamanho, nullable  FROM SYS.ALL_TAB_COLUMNS WHERE OWNER = '" & dr.Item(0) & "' and table_name = '" & Tabela & "'"
            End While
            dr = ExecutaQuery(str, Conexao)
        End If

        Return dr
    End Function

    Public Function ConsultarTabela(ByVal Tabela As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As OracleDataReader
        Dim dr As OracleDataReader
        Dim scsb As New OracleClient.OracleConnectionStringBuilder(Conexao.ConnectionString)
        Dim banco As String = scsb.UserID.ToUpper
        Dim str As String = "SELECT COLUMN_NAME nome, data_type tipo, data_length tamanho, nullable  FROM SYS.ALL_TAB_COLUMNS WHERE OWNER = '" & banco & "' and table_name = '" & Tabela & "'"
        dr = ExecutaQuery(str, Conexao, Transacao)
        Return dr
    End Function

    Public Function ObterPrimaryKey(ByVal Tabela As String, ByRef Conexao As OracleConnection) As String()
        Dim lSql As String
        Dim dsPrimaryKey As DataSet
        Try
            lSql = "SELECT CONST.CONSTRAINT_NAME index_name, '' index_description, COLUN.COLUMN_NAME index_keys "
            lSql += "FROM dba_CONSTRAINTS CONST, dba_cons_COLUMNS COLUN "
            lSql += "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql += "and CONST.CONSTRAINT_TYPE = 'P' "
            lSql += "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "

            dsPrimaryKey = PreencheDataSet(lSql, Tabela, Conexao)
        Catch ex As Exception
            lSql = "SELECT ALL_CONS_COLUMNS.CONSTRAINT_NAME index_name, '' index_description, ALL_CONS_COLUMNS.COLUMN_NAME index_keys "
            lSql += "FROM ALL_CONS_COLUMNS, ALL_CONSTRAINTS "
            lSql += "WHERE UPPER(ALL_CONS_COLUMNS.TABLE_NAME)= '" & Tabela & "' "
            lSql += "and ALL_CONSTRAINTS.CONSTRAINT_TYPE = 'P' "
            lSql += "AND ALL_CONS_COLUMNS.CONSTRAINT_NAME = ALL_CONSTRAINTS.CONSTRAINT_NAME "

            dsPrimaryKey = PreencheDataSet(lSql, Tabela, Conexao)
        End Try

        Dim Indice As String()
        Dim lStr As String = ""
        For Each lRow As DataRow In dsPrimaryKey.Tables(Tabela).Rows
            lStr = lStr & lRow("index_keys").ToString.Trim & ", "
        Next
        lStr = lStr.Substring(0, lStr.Length - 2)
        Indice = lStr.Split(", ")
        Return Indice
    End Function

    Public Function ObterPrimaryKey(ByVal Tabela As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As String()
        Dim lSql As String
        Dim dsPrimaryKey As DataSet

        Try
            lSql = "SELECT CONST.CONSTRAINT_NAME index_name, '' index_description, COLUN.COLUMN_NAME index_keys "
            lSql += "FROM dba_CONSTRAINTS CONST, dba_cons_COLUMNS COLUN "
            lSql += "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql += "and CONST.CONSTRAINT_TYPE = 'P' "
            lSql += "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "

            dsPrimaryKey = PreencheDataSet(lSql, Tabela, Conexao, Transacao)
        Catch ex As Exception
            lSql = ""
            lSql = "SELECT ALL_CONS_COLUMNS.CONSTRAINT_NAME index_name, '' index_description, ALL_CONS_COLUMNS.COLUMN_NAME index_keys "
            lSql += "FROM ALL_CONS_COLUMNS, ALL_CONSTRAINTS "
            lSql += "WHERE UPPER(ALL_CONS_COLUMNS.TABLE_NAME)= '" & Tabela & "' "
            lSql += "and ALL_CONSTRAINTS.CONSTRAINT_TYPE = 'P' "
            lSql += "AND ALL_CONS_COLUMNS.CONSTRAINT_NAME = ALL_CONSTRAINTS.CONSTRAINT_NAME "

            dsPrimaryKey = PreencheDataSet(lSql, Tabela, Conexao, Transacao)
        End Try


        Dim Indice As String()
        Dim lStr As String = ""
        For Each lRow As DataRow In dsPrimaryKey.Tables(Tabela).Rows
            lStr = lStr & lRow("index_keys").ToString.Trim & ", "
        Next
        lStr = lStr.Substring(0, lStr.Length - 2)
        Indice = lStr.Split(", ")
        Return Indice
    End Function

    Public Function ObterForeignKey(ByVal Tabela As String, ByRef Conexao As OracleConnection) As DataView
        Dim lSql As String = ""
        Dim dsFK As DataSet

        Try
            lSql = lSql & "SELECT CONST.OWNER PKTABLE_QUALIFIER, 'dbo' PKTABLE_OWNER, COLUN_DEST.TABLE_NAME PKTABLE_NAME, "
            lSql = lSql & "COLUN.COLUMN_NAME PKCOLUMN_NAME, CONST.OWNER FKTABLE_QUALIFIER, 'dbo' FKTABLE_OWNER, "
            lSql = lSql & "CONST.TABLE_NAME FKTABLE_NAME, COLUN_DEST.COLUMN_NAME FKCOLUMN_NAME, '1' KEY_SEQ, "
            lSql = lSql & "'1' UPDATE_RULE, '1' DELETE_RULE, CONST.CONSTRAINT_NAME FK_NAME, COLUN_DEST.COLUMN_NAME PK_NAME, '7' DEFERRABILITY "
            lSql = lSql & "FROM dba_CONSTRAINTS CONST, dba_cons_COLUMNS COLUN, dba_CONSTRAINTS CONST_DEST, dba_cons_COLUMNS COLUN_DEST "
            lSql = lSql & "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql = lSql & "and CONST.CONSTRAINT_TYPE = 'R' "
            lSql = lSql & "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST.R_CONSTRAINT_NAME = CONST_DEST.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST_DEST.CONSTRAINT_NAME = COLUN_DEST.CONSTRAINT_NAME "
            dsFK = PreencheDataSet(lSql, Tabela, Conexao)
        Catch ex As Exception
            lSql = ""
            lSql = lSql & "SELECT CONST.OWNER PKTABLE_QUALIFIER, 'dbo' PKTABLE_OWNER, COLUN_DEST.TABLE_NAME PKTABLE_NAME, "
            lSql = lSql & "COLUN.COLUMN_NAME PKCOLUMN_NAME, CONST.OWNER FKTABLE_QUALIFIER, 'dbo' FKTABLE_OWNER, "
            lSql = lSql & "CONST.TABLE_NAME FKTABLE_NAME, COLUN_DEST.COLUMN_NAME FKCOLUMN_NAME, '1' KEY_SEQ, "
            lSql = lSql & "'1' UPDATE_RULE, '1' DELETE_RULE, CONST.CONSTRAINT_NAME FK_NAME, COLUN_DEST.COLUMN_NAME PK_NAME, '7' DEFERRABILITY "
            lSql = lSql & "FROM ALL_CONS_COLUMNS COLUN, ALL_CONSTRAINTS CONST, ALL_CONS_COLUMNS COLUN_DEST, ALL_CONSTRAINTS CONST_DEST "
            lSql = lSql & "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql = lSql & "and CONST.CONSTRAINT_TYPE = 'R' "
            lSql = lSql & "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST.R_CONSTRAINT_NAME = CONST_DEST.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST_DEST.CONSTRAINT_NAME = COLUN_DEST.CONSTRAINT_NAME "
            dsFK = PreencheDataSet(lSql, Tabela, Conexao)
        End Try
        Return dsFK.Tables(0).DefaultView
    End Function

    Public Function ObterForeignKey(ByVal Tabela As String, ByRef Conexao As OracleConnection, ByRef Transacao As OracleTransaction) As DataView
        Dim lSql As String = ""
        Dim dsFK As DataSet

        Try
            lSql = lSql & "SELECT CONST.OWNER PKTABLE_QUALIFIER, 'dbo' PKTABLE_OWNER, COLUN_DEST.TABLE_NAME PKTABLE_NAME, "
            lSql = lSql & "COLUN.COLUMN_NAME PKCOLUMN_NAME, CONST.OWNER FKTABLE_QUALIFIER, 'dbo' FKTABLE_OWNER, "
            lSql = lSql & "CONST.TABLE_NAME FKTABLE_NAME, COLUN_DEST.COLUMN_NAME FKCOLUMN_NAME, '1' KEY_SEQ, "
            lSql = lSql & "'1' UPDATE_RULE, '1' DELETE_RULE, CONST.CONSTRAINT_NAME FK_NAME, COLUN_DEST.COLUMN_NAME PK_NAME, '7' DEFERRABILITY "
            lSql = lSql & "FROM dba_CONSTRAINTS CONST, dba_cons_COLUMNS COLUN, dba_CONSTRAINTS CONST_DEST, dba_cons_COLUMNS COLUN_DEST "
            lSql = lSql & "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql = lSql & "and CONST.CONSTRAINT_TYPE = 'R' "
            lSql = lSql & "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST.R_CONSTRAINT_NAME = CONST_DEST.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST_DEST.CONSTRAINT_NAME = COLUN_DEST.CONSTRAINT_NAME "
            dsFK = PreencheDataSet(lSql, Tabela, Conexao, Transacao)
        Catch ex As Exception
            lSql = ""
            lSql = lSql & "SELECT CONST.OWNER PKTABLE_QUALIFIER, 'dbo' PKTABLE_OWNER, COLUN_DEST.TABLE_NAME PKTABLE_NAME, "
            lSql = lSql & "COLUN.COLUMN_NAME PKCOLUMN_NAME, CONST.OWNER FKTABLE_QUALIFIER, 'dbo' FKTABLE_OWNER, "
            lSql = lSql & "CONST.TABLE_NAME FKTABLE_NAME, COLUN_DEST.COLUMN_NAME FKCOLUMN_NAME, '1' KEY_SEQ, "
            lSql = lSql & "'1' UPDATE_RULE, '1' DELETE_RULE, CONST.CONSTRAINT_NAME FK_NAME, COLUN_DEST.COLUMN_NAME PK_NAME, '7' DEFERRABILITY "
            lSql = lSql & "FROM ALL_CONS_COLUMNS COLUN, ALL_CONSTRAINTS CONST, ALL_CONS_COLUMNS COLUN_DEST, ALL_CONSTRAINTS CONST_DEST "
            lSql = lSql & "WHERE CONST.TABLE_NAME = '" & Tabela & "' "
            lSql = lSql & "and CONST.CONSTRAINT_TYPE = 'R' "
            lSql = lSql & "AND CONST.CONSTRAINT_NAME = COLUN.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST.R_CONSTRAINT_NAME = CONST_DEST.CONSTRAINT_NAME "
            lSql = lSql & "AND CONST_DEST.CONSTRAINT_NAME = COLUN_DEST.CONSTRAINT_NAME "
            dsFK = PreencheDataSet(lSql, Tabela, Conexao, Transacao)
        End Try

        Return dsFK.Tables(0).DefaultView
    End Function

    Private Function BuscaTipo(ByVal vTipo As String) As Type
        Select Case vTipo
            Case "bigint"
                Return GetType(Int64)
                'Case "binary"
            Case "bit"
                Return GetType(Byte)
            Case "char"
                Return GetType(String)
            Case "date"
                Return GetType(DateTime)
            Case "decimal"
                Return GetType(Decimal)
            Case "float"
                Return GetType(Double)
                'Case "image"
            Case "int"
                Return GetType(Int32)
            Case "money"
                Return GetType(Decimal)
            Case "nchar"
                Return GetType(String)
            Case "ntext"
                Return GetType(String)
            Case "numeric"
                Return GetType(Decimal)
            Case "nvarchar"
                Return GetType(String)
            Case "real"
                Return GetType(Single)
            Case "smalldatetime"
                Return GetType(DateTime)
            Case "smallint"
                Return GetType(Int16)
            Case "smallmoney"
                Return GetType(Single)
                'Case "Oracle_variant"
                'Case "sysname"
            Case "text"
                Return GetType(String)
                'Case "timestamp"
            Case "tinyint"
                Return GetType(Byte)
                'Case "uniqueidentifier"
                'Case "varbinary"
            Case "varchar"
                Return GetType(String)
            Case Else
                Return GetType(String)
        End Select
    End Function

    Private Function BuscaTipoSql(ByVal vTipo As String) As SqlDbType
        Select Case vTipo
            Case "bigint"
                Return OracleType.Int32
                'Case "binary"
                'Case "bit"
                '    Return OracleType.Bit
            Case "char"
                Return OracleType.Char
            Case "datetime"
                Return OracleType.DateTime
            Case "decimal"
                Return OracleType.Float
            Case "float"
                Return OracleType.Float
                'Case "image"
            Case "int"
                Return OracleType.Int16
            Case "money"
                Return OracleType.Number
            Case "nchar"
                Return OracleType.NChar
            Case "ntext"
                Return OracleType.NChar
            Case "nvarchar"
                Return OracleType.NVarChar
            Case "real"
                Return OracleType.Number
            Case "smalldatetime"
                Return OracleType.DateTime
            Case "smallint"
                Return OracleType.Int32
            Case "smallmoney"
                Return OracleType.Number
                'Case "sql_variant"
                'Case "sysname"
            Case "text"
                Return OracleType.VarChar
                'Case "timestamp"
            Case "tinyint"
                Return OracleType.Int32
                'Case "uniqueidentifier"
                'Case "varbinary"
            Case "varchar"
                Return OracleType.VarChar
            Case "varchar2"
                Return OracleType.VarChar
        End Select
    End Function
#End Region

End Class
