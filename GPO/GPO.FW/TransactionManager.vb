Imports System
Imports System.Diagnostics
Imports System.Reflection

'/ <summary>
'/ Summary description for TransactionManager.
'/ </summary>
Public Class TransactionManager

    Private Const _ORIGINAL_METHOD_BACKOFFSET As Integer = 1

#Region "Vote for a Transaction and eventually Close the Connection"
    Public Shared Sub SetComplete()
        Dim currentTC As TransactionContext = GetCurrentTransactionContext()
        If (currentTC Is Nothing) Then
            Return ' no transaction context
        End If

        currentTC.IsDone = True

        Dim StackTrace As StackTrace = New StackTrace
        If (currentTC.StackDepth = StackTrace.FrameCount - 1) Then
            CommitTransaction(currentTC)
        End If
    End Sub
    Public Shared Sub SetAbort()
        Dim currentTC As TransactionContext = GetCurrentTransactionContext()
        If (currentTC Is Nothing) Then
            Return ' no transaction context
        End If

        currentTC.IsDone = True
        currentTC.IsHappy = False

        Dim StackTrace As StackTrace = New StackTrace
        If (currentTC.StackDepth = StackTrace.FrameCount - 1) Then
            CommitTransaction(currentTC)
        End If
    End Sub
#End Region

#Region "Hold Connection for a DataReader"
    Public Shared Sub SetOnHold()
        Dim currentTC As TransactionContext = GetTransactionContext(_ORIGINAL_METHOD_BACKOFFSET)
        If (currentTC Is Nothing) Then
            Return 
        End If

        currentTC.IsDone = False

        'set HoldStackDepth
        Const CALLING_METHOD_OFFSET As Integer = 1
        Dim StackTrace As StackTrace = New StackTrace
        currentTC.HoldStackDepth = StackTrace.FrameCount - CALLING_METHOD_OFFSET

        TransactionContextCol.SetHoldContext(currentTC)
    End Sub

    Public Shared Sub SetHoldComplete()
        Dim currentTC As TransactionContext = TransactionContextCol.GetHoldContext()
        If (currentTC Is Nothing) Then
            Return ' no transaction context
        End If

        currentTC.IsDone = True

        If (currentTC.HoldStackDepth = currentTC.StackDepth) Then
            CommitTransaction(currentTC)
        End If
        TransactionContextCol.RemoveHoldContext()
    End Sub
#End Region

#Region "Helper functions"
    Private Shared Sub CommitTransaction(ByRef tc As TransactionContext)
        If (tc.IsDone) Then
            If (tc.IsHappy) Then
                tc.Commit()
            Else
                tc.Rollback()
            End If
            TransactionContextCol.RemoveContext(tc.Name)
            TransactionContextCol.RemoveCurrentContext()
            tc.Dispose()
            tc = Nothing
        End If
    End Sub

    Private Shared Function GetCurrentTransactionContext() As TransactionContext
        Return (TransactionContextCol.GetCurrentContext())
    End Function

    Friend Shared Function GetTransactionContext(ByVal originalMethodBackOffset As Integer) As TransactionContext
        Dim tc As TransactionContext = Nothing
        Dim stackTrace As stackTrace = New stackTrace
        originalMethodBackOffset += 1
        Dim prevMethodInfo As MemberInfo = stackTrace.GetFrame(originalMethodBackOffset).GetMethod()
        Dim arCustomAttributes As Object() = prevMethodInfo.GetCustomAttributes(GetType(TransactionContextAttribute), False)

        Dim transCtxAttrib As TransactionContextAttribute = Nothing
        If (arCustomAttributes.Length > 0) Then
            'There are transaction attributes!
            transCtxAttrib = arCustomAttributes(0)
            'If not Transaction.Disabled then get/create TransactionContext!
            If (transCtxAttrib.Transaction <> Transaction.Disabled And transCtxAttrib.Transaction <> Transaction.NotSupported) Then
                tc = TransactionContextCol.GetContext(transCtxAttrib)
                If (tc Is Nothing And transCtxAttrib.Transaction <> Transaction.Supported) Then
                    tc = CreateTransactionContext(transCtxAttrib, originalMethodBackOffset)
                End If
            End If
        End If


        Return (tc)
    End Function

    Private Shared Function CreateTransactionContext(ByRef transCtxAttrib As TransactionContextAttribute, ByVal originalMethodBackOffset As Integer) As TransactionContext
        'set StackDepth, check backwards the call stack ...
        Dim stackTrace As stackTrace = New stackTrace
        Dim stackFrameCount As Integer = stackTrace.FrameCount
        originalMethodBackOffset += 1
        Dim callingMethodStackIndex As Integer = originalMethodBackOffset
        If (transCtxAttrib.Transaction = Transaction.Required Or transCtxAttrib.Transaction = Transaction.Supported) Then

            Dim prevTransCtxAttrib As TransactionContextAttribute = Nothing
            Dim prevMethodInfo As MethodInfo = Nothing
            Dim arCustomAttributes As Object() = Nothing
            originalMethodBackOffset += 1
            For i As Integer = originalMethodBackOffset To stackFrameCount - 1
                prevMethodInfo = stackTrace.GetFrame(i).GetMethod()
                arCustomAttributes = prevMethodInfo.GetCustomAttributes(GetType(TransactionContextAttribute), False)
                If (arCustomAttributes.Length > 0) Then

                    prevTransCtxAttrib = arCustomAttributes(0)
                    If ((prevTransCtxAttrib.Transaction = Transaction.Required Or prevTransCtxAttrib.Transaction = Transaction.RequiresNew) _
                        And (prevTransCtxAttrib.Isolation = transCtxAttrib.Isolation Or transCtxAttrib.Isolation = IsolationLevel.Unspecified)) Then

                        callingMethodStackIndex = i
                        If (prevTransCtxAttrib.Transaction = Transaction.RequiresNew) Then
                            Exit For
                        End If
                    End If
                End If
            Next
            If (Not prevTransCtxAttrib Is Nothing) Then
                transCtxAttrib = prevTransCtxAttrib
            End If
        End If
        Dim tc As TransactionContext = New TransactionContext(transCtxAttrib.Transaction, transCtxAttrib.Isolation, stackFrameCount - callingMethodStackIndex)
        TransactionContextCol.AddContext(tc)
        TransactionContextCol.SetCurrentContext(tc)  'set the default context for easy lookup

        Return (tc)

    End Function
#End Region

End Class
