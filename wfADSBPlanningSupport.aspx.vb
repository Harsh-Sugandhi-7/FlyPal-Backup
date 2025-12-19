'Created by : Prashant 13-Sep-2022
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Public Class wfADSBPlanningSupport
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
    Public mEmployeeListAutoComplete As EmployeeListAutoComplete
    Public mADSBTechRecording As ADSBTechRecording
    Public mADSBReviewMeeting As ADSBReviewMeeting
    Public mADSBPlanningSupport As ADSBPlanningSupport
    Public mADSBVerificationList As ADSBVerificationList
    Public mADSBComplianceDuringList As ADSBComplianceDuringList
    Public mADSBFacilityList As ADSBFacilityList
     Public PartID As String = "{00000000-0000-0000-0000-000000000000}"
    Public PartNo As String = ""
    Public Description As String = ""
    Dim mUser As User
    Dim ADSBPlanningSupportOpenFrom As Integer = 0
    Dim myAttchedFileName As StringBuilder = New StringBuilder
#End Region

#Region "Helper Methods"
    Private Sub addAttributes()
        txtLabourCost.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLabourCost').value,event)")
        txtMaterialCost.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMaterialCost').value,event)")
        txtFacilityCost.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFacilityCost').value,event)")
        txtSubContratctedCost.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtSubContratctedCost').value,event)")
    End Sub
    Private Sub GetSession()
        mADSBPlanningSupport = Session("mADSBPlanningSupport")
        mADSBTechRecording = Session("mADSBTechRecordingForADSBPlanningSupportgPage")
        mADSBReviewMeeting = Session("mADSBReviewMeetingForADSBPlanningSupportgPage")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        IsInRoleString = "ADSBPlanningSupport"
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
        If txtADSBPlanningSupportDate.Text.ToString <> "" Then
            mADSBPlanningSupport.ActualMeetingDateTime = CDate(txtADSBPlanningSupportDate.Text)
        Else
            mADSBPlanningSupport.ActualMeetingDateTime = System.DBNull.Value
        End If
        mADSBPlanningSupport.RiskIdentification = txtRiskIdentification.Text.Trim
        mADSBPlanningSupport.VerificationID = CInt(cmbVerification.SelectedValue)
        mADSBPlanningSupport.ComplianceDuring = CInt(cmbComplianceDuring.SelectedValue)
        mADSBPlanningSupport.ComplianceDuringOthersRemark = txtComplianceDuringOthersRemark.Text.Trim
        mADSBPlanningSupport.AreaOfConcern = txtAreaOfConcern.Text.Trim
        mADSBPlanningSupport.IsAMOCInvokingRequired = chkIsAMOCInvokingRequired.Checked
        mADSBPlanningSupport.AMOCDescription = txtAMOCDescription.Text.Trim
        mADSBPlanningSupport.OEMDesignateMemberID = New Guid(cmbOEMDesignateMember.SelectedValue)
        mADSBPlanningSupport.OEMDesignateMember = cmbOEMDesignateMember.SelectedItem.ToString
        mADSBPlanningSupport.NAADesignateMemberID = New Guid(cmbNAADesignateMember.SelectedValue)
        mADSBPlanningSupport.NAADesignateMember = cmbNAADesignateMember.SelectedItem.ToString
        mADSBPlanningSupport.OtherDesignateMemberID = New Guid(cmbOtherDesignateMember.SelectedValue)
        mADSBPlanningSupport.OtherDesignateMember = cmbOtherDesignateMember.SelectedItem.ToString
        mADSBPlanningSupport.OtherDesignateMemberData = txtOtherDesignateMemberData.Text.Trim
        mADSBPlanningSupport.TechPubRecordImplications = txtTechPubRecordImplications.Text.Trim
        mADSBPlanningSupport.IsAuditRequired = chkIsAuditRequired.Checked
        mADSBPlanningSupport.AuditDescription = txtAuditDescription.Text
        mADSBPlanningSupport.FacilityID = CInt(cmbFacility.SelectedValue)
        mADSBPlanningSupport.LabourCost = txtLabourCost.Text.Trim
        mADSBPlanningSupport.MaterialCost = txtMaterialCost.Text.Trim
        mADSBPlanningSupport.FacilityCost = txtFacilityCost.Text.Trim
        mADSBPlanningSupport.SubContratctedCost = txtSubContratctedCost.Text.Trim
        mADSBPlanningSupport.VerificationName = cmbVerification.SelectedItem.Text
        mADSBPlanningSupport.ComplianceDuringName = cmbComplianceDuring.SelectedItem.Text
        mADSBPlanningSupport.FacilityName = cmbFacility.SelectedItem.Text

        '''''''AttachMyFile()
        For j As Integer = 0 To mADSBPlanningSupport.FileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgAttachment.Rows(j).FindControl("txtFileName"), TextBox)
            mADSBPlanningSupport.FileAttachments(j).FileName = txtValue.Text.Trim
        Next

        For j As Integer = 0 To mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Count - 1
            Dim txtQty As TextBox
            Dim txtLeadTime As TextBox
            txtQty = CType(Me.dgItemList.Rows(j).FindControl("txtQty"), TextBox)
            mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(j).Qty = CDec(Val(txtQty.Text))

            txtLeadTime = CType(Me.dgItemList.Rows(j).FindControl("txtLeadTime"), TextBox)
            mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(j).LeadTime = CDec(Val(txtLeadTime.Text))
        Next

        For k As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
            Dim txtCompliancePeriodInMeeting As TextBox
            Dim txtRemarkInMeeting As TextBox
            txtCompliancePeriodInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtCompliancePeriodInMeeting"), TextBox)
            mADSBTechRecording.ADSBTechRecordingApplicableOns(k).CompliancePeriodInMeeting = txtCompliancePeriodInMeeting.Text.Trim

            txtRemarkInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtRemarkInMeeting"), TextBox)
            mADSBTechRecording.ADSBTechRecordingApplicableOns(k).RemarkInMeeting = txtRemarkInMeeting.Text.Trim
        Next

        Session("mADSBPlanningSupport") = mADSBPlanningSupport
        Session("mADSBTechRecording") = mADSBTechRecording
    End Sub
    Private Sub setObjectForADSBReviewMeeting()

        For k As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
            Dim txtApprovedRemark As TextBox
            Dim rdbApproved As RadioButton
            Dim rdbNotApproved As RadioButton
            txtApprovedRemark = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("txtApprovedRemark"), TextBox)
            rdbApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbApproved"), RadioButton)
            rdbNotApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbNotApproved"), RadioButton)
            If rdbApproved.Checked = True Then
                mADSBReviewMeeting.ADSBReviewMeetingParticipants(k).ApprovedStatus = 1
            ElseIf rdbNotApproved.Checked = True Then
                mADSBReviewMeeting.ADSBReviewMeetingParticipants(k).ApprovedStatus = 2
            End If
            mADSBReviewMeeting.ADSBReviewMeetingParticipants(k).ApprovedRemark = txtApprovedRemark.Text.Trim
        Next

        Session("mADSBReviewMeeting") = mADSBReviewMeeting
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()
            If mADSBPlanningSupport.FileAttachments.Count > 1 Then
                Dim FileNameWiseItem = (From c In mADSBPlanningSupport.FileAttachments
                               Where c.FileName <> "" _
                             Group By FileName = c.FileName Into Group
                             Select New With {.FileName = FileName, .ReceiptItemCollection = Group, .InstanceCount = Group.Count()})

                Dim FileNameCount
                For Each FileNameCount In FileNameWiseItem
                    If FileNameCount.InstanceCount > 1 Then
                        '' MSGBoxCtrl.show("Alert!", "Can Not Save/Authorized !" + " <BR> Attached File Name " + FileNameCount.FileName + "Same.", "", MsgBoxStyle.OkOnly, "")
                        MSGBoxCtrl.show("Duplicate Alert!", "You are trying to add same filename. Only unique filename is allowed", "", MsgBoxStyle.OkOnly, "")
                        Return False
                        Exit Function
                    End If
                Next
            End If
            mADSBPlanningSupport.ApplyEdit()
            If mADSBPlanningSupport.IsValid Then
                mADSBPlanningSupport.Save()
            Else
                upnlValidationsummary.Update()
            End If
            DataFieldBind()
            ControlVisibility()
            SetPage()
            'Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
            'MarkLog(Util.Action.Save, "ADSBPlanningSupport", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
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
                        If mADSBPlanningSupport.IsValid = True Then
                            mADSBPlanningSupport.StatusID = 2
                            mADSBPlanningSupport.StatusName = "Authorized"
                            Save()
                            'If Save() = True Then
                            '    'If ADSBPlanningSupportOpenFrom = 1 Then  '1 means from  Planned command
                            '    '    mADSBTechRecording.ADSBStepsID = 4
                            '    '    mADSBTechRecording.Save()
                            '    'End If
                            'End If
                            UpdatePanel()
                            Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
                            MarkLog(Util.Action.Authorize, "ADSBPlanningSupport", User.Identity.Name + " Authorized AD/SB Planning : " + ADSBDetail, Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
                            MSGBoxCtrl.show("Authorized!", "Authorized SuccessFully", "", MsgBoxStyle.OkOnly, "")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                            If CustomValidate2() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "StatusCancel" Then
                        Session("sender") = ""
                        If mADSBPlanningSupport.IsValid = True Then
                            DataFieldBind()
                            Save()
                            Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
                            MarkLog(Util.Action.Cancel, "ADSBPlanningSupport", User.Identity.Name + " Canceled Invoice : " + ADSBDetail, Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
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
                        If CustomValidate2() = False Then
                            upnlValidationsummary.Update()
                            Exit Sub
                        End If
                        If Save() Then
                            SetPage()
                            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                            Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
                            MarkLog(Util.Action.Save, "ADSBPlanningSupport", User.Identity.Name + " Saved Invoice : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
                            Response.Redirect("Index.aspx")
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "CloseADSBReviewMeeting" Then
                        If CustomValidate3() = False Then
                            upnlValidationsummary.Update()
                            Exit Sub
                        End If
                        mADSBReviewMeeting.Save()
                        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                        Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted + " Review Meeting Saved From Approval Link "
                        MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD SB Review Meeting : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
                        Response.Redirect("Index.aspx")
                    End If
                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mADSBPlanningSupport As ADSBPlanningSupport
                            mADSBPlanningSupport = CType(Session("mADSBPlanningSupport"), ADSBPlanningSupport)
                            mADSBPlanningSupport.FileAttachments.Remove(mADSBPlanningSupport.FileAttachments.CurrentItem)
                            mADSBPlanningSupport.TempStatusID = 0
                            mADSBPlanningSupport.TempStatusID = 1
                            dgAttachment.DataSource = mADSBPlanningSupport.FileAttachments
                            dgAttachment.DataBind()
                            upnldgAttachment.Update()
                            upnlAttachment.Update()
                            Session("mADSBPlanningSupport") = mADSBPlanningSupport
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
                    If MSGBoxCtrl.Sender = "CloseADSBReviewMeeting" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If
                    If (MSGBoxCtrl.Sender = "Status" Or MSGBoxCtrl.Sender = "StatusCancel") Then
                        Session("Sender") = ""
                        Session.Remove("IsValid")
                        Session("mADSBPlanningSupport") = mADSBPlanningSupport
                        DataFieldBind()
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub AttachMyFile()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        mADSBPlanningSupport = Session("mADSBPlanningSupport")
        Try
            If Not mADSBPlanningSupport.FileAttachments.Contains(mADSBPlanningSupport.ID, CType(Session("FileUpload.FileName"), String)) Then
                mADSBPlanningSupport.FileAttachments.Add(mADSBPlanningSupport.ID, CType(Session("FileUpload.FileName"), String))
                mADSBPlanningSupport.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mADSBPlanningSupport.FileAttachments.CurrentItem.Size = Session("Size")
                mADSBPlanningSupport.FileAttachments.CurrentItem.Extension = Session("Extension")
                Session("mADSBPlanningSupport") = mADSBPlanningSupport
                dgAttachment.DataSource = mADSBPlanningSupport.FileAttachments
                dgAttachment.DataBind()

                For i As Integer = 0 To mADSBPlanningSupport.FileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mADSBPlanningSupport.FileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Else
                Session("mADSBPlanningSupport") = mADSBPlanningSupport
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mADSBPlanningSupport.FileAttachments.CurrentIndex = Index
        Session("mADSBPlanningSupport") = mADSBPlanningSupport
    End Sub
#End Region

#Region "Data Binding"
    Private Sub DataFieldBind()
        If Not mADSBPlanningSupport.ActualMeetingDateTimeFormatted Is System.DBNull.Value Then
            txtADSBPlanningSupportDate.Text = Format(CDate(mADSBPlanningSupport.ActualMeetingDateTimeFormatted), AppSettings("DateFormat"))
        Else
            txtADSBPlanningSupportDate.Text = ""
        End If
        txtADSBTechRecordingDate.Text = mADSBTechRecording.ADSBDateFormatted
        mEmployeeListAutoComplete = EmployeeListAutoComplete.GetEmployeeList(AddTopItem:="(All)")
        cmbOEMDesignateMember.DataSource = mEmployeeListAutoComplete
        cmbNAADesignateMember.DataSource = mEmployeeListAutoComplete
        cmbOtherDesignateMember.DataSource = mEmployeeListAutoComplete
        mADSBVerificationList = ADSBVerificationList.GetADSBVerificationList("(SELECT)")
        cmbVerification.DataSource = mADSBVerificationList
        mADSBComplianceDuringList = ADSBComplianceDuringList.GetADSBComplianceDuringList("(SELECT)")
        cmbComplianceDuring.DataSource = mADSBComplianceDuringList
        mADSBFacilityList = ADSBFacilityList.GetADSBFacilityList("(SELECT)")
        cmbFacility.DataSource = mADSBFacilityList
        dgItemList.DataSource = mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements
        dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
        dgEffectivityDetails.DataSource = mADSBTechRecording.ADSBTechRecordingApplicableOns
        dgAttachment.DataSource = mADSBPlanningSupport.FileAttachments
        DataBind()
        dgEffectivityDetails.Columns(0).Visible = mADSBTechRecording.ApplicableToModel
        dgEffectivityDetails.Columns(1).Visible = mADSBTechRecording.ApplicableToPart

        'If ADSBPlanningSupportOpenFrom = 2 Then
        '    For k As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
        '        Dim rdbApproved As RadioButton
        '        Dim rdbNotApproved As RadioButton
        '        rdbApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbApproved"), RadioButton)
        '        rdbNotApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbNotApproved"), RadioButton)
        '        If mADSBReviewMeeting.ADSBReviewMeetingParticipants(k).ApprovedStatus = 1 Then
        '            rdbApproved.Checked = True
        '        ElseIf mADSBReviewMeeting.ADSBReviewMeetingParticipants(k).ApprovedStatus = 2 Then
        '            rdbNotApproved.Checked = True
        '        End If
        '    Next
        'End If
    End Sub
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        setObject()
        If mADSBPlanningSupport.IsValid = False Then
            For i As Integer = 0 To mADSBPlanningSupport.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mADSBPlanningSupport.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        
        Dim mADSBPlanningSupportMaterialRequirement As ADSBPlanningSupportMaterialRequirement
        If Not mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.IsValid Then
            For Each mADSBPlanningSupportMaterialRequirement In mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements
                For i As Integer = 0 To mADSBPlanningSupportMaterialRequirement.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mADSBPlanningSupportMaterialRequirement.ItemName + " : " + mADSBPlanningSupportMaterialRequirement.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Function CustomValidate2() As Boolean
        Dim strMsg As String = ""
        setObject()
        For k As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
            Dim txtCompliancePeriodInMeeting As TextBox
            Dim txtRemarkInMeeting As TextBox
            txtCompliancePeriodInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtCompliancePeriodInMeeting"), TextBox)
            txtRemarkInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtRemarkInMeeting"), TextBox)
            If txtCompliancePeriodInMeeting.Text = "" And txtRemarkInMeeting.Text = "" Then
                strMsg = "Add Compliance Period" + "<Br>" + "Enter Remark in column"
            ElseIf txtCompliancePeriodInMeeting.Text <> "" And txtRemarkInMeeting.Text = "" Then
                strMsg = "Enter Remark in column"
            ElseIf txtCompliancePeriodInMeeting.Text = "" And txtRemarkInMeeting.Text <> "" Then
                strMsg = "Add Compliance Period"
            ElseIf txtCompliancePeriodInMeeting.Text = "" And txtRemarkInMeeting.Text = "" Then
                strMsg = ""
            End If
        Next
        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Function CustomValidate3() As Boolean
        Dim strMsg As String = ""
        setObject()
        For k As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
            Dim txtApprovedRemark As TextBox
            Dim rdbApproved As RadioButton
            Dim rdbNotApproved As RadioButton
            txtApprovedRemark = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("txtApprovedRemark"), TextBox)
            rdbApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbApproved"), RadioButton)
            rdbNotApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbNotApproved"), RadioButton)
            If txtApprovedRemark.Text = "" And (rdbApproved.Checked = True Or rdbNotApproved.Checked = True) Then
                strMsg = "Enter Remark in column"
                Exit For
            Else
                strMsg = ""
            End If
        Next
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
        If custValidator.ControlToValidate = "txtADSBPlanningSupportDate" Then
            If txtADSBPlanningSupportDate.Text = "" Then
                custValidator.ErrorMessage = "Select Actual Meeting Date."
                e.IsValid = False
            ElseIf txtADSBPlanningSupportDate.Text <> "" And CDate(mADSBTechRecording.IssueDateFormatted) > CDate(txtADSBPlanningSupportDate.Text) Then
                custValidator.ErrorMessage = "Actual Meeting Date should be greater than Issue Date"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtRiskIdentification" Then
            If txtRiskIdentification.Text.Length > 500 Then
                custValidator.ErrorMessage = "Risk Identification Description is too long."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtComplianceDuringOthersRemark" Then
            If txtComplianceDuringOthersRemark.Text.Length > 500 Then
                custValidator.ErrorMessage = "Remark is too long."
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtAreaOfConcern" Then
            If txtAreaOfConcern.Text.Length > 500 Then
                custValidator.ErrorMessage = "Area Of Concern is too long"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtAuditDescription" Then
            If txtAuditDescription.Text.Length > 500 Then
                custValidator.ErrorMessage = "Audit Description is too long"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtAMOCDescription" Then
            If txtAMOCDescription.Text.Length > 500 Then
                custValidator.ErrorMessage = "AMOC Description is too long"
                e.IsValid = False
            End If
        End If
        If custValidator.ControlToValidate = "txtsearch" Then
            If txtSearch.Text = "" Then
                e.IsValid = False
            ElseIf (txtSearch.Text.Trim.IndexOf("[") < 0 Or txtSearch.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = False
            ElseIf (txtSearch.Text.Trim.IndexOf("[") >= 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
                PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
                Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
                If PartNo = "" Or Description = "" Then
                    e.IsValid = False
                End If
            End If
        End If
    End Sub
#End Region

#Region "Buissness Methods"
    Private Sub ControlVisibility()
        txtADSBPlanningSupportDate.Enabled = IIf(Not mADSBPlanningSupport.IsNew, False, True)
        txtRiskIdentification.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbVerification.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbComplianceDuring.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtComplianceDuringOthersRemark.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        chkIsAMOCInvokingRequired.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtAMOCDescription.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbOEMDesignateMember.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbNAADesignateMember.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtOtherDesignateMemberData.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbOtherDesignateMember.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtTechPubRecordImplications.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        cmbFacility.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        chkIsAuditRequired.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtAuditDescription.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtSearch.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtAreaOfConcern.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtLabourCost.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtMaterialCost.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtFacilityCost.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        txtSubContratctedCost.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        btnAuthorized.Visible = (Not mADSBPlanningSupport.IsNew) And (mADSBPlanningSupport.StatusID = 1)

        
        dgItemList.Columns(6).Visible = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        dgAttachment.Columns(5).Visible = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        For k As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
            Dim txtCompliancePeriodInMeeting As TextBox
            Dim txtRemarkInMeeting As TextBox
            txtCompliancePeriodInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtCompliancePeriodInMeeting"), TextBox)
            txtCompliancePeriodInMeeting.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)

            txtRemarkInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtRemarkInMeeting"), TextBox)
            txtRemarkInMeeting.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        Next
        For j As Integer = 0 To mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Count - 1
            Dim txtQty As TextBox
            Dim txtLeadTime As TextBox
            txtQty = CType(Me.dgItemList.Rows(j).FindControl("txtQty"), TextBox)
            txtQty.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)

            txtLeadTime = CType(Me.dgItemList.Rows(j).FindControl("txtLeadTime"), TextBox)
            txtLeadTime.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        Next
        For h As Integer = 0 To mADSBPlanningSupport.FileAttachments.Count - 1
            Dim txtFileName As TextBox
            txtFileName = CType(Me.dgAttachment.Rows(h).FindControl("txtFileName"), TextBox)
            txtFileName.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        Next
        'ElseIf ADSBPlanningSupportOpenFrom = 2 Then '2 means from  Approval command
        '    txtRiskIdentification.Enabled = False
        '    cmbVerification.Enabled = False
        '    cmbComplianceDuring.Enabled = False
        '    txtComplianceDuringOthersRemark.Enabled = False
        '    chkIsAMOCInvokingRequired.Enabled = False
        '    txtAMOCDescription.Enabled = False
        '    cmbOEMDesignateMember.Enabled = False
        '    cmbNAADesignateMember.Enabled = False
        '    txtOtherDesignateMemberData.Enabled = False
        '    cmbOtherDesignateMember.Enabled = False
        '    txtTechPubRecordImplications.Enabled = False
        '    cmbFacility.Enabled = False
        '    chkIsAuditRequired.Enabled = False
        '    txtAuditDescription.Enabled = False
        '    txtSearch.Enabled = False
        '    txtAreaOfConcern.Enabled = False
        '    txtLabourCost.Enabled = False
        '    txtMaterialCost.Enabled = False
        '    txtFacilityCost.Enabled = False
        '    txtSubContratctedCost.Enabled = False
        '    btnSelectFiles.Enabled = False
        '    btnAuthorized.Visible = False
        '    dgMeetingParticipantsList.Columns(5).Visible = True     'Approved/NotApproved
        '    dgMeetingParticipantsList.Columns(6).Visible = True     'Approved Remark
        '    dgItemList.Columns(6).Visible = False
        '    dgAttachment.Columns(5).Visible = False
        '    For k As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
        '        Dim txtCompliancePeriodInMeeting As TextBox
        '        Dim txtRemarkInMeeting As TextBox
        '        txtCompliancePeriodInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtCompliancePeriodInMeeting"), TextBox)
        '        txtCompliancePeriodInMeeting.Enabled = False

        '        txtRemarkInMeeting = CType(Me.dgEffectivityDetails.Rows(k).FindControl("txtRemarkInMeeting"), TextBox)
        '        txtRemarkInMeeting.Enabled = False
        '    Next
        '    For h As Integer = 0 To mADSBPlanningSupport.FileAttachments.Count - 1
        '        Dim txtFileName As TextBox
        '        txtFileName = CType(Me.dgAttachment.Rows(h).FindControl("txtFileName"), TextBox)
        '        txtFileName.Enabled = False
        '    Next
        If ADSBPlanningSupportOpenFrom = 1 Then  '1 means from  Planned command
            btnSave.Visible = (Not mADSBPlanningSupport.StatusID >= 2)
            dgMeetingParticipantsList.Columns(5).Visible = True    'Approved/NotApproved
            dgMeetingParticipantsList.Columns(6).Visible = False    'Approved Remark
            For k As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
                Dim txtApprovedRemark As TextBox
                Dim rdbApproved As RadioButton
                Dim rdbNotApproved As RadioButton
                txtApprovedRemark = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("txtApprovedRemark"), TextBox)
                rdbApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbApproved"), RadioButton)
                rdbNotApproved = CType(Me.dgMeetingParticipantsList.Rows(k).FindControl("rdbNotApproved"), RadioButton)
                rdbApproved.Enabled = False
                rdbNotApproved.Enabled = False
                txtApprovedRemark.Enabled = False
            Next
        ElseIf ADSBPlanningSupportOpenFrom = 2 Then '2 means from  Approval command
            dgMeetingParticipantsList.Columns(5).Visible = True    'Approved/NotApproved
            dgMeetingParticipantsList.Columns(6).Visible = True     'Approved Remark
        End If
        btnSelectFiles.Enabled = IIf(mADSBPlanningSupport.StatusID >= 2, False, True)
        btnSendMail.Visible = IIf(ADSBPlanningSupportOpenFrom = 2, False, True)
    End Sub
    Private Sub UpdatePanel()
        upnlADSBPlanningSupportDetails.Update()
        upnlTitle.Update()
        upnlStatusName.Update()
        upnlADSBPlanningSupportDetails.Update()
        upnlEffectivityDetails.Update()
        upnlAMOCInvoking.Update()
        upnlDesignateMemberResponsibility.Update()
        upnlTechRecordImplications.Update()
        upnlMaterialRequirement.Update()
        upnlGridView.Update()
        upnlMeetingParticipantsList.Update()
        upnlAttachment.Update()
        upnldgAttachment.Update()
        upnlButtons.Update()
    End Sub
    Protected Sub AddAttributesForGridControls(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtQty As TextBox
        Dim txtLeadTime As TextBox
        For i As Integer = 0 To dgItemList.Rows.Count - 1
            Try
                txtQty = CType(Me.dgItemList.Rows(i).FindControl("txtQty"), TextBox)
                txtQty.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('" + txtQty.ClientID + "').value,event)")

                txtLeadTime = CType(Me.dgItemList.Rows(i).FindControl("txtLeadTime"), TextBox)
                txtLeadTime.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('" + txtLeadTime.ClientID + "').value,event)")
            Catch ex As Exception
            End Try
        Next
        upnlGridView.Update()
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        ADSBPlanningSupportOpenFrom = Request.QueryString("OpenFromLink")
        addAttributes()
        If Not IsPostBack Then
            DataFieldBind()
            SetPage()
            ControlVisibility()
        End If
        AddAttributesForGridControls(sender, e)
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then upnlValidationsummary.Update() : Exit Sub
        If ADSBPlanningSupportOpenFrom = 1 Then  '1 means from  Planned command
            If CustomValidate2() = False Then upnlValidationsummary.Update() : Exit Sub

            If CustomValidate1() Then
                If (Not IsInRole(Rights.[New]) And mADSBPlanningSupport.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBPlanningSupport.IsNew) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If Save() Then
                    SetPage()
                    Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
                    MarkLog(Util.Action.Save, "ADSBPlanningSupport", User.Identity.Name + " Saved AD/SB : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
                    If ADSBPlanningSupportOpenFrom = 1 Then  '1 means from  Planned command
                        mADSBTechRecording.ADSBStepsID = 4
                        mADSBTechRecording.Save()
                    End If
                    'If ADSBPlanningSupportOpenFrom = 2 Then  '1 means from  Approval command
                    '    mADSBReviewMeeting.Save()
                    '    ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted + " Review Meeting Saved From Approval Link "
                    '    MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD SB Review Meeting : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
                    'End If

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")

                End If
            Else
                upnlValidationsummary.Update()
            End If
        End If
        If ADSBPlanningSupportOpenFrom = 2 Then  '2 means from  Approval command
            If CustomValidate3() = False Then upnlValidationsummary.Update() : Exit Sub
            setObjectForADSBReviewMeeting()
            mADSBReviewMeeting.Save()
            dgMeetingParticipantsList.DataSource = mADSBReviewMeeting.ADSBReviewMeetingParticipants
            dgMeetingParticipantsList.DataBind()
            upnlMeetingParticipantsList.Update()
            Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted + " Review Meeting Saved From Approval Link "
            MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD SB Review Meeting : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
        End If
    End Sub
    Private Sub btnAuthorized_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        'If (Not IsInRole(Rights.Authorized)) Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
        '    Exit Sub
        'End If
      

        If IsValid Then
             MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<strong>AD/SB Planning</strong>", MsgBoxStyle.YesNo, "Status")
        End If
    End Sub
    Private Sub dgItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgItemList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"
                'Dim mid As Guid = New Guid(dgItemList.DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                Dim mID As Guid = New Guid(e.CommandArgument.ToString)
                mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Remove(mID)
                dgItemList.DataSource = mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements
                dgItemList.DataBind()
                upnlGridView.Update()
                upnlButtons.Update()
        End Select
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If (txtSearch.Text.Trim.IndexOf("[") > 0 And txtSearch.Text.Trim.IndexOf("]") > 0) Then
            PartNo = txtSearch.Text.Substring(0, txtSearch.Text.Trim.IndexOf("[")).Trim
            Description = Mid(txtSearch.Text.Trim, txtSearch.Text.Trim.IndexOf("[") + 2, txtSearch.Text.Trim.IndexOf("]") - txtSearch.Text.Trim.IndexOf("[") - 1).Trim
        Else
            PartNo = Trim(txtSearch.Text)
            Description = Trim(txtSearch.Text)
        End If
        If hdnpartId.Value <> String.Empty Then
            PartID = hdnpartId.Value.ToString
        End If
        If Not New Guid(PartID).Equals(Guid.Empty) Then
            If mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Count >= 1 Then
                If mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Contains(New Guid(PartID)) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not add duplicate record.", MsgBoxStyle.OkOnly, "")
                    txtSearch.Text = ""
                    upnlSearch.Update()
                    Exit Sub
                Else
                    GoTo Step1
                End If
            End If

Step1:      For j As Integer = 0 To mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Count - 1
                Dim txtQty As TextBox
                Dim txtLeadTime As TextBox
                txtQty = CType(Me.dgItemList.Rows(j).FindControl("txtQty"), TextBox)
                mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(j).Qty = CDec(Val(txtQty.Text))

                txtLeadTime = CType(Me.dgItemList.Rows(j).FindControl("txtLeadTime"), TextBox)
                mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(j).LeadTime = CDec(Val(txtLeadTime.Text))
            Next

            mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Add(mADSBPlanningSupport.ID, New Guid(PartID), PartNo, Description)
            dgItemList.DataSource = mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements
            dgItemList.DataBind()
            upnlGridView.Update()
            lblInstr.Visible = False
            AddAttributesForGridControls(sender, e)
        Else
            lblInstr.Visible = True
        End If
        txtSearch.Text = ""
        upnlSearch.Update()
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        If ADSBPlanningSupportOpenFrom = 1 Then  '1 means from  Planned command
            setObject()
            Dim ADSBDetail = "Actual Meeting Date : " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted
            MarkLog(Util.Action.Close, "ADSBPlanningSupport", ADSBDetail, Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
            If mADSBPlanningSupport.IsDirty Or mADSBTechRecording.IsDirty Then
                Session("IsValid") = "True"
                MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            Else
                Response.Redirect("index.aspx")
            End If
        End If
        If ADSBPlanningSupportOpenFrom = 2 Then  '2 means from  Approval command
            setObjectForADSBReviewMeeting()
            If mADSBReviewMeeting.IsDirty Then
                Session("IsValid") = "True"
                MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "CloseADSBReviewMeeting")
            Else
                Response.Redirect("index.aspx")
            End If
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        SetObject()
         ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
    Private Sub dgAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAttachment.RowCommand
        Dim mFileAttachments As FileAttachments
        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex
                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = mADSBPlanningSupport.FileAttachments
                'mFileAttachments.CurrentIndex = Index - 1
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
                dgAttachment.DataSource = mADSBPlanningSupport.FileAttachments
                dgAttachment.DataBind()
                ControlVisibility()
                upnlAttachment.Update()
                upnldgAttachment.Update()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument) + dgAttachment.PageSize * dgAttachment.PageIndex
                mFileAttachments = mADSBPlanningSupport.FileAttachments
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
    Private Sub lnkHintQuestion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHintQuestion.Click
        setObject()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenHintQuestionWindow", "OpenHintQuestionWindow();", True)
    End Sub
    Private Sub btnSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendMail.Click
        If (Not IsInRole(Rights.[New]) And mADSBPlanningSupport.IsNew) Or (Not IsInRole(Rights.Edit) And Not mADSBPlanningSupport.IsNew) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
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

        str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Review Board Meeting Successfully held on " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted.ToString + "</font></P></br>")
        str = str + ("<font face=""Calibri"">Following is information." + "</font> ")

        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>Date: " + "</b>" + mADSBTechRecording.ADSBDateFormatted.ToString + " | <b> No.:</b> " + mADSBTechRecording.ADSBRecordingText + " | <b>" + " AD/SB No.: " + "</b>" + mADSBTechRecording.ADSBNo + " | <b>" + " Subject: " + "</b>" + mADSBTechRecording.ADSBSubject)
        str = str + (IIf(mADSBReviewMeeting.IsOnLine = False, " | <b>Location: " + "</b>" + IIf(mADSBReviewMeeting.MeetingLocation = "", "", mADSBReviewMeeting.MeetingLocation), "") + IIf(mADSBReviewMeeting.IsOnLine = True, "<b>" + " | Meeting Link: " + "</b>" + IIf(mADSBReviewMeeting.MeetingLink = "", "", mADSBReviewMeeting.MeetingLink), ""))
        str = str + ("</font>")

        str = str + ("<p><font face=""Calibri"">")

        str = str + ("</br><TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table1"">")
        str = str + ("<TR>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Risk Identification: " + "</b>")
        str = str + ("</TD>")
        str = str + ("</font>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.RiskIdentification = "", "", mADSBPlanningSupport.RiskIdentification)
        str = str + ("</TD>")
        str = str + ("</font>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Verification: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.VerificationName = "", "", mADSBPlanningSupport.VerificationName)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("<TR>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Compliance During: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.ComplianceDuringName = "", "", mADSBPlanningSupport.ComplianceDuringName)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Remark: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.ComplianceDuringOthersRemark = "", "", mADSBPlanningSupport.ComplianceDuringOthersRemark)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("<TR>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " AMOC Invoking: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.IsAMOCInvokingRequired = True, "Yes", "No")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Description: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.AMOCDescription = "", "", mADSBPlanningSupport.AMOCDescription)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("<TR>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Tech Record Implications: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.TechPubRecordImplications = "", "", mADSBPlanningSupport.TechPubRecordImplications)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Facility: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(cmbFacility.SelectedIndex = 0, "", cmbFacility.SelectedItem.Text)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("<TR>")
        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Audit Require: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.IsAuditRequired = True, "Yes", "No")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""120"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Description: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" >")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.AuditDescription = "", "", mADSBPlanningSupport.AuditDescription)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("<TR>")

        str = str + ("<TD WIDTH=""120"">")
        str = str + ("<font face=""Calibri"">")
        str = str + ("<b>" + " Area Of Concern: " + "</b>")
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("<TD WIDTH=""400"" colspan=""3"">")
        str = str + ("<font face=""Calibri"">")
        str = str + IIf(mADSBPlanningSupport.AreaOfConcern = "", "", mADSBPlanningSupport.AreaOfConcern)
        str = str + ("</font>")
        str = str + ("</TD>")

        str = str + ("</TR>")

        str = str + ("</TABLE>")
        str = str + ("</font></p>")

        str = str + ("</br><b>" + " Reconfirmation Of AD/SB effectivity: ")
        str = str + ("<TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table2"">")
        If mADSBTechRecording.ApplicableToPart = True Then
            str = str + ("<tr>" & "<td align=""center"" width=""50"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Applicable part</b>" & "</font>" & "</td><td align=""center"" width=""50"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Serial No.</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Effective Date</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Compliance Period</b>" & "</font>" & "</td> <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Remark</b>" & "</font>" & "</td> <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Compliance Period while Planning</b>" & "</font>" & "</td>  <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Remark while Planning</b>" & "</font>" & "</td></tr>")
        ElseIf mADSBTechRecording.ApplicableToModel = True Then
            str = str + ("<tr>" & "<td align=""center"" width=""50"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Model</b>" & "</font>" & "</td><td align=""center"" width=""50"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Serial No.</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Effective Date</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Compliance Period</b>" & "</font>" & "</td> <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Remark</b>" & "</font>" & "</td> <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Compliance Period while Planning</b>" & "</font>" & "</td>  <td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Remark while Planning</b>" & "</font>" & "</td></tr>")
        End If

        For i As Integer = 0 To mADSBTechRecording.ADSBTechRecordingApplicableOns.Count - 1
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            If mADSBTechRecording.ApplicableToPart = True Then
                str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).PartName)
            ElseIf mADSBTechRecording.ApplicableToModel = True Then
                str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).ModelName)
            End If
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).SerialNo)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=200px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).EffectiveDateFormatted)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).CompliancePeriod)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).Remark)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=20px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).CompliancePeriodInMeeting)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=20px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBTechRecording.ADSBTechRecordingApplicableOns(i).RemarkInMeeting)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("</TR>")
        Next
        str = str + ("</TABLE>")

        str = str + ("</br>Material Requirement")
        str = str + ("</br><TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table3"">")
        str = str + ("<tr>" & "<td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Part No.</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Description</b>" & "</font>" & "</td><td align=""center"" width=""50"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Qty.</b>" & "</font>" & "</td><td align=""center"" width=""100"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Lead Time</b>" & "</font>" & "</td></tr>")
        For i As Integer = 0 To mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements.Count - 1
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=100px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(i).ItemName)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=100px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(i).ItemDescription)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD align=""center"" WIDTH=50px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(i).Qty.ToString)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD align=""center"" WIDTH=100px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements(i).LeadTime.ToString)
            str = str + ("</font>")
            str = str + ("</TD>")
            '------------
            str = str + ("</TR>")
        Next
        str = str + ("</TABLE>")
        str = str + ("</br>Participant")
        str = str + ("</br><TABLE BORDER=1 Style=""border-collapse: collapse"" BORDER-COLOR=""black"" ID=""Table4"">")
        str = str + ("<tr>" & "<td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"">" & "<font face=""Calibri""><b>Participant Name</b>" & "</font>" & "</td><td align=""center"" width=""200"" style=""background-color: #E4E2E1; color: black;"" >" & "<font face=""Calibri""><b>Email</b>" & "</font>" & "</td></tr>")

        For i As Integer = 0 To mADSBReviewMeeting.ADSBReviewMeetingParticipants.Count - 1
            str = str + ("<TR>")

            str = str + ("<TD WIDTH=200px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBReviewMeeting.ADSBReviewMeetingParticipants(i).EmployeeName)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("<TD WIDTH=200px >")
            str = str + ("<font face=""Calibri"">")
            str = str + (mADSBReviewMeeting.ADSBReviewMeetingParticipants(i).EmployeeEmail)
            str = str + ("</font>")
            str = str + ("</TD>")

            str = str + ("</TR>")
        Next
        str = str + ("</TABLE>")
        str = str + ("<font face=""Calibri"">Request all participants please login to FlyPal® system to approve this evaluation." + "</font> ")
        str = str + ("</body></html>")

        Dim mFileAttachments As FileAttachments
        Dim No As New Random
        mFileAttachments = mADSBPlanningSupport.FileAttachments

        If mFileAttachments.Count > 0 Then
            For i As Integer = 0 To mFileAttachments.Count - 1
                If mFileAttachments(i).Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & mFileAttachments(i).FileName '& mFileAttachments(i).Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & "abc" & No.Next.ToString & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments(i).ImageFile, 0, mFileAttachments(i).ImageFile.Length)
                        fs.Close()
                    End If
                    myAttchedFileName.Append(path)
                    myAttchedFileName.Append(",")
                End If
            Next
        End If

        SendMailFile.SendMailFile(, User.Identity.Name, "Review Board Meeting Successfully held on " + mADSBPlanningSupport.ActualMeetingDateTimeFormatted.ToString, Text:="File(s)", _
            Info:=str, ToMailID:=ToMailIDs.ToString.Substring(0, ToMailIDs.Length - 1), ReportPath:=myAttchedFileName.ToString, ReportByMail:=True, Remark:="", ReportGeneratedBy:="", _
            AttachedFile:=myAttchedFileName.ToString, MultipleAttachment:="Multiple Attachment")
        Dim mADSBReviewMeetingInfo As String = "Review Meeting Notification sent successfully to " + SubScribers.ToString.TrimEnd(",") + " by " + User.Identity.Name
        MarkLog(Util.Action.SendMail, "ADSBReviewMeeting", mADSBReviewMeetingInfo, Util.ErrorType.HandledError, mADSBReviewMeeting.ID, EventLogID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

        upnlGridView.Update()
    End Sub
    Private Sub dgMeetingParticipantsList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgMeetingParticipantsList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim lblApproved As Label = DirectCast(e.Row.FindControl("lblApproved"), Label)
            Dim lblNotApproved As Label = DirectCast(e.Row.FindControl("lblNotApproved"), Label)
            'Dim ImgApprovedStatus As ImageButton = DirectCast(e.Row.FindControl("ApprovedStatus"), ImageButton)
            'Dim lblNotApprovedImageButton As ImageButton = DirectCast(e.Row.FindControl("lblNotApprovedImageButton"), ImageButton)
            If mADSBReviewMeeting.ADSBReviewMeetingParticipants(e.Row.RowIndex).ApprovedStatus = 1 Then
                lblApproved.Font.Bold = True
                lblApproved.ForeColor = Color.Green
                'ImgApprovedStatus.Visible = True
                'lblApproved.Visible = False
            ElseIf mADSBReviewMeeting.ADSBReviewMeetingParticipants(e.Row.RowIndex).ApprovedStatus = 2 Then
                lblNotApproved.Font.Bold = True
                lblNotApproved.ForeColor = Color.Red
                'lblNotApprovedImageButton.Visible = True
                'lblNotApproved.Visible = False
            End If
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim rpt As ADSBReviewRegisterReport
        Dim ds As New dsADSBPlanningSupport
        myReport = New crptADSBPlanningSupport

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, ReportName:="AD/SB Planning", SearchStr1:=New SmartDate("6-Oct-2022").FormattedText, SearchStr2:=New SmartDate("6-Oct-2022").FormattedText, _
              SearchStr3:="", SearchStr4:="", SearchStr5:="", ProductVersion:=AppSettings("Product Version"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"), _
              SearchStr11:=AppSettings("MROISONo"), SearchStr12:="TELEFAX:" & mCompanyDetail.Fax & " " & mCompanyDetail.Email, SearchStr13:=txtADSBNO.Text.ToString, _
              SearchStr14:="", SearchStr16:="", SearchStr15:="", _
              SearchStr17:="", SearchStr18:="", SearchStr19:="", SearchStr20:="", _
              SearchStr21:="", SearchStr22:="", SearchStr23:="", SearchStr24:="", _
              SearchStr25:="", SINote:=AppSettings("SINote"))
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mADSBPlanningSupport)
        da.Fill(ds, mADSBPlanningSupport.ADSBPlanningSupportMaterialRequirements)
        da.Fill(ds, mADSBTechRecording)
        da.Fill(ds, mADSBTechRecording.ADSBTechRecordingApplicableOns)
        da.Fill(ds, mADSBReviewMeeting)
        da.Fill(ds, mADSBReviewMeeting.ADSBReviewMeetingParticipants)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        Dim Str1 As String
        Str1 = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    End Sub
    'Private Sub btnSaveAndClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveAndClose.Click
    '    If ADSBPlanningSupportOpenFrom = 2 Then  '2 means from  Approval command
    '        If CustomValidate3() = False Then upnlValidationsummary.Update() : Exit Sub
    '        setObjectForADSBReviewMeeting()
    '        mADSBReviewMeeting.Save()
    '        Dim ADSBDetail = mADSBTechRecording.ADSBNo + " Planned Dated : " + mADSBReviewMeeting.PlannedMeetingDateTimeFormatted + " Review Meeting Saved From Approval Link "
    '        MarkLog(Util.Action.Save, "ADSBReviewMeeting", User.Identity.Name + " Saved AD SB Review Meeting : " + ADSBDetail + " SuccessFully.", Util.ErrorType.NoError, mADSBPlanningSupport.ID, EventLogID)
    '        MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
    '        Response.Redirect("index.aspx")
    '    End If
    'End Sub
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