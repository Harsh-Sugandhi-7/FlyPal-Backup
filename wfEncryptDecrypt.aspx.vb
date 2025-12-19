Public Class wfEncryptDecrypt
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

#End Region

#Region "Business Methods"
    Public Function EncryptData(ByVal plaintext As String) As String
        Dim wrapper As New Simple3Des("FlyPal")
        Dim cipherText As String = wrapper.EncryptData(plaintext)
        Return cipherText
    End Function
    Public Function DecryptData(ByVal plaintext As String) As String
        Dim wrapper As New Simple3Des("FlyPal")
        Dim cipherText As String = wrapper.DecryptData(plaintext)
        Return cipherText
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "UpdateAll" Then
                        'Dim conString1 As String = AppSettings("DB:FlyPal")
                        'Dim conString2 As String = AppSettings("DB:FlyPal")
                        'Dim con1 = New SqlConnection(conString1)
                        'Dim con2 = New SqlConnection(conString2)
                        'Dim cmd1 As New SqlCommand()
                        'Dim cmd2 As New SqlCommand()
                        'Dim tr As SqlTransaction
                        'Try
                        '    con1.Open()
                        '    cmd1.Parameters.Clear()
                        '    cmd1.Connection = con1
                        '    cmd1.CommandText = "select tabMachine.RegNo,tabMachine.IsForInventory from tabMachine"
                        '    cmd1.CommandType = CommandType.Text

                        '    Dim dr1 As New CSLA.Data.SafeDataReader(cmd1.ExecuteReader)

                        '    If dr1.GetString(1) Is Nothing Then
                        '        With dr1
                        '            Try
                        '                con2.Open()
                        '                With cmd2
                        '                    .Connection = con2
                        '                    .CommandType = CommandType.Text
                        '                    .CommandText = "update tabMachine set IsForInventory = '" & EncryptData("False$$" & dr1.GetString(0)) & "' where tabMachine.RegNo = '" & dr1.GetString(0) + "'" 'string should be in Format True$$tabMachine.RegNo/False$$tabMachine.RegNo [NOTE : All characters are case sensitive] 
                        '                    .CommandTimeout = 1000

                        '                    Dim dr2 As New CSLA.Data.SafeDataReader(cmd2.ExecuteReader)

                        '                    dr2.Close()
                        '                End With
                        '            Catch ex As Exception
                        '                Throw ex.GetBaseException
                        '            Finally
                        '                con2.Close()
                        '            End Try

                        '        End With
                        '    End If


                        '    MSGBoxCtrl.show("Message", "IsForInventory status for all Aircraft's updated successfully.", "", MsgBoxStyle.OkOnly, "")
                        'Catch ex As Exception
                        '    tr.Rollback()
                        '    MSGBoxCtrl.show("Error", ex.Message.ToString, ex.InnerException.ToString, MsgBoxStyle.OkOnly, "")
                        'Finally
                        '    con1.Close()
                        'End Try
                        Dim cn As New SqlConnection
                        cn.ConnectionString = AppSettings("DB:FlyPal")
                        Dim cm As New SqlCommand

                        Dim cn1 As New SqlConnection
                        cn1.ConnectionString = AppSettings("DB:FlyPal")
                        Dim cm1 As New SqlCommand

                        Try
                            cn.Open()
                            With cm
                                .Parameters.Clear()

                                .Connection = cn
                                .CommandType = CommandType.Text
                                .CommandText = "select tabMachine.RegNo,tabMachine.IsForInventory from tabMachine"

                                Dim dr As New CSLA.Data.SafeDataReader(.ExecuteReader)

                                Try
                                    While dr.Read()
                                        With dr
                                            If dr.GetString(1) Is Nothing Or dr.GetString(1) = "" Then
                                                cn1.Open()
                                                With cm1
                                                    .Parameters.Clear()
                                                    .Connection = cn1
                                                    .CommandType = CommandType.Text
                                                    .CommandText = "update tabMachine set IsForInventory = '" & EncryptData("False$$" & dr.GetString(0)) & "' where tabMachine.RegNo = '" & dr.GetString(0) + "'" 'string should be in Format True$$tabMachine.RegNo/False$$tabMachine.RegNo [NOTE : All characters are case sensitive] 

                                                    cm1.ExecuteNonQuery()
                                                End With
                                                cn1.Close()
                                            End If
                                            
                                        End With
                                    End While
                                Catch
                                Finally
                                    dr.Close()
                                End Try
                            End With
                        Catch
                        Finally
                            cn.Close()
                        End Try
                        MSGBoxCtrl.show("Message", "", "IsForInventory status for All Aircraft's updated successfully.", MsgBoxStyle.OkOnly, "")
                    ElseIf MSGBoxCtrl.Sender = "UpdateAllNotInUse" Then
                        Dim cn As New SqlConnection
                        cn.ConnectionString = AppSettings("DB:FlyPal")
                        Dim cm As New SqlCommand

                        Dim cn1 As New SqlConnection
                        cn1.ConnectionString = AppSettings("DB:FlyPal")
                        Dim cm1 As New SqlCommand

                        Try
                            cn.Open()
                            With cm
                                .Parameters.Clear()

                                .Connection = cn
                                .CommandType = CommandType.Text
                                .CommandText = "select tabMachine.RegNo,tabMachine.HourType,tabMachine.NotInUse,tabMachine.NotInUseDate,tabMachine.IsReadOnly,tabMachine.ReadOnlyDate from tabMachine"

                                Dim dr As New CSLA.Data.SafeDataReader(.ExecuteReader)

                                Try
                                    While dr.Read()
                                        With dr
                                            'If dr.GetString(1) Is Nothing Or dr.GetString(1) = "" Then
                                            cn1.Open()
                                            With cm1
                                                .Parameters.Clear()
                                                .Connection = cn1
                                                .CommandType = CommandType.Text
                                                .CommandText = "update tabMachine set NIUDContext = " & New Period(2, dr.GetSmartDate(3).ToString, 0, True, False, dr.GetInt32(1)).DbValueDec.ToString & ",RODContext = " & New Period(2, dr.GetSmartDate(5).ToString, 0, True, False, dr.GetInt32(1)).DbValueDec.ToString & " where tabMachine.RegNo = '" & dr.GetString(0) + "'"

                                                cm1.ExecuteNonQuery()
                                            End With
                                            cn1.Close()
                                            'End If

                                        End With
                                    End While
                                Catch
                                Finally
                                    dr.Close()
                                End Try
                            End With
                        Catch
                        Finally
                            cn.Close()
                        End Try
                        MSGBoxCtrl.show("Message", "", "Not In Use and ReadOnly status for All Aircraft's updated successfully.", MsgBoxStyle.OkOnly, "")
                    End If
                Case MsgBoxResult.No
                  
                Case MsgBoxResult.Ok

            End Select
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub imgDecrypt_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgDecrypt.Click
        If IsValid Then
            txtPlainText.Text = DecryptData(Trim(txtEncryptedText.Text))
        End If
    End Sub
    Private Sub imgEncrypt_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgEncrypt.Click
        If IsValid Then
            txtEncryptedText.Text = EncryptData(Trim(txtPlainText.Text))
        End If
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        'MSGBoxCtrl.show("Save Alert!", "IsForInventory status for all Aircraft's updated will get Updated.<BR><BR>", "Do you want to continue ?", MsgBoxStyle.YesNo, "Update")
        Dim conString As String = AppSettings("DB:FlyPal")
        Dim con = New SqlConnection(conString)
        Dim cmd As New SqlCommand()
        Dim tr As SqlTransaction
        Try
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "update tabMachine set IsForInventory = '" & txtEncryptedText.Text & "' where tabMachine.RegNo = '" & Trim(txtPlainText.Text).Split("$$")(2) + "'" 'string should be in Format True$$tabMachine.RegNo/False$$tabMachine.RegNo [NOTE : All characters are case sensitive] 
            cmd.CommandType = CommandType.Text
            cmd.ExecuteNonQuery()
            MSGBoxCtrl.show("Message", "", "IsForInventory status for <b>'" & Trim(txtPlainText.Text).Split("$$")(2) & "'</b> updated successfully.", MsgBoxStyle.OkOnly, "")
        Catch ex As Exception
            tr.Rollback()
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        'Added by vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
    End Sub
    Protected Sub btnUpdateAllMachines_Click(sender As Object, e As EventArgs) Handles btnUpdateAllMachines.Click
        MSGBoxCtrl.show("Save Alert!", "IsForInventory status for all Aircraft's will get Updated.<BR>", "Do you want to continue ?", MsgBoxStyle.YesNo, "UpdateAll")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub btnUpdateNotInUseReadOnlyStatusOfAllMachines_Click(sender As Object, e As System.EventArgs) Handles btnUpdateNotInUseReadOnlyStatusOfAllMachines.Click
        MSGBoxCtrl.show("Save Alert!", "Not In Use and ReadOnly status for all Aircraft's will get Updated.<BR>", "Do you want to continue ?", MsgBoxStyle.YesNo, "UpdateAllNotInUse")
    End Sub
#End Region
    
    
   
   
    
End Class