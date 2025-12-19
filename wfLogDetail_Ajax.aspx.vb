' Rajnish   07-09-2006
Partial Class wfLogDetail_Ajax
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub


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
	Public mLog As Log
	Public mMachine As Machine
	''  Public mLogList As LogList
	Public mFlightLogClassificationList As FlightLogClassificationList
	Private Flag As Int16
	Dim Type As Integer
	Private LogListCount As Integer = 0
	Dim EventLogID As Guid 'Added by Prashant on 20-July-2011
	Dim IsValueZero As Boolean = False 'Shweta
	Dim mLogDetail As String
	Dim mFileAttach As FileAttach
	Dim IsAttachmentDeleted As Boolean = False

	Public mLogListOnDate As LogList
	Dim mCompanyDetail As New CompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
#End Region

#Region " Business Methods "
	Private Sub GetSession()
		mLog = CType(Session("mLog"), Log)
		''  mLogList = CType(Session("mLogList"), LogList)
		mMachine = CType(Session("mMachine"), Machine)
		mFlightLogClassificationList = CType(Session("mFlightLogClassificationList"), FlightLogClassificationList)
		LogListCount = CType(Session("LogListCount"), Integer)
		mFileAttach = Session("mFileAttach")
		IsAttachmentDeleted = Session("IsAttachmentDeleted")
		mLogListOnDate = Session("mLogListOnDate")
		mCompanyDetail = Session("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
	End Sub
	Private Sub SetSession()
		Session("mLog") = mLog
		Session("mMachine") = mMachine
		''  Session("mLogList") = mLogList
		Session("mFlightLogClassificationList") = mFlightLogClassificationList
		Session("LogListCount") = LogListCount
		Session("mLogListOnDate") = mLogListOnDate
		Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
	End Sub
	Private Sub RemoveSession()
		Session.Remove("mMachine")
		''Session.Remove("mLogList")
		Session.Remove("mLog")
		Session.Remove("LogListCount")
		Session.Remove("mFileAttach")
		Session.Remove("IsAttachmentDeleted")
		Session.Remove("mLogListOnDate")
		Session.Remove("mCompanyDetail") 'PBH Collective Hrs by Saylee on 30-Nov-2022
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
		'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
		If (AppSettings("ClientCode") = "Heligo" Or
			AppSettings("ClientCode") = "UHPL" Or
			AppSettings("ClientCode") = "APFT" Or
			AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
			mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
			mLog.Pilot1Name = "None"
		End If
		If Type = -1 Then
			Select Case AddType
				Case 0
					If (AppSettings("ClientCode") = "Heligo" Or
						AppSettings("ClientCode") = "UHPL" Or
						AppSettings("ClientCode") = "APFT" Or
						AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
						mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
						mLog.Pilot1Name = "None"
					Else
						mLog.PilotID1 = New Guid(Id)
						mLog.Pilot1Name = Name
					End If
				Case 1
					mLog.PilotID2 = New Guid(Id)
					mLog.Pilot2Name = Name
				Case 2
					mLog.DestinationID = New Guid(Id)
					' mLog.DestinationName = Name

				Case 3
					mLog.SourceID = New Guid(Id)
					' mLog.SourceName = Name
			End Select
		End If
		Session("mLog") = mLog
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As SIControls.SICalendar)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		Try
			Dim str As String
			'str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
			'ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)

			' '' ''AJAX-Date Control is present in Update Panel so ClientScript is not working/fired. Instead of it use ScriptManager
			str = "try{document.getElementById('" + cntrl.ClientID + "').focus();}catch (Error) {}"
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "focusscript", str, True)
		Catch ex As Exception
			'
		End Try
	End Sub
	Private Sub EnableDisableButton()

		btnLogPax.Enabled = Not mLog.IsNew
		btnDefectActionList.Enabled = Not mLog.IsNew
		'pnlPilot-Enabled
		calDateTime.Enabled = mLog.IsNew
		btnParameterList.Enabled = Not mLog.IsNew 'Added by Saylee on 6-Sep-2012
		btnFuelOil.Enabled = Not mLog.IsNew       'Added by Saylee on 6-Sep-2012
		'' imgbtnPilot1.Enabled = mLog.IsNew
		''imgbtnPilot2.Enabled = mLog.IsNew
		'' btnAddPilot.Enabled = mLog.IsNew
		'*pnlPlace-Enabled
		'Departure
		'' calDeparture.Visible = mLog.IsNew
		''  imgbtnDepPlace.Enabled = mLog.IsNew
		'' btnAddDepPlace.Enabled = mLog.IsNew
		'' cmbDepartureDayLightTime.Enabled = mLog.IsNew
		'Arrival
		'' calArrival.Enabled = mLog.IsNew
		'' imgbtnArrPlace.Enabled = mLog.IsNew
		''  btnAddArrPlace.Enabled = mLog.IsNew
		''  calDeparture.ReadOnly = Not mLog.IsNew
		''  calArrival.ReadOnly = Not mLog.IsNew
		If Not mLog.IsNew Then
			'' imgbtnPilot1.BackColor = Color.Gainsboro
			'' imgbtnPilot2.BackColor = Color.Gainsboro
			'' calDateTime.BackColor = Color.Gainsboro
			''  calDeparture.BackColor = Color.Gainsboro
			'' calArrival.BackColor = Color.Gainsboro
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
			'Added By Vikrant On 26-Nov-2018 For APFT26112018
			txtBlockTime.BackColor = Color.Gainsboro
			txtBlockTime.ReadOnly = True
			'End
		Else                                                        ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
			txtAirBorneTime.BackColor = Color.White
			txtGroundRunTime.BackColor = Color.White
			txtPercentTimeOnGround.BackColor = Color.White
			txtCurrentHobbsValue.BackColor = Color.White
			'Added By Vikrant On 26-Nov-2018 For APFT26112018
			If AppSettings("SetBlockTime") = "True" Then
				txtBlockTime.BackColor = Color.White
				txtBlockTime.ReadOnly = False
			Else
				txtBlockTime.BackColor = Color.Gainsboro
				txtBlockTime.ReadOnly = True
			End If

			'End
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
		'End Place
		If mCompanyDetail.IsSyncApplication Then
			imgbtnArrPlace.Visible = False
			imgbtnPilot2.Visible = False
			imgbtnPilot1.Visible = False
			imgbtnDepPlace.Visible = False
			btnAddPlaces.Visible = False
			btnAddPilot.Visible = False
		End If
		'Date 
		''If mLogList.Count = 0 Then
		If LogListCount = 0 Then
			calDeparture.Enabled = True  ''And mMachine.HourType = 1
			calArrival.Enabled = True ''And mMachine.HourType = 1
			calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
			calArrival.ReadOnly = Not (True) '' And mMachine.HourType = 1)
		End If
		''If mLogList.Count > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") = "9999" Then
		''    calDeparture.Visible = False
		''    calArrival.Enabled = False
		''    calDeparture.ReadOnly = True
		''    calArrival.ReadOnly = True
		''End If
		'' If mLogList.Count > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString = "" Then
		If LogListCount > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString = "" Then
			calDeparture.Enabled = True  ''And mMachine.HourType = 1
			'calArrival.Enabled = False
			calArrival.Enabled = True
			calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
			calArrival.ReadOnly = Not (True)
		End If
		If Not calDeparture.Enabled Then
			calDeparture.BackColor = Color.Gainsboro
		Else
			calDeparture.BackColor = Color.White
		End If
		If Not calArrival.Enabled Then
			calArrival.BackColor = Color.Gainsboro
		Else
			calArrival.BackColor = Color.White
		End If
		'-End Date

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

		'pnlHours
		txtAirBorneTime.ReadOnly = Not mLog.IsNew
		txtCurrentHobbsValue.ReadOnly = Not mLog.IsNew
		'This change is made to change the LogBook Time Entry Format. ------By Devendra
		'Local Entry Setting
		'Commented By Saylee On 12-Feb-2014 For ALL12022014-1
		'calDeparture.Enabled = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'calArrival.Enabled = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'CalUTCDateTime.Enabled = (AppSettings("LogBookTimeEntry") = "UTC")
		'CalUTCArrival.Enabled = (AppSettings("LogBookTimeEntry") = "UTC")

		'Added By Saylee On 12-Feb-2014 For ALL12022014-1
		calDeparture.Enabled = Not mMachine.IsUTC
		calArrival.Enabled = Not mMachine.IsUTC
		CalUTCDateTime.Enabled = mMachine.IsUTC
		CalUTCArrival.Enabled = mMachine.IsUTC

		'If mLog.ImageSize > 0 Then
		'    ImageButton2.Visible = True
		'    btnDelAttach.Enabled = True
		'Else
		'    ImageButton2.Visible = False
		'    btnDelAttach.Enabled = False
		'End If

		' '' ''AJAX- To reflect changes of controls we have call ".Update()" method of respective Panel
		upnlTabs.Update()
		upnlFlightDetails.Update()
		upnlFlightSummary.Update()

	End Sub
	Private Sub ControlVisibility()
		'Airframe ----> 
		'Hours
		dgAFPeriods.Columns(3).Visible = mLog.LogAFAssemblies.ShowHours
		dgAFPeriods.Columns(4).Visible = mLog.LogAFAssemblies.ShowHours

		'Landings
		dgAFPeriods.Columns(5).Visible = mLog.LogAFAssemblies.ShowLandings
		dgAFPeriods.Columns(6).Visible = mLog.LogAFAssemblies.ShowLandings
		'Cycles
		dgAFPeriods.Columns(7).Visible = mLog.LogAFAssemblies.ShowCycles
		dgAFPeriods.Columns(8).Visible = mLog.LogAFAssemblies.ShowCycles
		'Starts
		dgAFPeriods.Columns(9).Visible = mLog.LogAFAssemblies.ShowStarts
		dgAFPeriods.Columns(10).Visible = mLog.LogAFAssemblies.ShowStarts
		'NG
		dgAFPeriods.Columns(11).Visible = mLog.LogAFAssemblies.ShowNGCycles
		dgAFPeriods.Columns(12).Visible = mLog.LogAFAssemblies.ShowNGCycles
		'NF
		dgAFPeriods.Columns(13).Visible = mLog.LogAFAssemblies.ShowNFCycles
		dgAFPeriods.Columns(14).Visible = mLog.LogAFAssemblies.ShowNFCycles
		'RINS
		dgAFPeriods.Columns(15).Visible = mLog.LogAFAssemblies.ShowRINS
		dgAFPeriods.Columns(16).Visible = mLog.LogAFAssemblies.ShowRINS
		'Bleeds  'Added By Prashant 8-July-2009
		dgAFPeriods.Columns(17).Visible = mLog.LogAFAssemblies.ShowBleeds
		dgAFPeriods.Columns(18).Visible = mLog.LogAFAssemblies.ShowBleeds
		'ImpellerCycles  'Added By Prashant 10-Aug-2009
		dgAFPeriods.Columns(19).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
		dgAFPeriods.Columns(20).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
		'CTCycles
		dgAFPeriods.Columns(21).Visible = mLog.LogAFAssemblies.ShowCTCycles
		dgAFPeriods.Columns(22).Visible = mLog.LogAFAssemblies.ShowCTCycles
		'PTCycles
		dgAFPeriods.Columns(23).Visible = mLog.LogAFAssemblies.ShowPTCycles
		dgAFPeriods.Columns(24).Visible = mLog.LogAFAssemblies.ShowPTCycles
		'--------------------------------------
		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		dgAFPeriods.Columns(25).Visible = mLog.LogAFAssemblies.ShowGeneratorMods
		dgAFPeriods.Columns(26).Visible = mLog.LogAFAssemblies.ShowGeneratorMods
		'--------------------------------------
		'Engine ----> 
		'Hours
		dgEnginePeriods.Columns(3).Visible = mLog.LogEngAssemblies.ShowHours
		dgEnginePeriods.Columns(4).Visible = mLog.LogEngAssemblies.ShowHours
		'Landings
		dgEnginePeriods.Columns(5).Visible = mLog.LogEngAssemblies.ShowLandings
		dgEnginePeriods.Columns(6).Visible = mLog.LogEngAssemblies.ShowLandings
		'Cycles
		dgEnginePeriods.Columns(7).Visible = mLog.LogEngAssemblies.ShowCycles
		dgEnginePeriods.Columns(8).Visible = mLog.LogEngAssemblies.ShowCycles
		'Starts
		dgEnginePeriods.Columns(9).Visible = mLog.LogEngAssemblies.ShowStarts
		dgEnginePeriods.Columns(10).Visible = mLog.LogEngAssemblies.ShowStarts
		'NG
		dgEnginePeriods.Columns(11).Visible = mLog.LogEngAssemblies.ShowNGCycles
		dgEnginePeriods.Columns(12).Visible = mLog.LogEngAssemblies.ShowNGCycles
		'NF
		dgEnginePeriods.Columns(13).Visible = mLog.LogEngAssemblies.ShowNFCycles
		dgEnginePeriods.Columns(14).Visible = mLog.LogEngAssemblies.ShowNFCycles
		'RINS
		dgEnginePeriods.Columns(15).Visible = mLog.LogEngAssemblies.ShowRINS
		dgEnginePeriods.Columns(16).Visible = mLog.LogEngAssemblies.ShowRINS
		'RINS
		dgEnginePeriods.Columns(17).Visible = mLog.LogEngAssemblies.ShowCFactor
		dgEnginePeriods.Columns(18).Visible = mLog.LogEngAssemblies.ShowCFactor
		'Bleeds  'Added By Prashant 8-July-2009
		dgEnginePeriods.Columns(19).Visible = mLog.LogEngAssemblies.ShowBleeds
		dgEnginePeriods.Columns(20).Visible = mLog.LogEngAssemblies.ShowBleeds
		'ImpellerCycles  'Added By Prashant 10-Aug-2009
		dgEnginePeriods.Columns(21).Visible = mLog.LogEngAssemblies.ShowImpellerCycles
		dgEnginePeriods.Columns(22).Visible = mLog.LogEngAssemblies.ShowImpellerCycles
		'CTCycles  
		dgEnginePeriods.Columns(23).Visible = mLog.LogEngAssemblies.ShowCTCycles
		dgEnginePeriods.Columns(24).Visible = mLog.LogEngAssemblies.ShowCTCycles
		'PTCycles  
		dgEnginePeriods.Columns(25).Visible = mLog.LogEngAssemblies.ShowPTCycles
		dgEnginePeriods.Columns(26).Visible = mLog.LogEngAssemblies.ShowPTCycles
		'--------------------------------------
		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		dgEnginePeriods.Columns(27).Visible = mLog.LogEngAssemblies.ShowGeneratorMods
		dgEnginePeriods.Columns(28).Visible = mLog.LogEngAssemblies.ShowGeneratorMods

		'Rapid TakeOff Factor  'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
		dgEnginePeriods.Columns(29).Visible = mLog.LogEngAssemblies.ShowRapidTakeOffFactors
		dgEnginePeriods.Columns(30).Visible = mLog.LogEngAssemblies.ShowRapidTakeOffFactors

		'APU ----> 
		If mLog.LogAPUAssemblies.Count = 0 Then
			dgAPUPeriods.Visible = False
			lblAPUPeriod.Visible = False
		Else 'Added By utkarsh on 30-sep-2013 for log_ajax changes
			dgAPUPeriods.Visible = True
			lblAPUPeriod.Visible = True
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

		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		dgAPUPeriods.Columns(25).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
		dgAPUPeriods.Columns(26).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods

		'CGB ----> 
		If mLog.LogCGBAssemblies.Count = 0 Then
			dgCGBPeriods.Visible = False
			lblCGBPeriod.Visible = False
		Else 'Added By utkarsh on 30-sep-2013 for log_ajax changes
			dgCGBPeriods.Visible = True
			lblCGBPeriod.Visible = True
		End If
		'Hours
		dgCGBPeriods.Columns(3).Visible = mLog.LogCGBAssemblies.ShowHours
		dgCGBPeriods.Columns(4).Visible = mLog.LogCGBAssemblies.ShowHours
		'Landings
		dgCGBPeriods.Columns(5).Visible = mLog.LogCGBAssemblies.ShowLandings
		dgCGBPeriods.Columns(6).Visible = mLog.LogCGBAssemblies.ShowLandings
		'Cycles
		dgCGBPeriods.Columns(7).Visible = mLog.LogCGBAssemblies.ShowCycles
		dgCGBPeriods.Columns(8).Visible = mLog.LogCGBAssemblies.ShowCycles
		'Starts
		dgCGBPeriods.Columns(9).Visible = mLog.LogCGBAssemblies.ShowStarts
		dgCGBPeriods.Columns(10).Visible = mLog.LogCGBAssemblies.ShowStarts
		'NG
		dgCGBPeriods.Columns(11).Visible = mLog.LogCGBAssemblies.ShowNGCycles
		dgCGBPeriods.Columns(12).Visible = mLog.LogCGBAssemblies.ShowNGCycles
		'NF
		dgCGBPeriods.Columns(13).Visible = mLog.LogCGBAssemblies.ShowNFCycles
		dgCGBPeriods.Columns(14).Visible = mLog.LogCGBAssemblies.ShowNFCycles
		'RINS
		dgCGBPeriods.Columns(15).Visible = mLog.LogCGBAssemblies.ShowRINS
		dgCGBPeriods.Columns(16).Visible = mLog.LogCGBAssemblies.ShowRINS
		'Bleeds  'Added By Prashant 8-July-2009
		dgCGBPeriods.Columns(17).Visible = mLog.LogCGBAssemblies.ShowBleeds
		dgCGBPeriods.Columns(18).Visible = mLog.LogCGBAssemblies.ShowBleeds
		'ImpellerCycles  'Added By Prashant 10-Aug-2009
		dgCGBPeriods.Columns(19).Visible = mLog.LogCGBAssemblies.ShowImpellerCycles
		dgCGBPeriods.Columns(20).Visible = mLog.LogCGBAssemblies.ShowImpellerCycles
		'CTCycles  
		dgCGBPeriods.Columns(21).Visible = mLog.LogCGBAssemblies.ShowCTCycles
		dgCGBPeriods.Columns(22).Visible = mLog.LogCGBAssemblies.ShowCTCycles
		'PTCycles  
		dgCGBPeriods.Columns(23).Visible = mLog.LogCGBAssemblies.ShowPTCycles
		dgCGBPeriods.Columns(24).Visible = mLog.LogCGBAssemblies.ShowPTCycles


		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		dgCGBPeriods.Columns(25).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
		dgCGBPeriods.Columns(26).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
		'-------------------------------------
		'code added by DEVEN 24-03-2008
		calDeparture.ShowClearButton = False
		calArrival.ShowClearButton = False
		CalUTCArrival.ShowClearButton = False
		CalUTCDateTime.ShowClearButton = False
		calDateTime.ShowClearButton = False
		'-------------------------------------

		'Added By Saylee on 20-Mar-2009
		''If (AppSettings("ClientCode") = "Heligo") Or (mLog.IsHobbs = True) Then
		''    lblDateTimeStar1.Visible = False
		''    lblDateTimeStar2.Visible = False
		''    lblUTCDateTimeStar1.Visible = False
		''    lblUTCDateTimeStar2.Visible = False
		''    lblPlaceStar2.Visible = False
		''    lblPalceStar1.Visible = False
		''Else
		''    lblDateTimeStar1.Visible = True
		''    lblDateTimeStar2.Visible = True
		''    lblUTCDateTimeStar1.Visible = True
		''    lblUTCDateTimeStar2.Visible = True
		''    lblPlaceStar2.Visible = True
		''    lblPalceStar1.Visible = True
		''End If
		lblDateTimeStar1.Visible = False
		lblDateTimeStar2.Visible = False
		lblUTCDateTimeStar1.Visible = False
		lblUTCDateTimeStar2.Visible = False
		lblPlaceStar2.Visible = False
		lblPalceStar1.Visible = False


		'=====================================
		''Added by Saylee on 1-Mar-2022
		'ALL Assembly ----> 
		'Hours
		grdAllAssemblies.Columns(4).Visible = mLog.ALL_LogAssemblies.ShowHours
		grdAllAssemblies.Columns(5).Visible = mLog.ALL_LogAssemblies.ShowHours

		'Landings
		grdAllAssemblies.Columns(6).Visible = mLog.ALL_LogAssemblies.ShowLandings
		grdAllAssemblies.Columns(7).Visible = mLog.ALL_LogAssemblies.ShowLandings
		'Cycles
		grdAllAssemblies.Columns(8).Visible = mLog.ALL_LogAssemblies.ShowCycles
		grdAllAssemblies.Columns(9).Visible = mLog.ALL_LogAssemblies.ShowCycles
		'Starts
		grdAllAssemblies.Columns(10).Visible = mLog.ALL_LogAssemblies.ShowStarts
		grdAllAssemblies.Columns(11).Visible = mLog.ALL_LogAssemblies.ShowStarts
		'NG
		grdAllAssemblies.Columns(12).Visible = mLog.ALL_LogAssemblies.ShowNGCycles
		grdAllAssemblies.Columns(13).Visible = mLog.ALL_LogAssemblies.ShowNGCycles
		'NF
		grdAllAssemblies.Columns(14).Visible = mLog.ALL_LogAssemblies.ShowNFCycles
		grdAllAssemblies.Columns(15).Visible = mLog.ALL_LogAssemblies.ShowNFCycles
		'RINS
		grdAllAssemblies.Columns(16).Visible = mLog.ALL_LogAssemblies.ShowRINS
		grdAllAssemblies.Columns(17).Visible = mLog.ALL_LogAssemblies.ShowRINS
		'Bleeds  'Added By Prashant 8-July-2009
		grdAllAssemblies.Columns(18).Visible = mLog.ALL_LogAssemblies.ShowBleeds
		grdAllAssemblies.Columns(19).Visible = mLog.ALL_LogAssemblies.ShowBleeds
		'ImpellerCycles  'Added By Prashant 10-Aug-2009
		grdAllAssemblies.Columns(20).Visible = mLog.ALL_LogAssemblies.ShowImpellerCycles
		grdAllAssemblies.Columns(21).Visible = mLog.ALL_LogAssemblies.ShowImpellerCycles
		'CTCycles
		grdAllAssemblies.Columns(22).Visible = mLog.ALL_LogAssemblies.ShowCTCycles
		grdAllAssemblies.Columns(23).Visible = mLog.ALL_LogAssemblies.ShowCTCycles
		'PTCycles
		grdAllAssemblies.Columns(24).Visible = mLog.ALL_LogAssemblies.ShowPTCycles
		grdAllAssemblies.Columns(25).Visible = mLog.ALL_LogAssemblies.ShowPTCycles
		'--------------------------------------
		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		grdAllAssemblies.Columns(26).Visible = mLog.ALL_LogAssemblies.ShowGeneratorMods
		grdAllAssemblies.Columns(27).Visible = mLog.ALL_LogAssemblies.ShowGeneratorMods


		' '' ''AJAX- To reflect changes of controls we have call ".Update()" method of respective Panel
		upnlFlightDetails.Update()

		upnlAirframeDetail.Update()
		upnlEngineDetail.Update()
		upnlAPUDetail.Update()
		upnlCGBDetail.Update()
		upnlAssemblyInfo.Update()
	End Sub

	Public Function IsZeroValueLog(Optional ByVal isFromDataBindGrid As Boolean = False) As Boolean  ' For First Grid i.e AirFrame

		Dim isZero As Boolean = False
		Dim flag As Boolean = False
		If Val(mLog.TotalTime) = 0 Then

		End If
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
			'Added by Shweta on 7-May-2012 for ALL02052012
			If mLog.LogAFAssemblies.ShowGeneratorMods Then
				If Val(mLog.LogAFAssemblies(i).GeneratorMods) = 0 Then
					flag = True
					Exit For
				End If
			End If
			'----------
		Next
		If flag = True Then
			flag = False
			isZero = True
		End If

		For i As Integer = 0 To mLog.LogAPUAssemblies.Count - 1
			If mLog.LogAPUAssemblies(i).ShowHours Then
				If mLog.IsHobbs Then
					If Val(mLog.LogAPUAssemblies(i).Hours) = 0 Then
						flag = True
						Exit For
					End If
				Else
					If mLog.LogAPUAssemblies(i).Hours = "0:00" Then
						flag = True
						Exit For
					End If
				End If
			End If
			If mLog.LogAPUAssemblies(i).ShowImpellerCycles Then
				If (Val(mLog.LogAPUAssemblies(i).ImpellerCycles) = 0) Then
					flag = True
					Exit For
				End If
			End If

			If mLog.LogAPUAssemblies(i).ShowLandings Then
				If (Val(mLog.LogAPUAssemblies(i).Landings) = 0) Then
					flag = True
					Exit For
				End If
			End If
			If mLog.LogAPUAssemblies(i).ShowNFCycles Then
				If (Val(mLog.LogAPUAssemblies(i).NFCycles) = 0) Then
					flag = True
					Exit For
				End If
			End If

			If mLog.LogAPUAssemblies(i).ShowNGCycles Then
				If (Val(mLog.LogAPUAssemblies(i).NGCycles) = 0) Then
					flag = True
					Exit For
				End If
			End If

			If mLog.LogAPUAssemblies(i).ShowCTCycles Then
				If (Val(mLog.LogAPUAssemblies(i).CTCycles) = 0) Then
					flag = True
					Exit For

				End If
			End If
			If mLog.LogAPUAssemblies(i).ShowPTCycles Then
				If (Val(mLog.LogAPUAssemblies(i).PTCycles) = 0) Then
					flag = True
					Exit For
				End If
			End If
			If mLog.LogAPUAssemblies(i).ShowBleeds Then
				If (Val(mLog.LogAPUAssemblies(i).Bleeds) = 0) Then
					flag = True
					Exit For
				End If
			End If

			If mLog.LogAPUAssemblies(i).ShowCycles Then
				If (Val(mLog.LogAPUAssemblies(i).Cycles) = 0) Then
					flag = True
					Exit For
				End If
			End If
			If mLog.LogAPUAssemblies(i).ShowStarts Then
				If (Val(mLog.LogAPUAssemblies(i).Starts) = 0) Then
					flag = True
					Exit For
				End If
			End If
			'Added by Shweta on 7-May-2012 for ALL02052012
			If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then
				If (Val(mLog.LogAPUAssemblies(i).GeneratorMods) = 0) Then
					flag = True
					Exit For
				End If
			End If
			'----------
			If mLog.LogAPUAssemblies(i).ShowRINS Then
				If (Val(mLog.LogAPUAssemblies(i).RINS) = 0) Then
					flag = True
					Exit For
				End If
			End If
		Next


		If flag = True Then
			flag = False
			isZero = True
		End If

		For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
			'Engine
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
			If mLog.LogEngAssemblies(i).ShowCFactors Then
				If Val(mLog.LogEngAssemblies(i).CFactor) = 0 Then
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
			If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then   ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
				If Val(mLog.LogEngAssemblies(i).RapidTakeOffFactor) = 0 Then
					flag = True
					Exit For
				End If
			End If
		Next
		If flag = True Then
			flag = False
			isZero = True
		End If

		'Added by Shweta on 8-May-2012

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
			'Added by shweta on 7-May-2012 for ALL02052012
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
		'----------------------------
		Return isZero
	End Function

	Private Sub SetObject()
		With mLog
			'CNDC
			If Not (calDateTime.IsDateValue) Then
				.Date = System.DBNull.Value
			Else
				.Date = calDateTime.Value.ToString
			End If
			'If Not IsDate(calDateTime.Text) Then
			'    .Date = System.DBNull.Value
			'Else
			'    .Date = CType(Trim(calDateTime.Text), Object)
			'End If

			.LogText = Trim(txtLogText.Text)
			'.LogNo = Trim(txtLogNo.Text)
			.LogNo = CInt(Val(Trim(txtLogNo.Text)))
			If .IsUTC = True Then
				'CNDC
				If Not (CalUTCDateTime.IsDateValue) Then
					.SouUniverseDateTime = System.DBNull.Value
				Else
					.SouUniverseDateTime = CalUTCDateTime.Value.ToString
				End If
				'If Not IsDate(CalUTCDateTime.Text) Then
				'    .SouUniverseDateTime = System.DBNull.Value
				'Else
				'    .SouUniverseDateTime = CType(Trim(CalUTCDateTime.Text), Object)
				'End If
			Else
				'CNDC
				If Not (calDeparture.IsDateValue) Then
					.SouLocalDateTime = System.DBNull.Value
				Else
					.SouLocalDateTime = calDeparture.Value.ToString
				End If
				'If Not IsDate(calDeparture.Text) Then
				'    .SouLocalDateTime = System.DBNull.Value
				'Else
				'    .SouLocalDateTime = CType(Trim(calDeparture.Text), Object)
				'End If
			End If
			.SouDayLightTime = cmbDepartureDayLightTime.SelectedValue
			If .IsUTC = True Then
				'CNDC
				If Not (CalUTCArrival.IsDateValue) Then
					.DesUniverseDateTime = System.DBNull.Value
				Else
					.DesUniverseDateTime = CalUTCArrival.Value.ToString
				End If
				'If Not IsDate(CalUTCArrival.Text) Then
				'    .DesUniverseDateTime = System.DBNull.Value
				'Else
				'    .DesUniverseDateTime = CType(Trim(CalUTCArrival.Text), Object)
				'End If
			Else
				'CNDC
				If Not (calArrival.IsDateValue) Then
					.DesLocalDateTime = System.DBNull.Value
				Else
					.DesLocalDateTime = calArrival.Value.ToString
				End If
				'If Not IsDate(calArrival.Text) Then
				'    .DesLocalDateTime = System.DBNull.Value
				'Else
				'    .DesLocalDateTime = CType(Trim(calArrival.Text), Object)
				'End If
			End If
			.DesDayLightTime = cmbArrivalDayLightTime.SelectedValue
			'APFT26112018
			If AppSettings("SetBlockTime") = "True" Then
				If Not .BlockTime.Equals(Trim(txtBlockTime.Text)) Then
					.BlockTime = Trim(txtBlockTime.Text)
					txtAirBorneTime.DataBind()
					txtGroundRunTime.DataBind()
				End If

			End If
			'End
			.TimeInAir = Trim(txtAirBorneTime.Text)

			If Not AppSettings("Log") = "True" Then .TimeOnGround = Trim(txtGroundRunTime.Text)
			'.TimeOnGround = Trim(txtGroundRunTime.Text)
			.PercentTimeOnGround = Val(Trim(txtPercentTimeOnGround.Text))
			If mMachine.HourType = 2 Then
				.PrevHobbsValue = Trim(txtPrevHobbsValue.Text)
				.PrevHobbsOffsetValue = Trim(txtPrevHobbsOffset.Text)
				.CurrentHobbsOffsetValue = Trim(txtCurrentHobbsOffset.Text)
				.CurrentHobbsValue = Trim(txtCurrentHobbsValue.Text)
				.OffSet = Trim(txtCurrentHobbsOffset.Text)
			End If
			'Commented by Devendra Naik On 14/Arp/2007  Jay Bhim
			'*~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~*
			' .TotalTime = Trim(txtTotalTime.Text)
			'*~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~**~*~*~*~*~*~*~*~*~*
			'.LogPageNo = Val(txtLogPageNo.Text)
			.LogPageNo = txtLogPageNo.Text
			.FlightNo = txtFlightNo.Text.Trim
			.Remark = Trim(txtRemark.Text)
			.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
			.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
			'Added by Shweta on 10-FEB-12
			If Session("isvaluezero") = "True" Then
				.IsValZero = True
			Else
				.IsValZero = False
			End If
			If mFileAttach IsNot Nothing Then
				If mFileAttach.Size > 0 Then
					.IsAttachmentAdded = True
				Else
					.IsAttachmentAdded = False
				End If
			End If
		End With

		'''''''AttachMyFile()
		'Added by Saylee on 18-Oct-2022, for Multiple Attachment
		For i As Integer = 0 To mLog.FileAttachments.Count - 1
			Dim txtValue As TextBox
			txtValue = CType(Me.dgLogAttachment.Rows(i).FindControl("txtFileName"), TextBox)
			mLog.FileAttachments(i).FileName = txtValue.Text.Trim
		Next
		mLog.IsAttachmentAdded = IIf(mLog.FileAttachments.Count > 0, True, False)
		'***************************************

		dgAFPeriods.DataBind()
		dgEnginePeriods.DataBind()
		dgAPUPeriods.DataBind()
		dgCGBPeriods.DataBind() 'Added By Prashant 23-Oct-2009
		Session("mLog") = mLog
	End Sub
	'Private Sub GetAttachment()
	'    If mLog.IsAttachmentAdded And mFileAttach Is Nothing Then
	'        mFileAttach = FileAttach.GetAttachment(mLog.ID)
	'        Session("mFileAttach") = mFileAttach
	'    End If
	'End Sub
	'Private Sub ControlVisibilityForAttachment()
	'    If mLog.IsAttachmentAdded = True Then
	'        ImageButton1.Visible = True
	'        btnDelAttch.Enabled = True
	'    Else
	'        ImageButton1.Visible = False
	'        btnDelAttch.Enabled = False
	'    End If
	'End Sub
	'Private Sub SaveAttachment() '
	'    If Not mFileAttach Is Nothing Then
	'        If mFileAttach.Size > 0 Then
	'            Try
	'                mFileAttach.Save()
	'            Catch ex As Exception
	'                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
	'            End Try
	'        Else
	'            If (Not mLog.IsNew) And IsAttachmentDeleted Then
	'                FileAttach.DeleteAttachment(mFileAttach.ID, mLog.ID)
	'            End If
	'            IsAttachmentDeleted = False
	'            Session("IsAttachmentDeleted") = IsAttachmentDeleted
	'        End If
	'    End If
	'End Sub
	'Private Sub ViewImage()
	'    Dim No As New Random
	'    Dim StrName As String = "abc" & No.Next.ToString
	'    GetAttachment()
	'    If mFileAttach.Size > 0 Then
	'        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
	'        Dim fs As FileStream
	'        If File.Exists(AppSettings("DOCPath")) = False Then
	'            'Delete File if exist
	'            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
	'            ' Create the file.
	'            fs = File.Create(path)
	'            '' Add some information to the file.
	'            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
	'            fs.Close()
	'            Session("DOCPath") = path
	'            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
	'        End If
	'    End If
	'End Sub
	Private Sub NewRecord()
		mLog = Log.NewLog(mMachine, Today.Date)
		mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
		mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
		mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
		'''''CHECK_isRequiredAssembliesInstalled()

		' '' ''AJAX- New line added by Yogita after Save&New when user selects Date; previous Log Time get sets in control
		calDeparture.ShowTime = True
		calArrival.ShowTime = True
		CalUTCDateTime.ShowTime = True
		CalUTCArrival.ShowTime = True
		SetFromSearch() ' '' ''AJAX- New line added by Yogita bcaz Default Pilot is not coming for "Heligo"

		Session("mLog") = mLog
		MarkLog(Util.Action.[New], "Flight Log", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

		' '' ''AJAX- Title line comment as it present in SetTitle function and also Update panel need to called after that.
		' '' '''lblTitle.Text = "Status of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
		SetTitle()
		Dim str1 As String
		str1 = "delete_cookie();"
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)
	End Sub
	Public Function IsEngineHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
		Dim IsSame As Boolean = True
		For i As Integer = 0 To mLog.LogEngAssemblies.Count - 1
			'If mLog.LogAFAssemblies(0).Hours = mLog.LogEngAssemblies(i).Hours Then
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
	End Function
	Public Function IsCGBHoursSame() As Boolean 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
		Dim IsSame As Boolean = True
		''If mLog.LogCGBAssemblies Is Nothing Then
		''    Return True
		''End If
		For i As Integer = 0 To mLog.LogCGBAssemblies.Count - 1
			'If mLog.LogAFAssemblies(0).Hours = mLog.LogCGBAssemblies(i).Hours Then
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
	End Function

	Public Sub SetAirFrameGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)  ' For First Grid i.e AirFrame
		Dim txtAirFrameHours, txtAirFrameLandings, txtAirFrameCycles, txtAirFrameStarts, txtAirFrameNGCycles, txtAirFrameNFCycles, txtAirFrameRins,
			txtAirFrameBleeds, txtAirFrameImpellerCycles, txtAirFrameCTCycles, txtAirFramePTCycles, txtAirframeGeneratorMods As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAFPeriods.Items" is replaced by "dgAFPeriods.Rows"
		For i As Integer = 0 To Me.dgAFPeriods.Rows.Count - 1
			txtAirFrameHours = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameHours"), TextBox)
			txtAirFrameLandings = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameLandings"), TextBox)
			txtAirFrameCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameCycles"), TextBox)
			txtAirFrameStarts = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameStarts"), TextBox)
			txtAirFrameNGCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameNGCycles"), TextBox)
			txtAirFrameNFCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameNFCycles"), TextBox)
			txtAirFrameRins = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameRins"), TextBox)
			'Added By Prashant 8-July-2009
			txtAirFrameBleeds = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameBleeds"), TextBox)
			'-----------------------------
			'Added By Prashant 10-Aug-2009
			txtAirFrameImpellerCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameImpellerCycles"), TextBox)
			txtAirFrameCTCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFrameCTCycles"), TextBox)
			txtAirFramePTCycles = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirFramePTCycles"), TextBox)
			'Added by Shweta on 7-May-2012  for ALL02052012
			txtAirframeGeneratorMods = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirframeGeneratorMods"), TextBox)
			'-----------------------------
			If isFromDataBindGrid Then If mLog.LogAFAssemblies.ShowHours Then mLog.LogAFAssemblies(i).Hours = Trim(txtAirFrameHours.Text)
			If mLog.LogAFAssemblies.ShowLandings Then mLog.LogAFAssemblies(i).Landings = Trim(txtAirFrameLandings.Text)
			If mLog.LogAFAssemblies.ShowCycles Then mLog.LogAFAssemblies(i).Cycles = Trim(txtAirFrameCycles.Text)
			If mLog.LogAFAssemblies.ShowStarts Then mLog.LogAFAssemblies(i).Starts = Trim(txtAirFrameStarts.Text)
			If mLog.LogAFAssemblies.ShowNGCycles Then mLog.LogAFAssemblies(i).NGCycles = Trim(txtAirFrameNGCycles.Text)
			If mLog.LogAFAssemblies.ShowNFCycles Then mLog.LogAFAssemblies(i).NFCycles = Trim(txtAirFrameNFCycles.Text)
			If mLog.LogAFAssemblies.ShowRINS Then mLog.LogAFAssemblies(i).RINS = Trim(txtAirFrameRins.Text)
			'Added By Prashant 8-July-2009
			If mLog.LogAFAssemblies.ShowBleeds Then mLog.LogAFAssemblies(i).Bleeds = Trim(txtAirFrameBleeds.Text)

			'-----------------------------
			'Added By Prashant 10-Aug-2009
			If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.LogAFAssemblies(i).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
			If mLog.LogAFAssemblies.ShowCTCycles Then mLog.LogAFAssemblies(i).CTCycles = Trim(txtAirFrameCTCycles.Text)
			If mLog.LogAFAssemblies.ShowPTCycles Then mLog.LogAFAssemblies(i).PTCycles = Trim(txtAirFramePTCycles.Text)
			'-----------------------------

			'Added by Shweta on 7-May-2012  for ALL02052012
			If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.LogAFAssemblies(i).GeneratorMods = Trim(txtAirframeGeneratorMods.Text)

			'Added By Saylee 21-Mar-2013 for ALL11032013 - 1
			If mLog.LogAFAssemblies.ShowCycles Then mLog.UpdateChildPeriods(3, "Cycles", mLog.LogAFAssemblies(i).Cycles)
			If mLog.LogAFAssemblies.ShowNGCycles Then mLog.UpdateChildPeriods(4, "NgCycles", mLog.LogAFAssemblies(i).NGCycles)
			If mLog.LogAFAssemblies.ShowNFCycles Then mLog.UpdateChildPeriods(5, "NfCycles", mLog.LogAFAssemblies(i).NFCycles)
			If mLog.LogAFAssemblies.ShowRINS Then mLog.UpdateChildPeriods(6, "RINS", mLog.LogAFAssemblies(i).RINS)
			If mLog.LogAFAssemblies.ShowLandings Then mLog.UpdateChildPeriods(7, "Landings", mLog.LogAFAssemblies(i).Landings)
			If mLog.LogAFAssemblies.ShowStarts Then mLog.UpdateChildPeriods(8, "Starts", mLog.LogAFAssemblies(i).Starts)
			'If mLog.LogAFAssemblies.ShowLandings  Then mLog.UpdateChildPeriods(9, "Accumulated Cycles", mLog.LogAFAssemblies(i).AccumulatedCycles)
			If mLog.LogAFAssemblies.ShowBleeds Then mLog.UpdateChildPeriods(11, "Bleeds", mLog.LogAFAssemblies(i).Bleeds)
			If mLog.LogAFAssemblies.ShowImpellerCycles Then mLog.UpdateChildPeriods(12, "ImpellerCycles", mLog.LogAFAssemblies(i).ImpellerCycles)
			If mLog.LogAFAssemblies.ShowCTCycles Then mLog.UpdateChildPeriods(13, "CTCycles", mLog.LogAFAssemblies(i).CTCycles)
			If mLog.LogAFAssemblies.ShowPTCycles Then mLog.UpdateChildPeriods(14, "PTCycles", mLog.LogAFAssemblies(i).PTCycles)
			If mLog.LogAFAssemblies.ShowGeneratorMods Then mLog.UpdateChildPeriods(15, "GeneratorMods", mLog.LogAFAssemblies(i).GeneratorMods)
			'-----------------------------

		Next i
		Session("mLog") = mLog
	End Sub
	'Change by Deven 21-03-2008
	'Public Sub SetEngineGridObject()        ' For Second Grid i.e ENGINE
	Public Sub SetEngineGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Second Grid i.e ENGINE
		Dim txtEngineHours, txtEngineLandings, txtEngineCycles, txtEngineStarts, txtEngineNGCycles, txtEngineNFCycles, txtEngineRins,
			txtEngineCFactors, txtEngineBleeds, txtEngineImpellerCycles, txtEngineCTCycles, txtEnginePTCycles, txtEngineGeneratorMods, txtEngineRapidTakeOffFactor As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgEnginePeriods.Items" is replaced by "dgEnginePeriods.Rows"
		For i As Integer = 0 To Me.dgEnginePeriods.Rows.Count - 1
			txtEngineHours = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineHours"), TextBox)
			txtEngineLandings = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineLandings"), TextBox)
			txtEngineCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCycles"), TextBox)
			txtEngineStarts = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineStarts"), TextBox)
			txtEngineNGCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineNGCycles"), TextBox)
			txtEngineNFCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineNFCycles"), TextBox)
			txtEngineRins = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineRins"), TextBox)
			txtEngineCFactors = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCFactors"), TextBox)
			'Added By Prashant 8-July-2009
			txtEngineBleeds = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineBleeds"), TextBox)
			'-----------------------------------
			'Added By Prashant 10-Aug-2009
			txtEngineImpellerCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineImpellerCycles"), TextBox)
			txtEngineCTCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineCTCycles"), TextBox)
			txtEnginePTCycles = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEnginePTCycles"), TextBox)
			'-----------------------------------


			'Added by Shweta on 7-May-2012 for ALL02052012
			txtEngineGeneratorMods = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012
			txtEngineRapidTakeOffFactor = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineRapidTakeOffFactor"), TextBox) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee


			If isFromDataBindGrid Then If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtEngineHours.Text)
			If mLog.LogEngAssemblies(i).ShowLandings Then mLog.LogEngAssemblies(i).Landings = Trim(txtEngineLandings.Text)
			If mLog.LogEngAssemblies(i).ShowCycles Then mLog.LogEngAssemblies(i).Cycles = Trim(txtEngineCycles.Text)
			If mLog.LogEngAssemblies(i).ShowStarts Then mLog.LogEngAssemblies(i).Starts = Trim(txtEngineStarts.Text)
			If mLog.LogEngAssemblies(i).ShowNGCycles Then mLog.LogEngAssemblies(i).NGCycles = Trim(txtEngineNGCycles.Text)
			If mLog.LogEngAssemblies(i).ShowNFCycles Then mLog.LogEngAssemblies(i).NFCycles = Trim(txtEngineNFCycles.Text)
			If mLog.LogEngAssemblies(i).ShowRINS Then mLog.LogEngAssemblies(i).RINS = Trim(txtEngineRins.Text)
			If mLog.LogEngAssemblies(i).ShowCFactors Then mLog.LogEngAssemblies(i).CFactor = Trim(txtEngineCFactors.Text)
			'Added By Prashant 8-July-2009
			If mLog.LogEngAssemblies(i).ShowBleeds Then mLog.LogEngAssemblies(i).Bleeds = Trim(txtEngineBleeds.Text)
			'----------------------------- 
			'Added By Prashant 10-Aug-2009
			If mLog.LogEngAssemblies(i).ShowImpellerCycles Then mLog.LogEngAssemblies(i).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
			If mLog.LogEngAssemblies(i).ShowCTCycles Then mLog.LogEngAssemblies(i).CTCycles = Trim(txtEngineCTCycles.Text)
			If mLog.LogEngAssemblies(i).ShowPTCycles Then mLog.LogEngAssemblies(i).PTCycles = Trim(txtEnginePTCycles.Text)
			'-----------------------------

			'Added by Shweta on 7-May-2012 for ALL02052012
			If mLog.LogEngAssemblies(i).ShowGeneratorMods Then mLog.LogEngAssemblies(i).GeneratorMods = Trim(txtEngineGeneratorMods.Text) 'Added by Shweta on 7-May-2012  for ALL02052012
			If mLog.LogEngAssemblies(i).ShowRapidTakeOffFactors Then mLog.LogEngAssemblies(i).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee

		Next i
		Session("mLog") = mLog
	End Sub
	'Change by Deven 21-03-2008
	'Public Sub SetAPUGridObject()        ' For Third Grid i.e APU
	Public Sub SetAPUGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)        ' For Third Grid i.e APU
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

			'Added by Shweta on 7-May-2012  for ALL02052012
			txtAPUGeneratorMods = CType(Me.dgAPUPeriods.Rows(i).FindControl("txtAPUGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012


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
			'Added by Shweta on 7-May-2012  for ALL02052012
			If mLog.LogAPUAssemblies(i).ShowGeneratorMods Then mLog.LogAPUAssemblies(i).GeneratorMods = Trim(txtAPUGeneratorMods.Text)
		Next i
		Session("mLog") = mLog
	End Sub
	'Added By Prashant 23-Oct-2009
	Public Sub SetCGBGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)         'For 4th Grid i.e CGB
		Dim txtCGBHours, txtCGBLandings, txtCGBCycles As TextBox, txtCGBStarts, txtCGBNGCycles, txtCGBNFCycles, txtCGBRins, txtCGBBleeds, txtCGBImpellerCycles,
			txtCGBCTCycles, txtCGBPTCycles, txtCGBGeneratorMods As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgCGBPeriods.Items" is replaced by "dgCGBPeriods.Rows"
		For i As Integer = 0 To Me.dgCGBPeriods.Rows.Count - 1
			txtCGBHours = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBHours"), TextBox)
			txtCGBLandings = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBLandings"), TextBox)
			txtCGBCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBCycles"), TextBox)
			txtCGBStarts = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBStarts"), TextBox)
			txtCGBNGCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBNGCycles"), TextBox)
			txtCGBNFCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBNFCycles"), TextBox)
			txtCGBRins = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBRins"), TextBox)
			txtCGBBleeds = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBBleeds"), TextBox)
			txtCGBImpellerCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBImpellerCycles"), TextBox)
			txtCGBCTCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBCTCycles"), TextBox)
			txtCGBPTCycles = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBPTCycles"), TextBox)

			'Added by Shweta on 7-May-2012 for ALL02052012
			txtCGBGeneratorMods = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBGeneratorMods"), TextBox)

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

			'Added by Shweta on 7-May-2012  for ALL02052012
			If mLog.LogCGBAssemblies(i).ShowGeneratorMods Then mLog.LogCGBAssemblies.Item(i).GeneratorMods = Trim(txtCGBGeneratorMods.Text)

		Next i
		Session("mLog") = mLog
	End Sub
	'--------------------------------
	Private Function Save() As Boolean

		'Authentication
		If mLog.Date IsNot System.DBNull.Value Then
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			If mCheck.WebAuthentication = True Then
				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				'Dim strOutString As String = ReadXMLFile()
				'strOutString = strOutString.Split(CChar("$"))(1)
				'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")

				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
				'---------------------------------

				'CNDC
				If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
						Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
						Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then
					'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

					DataFieldBind()
					Exit Function
				End If
			End If
		End If
		'Authentication
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------

		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		If mLog.IsValid = True Then
			Try
				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					' If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
					'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
					Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
						' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")
						Return False
						Exit Function
					End If
				End If

				If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogAfterHrsSame"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
					Exit Function
				End If
				'End

				'Added By Vikrant on 01-Dec-2021 for PBH
				Dim IsNewLog As Boolean
				IsNewLog = mLog.IsNew
				'End
				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				'''''''''''' SaveAttachment()
				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP

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
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function

	Private Function SaveLogFlexiLog() As Boolean 'Added by Saylee on 18-May-2012 ALL17052012
		'Authentication
		If mLog.Date IsNot System.DBNull.Value Then
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			If mCheck.WebAuthentication = True Then
				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				'Dim strOutString As String = ReadXMLFile()
				'strOutString = strOutString.Split(CChar("$"))(1)
				'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")

				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
				'---------------------------------

				'CNDC
				If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
					Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
					Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then
					'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

					DataFieldBind()
					Exit Function
				End If
			End If
		End If
		'Authentication
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------

		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		If mLog.IsValid = True Then
			Try
				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
					'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
					Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
						' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

						Return False
						Exit Function
					End If
				End If

				'Added By Prashant 12-Apr-2010
				Dim IsMELCount As Boolean = False
				Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
				mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
				For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
					'If (mTempMELSnagCorrectiveActionList(i).DueDate > calDateTime.Value) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
					If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
						If (calDateTime.Value > mTempMELSnagCorrectiveActionList(i).DueDate) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
							IsMELCount = True
							Exit For
						Else
							IsMELCount = False
						End If
					End If
				Next
				mTempMELSnagCorrectiveActionList = Nothing
				'-----------------------------------
				If IsMELCount = True Then
					' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "MEL"
					' '' ''msg1.Show()

					MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, IIf(Session("New") = "True", "MELNew", "MEL"))
					Exit Function
				ElseIf IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogAfterHrsSame"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")

					Exit Function
				End If
				'-------------------------------
				'Added By Vikrant on 01-Dec-2021 for PBH
				Dim IsNewLog As Boolean
				IsNewLog = mLog.IsNew
				'End
				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				'-----------------------------------------------------------------------
				'''''''''''''SaveAttachment()
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP

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
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	Private Function SaveLogAfterHrsSame() As Boolean
		'Authentication
		If mLog.Date IsNot System.DBNull.Value Then
			Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
			If mCheck.WebAuthentication = True Then
				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				'Dim strOutString As String = ReadXMLFile()
				'strOutString = strOutString.Split(CChar("$"))(1)
				'Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, CInt(strOutString), mCheck.SubscriptionDate)

				'Changes by Kalpesh in 13-3-2013
				'These lines commented
				'
				Dim mDays As Integer = 0
				mDays = mCheck.Number("Days")

				Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
				'---------------------------------

				'CNDC
				If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < 0 _
								Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < 0) _
								Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < 0) Then
					'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

					DataFieldBind()
					Exit Function
				End If
			End If
		End If
		'Authentication
		Dim LogClone As Log
		LogClone = CType(mLog.Clone, Log)
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------

		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		If mLog.IsValid = True Then
			Try
				If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
					'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
					'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
					Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
						' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

						Return False
						Exit Function
					End If
				End If
				'Added By Vikrant on 01-Dec-2021 for PBH
				Dim IsNewLog As Boolean
				IsNewLog = mLog.IsNew
				'End
				mLog.ApplyEdit()
				mLog = CType(mLog.Save(), Log)
				''''''''''''''''''SaveAttachment()
				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry

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

				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OKOnly)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.LogExist, MSGBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function

	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
		' '' ''    Result1 = -1
		' '' ''Else
		' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
		' '' ''End If

		' '' ''AJAX- Here  "CType(Request.QueryString("MsgResult"), MsgBoxResult)" is replaced by "MSGBoxCtrl.Result"
		Result1 = MSGBoxCtrl.Result

		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Yes
					'Code Added By Deven for Save and New 20/03/2008
					' '' ''AJAX- "Session("sender")" is of no use now. Replaced "CType(Session("sender"), String)" -> MSGBoxCtrl.Sender wherever used in MessageBoxResult function
					' '' ''If CType(Session("sender"), String) = "SaveNew" Then
					If MSGBoxCtrl.Sender = "SaveNew" Then
						' '' ''Session("sender") = ""                   
						mLog = Session("mLog")
						' '' ''DataFieldBind()          ' '' ''AJAX- Value in hrs textbox (Grid) getting refresh
						DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever IsValid or CustomValidate has checked.
							If Save() = True Then
								'mLog = Log.GetLog(mLog.ID)
								NewRecord()
								Session("mLog") = mLog

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

								' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
								' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

								DataFieldBind() 'added

								EnableDisableButton()
								ControlVisibility()

								DataBindGrid()

								SetTitle()
							End If
						End If
					ElseIf MSGBoxCtrl.Sender = "Close" Then

						'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
						If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
							Dim Title As String = "Save Alert !"
							Dim Message As String = ""
							If Not mLog.PilotID1.Equals(Guid.Empty) Then
								Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
								If mEmployeeStatus(0).Information <> "" Then
									Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
								End If
							End If
							If Not mLog.PilotID2.Equals(Guid.Empty) Then
								Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
								If mEmployeeStatus(0).Information <> "" Then
									Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
								End If
							End If
							If Message.Length > 0 Then
								DataFieldBind()
								Session("sender") = ""

								' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
								MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
						End If
						'End

						'Added by Saylee on 18-May-2012 ALL17052012
						Dim mMaxLogOfAircraft As MaxLogOfAircraft
						mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)
						If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
							' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
							'If (AppSettings("ClientCode") <> "Heligo") Then
							If Not (AppSettings("ClientCode") = "Heligo" Or
									AppSettings("ClientCode") = "UHPL" Or
									AppSettings("ClientCode") = "APFT" Or
									AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
								'End
								Dim MaxLogDateTime As String = ""
								' If (AppSettings("LogBookTimeEntry") = "UTC") Then
								If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted
								Else
									MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted
								End If
								' '' ''Session("sender") = ""
								mLog = Session("mLog")
								DataFieldBind()
								If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
									' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
									' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
									' '' ''Session("sender") = "SaveLogFlexiLog"                                    
									' '' ''msg1.Show()
									Session("SaveNClose") = "SaveNClose"

									' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
									MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
									Exit Sub
								End If
							Else
								' '' ''Session("sender") = ""
								mLog = Session("mLog")
								DataFieldBind()
								If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then  'Added by Saylee on 18-May-2012 ALL17052012
									' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
									' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
									' '' ''Session("sender") = "SaveLogFlexiLog"                                    
									' '' ''msg1.Show()

									' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
									MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
									Session("SaveNClose") = "SaveNClose"
									Exit Sub
								End If
							End If
						End If
						'Code Added By Deven on 15-01-2009 for Checking MEL Qty
						'Commented By Prashant 12-Apr-2010
						'Dim IsMELCount As Boolean = True
						'Dim MELList As Aircraft_MEL.MELList
						'MELList = Aircraft_MEL.MELList.GetMELList(mMachine.ID, Guid.Empty, mLog.Date)
						'For i As Integer = 0 To MELList.Count - 1
						'    If MELList(i).FlyMELQty <> CDec(MELList(i).CurrentMELQty) Then
						'        IsMELCount = False
						'        Exit For
						'    End If
						'Next
						'MELList = Nothing
						'-----------------------------------
						'Added By Prashant 12-Apr-2010
						Dim IsMELCount As Boolean = False
						Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
						mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
						For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
							'If (mTempMELSnagCorrectiveActionList(i).DueDate <= mLog.Date) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
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
						'-----------------------------------
						If IsMELCount = True Then
							' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
							' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
							' '' ''Session("sender") = "MELClose"
							' '' ''msg1.Show()

							' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
							MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?", "", MsgBoxStyle.YesNo, "MELClose")
							Exit Sub
						Else
							' '' ''Session("sender") = ""
							mLog = Session("mLog")
							' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
							''DataBind()
							If mLog.IsValid Then
								If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
								Session("SaveNClose") = "SaveNClose"
								If Save() = True Then
									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog
									Session.Remove("mFileAttach")
									Session.Remove("IsAttachmentDeleted")
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
							End If
						End If
						'-------------------------------
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						Session("isvaluezero") = "True" 'Shweta
						' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
						''DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If SaveLogAfterHrsSame() = True Then
								If Session("New") = "True" Then
									Session("New") = ""
									NewRecord()
									Session("mLog") = mLog

									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

									DataFieldBind() 'added

									EnableDisableButton()
									ControlVisibility()

									DataBindGrid()

									SetTitle()
									mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
									Session("mLogListOnDate") = mLogListOnDate
									If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
										'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
										ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
										upnlLogInfo.Update()
									End If
									upnlLogDetails.Update()
								Else
									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog
									DataFieldBind()
									EnableDisableButton()
									SetTitle()
									If Session("SaveNClose") = "SaveNClose" Then
										Session("SaveNClose") = ""
										Session.Remove("SaveNClose")
										Session.Remove("mFileAttach")
										Session.Remove("IsAttachmentDeleted")
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									End If
								End If
								''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
							End If
							''' End If
						End If
						'-------------------------------
					ElseIf MSGBoxCtrl.Sender = "MELClose" Then
						Session("sender") = ""
						mLog = Session("mLog")
						' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
						DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If Save() = True Then
								mLog = Log.GetLog(mLog.ID)
								mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
								mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
								mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
								Session("mLog") = mLog
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
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
							End If
						End If
					ElseIf MSGBoxCtrl.Sender = "MEL" Then
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
						DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If Save() = True Then
								mLog = Log.GetLog(mLog.ID)
								mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
								mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
								mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
								Session("mLog") = mLog


								If Session("SaveNClose") = "SaveNClose" Then
									Session("SaveNClose") = ""
									Session.Remove("SaveNClose")
									Session.Remove("mFileAttach")
									Session.Remove("IsAttachmentDeleted")
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
									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
									DataFieldBind() 'added
									EnableDisableButton()
								End If

							End If
						End If
					ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then 'Added by Saylee on 18-May-2012 ALL17052012 to save Flexi log
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
						''DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If SaveLogFlexiLog() = True Then
								If Session("New") = "True" Then
									Session("New") = ""
									NewRecord()
									Session("mLog") = mLog

									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

									DataFieldBind() 'added

									EnableDisableButton()
									ControlVisibility()

									DataBindGrid()

									SetTitle()
									mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
									Session("mLogListOnDate") = mLogListOnDate
									If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
										'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
										ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
										upnlLogInfo.Update()
									End If
									upnlLogDetails.Update()
								Else
									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog
									DataFieldBind()
									EnableDisableButton()

									If Session("SaveNClose") = "SaveNClose" Then
										Session("SaveNClose") = ""
										Session.Remove("SaveNClose")
										Session.Remove("mFileAttach")
										Session.Remove("IsAttachmentDeleted")
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									End If
								End If

							End If

						End If
					ElseIf MSGBoxCtrl.Sender = "MELNew" Then
						' '' ''Session("sender") = ""
						mLog = Session("mLog")
						' '' ''DataFieldBind()  'Yogita value in hrs textbox (Grid) getting refresh
						DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							Session("New") = "True"
							If Save() = True Then
								'mLog = Log.GetLog(mLog.ID)
								NewRecord()
								Session("mLog") = mLog
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

								' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
								' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

								DataFieldBind() 'added

								EnableDisableButton()
								ControlVisibility()

								DataBindGrid()

								SetTitle()
								mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
								Session("mLogListOnDate") = mLogListOnDate
								If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
									'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
									ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
									upnlLogInfo.Update()
								End If
								upnlLogDetails.Update()
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
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 2627 Then
								MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
							ElseIf ex.Number = 547 Then
								MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
							End If
						End Try
					End If


				Case MsgBoxResult.No
					'Code Added By Deven for Save and New 20/03/2008
					If Session("New") = "True" Then Session("New") = ""
					If MSGBoxCtrl.Sender = "SaveNew" Then
						' '' ''Session("sender") = ""
						NewRecord()

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						DataFieldBind()  'added

						EnableDisableButton()
						ControlVisibility()

						DataBindGrid()

						SetTitle()
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
						' '' ''Session("sender") = ""
						Session.Remove("isvaluezero") 'Shweta

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						DataFieldBind()  'added
					ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then  'Added by Saylee on 18-May-2012 ALL17052012 to save Flexi log
						' '' ''Session("sender") = ""
						Session.Remove("isvaluezero") 'Shweta

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						DataFieldBind() 'added
					ElseIf MSGBoxCtrl.Sender = "Close" Then
						' '' ''Session("sender") = ""
						Session("SaveNClose") = ""
						Session.Remove("SaveNClose")
						'NewReccord()
						Session.Remove("mFileAttach")
						Session.Remove("IsAttachmentDeleted")
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "MELClose" Then
						' '' ''Session("sender") = ""
						'NewRecord()
						Session.Remove("mFileAttach")
						Session.Remove("IsAttachmentDeleted")
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "MEL" Or MSGBoxCtrl.Sender = "MELNew" Then
						' '' ''Session("sender") = ""

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						DataFieldBind() 'added
					End If

				Case MsgBoxResult.Cancel

					'Code Added By Deven for Save and New 20/03/2008
					'If CType(Session("sender"), String) = "Save"  Then
					If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
						' '' ''Session("sender") = ""

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

						DataFieldBind()  'added
					End If
				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					' '' ''Session("sender") = ""
					'Added By Vikrant on 01-Dec-2021 for PBH
					If MSGBoxCtrl.Sender = "AircraftMadeNotInUse" Then
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						Exit Sub
					End If
					'End
					DataFieldBind()

					' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
					' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))                   

				Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
					' '' ''Session("sender") = ""
					DataFieldBind()

					' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
					' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

			End Select
		ElseIf Result1 = -1 Then
			' '' ''Session("sender") = ""
			If Session("New") = "True" Then Session("New") = ""

			' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
			' '' ''Response.Redirect("wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			DataFieldBind() 'added
		ElseIf Result1 = 0 Then   'Code Added
			' '' ''Session("sender") = ""
			If Session("New") = "True" Then Session("New") = ""
			'   DataFieldBind()
		End If
	End Sub
	'''''Private Sub CHECK_isRequiredAssembliesInstalled()
	'''''    If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
	'''''        'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
	'''''        'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
	'''''        If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
	'''''        Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
	'''''            ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OKOnly)
	'''''            ' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
	'''''            ' '' ''msg1.Show()
	'''''            MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")
	'''''            Exit Sub
	'''''        End If
	'''''    End If

	'''''    Dim tmpAssemblyStatusList As tmpAssemblyStatusList = tmpAssemblyStatusList.GetAssemblyStatusList(Now.ToShortDateString, mMachine.ID, True)
	'''''    Dim IsAirFrameAvailable As Boolean = False
	'''''    Dim IsEngineAvailable As Boolean = False
	'''''    Dim AssembliesNotFound As String = ""
	'''''    Dim Obj As tmpAssemblyStatusList.tmpAssemblyStatusInfo
	'''''    For Each Obj In tmpAssemblyStatusList
	'''''        If Obj.AssemblyTypeID = 1 Then IsAirFrameAvailable = True
	'''''        If Obj.AssemblyTypeID = 2 Then IsEngineAvailable = True
	'''''    Next
	'''''    If (Not (IsAirFrameAvailable And IsEngineAvailable)) Then
	'''''        If IsEngineAvailable = False Then AssembliesNotFound = "Engine"
	'''''        If IsAirFrameAvailable = False Then AssembliesNotFound = AssembliesNotFound + IIf(AssembliesNotFound = "", "Machine", ",Machine").ToString
	'''''        ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OKOnly)
	'''''        ' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
	'''''        ' '' ''msg1.Show()
	'''''        MSGBoxCtrl.show(MSGBox.Message_title.EntryRestriction, MSGBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly, "")
	'''''        Exit Sub
	'''''    End If
	'''''End Sub
	Private Sub SetTitle()
		Dim Index As Integer
		Index = Session("Index")
		If mLog.IsNew Then
			If mLog.Date Is DBNull.Value Then
				lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of - [New]"
			Else
				lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
			End If
		Else
			lblTitle.Text = "Log Details of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
			''lblTitle.Text = "Status of " & mMachine.RegNo & " as of " & CStr(mLog.Date) & " [" & (mLogList(Index).LogTextNo) & "]"
		End If

		upnlTitle.Update()  ' '' ''AJAX- call "upnlTitle.Update" to show changes in title 
	End Sub
	Private Sub addAttributes()
		txtPercentTimeOnGround.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPercentTimeOnGround').value,event)")
		txtLogPageNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)") ''Commented by Saylee on 20-Aug-2018 as per requirement to allow entering alphanumeric 
	End Sub
	Private Sub NewRecord(ByVal LogDate As String, Optional ByVal mSouLocalDateTime As String = "", Optional ByVal mSouUTCDateTime As String = "")
		mLog = Log.NewLog(mMachine, LogDate, mSouLocalDateTime, mSouUTCDateTime)
		' mLog.BeginEdit()
		mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
		mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
		mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
		mMachine = Machine.GetMachine(mMachine.ID)
		DataBind()
		'''''CHECK_isRequiredAssembliesInstalled()
	End Sub
	Private Sub EditRecord(ByVal LogDate As DateTime)
		mLog = Log.GetLog(mLog.ID)
		' mLog.BeginEdit()
		mLog.Date = LogDate
		DataBind()
		'''''CHECK_isRequiredAssembliesInstalled()
	End Sub
	Private Sub CopyFromClone(ByVal ClonedLog As Log)
		mLog.PilotID1 = ClonedLog.PilotID1
		mLog.PilotID2 = ClonedLog.PilotID2
		mLog.IsUTC = ClonedLog.IsUTC
		mLog.SourceID = ClonedLog.SourceID
		'mLog.SouLocalDateTime = ClonedLog.SouLocalDateTime
		'' mLog.SouDayLightTime = ClonedLog.SouDayLightTime

		mLog.DestinationID = ClonedLog.DestinationID
		''mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
		''mLog.DesDayLightTime = ClonedLog.DesDayLightTime

		If Not mLog.IsNew Then
			mLog.SouLocalDateTime = ClonedLog.SouLocalDateTime
			mLog.SouDayLightTime = ClonedLog.SouDayLightTime
			mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
			mLog.DesDayLightTime = ClonedLog.DesDayLightTime
		End If


		mLog.TimeOnGround = ClonedLog.TimeOnGround
		mLog.PercentTimeOnGround = ClonedLog.PercentTimeOnGround
		mLog.TimeInAir = ClonedLog.TimeInAir
		mLog.Remark = ClonedLog.Remark

		mLog.LogPageNo = ClonedLog.LogPageNo
		mLog.FlightNo = ClonedLog.FlightNo
		mLog.FlightLogClassificationID = ClonedLog.FlightLogClassificationID
		mLog.FlightLogClassificationName = ClonedLog.FlightLogClassificationName

		'Hobbs - taken
		mLog.CurrentHobbsValue = ClonedLog.CurrentHobbsValue
		mLog.OffSet = ClonedLog.OffSet

		mLog.ImageFile = ClonedLog.ImageFile
		mLog.ImageSize = ClonedLog.ImageSize
		mLog.FileExtension = ClonedLog.FileExtension


		Session("mLog") = mLog
	End Sub
	'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013
	Private Function CheckZeroDifferenceValue() As Boolean
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
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogAPUAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogEngAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		checkcol = mLog.LogCGBAssemblies
		If Not callZeroDifferenceValue(checkcol) Then
			Return False
		End If
		Return True
	End Function
	Private Function callZeroDifferenceValue(ByVal obj) As Boolean
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
	End Function
	'End

	'Added By Vikrant on 01-Dec-2021 for PBH
	''Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
	''    Try
	''        If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022


	''            Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")
	''            If Not mPBH.MachineID.Equals(Guid.Empty) Then
	''                If CDate(mLog.Date) >= CDate(mPBH.StartDate) Then


	''                    mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
	''                    mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
	''                    mPBH.RemainingHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.ElapsedHoursDec, 1, False, False).Value
	''                    mPBH.LastLogDetails = TmpLog.DateFormatted
	''                    'For Not Active Case: If RemainingHours<=0 then mark Not Active flag
	''                    'Also mark Not InUse in tabMachine at same time 
	''                    If mPBH.RemainingHoursDec <= 0 Then
	''                        mPBH.IsNotActive = True
	''                        mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
	''                        mPBH.MachineNotInUse = True
	''                        Session("IsAircraftMadeNotInUse") = "True"
	''                    End If
	''                    mPBH.Save()
	''                End If
	''            End If
	''        ElseIf mCompanyDetail.IsCombinedHours = True Then 'PBH Collective Hrs by Saylee on 30-Nov-2022
	''            Dim mPBH As PBH
	''            If IsLogNew Then
	''                Dim mPBHList As PBHList = PBHList.GetList(IsAllRecordsRequired:=1)
	''                mPBH = PBH.GetPBH(mPBHList(0).ID)
	''                If CDate(mLog.Date) >= CDate(mPBH.StartDate) Then
	''                    mPBH.RemainingHours = New Period(1, mPBH.RemainingHoursDec - New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec, 1, False, False).Value
	''                    mPBH.ElapsedHours = New Period(1, mPBH.HoursFrequencyDec - mPBH.RemainingHoursDec, 1, False, False).Value
	''                    mPBH.LastLogDetails = TmpLog.DateFormatted
	''                    If mPBH.RemainingHoursDec <= 0 Then
	''                        mPBH.IsNotActive = True
	''                        mPBH.NotActiveDate = TmpLog.DateFormatted.ToString
	''                        mPBH.MachineNotInUse = True
	''                        Session("IsAircraftMadeNotInUse") = "True"
	''                    End If
	''                    mPBH.Save()
	''                End If

	''            End If
	''        End If
	''    Catch ex As Exception
	''        Throw ex
	''    End Try
	''End Sub
	Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
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
	Private Sub AttachMyFile()

		Dim BackupPath As String = ""
		BackupPath = AppSettings("DOCPath") & "New.PDF"
		mLog = Session("mLog")
		Try
			If Not mLog.FileAttachments.Contains(mLog.ID, CType(Session("FileUpload.FileName"), String)) Then

				mLog.FileAttachments.Add(mLog.ID, CType(Session("FileUpload.FileName"), String))
				' mLog.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
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
				MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
				Exit Sub
			End If
		Catch ex As Exception
		End Try
	End Sub
#End Region

#Region " Data Binding "
	Private Sub GridColumnHeadingSet() 'Added By Prashant 31-July-2009 To changed all grids heading "Cycles" to "Flights"  for "Heligo"
		'Changed By Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
		If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then
			dgAFPeriods.Columns(7).HeaderText = "Flights"
			dgAFPeriods.Columns(8).HeaderText = "Final Flights"
			dgEnginePeriods.Columns(7).HeaderText = "Flights"
			dgEnginePeriods.Columns(8).HeaderText = "Final Flights"
			dgAPUPeriods.Columns(7).HeaderText = "Flights"
			dgAPUPeriods.Columns(8).HeaderText = "Final Flights"
			dgCGBPeriods.Columns(7).HeaderText = "Flights"
			dgCGBPeriods.Columns(8).HeaderText = "Final Flights"
			lblCGBPeriod.Text = "CGB Period"
			'Dim txtCGBHours, txtCGBLandings, txtCGBCycles As TextBox, txtCGBStarts, txtCGBNGCycles, txtCGBNFCycles, txtCGBRins, txtCGBBleeds, txtCGBImpellerCycles, txtCGBCTCycles, txtCGBPTCycles, txtCGBGeneratorMods As TextBox

			' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgCGBPeriods.Items" is replaced by "dgCGBPeriods.Rows"
			For l As Integer = 0 To dgCGBPeriods.Rows.Count - 1
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBHours"), TextBox).ReadOnly = True
				CType(dgCGBPeriods.Rows(l).FindControl("txtCGBLandings"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBStarts"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNGCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNFCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBRins"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBBleeds"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBImpellerCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCTCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBPTCycles"), TextBox).ReadOnly = True

				'Added by Shweta on 7-May-2012  for ALL02052012
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBGeneratorMods"), TextBox).ReadOnly = True
			Next l
		End If
	End Sub
	Private Sub DataFieldBind()
		dgAFPeriods.DataSource = mLog.LogAFAssemblies
		dgEnginePeriods.DataSource = mLog.LogEngAssemblies
		dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
		dgCGBPeriods.DataSource = mLog.LogCGBAssemblies

		grdAllAssemblies.DataSource = mLog.ALL_LogAssemblies ''Added by Saylee on 1-Mar-2022

		txtLogNo.Text = mLog.LogNo
		txtLogText.Text = mLog.LogText

		'Added on 28-05-2007 by Kalpesh Shah
		'calDateTime.Text = mLog.Date.ToString
		'calDeparture.Text = mLog.SouLocalDateTime.ToString
		'calArrival.Text = mLog.DesLocalDateTime.ToString
		'CalUTCDateTime.Text = mLog.SouUniverseDateTime.ToString
		'CalUTCArrival.Text = mLog.DesUniverseDateTime.ToString

		'CNDC
		calDateTime.Value = mLog.Date
		calDeparture.Value = mLog.SouLocalDateTime
		calArrival.Value = mLog.DesLocalDateTime
		CalUTCDateTime.Value = mLog.SouUniverseDateTime
		CalUTCArrival.Value = mLog.DesUniverseDateTime
		txtBlockTime.Text = mLog.DiffTime
		'Prashant
		mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "<SELECT>")
		cmbFlightLogClassification.DataSource = mFlightLogClassificationList
		Session("mFlightLogClassificationList") = mFlightLogClassificationList
		'Code Added by DEVEN On 29/12/2007 --------------------------------------

		dgLogAttachment.DataSource = mLog.FileAttachments

		DataBind()
		GridColumnHeadingSet()
		If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
			cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
		Else
			cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
		End If
		'------------------------------------------------------------------------
		'If mLog.ImageSize > 0 Then
		'    ImageButton2.Visible = True
		'    btnDelAttach.Enabled = True
		'Else
		'    ImageButton2.Visible = False
		'    btnDelAttach.Enabled = False
		'End If

		mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
		Session("mLogListOnDate") = mLogListOnDate

		mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "") 'PBH Collective Hrs by Saylee on 30-Nov-2022
		Session("mCompanyDetail") = mCompanyDetail


		' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel
		upnlLogDetails.Update()
		upnlFlightDetails.Update()

		upnlAirframeDetail.Update()
		upnlEngineDetail.Update()
		upnlAPUDetail.Update()
		upnlCGBDetail.Update()

		upnlRemark.Update()
	End Sub
	'Code Added By Deven 21-03-2008 
	Private Sub BindClassification()
		mLog = CType(Session("mLog"), Log)
		mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "<SELECT>")
		cmbFlightLogClassification.DataSource = mFlightLogClassificationList
		Session("mFlightLogClassificationList") = mFlightLogClassificationList
		cmbFlightLogClassification.DataBind()
		If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
			cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
		Else
			cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
		End If
	End Sub
	'------------------------------------------------------------------------
	Private Sub DataBindGrid()
		If mLog IsNot Nothing Then
			'SetObject()

			SetAirFrameGridObject(True)
			SetEngineGridObject(True)
			SetAPUGridObject(True)
			SetCGBGridObject(True)

			dgAFPeriods.DataSource = mLog.LogAFAssemblies
			dgEnginePeriods.DataSource = mLog.LogEngAssemblies
			dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
			dgCGBPeriods.DataSource = mLog.LogCGBAssemblies


			dgAFPeriods.DataBind()
			dgEnginePeriods.DataBind()
			dgAPUPeriods.DataBind()
			dgCGBPeriods.DataBind()
			GridColumnHeadingSet()

			' '' ''AJAX- In DataFieldBind we binds object values to various controls. To reflect values we have call ".Update()" method of respective Panel
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

			Session("mLog") = mLog
		End If
	End Sub
	Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		GridColumnHeadingSet()
		If custValidator.ControlToValidate = "txtRemark" Then
			If Len(txtRemark.Text) > 500 Then
				custValidator.ErrorMessage = "Max. length of Remark should be 500 char"
				e.IsValid = False
			Else
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "calDeparture" Then
			'CNDC
			'If Not IsDate(calDeparture.Text) Then
			If Not (calDeparture.IsDateValue) Then
				custValidator.ErrorMessage = "Departure date should be in valid date time format."
				e.IsValid = False
			Else
				Dim Date1, Time1 As String
				Date1 = calDeparture.Value.ToString
				Time1 = calDeparture.Value.ToString
				'Date1 = CDate(calDeparture.Text).ToShortDateString()
				'Time1 = CDate(calDeparture.Text).ToShortTimeString()
				If Date1 = "1/1/0001" Then
					custValidator.ErrorMessage = "Departure date should be in valid date time format."
					e.IsValid = False
					Exit Sub
				End If
				'CNDC
				'calDeparture.Text = Date1 + " " + Time1
				calDeparture.Value = Date1

				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "calArrival" Then
			'CNDC
			'If Not IsDate(calArrival.Text) Then
			If Not (calArrival.IsDateValue) Then
				custValidator.ErrorMessage = "Arrival date should be in valid date time format."
				e.IsValid = False
			Else
				Dim Date1, Time1 As String
				'CNDC
				Date1 = calArrival.Value.ToString
				Time1 = calArrival.Value.ToString
				'Date1 = CDate(calArrival.Text).ToShortDateString()
				'Time1 = CDate(calArrival.Text).ToShortTimeString()
				If Date1 = "1/1/0001" Then
					custValidator.ErrorMessage = "Arrival date should be in valid date time format."
					e.IsValid = False
					Exit Sub
				End If
				'CNDC
				calArrival.Value = Date1
				'calArrival.Text = Date1 + " " + Time1
				e.IsValid = True
			End If
		End If
	End Sub
	Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
		If Flag = 1 Then Exit Sub
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject()
		SetAPUGridObject()
		SetCGBGridObject()
		GridColumnHeadingSet()
		Dim str As String = ""
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
			custValidator.ErrorMessage = str
			e.IsValid = False
		End If
		Flag = 1
	End Sub
	Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
		Dim str As String = ""
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
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		calDeparture.ShowTime = True
		calArrival.ShowTime = True
		CalUTCDateTime.ShowTime = True
		CalUTCArrival.ShowTime = True
		GetSession()
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		addAttributes()
		If Not IsPostBack And CType(Session("sender"), String) = "" Then
			If calDateTime.Enabled = True Then
				setFocus(calDateTime)
			End If
			SetFromSearch()
			DataFieldBind()

			If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
				'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
				upnlLogInfo.Update()
			End If
			upnlLogDetails.Update()
			''''''''''''''ControlVisibilityForAttachment()
		End If
		''  GridColumnHeadingSet()
		EnableDisableButton()
		ControlVisibility()

		' '' ''AJAX- "MessageBoxResult()" is commented here and called from new User Control Delegate event present at the bottom "MsgBoxCtrl_UserControlButtonClicked"
		' '' ''MessageBoxResult()
		DataBindGrid()

		SetTitle()

		mLog.LogPageNo = txtLogPageNo.Text.Trim  'Added By Utkarsh On 28-Nov-2011
		mLog.FlightNo = txtFlightNo.Text.Trim     'Added By Utkarsh On 28-Nov-2011
	End Sub
	Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

		If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
			'Code Added By Deven 21-03-2008 
			BindClassification()
			'-------------------------------
			SetObject()
			SetSession()
			mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Save, "Flight Log", User.Identity.Name & " is not Authorized User to save " & mLogDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)

			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			' '' ''msg.ReplacePage = "wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If

		If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

		'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
		If IsValid Then
			If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
				Dim Title As String = "Save Alert !"
				Dim Message As String = ""
				If Not mLog.PilotID1.Equals(Guid.Empty) Then
					Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
					If mEmployeeStatus(0).Information <> "" Then
						Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
					End If
				End If
				If Not mLog.PilotID2.Equals(Guid.Empty) Then
					Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
					If mEmployeeStatus(0).Information <> "" Then
						Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
					End If
				End If
				If Message.Length > 0 Then
					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If
		End If
		'End

		Dim mMaxLogOfAircraft As MaxLogOfAircraft
		mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)
		If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
			' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
			'If (AppSettings("ClientCode") <> "Heligo") Then
			If Not (AppSettings("ClientCode") = "Heligo" Or
					AppSettings("ClientCode") = "UHPL" Or
					AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
				'End
				'Added by Saylee on 18-May-2012 ALL17052012

				Dim MaxLogDateTime As String = ""
				If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1 Then
					MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted
				Else
					MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted
				End If

				If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogFlexiLog"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
					Exit Sub
				End If
			Else

				If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then   'Added by Saylee on 18-May-2012 ALL17052012
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogFlexiLog"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
					Exit Sub
				End If
			End If
		End If
		'Code Added By Deven on 15-01-2009 for Checking MEL Qty
		'Commented by Prashant 12-Apr-2010
		'Dim IsMELCount As Boolean = True
		'Dim MELList As Aircraft_MEL.MELList
		'MELList = Aircraft_MEL.MELList.GetMELList(mMachine.ID, Guid.Empty, calDateTime.Value.ToString)
		'For i As Integer = 0 To MELList.Count - 1
		'    If MELList(i).FlyMELQty <> CDec(MELList(i).CurrentMELQty) Then
		'        IsMELCount = False
		'        Exit For
		'    End If
		'Next
		'MELList = Nothing
		'-----------------------------------
		'Added By Prashant 12-Apr-2010
		Dim IsMELCount As Boolean = False
		Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
		mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
		For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
			'If (mTempMELSnagCorrectiveActionList(i).DueDate > calDateTime.Value) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = True) Then
			If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
				If (calDateTime.Value > mTempMELSnagCorrectiveActionList(i).DueDate) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
					IsMELCount = True
					Exit For
				Else
					IsMELCount = False
				End If
			End If
		Next
		mTempMELSnagCorrectiveActionList = Nothing
		'-----------------------------------
		If IsMELCount = True Then
			' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
			' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "MEL"
			' '' ''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MEL")

			If IsValid Then
				' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
				RefreshControlValues(True)
			End If
			Exit Sub
		End If
		'-------------------------------

		If IsValid Then
			If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
			If Save() = True Then
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
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
						MSGBoxCtrl.show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
						Exit Sub
					End If
				End If
				'End

				' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
				' '' ''Response.Redirect("wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage"))

				DataFieldBind()

				EnableDisableButton()
				ControlVisibility()
				'''''ControlVisibilityForAttachment()
				DataBindGrid()

				SetTitle()

				upnlLogDetails.Update()
				upnlFlightDetails.Update()
				upnlFlightSummary.Update()
				upnlTabs.Update()


			End If
		Else
			' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
			upnlErrorList.Update()
		End If
	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		Session("IsValid") = IsValid
		If mLog.IsDirty Then
			' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
			' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "Close"
			' '' ''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")

			If IsValid Then
				' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
				RefreshControlValues(True)
			Else
				' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
				upnlErrorList.Update()
			End If
		Else
			MarkLog(Util.Action.Close, "Flight Log", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

			RemoveSession()
			Response.Redirect(Request.QueryString("BackPage") & "?")
		End If
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
		If (Not User.IsInRole("LogPrint")) Then
			' '' ''    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
			' '' ''    msg.ReplacePage = "wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''    msg.Show()
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
			Exit Sub
		End If
	End Sub
	Private Sub btnAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
		'Code Added By Deven 21-03-2008 
		BindClassification()
		'-------------------------------
		SetObject()
		If (Not User.IsInRole("LogNew") And mLog.IsNew) Or (Not User.IsInRole("LogEdit") And Not mLog.IsNew) Then
			MarkLog(Util.Action.Save, "Flight Log", User.Identity.Name & " is not Authorized User to add ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
			' '' ''msg.ReplacePage = "wfLogDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

			Exit Sub
		End If

		If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

		'Added By Utkarsh ON 05-Aug-2013 FOR ALL01082013
		If IsValid Then
			If Not mLog.PilotID1.Equals(Guid.Empty) Or Not mLog.PilotID2.Equals(Guid.Empty) Then
				Dim Title As String = "Save Alert !"
				Dim Message As String = ""
				If Not mLog.PilotID1.Equals(Guid.Empty) Then
					Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID1.ToString, mLog.Date.ToString)
					If mEmployeeStatus(0).Information <> "" Then
						Message = "<b>Pilot in Command : </b> <br />" & mEmployeeStatus(0).Information
					End If
				End If
				If Not mLog.PilotID2.Equals(Guid.Empty) Then
					Dim mEmployeeStatus As EmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mLog.PilotID2.ToString, mLog.Date.ToString)
					If mEmployeeStatus(0).Information <> "" Then
						Message = IIf(Message.Length > 0, Message & "<br/ >", "") & "<b>Co-Pilot : </b> <br />" & mEmployeeStatus(0).Information
					End If
				End If
				If Message.Length > 0 Then
					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
			End If
		End If
		'End

		'Added by Saylee on 18-May-2012 ALL17052012
		Dim mMaxLogOfAircraft As MaxLogOfAircraft
		mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

		If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
			' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
			'If (AppSettings("ClientCode") <> "Heligo") Then
			If Not (AppSettings("ClientCode") = "Heligo" Or
					AppSettings("ClientCode") = "UHPL" Or
					AppSettings("ClientCode") = "APFT" Or
					AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
				'End
				Dim MaxLogDateTime As String = ""
				'If (AppSettings("LogBookTimeEntry") = "UTC") Then
				If mMachine.IsUTC Then '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
					MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted.ToString
				Else
					MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted.ToString
				End If

				If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogFlexiLog"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					Session("New") = "True"
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")

					Exit Sub
				End If
			Else
				If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogFlexiLog"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					Session("New") = "True"
					MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
					Exit Sub
				End If
			End If
		End If
		'End

		'Code Added By Deven on 15-01-2009 for Checking MEL Qty
		'Commented by Prashant 12-Apr-2010
		'Dim IsMELCount As Boolean = True
		'Dim MELList As Aircraft_MEL.MELList
		'MELList = Aircraft_MEL.MELList.GetMELList(mMachine.ID, Guid.Empty, calDateTime.Value.ToString)
		'For i As Integer = 0 To MELList.Count - 1
		'    If MELList(i).FlyMELQty <> CDec(MELList(i).CurrentMELQty) Then
		'        IsMELCount = False
		'        Exit For
		'    End If
		'Next
		'MELList = Nothing
		'-----------------------------------

		'Added By Prashant 12-Apr-2010
		Dim IsMELCount As Boolean = False
		Dim mTempMELSnagCorrectiveActionList As MELSnagCorrectiveActionList
		mTempMELSnagCorrectiveActionList = MELSnagCorrectiveActionList.GetMELSnagCorrectiveActionList(, , mMachine.ID.ToString)
		For i As Integer = 0 To mTempMELSnagCorrectiveActionList.Count - 1
			If mTempMELSnagCorrectiveActionList(i).IsMEL = True Then   'Added By Prashant 23-Sep-2010
				If (calDateTime.Value > mTempMELSnagCorrectiveActionList(i).DueDate) And (mTempMELSnagCorrectiveActionList(i).InvestigationStatus = False) Then
					IsMELCount = True
					Exit For
				Else
					IsMELCount = False
				End If
			End If
		Next
		mTempMELSnagCorrectiveActionList = Nothing
		'-----------------------------------
		If IsMELCount = True Then
			' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
			' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "MELNew"
			' '' ''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MELNew")

			If IsValid Then
				'BindClassification()
				' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
				RefreshControlValues(True)
			End If
			Exit Sub
		End If
		'-------------------------------

		''Code Added By Deven for Save and New 20/03/2008
		If IsValid Then
			If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
			Session("New") = "True"
			If Save() = True Then
				NewRecord()
				DataFieldBind()
				Session("mLog") = mLog

				'Added by Saylee on 14-July-2009
				Session("mAircraftInformationBoardList") = Nothing
				'*********************************
				'Added By Vikrant on 01-Dec-2021 for PBH
				If Session("IsAircraftMadeNotInUse") IsNot Nothing Then
					If Session("IsAircraftMadeNotInUse") = "True" Then
						Session.Remove("AircraftId")
						Session.Remove("IsAircraftMadeNotInUse")
						MSGBoxCtrl.show("Alert!", "", "Aircraft Subscription Hours expired after current log.</BR></BR>Aircraft will no longer be allowed to use in System", MsgBoxStyle.OkOnly, "AircraftMadeNotInUse")
						Exit Sub
					End If
				End If
				'End
				' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
				' '' ''Response.Redirect("wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage"))

				EnableDisableButton()
				ControlVisibility()
				'''''''''''ControlVisibilityForAttachment()
				DataBindGrid()

				SetTitle()

				mLog.LogPageNo = txtLogPageNo.Text.Trim  'Added By Utkarsh On 28-Nov-2011
				mLog.FlightNo = txtFlightNo.Text.Trim     'Added By Utkarsh On 28-Nov-2011

				mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
				Session("mLogListOnDate") = mLogListOnDate
				If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
					'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
					upnlLogInfo.Update()
				End If

				upnlLogDetails.Update()

			End If
		End If
		'************************************************************

	End Sub

	Private Sub imgbtnPilot1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnPilot1.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx&Type=Pilot")
	End Sub
	Private Sub imgbtnPilot2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnPilot2.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx&Type=Pilot&AddType=1")
	End Sub
	Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Dim mEmployee As Employee
		mEmployee = Employee.NewPilot()
		Session("mEmployee") = mEmployee

		Response.Redirect("wfPilot.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub

	Private Sub imgbtnArrPlace_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnArrPlace.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx&Type=Place&AddType=2")
	End Sub
	Private Sub btnAddArrPlace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddArrPlace.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)
		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub

	Private Sub imgbtnDepPlace_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDepPlace.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)
		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx&Type=Place&AddType=3")
	End Sub
	Private Sub btnAddDepPlace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDepPlace.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)
		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub

	Private Sub btnAddPlaces_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPlaces.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub

	Private Overloads Sub btnFlightLogClassification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightLogClassification.Click
		'SetObject()  Comeented By Saylee for bug-FLD10 (Maintenance) by Pramod
		'SetSession()
		Response.Redirect("wfFlightLogClassification.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub cmbFlightLogClassification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
		mLog.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
		mLog.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
		Session("mLog") = mLog
	End Sub

	'Added By Prashant 28-July-2009
	''''Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
	''''    ViewImage()
	''''End Sub
	Private Sub calDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDateTime.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007

			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), SmartDate.StringToDate(calDateTime.Text)) <> 0 Then
			If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.Date.ToString), New SmartDate(calDateTime.Value.ToString).Date) <> 0 Then
				REM: Clone the object
				Dim clnLog As Log
				clnLog = CType(mLog.Clone, Log)
				If mLog.IsNew Then
					'CNDC
					'NewRecord(calDateTime.Text)

					NewRecord(calDateTime.Value.ToString)
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(calDateTime.Text))
					EditRecord(calDateTime.Value.ToString)
				End If
				REM: Copy from Clone
				CopyFromClone(clnLog)
				DataFieldBind()
				'DataBind() 'Hobbs - taken
				'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
				'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
				'''''If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
				'''''Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
				'''''    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OKOnly)
				'''''    ' '' ''msg1.ReplacePage = "wfLogDetail.aspx?BackPage=" & Request.QueryString("BackPage")
				'''''    ' '' ''msg1.Show()
				'''''    MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, " ")
				'''''    Exit Sub
				'''''End If
			End If

			mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
			Session("mLogListOnDate") = mLogListOnDate
			If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then
				Dim str1 As String
				str1 = "delete_cookie();"
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
				upnlLogInfo.Update()
			End If
			upnlLogDetails.Update()

			'Added By utkarsh ON 30-sep-2013 for Log_ajax changes
			EnableDisableButton()
			ControlVisibility()
			'End
			'upnlFlightDetails.Update()
			'upnlFlightSummary.Update()
			'upnlAirframeDetail.Update()
			'upnlEngineDetail.Update()
			'upnlAPUDetail.Update()
			'upnlCGBDetail.Update()

		End If

	End Sub

	Private Sub calArrival_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calArrival.TextChanged
		If IsPostBack Then  'Added Code on May,29,2007
			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(calArrival.Value.ToString).Date) <> 0 Or _
				(calDeparture.Text = "") Then
				mLog.DesLocalDateTime = calArrival.Value
				CalUTCArrival.Value = mLog.DesUniverseDateTime
			End If
			txtAirBorneTime.DataBind()

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues(False)

		End If
	End Sub
	Private Sub calDeparture_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calDeparture.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007
			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(calDeparture.Value.ToString).Date) <> 0 Or _
				(calArrival.Text = "") Then
				REM: Clone the object
				Dim clnLog As Log
				clnLog = CType(mLog.Clone, Log)

				'CNDC
				'clnLog.SouLocalDateTime = calDeparture.Text
				clnLog.SouLocalDateTime = calDeparture.Value

				If mLog.IsNew Then
					'CNDC
					'NewRecord(calDateTime.Text, calDeparture.Text)
					NewRecord(calDateTime.Value.ToString, calDeparture.Value.ToString)
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(calDeparture.Text))
					EditRecord(New SmartDate(calDeparture.Value.ToString).Date)
				End If
				REM: Copy from Clone
				CopyFromClone(clnLog)
				DataFieldBind()
				'DataBind() 'Hobbs - taken
			End If

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues(False)

		End If
	End Sub

	Private Sub CalUTCArrival_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalUTCArrival.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007
			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(CalUTCArrival.Value.ToString).Date) <> 0 Or _
				(CalUTCDateTime.Text = "") Then
				mLog.DesUniverseDateTime = CalUTCArrival.Value
				calArrival.Value = mLog.DesLocalDateTime
			End If

			txtAirBorneTime.DataBind()

			If calDeparture.Value.ToString = "" Then
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

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues(False)

		End If
	End Sub
	Private Sub CalUTCDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalUTCDateTime.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007
			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), SmartDate.StringToDate(CalUTCDateTime.Text)) <> 0 Then
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), New SmartDate(CalUTCDateTime.Value.ToString).Date) <> 0 Or _
				(CalUTCArrival.Text = "") Then
				REM: Clone the object
				Dim clnLog As Log
				clnLog = CType(mLog.Clone, Log)

				'CNDC
				'clnLog.SouUniverseDateTime = CalUTCDateTime.Text
				clnLog.SouUniverseDateTime = CalUTCDateTime.Value

				If mLog.IsNew Then
					'CNDC
					'NewRecord(calDateTime.Text, , CalUTCDateTime.Text)
					NewRecord(calDateTime.Value.ToString, , CalUTCDateTime.Value.ToString)
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(CalUTCDateTime.Text))
					EditRecord(New SmartDate(CalUTCDateTime.Value.ToString).Date)
				End If
				REM: Copy from Clone
				CopyFromClone(clnLog)
				DataFieldBind()
				'DataBind() 'Hobbs - taken
			End If

			txtAirBorneTime.DataBind()

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues(False)

		End If
	End Sub

	Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click

		''''SetObject()
		''''SetAirFrameGridObject()
		''''SetEngineGridObject(True)
		''''SetAPUGridObject(True)
		''''SetCGBGridObject(True)

		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Session("OpenFromWO") = False
		Session("mOpenFromLogFuelNew") = False
		'-------------------------------
		Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Session("Edit") = False
		'-------------------------------
		Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub btnLogPax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		NewLogPax()
		'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
		Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub btnHobbsOffset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		NewHobbsOffSet()
		Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub btnParameterList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(True)

		Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogDetail_Ajax.aspx")
	End Sub
	Private Sub lnkAllAssembly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllAssembly.Click
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowAssembly", "ShowAssembly();", True)
	End Sub
#End Region

#Region " Air Frame Grid "
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameHours As TextBox = TryCast(currentRow.FindControl("txtAirFrameHours"), TextBox)
		mLog.LogAFAssemblies.Item(currentRow.RowIndex).Hours = Trim(txtAirFrameHours.Text)
		DataBindGrid()

	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameLandings As TextBox = TryCast(currentRow.FindControl("txtAirFrameLandings"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).Landings = Trim(txtAirFrameLandings.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).Cycles = Trim(txtAirFrameCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameStarts As TextBox = TryCast(currentRow.FindControl("txtAirFrameStarts"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).Starts = Trim(txtAirFrameStarts.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameNGCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameNGCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).NGCycles = Trim(txtAirFrameNGCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameNFCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameNFCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).NFCycles = Trim(txtAirFrameNFCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameRins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameRins As TextBox = TryCast(currentRow.FindControl("txtAirFrameRins"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).RINS = Trim(txtAirFrameRins.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameBleeds As TextBox = TryCast(currentRow.FindControl("txtAirFrameBleeds"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).Bleeds = Trim(txtAirFrameBleeds.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameImpellerCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtAirFrameImpellerCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFrameCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameCTCycles As TextBox = TryCast(currentRow.FindControl("txtAirFrameCTCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).CTCycles = Trim(txtAirFrameCTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirFramePTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFramePTCycles As TextBox = TryCast(currentRow.FindControl("txtAirFramePTCycles"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).PTCycles = Trim(txtAirFramePTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAirframeGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAirFrameGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtAirFrameGeneratorMods"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtAirFrameGeneratorMods.Text)
		DataBindGrid()
	End Sub

#End Region

#Region " Engine Grid "
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineHours As TextBox = TryCast(currentRow.FindControl("txtEngineHours"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).Hours = Trim(txtEngineHours.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineLandings As TextBox = TryCast(currentRow.FindControl("txtEngineLandings"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).Landings = Trim(txtEngineLandings.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineCycles As TextBox = TryCast(currentRow.FindControl("txtEngineCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).Cycles = Trim(txtEngineCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineStarts As TextBox = TryCast(currentRow.FindControl("txtEngineStarts"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).Starts = Trim(txtEngineStarts.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub btnEngineNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineNGCycles As TextBox = TryCast(currentRow.FindControl("txtEngineNGCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).NGCycles = Trim(txtEngineNGCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineNFCycles As TextBox = TryCast(currentRow.FindControl("txtEngineNFCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).NFCycles = Trim(txtEngineNFCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineRins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineRins As TextBox = TryCast(currentRow.FindControl("txtEngineRins"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).RINS = Trim(txtEngineRins.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineCFactors_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineCFactors As TextBox = TryCast(currentRow.FindControl("txtEngineCFactors"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).CFactor = Trim(txtEngineCFactors.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineBleeds As TextBox = TryCast(currentRow.FindControl("txtEngineBleeds"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).Bleeds = Trim(txtEngineBleeds.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtEngineImpellerCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtEngineImpellerCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineCTCycles As TextBox = TryCast(currentRow.FindControl("txtEngineCTCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).CTCycles = Trim(txtEngineCTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEnginePTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEnginePTCycles As TextBox = TryCast(currentRow.FindControl("txtEnginePTCycles"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).PTCycles = Trim(txtEnginePTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtEngineGeneratorMods"), TextBox)
		mLog.LogAFAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtEngineGeneratorMods.Text)
		DataBindGrid()
	End Sub
	'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtEngineRapidTakeOffFactor_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtEngineRapidTakeOffFactor As TextBox = TryCast(currentRow.FindControl("txtEngineRapidTakeOffFactor"), TextBox)
		mLog.LogEngAssemblies(currentRow.RowIndex).RapidTakeOffFactor = Trim(txtEngineRapidTakeOffFactor.Text)
		DataBindGrid()
	End Sub
#End Region

#Region " APU Grid "
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUHours As TextBox = TryCast(currentRow.FindControl("txtAPUHours"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Hours = Trim(txtAPUHours.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPULandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPULandings As TextBox = TryCast(currentRow.FindControl("txtAPULandings"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Landings = Trim(txtAPULandings.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Cycles = Trim(txtAPUCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUStarts As TextBox = TryCast(currentRow.FindControl("txtAPUStarts"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Starts = Trim(txtAPUStarts.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUNGCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNGCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).NGCycles = Trim(txtAPUNGCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUNFCycles As TextBox = TryCast(currentRow.FindControl("txtAPUNFCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).NFCycles = Trim(txtAPUNFCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPURins_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPURins As TextBox = TryCast(currentRow.FindControl("txtAPURins"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).RINS = Trim(txtAPURins.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUBleeds As TextBox = TryCast(currentRow.FindControl("txtAPUBleeds"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).Bleeds = Trim(txtAPUBleeds.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtAPUImpellerCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtAPUImpellerCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUCTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUCTCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).CTCycles = Trim(txtAPUCTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUPTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUPTCycles As TextBox = TryCast(currentRow.FindControl("txtAPUPTCycles"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).PTCycles = Trim(txtAPUPTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtAPUGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtAPUPGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtAPUPGeneratorMods"), TextBox)
		mLog.LogAPUAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtAPUPGeneratorMods.Text)
		DataBindGrid()
	End Sub
#End Region

#Region " CGB Grid "
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBHours_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBHours As TextBox = TryCast(currentRow.FindControl("txtCGBHours"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).Hours = Trim(txtCGBHours.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBLandings_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBLandings As TextBox = TryCast(currentRow.FindControl("txtCGBLandings"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).Landings = Trim(txtCGBLandings.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBCycles As TextBox = TryCast(currentRow.FindControl("txtCGBCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).Cycles = Trim(txtCGBCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBStarts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBStarts As TextBox = TryCast(currentRow.FindControl("txtCGBStarts"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).Starts = Trim(txtCGBStarts.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBNGCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBNGCycles As TextBox = TryCast(currentRow.FindControl("txtCGBNGCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).NGCycles = Trim(txtCGBNGCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBNFCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBNFCycles As TextBox = TryCast(currentRow.FindControl("txtCGBNFCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).NFCycles = Trim(txtCGBNFCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBRINS_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBRins As TextBox = TryCast(currentRow.FindControl("txtCGBRins"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).RINS = Trim(txtCGBRins.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBBleeds_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBBleeds As TextBox = TryCast(currentRow.FindControl("txtCGBBleeds"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).Bleeds = Trim(txtCGBBleeds.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBImpellerCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBImpellerCycles As TextBox = TryCast(currentRow.FindControl("txtCGBImpellerCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).ImpellerCycles = Trim(txtCGBImpellerCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBCTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBCTCycles As TextBox = TryCast(currentRow.FindControl("txtCGBCTCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).CTCycles = Trim(txtCGBCTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBPTCycles_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBPTCycles As TextBox = TryCast(currentRow.FindControl("txtCGBPTCycles"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).PTCycles = Trim(txtCGBPTCycles.Text)
		DataBindGrid()
	End Sub
	' '' ''AJAX- "Refresh" buttons removed from DataaGrid and new "OnTextChanged" event of every Textbox in Grid called to set TextBox value to Object
	Protected Sub txtCGBGeneratorMods_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim currentRow As GridViewRow = CType(sender, TextBox).Parent.Parent
		Dim txtCGBGeneratorMods As TextBox = TryCast(currentRow.FindControl("txtCGBGeneratorMods"), TextBox)
		mLog.LogCGBAssemblies(currentRow.RowIndex).GeneratorMods = Trim(txtCGBGeneratorMods.Text)
		DataBindGrid()
	End Sub

#End Region

	' '' ''AJAX- New Event for MessageBox Control 
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

	' '' ''AJAX- New Method
	Private Sub RefreshControlValues(ByVal isFromDataBindGrid As Boolean)
		SetObject()
		SetAirFrameGridObject(isFromDataBindGrid)
		SetEngineGridObject(isFromDataBindGrid)
		SetAPUGridObject(isFromDataBindGrid)
		SetCGBGridObject(isFromDataBindGrid)

		DataFieldBind()

		Session("mLog") = mLog
		ControlVisibility()
		EnableDisableButton()
		DataBind()

		upnlFlightSummary.Update()
		upnlAirframeDetail.Update()
		upnlEngineDetail.Update()
		upnlAPUDetail.Update()
		upnlCGBDetail.Update()
	End Sub

	' '' ''AJAX- New Event
	Protected Sub txtPercentTimeOnGround_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPercentTimeOnGround.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(False)

	End Sub

	' '' ''AJAX- New Event
	Protected Sub txtGroundRunTime_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtGroundRunTime.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		If mLog.IsLogAirborneEntry = True Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True
			mLog.TimeOnGround = Trim(txtGroundRunTime.Text)
			Session("mLog") = mLog
		End If
		RefreshControlValues(False)

		txtPercentTimeOnGround.Focus()
	End Sub

	' '' ''AJAX- New Event
	Protected Sub txtAirBorneTime_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtAirBorneTime.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(False)

		txtGroundRunTime.Focus()
	End Sub
	Protected Sub txtBlockTime_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBlockTime.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(False)
		txtAirBorneTime.Focus()
	End Sub

	' '' ''AJAX- New Event
	Protected Sub txtCurrentHobbsValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCurrentHobbsValue.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues(False)
	End Sub
	' '' ''AJAX- New Event to attached Browse File.
	'Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
	'    mLog.IsAttachmentAdded = True
	'    ControlVisibilityForAttachment()
	'    upnlFileupload.Update()
	'End Sub
	Private Sub dgLogAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgLogAttachment.RowCommand
		Dim mFileAttachments As FileAttachments
		Select Case e.CommandName
			Case "View"
				Dim Index As Integer = CInt(e.CommandArgument) '+ dgLogAttachment.PageSize * dgLogAttachment.PageIndex

				Dim No As New Random
				Dim StrName As String = "abc" & No.Next.ToString
				mFileAttachments = mLog.FileAttachments
				'mFileAttachments.CurrentIndex = Index - 1

				If mFileAttachments.Count = 1 Then
					mFileAttachments.CurrentIndex = 0
				Else
					mFileAttachments.CurrentIndex = Index - 1
				End If

				If mFileAttachments.CurrentItem.Size > 0 Then
					Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
					Dim fs As FileStream
					If File.Exists(AppSettings("DOCPath")) = False Then
						'Delete File if exist
						System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
						' Create the file.
						fs = File.Create(path)
						'' Add some information to the file.
						fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
						fs.Close()
						Session("DOCPath") = path
						ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
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
				mFileAttachments = mLog.FileAttachments
				If mFileAttachments.Count = 1 Then
					DeleteAttachment(0)
				Else
					DeleteAttachment(Index - 1)
				End If
		End Select

	End Sub
	Private Sub DeleteAttachment(ByVal Index As Int32)
		MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
		mLog.FileAttachments.CurrentIndex = Index
		Session("mLog") = mLog
	End Sub
	Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
		AttachMyFile()
		upnlLogAttachment.Update()
	End Sub
	Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
		SetObject()
		Session("mLog") = mLog
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
	End Sub
	'Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttch.Click
	'    Dim fileSize1 As Integer = 0
	'    Dim file1(fileSize1) As Byte
	'    GetAttachment()
	'    mFileAttach.ImageFile = file1
	'    mFileAttach.Size = 0
	'    ImageButton1.Visible = False
	'    btnDelAttch.Enabled = False
	'    IsAttachmentDeleted = True
	'    mLog.IsAttachmentAdded = False
	'    Session("IsAttachmentDeleted") = IsAttachmentDeleted
	'End Sub
	'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
	'    If mLog.IsAttachmentAdded Then
	'        mFileAttach = FileAttach.GetAttachment(mLog.ID)
	'    Else
	'        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mLog.ID)
	'    End If
	'    Session("mFileAttach") = mFileAttach
	'End Sub
End Class
