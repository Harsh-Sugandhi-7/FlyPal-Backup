'Created By Utkarsh ON 13-Mar-2012 For Link Maintenance

<Serializable()> _
Public Class LinkedMaintenanceActivityEvents

#Region "Variable Declaration"

	Dim mMachineList As MachineList

	Dim mtmpMachineList As tmpMachineList

	Private Flag As Int16
	Dim AOdate As String
	Dim AOnDate As String
	Dim Average As String
	Dim Aircraft As String
	Dim Periodcount As Integer
	Dim MachineName As String
	Dim AsonDate As String
	Dim Type As Integer = 1
	Dim AssemblyID As Guid
	Private AssemblyType As String
	Dim AircraftIndex As Integer
	Dim mAssemblyStatusList As AssemblyStatusList
	Dim AssemblyName As String
	Dim Assembly1 As String
	Private AssemblyStatusID As String
	Private ModelID As String
	Dim LogId As String
	Dim LogDate As String
	Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
	Dim tmpAssemblyStatusID As Guid
	Dim HourType As String
	Dim mLog As Log
	Public mMultiComplianceList As New MultiComplianceList
	Public mBoardInfo As AircraftInformationBoard.BoardInfo
	Public mAssemblyInfo As String
	Public mCompInfo As String

	Public mMachineMaintenanceForAssemblyService As MachineMaintenance
	Public mMachineMaintenanceListForAssemblyService As MachineMaintenanceList

	Public mMachineMaintenanceForAssemblyInsp As MachineMaintenance
	Public mMachineMaintenanceListForAssemblyInsp As MachineMaintenanceList

	Public mMachineMaintenanceForAssemblyMod As MachineMaintenance
	Public mMachineMaintenanceListForAssemblyMod As MachineMaintenanceList

	Public mMachineMaintenanceForCompService As MachineMaintenance
	Public mMachineMaintenanceListForCompService As MachineMaintenanceList

	Public mMachineMaintenanceForCompInsp As MachineMaintenance
	Public mMachineMaintenanceListForCompInsp As MachineMaintenanceList
	Public mMachineMaintenanceForCompMod As MachineMaintenance
	Public mMachineMaintenanceListForCompMod As MachineMaintenanceList

	Dim mMaintenanceOnDetail As String = String.Empty
	Public ErrorStr As String = String.Empty
	Public MonitorInfo As String = String.Empty

	Public AssemblyLogInfo As String = String.Empty
	Public MarkLogDetail As String = String.Empty
#End Region

#Region " Enumeration "
	Enum MaintenanceActivityTypes
		RemovalComp = 1
		InstallComp = 2
		RemovalAssembly = 3
		InstallAssembly = 4
		AssemblyService = 5
		AssemblyInspection = 6
		AssemblyDirective = 7
		ComponentService = 8
		ComponentInspection = 9
		ComponentDirective = 10
	End Enum
#End Region

#Region "Helper Events"
	'Assembly Service
	Private Sub SetAssemblyMonitorServiceStatusObject(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, Optional ByVal isFromMulticomplianceForm As Boolean = False)
		'If mMultiCompliance.MaintenanceActionID = 4 Then

		If mMultiCompliance.MaintenanceActionID = 1 Then        'Make Applicable
			mAssemblyMonitorServiceStatus.IsApplicable = True
		ElseIf mMultiCompliance.MaintenanceActionID = 2 Then     'Make Applicable And Start
			mAssemblyMonitorServiceStatus.IsApplicable = True

			'Setting Currrent Values to Done On Values...as default
			For i As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
				With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
					If .Item(i).PeriodID = 2 Then
						If Not Period.IsDate(.Item(i).CurrentValueFormatted) Then
							.Item(i).DoneOnValue = ""
						Else
							.Item(i).DoneOnValueFormatted = .Item(i).CurrentValueFormatted
						End If
					Else
						.Item(i).DoneOnValue = .Item(i).CurrentValue
					End If

					''ExtensionValue
					'.Item(i).ExtensionValue = PeriodValues(i)
				End With
			Next


			'Setting Main Monitor Activity DoneOn Values to linked Monitor Acitvities.
			'(if Main Monitor Activity values changed..... else same as Above chaged values...)
			For i As Integer = 0 To PeriodValues.GetUpperBound(0)   'Number of rows in 2 -dim array. Zero Based
				For j As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
					With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) Or (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).DoneOnValue = ""
								Else
									.Item(j).DoneOnValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).DoneOnValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next

			Next
		ElseIf mMultiCompliance.MaintenanceActionID = 3 Then 'Make Not Applicable
			mAssemblyMonitorServiceStatus.IsApplicable = False

		ElseIf mMultiCompliance.MaintenanceActionID = 4 Then  'Comply

			mAssemblyMonitorServiceStatus.DoneRemark = DoneRemark
			mAssemblyMonitorServiceStatus.DoneWONo = DoneWONo

			mAssemblyMonitorServiceStatus.Place = Place
			mAssemblyMonitorServiceStatus.LicenseNo = LicenceNo
			mAssemblyMonitorServiceStatus.DoneByID = DoneByID


			For i As Integer = 0 To PeriodValues.GetUpperBound(0)  'Number of rows in 2 -dim array.Zero Based
				For j As Integer = 0 To mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count - 1
					With mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) Or (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).CurrentValue = ""
								Else
									.Item(j).CurrentValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).CurrentValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next
			Next
		End If

		If mMultiCompliance.MaintenanceActionID = 4 And Not (mMachineMaintenanceListForAssemblyService.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyService = MachineMaintenance.NewMachineMaintenance(MachineID, 5, CurrentDate, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorServiceStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyService = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
		End If

		With mMachineMaintenanceForAssemblyService
			.MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID

			.Date = CurrentDate


			If LogID.Equals(Guid.Empty) Then
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(CurrentDate, MachineID, AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			Else
				Dim mLog As Log
				mLog = Log.GetLog(LogID)
				If mLog IsNot Nothing Then
					.LogNo = mLog.LogNo
					.LogID = mLog.ID
					.LogPageNo = mLog.LogPageNo
				End If
			End If

		End With

		'Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService
	End Sub
	Private Sub SaveAssemblyMonitorServiceStatusBoardInfo(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
		Dim mAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mAssemblyMonitorServiceStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorServiceStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorServiceStatusPeriod In mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods
				If mAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorServiceStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorServiceStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
		End If
	End Sub
	Private Sub SaveAssemblyMonitorServiceStatus(ByVal mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, ByVal isFromMulticomplianceForm As Boolean)
		Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
		clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

		SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus, mMultiCompliance, DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, LicenceNo, DoneByID, Place, isFromMulticomplianceForm)

		If mAssemblyMonitorServiceStatus.IsValid Then
			If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
				ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "Assembly Service Status can not be saved without period units."
				Exit Sub
			End If
			Try
				mAssemblyMonitorServiceStatus.ApplyEdit()
				mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
				SaveAssemblyMonitorServiceStatusBoardInfo(mAssemblyMonitorServiceStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyService)
				'Added By Utkarsh On 30-May-2012
				MarkLogDetail = "Link Maintenance :: Monitor Activity : Assembly Service Action : " & mMultiCompliance.MaintenanceActionName & " Monitor Info : " + mMultiCompliance.MonitorType + " Monitor Type : " & mMultiCompliance.MonitorInfo & " Description : " & mMultiCompliance.Description & vbCrLf & "Linked With :: " & AssemblyLogInfo
				MarkLog(Util.Action.Save, "Assembly Service Status", MarkLogDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
				'End
			Catch ex As SqlException
				If ex.Number = 8114 Or ex.Number = 8115 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "Rate or Qty or Conversion Factor. "
				ElseIf ex.Number = 8145 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "Procedure error : " & ex.Procedure
				ElseIf ex.Number = 2627 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "You are trying to add Duplicate Entry"
				ElseIf ex.Number = 547 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "Can not delete entry.Used by another entry."
				End If
			Finally
				clnAssemblyMonitorServiceStatus = Nothing
			End Try
		End If
	End Sub
	'End

	'Assembly Inspection
	Private Sub SetAssemblyMonitorInspStatusObject(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, ByVal isFromMulticomplianceForm As Boolean)
		'If mMultiCompliance.MaintenanceActionID = 4 Then        'Comply

		If mMultiCompliance.MaintenanceActionID = 1 Then        'Make Applicable
			mAssemblyMonitorInspStatus.IsApplicable = True
		ElseIf mMultiCompliance.MaintenanceActionID = 2 Then    'Make Applicable and Start
			mAssemblyMonitorInspStatus.IsApplicable = True

			'Setting Currrent Values to Done On Values...as default
			For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
				With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
					If .Item(i).PeriodID = 2 Then
						If Not Period.IsDate(.Item(i).CurrentValueFormatted) Then
							.Item(i).DoneOnValue = ""
						Else
							.Item(i).DoneOnValueFormatted = .Item(i).CurrentValueFormatted
						End If
					Else
						.Item(i).DoneOnValue = .Item(i).CurrentValue
					End If

					''ExtensionValue
					'.Item(i).ExtensionValue = PeriodValues(i)
				End With
			Next

			'Setting Main Monitor Activity DoneOn Values to linked Monitor Acitvities.
			'(if Main Monitor Activity values changed..... else same as Above chaged values...)
			For i As Integer = 0 To PeriodValues.GetUpperBound(0)   'Number of rows in 2 -dim array. Zero Based
				For j As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
					With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) And (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).DoneOnValue = ""
								Else
									.Item(j).DoneOnValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).DoneOnValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next

			Next
		ElseIf mMultiCompliance.MaintenanceActionID = 3 Then 'Make Not Applicable
			mAssemblyMonitorInspStatus.IsApplicable = False

		ElseIf mMultiCompliance.MaintenanceActionID = 4 Then  'Comply

			mAssemblyMonitorInspStatus.DoneRemark = DoneRemark 'mMultiCompliance.DoneRemark
			mAssemblyMonitorInspStatus.DoneWONo = DoneWONo

			mAssemblyMonitorInspStatus.Place = Place
			mAssemblyMonitorInspStatus.LicenseNo = LicenceNo
			mAssemblyMonitorInspStatus.DoneByID = DoneByID


			For i As Integer = 0 To PeriodValues.GetUpperBound(0)  'Number of rows in 2 -dim array.Zero Based
				For j As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
					With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) And (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).CurrentValue = ""
								Else
									.Item(j).CurrentValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).CurrentValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next
			Next
		End If



		If mMultiCompliance.MaintenanceActionID = 4 And Not (mMachineMaintenanceListForAssemblyInsp.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then    ''Session("From") = 0 And
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.NewMachineMaintenance(MachineID, 6, CurrentDate, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorInspStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
		End If

		With mMachineMaintenanceForAssemblyInsp
			.MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID

			.Date = CurrentDate


			If LogID.Equals(Guid.Empty) Then
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(CurrentDate, MachineID, AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			Else
				Dim mLog As Log
				mLog = Log.GetLog(LogID)
				If mLog IsNot Nothing Then
					.LogNo = mLog.LogNo
					.LogID = mLog.ID
					.LogPageNo = mLog.LogPageNo
				End If
			End If
		End With
	End Sub
	Private Sub SaveAssemblyMonitorInspStatusBoardInfo(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
		Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mAssemblyMonitorInspStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorInspStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
				If mAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
		End If
	End Sub
	Private Sub SaveAssemblyMonitorInspStatus(ByVal mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, ByVal isFromMulticomplianceForm As Boolean)
		Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
		clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

		SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus, mMultiCompliance, DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, LicenceNo, DoneByID, Place, isFromMulticomplianceForm)
		If mAssemblyMonitorInspStatus.IsValid Then
			If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
				ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "Assembly Inspection Status can not be saved without period units."
			End If
			Try
				mAssemblyMonitorInspStatus.ApplyEdit()
				mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
				SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyInsp)
				mMaintenanceOnDetail = Replace(mMultiCompliance.MaintenanceOn, "<BR>", "  ").ToString
				'Added By Utkarsh On 30-May-2012
				MarkLogDetail = "Link Maintenance :: Monitor Activity : Assembly Inspection Action : " & mMultiCompliance.MaintenanceActionName & " Monitor Info : " + mMultiCompliance.MonitorType + " Monitor Type : " & mMultiCompliance.MonitorInfo & " Description : " & mMultiCompliance.Description & vbCrLf & "Linked With :: " & AssemblyLogInfo
				MarkLog(Util.Action.Save, "Assembly Insection Status", MarkLogDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
				'End
			Catch ex As SqlException
				If ex.Number = 8114 Or ex.Number = 8115 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "Rate or Qty or Conversion Factor. "
				ElseIf ex.Number = 8145 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "Procedure error : " & ex.Procedure
				ElseIf ex.Number = 2627 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "You are trying to add Duplicate Entry"
				ElseIf ex.Number = 547 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "Can not delete entry.Used by another entry."
				End If
			Finally
				clnAssemblyMonitorInspStatus = Nothing
			End Try
		End If
	End Sub
	'End

	'Assembly Directives
	Private Sub SetAssemblyMonitorModStatusObject(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, ByVal isFromMulticomplianceForm As Boolean)
		'[If mMultiCompliance.MaintenanceActionID = 4 Then

		If mMultiCompliance.MaintenanceActionID = 1 Then 'Make Applicable
			mAssemblyMonitorModStatus.IsApplicable = True
		ElseIf mMultiCompliance.MaintenanceActionID = 2 Then 'Make Applicable and Start
			mAssemblyMonitorModStatus.IsApplicable = True

			For i As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
				With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
					If .Item(i).PeriodID = 2 Then
						If Not Period.IsDate(.Item(i).CurrentValueFormatted) Then
							.Item(i).DoneOnValue = ""
						Else
							.Item(i).DoneOnValueFormatted = .Item(i).CurrentValueFormatted
						End If
					Else
						.Item(i).DoneOnValue = .Item(i).CurrentValue
					End If

					''ExtensionValue
					'.Item(i).ExtensionValue = PeriodValues(i)
				End With
			Next

			'Setting Main Monitor Activity DoneOn Values to linked Monitor Acitvities.
			'(if Main Monitor Activity values changed..... else same as Above chaged values...)
			For i As Integer = 0 To PeriodValues.GetUpperBound(0)   'Number of rows in 2 -dim array. Zero Based
				For j As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
					With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) And (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).DoneOnValue = ""
								Else
									.Item(j).DoneOnValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).DoneOnValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next

			Next

		ElseIf mMultiCompliance.MaintenanceActionID = 3 Then 'Make Not Applicable
			mAssemblyMonitorModStatus.IsApplicable = False

		ElseIf mMultiCompliance.MaintenanceActionID = 4 Then  'Comply

			mAssemblyMonitorModStatus.DoneRemark = DoneRemark 'mMultiCompliance.DoneRemark
			mAssemblyMonitorModStatus.DoneWONo = DoneWONo

			mAssemblyMonitorModStatus.Place = Place
			mAssemblyMonitorModStatus.LicenseNo = LicenceNo
			mAssemblyMonitorModStatus.DoneByID = DoneByID

			For i As Integer = 0 To PeriodValues.GetUpperBound(0)  'Number of rows in 2 -dim array.Zero Based
				For j As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
					With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
						If (isFromMulticomplianceForm = False And PeriodValues(i, 0) = (.Item(j).PeriodUnitID)) And (isFromMulticomplianceForm = True And PeriodValues(i, 0) = (.Item(j).PeriodID)) Then

							If .Item(j).PeriodID = 2 Then
								If Not Period.IsDate(PeriodValues(i, 1)) Then
									.Item(j).CurrentValue = ""
								Else
									.Item(j).CurrentValueFormatted = PeriodValues(i, 1)
								End If
							Else
								.Item(j).CurrentValue = PeriodValues(i, 1)
							End If
						End If
					End With
				Next
			Next
		End If


		'Added by Saylee on 28th-Oct-2009
		If mMultiCompliance.MaintenanceActionID = 4 And Not (mMachineMaintenanceListForAssemblyMod.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then  ''Session("From") = 0 And
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.NewMachineMaintenance(MachineID, 7, CurrentDate, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorModStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
		End If

		With mMachineMaintenanceForAssemblyMod
			.MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID

			.Date = CurrentDate

			If LogID.Equals(Guid.Empty) Then
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(CurrentDate, MachineID, AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			Else
				Dim mLog As Log
				mLog = Log.GetLog(LogID)
				If mLog IsNot Nothing Then
					.LogNo = mLog.LogNo
					.LogID = mLog.ID
					.LogPageNo = mLog.LogPageNo
				End If
			End If

		End With
	End Sub
	Private Sub SaveAssemblyMonitorModStatusBoardInfo(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String)
		Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
		Dim DueOnValue As String

		If (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mAssemblyMonitorModStatus.DoneOn IsNot DBNull.Value) Or (mAssemblyMonitorModStatus.IsApplicable = False) Then
			DueOnValue = ""
		Else
			For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
				If mAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueFormatted
				Else
					DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueTextFormatted
				End If
			Next
		End If

		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
		End If
	End Sub
	Private Sub SaveAssemblyMonitorModStatus(ByVal mAssemblyMonitorModStatus As AssemblyMonitorModStatus, ByVal mMultiCompliance As MultiCompliance, ByVal DoneWONo As String, ByVal CurrentDate As String, ByVal LogID As Guid, ByVal HourType As Integer, ByVal MachineID As Guid, ByVal AssemblyID As Guid, ByVal PeriodValues(,) As String, ByVal DoneRemark As String, ByVal LicenceNo As String, ByVal DoneByID As Guid, ByVal Place As String, ByVal isFromMulticomplianceForm As Boolean)
		Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
		clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

		SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus, mMultiCompliance, DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, LicenceNo, DoneByID, Place, isFromMulticomplianceForm)
		If mAssemblyMonitorModStatus.IsValid Then
			If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
				ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "Assembly Directive Status can not be saved without period units."
			End If
			Try
				mAssemblyMonitorModStatus.ApplyEdit()
				mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
				SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus, DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyMod)
				'Added By Utkarsh On 30-May-2012
				MarkLogDetail = "Link Maintenance :: Monitor Activity : Assembly Directive Action : " & mMultiCompliance.MaintenanceActionName & " Monitor Info : " + mMultiCompliance.MonitorType + " Monitor Type : " & mMultiCompliance.MonitorInfo & " Description : " & mMultiCompliance.Description & vbCrLf & "Linked With :: " & AssemblyLogInfo
				MarkLog(Util.Action.Save, "Assembly Directive Status", MarkLogDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
				'End
			Catch ex As SqlException
				If ex.Number = 8114 Or ex.Number = 8115 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "Rate or Qty or Conversion Factor. "
				ElseIf ex.Number = 8145 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "Procedure error : " & ex.Procedure
				ElseIf ex.Number = 2627 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "You are trying to add Duplicate Entry"
				ElseIf ex.Number = 547 Then
					ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "Can not delete entry.Used by another entry."
				End If
			Finally
				clnAssemblyMonitorModStatus = Nothing
			End Try
		End If
	End Sub

	'End
	Private Sub SetMachineMaintenanceObject(ByVal MachineID As Guid, ByVal AssemblyID As Guid)
		Dim mMachineMaintenanceList As MachineMaintenanceList
		mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList(, , , MachineID.ToString, , AssemblyID.ToString)

		mMachineMaintenanceListForAssemblyService = mMachineMaintenanceList

		mMachineMaintenanceListForAssemblyInsp = mMachineMaintenanceList

		mMachineMaintenanceListForAssemblyMod = mMachineMaintenanceList

	End Sub
	Private Sub SaveMachineMaintenance(ByVal mMachineMaintenance As MachineMaintenance)
		If mMachineMaintenance.IsValid = True Then
			Try
				mMachineMaintenance.ApplyEdit()
				mMachineMaintenance.Save()
			Catch ex As Exception

			End Try
		End If
	End Sub
#End Region

#Region "Main Event"
	Public Function SaveLinkedMaintenanceActivies(ByVal MultiComplianceList As MultiComplianceList,
												  ByVal DoneWONo As String,
												  ByVal CurrentDate As String,
												  ByVal LogID As Guid,
												  ByVal HourType As Integer,
												  ByVal MachineID As Guid,
												  ByVal AssemblyID As Guid,
												  ByVal PeriodValues(,) As String,
												  ByVal DoneRemark As String,
												  Optional ByVal LicenceNo As String = "",
												  Optional ByVal EmployeeID As String = "",
												  Optional ByVal EmployeeName As String = "",
												  Optional ByVal Place As String = "",
												  Optional isFromMulticomplianceForm As Boolean = False,
												  Optional isFromWOComplaiance As Boolean = False
												 ) As Boolean
		SetMachineMaintenanceObject(MachineID, AssemblyID)

		Dim index As Integer
		For index = 0 To MultiComplianceList.Count - 1

			'Added by Saylee on 15-Sep-2025, as for complaince we need to skip because it is already in WOjob and will be complied from WO
			If isFromWOComplaiance = True And MultiComplianceList(index).MaintenanceActionID = 4 Then
				GoTo NextIndex
			End If
			'*******************************************

			If MultiComplianceList(index).IsSelect Then
				Select Case MultiComplianceList(index).MaintenanceActivity

					Case MaintenanceActivityTypes.AssemblyService '5. Assembly Service
						If MultiComplianceList(index).MaintenanceActionID = 5 Then 'Action = Do Nothing
							Exit Select
						End If
						Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
						Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus

						mPrevAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(MultiComplianceList(index).AssemblyMonitorServiceStatusID, MultiComplianceList.Item(index).AssemblyStatusID, HourType)
						If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
							'ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mPrevAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "You are trying to comply the record.One time monitoring already done. Can not be complied again."
							Exit Select
						ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
							ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Service : </B>" & mPrevAssemblyMonitorServiceStatus.ModelMonitorService.Description & "<BR>" & "You are trying to comply the record.Expiery compliance already done. Can not be complied again."
							Exit Select
						Else
							mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
							If MultiComplianceList(index).MaintenanceActionID = 4 Then  'Action = Comply

								mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, CurrentDate, MultiComplianceList(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, LogID, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, HourType)
								mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
							Else
								mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorServiceStatus.ID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, CurrentDate, Guid.Empty, HourType, mPrevAssemblyMonitorServiceStatus.ModelMonitorServiceID, True)
							End If
							With mAssemblyMonitorServiceStatus
								Dim Licenses() As String
								Dim EmpID() As String
								Dim EmpName() As String

								If LicenceNo <> "" Then
									If .MaintenanceDoneByEmployees.Count > 0 Then
										.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorServiceStatus.ID)
									End If

									Licenses = LicenceNo.Split(",")
									EmpID = EmployeeID.Split(",")
									EmpName = EmployeeName.Split(",")

									For i As Integer = 0 To EmpID.Length - 1
										.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, MultiComplianceList(index).MaintenanceActivity, Guid.Empty, Licenses(i), "", EmpName(i))
										.MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
									Next

									.LicenseNo = Licenses(0)
									.DoneByID = New Guid(EmpID(0))
								End If
							End With

							SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus, MultiComplianceList.Item(index), DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, mAssemblyMonitorServiceStatus.LicenseNo, mAssemblyMonitorServiceStatus.DoneByID, Place, isFromMulticomplianceForm)
						End If
					Case MaintenanceActivityTypes.AssemblyInspection   '6. Assembly Inspection
						If MultiComplianceList(index).MaintenanceActionID = 5 Then 'Action = Do Nothing
							Exit Select
						End If
						Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
						Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(MultiComplianceList.Item(index).AssemblyMonitorInspStatusID, MultiComplianceList.Item(index).AssemblyStatusID, HourType)
						If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
							'ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "You are trying to comply the record.One time monitoring already done. Can not be complied again."
							Exit Select
						ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
							ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Inspection : </B>" & mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.Description & "<BR>" & "You are trying to comply the record.Expiery compliance already done. Can not be complied again."
							Exit Select
						Else
							mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)

							If MultiComplianceList(index).MaintenanceActionID = 4 Then  'Action = Comply

								mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, CurrentDate, MultiComplianceList(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, LogID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, HourType)
								mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours

							Else
								mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, CurrentDate, Guid.Empty, HourType, mPrevAssemblyMonitorInspStatus.ModelMonitorInspID, True)

							End If

							With mAssemblyMonitorInspStatus
								Dim Licenses() As String
								Dim EmpID() As String
								Dim EmpName() As String

								If LicenceNo <> "" Then
									If .MaintenanceDoneByEmployees.Count > 0 Then
										.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorInspStatus.ID)
									End If

									Licenses = LicenceNo.Split(",")
									EmpID = EmployeeID.Split(",")
									EmpName = EmployeeName.Split(",")

									For i As Integer = 0 To EmpID.Length - 1
										.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, MultiComplianceList(index).MaintenanceActivity, Guid.Empty, Licenses(i), "", EmpName(i))
										.MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
									Next
									.LicenseNo = Licenses(0)
									.DoneByID = New Guid(EmpID(0))
								End If
							End With
							SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus, MultiComplianceList.Item(index), DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, mAssemblyMonitorInspStatus.LicenseNo, mAssemblyMonitorInspStatus.DoneByID, Place, isFromMulticomplianceForm)
						End If
					Case MaintenanceActivityTypes.AssemblyDirective    '7. Assembly Directive
						Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
						Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(MultiComplianceList.Item(index).AssemblyMonitorDirectiveStatusID, MultiComplianceList.Item(index).AssemblyStatusID, HourType)
						If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
							'ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mPrevAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "You are trying to comply the record.One time monitoring already done. Can not be complied again."
							Exit Select

						ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
							ErrorStr = ErrorStr & " <BR>" & "<B>Assembly Directive : </B>" & mPrevAssemblyMonitorModStatus.ModelMonitorMod.Description & "<BR>" & "You are trying to comply the record.Expiery compliance already done. Can not be complied again."
							Exit Select
						Else
							mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)

							If MultiComplianceList(index).MaintenanceActionID = 4 Then  'Action = Comply
								mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, CurrentDate, MultiComplianceList(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, HourType)
								mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
							Else
								mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, CurrentDate, Guid.Empty, HourType, mPrevAssemblyMonitorModStatus.ModelMonitorModID, True)
							End If

							With mAssemblyMonitorModStatus
								Dim Licenses() As String
								Dim EmpID() As String
								Dim EmpName() As String

								If LicenceNo <> "" Then
									If .MaintenanceDoneByEmployees.Count > 0 Then
										.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorModStatus.ID)
									End If

									Licenses = LicenceNo.Split(",")
									EmpID = EmployeeID.Split(",")
									EmpName = EmployeeName.Split(",")

									For i As Integer = 0 To EmpID.Length - 1
										.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, MultiComplianceList(index).MaintenanceActivity, Guid.Empty, Licenses(i), "", EmpName(i))
										.MaintenanceDoneByEmployees.CurrentItem.EmployeeID = New Guid(EmpID(i))
									Next
									.LicenseNo = Licenses(0)
									.DoneByID = New Guid(EmpID(0))
								End If
							End With
							SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus, MultiComplianceList.Item(index), DoneWONo, CurrentDate, LogID, HourType, MachineID, AssemblyID, PeriodValues, DoneRemark, mAssemblyMonitorModStatus.LicenseNo, mAssemblyMonitorModStatus.DoneByID, Place, isFromMulticomplianceForm)
						End If
				End Select
			End If
NextIndex: Next
		Return True
	End Function
#End Region

End Class
