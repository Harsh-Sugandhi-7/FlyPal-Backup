'Created By Utkarsh ON 02-Apr-2012
Imports System.Linq
Partial Class wfTLPDetail_Ajax
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

#Region "Variable Declaration"
    Public mLog As Log
    Public mLogDetail As LogDetail
    Public mRegNo As String
    Public mFlightLogClassificationList As FlightLogClassificationList
    Private Flag As Int16
    Dim Type As Integer
    Private LogListCount As Integer = 0
    Dim EventLogID As Guid

    Public mSearchListPlace As SearchList

    Dim Pilot1ID As Guid
    Dim Pilot2ID As Guid
    Dim SourceID As Guid
    Dim DestinationID As Guid
    Dim SetValue As Boolean = False

    Public Event TextChanged As EventHandler
    Public ArrivalDate As Boolean = False
    Public TouchDownDate As Boolean = False
    Public TakeOffDate As Boolean = False

    Public mMachine As Machine  'Added By Saylee On 12-Feb-2014 For ALL12022014-1
    Public mIsLastLog As Boolean 'Added By Saylee on 31-Aug-2016 
    Public mIsLastLogTLP As Boolean = True 'Added By Saylee on 31-Aug-2016 


#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mLog = CType(Session("mLog"), Log)
        mLogDetail = CType(Session("mLogDetail"), LogDetail)
        mRegNo = Session("mRegNo")
        LogListCount = CType(Session("LogListCount"), Integer)
        mSearchListPlace = Session("mSearchListPlace")
        ArrivalDate = Session("ArrivalDate")
        TouchDownDate = Session("TouchDownDate")
        TakeOffDate = Session("TakeOffDate")
        mMachine = CType(Session("mMachine"), Machine)  'Added By Saylee On 12-Feb-2014 For ALL12022014-1
        mIsLastLog = CType(Session("mIsLastLog"), Boolean)
        mIsLastLogTLP = CType(Session("mIsLastLogTLP"), Boolean)
    End Sub
    Private Sub SetSession()
        Session("mLog") = mLog
        Session("mLogDetail") = mLogDetail
        Session("mRegNo") = mRegNo
        Session("LogListCount") = LogListCount
        Session("mSearchListPlace") = mSearchListPlace

        Session("mMachine") = mMachine ' 'Added By Saylee On 12-Feb-2014 For ALL12022014-1
        Session("mIsLastLog") = mIsLastLog
        Session("mIsLastLogTLP") = mIsLastLogTLP
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRegNo")
        Session.Remove("mSearchListPlace")
        Session.Remove("mLogDetail")
        Session.Remove("LogDetailEdit")
        Session.Remove("Index")
        Session.Remove("ArrivalDate")
        Session.Remove("TouchDownDate")
        Session.Remove("TakeOffDate")
    End Sub
    Private Sub EnableDisableButton(Optional ByVal IsLastTLPDetail As Boolean = True)
        '   Dim IsLastTLPDetail As Boolean = mLog.IsLastTLPDetail(mLogDetail.ID) And mIsLastLog = True
        If mIsLastLog = False Then
            IsLastTLPDetail = False
        End If

        If (Not mLogDetail.IsNew Or Session("LogDetailEdit") = True) And IsLastTLPDetail = False Then
            txtAirBorneTime.BackColor = Color.Gainsboro
            txtBlockTime.BackColor = Color.Gainsboro
            txtLandings.BackColor = Color.Gainsboro

            txtAirBorneTime.ReadOnly = True
            txtBlockTime.ReadOnly = True
            txtLandings.ReadOnly = True

            chkArrival.Enabled = False
            chkUTCArrival.Enabled = False
            chkTouchDown.Enabled = False
            chkUTCTouchDown.Enabled = False
            chkTakeOff.Enabled = False
            chkUTCTakeOff.Enabled = False

        Else
            txtAirBorneTime.BackColor = Color.White
            txtBlockTime.BackColor = Color.White
            txtLandings.BackColor = Color.White

            txtAirBorneTime.ReadOnly = False
            txtBlockTime.ReadOnly = False
            txtLandings.ReadOnly = False

            chkArrival.Enabled = True
            chkUTCArrival.Enabled = True
            chkTouchDown.Enabled = True
            chkUTCTouchDown.Enabled = True
            chkTakeOff.Enabled = True
            chkUTCTakeOff.Enabled = True

        End If

        'Date 
        calArrival.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkArrival.Checked = False, True, False)
        calTouchDownLocalDateTime.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkTouchDown.Checked = False, True, False)

        CalUTCArrival.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCArrival.Checked = False, True, False)
        calUTCTouchDownDateTime.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCTouchDown.Checked = False, True, False)

        calTakeOffLocalDateTime.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkTakeOff.Checked = False, True, False)
        calUTCTakeOffDateTime.ReadOnly = IIf(Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCTakeOff.Checked = False, True, False)


        If LogListCount > 0 And mLog.PrevLogUniversalDateTime.ToString("yyyy") <> "9999" And mLog.IsNew = True And mLog.SouLocalDateTime.ToString <> "" Then
            calDeparture.Enabled = True
            calArrival.Enabled = True
            calDeparture.ReadOnly = Not (True)
            calArrival.ReadOnly = Not (True)
        End If
        If Not calDeparture.Enabled Then
            calDeparture.BackColor = Color.Gainsboro
        End If
        If Not calArrival.Enabled Then
            calArrival.BackColor = Color.Gainsboro
        End If
        '-End Date

        calArrival.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkArrival.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        CalUTCArrival.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCArrival.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        calTouchDownLocalDateTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkTouchDown.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        calUTCTouchDownDateTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCTouchDown.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        calTakeOffLocalDateTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkTakeOff.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        calUTCTakeOffDateTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True" Or chkUTCTakeOff.Checked = False) And IsLastTLPDetail = False, Color.Gainsboro, Color.White)


        Place1.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        Place2.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)

        Place1.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        Place2.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        txtFuelUplifted.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtFuelOnArrival.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)

        txtFuelUplifted.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtFuelOnArrival.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)


        txtFuelOnAdded.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtFuelOnAdded.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        txtFuelOnDeparture.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtFuelOnDeparture.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)


        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
            txtFuelUplifted.BackColor = Color.Gainsboro
            txtFuelUplifted.ReadOnly = "True"
            txtTotalFuelOnDeparture.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
            txtTotalFuelOnDeparture.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        Else
            txtTotalFuelOnDeparture.ReadOnly = True
            txtTotalFuelOnDeparture.BackColor = Color.Gainsboro
        End If

        'Time Boxes
        txtDepartureTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtUTCDepartureTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtArrivalTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtUTCArrivalTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)

        txtTakeOffLocalTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtUTCTakeOffTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtTouchDownLocalTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtUTCTouchDownTime.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)

        txtDepartureTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtUTCDepartureTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtArrivalTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtUTCArrivalTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        txtTakeOffLocalTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtUTCTakeOffTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtTouchDownLocalTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)
        txtUTCTouchDownTime.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        txtFlightNo.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, True, False)
        txtFlightNo.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, Color.Gainsboro, Color.White)

        chkIsPFIDone.Enabled = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False, False, True)
        'txtEmployee.ReadOnly = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False And chkIsPFIDone.Checked, True, False)
        'txtEmployee.BackColor = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = "True") And IsLastTLPDetail = False And chkIsPFIDone.Checked, Color.Gainsboro, Color.White)
        'txtEmployee.ReadOnly = IIf(chkIsPFIDone.Checked And chkIsPFIDone.Enabled, False, True)
        'txtEmployee.BackColor = IIf(chkIsPFIDone.Checked And chkIsPFIDone.Enabled, Color.White, Color.Gainsboro)
        txtEmployee.ReadOnly = IIf(chkIsPFIDone.Checked And chkIsPFIDone.Enabled, False, True)
        txtEmployee.BackColor = IIf(chkIsPFIDone.Checked And chkIsPFIDone.Enabled, Color.White, Color.Gainsboro)
        txtEmployee.Enabled = IIf(chkIsPFIDone.Checked And chkIsPFIDone.Enabled, True, False)
        '*pnlHours   

        lblAirBorneTime.Visible = True
        txtAirBorneTime.Visible = True
        txtBlockTime.Visible = True
        txtLandings.Visible = True

        'Commeneted and Added By Saylee On 12-Feb-2014 For ALL12022014-1
        ''lblTakeOffLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC"))
        ''lblUTCTakeOffDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC"))
        ''lblTouchDownLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC"))
        ''lblUTCTouchDownDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC"))

        ''calTouchDownLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC"))
        ''calUTCTouchDownDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC"))
        ''calTakeOffLocalDateTime.Visible = (Not (AppSettings("LogBookTimeEntry") = "UTC"))
        ''calUTCTakeOffDateTime.Visible = ((AppSettings("LogBookTimeEntry") = "UTC"))


        ''calDeparture.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblDepDateTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblDateTimeStar1.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

        ''calArrival.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblDateTimeStar2.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblArrDate.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

        ''CalUTCDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblUTCDateTimeStar1.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblUTCDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

        ''CalUTCArrival.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblUTCDateTimeStar2.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblUTCArrivalDateTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

        ''txtDepartureTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtArrivalTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtTakeOffLocalTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtTouchDownLocalTime.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

        ''txtUTCDepartureTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtUTCArrivalTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtUTCTakeOffTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''txtUTCTouchDownTime.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

        ''chkArrival.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''chkUTCArrival.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''chkTouchDown.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''chkUTCTouchDown.Visible = (AppSettings("LogBookTimeEntry") = "UTC")


        ''lblTakeOffStar.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblTouchDownStar.Visible = Not (AppSettings("LogBookTimeEntry") = "UTC")

        ''lblUTCTakeOffStar.Visible = (AppSettings("LogBookTimeEntry") = "UTC")
        ''lblUTCTouchDownStar.Visible = (AppSettings("LogBookTimeEntry") = "UTC")

        lblTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC))
        lblUTCTakeOffDateTime.Visible = ((mMachine.IsUTC))
        lblTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC))
        lblUTCTouchDownDateTime.Visible = ((mMachine.IsUTC))

        calTouchDownLocalDateTime.Visible = (Not (mMachine.IsUTC))
        calUTCTouchDownDateTime.Visible = ((mMachine.IsUTC))
        calTakeOffLocalDateTime.Visible = (Not (mMachine.IsUTC))
        calUTCTakeOffDateTime.Visible = ((mMachine.IsUTC))


        calDeparture.Visible = Not (mMachine.IsUTC)
        lblDepDateTime.Visible = Not (mMachine.IsUTC)
        lblDateTimeStar1.Visible = Not (mMachine.IsUTC)

        calArrival.Visible = Not (mMachine.IsUTC)
        lblDateTimeStar2.Visible = Not (mMachine.IsUTC)
        lblArrDate.Visible = Not (mMachine.IsUTC)

        CalUTCDateTime.Visible = (mMachine.IsUTC)
        lblUTCDateTimeStar1.Visible = (mMachine.IsUTC)
        lblUTCDateTime.Visible = (mMachine.IsUTC)

        CalUTCArrival.Visible = (mMachine.IsUTC)
        lblUTCDateTimeStar2.Visible = (mMachine.IsUTC)
        lblUTCArrivalDateTime.Visible = (mMachine.IsUTC)

        txtDepartureTime.Visible = Not (mMachine.IsUTC)
        txtArrivalTime.Visible = Not (mMachine.IsUTC)
        txtTakeOffLocalTime.Visible = Not (mMachine.IsUTC)
        txtTouchDownLocalTime.Visible = Not (mMachine.IsUTC)

        txtUTCDepartureTime.Visible = (mMachine.IsUTC)
        txtUTCArrivalTime.Visible = (mMachine.IsUTC)
        txtUTCTakeOffTime.Visible = (mMachine.IsUTC)
        txtUTCTouchDownTime.Visible = (mMachine.IsUTC)

        chkArrival.Visible = Not (mMachine.IsUTC)
        chkUTCArrival.Visible = (mMachine.IsUTC)
        chkTouchDown.Visible = Not (mMachine.IsUTC)
        chkUTCTouchDown.Visible = (mMachine.IsUTC)


        lblTakeOffStar.Visible = Not (mMachine.IsUTC)
        lblTouchDownStar.Visible = Not (mMachine.IsUTC)

        lblUTCTakeOffStar.Visible = (mMachine.IsUTC)
        lblUTCTouchDownStar.Visible = (mMachine.IsUTC)

        chkTakeOff.Visible = Not (mMachine.IsUTC)
        chkUTCTakeOff.Visible = (mMachine.IsUTC)
    End Sub
    Private Sub ControlVisibility(Optional ByVal IsLastTLPDetail As Boolean = True)
        lblDateTimeStar1.Visible = False
        lblDateTimeStar2.Visible = False
        lblUTCDateTimeStar1.Visible = False
        lblUTCDateTimeStar2.Visible = False
        lblPlaceStar2.Visible = False
        lblPalceStar1.Visible = False

        lblTakeOffStar.Visible = False
        lblTouchDownStar.Visible = False

        lblUTCTakeOffStar.Visible = False
        lblUTCTouchDownStar.Visible = False

        'Dim IsLastTLPDetail As Boolean = mLog.IsLastTLPDetail(mLogDetail.ID)
        If mIsLastLog = True Then
            btnAdd.Enabled = IIf((Not mLogDetail.IsNew Or Session("LogDetailEdit") = True) And IsLastTLPDetail = False, False, True)
        ElseIf mLog.IsNew Then
            btnAdd.Enabled = True
        Else
            btnAdd.Enabled = False
        End If
        'IIf(((mLogDetail.IsNew Or (Session("LogDetailEdit") = "True" And IsLastTLPDetail = True And mIsLastLog = True))), True, False)

    End Sub

    Private Sub SetObject()

        Try

            With mLogDetail

                If mLog.IsUTC = True Then

                    If Not IsDate(CalUTCDateTime.Text) Then
                        .SouUniverseDateTime = DBNull.Value
                    Else
                        .SouUniverseDateTime = CType(CalUTCDateTime.Text.ToString.Trim + " " + txtUTCDepartureTime.Text.ToString.Trim, DateTime)
                    End If

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

                Else

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

                    If Not IsDate(calDeparture.Text) Then
                        .SouLocalDateTime = DBNull.Value
                    Else
                        .SouLocalDateTime = CType(calDeparture.Text.ToString.Trim + " " + txtDepartureTime.Text.ToString.Trim, DateTime)
                    End If

                End If

                If mLog.IsUTC = True Then

                    If Not IsDate(CalUTCArrival.Text) Then
                        .DesUniverseDateTime = DBNull.Value
                    Else
                        .DesUniverseDateTime = CType(CalUTCArrival.Text.ToString.Trim + " " + txtUTCArrivalTime.Text.ToString.Trim, DateTime)

                    End If

                End If

                .TimeInAir = Trim(txtAirBorneTime.Text)
                .BlockTime = Trim(txtBlockTime.Text)
                .FlightNo = txtFlightNo.Text.Trim
                .Landings = Val(txtLandings.Text.Trim)

                'Fuel Detail
                .FuelUplifted = Val(txtFuelUplifted.Text.Trim)
                .FuelOnArrival = Val(txtFuelOnArrival.Text.Trim)
                .FuelOnDeparture = Val(txtFuelOnDeparture.Text.Trim)

                If AppSettings("ClientCode") = "BA" Or
                   AppSettings("ClientCode") = "YA" Or
                   AppSettings("ClientCode") = "TA" Then

                    .TotalFuelOnDeparture = Val(txtTotalFuelOnDeparture.Text.Trim)
                End If

                'Weight Info
                .CargoWeight = Val(txtCargoWeight.Text.Trim)
                .TakeOffWeight = Val(txtTakeOffWeight.Text.Trim)
                .IsPFIDone = chkIsPFIDone.Checked 'Added by vikrant on 14-Feb-2019 For ALL14022019
                .FuelOnAdded = Val(txtFuelOnAdded.Text.Trim)
                .PaxAdult = Val(txtPaxAdult.Text.Trim)
                .PaxChild = Val(txtPaxChild.Text.Trim)
                .PaxInfant = Val(txtPaxInfant.Text.Trim)
                .LHEngineOil = Val(txtLHEngineOil.Text.Trim)
                .RHEngineOil = Val(txtRHEngineOil.Text.Trim)
                .ExtraBaggage = Val(txtExtraBaggage.Text.Trim)

            End With

            Session("mLogDetail") = mLogDetail

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub
    Private Sub NewRecord()
        mLogDetail = LogDetail.NewChildLogDetail(mLog.ID, mLog.Date)
        If mLog.LogDetails.Count = 0 Then
            'Setting Source Place for the first child of the Log i.e. LogDetail
            mLogDetail.SourceID = mLog.SourceID
            mLogDetail.FuelOnDeparture = mLog.TotalFuelOnDeparture
        Else
            'Setting New Log Detail Child with default values
            mLogDetail.SourceID = mLog.LogDetails.CurrentItem.DestinationID
            mLogDetail.FuelOnDeparture = mLog.LogDetails.CurrentItem.FuelOnArrival
        End If
        Session("mLogDetail") = mLogDetail
        Session("mLog") = mLog
        'End
        Session("LogDetailEdit") = False
        Session("ArrivalDate") = False
        Session("TouchDownDate") = False
        Session("TakeOffDate") = False

        'Added By Utkarsh for Log_ajax page changes
        chkArrival.Checked = False
        chkUTCArrival.Checked = False
        chkTouchDown.Checked = False
        chkUTCTouchDown.Checked = False
        chkTakeOff.Checked = False
        chkUTCTakeOff.Checked = False
        'End
        EnableDisableButton(True)
        DataFieldBind()

        SetTitle()
    End Sub
    Private Sub AddTLP()
        If Not Session("LogDetailEdit") = True Then
            mLog.LogDetails.Add(mLogDetail)

            DataBindGrid()
        End If
        NewRecord()
        ControlVisibility()

        chkArrival.Checked = False
        chkUTCArrival.Checked = False
        chkTouchDown.Checked = False
        chkUTCTouchDown.Checked = False
        chkTakeOff.Checked = False
        chkUTCTakeOff.Checked = False
    End Sub
    Private Sub EditRecord(ByVal ID As Guid, ByVal Index As Integer)
        mLogDetail = mLog.LogDetails.Item(ID)

        'Added by Saylee on 29-Apr-2022
        Dim clnLogDetail As LogDetail
        Dim clnLogTLP As Log

        clnLogDetail = CType(mLogDetail.Clone, LogDetail)
        Session("clnLogDetail") = clnLogDetail

        clnLogTLP = CType(mLog.Clone, Log)
        Session("clnLogTLP") = clnLogTLP
        '**************************************************

        DataFieldBind()

        If Index <> mLog.LogDetails.Count Then
            mIsLastLogTLP = False
        Else
            mIsLastLogTLP = True
        End If
        Session("mIsLastLogTLP") = mIsLastLogTLP
        EnableDisableButton(mIsLastLogTLP)
        ControlVisibility(mIsLastLogTLP)
        Session("mLogDetail") = mLogDetail
        upnlDetails.Update()
        SetTitle()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        ' '' ''If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
        ' '' ''    Result1 = -1
        ' '' ''Else
        ' '' ''    Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        ' '' ''End If
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Remove" Then
                        mLog.LogDetails.Remove(mLog.LogDetails(mLog.LogDetails.CurrentIndex))
                        For i As Integer = 0 To mLog.LogDetails.Count - 1
                            mLog.LogDetails(i).SrNo = i + 1
                        Next
                        If Not mLog.IsNew Then
                            mLog = CType(mLog.Save(), Log)
                        End If
                        Session("mLog") = mLog
                        NewRecord()
                        'Added By utkarsh ON 24-sep-2013 FOr Log_ajax page changes
                        ControlVisibility()
                        'End
                        DataFieldBind()

                        ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        'Raise btnAdd Event
                        ''SetLogObject()
                        'Added by Saylee on 29-Apr-2022
                        If mLogDetail.IsNew Then

                        Else
                            Dim clnLogDetail As LogDetail
                            Dim clnLogTLP As Log
                            clnLogDetail = Session("clnLogDetail")
                            clnLogTLP = Session("clnLogTLP")
                            If Not clnLogDetail Is Nothing And Not clnLogTLP Is Nothing Then
                                '' mLogDetail = clnLogDetail
                                mLog = clnLogTLP
                                Session("mLog") = mLog
                                DataFieldBind()
                                upnlGrid.Update()
                                Session.Remove("clnLogTLP")
                                Session.Remove("clnLogDetail")
                            End If
                        End If
                        '**************************************************

                        RemoveSession()
                        ''  Response.Redirect("wfTLP_Ajax.aspx?MsgResult=0&BackPage=Index.aspx")
                        If Session("mTypeIDForLogEdit") = 2 Then
                            Response.Redirect("wfTLPEdit_Ajax.aspx?MsgResult=0&BackPage=Index.aspx")
                        Else
                            Response.Redirect("wfTLP_Ajax.aspx?MsgResult=0&BackPage=Index.aspx")
                        End If
                    End If
                    'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
                    If MSGBoxCtrl.Sender = "SaveLogAfterAvgFlightTimeDeviationWarning" Then
                        'If SaveLogAfterAvgFlightTimeDeviationWarning() = True Then
                        AddTLP()
                        upnlDetails.Update()
                        upnlTitle.Update()
                        'End If
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        DataFieldBind()
                        ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    End If
                    If MSGBoxCtrl.Sender = "Remove" Then
                        DataFieldBind()
                        ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    End If
                Case MsgBoxResult.Cancel
                    If MSGBoxCtrl.Sender = "Save" Or MSGBoxCtrl.Sender = "SaveNew" Then
                        DataFieldBind()
                        ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "ResetEmployee" Then
                        txtEmployee.Text = ""
                        If Not mLogDetail.PFIDoneByID.Equals(Guid.Empty) Then
                            txtEmployee.Text = mLogDetail.PFIDoneByEmpNoName
                        End If
                        upnlEmp.Update()
                    End If
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"
                    DataFieldBind()
                    ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            End Select
        ElseIf Result1 = -1 Then
            If Session("New") = "True" Then Session("New") = ""
            ' '' ''Response.Redirect("wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        ElseIf Result1 = 0 Then
            If Session("New") = "True" Then Session("New") = ""
        End If
    End Sub
    Private Sub SetTitle()
        Dim Index As Integer
        Index = Session("Index")

        If mLogDetail.IsNew Then
            If mLog.Date Is DBNull.Value Then
                lblTitle.Text = "Details of " & mRegNo & " as of - [New]"
            Else
                lblTitle.Text = "Details of " & mRegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText & " [New]"
            End If
        Else
            lblTitle.Text = "Details of " & mRegNo & " as of " & New SmartDate(mLog.Date.ToString).FormattedText
        End If

        upnlTitle.Update()
    End Sub

    Private Sub CopyFromClone(ByVal ClonedLogDetail As LogDetail, Optional ByVal isFromLogDate As Boolean = False)
        mLogDetail.SourceID = ClonedLogDetail.SourceID

        If Not mLog.IsNew Then
            mLogDetail.SouLocalDateTime = ClonedLogDetail.SouLocalDateTime
            mLogDetail.DesLocalDateTime = ClonedLogDetail.DesLocalDateTime
        End If

        mLogDetail.DestinationID = ClonedLogDetail.DestinationID
        If mLog.IsUTC Then
            If Not isFromLogDate Then
                mLogDetail.SouUniverseDateTime = ClonedLogDetail.SouUniverseDateTime
            End If
        End If
        If isFromLogDate Then
            mLog.DesLocalDateTime = ClonedLogDetail.DesLocalDateTime
            mLogDetail.TouchDownLocalDateTime = mLogDetail.DesLocalDateTime
        End If

        mLogDetail.TimeOnGround = ClonedLogDetail.TimeOnGround
        mLogDetail.TimeInAir = ClonedLogDetail.TimeInAir

        mLogDetail.FlightNo = ClonedLogDetail.FlightNo
        Session("mLogDetail") = mLogDetail
    End Sub
    Private Sub SetFromAutoComplete()

        Dim tempString As String
        Dim tempString1 As String

        tempString = Place1.Text.Trim
        If Not tempString = String.Empty Then
            If tempString.IndexOf("[") >= 0 Then
                tempString = tempString.Substring(0, tempString.IndexOf("[")).Trim
            End If
        End If

        tempString1 = Place2.Text.Trim
        If Not tempString1 = String.Empty Then
            If tempString1.IndexOf("[") >= 0 Then
                tempString1 = tempString1.Substring(0, tempString1.IndexOf("[")).Trim
            End If
        End If

        mLogDetail.SourceID = mSearchListPlace.Item(tempString).GId
        mLogDetail.DestinationID = mSearchListPlace.Item(tempString1).GId
        Session("mLogDetail") = mLogDetail

    End Sub
    Private Sub SetTakeoffTouchdownTitle()

        lblDepDateTime.Text = "ChocksOff Date/Time"
        lblUTCDateTime.Text = "UTC ChocksOff Date/Time"
        lblArrDate.Text = "ChocksOn Date/Time"
        lblUTCArrivalDateTime.Text = "UTC ChocksOn Date/Time"

    End Sub
    Private Sub SetLogObject()
        If Not mLog.LogDetails.Count = 0 Then

            'Time
            mLog.BlockTime = mLog.LogDetails.TotalBlockTime
            mLog.TimeInAir = mLog.LogDetails.TotalTimeInAir

            mLog.TimeOnGround = mLog.LogDetails.TotalTimeOnGround
            mLog.TotalTime = mLog.LogDetails.TotalTime

            'Fuel
            If mLog.LogFuels.Count > 0 Then
                mLog.LogFuels.CurrentItem.FuelUplifted = mLog.LogDetails.TotalFuelUplifted
                mLog.LogFuels.CurrentItem.FuelOnArrival = mLog.LogDetails(mLog.LogDetails.Count - 1).FuelOnArrival 'set Last child Fule at arrival to Log Fuel's Last Child
            End If

            'Total Landings
            If Not AppSettings("ClientCode") = "SIT" Then  'ClientCode added by Saylee on 11-Mar-2025 as SITA Air needed engine values to be different from Airframe
                mLog.TotalLandings = mLog.LogDetails.TotalLandings
            End If


            'Place
            mLog.SourceID = mLog.LogDetails(0).SourceID  'IF Source place changed
            mLog.DestinationID = mLog.LogDetails(mLog.LogDetails.Count - 1).DestinationID

            If Not mLog.IsNew And mLog.IsValid Then
                mLog = CType(mLog.Save(), Log)
            End If

        Else  'Set Default

            'Time
            mLog.BlockTime = "0:00"
            mLog.TimeInAir = "0:00"
            mLog.TimeOnGround = "0:00"
            mLog.TotalTime = "0:00"

            'Fuel
            If mLog.LogFuels.Count > 0 Then
                mLog.LogFuels.CurrentItem.FuelUplifted = 0D
                mLog.LogFuels.CurrentItem.FuelOnArrival = 0D 'set Last child Fule at arrival to Log Fuel's Last Child
            End If

            'Total Landings
            mLog.TotalLandings = 0D

            'Place
            'mLog.SourceID = mLog.LogDetails(0).SourceID  'IF Source place changed
            mLog.DestinationID = Guid.Empty


        End If
        Session("mLog") = mLog
    End Sub
    Private Function IsLogValid() As Boolean

        'Commented by Saylee on 28-apr-2022, as when edited ,to avoid overlapping of dates
        '''''''''''''Edit Log Detail
        ''''''''''''If Session("LogDetailEdit") = True Then
        ''''''''''''    Return True
        ''''''''''''End If
        '*************************************************************
        'Prev Log Details for Log Date Time Validation
        Dim mPrevLogDetail As PrevLogDetail = PrevLogDetail.GetPrevLogDetail(mLog.MachineID, mLog.Date.ToString, "")
        'Checking if previous log exist
        If mLog.LastLogExist Then
            'For first child
            If mLog.LogDetails.Count = 0 Then
                'Both Dates required
                If (Not mPrevLogDetail.DesUniverseDateTime Is DBNull.Value) And (Not mLogDetail.SouUniverseDateTime Is DBNull.Value) Then
                    'Previous Log Destination Date Time should not greater than current log child source Date Time
                    If CDate(mPrevLogDetail.DesUniverseDateTime) > CDate(mLogDetail.SouUniverseDateTime) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else 'Log Detail Child already exists

                If (Not mLogDetail.DesUniverseDateTime Is DBNull.Value) And (Not mLogDetail.SouUniverseDateTime Is DBNull.Value) Then
                    If mLog.LogDetails.Contains(mLogDetail.SouUniverseDateTime.ToString, mLogDetail.DesUniverseDateTime.ToString, mLogDetail.ID) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            End If
        Else 'First Log 
            'For first child
            If mLog.LogDetails.Count = 0 Then
                'Both Dates required
                If (Not mPrevLogDetail.DesUniverseDateTime Is DBNull.Value) And (Not mLogDetail.SouUniverseDateTime Is DBNull.Value) Then
                    'Previous Log Destination Date Time should not greater than current log child source Date Time
                    If CDate(mPrevLogDetail.DesUniverseDateTime) > CDate(mLogDetail.SouUniverseDateTime) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else 'Log Detail Child already exists

                If (Not mLogDetail.DesUniverseDateTime Is DBNull.Value) And (Not mLogDetail.SouUniverseDateTime Is DBNull.Value) Then
                    If mLog.LogDetails.Contains(mLogDetail.SouUniverseDateTime.ToString, mLogDetail.DesUniverseDateTime.ToString, mLogDetail.ID) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            End If
        End If
    End Function
    Private Sub RemoveRecord(ByVal ID As Guid)
        mLog.LogDetails.Remove(ID)

    End Sub
    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
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

    Private Sub AddAttributes()

        Try

            txtLandings.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")
            txtFuelUplifted.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")
            txtFuelOnArrival.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")
            txtCargoWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")
            txtTakeOffWeight.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLogPageNo').value,event)")

            'Added by Saylee on 5-Apr-2023 *******************
            txtFuelOnAdded.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtFuelOnAdded').value,event)")
            txtPaxAdult.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPaxAdult').value,event)")
            txtPaxChild.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPaxChild').value,event)")
            txtPaxInfant.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtPaxInfant').value,event)")
            txtLHEngineOil.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtLHEngineOil').value,event)")
            txtRHEngineOil.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtRHEngineOil').value,event)")
            '' '***********************

            txtExtraBaggage.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtExtraBaggage').value,event)")

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
    Public Function AvgFlightTimeDeviation(ByVal mLogDetail As LogDetail) As Boolean
        If AppSettings("NoOfLogsToConsiderForAvgFlightTime") <> "0" And AppSettings("DeviationInAvgFlightTimeInPercentage") <> "0" Then
            Dim NoOfLogsToConsiderForAvgFlightTime As Integer = (CType(AppSettings("NoOfLogsToConsiderForAvgFlightTime"), Integer))
            Dim Cnt As Integer = 0
            Dim AvgFlightTime As Decimal = 0
            Dim TLPCountWithSameSectorWithinCurrentLog As Integer = 0

            For Each LogDet As LogDetail In mLog.LogDetails
                If mLogDetail.SourceID.Equals(LogDet.SourceID) And mLogDetail.DestinationID.Equals(LogDet.DestinationID) And Not LogDet.ID.Equals(mLogDetail.ID) Then
                    TLPCountWithSameSectorWithinCurrentLog += 1
                End If
            Next

            If (NoOfLogsToConsiderForAvgFlightTime - TLPCountWithSameSectorWithinCurrentLog) > 0 Then
                Dim mLastLogDetails As LastLogDetails = LastLogDetails.GetLastLogDetails(True, IIf(mMachine.IsUTC, mLogDetail.DesUniverseDateTimeFormatted.ToString, mLogDetail.DesLocalDateTimeFormatted.ToString), (NoOfLogsToConsiderForAvgFlightTime - TLPCountWithSameSectorWithinCurrentLog), mLogDetail.SourceID.ToString, mLogDetail.DestinationID.ToString, mMachine.AssemblyStatus.Assembly.ModelID.ToString)

                If mLastLogDetails.Count > 0 Or TLPCountWithSameSectorWithinCurrentLog > 0 Then
                    'For i As Integer = 0 To TLPCountWithSameSectorWithinCurrentLog - 1
                    For j As Integer = mLog.LogDetails.Count - 1 To 0 Step -1
                        If mLogDetail.SourceID.Equals(mLog.LogDetails(j).SourceID) And mLogDetail.DestinationID.Equals(mLog.LogDetails(j).DestinationID) And Not mLog.LogDetails(j).ID.Equals(mLogDetail.ID) Then
                            AvgFlightTime += New Period(1, mLog.LogDetails(j).TimeInAir, 0, False, False).DbValueDec
                            Cnt += 1
                            If Cnt = TLPCountWithSameSectorWithinCurrentLog Then
                                Exit For
                            End If
                        End If
                    Next
                    'Next
                    AvgFlightTime += (mLastLogDetails.AvgFlightTime * mLastLogDetails.Count)

                    AvgFlightTime = AvgFlightTime / (mLastLogDetails.Count + TLPCountWithSameSectorWithinCurrentLog)

                    Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, mLogDetail.TimeInAir, 0, False, False).DbValueDec
                    Dim AllowedDeviationInDec = (AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
                    Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - AvgFlightTime)
                    If ActualDeviationInDec > AllowedDeviationInDec Then
                        If CurrentLogTimeInAirInDec > AvgFlightTime Then
                            Session("IsFlightTimeGreaterThanAvgFlightTime") = "True"
                        End If
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            ElseIf (NoOfLogsToConsiderForAvgFlightTime - TLPCountWithSameSectorWithinCurrentLog) <= 0 Then
                'For i As Integer = 0 To NoOfLogsToConsiderForAvgFlightTime - 1
                For j As Integer = mLog.LogDetails.Count - 1 To 0 Step -1
                    If mLogDetail.SourceID.Equals(mLog.LogDetails(j).SourceID) And mLogDetail.DestinationID.Equals(mLog.LogDetails(j).DestinationID) And Not mLog.LogDetails(j).ID.Equals(mLogDetail.ID) Then
                        AvgFlightTime += New Period(1, mLog.LogDetails(j).TimeInAir, 0, False, False).DbValueDec
                        Cnt += 1
                        If Cnt = TLPCountWithSameSectorWithinCurrentLog Then
                            Exit For
                        End If
                    End If
                Next
                'Next
                AvgFlightTime = AvgFlightTime / NoOfLogsToConsiderForAvgFlightTime
                Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, mLogDetail.TimeInAir, 0, False, False).DbValueDec
                Dim AllowedDeviationInDec = (AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
                Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - AvgFlightTime)

                If ActualDeviationInDec > AllowedDeviationInDec Then
                    If CurrentLogTimeInAirInDec > AvgFlightTime Then
                        Session("IsFlightTimeGreaterThanAvgFlightTime") = "True"
                    End If
                    Return True
                Else
                    Return False
                End If
                'ElseIf (NoOfLogsToConsiderForAvgFlightTime - TLPCountWithSameSectorWithinCurrentLog) < 0 Then
                '    For i As Integer = 0 To NoOfLogsToConsiderForAvgFlightTime - 1
                '        AvgFlightTime += New Period(1, mLog.LogDetails(mLog.LogDetails.Count - 1 - i).TimeInAir, 0, False, False).DbValueDec
                '    Next
                '    AvgFlightTime = AvgFlightTime / NoOfLogsToConsiderForAvgFlightTime
                '    Dim CurrentLogTimeInAirInDec As Decimal = New Period(1, mLogDetail.TimeInAir, 0, False, False).DbValueDec
                '    Dim AllowedDeviationInDec = (AvgFlightTime * CType(AppSettings("DeviationInAvgFlightTimeInPercentage"), Integer) / 100)
                '    Dim ActualDeviationInDec As Decimal = Math.Abs(CurrentLogTimeInAirInDec - AvgFlightTime)

                '    If ActualDeviationInDec > AllowedDeviationInDec Then
                '        If CurrentLogTimeInAirInDec > AvgFlightTime Then
                '            Session("IsFlightTimeGreaterThanAvgFlightTime") = "True"
                '        End If
                '        Return True
                '    Else
                '        Return False
                '    End If
            End If
        Else
            Return False
        End If
    End Function
    Private Sub SetCheckBoxStatus()
        If mLog.IsUTC Then
            ArrivalDate = chkUTCArrival.Checked
            Session("ArrivalDate") = ArrivalDate

            TouchDownDate = chkUTCTouchDown.Checked
            Session("TouchDownDate") = TouchDownDate

            TakeOffDate = chkUTCTakeOff.Checked
            Session("TakeOffDate") = TakeOffDate
        Else
            ArrivalDate = chkArrival.Checked
            Session("ArrivalDate") = ArrivalDate

            TouchDownDate = chkTouchDown.Checked
            Session("TouchDownDate") = TouchDownDate

            TakeOffDate = chkTakeOff.Checked
            Session("TakeOffDate") = TakeOffDate
        End If

    End Sub
#End Region

#Region "Data Binding"

    Private Sub GridColumnHeadingSet()
        dgLogTLPDetails.Columns(5).Visible = Not mLog.IsUTC
        dgLogTLPDetails.Columns(6).Visible = mLog.IsUTC
        dgLogTLPDetails.Columns(7).Visible = Not mLog.IsUTC
        dgLogTLPDetails.Columns(8).Visible = mLog.IsUTC

        dgLogTLPDetails.Columns(10).Visible = Not mLog.IsUTC
        dgLogTLPDetails.Columns(11).Visible = mLog.IsUTC
        dgLogTLPDetails.Columns(12).Visible = Not mLog.IsUTC
        dgLogTLPDetails.Columns(13).Visible = mLog.IsUTC

        'Remove link
        'Commented by Saylee on 31-Aug-2016, as always last entry will be removable
        'dgLogTLPDetails.Columns(23).Visible = mLog.IsNew



        'Commented by Saylee on 7-Jun-2023, for New Form conversion
        ''' dgLogTLPDetails.Columns(25).Visible = (mIsLastLog And mLog.LogDetails.Count > 1) Or mLog.IsNew
        For j As Integer = 0 To dgLogTLPDetails.Rows.Count - 1
            dgLogTLPDetails.Rows.Item(j).FindControl("DeleteRecord").Visible = (mIsLastLog And mLog.LogDetails.Count > 1) Or mLog.IsNew
        Next


    End Sub
    Private Sub DataFieldBind()

        calDateTime.Text = Format(CDate(mLog.Date), AppSettings("DateFormat"))

        If Not mLogDetail.SouLocalDateTime Is DBNull.Value Then
            'calDeparture.Text = Format(CDate(mLogDetail.SouLocalDateTime), AppSettings("DateTimeFormatLOG"))
            calDeparture.Text = Format(CDate(mLogDetail.SouLocalDateTime), AppSettings("DateFormat"))
            txtDepartureTime.Text = Format(CDate(mLogDetail.SouLocalDateTime), AppSettings("TimeFormat"))
        Else
            calDeparture.Text = ""
            txtDepartureTime.Text = ""
        End If

        If Not mLogDetail.DesLocalDateTime Is DBNull.Value Then
            'calArrival.Text = Format(CDate(mLogDetail.DesLocalDateTime), AppSettings("DateTimeFormatLOG"))
            calArrival.Text = Format(CDate(mLogDetail.DesLocalDateTime), AppSettings("DateFormat"))
            txtArrivalTime.Text = Format(CDate(mLogDetail.DesLocalDateTime), AppSettings("TimeFormat"))
        Else
            calArrival.Text = ""
            txtArrivalTime.Text = ""
        End If

        If Not mLogDetail.SouUniverseDateTime Is DBNull.Value Then
            'CalUTCDateTime.Text = Format(CDate(mLogDetail.SouUniverseDateTime), AppSettings("DateTimeFormatLOG"))
            CalUTCDateTime.Text = Format(CDate(mLogDetail.SouUniverseDateTime), AppSettings("DateFormat"))
            txtUTCDepartureTime.Text = Format(CDate(mLogDetail.SouUniverseDateTime), AppSettings("TimeFormat"))
        Else
            CalUTCDateTime.Text = ""
            txtUTCDepartureTime.Text = ""
        End If

        If Not mLogDetail.DesUniverseDateTime Is DBNull.Value Then
            'CalUTCArrival.Text = Format(CDate(mLogDetail.DesUniverseDateTime), AppSettings("DateTimeFormatLOG"))
            CalUTCArrival.Text = Format(CDate(mLogDetail.DesUniverseDateTime), AppSettings("DateFormat"))
            txtUTCArrivalTime.Text = Format(CDate(mLogDetail.DesUniverseDateTime), AppSettings("TimeFormat"))
        Else
            CalUTCArrival.Text = ""
            txtUTCArrivalTime.Text = ""
        End If

        If Not mLogDetail.TakeOffLocalDateTime Is DBNull.Value Then
            'calTakeOffLocalDateTime.Text = Format(CDate(mLogDetail.TakeOffLocalDateTime), AppSettings("DateTimeFormatLOG"))
            calTakeOffLocalDateTime.Text = Format(CDate(mLogDetail.TakeOffLocalDateTime), AppSettings("DateFormat"))
            txtTakeOffLocalTime.Text = Format(CDate(mLogDetail.TakeOffLocalDateTime), AppSettings("TimeFormat"))
        Else
            calTakeOffLocalDateTime.Text = ""
            txtTakeOffLocalTime.Text = ""
        End If


        If Not mLogDetail.TakeOffUniverseDateTime Is DBNull.Value Then
            ' calUTCTakeOffDateTime.Text = Format(CDate(mLogDetail.TakeOffUniverseDateTime), AppSettings("DateTimeFormatLOG"))
            calUTCTakeOffDateTime.Text = Format(CDate(mLogDetail.TakeOffUniverseDateTime), AppSettings("DateFormat"))
            txtUTCTakeOffTime.Text = Format(CDate(mLogDetail.TakeOffUniverseDateTime), AppSettings("TimeFormat"))
        Else
            calUTCTakeOffDateTime.Text = ""
            txtUTCTakeOffTime.Text = ""
        End If

        If Not mLogDetail.TouchDownLocalDateTime Is DBNull.Value Then
            'calTouchDownLocalDateTime.Text = Format(CDate(mLogDetail.TouchDownLocalDateTime), AppSettings("DateTimeFormatLOG"))
            calTouchDownLocalDateTime.Text = Format(CDate(mLogDetail.TouchDownLocalDateTime), AppSettings("DateFormat"))
            txtTouchDownLocalTime.Text = Format(CDate(mLogDetail.TouchDownLocalDateTime), AppSettings("TimeFormat"))
        Else
            calTouchDownLocalDateTime.Text = ""
            txtTouchDownLocalTime.Text = ""
        End If

        If Not mLogDetail.TouchDownUniverseDateTime Is DBNull.Value Then
            'calUTCTouchDownDateTime.Text = Format(CDate(mLogDetail.TouchDownUniverseDateTime), AppSettings("DateTimeFormatLOG"))
            calUTCTouchDownDateTime.Text = Format(CDate(mLogDetail.TouchDownUniverseDateTime), AppSettings("DateFormat"))
            txtUTCTouchDownTime.Text = Format(CDate(mLogDetail.TouchDownUniverseDateTime), AppSettings("TimeFormat"))
        Else
            calUTCTouchDownDateTime.Text = ""
            txtUTCTouchDownTime.Text = ""

        End If
        txtEmployee.Text = mLogDetail.PFIDoneByEmpNoName 'Added by vikrant on 14-Feb-2019 For ALL14022019
        'txtBlockTime.Text = mLogDetail.BlockTime
        'txtAirBorneTime.Text = mLogDetail.TimeInAir
        'txtLandings.Text = mLogDetail.Landings


        DataBindGrid()

        mSearchListPlace = SearchList.GetSearchList("Place", "", "")
        Session("mSearchListPlace") = mSearchListPlace

        DataBind()
        GridColumnHeadingSet()
        upnlDetails.Update()

    End Sub

    Private Sub DataBindGrid()
        dgLogTLPDetails.DataSource = mLog.LogDetails
        dgLogTLPDetails.DataBind()

        upnlGrid.Update()

    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        GridColumnHeadingSet()
        Dim tempString As String
        If custValidator.ControlToValidate = "Place1" Then

            tempString = Place1.Text.Trim
            If Not tempString = String.Empty Then

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
        ElseIf custValidator.ControlToValidate = "Place2" Then
            tempString = Place2.Text.Trim
            If Not tempString = String.Empty Then
                If tempString.IndexOf("[") < 0 Then
					'custValidator.ErrorMessage = "Enter correct Destination name."
					'e.IsValid = False
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
            'Added by vikrant on 14-Feb-2019 For ALL14022019
        ElseIf custValidator.ControlToValidate = "txtEmployee" Then


        End If
        'End
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        GridColumnHeadingSet()
        Dim str As String = ""
        'Log
        If Not mLogDetail.IsValid Then
            For i As Integer = 0 To mLogDetail.GetBrokenRulesCollection.Count - 1
                str = str + mLogDetail.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean    'For DgLog Fuel Oils
        Dim str As String = ""
        If Not mLogDetail.IsValid Then
            For i As Integer = 0 To mLogDetail.GetBrokenRulesCollection.Count - 1
                str = str + mLogDetail.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        If str <> "" Then
            CvTime.ErrorMessage = str
            CvTime.IsValid = False
            Return False
        End If
        Return True
    End Function
    Public Function CustomValidatePFIDoneBy() As Boolean
        Dim str As String = ""
        If AppSettings("ClientCode") = "Novo" Then
            For i As Integer = 0 To mLog.LogDetails.Count - 1
                If Not mLog.LogDetails(i).IsPFIDone Then
                    str = str + "Please check PFI Done checkbox and enter PFI Done By Employee."
                    Exit For
                Else
                    If mLog.LogDetails(i).PFIDoneByID.Equals(Guid.Empty) Then
                        str = str + "Enter PFI Done By Employee."
                        Exit For
                    End If
                End If
            Next
        Else
            For i As Integer = 0 To mLog.LogDetails.Count - 1
                If mLog.LogDetails(i).IsPFIDone And mLog.LogDetails(i).PFIDoneByID.Equals(Guid.Empty) Then
                    str = str + "Enter PFI Done By Employee."
                    Exit For
                End If
            Next
        End If
        If str <> "" Then
            CvTime.ErrorMessage = str
            CvTime.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        GetSession()
        AddAttributes()
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            DataFieldBind()
            txtFlightNo.Focus()
        End If
        EnableDisableButton(mIsLastLogTLP)
        ControlVisibility(mIsLastLogTLP)
        ' '' '''MessageBoxResult()
        SetTitle()
        SetTakeoffTouchdownTitle()
        SetFromAutoComplete()
        SetCheckBoxStatus()
        mLogDetail.FlightNo = txtFlightNo.Text.Trim
    End Sub
    Protected Sub btnAddPlaces_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnAddPlaces.Click
        SetObject()
        ' Response.Redirect("wfPlace_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&BackPage1=wfTLPDetail_Ajax.aspx")
        Dim bkpage As String = CType(Request.QueryString("BackPage"), String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenPlaceWindow(bkpage)", "OpenPlaceWindow();", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not CustomValidatePFIDoneBy() Then
            upnlErrorList.Update()
            Exit Sub
        End If
        If mLogDetail.IsDirty Then
            ' '' ''Dim msg1 As New SIMsgBox(Page, "Close Confirmation!", "<b> Do you really want to close ? </b> <BR> <BR> Click Yes to close current screen,Click No to remain on same screen. ", "", MsgBoxStyle.YesNo)
            ' '' ''msg1.ReplacePage = "wfTLPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            ' '' ''Session("sender") = "Close"
            ' '' ''msg1.Show()
            DataBindGrid()
            MSGBoxCtrl.Show("Close Confirmation!", "<b> Do you really want to close ? </b> <BR> <BR> Click Yes to close current screen,Click No to remain on same screen. ", "", MsgBoxStyle.YesNo, "Close")

        Else
            RemoveSession()
            SetLogObject()
            Response.Redirect(Request.QueryString("BackPage") & "?BackPage=Index.aspx")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("LogPrint")) Then
            ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            ' '' ''msg.ReplacePage = "wfTLPDetail.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
            ' '' ''msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")

            Exit Sub
        End If
    End Sub

    Private Sub calArrival_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calArrival.TextChanged
        If IsPostBack Then

            If Trim(calArrival.Text) = "" Then
                ViewState("calArrival") = calDateTime.Text.Trim
                Exit Sub
            End If

            '# Date Control Validation #

            Try
                Dim tempdate As DateTime
                Dim Datestring As String = Format(CDate(calArrival.Text.Trim), AppSettings("DateFormat"))
                tempdate = Date.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("calArrival") Is Nothing Then
                        calArrival.Text = Format(CDate(ViewState("calArrival")), AppSettings("DateFormat"))
                    Else
                        calArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If

                Else
                    calArrival.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calArrival") = calArrival.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calArrival") Is Nothing Then
                    calArrival.Text = Format(CDate(ViewState("calArrival")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
                calArrival_TextChanged(calArrival.Text, e)  'Raising textchange event for further calculation
                Exit Sub
            End Try

            '# End

            calTouchDownLocalDateTime.Text = calArrival.Text
            ViewState("calTouchDownLocalDateTime") = calArrival.Text.Trim

            Dim DateTimeString As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.DesLocalDateTime.ToString), New SmartDate(DateTimeString).Date) <> 0 Then
                mLogDetail.DesLocalDateTime = DateTimeString.Trim
                mLogDetail.TouchDownLocalDateTime = DateTimeString.Trim
                calUTCTouchDownDateTime.Text = Format(CDate(mLogDetail.TouchDownUniverseDateTime), AppSettings("DateFormat"))
            End If
            CalUTCArrival.Text = Format(CDate(mLogDetail.DesUniverseDateTime), AppSettings("DateFormat"))
            txtArrivalTime.Text = Format(CDate(mLogDetail.DesUniverseDateTime), AppSettings("TimeFormat"))
        End If

        If Not IsValid Then Exit Sub
        txtAirBorneTime.DataBind()
        SetObject()
        Session("mLogDetail") = mLogDetail
        DataBind()
        ' '' ''Response.Redirect("wfTLPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub calDeparture_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calDeparture.TextChanged
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
                    If Not ViewState("CalDeparture") Is Nothing Then
                        calDeparture.Text = Format(CDate(ViewState("CalDeparture")), AppSettings("DateFormat"))
                    Else
                        calDeparture.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    calDeparture.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("CalDeparture") = calDeparture.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("CalDeparture") Is Nothing Then
                    calDeparture.Text = Format(CDate(ViewState("CalDeparture")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calDeparture.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
                calDeparture_TextChanged(calDeparture.Text, e)  'Raising textchange event for further calculation
                Exit Sub
            End Try

            '# End

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.SouLocalDateTime.ToString), New SmartDate(calDeparture.Text.ToString).Date) <> 0 Then
                REM: Clone the object
                Dim clnLogDetail As LogDetail
                clnLogDetail = CType(mLogDetail.Clone, LogDetail)

                clnLogDetail.SouLocalDateTime = calDeparture.Text.ToString.Trim

                'If mLog.IsNew Then
                '    NewRecord(calDateTime.Text.ToString, calDeparture.Text.ToString)
                'Else
                '    EditRecord(New SmartDate(calDeparture.Text.ToString).Date)
                'End If

                REM: Copy from Clone
                'CopyFromClone(clnLog)

                mLogDetail.SouLocalDateTime = calDeparture.Text.ToString.Trim
                mLogDetail.TakeOffLocalDateTime = calDeparture.Text.ToString.Trim

                mLogDetail.DesLocalDateTime = calDeparture.Text.ToString.Trim
                mLogDetail.TouchDownLocalDateTime = calDeparture.Text.ToString.Trim

                DataFieldBind()
            End If
            ViewState("calTakeOffLocalDateTime") = calDeparture.Text.Trim
            ViewState("calTouchDownLocalDateTime") = calDeparture.Text.Trim
            ViewState("calArrival") = calDeparture.Text.Trim

            If Not IsValid Then Exit Sub
            If calDeparture.Text.ToString = "" Then
                mLogDetail.SouLocalDateTime = ""
                mLogDetail.DesLocalDateTime = ""
                calArrival.Enabled = False
                calArrival.ReadOnly = True
                calArrival.BackColor = Color.Gainsboro
                txtAirBorneTime.ReadOnly = True
                txtAirBorneTime.BackColor = Color.Gainsboro
            Else
                calArrival.Enabled = True
            End If
            SetObject()
            Session("mLogDetail") = mLogDetail
            ControlVisibility(mIsLastLogTLP)
            EnableDisableButton(mIsLastLogTLP)
            DataBind()
            ' '' ''Response.Redirect("wfTLPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))

        End If
    End Sub
    Private Sub CalUTCArrival_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalUTCArrival.TextChanged
        If IsPostBack Then

            If Trim(CalUTCArrival.Text) = "" Then
                ViewState("CalUTCArrival") = calDateTime.Text.Trim
                Exit Sub
            End If

            '# Date Control Validation #

            Try
                Dim tempdate As DateTime
                'Dim Datestring As String = CalUTCArrival.Text.Trim
                Dim Datestring As String = Format(CDate(CalUTCArrival.Text.Trim), AppSettings("DateFormat"))
                tempdate = Date.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("CalUTCArrival") Is Nothing Then
                        CalUTCArrival.Text = Format(CDate(ViewState("CalUTCArrival")), AppSettings("DateFormat"))
                    Else
                        CalUTCArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    CalUTCArrival.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("CalUTCArrival") = CalUTCArrival.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("CalUTCArrival") Is Nothing Then
                    CalUTCArrival.Text = Format(CDate(ViewState("CalUTCArrival")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    CalUTCArrival.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
                CalUTCArrival_TextChanged(CalUTCArrival.Text, e)  'Raising textchange event for further calculation
                Exit Sub
            End Try

            '# End

            calUTCTouchDownDateTime.Text = CalUTCArrival.Text
            ViewState("calUTCTakeOffDateTime") = CalUTCArrival.Text.Trim

            Dim DateTimeString As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.DesUniverseDateTime.ToString), New SmartDate(DateTimeString).Date) <> 0 Then
                mLogDetail.DesUniverseDateTime = DateTimeString.Trim
                mLogDetail.TouchDownUniverseDateTime = DateTimeString.Trim
            End If
        End If

        If Not IsValid Then Exit Sub
        txtAirBorneTime.DataBind()
        SetObject()
        Session("mLogDetail") = mLogDetail
        DataBind()

    End Sub
    Private Sub CalUTCDateTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalUTCDateTime.TextChanged
        If IsPostBack Then
            If Trim(CalUTCDateTime.Text) = "" Then
                ViewState("CalUTCDateTime") = calDateTime.Text.Trim
                Exit Sub
            End If

            '# Date Control Validation # 

            Try
                Dim tempdate As DateTime
                Dim Datestring As String = Format(CDate(CalUTCDateTime.Text.Trim), AppSettings("DateFormat"))
                tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("CalUTCDateTime") Is Nothing Then
                        CalUTCDateTime.Text = Format(CDate(ViewState("CalUTCDateTime")), AppSettings("DateFormat"))
                    Else
                        CalUTCDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    CalUTCDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("CalUTCDateTime") = CalUTCDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("CalUTCDateTime") Is Nothing Then
                    CalUTCDateTime.Text = Format(CDate(ViewState("CalUTCDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    CalUTCDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
                CalUTCDateTime_TextChanged(CalUTCDateTime.Text, e)  'Raising textchange event for further calculation
                Exit Sub
            End Try

            '# End

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.SouUniverseDateTime.ToString), New SmartDate(CalUTCDateTime.Text.ToString).Date) <> 0 Then
                REM: Clone the object
                Dim clnLogDetail As LogDetail
                clnLogDetail = CType(mLogDetail.Clone, LogDetail)

                clnLogDetail.SouUniverseDateTime = CalUTCDateTime.Text.ToString.Trim

                'If mLog.IsNew Then
                '    'CNDC
                '    'NewRecord(calDateTime.Text, , CalUTCDateTime.Text)
                '    NewRecord(calDateTime.Text.ToString, , CalUTCDateTime.Text.ToString)
                'Else
                '    'CNDC
                '    'EditRecord(SmartDate.StringToDate(CalUTCDateTime.Text))
                '    EditRecord(New SmartDate(CalUTCDateTime.Text.ToString).Date)
                'End If
                REM: Copy from Clone

                'CopyFromClone(clnLogDetail)
                mLogDetail.SouUniverseDateTime = CalUTCDateTime.Text.ToString.Trim
                mLogDetail.TakeOffUniverseDateTime = CalUTCDateTime.Text.ToString.Trim

                mLogDetail.DesUniverseDateTime = CalUTCDateTime.Text.ToString.Trim
                mLogDetail.TouchDownUniverseDateTime = CalUTCDateTime.Text.ToString.Trim
                DataFieldBind()
            End If
            ViewState("calUTCTakeOffDateTime") = CalUTCDateTime.Text.Trim
            ViewState("calUTCTouchDownDateTime") = CalUTCDateTime.Text.Trim
            ViewState("CalUTCArrival") = CalUTCDateTime.Text.Trim
        End If

        If Not IsValid Then Exit Sub
        If calDeparture.Text.ToString = "" Then
            mLogDetail.SouLocalDateTime = ""
            mLogDetail.DesLocalDateTime = ""
            calArrival.Enabled = False
            calArrival.ReadOnly = True
            calArrival.BackColor = Color.Gainsboro
            txtAirBorneTime.ReadOnly = True
            txtAirBorneTime.BackColor = Color.Gainsboro
        Else
            calArrival.Enabled = True
        End If
        SetObject()
        Session("mLogDetail") = mLogDetail
        ControlVisibility(mIsLastLogTLP)
        EnableDisableButton(mIsLastLogTLP)
        DataBind()

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
                Dim Datestring As String = Format(CDate(calTakeOffLocalDateTime.Text.Trim), AppSettings("DateFormat"))
                tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("calTakeOffLocalDateTime") Is Nothing Then
                        calTakeOffLocalDateTime.Text = Format(CDate(ViewState("calTakeOffLocalDateTime")), AppSettings("DateFormat"))
                    Else
                        calTakeOffLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    calTakeOffLocalDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calTakeOffLocalDateTime") = calTakeOffLocalDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calTakeOffLocalDateTime") Is Nothing Then
                    calTakeOffLocalDateTime.Text = Format(CDate(ViewState("calTakeOffLocalDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calTakeOffLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
            End Try

            '# End

        End If

        If Not IsValid Then Exit Sub
        If calTakeOffLocalDateTime.Text.ToString = "" Then
            mLogDetail.TakeOffLocalDateTime = ""
            mLogDetail.TouchDownLocalDateTime = ""
        End If
        SetObject()
        Session("mLogDetail") = mLogDetail
        ControlVisibility(mIsLastLogTLP)
        EnableDisableButton(mIsLastLogTLP)
        DataBind()
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
                Dim Datestring As String = Format(CDate(calTouchDownLocalDateTime.Text.Trim), AppSettings("DateFormat"))
                tempdate = Date.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("calTouchDownLocalDateTime") Is Nothing Then
                        calTouchDownLocalDateTime.Text = Format(CDate(ViewState("calTouchDownLocalDateTime")), AppSettings("DateFormat"))
                    Else
                        calTouchDownLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    calTouchDownLocalDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calTouchDownLocalDateTime") = calTouchDownLocalDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calTouchDownLocalDateTime") Is Nothing Then
                    calTouchDownLocalDateTime.Text = Format(CDate(ViewState("calTouchDownLocalDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calTouchDownLocalDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
            End Try
            '# End
        End If

        If Not IsValid Then Exit Sub
        txtAirBorneTime.DataBind()
        SetObject()
        Session("mLogDetail") = mLogDetail
        ControlVisibility(mIsLastLogTLP)
        EnableDisableButton(mIsLastLogTLP)
        DataBind()
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
                    If Not ViewState("calUTCTakeOffDateTime") Is Nothing Then
                        calUTCTakeOffDateTime.Text = Format(CDate(ViewState("calUTCTakeOffDateTime")), AppSettings("DateFormat"))
                    Else
                        calUTCTakeOffDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    calUTCTakeOffDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calUTCTakeOffDateTime") = calUTCTakeOffDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calUTCTakeOffDateTime") Is Nothing Then
                    calUTCTakeOffDateTime.Text = Format(CDate(ViewState("calUTCTakeOffDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calUTCTakeOffDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
            End Try

            '# End
        End If

        If Not IsValid Then Exit Sub
        SetObject()
        Session("mLogDetail") = mLogDetail
        ControlVisibility(mIsLastLogTLP)
        EnableDisableButton(mIsLastLogTLP)
        DataBind()
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
                Dim Datestring As String = Format(CDate(calUTCTouchDownDateTime.Text.Trim), AppSettings("DateFormat"))

                tempdate = DateTime.ParseExact(Datestring, AppSettings("DateFormat"), System.Globalization.CultureInfo.InvariantCulture).ToString()
                If tempdate.Year < 1753 Then     'Date should not be less than 1/1/1753(Sql Server MinDate)
                    If Not ViewState("calUTCTouchDownDateTime") Is Nothing Then
                        calUTCTouchDownDateTime.Text = Format(CDate(ViewState("calUTCTouchDownDateTime")), AppSettings("DateFormat"))
                    Else
                        calUTCTouchDownDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))
                    End If
                Else
                    calUTCTouchDownDateTime.Text = Format(tempdate, AppSettings("DateFormat"))
                End If
                ViewState("calUTCTouchDownDateTime") = calUTCTouchDownDateTime.Text.Trim  'Storing Current DateValue to ViewState for Date correction
            Catch ex As Exception
                If Not ViewState("calUTCTouchDownDateTime") Is Nothing Then
                    calUTCTouchDownDateTime.Text = Format(CDate(ViewState("calUTCTouchDownDateTime")), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                Else
                    calUTCTouchDownDateTime.Text = Format(CDate(calDateTime.Text.Trim), AppSettings("DateFormat"))   'Format(DateTime.Parse(Datestring), AppSettings("DateTimeFormatLOG"))
                End If
            End Try
            '# End


            calUTCTouchDownDateTime.Text = Format(CDate(calUTCTouchDownDateTime.Text.ToString.Trim), AppSettings("DateFormat")) 'Added By Utkarsh On 30-Aug-2011
        End If

        If Not IsValid Then Exit Sub
        txtAirBorneTime.DataBind()
        SetObject()
        Session("mLogDetail") = mLogDetail
        ControlVisibility(mIsLastLogTLP)
        EnableDisableButton(mIsLastLogTLP)
        DataBind()

    End Sub

	Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
		Dim isValidForCVA As Boolean = True
		If IsValid Then
			'Sankalp 28-10-25 Check for Flight No duplicate
			If AppSettings("ClientCode") = "CVA" Then
				isValidForCVA = ValidateForCVA(FlightNoText:=txtFlightNo.Text.Trim)
			End If
			If isValidForCVA = True Then
				SetObject()
				'If Not CustomValidate2() Then Exit Sub
				If mLogDetail.IsValid Then
					If IsLogValid() = True Then
						'Added By Vikrant On 30-Nov-2016 For ALL30112016-1
						If AvgFlightTimeDeviation(mLogDetail) = True Then
							MSGBoxCtrl.Show(MSGBox.Message_Title.SaveAlert, MSGBox.Message_Text.Alert, "Airborne Time of this flight is " & IIf(Session("IsFlightTimeGreaterThanAvgFlightTime") = "True", "greater", "less") & " than the Average Flight Time for this current sector .<br> <br> Do you still want to Save Log? ", MsgBoxStyle.YesNo, "SaveLogAfterAvgFlightTimeDeviationWarning")
							Session.Remove("IsFlightTimeGreaterThanAvgFlightTime")
							Exit Sub
						Else 'End
							AddTLP()
							SetLogObject()
							btnBack.Focus()
						End If
					Else
						MSGBoxCtrl.Show("Log Save Alert !", "<b>Invalid Log Date and Time.</b> <BR>  <BR> Current Log Date and Time should be greater than previous Log Date and Time.", "", MsgBoxStyle.OkOnly, "")

						'Added by Saylee on 29-Apr-2022
						If mLogDetail.IsNew Then

						Else
							Dim clnLogDetail As LogDetail
							Dim clnLogTLP As Log
							clnLogDetail = Session("clnLogDetail")
							clnLogTLP = Session("clnLogTLP")
							If Not clnLogDetail Is Nothing And Not clnLogTLP Is Nothing Then
								'' mLogDetail = clnLogDetail
								mLog = clnLogTLP
								Session("mLog") = mLog
								DataFieldBind()
								upnlGrid.Update()
							End If
						End If
						'**************************************************


					End If
				Else
					upnlErrorList.Update()
				End If
			Else
				MSGBoxCtrl.Show("Log Save Alert !", "<b>Duplicate Flight No.</b> <BR>  <BR> Current Flight No. should not be same as existing Flight No.", "", MsgBoxStyle.OkOnly, "")
			End If
		Else
			upnlErrorList.Update()
		End If
	End Sub

	Private Sub dgLogTLPDetails_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgLogTLPDetails.RowCommand

        'Dim Index As Int32 = e.Item.ItemIndex + dgLogTLPDetails.CurrentPageIndex * dgLogTLPDetails.PageSize
        Select Case e.CommandName
            Case "EditRec"
                Dim Index As Integer = CInt(e.CommandArgument)
                Dim ID As Guid = mLog.LogDetails(Index - 1).ID
                Session("LogDetailEdit") = True
                EditRecord(ID, Index)
            Case "RemoveRec"
                'Dim ID As Guid = New Guid(e.Item.Cells(0).Text)
                Dim Index As Integer = CInt(e.CommandArgument)
                Dim ID As Guid = mLog.LogDetails(Index - 1).ID
                If Index <> mLog.LogDetails.Count Then 'This is Not Last Record...
                    ' '' ''Dim msg As New SIMsgBox(Page, "Remove Alert !", "<b>You can not remove this record.</b><BR><BR>Selected record is not last record.", "", MsgBoxStyle.OkOnly)
                    ' '' ''msg.ReplacePage = "wfTLPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''msg.Show()
                    MSGBoxCtrl.Show("Remove Alert !", "<b>You can not remove this record.</b><BR><BR>Selected record is not last record.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    ' '' ''Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
                    ' '' ''msg.ReplacePage = "wfTLPDetail.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
                    ' '' ''Session("sender") = "Remove"
                    ' '' ''msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Remove")
                    Session("Index") = Index
                    mLog.LogDetails.CurrentIndex = Index - 1
                    Session("mLog") = mLog
                End If
        End Select
    End Sub
    Private Sub dgLogTLPDetails_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgLogTLPDetails.Sorting
        mLog.LogDetails.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mLog") = mLog
        DataBindGrid()
    End Sub

    Private Sub txtDepartureTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDepartureTime.TextChanged

        If IsValidTime(txtDepartureTime.Text.ToString.Trim) = False Then
            txtDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calDeparture.Text.ToString + " " + txtDepartureTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.SouLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

                mLogDetail.SouLocalDateTime = DateTime
                mLogDetail.TakeOffLocalDateTime = DateTime

                mLogDetail.DesLocalDateTime = DateTime
                mLogDetail.TouchDownLocalDateTime = DateTime

                DataFieldBind()
            End If
        End If
    End Sub
    Private Sub txtUTCDepartureTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUTCDepartureTime.TextChanged

        If IsValidTime(txtUTCDepartureTime.Text.ToString.Trim) = False Then
            txtUTCDepartureTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = CalUTCDateTime.Text.ToString + " " + txtUTCDepartureTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.SouUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then

                mLogDetail.SouUniverseDateTime = DateTime
                mLogDetail.TakeOffUniverseDateTime = DateTime

                mLogDetail.DesUniverseDateTime = DateTime
                mLogDetail.TouchDownUniverseDateTime = DateTime
                DataFieldBind()
            End If
        End If

        txtUTCTakeOffTime.Focus()
        'Dim tb As TextBox = CType(FormOrder.FindControl("txtUTCTakeOffTime"), TextBox)
        'tb.Focus()
    End Sub

    Private Sub txtTakeOffLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTakeOffLocalTime.TextChanged
        If IsValidTime(txtTakeOffLocalTime.Text.ToString.Trim) = False Then
            txtTakeOffLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calTakeOffLocalDateTime.Text.ToString.Trim + " " + txtTakeOffLocalTime.Text.ToString.Trim
            mLogDetail.TakeOffLocalDateTime = DateTime
            DataFieldBind()
        End If
    End Sub
    Private Sub txtUTCTakeOffTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTakeOffTime.TextChanged
        If IsValidTime(txtUTCTakeOffTime.Text.ToString.Trim) = False Then
            txtUTCTakeOffTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calUTCTakeOffDateTime.Text.ToString.Trim + " " + txtUTCTakeOffTime.Text.ToString.Trim
            mLogDetail.TakeOffUniverseDateTime = DateTime
            DataFieldBind()
        End If
        txtUTCTouchDownTime.Focus()
    End Sub

    Private Sub txtArrivalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtArrivalTime.TextChanged
        If IsValidTime(txtArrivalTime.Text.ToString.Trim) = False Then
            txtArrivalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else

            'calTouchDownLocalDateTime.Text = calArrival.Text.Trim
            'txtTouchDownLocalTime.Text = txtArrivalTime.Text.Trim

            Dim DateTime As String = calArrival.Text.ToString.Trim + " " + txtArrivalTime.Text.ToString.Trim

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.DesLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mLogDetail.DesLocalDateTime = DateTime
                ' mLogDetail.TouchDownLocalDateTime = DateTime
                DataFieldBind()
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
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.DesUniverseDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mLogDetail.DesUniverseDateTime = DateTime
                ' mLogDetail.TouchDownUniverseDateTime = DateTime
                DataFieldBind()
            End If
        End If
        txtBlockTime.Focus()
    End Sub

    Private Sub txtTouchDownLocalTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTouchDownLocalTime.TextChanged
        If IsValidTime(txtTouchDownLocalTime.Text.ToString.Trim) = False Then
            txtTouchDownLocalTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calTouchDownLocalDateTime.Text.ToString.Trim + " " + txtTouchDownLocalTime.Text.ToString.Trim
            mLogDetail.TouchDownLocalDateTime = DateTime
            DataFieldBind()

            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mLogDetail.DesLocalDateTime.ToString), New SmartDate(DateTime).Date) <> 0 Then
                ' mLogDetail.DesLocalDateTime = DateTime
                mLogDetail.TouchDownLocalDateTime = DateTime
                DataFieldBind()
            End If
        End If
    End Sub
    Private Sub txtUTCTouchDownTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUTCTouchDownTime.TextChanged
        If IsValidTime(txtUTCTouchDownTime.Text.ToString.Trim) = False Then
            txtUTCTouchDownTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = calUTCTouchDownDateTime.Text.ToString.Trim + " " + txtUTCTouchDownTime.Text.ToString.Trim
            mLogDetail.TouchDownUniverseDateTime = DateTime
            ' mLogDetail.DesUniverseDateTime = DateTime
            DataFieldBind()
        End If
        txtUTCArrivalTime.Focus()
    End Sub

    Private Sub chkArrival_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkArrival.CheckedChanged
        If chkArrival.Checked Then
            calArrival.ReadOnly = False
            calArrival.BackColor = Color.White
            calArrival_CalendarExtender.Enabled = True
        Else
            calArrival.ReadOnly = True
            calArrival.BackColor = Color.Gainsboro
            calArrival_CalendarExtender.Enabled = False
        End If
        ArrivalDate = chkArrival.Checked
        Session("ArrivalDate") = ArrivalDate
    End Sub
    Private Sub chkUTCArrival_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUTCArrival.CheckedChanged
        If chkUTCArrival.Checked Then
            CalUTCArrival.ReadOnly = False
            CalUTCArrival.BackColor = Color.White
            CalUTCArrival_CalendarExtender.Enabled = True
        Else
            CalUTCArrival.ReadOnly = True
            CalUTCArrival.BackColor = Color.Gainsboro
            CalUTCArrival_CalendarExtender.Enabled = False
        End If
        ArrivalDate = chkUTCArrival.Checked
        Session("ArrivalDate") = ArrivalDate
    End Sub
    Private Sub chkTouchDown_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTouchDown.CheckedChanged
        If chkTouchDown.Checked Then
            calTouchDownLocalDateTime.ReadOnly = False
            calTouchDownLocalDateTime.BackColor = Color.White
            calTouchDownLocalDateTime_CalendarExtender.Enabled = True
        Else
            calTouchDownLocalDateTime.ReadOnly = True
            calTouchDownLocalDateTime.BackColor = Color.Gainsboro
            calTouchDownLocalDateTime_CalendarExtender.Enabled = False
        End If
        TouchDownDate = chkTouchDown.Checked
        Session("TouchDownDate") = TouchDownDate
    End Sub
    Private Sub chkUTCTouchDown_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUTCTouchDown.CheckedChanged
        If chkUTCTouchDown.Checked Then
            calUTCTouchDownDateTime.ReadOnly = False
            calUTCTouchDownDateTime.BackColor = Color.White
            calUTCTouchDownDateTime_CalendarExtender.Enabled = True
        Else
            calUTCTouchDownDateTime.ReadOnly = True
            calUTCTouchDownDateTime.BackColor = Color.Gainsboro
            calUTCTouchDownDateTime_CalendarExtender.Enabled = False
        End If
        TouchDownDate = chkUTCTouchDown.Checked
        Session("TouchDownDate") = TouchDownDate
    End Sub

    Private Sub chkTakeOff_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTakeOff.CheckedChanged
        If chkTakeOff.Checked Then
            calTakeOffLocalDateTime.ReadOnly = False
            calTakeOffLocalDateTime.BackColor = Color.White
            calTakeOffLocalDateTime_CalendarExtender.Enabled = True
        Else
            calTakeOffLocalDateTime.ReadOnly = True
            calTakeOffLocalDateTime.BackColor = Color.Gainsboro
            calTakeOffLocalDateTime_CalendarExtender.Enabled = False
        End If
        TakeOffDate = chkTakeOff.Checked
        Session("TakeOffDate") = TakeOffDate
    End Sub
    Private Sub chkUTCTakeOff_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUTCTakeOff.CheckedChanged
        If chkUTCTakeOff.Checked Then
            calUTCTakeOffDateTime.ReadOnly = False
            calUTCTakeOffDateTime.BackColor = Color.White
            calUTCTakeOffDateTime_CalendarExtender.Enabled = True
        Else
            calUTCTakeOffDateTime.ReadOnly = True
            calUTCTakeOffDateTime.BackColor = Color.Gainsboro
            calUTCTakeOffDateTime_CalendarExtender.Enabled = False
        End If
        TakeOffDate = chkUTCTakeOff.Checked
        Session("TakeOffDate") = TakeOffDate
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Added by vikrant on 14-Feb-2019 For ALL14022019
    Protected Sub txtEmployee_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim message As String = ""
        Dim mEmployeeList As EmployeeList
        Dim mEmployeeStatus As EmployeeStatus

        mEmployeeList = EmployeeList.GetEmployeeList()

        If mEmployeeList.Contains(txtEmployee.Text) Then
            mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mEmployeeList(txtEmployee.Text, "").ID.ToString, mLog.DateFormatted.ToString)
            If mEmployeeStatus.Count > 0 Then
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.Custom, message, MsgBoxStyle.OkOnly, "ResetEmployee")
                    Exit Sub
                End If
                mLogDetail.PFIDoneByID = New Guid(mEmployeeList(txtEmployee.Text, "").ID.ToString)
                mLogDetail.PFIDoneByEmpNoName = txtEmployee.Text
                mLogDetail.PFIDoneByName = mEmployeeList(txtEmployee.Text, "").Name
                mLogDetail.PFIDoneByNo = mEmployeeList(txtEmployee.Text, "").EmpNo
            Else
                txtEmployee.Text = ""
                If Not mLogDetail.PFIDoneByID.Equals(Guid.Empty) Then
                    txtEmployee.Text = mLogDetail.PFIDoneByEmpNoName
                End If
            End If
        Else
            txtEmployee.Text = ""
            mLogDetail.PFIDoneByID = Guid.Empty
            mLogDetail.PFIDoneByEmpNoName = ""
            mLogDetail.PFIDoneByName = ""
            mLogDetail.PFIDoneByNo = ""
        End If
    End Sub
    Protected Sub chkIsPFI_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtEmployee.Text = ""
        mLogDetail.PFIDoneByID = Guid.Empty
        mLogDetail.PFIDoneByEmpNoName = ""
        mLogDetail.PFIDoneByName = ""
        mLogDetail.PFIDoneByNo = ""
    End Sub
    'End
    Private Sub txtFuelUplifted_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFuelUplifted.TextChanged, txtFuelOnDeparture.TextChanged, txtFuelOnArrival.TextChanged, txtTotalFuelOnDeparture.TextChanged
        mLogDetail = Session("mLogDetail")
        mLogDetail.FuelOnDeparture = Val(txtFuelOnDeparture.Text.Trim)
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
            mLogDetail.TotalFuelOnDeparture = Val(txtTotalFuelOnDeparture.Text.Trim)
            txtFuelOnAdded.Focus()
        Else
            mLogDetail.FuelUplifted = Val(txtFuelUplifted.Text.Trim)
            txtFuelUplifted.Focus()
        End If


        mLogDetail.FuelOnArrival = Val(txtFuelOnArrival.Text.Trim)
        txtTotalFuelOnDeparture.DataBind()
        txtFuelConsumption.DataBind()
        txtFuelUplifted.DataBind()
        txtFuelOnArrival.DataBind()
        Session("mLogDetail") = mLogDetail

    End Sub
    Private Sub txtFuelOnArrival_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFuelOnArrival.TextChanged
        mLogDetail = Session("mLogDetail")
        mLogDetail.FuelOnDeparture = Val(txtFuelOnDeparture.Text.Trim)
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
            mLogDetail.TotalFuelOnDeparture = Val(txtTotalFuelOnDeparture.Text.Trim)

        Else
            mLogDetail.FuelUplifted = Val(txtFuelUplifted.Text.Trim)

        End If


        mLogDetail.FuelOnArrival = Val(txtFuelOnArrival.Text.Trim)
        txtTotalFuelOnDeparture.DataBind()
        txtFuelConsumption.DataBind()
        txtFuelUplifted.DataBind()
        txtFuelOnArrival.DataBind()
        Session("mLogDetail") = mLogDetail
        btnAdd.Focus()
    End Sub
    Private Sub txtFuelOnDeparture_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTotalFuelOnDeparture.TextChanged
        mLogDetail = Session("mLogDetail")
        mLogDetail.FuelOnDeparture = Val(txtFuelOnDeparture.Text.Trim)
        If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
            mLogDetail.TotalFuelOnDeparture = Val(txtTotalFuelOnDeparture.Text.Trim)

        Else
            mLogDetail.FuelUplifted = Val(txtFuelUplifted.Text.Trim)

        End If


        mLogDetail.FuelOnArrival = Val(txtFuelOnArrival.Text.Trim)
        txtTotalFuelOnDeparture.DataBind()
        txtFuelConsumption.DataBind()
        txtFuelUplifted.DataBind()
        txtFuelOnArrival.DataBind()
        Session("mLogDetail") = mLogDetail
        txtFuelOnArrival.Focus()
    End Sub
	'Sankalp 28-10-25 Check for Flight No duplicate
	Private Function ValidateForCVA(ByVal FlightNoText As String) As Boolean
		If FlightNoText <> "" Then
			For j As Integer = 0 To dgLogTLPDetails.Rows.Count - 1
				Dim mFlightNo As String = dgLogTLPDetails.Rows.Item(j).Cells(2).Text.Trim()
				If String.Equals(mFlightNo, FlightNoText.Trim(), StringComparison.OrdinalIgnoreCase) Then
					Return False
				End If
			Next
			Return True
		Else
			Return True
		End If
	End Function
#End Region

#Region "Service Methods"
	'Added by vikrant on 14-Feb-2019 For ALL14022019
	<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmployeeList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim itemlist As EmpNoNameAutoComplete
        itemlist = EmpNoNameAutoComplete.GeEmpNoNameList(prefixText)
        If count = 0 Then
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).ToArray
        Else
            Return (From c As EmpNoNameAutoComplete.EmpListAutoCompleteInfo In itemlist
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.EmpNoName, c.ID.ToString())).Take(count).ToArray
        End If
    End Function

    'End
#End Region


End Class
