'******************************************************
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'******************************************************


Imports System.Linq


Public Class DiscrepancyTroubleShoot
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	'Added on 29-05-2007 by Saylee

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declaration "

	Public LicenseNoListWithEmployee As LicenseNoListWithEmployee
	Public DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
	Public DiscrepancyTroubleShootList As DiscrepancyTroubleShootList
	Public MaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
	Public LogMaintenance As LogMaintenance
	Public EmployeeStatus As EmployeeStatus
	Public AssemblyList As AssemblyList
	Public Log As Log
	Public LogList As LogList

	Dim Flag As Int16
	Dim DoneByID As Guid = Guid.Empty
	Dim EventLogID As Guid
	Dim mLogDetail As String
	Dim LicenseNo As String = String.Empty
	Dim EmpName As String = String.Empty
	Shared UserNameForLicenceList As String
	Dim TransTypeID As Integer

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		LogMaintenance = Session("LogMaintenance")
		Log = CType(Session("mLog"), Log)
		AssemblyList = Session("mAssemblylist")
		LicenseNoListWithEmployee = Session("LicenseNoListWithEmployee")
		DiscrepancyTroubleShootList = Session("DiscrepancyTroubleShootList")
		MaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
		UserNameForLicenceList = Session("UserNameForLicenceList")
		DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
		LogList = Session("LogList")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList
		Session("LogMaintenance") = LogMaintenance
		Session("mLog") = Log
		Session("LicenseNoListWithEmployee") = LicenseNoListWithEmployee
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Sub RemoveSession()

		Session.Remove("Edit")
		Session.Remove("LogMaintenance")
		Session.Remove("mAssemblylist")
		Session.Remove("LogMaintenanceEdit")
		Session.Remove("OpenFromLMA")
		Session.Remove("mMaintenanceDoneByEmployees")
		Session.Remove("UserNameForLicenceList")
		Session.Remove("TransTypeID")

	End Sub

	Public Sub SetLicenceCount(LogMaintenance As LogMaintenance)

		Try

			If LogMaintenance.MaintenanceDoneByEmployees.Count > 1 Then
				lblLicenceCount.Text = " and " + (LogMaintenance.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
			End If

			lblLicenceCount.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub BindLicenceNo(LogMaintenance As LogMaintenance)

		Try

			If LogMaintenance.MaintenanceDoneByEmployees.Count > 0 Then
				txtLicenceNo.Text = LogMaintenance.MaintenanceDoneByEmployees(0).LicenceNo + " [" + LogMaintenance.MaintenanceDoneByEmployees(0).EmployeeName + "]"
			Else
				txtLicenceNo.Text = String.Empty
			End If

			lblLicenceCount.ToolTip = LogMaintenance.AllLicenceNosWithEmpName

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		If control.Enabled = False Or control.Visible = False Then Exit Sub

		Try

			Dim str As String
			str = "document.getElementById('" + control.ClientID + "').focus();"

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"focusscript",
												str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteRecord(Index As Integer)

		Try

			MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
							MSGBox.Message_Text.Delete,
							"",
							MsgBoxStyle.YesNo,
							"Delete")

			Log.LogMaintenances.CurrentIndex = Index
			Session("mLog") = Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub CustomValidation(s As Object, e As ServerValidateEventArgs)

		If Flag = 1 Then Exit Sub

		Try

			Dim custValidator As CustomValidator
			custValidator = CType(s, CustomValidator)

			Dim str As String = ""

			'Log
			'Log Maintenance Activity
			If txtMainActivity.Text.Length > 2000 And custValidator.ControlToValidate = "txtMainActivity" Then
				custValidator.ErrorMessage = "Activity  must not be greater than 2000 characters."
				e.IsValid = False
			ElseIf txtNCRNo.Text.Length > 50 And custValidator.ControlToValidate = "txtNCRNo" Then
				custValidator.ErrorMessage = "NRC/WO No. must not be greater than 50 characters."
				e.IsValid = False
			End If

			If str <> "" Then
				custValidator.ErrorMessage = str
				e.IsValid = False
			End If

			Flag = 1

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function CustomValidate() As Boolean

		Dim str As String = ""
		Try

			'Log
			If Not Log.IsValid Then

				For i As Integer = 0 To Log.GetBrokenRulesCollection.Count - 1
					str = str + Log.GetBrokenRulesCollection(i).Description + "<BR>"
				Next

			End If

			'Log Maintenances
			For i As Integer = 0 To Log.LogMaintenances.Count - 1

				If Not Log.LogMaintenances(i).IsValid Then

					For j As Integer = 0 To Log.LogMaintenances(i).GetBrokenRulesCollection.Count - 1
						str = str + Log.LogMaintenances.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
					Next

				End If

			Next

			If str <> "" Then

				cvMainActivityList.ErrorMessage = str
				cvMainActivityList.IsValid = False

				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function CustomValidate_Description() As Boolean

		Dim strMSG As String = ""
		Try

			If Len(Trim(txtMainActivity.Text)) = 0 Then strMSG = "Description Required" + "<Br>"

			If strMSG.Trim <> "" Then

				Me.cvControlValidator.ErrorMessage = strMSG
				Me.cvControlValidator.IsValid = False

				Return False

			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		Dim msgCount As Integer = 0
		MsgBoxResult = MSGBoxCtrl.Result

		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Try

								Log.LogMaintenances.Remove(Log.LogMaintenances(Log.LogMaintenances.CurrentIndex))

								For i As Integer = 0 To Log.LogMaintenances.Count - 1
									Log.LogMaintenances(i).SrNo = i + 1
								Next

								Session("mLog") = Log
								Session("LogMaintenanceEdit") = False
								Log.Save()

								DataBindGrid()

								Log = Log.GetLog(Log.ID, IsFromTroubleshooting:=True)
								Log.LogMaintenances.Add(Log.ID)
								LogMaintenance = Log.LogMaintenances.CurrentItem

								Session("LogMaintenance") = LogMaintenance
								Session("mLog") = Log

								DataBind()

								upnlRecCnt.Update()
								upnlGridView.Update()
								upnlDetails.Update()
								ImageButton2.Visible = False
								btnDelAttach.Enabled = False

								ScriptManager.RegisterStartupScript(Me,
																	[GetType],
																	"CallautoResize",
																	"CallautoResize();",
																	True)

							Catch ex As SqlException

								If ex.Number = 8145 Then

									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
													MSGBox.Message_Text.ProcedureError,
													"",
													MsgBoxStyle.OkOnly,
													"")

								ElseIf ex.Number = 2627 Then

									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
													MSGBox.Message_Text.Duplicate,
													"",
													MsgBoxStyle.OkOnly,
													"")

								ElseIf ex.Number = 547 Then

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
													MSGBox.Message_Text.ReferenceDelete,
													"",
													MsgBoxStyle.OkOnly,
													"")

									MarkLog(Action.Delete,
											"Log Maintenance Activity List",
											"Can't delete : This is Currently in use",
											ErrorType.NoError,
											Log.ID,
											EventLogID)

								End If

								DataFieldBind()
								msgCount = ex.Errors.Count

							Finally

								If msgCount = 0 Then

									mLogDetail = Log.LogTextNo + " Dated : " + Log.DateFormatted + " Description :" + LogMaintenance.Maintenance

									MarkLog(Action.Delete,
											"LogMaintenanceActivityList",
											mLogDetail,
											ErrorType.NoError,
											Log.ID,
											EventLogID)

								End If

							End Try

						ElseIf MSGBoxCtrl.Sender = "SaveAndClose" Then

							If Save() = True Then

								mLogDetail = Log.LogTextNo +
											 " Dated :  " + Log.DateFormatted +
											 " Description :" + Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).Maintenance

								MarkLog(Action.Save,
										"LogMaintenanceActivityList",
										mLogDetail,
										ErrorType.HandledError,
										Log.ID,
										EventLogID)

								DataFieldBind()

								txtMainActivity.Text = ""
								txtNCRNo.Text = ""
								txtLicenceNo.Text = ""

								'MEL
								DiscrepancyCorrectiveAction.RectifiedDate = Session("LogDateForRectification").ToString
								DiscrepancyCorrectiveAction.RectifiedLogID = New Guid(Session("LogIDForRectification").ToString)
								DiscrepancyCorrectiveAction.InvestigationStatus = True
								DiscrepancyCorrectiveAction.Action = txtMainActivity.Text
								Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

								If DiscrepancyCorrectiveAction.IsValid Then

									DiscrepancyCorrectiveAction.Save()

									If (TransTypeID = 115) Then

										MSGBoxCtrl.Show("Alert..!!",
														"Do you want to Add to WatchList ?",
														"",
														MsgBoxStyle.YesNo,
														"AddWatchList")

									End If

									Exit Sub

								End If

							End If

						ElseIf MSGBoxCtrl.Sender = "AddWatchList" Then
							mdlPopUpPreventiveMeasures.Show()
						End If

					Case MsgBoxResult.No

						If MSGBoxCtrl.Sender = "Delete" Then
							DataFieldBind()
						ElseIf MSGBoxCtrl.Sender = "AddWatchList" Then

							MSGBoxCtrl.Show("Successfully Closed..!!",
											"Troubleshooting has been successfully closed, And the record does not need to be monitored any further.",
											"",
											MsgBoxStyle.OkOnly,
											"")

							chkClose.Checked = False
							Dim OpenAs As String = Request.QueryString("Type")

							If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

								ScriptManager.RegisterStartupScript(Me,
																	[GetType],
																	"onclose",
																	"CallParentCallback();",
																	True)
								Exit Sub

							End If

							Exit Sub

						End If

					Case MsgBoxResult.Cancel
						Session("sender") = ""
						DataFieldBind()
					Case MsgBoxResult.Ok

						If MSGBoxCtrl.Sender = "CloseCabinDefect" Then

							ScriptManager.RegisterStartupScript(Me,
																[GetType],
																"On Close",
																"CallParentCallback();",
																True)

						End If

					Case MsgBoxResult.Ok And Session("sender") = "Authorization"

				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
				DataFieldBind()
			ElseIf MsgBoxResult = 0 Then
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTitle()

		Try

			lblTitle.Text = $"{ IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")} Troubleshooting  [ {DiscrepancyCorrectiveAction.DefectNo} ]"
			upnlTitle.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetObject()

		Try

			Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).Maintenance = txtMainActivity.Text.Trim
			Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).NRCWONO = txtNCRNo.Text.Trim

			If calClosedDate.Text = "" Then
				Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ClosedDate = System.DBNull.Value
			Else
				Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ClosedDate = calClosedDate.Text.ToString
			End If

			If LogMaintenance IsNot Nothing Then

				Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageFile = LogMaintenance.ImageFile
				Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageSize = LogMaintenance.ImageSize
				Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).FileExtension = LogMaintenance.FileExtension

				'MLNo
				For i As Integer = 0 To LogMaintenance.MaintenanceDoneByEmployees.Count - 1

					Dim ID As Guid = LogMaintenance.MaintenanceDoneByEmployees(i).ID

					If Not Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Contains(ID) Then
						Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Add(LogMaintenance.MaintenanceDoneByEmployees(i))
					ElseIf Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Contains(ID) Then
						Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).LicenceNo = LogMaintenance.MaintenanceDoneByEmployees(i).LicenceNo
						Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).EmployeeID = LogMaintenance.MaintenanceDoneByEmployees(i).EmployeeID
						Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(ID).EmployeeName = LogMaintenance.MaintenanceDoneByEmployees(i).EmployeeName
					End If

				Next

				For j As Integer = 0 To Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Count - 1

					If Not LogMaintenance.MaintenanceDoneByEmployees.Contains(Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees(j).ID) Then
						Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Remove(LogMaintenance.MaintenanceDoneByEmployees(j).ID, "")
					End If

				Next

			End If

			If Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageSize > 0 Then
				ImageButton2.Visible = True
				btnDelAttach.Enabled = True
			Else
				ImageButton2.Visible = False
				btnDelAttach.Enabled = False
			End If

			Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).AssemblyStatusID = New Guid(cmbAssembly.SelectedValue)
			Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MELSnagCorrectiveActionID = DiscrepancyCorrectiveAction.ID
			Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).IsTroubleShoot = True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function Save() As Boolean

		Dim LogClone As Log
		Try

			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex > 0 Then

					If LogMaintenance Is Nothing Then

						Log = Log.GetLog(New Guid(cmbLog.SelectedValue), IsFromTroubleshooting:=True)
						Log.LogMaintenances.Add(Log.ID)
						LogMaintenance = Log.LogMaintenances.CurrentItem
						Session("mLog") = Log
						Session("LogMaintenance") = LogMaintenance
						LogClone = CType(Log.Clone, Log)

					Else

						If Session("LogMaintenanceEdit") = True Then

							Log = Session("LogForRectification")
							Dim LogMaintenanceCurrentIndex As Integer = Val(Session("LogMaintenanceCurrentIndex"))
							Log.LogMaintenances.CurrentIndex = LogMaintenanceCurrentIndex
							LogMaintenance = Log.LogMaintenances.CurrentItem

						End If

					End If

				End If

			ElseIf Session("TroubleShootFromLog") = "True" Then

				If LogMaintenance Is Nothing Then

					Log = CType(Session("mLog"), Log)
					Log = Log.GetLog(Log.ID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.Add(Log.ID)
					LogMaintenance = Log.LogMaintenances.CurrentItem
					Session("mLog") = Log
					Session("LogMaintenance") = LogMaintenance
					LogClone = CType(Log.Clone, Log)

				Else

					If Session("LogMaintenanceEdit") = True Then

						Log = Session("LogForRectification")
						Dim LogMaintenanceCurrentIndex As Integer = Val(Session("LogMaintenanceCurrentIndex"))
						Log.LogMaintenances.CurrentIndex = LogMaintenanceCurrentIndex
						LogMaintenance = Log.LogMaintenances.CurrentItem

					End If

				End If

			End If

			SetObject()

			If CustomValidate() Then

				Try

					If txtLicenceNo.Text <> "" Then

						If Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).MaintenanceDoneByEmployees.Count > 0 Then

							If Not Log.LogMaintenances.Item(Index:=Log.LogMaintenances.CurrentIndex).
														MaintenanceDoneByEmployees(0).EmployeeID.Equals(Guid.Empty) Then

								Dim Message As String = ""

								EmployeeStatus =
									EmployeeStatus.
										GetEmployeeWorkingStatus(EmployeeID:=Log.LogMaintenances.
																					Item(Index:=Log.LogMaintenances.CurrentIndex).
																						MaintenanceDoneByEmployees(0).EmployeeID.ToString,
																 EDate:=Log.Date)

								If (EmployeeStatus(0).Information <> "") Then

									Message = EmployeeStatus(0).Information

									MSGBoxCtrl.Show(MessageTitle:=" Save Alert !!",
													MessageText:="Resource Not Working.",
													ExtraMessage:=Message,
													ButtonToShow:=MsgBoxStyle.OkOnly,
													Sender:="")

									Return False

								End If

							End If

						End If

					End If

					If Not CheckZeroDifferenceValue() Then

						If Log.LogAFAssemblies.AssemblyRemoved Or
						   Log.LogEngAssemblies.AssemblyRemoved Or
						   Log.PropLogAssemblies.AssemblyRemoved Or
						   Log.LogAPUAssemblies.AssemblyRemoved Or
						   Log.LogCGBAssemblies.AssemblyRemoved Or
						   Log.LogNGBAssemblies.AssemblyRemoved Or
						   Log.LogGEAssemblies.AssemblyRemoved Or
						   Log.LogMRHAssemblies.AssemblyRemoved Or
						   Log.LogSPSAssemblies.AssemblyRemoved Or
						   Log.LogSSAAssemblies.AssemblyRemoved Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.EntryRestriction,
											MSGBox.Message_Text.EntryRestriction,
											"Required Assembly Of the Aircraft Is Not Installed On this Date Of Log. ",
											MsgBoxStyle.OkOnly,
											"")
							Return False

						End If

					End If

					Log.ApplyEdit()
					Log = CType(Log.Save(), Log)
					Session("mLog") = Log

					Return True

				Catch ex As SqlException

					Session("LogClone") = LogClone

					If ex.Number = 8114 Or ex.Number = 8115 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
										MSGBox.Message_Text.NumericOverFlow,
										"",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										"",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										"",
										MsgBoxStyle.OkOnly,
										"")

					End If

					Return False

				Finally
					LogClone = Nothing
				End Try

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Sub SetLabel()

		Try

			lblTroubleCount.Text = $"Troubleshooting ( {DiscrepancyTroubleShootList.Count} )"

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AttachMyFile()

		Try

			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex > 0 Then

					If LogMaintenance Is Nothing Then

						Log = Log.GetLog(New Guid(cmbLog.SelectedValue), IsFromTroubleshooting:=True)
						Log.LogMaintenances.Add(Log.ID)
						LogMaintenance = Log.LogMaintenances.CurrentItem
						Session("mLog") = Log
						Session("LogMaintenance") = LogMaintenance

					End If

				Else
					MSGBoxCtrl.Show(" Alert !! ", "Please select Log.", "", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If

			Else

				If LogMaintenance Is Nothing Then

					Log = CType(Session("mLog"), Log)
					Log = Log.GetLog(Log.ID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.Add(Log.ID)
					LogMaintenance = Log.LogMaintenances.CurrentItem
					Session("mLog") = Log
					Session("LogMaintenance") = LogMaintenance

				End If

			End If

			LogMaintenance = Session("LogMaintenance")

			Try

				LogMaintenance.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
				LogMaintenance.ImageSize = Session("FileUpload.FileSize")
				LogMaintenance.FileExtension = Session("FileUpload.FileExtension")
				Session.Remove("FileUpload.FileSize")
				Session.Remove("FileUpload.FileContent")
				Session.Remove("FileUpload.FileExtension")

				If LogMaintenance.ImageSize > 0 Then
					ImageButton2.Visible = True
					btnDelAttach.Enabled = True
				Else
					ImageButton2.Visible = False
					btnDelAttach.Enabled = False
				End If

				upnlAttach.Update()

			Catch ex As Exception

				MSGBoxCtrl.Show("Attachment Alert!",
								ex.Message,
								"",
								MsgBoxStyle.Information,
								"")

			End Try

			Session("LogMaintenance") = LogMaintenance
			Session("mLog") = Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Function CheckZeroDifferenceValue() As Boolean

		Try

			If Log.IsHobbs Then

				If Val(Log.TotalTime) <> 0 Then
					Return False
				End If

				If Val(Log.TimeInAir) <> 0 Then
					Return False
				End If

			Else

				If Log.TimeInAir = "0:00" OrElse Log.TimeInAir = "" Then
				Else
					Return False
				End If

				If Log.TotalTime = "0:00" OrElse Log.TotalTime = "" Then
				Else
					Return False
				End If

				If Log.BlockTime = "0:00" OrElse Log.BlockTime = "" Then
				Else
					Return False
				End If

				If Log.TimeOnGround = "0:00" OrElse Log.TimeOnGround = "" Then
				Else
					Return False
				End If

			End If

			If Val(Log.TotalLandings) <> 0 Then
				Return False
			End If

			Dim checkcol = Log.LogAFAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = Log.LogAPUAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = Log.LogEngAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = Log.LogCGBAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function CallZeroDifferenceValue(obj) As Boolean

		Try

			For i As Integer = 0 To obj.Count - 1

				If Log.IsHobbs Then

					If Val(obj(i).Hours) <> 0 Then
						Return False
					End If

				Else

					If obj(i).Hours <> "0:00" Then
						Return False
					End If

				End If

				If obj(i).ShowLandings Then

					If Val(obj(i).Landings) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowCycles Then

					If Val(obj(i).Cycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowStarts Then

					If Val(obj(i).Starts) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowNGCycles Then

					If Val(obj(i).NGCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowNFCycles Then

					If Val(obj(i).NFCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowRINS Then

					If Val(obj(i).RINS) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowBleeds Then

					If Val(obj(i).Bleeds) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowImpellerCycles Then

					If Val(obj(i).ImpellerCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowCTCycles Then

					If Val(obj(i).CTCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowPTCycles Then

					If Val(obj(i).PTCycles) <> 0 Then
						Return False
					End If

				End If

				If obj(i).ShowGeneratorMods Then

					If Val(obj(i).GeneratorMods) <> 0 Then
						Return False
					End If

				End If

			Next

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList()
			Session("LicenseNoListWithEmployee") = LicenseNoListWithEmployee
			DataBindGrid()

			AssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=0,
																   MachineID:=DiscrepancyCorrectiveAction.MachineID.ToString,
																   InstalledOn:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
																   AddTopItem:="",
																   IsInstalled:=True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList

			If DiscrepancyCorrectiveAction IsNot Nothing AndAlso
			   Not DiscrepancyCorrectiveAction.IsNew AndAlso
			   Not Session("TroubleShootFromLog") = "True" Then

				LogList = LogList.GetLogList(MachineID:=If(TransTypeID = 116, DiscrepancyCorrectiveAction.AircraftID, DiscrepancyCorrectiveAction.MachineID),
											 SouLocalDateTime:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
											 DesLocalDateTime:="1/1/2100",
											 ShowAddTopItem:=True,
											 AddTopItemText:="(SELECT)")

				cmbLog.DataSource = LogList
				Session("LogList") = LogList

			Else
				cmbLog.Visible = False
			End If

			DataBind()

			If Not (Log Is Nothing) And Session("TroubleShootFromLog") = "True" Then
				txtLogNoDet.Text = Log.LogNoLogPageNo
				cmbLog.Visible = False
				txtLogDate.Text = Log.DateFormatted
				txtLogNoDet.Visible = True
			Else
				cmbLog.Enabled = True
				cmbLog.Visible = True
				txtLogNoDet.Visible = False
			End If

			upnlDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DataBindGrid()

		Try

			DiscrepancyTroubleShootList = DiscrepancyTroubleShootList.GetDiscrepancyTroubleShootList(MELSnagCorrectiveActionID:=DiscrepancyCorrectiveAction.ID)
			dgDiscrepancyTroubleShootList.DataSource = DiscrepancyTroubleShootList
			dgDiscrepancyTroubleShootList.Columns(3).HeaderText = IIf(AppSettings("ClientCode") = "7AR", "Log Date (UTC)", "Log Date")
			dgDiscrepancyTroubleShootList.DataBind()
			Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList
			lblRecCount.Text = "Troubleshooting as per criteria : " & DiscrepancyTroubleShootList.Count.ToString & " Record(s) found."
			upnlRecCnt.Update()
			upnlGridView.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)

			TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
							  CInt(Request.QueryString("TransTypeID")),
							  115)

			Session("TransTypeID") = TransTypeID

			If Not IsPostBack And CType(Session("sender"), String) = "" Then

				UserNameForLicenceList = User.Identity.Name
				Session("UserNameForLicenceList") = UserNameForLicenceList

				SetTitle()
				DataFieldBind()

			End If

			SetLabel()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex = 0 Then

					MSGBoxCtrl.Show(" Alert !! ",
									"Please select a Log.",
									"",
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub

				End If

			End If

			If txtMainActivity.Text = "" Then

				MSGBoxCtrl.Show(" Alert !! ",
								"Please Add Troubleshooting Step(s).",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			If Save() = True Then

				mLogDetail = Log.LogTextNo +
							 " Dated : " + Log.DateFormatted +
							 " Description :" + Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).Maintenance

				MarkLog(Action.Save,
						"LogMaintenanceActivityList",
						mLogDetail,
						ErrorType.HandledError,
						Log.ID,
						EventLogID)

				DataFieldBind()

				txtMainActivity.Text = ""
				txtNCRNo.Text = ""

				txtLicenceNo.Text = ""
				btnDelAttach.Enabled = False
				ImageButton2.Visible = False

				upnlAdd.Update()

				RemoveSession()
				SetLabel()

				If chkClose.Checked Then

					DiscrepancyCorrectiveAction.RectifiedDate = Session("LogDateForRectification").ToString
					DiscrepancyCorrectiveAction.RectifiedLogID = New Guid(Session("LogIDForRectification").ToString)
					DiscrepancyCorrectiveAction.InvestigationStatus = True
					Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

					If DiscrepancyCorrectiveAction.IsValid Then

						DiscrepancyCorrectiveAction.Save()

						If TransTypeID = 115 Then

							MSGBoxCtrl.Show(" Alert ",
											"Do you want to Add to WatchList ?",
											"",
											MsgBoxStyle.YesNo,
											"AddWatchList")

						ElseIf TransTypeID = 116 Then

							MSGBoxCtrl.Show(" Alert ",
											$"Cabin Defect [ {DiscrepancyCorrectiveAction.DefectNo} ] Closed SuccessFully !!!",
											"",
											MsgBoxStyle.OkOnly,
											"CloseCabinDefect")

						End If

						Exit Sub

					End If

				Else
					Session.Remove("LogDateForRectification")
					Session.Remove("LogIDForRectification")
				End If

				MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
								MSGBox.Message_Text.SavedSuccessFully,
								"",
								MsgBoxStyle.OkOnly,
								"")
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveAndClose(sender As Object, e As EventArgs) Handles btnSaveAndClose.Click

		Try

			' Added By sachin on 29-Feb-2024
			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex = 0 Then

					MSGBoxCtrl.Show(" Alert !! ",
									"Please select a Log.",
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				End If

			End If

			If txtMainActivity.Text = "" Then

				MSGBoxCtrl.Show(" Alert !! ",
								"Please Enter Description.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			MSGBoxCtrl.Show(" Alert !! ",
							"Are you sure you want to Close this Discrepancy ?",
							"",
							MsgBoxStyle.YesNo,
							"SaveAndClose")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveWatchlist(sender As Object, e As EventArgs) Handles btnWatchlisteSave.Click

		Try

			If txtPreventiveMeasures.Text = "" Then

				MSGBoxCtrl.Show(" Alert !! ",
								"Please Enter Preventive Measures.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			mdlPopUpPreventiveMeasures.Hide()
			DiscrepancyCorrectiveAction.AddToWatchList = True
			DiscrepancyCorrectiveAction.PreventionTaken = txtPreventiveMeasures.Text

			If DiscrepancyCorrectiveAction.IsValid Then

				DiscrepancyCorrectiveAction.Save()

				RemoveSession()

				Dim Type As String = Request.QueryString("Type")
				If Type IsNot Nothing AndAlso Type = "pup" Then

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"On Close",
														"CallParentCallback();",
														True)
					Exit Sub

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddEmployeeLicences(sender As Object, e As ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click

		Try

			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex = 0 Then

					MSGBoxCtrl.Show("Alert !!",
									"Please select Log.",
									"",
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub

				End If

			End If

			Session("mMaintenanceID") = LogMaintenance.ID
			MaintenanceDoneByEmployees = LogMaintenance.MaintenanceDoneByEmployees
			Session("mMaintenanceDoneByEmployees") = MaintenanceDoneByEmployees
			Session("MaintenanceDoneOnDate") = Log.DateFormatted.ToString

			ScriptManager.RegisterClientScriptBlock(Me,
													[GetType],
													"AddEmployeeLicNo",
													"AddEmployeeLicNo();",
													True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseWatchlist(sender As Object, e As EventArgs) Handles btnWatchlisteClose.Click

		RemoveSession()
		Try

			Dim OpenAs As String = Request.QueryString("Type")

			If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"On Close",
													"CallParentCallback();",
													True)
				Exit Sub

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ReturnBack(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			MarkLog(Action.Close,
					"Log Maintenance Activity",
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

			SetSession()
			RemoveSession()

			Dim Type As String = Request.QueryString("Type")

			If Type IsNot Nothing AndAlso Type = "pup" Then

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"On Close",
													"CallParentCallback()",
													True)
				Exit Sub

			End If

			If Session("OpenFromLMA") = True Then
				Response.Redirect("Index.aspx")
			Else
				Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage")) 'Added by Prashant 23-Aug-2018
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub LicenceNoChanged(sender As Object, e As EventArgs) Handles txtLicenceNo.TextChanged

		Try

			If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
				LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
				EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
			Else
				LicenseNo = Trim(txtLicenceNo.Text)
			End If

			DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
			Session("LicenseNo") = LicenseNo
			Session("EmployeeID") = DoneByID

			If Not Session("TroubleShootFromLog") = "True" Then

				If cmbLog.SelectedIndex > 0 Then

					If LogMaintenance Is Nothing Then

						Log = Log.GetLog(New Guid(cmbLog.SelectedValue), IsFromTroubleshooting:=True)
						Log.LogMaintenances.Add(Log.ID)
						LogMaintenance = Log.LogMaintenances.CurrentItem
						Session("mLog") = Log
						Session("LogMaintenance") = LogMaintenance

					End If

				End If

			Else

				If LogMaintenance Is Nothing Then

					Log = CType(Session("mLog"), Log)
					Log = Log.GetLog(Log.ID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.Add(Log.ID)
					LogMaintenance = Log.LogMaintenances.CurrentItem
					Session("mLog") = Log
					Session("LogMaintenance") = LogMaintenance

				End If

			End If

			If Not DoneByID.Equals(Guid.Empty) Then

				If LogMaintenance.MaintenanceDoneByEmployees.Count > 0 Then
					LogMaintenance.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
					LogMaintenance.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
					LogMaintenance.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
				Else
					LogMaintenance.MaintenanceDoneByEmployees.Add(LogMaintenance.ID, 12, DoneByID, LicenseNo, "", EmpName)
				End If

			Else

				If LogMaintenance.MaintenanceDoneByEmployees.Count > 0 Then
					LogMaintenance.MaintenanceDoneByEmployees.RemoveAt(0)
				End If

			End If

			Session("LogMaintenance") = LogMaintenance
			BindLicenceNo(LogMaintenance)
			SetLicenceCount(LogMaintenance)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub LogChanged(sender As Object, e As EventArgs) Handles cmbLog.SelectedIndexChanged

		Try

			If cmbLog.SelectedIndex > 0 Then

				txtLogDate.Text = LogList(New Guid(cmbLog.SelectedValue.ToString)).DateFormatted
				Session("LogDateForRectification") = LogList(New Guid(cmbLog.SelectedValue.ToString)).DateFormatted
				Session("LogIDForRectification") = LogList(New Guid(cmbLog.SelectedValue.ToString)).ID

			Else
				txtLogDate.Text = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub ClosedDateChanged(sender As Object, e As EventArgs) Handles calClosedDate.TextChanged

		Try

			If IsDate(calClosedDate.Text) Or (calClosedDate.Text = "") Then

				If calClosedDate.Text = "" Then
					LogMaintenance.ClosedDate = System.DBNull.Value
					calClosedDate.Text = LogMaintenance.ClosedDate.ToString
				Else
					LogMaintenance.ClosedDate = calClosedDate.Text
					calClosedDate.Text = LogMaintenance.ClosedDateFormatted
				End If

			Else
				calClosedDate.Text = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click

		Dim AttachmentName As String
		Try

			If LogMaintenance.ImageSize > 0 Then

				Dim CompletePathForDownload As String = $"{AppSettings("DOCPath")}\{AttachmentName}{LogMaintenance.FileExtension}"
				Dim FileStream As FileStream

				If Not File.Exists(AppSettings("DOCPath")) Then

					'Delete File if exist
					File.Delete(AppSettings("DOCPath") & AttachmentName & LogMaintenance.FileExtension)

					' Create the file.
					FileStream = File.Create(CompletePathForDownload)

					'' Add some information to the file.
					FileStream.Write(LogMaintenance.ImageFile, 0, LogMaintenance.ImageFile.Length)
					FileStream.Close()

					Session("DOCPath") = CompletePathForDownload
					Session("AttachmentName") = AttachmentName

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnDelAttach.Click

		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte
		Try

			LogMaintenance.ImageFile = file1
			LogMaintenance.ImageSize = 0
			Session("LogMaintenance") = LogMaintenance
			ImageButton2.Visible = False
			btnDelAttach.Enabled = False

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_DiscrepancyTroubleShootList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgDiscrepancyTroubleShootList.Sorting

		Try

			DiscrepancyTroubleShootList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList
			dgDiscrepancyTroubleShootList.DataSource = DiscrepancyTroubleShootList
			dgDiscrepancyTroubleShootList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_DiscrepancyTroubleShootList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgDiscrepancyTroubleShootList.PageIndexChanging

		Try

			dgDiscrepancyTroubleShootList.PageIndex = e.NewPageIndex
			dgDiscrepancyTroubleShootList.DataSource = DiscrepancyTroubleShootList
			Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList
			dgDiscrepancyTroubleShootList.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_DiscrepancyTroubleShootList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgDiscrepancyTroubleShootList.RowCommand

		Try

			Dim index As Integer

			Select Case e.CommandName
				Case "EditRec"

					Dim mID As Guid
					mID = New Guid(e.CommandArgument.ToString)

					If DiscrepancyTroubleShootList(mID).RecordCount <> DiscrepancyTroubleShootList.Count Then

						MSGBoxCtrl.Show(" Alert !! ",
										"Record cannot be edited as its not the last Troubleshoot",
										"",
										MsgBoxStyle.OkOnly,
										"")
						Exit Sub

					End If

					Log = Log.GetLog(DiscrepancyTroubleShootList(mID).LogID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.CurrentIndex = DiscrepancyTroubleShootList(mID).SrNo - 1

					index = DiscrepancyTroubleShootList(mID).SrNo - 1
					Session("LogMaintenance") = Log.LogMaintenances.CurrentItem
					Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList

					If Session("TroubleShootFromLog") = "True" Then
						txtLogNoDet.Text = Log.LogNoLogPageNo
					Else
						cmbLog.SelectedValue = Log.LogMaintenances.Item(index).LogID.ToString
					End If

					txtMainActivity.Text = Log.LogMaintenances.Item(index).Maintenance
					txtNCRNo.Text = Log.LogMaintenances.Item(index).NRCWONO
					txtLogDate.Text = Log.DateFormatted
					chkClose.Checked = DiscrepancyCorrectiveAction.InvestigationStatus

					BindLicenceNo(Log.LogMaintenances.CurrentItem)
					SetLicenceCount(Log.LogMaintenances.CurrentItem)
					cmbAssembly.SelectedValue = Log.LogMaintenances.Item(index).AssemblyStatusID.ToString 'Added By Vikrant On 02-Sept-2014 For All04092014
					DataBindGrid()
					upnlGridView.Update()
					upnlDetails.Update()
					LogMaintenance = Log.LogMaintenances.CurrentItem

					Session("LogMaintenance") = LogMaintenance
					SetLabel()
					DataBind()

					mLogDetail = Log.LogTextNo + " Dated : " + Log.DateFormatted + " Description :" + LogMaintenance.Maintenance
					MarkLog(Action.Edit, "LogMaintenanceActivityList", mLogDetail, ErrorType.NoError, Log.ID, EventLogID)
					Session("LogMaintenanceEdit") = True
					Session("LogDateForRectification") = Log.DateFormatted
					Session("LogIDForRectification") = Log.ID
					Session("LogForRectification") = Log
					Session("LogMaintenanceCurrentIndex") = Log.LogMaintenances.CurrentIndex

					If Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageSize > 0 Then
						ImageButton2.Visible = True
						btnDelAttach.Enabled = True
					Else
						ImageButton2.Visible = False
						btnDelAttach.Enabled = False
					End If

					If Not (Log Is Nothing) And Session("TroubleShootFromLog") = "True" Then
						cmbLog.Visible = False
						txtLogNoDet.Visible = True
					Else
						cmbLog.Enabled = True
						cmbLog.Visible = True
						txtLogNoDet.Visible = False
					End If

					SetTitle()
					upnlErrorList.Update()

				Case "DeleteRec"

					Dim mID As Guid
					mID = New Guid(e.CommandArgument.ToString)

					If DiscrepancyTroubleShootList(mID).RecordCount <> DiscrepancyTroubleShootList.Count Then

						MSGBoxCtrl.Show(" Alert !! ",
										"Record cannot be deleted as its not the last Troubleshoot",
										"",
										MsgBoxStyle.OkOnly,
										"")
						Exit Sub

					End If

					MSGBoxCtrl.Show(MSGBox.Message_Title.Remove,
									MSGBox.Message_Text.Remove,
									"",
									MsgBoxStyle.YesNo,
									"Delete")

					Log = Log.GetLog(DiscrepancyTroubleShootList(mID).LogID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.CurrentIndex = DiscrepancyTroubleShootList(mID).SrNo - 1

					index = DiscrepancyTroubleShootList(mID).SrNo - 1
					Session("mLog") = Log
					Session("LogMaintenance") = Log.LogMaintenances.CurrentItem
					Session("mID") = mID
					Session("DiscrepancyTroubleShootList") = DiscrepancyTroubleShootList

				Case "ViewRec"

					Dim mID As Guid
					mID = New Guid(e.CommandArgument.ToString)

					If DiscrepancyTroubleShootList(mID).RecordCount <> DiscrepancyTroubleShootList.Count Then
						MSGBoxCtrl.Show(" Alert !! ", "Attachment can not be open as its not the last Troubleshoot", "", MsgBoxStyle.OkOnly, "")
						Exit Sub
					End If

					Log = Log.GetLog(DiscrepancyTroubleShootList(mID).LogID, IsFromTroubleshooting:=True)
					Log.LogMaintenances.CurrentIndex = DiscrepancyTroubleShootList(mID).SrNo - 1

					Dim No As New Random
					Dim strName As String = "Discrepancy Troubleshooting " & Log.LogNoLogPageNo & " as of " & New SmartDate(Log.Date.ToString).FormattedText

					If Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageSize > 0 Then

						Dim path As String = AppSettings("DOCPath") & strName & Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).FileExtension
						Dim fs As FileStream

						If File.Exists(AppSettings("DOCPath")) = False Then

							'Delete File if exist
							File.Delete(AppSettings("DOCPath") & strName & Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).FileExtension)

							' Create the file.
							fs = File.Create(path)
							fs.Write(Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageFile, 0, Log.LogMaintenances.Item(Log.LogMaintenances.CurrentIndex).ImageFile.Length)
							fs.Close()

							Session("DOCPath") = path
							Dim Str As String
							Str = "openFile();"
							ScriptManager.RegisterStartupScript(Me,
																[GetType],
																"open File",
																Str,
																True)

						End If

					End If

			End Select

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub HdnBtnMaintenanceDoneBy(sender As Object, e As EventArgs) Handles hdnBtnMaintDoneBy.Click

		Try

			For i As Integer = 0 To MaintenanceDoneByEmployees.Count - 1

				Dim ID As Guid = MaintenanceDoneByEmployees(i).ID

				If Not LogMaintenance.MaintenanceDoneByEmployees.Contains(ID) Then
					LogMaintenance.MaintenanceDoneByEmployees.Add(MaintenanceDoneByEmployees(i))
				ElseIf LogMaintenance.MaintenanceDoneByEmployees.Contains(ID) Then
					LogMaintenance.MaintenanceDoneByEmployees(ID).LicenceNo = MaintenanceDoneByEmployees(i).LicenceNo
					LogMaintenance.MaintenanceDoneByEmployees(ID).EmployeeID = MaintenanceDoneByEmployees(i).EmployeeID
					LogMaintenance.MaintenanceDoneByEmployees(ID).EmployeeName = MaintenanceDoneByEmployees(i).EmployeeName
				End If

			Next

			For j As Integer = 0 To LogMaintenance.MaintenanceDoneByEmployees.Count - 1

				If Not MaintenanceDoneByEmployees.Contains(LogMaintenance.MaintenanceDoneByEmployees(j).ID) Then
					LogMaintenance.MaintenanceDoneByEmployees.Remove(LogMaintenance.MaintenanceDoneByEmployees(j).ID, "")
				End If

			Next

			Session("LogMaintenance") = LogMaintenance
			BindLicenceNo(LogMaintenance)
			SetLicenceCount(LogMaintenance)
			upnlLicenceNo.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FileUploadHdnBtn(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		Try
			AttachMyFile()
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

#End Region

#Region " Service Methods "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetEmployeeList(prefixText As String, count As Integer, contextKey As String) As String()

		Try

			Dim Employeelist As LicenseNoListWithEmployee
			Employeelist = LicenseNoListWithEmployee.GetLicenseNoList(prefixText)

			If count = 0 Then
				Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In Employeelist
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
			Else
				Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In Employeelist
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

#End Region

End Class