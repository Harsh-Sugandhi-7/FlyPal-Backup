Imports System.Linq
'AJAX Conversion By Saylee On 31-Dec-2014
'UI & StyleSheets Changes by Harsh

Public Class wfMELSnagCorrectiveActionNew_AJAX
    Inherits Page

#Region " Variable Declaration "
    Public mMELSnagCorrectiveAction As MELSnagCorrectiveAction
    Public mMELSnagCorrectiveActionLog As MELSnagCorrectiveActionLog
    Public mMELSnagPartList As MELSnagPartList
    Public mReportLogRegister As New ReportLogRegister
    Public mRectifiedReportLogRegister As New ReportLogRegister
    Public BackPage As String
    Public mMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
    Public strMsg As String = ""
    Public DiscrepancyDetailsErrorMessage As String = ""
    Public VerificationDetailsErrorMessage As String = ""
    Public RectificationDetailsErrorMessage As String = ""
    Public mATAList As ATAList

    Dim MachineID As String
    Dim mTempAssemblyList As AssemblyList
    Dim mShowMEL As Boolean = False
    Dim EventLogID As Guid
    Dim mMELSnagDetail As String
    Dim mSubATAList As SubATAList
    Dim mAssemblylist As AssemblyList
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Dim mModuleList As ModuleList

    Shared UserNameForLicenceList As String

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMELSnagCorrectiveActionList = Session("mMELSnagCorrectiveActionList")
        mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
        MachineID = Session("MachineID")
        mMELSnagPartList = Session("mMELSnagPartList")
        mReportLogRegister = CType(Session("mReportLogRegister"), ReportLogRegister)
        mRectifiedReportLogRegister = CType(Session("mRectifiedReportLogRegister"), ReportLogRegister)
        mTempAssemblyList = CType(Session("mTempAssemblyList"), AssemblyList)
        mMELSnagCorrectiveActionLog = CType(Session("mMELSnagCorrectiveActionLog"), MELSnagCorrectiveActionLog)
        mShowMEL = CType(Session("ShowMEL"), Boolean)
        mATAList = CType(Session("mATAList"), ATAList)
        'Added By Vikrant On 02-Apr-2013 For ALL01042013
        mSubATAList = Session("mSubATAList")
        mAssemblylist = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        mModuleList = Session("mModuleList")
    End Sub

    Private Sub SetSession()
        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
        Session("MachineID") = MachineID
        Session("mReportLogRegister") = mReportLogRegister
        Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
        Session("mTempAssemblyList") = mTempAssemblyList
        Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
        Session("mMELSnagPartList") = mMELSnagPartList
        Session("mATAList") = mATAList
        Session("mSubATAList") = mSubATAList
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub

    Private Overloads Sub SetFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript([GetType](), "focusScript", str)
    End Sub

    Private Function GetDefectNo() As String
        Dim No As New Random
        Dim ReportNo As String
        ReportNo = "DEFECT" + "/" + Session("AircraftRegNo")
        Return ReportNo
    End Function

    Private Sub SetObject()
        With mMELSnagCorrectiveAction
            .LogID = New Guid(cmbLogNo.SelectedValue)
            .LogNo = mReportLogRegister(New Guid(cmbLogNo.SelectedValue)).LogNo
            If txtDateofOccurrence.Text <> "" Then
				.DateOfOccurrence = txtDateofOccurrence.Text
			Else
				.DateOfOccurrence = DBNull.Value
			End If
            .DefectReportNo = Trim(txtDefectReportNo.Text)
            .No = Val(txtNo.Text)
            .Sector = Trim(txtSector.Text)
            .LastMajorCheckHour = Trim(txtLastMajorCheck.Text)
            .SnagReportedBy = Trim(txtSnagReportedBy.Text)
            .ReportedBy = Trim(txtReportedBy.Text)
            .PartID = New Guid(cmbPartNo.SelectedValue)
            If cmbPartNo.SelectedIndex > 0 Then
                .PartSerialNo = mMELSnagPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").SerialNo
            Else
                .PartSerialNo = Trim(txtSerialNo.Text)
            End If
            .Description = Trim(txtDescription.Text)
            .ComponentHour = Trim(txtHrsofComp.Text)
            .Defect = Trim(txtDefect.Text)
            .CauseOfDefect = Trim(txtCauseofDefect.Text)
            .Action = Trim(txtAction.Text)
            .ActionAgainstStaff = Trim(txtActionTakenAganistEngStaff.Text)
            .PreventionTaken = Trim(txtPreventiveMeasuresTaken.Text)
            .IsMEL = chkShowMEL.Checked
            .MELCategoryID = cmbMELCategory.SelectedValue
            .ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
            .IsMajor = rbMajor.Checked
            .IsMinor = rbMinor.Checked
            .InvestigationStatus = chkClose.Checked
            .MachineID = New Guid(MachineID.ToString)
            .IsHours = chkIsInHours.Checked
            .FrequencyInDays = Val(txtFrequencyInDay.Text)
            .FrequencyInHours = txtFrequencyInHours.Text.Trim
            .RectifiedStation = txtRectificationSector.Text.Trim
            If txtDueDate.Text <> "" Then
                .DueDate = txtDueDate.Text
            Else
                .DueDate = DBNull.Value
            End If
            If txtRectifiedDate.Text <> "" Then
                .RectifiedDate = txtRectifiedDate.Text
            Else
                .RectifiedDate = DBNull.Value
            End If
            If cmbRectifiedLogNo.SelectedIndex > 0 Then
                .RectifiedLogID = New Guid(cmbRectifiedLogNo.SelectedValue)
            End If
            .PartNo = Trim(txtPartNo.Text)
            .IsRepetitive = chkIsRepetitive.Checked
            .Remark = Trim(txtRemark.Text)
            .SubATAID = New Guid(cmbSubATAList.SelectedValue) 'Added By Vikrant On 02-Apr-2013 For ALL01042013
            .IsPireps = rbPireps.Checked
            .IsMaintenanceDefect = rbMaintenanceDefect.Checked
            .IsInReliability = chkIsInReliability.Checked
            .AssemblyStatusID = New Guid(cmbAssembly.SelectedValue) 'Added By Vikrant On 02-Sept-2014 For All04092014
            .ExtensionApplied = chkExtensionApplied.Checked
            .ExtensionInDays = Val(txtExtensionInDays.Text)
            .ExtensionApprovalNo = Trim(txtExtensionApprovalNo.Text)
            .IncidentTypeID = cmbIncidentType.SelectedValue
            .IncidentTypeName = cmbIncidentType.SelectedItem.Text
        End With

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mMELSnagCorrectiveAction.IsAttachmentAdded = True
            Else
                mMELSnagCorrectiveAction.IsAttachmentAdded = False
            End If
        End If

        Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
    End Sub

    Private Sub FillRectifiedCombo()

        If IsDate(txtRectifiedDate.Text) Or (txtRectifiedDate.Text = "") Then
            'cmbRectifiedLogNo.Enabled = True

            If txtRectifiedDate.Text = "" Then
                mMELSnagCorrectiveAction.RectifiedDate = DBNull.Value
            Else
                mMELSnagCorrectiveAction.RectifiedDate = txtRectifiedDate.Text
            End If

            Dim tmpLogDetail As Log
            If cmbLogNo.SelectedIndex > 0 Then
                Dim LogID As Guid = New Guid(cmbLogNo.SelectedValue)
                tmpLogDetail = Log.GetLog(LogID)
            End If
            mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(tmpLogDetail.Date.ToString, txtDateofOccurrence.Text), "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID, False, , 0, , , , "(SELECT)", True, IIf(cmbLogNo.SelectedIndex > 0, cmbLogNo.SelectedValue.ToString, Guid.Empty), True)
            cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
            cmbRectifiedLogNo.DataBind()
            tmpLogDetail = Nothing

            If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
                txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
            Else
                txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
            End If

            Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
            cmbRectifiedLogNo.SelectedIndex = 0
        Else
            If mMELSnagCorrectiveAction.RectifiedDate.ToString = "" Then
                txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDate.ToString
            Else
                txtRectifiedDate.Text = mMELSnagCorrectiveAction.RectifiedDateFormatted
            End If

        End If

    End Sub

    Public Sub SetLicenceCount()
        If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
    End Sub

    Private Sub BindLicenceNo()
        If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
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
                            mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")
                            MELSnagCorrectiveAction.DeleteMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID)
                            ControlVisibility()
                            SetTitle()
                            upnlTitle.Update()
                            upnlMELSnagDetails.Update()
                            upnlCreateWO.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Action.Delete, "Log Defect Action", "Can't delete : This is Currently in use", ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                        End Try
                    ElseIf MSGBoxCtrl.Sender = "Close" Then    '' Close confirmation
                        Session("sender") = ""
                        mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")

                        'Added By Harsh on 11th March 2024 -- Validate Page instead of object
                        Page.Validate()
                        If Not Page.IsValid() Then
                            upnlErrorList.Update()
                            upnlVerificationDetailsErrors.Update()
                            upnlRectificationDetailsErrors.Update()
                            Exit Sub
                        End If

                        If mMELSnagCorrectiveAction.IsValid Then
                            Session("ForDateOfOccurance") = "ForDateOfOccurance"
                            Try
                                DataFieldBind()
                                ''Added By Prashant 2-Jan-2014  --ALL02012014-1
                                If (cmbATAChapter.SelectedIndex > 0 And chkIsRepetitive.Checked = False And mMELSnagCorrectiveAction.IsNew = True) Then
                                    Dim mMELSnagCountATAWise As MELSnagCountATAWise
									mMELSnagCountATAWise = MELSnagCountATAWise.GetMELSnagCountATAWise(mMELSnagCorrectiveAction.ATAChapterID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, mMELSnagCorrectiveAction.ID.ToString, Val(AppSettings("MEL_Occurrance_In_Days")), mMELSnagCorrectiveAction.DateOfOccurrenceFormatted, Val(AppSettings("MEL_Check_ON"))) 'Added config parameters by Saylee on  24-Feb-2020 for ALL24022020

									If mMELSnagCountATAWise.Item(0).MELSnagCount > 0 Then
										Dim MsgStr As String = String.Empty
										MsgStr = "There are " + mMELSnagCountATAWise.Item(0).MELSnagCount.ToString + IIf(AppSettings("MELSnagNomenclature") = "True", " ADD/Defect", " MEL/Snag") + " reported for this ATA. " + " Last Log Date is " + New SmartDate(mMELSnagCountATAWise.Item(0).LogInfo.ToString.Substring(0, mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(0).Trim.Length)).FormattedText + "<BR>" + " Log No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(1).Trim + "<BR>" + " Log Page No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(2).Trim + "<BR>" + " Do you want to make this " + IIf(AppSettings("MELSnagNomenclature") = "True", "Defect as Repetitive Defect", "Snag as Repetitive Snag") + "?"
										MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, MsgStr, MsgBoxStyle.YesNo, "MELSnagCountATAWise")
										Exit Sub
									End If
								End If
								'-------------------------------------------------------
								mMELSnagCorrectiveAction = mMELSnagCorrectiveAction.Save
								SaveAttachment()

								Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
								Session("mMELSnagCorrectiveActionList") = mMELSnagCorrectiveActionList
								mMELSnagDetail = mMELSnagCorrectiveAction.DefectNo + " Dated : " + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + mMELSnagCorrectiveAction.LogNo
								MarkLog(Action.Save, "MEL/Snag Defect Corrective Action", mMELSnagDetail, ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
								Session.Remove("mMELSnagCorrectiveAction")
								Session.Remove("mFileAttach")
								Session.Remove("mMaintenanceDoneByEmployees")
								Session.Remove("UserNameForLicenceList")
								If Request.QueryString("BackPage1") = "wfnWODetail_AJAX.aspx" Then
									Response.Redirect(Request.QueryString("BackPage1"))
								Else
									Response.Redirect(Request.QueryString("BackPage"))
								End If

							Catch ex As SqlException
								If ex.Number = 8145 Then
									MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 2627 Then
									MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 547 Then
									MarkLog(Action.Delete, "Log Defect Action", "Can't delete : This is Currently in use", ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
									MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
								End If
							End Try
						Else
							Session("ForDateOfOccurance") = "ForDateOfOccurance"
							DataFieldBind()
							ControlVisibility()
							If mMELSnagCorrectiveAction.RectifiedDate.ToString <> "" Then
								cmbRectifiedLogNo.Enabled = True
							End If

							'Added By Harsh on 26th Feb 2024 -- Validate Page instead of object
							Page.Validate()
							If Not Page.IsValid() Then
								upnlErrorList.Update()
								upnlVerificationDetailsErrors.Update()
								upnlRectificationDetailsErrors.Update()
								Exit Sub
							End If
						End If
						''Added By Prashant 2-Jan-2014  --ALL02012014-1
					ElseIf MSGBoxCtrl.Sender = "MELSnagCountATAWise" Then
						Session("sender") = ""
						Session("ForDateOfOccurance") = "ForDateOfOccurance"
						mMELSnagCorrectiveAction.IsRepetitive = True
						DataFieldBind()
						Save()
						ControlVisibility()
						SetTitle()
						chkIsRepetitive.Checked = mMELSnagCorrectiveAction.IsRepetitive
						upnlIsRepetitive.Update()
						upnlTitle.Update()
						upnlCreateWO.Update()
						upnlHeaderButtons.Update()
					End If
				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "Close" Then
						Session.Remove("IsValid")
						If mMELSnagCorrectiveAction.IsNew Then
							Session.Remove("mMELSnagCorrectiveAction")
						End If
						Session("Sender") = ""
						Session.Remove("mMELSnagCorrectiveAction")
						Session("mMELSnagCorrectiveActionList") = mMELSnagCorrectiveActionList
						'Added By Vikrant On 02-Apr-2013 For ALL01042013
						Session.Remove("mSubATAList")
						ControlVisibility()
						SetTitle()
						upnlTitle.Update()
						Session.Remove("mFileAttach")
						Session.Remove("mMaintenanceDoneByEmployees")
						Session.Remove("UserNameForLicenceList")
						BackPage = Session("BackPage")

						If BackPage = "" Then
							Response.Redirect("index.aspx")
						Else
							Response.Redirect(Request.QueryString("BackPage"))
						End If

						''Added By Prashant 2-Jan-2014  --ALL02012014-1
					ElseIf MSGBoxCtrl.Sender = "MELSnagCountATAWise" Then
						Session("sender") = ""
						Session("ForDateOfOccurance") = "ForDateOfOccurance"
						DataFieldBind()
						Save()
						ControlVisibility()
						SetTitle()
						upnlIsRepetitive.Update()
						upnlButtons.Update()
						upnlTitle.Update()
						upnlMELSnagDetails.Update()
						upnlHeaderButtons.Update()
						'----------------------------------------------------------------
					Else
						Session("sender") = ""
						ControlVisibility()
						SetTitle()
						upnlTitle.Update()
						upnlCreateWO.Update()
					End If
				Case MsgBoxResult.Ok
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					SetTitle()
					upnlTitle.Update()
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  '
					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					SetTitle()
					upnlTitle.Update()
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
			Session("ForDateOfOccurance") = "ForDateOfOccurance"
			DataFieldBind()
			ControlVisibility()
			SetTitle()
			upnlTitle.Update()
		ElseIf Result1 = 0 And Session("sender") = "Authorization" Then
			Session("sender") = ""
			DataFieldBind()
			ControlVisibility()
			SetTitle()
			upnlTitle.Update()
		End If
	End Sub

	Private Sub CheckVisibility()
		If chkClose.Checked Then
			txtRectifiedDate.ReadOnly = False
		Else
			txtRectifiedDate.Text = ""
			txtRectifiedDate.ReadOnly = True
			cmbRectifiedLogNo.SelectedIndex = 0
			cmbRectifiedLogNo.Enabled = False
			txtRectificationSector.Text = ""
			mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty
		End If
	End Sub

	Private Sub SetTitle()
		If mMELSnagCorrectiveAction.IsNew Then
			lblSnagCorrectiveActionInfo.Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") & " Corrective Action [ New ] "
		Else
			lblSnagCorrectiveActionInfo.Text = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") & " Corrective Action [ " & mMELSnagCorrectiveAction.DefectNo & " ]"
		End If
	End Sub

	Private Sub ControlVisibility()
		If chkClose.Checked Then
			txtRectifiedDate.Enabled = True
		Else
			txtRectifiedDate.Enabled = False
		End If
		txtDueDate.Enabled = False

		If cmbLogNo.SelectedIndex > 0 Then
			lnkCheckStatus.Enabled = True
		Else
			lnkCheckStatus.Enabled = False
		End If

		If cmbPartNo.SelectedIndex <= 0 Then
			If txtPartNo.Text <> "" Then
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtPartNo.Text = ""
			End If
			If txtDescription.Text <> "" Then
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtDescription.Text = ""
			End If
			If txtSerialNo.Text <> "" Then
			ElseIf cmbPartNo.SelectedIndex <= 0 Then
				txtSerialNo.Text = ""
			End If

			txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
			txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
			txtDescription.ReadOnly = False
			txtPartNo.ReadOnly = False
			txtSerialNo.ReadOnly = False
		Else
			txtDescription.ReadOnly = True
			txtPartNo.ReadOnly = True
			txtSerialNo.ReadOnly = True
			txtDescription.BackColor = Color.FromName("#E0E0E0")
			txtPartNo.BackColor = Color.FromName("#E0E0E0")
			txtSerialNo.BackColor = Color.FromName("#E0E0E0")
		End If
		If chkShowMEL.Checked = True Then
			If chkClose.Checked = True Then
				chkExtensionApplied.Enabled = False
			Else
				chkExtensionApplied.Enabled = True
			End If
		Else
			chkExtensionApplied.Enabled = False
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
		End If

		'Added By Vikrant On 03-Apr-2013 For ALL01042013
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		'End

		ControlVisibilityForAttachment()
		upnlFileupload.Update()
	End Sub

	Private Sub ControlVisibilityAfterEdit()

		Try
			If mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = True Then
				chkIsInHours.Enabled = True
				txtFrequencyInHours.Enabled = True
				txtFrequencyInDay.Enabled = False
			ElseIf mMELSnagCorrectiveAction.MELCategoryID = 1 And mMELSnagCorrectiveAction.IsHours = False Then
				chkIsInHours.Enabled = True
				txtFrequencyInHours.Enabled = False
				txtFrequencyInDay.Enabled = True
			ElseIf (mMELSnagCorrectiveAction.MELCategoryID = 2 Or mMELSnagCorrectiveAction.MELCategoryID = 3 Or mMELSnagCorrectiveAction.MELCategoryID = 4) Then
				chkIsInHours.Enabled = False
				txtFrequencyInHours.Enabled = False
			End If

			cmbMELCategory.Enabled = False

			If mMELSnagCorrectiveAction.RectifiedDate.ToString <> "" Then
				cmbRectifiedLogNo.Enabled = True
			End If
			If Not mMELSnagCorrectiveAction.LogID.Equals(Guid.Empty) Then
				lnkCheckStatus.Enabled = True
			End If

			If cmbPartNo.SelectedIndex <= 0 Then
				If txtPartNo.Text <> "" Then
					'
				ElseIf cmbPartNo.SelectedIndex <= 0 Then
					txtPartNo.Text = ""
				End If
				If txtDescription.Text <> "" Then
					'
				ElseIf cmbPartNo.SelectedIndex <= 0 Then
					txtDescription.Text = ""
				End If
				If txtSerialNo.Text <> "" Then
					'
				ElseIf cmbPartNo.SelectedIndex <= 0 Then
					txtSerialNo.Text = ""
				End If

				txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
				txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
				txtDescription.ReadOnly = False
				txtPartNo.ReadOnly = False
				txtSerialNo.ReadOnly = False
			Else
				txtDescription.ReadOnly = True
				txtPartNo.ReadOnly = True
				txtSerialNo.ReadOnly = True
				txtDescription.BackColor = Color.FromName("#E0E0E0")
				txtPartNo.BackColor = Color.FromName("#E0E0E0")
				txtSerialNo.BackColor = Color.FromName("#E0E0E0")
			End If

			If mMELSnagCorrectiveAction.IsHours = True Then
				txtFrequencyInDay.Enabled = False
				txtFrequencyInDay.Text = "0"
			Else
				txtFrequencyInHours.Text = ""
				txtFrequencyInHours.Enabled = False
			End If

			If mMELSnagCorrectiveAction.IsMEL = True Or chkShowMEL.Checked = True Then
				If mMELSnagCorrectiveAction.InvestigationStatus = True Or chkClose.Checked = True Then
					chkExtensionApplied.Enabled = False
				Else
					chkExtensionApplied.Enabled = True
				End If
				upnlExtension.Update()
			Else
				chkExtensionApplied.Enabled = False
				txtExtensionInDays.Enabled = False
				txtExtensionApprovalNo.Enabled = False
				txtExtensionInDays.Text = 0
				txtExtensionApprovalNo.Text = ""
				mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
				upnlExtension.Update()
			End If

			If mMELSnagCorrectiveAction.IsWOCreated Then
				lnkbtnCreateWorkOrder.Text = "View Work Order "
				lnkbtnCreateWorkOrder.ToolTip = mMELSnagCorrectiveAction.WONumber
			Else
				lnkbtnCreateWorkOrder.Text = "Create Work Order"
				lnkbtnCreateWorkOrder.ToolTip = "Create Work Order"
			End If
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub AddAttributes()
		txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
		txtExtensionInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtExtensionInDays').value,event)")
		txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtFrequencyInHours').value,event)")
	End Sub

	Private Function Get_Whichever_LesserDate(LogDate As String, OccurranceDate As String) As String
		Dim mLogDate As SmartDate = New SmartDate(LogDate)
		Dim mOccurranceDate As SmartDate = New SmartDate(OccurranceDate)

		If CDate(mLogDate.ToString) < CDate(mOccurranceDate.ToString) Then
			Return mLogDate.ToString
		Else
			Return mOccurranceDate.ToString
		End If
	End Function

	Private Sub ControlVisibilityForAttachment()

		If IsNothing(mFileAttach) Then
			GetAttachment()
		End If

		If Not mFileAttach Is Nothing Then
			If mFileAttach.Size > 0 Then
				attachmentICN.Visible = True
				btnDelAttach.Enabled = True
			Else
				attachmentICN.Visible = False
			End If
		End If

	End Sub

	Private Sub GetAttachment()
		If mMELSnagCorrectiveAction.IsAttachmentAdded And mFileAttach Is Nothing Then
			mFileAttach = FileAttach.GetAttachment(mMELSnagCorrectiveAction.ID)
			Session("mFileAttach") = mFileAttach
		End If
	End Sub

	Private Sub SaveAttachment() '
		mFileAttach.ReferenceID = mMELSnagCorrectiveAction.ID
		If mFileAttach.Size > 0 Then
			Try
				mFileAttach.Save()
				Session("mFileAttach") = mFileAttach
			Catch ex As Exception
				ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
			End Try
		Else
			If (Not mMELSnagCorrectiveAction.IsNew) And IsAttachmentDeleted Then
				FileAttach.DeleteAttachment(mFileAttach.ID, mMELSnagCorrectiveAction.ID)
			End If
			IsAttachmentDeleted = False
			Session("IsAttachmentDeleted") = IsAttachmentDeleted
		End If
	End Sub

	Private Sub ViewImage()
		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		If mFileAttach.Size > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
			End If
		End If
	End Sub
	'End
#End Region

#Region " DataBinding "
	Private Sub DataFieldBind()
		MachineID = Session("MachineID")
		mTempAssemblyList = AssemblyList.GetAssemblyList(1, MachineID)
		Session("mTempAssemblyList") = mTempAssemblyList

		mATAList = ATAList.GetATAList("", "(SELECT)")
		Session("mATAList") = mATAList
		cmbATAChapter.DataSource = mATAList

		If Not mMELSnagCorrectiveAction Is Nothing And Not mMELSnagCorrectiveAction.IsNew Then
			Dim tmpLogDetail As Log
			tmpLogDetail = Log.GetLog(mMELSnagCorrectiveAction.LogID)

			mReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(tmpLogDetail.Date.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString), "1/1/2100", mTempAssemblyList(0).ID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, False, , 0, , , , "(SELECT)", True, , True)
		ElseIf (Session("ForDateOfOccurance") = "ForDateOfOccurance") Then
			Session.Remove("ForDateOfOccurance")
			mReportLogRegister = ReportLogRegister.GetRectifiedLog(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, "1/1/2100", mTempAssemblyList(0).ID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, False, , 0, , , , "(SELECT)", True, , True)
		Else
			mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtDateofOccurrence.Text, "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID, False, , 0, , , , "(SELECT)", True, , True)
		End If

		cmbLogNo.DataSource = mReportLogRegister
		Session("mReportLogRegister") = mReportLogRegister
		upnlLogNo.Update()
		cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList("(SELECT)")
		cmbMELCategory.DataBind()
		cmbPartNo.Items.Clear()

		If Not mMELSnagCorrectiveAction Is Nothing And Not mMELSnagCorrectiveAction.IsNew Then
			mMELSnagPartList = MELSnagPartList.GePartList(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, MachineID, "(SELECT)")
		Else
			mMELSnagPartList = MELSnagPartList.GePartList(txtDateofOccurrence.Text, MachineID, "(SELECT)")
		End If

		cmbPartNo.DataSource = mMELSnagPartList
		Session("mMELSnagPartList") = mMELSnagPartList

		cmbPartNo.DataSource = mMELSnagPartList
		If Not mMELSnagPartList.Contains(mMELSnagCorrectiveAction.PartNo) Then mMELSnagCorrectiveAction.PartID = Guid.Empty
		'End
		cmbPartNo.DataSource = mMELSnagPartList
		cmbPartNo.DataBind()

		If mMELSnagCorrectiveAction.IsMEL Then
			cmbATAChapter.Enabled = False
			cmbSubATAList.Enabled = False
		Else
			cmbATAChapter.Enabled = True
			cmbSubATAList.Enabled = True
		End If

		If Not mMELSnagCorrectiveAction Is Nothing Then

			If Not mMELSnagCorrectiveAction.IsNew Then
				Dim tmpLogDetail As Log = Log.GetLog(mMELSnagCorrectiveAction.LogID)
				mRectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(tmpLogDetail.Date.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString), "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID, False, , 0, , , , "(SELECT)", True, mMELSnagCorrectiveAction.LogID.ToString)
				tmpLogDetail = Nothing
			End If

		End If

		cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
		Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister

		'Added By Vikrant On 02-Apr-2013 For ALL01042013
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
		cmbSubATAList.DataSource = mSubATAList
		Session("mSubATAList") = mSubATAList
		'End

		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, MachineID.ToString, mMELSnagCorrectiveAction.DateOfOccurrence.ToString, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		'End

		cmbIncidentType.DataSource = IncidentTypeList.GetIncidentTypeList() 'Added By Prashant On 23-Nov-2021 ALL23112021
		cmbIncidentType.DataBind()
		BindLicenceNo() 'MLNo
		DataBind()
		If cmbATAChapter.SelectedIndex > 0 Then cmbSubATAList.SelectedValue = mMELSnagCorrectiveAction.SubATAID.ToString
		cmbRectifiedLogNo.SelectedValue = mMELSnagCorrectiveAction.RectifiedLogID.ToString

		If Not mMELSnagCorrectiveAction Is Nothing Then
			txtDateofOccurrence.Text = IIf(mMELSnagCorrectiveAction.DateOfOccurrence Is DBNull.Value, "", mMELSnagCorrectiveAction.DateOfOccurrenceFormatted) 'mMELSnagCorrectiveAction.DateOfOccurence
			txtDueDate.Text = IIf(mMELSnagCorrectiveAction.DateValue Is DBNull.Value, "", mMELSnagCorrectiveAction.DateValue) 'mMELSnagCorrectiveAction.DueDateFormatted
			txtRectifiedDate.Text = IIf(mMELSnagCorrectiveAction.RectifiedDateFormatted Is DBNull.Value, "", mMELSnagCorrectiveAction.RectifiedDateFormatted) ' mMELSnagCorrectiveAction.RectifiedDateFormatted
			cmbLogNo.SelectedValue = mMELSnagCorrectiveAction.LogID.ToString
		End If

	End Sub
	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim CustValid As CustomValidator
		CustValid = CType(s, CustomValidator)
		If CustValid.ControlToValidate = "cmbLogNo" Then
			If cmbLogNo.SelectedIndex = 0 Then
				CustValid.ErrorMessage = "Please select the Log"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbRectifiedLogNo" Then
			If (chkClose.Checked = True And txtRectifiedDate.Text = "") Then
				CustValid.ErrorMessage = "Please select the Rectification Date "
				e.IsValid = False
				tabMELLogDetailsContainer.ActiveTabIndex = 1
				upnlTabs.Update()
			ElseIf (chkClose.Checked = True And txtRectifiedDate.Text <> "" And cmbRectifiedLogNo.SelectedIndex = 0) Then
				CustValid.ErrorMessage = "Select Rectified Log No."
				e.IsValid = False
				tabMELLogDetailsContainer.ActiveTabIndex = 1
				upnlTabs.Update()
			Else
				e.IsValid = True
			End If
			'ElseIf txtDefect.Text.Length > 2500 And CustValid.ControlToValidate = "txtDefect" Then
			'    txtDefect.Text = txtDefect.Text.Substring(0, 2486) + "..."
			'    CustValid.ErrorMessage = "Defect Length must not be greater than 2500 character."
			'    e.IsValid = False
			'ElseIf txtAction.Text.Length > 2500 And CustValid.ControlToValidate = "txtAction" Then
			'    txtAction.Text = txtAction.Text.Substring(0, 2486) + "..."
			'    CustValid.ErrorMessage = "Action Length must not be greater than 2500 character."
			'    e.IsValid = False
		ElseIf CustValid.ControlToValidate = "cmbATAChapter" Then
			If (chkIsInReliability.Checked = True And cmbATAChapter.SelectedIndex = 0) Then
				CustValid.ErrorMessage = "Select the ATA Chapter as it is to be considered in Reliability."
				e.IsValid = False
				tabMELLogDetailsContainer.ActiveTabIndex = 0
				upnlTabs.Update()
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "cmbMELCategory" Then
			If (chkShowMEL.Checked = True) And (cmbMELCategory.SelectedIndex = 0) Then
				CustValid.ErrorMessage = "Select the " & IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL") & " Category." 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf CustValid.ControlToValidate = "txtExtensionInDays" Then
			If (chkExtensionApplied.Checked = True And (txtExtensionInDays.Text = "0" Or txtExtensionInDays.Text = "")) Then
				CustValid.ErrorMessage = "Extension days should be greater than zero."
				e.IsValid = False
				tabMELLogDetailsContainer.ActiveTabIndex = 0
				upnlTabs.Update()
			Else
				e.IsValid = True
			End If
		End If
	End Sub
#End Region

#Region " Events "
	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		AddAttributes()

		If Not IsPostBack And Session("sender") = "" Then
			If txtDefectReportNo.Enabled = True Then
				SetFocus(txtDefectReportNo)
			End If
			BackPage = Request.QueryString("BackPage")
			Session("BackPage") = BackPage
			If mMELSnagCorrectiveAction.IsNew Then
				mMELSnagCorrectiveAction.DefectReportNo = GetDefectNo()
				If Session("mlnkCheckStatus") = True Then
					mMELSnagCorrectiveAction.DateOfOccurrence = Session("mDateofoccurrence")
					txtDateofOccurrence.Text = mMELSnagCorrectiveAction.DateOfOccurrence
					Session.Remove("mDateofoccurrence")
				Else
					Dim todayDate As SmartDate = New SmartDate(Today.Date.ToString)
					mMELSnagCorrectiveAction.DateOfOccurrence = todayDate.FormattedText
					txtDateofOccurrence.Text = todayDate.FormattedText
				End If
			Else
				ControlVisibilityAfterEdit()
				mMELSnagCorrectiveAction.DefectReportNo = mMELSnagCorrectiveAction.DefectReportNo
				mMELSnagCorrectiveAction.No = mMELSnagCorrectiveAction.No
			End If
			DataFieldBind()
			If chkShowMEL.Checked = False Then cmbMELCategory.Enabled = False
			If Not mMELSnagCorrectiveAction.IsNew Then cmbLogNo.SelectedValue = mMELSnagCorrectiveAction.LogID.ToString
			If cmbLogNo.SelectedIndex > 0 Then
				lnkCheckStatus.Enabled = True
			End If
			If mMELSnagCorrectiveAction.IsNew Then
				txtFrequencyInDay.Text = "0"
				txtFrequencyInHours.Text = ""
				chkIsInHours.Checked = False
				chkIsInHours.Enabled = False
				txtFrequencyInHours.Enabled = False
				txtFrequencyInDay.Enabled = False
				cmbMELCategory.Enabled = False
				chkShowMEL.Enabled = True
			Else
				chkShowMEL.Enabled = False
			End If
			SetLicenceCount()
			UserNameForLicenceList = User.Identity.Name
			Session("UserNameForLicenceList") = UserNameForLicenceList
			'End
		End If

		ControlVisibility()
		SetTitle()
	End Sub

	Private Sub Save()

		Try
			mMELSnagCorrectiveAction.Save()
			SaveAttachment()
			txtNo.DataBind()
			upnlMELSnagDetails.Update()

			If mMELSnagCorrectiveAction.IsNew = False Then
				btnPrint.Enabled = True
				btnSendMail.Visible = True

			End If

			mMELSnagDetail = mMELSnagCorrectiveAction.DefectNo + " Dated : " +
							 mMELSnagCorrectiveAction.DateOfOccurrenceFormatted +
							 " Log No. " + mMELSnagCorrectiveAction.LogNo
			MarkLog(Action.Save, "MEL / Snag Defect Corrective Action", mMELSnagDetail, ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
			lnkbtnCreateWorkOrder.Visible = (Not mMELSnagCorrectiveAction.InvestigationStatus And Not mMELSnagCorrectiveAction.IsNew)
			SetTitle()
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

		Try
			'Added by Saylee on 8-Apr-2014 for ALL08042014
			If (Not User.IsInRole("MELSnagCorrectiveActionNew") And Not User.IsInRole("MELSnagCorrectiveActionEdit")) Then
				SetSession()
				MarkLog(Action.Save, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to save ", ErrorType.HandledError, Guid.Empty, EventLogID)
				MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
				Exit Sub
			End If

			Page.Validate()
			If Not IsValid Then
				upnlErrorList.Update()
				upnlVerificationDetailsErrors.Update()
				upnlRectificationDetailsErrors.Update()
				Exit Sub
			End If

			SetObject()
			If mMELSnagCorrectiveAction.IsValid Then
				Try
					SetSession()
					'Added by Utkarsh ON 27-Feb-2013 FOR All27022013
					If mMELSnagCorrectiveAction.InvestigationStatus Then
						If ((CDate(mMELSnagCorrectiveAction.DateOfOccurrence) <= CDate(mMELSnagCorrectiveAction.RectifiedDate)) AndAlso (mRectifiedReportLogRegister.Item(New Guid(cmbRectifiedLogNo.SelectedValue)).IntLogNo < mReportLogRegister.Item(New Guid(cmbLogNo.SelectedValue)).IntLogNo)) Then
							MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, "Rectification can be done on same or later the occurance Log(TLP).", MsgBoxStyle.OkOnly, "")
							Exit Sub
						End If
					End If
					'End
					'Added By Prashant 2-Jan-2014  --ALL02012014-1
					If (cmbATAChapter.SelectedIndex > 0 And chkIsRepetitive.Checked = False) Then
						Dim mMELSnagCountATAWise As MELSnagCountATAWise
						mMELSnagCountATAWise = MELSnagCountATAWise.GetMELSnagCountATAWise(mMELSnagCorrectiveAction.ATAChapterID.ToString, mMELSnagCorrectiveAction.MachineID.ToString, mMELSnagCorrectiveAction.ID.ToString, Val(AppSettings("MEL_Occurrance_In_Days")), mMELSnagCorrectiveAction.DateOfOccurrenceFormatted, Val(AppSettings("MEL_Check_ON"))) 'Added config parameters by Saylee on  24-Feb-2020 for ALL24022020

						If mMELSnagCountATAWise.Item(0).MELSnagCount > 0 Then
							Dim MsgStr As String = String.Empty
							MsgStr = "There are " + mMELSnagCountATAWise.Item(0).MELSnagCount.ToString + IIf(AppSettings("MELSnagNomenclature") = "True", " ADD/Defect", " MEL/Snag") + " reported for this ATA. " + " Last Log Date is " + New SmartDate(mMELSnagCountATAWise.Item(0).LogInfo.ToString.Substring(0, mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(0).Trim.Length)).FormattedText + "<BR>" + " Log No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(1).Trim + "<BR>" + " Log Page No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(2).Trim + "<BR>" + " Do you want to make this " + IIf(AppSettings("MELSnagNomenclature") = "True", "Defect as Repetitive Defect", "Snag as Repetitive Snag") + "?"
							MSGBoxCtrl.Show(MSGBox.Message_title.Confirmation, MSGBox.Message_text.Confirmation, MsgStr, MsgBoxStyle.YesNo, "MELSnagCountATAWise")
							Exit Sub
						End If
					End If
					MSGBoxCtrl.Show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
					Save()
					lnkbtnCreateWorkOrder.Visible = (Not mMELSnagCorrectiveAction.InvestigationStatus And Not mMELSnagCorrectiveAction.IsNew)
					upnlMMELDetails.Update()
					upnlMELSnagDetails.Update()
					upnlTitle.Update()
					upnlCreateWO.Update()
					upnlHeaderButtons.Update()
					'Added By Harsh on 26th Feb 2024 -- Disable the Top 3 Controls Once the records is created 
					txtDefectReportNo.Enabled = mMELSnagCorrectiveAction.IsNew
					txtNo.Enabled = mMELSnagCorrectiveAction.IsNew
					txtDateofOccurrence.Enabled = mMELSnagCorrectiveAction.IsNew
					cmbLogNo.Enabled = mMELSnagCorrectiveAction.IsNew
				Catch ex As SqlException
					If ex.Number = 8145 Then
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
					ElseIf ex.Number = 2627 Then
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
					ElseIf ex.Number = 547 Then
						MarkLog(Action.Delete, "Log Defect Action", "Can't delete : This is Currently in use", ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
						MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
					End If
				End Try
			Else
				If Not mMELSnagCorrectiveAction.IsValid Then
					'Modified by Harsh on 5th March 2024 -- Based on the Group Name added in Description bifurcating the Validation Messages to show on UI accordingly
					For j As Integer = 0 To mMELSnagCorrectiveAction.GetBrokenRulesCollection.Count - 1

						If mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",").Length > 1 Then

							If mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(1).ToString.Trim.Equals("RectificationDetails", StringComparison.CurrentCultureIgnoreCase) Then
								RectificationDetailsErrorMessage += mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(0).ToString() + Environment.NewLine
							ElseIf mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(1).ToString.Trim.Equals("DiscrepancyDetails", StringComparison.CurrentCultureIgnoreCase) Then
								DiscrepancyDetailsErrorMessage += mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(0).ToString() + Environment.NewLine
							ElseIf mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(1).ToString.Trim.Equals("VerificationDetails", StringComparison.CurrentCultureIgnoreCase) Then
								VerificationDetailsErrorMessage += mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(0).ToString() + Environment.NewLine
							End If

						Else
							strMsg = strMsg + mMELSnagCorrectiveAction.GetBrokenRulesCollection(j).Description + Environment.NewLine
						End If

					Next
				End If

				If Not IsNothing(RectificationDetailsErrorMessage) Then
					cvRectificationDetails.ErrorMessage = RectificationDetailsErrorMessage
					cvRectificationDetails.IsValid = mMELSnagCorrectiveAction.IsValid
				End If

				If Not IsNothing(VerificationDetailsErrorMessage) Then
					cvVerificationDetails.ErrorMessage = VerificationDetailsErrorMessage
					cvVerificationDetails.IsValid = mMELSnagCorrectiveAction.IsValid
				End If

				If Not IsNothing(DiscrepancyDetailsErrorMessage) Then
					cvDiscrepancyDetails.ErrorMessage = DiscrepancyDetailsErrorMessage
					cvDiscrepancyDetails.IsValid = mMELSnagCorrectiveAction.IsValid
				End If

				If strMsg.Trim <> "" Then
					cvFrequencyInHours.ErrorMessage = strMsg
					cvDefectList.ErrorMessage = strMsg
					cvFrequencyInHours.IsValid = mMELSnagCorrectiveAction.IsValid
				End If

				upnlErrorList.Update()
				upnlVerificationDetailsErrors.Update()
				upnlRectificationDetailsErrors.Update()

			End If
		Catch ex As Exception
			ex.GetBaseException()
		End Try
	End Sub

	Private Sub CmbPartNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPartNo.SelectedIndexChanged

		If cmbPartNo.SelectedIndex <= 0 Then
			txtPartNo.Text = ""
			txtDescription.Text = ""
			txtSerialNo.Text = ""
			txtDescription.ReadOnly = False
			txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)

			txtPartNo.ReadOnly = False
			txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)

			txtSerialNo.ReadOnly = False
			txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)

		Else
			txtPartNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name
			txtDescription.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Description
			txtSerialNo.Text = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").SerialNo

			'Added By Saylee on 8-Aug-2019
			If mAssemblylist.Count > 0 And chkShowMEL.Checked Then
				If mAssemblylist.Contains(mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID, "") Then
					cmbAssembly.SelectedValue = mMELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID.ToString
					upnlMMELDetails.Update()
				End If
			End If
			'***************************************
			If chkShowMEL.Checked = False Then
				cmbATAChapter.SelectedValue = mMELSnagPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").CompStatusATAID.ToString
				cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
				If cmbATAChapter.SelectedIndex > 0 Then
					mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
					cmbSubATAList.DataSource = mSubATAList
					cmbSubATAList.DataBind()
				End If
			End If
			txtDescription.ReadOnly = True
			txtPartNo.ReadOnly = True
			txtSerialNo.ReadOnly = True

			txtDescription.BackColor = Color.FromName("#E0E0E0")
			txtPartNo.BackColor = Color.FromName("#E0E0E0")
			txtSerialNo.BackColor = Color.FromName("#E0E0E0")
		End If

		If txtDescription.Text.Length > 30 Then
			txtDescription.TextMode = TextBoxMode.MultiLine
		Else
			txtDescription.TextMode = TextBoxMode.SingleLine
		End If
		If cmbPartNo.Enabled = True Then
			SetFocus(cmbPartNo)
		End If

		upnlPartNo.Update()
		upnlDesc.Update()
		upnlSerialNo.Update()
		upnlATA.Update()
		upnlSubATA.Update()
		upnlMELCategory.Update()
		upnlFreq.Update()
		upnlDueDate.Update()
	End Sub

	Private Sub TxtDateOfOccurrence_TextChanged(sender As Object, e As EventArgs) Handles txtDateofOccurrence.TextChanged
		Session("mDateofoccurrence") = txtDateofOccurrence.Text

		'Here if New then consider Occurrence Date for binding else
		'if Old: then get Lesser date from LogDate or Occurrence date

		If Not IsDate(txtDateofOccurrence.Text) Then
			MSGBoxCtrl.Show("Alert !!", "Please select valid Date of Occurrence.", "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		If mMELSnagCorrectiveAction.IsNew Then
			mReportLogRegister = ReportLogRegister.GetRectifiedLog(txtDateofOccurrence.Text, "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID, False, , 0, , , , "(SELECT)", True, , True)
		Else
			Dim tmpLogDetail As Log
			tmpLogDetail = Log.GetLog(mMELSnagCorrectiveAction.LogID)
			mReportLogRegister = ReportLogRegister.GetRectifiedLog(Get_Whichever_LesserDate(tmpLogDetail.Date.ToString, txtDateofOccurrence.Text), "1/1/2100", mTempAssemblyList(0).ID.ToString, MachineID, False, , 0, , , , "(SELECT)", True, , True)
		End If

		cmbLogNo.DataSource = mReportLogRegister
		Session("mReportLogRegister") = mReportLogRegister
		cmbLogNo.DataBind()

		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofOccurrence.Text
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		Else
			mMELSnagCorrectiveAction.DateOfOccurrence = txtDateofOccurrence.Text
			mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
			If chkShowMEL.Checked = True And (txtFrequencyInDay.Text <> "0") Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			ElseIf chkShowMEL.Checked = True And cmbMELCategory.SelectedIndex = 1 Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		End If
		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, MachineID.ToString, txtDateofOccurrence.Text, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		cmbAssembly.DataBind()
		'End
		cmbRectifiedLogNo.DataSource = mRectifiedReportLogRegister
		Session("mRectifiedReportLogRegister") = mRectifiedReportLogRegister
		cmbRectifiedLogNo.DataBind()


		If Not mMELSnagCorrectiveAction Is Nothing And Not mMELSnagCorrectiveAction.IsNew Then
			mMELSnagPartList = MELSnagPartList.GePartList(mMELSnagCorrectiveAction.DateOfOccurrence.ToString, MachineID, "(SELECT)")
		Else
			mMELSnagPartList = MELSnagPartList.GePartList(txtDateofOccurrence.Text, MachineID, "(SELECT)")
		End If
		cmbPartNo.DataSource = mMELSnagPartList
		Session("mMELSnagPartList") = mMELSnagPartList

		cmbPartNo.DataSource = mMELSnagPartList


		cmbPartNo.DataSource = mMELSnagPartList
		cmbPartNo.DataBind()
		If Not mMELSnagPartList.Contains(mMELSnagCorrectiveAction.PartNo) Then
			mMELSnagCorrectiveAction.PartID = Guid.Empty
			txtDescription.Text = ""
			txtPartNo.Text = ""
			txtSerialNo.Text = ""
		Else
			cmbPartNo.SelectedValue = mMELSnagPartList(txtPartNo.Text).ID.ToString

		End If

		'Added by Saylee on 25-Nov-2014 to reset rectification details on date change
		chkClose.Checked = False
		txtRectifiedDate.Text = ""
		txtRectifiedDate.ReadOnly = True
		If Not mRectifiedReportLogRegister Is Nothing Then cmbRectifiedLogNo.SelectedIndex = 0
		cmbRectifiedLogNo.Enabled = False
		txtRectificationSector.Text = ""
		mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty

		upnlClose.Update()
		upnlRectifiedDate.Update()
		upnlRectifiedCombo.Update()
		upnlLogNo.Update()
		upnlDueDate.Update()
		upnlMELSnagDetails.Update()
	End Sub

	Private Sub CmbLogNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLogNo.SelectedIndexChanged
		If cmbLogNo.SelectedIndex > 0 Then
			lnkCheckStatus.Enabled = True
			mMELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbLogNo.SelectedValue.ToString)
			Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
			With mMELSnagCorrectiveActionLog
				txtSector.Text = mMELSnagCorrectiveActionLog.Item(0).SourceName
				Session("tmpLogDate") = mMELSnagCorrectiveActionLog.Item(0).LogDate
				If mMELSnagCorrectiveActionLog.Item(0).FinalLandings = "" Then
					txtLastMajorCheck.Text = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H"
				Else
					txtLastMajorCheck.Text = mMELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + mMELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
				End If
			End With
			FillRectifiedCombo()
			upnlRectifiedDate.Update()
			upnlRectifiedCombo.Update()
			upnlSector.Update()
			upnlLastMajorCheck.Update()
		Else
			txtSector.Text = ""
			txtLastMajorCheck.Text = ""
			Dim todayDate As SmartDate = New SmartDate(Today.Date.ToString)
			txtDateofOccurrence.Text = todayDate.FormattedText  '---  All05022013-1 Added by Prashant 5-Feb-2013
			upnlSector.Update()
			upnlLastMajorCheck.Update()
		End If
		'Added By Vikrant On 02-Sept-2014 For All04092014
		mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, MachineID.ToString, txtDateofOccurrence.Text, "", True)
		cmbAssembly.DataSource = mAssemblylist
		Session("mAssemblylist") = mAssemblylist
		cmbAssembly.DataBind()
		'End
		If cmbLogNo.Enabled = True Then
			SetFocus(cmbLogNo)
		End If


	End Sub

	Private Sub CmbRectifiedLogNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRectifiedLogNo.SelectedIndexChanged
		If cmbRectifiedLogNo.SelectedIndex > 0 Then
			mMELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbRectifiedLogNo.SelectedValue.ToString)
			Session("mMELSnagCorrectiveActionLog") = mMELSnagCorrectiveActionLog
			With mMELSnagCorrectiveActionLog
				txtRectificationSector.Text = mMELSnagCorrectiveActionLog.Item(0).DestinationName
			End With
		Else
			txtRectificationSector.Text = ""
		End If
		If cmbRectifiedLogNo.Enabled = True Then
			SetFocus(cmbRectifiedLogNo)
		End If
	End Sub

	Private Sub ChkClose_CheckedChanged(sender As Object, e As EventArgs) Handles chkClose.CheckedChanged
		If cmbLogNo.SelectedIndex = 0 And chkClose.Checked = True Then
			MSGBoxCtrl.Show("Alert !!", "Please select the Log", "", MsgBoxStyle.OkOnly, "Close")
			Exit Sub
		End If

		If chkClose.Checked Then
			txtRectifiedDate.ReadOnly = False
			cmbRectifiedLogNo.Enabled = True
		Else
			txtRectifiedDate.Text = ""
			txtRectifiedDate.ReadOnly = True
			cmbRectifiedLogNo.SelectedIndex = 0
			cmbRectifiedLogNo.Enabled = False
			txtRectificationSector.Text = ""
			mMELSnagCorrectiveAction.RectifiedLogID = Guid.Empty
		End If
		upnlRectifiedDate.Update()
		upnlRectifiedCombo.Update()
	End Sub

	Private Sub TxtFrequencyInDay_TextChanged(sender As Object, e As EventArgs) Handles txtFrequencyInDay.TextChanged

		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = ""
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If

		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked

		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text.Trim
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		Else
			mMELSnagCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text.Trim)  'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlDueDate.Update()
	End Sub

	Private Sub TxtFrequencyInHours_TextChanged(sender As Object, e As EventArgs) Handles txtFrequencyInHours.TextChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = ""
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		Else
			mMELSnagCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text.Trim)  'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlDueDate.Update()
	End Sub

	Private Sub ChkIsInHours_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsInHours.CheckedChanged
		If chkIsInHours.Checked = True Then
			txtFrequencyInDay.Text = ""
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = True
		Else
			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
			txtFrequencyInDay.Enabled = True
			txtFrequencyInHours.Enabled = False
		End If
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked

		If chkIsInHours.Checked = True Then
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		Else
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString 'DateAdd(DateInterval.Day, Val(txtFrequencyInDay.Text), CDate(txtDateofoccurrence.Text))
		End If
		upnlDueDate.Update()
	End Sub

	Private Sub CmbMELCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMELCategory.SelectedIndexChanged

		If cmbMELCategory.SelectedIndex > 0 Then mMELSnagCorrectiveAction.MELCategoryID = cmbMELCategory.SelectedValue
		txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
		mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked

		If cmbMELCategory.SelectedIndex = 1 Then
			chkIsInHours.Enabled = True
			txtFrequencyInDay.Enabled = True
			mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
			mMELSnagCorrectiveAction.FrequencyInDays = txtFrequencyInDay.Text
			If txtFrequencyInDay.Text <> "" Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		ElseIf cmbMELCategory.SelectedIndex = 0 Then
			txtFrequencyInDay.Enabled = False
			txtFrequencyInHours.Enabled = False
			chkIsInHours.Enabled = False

			txtFrequencyInHours.Text = ""
			txtFrequencyInDay.Text = "0"
			txtDueDate.Text = ""
			If chkIsInHours.Checked = True Then
				chkIsInHours.Checked = False
			End If
		Else
			If chkIsInHours.Checked = True Then
				chkIsInHours.Checked = False
			End If
			chkIsInHours.Enabled = False
			txtFrequencyInDay.Enabled = True

			If txtFrequencyInHours.Text <> "" Then
				txtFrequencyInHours.Text = ""
			End If
			txtFrequencyInHours.Enabled = False
			mMELSnagCorrectiveAction.IsHours = chkIsInHours.Checked
			mMELSnagCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString

		End If
		upnlFreq.Update()
		upnlDueDate.Update()
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		GetSession()
		Session.Remove("AircraftRegNo")
		Session.Remove("mDateofoccurrence")
		Session.Remove("mlnkCheckStatus")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("wfMELSnagCorrectiveActionNew_AJAX")
		Session.Remove("URLFromDueReportPreview")
		SetObject()

		If mMELSnagCorrectiveAction.IsDirty Then
			MSGBoxCtrl.Show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
		ElseIf Request.QueryString("BackPage1") = "wfnWODetail_AJAX.aspx" Then
			Response.Redirect(Request.QueryString("BackPage1"))
		Else
			mMELSnagDetail = mMELSnagCorrectiveAction.DefectNo + " Dated : " + mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + mMELSnagCorrectiveAction.LogNo
			MarkLog(Action.Close, "MEL / Snag Defect Corrective Action", mMELSnagDetail, ErrorType.NoError, mMELSnagCorrectiveAction.ID, EventLogID)
			Session("sender") = ""
			Session.Remove("mFileAttach")
			Session.Remove("mMaintenanceDoneByEmployees")
			Session.Remove("UserNameForLicenceList")
			Response.Redirect(Request.QueryString("BackPage"))
		End If
	End Sub

	Private Sub LnkCheckStatus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCheckStatus.Click
		SetObject()
		mMELSnagCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text)
		Session("mDateofoccurrence") = mMELSnagCorrectiveAction.DateOfOccurrence
		Session("mlnkCheckStatus") = True
		If cmbLogNo.SelectedIndex > 0 Then
			Session("mTempLogID") = cmbLogNo.SelectedValue.ToString
			'Response.Redirect("wfMELSnagCorrectiveActionLogInfo.aspx?BackPage1=wfMELSnagCorrectiveActionNew_Ajax.aspx" & "&BackPage=" & Request.QueryString("BackPage"))
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
		End If
	End Sub

	Private Sub chkShowMEL_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowMEL.CheckedChanged

		If chkShowMEL.Checked Then
			Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMELMasterWindow", "OpenMELMasterWindow()", True)
		Else
			cmbMELCategory.SelectedIndex = 0
			txtDefect.Text = ""
			cmbATAChapter.SelectedIndex = 0
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
			mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
			cmbSubATAList.DataSource = mSubATAList
			cmbSubATAList.DataBind()
			Session("mSubATAList") = mSubATAList
			upnlSubATA.Update()
			txtFrequencyInDay.Text = 0
			txtFrequencyInHours.Text = ""
			chkIsInHours.Checked = False
			txtDueDate.Text = ""
			chkExtensionApplied.Checked = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			ControlVisibilityAfterEdit()
			upnlMMELDetails.Update()
		End If

	End Sub

	Private Sub CmbATAChapter_SelectedIndexChanged(sender As Object, ByVal e As EventArgs) Handles cmbATAChapter.SelectedIndexChanged
		mMELSnagCorrectiveAction.SubATAID = Guid.Empty
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		mSubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
		cmbSubATAList.DataSource = mSubATAList
		cmbSubATAList.DataBind()
		Session("mSubATAList") = mSubATAList
		upnlSubATA.Update()
	End Sub
	'End
	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'MLNo
	Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
		If IsValid Then
			SetObject()
			Session("mMaintenanceID") = mMELSnagCorrectiveAction.ID
			mMaintenanceDoneByEmployees = mMELSnagCorrectiveAction.MaintenanceDoneByEmployees
			Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
			Session("MaintenanceDoneOnDate") = mMELSnagCorrectiveAction.DateOfOccurrence.ToString
			ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
		Else
			upnlErrorList.Update()
			upnlVerificationDetailsErrors.Update()
			upnlRectificationDetailsErrors.Update()
		End If

	End Sub

	Private Sub HdnImgBtnMELMasterChapter_Click(sender As Object, e As EventArgs) Handles hdnimgBtnMELMasterChapter.Click
		mMELSnagCorrectiveAction = Session("mMELSnagCorrectiveAction")

		If mMELSnagCorrectiveAction.MELCategoryID <= 0 Then
			chkShowMEL.Checked = False
		Else
			chkShowMEL.Checked = True
		End If

		cmbMELCategory.SelectedValue = mMELSnagCorrectiveAction.MELCategoryID
		txtDefect.Text = mMELSnagCorrectiveAction.Defect
		cmbATAChapter.SelectedValue = mMELSnagCorrectiveAction.ATAChapterID.ToString
		cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
		mSubATAList = SubATAList.GetSubATAList(mMELSnagCorrectiveAction.ATAChapterID, "", "(SELECT)")
		cmbSubATAList.DataSource = mSubATAList
		cmbSubATAList.DataBind()
		Session("mSubATAList") = mSubATAList
		upnlSubATA.Update()
		cmbSubATAList.SelectedValue = mMELSnagCorrectiveAction.SubATAID.ToString
		txtFrequencyInDay.Text = mMELSnagCorrectiveAction.FrequencyInDays
		txtFrequencyInHours.Text = mMELSnagCorrectiveAction.FrequencyInHours
		chkIsInHours.Checked = mMELSnagCorrectiveAction.IsHours
		txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString ''mMELSnagCorrectiveAction.DateValue
		cmbIncidentType.SelectedValue = mMELSnagCorrectiveAction.IncidentTypeID
		ControlVisibilityAfterEdit()
		upnlMMELDetails.Update()
	End Sub

	Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As EventArgs) Handles hdnBtnMaintDoneBy.Click
		For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
			Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
			If Not mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
			ElseIf mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
				'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
			End If
		Next

		For j As Integer = 0 To mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count - 1
			If Not mMaintenanceDoneByEmployees.Contains(mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(j).ID) Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Remove(mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(j).ID, "")
			End If
		Next
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		BindLicenceNo()
		SetLicenceCount() 'MLNo
		upnlLicenceNo.Update()
	End Sub

	Protected Sub txtLicenceNo_TextChanged(sender As Object, e As EventArgs)
		'SetObject()
		If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
			LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
			EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
		Else
			LicenseNo = Trim(txtLicenceNo.Text)
		End If
		DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
		Session("LicenseNo") = LicenseNo
		Session("EmployeeID") = DoneByID
		If Not DoneByID.Equals(Guid.Empty) Then
			If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
			Else
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Add(mMELSnagCorrectiveAction.ID, 11, DoneByID, LicenseNo, "", EmpName)
			End If

		Else
			If mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
				mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.RemoveAt(0)
			End If
		End If
		Session("mMELSnagCorrectiveAction") = mMELSnagCorrectiveAction
		BindLicenceNo()
		SetLicenceCount()
	End Sub
	'End
	Private Sub LnkMELDetail_Click(sender As Object, e As EventArgs) Handles lnkMELDetail.Click
		Dim mMEL As MEL
		mMEL = MEL.GetMEL(mMELSnagCorrectiveAction.MELID)
		mMEL.MarkClean()
		Session("mMEL") = mMEL
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMELDetail", "OpenMELDetail();", True)
	End Sub

	Private Sub chkExtensionApplied_CheckedChanged(sender As Object, e As EventArgs) Handles chkExtensionApplied.CheckedChanged
		If chkExtensionApplied.Checked = True Then
			txtExtensionInDays.Enabled = True
			txtExtensionApprovalNo.Enabled = True
		Else
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlDueDate.Update()
		upnlMMELDetails.Update()
		upnlExtension.Update()
	End Sub

	Private Sub TxtExtensionInDays_TextChanged(sender As Object, e As EventArgs) Handles txtExtensionInDays.TextChanged
		If chkExtensionApplied.Checked = True Then
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			If chkShowMEL.Checked = True Then
				txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
			Else
				txtDueDate.Text = ""
			End If
		Else
			txtExtensionInDays.Enabled = False
			txtExtensionApprovalNo.Enabled = False
			txtExtensionInDays.Text = 0
			txtExtensionApprovalNo.Text = ""
			mMELSnagCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
			txtDueDate.Text = mMELSnagCorrectiveAction.DateValue.ToString
		End If
		upnlDueDate.Update()
		upnlExtension.Update()
	End Sub

	Public Sub SetUserMailIDs()
		Session("UserEmailID") = mModuleList.Item("MELSnagCorrectiveAction").SendToMailID
		Session("UserCcEmailID") = mModuleList.Item("MELSnagCorrectiveAction").SendCCMailID
		Session("MailsRequire") = mModuleList.Item("MELSnagCorrectiveAction").MailsRequire
		Session("SmtpHost") = mModuleList.Item("MELSnagCorrectiveAction").SmtpHost
		Session("SmtpPort") = mModuleList.Item("MELSnagCorrectiveAction").SmtpPort
		Session("SmtpUser") = mModuleList.Item("MELSnagCorrectiveAction").SmtpUser
		Session("SmtpPassword") = mModuleList.Item("MELSnagCorrectiveAction").SmtpPassword
	End Sub

	Private Sub BtnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click
		Try
			Dim Str As String
			SetUserMailIDs()
			Session("btnSendMail") = "btnSendMail"
			Str = "OpenByMainWindow();"
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenByMainWindow", Str, True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try
	End Sub

	Private Sub SendMail(sender As Object, e As EventArgs) Handles hdnImgBtnSendMail.Click
		Try
			Dim str As String
			Dim mSendMailFile As New SendMailFile

			str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following New " +
						 IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") +
						 " has been added in FlyPal System and need your attention." + "</font></P></br> ")

			str = str + "<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> "

			str = str + "<p><font face=""Calibri"">"
			str = str + "<b> Aircraft : </b>" + mMELSnagCorrectiveAction.RegNo + "<b>" + "  Log No : " + "</b>" + mMELSnagCorrectiveAction.LogNo
			str = str + "</font></p>"

			str = str + "<p><font face=""Calibri"">"
			str = str + ("<b>Defect No. : " + "</b>" + mMELSnagCorrectiveAction.DefectNo + "<b>  Date of Occurrence : </b>" +
						 mMELSnagCorrectiveAction.DateOfOccurrenceFormatted + "<b>" + "  Defect : " + "</b>" + mMELSnagCorrectiveAction.Defect)
			str = str + "</font></p>"

            str = str + "<p><font face=""Calibri"">"
            str = str + "<b>" + " Name of Pilot / AME  &  License No. / Observed By : " + "</b>" + mMELSnagCorrectiveAction.ReportedBy
            str = str + "</font></p>"
            str = str + "</body></html>"

            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name,
                                      IIf(AppSettings("MELSnagNomenclature") = "True", "New ADD/Defect Notification", "New MEL/Snag Notification"), ,
                                      str, "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True,
                                      Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                                      SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"),
                                      SmtpPassword:=Session("SmtpPassword"))

            Dim mDirectiveDetail As String = "New " + IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") + " Notification sent successfully to " + Session("ToSendMailIDs") + " by " + User.Identity.Name
            MarkLog(Action.SendMail, "MELSnagCorrectiveAction", mDirectiveDetail, ErrorType.HandledError, mMELSnagCorrectiveAction.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, [GetType], "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        Finally
            Session.Remove("mModelMonitorModtmp")
        End Try

    End Sub

    Private Sub LnkBtnCreateWorkOrder_Click(sender As Object, e As EventArgs) Handles lnkbtnCreateWorkOrder.Click

        Dim mnWO As nWO
        Dim tmpAssemblyStatusList As AssemblyStatusList
        Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList

        Try
            If mMELSnagCorrectiveAction.IsWOCreated Then

                mnWO = nWO.GetWO(mMELSnagCorrectiveAction.WOID, False)
                Session("mnWO") = mnWO
                Session("IsShowAllWOs") = True
            Else


                mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
                mnWO.WODate = CDate(txtDateofOccurrence.Text)
                mnWO.MachineID = mReportLogRegister(New Guid(cmbLogNo.SelectedValue)).MachineID

                If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
                    Dim TempRegNo As String = ""
                    TempRegNo = mReportLogRegister(New Guid(cmbLogNo.SelectedValue)).RegNo
                    mnWO.WOText = Replace(TempRegNo, "VT-", "")
                    If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
                        mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
                    End If
                ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    mnWO.WOText = "MJO# " & CStr(CDate(txtDateofOccurrence.Text).Date.Year) & " - " & mnWO.ModelName
                ElseIf AppSettings("ClientCode") = "TP" Then
                    mnWO.WOText = Replace(mReportLogRegister(New Guid(cmbLogNo.SelectedValue)).RegNo, "VT-", "") & "/" & CStr(CDate(txtDateofOccurrence.Text).Date.Year)
                End If


                tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDateofOccurrence.Text.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

                mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

                mnWO.WOJobs.Add(mnWO.ID, 3)
                mnWO.WOJobs.CurrentItem.PreviousTransID = mMELSnagCorrectiveAction.ID

				mnWO.WOJobs.CurrentItem.DateOfOccurrence = mMELSnagCorrectiveAction.DateOfOccurrence
				mnWO.WOJobs.CurrentItem.MELCategoryID = mMELSnagCorrectiveAction.MELCategoryID

                mnWO.WOJobs.CurrentItem.ATAChapterID = mMELSnagCorrectiveAction.ATAChapterID
                mnWO.WOJobs.CurrentItem.IsUnderMEL = mMELSnagCorrectiveAction.IsMEL
                mnWO.WOJobs.CurrentItem.CompID = mMELSnagCorrectiveAction.PartID

                mnWO.WOJobs.CurrentItem.IsMajor = mMELSnagCorrectiveAction.IsMajor

                mnWO.WOJobs.CurrentItem.IsHours = mMELSnagCorrectiveAction.IsHours
                mnWO.WOJobs.CurrentItem.FrequencyInDays = mMELSnagCorrectiveAction.FrequencyInDays
                mnWO.WOJobs.CurrentItem.FrequencyInHours = mMELSnagCorrectiveAction.FrequencyInHours

                mnWO.WOJobs.CurrentItem.IsRepetitive = mMELSnagCorrectiveAction.IsRepetitive
                Dim Description As String = ""
				Description = mMELSnagCorrectiveAction.Description & "<BR>" & mMELSnagCorrectiveAction.LogNo & "<BR>" & mMELSnagCorrectiveAction.Defect & "<BR>" & "Date Of Occurence : " & mMELSnagCorrectiveAction.DateOfOccurrence

				'Component
				If mMELSnagCorrectiveAction.PartName <> "" Then Description = Description & "<BR>" & "On Part : " & mMELSnagCorrectiveAction.PartName

                'MEL Category
                If mMELSnagCorrectiveAction.MELCategoryName <> "" Then
                    Description = Description & "<BR>" & IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category : ", "MEL Category : ") & mMELSnagCorrectiveAction.MELCategoryName & "with "
                    If mMELSnagCorrectiveAction.FrequencyInDays <> 0 Then
                        Description = Description & mMELSnagCorrectiveAction.FrequencyInDays & " Days"
                    Else
                        Description = Description & mMELSnagCorrectiveAction.FrequencyInHours & " Hours"
                    End If
                End If
                mnWO.WOJobs.CurrentItem.WOJobDescription = Description.Replace("<BR>", vbCrLf)
                mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Trim(Description.Replace("<BR>", vbCrLf))
                mnWO.WOJobs.CurrentItem.DueAsOf = mMELSnagCorrectiveAction.DueDateFormatted.ToString

                If AppSettings("ShowCAMOOnlyForNewClients") = "True" Then
                    mnWO.WOJobs.CurrentItem.TaskCardNo = mMELSnagCorrectiveAction.DefectNo
                End If
                Session("mnWO") = mnWO

            End If
            Dim URLFromDueReportPreview As Stack = New Stack
            URLFromDueReportPreview.Push(Request.Url)
            Session("wfMELSnagCorrectiveActionNew_AJAX") = "wfMELSnagCorrectiveActionNew_AJAX"
            Session("URLFromDueReportPreview") = URLFromDueReportPreview
            Response.Redirect("wfnWODetail_Ajax.aspx?BackPage=index.aspx")

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

#Region " Report "
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        Try
            'Added by Saylee on 8-Apr-2014 for ALL08042014
            If (Not User.IsInRole("MELSnagCorrectiveActionPrint")) Then
                SetSession()
                MarkLog(Action.Print, "MELSnagCorrectiveAction", User.Identity.Name & " is not Authorized User to print ", ErrorType.HandledError, Guid.Empty, EventLogID)
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                Exit Sub
            End If

            Dim Rpt As Engine.ReportClass
            Dim ds As New dsMELSnagCorrectiveAction
            Dim da As New ObjectAdapter
            Dim mCompanyDetail As New CompanyDetail
            Dim mrptMELSnagCorrectiveAction As rptMELSnagCorrectiveAction
            mrptMELSnagCorrectiveAction = rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID.ToString)
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                                         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                         mCompanyDetail.Email, mCompanyDetail.WebSite,
                                         "PRELIMINARY DEFECT REPORT", "", "", "", "", "",
                                         AppSettings("Product Version"), AppSettings("SINote"),
                                         "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

            If mMELSnagCorrectiveAction.IsMEL = True Then
                Rpt = New crMELDetailReport
            Else
                Rpt = New crLogDefectActionList
            End If
            '-----------Added by Utkarsh for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, mrptMELSnagCorrectiveAction)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
            Rpt.SetDataSource(ds)
            Session("CrystalReport") = Rpt
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub
    Private Sub AttachmentICN_Click(sender As Object, e As ImageClickEventArgs) Handles attachmentICN.Click
        ViewImage()
    End Sub
    Private Sub HdnBtnFileUpload_Click(sender As Object, ByVal e As EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub BtnDelAttach_Click(sender As Object, ByVal e As EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        attachmentICN.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
#End Region

#Region "Service Methods"

    <Services.WebMethod(), Script.Services.ScriptMethod()>
    Public Shared Function GetLicenceList(prefixText As String, count As Integer, contextKey As String) As String()

        Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, "", , , True)
        Try
            If count = 0 Then
                Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                        Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
            Else
                Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
                        Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
            End If
        Catch ex As Exception
            ex.GetBaseException()
        End Try

        Return Nothing

    End Function

#End Region

End Class