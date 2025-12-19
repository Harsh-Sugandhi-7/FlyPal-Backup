'AJAX Conversion By Vikrant On 03-Jul-2015

Public Class wfProgramType_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mProgramType As ProgramType
    Public mProgramTypeList As ProgramTypeList
    Dim EventLogID As Guid
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mProgramType = CType(Session("mProgramType"), ProgramType)
        mProgramTypeList = CType(Session("mProgramTypeList"), ProgramTypeList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mProgramType")
        Session.Remove("mProgramTypeList")
    End Sub
    Private Sub NewRecord()
        mProgramType = ProgramType.NewProgramType(Guid.NewGuid)
        Session("mProgramType") = mProgramType
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mProgramType = ProgramType.GetProgramType(mId)
        Session("mProgramType") = mProgramType
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mProgramType = ProgramType.GetProgramType(mId)
        Session("mProgramType") = mProgramType
    End Sub
    Private Sub setObject()
        mProgramType.Name = Trim(txtProgramTypeName.Text)
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
                            Session("sender") = ""
                            ' mProgramType = CType(Session("mProgramType"), ProgramType)
                            ProgramType.DeleteProgramType(mProgramType.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlSave.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Util.Action.Delete, "Program Type", "Can't delete : " & mProgramType.Name & " is Currently in use", Util.ErrorType.NoError, mProgramType.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecord()
                            DataFieldBind()
                            upnlTitle.Update()
                            upnlSave.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "Program Type", mProgramType.Name, Util.ErrorType.NoError, mProgramType.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub SetTitle()
        If mProgramType.IsNew Then
            lbltitle.Text = "Program Type [New]"
        Else
            If Len(mProgramType.Name) > 15 Then
                lbltitle.Text = "Program Type [" & mProgramType.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Program Type [" & mProgramType.Name & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mProgramTypeList = ProgramTypeList.GetProgramTypeList()
        Session("mProgramTypeList") = mProgramTypeList
        dgProgramType.DataSource = mProgramTypeList
        DataBind()
        'dgProgramType.DataBind()
        lblSearch.Text = "List of Program Type : " & mProgramTypeList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtProgramTypeName.Enabled = True Then
                txtProgramTypeName.Focus()
            End If
            NewRecord()
            DataFieldBind()
            SetTitle()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "Program Type", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(Request.QueryString("GChildPage") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("MachineNew") And mProgramType.IsNew) Or (Not User.IsInRole("MachineEdit") And Not mProgramType.IsNew) Then
            MarkLog(Util.Action.Save, "Program Type", User.Identity.Name & " is not Authorized User to save " & mProgramType.Name, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            If IsValid Then
                setObject()
                mProgramType.Save()
                MarkLog(Util.Action.Save, "Program Type", mProgramType.Name, Util.ErrorType.NoError, mProgramType.ID, EventLogID)
                mProgramType = ProgramType.NewProgramType(Guid.NewGuid)
                Session("mProgramType") = mProgramType
                DataFieldBind()
                SetTitle()
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            End If
        End Try
    End Sub
    Private Sub dgProgramType_RowCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgProgramType.RowCommand
        Dim mId As Guid
        Dim mName As String
        Select Case e.CommandName
            Case "View"
                mId = New Guid(dgProgramType.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mName = dgProgramType.Rows(CInt(e.CommandArgument)).Cells(1).Text
                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then
                    MarkLog(Util.Action.Edit, "Program Type", User.Identity.Name & " is not Authorized User to edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                     MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                EditRecord(mId)
                txtProgramTypeName.DataBind()
                txtProgramTypeName.Focus()
                MarkLog(Util.Action.Edit, "Program Type", mProgramType.Name, Util.ErrorType.NoError, mProgramType.ID, EventLogID)
                SetTitle()
            Case "DeleteRec"
                mId = New Guid(dgProgramType.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mName = dgProgramType.Rows(CInt(e.CommandArgument)).Cells(1).Text
                If (Not User.IsInRole("MachineDelete")) Then
                    MarkLog(Util.Action.Delete, "Program Type", User.Identity.Name & " is not Authorized User to delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        MarkLog(Util.Action.[New], "Program Type", "", Util.ErrorType.NoError, mProgramType.ID, EventLogID)
        NewRecord()
        DataFieldBind()
        If txtProgramTypeName.Enabled = True Then
            txtProgramTypeName.Focus()
        End If
        SetTitle()
    End Sub
    Private Sub dgProgramType_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgProgramType.Sorting
        mProgramTypeList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mProgramTypeList") = mProgramTypeList
        dgProgramType.DataSource = mProgramTypeList
        dgProgramType.DataBind()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class