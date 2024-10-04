Imports System.Data
Imports System.Data.Common
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class Restricao
    Inherits dsBase

    Private Enum tipRestricao
        NotVisible = 1
        NotEnabled = 2
    End Enum

#Region "New"
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal Banco As String, ByVal Servidor As String)
        MyBase.New(tipoBanco.SqlDB, "Integrated Security=SSPI;Initial Catalog=" & Banco & ";Data Source=" & Servidor & ";")
    End Sub
#End Region

#Region "Estrutura"
    Public Function ObterEstruturaObjeto() As DataSet
        Dim lDataSet As DataSet
        lDataSet = MyBase.ObterEstrutura("tb_fw_objeto")
        Return lDataSet
    End Function

    Public Function ObterEstruturaRestricao() As DataSet
        Dim lDataSet As DataSet
        lDataSet = MyBase.ObterEstrutura("tb_fw_restricao")
        Return lDataSet
    End Function
#End Region

#Region "Listar"
    ''' -----------------------------------------------------------------------------
    ''' <summary>Lista Perfis do sistema</summary>
    ''' <returns>DataView com lista de perfis</returns>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function ListarPerfil() As DataView
        Dim lRetorno As DataSet
        lRetorno = MyBase.Listar("tb_fw_perfil")
        Return lRetorno.Tables("tb_fw_perfil").DefaultView
    End Function

    Public Function ListarRestricao(ByVal cod_objeto_pai_obj As Integer, ByVal tip_perfil_per As Integer, ByVal NumNivel As Integer) As DataSet
        Dim lCommand As SqlClient.SqlCommand = InicializaProcedure("usp_fw_ListarRestricaoCadastro")
        Dim lRetorno As DataSet, lDataSet As DataSet, lDataSetRec As DataSet
        Dim lRowRec As DataRow

        NumNivel += 1

        lCommand.Parameters.Clear()
        lCommand.Parameters.Add("@cod_objeto_pai_obj", SqlDbType.Int)
        lCommand.Parameters("@cod_objeto_pai_obj").Value = cod_objeto_pai_obj
        lCommand.Parameters.Add("@tip_perfil_per", SqlDbType.Int)
        lCommand.Parameters("@tip_perfil_per").Value = tip_perfil_per

        lDataSet = ConsultaQueryDataSet(lCommand, "tb_fw_objeto")
        lRetorno = lDataSet.Clone
        For Each lRow As DataRow In lDataSet.Tables("tb_fw_objeto").Rows
            With lRetorno.Tables("tb_fw_objeto")
                lRowRec = .NewRow
                lRowRec.ItemArray = lRow.ItemArray
                For i As Integer = 0 To NumNivel - 1
                    lRowRec("des_objeto_obj") = "- " + lRowRec("des_objeto_obj")
                    lRowRec("des_titulo_obj") = "- " + lRowRec("des_titulo_obj")
                Next
                .Rows.Add(lRowRec)

                lDataSetRec = ListarRestricao(lRow("cod_objeto_obj"), tip_perfil_per, NumNivel)
                For Each lRowIn As DataRow In lDataSetRec.Tables("tb_fw_objeto").Rows
                    lRowRec = .NewRow
                    lRowRec.ItemArray = lRowIn.ItemArray
                    For i As Integer = 0 To NumNivel
                        lRowRec("des_objeto_obj") = "  " + lRowRec("des_objeto_obj")
                    Next
                    .Rows.Add(lRowRec)
                Next
            End With
        Next
        Return lRetorno
    End Function

    Public Function ListarObjeto(ByVal cod_objeto_pai_obj As Integer, ByVal NumNivel As Integer) As DataSet
        Dim lCommand As SqlClient.SqlCommand = InicializaProcedure("usp_fw_ListarObjeto")
        Dim lRetorno As DataSet, lDataSet As DataSet, lDataSetRec As DataSet
        Dim lRowRec As DataRow

        NumNivel += 1

        lCommand.Parameters.Clear()
        lCommand.Parameters.Add("@cod_objeto_pai_obj", SqlDbType.Int)
        lCommand.Parameters("@cod_objeto_pai_obj").Value = cod_objeto_pai_obj

        lDataSet = ConsultaQueryDataSet(lCommand, "tb_fw_objeto")
        lRetorno = lDataSet.Clone
        For Each lRow As DataRow In lDataSet.Tables("tb_fw_objeto").Rows
            With lRetorno.Tables("tb_fw_objeto")
                lRowRec = .NewRow
                lRowRec.ItemArray = lRow.ItemArray
                For i As Integer = 0 To NumNivel - 1
                    lRowRec("des_objeto_obj") = "- " + lRowRec("des_objeto_obj")
                Next
                .Rows.Add(lRowRec)

                lDataSetRec = ListarObjeto(lRow("cod_objeto_obj"), NumNivel)
                For Each lRowIn As DataRow In lDataSetRec.Tables("tb_fw_objeto").Rows
                    lRowRec = .NewRow
                    lRowRec.ItemArray = lRowIn.ItemArray
                    For i As Integer = 0 To NumNivel
                        lRowRec("des_objeto_obj") = "  " + lRowRec("des_objeto_obj")
                    Next
                    .Rows.Add(lRowRec)
                Next
            End With
        Next
        Return lRetorno
    End Function
#End Region

#Region "Cadastro"
    Public Sub IncluirObjeto(ByVal dsRegistro As DataSet)
        MyBase.Incluir(dsRegistro.Tables("tb_fw_objeto"))
    End Sub

    Public Sub AlterarObjeto(ByVal dsRegistro As DataSet)
        MyBase.Alterar(dsRegistro.Tables("tb_fw_objeto"))
    End Sub

    Public Sub ExcluirObjeto(ByVal cod_objeto_obj As Integer)
        Dim dsRegistro As DataSet = ObterEstruturaObjeto()
        dsRegistro.Tables("tb_fw_objeto").Rows(0)("cod_objeto_obj") = cod_objeto_obj
        MyBase.Excluir(dsRegistro.Tables("tb_fw_objeto"))
    End Sub

    <TransactionContext(Transaction.Required, IsolationLevel.ReadCommitted)> _
    Public Sub ExcluirRestricao(ByVal tip_perfil_per As Integer)
        Try
            Dim lCommand As SqlClient.SqlCommand = InicializaProcedure("usp_fw_ExcluirRestricao")
            lCommand.Parameters("@tip_perfil_per").Value = tip_perfil_per
            ExecutaNonQuery(lCommand)
            TransactionManager.SetComplete()
        Catch ex As Exception
            TransactionManager.SetAbort()
            Throw ex
        End Try
    End Sub

    <TransactionContext(Transaction.Supported, IsolationLevel.ReadCommitted)> _
    Public Sub IncluirRestricao(ByVal dsRegistro As DataSet)
        Try
            MyBase.Incluir(dsRegistro.Tables("tb_fw_restricao"))
            TransactionManager.SetComplete()
        Catch ex As Exception
            TransactionManager.SetAbort()
            Throw ex
        End Try
    End Sub
#End Region

#Region "VerificaRestricao"
    ''' -----------------------------------------------------------------------------
    ''' <summary>Verifica restrição do objeto</summary>
    ''' <param name="tipPerfil">Perfil do usuário logado</param>
    ''' <param name="Pagina">Objeto {PAGE}</param>
    ''' <param name="ID">Nome da página</param>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub VerificaRestricao(ByVal tipPerfil As Integer, ByRef Pagina As Object, ByVal ID As String)
        Dim lPai As DataView
        lPai = ConsultarRestricao(tipPerfil, ID)
        If lPai.Table.Rows.Count > 0 Then
            Throw New INSIXAcessoNegadoException(lPai.Table.Rows(0)("des_titulo_obj"))
        End If

        Dim lCommand As DbCommand = InicializaProcedure("usp_fw_ListarRestricao")
        Dim lDataSet As DataSet, objRestricao As Object

        lCommand.Parameters("@tipPerfil").Value = tipPerfil
        lCommand.Parameters("@desObjeto").Value = ID

        lDataSet = ConsultaQueryDataSet(lCommand, "tb_fw_restricao")
        For Each lrow As DataRow In lDataSet.Tables("tb_fw_restricao").Rows
            If lrow("des_objeto_obj") = ID Then
                Throw New INSIXAcessoNegadoException(lrow("des_titulo_obj"))
            End If
            If lrow("tip_objeto_obj") = 0 Or lrow("tip_objeto_obj") = 1 Then
                Throw New INSIXAcessoNegadoException(lrow("des_titulo_obj"))
            End If
            objRestricao = ProcuraObjeto(lrow("des_objeto_obj"), Pagina)
            If Not objRestricao Is Nothing Then
                If lrow("tip_restricao_res") = tipRestricao.NotVisible Then
                    objRestricao.Visible = False
                ElseIf lrow("tip_restricao_res") = tipRestricao.NotEnabled Then
                    objRestricao.Enabled = False
                End If
            End If
        Next

        lDataSet.Dispose()
    End Sub

    Public Function ConsultarRestricao(ByVal tip_perfil_per As Integer, ByVal des_objeto_obj As String) As DataView
        Dim lRetorno As DataSet
        Dim lCommand As DbCommand = InicializaProcedure("usp_fw_ConsultarRestricao")

        lCommand.Parameters("@tip_perfil_per").Value = tip_perfil_per
        lCommand.Parameters("@desObjeto").Value = des_objeto_obj

        lRetorno = ConsultaQueryDataSet(lCommand, "tb_fw_restricao")
        Return lRetorno.Tables("tb_fw_restricao").DefaultView
    End Function

    Private Function ProcuraObjeto(ByVal Objeto As String, ByVal Pagina As Object) As Object
        Dim objDividido As String(), objPrincipal As String
        Dim objRestricao As Object
        objDividido = Objeto.Split(".")
        objPrincipal = objDividido(0)
        objRestricao = Pagina.FindControl(objPrincipal)
        If objRestricao.GetType().Name = "DataGrid" Then
            Dim encontrou As Boolean = False
            If objDividido(objDividido.Length - 1).Substring(0, 7) = "Columns" Then
                objRestricao = objRestricao.Columns(objDividido(objDividido.Length - 1).Substring(8, objDividido(objDividido.Length - 1).Length() - 9))
            ElseIf objRestricao.Items.Count > 0 Then
                For col As Int32 = 0 To objRestricao.Items(0).Cells.Count - 1
                    For ctr As Int32 = 0 To objRestricao.Items(0).Cells(col).Controls.Count - 1
                        If objRestricao.Items(0).Cells(col).Controls(ctr).ID = objDividido(objDividido.Length - 1) Then
                            objRestricao = objRestricao.Columns(col)
                            Return objRestricao
                        End If
                    Next
                Next
            End If
            If Not encontrou Then
                objRestricao = Nothing
            End If
        ElseIf objRestricao.GetType().Name = "GridView" Then
            Dim encontrou As Boolean = False
            If objDividido(objDividido.Length - 1).Substring(0, 7) = "Columns" Then
                objRestricao = objRestricao.Columns(objDividido(objDividido.Length - 1).Substring(8, objDividido(objDividido.Length - 1).Length() - 9))
            ElseIf objRestricao.Rows.Count > 0 Then
                For col As Int32 = 0 To objRestricao.Rows(0).Cells.Count - 1
                    For ctr As Int32 = 0 To objRestricao.Rows(0).Cells(col).Controls.Count - 1
                        If objRestricao.Rows(0).Cells(col).Controls(ctr).ID = objDividido(objDividido.Length - 1) Then
                            objRestricao = objRestricao.Columns(col)
                            Return objRestricao
                        End If
                    Next
                Next
            End If
            If Not encontrou Then
                objRestricao = Nothing
            End If
        End If
        Return objRestricao
    End Function
#End Region

End Class

#Region "Eventos"
Public Class INSIXAcessoNegadoException
    Inherits ApplicationException

    Private lDesObjeto As String
    Public ReadOnly Property Objeto() As String
        Get
            Return lDesObjeto
        End Get
    End Property

    Sub New(ByVal Objeto As String)
        MyBase.New("Acesso negado!")
        lDesObjeto = Objeto
    End Sub
End Class
#End Region