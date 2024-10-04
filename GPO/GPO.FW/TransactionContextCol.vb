Imports System
Imports System.Runtime.Remoting.Messaging
Imports System.Diagnostics

'/ <summary>
'/ Summary description for TransactionContextCol.
'/ </summary>
Friend Class TransactionContextCol
    Private Const HOLD_CONTEXT_NAME As String = "HoldContext"
    Private Const CURRENT_CONTEXT_NAME As String = "CurrentContext"

    Public Shared Sub AddContext(ByRef TC As TransactionContext)
        CallContext.SetData(TC.Name, TC)
    End Sub

    Public Shared Function GetContext(ByRef transCtxAttrib As TransactionContextAttribute) As TransactionContext
        Dim tc As TransactionContext = CallContext.GetData(MakeTCName(transCtxAttrib))
        'check other possible in case of Transaction.Supported or Transaction.Required
        If (tc Is Nothing) Then
            Select Case (transCtxAttrib.Transaction)
                Case Transaction.Supported
                    tc = CallContext.GetData(MakeTCName(Transaction.Required, transCtxAttrib.Isolation))
                    If (tc Is Nothing) Then
                        tc = CallContext.GetData(MakeTCName(Transaction.RequiresNew, transCtxAttrib.Isolation))
                    End If
                Case Transaction.Required
                    tc = CallContext.GetData(MakeTCName(Transaction.Supported, transCtxAttrib.Isolation))
                    If (tc Is Nothing And transCtxAttrib.Transaction = Transaction.Required) Then
                        tc = CallContext.GetData(MakeTCName(Transaction.RequiresNew, transCtxAttrib.Isolation))
                    End If
            End Select
        End If

        Return (tc)
    End Function

    Public Shared Sub RemoveContext(ByVal tcName As String)
        CallContext.FreeNamedDataSlot(tcName)
    End Sub

    Public Shared Sub SetHoldContext(ByRef TC As TransactionContext)
        CallContext.SetData(HOLD_CONTEXT_NAME, TC)
    End Sub

    Public Shared Function GetHoldContext() As TransactionContext
        Return (CallContext.GetData(HOLD_CONTEXT_NAME))
    End Function

    Public Shared Sub RemoveHoldContext()
        CallContext.FreeNamedDataSlot(HOLD_CONTEXT_NAME)
    End Sub

    Public Shared Sub SetCurrentContext(ByRef tc As TransactionContext)
        CallContext.SetData(CURRENT_CONTEXT_NAME, tc)
    End Sub
    Public Shared Function GetCurrentContext() As TransactionContext
        Return (CallContext.GetData(CURRENT_CONTEXT_NAME))
    End Function

    Public Shared Sub RemoveCurrentContext()
        ' **** ALTERADO DO ORIGINAL ****
        ' Evitar o problema que ocorria quando se removia o corrente contexto
        ' e ainda tinha um contexto REQUIRED.

        If ((CallContext.GetData(CURRENT_CONTEXT_NAME)).Name = "TCRequiresNew-ReadCommitted" Or (CallContext.GetData(CURRENT_CONTEXT_NAME)).Name = "TCRequiresNew-ReadUncommitted") Then
            ' No retorno da chamada de um método REQUIRES NEW é recuperado algum contexto REQUIRED que esteja ativo na pilha de execução.
            If (Not (CallContext.GetData("TCRequired-ReadCommitted")) Is Nothing) Then
                SetCurrentContext((CallContext.GetData("TCRequired-ReadCommitted")))
            ElseIf (Not (CallContext.GetData("TCRequired-ReadUncommitted")) Is Nothing) Then
                SetCurrentContext((CallContext.GetData("TCRequired-ReadUncommitted")))
            Else
                CallContext.FreeNamedDataSlot(CURRENT_CONTEXT_NAME)
            End If

        ElseIf ((CallContext.GetData(CURRENT_CONTEXT_NAME)).Name = "TCNotSupported-ReadCommitted" Or (CallContext.GetData(CURRENT_CONTEXT_NAME)).Name = "TCNotSupported-ReadUncommitted") Then
            ' No retorno da chamada de um método NOTSUPPORTED é recuperado algum contexto REQUIRED que esteja ativo na pilha de execução.
            If (Not (CallContext.GetData("TCRequired-ReadCommitted")) Is Nothing) Then
                SetCurrentContext((CallContext.GetData("TCRequired-ReadCommitted")))
            ElseIf (Not (CallContext.GetData("TCRequired-ReadUncommitted")) Is Nothing) Then
                SetCurrentContext((CallContext.GetData("TCRequired-ReadUncommitted")))
            Else
                CallContext.FreeNamedDataSlot(CURRENT_CONTEXT_NAME)
            End If
        Else

            CallContext.FreeNamedDataSlot(CURRENT_CONTEXT_NAME)
        End If
        ' **** ALTERADO DO ORIGINAL ****

        'CallContext.FreeNamedDataSlot(CURRENT_CONTEXT_NAME)
    End Sub

    Public Shared Function MakeTCName(ByVal transaction As Transaction, ByVal isolation As IsolationLevel) As String
        Return ("TC" + transaction.ToString() + "-" + isolation.ToString())
    End Function
    Public Shared Function MakeTCName(ByVal transCtxAttrib As TransactionContextAttribute) As String
        Return ("TC" + transCtxAttrib.Transaction.ToString() + "-" + transCtxAttrib.Isolation.ToString())
    End Function
End Class
