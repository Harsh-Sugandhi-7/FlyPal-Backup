'******************************************************
'Created by : Saylee 
'Dated      : 25-Feb-2025
'******************************************************

Public Class wfInspectionForWatchItem
	Inherits Page

#Region " ENUM "

	Private Enum LinkAction
		MakeApplicable = 1
		MakeApplicableAndStart = 2
		MakeNotApplicable = 3
		Comply = 4
		DoNothing = 5
	End Enum

#End Region

#Region " Variable Declaration "

	Public mAssemblyMonitorInspStatusThreshold As AssemblyMonitorInspStatus
	Public mModelMonitorInspThreshold As ModelMonitorInsp
	Public mAssemblyMonitorInspStatusInterval As AssemblyMonitorInspStatus
	Public mModelMonitorInspInterval As ModelMonitorInsp

	Public mInspTypeList As InspTypeList
	Public mATAList As ATAList

	Public mSelectPeriodUnits As SelectPeriodUnits
	Public mModelMonitorInspTypeList As ModelMonitorInspTypeList
	Public mModelMonitorInspPeriodUnitList As ModelMonitorInspPeriodUnitList

	Dim Flag As Int16
	Public mAssemblyStatus As AssemblyStatus
	Public mMachine As Machine
	Dim EventLogID As Guid

	Public mUnit As String
	Public mModel As String
	Public mMonitorType As String
	Public mDescription As String
	Public mDetail As String
	Dim mFileAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	Dim mModuleList As ModuleList

	Dim mMPDTypeList As MPDTypeList
	Dim mMPDSkillList As MPDSkillList

	Dim mLastMPDRef As LastMPDAMPRef
	Dim RegNo As String
	Dim LicenseNoThreshold As String = String.Empty
	Dim LicenseNoInterval As String = String.Empty

	Dim EmpNameThreshold As String = String.Empty
	Dim EmpNameInterval As String = String.Empty

	Dim DoneByIDThreshold As Guid = Guid.Empty
	Dim DoneByIDInterval As Guid = Guid.Empty
	Dim mFromEditThresholdInterval As String = ""
	Dim AirframeCurrentValues As String = ""

	Public mAssemblyMonitorInspStatusNA As AssemblyMonitorInspStatus
	Public mModelMonitorInspNA As ModelMonitorInsp
	Dim mLinkMaintenanceList As LinkMaintenanceList

	Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
	Public mModelMonitorInsp As ModelMonitorInsp
	Public mCloseDate As String

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mAssemblyMonitorInspStatusThreshold = CType(Session("mAssemblyMonitorInspStatusThreshold"), AssemblyMonitorInspStatus)
		mModelMonitorInspThreshold = CType(Session("mModelMonitorInspThreshold"), ModelMonitorInsp)
		mMachine = CType(Session("mMachine"), Machine)

		mAssemblyMonitorInspStatusInterval = CType(Session("mAssemblyMonitorInspStatusInterval"), AssemblyMonitorInspStatus)
		mModelMonitorInspInterval = CType(Session("mModelMonitorInspInterval"), ModelMonitorInsp)

		mATAList = CType(Session("mATAList"), ATAList)
		mModelMonitorInspTypeList = CType(Session("mModelMonitorInspTypeList"), ModelMonitorInspTypeList)
		mModelMonitorInspPeriodUnitList = CType(Session("mModelMonitorInspPeriodUnitList"), ModelMonitorInspPeriodUnitList)
		mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)

		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mModuleList = Session("mModuleList")
		mLastMPDRef = Session("mLastMPDRef")

		RegNo = Session("RegNo")
		mAssemblyStatus = Session("mAssemblyStatus")
		mFromEditThresholdInterval = Session("FromEditThresholdInterval")

		mModelMonitorInspNA = Session("mModelMonitorInspNA")
		mAssemblyMonitorInspStatusNA = Session("mAssemblyMonitorInspStatusNA")
		AirframeCurrentValues = Session("AirframeCurrentValues")
		mLinkMaintenanceList = Session("mLinkMaintenanceList")

		mModelMonitorInsp = Session("mModelMonitorInsp")
		mAssemblyMonitorInspStatus = Session("mAssemblyMonitorInspStatus")
		mCloseDate = Session("CloseDate")
	End Sub

	Private Sub SetSession()
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
		Session("mModelMonitorInspInterval") = mModelMonitorInspInterval

		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
		Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold

		Session("mAssemblyStatus") = mAssemblyStatus
		Session("mMachine") = mMachine

		Session("mATAList") = mATAList
		Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
		Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList

		Session("mSelectPeriodUnits") = mSelectPeriodUnits

		Session("mLastMPDRef") = mLastMPDRef
		Session("RegNo") = RegNo
		Session("mAssemblyStatus") = mAssemblyStatus
		Session("FromEditThresholdInterval") = mFromEditThresholdInterval
		Session("AirframeCurrentValues") = AirframeCurrentValues
		Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
		Session("mModelMonitorInsp") = mModelMonitorInsp
		Session("CloseDate") = mCloseDate
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mATAList")
		Session.Remove("mModelMonitorInspTypeList")
		Session.Remove("mSelectPeriodUnits")
		Session.Remove("URL")
		Session.Remove("MaintenanceActivityID")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")

		Session.Remove("mLastMPDRef")
		Session.Remove("RegNo")
		Session.Remove("mAssemblyStatus")
		Session.Remove("mAssemblyMonitorInspStatusInterval")
		Session.Remove("mModelMonitorInspInterval")

		Session.Remove("mAssemblyMonitorInspStatusThreshold")
		Session.Remove("mModelMonitorInspThreshold")
		Session.Remove("FromEditThresholdInterval")
		Session.Remove("AirframeCurrentValues")
		Session.Remove("mLinkMaintenanceList")
		Session.Remove("mModelMonitorInsp")
		Session.Remove("mAssemblyMonitorInspStatus")
		Session.Remove("CloseDate")
	End Sub

	Private Overloads Sub SetFocus(cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtReference" Then
			If Len(txtReference.Text) > 500 Then
				custValidator.ErrorMessage = "Reference Too Long"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "txtNote" Then
			If Len(txtNote.Text) > 1000 Then
				custValidator.ErrorMessage = "Note can't be more than 1000 chars."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		End If
	End Sub

	Private Sub SetObjectThreshold()

		Try

			mModelMonitorInspThreshold.Reference = Trim(txtReference.Text)
			mModelMonitorInspThreshold.Description = Trim(txtDescription.Text)
			mModelMonitorInspThreshold.ModelMonitorInspTypeID = CType(Val(mModelMonitorInspTypeList(Val(cmbType.SelectedValue.ToString), MonitorTypeID:=1).ID), Int32)
			mModelMonitorInspThreshold.Note = Trim(txtNote.Text)
			mModelMonitorInspThreshold.Zone = Trim(txtZone.Text)
			mModelMonitorInspThreshold.Area = Trim(txtArea.Text)
			mModelMonitorInspThreshold.Reference = Trim(txtReference.Text)
			mModelMonitorInspThreshold.ATAID = New Guid(cmbATAChapter.SelectedValue)

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then
					mModelMonitorInspThreshold.IsAttachmentAdded = True
				Else
					mModelMonitorInspThreshold.IsAttachmentAdded = False
				End If

			End If

			If AppSettings("SetModelCodeTypeWise") = "True" Then

				If Trim(txtAMPNo.Text).Length < 3 And Trim(txtAMPNo.Text) <> "" Then
					mModelMonitorInspThreshold.Code = Trim(txtAMPNo.Text).PadLeft(3, "0"c)
				Else
					mModelMonitorInspThreshold.Code = Trim(txtAMPNo.Text)
				End If

			Else
				mModelMonitorInspThreshold.Code = Trim(txtAMPNo.Text)
			End If

			Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetPeriodUnitsThreshold()
		mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits()
		Dim i As Int32
		Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
		'While i <= mPeriodUnitList.Count - 1
		'    If mModelMonitorInspThreshold.ModelMonitorInspPeriods.Contains(mPeriodUnitList(i).ID) = False Then
		'        mSelectPeriodUnits.Add(mPeriodUnitList(i).ID, mPeriodUnitList(i).PeriodID, mPeriodUnitList(i).PeriodUnitName)
		'    End If
		'    i = i + 1
		'End While

		While i <= mModelMonitorInspPeriodUnitList.Count - 1
			If mModelMonitorInspThreshold.ModelMonitorInspPeriods.Contains(mModelMonitorInspPeriodUnitList(i).ID) = False Then
				mSelectPeriodUnits.Add(mModelMonitorInspPeriodUnitList(i).ID, mModelMonitorInspPeriodUnitList(i).PeriodID, mModelMonitorInspPeriodUnitList(i).Name)
			End If
			i = i + 1
		End While

		Session("mSelectPeriodUnits") = mSelectPeriodUnits
	End Sub

	Public Sub SetGridObjectThreshold()

		Dim txtFrequencyValue As TextBox

		Try

			With mModelMonitorInspThreshold.ModelMonitorInspPeriods

				For i As Integer = 0 To .Count - 1

					REM: Getting the Controls from the DataGrid
					txtFrequencyValue = CType(Me.dgPeriodsThreshold.Rows(i).FindControl("txtFrequencyValueThreshold"), TextBox)
					REM:Setting the Object with the Values of the Controls
					If .Item(i).PeriodID = 2 And Decimal.MaxValue <= Val(txtFrequencyValue.Text.Trim) Then    'Hours 
						.Item(i).FrequencyValue = ""
					Else
						.Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
					End If

				Next i

			End With

			Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetObjectInterval()

		Try

			mModelMonitorInspInterval.Reference = Trim(txtReference.Text)
			mModelMonitorInspInterval.Description = Trim(txtDescription.Text)
			mModelMonitorInspInterval.ModelMonitorInspTypeID = CType(Val(mModelMonitorInspTypeList(Val(cmbType.SelectedValue.ToString), MonitorTypeID:=2).ID), Int32)
			mModelMonitorInspInterval.Note = Trim(txtNote.Text)
			mModelMonitorInspInterval.ATAID = New Guid(cmbATAChapter.SelectedValue)
			mModelMonitorInspThreshold.Zone = Trim(txtZone.Text)
			mModelMonitorInspThreshold.Area = Trim(txtArea.Text)
			mModelMonitorInspThreshold.Reference = Trim(txtReference.Text)

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then
					mModelMonitorInspInterval.IsAttachmentAdded = True
				Else
					mModelMonitorInspInterval.IsAttachmentAdded = False
				End If

			End If

			If AppSettings("SetModelCodeTypeWise") = "True" Then

				If Trim(txtAMPNo.Text).Length < 3 And Trim(txtAMPNo.Text) <> "" Then
					mModelMonitorInspInterval.Code = Trim(txtAMPNo.Text).PadLeft(3, "0"c)
				Else
					mModelMonitorInspInterval.Code = Trim(txtAMPNo.Text)
				End If

			Else
				mModelMonitorInspInterval.Code = Trim(txtAMPNo.Text)
			End If

			Session("mModelMonitorInspInterval") = mModelMonitorInspInterval

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetPeriodUnitsInterval()
		mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits()
		Dim i As Int32
		Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
		'While i <= mPeriodUnitList.Count - 1
		'    If mModelMonitorInspInterval.ModelMonitorInspPeriods.Contains(mPeriodUnitList(i).ID) = False Then
		'        mSelectPeriodUnits.Add(mPeriodUnitList(i).ID, mPeriodUnitList(i).PeriodID, mPeriodUnitList(i).PeriodUnitName)
		'    End If
		'    i = i + 1
		'End While
		While i <= mModelMonitorInspPeriodUnitList.Count - 1
			If mModelMonitorInspInterval.ModelMonitorInspPeriods.Contains(mModelMonitorInspPeriodUnitList(i).ID) = False Then
				mSelectPeriodUnits.Add(mModelMonitorInspPeriodUnitList(i).ID, mModelMonitorInspPeriodUnitList(i).PeriodID, mModelMonitorInspPeriodUnitList(i).Name)
			End If
			i = i + 1
		End While


		Session("mSelectPeriodUnits") = mSelectPeriodUnits
	End Sub

	Public Sub SetGridObjectInterval()

		Dim txtFrequencyValue As TextBox
		Try

			With mModelMonitorInspInterval.ModelMonitorInspPeriods

				For i As Integer = 0 To .Count - 1

					REM: Getting the Controls from the DataGrid
					txtFrequencyValue = CType(Me.dgPeriodsInterval.Rows(i).FindControl("txtFrequencyValueInterval"), TextBox)
					REM:Setting the Object with the Values of the Controls

					If .Item(i).PeriodID = 2 And Decimal.MaxValue <= Val(txtFrequencyValue.Text.Trim) Then    'Hours 
						.Item(i).FrequencyValue = ""
					Else
						.Item(i).FrequencyValue = Trim(txtFrequencyValue.Text)
					End If

				Next i

			End With

			Session("mModelMonitorInspInterval") = mModelMonitorInspInterval

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Sub DataFieldBind()
		mInspTypeList = InspTypeList.GetInspTypeList(False)
		cmbType.DataSource = mInspTypeList
		Session("mInspTypeList") = mInspTypeList

		mModelMonitorInspPeriodUnitList = ModelMonitorInspPeriodUnitList.GetModelMonitorInspPeriodUnitList(mAssemblyStatus.ID)         'mModel.ID)
		Session("mModelMonitorInspPeriodUnitList") = mModelMonitorInspPeriodUnitList

		dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
		dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods

		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods

		mATAList = ATAList.GetATAList(, "(SELECT)")
		cmbATAChapter.DataSource = mATAList
		Session("mATAList") = mATAList

		BindLicenceNoThreshold()
		BindLicenceNoInterval()
		DataBind()


		If mAssemblyMonitorInspStatusThreshold.DoneOn.ToString <> "" Then
			txtDoneOnDateThreshold.Text = CDate(mAssemblyMonitorInspStatusThreshold.DoneOn).ToString(AppSettings("DateFormat"))
		End If
		If mAssemblyMonitorInspStatusInterval.DoneOn.ToString <> "" Then
			txtDoneOnDateInterval.Text = CDate(mAssemblyMonitorInspStatusInterval.DoneOn).ToString(AppSettings("DateFormat"))
		End If

		If txtDoneOnDateThreshold.Text <> "" Then
			phThresholdDoneDetails.Visible = True
		Else
			phThresholdDoneDetails.Visible = False
		End If

		If txtDoneOnDateInterval.Text <> "" Then
			phIntervalDoneDetails.Visible = True
		Else
			phIntervalDoneDetails.Visible = False
		End If

		If Session("FromEditThresholdInterval") = "True" Then
			txtAMPNo.Enabled = False
			btnAddPeriodUnitInterval.Enabled = False
			btnAddPeriodUnitThreshold.Enabled = False
			dgPeriodsThreshold.Columns(2).Visible = False
			dgPeriodsInterval.Columns(2).Visible = False
			If Not mAssemblyMonitorInspStatusThreshold.IsNew Then
				pnlThreshold.Enabled = True
				chkIsThreshold.Checked = True

				If mAssemblyMonitorInspStatusThreshold.DoneOnFormatted.ToString <> "" Then
					phThresholdDoneDetails.Visible = True
					rdbIsComplianceThresholdYes.Checked = True
					upnlIsComplianceThreshold.Update()
				Else
					rdbIsComplianceThresholdNo.Checked = True
				End If

				txtAMPNo.Text = mModelMonitorInspThreshold.Code
				txtReference.Text = mModelMonitorInspThreshold.Reference
				txtNote.Text = mModelMonitorInspThreshold.Note

				If mLinkMaintenanceList Is Nothing Then
					mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorInspThreshold.ID.ToString)
					Session("mLinkMaintenanceList") = mLinkMaintenanceList

				End If

				If mLinkMaintenanceList.Count = 1 Then

					Try
						If mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeApplicable Then
							rdbMakeApplicable.Checked = True
						ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeApplicableAndStart Then
							rdbMakeApplicableAndStart.Checked = True
						ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.MakeNotApplicable Then
							rdbMakeNotApplicable.Checked = True
						ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.Comply Then
							rdbComply.Checked = True
						ElseIf mLinkMaintenanceList(0).MaintenanceActionID = LinkAction.DoNothing Then
							rdbDoNothing.Checked = True
						End If
					Catch ex As SqlException
						If ex.Number = 8145 Then
							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
						ElseIf ex.Number = 2627 Then
							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
						ElseIf ex.Number = 547 Then
							MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
						End If

					End Try
				End If


			Else
				pnlThreshold.Enabled = False
				chkIsThreshold.Checked = False
				rdbIsComplianceThresholdNo.Checked = True
			End If

			If Not mAssemblyMonitorInspStatusInterval.IsNew Then
				pnlInterval.Enabled = True
				chkIsInterval.Checked = True
				If mAssemblyMonitorInspStatusInterval.DoneOnFormatted.ToString <> "" Then
					phIntervalDoneDetails.Visible = True
					rdbIsComplianceIntervalYes.Checked = True
					upnlIsComplianceInterval.Update()
				Else
					rdbIsComplianceIntervalNo.Checked = True
				End If

				If mAssemblyMonitorInspStatusInterval.DoneOnFormatted.ToString = "" Then
					phNAStart.Visible = True
				End If

				txtAMPNo.Text = mModelMonitorInspInterval.Code
				txtReference.Text = mModelMonitorInspInterval.Reference
				txtNote.Text = mModelMonitorInspInterval.Note
			Else
				pnlInterval.Enabled = False
				chkIsInterval.Checked = False
				rdbIsComplianceIntervalNo.Checked = True
			End If

			If Session("MonitorTypeID") = "3" Then
				chkIsApplicable.Checked = False
				txtAMPNo.Text = mModelMonitorInspNA.Code
				txtReference.Text = mModelMonitorInspNA.Reference
				txtNote.Text = mModelMonitorInspNA.Note
			Else
				chkIsApplicable.Checked = True
			End If


			chkIsApplicable.Enabled = False

		Else
			' chkIsApplicable.Checked = False
			' chkIsApplicable.Enabled = True
			phThresholdDoneDetails.Visible = False
			txtAMPNo.Enabled = True
			rdbIsComplianceThresholdNo.Checked = True
			rdbIsComplianceThresholdYes.Checked = False
			rdbIsComplianceIntervalNo.Checked = True
			rdbIsComplianceIntervalYes.Checked = False

		End If
		If chkIsApplicable.Checked Then
			phCompliance.Visible = True
			phLine.Visible = True
		Else
			phCompliance.Visible = False
			phLine.Visible = False
		End If
		If rdbIsComplianceIntervalYes.Checked Then
			phNAStart.Visible = False
		Else
			phNAStart.Visible = True
		End If

	End Sub

	Private Sub AddSelectedPeriodUnitsThreshold(DoneOnDate As String)
		Dim clnModelMonitorInspThreshold = mModelMonitorInspThreshold.Clone
		'Added by Saylee on 10-Feb-2020,  All27072020
		Dim mHourType As Integer = 0
		If mAssemblyStatus.IsSpareAssembly = True Then
			mHourType = mAssemblyStatus.HourType
		Else
			mHourType = mMachine.HourType
		End If
		'*********************
		Try
			If IsNothing(mSelectPeriodUnits) Then
				mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
			End If
			For Each mSelectPeriodUnit As SelectPeriodUnit In mSelectPeriodUnits
				If mSelectPeriodUnit.IsSelected = True Then
					If Not mModelMonitorInspThreshold.ModelMonitorInspPeriods.Contains(mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID) Then
						mModelMonitorInspThreshold.ModelMonitorInspPeriods.Add(mModelMonitorInspThreshold.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
						mModelMonitorInspThreshold.ModelMonitorInspPeriods.CurrentItem.MonitorTypeID = mModelMonitorInspThreshold.MonitorTypeID
						Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod = AssemblyMonitorInspStatusPeriod.NewAssemblyMonitorInspStatusPeriod(mAssemblyMonitorInspStatusThreshold.ID,
																																												 mModelMonitorInspThreshold.ModelMonitorInspPeriods.CurrentItem.ID,
																																												 mAssemblyStatus.ID, mSelectPeriodUnit.PeriodID, mSelectPeriodUnit.PeriodUnitID, 0, DoneOnDate.ToString)
						mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Add(mAssemblyMonitorInspStatusPeriod)

					End If

				End If
			Next
			For i As Integer = 0 To mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count - 1
				mModelMonitorInspThreshold.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInspThreshold.ModelMonitorInspTypeID).MonitorTypeID
				If mModelMonitorInspTypeList(mModelMonitorInspThreshold.ModelMonitorInspTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
					mModelMonitorInspThreshold.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
				End If
			Next
			Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
		Catch ex As Exception
			mModelMonitorInspThreshold = clnModelMonitorInspThreshold
			Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
			If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
				MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
			End If
		Finally
			clnModelMonitorInspThreshold = Nothing

		End Try
	End Sub

	Private Sub AddSelectedPeriodUnitsInterval(DoneOnDate As String)
		Dim clnModelMonitorInspInterval = mModelMonitorInspInterval.Clone
		'Added by Saylee on 10-Feb-2020,  All27072020
		Dim mHourType As Integer = 0
		If mAssemblyStatus.IsSpareAssembly = True Then
			mHourType = mAssemblyStatus.HourType
		Else
			mHourType = mMachine.HourType
		End If
		'*********************
		Try
			If IsNothing(mSelectPeriodUnits) Then
				mSelectPeriodUnits = SelectPeriodUnits.NewSelectPeriodUnits
			End If
			For Each mSelectPeriodUnit As SelectPeriodUnit In mSelectPeriodUnits
				If mSelectPeriodUnit.IsSelected = True Then
					If Not mModelMonitorInspInterval.ModelMonitorInspPeriods.Contains(mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID) Then
						mModelMonitorInspInterval.ModelMonitorInspPeriods.Add(mModelMonitorInspInterval.ID, mSelectPeriodUnit.PeriodUnitID, mSelectPeriodUnit.PeriodID, mHourType)
						mModelMonitorInspInterval.ModelMonitorInspPeriods.CurrentItem.MonitorTypeID = mModelMonitorInspInterval.MonitorTypeID
						Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod = AssemblyMonitorInspStatusPeriod.NewAssemblyMonitorInspStatusPeriod(mAssemblyMonitorInspStatusInterval.ID,
																																											mModelMonitorInspInterval.ModelMonitorInspPeriods.CurrentItem.ID,
																																											mAssemblyStatus.ID, mSelectPeriodUnit.PeriodID, mSelectPeriodUnit.PeriodUnitID, 0, DoneOnDate.ToString)
						mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Add(mAssemblyMonitorInspStatusPeriod)
					End If

				End If
			Next
			For i As Integer = 0 To mModelMonitorInspInterval.ModelMonitorInspPeriods.Count - 1
				mModelMonitorInspInterval.ModelMonitorInspPeriods(i).MonitorTypeID = mModelMonitorInspTypeList(mModelMonitorInspInterval.ModelMonitorInspTypeID).MonitorTypeID
				If mModelMonitorInspTypeList(mModelMonitorInspInterval.ModelMonitorInspTypeID).MonitorTypeID = 3 Then        'this is for No Frequency
					mModelMonitorInspInterval.ModelMonitorInspPeriods(i).FrequencyValue = CStr(0)
				End If
			Next
			Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
			Session.Remove("mSelectPeriodUnits")
			mSelectPeriodUnits = Nothing
		Catch ex As Exception
			mModelMonitorInspInterval = clnModelMonitorInspInterval
			Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
			If InStr("Period Unit already present. Can not be added.", ex.Message, CompareMethod.Text) Then
				MSGBoxCtrl.Show("Alert!", "Similar Interval Unit can not be added together.</br>e.g. (Days/Mts/Year)and(Hrs/Hobbs)", "Select either of the Interval Unit and continue. ", MsgBoxStyle.OkOnly, "")
			End If
		Finally
			clnModelMonitorInspInterval = Nothing
			Session.Remove("mSelectPeriodUnits")
			mSelectPeriodUnits = Nothing
		End Try
	End Sub

	Public Sub AddPeriodUnitsInterval()
		If mModelMonitorInspInterval.ModelMonitorInspPeriods.Count > 0 And chkIsInterval.Checked Then
			For i As Integer = 0 To mModelMonitorInspInterval.ModelMonitorInspPeriods.Count - 1
				Dim PeriodID As Integer = mModelMonitorInspInterval.ModelMonitorInspPeriods(i).PeriodID
				Dim PeriodUnitID As Integer = mModelMonitorInspInterval.ModelMonitorInspPeriods(i).PeriodUnitID

				If Not mModelMonitorInspThreshold.ModelMonitorInspPeriods.Contains(PeriodUnitID, PeriodID) Then
					mModelMonitorInspThreshold.ModelMonitorInspPeriods.Add(mModelMonitorInspThreshold.ID, PeriodUnitID, PeriodID, 1)
					mModelMonitorInspThreshold.ModelMonitorInspPeriods.CurrentItem.MonitorTypeID = mModelMonitorInspThreshold.MonitorTypeID
					Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod = AssemblyMonitorInspStatusPeriod.NewAssemblyMonitorInspStatusPeriod(mAssemblyMonitorInspStatusThreshold.ID,
																																											  mModelMonitorInspThreshold.ModelMonitorInspPeriods.CurrentItem.ID,
																																											  mAssemblyStatus.ID, PeriodID, PeriodUnitID, 0, Today.Date.ToString)
					mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Add(mAssemblyMonitorInspStatusPeriod)
				End If

			Next

			Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
			dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
			dgPeriodsThreshold.DataBind()
			upnlPeriodsThreshold.Update()
		End If



		If mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count > 0 And chkIsThreshold.Checked Then
			For i As Integer = 0 To mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count - 1
				Dim PeriodID As Integer = mModelMonitorInspThreshold.ModelMonitorInspPeriods(i).PeriodID
				Dim PeriodUnitID As Integer = mModelMonitorInspThreshold.ModelMonitorInspPeriods(i).PeriodUnitID

				If Not mModelMonitorInspInterval.ModelMonitorInspPeriods.Contains(PeriodUnitID, PeriodID) Then
					mModelMonitorInspInterval.ModelMonitorInspPeriods.Add(mModelMonitorInspInterval.ID, PeriodUnitID, PeriodID, 1)
					mModelMonitorInspInterval.ModelMonitorInspPeriods.CurrentItem.MonitorTypeID = mModelMonitorInspInterval.MonitorTypeID
					Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod = AssemblyMonitorInspStatusPeriod.NewAssemblyMonitorInspStatusPeriod(mAssemblyMonitorInspStatusInterval.ID,
																																													mModelMonitorInspInterval.ModelMonitorInspPeriods.CurrentItem.ID,
																																													mAssemblyStatus.ID, PeriodID, PeriodUnitID, 0, Today.Date.ToString)
					mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Add(mAssemblyMonitorInspStatusPeriod)
				End If
			Next

			Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
			dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods
			dgPeriodsInterval.DataBind()
			upnlPeriodsInterval.Update()
		End If

	End Sub

	Public Sub SetLicenceCountThreshold()
		If mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count > 1 Then
			lblLicenceCount.Text = "and " + (mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
		End If
		lblLicenceCount.DataBind()
		'lblAllLicenceNos.DataBind()
	End Sub

	Private Sub BindLicenceNoThreshold()
		If mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
			txtLicenceNoThreshold.Text = mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeName + "]"
		Else
			txtLicenceNoThreshold.Text = String.Empty
		End If
	End Sub

	Public Sub SetLicenceCountInterval()
		If mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count > 1 Then
			lblLicenceCount.Text = "and " + (mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
		End If
		lblLicenceCount.DataBind()
		'lblAllLicenceNos.DataBind()
	End Sub

	Private Sub BindLicenceNoInterval()
		If mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
			txtLicenceNoInterval.Text = mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).EmployeeName + "]"
		Else
			txtLicenceNoInterval.Text = String.Empty
		End If
	End Sub

	Public Function CustomValidate3() As Boolean

		If chkIsApplicable.Checked = True Then Exit Function

		Dim str As String = ""
		SetObjectNA()

		If Not mModelMonitorInspNA.IsValid Then
			For i As Integer = 0 To mModelMonitorInspNA.GetBrokenRulesCollection.Count - 1
				str = str + "NA Activity : " + mModelMonitorInspNA.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
		End If

		For counter As Integer = 0 To mModelMonitorInspNA.ModelMonitorInspPeriods.Count - 1
			If Not mModelMonitorInspNA.ModelMonitorInspPeriods(counter).IsValid Then
				For i As Integer = 0 To mModelMonitorInspNA.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection.Count - 1
					str = str + "NA Activity : " + mModelMonitorInspNA.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
				Next
			End If
		Next

		If Not mAssemblyMonitorInspStatusNA.IsValid Then
			For i As Integer = 0 To mAssemblyMonitorInspStatusNA.GetBrokenRulesCollection.Count - 1
				str = str + "NA Activity : " + mAssemblyMonitorInspStatusNA.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
		End If
		For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Count - 1)
			If Not mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods(i).IsValid Then
				For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
					str = str + "NA Activity : " + mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
				Next
			End If
		Next
		If str <> "" Then
			cvATAChapter.ErrorMessage = str
			cvATAChapter.IsValid = False
			Return False
		Else
			cvATAChapter.IsValid = True
			Return True
		End If
	End Function

	Public Function CustomValidate2() As Boolean

		Dim str As String = ""
		Try

			SetObjectThreshold()
			SetGridObjectThreshold()

			SetThresholdStatusObject()
			SetGridThresholdStatusObject()

			SetObjectInterval()
			SetGridObjectInterval()

			SetIntervalStatusObject()
			SetGridIntervalStatusObject()

			If chkIsThreshold.Checked Then

				If Not mModelMonitorInspThreshold.IsValid Then

					For i As Integer = 0 To mModelMonitorInspThreshold.GetBrokenRulesCollection.Count - 1
						str = str + "Threshold Activity : " + mModelMonitorInspThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
					Next

				End If

				For counter As Integer = 0 To dgPeriodsThreshold.Rows.Count - 1

					If Not mModelMonitorInspThreshold.ModelMonitorInspPeriods(counter).IsValid Then

						For i As Integer = 0 To mModelMonitorInspThreshold.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection.Count - 1
							str = str + "Threshold Activity : " + mModelMonitorInspThreshold.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
						Next

					End If

				Next

				If Not mAssemblyMonitorInspStatusThreshold.IsValid Then

					For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.GetBrokenRulesCollection.Count - 1
						str = str + "Threshold Activity : " + mAssemblyMonitorInspStatusThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
					Next

				End If

				For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1)

					If Not mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).IsValid Then

						For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
							str = str + "Threshold Activity : " + mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
						Next

					End If

				Next

			End If

			If chkIsInterval.Checked Then

				If Not mModelMonitorInspInterval.IsValid Then

					For i As Integer = 0 To mModelMonitorInspInterval.GetBrokenRulesCollection.Count - 1
						str = str + "Interval Activity : " + mModelMonitorInspInterval.GetBrokenRulesCollection(i).Description + "<BR>"
					Next

				End If

				For counter As Integer = 0 To dgPeriodsInterval.Rows.Count - 1

					If Not mModelMonitorInspInterval.ModelMonitorInspPeriods(counter).IsValid Then

						For i As Integer = 0 To mModelMonitorInspInterval.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection.Count - 1
							str = str + "Interval Activity : " + mModelMonitorInspInterval.ModelMonitorInspPeriods(counter).GetBrokenRulesCollection(i).Description + "<BR>"
						Next

					End If

				Next

				If Not mAssemblyMonitorInspStatusInterval.IsValid Then

					For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.GetBrokenRulesCollection.Count - 1
						str = str + "Interval Activity : " + mAssemblyMonitorInspStatusInterval.GetBrokenRulesCollection(i).Description + "<BR>"
					Next

				End If

				For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1)

					If Not mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods(i).IsValid Then

						For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
							str = str + "Interval Activity : " + mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
						Next

					End If

				Next

			End If

			If str <> "" Then

				cvATAChapter.ErrorMessage = str
				cvATAChapter.IsValid = False

				Return False
			Else

				cvATAChapter.IsValid = True
				Return True

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Public Sub NewThreshold()

		Dim ID As Guid = Guid.NewGuid

		mModelMonitorInspThreshold = ModelMonitorInsp.NewModelMonitorInsp(ID,
																		  mModelMonitorInsp.ModelID,
																		  mMachine.HourType,
																		  ID) 'For new records ID,PrevRefID are same

		mModelMonitorInspThreshold.Description = mModelMonitorInsp.Description
		mModelMonitorInspThreshold.ATAID = mModelMonitorInsp.ATAID
		mModelMonitorInspThreshold.Code = mModelMonitorInsp.Code
		mModelMonitorInspThreshold.WatchItemID = mModelMonitorInsp.WatchItemID
		Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
		mModelMonitorInsp.BeginEdit()

		mAssemblyMonitorInspStatusThreshold =
			AssemblyMonitorInspStatus.
				NewAssemblyMonitorInspStatus(Guid.NewGuid,
											 mAssemblyMonitorInspStatus.AssemblyID,
											 mAssemblyMonitorInspStatus.AssemblyStatusID,
											 mAssemblyMonitorInspStatus.AsOnDate.ToString,
											 mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID,
											 mMachine.HourType)

		If Session("OpenFromDiscrepancyCorrectiveActionList") IsNot Nothing AndAlso
		   Session("OpenFromDiscrepancyCorrectiveActionList").ToString.ToLower = "true" Then

			mAssemblyMonitorInspStatusThreshold.
				LogID(Session("RectifiedLogID").ToString,
					  Session("mIssueDate").ToString,
					  True,
					  CType(Session("mModelMonitorInspThreshold"),
					  ModelMonitorInsp)) = New Guid(Session("RectifiedLogID").ToString)

		End If

		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold

	End Sub

	Public Function SaveThreshold() As Boolean

		Try

			If Not chkIsThreshold.Checked Then Return False

			SetObjectThreshold()
			SetGridObjectThreshold()

			Dim mModelMonitorInspThresholdClone As ModelMonitorInsp
			mModelMonitorInspThresholdClone = CType(mModelMonitorInspThreshold, ModelMonitorInsp)

			If mModelMonitorInspThreshold.IsValid = True Then

				Try

					Dim ServiceMPDTitle As String = ""

					If AppSettings("ShowMaintenanceForNewClients") = "True" Then
						ServiceMPDTitle = "AMP"
					Else
						ServiceMPDTitle = "Model Service"
					End If

					If mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count = 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.PeriodRequired,
										MSGBox.Message_Text.PeriodRequired,
										ServiceMPDTitle + " cannot be saved without Period units.",
										MsgBoxStyle.OkOnly,
										"")

						Return False

					End If

					mModelMonitorInspThreshold.ApplyEdit()
					mModelMonitorInspThreshold = CType(mModelMonitorInspThreshold.Save, ModelMonitorInsp)

					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
					Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
					mModel = mModelMonitorInspThreshold.Model.Name
					mMonitorType = mModelMonitorInspThreshold.ModelMonitorInspTypeName
					mDescription = txtDescription.Text
					mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

					MarkLog(Action:=Action.Save,
							ModuleName:="Model Service",
							Detail:=mDetail,
							ErrorType:=ErrorType.NoError,
							TransID:=mModelMonitorInspThreshold.ID,
							EventLogID)

					'End

					If SaveThresholdStatus() Then
						Return True
					End If

					Return False

				Catch ex As SqlException

					If ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										"This Entry is used by Some One.",
										MsgBoxStyle.OkOnly,
										"")

					End If

					mModelMonitorInspThreshold = mModelMonitorInspThresholdClone
					Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold

					Return False

				End Try

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Sub SetGridThresholdStatusObject()

		Try

			mAssemblyMonitorInspStatusThreshold = Session("mAssemblyMonitorInspStatusThreshold")

			Dim calDoneOn, txtDueOnValue, txtExtensionValue As TextBox

			For j As Integer = 0 To Me.dgThresholdValues.Rows.Count - 1

				calDoneOn = CType(Me.dgThresholdValues.Rows(j).FindControl("txtDoneOnValueThreshold"), TextBox)
				txtDueOnValue = CType(Me.dgThresholdValues.Rows(j).FindControl("txtDueOnValueThreshold"), TextBox)
				txtExtensionValue = CType(Me.dgThresholdValues.Rows(j).FindControl("txtExtensionValueThreshold"), TextBox) 'Added By Saylee on 22-07-2008

				With mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods

					If .Item(j).PeriodID = 2 Then
						If Not Period.IsDate(calDoneOn.Text.Trim) Then
							.Item(j).DoneOnValue = ""
						Else
							.Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
						End If
					Else
						.Item(j).DoneOnValue = Trim(calDoneOn.Text)
					End If

					.Item(j).ExtensionValue = Trim(txtExtensionValue.Text)

				End With

			Next j

			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetThresholdStatusObject()

		mAssemblyMonitorInspStatusThreshold = Session("mAssemblyMonitorInspStatusThreshold")
		Try

			With mAssemblyMonitorInspStatusThreshold

				If Not mModelMonitorInspThreshold.IsNew And mAssemblyMonitorInspStatusThreshold.IsNew Then

					.ModelMonitorInspID(True) = mModelMonitorInspThreshold.ID

					dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
					dgThresholdValues.DataBind()
					upnlThresholdValues.Update()

				End If

				.ModelMonitorInsp.Reference = mModelMonitorInspThreshold.Reference
				.ModelMonitorInsp.Description = mModelMonitorInspThreshold.Description
				.ModelMonitorInsp.RequiredManHours = mModelMonitorInspThreshold.RequiredManHours

				If txtDoneOnDateThreshold.Text = "" Then
					.DoneOn = System.DBNull.Value
				Else
					.DoneOn = txtDoneOnDateThreshold.Text
				End If

				If chkIsThreshold.Checked Then
					.IsApplicable = True
				Else
					.IsApplicable = False
				End If

				If txtDoneOnDateInterval.Text <> "" And rdbIsComplianceIntervalYes.Checked Then
					.IsApplicable = False
				Else
					.IsApplicable = True
				End If

				.DoneWONo = Trim(txtWorkOrNoThreshold.Text)
				.DoneRemark = Trim(txtRemarkThreshold.Text)
				.RequiredManHours = Trim(txtRequiredManHoursThreshold.Text)
				.Place = Trim(txtPlaceThreshold.Text)

				Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
				Dim EmpName As String = String.Empty

				If (txtLicenceNoThreshold.Text.Trim.IndexOf("[") > 0 And txtLicenceNoThreshold.Text.Trim.IndexOf("]") > 0) Then
					LicenseNo = txtLicenceNoThreshold.Text.Substring(0, txtLicenceNoThreshold.Text.Trim.IndexOf("[")).Trim
					EmpName = Mid(txtLicenceNoThreshold.Text.Trim, txtLicenceNoThreshold.Text.Trim.IndexOf("[") + 2, txtLicenceNoThreshold.Text.Trim.IndexOf("]") - txtLicenceNoThreshold.Text.Trim.IndexOf("[") - 1).Trim
				Else
					LicenseNo = Trim(txtLicenceNoThreshold.Text)
				End If

				.LicenseNo = LicenseNo
				.DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

			End With

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Function SaveThresholdStatus() As Boolean
		' If Not rdbIsComplianceThresholdYes.Checked Then Return False

		SetThresholdStatusObject()
		SetGridThresholdStatusObject()

		If mAssemblyMonitorInspStatusThreshold.IsValid Then
			mAssemblyMonitorInspStatusThreshold = Session("mAssemblyMonitorInspStatusThreshold")
			mAssemblyMonitorInspStatusThreshold.ApplyEdit()
			mAssemblyMonitorInspStatusThreshold = CType(mAssemblyMonitorInspStatusThreshold.Save(), AssemblyMonitorInspStatus)

			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold

			Return True
		Else
			Dim str As String = ""
			For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.GetBrokenRulesCollection.Count - 1
				str = str + "Threshold Activity : " + mAssemblyMonitorInspStatusThreshold.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
			For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1)
				If Not mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
					For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
						str = str + "Threshold Activity : " + mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next
				End If
			Next
			If str <> "" Then
				cvATAChapter.ErrorMessage = str
				cvATAChapter.IsValid = False
				Return False
			Else
				cvATAChapter.IsValid = True
				'  Return True
			End If
		End If

	End Function

	Public Sub NewInterval()
		Dim ID As Guid = Guid.NewGuid
		mModelMonitorInspInterval = ModelMonitorInsp.NewModelMonitorInsp(ID, mModelMonitorInsp.ModelID, mMachine.HourType, ID) 'For new records ID,PrevRefID are same
		mModelMonitorInspInterval.Description = mModelMonitorInsp.Description
		mModelMonitorInspInterval.ATAID = mModelMonitorInsp.ATAID
		mModelMonitorInspInterval.Code = mModelMonitorInsp.Code
		mModelMonitorInspInterval.WatchItemID = mModelMonitorInsp.WatchItemID
		Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
		mModelMonitorInsp.BeginEdit()


		mAssemblyMonitorInspStatusInterval = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyMonitorInspStatus.AssemblyID, mAssemblyMonitorInspStatus.AssemblyStatusID, mAssemblyMonitorInspStatus.AsOnDate.ToString, mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, mMachine.HourType)

		If Session("OpenFromDiscrepancyCorrectiveActionList") IsNot Nothing AndAlso
		   Session("OpenFromDiscrepancyCorrectiveActionList").ToString.ToLower = "true" Then

			mAssemblyMonitorInspStatusInterval.LogID(Session("RectifiedLogID").ToString, Session("mIssueDate").ToString, True, CType(Session("mModelMonitorInspInterval"), ModelMonitorInsp)) = New Guid(Session("RectifiedLogID").ToString)

		End If

		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
	End Sub

	Public Function SaveInterval() As Boolean

		Try

			If Not chkIsInterval.Checked Then Return False

			SetObjectInterval()
			SetGridObjectInterval()

			Dim mModelMonitorInspIntervalClone As ModelMonitorInsp
			mModelMonitorInspIntervalClone = CType(mModelMonitorInspInterval, ModelMonitorInsp)

			If mModelMonitorInspInterval.IsValid = True Then

				Try

					Dim ServiceMPDTitle As String = ""

					If AppSettings("ShowMaintenanceForNewClients") = "True" Then
						ServiceMPDTitle = "AMP"
					Else
						ServiceMPDTitle = "Model Service"
					End If

					If mModelMonitorInspInterval.ModelMonitorInspPeriods.Count = 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.PeriodRequired,
										MSGBox.Message_Text.PeriodRequired,
										ServiceMPDTitle + " cannot be saved without Period units",
										MsgBoxStyle.OkOnly,
										"")

						Return False

					End If

					mModelMonitorInspInterval.ApplyEdit()
					mModelMonitorInspInterval = CType(mModelMonitorInspInterval.Save, ModelMonitorInsp)

					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
					Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
					mModel = mModelMonitorInspInterval.Model.Name
					mMonitorType = mModelMonitorInspInterval.ModelMonitorInspTypeName
					mDescription = txtDescription.Text
					mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

					MarkLog(Action:=Action.Save,
							ModuleName:="Model Service",
							Detail:=mDetail,
							ErrorType:=ErrorType.NoError,
							TransID:=mModelMonitorInspInterval.ID,
							EventLogID)

					'End


					If SaveIntervalStatus() Then
						Return True
					End If



					Return False

				Catch ex As SqlException

					If ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										"This Entry is used by Some One.",
										MsgBoxStyle.OkOnly,
										"")

					End If

					mModelMonitorInspInterval = mModelMonitorInspIntervalClone
					Session("mModelMonitorInspInterval") = mModelMonitorInspInterval

					Return False

				End Try

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Sub SetGridIntervalStatusObject()

		Try

			mAssemblyMonitorInspStatusInterval = Session("mAssemblyMonitorInspStatusInterval")

			Dim calDoneOn, txtDueOnValue, txtExtensionValue As TextBox

			For j As Integer = 0 To Me.dgIntervalValues.Rows.Count - 1

				calDoneOn = CType(Me.dgIntervalValues.Rows(j).FindControl("txtDoneOnValueInterval"), TextBox)
				txtDueOnValue = CType(Me.dgIntervalValues.Rows(j).FindControl("txtDueOnValueInterval"), TextBox)
				txtExtensionValue = CType(Me.dgIntervalValues.Rows(j).FindControl("txtExtensionValueInterval"), TextBox) 'Added By Saylee on 22-07-2008

				With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods

					If .Item(j).PeriodID = 2 Then
						If Not Period.IsDate(calDoneOn.Text.Trim) Then
							.Item(j).DoneOnValue = ""
						Else
							.Item(j).DoneOnValueFormatted = Trim(calDoneOn.Text)
						End If
					Else
						.Item(j).DoneOnValue = Trim(calDoneOn.Text)
					End If

					.Item(j).ExtensionValue = Trim(txtExtensionValue.Text)

				End With

			Next j

			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetIntervalStatusObject()

		Try

			mAssemblyMonitorInspStatusInterval = Session("mAssemblyMonitorInspStatusInterval")

			With mAssemblyMonitorInspStatusInterval

				If Not mModelMonitorInspInterval.IsNew And mAssemblyMonitorInspStatusInterval.IsNew Then

					.ModelMonitorInspID(True) = mModelMonitorInspInterval.ID

					dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
					dgIntervalValues.DataBind()
					upnlIntervalValues.Update()

				End If

				.ModelMonitorInsp.Reference = mModelMonitorInspInterval.Reference
				.ModelMonitorInsp.Description = mModelMonitorInspInterval.Description
				.ModelMonitorInsp.RequiredManHours = mModelMonitorInspInterval.RequiredManHours

				If txtDoneOnDateInterval.Text = "" Then
					.DoneOn = System.DBNull.Value
				Else
					.DoneOn = txtDoneOnDateInterval.Text
				End If

				If chkIsInterval.Checked Then
					.IsApplicable = True
				Else
					.IsApplicable = False
				End If

				.DoneWONo = Trim(txtWorkOrNoInterval.Text)
				.DoneRemark = Trim(txtRemarkInterval.Text)
				.RequiredManHours = Trim(txtRequiredManHoursInterval.Text)
				.Place = Trim(txtPlaceInterval.Text)

				Dim LicenseNo As String = String.Empty 'Added By Prashant On 12-Jun-2012 FOR ALL08062012
				Dim EmpName As String = String.Empty

				If (txtLicenceNoInterval.Text.Trim.IndexOf("[") > 0 And txtLicenceNoInterval.Text.Trim.IndexOf("]") > 0) Then
					LicenseNo = txtLicenceNoInterval.Text.Substring(0, txtLicenceNoInterval.Text.Trim.IndexOf("[")).Trim
					EmpName = Mid(txtLicenceNoInterval.Text.Trim, txtLicenceNoInterval.Text.Trim.IndexOf("[") + 2, txtLicenceNoInterval.Text.Trim.IndexOf("]") - txtLicenceNoInterval.Text.Trim.IndexOf("[") - 1).Trim
				Else
					LicenseNo = Trim(txtLicenceNoInterval.Text)
				End If

				.LicenseNo = LicenseNo
				.DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

			End With

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Function SaveIntervalStatus() As Boolean
		'If Not rdbIsComplianceIntervalYes.Checked Then Return False

		SetIntervalStatusObject()
		SetGridIntervalStatusObject()



		'Linking Activity
		If rdbIsComplianceIntervalNo.Checked And rdbIsComplianceThresholdYes.Checked And txtDoneOnDateThreshold.Text <> "" Then
			If rdbMakeApplicable.Checked Then
				'' ActionID = LinkAction.MakeApplicable
				mAssemblyMonitorInspStatusInterval.IsApplicable = True
			ElseIf rdbMakeApplicableAndStart.Checked Then
				'MakeApplicableAndStart
				mAssemblyMonitorInspStatusInterval.IsApplicable = True

				'Setting Currrent Values to Done On Values...as default
				For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
					With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
						If .Item(i).PeriodID = 2 Then
							If Not Period.IsDate(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).CurrentValueFormatted) Then
								.Item(i).DoneOnValue = ""
							Else
								.Item(i).DoneOnValueFormatted = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).CurrentValueFormatted
							End If
						Else
							.Item(i).DoneOnValue = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(i).CurrentValue
						End If

						''ExtensionValue
						'.Item(i).ExtensionValue = PeriodValues(i)
					End With
				Next
			ElseIf rdbMakeNotApplicable.Checked Then
				'ActionID = LinkAction.MakeNotApplicable
				mAssemblyMonitorInspStatusInterval.IsApplicable = False
			ElseIf rdbComply.Checked Then
				'' ActionID = LinkAction.Comply
				mAssemblyMonitorInspStatusInterval.IsApplicable = True
				mAssemblyMonitorInspStatusInterval.DoneOn = mAssemblyMonitorInspStatusThreshold.DoneOnFormatted

				mAssemblyMonitorInspStatusInterval.DoneRemark = mAssemblyMonitorInspStatusThreshold.DoneRemark 'mMultiCompliance.DoneRemark
				mAssemblyMonitorInspStatusInterval.DoneWONo = mAssemblyMonitorInspStatusThreshold.DoneWONo

				mAssemblyMonitorInspStatusInterval.Place = mAssemblyMonitorInspStatusThreshold.Place
				mAssemblyMonitorInspStatusInterval.LicenseNo = mAssemblyMonitorInspStatusThreshold.AllLicenceNos
				mAssemblyMonitorInspStatusInterval.DoneByID = mAssemblyMonitorInspStatusThreshold.DoneByID


				For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1  'Number of rows in 2 -dim array.Zero Based
					For j As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
						With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
							If (mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).PeriodUnitID = (.Item(j).PeriodUnitID)) And (mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).PeriodID = (.Item(j).PeriodID)) Then

								If .Item(j).PeriodID = 2 Then
									If Not Period.IsDate(mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).CurrentValueFormatted) Then
										.Item(j).CurrentValue = ""
									Else
										.Item(j).CurrentValueFormatted = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).CurrentValueFormatted
									End If
								Else
									.Item(j).CurrentValue = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods(i).CurrentValue
								End If
							End If
						End With
					Next
				Next
			ElseIf rdbDoNothing.Checked Then
				'' ActionID = LinkAction.DoNothing

			End If

		End If
		If mAssemblyMonitorInspStatusInterval.IsValid Then

			mAssemblyMonitorInspStatusInterval.ApplyEdit()
			mAssemblyMonitorInspStatusInterval = CType(mAssemblyMonitorInspStatusInterval.Save(), AssemblyMonitorInspStatus)

			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval

			Return True
		Else
			Dim str As String = ""
			For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.GetBrokenRulesCollection.Count - 1
				str = str + "Interval Activity : " + mAssemblyMonitorInspStatusInterval.GetBrokenRulesCollection(i).Description + "<BR>"
			Next
			For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1)
				If Not mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
					For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
						str = str + "Interval Activity : " + mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next
				End If
			Next
			If str <> "" Then
				cvATAChapter.ErrorMessage = str
				cvATAChapter.IsValid = False
				Return False
			Else
				cvATAChapter.IsValid = True
				'  Return True
			End If
		End If
	End Function

	Private Sub SetColorThreshold()
		If mAssemblyMonitorInspStatusThreshold IsNot Nothing Then
			If mAssemblyMonitorInspStatusThreshold.ModelMonitorInsp.MonitorTypeID = 1 And mAssemblyMonitorInspStatusThreshold.DoneOn IsNot System.DBNull.Value Then
				Dim txtdueOnValue As TextBox
				For i As Integer = 0 To dgThresholdValues.Rows.Count - 1
					txtdueOnValue = CType(dgThresholdValues.Rows(i).FindControl("txtDueOnValueThreshold"), TextBox)
					txtdueOnValue.BackColor = System.Drawing.Color.Red
					txtdueOnValue.ForeColor = System.Drawing.Color.White
				Next
				lblRed.Visible = True
				lblInfo.Visible = True
			Else
				lblRed.Visible = False
				lblInfo.Visible = False
			End If
		End If
	End Sub 'End

	Private Sub SetObjectNA()

		Try

			If Session("FromEditThresholdInterval") = "True" Then
				'do nothing
			Else
				Dim mID As Guid = Guid.NewGuid
				mModelMonitorInspNA = ModelMonitorInsp.NewModelMonitorInsp(mID, mAssemblyStatus.Assembly.ModelID, mMachine.HourType, mID)
				mAssemblyMonitorInspStatusNA = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyStatus.AssemblyID, mAssemblyStatus.ID, Today.Date.ToString, mAssemblyStatus.Assembly.ModelID, mMachine.HourType)
			End If

			If mModelMonitorInspTypeList.Contains(Val(cmbType.SelectedValue), 3) And mModelMonitorInspNA.IsNew Then 'N/A type
				mModelMonitorInspNA.ModelMonitorInspTypeID = CType(Val(mModelMonitorInspTypeList(Val(cmbType.SelectedValue.ToString), MonitorTypeID:=3).ID), Int32)
			End If

			mModelMonitorInspNA.Reference = Trim(txtReference.Text)
			mModelMonitorInspNA.Description = Trim(txtDescription.Text)
			mModelMonitorInspNA.Note = Trim(txtNote.Text)

			If mFileAttach IsNot Nothing Then

				If mFileAttach.Size > 0 Then
					mModelMonitorInspNA.IsAttachmentAdded = True
				Else
					mModelMonitorInspNA.IsAttachmentAdded = False
				End If

			End If

			If AppSettings("SetModelCodeTypeWise") = "True" Then

				If Trim(txtAMPNo.Text).Length < 3 And Trim(txtAMPNo.Text) <> "" Then
					mModelMonitorInspNA.Code = Trim(txtAMPNo.Text).PadLeft(3, "0"c)
				Else
					mModelMonitorInspNA.Code = Trim(txtAMPNo.Text)
				End If

			Else
				mModelMonitorInspNA.Code = Trim(txtAMPNo.Text)
			End If

			''********************
			If mModelMonitorInspNA.IsNew Then mModelMonitorInspNA.ModelMonitorInspPeriods.Add(mModelMonitorInspNA.ID, 1, 1, mMachine.HourType)

			mModelMonitorInspNA.ModelMonitorInspPeriods.CurrentItem.MonitorTypeID = mModelMonitorInspNA.MonitorTypeID
			mModelMonitorInspNA.ModelMonitorInspPeriods.CurrentItem.FrequencyValue = "0"
			Session("mModelMonitorInspNA") = mModelMonitorInspNA


			'Status
			Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod

			If mAssemblyMonitorInspStatusNA.IsNew Then

				mAssemblyMonitorInspStatusPeriod =
					AssemblyMonitorInspStatusPeriod.NewAssemblyMonitorInspStatusPeriod(mAssemblyMonitorInspStatusNA.ID,
																							 mModelMonitorInspNA.ModelMonitorInspPeriods.CurrentItem.ID,
																							 mAssemblyStatus.ID, 1, 1, 0, Today.Date.ToString)
				mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Add(mAssemblyMonitorInspStatusPeriod)

			End If

			With mAssemblyMonitorInspStatusNA

				If Not mModelMonitorInspNA.IsNew And mAssemblyMonitorInspStatusNA.IsNew Then
					.ModelMonitorInspID(True) = mModelMonitorInspNA.ID
				End If

				.ModelMonitorInsp.Reference = mModelMonitorInspNA.Reference
				.ModelMonitorInsp.Description = mModelMonitorInspNA.Description
				.ModelMonitorInsp.RequiredManHours = mModelMonitorInspNA.RequiredManHours

			End With

			Session("mAssemblyMonitorInspStatusNA") = mAssemblyMonitorInspStatusNA

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Function SaveNARecord() As Boolean

		Try

			SetObjectNA()

			Dim mModelMonitorInspNAClone As ModelMonitorInsp
			mModelMonitorInspNAClone = CType(mModelMonitorInspNA, ModelMonitorInsp)

			If mModelMonitorInspNA.IsValid = True Then

				Try

					Dim ServiceMPDTitle As String = ""

					If AppSettings("ShowMaintenanceForNewClients") = "True" Then
						ServiceMPDTitle = "AMP"
					Else
						ServiceMPDTitle = "Model Service"
					End If

					If mModelMonitorInspNA.ModelMonitorInspPeriods.Count = 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.PeriodRequired,
										MSGBox.Message_Text.PeriodRequired,
										ServiceMPDTitle + " cannot be saved without Period units",
										MsgBoxStyle.OkOnly,
										"")

						Return False

					End If

					mModelMonitorInspNA.ApplyEdit()
					mModelMonitorInspNA = CType(mModelMonitorInspNA.Save, ModelMonitorInsp)

					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
					Session("mModelMonitorInspNA") = mModelMonitorInspNA
					mModel = mModelMonitorInspNA.Model.Name
					mMonitorType = mModelMonitorInspNA.ModelMonitorInspTypeName
					mDescription = txtDescription.Text
					mDetail = "Model : " + mModel + " Monitor Type : " + mMonitorType + " Description : " + mDescription

					MarkLog(Action:=Action.Save,
							ModuleName:="Model Service",
							Detail:=mDetail,
							ErrorType:=ErrorType.NoError,
							TransID:=mModelMonitorInspNA.ID,
							EventLogID)

					'End


					'Status
					mAssemblyMonitorInspStatusNA.IsApplicable = False
					mAssemblyMonitorInspStatusNA.ModelMonitorInspID(True) = mModelMonitorInspNA.ID

					If mAssemblyMonitorInspStatusNA.IsValid Then

						mAssemblyMonitorInspStatusNA = Session("mAssemblyMonitorInspStatusNA")
						mAssemblyMonitorInspStatusNA.ApplyEdit()
						mAssemblyMonitorInspStatusNA = CType(mAssemblyMonitorInspStatusNA.Save(), AssemblyMonitorInspStatus)

						Session("mAssemblyMonitorInspStatusNA") = mAssemblyMonitorInspStatusNA

						Return True

					Else

						Dim str As String = ""

						For i As Integer = 0 To mAssemblyMonitorInspStatusNA.GetBrokenRulesCollection.Count - 1
							str = str + "NA Activity : " + mAssemblyMonitorInspStatusNA.GetBrokenRulesCollection(i).Description + "<BR>"
						Next

						For i As Integer = 0 To CShort(mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Count - 1)

							If Not mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then

								For x As Int16 = 0 To CShort(mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1)
									str = str + "NA Activity : " + mAssemblyMonitorInspStatusNA.AssemblyMonitorInspStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
								Next

							End If

						Next

						If str <> "" Then
							cvATAChapter.ErrorMessage = str
							cvATAChapter.IsValid = False
							Return False
						Else
							cvATAChapter.IsValid = True
						End If

					End If

					Return False

				Catch ex As SqlException

					If ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										"This Entry is used by Some One.",
										MsgBoxStyle.OkOnly,
										"")

					End If

					mModelMonitorInspNA = mModelMonitorInspNAClone
					Session("mModelMonitorInspNA") = mModelMonitorInspNA

					Return False

				End Try

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Function Save() As Boolean

		Dim mIsThresholdSaved As Boolean = False
		Dim mIsIntervalSaved As Boolean = False
		Dim mIsNARecordSaved As Boolean = False

		Try

			If Not IsValid Then Exit Function

			If chkIsApplicable.Checked Then

				If SaveThreshold() Then
					mIsThresholdSaved = True
				Else
					mIsThresholdSaved = False
				End If

				If SaveInterval() Then
					mIsIntervalSaved = True
				Else
					mIsIntervalSaved = False
				End If

				If mIsThresholdSaved = True Or mIsIntervalSaved = True Then

					'Linking
					If rdbIsComplianceIntervalYes.Checked = False And chkIsInterval.Checked = True Then SaveLinkActivity()

					Return True

				Else
					Return False
				End If

			Else

				If SaveNARecord() Then
					Return True
				Else
					Return False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Public Sub SaveLinkActivity()

		Dim ActionID As Integer = 0

		Try

			If rdbMakeApplicable.Checked Then
				ActionID = LinkAction.MakeApplicable
			ElseIf rdbMakeApplicableAndStart.Checked Then
				ActionID = LinkAction.MakeApplicableAndStart
			ElseIf rdbMakeNotApplicable.Checked Then
				ActionID = LinkAction.MakeNotApplicable
			ElseIf rdbComply.Checked Then
				ActionID = LinkAction.Comply
			ElseIf rdbDoNothing.Checked Then
				ActionID = LinkAction.DoNothing
			End If

			If mLinkMaintenanceList Is Nothing Then
				mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mModelMonitorInspThreshold.ID.ToString)
			End If

			If mLinkMaintenanceList.Count = 0 Then

				mLinkMaintenanceList.add(LinkMaintenance.NewChildLinkedMaintenance(Guid.NewGuid, mModelMonitorInspThreshold.ID, mModelMonitorInspInterval.ID, 2))
				mLinkMaintenanceList(0).MaintenanceActionID = ActionID

				Try
					mLinkMaintenanceList = CType(mLinkMaintenanceList.Save, LinkMaintenanceList)
				Catch ex As SqlException

					If ex.Number = 8145 Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
					ElseIf ex.Number = 2627 Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					ElseIf ex.Number = 547 Then
						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.saveAlert, "This Entry is used by Some One.", MsgBoxStyle.OkOnly, "")
					End If

				End Try

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				SetFocus(txtAMPNo)
				mModelMonitorInspTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList("<SELECT>")
				Session("mModelMonitorInspTypeList") = mModelMonitorInspTypeList
				NewThreshold()
				NewInterval()
				AddSelectedPeriodUnitsThreshold(Today.Date.ToString)
				AddSelectedPeriodUnitsInterval(Today.Date.ToString)
				DataFieldBind()
				SetLicenceCountThreshold()
				SetLicenceCountInterval()
				SetColorThreshold()

			End If

			lblTitle.Text = $"Watchlist Configuration for {RegNo} [ {mAssemblyStatus.ModelName} - {mAssemblyStatus.Assembly.SerialNo} ]"
			txtCurrentValues.Text = AirframeCurrentValues.Trim.Replace("<br>", vbCrLf)
			txtDescription.Text = mModelMonitorInsp.Description.Trim.Replace("<BR>", vbCrLf)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub btnAddPeriodUnit_Click(sender As Object, e As EventArgs) Handles btnAddPeriodUnitThreshold.Click, btnAddPeriodUnitInterval.Click

		'THRESHOLD
		SetObjectThreshold()
		SetPeriodUnitsThreshold()
		SetGridObjectThreshold()

		'Added by saylee on 1-Jun-2016
		If Not mModelMonitorInspThreshold.IsNew Then
			Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
			mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspThreshold.ModelID, mModelMonitorInspThreshold.ID.ToString)

			If mModelMonitorConfiguredList.Count > 0 Then
				Dim SerialNos As String = String.Empty

				For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
					If i = mModelMonitorConfiguredList.Count - 1 Then
						SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
					Else
						SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
					End If
				Next

				Dim ServiceMPDTitle As String = ""
				If AppSettings("ShowMaintenanceForNewClients") = "True" Then
					ServiceMPDTitle = "MPD"
				Else
					ServiceMPDTitle = "Service"
				End If

				MSGBoxCtrl.Show("Alert!", ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")

				Exit Sub

			End If
		End If


		'INTERVAL
		SetObjectInterval()
		SetPeriodUnitsInterval()
		SetGridObjectInterval()

		'Added by saylee on 1-Jun-2016
		If Not mModelMonitorInspInterval.IsNew Then
			Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList
			mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspInterval.ModelID, mModelMonitorInspInterval.ID.ToString)

			If mModelMonitorConfiguredList.Count > 0 Then
				Dim SerialNos As String = String.Empty

				For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
					If i = mModelMonitorConfiguredList.Count - 1 Then
						SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
					Else
						SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
					End If
				Next

				Dim ServiceMPDTitle As String = ""
				If AppSettings("ShowMaintenanceForNewClients") = "True" Then
					ServiceMPDTitle = "MPD"
				Else
					ServiceMPDTitle = "Service"
				End If

				MSGBoxCtrl.Show("Alert!", ServiceMPDTitle + " is already configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So new Frequency cannot be added", "In Order to add frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")

				Exit Sub

			End If
		End If
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPeriodUnitWindow", "OpenPeriodUnitWindow()", True)
	End Sub

	Private Sub hdnBtnPeriodUnit_Click(sender As Object, e As EventArgs) Handles hdnBtnPeriodUnit.Click
		mSelectPeriodUnits = CType(Session("mSelectPeriodUnits"), SelectPeriodUnits)
		If chkIsThreshold.Checked Then AddSelectedPeriodUnitsThreshold(Today.Date.ToString)

		If chkIsInterval.Checked Then AddSelectedPeriodUnitsInterval(Today.Date.ToString)

		dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
		dgPeriodsThreshold.DataBind()
		upnlPeriodsThreshold.Update()

		dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods
		dgPeriodsInterval.DataBind()
		upnlPeriodsInterval.Update()

		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgThresholdValues.DataBind()
		upnlThresholdValues.Update()
		SetColorThreshold()
	End Sub

	Protected Sub txtLicenceNoThreshold_TextChanged(sender As Object, e As EventArgs)
		If (txtLicenceNoThreshold.Text.Trim.IndexOf("[") > 0 And txtLicenceNoThreshold.Text.Trim.IndexOf("]") > 0) Then
			LicenseNoThreshold = txtLicenceNoThreshold.Text.Substring(0, txtLicenceNoThreshold.Text.Trim.IndexOf("[")).Trim
			EmpNameThreshold = Mid(txtLicenceNoThreshold.Text.Trim, txtLicenceNoThreshold.Text.Trim.IndexOf("[") + 2, txtLicenceNoThreshold.Text.Trim.IndexOf("]") - txtLicenceNoThreshold.Text.Trim.IndexOf("[") - 1).Trim
		Else
			LicenseNoThreshold = Trim(txtLicenceNoThreshold.Text)
		End If
		DoneByIDThreshold = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNoThreshold, EmpNameThreshold).EmpID
		Session("LicenseNoThreshold") = LicenseNoThreshold
		Session("EmployeeIDThreshold") = DoneByIDThreshold
		If Not DoneByIDThreshold.Equals(Guid.Empty) Then
			If mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeID = DoneByIDThreshold
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNoThreshold
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursThreshold.Text
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).EmployeeName = EmpNameThreshold
			Else
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatusThreshold.ID, 5, DoneByIDThreshold, LicenseNoThreshold, txtRequiredManHoursThreshold.Text, EmpNameThreshold)
			End If
		Else
			If mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
				mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.RemoveAt(0)
			End If
		End If
		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
		BindLicenceNoThreshold()
		SetLicenceCountThreshold()
		'txtRequiredManHours.DataBind()
		upnlMonitoringStatusDetailsThreshold.Update()
	End Sub

	Protected Sub txtLicenceNoInterval_TextChanged(sender As Object, e As EventArgs)
		If (txtLicenceNoInterval.Text.Trim.IndexOf("[") > 0 And txtLicenceNoInterval.Text.Trim.IndexOf("]") > 0) Then
			LicenseNoInterval = txtLicenceNoInterval.Text.Substring(0, txtLicenceNoInterval.Text.Trim.IndexOf("[")).Trim
			EmpNameInterval = Mid(txtLicenceNoInterval.Text.Trim, txtLicenceNoInterval.Text.Trim.IndexOf("[") + 2, txtLicenceNoInterval.Text.Trim.IndexOf("]") - txtLicenceNoInterval.Text.Trim.IndexOf("[") - 1).Trim
		Else
			LicenseNoInterval = Trim(txtLicenceNoInterval.Text)
		End If
		DoneByIDInterval = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNoInterval, EmpNameInterval).EmpID
		Session("LicenseNoInterval") = LicenseNoInterval
		Session("EmployeeIDInterval") = DoneByIDInterval
		If Not DoneByIDInterval.Equals(Guid.Empty) Then
			If mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).EmployeeID = DoneByIDInterval
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNoInterval
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursInterval.Text
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).EmployeeName = EmpNameInterval
			Else
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatusInterval.ID, 5, DoneByIDInterval, LicenseNoInterval, txtRequiredManHoursInterval.Text, EmpNameInterval)
			End If
		Else
			If mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
				mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.RemoveAt(0)
			End If
		End If
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
		BindLicenceNoInterval()
		SetLicenceCountInterval()
		'txtRequiredManHours.DataBind()
		upnlMonitoringStatusDetailsInterval.Update()
	End Sub

	Protected Sub txtRequiredManHoursThreshold_TextChanged(sender As Object, e As EventArgs)
		If mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees.Count > 0 Then
			mAssemblyMonitorInspStatusThreshold.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursThreshold.Text
			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
			upnlMonitoringStatusDetailsThreshold.Update()
		End If
	End Sub

	Protected Sub txtRequiredManHoursInterval_TextChanged(sender As Object, e As EventArgs)
		If mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees.Count > 0 Then
			mAssemblyMonitorInspStatusInterval.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHoursInterval.Text
			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
			upnlMonitoringStatusDetailsInterval.Update()
		End If
	End Sub

	Protected Sub txtFrequencyValueThreshold_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim txtFrequencyValueThreshold As TextBox = CType(Me.dgPeriodsThreshold.Rows(i).FindControl("txtFrequencyValueThreshold"), TextBox)
			With mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
				.Item(i).FrequencyValue = Trim(txtFrequencyValueThreshold.Text)
			End With
		Next i
		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgThresholdValues.DataBind()
		upnlThresholdValues.Update()
		SetColorThreshold()
		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
	End Sub

	Protected Sub txtDoneOnValueThreshold_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim calDoneOn As TextBox = CType(Me.dgThresholdValues.Rows(i).FindControl("txtDoneOnValueThreshold"), TextBox)
			With mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
				If .Item(i).PeriodID = 2 Then
					If Not Period.IsDate(calDoneOn.Text) Then
						.Item(i).DoneOnValueFormatted = ""
					Else
						.Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
					End If
				Else
					.Item(i).DoneOnValue = Trim(calDoneOn.Text)
				End If
			End With
		Next i
		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgThresholdValues.DataBind()
		upnlThresholdValues.Update()
		SetColorThreshold()
		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
	End Sub

	Protected Sub txtDueOnValueThreshold_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim txtDueOnValue As TextBox = CType(Me.dgThresholdValues.Rows(i).FindControl("txtDueOnValueThreshold"), TextBox)
			With mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
				If .Item(i).PeriodID = 2 Then
					If Not Period.IsDate(txtDueOnValue.Text) Then
						.Item(i).DueOnValueFormatted = ""
					Else
						.Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
					End If
				Else
					.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
				End If
			End With
		Next i

		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgThresholdValues.DataBind()
		upnlThresholdValues.Update()
		SetColorThreshold()
		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
	End Sub

	Protected Sub txtExtensionValueThreshold_TextChanged(sender As Object, e As EventArgs)
		Dim txtExtensionValue As TextBox
		For i As Integer = 0 To mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Count - 1
			txtExtensionValue = CType(Me.dgThresholdValues.Rows(i).FindControl("txtExtensionValueThreshold"), TextBox)

			With mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
				.Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
			End With
		Next
		dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
		dgThresholdValues.DataBind()
		upnlThresholdValues.Update()
		SetColorThreshold()
		Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
	End Sub


	''' INTERVAL
	Protected Sub txtFrequencyValueInterval_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim txtFrequencyValueInterval As TextBox = CType(Me.dgPeriodsInterval.Rows(i).FindControl("txtFrequencyValueInterval"), TextBox)
			With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
				.Item(i).FrequencyValue = Trim(txtFrequencyValueInterval.Text)
			End With
		Next i
		dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
		dgIntervalValues.DataBind()
		upnlIntervalValues.Update()
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
	End Sub

	Protected Sub txtDoneOnValueInterval_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim calDoneOn As TextBox = CType(Me.dgIntervalValues.Rows(i).FindControl("txtDoneOnValueInterval"), TextBox)
			With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
				If .Item(i).PeriodID = 2 Then
					If Not Period.IsDate(calDoneOn.Text) Then
						.Item(i).DoneOnValueFormatted = ""
					Else
						.Item(i).DoneOnValueFormatted = Trim(calDoneOn.Text)
					End If
				Else
					.Item(i).DoneOnValue = Trim(calDoneOn.Text)
				End If
			End With
		Next i
		dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
		dgIntervalValues.DataBind()
		upnlIntervalValues.Update()
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
	End Sub

	Protected Sub txtDueOnValueInterval_TextChanged(sender As Object, e As EventArgs)
		For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
			Dim txtDueOnValue As TextBox = CType(Me.dgIntervalValues.Rows(i).FindControl("txtDueOnValueInterval"), TextBox)
			With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
				If .Item(i).PeriodID = 2 Then
					If Not Period.IsDate(txtDueOnValue.Text) Then
						.Item(i).DueOnValueFormatted = ""
					Else
						.Item(i).DueOnValueFormatted = Trim(txtDueOnValue.Text)
					End If
				Else
					.Item(i).DueOnValue = Trim(txtDueOnValue.Text)
				End If
			End With
		Next i

		dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
		dgIntervalValues.DataBind()
		upnlIntervalValues.Update()
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
	End Sub

	Protected Sub txtExtensionValueInterval_TextChanged(sender As Object, e As EventArgs)
		Dim txtExtensionValue As TextBox
		For i As Integer = 0 To mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Count - 1
			txtExtensionValue = CType(Me.dgIntervalValues.Rows(i).FindControl("txtExtensionValueInterval"), TextBox)

			With mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
				.Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
			End With
		Next
		dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
		dgIntervalValues.DataBind()
		upnlIntervalValues.Update()
		Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		RemoveSession()
		Response.Redirect("index.aspx")
	End Sub

	Private Sub SaveDetails(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If IsValid Then

				If chkIsThreshold.Checked = False And chkIsInterval.Checked = False Then

					MSGBoxCtrl.Show("Alert !!",
									"Please select at least one Threshold Or Interval",
									"",
									MsgBoxStyle.OkOnly,
									"App")

					Exit Sub

				End If

				If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub

				If txtDoneOnDateInterval.Text.ToString <> "" Then

					If CDate(mCloseDate) > CDate(txtDoneOnDateInterval.Text.ToString) Then

						MSGBoxCtrl.Show("Compliance Alert!",
										$"Selected {txtDoneOnDateInterval.Text} date should be greater than closing date 
												   {New SmartDate(mCloseDate.ToString).FormattedText}",
										"",
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

				End If

				If txtDoneOnDateThreshold.Text.ToString <> "" Then

					If CDate(mCloseDate) > CDate(txtDoneOnDateThreshold.Text.ToString) Then

						MSGBoxCtrl.Show("Compliance Alert!",
										$"Selected {txtDoneOnDateThreshold.Text} date should be greater than closing date 
													{New SmartDate(mCloseDate.ToString).FormattedText}",
										"",
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

				End If


				If Not CustomValidate2() = True Then upnlValidationSummary.Update() : Exit Sub

				If Save() = True Then

					pnlThreshold.Enabled = False
					pnlInterval.Enabled = False

					upnlThreshold.Update()
					upnlInterval.Update()

					If Session("OpenFromDiscrepancyCorrectiveActionList") = "True" Then

						Dim DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
						DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
						DiscrepancyCorrectiveAction.ConsideredInWatchList = True

						If DiscrepancyCorrectiveAction.IsValid Then

							DiscrepancyCorrectiveAction.Save()

							MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
											MSGBox.Message_Text.SavedSuccessFully,
											"",
											MsgBoxStyle.OkOnly,
											"")

						End If

					End If

					If Session("OpenFromJOBNRCList") = "True" Then

						Dim WODetails As nWO
						WODetails = Session("WODetails")

						WODetails.
							UpdateJobNRCDetailsForWatchList(NRCJobID:=New Guid(Session("NRCJobID").ToString),
															ConsiderInWatchList:=True)


						MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
										MSGBox.Message_Text.SavedSuccessFully,
										"",
										MsgBoxStyle.OkOnly,
										"")

					End If

				End If

			Else
				upnlValidationSummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub chkIsComplianceThreshold_CheckedChanged(sender As Object, e As EventArgs) Handles rdbIsComplianceThresholdYes.CheckedChanged, rdbIsComplianceThresholdNo.CheckedChanged
		If rdbIsComplianceThresholdYes.Checked Then
			phThresholdDoneDetails.Visible = True
		Else
			phThresholdDoneDetails.Visible = False
		End If
		upnlMonitoringStatusDetailsThreshold.Update()
	End Sub

	Private Sub chkIsComplianceInterval_CheckedChanged(sender As Object, e As EventArgs) Handles rdbIsComplianceIntervalYes.CheckedChanged, rdbIsComplianceIntervalNo.CheckedChanged
		If rdbIsComplianceIntervalYes.Checked Then
			phIntervalDoneDetails.Visible = True
			phNAStart.Visible = False
		Else
			phIntervalDoneDetails.Visible = False
			phNAStart.Visible = True
		End If
		upnlLinkActivity.Update()
		upnlMonitoringStatusDetailsInterval.Update()
	End Sub

	Private Sub dgPeriodsThreshold_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPeriodsThreshold.RowCommand
		Select Case e.CommandName
			Case "DeleteRec"
				Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriodsThreshold.PageIndex * dgPeriodsThreshold.PageSize
				If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
					If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
						If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
							mUnit = mModelMonitorInspThreshold.ModelMonitorInspPeriods(Index).PeriodUnitName
							ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
							Exit Sub
						End If
					ElseIf Not mAssemblyStatus.IsMaster Then
						If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
							ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
							Exit Sub
						End If
					End If '*******************************
				End If

				Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList

				If chkIsThreshold.Checked Then
					mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspThreshold.ModelID, mModelMonitorInspThreshold.ID.ToString)

					If mModelMonitorConfiguredList.Count > 0 Then
						Dim SerialNos As String = String.Empty

						For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
							If i = mModelMonitorConfiguredList.Count - 1 Then
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
							Else
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
							End If
						Next

						MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
						Exit Select
					End If

					mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Remove(mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).ID, "")
					Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
					dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
					dgThresholdValues.DataBind()
					upnlThresholdValues.Update()

					mModelMonitorInspThreshold.ModelMonitorInspPeriods.Remove(mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).ID)
					Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
					dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
					dgPeriodsThreshold.DataBind()
					upnlPeriodsThreshold.Update()

					SetColorThreshold()

				End If

				'Interval

				If chkIsInterval.Checked Then


					mModelMonitorConfiguredList = Nothing
					mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspInterval.ModelID, mModelMonitorInspInterval.ID.ToString)

					If mModelMonitorConfiguredList.Count > 0 Then
						Dim SerialNos As String = String.Empty

						For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
							If i = mModelMonitorConfiguredList.Count - 1 Then
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
							Else
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
							End If
						Next

						MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
						Exit Select
					End If

					mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Remove(mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).ID, "")
					Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
					dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
					dgIntervalValues.DataBind()
					upnlIntervalValues.Update()

					mModelMonitorInspInterval.ModelMonitorInspPeriods.Remove(mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).ID)
					Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
					dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods
					dgPeriodsInterval.DataBind()
					upnlPeriodsInterval.Update()
				End If

		End Select
	End Sub

	Private Sub dgPeriodsInterval_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgPeriodsInterval.RowCommand
		Select Case e.CommandName
			Case "DeleteRec"
				Dim Index As Int32 = CInt(e.CommandArgument) + dgPeriodsInterval.PageIndex * dgPeriodsInterval.PageSize
				If Session("ModelIDFromModelCreation") = Nothing Then 'Added by Saylee on 14-Nov-2019
					If mAssemblyStatus.IsMaster Then 'Added By Utkarsh On 15-Mar-2011
						If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
							mUnit = mModelMonitorInspInterval.ModelMonitorInspPeriods(Index).PeriodUnitName
							ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
							Exit Sub
						End If
					ElseIf Not mAssemblyStatus.IsMaster Then
						If (User.IsInRole("MachineAssemblyServiceNew") Or User.IsInRole("MachineAssemblyServiceEdit")) = False Then
							ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
							Exit Sub
						End If
					End If '*******************************
				End If
				Dim mModelMonitorConfiguredList As ModelMonitorConfiguredList

				If chkIsThreshold.Checked Then



					mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspThreshold.ModelID, mModelMonitorInspThreshold.ID.ToString)

					If mModelMonitorConfiguredList.Count > 0 Then
						Dim SerialNos As String = String.Empty

						For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
							If i = mModelMonitorConfiguredList.Count - 1 Then
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
							Else
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
							End If
						Next

						MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
						Exit Select
					End If
					mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Remove(mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).ID, "")
					Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
					dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
					dgThresholdValues.DataBind()
					upnlThresholdValues.Update()
					SetColorThreshold()

					mModelMonitorInspThreshold.ModelMonitorInspPeriods.Remove(mModelMonitorInspThreshold.ModelMonitorInspPeriods.Item(Index).ID)
					Session("mModelMonitorInspThreshold") = mModelMonitorInspThreshold
					dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
					dgPeriodsThreshold.DataBind()
					upnlPeriodsThreshold.Update()


				End If

				'Interval


				If chkIsInterval.Checked Then


					mModelMonitorConfiguredList = Nothing
					mModelMonitorConfiguredList = ModelMonitorConfiguredList.GetModelMonitorInspConfiguredList(mModelMonitorInspInterval.ModelID, mModelMonitorInspInterval.ID.ToString)

					If mModelMonitorConfiguredList.Count > 0 Then
						Dim SerialNos As String = String.Empty

						For i As Integer = 0 To mModelMonitorConfiguredList.Count - 1
							If i = mModelMonitorConfiguredList.Count - 1 Then
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo
							Else
								SerialNos = SerialNos + mModelMonitorConfiguredList(i).SerialNo + ","
							End If
						Next

						MSGBoxCtrl.Show("Remove Alert!", "Selected " + mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).PeriodUnitName + " frequency is configured on Assembly(ies) [with serial no(s) " & SerialNos & "]. So cannot be removed", "In Order to remove frequency please delete all configured status first.", MsgBoxStyle.OkOnly, "")
						Exit Select
					End If

					mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Remove(mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).ID, "")
					Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
					dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
					dgIntervalValues.DataBind()
					upnlIntervalValues.Update()

					mModelMonitorInspInterval.ModelMonitorInspPeriods.Remove(mModelMonitorInspInterval.ModelMonitorInspPeriods.Item(Index).ID)
					Session("mModelMonitorInspInterval") = mModelMonitorInspInterval
					dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods
					dgPeriodsInterval.DataBind()
					upnlPeriodsInterval.Update()


				End If
		End Select
	End Sub

	Private Sub txtDoneOnDateThreshold_TextChanged(sender As Object, e As EventArgs) Handles txtDoneOnDateThreshold.TextChanged

		If IsPostBack Then

			If CDate(mCloseDate) > CDate(txtDoneOnDateThreshold.Text.ToString) Then
				MSGBoxCtrl.Show("Compliance Alert!", "Selected " + txtDoneOnDateThreshold.Text + " date should be greater than closing date " + New SmartDate(mCloseDate.ToString).FormattedText, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			SetObjectThreshold()
			Dim mAssemblyMonitorInspStatusThresholdClone As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatusThreshold.Clone

			If Session("FromEditThresholdInterval") = "False" Then
				mAssemblyMonitorInspStatusThreshold = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyMonitorInspStatusThreshold.AssemblyID, mAssemblyMonitorInspStatusThreshold.AssemblyStatusID, txtDoneOnDateThreshold.Text.ToString, mModelMonitorInspThreshold.ModelID, mMachine.HourType)
				For Each tmpAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatusThresholdClone.AssemblyMonitorInspStatusPeriods
					tmpAssemblyMonitorInspStatusPeriod.DoneOnValue = tmpAssemblyMonitorInspStatusPeriod.CurrentValue
					mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Add(tmpAssemblyMonitorInspStatusPeriod)
				Next
			Else
				Dim mtmpAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mAssemblyMonitorInspStatusThreshold.ID, mAssemblyMonitorInspStatusThreshold.AssemblyStatusID, txtDoneOnDateThreshold.Text.ToString, mMachine.HourType)
				For Each tmpAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod In mtmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
					mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods.Item(tmpAssemblyMonitorInspStatusPeriod.PeriodID, tmpAssemblyMonitorInspStatusPeriod.PeriodUnitID).DoneOnValue = tmpAssemblyMonitorInspStatusPeriod.CurrentValue
				Next
			End If

			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold


			SetThresholdStatusObject()

			dgThresholdValues.DataSource = mAssemblyMonitorInspStatusThreshold.AssemblyMonitorInspStatusPeriods
			dgThresholdValues.DataBind()
			SetColorThreshold()
			upnlRedLabel.Update()
			'upnlElapsedRemainingValues.Update()
			upnlThresholdValues.Update()
			Session("mAssemblyMonitorInspStatusThreshold") = mAssemblyMonitorInspStatusThreshold
		End If
	End Sub

	Private Sub chkIsApplicable_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsApplicable.CheckedChanged
		If chkIsApplicable.Checked Then
			phCompliance.Visible = True
			phLine.Visible = True
		Else
			phCompliance.Visible = False
			phLine.Visible = False
		End If
	End Sub

	Private Sub txtDoneOnDateInterval_TextChanged(sender As Object, e As EventArgs) Handles txtDoneOnDateInterval.TextChanged
		If IsPostBack Then

			If CDate(mCloseDate) > CDate(txtDoneOnDateInterval.Text.ToString) Then
				MSGBoxCtrl.Show("Compliance Alert!", "Selected " + txtDoneOnDateInterval.Text + " date should be greater than closing date " + New SmartDate(mCloseDate.ToString).FormattedText, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If

			SetObjectInterval()
			Dim mAssemblyMonitorInspStatusIntervalClone As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatusInterval.Clone
			If Session("FromEditThresholdInterval") = "False" Then
				mAssemblyMonitorInspStatusInterval = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(Guid.NewGuid, mAssemblyMonitorInspStatusInterval.AssemblyID, mAssemblyMonitorInspStatusInterval.AssemblyStatusID, txtDoneOnDateInterval.Text.ToString, mModelMonitorInspInterval.ModelID, mMachine.HourType)

				For Each tmpAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatusIntervalClone.AssemblyMonitorInspStatusPeriods
					tmpAssemblyMonitorInspStatusPeriod.DoneOnValue = tmpAssemblyMonitorInspStatusPeriod.CurrentValue
					mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Add(tmpAssemblyMonitorInspStatusPeriod)
				Next
			Else
				Dim mtmpAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mAssemblyMonitorInspStatusInterval.ID, mAssemblyMonitorInspStatusInterval.AssemblyStatusID, txtDoneOnDateInterval.Text.ToString, mMachine.HourType)
				For Each tmpAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod In mtmpAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
					mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods.Item(tmpAssemblyMonitorInspStatusPeriod.PeriodID, tmpAssemblyMonitorInspStatusPeriod.PeriodUnitID).DoneOnValue = tmpAssemblyMonitorInspStatusPeriod.CurrentValue
				Next
			End If



			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval


			SetIntervalStatusObject()
			dgIntervalValues.DataSource = mAssemblyMonitorInspStatusInterval.AssemblyMonitorInspStatusPeriods
			dgIntervalValues.DataBind()
			upnlIntervalValues.Update()
			Session("mAssemblyMonitorInspStatusInterval") = mAssemblyMonitorInspStatusInterval
		End If
	End Sub

	Private Sub chkIsThreshold_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsThreshold.CheckedChanged
		If chkIsThreshold.Checked Then
			AddPeriodUnitsInterval()
		Else
			If mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count > 0 Then

				For i As Integer = mModelMonitorInspThreshold.ModelMonitorInspPeriods.Count - 1 To 0 Step -1
					mModelMonitorInspThreshold.ModelMonitorInspPeriods.RemoveAt(i)
				Next

				dgPeriodsThreshold.DataSource = mModelMonitorInspThreshold.ModelMonitorInspPeriods
				dgPeriodsThreshold.DataBind()
				upnlPeriodsThreshold.Update()
			End If

		End If
	End Sub

	Private Sub chkIsInterval_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsInterval.CheckedChanged
		If chkIsInterval.Checked Then
			pnlLinkActivity.Enabled = True
			AddPeriodUnitsInterval()
		Else
			pnlLinkActivity.Enabled = False

			If mModelMonitorInspInterval.ModelMonitorInspPeriods.Count > 0 Then

				For i As Integer = mModelMonitorInspInterval.ModelMonitorInspPeriods.Count - 1 To 0 Step -1
					mModelMonitorInspInterval.ModelMonitorInspPeriods.RemoveAt(i)
				Next

				dgPeriodsInterval.DataSource = mModelMonitorInspInterval.ModelMonitorInspPeriods
				dgPeriodsInterval.DataBind()
				upnlPeriodsInterval.Update()
			End If

		End If
		upnlLinkActivity.Update()
	End Sub

#End Region

End Class