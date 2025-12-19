Imports Flypal
Public Class wfEmpCAAuthorizationHistory_Ajax
    Inherits System.Web.UI.Page

#Region " Enumaration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        FindNow = 7
    End Enum
#End Region

#Region " Variable Declaration "
    Public mEmpCAAuthorization As EmpCAAuthorization
    Public mEmpCAAuthorizationHistoryList As EmpCAAuthorizationHistoryList
    Dim EventLogID As Guid
    Dim mEmpCAAuthorizationDetail As String
    Dim mFileAttach As FileAttach
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEmpCAAuthorization = Session("mEmpCAAuthorization")
        mEmpCAAuthorizationHistoryList = Session("mEmpCAAuthorizationHistoryList")
    End Sub
    Private Sub SetSession()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Session("mEmpCAAuthorizationHistoryList") = mEmpCAAuthorizationHistoryList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mEmpCAAuthorization")
        Session.Remove("mEmpCAAuthorizationHistoryList")
    End Sub

    Private Sub NewRecord()
        mEmpCAAuthorization = EmpCAAuthorization.NewEmpCAAuthorization(New Guid)
        mEmpCAAuthorization.EmpCAAuthorizationDate = Today.Date
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mId)
        mEmpCAAuthorization.MarkClean()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mId)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            Dim mEmpCAAuthorization As EmpCAAuthorization
                            Session("Sender") = ""
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.Delete()
                            mEmpCAAuthorization.Save()
                            DataFieldBind()
                            upnlTitle.Update()
                            upnlGrid.Update()
                        Catch ex As SqlException
                            Dim stringInfo As String = ""
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Message, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'If ex.Message.Contains("FKtabOrdertabMSP") Then
                                '    stringInfo = "Order."
                                'ElseIf ex.Message.Contains("FKtabnWOtabMSP") Then
                                '    stringInfo = "Work Order."
                                'End If
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, stringInfo, MsgBoxStyle.OkOnly, "")
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                mEmpCAAuthorizationDetail = "Authorization No.: " + mEmpCAAuthorization.CANumber + " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Employee: " + mEmpCAAuthorization.EmployeeName + " Code: " + mEmpCAAuthorization.EmployeeCode
                                MarkLog(Util.Action.Delete, "EmpCAAuthorization", mEmpCAAuthorizationDetail, Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        End If
    End Sub
#End Region

#Region " DatafieldBinding "
    Private Sub DataFieldBind()
        mEmpCAAuthorizationHistoryList = EmpCAAuthorizationHistoryList.GetEmpCAAuthorizationHistoryList(mEmpCAAuthorization.EmployeeID, mEmpCAAuthorization.ID, mEmpCAAuthorization.ReferenceID)
        dgEmpCAAuthorizationHistoryList.DataSource = mEmpCAAuthorizationHistoryList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        If Not IsPostBack And Session("sender") = "" Then
            DataFieldBind()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseTop.Click
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub dgEmpCAAuthorizationHistoryList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgEmpCAAuthorizationHistoryList.RowCommand
        Dim mId As New Guid
        Dim Idx As Int32
        Select Case e.CommandName
            Case "DetailView"
                Idx = CInt(e.CommandArgument) + dgEmpCAAuthorizationHistoryList.PageIndex * dgEmpCAAuthorizationHistoryList.PageSize
                mId = New Guid(dgEmpCAAuthorizationHistoryList.DataKeys(Idx).Value.ToString)
                EditRecord(mId)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationDetailsWindow", "OpenEmpCAAuthorizationDetailsWindow()", True)
        End Select
    End Sub
#End Region

End Class