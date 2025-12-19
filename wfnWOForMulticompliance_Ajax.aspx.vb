'************************************
'AJAX Created By :   Saylee
'Dated           :   14-July-2015
'Modified by Harsh Sugandhi on 26th May 2025 for FlyPaL-2439.
'************************************


Imports System.Collections.Generic
Imports System.Text


Public Class wfnWOForMulticompliance_Ajax
	Inherits Page

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

#Region " Variable Declaration "

	Public mSelectDueJobForWO As SelectDueJobFornWO
	Public mSelectDueJobsForWO As SelectDueJobsFornWO
	Public mnWO As nWO
	Public mDueLimits As DueLimits

	Dim mLog As Log
	Dim AsonDate As String
	Dim AOnDate As String
	Dim MachineName As String
	Dim AssemblyName As String
	Public mAssemblyInfo As String
	Public mCompInfo As String
	Dim LogId As String
	Dim WOName As String

	Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
	Public mBoardInfo As AircraftInformationBoard.BoardInfo

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

	Dim EventLogID As Guid
	Dim mAssemblyInfoDetail As String
	Dim LicenseNo As String = String.Empty
	Dim EmpName As String = String.Empty
	Dim DoneByID As Guid = Guid.Empty
	Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
	Shared UserNameForLicenceList As String
	Dim NoOfRecordsSelected As Integer
	Dim TypeID As Integer()
	Dim AssemblyMonitorServiceStatusIDs, AssemblyMonitorInspStatusIDs, AssemblyMonitorModStatusIDs, CompMonitorInspStatusIDs, CompMonitorModStatusIDs, CompMonitorServiceStatusIDs As New StringBuilder
	Dim NumberSequence As ArrayList = New ArrayList

	Private checkedIds As New List(Of String)()

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mnWO = Session("mnWO")
		AsonDate = Session("AsonDate")
		MachineName = Session("AircraftId")
		WOName = Session("WOId")

		LogId = CType(Session("LogID"), String)
		mSelectDueJobForWO = Session("mSelectDueJobForWO")
		mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
		AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")

		mMachineMaintenanceForAssemblyService = CType(Session("mMachineMaintenanceForAssemblyService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyService = CType(Session("mMachineMaintenanceListForAssemblyService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForAssemblyInsp = CType(Session("mMachineMaintenanceForAssemblyInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyInsp = CType(Session("mMachineMaintenanceListForAssemblyInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForAssemblyMod = CType(Session("mMachineMaintenanceForAssemblyMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForAssemblyMod = CType(Session("mMachineMaintenanceListForAssemblyMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompService = CType(Session("mMachineMaintenanceForCompService"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompService = CType(Session("mMachineMaintenanceListForCompService"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompInsp = CType(Session("mMachineMaintenanceForCompInsp"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompInsp = CType(Session("mMachineMaintenanceListForCompInsp"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009

		mMachineMaintenanceForCompMod = CType(Session("mMachineMaintenanceForCompMod"), MachineMaintenance) 'Added by Saylee on 28th-Oct-2009
		mMachineMaintenanceListForCompMod = CType(Session("mMachineMaintenanceListForCompMod"), MachineMaintenanceList) 'Added by Saylee on 28th-Oct-2009
		mDueLimits = CType(Session("mDueLimits"), DueLimits)

		'MLNo
		mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
		'End
	End Sub

	'Added by vikrant on 06-Sep-2019 for ALL06092019
	Private Function IsExpiryOCSLLServicePresent() As Boolean
		'For i As Integer = 0 To mSelectDueJobsForWO.Count - 1
		'    If mSelectDueJobsForWO.Item(i).IsSelected And (mSelectDueJobsForWO.Item(i).MonitorTypeID = 2 Or mSelectDueJobsForWO.Item(i).MonitorTypeID = 5 Or mSelectDueJobsForWO.Item(i).MonitorTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV")) Then
		'        Return True
		'    End If
		'Next
		'Return False
	End Function
	'End

	'Added by vikrant on 06-Sep-2019 for ALL06092019
	Private Sub RemoveComp(mDoneOn As String, mCompStatus As CompStatus, mMachine As Machine)
		''Removal

		Dim mMachineID As Guid = Guid.Empty
		If Not mnWO.IsSpareAssemblyWO Then
			mMachineID = mMachine.ID
		End If

		Dim mtmpInstalledCompList As tmpInstalledCompList
		mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(mDoneOn, mMachineID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, IsSpareAssembly:=IIf(mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO, True, False))
		'' Session("mInstalledCompList") = mInstalledCompList

		Dim mRemCompStatus As CompStatus
		mRemCompStatus = CompStatus.NewRemovalCompStatus(mtmpInstalledCompList(mCompStatus.ID).CompStatusID, mDoneOn.ToString,
														 mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID, Guid.Empty.ToString)

		Session("From_Remove") = 1 'NewRemove

		Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpInstalledCompList(mCompStatus.ID).CompStatusID,
																	 mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID,
																	 mtmpInstalledCompList(mCompStatus.ID).InstalledOnDBValue)
		'Added By Vikrant On 05-Oct-2021 for ALL05102021-1
		mRemCompStatus.RemovalWONo = mnWO.WONumber
		mPrevCompStatus.RemovalWONo = mnWO.WONumber
		'End

		Session("mRemCompStatus") = mRemCompStatus
		Dim mRemAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID)
		Session("mRemAssemblyStatus") = mRemAssemblyStatus
		Session("mPrevCompStatus") = mPrevCompStatus
		Session("From_Remove") = 1
		Session("From_Inst") = 1
		Session("mtmpInstalledCompList") = mtmpInstalledCompList
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then
			Session("IsFromSpareWO") = "True"
		End If
		'End
		Dim URLForWOCompliance As New Stack
		URLForWOCompliance.Push(Request.Url)
		Session("URLForWOCompliance") = URLForWOCompliance


		Session.Remove("mDoneOnCompliance")
		Response.Redirect("wfRemInstComp_AJAX.aspx?BackPage=" & Request.QueryString("BackPage1"))
	End Sub
	'End

	Private Sub SetIds()
		NumberSequence.Add(5)

		For Each mnWOJob As nWOJob In mnWO.WOJobs
			If mnWOJob.WOJobTypeID = 2 Then
				If mnWOJob.OnTypeID = 1 Then
					If Not NumberSequence.Contains(3) Then
						NumberSequence.Add(3)
					End If
					If mnWOJob.MonitorTypeID = 1 Then
						If Not NumberSequence.Contains(5) Then
							NumberSequence.Add(5)
						End If
						If AssemblyMonitorServiceStatusIDs.ToString = "" Then
							AssemblyMonitorServiceStatusIDs.Append("<AssMonServiceID>")
						End If
						AssemblyMonitorServiceStatusIDs.Append("<id>")
						AssemblyMonitorServiceStatusIDs.Append(mnWOJob.PreviousTransID)
						AssemblyMonitorServiceStatusIDs.Append("</id>")
					End If
					If mnWOJob.MonitorTypeID = 2 Then
						If Not NumberSequence.Contains(6) Then
							NumberSequence.Add(6)
						End If
						If AssemblyMonitorInspStatusIDs.ToString = "" Then
							AssemblyMonitorInspStatusIDs.Append("<AssMonInspID>")
						End If
						AssemblyMonitorInspStatusIDs.Append("<id>")
						AssemblyMonitorInspStatusIDs.Append(mnWOJob.PreviousTransID)
						AssemblyMonitorInspStatusIDs.Append("</id>")
					End If
					If mnWOJob.MonitorTypeID = 3 Then
						If Not NumberSequence.Contains(7) Then
							NumberSequence.Add(7)
						End If
						If AssemblyMonitorModStatusIDs.ToString = "" Then
							AssemblyMonitorModStatusIDs.Append("<AssMonModID>")
						End If
						AssemblyMonitorModStatusIDs.Append("<id>")
						AssemblyMonitorModStatusIDs.Append(mnWOJob.PreviousTransID)
						AssemblyMonitorModStatusIDs.Append("</id>")
					End If
				ElseIf mnWOJob.OnTypeID = 2 Then
					If Not NumberSequence.Contains(4) Then
						NumberSequence.Add(4)
					End If
					If mnWOJob.MonitorTypeID = 1 Then
						If Not NumberSequence.Contains(8) Then
							NumberSequence.Add(8)
						End If
						If CompMonitorServiceStatusIDs.ToString = "" Then
							CompMonitorServiceStatusIDs.Append("<CompMonServiceID>")
						End If
						CompMonitorServiceStatusIDs.Append("<id>")
						CompMonitorServiceStatusIDs.Append(mnWOJob.PreviousTransID)
						CompMonitorServiceStatusIDs.Append("</id>")
					End If
					If mnWOJob.MonitorTypeID = 2 Then
						If Not NumberSequence.Contains(9) Then
							NumberSequence.Add(9)
						End If
						If CompMonitorInspStatusIDs.ToString = "" Then
							CompMonitorInspStatusIDs.Append("<CompMonInspID>")
						End If
						CompMonitorInspStatusIDs.Append("<id>")
						CompMonitorInspStatusIDs.Append(mnWOJob.PreviousTransID)
						CompMonitorInspStatusIDs.Append("</id>")
					End If
					If mnWOJob.MonitorTypeID = 3 Then
						If Not NumberSequence.Contains(10) Then
							NumberSequence.Add(10)
						End If
						If CompMonitorModStatusIDs.ToString = "" Then
							CompMonitorModStatusIDs.Append("<CompMonModID>")
						End If
						CompMonitorModStatusIDs.Append("<id>")
						CompMonitorModStatusIDs.Append(mnWOJob.PreviousTransID)
						CompMonitorModStatusIDs.Append("</id>")
					End If
				End If
			End If
		Next
		If AssemblyMonitorServiceStatusIDs.ToString <> "" Then
			AssemblyMonitorServiceStatusIDs.Append("</AssMonServiceID>")
		End If
		If AssemblyMonitorInspStatusIDs.ToString <> "" Then
			AssemblyMonitorInspStatusIDs.Append("</AssMonInspID>")
		End If
		If AssemblyMonitorModStatusIDs.ToString <> "" Then
			AssemblyMonitorModStatusIDs.Append("</AssMonModID>")
		End If
		If CompMonitorServiceStatusIDs.ToString <> "" Then
			CompMonitorServiceStatusIDs.Append("</CompMonServiceID>")
		End If
		If CompMonitorInspStatusIDs.ToString <> "" Then
			CompMonitorInspStatusIDs.Append("</CompMonInspID>")
		End If
		If CompMonitorModStatusIDs.ToString <> "" Then
			CompMonitorModStatusIDs.Append("</CompMonModID>")
		End If

		TypeID = CType(NumberSequence.ToArray(GetType(Integer)), Integer())
	End Sub

	Private Sub SetSession()
		Session("mnWO") = mnWO
		Session("LogId") = LogId
		Session("AsonDate") = AsonDate

		Session("LogId") = LogId
		Session("AsonDate") = AsonDate
		Session("AircraftId") = MachineName

		Session("mSelectDueJobForWO") = mSelectDueJobForWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

		Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceListForAssemblyService 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceListForAssemblyInsp 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceListForAssemblyMod 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceListForCompService 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceListForCompInsp 'Added by Saylee on 28th-Oct-2009

		Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod 'Added by Saylee on 28th-Oct-2009
		Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceListForCompMod 'Added by Saylee on 28th-Oct-2009
	End Sub

	Private Sub RemoveSession()
		Session.Remove("AsonDate")
		Session.Remove("AonDate")
		Session.Remove("AircraftId")

		Session.Remove("mSelectDueJobForWO")
		Session.Remove("mSelectDueJobsForWO")
		Session.Remove("mDueLimits")

		Session.Remove("mLog")

		Session.Remove("MachineName")
		Session.Remove("mAssemblyInfo")
		Session.Remove("mCompInfo")
		Session.Remove("LogId")

		Session.Remove("mMachineMaintenanceForAssemblyService")
		Session.Remove("mMachineMaintenanceListForAssemblyService")

		Session.Remove("mMachineMaintenanceForAssemblyInsp")
		Session.Remove("mMachineMaintenanceListForAssemblyInsp")

		Session.Remove("mMachineMaintenanceForAssemblyMod")
		Session.Remove("mMachineMaintenanceListForAssemblyMod")

		Session.Remove("mMachineMaintenanceForCompService")
		Session.Remove("mMachineMaintenanceListForCompService")

		Session.Remove("mMachineMaintenanceForCompInsp")
		Session.Remove("mMachineMaintenanceListForCompInsp")

		Session.Remove("mMachineMaintenanceForCompMod")
		Session.Remove("mMachineMaintenanceListForCompMod")

		Session.Remove("WOId")

		Session.Remove("OpenFindNowSelectLogForm")

	End Sub

	Private Sub AddJobs()
		NoOfRecordsSelected = AddItemsToList()

		Dim builder = New StringBuilder()
		builder.Append("You have selected the following checks :<br/>")
		' get the selected checkboxes from the form data
		'Dim checkString = Request.Form("chkSelect")
		If NoOfRecordsSelected = 0 Then
			MSGBoxCtrl.Show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		Else
			' we'll need a split to get the individual ids
			For i As Integer = 0 To mSelectDueJobsForWO.Count - 1
				mSelectDueJobsForWO(i).IsSelected = CType(dgDueJob.Rows(i).FindControl("chkSelect"), CheckBox).Checked
			Next
			'Dim values = checkString.Split(","c)
			'For Each value As String In values
			'    builder.Append("<br/>")
			'    builder.Append(value)
			'    checkedIds.Add(value)
			'    mSelectDueJobsForWO(New Guid(value)).IsSelected = True
			'Next


			'For i As Integer = 0 To mSelectDueJobsForWO.Count - 1
			'    If mSelectDueJobsForWO(i).IsSelected = True And Array.IndexOf(values, mSelectDueJobsForWO(i).ID.ToString) = -1 Then
			'        mSelectDueJobsForWO(i).IsSelected = False
			'    End If
			'Next
			'values = ""
			'checkString = Nothing
		End If

		'For i As Integer = 0 To mSelectDueJobsForWO.Count - 1
		'    If mSelectDueJobsForWO(i).IsSelected = False Then
		'        If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(i).ID, "") Then
		'            mnWO.WOJobs.Remove(mSelectDueJobsForWO.Item(i).ID, "")
		'        End If
		'    End If
		'Next

		Dim item As GridViewRow
		Dim txtComplyRemark As TextBox
		Dim Recordno, PageItems As Integer

		Dim txtLicenceNo, txtPlace, txtActualManHrs As TextBox 'Added By Vikrant On 21-Jun-2016 For ALL21062016

		PageItems = dgDueJob.Rows.Count - 1
		' Set Selected DoneRemark value  
		For i As Integer = 0 To PageItems
			Recordno = i + dgDueJob.PageSize * dgDueJob.PageIndex
			item = dgDueJob.Rows(i)
			If mSelectDueJobsForWO(Recordno).IsSelected = True Then
				txtComplyRemark = CType(item.FindControl("txtAssemblyRemark"), TextBox)
				mSelectDueJobsForWO(Recordno).DoneRemark = txtComplyRemark.Text
				'Added By Vikrant On 21-Jun-2016 For ALL21062016
				Dim LicenseNo As String = String.Empty
				Dim EmpName As String = String.Empty
				txtLicenceNo = CType(item.FindControl("txtLicenceNo"), TextBox)
				If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
					LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
					EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
				Else
					LicenseNo = Trim(txtLicenceNo.Text)
				End If
				mSelectDueJobsForWO(Recordno).LicenseNo = LicenseNo
				mSelectDueJobsForWO(Recordno).DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
				txtPlace = CType(item.FindControl("txtPlace"), TextBox)
				mSelectDueJobsForWO(Recordno).Place = Trim(txtPlace.Text)
				txtActualManHrs = CType(item.FindControl("txtActualManHrs"), TextBox)
				mSelectDueJobsForWO(Recordno).RequiredManHours = Trim(txtActualManHrs.Text)
				'End
			End If

		Next
		'Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		Session("mnWO") = mnWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
	End Sub

	Private Sub SetValues()
		mnWO = Session("mnWO")
		MachineName = mnWO.MachineID.ToString
		WOName = mnWO.ID.ToString

		If CType(Session("LogId"), String) <> "" Or Session("LogId") IsNot Nothing Then
			'' SetLog()
			'do nothing
		Else
			'Commented By Vikrant On 07-Oct-2020 For Slow Perf SA
			'Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, MachineName.ToString, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
			'AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
			'tmpAssemblyStatusList = Nothing
			'End
			Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
		End If

		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'If Condition Added By Vikrant On 27-Jul-2020 For ALL27072020
			dgDoneOnValue.DataSource = mnWO.WOPeriods
			'End
		Else 'Existing Condition
			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
		End If
		dgDoneOnValue.DataBind()

		If (txtAsOnDate.Text = "") Then
			AsonDate = ""
			AOnDate = ""
		Else
			AsonDate = txtAsOnDate.Text
			AOnDate = txtAsOnDate.Text
		End If

		Session("AsonDate") = AsonDate
		Session("AonDate") = AOnDate
		Session("AircraftId") = MachineName
		Session("WOId") = WOName
	End Sub

	Private Sub ControlVisibility()
		mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
		If mSelectDueJobsForWO IsNot Nothing Then
			If mSelectDueJobsForWO.Count > 0 Then
				btnSave.Enabled = True
				If mSelectDueJobsForWO.Count > 10 Then
					btnSaveTop.Visible = True
					btnCloseTop.Visible = True
				Else
					btnSaveTop.Visible = False
					btnCloseTop.Visible = False
				End If
				lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
			Else
				btnSaveTop.Visible = False
				btnCloseTop.Visible = False
				btnSave.Enabled = False
			End If
		Else
			btnSave.Enabled = False
			btnSaveTop.Visible = False
			btnCloseTop.Visible = False
		End If
		'Added By Vikrant On 27-Jul-2020 For ALL27072020
		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then
			btnSelectLog.Enabled = False
		End If
		'End
	End Sub

	Private Sub ResetValues()
		MachineName = "{00000000-0000-0000-0000-000000000000}"
		If AsonDate <> "" Then
			txtAsOnDate.Text = AsonDate
		End If
		AsonDate = ""
		AssemblyName = Guid.Empty.ToString
		mSelectDueJobsForWO = Nothing
	End Sub

	'Added By Vikrant On 21-Jun-2016 For ALL21062016
	Private Function IsEmployeeWorking(DoneByID As Guid, DoneOn As Object) As String
		If Not DoneByID.Equals(Guid.Empty) AndAlso Not DoneOn.Equals(DBNull.Value) Then
			Dim title As String = "Save Alert !"
			Dim message As String = ""
			Dim mEmployeeStatus As EmployeeStatus

			mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(DoneByID.ToString, DoneOn)
			If (mEmployeeStatus(0).Information <> "") Then
				message = mEmployeeStatus(0).Information
				'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
				MSGBoxCtrl.Show(title, message, "", MsgBoxStyle.OkOnly, "")
				Return message
			Else
				Return ""
			End If
		Else
			Return True
		End If
	End Function
	'End
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					If MSGBoxCtrl.Sender = "RemoveComp" Then
						RemoveComp(CType(Session("mDoneOnCompliance"), String), CType(Session("mCompStatus"), CompStatus), CType(Session("mMachine"), Machine))
					End If

				Case MsgBoxResult.No
					If MSGBoxCtrl.Sender = "RemoveComp" Then
						Session("sender") = ""


					End If

				Case MsgBoxResult.Cancel

				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					Session("sender") = ""
					'DataFieldBind()
					'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
				Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
					Session("sender") = ""
					'DataFieldBind()
					'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
			End Select
		ElseIf Result1 = -1 Then
			Session("sender") = ""
		ElseIf Result1 = 0 Then   'Code Added
			Session("sender") = ""
		End If
	End Sub

#End Region

#Region " Data Binding "

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtAsOnDate" Then
			If (CDate(txtAsOnDate.Text) < CDate(mnWO.WODate.ToString)) And (Not AppSettings("ClientCode") = "STR" And Not AppSettings("ClientCode") = "IND") Then
				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					custValidator.ErrorMessage = "Compliance Date should be Later to or Equal to E.O. Date  " + mnWO.WODateFormatted + "."
				Else
					custValidator.ErrorMessage = "Compliance Date should be Later to or Equal to Work Order Date " + mnWO.WODateFormatted + "."
				End If
				e.IsValid = False
				Exit Sub
			ElseIf (CDate(txtAsOnDate.Text + " " + "23:59") < CDate(mnWO.WODate.ToString)) And (AppSettings("ClientCode") = "STR" Or AppSettings("ClientCode") = "IND") Then
				custValidator.ErrorMessage = "Compliance Date should be Later to or Equal to Work Order Date " + mnWO.WODateFormatted + "."
				e.IsValid = False
				Exit Sub
			End If
		ElseIf custValidator.ControlToValidate = "txtHiddenActManHrs" Then
			'Added By Vikrant On 21-Jun-2016 For ALL21062016
			Dim txtActualManHrs As TextBox
			Dim mActualManHours As New Period(1, DBNull.Value, 0, True, False)
			For i As Integer = 0 To dgDueJob.Rows.Count - 1
				txtActualManHrs = CType(dgDueJob.Rows(i).FindControl("txtActualManHrs"), TextBox)
				mActualManHours.Value = Trim(txtActualManHrs.Text)
				If (Not mActualManHours.IsValid And mActualManHours.Value <> "") Then
					custValidator.ErrorMessage = "Actual Man Hours : " & mActualManHours.ErrMsg
					e.IsValid = False
					Exit For
				Else
					e.IsValid = True
				End If
			Next
			'End
		ElseIf custValidator.ControlToValidate = "txtWOLabel" Then
			Dim txtLicNo As TextBox
			For i As Integer = 0 To dgDueJob.Rows.Count - 1
				txtLicNo = CType(dgDueJob.Rows(i).FindControl("txtLicenceNo"), TextBox)
				If (txtLicNo.Text.Trim.IndexOf("[") > 0 And txtLicNo.Text.Trim.IndexOf("]") > 0) Or (txtLicNo.Text.Trim.IndexOf("[") < 0 And txtLicNo.Text.Trim.IndexOf("]") < 0) Then
					e.IsValid = True
				Else
					custValidator.ErrorMessage = "Enter Correct License No."
					e.IsValid = False
					Exit For
				End If
			Next
		End If
		'End
	End Sub

	Private Sub DataFieldBind()
		'mWOListForCombo = FlyPal22.Maintain.WOListForCombo.GetWOListForCombo("(SELECT)")

		Dim mMachineMaintenanceList As MachineMaintenanceList
		mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
		Session("mMachineMaintenanceListForAssemblyService") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForAssemblyInsp") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForAssemblyMod") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompService") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompInsp") = mMachineMaintenanceList
		Session("mMachineMaintenanceListForCompMod") = mMachineMaintenanceList


		If AsonDate <> "" Then
			txtAsOnDate.Text = New SmartDate(AsonDate.ToString).FormattedText.ToString 'CDate(AsonDate).ToString
		Else
			txtAsOnDate.Text = mnWO.WOCloseDateFormattedForCompliance.ToString  'Modified by Harsh Sugandhi on 7th January 2025 => FLYPAL-2111
		End If

		If mSelectDueJobsForWO IsNot Nothing Then
			dgDueJob.DataSource = mSelectDueJobsForWO
			dgDueJob.DataBind()
		End If

		If AssemblyStatusPeriodList IsNot Nothing Then
			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			dgDoneOnValue.DataBind()
		ElseIf mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'Added By Vikrant On 27-Jul-2020 For ALL27072020
			dgDoneOnValue.DataSource = mnWO.WOPeriods
			dgDoneOnValue.DataBind()
			'End
		End If

		If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
			If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'If condition Added By Vikrant On 27-Jul-2020 For ALL27072020
				dgDoneOnValue.DataSource = mnWO.WOPeriods
				'End
			Else
				dgDoneOnValue.DataSource = AssemblyStatusPeriodList
			End If
			dgDoneOnValue.DataBind()
			txtAsOnDate.Text = AsonDate
		End If

		mDueLimits = DueLimits.GetDueLimits(New Guid("{00000000-0000-0000-0000-000000000000}"))
		Session("mDueLimits") = mDueLimits

		SetIds()

		mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mnWO.MachineID.ToString, 0, mnWO, AssemblyMonitorInspStatusIDs:=AssemblyMonitorInspStatusIDs.ToString, AssemblyMonitorModStatusIDs:=AssemblyMonitorModStatusIDs.ToString, CompMonitorInspStatusIDs:=CompMonitorInspStatusIDs.ToString, CompMonitorModStatusIDs:=CompMonitorModStatusIDs.ToString, CompMonitorServiceStatusIDs:=CompMonitorServiceStatusIDs.ToString, TypeID:=TypeID, AssemblyMonitorServiceStatusIDs:=AssemblyMonitorServiceStatusIDs.ToString, IsForSpareAssembly:=IIf(mnWO.TransTypeID = Trans.SpareAssemblyWO And CBool(Session("IsWOForRemovedOrSpareAssembly")) = True, True, False), IsForSpareComponent:=IIf(mnWO.TransTypeID = Trans.SpareComponentWO And CBool(Session("IsWOForRemovedOrSpareComp") = True), True, False), IsSpareOrRemoveComp:=IIf(mnWO.TransTypeID = Trans.SpareComponentWO And CBool(Session("IsWOForRemovedOrSpareComp") = False), 2, 1))

		''If mSelectDueJobsForWO.Count = 0 Then
		''    Dim msg1 As New SIMsgBox(Page, "Monitoring Services / Inspections / Directives not available", "<BR><BR> All Monitoring Services / Inspections / Directives may be already complied.", "", MsgBoxStyle.OKOnly)
		''    msg1.ReplacePage = "wfnWOForMulticompliance.aspx?BackPage1=" & Request.QueryString("BackPage1") & "&BackPage=" & Request.QueryString("BackPage")
		''    msg1.Show()
		''    dgDueJob.DataSource = mSelectDueJobsForWO
		''    Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		''    Session("mWO") = mWO
		''    dgDueJob.DataBind()
		''    Exit Sub
		''End If
		dgDueJob.DataSource = mSelectDueJobsForWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		Session("mnWO") = mnWO

		dgDueJob.DataBind()
		BindPlace() 'Sankalp 18-08-25
		BindLicenceNo(ShowDefaultValuesOnPageLoad:=True)

		If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'if condition Added By Vikrant On 27-Jul-2020 For ALL27072020
			dgDoneOnValue.DataSource = mnWO.WOPeriods
			'End
		Else
			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
			AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
			Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
			dgDoneOnValue.DataSource = AssemblyStatusPeriodList
		End If

		dgDoneOnValue.DataBind()



		Dim WOstr As String = ""
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			WOstr = "Engineering Order"
		Else
			WOstr = "Work Order"
		End If

		If mSelectDueJobsForWO.Count > 0 Then
			btnSave.Enabled = True
			If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
			If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True
			lblNote.Text = ""
		Else
			btnSave.Enabled = False
			lblNote.Text = "*Note : There are no Due jobs in this " & WOstr & " which may have been already complied by using Maintenance menu option."
		End If
		lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
		txtWOLabel.DataBind()

	End Sub

#End Region

#Region " Machine Maintenance "

	Private Sub SaveMachineMaintenance(mMachineMaintenance As MachineMaintenance)
		'Added by Saylee on 9th-Oct-2009
		If mMachineMaintenance.IsValid = True Then
			Try
				mMachineMaintenance.ApplyEdit()
				mMachineMaintenance.Save()
				Session("mMachineMaintenance") = mMachineMaintenance
				Session("mComplyMachineMaintenance") = mMachineMaintenance

			Catch ex As Exception

			End Try
		End If
		''  End If
	End Sub

#End Region

#Region " Common Methods "

	Public Sub CallCommonCodeAfterComplaince()
		SetIds()
		' mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mnWO.MachineID.ToString, 0, mnWO)
		'mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(Today.Date.ToString, mDueLimits, mnWO.MachineID.ToString, 0, mnWO)

		Dim CAMOUpdateRemark As String = If(mnWO.CAMOUpdateRemark = "", "", mnWO.CAMOUpdateRemark)
		mnWO = nWO.GetWO(mnWO.ID, False)
		Session("mnWO") = mnWO

		If (mnWO.WOJobs.IsCompleted = True) And (mnWO.WOJobs.IsScheduledJobExists) And (mnWO.WOJobs.IsJobsComplied = True) Then
			Try

				'Added By Vikrant On 14-Oct-2019 For New WO
				If AppSettings("ShowNewWOFlow") = "True" Then 'If AppSettings("ClientCode") = "IND" Then
					mnWO.IsCAMOUpdated = 1
					mnWO.CAMOUpdateRemark = CAMOUpdateRemark
					Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
					Dim mEmployee As Employee
					If Not mUser.EmployeeID.Equals(Guid.Empty) Then
						mEmployee = Employee.GetEmployee(mUser.EmployeeID)
						mnWO.CAMOUpdatedBy = mEmployee.Name
					End If
				End If
				'End
				mnWO.Save()

				Dim mnWOApproveReject As nWOApproveReject
				mnWOApproveReject = Session("mnWOApproveReject")
				If mnWOApproveReject IsNot Nothing Then
					Dim mUser As User = SI.UTILITY.User.GetUser(User.Identity.Name)
					Dim mEmployee As Employee
					If Not mUser.EmployeeID.Equals(Guid.Empty) Then
						mEmployee = Employee.GetEmployee(mUser.EmployeeID)
						mnWOApproveReject.DoneBy = mEmployee.Name
					End If
					mnWOApproveReject.Save()

					''mWODetail = Session("mWODetailforMarklog") + mnWOApproveReject.DoneBy
					''MarkLog(Util.Action.Save, "Work Order", mWODetail, Util.ErrorType.NoError, mnWO.ID, EventLogID)
					Session.Remove("mnWOApproveReject")
					mnWOApproveReject = Nothing
				End If

				Dim mWODetail As String = mnWO.IsCAMOUpdatedStatus + ": " + mnWO.WONumber + " Dated : " + mnWO.WODateFormatted + " Created By : " + mnWO.WOBy + IIf(Not mnWO.MachineID.Equals(Guid.Empty), " Aircraft : " + mnWO.RegNo, "") + IIf(mnWO.ModelName <> "", " Model : " + mnWO.ModelName, "") + IIf(mnWO.SerialNo <> "", " Serial No. : " + mnWO.SerialNo, "")
				MarkLog(Util.Action.CAMOUpdated, "Work Order", mWODetail, Util.ErrorType.NoError, mnWO.ID, EventLogID)
			Catch ex As Exception

			End Try

		End If
		mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mnWO.MachineID.ToString, 0, mnWO, AssemblyMonitorInspStatusIDs:=AssemblyMonitorInspStatusIDs.ToString, AssemblyMonitorModStatusIDs:=AssemblyMonitorModStatusIDs.ToString, CompMonitorInspStatusIDs:=CompMonitorInspStatusIDs.ToString, CompMonitorModStatusIDs:=CompMonitorModStatusIDs.ToString, CompMonitorServiceStatusIDs:=CompMonitorServiceStatusIDs.ToString, TypeID:=TypeID, AssemblyMonitorServiceStatusIDs:=AssemblyMonitorServiceStatusIDs.ToString, IsForSpareAssembly:=IIf(mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO, True, False))

		dgDueJob.DataSource = mSelectDueJobsForWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		mnWO = nWO.GetWO(mnWO.ID, False)
		Session("mnWO") = mnWO
		dgDueJob.DataBind()
		lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."

		Dim WOstr As String = ""
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			WOstr = "Engineering Order"
		Else
			WOstr = "Work Order"
		End If
		If mSelectDueJobsForWO.Count = 0 Then lblNote.Text = "*Note : There are no Due jobs in this " & WOstr & " which may have been already complied by using Maintenance menu option."

		'Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
		'AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
		'Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
		Session.Remove("FromLog")
		dgDoneOnValue.DataSource = AssemblyStatusPeriodList
		dgDoneOnValue.DataBind()
		ControlVisibility()
		SetLicenceCount()
		upnlResult.Update()
		upnlDueJob.Update()
		upnlNote.Update()
		upnlCurrent.Update()
		upnlTitle.Update()

	End Sub

#End Region

#Region " Save Status "

#Region " Assembly Service Status "

	Private Sub SaveAssemblyMonitorServiceStatusBoardInfo(mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus)
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

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorServiceStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub

	Public Function SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
		clnAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Clone, AssemblyMonitorServiceStatus)

		SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus, mSelectDueJobForWO)

		If mAssemblyMonitorServiceStatus.IsValid Then
			If mAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Monitor Service Status.Assembly Monitor Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If
			Try
				mAssemblyMonitorServiceStatus.ApplyEdit()
				mAssemblyMonitorServiceStatus = CType(mAssemblyMonitorServiceStatus.Save(), AssemblyMonitorServiceStatus)
				SaveAssemblyMonitorServiceStatusBoardInfo(mAssemblyMonitorServiceStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyService)
				Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyServiceMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorServiceStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorServiceStatus") = clnAssemblyMonitorServiceStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Finally
				clnAssemblyMonitorServiceStatus = Nothing
			End Try
		End If
	End Function

	Private Sub SetAssemblyMonitorServiceStatusObject(mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorServiceStatus.DoneWONo = mnWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForAssemblyService.Contains(mAssemblyMonitorServiceStatus.ID, 5, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 5, txtAsOnDate.Text, mAssemblyMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorServiceStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyService = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorServiceStatus.ID, 5)
		End If

		With mMachineMaintenanceForAssemblyService
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorServiceStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyService") = mMachineMaintenanceForAssemblyService
	End Sub

#End Region

#Region " Assembly Inspection Status "

	Private Sub SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
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

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub

	Public Function SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
		clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)

		SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus, mSelectDueJobForWO)

		If mAssemblyMonitorInspStatus.IsValid Then
			If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Monitor Insp Status.Assembly Monitor Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If

			Try
				mAssemblyMonitorInspStatus.ApplyEdit()
				mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
				SaveAssemblyMonitorInspStatusBoardInfo(mAssemblyMonitorInspStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyInsp)
				Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyInspMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Finally
				clnAssemblyMonitorInspStatus = Nothing
			End Try
		End If
	End Function

	Private Sub SetAssemblyMonitorInspStatusObject(mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorInspStatus.DoneWONo = mnWO.WONumber

		If Not (mMachineMaintenanceListForAssemblyInsp.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 6, txtAsOnDate.Text, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorInspStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyInsp = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
		End If

		With mMachineMaintenanceForAssemblyInsp
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyInsp") = mMachineMaintenanceForAssemblyInsp
	End Sub

#End Region

#Region " Assembly Modification Status "

	Private Sub SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus As AssemblyMonitorModStatus)
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

		mBoardInfo = Session("mBoardInfo")
		If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
			mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
			mBoardInfo.DueOnValue = DueOnValue
			mBoardInfo.ApplyEdit()
			mBoardInfo.Save()
			Session("mBoardInfo") = mBoardInfo
		End If
		Session("mAircraftInformationBoardList") = Nothing
	End Sub

	Public Function SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus As AssemblyMonitorModStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
		clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)

		SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus, mSelectDueJobForWO)

		If mAssemblyMonitorModStatus.IsValid Then
			If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Monitor Mod Status.Assembly Monitor Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If
			Try
				mAssemblyMonitorModStatus.ApplyEdit()
				mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
				SaveAssemblyMonitorModStatusBoardInfo(mAssemblyMonitorModStatus)
				SaveMachineMaintenance(mMachineMaintenanceForAssemblyMod)
				Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
				mAssemblyInfo = Session("mAssemblyInfo")
				'MarkLog(Util.Action.Save, "AssemblyModMonitor", mAssemblyInfo, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID)
				mAssemblyInfoDetail = Replace(mAssemblyInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Assembly Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Finally
				clnAssemblyMonitorModStatus = Nothing
			End Try
		End If
	End Function

	Private Sub SetAssemblyMonitorModStatusObject(mAssemblyMonitorModStatus As AssemblyMonitorModStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mAssemblyMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mAssemblyMonitorModStatus.DoneWONo = mnWO.WONumber

		If Not (mMachineMaintenanceListForAssemblyMod.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 7, txtAsOnDate.Text, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyMonitorModStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForAssemblyMod = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
		End If

		With mMachineMaintenanceForAssemblyMod
			''.MachineID = mAssemblyStatus.MachineID
			''.MaintenanceActivityTypeID =5
			.MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text
			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForAssemblyMod") = mMachineMaintenanceForAssemblyMod
	End Sub

#End Region

#Region " Component Service Status "

	Private Sub SetCompMonitorServiceStatusObject(mCompMonitorServiceStatus As CompMonitorServiceStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorServiceStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorServiceStatus.DoneWONo = mnWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompService.Contains(mCompMonitorServiceStatus.ID, 8, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompService = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 8, txtAsOnDate.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mCompMonitorServiceStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompService = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, 8)
		End If

		With mMachineMaintenanceForCompService
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompService") = mMachineMaintenanceForCompService
	End Sub

	Public Function SaveCompMonitorServiceStatus(mCompMonitorServiceStatus As CompMonitorServiceStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
		clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)

		SetCompMonitorServiceStatusObject(mCompMonitorServiceStatus, mSelectDueJobForWO)
		If mCompMonitorServiceStatus.IsValid Then
			If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Monitor Service Status.Component Monitor Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If
			Try
				mCompMonitorServiceStatus.ApplyEdit()
				mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
				Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompService)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompServiceMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Service Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)

				Return True
			Catch ex As SqlException
				Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Finally
				clnCompMonitorServiceStatus = Nothing
			End Try
		End If
	End Function

#End Region

#Region " Component Insp Status "

	Private Sub SetCompMonitorInspStatusObject(mCompMonitorInspStatus As CompMonitorInspStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorInspStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorInspStatus.DoneWONo = mnWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompInsp.Contains(mCompMonitorInspStatus.ID, 9, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompInsp = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 9, txtAsOnDate.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, mCompMonitorInspStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompInsp = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, 9)
		End If

		With mMachineMaintenanceForCompInsp
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompInsp") = mMachineMaintenanceForCompInsp
	End Sub

	Public Function SaveCompMonitorInspStatus(mCompMonitorInspStatus As CompMonitorInspStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorInspStatus As CompMonitorInspStatus
		clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)

		SetCompMonitorInspStatusObject(mCompMonitorInspStatus, mSelectDueJobForWO)
		If mCompMonitorInspStatus.IsValid Then
			If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Monitor Insp Status.Component Monitor Insp Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If
			Try
				mCompMonitorInspStatus.ApplyEdit()
				mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
				Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompInsp)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompInspMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Inspection Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If

			Finally
				clnCompMonitorInspStatus = Nothing
			End Try
		End If
	End Function

#End Region

#Region "Component Mod Status"
	Private Sub SetCompMonitorModStatusObject(mCompMonitorModStatus As CompMonitorModStatus, mSelectDueJobForWO As SelectDueJobFornWO)
		mCompMonitorModStatus.DoneRemark = mSelectDueJobForWO.DoneRemark
		mCompMonitorModStatus.DoneWONo = mnWO.WONumber

		'Added by Saylee on 28th-Oct-2009
		If Not (mMachineMaintenanceListForCompMod.Contains(mCompMonitorModStatus.ID, 10, "")) Then  '' Session("From") = 0 And
			mMachineMaintenanceForCompMod = MachineMaintenance.NewMachineMaintenance(New Guid(MachineName), 10, txtAsOnDate.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mCompMonitorModStatus.AssemblyStatusID)
		Else
			mMachineMaintenanceForCompMod = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, 10)
		End If

		With mMachineMaintenanceForCompMod
			''.MachineID = mCompStatus.MachineID
			''.MaintenanceActivityTypeID =8
			.MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
			''.AssemblyStatusID = mAssemblyStatus.ID

			.Date = txtAsOnDate.Text

			mLog = CType(Session("mLog"), Log)
			If mLog IsNot Nothing Then
				.LogNo = mLog.LogNo
				.LogID = mLog.ID
				.LogPageNo = mLog.LogPageNo
			Else
				Dim mMaxLogNo As MaxLogNo
				mMaxLogNo = MaxLogNo.GetMaxLogNo(txtAsOnDate.Text, New Guid(MachineName), mSelectDueJobForWO.AssemblyID)
				If mMaxLogNo.Count <> 0 Then
					.LogNo = mMaxLogNo(0).LogNo
					.LogID = mMaxLogNo(0).LogId
					.LogPageNo = mMaxLogNo(0).LogPageNo
				End If
			End If

		End With

		Session("mMachineMaintenanceForCompMod") = mMachineMaintenanceForCompMod
	End Sub
	Public Function SaveCompMonitorModStatus(mCompMonitorModStatus As CompMonitorModStatus, mSelectDueJobForWO As SelectDueJobFornWO) As Boolean
		Dim clnCompMonitorModStatus As CompMonitorModStatus
		clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)

		SetCompMonitorModStatusObject(mCompMonitorModStatus, mSelectDueJobForWO)
		If mCompMonitorModStatus.IsValid Then
			If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Monitor Mod Status.Component Monitor Mod Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
				Return False
			End If
			Try
				mCompMonitorModStatus.ApplyEdit()
				mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
				Session("mCompMonitorModStatus") = mCompMonitorModStatus
				SaveMachineMaintenance(mMachineMaintenanceForCompMod)
				mCompInfo = Session("mCompInfo")
				'MarkLog(Util.Action.Save, "CompModMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)
				mAssemblyInfoDetail = Replace(mCompInfo, "<BR>", "  ").ToString
				MarkLog(Util.Action.Save, "Component Modification Monitor", mAssemblyInfoDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)
				Return True
			Catch ex As SqlException
				Session("mCompMonitorModStatus") = clnCompMonitorModStatus
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				End If
			Finally
				clnCompMonitorModStatus = Nothing
			End Try
		End If
	End Function
#End Region

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

		If Not IsPostBack Then

			If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then

				ResetValues()
				txtAsOnDate.Text = mnWO.WOCloseDateFormattedForCompliance.ToString
				AOnDate = mnWO.WOCloseDateFormattedForCompliance.ToString 'Modified by Harsh Sugandhi on 7th January 2025 => FLYPAL-2111

			Else

				If AsonDate <> "" Then
					txtAsOnDate.Text = New SmartDate(AsonDate).FormattedText.ToString
				End If

			End If

			DataFieldBind()
			ControlVisibility()
			SetLicenceCount()

		End If

	End Sub

	Protected Sub txtAsOnDate_TextChanged(sender As Object, e As EventArgs)
		If txtAsOnDate.Text <> "" Then
			If Not (mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO) Then 'if condition Added By Vikrant On 27-Jul-2020 For ALL27072020
				Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList
				Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

				dgDoneOnValue.DataSource = AssemblyStatusPeriodList
				dgDoneOnValue.DataBind()
				upnlCurrent.Update()
			End If
		End If
	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub btnSelectLog_Click(sender As Object, e As EventArgs) Handles btnSelectLog.Click
		If IsValid = True Then
			'Dim builder = New StringBuilder()
			'builder.Append("You have selected the following checks :<br/>")
			'' get the selected checkboxes from the form data
			'Dim checkString = Request.Form("chkSelect")
			'If checkString Is Nothing Then
			'    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
			'    Exit Sub
			'End If
			NoOfRecordsSelected = AddItemsToList()
			If NoOfRecordsSelected = 0 Then
				MSGBoxCtrl.Show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
			SetValues()
			AddJobs()
			SetSession()
			Session("OpenFindNowSelectLogForm") = True
			mnWO = Session("mnWO")
			SetValues()
			Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Text, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
			' Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage6=wfnWOForMulticompliance.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & tmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & tmpAssemblyStatusList(0).AssemblyID.ToString)
			Session("mFromType") = 3
			Session("mMachineId") = tmpAssemblyStatusList(0).MachineID.ToString
			Session("mAssemblyStatusId") = tmpAssemblyStatusList(0).ID.ToString
			Session("mAssemblyID") = tmpAssemblyStatusList(0).AssemblyID.ToString
			Session("mDoneOn") = CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate))
			ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow();", True)


		End If
	End Sub

	Private Sub hdnBtnSelectLog_Click(sender As Object, e As EventArgs) Handles hdnBtnSelectLog.Click
		If Session("LogId") <> "" Or Session("LogId") IsNot Nothing Then
			LogId = CType(Session("LogID"), String)
			Session("LogId") = CType(Session("LogId"), String)
			Dim LogDate = CType(Session("mDoneOn"), String)
			mnWO = Session("mnWO")
			MachineName = mnWO.MachineID.ToString

			Dim mLog As Log
			mLog = Log.GetLog(New Guid(LogId.ToString))
			Session("mLog") = mLog

			If Not LogId.Equals(Guid.Empty) Then
				Dim tmpAssemblyStatusList As AssemblyStatusList
				If mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO Then 'if condition Added By Vikrant On 27-Jul-2020 For ALL27072020
					dgDoneOnValue.DataSource = mnWO.WOPeriods
				Else
					tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, MachineName.ToString, , , , , , , , , , True, , , , "Airframe", LogID:=mLog.ID.ToString, MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
					AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
					Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
					dgDoneOnValue.DataSource = AssemblyStatusPeriodList
					'End
				End If
				dgDoneOnValue.DataBind()
				upnlCurrent.Update()
				'Session.Remove("FromLog")
				tmpAssemblyStatusList = Nothing
			End If

		End If

	End Sub

	Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click, btnSaveTop.Click

		If Not Page.IsValid Then upnlValidationsummary.Update() : Exit Sub

		NoOfRecordsSelected = AddItemsToList()
		If NoOfRecordsSelected = 0 Then
			MSGBoxCtrl.Show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		SetValues()
		AddJobs()

		mSelectDueJobsForWO = Session("mSelectDueJobsForWO")

		Dim index As Integer
		Dim IsSelected As Boolean = False

		For index = 0 To mSelectDueJobsForWO.Count - 1
			If mSelectDueJobsForWO.Item(index).IsSelected = True Then
				IsSelected = True
				Exit For
			End If
		Next

		If IsSelected = True Then

			For index = 0 To mSelectDueJobsForWO.Count - 1

				If mSelectDueJobsForWO.Item(index).IsSelected = True Then

					Dim mMachine As Machine
					Dim mAssemblyStatus As AssemblyStatus
					Dim mHourType As Integer = 1

					Dim MethodOfCompliance As String = ""
					If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
						MethodOfCompliance = mnWO.WOJobs(mSelectDueJobsForWO.Item(index).WOJobID).MethodOfCompliance
					End If



					If Not mnWO.IsSpareAssemblyWO Then 'If condition Added By Vikrant On 27-Jul-2020 For ALL27072020
						mMachine = Machine.GetMachine(mSelectDueJobsForWO.Item(index).MachineID)
					End If

					If mSelectDueJobsForWO(index).IsSpareComponent = False Then
						mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mSelectDueJobsForWO(index).AssemblyStatusID)
						mHourType = mAssemblyStatus.HourType
					End If

					If mSelectDueJobsForWO(index).OnAssemblyOrComponent = "Assembly" Then

						Select Case mSelectDueJobsForWO(index).DataType
							Case "Servicing" 'Service

								Dim mAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus
								Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mHourType)

								If mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 1 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevAssemblyMonitorServiceStatus.ModelMonitorService.MonitorTypeID = 4 And mPrevAssemblyMonitorServiceStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, New Guid(LogId), mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mHourType)
									Else
										mAssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.NewComplyAssemblyMonitorServiceStatus(Guid.NewGuid, mPrevAssemblyMonitorServiceStatus.AssemblyID, mPrevAssemblyMonitorServiceStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorServiceStatus.ModelMonitorService, Guid.Empty, mPrevAssemblyMonitorServiceStatus.DoneOn.ToString, mHourType)
									End If

									Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
									Session("mPrevAssemblyMonitorServiceStatus") = mPrevAssemblyMonitorServiceStatus
									Session("From") = 0 'New record
									mAssemblyMonitorServiceStatus.RequiredManHours = mAssemblyMonitorServiceStatus.ModelMonitorService.RequiredManHours
									Session("mAssemblyMonitorServiceStatus") = mAssemblyMonitorServiceStatus
									Session("mMachine") = mMachine
									Session("mAssemblyStatus") = mAssemblyStatus
									mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorServiceStatus.ID)
									Session("mBoardInfo") = mBoardInfo
									Session("mAssemblyInfo") = ""
									Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook

									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mAssemblyMonitorServiceStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mAssemblyMonitorServiceStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mAssemblyMonitorServiceStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mAssemblyMonitorServiceStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID

									Dim strError As String = ""

									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees

											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mAssemblyMonitorServiceStatus.DoneOn)

											If message = "" Then
												mAssemblyMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If

									'*End***************************************************
									If SaveAssemblyMonitorServiceStatus(mAssemblyMonitorServiceStatus, mSelectDueJobsForWO.Item(index)) = True Then

										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If

										Dim mMaintenanceOnDetail As String = Replace(mSelectDueJobsForWO.Item(index).DataType, "<BR>", "  ").ToString + "Description : " + mSelectDueJobsForWO.Item(index).Description + " ATA Chapter : " + mSelectDueJobsForWO.Item(index).ATAChapter + IIf(mSelectDueJobsForWO.Item(index).Number <> "", " Directive No. : " + mSelectDueJobsForWO.Item(index).Number, "")

										If AppSettings("LinkMaintenance") = "True" Then


											LinkMaintenance(mAssemblyMonitorServiceStatus.ModelMonitorServiceID, mMachine, mMaintenanceOnDetail, mnWO.WONumber, mAssemblyMonitorServiceStatus.AssemblyID, "Assembly Servicing", mMachineMaintenanceForAssemblyService, txtAsOnDate.Text.ToString, mSelectDueJobsForWO.Item(index).DoneRemark, mSelectDueJobsForWO.Item(index).LicenseNo, mSelectDueJobsForWO.Item(index).DoneByID.ToString, mSelectDueJobsForWO.Item(index).AllLicenceNosWithEmpName)
										End If

									End If

									Session("MaintenanceActivityTypeID") = 5

								End If

							Case "Inspection" 'Inspection

								Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
								Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mHourType, mSelectDueJobsForWO.Item(index).IsSpareComponent)

								If mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4 And mPrevAssemblyMonitorInspStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, New Guid(LogId), mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mHourType)
									Else
										mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, Guid.Empty, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mHourType)
									End If

									Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
									Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
									Session("From") = 0 'New record
									mAssemblyMonitorInspStatus.RequiredManHours = mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours
									Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
									Session("mMachine") = mMachine
									Session("mAssemblyStatus") = mAssemblyStatus
									mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorInspStatus.ID)
									Session("mBoardInfo") = mBoardInfo
									Session("mAssemblyInfo") = ""
									Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook

									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mAssemblyMonitorInspStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mAssemblyMonitorInspStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mAssemblyMonitorInspStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mAssemblyMonitorInspStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID

									Dim strError As String = ""

									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees

											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mAssemblyMonitorInspStatus.DoneOn)

											If message = "" Then
												mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If
									''End

									If SaveAssemblyMonitorInspStatus(mAssemblyMonitorInspStatus, mSelectDueJobsForWO.Item(index)) = True Then

										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If

										'Added by Saylee on 8-Sep-2020
										Dim mMaintenanceOnDetail As String = Replace(mSelectDueJobsForWO.Item(index).DataType, "<BR>", "  ").ToString + "Description : " + mSelectDueJobsForWO.Item(index).Description + " ATA Chapter : " + mSelectDueJobsForWO.Item(index).ATAChapter + IIf(mSelectDueJobsForWO.Item(index).Number <> "", " Directive No. : " + mSelectDueJobsForWO.Item(index).Number, "")

										LinkMaintenance(mAssemblyMonitorInspStatus.ModelMonitorInspID, mMachine, mMaintenanceOnDetail, mnWO.WONumber, mAssemblyMonitorInspStatus.AssemblyID, "Assembly Servicing", mMachineMaintenanceForAssemblyInsp, txtAsOnDate.Text.ToString, mSelectDueJobsForWO.Item(index).DoneRemark, mSelectDueJobsForWO.Item(index).LicenseNo, mSelectDueJobsForWO.Item(index).DoneByID.ToString, mSelectDueJobsForWO.Item(index).AllLicenceNosWithEmpName)
										'*********************
									End If

									Session("MaintenanceActivityTypeID") = 6

								End If

							Case "Modification" 'Modification

								Dim mAssemblyMonitorModStatus As AssemblyMonitorModStatus
								Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mHourType)

								If mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And mPrevAssemblyMonitorModStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4 And mPrevAssemblyMonitorModStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, New Guid(LogId), mPrevAssemblyMonitorModStatus.DoneOn.ToString, mHourType)
									Else
										mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, mSelectDueJobsForWO(index).ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, Guid.Empty, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mHourType)
									End If

									Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
									Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
									Session("From") = 0 'New record
									mAssemblyMonitorModStatus.RequiredManHours = mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours
									Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
									Session("mMachine") = mMachine
									Session("mAssemblyStatus") = mAssemblyStatus
									mBoardInfo = AircraftInformationBoard.BoardInfo.GetBoardInfo(mPrevAssemblyMonitorModStatus.ID)
									Session("mBoardInfo") = mBoardInfo
									Session("mAssemblyInfo") = ""
									Session("mAssemblyInfo") = mSelectDueJobsForWO.Item(index).LogBook

									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mAssemblyMonitorModStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mAssemblyMonitorModStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mAssemblyMonitorModStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mAssemblyMonitorModStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID
									mAssemblyMonitorModStatus.MethodOfCompliance = MethodOfCompliance

									Dim strError As String = ""
									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees
											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mAssemblyMonitorModStatus.DoneOn)

											If message = "" Then
												mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If
									''End

									If SaveAssemblyMonitorModStatus(mAssemblyMonitorModStatus, mSelectDueJobsForWO.Item(index)) = True Then
										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If
										'Added by Saylee on 8-Sep-2020
										Dim mMaintenanceOnDetail As String = Replace(mSelectDueJobsForWO.Item(index).DataType, "<BR>", "  ").ToString + "Description : " + mSelectDueJobsForWO.Item(index).Description + " ATA Chapter : " + mSelectDueJobsForWO.Item(index).ATAChapter + IIf(mSelectDueJobsForWO.Item(index).Number <> "", " Directive No. : " + mSelectDueJobsForWO.Item(index).Number, "")

										LinkMaintenance(mAssemblyMonitorModStatus.ModelMonitorModID, mMachine, mMaintenanceOnDetail, mnWO.WONumber, mAssemblyMonitorModStatus.AssemblyID, "Assembly Servicing", mMachineMaintenanceForAssemblyMod, txtAsOnDate.Text.ToString, mSelectDueJobsForWO.Item(index).DoneRemark, mSelectDueJobsForWO.Item(index).LicenseNo, mSelectDueJobsForWO.Item(index).DoneByID.ToString, mSelectDueJobsForWO.Item(index).AllLicenceNosWithEmpName)
										'*********************
									End If

									Session("MaintenanceActivityTypeID") = 7

								End If

						End Select

					ElseIf mSelectDueJobsForWO(index).OnAssemblyOrComponent = "Component" Then

						Select Case mSelectDueJobsForWO(index).DataType
							Case "Servicing"

								Dim mCompMonitorServiceStatus As CompMonitorServiceStatus
								Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(index).IsSpareComponent)

								If mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 And mPrevCompMonitorServiceStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4 And mPrevCompMonitorServiceStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, New Guid(LogId), mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
									Else
										mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorServiceStatus.PartMonitorService.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
									End If

									Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
									Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
									Session("From") = 0 'NewRecord

									Dim mCompStatus As CompStatus
									If mSelectDueJobsForWO.Item(index).IsSpareComponent = False Then
										mCompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
									Else
										mCompStatus = CompStatus.GetSpareCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, IsForSpareComp:=True)
									End If

									Session("mMachine") = mMachine
									Session("mCompStatus") = mCompStatus
									Session("mAssemblyStatus") = mAssemblyStatus
									mCompMonitorServiceStatus.RequiredManHours = mCompMonitorServiceStatus.PartMonitorService.RequiredManHours
									Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
									Session("mCompInfo") = ""
									Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook

									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mCompMonitorServiceStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mCompMonitorServiceStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mCompMonitorServiceStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mCompMonitorServiceStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID

									Dim strError As String = ""

									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees

											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mCompMonitorServiceStatus.DoneOn)
											If message = "" Then
												mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If
									''End

									If SaveCompMonitorServiceStatus(mCompMonitorServiceStatus, mSelectDueJobsForWO.Item(index)) = True Then
										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If
									End If

									Session("MaintenanceActivityTypeID") = 8

									'Added by vikrant on 06-Sep-2019 for ALL06092019
									If mnWO.WOJobs.Count = 1 And (mnWO.TransTypeID <> Trans.SpareComponentWO) Then

										If (mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 2 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 5 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV")) Then
											CallCommonCodeAfterComplaince()
											RemoveComp(mCompMonitorServiceStatus.DoneOn.ToString, mCompStatus, mMachine)
										Else
											'Added by Saylee on 21-Aug-2020, All21082020
											Session("mDoneOnCompliance") = mCompMonitorServiceStatus.DoneOn.ToString
											MSGBoxCtrl.Show("Alert!", "Do you want to remove Component?", "Click Yes to Remove Component or click No to just Comply the Service.", MsgBoxStyle.YesNo, "RemoveComp")

										End If

									End If
									'End

								End If
							Case "Inspection" 'Inspection

								Dim mCompMonitorInspStatus As CompMonitorInspStatus
								Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(index).IsSpareComponent)

								If mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 And mPrevCompMonitorInspStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4 And mPrevCompMonitorInspStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, New Guid(LogId), mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType)
									Else
										mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorInspStatus.PartMonitorInsp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType)
									End If

									Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
									Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
									Session("From") = 0 'NewRecord

									Dim mCompStatus As CompStatus
									If mSelectDueJobsForWO.Item(index).IsSpareComponent = False Then
										mCompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
									Else
										mCompStatus = CompStatus.GetSpareCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, IsForSpareComp:=True)
									End If
									Session("mMachine") = mMachine
									Session("mCompStatus") = mCompStatus
									Session("mAssemblyStatus") = mAssemblyStatus
									mCompMonitorInspStatus.RequiredManHours = mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours
									Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
									Session("mCompInfo") = ""
									Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook
									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mCompMonitorInspStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mCompMonitorInspStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mCompMonitorInspStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mCompMonitorInspStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID

									Dim strError As String = ""

									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees

											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mCompMonitorInspStatus.DoneOn)

											If message = "" Then
												mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If
									''End
									If SaveCompMonitorInspStatus(mCompMonitorInspStatus, mSelectDueJobsForWO.Item(index)) = True Then

										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If

									End If

									Session("MaintenanceActivityTypeID") = 9
									If mnWO.WOJobs.Count = 1 And (mnWO.TransTypeID <> Trans.SpareComponentWO) Then
										'Added by Saylee on 21-Aug-2020, All21082020
										Session("mDoneOnCompliance") = mCompMonitorInspStatus.DoneOn.ToString
										MSGBoxCtrl.Show("Alert!", "Do you want to remove Component?", "Click Yes to Remove Component or click No to just Comply the Inspection.", MsgBoxStyle.YesNo, "RemoveComp")
									End If

								End If

							Case "Modification" 'Modification

								Dim mCompMonitorModStatus As CompMonitorModStatus
								Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(mSelectDueJobsForWO.Item(index).ID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, mSelectDueJobsForWO.Item(index).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(index).IsSpareComponent)

								If mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 1 And mPrevCompMonitorModStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.OneTimeMonitoring, MSGBox.Message_text.OneTimeMonitoring, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								ElseIf mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 4 And mPrevCompMonitorModStatus.IsCompleted Then
									MSGBoxCtrl.Show(MSGBox.Message_title.Expiry, MSGBox.Message_text.Expiry, "", MsgBoxStyle.OkOnly, "")
									Exit Sub
								Else

									If CType(Session("FromLog"), Boolean) = True Then
										mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, New Guid(LogId), mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType)
									Else
										mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, AsonDate, mPrevCompMonitorModStatus.PartMonitorMod.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType)
									End If

									Session("mCompMonitorModStatus") = mCompMonitorModStatus
									Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
									Session("From") = 0 'NewRecord

									Dim mCompStatus As CompStatus
									If mSelectDueJobsForWO.Item(index).IsSpareComponent = False Then
										mCompStatus = CompStatus.GetCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, mSelectDueJobsForWO.Item(index).AssemblyStatusID, AsonDate)
									Else
										mCompStatus = CompStatus.GetSpareCompStatus(mSelectDueJobsForWO.Item(index).CompStatusID, IsForSpareComp:=True)
									End If

									Session("mMachine") = mMachine
									Session("mCompStatus") = mCompStatus
									Session("mAssemblyStatus") = mAssemblyStatus
									mCompMonitorModStatus.RequiredManHours = mCompMonitorModStatus.PartMonitorMod.RequiredManHours
									Session("mCompMonitorModStatus") = mCompMonitorModStatus
									Session("mCompInfo") = ""
									Session("mCompInfo") = mSelectDueJobsForWO.Item(index).LogBook
									'Added By Vikrant On 21-Jun-2016 For ALL21062016
									mCompMonitorModStatus.LicenseNo = mSelectDueJobsForWO.Item(index).LicenseNo
									mCompMonitorModStatus.Place = mSelectDueJobsForWO.Item(index).Place
									mCompMonitorModStatus.RequiredManHours = mSelectDueJobsForWO.Item(index).RequiredManHours
									mCompMonitorModStatus.DoneByID = mSelectDueJobsForWO.Item(index).DoneByID
									mCompMonitorModStatus.MethodOfCompliance = MethodOfCompliance

									Dim strError As String = ""

									''MLNo****************************************************
									If mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees.Count > 0 Then

										For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In mSelectDueJobsForWO.Item(index).MaintenanceDoneByEmployees
											Dim message As String = ""
											message = IsEmployeeWorking(mMaintenanceDoneByEmployee.EmployeeID, mCompMonitorModStatus.DoneOn)

											If message = "" Then
												mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
											Else
												strError = strError + message
												Exit Select
											End If

										Next

									End If
									''End

									If SaveCompMonitorModStatus(mCompMonitorModStatus, mSelectDueJobsForWO.Item(index)) = True Then

										If mnWO.WOJobs.Contains(mSelectDueJobsForWO.Item(index).WOJobID) Then
											Dim mWOJob As nWOJob = nWOJob.GetWOJob(mSelectDueJobsForWO.Item(index).WOJobID)
											mWOJob.IsComplied = True
											mWOJob.Save()
										End If

									End If

									Session("MaintenanceActivityTypeID") = 10
									If mnWO.WOJobs.Count = 1 And (mnWO.TransTypeID <> Trans.SpareComponentWO) Then
										'Added by Saylee on 21-Aug-2020, All21082020
										Session("mDoneOnCompliance") = mCompMonitorModStatus.DoneOn.ToString
										MSGBoxCtrl.Show("Alert!", "Do you want to remove Component?", "Click Yes to Remove Component or click No to just Comply the Modification.", MsgBoxStyle.YesNo, "RemoveComp")
									End If

								End If

						End Select

					End If

				End If

			Next

			CallCommonCodeAfterComplaince()
			Session.Remove("mMultiComplianceList")

		Else

			MSGBoxCtrl.Show(MSGBox.Message_title.SelectAtleastOne,
							MSGBox.Message_text.SelectAtleastOne,
							"Please select atleast one item to Comply",
							MsgBoxStyle.OkOnly,
							"")

			Exit Sub

		End If

	End Sub

	Private Sub rdbCompletedJobs_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCompletedJobs.CheckedChanged, rdbALLJobs.CheckedChanged
		Dim ShowAllJobs As Boolean = False
		If rdbCompletedJobs.Checked = True Then
			ShowAllJobs = False
		ElseIf rdbALLJobs.Checked = True Then
			ShowAllJobs = True
		End If

		SetIds()
		'mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mnWO.MachineID.ToString, 0, mnWO, ShowAllJobs)
		mSelectDueJobsForWO = SelectDueJobsFornWO.GetSelectDueJobsFor_nWO(txtAsOnDate.Text, mDueLimits, mnWO.MachineID.ToString, 0, mnWO, AssemblyMonitorInspStatusIDs:=AssemblyMonitorInspStatusIDs.ToString, AssemblyMonitorModStatusIDs:=AssemblyMonitorModStatusIDs.ToString, CompMonitorInspStatusIDs:=CompMonitorInspStatusIDs.ToString, CompMonitorModStatusIDs:=CompMonitorModStatusIDs.ToString, CompMonitorServiceStatusIDs:=CompMonitorServiceStatusIDs.ToString, TypeID:=TypeID, AssemblyMonitorServiceStatusIDs:=AssemblyMonitorServiceStatusIDs.ToString, IsForSpareAssembly:=IIf(mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO, True, False))

		dgDueJob.DataSource = mSelectDueJobsForWO
		Session("mSelectDueJobsForWO") = mSelectDueJobsForWO
		dgDueJob.DataBind()

		Dim WOstr As String = ""
		If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
			WOstr = "Engineering Order"
		Else
			WOstr = "Work Order"
		End If

		If mSelectDueJobsForWO.Count > 0 Then
			btnSave.Enabled = True
			If mSelectDueJobsForWO.Count > 10 Then btnSaveTop.Visible = True
			If mSelectDueJobsForWO.Count > 10 Then btnCloseTop.Visible = True
			lblNote.Text = ""
		Else
			btnSave.Enabled = False
			lblNote.Text = "*Note : There are no Due jobs in this " & WOstr & " which may have been already complied by using Maintenance menu option."
		End If
		lblResult.Text = "List of Due Jobs as per selected criteria : " & mSelectDueJobsForWO.Count & " Record(s) found."
		txtWOLabel.DataBind()

		upnlResult.Update()
		upnlNote.Update()
		upnlDueJob.Update()
	End Sub

	Private Sub dgDueJob_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgDueJob.RowCommand
		Select Case e.CommandName
			Case "EmployeeLicence"

				Dim mID As Guid

				mID = mSelectDueJobsForWO(CInt(e.CommandArgument)).ID
				Session("mMaintenanceID") = mID
				Session("MaintenanceDoneOnDate") = txtAsOnDate.Text.ToString
				mMaintenanceDoneByEmployees = mSelectDueJobsForWO(mID).MaintenanceDoneByEmployees
				Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
				Dim MaintenanceActivityTypeID As Integer = mSelectDueJobsForWO(mID).MaintenanceActivityTypeID



				If mMaintenanceDoneByEmployees.Count = 1 Then
					Dim txtActualManHrs As TextBox
					'txtActualManHrs = CType(Me.dgDueJob.Rows(CInt(rowIndex)).FindControl("txtActualManHrs"), TextBox)
					txtActualManHrs = CType(Me.dgDueJob.Rows(CInt(e.CommandArgument)).FindControl("txtActualManHrs"), TextBox)
					mMaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHrs.Text
					Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
				End If
				ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo('" + MaintenanceActivityTypeID.ToString + "');", True)

		End Select
	End Sub

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, btnCloseTop.Click
		RemoveSession()
		Response.Redirect(Request.QueryString("BackPage1") & "?BackPage=" & Request.QueryString("BackPage"))
	End Sub

	Protected Sub LicenceNoChanged(sender As Object, e As EventArgs)

		Try

			If Not Request.Form("chkSelect") IsNot Nothing Then
				AddJobs()
			End If

			Dim txtLicenceNo As TextBox
			Dim txtActualManHrs As TextBox
			Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent

			txtLicenceNo = CType(currentRow.FindControl("txtLicenceNo"), TextBox)
			txtActualManHrs = CType(currentRow.FindControl("txtActualManHrs"), TextBox)

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

				If mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees.Count > 0 Then

					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHrs.Text
					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees(0).EmployeeName = EmpName

				Else
					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees.Add(mSelectDueJobsForWO(currentRow.RowIndex).ID, mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceActivityTypeID, DoneByID, LicenseNo, txtActualManHrs.Text, EmpName)
				End If

			Else

				If mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees.Count > 0 Then
					mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees.RemoveAt(0)
				End If

			End If

			txtActualManHrs.Text = mSelectDueJobsForWO(currentRow.RowIndex).TotalReqManHrs1
			txtLicenceNo.DataBind()
			txtActualManHrs.DataBind()
			txtActualManHrs.Enabled = mSelectDueJobsForWO(currentRow.RowIndex).MaintenanceDoneByEmployees.Count <= 1

			Session("mSelectDueJobsForWO") = mSelectDueJobsForWO

			BindLicenceNo()
			SetLicenceCount()

			upnlDueJob.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function AddItemsToList() As Integer
		Dim count As Integer = 0
		Dim chkBox As CheckBox
		For i As Integer = 0 To dgDueJob.Rows.Count - 1
			chkBox = CType(dgDueJob.Rows.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
			If chkBox.Checked Then
				count += 1
			End If
		Next
		Return count
	End Function

	Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As EventArgs) Handles hdnBtnMaintDoneBy.Click

		If mMaintenanceDoneByEmployees.Count > 0 Then
			Dim MaintenanceID As Guid
			MaintenanceID = mMaintenanceDoneByEmployees(0).MaintenanceID
			For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
				Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
				If Not mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Contains(ID) Then
					mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
				ElseIf mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Contains(ID) Then
					mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
					mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
					mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
				End If
			Next

			For j As Integer = 0 To mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Count - 1
				If Not mMaintenanceDoneByEmployees.Contains(mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(j).ID) Then
					mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Remove(mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(j).ID, "")
				End If
			Next


			'If mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Count > 0 Then
			'    txtLicenceNo.Text = mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(0).LicenceNo + " [" + mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees(0).EmployeeName + "]"
			'Else
			'    txtLicenceNo.Text = String.Empty
			'End If
			'If mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Count > 1 Then
			'    lblLicenceCount.Text = "and " + (mSelectDueJobsForWO(MaintenanceID).MaintenanceDoneByEmployees.Count - 1).ToString + " more"
			'End If
			'lblLicenceCount.DataBind()
			Session("mSelectDueJobsForWO") = mSelectDueJobsForWO


		End If
		BindLicenceNo()
		SetLicenceCount()
		upnlDueJob.Update()
	End Sub

	'MLNo
	Private Sub BindLicenceNo(Optional ShowDefaultValuesOnPageLoad As Boolean = False)
		Dim item As GridViewRow
		Dim Recordno, PageItems As Integer

		Dim txtLicenceNo, txtActualManHrs As TextBox

		PageItems = dgDueJob.Rows.Count - 1
		' Set Selected DoneRemark value  
		For i As Integer = 0 To PageItems
			Recordno = i + dgDueJob.PageSize * dgDueJob.PageIndex
			item = dgDueJob.Rows(i)
			'   If mSelectDueJobsForWO(Recordno).IsSelected = True Then
			txtLicenceNo = CType(item.FindControl("txtLicenceNo"), TextBox)
			txtActualManHrs = CType(item.FindControl("txtActualManHrs"), TextBox)
			If mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees.Count > 0 Then
				txtLicenceNo.Text = mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees(0).LicenceNo + " [" + mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees(0).EmployeeName + "]"

				If ShowDefaultValuesOnPageLoad Then
					txtActualManHrs.Text = mSelectDueJobsForWO(Recordno).ActualManHours
				Else
					txtActualManHrs.Text = mSelectDueJobsForWO(Recordno).TotalReqManHrs1
				End If

				txtLicenceNo.DataBind()
				txtActualManHrs.DataBind()
				'  End If

				txtActualManHrs.Enabled = mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees.Count <= 1
			Else
				txtLicenceNo.Text = String.Empty
				'txtActualManHrs.Text = String.Empty
				txtActualManHrs.Text = mSelectDueJobsForWO(Recordno).ActualManHours
				txtLicenceNo.DataBind()
				txtActualManHrs.DataBind()
				txtActualManHrs.Enabled = True
			End If
		Next

		upnlDueJob.Update()
	End Sub

	Public Sub SetLicenceCount()
		Dim item As GridViewRow
		Dim Recordno, PageItems As Integer

		Dim lblLicenceCount As Label

		PageItems = dgDueJob.Rows.Count - 1
		' Set Selected DoneRemark value  
		For i As Integer = 0 To PageItems
			Recordno = i + dgDueJob.PageSize * dgDueJob.PageIndex
			item = dgDueJob.Rows(i)
			lblLicenceCount = CType(item.FindControl("lblLicenceCount"), Label)
			If mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees.Count > 0 Then
				lblLicenceCount.ToolTip = mSelectDueJobsForWO(Recordno).AllLicenceNos
				lblLicenceCount.Text = "and " + (mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees.Count - 1).ToString + " more"
				lblLicenceCount.DataBind()
			End If
			lblLicenceCount.Visible = mSelectDueJobsForWO(Recordno).MaintenanceDoneByEmployees.Count > 1
		Next


	End Sub
	'End

#End Region

#Region " Service Methods "

	'Shifted this Logic to wfAutoPilotPlace to keep the Autocomplete Dropdowm functionality consitent.

#End Region

#Region " Link Maintenance "

#Region " Variable Declaration "

	Public mLinkMaintenanceList As LinkMaintenanceList
	Public mLinkMaintenance As LinkMaintenance
	Public mMultiComplianceLinkList As New MultiComplianceList
	Public mAssemblyMonitorServiceStatusForLM As AssemblyMonitorServiceStatus
	Public mAssemblyMonitorInspStatusForLM As AssemblyMonitorInspStatus
	Public mAssemblyMonitorModStatusForLM As AssemblyMonitorModStatus
	Public mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
	Public PeriodValues(,) As String
	Dim message As String = ""
	Dim mDetail As String = ""

#End Region

	Private Sub LinkMaintenance(MaintenanceActivityID As Guid, mMachine As Machine, Detail As String, DoneWONo As String, AssemblyId As Guid, MaintenanceActivity As String, mMachineMaintenance As MachineMaintenance, DoneOnDate As String, DoneRemark As String, Optional LicenceNo As String = "", Optional EmployeeID As String = "", Optional EmployeeName As String = "")
		If AppSettings("LinkMaintenance") = "True" Then
			Dim mMultiComplianceList As New MultiComplianceList
			mMultiComplianceList = Session("mMultiComplianceList")

			If mMultiComplianceList Is Nothing Then Exit Sub

			If mMultiComplianceList.Count > 0 Then

				''ShowLinkedMaintenaceActivity(mMachine, DoneOnDate, AssemblyId)

				Dim Result As Boolean
				Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents


				LicenseNo = IIf(Session("LicenseNo") Is Nothing, "", Session("LicenseNo"))
				If Session("EmployeeID") IsNot Nothing Then EmployeeID = IIf(Session("EmployeeID") Is Nothing, "", Session("EmployeeID").ToString)
				EmpName = IIf(Session("EmpName") Is Nothing, "", Session("EmpName"))

				EmployeeID = EmployeeID.ToString.TrimEnd(",")
				LicenseNo = LicenseNo.ToString.TrimEnd(",")

				EmpName = EmpName.ToString.TrimEnd(",")
				'Save Link Activities

				'Added by Saylee on 8-Feb-2021 for ALL08022021
				SetLinkGridObject()

				Dim checkString = Request.Form("chkSelect")
				If checkString IsNot Nothing Then
					Dim values = checkString.Split(","c)
					For Each value As String In values
						For i As Integer = 0 To mMultiComplianceList.Count - 1
							If mMultiComplianceList(i).ID.Equals(New Guid(value)) Then
								mMultiComplianceList(i).IsSelect = True
								Exit For
							Else
								'mMultiComplianceList(i).IsSelect = False
							End If
						Next
					Next
				End If
				'*************

				LinkMaintenanceEvents.AssemblyLogInfo = MaintenanceActivity & ": " & Detail 'setting Mark Log Detail ...
				Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList,
																			 DoneWONo,
																			 DoneOnDate,
																			 mMachineMaintenance.LogID,
																			 mMachine.HourType,
																			 mMachine.ID,
																			 AssemblyId,
																			 PeriodValues,
																			 DoneRemark,
																			 LicenceNo,
																			 EmployeeID,
																			 EmployeeName,
																			 isFromMulticomplianceForm:=True,
																			 isFromWOComplaiance:=True)
				If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
					Dim title As String = "Link Maintenance Alert !"
					Dim message As String = LinkMaintenanceEvents.ErrorStr
					' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
				End If
				Session.Remove("mMultiComplianceLinkList")
				mMultiComplianceLinkList = Nothing
			End If

		End If
	End Sub

	'Private Sub ShowLinkedMaintenaceActivity(mMachine As Machine, DoneOnDate As String, AssemblyID As Guid)

	'    mMultiComplianceLinkList = New MultiComplianceList

	'    Dim mPeriodUnitName As String
	'    Dim mFrequencyValue As String
	'    Dim mDoneOnValue As String
	'    Dim mCurrentValue As String
	'    Dim mDueOnValue As String
	'    Dim mElapsedValue As String
	'    Dim mRemainingValue As String
	'    Dim mDoneOn As String
	'    Dim mExtensionValue As String

	'    Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()

	'    For i As Integer = 0 To mLinkMaintenanceList.Count - 1

	'        If Not i = 0 Then

	'            mPeriodUnitName = String.Empty
	'            mFrequencyValue = String.Empty
	'            mDoneOnValue = String.Empty
	'            mCurrentValue = String.Empty
	'            mDueOnValue = String.Empty
	'            mElapsedValue = String.Empty
	'            mRemainingValue = String.Empty
	'            mDoneOn = String.Empty
	'            mExtensionValue = String.Empty
	'        End If

	'        Select Case mLinkMaintenanceList(i).LinkedMaintenanceTypeID

	'            Case 1 'Assembly Service

	'                mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(New Guid(MachineName), mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
	'                If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
	'                    Exit Select
	'                End If
	'                Dim mPrevAssemblyMonitorSeviceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

	'                mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

	'                Dim mAssemblyInfo As String = mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

	'                For j As Integer = 0 To mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods.Count - 1

	'                    If mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodID = 2 Then

	'                        Dim PeriodCode As String = mPeriodUnitList(3, "").Code

	'                        If j = 0 Then

	'                            mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If

	'                    Else

	'                        Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code

	'                        If j = 0 Then

	'                            mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If


	'                    End If
	'                Next
	'                mMultiComplianceLinkList.Add(mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, True, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
	'                mLinkMaintenanceMonitorStatus = Nothing

	'            Case 2 'Assembly Inspection

	'                mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
	'                If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
	'                    Exit Select
	'                End If
	'                Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

	'                mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

	'                Dim mAssemblyInfo As String = mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

	'                For j As Integer = 0 To mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods.Count - 1

	'                    If mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then

	'                        Dim PeriodCode As String = mPeriodUnitList(3, "").Code

	'                        If j = 0 Then
	'                            mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If

	'                    Else
	'                        Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code

	'                        If j = 0 Then
	'                            mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If

	'                    End If

	'                Next
	'                mMultiComplianceLinkList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, True, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
	'                mLinkMaintenanceMonitorStatus = Nothing

	'            Case 3 'Assembly Directive
	'                mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, AssemblyID)
	'                If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
	'                    Exit Select
	'                End If
	'                Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

	'                mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, DoneOnDate, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

	'                Dim mAssemblyInfo As String = mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

	'                For j As Integer = 0 To mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods.Count - 1

	'                    If mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then

	'                        Dim PeriodCode As String = mPeriodUnitList(3, "").Code

	'                        If j = 0 Then
	'                            mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If

	'                    Else
	'                        Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code

	'                        If j = 0 Then
	'                            mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        Else
	'                            mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
	'                            mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
	'                            mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
	'                            mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
	'                            mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
	'                            mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
	'                            mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
	'                            'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
	'                            mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
	'                        End If
	'                    End If


	'                Next
	'                mMultiComplianceLinkList.Add(mAssemblyMonitorModStatusForLM.ID, MaintenanceActivityTypes.AssemblyDirective, True, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Reference, mAssemblyMonitorModStatusForLM.ModelMonitorMod.MonitorTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelMonitorModTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Description, mAssemblyMonitorModStatusForLM.DoneOn.ToString, mAssemblyMonitorModStatusForLM.DoneWONo, mAssemblyMonitorModStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ATAChapter, , , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
	'                mLinkMaintenanceMonitorStatus = Nothing
	'        End Select
	'    Next
	'    Session("mMultiComplianceLinkList") = mMultiComplianceLinkList
	'End Sub

	Public Sub SetLinkGridObject()
		Dim j As Int32

		ReDim PeriodValues(dgDoneOnValue.Rows.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)

		For j = 0 To Me.dgDoneOnValue.Rows.Count - 1

			PeriodValues(j, 0) = Me.dgDoneOnValue.Rows(j).Cells(2).Text 'Me.dgDoneOnValue.Rows(j).Cells(0).Text 'To Check same Period
			PeriodValues(j, 1) = Me.dgDoneOnValue.Rows(j).Cells(1).Text 'Period Value 
		Next j

	End Sub

	Private Sub GV_DueJob_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgDueJob.RowDataBound

		Try

			If e.Row.RowType <> DataControlRowType.DataRow Then
				Return
			End If

			If (e.Row.RowType = DataControlRowType.DataRow) Then

				Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
				mSelectDueJobsForWO = Session("mSelectDueJobsForWO")
				Dim mHourType As Integer = 1
				Dim mMachine As Machine

				If Not (mnWO.TransTypeID = Trans.SpareAssemblyWO Or mnWO.TransTypeID = Trans.SpareComponentWO) Then
					mMachine = Machine.GetMachine(mSelectDueJobsForWO(ID).MachineID)
					mHourType = mMachine.HourType
				End If

				Dim grdLinkActivity As GridView = DirectCast(e.Row.FindControl("grdLinkActivity"), GridView)

				If mSelectDueJobsForWO(ID).OnAssemblyOrComponent = "Assembly" Then

					Select Case mSelectDueJobsForWO(ID).DataType
						Case "Servicing" 'Service
							Dim mPrevAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mHourType)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorServiceStatus.ModelMonitorServiceID.ToString)
						Case "Inspection"   '6. Assembly Inspection
							Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mHourType)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorInspStatus.ModelMonitorInspID.ToString)
						Case "Modification"    '7. Assembly Directive
							Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mHourType)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorModStatus.ModelMonitorModID.ToString)

					End Select

				ElseIf mSelectDueJobsForWO(ID).OnAssemblyOrComponent = "Component" Then

					Select Case mSelectDueJobsForWO(ID).DataType
						Case "Servicing"  '8. Comp Service
							Dim mPrevCompMonitorServiceStatus As CompMonitorServiceStatus = CompMonitorServiceStatus.GetCompMonitorServiceStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(ID).IsSpareComponent)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorServiceStatus.PartMonitorServiceID.ToString)
						Case "Inspection"   '9. Component Inspection
							Dim mPrevCompMonitorInspStatus As CompMonitorInspStatus = CompMonitorInspStatus.GetCompMonitorInspStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(ID).IsSpareComponent)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorInspStatus.PartMonitorInspID.ToString)
						Case "Modification"    '10. Component Directive
							Dim mPrevCompMonitorModStatus As CompMonitorModStatus = CompMonitorModStatus.GetCompMonitorModStatus(ID, mSelectDueJobsForWO.Item(ID).AssemblyStatusID, mSelectDueJobsForWO.Item(ID).CompStatusID, mHourType, IsForSpareComp:=mSelectDueJobsForWO.Item(ID).IsSpareComponent)
							mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevCompMonitorModStatus.PartMonitorModID.ToString)
					End Select

				End If

				If mLinkMaintenanceList.Count > 0 Then

					Session("mLinkMaintenanceList") = mLinkMaintenanceList
					grdLinkActivity.DataSource = ShowLinkedMaintenaceActivity(mLinkMaintenanceList, mSelectDueJobsForWO(ID), mHourType)
					grdLinkActivity.DataBind()
					e.Row.Cells(1).BackColor = Color.Yellow

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#Region " View Link Activity "

	Public Function ShowLinkedMaintenaceActivity(mLinkMaintenanceList As LinkMaintenanceList, mSelectDueJobFornWO As SelectDueJobFornWO, mHourType As Integer) As MultiComplianceList
		Dim mMultiComplianceList As New MultiComplianceList
		Dim mPeriodUnitName As String
		Dim mFrequencyValue As String
		Dim mDoneOnValue As String
		Dim mCurrentValue As String
		Dim mDueOnValue As String
		Dim mElapsedValue As String
		Dim mRemainingValue As String
		Dim mDoneOn As String
		Dim mExtensionValue As String

		Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()




		For i As Integer = 0 To mLinkMaintenanceList.Count - 1

			If Not i = 0 Then

				mPeriodUnitName = String.Empty
				mFrequencyValue = String.Empty
				mDoneOnValue = String.Empty
				mCurrentValue = String.Empty
				mDueOnValue = String.Empty
				mElapsedValue = String.Empty
				mRemainingValue = String.Empty
				mDoneOn = String.Empty
				mExtensionValue = String.Empty
			End If

			Select Case mLinkMaintenanceList(i).LinkedMaintenanceTypeID

				Case 1 'Assembly Service

					mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mSelectDueJobFornWO.MachineID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mSelectDueJobFornWO.AssemblyID)
					If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
						Exit Select
					End If
					Dim mPrevAssemblyMonitorSeviceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mHourType)

					mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, AsonDate, Guid.Empty, mHourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

					Dim mAssemblyInfo As String = mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.Name ' & VbCrLf & mAssemblyMonitorServiceStatusForLm.

					For j As Integer = 0 To mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods.Count - 1

						If mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodID = 2 Then

							Dim PeriodCode As String = mPeriodUnitList(3, "").Code

							If j = 0 Then

								mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							End If

						Else

							Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code

							If j = 0 Then

								mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							End If


						End If
					Next
					mMultiComplianceList.Add(mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, IIf(Session("From") = 1, False, True), mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mSelectDueJobFornWO.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
					mLinkMaintenanceMonitorStatus = Nothing

				Case 2 'Assembly Inspection

					mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mSelectDueJobFornWO.MachineID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mSelectDueJobFornWO.AssemblyID)
					If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
						Exit Select
					End If
					Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mHourType)

					mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, AsonDate, Guid.Empty, mHourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

					Dim mAssemblyInfo As String = mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

					For j As Integer = 0 To mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods.Count - 1

						If mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then

							Dim PeriodCode As String = mPeriodUnitList(3, "").Code

							If j = 0 Then
								mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)

							End If

						Else
							Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code

							If j = 0 Then
								mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted\
								mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							End If

						End If

					Next
					mMultiComplianceList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, IIf(Session("From") = 1, False, True), mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mSelectDueJobFornWO.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
					mLinkMaintenanceMonitorStatus = Nothing

				Case 3 'Assembly Directive
					mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mSelectDueJobFornWO.MachineID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mSelectDueJobFornWO.AssemblyID)
					If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
						Exit Select
					End If
					Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mHourType)

					mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, AsonDate, Guid.Empty, mHourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

					Dim mAssemblyInfo As String = mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

					For j As Integer = 0 To mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods.Count - 1

						If mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then

							Dim PeriodCode As String = mPeriodUnitList(3, "").Code

							If j = 0 Then
								mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							End If

						Else
							Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code

							If j = 0 Then
								mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							Else
								mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
								mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
								mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
								mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
								mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
								mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
								mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
								'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
								mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
							End If
						End If


					Next
					mMultiComplianceList.Add(mAssemblyMonitorModStatusForLM.ID, MaintenanceActivityTypes.AssemblyDirective, IIf(Session("From") = 1, False, True), mAssemblyMonitorModStatusForLM.ModelMonitorMod.Reference, mAssemblyMonitorModStatusForLM.ModelMonitorMod.MonitorTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelMonitorModTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Description, mAssemblyMonitorModStatusForLM.DoneOn.ToString, mAssemblyMonitorModStatusForLM.DoneWONo, mAssemblyMonitorModStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mSelectDueJobFornWO.RegNo, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ATAChapter, , , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
					mLinkMaintenanceMonitorStatus = Nothing
			End Select
		Next


		' dgMultiComplianceList.DataSource = mMultiComplianceList
		Session("mMultiComplianceList") = mMultiComplianceList 'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
		lblResult.Text = "List of Linked Maintenance Activity : " & mMultiComplianceList.Count & " Record(s) found."

		Return mMultiComplianceList
	End Function

#End Region

#End Region

#Region " Checked Selection "

	'ALL04032019
	Public Function NumeroChequeInclus(numero As String) As String
		If (checkedIds.Contains(numero)) Then
			Return "checked"
		Else
			Return String.Empty
		End If
	End Function
	'End

#End Region

	'Sankalp 18-08-25
	Public Sub BindPlace()
		For Each row As GridViewRow In dgDueJob.Rows
			If row.RowType = DataControlRowType.DataRow Then
				Dim txtPlace As TextBox = CType(row.FindControl("txtPlace"), TextBox)
				If txtPlace IsNot Nothing Then
					txtPlace.Text = mnWO.WorkShopName
				End If
			End If
		Next
	End Sub

End Class
