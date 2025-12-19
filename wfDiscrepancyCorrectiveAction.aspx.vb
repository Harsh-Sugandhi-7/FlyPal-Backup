'**********************************
'Created by : Saylee 
'Dated      : 22-Feb-2024
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'**********************************

Imports System.Linq

Public Class DiscrepancyCorrectiveActionDetailPage
	Inherits Page

#Region " Variable Declaration "

	Public DiscrepancyCorrectiveActionList As MELSnagCorrectiveActionListNew
	Public MaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
	Public DiscrepancyCorrectiveActionLog As MELSnagCorrectiveActionLog
	Public DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
	Public RectifiedReportLogRegister As New ReportLogRegister
	Public AuthorizationHelper As New AuthorizationHelper
	Public ReportLogRegister As New ReportLogRegister
	Public AttachmentHelper As New AttachmentHelper
	Public MELSnagPartList As MELSnagPartList
	Public TempAssemblyList As AssemblyList
	Public AssemblyList As AssemblyList
	Public FileAttach As FileAttach
	Public ModuleList As ModuleList
	Public SubATAList As SubATAList
	Public ATAList As ATAList

	Dim BackPage As String
	Dim strMsg As String = ""
	Dim MachineID As String
	Dim mShowMEL As Boolean = False
	Dim EventLogID As Guid
	Dim mMELSnagDetail As String
	Dim IsAttachmentDeleted As Boolean = False
	'MLNo
	Dim LicenseNo As String = String.Empty
	Dim EmpName As String = String.Empty
	Dim DoneByID As Guid = Guid.Empty
	Shared UserNameForLicenseList As String
	Dim CalFromDate, CalToDate As String
	Dim TransTypeID As Integer
	Dim Prefix As String

#End Region

#Region " Helper Methods "

	Private Sub GetSession()

		DiscrepancyCorrectiveActionList = Session("DiscrepancyCorrectiveActionList")
		DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
		MachineID = Session("MachineID").ToString
		MELSnagPartList = Session("MELSnagPartList")
		ReportLogRegister = CType(Session("ReportLogRegister"), ReportLogRegister)
		RectifiedReportLogRegister = CType(Session("RectifiedReportLogRegister"), ReportLogRegister)
		TempAssemblyList = CType(Session("TempAssemblyList"), AssemblyList)
		DiscrepancyCorrectiveActionLog = CType(Session("MELSnagCorrectiveActionLog"), MELSnagCorrectiveActionLog)
		mShowMEL = CType(Session("ShowMEL"), Boolean)
		ATAList = CType(Session("ATAList"), ATAList)
		'Added By Vikrant On 02-Apr-2013 For ALL01042013
		SubATAList = Session("SubATAList")
		AssemblyList = Session("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
		FileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		'MLNo
		MaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
		UserNameForLicenseList = Session("UserNameForLicenseList")
		ModuleList = Session("ModuleList")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
		Session("MachineID") = MachineID
		Session("ReportLogRegister") = ReportLogRegister
		Session("RectifiedReportLogRegister") = RectifiedReportLogRegister
		Session("TempAssemblyList") = TempAssemblyList
		Session("MELSnagCorrectiveActionLog") = DiscrepancyCorrectiveActionLog
		Session("MELSnagPartList") = MELSnagPartList
		Session("ATAList") = ATAList
		Session("SubATAList") = SubATAList
		Session("mFileAttach") = FileAttach
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			Dim str As String

			str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
			ClientScript.RegisterStartupScript([GetType], "focusscript", str)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Function GetDefectNo() As String

		Try

			Dim No As New Random
			Dim ReportNo As String
			ReportNo = $"{IIf(TransTypeID = 116, "CBDF", "DSCR")} / {Session("AircraftRegNo")}"

			Return ReportNo

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Sub SetObject()

		Try

			With DiscrepancyCorrectiveAction

				.LogID = New Guid(cmbLogNo.SelectedValue)
				.LogNo = ReportLogRegister(New Guid(cmbLogNo.SelectedValue)).LogNo
				.DefectReportNo = Trim(txtDefectReportNo.Text)
				.No = Val(txtNo.Text)
				.Sector = Trim(txtSector.Text)
				.LastMajorCheckHour = Trim(txtLastMajorCheck.Text)
				.ReportedBy = Trim(txtReportedBy.Text)
				.PartID = New Guid(cmbPartNo.SelectedValue)
				.Description = Trim(txtDescription.Text)
				.ComponentHour = Trim(txtHrsofComp.Text)
				.Defect = Trim(txtDefect.Text)
				.CauseOfDefect = Trim(txtCauseofDefect.Text)
				.Action = Trim(txtAction.Text)
				.ActionAgainstStaff = Trim(txtActionTakenAganistEngStaff.Text)
				.PreventionTaken = Trim(txtPreventiveMeasures.Text)
				.IsMEL = rdbMEL.Checked
				.MELCategoryID = cmbMELCategory.SelectedValue
				.ATAChapterID = New Guid(cmbATAChapter.SelectedValue)
				.IsMajor = rbMajor.Checked
				.IsMinor = rbMinor.Checked
				.InvestigationStatus = (cmbInvestigation.SelectedIndex = 1)
				.MachineID = New Guid(MachineID.ToString)
				.FrequencyInDays = Val(txtFrequencyInDay.Text)
				.FrequencyInHours = txtFrequencyInHours.Text.Trim
				.FrequencyInCycles = txtCycles.Text.Trim
				.RectifiedStation = txtRectificationSector.Text.Trim
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
				.IsDeviationList = rdbCDL.Checked
				.AddToWatchList = chkAddtoWatchList.Checked
				.DueInHrs = Trim(txtDueHrs.Text)
				.DueInCycles = Trim(txtDueCycles.Text)
				.IsIncident = rbIncident.Checked
				.ExtensionInHours = txtExtensionInHours.Text.Trim
				.ExtensionInCycles = txtExtensionInCycles.Text.Trim
				.IsAOG = (cmbInvestigation.SelectedIndex = 3)

				If txtDateofOccurrence.Text <> "" Then
					.DateOfOccurrence = txtDateofOccurrence.Text
				Else
					.DateOfOccurrence = DBNull.Value
				End If

				If cmbPartNo.SelectedIndex > 0 Then
					.PartSerialNo = MELSnagPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").SerialNo
				Else
					.PartSerialNo = Trim(txtSerialNo.Text)
				End If

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

				If FileAttach IsNot Nothing Then
					.IsAttachmentAdded = (FileAttach.Size > 0)
				End If

				.IsCabinDefect = (TransTypeID = 116)
				.AircraftID = New Guid(MachineID.ToString)

			End With

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub FillRectifiedCombo()

		Try

			If IsDate(txtRectifiedDate.Text) Or (txtRectifiedDate.Text = "") Then

				cmbRectifiedLogNo.Enabled = True

				If txtRectifiedDate.Text = "" Then
					DiscrepancyCorrectiveAction.RectifiedDate = DBNull.Value
				Else
					DiscrepancyCorrectiveAction.RectifiedDate = txtRectifiedDate.Text
				End If

				Dim TmpLogDetail As Log

				If cmbLogNo.SelectedIndex > 0 Then

					TmpLogDetail = Log.GetLog(New Guid(cmbLogNo.SelectedValue))

				End If

				RectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=GetEarlierDate(LogDate:=TmpLogDetail.Date.ToString,
																										 OccurrenceDate:=txtDateofOccurrence.Text),
																			   EndDate:="1/1/2100",
																			   AssemblyID:=TempAssemblyList(0).ID.ToString,
																			   MachineID:=MachineID,
																			   CalculateTotal:=False, ,
																			   StatusSelectLog:=0, , , ,
																			   AddTopItem:="(SELECT)",
																			   IsFromMEL:=True,
																			   LogID:=IIf(cmbLogNo.SelectedIndex > 0,
																						  cmbLogNo.SelectedValue.ToString,
																						  Guid.Empty),
																			   SkipVoidLog:=True)

				cmbRectifiedLogNo.DataSource = RectifiedReportLogRegister
				cmbRectifiedLogNo.DataBind()
				TmpLogDetail = Nothing


				If DiscrepancyCorrectiveAction.RectifiedDate.ToString = "" Then
					txtRectifiedDate.Text = DiscrepancyCorrectiveAction.RectifiedDate.ToString
				Else
					txtRectifiedDate.Text = DiscrepancyCorrectiveAction.RectifiedDateFormatted
				End If

				Session("RectifiedReportLogRegister") = RectifiedReportLogRegister
				cmbRectifiedLogNo.SelectedIndex = 0

			Else

				If DiscrepancyCorrectiveAction.RectifiedDate.ToString = "" Then
					txtRectifiedDate.Text = DiscrepancyCorrectiveAction.RectifiedDate.ToString
				Else
					txtRectifiedDate.Text = DiscrepancyCorrectiveAction.RectifiedDateFormatted
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	'MLNo
	Public Sub SetLicenseCount()

		Try

			If DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 Then
				lblLicenceCount.Text = $"And {(DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count - 1)} more"
			End If

			lblLicenceCount.DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub BindLicenseNo()

		Try

			If DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
				txtLicenceNo.Text = DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo +
									" [" + DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName + "]"
			Else
				txtLicenceNo.Text = String.Empty
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub
	'End

	Private Sub MessageBoxResult()

		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then

			Select Case Result1

				Case MsgBoxResult.Yes

					If MSGBoxCtrl.Sender = "Delete" Then

						Try

							Session("sender") = ""
							DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
							DiscrepancyCorrectiveAction.DeleteMELSnagCorrectiveAction(DiscrepancyCorrectiveAction.ID)
							ControlVisibility()
							SetTitle()
							upnlTitle.Update()
							upnlMELSnagDetails.Update()
							upnlCreateWO.Update()

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

								MarkLog(Action.Delete,
										"DiscrepancyAction",
										"Can't delete : This is Currently in use",
										ErrorType.NoError,
										DiscrepancyCorrectiveAction.ID,
										EventLogID)

								MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
												MSGBox.Message_Text.ReferenceDelete,
												"",
												MsgBoxStyle.OkOnly,
												"")

							End If

							DataFieldBind()

						End Try

					ElseIf MSGBoxCtrl.Sender = "Close" Then    '' Close confirmation

						Session("sender") = ""
						DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")

						'Added By Harsh on 11th March 2024 -- Validate Page instead of object
						Page.Validate()
						If Not Page.IsValid() Then

							upnlErrorList.Update()
							upnlVerificationDetailsErrors.Update()
							upnlRectificationDetailsErrors.Update()
							Exit Sub

						End If

						If cmbInvestigation.SelectedIndex = 0 And txtAction.Text <> "" Then

							Dim MessageText As String = $"{IIf(TransTypeID = 116,
														   "Cabin Defect should be Open Or Closed. Please select one from it.",
														   "Discrepancy should be Deferred / AOG Or Closed. Please select one from it.")}"

							MSGBoxCtrl.Show("Alert!",
											MessageText,
											"",
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						ElseIf cmbInvestigation.SelectedIndex = 2 AndAlso
							   (rdbMEL.Checked = False And rdbCDL.Checked = False) AndAlso
							   (TransTypeID = 115) Then

							MSGBoxCtrl.Show("Alert!",
											"Discrepancy should either be MEL or Deferred.",
											"",
											MsgBoxStyle.OkOnly,
											"")
							Exit Sub

						End If

						If chkExtensionApplied.Checked = True Then

							If txtExtensionApprovalNo.Text = "" Then
								MSGBoxCtrl.Show("Alert!",
												"Please Enter Approval Details.",
												"",
												MsgBoxStyle.OkOnly,
												"")
								Exit Sub
							End If

						End If

						'Modified by Harsh to bind the Log Dropdown
						If DiscrepancyCorrectiveAction.IsValid Then

							'Session("ForDateOfOccurrence") = "ForDateOfOccurrence"
							Try

								DataFieldBind()

								''Added By Prashant 2-Jan-2014  --ALL02012014-1
								If (cmbATAChapter.SelectedIndex > 0 And chkIsRepetitive.Checked = False And
									DiscrepancyCorrectiveAction.IsNew = True) Then

									Dim mMELSnagCountATAWise As MELSnagCountATAWise
									mMELSnagCountATAWise = MELSnagCountATAWise.GetMELSnagCountATAWise(ATAChapterID:=DiscrepancyCorrectiveAction.ATAChapterID.ToString,
																									  MachineID:=DiscrepancyCorrectiveAction.MachineID.ToString,
																									  MELSnagCorrectiveActionID:=DiscrepancyCorrectiveAction.ID.ToString,
																									  MELLastInDays:=Val(AppSettings("MEL_Occurrance_In_Days")),
																									  OccuranceDate:=DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted,
																									  MELCheckON:=Val(AppSettings("MEL_Check_ON"))) 'Added config parameters by Saylee on  24-Feb-2020 for ALL24022020

									If mMELSnagCountATAWise.Item(0).MELSnagCount > 0 Then

										Dim MsgStr As String = String.Empty
										MsgStr = "There are " + mMELSnagCountATAWise.Item(0).MELSnagCount.ToString +
												 " Discrepancies reported for this ATA. " +
												 " Last Log Date is " +
												 New SmartDate(mMELSnagCountATAWise.Item(0).LogInfo.ToString.Substring(0, mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(0).Trim.Length)).FormattedText +
												 "<BR>" + " Log No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(1).Trim +
												 "<BR>" + " Log Page No. : " + mMELSnagCountATAWise.Item(0).LogInfo.Split(",")(2).Trim +
												 "<BR>" + " Do you want to make this " + IIf(CBool(AppSettings("MELSnagNomenclature")), "Defect as Repetitive Defect", "Snag as Repetitive Snag") +
												 "?"

										MSGBoxCtrl.Show(MSGBox.Message_Title.Confirmation,
														MSGBox.Message_Text.Confirmation,
														MsgStr,
														MsgBoxStyle.YesNo,
														"MELSnagCountATAWise")

										Exit Sub

									End If

								End If

								DiscrepancyCorrectiveAction = DiscrepancyCorrectiveAction.Save
								SaveAttachment()

								Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
								Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList

								mMELSnagDetail = DiscrepancyCorrectiveAction.DefectNo + " Dated : " +
												 DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " +
												 DiscrepancyCorrectiveAction.LogNo

								MarkLog(Action.Save,
										"DiscrepancyAction",
										mMELSnagDetail,
										ErrorType.NoError,
										DiscrepancyCorrectiveAction.ID,
										EventLogID)

								Session.Remove("DiscrepancyCorrectiveAction")
								Session.Remove("FileAttach")
								'MLNoIf Not 
								Session.Remove("MaintenanceDoneByEmployees")
								Session.Remove("UserNameForLicenseList")
								'End
								If DiscrepancyCorrectiveAction.InvestigationStatus = True Then

									ScriptManager.RegisterStartupScript(Me,
																		[GetType],
																		"Disable Controls On Close",
																		"disableControlsOnClose();",
																		True)

								End If

								Dim Type As String = Request.QueryString("Type")
								Dim Script As String = IIf(TransTypeID = 115, "CallParentCallback()", "CallParentCallbackForCabinDefect()")

								If Type IsNot Nothing AndAlso Type = "pup" Then

									ScriptManager.RegisterStartupScript(page:=Me,
																		type:=[GetType],
																		key:="On Close",
																		script:=Script,
																		addScriptTags:=True)

									Exit Sub

								End If
								'End

								If Request.QueryString("BackPage1") = "wfnWODetail_AJAX.aspx" Then
									Response.Redirect(Request.QueryString("BackPage1"))
								Else
									Response.Redirect(Request.QueryString("BackPage"))
								End If

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

									MarkLog(Action.Delete,
											"DiscrepancyAction",
											"Can't delete : This is Currently in use",
											ErrorType.NoError,
											DiscrepancyCorrectiveAction.ID,
											EventLogID)

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
													MSGBox.Message_Text.ReferenceDelete,
													"",
													MsgBoxStyle.OkOnly,
													"")

								End If

							End Try

						Else

							'Session("ForDateOfOccurrence") = "ForDateOfOccurrence"
							DataFieldBind()
							ControlVisibility()

							If DiscrepancyCorrectiveAction.RectifiedDate.ToString <> "" Then
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
						Session("ForDateOfOccurrence") = "ForDateOfOccurrence"
						DiscrepancyCorrectiveAction.IsRepetitive = True
						DataFieldBind()

						If Save() Then

							ControlVisibility()
							SetTitle()

							chkIsRepetitive.Checked = DiscrepancyCorrectiveAction.IsRepetitive

							MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
											MSGBox.Message_Text.SavedSuccessFully,
											"",
											MsgBoxStyle.OkOnly,
											"")

							upnlIsRepetitive.Update()
							upnlTitle.Update()
							upnlCreateWO.Update()
							upnlHeaderButtons.Update()
							upnlInvestigation.Update()
							upnlMELDeviation.Update()
							upnlInvestigation.Update()
							upnlLinks.Update()

							If cmbInvestigation.SelectedValue = "1" Then

								ScriptManager.RegisterStartupScript(Me,
																	[GetType],
																	"Disable Controls On Close",
																	"disableControlsOnClose();",
																	True)
							End If

						End If

					End If

				Case MsgBoxResult.No

					If MSGBoxCtrl.Sender = "Close" Then

						Session.Remove("IsValid")

						If DiscrepancyCorrectiveAction.IsNew Then
							Session.Remove("DiscrepancyCorrectiveAction")
						End If

						Session("Sender") = ""
						Session.Remove("DiscrepancyCorrectiveAction")
						Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList

						'Added By Vikrant On 02-Apr-2013 For ALL01042013
						Session.Remove("SubATAList")
						'End
						ControlVisibility()
						SetTitle()
						upnlTitle.Update()
						Session.Remove("FileAttach")
						'MLNo
						Session.Remove("MaintenanceDoneByEmployees")
						Session.Remove("UserNameForLicenseList")
						'End
						BackPage = Session("BackPage")

						Dim Type As String = Request.QueryString("Type")
						Dim Script As String = IIf(TransTypeID = 115, "CallParentCallback()", "CallParentCallbackForCabinDefect()")

						If Type IsNot Nothing AndAlso Type = "pup" Then

							ScriptManager.RegisterStartupScript(page:=Me,
																type:=[GetType],
																key:="On Close",
																script:=Script,
																addScriptTags:=True)

							Exit Sub

						End If
						'End

						If BackPage = "" Then
							Response.Redirect("index.aspx")
						Else
							Response.Redirect(Request.QueryString("BackPage"))
						End If

						''Added By Prashant 2-Jan-2014  --ALL02012014-1
					ElseIf MSGBoxCtrl.Sender = "MELSnagCountATAWise" Then

						Session("sender") = ""
						Session("ForDateOfOccurrence") = "ForDateOfOccurrence"
						DataFieldBind()

						If Save() Then

							ControlVisibility()
							SetTitle()
							upnlIsRepetitive.Update()
							upnlButtons.Update()
							upnlTitle.Update()
							upnlMELSnagDetails.Update()
							upnlHeaderButtons.Update()
							upnlInvestigation.Update()

							If cmbInvestigation.SelectedValue = "1" Then

								ScriptManager.RegisterStartupScript(Me,
																	[GetType],
																	"Disable Controls On Close",
																	"disableControlsOnClose();",
																	True)

							End If

						End If

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

				Case MsgBoxResult.Ok And Session("sender") = "Authorization"

					Session("sender") = ""
					DataFieldBind()
					ControlVisibility()
					SetTitle()
					upnlTitle.Update()

			End Select

		ElseIf Result1 = -1 Then

			Session("sender") = ""
			Session("ForDateOfOccurrence") = "ForDateOfOccurrence"
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

		Try

			If chkClose.Checked Or cmbInvestigation.SelectedIndex = 1 Then
				txtRectifiedDate.ReadOnly = False
			Else

				txtRectifiedDate.Text = ""
				txtRectifiedDate.ReadOnly = True
				cmbRectifiedLogNo.SelectedIndex = 0
				cmbRectifiedLogNo.Enabled = False
				txtRectificationSector.Text = ""
				DiscrepancyCorrectiveAction.RectifiedLogID = Guid.Empty

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetTitle()

		Try

			If DiscrepancyCorrectiveAction.IsNew Then
				lblSnagCorrectiveActionInfo.Text = $"{Prefix} [ New ]"
			Else
				lblSnagCorrectiveActionInfo.Text = $"{Prefix}  [ {DiscrepancyCorrectiveAction.DefectNo} ]"
			End If

			lblTroubleCount.Text = $"Troubleshooting ( {DiscrepancyCorrectiveAction.TotalTroubleShootCount} )"

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ControlVisibility()

		Try

			If chkClose.Checked Or cmbInvestigation.SelectedIndex = 1 Then
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
				txtHrsofComp.BackColor = Color.FromKnownColor(KnownColor.White)
				txtDescription.ReadOnly = False
				txtPartNo.ReadOnly = False
				txtSerialNo.ReadOnly = False
				txtHrsofComp.ReadOnly = False

			Else

				txtDescription.ReadOnly = True
				txtPartNo.ReadOnly = True
				txtSerialNo.ReadOnly = True
				txtDescription.BackColor = Color.FromName("#E0E0E0")
				txtPartNo.BackColor = Color.FromName("#E0E0E0")
				txtSerialNo.BackColor = Color.FromName("#E0E0E0")
				txtHrsofComp.BackColor = Color.FromName("#E0E0E0")

			End If

			'Added By Vikrant On 03-Apr-2013 For ALL01042013
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
			'End

			If (rdbMEL.Checked = True Or rdbCDL.Checked = True) And
			   (txtMELDescription.Text <> "") And DiscrepancyCorrectiveAction.InvestigationStatus = False Then

				If cmbInvestigation.SelectedIndex = 1 Then
					chkExtensionApplied.Enabled = False
				Else
					chkExtensionApplied.Enabled = True
				End If

				phFreq.Visible = True

			Else

				chkExtensionApplied.Enabled = False
				txtExtensionInDays.Enabled = False
				txtExtensionApprovalNo.Enabled = False
				phFreq.Visible = False

			End If

			Dim OpenedFrom As String = Request.QueryString("OpenFromWatchDiscrepanciesLink")

			If cmbInvestigation.SelectedValue = "2" Or OpenedFrom = "WatchDiscrepanciesLink" Then

				If DiscrepancyCorrectiveAction.MELID.Equals(Guid.Empty) = True And
				   DiscrepancyCorrectiveAction.DeviationListID.Equals(Guid.Empty) = True And
				   OpenedFrom = "WatchDiscrepanciesLink" Then
					phDeviationMEL.Visible = False
				Else
					phDeviationMEL.Visible = True
				End If

			Else
				phDeviationMEL.Visible = False
			End If

			If cmbInvestigation.SelectedValue = "1" Then

				rectDate.Visible = True
				phWatchListDetails.Visible = (TransTypeID = 115)

				If DiscrepancyCorrectiveAction.RectifiedLogID.Equals(Guid.Empty) Then
					cmbRectifiedLogNo.Enabled = True
					txtRectifiedDate.Enabled = True
				Else
					cmbRectifiedLogNo.Enabled = False
					txtRectifiedDate.Enabled = False
				End If

			Else

				rectDate.Visible = False
				cmbRectifiedLogNo.Enabled = False
				txtRectifiedDate.Enabled = False

			End If

			If Not DiscrepancyCorrectiveAction.IsNew AndAlso TransTypeID = 115 Then

				If DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCycles = "" Then
					txtCycles.ReadOnly = True
				Else
					txtCycles.ReadOnly = False
				End If

				If DiscrepancyCorrectiveAction.IsDeviationList = True Then
					rdbCDL.Text = $"Other Deferred List ( {DiscrepancyCorrectiveAction.CauseOfDefect} )"
				Else
					rdbCDL.Text = "Other Deferred List"
				End If

			End If

			lblLogNoStar.Visible = (TransTypeID = 115)

			ControlVisibilityForAttachment()

			upnlFileupload.Update()
			upnlInvestigation.Update()
			upnlMELDeviation.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ControlVisibilityAfterEdit()

		Try

			If DiscrepancyCorrectiveAction.MELCategoryID = 1 And DiscrepancyCorrectiveAction.IsMEL Then

				txtFrequencyInHours.Enabled = True
				txtFrequencyInDay.Enabled = False

			ElseIf DiscrepancyCorrectiveAction.MELCategoryID = 1 And DiscrepancyCorrectiveAction.IsMEL Then

				txtFrequencyInHours.Enabled = False
				txtFrequencyInDay.Enabled = True

			ElseIf (DiscrepancyCorrectiveAction.MELCategoryID = 2 Or
					DiscrepancyCorrectiveAction.MELCategoryID = 3 Or
					DiscrepancyCorrectiveAction.MELCategoryID = 4) And
				   DiscrepancyCorrectiveAction.IsMEL Then

				txtFrequencyInHours.Enabled = False
				txtFrequencyInDay.Enabled = False
				txtCycles.Enabled = False

			Else

				txtFrequencyInHours.Enabled = True
				txtFrequencyInDay.Enabled = True
				txtCycles.Enabled = True

			End If

			If DiscrepancyCorrectiveAction.IsMEL Then

				cmbMELCategory.Enabled = False
				cmbMELCategory.Visible = True
				lblMELCategory.Visible = True

			ElseIf DiscrepancyCorrectiveAction.IsDeviationList Then

				cmbMELCategory.Visible = False
				lblMELCategory.Visible = False
				txtFrequencyInDay.Enabled = True
				DiscrepancyCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text)

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

			End If

			If DiscrepancyCorrectiveAction.RectifiedDate.ToString <> "" Then
				cmbRectifiedLogNo.Enabled = True
			End If

			If Not DiscrepancyCorrectiveAction.LogID.Equals(Guid.Empty) Then
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

			If (DiscrepancyCorrectiveAction.IsMEL = True Or rdbMEL.Checked = True) Or
			   (DiscrepancyCorrectiveAction.IsDeviationList = True Or rdbCDL.Checked = True) Then

				If DiscrepancyCorrectiveAction.InvestigationStatus = True Or chkClose.Checked = True Or cmbInvestigation.SelectedIndex = 1 Then
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
				DiscrepancyCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

				upnlExtension.Update()

			End If

			' Added by Saylee on 5-Feb-2024 
			If DiscrepancyCorrectiveAction.IsWOCreated Then

				lnkbtnCreateWorkOrder.Text = "View Work Order "
				lnkbtnCreateWorkOrder.ToolTip = DiscrepancyCorrectiveAction.WONumber

			Else

				lnkbtnCreateWorkOrder.Text = "Create Work Order"
				lnkbtnCreateWorkOrder.ToolTip = "Create Work Order"

			End If

			If DiscrepancyCorrectiveAction.InvestigationStatus = True Then

				cmbInvestigation.SelectedIndex = 1

			ElseIf DiscrepancyCorrectiveAction.InvestigationStatus = False And
				   (DiscrepancyCorrectiveAction.IsMEL Or DiscrepancyCorrectiveAction.IsDeviationList) Then

				cmbInvestigation.SelectedIndex = 2

			ElseIf DiscrepancyCorrectiveAction.InvestigationStatus = False And
				  (DiscrepancyCorrectiveAction.IsAOG) Then

				cmbInvestigation.SelectedIndex = 3

			End If

			If Not DiscrepancyCorrectiveAction.IsNew Then

				If DiscrepancyCorrectiveActionLog(LogID:=New Guid(cmbLogNo.SelectedValue.ToString)).FinalCycles = "" Then
					txtCycles.ReadOnly = True
				Else
					txtCycles.ReadOnly = False
				End If

				If DiscrepancyCorrectiveAction.IsDeviationList = True Then
					rdbCDL.Text = "Other Deferred List ( " + DiscrepancyCorrectiveAction.CauseOfDefect + " )"
				Else
					rdbCDL.Text = "Other Deferred List"

				End If

				If DiscrepancyCorrectiveAction.IsMEL Or DiscrepancyCorrectiveAction.IsDeviationList Then
					phFreq.Visible = True
				Else
					phFreq.Visible = False
				End If

			End If

			phReportedAs.Visible = Not (TransTypeID = 116)

			Dim ShowAllDropDownOptions As Boolean = (TransTypeID = 115)

			If Not ShowAllDropDownOptions Then

				Dim deferredItem As ListItem = cmbInvestigation.Items.FindByText("Deferred")

				If deferredItem IsNot Nothing Then
					cmbInvestigation.Items.Remove(deferredItem)
				End If

				Dim aogItem As ListItem = cmbInvestigation.Items.FindByText("AOG")

				If aogItem IsNot Nothing Then
					cmbInvestigation.Items.Remove(aogItem)
				End If

			End If

			upnlInvestigation.Update()
			upnlMELDeviation.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub AddAttributes()

		Try

			txtFrequencyInDay.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtFrequencyInDay').value,event)")
			txtExtensionInDays.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtExtensionInDays').value,event)")
			txtFrequencyInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtFrequencyInHours').value,event)")
			txtCycles.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtCycles').value,event)")
			txtExtensionInHours.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtExtensionInHours').value,event)")
			txtExtensionInCycles.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtExtensionInCycles').value,event)")
			txtCycles.Attributes.Add("onKeyPress", "validateText('NUM',document.getElementById('txtCycles').value,event)")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Function GetEarlierDate(LogDate As String,
									OccurrenceDate As String) As String

		Try

			If CDate(New SmartDate(LogDate).ToString) < CDate(New SmartDate(OccurrenceDate).ToString) Then
				Return New SmartDate(LogDate).ToString
			Else
				Return New SmartDate(OccurrenceDate).ToString
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Sub ControlVisibilityForAttachment()

		Try

			If FileAttach IsNot Nothing Then

				If FileAttach.Size > 0 Then 'change from  to current condition

					attachmentICN.Visible = True
					Dim OpenFrom As String = Request.QueryString("OpenFromWatchDiscrepanciesLink")
					If OpenFrom = "WatchDiscrepanciesLink" Then
						btnDelAttach.Enabled = False
					Else
						btnDelAttach.Enabled = True
					End If

				Else
					attachmentICN.Visible = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SaveAttachment()

		Try

			FileAttach.ReferenceID = DiscrepancyCorrectiveAction.ID

			If FileAttach.Size > 0 Then

				Try

					FileAttach.Save()
					Session("mFileAttach") = FileAttach

				Catch ex As Exception

					ScriptManager.RegisterClientScriptBlock(Me,
															[GetType],
															"",
															MessageBox.Show(ex.InnerException.ToString,
																				   False),
															True)

				End Try

			Else

				If (Not DiscrepancyCorrectiveAction.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAttachment(FileAttach.ID, DiscrepancyCorrectiveAction.ID)
				End If

				IsAttachmentDeleted = False
				Session("IsAttachmentDeleted") = IsAttachmentDeleted

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ViewImage()

		Try

			AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

			ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetLabelsAndVisibility()

		Try

			lblDetailLegend.Text = $"{Prefix} Detail"
			lblDefectReportNo.Text = $"{Prefix} No."
			lblDefect.Text = $"{Prefix} Detail"
			lblMELLabType.Text = $"{Prefix} Category."

			txtDefectReportNo.ToolTip = $"Enter {Prefix} Text."
			txtNo.ToolTip = $"Enter {Prefix} No."
			txtDefect.ToolTip = $"Enter {Prefix} Description."
			txtReportedBy.ToolTip = $"Enter {Prefix} Reported By."

			btnSave.ToolTip = $"Save {Prefix} Details."
			btnBack.ToolTip = $"Close {Prefix} Details screen."

			rfvText.ErrorMessage = $"{Prefix} Text is Required."
			rfvDefect.ErrorMessage = $"{Prefix} is Required."

			phDiscrepancyCategoryAndReliability.Visible = (TransTypeID = 115)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Validation(s) "

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Dim CustomValidator As CustomValidator
		CustomValidator = CType(s, CustomValidator)

		Try

			If CustomValidator.ControlToValidate = "cmbLogNo" AndAlso (TransTypeID <> 116) Then

				If cmbLogNo.SelectedIndex = 0 Then
					CustomValidator.ErrorMessage = "Please select a Log."
					e.IsValid = False
				Else
					e.IsValid = True
				End If

				CustomValidator.ValidationGroup = "a"

			ElseIf CustomValidator.ControlToValidate = "cmbRectifiedLogNo" Then

				If (cmbInvestigation.SelectedIndex = 1 And txtRectifiedDate.Text = "") Then
					CustomValidator.ErrorMessage = "Please select the Rectification Date "
					e.IsValid = False
				ElseIf (cmbInvestigation.SelectedIndex = 1 And txtRectifiedDate.Text <> "" And cmbRectifiedLogNo.SelectedIndex = 0) Then
					CustomValidator.ErrorMessage = "Select Rectified Log No."
					e.IsValid = False
				ElseIf (cmbInvestigation.SelectedIndex = 1 And txtRectifiedDate.Text <> "") Then

					If CDate(txtRectifiedDate.Text.ToString) < CDate(txtDateofOccurrence.Text.ToString) Then
						CustomValidator.ErrorMessage = "Rectified Date should be equal or later to Occurrence Date."
						e.IsValid = False
					End If

				Else
					e.IsValid = True
				End If

				CustomValidator.ValidationGroup = "2"

			ElseIf CustomValidator.ControlToValidate = "cmbATAChapter" Then

				If (chkIsInReliability.Checked = True And cmbATAChapter.SelectedIndex = 0) Then
					CustomValidator.ErrorMessage = "Select the ATA Chapter as it is to be considered in Reliability"
					e.IsValid = False
				Else
					e.IsValid = True
				End If

				CustomValidator.ValidationGroup = "2"

			ElseIf CustomValidator.ControlToValidate = "cmbMELCategory" Then

				If (rdbMEL.Checked = True) And (cmbMELCategory.SelectedIndex = 0) Then
					CustomValidator.ErrorMessage = "Select the " & IIf(AppSettings("MELSnagNomenclature") = "True", "ADD", "MEL") & " category" 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
					e.IsValid = False
				Else
					e.IsValid = True
				End If

				CustomValidator.ValidationGroup = "2"

			ElseIf CustomValidator.ControlToValidate = "txtExtensionInDays" Then

				If (chkExtensionApplied.Checked = True And (txtExtensionInDays.Text = "0" Or txtExtensionInDays.Text = "")) Then
					CustomValidator.ErrorMessage = "Extension days should be greater than zero"
					e.IsValid = False
				Else
					e.IsValid = True
				End If

				CustomValidator.ValidationGroup = "2"

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			MachineID = Session("MachineID").ToString
			TempAssemblyList = AssemblyList.GetAssemblyList(1, MachineID)
			Session("TempAssemblyList") = TempAssemblyList

			ATAList = ATAList.GetATAList(ATANomenclature:="", AddTopItem:="(SELECT)")
			Session("ATAList") = ATAList
			cmbATAChapter.DataSource = ATAList

			Dim mDate As Date

			If txtDateofOccurrence.Text = "" And DiscrepancyCorrectiveAction.IsNew Then
				mDate = Today.Date
			ElseIf Not DiscrepancyCorrectiveAction.IsNew Then
				mDate = CDate(DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted)
			ElseIf txtDateofOccurrence.Text <> "" Then
				mDate = CDate(txtDateofOccurrence.Text)
			End If

			CalFromDate = IIf(CalFromDate = "", mDate.Date.AddDays(-1), CalFromDate)
			CalToDate = IIf(CalToDate = "", mDate.Date.ToString, CalToDate)

			If DiscrepancyCorrectiveAction IsNot Nothing And Not DiscrepancyCorrectiveAction.IsNew Then

				Dim LogDetail As Log
				LogDetail = Log.GetLog(ID:=DiscrepancyCorrectiveAction.LogID)

				ReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=CalFromDate.ToString,
																	  EndDate:=CalToDate.ToString,
																	  AssemblyID:=TempAssemblyList(0).ID.ToString,
																	  MachineID:=DiscrepancyCorrectiveAction.MachineID.ToString,
																	  CalculateTotal:=False, ,
																	  StatusSelectLog:=1, , , ,
																	  AddTopItem:="(SELECT)",
																	  IsFromMEL:=True,
																	  LogID:=LogDetail.ID.ToString,
																	  SkipVoidLog:=True)

			ElseIf (Session("ForDateOfOccurrence") = "ForDateOfOccurrence") Then

				Session.Remove("ForDateOfOccurrence")
				ReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
																	  EndDate:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
																	  AssemblyID:=TempAssemblyList(0).ID.ToString,
																	  MachineID:=DiscrepancyCorrectiveAction.MachineID.ToString,
																	  CalculateTotal:=False, ,
																	  StatusSelectLog:=0, , , ,
																	  AddTopItem:="(SELECT)",
																	  IsFromMEL:=True, ,
																	  SkipVoidLog:=True)

			Else

				ReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=CalFromDate,
																	  EndDate:=CalToDate,
																	  AssemblyID:=TempAssemblyList(0).ID.ToString,
																	  MachineID:=MachineID,
																	  CalculateTotal:=False, ,
																	  StatusSelectLog:=1, , , ,
																	  AddTopItem:="(SELECT)",
																	  IsFromMEL:=True, ,
																	  SkipVoidLog:=True)
			End If

			cmbLogNo.DataSource = ReportLogRegister
			Session("ReportLogRegister") = ReportLogRegister

			upnlLogNo.Update()

			cmbMELCategory.DataSource = MELCategoryList.GetMELCategoryList(AddTopItem:="(SELECT)")
			cmbMELCategory.DataBind()

			cmbPartNo.Items.Clear()

			If DiscrepancyCorrectiveAction IsNot Nothing And Not DiscrepancyCorrectiveAction.IsNew Then

				MELSnagPartList = MELSnagPartList.GePartList(CurrentDate:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
															 MachineID:=MachineID,
															 AddTopItem:="(SELECT)")

			Else

				MELSnagPartList = MELSnagPartList.GePartList(CurrentDate:=txtDateofOccurrence.Text,
															 MachineID:=MachineID,
															 AddTopItem:="(SELECT)")
			End If

			cmbPartNo.DataSource = MELSnagPartList
			Session("MELSnagPartList") = MELSnagPartList
			cmbPartNo.DataBind()

			If Not MELSnagPartList.Contains(DiscrepancyCorrectiveAction.PartNo) Then DiscrepancyCorrectiveAction.PartID = Guid.Empty

			If DiscrepancyCorrectiveAction.IsMEL Then
				cmbATAChapter.Enabled = False
				cmbSubATAList.Enabled = False
			Else
				cmbATAChapter.Enabled = True
				cmbSubATAList.Enabled = True
			End If

			If DiscrepancyCorrectiveAction IsNot Nothing AndAlso Not DiscrepancyCorrectiveAction.IsNew Then

				Dim LogDetail As Log = Log.GetLog(ID:=DiscrepancyCorrectiveAction.LogID)

				Dim StartDate As String
				If TransTypeID = 116 Then
					StartDate = DiscrepancyCorrectiveAction.DateOfOccurrence.ToString
				Else
					StartDate = GetEarlierDate(LogDate:=LogDetail.Date.ToString,
											   OccurrenceDate:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString)
				End If

				RectifiedReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=StartDate,
																			   EndDate:="1/1/2100",
																			   AssemblyID:=TempAssemblyList(0).ID.ToString, MachineID,
																			   CalculateTotal:=False, ,
																			   StatusSelectLog:=0, , , ,
																			   AddTopItem:="(SELECT)",
																			   IsFromMEL:=True,
																			   LogID:=DiscrepancyCorrectiveAction.LogID.ToString)

				LogDetail = Nothing

			End If

			cmbRectifiedLogNo.DataSource = RectifiedReportLogRegister
			Session("RectifiedReportLogRegister") = RectifiedReportLogRegister

			'Added By Vikrant On 02-Apr-2013 For ALL01042013
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

			SubATAList = SubATAList.GetSubATAList(ATAID:=DiscrepancyCorrectiveAction.ATAChapterID,
												  SubATANomenclature:="",
												  AddTopItem:="(SELECT)")
			cmbSubATAList.DataSource = SubATAList
			Session("SubATAList") = SubATAList
			'End

			'Added By Vikrant On 02-Sept-2014 For All04092014
			AssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=0,
																   MachineID:=MachineID.ToString,
																   InstalledOn:=DiscrepancyCorrectiveAction.DateOfOccurrence.ToString,
																   AddTopItem:="",
																   IsInstalled:=True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList
			'End

			cmbIncidentType.DataSource = IncidentTypeList.GetIncidentTypeList() 'Added By Prashant On 23-Nov-2021 ALL23112021
			cmbIncidentType.DataBind()

			BindLicenseNo() 'MLNo

			DataBind()
			If cmbATAChapter.SelectedIndex > 0 Then cmbSubATAList.SelectedValue = DiscrepancyCorrectiveAction.SubATAID.ToString

			cmbRectifiedLogNo.SelectedValue = DiscrepancyCorrectiveAction.RectifiedLogID.ToString

			If DiscrepancyCorrectiveAction IsNot Nothing Then

				txtDateofOccurrence.Text = IIf(DiscrepancyCorrectiveAction.DateOfOccurrence Is DBNull.Value,
											   "",
											   DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted)

				txtDueDate.Text = IIf(DiscrepancyCorrectiveAction.DateValue Is DBNull.Value Or DiscrepancyCorrectiveAction.FrequencyInDays = 0,
									  "",
									  DiscrepancyCorrectiveAction.DateValue)

				txtRectifiedDate.Text = IIf(DiscrepancyCorrectiveAction.RectifiedDateFormatted Is DBNull.Value,
											"",
											DiscrepancyCorrectiveAction.RectifiedDateFormatted)

				cmbLogNo.SelectedValue = DiscrepancyCorrectiveAction.LogID.ToString

				If DiscrepancyCorrectiveAction.IsMEL Then
					txtMELDescription.Text = DiscrepancyCorrectiveAction.MELDescription
				Else
					txtMELDescription.Text = DiscrepancyCorrectiveAction.DeviationDescription
				End If

				If Session("IsFromLog") = True Then txtDateofOccurrence.Enabled = False

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

			AddAttributes()

			TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
							  CInt(Request.QueryString("TransTypeID")),
 							  115)

			Prefix = IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")

			Session("TransTypeID") = TransTypeID

			If Not IsPostBack And Session("sender") = "" Then

				If txtDefectReportNo.Enabled = True Then
					SetFocus(txtDefectReportNo)
				End If

				BackPage = Request.QueryString("BackPage")
				Session("BackPage") = BackPage

				If DiscrepancyCorrectiveAction.IsNew Then

					DiscrepancyCorrectiveAction.DefectReportNo = GetDefectNo()
					If Session("IsFromLog") = True Then

						DiscrepancyCorrectiveAction.DateOfOccurrence = Session("DateOfOccurrence")
						txtDateofOccurrence.Text = DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted
						txtDateofOccurrence.Enabled = False
						Session.Remove("DateOfOccurrence")
						cmbLogNo.SelectedValue = DiscrepancyCorrectiveAction.LogID.ToString

					Else

						Dim TodayDate As New SmartDate(Today.Date.ToString)
						DiscrepancyCorrectiveAction.DateOfOccurrence = TodayDate.FormattedText
						txtDateofOccurrence.Text = TodayDate.FormattedText

					End If

				Else

					DiscrepancyCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(LogID:=DiscrepancyCorrectiveAction.LogID.ToString)
					Session("MELSnagCorrectiveActionLog") = DiscrepancyCorrectiveActionLog
					DiscrepancyCorrectiveAction.DefectReportNo = DiscrepancyCorrectiveAction.DefectReportNo
					DiscrepancyCorrectiveAction.No = DiscrepancyCorrectiveAction.No

				End If

				DataFieldBind()
				ControlVisibilityAfterEdit()
				txtFrequencyInDay.DataBind()

				If cmbLogNo.SelectedIndex > 0 Then
					lnkCheckStatus.Enabled = True
				End If

				If DiscrepancyCorrectiveAction.IsNew Then

					txtFrequencyInDay.Text = ""
					txtFrequencyInHours.Text = ""
					txtCycles.Text = ""
					txtFrequencyInHours.Enabled = False
					txtFrequencyInDay.Enabled = False
					txtCycles.Enabled = False
					cmbMELCategory.Enabled = False
					rdbMEL.Enabled = True
					rdbCDL.Enabled = True

				Else

					If DiscrepancyCorrectiveAction.InvestigationStatus = False And DiscrepancyCorrectiveAction.TotalTroubleShootCount = 0 Then

						rdbMEL.Enabled = True
						rdbCDL.Enabled = True
						lnkMEL.Enabled = True
						rdbCDL.Enabled = True

					Else

						rdbMEL.Enabled = False
						rdbCDL.Enabled = False
						lnkMEL.Enabled = False
						rdbCDL.Enabled = False

					End If

				End If

				'MLNo
				SetLicenseCount()
				UserNameForLicenseList = User.Identity.Name
				Session("UserNameForLicenseList") = UserNameForLicenseList

			End If

			If Session("IsFromLog") = True Then

				If cmbInvestigation.SelectedIndex > 0 Or txtAction.Text <> "" Or cmbATAChapter.SelectedIndex > 0 Then
					tabMELLogDetailsContainer.Attributes("style") = "display: block"
				Else
					tabMELLogDetailsContainer.Attributes("style") = "display: none"
				End If

				phShowVerificationDet.Visible = True
				cmbLogNo.Enabled = False

			Else

				tabMELLogDetailsContainer.Attributes("style") = "display: block"
				phShowVerificationDet.Visible = False
				cmbLogNo.Enabled = (DiscrepancyCorrectiveAction.IsNew) 'IsNew condition added as per discussion with Sir and Abhijit, to lock once record is saved.

			End If

			ControlVisibility()
			SetTitle()
			SetLabelsAndVisibility()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Function Save() As Boolean

		Try

			DiscrepancyCorrectiveAction.Save()
			SaveAttachment()

			txtNo.DataBind()
			upnlMELSnagDetails.Update()

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

			If DiscrepancyCorrectiveAction.IsNew = False Then
				btnPrint.Enabled = True
			End If

			mMELSnagDetail = $"{DiscrepancyCorrectiveAction.DefectNo} 
							   Dated : {DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted}
                               Log No. {DiscrepancyCorrectiveAction.LogNo}"

			MarkLog(Action.Save,
					"DiscrepancyAction",
					mMELSnagDetail,
					ErrorType.NoError,
					DiscrepancyCorrectiveAction.ID,
					EventLogID)

			lnkbtnCreateWorkOrder.Visible = (Not DiscrepancyCorrectiveAction.InvestigationStatus And Not DiscrepancyCorrectiveAction.IsNew)
			SetTitle()

			Return True

		Catch ex As SqlException

			If ex.Number = 8145 Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
								MSGBox.Message_Text.ProcedureError,
								"",
								MsgBoxStyle.OkOnly,
								"")

			ElseIf ex.Number = 2627 Or ex.Number = 2601 Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
								MSGBox.Message_Text.Duplicate,
								"",
								MsgBoxStyle.OkOnly,
								"")

			ElseIf ex.Number = 547 Then

				MarkLog(Action.Delete,
						"DiscrepancyAction",
						"Can't delete : This is Currently in use",
						ErrorType.NoError,
						DiscrepancyCorrectiveAction.ID,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
								MSGBox.Message_Text.ReferenceDelete,
								"",
								MsgBoxStyle.OkOnly,
								"")

			End If

		End Try

	End Function

	Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.[New], Action.Edit},
																	MSGBoxSender:="Authorization") Then

				Exit Sub

			End If

			Page.Validate()

			If Not IsValid Then

				upnlErrorList.Update()
				upnlVerificationDetailsErrors.Update()
				upnlRectificationDetailsErrors.Update()
				Exit Sub

			End If

			If cmbInvestigation.SelectedIndex = 0 And txtAction.Text <> "" Then

				Dim MessageText As String = $"{IIf(TransTypeID = 116,
												   "Cabin Defect should be Open Or Closed. Please select one from it.",
												   "Discrepancy should be Deferred / AOG Or Closed. Please select one from it.")}"
				MSGBoxCtrl.Show("Alert!",
								MessageText,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			ElseIf cmbInvestigation.SelectedIndex = 2 AndAlso
				   (rdbMEL.Checked = False And rdbCDL.Checked = False) AndAlso
				   (TransTypeID = 115) Then

				MSGBoxCtrl.Show("Alert!",
								"Discrepancy should either be MEL or Deferred.",
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			If chkExtensionApplied.Checked = True Then

				If txtExtensionApprovalNo.Text = "" Then

					MSGBoxCtrl.Show("Alert!",
									"Please Enter Approval Details.",
									"",
									MsgBoxStyle.OkOnly,
									"")

					Exit Sub

				End If

			End If

			SetObject()

			If DiscrepancyCorrectiveAction.IsValid Then

				Try

					SetSession()

					'Added by Utkarsh ON 27-Feb-2013 FOR All27022013
					If DiscrepancyCorrectiveAction.InvestigationStatus Then

						If ((CDate(DiscrepancyCorrectiveAction.DateOfOccurrence) <= CDate(DiscrepancyCorrectiveAction.RectifiedDate)) AndAlso
							(RectifiedReportLogRegister.Item(New Guid(cmbRectifiedLogNo.SelectedValue)).IntLogNo < ReportLogRegister.Item(New Guid(cmbLogNo.SelectedValue)).IntLogNo)) Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
											MSGBox.Message_Text.Custom,
											"Rectification can be done on same or later the occurrence Log (TLP).",
											MsgBoxStyle.OkOnly,
											"")

							Exit Sub

						End If

					End If
					'End

					'Added By Prashant 2-Jan-2014  --ALL02012014-1

					If Save() Then

						DataFieldBind()
						ControlVisibility()
						lnkbtnCreateWorkOrder.Visible = (Not DiscrepancyCorrectiveAction.InvestigationStatus And Not DiscrepancyCorrectiveAction.IsNew)

						If cmbInvestigation.SelectedValue = "1" Then

							ScriptManager.RegisterStartupScript(Me,
																[GetType],
																"Disable Controls On Close",
																"disableControlsOnClose();",
																True)

						End If

						MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
										MSGBox.Message_Text.SavedSuccessFully,
										"",
										MsgBoxStyle.OkOnly,
										"")

						upnlMMELDetails.Update()
						upnlMELSnagDetails.Update()
						upnlTitle.Update()
						upnlCreateWO.Update()
						upnlHeaderButtons.Update()
						upnlMELDeviation.Update()
						upnlInvestigation.Update()
						upnlLinks.Update()

						Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

					End If

				Catch ex As SqlException

					If ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										"",
										MsgBoxStyle.OkOnly,
										"")


					ElseIf ex.Number = 2627 Or ex.Number = 2601 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										"",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 547 Then

						MarkLog(Action.Delete,
								"DiscrepancyAction",
								"Can't delete : This is Currently in use",
								ErrorType.NoError,
								DiscrepancyCorrectiveAction.ID,
								EventLogID)

						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										"",
										MsgBoxStyle.OkOnly,
										"")

					Else

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.Duplicate,
										"",
										MsgBoxStyle.OkOnly,
										"")

					End If

				End Try

			Else

				'Modified by Harsh on 5th March 2024 --
				'Based on the Group Name added in Description bifurcate the Validation Messages to show on UI accordingly
				Dim RectificationDetailsErrorMessage As String = ""
				Dim DiscrepancyDetailsErrorMessage As String = ""

				For j As Integer = 0 To DiscrepancyCorrectiveAction.GetBrokenRulesCollection.Count - 1

					If DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",").Length > 1 Then

						If DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(1).ToString.Trim.Equals("RectificationDetails", StringComparison.CurrentCultureIgnoreCase) Then
							RectificationDetailsErrorMessage += DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(0).ToString() + "<BR>"
						ElseIf DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(1).ToString.Trim.Equals("DiscrepancyDetails", StringComparison.CurrentCultureIgnoreCase) Then
							DiscrepancyDetailsErrorMessage += DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description.Split(",")(0).ToString() + "<BR>"
						End If

					Else
						strMsg = strMsg + DiscrepancyCorrectiveAction.GetBrokenRulesCollection(j).Description + "<BR>"
					End If

				Next


				If Not IsNothing(RectificationDetailsErrorMessage) Then
					cvRectificationDetails.ErrorMessage = RectificationDetailsErrorMessage
					cvRectificationDetails.IsValid = DiscrepancyCorrectiveAction.IsValid
				End If

				If Not IsNothing(DiscrepancyDetailsErrorMessage) Then
					cvDiscrepancyDetails.ErrorMessage = DiscrepancyDetailsErrorMessage
					cvDiscrepancyDetails.IsValid = DiscrepancyCorrectiveAction.IsValid
				End If

				If strMsg.Trim <> "" Then

					cvFrequencyInHours.ErrorMessage = strMsg
					cvDefectList.ErrorMessage = strMsg
					cvFrequencyInHours.IsValid = DiscrepancyCorrectiveAction.IsValid

				End If

				upnlErrorList.Update()
				upnlVerificationDetailsErrors.Update()
				upnlRectificationDetailsErrors.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub PartNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPartNo.SelectedIndexChanged

		Try

			If cmbPartNo.SelectedIndex <= 0 Then

				txtPartNo.Text = ""
				txtDescription.Text = ""
				txtSerialNo.Text = ""
				txtHrsofComp.Text = ""
				txtDescription.ReadOnly = False
				txtDescription.BackColor = Color.FromKnownColor(KnownColor.White)
				txtPartNo.ReadOnly = False
				txtPartNo.BackColor = Color.FromKnownColor(KnownColor.White)
				txtSerialNo.ReadOnly = False
				txtSerialNo.BackColor = Color.FromKnownColor(KnownColor.White)
				txtHrsofComp.ReadOnly = False
				txtHrsofComp.BackColor = Color.FromKnownColor(KnownColor.White)

			Else

				txtPartNo.Text = MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Name
				txtDescription.Text = MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").Description
				txtSerialNo.Text = MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").SerialNo
				txtHrsofComp.Text = ""

				Dim HrsCurrentValue As CompCurrentValue = CompCurrentValue.
																GetCompCurrentValue(MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").CompStatusID,
																					Today.Date.ToString,
																					1)

				If HrsCurrentValue.Count > 0 Then
					txtHrsofComp.Text = New Period(1,
												   HrsCurrentValue(0).CurrentValueDec,
												   0,
												   False,
												   False).TextFormatted
				Else
					txtHrsofComp.Text = ""
				End If


				Dim CycCurrentValue As CompCurrentValue = CompCurrentValue.
																GetCompCurrentValue(MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").CompStatusID,
																					Today.Date.ToString, 3)
				If CycCurrentValue.Count > 0 Then

					If txtHrsofComp.Text <> "" Then

						txtHrsofComp.Text = txtHrsofComp.Text + "," + New Period(3,
																				 CycCurrentValue(0).CurrentValueDec,
																				 0,
																				 False,
																				 False).TextFormatted
					Else

						txtHrsofComp.Text = New Period(3,
													   CycCurrentValue(0).CurrentValueDec,
													   0,
													   False,
													   False).TextFormatted

					End If

				Else
					txtHrsofComp.Text = txtHrsofComp.Text.TrimEnd(New Char() {","})
				End If



				'Added By Saylee on 8-Aug-2019
				If AssemblyList.Count > 0 And rdbMEL.Checked Then

					If AssemblyList.Contains(MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID, "") Then
						cmbAssembly.SelectedValue = MELSnagPartList.Item(New Guid(cmbPartNo.SelectedValue), "").AssemblyStatusID.ToString
						upnlMMELDetails.Update()
					End If

				End If
				'***************************************
				If rdbMEL.Checked = False Then

					cmbATAChapter.SelectedValue = MELSnagPartList(New Guid(cmbPartNo.SelectedValue.ToString), "").CompStatusATAID.ToString
					cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

					If cmbATAChapter.SelectedIndex > 0 Then

						SubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue),
															   "",
															   "(SELECT)")
						cmbSubATAList.DataSource = SubATAList
						cmbSubATAList.DataBind()
					End If

				End If

				txtDescription.ReadOnly = True
				txtPartNo.ReadOnly = True
				txtSerialNo.ReadOnly = True
				txtHrsofComp.ReadOnly = True
				txtDescription.BackColor = Color.FromName("#E0E0E0")
				txtPartNo.BackColor = Color.FromName("#E0E0E0")
				txtSerialNo.BackColor = Color.FromName("#E0E0E0")
				txtHrsofComp.BackColor = Color.FromName("#E0E0E0")

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

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub DateOfOccurrence_Changed(sender As Object, e As EventArgs) Handles txtDateofOccurrence.TextChanged

		Try

			Session("DateOfOccurrence") = txtDateofOccurrence.Text

			'Here if New then consider Occurrence Date for binding else
			'if Old: then get Lesser date from LogDate or Occurrence date
			Dim mDate As Date

			If txtDateofOccurrence.Text = "" Then
				mDate = Today.Date
			Else
				mDate = CDate(txtDateofOccurrence.Text)
			End If

			CalFromDate = IIf(CalFromDate = "", mDate.Date.AddDays(-1), CalFromDate)
			CalToDate = IIf(CalToDate = "", mDate.Date.ToString, CalToDate)

			If DiscrepancyCorrectiveAction.IsNew Then

				ReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=CalFromDate,
																	  EndDate:=CalToDate,
																	  AssemblyID:=TempAssemblyList(0).ID.ToString,
																	  MachineID:=MachineID,
																	  CalculateTotal:=False, ,
																	  StatusSelectLog:=1, , , ,
																	  AddTopItem:="(SELECT)",
																	  IsFromMEL:=True, ,
																	  SkipVoidLog:=True)
			Else

				Dim TmpLogDetail As Log
				TmpLogDetail = Log.GetLog(DiscrepancyCorrectiveAction.LogID)
				ReportLogRegister = ReportLogRegister.GetRectifiedLog(StartDate:=GetEarlierDate(LogDate:=TmpLogDetail.Date.ToString,
																								OccurrenceDate:=CalFromDate),
																	  EndDate:=CalToDate,
																	  AssemblyID:=TempAssemblyList(0).ID.ToString,
																	  MachineID:=MachineID,
																	  CalculateTotal:=False, ,
																	  StatusSelectLog:=1, , , ,
																	  AddTopItem:="(SELECT)",
																	  IsFromMEL:=True, ,
																	  SkipVoidLog:=True)
			End If

			cmbLogNo.DataSource = ReportLogRegister
			Session("ReportLogRegister") = ReportLogRegister
			cmbLogNo.DataBind()
			upnlLogNo.Update()
			DiscrepancyCorrectiveAction.DateOfOccurrence = txtDateofOccurrence.Text
			DiscrepancyCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			DiscrepancyCorrectiveAction.DateOfOccurrence = txtDateofOccurrence.Text
			DiscrepancyCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text)
			DiscrepancyCorrectiveAction.FrequencyInCycles = txtCycles.Text

			'Added By Vikrant On 02-Sept-2014 For All04092014
			AssemblyList = AssemblyList.GetAssemblyListForComboBox(0,
																	MachineID.ToString,
																	txtDateofOccurrence.Text,
																	"",
																	True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList
			cmbAssembly.DataBind()
			upnlAssembly.Update()
			'End
			cmbRectifiedLogNo.DataSource = RectifiedReportLogRegister
			Session("RectifiedReportLogRegister") = RectifiedReportLogRegister
			cmbRectifiedLogNo.DataBind()
			upnlRectifiedCombo.Update()

			If DiscrepancyCorrectiveAction IsNot Nothing And Not DiscrepancyCorrectiveAction.IsNew Then

				MELSnagPartList = MELSnagPartList.GePartList(DiscrepancyCorrectiveAction.DateOfOccurrence.ToString, MachineID,
															  "(SELECT)")

			Else

				MELSnagPartList = MELSnagPartList.GePartList(txtDateofOccurrence.Text, MachineID,
															  "(SELECT)")

			End If

			cmbPartNo.DataSource = MELSnagPartList
			Session("MELSnagPartList") = MELSnagPartList
			cmbPartNo.DataSource = MELSnagPartList
			cmbPartNo.DataSource = MELSnagPartList
			cmbPartNo.DataBind()

			If Not MELSnagPartList.Contains(DiscrepancyCorrectiveAction.PartNo) Then

				DiscrepancyCorrectiveAction.PartID = Guid.Empty
				txtDescription.Text = ""
				txtPartNo.Text = ""
				txtSerialNo.Text = ""

			Else
				cmbPartNo.SelectedValue = MELSnagPartList(txtPartNo.Text).ID.ToString
			End If

			upnlPartNoCombo.Update()
			upnlPartNo.Update()

			'Added by Saylee on 25-Nov-2014 to reset rectification details on date change
			rectDate.Visible = (cmbInvestigation.SelectedValue = "1")
			phWatchListDetails.Visible = (TransTypeID = 115)

			If RectifiedReportLogRegister IsNot Nothing Then cmbRectifiedLogNo.SelectedIndex = 0

			cmbRectifiedLogNo.Enabled = False
			txtRectificationSector.Text = ""
			DiscrepancyCorrectiveAction.RectifiedLogID = Guid.Empty
			upnlClose.Update()
			upnlRectifiedDate.Update()
			upnlRectifiedCombo.Update()
			upnlLogNo.Update()
			upnlDueDate.Update()
			upnlMELSnagDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub LogNo_Changed(sender As Object, e As EventArgs) Handles cmbLogNo.SelectedIndexChanged

		Try

			If cmbLogNo.SelectedIndex > 0 Then

				lnkCheckStatus.Enabled = True
				DiscrepancyCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbLogNo.SelectedValue.ToString)
				Session("MELSnagCorrectiveActionLog") = DiscrepancyCorrectiveActionLog

				With DiscrepancyCorrectiveActionLog

					txtSector.Text = DiscrepancyCorrectiveActionLog.Item(0).DestinationName
					Session("TmpLogDate") = DiscrepancyCorrectiveActionLog.Item(0).LogDate

					If DiscrepancyCorrectiveActionLog.Item(0).FinalLandings = "" Then
						txtLastMajorCheck.Text = DiscrepancyCorrectiveActionLog.Item(0).FinalHours + " H"
					Else
						txtLastMajorCheck.Text = DiscrepancyCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + DiscrepancyCorrectiveActionLog.Item(0).FinalLandings + " L"
					End If

					If DiscrepancyCorrectiveActionLog.Item(0).FinalCycles = "" Then
						txtLastMajorCheck.Text = txtLastMajorCheck.Text
					Else
						txtLastMajorCheck.Text = txtLastMajorCheck.Text + ", " + DiscrepancyCorrectiveActionLog.Item(0).FinalCycles + " C"
					End If

				End With

				If Not DiscrepancyCorrectiveAction.IsNew Then

					If DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCycles = "" Then
						txtCycles.ReadOnly = True
					Else
						txtCycles.ReadOnly = False
					End If

				End If

				FillRectifiedCombo()
				upnlRectifiedDate.Update()
				upnlRectifiedCombo.Update()
				upnlSector.Update()
				upnlLastMajorCheck.Update()
				upnlFreq.Update()

			Else

				txtSector.Text = ""
				txtLastMajorCheck.Text = ""
				Dim todayDate As SmartDate = New SmartDate(Today.Date.ToString)
				txtDateofOccurrence.Text = todayDate.FormattedText  '---  All05022013-1 Added by Prashant 5-Feb-2013
				upnlSector.Update()
				upnlLastMajorCheck.Update()

			End If

			'Added By Vikrant On 02-Sept-2014 For All04092014
			AssemblyList = AssemblyList.GetAssemblyListForComboBox(0,
																   MachineID.ToString,
																   txtDateofOccurrence.Text,
																   "",
																   True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList
			cmbAssembly.DataBind()
			'End

			If cmbLogNo.Enabled = True Then
				SetFocus(cmbLogNo)
			End If

			upnlAssembly.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub RectifiedLogNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRectifiedLogNo.SelectedIndexChanged

		Try

			If cmbRectifiedLogNo.SelectedIndex > 0 Then

				DiscrepancyCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(cmbRectifiedLogNo.SelectedValue.ToString)
				Session("MELSnagCorrectiveActionLog") = DiscrepancyCorrectiveActionLog

				With DiscrepancyCorrectiveActionLog

					txtRectificationSector.Text = DiscrepancyCorrectiveActionLog.Item(0).DestinationName

				End With

			Else
				txtRectificationSector.Text = ""
			End If

			If cmbRectifiedLogNo.Enabled = True Then
				SetFocus(cmbRectifiedLogNo)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub Close_CheckedChanged(sender As Object, e As EventArgs) Handles chkClose.CheckedChanged

		Try

			'If cmbLogNo.SelectedIndex = 0 And chkClose.Checked = True Then

			'	MSGBoxCtrl.Show("Alert!",
			'					"Please select the Log",
			'					"",
			'					MsgBoxStyle.OkOnly,
			'					"Close")
			'	Exit Sub

			'End If

			If chkClose.Checked Then

				txtRectifiedDate.ReadOnly = False
				cmbRectifiedLogNo.Enabled = True

			Else

				txtRectifiedDate.Text = ""
				txtRectifiedDate.ReadOnly = True
				cmbRectifiedLogNo.SelectedIndex = 0
				cmbRectifiedLogNo.Enabled = False
				txtRectificationSector.Text = ""
				DiscrepancyCorrectiveAction.RectifiedLogID = Guid.Empty

			End If

			upnlRectifiedDate.Update()
			upnlRectifiedCombo.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub FrequencyInDayChanged(sender As Object, e As EventArgs) Handles txtFrequencyInDay.TextChanged

		Try

			If txtFrequencyInDay.Text = "0" Or txtFrequencyInDay.Text = "" Then
				txtDueDate.Text = ""
				Exit Sub
			End If


			txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
			DiscrepancyCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text.Trim)  'Changed By Utkarsh on 13-Aug-2013 for ALL13082013-2
			txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
			upnlDueDate.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try
	End Sub

	Private Sub FrequencyInHoursChanged(sender As Object, e As EventArgs) Handles txtFrequencyInHours.TextChanged

		Try

			If txtFrequencyInHours.Text = "0" Or txtFrequencyInHours.Text = "" Or txtFrequencyInHours.Text = "0:00" Then
				txtDueHrs.Text = ""
				Exit Sub
			End If

			DiscrepancyCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text
			DiscrepancyCorrectiveAction.DueInHrs = New Period(1, DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalHoursDec + DiscrepancyCorrectiveAction.FrequencyInHoursDec, 0, False, False).Value
			txtDueHrs.Text = DiscrepancyCorrectiveAction.DueInHrs
			upnlDueHrs.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub FrequencyInCyclesChanged(sender As Object, e As EventArgs) Handles txtCycles.TextChanged

		Try

			If txtCycles.Text = "0" Or txtCycles.Text = "" Then

				txtDueCycles.Text = ""
				DiscrepancyCorrectiveAction.FrequencyInCycles = txtCycles.Text
				Exit Sub

			End If

			DiscrepancyCorrectiveAction.FrequencyInCycles = txtCycles.Text
			DiscrepancyCorrectiveAction.DueInCycles = New Period(3, DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCyclesDec + DiscrepancyCorrectiveAction.FrequencyInCyclesDec, 0, False, False).Value
			txtDueCycles.Text = DiscrepancyCorrectiveAction.DueInCycles
			upnlDueHrs.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MELCategory_Changed(sender As Object, e As EventArgs) Handles cmbMELCategory.SelectedIndexChanged

		Try

			If cmbMELCategory.SelectedIndex > 0 Then DiscrepancyCorrectiveAction.MELCategoryID = cmbMELCategory.SelectedValue

			txtFrequencyInDay.Text = DiscrepancyCorrectiveAction.FrequencyInDays

			If cmbMELCategory.SelectedIndex = 1 Then

				txtFrequencyInDay.Enabled = True
				DiscrepancyCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text)

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

			ElseIf cmbMELCategory.SelectedIndex = 0 Then

				txtFrequencyInDay.Enabled = False
				txtFrequencyInHours.Enabled = False
				txtCycles.Enabled = False

				txtFrequencyInHours.Text = ""
				txtFrequencyInDay.Text = ""
				txtCycles.Text = ""
				txtDueDate.Text = ""

			Else

				txtFrequencyInDay.Enabled = True

				If txtFrequencyInHours.Text <> "" Then
					txtFrequencyInHours.Text = ""
				End If

				txtFrequencyInHours.Enabled = False
				DiscrepancyCorrectiveAction.FrequencyInHours = txtFrequencyInHours.Text

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

			End If

			upnlFreq.Update()
			upnlDueDate.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		Try

			GetSession()
			Session.Remove("AircraftRegNo")
			Session.Remove("DateOfOccurrence")
			Session.Remove("mlnkCheckStatus")
			Session.Remove("IsAttachmentDeleted")
			Session.Remove("wfDiscrepancyCorrectiveAction")
			Session.Remove("URLFromDueReportPreview")

			SetObject()

			If DiscrepancyCorrectiveAction.IsDirty Then

				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm,
								MSGBox.Message_Text.Save,
								"",
								MsgBoxStyle.YesNo,
								"Close")

			ElseIf Request.QueryString("BackPage1") = "wfnWODetail_AJAX.aspx" Then

				Response.Redirect(Request.QueryString("BackPage1"))

			Else

				mMELSnagDetail = DiscrepancyCorrectiveAction.DefectNo + " Dated : " +
								 DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " +
								 DiscrepancyCorrectiveAction.LogNo

				MarkLog(Action.Close,
						"DiscrepancyAction",
						mMELSnagDetail,
						ErrorType.NoError,
						DiscrepancyCorrectiveAction.ID,
						EventLogID)

				Session("sender") = ""
				Session.Remove("FileAttach")
				'MLNo
				Session.Remove("MaintenanceDoneByEmployees")
				Session.Remove("UserNameForLicenseList")
				'End

				Dim Type As String = Request.QueryString("Type")
				Dim Script As String = IIf(TransTypeID = 115, "CallParentCallback()", "CallParentCallbackForCabinDefect()")

				If Type IsNot Nothing AndAlso Type = "pup" Then

					ScriptManager.RegisterStartupScript(page:=Me,
														type:=[GetType],
														key:="On Close",
														script:=Script,
														addScriptTags:=True)

					Exit Sub

				End If
				'End

				Response.Redirect(Request.QueryString("BackPage"))

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub CheckStatus_Click(sender As Object, e As EventArgs) Handles lnkCheckStatus.Click

		Try

			SetObject()
			DiscrepancyCorrectiveAction.FrequencyInDays = Val(txtFrequencyInDay.Text)
			Session("DateOfOccurrence") = DiscrepancyCorrectiveAction.DateOfOccurrence
			Session("mlnkCheckStatus") = True

			If cmbLogNo.SelectedIndex > 0 Then

				Session("mTempLogID") = cmbLogNo.SelectedValue.ToString
				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenSelectLogWindow",
													"OpenSelectLogWindow()",
													True)

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ATAChapter_Changed(sender As Object, e As EventArgs) Handles cmbATAChapter.SelectedIndexChanged

		Try

			DiscrepancyCorrectiveAction.SubATAID = Guid.Empty
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
			SubATAList = SubATAList.GetSubATAList(New Guid(cmbATAChapter.SelectedValue), "", "(SELECT)")
			cmbSubATAList.DataSource = SubATAList
			cmbSubATAList.DataBind()
			Session("SubATAList") = SubATAList
			upnlSubATA.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub
	'End

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub
	'MLNo

	Private Sub EmployeeLicense_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click

		Try

			If IsValid Then

				SetObject()
				Session("mMaintenanceID") = DiscrepancyCorrectiveAction.ID
				MaintenanceDoneByEmployees = DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees
				Session("mMaintenanceDoneByEmployees") = MaintenanceDoneByEmployees
				Session("MaintenanceDoneOnDate") = DiscrepancyCorrectiveAction.DateOfOccurrence.ToString
				ScriptManager.RegisterClientScriptBlock(Me,
														[GetType],
														"AddEmployeeLicNo",
														"AddEmployeeLicNo();",
														True)
			Else
				upnlErrorList.Update()
				upnlVerificationDetailsErrors.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub CDLMasterChapter_Click(sender As Object, e As EventArgs) Handles hdnimgBtnCDLMasterChapter.Click

		Try

			DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")

			DiscrepancyCorrectiveAction.IsMEL = False
			DiscrepancyCorrectiveAction.MELID = Guid.Empty
			rdbMEL.Checked = False

			If DiscrepancyCorrectiveAction.IsDeviationList = False Then
				rdbCDL.Checked = False
			Else
				rdbCDL.Checked = True
			End If

			txtMELDescription.Text = DiscrepancyCorrectiveAction.DeviationDescription

			If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
				txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
			Else
				txtDueDate.Text = ""
			End If

			If txtFrequencyInHours.Text <> "" And txtFrequencyInHours.Text <> "0" Then

				DiscrepancyCorrectiveAction.DueInHrs = New Period(1,
															   DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalHoursDec +
																	  DiscrepancyCorrectiveAction.FrequencyInHoursDec,
															   0,
															   False,
															   False).Value
				txtDueHrs.Text = DiscrepancyCorrectiveAction.DueInHrs

			Else
				txtDueHrs.Text = ""
			End If

			If txtCycles.Text <> "" And txtCycles.Text <> "0" Then

				DiscrepancyCorrectiveAction.DueInCycles = New Period(3,
																  DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCyclesDec +
																		 DiscrepancyCorrectiveAction.FrequencyInCyclesDec,
																  0,
																  False,
																  False).Value
				txtDueCycles.Text = DiscrepancyCorrectiveAction.DueInCycles

			Else
				txtDueCycles.Text = ""
			End If

			txtCauseofDefect.Text = DiscrepancyCorrectiveAction.CauseOfDefect
			cmbATAChapter.SelectedValue = DiscrepancyCorrectiveAction.ATAChapterID.ToString
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)

			SubATAList = SubATAList.GetSubATAList(DiscrepancyCorrectiveAction.ATAChapterID,
												   "",
												   "(SELECT)")
			cmbSubATAList.DataSource = SubATAList
			cmbSubATAList.DataBind()
			Session("SubATAList") = SubATAList
			upnlSubATA.Update()
			cmbSubATAList.SelectedValue = DiscrepancyCorrectiveAction.SubATAID.ToString
			txtItemSequenceNo.Text = DiscrepancyCorrectiveAction.ItemSequenceNo
			ControlVisibilityAfterEdit()
			txtFrequencyInDay.Focus()
			upnlMMELDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MELMasterChapter_Click(sender As Object, e As EventArgs) Handles hdnimgBtnMELMasterChapter.Click

		Try

			DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
			DiscrepancyCorrectiveAction.CauseOfDefect = ""
			DiscrepancyCorrectiveAction.IsDeviationList = False
			DiscrepancyCorrectiveAction.DeviationListID = Guid.Empty
			rdbCDL.Checked = False

			If DiscrepancyCorrectiveAction.IsMEL = False Then
				rdbMEL.Checked = False
			Else
				rdbMEL.Checked = True
			End If

			cmbMELCategory.SelectedValue = DiscrepancyCorrectiveAction.MELCategoryID
			txtDefect.Text = DiscrepancyCorrectiveAction.Defect
			cmbATAChapter.SelectedValue = DiscrepancyCorrectiveAction.ATAChapterID.ToString
			cmbSubATAList.Enabled = IIf(cmbATAChapter.SelectedIndex > 0, True, False)
			SubATAList = SubATAList.GetSubATAList(DiscrepancyCorrectiveAction.ATAChapterID, "", "(SELECT)")
			cmbSubATAList.DataSource = SubATAList
			cmbSubATAList.DataBind()
			Session("SubATAList") = SubATAList
			upnlSubATA.Update()
			cmbSubATAList.SelectedValue = DiscrepancyCorrectiveAction.SubATAID.ToString
			txtFrequencyInDay.Text = DiscrepancyCorrectiveAction.FrequencyInDays
			txtFrequencyInHours.Text = DiscrepancyCorrectiveAction.FrequencyInHours
			txtCycles.Text = DiscrepancyCorrectiveAction.FrequencyInCycles

			If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
				txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
			Else
				txtDueDate.Text = ""
			End If

			If txtFrequencyInHours.Text <> "" And txtFrequencyInHours.Text <> "0" Then

				DiscrepancyCorrectiveAction.DueInHrs = New Period(1,
															   DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalHoursDec +
																	  DiscrepancyCorrectiveAction.FrequencyInHoursDec,
															   0,
															   False,
															   False).Value

				txtDueHrs.Text = DiscrepancyCorrectiveAction.DueInHrs
			Else
				txtDueHrs.Text = ""
			End If

			If txtCycles.Text <> "" And txtCycles.Text <> "0" Then

				DiscrepancyCorrectiveAction.DueInCycles = New Period(3,
																  DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCyclesDec +
																		 DiscrepancyCorrectiveAction.FrequencyInCyclesDec,
																  0,
																  False,
																  False).Value

				txtDueCycles.Text = DiscrepancyCorrectiveAction.DueInCycles
			Else
				txtDueCycles.Text = ""

			End If

			cmbIncidentType.SelectedValue = DiscrepancyCorrectiveAction.IncidentTypeID
			txtMELDescription.Text = DiscrepancyCorrectiveAction.MELDescription
			txtItemSequenceNo.Text = DiscrepancyCorrectiveAction.ItemSequenceNo
			ControlVisibilityAfterEdit()
			txtFrequencyInDay.Focus()
			upnlMMELDetails.Update()
			upnlMELDeviation.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MaintenanceDoneBy(sender As Object, e As EventArgs) Handles hdnBtnMaintDoneBy.Click

		Try

			For i As Integer = 0 To MaintenanceDoneByEmployees.Count - 1

				Dim ID As Guid = MaintenanceDoneByEmployees(i).ID

				If Not DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Add(MaintenanceDoneByEmployees(i))
				ElseIf DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Contains(ID) Then

					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(ID).LicenceNo = MaintenanceDoneByEmployees(i).LicenceNo
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeID = MaintenanceDoneByEmployees(i).EmployeeID
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(ID).EmployeeName = MaintenanceDoneByEmployees(i).EmployeeName

				End If

			Next

			For j As Integer = 0 To DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count - 1

				If Not MaintenanceDoneByEmployees.Contains(DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(j).ID) Then
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Remove(DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(j).ID, "")
				End If

			Next

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			BindLicenseNo()
			SetLicenseCount() 'MLNo
			upnlLicenceNo.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Protected Sub LicenseNo_TextChanged(sender As Object, e As EventArgs)

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

			If Not DoneByID.Equals(Guid.Empty) Then

				If DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then

					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees(0).EmployeeName = EmpName

				Else

					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Add(DiscrepancyCorrectiveAction.ID,
																			11,
																			DoneByID,
																			LicenseNo,
																			"",
																			EmpName)

				End If

			Else

				If DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.Count > 0 Then
					DiscrepancyCorrectiveAction.MaintenanceDoneByEmployees.RemoveAt(0)
				End If

			End If

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			BindLicenseNo()
			SetLicenseCount()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub
	'End

	Private Sub MELDetail_Click(sender As Object, e As EventArgs) Handles lnkMELDetail.Click

		Try

			Dim mMEL As MEL
			mMEL = MEL.GetMEL(DiscrepancyCorrectiveAction.MELID)
			mMEL.MarkClean()
			Session("mMEL") = mMEL

			ScriptManager.RegisterStartupScript(Me,
											[GetType],
											"OpenMELDetail",
											"OpenMELDetail();",
											True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub DeferredDetail_Click(sender As Object, e As EventArgs) Handles lnkDeferredDetail.Click

		Try

			Dim mDeviationList As DeviationList
			mDeviationList = DeviationList.GetDeviationList(DiscrepancyCorrectiveAction.DeviationListID)
			mDeviationList.MarkClean()
			Session("mDeviationList") = mDeviationList

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenDeferredDetail",
												"OpenDeferredDetail();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ExtensionApplied_CheckedChanged(sender As Object, e As EventArgs) Handles chkExtensionApplied.CheckedChanged

		Try

			If chkExtensionApplied.Checked = True Then

				If txtFrequencyInDay.Text = "" Then
					txtExtensionInDays.Enabled = False
				Else

					If txtFrequencyInDay.Text > 0 Then
						txtExtensionInDays.Enabled = True
					Else
						txtExtensionInDays.Enabled = False
					End If

				End If

				If txtFrequencyInHours.Text = "" Then

					txtExtensionInHours.Enabled = False
				Else

					If Val(txtFrequencyInHours.Text) > 0 Then
						txtExtensionInHours.Enabled = True
					Else
						txtExtensionInHours.Enabled = False
					End If

				End If

				If txtCycles.Text = "" Then
					txtExtensionInCycles.Enabled = False
				Else

					If txtCycles.Text > 0 Then
						txtExtensionInCycles.Enabled = True
					Else
						txtExtensionInCycles.Enabled = False
					End If

				End If

				txtExtensionApprovalNo.Enabled = True

			Else

				txtExtensionInDays.Enabled = False
				txtExtensionInCycles.Enabled = False
				txtExtensionInHours.Enabled = False
				txtExtensionApprovalNo.Enabled = False
				txtExtensionInDays.Text = 0
				txtExtensionApprovalNo.Text = ""
				txtExtensionInHours.Text = ""
				txtExtensionInCycles.Text = ""
				DiscrepancyCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

			End If

			upnlDueDate.Update()
			upnlMMELDetails.Update()
			upnlExtension.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ExtensionInDays_TextChanged(sender As Object, e As EventArgs) Handles txtExtensionInDays.TextChanged

		Try

			If chkExtensionApplied.Checked = True Then

				DiscrepancyCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)
				txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue ' Added by sachin on 21-June-2024

			Else

				txtExtensionInDays.Enabled = False
				txtExtensionApprovalNo.Enabled = False
				txtExtensionInDays.Text = 0
				txtExtensionApprovalNo.Text = ""
				DiscrepancyCorrectiveAction.ExtensionInDays = Val(txtExtensionInDays.Text.Trim)

				If txtFrequencyInDay.Text <> "" And txtFrequencyInDay.Text <> "0" Then
					txtDueDate.Text = DiscrepancyCorrectiveAction.DateValue
				Else
					txtDueDate.Text = ""
				End If

			End If

			upnlDueDate.Update()
			upnlExtension.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetUserMailIDs()

		Try

			Session("UserEmailID") = ModuleList.Item("DiscrepancyAction").SendToMailID
			Session("UserCcEmailID") = ModuleList.Item("DiscrepancyAction").SendCCMailID
			Session("MailsRequire") = ModuleList.Item("DiscrepancyAction").MailsRequire
			Session("SmtpHost") = ModuleList.Item("DiscrepancyAction").SmtpHost
			Session("SmtpPort") = ModuleList.Item("DiscrepancyAction").SmtpPort
			Session("SmtpUser") = ModuleList.Item("DiscrepancyAction").SmtpUser
			Session("SmtpPassword") = ModuleList.Item("DiscrepancyAction").SmtpPassword

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub BtnSendMail_Click(sender As Object, e As EventArgs) Handles btnSendMail.Click

		Dim Str As String
		Try

			SetUserMailIDs()
			Session("btnSendMail") = "btnSendMail"

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenByMainWindow",
												"OpenByMainWindow()",
												True)
		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SendMail_HdnBtn(sender As Object, e As EventArgs) Handles hdnImgBtnSendMail.Click

		Dim MailInfo As String
		Dim mSendMailFile As New SendMailFile

		Try

			MailInfo += ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following New " +
						 $"{Prefix}" +
						 "has been added in FlyPal System and need your attention." + "</font></P></br> ")

			MailInfo += "<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> "

			MailInfo += "<p><font face=""Calibri"">"
			MailInfo += "<b> Aircraft : </b>" + DiscrepancyCorrectiveAction.RegNo + "<b>" + "  Log No : " + "</b>" + DiscrepancyCorrectiveAction.LogNo
			MailInfo += "</font></p>"
			MailInfo += "<p><font face=""Calibri"">"
			MailInfo += ($"<b>{Prefix} No : </b> {DiscrepancyCorrectiveAction.DefectNo}" +
						 $"<b>  Date of Occurrence : </b> {DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted}")
			MailInfo += "</font></p>"
			MailInfo += "<p><font face=""Calibri"">"
			MailInfo += $"<b> {Prefix} Details : </b> {DiscrepancyCorrectiveAction.Defect}"
			MailInfo += "</font></p>"
			MailInfo += "<p><font face=""Calibri"">"
			MailInfo += "<b>" + " Reported By : " + "</b>" + DiscrepancyCorrectiveAction.ReportedBy
			MailInfo += "</font></p>"
			MailInfo += "</body></html>"

			SendMailFile.SendMailFile(, UserName:=Thread.CurrentPrincipal.Identity.Name,
									  Subject:=$"{Prefix} Notification", ,
									  Info:=MailInfo,
									  VendorEmailID:="",
									  ToMailID:=Session("ToSendMailIDs"),
									  CCMailID:=Session("CcSendMailIDs"),
									  ReportPath:="",
									  ReportByMail:=True,
									  Remark:=Session("SendMailRemark"),
									  ReportGeneratedBy:=Session("ReportGenratedBy"),
									  SmtpHost:=Session("SmtpHost"),
									  SmtpPort:=Session("SmtpPort"),
									  SmtpUser:=Session("SmtpUser"),
									  SmtpPassword:=Session("SmtpPassword"))

			Dim mDirectiveDetail As String = $"New {Prefix} 
											  Notification sent successfully to {Session("ToSendMailIDs")} 
											  By {User.Identity.Name}"

			MarkLog(Action.SendMail,
					"DiscrepancyAction",
					mDirectiveDetail,
					ErrorType.HandledError,
					DiscrepancyCorrectiveAction.ID,
					EventLogID)

		Catch ex As Exception

			Dim Day, Month, Year As String
			Dim todaydate As String = Day & Month & Year
			Dim Path As String = AppSettings("DOCPath") & todaydate

			Day = Format(Today.Date.Day, "0#")
			Month = Format(Today.Date.Month, "0#")
			Year = Format(Today.Date.Year, "0#")

			FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
			WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
			FileClose(1)

		Finally
			Session.Remove("mModelMonitorModtmp")
		End Try

	End Sub

	Private Sub CreateWorkOrder(sender As Object, e As EventArgs) Handles lnkbtnCreateWorkOrder.Click

		Dim mnWO As nWO
		Dim tmpAssemblyStatusList As AssemblyStatusList
		Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList

		Try

			If DiscrepancyCorrectiveAction.IsWOCreated Then

				mnWO = nWO.GetWO(DiscrepancyCorrectiveAction.WOID, False)
				Session("mnWO") = mnWO
				Session("IsShowAllWOs") = True

			Else

				mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
				mnWO.WODate = CDate(txtDateofOccurrence.Text)
				mnWO.MachineID = ReportLogRegister(New Guid(cmbLogNo.SelectedValue)).MachineID

				If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then

					Dim TempRegNo As String = ""
					TempRegNo = ReportLogRegister(New Guid(cmbLogNo.SelectedValue)).RegNo
					mnWO.WOText = Replace(TempRegNo, "VT-", "")

					If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
						mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
					End If

				ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
					   (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then

					mnWO.WOText = "MJO# " & CStr(CDate(txtDateofOccurrence.Text).Date.Year) & " - " & mnWO.ModelName

				ElseIf AppSettings("ClientCode") = "TP" Then

					mnWO.WOText = Replace(ReportLogRegister(New Guid(cmbLogNo.SelectedValue)).RegNo,
										  "VT-", "") & "/" &
								  CStr(CDate(txtDateofOccurrence.Text).Date.Year)
				End If

				tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDateofOccurrence.Text.ToString,
																						 mnWO.MachineID.ToString, , , , , , , , , ,
																						 True, , , ,
																						 "Airframe", , , , , , , , , , , , , , , , , ,
																						 True,
																						 SkipIsForInventoryAircarft:=True,
																						 MonitoringServiceRequired:=False,
																						 MonitoringModRequired:=False,
																						 MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
				AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

				mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

				mnWO.WOJobs.Add(mnWO.ID, 3)
				mnWO.WOJobs.CurrentItem.PreviousTransID = DiscrepancyCorrectiveAction.ID
				mnWO.WOJobs.CurrentItem.DateOfOccurrence = DiscrepancyCorrectiveAction.DateOfOccurrence
				mnWO.WOJobs.CurrentItem.MELCategoryID = DiscrepancyCorrectiveAction.MELCategoryID
				mnWO.WOJobs.CurrentItem.ATAChapterID = DiscrepancyCorrectiveAction.ATAChapterID
				mnWO.WOJobs.CurrentItem.IsUnderMEL = DiscrepancyCorrectiveAction.IsMEL
				mnWO.WOJobs.CurrentItem.CompID = DiscrepancyCorrectiveAction.PartID
				mnWO.WOJobs.CurrentItem.IsMajor = DiscrepancyCorrectiveAction.IsMajor
				mnWO.WOJobs.CurrentItem.IsHours = DiscrepancyCorrectiveAction.IsHours
				mnWO.WOJobs.CurrentItem.FrequencyInDays = DiscrepancyCorrectiveAction.FrequencyInDays
				mnWO.WOJobs.CurrentItem.FrequencyInHours = DiscrepancyCorrectiveAction.FrequencyInHours
				mnWO.WOJobs.CurrentItem.IsRepetitive = DiscrepancyCorrectiveAction.IsRepetitive

				Dim Description As String = ""

				Description = DiscrepancyCorrectiveAction.Description & "<BR>" &
							  DiscrepancyCorrectiveAction.LogNo & "<BR>" &
							  DiscrepancyCorrectiveAction.Defect & "<BR>" & "Date Of Occurrence : " &
							  DiscrepancyCorrectiveAction.DateOfOccurrence

				'Component
				If DiscrepancyCorrectiveAction.PartName <> "" Then Description = Description & "<BR>" & "On Part : " & DiscrepancyCorrectiveAction.PartName

				'MEL Category
				If DiscrepancyCorrectiveAction.MELCategoryName <> "" Then

					Description = Description & "<BR>" &
								  IIf(AppSettings("MELSnagNomenclature") = "True",
									  "ADD Category : ",
									  "MEL Category : ") &
								  DiscrepancyCorrectiveAction.MELCategoryName & "with "

					If DiscrepancyCorrectiveAction.FrequencyInDays <> 0 Then
						Description = Description & DiscrepancyCorrectiveAction.FrequencyInDays & " Days"
					Else
						Description = Description & DiscrepancyCorrectiveAction.FrequencyInHours & " Hours"
					End If

				End If

				mnWO.WOJobs.CurrentItem.WOJobDescription = Description.Replace("<BR>", vbCrLf)
				mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Trim(Description.Replace("<BR>", vbCrLf))
				mnWO.WOJobs.CurrentItem.DueAsOf = DiscrepancyCorrectiveAction.DueDateFormatted.ToString

				If AppSettings("ShowCAMOOnlyForNewClients") = "True" Then
					mnWO.WOJobs.CurrentItem.TaskCardNo = DiscrepancyCorrectiveAction.DefectNo
				End If
				Session("mnWO") = mnWO

			End If

			Dim URLFromDueReportPreview As New Stack
			URLFromDueReportPreview.Push(Request.Url)
			Session("wfDiscrepancyCorrectiveAction") = "wfDiscrepancyCorrectiveAction"
			Session("URLFromDueReportPreview") = URLFromDueReportPreview
			Response.Redirect("wfnWODetail_Ajax.aspx?BackPage=index.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub Investigation_Changed(sender As Object, e As EventArgs) Handles cmbInvestigation.SelectedIndexChanged

		Try

			If cmbInvestigation.SelectedValue = "2" Then

				phDeviationMEL.Visible = True
				upnlInvestigation.Update()

			Else

				phDeviationMEL.Visible = False
				upnlInvestigation.Update()

			End If

			rectDate.Visible = (cmbInvestigation.SelectedValue = "1")
			phWatchListDetails.Visible = Not (TransTypeID = 116)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MELChecked(sender As Object, e As EventArgs) Handles rdbMEL.CheckedChanged, lnkMEL.Click

		Try

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("mMELSnagCorrectiveAction") = DiscrepancyCorrectiveAction

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"OpenMELMasterWindow",
												"OpenMELMasterWindow();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub CDLChecked(sender As Object, e As EventArgs) Handles rdbCDL.CheckedChanged, lnkDeferredList.Click

		Try

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("mMELSnagCorrectiveAction") = DiscrepancyCorrectiveAction

			ScriptManager.RegisterStartupScript(Me, [GetType],
												"OpenCDLMasterWindow",
												"OpenCDLMasterWindow();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ExtensionInHours_Changed(sender As Object, e As EventArgs) Handles txtExtensionInHours.TextChanged

		Try

			If txtFrequencyInHours.Text = "0" Or txtFrequencyInHours.Text = "" Then

				txtDueHrs.Text = ""
				Exit Sub

			End If

			DiscrepancyCorrectiveAction.ExtensionInHours = txtExtensionInHours.Text
			DiscrepancyCorrectiveAction.DueInHrs = New Period(1,
														   DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalHoursDec +
																  DiscrepancyCorrectiveAction.FrequencyInHoursDec +
																  DiscrepancyCorrectiveAction.ExtensionInHoursDec,
														   0,
														   False,
														   False).Value

			txtDueHrs.Text = DiscrepancyCorrectiveAction.DueInHrs
			upnlDueHrs.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ExtensionInCycles_Changed(sender As Object, e As EventArgs) Handles txtExtensionInCycles.TextChanged

		Try

			If txtExtensionInCycles.Text = "0" Or txtExtensionInCycles.Text = "" Then
				txtDueCycles.Text = ""
				Exit Sub
			End If

			DiscrepancyCorrectiveAction.ExtensionInCycles = txtExtensionInCycles.Text
			DiscrepancyCorrectiveAction.DueInCycles = New Period(3,
															  DiscrepancyCorrectiveActionLog(New Guid(cmbLogNo.SelectedValue.ToString)).FinalCyclesDec +
																	 DiscrepancyCorrectiveAction.FrequencyInCyclesDec +
																	 DiscrepancyCorrectiveAction.ExtensionInCyclesDec,
															  0,
															  False,
															  False).Value
			txtDueCycles.Text = DiscrepancyCorrectiveAction.DueInCycles
			upnlDueHrs.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Report "

	Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		Dim dataAdapter As New ObjectAdapter
		Dim CompanyDetail As New CompanyDetail
		Dim crystalReport As Engine.ReportClass
		Dim dataSet As New dsMELSnagCorrectiveAction
		Dim MELSnagCorrectiveActionReport As rptMELSnagCorrectiveAction

		Try

			'Added by Saylee on 8-Apr-2014 for ALL08042014
			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=TransTypeID,
															Action:={Action.Print},
															MSGBoxSender:="Authorization") Then

				Exit Sub

			End If

			MELSnagCorrectiveActionReport = rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(DiscrepancyCorrectiveAction.ID.ToString)

			Dim Report As New ReportData(CompanyDetail.CompanyName,
										 CompanyDetail.Address,
										 CompanyDetail.Tel1,
										 CompanyDetail.Tel2,
										 CompanyDetail.Fax,
										 CompanyDetail.Email,
										 CompanyDetail.WebSite,
										 ReportName:="PRELIMINARY DEFECT REPORT",
										 "", "", "", "", "",
										 ProductVersion:=AppSettings("Product Version"),
										 SINote:=AppSettings("SINote"),
										 "", "", "", "",
										 SearchStr10:=AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

			If DiscrepancyCorrectiveAction.IsMEL = True Then
				crystalReport = New crMELDetailReport
			Else
				crystalReport = New crLogDefectActionList
			End If

			Dim companyLogo As rptImage = rptImage.GetImage(dataSet)
			dataAdapter.Fill(dataSet, DiscrepancyCorrectiveAction)
			dataAdapter.Fill(dataSet, Report)
			dataAdapter.Fill(dataSet, companyLogo) 'Added by Utkarsh for Report Logo

			crystalReport.SetDataSource(dataSet)
			Session("CrystalReport") = crystalReport

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openTranDetail",
												"openTranDetail();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles attachmentICN.Click

		Try
			ViewImage()
		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub FileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

		Try

			ControlVisibilityForAttachment()
			upnlFileupload.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub DeleteAttachment(sender As Object, e As EventArgs) Handles btnDelAttach.Click

		Try

			Dim fileSize As Integer = 0
			Dim ImageFile(fileSize) As Byte

			FileAttach.ImageFile = ImageFile
			FileAttach.Size = 0
			attachmentICN.Visible = False
			btnDelAttach.Enabled = False
			IsAttachmentDeleted = True
			Session("IsAttachmentDeleted") = IsAttachmentDeleted

			MSGBoxCtrl.Show(MSGBox.Message_Title.AttachmentAlert,
							MSGBox.Message_Text.AttachmentRemovedSuccessFully,
							"",
							MsgBoxStyle.OkOnly,
							"")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Service Method(s) "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetLicenseList(prefixText As String,
										  count As Integer,
										  contextKey As String) As String()

		Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText,
																								"", , ,
																								True)

		Try

			If count = 0 Then

				Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray

			Else

				Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

		Return Nothing

	End Function

#End Region

End Class