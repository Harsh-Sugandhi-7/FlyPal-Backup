'******************************************************
'Created by : Saylee 
'Dated      : 22-Feb-2024
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'******************************************************


Imports System.Linq


Public Class DiscrepancyCorrectiveActionListPage
	Inherits Page


#Region " Variable Declaration "

	Public DiscrepancyCorrectiveActionList As MELSnagCorrectiveActionListNew
	Public DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
	Public AuthorizationHelper As New AuthorizationHelper
	Public AttachmentHelper As New AttachmentHelper
	Public AircraftList As MachineNameValueList
	Public IncidentTypeList As IncidentTypeList
	Public CompanyDetail As New CompanyDetail
	Public AssemblyList As AssemblyList
	Public ModuleList As ModuleList
	Public ATAList As ATAList

	Dim EventLogID As Guid
	Dim BackPage As String
	Dim AircraftId As String
	Dim AssemblyId As String
	Dim ATAChapterId As String
	Dim MELSnagDetail As String
	Dim ShowNoEntries As String
	Dim IsTroubleshoot As Integer
	Dim TypeOfIncidentID As Integer
	Dim StatusCode, ATACode, MELSnagCode, DefectNo As Integer
	Dim DateIndex, FromDate, ToDate, MachineID, Name, No, ATANomenclature, DefectType, DefectText As String

	Dim Prefix As String
	Dim ModuleName As String
	Dim TransTypeID As Integer

#End Region

#Region " Helper Methods "

	Private Sub GetSession()

		FromDate = Session("FromDate")
		ToDate = Session("ToDate")
		AircraftId = CType(Session("AircraftId"), String)
		StatusCode = Session("StatusCode")
		MELSnagCode = Session("MELSnagCode")
		MachineID = Session("MachineID")
		DiscrepancyCorrectiveActionList = Session("DiscrepancyCorrectiveActionList")
		DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
		AircraftList = CType(Session("MachineNameValueList"), MachineNameValueList)
		ATAList = CType(Session("ATAList"), ATAList)
		ATAChapterId = CType(Session("ATAChapterId"), String)
		AssemblyList = Session("mAssemblylist")
		AssemblyId = CType(Session("AssemblyId"), String)
		DefectType = Session("DefectType")
		IncidentTypeList = CType(Session("IncidentTypeList"), IncidentTypeList)
		TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)
		ShowNoEntries = CType(Session("ShowNoEntries"), String)
		IsTroubleshoot = CType(Session("IsTroubleshoot"), Integer)
		ModuleList = Session("ModuleList")
		CompanyDetail = Session("CompanyDetail")
		DefectText = Session("DefectText")
		DefectNo = Session("DefectNo")
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub SetSession()

		Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
		Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
		Session("MachineID") = MachineID
		Session("MachineNameValueList") = AircraftList
		Session("ATAList") = ATAList
		Session("AssemblyId") = AssemblyId
		Session("IncidentTypeList") = IncidentTypeList
		Session("ShowNoEntries") = ShowNoEntries
		Session("IsTroubleshoot") = IsTroubleshoot
		Session("CompanyDetail") = CompanyDetail
		Session("TransTypeID") = TransTypeID

	End Sub

	Private Sub RemoveSession()

		AircraftList = Nothing
		Session.Remove("DiscrepancyCorrectiveActionList")
		Session.Remove("MachineNameValueList")
		Session.Remove("ATAList")
		Session.Remove("AssemblyList")
		Session.Remove("AssemblyId")
		Session.Remove("DefectType")
		Session.Remove("IncidentTypeList")
		Session.Remove("TypeOfIncidentID")
		Session.Remove("IsTroubleshoot")
		Session.Remove("CompanyDetail")
		Session.Remove("DefectText")
		Session.Remove("DefectNo")
		Session.Remove("TransTypeID")

	End Sub

	Private Sub ClearAll()

		If Session("MiddleFrame") <> $"wfDiscrepancyCorrectiveActionList_AJAX.aspx?Troubleshoot={Request.QueryString("Troubleshoot")}&TransTypeID={Request.QueryString("TransTypeID")}" Then

			Session.Remove("DiscrepancyCorrectiveActionList")
			Session.Remove("DiscrepancyCorrectiveAction")
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
			Session.Remove("AssemblyList")
			Session.Remove("AssemblyId")
			Session.Remove("DefectType")
			Session.Remove("IncidentTypeList")
			Session.Remove("TypeOfIncidentID")
			Session.Remove("IsTroubleshoot")
			Session.Remove("mLog")
			Session.Remove("CompanyDetail")
			Session.Remove("DefectText")
			Session.Remove("DefectNo")
			Session.Remove("TransTypeID")

		End If

	End Sub

	Private Sub AddAttributes()

		Try

			txtNo.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtNo').value,event)")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		Try

			If control.Enabled = False Or control.Visible = False Then Exit Sub

			Dim str As String
			str = "<script language='javascript'> document.getElementById('" + control.ClientID + "').focus();</script>"

			ClientScript.RegisterStartupScript([GetType],
											   "FocusScript",
											   str)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub NewRecord()

		Try

			DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(AssemblyStatusID:=AssemblyList(1).AssemblyStatusID.ToString,
																							 TransTypeID:=TransTypeID)
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

			Dim FileAttach As FileAttach = FileAttach.NewAttachment(ID:=Guid.Empty,
																	ReferenceID:=DiscrepancyCorrectiveAction.ID) 'Sort = 1 : Installation
			Session("mFileAttach") = FileAttach

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub EditRecord(DiscrepancyCorrectiveActionID As Guid)

		Dim LogDetails As Log
		Try

			DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID)
			LogDetails = Log.GetLog(ID:=DiscrepancyCorrectiveAction.LogID)
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("MachineID") = cmbAircraft.SelectedValue.ToString
			Session("TmpLogDate") = LogDetails.Date
			AircraftId = Session("MachineID")

			If DiscrepancyCorrectiveAction.IsAttachmentAdded Then

				Dim FileAttach As FileAttach = FileAttach.GetAttachment(DiscrepancyCorrectiveAction.ID) 'Sort = 1 - Installation
				Session("mFileAttach") = FileAttach

			Else

				Dim FileAttach As FileAttach
				FileAttach = FileAttach.NewAttachment(Guid.Empty, DiscrepancyCorrectiveAction.ID)
				Session("mFileAttach") = FileAttach

			End If

			MELSnagDetail = $"{DiscrepancyCorrectiveAction.DefectNo} Dated : {DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted} Log No. {DiscrepancyCorrectiveAction.LogNo}"

			MarkLog(Action.Edit,
					"DiscrepancyAction",
					MELSnagDetail,
					ErrorType.NoError,
					DiscrepancyCorrectiveActionID,
					EventLogID)

			Session("IsFromLog") = False

			If IsTroubleshoot = 2 Then 'Added By Prashant 12-Mar-2024 From Watch Discrepancies Link

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Open Discrepancy Detai lWindow",
													"OpenDiscrepancyDetailWindow()",
													True)

			Else

				Dim str As String
				str = $"openPageInSameTab('wfDiscrepancyCorrectiveAction.aspx?BackPage=Index.aspx&TransTypeID={Session("TransTypeID")}');"

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenScript",
													str,
													True)


			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub DeleteRecord(ID As Guid)

		Try

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
						Optional DefectType As Integer = 0,
						Optional TypeOfIncidentID As Integer = -1,
						Optional DefectText As String = "",
						Optional DefectNo As Integer = 0)

		Try

			DiscrepancyCorrectiveActionList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(FromDate:=FromDate,
																											   ToDate:=ToDate,
																											   MachineID:=MachineID,
																											   InvestigationStatus:=InvestigationStatus,
																											   TimeFormat:=IIf(AppSettings("TimeFormat") = "HH:mm" Or
																																		AppSettings("TimeFormat") = "hh:mm",
																															   "HH:mm", ""),
																											   ATACode:=ATACode,
																											   ATANomenclature:=ATANomenclature,
																											   MELSnag:=MELSnag,
																											   AssemblyStatusID:=cmbAssembly.SelectedValue.ToString,
																											   DefectType:=DefectType,
																											   IncidentTypeID:=TypeOfIncidentID,
																											   AddedToWatchList:=IIf(IsTroubleshoot = 2,
																																	 1, 0),
																											   DefectText:=DefectText,
																											   DefectNo:=DefectNo,
																											   IsCabinDefect:=(TransTypeID = 116))

			dgDiscrepancyCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			dgDiscrepancyCorrectiveActionList.DataBind()

			dgDiscrepancyCorrectiveActionList.Columns(2).HeaderText = IIf(AircraftList(ID:=New Guid(MachineID)).IsUTC,
																				"Date Of Occurrence (UTC)",
																				"Date Of Occurrence")

			dgDiscrepancyCorrectiveActionList.Columns(1).HeaderText = $"{Prefix} No"

			dgDiscrepancyCorrectiveActionList.Columns(6).HeaderText = $"{Prefix}"

			SetGrid()
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList

			lblResult.Text = $"List of {Prefix} as per criteria : {DiscrepancyCorrectiveActionList.Count} Record(s) found."

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MessageBoxResult()

		Try

			Dim MsgBoxResult As MsgBoxResult
			Dim msgCount As Integer = 0
			MsgBoxResult = MSGBoxCtrl.Result

			If MsgBoxResult > 0 Then

				Select Case MsgBoxResult
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "Delete" Then

							Dim TempID As Guid

							Try

								Session("sender") = ""
								DiscrepancyCorrectiveAction = Session("DiscrepancyCorrectiveAction")
								TempID = DiscrepancyCorrectiveAction.ID
								MELSnagDetail = DiscrepancyCorrectiveAction.DefectNo + " Dated : " + DiscrepancyCorrectiveAction.DateOfOccurrenceFormatted + " Log No. " + DiscrepancyCorrectiveAction.LogNo
								MELSnagCorrectiveAction.DeleteMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveAction.ID)

								MarkLog(Action.Delete,
										"DiscrepancyAction",
										MELSnagDetail,
										ErrorType.NoError,
										TempID,
										EventLogID)

								Session.Remove("MELSnagCorrectiveAction")

								GridBind()

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

									MarkLog(Action.Delete,
											"Work Order",
											"Can't delete : " & MELSnagDetail & " is Currently in use",
											ErrorType.NoError,
											TempID,
											EventLogID)

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
													MSGBox.Message_Text.ReferenceDelete,
													ex.Procedure,
													MsgBoxStyle.OkOnly,
													"")

								End If

								DataFieldBind()
								SetControl()
								msgCount = ex.Errors.Count

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

			PreserveStateOfFavIcon()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetControl()

		Dim mMachineID As String
		Try

			'@MELSnag = 0												--ALL MEL & Snag parts
			'@MELSnag = 1										        --MEL Parts
			'@MELSnag = 2										        --Snag Parts
			'@InvestigationStatus = 0									--ALL Open & Closed
			'@InvestigationStatus = 1									--Closed
			'@InvestigationStatus = 2									--Open

			Name = Session("Name")

			If IsTroubleshoot = 2 Then  'From Discrepancy Watch List Added By Prashant 4-Mar-2024
				StatusCode = 1  ' Closed
			ElseIf IsTroubleshoot = 1 AndAlso TransTypeID = 115 Then    'From Discrepancy
				StatusCode = 2  ' Deferred
			ElseIf IsTroubleshoot = 1 AndAlso TransTypeID = 116 Then    'From Cabin Defect
				StatusCode = 3  ' Open
			ElseIf IsTroubleshoot = 0 Then
				StatusCode = 3  ' Open
			Else
				StatusCode = Session("StatusCode")
			End If

			MELSnagCode = CType(Session("MELSnagCode"), Integer)
			ATACode = CType(Session("ATACode"), Integer)
			ATANomenclature = Session("ATANomenclature")
			ATAChapterId = Session("ATAChapterId")
			FromDate = Session("FromDate")
			ToDate = Session("ToDate")
			DefectType = CType(Session("DefectType"), Integer)
			DefectText = CType(Session("DefectText"), String)
			DefectNo = CType(Session("DefectNo"), Integer)

			mMachineID = Session("AircraftId")
			If mMachineID = "" Then mMachineID = Guid.Empty.ToString

			FindNow(FromDate:=FromDate,
					ToDate:=ToDate,
					MachineID:=mMachineID,
					InvestigationStatus:=StatusCode,
					ATACode:=ATAList(New Guid(ATAChapterId)).ATACode,
					ATANomenclature:=ATAList(New Guid(ATAChapterId)).ATANomenclature,
					MELSnag:=MELSnagCode,
					AssemblyStatusID:=AssemblyId,
					DefectType:=DefectType,
					TypeOfIncidentID:=TypeOfIncidentID,
					DefectText:=DefectText,
					DefectNo:=DefectNo)

			dgDiscrepancyCorrectiveActionList.DataBind()
			txtFromDate.Text = FromDate
			txtToDate.Text = ToDate
			cmbStatus.SelectedValue = StatusCode
			cmbMELSnag.SelectedValue = MELSnagCode
			cmbATAChapter.SelectedValue = ATAChapterId
			cmbAssembly.SelectedValue = AssemblyId
			cmbAircraft.SelectedValue = mMachineID
			cmbIncidentType.SelectedValue = TypeOfIncidentID
			cmbDefectType.SelectedValue = DefectType
			txtText.Text = DefectText
			txtNo.Text = DefectNo

			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub GridBind()

		Try

			Session("AircraftId") = cmbAircraft.SelectedValue
			Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
			FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "")
			ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "")
			StatusCode = cmbStatus.SelectedValue
			MELSnagCode = cmbMELSnag.SelectedValue
			AssemblyId = cmbAssembly.SelectedValue
			DefectType = cmbDefectType.SelectedValue
			TypeOfIncidentID = CInt(cmbIncidentType.SelectedValue)
			DefectText = txtText.Text.Trim
			DefectNo = Val(txtNo.Text.Trim)

			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("Name") = Name
			Session("ATACode") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
			Session("ATANomenclature") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
			Session("ATAChapterId") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ID.ToString
			Session("StatusCode") = StatusCode
			Session("MELSnagCode") = MELSnagCode
			Session("AssemblyId") = AssemblyId
			Session("DefectType") = DefectType
			Session("TypeOfIncidentID") = TypeOfIncidentID
			Session("DefectText") = DefectText
			Session("DefectNo") = DefectNo

			FindNow(FromDate:=txtFromDate.Text.ToString,
					ToDate:=txtToDate.Text.ToString,
					MachineID:=mMachineID.ToString,
					InvestigationStatus:=StatusCode,
					ATACode:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode,
					ATANomenclature:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
					MELSnag:=MELSnagCode,
					AssemblyStatusID:=cmbAssembly.SelectedValue.ToString,
					DefectType:=DefectType,
					TypeOfIncidentID:=TypeOfIncidentID,
					DefectText:=DefectText,
					DefectNo:=DefectNo)

			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetGrid()

		Try

			For Each column As DataControlField In dgDiscrepancyCorrectiveActionList.Columns

				Select Case column.HeaderText
					Case "Category"
						column.Visible = IIf(IsTroubleshoot = 0 Or (TransTypeID = 116), False, True)
					Case "Due"
						column.Visible = IIf((IsTroubleshoot = 0 Or IsTroubleshoot = 2 Or TransTypeID = 116), False, True)
					Case "Close Date"
						column.Visible = IIf(IsTroubleshoot = 0 Or IsTroubleshoot = 1, False, True)
					Case "Rectified Log No."
						column.Visible = IIf(IsTroubleshoot = 0 Or IsTroubleshoot = 1, False, True)
					Case "Watchlist Instruction"
						column.Visible = IIf(IsTroubleshoot = 0 Or IsTroubleshoot = 1, False, True)
					Case "Action"
						column.Visible = IIf(IsTroubleshoot = 1 Or IsTroubleshoot = 2, False, True)
					Case "Troubleshooting"
						column.Visible = IIf(IsTroubleshoot = 1, True, False)
					Case "View Details"
						column.Visible = IIf(IsTroubleshoot = 2, True, False)
					Case "View Troubleshooting"
						column.Visible = IIf(IsTroubleshoot = 2, True, False)
					Case "Add To Inspection"
						column.Visible = IIf(IsTroubleshoot = 2, True, False)
					Case "Item Sequence No."
						column.Visible = IIf(TransTypeID = 116, False, True)
				End Select

			Next

			cmbStatus.Enabled = IIf(IsTroubleshoot = 1 Or IsTroubleshoot = 2, False, True)
			cmbStatus.Visible = IIf(IsTroubleshoot = 2, False, True)
			lblStatus.Visible = IIf(IsTroubleshoot = 2, False, True)

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

	Public Sub SendMail(FilePath As String,
						Optional IsForNewDiscrepancyImported As Boolean = False,
						Optional ImportedDiscrepancy As MELSnagCorrectiveAction = Nothing)

		Dim str As String

		Try

			If IsForNewDiscrepancyImported = False Then
				str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Attached file has list of log(s) which failed while transferring from CRS and sent by  <b>" + User.Identity.Name + "</b>" + " in FlyPal System." + "</font></P></br> ")

				str = str + ("<p><font face=""Calibri"">")
				str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
				str = str + ("</body></html>")
				Session("UserEmailID") = "support@bytzsoft.com"
				SendMailFile.SendMailFile(Session("CrystalReport"),
										  User.Identity.Name,
										  "List of Failed Logs from CRS",
										  "",
										  Info:=str,
										  VendorEmailID:="",
										  ToMailID:=Session("UserEmailID"),
										  CCMailID:="",
										  BCCMailID:="",
										  ReportPath:=FilePath,
										  SmtpHost:=Session("SmtpHost"),
										  SmtpPort:=Session("SmtpPort"),
										  SmtpUser:=Session("SmtpUser"),
										  SmtpPassword:=Session("SmtpPassword"))


			Else

				If ImportedDiscrepancy IsNot Nothing Then

					str = str + ("<html>" & "<head>" & "</head>" & "<body >" &
						  "<P><font face=""Calibri"">New Discrepancy has been added in FlyPal System and need your attention." + "</font></P></br> ")

					str = str + "<p><font face=""Calibri"">"
					str = str + "<b> Aircraft : </b>" + ImportedDiscrepancy.RegNo + "<b>" + "  Log No : " + "</b>" + ImportedDiscrepancy.LogNo
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + ("<b>Discrepancy No. : " + "</b>" +
								ImportedDiscrepancy.DefectNo + "<b>  Date of Occurrence : </b>" +
								ImportedDiscrepancy.DateOfOccurrenceFormatted)
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + "<b>" + " Discrepancy : " + "</b>" + ImportedDiscrepancy.Defect
					str = str + "</font></p>"
					str = str + "<p><font face=""Calibri"">"
					str = str + "<b>" + " Reported By : " + "</b>" + ImportedDiscrepancy.ReportedBy
					str = str + "</font></p>"
					str = str + "</body></html>"
					str = str + ("</br></br><p><font face=""Calibri"">")
					str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")
					str = str + ("</body></html>")

					SetUserMailIDs()

					SendMailFile.SendMailFile(Session("CrystalReport"),
											  User.Identity.Name,
											  "New Discrepancy Reported",
											  "",
											  Info:=str,
											  VendorEmailID:="",
											  ToMailID:=Session("UserEmailID"),
											  CCMailID:="",
											  BCCMailID:="",
											  ReportPath:=FilePath,
											  SmtpHost:=Session("SmtpHost"),
											  SmtpPort:=Session("SmtpPort"),
											  SmtpUser:=Session("SmtpUser"),
											  SmtpPassword:=Session("SmtpPassword"))


				End If

			End If

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"openTransDetail",
												MessageBox.Show("Mail Sent Successfully",
																	   False),
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetPBHValues(ID As Guid, MachineID As Guid, Optional HourDiff_Dec As Decimal = 0)

		Try

			Dim CompanyDetail As New CompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022

			If CompanyDetail.IsCombinedHours = False Then

				Dim TmpLog As Log
				Dim mPBH As PBH

				If ID.Equals(Guid.Empty) Then

					mPBH = PBH.GetPBHByMachine(MachineID, "")

					If Not mPBH.MachineID.Equals(Guid.Empty) Then

						If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
							mPBH.CurrentHours = mPBH.StartHoursDec
							mPBH.ElapsedHours = 0
							mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value

							'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
							'Also mark Not InUse in tabMachine at same time 

							If mPBH.RemainingHoursDec <= 0 Then

								mPBH.IsNotActive = True
								mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
								mPBH.MachineNotInUse = True
								Session("IsAircraftMadeNotInUse") = "True"

							End If

						End If

					End If

				Else

					TmpLog = Log.GetLog(ID)
					mPBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")

					If Not mPBH.MachineID.Equals(Guid.Empty) Then

						If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

							mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
							mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
							mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value

							'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
							'Also mark Not InUse in tabMachine at same time 

							If mPBH.RemainingHoursDec <= 0 Then

								mPBH.IsNotActive = True
								mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
								mPBH.MachineNotInUse = True
								Session("IsAircraftMadeNotInUse") = "True"

							End If

						End If

					End If

				End If

				mPBH.Save()

			ElseIf CompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022

				Dim mPBH As PBH
				Dim TmpLog As Log
				Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
				mPBH = PBH.GetPBH(mPBHList(0).ID)

				If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

					mPBH = PBH.GetPBH(mPBHList(0).ID)

					If (mPBH.RemainingHoursDec + HourDiff_Dec) <= (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) Then
						mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec + HourDiff_Dec, 1, False, False).Value

						If mPBH.CarryForwardHoursDec < 0 Then
							mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec) - mPBH.RemainingHoursDec, 1, False, False).Value
						Else
							mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.RemainingHoursDec, 1, False, False).Value
						End If


						If mPBH.RemainingHoursDec <= 0 Then
							mPBH.IsNotActive = True
							mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
							mPBH.MachineNotInUse = True
							Session("IsAircraftMadeNotInUse") = "True"
						End If

						mPBH.Save()

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Public Function CustomValidateLog2() As String    'For DgLog Fuel Oils

		Dim str As String = ""
		Dim mLog As Log

		Try

			mLog = Session("mLog")
			Dim LogStr As String = mLog.RegNo + " :" + mLog.LogNoLogPageNo + " "

			If Not mLog.IsValid Then

				For j As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
					str = str + LogStr + mLog.GetBrokenRulesCollection(j).Description + "<BR>"
				Next

			End If

			'AirFrame
			For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1

				If Not mLog.LogAFAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next

				End If

			Next

			'Engine
			For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

				If Not mLog.LogEngAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next

				End If

			Next

			'APU
			For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

				If Not mLog.LogAPUAssemblies(i).IsValid Then

					Dim x As Integer

					For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
					Next

				End If

			Next

			'Log Oils
			For i As Integer = 0 To mLog.LogOils.Count - 1

				If Not mLog.LogOils(i).IsValid Then

					For j As Integer = 0 To mLog.LogOils(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogOils.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
					Next

				End If

			Next

			For i As Integer = 0 To mLog.FuelUpLifts.Count - 1

				If Not mLog.FuelUpLifts(i).IsValid Then

					For j As Integer = 0 To mLog.FuelUpLifts(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.FuelUpLifts.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
					Next

				End If

			Next

			For i As Integer = 0 To mLog.LogFuels.Count - 1

				If Not mLog.LogFuels(i).IsValid Then

					For j As Integer = 0 To mLog.LogFuels(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogFuels.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
					Next

				End If

			Next

			For i As Integer = 0 To mLog.LogDetails.Count - 1

				If Not mLog.LogDetails(i).IsValid Then

					For j As Integer = 0 To mLog.LogDetails(i).GetBrokenRulesCollection.Count - 1
						str = str + LogStr + mLog.LogDetails.Item(i).GetBrokenRulesCollection(j).Description + "<BR>"
					Next

				End If

			Next

			If str <> "" Then
				Return str
			End If

			Return ""

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

	Private Sub ImportCRSLogs()

		Dim mLogPageNo As String = ""
		Dim mCRSLogs As CRSLogTransfer
		Dim mLog As Log
		Dim mError As Boolean = False
		mCRSLogs = CRSLogTransfer.GetLogList()
		Dim mMachine As Machine

		Try

			If mCRSLogs.Count > 0 Then

				For i As Integer = 0 To mCRSLogs.Count - 1

					mMachine = Machine.GetMachine(mCRSLogs(i).MachineID, False)
					If mMachine.IsReadOnly Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1, mMachine.RegNo + " is ReadOnly aircraft, so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)
						FileClose(1)

						mError = True

						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
						MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).LogPageNo + " : " + mMachine.RegNo + " is ReadOnly aircraft, so cannot be transferred into system. ", ErrorType.UnhandledError, Guid.Empty, EventLogID)

						GoTo 2

					ElseIf mMachine.NotInUse Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1, mMachine.RegNo + " is Not In Use since " + mMachine.NotInUseDateFormatted + ", so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)
						FileClose(1)

						mError = True

						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
						MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).LogPageNo + " : " + mMachine.RegNo + " is Not In Use since " + mMachine.NotInUseDateFormatted + ", so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system.", ErrorType.UnhandledError, Guid.Empty, EventLogID)

						GoTo 2

					End If

					Dim dtString As DateTime = CType(mCRSLogs(i).DateFormatted.ToString.Trim + " " + "23:59", DateTime)
					mLog = Log.NewCRSLog(Guid.NewGuid, mMachine, mCRSLogs(i).DateFormatted, "", dtString.ToString, 1)
					mLog.IsSyncFromCRS = True
					mLog.IsUTC = True
					mLog.IsTakeoffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean)
					mLog.LogPageNo = mCRSLogs(i).LogPageNo
					mLog.FlightNo = mCRSLogs(i).FlightNo

					If Not mCRSLogs(i).PICID.Equals(Guid.Empty) Then
						mLog.PilotID1 = mCRSLogs(i).PICID
					Else
						mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
					End If

					mLog.PilotID2 = mCRSLogs(i).SICID
					mLog.FlightLogClassificationID = mCRSLogs(i).FlightLogClassificationID
					mLog.SourceID = mCRSLogs(i).FromPlaceID
					mLog.DestinationID = mCRSLogs(i).ToPlaceID

					mLog.SouUniverseDateTime = mCRSLogs(i).UTCChocksOffDateTimeFormatted
					mLog.TakeOffUniverseDateTime = mCRSLogs(i).UTCTakeOffDateTimeFormatted
					mLog.TouchDownUniverseDateTime = mCRSLogs(i).UTCTouchDownDateTimeFormatted
					mLog.DesUniverseDateTime = mCRSLogs(i).UTCChocksOnDateTimeFormatted

					mLog.Remark = mCRSLogs(i).Remark

					For j As Integer = 0 To mLog.LogAFAssemblies.Count - 1

						If mLog.LogAFAssemblies(j).LogPeriods.Contains(3) Then mLog.LogAFAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						If mLog.LogAFAssemblies(j).LogPeriods.Contains(7) Then mLog.LogAFAssemblies(j).Landings = mCRSLogs(i).Landings.ToString

					Next

					For j As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

						If mLog.LogAPUAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogAPUAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					For j As Integer = 0 To mLog.LogCGBAssemblies.Count - 1

						If mLog.LogCGBAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogCGBAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					For j As Integer = 0 To mLog.LogEngAssemblies.Count - 1

						If mLog.LogEngAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogEngAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						End If

					Next

					If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mCRSLogs(i).Cycles.ToString)
					If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mCRSLogs(i).Landings.ToString)

					Session("mLog") = mLog
					Dim mLogDetail As LogDetail

					mLogDetail = Session("mLogDetail")
					mLog = Session("mLog")

					If mLog.IsValid Then

						mLog.CRSLogTransferID = mCRSLogs(i).ID
						mLog.Save()

						'Discrepancies

						If mCRSLogs(i).DiscrepancyCount > 0 Then

							Dim mCRSLogTransferDiscrepancies As CRSLogTransferDiscrepancies = CRSLogTransferDiscrepancies.GetLogDiscrepancyList(mCRSLogs(i).ID)

							If mCRSLogTransferDiscrepancies.Count > 0 Then

								For m As Integer = 0 To mCRSLogTransferDiscrepancies.Count - 1

									Dim MELSnagCorrectiveAction As MELSnagCorrectiveAction
									Dim AssemblyList As AssemblyList = AssemblyList.GetAssemblyListForComboBox(0, mMachine.ID.ToString, mLog.DateFormatted.ToString, "", True)

									MELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(AssemblyList(0).AssemblyStatusID.ToString)

									MELSnagCorrectiveAction.Defect = mCRSLogTransferDiscrepancies(m).Discrepancy
									MELSnagCorrectiveAction.Sector = mLog.SourceName

									If Not mCRSLogTransferDiscrepancies(m).ReportCrewID.Equals(Guid.Empty) Then
										Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(mCRSLogTransferDiscrepancies(m).EmployeeName, User.Identity.Name, WithoutLicenseNoAlso:=1)
										MELSnagCorrectiveAction.ReportedBy = mLicenses(0).LicenseNoEmpName
									End If

									MELSnagCorrectiveAction.DefectReportNo = "Dscr" + "/" + mMachine.RegNo
									MELSnagCorrectiveAction.LogID = mLog.ID
									MELSnagCorrectiveAction.DateOfOccurrence = mLog.DateFormatted
									MELSnagCorrectiveAction.RegNo = mLog.RegNo

									If mLog.LogAFAssemblies(0).FinalLandings = "" Or mLog.LogAFAssemblies(0).FinalLandings = "0" Then
										MELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H"
									Else
										MELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H" + ", " + mLog.LogAFAssemblies(0).FinalLandings + " L"
									End If

									If mLog.LogAFAssemblies(0).FinalCycles = "" Or mLog.LogAFAssemblies(0).FinalCycles = "0" Then
										MELSnagCorrectiveAction.LastMajorCheckHour = MELSnagCorrectiveAction.LastMajorCheckHour
									Else
										MELSnagCorrectiveAction.LastMajorCheckHour = MELSnagCorrectiveAction.LastMajorCheckHour + ", " + mLog.LogAFAssemblies(0).FinalCycles + " C"
									End If

									MELSnagCorrectiveAction.IsSyncFromCRS = True

									If MELSnagCorrectiveAction.IsValid Then
										MELSnagCorrectiveAction.Save()
										MELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(MELSnagCorrectiveAction.ID)
										SendMail("", IsForNewDiscrepancyImported:=True, ImportedDiscrepancy:=MELSnagCorrectiveAction)
									End If

								Next

							End If

						End If
						'********************

						mLogPageNo = mCRSLogs(i).LogPageNo

						Dim mLogList As LogList
						mLogList = LogList.GetLogList(mMachine.ID, Show_100_Records:=True)

						If mLogList.Count > 1 Then
							SetPBHValues(mLogList(1).ID, Guid.Empty, mLog.LogAFAssemblies(0).HoursDec)
						Else
							SetPBHValues(Guid.Empty, mMachine.ID)
						End If

					Else

						Dim str As String = ""
						str = CustomValidateLog2()
						str = str.Replace("<BR>", vbCrLf)
						mError = True

						If str <> "" Then

							FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs.txt", OpenMode.Append, OpenAccess.ReadWrite)
							WriteLine(1, str + vbCrLf)
							FileClose(1)
							SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")
							MarkLog(Action.Save, "Flight Log", "Log(s) failed for importing " + mCRSLogs(i).AircraftRegNo + " (" + mCRSLogs(i).LogPageNo + ") :" + str, ErrorType.UnhandledError, Guid.Empty, EventLogID)

							GoTo 2

						End If

					End If

2:              Next

				If mError = True Then

					MSGBoxCtrl.Show("Success",
									"Log(s) Imported successfully with some error(s).",
									"Check file for error(s) " + AppSettings("DOCPath") & "ImportedFailedLogs",
									MsgBoxStyle.OkOnly,
									"Success")

				Else

					MSGBoxCtrl.Show("Success",
								"Log(s) Imported successfully",
								"",
								MsgBoxStyle.OkOnly,
								"Success")

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetLabelsAndVisibility()

		Dim ShowAllDropDownOptions As Boolean = (TransTypeID = 115)
		Try

			If TransTypeID = 116 Then

				lblTitle.Text = "Cabin Defect List"
				lblDiscrepancyNo.Text = "Cabin Defect No."
				btnAddNew.ToolTip = "Add New Cabin Defect"
				btnClose.ToolTip = "Close Cabin Defect List Screen"
				btnFindNow.ToolTip = "Search Cabin Defects as per searching criteria"

				cmbMELSnag.Visible = False
				cmbDefectType.Visible = False
				lblMELOrDeviation.Visible = False
				lblDefectType.Visible = False

			End If

			If IsTroubleshoot = 1 Then 'TroubleShoot Link

				lblTitle.Text = $"{IIf(TransTypeID = 116, "Open Cabin Defect List.", "Deferred Discrepancy List.")}"
				btnClose.ToolTip = $"Close Open {Prefix} List Screen."
				btnAddNew.Visible = False

			ElseIf IsTroubleshoot = 2 Then 'WatchList Link

				lblTitle.Text = $"Discrepancy Watchlist"
				btnAddNew.Visible = False

			End If

			If Not ShowAllDropDownOptions Then

				Dim deferredItem As ListItem = cmbStatus.Items.FindByText("Deferred")

				If deferredItem IsNot Nothing Then
					cmbStatus.Items.Remove(deferredItem)
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " DataBinding "

	Public Sub DataFieldBind()

		Try

			AircraftList = MachineNameValueList.GetMachineList(CurrentDate:="", SkipIsForInventoryAircarft:=True)
			cmbAircraft.DataSource = AircraftList
			Session("MachineNameValueList") = AircraftList

			ATAList = ATAList.GetATAList(ATANomenclature:="", AddTopItem:="(ALL)")
			Session("ATAList") = ATAList
			cmbATAChapter.DataSource = ATAList

			If ATAList.Count <> 0 Then
				If IsNothing(ATAChapterId) Then ATAChapterId = ATAList(0).ID.ToString Else ATAChapterId = ATAChapterId
			Else
				ATAChapterId = "00000000-0000-0000-0000-000000000000"
			End If

			If AircraftId Is Nothing Then
				AircraftId = AircraftList(0).ID.ToString
			End If

			AssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=0,
																   MachineID:=AircraftId,
																   InstalledOn:=Today.Date.ToString,
																   AddTopItem:="(ALL)",
																   IsInstalled:=True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList

			IncidentTypeList = IncidentTypeList.GetIncidentTypeList(AddTopItem:="(ALL)")
			cmbIncidentType.DataSource = IncidentTypeList
			Session("IncidentTypeList") = IncidentTypeList

			If IncidentTypeList.Count <> 0 Then

				If Session("TypeOfIncidentID") Is Nothing Then
					TypeOfIncidentID = IncidentTypeList(0).ID.ToString
					Session("TypeOfIncidentID") = TypeOfIncidentID
				Else
					TypeOfIncidentID = CType(Session("TypeOfIncidentID"), Integer)
				End If

			Else
				TypeOfIncidentID = -1
			End If

			DataBind()

			If AircraftList.Count > 1 And
			   IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 0 Else cmbAircraft.SelectedValue = AircraftId

			AircraftId = cmbAircraft.SelectedValue
			Session("AircraftId") = AircraftId

			If ATAList.Count > 1 And IsNothing(AircraftId) Then cmbATAChapter.SelectedIndex = 0 Else cmbATAChapter.SelectedValue = ATAChapterId

			ATAChapterId = cmbATAChapter.SelectedValue
			Session("ATAChapterId") = ATAChapterId

			If CompanyDetail Is Nothing Then

				CompanyDetail = CompanyDetail.GetCompanyDetail("",
															   "",
															   "",
															   "",
															   "",
															   "",
															   "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
				Session("CompanyDetail") = CompanyDetail

			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			ClearAll()
			GetSession()
			AddAttributes()

			If Session("ShowNoEntries") Is Nothing Then

				ddlShowEntries.SelectedValue = "4"
				Session("ShowNoEntries") = ddlShowEntries.SelectedValue
				ShowNoEntries = ddlShowEntries.SelectedValue

			End If

			EventLogID = CType(Session("EventLogID"), Guid)

			TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
							  CInt(Request.QueryString("TransTypeID")),
							  115)

			Prefix = IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")

			Session("TransTypeID") = TransTypeID

			If Not IsPostBack Then

				If cmbAircraft.Enabled = True Then
					SetFocus(cmbAircraft)
				End If

				IsTroubleshoot = Request.QueryString("Troubleshoot")
				Session("IsTroubleshoot") = IsTroubleshoot
				Session("MiddleFrame") = $"wfDiscrepancyCorrectiveActionList_AJAX.aspx?Troubleshoot={IsTroubleshoot}&TransTypeID={TransTypeID}"

				DataFieldBind()
				SetControl()
				PreserveStateOfFavIcon()

			End If

			SetLabelsAndVisibility()

			If CompanyDetail.IsSyncApplication Then
				spnImportFromCRS.Visible = True
			Else
				spnImportFromCRS.Visible = False
			End If

			SetGrid()

			ModuleName = $"{IIf(TransTypeID = 116,
								"CabinDefectAction",
								"DiscrepancyAction")}"

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SearchRecords(sender As Object, e As ImageClickEventArgs) Handles btnFindNow.Click

		Try

			Session("AircraftId") = cmbAircraft.SelectedValue
			Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
			FromDate = IIf(txtFromDate.Text.ToString <> "", txtFromDate.Text.ToString, "")
			ToDate = IIf(txtToDate.Text.ToString <> "", txtToDate.Text.ToString, "")
			StatusCode = cmbStatus.SelectedValue
			MELSnagCode = cmbMELSnag.SelectedValue
			AssemblyId = cmbAssembly.SelectedValue
			DefectType = cmbDefectType.SelectedValue
			TypeOfIncidentID = CInt(cmbIncidentType.SelectedValue)
			ShowNoEntries = ddlShowEntries.SelectedValue
			DefectText = txtText.Text.Trim.ToString
			DefectNo = Val(txtNo.Text.Trim.ToString)

			Session("FromDate") = FromDate
			Session("ToDate") = ToDate
			Session("Name") = Name
			Session("ATACode") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
			Session("ATANomenclature") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
			Session("ATAChapterId") = ATAList(New Guid(cmbATAChapter.SelectedValue)).ID.ToString
			Session("StatusCode") = StatusCode
			Session("MELSnagCode") = MELSnagCode
			Session("AssemblyId") = AssemblyId
			Session("DefectType") = DefectType
			Session("TypeOfIncidentID") = TypeOfIncidentID
			Session("ShowNoEntries") = ShowNoEntries
			Session("DefectText") = DefectText
			Session("DefectNo") = DefectNo

			FindNow(FromDate:=txtFromDate.Text.ToString,
					ToDate:=txtToDate.Text.ToString,
					MachineID:=mMachineID.ToString,
					InvestigationStatus:=StatusCode,
					ATACode:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode,
					ATANomenclature:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
					MELSnag:=MELSnagCode,
					AssemblyStatusID:=cmbAssembly.SelectedValue.ToString,
					DefectType:=DefectType,
					TypeOfIncidentID:=TypeOfIncidentID,
					DefectText:=DefectText,
					DefectNo:=DefectNo)

			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()

			PreserveStateOfFavIcon()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgDiscrepancyCorrectiveActionList.RowCommand

		Try

			Dim DiscrepancyCorrectiveActionID = New Guid(e.CommandArgument.ToString)

			Select Case e.CommandName
				Case "EditRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.Edit, Action.View}) Then

						Exit Sub

					End If

					EditRecord(DiscrepancyCorrectiveActionID:=DiscrepancyCorrectiveActionID)

				Case "AttachRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.View}) Then

						Exit Sub

					End If

					Dim FileAttach As FileAttach
					Dim DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
					DiscrepancyCorrectiveAction =
						MELSnagCorrectiveAction.
							GetMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID)
					FileAttach = FileAttach.GetAttachment(ReferenceID:=DiscrepancyCorrectiveAction.ID)

					Session("mFileAttach") = FileAttach

					AttachmentHelper.DownloadAttachmentWithName(AttachmentObject:=FileAttach)

					ScriptManager.RegisterStartupScript(Me, [GetType], "Download Attachment", "openFile();", True)


				Case "PrintRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.Print}) Then

						Exit Sub

					End If

					Dim CrystalReport As Engine.ReportClass
					Dim dataSet As New dsMELSnagCorrectiveAction
					Dim dataAdapter As New ObjectAdapter
					Dim CompanyDetail As New CompanyDetail
					Dim IsMEL As Boolean = DiscrepancyCorrectiveActionList(ID:=DiscrepancyCorrectiveActionID).IsMEL
					Dim MELSnagCorrectiveAction As rptMELSnagCorrectiveAction =
														rptMELSnagCorrectiveAction.
															GetrptMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID.ToString)

					Dim ReportData As New ReportData(CompanyDetail.CompanyName,
													 CompanyDetail.Address,
													 CompanyDetail.Tel1,
													 CompanyDetail.Tel2,
													 CompanyDetail.Fax,
													 CompanyDetail.Email,
													 CompanyDetail.WebSite,
													 "PRELIMINARY DEFECT REPORT", "", "", "", "", "",
													 AppSettings("Product Version"),
													 AppSettings("SINote"), "", "", "", "",
													 AppSettings("Logo")) 'Changed By Utkarsh For Report Logo.

					If IsMEL Then
						CrystalReport = New crMELDetailReport
					Else
						CrystalReport = New crLogDefectActionList
					End If

					Dim CompanyLogo As rptImage = rptImage.GetImage(dataSet)
					dataAdapter.Fill(dataSet, MELSnagCorrectiveAction)
					dataAdapter.Fill(dataSet, ReportData)
					dataAdapter.Fill(dataSet, CompanyLogo)
					CrystalReport.SetDataSource(dataSet)
					Session("CrystalReport") = CrystalReport

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Display Report",
														"displayReportInPDF();",
														True)

				Case "DeleteRec"

					If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
																	MSGBoxCtrl:=MSGBoxCtrl,
																	ModuleName:=Prefix,
																	TransTypeID:=TransTypeID,
																	Action:={Action.Delete}) Then

						Exit Sub

					End If

					DeleteRecord(ID:=DiscrepancyCorrectiveActionID)

				Case "TroubleShootRec"

					DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID)
					Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
					SetSession()
					Session.Remove("mLog")
					Session("TroubleShootFromLog") = "False"
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Discrepancy TroubleShoot Window",
														"OpenDiscrepancyTroubleShootWindow()",
														True)


				Case "AddToInspection" 'Added By Prashant 4-Mar-2024

					DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID)
					Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

					SetSession()

					Dim mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
					Dim mModelMonitorInsp As ModelMonitorInsp
					Dim mMachine As Machine
					Dim mAssemblyStatus As AssemblyStatus
					Dim ID As Guid = Guid.NewGuid 'Revise Activity

					mMachine = Machine.GetMachine(MachineID:=New Guid(AircraftId))
					mAssemblyStatus = AssemblyStatus.GetAssemblyStatus(mMachine.AssemblyStatus.ID)

					Session("mMachine") = mMachine
					Session("mAssemblyStatus") = mAssemblyStatus
					Session("CloseDate") = DiscrepancyCorrectiveAction.RectifiedDate.ToString

					mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewAssemblyMonitorInspStatus(ID:=Guid.NewGuid, mAssemblyStatus.AssemblyID,
																										AssemblyStatusID:=mAssemblyStatus.ID,
																										AsOnDate:=DiscrepancyCorrectiveAction.RectifiedDate.ToString,
																										ModelID:=mAssemblyStatus.Assembly.ModelID,
																										HourType:=mMachine.HourType)

					mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(ID:=ID,
																			 ModelID:=mAssemblyStatus.Assembly.ModelID,
																			 HourType:=mMachine.HourType,
																			 PrevRefID:=ID) 'For new records ID,PrevRefID are same

					mModelMonitorInsp.Description = DiscrepancyCorrectiveAction.PreventionTaken
					mModelMonitorInsp.ATAID = DiscrepancyCorrectiveAction.ATAChapterID
					mModelMonitorInsp.Code = DiscrepancyCorrectiveAction.DefectNo
					mModelMonitorInsp.WatchItemID = DiscrepancyCorrectiveAction.ID
					Session("mModelMonitorInsp") = mModelMonitorInsp
					mModelMonitorInsp.BeginEdit()

					mAssemblyMonitorInspStatus.LogID(mLogID:=DiscrepancyCorrectiveAction.RectifiedLogID.ToString,
													 mLogDate:=DiscrepancyCorrectiveAction.RectifiedDateFormatted.ToString,
													 IsFromMain:=True,
													 mModelMonitorInsp:=CType(Session("mModelMonitorInsp"), ModelMonitorInsp)) = DiscrepancyCorrectiveAction.RectifiedLogID

					Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

					Dim AirframeCurrentValues As String
					Dim mAircraftCurrValue As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(, mMachine.RegNo, , , ,
																																	  CurrentDate:=Today.Date.ToString)
					AirframeCurrentValues = mAircraftCurrValue(0).ShowPeriods

					Session("NewPage") = "True"
					Session("mIssueDate") = DiscrepancyCorrectiveAction.RectifiedDateFormatted
					Session("RectifiedLogID") = DiscrepancyCorrectiveAction.RectifiedLogID.ToString
					Session("IsOpenFromMPD") = "True"
					Session("OpenFromDiscrepancyCorrectiveActionList") = "True"
					Session("AirframeCurrentValues") = AirframeCurrentValues
					Session("FromEditThresholdInterval") = "False"

					MarkLog(Action.[New],
							"Model Monitor Insp",
							" Model : " & mAssemblyStatus.Assembly.ModelName,
							ErrorType.NoError,
							Guid.Empty,
							EventLogID)

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Script",
														"openPageInSameTab('wfInspectionForWatchItem.aspx?BackPage=Index.aspx');",
														True)

				Case "ViewDetails"

					EditRecord(DiscrepancyCorrectiveActionID:=DiscrepancyCorrectiveActionID)

				Case "ViewTroubleshoot"

					DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=DiscrepancyCorrectiveActionID)
					Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Discrepancy TroubleShoot View",
														"OpenDiscrepancyTroubleshootView();",
														True)

			End Select

			PreserveStateOfFavIcon()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgDiscrepancyCorrectiveActionList.PageIndexChanging

		Try

			dgDiscrepancyCorrectiveActionList.PageIndex = e.NewPageIndex
			dgDiscrepancyCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			dgDiscrepancyCorrectiveActionList.PageSize = CInt(ddlShowEntries.SelectedItem.ToString)
			dgDiscrepancyCorrectiveActionList.DataBind()
			SetGrid()

			PreserveStateOfFavIcon()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub GV_SnagCorrectiveActionList_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgDiscrepancyCorrectiveActionList.Sorting

		Try

			DiscrepancyCorrectiveActionList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			dgDiscrepancyCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			dgDiscrepancyCorrectiveActionList.DataBind()
			SetGrid()

			PreserveStateOfFavIcon()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub AddRecord(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Dim str As String
		Try

			If Not AuthorizationHelper.CheckIfUserHasRights(User:=User,
															MSGBoxCtrl:=MSGBoxCtrl,
															ModuleName:=Prefix,
															TransTypeID:=TransTypeID,
															Action:={Action.[New]}) Then

				Exit Sub

			End If

			NewRecord()

			Session("IsFromLog") = False
			Session("MachineID") = cmbAircraft.SelectedValue.ToString
			Session("AircraftRegNo") = cmbAircraft.SelectedItem.ToString
			Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			AircraftId = Session("MachineID")
			DiscrepancyCorrectiveAction.MachineID = New Guid(AircraftId)
			DiscrepancyCorrectiveAction.AircraftID = New Guid(AircraftId)

			MarkLog(Action.[New],
					"DiscrepancyAction",
					"",
					ErrorType.NoError,
					Guid.Empty,
					EventLogID) 'Added By Prashant 20-Jul-2011

			str = $"openPageInSameTab('wfDiscrepancyCorrectiveAction.aspx?BackPage=Index.aspx&TransTypeID={Session("TransTypeID")}');"

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Open Script",
												str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub ClosScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			RemoveSession()

			Session("sender") = ""
			Session("MiddleFrame") = ""
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged

		Try

			AssemblyList = AssemblyList.GetAssemblyListForComboBox(AssemblyTypeID:=0,
																   MachineID:=cmbAircraft.SelectedValue.ToString,
																   InstalledOn:=Today.Date.ToString,
																   AddTopItem:="(ALL)",
																   IsInstalled:=True)
			cmbAssembly.DataSource = AssemblyList
			Session("mAssemblylist") = AssemblyList
			cmbAssembly.DataBind()
			Session("AircraftId") = cmbAircraft.SelectedValue
			Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

			FindNow(FromDate:=txtFromDate.Text.ToString,
					ToDate:=txtToDate.Text.ToString,
					MachineID:=cmbAircraft.SelectedValue.ToString,
					InvestigationStatus:=cmbStatus.SelectedValue,
					ATACode:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode,
					ATANomenclature:=ATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature,
					MELSnag:=cmbMELSnag.SelectedValue,
					AssemblyStatusID:=cmbAssembly.SelectedValue.ToString,
					DefectType:=Val(cmbDefectType.SelectedValue),
					TypeOfIncidentID:=CInt(cmbIncidentType.SelectedValue),
					DefectText:=txtText.Text.Trim,
					DefectNo:=txtNo.Text.Trim)

			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()
			upnlAvanceSearchContent.Update()

			If cmbAircraft.Enabled = True Then
				SetFocus(cmbAircraft)
			End If

			dgDiscrepancyCorrectiveActionList.PageSize = CInt(ddlShowEntries.SelectedItem.ToString)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	'Added By on Harsh on 7th Feb 2024
	Protected Sub ShowEntriesChanged(sender As Object, e As EventArgs)

		Try

			dgDiscrepancyCorrectiveActionList.PageSize = CInt(ddlShowEntries.SelectedItem.ToString)
			dgDiscrepancyCorrectiveActionList.DataSource = DiscrepancyCorrectiveActionList
			dgDiscrepancyCorrectiveActionList.DataBind()
			Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
			upnlGridView.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click

		Try
			MarkFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click

		Try
			RemoveFavourite(HttpContext.Current.User.Identity.Name, ModuleName)
		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub PreserveStateOfFavIcon()

		If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, ModuleName) Then

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Mark As Favourite",
												"MarkAsFavourite();",
												True)

		Else

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Remove From Favourite",
												"RemoveFromFavourite();",
												True)

		End If

	End Sub

	Private Sub HdnBtnDiscrepancyTroubleShoots(sender As Object, e As EventArgs) Handles hdnBtnDiscrepancyTroubleShoot.Click

		Try

			DataFieldBind()
			SetControl()
			SetGrid()
			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub HdnBtnModelInspMasters(sender As Object, e As EventArgs) Handles hdnBtnModelInspMaster.Click

		Try

			DataFieldBind()
			SetControl()
			SetGrid()
			upnlGridView.Update()
			upnlActionBtnTop.Update()
			upnlResult.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub HdnBtnImportCRSLog(sender As Object, e As EventArgs) Handles hdnBtnImportCRSLogs.Click

		Try

			ImportCRSLogs()
			SetControl()
			upnlGridView.Update()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Service Methods "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetTextList(prefixText As String, count As Integer) As String()

		Dim DistinctTextList As DistinctTextListAutoComplete
		Dim TransTypeID = HttpContext.Current.Session("TransTypeID")

		DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(Text:=prefixText,
																			[Of]:=33,
																			IsForText:=True,
																			TransTypeID:=TransTypeID)

		Try

			If count = 0 Then
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
			Else
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Function

#End Region

End Class