'******************************************************
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'******************************************************


Public Class DiscrepancyCorrectiveActionListFromLog
	Inherits Page

#Region " Variable Declaration "

	Public DiscrepancyCorrectiveActionList As MELSnagCorrectiveActionListNew
	Public DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
	Public AttachmentHelper As New AttachmentHelper
	Public IncidentTypeList As IncidentTypeList
	Public Aircraft As MachineNameValueList
	Public Assemblylist As AssemblyList
	Public ATAList As ATAList
	Public Machine As Machine
	Public Log As Log

	Dim BackPage As String
	Dim AircraftId As String
	Dim AssemblyId As String
	Dim ATAChapterId As String
	Dim DateIndex, FromDate, ToDate, MachineID, Name, No, ATANomenclature, DefectType As String
	Dim StatusCode, ATACode, MELSnagCode As Integer
	Dim EventLogID As Guid
	Dim mMELSnagDetail As String
	Dim ExtensionApplied As Integer
	Dim IsInReliability As Integer
	Dim TypeOfIncidentID As Integer
	Dim ShowNoEntries As String
	Dim IsTroubleshoot As Integer
	Dim TransTypeID As Integer
	Dim Prefix As String

#End Region

#Region " Helper Methods "

	Private Sub GetSession()

		Log = Session("mLog")
		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		AircraftId = CType(Session("AircraftId"), String)
		StatusCode = Session("StatusCode")
		MELSnagCode = Session("MELSnagCode")
		MachineID = Session("MachineID")
		DiscrepancyCorrectiveActionList = Session("DiscrepancyCorrectiveActionList")
		DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
		Aircraft = CType(Session("MachineNameValueList"), MachineNameValueList)
		ATAList = CType(Session("ATAList"), ATAList)
		ATAChapterId = CType(Session("ATAChapterId"), String)
		Assemblylist = Session("mAssemblylist")
		AssemblyId = CType(Session("AssemblyId"), String)
		ExtensionApplied = Session("ExtensionApplied")
		IsInReliability = Session("IsInReliability")
		DefectType = Session("DefectType")
		IncidentTypeList = CType(Session("IncidentTypeList"), IncidentTypeList)
		TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)
		ShowNoEntries = CType(Session("ShowNoEntries"), String)
		IsTroubleshoot = CType(Session("IsTroubleshoot"), Integer)
		Machine = Session("mMachine")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
		Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
		Session("MachineID") = MachineID
		Session("MachineNameValueList") = Aircraft
		Session("ATAList") = ATAList
		Session("AssemblyId") = AssemblyId
		Session("IncidentTypeList") = IncidentTypeList
		Session("ShowNoEntries") = ShowNoEntries
		Session("IsTroubleshoot") = IsTroubleshoot
		Session("mMachine") = Machine
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Sub RemoveSession()

		Aircraft = Nothing
		Session.Remove("MELSnagCorrectiveActionListNew")
		Session.Remove("MachineNameValueList")
		Session.Remove("ATAList")
		Session.Remove("mAssemblylist") 'Added By Vikrant On 02-Sept-2014 For All04092014
		Session.Remove("AssemblyId")
		Session.Remove("ExtensionApplied")
		Session.Remove("IsInReliability")
		Session.Remove("DefectType")
		Session.Remove("IncidentTypeList")
		Session.Remove("TypeOfIncidentID")
		Session.Remove("mIsTroubleshoot")

	End Sub

	Private Sub ClearAll()

		If Session("MiddleFrame") <> $"wfDiscrepancyCorrectiveActionList_AJAX.aspx?Troubleshoot={Request.QueryString("Troubleshoot")}&TransTypeID={Request.QueryString("TransTypeID")}" Then

			Session.Remove("MELSnagCorrectiveActionListNew")
			Session.Remove("MELSnagCorrectiveAction")
			Session.Remove("Name")
			Session.Remove("MachineNameValueList")
			Session.Remove("FromDate")
			Session.Remove("ToDate")
			Session.Remove("AircraftId")
			Session.Remove("MachineID")
			Session.Remove("StatusCode")
			Session.Remove("MELSnagCode")
			Session.Remove("ATACode")
			Session.Remove("ATANomenclature")
			Session.Remove("ATAChapterId")
			Session.Remove("mAssemblylist")
			Session.Remove("AssemblyId")
			Session.Remove("ExtensionApplied")
			Session.Remove("IsInReliability")
			Session.Remove("DefectType")
			Session.Remove("IncidentTypeList")
			Session.Remove("TypeOfIncidentID")
			Session.Remove("mIsTroubleshoot")

		End If

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			Dim str As String
			str = "<script language='javascript'> document.getElementById('" + control.ClientID + "').focus();</script>"
			ClientScript.RegisterStartupScript([GetType], "FocusScript", str)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub NewRecord()

		Try

			DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(AssemblyStatusID:=Assemblylist(1).AssemblyStatusID.ToString,
																							 TransTypeID:=TransTypeID)
			DiscrepancyCorrectiveAction.LogID = Log.ID
			DiscrepancyCorrectiveAction.DateOfOccurrence = Log.DateFormatted.ToString

			Dim MELSnagCorrectiveActionLog As MELSnagCorrectiveActionLog
			MELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(LogID:=DiscrepancyCorrectiveAction.LogID.ToString)

			With MELSnagCorrectiveActionLog

				DiscrepancyCorrectiveAction.Sector = MELSnagCorrectiveActionLog.Item(0).DestinationName
				Session("TmpLogDate") = MELSnagCorrectiveActionLog.Item(0).LogDate

				If MELSnagCorrectiveActionLog.Item(0).FinalLandings = "" Then
					DiscrepancyCorrectiveAction.LastMajorCheckHour = MELSnagCorrectiveActionLog.Item(0).FinalHours + " H"
				Else
					DiscrepancyCorrectiveAction.LastMajorCheckHour = MELSnagCorrectiveActionLog.Item(0).FinalHours + " H" + ", " + MELSnagCorrectiveActionLog.Item(0).FinalLandings + " L"
				End If

				If MELSnagCorrectiveActionLog.Item(0).FinalCycles = "" Then
					DiscrepancyCorrectiveAction.LastMajorCheckHour = DiscrepancyCorrectiveAction.LastMajorCheckHour
				Else
					DiscrepancyCorrectiveAction.LastMajorCheckHour = DiscrepancyCorrectiveAction.LastMajorCheckHour + ", " + MELSnagCorrectiveActionLog.Item(0).FinalCycles + " C"
				End If

			End With

			Dim FileAttach As FileAttach = FileAttach.NewAttachment(ID:=Guid.Empty,
																	ReferenceID:=DiscrepancyCorrectiveAction.ID)

			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("MELSnagCorrectiveActionLog") = MELSnagCorrectiveActionLog
			Session("mFileAttach") = FileAttach

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Dim MsgBoxResult As MsgBoxResult
		Dim msgCount As Integer = 0
		MsgBoxResult = MSGBoxCtrl.Result

		Try

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Dim TempID As Guid

							Try

								Session("sender") = ""
								DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
								TempID = DiscrepancyCorrectiveAction.ID
								mMELSnagDetail = DiscrepancyCorrectiveAction.DefectNo + " Dated : " + DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + DiscrepancyCorrectiveAction.LogNo
								MELSnagCorrectiveAction.DeleteMELSnagCorrectiveAction(DiscrepancyCorrectiveAction.ID)
								MarkLog(Action.Delete, "DiscrepancyAction", mMELSnagDetail, ErrorType.NoError, TempID, EventLogID)
								Session.Remove("MELSnagCorrectiveAction")
								DataFieldBind()

								upnlGridView.Update()

							Catch ex As SqlException

								If ex.Number = 8145 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 2627 Then
									MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError, MSGBox.Message_Text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
								ElseIf ex.Number = 547 Then
									MarkLog(Action.Delete, "DiscrepancyAction", "Can't delete : " & mMELSnagDetail & " is Currently in use", ErrorType.NoError, TempID, EventLogID)
									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete, MSGBox.Message_Text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
								End If
								DataFieldBind()

								msgCount = ex.Errors.Count

							Finally

								If msgCount = 0 Then

									If IsTroubleshoot = 1 Then
										ScriptManager.RegisterStartupScript(Me, [GetType], "CallParentFunctionautoResize", "CallParentFunctionautoResize()", True)
									Else
										ScriptManager.RegisterStartupScript(Me, [GetType], "CallParentFunctionautoResizeDiscrepanciesReporting", "CallParentFunctionautoResizeDiscrepanciesReporting()", True)
									End If

								End If

							End Try

						End If

					Case MsgBoxResult.No
						Session("sender") = ""
					Case MsgBoxResult.Ok
						Session("sender") = ""
						DataFieldBind()
					Case MsgBoxResult.Ok And Session("sender") = "Authorization"
						DataFieldBind()
				End Select

			ElseIf MsgBoxResult = -1 Then
				Session("sender") = ""
			ElseIf MsgBoxResult = 0 And Session("sender") = "Authorization" Then

				Session("sender") = ""
				DataFieldBind()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub DeleteRecord(ID As Guid)

		Try

			DataFieldBind()
			SetGrid()
			upnlGridView.Update()

			MSGBoxCtrl.Show(MSGBox.Message_Title.Delete,
							MSGBox.Message_Text.Delete,
							" ",
							MsgBoxStyle.YesNo,
							"Delete")

			DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=ID)
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub FindNow(Optional FromDate As String = "1-1-1900",
						Optional ToDate As String = "1-1-3300",
						Optional MachineID As String = "{00000000-0000-0000-0000-000000000000}",
						Optional InvestigationStatus As Integer = 0,
						Optional ATACode As Integer = 0,
						Optional ATANomenclature As String = "",
						Optional MELSnag As Integer = 0,
						Optional AssemblyStatusID As String = "{00000000-0000-0000-0000-000000000000}",
						Optional ExtensionApplied As Integer = 0,
						Optional IsInReliability As Integer = 0,
						Optional DefectType As Integer = 0,
						Optional TypeOfIncidentID As Integer = -1)

		Try

			If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then

				DiscrepancyCorrectiveActionList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate:="1-1-1900".ToString,
																												  ToDate:=Log.Date.ToString,
																												  MachineID:=Log.MachineID.ToString,
																												  InvestigationStatus:=2,
																												  TimeFormat:="HH:mm")

			Else

				DiscrepancyCorrectiveActionList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate:="1-1-1900".ToString,
																												  ToDate:=Log.Date.ToString,
																												  MachineID:=Log.MachineID.ToString,
																												  InvestigationStatus:=2)

			End If

			dgSnagCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			dgSnagCorrectiveActionList.DataBind()

			SetGrid()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetGrid()

		Try

			For Each column As DataControlField In dgSnagCorrectiveActionList.Columns

				Select Case column.HeaderText
					Case "Due"
						column.Visible = IIf(IsTroubleshoot = 0, False, True)
					Case "Action"
						column.Visible = IIf(IsTroubleshoot = 1, False, True)
					Case "Troubleshooting"
						column.Visible = IIf(IsTroubleshoot = 1, True, False)
				End Select

			Next

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub EditRecord(mID As Guid)

		Try

			DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mID)
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("MachineID") = DiscrepancyCorrectiveAction.MachineID.ToString
			Dim mtmpLog As Log = Log.GetLog(DiscrepancyCorrectiveAction.LogID, IsFromTroubleshooting:=True)
			Session("TmpLogDate") = mtmpLog.Date
			Session("IsFromLog") = True
			AircraftId = Session("MachineID")

			If DiscrepancyCorrectiveAction.IsAttachmentAdded Then
				Dim mFileAttach As FileAttach = FileAttach.GetAttachment(DiscrepancyCorrectiveAction.ID) 'Sort = 1 - Installation
				Session("mFileAttach") = mFileAttach
			Else

				Dim mFileAttach As FileAttach
				mFileAttach = FileAttach.NewAttachment(Guid.Empty, DiscrepancyCorrectiveAction.ID)
				Session("mFileAttach") = mFileAttach

			End If

			mMELSnagDetail = $"{DiscrepancyCorrectiveAction.DefectNo} 
							  Dated : {DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted} 
							  Log No. {DiscrepancyCorrectiveAction.LogNo}"

			MarkLog(Action.Edit,
					"DiscrepancyAction",
					mMELSnagDetail,
					ErrorType.NoError,
					mID,
					EventLogID)

			upnlGridView.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetLabelsAndVisibility()

		Try

			lblTitle.Text = $"{Prefix} Detail"

			btnAddNew.ToolTip = $"Add New {Prefix}"
			btnClose.ToolTip = $"Close {Prefix} detail Screen."

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " DataBinding "

	Public Sub DataFieldBind()

		Try

			If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then

				DiscrepancyCorrectiveActionList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate:=IIf(IsTroubleshoot = 1,
																																 "1-1-1900",
																															 	 Log.Date.ToString),
																												   ToDate:=Log.Date.ToString,
																												   MachineID:=Log.MachineID.ToString,
																												   InvestigationStatus:=IIf(IsTroubleshoot = 1,
																																			2,
																																			0),
																												   TimeFormat:="HH:mm",
																												   LogID:=IIf(IsTroubleshoot = 1,
																															  Guid.Empty.ToString,
																															  Log.ID.ToString),
																												   IsCabinDefect:=(TransTypeID = 116))

			Else

				DiscrepancyCorrectiveActionList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate:=IIf(IsTroubleshoot = 1,
																																 "1-1-1900",
																																 Log.Date.ToString),
																												   ToDate:=Log.Date.ToString,
																												   MachineID:=Log.MachineID.ToString,
																												   InvestigationStatus:=IIf(IsTroubleshoot = 1,
																																			2,
																																			0),
																												   LogID:=IIf(IsTroubleshoot = 1,
																															  Guid.Empty.ToString,
																															  Log.ID.ToString),
																												  IsCabinDefect:=(TransTypeID = 116))

			End If

			dgSnagCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList

			dgSnagCorrectiveActionList.Columns(1).HeaderText = $"{Prefix} No."
			dgSnagCorrectiveActionList.Columns(2).HeaderText = $"Date Of Occurrence {IIf(Log.IsUTC = True, "(UTC)", "")}"
			dgSnagCorrectiveActionList.Columns(6).HeaderText = $"{Prefix}."

			dgSnagCorrectiveActionList.Columns(5).Visible = IIf(TransTypeID = 116, False, True)
			dgSnagCorrectiveActionList.Columns(8).Visible = IIf(TransTypeID = 116, False, True)
			btnAddNew.Visible = IIf(IsTroubleshoot = 1, False, True)

			dgSnagCorrectiveActionList.DataBind()
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList

		Catch ex As Exception
			Throw ex.GetBaseException()
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

			Prefix = IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")

			Session("TransTypeID") = TransTypeID

			If Not IsPostBack And Session("sender") = "" Then

				IsTroubleshoot = Request.QueryString("Troubleshoot")
				Session("IsTroubleshoot") = IsTroubleshoot

				DataFieldBind()

			End If

			SetGrid()
			SetLabelsAndVisibility()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Dim parentPageScript, ModuleNameForMarkLog As String
		Try

			If (Not User.IsInRole("DiscrepancyActionNew") And
				Not User.IsInRole("DiscrepancyActionEdit")) Then

				SetSession()

				MarkLog(Action.New,
						"DiscrepancyAction",
						User.Identity.Name & " is not Authorized User to add ",
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			NewRecord()

			DiscrepancyCorrectiveAction.MachineID = Log.MachineID
			DiscrepancyCorrectiveAction.AssemblyStatusID = Machine.AssemblyStatus.ID

			Session("MachineID") = Log.MachineID.ToString
			Session("AircraftRegNo") = Log.RegNo.ToString
			Session("IsFromLog") = True
			Session("DateOfOccurrence") = DiscrepancyCorrectiveAction.DateOfOccurrence
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

			ModuleNameForMarkLog = IIf(TransTypeID = 115, "DiscrepancyAction", "CabinDefectAction")

			MarkLog(Action.[New],
					ModuleNameForMarkLog,
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

			parentPageScript = IIf(TransTypeID = 115,
								   "CallParentDiscrepancy()",
								   "CallParentFunctionForCabinDefect()")

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Call Parent Function",
												parentPageScript,
												True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			MarkLog(Action.Close,
					"DiscrepancyAction",
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

			RemoveSession()

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"CallCloseChildPage",
												"CallCloseChildPage();",
												True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgSnagCorrectiveActionList.PageIndexChanging

		Dim parentAutoResizeScript As String
		Try

			dgSnagCorrectiveActionList.PageIndex = e.NewPageIndex
			dgSnagCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			dgSnagCorrectiveActionList.DataBind()

			SetGrid()

			If IsTroubleshoot = 1 Then

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Call Parent Function AutoResize",
													"CallParentFunctionautoResize()",
													True)

			Else

				parentAutoResizeScript = IIf(TransTypeID = 115,
											 "Call Parent Function AutoResize Discrepancies Reporting()",
											 "CallParentFunctionautoResizeforCabinDefect()")

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Call Parent Auto-Resize Function",
													parentAutoResizeScript,
													True)

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgSnagCorrectiveActionList.RowCommand

		Dim ID
		Dim Index As Integer
		Dim str As String

		Try

			Index = CInt(e.CommandArgument) + dgSnagCorrectiveActionList.PageSize * dgSnagCorrectiveActionList.PageIndex
			ID = DiscrepancyCorrectiveActionList(Index).ID

			Select Case e.CommandName
				Case "EditRec"

					If (Not User.IsInRole("DiscrepancyActionView") And Not User.IsInRole("DiscrepancyActionEdit")) Then

						SetSession()

						MarkLog(Action.Edit,
								"DiscrepancyAction",
								User.Identity.Name & " is not Authorized User to edit ",
								ErrorType.HandledError,
								Guid.Empty,
								EventLogID)

						ClientScript.RegisterStartupScript([GetType],
														   "OpenScript",
														   MessageBox.Show("You are not authorized user"))

						Exit Sub

					End If

					DataFieldBind()
					EditRecord(ID)

					Dim script As String = IIf(TransTypeID = 116, "CallParentFunctionForCabinDefect()", "CallParentDiscrepancy()")
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Detail Page",
														script,
														True)


				Case "AttachRec"

					If (Not User.IsInRole("DiscrepancyActionView")) Then

						SetSession()

						MarkLog(Action.View,
								"DiscrepancyAction",
								User.Identity.Name & " is not Authorized User to view ",
								ErrorType.HandledError,
								Guid.Empty,
								EventLogID)

						ClientScript.RegisterStartupScript([GetType],
														   "OpenScript",
														   MessageBox.Show("You are not authorized user"))
						Exit Sub

					End If

					Dim FileAttach As FileAttach
					Dim DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
					DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=ID)
					FileAttach = FileAttach.GetAttachment(ReferenceID:=DiscrepancyCorrectiveAction.ID)
					Session("mFileAttach") = FileAttach

					AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)

				Case "PrintRec"

					If Not User.IsInRole("DiscrepancyActionPrint") Then

						SetSession()

						MarkLog(Action.Print,
								"DiscrepancyAction",
								User.Identity.Name & " is not Authorized User to print ",
								ErrorType.HandledError,
								Guid.Empty,
								EventLogID)

						ClientScript.RegisterStartupScript([GetType],
														   "OpenScript",
														   MessageBox.Show("You are not authorized user"))

						Exit Sub

					End If

					Dim DiscrepancyCorrectiveActionID As Guid = DiscrepancyCorrectiveActionList(Index).ID
					Dim MELTag As String = DiscrepancyCorrectiveActionList(Index).IsMEL
					Dim CrystalReport As Engine.ReportClass
					Dim dataSet As New dsMELSnagCorrectiveAction
					Dim dataAdapter As New ObjectAdapter
					Dim mCompanyDetail As New CompanyDetail
					Dim DiscrepancyCorrectiveActionForReport As rptMELSnagCorrectiveAction

					DiscrepancyCorrectiveActionForReport = rptMELSnagCorrectiveAction.GetrptMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID.ToString)

					Dim Report As New ReportData(mCompanyDetail.CompanyName,
												 mCompanyDetail.Address,
												 mCompanyDetail.Tel1,
												 mCompanyDetail.Tel2,
												 mCompanyDetail.Fax,
												 mCompanyDetail.Email,
												 mCompanyDetail.WebSite,
												 "PRELIMINARY DEFECT REPORT",
												 "", "", "", "", "",
												 AppSettings("Product Version"),
												 AppSettings("SINote"), "", "", "", "",
												 AppSettings("Logo"))

					If MELTag = "Yes" Then
						CrystalReport = New crMELDetailReport
					Else
						CrystalReport = New crLogDefectActionList
					End If

					Dim CompanyLogo As rptImage = rptImage.GetImage(dataSet)

					dataAdapter.Fill(dataSet, DiscrepancyCorrectiveActionForReport)
					dataAdapter.Fill(dataSet, Report)
					dataAdapter.Fill(dataSet, CompanyLogo)
					CrystalReport.SetDataSource(dataSet)

					Session("CrystalReport") = CrystalReport

					DataFieldBind()
					upnlGridView.Update()

					str = "openTranDetail();"
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"openTranDetail",
														Str,
														True)

				Case "DeleteRec"

					If Not User.IsInRole("DiscrepancyActionDelete") Then

						SetSession()

						MarkLog(Action.Delete,
								"DiscrepancyAction",
								User.Identity.Name & " is not Authorized User to delete ",
								ErrorType.HandledError,
								Guid.Empty,
								EventLogID)

						ClientScript.RegisterStartupScript([GetType],
														   "OpenScript",
														   MessageBox.Show("You are not authorized user"))

						Exit Sub

					End If

					DeleteRecord(ID:=ID)

				Case "TroubleShootRec"

					DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=ID)
					Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
					Session("LogDateForRectification") = Log.DateFormatted
					Session("LogIDForRectification") = Log.ID
					Session("TroubleShootFromLog") = "True"

					SetSession()
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallParentDiscrepancyTroubleShootWindow",
														"CallParentDiscrepancyTroubleShootWindow()",
														True)

			End Select

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgSnagCorrectiveActionList.Sorting

		Try

			DiscrepancyCorrectiveActionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			dgSnagCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			dgSnagCorrectiveActionList.DataBind()

			SetGrid()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

#End Region

End Class