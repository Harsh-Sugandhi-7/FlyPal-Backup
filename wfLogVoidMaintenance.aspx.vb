Public Class wfLogVoidMaintenance
	Inherits Page

#Region " Variable Declaration "

	Public mLog As Log
	Public mMachine As Machine
	Dim mLogDetail As String
	Dim mFileAttachLogMaint As FileAttach
	Dim IsAttachmentDeleted As Boolean = False
	Public mMaintLogListOnDate As LogList

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		mLog = CType(Session("mLog"), Log)
		mMachine = CType(Session("mMachine"), Machine)
		mFileAttachLogMaint = Session("mFileAttachLogMaint")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mMaintLogListOnDate = Session("mMaintLogListOnDate")
	End Sub

	Private Sub SetSession()
		Session("mLog") = mLog
		Session("mMachine") = mMachine
		Session("mMaintLogListOnDate") = mMaintLogListOnDate
	End Sub

	Private Sub RemoveSession()
		Session.Remove("mLog")
		Session.Remove("mMachine")
		Session.Remove("mFileAttachLogMaint")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("mMaintLogListOnDate")
	End Sub

	Private Sub ControlVisibility()

		Try

			txtLogDate.Enabled = IIf((AppSettings("ClientCode") = "Heligo" Or
									  AppSettings("ClientCode") = "UHPL" Or
									  AppSettings("ClientCode") = "APFT" Or
									  AppSettings("ClientCode") = "AAP"), True, False)

			If mLog.LogTypeID = 3 Then txtLogDate.Visible = False

			lblLogDetails.Visible = (mLog.LogTypeID = 2) And (Not mLog.IsNew)
			btnDefectActionList.Visible = (mLog.LogTypeID = 2) And (Not mLog.IsNew)
			btnFuelOil.Visible = (mLog.LogTypeID = 2) And (Not mLog.IsNew)
			btnMaintenanceAcitvity.Visible = (mLog.LogTypeID = 2) And (Not mLog.IsNew)

			If mLog.IsTLP = True Then lblLogPageNo.Text = "TLP No." : txtLogPageNo.ToolTip = "Enter TLP No."

			''Added by Saylee on 13-Oct-2022, for CMX13102022 to show APU hours for entry
			'APU ----> 
			If mLog.LogAPUAssemblies.Count = 0 Then
				dgAPUPeriods.Visible = False
				lblAPUPeriod.Visible = False
			Else
				dgAPUPeriods.Visible = (mLog.LogTypeID = 2) ''And (Not mLog.IsNew)
				lblAPUPeriod.Visible = (mLog.LogTypeID = 2) ''And (Not mLog.IsNew)
			End If

			'Hours
			dgAPUPeriods.Columns(3).Visible = mLog.LogAPUAssemblies.ShowHours
			dgAPUPeriods.Columns(4).Visible = mLog.LogAPUAssemblies.ShowHours
			'Landings
			dgAPUPeriods.Columns(5).Visible = mLog.LogAPUAssemblies.ShowLandings
			dgAPUPeriods.Columns(6).Visible = mLog.LogAPUAssemblies.ShowLandings
			'Cycles
			dgAPUPeriods.Columns(7).Visible = mLog.LogAPUAssemblies.ShowCycles
			dgAPUPeriods.Columns(8).Visible = mLog.LogAPUAssemblies.ShowCycles
			'Starts
			dgAPUPeriods.Columns(9).Visible = mLog.LogAPUAssemblies.ShowStarts
			dgAPUPeriods.Columns(10).Visible = mLog.LogAPUAssemblies.ShowStarts
			'NG
			dgAPUPeriods.Columns(11).Visible = mLog.LogAPUAssemblies.ShowNGCycles
			dgAPUPeriods.Columns(12).Visible = mLog.LogAPUAssemblies.ShowNGCycles
			'NF
			dgAPUPeriods.Columns(13).Visible = mLog.LogAPUAssemblies.ShowNFCycles
			dgAPUPeriods.Columns(14).Visible = mLog.LogAPUAssemblies.ShowNFCycles
			'RINS
			dgAPUPeriods.Columns(15).Visible = mLog.LogAPUAssemblies.ShowRINS
			dgAPUPeriods.Columns(16).Visible = mLog.LogAPUAssemblies.ShowRINS
			'Bleeds  'Added By Prashant 8-July-2009
			dgAPUPeriods.Columns(17).Visible = mLog.LogAPUAssemblies.ShowBleeds
			dgAPUPeriods.Columns(18).Visible = mLog.LogAPUAssemblies.ShowBleeds
			'ImpellerCycles
			dgAPUPeriods.Columns(19).Visible = mLog.LogAPUAssemblies.ShowImpellerCycles
			dgAPUPeriods.Columns(20).Visible = mLog.LogAPUAssemblies.ShowImpellerCycles
			'CTCycles 
			dgAPUPeriods.Columns(21).Visible = mLog.LogAPUAssemblies.ShowCTCycles
			dgAPUPeriods.Columns(22).Visible = mLog.LogAPUAssemblies.ShowCTCycles
			'PTCycles  
			dgAPUPeriods.Columns(23).Visible = mLog.LogAPUAssemblies.ShowPTCycles
			dgAPUPeriods.Columns(24).Visible = mLog.LogAPUAssemblies.ShowPTCycles

			'Generator Mods
			dgAPUPeriods.Columns(25).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
			dgAPUPeriods.Columns(26).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
			'-----------------------------------------End

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetObject()
		With mLog
			If Not IsDate(txtLogDate.Text) Then
				.Date = DBNull.Value
			Else
				.Date = txtLogDate.Text.ToString.Trim
			End If

			.LogText = Trim(txtLogText.Text)
			.LogNo = CInt(Val(Trim(txtLogNo.Text)))

			.LogPageNo = txtLogPageNo.Text
			.Remark = Trim(txtRemark.Text)

			.TimeInAir = "0"

			If mFileAttachLogMaint IsNot Nothing Then
				If mFileAttachLogMaint.Size > 0 Then
					.IsAttachmentAdded = True
				Else
					.IsAttachmentAdded = False
				End If
			End If
		End With

		Session("mLog") = mLog
	End Sub

	''Added by Saylee on 13-Oct-2022, for CMX13102022 to show APU hours for entry
	Public Sub SetAPUGridObject(Optional isFromDataBindGrid As Boolean = False)        ' For Third Grid i.e APU
		Dim txtAPUHours, txtAPULandings, txtAPUCycles As TextBox, txtAPUStarts, txtAPUNGCycles, txtAPUNFCycles, txtAPURins, txtAPUBleeds, txtAPUImpellerCycles,
			txtAPUCTCycles, txtAPUPTCycles, txtAPUGeneratorMods As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAPUPeriods.Items" is replaced by "dgAPUPeriods.Rows"
		For i As Integer = 0 To Me.dgAPUPeriods.Rows.Count - 1
			txtAPUHours = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUHours"), TextBox)
			txtAPULandings = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPULandings"), TextBox)
			txtAPUCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUCycles"), TextBox)
			txtAPUStarts = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUStarts"), TextBox)
			txtAPUNGCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUNGCycles"), TextBox)
			txtAPUNFCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUNFCycles"), TextBox)
			txtAPURins = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPURins"), TextBox)
			'Added By Prashant 8-July-2009
			txtAPUBleeds = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUBleeds"), TextBox)
			'-----------------------------
			'Added By Prashant 10-Aug-2009
			txtAPUImpellerCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUImpellerCycles"), TextBox)
			txtAPUCTCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUCTCycles"), TextBox)
			txtAPUPTCycles = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUPTCycles"), TextBox)
			'-----------------------------
			txtAPUGeneratorMods = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012
			'If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAPUHours.Text)
			If isFromDataBindGrid Then If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAPUHours.Text)
			If mLog.LogAPUAssemblies(i).ShowLandings Then mLog.LogAPUAssemblies.Item(i).Landings = Trim(txtAPULandings.Text)
			If mLog.LogAPUAssemblies(i).ShowCycles Then mLog.LogAPUAssemblies.Item(i).Cycles = Trim(txtAPUCycles.Text)
			If mLog.LogAPUAssemblies(i).ShowStarts Then mLog.LogAPUAssemblies.Item(i).Starts = Trim(txtAPUStarts.Text)
			If mLog.LogAPUAssemblies(i).ShowNGCycles Then mLog.LogAPUAssemblies.Item(i).NGCycles = Trim(txtAPUNGCycles.Text)
			If mLog.LogAPUAssemblies(i).ShowNFCycles Then mLog.LogAPUAssemblies.Item(i).NFCycles = Trim(txtAPUNFCycles.Text)
			If mLog.LogAPUAssemblies(i).ShowRINS Then mLog.LogAPUAssemblies.Item(i).RINS = Trim(txtAPURins.Text)
			'Added By Prashant 8-July-2009
			If mLog.LogAPUAssemblies(i).ShowBleeds Then mLog.LogAPUAssemblies.Item(i).Bleeds = Trim(txtAPUBleeds.Text)
			'-----------------------------
			'Added By Prashant 10-Aug-2009
			If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then mLog.LogAPUAssemblies.Item(i).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
			If mLog.LogAPUAssemblies(i).ShowCTCycles Then mLog.LogAPUAssemblies.Item(i).CTCycles = Trim(txtAPUCTCycles.Text)
			If mLog.LogAPUAssemblies(i).ShowPTCycles Then mLog.LogAPUAssemblies.Item(i).PTCycles = Trim(txtAPUPTCycles.Text)
			'-----------------------------
			If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then mLog.LogAPUAssemblies.Item(i).GeneratorMods = Trim(txtAPUGeneratorMods.Text) 'Added by Shweta on 7-May-2012
		Next i
		Session("mLog") = mLog
	End Sub

	''Added by Saylee on 13-Oct-2022, for CMX13102022 to show APU hours for entry
	Private Sub DataBindGrid()
		If mLog IsNot Nothing Then
			SetAPUGridObject(True)
			dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
			dgAPUPeriods.DataBind()
			GridColumnHeadingSet()
			upnlAPUDetail.Update()
			Session("mLog") = mLog
		End If
	End Sub

	Private Sub NewRecord(LogDate As String, Optional mSouLocalDateTime As String = "", Optional mSouUTCDateTime As String = "", Optional LogTypeID As Integer = 0)
		mLog = Log.NewLog(mMachine, LogDate, mSouLocalDateTime, mSouUTCDateTime, LogTypeID)
		mLog.BeginEdit()
		mMachine = Machine.GetMachine(mMachine.ID)
		DataBind()

	End Sub

	Private Sub EditRecord(LogDate As DateTime)
		mLog = Log.GetLog(mLog.ID)

		mLog.Date = LogDate
		DataBind()

	End Sub

	Private Sub CopyFromClone(ClonedLog As Log, Optional isFromLogDate As Boolean = False)
		mLog.LogText = ClonedLog.LogText
		mLog.LogNo = ClonedLog.LogNo
		mLog.LogTypeID = ClonedLog.LogTypeID
		mLog.LogPageNo = ClonedLog.LogPageNo
		mLog.Remark = ClonedLog.Remark
		mLog.SourceID = ClonedLog.SourceID
		mLog.DestinationID = ClonedLog.DestinationID

		mLog.TimeInAir = "0"

		Session("mLog") = mLog
	End Sub

	Private Sub SetPage()
		If mLog.IsNew Then
			If mLog.Date Is DBNull.Value Then
				lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of - [New]"
			Else
				If mLog.LogTypeID = 2 Then
					lblTitle.Text = "Maintenance Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
				ElseIf mLog.LogTypeID = 3 Then
					lblTitle.Text = "VOID Log Details of " & mMachine.RegNo & " [New]"
				End If

			End If
		Else
			If mLog.LogTypeID = 2 Then
				lblTitle.Text = "Maintenance Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
			ElseIf mLog.LogTypeID = 3 Then
				lblTitle.Text = "VOID Log Details of " & mMachine.RegNo
			End If
		End If
	End Sub

	Private Sub DataFieldBind()

		dgAPUPeriods.DataSource = mLog.LogAPUAssemblies   ''Added by Saylee on 13-Oct-2022, for CMX13102022 to show APU hours for entry

		txtLogNo.Text = mLog.LogNo
		txtLogText.Text = mLog.LogText

		If mLog.Date IsNot DBNull.Value Then
			txtLogDate.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
		Else
			txtLogDate.Text = ""
		End If

		mMaintLogListOnDate = LogList.GetLogList(mMachine.ID, txtLogDate.Text.ToString, txtLogDate.Text.ToString)
		Session("mMaintLogListOnDate") = mMaintLogListOnDate
		grdLogDet.DataSource = mMaintLogListOnDate
		DataBind()

		GridColumnHeadingSet()

		upnlDetails.Update()
	End Sub

	''Added by Saylee on 13-Oct-2022, for CMX13102022 to show APU hours for entry
	Private Sub GridColumnHeadingSet()
		If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
			dgAPUPeriods.Columns(7).HeaderText = "Flights"
			dgAPUPeriods.Columns(8).HeaderText = "Final Flights"
		ElseIf (AppSettings("ClientCode") = "IND") Then
			dgAPUPeriods.Columns(17).HeaderText = "APU Hours"
			dgAPUPeriods.Columns(18).HeaderText = "Final APU Hours"
		ElseIf (AppSettings("ClientCode") = "FBW") Then
			dgAPUPeriods.Columns(17).HeaderText = "AHH"
			dgAPUPeriods.Columns(18).HeaderText = "Final AHH"
		End If
	End Sub

	Private Function Save() As Boolean

		'Authentication
		If mLog.Date IsNot DBNull.Value Then

			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

			If mCheck.WebAuthentication = True Then

				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")

				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

				'CNDC
				If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < -10 Then

					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
									MSGBox.Message_text.saveAlert,
									" Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" &
									maxAllowableDate.ToString(WebDateFormat),
									MsgBoxStyle.OkOnly,
									"")

					DataFieldBind()
					Exit Function

				End If

			End If

		End If

		'Authentication
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)

		SetObject()
		SetAPUGridObject(True)

		If mLog.IsValid = True Then

			Try

				If mLog.IsTLP = "True" Then

					If mLog.LogPageNo <> "0" Or mLog.LogPageNo <> "" Then

						Dim mPrevLogDetail As PrevLogDetail = PrevLogDetail.GetPrevLogDetail(mLog.MachineID, mLog.Date, mLog.LogPageNo)

						If mPrevLogDetail.IsTLPNODuplicate And mLog.LogNo <> mPrevLogDetail.LogNo Then

							MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
											MSGBox.Message_text.Duplicate,
											"TLP No. already exists.",
											MsgBoxStyle.OkOnly,
											"")

							mLog = LogClone
							Session("mLog") = mLog

							Return False

						End If

					End If

				End If

				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)

				SaveAttachment()

				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLogDetail = (IIf(mLog.LogTypeID = 2, "MAINT. LOG", "VOID LOG")) + " : " +
							 mLog.LogTextNo + " Dated : " + mLog.DateFormatted

				MarkLog(Action.Save,
						"Flight Log",
						mLogDetail,
						ErrorType.HandledError,
						mLog.ID,
						EventLogID)

				Session("mLog") = mLog

				MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
								MSGBox.Message_text.SavedSuccessFully,
								"",
								MsgBoxStyle.OkOnly,
								"")

				Return True

			Catch ex As SqlException

				Session("LogClone") = LogClone

				If ex.Number = 8114 Or ex.Number = 8115 Then

					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow,
									MSGBox.Message_text.NumericOverFlow,
									" Rate or Qty or Conversion Factor. ",
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 8145 Then

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.ProcedureError,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 2627 Then

					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
									MSGBox.Message_text.Duplicate,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 547 Then

					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
									MSGBox.Message_text.ReferenceDelete,
									ex.Procedure,
									MsgBoxStyle.OkOnly,
									"")

				ElseIf ex.Number = 50000 Then

					MSGBoxCtrl.show(MSGBox.Message_title.LogExist,
									MSGBox.Message_text.LogExist,
									" between Current Date and Time Span for this Aircraft. ",
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

	End Function
	'Added By Prashant 28-July-2009

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		If custValidator.ControlToValidate = "txtLogPageNo" And mLog.IsTLP = "True" Then
			If txtLogPageNo.Text.Trim = "0" Or txtLogPageNo.Text.Trim = "" Then
				custValidator.ErrorMessage = "Enter TLP No."
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		End If
	End Sub

	Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
		Dim str As String = ""
		SetObject()

		If Not mLog.IsValid Then
			For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
				str = str + mLog.GetBrokenRulesCollection(i).Description + "<Br>"
			Next
		End If

		'AirFrame
		For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1
			If Not mLog.LogAFAssemblies(i).IsValid Then
				Dim x As Integer
				For x = 0 To mLog.LogAFAssemblies(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogAFAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
				Next
			End If
		Next
		'Engine
		For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
			If Not mLog.LogEngAssemblies(i).IsValid Then
				Dim x As Integer
				For x = 0 To mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogEngAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
				Next
			End If
		Next
		'APU
		For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
			If Not mLog.LogAPUAssemblies(i).IsValid Then
				Dim x As Integer
				For x = 0 To mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection.Count - 1
					str = str + mLog.LogAPUAssemblies.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
				Next
			End If
		Next



		If str <> "" Then
			cvRemark.ErrorMessage = str
			cvRemark.IsValid = False

			Return False
		End If

		Return True
	End Function

	Private Sub GetAttachment()
		If mLog.IsAttachmentAdded And mFileAttachLogMaint Is Nothing Then
			mFileAttachLogMaint = FileAttach.GetAttachment(mLog.ID)
			Session("mFileAttachLogMaint") = mFileAttachLogMaint
		End If
	End Sub

	Private Sub ControlVisibilityForAttachment()
		If mLog.IsAttachmentAdded = True Then
			ImageButton1.Visible = True
			btnDelAttch.Enabled = True
		Else
			ImageButton1.Visible = False
			btnDelAttch.Enabled = False
		End If
	End Sub

	Private Sub SaveAttachment()

		If mFileAttachLogMaint IsNot Nothing Then

			If mFileAttachLogMaint.Size > 0 Then

				Try
					mFileAttachLogMaint.Save()
				Catch ex As Exception
					ScriptManager.RegisterClientScriptBlock(Me,
															[GetType],
															"",
															MessageBox.Show(ex.InnerException.ToString, False), True)
				End Try

			Else

				If (Not mLog.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAttachment(mFileAttachLogMaint.ID, mLog.ID)
				End If

				IsAttachmentDeleted = False
				Session("IsAttachmentDeleted") = IsAttachmentDeleted

			End If

		End If

	End Sub

	Private Sub ViewImage()
		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString
		GetAttachment()
		If mFileAttachLogMaint.Size > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttachLogMaint.Extension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				File.Delete(AppSettings("DOCPath") & StrName & mFileAttachLogMaint.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileAttachLogMaint.ImageFile, 0, mFileAttachLogMaint.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, [GetType], "openFile", "openFile();", True)
			End If
		End If
	End Sub

#End Region

#Region "Events"

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)
		If Not IsPostBack Then
			DataFieldBind()
			ControlVisibility()
			SetPage()
			ControlVisibilityForAttachment()

		End If
	End Sub

	Private Sub SaveFlightLog(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If Not IsValid Then upnlValidationsummary.Update() : Exit Sub

			If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then

				SetObject()
				SetSession()
				mLogDetail = "Log : " + mLog.LogTextNo + " Dated : " + mLog.DateFormatted

				MarkLog(Action.Save,
						"Flight Log",
						User.Identity.Name & " is not Authorized User to Save " & mLogDetail,
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
								MSGBox.Message_text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			If IsValid Then

				If Not CustomValidate2() Then upnlValidationsummary.Update() : Exit Sub

				If Save() = True Then

					Session("mAircraftInformationBoardList") = Nothing
					DataFieldBind()
					ControlVisibility()
					ControlVisibilityForAttachment()
					SetPage()

					If mLog.LogTypeID = 3 Then txtLogDate.Visible = False

					upnlDetails.Update()
					upnlRemark.Update()
					upnlTitle.Update()
					upnlTabs.Update()

				Else
					upnlValidationsummary.Update()
				End If

			Else
				upnlValidationsummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub btnFuelOil_Click(sender As Object, e As EventArgs) Handles btnFuelOil.Click
		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		'-------------------------------
		If (Not User.IsInRole("LogFuelOilNew") And mLog.IsNew) Or (Not User.IsInRole("LogFuelOilEdit") And Not mLog.IsNew) Then
			'setObject()
			SetSession()
			MarkLog(Action.Save, "LogFuelOil", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

			Exit Sub
		End If
		DataFieldBind()

		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenLogFuelOilWindow", "OpenLogFuelOilWindow()", True)

	End Sub

	Private Sub btnDefectActionList_Click(sender As Object, e As EventArgs) Handles btnDefectActionList.Click

		Session("Edit") = False
		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenLogDefectActionWindow", "OpenLogDefectActionWindow()", True)

	End Sub

	Private Sub BtnMaintenanceActivity_Click(sender As Object, e As EventArgs) Handles btnMaintenanceAcitvity.Click
		SetObject()

		ScriptManager.RegisterStartupScript(Me, [GetType], "OpenLogMaintenanceActivityWindow", "OpenLogMaintenanceActivityWindow()", True)
	End Sub

	Protected Sub txtLogDate_TextChanged(sender As Object, e As EventArgs)
		If IsPostBack Then

			'# Date Control Validation #
			Try

				Dim tempdate As DateTime
				Dim Datestring As String = Format(CDate(txtLogDate.Text.Trim), AppSettings("DateFormat"))

				tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
				If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
					If ViewState("calDateTime") IsNot Nothing Then
						txtLogDate.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))
					Else
						txtLogDate.Text = Format(Today.Date, AppSettings("DateFormat"))
					End If
				Else
					txtLogDate.Text = Format(tempdate, AppSettings("DateFormat"))
				End If
				ViewState("calDateTime") = txtLogDate.Text.Trim  'Storing Current DateValue to ViewState for Date correction
			Catch ex As Exception
				If ViewState("calDateTime") IsNot Nothing Then
					txtLogDate.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
				Else
					txtLogDate.Text = Format(Today.Date, AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
				End If
				txtLogDate_TextChanged(txtLogDate.Text, e)  'Raising textchange event for further calculation
				Exit Sub
			End Try

			'# End
			If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), New SmartDate(txtLogDate.Text.ToString).Date) <> 0 Then
				REM: Clone the object
				Dim clnLog As Log
				mLog.LogPageNo = txtLogPageNo.Text
				mLog.Remark = txtRemark.Text
				clnLog = CType(mLog.Clone, Log)
				If mLog.IsNew Then
					NewRecord(txtLogDate.Text.ToString, , , clnLog.LogTypeID)
					mMaintLogListOnDate = LogList.GetLogList(mMachine.ID, txtLogDate.Text.ToString, txtLogDate.Text.ToString)
					Session("mMaintLogListOnDate") = mMaintLogListOnDate
					grdLogDet.DataSource = mMaintLogListOnDate
					grdLogDet.DataBind()
					upnlLogInfo.Update()
					If mMaintLogListOnDate.Count > 1 And mLog.IsNew Then
						Dim str1 As String
						str1 = "delete_cookie();"
						ScriptManager.RegisterStartupScript(Me, [GetType], "ShowLastDet", "ShowLastDet();", True)
						upnlLogInfo.Update()
					End If
				Else
					EditRecord(txtLogDate.Text.ToString)
				End If
				REM: Copy from Clone
				CopyFromClone(clnLog, True)
				DataFieldBind()

			End If


			SetPage()
			ControlVisibility()
			ControlVisibilityForAttachment()
			If mLog.LogTypeID = 3 Then txtLogDate.Visible = False
			upnlDetails.Update()
			upnlRemark.Update()
			upnlTitle.Update()
			upnlTabs.Update()


		End If
	End Sub

	Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
		' Session("IsValid") = IsValid

		Dim mopenas As String = Request.QueryString("Type")
		If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
			Session.Remove("mFileAttach")
			Session.Remove("mFileAttachLogMaint")
			Session.Remove("IsAttachmentDeleted")
			ScriptManager.RegisterStartupScript(Me, [GetType], "OnClose", "CallParentCallback();", True)
			Exit Sub
		End If
		'End
	End Sub

	Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
		ViewImage()
	End Sub

	Private Sub hdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
		mLog.IsAttachmentAdded = True
		mFileAttachLogMaint = CType(Session("mFileAttach"), FileAttach)
		Session("mFileAttachLogMaint") = mFileAttachLogMaint
		ControlVisibilityForAttachment()
		upnlFileupload.Update()
	End Sub

	Private Sub btnDelAttach_Click(sender As Object, e As EventArgs) Handles btnDelAttch.Click
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte
		GetAttachment()
		mFileAttachLogMaint.ImageFile = file1
		mFileAttachLogMaint.Size = 0
		ImageButton1.Visible = False
		btnDelAttch.Enabled = False
		IsAttachmentDeleted = True
		mLog.IsAttachmentAdded = False
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub

	Private Sub btnSelectFile_ServerClick(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick
		If mLog.IsAttachmentAdded Then
			mFileAttachLogMaint = FileAttach.GetAttachment(mLog.ID)
		Else
			mFileAttachLogMaint = FileAttach.NewAttachment(Guid.NewGuid, mLog.ID)
		End If
		Session("mFileAttachLogMaint") = mFileAttachLogMaint
		Session("mFileAttach") = mFileAttachLogMaint
	End Sub

#End Region

#Region " TAB's "

	Private Sub tabLogDetailsContainer_ActiveTabChanged(sender As Object, e As EventArgs) Handles tabLogDetailsContainer.ActiveTabChanged
		Select Case tabLogDetailsContainer.ActiveTabIndex
			Case 0
			Case 1 'Fuel Oil
				Session("OpenFromWO") = False
				Session("mOpenFromLogFuelNew") = False

				If (Not User.IsInRole("LogFuelOilNew") And mLog.IsNew) Or (Not User.IsInRole("LogFuelOilEdit") And Not mLog.IsNew) Then
					'setObject()
					SetSession()
					MarkLog(Action.Save, "LogFuelOil", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

					Exit Sub
				End If
				DataFieldBind()

				ScriptManager.RegisterStartupScript(Me, [GetType], "CallFuelOil", "CallFuelOil()", True)
			Case 2
				Session("Edit") = False
				'-------------------------------

				ScriptManager.RegisterStartupScript(Me, [GetType], "CallSnagReporting", "CallSnagReporting()", True)
			Case 3
				SetObject()
				ScriptManager.RegisterStartupScript(Me, [GetType], "CallMaintActivity", "CallMaintActivity()", True)

			Case 4
				'DiscrepancyReporting
				Session("mMachine") = mMachine
				mLog = Log.GetLog(mLog.ID)

				ScriptManager.RegisterStartupScript(Me, [GetType], "callDiscrepancyReporting", "callDiscrepancyReporting()", True)

			Case 5
				'Deffered Discrepancies
				Session("mMachine") = mMachine
				ScriptManager.RegisterStartupScript(Me, [GetType], "callDeferredDiscrepancies", "callDeferredDiscrepancies()", True)
		End Select
	End Sub

#End Region

#Region " APU Grid "
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUHours_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUHours As TextBox = TryCast(currentRow.FindControl("txtAPUHours"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Hours = Trim(txtAPUHours.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPULandings_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPULandings As TextBox = TryCast(currentRow.FindControl("txtAPULandings"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Landings = Trim(txtAPULandings.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Cycles = Trim(txtAPUCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUStarts_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUStarts As TextBox = TryCast(currentRow.FindControl("txtAPUStarts"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Starts = Trim(txtAPUStarts.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUNGCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUNGCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNGCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).NGCycles = Trim(txtAPUNGCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUNFCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUNFCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNFCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).NFCycles = Trim(txtAPUNFCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPURins_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPURins As TextBox = TryCast(currentRow.FindControl("txtAPURins"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).RINS = Trim(txtAPURins.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUBleeds_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUBleeds As TextBox = TryCast(currentRow.FindControl("txtAPUBleeds"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Bleeds = Trim(txtAPUBleeds.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUImpellerCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtAPUImpellerCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUCTCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUCTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCTCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).CTCycles = Trim(txtAPUCTCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUPTCycles_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUPTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUPTCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).PTCycles = Trim(txtAPUPTCycles.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUGeneratorMods_TextChanged(sender As Object, e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUPGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtAPUPGeneratorMods"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtAPUPGeneratorMods.Text)
		DataBindGrid()
	End Sub

	Private Sub grdLogDet_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdLogDet.RowCommand
		Dim Index As Int32
		Dim ID As Guid

		Select Case e.CommandName
			Case "Select"
				Index = CInt(e.CommandArgument)
				ID = mMaintLogListOnDate(Index).ID
				Session("mLog") = mLog
				If mLog.IsUTC Then
					If (AppSettings("ClientCode") = "Heligo" Or
						AppSettings("ClientCode") = "UHPL" Or
						AppSettings("ClientCode") = "APFT" Or
						AppSettings("ClientCode") = "AAP" Or
						mLog.IsLogAirborneEntry = True) Then    'Changed By Utkarsh on 02-Apr-2012 for TLP.
					Else
						If CDate(mLog.Date.ToString) = CDate(New SmartDate(mMaintLogListOnDate(Index).SouUniverseDateTimeFormatted.ToString).FormattedText) Then
							mLog.Date = New SmartDate(mMaintLogListOnDate(Index).SouUniverseDateTimeFormatted.ToString).ToString 'mPrevLogUniversalDateTime 'mPrevLogDate ''mPrevLogUniversalDateTime '
							mLog.SouUniverseDateTime = mMaintLogListOnDate(Index).SouUniverseDateTime.Date.AddMinutes(1)
							mLog.DesUniverseDateTime = mMaintLogListOnDate(Index).SouUniverseDateTime.Date.AddMinutes(1)
							mLog.TouchDownUniverseDateTime = mMaintLogListOnDate(Index).SouUniverseDateTime.Date.AddMinutes(1)
							mLog.TakeOffUniverseDateTime = mMaintLogListOnDate(Index).SouUniverseDateTime.Date.AddMinutes(1)
							If CDate(mLog.Date.ToString) <> CDate(mLog.SouUniverseDateTime.ToString) Then mLog.Date = New SmartDate(CDate(mLog.SouUniverseDateTimeFormatted))
						End If
					End If
				Else
					If (AppSettings("ClientCode") = "Heligo" Or
						AppSettings("ClientCode") = "UHPL" Or
						AppSettings("ClientCode") = "APFT" Or
						AppSettings("ClientCode") = "AAP" Or
						mLog.IsLogAirborneEntry = True) Then    'Changed By Utkarsh on 02-Apr-2012 for TLP.
					Else
						If CDate(mLog.Date.ToString) = CDate(New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).FormattedText) Then
							mLog.Date = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).ToString 'mPrevLogUniversalDateTime 'mPrevLogDate ''mPrevLogUniversalDateTime '
							mLog.SouLocalDateTime = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).Date.AddMinutes(1)
							mLog.DesLocalDateTime = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).Date.AddMinutes(1)
							mLog.TouchDownLocalDateTime = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).Date.AddMinutes(1)
							mLog.TakeOffLocalDateTime = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).Date.AddMinutes(1)
							If CDate(mLog.Date.ToString) <> CDate(New SmartDate(mLog.SouLocalDateTime.ToString).FormattedText) Then mLog.Date = New SmartDate(mMaintLogListOnDate(Index).SouLocalDateTimeFormatted.ToString).ToString
						End If
					End If
				End If

				Session("mLog") = mLog
		End Select
	End Sub

	Private Sub hdnBtnDiscrepancyTroubleShoot1_Click(sender As Object, e As EventArgs) Handles hdnBtnDiscrepancyTroubleShoot1.Click
		ScriptManager.RegisterStartupScript(Me, [GetType], "callDeferredDiscrepancies", "callDeferredDiscrepancies()", True)
	End Sub

	Private Sub hdnBtnDiscrepancyDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnDiscrepancyDetail.Click

		ScriptManager.RegisterStartupScript(Me, [GetType], "callDiscrepancyReporting", "callDiscrepancyReporting()", True)

	End Sub

#End Region

End Class