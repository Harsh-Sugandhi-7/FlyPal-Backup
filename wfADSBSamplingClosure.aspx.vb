'Created by : Prashant 22-Sep-2022
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public Class wfADSBSamplingClosure
    Inherits System.Web.UI.Page

#Region "Enumaration"
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
    Public mEmployeeListForCombo As EmployeeListForCombo
    Public mADSBTechRecording As ADSBTechRecording
    Public mADSBReviewMeeting As ADSBReviewMeeting
    Public mADSBSamplingClosure As ADSBSamplingClosure
    Dim mUser As User
#End Region

#Region "Helper Methods"
    Private Sub addAttributes()

    End Sub
    Private Sub GetSession()
        mADSBSamplingClosure = Session("mADSBSamplingClosure")
        mADSBTechRecording = Session("mADSBTechRecordingForADSBSamplingClosuregPage")
        mADSBReviewMeeting = Session("mADSBReviewMeetingForADSBSamplingClosuregPage")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBSamplingClosure"
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
        mADSBSamplingClosure.AircraftEngineAPU = txtAircraftEngineAPU.Text.Trim
        mADSBSamplingClosure.AuditConformance = txtAuditConformance.Text.Trim
        mADSBSamplingClosure.Observation = txtObservation.Text.Trim
        mADSBSamplingClosure.NoticesForRectification = txtNoticesForRectification.Text.Trim
        If txtAuditComplianceDate.Text.ToString <> "" Then
            mADSBSamplingClosure.AuditComplianceDate = CDate(txtAuditComplianceDate.Text)
        Else
            mADSBSamplingClosure.AuditComplianceDate = System.DBNull.Value
        End If
        If txtAuditSamplingDate.Text.ToString <> "" Then
            mADSBSamplingClosure.AuditSamplingDate = CDate(txtAuditSamplingDate.Text)
        Else
            mADSBSamplingClosure.AuditSamplingDate = System.DBNull.Value
        End If
        If txtOEMReportingDate.Text.ToString <> "" Then
            mADSBSamplingClosure.OEMReportingDate = CDate(txtOEMReportingDate.Text)
        Else
            mADSBSamplingClosure.OEMReportingDate = System.DBNull.Value
        End If
        If txtLessorReportingDate.Text.ToString <> "" Then
            mADSBSamplingClosure.LessorReportingDate = CDate(txtLessorReportingDate.Text)
        Else
            mADSBSamplingClosure.LessorReportingDate = System.DBNull.Value
        End If
        If txtReCurrentCheckMonitoringDate.Text.ToString <> "" Then
            mADSBSamplingClosure.ReCurrentCheckMonitoringDate = CDate(txtReCurrentCheckMonitoringDate.Text)
        Else
            mADSBSamplingClosure.ReCurrentCheckMonitoringDate = System.DBNull.Value
        End If
        If txtRecordUpdatingDate.Text.ToString <> "" Then
            mADSBSamplingClosure.RecordUpdatingDate = CDate(txtRecordUpdatingDate.Text)
        Else
            mADSBSamplingClosure.RecordUpdatingDate = System.DBNull.Value
        End If
        If txtNAAReporting.Text.ToString <> "" Then
            mADSBSamplingClosure.NAAReporting = CDate(txtNAAReporting.Text)
        Else
            mADSBSamplingClosure.NAAReporting = System.DBNull.Value
        End If
        '''''''AttachMyFile()
        For j As Integer = 0 To mADSBSamplingClosure.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgAttachment.Rows(j).FindControl("txtFileName"), TextBox)
            mADSBSamplingClosure.FileAttachments(j).FileName = txtValue.Text.Trim
        Next
        Session("mADSBSamplingClosure") = mADSBSamplingClosure
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()
            mADSBSamplingClosure.ApplyEdit()
            If mADSBSamplingClosure.IsValid Then
                mADSBSamplingClosure.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            SetPage()

            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
            MarkLog(Util.Action.Save, "ADSBSamplingClosure", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)

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
    Private Sub SetPage()
        'If mADSBSamplingClosure.IsNew = True Then
        '    lblTitle.InnerText = "AD/SB For " + mADSBSamplingClosure.ADSBRecordingText.ToString + " [ NEW ]"
        'Else
        '    lblTitle.InnerText = "AD/SB For " + mADSBSamplingClosure.ADSBRecordingText.ToString + " [" + mADSBSamplingClosure.ADSBNo + "]"
        'End If
        upnlTitle.Update()
    End Sub
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
                        If mADSBSamplingClosure.IsValid = True Then
                            'mADSBSamplingClosure.StatusID = 2
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
                            MarkLog(Util.Action.Authorize, "ADSBSamplingClosure", User.Identity.Name + " Authorized AD/SB : " + ADSBDetail, Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)
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
                        If mADSBSamplingClosure.IsValid = True Then
                            'mADSBSamplingClosure.StatusID = 4
                            DataFieldBind()
                            Save()

                            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
                            MarkLog(Util.Action.Cancel, "ADSBSamplingClosure", User.Identity.Name + " Canceled Invoice : " + ADSBDetail, Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)
                            MSGBoxCtrl.show(MSGBox.Message_title.CanceledSuccessFully, MSGBox.Message_text.CanceledSuccessFully, "", MsgBoxStyle.OkOnly, "")

                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If CustomValidate1() = False Then
                            upnlValidationsummary.Update()
                            Exit Sub
                        End If

                        If Save() Then
                            SetPage()
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
                            MarkLog(Util.Action.Save, "ADSBSamplingClosure", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)
                            Response.Redirect("Index.aspx")
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mADSBSamplingClosure As ADSBSamplingClosure
                            mADSBSamplingClosure = CType(Session("mADSBSamplingClosure"), ADSBSamplingClosure)
                            mADSBSamplingClosure.FileAttachments.Remove(mADSBSamplingClosure.FileAttachments.CurrentItem)
                            dgAttachment.DataSource = mADSBSamplingClosure.FileAttachments
                            dgAttachment.DataBind()
                            upnldgAttachment.Update()
                            upnlAttachment.Update()
                            Session("mADSBSamplingClosure") = mADSBSamplingClosure
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
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
                        Session("mADSBSamplingClosure") = mADSBSamplingClosure
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub AttachMyFile()

        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        mADSBSamplingClosure = Session("mADSBSamplingClosure")
        Try
            If Not mADSBSamplingClosure.FileAttachments.Contains(mADSBSamplingClosure.ID, CType(Session("FileUpload.FileName"), String)) Then

                mADSBSamplingClosure.FileAttachments.Add(mADSBSamplingClosure.ID, CType(Session("FileUpload.FileName"), String))
                mADSBSamplingClosure.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mADSBSamplingClosure.FileAttachments.CurrentItem.Size = Session("Size")
                mADSBSamplingClosure.FileAttachments.CurrentItem.Extension = Session("Extension")
               
                Session("mADSBSamplingClosure") = mADSBSamplingClosure
                dgAttachment.DataSource = mADSBSamplingClosure.FileAttachments
                dgAttachment.DataBind()

                For i As Integer = 0 To mADSBSamplingClosure.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mADSBSamplingClosure.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Else
                Session("mADSBSamplingClosure") = mADSBSamplingClosure
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mADSBSamplingClosure.FileAttachments.CurrentIndex = Index
        Session("mADSBSamplingClosure") = mADSBSamplingClosure
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        If Not mADSBSamplingClosure.AuditComplianceDateFormatted Is System.DBNull.Value Then
            txtAuditComplianceDate.Text = Format(CDate(mADSBSamplingClosure.AuditComplianceDateFormatted), AppSettings("DateFormat"))
        Else
            txtAuditComplianceDate.Text = ""
        End If
        If Not mADSBSamplingClosure.AuditSamplingDateFormatted Is System.DBNull.Value Then
            txtAuditSamplingDate.Text = Format(CDate(mADSBSamplingClosure.AuditSamplingDateFormatted), AppSettings("DateFormat"))
        Else
            txtAuditSamplingDate.Text = ""
        End If
        If Not mADSBSamplingClosure.OEMReportingDateFormatted Is System.DBNull.Value Then
            txtOEMReportingDate.Text = Format(CDate(mADSBSamplingClosure.OEMReportingDateFormatted), AppSettings("DateFormat"))
        Else
            txtOEMReportingDate.Text = ""
        End If
        If Not mADSBSamplingClosure.LessorReportingDateFormatted Is System.DBNull.Value Then
            txtLessorReportingDate.Text = Format(CDate(mADSBSamplingClosure.LessorReportingDateFormatted), AppSettings("DateFormat"))
        Else
            txtLessorReportingDate.Text = ""
        End If
        If Not mADSBSamplingClosure.ReCurrentCheckMonitoringDateFormatted Is System.DBNull.Value Then
            txtReCurrentCheckMonitoringDate.Text = Format(CDate(mADSBSamplingClosure.ReCurrentCheckMonitoringDateFormatted), AppSettings("DateFormat"))
        Else
            txtReCurrentCheckMonitoringDate.Text = ""
        End If
        If Not mADSBSamplingClosure.RecordUpdatingDateFormatted Is System.DBNull.Value Then
            txtRecordUpdatingDate.Text = Format(CDate(mADSBSamplingClosure.RecordUpdatingDateFormatted), AppSettings("DateFormat"))
        Else
            txtRecordUpdatingDate.Text = ""
        End If
        If Not mADSBSamplingClosure.NAAReportingFormatted Is System.DBNull.Value Then
            txtNAAReporting.Text = Format(CDate(mADSBSamplingClosure.NAAReportingFormatted), AppSettings("DateFormat"))
        Else
            txtNAAReporting.Text = ""
        End If
        txtADSBTechRecordingDate.Text = mADSBTechRecording.ADSBDateFormatted
        mEmployeeListForCombo = EmployeeListForCombo.GetEmployeeListForCombo("(All)")
        dgAttachment.DataSource = mADSBSamplingClosure.FileAttachments
        DataBind()
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBSamplingClosure.IsValid = False Then
            For i As Integer = 0 To mADSBSamplingClosure.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBSamplingClosure.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
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
        'If custValidator.ControlToValidate = "txtADSBSamplingClosureDate" Then
        '    If txtADSBSamplingClosureDate.Text = "" Then
        '        custValidator.ErrorMessage = "Select ActualMeetingDateTime."
        '        e.IsValid = False
        '    End If
        'End If
        If custValidator.ControlToValidate = "txtAircraftEngineAPU" Then
            If txtAircraftEngineAPU.Text.Length > 500 Then
                custValidator.ErrorMessage = "Aircraft Engine APU is too long."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtAuditConformance" Then
            If txtAuditConformance.Text.Length > 500 Then
                custValidator.ErrorMessage = "Audit Conformance is too long."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtObservation" Then
            If txtObservation.Text.Length > 500 Then
                custValidator.ErrorMessage = "Observation is too long"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtNoticesForRectification" Then
            If txtNoticesForRectification.Text.Length > 500 Then
                custValidator.ErrorMessage = "Notices For Rectification is too long"
                e.IsValid = False
            End If
        End If
        'If custValidator.ControlToValidate = "txtAMOCDescription" Then
        '    If txtAMOCDescription.Text.Length > 500 Then
        '        custValidator.ErrorMessage = "AMOC Description is too long"
        '        e.IsValid = False
        '    End If
        'End If
        'If custValidator.ControlToValidate = "txtsearch" Then
        '    If txtSearch.Text = "" Then
        '        e.IsValid = False
        '    ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
        '        e.IsValid = False
        '    ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
        '        PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
        '        Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        '        If PartNo = "" Or Description = "" Then
        '            e.IsValid = False
        '        End If
        '    End If
        'End If
    End Sub
#End Region

#Region "Buissness Methods"
    Private Sub ControlVisibility()
        'txtADSBSamplingClosureDate.Enabled = IIf(Not mADSBSamplingClosure.IsNew, False, True)
        'txtADSBSamplingClosureText.Enabled = IIf(Not mADSBSamplingClosure.IsNew, False, True)
        'txtADSBSamplingClosureNo.Enabled = IIf(Not mADSBSamplingClosure.IsNew, False, True)

        'txtADSBNO.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtSubject.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtDescription.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtIssueDate.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtEffectiveDate.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtRevDate.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtRevNo.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtCompliance.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'txtRevChange.Enabled = IIf(mADSBSamplingClosure.StatusID >= 2, False, True)
        'btnCancel.Visible = (Not mADSBSamplingClosure.IsNew) And (mADSBSamplingClosure.StatusID = 2)
        'btnAuthorized.Visible = (Not mADSBSamplingClosure.IsNew) And (mADSBSamplingClosure.StatusID = 1)
        'btnSave.Visible = (Not mADSBSamplingClosure.StatusID >= 2)
        'btnPrint.Visible = (Not mADSBSamplingClosure.IsNew)
        'UpdatePanel()
    End Sub
    Private Sub UpdatePanel()
        'upnlADSBSamplingClosureDetails.Update()
        'upnlStatusName.Update()
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
            SetPage()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

        If CustomValidate1() Then
            If (Not IsInRole(Rights.[New]) And mADSBSamplingClosure.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBSamplingClosure.IsNew) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            If Save() Then
                SetPage()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
                MarkLog(Util.Action.Save, "ADSBSamplingClosure", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)
            End If
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    'Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
    '    If (Not IsInRole(Rights.Authorized)) Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
    '        Exit Sub
    '    End If
    '    If IsValid Then
    '        Session("mADSBSamplingClosure") = mADSBSamplingClosure
    '        MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>AD/SB</strong>", MsgBoxStyle.YesNo, "Status")
    '    End If
    'End Sub
    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    If (Not IsInRole(Rights.Authorized)) Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user to cancel this AD/SB", False), True)
    '        Exit Sub
    '    End If
    '    If IsValid Then
    '        MSGBoxCtrl.show(MSGBox.Message_title.StatusCanceled, MSGBox.Message_text.StatusCanceled, "<Strong> AD/SB </Strong>", MsgBoxStyle.YesNo, "StatusCancel")
    '        Session("mADSBSamplingClosure") = mADSBSamplingClosure
    '    End If
    'End Sub
    '    Private Sub cmbItemList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemList.SelectedIndexChanged
    '        If cmbItemList.SelectedIndex > 0 Then
    '            If mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Count > 1 Then
    '                If mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Contains(New Guid(cmbItemList.SelectedValue)) Then
    '                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not add duplicate participant.", MsgBoxStyle.OkOnly, "")
    '                    Exit Sub
    '                Else
    '                    GoTo Step1
    '                End If
    '            End If
    'Step1:      mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Add(mADSBSamplingClosure.ID, New Guid(cmbItemList.SelectedValue), cmbItemList.SelectedItem.Text, "")
    '            dgItemList.DataSource = mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements
    '            dgItemList.DataBind()
    '            upnlGridView.Update()
    '        End If
    '    End Sub
    'Private Sub dgItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemList.RowCommand
    '    Select Case e.CommandName
    '        Case "DeleteRec"
    '            'Dim mid As Guid = New Guid(dgItemList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
    '            Dim mID As Guid = New Guid(e.CommandArgument.ToString)
    '            mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Remove(mID)
    '            dgItemList.DataSource = mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements
    '            dgItemList.DataBind()
    '            upnlGridView.Update()
    '            upnlButtons.Update()
    '    End Select
    'End Sub
    '    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
    '        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
    '            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
    '            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
    '        Else
    '            PartNo = Trim(txtSearch.Text)
    '            Description = Trim(txtSearch.Text)
    '        End If
    '        If hdnpartId.Value <> String.Empty Then
    '            PartID = hdnpartId.Value.ToString
    '        End If
    '        If Not New Guid(PartID).Equals(Guid.Empty) Then
    '            If mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Count > 1 Then
    '                If mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Contains(New Guid(PartID)) Then
    '                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not add duplicate record.", MsgBoxStyle.OkOnly, "")
    '                    Exit Sub
    '                Else
    '                    GoTo Step1
    '                End If
    '            End If
    'Step1:      mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements.Add(mADSBSamplingClosure.ID, New Guid(PartID), PartNo, Description)
    '            dgItemList.DataSource = mADSBSamplingClosure.ADSBSamplingClosureMaterialRequirements
    '            dgItemList.DataBind()
    '            upnlGridView.Update()
    '            lblInstr.Visible = False
    '        Else
    '            lblInstr.Visible = True
    '        End If
    '    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        setObject()
        Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Actual Meeting Date : " + mADSBSamplingClosure.AuditComplianceDateFormatted
        MarkLog(Util.Action.Close, "ADSBSamplingClosure", ADSBDetail, Util.ErrorType.NoError, mADSBSamplingClosure.ID, EventLogID)
        If mADSBSamplingClosure.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            Response.Redirect("index.aspx")
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        setObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub dgAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mADSBSamplingClosure.FileAttachments
                 If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If
                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                    End If
                End If
                dgAttachment.DataSource = mADSBSamplingClosure.FileAttachments
                dgAttachment.DataBind()
                ControlVisibility()
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgAttachment.PageSize * dgAttachment.PageIndex
                mFileAttachments = mADSBSamplingClosure.FileAttachments
                If mFileAttachments.Count = 1 Then
                    DeleteAttachment(0)
                Else
                    DeleteAttachment(Index - 1)
                End If
        End Select
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlAttachment.Update()
    End Sub
#End Region


End Class