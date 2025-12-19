'*********************************
'Modified by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
'*********************************


Imports System.Data.OleDb


Partial Class wfLogList
	Inherits Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As Object

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region

#Region " Variable Declarations "

	Public mLog As Log
	Public mMachine As Machine
	Public mLogList As LogList 'Added Code
	Public mModuleList As ModuleList
	Public mFileAttach As FileAttach
	Public mLogTypeList As LogTypeList
	Public mCompanyDetail As New CompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
	Public mMachineNameValueList As MachineNameValueList
	Public mLogListForSelection As New LogListForSelection
	Dim dtTablesList As DataTable 'Added For Import Utility
	Dim Conn As OleDbConnection, Ada As OleDbDataAdapter, dsMain As New DataSet

	Public AircraftId As String
	Public StartDate As String
	Public EndDate As String
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mLogDetail As String
	Dim sSheetName As String 'End
	Dim mChkShowAll As Boolean = False
	Public mTypeIDForLogEdit As Integer 'Added by Shital on 04-Mar-2022
	Public mCurrentpage As Integer = 1
	Public mpageSize As Integer = 25
	Dim mpageindex As Integer = 0
	Dim pagecount As Integer = 0
	Dim totalCount As Integer = 0
	Public ToDate As String

#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mMachine = CType(Session("mMachine"), Machine)
		'mMachineNameValueList = CType(Session("mMachineNameValueList"), tmpMachineList)
		mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
		mLogList = CType(Session("mLogList"), LogList)
		AircraftId = CType(Session("AircraftId"), String)
		StartDate = CType(Session("StartDate"), String)
		EndDate = CType(Session("EndDate"), String)
		mLogTypeList = Session("mLogTypeList")
		mChkShowAll = Session("ChkShowAll") 'Added on 20-Oct-2021
		mTypeIDForLogEdit = Session("mTypeIDForLogEdit") 'Added by Shital on 04-Mar-2022
		mCompanyDetail = Session("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
		mModuleList = Session("mModuleList")

	End Sub

	Private Sub SetSession()
		Session("mMachine") = mMachine
		Session("mMachineNameValueList") = mMachineNameValueList
		Session("mLogList") = mLogList
		Session("StartDate") = StartDate
		Session("EndDate") = EndDate
		Session("mLogTypeList") = mLogTypeList
		Session("mTypeIDForLogEdit") = mTypeIDForLogEdit 'Added by Shital on 04-Mar-2022
		Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
	End Sub

	Private Sub RemoveSession()

		mMachineNameValueList = Nothing
		mLogList = Nothing
		Session.Remove("mMachineNameValueList")
		Session.Remove("mLogList")
		Session.Remove("mLogTypeList")
		Session.Remove("ChkShowAll")
		Session.Remove("mTypeIDForLogEdit")  'Added by Shital on 04-Mar-2022
		Session.Remove("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022

	End Sub

	Private Sub ClearAll()
		If Session("MiddleFrame") <> "wfLogList.aspx" AndAlso Session("MiddleFrame") <> "wfLogList.aspx?Type=2" Then
			Session.Remove("mMachineNameValueList")
			Session.Remove("mLogList")
			Session.Remove("AircraftId")
			Session.Remove("StartDate")
			Session.Remove("EndDate")
			Session.Remove("mLogTypeList")
			Session.Remove("mMELSnagCorrectiveAction")
		End If
		If Session("MiddleFrame") <> "wfLogList.aspx?Type=2" Then
			Session.Remove("mTypeIDForLogEdit")  'Added by Shital on 04-Mar-2022
		End If
	End Sub

	Private Sub SetPBHValues(ID As Guid, MachineID As Guid, Optional HourDiff_Dec As Decimal = 0)

		Try

			If mCompanyDetail.IsCombinedHours = False Then

				Dim TmpLog As Log
				Dim mPBH As PBH

				If ID.Equals(Guid.Empty) Then

					mPBH = PBH.GetPBHByMachine(MachineID, "")

					If Not mPBH.MachineID.Equals(Guid.Empty) Then

						If CDate(Today.Date) >= CDate(mPBH.StartDate) Then
							mPBH.CurrentHours = mPBH.StartHoursDec
							mPBH.ElapsedHours = 0
							mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value

							'For Not Active Case: If RemainingHours <= 0 then mark Not Active flag
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

							'For Not Active Case: If RemainingHours <= 0 then mark Not Active flag
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

			ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022

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

						'For Not Active Case: If RemainingHours <= 0 then mark Not Active flag
						'Also mark Not InUse in tabMachine at same time 
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
	'End

	Private Sub EditRecord(Id As Guid)

		Try

			Dim mLog As Log
			Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
			Dim mMachine As Machine = Machine.GetMachine(mMachineID)

			Session("mLogList") = Nothing
			Session("LogListCount") = mLogList.Count
			Session("mMachine") = mMachine

			mLog = Log.GetLog(Id)
			mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
			mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
			mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry

			Session("mLog") = mLog
			mLogDetail = mLog.LogTextNo.ToString + " Dated : " + mLog.DateFormatted

			MarkLog(Action.Edit,
					"Flight Log",
					mLogDetail,
					ErrorType.HandledError,
					mLog.ID,
					EventLogID)

			Dim str As String
			Session("mIsLastLog") = IIf((MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID, True).LogID).Equals(mLog.ID), True, False)

			If mLog.LogTypeID = 1 Then

				Session("ChkShowAll") = chkShowAll.Checked 'added on 20-Oct-2021

				If mLog.IsTLP = True Then

					If Session("mTypeIDForLogEdit") = 2 Then
						str = "openLedgerInSameWindow('wfTLPEdit_Ajax.aspx?BackPage=Index.aspx');"
					Else
						str = "openLedgerInSameWindow('wfTLP_Ajax.aspx?BackPage=Index.aspx');"
					End If

				Else

					If Session("mTypeIDForLogEdit") = 2 Then
						str = "openLedgerInSameWindow('wfLogSOPEdit_Ajax.aspx?BackPage=Index.aspx');"
					Else
						str = "openLedgerInSameWindow('wfLogSOP_Ajax.aspx?BackPage=Index.aspx');"
					End If

				End If

				Session("mLog") = mLog

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"OpenScript",
													str,
													True)
			Else
				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Open Log-Detail Window",
													"OpenLogDetailWindow()",
													True)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GetAttachment(ID As Guid, mIsAttachemntAdded As Boolean)
		If mIsAttachemntAdded = True Then
			mFileAttach = FileAttach.GetAttachmentChild(ID)
			Session("mFileAttach") = mFileAttach
		End If
	End Sub

	Private Sub DeleteRecord(Index As Int32)

		Dim mMachineID As New Guid(cmbAircraft.SelectedValue)
		Try

			mMachine = Machine.GetMachine(mMachineID)
			Session("mMachine") = mMachine

			MSGBoxCtrl.show(MSGBox.Message_title.Delete,
							MSGBox.Message_text.Delete,
							"",
							MsgBoxStyle.YesNo,
							"Delete")

			mLogList.CurrentIndex = Index
			Session("mLogList") = mLogList

		Catch ex As Exception
			Throw ex.GetBaseException
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

							Dim TempLogID As Guid

							Try

								mLogList = CType(Session("mLogList"), LogList)

								If mLogList.CurrentItem.IsAttachmentAdded = True Then
									mFileAttach = FileAttach.GetAttachment(mLogList.CurrentItem.ID)
								End If

								Dim Hours_DecTemp As Decimal = New Period(1, mLogList.CurrentItem.TimeInAir.ToString, 1, False, False).DbValueDec 'PBH Collective Hrs by Saylee on 30-Nov-2022

								TempLogID = mLogList.CurrentItem.ID
								mLogDetail = mLogList.CurrentItem.LogTextNo.ToString + " Dated : " + mLogList.CurrentItem.DateFormatted.ToString

								Log.DeleteLog(mLogList.CurrentItem.ID,
											  mMachine.ID,
											  mLogList.CurrentItem.SouLocalDateTimeForDelete.ToString,
											  mLogList.CurrentItem.DesLocalDateTimeForDelete.ToString)

								If mFileAttach IsNot Nothing Then

									If mFileAttach.Size > 0 Then
										FileAttach.DeleteAttachment(mFileAttach.ID, mFileAttach.ReferenceID)
									End If

								End If

								'Added By Vikrant on 01-Dec-2021 for PBH
								If mLogList.Count > 1 Then
									SetPBHValues(mLogList(1).ID, Guid.Empty, Hours_DecTemp)
								Else
									SetPBHValues(Guid.Empty, mMachine.ID)
								End If
								'End

								MarkLog(Action.Delete, "Flight Log", mLogDetail, ErrorType.NoError, mLogList.Item(mLogList.CurrentIndex).ID, EventLogID)
								'Added by Saylee on 27-July-2009
								Session("mAircraftInformationBoardList") = Nothing

								DataFieldBindForPageLoad()
								SetPage()

							Catch ex As SqlException

								Dim stringInfo As String = "Other transaction(s)."

								If ex.Message.Contains("tabnWO") Then
									stringInfo = "Work Order."
								ElseIf ex.Message.Contains("tabFlightDelayAndCancellation") Then
									stringInfo = "Flight Delay / Cancellation."
								ElseIf ex.Message.Contains("tabDentBuckle") Then
									stringInfo = "Dent & Buckle Chart."
								ElseIf ex.Message.Contains("tabMELSnagCorrectiveAction") Then
									stringInfo = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect.", "MEL / Snag.") 'Added By Vikrant On 07-Sep-2020 For ALL07092020
								Else
									stringInfo = ex.Message
								End If

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

									MarkLog(Action.Delete, "Flight Log",
											"Can't delete : " & mLogDetail & " is Currently in use",
											ErrorType.NoError,
											TempLogID,
											EventLogID)

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDeleting,
													MSGBox.Message_Text.ReferenceDeleting,
													stringInfo,
													MsgBoxStyle.OkOnly,
													"")

								ElseIf ex.Number = 50000 Then

									If CBool(AppSettings("ShowNewDiscrepancyFlow")) Then
										MSGBoxCtrl.Show("Deletion Alert !", stringInfo, "", MsgBoxStyle.OkOnly, "")
									Else
										MSGBoxCtrl.Show("Deletion Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
									End If

								End If

								DataFieldBindForPageLoad()
								msgCount = ex.Errors.Count

							Finally

								If msgCount = 0 Then

									MarkLog(Action.Delete,
											"Flight Log",
											"Deleted SuccessFully : " & mLogDetail,
											ErrorType.NoError,
											TempLogID,
											EventLogID)

									MSGBoxCtrl.Show(MSGBox.Message_Title.DeletedSuccessFully,
													MSGBox.Message_Text.DeletedSuccessFully,
													"",
													MsgBoxStyle.OkOnly,
													"")

								End If

							End Try

						End If

					Case MsgBoxResult.No
						DataFieldBind()
					Case MsgBoxResult.Ok
						DataFieldBindForPageLoad() 'changed by Utkarsh on 24-sep-2013 for log_ajax changes
					Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
						DataFieldBind()

				End Select

			ElseIf MsgBoxResult = -1 Then
			ElseIf MsgBoxResult = 0 Then   'Code Added
				Session("sender") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub SearchRecords(Optional Show_100_Records As Boolean = False)

		Session("AircraftId") = cmbAircraft.SelectedValue
		Session("StartDate") = txtStartDate.Text
		Session("EndDate") = txtEndDate.Text
		Session("chkShowAll") = chkShowAll.Checked        'Added by Shital on 20-Oct-2021

		Dim mMachineID As New Guid(cmbAircraft.SelectedValue)

		mMachine = Machine.GetMachine(mMachineID) 'Added by Saylee On 12-Feb-2014 For ALL12022014-1
		Session("mMachine") = mMachine

		mLogList = Nothing
		Session.Remove("mLogList")

		mLogList = LogList.GetLogList(mMachineID,
									  txtStartDate.Text,
									  txtEndDate.Text,
									  Show_100_Records,
									  txtLogPageNo.Text.Trim)

		Session("mLogList") = mLogList

		'Added By Saylee on 01-Dec-2021 for PBH

		Dim mPBHMachine As PBH

		If mCompanyDetail.IsCombinedHours Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
			Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
			mPBHMachine = PBH.GetPBH(mPBHList(0).ID)
		Else
			mPBHMachine = PBH.GetPBHByMachine(mMachineID, "")
		End If

		lblPBH.Text = ""
		upnlpbh.Update()

		If mPBHMachine IsNot Nothing And (mPBHMachine.HoursFrequency <> "") Then

			lblPBH.Text = "Subscribed Hours : " + mPBHMachine.HoursFrequency +
						  " Elapsed Hours : " + mPBHMachine.ElapsedHoursText +
						  " Remaining Hours : " + mPBHMachine.RemainingHoursText

			If mPBHMachine.RemainingHoursDec <= 1800 Then  'if Remaining hrs < than 30 hrs then mark red
				lblPBH.ForeColor = Color.Red
			Else
				lblPBH.ForeColor = Color.Maroon
			End If

			upnlpbh.Update()

		End If
		'End

		ControlVisibility()

		If mMachine.IsUTC Then
			gdvLogList.Columns(1).HeaderText = "Date (UTC)"
		End If

		DataGridBind()
		SetGrid()

	End Sub

	Private Sub DataGridBind()

		Try

			gdvLogList.DataSource = mLogList
			gdvLogList.DataBind()

			upnlGrid.Update()
			upnlLogGrid.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPage()
		If mLogList Is Nothing Then
			lblResult.Text = "As per criteria : 0 Record(s) found."
		Else
			lblResult.Text = "As per criteria : " & mLogList.Count & " Record(s) found."
		End If

		upnlGrid.Update()
	End Sub

	Private Sub ControlVisibility()

		If ((mLogList IsNot Nothing) AndAlso mLogList.Count <= 0) Or mLogList Is Nothing Then
			btnPrint.Enabled = False
		Else
			btnPrint.Enabled = True
		End If

		If AppSettings("IsValZero") = "True" Then
			gdvLogList.Columns(18).Visible = True
		Else
			gdvLogList.Columns(18).Visible = False
		End If

		If mMachine IsNot Nothing Then

			If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1

				gdvLogList.Columns(1).HeaderText = "Date (UTC)"
				GV_LogSelection.Columns(1).HeaderText = "Date (UTC)"

				gdvLogList.Columns(6).Visible = True
				gdvLogList.Columns(9).Visible = True
				gdvLogList.Columns(5).Visible = False
				gdvLogList.Columns(8).Visible = False

				'Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
				GV_LogSelection.Columns(5).Visible = False
				GV_LogSelection.Columns(8).Visible = False
				GV_LogSelection.Columns(6).Visible = True
				GV_LogSelection.Columns(9).Visible = True

			Else

				gdvLogList.Columns(6).Visible = False
				gdvLogList.Columns(9).Visible = False
				gdvLogList.Columns(5).Visible = True
				gdvLogList.Columns(8).Visible = True

				'Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
				GV_LogSelection.Columns(5).Visible = True
				GV_LogSelection.Columns(8).Visible = True
				GV_LogSelection.Columns(6).Visible = False
				GV_LogSelection.Columns(9).Visible = False

			End If

			'Added By Utkarsh ON 12-Apr-2012
			If mMachine.IsTLP = True Then   ' If mLog.IsTLP = True Then   -----------Changed by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP

				gdvLogList.Columns(3).HeaderText = "TLP No."
				gdvLogList.HeaderRow.Cells(3).Text = "TLP No."

				GV_LogSelection.Columns(3).HeaderText = "TLP No."

				gdvLogList.Columns(5).Visible = False
				gdvLogList.Columns(6).Visible = False
				gdvLogList.Columns(8).Visible = False
				gdvLogList.Columns(9).Visible = False

				GV_LogSelection.Columns(5).Visible = False
				GV_LogSelection.Columns(6).Visible = False
				GV_LogSelection.Columns(8).Visible = False
				GV_LogSelection.Columns(9).Visible = False

				lblLogPageNo.Visible = True
				txtLogPageNo.Visible = True

			ElseIf AppSettings("ClientCode") = "STR" Then 'Added by Shital on 19-Apr-2021 suggested by Abhijit
				lblLogPageNo.Visible = True
				txtLogPageNo.Visible = True
			Else
				lblLogPageNo.Visible = False
				txtLogPageNo.Visible = False
			End If

		End If
		'End

		If AppSettings("IsMAINTLogVOIDLogRequired") = "False" Then     'Or mLogList.Count = 0
			cmbLogType.Visible = False
			lblLogType.Visible = False
		Else
			cmbLogType.Visible = True
			lblLogType.Visible = True
		End If

		If Session("mTypeIDForLogEdit") = 2 Then
			gdvLogList.Columns(15).Visible = False
			btnAddNew.Visible = False
		Else

			gdvLogList.Columns(15).Visible = True

			If mCompanyDetail Is Nothing Then
				mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
				Session("mCompanyDetail") = mCompanyDetail
			End If

			If mCompanyDetail.IsSyncApplication And cmbLogType.SelectedIndex = 0 Then
				btnAddNew.Visible = False
				spnImportFromCRS.Visible = True
			Else
				btnAddNew.Visible = True
				spnImportFromCRS.Visible = False
			End If

		End If

		'Ajay 07-Nov-2022
		If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Log") Then
			ScriptManager.RegisterStartupScript(Me, [GetType], "MarkFav", "MarkFav();", True)
		Else
			ScriptManager.RegisterStartupScript(Me, [GetType], "RemoveFav", "RemoveFav();", True)
		End If
		'--------------------------
		upnlGrid.Update()

	End Sub

	Private Function CHECK_IsRequiredAssembliesInstalled(mLog As Log) As Boolean

		If mLog.LogAFAssemblies.AssemblyRemoved Or
		   mLog.LogEngAssemblies.AssemblyRemoved Or
		   mLog.PropLogAssemblies.AssemblyRemoved Or
		   mLog.LogAPUAssemblies.AssemblyRemoved Or
		   mLog.LogCGBAssemblies.AssemblyRemoved Or
		   mLog.LogNGBAssemblies.AssemblyRemoved Or
		   mLog.LogGEAssemblies.AssemblyRemoved Then

			MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction,
							MSGBox.Message_text.EntryRestriction,
							"You are trying to create new log. Selected machine does not have required assemblies installed. ",
							MsgBoxStyle.OkOnly,
							"")
			Return False

		End If

		Dim mLogAssemblyInstalledList As LogAssemblyInstalledList = LogAssemblyInstalledList.GetLogAssemblyInstalledList(MachineID:=New Guid(cmbAircraft.SelectedValue),
																														 CurrentDate:=Now.ToShortDateString)

		Dim IsAirFrameAvailable As Boolean = False
		Dim IsEngineAvailable As Boolean = False
		Dim AssembliesNotFound As String = ""

		Dim obj As LogAssemblyInstalledList.LogAssemblyInstalledListInfo

		For Each obj In mLogAssemblyInstalledList
			If obj.AssemblyTypeID = 1 Then IsAirFrameAvailable = True
			If obj.AssemblyTypeID = 2 Then IsEngineAvailable = True
		Next

		If (Not (IsAirFrameAvailable And IsEngineAvailable)) Then

			If IsEngineAvailable = False Then AssembliesNotFound = "Engine"
			If IsAirFrameAvailable = False Then AssembliesNotFound = AssembliesNotFound + IIf(AssembliesNotFound = "", "Machine", ",Machine").ToString

			MSGBoxCtrl.show(MSGBox.Message_title.Restriction,
							MSGBox.Message_text.Restriction,
							" ",
							MsgBoxStyle.OkOnly,
							"")
			Return False

		End If

		Return True

	End Function

	Private Sub SetGrid()

		Dim IsSyncFromCRS As Boolean

		For j As Integer = 0 To gdvLogList.Rows.Count - 1

			Dim P As New Integer
			Dim mStr As String  'Label
			IsSyncFromCRS = CType(Me.gdvLogList.Rows(j).Cells(19).Text, Boolean)

			If mLogList(j).LogTypeID = 1 Then

				mStr = Me.gdvLogList.Rows(j).Cells(17).Text
				If mStr = "True" Then
					Me.gdvLogList.Rows(j).Cells(16).BackColor = ColorTranslator.FromHtml("#B0F")
				End If

			Else
				Me.gdvLogList.Rows(j).Cells(16).BackColor = ColorTranslator.FromHtml("#0000FF") 'Added by Saylee on 3-Dec-2014 for ALL03122014-1 : to show Blue for Zero valued Log
			End If

			'Added by Shital on 31-Maar-2022
			Dim IsLogEdited As Boolean
			If mTypeIDForLogEdit = 2 Then

				gdvLogList.Rows.Item(j).FindControl("DeleteRecord").Visible = False
				IsLogEdited = CType(gdvLogList.Rows.Item(j).Cells(18).Text, Boolean)

				If IsLogEdited = True Then

					Dim EditRec As ImageButton = gdvLogList.Rows.Item(j).FindControl("EditView")
					EditRec.Enabled = False
					EditRec.ToolTip = "Record Already Edited once!"

				End If

			End If

		Next

	End Sub

	'Added For Import Utility
	Private Sub ImportLogs()

		Try

			Conn = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; data source=" & AppSettings("DOCPath") & "Import Logs.xls" & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1""")
			Conn.Open()

			dtTablesList = Conn.GetSchema("Tables")

			If dtTablesList.Rows.Count > 0 Then

				For i As Integer = 0 To dtTablesList.Rows.Count - 1
					sSheetName = dtTablesList.Rows(i)("TABLE_NAME").ToString

					If sSheetName <> "" Then

						Ada = New OleDbDataAdapter("SELECT * FROM [" & sSheetName & "]", Conn)

						Try
							Ada.Fill(dsMain, "Test")
						Catch ex As Exception
							Throw ex
						Finally

							Session("dsMain") = dsMain
							Session("mLogList") = mLogList
							Session("MachineIDToSet") = cmbAircraft.SelectedValue.ToString
							Conn.Close()

						End Try

					End If

					Exit For

				Next

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'End

	Private Sub ImportCRSLogs()

		Dim mLogPageNo As String = ""
		Dim mCRSLogs As CRSLogTransfer
		Dim mLog As Log
		Dim mError As Boolean = False
		mCRSLogs = CRSLogTransfer.GetLogList()

		If mCRSLogs.Count > 0 Then

			Try

				For i As Integer = 0 To mCRSLogs.Count - 1

					mMachine = Machine.GetMachine(mCRSLogs(i).MachineID, False)

					If mMachine.IsReadOnly Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1,
								  mMachine.RegNo + " is ReadOnly aircraft, so log " +
									mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)

						FileClose(1)
						mError = True
						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")

						MarkLog(Action.Save,
								"Flight Log",
								"Log(s) failed for Importing " +
									mCRSLogs(i).LogPageNo + " : " + mMachine.RegNo +
									" is ReadOnly aircraft, so cannot be transferred into system. ",
								ErrorType.UnhandledError,
								Guid.Empty,
								EventLogID)

						GoTo 2

					ElseIf mMachine.NotInUse Then

						FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
						WriteLine(1, mMachine.RegNo + " is Not In Use since " + mMachine.NotInUseDateFormatted + ", so log " + mCRSLogs(i).LogPageNo + " cannot be transferred into system. " + vbCrLf)
						FileClose(1)
						mError = True

						SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")

						MarkLog(Action.Save,
								"Flight Log",
								"Log(s) failed for Importing " +
									mCRSLogs(i).LogPageNo + " : " +
									mMachine.RegNo + " is Not In Use since " +
									mMachine.NotInUseDateFormatted + ", so Log " + mCRSLogs(i).LogPageNo +
									" cannot be transferred into system.",
								ErrorType.UnhandledError,
								Guid.Empty,
								EventLogID)

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
					'mLog.FlightLogClassificationID = mCRSLogs(i).FlightLogClassificationID

					Dim mFlightLogClassificationList As FlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList(Name:=mCRSLogs(i).FlightTypeRevenueStatus)

					If mFlightLogClassificationList.Count > 0 Then
						mLog.FlightLogClassificationID = mFlightLogClassificationList(0).ID
					Else
						mLog.FlightLogClassificationID = New Guid("{00000000-0000-0000-0000-000000000000}")
					End If

					mLog.SourceID = mCRSLogs(i).FromPlaceID
					mLog.DestinationID = mCRSLogs(i).ToPlaceID
					mLog.SouUniverseDateTime = mCRSLogs(i).UTCChocksOffDateTimeFormatted
					mLog.TakeOffUniverseDateTime = mCRSLogs(i).UTCTakeOffDateTimeFormatted
					mLog.TouchDownUniverseDateTime = mCRSLogs(i).UTCTouchDownDateTimeFormatted
					mLog.DesUniverseDateTime = mCRSLogs(i).UTCChocksOnDateTimeFormatted
					mLog.Remark = mCRSLogs(i).Remark
					mLog.EngineDerateID = 1

					For j As Integer = 0 To mLog.LogAFAssemblies.Count - 1

						If mLog.LogAFAssemblies(j).LogPeriods.Contains(3) Then mLog.LogAFAssemblies(j).Cycles = mCRSLogs(i).Cycles.ToString
						If mLog.LogAFAssemblies(j).LogPeriods.Contains(7) Then mLog.LogAFAssemblies(j).Landings = mCRSLogs(i).Landings.ToString

					Next

					For j As Integer = 0 To mLog.LogAPUAssemblies.Count - 1

						If mLog.LogAPUAssemblies(j).LogPeriods.Contains(3) Then
							mLog.LogAPUAssemblies(j).Cycles = 0 'mCRSLogs(i).Cycles.ToString
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

					If mMachine.IsTLP Then
						SETLogDetail(mCRSLogs(i))
					End If

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

									Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction
									Dim mAssemblyList As AssemblyList = AssemblyList.GetAssemblyListForComboBox(0, mMachine.ID.ToString, mLog.DateFormatted.ToString, "", True)

									mMELSnagCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction(mAssemblyList(0).AssemblyStatusID.ToString)
									mMELSnagCorrectiveAction.Defect = mCRSLogTransferDiscrepancies(m).Discrepancy
									mMELSnagCorrectiveAction.Sector = mLog.SourceName

									If Not mCRSLogTransferDiscrepancies(m).ReportCrewID.Equals(Guid.Empty) Then
										Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(mCRSLogTransferDiscrepancies(m).EmployeeName, User.Identity.Name, WithoutLicenseNoAlso:=1)
										mMELSnagCorrectiveAction.ReportedBy = mLicenses(0).LicenseNoEmpName
									End If

									mMELSnagCorrectiveAction.DefectReportNo = "Dscr" + "/" + mMachine.RegNo
									mMELSnagCorrectiveAction.LogID = mLog.ID
									mMELSnagCorrectiveAction.DateOfOccurrence = mLog.DateFormatted
									mMELSnagCorrectiveAction.RegNo = mLog.RegNo

									If mLog.LogAFAssemblies(0).FinalLandings = "" Or mLog.LogAFAssemblies(0).FinalLandings = "0" Then
										mMELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H"
									Else
										mMELSnagCorrectiveAction.LastMajorCheckHour = mLog.LogAFAssemblies(0).FinalHours + " H" + ", " + mLog.LogAFAssemblies(0).FinalLandings + " L"
									End If

									If mLog.LogAFAssemblies(0).FinalCycles = "" Or mLog.LogAFAssemblies(0).FinalCycles = "0" Then
										mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveAction.LastMajorCheckHour
									Else
										mMELSnagCorrectiveAction.LastMajorCheckHour = mMELSnagCorrectiveAction.LastMajorCheckHour + ", " + mLog.LogAFAssemblies(0).FinalCycles + " C"
									End If


									mMELSnagCorrectiveAction.IsSyncFromCRS = True

									Dim mATAlist As ATAList = ATAList.GetATAList()
									Try
										If Not (mATAlist.Item(101, "").ID).Equals(Guid.Empty) Then
											mMELSnagCorrectiveAction.ATAChapterID = mATAlist.Item(101, "").ID
										End If
									Catch ex As Exception

									End Try



									If mMELSnagCorrectiveAction.IsValid Then

										mMELSnagCorrectiveAction.Save()
										mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(mMELSnagCorrectiveAction.ID)
										Try
											SendMail("",
											  IsForNewDiscrepancyImported:=True,
											  ImportedDiscrepancy:=mMELSnagCorrectiveAction)
										Catch ex As Exception
											FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs", OpenMode.Append, OpenAccess.ReadWrite)
											WriteLine(1, "Sending of as Mail failed for defect " + mMELSnagCorrectiveAction.Defect + vbCrLf)
											FileClose(1)
											mError = True
										End Try


									End If

								Next

							End If

						End If
						'********************

						mLogPageNo = mCRSLogs(i).LogPageNo

						If mLogList.Count > 1 Then
							SetPBHValues(mLogList(1).ID, Guid.Empty, mLog.LogAFAssemblies(0).HoursDec)
						Else
							SetPBHValues(Guid.Empty, mMachine.ID)
						End If

					Else

						Dim str As String = ""
						str = CustomValidate2()
						str = str.Replace("<BR>", vbCrLf)
						mError = True

						If str <> "" Then

							FileOpen(1, AppSettings("DOCPath") & "ImportedFailedLogs.txt", OpenMode.Append, OpenAccess.ReadWrite)
							WriteLine(1, str + vbCrLf)
							FileClose(1)

							SendMail(AppSettings("DOCPath") & "ImportedFailedLogs")

							MarkLog(Action.Save,
									"Flight Log",
									"Log(s) failed for Importing " + mCRSLogs(i).AircraftRegNo +
										" (" + mCRSLogs(i).LogPageNo + ") :" + str,
									ErrorType.UnhandledError,
									Guid.Empty,
									EventLogID)

							GoTo 2

						End If

					End If

2:              Next

			Catch ex As Exception
				Throw ex.GetBaseException
			End Try

			If mError = True Then

				MSGBoxCtrl.Show("Success",
								"Log(s) Imported Successfully with some Error(s).",
								"Check file for error(s) " + AppSettings("DOCPath") & "ImportedFailedLogs",
								MsgBoxStyle.OkOnly,
								"Success")

			Else
				MSGBoxCtrl.Show("Success",
								"Log(s) Imported Successfully",
								"",
								MsgBoxStyle.OkOnly,
								"Success")

			End If

		End If

	End Sub

	Public Sub SetUserMailIDs()
		Session("UserEmailID") = mModuleList.Item("DiscrepancyAction").SendToMailID
		Session("UserCcEmailID") = mModuleList.Item("DiscrepancyAction").SendCCMailID
		Session("MailsRequire") = mModuleList.Item("DiscrepancyAction").MailsRequire
		Session("SmtpHost") = mModuleList.Item("DiscrepancyAction").SmtpHost
		Session("SmtpPort") = mModuleList.Item("DiscrepancyAction").SmtpPort
		Session("SmtpUser") = mModuleList.Item("DiscrepancyAction").SmtpUser
		Session("SmtpPassword") = mModuleList.Item("DiscrepancyAction").SmtpPassword
	End Sub

	Public Sub SendMail(FilePath As String,
						Optional IsForNewDiscrepancyImported As Boolean = False,
						Optional ImportedDiscrepancy As MELSnagCorrectiveAction = Nothing)

		Dim str As String

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

				str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">New Discrepancy has been added in FlyPal System and need your attention." + "</font></P></br> ")
				str = str + "<p><font face=""Calibri"">"
				str = str + "<b> Aircraft : </b>" + ImportedDiscrepancy.RegNo + "<b>" + "  Log No : " + "</b>" + ImportedDiscrepancy.LogNo
				str = str + "</font></p>"
				str = str + "<p><font face=""Calibri"">"
				str = str + ("<b>Discrepancy No. : " + "</b>" + ImportedDiscrepancy.DefectNo + "<b>  Date of Occurrence : </b>" +
						 ImportedDiscrepancy.DateOfOccurrenceFormatted)
				str = str + "</font></p>"
				str = str + "<p><font face=""Calibri"">"
				str = str + "<b>" + " Discrepancy : " + "</b>" + ImportedDiscrepancy.Defect
				str = str + "</font></p>"
				str = str + "<p><font face=""Calibri"">"
				str = str + "<b>" + " Reported By : " + "</b>" + ImportedDiscrepancy.ReportedBy
				str = str + "</font></p>"
				str = str + "</body></html>"
				str = str + ("</br><p><font face=""Calibri"">")
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
											MessageBox.Show("Mail Sent Successfully", False),
											True)

	End Sub

	Public Function CustomValidate2() As String    'For DgLog Fuel Oils
		Dim str As String = ""
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
	End Function

	Private Sub SETLogDetail(mCRSLogsInfo As CRSLogTransfer.CRSLogTransferInfo)
		mLog = Session("mLog")
		Dim mLogDetail As LogDetail
		mLogDetail = LogDetail.NewChildLogDetail(mLog.ID, mLog.Date.ToString)

		With mLogDetail

			If mLog.IsUTC = True Then
				If Not IsDate(mCRSLogsInfo.UTCChocksOffDateTimeFormatted) Then
					.SouUniverseDateTime = System.DBNull.Value
				Else
					'.SouUniverseDateTime = CalUTCDateTime.Text.ToString.Trim
					.SouUniverseDateTime = mCRSLogsInfo.UTCChocksOffDateTimeFormatted
				End If
				If Not IsDate(mCRSLogsInfo.UTCTakeOffDateTimeFormatted) Then
					.TakeOffUniverseDateTime = System.DBNull.Value
				Else
					'.TakeOffUniverseDateTime = calUTCTakeOffDateTime.Text.ToString.Trim
					.TakeOffUniverseDateTime = mCRSLogsInfo.UTCTakeOffDateTimeFormatted
				End If

				If Not IsDate(mCRSLogsInfo.UTCTouchDownDateTimeFormatted) Then
					.TouchDownUniverseDateTime = System.DBNull.Value
				Else
					'.TouchDownUniverseDateTime = calUTCTouchDownDateTime.Text.ToString.Trim
					.TouchDownUniverseDateTime = mCRSLogsInfo.UTCTouchDownDateTimeFormatted
				End If
				If Not IsDate(mCRSLogsInfo.UTCChocksOnDateTimeFormatted) Then
					.DesUniverseDateTime = System.DBNull.Value
				Else
					'.DesUniverseDateTime = CalUTCArrival.Text.ToString.Trim
					.DesUniverseDateTime = mCRSLogsInfo.UTCChocksOnDateTimeFormatted

				End If
			End If

			mLogDetail.SourceID = mLog.SourceID
			mLogDetail.DestinationID = mLog.DestinationID

			.FlightNo = mCRSLogsInfo.FlightNo.ToString.Trim
			.Landings = Val(mCRSLogsInfo.Landings.ToString.Trim)
		End With
		Session("mLogDetail") = mLogDetail

		mLog.LogDetails.Add(mLogDetail)
		Session("mLog") = mLog
	End Sub

#End Region

#Region " Data Bindings "

	Private Sub LogListBind(Optional Show_100_Records As Boolean = False)

		mMachineNameValueList = MachineNameValueList.GetMachineList("", , , , , , , True, "<SELECT>", , SkipIsForInventoryAircarft:=True)
		Session("mMachineNameValueList") = mMachineNameValueList

		cmbAircraft.DataSource = mMachineNameValueList

		If mMachineNameValueList.Count <> 0 And mMachineNameValueList.Count > 1 Then
			If IsNothing(AircraftId) Then AircraftId = mMachineNameValueList(1).ID.ToString Else AircraftId = AircraftId
		Else
			AircraftId = "00000000-0000-0000-0000-000000000000"
		End If

		Session("AircraftId") = AircraftId

		If Session("ChkShowAll") = True Then

			mLogList = LogList.GetLogList(New Guid(AircraftId),
											  txtStartDate.Text,
											  txtEndDate.Text,
											  False,
											  txtLogPageNo.Text.Trim)

		Else

			mLogList = LogList.GetLogList(New Guid(AircraftId),
											  txtStartDate.Text,
											  txtEndDate.Text,
											  Show_100_Records,
											  txtLogPageNo.Text.Trim)

		End If

		Session("mLogList") = mLogList

		If mMachineNameValueList.Count > 1 And IsNothing(AircraftId) Then cmbAircraft.SelectedIndex = 1 Else cmbAircraft.SelectedValue = AircraftId

		If AircraftId <> "00000000-0000-0000-0000-000000000000" Then
			mMachine = Machine.GetMachine(New Guid(AircraftId))  'Added by Saylee On 12-Feb-2014 For ALL12022014-1
			Session("mMachine") = mMachine
		End If

		If mMachine.IsUTC Then
			gdvLogList.Columns(1).HeaderText = "Date (UTC)"
		End If

		DataGridBind()
		SetGrid()
		ControlVisibility()

		upnlSearchCriteria.DataBind() 'Added By Vikrant On 10-Dec-2013 For ALL09122013-2
		AircraftId = cmbAircraft.SelectedValue
		Session("AircraftId") = AircraftId

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
		Session("mCompanyDetail") = mCompanyDetail

		'Added By Saylee on 01-Dec-2021 for PBH
		Dim mPBHMachine As PBH

		If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
			mPBHMachine = PBH.GetPBHByMachine(New Guid(AircraftId), "")
		Else
			Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
			mPBHMachine = PBH.GetPBH(mPBHList(0).ID)
		End If

		lblPBH.Text = ""
		upnlpbh.Update()

		If mPBHMachine IsNot Nothing And (mPBHMachine.HoursFrequency <> "") Then

			lblPBH.Text = "Subscribed Hours : " + mPBHMachine.HoursFrequency + " Elapsed Hours : " + mPBHMachine.ElapsedHoursText + " Remaining Hours : " + mPBHMachine.RemainingHoursText
			If mPBHMachine.RemainingHoursDec <= 1800 Then  'if Remaining hrs < than 30 hrs then mark red
				lblPBH.ForeColor = Color.Red
			Else
				lblPBH.ForeColor = Color.Maroon
			End If

			upnlpbh.Update()

		End If
		'End

		upnlSearchCriteria.Update()
		upnlGrid.Update()

	End Sub

	Private Sub DataFieldBindForPageLoad()

		If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
			'CNDC
			txtStartDate.Text = ""
			txtEndDate.Text = ""
		Else
			'CNDC
			txtStartDate.Text = StartDate
			txtEndDate.Text = EndDate
		End If

		StartDate = txtStartDate.Text
		EndDate = txtEndDate.Text

		Session("StartDate") = StartDate
		Session("EndDate") = EndDate

		chkShowAll.Checked = Session("chkShowAll") 'Added by Shital on 20-Oct-2021
		mLogTypeList = LogTypeList.GetLogTypeList()
		cmbLogType.DataSource = mLogTypeList
		cmbLogType.DataBind()

		LogListBind(True)

		upnlSearchCriteria.Update()

	End Sub

	Private Sub DataFieldBind()

		If Not IsDate(StartDate) Or Not IsDate(EndDate) Then
			'CNDC
			txtStartDate.Text = ""
			txtEndDate.Text = ""

		Else
			'CNDC
			txtStartDate.Text = StartDate
			txtEndDate.Text = EndDate

		End If

		StartDate = txtStartDate.Text
		EndDate = txtEndDate.Text

		Session("StartDate") = StartDate
		Session("EndDate") = EndDate

		mLogTypeList = LogTypeList.GetLogTypeList()
		cmbLogType.DataSource = mLogTypeList
		cmbLogType.DataBind()

		LogListBind()

	End Sub

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)

		If custValidator.ControlToValidate = "cmbAircraft" Then

			If cmbAircraft.SelectedIndex <= 0 Then
				e.IsValid = False
			End If

		End If

	End Sub

	'Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
	Public Sub DataFieldBindLogSelectionGrid(ToDate As String)

		Try

			mLogListForSelection = LogListForSelection.GetLogList(StartDate:=ToDate,
																  EndDate:=ToDate,
																  AssemblyID:=mMachine.AssemblyStatus.AssemblyID.ToString,
																  MachineID:=AircraftId.ToString,
																  CalculateTotal:=False, ,
																  StatusSelectLog:=1)

			GV_LogSelection.DataSource = mLogListForSelection
			GV_LogSelection.DataBind()
			Session("mLogListForSelection") = mLogListForSelection

			upnlLogSelection.Update()
			upnlLogSelectionGridView.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		ClearAll()
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)     'Added by Prashant on 20-July-2011

		Try

			If Not IsPostBack And CType(Session("sender"), String) = "" Then

				If cmbAircraft.Enabled = True Then
					cmbAircraft.Focus()
				End If

				'Added by Shital on 04-Mar-2022
				mTypeIDForLogEdit = Request.QueryString("Type")
				Session("mTypeIDForLogEdit") = mTypeIDForLogEdit
				'---

				If Session("mTypeIDForLogEdit") = 2 Then
					Session("MiddleFrame") = "wfLogList.aspx?Type=2"
				Else
					Session("MiddleFrame") = "wfLogList.aspx"
				End If

				DataFieldBindForPageLoad()
				SetGrid()
				SetPage()
				ControlVisibility()

			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Sub

	Private Sub Search(sender As Object, e As ImageClickEventArgs) Handles btnSearch.Click

		If IsValid Then

			If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
				SearchRecords()
			Else 'End

				If chkShowAll.Checked = True Then
					SearchRecords()
				Else
					SearchRecords(True)
				End If

			End If

			SetPage()

		Else

			upnlError.Update()
			mLogList = Nothing
			Session("mLogList") = mLogList
			gdvLogList.DataSource = Nothing
			gdvLogList.DataBind()
			upnlGrid.Update()

			SetGrid()
			SetPage()

		End If

	End Sub

	Private Sub ShowAll_Changed(sender As Object, e As EventArgs)

		If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
			SearchRecords()
		Else 'End
			If chkShowAll.Checked = True Then
				SearchRecords()
			Else
				SearchRecords(True)
			End If
		End If

		SetPage()
		ControlVisibility()

		If cmbAircraft.Enabled = True Then
			cmbAircraft.Focus()
		End If

	End Sub

	Private Sub GV_LogList_RowCommand(source As Object, e As GridViewCommandEventArgs) Handles gdvLogList.RowCommand

		Dim Index As Int32
		Dim ID As Guid

		Select Case e.CommandName
			Case "EditRec"

				Index = CInt(e.CommandArgument)
				Session("Index") = Index

				Session.Remove("isvaluezero")
				Session.Remove("mFileAttach")
				ID = mLogList(Index).ID
				mLogDetail = mLogList(Index).LogTextNo + " Dated : " + mLogList(Index).DateFormatted

				If (Not User.IsInRole("LogView") And Not User.IsInRole("LogEdit")) Then

					MarkLog(Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub

				End If

				EditRecord(ID)

			Case "DeleteRec"

				Index = CInt(e.CommandArgument)
				Session("Index") = Index
				ID = mLogList(Index).ID
				mLogDetail = mLogList(Index).LogTextNo + " Dated : " + mLogList(Index).DateFormatted

				If (Not User.IsInRole("LogDelete")) Then

					MarkLog(Action.Delete, "Flight Log", User.Identity.Name & " is not Authorized User to delete " & mLogDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub

				End If
				DeleteRecord(Index)

				'************ Added by Saylee on 19-Oct-2022 ,to give choice of print single from multiple attachments ***********************
			Case "ViewRec"    'Added By Prashant 28-July-2009

				Dim mFileAttachments As New FileAttachments
				Index = CInt(e.CommandArgument)

				If (Not User.IsInRole("LogView")) Then

					MarkLog(Action.Edit, "Flight Log", User.Identity.Name & " is not Authorized User to edit " & mLogDetail, ErrorType.HandledError, Guid.Empty, EventLogID)
					MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
					Exit Sub

				End If

				Dim Idx As Int32
				Dim mID As Guid
				Idx = CInt(e.CommandArgument)
				mID = mLogList(Idx).ID
				mLog = Log.GetLog(mID)

				mFileAttachments = FileAttachments.GetChildFileAttachments(mLog.ID)
				Dim AttachmentCount As Integer = mFileAttachments.Count
				DataFieldBind()
				SetGrid()
				upnlGrid.Update()
				Session("mLog") = mLog

				If AttachmentCount > 1 Then

					Session("mFileAttachments") = mFileAttachments
					Session("TransactionNameMarkLog") = "Flight Log" 'Used for Mark Log
					Session("TransactionName") = "Flight Log No. & Date"
					Session("TransactionDetails") = mLog.LogTextNo + " & " + mLog.DateFormatted.ToString

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenAttachWindow",
														"OpenAttachWindow();",
														True)

				Else

					Session("Index") = Index

					Dim No As New Random
					Dim StrName As String = "FlightLog" & No.Next.ToString

					ID = mLogList(Index).ID
					mFileAttach = FileAttach.GetAttachment(ID)

					If mFileAttach.Size > 0 Then

						If mFileAttach.FileName <> "" Then
							StrName = mFileAttach.FileName
						Else
							StrName = StrName & mFileAttach.Extension
						End If

						Dim path As String = AppSettings("DOCPath") & StrName ''& mFileAttach.Extension
						Dim fs As FileStream

						If File.Exists(AppSettings("DOCPath")) = False Then

							'Delete File if exist
							File.Delete(AppSettings("DOCPath") & StrName) ''& mFileAttach.Extension)
							' Create the file.
							fs = File.Create(path)
							'' Add some information to the file.
							fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
							fs.Close()
							Session("DOCPath") = path
							Dim Str As String
							Str = "openFile();"

							ScriptManager.RegisterStartupScript(Me,
																[GetType],
																"openFile",
																Str,
																True)

						End If

					End If

				End If

		End Select

	End Sub

	Private Sub GV_LogList_PageIndexChanging(source As Object, e As GridViewPageEventArgs) Handles gdvLogList.PageIndexChanging

		gdvLogList.PageIndex = e.NewPageIndex
		DataGridBind()
		Session("mLogList") = mLogList
		SetGrid()

	End Sub

	'Added By Prashant 22-June-2009 for grid sorting
	Private Sub GV_LogList_Sorting(source As Object, e As GridViewSortEventArgs) Handles gdvLogList.Sorting

		mLogList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
		Session("mLogList") = mLogList
		DataGridBind()
		SetGrid()

	End Sub
	'---------------------------------------------

	'Modified by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
	Private Sub AddLog(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Try

			If (Not User.IsInRole("LogNew")) Then

				MarkLog(Action.[New],
						"Flight Log",
						User.Identity.Name & " is not Authorized User to add.",
						ErrorType.NoError,
						Guid.Empty, EventLogID)   'Added By Prashant 20-Jul-2011

				MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
								MSGBox.Message_text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			Session.Remove("isvaluezero")
			Session.Remove("mFileAttach")

			Dim mMachineID As New Guid(AircraftId)
			Dim mMachine As Machine = Machine.GetMachine(mMachineID)

			If Not IsValid Then upnlError.Update() : Exit Sub

			Session("mMachine") = mMachine
			Session("LogListCount") = mLogList.Count

			'Added By Vikrant On 05-Nov-2015 For All05112015
			If mMachine.IsReadOnly Then

				MSGBoxCtrl.Show("Alert!",
								"As <b>" & cmbAircraft.SelectedItem.ToString & "</b> is marked as ReadOnly,You can not add new Flight Log Entry.",
								"",
								MsgBoxStyle.OkOnly,
								"")

				Exit Sub

			End If

			Dim str As String

			Dim str1 As String
			str1 = "delete_cookie();"
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												Guid.NewGuid.ToString,
												str1,
												True)


			If cmbLogType.SelectedValue = 1 Then

				mLog = Log.NewLog(Machine:=mMachine,
								  LogDate:=Today.ToShortDateString, , ,
								  LogTypeID:=cmbLogType.SelectedValue)

				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP

				If mLog.IsTLP = True Then

					If Session("mTypeIDForLogEdit") = 2 Then
						str = "openLedgerInSameWindow('wfTLPEdit_Ajax.aspx?BackPage=Index.aspx');"
					Else
						str = "openLedgerInSameWindow('wfTLP_Ajax.aspx?BackPage=Index.aspx');"
					End If

				Else

					If Session("mTypeIDForLogEdit") = 2 Then
						str = "openLedgerInSameWindow('wfLogSOPEdit_Ajax.aspx?BackPage=Index.aspx');"
					Else
						str = "openLedgerInSameWindow('wfLogSOP_Ajax.aspx?BackPage=Index.aspx');"
					End If

				End If


				mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.LogTypeID = cmbLogType.SelectedValue
				Session("mLog") = mLog

				If CHECK_IsRequiredAssembliesInstalled(mLog) = True Then

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"OpenScript",
														str,
														True)

				End If

			Else

				Dim LogType As String = String.Empty

				If cmbLogType.SelectedValue = 2 Then
					LogType = "MAINT. LOG"
				Else
					LogType = "VOID LOG"
				End If

				Dim LogDate As String

				If mLogList.Count > 0 Then
					If (mLogList(0).DesUniverseDateTime).ToString = "" Then
						LogDate = mLogList(0).Date.ToString
					Else

						If mMachine.IsUTC Then
							LogDate = (mLogList(0).DesUniverseDateTime)
						Else
							LogDate = (mLogList(0).DesLocalDateTime)
						End If

					End If
				Else
					LogDate = Today.ToShortDateString
				End If


				'**************Log Selection Pop-Up**************

				'Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
				If (AppSettings("ClientCode") = "Heligo" Or
					AppSettings("ClientCode") = "UHPL" Or
					AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP" Or
					mLogList.Count = 0) Then

					mLog = Log.NewLog(mMachine,
									  LogDate, , ,
									  cmbLogType.SelectedValue)

					mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
					mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
					mLog.LogTypeID = cmbLogType.SelectedValue
					Session("mLog") = mLog

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Log-Detail Window",
														"OpenLogDetailWindow()",
														True)

				Else

					txtLogSelectionDate.Text = New SmartDate(Today.Date.ToString).FormattedText
					ToDate = txtLogSelectionDate.Text

					DataFieldBindLogSelectionGrid(ToDate:=ToDate)

					mdlPopupLogSelection.Show()

				End If


			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseFlightLogScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		RemoveSession()

		Session("sender") = ""
		Session("MiddleFrame") = ""
		Session.Remove("AircraftId")
		Session.Remove("StartDate")
		Session.Remove("EndDate")

		Response.Redirect("Dashboard.aspx")

	End Sub

	Private Sub Aircraft_Changed(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged

		Page.Validate()
		upnlError.Update()

		'Added by saylee on 21-Apr-2011
		If IsValid Then
			If Trim(txtLogPageNo.Text) <> "" Then 'Added By Vikrant On 12-Dec-2013 For ALL09122013-2
				SearchRecords()
			Else 'End
				If chkShowAll.Checked = True Then
					SearchRecords(False) 'Show all records irrespective of 100
				Else
					SearchRecords(True)
				End If
			End If
			SetPage()

		Else

			mLogList = Nothing
			Session("mLogList") = mLogList
			gdvLogList.DataSource = Nothing
			gdvLogList.DataBind()
			SetPage()

		End If

		fname.Value = ""
		ControlVisibility()
		If cmbAircraft.Enabled = True Then
			cmbAircraft.Focus()
		End If

	End Sub

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MessageBoxResult()
	End Sub

	Private Sub HdnBtnVoidLog_Click(sender As Object, e As EventArgs) Handles hdnBtnVoidLog.Click

		If Session("chkShowAll") = True Then

			mLogList = LogList.GetLogList(New Guid(AircraftId),
											  txtStartDate.Text,
											  txtEndDate.Text,
											  False,
											  txtLogPageNo.Text.Trim)

		Else

			mLogList = LogList.GetLogList(New Guid(AircraftId),
											  txtStartDate.Text,
											  txtEndDate.Text,
											  True,
											  txtLogPageNo.Text.Trim)

		End If

		Session("mLogList") = mLogList
		DataGridBind()
		SetGrid()

	End Sub

	'Added For Import Utility
	Private Sub LnkImportFromAPI_Click(sender As Object, e As EventArgs) Handles lnkImportFromAPI.Click

		Session("mMachine") = mMachine
		Session("mLogList") = mLogList
		ImportLogs()

		ScriptManager.RegisterStartupScript(Me,
											[GetType],
											"OpenImportLogsWindow",
											"OpenImportLogsWindow();",
											True)

	End Sub

	Private Sub HdnBtnImportLogs_Click(sender As Object, e As EventArgs) Handles hdnBtnImportLogs.Click
		LogListBind()
		SetPage()
	End Sub
	'End

	'Added by Harsh on 15th July 2024
	Private Sub MarkFav(sender As Object, e As EventArgs) Handles hdnBtnMarkFavourite.Click 'Ajay 07-Nov-2022
		MarkFavourite(HttpContext.Current.User.Identity.Name, "Log")

	End Sub

	Private Sub RemoveFav(sender As Object, e As EventArgs) Handles hdnBtnRemoveFavourite.Click 'Ajay 07-Nov-2022
		RemoveFavourite(HttpContext.Current.User.Identity.Name, "Log")

	End Sub

	'Added by Harsh Sugandhi on 5th Feb 2025 => FLYPAL-2185
	Private Sub CloseLogSelection() Handles btnCloseLogSelection.Click

		Try
			mdlPopupLogSelection.Hide()
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_LogSelection_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GV_LogSelection.RowCommand

		Try

			Select Case e.CommandName
				Case "Select"

					Dim Index As Integer = CInt(e.CommandArgument) + GV_LogSelection.PageSize * GV_LogSelection.PageIndex

					mLogListForSelection = Session("mLogListForSelection")

					Dim ID As Guid = mLogListForSelection(Index).LogID

					mLog = Log.NewLog(Machine:=mMachine,
									  LogDate:=txtLogSelectionDate.Text, , ,
									  LogTypeID:=cmbLogType.SelectedValue,
									  PrevLogID:=mLogListForSelection(ID).LogID.ToString)

					mLog.IsUTC = mMachine.IsUTC
					mLog.IsTLP = mMachine.IsTLP
					mLog.LogTypeID = cmbLogType.SelectedValue
					Session("mLog") = mLog

					MarkLog(Action.[New],
							"Flight Log",
							"",
							ErrorType.NoError,
							mLog.ID,
							EventLogID) 'Added By Prashant 20-Jul-2011

					mdlPopupLogSelection.Hide()

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Open Log-Detail Window",
														"OpenLogDetailWindow()",
														True)

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_LogSelection_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GV_LogSelection.PageIndexChanging

		Try

			GV_LogSelection.PageIndex = e.NewPageIndex
			ToDate = txtLogSelectionDate.Text
			DataFieldBindLogSelectionGrid(ToDate:=ToDate)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub GV_LogSelection_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GV_LogSelection.Sorting

		Try

			mLogList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)

			DataFieldBindLogSelectionGrid(ToDate:=ToDate)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub LogSelectionDateChanged(sender As Object, e As EventArgs) Handles txtLogSelectionDate.TextChanged

		Try

			ToDate = txtLogSelectionDate.Text

			DataFieldBindLogSelectionGrid(ToDate:=ToDate)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'End

#End Region

#Region " Report "

#Region "Report Variable Declaration"

	Dim objStatus As rptStatus
	Private SearchStr1 As String = ""
	Private SearchStr2 As String = ""
	Private SearchStr3 As String = ""
	Private SearchStr4 As String = ""

#End Region

#Region " Event "

	Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click ''btnPrint.Click,

		If (Not User.IsInRole("LogPrint")) Then
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		If mLogList Is Nothing OrElse mLogList.Count = 0 Then
			MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
			Exit Sub
		End If

		Dim Rpt As New crListFlightLog
		Dim da As New ObjectAdapter
		Dim ds As New dsCommon
		Dim ReportDetails As New rptStatusList

		SearchStr1 = "The report shows records filtered by the following criteria"
		SearchStr2 = "Aircraft :" + "  " + cmbAircraft.SelectedItem.Text
		If StartDate = "" Then
			SearchStr3 = ""
		Else
			SearchStr3 = "Start Date :" + "  " + New SmartDate(txtStartDate.Text).FormattedText   'StartDate
		End If
		If EndDate = "" Then
			SearchStr4 = ""
		Else
			SearchStr4 = "End Date :" + "  " + New SmartDate(txtEndDate.Text).FormattedText   'EndDate
		End If

		If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
			ReportDetails.Add(New rptStatus(, 0, , , , , gdvLogList.Columns.Item(1).HeaderText, ,
							  gdvLogList.Columns.Item(2).HeaderText, gdvLogList.Columns.Item(3).HeaderText, gdvLogList.Columns.Item(4).HeaderText,
							 gdvLogList.Columns.Item(6).HeaderText, gdvLogList.Columns.Item(7).HeaderText,
							 gdvLogList.Columns.Item(9).HeaderText, gdvLogList.Columns.Item(10).HeaderText,
							 gdvLogList.Columns.Item(11).HeaderText, gdvLogList.Columns.Item(12).HeaderText))
		Else
			ReportDetails.Add(New rptStatus(, 0, , , , , gdvLogList.Columns.Item(1).HeaderText, ,
							  gdvLogList.Columns.Item(2).HeaderText, gdvLogList.Columns.Item(3).HeaderText, gdvLogList.Columns.Item(4).HeaderText,
							 gdvLogList.Columns.Item(5).HeaderText, gdvLogList.Columns.Item(7).HeaderText,
							 gdvLogList.Columns.Item(8).HeaderText, gdvLogList.Columns.Item(10).HeaderText,
							gdvLogList.Columns.Item(11).HeaderText, gdvLogList.Columns.Item(12).HeaderText))

		End If


		Dim TotalCount As Integer
		TotalCount = Me.mLogList.Count
		Dim m As Integer

		For m = 0 To TotalCount - 1
			Dim str(15) As String
			str(0) = ""
			str(1) = ""
			str(2) = ""
			str(3) = ""
			str(4) = ""
			str(5) = ""
			str(6) = ""
			str(7) = ""
			str(8) = ""
			str(9) = ""
			str(10) = ""
			str(11) = ""
			str(12) = ""
			str(13) = ""
			str(14) = ""
			str(15) = ""
			If Me.gdvLogList.Rows(m).Cells(1).Text <> "&nbsp;" Then str(0) = Me.gdvLogList.Rows(m).Cells(1).Text
			If Me.gdvLogList.Rows(m).Cells(2).Text <> "&nbsp;" Then str(1) = Me.gdvLogList.Rows(m).Cells(2).Text
			If Me.gdvLogList.Rows(m).Cells(3).Text <> "&nbsp;" Then str(2) = Me.gdvLogList.Rows(m).Cells(3).Text
			If Me.gdvLogList.Rows(m).Cells(4).Text <> "&nbsp;" Then str(3) = Me.gdvLogList.Rows(m).Cells(4).Text
			If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				' If (AppSettings("LogBookTimeEntry") = "UTC") Then
				If Me.gdvLogList.Rows(m).Cells(6).Text <> "&nbsp;" Then str(4) = Me.gdvLogList.Rows(m).Cells(6).Text
			Else
				If Me.gdvLogList.Rows(m).Cells(5).Text <> "&nbsp;" Then str(4) = Me.gdvLogList.Rows(m).Cells(5).Text
			End If

			If Me.gdvLogList.Rows(m).Cells(7).Text <> "&nbsp;" Then str(5) = Me.gdvLogList.Rows(m).Cells(7).Text
			If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				'If (AppSettings("LogBookTimeEntry") = "UTC") Then
				If Me.gdvLogList.Rows(m).Cells(9).Text <> "&nbsp;" Then str(6) = Me.gdvLogList.Rows(m).Cells(9).Text
			Else
				If Me.gdvLogList.Rows(m).Cells(8).Text <> "&nbsp;" Then str(6) = Me.gdvLogList.Rows(m).Cells(8).Text
			End If

			If Me.gdvLogList.Rows(m).Cells(10).Text <> "&nbsp;" Then str(7) = Me.gdvLogList.Rows(m).Cells(10).Text
			If Me.gdvLogList.Rows(m).Cells(11).Text <> "&nbsp;" Then str(8) = Me.gdvLogList.Rows(m).Cells(11).Text
			If Me.gdvLogList.Rows(m).Cells(12).Text <> "&nbsp;" Then str(9) = Me.gdvLogList.Rows(m).Cells(12).Text

			ReportDetails.Add(New rptStatus(, 1, , , , , str(0), , str(1),
			str(2), str(3), str(4), str(5),
			str(6), str(7), str(8), str(9)))

		Next

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

		Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
   mCompanyDetail.WebSite, "Flight Log List Report", SearchStr1, SearchStr2, SearchStr3, SearchStr4, "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

		Dim mrptImage As rptImage = rptImage.GetImage(ds)

		da.Fill(ds, ReportDetails)

		da.Fill(ds, Report)
		da.Fill(ds, mrptImage)

		Rpt.SetDataSource(ds)
		Session("CrystalReport") = Rpt
		'MarkLog(Action.Print, "Log", "Log List Report", ErrorType.NoError, Guid.Empty)
		Dim Str1 As String
		Str1 = "openTranDetail();"
		ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", Str1, True)

	End Sub

	Private Sub calStartDate_TextChanged(sender As Object, e As EventArgs)
		StartDate = txtStartDate.Text
	End Sub

	Private Sub calEndDate_TextChanged(sender As Object, e As EventArgs)
		EndDate = txtEndDate.Text
	End Sub

	Private Sub hdnBtnImportCRSLogs_Click(sender As Object, e As EventArgs) Handles hdnBtnImportCRSLogs.Click
		Session("mMachine") = mMachine
		Session("mLogList") = mLogList
		ImportCRSLogs()
		upnlGrid.Update()
		upnlLogGrid.Update()
	End Sub

	Private Sub cmbLogType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLogType.SelectedIndexChanged
		If mCompanyDetail.IsSyncApplication And cmbLogType.SelectedIndex = 0 Then
			'' btnAdd.Visible = False
			btnAddNew.Visible = False
		Else
			''btnAdd.Visible = True
			btnAddNew.Visible = True
		End If
		SetGrid()
		upnlButtons.Update()
	End Sub

#End Region

#End Region

End Class
