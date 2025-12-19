'AJAX Conversion By Vikrant On 02-Jul-2015

Public Class wfMaintenanceProgram_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mMaintenanceProgram As MaintenanceProgram
    Public mMaintenanceProgramList As MaintenanceProgramList
    Dim EventLogID As Guid

#End Region

#Region " Business Methods "

    Private Sub GetSession()
        mMaintenanceProgram = CType(Session("mMaintenanceProgram"), MaintenanceProgram)
        mMaintenanceProgramList = CType(Session("mMaintenanceProgramList"), MaintenanceProgramList)
    End Sub

    Private Sub RemoveSession()
        Session.Remove("mMaintenanceProgram")
        Session.Remove("mMaintenanceProgramList")
    End Sub

    Private Sub NewRecord()
        mMaintenanceProgram = MaintenanceProgram.NewMaintenanceProgram(Guid.NewGuid)
        Session("mMaintenanceProgram") = mMaintenanceProgram
    End Sub

    Private Sub EditRecord(mId As Guid)
        mMaintenanceProgram = MaintenanceProgram.GetMaintenanceProgram(mId)
        Session("mMaintenanceProgram") = mMaintenanceProgram
    End Sub

    Private Sub DeleteRecord(mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mMaintenanceProgram = MaintenanceProgram.GetMaintenanceProgram(mId)
        Session("mMaintenanceProgram") = mMaintenanceProgram
    End Sub

    Private Sub SetObject()
        mMaintenanceProgram.Name = Trim(txtMaintenanceProgramName.Text)
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
                            MaintenanceProgram.DeleteMaintenanceProgram(mMaintenanceProgram.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            upnlSave.Update()

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            End If

                            NewRecord()
                            DataFieldBind()
                            upnlTitle.Update()
                            upnlSave.Update()
                            msgCount = ex.Errors.Count

                        Finally

                            If msgCount = 0 Then
                                MarkLog(Action.Delete,
                                        "Maintenance Program",
                                        mMaintenanceProgram.Name,
                                        ErrorType.NoError,
                                        mMaintenanceProgram.ID,
                                        EventLogID)
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

        If mMaintenanceProgram.IsNew Then
            lbltitle.Text = "Maintenance Program [New]"
        Else

            If Len(mMaintenanceProgram.Name) > 15 Then
                lbltitle.Text = "Maintenance Program [" & mMaintenanceProgram.Name.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Maintenance Program [" & mMaintenanceProgram.Name & "]"
            End If

        End If

        upnlTitle.Update()

    End Sub

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        mMaintenanceProgramList = MaintenanceProgramList.GetMaintenanceProgramList()
        Session("mMaintenanceProgramList") = mMaintenanceProgramList
        dgMaintenanceProgram.DataSource = mMaintenanceProgramList
        DataBind()

        lblSearch.Text = "List of Maintenance Program : " & mMaintenanceProgramList.Count & " Record(s) Found."

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And CType(Session("sender"), String) = "" Then

            If txtMaintenanceProgramName.Enabled = True Then
                txtMaintenanceProgramName.Focus()
            End If
            NewRecord()
            DataFieldBind()
            SetTitle()

        End If

    End Sub

    Private Sub ClosePopUp(sender As Object, e As EventArgs) Handles btnClose.Click

        MarkLog(Action.Close, "Maintenance Program", "", ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        RemoveSession()

        Dim openAs As String = Request.QueryString("Type")

        If openAs IsNot Nothing AndAlso openAs = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If

    End Sub

    Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            If (Not User.IsInRole("MachineNew") And mMaintenanceProgram.IsNew) Or
               (Not User.IsInRole("MachineEdit") And Not mMaintenanceProgram.IsNew) Then

                MarkLog(Action.Save,
                        "Maintenance Program",
                        User.Identity.Name & " is not Authorized User to save " & mMaintenanceProgram.Name,
                        ErrorType.HandledError,
                        Guid.Empty,
                        EventLogID)

                MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                MSGBox.Message_text.Authorization,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            If IsValid Then

                SetObject()
                mMaintenanceProgram.Save()
                MarkLog(Action.Save,
                        "Maintenance Program",
                        mMaintenanceProgram.Name,
                        ErrorType.NoError,
                        mMaintenanceProgram.ID,
                        EventLogID)

                mMaintenanceProgram = MaintenanceProgram.NewMaintenanceProgram(Guid.NewGuid)
                Session("mMaintenanceProgram") = mMaintenanceProgram
                DataFieldBind()
                SetTitle()
                upnlSave.Update()


            End If

        Catch ex As SqlException

            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        End Try

    End Sub

    Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAdd.Click

        MarkLog(Action.[New],
                "Maintenance Program",
                "",
                ErrorType.NoError,
                mMaintenanceProgram.ID,
                EventLogID)

        NewRecord()
        DataFieldBind()

        If txtMaintenanceProgramName.Enabled = True Then
            txtMaintenanceProgramName.Focus()
        End If

        SetTitle()
        upnlSave.Update()

    End Sub

    Private Sub GV_MaintenanceProgram_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles dgMaintenanceProgram.RowCommand

        Dim mId As Guid
        Dim mName As String

        Select Case e.CommandName
            Case "View"

                mId = New Guid(dgMaintenanceProgram.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mName = dgMaintenanceProgram.Rows(CInt(e.CommandArgument)).Cells(1).Text

                If (Not User.IsInRole("MachineView") And Not User.IsInRole("MachineEdit")) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    MarkLog(Action.Edit,
                            "Maintenance Program",
                            User.Identity.Name & " is not Authorized User to edit " & mName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    Exit Sub

                End If

                EditRecord(mId)
                txtMaintenanceProgramName.DataBind()
                txtMaintenanceProgramName.Focus()

                MarkLog(Action.Edit,
                        "Maintenance Program",
                        mMaintenanceProgram.Name,
                        ErrorType.NoError,
                        mMaintenanceProgram.ID,
                        EventLogID)

                SetTitle()

            Case "DeleteRec"

                mId = New Guid(dgMaintenanceProgram.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mName = dgMaintenanceProgram.Rows(CInt(e.CommandArgument)).Cells(1).Text
                If (Not User.IsInRole("MachineDelete")) Then

                    MarkLog(Action.Delete,
                            "Maintenance Program",
                            User.Identity.Name & " is not Authorized User to delete " & mName,
                            ErrorType.HandledError,
                            Guid.Empty,
                            EventLogID)

                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                    MSGBox.Message_text.Authorization,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Exit Sub

                End If

                DeleteRecord(mId)

        End Select

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub GV_MaintenanceProgram_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgMaintenanceProgram.Sorting

        mMaintenanceProgramList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMaintenanceProgramList") = mMaintenanceProgramList
        dgMaintenanceProgram.DataSource = mMaintenanceProgramList
        dgMaintenanceProgram.DataBind()

    End Sub

#End Region

End Class