Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics

'#Region "Isolation"
'Public Enum Isolation
'    Chaos = 0  '// not really used, copied from Data.IsolationLevel
'    ReadCommitted
'    ReadUncommitted
'    RepeatableRead
'    Serializable
'    Unspecified  '// default, meaning not set
'End Enum
'#End Region

#Region "Transaction"
Public Enum Transaction
    Disabled = 0 ' no transaction context will be created
    NotSupported ' transaction context will be created 
    ' managing internally a connection, no transaction is opened though
    Required  ' transaction context will be created if not present 
    ' managing internally a connection and a transaction
    RequiresNew ' a new transaction context will be created 
    ' managing internally a connection and a transaction
    Supported  ' an existing appropriate transaction context will be joined if present
End Enum
#End Region

#Region "TransactionContextAttribute"
<AttributeUsage(AttributeTargets.Method Or AttributeTargets.Constructor)> _
Public Class TransactionContextAttribute
    Inherits Attribute
    Public Transaction As Transaction = Transaction.Disabled
    Private _isolation As System.Data.IsolationLevel = IsolationLevel.Unspecified

    Public Sub New(ByVal transaction As Transaction, ByVal Isolation As IsolationLevel)
        Me.Transaction = transaction
        Me._isolation = Isolation
    End Sub


    Public Property Isolation() As IsolationLevel
        Get
            Return _isolation
        End Get
        Set(ByVal Value As IsolationLevel)
            _isolation = Value
        End Set
    End Property
End Class
#End Region

#Region "TransactionDetails helper class"
Friend Class TransactionDetails
    Public ConnectionString As String = ""
    Public Connection As System.Data.IDbConnection = Nothing
    Public Transaction As System.Data.IDbTransaction = Nothing

    Public Sub New(ByVal lConnectionString As String, ByRef lConnection As System.Data.IDbConnection, ByRef lTransaction As System.Data.IDbTransaction)
        ConnectionString = lconnectionString
        Connection = lconnection
        Transaction = ltransaction
    End Sub
End Class
#End Region

#Region "TransactionContext class"
Friend Class TransactionContext
    Implements IDisposable
    Private _transactionDetailsCol As ListDictionary = Nothing
    Private _transactionDetails As TransactionDetails = Nothing

    Private _isSingleConnection As Boolean = True

    Public Isolation As IsolationLevel
    Public Transaction As Transaction

    Public IsHappy As Boolean = True
    Public IsDone As Boolean = False
    Public StackDepth As Integer
    Public HoldStackDepth As Integer

    Public ReadOnly Property Name() As String
        Get
            Return TransactionContextCol.MakeTCName(Transaction, Isolation)
        End Get
    End Property

    Public Sub New(ByVal lTransaction As Transaction, ByVal lIsolation As IsolationLevel, ByVal lStackDepth As Integer)
        Transaction = lTransaction
        Isolation = lIsolation
        StackDepth = lStackDepth
    End Sub

    Public Sub Commit()
        If (_isSingleConnection) Then
            If (Not _transactionDetails.Transaction Is Nothing) Then
                _transactionDetails.Transaction.Commit()
                _transactionDetails.Transaction.Dispose()
                _transactionDetails.Transaction = Nothing
            End If
            _transactionDetails.Connection.Dispose()
            _transactionDetails.Connection = Nothing
        Else
            For Each entry As DictionaryEntry In _transactionDetailsCol
                Dim td As TransactionDetails = entry.Value
                If (Not td.Transaction Is Nothing) Then
                    td.Transaction.Commit()
                    td.Transaction.Dispose()
                    td.Transaction = Nothing
                End If
                td.Connection.Close()
                td.Connection = Nothing
            Next
        End If
    End Sub

    Public Sub Rollback()
        If (_isSingleConnection) Then
            If (Not _transactionDetails.Transaction Is Nothing) Then
                _transactionDetails.Transaction.Rollback()
                _transactionDetails.Transaction.Dispose()
                _transactionDetails.Transaction = Nothing
            End If
            _transactionDetails.Connection.Dispose()
            _transactionDetails.Connection = Nothing
        Else
            For Each entry As DictionaryEntry In _transactionDetailsCol
                Dim td As TransactionDetails = entry.Value
                If (Not td.Transaction Is Nothing) Then
                    td.Transaction.Rollback()
                    td.Transaction.Dispose()
                    td.Transaction = Nothing
                End If
                td.Connection.Close()
                td.Connection = Nothing
            Next
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If (Not _isSingleConnection) Then
            _transactionDetailsCol.Clear()
            _transactionDetailsCol = Nothing
        End If
    End Sub

    Friend Function GetTransactionDetails(ByVal data As dsBase) As TransactionDetails
        Dim con As System.Data.IDbConnection = Nothing
        Dim tran As System.Data.IDbTransaction = Nothing
        Dim td As TransactionDetails = Nothing
        Dim connectionString As String = data.StringConexao

        If (_isSingleConnection) Then
            If (Not _transactionDetails Is Nothing AndAlso connectionString = _transactionDetails.ConnectionString) Then

                td = _transactionDetails
            End If
        Else
            td = _transactionDetailsCol(connectionString)
        End If

        If (td Is Nothing) Then
            con = data.CriaConexao()
            If (Transaction <> Transaction.Disabled And Transaction <> Transaction.NotSupported) Then
                tran = con.BeginTransaction(Isolation)
            End If

            td = New TransactionDetails(connectionString, con, tran)

            If (_isSingleConnection And _transactionDetails Is Nothing) Then
                _transactionDetails = td
            Else
                If (_isSingleConnection And Not _transactionDetails Is Nothing) Then
                    '					_transactionDetailsCol = new Hashtable(1, 1)
                    _transactionDetailsCol = New ListDictionary
                    _isSingleConnection = False
                    _transactionDetailsCol(_transactionDetails.ConnectionString) = _transactionDetails
                    _transactionDetails = Nothing
                End If
                _transactionDetailsCol(connectionString) = td
            End If
        End If

        Return (td)
    End Function
End Class
#End Region