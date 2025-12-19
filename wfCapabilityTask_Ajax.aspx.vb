Public Class wfCapabilityTask_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mCapabilityTask As CapabilityTask
    Public mCapabilityTaskList As CapabilityTaskList
    Public mCapabilityList As CapabilityList
    Dim EventLogID As Guid
    Public mHSNACSList As HSNACSList
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mCapabilityTask = CType(Session("mCapabilityTask"), CapabilityTask)
        mCapabilityTaskList = CType(Session("mCapabilityTaskList"), CapabilityTaskList)
    End Sub
    Private Sub SetSession()
        Session("mCapabilityTask") = mCapabilityTask
        Session("mCapabilityTaskList") = mCapabilityTaskList
    End Sub
    Private Sub NewRecord()
        mCapabilityTask = CapabilityTask.NewCapabilityTask()
        Session("mCapabilityTask") = mCapabilityTask
    End Sub
    Private Sub EditRecord(ByVal mId As Guid)
        mCapabilityTask = CapabilityTask.GetCapabilityTask(mId)
        Session("mCapabilityTask") = mCapabilityTask
    End Sub
    Private Sub DeleteRecord(ByVal mId As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")

        mCapabilityTask = CapabilityTask.GetCapabilityTask(mId)
        Session("mCapabilityTask") = mCapabilityTask
    End Sub
    Private Sub setObject()
        mCapabilityTask.TaskDescription = Trim(txtTaskDescription.Text)
        mCapabilityTask.CapabilityID = Val(cmbCapability.SelectedValue)
        mCapabilityTask.CapabilityName = cmbCapability.SelectedItem.Text
        mCapabilityTask.HSNACSID = New Guid(cmbHSNACSList.SelectedValue)
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Dim CapabilityTaskDet As String = String.Empty
                        Try
                            Session("sender") = ""
                            mCapabilityTask = CType(Session("mCapabilityTask"), CapabilityTask)

                            CapabilityTaskDet = mCapabilityTask.TaskDescription + " Date : " + mCapabilityTask.CapabilityName
                            CapabilityTask.DeleteCapabilityTask(mCapabilityTask.ID)
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.Show("Alert...!!", "Entry cannot be deleted. It is already used in Contract", "", MsgBoxStyle.OkOnly, "")
                                MarkLog(Util.Action.Delete, "CapabilityTask", "Can't delete : " & mCapabilityTask.TaskDescription & " is Currently in use", Util.ErrorType.NoError, mCapabilityTask.ID, EventLogID)
                            End If
                            NewRecord()
                            DataFieldBind()
                            SetTitle()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Util.Action.Delete, "CapabilityTask", CapabilityTaskDet, Util.ErrorType.NoError, mCapabilityTask.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Delete" Then
                        NewRecord()
                        DataFieldBind()
                        SetTitle()
                    End If
                    Session("sender") = ""
                    SetTitle()
                Case MsgBoxResult.Ok
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()

            End Select
        ElseIf Result1 = -1 Then
            DataFieldBind()

        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
            'Session("sender") = ""
            DataFieldBind()
        End If
        upnlCapabilityTask.Update()
    End Sub
    Private Sub SetTitle()
        If mCapabilityTask.IsNew Then
            lbltitle.Text = "Capability Task [New]"
        Else
            If Len(mCapabilityTask.TaskDescription) > 15 Then
                lbltitle.Text = "Capability Task [" & mCapabilityTask.TaskDescription.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Capability Task [" & mCapabilityTask.TaskDescription & "]"
            End If
        End If
        lblResult.Text = "Capability Task List: " & mCapabilityTaskList.Count & " Record(s) Found."
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mCapabilityTaskList = CapabilityTaskList.GetCapabilityTaskList()
        Session("mCapabilityTaskList") = mCapabilityTaskList
        dgCapabilityTask.DataSource = mCapabilityTaskList
        dgCapabilityTask.DataBind() '''''DataBind()

        txtTaskDescription.Text = mCapabilityTask.TaskDescription
        mCapabilityList = CapabilityList.GetCapabilityList("(SELECT)")
        cmbCapability.DataSource = mCapabilityList
        cmbCapability.DataBind()

        mHSNACSList = HSNACSList.GetHSNACSList("", "", "(SELECT)")
        Session("mHSNACSList") = mHSNACSList
        cmbHSNACSList.DataSource = mHSNACSList
        cmbHSNACSList.DataBind()

        upnlCapabilityTask.Update()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValidator As CustomValidator
        CustValidator = CType(s, CustomValidator)
        Dim strMsg As String = ""
        setObject()
        If Not mCapabilityTask.IsValid Then
            For i As Integer = 0 To mCapabilityTask.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mCapabilityTask.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            e.IsValid = False
        End If
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If Not mCapabilityTask.IsValid Then
            For i As Integer = 0 To mCapabilityTask.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mCapabilityTask.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If

        If strMsg.Trim <> "" Then
            cvDesc.ErrorMessage = strMsg
            cvDesc.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack Then
            NewRecord()
            DataFieldBind()
            SetTitle()
            lblHSNACS.Visible = IIf(AppSettings("HSNACSCodeVisibleInCapabilityTaskMaster") = "True", True, False)
            dgCapabilityTask.Columns(3).Visible = IIf(AppSettings("HSNACSCodeVisibleInCapabilityTaskMaster") = "True", True, False)
        End If

    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Util.Action.Close, "CapabilityTask", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Session("sender") = ""
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            Session.Remove("mCapabilityTask")
            Session.Remove("mCapabilityTaskList")
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ''If (Not User.IsInRole("CapabilityTaskNew") And mCapabilityTask.IsNew) Or (Not User.IsInRole("CapabilityTaskEdit") And Not mCapabilityTask.IsNew) Then
        ''    setObject()
        ''    SetSession()
        ''    MarkLog(Util.Action.Save, "CapabilityTask", User.Identity.Name & " is not Authorized User to save " & mCapabilityTask.Description, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        ''    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
        ''    Exit Sub
        ''End If
        If Not IsValid Then Exit Sub

        If CustomValidate1() Then
            Try
                setObject()
                mCapabilityTask.Save()
                MarkLog(Util.Action.Save, "CapabilityTask", mCapabilityTask.TaskDescription, Util.ErrorType.NoError, mCapabilityTask.ID, EventLogID)
                NewRecord()
                DataFieldBind()
                SetSession()
                SetTitle()
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    If InStr(ex.Message, "UK_tabHolidays", CompareMethod.Text) Then
                        MSGBoxCtrl.show("Save Error!", "Duplicate Record", "You are trying to add duplicate.", MsgBoxStyle.OkOnly, "")
                    End If
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgCapabilityTask_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgCapabilityTask.RowCommand
        Dim mId As Guid
        Select Case e.CommandName
            Case "ViewRec"
                mId = New Guid(e.CommandArgument.ToString)
                'Dim mName As String = mCapabilityTaskList(Idx).Description
                'If (Not User.IsInRole("CapabilityTaskView") And Not User.IsInRole("CapabilityTaskEdit")) Then
                '    setObject()
                '    SetSession()
                '    MarkLog(Util.Action.Edit, "CapabilityTask", User.Identity.Name & " is not Authorized User to Edit " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                EditRecord(mId)
                txtTaskDescription.Text = mCapabilityTask.TaskDescription
                cmbCapability.SelectedValue = mCapabilityTask.CapabilityID
                cmbHSNACSList.SelectedValue = mCapabilityTask.HSNACSID.ToString
                SetTitle()

                MarkLog(Util.Action.Edit, "CapabilityTask", mCapabilityTask.TaskDescription, Util.ErrorType.NoError, mCapabilityTask.ID, EventLogID)
                upnlCapabilityTask.Update()
            Case "DeleteRec"
                'Idx = CInt(e.CommandArgument) + dgCapabilityTask.PageIndex * dgCapabilityTask.PageSize
                mId = New Guid(e.CommandArgument.ToString)
                'Dim mName As String = mCapabilityTaskList(Idx).Description
                'If (Not User.IsInRole("CapabilityTaskDelete")) Then
                '    setObject()
                '    SetSession()
                '    MarkLog(Util.Action.Delete, "CapabilityTask", User.Identity.Name & " is not Authorized User to Delete " & mName, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                '    Exit Sub
                'End If
                DeleteRecord(mId)
        End Select
    End Sub
    Private Sub dgCapabilityTask_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgCapabilityTask.PageIndexChanging
        dgCapabilityTask.PageIndex = e.NewPageIndex
        dgCapabilityTask.DataSource = mCapabilityTaskList
        Session("mCapabilityTaskList") = mCapabilityTaskList
        dgCapabilityTask.DataBind()
    End Sub
    Private Sub dgCapabilityTask_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgCapabilityTask.Sorting
        mCapabilityTaskList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mCapabilityTaskList") = mCapabilityTaskList
        dgCapabilityTask.DataSource = mCapabilityTaskList
        dgCapabilityTask.DataBind()
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        NewRecord()
        MarkLog(Util.Action.[New], "AccountHead", "", Util.ErrorType.NoError, mCapabilityTask.ID, EventLogID)
        DataFieldBind()
        SetTitle()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

End Class