'Prashant
Imports System.Web.Services
Public Class wfLogSOPEdit_Ajax
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
	Dim mLogDetail As String

	Public mSearchListPilot As SearchList
	Public mSearchListPlace As SearchList
	'Added By Utkarsh On 21-Sep-2011
	Dim TakeOffTouchDown As Boolean
	Dim Pilot1ID As Guid
	Dim Pilot2ID As Guid
	Dim SourceID As Guid
	Dim DestinationID As Guid
	Dim SetValue As Boolean = False
	Dim IsValueZero As Boolean = False
	Public Event TextChanged As EventHandler
	'End
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

		mSearchListPlace = Session("mSearchListPlace")
		mSearchListPilot = Session("mSearchListPilot")
		'Added By Utkarsh On 21-Sep-2011
		Pilot1ID = CType(Session("Pilot1ID"), Guid)
		Pilot2ID = CType(Session("Pilot2ID"), Guid)
		SourceID = CType(Session("SourceID"), Guid)
		DestinationID = CType(Session("DestinationID"), Guid)
		SetValue = CType(Session("SetValue"), Boolean)
		'End
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

		Session("mSearchListPlace") = mSearchListPlace
		Session("mSearchListPilot") = mSearchListPilot

		'Added By Utkarsh On 21-Sep-2011
		Session("Pilot1ID") = Pilot1ID
		Session("Pilot2ID") = Pilot2ID
		Session("SourceID") = SourceID
		Session("DestinationID") = DestinationID
		Session("SetValue") = SetValue
		'End
		Session("mLogListOnDate") = mLogListOnDate
		Session("mCompanyDetail") = mCompanyDetail 'PBH Collective Hrs by Saylee on 30-Nov-2022
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
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
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

		'btnLogPax.Enabled = Not mLog.IsNew
		'btnDefectActionList.Enabled = Not mLog.IsNew
		'pnlPilot-Enabled
		calDateTime.Enabled = mLog.IsNew
		'btnParameterList.Enabled = Not mLog.IsNew 'Added by Saylee on 6-Sep-2012
		'btnFuelOil.Enabled = Not mLog.IsNew       'Added by Saylee on 6-Sep-2012
		'btnFlightCrew.Enabled = Not mLog.IsNew      'Added by Saylee on 6-Sep-2012
		'btnMaintenanceAcitvity.Enabled = Not mLog.IsNew 'Utkarsh
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
		Else                                                        ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
			txtAirBorneTime.BackColor = Color.White
			txtGroundRunTime.BackColor = Color.White
			txtPercentTimeOnGround.BackColor = Color.White

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
			btnAddPlace.Visible = False
			btnAddPilots.Visible = False
		End If
		'Date 
		''If mLogList.Count = 0 Then
		If LogListCount = 0 Then
			calDeparture.Enabled = True  ''And mMachine.HourType = 1
			calArrival.Enabled = True ''And mMachine.HourType = 1
			calDeparture.ReadOnly = Not (True) '' And mMachine.HourType = 1)
			calArrival.ReadOnly = Not (True) '' And mMachine.HourType = 1)


			'txtDepartureTime.Enabled = True       'Commented on 04-Mar-2022 by shital
			'txtArrivalTime.Enabled = True             'Commented on 04-Mar-2022 by shital
			'txtDepartureTime.ReadOnly = Not (True)     'Commented on 04-Mar-2022 by shital
			'txtArrivalTime.ReadOnly = Not (True)       'Commented on 04-Mar-2022 by shital

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
		''If mLogList.Count > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") = "9999" Then
		''    calDeparture.Visible = False
		''    calArrival.Enabled = False
		''    calDeparture.ReadOnly = True
		''    calArrival.ReadOnly = True
		''End If
		'' If mLogList.Count > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString = "" Then
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

		'Commented by utkarsh on 01-oct-2013 for log_ajax changes
		'If Not calDeparture.Enabled Then
		'    calDeparture.BackColor = Color.Gainsboro
		'End If
		'If Not calArrival.Enabled Then
		'    calArrival.BackColor = Color.Gainsboro
		'End If
		'-End Date


		'Added By Utkarsh On 31-Aug-2011

		If Not mLog.IsNew Then
			' calDeparture.Enabled = False    '04-Mar-2022
			' calArrival.Enabled = False      '04-Mar-2022
			CalUTCDateTime.Enabled = False
			CalUTCArrival.Enabled = False

			'Commented By Utkarsh On 19-Apr-2012 For ALL19042012

			'Pilot1.Enabled = False
			'Pilot2.Enabled = False

			'End

			'Commented on 04-Mar-2022 by shital
			Place1.Enabled = False
			Place2.Enabled = False
			'---

			'Commented By Utkarsh On 19-Apr-2012 For ALL19042012

			'Pilot1.BackColor = Color.Gainsboro
			'Pilot2.BackColor = Color.Gainsboro

			'End
			Place1.BackColor = Color.Gainsboro   '04-Mar-2022
			Place2.BackColor = Color.Gainsboro      '04-Mar-2022

			'If takeofftouchdown Then

			'    calTakeOffLocalDateTime.Enabled = False
			'    calUTCTakeOffDateTime.Enabled = False
			'    calTouchDownLocalDateTime.Enabled = False
			'    calUTCTouchDownDateTime.Enabled = False
			'End If
			' commented on 04-Mar-2022 by Shital
			'txtDepartureTime.Enabled = False
			'txtArrivalTime.Enabled = False
			'txtUTCDepartureTime.Enabled = False
			'txtUTCArrivalTime.Enabled = False
			' chkArrival.Enabled = False          '04-Mar-2022
			' chkTouchDown.Enabled = False         '04-Mar-2022
			' chkTakeOff.Enabled = False               '04-Mar-2022
			'---------End
		Else                                                    ' '' ''AJAX-Else case explicitly added bcaz after partial postback (Save&New) controls have to refresh.
			Place1.Enabled = True
			Place2.Enabled = True
			'Place1.ReadOnly = False 04-Mar-2022
			' Place2.ReadOnly = False   04-Mar-2022
			Place1.BackColor = Color.White
			Place2.BackColor = Color.White
			chkArrival.Enabled = True
			chkTouchDown.Enabled = True
			chkTakeOff.Enabled = True
		End If

		'End

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
		' btnHobbsOffset.Enabled = (mMachine.HourType = 2)
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

		'Added By Utkarsh On 31-Aug-2011

		If TakeOffTouchDown And mLog.IsLogAirborneEntry = False Then  'Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = False
			txtAirBorneTime.BackColor = Color.Gainsboro
			txtGroundRunTime.BackColor = Color.Gainsboro
			txtAirBorneTime.ReadOnly = True
			txtGroundRunTime.ReadOnly = True
		End If

		'commenetd and Added  By Saylee On 12-Feb-2014 For ALL12022014-1
		''''lblTakeOffLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''lblUTCTakeOffDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''lblTouchDownLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''lblUTCTouchDownDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)

		''''calTouchDownLocalDateTime.Enabled = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown And mLog.IsNew)
		''''calUTCTouchDownDateTime.Enabled = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown And mLog.IsNew)
		''''calTakeOffLocalDateTime.Enabled = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown And mLog.IsNew)
		''''calUTCTakeOffDateTime.Enabled = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown And mLog.IsNew)

		''''calTouchDownLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''calUTCTouchDownDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''calTakeOffLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		''''calUTCTakeOffDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)

		lblTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		lblUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
		lblTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		lblUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)

		' commented on 04-Mar-2022 by Shital
		calTouchDownLocalDateTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		calUTCTouchDownDateTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown)
		calTakeOffLocalDateTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		calUTCTakeOffDateTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown)

		calTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		calUTCTouchDownDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
		calTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		calUTCTakeOffDateTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)

		txtTouchDownLocalTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		txtUTCTouchDownTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown)
		txtTakeOffLocalTime.Enabled = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		txtUTCTakeOffTime.Enabled = ((mMachine.IsUTC) And TakeOffTouchDown)

		txtTakeOffLocalTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		txtUTCTakeOffTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
		txtTouchDownLocalTime.Visible = (Not (mMachine.IsUTC) And TakeOffTouchDown)
		txtUTCTouchDownTime.Visible = ((mMachine.IsUTC) And TakeOffTouchDown)
		'---End--

		chkTakeOff.Visible = TakeOffTouchDown
		chkTouchDown.Visible = TakeOffTouchDown
		' '' ''btnTakeOffDate.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		' '' ''btnUTCTakeOffDate.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		' '' ''btnTouchDowndate.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)
		' '' ''btnUTCTouchDowndate.Visible = ((AppSettings("LogBookTimeEntry") = "UTC") And takeofftouchdown)


		'End

		'pnlHours

		'Added By Utkarsh On 05-Sep-2011
		If Not TakeOffTouchDown Then
			txtAirBorneTime.ReadOnly = Not mLog.IsNew
		End If
		'End
		txtCurrentHobbsValue.ReadOnly = Not mLog.IsNew
		'This change is made to change the LogBook Time Entry Format. ------By Devendra
		'Local Entry Setting
		'''calDeparture.Enabled = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''calArrival.Enabled = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''CalUTCDateTime.Enabled = (AppSettings("LogBookTimeEntry") = "UTC")
		'''CalUTCArrival.Enabled = (AppSettings("LogBookTimeEntry") = "UTC")

		'''calDeparture.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblDepDateTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblDateTimeStar1.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

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


		' '' ''btnDepartureDate.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

		'''calArrival.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblDateTimeStar2.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblArrDate.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
		'''' '' ''btnArrivaldate.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

		'''CalUTCDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblUTCDateTimeStar1.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblUTCDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''' '' ''btnUTCDepartureDate.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

		'''CalUTCArrival.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblUTCDateTimeStar2.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''lblUTCArrivalDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
		'''' '' ''btnUTCArrivaldate.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

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
			CalUTCDateTime.Enabled = False

			'Commented by Shital on 04-Mar-2022
			' calArrival.Enabled = False
			' CalUTCArrival.Enabled = False
			'----

			'Commented By Utkarsh On 19-Apr-2012 For ALL19042012
			'Pilot1.ReadOnly = True
			'Pilot2.ReadOnly = True
			'End


			'  Place1.ReadOnly = True        04-Mar-2022
			' Place2.ReadOnly = True         04-Mar-2022

			'Commented By Utkarsh On 19-Apr-2012 For ALL19042012
			'Pilot1.BackColor = Color.Gainsboro
			'Pilot2.BackColor = Color.Gainsboro
			'End

			'Place1.BackColor = Color.Gainsboro   04-Mar-2022
			' Place2.BackColor = Color.Gainsboro    04-Mar-2022

			'If takeofftouchdown Then

			'    calTakeOffLocalDateTime.Enabled = False
			'    calUTCTakeOffDateTime.Enabled = False
			'    calTouchDownLocalDateTime.Enabled = False
			'    calUTCTouchDownDateTime.Enabled = False
			'End If

			' commented on 04-Mar-2022 by Shital
			'txtDepartureTime.Enabled = False
			'txtArrivalTime.Enabled = False
			'txtUTCDepartureTime.Enabled = False
			'txtUTCArrivalTime.Enabled = False
			'----End
		End If
		'End



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
		'-------------------------------------------
		'Generator Mods
		dgEnginePeriods.Columns(27).Visible = mLog.LogEngAssemblies.ShowGeneratorMods
		dgEnginePeriods.Columns(28).Visible = mLog.LogEngAssemblies.ShowGeneratorMods
		'-----------------------------------------End


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
		'-------------------------------------------
		'Added by Shweta on 7-May-2012 for ALL02052012
		'-------------------------------------------
		'Generator Mods
		dgAPUPeriods.Columns(25).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
		dgAPUPeriods.Columns(26).Visible = mLog.LogAPUAssemblies.ShowGeneratorMods
		'-----------------------------------------End

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
		'-------------------------------------------
		'Generator Mods
		dgCGBPeriods.Columns(25).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
		dgCGBPeriods.Columns(26).Visible = mLog.LogCGBAssemblies.ShowGeneratorMods
		'-----------------------------------------End

		'code added by DEVEN 24-03-2008
		'''''''''''''calDeparture.ShowClearButton = False
		'''''''''''''calArrival.ShowClearButton = False
		'''''''''''''CalUTCArrival.ShowClearButton = False
		'''''''''''''CalUTCDateTime.ShowClearButton = False
		'''''''''''''calDateTime.ShowClearButton = False
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


		'=====================================
		''Added by Saylee on 1-Mar-2022
		'ALL Assembly ----> 
		'Hours
		grdAllAssemblies.Columns(4).Visible = mLog.ALL_LogAssemblies.ShowHours
		grdAllAssemblies.Columns(5).Visible = mLog.ALL_LogAssemblies.ShowHours

		'Landings
		grdAllAssemblies.Columns(6).Visible = mLog.LogAFAssemblies.ShowLandings
		grdAllAssemblies.Columns(7).Visible = mLog.LogAFAssemblies.ShowLandings
		'Cycles
		grdAllAssemblies.Columns(8).Visible = mLog.LogAFAssemblies.ShowCycles
		grdAllAssemblies.Columns(9).Visible = mLog.LogAFAssemblies.ShowCycles
		'Starts
		grdAllAssemblies.Columns(10).Visible = mLog.LogAFAssemblies.ShowStarts
		grdAllAssemblies.Columns(11).Visible = mLog.LogAFAssemblies.ShowStarts
		'NG
		grdAllAssemblies.Columns(12).Visible = mLog.LogAFAssemblies.ShowNGCycles
		grdAllAssemblies.Columns(13).Visible = mLog.LogAFAssemblies.ShowNGCycles
		'NF
		grdAllAssemblies.Columns(14).Visible = mLog.LogAFAssemblies.ShowNFCycles
		dgAFPeriods.Columns(15).Visible = mLog.LogAFAssemblies.ShowNFCycles
		'RINS
		grdAllAssemblies.Columns(16).Visible = mLog.LogAFAssemblies.ShowRINS
		grdAllAssemblies.Columns(17).Visible = mLog.LogAFAssemblies.ShowRINS
		'Bleeds  'Added By Prashant 8-July-2009
		grdAllAssemblies.Columns(18).Visible = mLog.LogAFAssemblies.ShowBleeds
		grdAllAssemblies.Columns(19).Visible = mLog.LogAFAssemblies.ShowBleeds
		'ImpellerCycles  'Added By Prashant 10-Aug-2009
		grdAllAssemblies.Columns(20).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
		grdAllAssemblies.Columns(21).Visible = mLog.LogAFAssemblies.ShowImpellerCycles
		'CTCycles
		grdAllAssemblies.Columns(22).Visible = mLog.LogAFAssemblies.ShowCTCycles
		grdAllAssemblies.Columns(23).Visible = mLog.LogAFAssemblies.ShowCTCycles
		'PTCycles
		grdAllAssemblies.Columns(24).Visible = mLog.LogAFAssemblies.ShowPTCycles
		grdAllAssemblies.Columns(25).Visible = mLog.LogAFAssemblies.ShowPTCycles
		'--------------------------------------
		'Added by Shweta on 7-May-2012 for ALL02052012
		'Generator Mods
		grdAllAssemblies.Columns(26).Visible = mLog.LogAFAssemblies.ShowGeneratorMods
		grdAllAssemblies.Columns(27).Visible = mLog.LogAFAssemblies.ShowGeneratorMods



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
			If mLog.LogAPUAssemblies(i).ShowRINS Then
				If (Val(mLog.LogAPUAssemblies(i).RINS) = 0) Then
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
			'Added by shweta on 7-May-2012 for ALL02052012
			If mLog.LogAPUAssemblies.ShowGeneratorMods Then
				If Val(mLog.LogAPUAssemblies(i).GeneratorMods) = 0 Then
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
				'----------------------------- 
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
			''''''''''''If Not (calDateTime.IsDateValue) Then
			If Not IsDate(calDateTime.Text) Then
				.Date = System.DBNull.Value
			Else
				.Date = calDateTime.Text.ToString.Trim
			End If

			.LogText = Trim(txtLogText.Text)
			'.LogNo = Trim(txtLogNo.Text)
			.LogNo = CInt(Val(Trim(txtLogNo.Text)))
			If .IsUTC = True Then
				'CNDC
				''''''''''''If Not (CalUTCDateTime.IsDateValue) Then
				If Not IsDate(CalUTCDateTime.Text) Then
					.SouUniverseDateTime = System.DBNull.Value
				Else
					.SouUniverseDateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)
				End If


			Else

				'CNDC ''''''''''''''If Not (calDeparture.IsDateValue) Then
				If Not IsDate(calDeparture.Text) Then
					.SouLocalDateTime = System.DBNull.Value
				Else
					.SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime)
					' .SouLocalDateTime = CType(calDeparture.Text.ToString + " " + calDepartureTime.Text.ToString, DateTime)
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
				'''''''''''''''If Not (CalUTCArrival.IsDateValue) Then
				If Not IsDate(CalUTCArrival.Text) Then
					.DesUniverseDateTime = System.DBNull.Value
				Else
					.DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime)
				End If
				'If Not IsDate(CalUTCArrival.Text) Then
				'    .DesUniverseDateTime = System.DBNull.Value
				'Else
				'    .DesUniverseDateTime = CType(Trim(CalUTCArrival.Text), Object)
				'End If
			Else
				'CNDC

				If Not IsDate(calArrival.Text) Then
					.DesLocalDateTime = System.DBNull.Value
				Else
					.DesLocalDateTime = CType(calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim, DateTime)
				End If
			End If
			.DesDayLightTime = cmbArrivalDayLightTime.SelectedValue
			'Added By Utkarsh On 31-Aug-2011

			If .IsUTC Then
				If TakeOffTouchDown Then
					If Not IsDate(calUTCTakeOffDateTime.Text) Then
						.TakeOffUniverseDateTime = System.DBNull.Value
					Else
						.TakeOffUniverseDateTime = CType(calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim, DateTime)
					End If

					If Not IsDate(calUTCTouchDownDateTime.Text) Then
						.TouchDownUniverseDateTime = System.DBNull.Value
					Else
						.TouchDownUniverseDateTime = CType(calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim, DateTime)
					End If
				End If
				'End
			Else
				'Added By Utkarsh On 31-Aug-2011

				If TakeOffTouchDown Then
					If Not IsDate(calTakeOffLocalDateTime.Text) Then
						.TakeOffLocalDateTime = System.DBNull.Value
					Else
						.TakeOffLocalDateTime = CType(calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim, DateTime)
					End If

					If Not IsDate(calTouchDownLocalDateTime.Text) Then
						.TouchDownLocalDateTime = System.DBNull.Value
					Else
						.TouchDownLocalDateTime = CType(calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim, DateTime)
					End If
				End If
			End If
			'End     

			'If Not takeofftouchdown Then
			'    .TimeInAir = Trim(txtAirBorneTime.Text)
			'End If
			'If Not AppSettings("Log") = "True" Then .TimeOnGround = Trim(txtGroundRunTime.Text)
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
			If Session("IsValueZero") = "True" Then
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
			'''Added by Saylee on 4-Apr-2022, only for edited logs
			.RemarkEdit = "<NewRemark><Update> Update Due to Log Edit</Update><UserName>" + User.Identity.Name + "</UserName><LogDet>" + .LogTextNo + "</LogDet><UpdateDateTime>" + New SmartDate(Now.ToString("yyyy/MM/dd HH:mm")).FormattedTextTime + "</UpdateDateTime></NewRemark>"
			.IsLogEdited = True
			'*************************
		End With
		'Added By Prashant 28-July-2009
		' '' '''AttachMyFile()
		'-----------------------------
		dgAFPeriods.DataBind()
		dgEnginePeriods.DataBind()
		dgAPUPeriods.DataBind()
		dgCGBPeriods.DataBind() 'Added By Prashant 23-Oct-2009

		Session("mLog") = mLog
	End Sub
	Private Sub NewRecord()
		mLog = Log.NewLog(mMachine, Today.Date)
		mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
		mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
		'''''CHECK_isRequiredAssembliesInstalled()
		mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
		Session("mLog") = mLog
		MarkLog(Util.Action.[New], "Flight Log Edit", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

		' '' ''AJAX- Title line comment as it present in SetTitle function and also Update panel need to called after that.
		SetTitle()
		' '' ''lblTitle.Text = "Status of " & mMachine.RegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"

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

				'End If

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

				Dim IsShowDateCntrl As String
				If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
					IsShowDateCntrl = "False"
					Session("IsSaveAndNew") = 0
				Else
					IsShowDateCntrl = "True"
				End If
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)

				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
						MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
					Else
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					End If

				ElseIf ex.Number = 547 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				'Added by utkash on 1-oct-2013 for log_ajax changes
				mLog = LogClone
				Session("mLog") = mLog
				'end
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	'End
	Public Sub SetAirFrameGridObject(Optional ByVal isFromDataBindGrid As Boolean = False)  ' For First Grid i.e AirFrame
		Dim txtAirFrameHours, txtAirFrameLandings, txtAirFrameCycles, txtAirFrameStarts, txtAirFrameNGCycles, txtAirFrameNFCycles, txtAirFrameRins,
			txtAirFrameBleeds, txtAirFrameImpellerCycles, txtAirFrameCTCycles, txtAirFramePTCycles, txtAirframeGeneratorMods As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgAFPeriods.Items" is replaced by "dgAFPeriods.Rows"
		For i As Integer = 0 To dgAFPeriods.Rows.Count - 1
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
			'-----------------------------
			txtAirframeGeneratorMods = CType(Me.dgAFPeriods.Rows(i).FindControl("txtAirframeGeneratorMods"), TextBox) 'Added By Shweta 07-May-2012

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
			'Added By Shweta 07-May-2012
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
		Dim txtEngineHours, txtEngineLandings, txtEngineCycles, txtEngineStarts, txtEngineNGCycles, txtEngineNFCycles, txtEngineRins, txtEngineCFactors,
			txtEngineBleeds, txtEngineImpellerCycles, txtEngineCTCycles, txtEnginePTCycles, txtEngineGeneratorMods, txtEngineRapidTakeOffFactor As TextBox

		' '' ''AJAX- DataGrid is replaced by GridView for removing "Refresh" buttons. So here "dgEnginePeriods.Items" is replaced by "dgEnginePeriods.Rows"
		For i As Integer = 0 To dgEnginePeriods.Rows.Count - 1
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
			txtEngineGeneratorMods = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012
			'-----------------------------------

			txtEngineRapidTakeOffFactor = CType(Me.dgEnginePeriods.Rows(i).FindControl("txtEngineRapidTakeOffFactor"), TextBox) ' 'Code added for Rapid TakeOff on 31-Oct-2022 by Saylee


			'If mLog.LogEngAssemblies(i).ShowHours Then mLog.LogEngAssemblies(i).Hours = Trim(txtEngineHours.Text)
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
			txtCGBGeneratorMods = CType(Me.dgCGBPeriods.Rows(i).FindControl("txtCGBGeneratorMods"), TextBox) 'Added by Shweta on 7-May-2012  for ALL02052012

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
				If DateDiff(DateInterval.Day, mLog.Date, maxAllowableDate) < -10 _
					Or (IsDate(mLog.SouLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.SouLocalDateTime, maxAllowableDate) < -10) _
					Or (IsDate(mLog.DesLocalDateTime) AndAlso DateDiff(DateInterval.Day, mLog.DesLocalDateTime, maxAllowableDate) < -10) Then
					'If DateDiff(DateInterval.Day, CDate(mLog.Date), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.SouLocalDateTime), maxAllowableDate) < 0 Or DateDiff(DateInterval.Day, CDate(mLog.DesLocalDateTime), maxAllowableDate) < 0 Then

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater by 10 Days or more than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

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
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------

		SetObject()

		'SetAirFrameGridObject()  '''Commented by Saylee on 1-Sep-2021 for ALL01092021 :
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
					' If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
					'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
					Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
						' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

						Return False
						Exit Function
					End If
				End If

				If IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogAfterHrsSame"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.</br> </br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
					Exit Function
				End If

				'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
				If AvgFlightTimeDeviation() = True Then
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
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
				'End If
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
				Dim IsShowDateCntrl As String
				If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
					IsShowDateCntrl = "False"
					Session("IsSaveAndNew") = 0
				Else
					IsShowDateCntrl = "True"
				End If
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)

				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					' MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
						MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
					Else
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					End If
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
				End If
				'Added by utkash on 1-oct-2013 for log_ajax changes
				mLog = LogClone
				Session("mLog") = mLog
				'end
				Return False
			Finally
				LogClone = Nothing
			End Try
		Else
			Return False
		End If
	End Function
	Private Function SaveLogFlexiLog() As Boolean 'Added by Saylee on 21-May-2012 ALL17052012
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
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

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
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
						' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

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
				'-----------------------------------
				If IsMELCount = True Then
					' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "MEL"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, IIf(Session("New") = "True", "MELNew", "MEL"))

					Exit Function
				ElseIf IsEngineHoursSame() = False Or IsCGBHoursSame() = False Or IsZeroValueLog() = True Then  'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
					'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.HoursZero, SIMsgBox.Message_text.HoursZero, "Airframe,Engine,APU... Hours/Landins/Cycles... are Zero. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the readings.", MsgBoxStyle.YesNo)

					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogAfterHrsSame"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					'MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Airframe, Engine Hours are not matching/are zero entered. Still you want to save the record? Click Yes to proceed & click No to cancel current operation & correct the hours.", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "There is some information missing / not entered correctly.<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterHrsSame")

					Exit Function
				ElseIf AvgFlightTimeDeviation() = True Then 'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
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

				'End If

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
				Dim IsShowDateCntrl As String
				If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
					IsShowDateCntrl = "False"
					Session("IsSaveAndNew") = 0
				Else
					IsShowDateCntrl = "True"
				End If
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
						MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
					Else
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					End If
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
				End If
				'Added by utkash on 1-oct-2013 for log_ajax changes
				mLog = LogClone
				Session("mLog") = mLog
				'end
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
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Log. <br> Log/Departure/Arrival Date can not be greater than - <br>" & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")

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
		''SetAirFrameGridObject()
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
					'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
					'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
					If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
					Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
						' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
						' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
						' '' ''msg1.Show()

						' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
						MSGBoxCtrl.Show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")

						Return False
						Exit Function
					End If
				End If
				'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
				If AvgFlightTimeDeviation() = True Then
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "Greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
					Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
					Return False
					Exit Function
				End If
				'End

				mLog.ApplyEdit()
				'Add Pilot and Co-pilot in Log Crew as Child...
				'Pilot In Command
				'If mLog.IsNew Then
				'    If Not mLog.PilotID1.Equals(Guid.Empty) Then
				'        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
				'        mLogCrew.CrewID = mLog.PilotID1
				'        mLogCrew.DutyAsID = 1
				'        mLog.LogCrews.Add(mLogCrew)
				'    End If
				'    'Co-Pilot
				'    If Not mLog.PilotID2.Equals(Guid.Empty) Then
				'        Dim mLogCrew As LogCrew = LogCrew.NewChildLogCrew(mLog.ID)
				'        mLogCrew.CrewID = mLog.PilotID2
				'        mLogCrew.DutyAsID = 2
				'        mLog.LogCrews.Add(mLogCrew)
				'    End If
				'End If
				'End 

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

				'End If

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
				Dim IsShowDateCntrl As String
				If (Session("IsSaveAndNew") Is Nothing OrElse Session("IsSaveAndNew") <> 1) Then
					IsShowDateCntrl = "False"
					Session("IsSaveAndNew") = 0
				Else
					IsShowDateCntrl = "True"
				End If
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AfterSave", "AfterSave('" + IsShowDateCntrl + "');", True)
				'-----------------------------------------------------------------------
				mLog = Log.GetLog(mLog.ID)
				mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
				mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
				mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
				mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
				MarkLog(Util.Action.Save, "Flight Log Edit", mLogDetail, Util.ErrorType.HandledError, mLog.ID, EventLogID)
				'-----------------------------------------------------------------------
				Session("mLog") = mLog
				Return True
			Catch ex As SqlException
				Session("LogClone") = LogClone
				If ex.Number = 8114 Or ex.Number = 8115 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NumericOverFlow, SIMsgBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

				ElseIf ex.Number = 8145 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 2627 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					' MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					If ex.Message.Contains("UKtabLogMachineIDLogPageNo") Then
						MSGBoxCtrl.Show("Alert!", "Save Alert ! ", "<strong> Please enter the unique Log Page No. </strong> ", MsgBoxStyle.OkOnly, "")
					Else
						MSGBoxCtrl.Show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
					End If
				ElseIf ex.Number = 547 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
				ElseIf ex.Number = 50000 Then
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.LogExist, SIMsgBox.Message_text.LogExist, " between Current Date and Time Span for this Aircraft. ", MsgBoxStyle.OkOnly)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.LogExist, MSGBox.Message_text.Alert, "Log already entered between current Date and Time span for this Aircraft.", MsgBoxStyle.OkOnly, "")
				End If
				Return False
			Finally
				'Added by utkash on 1-oct-2013 for log_ajax changes
				mLog = LogClone
				Session("mLog") = mLog
				'end
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
						mLog = Session("mLog")
						DataFieldBind()
						DataBind()

						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If Save() = True Then
								'mLog = Log.GetLog(mLog.ID)
								NewRecord()
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
								Session("mLog") = mLog

								' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
								' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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


							End If
						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
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
								MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
								Exit Sub
							End If
						End If
						'End

						Dim mMaxLogOfAircraft As MaxLogOfAircraft
						mMaxLogOfAircraft = MaxLogOfAircraft.GetMaxLogOfAircraft(mLog.MachineID)

						If Not mMaxLogOfAircraft.LogID.Equals(Guid.Empty) Then
							'Added by Saylee on 18-May-2012 ALL17052012
							' Commented and Added by Utkarsh On 21-Jan-2013 FOR ALL18012013(ClientCode=UHPL)
							'If (AppSettings("ClientCode") <> "Heligo") Then
							If Not (AppSettings("ClientCode") = "Heligo" Or
									AppSettings("ClientCode") = "UHPL" Or
									AppSettings("ClientCode") = "APFT" Or
									AppSettings("ClientCode") = "AAP") Then 'ClientCode APFT added on 25-Jan-2018 For APFT25012018
								'End
								Dim MaxLogDateTime As String = ""

								'  If (AppSettings("LogBookTimeEntry") = "UTC") Then
								If mMachine.IsUTC Then 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									MaxLogDateTime = mMaxLogOfAircraft.SouUniverseDateTimeFormatted
								Else
									MaxLogDateTime = mMaxLogOfAircraft.SouLocalDateTimeFormatted
								End If

								mLog = Session("mLog")
								DataFieldBind()

								If CDate(mLog.SouUniverseDateTime) < CDate(mMaxLogOfAircraft.SouUniverseDateTime) Then 'Added by Saylee on 18-May-2012 ALL17052012
									' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
									' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
									' '' ''Session("sender") = "SaveLogFlexiLog"
									' '' ''Session("SaveNClose") = "SaveNClose"
									' '' ''msg1.Show()

									Session("SaveNClose") = "SaveNClose"

									' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
									MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")

									Exit Sub
								End If
							Else

								mLog = Session("mLog")
								DataFieldBind()

								If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
									' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
									' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
									' '' ''Session("sender") = "SaveLogFlexiLog"
									' '' ''Session("SaveNClose") = "SaveNClose"
									' '' ''msg1.Show()
									Session("SaveNClose") = "SaveNClose"

									' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
									MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, "You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
									Exit Sub
								End If
							End If
						End If

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
							' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
							' '' ''Session("sender") = "MELClose"
							' '' ''msg1.Show()

							' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
							MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue?", "", MsgBoxStyle.YesNo, "MELClose")
							DataBind() 'Added By Utkarsh On 12-Sep-2011
							Exit Sub
						Else

							mLog = Session("mLog")
							DataFieldBind()

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
							Else
								' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
								upnlErrorList.Update()
							End If
						End If
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours

						mLog = Session("mLog")
						Session("IsValueZero") = "True"
						DataFieldBind()

						''DataBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If SaveLogAfterHrsSame() = True Then
								If Session("New") = "True" Then
									Session("New") = ""

									NewRecord()
									Session.Remove("mFileAttach")
									Session.Remove("IsAttachmentDeleted")
									Session("mLog") = mLog

									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

									DataFieldBind()

									EnableDisableButton()
									ControlVisibility()
									ControlVisibilityForAttachment()
									DataBindGrid()

									SetTitle()
									mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
									Session("mLogListOnDate") = mLogListOnDate
									If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
										'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
										ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
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

								Else
									mLog = Log.GetLog(mLog.ID)
									mLog.IsUTC = mMachine.IsUTC '(AppSettings("LogBookTimeEntry") = "UTC") 'Changed By Saylee On 12-Feb-2014 For ALL12022014-1
									mLog.IsTLP = mMachine.IsTLP 'Added by Saylee On 14-Jun-2018 For ALL14062018, from AppSettings to Machine TLP
									mLog.IsLogAirborneEntry = mMachine.IsLogAirborneEntry
									Session("mLog") = mLog

									SetTitle()
									DataFieldBind()
									EnableDisableButton()


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

									If Session("SaveNClose") = "SaveNClose" Then
										Session("SaveNClose") = ""
										Session.Remove("SaveNClose")
										Session.Remove("mFileAttach")
										Session.Remove("IsAttachmentDeleted")
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									End If
								End If

							End If
						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							upnlErrorList.Update()

						End If
					ElseIf MSGBoxCtrl.Sender = "MELClose" Then
						mLog = Session("mLog")
						DataFieldBind()
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
						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							upnlErrorList.Update()
						End If
					ElseIf MSGBoxCtrl.Sender = "MEL" Then
						mLog = Session("mLog")
						DataFieldBind()
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
									' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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

								End If
							End If
						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							upnlErrorList.Update()
						End If

					ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then 'Added by Saylee on 21-May-2012 ALL17052012 to save Flexi log

						mLog = Session("mLog")
						DataFieldBind()
						''DataBind()

						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							If SaveLogFlexiLog() = True Then
								If Session("New") = "True" Then
									Session("New") = ""
									NewRecord()
									Session.Remove("mFileAttach")
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

									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
										Session.Remove("mFileAttach")
										Session.Remove("IsAttachmentDeleted")
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									End If
								End If

							End If

						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							upnlErrorList.Update()
						End If
					ElseIf MSGBoxCtrl.Sender = "MELNew" Then
						mLog = Session("mLog")
						DataFieldBind()
						DataBind()

						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							Session("New") = "True"
							If Save() = True Then
								'mLog = Log.GetLog(mLog.ID)
								NewRecord()
								Session.Remove("mFileAttach")
								Session.Remove("IsAttachmentDeleted")
								Session("mLog") = mLog

								' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
								' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
								DataFieldBind()

								EnableDisableButton()
								ControlVisibility()
								ControlVisibilityForAttachment()
								DataBindGrid()

								SetTitle()
								mLogListOnDate = LogList.GetLogList(mMachine.ID, calDateTime.Text.ToString, calDateTime.Text.ToString)
								Session("mLogListOnDate") = mLogListOnDate
								If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
									'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
									ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
									upnlLogInfo.Update()
								End If
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
								upnlLogDetails.Update()
								upnlFlightDetails.Update()
								upnlFlightSummary.Update()
								upnlTabs.Update()

							End If
						Else
							' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
							upnlErrorList.Update()
						End If
						'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterAvgFlightTimeDeviationWarning" Then
						mLog = Session("mLog")
						DataFieldBind()
						If mLog.IsValid Then
							If Not CustomValidate2() Then upnlErrorList.Update() : Exit Sub
							If SaveLogAfterAvgFlightTimeDeviationWarning() = True Then
								If Session("New") = "True" Then
									Session("New") = ""
									NewRecord()
									Session.Remove("mFileAttach")
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

									' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
									' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
										Session.Remove("mFileAttach")
										Session.Remove("IsAttachmentDeleted")
										Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
									End If
								End If
							Else
								' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
								upnlErrorList.Update()
							End If
						End If
					End If
					'End
				Case MsgBoxResult.No
					'Code Added By Deven for Save and New 20/03/2008
					If Session("New") = "True" Then Session("New") = ""
					If MSGBoxCtrl.Sender = "SaveNew" Then
						NewRecord()
						DataFieldBind()
						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "SaveLogFlexiLog" Then  'Added by Saylee on 21-May-2012 ALL17052012 to save Flexi log
						Session.Remove("IsValueZero") 'Shweta

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "SaveLogAfterHrsSame" Then 'Added by Saylee on 8-Oct-2009 to check Assembly hours and Airframe hours
						Session.Remove("IsValueZero")

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "Close" Then
						Session("SaveNClose") = ""
						Session.Remove("SaveNClose")
						'NewReccord()
						Session.Remove("mFileAttach")
						Session.Remove("IsAttachmentDeleted")
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "MELClose" Then
						'NewRecord()
						Session.Remove("mFileAttach")
						Session.Remove("IsAttachmentDeleted")
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					ElseIf MSGBoxCtrl.Sender = "MEL" Or MSGBoxCtrl.Sender = "MELNew" Then

						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If

				Case MsgBoxResult.Cancel

					'Code Added By Deven for Save and New 20/03/2008
					If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
						' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
						' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
					End If

				Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
					'Added By Vikrant on 01-Dec-2021 for PBH
					If MSGBoxCtrl.Sender = "AircraftMadeNotInUse" Then
						Response.Redirect("Index.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
						Exit Sub
					End If
					'End
					DataFieldBind()
					' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
					' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))

				Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
					DataFieldBind()

					' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
					' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
			End Select

		ElseIf Result1 = 0 Then   'Code Added
			If Session("New") = "True" Then Session("New") = ""
			'DataFieldBind()

		ElseIf Result1 = -1 Then
			If Session("New") = "True" Then Session("New") = ""

			' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
			' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
		End If
	End Sub

	'''''Private Sub CHECK_isRequiredAssembliesInstalled()
	'''''    If Not CheckZeroDifferenceValue() Then 'Added By Utkarsh ON 12-Aug-2013 FOR ALL12082013   
	'''''        'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
	'''''        'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
	'''''        If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved _
	'''''            Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved _
	'''''            Or mLog.LogGEAssemblies.AssemblyRemoved Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved _
	'''''            Or mLog.LogSSAAssemblies.AssemblyRemoved Then

	'''''            ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
	'''''            ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
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

	'''''        ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.EntryRestriction, SIMsgBox.Message_text.EntryRestriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log. ", MsgBoxStyle.OkOnly)
	'''''        ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
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
		'Commented by Saylee on 20-Aug-2018 as per requirement to allow entering alphanumeric 
		'If Not AppSettings("ClientCode") = "GlobalJet" Then
		'    txtLogPageNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")
		'End If
		'*********************************************
		upnlLogDetails.Update()
	End Sub
	Private Sub NewRecord(ByVal LogDate As String, Optional ByVal mSouLocalDateTime As String = "", Optional ByVal mSouUTCDateTime As String = "")
		mLog = Log.NewLog(mMachine, LogDate, mSouLocalDateTime, mSouUTCDateTime)
		' mLog.BeginEdit()
		mMachine = Machine.GetMachine(mMachine.ID)
		DataBind()
		'''''CHECK_isRequiredAssembliesInstalled()
	End Sub
	Private Sub EditRecord(ByVal LogDate As DateTime)
		mLog = Log.GetLog(mLog.ID)
		' mLog.BeginEdit()
		mLog.Date = LogDate
		DataBind()
		''''CHECK_isRequiredAssembliesInstalled()
	End Sub
	Private Sub CopyFromClone(ByVal ClonedLog As Log, Optional ByVal isFromLogDate As Boolean = False)
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
		'Commeneted By Vikrant On 20-Oct-2015
		'If isFromLogDate Then
		'    mLog.DesLocalDateTime = ClonedLog.DesLocalDateTime
		'    mLog.DesDayLightTime = ClonedLog.DesDayLightTime
		'    If takeofftouchdown Then
		'        mLog.TouchDownLocalDateTime = mLog.DesLocalDateTime
		'    End If
		'End If
		'End
		'Added By Utkarsh On 05-Sep-2011
		'Commeneted by vikrant on 28-Oct-2015
		'If Not takeofftouchdown Then
		'    mLog.TimeOnGround = ClonedLog.TimeOnGround
		'    mLog.PercentTimeOnGround = ClonedLog.PercentTimeOnGround
		'    mLog.TimeInAir = ClonedLog.TimeInAir
		'End If
		'End

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
	End Sub

	Private Sub SetFromAutoComplete()
		'If Not SetValue Then
		'    Session("Pilot1ID") = mSearchListPilot.Item(Pilot1.Text).GId
		'    Session("Pilot2ID") = mSearchListPilot.Item(Pilot2.Text).GId
		'    Session("SourceID") = mSearchListPlace.Item(Place1.Text).GId
		'    Session("DestinationID") = mSearchListPlace.Item(Place2.Text).GId
		'    Session("SetValue") = True

		'Added by Utkarsh On 24-Nov-2011 For ALL23112011

		Dim tempString As String
		Dim tempString1 As String
		Dim Place1Code As String
		Dim Place2Code As String

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
		'End If

		'If SetValue Then

		'    If (AppSettings("ClientCode") = "Heligo") Then
		'        mLog.PilotID1 = New Guid("{EC934C03-36DB-42B4-8F04-D744BE7E6451}")
		'    Else
		'        mLog.PilotID1 = Pilot1ID
		'    End If
		'    mLog.PilotID2 = Pilot2ID
		'    mLog.SourceID = SourceID
		'    mLog.DestinationID = DestinationID
		'End If

	End Sub

	Private Sub SetTakeoffTouchdownTitle()

		If TakeOffTouchDown Then

			'Commented and Changed by Utkarsh On 22-Mar-2012

			'lblDepDateTime.Text = "ChokesOn Date/Time"
			'lblUTCDateTime.Text = "UTC ChokesOn Date/Time"
			'lblArrDate.Text = "ChokesOff Date/Time"
			'lblUTCArrivalDateTime.Text = "UTC ChokesOff Date/Time"

			'btnDepartureDate.ToolTip = "Refresh Chokes On date."
			'btnUTCDepartureDate.ToolTip = "Refresh Chokes On date."
			'btnArrivaldate.ToolTip = "Refresh Chokes Off date."
			'btnUTCArrivaldate.ToolTip = "Refresh Chokes Off date."

			lblDepDateTime.Text = "ChocksOff Date/Time"
			lblUTCDateTime.Text = "UTC ChocksOff Date/Time"
			lblArrDate.Text = "ChocksOn Date/Time"
			lblUTCArrivalDateTime.Text = "UTC ChocksOn Date/Time"

			' '' ''btnDepartureDate.ToolTip = "Refresh Chocks Off date."
			' '' ''btnUTCDepartureDate.ToolTip = "Refresh Chocks Off date."
			' '' ''btnArrivaldate.ToolTip = "Refresh Chocks On date."
			' '' ''btnUTCArrivaldate.ToolTip = "Refresh Chocks On date."

			'End

			upnlFlightDetails.Update()

		End If

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
	Private Sub GetAttachment()
		If mLog.IsAttachmentAdded And mFileAttach Is Nothing Then
			mFileAttach = FileAttach.GetAttachment(mLog.ID)
			Session("mFileAttach") = mFileAttach
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
	'Added By Vikrant on 01-Dec-2021 for PBH
	''Private Sub SetPBHValues(ByVal TmpLog As Log, ByVal IsLogNew As Boolean)
	''    Try
	''        If mCompanyDetail.IsCombinedHours = False Then 'PBH Collective Hrs by Saylee on 30-Nov-2022


	''            Dim mPBH As PBH = PBH.GetPBHByMachine(TmpLog.MachineID, "")
	''            If Not mPBH.MachineID.Equals(Guid.Empty) Then
	''                If CDate(mLog.Date) >= CDate(mPBH.StartDate) Then
	''                    mPBH.CurrentHours = TmpLog.LogAFAssemblies(0).FinalHours_Dec
	''                    If IsLogNew Then
	''                        mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec), 1, False, False).Value
	''                    Else
	''                        mPBH.ElapsedHours = New Period(1, (New Period(1, TmpLog.LogAFAssemblies(0).FinalHours_Dec, 1, False, False).DbValueDec - mPBH.StartHoursDec + New Period(1, TmpLog.LogAFAssemblies(0).Hours_Dec, 1, False, False).DbValueDec), 1, False, False).Value
	''                    End If

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
	Private Sub SaveAttachment() '
		If mFileAttach IsNot Nothing Then
			If mFileAttach.Size > 0 Then
				Try
					mFileAttach.Save()
				Catch ex As Exception
					ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
				End Try
			Else
				If (Not mLog.IsNew) And IsAttachmentDeleted Then
					FileAttach.DeleteAttachment(mFileAttach.ID, mLog.ID)
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
		If mFileAttach.Size > 0 Then
			Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
			Dim fs As FileStream
			If File.Exists(AppSettings("DOCPath")) = False Then
				'Delete File if exist
				System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
				' Create the file.
				fs = File.Create(path)
				'' Add some information to the file.
				fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
				fs.Close()
				Session("DOCPath") = path
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
			End If
		End If
	End Sub
	Private Function IsValidTime(ByVal TimeValue As String) As Boolean
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
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBLandings"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBStarts"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNGCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBNFCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBRins"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBBleeds"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBImpellerCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBCTCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBPTCycles"), TextBox).ReadOnly = True
				CType(Me.dgCGBPeriods.Rows(l).FindControl("txtCGBGeneratorMods"), TextBox).ReadOnly = True
			Next l
		ElseIf (AppSettings("ClientCode") = "IND") Then
			dgEnginePeriods.Columns(11).HeaderText = "PTCNTC"
			dgEnginePeriods.Columns(12).HeaderText = "Final PTCNTC"

			dgEnginePeriods.Columns(13).HeaderText = "CTCNTC"
			dgEnginePeriods.Columns(14).HeaderText = "Final CTCNTC"

			dgEnginePeriods.Columns(21).HeaderText = "IMCNTC"
			dgEnginePeriods.Columns(22).HeaderText = "Final IMCNTC"

			dgEnginePeriods.Columns(23).HeaderText = "C1"
			dgEnginePeriods.Columns(24).HeaderText = "Final C1"

			dgEnginePeriods.Columns(25).HeaderText = "C2"
			dgEnginePeriods.Columns(26).HeaderText = "Final C2"

			'Added by Saylee 0n 23-Sep-2020 for IND23092020
			dgAPUPeriods.Columns(17).HeaderText = "APU Hours"
			dgAPUPeriods.Columns(18).HeaderText = "Final APU Hours"
			'******************
		ElseIf (AppSettings("ClientCode") = "FBW") Then ''Added by Saylee on 15-Dec-2021
			dgAFPeriods.Columns(17).HeaderText = "AHH"
			dgAFPeriods.Columns(18).HeaderText = "Final AHH"

			dgEnginePeriods.Columns(19).HeaderText = "AHH"
			dgEnginePeriods.Columns(20).HeaderText = "Final AHH"

			dgAPUPeriods.Columns(17).HeaderText = "AHH"
			dgAPUPeriods.Columns(18).HeaderText = "Final AHH"

			dgCGBPeriods.Columns(7).HeaderText = "AHH"
			dgCGBPeriods.Columns(8).HeaderText = "Final AHH"
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
		'''''''''''''''''''calDateTime.Value = mLog.Date
		If mLog.Date IsNot System.DBNull.Value Then
			calDateTime.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))
		Else
			calDateTime.Text = ""
		End If

		'''''''''calDeparture.Value = mLog.SouLocalDateTime  DateFormat
		If mLog.SouLocalDateTime IsNot System.DBNull.Value Then
			calDeparture.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
			txtDepartureTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
		Else
			calDeparture.Text = ""
			'calDepartureTime.Text = ""
		End If

		'calArrival.Text = mLog.DesLocalDateTime
		If mLog.DesLocalDateTime IsNot System.DBNull.Value Then
			calArrival.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("DateFormat"))
			txtArrivalTime.Text = Format(CDate(mLog.DesLocalDateTime), AppSettings("TimeFormat"))
		Else
			If mLog.SouLocalDateTime IsNot System.DBNull.Value Then
				calArrival.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("DateFormat"))
				txtArrivalTime.Text = Format(CDate(mLog.SouLocalDateTime), AppSettings("TimeFormat"))
			Else
				calArrival.Text = ""
			End If
		End If
		''''''''''''''''CalUTCDateTime.Value = mLog.SouUniverseDateTime
		If mLog.SouUniverseDateTime IsNot System.DBNull.Value Then
			CalUTCDateTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
			txtUTCDepartureTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
		Else
			CalUTCDateTime.Text = ""
		End If

		''''''''''''''''''''CalUTCArrival.Value = mLog.DesUniverseDateTime
		If mLog.DesUniverseDateTime IsNot System.DBNull.Value Then
			CalUTCArrival.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("DateFormat"))
			txtUTCArrivalTime.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("TimeFormat"))
		Else
			If mLog.SouUniverseDateTime IsNot System.DBNull.Value Then
				CalUTCArrival.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("DateFormat"))
				txtUTCArrivalTime.Text = Format(CDate(mLog.SouUniverseDateTime), AppSettings("TimeFormat"))
			Else
				CalUTCArrival.Text = "" 'Change by Vikrant on 20-Oct-2015 for Religare
			End If
		End If

		'Added By Utkarsh On 30-Aug-2011

		If TakeOffTouchDown Then
			If mLog.TakeOffLocalDateTime IsNot System.DBNull.Value Then
				calTakeOffLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
				txtTakeOffLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
			Else
				calTakeOffLocalDateTime.Text = ""
			End If


			If mLog.TakeOffUniverseDateTime IsNot System.DBNull.Value Then
				calUTCTakeOffDateTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("DateFormat"))
				txtUTCTakeOffTime.Text = Format(CDate(mLog.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
			Else
				calUTCTakeOffDateTime.Text = ""
			End If

			If mLog.TouchDownLocalDateTime IsNot System.DBNull.Value Then
				calTouchDownLocalDateTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("DateFormat"))
				txtTouchDownLocalTime.Text = Format(CDate(mLog.TouchDownLocalDateTime), AppSettings("TimeFormat"))
			Else
				If mLog.TakeOffLocalDateTime IsNot System.DBNull.Value Then
					calTouchDownLocalDateTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("DateFormat"))
					txtTouchDownLocalTime.Text = Format(CDate(mLog.TakeOffLocalDateTime), AppSettings("TimeFormat"))
				Else
					calTouchDownLocalDateTime.Text = ""
				End If
			End If

			If mLog.TouchDownUniverseDateTime IsNot System.DBNull.Value Then
				calUTCTouchDownDateTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("DateFormat"))
				txtUTCTouchDownTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("TimeFormat"))
			Else
				If mLog.TakeOffUniverseDateTime IsNot System.DBNull.Value Then
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
		mFlightLogClassificationList = FlightLogClassificationList.GetFlightLogClassificationList("", "<SELECT>")
		cmbFlightLogClassification.DataSource = mFlightLogClassificationList
		Session("mFlightLogClassificationList") = mFlightLogClassificationList

		'Code Added by DEVEN On 29/12/2007 --------------------------------------
		DataBind()
		GridColumnHeadingSet()

		If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
			cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
		Else
			cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
		End If
		'------------------------------------------------------------------------

		'Added By Utkarsh On 23-Aug-2011
		mSearchListPilot = SearchList.GetSearchList("Pilot", "", "")
		Session("mSearchListPilot") = mSearchListPilot
		mSearchListPlace = SearchList.GetSearchList("Place", "", "")
		Session("mSearchListPlace") = mSearchListPlace
		'end

		Pilot1.Text = mLog.Pilot1Name
		Pilot2.Text = mLog.Pilot2Name

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
		cmbFlightLogClassification.DataBind()

		Session("mFlightLogClassificationList") = mFlightLogClassificationList

		If cmbFlightLogClassification.Items.Contains(New System.Web.UI.WebControls.ListItem(mLog.FlightLogClassificationName, mLog.FlightLogClassificationID.ToString)) Then
			cmbFlightLogClassification.SelectedValue = mLog.FlightLogClassificationID.ToString
		Else
			cmbFlightLogClassification.SelectedValue = Guid.Empty.ToString
		End If

		upnlLogDetails.Update()
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
			dgAFPeriods.DataBind()

			dgEnginePeriods.DataSource = mLog.LogEngAssemblies
			dgEnginePeriods.DataBind()

			dgAPUPeriods.DataSource = mLog.LogAPUAssemblies
			dgAPUPeriods.DataBind()

			dgCGBPeriods.DataSource = mLog.LogCGBAssemblies
			dgCGBPeriods.DataBind()

			grdAllAssemblies.DataSource = mLog.ALL_LogAssemblies
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
		Dim tempString As String 'Added By Utkarsh On 24-Nov-2011 For ALL23112011

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
			''''''''''''''If Not (calDeparture.IsDateValue) Then
			If Not IsDate(calDeparture.Text) Then
				custValidator.ErrorMessage = "Departure date should be in valid date time format."
				e.IsValid = False
			Else
				Dim Date1, Time1 As String
				'''''''''Date1 = calDeparture.Value.ToString
				'''''''''Time1 = calDeparture.Value.ToString
				Date1 = calDeparture.Text.ToString
				Time1 = calDeparture.Text.ToString
				If Date1 = "1/1/0001" Then
					custValidator.ErrorMessage = "Departure date should be in valid date time format."
					e.IsValid = False

					Exit Sub
				End If
				'CNDC
				'calDeparture.Text = Date1 + " " + Time1
				''''''''''calDeparture.Value = Date1
				calDeparture.Text = Date1
				e.IsValid = True
			End If
		ElseIf custValidator.ControlToValidate = "calArrival" Then
			'CNDC
			'If Not IsDate(calArrival.Text) Then
			If Not IsDate(calArrival.Text) Then
				custValidator.ErrorMessage = "Arrival date should be in valid date time format."
				e.IsValid = False
			Else
				Dim Date1, Time1 As String
				'CNDC
				Date1 = calArrival.Text.ToString
				Time1 = calArrival.Text.ToString
				'Date1 = CDate(calArrival.Text).ToShortDateString()
				'Time1 = CDate(calArrival.Text).ToShortTimeString()
				If Date1 = "1/1/0001" Then
					custValidator.ErrorMessage = "Arrival date should be in valid date time format."
					e.IsValid = False

					Exit Sub
				End If
				'CNDC
				calArrival.Text = Date1
				'calArrival.Text = Date1 + " " + Time1
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "Pilot1" Then

			If Not mSearchListPilot.Contains(Pilot1.Text.Trim) Then
				custValidator.ErrorMessage = "Enter correct Pilot1 name."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "Place1" Then
			'Added by Utkarsh On 24-Nov-2011 For ALL23112011
			tempString = Place1.Text.Trim
			If Not tempString = String.Empty Then
				'tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
				If tempString.IndexOf("[") < 0 Then
					custValidator.ErrorMessage = "Enter correct Source name."
					e.IsValid = False
				Else
					tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
					If Not mSearchListPlace.Contains(tempString) Then
						custValidator.ErrorMessage = "Enter correct Source name."
						e.IsValid = False
					Else
						e.IsValid = True
					End If
				End If
			End If
			'End
		ElseIf custValidator.ControlToValidate = "Pilot2" Then
			If Not mSearchListPilot.Contains(Pilot2.Text.Trim) Then
				custValidator.ErrorMessage = "Enter correct Pilot2 name."
				e.IsValid = False
			Else
				e.IsValid = True
			End If

		ElseIf custValidator.ControlToValidate = "Place2" Then
			'Added by Utkarsh On 24-Nov-2011 For ALL23112011
			tempString = Place2.Text.Trim
			If Not tempString = String.Empty Then
				'  tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
				If tempString.IndexOf("[") < 0 Then
					custValidator.ErrorMessage = "Enter correct Destination name."
					e.IsValid = False
				Else
					tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
					If Not mSearchListPlace.Contains(tempString) Then
						custValidator.ErrorMessage = "Enter correct Destination name."
						e.IsValid = False
					Else
						e.IsValid = True
					End If
				End If
			End If
			'End
		End If

	End Sub
	Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs) ' Validation From AIRFRAMEGRID (Grid-1)
		If Flag = 1 Then Exit Sub
		Dim custValidator As CustomValidator
		custValidator = CType(s, CustomValidator)
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		'-------------------------------
		upnlFlightSummary.DataBind()
		upnlFlightSummary.Update()

		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)  'True added by Saylee 25-July-2012
		SetAPUGridObject(True)     'True added by Saylee 25-July-2012
		SetCGBGridObject(True)     'True added by Saylee 25-July-2012
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
	End Function
#End Region

#Region " Events "
	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		''''''''''''calDeparture.ShowTime = True
		'''''''''''' calArrival.ShowTime = True
		''''''''''''CalUTCDateTime.ShowTime = True
		''''''''''''CalUTCArrival.ShowTime = True
		GetSession()

		TakeOffTouchDown = CType(AppSettings("TakeOffTouchDown"), Boolean) 'Added By Utkarsh On 31-Aug-2011
		mLog.IsTakeoffTouchDown = TakeOffTouchDown  'Added By Utkarsh On 02-Sep-2011
		EventLogID = CType(Session("EventLogID"), Guid)  'Added by Prashant on 20-July-2011
		addAttributes()

		If Not IsPostBack Then
			If calDateTime.Enabled = True Then
				setFocus(calDateTime)
			End If
			' SetFromSearch()  'Commented By Utkarsh On 24-Aug-2011
			DataFieldBind()

			'Added By Prashant 28-July-2009
			'Attach File
			' '' ''AttachMyFile()

			If mLogListOnDate.Count > 0 And mLog.IsNew And AppSettings("ShowLogDetailsOnAddingSameLogDate") = "True" Then 'Added by Saylee on 2-Sep-2016 for ALL02092016 as per config key ShowLogDetailsOnAddingSameLogDate
				'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
				ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
				upnlLogInfo.Update()
			End If
			upnlLogDetails.Update()

			ControlVisibilityForAttachment()
		End If
		''  GridColumnHeadingSet()

		EnableDisableButton()
		ControlVisibility()

		' '' ''AJAX- "MessageBoxResult()" is commented here and called from new User Control Delegate event present at the bottom "MsgBoxCtrl_UserControlButtonClicked"
		' '' ''MessageBoxResult()

		DataBindGrid()

		SetTitle()
		SetTakeoffTouchdownTitle()  'Added By Utkarsh On 31-Aug-2011
		SetFromAutoComplete() 'Added By Utkarsh On 24-Aug-2011
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
			' '' ''msg.ReplacePage = "wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
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
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")

					Exit Sub
				End If
			Else
				If CDate(mLog.Date) < CDate(mMaxLogOfAircraft.LogDate) Then 'Added by Saylee on 18-May-2012 ALL17052012
					' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo)
					' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
					' '' ''Session("sender") = "SaveLogFlexiLog"
					' '' ''msg1.Show()

					' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
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
		'-----------------------------------
		If IsMELCount = True Then
			' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
			' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "MEL"
			' '' ''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MEL")

			If IsValid Then
				'BindClassification()
				SetObject()
				SetAirFrameGridObject()
				SetEngineGridObject(True)
				SetAPUGridObject(True)
				SetCGBGridObject(True)
			Else
				' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
				upnlErrorList.Update()
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

				' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
				' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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

			End If
		Else
			' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
			upnlErrorList.Update()
		End If

	End Sub
	Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
		'MarkLog(Util.Action.Close, "Log", "", Util.ErrorType.NoError, Guid.Empty)
		Session("IsValid") = IsValid
		If mLog.IsDirty And mLog.IsLogEdited = False Then

			'''''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
			'''''msg1.ReplacePage = "wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") '"wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
			'''''Session("sender") = "Close"
			'''''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")

			If IsValid Then
				'Code Added By Deven 21-03-2008 
				'BindClassification()
				SetObject()
				SetAirFrameGridObject()
				SetEngineGridObject(True)
				SetAPUGridObject(True)
				SetCGBGridObject(True)
				'-------------------------------
			Else
				' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
				upnlErrorList.Update()
			End If
		Else
			'mLogDetail = mLog.LogTextNo + " Dated : " + mLog.DateFormatted
			MarkLog(Util.Action.Close, "Flight Log Edit", "", Util.ErrorType.HandledError, mLog.ID, EventLogID)

			RemoveSession()
			Response.Redirect(Request.QueryString("BackPage") & "?")
		End If
	End Sub
	Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
		If (Not User.IsInRole("LogPrint")) Then
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			' '' ''msg.ReplacePage = "wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''msg.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
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
			' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
			' '' ''msg.ReplacePage = "wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "Authorization"
			' '' ''msg.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")

			Exit Sub
		End If

		If Not IsValid Then upnlErrorList.Update() : Exit Sub ' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.

		Session("IsSaveAndNew") = 1

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
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, Message, MsgBoxStyle.OkOnly, "")
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
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & MaxLogDateTime & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")

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
					MSGBoxCtrl.Show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Alert, " You are about to enter a Back dated Flight Log. The last Log entered for this Aircraft is dated " & mMaxLogOfAircraft.LogDateFormatted & "<BR> <BR>Do you want to continue?", MsgBoxStyle.YesNo, "SaveLogFlexiLog")
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
		'-----------------------------------

		If IsMELCount = True Then
			' '' ''Dim msg1 As New SIMsgBox(Page, "Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
			' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
			' '' ''Session("sender") = "MELNew"
			' '' ''msg1.Show()

			' '' ''AJAX- Old SIMsgBox is replaced by new User Control Modal PopUp.
			MSGBoxCtrl.Show("Minimum Equipment Level", "Installed Components does not fulfill Minimum Equipment Level to Fly <BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo, "MELNew")

			If IsValid Then
				'BindClassification()
				SetObject()
				SetAirFrameGridObject()
				SetEngineGridObject(True)
				SetAPUGridObject(True)
				SetCGBGridObject(True)
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
				Session.Remove("mFileAttach")
				Session.Remove("IsAttachmentDeleted")
				DataFieldBind()
				Session("mLog") = mLog
				'Added by Saylee on 14-July-2009
				Session("mAircraftInformationBoardList") = Nothing
				'*********************************

				' '' ''AJAX- Avoid Self Refresh or rendering of complete page. Instead call "DataFieldbind" and other functions is required.
				' '' ''Response.Redirect("wfLogSOP.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
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
					'  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FuncLastLogDet", "FuncLastLogDet('" + mLog.MachineID.ToString + "', '" + calDateTime.Text.ToString + "');", True)
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
					upnlLogInfo.Update()
				End If
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
				upnlLogDetails.Update()
				upnlFlightDetails.Update()
				upnlFlightSummary.Update()

				upnlAirframeDetail.Update()
				upnlEngineDetail.Update()
				upnlAPUDetail.Update()
				upnlCGBDetail.Update()

				upnlTabs.Update()
			End If
		Else
			' '' ''AJAX- Update ErrorList UpdatePanel wherever Save, IsValid or CustomValidate has checked.
			upnlErrorList.Update()
		End If
		'************************************************************

	End Sub

	Private Sub imgbtnPilot1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnPilot1.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx&Type=Pilot")
	End Sub
	Private Sub imgbtnPilot2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnPilot2.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx&Type=Pilot&AddType=1")
	End Sub
	Protected Sub btnAddPilots_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPilots.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Dim mEmployee As Employee
		mEmployee = Employee.NewPilot()
		Session("mEmployee") = mEmployee

		Response.Redirect("wfPilot.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	End Sub

	Private Sub imgbtnArrPlace_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnArrPlace.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx&Type=Place&AddType=2")
	End Sub
	Private Sub btnAddArrPlace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddArrPlace.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	End Sub

	Private Sub imgbtnDepPlace_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDepPlace.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfSearch.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx&Type=Place&AddType=3")
	End Sub
	Private Sub btnAddDepPlace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDepPlace.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	End Sub

	Protected Sub btnAddPlace_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAddPlace.Click
		'Code Added By Deven 21-03-2008 
		'BindClassification()
		SetObject()
		SetAirFrameGridObject()
		SetEngineGridObject(True)
		SetAPUGridObject(True)
		SetCGBGridObject(True)
		'-------------------------------
		Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	End Sub

	Protected Sub btnFlightLogClassifications_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFlightLogClassifications.Click
		'SetObject()  Comeented By Saylee for bug-FLD10 (Maintenance) by Pramod
		'SetSession()
		Response.Redirect("wfFlightLogClassification.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	End Sub
	Private Sub cmbFlightLogClassification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFlightLogClassification.SelectedIndexChanged
		mLog.FlightLogClassificationID = New Guid(cmbFlightLogClassification.SelectedValue.ToString)
		mLog.FlightLogClassificationName = cmbFlightLogClassification.SelectedItem.Text
		Session("mLog") = mLog
	End Sub
	Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
		ViewImage()
	End Sub
	Private Sub calDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDateTime.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007
			'  calDateTime.Text = Format(CDate(calDateTime.Text), AppSettings("DateFormat"))
			'If Trim(calDateTime.Text) = "" Then
			'    cvRemark.ErrorMessage = "Log date required."
			'    cvRemark.IsValid = False
			'    Exit Sub
			'End If

			'# Date Control Validation #
			Try

				Dim tempdate As DateTime
				'Dim Datestring As String = calDateTime.Text.Trim
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
				calDateTime_TextChanged(calDateTime.Text, e)  'Raising textchange event for further calculation
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
					'CNDC
					'NewRecord(calDateTime.Text)


					'Commented on 30-Dec-2019
					'NewRecord(calDateTime.Text.ToString)

					Dim dtString As DateTime = CType(calDateTime.Text.ToString.Trim + " " + "23:59", DateTime)
					NewRecord(calDateTime.Text.ToString, , dtString.ToString)
					'---------
					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(calDateTime.Text))
					EditRecord(calDateTime.Text.ToString)
				End If
				REM: Copy from Clone
				CopyFromClone(clnLog, True) 'Changed By Utkarsh On 13-Sep-2011
				'Added By Vikrant on 20-Oct-2015 to change date after LogDate Change
				'If Not mLog.DestinationID.Equals(Guid.Empty) Then
				'mLog.DesLocalDateTime = mLog.SouLocalDateTime
				'mLog.DesUniverseDateTime = mLog.SouUniverseDateTime
				'mLog.TouchDownLocalDateTime = mLog.TakeOffLocalDateTime
				'mLog.TouchDownUniverseDateTime = mLog.TakeOffUniverseDateTime
				'End If
				'End
				DataFieldBind()
				'DataBind() 'Hobbs - taken
				'If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Then
				'changed By Utkarsh On 15-Mar-2013 FOR ALL14032013-1 FOR MRH,SPS,SSA assemblies
				'''''If mLog.LogAFAssemblies.AssemblyRemoved Or mLog.LogEngAssemblies.AssemblyRemoved Or mLog.PropLogAssemblies.AssemblyRemoved Or mLog.LogAPUAssemblies.AssemblyRemoved Or mLog.LogCGBAssemblies.AssemblyRemoved Or mLog.LogNGBAssemblies.AssemblyRemoved Or mLog.LogGEAssemblies.AssemblyRemoved _
				'''''Or mLog.LogMRHAssemblies.AssemblyRemoved Or mLog.LogSPSAssemblies.AssemblyRemoved Or mLog.LogSSAAssemblies.AssemblyRemoved Then
				'''''    ' '' ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Restriction, SIMsgBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly)
				'''''    ' '' ''msg1.ReplacePage = "wfLogSOP.aspx?BackPage=" & Request.QueryString("BackPage")
				'''''    ' '' ''msg1.Show()

				'''''    MSGBoxCtrl.show(MSGBox.Message_title.Restriction, MSGBox.Message_text.Restriction, "Required Assembly of the Aircraft is Not Installed on this Date of Log.", MsgBoxStyle.OkOnly, "")
				'''''    Exit Sub
				'''''End If
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
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), Guid.NewGuid.ToString, str1, True)
					ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowLastDet", "ShowLastDet();", True)
					upnlLogInfo.Update()
				End If
				upnlLogDetails.Update()
			End If
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
			''Custom Validation for CalArrival Date
			If Trim(calArrival.Text) = "" Then
				ViewState("calArrival") = calDateTime.Text.Trim
				Exit Sub
			End If
			'If Not IsDate(Trim(calArrival.Text)) Then
			'    cvRemark.ErrorMessage = "Arrival date should be in valid date time format."
			'    cvRemark.IsValid = False
			'    Exit Sub
			'Else
			'    Dim Date1, Time1 As String
			'    '''''''''Date1 = calDeparture.Value.ToString
			'    '''''''''Time1 = calDeparture.Value.ToString
			'    Date1 = calArrival.Text.ToString
			'    Time1 = calArrival.Text.ToString
			'    If Date1 = "1/1/0001" Then
			'        cvRemark.ErrorMessage = "Arrival date should be in valid date time format."
			'        cvRemark.IsValid = False
			'        Exit Sub
			'    End If
			'    'CNDC
			'    'calDeparture.Text = Date1 + " " + Time1
			'    ''''''''''calDeparture.Value = Date1
			'    'calDeparture.Text = Date1
			'    cvRemark.IsValid = True
			'End If

			'# Date Control Validation #

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = calArrival.Text.Trim
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
				calArrival_TextChanged(calArrival.Text, e)  'Raising textchange event for further calculation
				Exit Sub
			End Try

			'# End

			'Commented By Utkarsh ON 03-Apr-2013 FOR ALL03042013
			'If takeofftouchdown Then
			'    calTouchDownLocalDateTime.Text = calArrival.Text 'Added By Utkarsh On 31-Aug-2011
			'    ViewState("calTouchDownLocalDateTime") = calArrival.Text.Trim
			'End If

			'END

			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			''''If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(calArrival.Value.ToString).Date) <> 0 Then
			If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(calArrival.Text.ToString).Date) <> 0) Or
				(calDeparture.Text = "") Then
				mLog.DesLocalDateTime = CType(calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim, DateTime) 'calArrival.Text.Trim
				'Commented By Utkarsh ON 03-Apr-2013 FOR ALL03042013
				'If takeofftouchdown Then
				'    mLog.TouchDownLocalDateTime = calTouchDownLocalDateTime.Text.Trim 'Added By Utkarsh On 30-Aug-2011
				'    calUTCTouchDownDateTime.Text = Format(CDate(mLog.TouchDownUniverseDateTime), AppSettings("DateTimeFormatLOG")) 'Added By Utkarsh On 30-Aug-2011

				'End If
				'End
				'CalUTCArrival.Text = Format(CDate(mLog.DesUniverseDateTime), AppSettings("DateFormat")) 'Format(CDate(mLog.DesUniverseDateTime), AppSettings("DateTimeFormatLOG")) ''mLog.DesUniverseDateTime
				Session("mLog") = mLog
			End If

			txtAirBorneTime.DataBind()
			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtArrivalTime.Focus() 'SetFocus after databind
			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub
	Private Sub calDeparture_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDeparture.TextChanged
		If IsPostBack Then
			'calDeparture.Text = Format(Convert.ToDateTime(calDeparture.Text), AppSettings("DateTimeFormatLOG"))
			If Trim(calDeparture.Text) = "" Then
				ViewState("CalDeparture") = calDateTime.Text.Trim
				Exit Sub
			End If

			'# Date Control Validation #


			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = calDeparture.Text.Trim
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
				calDeparture_TextChanged(calDeparture.Text, e)  'Raising textchange event for further calculation
				Exit Sub
			End Try

			'# End

			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			''''''''''If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(calDeparture.Value.ToString).Date) <> 0 Then
			If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), New SmartDate(calDeparture.Text.ToString).Date) <> 0) Or
				(calArrival.Text = "") Then
				REM: Clone the object
				Dim clnLog As Log
				clnLog = CType(mLog.Clone, Log)

				'CNDC
				'clnLog.SouLocalDateTime = calDeparture.Text
				''''''''''clnLog.SouLocalDateTime = calDeparture.Value
				' clnLog.SouLocalDateTime = Format(calDeparture.Text + " " + calDepartureTime.Text, AppSettings("DateTimeFormatLOG"))
				'clnLog.SouLocalDateTime = Format(calDeparture.Text.ToString, AppSettings("DateTimeFormatLOG"))
				clnLog.SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime) ' calDeparture.Text.ToString.Trim
				If mLog.IsNew Then
					'CNDC
					'NewRecord(calDateTime.Text, calDeparture.Text)
					'NewRecord(calDateTime.Text.ToString, calDeparture.Text.ToString)
					NewRecord(calDateTime.Text.ToString, CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime).ToString)
					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(calDeparture.Text))
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
				'DataBind() 'Hobbs - taken
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
	End Sub

	Private Sub CalUTCArrival_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalUTCArrival.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007

			If Trim(CalUTCArrival.Text) = "" Then
				ViewState("CalUTCArrival") = calDateTime.Text.Trim
				Exit Sub
			End If

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = CalUTCArrival.Text.Trim
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
			'Commented By Utkarsh ON 03-Apr-2013 FOR ALL03042013
			'If takeofftouchdown Then
			'    calUTCTouchDownDateTime.Text = CalUTCArrival.Text  'Added By Utkarsh On 31-Aug-2011
			'    ViewState("calUTCTakeOffDateTime") = CalUTCArrival.Text.Trim
			'End If
			'End
			'CNDC
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouLocalDateTime.ToString), SmartDate.StringToDate(calDeparture.Text)) <> 0 Then
			If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(CalUTCArrival.Text.ToString).Date) <> 0) Or
				(CalUTCDateTime.Text = "") Then
				mLog.DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime) 'CalUTCArrival.Text.Trim
				'Commented By Utkarsh ON 03-Apr-2013 FOR ALL03042013
				'If takeofftouchdown Then
				'    mLog.TouchDownUniverseDateTime = calUTCTouchDownDateTime.Text.Trim 'Added By Utkarsh on 30-Aug-2011
				'End If
				'calArrival.Value = mLog.DesLocalDateTime
				'End
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
			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtArrivalTime.Focus() 'SetFocus after databind
			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub
	Private Sub CalUTCDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalUTCDateTime.TextChanged
		If IsPostBack Then         'Added Code on May,29,2007

			'CalUTCDateTime.Text = Format(Convert.ToDateTime(CalUTCDateTime.Text), AppSettings("DateTimeFormatLOG"))
			If Trim(CalUTCDateTime.Text) = "" Then
				ViewState("CalUTCDateTime") = calDateTime.Text.Trim
				Exit Sub
			End If

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = CalUTCDateTime.Text.Trim
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
			'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), SmartDate.StringToDate(CalUTCDateTime.Text)) <> 0 Then
			If (DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.SouUniverseDateTime.ToString), New SmartDate(CalUTCDateTime.Text.ToString).Date) <> 0) Or
				(CalUTCArrival.Text = "") Then
				REM: Clone the object
				Dim clnLog As Log
				clnLog = CType(mLog.Clone, Log)

				Dim dtString As DateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)
				'CNDC
				'clnLog.SouUniverseDateTime = CalUTCDateTime.Text
				clnLog.SouUniverseDateTime = dtString  'CalUTCDateTime.Text.ToString.Trim

				If mLog.IsNew Then
					'CNDC
					'NewRecord(calDateTime.Text, , CalUTCDateTime.Text)
					'NewRecord(calDateTime.Text.ToString, , CalUTCDateTime.Text.ToString)
					NewRecord(calDateTime.Text.ToString, , dtString.ToString)
					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
				Else
					'CNDC
					'EditRecord(SmartDate.StringToDate(CalUTCDateTime.Text))
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
			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()

			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()
			upnlFileupload.Update()

		End If
	End Sub

	Private Sub calTakeOffLocalDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calTakeOffLocalDateTime.TextChanged
		If IsPostBack Then

			If Trim(calTakeOffLocalDateTime.Text) = "" Then
				ViewState("calTakeOffLocalDateTime") = calDateTime.Text.Trim
				Exit Sub
			End If

			'# Date Control Validation #

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = calTakeOffLocalDateTime.Text.Trim
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
			'mLog.TakeOffLocalDateTime = calTakeOffLocalDateTime.Text.Trim
			'Session("mLog") = mLog
			' calTakeOffLocalDateTime.Text = Format(CDate(calTakeOffLocalDateTime.Text.ToString.Trim), AppSettings("DateTimeFormatLOG")) 'Added By Utkarsh On 30-Aug-2011

			If calTakeOffLocalDateTime.Text.ToString = "" Then
				mLog.TakeOffLocalDateTime = ""
				mLog.TouchDownLocalDateTime = ""
			End If
			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtTakeOffLocalTime.Focus() 'SetFocus after databind

			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub
	Private Sub calTouchDownLocalDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calTouchDownLocalDateTime.TextChanged

		If IsPostBack Then

			If Trim(calTouchDownLocalDateTime.Text) = "" Then
				ViewState("calTouchDownLocalDateTime") = calDateTime.Text.Trim
				Exit Sub
			End If
			'# Date Control Validation #

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = calTouchDownLocalDateTime.Text.Trim
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
			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtTouchDownLocalTime.Focus() 'SetFocus after databind
			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub

	Private Sub calUTCTakeOffDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calUTCTakeOffDateTime.TextChanged
		If IsPostBack Then

			If Trim(calUTCTakeOffDateTime.Text) = "" Then
				ViewState("calUTCTakeOffDateTime") = calDateTime.Text.Trim
				Exit Sub
			End If

			'# Date Control Validation #

			Try
				Dim tempdate As DateTime
				'Dim Datestring As String = calUTCTakeOffDateTime.Text.Trim
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
			'mLog.TakeOffUniverseDateTime = calUTCTakeOffDateTime.Text.Trim
			'Session("mLog") = mLog
			'calUTCTakeOffDateTime.Text = Format(CDate(calUTCTakeOffDateTime.Text.ToString.Trim), AppSettings("DateTimeFormatLOG")) 'Added By Utkarsh On 30-Aug-2011

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtUTCTakeOffTime.Focus() 'SetFocus after databind
			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub
	Private Sub calUTCTouchDownDateTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calUTCTouchDownDateTime.TextChanged
		If IsPostBack Then

			If Trim(calUTCTouchDownDateTime.Text) = "" Then
				ViewState("calUTCTouchDownDateTime") = calDateTime.Text.Trim
				Exit Sub
			End If

			'# Date Control Validation #

			Try

				Dim tempdate As DateTime
				'Dim Datestring As String = calUTCTouchDownDateTime.Text.Trim
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

			' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
			RefreshControlValues()
			txtUTCTouchDownTime.Focus() 'SetFocus after databind
			upnlFlightSummary.Update()
			upnlAirframeDetail.Update()
			upnlEngineDetail.Update()
			upnlAPUDetail.Update()
			upnlCGBDetail.Update()

		End If
	End Sub

	'Private Sub btnFuelOil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuelOil.Click
	'    'Code Added By Deven 21-03-2008 
	'    'BindClassification()
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    Session("OpenFromWO") = False
	'    Session("mOpenFromLogFuelNew") = False
	'    '-------------------------------
	'    Response.Redirect("wfLogFuelOil_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnDefectActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefectActionList.Click
	'    'Code Added By Deven 21-03-2008 
	'    'BindClassification()
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    Session("Edit") = False
	'    '-------------------------------
	'    Response.Redirect("wfLogDefectActionList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnLogPax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogPax.Click
	'    'Code Added By Deven 21-03-2008 
	'    'BindClassification()
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    '-------------------------------
	'    NewLogPax()
	'    'Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	'    Response.Redirect("wfLogPax_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnHobbsOffset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHobbsOffset.Click
	'    'Code Added By Deven 21-03-2008 
	'    'BindClassification()
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    '-------------------------------
	'    NewHobbsOffSet()
	'    Response.Redirect("wfHobbsOffset_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnParameterList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParameterList.Click
	'    'Code Added By Deven 21-03-2008 
	'    'BindClassification()
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    '-------------------------------
	'    Response.Redirect("wfLogParameterList_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnFlightCrew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFlightCrew.Click
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    Response.Redirect("wfLogFlightCrew_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub btnMaintenanceAcitvity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMaintenanceAcitvity.Click
	'    SetObject()
	'    SetAirFrameGridObject()
	'    SetEngineGridObject(True)
	'    SetAPUGridObject(True)
	'    SetCGBGridObject(True)
	'    'Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfLogSOPEdit_Ajax.aspx")
	'    Response.Redirect("wfLogMaintenanceActivity_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=wfLogSOPEdit_Ajax.aspx")
	'End Sub
	'Private Sub lnkAllAssembly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllAssembly.Click
	'    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowAssembly", "ShowAssembly();", True)
	'End Sub
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

#Region " Refresh Controls Values "
	' '' ''AJAX- New Method
	Private Sub RefreshControlValues(Optional ByVal isDatabindFromAirborn As Boolean = False)
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

	' '' ''AJAX- New Event for MessageBox Control 
	Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		AjaxLoader.Attributes.Add("Style=z-index", MSGBoxCtrl.Attributes("Style=z-index") + 1)
		MessageBoxResult()
	End Sub

	' '' ''AJAX- New Event
	Protected Sub txtPercentTimeOnGround_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPercentTimeOnGround.TextChanged, txtCurrentHobbsValue.TextChanged
		' '' ''AJAX- Above methods shifted in one new method"RefreshControlValues"; as above code is repeatedly get called again and again from different locations
		RefreshControlValues()
	End Sub
	Protected Sub txtAirBorneTime_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtAirBorneTime.TextChanged
		If Not TakeOffTouchDown Or mLog.IsLogAirborneEntry = True Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True
			mLog.TimeInAir = Trim(txtAirBorneTime.Text)
			Session("mLog") = mLog
		End If
		RefreshControlValues(True)
		txtGroundRunTime.Focus()
	End Sub
	'Private Sub dgAFPeriods_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgAFPeriods.RowDataBound
	'    If e.Row.RowType = DataControlRowType.DataRow Then

	'        Dim idx As Integer = e.Row.RowIndex

	'        DirectCast(e.Row.FindControl("txtAirFrameHours"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameLandings"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameStarts"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameNGCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameNFCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameRins"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameBleeds"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameImpellerCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFrameCTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirFramePTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAirframeGeneratorMods"), TextBox).Attributes.Add("onfocus", "onTextFocus();")

	'    End If

	'End Sub


	'Private Sub dgEnginePeriods_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgEnginePeriods.RowDataBound

	'    If e.Row.RowType = DataControlRowType.DataRow Then

	'        DirectCast(e.Row.FindControl("txtEngineHours"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineLandings"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineStarts"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("btnEngineNGCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineNFCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineRins"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineCFactors"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineBleeds"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineImpellerCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineCTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEnginePTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtEngineGeneratorMods"), TextBox).Attributes.Add("onfocus", "onTextFocus();")

	'    End If

	'End Sub

	'Private Sub dgAPUPeriods_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgAPUPeriods.RowDataBound

	'    If e.Row.RowType = DataControlRowType.DataRow Then

	'        DirectCast(e.Row.FindControl("txtAPUHours"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPULandings"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUStarts"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUNGCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUNFCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPURins"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUBleeds"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUImpellerCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUCTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUPTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtAPUGeneratorMods"), TextBox).Attributes.Add("onfocus", "onTextFocus();")

	'    End If

	'End Sub

	'Private Sub dgCGBPeriods_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgCGBPeriods.RowDataBound

	'    If e.Row.RowType = DataControlRowType.DataRow Then

	'        DirectCast(e.Row.FindControl("txtCGBHours"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBLandings"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBStarts"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBNGCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBNFCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBRINS"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBBleeds"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBImpellerCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBCTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBPTCycles"), TextBox).Attributes.Add("onfocus", "onTextFocus();")
	'        DirectCast(e.Row.FindControl("txtCGBGeneratorMods"), TextBox).Attributes.Add("onfocus", "onTextFocus();")

	'    End If

	'End Sub

	' '' ''AJAX- New Event For JQuery control. As Page partialy PostBack Object(Log) value doesn't reflects in HTML. So Put Object values in Hidden Field and use it in HTML JQuery
	Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
		LogObjValue.Value = IIf(mLog.IsNew, "True", "False")

	End Sub
	'Added by utkarsh on 03-oct-2013 for log_ajax changes
	Protected Sub txtGroundRunTime_TextChanged(sender As Object, e As EventArgs) Handles txtGroundRunTime.TextChanged
		If Not AppSettings("Log") = "True" Or mLog.IsLogAirborneEntry = True Then  ''Added by Saylee on 1-Sep-2021 for ALL01092021 : mLog.IsLogAirborneEntry = True
			mLog.TimeOnGround = Trim(txtGroundRunTime.Text)
			Session("mLog") = mLog
		End If

		RefreshControlValues(True)
	End Sub
	'End
	Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
		mLog.IsAttachmentAdded = True
		ControlVisibilityForAttachment()
		upnlFileupload.Update()
	End Sub
	Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttch.Click
		Dim fileSize1 As Integer = 0
		Dim file1(fileSize1) As Byte
		GetAttachment()
		mFileAttach.ImageFile = file1
		mFileAttach.Size = 0
		ImageButton1.Visible = False
		btnDelAttch.Enabled = False
		IsAttachmentDeleted = True
		mLog.IsAttachmentAdded = False
		Session("IsAttachmentDeleted") = IsAttachmentDeleted
	End Sub
	Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
		If mLog.IsAttachmentAdded Then
			mFileAttach = FileAttach.GetAttachment(mLog.ID)
		Else
			mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mLog.ID)
		End If
		Session("mFileAttach") = mFileAttach
	End Sub
	Private Sub txtDepartureTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDepartureTime.TextChanged
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
					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
					CopyFromClone(clnLog, True)
				End If
				' -------------

				DataFieldBind()
				'SetFocus after databind
				If TakeOffTouchDown Then
					chkTakeOff.Focus()
				Else
					If (Not (mMachine.IsUTC) And TakeOffTouchDown) Then
						calTakeOffLocalDateTime.Focus()
					Else
						calUTCTakeOffDateTime.Focus()
					End If
				End If
				'End
			End If
		End If
	End Sub
	Private Sub txtUTCDepartureTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUTCDepartureTime.TextChanged
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
					Session.Remove("mFileAttach")
					Session.Remove("IsAttachmentDeleted")
					CopyFromClone(clnLog, True)
				End If
				' -------------

				DataFieldBind()
				Place2.Focus() 'SetFocus after databind
			End If
		End If
	End Sub
	Private Sub txtTakeOffLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTakeOffLocalTime.TextChanged
		If IsValidTime(txtTakeOffLocalTime.Text.ToString.Trim) = False Then
			txtTakeOffLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim
			mLog.TakeOffLocalDateTime = DateTime
			DataFieldBind()
			Place2.Focus() 'SetFocus after databind
		End If
	End Sub
	Private Sub txtUTCTakeOffTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTakeOffTime.TextChanged
		If IsValidTime(txtUTCTakeOffTime.Text.ToString.Trim) = False Then
			txtUTCTakeOffTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim
			mLog.TakeOffUniverseDateTime = DateTime
			DataFieldBind()
			Place2.Focus() 'SetFocus after databind
		End If
	End Sub
	Private Sub txtArrivalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtArrivalTime.TextChanged
		If IsValidTime(txtArrivalTime.Text.ToString.Trim) = False Then
			txtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else

			'calTouchDownLocalDateTime.Text = calArrival.Text.Trim
			'txtTouchDownLocalTime.Text = txtArrivalTime.Text.Trim

			Dim DateTime As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mLog.DesLocalDateTime = DateTime
				'mLog.TouchDownLocalDateTime = DateTime
				DataFieldBind()
				DataBindGrid()
				'SetFocus after databind
				If TakeOffTouchDown Then
					chkTouchDown.Focus()
				Else
					If (Not (mMachine.IsUTC) And TakeOffTouchDown) Then
						calTouchDownLocalDateTime.Focus()
					Else
						calUTCTouchDownDateTime.Focus()
					End If
				End If
				'End
			End If
		End If
	End Sub
	Private Sub txtUTCArrivalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCArrivalTime.TextChanged
		If IsValidTime(txtUTCArrivalTime.Text.ToString.Trim) = False Then
			txtUTCArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			'calUTCTouchDownDateTime.Text = CalUTCArrival.Text.Trim
			'txtUTCTouchDownTime.Text = txtUTCArrivalTime.Text.Trim
			Dim DateTime As String = CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim
			If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLog.DesUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
				mLog.DesUniverseDateTime = DateTime
				'mLog.TouchDownUniverseDateTime = DateTime
				DataFieldBind()
				DataBindGrid()
			End If
		End If
	End Sub
	Private Sub txtTouchDownLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTouchDownLocalTime.TextChanged
		If IsValidTime(txtTouchDownLocalTime.Text.ToString.Trim) = False Then
			txtTouchDownLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim
			mLog.TouchDownLocalDateTime = DateTime
			DataFieldBind()
			DataBindGrid()
			txtAirBorneTime.Focus() 'SetFocus after databind
		End If
	End Sub
	Private Sub txtUTCTouchDownTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTouchDownTime.TextChanged
		If IsValidTime(txtUTCTouchDownTime.Text.ToString.Trim) = False Then
			txtUTCTouchDownTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
		Else
			Dim DateTime As String = calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim
			mLog.TouchDownUniverseDateTime = DateTime
			DataFieldBind()
			DataBindGrid()
			txtAirBorneTime.Focus() 'SetFocus after databind
		End If
	End Sub
	Private Sub chkArrival_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkArrival.CheckedChanged
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
		'ArrivalDate = chkArrival.Checked
		'Session("ArrivalDate") = ArrivalDate
	End Sub
	Private Sub chkTouchDown_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkTouchDown.CheckedChanged
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
		'ArrivalDate = chkArrival.Checked
		'Session("ArrivalDate") = ArrivalDate
	End Sub
	Private Sub chkTakeOff_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkTakeOff.CheckedChanged
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
	End Sub

#Region "Web Methods"
	<WebMethod(EnableSession:=True)>
	Public Shared Function LogDetails(MachineID, LogDate) As Object
		Dim mLogListOnDate As LogList = LogList.GetLogList(MachineID, LogDate.Text.ToString, LogDate.Text.ToString)
		Return mLogListOnDate
	End Function
#End Region
End Class