''******************************
'Created by:    'Prashant
'******************************


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web.Services


Public Class wfLogSOP_Ajax
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

#Region " Variable Declaration "

	Public mLog As Log
	Public mMachine As Machine
	Public mFlightLogClassificationList As FlightLogClassificationList
	Public DiscrepancyCorrectiveActionList As MELSnagCorrectiveActionListNew
	Public DiscrepancyCorrectiveAction As MELSnagCorrectiveAction
	Public mSearchListPilot As SearchList
	Public mSearchListPlace As SearchList
	Public EngineDerate As EngineDerate
	Public mCompanyDetail As New CompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
	Public mLogListOnDate As LogList

	Private Flag As Int16
	Dim Type As Integer
	Private LogListCount As Integer = 0
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim mLogDetail As String
	Dim TakeOffTouchDown As Boolean 'Added By Utkarsh On 21-Sep-2011
	Dim Pilot1ID As Guid
	Dim Pilot2ID As Guid
	Dim SourceID As Guid
	Dim DestinationID As Guid
	Dim SetValue As Boolean = False
	Dim IsValueZero As Boolean = False
	Public Event TextChanged As EventHandler  'End
	Dim FileAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False


#End Region

#Region " Business Methods "

	Private Sub GetSession()

		mLog = CType(Session("mLog"), Log)
		mMachine = CType(Session("mMachine"), Machine)
		mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)
		LogListCount = CType(Session("LogListCount"), Integer)
		mSearchListPlace = Session("mSearchListPlace")
		mSearchListPilot = Session("mSearchListPilot")
		'Added By Utkarsh On 21-Sep-2011
		Pilot1ID = CType(Session("Pilot1ID"), Guid)
		Pilot2ID = CType(Session("Pilot2ID"), Guid)
		SourceID = CType(Session("SourceID"), Guid)
		DestinationID = CType(Session("DestinationID"), Guid)
		SetValue = CType(Session("SetValue"), Boolean)
		'End
		FileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mLogListOnDate = Session("mLogListOnDate")
		mCompanyDetail = Session("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
		DiscrepancyCorrectiveActionList = Session("DiscrepancyCorrectiveActionList")
		EngineDerate = Session("EngineDerate")

	End Sub

	Private Sub SetSession()

		Session("mLog") = mLog
		Session("mMachine") = mMachine
		Session("mFlightLogClassificationList") = mFlightLogClassificationList
		Session("LogListCount") = LogListCount
		Session("mSearchListPlace") = mSearchListPlace
		Session("mSearchListPilot") = mSearchListPilot
		Session("Pilot1ID") = Pilot1ID
		Session("Pilot2ID") = Pilot2ID
		Session("SourceID") = SourceID
		Session("DestinationID") = DestinationID
		Session("SetValue") = SetValue
		Session("mLogListOnDate") = mLogListOnDate
		Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
		Session("EngineDerate") = EngineDerate

	End Sub

	Private Sub RemoveSession()
		Session.Remove("mMachine")
		''Session.Remove("mLogList")
		Session.Remove("mLog")
		Session.Remove("LogListCount")
		Session.Remove("mSearchListPlace")
		Session.Remove("mSearchListPilot")
		'Added By Utkarsh On 12-Sep-2011
		Session.Remove("Pilot1ID")
		Session.Remove("Pilot2ID")
		Session.Remove("SourceID")
		Session.Remove("DestinationID")
		Session.Remove("SetValue")
		'End
		Session.Remove("FileAttach")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("mLogListOnDate")
		Session.Remove("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
		Session.Remove("TroubleShootFromLog")
	End Sub

	Private Sub NewLogPax()
		Dim mLogPax As LogPax
		mLogPax = LogPax.NewLogPax(mLog.ID)
		Session("mLogPax") = mLogPax
	End Sub

	Private Sub NewHobbsOffSet()
		Dim mHobbsOffset As HobbsOffset
		mHobbsOffset = HobbsOffset.NewHobbsOffset(Guid.NewGuid, mLog.MachineID)
		Session("mHobbsOffset") = mHobbsOffset
	End Sub

	Private Sub SetFromSearch()

		Dim Type As Short = Val(Request.QueryString("Type"))
		Dim Id As String = Request.QueryString("Id")
		Dim Name As String = Request.QueryString("Name")
		Dim AddType As Short = Val(Request.QueryString("AddType"))

		Try

			'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
			If (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
				mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
			End If

			If Type = -1 Then

				Select Case AddType
					Case 0

						'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
						If (AppSettings("ClientCode") = "Heligo" Or
							AppSettings("ClientCode") = "UHPL" Or
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
							mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
						Else
							mLog.PilotID1 = New Guid(Id)
						End If

					Case 1
						mLog.PilotID2 = New Guid(Id)
					Case 2
						mLog.DestinationID = New Guid(Id)
					Case 3
						mLog.SourceID = New Guid(Id)
				End Select

			End If

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Overloads Sub SetFocus(control As WebControl)

		If control.Enabled = False Or control.Visible = False Then Exit Sub

		Try

			Dim str As String
			str = "try{document.getElementById('" + control.ClientID + "').focus();}catch (Error) {}"
			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Focus Script",
												str,
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub EnableDisableButton()

		Try

			btnLogPax.Enabled = Not mLog.IsNew
			btnDefectActionList.Enabled = Not mLog.IsNew
			'pnlPilot-Enabled
			calDateTime.Enabled = mLog.IsNew
			btnParameterList.Enabled = Not mLog.IsNew 'Added by Saylee on 6-Sep-2012
			btnFuelOil.Enabled = Not mLog.IsNew       'Added by Saylee on 6-Sep-2012
			btnFlightCrew.Enabled = Not mLog.IsNew      'Added by Saylee on 6-Sep-2012
			btnMaintenanceAcitvity.Enabled = Not mLog.IsNew 'Utkarsh

			If Not mLog.IsNew Then

			End If

			'grbAirGround

			If Not mLog.IsNew Then
				txtAirBorneTime.BackColor = Color.Gainsboro
				txtGroundRunTime.BackColor = Color.Gainsboro
				txtPercentTimeOnGround.BackColor = Color.Gainsboro
				txtPrevHobbsValue.BackColor = Color.Gainsboro
				txtPrevHobbsOffset.BackColor = Color.Gainsboro
				txtCurrentHobbsOffset.BackColor = Color.Gainsboro
				txtCurrentHobbsValue.BackColor = Color.Gainsboro
				txtTotalTime.BackColor = Color.Gainsboro
				txtBlockTime.BackColor = Color.Gainsboro
				txtBlockTime.ReadOnly = True
			Else                                                        ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
				txtAirBorneTime.BackColor = Color.White
				txtGroundRunTime.BackColor = Color.White
				txtPercentTimeOnGround.BackColor = Color.White
				'Detail Page code
				If AppSettings("SetBlockTime") = "True" Then
					txtBlockTime.BackColor = Color.White
					txtBlockTime.ReadOnly = False
				Else
					txtBlockTime.BackColor = Color.Gainsboro
					txtBlockTime.ReadOnly = True
				End If
				''''''''''''''''''''''''''
			End If

			'Place
			' If mLogList.Count = 0 Then
			If LogListCount = 0 Then
				imgbtnDepPlace.Enabled = True ''And mMachine.HourType = 1
				imgbtnArrPlace.Enabled = True ''And mMachine.HourType = 1
			End If
			'If mLogList.Count > 0 And mLog.Source.Name = "" Then
			If LogListCount > 0 And mLog.SourceName = "" Then
				imgbtnDepPlace.Enabled = True
				imgbtnArrPlace.Enabled = True
			End If
			'If mLogList.Count > 0 And mLog.Source.Name <> "" Then
			If LogListCount > 0 And mLog.SourceName <> "" Then
				'Comment opened for Indamar 18-May-2010
				''imgbtnDepPlace.Enabled = False
				imgbtnDepPlace.Enabled = True
				'======================================
				imgbtnArrPlace.Enabled = True ''And mMachine.HourType = 1
			End If
			'
			If Not imgbtnDepPlace.Enabled Then imgbtnDepPlace.BackColor = Color.Gainsboro
			If Not imgbtnArrPlace.Enabled Then imgbtnArrPlace.BackColor = Color.Gainsboro

			If mCompanyDetail.IsSyncApplication Then
				btnAddPlace.Visible = False
				btnAddPilots.Visible = False
				btnAddNew.Visible = False
			End If
			'End Place

			'Date 
			''If mLogList.Count = 0 Then
			If LogListCount = 0 Then
				calDeparture.Enabled = True  ''And mMachine.HourType = 1
				calArrival.Enabled = True ''And mMachine.HourType = 1
				calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
				calArrival.ReadOnly = Not (True) '' And mMachine.HourType = 1)

				txtDepartureTime.Enabled = True
				txtArrivalTime.Enabled = True
				txtDepartureTime.ReadOnly = Not (True)
				txtArrivalTime.ReadOnly = Not (True)

				'Added By Utkarsh On 31-Aug-2011
				If TakeOffTouchDown Then
					calTakeOffLocalDateTime.Enabled = True
					calTouchDownLocalDateTime.Enabled = True
					calTakeOffLocalDateTime.ReadOnly = Not (True)
					calTouchDownLocalDateTime.ReadOnly = Not (True)

					txtTakeOffLocalTime.Enabled = True
					txtTouchDownLocalTime.Enabled = True
					txtTakeOffLocalTime.ReadOnly = Not (True)
					txtTouchDownLocalTime.ReadOnly = Not (True)
				End If
				'End

			End If

			If LogListCount > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString <> "" Then
				calDeparture.Enabled = True  ''And mMachine.HourType = 1
				'calArrival.Enabled = False
				calArrival.Enabled = True
				calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
				calArrival.ReadOnly = Not (True)

				txtDepartureTime.Enabled = True
				txtArrivalTime.Enabled = True
				txtDepartureTime.ReadOnly = Not (True)
				txtArrivalTime.ReadOnly = Not (True)
			End If

			'Added By Utkarsh On 31-Aug-2011

			If Not mLog.IsNew Then
				calDeparture.Enabled = False
				calArrival.Enabled = False
				CalUTCDateTime.Enabled = False
				CalUTCArrival.Enabled = False

				Place1.Enabled = False
				Place2.Enabled = False

				Place1.BackColor = Color.Gainsboro
				Place2.BackColor = Color.Gainsboro

				txtDepartureTime.Enabled = False
				txtArrivalTime.Enabled = False
				txtUTCDepartureTime.Enabled = False
				txtUTCArrivalTime.Enabled = False
				chkArrival.Enabled = False
				chkTouchDown.Enabled = False
				chkTakeOff.Enabled = False
			Else                                                    ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
				Place1.Enabled = True
				Place2.Enabled = True
				Place1.ReadOnly = False
				Place2.ReadOnly = False
				Place1.BackColor = Color.White
				Place2.BackColor = Color.White
				chkArrival.Enabled = True
				chkTouchDown.Enabled = True
				chkTakeOff.Enabled = True
			End If

			'Date Light Time
			If mLog.SouIsDayLight = True Then
				cmbDepartureDayLightTime.Enabled = True ''And mMachine.HourType = 1
			Else
				cmbDepartureDayLightTime.Enabled = False
			End If
			If mLog.DesIsDayLight = True Then
				cmbArrivalDayLightTime.Enabled = True ''And mMachine.HourType = 1
			Else
				cmbArrivalDayLightTime.Enabled = False
			End If

			'End Date Light Time

			'Hobbs-taken
			btnHobbsOffset.Enabled = (mMachine.HourType = 2)
			pnlHours.Visible = True '= Not (mMachine.HourType = 2) 'Added Code
			pnlDecimal.Visible = (mMachine.HourType = 2)
			plDecimal.Visible = (mMachine.HourType = 2)
			'================Visibility for Hours and Decimal===================
			'*pnlHours   

			'Code Added By Girish April.09,2007

			lblAirBorneTime.Visible = True
			txtAirBorneTime.Visible = True
			txtBlockTime.Visible = True
			lblGroundRunTime.Visible = True
			txtGroundRunTime.Visible = True
			lblPercentTimeOnGround.Visible = True
			txtPercentTimeOnGround.Visible = True

			'pnlDecimal
			lblHobbsPrevVal.Visible = (mMachine.HourType = 2)
			txtPrevHobbsValue.Visible = (mMachine.HourType = 2)
			lblOffsetPreVal.Visible = (mMachine.HourType = 2)
			txtPrevHobbsOffset.Visible = (mMachine.HourType = 2)
			lblOffsetCurrentVal.Visible = (mMachine.HourType = 2)
			txtCurrentHobbsOffset.Visible = (mMachine.HourType = 2)
			lblHobbsCurrentReading.Visible = (mMachine.HourType = 2)
			txtCurrentHobbsValue.Visible = (mMachine.HourType = 2)

			'===========ReadOnly for Hours and Decimal=============
			lblairfly.Visible = (mMachine.HourType = 1)
			txtBlockTime.Visible = (mMachine.HourType = 1)
			lblAirBorneTime.Visible = (mMachine.HourType = 1)
			txtAirBorneTime.Visible = (mMachine.HourType = 1)
			lblGroundRunTime.Visible = (mMachine.HourType = 1)
			txtGroundRunTime.Visible = (mMachine.HourType = 1)
			lblPercentTimeOnGround.Visible = (mMachine.HourType = 1)
			txtPercentTimeOnGround.Visible = (mMachine.HourType = 1)

			'Added By Utkarsh On 31-Aug-2011

			If TakeOffTouchDown And mLog.IsLogAirborneEntry = False Then  'Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = False
				txtAirBorneTime.BackColor = Color.Gainsboro
				txtGroundRunTime.BackColor = Color.Gainsboro
				txtAirBorneTime.ReadOnly = True
				txtGroundRunTime.ReadOnly = True
			End If

			lblTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			lblUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
			lblTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			lblUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)

			calTouchDownLocalDateTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			calUTCTouchDownDateTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			calTakeOffLocalDateTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			calUTCTakeOffDateTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)

			calTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			calUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
			calTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			calUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)

			txtTouchDownLocalTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			txtUTCTouchDownTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			txtTakeOffLocalTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)
			txtUTCTakeOffTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown And mLog.IsNew)

			txtTakeOffLocalTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			txtUTCTakeOffTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
			txtTouchDownLocalTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
			txtUTCTouchDownTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)


			chkTakeOff.Visible = TakeOffTouchDown
			chkTouchDown.Visible = TakeOffTouchDown

			'pnlHours

			'Added By Utkarsh On 05-Sep-2011
			If Not TakeOffTouchDown Then
				txtAirBorneTime.ReadOnly = Not mLog.IsNew
			End If
			'End
			txtCurrentHobbsValue.ReadOnly = Not mLog.IsNew


			'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
			calDeparture.Enabled = Not (mMachine.IsUTC)
			calArrival.Enabled = Not (mMachine.IsUTC)
			CalUTCDateTime.Enabled = (mMachine.IsUTC)
			CalUTCArrival.Enabled = (mMachine.IsUTC)

			calDeparture.Visible = Not (mMachine.IsUTC)
			lblDepDateTime.Visible = Not (mMachine.IsUTC)

			txtDepartureTime.Enabled = Not (mMachine.IsUTC)
			txtArrivalTime.Enabled = Not (mMachine.IsUTC)
			txtDepartureTime.Visible = Not (mMachine.IsUTC)
			txtArrivalTime.Visible = Not (mMachine.IsUTC)
			txtUTCDepartureTime.Enabled = (mMachine.IsUTC)
			txtUTCArrivalTime.Enabled = (mMachine.IsUTC)

			calArrival.Visible = Not (mMachine.IsUTC)
			lblArrDate.Visible = Not (mMachine.IsUTC)

			CalUTCDateTime.Visible = (mMachine.IsUTC)
			lblUTCDateTime.Visible = (mMachine.IsUTC)

			CalUTCArrival.Visible = (mMachine.IsUTC)
			lblUTCArrivalDateTime.Visible = (mMachine.IsUTC)

			txtUTCDepartureTime.Visible = (mMachine.IsUTC)
			txtUTCArrivalTime.Visible = (mMachine.IsUTC)
			'Added By Utkarsh On 31-Aug-2011

			If Not mLog.IsNew Then

				calDeparture.Enabled = False
				calArrival.Enabled = False
				CalUTCDateTime.Enabled = False
				CalUTCArrival.Enabled = False
				Place1.ReadOnly = True
				Place2.ReadOnly = True
				Place1.BackColor = Color.Gainsboro
				txtDepartureTime.Enabled = False
				txtArrivalTime.Enabled = False
				txtUTCDepartureTime.Enabled = False
				txtUTCArrivalTime.Enabled = False

			End If

			upnlTabs.Update()
			upnlFlightDetails.Update()
			upnlFlightSummary.Update()
			upnlTabsNew.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Modified by Harsh Sugandhi on 20th August 2025 For FLYPAL-2629
	Private Sub ControlVisibility()

		Try

			SetGridColumnVisibility(gvAFPeriods, mLog.LogAFAssemblies, "Airframe")
			SetGridColumnVisibility(gvEnginePeriods, mLog.LogEngAssemblies, "Engine")
			SetGridColumnVisibility(gvAPUPeriods, mLog.LogAPUAssemblies, "APU")
			SetGridColumnVisibility(gvCGBPeriods, mLog.LogCGBAssemblies, "CGB")
			SetGridColumnVisibility(gvALLAssemblies, mLog.ALL_LogAssemblies, "ALL")

			Dim showAPU As Boolean = mLog.LogAPUAssemblies.Count > 0
			gvAPUPeriods.Visible = showAPU
			lblAPUPeriod.Visible = showAPU
			fldAPUPeriods.Visible = showAPU

			Dim showCGB As Boolean = mLog.LogCGBAssemblies.Count > 0
			gvCGBPeriods.Visible = showCGB
			lblCGBPeriod.Visible = showCGB
			fldCGBPeriods.Visible = showCGB

			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetGridColumnVisibility(gridView As GridView, logAssembly As Object, assemblyType As String)

		Dim columnMapping As New Dictionary(Of String, Integer())

		Try

			If assemblyType = "Engine" Then

				columnMapping.Add("ShowHours", New Integer() {3, 4})
				columnMapping.Add("ShowLandings", New Integer() {5, 6})
				columnMapping.Add("ShowCycles", New Integer() {7, 8})
				columnMapping.Add("ShowStarts", New Integer() {9, 10})
				columnMapping.Add("ShowNGCycles", New Integer() {11, 12})
				columnMapping.Add("ShowNFCycles", New Integer() {13, 14})
				columnMapping.Add("ShowRINS", New Integer() {15, 16})
				columnMapping.Add("ShowCFactor", New Integer() {17, 18})
				columnMapping.Add("ShowBleeds", New Integer() {19, 20})
				columnMapping.Add("ShowImpellerCycles", New Integer() {21, 22})
				columnMapping.Add("ShowCTCycles", New Integer() {23, 24})
				columnMapping.Add("ShowPTCycles", New Integer() {25, 26})
				columnMapping.Add("ShowGeneratorMods", New Integer() {27, 28})
				columnMapping.Add("ShowRapidTakeOffFactors", New Integer() {29, 30})
				columnMapping.Add("ShowN1Cycles", New Integer() {31, 32})
				columnMapping.Add("ShowN2Cycles", New Integer() {33, 34})

			ElseIf assemblytype = "ALL" Then

				columnMapping.Add("ShowHours", New Integer() {4, 5})
				columnMapping.Add("ShowLandings", New Integer() {6, 7})
				columnMapping.Add("ShowCycles", New Integer() {8, 9})
				columnMapping.Add("ShowStarts", New Integer() {10, 11})
				columnMapping.Add("ShowNGCycles", New Integer() {12, 13})
				columnMapping.Add("ShowNFCycles", New Integer() {14, 15})
				columnMapping.Add("ShowRINS", New Integer() {16, 17})
				columnMapping.Add("ShowBleeds", New Integer() {18, 19})
				columnMapping.Add("ShowImpellerCycles", New Integer() {20, 21})
				columnMapping.Add("ShowCTCycles", New Integer() {22, 23})
				columnMapping.Add("ShowPTCycles", New Integer() {24, 25})
				columnMapping.Add("ShowGeneratorMods", New Integer() {26, 27})
				columnMapping.Add("ShowNRCycles", New Integer() {28, 29})
				columnMapping.Add("ShowLandingCycles", New Integer() {30, 31})
				columnMapping.Add("ShowLandingGearCycles", New Integer() {32, 33})
				columnMapping.Add("ShowOverSpeedLHMLGCycles", New Integer() {34, 35})
				columnMapping.Add("ShowOverSpeedRHMLGCycles", New Integer() {36, 37})
				columnMapping.Add("ShowOverSpeedNLGCycles", New Integer() {38, 39})
				columnMapping.Add("ShowMGBTorqueCycles", New Integer() {40, 41})
				columnMapping.Add("ShowRotorBrakeCycles", New Integer() {42, 43})

			Else

				columnMapping.Add("ShowHours", New Integer() {3, 4})
				columnMapping.Add("ShowLandings", New Integer() {5, 6})
				columnMapping.Add("ShowCycles", New Integer() {7, 8})
				columnMapping.Add("ShowStarts", New Integer() {9, 10})
				columnMapping.Add("ShowNGCycles", New Integer() {11, 12})
				columnMapping.Add("ShowNFCycles", New Integer() {13, 14})
				columnMapping.Add("ShowRINS", New Integer() {15, 16})
				columnMapping.Add("ShowBleeds", New Integer() {17, 18})
				columnMapping.Add("ShowImpellerCycles", New Integer() {19, 20})
				columnMapping.Add("ShowCTCycles", New Integer() {21, 22})
				columnMapping.Add("ShowPTCycles", New Integer() {23, 24})
				columnMapping.Add("ShowGeneratorMods", New Integer() {25, 26})
				columnMapping.Add("ShowNRCycles", New Integer() {27, 28})
				columnMapping.Add("ShowLandingCycles", New Integer() {29, 30})
				columnMapping.Add("ShowLandingGearCycles", New Integer() {31, 32})
				columnMapping.Add("ShowOverSpeedLHMLGCycles", New Integer() {33, 34})
				columnMapping.Add("ShowOverSpeedRHMLGCycles", New Integer() {35, 36})
				columnMapping.Add("ShowOverSpeedNLGCycles", New Integer() {37, 38})
				columnMapping.Add("ShowMGBTorqueCycles", New Integer() {39, 40})
				columnMapping.Add("ShowRotorBrakeCycles", New Integer() {41, 42})

			End If

			For Each mapping As KeyValuePair(Of String, Integer()) In columnMapping

				Dim prop As Reflection.PropertyInfo = logAssembly.GetType().GetProperty(mapping.Key)

				If prop IsNot Nothing Then

					Dim isVisible As Boolean = CType(prop.GetValue(logAssembly, Nothing), Boolean)

					For Each colIndex As Integer In mapping.Value

						If colIndex < gridView.Columns.Count Then
							gridView.Columns(colIndex).Visible = isVisible
						End If

					Next

				End If

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function IsZeroValueLog(Optional isFromDataBindGrid As Boolean = False) As Boolean  ' For First Grid i.e AirFrame

		Dim isZero As Boolean = False
		Dim flag As Boolean = False

		Try

			For i As Integer = 0 To mLog.LogAFAssemblies.Count - 1

				If mLog.LogAFAssemblies.ShowHours Then

					If mLog.IsHobbs Then

						If Val(mLog.LogAFAssemblies(i).Hours) = 0 Then
							flag = True
							Exit For
						End If

					Else

						If mLog.LogAFAssemblies(i).Hours = "0:00" Then
							flag = True
							Exit For
						End If

					End If

				End If

				If mLog.LogAFAssemblies.ShowLandings Then

					If Val(mLog.LogAFAssemblies(i).Landings) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowCycles Then

					If Val(mLog.LogAFAssemblies(i).Cycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowStarts Then

					If Val(mLog.LogAFAssemblies(i).Starts) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowNGCycles Then

					If Val(mLog.LogAFAssemblies(i).NGCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowNFCycles Then

					If Val(mLog.LogAFAssemblies(i).NFCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowRINS Then

					If Val(mLog.LogAFAssemblies(i).RINS) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowBleeds Then

					If Val(mLog.LogAFAssemblies(i).Bleeds) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowImpellerCycles Then

					If Val(mLog.LogAFAssemblies(i).ImpellerCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowCTCycles Then

					If Val(mLog.LogAFAssemblies(i).CTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogAFAssemblies.ShowPTCycles Then

					If Val(mLog.LogAFAssemblies(i).PTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				'Added by shweta on 7-May-2012 for ALL02052012
				If mLog.LogAFAssemblies.ShowGeneratorMods Then

					If Val(mLog.LogAFAssemblies(i).GeneratorMods) = 0 Then
						flag = True
						Exit For
					End If

				End If

			Next

			If flag = True Then
				flag = False
				isZero = True
			End If

			'Engine
			For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

				If mLog.LogEngAssemblies(i).ShowHours Then

					If mLog.IsHobbs Then

						If Val(mLog.LogEngAssemblies(i).Hours) = 0 Then
							flag = True
							Exit For
						End If

					Else

						If mLog.LogEngAssemblies(i).Hours = "0:00" Then
							flag = True
							Exit For
						End If

					End If

				End If

				If mLog.LogEngAssemblies(i).ShowLandings Then

					If Val(mLog.LogEngAssemblies(i).Landings) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowCycles Then

					If Val(mLog.LogEngAssemblies(i).Cycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowStarts Then

					If Val(mLog.LogEngAssemblies(i).Starts) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowNGCycles Then

					If Val(mLog.LogEngAssemblies(i).NGCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowNFCycles Then

					If Val(mLog.LogEngAssemblies(i).NFCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowRINS Then

					If Val(mLog.LogEngAssemblies(i).RINS) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowBleeds Then

					If Val(mLog.LogEngAssemblies(i).Bleeds) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowGeneratorMods Then

					If Val(mLog.LogEngAssemblies(i).GeneratorMods) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowImpellerCycles Then

					If Val(mLog.LogEngAssemblies(i).ImpellerCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowCTCycles Then

					If Val(mLog.LogEngAssemblies(i).CTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogEngAssemblies(i).ShowPTCycles Then

					If Val(mLog.LogEngAssemblies(i).PTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

			Next

			If flag = True Then
				flag = False
				isZero = True
			End If

			'Added by Shweta on 8May-2012
			For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1

				If mLog.LogCGBAssemblies.ShowHours Then

					If mLog.IsHobbs Then

						If Val(mLog.LogCGBAssemblies(i).Hours) = 0 Then
							flag = True
							Exit For
						End If

					Else

						If mLog.LogCGBAssemblies(i).Hours = "0:00" Then
							flag = True
							Exit For
						End If

					End If

				End If

				If mLog.LogCGBAssemblies.ShowLandings Then

					If Val(mLog.LogCGBAssemblies(i).Landings) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowCycles Then

					If Val(mLog.LogCGBAssemblies(i).Cycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowStarts Then

					If Val(mLog.LogCGBAssemblies(i).Starts) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowNGCycles Then

					If Val(mLog.LogCGBAssemblies(i).NGCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowNFCycles Then

					If Val(mLog.LogCGBAssemblies(i).NFCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowRINS Then

					If Val(mLog.LogCGBAssemblies(i).RINS) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowBleeds Then

					If Val(mLog.LogCGBAssemblies(i).Bleeds) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowImpellerCycles Then

					If Val(mLog.LogCGBAssemblies(i).ImpellerCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowCTCycles Then

					If Val(mLog.LogCGBAssemblies(i).CTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				If mLog.LogCGBAssemblies.ShowPTCycles Then

					If Val(mLog.LogCGBAssemblies(i).PTCycles) = 0 Then
						flag = True
						Exit For
					End If

				End If

				'Added by Shweta on 7-May-2012 for ALL02052012
				If mLog.LogCGBAssemblies.ShowGeneratorMods Then

					If Val(mLog.LogCGBAssemblies(i).GeneratorMods) = 0 Then
						flag = True
						Exit For
					End If

				End If

			Next

			If flag = True Then
				flag = False
				isZero = True
			End If

			Return isZero

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Sub SetObject()

		Try

			With mLog

				'CNDC
				If Not IsDate(calDateTime.Text) Then
					.Date = DBNull.Value
				Else
					.Date = calDateTime.Text.ToString.Trim
				End If

				.LogText = Trim(txtLogText.Text)
				.LogNo = CInt(Val(Trim(txtLogNo.Text)))

				If .IsUTC = True Then

					'CNDC
					If Not IsDate(CalUTCDateTime.Text) Then
						.SouUniverseDateTime = DBNull.Value
					Else
						.SouUniverseDateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)
					End If

				Else

					If Not IsDate(calDeparture.Text) Then
						.SouLocalDateTime = DBNull.Value
					Else
						.SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime)
					End If

				End If

				.SouDayLightTime = cmbDepartureDayLightTime.SelectedValue

				If .IsUTC = True Then

					'CNDC
					If Not IsDate(CalUTCArrival.Text) Then
						.DesUniverseDateTime = DBNull.Value
					Else
						.DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime)
					End If

				Else

					'CNDC
					If Not IsDate(calArrival.Text) Then
						.DesLocalDateTime = DBNull.Value
					Else
						.DesLocalDateTime = CType(calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim, DateTime)
					End If

				End If

				.DesDayLightTime = cmbArrivalDayLightTime.SelectedValue

				'Added By Utkarsh On 31-Aug-2011
				If .IsUTC Then

					If TakeOffTouchDown Then

						If Not IsDate(calUTCTakeOffDateTime.Text) Then
							.TakeOffUniverseDateTime = DBNull.Value
						Else
							.TakeOffUniverseDateTime = CType(calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim, DateTime)
						End If

						If Not IsDate(calUTCTouchDownDateTime.Text) Then
							.TouchDownUniverseDateTime = DBNull.Value
						Else
							.TouchDownUniverseDateTime = CType(calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim, DateTime)
						End If

					End If
					'End

				Else

					'Added By Utkarsh On 31-Aug-2011
					If TakeOffTouchDown Then

						If Not IsDate(calTakeOffLocalDateTime.Text) Then
							.TakeOffLocalDateTime = DBNull.Value
						Else
							.TakeOffLocalDateTime = CType(calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim, DateTime)
						End If

						If Not IsDate(calTouchDownLocalDateTime.Text) Then
							.TouchDownLocalDateTime = DBNull.Value
						Else
							.TouchDownLocalDateTime = CType(calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim, DateTime)
						End If

					End If

				End If
				'End

				'Detail Page code ***************************
				If AppSettings("SetBlockTime") = "True" Then

					If Not .BlockTime.Equals(Trim(txtBlockTime.Text)) Then

						.BlockTime = Trim(txtBlockTime.Text)
						txtAirBorneTime.DataBind()
						txtGroundRunTime.DataBind()

					End If

				End If

				If AppSettings("LogDetailPage") = "OldPage" Then

					.TimeInAir = Trim(txtAirBorneTime.Text)

					If Not AppSettings("Log") = "True" Then .TimeOnGround = Trim(txtGroundRunTime.Text)

				End If
				'******************************************************

				.PercentTimeOnGround = Val(Trim(txtPercentTimeOnGround.Text))

				If mMachine.HourType = 2 Then

					.PrevHobbsValue = Trim(txtPrevHobbsValue.Text)
					.PrevHobbsOffsetValue = Trim(txtPrevHobbsOffset.Text)
					.CurrentHobbsOffsetValue = Trim(txtCurrentHobbsOffset.Text)
					.CurrentHobbsValue = Trim(txtCurrentHobbsValue.Text)
					.OffSet = Trim(txtCurrentHobbsOffset.Text)

				End If

				.LogPageNo = txtLogPageNo.Text
				.FlightNo = txtFlightNo.Text.Trim
				.Remark = Trim(txtRemark.Text)
				.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
				.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text

				'Added by Shweta on 10-FEB-12
				If Session("IsValueZero") = "True" Then
					.IsValZero = True
				Else
					.IsValZero = False
				End If

				If FileAttach IsNot Nothing Then

					If FileAttach.Size > 0 Then
						.IsAttachmentAdded = True
					Else
						.IsAttachmentAdded = False
					End If

				End If

				If CBool(AppSettings("ShowEngineDerateOptions")) Then
					.EngineDerateID = Val(ddlEngineDerate.SelectedValue.ToString)
					.EngineDerateValue = ddlEngineDerate.SelectedItem.Text
				Else
					.EngineDerateID = 0
					.EngineDerateValue = ""
				End If

			End With

			'Added by Saylee on 18-Oct-2022, for Multiple Attachment
			For i As Integer = 0 To mLog.FileAttachments.Count - 1

				Dim txtValue As TextBox
				txtValue = CType(Me.dgLogAttachment.Rows(i).FindControl("txtFileName"), TextBox)
				mLog.FileAttachments(i).FileName = txtValue.Text.Trim

			Next

			mLog.IsAttachmentAdded = IIf(mLog.FileAttachments.Count > 0, True, False)

			gvAFPeriods.DataBind()
			gvEnginePeriods.DataBind()
			gvAPUPeriods.DataBind()
			gvCGBPeriods.DataBind() 'Added By Prashant 23-Oct-2009

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewRecord()

		Try

			mLog = Log.NewLog(mMachine, Today.Date)
			mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
			mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
			mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry

			If (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

				mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
				mLog.Pilot1Name = "None"

			End If

			Session("mLog") = mLog

			MarkLog(Action.[New],
					"Flight Log",
					"",
					ErrorType.HandledError,
					mLog.ID,
					EventLogID)

			SetTitle()

			Dim str1 As String
			str1 = "delete_cookie();"
			ScriptManager.RegisterStartupScript(Me, [GetType], Guid.NewGuid.ToString, str1, True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function IsEngineHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

		Dim IsSame As Boolean = True

		Try

			For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1

				If mLog.TotalTime = mLog.LogEngAssemblies(i).Hours Then
					IsSame = True
				Else
					IsSame = False
					Exit For
				End If

			Next

			If IsSame = True Then
				Return True
			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Public Function IsCGBHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

		Dim IsSame As Boolean = True

		Try

			For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1

				If mLog.TotalTime = mLog.LogCGBAssemblies(i).Hours Then
					IsSame = True
				Else
					IsSame = False
					Exit For
				End If

			Next

			If IsSame = True Then
				Return True
			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
	Public Function AvgFlightTimeDeviation() As Boolean

		If AppSettings("NoOfLogsToConsiderForAvgFlightTime") <> "0" And AppSettings("DeviationInAvgFlightTimeInPercentage") <> "0" Then

			Dim mLastLogDetails As LastLogDetails = LastLogDetails.GetLastLogDetails(False, mLog.DateFormatted.ToString, CType(AppSettings("NoOfLogsToConsiderForAvgFlightTime"), Integer), mLog.SourceID.ToString, mLog.DestinationID.ToString, mMachine.AssemblyStatus.Assembly.ModelID.ToString)

			If mLastLogDetails.Count > 0 Then

				Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, mLog.TimeInAir, 0, False, False).DbValueDec
				Dim AllowedDeviationInDec = (mLastLogDetails.AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
				Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - mLastLogDetails.AvgFlightTime)

				If ActualDeviationInDec > AllowedDeviationInDec Then

					If CurrentLogTimeInAirInDec > mLastLogDetails.AvgFlightTime Then
						Session("IsFlightTimeGreaterThanAvgFlightTime") = "True"
					End If

					Return True

				Else
					Return False
				End If

			Else
				Return False
			End If

		Else
			Return False
		End If

	End Function

	Public Sub SetAirFrameGridObject(Optional isFromDataBindGrid As Boolean = False)  ' For First Grid i.e AirFrame

		Dim txtAirFrameHours,
			txtAirFrameLandings,
			txtAirFrameCycles,
			txtAirFrameStarts,
			txtAirFrameNGCycles,
			txtAirFrameNFCycles,
			txtAirFrameRins,
			txtAirFrameBleeds,
			txtAirFrameImpellerCycles,
			txtAirFrameCTCycles,
			txtAirFramePTCycles,
			txtAirframeGeneratorMods As TextBox

		Dim txtAirframeNRCycles,
			txtAirframeLandingCycles,
			txtAirframeLandingGearCycles,
			txtAirframeOverSpeedLHMLGCycles,
			txtAirframeOverSpeedRHMLGCycles,
			txtAirframeOverSpeedNLGCycles,
			txtAirframeMGBTorqueCycles,
			txtAirframeRotorBrakeCycles As TextBox

		Try

			For i As Integer = 0 To gvAFPeriods.Rows.Count - 1

				txtAirFrameHours = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameHours"), TextBox)
				txtAirFrameLandings = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameLandings"), TextBox)
				txtAirFrameCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameCycles"), TextBox)
				txtAirFrameStarts = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameStarts"), TextBox)
				txtAirFrameNGCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameNGCycles"), TextBox)
				txtAirFrameNFCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameNFCycles"), TextBox)
				txtAirFrameRins = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameRins"), TextBox)
				txtAirFrameBleeds = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameBleeds"), TextBox)
				txtAirFrameImpellerCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameImpellerCycles"), TextBox)
				txtAirFrameCTCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFrameCTCycles"), TextBox)
				txtAirFramePTCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirFramePTCycles"), TextBox)
				txtAirframeGeneratorMods = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeGeneratorMods"), TextBox)

				txtAirframeNRCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeNRCycles"), TextBox)
				txtAirframeLandingCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeLandingCycles"), TextBox)
				txtAirframeLandingGearCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeLandingGearCycles"), TextBox)
				txtAirframeOverSpeedLHMLGCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeOverSpeedLHMLGCycles"), TextBox)
				txtAirframeOverSpeedRHMLGCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeOverSpeedRHMLGCycles"), TextBox)
				txtAirframeOverSpeedNLGCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeOverSpeedNLGCycles"), TextBox)
				txtAirframeMGBTorqueCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeMGBTorqueCycles"), TextBox)
				txtAirframeRotorBrakeCycles = CType(Me.gvAFPeriods.Rows(i).FindControl("txtAirframeRotorBrakeCycles"), TextBox)

				If isFromDataBindGrid Then If mLog.LogAFAssemblies.ShowHours Then mLog.LogAFAssemblies(i).Hours = Trim(txtAirFrameHours.Text)

				If mLog.LogAFAssemblies.ShowLandings Then mLog.LogAFAssemblies(i).Landings = Trim(txtAirFrameLandings.Text)
				If mLog.LogAFAssemblies.ShowCycles Then mLog.LogAFAssemblies(i).Cycles = Trim(txtAirFrameCycles.Text)
				If mLog.LogAFAssemblies.ShowStarts Then mLog.LogAFAssemblies(i).Starts = Trim(txtAirFrameStarts.Text)
				If mLog.LogAFAssemblies.ShowNGCycles Then mLog.LogAFAssemblies(i).NGCycles = Trim(txtAirFrameNGCycles.Text)
				If mLog.LogAFAssemblies.ShowNFCycles Then mLog.LogAFAssemblies(i).NFCycles = Trim(txtAirFrameNFCycles.Text)
				If mLog.LogAFAssemblies.ShowRINS Then mLog.LogAFAssemblies(i).RINS = Trim(txtAirFrameRins.Text)
				If mLog.LogAFAssemblies.ShowBleeds Then mLog.LogAFAssemblies(i).Bleeds = Trim(txtAirFrameBleeds.Text)
				If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.LogAFAssemblies(i).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
				If mLog.LogAFAssemblies.ShowCTCycles Then mLog.LogAFAssemblies(i).CTCycles = Trim(txtAirFrameCTCycles.Text)
				If mLog.LogAFAssemblies.ShowPTCycles Then mLog.LogAFAssemblies(i).PTCycles = Trim(txtAirFramePTCycles.Text)

				If mLog.LogAFAssemblies.ShowNRCycles Then mLog.LogAFAssemblies(i).NRCycles = Trim(txtAirframeNRCycles.Text)
				If mLog.LogAFAssemblies.ShowLandingCycles Then mLog.LogAFAssemblies(i).LandingCycles = Trim(txtAirframeLandingCycles.Text)
				If mLog.LogAFAssemblies.ShowLandingGearCycles Then mLog.LogAFAssemblies(i).LandingGearCycles = Trim(txtAirframeLandingGearCycles.Text)
				If mLog.LogAFAssemblies.ShowOverSpeedLHMLGCycles Then mLog.LogAFAssemblies(i).OverSpeedLHMLGCycles = Trim(txtAirframeOverSpeedLHMLGCycles.Text)
				If mLog.LogAFAssemblies.ShowOverSpeedRHMLGCycles Then mLog.LogAFAssemblies(i).OverSpeedRHMLGCycles = Trim(txtAirframeOverSpeedRHMLGCycles.Text)
				If mLog.LogAFAssemblies.ShowOverSpeedNLGCycles Then mLog.LogAFAssemblies(i).OverSpeedNLGCycles = Trim(txtAirframeOverSpeedNLGCycles.Text)
				If mLog.LogAFAssemblies.ShowMGBTorqueCycles Then mLog.LogAFAssemblies(i).MGBTorqueCycles = Trim(txtAirframeMGBTorqueCycles.Text)
				If mLog.LogAFAssemblies.ShowRotorBrakeCycles Then mLog.LogAFAssemblies(i).RotorBrakeCycles = Trim(txtAirframeRotorBrakeCycles.Text)


				If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mLog.LogAFAssemblies(i).Cycles)
				If mLog.LogAFAssemblies.ShowNGCycles Then mLog.UpdateChildPeriods(4, "NgCycles", mLog.LogAFAssemblies(i).NGCycles)
				If mLog.LogAFAssemblies.ShowNFCycles Then mLog.UpdateChildPeriods(5, "NfCycles", mLog.LogAFAssemblies(i).NFCycles)
				If mLog.LogAFAssemblies.ShowRINS Then mLog.UpdateChildPeriods(6, "RINS", mLog.LogAFAssemblies(i).RINS)
				If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mLog.LogAFAssemblies(i).Landings)
				If mLog.LogAFAssemblies.ShowStarts Then mLog.UpdateChildPeriods(8, "Starts", mLog.LogAFAssemblies(i).Starts)
				If mLog.LogAFAssemblies.ShowBleeds Then mLog.UpdateChildPeriods(11, "Bleeds", mLog.LogAFAssemblies(i).Bleeds)
				If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.UpdateChildPeriods(12, "ImpellerCycles", mLog.LogAFAssemblies(i).ImpellerCycles)
				If mLog.LogAFAssemblies.ShowCTCycles Then mLog.UpdateChildPeriods(13, "CTCycles", mLog.LogAFAssemblies(i).CTCycles)
				If mLog.LogAFAssemblies.ShowPTCycles Then mLog.UpdateChildPeriods(14, "PTCycles", mLog.LogAFAssemblies(i).PTCycles)
				If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.UpdateChildPeriods(15, "GeneratorMods", mLog.LogAFAssemblies(i).GeneratorMods)

				If mLog.LogAFAssemblies.ShowNRCycles Then mLog.UpdateChildPeriods(17, "NRCycles", mLog.LogAFAssemblies(i).NRCycles)
				If mLog.LogAFAssemblies.ShowLandingCycles Then mLog.UpdateChildPeriods(18, "LandingCycles", mLog.LogAFAssemblies(i).LandingCycles)
				If mLog.LogAFAssemblies.ShowLandingGearCycles Then mLog.UpdateChildPeriods(19, "LandingGearCycles", mLog.LogAFAssemblies(i).LandingGearCycles)
				If mLog.LogAFAssemblies.ShowOverSpeedLHMLGCycles Then mLog.UpdateChildPeriods(20, "OverSpeedLHMLGCycles", mLog.LogAFAssemblies(i).OverSpeedLHMLGCycles)
				If mLog.LogAFAssemblies.ShowOverSpeedRHMLGCycles Then mLog.UpdateChildPeriods(21, "OverSpeedRHMLGCycles", mLog.LogAFAssemblies(i).OverSpeedRHMLGCycles)
				If mLog.LogAFAssemblies.ShowOverSpeedNLGCycles Then mLog.UpdateChildPeriods(22, "OverSpeedNLGCycles", mLog.LogAFAssemblies(i).OverSpeedNLGCycles)
				If mLog.LogAFAssemblies.ShowMGBTorqueCycles Then mLog.UpdateChildPeriods(23, "ShowMGBTorqueCycles", mLog.LogAFAssemblies(i).ShowMGBTorqueCycles)
				If mLog.LogAFAssemblies.ShowRotorBrakeCycles Then mLog.UpdateChildPeriods(24, "RotorBrakeCycles", mLog.LogAFAssemblies(i).RotorBrakeCycles)

			Next i

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Change by Deven 21-03-2008
	Public Sub SetEngineGridObject(Optional isFromDataBindGrid As Boolean = False)        ' For Second Grid i.e ENGINE

		Dim txtEngineHours,
			txtEngineLandings,
			txtEngineCycles,
			txtEngineStarts,
			txtEngineNGCycles,
			txtEngineNFCycles,
			txtEngineRins,
			txtEngineCFactors,
			txtEngineBleeds,
			txtEngineImpellerCycles,
			txtEngineCTCycles,
			txtEnginePTCycles,
			txtEngineGeneratorMods,
			txtEngineRapidTakeOffFactor As TextBox

		Dim txtEngineN1Cycles,
			txtEngineN2Cycles As TextBox

		Try

			For i As Integer = 0 To gvEnginePeriods.Rows.Count - 1

				txtEngineHours = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineHours"), TextBox)
				txtEngineLandings = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineLandings"), TextBox)
				txtEngineCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineCycles"), TextBox)
				txtEngineStarts = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineStarts"), TextBox)
				txtEngineNGCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineNGCycles"), TextBox)
				txtEngineNFCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineNFCycles"), TextBox)
				txtEngineRins = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineRins"), TextBox)
				txtEngineCFactors = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineCFactors"), TextBox)
				txtEngineBleeds = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineBleeds"), TextBox)
				txtEngineImpellerCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineImpellerCycles"), TextBox)
				txtEngineCTCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineCTCycles"), TextBox)
				txtEnginePTCycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEnginePTCycles"), TextBox)
				txtEngineGeneratorMods = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012
				txtEngineRapidTakeOffFactor = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineRapidTakeOffFactor"), TextBox) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee

				txtEngineN1Cycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineN1Cycles"), TextBox)
				txtEngineN2Cycles = CType(Me.gvEnginePeriods.Rows(i).FindControl("txtEngineN2Cycles"), TextBox)

				If isFromDataBindGrid Then If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtEngineHours.Text)

				If mLog.LogEngAssemblies(i).ShowLandings Then mLog.LogEngAssemblies(i).Landings = Trim(txtEngineLandings.Text)
				If mLog.LogEngAssemblies(i).ShowCycles Then mLog.LogEngAssemblies(i).Cycles = Trim(txtEngineCycles.Text)
				If mLog.LogEngAssemblies(i).ShowStarts Then mLog.LogEngAssemblies(i).Starts = Trim(txtEngineStarts.Text)
				If mLog.LogEngAssemblies(i).ShowNGCycles Then mLog.LogEngAssemblies(i).NGCycles = Trim(txtEngineNGCycles.Text)
				If mLog.LogEngAssemblies(i).ShowNFCycles Then mLog.LogEngAssemblies(i).NFCycles = Trim(txtEngineNFCycles.Text)
				If mLog.LogEngAssemblies(i).ShowRINS Then mLog.LogEngAssemblies(i).RINS = Trim(txtEngineRins.Text)
				If mLog.LogEngAssemblies(i).ShowCFactors Then mLog.LogEngAssemblies(i).CFactor = Trim(txtEngineCFactors.Text)
				If mLog.LogEngAssemblies(i).ShowBleeds Then mLog.LogEngAssemblies(i).Bleeds = Trim(txtEngineBleeds.Text)
				If mLog.LogEngAssemblies(i).ShowImpellerCycles Then mLog.LogEngAssemblies(i).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
				If mLog.LogEngAssemblies(i).ShowCTCycles Then mLog.LogEngAssemblies(i).CTCycles = Trim(txtEngineCTCycles.Text)
				If mLog.LogEngAssemblies(i).ShowPTCycles Then mLog.LogEngAssemblies(i).PTCycles = Trim(txtEnginePTCycles.Text)
				If mLog.LogEngAssemblies(i).ShowGeneratorMods Then mLog.LogEngAssemblies(i).GeneratorMods = Trim(txtEngineGeneratorMods.Text) 'Added by Shweta on 7-May-2012  for ALL02052012
				If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then mLog.LogEngAssemblies(i).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee

				If mLog.LogEngAssemblies.ShowN1Cycles Then mLog.LogEngAssemblies(i).N1Cycles = Trim(txtEngineN1Cycles.Text)
				If mLog.LogEngAssemblies.ShowN2Cycles Then mLog.LogEngAssemblies(i).N2Cycles = Trim(txtEngineN2Cycles.Text)

			Next i

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Change by Deven 21-03-2008
	Public Sub SetAPUGridObject(Optional isFromDataBindGrid As Boolean = False)        ' For Third Grid i.e APU

		Dim txtAPUHours,
			txtAPULandings,
			txtAPUCycles,
			txtAPUStarts,
			txtAPUNGCycles,
			txtAPUNFCycles,
			txtAPURins,
			txtAPUBleeds,
			txtAPUImpellerCycles,
			txtAPUCTCycles,
			txtAPUPTCycles,
			txtAPUGeneratorMods As TextBox

		Try

			For i As Integer = 0 To Me.gvAPUPeriods.Rows.Count - 1

				txtAPUHours = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUHours"), TextBox)
				txtAPULandings = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPULandings"), TextBox)
				txtAPUCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUCycles"), TextBox)
				txtAPUStarts = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUStarts"), TextBox)
				txtAPUNGCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUNGCycles"), TextBox)
				txtAPUNFCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUNFCycles"), TextBox)
				txtAPURins = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPURins"), TextBox)
				txtAPUBleeds = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUBleeds"), TextBox)
				txtAPUImpellerCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUImpellerCycles"), TextBox)
				txtAPUCTCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUCTCycles"), TextBox)
				txtAPUPTCycles = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUPTCycles"), TextBox)
				txtAPUGeneratorMods = CType(Me.gvAPUPeriods.Rows(i).FindControl("txtAPUGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012

				If isFromDataBindGrid Then If mLog.LogAPUAssemblies(i).ShowHours Then mLog.LogAPUAssemblies.Item(i).Hours = Trim(txtAPUHours.Text)

				If mLog.LogAPUAssemblies(i).ShowLandings Then mLog.LogAPUAssemblies.Item(i).Landings = Trim(txtAPULandings.Text)
				If mLog.LogAPUAssemblies(i).ShowCycles Then mLog.LogAPUAssemblies.Item(i).Cycles = Trim(txtAPUCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowStarts Then mLog.LogAPUAssemblies.Item(i).Starts = Trim(txtAPUStarts.Text)
				If mLog.LogAPUAssemblies(i).ShowNGCycles Then mLog.LogAPUAssemblies.Item(i).NGCycles = Trim(txtAPUNGCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowNFCycles Then mLog.LogAPUAssemblies.Item(i).NFCycles = Trim(txtAPUNFCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowRINS Then mLog.LogAPUAssemblies.Item(i).RINS = Trim(txtAPURins.Text)
				If mLog.LogAPUAssemblies(i).ShowBleeds Then mLog.LogAPUAssemblies.Item(i).Bleeds = Trim(txtAPUBleeds.Text)
				If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then mLog.LogAPUAssemblies.Item(i).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowCTCycles Then mLog.LogAPUAssemblies.Item(i).CTCycles = Trim(txtAPUCTCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowPTCycles Then mLog.LogAPUAssemblies.Item(i).PTCycles = Trim(txtAPUPTCycles.Text)
				If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then mLog.LogAPUAssemblies.Item(i).GeneratorMods = Trim(txtAPUGeneratorMods.Text) 'Added by Shweta on 7-May-2012

			Next i

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Added By Prashant 23-Oct-2009
	Public Sub SetCGBGridObject(Optional isFromDataBindGrid As Boolean = False)         'For 4th Grid i.e CGB

		Dim txtCGBHours,
			txtCGBLandings,
			txtCGBCycles,
			txtCGBStarts,
			txtCGBNGCycles,
			txtCGBNFCycles,
			txtCGBRins,
			txtCGBBleeds,
			txtCGBImpellerCycles,
			txtCGBCTCycles,
			txtCGBPTCycles,
			txtCGBGeneratorMods As TextBox

		Try

			For i As Integer = 0 To Me.gvCGBPeriods.Rows.Count - 1

				txtCGBHours = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBHours"), TextBox)
				txtCGBLandings = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBLandings"), TextBox)
				txtCGBCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBCycles"), TextBox)
				txtCGBStarts = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBStarts"), TextBox)
				txtCGBNGCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBNGCycles"), TextBox)
				txtCGBNFCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBNFCycles"), TextBox)
				txtCGBRins = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBRins"), TextBox)
				txtCGBBleeds = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBBleeds"), TextBox)
				txtCGBImpellerCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBImpellerCycles"), TextBox)
				txtCGBCTCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBCTCycles"), TextBox)
				txtCGBPTCycles = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBPTCycles"), TextBox)
				txtCGBGeneratorMods = CType(Me.gvCGBPeriods.Rows(i).FindControl("txtCGBGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012

				If isFromDataBindGrid Then If mLog.LogCGBAssemblies(i).ShowHours Then mLog.LogCGBAssemblies.Item(i).Hours = Trim(txtCGBHours.Text)

				If mLog.LogCGBAssemblies(i).ShowLandings Then mLog.LogCGBAssemblies.Item(i).Landings = Trim(txtCGBLandings.Text)
				If mLog.LogCGBAssemblies(i).ShowCycles Then mLog.LogCGBAssemblies.Item(i).Cycles = Trim(txtCGBCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowStarts Then mLog.LogCGBAssemblies.Item(i).Starts = Trim(txtCGBStarts.Text)
				If mLog.LogCGBAssemblies(i).ShowNGCycles Then mLog.LogCGBAssemblies.Item(i).NGCycles = Trim(txtCGBNGCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowNFCycles Then mLog.LogCGBAssemblies.Item(i).NFCycles = Trim(txtCGBNFCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowRINS Then mLog.LogCGBAssemblies.Item(i).RINS = Trim(txtCGBRins.Text)
				If mLog.LogCGBAssemblies(i).ShowBleeds Then mLog.LogCGBAssemblies.Item(i).Bleeds = Trim(txtCGBBleeds.Text)
				If mLog.LogCGBAssemblies(i).ShowImpellerCycles Then mLog.LogCGBAssemblies.Item(i).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowCTCycles Then mLog.LogCGBAssemblies.Item(i).CTCycles = Trim(txtCGBCTCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowPTCycles Then mLog.LogCGBAssemblies.Item(i).PTCycles = Trim(txtCGBPTCycles.Text)
				If mLog.LogCGBAssemblies(i).ShowGeneratorMods Then mLog.LogCGBAssemblies.Item(i).GeneratorMods = Trim(txtCGBGeneratorMods.Text) 'Added by Shweta on 7-May-2012 for ALL02052012

			Next i

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'--------------------------------

	Private Function Save() As Boolean

		Try

			'Authentication
			If mLog.Date IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = 0
					mDays = mCheck.Number("Days")

					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

					'CNDC
					If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < -10 _
						Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < -10) _
						Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < -10) Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										" Your subscription has been expired. can not save Log. <br> Log / Departure / Arrival Date can not be greater by 10 Days or more than - <br>" &
													maxAllowableDate.ToString(WebDateFormat),
										MsgBoxStyle.OkOnly,
										"")

						DataFieldBind()
						Exit Function

					End If

				End If

			End If

			If mLog.IsLogAirborneEntry Then 'Added by Saylee on 1-Sep-2021 for ALL01092021 :
				mLog = Session("mLog")
			End If

			'Authentication
			Dim LogClone As Log
			LogClone = CType(mLog.Clone, Log)

			SetObject()

			If mLog.IsLogAirborneEntry Then 'Added by Saylee on 1-Sep-2021 for ALL01092021 
				SetAirFrameGridObject(True)
			Else
				SetAirFrameGridObject()
			End If

			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			If mLog.IsValid = True Then

				Try

					If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013

						'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
						If mLog.LogAFAssemblies.AssemblyRemoved Or
						   mLog.LogEngAssemblies.AssemblyRemoved Or
						   mLog.PropLogAssemblies.AssemblyRemoved Or
						   mLog.LogAPUAssemblies.AssemblyRemoved Or
						   mLog.LogCGBAssemblies.AssemblyRemoved Or
						   mLog.LogNGBAssemblies.AssemblyRemoved Or
						   mLog.LogGEAssemblies.AssemblyRemoved Or
						   mLog.LogMRHAssemblies.AssemblyRemoved Or
						   mLog.LogSPSAssemblies.AssemblyRemoved Or
						   mLog.LogSSAAssemblies.AssemblyRemoved Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.Restriction,
											MSGBox.Message_Text.Restriction,
											"Required Assembly of the Aircraft is Not Installed on this Date of Log.",
											MsgBoxStyle.OkOnly,
											"")

							Return False

							Exit Function

						End If

					End If

					If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										"There is some information missing / not entered correctly.</br> </br> Do you still want to Save Log? ",
										MsgBoxStyle.YesNo,
										"SaveLogAfterHrsSame")

						Exit Function

					End If

					'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
					If AvgFlightTimeDeviation() = True And
					   Not (AppSettings("ClientCode") = "Heligo" Or
							AppSettings("ClientCode") = "UHPL" Or
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP") Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										"Airborne Time of this flight is " &
													IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True",
														"Greater",
														"less") &
													" than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ",
										MsgBoxStyle.YesNo,
										"SaveLogAfterAvgFlightTimeDeviationWarning")

						Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
						Exit Function

					End If
					'End

					mLog.ApplyEdit()

					'Pilot 1
					'IF Pilot 1 deleted...
					If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID1
							LogCrew.DutyAsID = 1

							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1

								mLog.LogCrews.Add(LogCrew)

							End If

						Else

							If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1

								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
							End If

						End If

					End If

					'Pilot 2
					'IF Pilot 2 deleted...
					If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID2
							LogCrew.DutyAsID = 2

							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2

								mLog.LogCrews.Add(LogCrew)

							End If

						Else

							If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2

								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
							End If

						End If

					End If
					'End 

					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim IsNewLog As Boolean
					IsNewLog = mLog.IsNew
					'End

					mLog = CType(mLog.Save(), Log)

					SaveAttachment()

					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim mMaxLogOfAircraft As MaxLogOfAircraft
					mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

					If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

						If Not (AppSettings("ClientCode") = "Heligo" Or
								AppSettings("ClientCode") = "UHPL" Or
								AppSettings("ClientCode") = "APFT" Or
								AppSettings("ClientCode") = "AAP") Then

							If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						Else

							If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						End If

					End If
					'End

					Dim IsShowDateControl As String
					If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
						IsShowDateControl = "False"
						Session("IsSaveAndNew") = 0
					Else
						IsShowDateControl = "True"
					End If

					mLog = Log.GetLog(mLog.ID)
					mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
					mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
					mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
					mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted

					MarkLog(Action.Save,
							"Flight Log",
							mLogDetail,
							ErrorType.HandledError,
							mLog.ID,
							EventLogID)

					Session("mLog") = mLog
					upnlTabs.Update()
					upnlTabsNew.Update()

					Return True

				Catch ex As SqlException

					Session("LogClone") = LogClone

					If ex.Number = 8114 Or ex.Number = 8115 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
										MSGBox.Message_Text.NumericOverFlow,
										" Rate or Qty or Conversion Factor. ",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then

							MSGBoxCtrl.Show("Alert!",
											"Save Alert ! ",
											"<strong> Please enter the unique Log Page No. </strong> ",
											MsgBoxStyle.OkOnly,
											"")
						Else

							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.Duplicate,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")
						End If

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 50000 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.LogExist,
										MSGBox.Message_Text.Alert,
										"Log already entered between current Date and Time span for this Aircraft.",
										MsgBoxStyle.OkOnly,
										"")
					End If

					'Added by Utkarsh on 1-oct-2013 for log_ajax changes
					mLog = LogClone
					Session("mLog") = mLog 'end

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

	Private Function SaveLogFlexiLog() As Boolean 'Added by Saylee on 21-May-2012 ALL17052012

		Try

			'Authentication
			If mLog.Date IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = 0
					mDays = mCheck.Number("Days")
					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

					'CNDC
					If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
						Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
						Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										" Your subscription has been expired. can not save Log. <br> Log / Departure / Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat),
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
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			If mLog.IsValid = True Then

				Try

					If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   

						If mLog.LogAFAssemblies.AssemblyRemoved Or
						   mLog.LogEngAssemblies.AssemblyRemoved Or
						   mLog.PropLogAssemblies.AssemblyRemoved Or
						   mLog.LogAPUAssemblies.AssemblyRemoved Or
						   mLog.LogCGBAssemblies.AssemblyRemoved Or
						   mLog.LogNGBAssemblies.AssemblyRemoved Or
						   mLog.LogGEAssemblies.AssemblyRemoved Or
						   mLog.LogMRHAssemblies.AssemblyRemoved Or
						   mLog.LogSPSAssemblies.AssemblyRemoved Or
						   mLog.LogSSAAssemblies.AssemblyRemoved Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.Restriction,
											MSGBox.Message_Text.Restriction,
											"Required Assembly of the Aircraft is Not Installed on this Date of Log.",
											MsgBoxStyle.OkOnly,
											"")

							Return False

							Exit Function

						End If

					End If

					'Added By Prashant 12-Apr-2010
					Dim IsMELCount As Boolean = False
					Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
					mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)

					For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1

						If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then   'Added By Prashant 23-Sep-2010

							If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
								IsMELCount = True
								Exit For
							Else
								IsMELCount = False
							End If

						End If

					Next

					mTempMELSnagCorrectiveActionList = Nothing

					If IsMELCount = True Then

						MSGBoxCtrl.Show("Minimum Equipment Level",
										"Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ",
										"",
										MsgBoxStyle.YesNo,
										IIf(Session("New") = "True", "MELNew", "MEL"))

						Exit Function

					ElseIf IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

						MSGBoxCtrl.Show("Alert",
										"There is some Information missing / not entered correctly.<br> <br> Do you still want to Save Log? ",
										"",
										MsgBoxStyle.YesNo,
										"SaveLogAfterHrsSame")

						Exit Function

					ElseIf AvgFlightTimeDeviation() = True And
						   Not (AppSettings("ClientCode") = "Heligo" Or
								AppSettings("ClientCode") = "UHPL" Or
								AppSettings("ClientCode") = "APFT" Or
								AppSettings("ClientCode") = "AAP") Then 'Added By Vikrant On 30-Nov-2016 For ALL30112016-1

						MSGBoxCtrl.Show("Alert",
										$"Airborne Time of this flight is {IIf(CBool(Session("IsFlightTimeGreaterThanAvgFlightTime")), "Greater", "less")} than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ",
										"",
										MsgBoxStyle.YesNo,
										"SaveLogAfterAvgFlightTimeDeviationWarning")

						Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
						Exit Function

					End If
					'End

					mLog.ApplyEdit()
					'Add Pilot and Co-pilot in Log Crew as Child...

					'IF Pilot 1 deleted...
					If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID1
							LogCrew.DutyAsID = 1
							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1
								mLog.LogCrews.Add(LogCrew)

							End If

						Else


							If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1
								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
							End If

						End If

					End If

					'Pilot 2
					'IF Pilot 2 deleted...
					If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID2
							LogCrew.DutyAsID = 2
							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2
								mLog.LogCrews.Add(LogCrew)

							End If

						Else

							If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2
								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
							End If

						End If

					End If


					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim IsNewLog As Boolean
					IsNewLog = mLog.IsNew
					'End

					mLog = CType(mLog.Save(), Log)
					SaveAttachment()

					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim mMaxLogOfAircraft As MaxLogOfAircraft
					mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

					If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

						If Not (AppSettings("ClientCode") = "Heligo" Or
								AppSettings("ClientCode") = "UHPL" Or
								AppSettings("ClientCode") = "APFT" Or
								AppSettings("ClientCode") = "AAP") Then

							If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						Else

							If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						End If

					End If
					'End

					Dim IsShowDateCntrl As String
					If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
						IsShowDateCntrl = "False"
						Session("IsSaveAndNew") = 0
					Else
						IsShowDateCntrl = "True"
					End If

					mLog = Log.GetLog(mLog.ID)
					mLog.IsUTC = mMachine.IsUTC
					mLog.IsTLP = mMachine.IsTLP
					mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
					mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted

					MarkLog(Action.Save,
							"Flight Log",
							mLogDetail,
							ErrorType.HandledError,
							mLog.ID,
							EventLogID)

					Session("mLog") = mLog
					upnlTabs.Update()
					upnlTabsNew.Update()

					Return True

				Catch ex As SqlException

					Session("LogClone") = LogClone
					If ex.Number = 8114 Or ex.Number = 8115 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
										MSGBox.Message_Text.NumericOverFlow,
										" Rate or Qty or Conversion Factor. ",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then

							MSGBoxCtrl.Show("Alert!",
											"Save Alert ! ",
											"<strong> Please enter the unique Log Page No. </strong> ",
											MsgBoxStyle.OkOnly,
											"")
						Else

							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.Duplicate,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")
						End If

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 50000 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.LogExist,
										MSGBox.Message_Text.Alert,
										"Log already entered between current Date and Time span for this Aircraft.",
										MsgBoxStyle.OkOnly,
										"")
					End If

					'Added by Utkarsh on 1-oct-2013 for log_ajax changes
					mLog = LogClone
					Session("mLog") = mLog

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

	Private Function SaveLogAfterHrsSame() As Boolean

		Try

			'Authentication
			If mLog.Date IsNot DBNull.Value Then

				Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))

				If mCheck.WebAuthentication = True Then

					Dim mDays As Integer = 0
					mDays = mCheck.Number("Days")
					Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)

					'CNDC
					If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 Or
					   (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) Or
					   (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.saveAlert,
										" Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat),
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

			If mLog.IsLogAirborneEntry Then 'Added by Saylee on 1-Sep-2021 for ALL01092021 
				SetAirFrameGridObject(True)
			Else
				SetAirFrameGridObject()
			End If

			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			If mLog.IsValid = True Then

				Try

					If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   

						If mLog.LogAFAssemblies.AssemblyRemoved Or
						   mLog.LogEngAssemblies.AssemblyRemoved Or
						   mLog.PropLogAssemblies.AssemblyRemoved Or
						   mLog.LogAPUAssemblies.AssemblyRemoved Or
						   mLog.LogCGBAssemblies.AssemblyRemoved Or
						   mLog.LogNGBAssemblies.AssemblyRemoved Or
						   mLog.LogGEAssemblies.AssemblyRemoved Or
						   mLog.LogMRHAssemblies.AssemblyRemoved Or
						   mLog.LogSPSAssemblies.AssemblyRemoved Or
						   mLog.LogSSAAssemblies.AssemblyRemoved Then

							MSGBoxCtrl.Show(MSGBox.Message_Title.Restriction,
											MSGBox.Message_Text.Restriction,
											"Required Assembly of the Aircraft is Not Installed on this Date of Log.",
											MsgBoxStyle.OkOnly,
											"")

							Return False
							Exit Function

						End If

					End If

					'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
					If AvgFlightTimeDeviation() = True And
					   Not (AppSettings("ClientCode") = "Heligo" Or
							AppSettings("ClientCode") = "UHPL" Or
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP") Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										$"Airborne Time of this flight is {IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less")} than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ",
										MsgBoxStyle.YesNo,
										"SaveLogAfterAvgFlightTimeDeviationWarning")

						Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
						Return False
						Exit Function

					End If
					'End

					mLog.ApplyEdit()

					'Pilot 1
					'IF Pilot 1 deleted...
					If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID1
							LogCrew.DutyAsID = 1
							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

						If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1
								mLog.LogCrews.Add(LogCrew)

							End If

						Else

							If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID1
								LogCrew.DutyAsID = 1
								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
							End If

						End If

					End If

					'Pilot 2
					'IF Pilot 2 deleted...
					If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
							mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID2
							LogCrew.DutyAsID = 2
							mLog.LogCrews.Add(LogCrew)

						End If

					ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

						If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

							If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2
								mLog.LogCrews.Add(LogCrew)

							End If

						Else

							If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then

								Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
								LogCrew.CrewID = mLog.PilotID2
								LogCrew.DutyAsID = 2
								mLog.LogCrews.Add(LogCrew)

							Else
								mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
							End If

						End If

					End If

					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim IsNewLog As Boolean
					IsNewLog = mLog.IsNew
					'End

					mLog = CType(mLog.Save(), Log)

					SaveAttachment()

					'Added By Vikrant on 01-Dec-2021 for PBH
					Dim mMaxLogOfAircraft As MaxLogOfAircraft
					mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

					If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

						If Not (AppSettings("ClientCode") = "Heligo" Or
								AppSettings("ClientCode") = "UHPL" Or
								AppSettings("ClientCode") = "APFT" Or
								AppSettings("ClientCode") = "AAP") Then

							If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						Else

							If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
								SetPBHValues(mLog, IsNewLog)
							End If

						End If

					End If
					'End

					Dim IsShowDateCntrl As String
					If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
						IsShowDateCntrl = "False"
						Session("IsSaveAndNew") = 0
					Else
						IsShowDateCntrl = "True"
					End If

					mLog = Log.GetLog(mLog.ID)
					mLog.IsUTC = mMachine.IsUTC
					mLog.IsTLP = mMachine.IsTLP
					mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
					mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted

					MarkLog(Action.Save,
							"Flight Log",
							mLogDetail,
							ErrorType.HandledError,
							mLog.ID,
							EventLogID)

					Session("mLog") = mLog
					upnlTabs.Update()
					upnlTabsNew.Update()

					Return True

				Catch ex As SqlException

					Session("LogClone") = LogClone

					If ex.Number = 8114 Or ex.Number = 8115 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
										MSGBox.Message_Text.NumericOverFlow,
										" Rate or Qty or Conversion Factor. ",
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 8145 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 2627 Then

						If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then

							MSGBoxCtrl.Show("Alert!",
											"Save Alert ! ",
											"<strong> Please enter the unique Log Page No. </strong> ",
											MsgBoxStyle.OkOnly,
											"")

						Else

							MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.Duplicate,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")

						End If

					ElseIf ex.Number = 547 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

					ElseIf ex.Number = 50000 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.LogExist,
										MSGBox.Message_Text.Alert,
										"Log already entered between current Date and Time span for this Aircraft.",
										MsgBoxStyle.OkOnly,
										"")

					End If

					Return False

				Finally

					'Added by Utkarsh on 1-oct-2013 for log_ajax changes
					mLog = LogClone
					Session("mLog") = mLog
					LogClone = Nothing

				End Try

			Else
				Return False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

	Private Function SaveLogAfterAvgFlightTimeDeviationWarning() As Boolean

		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)

		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)

		If mLog.IsValid = True Then

			Try

				mLog.ApplyEdit()

				'Pilot 1
				'IF Pilot 1 deleted...
				If mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

					If mLog.LogCrews.Contains(mLog.PrevPilotID1) Then
						mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID1, ""))
					End If

				ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And mLog.PrevPilotID1.Equals(Guid.Empty) Then

					If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID1
						LogCrew.DutyAsID = 1

						mLog.LogCrews.Add(LogCrew)

					End If

				ElseIf Not mLog.PilotID1.Equals(Guid.Empty) And Not mLog.PrevPilotID1.Equals(Guid.Empty) Then

					If mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID1) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID1
							LogCrew.DutyAsID = 1

							mLog.LogCrews.Add(LogCrew)

						End If

					Else

						If Not mLog.LogCrews.Contains(mLog.PrevPilotID1) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID1
							LogCrew.DutyAsID = 1

							mLog.LogCrews.Add(LogCrew)

						Else
							mLog.LogCrews(mLog.PrevPilotID1, "").CrewID = mLog.PilotID1
						End If

					End If

				End If

				'Pilot 2
				'IF Pilot 2 deleted...
				If mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

					If mLog.LogCrews.Contains(mLog.PrevPilotID2) Then
						mLog.LogCrews.Remove(mLog.LogCrews(mLog.PrevPilotID2, ""))
					End If

				ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And mLog.PrevPilotID2.Equals(Guid.Empty) Then

					If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

						Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
						LogCrew.CrewID = mLog.PilotID2
						LogCrew.DutyAsID = 2

						mLog.LogCrews.Add(LogCrew)

					End If

				ElseIf Not mLog.PilotID2.Equals(Guid.Empty) And Not mLog.PrevPilotID2.Equals(Guid.Empty) Then

					If mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

						If Not mLog.LogCrews.Contains(mLog.PilotID2) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID2
							LogCrew.DutyAsID = 2

							mLog.LogCrews.Add(LogCrew)

						End If

					Else

						If Not mLog.LogCrews.Contains(mLog.PrevPilotID2) Then

							Dim LogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
							LogCrew.CrewID = mLog.PilotID2
							LogCrew.DutyAsID = 2

							mLog.LogCrews.Add(LogCrew)

						Else
							mLog.LogCrews(mLog.PrevPilotID2, "").CrewID = mLog.PilotID2
						End If

					End If

				End If

				'Added By Vikrant on 01-Dec-2021 for PBH
				Dim IsNewLog As Boolean
				IsNewLog = mLog.IsNew
				'End
				mLog = CType(mLog.Save(), Log)
				SaveAttachment()

				'Added By Vikrant on 01-Dec-2021 for PBH
				Dim mMaxLogOfAircraft As MaxLogOfAircraft
				mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

				If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

					If Not (AppSettings("ClientCode") = "Heligo" Or
							AppSettings("ClientCode") = "UHPL" Or
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP") Then

						If Not (CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime)) Then 'Last Log
							SetPBHValues(mLog, IsNewLog)
						End If

					Else

						If Not (CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate)) Then 'Last Log
							SetPBHValues(mLog, IsNewLog)
						End If

					End If

				End If
				'End

				Dim IsShowDateCntrl As String
				If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
					IsShowDateCntrl = "False"
					Session("IsSaveAndNew") = 0
				Else
					IsShowDateCntrl = "True"
				End If

				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC
				mLog.IsTLP = mMachine.IsTLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Action.Save, "Flight Log", mLogDetail, ErrorType.HandledError, mLog.ID, EventLogID)

				Session("mLog") = mLog
				upnlTabs.Update()
				upnlTabsNew.Update()

				Return True

			Catch ex As SqlException

				Session("LogClone") = LogClone

				If ex.Number = 8114 Or ex.Number = 8115 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.NumericOverFlow,
										MSGBox.Message_Text.NumericOverFlow,
										" Rate or Qty or Conversion Factor. ",
										MsgBoxStyle.OkOnly,
										"")

				ElseIf ex.Number = 8145 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
										MSGBox.Message_Text.ProcedureError,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

				ElseIf ex.Number = 2627 Then

					If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then

						MSGBoxCtrl.Show("Alert!",
											"Save Alert ! ",
											"<strong> Please enter the unique Log Page No. </strong> ",
											MsgBoxStyle.OkOnly,
											"")
					Else

						MSGBoxCtrl.Show(MSGBox.Message_Title.DataBaseError,
											MSGBox.Message_Text.Duplicate,
											ex.Procedure,
											MsgBoxStyle.OkOnly,
											"")
					End If

				ElseIf ex.Number = 547 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
										MSGBox.Message_Text.ReferenceDelete,
										ex.Procedure,
										MsgBoxStyle.OkOnly,
										"")

				ElseIf ex.Number = 50000 Then

					MSGBoxCtrl.Show(MSGBox.Message_Title.LogExist,
										MSGBox.Message_Text.Alert,
										"Log already entered between current Date and Time span for this Aircraft.",
										MsgBoxStyle.OkOnly,
										"")
				End If

				'Added by Utkarsh on 1-oct-2013 for log_ajax changes
				mLog = LogClone
				Session("mLog") = mLog 'end

				Return False

			Finally
				LogClone = Nothing
			End Try

		Else
			Return False
		End If

	End Function

	Private Sub MessageBoxResult()

		Try

			Dim Result As MsgBoxResult
			Result = MSGBoxCtrl.Result

			If Result > 0 Then

				Select Case Result
					Case MsgBoxResult.Yes

						If MSGBoxCtrl.Sender = "SaveNew" Then

							mLog = Session("mLog")
							DataFieldBind()
							DataBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If Save() = True Then

									SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
									NewRecord()
									Session.Remove("FileAttach")
									Session.Remove("IsAttachmentDeleted")
									Session("mLog") = mLog

									'Added By Vikrant on 01-Dec-2021 for PBH
									If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

										If Session("IsAircraftMadeNotInUse") = "True" Then

											Session.Remove("AircraftId")
											Session.Remove("IsAircraftMadeNotInUse")

											MSGBoxCtrl.Show("Alert!",
															"",
															"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
															MsgBoxStyle.OkOnly,
															"AircraftMadeNotInUse")

											Exit Sub

										End If

									End If
									'End
									DataFieldBind()
									EnableDisableButton()
									ControlVisibility()
									ControlVisibilityForAttachment()
									DataBindGrid()
									SetTitle()

									upnlLogDetails.Update()
									upnlFlightDetails.Update()
									upnlFlightSummary.Update()
									upnlTabs.Update()
									upnlTabsNew.Update()

								End If

							Else
								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "Close" Then

							'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
							If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then

								Dim Title As String = "Save Alert !"
								Dim Message As String = ""

								If Not mLog.PilotID1.Equals(Guid.Empty) Then

									Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)

									If mEmployeeStatus(0).Information <> "" Then
										Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information.ToString.Replace("Resource", "")
									End If

								End If

								If Not mLog.PilotID2.Equals(Guid.Empty) Then

									Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)

									If mEmployeeStatus(0).Information <> "" Then
										Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information.ToString.Replace("Resource", "")
									End If

								End If

								If Message.Length > 0 Then

									DataFieldBind()
									Session("sender") = ""
									MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
													MSGBox.Message_Text.Custom,
													Message,
													MsgBoxStyle.OkOnly,
													"")

									Exit Sub

								End If

							End If
							'End

							Dim mMaxLogOfAircraft As MaxLogOfAircraft
							mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

							If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

								If Not (AppSettings("ClientCode") = "Heligo" Or
										AppSettings("ClientCode") = "UHPL" Or
										AppSettings("ClientCode") = "APFT" Or
										AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

									Dim MaxLogDateTime As String = ""

									If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
										MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted
									Else
										MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted
									End If

									mLog = Session("mLog")
									DataFieldBind()

									If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012

										Session("SaveNClose") = "SaveNClose"
										MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
														MSGBox.Message_Text.Alert,
														"You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
																	MaxLogDateTime &
																	"<BR> <BR>Do you want to continue?",
														MsgBoxStyle.YesNo,
														"SaveLogFlexiLog")

										Exit Sub

									End If

								Else

									mLog = Session("mLog")
									DataFieldBind()

									If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012

										Session("SaveNClose") = "SaveNClose"
										MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
														MSGBox.Message_Text.Alert,
														"You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
																	mMaxLogOfAircraft.LogDateFormatted &
																	"<BR> <BR>Do you want to continue?",
														MsgBoxStyle.YesNo,
														"SaveLogFlexiLog")

										Exit Sub

									End If

								End If

							End If

							'Added By Prashant 12-Apr-2010
							Dim IsMELCount As Boolean = False
							Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList

							mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)

							For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1

								If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010

									If (CDate(mLog.Date) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
										IsMELCount = True
										Exit For
									Else
										IsMELCount = False
									End If

								End If

							Next

							mTempMELSnagCorrectiveActionList = Nothing

							If IsMELCount = True Then

								MSGBoxCtrl.Show("Minimum Equipment Level",
												"Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?",
												"",
												MsgBoxStyle.YesNo,
												"MELClose")

								DataBind() 'Added By Utkarsh On 12-Sep-2011
								Exit Sub

							Else

								mLog = Session("mLog")
								DataFieldBind()

								If mLog.IsValid Then

									If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

									Session("SaveNClose") = "SaveNClose"

									If Save() = True Then

										mLog = Log.GetLog(mLog.ID)
										mLog.IsUTC = mMachine.IsUTC
										mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
										mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
										Session("mLog") = mLog
										Session.Remove("FileAttach")
										Session.Remove("IsAttachmentDeleted")
										SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then

												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")
												MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")

												Exit Sub

											End If

										End If
										'End
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									Else
										Exit Sub
									End If
								Else

									upnlErrorList.Update()
								End If

							End If

						ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

							mLog = Session("mLog")
							Session("IsValueZero") = "True"
							DataFieldBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If SaveLogAfterHrsSame() = True Then

									If Session("New") = "True" Then

										Session("New") = ""
										SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
										NewRecord()
										Session.Remove("FileAttach")
										Session.Remove("IsAttachmentDeleted")
										Session("mLog") = mLog

										DataFieldBind()
										EnableDisableButton()
										ControlVisibility()
										ControlVisibilityForAttachment()
										DataBindGrid()
										SetTitle()

										mLogListOnDate = LogList.GetLogList(mMachine.ID,
																			calDateTime.Text.ToString,
																			calDateTime.Text.ToString)

										Session("mLogListOnDate") = mLogListOnDate

										If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate

											ScriptManager.RegisterStartupScript(Me,
																				[GetType],
																				"ShowLastDet",
																				"ShowLastDet();",
																				True)

											upnlLogInfo.Update()

										End If

										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then
												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")
												MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
											End If

										End If
										'End

										upnlLogDetails.Update()
										upnlFlightDetails.Update()
										upnlFlightSummary.Update()
										upnlTabs.Update()
										upnlTabsNew.Update()

										MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
														MSGBox.Message_Text.SavedSuccessFully,
														"",
														MsgBoxStyle.OkOnly,
														"")

									Else

										mLog = Log.GetLog(mLog.ID)
										mLog.IsUTC = mMachine.IsUTC
										mLog.IsTLP = mMachine.IsTLP
										mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
										Session("mLog") = mLog
										SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
										SetTitle()
										DataFieldBind()
										EnableDisableButton()

										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then

												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")

												MSGBoxCtrl.Show("Alert!",
																"",
																"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
																MsgBoxStyle.OkOnly,
																"AircraftMadeNotInUse")

												Exit Sub

											End If

										End If

										If Session("SaveNClose") = "SaveNClose" Then

											Session("SaveNClose") = ""
											Session.Remove("SaveNClose")
											Session.Remove("FileAttach")
											Session.Remove("IsAttachmentDeleted")
											Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

										End If

										upnlTabsNew.Update()

										MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
														MSGBox.Message_Text.SavedSuccessFully,
														"",
														MsgBoxStyle.OkOnly,
														"")

									End If

								End If

							Else
								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "MELClose" Then

							mLog = Session("mLog")
							DataFieldBind()
							DataBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If Save() = True Then

									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog
									Session.Remove("FileAttach")
									Session.Remove("IsAttachmentDeleted")
									SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification

									'Added By Vikrant on 01-Dec-2021 for PBH
									If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

										If Session("IsAircraftMadeNotInUse") = "True" Then

											Session.Remove("AircraftId")
											Session.Remove("IsAircraftMadeNotInUse")
											MSGBoxCtrl.Show("Alert!",
															"",
															"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
															MsgBoxStyle.OkOnly,
															"AircraftMadeNotInUse")
											Exit Sub

										End If

									End If

									Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

								End If

							Else

								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "MEL" Then

							mLog = Session("mLog")
							DataFieldBind()
							DataBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If Save() = True Then

									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog

									SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification

									If Session("SaveNClose") = "SaveNClose" Then

										Session("SaveNClose") = ""
										Session.Remove("SaveNClose")
										Session.Remove("FileAttach")
										Session.Remove("IsAttachmentDeleted")

										SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification

										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then

												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")

												MSGBoxCtrl.Show("Alert!",
																"",
																"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
																MsgBoxStyle.OkOnly,
																"AircraftMadeNotInUse")

												Exit Sub

											End If

										End If

										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

									Else

										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then

												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")

												MSGBoxCtrl.Show("Alert!",
																"",
																"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
																MsgBoxStyle.OkOnly,
																"AircraftMadeNotInUse")
												Exit Sub

											End If

										End If
										'End

										DataFieldBind()
										EnableDisableButton()
										ControlVisibility()
										ControlVisibilityForAttachment()
										DataBindGrid()
										SetTitle()

										upnlLogDetails.Update()
										upnlFlightDetails.Update()
										upnlFlightSummary.Update()
										upnlTabs.Update()
										upnlTabsNew.Update()

										MSGBoxCtrl.Show(MSGBox.Message_Title.SavedSuccessFully,
														MSGBox.Message_Text.SavedSuccessFully,
														"",
														MsgBoxStyle.OkOnly,
														"")

									End If

								End If

							Else
								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then 'Added by Saylee on 21-May-2012 ALL17052012 to save Flexi log

							mLog = Session("mLog")
							DataFieldBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If SaveLogFlexiLog() = True Then

									If Session("New") = "True" Then

										Session("New") = ""
										NewRecord()
										Session.Remove("FileAttach")
										Session.Remove("IsAttachmentDeleted")
										Session("mLog") = mLog
										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then

												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")

												MSGBoxCtrl.Show("Alert!",
																"",
																"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
																MsgBoxStyle.OkOnly,
																"AircraftMadeNotInUse")

											End If

										End If
										'End

										DataFieldBind()
										EnableDisableButton()
										ControlVisibility()
										ControlVisibilityForAttachment()
										DataBindGrid()
										SetTitle()

										upnlLogDetails.Update()
										upnlFlightDetails.Update()
										upnlFlightSummary.Update()
										upnlTabs.Update()
										upnlTabsNew.Update()

										SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification

									Else

										mLog = Log.GetLog(mLog.ID)
										mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
										mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
										mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
										Session("mLog") = mLog

										SetTitle()
										DataFieldBind()
										EnableDisableButton()

										If Session("SaveNClose") = "SaveNClose" Then

											Session("SaveNClose") = ""
											Session.Remove("SaveNClose")
											Session.Remove("FileAttach")
											Session.Remove("IsAttachmentDeleted")
											Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

										End If

									End If

								End If

							Else

								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "MELNew" Then

							mLog = Session("mLog")
							DataFieldBind()
							DataBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								Session("New") = "True"

								If Save() = True Then

									SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification
									NewRecord()

									Session.Remove("FileAttach")
									Session.Remove("IsAttachmentDeleted")
									Session("mLog") = mLog

									DataFieldBind()
									EnableDisableButton()
									ControlVisibility()
									ControlVisibilityForAttachment()
									DataBindGrid()
									SetTitle()

									mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
									Session("mLogListOnDate") = mLogListOnDate

									If mLogListOnDate.Count > 0 And mLog.IsNew And
									   AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate

										ScriptManager.RegisterStartupScript(Me,
																			[GetType],
																			"ShowLastDet",
																			"ShowLastDet();",
																			True)
										upnlLogInfo.Update()

									End If

									'Added By Vikrant on 01-Dec-2021 for PBH
									If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

										If Session("IsAircraftMadeNotInUse") = "True" Then

											Session.Remove("AircraftId")
											Session.Remove("IsAircraftMadeNotInUse")

											MSGBoxCtrl.Show("Alert!",
															"",
															"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
															MsgBoxStyle.OkOnly,
															"AircraftMadeNotInUse")

											Exit Sub

										End If

									End If
									'End

									upnlLogDetails.Update()
									upnlFlightDetails.Update()
									upnlFlightSummary.Update()
									upnlTabs.Update()
									upnlTabsNew.Update()

								End If

							Else
								upnlErrorList.Update()
							End If

						ElseIf MSGBoxCtrl.Sender = "SaveLogAfterAvgFlightTimeDeviationWarning" Then 'Added By Vikrant On 30-Nov-2016 For ALL30112016-1

							mLog = Session("mLog")
							DataFieldBind()

							If mLog.IsValid Then

								If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

								If SaveLogAfterAvgFlightTimeDeviationWarning() = True Then

									If Session("New") = "True" Then

										Session("New") = ""
										NewRecord()
										Session.Remove("FileAttach")
										Session.Remove("IsAttachmentDeleted")
										Session("mLog") = mLog
										'Added By Vikrant on 01-Dec-2021 for PBH
										If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

											If Session("IsAircraftMadeNotInUse") = "True" Then
												Session.Remove("AircraftId")
												Session.Remove("IsAircraftMadeNotInUse")
												MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
											End If

										End If

										'End
										DataFieldBind()
										EnableDisableButton()
										ControlVisibility()
										ControlVisibilityForAttachment()
										DataBindGrid()
										SetTitle()

										upnlLogDetails.Update()
										upnlFlightDetails.Update()
										upnlFlightSummary.Update()
										upnlTabs.Update()
										upnlTabsNew.Update()

									Else

										mLog = Log.GetLog(mLog.ID)
										mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
										mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
										mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
										Session("mLog") = mLog

										SetTitle()
										DataFieldBind()
										EnableDisableButton()

										If Session("SaveNClose") = "SaveNClose" Then

											Session("SaveNClose") = ""
											Session.Remove("SaveNClose")
											Session.Remove("FileAttach")
											Session.Remove("IsAttachmentDeleted")
											Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

										End If

									End If

								Else
									upnlErrorList.Update()
								End If

							End If

						ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then

							Try

								Session("Sender") = ""
								Dim mLog As Log
								mLog = CType(Session("mLog"), Log)
								mLog.FileAttachments.Remove(mLog.FileAttachments.CurrentItem)
								dgLogAttachment.DataSource = mLog.FileAttachments
								dgLogAttachment.DataBind()
								upnldgLogAttachment.Update()
								upnlLogAttachment.Update()
								Session("mLog") = mLog

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

									MSGBoxCtrl.Show(MSGBox.Message_Title.ReferenceDelete,
													MSGBox.Message_Text.ReferenceDelete,
													ex.Procedure,
													MsgBoxStyle.OkOnly,
													"")

								End If

							End Try

						End If

					Case MsgBoxResult.No

						If Session("New") = "True" Then Session("New") = ""

						If MSGBoxCtrl.Sender = "SaveNew" Then
							NewRecord()
							DataFieldBind()
						ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then  'Added by Saylee on 21-May-2012 ALL17052012 to save Flexi log
							Session.Remove("IsValueZero") 'Shweta
						ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
							Session.Remove("IsValueZero")
						ElseIf MSGBoxCtrl.Sender = "Close" Then

							Session("SaveNClose") = ""
							Session.Remove("SaveNClose")
							Session.Remove("FileAttach")
							Session.Remove("IsAttachmentDeleted")
							Session.Remove("mLog")
							Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						ElseIf MSGBoxCtrl.Sender = "MELClose" Then

							Session.Remove("FileAttach")
							Session.Remove("IsAttachmentDeleted")
							Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						ElseIf MSGBoxCtrl.Sender = "MEL" Or MSGBoxCtrl.Sender = "MELNew" Then

						End If

					Case MsgBoxResult.Cancel

						'Code Added By Deven for Save and New 20/03/2008
						If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
						End If

					Case MsgBoxResult.Ok

						'Added By Vikrant on 01-Dec-2021 for PBH
						If MSGBoxCtrl.Sender = "AircraftMadeNotInUse" Then
							Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
							Exit Sub
						End If

						DataFieldBind()

					Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
						DataFieldBind()
				End Select

			ElseIf Result = 0 Then   'Code Added
				If Session("New") = "True" Then Session("New") = ""
			ElseIf Result = -1 Then
				If Session("New") = "True" Then Session("New") = ""
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTitle()

		Dim Index As Integer
		Index = Session("Index")

		Try

			If mLog.IsNew Then

				If mLog.Date Is DBNull.Value Then
					lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of - [New]"
				Else
					lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
				End If

			Else
				lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
			End If

			upnlTitle.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddAttributes()

		Try

			txtPercentTimeOnGround.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentTimeOnGround').value,event)")
			upnlLogDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub NewRecord(LogDate As String,
						  Optional mSouLocalDateTime As String = "",
						  Optional mSouUTCDateTime As String = "")

		Try

			mLog = Log.NewLog(Machine:=mMachine,
							  LogDate:=LogDate,
							  mSouLocalDateTime:=mSouLocalDateTime,
							  mSouUTCDateTime:=mSouUTCDateTime)

			mMachine = Machine.GetMachine(MachineID:=mMachine.ID)

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub EditRecord(LogDate As DateTime)

		Try

			mLog = Log.GetLog(ID:=mLog.ID)
			mLog.Date = LogDate

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CopyFromClone(ClonedLog As Log,
							  Optional isFromLogDate As Boolean = False)

		Try

			mLog.PilotID1 = ClonedLog.PilotID1
			mLog.PilotID2 = ClonedLog.PilotID2
			mLog.Pilot1Name = ClonedLog.Pilot1Name
			mLog.Pilot2Name = ClonedLog.Pilot2Name
			mLog.IsUTC = ClonedLog.IsUTC
			mLog.SourceID = ClonedLog.SourceID

			If Not mLog.IsNew Then

				mLog.SouLocalDateTime = ClonedLog.SouLocalDateTime
				mLog.SouDayLightTime = ClonedLog.SouDayLightTime
				mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
				mLog.DesDayLightTime = ClonedLog.DesDayLightTime

			End If

			mLog.DestinationID = ClonedLog.DestinationID

			If mLog.IsUTC Then

				If Not isFromLogDate Then
					mLog.SouUniverseDateTime = ClonedLog.SouUniverseDateTime
				End If

			End If

			mLog.Remark = ClonedLog.Remark
			mLog.LogPageNo = ClonedLog.LogPageNo
			mLog.FlightNo = ClonedLog.FlightNo
			mLog.FlightLogClassificationID = ClonedLog.FlightLogClassificationID
			mLog.FlightLogClassificationName = ClonedLog.FlightLogClassificationName

			'Hobbs - taken
			mLog.CurrentHobbsValue = ClonedLog.CurrentHobbsValue
			mLog.OffSet = ClonedLog.OffSet

			'Added by Yogita Ajax
			mLog.ImageFile = ClonedLog.ImageFile
			mLog.ImageSize = ClonedLog.ImageSize
			mLog.FileExtension = ClonedLog.FileExtension
			'--------------------------------------

			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetFromAutoComplete()

		Dim tempString As String
		Dim tempString1 As String
		Dim Place1Code As String
		Dim Place2Code As String

		Try

			tempString = Place1.Text.Trim
			If Not tempString = String.Empty Then

				If tempString.IndexOf("[") >= 0 Then
					tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
				End If

				If tempString.IndexOf("[") >= 0 And tempString.IndexOf("]") >= 0 Then
					Place1Code = tempString.Substring(tempString.IndexOf("["), tempString.IndexOf("]") - tempString.IndexOf("[")).Trim
				End If

			End If

			tempString1 = Place2.Text.Trim
			If Not tempString1 = String.Empty Then

				If tempString1.IndexOf("[") >= 0 Then
					tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
				End If

				If tempString1.IndexOf("[") >= 0 And tempString1.IndexOf("]") >= 0 Then
					Place2Code = tempString1.Substring(tempString1.IndexOf("["), tempString1.IndexOf("]") - tempString1.IndexOf("[")).Trim
				End If

			End If
			'End

			'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
			If (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

				mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
				mLog.Pilot1Name = "None"

			Else
				mLog.PilotID1 = mSearchListPilot.Item(Pilot1.Text.Trim).GId
				mLog.Pilot1Name = mSearchListPilot.Item(Pilot1.Text.Trim).Name
			End If

			mLog.PilotID2 = mSearchListPilot.Item(Pilot2.Text.Trim).GId
			mLog.Pilot2Name = mSearchListPilot.Item(Pilot2.Text.Trim).Name

			'Changed By Utkarsh On 24-Nov-2011 For ALL23112011
			mLog.SourceID = mSearchListPlace.Item(tempString).GId
			mLog.DestinationID = mSearchListPlace.Item(tempString1).GId
			'End

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetTakeoffTouchdownTitle()

		If TakeOffTouchDown Then

			lblDepDateTime.Text = "ChocksOff Date / Time"
			lblUTCDateTime.Text = "UTC ChocksOff Date / Time"
			lblArrDate.Text = "ChocksOn Date / Time"
			lblUTCArrivalDateTime.Text = "UTC ChocksOn Date / Time"

			upnlFlightDetails.Update()

		End If

	End Sub

	'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013
	Private Function CheckZeroDifferenceValue() As Boolean

		Try

			If mLog.IsHobbs Then

				If Val(mLog.TotalTime) <> 0 Then
					Return False
				End If

				If Val(mLog.TimeInAir) <> 0 Then
					Return False
				End If

			Else

				If mLog.TimeInAir = "0:00" OrElse mLog.TimeInAir = "" Then
				Else
					Return False
				End If

				If mLog.TotalTime = "0:00" OrElse mLog.TotalTime = "" Then
				Else
					Return False
				End If

				If mLog.BlockTime = "0:00" OrElse mLog.BlockTime = "" Then
				Else
					Return False
				End If

				If mLog.TimeOnGround = "0:00" OrElse mLog.TimeOnGround = "" Then
				Else
					Return False
				End If

			End If

			If Val(mLog.TotalLandings) <> 0 Then
				Return False
			End If

			Dim checkcol = mLog.LogAFAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = mLog.LogAPUAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = mLog.LogEngAssemblies
			If Not CallZeroDifferenceValue(checkcol) Then
				Return False
			End If

			checkcol = mLog.LogCGBAssemblies
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

				If mLog.IsHobbs Then

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
	'End

	Private Sub GetAttachment()

		Try

			If mLog.IsAttachmentAdded And FileAttach Is Nothing Then

				FileAttach = FileAttach.GetAttachment(mLog.ID)
				Session("mFileAttach") = FileAttach

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityForAttachment()

		Try

			If mLog.IsAttachmentAdded = True Then
				ImageButton1.Visible = True
				btnDelAttch.Enabled = True
			Else
				ImageButton1.Visible = False
				btnDelAttch.Enabled = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetPBHValues(TmpLog As Log, IsLogNew As Boolean)

		Try

			If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022

				If IsLogNew Then

					Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")

					If Not mPBH.MachineID.Equals(Guid.Empty) Then

						If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

							mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec

							If IsLogNew Then
								mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
							Else
								mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec + New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec), 1, False, False).Value
							End If

							mPBH.RemainingHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.ElapsedHoursDec, 1, False, False).Value
							mPBH.LastLogDetails = TmpLog.DateFormatted

							'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
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

			ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022

				Dim mPBH As PBH

				If IsLogNew Then

					Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
					mPBH = PBH.GetPBH(mPBHList(0).ID)

					If CDate(Today.Date) >= CDate(mPBH.StartDate) Then

						mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value

						If mPBH.CarryForwardHoursDec < 0 Then
							mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
						Else
							mPBH.ElapsedHours = New Period(1, (mPBH.HoursFrequencyDec + mPBH.CarryForwardHoursDec) - mPBH.RemainingHoursDec, 1, False, False).Value
						End If

						mPBH.LastLogDetails = TmpLog.DateFormatted

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

	Private Sub SaveAttachment() '

		Try

			If FileAttach IsNot Nothing Then

				If FileAttach.Size > 0 Then

					Try

						FileAttach.Save()

					Catch ex As Exception
						ScriptManager.RegisterClientScriptBlock(Me,
																[GetType],
																"",
																MessageBox.Show(ex.InnerException.ToString, False),
																True)
					End Try

				Else

					If (Not mLog.IsNew) And IsAttachmentDeleted Then
						FileAttach.DeleteAttachment(FileAttach.ID, mLog.ID)
					End If

					IsAttachmentDeleted = False
					Session("IsAttachmentDeleted") = IsAttachmentDeleted

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ViewImage()

		Dim No As New Random
		Dim StrName As String = "abc" & No.Next.ToString

		Try

			GetAttachment()

			If FileAttach.Size > 0 Then

				Dim path As String = AppSettings("DOCPath") & "\" & StrName & FileAttach.Extension
				Dim fs As FileStream

				If File.Exists(AppSettings("DOCPath")) = False Then

					'Delete File if exist
					File.Delete(AppSettings("DOCPath") & StrName & FileAttach.Extension)
					' Create the file.
					fs = File.Create(path)
					'' Add some information to the file.
					fs.Write(FileAttach.ImageFile, 0, FileAttach.ImageFile.Length)
					fs.Close()
					Session("DOCPath") = path

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"openFile",
														"openFile();",
														True)

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub dgLogAttachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgLogAttachment.RowCommand
		Dim FileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgLogAttachment.PageSize * dgLogAttachment.PageIndex

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				FileAttachments = mLog.FileAttachments
				'FileAttachments.CurrentIndex = Index - 1

				If FileAttachments.Count = 1 Then
					FileAttachments.CurrentIndex = 0
				Else
					FileAttachments.CurrentIndex = Index - 1
				End If

				If FileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & FileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & FileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(FileAttachments.CurrentItem.ImageFile, 0, FileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, [GetType], "openFile", "openFile();", True)
					End If
				End If
				dgLogAttachment.DataSource = mLog.FileAttachments
				dgLogAttachment.DataBind()
				ControlVisibility()
				upnlLogAttachment.Update()
				upnldgLogAttachment.Update()
			Case "Remove"
				'Dim Index As Integer = CInt(e.CommandArgument) '+ dgLogAttachment.PageSize * dgLogAttachment.PageIndex
				Dim Index As Integer = CInt(e.CommandArgument) + dgLogAttachment.PageSize * dgLogAttachment.PageIndex
				' DeleteAttachment(Index)
				FileAttachments = mLog.FileAttachments
				If FileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select

	End Sub

	Private Sub DeleteAttachment(Index As Int32)
		MSGBoxCtrl.Show(MSGBox.Message_Title.RemoveItem, MSGBox.Message_Text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mLog.FileAttachments.CurrentIndex = Index
		Session("mLog") = mLog
	End Sub

	Private Sub AttachMyFile()

		Dim BackupPath As String = ""
		BackupPath = AppSettings("DOCPath") & "New.PDF"
		mLog = Session("mLog")
		Try
			If Not mLog.FileAttachments.Contains(mLog.ID, CType(Session("FileUpload.FileName"), String)) Then

				mLog.FileAttachments.Add(mLog.ID, CType(Session("FileUpload.FileName"), String))
				' mLog.FileAttachments.CurrentItem.FileName = FileAttach.FileName
				mLog.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
				mLog.FileAttachments.CurrentItem.Size = Session("Size")
				mLog.FileAttachments.CurrentItem.Extension = Session("Extension")
				'   mLog.FileAttachments.CurrentItem.SrNo = (mLog.FileAttachments.Count - 1) + 1

				Session("mLog") = mLog
				dgLogAttachment.DataSource = mLog.FileAttachments
				dgLogAttachment.DataBind()

				For i As Integer = 0 To mLog.FileAttachments.Count - 1
					Dim txtValue As TextBox
					txtValue = CType(Me.dgLogAttachment.Rows(i).FindControl("txtFileName"), TextBox)
					txtValue.Text = mLog.FileAttachments(i).FileName
				Next

				Session.Remove("Size")
				Session.Remove("ImageFile")
				Session.Remove("Extension")
				Session.Remove("FileUpload.FileName")
				upnlLogAttachment.Update()
				upnldgLogAttachment.Update()
			Else
				Session("mLog") = mLog
				MSGBoxCtrl.Show(MSGBox.Message_Title.Duplicate, MSGBox.Message_Text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub

	Private Function IsValidTime(TimeValue As String) As Boolean
		Dim TimeRegulerExpression As String = ""
		If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
			'TimeRegulerExpression = "^(([01][\d]+)|(2[0-3]))\:[0-5][0-9]( )*(AM|am|PM|pm)$"   '12 Hour Format
			TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
		Else
			TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
		End If

		If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
			Return True
		Else
			Return False
		End If
	End Function

	'Added by Saylee on 9-Mar-2022, FlyAPP Notification
	Public Sub SendPUSHNotification(Log As Log)

		Try

			Dim PreviousStepStatus As Boolean = False

			'Step # 1: Get User Devices
			Dim mUserDeviceList As APP_UserDeviceList = APP_UserDeviceList.GetUserDeviceList(1) '1:Flight Log

			If mUserDeviceList.Count = 0 Then
				PreviousStepStatus = False
			Else
				PreviousStepStatus = True
			End If

			If PreviousStepStatus = False Then Exit Sub

			'Step # 2: Record PUSH Notification in the table
			Dim UserIDs(50) As Guid
			UserIDs = (From c As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList
					   Select (c.UserID)).Distinct().ToArray()

			Dim Notifications(UserIDs.Count - 1) As APP_UserNotification

			For i As Integer = 0 To UserIDs.Count - 1

				If UserIDs(i).Equals(Guid.Empty) Then Exit For

				Dim mAPP_UserNotification As APP_UserNotification = APP_UserNotification.NewAPP_UserNotification(Guid.NewGuid)

				Try

					With mAPP_UserNotification

						.UserID = UserIDs(i)
						.SentOn = Now

						'Flight Log Created for VT-WED as on 24-Feb-2021
						.Message = "Flight Log Created for:- " + Log.RegNo + " as on:- " + Log.DateFormatted
						.ModuleType = 1 'Flight Log
						.ModuleID = Log.ID

					End With

					mAPP_UserNotification = CType(mAPP_UserNotification.Save, APP_UserNotification)
					Notifications(i) = mAPP_UserNotification
					PreviousStepStatus = True

				Catch ex As Exception
					PreviousStepStatus = False
				End Try

			Next

			If PreviousStepStatus = False Then Exit Sub

			For Each Notification As APP_UserNotification In Notifications

				Dim ErrorCount As Integer = 0

				'Step # 3: Trigger PUSH Notification
StartStep3:     ErrorCount = ErrorCount + 1

				Net.ServicePointManager.Expect100Continue = True
				Net.ServicePointManager.SecurityProtocol = 3072

				Dim request = TryCast(Net.WebRequest.Create("https://onesignal.com/api/v1/notifications"), Net.HttpWebRequest)

				request.KeepAlive = True
				request.Method = "POST"
				request.ContentType = "application/json; charset=utf-8"

				request.Headers.Add("authorization", "Basic YmE0YTUwZDgtMmJkYS00MjMzLWI5NjgtZTkxZmE5MzQ0NzMw")

				Dim serializer = New JavaScriptSerializer()

				'Forming Notification Detail URL
				Dim index As Integer = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("wfLogSOP_Ajax.aspx")
				Dim urlNotificationDetail As String = ""
				urlNotificationDetail = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, index) + "APP/Launcher.aspx?NotificationID=" + Notification.ID.ToString + "&ModuleID=" + Log.ID.ToString + "&username=" + Notification.UserName + "&EventLogSessionID=" + Guid.NewGuid.ToString + "&ModuleTypeID=1"

				Dim filterObject As Object()
				ReDim filterObject(((mUserDeviceList.Count - 1) * 2) + 1)

				Dim IndexStep3 As Integer = 0
				Dim RIndex As Integer = 0

				For Each info As APP_UserDeviceList.UserDeviceinfo In mUserDeviceList

					If Notification.UserID.Equals(info.UserID) Then

						If index = 0 Then
							filterObject(index) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(0).DeviceID.ToString}
							index = index + 1
						Else

							RIndex = RIndex + 1

							filterObject(index) = New With {Key .[operator] = "OR"}
							index = index + 1

							filterObject(index) = New With {Key .field = "tag", Key .key = "DeviceID", Key .value = mUserDeviceList(RIndex).DeviceID.ToString}
							index = index + 1

						End If

					End If

				Next

				Dim obj = New With {Key .app_id = "f877b4d2-b6e5-4595-a381-87165f6e46a0", Key .contents = New With {Key .en = Notification.Message}, Key .headings = New With {Key .en = "FlyPal"}, Key .filters = filterObject, Key .data = New With {Key .url = urlNotificationDetail.ToString}}

				Dim param = serializer.Serialize(obj)
				Dim byteArray As Byte() = Encoding.UTF8.GetBytes(param)

				Dim responseContent As String = Nothing

				Try

					Using writer = request.GetRequestStream()
						writer.Write(byteArray, 0, byteArray.Length)
					End Using

					Using response As System.Net.HttpWebResponse = request.GetResponse()

						Using reader = New System.IO.StreamReader(response.GetResponseStream())

							responseContent = reader.ReadToEnd()

						End Using

					End Using

				Catch ex As Net.WebException

					Diagnostics.Debug.WriteLine(ex.Message)
					Diagnostics.Debug.WriteLine(New StreamReader(ex.Response.GetResponseStream()).ReadToEnd())

					If ErrorCount <= 3 Then GoTo StartStep3

				End Try

				Diagnostics.Debug.WriteLine(responseContent)

			Next

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Private Sub ControlExceedanceTabVisibility()

	'    Try

	'        If (Not mLog.IsNew) AndAlso
	'           (mLog.LogAFAssemblies.Count > 0 OrElse
	'            mLog.LogAFAssemblies.Count > 0 OrElse
	'            mLog.LogAFAssemblies.Count > 0 OrElse
	'            mLog.LogAFAssemblies.Count > 0) Then

	'            tbpnlExceedancePeriodValues.Visible = True

	'        End If

	'    Catch ex As Exception
	'        Throw ex.GetBaseException
	'    End Try

	'End Sub

#End Region

#Region " Custom Validations "

	Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

		Try

			Dim CustomValidator As CustomValidator
			CustomValidator = CType(s, CustomValidator)
			GridColumnHeadingSet()
			Dim tempString As String 'Added By Utkarsh On 24-Nov-2011 For ALL23112011

			If CustomValidator.ControlToValidate = "txtRemark" Then

				If Len(txtRemark.Text) > 500 Then
					CustomValidator.ErrorMessage = "Max. length of Remark should be 500 char"
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			ElseIf CustomValidator.ControlToValidate = "calDeparture" Then

				'CNDC
				If Not IsDate(calDeparture.Text) Then
					CustomValidator.ErrorMessage = "Departure date should be in valid date time format."
					e.IsValid = False
				Else

					Dim Date1, Time1 As String
					Date1 = calDeparture.Text.ToString
					Time1 = calDeparture.Text.ToString

					If Date1 = "1/1/0001" Then

						CustomValidator.ErrorMessage = "Departure date should be in valid date time format."
						e.IsValid = False

						Exit Sub
					End If

					'CNDC
					calDeparture.Text = Date1
					e.IsValid = True

				End If

			ElseIf CustomValidator.ControlToValidate = "calArrival" Then

				'CNDC
				If Not IsDate(calArrival.Text) Then
					CustomValidator.ErrorMessage = "Arrival date should be in valid date time format."
					e.IsValid = False
				Else

					Dim Date1, Time1 As String
					'CNDC
					Date1 = calArrival.Text.ToString
					Time1 = calArrival.Text.ToString

					If Date1 = "1/1/0001" Then

						CustomValidator.ErrorMessage = "Arrival date should be in valid date time format."
						e.IsValid = False

						Exit Sub

					End If

					'CNDC
					calArrival.Text = Date1
					e.IsValid = True

				End If

			ElseIf CustomValidator.ControlToValidate = "Pilot1" Then

				If Not mSearchListPilot.Contains(Pilot1.Text.Trim) Then
					CustomValidator.ErrorMessage = "Enter correct Pilot1 name."
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			ElseIf CustomValidator.ControlToValidate = "Place1" Then

				'Added by Utkarsh On 24-Nov-2011 For ALL23112011
				tempString = Place1.Text.Trim

				If Not tempString = String.Empty Then

					If tempString.IndexOf("[") < 0 Then
						CustomValidator.ErrorMessage = "Enter correct Source name."
						e.IsValid = False
					Else

						tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
						If Not mSearchListPlace.Contains(tempString) Then
							CustomValidator.ErrorMessage = "Enter correct Source name."
							e.IsValid = False
						Else
							e.IsValid = True
						End If

					End If

				End If

			ElseIf CustomValidator.ControlToValidate = "Pilot2" Then

				If Not mSearchListPilot.Contains(Pilot2.Text.Trim) Then
					CustomValidator.ErrorMessage = "Enter correct Pilot2 name."
					e.IsValid = False
				Else
					e.IsValid = True
				End If

			ElseIf CustomValidator.ControlToValidate = "Place2" Then
				'Added by Utkarsh On 24-Nov-2011 For ALL23112011

				tempString = Place2.Text.Trim

				If Not tempString = String.Empty Then

					If tempString.IndexOf("[") < 0 Then
						CustomValidator.ErrorMessage = "Enter correct Destination name."
						e.IsValid = False
					Else

						tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim

						If Not mSearchListPlace.Contains(tempString) Then
							CustomValidator.ErrorMessage = "Enter correct Destination name."
							e.IsValid = False
						Else
							e.IsValid = True
						End If

					End If

				End If

			End If

			'Sankalp 19-Aug-25
			If (AppSettings("ClientCode") = "AFC") Then

				If CustomValidator.ControlToValidate = "cmbFlightLogClassification" Then

					If (cmbFlightLogClassification.SelectedItem.Text = "" Or cmbFlightLogClassification.SelectedItem.Text = "(SELECT)") Then
						CustomValidator.ErrorMessage = "Select Log Classification."
						e.IsValid = False 'Validation fails
					Else
						e.IsValid = True 'Validation passes
					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Sub CustomValidation(s As Object, e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)

		Try

			If Flag = 1 Then Exit Sub

			Dim str As String = ""
			Dim CustomValidator As CustomValidator
			CustomValidator = CType(s, CustomValidator)
			upnlFlightSummary.DataBind()
			upnlFlightSummary.Update()

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)  'True added by Saylee 25-July-2012
			SetAPUGridObject(True)     'True added by Saylee 25-July-2012
			SetCGBGridObject(True)     'True added by Saylee 25-July-2012
			GridColumnHeadingSet()

			'Log
			If Not mLog.IsValid Then
				For i As Integer = 0 To mLog.GetBrokenRulesCollection.Count - 1
					str = str + mLog.GetBrokenRulesCollection(i).Description + "<BR>"
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
				CustomValidator.ErrorMessage = str
				e.IsValid = False

			End If

			Flag = 1

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Public Function GVLogFuelOilValidations() As Boolean    'For DgLog Fuel Oils

		Dim str As String = ""

		Try

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

			'Added By Vikrant On 02-Sept-2013 For ALL02092013
			If Not mLog.PilotID1.Equals(mLog.PrevPilotID2) And Not mLog.PilotID2.Equals(mLog.PrevPilotID1) Then

				If Not mLog.PilotID1.Equals(mLog.PrevPilotID1) Then

					If mLog.LogCrews.Contains(mLog.PilotID1) Then
						str = str + "Please Select Different Pilot as '" + "<b>" + mLog.Pilot1Name + "</b>" + "' is already entered in Flight Crew."
						Pilot1.Text = mSearchListPilot(mLog.PrevPilotID1).Name
					End If

				End If

				If Not mLog.PilotID2.Equals(mLog.PrevPilotID2) Then

					If mLog.LogCrews.Contains(mLog.PilotID2) Then
						str = str + "Please Select Different Co-Pilot as '" + "<b>" + mLog.Pilot2Name + "</b>" + "' is already entered in Flight Crew."
						Pilot2.Text = mSearchListPilot(mLog.PrevPilotID2).Name
					End If

				End If

			End If
			'End

			If str <> "" Then
				cvRemark.ErrorMessage = str
				cvRemark.IsValid = False

				Return False
			End If

			Return True

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " Data Binding "

	'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
	'Added By Prashant 31-July-2009 To changed all grids heading "Cycles" to "Flights"  for "Heligo"
	Private Sub GridColumnHeadingSet()

		Dim clientCode As String = AppSettings("ClientCode")
		Try

			Select Case clientCode
				Case "Heligo", "UHPL", "BAL"

					Dim gridsToUpdate = New List(Of GridView)() From {gvAFPeriods, gvEnginePeriods, gvAPUPeriods, gvCGBPeriods}

					For Each grid As GridView In gridsToUpdate
						grid.Columns(7).HeaderText = "Flights"
						grid.Columns(8).HeaderText = "Final Flights"
					Next

					lblCGBPeriod.Text = "CGB Period"

					SetCGBReadOnly()

				Case "IND"

					gvEnginePeriods.Columns(11).HeaderText = "PTCNTC"
					gvEnginePeriods.Columns(12).HeaderText = "Final PTCNTC"
					gvEnginePeriods.Columns(13).HeaderText = "CTCNTC"
					gvEnginePeriods.Columns(14).HeaderText = "Final CTCNTC"
					gvEnginePeriods.Columns(21).HeaderText = "IMCNTC"
					gvEnginePeriods.Columns(22).HeaderText = "Final IMCNTC"
					gvEnginePeriods.Columns(23).HeaderText = "C1"
					gvEnginePeriods.Columns(24).HeaderText = "Final C1"
					gvEnginePeriods.Columns(25).HeaderText = "C2"
					gvEnginePeriods.Columns(26).HeaderText = "Final C2"
					gvAPUPeriods.Columns(17).HeaderText = "APU Hours"
					gvAPUPeriods.Columns(18).HeaderText = "Final APU Hours"

				Case "FBW"

					Dim gridsToUpdate = New List(Of GridView)() From {gvAFPeriods, gvEnginePeriods, gvAPUPeriods, gvCGBPeriods}

					For Each grid As GridView In gridsToUpdate
						grid.Columns(17).HeaderText = "AHH"
						grid.Columns(18).HeaderText = "Final AHH"
					Next

				Case "ABD"

					gvEnginePeriods.Columns(11).HeaderText = "N1"
					gvEnginePeriods.Columns(12).HeaderText = "Final N1"
					gvEnginePeriods.Columns(13).HeaderText = "N2"
					gvEnginePeriods.Columns(14).HeaderText = "Final N2"

				Case "SHR"

					gvEnginePeriods.Columns(23).HeaderText = "Creep %"
					gvEnginePeriods.Columns(24).HeaderText = "Final Creep %"

				Case "SAP"

					Dim gridsToUpdate = New List(Of GridView)() From {gvAFPeriods, gvEnginePeriods, gvAPUPeriods, gvCGBPeriods}

					For Each grid As GridView In gridsToUpdate
						grid.Columns(7).HeaderText = "Flight Cycles"
						grid.Columns(8).HeaderText = "Final Flight Cycles"
					Next

					gvEnginePeriods.Columns(15).HeaderText = "Cycles"
					gvEnginePeriods.Columns(16).HeaderText = "Final Cycles"
					gvAPUPeriods.Columns(15).HeaderText = "Cycles"
					gvAPUPeriods.Columns(16).HeaderText = "Final Cycles"

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetCGBReadOnly()

		Try

			Dim txtControls As String() = {
				"txtCGBHours", "txtCGBLandings", "txtCGBCycles", "txtCGBStarts",
				"txtCGBNGCycles", "txtCGBNFCycles", "txtCGBRins", "txtCGBBleeds",
				"txtCGBImpellerCycles", "txtCGBCTCycles", "txtCGBPTCycles", "txtCGBGeneratorMods"
			}

			For l As Integer = 0 To gvCGBPeriods.Rows.Count - 1

				For Each controlId As String In txtControls

					Dim txtBox As TextBox = TryCast(Me.gvCGBPeriods.Rows(l).FindControl(controlId), TextBox)
					If txtBox IsNot Nothing Then
						txtBox.ReadOnly = True
					End If

				Next

			Next

		Catch ex As Exception
			Throw
		End Try

	End Sub

	Private Sub DataFieldBind()

		Try

			gvAFPeriods.DataSource = mLog.LogAFAssemblies
			gvEnginePeriods.DataSource = mLog.LogEngAssemblies
			gvAPUPeriods.DataSource = mLog.LogAPUAssemblies
			gvCGBPeriods.DataSource = mLog.LogCGBAssemblies
			gvALLAssemblies.DataSource = mLog.ALL_LogAssemblies ''Added by Saylee on 1-Mar-2022
			txtLogNo.Text = mLog.LogNo
			txtLogText.Text = mLog.LogText

			'CNDC
			If mLog.Date IsNot DBNull.Value Then
				calDateTime.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
			Else
				calDateTime.Text = ""
			End If

			If mLog.SouLocalDateTime IsNot DBNull.Value Then
				calDeparture.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
				txtDepartureTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
			Else
				calDeparture.Text = ""
				'calDepartureTime.Text = ""
			End If

			If mLog.DesLocalDateTime IsNot DBNull.Value Then
				calArrival.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("DateFormat"))
				txtArrivalTime.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("TimeFormat"))
			Else

				If mLog.SouLocalDateTime IsNot DBNull.Value Then
					calArrival.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
					txtArrivalTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
				Else
					calArrival.Text = ""
				End If

			End If

			''''''''''''''''CalUTCDateTime.Value = mLog.SouUniverseDateTime
			If mLog.SouUniverseDateTime IsNot DBNull.Value Then
				CalUTCDateTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
				txtUTCDepartureTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
			Else
				CalUTCDateTime.Text = ""
			End If

			''''''''''''''''''''CalUTCArrival.Value = mLog.DesUniverseDateTime
			If mLog.DesUniverseDateTime IsNot DBNull.Value Then
				CalUTCArrival.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("DateFormat"))
				txtUTCArrivalTime.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("TimeFormat"))
			Else

				If mLog.SouUniverseDateTime IsNot DBNull.Value Then
					CalUTCArrival.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
					txtUTCArrivalTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
				Else
					CalUTCArrival.Text = "" 'Change by Vikrant on 20-Oct-2015 for Religare
				End If

			End If

			'Added By Utkarsh On 30-Aug-2011
			If TakeOffTouchDown Then

				If mLog.TakeOffLocalDateTime IsNot DBNull.Value Then
					calTakeOffLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
					txtTakeOffLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
				Else
					calTakeOffLocalDateTime.Text = ""
				End If

				If mLog.TakeOffUniverseDateTime IsNot DBNull.Value Then
					calUTCTakeOffDateTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("DateFormat"))
					txtUTCTakeOffTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
				Else
					calUTCTakeOffDateTime.Text = ""
				End If

				If mLog.TouchDownLocalDateTime IsNot DBNull.Value Then
					calTouchDownLocalDateTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("DateFormat"))
					txtTouchDownLocalTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("TimeFormat"))
				Else

					If mLog.TakeOffLocalDateTime IsNot DBNull.Value Then
						calTouchDownLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
						txtTouchDownLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
					Else
						calTouchDownLocalDateTime.Text = ""
					End If

				End If

				If mLog.TouchDownUniverseDateTime IsNot DBNull.Value Then
					calUTCTouchDownDateTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("DateFormat"))
					txtUTCTouchDownTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("TimeFormat"))
				Else

					If mLog.TakeOffUniverseDateTime IsNot DBNull.Value Then
						calUTCTouchDownDateTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("DateFormat"))
						txtUTCTouchDownTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
					Else
						calUTCTouchDownDateTime.Text = "" 'Change by Vikrant on 20-Oct-2015 for Religare
					End If

				End If

			End If
			'End

			If TakeOffTouchDown Then
				txtBlockTime.Text = mLog.DiffTime
				txtGroundRunTime.Text = mLog.TimeOnGround
			Else
				txtBlockTime.Text = mLog.DiffTime
			End If

			'Prashant
			mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
			cmbFlightLogClassification.DataSource = mFlightLogClassificationList
			Session("mFlightLogClassificationList") = mFlightLogClassificationList

			dgLogAttachment.DataSource = mLog.FileAttachments

			EngineDerate = EngineDerate.GetDerateList("", "(SELECT)")
			ddlEngineDerate.DataSource = EngineDerate
			Session("EngineDerate") = EngineDerate

			'Code Added by DEVEN On 29/12/2007 --------------------------------------
			DataBind()
			GridColumnHeadingSet()

			If cmbFlightLogClassification.Items.Contains(New ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
				cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
			Else
				cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
			End If
			'------------------------------------------------------------------------

			If CBool(AppSettings("ShowEngineDerateOptions")) Then

				If ddlEngineDerate.Items.Contains(New ListItem(mLog.EngineDerateValue, mLog.EngineDerateID.ToString)) Then
					ddlEngineDerate.SelectedValue = mLog.EngineDerateID.ToString
				Else
					ddlEngineDerate.SelectedValue = EngineDerate(1).ID.ToString
				End If

			End If

			'Added By Utkarsh On 23-Aug-2011
			mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
			Session("mSearchListPilot") = mSearchListPilot
			mSearchListPlace = SearchList.GetSearchList("Place", "", "")
			Session("mSearchListPlace") = mSearchListPlace
			'end

			Pilot1.Text = mLog.Pilot1Name
			Pilot2.Text = mLog.Pilot2Name

			mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
			Session("mLogListOnDate") = mLogListOnDate

			mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
			Session("mCompanyDetail") = mCompanyDetail

			upnlLogDetails.Update()
			upnlFlightDetails.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()
			upnlRemark.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DataBindGrid()

		Try

			If mLog IsNot Nothing Then

				SetAirFrameGridObject(True)
				SetEngineGridObject(True)
				SetAPUGridObject(True)
				SetCGBGridObject(True)

				gvAFPeriods.DataSource = mLog.LogAFAssemblies
				gvAFPeriods.DataBind()

				gvEnginePeriods.DataSource = mLog.LogEngAssemblies
				gvEnginePeriods.DataBind()

				gvAPUPeriods.DataSource = mLog.LogAPUAssemblies
				gvAPUPeriods.DataBind()

				gvCGBPeriods.DataSource = mLog.LogCGBAssemblies
				gvCGBPeriods.DataBind()

				gvALLAssemblies.DataSource = mLog.ALL_LogAssemblies
				gvALLAssemblies.DataBind()

				GridColumnHeadingSet()

				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

				Session("mLog") = mLog

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Code Added By Deven 21-03-2008 
	Private Sub BindClassification()

		Try

			mLog = CType(Session("mLog"), Log)

			mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "(SELECT)")
			cmbFlightLogClassification.DataSource = mFlightLogClassificationList
			cmbFlightLogClassification.DataBind()

			Session("mFlightLogClassificationList") = mFlightLogClassificationList

			If cmbFlightLogClassification.Items.Contains(New ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
				cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
			Else
				cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
			End If

			upnlLogDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'------------------------------------------------------------------------

	Private Sub BindEngineDerate()

		Try

			mLog = CType(Session("mLog"), Log)

			EngineDerate = EngineDerate.GetDerateList("", "(SELECT)")
			ddlEngineDerate.DataSource = EngineDerate
			ddlEngineDerate.DataBind()
			Session("EngineDerate") = EngineDerate

			If ddlEngineDerate.Items.Contains(New ListItem(mLog.EngineDerateValue, mLog.EngineDerateID.ToString)) Then
				ddlEngineDerate.SelectedValue = mLog.EngineDerateID.ToString
			Else
				ddlEngineDerate.SelectedValue = "0"
			End If

			upnlLogDetails.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()

			TakeOffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean) 'Added By Utkarsh On 31-Aug-2011
			mLog.IsTakeoffTouchDown = TakeOffTouchDown  'Added By Utkarsh On 02-Sep-2011
			EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011

			AddAttributes()

			If Not IsPostBack Then

				If calDateTime.Enabled = True Then
					SetFocus(calDateTime)
				End If

				If (AppSettings("ClientCode") = "Heligo" Or
					AppSettings("ClientCode") = "UHPL" Or
					AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP") Then

					mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
					mLog.Pilot1Name = "None"

				End If

				DataFieldBind()

				If mLogListOnDate.Count > 0 And
				   mLog.IsNew And
				   AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"Show Last Det",
														"ShowLastDet();",
														True)
					upnlLogInfo.Update()

				End If

				upnlLogDetails.Update()

				ControlVisibilityForAttachment()

			End If

			EnableDisableButton()
			ControlVisibility()
			DataBindGrid()
			SetTitle()
			SetTakeoffTouchdownTitle()  'Added By Utkarsh On 31-Aug-2011
			SetFromAutoComplete() 'Added By Utkarsh On 24-Aug-2011
			'ControlExceedanceTabVisibility()

			mLog.LogPageNo = txtLogPageNo.Text.Trim  'Added By Utkarsh On 28-Nov-2011
			mLog.FlightNo = txtFlightNo.Text.Trim     'Added By Utkarsh On 28-Nov-2011

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

		Try

			LogObjValue.Value = IIf(mLog.IsNew, "True", "False")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SaveLog(sender As Object, e As EventArgs) Handles btnSave.Click

		Try

			If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then

				BindClassification()
				BindEngineDerate()
				SetObject()
				SetSession()
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Action.Save,
						"Flight Log",
						User.Identity.Name & " is not Authorized User to save " & mLogDetail,
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")

				Exit Sub

			End If

			If Not IsValid Then upnlErrorList.Update() : Exit Sub

			'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
			If IsValid Then

				If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then

					Dim Title As String = "Save Alert !"
					Dim Message As String = ""

					If Not mLog.PilotID1.Equals(Guid.Empty) Then

						Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString,
																										mLog.Date.ToString)

						If mEmployeeStatus(0).Information <> "" Then
							Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information.ToString.Replace("Resource",
																																  "")
						End If

					End If

					If Not mLog.PilotID2.Equals(Guid.Empty) Then

						Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString,
																										mLog.Date.ToString)

						If mEmployeeStatus(0).Information <> "" Then

							Message = IIf(Message.Length > 0,
										  Message & "<br/ >",
										  "") &
									  "<b>Co-Pilot : </b> <br />" &
									  mEmployeeStatus(0).Information.ToString.Replace("Resource", "")

						End If

					End If

					If Message.Length > 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Custom,
										Message,
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

				End If

			End If
			'End

			'Added by Saylee on 18-May-2012 ALL17052012
			Dim mMaxLogOfAircraft As MaxLogOfAircraft
			mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

			If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

				If Not (AppSettings("ClientCode") = "Heligo" Or
						AppSettings("ClientCode") = "UHPL" Or
						AppSettings("ClientCode") = "APFT" Or
						AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

					Dim MaxLogDateTime As String = ""

					If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
						MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
					Else
						MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
					End If

					If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										" You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
												   MaxLogDateTime & "<BR> <BR>Do you want to continue?",
										MsgBoxStyle.YesNo,
										"SaveLogFlexiLog")

						Exit Sub

					End If

				Else

					If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										" You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
												   mMaxLogOfAircraft.LogDateFormatted &
												   "<BR> <BR>Do you want to continue?",
										MsgBoxStyle.YesNo,
										"SaveLogFlexiLog")

						Exit Sub

					End If

				End If

			End If

			'Added By Prashant 12-Apr-2010
			Dim IsMELCount As Boolean = False
			Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
			mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)

			For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1

				If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then   'Added By Prashant 23-Sep-2010

					If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then

						IsMELCount = True
						Exit For

					Else
						IsMELCount = False
					End If

				End If

			Next

			mTempMELSnagCorrectiveActionList = Nothing

			If IsMELCount = True Then

				MSGBoxCtrl.Show("Minimum Equipment Level",
								"Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ",
								"",
								MsgBoxStyle.YesNo,
								"MEL")

				If IsValid Then

					SetObject()
					SetAirFrameGridObject()
					SetEngineGridObject(True)
					SetAPUGridObject(True)
					SetCGBGridObject(True)

				Else
					upnlErrorList.Update()
				End If

				Exit Sub

			End If

			If IsValid Then

				If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub

				If Save() = True Then

					mLog = Log.GetLog(mLog.ID)
					mLog.IsUTC = mMachine.IsUTC 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
					mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
					mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
					Session("mLog") = mLog

					'Added by Saylee on 14-July-2009
					Session("mAircraftInformationBoardList") = Nothing
					'*********************************

					'Added By Vikrant on 01-Dec-2021 for PBH
					If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

						If Session("IsAircraftMadeNotInUse") = "True" Then

							Session.Remove("AircraftId")
							Session.Remove("IsAircraftMadeNotInUse")
							MSGBoxCtrl.Show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
							Exit Sub

						End If

					End If
					'End

					DataFieldBind()
					EnableDisableButton()
					ControlVisibility()
					ControlVisibilityForAttachment()
					DataBindGrid()
					SetTitle()

					upnlLogDetails.Update()
					upnlFlightDetails.Update()
					upnlFlightSummary.Update()
					upnlTabs.Update()
					upnlTabsNew.Update()

					SendPUSHNotification(mLog) 'Added by Saylee on 9-Mar-2022, FlyAPP Notification

				End If

			Else
				upnlErrorList.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnBack.Click

		Session("IsValid") = IsValid
		Try

			If mLog.IsDirty Then

				Session.Remove("wfLogDefectActionList_Ajax") 'Added by saylee 20-11-2023 

				MSGBoxCtrl.Show(MSGBox.Message_Title.CloseConfirm,
								MSGBox.Message_Text.Save,
								"",
								MsgBoxStyle.YesNo,
								"Close")

				If IsValid Then

					SetObject()
					SetAirFrameGridObject()
					SetEngineGridObject(True)
					SetAPUGridObject(True)
					SetCGBGridObject(True)

				Else
					upnlErrorList.Update()
				End If

			Else
				MarkLog(Action.Close, "Flight Log", "", ErrorType.HandledError, mLog.ID, EventLogID)
				RemoveSession()
				Response.Redirect(Request.QueryString("BackPage") & "?")
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

		If (Not User.IsInRole("LogPrint")) Then

			MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
							MSGBox.Message_Text.Authorization,
							"",
							MsgBoxStyle.OkOnly,
							"Authorization")
			Exit Sub

		End If

	End Sub

	Private Sub AddNew(sender As Object, e As EventArgs) Handles btnAddNew.Click

		Try

			BindClassification()
			BindEngineDerate()
			SetObject()

			If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then

				MarkLog(Action.Save,
						"Flight Log",
						User.Identity.Name & " is not Authorized User to add ",
						ErrorType.HandledError,
						Guid.Empty,
						EventLogID)

				MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
								MSGBox.Message_Text.Authorization,
								"",
								MsgBoxStyle.OkOnly,
								"Authorization")
				Exit Sub

			End If

			If Not IsValid Then upnlErrorList.Update() : Exit Sub

			Session("IsSaveAndNew") = 1

			'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
			If IsValid Then

				If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then

					Dim Title As String = "Save Alert !"
					Dim Message As String = ""

					If Not mLog.PilotID1.Equals(Guid.Empty) Then

						Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)

						If mEmployeeStatus(0).Information <> "" Then
							Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information.ToString.Replace("Resource", "")
						End If

					End If

					If Not mLog.PilotID2.Equals(Guid.Empty) Then

						Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)

						If mEmployeeStatus(0).Information <> "" Then
							Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information.ToString.Replace("Resource", "")
						End If

					End If

					If Message.Length > 0 Then

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Custom,
										Message,
										MsgBoxStyle.OkOnly,
										"")

						Exit Sub

					End If

				End If

			End If
			'End

			'Added by Saylee on 18-May-2012 ALL17052012
			Dim mMaxLogOfAircraft As MaxLogOfAircraft
			mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

			If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then

				If Not (AppSettings("ClientCode") = "Heligo" Or
						AppSettings("ClientCode") = "UHPL" Or
						AppSettings("ClientCode") = "APFT" Or
						AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018

					Dim MaxLogDateTime As String = ""

					If mMachine.IsUTC Then
						MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
					Else
						MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
					End If

					If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012

						Session("New") = "True"
						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										" You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
												   MaxLogDateTime &
												   "<BR> <BR>Do you want to continue?",
										MsgBoxStyle.YesNo,
										"SaveLogFlexiLog")

						Exit Sub

					End If

				Else

					If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012

						Session("New") = "True"

						MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert,
										MSGBox.Message_Text.Alert,
										" You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " &
												   mMaxLogOfAircraft.LogDateFormatted &
												   "<BR> <BR>Do you want to continue?",
										MsgBoxStyle.YesNo,
										"SaveLogFlexiLog")

						Exit Sub

					End If

				End If

			End If
			'End

			'Added By Prashant 12-Apr-2010
			Dim IsMELCount As Boolean = False
			Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
			mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)

			For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1

				If mTempMELSnagCorrectiveActionList(i).IsMEL = True And mTempMELSnagCorrectiveActionList(i).DueDate.ToString <> "" Then   'Added By Prashant 23-Sep-2010

					If (CDate(calDateTime.Text) > CDate(mTempMELSnagCorrectiveActionList(i).DueDate)) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
						IsMELCount = True
						Exit For
					Else
						IsMELCount = False
					End If

				End If

			Next

			mTempMELSnagCorrectiveActionList = Nothing

			If IsMELCount = True Then

				MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MELNew")

				If IsValid Then

					SetObject()
					SetAirFrameGridObject()
					SetEngineGridObject(True)
					SetAPUGridObject(True)
					SetCGBGridObject(True)

				End If

				Exit Sub

			End If

			''Code Added By Deven for Save and New 20/03/2008
			If IsValid Then

				If Not GVLogFuelOilValidations() Then upnlErrorList.Update() : Exit Sub
				Session("New") = "True"

				If Save() = True Then

					NewRecord()
					Session.Remove("FileAttach")
					Session.Remove("IsAttachmentDeleted")
					DataFieldBind()
					Session("mLog") = mLog
					Session("mAircraftInformationBoardList") = Nothing

					EnableDisableButton()
					ControlVisibility()
					ControlVisibilityForAttachment()
					DataBindGrid()
					SetTitle()
					SetTakeoffTouchdownTitle()  'Added By Utkarsh On 31-Aug-2011
					SetFromAutoComplete() 'Added By Utkarsh On 24-Aug-2011

					mLog.LogPageNo = txtLogPageNo.Text.Trim  'Added By Utkarsh On 28-Nov-2011
					mLog.FlightNo = txtFlightNo.Text.Trim     'Added By Utkarsh On 28-Nov-2011
					mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)

					Session("mLogListOnDate") = mLogListOnDate

					If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate

						ScriptManager.RegisterStartupScript(Me,
															[GetType],
															"ShowLastDet",
															"ShowLastDet();",
															True)

						upnlLogInfo.Update()

					End If

					'Added By Vikrant on 01-Dec-2021 for PBH
					If Session("IsAircraftMadeNotInUse") IsNot Nothing Then

						If Session("IsAircraftMadeNotInUse") = "True" Then

							Session.Remove("AircraftId")
							Session.Remove("IsAircraftMadeNotInUse")
							MSGBoxCtrl.Show("Alert!",
											"",
											"Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System",
											MsgBoxStyle.OkOnly,
											"AircraftMadeNotInUse")

							Exit Sub

						End If

					End If

					'End
					upnlLogDetails.Update()
					upnlFlightDetails.Update()
					upnlFlightSummary.Update()
					upnlAirframeDetail.Update()
					upnlEngineDetail.Update()
					upnlAPUDetail.Update()
					upnlCGBDetail.Update()
					upnlTabs.Update()
					upnlTabsNew.Update()

				End If

			Else
				upnlErrorList.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddPilot1(sender As Object, e As ImageClickEventArgs) Handles imgbtnPilot1.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx&Type=Pilot")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddPilot2(sender As Object, e As ImageClickEventArgs) Handles imgbtnPilot2.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx&Type=Pilot&AddType=1")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub AddPilots(sender As Object, e As ImageClickEventArgs) Handles btnAddPilots.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Dim mEmployee As Employee
			mEmployee = Employee.NewPilot()
			Session("mEmployee") = mEmployee

			Dim BackPage As String = CType(Request.QueryString("BackPage"), String)
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenPilotWindow(BackPage)", "OpenPilotWindow();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddArrivalPlace(sender As Object, e As ImageClickEventArgs) Handles imgbtnArrPlace.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx&Type=Place&AddType=2")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddDeparturePlace(sender As Object, e As ImageClickEventArgs) Handles imgbtnDepPlace.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx&Type=Place&AddType=3")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub AddPlace(sender As Object, e As ImageClickEventArgs) Handles btnAddPlace.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			Dim bkpage As String = CType(Request.QueryString("BackPage"), String)
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenPlaceWindow(bkpage)", "OpenPlaceWindow();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub FlightLogClassifications(sender As Object, e As ImageClickEventArgs) Handles btnFlightLogClassifications.Click

		Try

			Dim bkpage As String = CType(Request.QueryString("BackPage"), String)
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenFlightLogClassificationWindow(bkpage)", "OpenFlightLogClassificationWindow();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FlightLogClassification_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFlightLogClassification.SelectedIndexChanged

		Try

			mLog.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
			mLog.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HdnImgBtnFlightLogClassification_Click(sender As Object, e As EventArgs) Handles hdnimgBtnFlightLogClassification.Click 'Added by Saylee On 25-Nov-2014

		Try

			BindClassification()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click

		Try

			ViewImage()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CalDateTimeChanged(sender As Object, e As EventArgs) Handles calDateTime.TextChanged

		Try

			If IsPostBack Then         'Added Code on May,29,2007

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))

					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calDateTime") IsNot Nothing Then
							calDateTime.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))
						Else
							calDateTime.Text = Format(Today.Date, AppSettings("DateFormat"))
						End If

					Else
						calDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calDateTime") = calDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calDateTime") IsNot Nothing Then
						calDateTime.Text = Format(CDate(ViewState("calDateTime")), AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calDateTime.Text = Format(Today.Date, AppSettings("DateFormat"))  'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

					CalDateTimeChanged(calDateTime.Text, e)  'Raising Text Change Event for further calculation
					Exit Sub

				End Try
				'# End

				'CNDC
				'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), SmartDate.StringToDate(calDateTime.Text)) <> 0 Then
				If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), New SmartDate(calDateTime.Text.ToString).Date) <> 0 Then

					REM: Clone the object
					Dim clnLog As Log
					clnLog = CType(mLog.Clone, Log)
					If mLog.IsNew Then

						Dim dtString As DateTime = CType(calDateTime.Text.ToString.Trim + " " + "23:59", DateTime)

						If (AppSettings("ClientCode") = "Heligo" Or
							AppSettings("ClientCode") = "APFT" Or
							AppSettings("ClientCode") = "AAP") Then
							NewRecord(calDateTime.Text.ToString)
						Else
							NewRecord(calDateTime.Text.ToString, , dtString.ToString)
						End If

						Session.Remove("FileAttach")
						Session.Remove("IsAttachmentDeleted")

					Else

						'CNDC
						EditRecord(calDateTime.Text.ToString)

					End If

					REM: Copy from Clone
					CopyFromClone(clnLog, True) 'Changed By Utkarsh On 13-Sep-2011

					DataFieldBind()

				End If

				SetTitle() 'Added By Utkarsh on 24-Nov-2011 For Title
				'Added By utkarsh ON 30-sep-2013 for Log_ajax changes
				EnableDisableButton()
				ControlVisibility()
				ControlVisibilityForAttachment()
				upnlFileupload.Update()
				'End

				If mLog.IsNew Then

					txtDepartureTime.Text = "00:00"
					txtUTCDepartureTime.Text = "00:00"
					txtArrivalTime.Text = "00:00"
					txtUTCArrivalTime.Text = "00:00"
					txtTouchDownLocalTime.Text = "00:00"
					txtUTCTouchDownTime.Text = "00:00"
					txtTakeOffLocalTime.Text = "00:00"
					txtUTCTakeOffTime.Text = "00:00"

				End If

				If (AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True") Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate

					mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
					Session("mLogListOnDate") = mLogListOnDate

					If mLogListOnDate.Count > 0 And mLog.IsNew Then

						Dim str1 As String
						str1 = "delete_cookie();"
						ScriptManager.RegisterStartupScript(Me, [GetType], Guid.NewGuid.ToString, str1, True)
						ScriptManager.RegisterStartupScript(Me, [GetType], "ShowLastDet", "ShowLastDet();", True)
						upnlLogInfo.Update()

					End If

					upnlLogDetails.Update()

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CalArrivalChanged(sender As Object, e As EventArgs) Handles calArrival.TextChanged

		Try

			If IsPostBack Then  'Added Code on May,29,2007

				If Trim(calArrival.Text) = "" Then
					ViewState("calArrival") = calDateTime.Text.Trim
					Exit Sub
				End If

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calArrival.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calArrival") IsNot Nothing Then
							calArrival.Text = Format(CDate(ViewState("calArrival")), AppSettings("DateFormat"))
						Else
							calArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calArrival.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calArrival") = calArrival.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calArrival") IsNot Nothing Then
						calArrival.Text = Format(CDate(ViewState("calArrival")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

					CalArrivalChanged(calArrival.Text, e)  'Raising textchange event for further calculation
					Exit Sub

				End Try
				'# End

				'CNDC
				If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(calArrival.Text.ToString).Date) <> 0) Or
					(calDeparture.Text = "") Then
					mLog.DesLocalDateTime = CType(calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim, DateTime) 'calArrival.Text.Trim
					Session("mLog") = mLog
				End If

				txtAirBorneTime.DataBind()
				RefreshControlValues()
				txtArrivalTime.Focus() 'SetFocus after databind
				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CalDepartureChanged(sender As Object, e As EventArgs) Handles calDeparture.TextChanged

		Try

			If IsPostBack Then

				If Trim(calDeparture.Text) = "" Then
					ViewState("CalDeparture") = calDateTime.Text.Trim
					Exit Sub
				End If
				'# Date Control Validation #

				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calDeparture.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("CalDeparture") IsNot Nothing Then
							calDeparture.Text = Format(CDate(ViewState("CalDeparture")), AppSettings("DateFormat"))
						Else
							calDeparture.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calDeparture.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("CalDeparture") = calDeparture.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("CalDeparture") IsNot Nothing Then
						calDeparture.Text = Format(CDate(ViewState("CalDeparture")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calDeparture.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

					CalDepartureChanged(calDeparture.Text, e)  'Raising textchange event for further calculation
					Exit Sub

				End Try
				'# End

				'CNDC
				If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(calDeparture.Text.ToString).Date) <> 0) Or
				   (calArrival.Text = "") Then

					REM: Clone the object
					Dim clnLog As Log
					clnLog = CType(mLog.Clone, Log)

					'CNDC
					clnLog.SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime) ' calDeparture.Text.ToString.Trim

					If mLog.IsNew Then

						'CNDC
						NewRecord(calDateTime.Text.ToString, CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime).ToString)
						Session.Remove("FileAttach")
						Session.Remove("IsAttachmentDeleted")

					Else

						'CNDC
						EditRecord(New SmartDate(calDeparture.Text.ToString).Date)

					End If

					REM: Copy from Clone
					CopyFromClone(clnLog)
					mLog.DesLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime) 'calDeparture.Text.ToString.Trim

					'Added By Utkarsh On 31-Aug-2011
					If TakeOffTouchDown Then
						mLog.TouchDownLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime) 'calDeparture.Text.ToString.Trim
					End If
					Session("mLog") = mLog
					'End
					DataFieldBind()
					ControlVisibilityForAttachment()
					txtDepartureTime.Focus() 'SetFocus after databind

				End If

				'Added By Utkarsh On 31-Aug-2011
				If TakeOffTouchDown Then
					ViewState("calTakeOffLocalDateTime") = calDeparture.Text.Trim
					ViewState("calTouchDownLocalDateTime") = calDeparture.Text.Trim
					calTakeOffLocalDateTime.Text = calDeparture.Text.Trim
					calTouchDownLocalDateTime.Text = calDeparture.Text.Trim
				End If

				ViewState("calArrival") = calDeparture.Text.Trim
				calArrival.Text = calDeparture.Text.Trim
				'End

				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()
				upnlFileupload.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CalUTCArrival_TextChanged(sender As Object, e As EventArgs) Handles CalUTCArrival.TextChanged

		Try

			If IsPostBack Then         'Added Code on May,29,2007

				If Trim(CalUTCArrival.Text) = "" Then
					ViewState("CalUTCArrival") = calDateTime.Text.Trim
					Exit Sub
				End If

				Try
					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(CalUTCArrival.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("CalUTCArrival") IsNot Nothing Then
							CalUTCArrival.Text = Format(CDate(ViewState("CalUTCArrival")), AppSettings("DateFormat"))
						Else
							CalUTCArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						CalUTCArrival.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("CalUTCArrival") = CalUTCArrival.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("CalUTCArrival") IsNot Nothing Then
						CalUTCArrival.Text = Format(CDate(ViewState("CalUTCArrival")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						CalUTCArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

					CalUTCArrival_TextChanged(CalUTCArrival.Text, e)  'Raising textchange event for further calculation
					Exit Sub

				End Try
				'# End

				'CNDC
				If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(CalUTCArrival.Text.ToString).Date) <> 0) Or
				   (CalUTCDateTime.Text = "") Then

					mLog.DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime) 'CalUTCArrival.Text.Trim
					Session("mLog") = mLog

				End If

				If calDeparture.Text.ToString = "" Then

					mLog.SouLocalDateTime = ""
					mLog.DesLocalDateTime = ""
					calArrival.Enabled = False
					calArrival.ReadOnly = True
					calArrival.BackColor = Color.Gainsboro
					txtAirBorneTime.ReadOnly = True
					txtAirBorneTime.BackColor = Color.Gainsboro

				Else
					calArrival.Enabled = True
				End If

				txtAirBorneTime.DataBind()
				RefreshControlValues()
				txtArrivalTime.Focus() 'SetFocus after databind
				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CalUTCDateTime_TextChanged(sender As Object, e As EventArgs) Handles CalUTCDateTime.TextChanged

		Try

			If IsPostBack Then         'Added Code on May,29,2007

				If Trim(CalUTCDateTime.Text) = "" Then
					ViewState("CalUTCDateTime") = calDateTime.Text.Trim
					Exit Sub
				End If

				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(CalUTCDateTime.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("CalUTCDateTime") IsNot Nothing Then
							CalUTCDateTime.Text = Format(CDate(ViewState("CalUTCDateTime")), AppSettings("DateFormat"))
						Else
							CalUTCDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						CalUTCDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("CalUTCDateTime") = CalUTCDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("CalUTCDateTime") IsNot Nothing Then
						CalUTCDateTime.Text = Format(CDate(ViewState("CalUTCDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						CalUTCDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

					CalUTCDateTime_TextChanged(CalUTCDateTime.Text, e)  'Raising textchange event for further calculation
					Exit Sub

				End Try
				'# End

				'CNDC
				If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), New SmartDate(CalUTCDateTime.Text.ToString).Date) <> 0) Or
				   (CalUTCArrival.Text = "") Then

					REM: Clone the object
					Dim clnLog As Log
					clnLog = CType(mLog.Clone, Log)
					Dim dtString As DateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)

					'CNDC
					clnLog.SouUniverseDateTime = dtString  'CalUTCDateTime.Text.ToString.Trim

					If mLog.IsNew Then
						'CNDC
						NewRecord(calDateTime.Text.ToString, , dtString.ToString)
						Session.Remove("FileAttach")
						Session.Remove("IsAttachmentDeleted")
					Else
						'CNDC
						EditRecord(New SmartDate(CalUTCDateTime.Text.ToString).Date)
					End If

					REM: Copy from Clone
					CopyFromClone(clnLog)
					mLog.DesUniverseDateTime = dtString

					If TakeOffTouchDown Then
						mLog.TouchDownUniverseDateTime = dtString  'CalUTCDateTime.Text.ToString.Trim 'Added By Utkarsh On 31-Aug-2011
					End If

					Session("mLog") = mLog
					DataFieldBind()
					ControlVisibilityForAttachment()
					'DataBind() 'Hobbs - taken
					txtDepartureTime.Focus() 'SetFocus after databind

				End If

				'Added By Utkarsh On 31-Aug-2011
				If TakeOffTouchDown Then

					ViewState("calUTCTakeOffDateTime") = CalUTCDateTime.Text.Trim
					ViewState("calUTCTouchDownDateTime") = CalUTCDateTime.Text.Trim

					calUTCTakeOffDateTime.Text = CalUTCDateTime.Text.Trim
					calUTCTouchDownDateTime.Text = CalUTCDateTime.Text.Trim

				End If

				ViewState("CalUTCArrival") = CalUTCDateTime.Text.Trim
				CalUTCArrival.Text = CalUTCDateTime.Text.Trim
				'End

				If calDeparture.Text.ToString = "" Then

					mLog.SouLocalDateTime = ""
					mLog.DesLocalDateTime = ""
					calArrival.Enabled = False
					calArrival.ReadOnly = True
					calArrival.BackColor = Color.Gainsboro
					txtAirBorneTime.ReadOnly = True
					txtAirBorneTime.BackColor = Color.Gainsboro

				Else
					calArrival.Enabled = True
				End If

				RefreshControlValues()

				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()
				upnlFileupload.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TakeOffLocalDateTimeChanged(sender As Object, e As EventArgs) Handles calTakeOffLocalDateTime.TextChanged

		Try

			If IsPostBack Then

				If Trim(calTakeOffLocalDateTime.Text) = "" Then
					ViewState("calTakeOffLocalDateTime") = calDateTime.Text.Trim
					Exit Sub
				End If

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calTakeOffLocalDateTime.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calTakeOffLocalDateTime") IsNot Nothing Then
							calTakeOffLocalDateTime.Text = Format(CDate(ViewState("calTakeOffLocalDateTime")), AppSettings("DateFormat"))
						Else
							calTakeOffLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calTakeOffLocalDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calTakeOffLocalDateTime") = calTakeOffLocalDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calTakeOffLocalDateTime") IsNot Nothing Then
						calTakeOffLocalDateTime.Text = Format(CDate(ViewState("calTakeOffLocalDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calTakeOffLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

				End Try
				'# End

				If calTakeOffLocalDateTime.Text.ToString = "" Then
					mLog.TakeOffLocalDateTime = ""
					mLog.TouchDownLocalDateTime = ""
				End If

				RefreshControlValues()
				txtTakeOffLocalTime.Focus() 'SetFocus after databind

				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TouchDownLocalDateTimeChanged(sender As Object, e As EventArgs) Handles calTouchDownLocalDateTime.TextChanged

		Try

			If IsPostBack Then

				If Trim(calTouchDownLocalDateTime.Text) = "" Then
					ViewState("calTouchDownLocalDateTime") = calDateTime.Text.Trim
					Exit Sub
				End If

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calTouchDownLocalDateTime.Text.Trim), AppSettings("DateFormat"))
					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calTouchDownLocalDateTime") IsNot Nothing Then
							calTouchDownLocalDateTime.Text = Format(CDate(ViewState("calTouchDownLocalDateTime")), AppSettings("DateFormat"))
						Else
							calTouchDownLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calTouchDownLocalDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calTouchDownLocalDateTime") = calTouchDownLocalDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calTouchDownLocalDateTime") IsNot Nothing Then
						calTouchDownLocalDateTime.Text = Format(CDate(ViewState("calTouchDownLocalDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calTouchDownLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

				End Try
				'# End

				calTouchDownLocalDateTime.Text = Format(CDate(calTouchDownLocalDateTime.Text.ToString.Trim), AppSettings("DateFormat")) 'Added By Utkarsh On 30-Aug-2011
				mLog.TouchDownLocalDateTime = CType(calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim, DateTime) 'calTouchDownLocalDateTime.Text.Trim
				Session("mLog") = mLog
				txtAirBorneTime.DataBind()
				RefreshControlValues()
				txtTouchDownLocalTime.Focus() 'SetFocus after databind
				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCTakeOffDateTimeChanged(sender As Object, e As EventArgs) Handles calUTCTakeOffDateTime.TextChanged

		Try

			If IsPostBack Then

				If Trim(calUTCTakeOffDateTime.Text) = "" Then
					ViewState("calUTCTakeOffDateTime") = calDateTime.Text.Trim
					Exit Sub
				End If

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calUTCTakeOffDateTime.Text.Trim), AppSettings("DateFormat"))

					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calUTCTakeOffDateTime") IsNot Nothing Then
							calUTCTakeOffDateTime.Text = Format(CDate(ViewState("calUTCTakeOffDateTime")), AppSettings("DateFormat"))
						Else
							calUTCTakeOffDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calUTCTakeOffDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calUTCTakeOffDateTime") = calUTCTakeOffDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calUTCTakeOffDateTime") IsNot Nothing Then
						calUTCTakeOffDateTime.Text = Format(CDate(ViewState("calUTCTakeOffDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calUTCTakeOffDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

				End Try
				'# End

				RefreshControlValues()
				txtUTCTakeOffTime.Focus() 'SetFocus after databind
				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCTouchDownDateTimeChanged(sender As Object, e As EventArgs) Handles calUTCTouchDownDateTime.TextChanged

		Try

			If IsPostBack Then

				If Trim(calUTCTouchDownDateTime.Text) = "" Then
					ViewState("calUTCTouchDownDateTime") = calDateTime.Text.Trim
					Exit Sub
				End If

				'# Date Control Validation #
				Try

					Dim tempdate As DateTime
					Dim Datestring As String = Format(CDate(calUTCTouchDownDateTime.Text.Trim), AppSettings("DateFormat"))

					tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()

					If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)

						If ViewState("calUTCTouchDownDateTime") IsNot Nothing Then
							calUTCTouchDownDateTime.Text = Format(CDate(ViewState("calUTCTouchDownDateTime")), AppSettings("DateFormat"))
						Else
							calUTCTouchDownDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
						End If

					Else
						calUTCTouchDownDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
					End If

					ViewState("calUTCTouchDownDateTime") = calUTCTouchDownDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction

				Catch ex As Exception

					If ViewState("calUTCTouchDownDateTime") IsNot Nothing Then
						calUTCTouchDownDateTime.Text = Format(CDate(ViewState("calUTCTouchDownDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					Else
						calUTCTouchDownDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
					End If

				End Try
				'# End
				calUTCTouchDownDateTime.Text = Format(CDate(calUTCTouchDownDateTime.Text.ToString.Trim), AppSettings("DateFormat")) 'Added By Utkarsh On 30-Aug-2011
				mLog.TouchDownUniverseDateTime = CType(calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim, DateTime) ' calUTCTouchDownDateTime.Text.Trim
				Session("mLog") = mLog

				RefreshControlValues()
				txtUTCTouchDownTime.Focus() 'SetFocus after databind
				upnlFlightSummary.Update()
				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FuelOil_Click(sender As Object, e As EventArgs) Handles btnFuelOil.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			Session("OpenFromWO") = False
			Session("mOpenFromLogFuelNew") = False

			Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DefectActionList_Click(sender As Object, e As EventArgs) Handles btnDefectActionList.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			Session("Edit") = False

			Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub LogPax_Click(sender As Object, e As EventArgs) Handles btnLogPax.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			NewLogPax()

			Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub HobbsOffset_Click(sender As Object, e As EventArgs) Handles btnHobbsOffset.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			NewHobbsOffSet()

			Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ParameterList_Click(sender As Object, e As EventArgs) Handles btnParameterList.Click

		Try

			'Code Added By Deven 21-03-2008 
			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)
			Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub FlightCrew_Click(sender As Object, e As EventArgs) Handles btnFlightCrew.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub MaintenanceAcitvity_Click(sender As Object, e As EventArgs) Handles btnMaintenanceAcitvity.Click

		Try

			SetObject()
			SetAirFrameGridObject()
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOP_Ajax.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ShowAllAssemblies(sender As Object, e As EventArgs) Handles lnkAllAssembly.Click

		Try

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Show All Assemblies",
												"ShowAllAssemblies();",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

	Private Sub EngineDerateChanged(sender As Object, e As EventArgs) Handles ddlEngineDerate.SelectedIndexChanged

		Try

			mLog.EngineDerateID = CInt(ddlEngineDerate.SelectedValue.ToString)
			mLog.EngineDerateValue = ddlEngineDerate.SelectedItem.Text
			Session("mLog") = mLog

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Air Frame Grid Textbox's Change Event "

	Protected Sub AirFrameTextChanged(sender As Object, e As EventArgs)

		Try

			Dim txtBox As TextBox = CType(sender, TextBox)
			Dim currentRow As GridViewRow = CType(txtBox.Parent.Parent, GridViewRow)
			Dim rowIndex As Integer = currentRow.RowIndex
			Dim afAssembly = mLog.LogAFAssemblies(rowIndex)

			Select Case txtBox.ID
				Case "txtAirFrameHours"
					afAssembly.Hours = Trim(txtBox.Text)
				Case "txtAirFrameLandings"
					afAssembly.Landings = Trim(txtBox.Text)
				Case "txtAirFrameCycles"
					afAssembly.Cycles = Trim(txtBox.Text)
				Case "txtAirFrameStarts"
					afAssembly.Starts = Trim(txtBox.Text)
				Case "txtAirFrameNGCycles"
					afAssembly.NGCycles = Trim(txtBox.Text)
				Case "txtAirFrameNFCycles"
					afAssembly.NFCycles = Trim(txtBox.Text)
				Case "txtAirFrameRins"
					afAssembly.RINS = Trim(txtBox.Text)
				Case "txtAirFrameBleeds"
					afAssembly.Bleeds = Trim(txtBox.Text)
				Case "txtAirFrameImpellerCycles"
					afAssembly.ImpellerCycles = Trim(txtBox.Text)
				Case "txtAirFrameCTCycles"
					afAssembly.CTCycles = Trim(txtBox.Text)
				Case "txtAirFramePTCycles"
					afAssembly.PTCycles = Trim(txtBox.Text)
				Case "txtAirFrameGeneratorMods"
					afAssembly.GeneratorMods = Trim(txtBox.Text)
				Case "txtAirframeNRCycles"
					afAssembly.NRCycles = Trim(txtBox.Text)
				Case "txtAirframeLandingCycles"
					afAssembly.LandingCycles = Trim(txtBox.Text)
				Case "txtAirframeLandingGearCycles"
					afAssembly.LandingGearCycles = Trim(txtBox.Text)
				Case "txtAirframeOverSpeedLHMLGCycles"
					afAssembly.OverSpeedLHMLGCycles = Trim(txtBox.Text)
				Case "txtAirframeOverSpeedRHMLGCycles"
					afAssembly.OverSpeedRHMLGCycles = Trim(txtBox.Text)
				Case "txtAirframeOverSpeedNLGCycles"
					afAssembly.OverSpeedNLGCycles = Trim(txtBox.Text)
				Case "txtAirframeMGBTorqueCycles"
					afAssembly.MGBTorqueCycles = Trim(txtBox.Text)
				Case "txtAirframeRotorBrakeCycles"
					afAssembly.RotorBrakeCycles = Trim(txtBox.Text)
			End Select

			DataBindGrid()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Engine Grid Textbox's Change Event "

	Protected Sub EngineTextChanged(sender As Object, e As EventArgs)

		Try

			Dim txtBox As TextBox = CType(sender, TextBox)
			Dim currentRow As GridViewRow = CType(txtBox.Parent.Parent, GridViewRow)
			Dim rowIndex As Integer = currentRow.RowIndex
			Dim engAssembly = mLog.LogEngAssemblies(rowIndex)

			Select Case txtBox.ID
				Case "txtEngineHours"
					engAssembly.Hours = Trim(txtBox.Text)
				Case "txtEngineLandings"
					engAssembly.Landings = Trim(txtBox.Text)
				Case "txtEngineCycles"
					engAssembly.Cycles = Trim(txtBox.Text)
				Case "txtEngineStarts"
					engAssembly.Starts = Trim(txtBox.Text)
				Case "txtEngineNGCycles"
					engAssembly.NGCycles = Trim(txtBox.Text)
				Case "txtEngineNFCycles"
					engAssembly.NFCycles = Trim(txtBox.Text)
				Case "txtEngineRins"
					engAssembly.RINS = Trim(txtBox.Text)
				Case "txtEngineCFactors"
					engAssembly.CFactor = Trim(txtBox.Text)
				Case "txtEngineBleeds"
					engAssembly.Bleeds = Trim(txtBox.Text)
				Case "txtEngineImpellerCycles"
					engAssembly.ImpellerCycles = Trim(txtBox.Text)
				Case "txtEngineCTCycles"
					engAssembly.CTCycles = Trim(txtBox.Text)
				Case "txtEnginePTCycles"
					engAssembly.PTCycles = Trim(txtBox.Text)
				Case "txtEngineGeneratorMods"
					engAssembly.GeneratorMods = Trim(txtBox.Text)
				Case "txtEngineRapidTakeOffFactor"
					engAssembly.RapidTakeOffFactor = Trim(txtBox.Text)
				Case "txtEngineN1Cycles"
					engAssembly.N1Cycles = Trim(txtBox.Text)
				Case "txtEngineN2Cycles"
					engAssembly.N2Cycles = Trim(txtBox.Text)
			End Select

			DataBindGrid()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " APU Grid Textbox's Change Event "

	Protected Sub APUTextChanged(sender As Object, e As EventArgs)

		Try

			Dim txtBox As TextBox = CType(sender, TextBox)
			Dim currentRow As GridViewRow = CType(txtBox.Parent.Parent, GridViewRow)
			Dim rowIndex As Integer = currentRow.RowIndex
			Dim apuAssembly = mLog.LogAPUAssemblies(rowIndex)

			Select Case txtBox.ID
				Case "txtAPUHours"
					apuAssembly.Hours = Trim(txtBox.Text)
				Case "txtAPULandings"
					apuAssembly.Landings = Trim(txtBox.Text)
				Case "txtAPUCycles"
					apuAssembly.Cycles = Trim(txtBox.Text)
				Case "txtAPUStarts"
					apuAssembly.Starts = Trim(txtBox.Text)
				Case "txtAPUNGCycles"
					apuAssembly.NGCycles = Trim(txtBox.Text)
				Case "txtAPUNFCycles"
					apuAssembly.NFCycles = Trim(txtBox.Text)
				Case "txtAPURins"
					apuAssembly.RINS = Trim(txtBox.Text)
				Case "txtAPUBleeds"
					apuAssembly.Bleeds = Trim(txtBox.Text)
				Case "txtAPUImpellerCycles"
					apuAssembly.ImpellerCycles = Trim(txtBox.Text)
				Case "txtAPUCTCycles"
					apuAssembly.CTCycles = Trim(txtBox.Text)
				Case "txtAPUPTCycles"
					apuAssembly.PTCycles = Trim(txtBox.Text)
				Case "txtAPUGeneratorMods"
					apuAssembly.GeneratorMods = Trim(txtBox.Text)
			End Select

			DataBindGrid()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " CGB Grid Textbox's Change Event "

	Protected Sub CGBTextChanged(sender As Object, e As EventArgs)

		Try

			Dim txtBox As TextBox = CType(sender, TextBox)
			Dim currentRow As GridViewRow = CType(txtBox.Parent.Parent, GridViewRow)
			Dim rowIndex As Integer = currentRow.RowIndex
			Dim cgbAssembly = mLog.LogCGBAssemblies(rowIndex)

			Select Case txtBox.ID
				Case "txtCGBHours"
					cgbAssembly.Hours = Trim(txtBox.Text)
				Case "txtCGBLandings"
					cgbAssembly.Landings = Trim(txtBox.Text)
				Case "txtCGBCycles"
					cgbAssembly.Cycles = Trim(txtBox.Text)
				Case "txtCGBStarts"
					cgbAssembly.Starts = Trim(txtBox.Text)
				Case "txtCGBNGCycles"
					cgbAssembly.NGCycles = Trim(txtBox.Text)
				Case "txtCGBNFCycles"
					cgbAssembly.NFCycles = Trim(txtBox.Text)
				Case "txtCGBRINS"
					cgbAssembly.RINS = Trim(txtBox.Text)
				Case "txtCGBBleeds"
					cgbAssembly.Bleeds = Trim(txtBox.Text)
				Case "txtCGBImpellerCycles"
					cgbAssembly.ImpellerCycles = Trim(txtBox.Text)
				Case "txtCGBCTCycles"
					cgbAssembly.CTCycles = Trim(txtBox.Text)
				Case "txtCGBPTCycles"
					cgbAssembly.PTCycles = Trim(txtBox.Text)
				Case "txtCGBGeneratorMods"
					cgbAssembly.GeneratorMods = Trim(txtBox.Text)
			End Select

			DataBindGrid()

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Refresh Controls Values "

	Private Sub RefreshControlValues(Optional isDatabindFromAirborn As Boolean = False)
		'If Not IsValid Then Exit Sub

		SetObject()
		SetAirFrameGridObject(isDatabindFromAirborn)
		SetEngineGridObject(isDatabindFromAirborn)
		SetAPUGridObject(isDatabindFromAirborn)
		SetCGBGridObject(isDatabindFromAirborn)

		DataFieldBind()

		Session("mLog") = mLog
		ControlVisibility()
		EnableDisableButton()
		DataBind()

	End Sub

#End Region

#Region " Methods "

	Private Sub MsgBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	Protected Sub PercentTimeOnGround_TextChanged(sender As Object, e As EventArgs) Handles txtPercentTimeOnGround.TextChanged, txtCurrentHobbsValue.TextChanged

		Try
			RefreshControlValues()
		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub AirBorneTime_TextChanged(sender As Object, e As EventArgs) Handles txtAirBorneTime.TextChanged

		Try

			If Not TakeOffTouchDown Or
			   mLog.IsLogAirborneEntry = True Or
			   (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True

				mLog.TimeInAir = Trim(txtAirBorneTime.Text)
				Session("mLog") = mLog

			End If

			RefreshControlValues(True)
			txtGroundRunTime.Focus()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Protected Sub BlockTime_TextChanged(sender As Object, e As EventArgs) Handles txtBlockTime.TextChanged

		Try

			If Not TakeOffTouchDown Or
			   mLog.IsLogAirborneEntry = True Or
			   AppSettings("ClientCode") = "Heligo" Or
			   AppSettings("ClientCode") = "APFT" Or
			   AppSettings("ClientCode") = "AAP" Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True

				mLog.TimeInAir = Trim(txtAirBorneTime.Text)
				Session("mLog") = mLog

			End If

			RefreshControlValues(False)
			txtAirBorneTime.Focus()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	'Added by Utkarsh on 03-oct-2013 for log_ajax changes
	Protected Sub GroundRunTime_TextChanged(sender As Object, e As EventArgs) Handles txtGroundRunTime.TextChanged

		Try

			If Not AppSettings("Log") = "True" Or mLog.IsLogAirborneEntry = True Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True
				mLog.TimeOnGround = Trim(txtGroundRunTime.Text)
				Session("mLog") = mLog
			End If

			RefreshControlValues(True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub
	'End

	Private Sub FileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014

		Try

			If (AppSettings("ClientCode") = "Heligo" Or
				AppSettings("ClientCode") = "UHPL" Or
				AppSettings("ClientCode") = "APFT" Or
				AppSettings("ClientCode") = "AAP") Then

				AttachMyFile()
				upnlLogAttachment.Update()

			Else
				mLog.IsAttachmentAdded = True
				ControlVisibilityForAttachment()
				upnlFileupload.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DeleteAttachment(sender As Object, e As EventArgs) Handles btnDelAttch.Click

		Try

			Dim fileSize1 As Integer = 0
			Dim file1(fileSize1) As Byte

			GetAttachment()

			FileAttach.ImageFile = file1
			FileAttach.Size = 0
			ImageButton1.Visible = False
			btnDelAttch.Enabled = False
			IsAttachmentDeleted = True
			mLog.IsAttachmentAdded = False
			Session("IsAttachmentDeleted") = IsAttachmentDeleted

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SelectFile_ServerClick(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

		Try

			If mLog.IsAttachmentAdded Then
				FileAttach = FileAttach.GetAttachment(mLog.ID)
			Else
				FileAttach = FileAttach.NewAttachment(Guid.NewGuid, mLog.ID)
			End If

			Session("mFileAttach") = FileAttach

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DepartureTime_TextChanged(sender As Object, e As EventArgs) Handles txtDepartureTime.TextChanged

		Try

			If IsValidTime(txtDepartureTime.Text.ToString.Trim) = False Then
				txtDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calDeparture.Text.ToString + " " + txtDepartureTime.Text.ToString.Trim

				If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
					mLog.SouLocalDateTime = DateTime
					mLog.TakeOffLocalDateTime = DateTime
					mLog.DesLocalDateTime = DateTime
					mLog.TouchDownLocalDateTime = DateTime

					'Added on 30-Dec-2019
					Dim clnLog As Log
					clnLog = CType(mLog.Clone, Log)

					If mLog.IsNew Then

						NewRecord(calDateTime.Text.ToString, CType(calDateTime.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime).ToString)
						Session.Remove("FileAttach")
						Session.Remove("IsAttachmentDeleted")
						CopyFromClone(clnLog, True)
					End If

					DataFieldBind()

					If TakeOffTouchDown Then
						chkTakeOff.Focus()
					Else

						If (Not (mMachine.IsUTC) And TakeOffTouchDown) Then
							calTakeOffLocalDateTime.Focus()
						Else
							calUTCTakeOffDateTime.Focus()
						End If

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCDepartureTime_TextChanged(sender As Object, e As EventArgs) Handles txtUTCDepartureTime.TextChanged

		Try

			If IsValidTime(txtUTCDepartureTime.Text.ToString.Trim) = False Then
				txtUTCDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = CalUTCDateTime.Text.ToString + " " + txtUTCDepartureTime.Text.ToString.Trim

				If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

					mLog.SouUniverseDateTime = DateTime
					mLog.TakeOffUniverseDateTime = DateTime
					mLog.DesUniverseDateTime = DateTime
					mLog.TouchDownUniverseDateTime = DateTime

					'Added on 30-Dec-2019
					Dim clnLog As Log
					clnLog = CType(mLog.Clone, Log)

					If mLog.IsNew Then
						NewRecord(calDateTime.Text.ToString, , CType(calDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime).ToString)
						Session.Remove("FileAttach")
						Session.Remove("IsAttachmentDeleted")
						CopyFromClone(clnLog, True)
					End If

					DataFieldBind()
					txtUTCTakeOffTime.Focus()

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TakeOffLocalTime_TextChanged(sender As Object, e As EventArgs) Handles txtTakeOffLocalTime.TextChanged

		Try

			If IsValidTime(txtTakeOffLocalTime.Text.ToString.Trim) = False Then
				txtTakeOffLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim
				mLog.TakeOffLocalDateTime = DateTime
				DataFieldBind()
				txtTouchDownLocalTime.Focus() 'SetFocus

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCTakeOffTime_TextChanged(sender As Object, e As EventArgs) Handles txtUTCTakeOffTime.TextChanged

		Try

			If IsValidTime(txtUTCTakeOffTime.Text.ToString.Trim) = False Then
				txtUTCTakeOffTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim
				mLog.TakeOffUniverseDateTime = DateTime
				DataFieldBind()
				txtUTCTouchDownTime.Focus()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ArrivalTime_TextChanged(sender As Object, e As EventArgs) Handles txtArrivalTime.TextChanged

		Try

			If IsValidTime(txtArrivalTime.Text.ToString.Trim) = False Then
				txtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

				If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

					mLog.DesLocalDateTime = DateTime
					DataFieldBind()
					DataBindGrid()

					If TakeOffTouchDown Then
						chkTouchDown.Focus()
					Else

						If (Not (mMachine.IsUTC) And TakeOffTouchDown) Then
							calTouchDownLocalDateTime.Focus()
						Else
							calUTCTouchDownDateTime.Focus()
						End If

					End If

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCArrivalTime_TextChanged(sender As Object, e As EventArgs) Handles txtUTCArrivalTime.TextChanged

		Try

			If IsValidTime(txtUTCArrivalTime.Text.ToString.Trim) = False Then
				txtUTCArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim
				If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

					mLog.DesUniverseDateTime = DateTime
					DataFieldBind()
					DataBindGrid()
					txtAirBorneTime.Focus()

				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TouchDownLocalTime_TextChanged(sender As Object, e As EventArgs) Handles txtTouchDownLocalTime.TextChanged

		Try

			If IsValidTime(txtTouchDownLocalTime.Text.ToString.Trim) = False Then
				txtTouchDownLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim
				mLog.TouchDownLocalDateTime = DateTime
				DataFieldBind()
				DataBindGrid()
				txtArrivalTime.Focus() 'SetFocus

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub UTCTouchDownTime_TextChanged(sender As Object, e As EventArgs) Handles txtUTCTouchDownTime.TextChanged

		Try

			If IsValidTime(txtUTCTouchDownTime.Text.ToString.Trim) = False Then
				txtUTCTouchDownTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
			Else

				Dim DateTime As String = calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim
				mLog.TouchDownUniverseDateTime = DateTime
				DataFieldBind()
				DataBindGrid()
				txtUTCArrivalTime.Focus()

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub Arrival_CheckedChanged(sender As Object, e As EventArgs) Handles chkArrival.CheckedChanged

		Try

			If (mMachine.IsUTC) Then

				If chkArrival.Checked Then
					CalUTCArrival.ReadOnly = False
					CalUTCArrival.BackColor = Color.White
					CalUTCArrival_CalendarExtender.Enabled = True
				Else
					CalUTCArrival.ReadOnly = True
					CalUTCArrival.BackColor = Color.Gainsboro
					CalUTCArrival_CalendarExtender.Enabled = False
				End If

			Else

				If chkArrival.Checked Then
					calArrival.ReadOnly = False
					calArrival.BackColor = Color.White
					calArrival_CalendarExtender.Enabled = True
				Else
					calArrival.ReadOnly = True
					calArrival.BackColor = Color.Gainsboro
					calArrival_CalendarExtender.Enabled = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TouchDown_CheckedChanged(sender As Object, e As EventArgs) Handles chkTouchDown.CheckedChanged

		Try

			If (mMachine.IsUTC) Then

				If chkTouchDown.Checked Then

					calUTCTouchDownDateTime.ReadOnly = False
					calUTCTouchDownDateTime.BackColor = Color.White
					calUTCTouchDownDateTime_CalendarExtender.Enabled = True

				Else

					calUTCTouchDownDateTime.ReadOnly = True
					calUTCTouchDownDateTime.BackColor = Color.Gainsboro
					calUTCTouchDownDateTime_CalendarExtender.Enabled = False

				End If

			Else

				If chkTouchDown.Checked Then
					calTouchDownLocalDateTime.ReadOnly = False
					calTouchDownLocalDateTime.BackColor = Color.White
					calTouchDownLocalDateTime_CalendarExtender.Enabled = True
				Else
					calTouchDownLocalDateTime.ReadOnly = True
					calTouchDownLocalDateTime.BackColor = Color.Gainsboro
					calTouchDownLocalDateTime_CalendarExtender.Enabled = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TakeOff_CheckedChanged(sender As Object, e As EventArgs) Handles chkTakeOff.CheckedChanged

		Try

			If (mMachine.IsUTC) Then

				If chkTakeOff.Checked Then
					calUTCTakeOffDateTime.ReadOnly = False
					calUTCTakeOffDateTime.BackColor = Color.White
					calUTCTakeOffDateTime_CalendarExtender.Enabled = True
				Else
					calUTCTakeOffDateTime.ReadOnly = True
					calUTCTakeOffDateTime.BackColor = Color.Gainsboro
					calUTCTakeOffDateTime_CalendarExtender.Enabled = False
				End If

			Else

				If chkTakeOff.Checked Then
					calTakeOffLocalDateTime.ReadOnly = False
					calTakeOffLocalDateTime.BackColor = Color.White
					calTakeOffLocalDateTime_CalendarExtender.Enabled = True
				Else
					calTakeOffLocalDateTime.ReadOnly = True
					calTakeOffLocalDateTime.BackColor = Color.Gainsboro
					calTakeOffLocalDateTime_CalendarExtender.Enabled = False
				End If

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddSelectFiles(sender As Object, e As ImageClickEventArgs) Handles btnSelectFiles.Click

		Try

			SetObject()
			Session("mLog") = mLog
			ScriptManager.RegisterStartupScript(Me, [GetType], "OpenFileUploadWindow", "OpenFileUploadWindow();", True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub hdnBtnDiscrepancyTroubleShoot1_Click(sender As Object, e As EventArgs) Handles hdnBtnDiscrepancyTroubleShoot1.Click

		ScriptManager.RegisterStartupScript(Me, [GetType], "callDeferredDiscrepancies", "callDeferredDiscrepancies()", True)

	End Sub

	Private Sub DiscrepancyOrCabinDefectDetail(sender As Object, e As EventArgs) Handles hdnBtnDiscrepancyDetail.Click

		Try

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Discrepancy Reporting",
												"callDiscrepancyReporting()",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CabinDefectReporting(sender As Object, e As EventArgs) Handles hdnBtnCabinfDefectDetail.Click

		Try

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												"Cabin Defect Reporting",
												"ShowCabinDefectList()",
												True)

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Helper Method(s) "

	Public Function SetDiscrepancyDetailsForLog(Log As Log, ModuleName As String) As Log

		Dim DetailPageScript, ListPageScript, ModuleNameForMarkLog As String
		Try

			If Log.MELSnagCorrectiveActions.Count = 0 Then

				DiscrepancyCorrectiveAction = MELSnagCorrectiveAction.NewMELSnagCorrectiveAction()
				DiscrepancyCorrectiveAction.LogID = Log.ID
				DiscrepancyCorrectiveAction.DateOfOccurrence = Log.DateFormatted.ToString
				DiscrepancyCorrectiveAction.MachineID = Log.MachineID
				DiscrepancyCorrectiveAction.AssemblyStatusID = mMachine.AssemblyStatus.ID

				Dim MELSnagCorrectiveActionLog As MELSnagCorrectiveActionLog
				MELSnagCorrectiveActionLog = MELSnagCorrectiveActionLog.GetMELSnagCorrectiveActionLog(LogID:=DiscrepancyCorrectiveAction.LogID.ToString)
				Dim FileAttach As FileAttach = FileAttach.NewAttachment(ID:=Guid.Empty,
																		ReferenceID:=DiscrepancyCorrectiveAction.ID)

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

				Session("DiscrepancyCorrectiveActionList") = DiscrepancyCorrectiveActionList
				Session("DiscrepancyCorrectiveAction") = DiscrepancyCorrectiveAction
				Session("MELSnagCorrectiveActionLog") = MELSnagCorrectiveActionLog
				Session("mFileAttach") = FileAttach
				Session("DateOfOccurrence") = DiscrepancyCorrectiveAction.DateOfOccurrence
				Session("MachineID") = Log.MachineID.ToString
				Session("AircraftRegNo") = Log.RegNo.ToString
				Session("IsFromLog") = True

				ModuleNameForMarkLog = IIf(ModuleName = "Discrepancy", "DiscrepancyAction", "CabinDefectAction")

				MarkLog(Action.[New],
						ModuleNameForMarkLog,
						"",
						ErrorType.NoError,
						Guid.Empty,
						EventLogID)

				DetailPageScript = IIf(ModuleName = "Discrepancy", "OpenDiscrepancyDetailWindow()", "OpenCabinDefectDetailWindow()")

				ScriptManager.RegisterStartupScript(page:=Me,
													type:=[GetType],
													key:="Open Detail Window",
													script:=DetailPageScript,
													addScriptTags:=True)
			Else

				ListPageScript = IIf(ModuleName = "Discrepancy", "callDiscrepancyReporting()", "ShowCabinDefectList()")

				ScriptManager.RegisterStartupScript(page:=Me,
													type:=[GetType],
													key:="Open List Page",
													script:=ListPageScript,
													addScriptTags:=True)

			End If

			Return Log

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

#Region " TAB's " 'Tabs' added by Saylee on 3-Apr-2023

	Private Sub LogDetailsContainerTabChanged(sender As Object, e As EventArgs) Handles tabLogDetailsContainer.ActiveTabChanged

		Try

			Select Case tabLogDetailsContainer.ActiveTabIndex
				Case 0
				Case 1 'Fuel Oil

					Session("OpenFromWO") = False
					Session("mOpenFromLogFuelNew") = False

					If (Not User.IsInRole("LogFuelOilNew") And mLog.IsNew) Or
					   (Not User.IsInRole("LogFuelOilEdit") And Not mLog.IsNew) Then

						SetSession()
						MarkLog(Action.Save,
								"LogFuelOil",
								User.Identity.Name & " is not Authorized User to edit " & mLogDetail,
								ErrorType.HandledError,
								Guid.Empty,
								EventLogID)

						MSGBoxCtrl.Show(MSGBox.Message_Title.Authorization,
										MSGBox.Message_Text.Authorization,
										"",
										MsgBoxStyle.OkOnly,
										"Authorization")

						Exit Sub

					End If

					DataFieldBind()
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallFuelOil",
														"CallFuelOil()",
														True)

				Case 2

					Session("Edit") = False
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallSnagReporting",
														"CallSnagReporting()",
														True)

				Case 3

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallParameterList",
														"CallParameterList()",
														True)

				Case 4

					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallFlightCrewList",
														"CallFlightCrewList()",
														True)

				Case 5

					SetObject()
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"CallMaintActivity",
														"CallMaintActivity()",
														True)

				Case 6  'DiscrepancyReporting

					Session("mMachine") = mMachine
					mLog = Log.GetLog(ID:=mLog.ID)

					mLog = SetDiscrepancyDetailsForLog(Log:=mLog, ModuleName:="Discrepancy")

				Case 7  'Deferred Discrepancies

					Session("mMachine") = mMachine
					ScriptManager.RegisterStartupScript(Me,
														[GetType],
														"callDeferredDiscrepancies",
														"callDeferredDiscrepancies()",
														True)
				Case 8  'Cabin Defect

					Session("mMachine") = mMachine
					mLog = Log.GetLog(ID:=mLog.ID, IsCabinDefect:=True)

					mLog = SetDiscrepancyDetailsForLog(Log:=mLog, ModuleName:="Cabin Defect")

			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Web Methods "

	<WebMethod(EnableSession:=True)>
	Public Shared Function LogDetails(MachineID, LogDate) As Object
		Dim mLogListOnDate As LogList = LogList.GetLogList(MachineID, LogDate.Text.ToString, LogDate.Text.ToString)
		Return mLogListOnDate
	End Function

	Private Sub Place2_TextChanged(sender As Object, e As EventArgs) Handles Place2.TextChanged
		txtUTCDepartureTime.Focus()
	End Sub

#End Region

End Class