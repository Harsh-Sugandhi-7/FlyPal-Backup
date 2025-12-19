Imports System.Text
Imports System.Linq
Imports System.Linq.Enumerable
Imports System.Collections.Generic
Public Class wfADSBReviewMeeting
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
        Authorized = 8
    End Enum
#End Region

#Region "Variables and Declarations"
    Public mEmployeeListAutoComplete As EmployeeListAutoComplete
    Public mADSBTechRecording As ADSBTechRecording
    Public mADSBReviewMeeting As ADSBReviewMeeting
    Dim mUser As User
#End Region

#Region "Helper Methods"

    Private Sub addAttributes()
        'txtADSBReviewMeetingText.Attributes.Add("onblur", "WaterMark(this, event);")
        'txtADSBReviewMeetingText.Attributes.Add("onfocus", "WaterMark(this, event);")
    End Sub
    Private Sub GetSession()
        mADSBReviewMeeting = Session("mADSBReviewMeeting")
        mADSBTechRecording = Session("mADSBTechRecordingForADSBReviewMeetingPage")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBReviewMeeting"
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub setObject()
        'mADSBReviewMeeting.ReviewDate = Today.Date.ToString
        If txtPlannedMeetingDateTime.Text.ToString <> "" Then
            mADSBReviewMeeting.PlannedMeetingDateTime = CDate(txtPlannedMeetingDateTime.Text)
        Else
            mADSBReviewMeeting.PlannedMeetingDateTime = System.DBNull.Value
        End If
        mADSBReviewMeeting.MeetingLink = txtMeetingLink.Text.Trim
        mADSBReviewMeeting.MeetingLocation = txtLocation.Text.Trim
        If optOnline.Checked = True Then
            mADSBReviewMeeting.IsOnLine = True
        ElseIf optOffline.Checked = True Then
            mADSBReviewMeeting.IsOnLine = False
        End If
        mADSBReviewMeeting.MeetingRequestCreated = 1
        Session("mADSBReviewMeeting") = mADSBReviewMeeting
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()
            mADSBReviewMeeting.ApplyEdit()

            If mADSBReviewMeeting.IsValid Then
                mADSBReviewMeeting.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            'SetPage()

            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted
            MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)

            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2627 Or ex.Number = 2601 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.Information, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            Return False
        End Try
    End Function
    'Private Sub SetPage()
    '    If mADSBReviewMeeting.IsNew = True Then
    '        lblTitle.InnerText = "AD/SB For " + mADSBReviewMeeting.ADSBRecordingText.ToString + " [ NEW ]"
    '    Else
    '        lblTitle.InnerText = "AD/SB For " + mADSBReviewMeeting.ADSBRecordingText.ToString + " [" + mADSBReviewMeeting.ADSBNo + "]"
    '    End If
    '    upnlTitle.Update()
    'End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mADSBReviewMeeting.IsValid = True Then
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted
                            MarkLog(Util.Action.Authorize, "ADSBReviewMeeting", User.Identity.Name + " Authorized AD/SB : " + ADSBDetail, Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)
                            MSGBoxCtrl.show("Authorized!", "Authorized SuccessFully", "", MsgBoxStyle.OkOnly, "")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        If mADSBReviewMeeting.IsValid = True Then
                            'mADSBReviewMeeting.StatusID = 4
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted
                            MarkLog(Util.Action.Cancel, "ADSBReviewMeeting", User.Identity.Name + " Canceled Invoice : " + ADSBDetail, Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)
                            MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")

                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If CustomValidate1() = True Then
                            If Save() Then
                                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                                Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted
                                MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)
                                Response.Redirect("Index.aspx")
                            End If
                        Else
                            upnlValidationsummary.Update()
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mADSBReviewMeeting") = mADSBReviewMeeting
                        DataFieldBind()

                    End If
                Case MsgBoxResult.Ok

            End Select

        End If
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        mEmployeeListAutoComplete = EmployeeListAutoComplete.GetEmployeeList(AddTopItem:="(All)", EmployeeSelectedInUser:=True)
        cmbEmployeeList.DataSource = mEmployeeListAutoComplete
        dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
        txtADSBTechRecordingDate.Text = mADSBTechRecording.ADSBDateFormatted
        txtPlannedMeetingDateTime.Text = mADSBReviewMeeting.PlannedMeetingDateTimeFormatted.ToString
        DataBind()
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBReviewMeeting.IsValid = False Then
            For i As Integer = 0 To mADSBReviewMeeting.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBReviewMeeting.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count = 0 Then
            strMsg = strMsg + "Add Participants" + "<Br>"
        End If
        If hdnValue.Value = "false" Then
            strMsg = strMsg + "Please Enter Valid Metting link" + "<Br>"
        End If
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtPlannedMeetingDateTime" Then
            If txtPlannedMeetingDateTime.Text = "" Then
                custValidator.ErrorMessage = "Planning Date Require."
                e.IsValid = False
            ElseIf txtPlannedMeetingDateTime.Text <> "" And CDate(mADSBTechRecording.IssueDateFormatted) > CDate(txtPlannedMeetingDateTime.Text) Then
                custValidator.ErrorMessage = "Planning Date should be greater than Issue Date"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtLocation" Then
            If (optOffline.Checked = True And txtLocation.Text.Trim = "") Then
                custValidator.ErrorMessage = "Location Required"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtMeetingLink" Then
            If (optOnline.Checked = True And txtMeetingLink.Text.Trim = "") Then
                custValidator.ErrorMessage = "Meeting Link Required"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "cmbEmployeeList" Then
            If cmbEmployeeList.SelectedIndex = 0 And mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count = 0 Then
                custValidator.ErrorMessage = "Add Participants"
                e.IsValid = False
            End If
        End If
    End Sub
#End Region

#Region "Buissness Methods"
    Private Sub ControlVisibility()
        If mADSBReviewMeeting.IsOnLine = False Then
            txtMeetingLink.Text = ""
            If (mADSBTechRecording.ADSBStepsID <= 3) Then
                txtLocation.Enabled = True
            Else
                txtLocation.Enabled = False
            End If
            txtMeetingLink.Enabled = False
            upnlADSBTechRecordingDetails.Update()
        End If
        If mADSBReviewMeeting.IsOnLine = True Then
            txtLocation.Text = ""
            txtLocation.Enabled = False
            If (mADSBTechRecording.ADSBStepsID <= 3) Then
                txtMeetingLink.Enabled = True
            Else
                txtMeetingLink.Enabled = False
            End If
            upnlADSBTechRecordingDetails.Update()
        End If
        dgMeetingParticipantsList.Columns(5).Visible = (mADSBTechRecording.ADSBStepsID <= 3)
    End Sub
     Private Sub UpdatePanel()
        'upnlADSBReviewMeetingDetails.Update()
        upnlTitle.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If Not IsPostBack Then
            DataFieldBind()
            'SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnSetMeeting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetMeeting.Click
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
        If CustomValidate1() Then
            'If (Not IsInRole(Rights.[New]) And mADSBReviewMeeting.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBReviewMeeting.IsNew) Then
            '    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            '    Exit Sub
            'End If
            If Save() Then
                'SetPage()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted
                MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)
                '----------
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub cmbEmployeeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEmployeeList.SelectedIndexChanged
        If cmbEmployeeList.SelectedIndex > 0 Then
            If mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count >= 1 Then
                If mADSBReviewMeeting.ADSBReviewMeetingParticipants.Contains(New Guid(cmbEmployeeList.SelectedValue)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not add duplicate participant.", MsgBoxStyle.OkOnly, "")
                    cmbEmployeeList.ClearSelection()
                    Exit Sub
                Else
                    GoTo Step1
                End If
            End If
Step1:      mADSBReviewMeeting.ADSBReviewMeetingParticipants.Add(mADSBReviewMeeting.ID, New Guid(cmbEmployeeList.SelectedValue), cmbEmployeeList.SelectedItem.Text, _
                                                                 EmployeeEmail:=Employee.GetEmployee(New Guid(cmbEmployeeList.SelectedValue)).Email)
            dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
            dgMeetingParticipantsList.DataBind()
            cmbEmployeeList.ClearSelection()
            upnlGridView.Update()
        End If
    End Sub
    Private Sub optOffline_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optOffline.CheckedChanged
        txtMeetingLink.Text = ""
        txtLocation.Enabled = True
        txtMeetingLink.Enabled = False
        upnlADSBTechRecordingDetails.Update()
    End Sub
    Private Sub optOnline_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optOnline.CheckedChanged
        txtLocation.Text = ""
        txtLocation.Enabled = False
        txtMeetingLink.Enabled = True
        upnlADSBTechRecordingDetails.Update()
    End Sub
    'Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click

    '    If (Not IsInRole(Rights.Authorized)) Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '        Exit Sub
    '    End If

    '    If IsValid Then
    '        Session("mADSBReviewMeeting") = mADSBReviewMeeting
    '        MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>AD/SB</strong>", MsgBoxStyle.YesNo, "Status")
    '    End If
    'End Sub
    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click ''===============================WO - 2006-2007-1-19

    '    If (Not IsInRole(Rights.Authorized)) Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to cancel this AD/SB", False), True)
    '        Exit Sub
    '    End If

    '    If IsValid Then

    '        MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> AD/SB </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
    '        Session("mADSBReviewMeeting") = mADSBReviewMeeting
    '    End If
    'End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        setObject()
        Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted.ToString

        MarkLog(Util.Action.Close, "WOInvoice", ADSBDetail, Util.ErrorType.NoError, mADSBReviewMeeting.ID, EventLogID)


        If mADSBReviewMeeting.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub dgMeetingParticipantsList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMeetingParticipantsList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mADSBReviewMeeting.ADSBReviewMeetingParticipants.Remove(mID)
                dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
                dgMeetingParticipantsList.DataBind()
                upnlGridView.Update()
                btnSendMail.DataBind()
                upnlButtons.Update()
        End Select
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        If (Not User.IsInRole("ADSBReviewMeetingNew")) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'Dim checkString = Request.Form("chkSelect")
        'If checkString Is Nothing Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'End If
        Dim str As String
        Dim mSendMailFile As New SendMailFile
        Dim ToMailIDs As New StringBuilder
        Dim SubScribers As New StringBuilder
        ' we'll need a split to get the individual ids
        Dim mEmployeeForADSBReviewMeetingMail As Employee
        'Dim values = checkString.Split(","c)
        For b As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
            'If mManual.ManualSubscribers.Contains(New Guid(value)) Then
            mEmployeeForADSBReviewMeetingMail = Employee.GetEmployee(mADSBReviewMeeting.ADSBReviewMeetingParticipants(b).EmployeeID)
            If mEmployeeForADSBReviewMeetingMail.Email <> "" Then
                SubScribers.Append(mEmployeeForADSBReviewMeetingMail.EmpNoName + "(" + mEmployeeForADSBReviewMeetingMail.Email + ")" + ",")
                ToMailIDs.Append(mEmployeeForADSBReviewMeetingMail.Email + ",")
            End If
            'End If
        Next

        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Review Board Meeting has been Planned On. " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted.ToString + "</font></P></br> ")
        'str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")

        str = str + ("<p><font face=""Calibri"">")
        str = str + ("<b>" + " Subject: " + "</b>" + mADSBTechRecording.ADSBSubject + "</br><b>Date: " + "</b>" + mADSBTechRecording.ADSBDateFormatted.ToString + "</br><b> No.:</b> " + mADSBTechRecording.ADSBRecordingText + "</br><b>" + " AD/SB No.: " + "</b>" + mADSBTechRecording.ADSBNo)
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")

        'If mManual.Revisions(mManual.Revisions.Count - 1).No = "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo = "" Then
        '    mNoRevNo = ""
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No <> "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo = "" Then
        '    mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).No
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No = "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo <> "" Then
        '    mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).RevNo
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).No <> "" And mManual.Revisions(mManual.Revisions.Count - 1).RevNo <> "" Then
        '    mNoRevNo = "<b>Last Revision No.: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).No + "/ " + mManual.Revisions(mManual.Revisions.Count - 1).RevNo
        'Else
        '    mNoRevNo = ""
        'End If
        str = str + (IIf(mADSBReviewMeeting.IsOnLine = False, "<b>" + " Location: " + "</b>" + IIf(mADSBReviewMeeting.MeetingLocation = "", "", mADSBReviewMeeting.MeetingLocation), "") + IIf(mADSBReviewMeeting.IsOnLine = True, "<b>" + " Meeting Link: " + "</b>" + IIf(mADSBReviewMeeting.MeetingLink = "", "", mADSBReviewMeeting.MeetingLink), ""))
        str = str + ("</font></p>")

        'If mManual.Revisions(mManual.Revisions.Count - 1).Remark = "" And mManual.Revisions(mManual.Revisions.Count - 1).Note = "" Then
        '    mRemarkNote = ""
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark <> "" And mManual.Revisions(mManual.Revisions.Count - 1).Note = "" Then
        '    mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Remark
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark = "" And mManual.Revisions(mManual.Revisions.Count - 1).Note <> "" Then
        '    mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Note
        'ElseIf mManual.Revisions(mManual.Revisions.Count - 1).Remark <> "" And mManual.Revisions(mManual.Revisions.Count - 1).Note <> "" Then
        '    mRemarkNote = "<b>Remark / Note: " + "</b>" + mManual.Revisions(mManual.Revisions.Count - 1).Remark + "/ " + mManual.Revisions(mManual.Revisions.Count - 1).Note
        'Else
        '    mRemarkNote = ""
        'End If

        str = str + ("<p><font face=""Calibri"">")
        'str = str + mRemarkNote
        str = str + ("</font></p>")

        str = str + ("<p><font face=""Calibri"">")
        'str = str + ("<b>Soft Copy Available: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).SoftCopy, "Yes", "No") + "<b>" + " Hard Copy Available: " + "</b>" + IIf(mManual.Revisions(mManual.Revisions.Count - 1).HardCopy, "Yes", "No"))
        str = str + ("</font></p>")

        str = str + ("</body></html>")

        SendMailFile.SendMailFile(, User.Identity.Name, "Review Meeting Notification", Info:=str, ToMailID:=ToMailIDs.ToString.Substring(0, ToMailIDs.Length - 1), Remark:="", ReportGeneratedBy:="")
        Dim mADSBReviewMeetingInfo As String = "Review Meeting Notification sent successfully to " + SubScribers.ToString.TrimEnd(",") + " by " + User.Identity.Name
        mADSBReviewMeeting.MailSendDateTimeUpdate(mADSBReviewMeeting.ID)
        MarkLog(Util.Action.SendMail, "ADSBReviewMeeting", mADSBReviewMeetingInfo, Util.ErrorType.HandledError, mADSBReviewMeeting.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

        mADSBReviewMeeting = ADSBReviewMeeting.GetADSBReviewMeeting(mADSBReviewMeeting.ID.ToString)
        Session("mADSBReviewMeeting") = mADSBReviewMeeting
        dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
        dgMeetingParticipantsList.DataBind()
        upnlGridView.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetPartNoDescriptionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As ItemListAutoComplete
        itemlist = ItemListAutoComplete.GetItemList(prefixText, False)
        If count = 0 Then
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).ToArray
        Else
            Return (From c As ItemListAutoComplete.ItemListAutoCompleteInfo In itemlist
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Item, c.ID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region

End Class