Public Class wfRootCause_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mRootCause As RootCause
    Public mRootCauseList As RootCauseList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mRootCause = CType(Session("mRootCause"), RootCause)
        mRootCauseList = CType(Session("mRootCauseList"), RootCauseList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRootCause")
        Session.Remove("mRootCauseList")
    End Sub
    Private Sub NewRecord()
        mRootCause = RootCause.NewRootCause()
        Session("mRootCause") = mRootCause
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mRootCause = RootCause.GetRootCause(mId)
        Session("mRootCause") = mRootCause
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mRootCause = RootCause.GetRootCause(mId)
        Session("mRootCause") = mRootCause
    End Sub
    Private Sub setObject()
        mRootCause.RootCause = Trim(txtRootCause.Text)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim mRootCauseName As String
                        Try
                            Session("sender") = ""
                            mRootCause = CType(Session("mRootCause"), RootCause)
                            mRootCauseName = mRootCause.RootCause
                            RootCause.DeleteRootCause(mRootCause.ID)
                            NewRecord()
                            DataFieldBind()
                            txtRootCause.Text = ""
                            SetTitle()
                            upnlRootCause.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDeleting, MSGBox.Message_text.ReferenceDeleting, " Findings", MsgBoxStyle.OkOnly, "")
                                NewRecord()
                                DataFieldBind()
                                SetTitle()
                                upnlRootCause.Update()
                            End If
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "RootCause", mRootCauseName, Flypal.Util.ErrorType.NoError, mRootCause.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetTitle()
        If mRootCause.IsNew Then
            lbltitle.Text = "Root Cause [New]"
        Else
            If Len(mRootCause.RootCause) > 15 Then
                lbltitle.Text = "Root Cause [" & mRootCause.RootCause.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Root Cause [" & mRootCause.RootCause & "]"
            End If
        End If
        lblResult.Text = "Root Cause List: " & mRootCauseList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mRootCauseList = RootCauseList.GetRootCauseList()
        Session("mRootCauseList") = mRootCauseList
        dgRootCauseList.DataSource = mRootCauseList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            If txtRootCause.Enabled = True Then
                txtRootCause.Focus()
            End If
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Flypal.Util.Action.Close, "RootCause", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            setObject()
            mRootCause.Save()
            MarkLog(Flypal.Util.Action.Save, "RootCause", mRootCause.RootCause, Flypal.Util.ErrorType.HandledError, mRootCause.ID, EventLogID)
            NewRecord()
            DataFieldBind()
            SetTitle()
            If txtRootCause.Enabled Then
                txtRootCause.Focus()
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub dgRootCauseList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRootCauseList.RowCommand
        Select Case e.CommandName
            Case "EditRec"
                Dim mId As Guid = mRootCauseList(CInt(e.CommandArgument)).ID
                EditRecord(mId)
                txtRootCause.DataBind()
                SetTitle()
                If txtRootCause.Enabled Then
                    txtRootCause.Focus()
                End If
                MarkLog(Flypal.Util.Action.Edit, "RootCause", mRootCause.RootCause, Flypal.Util.ErrorType.NoError, mRootCause.ID, EventLogID)
            Case "DeleteRec"
                Dim mId As Guid = mRootCauseList(CInt(e.CommandArgument)).ID
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Flypal.Util.Action.[New], "RootCause", "", Flypal.Util.ErrorType.NoError, mRootCause.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtRootCause.Enabled Then
            txtRootCause.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub dgRootCauseList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgRootCauseList.Sorting
        mRootCauseList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mRootCauseList") = mRootCauseList
        dgRootCauseList.DataSource = mRootCauseList
        dgRootCauseList.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class