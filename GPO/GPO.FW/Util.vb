Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Security
Imports System.Text.RegularExpressions
Imports System.Text

Public Class Util

#Region " Números "

    ''' -----------------------------------------------------------------------------
    ''' <summary>Retorna numeros randomicos</summary>
    ''' <param name="LimiteInferior">Limite inferior</param>
    ''' <param name="LimiteSuperior">Limite superior</param>
    ''' <returns>Número randomico</returns>
    ''' <history>
    ''' 	[fcampinho]	12/11/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetRandomNumber(ByVal LimiteInferior As Integer, ByVal LimiteSuperior As Integer) As Integer
        Dim RandomGenerator As Random
        Dim intRandomNumber As Integer
        RandomGenerator = New Random
        intRandomNumber = RandomGenerator.Next(LimiteInferior, LimiteSuperior + 1)
        GetRandomNumber = intRandomNumber
    End Function

#End Region

#Region " Tratamento de Erro "

    Public Shared Function TrataErro(ByVal objException As Exception, ByVal objServer As System.Web.HttpServerUtility, ByVal objPage As Page) As String

        If Configuration.ConfigurationManager.AppSettings("CustomError") Then

            Dim objErro As TrataErro
            objErro = New TrataErro(objException, objPage.Request.PhysicalApplicationPath() & "Erros.xml")

            Dim usuario As String
            If Not String.IsNullOrEmpty(objPage.Session("num_matricula_usr")) Then
                usuario = objPage.Session("num_matricula_usr")
            ElseIf Not String.IsNullOrEmpty(objPage.Session("cod_usuario_usu")) Then
                usuario = objPage.Session("cod_usuario_usu")
            Else
                usuario = ""
            End If


            If objErro.TipoErro = GPO.FW.TrataErro.TipoExcecao.NaoTratado Then

                Dim MensErro As String = objPage.Request.Url.ToString & vbCrLf & vbCrLf & vbCrLf & "Usuario: " & usuario & " IP: " & objPage.Request.UserHostAddress & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & objPage.Server.GetLastError.GetBaseException.Message.ToString() & vbCrLf & vbCrLf & vbCrLf & objPage.Server.GetLastError.GetBaseException.StackTrace.ToString() & vbCrLf & vbCrLf & vbCrLf & objPage.Server.GetLastError.GetBaseException.Source.ToString()
                Dim from As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress("suporte@insix.com.br", "Suporte Insix")
                Dim destino As System.Net.Mail.MailAddress = New System.Net.Mail.MailAddress("suporte@insix.com.br", "Suporte Insix")
                Dim objMensagem As New System.Net.Mail.MailMessage(from, destino)

                objMensagem.Subject = "INSIX - Erro não tratado!"
                objMensagem.Body = MensErro

                Dim client As New System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings("smtp"))
                Try
                    client.Send(objMensagem)
                Catch ex As Exception

                End Try

            End If

            Return objErro.Excecao.Message

        End If

        Return objException.Message

    End Function

#End Region

#Region " Manipulação de Dados "

    Public Shared Sub ManterCamposViaRequestForm(ByVal objPage As Page, ByVal numCotrole As Int16)

        Dim controle As Control

        For Each campo As String In objPage.Request.Form
            'objPage.Response.Write(campo & ": " & objPage.Request(campo) & " " & campo.Split("$")(campo.Split("$").Length - 1) & "<br>")

            controle = objPage.Form.Controls.Item(numCotrole).FindControl(campo.Split("$")(campo.Split("$").Length - 1))
            If Not controle Is Nothing Then
                If controle.GetType().FullName = "System.Web.UI.WebControls.TextBox" Then
                    CType(controle, TextBox).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCpf" Then
                    '    CType(controle, ixCpf).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCnpj" Then
                    '    CType(controle, ixCnpj).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixTelefone" Then
                    '    CType(controle, ixTelefone).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixData" Then
                    '    CType(controle, ixData).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCep" Then
                    '    CType(controle, ixCep).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixInteiro" Then
                    '    CType(controle, ixInteiro).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixMonetario" Then
                    '    CType(controle, ixMonetario).Text = objPage.Request(campo)
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixReal" Then
                    '    CType(controle, ixReal).Text = objPage.Request(campo)
                ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.DropDownList" Then
                    If objPage.Request(campo) <> "-1" Then
                        objPage.Session.Add(controle.ID, IIf(objPage.Request(campo) Is Nothing, "-1", objPage.Request(campo)))
                        'CType(controle, DropDownList).SelectedValue = objPage.Request(campo)
                    End If
                End If
            End If

        Next

    End Sub

    Public Shared Sub ManterCamposViaDataView(ByVal objPage As Page, ByVal lDataView As Data.DataView, ByVal numCotrole As Int16)
        Dim controle As Control
        If lDataView.Table.Rows.Count > 0 Then
            For c As Int32 = 0 To lDataView.Table.Columns.Count - 1
                controle = objPage.Form.Controls.Item(numCotrole).FindControl(lDataView.Table.Columns(c).ColumnName)

                If Not controle Is Nothing Then
                    If controle.GetType().FullName = "System.Web.UI.WebControls.TextBox" Then
                        If lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                            CType(controle, TextBox).Text = ""
                        ElseIf lDataView.Table.Columns(c).DataType Is GetType(DateTime) Then
                            CType(controle, TextBox).Text = CType(lDataView.Table.Rows(0)(c), DateTime).ToString("dd/MM/yyyy")
                        ElseIf lDataView.Table.Columns(c).DataType Is GetType(Decimal) Then
                            CType(controle, TextBox).Text = CType(lDataView.Table.Rows(0)(c), Decimal).ToString("0.00")
                        Else
                            CType(controle, TextBox).Text = lDataView.Table.Rows(0)(c)
                        End If
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.Label" Then
                        CType(controle, Label).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCpf" Then
                        '    CType(controle, ixCpf).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCnpj" Then
                        '    CType(controle, ixCnpj).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixTelefone" Then
                        '    CType(controle, ixTelefone).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixData" Then
                        '    If lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                        '        CType(controle, ixData).Text = ""
                        '    Else
                        '        CType(controle, ixData).Text = CType(lDataView.Table.Rows(0)(c), Date).ToString("dd/MM/yyyy")
                        '    End If
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCep" Then
                        '    CType(controle, ixCep).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixInteiro" Then
                        '    CType(controle, ixInteiro).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixMonetario" Then
                        '    CType(controle, ixMonetario).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixReal" Then
                        '    CType(controle, ixReal).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.DropDownList" Then
                        objPage.Session.Add(lDataView.Table.Columns(c).ColumnName, IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "-1", lDataView.Table.Rows(0)(c)))
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.CheckBox" Then
                        Try
                            CType(controle, CheckBox).Checked = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, False, lDataView.Table.Rows(0)(c))
                        Catch ex As Exception
                            If Not lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                                CType(controle, CheckBox).Checked = IIf(lDataView.Table.Rows(0)(c) = "N", False, True)
                            End If
                        End Try
                    End If
                End If

            Next

        End If

    End Sub

    Public Shared Sub ManterCamposViaDataViewPorNome(ByVal objPage As Page, ByVal lDataView As Data.DataView, ByVal nomCotrole As String)
        Dim controle As Control
        If lDataView.Table.Rows.Count > 0 Then
            For c As Int32 = 0 To lDataView.Table.Columns.Count - 1
                controle = objPage.Form.FindControl(nomCotrole).FindControl(lDataView.Table.Columns(c).ColumnName)

                If Not controle Is Nothing Then
                    If controle.GetType().FullName = "System.Web.UI.WebControls.TextBox" Then
                        If lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                            CType(controle, TextBox).Text = ""
                        ElseIf lDataView.Table.Columns(c).DataType Is GetType(DateTime) Then
                            CType(controle, TextBox).Text = CType(lDataView.Table.Rows(0)(c), DateTime).ToString("dd/MM/yyyy")
                        ElseIf lDataView.Table.Columns(c).DataType Is GetType(Decimal) Then
                            CType(controle, TextBox).Text = CType(lDataView.Table.Rows(0)(c), Decimal).ToString("0.00")
                        Else
                            CType(controle, TextBox).Text = lDataView.Table.Rows(0)(c)
                        End If
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.Label" Then
                        CType(controle, Label).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCpf" Then
                        '    CType(controle, ixCpf).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCnpj" Then
                        '    CType(controle, ixCnpj).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixTelefone" Then
                        '    CType(controle, ixTelefone).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixData" Then
                        '    If lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                        '        CType(controle, ixData).Text = ""
                        '    Else
                        '        CType(controle, ixData).Text = CType(lDataView.Table.Rows(0)(c), Date).ToString("dd/MM/yyyy")
                        '    End If
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCep" Then
                        '    CType(controle, ixCep).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixInteiro" Then
                        '    CType(controle, ixInteiro).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixMonetario" Then
                        '    CType(controle, ixMonetario).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                        'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixReal" Then
                        '    CType(controle, ixReal).Text = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "", lDataView.Table.Rows(0)(c)).ToString.Trim
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.DropDownList" Then
                        objPage.Session.Add(lDataView.Table.Columns(c).ColumnName, IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, "-1", lDataView.Table.Rows(0)(c)))
                    ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.CheckBox" Then
                        Try
                            CType(controle, CheckBox).Checked = IIf(lDataView.Table.Rows(0)(c) Is DBNull.Value, False, lDataView.Table.Rows(0)(c))
                        Catch ex As Exception
                            If Not lDataView.Table.Rows(0)(c) Is DBNull.Value Then
                                CType(controle, CheckBox).Checked = IIf(lDataView.Table.Rows(0)(c) = "N", False, True)
                            End If
                        End Try
                    End If
                End If

            Next

        End If

    End Sub

    Public Shared Sub MontarDatasetViaForm(ByVal objPage As Page, ByRef lDataSet As Data.DataSet, ByVal valNullTextbox As String, ByVal valNullDropDownList As String, ByVal numCotrole As Int16)

        Dim controle As Control

        For c As Int32 = 0 To lDataSet.Tables(0).Columns.Count - 1
            controle = objPage.Form.Controls.Item(numCotrole).FindControl(lDataSet.Tables(0).Columns(c).ColumnName)

            If Not controle Is Nothing Then
                If controle.GetType().FullName = "System.Web.UI.WebControls.TextBox" Then
                    If CType(controle, TextBox).Text <> valNullTextbox Then
                        lDataSet.Tables(0).Rows(0)(c) = CType(controle, TextBox).Text
                    Else
                        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCpf" Then
                    '    If CType(controle, ixCpf).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCpf).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCnpj" Then
                    '    If CType(controle, ixCnpj).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCnpj).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixTelefone" Then
                    '    If CType(controle, ixTelefone).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixTelefone).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixData" Then
                    '    If CType(controle, ixData).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixData).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCep" Then
                    '    If CType(controle, ixCep).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCep).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixInteiro" Then
                    '    If CType(controle, ixInteiro).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixInteiro).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixMonetario" Then
                    '    If CType(controle, ixMonetario).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixMonetario).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixReal" Then
                    '    If CType(controle, ixReal).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixReal).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.DropDownList" Then
                    If CType(controle, DropDownList).SelectedValue <> valNullDropDownList Then
                        lDataSet.Tables(0).Rows(0)(c) = CType(controle, DropDownList).SelectedValue
                    Else
                        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    End If
                ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.CheckBox" Then
                    lDataSet.Tables(0).Rows(0)(c) = CType(controle, CheckBox).Checked
                End If
            Else
                lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
            End If

        Next

    End Sub

    Public Shared Sub MontarDatasetViaFormPorNome(ByVal objPage As Page, ByRef lDataSet As Data.DataSet, ByVal valNullTextbox As String, ByVal valNullDropDownList As String, ByVal nomCotrole As String)

        Dim controle As Control

        For c As Int32 = 0 To lDataSet.Tables(0).Columns.Count - 1
            controle = objPage.Form.FindControl(nomCotrole).FindControl(lDataSet.Tables(0).Columns(c).ColumnName)
            'controle = objPage.Form.Controls.Item(numCotrole).FindControl(lDataSet.Tables(0).Columns(c).ColumnName)

            If Not controle Is Nothing Then
                If controle.GetType().FullName = "System.Web.UI.WebControls.TextBox" Then
                    If CType(controle, TextBox).Text <> valNullTextbox Then
                        lDataSet.Tables(0).Rows(0)(c) = CType(controle, TextBox).Text
                    Else
                        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCpf" Then
                    '    If CType(controle, ixCpf).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCpf).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCnpj" Then
                    '    If CType(controle, ixCnpj).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCnpj).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixTelefone" Then
                    '    If CType(controle, ixTelefone).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixTelefone).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixData" Then
                    '    If CType(controle, ixData).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixData).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixCep" Then
                    '    If CType(controle, ixCep).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixCep).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixInteiro" Then
                    '    If CType(controle, ixInteiro).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixInteiro).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixMonetario" Then
                    '    If CType(controle, ixMonetario).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixMonetario).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                    'ElseIf controle.GetType().FullName = "INSIX.FW.WebControls.ixReal" Then
                    '    If CType(controle, ixReal).Text <> valNullTextbox Then
                    '        lDataSet.Tables(0).Rows(0)(c) = CType(controle, ixReal).Text
                    '    Else
                    '        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    '    End If
                ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.DropDownList" Then
                    If CType(controle, DropDownList).SelectedValue <> valNullDropDownList Then
                        lDataSet.Tables(0).Rows(0)(c) = CType(controle, DropDownList).SelectedValue
                    Else
                        lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
                    End If
                ElseIf controle.GetType().FullName = "System.Web.UI.WebControls.CheckBox" Then
                    lDataSet.Tables(0).Rows(0)(c) = CType(controle, CheckBox).Checked
                End If
            Else
                lDataSet.Tables(0).Rows(0)(c) = DBNull.Value
            End If

        Next

    End Sub

#End Region

#Region " Criptografia "
    Public Shared Function Criptografa(ByVal Frase As String) As String
        Dim lRetorno As String
        lRetorno = FormsAuthentication.HashPasswordForStoringInConfigFile(Frase, "sha1")
        Return lRetorno
    End Function
#End Region

#Region " Validação de Campos "

    Public Shared Function EhURL(ByVal strValue As String) As Boolean
        Dim regExPattern As String = "^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$"
        Dim validateURL As New Regex(regExPattern)
        Return validateURL.IsMatch(strValue)
    End Function

    'public bool validateURL(string strValue)
    '{
    '    string regExPattern = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
    '    Regex validateURL = new Regex(regExPattern);
    '    return validateURL.IsMatch(strValue);
    '}

#End Region

#Region " CPF "

    Public Shared Function ValidarCPF(ByVal CPF As String) As Boolean

        Dim i, x, n1, n2 As Integer
        Dim dadosArray() As String = {"111.111.111-11", "222.222.222-22", "333.333.333-33", "444.444.444-44", _
                                              "555.555.555-55", "666.666.666-66", "777.777.777-77", "888.888.888-88", "999.999.999-99"}

        CPF = CPF.Trim

        For i = 0 To dadosArray.Length - 1
            If CPF.Length <> 14 Or dadosArray(i).Equals(CPF) Then
                Return False
            End If
        Next

        'remove a maskara
        CPF = CPF.Substring(0, 3) + CPF.Substring(4, 3) + CPF.Substring(8, 3) + CPF.Substring(12)

        For x = 0 To 1
            n1 = 0
            For i = 0 To 8 + x
                n1 = n1 + Val(CPF.Substring(i, 1)) * (10 + x - i)
            Next
            n2 = 11 - (n1 - (Int(n1 / 11) * 11))
            If n2 = 10 Or n2 = 11 Then n2 = 0
            If n2 <> Val(CPF.Substring(9 + x, 1)) Then
                Return False
            End If
        Next

        Return True

    End Function

#End Region

#Region "Texto"

    Public Shared Function FormatarTexto(ByVal msg As String) As String

        Dim c As Char
        Dim strMsg As StringBuilder = New StringBuilder

        msg = Trim(msg)

        Do While msg.Length > 0
            c = Left(msg, 1)
            If Asc(c) = 13 Or Asc(c) = 10 Then
                strMsg.Append("<br>")
            Else
                strMsg.Append(c)
            End If
            msg = Right(msg, msg.Length - 1)
        Loop

        Return strMsg.ToString

    End Function

#End Region


End Class
